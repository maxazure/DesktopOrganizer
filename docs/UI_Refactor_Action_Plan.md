# Desktop Organizer UI 重构行动计划

## 📋 总体规划

### 重构目标
- 简化主界面，减少用户学习成本
- 用自然语言输入替代复杂配置
- 优化用户工作流程
- 现代化界面设计

### 预计工作量
- **总工时**：约 15-20 小时
- **完成周期**：3-5 个工作日
- **风险评估**：中等（涉及主要界面重构）

## 🎯 分阶段实施计划

### Phase 1: 基础架构准备 (4-5小时)
**目标**：为新界面创建必要的基础组件和数据结构

#### 1.1 创建新的数据模型 (1小时)
- [ ] 创建 `PreferenceTemplate` 类
- [ ] 创建 `PreferenceTemplateManager` 类
- [ ] 修改 `Preferences` 类，添加自然语言偏好字段
- [ ] 创建 `AppState` 枚举

#### 1.2 新增用户控件 (2-3小时)
- [ ] 创建 `FolderPreviewCard` UserControl
- [ ] 创建 `SimplifiedProgressIndicator` UserControl
- [ ] 创建 `PreferenceInputPanel` UserControl
- [ ] 设计并实现卡片式预览布局

#### 1.3 创建新的设置窗口 (1小时)
- [ ] 创建 `SettingsForm` 基础结构
- [ ] 实现标签页布局
- [ ] 迁移现有设置逻辑

### Phase 2: 主界面重构 (6-8小时)
**目标**：完全重构 MainForm，实现简化设计

#### 2.1 MainForm 布局重构 (3-4小时)
- [ ] 备份现有 MainForm.cs 和 MainForm.Designer.cs
- [ ] 清理现有复杂布局（移除 SplitContainer）
- [ ] 实现新的简化布局
- [ ] 添加偏好输入区域
- [ ] 实现预览卡片区域
- [ ] 添加大型操作按钮

#### 2.2 新增交互逻辑 (2-3小时)
- [ ] 实现偏好模板管理功能
- [ ] 添加自然语言偏好处理逻辑
- [ ] 实现新的状态管理 (AppState)
- [ ] 添加一键整理功能
- [ ] 实现预览调整功能

#### 2.3 移除旧功能 (1小时)
- [ ] 移除 PreferencesPane 嵌入逻辑
- [ ] 清理不需要的旧界面控件
- [ ] 简化工具栏，只保留设置按钮

### Phase 3: 业务逻辑调整 (3-4小时)
**目标**：调整服务层以支持新的交互模式

#### 3.1 提示词系统优化 (2小时)
- [ ] 修改 LLM 提示词模板
- [ ] 实现用户偏好与系统提示词的融合
- [ ] 添加模板偏好的处理逻辑
- [ ] 优化 JSON 响应解析

#### 3.2 服务层调整 (1-2小时)
- [ ] 修改 `OrganizationService` 支持新流程
- [ ] 调整 `PlanPreviewService` 输出格式
- [ ] 优化 `DesktopScanService` 自动扫描
- [ ] 确保 `UndoService` 兼容性

### Phase 4: 界面优化与测试 (2-3小时)
**目标**：完善界面细节，确保功能稳定

#### 4.1 视觉优化 (1小时)
- [ ] 应用统一的色彩方案
- [ ] 优化字体和间距
- [ ] 添加图标和视觉元素
- [ ] 实现响应式布局

#### 4.2 交互完善 (1小时)
- [ ] 添加键盘快捷键支持
- [ ] 实现工具提示
- [ ] 优化错误处理和用户反馈
- [ ] 添加加载动画

#### 4.3 功能测试 (1小时)
- [ ] 测试完整工作流程
- [ ] 验证模板功能
- [ ] 测试设置窗口
- [ ] 验证撤销功能

## 📂 文件修改清单

### 需要修改的文件

#### Domain 层
- `DesktopOrganizer.Domain/Preferences.cs` - 添加自然语言偏好字段
- `DesktopOrganizer.Domain/ModelProfile.cs` - 可能需要调整

#### Application 层
- `DesktopOrganizer.App/Services/OrganizationService.cs` - 调整工作流程
- `DesktopOrganizer.App/Services/PlanPreviewService.cs` - 调整输出格式

#### Infrastructure 层
- `DesktopOrganizer.Infrastructure/LLM/BaseLLMClient.cs` - 可能需要调整提示词处理
- 各个具体 LLM 客户端 - 确保兼容性

#### UI 层
- `DesktopOrganizer.UI/MainForm.cs` - **重大重构**
- `DesktopOrganizer.UI/MainForm.Designer.cs` - **重大重构**
- `DesktopOrganizer.UI/Program.cs` - 可能需要调整 DI 配置

### 需要新建的文件

#### Domain 层
- `PreferenceTemplate.cs` - 偏好模板数据模型
- `AppState.cs` - 应用状态枚举

#### Application 层
- `PreferenceTemplateManager.cs` - 模板管理服务
- `PreferenceProcessor.cs` - 偏好处理逻辑

#### UI 层
- `SettingsForm.cs` - 新的设置窗口
- `SettingsForm.Designer.cs` - 设置窗口布局
- `FolderPreviewCard.cs` - 文件夹预览卡片
- `FolderPreviewCard.Designer.cs` - 卡片布局
- `SimplifiedProgressIndicator.cs` - 简化进度指示器
- `SimplifiedProgressIndicator.Designer.cs` - 进度指示器布局
- `PreferenceInputPanel.cs` - 偏好输入面板
- `PreferenceInputPanel.Designer.cs` - 输入面板布局

## ⚠️ 风险与预防措施

### 潜在风险
1. **界面重构可能破坏现有功能** - 需要充分测试
2. **用户习惯改变** - 需要保留核心功能的访问路径
3. **性能影响** - 新的预览方式可能影响性能

### 预防措施
1. **备份策略**：
   - 重构前创建分支备份
   - 保留旧界面文件作为参考
   
2. **渐进式开发**：
   - 每个阶段完成后进行测试
   - 确保应用始终可运行
   
3. **兼容性保证**：
   - 保持现有配置文件格式兼容
   - 确保 API 和数据结构向后兼容

## 🔄 回滚计划

如果重构过程中遇到严重问题：

1. **立即回滚**：
   - 切换到备份分支
   - 恢复原始 MainForm 文件
   
2. **分析问题**：
   - 记录遇到的具体问题
   - 评估是否可以快速修复
   
3. **重新规划**：
   - 如果问题复杂，重新评估重构方案
   - 考虑分步骤更小的增量改进

## ✅ 完成标准

### 功能完整性
- [ ] 所有原有核心功能正常工作
- [ ] 新的简化流程完整可用
- [ ] 设置功能迁移完成
- [ ] 撤销功能正常

### 用户体验
- [ ] 界面简洁直观
- [ ] 操作流程简化
- [ ] 错误处理友好
- [ ] 性能无明显下降

### 代码质量
- [ ] 无编译错误或警告
- [ ] 代码结构清晰
- [ ] 遵循现有编码规范
- [ ] 添加必要的注释

---

**准备开始？** 请确认此行动计划，我将按照既定步骤开始实施UI重构。