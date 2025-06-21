using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure.Utilities;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DesktopOrganizer.App.Services;

/// <summary>
/// Application service for plan preview and validation
/// </summary>
public class PlanPreviewService : IPlanPreviewService
{
    private readonly ILogger<PlanPreviewService>? _logger;

    public PlanPreviewService(ILogger<PlanPreviewService>? logger = null)
    {
        _logger = logger;
    }

    public async Task<Plan> ParseLLMResponseAsync(string llmResponse, List<Item> desktopItems)
    {
        return await Task.Run(() =>
        {
            try
            {
                _logger?.LogDebug("开始解析LLM响应，原始响应长度: {Length} 字符", llmResponse.Length);

                // 使用专门的JsonExtractor工具提取JSON
                var jsonContent = JsonExtractor.CleanAndExtractJson(llmResponse, useBalancedPattern: true);

                _logger?.LogDebug("提取的JSON内容长度: {Length} 字符", jsonContent.Length);
                _logger?.LogDebug("提取的JSON前100字符: {JsonPreview}",
                    jsonContent.Length > 100 ? jsonContent.Substring(0, 100) + "..." : jsonContent);

                // 验证JSON格式
                if (!JsonExtractor.IsValidJsonFormat(jsonContent))
                {
                    _logger?.LogError("提取的内容不是有效的JSON格式");
                    throw new InvalidOperationException("提取的内容不是有效的JSON格式");
                }

                using var document = JsonDocument.Parse(jsonContent);
                var root = document.RootElement;

                var newFolders = new List<string>();
                var moveOperations = new List<MoveOperation>();
                var planFolders = new List<PlanFolder>();

                // Try new format first (folders array)
                if (root.TryGetProperty("folders", out var foldersArrayElement) && foldersArrayElement.ValueKind == JsonValueKind.Array)
                {
                    _logger?.LogDebug("Parsing new folder format with 'folders' array");
                    
                    foreach (var folderElement in foldersArrayElement.EnumerateArray())
                    {
                        if (folderElement.TryGetProperty("name", out var nameElement))
                        {
                            var folderName = nameElement.GetString() ?? "";
                            if (!string.IsNullOrEmpty(folderName))
                            {
                                var planFolder = new PlanFolder
                                {
                                    Name = folderName,
                                    Description = folderElement.TryGetProperty("description", out var descElement) ? descElement.GetString() : "",
                                    KeepOnDesktop = folderElement.TryGetProperty("keepOnDesktop", out var keepElement) && keepElement.GetBoolean()
                                };

                                // Parse files array
                                if (folderElement.TryGetProperty("files", out var filesElement) && filesElement.ValueKind == JsonValueKind.Array)
                                {
                                    var files = new List<string>();
                                    foreach (var fileElement in filesElement.EnumerateArray())
                                    {
                                        var fileName = fileElement.GetString();
                                        if (!string.IsNullOrEmpty(fileName))
                                        {
                                            files.Add(fileName);
                                            
                                            // Create move operations for backwards compatibility
                                            if (!planFolder.KeepOnDesktop)
                                            {
                                                moveOperations.Add(new MoveOperation(fileName, folderName));
                                            }
                                        }
                                    }
                                    planFolder.Files = files;
                                }

                                planFolders.Add(planFolder);
                                
                                // Add to new folders list if not keeping on desktop
                                if (!planFolder.KeepOnDesktop)
                                {
                                    newFolders.Add(folderName);
                                }
                            }
                        }
                    }
                }
                // Fallback to old format
                else if (root.TryGetProperty("new_folders", out var foldersElement))
                {
                    foreach (var folder in foldersElement.EnumerateArray())
                    {
                        var folderName = folder.GetString();
                        if (!string.IsNullOrWhiteSpace(folderName))
                        {
                            newFolders.Add(folderName);
                        }
                    }
                }

                // Parse move_operations
                if (root.TryGetProperty("move_operations", out var operationsElement))
                {
                    foreach (var operation in operationsElement.EnumerateArray())
                    {
                        if (operation.TryGetProperty("item", out var itemElement) &&
                            operation.TryGetProperty("target_folder", out var targetElement))
                        {
                            var itemName = itemElement.GetString();
                            var targetFolder = targetElement.GetString();

                            if (!string.IsNullOrWhiteSpace(itemName) && !string.IsNullOrWhiteSpace(targetFolder))
                            {
                                moveOperations.Add(new MoveOperation(itemName, targetFolder));
                            }
                        }
                    }
                }

                _logger?.LogInformation("成功解析JSON，新文件夹: {FolderCount}，移动操作: {OperationCount}，计划文件夹: {PlanFolderCount}",
                    newFolders.Count, moveOperations.Count, planFolders.Count);

                var plan = new Plan(newFolders, moveOperations, "LLM Generated");
                plan.Folders = planFolders;
                return plan;
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "JSON解析失败: {Message}", ex.Message);
                _logger?.LogDebug("失败的JSON内容: {JsonContent}", llmResponse);
                throw new InvalidOperationException($"Failed to parse LLM response as JSON: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析LLM响应时发生意外错误: {Message}", ex.Message);
                throw;
            }
        });
    }

    public async Task<bool> ValidatePlanAsync(Plan plan, List<Item> desktopItems)
    {
        return await Task.Run(() =>
        {
            _logger?.LogDebug("开始验证方案...");

            // 检查方案基本有效性
            if (!plan.IsValid())
            {
                _logger?.LogError("方案基本验证失败: Plan.IsValid() 返回 false");
                return false;
            }
            _logger?.LogDebug("方案基本验证通过");

            var itemNames = desktopItems.Select(i => i.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
            _logger?.LogDebug("桌面项目名称集合创建完成，包含 {ItemCount} 个项目", itemNames.Count);

            // 检查移动操作中的所有项目是否在桌面上存在
            _logger?.LogDebug("检查移动操作中的项目是否存在...");
            var missingItems = new List<string>();
            var validOperations = new List<MoveOperation>();

            foreach (var operation in plan.MoveOperations)
            {
                // 统一全角/半角括号，去除首尾空格
                string Normalize(string name)
                {
                    if (string.IsNullOrWhiteSpace(name)) return "";
                    var n = name.Trim()
                        .Replace('（', '(').Replace('）', ')')
                        .Replace('【', '[').Replace('】', ']')
                        .Replace('｛', '{').Replace('｝', '}')
                        .Replace('《', '<').Replace('》', '>')
                        .Replace('“', '"').Replace('”', '"')
                        .Replace('‘', '\'').Replace('’', '\'');
                    // 可根据需要添加更多符号替换
                    return n;
                }

                var normalizedItem = Normalize(operation.Item);

                // 先精确匹配
                var matchedItem = itemNames.FirstOrDefault(item =>
                    string.Equals(Normalize(item), normalizedItem, StringComparison.OrdinalIgnoreCase));

                // 再尝试包含关系和去除空格、符号模糊匹配
                if (matchedItem == null)
                {
                    matchedItem = itemNames.FirstOrDefault(item =>
                        Normalize(item).Replace(" ", "") == normalizedItem.Replace(" ", "") ||
                        Normalize(item).Contains(normalizedItem, StringComparison.OrdinalIgnoreCase) ||
                        normalizedItem.Contains(Normalize(item), StringComparison.OrdinalIgnoreCase));
                }

                if (matchedItem != null)
                {
                    if (!string.Equals(matchedItem, operation.Item, StringComparison.Ordinal))
                    {
                        _logger?.LogWarning("修正了项目名称: '{OriginalName}' -> '{CorrectedName}'",
                            operation.Item, matchedItem);
                    }
                    validOperations.Add(new MoveOperation(matchedItem, operation.TargetFolder));
                }
                else
                {
                    missingItems.Add(operation.Item);
                    _logger?.LogWarning("跳过不存在的项目: '{ItemName}'", operation.Item);
                }
            }

            if (missingItems.Any())
            {
                _logger?.LogWarning("发现 {MissingCount} 个不存在的项目: {MissingItems}，已跳过这些项目",
                    missingItems.Count, string.Join(", ", missingItems));
            }

            // 更新计划以使用修正后的操作列表
            if (validOperations.Count != plan.MoveOperations.Count)
            {
                var updatedPlan = new Plan(plan.NewFolders, validOperations, plan.ModelUsed);
                // 这里我们需要某种方式来更新原始计划，但由于结构限制，我们记录警告
                _logger?.LogWarning("移动操作从 {OriginalCount} 个减少到 {ValidCount} 个",
                    plan.MoveOperations.Count, validOperations.Count);
            }

            // 如果没有有效的移动操作，则验证失败
            if (!validOperations.Any())
            {
                _logger?.LogError("没有有效的移动操作");
                return false;
            }

            _logger?.LogDebug("验证了 {ValidCount} 个有效的移动操作", validOperations.Count);

            // 检查文件夹名称是否有效
            _logger?.LogDebug("检查文件夹名称有效性...");
            var invalidFolders = new List<string>();
            foreach (var folderName in plan.NewFolders)
            {
                if (!IsValidWindowsFolderName(folderName))
                {
                    invalidFolders.Add(folderName);
                }
            }

            if (invalidFolders.Any())
            {
                _logger?.LogError("发现 {InvalidCount} 个无效文件夹名称: {InvalidFolders}",
                    invalidFolders.Count, string.Join(", ", invalidFolders));
                return false;
            }
            _logger?.LogDebug("所有文件夹名称都有效");

            _logger?.LogInformation("方案验证成功通过");
            return true;
        });
    }

    public async Task<Plan> AdjustPlanAsync(Plan originalPlan, List<MoveOperation> adjustments)
    {
        return await Task.Run(() =>
        {
            var adjustedOperations = new List<MoveOperation>(originalPlan.MoveOperations);

            foreach (var adjustment in adjustments)
            {
                // Find existing operation for the same item
                var existingIndex = adjustedOperations.FindIndex(op =>
                    op.Item.Equals(adjustment.Item, StringComparison.OrdinalIgnoreCase));

                if (existingIndex >= 0)
                {
                    // Update existing operation
                    adjustedOperations[existingIndex] = adjustment;
                }
                else
                {
                    // Add new operation
                    adjustedOperations.Add(adjustment);
                }
            }

            // Update new folders list based on adjusted operations
            var requiredFolders = adjustedOperations
                .Select(op => op.TargetFolder)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return new Plan(requiredFolders, adjustedOperations, originalPlan.ModelUsed);
        });
    }

    public async Task<Dictionary<string, List<string>>> GetPreviewGroupsAsync(Plan plan)
    {
        return await Task.Run(() => plan.GroupByFolder());
    }

    public async Task<List<string>> ValidateFolderNamesAsync(List<string> folderNames)
    {
        return await Task.Run(() =>
        {
            var invalidNames = new List<string>();

            foreach (var name in folderNames)
            {
                if (!IsValidWindowsFolderName(name))
                {
                    invalidNames.Add(name);
                }
            }

            return invalidNames;
        });
    }


    private static bool IsValidWindowsFolderName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Check for invalid characters
        var invalidChars = new char[] { '<', '>', ':', '"', '|', '?', '*', '\\', '/' };
        if (name.IndexOfAny(invalidChars) >= 0)
            return false;

        // Check for reserved names
        var reservedNames = new[] { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5",
                                   "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4",
                                   "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
        if (reservedNames.Contains(name.ToUpperInvariant()))
            return false;

        // Check length (Windows has a 255 character limit)
        if (name.Length > 255)
            return false;

        // Cannot end with space or period
        if (name.EndsWith(' ') || name.EndsWith('.'))
            return false;

        return true;
    }
}