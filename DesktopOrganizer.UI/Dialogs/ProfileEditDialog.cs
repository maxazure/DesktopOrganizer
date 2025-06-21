using DesktopOrganizer.Domain;
using System.Diagnostics;

namespace DesktopOrganizer.UI;

/// <summary>
/// Dialog for editing individual model profiles
/// </summary>
public partial class ProfileEditDialog : Form
{
    private readonly ICredentialService? _credentialService;
    public ModelProfile Profile { get; private set; }

    public ProfileEditDialog(ICredentialService? credentialService, ModelProfile? existingProfile = null)
    {
        _credentialService = credentialService;
        InitializeComponent();

        if (existingProfile != null)
        {
            LoadProfile(existingProfile);
            Text = "Edit Model Profile";
        }
        else
        {
            Text = "Add Model Profile";
            SetDefaults();
        }
    }

    private void SetDefaults()
    {
        txtName.Text = "";
        cmbProvider.SelectedIndex = 0; // DeepSeek
        txtModelId.Text = "deepseek-chat";
        txtBaseUrl.Text = "https://api.deepseek.com";
        txtKeyRef.Text = "";
        numTimeout.Value = 60;
        chkIsDefault.Checked = false;
    }

    private void LoadProfile(ModelProfile profile)
    {
        txtName.Text = profile.Name;
        cmbProvider.Text = profile.Provider;
        txtModelId.Text = profile.ModelId;
        txtBaseUrl.Text = profile.BaseUrl;
        txtKeyRef.Text = profile.KeyRef;
        numTimeout.Value = profile.TimeoutSeconds;
        chkIsDefault.Checked = profile.IsDefault;

        Profile = profile;
    }

    private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        var provider = cmbProvider.Text;

        switch (provider)
        {
            case "DeepSeek":
                txtBaseUrl.Text = "https://api.deepseek.com";
                txtModelId.Text = "deepseek-chat";
                break;
            case "OpenAI":
                txtBaseUrl.Text = "https://api.openai.com";
                txtModelId.Text = "gpt-4";
                break;
            case "Anthropic":
                txtBaseUrl.Text = "https://api.anthropic.com";
                txtModelId.Text = "claude-3-sonnet-20240229";
                break;
        }
    }

    private async void btnSetApiKey_Click(object sender, EventArgs e)
    {
        var keyRef = txtKeyRef.Text.Trim();
        if (string.IsNullOrEmpty(keyRef))
        {
            MessageBox.Show("Please enter a key reference first.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtKeyRef.Focus();
            return;
        }

        // Validate key reference format
        if (!IsValidKeyReference(keyRef))
        {
            MessageBox.Show("Key reference must contain only letters, numbers, hyphens, and underscores.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtKeyRef.Focus();
            return;
        }

        var operationId = Guid.NewGuid().ToString("N")[..8];
        LogOperation($"[{operationId}] Starting API key save operation for key reference: {keyRef}");

        using var dialog = new ApiKeyDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var apiKey = dialog.ApiKey;

            // Validate API key
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                LogError($"[{operationId}] API key is empty or whitespace");
                MessageBox.Show("API key cannot be empty.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (apiKey.Length < 10)
            {
                LogError($"[{operationId}] API key too short: {apiKey.Length} characters");
                MessageBox.Show("API key appears to be too short. Please verify you entered the complete key.", "Validation Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (_credentialService != null)
            {
                try
                {
                    LogOperation($"[{operationId}] Attempting to save API key to credential service");

                    // Check if key already exists
                    var existingKey = await _credentialService.GetApiKeyAsync(keyRef);
                    bool isUpdate = !string.IsNullOrEmpty(existingKey);

                    if (isUpdate)
                    {
                        LogOperation($"[{operationId}] Updating existing API key");
                        var confirmUpdate = MessageBox.Show(
                            $"An API key already exists for '{keyRef}'. Do you want to replace it?",
                            "Confirm Update",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirmUpdate != DialogResult.Yes)
                        {
                            LogOperation($"[{operationId}] User cancelled API key update");
                            return;
                        }
                    }
                    else
                    {
                        LogOperation($"[{operationId}] Creating new API key");
                    }

                    var success = await _credentialService.SaveApiKeyAsync(keyRef, apiKey);

                    if (success)
                    {
                        LogOperation($"[{operationId}] Successfully saved API key");

                        // Verify the save by reading it back
                        var verificationKey = await _credentialService.GetApiKeyAsync(keyRef);
                        if (string.IsNullOrEmpty(verificationKey))
                        {
                            LogError($"[{operationId}] Verification failed: Could not retrieve saved API key");
                            MessageBox.Show("API key was saved but verification failed. Please test the configuration.", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            LogOperation($"[{operationId}] API key verification successful");
                            string actionText = isUpdate ? "updated" : "saved";
                            MessageBox.Show($"API key {actionText} successfully and verified.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        LogError($"[{operationId}] Failed to save API key - credential service returned false");
                        MessageBox.Show("Failed to save API key. Please check your system permissions and try again.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    LogError($"[{operationId}] Exception during API key save: {ex.Message}");
                    LogError($"[{operationId}] Stack trace: {ex.StackTrace}");
                    MessageBox.Show($"Error saving API key: {ex.Message}\n\nPlease check the application logs for more details.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                LogError($"[{operationId}] Credential service is null");
                MessageBox.Show("Credential service is not available. Cannot save API key.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            LogOperation($"[{operationId}] User cancelled API key dialog");
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (!ValidateInput())
            return;

        Profile = new ModelProfile(
            txtName.Text.Trim(),
            cmbProvider.Text,
            txtBaseUrl.Text.Trim(),
            txtModelId.Text.Trim(),
            txtKeyRef.Text.Trim(),
            (int)numTimeout.Value,
            chkIsDefault.Checked
        );

        if (Profile?.Id != null && Profile.Id != Guid.Empty.ToString())
        {
            // Keep existing ID if editing
            Profile = new ModelProfile(
                Profile.Name,
                Profile.Provider,
                Profile.BaseUrl,
                Profile.ModelId,
                Profile.KeyRef,
                Profile.TimeoutSeconds,
                Profile.IsDefault
            );
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Please enter a profile name.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(cmbProvider.Text))
        {
            MessageBox.Show("Please select a provider.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbProvider.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtBaseUrl.Text))
        {
            MessageBox.Show("Please enter a base URL.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBaseUrl.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtModelId.Text))
        {
            MessageBox.Show("Please enter a model ID.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtModelId.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtKeyRef.Text))
        {
            MessageBox.Show("Please enter a key reference.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtKeyRef.Focus();
            return false;
        }

        if (!Uri.TryCreate(txtBaseUrl.Text, UriKind.Absolute, out _))
        {
            MessageBox.Show("Please enter a valid URL.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBaseUrl.Focus();
            return false;
        }

        return true;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private static bool IsValidKeyReference(string keyRef)
    {
        if (string.IsNullOrWhiteSpace(keyRef))
            return false;

        // Key reference should only contain alphanumeric characters, hyphens, and underscores
        return keyRef.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }

    private static void LogOperation(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"[{timestamp}] [INFO] ProfileEditDialog: {message}";
        Debug.WriteLine(logMessage);
        Console.WriteLine(logMessage);
    }

    private static void LogError(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var logMessage = $"[{timestamp}] [ERROR] ProfileEditDialog: {message}";
        Debug.WriteLine(logMessage);
        Console.WriteLine(logMessage);
    }
}