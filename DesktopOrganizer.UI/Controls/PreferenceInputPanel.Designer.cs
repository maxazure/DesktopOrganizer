namespace DesktopOrganizer.UI.Controls
{
    partial class PreferenceInputPanel
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
            groupBox = new GroupBox();
            actionPanel = new Panel();
            cmbTemplates = new ComboBox();
            btnClear = new Button();
            txtPreference = new TextBox();
            lblPrompt = new Label();
            groupBox.SuspendLayout();
            actionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.Controls.Add(actionPanel);
            groupBox.Controls.Add(txtPreference);
            groupBox.Controls.Add(lblPrompt);
            groupBox.Dock = DockStyle.Fill;
            groupBox.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            groupBox.ForeColor = Color.FromArgb(31, 41, 55);
            groupBox.Location = new Point(0, 0);
            groupBox.Name = "groupBox";
            groupBox.Padding = new Padding(15);
            groupBox.Size = new Size(500, 200);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "偏好设置";
            // 
            // actionPanel
            // 
            actionPanel.Controls.Add(cmbTemplates);
            actionPanel.Controls.Add(btnClear);
            actionPanel.Dock = DockStyle.Bottom;
            actionPanel.Location = new Point(15, 155);
            actionPanel.Name = "actionPanel";
            actionPanel.Size = new Size(470, 30);
            actionPanel.TabIndex = 2;
            // 
            // cmbTemplates
            // 
            cmbTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbTemplates.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTemplates.Font = new Font("Microsoft YaHei UI", 9F);
            cmbTemplates.FormattingEnabled = true;
            cmbTemplates.Location = new Point(270, 3);
            cmbTemplates.Name = "cmbTemplates";
            cmbTemplates.Size = new Size(200, 25);
            cmbTemplates.TabIndex = 1;
            cmbTemplates.SelectedIndexChanged += cmbTemplates_SelectedIndexChanged;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(156, 163, 175);
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Microsoft YaHei UI", 9F);
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(0, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(60, 25);
            btnClear.TabIndex = 0;
            btnClear.Text = "清空";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // txtPreference
            // 
            txtPreference.Dock = DockStyle.Fill;
            txtPreference.Font = new Font("Microsoft YaHei UI", 10F);
            txtPreference.ForeColor = Color.FromArgb(31, 41, 55);
            txtPreference.Location = new Point(15, 55);
            txtPreference.Multiline = true;
            txtPreference.Name = "txtPreference";
            txtPreference.PlaceholderText = "例如：请将文档类文件整理到'工作文档'文件夹，图片按时间分类，视频文件单独存放...";
            txtPreference.ScrollBars = ScrollBars.Vertical;
            txtPreference.Size = new Size(470, 100);
            txtPreference.TabIndex = 1;
            txtPreference.TextChanged += txtPreference_TextChanged;
            // 
            // lblPrompt
            // 
            lblPrompt.Dock = DockStyle.Top;
            lblPrompt.Font = new Font("Microsoft YaHei UI", 10F);
            lblPrompt.ForeColor = Color.FromArgb(75, 85, 99);
            lblPrompt.Location = new Point(15, 29);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(470, 26);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "💬 告诉我你的整理偏好：";
            // 
            // PreferenceInputPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(groupBox);
            Font = new Font("Microsoft YaHei UI", 9F);
            Name = "PreferenceInputPanel";
            Size = new Size(500, 200);
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            actionPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox;
        private Panel actionPanel;
        private ComboBox cmbTemplates;
        private Button btnClear;
        private TextBox txtPreference;
        private Label lblPrompt;
    }
}