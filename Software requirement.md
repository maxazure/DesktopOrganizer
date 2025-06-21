# 桌面整理助手 Desktop Organizer

.NET 8 WinForms 软件需求规格说明书（SRS）
版本 v1.2 发布日期 2025-06-21

---

## 目录

1. **引言**
2. **项目概述**
3. **术语与缩写**
4. **总体需求**
5. **功能需求明细**
6. **非功能需求**
7. **系统架构与分层设计**
8. **数据设计**
9. **用户界面设计**
10. **LLM 提示词模板**
11. **安全与隐私**
12. **部署与运维要求**
13. **扩展规划**
14. **附录：文件与目录约定**

---

## 1 引言

* **目标受众**：产品经理、系统架构师、.NET8 WinForms 开发人员、测试工程师、技术文档撰写者。
* **目标**：本文档统一、完整地描述桌面整理助手的所有需求，指导后续设计、实现、测试和维护。

---

## 2 项目概述

| 项目         | 说明                                                                                                          |
| ------------ | ------------------------------------------------------------------------------------------------------------- |
| **产品名称** | 桌面整理助手 Desktop Organizer                                                                                |
| **核心价值** | 利用大型语言模型（LLM）自动分析并分类整理 Windows 桌面文件，保持桌面简洁；支持用户自定义偏好与多 LLM 提供商。 |
| **目标用户** | Windows 10/11 桌面用户（尤其文件繁杂者）、对 AI 自动整理有需求的人群。                                        |
| **运行环境** | - Windows 10 21H2+ / Windows 11<br>- .NET 8 Desktop Runtime (x64)<br>- 可访问互联网（调用 LLM API）           |
| **主要技术** | .NET 8 WinForms、HTTP/HTTPS、JSON、Windows Credential Manager、Task Scheduler。                               |

---

## 3 术语与缩写

| 术语/缩写        | 解释                                                                          |
| ---------------- | ----------------------------------------------------------------------------- |
| LLM              | Large Language Model，大型语言模型（DeepSeek、OpenAI、Anthropic Claude 等）。 |
| Desktop Root     | `%USERPROFILE%\Desktop`，即当前用户桌面根目录。                               |
| Provider Profile | LLM 提供商配置档，包含 Base URL、Model ID、API Key 等信息。                   |
| Preferences JSON | 用户整理偏好文件 `%APPDATA%\DesktopOrganizer\preferences.json`。              |
| Models JSON      | Provider Profile 列表文件 `%APPDATA%\DesktopOrganizer\models.json`。          |

---

## 4 总体需求

1. **智能整理**：扫描桌面 → 调用 LLM（结合用户偏好） → 生成“文件-目标文件夹”映射 → 预览 → 执行移动。
2. **用户偏好**：UI 中可编辑个人分类偏好，LLM 必须优先遵循。
3. **多模型支持**：用户可配置 DeepSeek、OpenAI、Anthropic 等兼容 API，热切换且安全存储密钥。
4. **安全可靠**：操作前可模拟（试运行），执行后可一键撤回。
5. **良好 UX**：所见即所得的整理预览、流式日志、可拖拽调节分类结果。
6. **高可维护性**：严格的分层与文件拆分，UI 与业务逻辑解耦；核心逻辑可单元测试。

---

## 5 功能需求明细

| 编号    | 模块                | 功能描述                                                                                                                              | 关键接口/存储                                    |
| ------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------ |
| **F0**  | **用户偏好**        | - 在侧边栏编辑偏好（表单/JSON 切换）<br>- 预设快速规则模板<br>- 自动保存至 `preferences.json`<br>- 加载并注入 LLM 提示词              | `IPreferenceService`                             |
| **F1**  | **桌面扫描**        | - 递归枚举桌面文件与一级子文件夹<br>- 采集元数据：名称、扩展名、大小、修改时间、快捷方式标记                                          | `IDesktopScanService`                            |
| **F2**  | **LLM 交互**        | - 构造提示词（含偏好与桌面 dump）<br>- 根据当前 Provider Profile 调用 `/v1/chat/completions`（支持 `stream=true`）<br>- 实时日志流→UI | `ILLMClient`                                     |
| **F3**  | **分类预览**        | - 解析 LLM JSON 输出<br>- TreeView 展示“拟建文件夹 → 其中文件”<br>- 拖拽/右键微调，双击改名                                           | `IPlanPreviewView`                               |
| **F4**  | **执行与进度**      | - 创建文件夹（若不存在）<br>- 移动文件/文件夹，显示整体与当前进度条<br>- 取消或失败时回滚<br>- 冲突时自动重命名或询问                 | `IExecutionService`                              |
| **F5**  | **撤销与日志**      | - 记录最近一次移动的源→目标映射<br>- 提供“撤销”按钮（一次）<br>- `DesktopOrganizer.log` 持久化操作与错误                              | `IUndoService`                                   |
| **F6’** | **模型与 API 配置** | - Provider Profile CRUD<br>- API Key 加密入 Credential Manager<br>- \[测试连接] 按钮<br>- 导入/导出 JSON                              | `IModelProfileService`, `ModelProfileRepository` |
| **F7**  | **通用设置**        | - 忽略后缀、最大文件大小、超时等<br>- 自动更新检查（GitHub Releases）<br>- 语言选择（自动随系统）                                     | `ISettingsService`                               |

---

## 6 非功能需求

| 分类       | 指标                                                                                    |
| ---------- | --------------------------------------------------------------------------------------- |
| **性能**   | 桌面 ≤ 5000 项：扫描 ≤ 2 s；分类预览生成 ≤ 2 s；移动速度受磁盘 IO。                     |
| **可用性** | 所有长任务均在后台线程；UI 不冻结。                                                     |
| **可恢复** | 任何移动失败自动回滚；撤销功能完整。                                                    |
| **安全**   | API Key 仅存储在 Credential Manager，加密传输；程序仅对桌面目录及自身配置路径有写权限。 |
| **可维护** | 单文件 ≤ 300 行；方法 ≤ 40 行；单元测试覆盖 Domain 与 App 层关键逻辑。                  |
| **国际化** | 支持 `zh-CN`、`en-US`；资源文件 `.resx`。                                               |

---

## 7 系统架构与分层设计

### 7.1 解决方案结构

```
DesktopOrganizer.sln
├─ DesktopOrganizer.UI             (WinForms)
│   ├─ MainForm.cs / MainForm.Designer.cs
│   ├─ PreferencesPane.cs
│   └─ ModelProfileDialog.cs
├─ DesktopOrganizer.App            (Application layer)
│   └─ Services / DTO / Commands
├─ DesktopOrganizer.Domain         (Entities, ValueObjects)
│   ├─ Item.cs       ─ 文件/文件夹实体
│   ├─ Plan.cs       ─ 移动方案
│   └─ Interfaces.cs ─ 领域接口
└─ DesktopOrganizer.Infrastructure (技术细节)
    ├─ FileSystemScanner.cs
    ├─ ExecutionEngine.cs
    ├─ LLM
    │   ├─ ILLMClient.cs
    │   ├─ DeepSeekClient.cs
    │   ├─ OpenAIClient.cs
    │   └─ AnthropicClient.cs
    └─ Repositories
        ├─ PreferencesRepo.cs
        └─ ModelProfileRepo.cs
```

### 7.2 分层职责

| 层级               | 职责                                                       | 依赖                                |
| ------------------ | ---------------------------------------------------------- | ----------------------------------- |
| **UI**             | 仅负责输入/展示；把事件交给 Presenter；绝不含业务逻辑。    | App                                 |
| **App**            | 协调服务；组合 Domain 对象；实现用例。                     | Domain + Infrastructure（接口定义） |
| **Domain**         | 业务核心：实体、值对象、领域服务、不依赖任何技术栈。       | 无外部依赖                          |
| **Infrastructure** | 文件系统、HTTP、持久化、日志等技术实现；实现 Domain 接口。 | 第三方库                            |

> **依赖原则**：高层不依赖底层实现，仅依赖接口。注册在 `HostBuilder` 中完成 DI。

---

## 8 数据设计

| 文件                  | 路径                                          | 示例                                                                      |
| --------------------- | --------------------------------------------- | ------------------------------------------------------------------------- |
| **偏好文件**          | `%APPDATA%\DesktopOrganizer\preferences.json` | `json { "Pictures": ["*.jpg","*.png"], "Documents":["*.docx","*.pdf"] } ` |
| **Provider Profiles** | `%APPDATA%\DesktopOrganizer\models.json`      | 见第 6’ 节示例。                                                          |
| **操作日志**          | `%USERPROFILE%\DesktopOrganizer.log`          | 文本/JSON，每行一条。                                                     |
| **撤销记录**          | 内存 + 临时 JSON（执行完写入 `%TEMP%`）。     |                                                                           |

---

## 9 用户界面设计

### 9.1 主窗口（文字线框）

```
┌───────────── 桌面整理助手 ─────────────┐
│ [扫描] [分析] [执行] [撤销]           │  当前模型: DeepSeek Chat ▼
├──────────┬──────────┬──────────┐
│① 原始列表│② 模型日志│③ 整理预览│
│ ListView │ RichText │ TreeView │
│          │ (流式)   │(可拖拽) │
├──────────┴──────────┴──────────┤
│④ 用户偏好面板                        │
│┌───────────────┐                  │
││··快速模板··   │                  │
││··JSON 编辑··  │                  │
│└───────────────┘                  │
└───────────────────────────────────┘
状态栏：Ready | Items: 234 | Model: deepseek-chat @ api.deepseek.com
```

### 9.2 模型与 API 设置对话框

见 v1.2 UI 文字线框（§ 模型与 API 配置）。

---

## 10 LLM 提示词模板（最终版）

> 由 `ILLMClient.BuildPrompt(ModelProfile profile, string jsonDesktop, string jsonPrefs)` 生成。

````text
System:
You are an expert desktop organization assistant.

User:
Below is my Windows desktop dump and my personal preferences.  
Please classify each item, create up to 12 folders, and return JSON only.

### Personal preferences (high priority)
```json
{{user_preferences}}
````

### Desktop dump (root = %USERPROFILE%\Desktop)

```json
{{json_desktop}}
```

Rules:

* Follow preferences whenever applicable.
* Keep system icons untouched; max depth = 1.
* Schema to return:
  {
  "new\_folders": \["<FolderName>", ...],
  "move\_operations": \[
  {"item":"<OriginalName>","target\_folder":"<FolderName>"},
  ...
  ]
  }
  Return **only** valid JSON.

```

---

## 11 安全与隐私  

1. **密钥存储**：API Key 使用 Windows Credential Manager (`CredentialType.Generic`)；字段名=`DesktopOrganizer_{profileName}`。  
2. **网络传输**：仅 HTTPS；超时默认 60 s。  
3. **权限最小化**：程序仅对桌面目录及自身配置路径写权限；其余操作均受限。  
4. **错误脱敏**：日志中不写入 API Key、桌面真实路径。  

---

## 12 部署与运维要求  

| 项目         | 说明                                                                                   |
| ------------ | -------------------------------------------------------------------------------------- |
| **发布模式** | .NET 8 自包含 (self-contained) 单文件发布；`publishSingleFile=true`；`TrimMode=link`。 |
| **安装**     | Inno Setup/NSIS 构建 MSI/EXE；默认安装 `%ProgramFiles%\DesktopOrganizer`。             |
| **自动更新** | 启动时检查 GitHub Releases RSS；若有新版本弹窗提示并跳转浏览器下载。                   |
| **日志轮换** | 日志文件 > 5 MB 自动归档为 `DesktopOrganizer_yyyyMMdd.log`.zip。                       |
| **计划任务** | 可在设置中启用“开机自动整理（试运行）”，实现为 Task Scheduler。                        |

---

## 13 扩展规划  

1. **Shell 扩展**：在文件/文件夹右键菜单中“使用桌面整理助手”。  
2. **多层分类**：允许 LLM 建议二级子文件夹（受限深度 = 2）。  
3. **模型微调**：上传历史整理结果微调个人模型。  
4. **跨平台**：未来发布 .NET MAUI 版以支持 macOS Desktop。  

---

## 14 附录：文件与目录约定  

| 路径                                          | 作用              |
| --------------------------------------------- | ----------------- |
| `%APPDATA%\DesktopOrganizer\preferences.json` | 用户偏好          |
| `%APPDATA%\DesktopOrganizer\models.json`      | Provider Profiles |
| `%USERPROFILE%\DesktopOrganizer.log`          | 运行日志          |
| `%TEMP%\DesktopOrganizer_Undo.json`           | 最近一次撤销映射  |
| `%ProgramFiles%\DesktopOrganizer\*`           | 主程序及依赖      |

