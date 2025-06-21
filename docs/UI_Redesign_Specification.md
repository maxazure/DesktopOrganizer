# Desktop Organizer UI 重构设计规范

## 设计目标

- **极简化**：减少用户学习成本，专注核心功能
- **自然交互**：用文字描述替代复杂配置界面
- **流程优化**：简化为4步操作流程
- **现代化**：清爽的视觉设计和友好的交互体验

## 新界面结构

### 主界面 (MainForm)

#### 布局结构
```
┌─ 标题栏 ─────────────────────────────────────┐
│ Desktop Organizer        [设置] [帮助] [最小化] [关闭] │
├─ 主内容区 ──────────────────────────────────┤
│                                              │
│ 📁 当前桌面文件： XX 个文件                  │
│                                              │
│ ┌─ 偏好输入区 ──────────────────────────────┐ │
│ │ 💬 告诉我你的整理偏好：                    │ │
│ │ [大文本框 - 多行输入]                      │ │
│ │                      [清空] [使用模板▼]    │ │
│ └─────────────────────────────────────────┘ │
│                                              │
│             [🚀 开始整理] (大按钮)           │
│                                              │
│ ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ │
│                                              │
│ 📊 整理计划预览：                           │
│ [预览卡片区域 - FlowLayoutPanel]             │
│                                              │
│ [✏️调整计划] [✅执行整理] [↶撤销操作]        │
│                                              │
│ [进度条和状态信息]                          │
└─────────────────────────────────────────────┘
```

#### 控件规格

1. **文件数量显示**
   - Label，大字体，图标 + 文字
   - 实时更新桌面文件数量

2. **偏好输入区**
   - GroupBox 包装
   - TextBox：多行，自动换行，占据主要空间
   - Button：清空按钮
   - ComboBox：模板下拉选择

3. **开始整理按钮**
   - 大型按钮，居中显示
   - 图标 + 文字，明显的视觉焦点

4. **预览区域**
   - FlowLayoutPanel 布局
   - 自定义 UserControl 显示文件夹卡片

5. **操作按钮组**
   - 三个并排按钮
   - 根据状态启用/禁用

### 设置窗口 (SettingsForm)

#### 标签页设计
- **AI模型** - 模型选择、API密钥配置
- **界面设置** - 语言、主题、界面选项
- **模板管理** - 偏好模板的增删改
- **高级选项** - 日志级别、超时设置等

## 新增组件设计

### 1. PreferenceTemplateManager
```csharp
public class PreferenceTemplateManager
{
    public List<PreferenceTemplate> Templates { get; set; }
    public void LoadDefaultTemplates();
    public void SaveCustomTemplate(string name, string content);
    public void DeleteTemplate(string name);
}

public class PreferenceTemplate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public bool IsBuiltIn { get; set; }
}
```

### 2. FolderPreviewCard (UserControl)
```csharp
public partial class FolderPreviewCard : UserControl
{
    public string FolderName { get; set; }
    public List<string> FileList { get; set; }
    public bool KeepOnDesktop { get; set; }
    public event EventHandler<FolderCardEventArgs> CardClicked;
}
```

### 3. SimplifiedProgressIndicator
```csharp
public partial class SimplifiedProgressIndicator : UserControl
{
    public string StatusText { get; set; }
    public int ProgressPercentage { get; set; }
    public bool ShowCancelButton { get; set; }
    public event EventHandler CancelRequested;
}
```

## 数据流更新

### 偏好处理逻辑
```csharp
public class PreferenceProcessor
{
    public string CombineWithPrompt(string userPreference, List<Item> items)
    {
        var systemPrompt = GetSystemPrompt();
        var fileContext = GenerateFileContext(items);
        var userContext = $"用户偏好：{userPreference}";
        
        return $"{systemPrompt}\n\n{fileContext}\n\n{userContext}";
    }
}
```

### 状态管理简化
```csharp
public enum AppState
{
    Ready,          // 准备状态，可以输入偏好
    Processing,     // AI 分析中
    PreviewReady,   // 预览准备好，可以调整和执行
    Executing,      // 执行中
    Completed       // 完成，可以撤销
}
```

## 视觉设计规范

### 色彩方案
- **主色调**：#2563EB (蓝色)
- **成功色**：#059669 (绿色)
- **警告色**：#D97706 (橙色)
- **背景色**：#F9FAFB (浅灰)
- **文字色**：#1F2937 (深灰)

### 字体规范
- **标题**：微软雅黑 14pt Bold
- **正文**：微软雅黑 10pt Regular
- **按钮**：微软雅黑 11pt Medium

### 间距规范
- **页边距**：20px
- **组件间距**：15px
- **按钮高度**：35px
- **大按钮高度**：45px

## 交互行为规范

### 状态转换
1. **启动** → 自动扫描桌面 → **Ready**
2. **Ready** + 点击开始整理 → **Processing**
3. **Processing** + AI完成 → **PreviewReady**
4. **PreviewReady** + 点击执行 → **Executing**
5. **Executing** + 完成 → **Completed**

### 用户反馈
- **加载状态**：进度条 + 状态文字
- **错误处理**：Toast 通知 + 详细错误对话框
- **操作确认**：重要操作前显示确认对话框

### 键盘快捷键
- **Ctrl+Enter**：开始整理
- **Ctrl+Z**：撤销操作
- **F5**：刷新桌面扫描
- **Ctrl+,**：打开设置

## 可访问性要求

- 所有控件支持键盘导航
- 重要操作有工具提示
- 支持高对比度主题
- 文字大小可调节

## 性能优化

- 桌面扫描异步执行，不阻塞UI
- 预览卡片懒加载
- 大量文件时分页显示
- 内存使用优化，及时释放资源

---

*此文档定义了Desktop Organizer UI重构的完整规范，所有开发工作应遵循此设计。*