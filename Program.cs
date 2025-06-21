using System;
using System.Text.RegularExpressions;

namespace JsonExtractorTest
{
    /// <summary>
    /// JSON提取器工具类
    /// </summary>
    public static class JsonExtractor
    {
        /// <summary>
        /// 平衡组正则表达式：匹配完整的JSON对象
        /// </summary>
        private static readonly Regex JsonPattern = new Regex(
            @"(?s)\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        /// <summary>
        /// 简化正则表达式：匹配第一个{到最后一个}
        /// </summary>
        private static readonly Regex SimpleJsonPattern = new Regex(
            @"(?s)\{.*\}",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        /// <summary>
        /// 从文本中提取JSON内容
        /// </summary>
        public static string ExtractJson(string responseText, bool useBalancedPattern = true)
        {
            if (string.IsNullOrWhiteSpace(responseText))
                return responseText;

            try
            {
                var pattern = useBalancedPattern ? JsonPattern : SimpleJsonPattern;
                var match = pattern.Match(responseText);

                if (match.Success)
                {
                    return match.Value.Trim();
                }

                return responseText;
            }
            catch (Exception)
            {
                return responseText;
            }
        }

        /// <summary>
        /// 移除markdown代码块标记
        /// </summary>
        public static string RemoveMarkdownCodeBlocks(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var cleaned = Regex.Replace(text, @"```[\w]*\s*", "", RegexOptions.Multiline);
            cleaned = Regex.Replace(cleaned, @"```\s*", "", RegexOptions.Multiline);

            return cleaned.Trim();
        }

        /// <summary>
        /// 综合处理：移除markdown + 提取JSON
        /// </summary>
        public static string CleanAndExtractJson(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
                return responseText;

            var cleanedText = RemoveMarkdownCodeBlocks(responseText);
            return ExtractJson(cleanedText);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== JSON提取器测试 ===");
            Console.WriteLine();

            // 测试您提供的示例
            var testCase = @"```json
{
  ""new_folders"": [
    ""3D_Models"",
    ""Audio_Projects"",
    ""Design_Files"",
    ""Shortcuts""
  ],
  ""move_operations"": [
    {""item"": ""(成绩单）委托书.docx"", ""target_folder"": ""Documents""},
    {""item"": ""099999微信图片_20250326204211.png"", ""target_folder"": ""Pictures""},
    {""item"": ""123456_20250408104813.jpg"", ""target_folder"": ""Pictures""},
    {""item"": ""apple pencil 笔套.skb"", ""target_folder"": ""3D_Models""},
    {""item"": ""Blender 3.4.lnk"", ""target_folder"": ""Shortcuts""}
  ]
}
```";

            Console.WriteLine("原始响应内容:");
            Console.WriteLine("长度: " + testCase.Length);
            Console.WriteLine("包含markdown标记: " + testCase.Contains("```"));
            Console.WriteLine();

            // 测试提取效果
            string extracted = JsonExtractor.CleanAndExtractJson(testCase);

            Console.WriteLine("提取后的JSON:");
            Console.WriteLine("长度: " + extracted.Length);
            Console.WriteLine("是否为纯JSON: " + (extracted.StartsWith("{") && extracted.EndsWith("}")));
            Console.WriteLine("包含markdown标记: " + extracted.Contains("```"));
            Console.WriteLine();

            Console.WriteLine("提取的JSON内容 (前200字符):");
            Console.WriteLine(extracted.Substring(0, Math.Min(200, extracted.Length)) + "...");
            Console.WriteLine();

            Console.WriteLine("=== 正则表达式模式说明 ===");
            Console.WriteLine();
            Console.WriteLine("1. 平衡组模式 (推荐用于复杂JSON):");
            Console.WriteLine("   @\"(?s)\\{(?:[^{}]|(?<open>\\{)|(?<-open>\\}))*(?(open)(?!))\\}\"");
            Console.WriteLine();
            Console.WriteLine("2. 简化模式 (适用于大多数情况):");
            Console.WriteLine("   @\"(?s)\\{.*\\}\"");
            Console.WriteLine();
            Console.WriteLine("3. Markdown清理:");
            Console.WriteLine("   @\"```[\\w]*\\s*\" - 移除开始标记");
            Console.WriteLine("   @\"```\\s*\" - 移除结束标记");
            Console.WriteLine();

            Console.WriteLine("使用方法:");
            Console.WriteLine("string cleanJson = JsonExtractor.CleanAndExtractJson(apiResponse);");

            Console.WriteLine("\n测试完成！按任意键退出...");
            Console.ReadKey();
        }
    }
}