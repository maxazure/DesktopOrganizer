namespace DesktopOrganizer.UI
{
    partial class ModelProfileDialog
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
            this.listViewProfiles = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader4 = new ColumnHeader();
            this.columnHeader5 = new ColumnHeader();
            this.panel1 = new Panel();
            this.btnClearAllApiKeys = new Button();
            this.btnImportExport = new Button();
            this.btnTestConnection = new Button();
            this.btnSetDefault = new Button();
            this.btnDelete = new Button();
            this.btnEdit = new Button();
            this.btnAdd = new Button();
            this.panel2 = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();

            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();

            // listViewProfiles
            this.listViewProfiles.Columns.AddRange(new ColumnHeader[] {
                this.columnHeader1,
                this.columnHeader2,
                this.columnHeader3,
                this.columnHeader4,
                this.columnHeader5});
            this.listViewProfiles.Dock = DockStyle.Fill;
            this.listViewProfiles.FullRowSelect = true;
            this.listViewProfiles.GridLines = true;
            this.listViewProfiles.Location = new Point(0, 0);
            this.listViewProfiles.MultiSelect = false;
            this.listViewProfiles.Name = "listViewProfiles";
            this.listViewProfiles.Size = new Size(784, 380);
            this.listViewProfiles.TabIndex = 0;
            this.listViewProfiles.UseCompatibleStateImageBehavior = false;
            this.listViewProfiles.View = View.Details;
            this.listViewProfiles.SelectedIndexChanged += new EventHandler(this.listViewProfiles_SelectedIndexChanged);

            // columnHeader1
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 150;

            // columnHeader2
            this.columnHeader2.Text = "Provider";
            this.columnHeader2.Width = 100;

            // columnHeader3
            this.columnHeader3.Text = "Model ID";
            this.columnHeader3.Width = 150;

            // columnHeader4
            this.columnHeader4.Text = "Base URL";
            this.columnHeader4.Width = 300;

            // columnHeader5
            this.columnHeader5.Text = "Default";
            this.columnHeader5.Width = 70;

            // panel1
            this.panel1.Controls.Add(this.btnClearAllApiKeys);
            this.panel1.Controls.Add(this.btnImportExport);
            this.panel1.Controls.Add(this.btnTestConnection);
            this.panel1.Controls.Add(this.btnSetDefault);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 380);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(784, 40);
            this.panel1.TabIndex = 1;

            // btnClearAllApiKeys
            this.btnClearAllApiKeys.BackColor = Color.FromArgb(255, 240, 240);
            this.btnClearAllApiKeys.ForeColor = Color.DarkRed;
            this.btnClearAllApiKeys.Location = new Point(610, 8);
            this.btnClearAllApiKeys.Name = "btnClearAllApiKeys";
            this.btnClearAllApiKeys.Size = new Size(120, 23);
            this.btnClearAllApiKeys.TabIndex = 6;
            this.btnClearAllApiKeys.Text = "Clear All API Keys";
            this.btnClearAllApiKeys.UseVisualStyleBackColor = false;
            this.btnClearAllApiKeys.Click += new EventHandler(this.btnClearAllApiKeys_Click);

            // btnImportExport
            this.btnImportExport.Location = new Point(500, 8);
            this.btnImportExport.Name = "btnImportExport";
            this.btnImportExport.Size = new Size(100, 23);
            this.btnImportExport.TabIndex = 5;
            this.btnImportExport.Text = "Import/Export ▼";
            this.btnImportExport.UseVisualStyleBackColor = true;
            this.btnImportExport.Click += new EventHandler(this.btnImportExport_Click);

            // btnTestConnection
            this.btnTestConnection.Enabled = false;
            this.btnTestConnection.Location = new Point(394, 8);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new Size(100, 23);
            this.btnTestConnection.TabIndex = 4;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new EventHandler(this.btnTestConnection_Click);

            // btnSetDefault
            this.btnSetDefault.Enabled = false;
            this.btnSetDefault.Location = new Point(306, 8);
            this.btnSetDefault.Name = "btnSetDefault";
            this.btnSetDefault.Size = new Size(82, 23);
            this.btnSetDefault.TabIndex = 3;
            this.btnSetDefault.Text = "Set Default";
            this.btnSetDefault.UseVisualStyleBackColor = true;
            this.btnSetDefault.Click += new EventHandler(this.btnSetDefault_Click);

            // btnDelete
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new Point(162, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // btnEdit
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new Point(81, 8);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);

            // btnAdd
            this.btnAdd.Location = new Point(0, 8);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // panel2
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 420);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(784, 41);
            this.panel2.TabIndex = 2;

            // btnCancel
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(697, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // btnOK
            this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnOK.Location = new Point(616, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // ModelProfileDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new Size(784, 461);
            this.Controls.Add(this.listViewProfiles);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MinimizeBox = false;
            this.Name = "ModelProfileDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Model Profile Settings";

            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private ListView listViewProfiles;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private Panel panel1;
        private Button btnClearAllApiKeys;
        private Button btnImportExport;
        private Button btnTestConnection;
        private Button btnSetDefault;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private Panel panel2;
        private Button btnCancel;
        private Button btnOK;
    }
}