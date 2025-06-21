using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DesktopOrganizer.UI.Logging;

/// <summary>
/// 全局日志查看器提供程序，用于将日志同时发送到文件和UI查看器
/// </summary>
public class GlobalLogViewerProvider : ILoggerProvider
{
    private static readonly ConcurrentBag<LogViewerForm> _logViewers = new();
    private readonly ConcurrentDictionary<string, GlobalLogViewerLogger> _loggers = new();

    public static void RegisterLogViewer(LogViewerForm logViewer)
    {
        _logViewers.Add(logViewer);
    }

    public static void UnregisterLogViewer(LogViewerForm logViewer)
    {
        // ConcurrentBag doesn't support removal, but we can check for disposed forms
        // The logger will handle disposed forms automatically
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new GlobalLogViewerLogger(name));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }

    private class GlobalLogViewerLogger : ILogger
    {
        private readonly string _categoryName;

        public GlobalLogViewerLogger(string categoryName)
        {
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Debug;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);

            // 发送到所有活跃的日志查看器
            foreach (var viewer in _logViewers)
            {
                try
                {
                    if (!viewer.IsDisposed)
                    {
                        viewer.AddLogEntry(logLevel, _categoryName, message, exception);
                    }
                }
                catch
                {
                    // 忽略已释放的查看器
                }
            }
        }
    }
}