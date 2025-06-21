using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure.LLM;
using Microsoft.Extensions.Logging;

namespace DesktopOrganizer.App.Services;

/// <summary>
/// Main orchestration service for desktop organization workflow
/// </summary>
public class OrganizationService
{
    private readonly DesktopScanService _scanService;
    private readonly IPlanPreviewService _planService;
    private readonly IExecutionService _executionService;
    private readonly IUndoService _undoService;
    private readonly ILLMClient _llmClient;
    private readonly IPreferencesRepository _preferencesRepository;
    private readonly IModelProfileRepository _modelProfileRepository;
    private readonly ICredentialService _credentialService;
    private readonly ILogger<OrganizationService> _logger;

    public OrganizationService(
        DesktopScanService scanService,
        IPlanPreviewService planService,
        IExecutionService executionService,
        IUndoService undoService,
        ILLMClient llmClient,
        IPreferencesRepository preferencesRepository,
        IModelProfileRepository modelProfileRepository,
        ICredentialService credentialService,
        ILogger<OrganizationService> logger)
    {
        _scanService = scanService;
        _planService = planService;
        _executionService = executionService;
        _undoService = undoService;
        _llmClient = llmClient;
        _preferencesRepository = preferencesRepository;
        _modelProfileRepository = modelProfileRepository;
        _credentialService = credentialService;
        _logger = logger;
    }

    public async Task<List<Item>> ScanDesktopAsync()
    {
        _logger.LogInformation("开始扫描桌面文件...");

        try
        {
            var items = await _scanService.ScanDesktopAsync();
            _logger.LogInformation("桌面扫描完成，发现 {ItemCount} 个项目", items.Count);
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "扫描桌面文件时发生错误");
            throw;
        }
    }

    public async Task<Plan> GenerateOrganizationPlanAsync(IProgress<string>? progress = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("开始生成整理方案...");

        try
        {
            // Get desktop items and preferences
            _logger.LogDebug("获取桌面项目和用户偏好设置...");
            var items = await _scanService.ScanDesktopAsync();
            var preferences = await _preferencesRepository.LoadAsync();
            var currentProfile = await _modelProfileRepository.GetDefaultAsync();

            _logger.LogInformation("发现 {ItemCount} 个桌面项目，使用模型配置: {ProfileName}",
                items.Count, currentProfile?.Name ?? "无");

            if (currentProfile == null)
            {
                var error = "未配置默认模型配置文件";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            // Check API key
            _logger.LogDebug("验证 API 密钥...");
            var apiKey = await _credentialService.GetApiKeyAsync(currentProfile.KeyRef);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                var error = $"未找到配置文件 '{currentProfile.Name}' 的 API 密钥 (引用: {currentProfile.KeyRef})";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            // Build prompt
            _logger.LogDebug("构建 LLM 提示...");
            var desktopJson = await _scanService.GetDesktopJsonAsync(items);
            var preferencesJson = preferences.ToJsonString();
            // 增强 prompt，明确说明 folders 为已有分类
            var desktopJsonWithNote =
                "注意：desktopJson 中 folders 字段为桌面上已存在的文件夹，请优先考虑将合适的文件归入这些已有文件夹作为分类。\n" +
                desktopJson;
            var prompt = _llmClient.BuildPrompt(currentProfile, desktopJsonWithNote, preferencesJson);

            _logger.LogDebug("提示长度: {PromptLength} 字符，桌面 JSON 长度: {DesktopJsonLength} 字符",
                prompt.Length, desktopJson.Length);

            // Call LLM
            _logger.LogInformation("调用 LLM API 生成整理方案...");
            var llmResponse = await _llmClient.ChatAsync(prompt, currentProfile, progress, cancellationToken);

            _logger.LogInformation("收到 LLM 响应，长度: {ResponseLength} 字符", llmResponse.Length);
            _logger.LogTrace("LLM 响应内容: {LlmResponse}", llmResponse);

            // Parse response
            _logger.LogDebug("解析 LLM 响应...");
            var plan = await _planService.ParseLLMResponseAsync(llmResponse, items);

            _logger.LogInformation("成功解析方案，包含 {FolderCount} 个文件夹，{OperationCount} 个移动操作",
                plan.NewFolders.Count, plan.MoveOperations.Count);

            // Validate plan
            _logger.LogDebug("验证方案...");
            var isValid = await _planService.ValidatePlanAsync(plan, items);
            if (!isValid)
            {
                var error = "生成的整理方案无效或包含错误";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            _logger.LogInformation("整理方案生成并验证成功");
            return plan;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("整理方案生成被取消");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "生成整理方案时发生错误");
            throw;
        }
    }

    public async Task<ExecutionResult> ExecutePlanAsync(Plan plan, IProgress<ExecutionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // 执行前过滤掉不存在的源文件/文件夹
        var filteredOperations = plan.MoveOperations
            .Where(op =>
            {
                var src = Path.Combine(desktopPath, op.Item);
                if (!File.Exists(src) && !Directory.Exists(src))
                {
                    _logger.LogWarning("批量执行时跳过不存在的项目: {Item} -> {TargetFolder}", op.Item, op.TargetFolder);
                    return false;
                }
                return true;
            })
            .ToList();

        var filteredPlan = new Plan(plan.NewFolders, filteredOperations, plan.ModelUsed);

        // 直接执行计划，不中断，失败项后续处理
        var result = await _executionService.ExecutePlanAsync(filteredPlan, desktopPath, progress, cancellationToken);

        // 记录未找到文件/文件夹的操作到日志
        if (result.FailedOperations != null && result.FailedOperations.Any())
        {
            foreach (var op in result.FailedOperations)
            {
                _logger.LogWarning("未找到源文件或文件夹，已跳过：{Item}", op.Item);
            }
        }

        // Save undo information if successful
        if (result.Success && result.CompletedOperations.Any())
        {
            await _undoService.SaveUndoInfoAsync(result.CompletedOperations);
        }

        return result;
    }

    public async Task<ExecutionResult> SimulatePlanAsync(Plan plan)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return await _executionService.SimulatePlanAsync(plan, desktopPath);
    }

    public async Task<bool> UndoLastOperationAsync(IProgress<ExecutionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        return await _undoService.ExecuteUndoAsync(progress, cancellationToken);
    }

    public async Task<bool> HasUndoDataAsync()
    {
        return await _undoService.HasUndoDataAsync();
    }

    public async Task<DateTime?> GetLastOperationTimestampAsync()
    {
        return await _undoService.GetUndoTimestampAsync();
    }

    public async Task<bool> TestModelConnectionAsync(ModelProfile profile)
    {
        _logger.LogInformation("测试模型连接，配置: {ProfileName}", profile.Name);

        try
        {
            var apiKey = await _credentialService.GetApiKeyAsync(profile.KeyRef);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("API 密钥为空，连接测试失败");
                return false;
            }

            var result = await _llmClient.TestConnectionAsync(profile);
            _logger.LogInformation("模型连接测试结果: {TestResult}", result ? "成功" : "失败");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "测试模型连接时发生错误");
            return false;
        }
    }

    public async Task<Dictionary<string, int>> GetDesktopStatisticsAsync()
    {
        return await _scanService.GetItemStatisticsAsync();
    }

    public async Task<Plan> AdjustPlanAsync(Plan originalPlan, List<MoveOperation> adjustments)
    {
        return await _planService.AdjustPlanAsync(originalPlan, adjustments);
    }

    public async Task<Dictionary<string, List<string>>> GetPlanPreviewAsync(Plan plan)
    {
        return await _planService.GetPreviewGroupsAsync(plan);
    }

    /// <summary>
    /// Generate organization plan using natural language preferences
    /// </summary>
    public async Task<Plan> GenerateOrganizationPlanAsync(List<Item> items, string combinedPrompt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating organization plan with natural language preferences");

        try
        {
            // Get current model profile
            var profiles = await _modelProfileRepository.LoadAllAsync();
            var currentProfile = profiles.FirstOrDefault();

            if (currentProfile == null)
            {
                var error = "未找到可用的模型配置";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            // Validate API key
            var apiKey = await _credentialService.GetApiKeyAsync(currentProfile.KeyRef);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                var error = "API 密钥未配置或为空";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            _logger.LogDebug("Using combined prompt length: {PromptLength} characters", combinedPrompt.Length);

            // Call LLM with the combined prompt
            _logger.LogInformation("Calling LLM API to generate organization plan...");
            var llmResponse = await _llmClient.ChatAsync(combinedPrompt, currentProfile, null, cancellationToken);

            _logger.LogInformation("Received LLM response, length: {ResponseLength} characters", llmResponse.Length);
            _logger.LogTrace("LLM response content: {LlmResponse}", llmResponse);

            // Parse response
            _logger.LogDebug("Parsing LLM response...");
            var plan = await _planService.ParseLLMResponseAsync(llmResponse, items);

            _logger.LogInformation("Successfully parsed plan with {FolderCount} folders, {OperationCount} move operations",
                plan.NewFolders?.Count ?? 0, plan.MoveOperations?.Count ?? 0);

            // Validate plan
            _logger.LogDebug("Validating plan...");
            var isValid = await _planService.ValidatePlanAsync(plan, items);
            if (!isValid)
            {
                var error = "Generated organization plan is invalid or contains errors";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            _logger.LogInformation("Organization plan generated and validated successfully");
            return plan;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Organization plan generation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate organization plan");
            throw;
        }
    }

    /// <summary>
    /// Execute a single move operation
    /// </summary>
    public async Task ExecuteSingleOperationAsync(MoveOperation operation)
    {
        _logger.LogDebug("Executing single operation: {Item} -> {TargetFolder}", operation.Item, operation.TargetFolder);

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sourcePath = Path.Combine(desktopPath, operation.Item);

            if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
            {
                _logger.LogWarning("源文件或文件夹不存在，跳过操作: {Item} -> {TargetFolder}", operation.Item, operation.TargetFolder);
                return;
            }

            // Create a minimal plan with just this operation
            var tempPlan = new Plan
            {
                MoveOperations = new List<MoveOperation> { operation },
                NewFolders = new List<string>()
            };

            // Add the target folder to the plan if it's not empty
            if (!string.IsNullOrEmpty(operation.TargetFolder))
            {
                var targetFolderPath = Path.Combine(desktopPath, operation.TargetFolder);
                if (!Directory.Exists(targetFolderPath))
                {
                    tempPlan.NewFolders.Add(operation.TargetFolder);
                }
            }

            // Execute the single operation
            var result = await _executionService.ExecutePlanAsync(tempPlan, desktopPath, null, CancellationToken.None);

            if (!result.Success)
            {
                var errorMessage = result.FailedOperations?.FirstOrDefault()?.ToString() ?? "Unknown error";
                _logger.LogError("Failed to execute operation: {Item} -> {TargetFolder}. Error: {Error}",
                    operation.Item, operation.TargetFolder, errorMessage);
                throw new InvalidOperationException($"Failed to execute operation: {operation.Item} -> {operation.TargetFolder}. {errorMessage}");
            }

            _logger.LogDebug("Successfully executed operation: {Item} -> {TargetFolder}", operation.Item, operation.TargetFolder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute single operation: {Item} -> {TargetFolder}", operation.Item, operation.TargetFolder);
            throw;
        }
    }

    /// <summary>
    /// Release all files from folders on desktop to desktop root
    /// </summary>
    public async Task ReleaseFoldersAsync()
    {
        _logger.LogInformation("Starting folder release operation");

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var movedFiles = new List<string>();
            
            // Get all directories on desktop
            var directories = Directory.GetDirectories(desktopPath, "*", SearchOption.TopDirectoryOnly);
            
            foreach (var directory in directories)
            {
                _logger.LogDebug("Processing directory: {Directory}", directory);
                await ReleaseFolderRecursiveAsync(directory, desktopPath, movedFiles);
            }

            // Remove empty directories
            foreach (var directory in directories)
            {
                try
                {
                    if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any())
                    {
                        Directory.Delete(directory, true);
                        _logger.LogDebug("Removed empty directory: {Directory}", directory);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to remove directory: {Directory}", directory);
                }
            }

            _logger.LogInformation("Folder release completed. Moved {FileCount} files", movedFiles.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to release folders");
            throw;
        }
    }

    private async Task ReleaseFolderRecursiveAsync(string folderPath, string desktopPath, List<string> movedFiles)
    {
        await Task.Run(() =>
        {
            try
            {
                // Get all files in current folder
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var destinationPath = Path.Combine(desktopPath, fileName);
                    
                    // Handle name conflicts by adding numbers
                    destinationPath = GetUniqueFileName(destinationPath);
                    
                    try
                    {
                        File.Move(file, destinationPath);
                        movedFiles.Add(destinationPath);
                        _logger.LogTrace("Moved file: {Source} -> {Destination}", file, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to move file: {File}", file);
                    }
                }

                // Process subdirectories recursively
                var subdirectories = Directory.GetDirectories(folderPath);
                foreach (var subdirectory in subdirectories)
                {
                    ReleaseFolderRecursiveAsync(subdirectory, desktopPath, movedFiles).Wait();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to process folder: {Folder}", folderPath);
            }
        });
    }

    private static string GetUniqueFileName(string originalPath)
    {
        if (!File.Exists(originalPath))
            return originalPath;

        var directory = Path.GetDirectoryName(originalPath) ?? string.Empty;
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalPath);
        var extension = Path.GetExtension(originalPath);

        var counter = 1;
        string newPath;
        do
        {
            var newName = $"{nameWithoutExtension} ({counter}){extension}";
            newPath = Path.Combine(directory, newName);
            counter++;
        }
        while (File.Exists(newPath));

        return newPath;
    }
}