using DesktopOrganizer.UI.Controls;

namespace DesktopOrganizer.UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            
            mainPanel = new Panel();
            headerPanel = new Panel();
            lblTitle = new Label();
            titleButtonPanel = new Panel();
            btnSettings = new Button();
            btnHelp = new Button();
            btnMinimize = new Button();
            contentPanel = new Panel();
            fileCountPanel = new Panel();
            lblFileCount = new Label();
            preferenceInputPanel = new PreferenceInputPanel();
            organizeButtonPanel = new Panel();
            btnStartOrganize = new Button();
            separatorPanel = new Panel();
            lblPreviewTitle = new Label();
            previewPanel = new FlowLayoutPanel();
            actionButtonPanel = new Panel();
            btnAdjustPlan = new Button();
            btnExecutePlan = new Button();
            btnUndo = new Button();
            statusPanel = new Panel();
            progressIndicator = new SimplifiedProgressIndicator();
            
            mainPanel.SuspendLayout();
            headerPanel.SuspendLayout();
            titleButtonPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            fileCountPanel.SuspendLayout();
            organizeButtonPanel.SuspendLayout();
            separatorPanel.SuspendLayout();
            actionButtonPanel.SuspendLayout();
            statusPanel.SuspendLayout();
            SuspendLayout();
            
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(249, 250, 251);
            mainPanel.Controls.Add(contentPanel);
            mainPanel.Controls.Add(headerPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(900, 700);
            mainPanel.TabIndex = 0;
            
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(titleButtonPanel);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(900, 50);
            headerPanel.TabIndex = 0;
            
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(187, 26);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Desktop Organizer";
            
            // 
            // titleButtonPanel
            // 
            titleButtonPanel.Controls.Add(btnMinimize);
            titleButtonPanel.Controls.Add(btnHelp);
            titleButtonPanel.Controls.Add(btnSettings);
            titleButtonPanel.Dock = DockStyle.Right;
            titleButtonPanel.Location = new Point(700, 0);
            titleButtonPanel.Name = "titleButtonPanel";
            titleButtonPanel.Size = new Size(200, 50);
            titleButtonPanel.TabIndex = 1;
            
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.None;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Microsoft YaHei UI", 9F);
            btnSettings.Location = new Point(10, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(60, 26);
            btnSettings.TabIndex = 0;
            btnSettings.Text = "设置";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;
            
            // 
            // btnHelp
            // 
            btnHelp.Anchor = AnchorStyles.None;
            btnHelp.FlatAppearance.BorderSize = 0;
            btnHelp.FlatStyle = FlatStyle.Flat;
            btnHelp.Font = new Font("Microsoft YaHei UI", 9F);
            btnHelp.Location = new Point(75, 12);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(60, 26);
            btnHelp.TabIndex = 1;
            btnHelp.Text = "帮助";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += btnHelp_Click;
            
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.None;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Microsoft YaHei UI", 9F);
            btnMinimize.Location = new Point(140, 12);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(50, 26);
            btnMinimize.TabIndex = 2;
            btnMinimize.Text = "最小化";
            btnMinimize.UseVisualStyleBackColor = true;
            btnMinimize.Click += btnMinimize_Click;
            
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(statusPanel);
            contentPanel.Controls.Add(actionButtonPanel);
            contentPanel.Controls.Add(previewPanel);
            contentPanel.Controls.Add(separatorPanel);
            contentPanel.Controls.Add(organizeButtonPanel);
            contentPanel.Controls.Add(preferenceInputPanel);
            contentPanel.Controls.Add(fileCountPanel);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 50);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(20);
            contentPanel.Size = new Size(900, 650);
            contentPanel.TabIndex = 1;
            
            // 
            // fileCountPanel
            // 
            fileCountPanel.Controls.Add(lblFileCount);
            fileCountPanel.Dock = DockStyle.Top;
            fileCountPanel.Location = new Point(20, 20);
            fileCountPanel.Name = "fileCountPanel";
            fileCountPanel.Size = new Size(860, 40);
            fileCountPanel.TabIndex = 0;
            
            // 
            // lblFileCount
            // 
            lblFileCount.AutoSize = true;
            lblFileCount.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            lblFileCount.ForeColor = Color.FromArgb(31, 41, 55);
            lblFileCount.Location = new Point(0, 10);
            lblFileCount.Name = "lblFileCount";
            lblFileCount.Size = new Size(203, 22);
            lblFileCount.TabIndex = 0;
            lblFileCount.Text = "📁 当前桌面文件：0 个文件";
            
            // 
            // preferenceInputPanel
            // 
            preferenceInputPanel.BackColor = Color.Transparent;
            preferenceInputPanel.Dock = DockStyle.Top;
            preferenceInputPanel.Font = new Font("Microsoft YaHei UI", 9F);
            preferenceInputPanel.Location = new Point(20, 60);
            preferenceInputPanel.Margin = new Padding(5);
            preferenceInputPanel.Name = "preferenceInputPanel";
            preferenceInputPanel.Size = new Size(860, 150);
            preferenceInputPanel.TabIndex = 1;
            preferenceInputPanel.PreferenceChanged += PreferenceInputPanel_PreferenceChanged;
            preferenceInputPanel.TemplateSelected += PreferenceInputPanel_TemplateSelected;
            
            // 
            // organizeButtonPanel
            // 
            organizeButtonPanel.Controls.Add(btnStartOrganize);
            organizeButtonPanel.Dock = DockStyle.Top;
            organizeButtonPanel.Location = new Point(20, 210);
            organizeButtonPanel.Name = "organizeButtonPanel";
            organizeButtonPanel.Padding = new Padding(0, 15, 0, 0);
            organizeButtonPanel.Size = new Size(860, 70);
            organizeButtonPanel.TabIndex = 2;
            
            // 
            // btnStartOrganize
            // 
            btnStartOrganize.Anchor = AnchorStyles.None;
            btnStartOrganize.BackColor = Color.FromArgb(37, 99, 235);
            btnStartOrganize.FlatAppearance.BorderSize = 0;
            btnStartOrganize.FlatStyle = FlatStyle.Flat;
            btnStartOrganize.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            btnStartOrganize.ForeColor = Color.White;
            btnStartOrganize.Location = new Point(350, 15);
            btnStartOrganize.Name = "btnStartOrganize";
            btnStartOrganize.Size = new Size(160, 45);
            btnStartOrganize.TabIndex = 0;
            btnStartOrganize.Text = "🚀 开始整理";
            btnStartOrganize.UseVisualStyleBackColor = false;
            btnStartOrganize.Click += btnStartOrganize_Click;
            
            // 
            // separatorPanel
            // 
            separatorPanel.Controls.Add(lblPreviewTitle);
            separatorPanel.Dock = DockStyle.Top;
            separatorPanel.Location = new Point(20, 280);
            separatorPanel.Name = "separatorPanel";
            separatorPanel.Size = new Size(860, 40);
            separatorPanel.TabIndex = 3;
            
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.AutoSize = true;
            lblPreviewTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            lblPreviewTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblPreviewTitle.Location = new Point(0, 10);
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.Size = new Size(141, 22);
            lblPreviewTitle.TabIndex = 0;
            lblPreviewTitle.Text = "📊 整理计划预览：";
            lblPreviewTitle.Visible = false;
            
            // 
            // previewPanel
            // 
            previewPanel.AutoScroll = true;
            previewPanel.BackColor = Color.Transparent;
            previewPanel.Dock = DockStyle.Fill;
            previewPanel.FlowDirection = FlowDirection.LeftToRight;
            previewPanel.Location = new Point(20, 320);
            previewPanel.Name = "previewPanel";
            previewPanel.Padding = new Padding(5);
            previewPanel.Size = new Size(860, 230);
            previewPanel.TabIndex = 4;
            previewPanel.WrapContents = true;
            
            // 
            // actionButtonPanel
            // 
            actionButtonPanel.Controls.Add(btnUndo);
            actionButtonPanel.Controls.Add(btnExecutePlan);
            actionButtonPanel.Controls.Add(btnAdjustPlan);
            actionButtonPanel.Dock = DockStyle.Bottom;
            actionButtonPanel.Location = new Point(20, 550);
            actionButtonPanel.Name = "actionButtonPanel";
            actionButtonPanel.Size = new Size(860, 50);
            actionButtonPanel.TabIndex = 5;
            actionButtonPanel.Visible = false;
            
            // 
            // btnAdjustPlan
            // 
            btnAdjustPlan.BackColor = Color.FromArgb(156, 163, 175);
            btnAdjustPlan.FlatAppearance.BorderSize = 0;
            btnAdjustPlan.FlatStyle = FlatStyle.Flat;
            btnAdjustPlan.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btnAdjustPlan.ForeColor = Color.White;
            btnAdjustPlan.Location = new Point(0, 10);
            btnAdjustPlan.Name = "btnAdjustPlan";
            btnAdjustPlan.Size = new Size(120, 35);
            btnAdjustPlan.TabIndex = 0;
            btnAdjustPlan.Text = "✏️ 调整计划";
            btnAdjustPlan.UseVisualStyleBackColor = false;
            btnAdjustPlan.Click += btnAdjustPlan_Click;
            
            // 
            // btnExecutePlan
            // 
            btnExecutePlan.BackColor = Color.FromArgb(5, 150, 105);
            btnExecutePlan.FlatAppearance.BorderSize = 0;
            btnExecutePlan.FlatStyle = FlatStyle.Flat;
            btnExecutePlan.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btnExecutePlan.ForeColor = Color.White;
            btnExecutePlan.Location = new Point(130, 10);
            btnExecutePlan.Name = "btnExecutePlan";
            btnExecutePlan.Size = new Size(120, 35);
            btnExecutePlan.TabIndex = 1;
            btnExecutePlan.Text = "✅ 执行整理";
            btnExecutePlan.UseVisualStyleBackColor = false;
            btnExecutePlan.Click += btnExecutePlan_Click;
            
            // 
            // btnUndo
            // 
            btnUndo.BackColor = Color.FromArgb(217, 119, 6);
            btnUndo.FlatAppearance.BorderSize = 0;
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btnUndo.ForeColor = Color.White;
            btnUndo.Location = new Point(260, 10);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(120, 35);
            btnUndo.TabIndex = 2;
            btnUndo.Text = "↶ 撤销操作";
            btnUndo.UseVisualStyleBackColor = false;
            btnUndo.Enabled = false;
            btnUndo.Click += btnUndo_Click;
            
            // 
            // statusPanel
            // 
            statusPanel.Controls.Add(progressIndicator);
            statusPanel.Dock = DockStyle.Bottom;
            statusPanel.Location = new Point(20, 600);
            statusPanel.Name = "statusPanel";
            statusPanel.Size = new Size(860, 30);
            statusPanel.TabIndex = 6;
            statusPanel.Visible = false;
            
            // 
            // progressIndicator
            // 
            progressIndicator.BackColor = Color.FromArgb(249, 250, 251);
            progressIndicator.Dock = DockStyle.Fill;
            progressIndicator.Font = new Font("Microsoft YaHei UI", 9F);
            progressIndicator.Location = new Point(0, 0);
            progressIndicator.Name = "progressIndicator";
            progressIndicator.Size = new Size(860, 30);
            progressIndicator.TabIndex = 0;
            progressIndicator.CancelRequested += ProgressIndicator_CancelRequested;
            
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(249, 250, 251);
            ClientSize = new Size(900, 700);
            Controls.Add(mainPanel);
            Font = new Font("Microsoft YaHei UI", 9F);
            // Icon = (Icon)resources.GetObject("$this.Icon"); // Removed to fix resource issue
            MinimumSize = new Size(800, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Desktop Organizer";
            Load += MainForm_Load;
            
            mainPanel.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            titleButtonPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            fileCountPanel.ResumeLayout(false);
            fileCountPanel.PerformLayout();
            organizeButtonPanel.ResumeLayout(false);
            separatorPanel.ResumeLayout(false);
            separatorPanel.PerformLayout();
            actionButtonPanel.ResumeLayout(false);
            statusPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Panel headerPanel;
        private Label lblTitle;
        private Panel titleButtonPanel;
        private Button btnSettings;
        private Button btnHelp;
        private Button btnMinimize;
        private Panel contentPanel;
        private Panel fileCountPanel;
        private Label lblFileCount;
        private PreferenceInputPanel preferenceInputPanel;
        private Panel organizeButtonPanel;
        private Button btnStartOrganize;
        private Panel separatorPanel;
        private Label lblPreviewTitle;
        private FlowLayoutPanel previewPanel;
        private Panel actionButtonPanel;
        private Button btnAdjustPlan;
        private Button btnExecutePlan;
        private Button btnUndo;
        private Panel statusPanel;
        private SimplifiedProgressIndicator progressIndicator;
    }
}