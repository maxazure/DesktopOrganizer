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
            var prompt = _llmClient.BuildPrompt(currentProfile, desktopJson, preferencesJson);

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

        // 直接执行计划，不中断，失败项后续处理
        var result = await _executionService.ExecutePlanAsync(plan, desktopPath, progress, cancellationToken);

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
}