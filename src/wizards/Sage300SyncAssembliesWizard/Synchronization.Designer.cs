using System;

namespace Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard
{
    partial class Synchronization
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Synchronization));
            this.cboSourceType = new System.Windows.Forms.ComboBox();
            this.lblSourceType = new System.Windows.Forms.Label();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlSyncedAssemblies = new System.Windows.Forms.Panel();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.pnlSyncAssemblies = new System.Windows.Forms.Panel();
            this.lblProcessingFile = new System.Windows.Forms.Label();
            this.lblSyncAssembliesHelp = new System.Windows.Forms.Label();
            this.pnlAssemblies = new System.Windows.Forms.Panel();
            this.splitAssemblies = new System.Windows.Forms.SplitContainer();
            this.tbrAssemblies = new System.Windows.Forms.ToolStrip();
            this.btnInitialSync = new System.Windows.Forms.ToolStripButton();
            this.btnIncludeAll = new System.Windows.Forms.ToolStripButton();
            this.btnOverrideAll = new System.Windows.Forms.ToolStripButton();
            this.pnlAssembliesGrid = new System.Windows.Forms.Panel();
            this.grdAssemblies = new System.Windows.Forms.DataGridView();
            this.pnlDestination = new System.Windows.Forms.Panel();
            this.lblDestinationWebHelp = new System.Windows.Forms.Label();
            this.btnDestinationWebDialog = new System.Windows.Forms.Button();
            this.txtDestinationWeb = new System.Windows.Forms.TextBox();
            this.lblDestinationWeb = new System.Windows.Forms.Label();
            this.lblDestinationHelp = new System.Windows.Forms.Label();
            this.btnDestinationDialog = new System.Windows.Forms.Button();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.pnlSourceType = new System.Windows.Forms.Panel();
            this.chkIncludePDBFiles = new System.Windows.Forms.CheckBox();
            this.lblIncludePDBFilesHelp = new System.Windows.Forms.Label();
            this.lblSourceTypeDescriptionHelp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlSyncedAssemblies.SuspendLayout();
            this.pnlSyncAssemblies.SuspendLayout();
            this.pnlAssemblies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAssemblies)).BeginInit();
            this.splitAssemblies.Panel1.SuspendLayout();
            this.splitAssemblies.Panel2.SuspendLayout();
            this.splitAssemblies.SuspendLayout();
            this.tbrAssemblies.SuspendLayout();
            this.pnlAssembliesGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAssemblies)).BeginInit();
            this.pnlDestination.SuspendLayout();
            this.pnlSourceType.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSourceType
            // 
            this.cboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceType.FormattingEnabled = true;
            this.cboSourceType.Items.AddRange(new object[] {
            "Local",
            "Jenkins"});
            this.cboSourceType.Location = new System.Drawing.Point(83, 27);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Size = new System.Drawing.Size(141, 21);
            this.cboSourceType.TabIndex = 1;
            // 
            // lblSourceType
            // 
            this.lblSourceType.AutoSize = true;
            this.lblSourceType.Location = new System.Drawing.Point(6, 30);
            this.lblSourceType.Name = "lblSourceType";
            this.lblSourceType.Size = new System.Drawing.Size(71, 13);
            this.lblSourceType.TabIndex = 0;
            this.lblSourceType.Text = "Source Type:";
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
            this.splitBase.Panel2.Controls.Add(this.pnlButtons);
            this.splitBase.Panel2.Controls.Add(this.pnlSyncedAssemblies);
            this.splitBase.Panel2.Controls.Add(this.pnlSyncAssemblies);
            this.splitBase.Panel2.Controls.Add(this.pnlAssemblies);
            this.splitBase.Panel2.Controls.Add(this.pnlDestination);
            this.splitBase.Panel2.Controls.Add(this.pnlSourceType);
            this.splitBase.Size = new System.Drawing.Size(476, 520);
            this.splitBase.SplitterDistance = 90;
            this.splitBase.TabIndex = 9;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(258, 50);
            this.lblStepDescription.TabIndex = 3;
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
            this.lblStepTitle.TabIndex = 2;
            this.lblStepTitle.Text = "This is the title of the step";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 389);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(476, 37);
            this.pnlButtons.TabIndex = 52;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(401, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.TabIndex = 16;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(327, 7);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pnlSyncedAssemblies
            // 
            this.pnlSyncedAssemblies.Controls.Add(this.lblCompleted);
            this.pnlSyncedAssemblies.Location = new System.Drawing.Point(428, 267);
            this.pnlSyncedAssemblies.Name = "pnlSyncedAssemblies";
            this.pnlSyncedAssemblies.Size = new System.Drawing.Size(326, 55);
            this.pnlSyncedAssemblies.TabIndex = 51;
            // 
            // lblCompleted
            // 
            this.lblCompleted.AutoSize = true;
            this.lblCompleted.Location = new System.Drawing.Point(88, 100);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(112, 13);
            this.lblCompleted.TabIndex = 0;
            this.lblCompleted.Text = "{0} Files Synchronized";
            // 
            // pnlSyncAssemblies
            // 
            this.pnlSyncAssemblies.Controls.Add(this.lblProcessingFile);
            this.pnlSyncAssemblies.Controls.Add(this.lblSyncAssembliesHelp);
            this.pnlSyncAssemblies.Location = new System.Drawing.Point(426, 193);
            this.pnlSyncAssemblies.Name = "pnlSyncAssemblies";
            this.pnlSyncAssemblies.Size = new System.Drawing.Size(268, 54);
            this.pnlSyncAssemblies.TabIndex = 50;
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(74, 176);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 13);
            this.lblProcessingFile.TabIndex = 1;
            // 
            // lblSyncAssembliesHelp
            // 
            this.lblSyncAssembliesHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSyncAssembliesHelp.Location = new System.Drawing.Point(74, 74);
            this.lblSyncAssembliesHelp.Name = "lblSyncAssembliesHelp";
            this.lblSyncAssembliesHelp.Size = new System.Drawing.Size(288, 78);
            this.lblSyncAssembliesHelp.TabIndex = 0;
            this.lblSyncAssembliesHelp.Text = "Select the \'Sync\' button below to sync assemblies based upon the Source Type and " +
    "assemblies selected in the proceeding steps.";
            // 
            // pnlAssemblies
            // 
            this.pnlAssemblies.Controls.Add(this.splitAssemblies);
            this.pnlAssemblies.Location = new System.Drawing.Point(423, 131);
            this.pnlAssemblies.Name = "pnlAssemblies";
            this.pnlAssemblies.Size = new System.Drawing.Size(186, 43);
            this.pnlAssemblies.TabIndex = 47;
            // 
            // splitAssemblies
            // 
            this.splitAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAssemblies.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitAssemblies.IsSplitterFixed = true;
            this.splitAssemblies.Location = new System.Drawing.Point(0, 0);
            this.splitAssemblies.Name = "splitAssemblies";
            this.splitAssemblies.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitAssemblies.Panel1
            // 
            this.splitAssemblies.Panel1.Controls.Add(this.tbrAssemblies);
            // 
            // splitAssemblies.Panel2
            // 
            this.splitAssemblies.Panel2.Controls.Add(this.pnlAssembliesGrid);
            this.splitAssemblies.Size = new System.Drawing.Size(186, 43);
            this.splitAssemblies.SplitterDistance = 25;
            this.splitAssemblies.SplitterWidth = 1;
            this.splitAssemblies.TabIndex = 0;
            // 
            // tbrAssemblies
            // 
            this.tbrAssemblies.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInitialSync,
            this.btnIncludeAll,
            this.btnOverrideAll});
            this.tbrAssemblies.Location = new System.Drawing.Point(0, 0);
            this.tbrAssemblies.Name = "tbrAssemblies";
            this.tbrAssemblies.Size = new System.Drawing.Size(186, 25);
            this.tbrAssemblies.TabIndex = 53;
            this.tbrAssemblies.Text = "toolStrip1";
            // 
            // btnInitialSync
            // 
            this.btnInitialSync.CheckOnClick = true;
            this.btnInitialSync.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnInitialSync.Image = ((System.Drawing.Image)(resources.GetObject("btnInitialSync.Image")));
            this.btnInitialSync.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInitialSync.Name = "btnInitialSync";
            this.btnInitialSync.Size = new System.Drawing.Size(68, 22);
            this.btnInitialSync.Text = "Initial Sync";
            this.btnInitialSync.Click += new System.EventHandler(this.btnInitialSync_Click);
            // 
            // btnIncludeAll
            // 
            this.btnIncludeAll.CheckOnClick = true;
            this.btnIncludeAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnIncludeAll.Image = ((System.Drawing.Image)(resources.GetObject("btnIncludeAll.Image")));
            this.btnIncludeAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIncludeAll.Name = "btnIncludeAll";
            this.btnIncludeAll.Size = new System.Drawing.Size(67, 22);
            this.btnIncludeAll.Text = "Include All";
            this.btnIncludeAll.Click += new System.EventHandler(this.btnIncludeAll_Click);
            // 
            // btnOverrideAll
            // 
            this.btnOverrideAll.Checked = true;
            this.btnOverrideAll.CheckOnClick = true;
            this.btnOverrideAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnOverrideAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOverrideAll.Image = ((System.Drawing.Image)(resources.GetObject("btnOverrideAll.Image")));
            this.btnOverrideAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOverrideAll.Name = "btnOverrideAll";
            this.btnOverrideAll.Size = new System.Drawing.Size(73, 19);
            this.btnOverrideAll.Text = "Override All";
            this.btnOverrideAll.Click += new System.EventHandler(this.btnOverrideAll_Click);
            // 
            // pnlAssembliesGrid
            // 
            this.pnlAssembliesGrid.Controls.Add(this.grdAssemblies);
            this.pnlAssembliesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAssembliesGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlAssembliesGrid.Name = "pnlAssembliesGrid";
            this.pnlAssembliesGrid.Size = new System.Drawing.Size(186, 28);
            this.pnlAssembliesGrid.TabIndex = 53;
            // 
            // grdAssemblies
            // 
            this.grdAssemblies.AllowUserToAddRows = false;
            this.grdAssemblies.AllowUserToDeleteRows = false;
            this.grdAssemblies.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdAssemblies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAssemblies.Location = new System.Drawing.Point(0, 0);
            this.grdAssemblies.Name = "grdAssemblies";
            this.grdAssemblies.Size = new System.Drawing.Size(186, 28);
            this.grdAssemblies.TabIndex = 52;
            // 
            // pnlDestination
            // 
            this.pnlDestination.Controls.Add(this.lblDestinationWebHelp);
            this.pnlDestination.Controls.Add(this.btnDestinationWebDialog);
            this.pnlDestination.Controls.Add(this.txtDestinationWeb);
            this.pnlDestination.Controls.Add(this.lblDestinationWeb);
            this.pnlDestination.Controls.Add(this.lblDestinationHelp);
            this.pnlDestination.Controls.Add(this.btnDestinationDialog);
            this.pnlDestination.Controls.Add(this.txtDestination);
            this.pnlDestination.Controls.Add(this.lblDestination);
            this.pnlDestination.Location = new System.Drawing.Point(423, 71);
            this.pnlDestination.Name = "pnlDestination";
            this.pnlDestination.Size = new System.Drawing.Size(272, 39);
            this.pnlDestination.TabIndex = 49;
            // 
            // lblDestinationWebHelp
            // 
            this.lblDestinationWebHelp.Location = new System.Drawing.Point(73, 182);
            this.lblDestinationWebHelp.Name = "lblDestinationWebHelp";
            this.lblDestinationWebHelp.Size = new System.Drawing.Size(289, 58);
            this.lblDestinationWebHelp.TabIndex = 38;
            this.lblDestinationWebHelp.Text = "A destination Web folder must be specfied in order to copy the selected source fi" +
    "les.";
            // 
            // btnDestinationWebDialog
            // 
            this.btnDestinationWebDialog.Location = new System.Drawing.Point(390, 139);
            this.btnDestinationWebDialog.Name = "btnDestinationWebDialog";
            this.btnDestinationWebDialog.Size = new System.Drawing.Size(27, 20);
            this.btnDestinationWebDialog.TabIndex = 37;
            this.btnDestinationWebDialog.Text = "...";
            this.btnDestinationWebDialog.UseVisualStyleBackColor = true;
            this.btnDestinationWebDialog.Click += new System.EventHandler(this.btnDestinationWebDialog_Click);
            // 
            // txtDestinationWeb
            // 
            this.txtDestinationWeb.Location = new System.Drawing.Point(76, 139);
            this.txtDestinationWeb.Name = "txtDestinationWeb";
            this.txtDestinationWeb.Size = new System.Drawing.Size(308, 20);
            this.txtDestinationWeb.TabIndex = 36;
            // 
            // lblDestinationWeb
            // 
            this.lblDestinationWeb.AutoSize = true;
            this.lblDestinationWeb.Location = new System.Drawing.Point(37, 142);
            this.lblDestinationWeb.Name = "lblDestinationWeb";
            this.lblDestinationWeb.Size = new System.Drawing.Size(33, 13);
            this.lblDestinationWeb.TabIndex = 35;
            this.lblDestinationWeb.Text = "Web:";
            // 
            // lblDestinationHelp
            // 
            this.lblDestinationHelp.Location = new System.Drawing.Point(73, 75);
            this.lblDestinationHelp.Name = "lblDestinationHelp";
            this.lblDestinationHelp.Size = new System.Drawing.Size(289, 58);
            this.lblDestinationHelp.TabIndex = 35;
            this.lblDestinationHelp.Text = "A destination Assemblies folder must be specfied in order to copy the selected so" +
    "urce files.";
            // 
            // btnDestinationDialog
            // 
            this.btnDestinationDialog.Location = new System.Drawing.Point(390, 40);
            this.btnDestinationDialog.Name = "btnDestinationDialog";
            this.btnDestinationDialog.Size = new System.Drawing.Size(27, 20);
            this.btnDestinationDialog.TabIndex = 34;
            this.btnDestinationDialog.Text = "...";
            this.btnDestinationDialog.UseVisualStyleBackColor = true;
            this.btnDestinationDialog.Click += new System.EventHandler(this.btnDestinationDialog_Click);
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(76, 42);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(308, 20);
            this.txtDestination.TabIndex = 33;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(8, 44);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(62, 13);
            this.lblDestination.TabIndex = 32;
            this.lblDestination.Text = "Assemblies:";
            // 
            // pnlSourceType
            // 
            this.pnlSourceType.Controls.Add(this.chkIncludePDBFiles);
            this.pnlSourceType.Controls.Add(this.lblIncludePDBFilesHelp);
            this.pnlSourceType.Controls.Add(this.lblSourceTypeDescriptionHelp);
            this.pnlSourceType.Controls.Add(this.lblSourceType);
            this.pnlSourceType.Controls.Add(this.cboSourceType);
            this.pnlSourceType.Location = new System.Drawing.Point(423, 14);
            this.pnlSourceType.Name = "pnlSourceType";
            this.pnlSourceType.Size = new System.Drawing.Size(309, 48);
            this.pnlSourceType.TabIndex = 44;
            // 
            // chkIncludePDBFiles
            // 
            this.chkIncludePDBFiles.AutoSize = true;
            this.chkIncludePDBFiles.Location = new System.Drawing.Point(83, 70);
            this.chkIncludePDBFiles.Name = "chkIncludePDBFiles";
            this.chkIncludePDBFiles.Size = new System.Drawing.Size(116, 17);
            this.chkIncludePDBFiles.TabIndex = 4;
            this.chkIncludePDBFiles.Text = "Include PDB Files?";
            this.chkIncludePDBFiles.UseVisualStyleBackColor = true;
            // 
            // lblIncludePDBFilesHelp
            // 
            this.lblIncludePDBFilesHelp.Location = new System.Drawing.Point(80, 163);
            this.lblIncludePDBFilesHelp.Name = "lblIncludePDBFilesHelp";
            this.lblIncludePDBFilesHelp.Size = new System.Drawing.Size(301, 87);
            this.lblIncludePDBFilesHelp.TabIndex = 3;
            this.lblIncludePDBFilesHelp.Text = "Include PDB files if they are available from the Source.";
            // 
            // lblSourceTypeDescriptionHelp
            // 
            this.lblSourceTypeDescriptionHelp.Location = new System.Drawing.Point(80, 102);
            this.lblSourceTypeDescriptionHelp.Name = "lblSourceTypeDescriptionHelp";
            this.lblSourceTypeDescriptionHelp.Size = new System.Drawing.Size(283, 45);
            this.lblSourceTypeDescriptionHelp.TabIndex = 2;
            this.lblSourceTypeDescriptionHelp.Text = "The Source Type allows the developer to choose the source for the assemblies (Loc" +
    "al Installation or Latest Build on Jenkins).";
            // 
            // Synchronization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 520);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Synchronization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sync Assemblies";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Generation_HelpButtonClicked);
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlSyncedAssemblies.ResumeLayout(false);
            this.pnlSyncedAssemblies.PerformLayout();
            this.pnlSyncAssemblies.ResumeLayout(false);
            this.pnlSyncAssemblies.PerformLayout();
            this.pnlAssemblies.ResumeLayout(false);
            this.splitAssemblies.Panel1.ResumeLayout(false);
            this.splitAssemblies.Panel1.PerformLayout();
            this.splitAssemblies.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAssemblies)).EndInit();
            this.splitAssemblies.ResumeLayout(false);
            this.tbrAssemblies.ResumeLayout(false);
            this.tbrAssemblies.PerformLayout();
            this.pnlAssembliesGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAssemblies)).EndInit();
            this.pnlDestination.ResumeLayout(false);
            this.pnlDestination.PerformLayout();
            this.pnlSourceType.ResumeLayout(false);
            this.pnlSourceType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ComboBox cboSourceType;
        private System.Windows.Forms.Label lblSourceType;
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.Panel pnlSourceType;
        private System.Windows.Forms.Label lblSourceTypeDescriptionHelp;
        private System.Windows.Forms.Panel pnlAssemblies;
        private System.Windows.Forms.Panel pnlDestination;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Panel pnlSyncedAssemblies;
        private System.Windows.Forms.Panel pnlSyncAssemblies;
        private System.Windows.Forms.Label lblSyncAssembliesHelp;
        private System.Windows.Forms.Label lblProcessingFile;
        private System.Windows.Forms.Label lblIncludePDBFilesHelp;
        private System.Windows.Forms.CheckBox chkIncludePDBFiles;
        private System.Windows.Forms.Button btnDestinationDialog;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblDestinationHelp;
        private System.Windows.Forms.SplitContainer splitAssemblies;
        private System.Windows.Forms.ToolStrip tbrAssemblies;
        private System.Windows.Forms.ToolStripButton btnIncludeAll;
        private System.Windows.Forms.ToolStripButton btnOverrideAll;
        private System.Windows.Forms.DataGridView grdAssemblies;
        private System.Windows.Forms.ToolStripButton btnInitialSync;
        private System.Windows.Forms.Label lblDestinationWebHelp;
        private System.Windows.Forms.Button btnDestinationWebDialog;
        private System.Windows.Forms.TextBox txtDestinationWeb;
        private System.Windows.Forms.Label lblDestinationWeb;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Panel pnlAssembliesGrid;
    }
}

