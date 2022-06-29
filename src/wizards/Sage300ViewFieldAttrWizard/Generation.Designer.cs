namespace Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard
{
    partial class Generation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generation));
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.lblUpperBorder = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlWizardSummary = new System.Windows.Forms.Panel();
            this.lblWizardSummary = new MetroFramework.Controls.MetroLabel();
            this.pnlGenerated = new System.Windows.Forms.Panel();
            this.splitGenerated = new System.Windows.Forms.SplitContainer();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.lblGenerated = new MetroFramework.Controls.MetroLabel();
            this.pnlFolderCreds = new System.Windows.Forms.Panel();
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.txtUser = new MetroFramework.Controls.MetroTextBox();
            this.txtCompany = new MetroFramework.Controls.MetroTextBox();
            this.txtPassword = new MetroFramework.Controls.MetroTextBox();
            this.txtVersion = new MetroFramework.Controls.MetroTextBox();
            this.lblCompany = new MetroFramework.Controls.MetroLabel();
            this.lblVersion = new MetroFramework.Controls.MetroLabel();
            this.lblPassword = new MetroFramework.Controls.MetroLabel();
            this.lblUser = new MetroFramework.Controls.MetroLabel();
            this.lblFolder = new MetroFramework.Controls.MetroLabel();
            this.txtFolderName = new MetroFramework.Controls.MetroTextBox();
            this.pnlGenerate = new System.Windows.Forms.Panel();
            this.txtFilesToModify = new MetroFramework.Controls.MetroTextBox();
            this.lblGenerateFiles = new MetroFramework.Controls.MetroLabel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblLowerBorder = new System.Windows.Forms.Label();
            this.btnNext = new MetroFramework.Controls.MetroButton();
            this.btnBack = new MetroFramework.Controls.MetroButton();
            this.lblProcessingFile = new MetroFramework.Controls.MetroLabel();
            this.tbrControls = new System.Windows.Forms.ToolStrip();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            this.pnlWizardSummary.SuspendLayout();
            this.pnlGenerated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitGenerated)).BeginInit();
            this.splitGenerated.Panel1.SuspendLayout();
            this.splitGenerated.Panel2.SuspendLayout();
            this.splitGenerated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.pnlFolderCreds.SuspendLayout();
            this.grpCredentials.SuspendLayout();
            this.pnlGenerate.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(20, 60);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.Controls.Add(this.lblUpperBorder);
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(921, 555);
            this.splitBase.SplitterDistance = 90;
            this.splitBase.TabIndex = 1;
            this.splitBase.TabStop = false;
            // 
            // lblUpperBorder
            // 
            this.lblUpperBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblUpperBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblUpperBorder.Location = new System.Drawing.Point(0, 89);
            this.lblUpperBorder.Name = "lblUpperBorder";
            this.lblUpperBorder.Size = new System.Drawing.Size(921, 1);
            this.lblUpperBorder.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard.Properties.Resources.sage_logo_square;
            this.pictureBox1.Location = new System.Drawing.Point(847, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(72, 69);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(9, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(801, 50);
            this.lblStepDescription.TabIndex = 5;
            this.lblStepDescription.Text = "This is the detailed description";
            this.lblStepDescription.WrapToLine = true;
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.AutoSize = true;
            this.lblStepTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblStepTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lblStepTitle.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblStepTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepTitle.Location = new System.Drawing.Point(9, 9);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(234, 25);
            this.lblStepTitle.Style = MetroFramework.MetroColorStyle.Green;
            this.lblStepTitle.TabIndex = 4;
            this.lblStepTitle.Text = "This is the title of the step";
            this.lblStepTitle.UseStyleColors = true;
            // 
            // splitSteps
            // 
            this.splitSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSteps.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSteps.IsSplitterFixed = true;
            this.splitSteps.Location = new System.Drawing.Point(0, 0);
            this.splitSteps.Name = "splitSteps";
            this.splitSteps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSteps.Panel1
            // 
            this.splitSteps.Panel1.Controls.Add(this.pnlWizardSummary);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerated);
            this.splitSteps.Panel1.Controls.Add(this.pnlFolderCreds);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerate);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(921, 461);
            this.splitSteps.SplitterDistance = 410;
            this.splitSteps.TabIndex = 5;
            // 
            // pnlWizardSummary
            // 
            this.pnlWizardSummary.Controls.Add(this.lblWizardSummary);
            this.pnlWizardSummary.Location = new System.Drawing.Point(825, 3);
            this.pnlWizardSummary.Name = "pnlWizardSummary";
            this.pnlWizardSummary.Size = new System.Drawing.Size(481, 42);
            this.pnlWizardSummary.TabIndex = 0;
            // 
            // lblWizardSummary
            // 
            this.lblWizardSummary.AutoSize = true;
            this.lblWizardSummary.Location = new System.Drawing.Point(186, 124);
            this.lblWizardSummary.Name = "lblWizardSummary";
            this.lblWizardSummary.Size = new System.Drawing.Size(438, 190);
            this.lblWizardSummary.TabIndex = 6;
            this.lblWizardSummary.Text = resources.GetString("lblWizardSummary.Text");
            // 
            // pnlGenerated
            // 
            this.pnlGenerated.Controls.Add(this.splitGenerated);
            this.pnlGenerated.Location = new System.Drawing.Point(805, 289);
            this.pnlGenerated.Name = "pnlGenerated";
            this.pnlGenerated.Size = new System.Drawing.Size(442, 79);
            this.pnlGenerated.TabIndex = 4;
            // 
            // splitGenerated
            // 
            this.splitGenerated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGenerated.IsSplitterFixed = true;
            this.splitGenerated.Location = new System.Drawing.Point(0, 0);
            this.splitGenerated.Name = "splitGenerated";
            this.splitGenerated.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitGenerated.Panel1
            // 
            this.splitGenerated.Panel1.Controls.Add(this.grdInfo);
            // 
            // splitGenerated.Panel2
            // 
            this.splitGenerated.Panel2.Controls.Add(this.lblGenerated);
            this.splitGenerated.Size = new System.Drawing.Size(442, 79);
            this.splitGenerated.SplitterDistance = 37;
            this.splitGenerated.TabIndex = 0;
            // 
            // grdInfo
            // 
            this.grdInfo.AllowUserToAddRows = false;
            this.grdInfo.AllowUserToDeleteRows = false;
            this.grdInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInfo.Location = new System.Drawing.Point(0, 0);
            this.grdInfo.Name = "grdInfo";
            this.grdInfo.Size = new System.Drawing.Size(442, 37);
            this.grdInfo.TabIndex = 0;
            // 
            // lblGenerated
            // 
            this.lblGenerated.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblGenerated.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGenerated.Location = new System.Drawing.Point(114, 40);
            this.lblGenerated.Name = "lblGenerated";
            this.lblGenerated.Size = new System.Drawing.Size(504, 84);
            this.lblGenerated.TabIndex = 0;
            this.lblGenerated.Text = resources.GetString("lblGenerated.Text");
            this.lblGenerated.WrapToLine = true;
            // 
            // pnlFolderCreds
            // 
            this.pnlFolderCreds.Controls.Add(this.grpCredentials);
            this.pnlFolderCreds.Controls.Add(this.lblFolder);
            this.pnlFolderCreds.Controls.Add(this.txtFolderName);
            this.pnlFolderCreds.Location = new System.Drawing.Point(23, 32);
            this.pnlFolderCreds.Name = "pnlFolderCreds";
            this.pnlFolderCreds.Size = new System.Drawing.Size(735, 307);
            this.pnlFolderCreds.TabIndex = 1;
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add(this.txtUser);
            this.grpCredentials.Controls.Add(this.txtCompany);
            this.grpCredentials.Controls.Add(this.txtPassword);
            this.grpCredentials.Controls.Add(this.txtVersion);
            this.grpCredentials.Controls.Add(this.lblCompany);
            this.grpCredentials.Controls.Add(this.lblVersion);
            this.grpCredentials.Controls.Add(this.lblPassword);
            this.grpCredentials.Controls.Add(this.lblUser);
            this.grpCredentials.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCredentials.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grpCredentials.Location = new System.Drawing.Point(69, 33);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(644, 98);
            this.grpCredentials.TabIndex = 0;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Application Credentials";
            // 
            // txtUser
            // 
            this.txtUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtUser.CustomButton.Image = null;
            this.txtUser.CustomButton.Location = new System.Drawing.Point(187, 1);
            this.txtUser.CustomButton.Name = "";
            this.txtUser.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtUser.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUser.CustomButton.TabIndex = 1;
            this.txtUser.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUser.CustomButton.UseSelectable = true;
            this.txtUser.CustomButton.Visible = false;
            this.txtUser.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtUser.Lines = new string[] {
        "ADMIN"};
            this.txtUser.Location = new System.Drawing.Point(84, 27);
            this.txtUser.MaxLength = 32767;
            this.txtUser.Name = "txtUser";
            this.txtUser.PasswordChar = '\0';
            this.txtUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUser.SelectedText = "";
            this.txtUser.SelectionLength = 0;
            this.txtUser.SelectionStart = 0;
            this.txtUser.ShortcutsEnabled = true;
            this.txtUser.Size = new System.Drawing.Size(211, 25);
            this.txtUser.Style = MetroFramework.MetroColorStyle.Green;
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "ADMIN";
            this.txtUser.UseSelectable = true;
            this.txtUser.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUser.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtCompany
            // 
            this.txtCompany.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtCompany.CustomButton.Image = null;
            this.txtCompany.CustomButton.Location = new System.Drawing.Point(59, 1);
            this.txtCompany.CustomButton.Name = "";
            this.txtCompany.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtCompany.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCompany.CustomButton.TabIndex = 1;
            this.txtCompany.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCompany.CustomButton.UseSelectable = true;
            this.txtCompany.CustomButton.Visible = false;
            this.txtCompany.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCompany.Lines = new string[] {
        "SAMLTD"};
            this.txtCompany.Location = new System.Drawing.Point(412, 58);
            this.txtCompany.MaxLength = 32767;
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.PasswordChar = '\0';
            this.txtCompany.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCompany.SelectedText = "";
            this.txtCompany.SelectionLength = 0;
            this.txtCompany.SelectionStart = 0;
            this.txtCompany.ShortcutsEnabled = true;
            this.txtCompany.Size = new System.Drawing.Size(83, 25);
            this.txtCompany.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCompany.TabIndex = 8;
            this.txtCompany.Text = "SAMLTD";
            this.txtCompany.UseSelectable = true;
            this.txtCompany.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCompany.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.CustomButton.Image = null;
            this.txtPassword.CustomButton.Location = new System.Drawing.Point(187, 1);
            this.txtPassword.CustomButton.Name = "";
            this.txtPassword.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPassword.CustomButton.TabIndex = 1;
            this.txtPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPassword.CustomButton.UseSelectable = true;
            this.txtPassword.CustomButton.Visible = false;
            this.txtPassword.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtPassword.Lines = new string[0];
            this.txtPassword.Location = new System.Drawing.Point(84, 57);
            this.txtPassword.MaxLength = 32767;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPassword.SelectedText = "";
            this.txtPassword.SelectionLength = 0;
            this.txtPassword.SelectionStart = 0;
            this.txtPassword.ShortcutsEnabled = true;
            this.txtPassword.Size = new System.Drawing.Size(211, 25);
            this.txtPassword.Style = MetroFramework.MetroColorStyle.Green;
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSelectable = true;
            this.txtPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtVersion
            // 
            this.txtVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtVersion.CustomButton.Image = null;
            this.txtVersion.CustomButton.Location = new System.Drawing.Point(59, 1);
            this.txtVersion.CustomButton.Name = "";
            this.txtVersion.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtVersion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtVersion.CustomButton.TabIndex = 1;
            this.txtVersion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtVersion.CustomButton.UseSelectable = true;
            this.txtVersion.CustomButton.Visible = false;
            this.txtVersion.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtVersion.Lines = new string[] {
        "[VERSION SET IN CODE]"};
            this.txtVersion.Location = new System.Drawing.Point(412, 28);
            this.txtVersion.MaxLength = 32767;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.PasswordChar = '\0';
            this.txtVersion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtVersion.SelectedText = "";
            this.txtVersion.SelectionLength = 0;
            this.txtVersion.SelectionStart = 0;
            this.txtVersion.ShortcutsEnabled = true;
            this.txtVersion.Size = new System.Drawing.Size(83, 25);
            this.txtVersion.Style = MetroFramework.MetroColorStyle.Green;
            this.txtVersion.TabIndex = 6;
            this.txtVersion.Text = "[VERSION SET IN CODE]";
            this.txtVersion.UseSelectable = true;
            this.txtVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCompany.Location = new System.Drawing.Point(336, 59);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(71, 19);
            this.lblCompany.TabIndex = 7;
            this.lblCompany.Text = "Company:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(350, 29);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(57, 19);
            this.lblVersion.TabIndex = 5;
            this.lblVersion.Text = "Version:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPassword.Location = new System.Drawing.Point(7, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 19);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblUser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUser.Location = new System.Drawing.Point(37, 29);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(40, 19);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "User:";
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblFolder.Location = new System.Drawing.Point(13, 153);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(50, 19);
            this.lblFolder.TabIndex = 9;
            this.lblFolder.Text = "Folder:";
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtFolderName.CustomButton.Image = null;
            this.txtFolderName.CustomButton.Location = new System.Drawing.Point(620, 1);
            this.txtFolderName.CustomButton.Name = "";
            this.txtFolderName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtFolderName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFolderName.CustomButton.TabIndex = 1;
            this.txtFolderName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFolderName.CustomButton.UseSelectable = true;
            this.txtFolderName.Lines = new string[0];
            this.txtFolderName.Location = new System.Drawing.Point(69, 153);
            this.txtFolderName.MaxLength = 255;
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.PasswordChar = '\0';
            this.txtFolderName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFolderName.SelectedText = "";
            this.txtFolderName.SelectionLength = 0;
            this.txtFolderName.SelectionStart = 0;
            this.txtFolderName.ShortcutsEnabled = true;
            this.txtFolderName.ShowButton = true;
            this.txtFolderName.ShowClearButton = true;
            this.txtFolderName.Size = new System.Drawing.Size(644, 25);
            this.txtFolderName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtFolderName.TabIndex = 10;
            this.txtFolderName.TabStop = false;
            this.txtFolderName.UseSelectable = true;
            this.txtFolderName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFolderName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtFolderName.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.btnFolder_Click);
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.Controls.Add(this.txtFilesToModify);
            this.pnlGenerate.Controls.Add(this.lblGenerateFiles);
            this.pnlGenerate.Location = new System.Drawing.Point(805, 162);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Size = new System.Drawing.Size(467, 69);
            this.pnlGenerate.TabIndex = 3;
            // 
            // txtFilesToModify
            // 
            this.txtFilesToModify.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtFilesToModify.CustomButton.Image = null;
            this.txtFilesToModify.CustomButton.Location = new System.Drawing.Point(433, 2);
            this.txtFilesToModify.CustomButton.Name = "";
            this.txtFilesToModify.CustomButton.Size = new System.Drawing.Size(31, 31);
            this.txtFilesToModify.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFilesToModify.CustomButton.TabIndex = 1;
            this.txtFilesToModify.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFilesToModify.CustomButton.UseSelectable = true;
            this.txtFilesToModify.CustomButton.Visible = false;
            this.txtFilesToModify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilesToModify.Lines = new string[0];
            this.txtFilesToModify.Location = new System.Drawing.Point(0, 33);
            this.txtFilesToModify.MaxLength = 32767;
            this.txtFilesToModify.Multiline = true;
            this.txtFilesToModify.Name = "txtFilesToModify";
            this.txtFilesToModify.PasswordChar = '\0';
            this.txtFilesToModify.ReadOnly = true;
            this.txtFilesToModify.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFilesToModify.SelectedText = "";
            this.txtFilesToModify.SelectionLength = 0;
            this.txtFilesToModify.SelectionStart = 0;
            this.txtFilesToModify.ShortcutsEnabled = true;
            this.txtFilesToModify.Size = new System.Drawing.Size(467, 36);
            this.txtFilesToModify.TabIndex = 1;
            this.txtFilesToModify.UseSelectable = true;
            this.txtFilesToModify.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFilesToModify.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblGenerateFiles
            // 
            this.lblGenerateFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGenerateFiles.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblGenerateFiles.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGenerateFiles.Location = new System.Drawing.Point(0, 0);
            this.lblGenerateFiles.Name = "lblGenerateFiles";
            this.lblGenerateFiles.Size = new System.Drawing.Size(467, 33);
            this.lblGenerateFiles.TabIndex = 2;
            this.lblGenerateFiles.Text = "The model file(s) that will be modified:";
            this.lblGenerateFiles.WrapToLine = true;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.lblLowerBorder);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(921, 47);
            this.pnlButtons.TabIndex = 2;
            // 
            // lblLowerBorder
            // 
            this.lblLowerBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblLowerBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLowerBorder.Location = new System.Drawing.Point(0, 0);
            this.lblLowerBorder.Name = "lblLowerBorder";
            this.lblLowerBorder.Size = new System.Drawing.Size(921, 1);
            this.lblLowerBorder.TabIndex = 8;
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(850, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.Style = MetroFramework.MetroColorStyle.Green;
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseCustomForeColor = true;
            this.btnNext.UseSelectable = true;
            this.btnNext.UseStyleColors = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnBack.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnBack.Highlight = true;
            this.btnBack.Location = new System.Drawing.Point(776, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.UseCustomForeColor = true;
            this.btnBack.UseSelectable = true;
            this.btnBack.UseStyleColors = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(11, 13);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 0);
            this.lblProcessingFile.TabIndex = 2;
            // 
            // tbrControls
            // 
            this.tbrControls.Location = new System.Drawing.Point(0, 0);
            this.tbrControls.Name = "tbrControls";
            this.tbrControls.Size = new System.Drawing.Size(100, 25);
            this.tbrControls.TabIndex = 0;
            // 
            // wrkBackground
            // 
            this.wrkBackground.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wrkBackground_DoWork);
            this.wrkBackground.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wrkBackground_RunWorkerCompleted);
            // 
            // Generation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(961, 635);
            this.Controls.Add(this.splitBase);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Generation";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "View Field Attribute Wizard";
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitSteps.Panel1.ResumeLayout(false);
            this.splitSteps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).EndInit();
            this.splitSteps.ResumeLayout(false);
            this.pnlWizardSummary.ResumeLayout(false);
            this.pnlWizardSummary.PerformLayout();
            this.pnlGenerated.ResumeLayout(false);
            this.splitGenerated.Panel1.ResumeLayout(false);
            this.splitGenerated.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGenerated)).EndInit();
            this.splitGenerated.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.pnlFolderCreds.ResumeLayout(false);
            this.pnlFolderCreds.PerformLayout();
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitBase;
        private MetroFramework.Controls.MetroButton btnNext;
        private MetroFramework.Controls.MetroButton btnBack;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlGenerated;
        private System.Windows.Forms.Panel pnlGenerate;
        private System.Windows.Forms.Panel pnlFolderCreds;
        private System.Windows.Forms.Panel pnlWizardSummary;
        private MetroFramework.Controls.MetroLabel lblStepDescription;
        private MetroFramework.Controls.MetroLabel lblStepTitle;
        private System.Windows.Forms.SplitContainer splitGenerated;
        private System.Windows.Forms.DataGridView grdInfo;
        private MetroFramework.Controls.MetroLabel lblGenerated;
        private MetroFramework.Controls.MetroLabel lblProcessingFile;
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ToolStrip tbrControls;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUpperBorder;
        private System.Windows.Forms.Label lblLowerBorder;
        private MetroFramework.Controls.MetroLabel lblWizardSummary;
        private System.Windows.Forms.GroupBox grpCredentials;
        private MetroFramework.Controls.MetroTextBox txtUser;
        private MetroFramework.Controls.MetroTextBox txtCompany;
        private MetroFramework.Controls.MetroTextBox txtPassword;
        private MetroFramework.Controls.MetroTextBox txtVersion;
        private MetroFramework.Controls.MetroLabel lblCompany;
        private MetroFramework.Controls.MetroLabel lblVersion;
        private MetroFramework.Controls.MetroLabel lblPassword;
        private MetroFramework.Controls.MetroLabel lblUser;
        private MetroFramework.Controls.MetroLabel lblFolder;
        private MetroFramework.Controls.MetroTextBox txtFolderName;
        private MetroFramework.Controls.MetroTextBox txtFilesToModify;
        private MetroFramework.Controls.MetroLabel lblGenerateFiles;
    }
}

