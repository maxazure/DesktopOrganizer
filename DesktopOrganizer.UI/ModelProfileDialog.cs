using DesktopOrganizer.Domain;
using System.Diagnostics;

namespace DesktopOrganizer.UI;

/// <summary>
/// Dialog for managing LLM model profiles
/// </summary>
public partial class ModelProfileDialog : Form
{
    private readonly IModelProfileRepository _modelProfileRepository;
    private readonly ICredentialService _credentialService;
    private List<ModelProfile> _profiles = new();

    public ModelProfileDialog(IModelProfileRepository modelProfileRepository,
        ICredentialService credentialService)
    {
        _modelProfileRepository = modelProfileRepository;
        _credentialService = credentialService;
        InitializeComponent();
        LoadProfilesAsync();
    }

    private async void LoadProfilesAsync()
    {
        try
        {
            _profiles = await _modelProfileRepository.LoadAllAsync();
            UpdateProfilesList();
        }
        catch (Exception ex)
        {
            ShowError($"Error loading profiles: {ex.Message}");
        }
    }

    private void UpdateProfilesList()
    {
        listViewProfiles.Items.Clear();

        foreach (var profile in _profiles)
        {
            var item = new ListViewItem(profile.Name);
            item.SubItems.Add(profile.Provider);
            item.SubItems.Add(profile.ModelId);
            item.SubItems.Add(profile.BaseUrl);
            item.SubItems.Add(profile.IsDefault ? "Yes" : "No");
            item.Tag = profile;
            listViewProfiles.Items.Add(item);
        }

        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        var hasSelection = listViewProfiles.SelectedItems.Count > 0;
        btnEdit.Enabled = hasSelection;
        btnDelete.Enabled = hasSelection;
        btnSetDefault.Enabled = hasSelection;
        btnTestConnection.Enabled = hasSelection;
    }

    private void listViewProfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateButtonStates();
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        using var dialog = new ProfileEditDialog(_credentialService);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var newProfile = dialog.Profile;
            LogOperation($"Adding new profile: {newProfile.Name} ({newProfile.Provider})");

            _profiles.Add(newProfile);
            var success = await SaveProfilesAsync();

            if (success)
            {
                LogOperation($"Successfully added profile: {newProfile.Name}");
            }
            else
            {
                LogError($"Failed to add profile: {newProfile.Name}");
                // Remove the profile from local list if save failed
                _profiles.RemoveAll(p => p.Id == newProfile.Id);
            }
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        if (listViewProfiles.SelectedItems.Count == 0) return;

        var selectedProfile = (ModelProfile)listViewProfiles.SelectedItems[0].Tag;
        var originalProfile = selectedProfile; // Keep reference for rollback

        using var dialog = new ProfileEditDialog(_credentialService, selectedProfile);

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var editedProfile = dialog.Profile;
            var index = _profiles.FindIndex(p => p.Id == selectedProfile.Id);
            if (index >= 0)
            {
                LogOperation($"Editing profile: {originalProfile.Name} -> {editedProfile.Name}");

                _profiles[index] = editedProfile;
                var success = await SaveProfilesAsync();

                if (success)
                {
                    LogOperation($"Successfully edited profile: {editedProfile.Name}");
                }
                else
                {
                    LogError($"Failed to edit profile: {editedProfile.Name}");
                    // Rollback local change if save failed
                    _profiles[index] = originalProfile;
                    UpdateProfilesList();
                }
            }
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (listViewProfiles.SelectedItems.Count == 0) return;

        var selectedProfile = (ModelProfile)listViewProfiles.SelectedItems[0].Tag;

        var result = MessageBox.Show(
            $"Are you sure you want to delete the profile '{selectedProfile.Name}'?\n\nThis action cannot be undone and will also delete the associated API key.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            LogOperation($"Deleting profile: {selectedProfile.Name} (ID: {selectedProfile.Id})");

            // Keep backup for potential rollback
            var profileBackup = selectedProfile;
            var profileIndex = _profiles.FindIndex(p => p.Id == selectedProfile.Id);

            // Remove from local list
            _profiles.RemoveAll(p => p.Id == selectedProfile.Id);

            // Attempt to save the updated list
            var success = await SaveProfilesAsync();

            if (success)
            {
                LogOperation($"Successfully deleted profile: {selectedProfile.Name}");

                // Delete associated API key only after successful profile deletion
                try
                {
                    if (_credentialService != null)
                    {
                        var keyDeleted = await _credentialService.DeleteApiKeyAsync(selectedProfile.KeyRef);
                        if (keyDeleted)
                        {
                            LogOperation($"Successfully deleted API key for profile: {selectedProfile.Name}");
                        }
                        else
                        {
                            LogError($"Failed to delete API key for profile: {selectedProfile.Name}");
                            ShowInfo($"Profile '{selectedProfile.Name}' was deleted, but the API key could not be removed. You may need to delete it manually.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Error deleting API key for profile {selectedProfile.Name}: {ex.Message}");
                    ShowInfo($"Profile '{selectedProfile.Name}' was deleted, but there was an error removing the API key: {ex.Message}");
                }
            }
            else
            {
                LogError($"Failed to delete profile: {selectedProfile.Name}");
                // Rollback: add the profile back to the list
                if (profileIndex >= 0)
                {
                    _profiles.Insert(profileIndex, profileBackup);
                }
                else
                {
                    _profiles.Add(profileBackup);
                }
                UpdateProfilesList();
            }
        }
    }

    private async void btnSetDefault_Click(object sender, EventArgs e)
    {
        if (listViewProfiles.SelectedItems.Count == 0) return;

        var selectedProfile = (ModelProfile)listViewProfiles.SelectedItems[0].Tag;

        try
        {
            await _modelProfileRepository.SetDefaultAsync(selectedProfile.Id);

            // Update local list
            foreach (var profile in _profiles)
            {
                profile.IsDefault = profile.Id == selectedProfile.Id;
            }

            UpdateProfilesList();
            ShowInfo($"'{selectedProfile.Name}' is now the default profile.");
        }
        catch (Exception ex)
        {
            ShowError($"Error setting default profile: {ex.Message}");
        }
    }

    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        if (listViewProfiles.SelectedItems.Count == 0) return;

        var selectedProfile = (ModelProfile)listViewProfiles.SelectedItems[0].Tag;

        try
        {
            btnTestConnection.Enabled = false;
            btnTestConnection.Text = "Testing...";

            // Check if API key exists
            if (_credentialService != null)
            {
                var hasKey = await _credentialService.TestApiKeyAsync(selectedProfile.KeyRef);
                if (!hasKey)
                {
                    ShowError("API key not found. Please edit the profile and set the API key.");
                    return;
                }
            }

            // Test connection would require access to LLM client
            // For now, just check API key existence
            ShowInfo("API key is configured. Full connection test requires LLM client integration.");
        }
        catch (Exception ex)
        {
            ShowError($"Connection test failed: {ex.Message}");
        }
        finally
        {
            btnTestConnection.Enabled = true;
            btnTestConnection.Text = "Test Connection";
        }
    }

    private async void btnClearAllApiKeys_Click(object sender, EventArgs e)
    {
        var operationId = Guid.NewGuid().ToString("N")[..8];
        LogOperation($"[{operationId}] Starting clear all API keys operation");

        // Show confirmation dialog
        var result = MessageBox.Show(
            "Are you sure you want to clear ALL API keys?\n\n" +
            "This action will remove all stored API keys for all profiles and cannot be undone.\n\n" +
            "Profiles themselves will remain, but you will need to re-enter all API keys.",
            "Confirm Clear All API Keys",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2); // Default to "No"

        if (result != DialogResult.Yes)
        {
            LogOperation($"[{operationId}] User cancelled clear all API keys operation");
            return;
        }

        try
        {
            // Collect all unique key references from profiles
            var keyRefs = _profiles
                .Where(p => !string.IsNullOrWhiteSpace(p.KeyRef))
                .Select(p => p.KeyRef)
                .Distinct()
                .ToList();

            LogOperation($"[{operationId}] Found {keyRefs.Count} unique API key references to clear");

            if (keyRefs.Count == 0)
            {
                ShowInfo("No API keys found to clear.");
                LogOperation($"[{operationId}] No API keys to clear");
                return;
            }

            // Show progress by disabling button
            btnClearAllApiKeys.Enabled = false;
            btnClearAllApiKeys.Text = "Clearing...";

            int successCount = 0;
            int failureCount = 0;
            var failedKeys = new List<string>();

            // Clear each API key
            foreach (var keyRef in keyRefs)
            {
                try
                {
                    LogOperation($"[{operationId}] Attempting to delete API key: {keyRef}");
                    var deleted = await _credentialService.DeleteApiKeyAsync(keyRef);

                    if (deleted)
                    {
                        successCount++;
                        LogOperation($"[{operationId}] Successfully deleted API key: {keyRef}");
                    }
                    else
                    {
                        failureCount++;
                        failedKeys.Add(keyRef);
                        LogError($"[{operationId}] Failed to delete API key: {keyRef} (service returned false)");
                    }
                }
                catch (Exception ex)
                {
                    failureCount++;
                    failedKeys.Add(keyRef);
                    LogError($"[{operationId}] Exception while deleting API key {keyRef}: {ex.Message}");
                }
            }

            // Show results
            if (failureCount == 0)
            {
                LogOperation($"[{operationId}] Successfully cleared all {successCount} API keys");
                ShowSuccess($"Successfully cleared all {successCount} API key(s).");
            }
            else if (successCount > 0)
            {
                LogOperation($"[{operationId}] Partially successful: {successCount} cleared, {failureCount} failed");
                ShowInfo($"Partially successful:\n- Cleared: {successCount} API key(s)\n- Failed: {failureCount} API key(s)\n\nFailed keys: {string.Join(", ", failedKeys)}");
            }
            else
            {
                LogError($"[{operationId}] Failed to clear any API keys");
                ShowError($"Failed to clear any API keys.\n\nFailed keys: {string.Join(", ", failedKeys)}");
            }
        }
        catch (Exception ex)
        {
            LogError($"[{operationId}] Unexpected error during clear all API keys operation: {ex.Message}");
            LogError($"[{operationId}] Stack trace: {ex.StackTrace}");
            ShowError($"Unexpected error while clearing API keys: {ex.Message}");
        }
        finally
        {
            // Restore button state
            btnClearAllApiKeys.Enabled = true;
            btnClearAllApiKeys.Text = "Clear All API Keys";
            LogOperation($"[{operationId}] Clear all API keys operation completed");
        }
    }

    private void btnImportExport_Click(object sender, EventArgs e)
    {
        var menu = new ContextMenuStrip();

        var importItem = new ToolStripMenuItem("Import from JSON...");
        importItem.Click += ImportProfiles_Click;
        menu.Items.Add(importItem);

        var exportItem = new ToolStripMenuItem("Export to JSON...");
        exportItem.Click += ExportProfiles_Click;
        menu.Items.Add(exportItem);

        menu.Show(btnImportExport, new Point(0, btnImportExport.Height));
    }

    private async void ImportProfiles_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Import Model Profiles"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var json = await File.ReadAllTextAsync(dialog.FileName);
                var importedProfiles = System.Text.Json.JsonSerializer.Deserialize<List<ModelProfile>>(json);

                if (importedProfiles?.Any() == true)
                {
                    _profiles.AddRange(importedProfiles);
                    await SaveProfilesAsync();
                    ShowInfo($"Imported {importedProfiles.Count} profile(s).");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Import failed: {ex.Message}");
            }
        }
    }

    private async void ExportProfiles_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Export Model Profiles",
            FileName = "model_profiles.json"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(_profiles, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(dialog.FileName, json);
                ShowInfo("Profiles exported successfully.");
            }
            catch (Exception ex)
            {
                ShowError($"Export failed: {ex.Message}");
            }
        }
    }

    private async Task<bool> SaveProfilesAsync()
    {
        var operationId = Guid.NewGuid().ToString("N")[..8];
        LogOperation($"[{operationId}] Starting profile save operation with {_profiles.Count} profiles");

        // Create backup of current profiles for rollback
        List<ModelProfile> backupProfiles;
        try
        {
            backupProfiles = await _modelProfileRepository.LoadAllAsync();
            LogOperation($"[{operationId}] Created backup of {backupProfiles.Count} existing profiles");
        }
        catch (Exception ex)
        {
            LogError($"[{operationId}] Failed to create backup: {ex.Message}");
            ShowError("Failed to create backup before saving. Operation cancelled for safety.");
            return false;
        }

        // Validate profiles before saving
        var validationResult = ValidateProfiles(_profiles);
        if (!validationResult.IsValid)
        {
            LogError($"[{operationId}] Profile validation failed: {validationResult.ErrorMessage}");
            ShowError($"Profile validation failed: {validationResult.ErrorMessage}");
            return false;
        }
        LogOperation($"[{operationId}] Profile validation passed");

        try
        {
            // Attempt to save profiles
            LogOperation($"[{operationId}] Attempting to save {_profiles.Count} profiles to repository");
            await _modelProfileRepository.SaveAllAsync(_profiles);
            LogOperation($"[{operationId}] Successfully saved profiles to repository");

            // Verify the save by loading back
            var verificationProfiles = await _modelProfileRepository.LoadAllAsync();
            if (verificationProfiles.Count != _profiles.Count)
            {
                throw new InvalidOperationException($"Verification failed: Expected {_profiles.Count} profiles but found {verificationProfiles.Count}");
            }
            LogOperation($"[{operationId}] Save verification passed - {verificationProfiles.Count} profiles confirmed");

            // Update UI
            UpdateProfilesList();
            LogOperation($"[{operationId}] UI updated successfully");

            // Show success confirmation
            ShowSuccess($"Successfully saved {_profiles.Count} profile(s).");
            LogOperation($"[{operationId}] Operation completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            LogError($"[{operationId}] Save operation failed: {ex.Message}");
            LogError($"[{operationId}] Stack trace: {ex.StackTrace}");

            // Attempt rollback
            try
            {
                LogOperation($"[{operationId}] Attempting rollback to previous state");
                await _modelProfileRepository.SaveAllAsync(backupProfiles);
                _profiles = backupProfiles;
                UpdateProfilesList();
                LogOperation($"[{operationId}] Rollback completed successfully");

                ShowError($"Save operation failed and was rolled back. Error: {ex.Message}");
            }
            catch (Exception rollbackEx)
            {
                LogError($"[{operationId}] Rollback failed: {rollbackEx.Message}");
                ShowError($"Critical error: Save failed and rollback also failed. Manual intervention may be required.\n\nOriginal error: {ex.Message}\nRollback error: {rollbackEx.Message}");
            }

            return false;
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private static void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static void ShowInfo(string message)
    {
        MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void ShowSuccess(string message)
    {
        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static void LogOperation(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"[{timestamp}] [INFO] ModelProfileDialog: {message}";
        Debug.WriteLine(logMessage);
        Console.WriteLine(logMessage);
    }

    private static void LogError(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"[{timestamp}] [ERROR] ModelProfileDialog: {message}";
        Debug.WriteLine(logMessage);
        Console.WriteLine(logMessage);
    }

    private static ValidationResult ValidateProfiles(List<ModelProfile> profiles)
    {
        if (profiles == null || profiles.Count == 0)
        {
            return new ValidationResult(false, "Profile list cannot be empty");
        }

        // Check for duplicate names
        var duplicateNames = profiles
            .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateNames.Any())
        {
            return new ValidationResult(false, $"Duplicate profile names found: {string.Join(", ", duplicateNames)}");
        }

        // Check for duplicate IDs
        var duplicateIds = profiles
            .GroupBy(p => p.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Any())
        {
            return new ValidationResult(false, $"Duplicate profile IDs found: {string.Join(", ", duplicateIds)}");
        }

        // Check for multiple default profiles
        var defaultProfiles = profiles.Where(p => p.IsDefault).ToList();
        if (defaultProfiles.Count > 1)
        {
            return new ValidationResult(false, $"Multiple default profiles found: {string.Join(", ", defaultProfiles.Select(p => p.Name))}");
        }

        // Validate individual profiles
        foreach (var profile in profiles)
        {
            var profileValidation = ValidateProfile(profile);
            if (!profileValidation.IsValid)
            {
                return new ValidationResult(false, $"Profile '{profile.Name}': {profileValidation.ErrorMessage}");
            }
        }

        return new ValidationResult(true, string.Empty);
    }

    private static ValidationResult ValidateProfile(ModelProfile profile)
    {
        if (string.IsNullOrWhiteSpace(profile.Name))
        {
            return new ValidationResult(false, "Profile name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(profile.Provider))
        {
            return new ValidationResult(false, "Provider cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(profile.ModelId))
        {
            return new ValidationResult(false, "Model ID cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(profile.BaseUrl))
        {
            return new ValidationResult(false, "Base URL cannot be empty");
        }

        if (!Uri.TryCreate(profile.BaseUrl, UriKind.Absolute, out _))
        {
            return new ValidationResult(false, "Base URL is not a valid URL");
        }

        if (string.IsNullOrWhiteSpace(profile.KeyRef))
        {
            return new ValidationResult(false, "Key reference cannot be empty");
        }

        if (profile.TimeoutSeconds <= 0 || profile.TimeoutSeconds > 300)
        {
            return new ValidationResult(false, "Timeout must be between 1 and 300 seconds");
        }

        return new ValidationResult(true, string.Empty);
    }

    private class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        public ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}