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
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.splitStep = new System.Windows.Forms.SplitContainer();
            this.lblLowerBorder = new System.Windows.Forms.Label();
            this.lblUpperBorder = new System.Windows.Forms.Label();
            this.lblContent = new MetroFramework.Controls.MetroLabel();
            this.checkBox = new MetroFramework.Controls.MetroCheckBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblProcessing = new MetroFramework.Controls.MetroLabel();
            this.btnNext = new MetroFramework.Controls.MetroButton();
            this.btnBack = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStep)).BeginInit();
            this.splitStep.Panel1.SuspendLayout();
            this.splitStep.Panel2.SuspendLayout();
            this.splitStep.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
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
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(630, 507);
            this.splitBase.SplitterDistance = 84;
            this.splitBase.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(547, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(7, 33);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(534, 51);
            this.lblStepDescription.TabIndex = 7;
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
            this.lblStepTitle.Location = new System.Drawing.Point(7, 4);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(234, 25);
            this.lblStepTitle.Style = MetroFramework.MetroColorStyle.Green;
            this.lblStepTitle.TabIndex = 6;
            this.lblStepTitle.Text = "This is the title of the step";
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
            this.splitSteps.Panel1.Controls.Add(this.splitStep);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(630, 419);
            this.splitSteps.SplitterDistance = 375;
            this.splitSteps.TabIndex = 0;
            // 
            // splitStep
            // 
            this.splitStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitStep.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitStep.IsSplitterFixed = true;
            this.splitStep.Location = new System.Drawing.Point(0, 0);
            this.splitStep.Name = "splitStep";
            this.splitStep.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitStep.Panel1
            // 
            this.splitStep.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitStep.Panel1.Controls.Add(this.lblLowerBorder);
            this.splitStep.Panel1.Controls.Add(this.lblUpperBorder);
            this.splitStep.Panel1.Controls.Add(this.lblContent);
            // 
            // splitStep.Panel2
            // 
            this.splitStep.Panel2.Controls.Add(this.checkBox);
            this.splitStep.Size = new System.Drawing.Size(630, 375);
            this.splitStep.SplitterDistance = 301;
            this.splitStep.TabIndex = 0;
            // 
            // lblLowerBorder
            // 
            this.lblLowerBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblLowerBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLowerBorder.Location = new System.Drawing.Point(0, 300);
            this.lblLowerBorder.Name = "lblLowerBorder";
            this.lblLowerBorder.Size = new System.Drawing.Size(630, 1);
            this.lblLowerBorder.TabIndex = 2;
            // 
            // lblUpperBorder
            // 
            this.lblUpperBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblUpperBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpperBorder.Location = new System.Drawing.Point(0, 0);
            this.lblUpperBorder.Name = "lblUpperBorder";
            this.lblUpperBorder.Size = new System.Drawing.Size(630, 1);
            this.lblUpperBorder.TabIndex = 1;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblContent.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblContent.Location = new System.Drawing.Point(7, 8);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(223, 19);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "Content to be supplied by the step";
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.checkBox.Location = new System.Drawing.Point(10, 8);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(213, 19);
            this.checkBox.Style = MetroFramework.MetroColorStyle.Green;
            this.checkBox.TabIndex = 0;
            this.checkBox.Text = "Text to be supplied by the step";
            this.checkBox.UseSelectable = true;
            this.checkBox.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.lblProcessing);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(630, 40);
            this.pnlButtons.TabIndex = 0;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblProcessing.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblProcessing.Location = new System.Drawing.Point(9, 10);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(74, 19);
            this.lblProcessing.TabIndex = 2;
            this.lblProcessing.Text = "Processing";
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(560, 10);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.Style = MetroFramework.MetroColorStyle.Green;
            this.btnNext.TabIndex = 1;
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
            this.btnBack.Location = new System.Drawing.Point(486, 10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseSelectable = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // Upgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(670, 587);
            this.Controls.Add(this.splitBase);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Upgrade";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Solution Upgrade Wizard";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Generation_HelpButtonClicked);
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
            this.splitStep.Panel1.ResumeLayout(false);
            this.splitStep.Panel1.PerformLayout();
            this.splitStep.Panel2.ResumeLayout(false);
            this.splitStep.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStep)).EndInit();
            this.splitStep.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.SplitContainer splitBase;
        private MetroFramework.Controls.MetroLabel lblStepDescription;
        private MetroFramework.Controls.MetroLabel lblStepTitle;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Panel pnlButtons;
        private MetroFramework.Controls.MetroButton btnBack;
        private MetroFramework.Controls.MetroButton btnNext;
        private System.Windows.Forms.SplitContainer splitStep;
        private MetroFramework.Controls.MetroCheckBox checkBox;
        private MetroFramework.Controls.MetroLabel lblContent;
        private MetroFramework.Controls.MetroLabel lblProcessing;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUpperBorder;
        private System.Windows.Forms.Label lblLowerBorder;
    }
}

