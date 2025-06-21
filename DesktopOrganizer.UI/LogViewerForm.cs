using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DesktopOrganizer.UI;

/// <summary>
/// 日志查看器窗口
/// </summary>
public partial class LogViewerForm : Form
{
    private readonly ConcurrentQueue<LogEntry> _logEntries = new();
    private readonly System.Threading.Timer _updateTimer;
    private readonly object _lockObject = new();

    public LogViewerForm()
    {
        InitializeComponent();

        // 设置更新定时器
        _updateTimer = new System.Threading.Timer(UpdateLogDisplay, null, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(500));

        // 设置窗口属性
        this.Text = "日志查看器";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterParent;
    }

    private void InitializeComponent()
    {
        var tableLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };

        // 工具栏
        var toolStrip = new ToolStrip();
        var clearButton = new ToolStripButton("清空日志", null, ClearLogs_Click);
        var saveButton = new ToolStripButton("保存日志", null, SaveLogs_Click);
        var levelCombo = new ToolStripComboBox("日志级别")
        {
            Items = { "全部", "Debug", "Information", "Warning", "Error" },
            SelectedIndex = 0
        };
        levelCombo.SelectedIndexChanged += LogLevel_Changed;

        toolStrip.Items.AddRange(new ToolStripItem[] { clearButton, saveButton, new ToolStripSeparator(), levelCombo });

        // 日志显示区域
        var logTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            BackColor = Color.Black,
            ForeColor = Color.White,
            Font = new Font("Consolas", 9F),
            ScrollBars = RichTextBoxScrollBars.Both,
            WordWrap = false
        };

        tableLayout.Controls.Add(toolStrip, 0, 0);
        tableLayout.Controls.Add(logTextBox, 0, 1);
        tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        this.Controls.Add(tableLayout);

        // 保存引用
        LogTextBox = logTextBox;
        LevelComboBox = levelCombo;
    }

    private RichTextBox LogTextBox { get; set; } = null!;
    private ToolStripComboBox LevelComboBox { get; set; } = null!;

    public void AddLogEntry(LogLevel level, string category, string message, Exception? exception = null)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Category = category,
            Message = message,
            Exception = exception
        };

        _logEntries.Enqueue(entry);

        // 限制日志条目数量
        while (_logEntries.Count > 1000)
        {
            _logEntries.TryDequeue(out _);
        }
    }

    private void UpdateLogDisplay(object? state)
    {
        if (InvokeRequired)
        {
            Invoke(new Action(() => UpdateLogDisplay(state)));
            return;
        }

        lock (_lockObject)
        {
            var newEntries = new List<LogEntry>();
            while (_logEntries.TryDequeue(out var entry))
            {
                newEntries.Add(entry);
            }

            if (newEntries.Count == 0) return;

            var selectedLevel = GetSelectedLogLevel();
            var filteredEntries = newEntries.Where(e => ShouldShowLogLevel(e.Level, selectedLevel));

            foreach (var entry in filteredEntries)
            {
                AppendLogEntry(entry);
            }

            // 自动滚动到底部
            LogTextBox.SelectionStart = LogTextBox.Text.Length;
            LogTextBox.ScrollToCaret();
        }
    }

    private void AppendLogEntry(LogEntry entry)
    {
        var timestamp = entry.Timestamp.ToString("HH:mm:ss.fff");
        var level = entry.Level.ToString().ToUpper().PadRight(4);
        var category = entry.Category.Length > 30 ? entry.Category.Substring(entry.Category.Length - 30) : entry.Category;

        // 设置颜色
        Color color = entry.Level switch
        {
            LogLevel.Error => Color.Red,
            LogLevel.Warning => Color.Yellow,
            LogLevel.Information => Color.LightGreen,
            LogLevel.Debug => Color.LightBlue,
            _ => Color.White
        };

        LogTextBox.SelectionStart = LogTextBox.TextLength;
        LogTextBox.SelectionLength = 0;
        LogTextBox.SelectionColor = Color.Gray;
        LogTextBox.AppendText($"[{timestamp}] ");

        LogTextBox.SelectionColor = color;
        LogTextBox.AppendText($"{level} ");

        LogTextBox.SelectionColor = Color.Cyan;
        LogTextBox.AppendText($"{category}: ");

        LogTextBox.SelectionColor = Color.White;
        LogTextBox.AppendText($"{entry.Message}");

        if (entry.Exception != null)
        {
            LogTextBox.SelectionColor = Color.Red;
            LogTextBox.AppendText($"\n    异常: {entry.Exception.Message}");
        }

        LogTextBox.AppendText("\n");
    }

    private LogLevel GetSelectedLogLevel()
    {
        return LevelComboBox.SelectedIndex switch
        {
            1 => LogLevel.Debug,
            2 => LogLevel.Information,
            3 => LogLevel.Warning,
            4 => LogLevel.Error,
            _ => LogLevel.Trace
        };
    }

    private bool ShouldShowLogLevel(LogLevel entryLevel, LogLevel selectedLevel)
    {
        if (selectedLevel == LogLevel.Trace) return true; // 显示全部
        return entryLevel >= selectedLevel;
    }

    private void ClearLogs_Click(object? sender, EventArgs e)
    {
        LogTextBox.Clear();
        while (_logEntries.TryDequeue(out _)) { }
    }

    private void SaveLogs_Click(object? sender, EventArgs e)
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = "日志文件 (*.log)|*.log|文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
            DefaultExt = "log",
            FileName = $"DesktopOrganizer_{DateTime.Now:yyyyMMdd_HHmmss}.log"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                File.WriteAllText(saveDialog.FileName, LogTextBox.Text);
                MessageBox.Show("日志已保存", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存日志失败: {ex.Message}", "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void LogLevel_Changed(object? sender, EventArgs e)
    {
        // 重新显示现有日志
        LogTextBox.Clear();
        // 这里可以重新加载已过滤的日志
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _updateTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
}
