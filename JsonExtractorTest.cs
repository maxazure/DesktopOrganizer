using System;
using DesktopOrganizer.Infrastructure.Utilities;

namespace DesktopOrganizer.Tests
{
    /// <summary>
    /// JSON提取器测试类
    /// </summary>
    public class JsonExtractorTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== JSON提取器测试 ===");

            // 测试用例1：标准markdown格式
            var testCase1 = @"```json
{
  ""new_folders"": [
    ""3D_Models"",
    ""Audio_Projects"",
    ""Design_Files"",
    ""Shortcuts""
  ],
  ""move_operations"": [
    {""item"": ""test.txt"", ""target_folder"": ""Documents""}
  ]
}
```";

            Console.WriteLine("测试用例1 - 标准markdown格式:");
            var result1 = JsonExtractor.ExtractJson(testCase1);
            Console.WriteLine($"原文本长度: {testCase1.Length}");
            Console.WriteLine($"提取结果长度: {result1.Length}");
            Console.WriteLine($"提取成功: {result1.StartsWith("{") && result1.EndsWith("}")}");
            Console.WriteLine();

            // 测试用例2：带其他文本的响应
            var testCase2 = @"这是一个API响应示例：

```json
{
  ""status"": ""success"",
  ""data"": {
    ""files"": [""file1.txt"", ""file2.pdf""]
  }
}
```

希望这个结果对您有帮助。";

            Console.WriteLine("测试用例2 - 带其他文本:");
            var result2 = JsonExtractor.CleanAndExtractJson(testCase2);
            Console.WriteLine($"提取结果: {result2.Substring(0, Math.Min(50, result2.Length))}...");
            Console.WriteLine($"是否为纯JSON: {result2.StartsWith("{") && result2.EndsWith("}")}");
            Console.WriteLine();

            // 测试用例3：复杂嵌套JSON
            var testCase3 = @"```json
{
  ""config"": {
    ""nested"": {
      ""deep"": {
        ""value"": ""test""
      }
    }
  },
  ""array"": [
    {""item1"": ""value1""},
    {""item2"": ""value2""}
  ]
}
```";

            Console.WriteLine("测试用例3 - 复杂嵌套JSON:");
            var result3 = JsonExtractor.ExtractJson(testCase3);
            Console.WriteLine($"嵌套解析成功: {result3.Contains("nested") && result3.Contains("deep")}");
            Console.WriteLine();

            // 测试用例4：不含markdown的纯JSON
            var testCase4 = @"{""simple"": ""json"", ""without"": ""markdown""}";

            Console.WriteLine("测试用例4 - 纯JSON:");
            var result4 = JsonExtractor.ExtractJson(testCase4);
            Console.WriteLine($"纯JSON处理: {result4 == testCase4}");
            Console.WriteLine();

            // 测试用例5：您提供的示例
            var testCase5 = @"```json
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
    {""item"": ""apple pencil 笔套.skb"", ""target_folder"": ""3D_Models""}
  ]
}
```";

            Console.WriteLine("测试用例5 - 您的示例:");
            var result5 = JsonExtractor.CleanAndExtractJson(testCase5);
            Console.WriteLine($"提取的JSON包含new_folders: {result5.Contains("new_folders")}");
            Console.WriteLine($"提取的JSON包含move_operations: {result5.Contains("move_operations")}");
            Console.WriteLine($"提取的JSON包含中文文件名: {result5.Contains("成绩单")}");

            Console.WriteLine("\n=== 正则表达式模式 ===");
            Console.WriteLine("主要正则表达式: @\"(?s)\\{(?:[^{}]|(?<open>\\{)|(?<-open>\\}))*(?(open)(?!))\\}\"");
            Console.WriteLine("简化正则表达式: @\"(?s)\\{.*\\}\"");
            Console.WriteLine("Markdown清理: @\"```[\\w]*\\s*\" 和 @\"```\\s*\"");
        }

        /// <summary>
        /// 直接使用正则表达式的示例
        /// </summary>
        public static void ShowRegexPatterns()
        {
            Console.WriteLine("\n=== 正则表达式说明 ===");
            Console.WriteLine();

            Console.WriteLine("1. 平衡组模式（推荐）:");
            Console.WriteLine("   @\"(?s)\\{(?:[^{}]|(?<open>\\{)|(?<-open>\\}))*(?(open)(?!))\\}\"");
            Console.WriteLine("   - (?s): 单行模式，让.匹配换行符");
            Console.WriteLine("   - (?<open>\\{): 遇到{时压入open组");
            Console.WriteLine("   - (?<-open>\\}): 遇到}时从open组弹出");
            Console.WriteLine("   - (?(open)(?!)): 如果open组不为空则匹配失败");
            Console.WriteLine();

            Console.WriteLine("2. 简化模式:");
            Console.WriteLine("   @\"(?s)\\{.*\\}\"");
            Console.WriteLine("   - 匹配第一个{到最后一个}的所有内容");
            Console.WriteLine();

            Console.WriteLine("3. Markdown清理:");
            Console.WriteLine("   移除 ```json 和 ``` 标记");
            Console.WriteLine("   @\"```[\\w]*\\s*\" 和 @\"```\\s*\"");
        }
    }

    /// <summary>
    /// 程序入口点，用于独立测试
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            JsonExtractorTest.RunTests();
            JsonExtractorTest.ShowRegexPatterns();

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}