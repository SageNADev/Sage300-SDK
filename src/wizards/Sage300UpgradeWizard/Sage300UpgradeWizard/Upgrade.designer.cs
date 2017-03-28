using System;

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    partial class Upgrade
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Upgrade));
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnBack = new System.Windows.Forms.ToolStripButton();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.lnkBlog = new System.Windows.Forms.LinkLabel();
            this.picProcess = new System.Windows.Forms.PictureBox();
            this.lblInformation = new System.Windows.Forms.Label();
            this.chkConvert = new System.Windows.Forms.CheckBox();
            this.tbrMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // wrkBackground
            // 
            this.wrkBackground.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wrkBackground_DoWork);
            this.wrkBackground.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wrkBackground_RunWorkerCompleted);
            // 
            // tbrMain
            // 
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbrMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNext,
            this.btnBack});
            this.tbrMain.Location = new System.Drawing.Point(0, 491);
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(474, 29);
            this.tbrMain.TabIndex = 10;
            this.tbrMain.Text = "toolStrip2";
            this.tbrMain.Paint += new System.Windows.Forms.PaintEventHandler(this.tbrMain_Paint);
            // 
            // btnNext
            // 
            this.btnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnNext.AutoSize = false;
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 26);
            this.btnNext.Text = "Next";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBack.AutoSize = false;
            this.btnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(80, 26);
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblStepTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblStepTitle.ForeColor = System.Drawing.Color.White;
            this.lblStepTitle.Location = new System.Drawing.Point(10, 28);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(350, 49);
            this.lblStepTitle.TabIndex = 2;
            this.lblStepTitle.Text = "Step {0} - {1}";
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(366, 28);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(95, 49);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 4;
            this.picLogo.TabStop = false;
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
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            this.splitBase.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitBase_Panel1_Paint);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.lnkBlog);
            this.splitBase.Panel2.Controls.Add(this.picProcess);
            this.splitBase.Panel2.Controls.Add(this.lblInformation);
            this.splitBase.Panel2.Controls.Add(this.chkConvert);
            this.splitBase.Size = new System.Drawing.Size(474, 520);
            this.splitBase.SplitterDistance = 89;
            this.splitBase.TabIndex = 9;
            // 
            // lnkBlog
            // 
            this.lnkBlog.AutoSize = true;
            this.lnkBlog.Location = new System.Drawing.Point(20, 362);
            this.lnkBlog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkBlog.Name = "lnkBlog";
            this.lnkBlog.Size = new System.Drawing.Size(393, 13);
            this.lnkBlog.TabIndex = 3;
            this.lnkBlog.TabStop = true;
            this.lnkBlog.Text = "https://jthomas903.wordpress.com/2017/01/24/sage-300-optional-resource-files/";
            this.lnkBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBlog_LinkClicked);
            // 
            // picProcess
            // 
            this.picProcess.Image = global::Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties.Resources.loading_new;
            this.picProcess.Location = new System.Drawing.Point(184, 16);
            this.picProcess.Margin = new System.Windows.Forms.Padding(2);
            this.picProcess.Name = "picProcess";
            this.picProcess.Size = new System.Drawing.Size(128, 131);
            this.picProcess.TabIndex = 2;
            this.picProcess.TabStop = false;
            // 
            // lblInformation
            // 
            this.lblInformation.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformation.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblInformation.Location = new System.Drawing.Point(18, 16);
            this.lblInformation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(427, 361);
            this.lblInformation.TabIndex = 1;
            // 
            // chkConvert
            // 
            this.chkConvert.AutoSize = true;
            this.chkConvert.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkConvert.Location = new System.Drawing.Point(20, 379);
            this.chkConvert.Margin = new System.Windows.Forms.Padding(2);
            this.chkConvert.Name = "chkConvert";
            this.chkConvert.Size = new System.Drawing.Size(209, 17);
            this.chkConvert.TabIndex = 0;
            this.chkConvert.Text = "Convert to module specific web project";
            this.chkConvert.UseVisualStyleBackColor = true;
            // 
            // Upgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 520);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Upgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sage300 Upgrade Wizard";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Generation_HelpButtonClicked);
            this.tbrMain.ResumeLayout(false);
            this.tbrMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel2.ResumeLayout(false);
            this.splitBase.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picProcess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnBack;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.CheckBox chkConvert;
        private System.Windows.Forms.Label lblInformation;
		private System.Windows.Forms.PictureBox picProcess;
		private System.Windows.Forms.LinkLabel lnkBlog;
    }
}

