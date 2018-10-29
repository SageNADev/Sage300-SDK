namespace Sage300InquiryConfigurationWizardUI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.txtPassword = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtUser = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtCompany = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtVersion = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpLanguageSupport = new System.Windows.Forms.GroupBox();
            this.txtLanguageSupportPasswordChn = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportPasswordCht = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportPasswordEsn = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportPasswordFra = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportUserChn = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportUserCht = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportUserEsn = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtLanguageSupportUserFra = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.lblLanguageSupportPassword = new System.Windows.Forms.Label();
            this.lblLanguageSupportUser = new System.Windows.Forms.Label();
            this.chkLanguageChn = new System.Windows.Forms.CheckBox();
            this.chkLanguageCht = new System.Windows.Forms.CheckBox();
            this.chkLanguageEsn = new System.Windows.Forms.CheckBox();
            this.chkLanguageFra = new System.Windows.Forms.CheckBox();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.txtSQLScriptName = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.lblSQLScriptName = new System.Windows.Forms.Label();
            this.grpConfigurationFiles = new System.Windows.Forms.GroupBox();
            this.txtDatasourceConfigurationFile = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.lblDatasourceConfigurationFile = new System.Windows.Forms.Label();
            this.lblTemplateConfigurationFile = new System.Windows.Forms.Label();
            this.btnTemplateConfigurationFileFinder = new System.Windows.Forms.Button();
            this.txtTemplateConfigurationFile = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.btnDatasourceConfigurationFileFinder = new System.Windows.Forms.Button();
            this.btnOutputPathFinder = new System.Windows.Forms.Button();
            this.btnRootPathFinder = new System.Windows.Forms.Button();
            this.txtOutputPath = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.txtRootPath = new Sage300InquiryConfigurationWizardUI.BorderedTextBox();
            this.lblRootPath = new System.Windows.Forms.Label();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.btnOptionInquiry = new System.Windows.Forms.Button();
            this.btnOptionCrm = new System.Windows.Forms.Button();
            this.lblOption = new System.Windows.Forms.Label();
            this.btnOptionAdhoc = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtLogWindow = new System.Windows.Forms.TextBox();
            this.lblConsole = new System.Windows.Forms.Label();
            this.chkDisplayOutputFolderOnCompletion = new System.Windows.Forms.CheckBox();
            this.chkDisplayLogFileOnCompletion = new System.Windows.Forms.CheckBox();
            this.grpCredentials.SuspendLayout();
            this.grpLanguageSupport.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpConfigurationFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add(this.txtPassword);
            this.grpCredentials.Controls.Add(this.txtUser);
            this.grpCredentials.Controls.Add(this.txtCompany);
            this.grpCredentials.Controls.Add(this.txtVersion);
            this.grpCredentials.Controls.Add(this.lblVersion);
            this.grpCredentials.Controls.Add(this.lblCompany);
            this.grpCredentials.Controls.Add(this.lblPassword);
            this.grpCredentials.Controls.Add(this.lblUser);
            this.grpCredentials.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCredentials.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grpCredentials.Location = new System.Drawing.Point(13, 12);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(268, 156);
            this.grpCredentials.TabIndex = 12;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Application Credentials";
            this.grpCredentials.Enter += new System.EventHandler(this.group_Enter);
            this.grpCredentials.Leave += new System.EventHandler(this.group_Leave);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPassword.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtPassword.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtPassword.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtPassword, 10);
            this.txtPassword.Location = new System.Drawing.Point(80, 61);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(1);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(151, 22);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
            // 
            // txtUser
            // 
            this.txtUser.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUser.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtUser.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtUser.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtUser, 10);
            this.txtUser.Location = new System.Drawing.Point(80, 33);
            this.txtUser.Name = "txtUser";
            this.txtUser.Padding = new System.Windows.Forms.Padding(1);
            this.txtUser.PasswordChar = '\0';
            this.txtUser.Size = new System.Drawing.Size(151, 22);
            this.txtUser.TabIndex = 1;
            this.txtUser.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.txtUser_Validating);
            // 
            // txtCompany
            // 
            this.txtCompany.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtCompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompany.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCompany.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtCompany.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtCompany.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtCompany, 10);
            this.txtCompany.Location = new System.Drawing.Point(80, 89);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Padding = new System.Windows.Forms.Padding(1);
            this.txtCompany.PasswordChar = '\0';
            this.txtCompany.Size = new System.Drawing.Size(151, 22);
            this.txtCompany.TabIndex = 3;
            this.txtCompany.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtCompany.Validating += new System.ComponentModel.CancelEventHandler(this.txtCompany_Validating);
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtVersion.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtVersion.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtVersion.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtVersion, 10);
            this.txtVersion.Location = new System.Drawing.Point(80, 118);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Padding = new System.Windows.Forms.Padding(1);
            this.txtVersion.PasswordChar = '\0';
            this.txtVersion.Size = new System.Drawing.Size(40, 22);
            this.txtVersion.TabIndex = 4;
            this.txtVersion.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtVersion.Validating += new System.ComponentModel.CancelEventHandler(this.txtVersion_Validating);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(26, 121);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 13);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version:";
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCompany.Location = new System.Drawing.Point(16, 92);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(58, 13);
            this.lblCompany.TabIndex = 5;
            this.lblCompany.Text = "Company:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPassword.Location = new System.Drawing.Point(15, 62);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(59, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUser.Location = new System.Drawing.Point(41, 36);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(33, 13);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "User:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.Location = new System.Drawing.Point(631, 544);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(95, 44);
            this.btnGenerate.TabIndex = 31;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(13, 544);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 44);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpLanguageSupport
            // 
            this.grpLanguageSupport.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportPasswordChn);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportPasswordCht);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportPasswordEsn);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportPasswordFra);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportUserChn);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportUserCht);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportUserEsn);
            this.grpLanguageSupport.Controls.Add(this.txtLanguageSupportUserFra);
            this.grpLanguageSupport.Controls.Add(this.lblLanguageSupportPassword);
            this.grpLanguageSupport.Controls.Add(this.lblLanguageSupportUser);
            this.grpLanguageSupport.Controls.Add(this.chkLanguageChn);
            this.grpLanguageSupport.Controls.Add(this.chkLanguageCht);
            this.grpLanguageSupport.Controls.Add(this.chkLanguageEsn);
            this.grpLanguageSupport.Controls.Add(this.chkLanguageFra);
            this.grpLanguageSupport.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grpLanguageSupport.Location = new System.Drawing.Point(287, 12);
            this.grpLanguageSupport.Name = "grpLanguageSupport";
            this.grpLanguageSupport.Size = new System.Drawing.Size(439, 156);
            this.grpLanguageSupport.TabIndex = 15;
            this.grpLanguageSupport.TabStop = false;
            this.grpLanguageSupport.Text = "Language Support";
            this.grpLanguageSupport.Enter += new System.EventHandler(this.group_Enter);
            this.grpLanguageSupport.Leave += new System.EventHandler(this.group_Leave);
            // 
            // txtLanguageSupportPasswordChn
            // 
            this.txtLanguageSupportPasswordChn.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordChn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportPasswordChn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportPasswordChn.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordChn.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportPasswordChn.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportPasswordChn.Location = new System.Drawing.Point(275, 108);
            this.txtLanguageSupportPasswordChn.Name = "txtLanguageSupportPasswordChn";
            this.txtLanguageSupportPasswordChn.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportPasswordChn.PasswordChar = '*';
            this.txtLanguageSupportPasswordChn.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportPasswordChn.TabIndex = 16;
            // 
            // txtLanguageSupportPasswordCht
            // 
            this.txtLanguageSupportPasswordCht.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordCht.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportPasswordCht.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportPasswordCht.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordCht.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportPasswordCht.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportPasswordCht.Location = new System.Drawing.Point(275, 85);
            this.txtLanguageSupportPasswordCht.Name = "txtLanguageSupportPasswordCht";
            this.txtLanguageSupportPasswordCht.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportPasswordCht.PasswordChar = '*';
            this.txtLanguageSupportPasswordCht.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportPasswordCht.TabIndex = 13;
            // 
            // txtLanguageSupportPasswordEsn
            // 
            this.txtLanguageSupportPasswordEsn.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordEsn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportPasswordEsn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportPasswordEsn.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordEsn.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportPasswordEsn.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportPasswordEsn.Location = new System.Drawing.Point(275, 62);
            this.txtLanguageSupportPasswordEsn.Name = "txtLanguageSupportPasswordEsn";
            this.txtLanguageSupportPasswordEsn.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportPasswordEsn.PasswordChar = '*';
            this.txtLanguageSupportPasswordEsn.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportPasswordEsn.TabIndex = 10;
            // 
            // txtLanguageSupportPasswordFra
            // 
            this.txtLanguageSupportPasswordFra.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordFra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportPasswordFra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportPasswordFra.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportPasswordFra.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportPasswordFra.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportPasswordFra.Location = new System.Drawing.Point(275, 39);
            this.txtLanguageSupportPasswordFra.Name = "txtLanguageSupportPasswordFra";
            this.txtLanguageSupportPasswordFra.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportPasswordFra.PasswordChar = '*';
            this.txtLanguageSupportPasswordFra.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportPasswordFra.TabIndex = 7;
            // 
            // txtLanguageSupportUserChn
            // 
            this.txtLanguageSupportUserChn.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserChn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportUserChn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportUserChn.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserChn.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportUserChn.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportUserChn.Location = new System.Drawing.Point(161, 108);
            this.txtLanguageSupportUserChn.Name = "txtLanguageSupportUserChn";
            this.txtLanguageSupportUserChn.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportUserChn.PasswordChar = '\0';
            this.txtLanguageSupportUserChn.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportUserChn.TabIndex = 15;
            // 
            // txtLanguageSupportUserCht
            // 
            this.txtLanguageSupportUserCht.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserCht.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportUserCht.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportUserCht.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserCht.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportUserCht.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportUserCht.Location = new System.Drawing.Point(161, 85);
            this.txtLanguageSupportUserCht.Name = "txtLanguageSupportUserCht";
            this.txtLanguageSupportUserCht.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportUserCht.PasswordChar = '\0';
            this.txtLanguageSupportUserCht.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportUserCht.TabIndex = 12;
            // 
            // txtLanguageSupportUserEsn
            // 
            this.txtLanguageSupportUserEsn.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserEsn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportUserEsn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportUserEsn.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserEsn.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportUserEsn.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportUserEsn.Location = new System.Drawing.Point(161, 62);
            this.txtLanguageSupportUserEsn.Name = "txtLanguageSupportUserEsn";
            this.txtLanguageSupportUserEsn.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportUserEsn.PasswordChar = '\0';
            this.txtLanguageSupportUserEsn.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportUserEsn.TabIndex = 9;
            // 
            // txtLanguageSupportUserFra
            // 
            this.txtLanguageSupportUserFra.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserFra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLanguageSupportUserFra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLanguageSupportUserFra.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtLanguageSupportUserFra.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtLanguageSupportUserFra.FocusedBorderColor = System.Drawing.Color.Blue;
            this.txtLanguageSupportUserFra.Location = new System.Drawing.Point(161, 39);
            this.txtLanguageSupportUserFra.Name = "txtLanguageSupportUserFra";
            this.txtLanguageSupportUserFra.Padding = new System.Windows.Forms.Padding(1);
            this.txtLanguageSupportUserFra.PasswordChar = '\0';
            this.txtLanguageSupportUserFra.Size = new System.Drawing.Size(88, 20);
            this.txtLanguageSupportUserFra.TabIndex = 6;
            // 
            // lblLanguageSupportPassword
            // 
            this.lblLanguageSupportPassword.AutoSize = true;
            this.lblLanguageSupportPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLanguageSupportPassword.Location = new System.Drawing.Point(274, 18);
            this.lblLanguageSupportPassword.Name = "lblLanguageSupportPassword";
            this.lblLanguageSupportPassword.Size = new System.Drawing.Size(53, 13);
            this.lblLanguageSupportPassword.TabIndex = 20;
            this.lblLanguageSupportPassword.Text = "Password";
            // 
            // lblLanguageSupportUser
            // 
            this.lblLanguageSupportUser.AutoSize = true;
            this.lblLanguageSupportUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLanguageSupportUser.Location = new System.Drawing.Point(158, 18);
            this.lblLanguageSupportUser.Name = "lblLanguageSupportUser";
            this.lblLanguageSupportUser.Size = new System.Drawing.Size(29, 13);
            this.lblLanguageSupportUser.TabIndex = 20;
            this.lblLanguageSupportUser.Text = "User";
            // 
            // chkLanguageChn
            // 
            this.chkLanguageChn.AutoSize = true;
            this.chkLanguageChn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkLanguageChn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLanguageChn.Location = new System.Drawing.Point(22, 110);
            this.chkLanguageChn.Name = "chkLanguageChn";
            this.chkLanguageChn.Size = new System.Drawing.Size(117, 17);
            this.chkLanguageChn.TabIndex = 14;
            this.chkLanguageChn.Text = "Chinese (Simplified)";
            this.chkLanguageChn.UseVisualStyleBackColor = true;
            this.chkLanguageChn.CheckedChanged += new System.EventHandler(this.chkLanguageChn_CheckedChanged);
            // 
            // chkLanguageCht
            // 
            this.chkLanguageCht.AutoSize = true;
            this.chkLanguageCht.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkLanguageCht.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLanguageCht.Location = new System.Drawing.Point(22, 87);
            this.chkLanguageCht.Name = "chkLanguageCht";
            this.chkLanguageCht.Size = new System.Drawing.Size(122, 17);
            this.chkLanguageCht.TabIndex = 11;
            this.chkLanguageCht.Text = "Chinese (Traditional)";
            this.chkLanguageCht.UseVisualStyleBackColor = true;
            this.chkLanguageCht.CheckedChanged += new System.EventHandler(this.chkLanguageCht_CheckedChanged);
            // 
            // chkLanguageEsn
            // 
            this.chkLanguageEsn.AutoSize = true;
            this.chkLanguageEsn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkLanguageEsn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLanguageEsn.Location = new System.Drawing.Point(22, 64);
            this.chkLanguageEsn.Name = "chkLanguageEsn";
            this.chkLanguageEsn.Size = new System.Drawing.Size(64, 17);
            this.chkLanguageEsn.TabIndex = 8;
            this.chkLanguageEsn.Text = "Spanish";
            this.chkLanguageEsn.UseVisualStyleBackColor = true;
            this.chkLanguageEsn.CheckedChanged += new System.EventHandler(this.chkLanguageEsn_CheckedChanged);
            // 
            // chkLanguageFra
            // 
            this.chkLanguageFra.AutoSize = true;
            this.chkLanguageFra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkLanguageFra.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLanguageFra.Location = new System.Drawing.Point(22, 41);
            this.chkLanguageFra.Name = "chkLanguageFra";
            this.chkLanguageFra.Size = new System.Drawing.Size(59, 17);
            this.chkLanguageFra.TabIndex = 5;
            this.chkLanguageFra.Text = "French";
            this.chkLanguageFra.UseVisualStyleBackColor = true;
            this.chkLanguageFra.CheckedChanged += new System.EventHandler(this.chkLanguageFra_CheckedChanged);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.txtSQLScriptName);
            this.grpSettings.Controls.Add(this.lblSQLScriptName);
            this.grpSettings.Controls.Add(this.grpConfigurationFiles);
            this.grpSettings.Controls.Add(this.btnOutputPathFinder);
            this.grpSettings.Controls.Add(this.btnRootPathFinder);
            this.grpSettings.Controls.Add(this.txtOutputPath);
            this.grpSettings.Controls.Add(this.txtRootPath);
            this.grpSettings.Controls.Add(this.lblRootPath);
            this.grpSettings.Controls.Add(this.lblOutputPath);
            this.grpSettings.Controls.Add(this.btnOptionInquiry);
            this.grpSettings.Controls.Add(this.btnOptionCrm);
            this.grpSettings.Controls.Add(this.lblOption);
            this.grpSettings.Controls.Add(this.btnOptionAdhoc);
            this.grpSettings.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grpSettings.Location = new System.Drawing.Point(13, 167);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(713, 248);
            this.grpSettings.TabIndex = 16;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            this.grpSettings.Enter += new System.EventHandler(this.group_Enter);
            this.grpSettings.Leave += new System.EventHandler(this.group_Leave);
            // 
            // txtSQLScriptName
            // 
            this.txtSQLScriptName.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtSQLScriptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSQLScriptName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtSQLScriptName.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtSQLScriptName.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtSQLScriptName.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtSQLScriptName, 5);
            this.txtSQLScriptName.Location = new System.Drawing.Point(109, 96);
            this.txtSQLScriptName.Name = "txtSQLScriptName";
            this.txtSQLScriptName.Padding = new System.Windows.Forms.Padding(1);
            this.txtSQLScriptName.PasswordChar = '\0';
            this.txtSQLScriptName.Size = new System.Drawing.Size(155, 20);
            this.txtSQLScriptName.TabIndex = 22;
            this.txtSQLScriptName.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtSQLScriptName.Validating += new System.ComponentModel.CancelEventHandler(this.txtSQLScriptName_Validating);
            // 
            // lblSQLScriptName
            // 
            this.lblSQLScriptName.AutoSize = true;
            this.lblSQLScriptName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSQLScriptName.Location = new System.Drawing.Point(11, 99);
            this.lblSQLScriptName.Name = "lblSQLScriptName";
            this.lblSQLScriptName.Size = new System.Drawing.Size(92, 13);
            this.lblSQLScriptName.TabIndex = 41;
            this.lblSQLScriptName.Text = "SQL Script Name:";
            // 
            // grpConfigurationFiles
            // 
            this.grpConfigurationFiles.Controls.Add(this.txtDatasourceConfigurationFile);
            this.grpConfigurationFiles.Controls.Add(this.lblDatasourceConfigurationFile);
            this.grpConfigurationFiles.Controls.Add(this.lblTemplateConfigurationFile);
            this.grpConfigurationFiles.Controls.Add(this.btnTemplateConfigurationFileFinder);
            this.grpConfigurationFiles.Controls.Add(this.txtTemplateConfigurationFile);
            this.grpConfigurationFiles.Controls.Add(this.btnDatasourceConfigurationFileFinder);
            this.grpConfigurationFiles.Location = new System.Drawing.Point(111, 150);
            this.grpConfigurationFiles.Name = "grpConfigurationFiles";
            this.grpConfigurationFiles.Size = new System.Drawing.Size(586, 83);
            this.grpConfigurationFiles.TabIndex = 40;
            this.grpConfigurationFiles.TabStop = false;
            this.grpConfigurationFiles.Text = "Configuration Files";
            // 
            // txtDatasourceConfigurationFile
            // 
            this.txtDatasourceConfigurationFile.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtDatasourceConfigurationFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDatasourceConfigurationFile.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDatasourceConfigurationFile.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtDatasourceConfigurationFile.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtDatasourceConfigurationFile.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtDatasourceConfigurationFile, 19);
            this.txtDatasourceConfigurationFile.Location = new System.Drawing.Point(100, 19);
            this.txtDatasourceConfigurationFile.Name = "txtDatasourceConfigurationFile";
            this.txtDatasourceConfigurationFile.Padding = new System.Windows.Forms.Padding(1);
            this.txtDatasourceConfigurationFile.PasswordChar = '\0';
            this.txtDatasourceConfigurationFile.Size = new System.Drawing.Size(437, 20);
            this.txtDatasourceConfigurationFile.TabIndex = 25;
            this.txtDatasourceConfigurationFile.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtDatasourceConfigurationFile.Validating += new System.ComponentModel.CancelEventHandler(this.txtDatasourceConfigurationFile_Validating);
            // 
            // lblDatasourceConfigurationFile
            // 
            this.lblDatasourceConfigurationFile.AutoSize = true;
            this.lblDatasourceConfigurationFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDatasourceConfigurationFile.Location = new System.Drawing.Point(33, 21);
            this.lblDatasourceConfigurationFile.Name = "lblDatasourceConfigurationFile";
            this.lblDatasourceConfigurationFile.Size = new System.Drawing.Size(62, 13);
            this.lblDatasourceConfigurationFile.TabIndex = 32;
            this.lblDatasourceConfigurationFile.Text = "Datasource";
            // 
            // lblTemplateConfigurationFile
            // 
            this.lblTemplateConfigurationFile.AutoSize = true;
            this.lblTemplateConfigurationFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTemplateConfigurationFile.Location = new System.Drawing.Point(44, 47);
            this.lblTemplateConfigurationFile.Name = "lblTemplateConfigurationFile";
            this.lblTemplateConfigurationFile.Size = new System.Drawing.Size(51, 13);
            this.lblTemplateConfigurationFile.TabIndex = 34;
            this.lblTemplateConfigurationFile.Text = "Template";
            // 
            // btnTemplateConfigurationFileFinder
            // 
            this.btnTemplateConfigurationFileFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTemplateConfigurationFileFinder.BackgroundImage")));
            this.btnTemplateConfigurationFileFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTemplateConfigurationFileFinder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTemplateConfigurationFileFinder.FlatAppearance.BorderSize = 0;
            this.btnTemplateConfigurationFileFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTemplateConfigurationFileFinder.Location = new System.Drawing.Point(538, 46);
            this.btnTemplateConfigurationFileFinder.Name = "btnTemplateConfigurationFileFinder";
            this.btnTemplateConfigurationFileFinder.Size = new System.Drawing.Size(19, 20);
            this.btnTemplateConfigurationFileFinder.TabIndex = 28;
            this.btnTemplateConfigurationFileFinder.TabStop = false;
            this.btnTemplateConfigurationFileFinder.Text = " = String.Empty;";
            this.btnTemplateConfigurationFileFinder.UseVisualStyleBackColor = true;
            this.btnTemplateConfigurationFileFinder.Click += new System.EventHandler(this.btnTemplateConfigurationFileFinder_Click);
            // 
            // txtTemplateConfigurationFile
            // 
            this.txtTemplateConfigurationFile.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtTemplateConfigurationFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTemplateConfigurationFile.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtTemplateConfigurationFile.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtTemplateConfigurationFile.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtTemplateConfigurationFile.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtTemplateConfigurationFile, 19);
            this.txtTemplateConfigurationFile.Location = new System.Drawing.Point(100, 45);
            this.txtTemplateConfigurationFile.Name = "txtTemplateConfigurationFile";
            this.txtTemplateConfigurationFile.Padding = new System.Windows.Forms.Padding(1);
            this.txtTemplateConfigurationFile.PasswordChar = '\0';
            this.txtTemplateConfigurationFile.Size = new System.Drawing.Size(437, 20);
            this.txtTemplateConfigurationFile.TabIndex = 27;
            this.txtTemplateConfigurationFile.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtTemplateConfigurationFile.Validating += new System.ComponentModel.CancelEventHandler(this.txtTemplateConfigurationFile_Validating);
            // 
            // btnDatasourceConfigurationFileFinder
            // 
            this.btnDatasourceConfigurationFileFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDatasourceConfigurationFileFinder.BackgroundImage")));
            this.btnDatasourceConfigurationFileFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDatasourceConfigurationFileFinder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDatasourceConfigurationFileFinder.FlatAppearance.BorderSize = 0;
            this.btnDatasourceConfigurationFileFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatasourceConfigurationFileFinder.Location = new System.Drawing.Point(538, 20);
            this.btnDatasourceConfigurationFileFinder.Name = "btnDatasourceConfigurationFileFinder";
            this.btnDatasourceConfigurationFileFinder.Size = new System.Drawing.Size(19, 20);
            this.btnDatasourceConfigurationFileFinder.TabIndex = 26;
            this.btnDatasourceConfigurationFileFinder.TabStop = false;
            this.btnDatasourceConfigurationFileFinder.Text = " = String.Empty;";
            this.btnDatasourceConfigurationFileFinder.UseVisualStyleBackColor = true;
            this.btnDatasourceConfigurationFileFinder.Click += new System.EventHandler(this.btnDatasourceConfigurationFileFinder_Click);
            // 
            // btnOutputPathFinder
            // 
            this.btnOutputPathFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOutputPathFinder.BackgroundImage")));
            this.btnOutputPathFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOutputPathFinder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOutputPathFinder.FlatAppearance.BorderSize = 0;
            this.btnOutputPathFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutputPathFinder.Location = new System.Drawing.Point(648, 125);
            this.btnOutputPathFinder.Name = "btnOutputPathFinder";
            this.btnOutputPathFinder.Size = new System.Drawing.Size(19, 20);
            this.btnOutputPathFinder.TabIndex = 24;
            this.btnOutputPathFinder.TabStop = false;
            this.btnOutputPathFinder.Text = " = String.Empty;";
            this.btnOutputPathFinder.UseVisualStyleBackColor = true;
            this.btnOutputPathFinder.Click += new System.EventHandler(this.btnOutputPathFinder_Click);
            // 
            // btnRootPathFinder
            // 
            this.btnRootPathFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRootPathFinder.BackgroundImage")));
            this.btnRootPathFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRootPathFinder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRootPathFinder.FlatAppearance.BorderSize = 0;
            this.btnRootPathFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRootPathFinder.Location = new System.Drawing.Point(648, 69);
            this.btnRootPathFinder.Name = "btnRootPathFinder";
            this.btnRootPathFinder.Size = new System.Drawing.Size(19, 20);
            this.btnRootPathFinder.TabIndex = 21;
            this.btnRootPathFinder.TabStop = false;
            this.btnRootPathFinder.Text = " = String.Empty;";
            this.btnRootPathFinder.UseVisualStyleBackColor = true;
            this.btnRootPathFinder.Click += new System.EventHandler(this.btnRootPathFinder_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtOutputPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOutputPath.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtOutputPath.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtOutputPath.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtOutputPath.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtOutputPath, 19);
            this.txtOutputPath.Location = new System.Drawing.Point(210, 124);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtOutputPath.PasswordChar = '\0';
            this.txtOutputPath.Size = new System.Drawing.Size(437, 20);
            this.txtOutputPath.TabIndex = 23;
            this.txtOutputPath.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtOutputPath.Validating += new System.ComponentModel.CancelEventHandler(this.txtOutputPath_Validating);
            // 
            // txtRootPath
            // 
            this.txtRootPath.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtRootPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRootPath.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtRootPath.DefaultBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtRootPath.ErrorBorderColor = System.Drawing.Color.Red;
            this.txtRootPath.FocusedBorderColor = System.Drawing.Color.Blue;
            this.errorProvider.SetIconPadding(this.txtRootPath, 19);
            this.txtRootPath.Location = new System.Drawing.Point(109, 69);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtRootPath.PasswordChar = '\0';
            this.txtRootPath.Size = new System.Drawing.Size(538, 20);
            this.txtRootPath.TabIndex = 20;
            this.txtRootPath.Enter += new System.EventHandler(this.textBox_Enter);
            this.txtRootPath.Validating += new System.ComponentModel.CancelEventHandler(this.txtRootPath_Validating);
            // 
            // lblRootPath
            // 
            this.lblRootPath.AutoSize = true;
            this.lblRootPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRootPath.Location = new System.Drawing.Point(44, 71);
            this.lblRootPath.Name = "lblRootPath";
            this.lblRootPath.Size = new System.Drawing.Size(58, 13);
            this.lblRootPath.TabIndex = 24;
            this.lblRootPath.Text = "Root Path:";
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOutputPath.Location = new System.Drawing.Point(139, 127);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(67, 13);
            this.lblOutputPath.TabIndex = 26;
            this.lblOutputPath.Text = "Output Path:";
            // 
            // btnOptionInquiry
            // 
            this.btnOptionInquiry.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnOptionInquiry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOptionInquiry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionInquiry.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnOptionInquiry.Location = new System.Drawing.Point(272, 23);
            this.btnOptionInquiry.Name = "btnOptionInquiry";
            this.btnOptionInquiry.Size = new System.Drawing.Size(75, 38);
            this.btnOptionInquiry.TabIndex = 19;
            this.btnOptionInquiry.Text = "Inquiry";
            this.btnOptionInquiry.UseVisualStyleBackColor = false;
            this.btnOptionInquiry.Click += new System.EventHandler(this.btnOptionInquiry_Click);
            // 
            // btnOptionCrm
            // 
            this.btnOptionCrm.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnOptionCrm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOptionCrm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionCrm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOptionCrm.Location = new System.Drawing.Point(190, 23);
            this.btnOptionCrm.Name = "btnOptionCrm";
            this.btnOptionCrm.Size = new System.Drawing.Size(75, 38);
            this.btnOptionCrm.TabIndex = 18;
            this.btnOptionCrm.Text = "CRM";
            this.btnOptionCrm.UseVisualStyleBackColor = false;
            this.btnOptionCrm.Click += new System.EventHandler(this.btnOptionCrm_Click);
            // 
            // lblOption
            // 
            this.lblOption.AutoSize = true;
            this.lblOption.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOption.Location = new System.Drawing.Point(63, 35);
            this.lblOption.Name = "lblOption";
            this.lblOption.Size = new System.Drawing.Size(41, 13);
            this.lblOption.TabIndex = 28;
            this.lblOption.Text = "Option:";
            // 
            // btnOptionAdhoc
            // 
            this.btnOptionAdhoc.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnOptionAdhoc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOptionAdhoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOptionAdhoc.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnOptionAdhoc.Location = new System.Drawing.Point(109, 23);
            this.btnOptionAdhoc.Name = "btnOptionAdhoc";
            this.btnOptionAdhoc.Size = new System.Drawing.Size(75, 38);
            this.btnOptionAdhoc.TabIndex = 17;
            this.btnOptionAdhoc.Text = "Adhoc";
            this.btnOptionAdhoc.UseVisualStyleBackColor = false;
            this.btnOptionAdhoc.Click += new System.EventHandler(this.btnOptionAdhoc_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveSettings.Location = new System.Drawing.Point(114, 544);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(95, 44);
            this.btnSaveSettings.TabIndex = 30;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            this.errorProvider.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider.Icon")));
            // 
            // txtLogWindow
            // 
            this.txtLogWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtLogWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogWindow.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogWindow.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.txtLogWindow.Location = new System.Drawing.Point(13, 437);
            this.txtLogWindow.Multiline = true;
            this.txtLogWindow.Name = "txtLogWindow";
            this.txtLogWindow.ReadOnly = true;
            this.txtLogWindow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogWindow.Size = new System.Drawing.Size(713, 98);
            this.txtLogWindow.TabIndex = 32;
            // 
            // lblConsole
            // 
            this.lblConsole.AutoSize = true;
            this.lblConsole.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConsole.Location = new System.Drawing.Point(10, 420);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(45, 13);
            this.lblConsole.TabIndex = 8;
            this.lblConsole.Text = "Console";
            // 
            // chkDisplayOutputFolderOnCompletion
            // 
            this.chkDisplayOutputFolderOnCompletion.AutoSize = true;
            this.chkDisplayOutputFolderOnCompletion.Location = new System.Drawing.Point(215, 548);
            this.chkDisplayOutputFolderOnCompletion.Name = "chkDisplayOutputFolderOnCompletion";
            this.chkDisplayOutputFolderOnCompletion.Size = new System.Drawing.Size(191, 17);
            this.chkDisplayOutputFolderOnCompletion.TabIndex = 33;
            this.chkDisplayOutputFolderOnCompletion.Text = "Display output folder on completion";
            this.chkDisplayOutputFolderOnCompletion.UseVisualStyleBackColor = true;
            // 
            // chkDisplayLogFileOnCompletion
            // 
            this.chkDisplayLogFileOnCompletion.AutoSize = true;
            this.chkDisplayLogFileOnCompletion.Location = new System.Drawing.Point(215, 567);
            this.chkDisplayLogFileOnCompletion.Name = "chkDisplayLogFileOnCompletion";
            this.chkDisplayLogFileOnCompletion.Size = new System.Drawing.Size(162, 17);
            this.chkDisplayLogFileOnCompletion.TabIndex = 34;
            this.chkDisplayLogFileOnCompletion.Text = "Display log file on completion";
            this.chkDisplayLogFileOnCompletion.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(735, 603);
            this.Controls.Add(this.chkDisplayLogFileOnCompletion);
            this.Controls.Add(this.chkDisplayOutputFolderOnCompletion);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.txtLogWindow);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpLanguageSupport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.grpCredentials);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inquiry Configuration";
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            this.grpLanguageSupport.ResumeLayout(false);
            this.grpLanguageSupport.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpConfigurationFiles.ResumeLayout(false);
            this.grpConfigurationFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCredentials;
        private BorderedTextBox txtPassword;
        private BorderedTextBox txtUser;
        private BorderedTextBox txtCompany;
        private BorderedTextBox txtVersion;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpLanguageSupport;
        private System.Windows.Forms.CheckBox chkLanguageChn;
        private System.Windows.Forms.CheckBox chkLanguageCht;
        private System.Windows.Forms.CheckBox chkLanguageEsn;
        private System.Windows.Forms.CheckBox chkLanguageFra;
        private BorderedTextBox txtLanguageSupportPasswordChn;
        private BorderedTextBox txtLanguageSupportPasswordCht;
        private BorderedTextBox txtLanguageSupportPasswordEsn;
        private BorderedTextBox txtLanguageSupportPasswordFra;
        private BorderedTextBox txtLanguageSupportUserChn;
        private BorderedTextBox txtLanguageSupportUserCht;
        private BorderedTextBox txtLanguageSupportUserEsn;
        private BorderedTextBox txtLanguageSupportUserFra;
        private System.Windows.Forms.Label lblLanguageSupportPassword;
        private System.Windows.Forms.Label lblLanguageSupportUser;
        private System.Windows.Forms.GroupBox grpSettings;
        private BorderedTextBox txtOutputPath;
        private System.Windows.Forms.Label lblOutputPath;
        private BorderedTextBox txtRootPath;
        private System.Windows.Forms.Label lblRootPath;
        private System.Windows.Forms.Button btnOptionInquiry;
        private System.Windows.Forms.Button btnOptionCrm;
        private System.Windows.Forms.Label lblOption;
        private System.Windows.Forms.Button btnOptionAdhoc;
        private BorderedTextBox txtTemplateConfigurationFile;
        private System.Windows.Forms.Label lblTemplateConfigurationFile;
        private BorderedTextBox txtDatasourceConfigurationFile;
        private System.Windows.Forms.Label lblDatasourceConfigurationFile;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnRootPathFinder;
        private System.Windows.Forms.Button btnOutputPathFinder;
        private System.Windows.Forms.Button btnTemplateConfigurationFileFinder;
        private System.Windows.Forms.Button btnDatasourceConfigurationFileFinder;
        private System.Windows.Forms.GroupBox grpConfigurationFiles;
        private BorderedTextBox txtSQLScriptName;
        private System.Windows.Forms.Label lblSQLScriptName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox txtLogWindow;
        private System.Windows.Forms.Label lblConsole;
        private System.Windows.Forms.CheckBox chkDisplayLogFileOnCompletion;
        private System.Windows.Forms.CheckBox chkDisplayOutputFolderOnCompletion;
    }
}

