using DesktopOrganizer.Domain;
using Microsoft.Extensions.Logging;

namespace DesktopOrganizer.App.Services
{

/// <summary>
/// Processes user preferences and combines them with system prompts for LLM processing
/// </summary>
public class PreferenceProcessor
{
    private readonly ILogger<PreferenceProcessor> _logger;

    public PreferenceProcessor(ILogger<PreferenceProcessor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Combines user preference with system prompt and file context
    /// </summary>
    public string CombineWithPrompt(string userPreference, List<Item> items)
    {
        var systemPrompt = GetSystemPrompt();
        var fileContext = GenerateFileContext(items);
        var userContext = string.IsNullOrWhiteSpace(userPreference) 
            ? GetDefaultUserPreference() 
            : $"用户偏好：{userPreference}";
        
        var combinedPrompt = $"{systemPrompt}\n\n{fileContext}\n\n{userContext}";
        
        _logger.LogDebug("Generated combined prompt with {ItemCount} items and user preference length: {PreferenceLength}", 
            items.Count, userPreference?.Length ?? 0);
        
        return combinedPrompt;
    }

    private string GetSystemPrompt()
    {
        return @"你是一个专业的文件整理助手。请根据用户的偏好和文件信息，为桌面文件制定一个合理的整理计划。

要求：
1. 仔细分析用户提供的整理偏好
2. 根据文件的类型、名称、大小等信息进行分类
3. 创建合理的文件夹结构
4. 确保文件夹名称简洁明了
5. 避免创建过深的文件夹层级
6. 优先考虑用户的明确偏好
7. 对于不确定的文件，可以放入""其他""类别

请以JSON格式返回整理计划，格式如下：
{{
  ""folders"": [
    {{
      ""name"": ""文件夹名称"",
      ""description"": ""文件夹描述"",
      ""files"": [""文件名1"", ""文件名2""],
      ""keepOnDesktop"": false
    }}
  ],
  ""summary"": ""整理计划的简要说明""
}}

注意：
- 如果用户希望某些文件保留在桌面上，请设置keepOnDesktop为true
- 文件夹名称应该使用中文，简洁明了
- 每个文件只能属于一个文件夹
- 系统文件和回收站等特殊文件会被自动排除";
    }

    private string GenerateFileContext(List<Item> items)
    {
        if (!items.Any())
        {
            return "当前桌面没有需要整理的文件。";
        }

        var fileInfo = items.Select(item => new
        {
            Name = item.Name,
            Extension = item.Extension,
            Size = FormatFileSize(item.Size),
            Type = GetFileTypeDescription(item.Extension)
        }).ToList();

        var context = $"当前桌面文件信息（共{items.Count}个文件）：\n";
        
        foreach (var file in fileInfo)
        {
            context += $"- {file.Name} ({file.Type}, {file.Size})\n";
        }

        return context;
    }

    private string GetDefaultUserPreference()
    {
        return "请根据文件类型进行基础分类整理，创建合理的文件夹结构。";
    }

    private string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }

    private string GetFileTypeDescription(string extension)
    {
        return extension.ToLower() switch
        {
            ".txt" or ".md" or ".doc" or ".docx" => "文档",
            ".pdf" => "PDF文档",
            ".xls" or ".xlsx" => "电子表格",
            ".ppt" or ".pptx" => "演示文稿",
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => "图片",
            ".mp4" or ".avi" or ".mov" or ".wmv" => "视频",
            ".mp3" or ".wav" or ".flac" => "音频",
            ".zip" or ".rar" or ".7z" => "压缩包",
            ".exe" or ".msi" => "可执行文件",
            _ => "其他文件"
        };
    }
}
}