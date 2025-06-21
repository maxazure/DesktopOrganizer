using System;
using System.Text.RegularExpressions;

namespace DesktopOrganizer.Infrastructure.Utilities
{
    /// <summary>
    /// 用于从API响应中提取JSON内容的工具类
    /// </summary>
    public static class JsonExtractor
    {
        /// <summary>
        /// 正则表达式模式：匹配从第一个{开始到最后一个}结束的完整JSON内容
        /// 使用懒惰匹配和平衡组来确保正确匹配嵌套的大括号
        /// </summary>
        private static readonly Regex JsonPattern = new Regex(
            @"(?s)\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}",
            RegexOptions.Compiled | RegexOptions.Singleline
        );

        /// <summary>
        /// 简化版正则表达式：匹配第一个{到最后一个}之间的内容
        /// 适用于大多数情况
        /// </summary>
        private static readonly Regex SimpleJsonPattern = new Regex(
            @"(?s)\{.*\}",
            RegexOptions.Compiled | RegexOptions.Singleline
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
                var pattern = useBalancedPattern ? JsonPattern : SimpleJsonPattern;

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

            // 移除markdown代码块标记
            var cleaned = Regex.Replace(text, @"```[\w]*\s*", "", RegexOptions.Multiline);
            cleaned = Regex.Replace(cleaned, @"```\s*", "", RegexOptions.Multiline);

            return cleaned.Trim();
        }

        /// <summary>
        /// 综合处理方法：先移除markdown标记，再提取JSON
        /// </summary>
        /// <param name="responseText">API响应文本</param>
        /// <returns>提取的JSON字符串</returns>
        public static string CleanAndExtractJson(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
                return responseText;

            // 先移除markdown标记
            var cleanedText = RemoveMarkdownCodeBlocks(responseText);

            // 再提取JSON
            return ExtractJson(cleanedText);
        }
    }
}