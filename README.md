# Desktop Organizer - 桌面整理助手

A .NET 8 WinForms application that uses AI to intelligently organize your Windows desktop files.

## 🌟 Features

- **AI-Powered Organization**: Uses LLM (Large Language Models) to analyze and categorize your desktop files
- **Multiple LLM Providers**: Supports DeepSeek, OpenAI, and Anthropic Claude
- **User Preferences**: Customizable folder rules and organization preferences
- **Preview & Adjust**: Visual preview of organization plan with drag-and-drop adjustments
- **Safe Operations**: One-click undo functionality and simulation mode
- **Secure API Key Storage**: Uses Windows Credential Manager for secure key storage
- **Multilingual Support**: English and Chinese interface

## 🏗️ Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

```
DesktopOrganizer.sln
├─ DesktopOrganizer.UI             # WinForms presentation layer
├─ DesktopOrganizer.App            # Application services and orchestration
├─ DesktopOrganizer.Domain         # Core business entities and interfaces
└─ DesktopOrganizer.Infrastructure # Technical implementation (file system, HTTP, storage)
```

### Layer Responsibilities

- **Domain**: Core business logic, entities (Item, Plan, ModelProfile, Preferences), and interfaces
- **Infrastructure**: File system scanning, LLM API clients, credential management, repositories
- **Application**: Service orchestration, use case implementations, data transformation
- **UI**: WinForms controls, event handling, user interaction (no business logic)

## 🚀 Getting Started

### Prerequisites

- Windows 10 21H2+ or Windows 11
- .NET 8 Desktop Runtime
- Internet connection for LLM API calls
- API key for at least one supported LLM provider

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd desktop-cleaner-claude
   ```

2. Build the solution:
   ```bash
   dotnet build DesktopOrganizer.sln
   ```

3. Run the application:
   ```bash
   dotnet run --project DesktopOrganizer.UI
   ```

### 使用方法（极简流程）

1. **首次设置**：
   - 点击右上角"设置"按钮
   - 选择AI提供商并输入API密钥
   - 保存设置

2. **描述整理偏好**：
   - 在文本框中用自然语言描述你的需求
   - 例如："将设计相关快捷方式留在桌面，文档按项目分类"
   - 或点击"使用模板"选择预设偏好

3. **一键整理**：
   - 点击"🚀 开始整理"按钮
   - AI会自动分析桌面文件并生成整理方案
   - 预览整理计划，可拖拽调整

4. **执行或撤销**：
   - 点击"✅ 执行整理"完成文件整理
   - 如需要可点击"↶ 撤销操作"恢复原状

## 🔧 Configuration

### Model Profiles

The application supports multiple LLM providers:

- **DeepSeek**: `https://api.deepseek.com` with model `deepseek-chat`
- **OpenAI**: `https://api.openai.com` with models like `gpt-4`
- **Anthropic**: `https://api.anthropic.com` with models like `claude-3-sonnet-20240229`

### User Preferences

Stored in `%APPDATA%\DesktopOrganizer\preferences.json`:

```json
{
  "folderRules": {
    "Documents": [".pdf", ".doc", ".docx", ".txt"],
    "Pictures": [".jpg", ".jpeg", ".png", ".gif"],
    "Videos": [".mp4", ".avi", ".mkv", ".mov"]
  },
  "ignoreExtensions": [".tmp", ".log", ".cache"],
  "maxFileSizeBytes": 1073741824,
  "autoCreateFolders": true,
  "confirmBeforeExecution": true
}
```

### API Key Security

API keys are stored securely using Windows Credential Manager with the target format:
`DesktopOrganizer_{keyReference}`

## 🛠️ Development

### Project Structure

Each project follows the single responsibility principle:

- **Files ≤ 300 lines**: Maintains readability and testability
- **Methods ≤ 40 lines**: Ensures focused functionality
- **Async/await**: All I/O operations are asynchronous
- **Dependency Injection**: Clean separation and testability

### Key Design Patterns

- **Repository Pattern**: Data access abstraction
- **Factory Pattern**: LLM client creation
- **Observer Pattern**: Progress reporting
- **Command Pattern**: Undo operations
- **MVP Pattern**: UI separation

### Adding New LLM Providers

1. Create a new client class implementing `ILLMClient`:
   ```csharp
   public class NewProviderClient : BaseLLMClient
   {
       // Implement ChatAsync and TestConnectionAsync
   }
   ```

2. Register in DI container:
   ```csharp
   services.AddHttpClient<NewProviderClient>();
   ```

3. Add to provider dropdown in ProfileEditDialog

### Testing

The architecture supports comprehensive testing:

- **Unit Tests**: Domain entities and application services
- **Integration Tests**: Infrastructure components
- **UI Tests**: Can be added for critical user workflows

## 📁 File Locations

| File             | Location                                      | Purpose                         |
| ---------------- | --------------------------------------------- | ------------------------------- |
| User Preferences | `%APPDATA%\DesktopOrganizer\preferences.json` | Organization rules and settings |
| Model Profiles   | `%APPDATA%\DesktopOrganizer\models.json`      | LLM provider configurations     |
| Application Log  | `%USERPROFILE%\DesktopOrganizer.log`          | Runtime logs and errors         |
| Undo Data        | `%TEMP%\DesktopOrganizer_Undo.json`           | Last operation undo information |

## 🔒 Security & Privacy

- **Local Processing**: Files are never uploaded; only metadata is sent to LLM
- **API Key Encryption**: Stored in Windows Credential Manager
- **Minimal Permissions**: Only desktop and configuration directories access
- **HTTPS Only**: All API communications are encrypted
- **No Telemetry**: No usage data is collected or transmitted

## 🐛 Troubleshooting

### Common Issues

1. **API Key Not Found**:
   - Verify the key is set in Model Profile settings
   - Check Windows Credential Manager for the entry

2. **LLM Connection Failed**:
   - Verify internet connection
   - Check API key validity
   - Test connection in Model Profile dialog

3. **Files Not Moving**:
   - Check file permissions
   - Ensure files are not in use by other applications
   - Review execution logs for specific errors

4. **Undo Not Available**:
   - Undo data expires after application restart
   - Only the most recent operation can be undone

### Logging

Enable detailed logging by setting environment variable:
```bash
set DOTNET_ENVIRONMENT=Development
```

Check logs in: `%USERPROFILE%\DesktopOrganizer.log`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Follow the existing code style and architecture patterns
4. Ensure all files remain under 300 lines
5. Add tests for new functionality
6. Commit changes: `git commit -m 'Add amazing feature'`
7. Push to branch: `git push origin feature/amazing-feature`
8. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- Built with .NET 8 and WinForms
- LLM integration with DeepSeek, OpenAI, and Anthropic
- Windows Credential Manager for secure storage
- Clean Architecture principles by Robert Martin

---

**Desktop Organizer** - Making desktop organization intelligent and effortless! 🎯