using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DesktopOrganizer.App.Services;
using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure;
using DesktopOrganizer.Infrastructure.LLM;
using DesktopOrganizer.Infrastructure.Repositories;
using DesktopOrganizer.Infrastructure.Logging;
using DesktopOrganizer.UI.Logging;

namespace DesktopOrganizer.UI;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Build host with dependency injection
        var host = CreateHostBuilder().Build();

        // Get the main form from DI container
        var mainForm = host.Services.GetRequiredService<MainForm>();

        // Run the application
        Application.Run(mainForm);
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();

                // 添加文件日志提供程序
                var logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "DesktopOrganizer.log");
                logging.AddProvider(new FileLoggerProvider(logFilePath, LogLevel.Information));

                // 添加全局日志查看器提供程序
                logging.AddProvider(new GlobalLogViewerProvider());

                // 只在控制台模式下添加控制台日志
#if DEBUG
                logging.AddConsole();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Debug);
#else
                logging.SetMinimumLevel(LogLevel.Information);
#endif

                // 为特定组件设置详细日志
                logging.AddFilter("DesktopOrganizer.Infrastructure.LLM", LogLevel.Debug);
                logging.AddFilter("DesktopOrganizer.App.Services", LogLevel.Debug);
                logging.AddFilter("DesktopOrganizer.Infrastructure.CredentialService", LogLevel.Information);
            })
            .ConfigureServices((context, services) =>
            {
                // Domain services
                services.AddSingleton<IDesktopScanService, FileSystemScanner>();
                services.AddSingleton<IPlanPreviewService>(provider =>
                    new PlanPreviewService(provider.GetService<ILogger<PlanPreviewService>>()));
                services.AddSingleton<IExecutionService, ExecutionEngine>();
                services.AddSingleton<IUndoService, UndoService>();
                services.AddSingleton<ICredentialService, CredentialService>();

                // Repositories
                services.AddSingleton<IPreferencesRepository, PreferencesRepository>();
                services.AddSingleton<IModelProfileRepository, ModelProfileRepository>();

                // Application services
                services.AddSingleton<DesktopScanService>();
                services.AddSingleton<OrganizationService>();

                // LLM clients
                services.AddHttpClient<DeepSeekClient>();
                services.AddHttpClient<OpenAIClient>();
                services.AddHttpClient<AnthropicClient>();

                // LLM client factory/resolver
                // 使用本地模拟LLM客户端，便于测试
                services.AddSingleton<ILLMClient>(provider =>
                {
                    return new LocalMockLLMClient("respones.json");
                });

                // UI Forms
                services.AddTransient<MainForm>();
                services.AddTransient<PreferencesPane>();
                services.AddTransient<ModelProfileDialog>();
                services.AddTransient<LogViewerForm>();
            });
    }
}