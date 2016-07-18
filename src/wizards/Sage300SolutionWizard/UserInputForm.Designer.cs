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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInputForm));
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblModuleId = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtApplicationID = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.lblNamespace = new System.Windows.Forms.Label();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.pnlGenerateSolution = new System.Windows.Forms.Panel();
            this.lblGenerateHelp = new System.Windows.Forms.Label();
            this.pnlKendo = new System.Windows.Forms.Panel();
            this.lblKendoVersionHelp = new System.Windows.Forms.Label();
            this.lblKendoLink = new System.Windows.Forms.LinkLabel();
            this.lblKendoFolderHelp = new System.Windows.Forms.Label();
            this.btnKendoDialog = new System.Windows.Forms.Button();
            this.txtKendoFolder = new System.Windows.Forms.TextBox();
            this.lblKendoFolder = new System.Windows.Forms.Label();
            this.chkKendoLicense = new System.Windows.Forms.CheckBox();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblNamespaceHelp = new System.Windows.Forms.Label();
            this.lblModuleIdHelp = new System.Windows.Forms.Label();
            this.lblCompanyNameHelp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlGenerateSolution.SuspendLayout();
            this.pnlKendo.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(12, 22);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(85, 13);
            this.lblCompanyName.TabIndex = 1;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblModuleId
            // 
            this.lblModuleId.AutoSize = true;
            this.lblModuleId.Location = new System.Drawing.Point(12, 51);
            this.lblModuleId.Name = "lblModuleId";
            this.lblModuleId.Size = new System.Drawing.Size(59, 13);
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
            this.lblNamespace.Location = new System.Drawing.Point(12, 77);
            this.lblNamespace.Name = "lblNamespace";
            this.lblNamespace.Size = new System.Drawing.Size(67, 13);
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
            this.splitBase.Panel1.Controls.Add(this.picLogo);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            this.splitBase.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitBase_Panel1_Paint);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.pnlGenerateSolution);
            this.splitBase.Panel2.Controls.Add(this.pnlKendo);
            this.splitBase.Panel2.Controls.Add(this.tbrMain);
            this.splitBase.Panel2.Controls.Add(this.pnlInfo);
            this.splitBase.Size = new System.Drawing.Size(474, 520);
            this.splitBase.SplitterDistance = 100;
            this.splitBase.TabIndex = 7;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Image = global::Sage.CA.SBS.ERP.Sage300.SolutionWizard.Properties.Resources.sage_300_logo;
            this.picLogo.Location = new System.Drawing.Point(366, 28);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(95, 49);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 6;
            this.picLogo.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.Color.White;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(258, 50);
            this.lblStepDescription.TabIndex = 5;
            this.lblStepDescription.Text = "This is the detailed description";
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.AutoSize = true;
            this.lblStepTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblStepTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepTitle.ForeColor = System.Drawing.Color.White;
            this.lblStepTitle.Location = new System.Drawing.Point(12, 9);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(218, 20);
            this.lblStepTitle.TabIndex = 4;
            this.lblStepTitle.Text = "This is the title of the step";
            // 
            // pnlGenerateSolution
            // 
            this.pnlGenerateSolution.Controls.Add(this.lblGenerateHelp);
            this.pnlGenerateSolution.Location = new System.Drawing.Point(363, 116);
            this.pnlGenerateSolution.Name = "pnlGenerateSolution";
            this.pnlGenerateSolution.Size = new System.Drawing.Size(98, 37);
            this.pnlGenerateSolution.TabIndex = 3;
            // 
            // lblGenerateHelp
            // 
            this.lblGenerateHelp.Location = new System.Drawing.Point(112, 65);
            this.lblGenerateHelp.Name = "lblGenerateHelp";
            this.lblGenerateHelp.Size = new System.Drawing.Size(273, 111);
            this.lblGenerateHelp.TabIndex = 0;
            this.lblGenerateHelp.Text = "Select the \'Generate\' button below to generate the solution based upon the inform" +
    "ation entered and selected in the proceeding steps.";
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
            this.pnlKendo.Location = new System.Drawing.Point(395, 74);
            this.pnlKendo.Name = "pnlKendo";
            this.pnlKendo.Size = new System.Drawing.Size(56, 30);
            this.pnlKendo.TabIndex = 2;
            // 
            // lblKendoVersionHelp
            // 
            this.lblKendoVersionHelp.Location = new System.Drawing.Point(109, 203);
            this.lblKendoVersionHelp.Name = "lblKendoVersionHelp";
            this.lblKendoVersionHelp.Size = new System.Drawing.Size(293, 36);
            this.lblKendoVersionHelp.TabIndex = 6;
            this.lblKendoVersionHelp.Text = "The Kendo version used by these projects is v2014.2.1020";
            // 
            // lblKendoLink
            // 
            this.lblKendoLink.AutoSize = true;
            this.lblKendoLink.Location = new System.Drawing.Point(107, 168);
            this.lblKendoLink.Name = "lblKendoLink";
            this.lblKendoLink.Size = new System.Drawing.Size(348, 13);
            this.lblKendoLink.TabIndex = 5;
            this.lblKendoLink.TabStop = true;
            this.lblKendoLink.Text = "http://www.telerik.com/purchase/license-agreement/kendo-ui-complete";
            this.lblKendoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblKendoLink_LinkClicked);
            // 
            // lblKendoFolderHelp
            // 
            this.lblKendoFolderHelp.Location = new System.Drawing.Point(107, 97);
            this.lblKendoFolderHelp.Name = "lblKendoFolderHelp";
            this.lblKendoFolderHelp.Size = new System.Drawing.Size(297, 71);
            this.lblKendoFolderHelp.TabIndex = 4;
            this.lblKendoFolderHelp.Text = resources.GetString("lblKendoFolderHelp.Text");
            // 
            // btnKendoDialog
            // 
            this.btnKendoDialog.Enabled = false;
            this.btnKendoDialog.Location = new System.Drawing.Point(419, 49);
            this.btnKendoDialog.Name = "btnKendoDialog";
            this.btnKendoDialog.Size = new System.Drawing.Size(27, 20);
            this.btnKendoDialog.TabIndex = 3;
            this.btnKendoDialog.Text = "...";
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
            this.chkKendoLicense.Location = new System.Drawing.Point(16, 21);
            this.chkKendoLicense.Name = "chkKendoLicense";
            this.chkKendoLicense.Size = new System.Drawing.Size(214, 17);
            this.chkKendoLicense.TabIndex = 0;
            this.chkKendoLicense.Text = "Purchased Kendo Commercial License?";
            this.chkKendoLicense.UseVisualStyleBackColor = true;
            this.chkKendoLicense.CheckedChanged += new System.EventHandler(this.chkKendoLicense_CheckedChanged);
            // 
            // tbrMain
            // 
            this.tbrMain.BackColor = System.Drawing.SystemColors.Control;
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNext,
            this.btnBack});
            this.tbrMain.Location = new System.Drawing.Point(0, 391);
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(474, 25);
            this.tbrMain.TabIndex = 1;
            this.tbrMain.Text = "toolStrip1";
            this.tbrMain.Paint += new System.Windows.Forms.PaintEventHandler(this.tbrMain_Paint);
            // 
            // btnNext
            // 
            this.btnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnNext.AutoSize = false;
            this.btnNext.BackColor = System.Drawing.SystemColors.Control;
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 22);
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBack.AutoSize = false;
            this.btnBack.BackColor = System.Drawing.SystemColors.Control;
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(58, 22);
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblNamespaceHelp);
            this.pnlInfo.Controls.Add(this.lblModuleIdHelp);
            this.pnlInfo.Controls.Add(this.lblCompanyNameHelp);
            this.pnlInfo.Controls.Add(this.lblCompanyName);
            this.pnlInfo.Controls.Add(this.txtNamespace);
            this.pnlInfo.Controls.Add(this.lblModuleId);
            this.pnlInfo.Controls.Add(this.lblNamespace);
            this.pnlInfo.Controls.Add(this.txtCompanyName);
            this.pnlInfo.Controls.Add(this.txtApplicationID);
            this.pnlInfo.Location = new System.Drawing.Point(372, 23);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(89, 27);
            this.pnlInfo.TabIndex = 0;
            // 
            // lblNamespaceHelp
            // 
            this.lblNamespaceHelp.Location = new System.Drawing.Point(101, 245);
            this.lblNamespaceHelp.Name = "lblNamespaceHelp";
            this.lblNamespaceHelp.Size = new System.Drawing.Size(238, 39);
            this.lblNamespaceHelp.TabIndex = 9;
            this.lblNamespaceHelp.Text = "The Namespace is defaulted from the Company Name and may be overwritten. It will " +
    "be used for the generated code\'s namespace.";
            // 
            // lblModuleIdHelp
            // 
            this.lblModuleIdHelp.Location = new System.Drawing.Point(101, 186);
            this.lblModuleIdHelp.Name = "lblModuleIdHelp";
            this.lblModuleIdHelp.Size = new System.Drawing.Size(226, 37);
            this.lblModuleIdHelp.TabIndex = 8;
            this.lblModuleIdHelp.Text = "The Module ID is the two character code used to identify a module.";
            // 
            // lblCompanyNameHelp
            // 
            this.lblCompanyNameHelp.Location = new System.Drawing.Point(101, 126);
            this.lblCompanyNameHelp.Name = "lblCompanyNameHelp";
            this.lblCompanyNameHelp.Size = new System.Drawing.Size(238, 48);
            this.lblCompanyNameHelp.TabIndex = 7;
            this.lblCompanyNameHelp.Text = "The Company Name is used to generate  Copyright tags and default the Namespace.";
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 520);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solution Wizard";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.UserInputForm_HelpButtonClicked);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserInputForm_FormClosed);
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            this.splitBase.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlGenerateSolution.ResumeLayout(false);
            this.pnlKendo.ResumeLayout(false);
            this.pnlKendo.PerformLayout();
            this.tbrMain.ResumeLayout(false);
            this.tbrMain.PerformLayout();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
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
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.Label lblNamespaceHelp;
        private System.Windows.Forms.Label lblModuleIdHelp;
        private System.Windows.Forms.Label lblCompanyNameHelp;
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
    }
}