# JSON提取正则表达式使用指南

## 问题描述

在API请求时，有些模型会返回包含markdown标签的响应，我们需要提取其中的纯JSON内容，去除不相关的markdown标记。

## 解决方案

### 核心正则表达式

#### 1. 平衡组模式（推荐）

```csharp
@"(?s)\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}"
```

**解释：**
- `(?s)` - 单行模式，让`.`匹配换行符
- `\{` - 匹配开始的大括号
- `(?:[^{}]|(?<open>\{)|(?<-open>\}))` - 匹配非大括号字符，或者平衡组操作
- `(?<open>\{)` - 遇到`{`时压入open组
- `(?<-open>\})` - 遇到`}`时从open组弹出
- `*(?(open)(?!))` - 如果open组不为空则匹配失败
- `\}` - 匹配结束的大括号

#### 2. 简化模式

```csharp
@"(?s)\{.*\}"
```

**解释：**
- `(?s)` - 单行模式
- `\{.*\}` - 匹配第一个`{`到最后一个`}`的所有内容

#### 3. Markdown清理

```csharp
// 移除开始标记（如 ```json）
@"```[\w]*\s*"

// 移除结束标记（如 ```）
@"```\s*"
```

## 使用方法

### 基础用法

```csharp
using DesktopOrganizer.Infrastructure.Utilities;

// 包含markdown的API响应
string apiResponse = @"```json
{
  ""name"": ""test"",
  ""value"": 123
}
```";

// 提取纯JSON
string cleanJson = JsonExtractor.CleanAndExtractJson(apiResponse);
```

### 高级用法

```csharp
// 选择使用简化模式（性能更好）
string cleanJson = JsonExtractor.CleanAndExtractJson(apiResponse, useBalancedPattern: false);

// 只移除markdown标记，不提取JSON
string withoutMarkdown = JsonExtractor.RemoveMarkdownCodeBlocks(apiResponse);

// 只提取JSON，不处理markdown
string jsonOnly = JsonExtractor.ExtractJson(cleanText);

// 验证结果是否为有效JSON格式
bool isValid = JsonExtractor.IsValidJsonFormat(cleanJson);
```

## 示例测试

### 输入示例

```
```json
{
  "new_folders": [
    "3D_Models",
    "Audio_Projects",
    "Design_Files",
    "Shortcuts"
  ],
  "move_operations": [
    {"item": "(成绩单）委托书.docx", "target_folder": "Documents"},
    {"item": "099999微信图片_20250326204211.png", "target_folder": "Pictures"},
    {"item": "123456_20250408104813.jpg", "target_folder": "Pictures"},
    {"item": "apple pencil 笔套.skb", "target_folder": "3D_Models"},
    {"item": "Blender 3.4.lnk", "target_folder": "Shortcuts"}
  ]
}
```

### 输出结果

```json
{
  "new_folders": [
    "3D_Models",
    "Audio_Projects",
    "Design_Files",
    "Shortcuts"
  ],
  "move_operations": [
    {"item": "(成绩单）委托书.docx", "target_folder": "Documents"},
    {"item": "099999微信图片_20250326204211.png", "target_folder": "Pictures"},
    {"item": "123456_20250408104813.jpg", "target_folder": "Pictures"},
    {"item": "apple pencil 笔套.skb", "target_folder": "3D_Models"},
    {"item": "Blender 3.4.lnk", "target_folder": "Shortcuts"}
  ]
}
```

## 特性

### ✅ 支持的功能

- ✅ 移除markdown代码块标记（```json 和 ```）
- ✅ 提取嵌套JSON对象
- ✅ 处理包含中文字符的JSON
- ✅ 支持复杂的JSON结构
- ✅ 平衡组确保正确匹配大括号
- ✅ 异常处理，失败时返回原文本
- ✅ 性能优化（编译后的正则表达式）

### 🔧 适用场景

- 🔧 LLM API响应清理
- 🔧 Markdown文档中的JSON提取
- 🔧 混合格式文本处理
- 🔧 API响应格式标准化

## 性能对比

| 模式       | 性能 | 准确性 | 适用场景     |
| ---------- | ---- | ------ | ------------ |
| 平衡组模式 | 中等 | 最高   | 复杂嵌套JSON |
| 简化模式   | 最快 | 高     | 大多数情况   |

## 错误处理

工具类包含完善的错误处理：

1. **输入验证** - 检查null和空字符串
2. **异常捕获** - 正则表达式错误时返回原文本
3. **格式验证** - 提供JSON格式验证方法

## 集成到项目

工具类位于：`DesktopOrganizer.Infrastructure.Utilities.JsonExtractor`

可以在任何需要处理API响应的地方使用：

```csharp
// 在LLM客户端中使用
public async Task<string> GetCleanJsonResponse(string prompt)
{
    var response = await CallLLMApi(prompt);
    return JsonExtractor.CleanAndExtractJson(response);
}
```

## 注意事项

1. **编码支持** - 支持UTF-8编码，包括中文字符
2. **大小写敏感** - JSON内容保持原样，不做大小写转换
3. **空白处理** - 自动去除前后空白字符
4. **嵌套限制** - 平衡组模式支持任意层级嵌套

## 测试验证

已通过以下测试场景：

- ✅ 标准markdown格式
- ✅ 包含其他文本的响应
- ✅ 复杂嵌套JSON
- ✅ 纯JSON文本（无markdown）
- ✅ 包含中文文件名的JSON
- ✅ 特殊字符处理

## 总结

这个正则表达式解决方案能够有效地从包含markdown标记的API响应中提取纯JSON内容，支持复杂的嵌套结构和中文字符，并提供了良好的错误处理和性能优化。