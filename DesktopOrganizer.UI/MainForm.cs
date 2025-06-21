using DesktopOrganizer.App.Services;
using DesktopOrganizer.Domain;
using Microsoft.Extensions.Logging;
using DesktopOrganizer.UI.Logging;

namespace DesktopOrganizer.UI;

/// <summary>
/// Main application window
/// </summary>
public partial class MainForm : Form
{
    private readonly OrganizationService _organizationService;
    private readonly IModelProfileRepository _modelProfileRepository;
    private readonly ICredentialService _credentialService;
    private readonly ILogger<MainForm> _logger;

    private List<Item> _currentItems = new();
    private Plan? _currentPlan;
    private CancellationTokenSource? _cancellationTokenSource;
    private LogViewerForm? _logViewerForm;

    public MainForm(OrganizationService organizationService, IModelProfileRepository modelProfileRepository, ICredentialService credentialService, ILogger<MainForm> logger)
    {
        _organizationService = organizationService;
        _modelProfileRepository = modelProfileRepository;
        _credentialService = credentialService;
        _logger = logger;
        InitializeComponent();
        InitializeUIAsync();
    }

    private async void InitializeUIAsync()
    {
        try
        {
            _logger.LogInformation("初始化主界面...");
            await LoadCurrentModelAsync();
            await RefreshDesktopItemsAsync();
            UpdateUI();
            _logger.LogInformation("主界面初始化完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "初始化主界面时发生错误");
            ShowError($"初始化错误: {ex.Message}");
        }
    }

    private async void btnScan_Click(object sender, EventArgs e)
    {
        await ExecuteAsync(async () =>
        {
            await RefreshDesktopItemsAsync();
            UpdateUI();
        }, "Scanning desktop...");
    }

    private async void btnAnalyze_Click(object sender, EventArgs e)
    {
        _logger.LogInformation("用户点击分析按钮");

        await ExecuteAsync(async () =>
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var progress = new Progress<string>(token =>
            {
                if (InvokeRequired)
                {
                    Invoke(() => AppendToLog(token));
                }
                else
                {
                    AppendToLog(token);
                }
            });

            _logger.LogInformation("开始生成整理方案...");
            _currentPlan = await _organizationService.GenerateOrganizationPlanAsync(
                progress, _cancellationTokenSource.Token);

            _logger.LogInformation("整理方案生成完成，开始显示预览");
            await DisplayPlanPreviewAsync(_currentPlan);
            UpdateUI();

        }, "使用 LLM 分析中...");
    }

    private async void btnExecute_Click(object sender, EventArgs e)
    {
        if (_currentPlan == null)
        {
            ShowError("No plan available. Please analyze first.");
            return;
        }

        var result = MessageBox.Show(
            "Are you sure you want to execute this organization plan? This will move files on your desktop.",
            "Confirm Execution",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        await ExecuteAsync(async () =>
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var progress = new Progress<ExecutionProgress>(prog =>
            {
                if (InvokeRequired)
                {
                    Invoke(() => UpdateExecutionProgress(prog));
                }
                else
                {
                    UpdateExecutionProgress(prog);
                }
            });

            var execResult = await _organizationService.ExecutePlanAsync(
                _currentPlan, progress, _cancellationTokenSource.Token);

            if (execResult.Success)
            {
                ShowInfo($"Organization completed successfully! Moved {execResult.CompletedOperations.Count} items.");
                await RefreshDesktopItemsAsync();
                _currentPlan = null;
            }
            else
            {
                ShowError($"Organization failed: {execResult.ErrorMessage}");
            }

            UpdateUI();

        }, "Executing organization plan...");
    }

    private async void btnUndo_Click(object sender, EventArgs e)
    {
        var hasUndo = await _organizationService.HasUndoDataAsync();
        if (!hasUndo)
        {
            ShowError("No undo data available.");
            return;
        }

        var result = MessageBox.Show(
            "Are you sure you want to undo the last organization operation?",
            "Confirm Undo",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        await ExecuteAsync(async () =>
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var progress = new Progress<ExecutionProgress>(prog =>
            {
                if (InvokeRequired)
                {
                    Invoke(() => UpdateExecutionProgress(prog));
                }
                else
                {
                    UpdateExecutionProgress(prog);
                }
            });

            var success = await _organizationService.UndoLastOperationAsync(
                progress, _cancellationTokenSource.Token);

            if (success)
            {
                ShowInfo("Undo completed successfully!");
                await RefreshDesktopItemsAsync();
            }
            else
            {
                ShowError("Undo operation failed.");
            }

            UpdateUI();

        }, "Undoing last operation...");
    }

    private async void cmbCurrentModel_DropDownOpened(object sender, EventArgs e)
    {
        await LoadModelProfilesAsync();
    }

    private async void cmbCurrentModel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbCurrentModel.SelectedItem is ModelProfile selectedProfile)
        {
            await _modelProfileRepository.SetDefaultAsync(selectedProfile.Id);
        }
    }

    private async void btnModelSettings_Click(object sender, EventArgs e)
    {
        using var dialog = new ModelProfileDialog(_modelProfileRepository, _credentialService);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            await LoadCurrentModelAsync();
            await LoadModelProfilesAsync();
        }
    }

    private void btnViewLogs_Click(object sender, EventArgs e)
    {
        if (_logViewerForm == null || _logViewerForm.IsDisposed)
        {
            _logViewerForm = new LogViewerForm();

            // 注册到全局日志查看器提供程序
            GlobalLogViewerProvider.RegisterLogViewer(_logViewerForm);
        }

        _logViewerForm.Show();
        _logViewerForm.BringToFront();
    }

    private async Task RefreshDesktopItemsAsync()
    {
        _currentItems = await _organizationService.ScanDesktopAsync();

        // Update original items list
        listViewOriginal.Items.Clear();
        foreach (var item in _currentItems)
        {
            var listItem = new ListViewItem(item.Name);
            listItem.SubItems.Add(item.IsDirectory ? "Folder" : item.Extension);
            listItem.SubItems.Add(item.IsDirectory ? "" : FormatFileSize(item.Size));
            listItem.SubItems.Add(item.ModifiedTime.ToString("yyyy-MM-dd HH:mm"));
            listItem.Tag = item;
            listViewOriginal.Items.Add(listItem);
        }

        // Update statistics
        var stats = await _organizationService.GetDesktopStatisticsAsync();
        lblItemCount.Text = $"Items: {stats.GetValueOrDefault("Total", 0)}";
    }

    private async Task DisplayPlanPreviewAsync(Plan plan)
    {
        treeViewPreview.Nodes.Clear();

        var groups = await _organizationService.GetPlanPreviewAsync(plan);

        foreach (var group in groups)
        {
            var folderNode = new TreeNode(group.Key) { Tag = group.Key };

            foreach (var item in group.Value)
            {
                var itemNode = new TreeNode(item) { Tag = item };
                folderNode.Nodes.Add(itemNode);
            }

            treeViewPreview.Nodes.Add(folderNode);
        }

        treeViewPreview.ExpandAll();
    }

    private async Task LoadCurrentModelAsync()
    {
        var currentModel = await _modelProfileRepository.GetDefaultAsync();
        if (currentModel != null)
        {
            lblCurrentModel.Text = $"Current Model: {currentModel.Name}";
        }
        else
        {
            lblCurrentModel.Text = "Current Model: None configured";
        }
    }

    private async Task LoadModelProfilesAsync()
    {
        var profiles = await _modelProfileRepository.LoadAllAsync();

        cmbCurrentModel.Items.Clear();
        foreach (var profile in profiles)
        {
            cmbCurrentModel.Items.Add(profile);
        }

        var defaultProfile = profiles.FirstOrDefault(p => p.IsDefault);
        if (defaultProfile != null)
        {
            cmbCurrentModel.SelectedItem = defaultProfile;
        }
    }

    private async Task ExecuteAsync(Func<Task> action, string statusText)
    {
        _logger.LogDebug("开始执行操作: {StatusText}", statusText);

        try
        {
            SetUIEnabled(false);
            lblStatus.Text = statusText;

            await action();

            lblStatus.Text = "就绪";
            _logger.LogDebug("操作执行成功: {StatusText}", statusText);
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = "已取消";
            _logger.LogWarning("操作被取消: {StatusText}", statusText);
        }
        catch (Exception ex)
        {
            var errorMessage = $"错误: {ex.Message}";
            _logger.LogError(ex, "执行操作时发生错误: {StatusText}", statusText);

            // 为常见错误提供更友好的提示
            if (ex.Message.Contains("API key"))
            {
                errorMessage = "API 密钥错误，请检查模型配置中的 API 密钥设置";
            }
            else if (ex.Message.Contains("timeout") || ex.Message.Contains("超时"))
            {
                errorMessage = "请求超时，请检查网络连接或尝试增加超时时间";
            }
            else if (ex.Message.Contains("Invalid response format") || ex.Message.Contains("响应格式"))
            {
                errorMessage = "API 响应格式错误，请检查模型配置或稍后重试";
            }

            ShowError(errorMessage);
            lblStatus.Text = "错误";
        }
        finally
        {
            SetUIEnabled(true);
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void SetUIEnabled(bool enabled)
    {
        btnScan.Enabled = enabled;
        btnAnalyze.Enabled = enabled && _currentItems.Any();
        btnExecute.Enabled = enabled && _currentPlan != null;
        btnUndo.Enabled = enabled; // Will be checked async

        if (!enabled)
        {
            progressBar.Style = ProgressBarStyle.Marquee;
        }
        else
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Value = 0;
        }
    }

    private void UpdateUI()
    {
        SetUIEnabled(true);
        CheckUndoAvailabilityAsync();
    }

    private async void CheckUndoAvailabilityAsync()
    {
        try
        {
            var hasUndo = await _organizationService.HasUndoDataAsync();
            btnUndo.Enabled = hasUndo;
        }
        catch
        {
            btnUndo.Enabled = false;
        }
    }

    private void UpdateExecutionProgress(ExecutionProgress progress)
    {
        progressBar.Style = ProgressBarStyle.Continuous;
        progressBar.Maximum = 100;
        progressBar.Value = Math.Min(100, (int)progress.PercentComplete);

        lblStatus.Text = progress.CurrentOperation ?? progress.Status ?? "Processing...";
    }

    private void AppendToLog(string text)
    {
        richTextBoxLog.AppendText(text);
        richTextBoxLog.ScrollToCaret();
    }

    private static void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static void ShowInfo(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }
}