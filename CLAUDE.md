# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Desktop Organizer is a .NET 8 WinForms application that uses AI (Large Language Models) to intelligently organize Windows desktop files. The application follows Clean Architecture principles with a layered structure supporting multiple LLM providers (DeepSeek, OpenAI, Anthropic Claude).

## Project Architecture

### Layer Structure
```
DesktopOrganizer.sln
├─ DesktopOrganizer.UI/           # WinForms presentation layer
├─ DesktopOrganizer.App/          # Application services & orchestration
├─ DesktopOrganizer.Domain/       # Core business entities & interfaces
└─ DesktopOrganizer.Infrastructure/ # Technical implementation
```

### Key Technologies
- **.NET 8 WinForms** - Desktop UI framework
- **Microsoft.Extensions.*** - DI, hosting, logging
- **System.Text.Json** - JSON serialization
- **CredentialManagement** - Windows credential storage
- **HttpClient** - LLM API communications

## Core Workflow (Simplified)

1. **Automatic Scanning** → `FileSystemScanner` auto-scans desktop on startup
2. **Natural Language Input** → Users describe preferences in plain text
3. **AI Analysis** → LLM processes files + user preferences to generate plan
4. **Plan Preview** → Simple card-based preview of organization plan
5. **One-Click Execution** → `ExecutionEngine` performs file operations
6. **Undo Support** → `UndoService` provides one-click rollback

## Important File Locations

### Code Structure
- **Main Entry Point**: `DesktopOrganizer.UI/Program.cs`
- **Main Form**: `DesktopOrganizer.UI/MainForm.cs`
- **Core Services**: `DesktopOrganizer.App/Services/`
- **Domain Models**: `DesktopOrganizer.Domain/`
- **LLM Clients**: `DesktopOrganizer.Infrastructure/LLM/`

### Configuration Files
- User Preferences: `%APPDATA%\DesktopOrganizer\preferences.json`
- Model Profiles: `%APPDATA%\DesktopOrganizer\models.json`
- Application Log: `%USERPROFILE%\DesktopOrganizer.log`
- Undo Data: `%TEMP%\DesktopOrganizer_Undo.json`

## Key Domain Models

### Core Entities
- **Item**: Represents desktop files/folders with metadata
- **Plan**: AI-generated organization strategy with move operations
- **Preferences**: User-defined organization rules and settings
- **ModelProfile**: LLM provider configuration
- **MoveOperation**: Single file relocation operation

### Important Business Rules
- System files (Recycle Bin, etc.) are automatically excluded
- File size limits prevent processing oversized files
- Custom folder rules take precedence over AI suggestions
- All operations support undo functionality

## LLM Integration

### Supported Providers
- **DeepSeek** (default): `https://api.deepseek.com` - `deepseek-chat`
- **OpenAI**: `https://api.openai.com` - GPT-4 models
- **Anthropic**: `https://api.anthropic.com` - Claude models
- **LocalMock**: Development testing client

### Client Architecture
- **ILLMClient** interface with `BaseLLMClient` abstract class
- **Strategy Pattern**: Pluggable LLM implementations
- **JSON Response Processing**: Custom `JsonExtractor` utility
- **Secure Credentials**: Windows Credential Manager integration

## Development Guidelines

### Code Standards
- Files ≤ 300 lines for maintainability
- Methods ≤ 40 lines for focused functionality
- Async/await for all I/O operations
- Comprehensive error handling and logging
- Nullable reference types enabled

### Testing
- Use `LocalMockLLMClient` for development testing
- Mock responses stored in `respones.json`
- Architecture supports unit and integration testing

## Build and Development Commands

```bash
# Build the entire solution
dotnet build DesktopOrganizer.sln

# Run the application
dotnet run --project DesktopOrganizer.UI

# Build and run in release mode
dotnet build --configuration Release
dotnet run --project DesktopOrganizer.UI --configuration Release

# Clean build artifacts
dotnet clean
```

## Architecture Notes

### Clean Architecture Implementation
The solution follows Clean Architecture with strict dependency flow:
- **Domain** → No dependencies (pure business logic)
- **Application** → Depends on Domain only
- **Infrastructure** → Implements Domain interfaces
- **UI** → Orchestrates Application layer

### LLM Client Strategy Pattern
All LLM providers implement `ILLMClient` interface:
- `BaseLLMClient` provides common functionality
- Provider-specific clients (DeepSeek, OpenAI, Anthropic) handle API differences
- `LocalMockLLMClient` uses `respones.json` for development testing

### JSON Response Processing
LLM responses are processed through `JsonExtractor` utility:
- Handles markdown-wrapped JSON responses
- Uses regex patterns for balanced bracket matching
- Fallback patterns for malformed responses

## Debugging and Troubleshooting

### Logging
- **File Logging**: `%USERPROFILE%\DesktopOrganizer.log`
- **UI Log Viewer**: Built-in log viewer form
- **Console Logging**: Debug mode output
- **Log Levels**: Debug, Info, Warning, Error

### Common Issues
- **API Key Problems**: Check Windows Credential Manager
- **File Access Errors**: Verify file permissions
- **JSON Parsing Issues**: Check `JsonExtractor` regex patterns
- **LLM Timeout**: Adjust `TimeoutSeconds` in model profile

### Development Tools
- **Log Viewer**: Built-in real-time log monitoring
- **Mock Client**: Test without API costs
- **Simulation Mode**: Test operations without file changes

## Security Considerations

### Data Protection
- API keys stored in Windows Credential Manager
- No file content uploaded to LLM (only metadata)
- HTTPS-only communications
- Minimal file system permissions

### Safe Operations
- All operations support undo
- Confirmation dialogs for destructive actions
- Simulation mode for testing
- Comprehensive error handling

## Development Workflow

### Testing with Mock LLM
- Use `LocalMockLLMClient` for development without API costs
- Mock responses are stored in `respones.json` in the root directory
- Switch between real and mock clients in `Program.cs`

### Adding New LLM Provider
1. Create class inheriting from `BaseLLMClient` in `Infrastructure/LLM/`
2. Implement `ChatAsync` and `TestConnectionAsync` methods
3. Register in DI container in `Program.cs`
4. Add to model profile defaults in `ModelProfileRepository`

### Debugging
- Application logs to `%USERPROFILE%\DesktopOrganizer.log`
- Use built-in LogViewerForm for real-time log monitoring
- Set `DOTNET_ENVIRONMENT=Development` for detailed logging

### Key Constraints
- Files must be ≤ 300 lines for maintainability
- Methods must be ≤ 40 lines for focused functionality
- All I/O operations must be async
- Use dependency injection for all cross-cutting concerns

---

*Architecture follows Clean Architecture principles with strict separation of concerns and dependency inversion.*