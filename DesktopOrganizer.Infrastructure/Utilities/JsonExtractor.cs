using System;
using System.Text.RegularExpressions;

namespace DesktopOrganizer.Infrastructure.Utilities
{
    /// <summary>
    /// 用于从API响应中提取JSON内容的工具类
    /// 专门处理包含markdown标记的LLM API响应
    /// </summary>
    public static class JsonExtractor
    {
        /// <summary>
        /// 平衡组正则表达式：匹配完整的JSON对象
        /// 使用.NET平衡组语法确保正确匹配嵌套的大括号
        /// </summary>
        private static readonly Regex JsonBalancedPattern = new Regex(
            @"(?s)\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        /// <summary>
        /// 简化正则表达式：匹配第一个{到最后一个}之间的内容
        /// 适用于大多数情况，性能更好
        /// </summary>
        private static readonly Regex JsonSimplePattern = new Regex(
            @"(?s)\{.*\}",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        /// <summary>
        /// Markdown代码块开始标记正则
        /// </summary>
        private static readonly Regex MarkdownStartPattern = new Regex(
            @"```[\w]*\s*",
            RegexOptions.Compiled | RegexOptions.Multiline
        );

        /// <summary>
        /// Markdown代码块结束标记正则
        /// </summary>
        private static readonly Regex MarkdownEndPattern = new Regex(
            @"```\s*",
            RegexOptions.Compiled | RegexOptions.Multiline
        );

        /// <summary>
        /// 从包含markdown标记的文本中提取JSON内容
        /// </summary>
        /// <param name="responseText">包含markdown标记的API响应文本</param>
        /// <param name="useBalancedPattern">是否使用平衡组模式（推荐用于复杂嵌套JSON）</param>
        /// <returns>提取的JSON字符串，如果没有找到则返回原文本</returns>
        public static string ExtractJson(string responseText, bool useBalancedPattern = true)
        {
            if (string.IsNullOrWhiteSpace(responseText))
                return responseText;

            try
            {
                // 选择使用的正则表达式模式
                var pattern = useBalancedPattern ? JsonBalancedPattern : JsonSimplePattern;

                // 查找匹配的JSON内容
                var match = pattern.Match(responseText);

                if (match.Success)
                {
                    return match.Value.Trim();
                }

                // 如果没有找到匹配，返回原文本
                return responseText;
            }
            catch (Exception)
            {
                // 如果正则表达式处理失败，返回原文本
                return responseText;
            }
        }

        /// <summary>
        /// 移除markdown代码块标记（如```json 和 ```）
        /// </summary>
        /// <param name="text">包含markdown标记的文本</param>
        /// <returns>清理后的文本</returns>
        public static string RemoveMarkdownCodeBlocks(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            try
            {
                // 移除markdown代码块标记
                var cleaned = MarkdownStartPattern.Replace(text, "");
                cleaned = MarkdownEndPattern.Replace(cleaned, "");

                return cleaned.Trim();
            }
            catch (Exception)
            {
                return text;
            }
        }

        /// <summary>
        /// 综合处理方法：先移除markdown标记，再提取JSON
        /// 这是推荐的使用方法
        /// </summary>
        /// <param name="responseText">API响应文本</param>
        /// <param name="useBalancedPattern">是否使用平衡组模式</param>
        /// <returns>提取的JSON字符串</returns>
        public static string CleanAndExtractJson(string responseText, bool useBalancedPattern = true)
        {
            if (string.IsNullOrWhiteSpace(responseText))
                return responseText;

            // 先移除markdown标记
            var cleanedText = RemoveMarkdownCodeBlocks(responseText);

            // 再提取JSON
            return ExtractJson(cleanedText, useBalancedPattern);
        }

        /// <summary>
        /// 验证提取的内容是否为有效的JSON格式
        /// </summary>
        /// <param name="jsonText">待验证的JSON文本</param>
        /// <returns>是否为有效JSON格式</returns>
        public static bool IsValidJsonFormat(string jsonText)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                return false;

            var trimmed = jsonText.Trim();
            return trimmed.StartsWith("{") && trimmed.EndsWith("}");
        }

        /// <summary>
        /// 获取正则表达式模式信息（用于调试和文档）
        /// </summary>
        /// <returns>正则表达式模式的描述</returns>
        public static string GetPatternInfo()
        {
            return @"
=== JSON提取正则表达式模式 ===

1. 平衡组模式 (推荐用于复杂JSON):
   Pattern: (?s)\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}
   说明:
   - (?s): 单行模式，让.匹配换行符
   - (?<open>\{): 遇到{时压入open组
   - (?<-open>\}): 遇到}时从open组弹出
   - (?(open)(?!)): 如果open组不为空则匹配失败

2. 简化模式 (适用于大多数情况):
   Pattern: (?s)\{.*\}
   说明: 匹配第一个{到最后一个}的所有内容

3. Markdown清理:
   开始标记: ```[\w]*\s*
   结束标记: ```\s*

使用方法:
string cleanJson = JsonExtractor.CleanAndExtractJson(apiResponse);
";
        }
    }
}