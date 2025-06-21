namespace DesktopOrganizer.UI
{
    partial class PreferencesPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlPreferences = new TabControl();
            this.tabPageRules = new TabPage();
            this.splitContainer1 = new SplitContainer();
            this.groupBox1 = new GroupBox();
            this.listViewRules = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.panel1 = new Panel();
            this.btnDeleteRule = new Button();
            this.btnEditRule = new Button();
            this.btnAddRule = new Button();
            this.btnLoadTemplate = new Button();
            this.groupBox2 = new GroupBox();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.label1 = new Label();
            this.txtIgnoreExtensions = new TextBox();
            this.label2 = new Label();
            this.numMaxFileSize = new NumericUpDown();
            this.label3 = new Label();
            this.chkAutoCreateFolders = new CheckBox();
            this.chkConfirmExecution = new CheckBox();
            this.tabPageJson = new TabPage();
            this.txtJsonView = new TextBox();
            this.panel2 = new Panel();
            this.btnApplyJson = new Button();

            this.tabControlPreferences.SuspendLayout();
            this.tabPageRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFileSize)).BeginInit();
            this.tabPageJson.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();

            // tabControlPreferences
            this.tabControlPreferences.Controls.Add(this.tabPageRules);
            this.tabControlPreferences.Controls.Add(this.tabPageJson);
            this.tabControlPreferences.Dock = DockStyle.Fill;
            this.tabControlPreferences.Location = new Point(0, 0);
            this.tabControlPreferences.Name = "tabControlPreferences";
            this.tabControlPreferences.SelectedIndex = 0;
            this.tabControlPreferences.Size = new Size(800, 200);
            this.tabControlPreferences.TabIndex = 0;
            this.tabControlPreferences.SelectedIndexChanged += new EventHandler(this.tabControlPreferences_SelectedIndexChanged);

            // tabPageRules
            this.tabPageRules.Controls.Add(this.splitContainer1);
            this.tabPageRules.Location = new Point(4, 24);
            this.tabPageRules.Name = "tabPageRules";
            this.tabPageRules.Padding = new Padding(3);
            this.tabPageRules.Size = new Size(792, 172);
            this.tabPageRules.TabIndex = 0;
            this.tabPageRules.Text = "Folder Rules";
            this.tabPageRules.UseVisualStyleBackColor = true;

            // splitContainer1
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new Size(786, 166);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 0;

            // groupBox1
            this.groupBox1.Controls.Add(this.listViewRules);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = DockStyle.Fill;
            this.groupBox1.Location = new Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(400, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folder Rules";

            // listViewRules
            this.listViewRules.Columns.AddRange(new ColumnHeader[] {
                this.columnHeader1,
                this.columnHeader2});
            this.listViewRules.Dock = DockStyle.Fill;
            this.listViewRules.FullRowSelect = true;
            this.listViewRules.GridLines = true;
            this.listViewRules.Location = new Point(3, 19);
            this.listViewRules.MultiSelect = false;
            this.listViewRules.Name = "listViewRules";
            this.listViewRules.Size = new Size(394, 114);
            this.listViewRules.TabIndex = 0;
            this.listViewRules.UseCompatibleStateImageBehavior = false;
            this.listViewRules.View = View.Details;

            // columnHeader1
            this.columnHeader1.Text = "Folder";
            this.columnHeader1.Width = 120;

            // columnHeader2
            this.columnHeader2.Text = "Extensions";
            this.columnHeader2.Width = 250;

            // panel1
            this.panel1.Controls.Add(this.btnLoadTemplate);
            this.panel1.Controls.Add(this.btnDeleteRule);
            this.panel1.Controls.Add(this.btnEditRule);
            this.panel1.Controls.Add(this.btnAddRule);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(3, 133);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(394, 30);
            this.panel1.TabIndex = 1;

            // btnLoadTemplate
            this.btnLoadTemplate.Location = new Point(240, 3);
            this.btnLoadTemplate.Name = "btnLoadTemplate";
            this.btnLoadTemplate.Size = new Size(75, 23);
            this.btnLoadTemplate.TabIndex = 3;
            this.btnLoadTemplate.Text = "Template";
            this.btnLoadTemplate.UseVisualStyleBackColor = true;
            this.btnLoadTemplate.Click += new EventHandler(this.btnLoadTemplate_Click);

            // btnDeleteRule
            this.btnDeleteRule.Location = new Point(162, 3);
            this.btnDeleteRule.Name = "btnDeleteRule";
            this.btnDeleteRule.Size = new Size(75, 23);
            this.btnDeleteRule.TabIndex = 2;
            this.btnDeleteRule.Text = "Delete";
            this.btnDeleteRule.UseVisualStyleBackColor = true;
            this.btnDeleteRule.Click += new EventHandler(this.btnDeleteRule_Click);

            // btnEditRule
            this.btnEditRule.Location = new Point(81, 3);
            this.btnEditRule.Name = "btnEditRule";
            this.btnEditRule.Size = new Size(75, 23);
            this.btnEditRule.TabIndex = 1;
            this.btnEditRule.Text = "Edit";
            this.btnEditRule.UseVisualStyleBackColor = true;
            this.btnEditRule.Click += new EventHandler(this.btnEditRule_Click);

            // btnAddRule
            this.btnAddRule.Location = new Point(0, 3);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new Size(75, 23);
            this.btnAddRule.TabIndex = 0;
            this.btnAddRule.Text = "Add";
            this.btnAddRule.UseVisualStyleBackColor = true;
            this.btnAddRule.Click += new EventHandler(this.btnAddRule_Click);

            // groupBox2
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Dock = DockStyle.Fill;
            this.groupBox2.Location = new Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(382, 166);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General Settings";

            // tableLayoutPanel1
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtIgnoreExtensions, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numMaxFileSize, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkAutoCreateFolders, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkConfirmExecution, 1, 3);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(376, 144);
            this.tableLayoutPanel1.TabIndex = 0;

            // label1
            this.label1.Anchor = AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(104, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ignore Extensions:";

            // txtIgnoreExtensions
            this.txtIgnoreExtensions.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtIgnoreExtensions.Location = new Point(123, 3);
            this.txtIgnoreExtensions.Name = "txtIgnoreExtensions";
            this.txtIgnoreExtensions.PlaceholderText = ".tmp, .log, .cache";
            this.txtIgnoreExtensions.Size = new Size(250, 23);
            this.txtIgnoreExtensions.TabIndex = 1;
            this.txtIgnoreExtensions.TextChanged += new EventHandler(this.txtIgnoreExtensions_TextChanged);

            // label2
            this.label2.Anchor = AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(109, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Max File Size (MB):";

            // numMaxFileSize
            this.numMaxFileSize.Anchor = AnchorStyles.Left;
            this.numMaxFileSize.Location = new Point(123, 33);
            this.numMaxFileSize.Maximum = new decimal(new int[] { 10240, 0, 0, 0 });
            this.numMaxFileSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMaxFileSize.Name = "numMaxFileSize";
            this.numMaxFileSize.Size = new Size(120, 23);
            this.numMaxFileSize.TabIndex = 3;
            this.numMaxFileSize.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            this.numMaxFileSize.ValueChanged += new EventHandler(this.numMaxFileSize_ValueChanged);

            // label3
            this.label3.Anchor = AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(3, 65);
            this.label3.Name = "label3";
            this.label3.Size = new Size(49, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Options:";

            // chkAutoCreateFolders
            this.chkAutoCreateFolders.Anchor = AnchorStyles.Left;
            this.chkAutoCreateFolders.AutoSize = true;
            this.chkAutoCreateFolders.Checked = true;
            this.chkAutoCreateFolders.CheckState = CheckState.Checked;
            this.chkAutoCreateFolders.Location = new Point(123, 63);
            this.chkAutoCreateFolders.Name = "chkAutoCreateFolders";
            this.chkAutoCreateFolders.Size = new Size(131, 19);
            this.chkAutoCreateFolders.TabIndex = 5;
            this.chkAutoCreateFolders.Text = "Auto create folders";
            this.chkAutoCreateFolders.UseVisualStyleBackColor = true;
            this.chkAutoCreateFolders.CheckedChanged += new EventHandler(this.chkAutoCreateFolders_CheckedChanged);

            // chkConfirmExecution
            this.chkConfirmExecution.Anchor = AnchorStyles.Left;
            this.chkConfirmExecution.AutoSize = true;
            this.chkConfirmExecution.Checked = true;
            this.chkConfirmExecution.CheckState = CheckState.Checked;
            this.chkConfirmExecution.Location = new Point(123, 88);
            this.chkConfirmExecution.Name = "chkConfirmExecution";
            this.chkConfirmExecution.Size = new Size(143, 19);
            this.chkConfirmExecution.TabIndex = 6;
            this.chkConfirmExecution.Text = "Confirm before execute";
            this.chkConfirmExecution.UseVisualStyleBackColor = true;
            this.chkConfirmExecution.CheckedChanged += new EventHandler(this.chkConfirmExecution_CheckedChanged);

            // tabPageJson
            this.tabPageJson.Controls.Add(this.txtJsonView);
            this.tabPageJson.Controls.Add(this.panel2);
            this.tabPageJson.Location = new Point(4, 24);
            this.tabPageJson.Name = "tabPageJson";
            this.tabPageJson.Padding = new Padding(3);
            this.tabPageJson.Size = new Size(792, 172);
            this.tabPageJson.TabIndex = 1;
            this.tabPageJson.Text = "JSON Edit";
            this.tabPageJson.UseVisualStyleBackColor = true;

            // txtJsonView
            this.txtJsonView.Dock = DockStyle.Fill;
            this.txtJsonView.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.txtJsonView.Location = new Point(3, 3);
            this.txtJsonView.Multiline = true;
            this.txtJsonView.Name = "txtJsonView";
            this.txtJsonView.ScrollBars = ScrollBars.Both;
            this.txtJsonView.Size = new Size(786, 136);
            this.txtJsonView.TabIndex = 0;

            // panel2
            this.panel2.Controls.Add(this.btnApplyJson);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(3, 139);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(786, 30);
            this.panel2.TabIndex = 1;

            // btnApplyJson
            this.btnApplyJson.Location = new Point(3, 3);
            this.btnApplyJson.Name = "btnApplyJson";
            this.btnApplyJson.Size = new Size(100, 23);
            this.btnApplyJson.TabIndex = 0;
            this.btnApplyJson.Text = "Apply JSON";
            this.btnApplyJson.UseVisualStyleBackColor = true;
            this.btnApplyJson.Click += new EventHandler(this.btnApplyJson_Click);

            // PreferencesPane
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.tabControlPreferences);
            this.Name = "PreferencesPane";
            this.Size = new Size(800, 200);

            this.tabControlPreferences.ResumeLayout(false);
            this.tabPageRules.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFileSize)).EndInit();
            this.tabPageJson.ResumeLayout(false);
            this.tabPageJson.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlPreferences;
        private TabPage tabPageRules;
        private TabPage tabPageJson;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private ListView listViewRules;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Panel panel1;
        private Button btnDeleteRule;
        private Button btnEditRule;
        private Button btnAddRule;
        private Button btnLoadTemplate;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox txtIgnoreExtensions;
        private Label label2;
        private NumericUpDown numMaxFileSize;
        private Label label3;
        private CheckBox chkAutoCreateFolders;
        private CheckBox chkConfirmExecution;
        private TextBox txtJsonView;
        private Panel panel2;
        private Button btnApplyJson;
    }
}