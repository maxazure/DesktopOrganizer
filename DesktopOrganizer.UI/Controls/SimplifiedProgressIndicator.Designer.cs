namespace DesktopOrganizer.UI.Controls
{
    partial class SimplifiedProgressIndicator
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
            buttonPanel = new Panel();
            btnCancel = new Button();
            statusPanel = new Panel();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            mainPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            statusPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(statusPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(400, 60);
            mainPanel.TabIndex = 0;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Dock = DockStyle.Right;
            buttonPanel.Location = new Point(320, 0);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(80, 60);
            buttonPanel.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.BackColor = Color.FromArgb(239, 68, 68);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(10, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(60, 30);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // statusPanel
            // 
            statusPanel.Controls.Add(progressBar);
            statusPanel.Controls.Add(lblStatus);
            statusPanel.Dock = DockStyle.Fill;
            statusPanel.Location = new Point(0, 0);
            statusPanel.Name = "statusPanel";
            statusPanel.Padding = new Padding(10);
            statusPanel.Size = new Size(320, 60);
            statusPanel.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Top;
            lblStatus.Font = new Font("Microsoft YaHei UI", 10F);
            lblStatus.ForeColor = Color.FromArgb(31, 41, 55);
            lblStatus.Location = new Point(10, 10);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(300, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "准备中...";
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Fill;
            progressBar.Location = new Point(10, 30);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(300, 20);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;
            // 
            // SimplifiedProgressIndicator
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(249, 250, 251);
            Controls.Add(mainPanel);
            Font = new Font("Microsoft YaHei UI", 9F);
            Name = "SimplifiedProgressIndicator";
            Size = new Size(400, 60);
            mainPanel.ResumeLayout(false);
            buttonPanel.ResumeLayout(false);
            statusPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Panel buttonPanel;
        private Button btnCancel;
        private Panel statusPanel;
        private Label lblStatus;
        private ProgressBar progressBar;
    }
}