namespace DesktopOrganizer.UI
{
    partial class ProfileEditDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.label1 = new Label();
            this.txtName = new TextBox();
            this.label2 = new Label();
            this.cmbProvider = new ComboBox();
            this.label3 = new Label();
            this.txtModelId = new TextBox();
            this.label4 = new Label();
            this.txtBaseUrl = new TextBox();
            this.label5 = new Label();
            this.panel1 = new Panel();
            this.btnSetApiKey = new Button();
            this.txtKeyRef = new TextBox();
            this.label6 = new Label();
            this.numTimeout = new NumericUpDown();
            this.chkIsDefault = new CheckBox();
            this.panel2 = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();

            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();

            // tableLayoutPanel1
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbProvider, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtModelId, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtBaseUrl, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.numTimeout, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkIsDefault, 1, 6);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(464, 225);
            this.tableLayoutPanel1.TabIndex = 0;

            // label1
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(42, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";

            // txtName
            this.txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtName.Location = new Point(123, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(338, 23);
            this.txtName.TabIndex = 1;

            // label2
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(55, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Provider:";

            // cmbProvider
            this.cmbProvider.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cmbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProvider.FormattingEnabled = true;
            this.cmbProvider.Items.AddRange(new object[] { "DeepSeek", "OpenAI", "Anthropic" });
            this.cmbProvider.Location = new Point(123, 33);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new Size(338, 23);
            this.cmbProvider.TabIndex = 3;
            this.cmbProvider.SelectedIndexChanged += new EventHandler(this.cmbProvider_SelectedIndexChanged);

            // label3
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 67);
            this.label3.Name = "label3";
            this.label3.Size = new Size(60, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Model ID:";

            // txtModelId
            this.txtModelId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtModelId.Location = new Point(123, 63);
            this.txtModelId.Name = "txtModelId";
            this.txtModelId.Size = new Size(338, 23);
            this.txtModelId.TabIndex = 5;

            // label4
            this.label4.Anchor = AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(3, 97);
            this.label4.Name = "label4";
            this.label4.Size = new Size(58, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Base URL:";

            // txtBaseUrl
            this.txtBaseUrl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtBaseUrl.Location = new Point(123, 93);
            this.txtBaseUrl.Name = "txtBaseUrl";
            this.txtBaseUrl.Size = new Size(338, 23);
            this.txtBaseUrl.TabIndex = 7;

            // label5
            this.label5.Anchor = AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(3, 127);
            this.label5.Name = "label5";
            this.label5.Size = new Size(79, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Key Reference:";

            // panel1
            this.panel1.Controls.Add(this.btnSetApiKey);
            this.panel1.Controls.Add(this.txtKeyRef);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(120, 120);
            this.panel1.Margin = new Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(344, 30);
            this.panel1.TabIndex = 9;

            // btnSetApiKey
            this.btnSetApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnSetApiKey.Location = new Point(266, 3);
            this.btnSetApiKey.Name = "btnSetApiKey";
            this.btnSetApiKey.Size = new Size(75, 23);
            this.btnSetApiKey.TabIndex = 1;
            this.btnSetApiKey.Text = "Set API Key";
            this.btnSetApiKey.UseVisualStyleBackColor = true;
            this.btnSetApiKey.Click += new EventHandler(this.btnSetApiKey_Click);

            // txtKeyRef
            this.txtKeyRef.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtKeyRef.Location = new Point(3, 3);
            this.txtKeyRef.Name = "txtKeyRef";
            this.txtKeyRef.Size = new Size(257, 23);
            this.txtKeyRef.TabIndex = 0;

            // label6
            this.label6.Anchor = AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(3, 157);
            this.label6.Name = "label6";
            this.label6.Size = new Size(87, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Timeout (sec):";

            // numTimeout
            this.numTimeout.Anchor = AnchorStyles.Left;
            this.numTimeout.Location = new Point(123, 153);
            this.numTimeout.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            this.numTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new Size(120, 23);
            this.numTimeout.TabIndex = 11;
            this.numTimeout.Value = new decimal(new int[] { 60, 0, 0, 0 });

            // chkIsDefault
            this.chkIsDefault.Anchor = AnchorStyles.Left;
            this.chkIsDefault.AutoSize = true;
            this.chkIsDefault.Location = new Point(123, 183);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Size = new Size(105, 19);
            this.chkIsDefault.TabIndex = 12;
            this.chkIsDefault.Text = "Set as default";
            this.chkIsDefault.UseVisualStyleBackColor = true;

            // panel2
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(10, 235);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(464, 40);
            this.panel2.TabIndex = 1;

            // btnCancel
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(386, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // btnOK
            this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnOK.Location = new Point(305, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // ProfileEditDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(484, 285);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProfileEditDialog";
            this.Padding = new Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Profile";

            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cmbProvider;
        private Label label3;
        private TextBox txtModelId;
        private Label label4;
        private TextBox txtBaseUrl;
        private Label label5;
        private Panel panel1;
        private Button btnSetApiKey;
        private TextBox txtKeyRef;
        private Label label6;
        private NumericUpDown numTimeout;
        private CheckBox chkIsDefault;
        private Panel panel2;
        private Button btnCancel;
        private Button btnOK;
    }
}