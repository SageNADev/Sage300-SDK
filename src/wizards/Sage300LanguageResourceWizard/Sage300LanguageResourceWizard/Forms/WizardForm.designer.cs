using System;

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard
{
    partial class WizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardForm));
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.splitStep = new System.Windows.Forms.SplitContainer();
            this.pnlReview = new System.Windows.Forms.Panel();
            this.lblReview_ContentTemplate = new MetroFramework.Controls.MetroLabel();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.lblWelcome_Content = new MetroFramework.Controls.MetroLabel();
            this.pnlSelectLanguage = new System.Windows.Forms.Panel();
            this.cboLanguage = new MetroFramework.Controls.MetroComboBox();
            this.lblLanguage = new MetroFramework.Controls.MetroLabel();
            this.lblLowerBorder = new System.Windows.Forms.Label();
            this.lblUpperBorder = new System.Windows.Forms.Label();
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
            this.pnlReview.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.pnlSelectLanguage.SuspendLayout();
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
            this.splitBase.BackColor = System.Drawing.Color.Red;
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(27, 74);
            this.splitBase.Margin = new System.Windows.Forms.Padding(4);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.BackColor = System.Drawing.Color.LimeGreen;
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(839, 608);
            this.splitBase.SplitterDistance = 139;
            this.splitBase.SplitterWidth = 5;
            this.splitBase.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(729, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 96);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(9, 41);
            this.lblStepDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(712, 63);
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
            this.lblStepTitle.Location = new System.Drawing.Point(9, 5);
            this.lblStepTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(236, 25);
            this.lblStepTitle.Style = MetroFramework.MetroColorStyle.Green;
            this.lblStepTitle.TabIndex = 6;
            this.lblStepTitle.Text = "This is the title of the step";
            this.lblStepTitle.UseStyleColors = true;
            // 
            // splitSteps
            // 
            this.splitSteps.BackColor = System.Drawing.Color.DeepPink;
            this.splitSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSteps.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSteps.IsSplitterFixed = true;
            this.splitSteps.Location = new System.Drawing.Point(0, 0);
            this.splitSteps.Margin = new System.Windows.Forms.Padding(4);
            this.splitSteps.Name = "splitSteps";
            this.splitSteps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSteps.Panel1
            // 
            this.splitSteps.Panel1.Controls.Add(this.splitStep);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(839, 464);
            this.splitSteps.SplitterDistance = 300;
            this.splitSteps.SplitterWidth = 5;
            this.splitSteps.TabIndex = 0;
            // 
            // splitStep
            // 
            this.splitStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitStep.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitStep.IsSplitterFixed = true;
            this.splitStep.Location = new System.Drawing.Point(0, 0);
            this.splitStep.Margin = new System.Windows.Forms.Padding(4);
            this.splitStep.Name = "splitStep";
            this.splitStep.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitStep.Panel1
            // 
            this.splitStep.Panel1.BackColor = System.Drawing.Color.Aqua;
            this.splitStep.Panel1.Controls.Add(this.pnlReview);
            this.splitStep.Panel1.Controls.Add(this.pnlWelcome);
            this.splitStep.Panel1.Controls.Add(this.pnlSelectLanguage);
            this.splitStep.Panel1.Controls.Add(this.lblLowerBorder);
            this.splitStep.Panel1.Controls.Add(this.lblUpperBorder);
            this.splitStep.Panel1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            // 
            // splitStep.Panel2
            // 
            this.splitStep.Panel2.BackColor = System.Drawing.Color.BlueViolet;
            this.splitStep.Panel2.Controls.Add(this.checkBox);
            this.splitStep.Size = new System.Drawing.Size(839, 300);
            this.splitStep.SplitterDistance = 271;
            this.splitStep.SplitterWidth = 5;
            this.splitStep.TabIndex = 0;
            // 
            // pnlReview
            // 
            this.pnlReview.Controls.Add(this.lblReview_ContentTemplate);
            this.pnlReview.Location = new System.Drawing.Point(473, 162);
            this.pnlReview.Margin = new System.Windows.Forms.Padding(4);
            this.pnlReview.Name = "pnlReview";
            this.pnlReview.Size = new System.Drawing.Size(343, 123);
            this.pnlReview.TabIndex = 4;
            // 
            // lblReview_ContentTemplate
            // 
            this.lblReview_ContentTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReview_ContentTemplate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblReview_ContentTemplate.Location = new System.Drawing.Point(0, 0);
            this.lblReview_ContentTemplate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReview_ContentTemplate.Name = "lblReview_ContentTemplate";
            this.lblReview_ContentTemplate.Size = new System.Drawing.Size(343, 123);
            this.lblReview_ContentTemplate.TabIndex = 5;
            this.lblReview_ContentTemplate.Text = resources.GetString("lblReview_ContentTemplate.Text");
            this.lblReview_ContentTemplate.WrapToLine = true;
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.Controls.Add(this.lblWelcome_Content);
            this.pnlWelcome.Location = new System.Drawing.Point(24, 162);
            this.pnlWelcome.Margin = new System.Windows.Forms.Padding(4);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(244, 123);
            this.pnlWelcome.TabIndex = 4;
            // 
            // lblWelcome_Content
            // 
            this.lblWelcome_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome_Content.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblWelcome_Content.Location = new System.Drawing.Point(0, 0);
            this.lblWelcome_Content.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome_Content.Name = "lblWelcome_Content";
            this.lblWelcome_Content.Size = new System.Drawing.Size(244, 123);
            this.lblWelcome_Content.TabIndex = 2;
            this.lblWelcome_Content.Text = resources.GetString("lblWelcome_Content.Text");
            this.lblWelcome_Content.WrapToLine = true;
            // 
            // pnlSelectLanguage
            // 
            this.pnlSelectLanguage.Controls.Add(this.cboLanguage);
            this.pnlSelectLanguage.Controls.Add(this.lblLanguage);
            this.pnlSelectLanguage.Location = new System.Drawing.Point(301, 162);
            this.pnlSelectLanguage.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSelectLanguage.Name = "pnlSelectLanguage";
            this.pnlSelectLanguage.Size = new System.Drawing.Size(140, 123);
            this.pnlSelectLanguage.TabIndex = 3;
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.ItemHeight = 24;
            this.cboLanguage.Items.AddRange(new object[] {
            "Flat",
            "Process",
            "DynamicQuery",
            "Report",
            "Inquiry",
            "HeaderDetail"});
            this.cboLanguage.Location = new System.Drawing.Point(120, 20);
            this.cboLanguage.Margin = new System.Windows.Forms.Padding(4);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(321, 30);
            this.cboLanguage.TabIndex = 2;
            this.cboLanguage.UseSelectable = true;
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblLanguage.Location = new System.Drawing.Point(16, 23);
            this.lblLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(77, 20);
            this.lblLanguage.TabIndex = 1;
            this.lblLanguage.Text = "Language:";
            // 
            // lblLowerBorder
            // 
            this.lblLowerBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblLowerBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLowerBorder.Location = new System.Drawing.Point(0, 270);
            this.lblLowerBorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerBorder.Name = "lblLowerBorder";
            this.lblLowerBorder.Size = new System.Drawing.Size(839, 1);
            this.lblLowerBorder.TabIndex = 2;
            // 
            // lblUpperBorder
            // 
            this.lblUpperBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblUpperBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpperBorder.Location = new System.Drawing.Point(0, 20);
            this.lblUpperBorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpperBorder.Name = "lblUpperBorder";
            this.lblUpperBorder.Size = new System.Drawing.Size(839, 1);
            this.lblUpperBorder.TabIndex = 1;
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(13, 10);
            this.checkBox.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(205, 17);
            this.checkBox.TabIndex = 0;
            this.checkBox.Text = "Text to be supplied by the step";
            this.checkBox.UseSelectable = true;
            this.checkBox.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.lblProcessing);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(839, 159);
            this.pnlButtons.TabIndex = 0;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblProcessing.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblProcessing.Location = new System.Drawing.Point(12, 12);
            this.lblProcessing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(79, 20);
            this.lblProcessing.TabIndex = 2;
            this.lblProcessing.Text = "Processing";
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(747, 12);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(91, 31);
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
            this.btnBack.Location = new System.Drawing.Point(648, 12);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(91, 31);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseSelectable = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // WizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(893, 707);
            this.Controls.Add(this.splitBase);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardForm";
            this.Padding = new System.Windows.Forms.Padding(27, 74, 27, 25);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Language Resource Wizard";
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
            this.splitStep.Panel2.ResumeLayout(false);
            this.splitStep.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitStep)).EndInit();
            this.splitStep.ResumeLayout(false);
            this.pnlReview.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            this.pnlSelectLanguage.ResumeLayout(false);
            this.pnlSelectLanguage.PerformLayout();
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
        private MetroFramework.Controls.MetroLabel lblProcessing;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUpperBorder;
        private System.Windows.Forms.Label lblLowerBorder;
        private System.Windows.Forms.Panel pnlSelectLanguage;
        private MetroFramework.Controls.MetroComboBox cboLanguage;
        private MetroFramework.Controls.MetroLabel lblLanguage;
        private System.Windows.Forms.Panel pnlWelcome;
        private MetroFramework.Controls.MetroLabel lblWelcome_Content;
        private System.Windows.Forms.Panel pnlReview;
        private MetroFramework.Controls.MetroLabel lblReview_ContentTemplate;
    }
}

