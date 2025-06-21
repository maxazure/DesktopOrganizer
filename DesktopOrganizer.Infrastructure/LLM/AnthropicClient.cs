using DesktopOrganizer.Domain;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DesktopOrganizer.Infrastructure.LLM;

/// <summary>
/// Anthropic Claude API client implementation
/// </summary>
public class AnthropicClient : BaseLLMClient
{
    public AnthropicClient(HttpClient httpClient) : base(httpClient) { }

    public override async Task<string> ChatAsync(string prompt, ModelProfile profile,
        IProgress<string>? progress = null, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{profile.BaseUrl.TrimEnd('/')}/v1/messages");

        var apiKey = await GetApiKeyAsync(profile);
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Headers.Add("User-Agent", "DesktopOrganizer/1.0");

        var requestBody = new
        {
            model = profile.ModelId,
            max_tokens = 4000,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            stream = progress != null
        };

        var json = JsonSerializer.Serialize(requestBody);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.Timeout = TimeSpan.FromSeconds(profile.TimeoutSeconds);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        if (progress != null)
        {
            return await ProcessStreamingResponseAsync(response, progress, cancellationToken);
        }
        else
        {
            return await ProcessNonStreamingResponseAsync(response, cancellationToken);
        }
    }

    public override async Task<bool> TestConnectionAsync(ModelProfile profile, CancellationToken cancellationToken = default)
    {
        try
        {
            var testPrompt = "Say 'test' only.";
            var response = await ChatAsync(testPrompt, profile, null, cancellationToken);
            return !string.IsNullOrWhiteSpace(response);
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> ProcessStreamingResponseAsync(HttpResponseMessage response,
        IProgress<string> progress, CancellationToken cancellationToken)
    {
        var fullResponse = new StringBuilder();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (line.StartsWith("data: "))
            {
                var data = line.Substring(6);
                if (data == "[DONE]") break;

                try
                {
                    var chunk = JsonSerializer.Deserialize<JsonElement>(data);
                    if (chunk.TryGetProperty("type", out var type) && type.GetString() == "content_block_delta")
                    {
                        if (chunk.TryGetProperty("delta", out var delta) &&
                            delta.TryGetProperty("text", out var text))
                        {
                            var token = text.GetString() ?? string.Empty;
                            fullResponse.Append(token);
                            progress.Report(token);
                        }
                    }
                }
                catch (JsonException)
                {
                    // Skip invalid JSON chunks
                }
            }
        }

        var rawResponse = fullResponse.ToString();
        // 清理流式响应中的markdown标记并提取JSON
        return CleanApiResponse(rawResponse);
    }

    private async Task<string> ProcessNonStreamingResponseAsync(HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(content);

        if (jsonResponse.TryGetProperty("content", out var contentArray) && contentArray.GetArrayLength() > 0)
        {
            var firstContent = contentArray[0];
            if (firstContent.TryGetProperty("text", out var text))
            {
                var rawResponse = text.GetString() ?? string.Empty;
                // 清理API响应中的markdown标记并提取JSON
                return CleanApiResponse(rawResponse);
            }
        }

        throw new InvalidOperationException("Invalid response format from Anthropic API");
    }

    private async Task<string> GetApiKeyAsync(ModelProfile profile)
    {
        // This would be implemented by the credential service
        // For now, return a placeholder that will be injected by DI
        await Task.CompletedTask;
        return "api-key-placeholder";
    }
}