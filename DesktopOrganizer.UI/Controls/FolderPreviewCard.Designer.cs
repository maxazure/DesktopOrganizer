namespace DesktopOrganizer.UI.Controls
{
    partial class FolderPreviewCard
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
            mainPanel = new Panel();
            headerPanel = new Panel();
            lblFolderName = new Label();
            chkKeepOnDesktop = new CheckBox();
            lblDescription = new Label();
            fileListPanel = new Panel();
            lstFiles = new ListBox();
            lblFileCount = new Label();
            mainPanel.SuspendLayout();
            headerPanel.SuspendLayout();
            fileListPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.White;
            mainPanel.BorderStyle = BorderStyle.FixedSingle;
            mainPanel.Controls.Add(fileListPanel);
            mainPanel.Controls.Add(lblDescription);
            mainPanel.Controls.Add(headerPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Margin = new Padding(5);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(10);
            mainPanel.Size = new Size(280, 200);
            mainPanel.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(chkKeepOnDesktop);
            headerPanel.Controls.Add(lblFolderName);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(10, 10);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(258, 35);
            headerPanel.TabIndex = 0;
            // 
            // lblFolderName
            // 
            lblFolderName.AutoSize = true;
            lblFolderName.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            lblFolderName.ForeColor = Color.FromArgb(31, 41, 55);
            lblFolderName.Location = new Point(0, 5);
            lblFolderName.Name = "lblFolderName";
            lblFolderName.Size = new Size(90, 22);
            lblFolderName.TabIndex = 0;
            lblFolderName.Text = "文件夹名称";
            // 
            // chkKeepOnDesktop
            // 
            chkKeepOnDesktop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkKeepOnDesktop.AutoSize = true;
            chkKeepOnDesktop.Font = new Font("Microsoft YaHei UI", 9F);
            chkKeepOnDesktop.Location = new Point(180, 8);
            chkKeepOnDesktop.Name = "chkKeepOnDesktop";
            chkKeepOnDesktop.Size = new Size(75, 21);
            chkKeepOnDesktop.TabIndex = 1;
            chkKeepOnDesktop.Text = "保留桌面";
            chkKeepOnDesktop.UseVisualStyleBackColor = true;
            chkKeepOnDesktop.CheckedChanged += chkKeepOnDesktop_CheckedChanged;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Top;
            lblDescription.Font = new Font("Microsoft YaHei UI", 9F);
            lblDescription.ForeColor = Color.FromArgb(107, 114, 128);
            lblDescription.Location = new Point(10, 45);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(258, 20);
            lblDescription.TabIndex = 1;
            lblDescription.Text = "文件夹描述";
            // 
            // fileListPanel
            // 
            fileListPanel.Controls.Add(lstFiles);
            fileListPanel.Controls.Add(lblFileCount);
            fileListPanel.Dock = DockStyle.Fill;
            fileListPanel.Location = new Point(10, 65);
            fileListPanel.Name = "fileListPanel";
            fileListPanel.Size = new Size(258, 123);
            fileListPanel.TabIndex = 2;
            // 
            // lstFiles
            // 
            lstFiles.BorderStyle = BorderStyle.None;
            lstFiles.Dock = DockStyle.Fill;
            lstFiles.Font = new Font("Microsoft YaHei UI", 8.25F);
            lstFiles.ForeColor = Color.FromArgb(75, 85, 99);
            lstFiles.FormattingEnabled = true;
            lstFiles.ItemHeight = 16;
            lstFiles.Location = new Point(0, 20);
            lstFiles.Name = "lstFiles";
            lstFiles.SelectionMode = SelectionMode.None;
            lstFiles.Size = new Size(258, 103);
            lstFiles.TabIndex = 1;
            // 
            // lblFileCount
            // 
            lblFileCount.Dock = DockStyle.Top;
            lblFileCount.Font = new Font("Microsoft YaHei UI", 8.25F, FontStyle.Bold);
            lblFileCount.ForeColor = Color.FromArgb(37, 99, 235);
            lblFileCount.Location = new Point(0, 0);
            lblFileCount.Name = "lblFileCount";
            lblFileCount.Size = new Size(258, 20);
            lblFileCount.TabIndex = 0;
            lblFileCount.Text = "文件数量: 0";
            // 
            // FolderPreviewCard
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(mainPanel);
            Font = new Font("Microsoft YaHei UI", 9F);
            Margin = new Padding(5);
            Name = "FolderPreviewCard";
            Size = new Size(280, 200);
            mainPanel.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            fileListPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Panel headerPanel;
        private Label lblFolderName;
        private CheckBox chkKeepOnDesktop;
        private Label lblDescription;
        private Panel fileListPanel;
        private ListBox lstFiles;
        private Label lblFileCount;
    }
}