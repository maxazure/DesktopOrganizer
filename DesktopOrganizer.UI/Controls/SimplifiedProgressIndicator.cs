using System.ComponentModel;

namespace DesktopOrganizer.UI.Controls;

/// <summary>
/// Simplified progress indicator for showing operation status
/// </summary>
public partial class SimplifiedProgressIndicator : UserControl
{
    public event EventHandler? CancelRequested;

    private string _statusText = "准备中...";
    private int _progressPercentage = 0;
    private bool _showCancelButton = false;

    public SimplifiedProgressIndicator()
    {
        InitializeComponent();
        ApplyModernStyling();
    }

    [Category("Appearance")]
    [Description("The status text to display")]
    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            lblStatus.Text = value;
        }
    }

    [Category("Behavior")]
    [Description("The progress percentage (0-100)")]
    public int ProgressPercentage
    {
        get => _progressPercentage;
        set
        {
            _progressPercentage = Math.Max(0, Math.Min(100, value));
            progressBar.Value = _progressPercentage;
        }
    }

    [Category("Behavior")]
    [Description("Whether to show the cancel button")]
    public bool ShowCancelButton
    {
        get => _showCancelButton;
        set
        {
            _showCancelButton = value;
            btnCancel.Visible = value;
        }
    }

    [Category("Behavior")]
    [Description("Whether the progress bar is in indeterminate mode")]
    public bool IsIndeterminate
    {
        get => progressBar.Style == ProgressBarStyle.Marquee;
        set
        {
            progressBar.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            if (value)
            {
                progressBar.MarqueeAnimationSpeed = 30;
            }
        }
    }

    private void ApplyModernStyling()
    {
        // Modern color scheme
        BackColor = Color.FromArgb(249, 250, 251);
        progressBar.ForeColor = Color.FromArgb(37, 99, 235); // Blue
        
        // Round button corners
        btnCancel.Region = CreateRoundedRegion(btnCancel.Size, 4);
    }

    private Region CreateRoundedRegion(Size size, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(size.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(size.Width - radius, size.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, size.Height - radius, radius, radius, 90, 90);
        path.CloseAllFigures();
        return new Region(path);
    }

    public void SetProgress(int percentage, string status)
    {
        ProgressPercentage = percentage;
        StatusText = status;
    }

    public void SetIndeterminateProgress(string status)
    {
        IsIndeterminate = true;
        StatusText = status;
    }

    public void SetComplete(string status = "完成")
    {
        IsIndeterminate = false;
        ProgressPercentage = 100;
        StatusText = status;
        ShowCancelButton = false;
    }

    public void SetError(string status = "出现错误")
    {
        IsIndeterminate = false;
        StatusText = status;
        ShowCancelButton = false;
        
        // Change colors to indicate error
        lblStatus.ForeColor = Color.FromArgb(239, 68, 68); // Red
    }

    public void Reset()
    {
        IsIndeterminate = false;
        ProgressPercentage = 0;
        StatusText = "准备中...";
        ShowCancelButton = false;
        
        // Reset colors
        lblStatus.ForeColor = Color.FromArgb(31, 41, 55);
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        OnCancelRequested();
    }

    protected virtual void OnCancelRequested()
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }

    // Animation methods for smooth transitions
    public async Task AnimateToProgressAsync(int targetPercentage, string status, int durationMs = 1000)
    {
        StatusText = status;
        IsIndeterminate = false;
        
        var startPercentage = ProgressPercentage;
        var steps = 50;
        var stepDelay = durationMs / steps;
        var stepSize = (targetPercentage - startPercentage) / (double)steps;
        
        for (int i = 0; i < steps; i++)
        {
            ProgressPercentage = (int)(startPercentage + stepSize * (i + 1));
            await Task.Delay(stepDelay);
        }
        
        ProgressPercentage = targetPercentage;
    }
}