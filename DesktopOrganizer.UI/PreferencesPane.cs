using DesktopOrganizer.Domain;
using System.Text.Json;

namespace DesktopOrganizer.UI;

/// <summary>
/// User control for editing preferences
/// </summary>
public partial class PreferencesPane : UserControl
{
    private readonly IPreferencesRepository _preferencesRepository;
    private Preferences _currentPreferences = new();
    private bool _isLoading = false;

    public PreferencesPane()
    {
        InitializeComponent();
    }

    public PreferencesPane(IPreferencesRepository preferencesRepository) : this()
    {
        _preferencesRepository = preferencesRepository;
        LoadPreferencesAsync();
    }

    private async void LoadPreferencesAsync()
    {
        try
        {
            _isLoading = true;
            _currentPreferences = await _preferencesRepository.LoadAsync();
            UpdateUI();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading preferences: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void UpdateUI()
    {
        if (_isLoading) return;

        // Update folder rules list
        listViewRules.Items.Clear();
        foreach (var rule in _currentPreferences.FolderRules)
        {
            var item = new ListViewItem(rule.Key);
            item.SubItems.Add(string.Join(", ", rule.Value));
            item.Tag = rule;
            listViewRules.Items.Add(item);
        }

        // Update ignore extensions
        txtIgnoreExtensions.Text = string.Join(", ", _currentPreferences.IgnoreExtensions);

        // Update other settings
        numMaxFileSize.Value = _currentPreferences.MaxFileSizeBytes / (1024 * 1024); // Convert to MB
        chkAutoCreateFolders.Checked = _currentPreferences.AutoCreateFolders;
        chkConfirmExecution.Checked = _currentPreferences.ConfirmBeforeExecution;

        // Update JSON view
        UpdateJsonView();
    }

    private void UpdateJsonView()
    {
        try
        {
            var json = JsonSerializer.Serialize(_currentPreferences, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (!txtJsonView.Focused) // Don't update if user is editing
            {
                txtJsonView.Text = json;
            }
        }
        catch (Exception ex)
        {
            txtJsonView.Text = $"Error serializing preferences: {ex.Message}";
        }
    }

    private async void SavePreferencesAsync()
    {
        if (_isLoading) return;

        try
        {
            await _preferencesRepository.SaveAsync(_currentPreferences);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving preferences: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnAddRule_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderRuleDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _currentPreferences.AddFolderRule(dialog.FolderName, dialog.Extensions);
            UpdateUI();
            SavePreferencesAsync();
        }
    }

    private void btnEditRule_Click(object sender, EventArgs e)
    {
        if (listViewRules.SelectedItems.Count == 0) return;

        var selectedItem = listViewRules.SelectedItems[0];
        var rule = (KeyValuePair<string, List<string>>)selectedItem.Tag;

        using var dialog = new FolderRuleDialog(rule.Key, rule.Value);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            _currentPreferences.RemoveFolderRule(rule.Key);
            _currentPreferences.AddFolderRule(dialog.FolderName, dialog.Extensions);
            UpdateUI();
            SavePreferencesAsync();
        }
    }

    private void btnDeleteRule_Click(object sender, EventArgs e)
    {
        if (listViewRules.SelectedItems.Count == 0) return;

        var selectedItem = listViewRules.SelectedItems[0];
        var rule = (KeyValuePair<string, List<string>>)selectedItem.Tag;

        var result = MessageBox.Show($"Delete rule for '{rule.Key}'?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            _currentPreferences.RemoveFolderRule(rule.Key);
            UpdateUI();
            SavePreferencesAsync();
        }
    }

    private void btnLoadTemplate_Click(object sender, EventArgs e)
    {
        var templates = new Dictionary<string, Dictionary<string, List<string>>>
        {
            ["Documents & Media"] = new()
            {
                ["Documents"] = new() { ".pdf", ".doc", ".docx", ".txt", ".rtf" },
                ["Pictures"] = new() { ".jpg", ".jpeg", ".png", ".gif", ".bmp" },
                ["Videos"] = new() { ".mp4", ".avi", ".mkv", ".mov" },
                ["Music"] = new() { ".mp3", ".wav", ".flac", ".aac" }
            },
            ["Development"] = new()
            {
                ["Code"] = new() { ".cs", ".js", ".ts", ".py", ".java", ".cpp" },
                ["Projects"] = new() { ".sln", ".csproj", ".vsproj" },
                ["Archives"] = new() { ".zip", ".rar", ".7z", ".tar", ".gz" }
            },
            ["Office Work"] = new()
            {
                ["Documents"] = new() { ".pdf", ".doc", ".docx", ".txt" },
                ["Spreadsheets"] = new() { ".xls", ".xlsx", ".csv" },
                ["Presentations"] = new() { ".ppt", ".pptx" },
                ["Archives"] = new() { ".zip", ".rar" }
            }
        };

        var templateNames = templates.Keys.ToArray();
        using var dialog = new ListSelectionDialog("Select Template", templateNames);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var selectedTemplate = templates[dialog.SelectedItem];

            // Clear existing rules and add template rules
            _currentPreferences.FolderRules.Clear();
            foreach (var rule in selectedTemplate)
            {
                _currentPreferences.AddFolderRule(rule.Key, rule.Value);
            }

            UpdateUI();
            SavePreferencesAsync();
        }
    }

    private void txtIgnoreExtensions_TextChanged(object sender, EventArgs e)
    {
        if (_isLoading) return;

        var extensions = txtIgnoreExtensions.Text
            .Split(',')
            .Select(ext => ext.Trim())
            .Where(ext => !string.IsNullOrWhiteSpace(ext))
            .ToList();

        _currentPreferences.IgnoreExtensions.Clear();
        _currentPreferences.IgnoreExtensions.AddRange(extensions);

        UpdateJsonView();
        SavePreferencesAsync();
    }

    private void numMaxFileSize_ValueChanged(object sender, EventArgs e)
    {
        if (_isLoading) return;

        _currentPreferences = new Preferences(
            _currentPreferences.FolderRules,
            _currentPreferences.IgnoreExtensions,
            (long)(numMaxFileSize.Value * 1024 * 1024), // Convert MB to bytes
            _currentPreferences.AutoCreateFolders,
            _currentPreferences.ConfirmBeforeExecution,
            _currentPreferences.Language
        );

        UpdateJsonView();
        SavePreferencesAsync();
    }

    private void chkAutoCreateFolders_CheckedChanged(object sender, EventArgs e)
    {
        if (_isLoading) return;

        _currentPreferences = new Preferences(
            _currentPreferences.FolderRules,
            _currentPreferences.IgnoreExtensions,
            _currentPreferences.MaxFileSizeBytes,
            chkAutoCreateFolders.Checked,
            _currentPreferences.ConfirmBeforeExecution,
            _currentPreferences.Language
        );

        UpdateJsonView();
        SavePreferencesAsync();
    }

    private void chkConfirmExecution_CheckedChanged(object sender, EventArgs e)
    {
        if (_isLoading) return;

        _currentPreferences = new Preferences(
            _currentPreferences.FolderRules,
            _currentPreferences.IgnoreExtensions,
            _currentPreferences.MaxFileSizeBytes,
            _currentPreferences.AutoCreateFolders,
            chkConfirmExecution.Checked,
            _currentPreferences.Language
        );

        UpdateJsonView();
        SavePreferencesAsync();
    }

    private void btnApplyJson_Click(object sender, EventArgs e)
    {
        try
        {
            var preferences = JsonSerializer.Deserialize<Preferences>(txtJsonView.Text,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            if (preferences != null)
            {
                _currentPreferences = preferences;
                UpdateUI();
                SavePreferencesAsync();
                MessageBox.Show("JSON preferences applied successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (JsonException ex)
        {
            MessageBox.Show($"Invalid JSON format: {ex.Message}", "JSON Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void tabControlPreferences_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabControlPreferences.SelectedTab?.Name == "tabPageJson")
        {
            UpdateJsonView();
        }
    }
}