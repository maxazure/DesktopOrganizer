namespace DesktopOrganizer.UI;

/// <summary>
/// Generic dialog for selecting from a list of items
/// </summary>
public partial class ListSelectionDialog : Form
{
    public string SelectedItem { get; private set; } = string.Empty;

    public ListSelectionDialog(string title, string[] items)
    {
        InitializeComponent();
        Text = title;

        listBox1.Items.AddRange(items);
        if (items.Length > 0)
        {
            listBox1.SelectedIndex = 0;
        }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (listBox1.SelectedItem == null)
        {
            MessageBox.Show("Please select an item.", "Selection Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SelectedItem = listBox1.SelectedItem.ToString() ?? string.Empty;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void listBox1_DoubleClick(object sender, EventArgs e)
    {
        if (listBox1.SelectedItem != null)
        {
            btnOK_Click(sender, e);
        }
    }
}

/// <summary>
/// Designer code for ListSelectionDialog
/// </summary>
public partial class ListSelectionDialog
{
    private System.ComponentModel.IContainer components = null;
    private ListBox listBox1;
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
        this.listBox1 = new ListBox();
        this.btnOK = new Button();
        this.btnCancel = new Button();
        this.SuspendLayout();

        // listBox1
        this.listBox1.FormattingEnabled = true;
        this.listBox1.ItemHeight = 15;
        this.listBox1.Location = new Point(12, 12);
        this.listBox1.Name = "listBox1";
        this.listBox1.Size = new Size(360, 199);
        this.listBox1.TabIndex = 0;
        this.listBox1.DoubleClick += new EventHandler(this.listBox1_DoubleClick);

        // btnOK
        this.btnOK.Location = new Point(216, 226);
        this.btnOK.Name = "btnOK";
        this.btnOK.Size = new Size(75, 23);
        this.btnOK.TabIndex = 1;
        this.btnOK.Text = "OK";
        this.btnOK.UseVisualStyleBackColor = true;
        this.btnOK.Click += new EventHandler(this.btnOK_Click);

        // btnCancel
        this.btnCancel.DialogResult = DialogResult.Cancel;
        this.btnCancel.Location = new Point(297, 226);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new Size(75, 23);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        // ListSelectionDialog
        this.AcceptButton = this.btnOK;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new Size(384, 261);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnOK);
        this.Controls.Add(this.listBox1);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ListSelectionDialog";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Select Item";
        this.ResumeLayout(false);
    }
}