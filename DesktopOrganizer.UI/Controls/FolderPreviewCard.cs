using System.ComponentModel;

namespace DesktopOrganizer.UI.Controls;

/// <summary>
/// User control for displaying folder preview cards in the organization plan
/// </summary>
public partial class FolderPreviewCard : UserControl
{
    public event EventHandler<FolderCardEventArgs>? CardClicked;
    public event EventHandler<FolderCardEventArgs>? KeepOnDesktopChanged;

    private string _folderName = string.Empty;
    private string _description = string.Empty;
    private List<string> _fileList = new();
    private bool _keepOnDesktop = false;

    public FolderPreviewCard()
    {
        InitializeComponent();
        SetupEventHandlers();
        ApplyModernStyling();
    }

    [Category("Appearance")]
    [Description("The name of the folder")]
    public string FolderName
    {
        get => _folderName;
        set
        {
            _folderName = value;
            lblFolderName.Text = value;
        }
    }

    [Category("Appearance")]
    [Description("Description of the folder")]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            lblDescription.Text = value;
        }
    }

    [Category("Data")]
    [Description("List of files in this folder")]
    public List<string> FileList
    {
        get => _fileList;
        set
        {
            _fileList = value ?? new List<string>();
            UpdateFileList();
        }
    }

    [Category("Behavior")]
    [Description("Whether to keep files on desktop instead of creating folder")]
    public bool KeepOnDesktop
    {
        get => _keepOnDesktop;
        set
        {
            _keepOnDesktop = value;
            chkKeepOnDesktop.Checked = value;
            UpdateAppearanceForKeepOnDesktop();
        }
    }

    private void SetupEventHandlers()
    {
        mainPanel.Click += MainPanel_Click;
        lblFolderName.Click += MainPanel_Click;
        lblDescription.Click += MainPanel_Click;
        lstFiles.Click += MainPanel_Click;
        
        // Add hover effects
        mainPanel.MouseEnter += MainPanel_MouseEnter;
        mainPanel.MouseLeave += MainPanel_MouseLeave;
    }

    private void ApplyModernStyling()
    {
        // Apply modern card styling
        mainPanel.BackColor = Color.White;
        mainPanel.BorderStyle = BorderStyle.None;
        
        // Add shadow effect simulation
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        
        // Draw subtle shadow
        using (var shadowBrush = new SolidBrush(Color.FromArgb(10, 0, 0, 0)))
        {
            e.Graphics.FillRectangle(shadowBrush, 2, 2, Width - 2, Height - 2);
        }
        
        // Draw main panel
        using (var backgroundBrush = new SolidBrush(mainPanel.BackColor))
        {
            e.Graphics.FillRectangle(backgroundBrush, 0, 0, Width - 2, Height - 2);
        }
        
        // Draw border
        using (var borderPen = new Pen(Color.FromArgb(229, 231, 235)))
        {
            e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 3, Height - 3);
        }
    }

    private void UpdateFileList()
    {
        lstFiles.Items.Clear();
        
        if (_fileList != null && _fileList.Any())
        {
            // Show first few files
            var filesToShow = _fileList.Take(8).ToList();
            foreach (var file in filesToShow)
            {
                lstFiles.Items.Add($"• {file}");
            }
            
            // Add "and more" if there are more files
            if (_fileList.Count > 8)
            {
                lstFiles.Items.Add($"... 和其他 {_fileList.Count - 8} 个文件");
            }
        }
        
        lblFileCount.Text = $"文件数量: {_fileList?.Count ?? 0}";
    }

    private void UpdateAppearanceForKeepOnDesktop()
    {
        if (_keepOnDesktop)
        {
            mainPanel.BackColor = Color.FromArgb(254, 249, 195); // Light yellow
            lblFolderName.ForeColor = Color.FromArgb(146, 64, 14); // Dark orange
        }
        else
        {
            mainPanel.BackColor = Color.White;
            lblFolderName.ForeColor = Color.FromArgb(31, 41, 55); // Dark gray
        }
        
        Invalidate(); // Trigger repaint
    }

    private void MainPanel_Click(object? sender, EventArgs e)
    {
        OnCardClicked();
    }

    private void MainPanel_MouseEnter(object? sender, EventArgs e)
    {
        if (!_keepOnDesktop)
        {
            mainPanel.BackColor = Color.FromArgb(249, 250, 251); // Light gray
        }
        Invalidate();
    }

    private void MainPanel_MouseLeave(object? sender, EventArgs e)
    {
        UpdateAppearanceForKeepOnDesktop();
    }

    private void chkKeepOnDesktop_CheckedChanged(object? sender, EventArgs e)
    {
        _keepOnDesktop = chkKeepOnDesktop.Checked;
        UpdateAppearanceForKeepOnDesktop();
        OnKeepOnDesktopChanged();
    }

    protected virtual void OnCardClicked()
    {
        CardClicked?.Invoke(this, new FolderCardEventArgs(_folderName, _fileList, _keepOnDesktop));
    }

    protected virtual void OnKeepOnDesktopChanged()
    {
        KeepOnDesktopChanged?.Invoke(this, new FolderCardEventArgs(_folderName, _fileList, _keepOnDesktop));
    }
}

/// <summary>
/// Event arguments for folder card events
/// </summary>
public class FolderCardEventArgs : EventArgs
{
    public string FolderName { get; }
    public List<string> FileList { get; }
    public bool KeepOnDesktop { get; }

    public FolderCardEventArgs(string folderName, List<string> fileList, bool keepOnDesktop)
    {
        FolderName = folderName;
        FileList = fileList;
        KeepOnDesktop = keepOnDesktop;
    }
}