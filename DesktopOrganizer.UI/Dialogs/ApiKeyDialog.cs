namespace DesktopOrganizer.UI;

/// <summary>
/// Simple dialog for entering API keys securely
/// </summary>
public partial class ApiKeyDialog : Form
{
    public string ApiKey => txtApiKey.Text;

    public ApiKeyDialog()
    {
        InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtApiKey.Text))
        {
            MessageBox.Show("Please enter an API key.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtApiKey.Focus();
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void btnShowHide_Click(object sender, EventArgs e)
    {
        if (txtApiKey.PasswordChar == '*')
        {
            txtApiKey.PasswordChar = '\0';
            btnShowHide.Text = "Hide";
        }
        else
        {
            txtApiKey.PasswordChar = '*';
            btnShowHide.Text = "Show";
        }
    }
}

/// <summary>
/// Partial class for the designer code
/// </summary>
public partial class ApiKeyDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label label1;
    private TextBox txtApiKey;
    private Button btnShowHide;
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
        this.txtApiKey = new TextBox();
        this.btnShowHide = new Button();
        this.btnOK = new Button();
        this.btnCancel = new Button();
        this.SuspendLayout();

        // label1
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 15);
        this.label1.Name = "label1";
        this.label1.Size = new Size(50, 15);
        this.label1.TabIndex = 0;
        this.label1.Text = "API Key:";

        // txtApiKey
        this.txtApiKey.Location = new Point(12, 33);
        this.txtApiKey.Name = "txtApiKey";
        this.txtApiKey.PasswordChar = '*';
        this.txtApiKey.Size = new Size(300, 23);
        this.txtApiKey.TabIndex = 1;

        // btnShowHide
        this.btnShowHide.Location = new Point(318, 33);
        this.btnShowHide.Name = "btnShowHide";
        this.btnShowHide.Size = new Size(50, 23);
        this.btnShowHide.TabIndex = 2;
        this.btnShowHide.Text = "Show";
        this.btnShowHide.UseVisualStyleBackColor = true;
        this.btnShowHide.Click += new EventHandler(this.btnShowHide_Click);

        // btnOK
        this.btnOK.Location = new Point(212, 70);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new Size(75, 23);
        this.btnOK.TabIndex = 3;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new EventHandler(this.btnOK_Click);

        // btnCancel
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(293, 70);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(75, 23);
        this.btnCancel.TabIndex = 4;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        // ApiKeyDialog
        this.AcceptButton = this.btnOK;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new Size(384, 111);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.btnShowHide);
        this.Controls.Add(this.txtApiKey);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ApiKeyDialog";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Enter API Key";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}