namespace DesktopOrganizer.UI;

/// <summary>
/// Dialog for editing folder rules
/// </summary>
public partial class FolderRuleDialog : Form
{
    public string FolderName => txtFolderName.Text.Trim();
    public List<string> Extensions => txtExtensions.Text
        .Split(',')
        .Select(ext => ext.Trim())
        .Where(ext => !string.IsNullOrWhiteSpace(ext))
        .Select(ext => ext.StartsWith('.') ? ext : '.' + ext)
        .ToList();

    public FolderRuleDialog(string? folderName = null, List<string>? extensions = null)
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(folderName))
        {
            txtFolderName.Text = folderName;
            Text = "Edit Folder Rule";
        }
        else
        {
            Text = "Add Folder Rule";
        }

        if (extensions?.Any() == true)
        {
            txtExtensions.Text = string.Join(", ", extensions);
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (!ValidateInput())
            return;

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtFolderName.Text))
        {
            MessageBox.Show("Please enter a folder name.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFolderName.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtExtensions.Text))
        {
            MessageBox.Show("Please enter at least one file extension.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtExtensions.Focus();
            return false;
        }

        // Check for invalid folder name characters
        var invalidChars = new char[] { '<', '>', ':', '"', '|', '?', '*', '\\', '/' };
        if (txtFolderName.Text.IndexOfAny(invalidChars) >= 0)
        {
            MessageBox.Show("Folder name contains invalid characters.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFolderName.Focus();
            return false;
        }

        return true;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}

/// <summary>
/// Designer code for FolderRuleDialog
/// </summary>
public partial class FolderRuleDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label label1;
    private TextBox txtFolderName;
    private Label label2;
    private TextBox txtExtensions;
    private Label label3;
    private Button btnOK;
    private Button btnCancel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.label1 = new Label();
        this.txtFolderName = new TextBox();
        this.label2 = new Label();
        this.txtExtensions = new TextBox();
        this.label3 = new Label();
        this.btnOK = new Button();
        this.btnCancel = new Button();
        this.SuspendLayout();

        // label1
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 15);
        this.label1.Name = "label1";
        this.label1.Size = new Size(78, 15);
        this.label1.TabIndex = 0;
        this.label1.Text = "Folder Name:";

        // txtFolderName
        this.txtFolderName.Location = new Point(12, 33);
        this.txtFolderName.Name = "txtFolderName";
        this.txtFolderName.Size = new Size(360, 23);
        this.txtFolderName.TabIndex = 1;

        // label2
        this.label2.AutoSize = true;
        this.label2.Location = new Point(12, 70);
        this.label2.Name = "label2";
        this.label2.Size = new Size(69, 15);
        this.label2.TabIndex = 2;
        this.label2.Text = "Extensions:";

        // txtExtensions
        this.txtExtensions.Location = new Point(12, 88);
        this.txtExtensions.Name = "txtExtensions";
        this.txtExtensions.PlaceholderText = ".pdf, .doc, .docx, .txt";
        this.txtExtensions.Size = new Size(360, 23);
        this.txtExtensions.TabIndex = 3;

        // label3
        this.label3.AutoSize = true;
        this.label3.ForeColor = SystemColors.GrayText;
        this.label3.Location = new Point(12, 114);
        this.label3.Name = "label3";
        this.label3.Size = new Size(280, 15);
        this.label3.TabIndex = 4;
        this.label3.Text = "Separate multiple extensions with commas";

        // btnOK
        this.btnOK.Location = new Point(216, 145);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new Size(75, 23);
        this.btnOK.TabIndex = 5;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new EventHandler(this.btnOK_Click);

        // btnCancel
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(297, 145);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(75, 23);
        this.btnCancel.TabIndex = 6;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        // FolderRuleDialog
        this.AcceptButton = this.btnOK;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new Size(384, 186);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.txtExtensions);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.txtFolderName);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "FolderRuleDialog";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Folder Rule";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}