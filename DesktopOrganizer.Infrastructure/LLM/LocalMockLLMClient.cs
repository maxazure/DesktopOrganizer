// LocalMockLLMClient.cs
using DesktopOrganizer.Domain;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopOrganizer.Infrastructure.LLM
{
    /// <summary>
    /// 本地模拟LLM客户端，直接读取 respones.json 文件作为响应
    /// </summary>
    public class LocalMockLLMClient : BaseLLMClient
    {
        private readonly string _jsonPath;

        public LocalMockLLMClient(string jsonPath = "respones.json") : base(new HttpClient())
        {
            _jsonPath = jsonPath;
        }

        public override async Task<string> ChatAsync(string prompt, ModelProfile profile, IProgress<string>? progress = null, CancellationToken cancellationToken = default)
        {
            // 直接读取 respones.json 文件内容
            if (!File.Exists(_jsonPath))
                throw new FileNotFoundException($"模拟LLM响应文件未找到: {_jsonPath}");

            var json = await File.ReadAllTextAsync(_jsonPath, cancellationToken);
            progress?.Report(json);
            return json;
        }

        public override Task<bool> TestConnectionAsync(ModelProfile profile, CancellationToken cancellationToken = default)
        {
            // 检查文件是否存在
            return Task.FromResult(File.Exists(_jsonPath));
        }

        public override string BuildPrompt(ModelProfile profile, string jsonDesktop, string jsonPreferences)
        {
            // 不需要实际构建prompt，直接返回空字符串或调试信息
            return $"[MOCK] prompt ignored, using {_jsonPath}";
        }
    }
}