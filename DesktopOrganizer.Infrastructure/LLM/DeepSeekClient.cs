using DesktopOrganizer.Domain;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DesktopOrganizer.Infrastructure.LLM;

/// <summary>
/// DeepSeek API client implementation
/// </summary>
public class DeepSeekClient : BaseLLMClient
{
    private readonly ICredentialService _credentialService;
    private readonly ILogger<DeepSeekClient> _logger;

    public DeepSeekClient(HttpClient httpClient, ICredentialService credentialService, ILogger<DeepSeekClient> logger)
        : base(httpClient)
    {
        _credentialService = credentialService;
        _logger = logger;
    }

    public override async Task<string> ChatAsync(string prompt, ModelProfile profile,
        IProgress<string>? progress = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("开始调用 DeepSeek API，模型: {ModelId}, 流式传输: {IsStreaming}",
            profile.ModelId, progress != null);

        try
        {
            var apiKey = await GetApiKeyAsync(profile);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                var error = $"API 密钥未找到，配置文件: {profile.Name}, 密钥引用: {profile.KeyRef}";
                _logger.LogError(error);
                throw new InvalidOperationException(error);
            }

            var requestUrl = $"{profile.BaseUrl.TrimEnd('/')}/v1/chat/completions";
            _logger.LogDebug("请求 URL: {RequestUrl}", requestUrl);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Headers.Add("User-Agent", "DesktopOrganizer/1.0");

            var requestBody = new
            {
                model = profile.ModelId,
                messages = new[]
                {
                    new { role = "system", content = "你是一个专业的桌面文件整理助手。请严格按照用户的偏好设置和指令，分析桌面文件并生成合理的整理方案。" },
                    new { role = "user", content = prompt }
                },
                stream = progress != null,
                temperature = 0.1,
                max_tokens = 4000,
                top_p = 0.8
            };

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            _logger.LogDebug("请求体大小: {RequestSize} 字符", json.Length);
            _logger.LogTrace("请求体内容: {RequestBody}", json);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.Timeout = TimeSpan.FromSeconds(profile.TimeoutSeconds);

            _logger.LogInformation("发送 HTTP 请求到 DeepSeek API...");
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            _logger.LogInformation("收到响应，状态码: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("API 请求失败，状态码: {StatusCode}, 错误内容: {ErrorContent}",
                    response.StatusCode, errorContent);
                throw new HttpRequestException($"DeepSeek API 请求失败 (状态码: {response.StatusCode}): {errorContent}");
            }

            if (progress != null)
            {
                _logger.LogInformation("开始处理流式响应...");
                return await ProcessStreamingResponseAsync(response, progress, cancellationToken);
            }
            else
            {
                _logger.LogInformation("开始处理非流式响应...");
                return await ProcessNonStreamingResponseAsync(response, cancellationToken);
            }
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogError("DeepSeek API 请求超时，超时时间: {TimeoutSeconds} 秒", profile.TimeoutSeconds);
            throw new TimeoutException($"DeepSeek API 请求超时 ({profile.TimeoutSeconds} 秒)");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "DeepSeek API HTTP 请求错误");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeepSeek API 调用过程中发生未知错误");
            throw;
        }
    }

    public override async Task<bool> TestConnectionAsync(ModelProfile profile, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("测试 DeepSeek API 连接，配置文件: {ProfileName}", profile.Name);

        try
        {
            var testPrompt = "请回复 'test' 即可。";
            var response = await ChatAsync(testPrompt, profile, null, cancellationToken);
            var isSuccess = !string.IsNullOrWhiteSpace(response);

            _logger.LogInformation("连接测试结果: {TestResult}, 响应长度: {ResponseLength}",
                isSuccess ? "成功" : "失败", response?.Length ?? 0);

            return isSuccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DeepSeek API 连接测试失败");
            return false;
        }
    }

    private async Task<string> ProcessStreamingResponseAsync(HttpResponseMessage response,
        IProgress<string> progress, CancellationToken cancellationToken)
    {
        var fullResponse = new StringBuilder();
        var chunkCount = 0;

        try
        {
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (line.StartsWith("data: "))
                {
                    var data = line.Substring(6);
                    if (data == "[DONE]")
                    {
                        _logger.LogDebug("流式响应结束，共处理 {ChunkCount} 个数据块", chunkCount);
                        break;
                    }

                    try
                    {
                        var chunk = JsonSerializer.Deserialize<JsonElement>(data);
                        if (chunk.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                        {
                            var choice = choices[0];
                            if (choice.TryGetProperty("delta", out var delta) &&
                                delta.TryGetProperty("content", out var content))
                            {
                                var token = content.GetString() ?? string.Empty;
                                fullResponse.Append(token);
                                progress.Report(token);
                                chunkCount++;
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogWarning("跳过无效的 JSON 数据块: {Data}, 错误: {Error}", data, ex.Message);
                    }
                }
            }

            var rawResponse = fullResponse.ToString();
            _logger.LogInformation("流式响应处理完成，原始响应长度: {ResponseLength} 字符", rawResponse.Length);

            // 清理流式响应中的markdown标记并提取JSON
            var cleanedResponse = CleanApiResponse(rawResponse);
            _logger.LogInformation("流式响应清理完成，清理后长度: {CleanedLength} 字符", cleanedResponse.Length);

            return cleanedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理流式响应时发生错误");
            throw;
        }
    }

    private async Task<string> ProcessNonStreamingResponseAsync(HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("收到非流式响应，长度: {ContentLength} 字符", content.Length);
            _logger.LogTrace("响应内容: {ResponseContent}", content);

            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(content);

            if (jsonResponse.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var choice = choices[0];
                if (choice.TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var messageContent))
                {
                    var rawResponse = messageContent.GetString() ?? string.Empty;
                    _logger.LogInformation("成功解析响应，原始内容长度: {RawLength} 字符", rawResponse.Length);

                    // 清理API响应中的markdown标记并提取JSON
                    var cleanedResponse = CleanApiResponse(rawResponse);
                    _logger.LogInformation("清理后响应长度: {CleanedLength} 字符", cleanedResponse.Length);

                    return cleanedResponse;
                }
            }

            // 记录响应结构以便调试
            _logger.LogWarning("响应格式异常，无法找到 choices[0].message.content 路径");
            _logger.LogDebug("实际响应结构: {ResponseStructure}", content);
            throw new InvalidOperationException("DeepSeek API 响应格式无效：无法找到消息内容");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "解析 DeepSeek API 响应 JSON 时发生错误");
            throw new InvalidOperationException("DeepSeek API 响应 JSON 格式无效", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理非流式响应时发生错误");
            throw;
        }
    }

    private async Task<string> GetApiKeyAsync(ModelProfile profile)
    {
        _logger.LogDebug("获取 API 密钥，密钥引用: {KeyRef}", profile.KeyRef);

        try
        {
            var apiKey = await _credentialService.GetApiKeyAsync(profile.KeyRef);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("未找到 API 密钥，密钥引用: {KeyRef}", profile.KeyRef);
            }
            else
            {
                _logger.LogDebug("成功获取 API 密钥，长度: {KeyLength} 字符", apiKey.Length);
            }

            return apiKey ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取 API 密钥时发生错误，密钥引用: {KeyRef}", profile.KeyRef);
            throw;
        }
    }
}