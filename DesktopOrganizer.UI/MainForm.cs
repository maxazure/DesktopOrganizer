using DesktopOrganizer.App.Services;
using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure.Repositories;
using DesktopOrganizer.UI.Controls;
using DesktopOrganizer.UI.Logging;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DesktopOrganizer.UI;

/// <summary>
/// Simplified main application window with modern UI
/// </summary>
public partial class MainForm : Form
{
    private readonly OrganizationService _organizationService;
    private readonly DesktopScanService _desktopScanService;
    private readonly IPlanPreviewService _planPreviewService;
    private readonly IUndoService _undoService;
    private readonly PreferenceTemplateManager _templateManager;
    private readonly PreferenceProcessor _preferenceProcessor;
    private readonly PreferencesRepository _preferencesRepository;
    private readonly ModelProfileRepository _modelProfileRepository;
    private readonly ILogger<MainForm> _logger;

    private List<Item> _currentItems = new();
    private Plan? _currentPlan;
    private AppState _currentState = AppState.Ready;
    private CancellationTokenSource? _cancellationTokenSource;
    private LogViewerForm? _logViewerForm;
    private string _currentPreference = string.Empty;

    public MainForm(
        OrganizationService organizationService,
        DesktopScanService desktopScanService,
        IPlanPreviewService planPreviewService,
        IUndoService undoService,
        PreferenceTemplateManager templateManager,
        PreferenceProcessor preferenceProcessor,
        PreferencesRepository preferencesRepository,
        ModelProfileRepository modelProfileRepository,
        ILogger<MainForm> logger)
    {
        _organizationService = organizationService;
        _desktopScanService = desktopScanService;
        _planPreviewService = planPreviewService;
        _undoService = undoService;
        _templateManager = templateManager;
        _preferenceProcessor = preferenceProcessor;
        _preferencesRepository = preferencesRepository;
        _modelProfileRepository = modelProfileRepository;
        _logger = logger;

        InitializeComponent();
        ApplyModernStyling();
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        try
        {
            _logger.LogInformation("Initializing main form...");
            
            // Load templates for preference input panel
            preferenceInputPanel.Templates = _templateManager.Templates;
            
            // Start desktop scanning
            await RefreshDesktopScanAsync();
            
            // Update UI state
            UpdateUIForState(_currentState);
            
            _logger.LogInformation("Main form initialization completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize main form");
            ShowError($"初始化错误: {ex.Message}");
        }
    }

    private void ApplyModernStyling()
    {
        // Apply rounded corners to main action buttons
        btnStartOrganize.Region = CreateRoundedRegion(btnStartOrganize.Size, 8);
        btnReleaseFolders.Region = CreateRoundedRegion(btnReleaseFolders.Size, 8);
        
        // Apply rounded corners to action buttons
        btnAdjustPlan.Region = CreateRoundedRegion(btnAdjustPlan.Size, 6);
        btnExecutePlan.Region = CreateRoundedRegion(btnExecutePlan.Size, 6);
        btnUndo.Region = CreateRoundedRegion(btnUndo.Size, 6);
        
        // Add tooltips for better user experience
        AddTooltips();
    }

    private void AddTooltips()
    {
        var toolTip = new ToolTip();
        toolTip.SetToolTip(btnStartOrganize, "开始AI智能整理 (Ctrl+Enter)");
        toolTip.SetToolTip(btnReleaseFolders, "将所有文件夹中的文件移到桌面 (Ctrl+Shift+R)");
        toolTip.SetToolTip(btnSettings, "打开设置窗口 (Ctrl+,)");
        toolTip.SetToolTip(btnHelp, "查看应用日志");
        toolTip.SetToolTip(btnMinimize, "最小化窗口");
        toolTip.SetToolTip(btnAdjustPlan, "返回修改整理偏好");
        toolTip.SetToolTip(btnExecutePlan, "执行当前整理计划");
        toolTip.SetToolTip(btnUndo, "撤销上次整理操作 (Ctrl+Z)");
        toolTip.SetToolTip(lblFileCount, "按F5刷新桌面文件扫描");
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

    private async Task RefreshDesktopScanAsync()
    {
        try
        {
            _logger.LogInformation("Scanning desktop files...");
            
            _currentItems = await _desktopScanService.ScanDesktopAsync();
            
            // Update file count display
            lblFileCount.Text = $"📁 当前桌面文件：{_currentItems.Count} 个文件";
            
            _logger.LogInformation("Found {Count} desktop files", _currentItems.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to scan desktop");
            ShowError($"扫描桌面时出错: {ex.Message}");
        }
    }

    private void UpdateUIForState(AppState state)
    {
        _currentState = state;

        switch (state)
        {
            case AppState.Ready:
                btnStartOrganize.Enabled = true;
                btnStartOrganize.Text = "🚀 开始整理";
                btnReleaseFolders.Enabled = true;
                preferenceInputPanel.ReadOnly = false;

                lblPreviewTitle.Visible = false;
                actionButtonPanel.Visible = false;
                statusPanel.Visible = false;

                // Clear preview panel
                previewPanel.Controls.Clear();
                break;

            case AppState.Processing:
                btnStartOrganize.Enabled = false;
                btnStartOrganize.Text = "处理中...";
                btnReleaseFolders.Enabled = false;
                preferenceInputPanel.ReadOnly = true;

                statusPanel.Visible = true;
                progressIndicator.SetIndeterminateProgress("AI正在分析文件并生成整理计划...");
                progressIndicator.ShowCancelButton = true;
                break;

            case AppState.PreviewReady:
                btnStartOrganize.Enabled = true;
                btnStartOrganize.Text = "🔄 重新分析";
                btnReleaseFolders.Enabled = true;
                preferenceInputPanel.ReadOnly = false;

                lblPreviewTitle.Visible = true;
                actionButtonPanel.Visible = true;
                statusPanel.Visible = false;

                btnExecutePlan.Enabled = true;
                btnAdjustPlan.Enabled = true;
                break;

            case AppState.Executing:
                btnStartOrganize.Enabled = false;
                btnReleaseFolders.Enabled = false;
                preferenceInputPanel.ReadOnly = true;
                actionButtonPanel.Visible = false;

                statusPanel.Visible = true;
                progressIndicator.ShowCancelButton = false;
                break;

            case AppState.Completed:
                btnStartOrganize.Enabled = true;
                btnStartOrganize.Text = "🚀 开始整理";
                btnReleaseFolders.Enabled = true;
                preferenceInputPanel.ReadOnly = false;

                lblPreviewTitle.Visible = false;
                actionButtonPanel.Visible = false;
                statusPanel.Visible = false;

                btnUndo.Enabled = _undoService.HasUndoDataAsync().GetAwaiter().GetResult();

                // Clear preview and refresh desktop scan
                previewPanel.Controls.Clear();
                _ = RefreshDesktopScanAsync();
                break;
        }
    }

    private async void btnStartOrganize_Click(object sender, EventArgs e)
    {
        await StartOrganizationAsync();
    }

    private async Task StartOrganizationAsync()
    {
        if (!preferenceInputPanel.HasValidPreference())
        {
            preferenceInputPanel.ShowValidationError("请输入整理偏好或选择模板");
            return;
        }

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            UpdateUIForState(AppState.Processing);

            _currentPreference = preferenceInputPanel.PreferenceText;
            
            // Generate organization plan using AI
            var combinedPrompt = _preferenceProcessor.CombineWithPrompt(_currentPreference, _currentItems);
            _currentPlan = await _organizationService.GenerateOrganizationPlanAsync(_currentItems, combinedPrompt, _cancellationTokenSource.Token);

            if (_currentPlan?.Folders?.Any() == true)
            {
                // Display preview
                DisplayPlanPreview(_currentPlan);
                UpdateUIForState(AppState.PreviewReady);
                
                _logger.LogInformation("Organization plan generated with {FolderCount} folders", _currentPlan.Folders.Count);
            }
            else
            {
                UpdateUIForState(AppState.Ready);
                ShowInfo("未能生成有效的整理计划，请尝试调整您的偏好描述。");
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Organization plan generation was cancelled");
            UpdateUIForState(AppState.Ready);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate organization plan");
            UpdateUIForState(AppState.Ready);
            ShowError($"生成整理计划时出错: {ex.Message}");
        }
        finally
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void DisplayPlanPreview(Plan plan)
    {
        previewPanel.Controls.Clear();

        if (plan.Folders == null || !plan.Folders.Any())
        {
            var noPreviewLabel = new Label
            {
                Text = "没有生成预览",
                Font = new Font("Microsoft YaHei UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true
            };
            previewPanel.Controls.Add(noPreviewLabel);
            return;
        }

        foreach (var folder in plan.Folders)
        {
            var card = new FolderPreviewCard
            {
                FolderName = folder.Name,
                Description = folder.Description ?? "",
                FileList = folder.Files?.ToList() ?? new List<string>(),
                KeepOnDesktop = folder.KeepOnDesktop,
                Size = new Size(280, 200),
                Margin = new Padding(5)
            };

            card.KeepOnDesktopChanged += (s, args) =>
            {
                // Update the plan when user changes keep on desktop setting
                var planFolder = plan.Folders.FirstOrDefault(f => f.Name == args.FolderName);
                if (planFolder != null)
                {
                    planFolder.KeepOnDesktop = args.KeepOnDesktop;
                }
            };

            previewPanel.Controls.Add(card);
        }
    }

    private async void btnExecutePlan_Click(object sender, EventArgs e)
    {
        if (_currentPlan == null) return;

        var confirmResult = MessageBox.Show(
            $"确定要执行整理计划吗？\n\n将会移动 {_currentPlan.GetMoveOperations().Count()} 个文件到相应文件夹。",
            "确认执行",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        try
        {
            UpdateUIForState(AppState.Executing);
            
            var operations = _currentPlan.GetMoveOperations().ToList();
            
            // Execute with progress updates
            for (int i = 0; i < operations.Count; i++)
            {
                var operation = operations[i];
                await _organizationService.ExecuteSingleOperationAsync(operation);
                
                var progress = (int)((i + 1) * 100.0 / operations.Count);
                progressIndicator.SetProgress(progress, $"正在整理文件 ({i + 1}/{operations.Count})...");
                
                await Task.Delay(50); // Small delay for UI responsiveness
            }

            progressIndicator.SetComplete("整理完成！");
            await Task.Delay(1500); // Show completion message
            
            UpdateUIForState(AppState.Completed);
            
            ShowInfo($"整理完成！已移动 {operations.Count} 个文件。");
            _logger.LogInformation("Organization completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute organization plan");
            UpdateUIForState(AppState.PreviewReady);
            ShowError($"执行整理计划时出错: {ex.Message}");
        }
    }

    private void btnAdjustPlan_Click(object sender, EventArgs e)
    {
        // Allow user to modify preferences and regenerate
        UpdateUIForState(AppState.Ready);
        preferenceInputPanel.FocusOnInput();
    }

    private async void btnUndo_Click(object sender, EventArgs e)
    {
        if (!await _undoService.HasUndoDataAsync()) return;

        var confirmResult = MessageBox.Show(
            "确定要撤销上次的整理操作吗？",
            "确认撤销",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        try
        {
            UpdateUIForState(AppState.Executing);
            progressIndicator.SetIndeterminateProgress("正在撤销操作...");
            
            await _undoService.ExecuteUndoAsync();
            
            progressIndicator.SetComplete("撤销完成！");
            await Task.Delay(1500);
            
            UpdateUIForState(AppState.Ready);
            ShowInfo("撤销操作完成！");
            
            _logger.LogInformation("Undo operation completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to undo operation");
            UpdateUIForState(AppState.Ready);
            ShowError($"撤销操作时出错: {ex.Message}");
        }
    }

    private void btnSettings_Click(object sender, EventArgs e)
    {
        try
        {
            using var settingsForm = new SettingsForm(
                _preferencesRepository,
                _modelProfileRepository,
                _templateManager,
                _logger as ILogger<SettingsForm> ?? throw new InvalidCastException("Logger类型不匹配"));
                
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                // Reload templates after settings change
                preferenceInputPanel.Templates = _templateManager.Templates;
                _logger.LogInformation("Settings updated");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to open settings");
            ShowError($"打开设置时出错: {ex.Message}");
        }
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
        try
        {
            // Open log viewer as help for now
            if (_logViewerForm == null || _logViewerForm.IsDisposed)
            {
                _logViewerForm = new LogViewerForm();
            }
            
            _logViewerForm.Show();
            _logViewerForm.BringToFront();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to open help");
            ShowError($"打开帮助时出错: {ex.Message}");
        }
    }

    private void btnMinimize_Click(object sender, EventArgs e)
    {
        WindowState = FormWindowState.Minimized;
    }

    private void PreferenceInputPanel_PreferenceChanged(object? sender, PreferenceChangedEventArgs e)
    {
        // Update state when preference changes
        if (_currentState == AppState.PreviewReady && !string.IsNullOrWhiteSpace(e.PreferenceText))
        {
            // If user changes preference while preview is shown, reset to ready state
            UpdateUIForState(AppState.Ready);
        }
    }

    private void PreferenceInputPanel_TemplateSelected(object? sender, TemplateSelectedEventArgs e)
    {
        _logger.LogInformation("Template selected: {TemplateName}", e.Template.Name);
    }

    private void ProgressIndicator_CancelRequested(object? sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();
        UpdateUIForState(AppState.Ready);
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowInfo(string message)
    {
        MessageBox.Show(message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        try
        {
            _cancellationTokenSource?.Cancel();
            _logViewerForm?.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during form closing");
        }
        
        base.OnFormClosing(e);
    }

    private async void btnReleaseFolders_Click(object sender, EventArgs e)
    {
        await ReleaseFoldersAsync();
    }

    private async Task ReleaseFoldersAsync()
    {
        var confirmResult = MessageBox.Show(
            "确定要释放桌面上所有文件夹中的文件吗？\n\n此操作将会：\n• 遍历桌面上的所有文件夹（包括子文件夹）\n• 将所有文件移动到桌面根目录\n• 自动处理重名文件（添加数字后缀）\n• 重新扫描桌面文件",
            "确认释放文件夹",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmResult != DialogResult.Yes) return;

        try
        {
            UpdateUIForState(AppState.Processing);
            progressIndicator.SetIndeterminateProgress("正在释放文件夹中的文件...");

            await _organizationService.ReleaseFoldersAsync();

            progressIndicator.SetComplete("释放完成！");
            await Task.Delay(1500);

            // Refresh desktop scan
            await RefreshDesktopScanAsync();

            UpdateUIForState(AppState.Ready);
            ShowInfo("文件夹释放完成！所有文件已移动到桌面根目录。");

            _logger.LogInformation("Folders released successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to release folders");
            UpdateUIForState(AppState.Ready);
            ShowError($"释放文件夹时出错: {ex.Message}");
        }
    }

    // Keyboard shortcuts
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.Enter:
                if (_currentState == AppState.Ready && btnStartOrganize.Enabled)
                {
                    _ = StartOrganizationAsync();
                    return true;
                }
                break;
                
            case Keys.Control | Keys.Z:
                if (btnUndo.Enabled)
                {
                    btnUndo_Click(this, EventArgs.Empty);
                    return true;
                }
                break;
                
            case Keys.F5:
                _ = RefreshDesktopScanAsync();
                return true;
                
            case Keys.Control | Keys.Oemcomma: // Ctrl+,
                btnSettings_Click(this, EventArgs.Empty);
                return true;
                
            case Keys.Control | Keys.Shift | Keys.R: // Ctrl+Shift+R
                if (btnReleaseFolders.Enabled)
                {
                    _ = ReleaseFoldersAsync();
                    return true;
                }
                break;
        }
        
        return base.ProcessCmdKey(ref msg, keyData);
    }
}