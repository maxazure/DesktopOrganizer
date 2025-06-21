using DesktopOrganizer.App.Services;
using DesktopOrganizer.Domain;
using DesktopOrganizer.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace DesktopOrganizer.UI;

/// <summary>
/// Modern settings form with tabbed interface
/// </summary>
public partial class SettingsForm : Form
{
    private readonly PreferencesRepository _preferencesRepository;
    private readonly ModelProfileRepository _modelProfileRepository;
    private readonly PreferenceTemplateManager _templateManager;
    private readonly ILogger<SettingsForm> _logger;

    private Preferences _preferences;
    private List<ModelProfile> _modelProfiles = new();
    private bool _hasChanges = false;

    public SettingsForm(
        PreferencesRepository preferencesRepository,
        ModelProfileRepository modelProfileRepository,
        PreferenceTemplateManager templateManager,
        ILogger<SettingsForm> logger)
    {
        InitializeComponent();
        _preferencesRepository = preferencesRepository;
        _modelProfileRepository = modelProfileRepository;
        _templateManager = templateManager;
        _logger = logger;
        _preferences = new Preferences();
    }

    private async void SettingsForm_Load(object sender, EventArgs e)
    {
        try
        {
            await LoadSettingsAsync();
            ApplyModernStyling();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load settings");
            MessageBox.Show($"加载设置时出现错误：{ex.Message}", "错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadSettingsAsync()
    {
        // Load preferences
        _preferences = await _preferencesRepository.LoadAsync();
        
        // Load model profiles
        _modelProfiles = await _modelProfileRepository.LoadAllAsync();
        
        // Initialize UI with loaded data
        InitializeProviderComboBox();
        InitializeLanguageComboBox();
        InitializeLogLevelComboBox();
        LoadTemplatesList();
        
        // Set values from preferences
        chkConfirmBeforeExecution.Checked = _preferences.ConfirmBeforeExecution;
        chkAutoCreateFolders.Checked = _preferences.AutoCreateFolders;
        cmbLanguage.SelectedValue = _preferences.Language;
        numMaxFileSize.Value = _preferences.MaxFileSizeBytes / (1024 * 1024); // Convert to MB
        
        // Load current model profile if available
        var currentProfile = _modelProfiles.FirstOrDefault();
        if (currentProfile != null)
        {
            cmbProvider.SelectedValue = currentProfile.Provider;
            UpdateModelComboBox();
            cmbModel.SelectedValue = currentProfile.ModelId;
            numTimeoutSeconds.Value = currentProfile.TimeoutSeconds;
        }
    }

    private void InitializeProviderComboBox()
    {
        var providers = new[]
        {
            new { Text = "DeepSeek", Value = "DeepSeek" },
            new { Text = "OpenAI", Value = "OpenAI" },
            new { Text = "Anthropic Claude", Value = "Anthropic" },
            new { Text = "本地模拟 (测试)", Value = "LocalMock" }
        };

        cmbProvider.DisplayMember = "Text";
        cmbProvider.ValueMember = "Value";
        cmbProvider.DataSource = providers;
    }

    private void InitializeLanguageComboBox()
    {
        var languages = new[]
        {
            new { Text = "中文 (简体)", Value = "zh-CN" },
            new { Text = "English", Value = "en-US" }
        };

        cmbLanguage.DisplayMember = "Text";
        cmbLanguage.ValueMember = "Value";
        cmbLanguage.DataSource = languages;
    }

    private void InitializeLogLevelComboBox()
    {
        var logLevels = new[]
        {
            new { Text = "调试", Value = "Debug" },
            new { Text = "信息", Value = "Information" },
            new { Text = "警告", Value = "Warning" },
            new { Text = "错误", Value = "Error" }
        };

        cmbLogLevel.DisplayMember = "Text";
        cmbLogLevel.ValueMember = "Value";
        cmbLogLevel.DataSource = logLevels;
        cmbLogLevel.SelectedValue = "Information";
    }

    private void LoadTemplatesList()
    {
        lstTemplates.Items.Clear();
        
        var templates = _templateManager.Templates;
        foreach (var template in templates)
        {
            var displayText = template.IsBuiltIn ? 
                $"[内置] {template.Name}" : 
                $"[自定义] {template.Name}";
            
            if (!string.IsNullOrEmpty(template.Description))
                displayText += $" - {template.Description}";
                
            lstTemplates.Items.Add(new { Template = template, Display = displayText });
        }
        
        lstTemplates.DisplayMember = "Display";
        lstTemplates.ValueMember = "Template";
    }

    private void UpdateModelComboBox()
    {
        var selectedProvider = cmbProvider.SelectedValue?.ToString();
        if (string.IsNullOrEmpty(selectedProvider)) return;

        var models = selectedProvider switch
        {
            "DeepSeek" => new[] { "deepseek-chat" },
            "OpenAI" => new[] { "gpt-4", "gpt-4-turbo", "gpt-3.5-turbo" },
            "Anthropic" => new[] { "claude-3-sonnet", "claude-3-haiku" },
            "LocalMock" => new[] { "mock-model" },
            _ => new[] { "default-model" }
        };

        cmbModel.Items.Clear();
        cmbModel.Items.AddRange(models);
        if (models.Length > 0) cmbModel.SelectedIndex = 0;
    }

    private void ApplyModernStyling()
    {
        BackColor = Color.FromArgb(249, 250, 251);
        
        // Round button corners
        btnSave.Region = CreateRoundedRegion(btnSave.Size, 6);
        btnNewTemplate.Region = CreateRoundedRegion(btnNewTemplate.Size, 6);
        btnDeleteTemplate.Region = CreateRoundedRegion(btnDeleteTemplate.Size, 6);
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

    private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateModelComboBox();
        _hasChanges = true;
    }

    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtApiKey.Text))
        {
            MessageBox.Show("请先输入API密钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTestConnection.Enabled = false;
        btnTestConnection.Text = "测试中...";

        try
        {
            // Here you would implement actual connection testing
            await Task.Delay(2000); // Simulate API call
            
            MessageBox.Show("连接测试成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Connection test failed");
            MessageBox.Show($"连接测试失败：{ex.Message}", "错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnTestConnection.Enabled = true;
            btnTestConnection.Text = "测试连接";
        }
    }

    private void btnNewTemplate_Click(object sender, EventArgs e)
    {
        using var dialog = new TemplateEditDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _templateManager.SaveCustomTemplate(dialog.TemplateName, dialog.TemplateContent, dialog.TemplateDescription);
            LoadTemplatesList();
            _hasChanges = true;
        }
    }

    private void btnEditTemplate_Click(object sender, EventArgs e)
    {
        if (lstTemplates.SelectedItem == null) return;
        
        var selectedTemplate = ((dynamic)lstTemplates.SelectedItem).Template as PreferenceTemplate;
        if (selectedTemplate == null || selectedTemplate.IsBuiltIn)
        {
            MessageBox.Show("无法编辑内置模板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialog = new TemplateEditDialog(selectedTemplate);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _templateManager.SaveCustomTemplate(dialog.TemplateName, dialog.TemplateContent, dialog.TemplateDescription);
            LoadTemplatesList();
            _hasChanges = true;
        }
    }

    private void btnDeleteTemplate_Click(object sender, EventArgs e)
    {
        if (lstTemplates.SelectedItem == null) return;
        
        var selectedTemplate = ((dynamic)lstTemplates.SelectedItem).Template as PreferenceTemplate;
        if (selectedTemplate == null || selectedTemplate.IsBuiltIn)
        {
            MessageBox.Show("无法删除内置模板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show($"确定要删除模板 '{selectedTemplate.Name}' 吗？", 
            "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
        if (result == DialogResult.Yes)
        {
            _templateManager.DeleteTemplate(selectedTemplate.Name);
            LoadTemplatesList();
            _hasChanges = true;
        }
    }

    private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        var hasSelection = lstTemplates.SelectedItem != null;
        var selectedTemplate = hasSelection ? 
            ((dynamic)lstTemplates.SelectedItem).Template as PreferenceTemplate : null;
        var isCustomTemplate = selectedTemplate != null && !selectedTemplate.IsBuiltIn;
        
        btnEditTemplate.Enabled = isCustomTemplate;
        btnDeleteTemplate.Enabled = isCustomTemplate;
    }

    private void btnOpenLogFile_Click(object sender, EventArgs e)
    {
        var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
            "DesktopOrganizer.log");
            
        if (File.Exists(logPath))
        {
            System.Diagnostics.Process.Start("notepad.exe", logPath);
        }
        else
        {
            MessageBox.Show("日志文件不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        await SaveSettingsAsync();
        DialogResult = DialogResult.OK;
        Close();
    }

    private async void btnApply_Click(object sender, EventArgs e)
    {
        await SaveSettingsAsync();
    }

    private async Task SaveSettingsAsync()
    {
        try
        {
            // Update preferences
            _preferences = new Preferences(
                _preferences.FolderRules,
                _preferences.IgnoreExtensions,
                (long)numMaxFileSize.Value * 1024 * 1024, // Convert MB to bytes
                chkAutoCreateFolders.Checked,
                chkConfirmBeforeExecution.Checked,
                cmbLanguage.SelectedValue?.ToString() ?? "zh-CN"
            );

            await _preferencesRepository.SaveAsync(_preferences);

            // Save model profile if needed
            if (cmbProvider.SelectedValue != null && cmbModel.SelectedValue != null)
            {
                var profile = new ModelProfile(
                    "默认配置",
                    cmbProvider.SelectedValue.ToString()!,
                    "",
                    cmbModel.SelectedValue.ToString()!,
                    "",
                    (int)numTimeoutSeconds.Value,
                    false
                );
                _modelProfiles.Add(profile);
                await _modelProfileRepository.SaveAllAsync(_modelProfiles);

                await _modelProfileRepository.SaveAllAsync(_modelProfiles);
            }

            _hasChanges = false;
            _logger.LogInformation("Settings saved successfully");
            
            MessageBox.Show("设置已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save settings");
            MessageBox.Show($"保存设置时出现错误：{ex.Message}", "错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_hasChanges)
        {
            var result = MessageBox.Show("设置已更改，是否保存？", "确认", 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                
            switch (result)
            {
                case DialogResult.Yes:
                    _ = SaveSettingsAsync();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }
        
        base.OnFormClosing(e);
    }
}

/// <summary>
/// Simple template edit dialog
/// </summary>
public partial class TemplateEditDialog : Form
{
    public string TemplateName { get; private set; } = string.Empty;
    public string TemplateDescription { get; private set; } = string.Empty;
    public string TemplateContent { get; private set; } = string.Empty;

    private TextBox txtName = new();
    private TextBox txtDescription = new();
    private TextBox txtContent = new();
    private Button btnOK = new();
    private Button btnCancel = new();

    public TemplateEditDialog(PreferenceTemplate? template = null)
    {
        InitializeDialog();
        
        if (template != null)
        {
            txtName.Text = template.Name;
            txtDescription.Text = template.Description;
            txtContent.Text = template.Content;
            Text = "编辑模板";
        }
        else
        {
            Text = "新建模板";
        }
    }

    private void InitializeDialog()
    {
        Size = new Size(500, 400);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var lblName = new Label { Text = "模板名称:", Location = new Point(15, 15), Size = new Size(80, 20) };
        txtName.Location = new Point(100, 12);
        txtName.Size = new Size(365, 23);

        var lblDesc = new Label { Text = "模板描述:", Location = new Point(15, 45), Size = new Size(80, 20) };
        txtDescription.Location = new Point(100, 42);
        txtDescription.Size = new Size(365, 23);

        var lblContent = new Label { Text = "模板内容:", Location = new Point(15, 75), Size = new Size(80, 20) };
        txtContent.Location = new Point(15, 95);
        txtContent.Size = new Size(450, 220);
        txtContent.Multiline = true;
        txtContent.ScrollBars = ScrollBars.Vertical;

        btnOK.Text = "确定";
        btnOK.Location = new Point(315, 330);
        btnOK.Size = new Size(75, 30);
        btnOK.DialogResult = DialogResult.OK;
        btnOK.Click += (s, e) => {
            TemplateName = txtName.Text.Trim();
            TemplateDescription = txtDescription.Text.Trim();
            TemplateContent = txtContent.Text.Trim();
        };

        btnCancel.Text = "取消";
        btnCancel.Location = new Point(395, 330);
        btnCancel.Size = new Size(75, 30);
        btnCancel.DialogResult = DialogResult.Cancel;

        Controls.AddRange(new Control[] { 
            lblName, txtName, lblDesc, txtDescription, 
            lblContent, txtContent, btnOK, btnCancel 
        });

        AcceptButton = btnOK;
        CancelButton = btnCancel;
    }
}