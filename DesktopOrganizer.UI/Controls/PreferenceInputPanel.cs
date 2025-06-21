using System.ComponentModel;
using DesktopOrganizer.Domain;

namespace DesktopOrganizer.UI.Controls;

/// <summary>
/// User control for inputting natural language preferences
/// </summary>
public partial class PreferenceInputPanel : UserControl
{
    public event EventHandler<PreferenceChangedEventArgs>? PreferenceChanged;
    public event EventHandler<TemplateSelectedEventArgs>? TemplateSelected;

    private List<PreferenceTemplate> _templates = new();
    private string _preferenceText = string.Empty;

    public PreferenceInputPanel()
    {
        InitializeComponent();
        ApplyModernStyling();
        SetupPlaceholder();
    }

    [Category("Data")]
    [Description("The preference text entered by user")]
    public string PreferenceText
    {
        get => _preferenceText;
        set
        {
            _preferenceText = value;
            txtPreference.Text = value;
        }
    }

    [Category("Data")]
    [Description("Available preference templates")]
    public List<PreferenceTemplate> Templates
    {
        get => _templates;
        set
        {
            _templates = value ?? new List<PreferenceTemplate>();
            UpdateTemplateComboBox();
        }
    }

    [Category("Behavior")]
    [Description("Whether the input is read-only")]
    public bool ReadOnly
    {
        get => txtPreference.ReadOnly;
        set
        {
            txtPreference.ReadOnly = value;
            btnClear.Enabled = !value;
            cmbTemplates.Enabled = !value;
        }
    }

    private void ApplyModernStyling()
    {
        // Modern styling
        groupBox.BackColor = Color.FromArgb(249, 250, 251);
        txtPreference.BorderStyle = BorderStyle.FixedSingle;
        
        // Round button corners
        btnClear.Region = CreateRoundedRegion(btnClear.Size, 4);
        
        // ComboBox styling
        cmbTemplates.FlatStyle = FlatStyle.Flat;
    }

    private void SetupPlaceholder()
    {
        // Custom placeholder handling for better UX
        txtPreference.GotFocus += (s, e) =>
        {
            if (txtPreference.ForeColor == Color.Gray)
            {
                txtPreference.Text = "";
                txtPreference.ForeColor = Color.FromArgb(31, 41, 55);
            }
        };

        txtPreference.LostFocus += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtPreference.Text))
            {
                txtPreference.ForeColor = Color.Gray;
            }
        };
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

    private void UpdateTemplateComboBox()
    {
        cmbTemplates.Items.Clear();
        cmbTemplates.Items.Add("使用模板 ▼");
        
        if (_templates.Any())
        {
            foreach (var template in _templates)
            {
                cmbTemplates.Items.Add(template.Name);
            }
        }
        
        cmbTemplates.SelectedIndex = 0;
    }

    public void ClearPreference()
    {
        txtPreference.Clear();
        _preferenceText = string.Empty;
        OnPreferenceChanged();
    }

    public void SetPreference(string preference)
    {
        PreferenceText = preference;
        OnPreferenceChanged();
    }

    public void ApplyTemplate(PreferenceTemplate template)
    {
        if (template != null)
        {
            SetPreference(template.Content);
            OnTemplateSelected(template);
        }
    }

    private void txtPreference_TextChanged(object? sender, EventArgs e)
    {
        _preferenceText = txtPreference.Text;
        OnPreferenceChanged();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        ClearPreference();
    }

    private void cmbTemplates_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbTemplates.SelectedIndex > 0 && cmbTemplates.SelectedIndex <= _templates.Count)
        {
            var selectedTemplate = _templates[cmbTemplates.SelectedIndex - 1];
            ApplyTemplate(selectedTemplate);
        }
        
        // Reset to default text
        Invoke(new Action(() =>
        {
            cmbTemplates.SelectedIndex = 0;
        }));
    }

    protected virtual void OnPreferenceChanged()
    {
        PreferenceChanged?.Invoke(this, new PreferenceChangedEventArgs(_preferenceText));
    }

    protected virtual void OnTemplateSelected(PreferenceTemplate template)
    {
        TemplateSelected?.Invoke(this, new TemplateSelectedEventArgs(template));
    }

    // Focus methods for better UX
    public void FocusOnInput()
    {
        txtPreference.Focus();
    }

    public void SelectAllText()
    {
        txtPreference.SelectAll();
    }

    // Validation
    public bool HasValidPreference()
    {
        return !string.IsNullOrWhiteSpace(_preferenceText) && _preferenceText.Length >= 5;
    }

    public void ShowValidationError(string message)
    {
        // Could implement tooltip or status message here
        txtPreference.BackColor = Color.FromArgb(254, 242, 242); // Light red
        
        // Reset after 2 seconds
        var timer = new System.Windows.Forms.Timer();
        timer.Interval = 2000;
        timer.Tick += (s, e) =>
        {
            txtPreference.BackColor = Color.White;
            timer.Stop();
            timer.Dispose();
        };
        timer.Start();
    }
}

/// <summary>
/// Event arguments for preference changed events
/// </summary>
public class PreferenceChangedEventArgs : EventArgs
{
    public string PreferenceText { get; }

    public PreferenceChangedEventArgs(string preferenceText)
    {
        PreferenceText = preferenceText;
    }
}

/// <summary>
/// Event arguments for template selected events
/// </summary>
public class TemplateSelectedEventArgs : EventArgs
{
    public PreferenceTemplate Template { get; }

    public TemplateSelectedEventArgs(PreferenceTemplate template)
    {
        Template = template;
    }
}