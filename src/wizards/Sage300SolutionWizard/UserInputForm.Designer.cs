namespace Sage.CA.SBS.ERP.Sage300.SolutionWizard
{
    partial class UserInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInputForm));
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblModuleId = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtApplicationID = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.lblNamespace = new System.Windows.Forms.Label();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.pnlGenerateSolution = new System.Windows.Forms.Panel();
            this.lblGenerateHelp = new System.Windows.Forms.Label();
            this.pnlResourceFiles = new System.Windows.Forms.Panel();
            this.chkFrench = new System.Windows.Forms.CheckBox();
            this.chkSpanish = new System.Windows.Forms.CheckBox();
            this.chkChineseTraditional = new System.Windows.Forms.CheckBox();
            this.chkChineseSimplified = new System.Windows.Forms.CheckBox();
            this.chkEnglish = new System.Windows.Forms.CheckBox();
            this.pnlKendo = new System.Windows.Forms.Panel();
            this.lblKendoVersionHelp = new System.Windows.Forms.Label();
            this.lblKendoLink = new System.Windows.Forms.LinkLabel();
            this.lblKendoFolderHelp = new System.Windows.Forms.Label();
            this.btnKendoDialog = new System.Windows.Forms.Button();
            this.txtKendoFolder = new System.Windows.Forms.TextBox();
            this.lblKendoFolder = new System.Windows.Forms.Label();
            this.chkKendoLicense = new System.Windows.Forms.CheckBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.pnlGenerateSolution.SuspendLayout();
            this.pnlResourceFiles.SuspendLayout();
            this.pnlKendo.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(5, 25);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(90, 13);
            this.lblCompanyName.TabIndex = 1;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblModuleId
            // 
            this.lblModuleId.AutoSize = true;
            this.lblModuleId.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModuleId.Location = new System.Drawing.Point(31, 51);
            this.lblModuleId.Name = "lblModuleId";
            this.lblModuleId.Size = new System.Drawing.Size(64, 13);
            this.lblModuleId.TabIndex = 2;
            this.lblModuleId.Text = "Module ID:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Location = new System.Drawing.Point(101, 22);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(238, 20);
            this.txtCompanyName.TabIndex = 1;
            this.txtCompanyName.TextChanged += new System.EventHandler(this.txtCompanyName_TextChanged);
            // 
            // txtApplicationID
            // 
            this.txtApplicationID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtApplicationID.Location = new System.Drawing.Point(101, 48);
            this.txtApplicationID.Name = "txtApplicationID";
            this.txtApplicationID.Size = new System.Drawing.Size(107, 20);
            this.txtApplicationID.TabIndex = 2;
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(101, 74);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(238, 20);
            this.txtNamespace.TabIndex = 3;
            // 
            // lblNamespace
            // 
            this.lblNamespace.AutoSize = true;
            this.lblNamespace.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNamespace.Location = new System.Drawing.Point(27, 77);
            this.lblNamespace.Name = "lblNamespace";
            this.lblNamespace.Size = new System.Drawing.Size(68, 13);
            this.lblNamespace.TabIndex = 6;
            this.lblNamespace.Text = "Namespace:";
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(0, 0);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(474, 346);
            this.splitBase.SplitterDistance = 66;
            this.splitBase.TabIndex = 7;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(450, 26);
            this.lblStepDescription.TabIndex = 5;
            this.lblStepDescription.Text = "This is the detailed description";
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.AutoSize = true;
            this.lblStepTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblStepTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepTitle.Location = new System.Drawing.Point(12, 9);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(206, 21);
            this.lblStepTitle.TabIndex = 4;
            this.lblStepTitle.Text = "This is the title of the step";
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
            this.splitSteps.Panel1.Controls.Add(this.pnlInfo);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerateSolution);
            this.splitSteps.Panel1.Controls.Add(this.pnlResourceFiles);
            this.splitSteps.Panel1.Controls.Add(this.pnlKendo);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(474, 276);
            this.splitSteps.SplitterDistance = 237;
            this.splitSteps.TabIndex = 5;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblCompanyName);
            this.pnlInfo.Controls.Add(this.txtNamespace);
            this.pnlInfo.Controls.Add(this.lblModuleId);
            this.pnlInfo.Controls.Add(this.lblNamespace);
            this.pnlInfo.Controls.Add(this.txtCompanyName);
            this.pnlInfo.Controls.Add(this.txtApplicationID);
            this.pnlInfo.Location = new System.Drawing.Point(408, 12);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(295, 33);
            this.pnlInfo.TabIndex = 0;
            // 
            // pnlGenerateSolution
            // 
            this.pnlGenerateSolution.Controls.Add(this.lblGenerateHelp);
            this.pnlGenerateSolution.Location = new System.Drawing.Point(408, 144);
            this.pnlGenerateSolution.Name = "pnlGenerateSolution";
            this.pnlGenerateSolution.Size = new System.Drawing.Size(288, 38);
            this.pnlGenerateSolution.TabIndex = 3;
            // 
            // lblGenerateHelp
            // 
            this.lblGenerateHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenerateHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGenerateHelp.Location = new System.Drawing.Point(84, 33);
            this.lblGenerateHelp.Name = "lblGenerateHelp";
            this.lblGenerateHelp.Size = new System.Drawing.Size(273, 111);
            this.lblGenerateHelp.TabIndex = 0;
            this.lblGenerateHelp.Text = "Select the \'Generate\' button below to generate the solution based upon the inform" +
    "ation entered and selected in the proceeding steps.";
            // 
            // pnlResourceFiles
            // 
            this.pnlResourceFiles.Controls.Add(this.chkFrench);
            this.pnlResourceFiles.Controls.Add(this.chkSpanish);
            this.pnlResourceFiles.Controls.Add(this.chkChineseTraditional);
            this.pnlResourceFiles.Controls.Add(this.chkChineseSimplified);
            this.pnlResourceFiles.Controls.Add(this.chkEnglish);
            this.pnlResourceFiles.Location = new System.Drawing.Point(408, 97);
            this.pnlResourceFiles.Name = "pnlResourceFiles";
            this.pnlResourceFiles.Size = new System.Drawing.Size(271, 25);
            this.pnlResourceFiles.TabIndex = 4;
            // 
            // chkFrench
            // 
            this.chkFrench.AutoSize = true;
            this.chkFrench.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFrench.Location = new System.Drawing.Point(16, 114);
            this.chkFrench.Name = "chkFrench";
            this.chkFrench.Size = new System.Drawing.Size(61, 17);
            this.chkFrench.TabIndex = 4;
            this.chkFrench.Text = "French";
            this.chkFrench.UseVisualStyleBackColor = true;
            // 
            // chkSpanish
            // 
            this.chkSpanish.AutoSize = true;
            this.chkSpanish.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSpanish.Location = new System.Drawing.Point(16, 91);
            this.chkSpanish.Name = "chkSpanish";
            this.chkSpanish.Size = new System.Drawing.Size(67, 17);
            this.chkSpanish.TabIndex = 3;
            this.chkSpanish.Text = "Spanish";
            this.chkSpanish.UseVisualStyleBackColor = true;
            // 
            // chkChineseTraditional
            // 
            this.chkChineseTraditional.AutoSize = true;
            this.chkChineseTraditional.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChineseTraditional.Location = new System.Drawing.Point(16, 68);
            this.chkChineseTraditional.Name = "chkChineseTraditional";
            this.chkChineseTraditional.Size = new System.Drawing.Size(124, 17);
            this.chkChineseTraditional.TabIndex = 2;
            this.chkChineseTraditional.Text = "Chinese Traditional";
            this.chkChineseTraditional.UseVisualStyleBackColor = true;
            // 
            // chkChineseSimplified
            // 
            this.chkChineseSimplified.AutoSize = true;
            this.chkChineseSimplified.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkChineseSimplified.Location = new System.Drawing.Point(16, 45);
            this.chkChineseSimplified.Name = "chkChineseSimplified";
            this.chkChineseSimplified.Size = new System.Drawing.Size(121, 17);
            this.chkChineseSimplified.TabIndex = 1;
            this.chkChineseSimplified.Text = "Chinese Simplified";
            this.chkChineseSimplified.UseVisualStyleBackColor = true;
            // 
            // chkEnglish
            // 
            this.chkEnglish.AutoSize = true;
            this.chkEnglish.Checked = true;
            this.chkEnglish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnglish.Enabled = false;
            this.chkEnglish.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnglish.Location = new System.Drawing.Point(16, 22);
            this.chkEnglish.Name = "chkEnglish";
            this.chkEnglish.Size = new System.Drawing.Size(64, 17);
            this.chkEnglish.TabIndex = 0;
            this.chkEnglish.Text = "English";
            this.chkEnglish.UseVisualStyleBackColor = true;
            // 
            // pnlKendo
            // 
            this.pnlKendo.Controls.Add(this.lblKendoVersionHelp);
            this.pnlKendo.Controls.Add(this.lblKendoLink);
            this.pnlKendo.Controls.Add(this.lblKendoFolderHelp);
            this.pnlKendo.Controls.Add(this.btnKendoDialog);
            this.pnlKendo.Controls.Add(this.txtKendoFolder);
            this.pnlKendo.Controls.Add(this.lblKendoFolder);
            this.pnlKendo.Controls.Add(this.chkKendoLicense);
            this.pnlKendo.Location = new System.Drawing.Point(408, 53);
            this.pnlKendo.Name = "pnlKendo";
            this.pnlKendo.Size = new System.Drawing.Size(233, 34);
            this.pnlKendo.TabIndex = 2;
            // 
            // lblKendoVersionHelp
            // 
            this.lblKendoVersionHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKendoVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoVersionHelp.Location = new System.Drawing.Point(109, 129);
            this.lblKendoVersionHelp.Name = "lblKendoVersionHelp";
            this.lblKendoVersionHelp.Size = new System.Drawing.Size(293, 36);
            this.lblKendoVersionHelp.TabIndex = 6;
            this.lblKendoVersionHelp.Text = "The Kendo UI version used in these projects is v2016.2.714";
            // 
            // lblKendoLink
            // 
            this.lblKendoLink.AutoSize = true;
            this.lblKendoLink.Location = new System.Drawing.Point(110, 104);
            this.lblKendoLink.Name = "lblKendoLink";
            this.lblKendoLink.Size = new System.Drawing.Size(348, 13);
            this.lblKendoLink.TabIndex = 5;
            this.lblKendoLink.TabStop = true;
            this.lblKendoLink.Text = "http://www.telerik.com/purchase/license-agreement/kendo-ui-complete";
            this.lblKendoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblKendoLink_LinkClicked);
            // 
            // lblKendoFolderHelp
            // 
            this.lblKendoFolderHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKendoFolderHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoFolderHelp.Location = new System.Drawing.Point(107, 77);
            this.lblKendoFolderHelp.Name = "lblKendoFolderHelp";
            this.lblKendoFolderHelp.Size = new System.Drawing.Size(297, 27);
            this.lblKendoFolderHelp.TabIndex = 4;
            this.lblKendoFolderHelp.Text = "The Kendo UI Commercial License may be obtained at:";
            // 
            // btnKendoDialog
            // 
            this.btnKendoDialog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnKendoDialog.BackgroundImage")));
            this.btnKendoDialog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKendoDialog.Enabled = false;
            this.btnKendoDialog.FlatAppearance.BorderSize = 0;
            this.btnKendoDialog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKendoDialog.Location = new System.Drawing.Point(419, 49);
            this.btnKendoDialog.Name = "btnKendoDialog";
            this.btnKendoDialog.Size = new System.Drawing.Size(27, 20);
            this.btnKendoDialog.TabIndex = 3;
            this.btnKendoDialog.UseVisualStyleBackColor = true;
            this.btnKendoDialog.Click += new System.EventHandler(this.btnKendoDialog_Click);
            // 
            // txtKendoFolder
            // 
            this.txtKendoFolder.Enabled = false;
            this.txtKendoFolder.Location = new System.Drawing.Point(110, 49);
            this.txtKendoFolder.Name = "txtKendoFolder";
            this.txtKendoFolder.Size = new System.Drawing.Size(303, 20);
            this.txtKendoFolder.TabIndex = 2;
            // 
            // lblKendoFolder
            // 
            this.lblKendoFolder.AutoSize = true;
            this.lblKendoFolder.Enabled = false;
            this.lblKendoFolder.Location = new System.Drawing.Point(35, 52);
            this.lblKendoFolder.Name = "lblKendoFolder";
            this.lblKendoFolder.Size = new System.Drawing.Size(73, 13);
            this.lblKendoFolder.TabIndex = 1;
            this.lblKendoFolder.Text = "Kendo Folder:";
            // 
            // chkKendoLicense
            // 
            this.chkKendoLicense.AutoSize = true;
            this.chkKendoLicense.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkKendoLicense.Location = new System.Drawing.Point(16, 21);
            this.chkKendoLicense.Name = "chkKendoLicense";
            this.chkKendoLicense.Size = new System.Drawing.Size(236, 17);
            this.chkKendoLicense.TabIndex = 0;
            this.chkKendoLicense.Text = "Purchased Kendo UI Commercial License?";
            this.chkKendoLicense.UseVisualStyleBackColor = true;
            this.chkKendoLicense.CheckedChanged += new System.EventHandler(this.chkKendoLicense_CheckedChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(474, 35);
            this.pnlButtons.TabIndex = 5;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(394, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(318, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 346);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solution Generation";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.UserInputForm_HelpButtonClicked);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserInputForm_FormClosed);
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            this.splitSteps.Panel1.ResumeLayout(false);
            this.splitSteps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).EndInit();
            this.splitSteps.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlGenerateSolution.ResumeLayout(false);
            this.pnlResourceFiles.ResumeLayout(false);
            this.pnlResourceFiles.PerformLayout();
            this.pnlKendo.ResumeLayout(false);
            this.pnlKendo.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblModuleId;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtApplicationID;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label lblNamespace;
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Panel pnlKendo;
        private System.Windows.Forms.Label lblKendoVersionHelp;
        private System.Windows.Forms.LinkLabel lblKendoLink;
        private System.Windows.Forms.Label lblKendoFolderHelp;
        private System.Windows.Forms.Button btnKendoDialog;
        private System.Windows.Forms.TextBox txtKendoFolder;
        private System.Windows.Forms.Label lblKendoFolder;
        private System.Windows.Forms.CheckBox chkKendoLicense;
        private System.Windows.Forms.Panel pnlGenerateSolution;
        private System.Windows.Forms.Label lblGenerateHelp;
        private System.Windows.Forms.Panel pnlResourceFiles;
        private System.Windows.Forms.CheckBox chkFrench;
        private System.Windows.Forms.CheckBox chkSpanish;
        private System.Windows.Forms.CheckBox chkChineseTraditional;
        private System.Windows.Forms.CheckBox chkChineseSimplified;
        private System.Windows.Forms.CheckBox chkEnglish;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.ToolTip tooltip;
    }
}