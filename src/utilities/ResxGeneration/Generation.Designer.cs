namespace Sage.CA.SBS.ERP.Sage300.ResxGeneration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generation));
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.btnProceed = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRowAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteRow = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteRows = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.sbrMain = new System.Windows.Forms.StatusStrip();
            this.lblProcessing = new System.Windows.Forms.ToolStripStatusLabel();
            this.proProcessing = new System.Windows.Forms.ToolStripProgressBar();
            this.lblProcessingFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdResourceInfo = new System.Windows.Forms.DataGridView();
            this.chkTraditionalChinese = new System.Windows.Forms.CheckBox();
            this.chkSimplifiedChinese = new System.Windows.Forms.CheckBox();
            this.chkSpanish = new System.Windows.Forms.CheckBox();
            this.chkFrench = new System.Windows.Forms.CheckBox();
            this.chkEnglish = new System.Windows.Forms.CheckBox();
            this.lblLanguages = new System.Windows.Forms.Label();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.tbrMain.SuspendLayout();
            this.sbrMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResourceInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrMain
            // 
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProceed,
            this.btnSave,
            this.btnExit,
            this.toolStripSeparator1,
            this.btnImport,
            this.btnExport,
            this.toolStripSeparator2,
            this.btnRowAdd,
            this.btnDeleteRow,
            this.btnDeleteRows,
            this.toolStripSeparator3,
            this.btnHelp});
            this.tbrMain.Location = new System.Drawing.Point(0, 0);
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(1115, 25);
            this.tbrMain.TabIndex = 0;
            this.tbrMain.Text = "toolStrip1";
            // 
            // btnProceed
            // 
            this.btnProceed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProceed.Image = ((System.Drawing.Image)(resources.GetObject("btnProceed.Image")));
            this.btnProceed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(23, 22);
            this.btnProceed.Text = "Proceed";
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Save with Statuses";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(23, 22);
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnImport
            // 
            this.btnImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(23, 22);
            this.btnImport.Text = "Import Settings";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(23, 22);
            this.btnExport.Text = "Export Settings";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRowAdd
            // 
            this.btnRowAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRowAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnRowAdd.Image")));
            this.btnRowAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRowAdd.Name = "btnRowAdd";
            this.btnRowAdd.Size = new System.Drawing.Size(23, 22);
            this.btnRowAdd.Text = "Add Row";
            this.btnRowAdd.Click += new System.EventHandler(this.btnRowAdd_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteRow.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteRow.Image")));
            this.btnDeleteRow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // btnDeleteRows
            // 
            this.btnDeleteRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteRows.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteRows.Image")));
            this.btnDeleteRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteRows.Name = "btnDeleteRows";
            this.btnDeleteRows.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteRows.Text = "Delete Rows";
            this.btnDeleteRows.Click += new System.EventHandler(this.btnDeleteRows_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 22);
            this.btnHelp.Text = "Help";
            this.btnHelp.Visible = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // sbrMain
            // 
            this.sbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblProcessing,
            this.proProcessing,
            this.lblProcessingFile});
            this.sbrMain.Location = new System.Drawing.Point(0, 537);
            this.sbrMain.Name = "sbrMain";
            this.sbrMain.Size = new System.Drawing.Size(1115, 22);
            this.sbrMain.TabIndex = 1;
            this.sbrMain.Text = "statusStrip1";
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = false;
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(200, 17);
            this.lblProcessing.Text = "Processing Placeholder";
            // 
            // proProcessing
            // 
            this.proProcessing.Name = "proProcessing";
            this.proProcessing.Size = new System.Drawing.Size(150, 16);
            this.proProcessing.Step = 1;
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(125, 17);
            this.lblProcessingFile.Text = "File Name Placeholder";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grdResourceInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chkTraditionalChinese);
            this.splitContainer1.Panel2.Controls.Add(this.chkSimplifiedChinese);
            this.splitContainer1.Panel2.Controls.Add(this.chkSpanish);
            this.splitContainer1.Panel2.Controls.Add(this.chkFrench);
            this.splitContainer1.Panel2.Controls.Add(this.chkEnglish);
            this.splitContainer1.Panel2.Controls.Add(this.lblLanguages);
            this.splitContainer1.Panel2.Controls.Add(this.chkOverwrite);
            this.splitContainer1.Size = new System.Drawing.Size(1115, 512);
            this.splitContainer1.SplitterDistance = 371;
            this.splitContainer1.TabIndex = 2;
            // 
            // grdResourceInfo
            // 
            this.grdResourceInfo.AllowUserToAddRows = false;
            this.grdResourceInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResourceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResourceInfo.Location = new System.Drawing.Point(0, 0);
            this.grdResourceInfo.Name = "grdResourceInfo";
            this.grdResourceInfo.Size = new System.Drawing.Size(1115, 371);
            this.grdResourceInfo.TabIndex = 0;
            // 
            // chkTraditionalChinese
            // 
            this.chkTraditionalChinese.AutoSize = true;
            this.chkTraditionalChinese.Location = new System.Drawing.Point(479, 54);
            this.chkTraditionalChinese.Name = "chkTraditionalChinese";
            this.chkTraditionalChinese.Size = new System.Drawing.Size(122, 17);
            this.chkTraditionalChinese.TabIndex = 6;
            this.chkTraditionalChinese.Tag = "";
            this.chkTraditionalChinese.Text = "Chinese (Traditional)";
            this.chkTraditionalChinese.UseVisualStyleBackColor = true;
            // 
            // chkSimplifiedChinese
            // 
            this.chkSimplifiedChinese.AutoSize = true;
            this.chkSimplifiedChinese.Location = new System.Drawing.Point(339, 54);
            this.chkSimplifiedChinese.Name = "chkSimplifiedChinese";
            this.chkSimplifiedChinese.Size = new System.Drawing.Size(117, 17);
            this.chkSimplifiedChinese.TabIndex = 5;
            this.chkSimplifiedChinese.Tag = "";
            this.chkSimplifiedChinese.Text = "Chinese (Simplified)";
            this.chkSimplifiedChinese.UseVisualStyleBackColor = true;
            // 
            // chkSpanish
            // 
            this.chkSpanish.AutoSize = true;
            this.chkSpanish.Location = new System.Drawing.Point(257, 54);
            this.chkSpanish.Name = "chkSpanish";
            this.chkSpanish.Size = new System.Drawing.Size(64, 17);
            this.chkSpanish.TabIndex = 4;
            this.chkSpanish.Tag = "";
            this.chkSpanish.Text = "Spanish";
            this.chkSpanish.UseVisualStyleBackColor = true;
            // 
            // chkFrench
            // 
            this.chkFrench.AutoSize = true;
            this.chkFrench.Location = new System.Drawing.Point(178, 54);
            this.chkFrench.Name = "chkFrench";
            this.chkFrench.Size = new System.Drawing.Size(59, 17);
            this.chkFrench.TabIndex = 3;
            this.chkFrench.Tag = "";
            this.chkFrench.Text = "French";
            this.chkFrench.UseVisualStyleBackColor = true;
            // 
            // chkEnglish
            // 
            this.chkEnglish.AutoSize = true;
            this.chkEnglish.Location = new System.Drawing.Point(103, 54);
            this.chkEnglish.Name = "chkEnglish";
            this.chkEnglish.Size = new System.Drawing.Size(60, 17);
            this.chkEnglish.TabIndex = 2;
            this.chkEnglish.Tag = "";
            this.chkEnglish.Text = "English";
            this.chkEnglish.UseVisualStyleBackColor = true;
            // 
            // lblLanguages
            // 
            this.lblLanguages.AutoSize = true;
            this.lblLanguages.Location = new System.Drawing.Point(20, 55);
            this.lblLanguages.Name = "lblLanguages";
            this.lblLanguages.Size = new System.Drawing.Size(63, 13);
            this.lblLanguages.TabIndex = 1;
            this.lblLanguages.Text = "Languages:";
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Checked = true;
            this.chkOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverwrite.Location = new System.Drawing.Point(17, 21);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(153, 17);
            this.chkOverwrite.TabIndex = 0;
            this.chkOverwrite.Text = "Overwrite file(s) if they exist";
            this.chkOverwrite.UseVisualStyleBackColor = true;
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
            this.ClientSize = new System.Drawing.Size(1115, 559);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.sbrMain);
            this.Controls.Add(this.tbrMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Generation";
            this.Text = "Resx Generation";
            this.Load += new System.EventHandler(this.Generation_Load);
            this.tbrMain.ResumeLayout(false);
            this.tbrMain.PerformLayout();
            this.sbrMain.ResumeLayout(false);
            this.sbrMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResourceInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton btnProceed;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.StatusStrip sbrMain;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessing;
        private System.Windows.Forms.ToolStripProgressBar proProcessing;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessingFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grdResourceInfo;
        private System.Windows.Forms.CheckBox chkTraditionalChinese;
        private System.Windows.Forms.CheckBox chkSimplifiedChinese;
        private System.Windows.Forms.CheckBox chkSpanish;
        private System.Windows.Forms.CheckBox chkFrench;
        private System.Windows.Forms.CheckBox chkEnglish;
        private System.Windows.Forms.Label lblLanguages;
        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnDeleteRow;
        private System.Windows.Forms.ToolStripButton btnDeleteRows;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnRowAdd;
    }
}

