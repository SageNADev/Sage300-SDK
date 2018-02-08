namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
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
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlCreateEdit = new System.Windows.Forms.Panel();
            this.grdProperties = new System.Windows.Forms.DataGridView();
            this.lblModel = new System.Windows.Forms.Label();
            this.cboAssembly = new System.Windows.Forms.ComboBox();
            this.lblAssembly = new System.Windows.Forms.Label();
            this.txtInquiryDescription = new System.Windows.Forms.TextBox();
            this.lblInquiryDescription = new System.Windows.Forms.Label();
            this.btnFolder = new System.Windows.Forms.Button();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnInquiryFinder = new System.Windows.Forms.Button();
            this.txtInquiryId = new System.Windows.Forms.TextBox();
            this.lblInquiryId = new System.Windows.Forms.Label();
            this.pnlGenerated = new System.Windows.Forms.Panel();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.pnlGenerate = new System.Windows.Forms.Panel();
            this.txtJsonToGenerate = new System.Windows.Forms.TextBox();
            this.lblGenerateJson = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblProcessingFile = new System.Windows.Forms.Label();
            this.tbrControls = new System.Windows.Forms.ToolStrip();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            this.pnlCreateEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProperties)).BeginInit();
            this.pnlGenerated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.pnlGenerate.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(0, 0);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(959, 554);
            this.splitBase.SplitterDistance = 90;
            this.splitBase.TabIndex = 1;
            this.splitBase.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(929, 50);
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
            this.splitSteps.Panel1.Controls.Add(this.pnlCreateEdit);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerated);
            this.splitSteps.Panel1.Controls.Add(this.pnlGenerate);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(959, 460);
            this.splitSteps.SplitterDistance = 415;
            this.splitSteps.TabIndex = 5;
            // 
            // pnlCreateEdit
            // 
            this.pnlCreateEdit.Controls.Add(this.grdProperties);
            this.pnlCreateEdit.Controls.Add(this.lblModel);
            this.pnlCreateEdit.Controls.Add(this.cboAssembly);
            this.pnlCreateEdit.Controls.Add(this.lblAssembly);
            this.pnlCreateEdit.Controls.Add(this.txtInquiryDescription);
            this.pnlCreateEdit.Controls.Add(this.lblInquiryDescription);
            this.pnlCreateEdit.Controls.Add(this.btnFolder);
            this.pnlCreateEdit.Controls.Add(this.cboModel);
            this.pnlCreateEdit.Controls.Add(this.txtFolderName);
            this.pnlCreateEdit.Controls.Add(this.lblFolder);
            this.pnlCreateEdit.Controls.Add(this.btnNew);
            this.pnlCreateEdit.Controls.Add(this.btnInquiryFinder);
            this.pnlCreateEdit.Controls.Add(this.txtInquiryId);
            this.pnlCreateEdit.Controls.Add(this.lblInquiryId);
            this.pnlCreateEdit.Location = new System.Drawing.Point(782, 35);
            this.pnlCreateEdit.Name = "pnlCreateEdit";
            this.pnlCreateEdit.Size = new System.Drawing.Size(554, 109);
            this.pnlCreateEdit.TabIndex = 0;
            // 
            // grdProperties
            // 
            this.grdProperties.AllowUserToAddRows = false;
            this.grdProperties.AllowUserToDeleteRows = false;
            this.grdProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdProperties.Location = new System.Drawing.Point(0, -93);
            this.grdProperties.Name = "grdProperties";
            this.grdProperties.Size = new System.Drawing.Size(554, 202);
            this.grdProperties.TabIndex = 24;
            // 
            // lblModel
            // 
            this.lblModel.AutoSize = true;
            this.lblModel.Location = new System.Drawing.Point(51, 131);
            this.lblModel.Name = "lblModel";
            this.lblModel.Size = new System.Drawing.Size(43, 13);
            this.lblModel.TabIndex = 23;
            this.lblModel.Text = "Model:";
            // 
            // cboAssembly
            // 
            this.cboAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssembly.FormattingEnabled = true;
            this.cboAssembly.Location = new System.Drawing.Point(100, 101);
            this.cboAssembly.Name = "cboAssembly";
            this.cboAssembly.Size = new System.Drawing.Size(487, 21);
            this.cboAssembly.TabIndex = 2;
            this.cboAssembly.SelectedIndexChanged += new System.EventHandler(this.cboAssembly_SelectedIndexChanged);
            // 
            // lblAssembly
            // 
            this.lblAssembly.AutoSize = true;
            this.lblAssembly.Location = new System.Drawing.Point(37, 104);
            this.lblAssembly.Name = "lblAssembly";
            this.lblAssembly.Size = new System.Drawing.Size(57, 13);
            this.lblAssembly.TabIndex = 22;
            this.lblAssembly.Text = "Assembly:";
            // 
            // txtInquiryDescription
            // 
            this.txtInquiryDescription.Location = new System.Drawing.Point(100, 73);
            this.txtInquiryDescription.MaxLength = 255;
            this.txtInquiryDescription.Name = "txtInquiryDescription";
            this.txtInquiryDescription.Size = new System.Drawing.Size(487, 22);
            this.txtInquiryDescription.TabIndex = 10;
            // 
            // lblInquiryDescription
            // 
            this.lblInquiryDescription.AutoSize = true;
            this.lblInquiryDescription.Location = new System.Drawing.Point(25, 76);
            this.lblInquiryDescription.Name = "lblInquiryDescription";
            this.lblInquiryDescription.Size = new System.Drawing.Size(69, 13);
            this.lblInquiryDescription.TabIndex = 9;
            this.lblInquiryDescription.Text = "Description:";
            // 
            // btnFolder
            // 
            this.btnFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFolder.BackgroundImage")));
            this.btnFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFolder.FlatAppearance.BorderSize = 0;
            this.btnFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolder.Location = new System.Drawing.Point(593, 45);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(25, 25);
            this.btnFolder.TabIndex = 6;
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // cboModel
            // 
            this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Location = new System.Drawing.Point(100, 128);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(487, 21);
            this.cboModel.TabIndex = 4;
            this.cboModel.SelectedIndexChanged += new System.EventHandler(this.cboModel_SelectedIndexChanged);
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Control;
            this.txtFolderName.Location = new System.Drawing.Point(100, 45);
            this.txtFolderName.MaxLength = 255;
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.ReadOnly = true;
            this.txtFolderName.Size = new System.Drawing.Size(487, 22);
            this.txtFolderName.TabIndex = 5;
            this.txtFolderName.TabStop = false;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(51, 48);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(43, 13);
            this.lblFolder.TabIndex = 4;
            this.lblFolder.Text = "Folder:";
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.Window;
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Location = new System.Drawing.Point(453, 17);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(25, 25);
            this.btnNew.TabIndex = 3;
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnInquiryFinder
            // 
            this.btnInquiryFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInquiryFinder.BackgroundImage")));
            this.btnInquiryFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInquiryFinder.FlatAppearance.BorderSize = 0;
            this.btnInquiryFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInquiryFinder.Location = new System.Drawing.Point(422, 17);
            this.btnInquiryFinder.Name = "btnInquiryFinder";
            this.btnInquiryFinder.Size = new System.Drawing.Size(25, 25);
            this.btnInquiryFinder.TabIndex = 2;
            this.btnInquiryFinder.UseVisualStyleBackColor = true;
            this.btnInquiryFinder.Click += new System.EventHandler(this.btnInquiryFinder_Click);
            // 
            // txtInquiryId
            // 
            this.txtInquiryId.BackColor = System.Drawing.SystemColors.Control;
            this.txtInquiryId.Location = new System.Drawing.Point(100, 17);
            this.txtInquiryId.MaxLength = 36;
            this.txtInquiryId.Name = "txtInquiryId";
            this.txtInquiryId.ReadOnly = true;
            this.txtInquiryId.Size = new System.Drawing.Size(316, 22);
            this.txtInquiryId.TabIndex = 1;
            this.txtInquiryId.TabStop = false;
            // 
            // lblInquiryId
            // 
            this.lblInquiryId.AutoSize = true;
            this.lblInquiryId.Location = new System.Drawing.Point(48, 20);
            this.lblInquiryId.Name = "lblInquiryId";
            this.lblInquiryId.Size = new System.Drawing.Size(46, 13);
            this.lblInquiryId.TabIndex = 0;
            this.lblInquiryId.Text = "Inquiry:";
            // 
            // pnlGenerated
            // 
            this.pnlGenerated.Controls.Add(this.grdInfo);
            this.pnlGenerated.Location = new System.Drawing.Point(788, 294);
            this.pnlGenerated.Name = "pnlGenerated";
            this.pnlGenerated.Size = new System.Drawing.Size(505, 101);
            this.pnlGenerated.TabIndex = 4;
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
            this.grdInfo.Size = new System.Drawing.Size(505, 101);
            this.grdInfo.TabIndex = 0;
            // 
            // pnlGenerate
            // 
            this.pnlGenerate.Controls.Add(this.txtJsonToGenerate);
            this.pnlGenerate.Controls.Add(this.lblGenerateJson);
            this.pnlGenerate.Location = new System.Drawing.Point(788, 177);
            this.pnlGenerate.Name = "pnlGenerate";
            this.pnlGenerate.Size = new System.Drawing.Size(373, 99);
            this.pnlGenerate.TabIndex = 3;
            // 
            // txtJsonToGenerate
            // 
            this.txtJsonToGenerate.BackColor = System.Drawing.SystemColors.Window;
            this.txtJsonToGenerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJsonToGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJsonToGenerate.Location = new System.Drawing.Point(0, 35);
            this.txtJsonToGenerate.Multiline = true;
            this.txtJsonToGenerate.Name = "txtJsonToGenerate";
            this.txtJsonToGenerate.ReadOnly = true;
            this.txtJsonToGenerate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJsonToGenerate.Size = new System.Drawing.Size(373, 64);
            this.txtJsonToGenerate.TabIndex = 1;
            // 
            // lblGenerateJson
            // 
            this.lblGenerateJson.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGenerateJson.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenerateJson.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblGenerateJson.Location = new System.Drawing.Point(0, 0);
            this.lblGenerateJson.Name = "lblGenerateJson";
            this.lblGenerateJson.Size = new System.Drawing.Size(373, 35);
            this.lblGenerateJson.TabIndex = 2;
            this.lblGenerateJson.Text = "The {0}.json file will be generated with the following content";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(959, 41);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(879, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(805, 7);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(11, 13);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 13);
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
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(959, 554);
            this.Controls.Add(this.splitBase);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Generation";
            this.Text = "Inquiry Configuration";
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            this.splitSteps.Panel1.ResumeLayout(false);
            this.splitSteps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).EndInit();
            this.splitSteps.ResumeLayout(false);
            this.pnlCreateEdit.ResumeLayout(false);
            this.pnlCreateEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdProperties)).EndInit();
            this.pnlGenerated.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.pnlGenerate.ResumeLayout(false);
            this.pnlGenerate.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlGenerated;
        private System.Windows.Forms.Panel pnlGenerate;
        private System.Windows.Forms.Panel pnlCreateEdit;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.TextBox txtInquiryDescription;
        private System.Windows.Forms.Label lblInquiryDescription;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnInquiryFinder;
        private System.Windows.Forms.TextBox txtInquiryId;
        private System.Windows.Forms.Label lblInquiryId;
        private System.Windows.Forms.DataGridView grdInfo;
        private System.Windows.Forms.Label lblProcessingFile;
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ToolStrip tbrControls;
        private System.Windows.Forms.ComboBox cboModel;
        private System.Windows.Forms.ComboBox cboAssembly;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TextBox txtJsonToGenerate;
        private System.Windows.Forms.Label lblAssembly;
        private System.Windows.Forms.Label lblGenerateJson;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.DataGridView grdProperties;
    }
}

