using System;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
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
            this.txtCompany = new MetroFramework.Controls.MetroTextBox();
            this.txtVersion = new MetroFramework.Controls.MetroTextBox();
            this.lblPassword = new MetroFramework.Controls.MetroLabel();
            this.lblUser = new MetroFramework.Controls.MetroLabel();
            this.lblVersion = new MetroFramework.Controls.MetroLabel();
            this.txtPassword = new MetroFramework.Controls.MetroTextBox();
            this.lblCompany = new MetroFramework.Controls.MetroLabel();
            this.txtUser = new MetroFramework.Controls.MetroTextBox();
            this.cboRepositoryType = new MetroFramework.Controls.MetroComboBox();
            this.lblRepositoryType = new MetroFramework.Controls.MetroLabel();
            this.lblViewID = new MetroFramework.Controls.MetroLabel();
            this.txtViewID = new MetroFramework.Controls.MetroTextBox();
            this.grdResourceInfo = new System.Windows.Forms.DataGridView();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.lblUpperBorder = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlUIGeneration = new System.Windows.Forms.Panel();
            this.splitDesigner = new System.Windows.Forms.SplitContainer();
            this.treeUIEntities = new System.Windows.Forms.TreeView();
            this.grpContainers = new System.Windows.Forms.GroupBox();
            this.tabUI = new System.Windows.Forms.TabControl();
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.txtPropWidget = new System.Windows.Forms.TextBox();
            this.lblPropType = new System.Windows.Forms.Label();
            this.txtPropText = new System.Windows.Forms.TextBox();
            this.lblPropText = new System.Windows.Forms.Label();
            this.tabPageFinder = new System.Windows.Forms.TabPage();
            this.pnlFinder = new System.Windows.Forms.Panel();
            this.btnFinderPropFile = new System.Windows.Forms.Button();
            this.txtFinderPropFile = new System.Windows.Forms.TextBox();
            this.lblFinderPropFile = new System.Windows.Forms.Label();
            this.lblFinderDisplay = new System.Windows.Forms.Label();
            this.lblFinderProp = new System.Windows.Forms.Label();
            this.cboFinderProp = new System.Windows.Forms.ComboBox();
            this.cboFinderDisplay = new System.Windows.Forms.ComboBox();
            this.tbrProperties = new System.Windows.Forms.ToolStrip();
            this.btnTab = new System.Windows.Forms.ToolStripButton();
            this.btnAddTabPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGrid = new System.Windows.Forms.ToolStripButton();
            this.btnButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteControl = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlCodeType = new System.Windows.Forms.Panel();
            this.lblCodeTypeFilesHelp = new MetroFramework.Controls.MetroLabel();
            this.lblUnknownCodeTypeFilesHelp = new MetroFramework.Controls.MetroLabel();
            this.lblCodeTypeDescriptionHelp = new MetroFramework.Controls.MetroLabel();
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.lblModule = new MetroFramework.Controls.MetroLabel();
            this.cboModule = new MetroFramework.Controls.MetroComboBox();
            this.pnlGeneratedCode = new System.Windows.Forms.Panel();
            this.pnlEntities = new System.Windows.Forms.Panel();
            this.splitEntities = new System.Windows.Forms.SplitContainer();
            this.pnlEntityTree = new System.Windows.Forms.Panel();
            this.pnlEntityGrid = new System.Windows.Forms.Panel();
            this.treeEntities = new System.Windows.Forms.TreeView();
            this.pnlEntitiesLabel = new System.Windows.Forms.Panel();
            this.lblEntities = new MetroFramework.Controls.MetroLabel();
            this.tabEntity = new MetroFramework.Controls.MetroTabControl();
            this.tabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.txtReportIniFile = new MetroFramework.Controls.MetroTextBox();
            this.txtEntityName = new MetroFramework.Controls.MetroTextBox();
            this.lblReportKeys = new MetroFramework.Controls.MetroLabel();
            this.cboReportKeys = new MetroFramework.Controls.MetroComboBox();
            this.txtResxName = new MetroFramework.Controls.MetroTextBox();
            this.lblReportIniFile = new MetroFramework.Controls.MetroLabel();
            this.txtReportProgramId = new MetroFramework.Controls.MetroTextBox();
            this.lblResxName = new MetroFramework.Controls.MetroLabel();
            this.lblReportProgramId = new MetroFramework.Controls.MetroLabel();
            this.txtModelName = new MetroFramework.Controls.MetroTextBox();
            this.lblModelName = new MetroFramework.Controls.MetroLabel();
            this.lblEntityName = new MetroFramework.Controls.MetroLabel();
            this.tabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.chkGenerateGridModel = new MetroFramework.Controls.MetroCheckBox();
            this.chkGenerateEnumerationsInSingleFile = new MetroFramework.Controls.MetroCheckBox();
            this.chkSequenceRevisionList = new MetroFramework.Controls.MetroCheckBox();
            this.chkGenerateIfExist = new MetroFramework.Controls.MetroCheckBox();
            this.chkGenerateClientFiles = new MetroFramework.Controls.MetroCheckBox();
            this.chkGenerateFinder = new MetroFramework.Controls.MetroCheckBox();
            this.chkGenerateDynamicEnablement = new MetroFramework.Controls.MetroCheckBox();
            this.tabPage3 = new MetroFramework.Controls.MetroTabPage();
            this.pnlColumns = new System.Windows.Forms.Panel();
            this.grdEntityFields = new System.Windows.Forms.DataGridView();
            this.tbrEntity = new System.Windows.Forms.ToolStrip();
            this.btnRowAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteRow = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteRows = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new MetroFramework.Controls.MetroTabPage();
            this.pnlComposition = new System.Windows.Forms.Panel();
            this.grdEntityCompositions = new System.Windows.Forms.DataGridView();
            this.pnlGenerateCode = new System.Windows.Forms.Panel();
            this.txtLayoutToGenerate = new MetroFramework.Controls.MetroTextBox();
            this.txtEntitiesToGenerate = new MetroFramework.Controls.MetroTextBox();
            this.lblGenerateHelp = new MetroFramework.Controls.MetroLabel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.lblLowerBorder = new System.Windows.Forms.Label();
            this.lblProcessingFile = new MetroFramework.Controls.MetroLabel();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.btnNext = new MetroFramework.Controls.MetroButton();
            this.btnBack = new MetroFramework.Controls.MetroButton();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdResourceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            this.pnlUIGeneration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDesigner)).BeginInit();
            this.splitDesigner.Panel2.SuspendLayout();
            this.splitDesigner.SuspendLayout();
            this.grpContainers.SuspendLayout();
            this.tabUI.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            this.tabPageFinder.SuspendLayout();
            this.pnlFinder.SuspendLayout();
            this.tbrProperties.SuspendLayout();
            this.pnlCodeType.SuspendLayout();
            this.grpCredentials.SuspendLayout();
            this.pnlGeneratedCode.SuspendLayout();
            this.pnlEntities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEntities)).BeginInit();
            this.splitEntities.Panel1.SuspendLayout();
            this.splitEntities.Panel2.SuspendLayout();
            this.splitEntities.SuspendLayout();
            this.pnlEntityTree.SuspendLayout();
            this.pnlEntityGrid.SuspendLayout();
            this.pnlEntitiesLabel.SuspendLayout();
            this.tabEntity.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.pnlColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEntityFields)).BeginInit();
            this.tbrEntity.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.pnlComposition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEntityCompositions)).BeginInit();
            this.pnlGenerateCode.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
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
            this.txtCompany.Location = new System.Drawing.Point(287, 57);
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
            this.txtCompany.TabIndex = 10;
            this.txtCompany.Text = "SAMLTD";
            this.txtCompany.UseSelectable = true;
            this.txtCompany.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCompany.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
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
            this.txtVersion.Location = new System.Drawing.Point(287, 27);
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
            this.txtVersion.TabIndex = 8;
            this.txtVersion.Text = "[VERSION SET IN CODE]";
            this.txtVersion.UseSelectable = true;
            this.txtVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPassword.Location = new System.Drawing.Point(7, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 19);
            this.lblPassword.TabIndex = 5;
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
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "User:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblVersion.Location = new System.Drawing.Point(225, 28);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(57, 19);
            this.lblVersion.TabIndex = 7;
            this.lblVersion.Text = "Version:";
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.CustomButton.Image = null;
            this.txtPassword.CustomButton.Location = new System.Drawing.Point(91, 1);
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
            this.txtPassword.Size = new System.Drawing.Size(115, 25);
            this.txtPassword.Style = MetroFramework.MetroColorStyle.Green;
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSelectable = true;
            this.txtPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCompany.Location = new System.Drawing.Point(211, 58);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(71, 19);
            this.lblCompany.TabIndex = 9;
            this.lblCompany.Text = "Company:";
            // 
            // txtUser
            // 
            this.txtUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtUser.CustomButton.Image = null;
            this.txtUser.CustomButton.Location = new System.Drawing.Point(91, 1);
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
            this.txtUser.Size = new System.Drawing.Size(115, 25);
            this.txtUser.Style = MetroFramework.MetroColorStyle.Green;
            this.txtUser.TabIndex = 4;
            this.txtUser.Text = "ADMIN";
            this.txtUser.UseSelectable = true;
            this.txtUser.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUser.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cboRepositoryType
            // 
            this.cboRepositoryType.FormattingEnabled = true;
            this.cboRepositoryType.ItemHeight = 23;
            this.cboRepositoryType.Items.AddRange(new object[] {
            "Flat",
            "Process",
            "Report",
            "HeaderDetail"});
            this.cboRepositoryType.Location = new System.Drawing.Point(98, 27);
            this.cboRepositoryType.Name = "cboRepositoryType";
            this.cboRepositoryType.Size = new System.Drawing.Size(141, 29);
            this.cboRepositoryType.Style = MetroFramework.MetroColorStyle.Green;
            this.cboRepositoryType.TabIndex = 1;
            this.cboRepositoryType.UseSelectable = true;
            this.cboRepositoryType.SelectedIndexChanged += new System.EventHandler(this.cboRepositoryType_SelectedIndexChanged);
            // 
            // lblRepositoryType
            // 
            this.lblRepositoryType.AutoSize = true;
            this.lblRepositoryType.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblRepositoryType.Location = new System.Drawing.Point(15, 30);
            this.lblRepositoryType.Name = "lblRepositoryType";
            this.lblRepositoryType.Size = new System.Drawing.Size(76, 19);
            this.lblRepositoryType.TabIndex = 0;
            this.lblRepositoryType.Text = "Code Type:";
            // 
            // lblViewID
            // 
            this.lblViewID.AutoSize = true;
            this.lblViewID.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblViewID.Location = new System.Drawing.Point(19, 15);
            this.lblViewID.Name = "lblViewID";
            this.lblViewID.Size = new System.Drawing.Size(59, 19);
            this.lblViewID.TabIndex = 2;
            this.lblViewID.Text = "View ID:";
            // 
            // txtViewID
            // 
            this.txtViewID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtViewID.CustomButton.Image = null;
            this.txtViewID.CustomButton.Location = new System.Drawing.Point(165, 1);
            this.txtViewID.CustomButton.Name = "";
            this.txtViewID.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtViewID.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtViewID.CustomButton.TabIndex = 1;
            this.txtViewID.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtViewID.CustomButton.UseSelectable = true;
            this.txtViewID.CustomButton.Visible = false;
            this.txtViewID.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtViewID.Lines = new string[0];
            this.txtViewID.Location = new System.Drawing.Point(83, 13);
            this.txtViewID.MaxLength = 32767;
            this.txtViewID.Name = "txtViewID";
            this.txtViewID.PasswordChar = '\0';
            this.txtViewID.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtViewID.SelectedText = "";
            this.txtViewID.SelectionLength = 0;
            this.txtViewID.SelectionStart = 0;
            this.txtViewID.ShortcutsEnabled = true;
            this.txtViewID.Size = new System.Drawing.Size(189, 25);
            this.txtViewID.Style = MetroFramework.MetroColorStyle.Green;
            this.txtViewID.TabIndex = 3;
            this.txtViewID.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtViewID.UseSelectable = true;
            this.txtViewID.WaterMarkColor = System.Drawing.Color.Maroon;
            this.txtViewID.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtViewID.Leave += new System.EventHandler(this.txtViewID_Leave);
            // 
            // grdResourceInfo
            // 
            this.grdResourceInfo.AllowUserToAddRows = false;
            this.grdResourceInfo.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdResourceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResourceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResourceInfo.Location = new System.Drawing.Point(0, 0);
            this.grdResourceInfo.Name = "grdResourceInfo";
            this.grdResourceInfo.Size = new System.Drawing.Size(497, 50);
            this.grdResourceInfo.TabIndex = 43;
            this.grdResourceInfo.TabStop = false;
            // 
            // wrkBackground
            // 
            this.wrkBackground.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wrkBackground_DoWork);
            this.wrkBackground.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wrkBackground_RunWorkerCompleted);
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(20, 60);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel1.Controls.Add(this.lblUpperBorder);
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(997, 605);
            this.splitBase.SplitterDistance = 88;
            this.splitBase.TabIndex = 9;
            // 
            // lblUpperBorder
            // 
            this.lblUpperBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblUpperBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblUpperBorder.Location = new System.Drawing.Point(0, 87);
            this.lblUpperBorder.Name = "lblUpperBorder";
            this.lblUpperBorder.Size = new System.Drawing.Size(997, 1);
            this.lblUpperBorder.TabIndex = 53;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties.Resources.sage300_logo_sq;
            this.pictureBox1.Location = new System.Drawing.Point(887, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(105, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(6, 37);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(672, 41);
            this.lblStepDescription.TabIndex = 3;
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
            this.lblStepTitle.Location = new System.Drawing.Point(6, 6);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(185, 25);
            this.lblStepTitle.Style = MetroFramework.MetroColorStyle.Green;
            this.lblStepTitle.TabIndex = 2;
            this.lblStepTitle.Text = "Step X - Step Details";
            this.lblStepTitle.UseStyleColors = true;
            // 
            // splitSteps
            // 
            this.splitSteps.BackColor = System.Drawing.Color.Transparent;
            this.splitSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSteps.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSteps.IsSplitterFixed = true;
            this.splitSteps.Location = new System.Drawing.Point(0, 0);
            this.splitSteps.Name = "splitSteps";
            this.splitSteps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSteps.Panel1
            // 
            this.splitSteps.Panel1.Controls.Add(this.pnlUIGeneration);
            this.splitSteps.Panel1.Controls.Add(this.pnlCodeType);
            this.splitSteps.Panel1.Controls.Add(this.pnlGeneratedCode);
            this.splitSteps.Panel1.Controls.Add(this.pnlEntities);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerateCode);
            this.splitSteps.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(997, 513);
            this.splitSteps.SplitterDistance = 430;
            this.splitSteps.TabIndex = 52;
            // 
            // pnlUIGeneration
            // 
            this.pnlUIGeneration.Controls.Add(this.splitDesigner);
            this.pnlUIGeneration.Location = new System.Drawing.Point(17, 20);
            this.pnlUIGeneration.Name = "pnlUIGeneration";
            this.pnlUIGeneration.Size = new System.Drawing.Size(901, 378);
            this.pnlUIGeneration.TabIndex = 52;
            // 
            // splitDesigner
            // 
            this.splitDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDesigner.Location = new System.Drawing.Point(0, 0);
            this.splitDesigner.Name = "splitDesigner";
            // 
            // splitDesigner.Panel1
            // 
            this.splitDesigner.Panel1.AllowDrop = true;
            this.splitDesigner.Panel1.BackColor = System.Drawing.Color.White;
            this.splitDesigner.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            // 
            // splitDesigner.Panel2
            // 
            this.splitDesigner.Panel2.Controls.Add(this.treeUIEntities);
            this.splitDesigner.Panel2.Controls.Add(this.grpContainers);
            this.splitDesigner.Size = new System.Drawing.Size(901, 378);
            this.splitDesigner.SplitterDistance = 629;
            this.splitDesigner.TabIndex = 2;
            // 
            // treeUIEntities
            // 
            this.treeUIEntities.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeUIEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeUIEntities.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeUIEntities.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeUIEntities.Location = new System.Drawing.Point(0, 181);
            this.treeUIEntities.Name = "treeUIEntities";
            this.treeUIEntities.Size = new System.Drawing.Size(268, 197);
            this.treeUIEntities.TabIndex = 4;
            this.treeUIEntities.TabStop = false;
            // 
            // grpContainers
            // 
            this.grpContainers.BackColor = System.Drawing.Color.White;
            this.grpContainers.Controls.Add(this.tabUI);
            this.grpContainers.Controls.Add(this.tbrProperties);
            this.grpContainers.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpContainers.Location = new System.Drawing.Point(0, 0);
            this.grpContainers.Name = "grpContainers";
            this.grpContainers.Size = new System.Drawing.Size(268, 181);
            this.grpContainers.TabIndex = 3;
            this.grpContainers.TabStop = false;
            this.grpContainers.Text = "Toolbox";
            // 
            // tabUI
            // 
            this.tabUI.Controls.Add(this.tabPageInfo);
            this.tabUI.Controls.Add(this.tabPageFinder);
            this.tabUI.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabUI.Location = new System.Drawing.Point(3, 60);
            this.tabUI.Name = "tabUI";
            this.tabUI.SelectedIndex = 0;
            this.tabUI.Size = new System.Drawing.Size(262, 118);
            this.tabUI.TabIndex = 20;
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.Controls.Add(this.txtPropWidget);
            this.tabPageInfo.Controls.Add(this.lblPropType);
            this.tabPageInfo.Controls.Add(this.txtPropText);
            this.tabPageInfo.Controls.Add(this.lblPropText);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInfo.Size = new System.Drawing.Size(254, 92);
            this.tabPageInfo.TabIndex = 0;
            this.tabPageInfo.Text = "Info";
            this.tabPageInfo.UseVisualStyleBackColor = true;
            // 
            // txtPropWidget
            // 
            this.txtPropWidget.Location = new System.Drawing.Point(41, 16);
            this.txtPropWidget.Name = "txtPropWidget";
            this.txtPropWidget.Size = new System.Drawing.Size(174, 22);
            this.txtPropWidget.TabIndex = 23;
            // 
            // lblPropType
            // 
            this.lblPropType.AutoSize = true;
            this.lblPropType.Location = new System.Drawing.Point(5, 19);
            this.lblPropType.Name = "lblPropType";
            this.lblPropType.Size = new System.Drawing.Size(33, 13);
            this.lblPropType.TabIndex = 22;
            this.lblPropType.Text = "Type:";
            // 
            // txtPropText
            // 
            this.txtPropText.Location = new System.Drawing.Point(41, 44);
            this.txtPropText.Name = "txtPropText";
            this.txtPropText.Size = new System.Drawing.Size(174, 22);
            this.txtPropText.TabIndex = 17;
            this.txtPropText.TextChanged += new System.EventHandler(this.txtPropText_TextChanged);
            // 
            // lblPropText
            // 
            this.lblPropText.AutoSize = true;
            this.lblPropText.Location = new System.Drawing.Point(5, 47);
            this.lblPropText.Name = "lblPropText";
            this.lblPropText.Size = new System.Drawing.Size(30, 13);
            this.lblPropText.TabIndex = 21;
            this.lblPropText.Text = "Text:";
            // 
            // tabPageFinder
            // 
            this.tabPageFinder.Controls.Add(this.pnlFinder);
            this.tabPageFinder.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinder.Name = "tabPageFinder";
            this.tabPageFinder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinder.Size = new System.Drawing.Size(254, 92);
            this.tabPageFinder.TabIndex = 1;
            this.tabPageFinder.Text = "Finder";
            this.tabPageFinder.UseVisualStyleBackColor = true;
            // 
            // pnlFinder
            // 
            this.pnlFinder.Controls.Add(this.btnFinderPropFile);
            this.pnlFinder.Controls.Add(this.txtFinderPropFile);
            this.pnlFinder.Controls.Add(this.lblFinderPropFile);
            this.pnlFinder.Controls.Add(this.lblFinderDisplay);
            this.pnlFinder.Controls.Add(this.lblFinderProp);
            this.pnlFinder.Controls.Add(this.cboFinderProp);
            this.pnlFinder.Controls.Add(this.cboFinderDisplay);
            this.pnlFinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinder.Location = new System.Drawing.Point(3, 3);
            this.pnlFinder.Name = "pnlFinder";
            this.pnlFinder.Size = new System.Drawing.Size(248, 86);
            this.pnlFinder.TabIndex = 0;
            // 
            // btnFinderPropFile
            // 
            this.btnFinderPropFile.Location = new System.Drawing.Point(218, 6);
            this.btnFinderPropFile.Name = "btnFinderPropFile";
            this.btnFinderPropFile.Size = new System.Drawing.Size(29, 22);
            this.btnFinderPropFile.TabIndex = 24;
            this.btnFinderPropFile.Text = "...";
            this.btnFinderPropFile.UseVisualStyleBackColor = true;
            this.btnFinderPropFile.Click += new System.EventHandler(this.btnFinderPropFile_Click);
            // 
            // txtFinderPropFile
            // 
            this.txtFinderPropFile.Location = new System.Drawing.Point(56, 6);
            this.txtFinderPropFile.Name = "txtFinderPropFile";
            this.txtFinderPropFile.Size = new System.Drawing.Size(156, 22);
            this.txtFinderPropFile.TabIndex = 23;
            // 
            // lblFinderPropFile
            // 
            this.lblFinderPropFile.AutoSize = true;
            this.lblFinderPropFile.Location = new System.Drawing.Point(22, 9);
            this.lblFinderPropFile.Name = "lblFinderPropFile";
            this.lblFinderPropFile.Size = new System.Drawing.Size(28, 13);
            this.lblFinderPropFile.TabIndex = 22;
            this.lblFinderPropFile.Text = "File:";
            // 
            // lblFinderDisplay
            // 
            this.lblFinderDisplay.AutoSize = true;
            this.lblFinderDisplay.Location = new System.Drawing.Point(5, 64);
            this.lblFinderDisplay.Name = "lblFinderDisplay";
            this.lblFinderDisplay.Size = new System.Drawing.Size(47, 13);
            this.lblFinderDisplay.TabIndex = 27;
            this.lblFinderDisplay.Text = "Display:";
            // 
            // lblFinderProp
            // 
            this.lblFinderProp.AutoSize = true;
            this.lblFinderProp.Location = new System.Drawing.Point(7, 37);
            this.lblFinderProp.Name = "lblFinderProp";
            this.lblFinderProp.Size = new System.Drawing.Size(45, 13);
            this.lblFinderProp.TabIndex = 25;
            this.lblFinderProp.Text = "Config:";
            // 
            // cboFinderProp
            // 
            this.cboFinderProp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFinderProp.FormattingEnabled = true;
            this.cboFinderProp.Location = new System.Drawing.Point(56, 34);
            this.cboFinderProp.Name = "cboFinderProp";
            this.cboFinderProp.Size = new System.Drawing.Size(189, 21);
            this.cboFinderProp.TabIndex = 26;
            this.cboFinderProp.SelectedIndexChanged += new System.EventHandler(this.cboFinderProp_SelectedIndexChanged);
            // 
            // cboFinderDisplay
            // 
            this.cboFinderDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFinderDisplay.FormattingEnabled = true;
            this.cboFinderDisplay.Location = new System.Drawing.Point(56, 61);
            this.cboFinderDisplay.Name = "cboFinderDisplay";
            this.cboFinderDisplay.Size = new System.Drawing.Size(186, 21);
            this.cboFinderDisplay.TabIndex = 28;
            this.cboFinderDisplay.SelectionChangeCommitted += new System.EventHandler(this.cboFinderDisplay_SelectionChangeCommitted);
            // 
            // tbrProperties
            // 
            this.tbrProperties.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.tbrProperties.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTab,
            this.btnAddTabPage,
            this.toolStripSeparator2,
            this.btnGrid,
            this.btnButton,
            this.toolStripSeparator1,
            this.btnDeleteControl,
            this.toolStripSeparator3});
            this.tbrProperties.Location = new System.Drawing.Point(3, 18);
            this.tbrProperties.Name = "tbrProperties";
            this.tbrProperties.Size = new System.Drawing.Size(262, 32);
            this.tbrProperties.TabIndex = 19;
            this.tbrProperties.Text = "toolStrip1";
            // 
            // btnTab
            // 
            this.btnTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTab.Image = ((System.Drawing.Image)(resources.GetObject("btnTab.Image")));
            this.btnTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTab.Name = "btnTab";
            this.btnTab.Size = new System.Drawing.Size(29, 29);
            this.btnTab.Tag = "Tab";
            this.btnTab.ToolTipText = "Add Tab Control";
            // 
            // btnAddTabPage
            // 
            this.btnAddTabPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddTabPage.Image = ((System.Drawing.Image)(resources.GetObject("btnAddTabPage.Image")));
            this.btnAddTabPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddTabPage.Name = "btnAddTabPage";
            this.btnAddTabPage.Size = new System.Drawing.Size(29, 29);
            this.btnAddTabPage.ToolTipText = "Add Tab Page";
            this.btnAddTabPage.Click += new System.EventHandler(this.btnAddTabPage_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // btnGrid
            // 
            this.btnGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGrid.Image = ((System.Drawing.Image)(resources.GetObject("btnGrid.Image")));
            this.btnGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(29, 29);
            this.btnGrid.Tag = "Grid";
            this.btnGrid.ToolTipText = "Add Grid Control";
            // 
            // btnButton
            // 
            this.btnButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnButton.Image = ((System.Drawing.Image)(resources.GetObject("btnButton.Image")));
            this.btnButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnButton.Name = "btnButton";
            this.btnButton.Size = new System.Drawing.Size(29, 29);
            this.btnButton.Tag = "Button";
            this.btnButton.ToolTipText = "Add Button Control";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // btnDeleteControl
            // 
            this.btnDeleteControl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteControl.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteControl.Image")));
            this.btnDeleteControl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteControl.Name = "btnDeleteControl";
            this.btnDeleteControl.Size = new System.Drawing.Size(29, 29);
            this.btnDeleteControl.ToolTipText = "Delete Control";
            this.btnDeleteControl.Click += new System.EventHandler(this.btnDeleteControl_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // pnlCodeType
            // 
            this.pnlCodeType.Controls.Add(this.lblCodeTypeFilesHelp);
            this.pnlCodeType.Controls.Add(this.lblUnknownCodeTypeFilesHelp);
            this.pnlCodeType.Controls.Add(this.lblCodeTypeDescriptionHelp);
            this.pnlCodeType.Controls.Add(this.lblRepositoryType);
            this.pnlCodeType.Controls.Add(this.cboRepositoryType);
            this.pnlCodeType.Controls.Add(this.grpCredentials);
            this.pnlCodeType.Controls.Add(this.lblModule);
            this.pnlCodeType.Controls.Add(this.cboModule);
            this.pnlCodeType.Location = new System.Drawing.Point(951, 25);
            this.pnlCodeType.Name = "pnlCodeType";
            this.pnlCodeType.Size = new System.Drawing.Size(719, 55);
            this.pnlCodeType.TabIndex = 44;
            // 
            // lblCodeTypeFilesHelp
            // 
            this.lblCodeTypeFilesHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCodeTypeFilesHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblCodeTypeFilesHelp.Location = new System.Drawing.Point(266, 128);
            this.lblCodeTypeFilesHelp.Name = "lblCodeTypeFilesHelp";
            this.lblCodeTypeFilesHelp.Size = new System.Drawing.Size(677, 61);
            this.lblCodeTypeFilesHelp.TabIndex = 4;
            this.lblCodeTypeFilesHelp.Text = resources.GetString("lblCodeTypeFilesHelp.Text");
            this.lblCodeTypeFilesHelp.WrapToLine = true;
            // 
            // lblUnknownCodeTypeFilesHelp
            // 
            this.lblUnknownCodeTypeFilesHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblUnknownCodeTypeFilesHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblUnknownCodeTypeFilesHelp.Location = new System.Drawing.Point(266, 74);
            this.lblUnknownCodeTypeFilesHelp.Name = "lblUnknownCodeTypeFilesHelp";
            this.lblUnknownCodeTypeFilesHelp.Size = new System.Drawing.Size(677, 46);
            this.lblUnknownCodeTypeFilesHelp.TabIndex = 3;
            this.lblUnknownCodeTypeFilesHelp.Text = "All Code Types will, at a minimum, generate a Model file, a Model Fields file, a " +
    "Model Mapper file and Enumeration files (based upon Presentation Lists in the Bu" +
    "siness View). ";
            this.lblUnknownCodeTypeFilesHelp.WrapToLine = true;
            // 
            // lblCodeTypeDescriptionHelp
            // 
            this.lblCodeTypeDescriptionHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCodeTypeDescriptionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblCodeTypeDescriptionHelp.Location = new System.Drawing.Point(266, 26);
            this.lblCodeTypeDescriptionHelp.Name = "lblCodeTypeDescriptionHelp";
            this.lblCodeTypeDescriptionHelp.Size = new System.Drawing.Size(692, 45);
            this.lblCodeTypeDescriptionHelp.TabIndex = 2;
            this.lblCodeTypeDescriptionHelp.Text = "A Code Type is based upon the Repository Type that will be used for the Business " +
    "View or Report.";
            this.lblCodeTypeDescriptionHelp.WrapToLine = true;
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
            this.grpCredentials.Location = new System.Drawing.Point(15, 187);
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size(389, 98);
            this.grpCredentials.TabIndex = 2;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Application Credentials";
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblModule.Location = new System.Drawing.Point(30, 301);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(59, 19);
            this.lblModule.TabIndex = 11;
            this.lblModule.Text = "Module:";
            // 
            // cboModule
            // 
            this.cboModule.FormattingEnabled = true;
            this.cboModule.ItemHeight = 23;
            this.cboModule.Location = new System.Drawing.Point(97, 297);
            this.cboModule.Name = "cboModule";
            this.cboModule.Size = new System.Drawing.Size(56, 29);
            this.cboModule.Style = MetroFramework.MetroColorStyle.Green;
            this.cboModule.TabIndex = 19;
            this.cboModule.UseSelectable = true;
            // 
            // pnlGeneratedCode
            // 
            this.pnlGeneratedCode.Controls.Add(this.grdResourceInfo);
            this.pnlGeneratedCode.Location = new System.Drawing.Point(966, 348);
            this.pnlGeneratedCode.Name = "pnlGeneratedCode";
            this.pnlGeneratedCode.Size = new System.Drawing.Size(497, 50);
            this.pnlGeneratedCode.TabIndex = 51;
            // 
            // pnlEntities
            // 
            this.pnlEntities.Controls.Add(this.splitEntities);
            this.pnlEntities.Location = new System.Drawing.Point(951, 101);
            this.pnlEntities.Name = "pnlEntities";
            this.pnlEntities.Size = new System.Drawing.Size(623, 49);
            this.pnlEntities.TabIndex = 45;
            // 
            // splitEntities
            // 
            this.splitEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitEntities.IsSplitterFixed = true;
            this.splitEntities.Location = new System.Drawing.Point(0, 0);
            this.splitEntities.Name = "splitEntities";
            this.splitEntities.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitEntities.Panel1
            // 
            this.splitEntities.Panel1.Controls.Add(this.pnlEntityTree);
            // 
            // splitEntities.Panel2
            // 
            this.splitEntities.Panel2.Controls.Add(this.tabEntity);
            this.splitEntities.Size = new System.Drawing.Size(623, 49);
            this.splitEntities.SplitterDistance = 25;
            this.splitEntities.TabIndex = 20;
            // 
            // pnlEntityTree
            // 
            this.pnlEntityTree.Controls.Add(this.pnlEntityGrid);
            this.pnlEntityTree.Controls.Add(this.pnlEntitiesLabel);
            this.pnlEntityTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEntityTree.Location = new System.Drawing.Point(0, 0);
            this.pnlEntityTree.Name = "pnlEntityTree";
            this.pnlEntityTree.Size = new System.Drawing.Size(623, 25);
            this.pnlEntityTree.TabIndex = 0;
            // 
            // pnlEntityGrid
            // 
            this.pnlEntityGrid.Controls.Add(this.treeEntities);
            this.pnlEntityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEntityGrid.Location = new System.Drawing.Point(0, 26);
            this.pnlEntityGrid.Name = "pnlEntityGrid";
            this.pnlEntityGrid.Size = new System.Drawing.Size(623, 0);
            this.pnlEntityGrid.TabIndex = 3;
            // 
            // treeEntities
            // 
            this.treeEntities.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeEntities.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeEntities.Location = new System.Drawing.Point(0, 0);
            this.treeEntities.Name = "treeEntities";
            this.treeEntities.Size = new System.Drawing.Size(623, 0);
            this.treeEntities.TabIndex = 0;
            this.treeEntities.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeEntities_NodeMouseClick);
            this.treeEntities.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeEntities_NodeMouseDoubleClick);
            // 
            // pnlEntitiesLabel
            // 
            this.pnlEntitiesLabel.Controls.Add(this.lblEntities);
            this.pnlEntitiesLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEntitiesLabel.Location = new System.Drawing.Point(0, 0);
            this.pnlEntitiesLabel.Name = "pnlEntitiesLabel";
            this.pnlEntitiesLabel.Size = new System.Drawing.Size(623, 26);
            this.pnlEntitiesLabel.TabIndex = 2;
            // 
            // lblEntities
            // 
            this.lblEntities.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEntities.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEntities.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblEntities.Location = new System.Drawing.Point(0, 0);
            this.lblEntities.Name = "lblEntities";
            this.lblEntities.Size = new System.Drawing.Size(623, 26);
            this.lblEntities.TabIndex = 1;
            this.lblEntities.Text = "Right-Click on entities or entity to Add, Edit or Delete";
            // 
            // tabEntity
            // 
            this.tabEntity.Controls.Add(this.tabPage1);
            this.tabEntity.Controls.Add(this.tabPage2);
            this.tabEntity.Controls.Add(this.tabPage3);
            this.tabEntity.Controls.Add(this.tabPage4);
            this.tabEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEntity.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
            this.tabEntity.Location = new System.Drawing.Point(0, 0);
            this.tabEntity.Name = "tabEntity";
            this.tabEntity.SelectedIndex = 1;
            this.tabEntity.Size = new System.Drawing.Size(623, 25);
            this.tabEntity.Style = MetroFramework.MetroColorStyle.Green;
            this.tabEntity.TabIndex = 1;
            this.tabEntity.UseSelectable = true;
            this.tabEntity.Click += new System.EventHandler(this.tabEntity_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtViewID);
            this.tabPage1.Controls.Add(this.lblViewID);
            this.tabPage1.Controls.Add(this.txtReportIniFile);
            this.tabPage1.Controls.Add(this.txtEntityName);
            this.tabPage1.Controls.Add(this.lblReportKeys);
            this.tabPage1.Controls.Add(this.cboReportKeys);
            this.tabPage1.Controls.Add(this.txtResxName);
            this.tabPage1.Controls.Add(this.lblReportIniFile);
            this.tabPage1.Controls.Add(this.txtReportProgramId);
            this.tabPage1.Controls.Add(this.lblResxName);
            this.tabPage1.Controls.Add(this.lblReportProgramId);
            this.tabPage1.Controls.Add(this.txtModelName);
            this.tabPage1.Controls.Add(this.lblModelName);
            this.tabPage1.Controls.Add(this.lblEntityName);
            this.tabPage1.HorizontalScrollbarBarColor = true;
            this.tabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPage1.HorizontalScrollbarSize = 10;
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(615, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Entity";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.VerticalScrollbarBarColor = true;
            this.tabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.tabPage1.VerticalScrollbarSize = 10;
            // 
            // txtReportIniFile
            // 
            // 
            // 
            // 
            this.txtReportIniFile.CustomButton.Image = null;
            this.txtReportIniFile.CustomButton.Location = new System.Drawing.Point(281, 1);
            this.txtReportIniFile.CustomButton.Name = "";
            this.txtReportIniFile.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtReportIniFile.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtReportIniFile.CustomButton.TabIndex = 1;
            this.txtReportIniFile.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtReportIniFile.CustomButton.UseSelectable = true;
            this.txtReportIniFile.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtReportIniFile.Lines = new string[0];
            this.txtReportIniFile.Location = new System.Drawing.Point(83, 45);
            this.txtReportIniFile.MaxLength = 32767;
            this.txtReportIniFile.Name = "txtReportIniFile";
            this.txtReportIniFile.PasswordChar = '\0';
            this.txtReportIniFile.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtReportIniFile.SelectedText = "";
            this.txtReportIniFile.SelectionLength = 0;
            this.txtReportIniFile.SelectionStart = 0;
            this.txtReportIniFile.ShortcutsEnabled = true;
            this.txtReportIniFile.ShowButton = true;
            this.txtReportIniFile.ShowClearButton = true;
            this.txtReportIniFile.Size = new System.Drawing.Size(305, 25);
            this.txtReportIniFile.Style = MetroFramework.MetroColorStyle.Green;
            this.txtReportIniFile.TabIndex = 5;
            this.txtReportIniFile.UseSelectable = true;
            this.txtReportIniFile.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtReportIniFile.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportIniFile.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.btnIniDialog_Click);
            // 
            // txtEntityName
            // 
            // 
            // 
            // 
            this.txtEntityName.CustomButton.Image = null;
            this.txtEntityName.CustomButton.Location = new System.Drawing.Point(185, 1);
            this.txtEntityName.CustomButton.Name = "";
            this.txtEntityName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtEntityName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtEntityName.CustomButton.TabIndex = 1;
            this.txtEntityName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtEntityName.CustomButton.UseSelectable = true;
            this.txtEntityName.CustomButton.Visible = false;
            this.txtEntityName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtEntityName.Lines = new string[0];
            this.txtEntityName.Location = new System.Drawing.Point(534, 45);
            this.txtEntityName.MaxLength = 32767;
            this.txtEntityName.Name = "txtEntityName";
            this.txtEntityName.PasswordChar = '\0';
            this.txtEntityName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtEntityName.SelectedText = "";
            this.txtEntityName.SelectionLength = 0;
            this.txtEntityName.SelectionStart = 0;
            this.txtEntityName.ShortcutsEnabled = true;
            this.txtEntityName.Size = new System.Drawing.Size(209, 25);
            this.txtEntityName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtEntityName.TabIndex = 12;
            this.txtEntityName.UseSelectable = true;
            this.txtEntityName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtEntityName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtEntityName.Leave += new System.EventHandler(this.txtEntityName_Leave);
            // 
            // lblReportKeys
            // 
            this.lblReportKeys.AutoSize = true;
            this.lblReportKeys.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblReportKeys.Location = new System.Drawing.Point(19, 79);
            this.lblReportKeys.Name = "lblReportKeys";
            this.lblReportKeys.Size = new System.Drawing.Size(59, 19);
            this.lblReportKeys.TabIndex = 7;
            this.lblReportKeys.Text = "Reports:";
            // 
            // cboReportKeys
            // 
            this.cboReportKeys.FormattingEnabled = true;
            this.cboReportKeys.ItemHeight = 23;
            this.cboReportKeys.Location = new System.Drawing.Point(83, 77);
            this.cboReportKeys.Name = "cboReportKeys";
            this.cboReportKeys.Size = new System.Drawing.Size(189, 29);
            this.cboReportKeys.Style = MetroFramework.MetroColorStyle.Green;
            this.cboReportKeys.TabIndex = 8;
            this.cboReportKeys.UseSelectable = true;
            this.cboReportKeys.SelectedIndexChanged += new System.EventHandler(this.cboReportKeys_SelectedIndexChanged);
            // 
            // txtResxName
            // 
            // 
            // 
            // 
            this.txtResxName.CustomButton.Image = null;
            this.txtResxName.CustomButton.Location = new System.Drawing.Point(185, 1);
            this.txtResxName.CustomButton.Name = "";
            this.txtResxName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtResxName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtResxName.CustomButton.TabIndex = 1;
            this.txtResxName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtResxName.CustomButton.UseSelectable = true;
            this.txtResxName.CustomButton.Visible = false;
            this.txtResxName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtResxName.Lines = new string[0];
            this.txtResxName.Location = new System.Drawing.Point(534, 111);
            this.txtResxName.MaxLength = 32767;
            this.txtResxName.Name = "txtResxName";
            this.txtResxName.PasswordChar = '\0';
            this.txtResxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtResxName.SelectedText = "";
            this.txtResxName.SelectionLength = 0;
            this.txtResxName.SelectionStart = 0;
            this.txtResxName.ShortcutsEnabled = true;
            this.txtResxName.Size = new System.Drawing.Size(209, 25);
            this.txtResxName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtResxName.TabIndex = 16;
            this.txtResxName.UseSelectable = true;
            this.txtResxName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtResxName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblReportIniFile
            // 
            this.lblReportIniFile.AutoSize = true;
            this.lblReportIniFile.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblReportIniFile.Location = new System.Drawing.Point(27, 47);
            this.lblReportIniFile.Name = "lblReportIniFile";
            this.lblReportIniFile.Size = new System.Drawing.Size(51, 19);
            this.lblReportIniFile.TabIndex = 4;
            this.lblReportIniFile.Text = "Ini File:";
            // 
            // txtReportProgramId
            // 
            this.txtReportProgramId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtReportProgramId.CustomButton.Image = null;
            this.txtReportProgramId.CustomButton.Location = new System.Drawing.Point(185, 1);
            this.txtReportProgramId.CustomButton.Name = "";
            this.txtReportProgramId.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtReportProgramId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtReportProgramId.CustomButton.TabIndex = 1;
            this.txtReportProgramId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtReportProgramId.CustomButton.UseSelectable = true;
            this.txtReportProgramId.CustomButton.Visible = false;
            this.txtReportProgramId.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtReportProgramId.Lines = new string[0];
            this.txtReportProgramId.Location = new System.Drawing.Point(83, 111);
            this.txtReportProgramId.MaxLength = 32767;
            this.txtReportProgramId.Name = "txtReportProgramId";
            this.txtReportProgramId.PasswordChar = '\0';
            this.txtReportProgramId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtReportProgramId.SelectedText = "";
            this.txtReportProgramId.SelectionLength = 0;
            this.txtReportProgramId.SelectionStart = 0;
            this.txtReportProgramId.ShortcutsEnabled = true;
            this.txtReportProgramId.Size = new System.Drawing.Size(209, 25);
            this.txtReportProgramId.Style = MetroFramework.MetroColorStyle.Green;
            this.txtReportProgramId.TabIndex = 10;
            this.txtReportProgramId.UseSelectable = true;
            this.txtReportProgramId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtReportProgramId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtReportProgramId.Leave += new System.EventHandler(this.txtReportProgramId_Leave);
            // 
            // lblResxName
            // 
            this.lblResxName.AutoSize = true;
            this.lblResxName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblResxName.Location = new System.Drawing.Point(448, 111);
            this.lblResxName.Name = "lblResxName";
            this.lblResxName.Size = new System.Drawing.Size(79, 19);
            this.lblResxName.TabIndex = 15;
            this.lblResxName.Text = "Resx Name:";
            // 
            // lblReportProgramId
            // 
            this.lblReportProgramId.AutoSize = true;
            this.lblReportProgramId.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblReportProgramId.Location = new System.Drawing.Point(-5, 111);
            this.lblReportProgramId.Name = "lblReportProgramId";
            this.lblReportProgramId.Size = new System.Drawing.Size(83, 19);
            this.lblReportProgramId.TabIndex = 9;
            this.lblReportProgramId.Text = "Program ID:";
            // 
            // txtModelName
            // 
            // 
            // 
            // 
            this.txtModelName.CustomButton.Image = null;
            this.txtModelName.CustomButton.Location = new System.Drawing.Point(185, 1);
            this.txtModelName.CustomButton.Name = "";
            this.txtModelName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtModelName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtModelName.CustomButton.TabIndex = 1;
            this.txtModelName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtModelName.CustomButton.UseSelectable = true;
            this.txtModelName.CustomButton.Visible = false;
            this.txtModelName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtModelName.Lines = new string[0];
            this.txtModelName.Location = new System.Drawing.Point(534, 79);
            this.txtModelName.MaxLength = 32767;
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.PasswordChar = '\0';
            this.txtModelName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtModelName.SelectedText = "";
            this.txtModelName.SelectionLength = 0;
            this.txtModelName.SelectionStart = 0;
            this.txtModelName.ShortcutsEnabled = true;
            this.txtModelName.Size = new System.Drawing.Size(209, 25);
            this.txtModelName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtModelName.TabIndex = 14;
            this.txtModelName.UseSelectable = true;
            this.txtModelName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtModelName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtModelName.Leave += new System.EventHandler(this.txtModelName_Leave);
            // 
            // lblModelName
            // 
            this.lblModelName.AutoSize = true;
            this.lblModelName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblModelName.Location = new System.Drawing.Point(436, 80);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(91, 19);
            this.lblModelName.TabIndex = 13;
            this.lblModelName.Text = "Model Name:";
            // 
            // lblEntityName
            // 
            this.lblEntityName.AutoSize = true;
            this.lblEntityName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEntityName.Location = new System.Drawing.Point(440, 46);
            this.lblEntityName.Name = "lblEntityName";
            this.lblEntityName.Size = new System.Drawing.Size(87, 19);
            this.lblEntityName.TabIndex = 11;
            this.lblEntityName.Text = "Entity Name:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkGenerateGridModel);
            this.tabPage2.Controls.Add(this.chkGenerateEnumerationsInSingleFile);
            this.tabPage2.Controls.Add(this.chkSequenceRevisionList);
            this.tabPage2.Controls.Add(this.chkGenerateIfExist);
            this.tabPage2.Controls.Add(this.chkGenerateClientFiles);
            this.tabPage2.Controls.Add(this.chkGenerateFinder);
            this.tabPage2.Controls.Add(this.chkGenerateDynamicEnablement);
            this.tabPage2.HorizontalScrollbarBarColor = true;
            this.tabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPage2.HorizontalScrollbarSize = 10;
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(615, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.VerticalScrollbarBarColor = true;
            this.tabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.tabPage2.VerticalScrollbarSize = 10;
            // 
            // chkGenerateGridModel
            // 
            this.chkGenerateGridModel.AutoSize = true;
            this.chkGenerateGridModel.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateGridModel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkGenerateGridModel.Location = new System.Drawing.Point(448, 44);
            this.chkGenerateGridModel.Name = "chkGenerateGridModel";
            this.chkGenerateGridModel.Size = new System.Drawing.Size(154, 19);
            this.chkGenerateGridModel.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateGridModel.TabIndex = 28;
            this.chkGenerateGridModel.Text = "Generate Grid Model";
            this.chkGenerateGridModel.UseCustomForeColor = true;
            this.chkGenerateGridModel.UseSelectable = true;
            this.chkGenerateGridModel.UseStyleColors = true;
            // 
            // chkGenerateEnumerationsInSingleFile
            // 
            this.chkGenerateEnumerationsInSingleFile.AutoSize = true;
            this.chkGenerateEnumerationsInSingleFile.Enabled = false;
            this.chkGenerateEnumerationsInSingleFile.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateEnumerationsInSingleFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkGenerateEnumerationsInSingleFile.Location = new System.Drawing.Point(15, 111);
            this.chkGenerateEnumerationsInSingleFile.Name = "chkGenerateEnumerationsInSingleFile";
            this.chkGenerateEnumerationsInSingleFile.Size = new System.Drawing.Size(248, 19);
            this.chkGenerateEnumerationsInSingleFile.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateEnumerationsInSingleFile.TabIndex = 25;
            this.chkGenerateEnumerationsInSingleFile.Text = "Generate Enumerations in Single File";
            this.chkGenerateEnumerationsInSingleFile.UseCustomForeColor = true;
            this.chkGenerateEnumerationsInSingleFile.UseSelectable = true;
            this.chkGenerateEnumerationsInSingleFile.UseStyleColors = true;
            // 
            // chkSequenceRevisionList
            // 
            this.chkSequenceRevisionList.AutoSize = true;
            this.chkSequenceRevisionList.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkSequenceRevisionList.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSequenceRevisionList.Location = new System.Drawing.Point(428, 65);
            this.chkSequenceRevisionList.Name = "chkSequenceRevisionList";
            this.chkSequenceRevisionList.Size = new System.Drawing.Size(162, 19);
            this.chkSequenceRevisionList.Style = MetroFramework.MetroColorStyle.Green;
            this.chkSequenceRevisionList.TabIndex = 27;
            this.chkSequenceRevisionList.Text = "Sequence Revision List";
            this.chkSequenceRevisionList.UseCustomForeColor = true;
            this.chkSequenceRevisionList.UseSelectable = true;
            this.chkSequenceRevisionList.UseStyleColors = true;
            // 
            // chkGenerateIfExist
            // 
            this.chkGenerateIfExist.AutoSize = true;
            this.chkGenerateIfExist.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateIfExist.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkGenerateIfExist.Location = new System.Drawing.Point(15, 88);
            this.chkGenerateIfExist.Name = "chkGenerateIfExist";
            this.chkGenerateIfExist.Size = new System.Drawing.Size(203, 19);
            this.chkGenerateIfExist.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateIfExist.TabIndex = 24;
            this.chkGenerateIfExist.Text = "Generate if Files Already Exist";
            this.chkGenerateIfExist.UseCustomForeColor = true;
            this.chkGenerateIfExist.UseSelectable = true;
            this.chkGenerateIfExist.UseStyleColors = true;
            // 
            // chkGenerateClientFiles
            // 
            this.chkGenerateClientFiles.AutoSize = true;
            this.chkGenerateClientFiles.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateClientFiles.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkGenerateClientFiles.Location = new System.Drawing.Point(15, 65);
            this.chkGenerateClientFiles.Name = "chkGenerateClientFiles";
            this.chkGenerateClientFiles.Size = new System.Drawing.Size(376, 19);
            this.chkGenerateClientFiles.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateClientFiles.TabIndex = 23;
            this.chkGenerateClientFiles.Text = "Generate Client Files (Controllers, Razor Views, JavaScript)";
            this.chkGenerateClientFiles.UseCustomForeColor = true;
            this.chkGenerateClientFiles.UseSelectable = true;
            this.chkGenerateClientFiles.UseStyleColors = true;
            // 
            // chkGenerateFinder
            // 
            this.chkGenerateFinder.AutoSize = true;
            this.chkGenerateFinder.Checked = true;
            this.chkGenerateFinder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateFinder.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateFinder.Location = new System.Drawing.Point(15, 19);
            this.chkGenerateFinder.Name = "chkGenerateFinder";
            this.chkGenerateFinder.Size = new System.Drawing.Size(123, 19);
            this.chkGenerateFinder.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateFinder.TabIndex = 21;
            this.chkGenerateFinder.Text = "Generate Finder";
            this.chkGenerateFinder.UseSelectable = true;
            // 
            // chkGenerateDynamicEnablement
            // 
            this.chkGenerateDynamicEnablement.AutoSize = true;
            this.chkGenerateDynamicEnablement.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkGenerateDynamicEnablement.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkGenerateDynamicEnablement.Location = new System.Drawing.Point(15, 42);
            this.chkGenerateDynamicEnablement.Name = "chkGenerateDynamicEnablement";
            this.chkGenerateDynamicEnablement.Size = new System.Drawing.Size(214, 19);
            this.chkGenerateDynamicEnablement.Style = MetroFramework.MetroColorStyle.Green;
            this.chkGenerateDynamicEnablement.TabIndex = 22;
            this.chkGenerateDynamicEnablement.Text = "Generate Dynamic Enablement";
            this.chkGenerateDynamicEnablement.UseCustomForeColor = true;
            this.chkGenerateDynamicEnablement.UseSelectable = true;
            this.chkGenerateDynamicEnablement.UseStyleColors = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pnlColumns);
            this.tabPage3.HorizontalScrollbarBarColor = true;
            this.tabPage3.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPage3.HorizontalScrollbarSize = 10;
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(615, 0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Properties";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.VerticalScrollbarBarColor = true;
            this.tabPage3.VerticalScrollbarHighlightOnWheel = false;
            this.tabPage3.VerticalScrollbarSize = 10;
            // 
            // pnlColumns
            // 
            this.pnlColumns.Controls.Add(this.grdEntityFields);
            this.pnlColumns.Controls.Add(this.tbrEntity);
            this.pnlColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColumns.Location = new System.Drawing.Point(0, 0);
            this.pnlColumns.Name = "pnlColumns";
            this.pnlColumns.Size = new System.Drawing.Size(615, 0);
            this.pnlColumns.TabIndex = 19;
            // 
            // grdEntityFields
            // 
            this.grdEntityFields.AllowUserToAddRows = false;
            this.grdEntityFields.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdEntityFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEntityFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEntityFields.Location = new System.Drawing.Point(0, 27);
            this.grdEntityFields.Name = "grdEntityFields";
            this.grdEntityFields.Size = new System.Drawing.Size(615, 0);
            this.grdEntityFields.TabIndex = 18;
            // 
            // tbrEntity
            // 
            this.tbrEntity.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tbrEntity.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRowAdd,
            this.btnDeleteRow,
            this.btnDeleteRows});
            this.tbrEntity.Location = new System.Drawing.Point(0, 0);
            this.tbrEntity.Name = "tbrEntity";
            this.tbrEntity.Size = new System.Drawing.Size(615, 27);
            this.tbrEntity.TabIndex = 17;
            this.tbrEntity.Text = "toolStrip1";
            // 
            // btnRowAdd
            // 
            this.btnRowAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRowAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnRowAdd.Image")));
            this.btnRowAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRowAdd.Name = "btnRowAdd";
            this.btnRowAdd.Size = new System.Drawing.Size(24, 24);
            this.btnRowAdd.Text = "Add Row";
            this.btnRowAdd.Click += new System.EventHandler(this.btnRowAdd_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteRow.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteRow.Image")));
            this.btnDeleteRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(24, 24);
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // btnDeleteRows
            // 
            this.btnDeleteRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteRows.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteRows.Image")));
            this.btnDeleteRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRows.Name = "btnDeleteRows";
            this.btnDeleteRows.Size = new System.Drawing.Size(24, 24);
            this.btnDeleteRows.Text = "Delete Rows";
            this.btnDeleteRows.Click += new System.EventHandler(this.btnDeleteRows_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pnlComposition);
            this.tabPage4.HorizontalScrollbarBarColor = true;
            this.tabPage4.HorizontalScrollbarHighlightOnWheel = false;
            this.tabPage4.HorizontalScrollbarSize = 10;
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(615, 0);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Composition";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.VerticalScrollbarBarColor = true;
            this.tabPage4.VerticalScrollbarHighlightOnWheel = false;
            this.tabPage4.VerticalScrollbarSize = 10;
            // 
            // pnlComposition
            // 
            this.pnlComposition.Controls.Add(this.grdEntityCompositions);
            this.pnlComposition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlComposition.Location = new System.Drawing.Point(0, 0);
            this.pnlComposition.Name = "pnlComposition";
            this.pnlComposition.Size = new System.Drawing.Size(615, 0);
            this.pnlComposition.TabIndex = 0;
            // 
            // grdEntityCompositions
            // 
            this.grdEntityCompositions.AllowUserToAddRows = false;
            this.grdEntityCompositions.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdEntityCompositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEntityCompositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEntityCompositions.Location = new System.Drawing.Point(0, 0);
            this.grdEntityCompositions.Name = "grdEntityCompositions";
            this.grdEntityCompositions.Size = new System.Drawing.Size(615, 0);
            this.grdEntityCompositions.TabIndex = 1;
            this.grdEntityCompositions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdEntityCompositions_CellFormatting);
            // 
            // pnlGenerateCode
            // 
            this.pnlGenerateCode.Controls.Add(this.txtLayoutToGenerate);
            this.pnlGenerateCode.Controls.Add(this.txtEntitiesToGenerate);
            this.pnlGenerateCode.Controls.Add(this.lblGenerateHelp);
            this.pnlGenerateCode.Location = new System.Drawing.Point(972, 236);
            this.pnlGenerateCode.Name = "pnlGenerateCode";
            this.pnlGenerateCode.Size = new System.Drawing.Size(468, 64);
            this.pnlGenerateCode.TabIndex = 50;
            // 
            // txtLayoutToGenerate
            // 
            this.txtLayoutToGenerate.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtLayoutToGenerate.CustomButton.Image = null;
            this.txtLayoutToGenerate.CustomButton.Location = new System.Drawing.Point(304, 2);
            this.txtLayoutToGenerate.CustomButton.Name = "";
            this.txtLayoutToGenerate.CustomButton.Size = new System.Drawing.Size(161, 161);
            this.txtLayoutToGenerate.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtLayoutToGenerate.CustomButton.TabIndex = 1;
            this.txtLayoutToGenerate.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtLayoutToGenerate.CustomButton.UseSelectable = true;
            this.txtLayoutToGenerate.CustomButton.Visible = false;
            this.txtLayoutToGenerate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLayoutToGenerate.Lines = new string[0];
            this.txtLayoutToGenerate.Location = new System.Drawing.Point(0, -102);
            this.txtLayoutToGenerate.MaxLength = 32767;
            this.txtLayoutToGenerate.Multiline = true;
            this.txtLayoutToGenerate.Name = "txtLayoutToGenerate";
            this.txtLayoutToGenerate.PasswordChar = '\0';
            this.txtLayoutToGenerate.ReadOnly = true;
            this.txtLayoutToGenerate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLayoutToGenerate.SelectedText = "";
            this.txtLayoutToGenerate.SelectionLength = 0;
            this.txtLayoutToGenerate.SelectionStart = 0;
            this.txtLayoutToGenerate.ShortcutsEnabled = true;
            this.txtLayoutToGenerate.Size = new System.Drawing.Size(468, 166);
            this.txtLayoutToGenerate.TabIndex = 3;
            this.txtLayoutToGenerate.UseSelectable = true;
            this.txtLayoutToGenerate.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtLayoutToGenerate.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtEntitiesToGenerate
            // 
            this.txtEntitiesToGenerate.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.txtEntitiesToGenerate.CustomButton.Image = null;
            this.txtEntitiesToGenerate.CustomButton.Location = new System.Drawing.Point(442, 2);
            this.txtEntitiesToGenerate.CustomButton.Name = "";
            this.txtEntitiesToGenerate.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtEntitiesToGenerate.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtEntitiesToGenerate.CustomButton.TabIndex = 1;
            this.txtEntitiesToGenerate.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtEntitiesToGenerate.CustomButton.UseSelectable = true;
            this.txtEntitiesToGenerate.CustomButton.Visible = false;
            this.txtEntitiesToGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEntitiesToGenerate.Lines = new string[0];
            this.txtEntitiesToGenerate.Location = new System.Drawing.Point(0, 36);
            this.txtEntitiesToGenerate.MaxLength = 32767;
            this.txtEntitiesToGenerate.Multiline = true;
            this.txtEntitiesToGenerate.Name = "txtEntitiesToGenerate";
            this.txtEntitiesToGenerate.PasswordChar = '\0';
            this.txtEntitiesToGenerate.ReadOnly = true;
            this.txtEntitiesToGenerate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEntitiesToGenerate.SelectedText = "";
            this.txtEntitiesToGenerate.SelectionLength = 0;
            this.txtEntitiesToGenerate.SelectionStart = 0;
            this.txtEntitiesToGenerate.ShortcutsEnabled = true;
            this.txtEntitiesToGenerate.Size = new System.Drawing.Size(468, 28);
            this.txtEntitiesToGenerate.TabIndex = 2;
            this.txtEntitiesToGenerate.UseSelectable = true;
            this.txtEntitiesToGenerate.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtEntitiesToGenerate.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblGenerateHelp
            // 
            this.lblGenerateHelp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGenerateHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblGenerateHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGenerateHelp.Location = new System.Drawing.Point(0, 0);
            this.lblGenerateHelp.Name = "lblGenerateHelp";
            this.lblGenerateHelp.Size = new System.Drawing.Size(468, 36);
            this.lblGenerateHelp.TabIndex = 0;
            this.lblGenerateHelp.Text = "The entities will be generated or referenced based upon the following content:";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.lblLowerBorder);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(997, 79);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnCancel.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnCancel.Highlight = true;
            this.btnCancel.Location = new System.Drawing.Point(776, 50);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.Style = MetroFramework.MetroColorStyle.Green;
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLowerBorder
            // 
            this.lblLowerBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblLowerBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLowerBorder.Location = new System.Drawing.Point(0, 0);
            this.lblLowerBorder.Name = "lblLowerBorder";
            this.lblLowerBorder.Size = new System.Drawing.Size(997, 1);
            this.lblLowerBorder.TabIndex = 52;
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblProcessingFile.Location = new System.Drawing.Point(17, 10);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 0);
            this.lblProcessingFile.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnSave.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSave.Highlight = true;
            this.btnSave.Location = new System.Drawing.Point(702, 50);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 25);
            this.btnSave.Style = MetroFramework.MetroColorStyle.Green;
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "Save";
            this.btnSave.UseSelectable = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(924, 50);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.Style = MetroFramework.MetroColorStyle.Green;
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Next";
            this.btnNext.UseSelectable = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnBack.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnBack.Highlight = true;
            this.btnBack.Location = new System.Drawing.Point(850, 50);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 13;
            this.btnBack.Text = "Back";
            this.btnBack.UseSelectable = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // Generation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(1037, 685);
            this.Controls.Add(this.splitBase);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Generation";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Code Generation Wizard";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Generation_HelpButtonClicked);
            ((System.ComponentModel.ISupportInitialize)(this.grdResourceInfo)).EndInit();
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
            this.pnlUIGeneration.ResumeLayout(false);
            this.splitDesigner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDesigner)).EndInit();
            this.splitDesigner.ResumeLayout(false);
            this.grpContainers.ResumeLayout(false);
            this.grpContainers.PerformLayout();
            this.tabUI.ResumeLayout(false);
            this.tabPageInfo.ResumeLayout(false);
            this.tabPageInfo.PerformLayout();
            this.tabPageFinder.ResumeLayout(false);
            this.pnlFinder.ResumeLayout(false);
            this.pnlFinder.PerformLayout();
            this.tbrProperties.ResumeLayout(false);
            this.tbrProperties.PerformLayout();
            this.pnlCodeType.ResumeLayout(false);
            this.pnlCodeType.PerformLayout();
            this.grpCredentials.ResumeLayout(false);
            this.grpCredentials.PerformLayout();
            this.pnlGeneratedCode.ResumeLayout(false);
            this.pnlEntities.ResumeLayout(false);
            this.splitEntities.Panel1.ResumeLayout(false);
            this.splitEntities.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitEntities)).EndInit();
            this.splitEntities.ResumeLayout(false);
            this.pnlEntityTree.ResumeLayout(false);
            this.pnlEntityGrid.ResumeLayout(false);
            this.pnlEntitiesLabel.ResumeLayout(false);
            this.tabEntity.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.pnlColumns.ResumeLayout(false);
            this.pnlColumns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEntityFields)).EndInit();
            this.tbrEntity.ResumeLayout(false);
            this.tbrEntity.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.pnlComposition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEntityCompositions)).EndInit();
            this.pnlGenerateCode.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdResourceInfo;
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private MetroFramework.Controls.MetroTextBox txtViewID;
        private MetroFramework.Controls.MetroLabel lblViewID;
        private MetroFramework.Controls.MetroTextBox txtCompany;
        private MetroFramework.Controls.MetroTextBox txtVersion;
        private MetroFramework.Controls.MetroLabel lblPassword;
        private MetroFramework.Controls.MetroLabel lblUser;
        private MetroFramework.Controls.MetroLabel lblVersion;
        private MetroFramework.Controls.MetroTextBox txtPassword;
        private MetroFramework.Controls.MetroLabel lblCompany;
        private MetroFramework.Controls.MetroTextBox txtUser;
        private MetroFramework.Controls.MetroComboBox cboRepositoryType;
        private MetroFramework.Controls.MetroLabel lblRepositoryType;
        private System.Windows.Forms.SplitContainer splitBase;
        private MetroFramework.Controls.MetroCheckBox chkGenerateFinder;
        private MetroFramework.Controls.MetroCheckBox chkGenerateDynamicEnablement;
        private MetroFramework.Controls.MetroTextBox txtResxName;
        private MetroFramework.Controls.MetroLabel lblResxName;
        private System.Windows.Forms.Panel pnlEntities;
        private System.Windows.Forms.Panel pnlCodeType;
        private MetroFramework.Controls.MetroLabel lblCodeTypeDescriptionHelp;
        private MetroFramework.Controls.MetroTextBox txtModelName;
        private MetroFramework.Controls.MetroLabel lblModelName;
        private System.Windows.Forms.DataGridView grdEntityFields;
        private System.Windows.Forms.ToolStrip tbrEntity;
        private MetroFramework.Controls.MetroTextBox txtReportProgramId;
        private MetroFramework.Controls.MetroComboBox cboReportKeys;
        private MetroFramework.Controls.MetroTextBox txtReportIniFile;
        private MetroFramework.Controls.MetroLabel lblReportProgramId;
        private MetroFramework.Controls.MetroLabel lblReportKeys;
        private MetroFramework.Controls.MetroLabel lblReportIniFile;
        private MetroFramework.Controls.MetroLabel lblStepDescription;
        private MetroFramework.Controls.MetroLabel lblStepTitle;
        private System.Windows.Forms.Panel pnlGeneratedCode;
        private System.Windows.Forms.Panel pnlGenerateCode;
        private MetroFramework.Controls.MetroLabel lblCodeTypeFilesHelp;
        private System.Windows.Forms.GroupBox grpCredentials;
        private System.Windows.Forms.ToolStripButton btnRowAdd;
        private System.Windows.Forms.ToolStripButton btnDeleteRow;
        private System.Windows.Forms.ToolStripButton btnDeleteRows;
        private MetroFramework.Controls.MetroLabel lblGenerateHelp;
        private MetroFramework.Controls.MetroComboBox cboModule;
        private MetroFramework.Controls.MetroLabel lblModule;
        private MetroFramework.Controls.MetroLabel lblProcessingFile;
        private MetroFramework.Controls.MetroTextBox txtEntityName;
        private MetroFramework.Controls.MetroLabel lblEntityName;
        private MetroFramework.Controls.MetroLabel lblUnknownCodeTypeFilesHelp;
        private System.Windows.Forms.SplitContainer splitSteps;
        private MetroFramework.Controls.MetroButton btnNext;
        private MetroFramework.Controls.MetroButton btnBack;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.Panel pnlButtons;
        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Controls.MetroButton btnSave;
        private System.Windows.Forms.SplitContainer splitEntities;
        private System.Windows.Forms.Panel pnlEntityTree;
        private System.Windows.Forms.TreeView treeEntities;
        private MetroFramework.Controls.MetroTabControl tabEntity;
        private MetroFramework.Controls.MetroTabPage tabPage1;
        private MetroFramework.Controls.MetroTabPage tabPage2;
        private MetroFramework.Controls.MetroCheckBox chkGenerateIfExist;
        private MetroFramework.Controls.MetroCheckBox chkGenerateClientFiles;
        private MetroFramework.Controls.MetroTextBox txtEntitiesToGenerate;
        private System.Windows.Forms.Panel pnlEntityGrid;
        private System.Windows.Forms.Panel pnlEntitiesLabel;
        private MetroFramework.Controls.MetroLabel lblEntities;
        private MetroFramework.Controls.MetroTabPage tabPage3;
        private System.Windows.Forms.Panel pnlColumns;
        private MetroFramework.Controls.MetroTabPage tabPage4;
        private System.Windows.Forms.Panel pnlComposition;
        private System.Windows.Forms.DataGridView grdEntityCompositions;
        private MetroFramework.Controls.MetroCheckBox chkSequenceRevisionList;
        private MetroFramework.Controls.MetroCheckBox chkGenerateEnumerationsInSingleFile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUpperBorder;
        private System.Windows.Forms.Label lblLowerBorder;
        private System.Windows.Forms.Panel pnlUIGeneration;
        private MetroFramework.Controls.MetroTextBox txtLayoutToGenerate;
        private System.Windows.Forms.SplitContainer splitDesigner;
        private System.Windows.Forms.TreeView treeUIEntities;
        private System.Windows.Forms.GroupBox grpContainers;
        private System.Windows.Forms.TextBox txtPropText;
        private System.Windows.Forms.ToolStrip tbrProperties;
        private System.Windows.Forms.ToolStripButton btnTab;
        private System.Windows.Forms.ToolStripButton btnGrid;
        private System.Windows.Forms.ToolStripButton btnDeleteControl;
        private System.Windows.Forms.ToolStripButton btnAddTabPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button btnFinderPropFile;
        private System.Windows.Forms.TextBox txtFinderPropFile;
        private System.Windows.Forms.Label lblFinderPropFile;
        private System.Windows.Forms.Label lblPropText;
        private System.Windows.Forms.ComboBox cboFinderDisplay;
        private System.Windows.Forms.Label lblFinderDisplay;
        private System.Windows.Forms.ComboBox cboFinderProp;
        private System.Windows.Forms.Label lblFinderProp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TabControl tabUI;
        private System.Windows.Forms.TabPage tabPageInfo;
        private System.Windows.Forms.TextBox txtPropWidget;
        private System.Windows.Forms.Label lblPropType;
        private System.Windows.Forms.TabPage tabPageFinder;
        private System.Windows.Forms.Panel pnlFinder;
        private MetroFramework.Controls.MetroCheckBox chkGenerateGridModel;
    }
}

