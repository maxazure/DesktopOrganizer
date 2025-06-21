namespace DesktopOrganizer.UI
{
    partial class SettingsForm
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
            buttonPanel = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            btnApply = new Button();
            tabControl = new TabControl();
            tabAIModel = new TabPage();
            aiModelPanel = new Panel();
            grpAPISettings = new GroupBox();
            lblApiKey = new Label();
            txtApiKey = new TextBox();
            btnTestConnection = new Button();
            grpModelSelection = new GroupBox();
            lblModel = new Label();
            cmbModel = new ComboBox();
            lblProvider = new Label();
            cmbProvider = new ComboBox();
            tabInterface = new TabPage();
            interfacePanel = new Panel();
            grpInterfaceOptions = new GroupBox();
            chkConfirmBeforeExecution = new CheckBox();
            chkAutoCreateFolders = new CheckBox();
            grpLanguage = new GroupBox();
            lblLanguage = new Label();
            cmbLanguage = new ComboBox();
            tabTemplates = new TabPage();
            templatesPanel = new Panel();
            templatesActionPanel = new Panel();
            btnDeleteTemplate = new Button();
            btnEditTemplate = new Button();
            btnNewTemplate = new Button();
            templatesListPanel = new Panel();
            lstTemplates = new ListBox();
            lblTemplatesList = new Label();
            tabAdvanced = new TabPage();
            advancedPanel = new Panel();
            grpPerformance = new GroupBox();
            lblMaxFileSize = new Label();
            numMaxFileSize = new NumericUpDown();
            lblMaxFileSizeUnit = new Label();
            grpLogging = new GroupBox();
            lblLogLevel = new Label();
            cmbLogLevel = new ComboBox();
            btnOpenLogFile = new Button();
            grpTimeout = new GroupBox();
            lblTimeoutSeconds = new Label();
            numTimeoutSeconds = new NumericUpDown();
            buttonPanel.SuspendLayout();
            tabControl.SuspendLayout();
            tabAIModel.SuspendLayout();
            aiModelPanel.SuspendLayout();
            grpAPISettings.SuspendLayout();
            grpModelSelection.SuspendLayout();
            tabInterface.SuspendLayout();
            interfacePanel.SuspendLayout();
            grpInterfaceOptions.SuspendLayout();
            grpLanguage.SuspendLayout();
            tabTemplates.SuspendLayout();
            templatesPanel.SuspendLayout();
            templatesActionPanel.SuspendLayout();
            templatesListPanel.SuspendLayout();
            tabAdvanced.SuspendLayout();
            advancedPanel.SuspendLayout();
            grpPerformance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxFileSize).BeginInit();
            grpLogging.SuspendLayout();
            grpTimeout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeoutSeconds).BeginInit();
            SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnApply);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(0, 420);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(15);
            buttonPanel.Size = new Size(600, 60);
            buttonPanel.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Font = new Font("Microsoft YaHei UI", 9F);
            btnCancel.Location = new Point(525, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(60, 30);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.DialogResult = DialogResult.OK;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(395, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(60, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnApply
            // 
            btnApply.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnApply.Font = new Font("Microsoft YaHei UI", 9F);
            btnApply.Location = new Point(460, 15);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(60, 30);
            btnApply.TabIndex = 1;
            btnApply.Text = "应用";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabAIModel);
            tabControl.Controls.Add(tabInterface);
            tabControl.Controls.Add(tabTemplates);
            tabControl.Controls.Add(tabAdvanced);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Microsoft YaHei UI", 9F);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(600, 420);
            tabControl.TabIndex = 1;
            // 
            // tabAIModel
            // 
            tabAIModel.Controls.Add(aiModelPanel);
            tabAIModel.Location = new Point(4, 26);
            tabAIModel.Name = "tabAIModel";
            tabAIModel.Padding = new Padding(3);
            tabAIModel.Size = new Size(592, 390);
            tabAIModel.TabIndex = 0;
            tabAIModel.Text = "AI模型";
            tabAIModel.UseVisualStyleBackColor = true;
            // 
            // aiModelPanel
            // 
            aiModelPanel.Controls.Add(grpAPISettings);
            aiModelPanel.Controls.Add(grpModelSelection);
            aiModelPanel.Dock = DockStyle.Fill;
            aiModelPanel.Location = new Point(3, 3);
            aiModelPanel.Name = "aiModelPanel";
            aiModelPanel.Padding = new Padding(15);
            aiModelPanel.Size = new Size(586, 384);
            aiModelPanel.TabIndex = 0;
            // 
            // grpAPISettings
            // 
            grpAPISettings.Controls.Add(btnTestConnection);
            grpAPISettings.Controls.Add(txtApiKey);
            grpAPISettings.Controls.Add(lblApiKey);
            grpAPISettings.Dock = DockStyle.Fill;
            grpAPISettings.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpAPISettings.Location = new Point(15, 135);
            grpAPISettings.Name = "grpAPISettings";
            grpAPISettings.Padding = new Padding(15);
            grpAPISettings.Size = new Size(556, 234);
            grpAPISettings.TabIndex = 1;
            grpAPISettings.TabStop = false;
            grpAPISettings.Text = "API设置";
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Font = new Font("Microsoft YaHei UI", 9F);
            lblApiKey.Location = new Point(15, 35);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(56, 17);
            lblApiKey.TabIndex = 0;
            lblApiKey.Text = "API密钥:";
            // 
            // txtApiKey
            // 
            txtApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtApiKey.Font = new Font("Microsoft YaHei UI", 9F);
            txtApiKey.Location = new Point(15, 55);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.PasswordChar = '*';
            txtApiKey.Size = new Size(526, 23);
            txtApiKey.TabIndex = 1;
            // 
            // btnTestConnection
            // 
            btnTestConnection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTestConnection.Font = new Font("Microsoft YaHei UI", 9F);
            btnTestConnection.Location = new Point(456, 90);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(85, 25);
            btnTestConnection.TabIndex = 2;
            btnTestConnection.Text = "测试连接";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += btnTestConnection_Click;
            // 
            // grpModelSelection
            // 
            grpModelSelection.Controls.Add(cmbProvider);
            grpModelSelection.Controls.Add(lblProvider);
            grpModelSelection.Controls.Add(cmbModel);
            grpModelSelection.Controls.Add(lblModel);
            grpModelSelection.Dock = DockStyle.Top;
            grpModelSelection.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpModelSelection.Location = new Point(15, 15);
            grpModelSelection.Name = "grpModelSelection";
            grpModelSelection.Padding = new Padding(15);
            grpModelSelection.Size = new Size(556, 120);
            grpModelSelection.TabIndex = 0;
            grpModelSelection.TabStop = false;
            grpModelSelection.Text = "模型选择";
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Font = new Font("Microsoft YaHei UI", 9F);
            lblModel.Location = new Point(15, 70);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(56, 17);
            lblModel.TabIndex = 2;
            lblModel.Text = "模型名称:";
            // 
            // cmbModel
            // 
            cmbModel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModel.Font = new Font("Microsoft YaHei UI", 9F);
            cmbModel.FormattingEnabled = true;
            cmbModel.Location = new Point(90, 67);
            cmbModel.Name = "cmbModel";
            cmbModel.Size = new Size(451, 25);
            cmbModel.TabIndex = 3;
            // 
            // lblProvider
            // 
            lblProvider.AutoSize = true;
            lblProvider.Font = new Font("Microsoft YaHei UI", 9F);
            lblProvider.Location = new Point(15, 35);
            lblProvider.Name = "lblProvider";
            lblProvider.Size = new Size(68, 17);
            lblProvider.TabIndex = 0;
            lblProvider.Text = "服务提供商:";
            // 
            // cmbProvider
            // 
            cmbProvider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProvider.Font = new Font("Microsoft YaHei UI", 9F);
            cmbProvider.FormattingEnabled = true;
            cmbProvider.Location = new Point(90, 32);
            cmbProvider.Name = "cmbProvider";
            cmbProvider.Size = new Size(451, 25);
            cmbProvider.TabIndex = 1;
            cmbProvider.SelectedIndexChanged += cmbProvider_SelectedIndexChanged;
            // 
            // tabInterface
            // 
            tabInterface.Controls.Add(interfacePanel);
            tabInterface.Location = new Point(4, 26);
            tabInterface.Name = "tabInterface";
            tabInterface.Padding = new Padding(3);
            tabInterface.Size = new Size(592, 390);
            tabInterface.TabIndex = 1;
            tabInterface.Text = "界面设置";
            tabInterface.UseVisualStyleBackColor = true;
            // 
            // interfacePanel
            // 
            interfacePanel.Controls.Add(grpInterfaceOptions);
            interfacePanel.Controls.Add(grpLanguage);
            interfacePanel.Dock = DockStyle.Fill;
            interfacePanel.Location = new Point(3, 3);
            interfacePanel.Name = "interfacePanel";
            interfacePanel.Padding = new Padding(15);
            interfacePanel.Size = new Size(586, 384);
            interfacePanel.TabIndex = 0;
            // 
            // grpInterfaceOptions
            // 
            grpInterfaceOptions.Controls.Add(chkAutoCreateFolders);
            grpInterfaceOptions.Controls.Add(chkConfirmBeforeExecution);
            grpInterfaceOptions.Dock = DockStyle.Fill;
            grpInterfaceOptions.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpInterfaceOptions.Location = new Point(15, 95);
            grpInterfaceOptions.Name = "grpInterfaceOptions";
            grpInterfaceOptions.Padding = new Padding(15);
            grpInterfaceOptions.Size = new Size(556, 274);
            grpInterfaceOptions.TabIndex = 1;
            grpInterfaceOptions.TabStop = false;
            grpInterfaceOptions.Text = "界面选项";
            // 
            // chkConfirmBeforeExecution
            // 
            chkConfirmBeforeExecution.AutoSize = true;
            chkConfirmBeforeExecution.Font = new Font("Microsoft YaHei UI", 9F);
            chkConfirmBeforeExecution.Location = new Point(15, 35);
            chkConfirmBeforeExecution.Name = "chkConfirmBeforeExecution";
            chkConfirmBeforeExecution.Size = new Size(99, 21);
            chkConfirmBeforeExecution.TabIndex = 0;
            chkConfirmBeforeExecution.Text = "执行前确认";
            chkConfirmBeforeExecution.UseVisualStyleBackColor = true;
            // 
            // chkAutoCreateFolders
            // 
            chkAutoCreateFolders.AutoSize = true;
            chkAutoCreateFolders.Font = new Font("Microsoft YaHei UI", 9F);
            chkAutoCreateFolders.Location = new Point(15, 70);
            chkAutoCreateFolders.Name = "chkAutoCreateFolders";
            chkAutoCreateFolders.Size = new Size(123, 21);
            chkAutoCreateFolders.TabIndex = 1;
            chkAutoCreateFolders.Text = "自动创建文件夹";
            chkAutoCreateFolders.UseVisualStyleBackColor = true;
            // 
            // grpLanguage
            // 
            grpLanguage.Controls.Add(cmbLanguage);
            grpLanguage.Controls.Add(lblLanguage);
            grpLanguage.Dock = DockStyle.Top;
            grpLanguage.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpLanguage.Location = new Point(15, 15);
            grpLanguage.Name = "grpLanguage";
            grpLanguage.Padding = new Padding(15);
            grpLanguage.Size = new Size(556, 80);
            grpLanguage.TabIndex = 0;
            grpLanguage.TabStop = false;
            grpLanguage.Text = "语言设置";
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Font = new Font("Microsoft YaHei UI", 9F);
            lblLanguage.Location = new Point(15, 35);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(44, 17);
            lblLanguage.TabIndex = 0;
            lblLanguage.Text = "语言:";
            // 
            // cmbLanguage
            // 
            cmbLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.Font = new Font("Microsoft YaHei UI", 9F);
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(90, 32);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(451, 25);
            cmbLanguage.TabIndex = 1;
            // 
            // tabTemplates
            // 
            tabTemplates.Controls.Add(templatesPanel);
            tabTemplates.Location = new Point(4, 26);
            tabTemplates.Name = "tabTemplates";
            tabTemplates.Size = new Size(592, 390);
            tabTemplates.TabIndex = 2;
            tabTemplates.Text = "模板管理";
            tabTemplates.UseVisualStyleBackColor = true;
            // 
            // templatesPanel
            // 
            templatesPanel.Controls.Add(templatesListPanel);
            templatesPanel.Controls.Add(templatesActionPanel);
            templatesPanel.Dock = DockStyle.Fill;
            templatesPanel.Location = new Point(0, 0);
            templatesPanel.Name = "templatesPanel";
            templatesPanel.Padding = new Padding(15);
            templatesPanel.Size = new Size(592, 390);
            templatesPanel.TabIndex = 0;
            // 
            // templatesActionPanel
            // 
            templatesActionPanel.Controls.Add(btnNewTemplate);
            templatesActionPanel.Controls.Add(btnEditTemplate);
            templatesActionPanel.Controls.Add(btnDeleteTemplate);
            templatesActionPanel.Dock = DockStyle.Bottom;
            templatesActionPanel.Location = new Point(15, 335);
            templatesActionPanel.Name = "templatesActionPanel";
            templatesActionPanel.Size = new Size(562, 40);
            templatesActionPanel.TabIndex = 1;
            // 
            // btnDeleteTemplate
            // 
            btnDeleteTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDeleteTemplate.BackColor = Color.FromArgb(239, 68, 68);
            btnDeleteTemplate.FlatAppearance.BorderSize = 0;
            btnDeleteTemplate.FlatStyle = FlatStyle.Flat;
            btnDeleteTemplate.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnDeleteTemplate.ForeColor = Color.White;
            btnDeleteTemplate.Location = new Point(482, 5);
            btnDeleteTemplate.Name = "btnDeleteTemplate";
            btnDeleteTemplate.Size = new Size(75, 30);
            btnDeleteTemplate.TabIndex = 2;
            btnDeleteTemplate.Text = "删除";
            btnDeleteTemplate.UseVisualStyleBackColor = false;
            btnDeleteTemplate.Click += btnDeleteTemplate_Click;
            // 
            // btnEditTemplate
            // 
            btnEditTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditTemplate.Font = new Font("Microsoft YaHei UI", 9F);
            btnEditTemplate.Location = new Point(402, 5);
            btnEditTemplate.Name = "btnEditTemplate";
            btnEditTemplate.Size = new Size(75, 30);
            btnEditTemplate.TabIndex = 1;
            btnEditTemplate.Text = "编辑";
            btnEditTemplate.UseVisualStyleBackColor = true;
            btnEditTemplate.Click += btnEditTemplate_Click;
            // 
            // btnNewTemplate
            // 
            btnNewTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNewTemplate.BackColor = Color.FromArgb(5, 150, 105);
            btnNewTemplate.FlatAppearance.BorderSize = 0;
            btnNewTemplate.FlatStyle = FlatStyle.Flat;
            btnNewTemplate.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnNewTemplate.ForeColor = Color.White;
            btnNewTemplate.Location = new Point(322, 5);
            btnNewTemplate.Name = "btnNewTemplate";
            btnNewTemplate.Size = new Size(75, 30);
            btnNewTemplate.TabIndex = 0;
            btnNewTemplate.Text = "新建";
            btnNewTemplate.UseVisualStyleBackColor = false;
            btnNewTemplate.Click += btnNewTemplate_Click;
            // 
            // templatesListPanel
            // 
            templatesListPanel.Controls.Add(lstTemplates);
            templatesListPanel.Controls.Add(lblTemplatesList);
            templatesListPanel.Dock = DockStyle.Fill;
            templatesListPanel.Location = new Point(15, 15);
            templatesListPanel.Name = "templatesListPanel";
            templatesListPanel.Size = new Size(562, 320);
            templatesListPanel.TabIndex = 0;
            // 
            // lstTemplates
            // 
            lstTemplates.Dock = DockStyle.Fill;
            lstTemplates.Font = new Font("Microsoft YaHei UI", 9F);
            lstTemplates.FormattingEnabled = true;
            lstTemplates.ItemHeight = 17;
            lstTemplates.Location = new Point(0, 25);
            lstTemplates.Name = "lstTemplates";
            lstTemplates.Size = new Size(562, 295);
            lstTemplates.TabIndex = 1;
            lstTemplates.SelectedIndexChanged += lstTemplates_SelectedIndexChanged;
            // 
            // lblTemplatesList
            // 
            lblTemplatesList.Dock = DockStyle.Top;
            lblTemplatesList.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            lblTemplatesList.Location = new Point(0, 0);
            lblTemplatesList.Name = "lblTemplatesList";
            lblTemplatesList.Size = new Size(562, 25);
            lblTemplatesList.TabIndex = 0;
            lblTemplatesList.Text = "偏好模板列表：";
            // 
            // tabAdvanced
            // 
            tabAdvanced.Controls.Add(advancedPanel);
            tabAdvanced.Location = new Point(4, 26);
            tabAdvanced.Name = "tabAdvanced";
            tabAdvanced.Size = new Size(592, 390);
            tabAdvanced.TabIndex = 3;
            tabAdvanced.Text = "高级选项";
            tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // advancedPanel
            // 
            advancedPanel.Controls.Add(grpTimeout);
            advancedPanel.Controls.Add(grpLogging);
            advancedPanel.Controls.Add(grpPerformance);
            advancedPanel.Dock = DockStyle.Fill;
            advancedPanel.Location = new Point(0, 0);
            advancedPanel.Name = "advancedPanel";
            advancedPanel.Padding = new Padding(15);
            advancedPanel.Size = new Size(592, 390);
            advancedPanel.TabIndex = 0;
            // 
            // grpPerformance
            // 
            grpPerformance.Controls.Add(lblMaxFileSizeUnit);
            grpPerformance.Controls.Add(numMaxFileSize);
            grpPerformance.Controls.Add(lblMaxFileSize);
            grpPerformance.Dock = DockStyle.Top;
            grpPerformance.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpPerformance.Location = new Point(15, 15);
            grpPerformance.Name = "grpPerformance";
            grpPerformance.Padding = new Padding(15);
            grpPerformance.Size = new Size(562, 80);
            grpPerformance.TabIndex = 0;
            grpPerformance.TabStop = false;
            grpPerformance.Text = "性能设置";
            // 
            // lblMaxFileSize
            // 
            lblMaxFileSize.AutoSize = true;
            lblMaxFileSize.Font = new Font("Microsoft YaHei UI", 9F);
            lblMaxFileSize.Location = new Point(15, 35);
            lblMaxFileSize.Name = "lblMaxFileSize";
            lblMaxFileSize.Size = new Size(92, 17);
            lblMaxFileSize.TabIndex = 0;
            lblMaxFileSize.Text = "最大文件大小:";
            // 
            // numMaxFileSize
            // 
            numMaxFileSize.Font = new Font("Microsoft YaHei UI", 9F);
            numMaxFileSize.Location = new Point(115, 32);
            numMaxFileSize.Maximum = new decimal(new int[] { 10240, 0, 0, 0 });
            numMaxFileSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxFileSize.Name = "numMaxFileSize";
            numMaxFileSize.Size = new Size(80, 23);
            numMaxFileSize.TabIndex = 1;
            numMaxFileSize.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            // 
            // lblMaxFileSizeUnit
            // 
            lblMaxFileSizeUnit.AutoSize = true;
            lblMaxFileSizeUnit.Font = new Font("Microsoft YaHei UI", 9F);
            lblMaxFileSizeUnit.Location = new Point(205, 35);
            lblMaxFileSizeUnit.Name = "lblMaxFileSizeUnit";
            lblMaxFileSizeUnit.Size = new Size(25, 17);
            lblMaxFileSizeUnit.TabIndex = 2;
            lblMaxFileSizeUnit.Text = "MB";
            // 
            // grpLogging
            // 
            grpLogging.Controls.Add(btnOpenLogFile);
            grpLogging.Controls.Add(cmbLogLevel);
            grpLogging.Controls.Add(lblLogLevel);
            grpLogging.Dock = DockStyle.Top;
            grpLogging.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpLogging.Location = new Point(15, 95);
            grpLogging.Name = "grpLogging";
            grpLogging.Padding = new Padding(15);
            grpLogging.Size = new Size(562, 100);
            grpLogging.TabIndex = 1;
            grpLogging.TabStop = false;
            grpLogging.Text = "日志设置";
            // 
            // lblLogLevel
            // 
            lblLogLevel.AutoSize = true;
            lblLogLevel.Font = new Font("Microsoft YaHei UI", 9F);
            lblLogLevel.Location = new Point(15, 35);
            lblLogLevel.Name = "lblLogLevel";
            lblLogLevel.Size = new Size(68, 17);
            lblLogLevel.TabIndex = 0;
            lblLogLevel.Text = "日志级别:";
            // 
            // cmbLogLevel
            // 
            cmbLogLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLogLevel.Font = new Font("Microsoft YaHei UI", 9F);
            cmbLogLevel.FormattingEnabled = true;
            cmbLogLevel.Location = new Point(90, 32);
            cmbLogLevel.Name = "cmbLogLevel";
            cmbLogLevel.Size = new Size(150, 25);
            cmbLogLevel.TabIndex = 1;
            // 
            // btnOpenLogFile
            // 
            btnOpenLogFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenLogFile.Font = new Font("Microsoft YaHei UI", 9F);
            btnOpenLogFile.Location = new Point(462, 30);
            btnOpenLogFile.Name = "btnOpenLogFile";
            btnOpenLogFile.Size = new Size(85, 25);
            btnOpenLogFile.TabIndex = 2;
            btnOpenLogFile.Text = "打开日志文件";
            btnOpenLogFile.UseVisualStyleBackColor = true;
            btnOpenLogFile.Click += btnOpenLogFile_Click;
            // 
            // grpTimeout
            // 
            grpTimeout.Controls.Add(numTimeoutSeconds);
            grpTimeout.Controls.Add(lblTimeoutSeconds);
            grpTimeout.Dock = DockStyle.Top;
            grpTimeout.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grpTimeout.Location = new Point(15, 195);
            grpTimeout.Name = "grpTimeout";
            grpTimeout.Padding = new Padding(15);
            grpTimeout.Size = new Size(562, 80);
            grpTimeout.TabIndex = 2;
            grpTimeout.TabStop = false;
            grpTimeout.Text = "超时设置";
            // 
            // lblTimeoutSeconds
            // 
            lblTimeoutSeconds.AutoSize = true;
            lblTimeoutSeconds.Font = new Font("Microsoft YaHei UI", 9F);
            lblTimeoutSeconds.Location = new Point(15, 35);
            lblTimeoutSeconds.Name = "lblTimeoutSeconds";
            lblTimeoutSeconds.Size = new Size(92, 17);
            lblTimeoutSeconds.TabIndex = 0;
            lblTimeoutSeconds.Text = "请求超时时间:";
            // 
            // numTimeoutSeconds
            // 
            numTimeoutSeconds.Font = new Font("Microsoft YaHei UI", 9F);
            numTimeoutSeconds.Location = new Point(115, 32);
            numTimeoutSeconds.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            numTimeoutSeconds.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numTimeoutSeconds.Name = "numTimeoutSeconds";
            numTimeoutSeconds.Size = new Size(80, 23);
            numTimeoutSeconds.TabIndex = 1;
            numTimeoutSeconds.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // SettingsForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(600, 480);
            Controls.Add(tabControl);
            Controls.Add(buttonPanel);
            Font = new Font("Microsoft YaHei UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "设置";
            Load += SettingsForm_Load;
            buttonPanel.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            tabAIModel.ResumeLayout(false);
            aiModelPanel.ResumeLayout(false);
            grpAPISettings.ResumeLayout(false);
            grpAPISettings.PerformLayout();
            grpModelSelection.ResumeLayout(false);
            grpModelSelection.PerformLayout();
            tabInterface.ResumeLayout(false);
            interfacePanel.ResumeLayout(false);
            grpInterfaceOptions.ResumeLayout(false);
            grpInterfaceOptions.PerformLayout();
            grpLanguage.ResumeLayout(false);
            grpLanguage.PerformLayout();
            tabTemplates.ResumeLayout(false);
            templatesPanel.ResumeLayout(false);
            templatesActionPanel.ResumeLayout(false);
            templatesListPanel.ResumeLayout(false);
            tabAdvanced.ResumeLayout(false);
            advancedPanel.ResumeLayout(false);
            grpPerformance.ResumeLayout(false);
            grpPerformance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxFileSize).EndInit();
            grpLogging.ResumeLayout(false);
            grpLogging.PerformLayout();
            grpTimeout.ResumeLayout(false);
            grpTimeout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeoutSeconds).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel buttonPanel;
        private Button btnCancel;
        private Button btnSave;
        private Button btnApply;
        private TabControl tabControl;
        private TabPage tabAIModel;
        private Panel aiModelPanel;
        private GroupBox grpAPISettings;
        private Button btnTestConnection;
        private TextBox txtApiKey;
        private Label lblApiKey;
        private GroupBox grpModelSelection;
        private ComboBox cmbProvider;
        private Label lblProvider;
        private ComboBox cmbModel;
        private Label lblModel;
        private TabPage tabInterface;
        private Panel interfacePanel;
        private GroupBox grpInterfaceOptions;
        private CheckBox chkAutoCreateFolders;
        private CheckBox chkConfirmBeforeExecution;
        private GroupBox grpLanguage;
        private ComboBox cmbLanguage;
        private Label lblLanguage;
        private TabPage tabTemplates;
        private Panel templatesPanel;
        private Panel templatesListPanel;
        private ListBox lstTemplates;
        private Label lblTemplatesList;
        private Panel templatesActionPanel;
        private Button btnNewTemplate;
        private Button btnEditTemplate;
        private Button btnDeleteTemplate;
        private TabPage tabAdvanced;
        private Panel advancedPanel;
        private GroupBox grpTimeout;
        private NumericUpDown numTimeoutSeconds;
        private Label lblTimeoutSeconds;
        private GroupBox grpLogging;
        private Button btnOpenLogFile;
        private ComboBox cmbLogLevel;
        private Label lblLogLevel;
        private GroupBox grpPerformance;
        private Label lblMaxFileSizeUnit;
        private NumericUpDown numMaxFileSize;
        private Label lblMaxFileSize;
    }
}