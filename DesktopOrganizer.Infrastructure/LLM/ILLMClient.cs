using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure.Utilities;

namespace DesktopOrganizer.Infrastructure.LLM;

/// <summary>
/// Base interface for LLM client implementations
/// </summary>
public interface ILLMClient
{
    Task<string> ChatAsync(string prompt, ModelProfile profile, IProgress<string>? progress = null, CancellationToken cancellationToken = default);
    Task<bool> TestConnectionAsync(ModelProfile profile, CancellationToken cancellationToken = default);
    string BuildPrompt(ModelProfile profile, string jsonDesktop, string jsonPreferences);
}

/// <summary>
/// Base abstract class for LLM clients with common functionality
/// </summary>
public abstract class BaseLLMClient : ILLMClient
{
    protected readonly HttpClient _httpClient;

    protected BaseLLMClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public abstract Task<string> ChatAsync(string prompt, ModelProfile profile, IProgress<string>? progress = null, CancellationToken cancellationToken = default);

    public abstract Task<bool> TestConnectionAsync(ModelProfile profile, CancellationToken cancellationToken = default);

    public virtual string BuildPrompt(ModelProfile profile, string jsonDesktop, string jsonPreferences)
    {
        var template = @"System:
You are an expert desktop organization assistant.

User:
Below is my Windows desktop dump and my personal preferences.
Please classify each item, create up to 12 folders, and return JSON only.

### Personal preferences (high priority)
```json
{{user_preferences}}
```

### Desktop dump (root = %USERPROFILE%\Desktop)
```json
{{json_desktop}}
```

Rules:
* Follow preferences whenever applicable.
* Keep system icons untouched; max depth = 1.
* Schema to return:
  {
    ""new_folders"": [""<FolderName>"", ...],
    ""move_operations"": [
      {""item"":""<OriginalName>"",""target_folder"":""<FolderName>""},
      ...
    ]
  }
Return **only** valid JSON.";

        return template
            .Replace("{{user_preferences}}", jsonPreferences)
            .Replace("{{json_desktop}}", jsonDesktop);
    }

    /// <summary>
    /// 清理API响应中的markdown标记并提取JSON内容
    /// </summary>
    /// <param name="rawResponse">原始API响应</param>
    /// <returns>清理后的JSON字符串</returns>
    protected virtual string CleanApiResponse(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
            return rawResponse;

        // 使用JsonExtractor清理响应
        var cleanedResponse = JsonExtractor.CleanAndExtractJson(rawResponse);

        // 验证结果是否为有效JSON格式
        if (!JsonExtractor.IsValidJsonFormat(cleanedResponse))
        {
            // 如果清理后不是有效JSON格式，尝试简化模式
            cleanedResponse = JsonExtractor.CleanAndExtractJson(rawResponse, useBalancedPattern: false);
        }

        return cleanedResponse;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _httpClient?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}