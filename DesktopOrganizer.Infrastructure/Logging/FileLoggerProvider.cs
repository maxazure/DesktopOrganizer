using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DesktopOrganizer.Infrastructure.Logging;

/// <summary>
/// 文件日志提供程序
/// </summary>
public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _logFilePath;
    private readonly LogLevel _minLevel;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();
    private readonly object _lockObject = new();

    public FileLoggerProvider(string logFilePath, LogLevel minLevel = LogLevel.Information)
    {
        _logFilePath = logFilePath;
        _minLevel = minLevel;

        // 确保日志目录存在
        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _logFilePath, _minLevel, _lockObject));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}

/// <summary>
/// 文件日志记录器
/// </summary>
public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly string _logFilePath;
    private readonly LogLevel _minLevel;
    private readonly object _lockObject;

    public FileLogger(string categoryName, string logFilePath, LogLevel minLevel, object lockObject)
    {
        _categoryName = categoryName;
        _logFilePath = logFilePath;
        _minLevel = minLevel;
        _lockObject = lockObject;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logEntry = $"[{timestamp}] [{logLevel.ToString().ToUpper()}] {_categoryName}: {message}";

        if (exception != null)
        {
            logEntry += $"\n    异常: {exception.Message}";
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                logEntry += $"\n    堆栈跟踪: {exception.StackTrace}";
            }
        }

        try
        {
            lock (_lockObject)
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }

            // 检查日志文件大小，如果超过5MB则进行轮换
            CheckAndRotateLogFile();
        }
        catch (Exception ex)
        {
            // 避免在日志记录本身出错时产生无限循环
            System.Diagnostics.Debug.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    private void CheckAndRotateLogFile()
    {
        try
        {
            var fileInfo = new FileInfo(_logFilePath);
            if (fileInfo.Exists && fileInfo.Length > 5 * 1024 * 1024) // 5MB
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var directory = Path.GetDirectoryName(_logFilePath) ?? "";
                var fileName = Path.GetFileNameWithoutExtension(_logFilePath);
                var extension = Path.GetExtension(_logFilePath);
                var archivedPath = Path.Combine(directory, $"{fileName}_{timestamp}{extension}");

                lock (_lockObject)
                {
                    if (File.Exists(_logFilePath))
                    {
                        File.Move(_logFilePath, archivedPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error rotating log file: {ex.Message}");
        }
    }
}