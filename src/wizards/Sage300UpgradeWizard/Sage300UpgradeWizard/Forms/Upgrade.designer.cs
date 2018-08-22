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
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.splitStep = new System.Windows.Forms.SplitContainer();
            this.lblContent = new System.Windows.Forms.Label();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
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
            this.splitBase.Size = new System.Drawing.Size(459, 517);
            this.splitBase.SplitterDistance = 88;
            this.splitBase.TabIndex = 9;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 31);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(346, 38);
            this.lblStepDescription.TabIndex = 7;
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
            this.lblStepTitle.TabIndex = 6;
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
            this.splitSteps.Panel1.Controls.Add(this.splitStep);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(459, 425);
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
            this.splitStep.Panel1.Controls.Add(this.lblContent);
            // 
            // splitStep.Panel2
            // 
            this.splitStep.Panel2.Controls.Add(this.checkBox);
            this.splitStep.Size = new System.Drawing.Size(459, 375);
            this.splitStep.SplitterDistance = 320;
            this.splitStep.TabIndex = 0;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContent.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblContent.Location = new System.Drawing.Point(0, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Padding = new System.Windows.Forms.Padding(1);
            this.lblContent.Size = new System.Drawing.Size(189, 15);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "Content to be supplied by the step";
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(10, 8);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(171, 17);
            this.checkBox.TabIndex = 0;
            this.checkBox.Text = "Text to be supplied by the step";
            this.checkBox.UseVisualStyleBackColor = true;
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
            this.pnlButtons.Size = new System.Drawing.Size(459, 46);
            this.pnlButtons.TabIndex = 0;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessing.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblProcessing.Location = new System.Drawing.Point(9, 16);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(62, 13);
            this.lblProcessing.TabIndex = 2;
            this.lblProcessing.Text = "Processing";
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(379, 10);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(305, 10);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // Upgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 517);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Upgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solution Upgrade";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Generation_HelpButtonClicked);
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.SplitContainer splitStep;
        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label lblProcessing;
    }
}

