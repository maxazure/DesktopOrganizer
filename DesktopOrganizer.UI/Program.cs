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
        try
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
        catch (Exception ex)
        {
            // Show error message if startup fails
            MessageBox.Show($"应用程序启动失败:\n\n{ex.Message}\n\n详细信息:\n{ex}", 
                "启动错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
                services.AddSingleton<IPlanPreviewService, PlanPreviewService>();
                services.AddSingleton<IExecutionService, ExecutionEngine>();
                services.AddSingleton<IUndoService, UndoService>();
                services.AddSingleton<ICredentialService, CredentialService>();

                // Repositories
                services.AddSingleton<IPreferencesRepository, PreferencesRepository>();
                services.AddSingleton<PreferencesRepository>();
                services.AddSingleton<IModelProfileRepository, ModelProfileRepository>();
                services.AddSingleton<ModelProfileRepository>();

                // Application services
                services.AddSingleton<DesktopScanService>();
                services.AddSingleton<OrganizationService>();
                services.AddSingleton<PreferenceTemplateManager>();
                services.AddSingleton<PreferenceProcessor>();

                // LLM clients
                services.AddHttpClient<DeepSeekClient>();
                services.AddHttpClient<OpenAIClient>();
                services.AddHttpClient<AnthropicClient>();

                // LLM client factory/resolver
                // 恢复为真实 LLM 客户端（默认 DeepSeek，可根据需要切换）
                services.AddSingleton<ILLMClient>(provider =>
                {
                    return provider.GetRequiredService<DeepSeekClient>();
                });

                // UI Forms
                services.AddTransient<MainForm>();
                services.AddTransient<SettingsForm>();
                services.AddTransient<LogViewerForm>();
            });
    }
}