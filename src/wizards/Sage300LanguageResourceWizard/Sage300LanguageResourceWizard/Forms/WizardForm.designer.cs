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
            this.lblTopSeparator = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlFinish = new System.Windows.Forms.Panel();
            this.lblFinish_Content = new MetroFramework.Controls.MetroLabel();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.lblWelcome_Content = new MetroFramework.Controls.MetroLabel();
            this.pnlReview = new System.Windows.Forms.Panel();
            this.lblReview_ContentTemplate = new MetroFramework.Controls.MetroLabel();
            this.pnlSelectLanguage = new System.Windows.Forms.Panel();
            this.cboLanguage = new MetroFramework.Controls.MetroComboBox();
            this.lblLanguage = new MetroFramework.Controls.MetroLabel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.progressBar = new MetroFramework.Controls.MetroProgressBar();
            this.lblBottomSeparator = new System.Windows.Forms.Label();
            this.lblStatus = new MetroFramework.Controls.MetroLabel();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
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
            this.pnlFinish.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.pnlReview.SuspendLayout();
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
            this.splitBase.Location = new System.Drawing.Point(20, 60);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitBase.Panel1.Controls.Add(this.lblTopSeparator);
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(629, 606);
            this.splitBase.SplitterDistance = 99;
            this.splitBase.SplitterWidth = 1;
            this.splitBase.TabIndex = 9;
            // 
            // lblTopSeparator
            // 
            this.lblTopSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTopSeparator.Location = new System.Drawing.Point(2, 85);
            this.lblTopSeparator.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTopSeparator.Name = "lblTopSeparator";
            this.lblTopSeparator.Size = new System.Drawing.Size(623, 2);
            this.lblTopSeparator.TabIndex = 9;
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
            this.lblStepDescription.Location = new System.Drawing.Point(7, 32);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(534, 44);
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
            this.splitSteps.BackColor = System.Drawing.Color.DeepPink;
            this.splitSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSteps.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSteps.IsSplitterFixed = true;
            this.splitSteps.Location = new System.Drawing.Point(0, 0);
            this.splitSteps.Name = "splitSteps";
            this.splitSteps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSteps.Panel1
            // 
            this.splitSteps.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitSteps.Panel1.Controls.Add(this.pnlFinish);
            this.splitSteps.Panel1.Controls.Add(this.pnlWelcome);
            this.splitSteps.Panel1.Controls.Add(this.pnlReview);
            this.splitSteps.Panel1.Controls.Add(this.pnlSelectLanguage);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(629, 506);
            this.splitSteps.SplitterDistance = 300;
            this.splitSteps.TabIndex = 0;
            // 
            // pnlFinish
            // 
            this.pnlFinish.Controls.Add(this.lblFinish_Content);
            this.pnlFinish.Location = new System.Drawing.Point(21, 109);
            this.pnlFinish.Name = "pnlFinish";
            this.pnlFinish.Size = new System.Drawing.Size(180, 58);
            this.pnlFinish.TabIndex = 6;
            // 
            // lblFinish_Content
            // 
            this.lblFinish_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinish_Content.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblFinish_Content.Location = new System.Drawing.Point(0, 0);
            this.lblFinish_Content.Name = "lblFinish_Content";
            this.lblFinish_Content.Size = new System.Drawing.Size(180, 58);
            this.lblFinish_Content.TabIndex = 5;
            this.lblFinish_Content.Text = "The creation of the new language resources has completed successfully.\r\n\r\n";
            this.lblFinish_Content.WrapToLine = true;
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.Controls.Add(this.lblWelcome_Content);
            this.pnlWelcome.Location = new System.Drawing.Point(18, 21);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(183, 63);
            this.pnlWelcome.TabIndex = 4;
            // 
            // lblWelcome_Content
            // 
            this.lblWelcome_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome_Content.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblWelcome_Content.Location = new System.Drawing.Point(0, 0);
            this.lblWelcome_Content.Name = "lblWelcome_Content";
            this.lblWelcome_Content.Size = new System.Drawing.Size(183, 63);
            this.lblWelcome_Content.TabIndex = 2;
            this.lblWelcome_Content.Text = resources.GetString("lblWelcome_Content.Text");
            this.lblWelcome_Content.WrapToLine = true;
            // 
            // pnlReview
            // 
            this.pnlReview.Controls.Add(this.lblReview_ContentTemplate);
            this.pnlReview.Location = new System.Drawing.Point(362, 25);
            this.pnlReview.Name = "pnlReview";
            this.pnlReview.Size = new System.Drawing.Size(195, 168);
            this.pnlReview.TabIndex = 4;
            // 
            // lblReview_ContentTemplate
            // 
            this.lblReview_ContentTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReview_ContentTemplate.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblReview_ContentTemplate.Location = new System.Drawing.Point(0, 0);
            this.lblReview_ContentTemplate.Name = "lblReview_ContentTemplate";
            this.lblReview_ContentTemplate.Size = new System.Drawing.Size(195, 168);
            this.lblReview_ContentTemplate.TabIndex = 5;
            this.lblReview_ContentTemplate.Text = resources.GetString("lblReview_ContentTemplate.Text");
            this.lblReview_ContentTemplate.WrapToLine = true;
            // 
            // pnlSelectLanguage
            // 
            this.pnlSelectLanguage.Controls.Add(this.cboLanguage);
            this.pnlSelectLanguage.Controls.Add(this.lblLanguage);
            this.pnlSelectLanguage.Location = new System.Drawing.Point(226, 25);
            this.pnlSelectLanguage.Name = "pnlSelectLanguage";
            this.pnlSelectLanguage.Size = new System.Drawing.Size(105, 59);
            this.pnlSelectLanguage.TabIndex = 3;
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.ItemHeight = 23;
            this.cboLanguage.Items.AddRange(new object[] {
            "Flat",
            "Process",
            "DynamicQuery",
            "Report",
            "Inquiry",
            "HeaderDetail"});
            this.cboLanguage.Location = new System.Drawing.Point(90, 16);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(242, 29);
            this.cboLanguage.Style = MetroFramework.MetroColorStyle.Green;
            this.cboLanguage.TabIndex = 2;
            this.cboLanguage.UseSelectable = true;
            this.cboLanguage.UseStyleColors = true;
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblLanguage.Location = new System.Drawing.Point(12, 19);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(72, 19);
            this.lblLanguage.TabIndex = 1;
            this.lblLanguage.Text = "Language:";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.Controls.Add(this.progressBar);
            this.pnlButtons.Controls.Add(this.lblBottomSeparator);
            this.pnlButtons.Controls.Add(this.lblStatus);
            this.pnlButtons.Controls.Add(this.textBoxStatus);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Location = new System.Drawing.Point(0, -2);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(628, 205);
            this.pnlButtons.TabIndex = 0;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(62, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(563, 16);
            this.progressBar.Style = MetroFramework.MetroColorStyle.Green;
            this.progressBar.TabIndex = 6;
            // 
            // lblBottomSeparator
            // 
            this.lblBottomSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblBottomSeparator.Location = new System.Drawing.Point(2, 3);
            this.lblBottomSeparator.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBottomSeparator.Name = "lblBottomSeparator";
            this.lblBottomSeparator.Size = new System.Drawing.Size(623, 2);
            this.lblBottomSeparator.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStatus.Location = new System.Drawing.Point(8, 8);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 19);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBoxStatus.Location = new System.Drawing.Point(7, 32);
            this.textBoxStatus.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(619, 136);
            this.textBoxStatus.TabIndex = 3;
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(557, 173);
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
            this.btnBack.Location = new System.Drawing.Point(483, 173);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Back";
            this.btnBack.UseSelectable = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // WizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(669, 686);
            this.Controls.Add(this.splitBase);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardForm";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Language Resource Wizard";
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
            this.pnlFinish.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            this.pnlReview.ResumeLayout(false);
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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlSelectLanguage;
        private MetroFramework.Controls.MetroComboBox cboLanguage;
        private MetroFramework.Controls.MetroLabel lblLanguage;
        private System.Windows.Forms.Panel pnlWelcome;
        private MetroFramework.Controls.MetroLabel lblWelcome_Content;
        private System.Windows.Forms.Panel pnlReview;
        private MetroFramework.Controls.MetroLabel lblReview_ContentTemplate;
        private System.Windows.Forms.TextBox textBoxStatus;
        private MetroFramework.Controls.MetroLabel lblStatus;
        private System.Windows.Forms.Panel pnlFinish;
        private MetroFramework.Controls.MetroLabel lblFinish_Content;
        private System.Windows.Forms.Label lblBottomSeparator;
        private System.Windows.Forms.Label lblTopSeparator;
        private MetroFramework.Controls.MetroProgressBar progressBar;
    }
}

