// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Sage300UICustomizationWizard
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
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblUpperBorder = new System.Windows.Forms.Label();
            this.lblStepDescription = new MetroFramework.Controls.MetroLabel();
            this.lblStepTitle = new MetroFramework.Controls.MetroLabel();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlKendo = new System.Windows.Forms.Panel();
            this.lblKendoVersionHelp = new MetroFramework.Controls.MetroLabel();
            this.lblKendoLink = new System.Windows.Forms.LinkLabel();
            this.lblKendoFolderHelp = new MetroFramework.Controls.MetroLabel();
            this.txtKendoFolder = new MetroFramework.Controls.MetroTextBox();
            this.chkKendoLicense = new MetroFramework.Controls.MetroCheckBox();
            this.pnlCreateEdit = new System.Windows.Forms.Panel();
            this.txtModule = new MetroFramework.Controls.MetroTextBox();
            this.lblModule = new MetroFramework.Controls.MetroLabel();
            this.txtProject = new MetroFramework.Controls.MetroTextBox();
            this.lblProject = new MetroFramework.Controls.MetroLabel();
            this.txtEula = new MetroFramework.Controls.MetroTextBox();
            this.lblEula = new MetroFramework.Controls.MetroLabel();
            this.txtAssembly = new MetroFramework.Controls.MetroTextBox();
            this.lblAssembly = new MetroFramework.Controls.MetroLabel();
            this.txtBootstrapper = new MetroFramework.Controls.MetroTextBox();
            this.lblBootstrapper = new MetroFramework.Controls.MetroLabel();
            this.txtVersion = new MetroFramework.Controls.MetroTextBox();
            this.txtCompatibility = new MetroFramework.Controls.MetroTextBox();
            this.lblVersion = new MetroFramework.Controls.MetroLabel();
            this.lblCompatibility = new MetroFramework.Controls.MetroLabel();
            this.txtCompanyName = new MetroFramework.Controls.MetroTextBox();
            this.txtCustomizationDescription = new MetroFramework.Controls.MetroTextBox();
            this.txtCustomizationName = new MetroFramework.Controls.MetroTextBox();
            this.lblCompanyName = new MetroFramework.Controls.MetroLabel();
            this.lblCustomizationDescription = new MetroFramework.Controls.MetroLabel();
            this.lblCustomizationName = new MetroFramework.Controls.MetroLabel();
            this.txtFolderName = new MetroFramework.Controls.MetroTextBox();
            this.lblFolder = new MetroFramework.Controls.MetroLabel();
            this.txtPackageId = new MetroFramework.Controls.MetroTextBox();
            this.lblPackageId = new MetroFramework.Controls.MetroLabel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.lblLowerBorder = new System.Windows.Forms.Label();
            this.btnBack = new MetroFramework.Controls.MetroButton();
            this.btnNext = new MetroFramework.Controls.MetroButton();
            this.lblProcessingFile = new MetroFramework.Controls.MetroLabel();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSteps)).BeginInit();
            this.splitSteps.Panel1.SuspendLayout();
            this.splitSteps.Panel2.SuspendLayout();
            this.splitSteps.SuspendLayout();
            this.pnlKendo.SuspendLayout();
            this.pnlCreateEdit.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.IsSplitterFixed = true;
            this.splitBase.Location = new System.Drawing.Point(15, 60);
            this.splitBase.Name = "splitBase";
            this.splitBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel1.Controls.Add(this.pictureBox1);
            this.splitBase.Panel1.Controls.Add(this.lblUpperBorder);
            this.splitBase.Panel1.Controls.Add(this.lblStepDescription);
            this.splitBase.Panel1.Controls.Add(this.lblStepTitle);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.BackColor = System.Drawing.Color.White;
            this.splitBase.Panel2.Controls.Add(this.splitSteps);
            this.splitBase.Size = new System.Drawing.Size(952, 637);
            this.splitBase.SplitterDistance = 98;
            this.splitBase.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sage300UICustomizationWizard.Properties.Resources.sage_logo_square;
            this.pictureBox1.Location = new System.Drawing.Point(878, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(69, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // lblUpperBorder
            // 
            this.lblUpperBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblUpperBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblUpperBorder.Location = new System.Drawing.Point(0, 97);
            this.lblUpperBorder.Name = "lblUpperBorder";
            this.lblUpperBorder.Size = new System.Drawing.Size(952, 1);
            this.lblUpperBorder.TabIndex = 27;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(842, 37);
            this.lblStepDescription.TabIndex = 15;
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
            this.lblStepTitle.Location = new System.Drawing.Point(12, 9);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(234, 25);
            this.lblStepTitle.Style = MetroFramework.MetroColorStyle.Green;
            this.lblStepTitle.TabIndex = 14;
            this.lblStepTitle.Text = "This is the title of the step";
            this.lblStepTitle.UseStyleColors = true;
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
            this.splitSteps.Panel1.BackColor = System.Drawing.Color.White;
            this.splitSteps.Panel1.Controls.Add(this.pnlKendo);
            this.splitSteps.Panel1.Controls.Add(this.pnlCreateEdit);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.BackColor = System.Drawing.Color.White;
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(952, 535);
            this.splitSteps.SplitterDistance = 450;
            this.splitSteps.TabIndex = 0;
            // 
            // pnlKendo
            // 
            this.pnlKendo.Controls.Add(this.lblKendoVersionHelp);
            this.pnlKendo.Controls.Add(this.lblKendoLink);
            this.pnlKendo.Controls.Add(this.lblKendoFolderHelp);
            this.pnlKendo.Controls.Add(this.txtKendoFolder);
            this.pnlKendo.Controls.Add(this.chkKendoLicense);
            this.pnlKendo.Location = new System.Drawing.Point(643, 16);
            this.pnlKendo.Margin = new System.Windows.Forms.Padding(2);
            this.pnlKendo.Name = "pnlKendo";
            this.pnlKendo.Size = new System.Drawing.Size(258, 379);
            this.pnlKendo.TabIndex = 26;
            // 
            // lblKendoVersionHelp
            // 
            this.lblKendoVersionHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblKendoVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoVersionHelp.Location = new System.Drawing.Point(40, 156);
            this.lblKendoVersionHelp.Name = "lblKendoVersionHelp";
            this.lblKendoVersionHelp.Size = new System.Drawing.Size(428, 36);
            this.lblKendoVersionHelp.TabIndex = 13;
            this.lblKendoVersionHelp.Text = "The Kendo UI version used in these projects is v2019.1.115";
            this.lblKendoVersionHelp.WrapToLine = true;
            // 
            // lblKendoLink
            // 
            this.lblKendoLink.AutoSize = true;
            this.lblKendoLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKendoLink.Location = new System.Drawing.Point(40, 127);
            this.lblKendoLink.Name = "lblKendoLink";
            this.lblKendoLink.Size = new System.Drawing.Size(166, 16);
            this.lblKendoLink.TabIndex = 12;
            this.lblKendoLink.TabStop = true;
            this.lblKendoLink.Text = "Kendo License Agreement";
            this.lblKendoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblKendoLink_LinkClicked);
            // 
            // lblKendoFolderHelp
            // 
            this.lblKendoFolderHelp.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblKendoFolderHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoFolderHelp.Location = new System.Drawing.Point(40, 79);
            this.lblKendoFolderHelp.Name = "lblKendoFolderHelp";
            this.lblKendoFolderHelp.Size = new System.Drawing.Size(428, 50);
            this.lblKendoFolderHelp.TabIndex = 11;
            this.lblKendoFolderHelp.Text = "The Kendo UI Commercial License may be obtained by clicking the link below:";
            this.lblKendoFolderHelp.WrapToLine = true;
            // 
            // txtKendoFolder
            // 
            // 
            // 
            // 
            this.txtKendoFolder.CustomButton.Image = null;
            this.txtKendoFolder.CustomButton.Location = new System.Drawing.Point(278, 1);
            this.txtKendoFolder.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtKendoFolder.CustomButton.Name = "";
            this.txtKendoFolder.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtKendoFolder.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtKendoFolder.CustomButton.TabIndex = 1;
            this.txtKendoFolder.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtKendoFolder.CustomButton.UseSelectable = true;
            this.txtKendoFolder.Enabled = false;
            this.txtKendoFolder.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtKendoFolder.Lines = new string[0];
            this.txtKendoFolder.Location = new System.Drawing.Point(40, 42);
            this.txtKendoFolder.MaxLength = 32767;
            this.txtKendoFolder.Name = "txtKendoFolder";
            this.txtKendoFolder.PasswordChar = '\0';
            this.txtKendoFolder.PromptText = "Kendo Folder";
            this.txtKendoFolder.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtKendoFolder.SelectedText = "";
            this.txtKendoFolder.SelectionLength = 0;
            this.txtKendoFolder.SelectionStart = 0;
            this.txtKendoFolder.ShortcutsEnabled = true;
            this.txtKendoFolder.ShowButton = true;
            this.txtKendoFolder.ShowClearButton = true;
            this.txtKendoFolder.Size = new System.Drawing.Size(302, 25);
            this.txtKendoFolder.Style = MetroFramework.MetroColorStyle.Green;
            this.txtKendoFolder.TabIndex = 9;
            this.txtKendoFolder.UseSelectable = true;
            this.txtKendoFolder.WaterMark = "Kendo Folder";
            this.txtKendoFolder.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtKendoFolder.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtKendoFolder.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.btnKendoDialog_Click);
            // 
            // chkKendoLicense
            // 
            this.chkKendoLicense.AutoSize = true;
            this.chkKendoLicense.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.chkKendoLicense.Location = new System.Drawing.Point(11, 15);
            this.chkKendoLicense.Name = "chkKendoLicense";
            this.chkKendoLicense.Size = new System.Drawing.Size(279, 19);
            this.chkKendoLicense.Style = MetroFramework.MetroColorStyle.Green;
            this.chkKendoLicense.TabIndex = 7;
            this.chkKendoLicense.Text = "Purchased Kendo UI Commercial License?";
            this.chkKendoLicense.UseCustomForeColor = true;
            this.chkKendoLicense.UseSelectable = true;
            this.chkKendoLicense.UseStyleColors = true;
            this.chkKendoLicense.CheckedChanged += new System.EventHandler(this.chkKendoLicense_CheckedChanged);
            // 
            // pnlCreateEdit
            // 
            this.pnlCreateEdit.Controls.Add(this.txtModule);
            this.pnlCreateEdit.Controls.Add(this.lblModule);
            this.pnlCreateEdit.Controls.Add(this.txtProject);
            this.pnlCreateEdit.Controls.Add(this.lblProject);
            this.pnlCreateEdit.Controls.Add(this.txtEula);
            this.pnlCreateEdit.Controls.Add(this.lblEula);
            this.pnlCreateEdit.Controls.Add(this.txtAssembly);
            this.pnlCreateEdit.Controls.Add(this.lblAssembly);
            this.pnlCreateEdit.Controls.Add(this.txtBootstrapper);
            this.pnlCreateEdit.Controls.Add(this.lblBootstrapper);
            this.pnlCreateEdit.Controls.Add(this.txtVersion);
            this.pnlCreateEdit.Controls.Add(this.txtCompatibility);
            this.pnlCreateEdit.Controls.Add(this.lblVersion);
            this.pnlCreateEdit.Controls.Add(this.lblCompatibility);
            this.pnlCreateEdit.Controls.Add(this.txtCompanyName);
            this.pnlCreateEdit.Controls.Add(this.txtCustomizationDescription);
            this.pnlCreateEdit.Controls.Add(this.txtCustomizationName);
            this.pnlCreateEdit.Controls.Add(this.lblCompanyName);
            this.pnlCreateEdit.Controls.Add(this.lblCustomizationDescription);
            this.pnlCreateEdit.Controls.Add(this.lblCustomizationName);
            this.pnlCreateEdit.Controls.Add(this.txtFolderName);
            this.pnlCreateEdit.Controls.Add(this.lblFolder);
            this.pnlCreateEdit.Controls.Add(this.txtPackageId);
            this.pnlCreateEdit.Controls.Add(this.lblPackageId);
            this.pnlCreateEdit.Location = new System.Drawing.Point(12, 16);
            this.pnlCreateEdit.Name = "pnlCreateEdit";
            this.pnlCreateEdit.Size = new System.Drawing.Size(604, 387);
            this.pnlCreateEdit.TabIndex = 1;
            // 
            // txtModule
            // 
            // 
            // 
            // 
            this.txtModule.CustomButton.Image = null;
            this.txtModule.CustomButton.Location = new System.Drawing.Point(33, 1);
            this.txtModule.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtModule.CustomButton.Name = "";
            this.txtModule.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtModule.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtModule.CustomButton.TabIndex = 1;
            this.txtModule.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtModule.CustomButton.UseSelectable = true;
            this.txtModule.CustomButton.Visible = false;
            this.txtModule.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtModule.Lines = new string[0];
            this.txtModule.Location = new System.Drawing.Point(127, 257);
            this.txtModule.MaxLength = 2;
            this.txtModule.Name = "txtModule";
            this.txtModule.PasswordChar = '\0';
            this.txtModule.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtModule.SelectedText = "";
            this.txtModule.SelectionLength = 0;
            this.txtModule.SelectionStart = 0;
            this.txtModule.ShortcutsEnabled = true;
            this.txtModule.Size = new System.Drawing.Size(57, 25);
            this.txtModule.Style = MetroFramework.MetroColorStyle.Green;
            this.txtModule.TabIndex = 18;
            this.txtModule.UseSelectable = true;
            this.txtModule.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtModule.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblModule.Location = new System.Drawing.Point(58, 258);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(59, 19);
            this.lblModule.TabIndex = 17;
            this.lblModule.Text = "Module:";
            // 
            // txtProject
            // 
            // 
            // 
            // 
            this.txtProject.CustomButton.Image = null;
            this.txtProject.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtProject.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtProject.CustomButton.Name = "";
            this.txtProject.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtProject.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtProject.CustomButton.TabIndex = 1;
            this.txtProject.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtProject.CustomButton.UseSelectable = true;
            this.txtProject.CustomButton.Visible = false;
            this.txtProject.Lines = new string[0];
            this.txtProject.Location = new System.Drawing.Point(127, 286);
            this.txtProject.MaxLength = 32767;
            this.txtProject.Name = "txtProject";
            this.txtProject.PasswordChar = '\0';
            this.txtProject.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtProject.SelectedText = "";
            this.txtProject.SelectionLength = 0;
            this.txtProject.SelectionStart = 0;
            this.txtProject.ShortcutsEnabled = true;
            this.txtProject.Size = new System.Drawing.Size(315, 25);
            this.txtProject.Style = MetroFramework.MetroColorStyle.Green;
            this.txtProject.TabIndex = 20;
            this.txtProject.UseSelectable = true;
            this.txtProject.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtProject.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtProject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProject_KeyPress);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblProject.Location = new System.Drawing.Point(63, 287);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(54, 19);
            this.lblProject.TabIndex = 19;
            this.lblProject.Text = "Project:";
            // 
            // txtEula
            // 
            this.txtEula.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtEula.CustomButton.Image = null;
            this.txtEula.CustomButton.Location = new System.Drawing.Point(462, 1);
            this.txtEula.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtEula.CustomButton.Name = "";
            this.txtEula.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtEula.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtEula.CustomButton.TabIndex = 1;
            this.txtEula.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtEula.CustomButton.UseSelectable = true;
            this.txtEula.CustomButton.Visible = false;
            this.txtEula.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtEula.Lines = new string[0];
            this.txtEula.Location = new System.Drawing.Point(127, 220);
            this.txtEula.MaxLength = 32767;
            this.txtEula.Name = "txtEula";
            this.txtEula.PasswordChar = '\0';
            this.txtEula.ReadOnly = true;
            this.txtEula.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtEula.SelectedText = "";
            this.txtEula.SelectionLength = 0;
            this.txtEula.SelectionStart = 0;
            this.txtEula.ShortcutsEnabled = true;
            this.txtEula.Size = new System.Drawing.Size(486, 25);
            this.txtEula.Style = MetroFramework.MetroColorStyle.Green;
            this.txtEula.TabIndex = 16;
            this.txtEula.UseSelectable = true;
            this.txtEula.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtEula.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblEula
            // 
            this.lblEula.AutoSize = true;
            this.lblEula.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEula.Location = new System.Drawing.Point(72, 222);
            this.lblEula.Name = "lblEula";
            this.lblEula.Size = new System.Drawing.Size(45, 19);
            this.lblEula.TabIndex = 15;
            this.lblEula.Text = "EULA:";
            // 
            // txtAssembly
            // 
            this.txtAssembly.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtAssembly.CustomButton.Image = null;
            this.txtAssembly.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtAssembly.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtAssembly.CustomButton.Name = "";
            this.txtAssembly.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtAssembly.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtAssembly.CustomButton.TabIndex = 1;
            this.txtAssembly.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtAssembly.CustomButton.UseSelectable = true;
            this.txtAssembly.CustomButton.Visible = false;
            this.txtAssembly.Lines = new string[0];
            this.txtAssembly.Location = new System.Drawing.Point(127, 353);
            this.txtAssembly.MaxLength = 32767;
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.PasswordChar = '\0';
            this.txtAssembly.ReadOnly = true;
            this.txtAssembly.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAssembly.SelectedText = "";
            this.txtAssembly.SelectionLength = 0;
            this.txtAssembly.SelectionStart = 0;
            this.txtAssembly.ShortcutsEnabled = true;
            this.txtAssembly.Size = new System.Drawing.Size(315, 25);
            this.txtAssembly.Style = MetroFramework.MetroColorStyle.Green;
            this.txtAssembly.TabIndex = 24;
            this.txtAssembly.UseSelectable = true;
            this.txtAssembly.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtAssembly.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblAssembly
            // 
            this.lblAssembly.AutoSize = true;
            this.lblAssembly.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblAssembly.Location = new System.Drawing.Point(47, 353);
            this.lblAssembly.Name = "lblAssembly";
            this.lblAssembly.Size = new System.Drawing.Size(70, 19);
            this.lblAssembly.TabIndex = 23;
            this.lblAssembly.Text = "Assembly:";
            // 
            // txtBootstrapper
            // 
            this.txtBootstrapper.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtBootstrapper.CustomButton.Image = null;
            this.txtBootstrapper.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtBootstrapper.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtBootstrapper.CustomButton.Name = "";
            this.txtBootstrapper.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtBootstrapper.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtBootstrapper.CustomButton.TabIndex = 1;
            this.txtBootstrapper.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtBootstrapper.CustomButton.UseSelectable = true;
            this.txtBootstrapper.CustomButton.Visible = false;
            this.txtBootstrapper.Lines = new string[0];
            this.txtBootstrapper.Location = new System.Drawing.Point(127, 324);
            this.txtBootstrapper.MaxLength = 32767;
            this.txtBootstrapper.Name = "txtBootstrapper";
            this.txtBootstrapper.PasswordChar = '\0';
            this.txtBootstrapper.ReadOnly = true;
            this.txtBootstrapper.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBootstrapper.SelectedText = "";
            this.txtBootstrapper.SelectionLength = 0;
            this.txtBootstrapper.SelectionStart = 0;
            this.txtBootstrapper.ShortcutsEnabled = true;
            this.txtBootstrapper.Size = new System.Drawing.Size(315, 25);
            this.txtBootstrapper.Style = MetroFramework.MetroColorStyle.Green;
            this.txtBootstrapper.TabIndex = 22;
            this.txtBootstrapper.UseSelectable = true;
            this.txtBootstrapper.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtBootstrapper.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblBootstrapper
            // 
            this.lblBootstrapper.AutoSize = true;
            this.lblBootstrapper.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblBootstrapper.Location = new System.Drawing.Point(30, 327);
            this.lblBootstrapper.Name = "lblBootstrapper";
            this.lblBootstrapper.Size = new System.Drawing.Size(87, 19);
            this.lblBootstrapper.TabIndex = 21;
            this.lblBootstrapper.Text = "Boostrapper:";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtVersion.CustomButton.Image = null;
            this.txtVersion.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtVersion.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtVersion.CustomButton.Name = "";
            this.txtVersion.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtVersion.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtVersion.CustomButton.TabIndex = 1;
            this.txtVersion.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtVersion.CustomButton.UseSelectable = true;
            this.txtVersion.CustomButton.Visible = false;
            this.txtVersion.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtVersion.Lines = new string[0];
            this.txtVersion.Location = new System.Drawing.Point(127, 191);
            this.txtVersion.MaxLength = 18;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.PasswordChar = '\0';
            this.txtVersion.ReadOnly = true;
            this.txtVersion.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtVersion.SelectedText = "";
            this.txtVersion.SelectionLength = 0;
            this.txtVersion.SelectionStart = 0;
            this.txtVersion.ShortcutsEnabled = true;
            this.txtVersion.Size = new System.Drawing.Size(315, 25);
            this.txtVersion.Style = MetroFramework.MetroColorStyle.Green;
            this.txtVersion.TabIndex = 14;
            this.txtVersion.UseSelectable = true;
            this.txtVersion.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtVersion.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtCompatibility
            // 
            this.txtCompatibility.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtCompatibility.CustomButton.Image = null;
            this.txtCompatibility.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtCompatibility.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtCompatibility.CustomButton.Name = "";
            this.txtCompatibility.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtCompatibility.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCompatibility.CustomButton.TabIndex = 1;
            this.txtCompatibility.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCompatibility.CustomButton.UseSelectable = true;
            this.txtCompatibility.CustomButton.Visible = false;
            this.txtCompatibility.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCompatibility.Lines = new string[0];
            this.txtCompatibility.Location = new System.Drawing.Point(127, 162);
            this.txtCompatibility.MaxLength = 60;
            this.txtCompatibility.Name = "txtCompatibility";
            this.txtCompatibility.PasswordChar = '\0';
            this.txtCompatibility.ReadOnly = true;
            this.txtCompatibility.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCompatibility.SelectedText = "";
            this.txtCompatibility.SelectionLength = 0;
            this.txtCompatibility.SelectionStart = 0;
            this.txtCompatibility.ShortcutsEnabled = true;
            this.txtCompatibility.Size = new System.Drawing.Size(315, 25);
            this.txtCompatibility.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCompatibility.TabIndex = 12;
            this.txtCompatibility.UseSelectable = true;
            this.txtCompatibility.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCompatibility.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblVersion.Location = new System.Drawing.Point(60, 189);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(57, 19);
            this.lblVersion.TabIndex = 13;
            this.lblVersion.Text = "Version:";
            // 
            // lblCompatibility
            // 
            this.lblCompatibility.AutoSize = true;
            this.lblCompatibility.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCompatibility.Location = new System.Drawing.Point(24, 161);
            this.lblCompatibility.Name = "lblCompatibility";
            this.lblCompatibility.Size = new System.Drawing.Size(93, 19);
            this.lblCompatibility.TabIndex = 11;
            this.lblCompatibility.Text = "Compatibility:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtCompanyName.CustomButton.Image = null;
            this.txtCompanyName.CustomButton.Location = new System.Drawing.Point(462, 1);
            this.txtCompanyName.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtCompanyName.CustomButton.Name = "";
            this.txtCompanyName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtCompanyName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCompanyName.CustomButton.TabIndex = 1;
            this.txtCompanyName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCompanyName.CustomButton.UseSelectable = true;
            this.txtCompanyName.CustomButton.Visible = false;
            this.txtCompanyName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCompanyName.Lines = new string[0];
            this.txtCompanyName.Location = new System.Drawing.Point(127, 133);
            this.txtCompanyName.MaxLength = 60;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.PasswordChar = '\0';
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCompanyName.SelectedText = "";
            this.txtCompanyName.SelectionLength = 0;
            this.txtCompanyName.SelectionStart = 0;
            this.txtCompanyName.ShortcutsEnabled = true;
            this.txtCompanyName.Size = new System.Drawing.Size(486, 25);
            this.txtCompanyName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCompanyName.TabIndex = 10;
            this.txtCompanyName.UseSelectable = true;
            this.txtCompanyName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCompanyName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtCustomizationDescription
            // 
            this.txtCustomizationDescription.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtCustomizationDescription.CustomButton.Image = null;
            this.txtCustomizationDescription.CustomButton.Location = new System.Drawing.Point(462, 1);
            this.txtCustomizationDescription.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomizationDescription.CustomButton.Name = "";
            this.txtCustomizationDescription.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtCustomizationDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCustomizationDescription.CustomButton.TabIndex = 1;
            this.txtCustomizationDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCustomizationDescription.CustomButton.UseSelectable = true;
            this.txtCustomizationDescription.CustomButton.Visible = false;
            this.txtCustomizationDescription.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCustomizationDescription.Lines = new string[0];
            this.txtCustomizationDescription.Location = new System.Drawing.Point(127, 104);
            this.txtCustomizationDescription.MaxLength = 255;
            this.txtCustomizationDescription.Name = "txtCustomizationDescription";
            this.txtCustomizationDescription.PasswordChar = '\0';
            this.txtCustomizationDescription.ReadOnly = true;
            this.txtCustomizationDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCustomizationDescription.SelectedText = "";
            this.txtCustomizationDescription.SelectionLength = 0;
            this.txtCustomizationDescription.SelectionStart = 0;
            this.txtCustomizationDescription.ShortcutsEnabled = true;
            this.txtCustomizationDescription.Size = new System.Drawing.Size(486, 25);
            this.txtCustomizationDescription.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCustomizationDescription.TabIndex = 8;
            this.txtCustomizationDescription.UseSelectable = true;
            this.txtCustomizationDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCustomizationDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtCustomizationName
            // 
            this.txtCustomizationName.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtCustomizationName.CustomButton.Image = null;
            this.txtCustomizationName.CustomButton.Location = new System.Drawing.Point(462, 1);
            this.txtCustomizationName.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomizationName.CustomButton.Name = "";
            this.txtCustomizationName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtCustomizationName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCustomizationName.CustomButton.TabIndex = 1;
            this.txtCustomizationName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCustomizationName.CustomButton.UseSelectable = true;
            this.txtCustomizationName.CustomButton.Visible = false;
            this.txtCustomizationName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCustomizationName.Lines = new string[0];
            this.txtCustomizationName.Location = new System.Drawing.Point(127, 75);
            this.txtCustomizationName.MaxLength = 60;
            this.txtCustomizationName.Name = "txtCustomizationName";
            this.txtCustomizationName.PasswordChar = '\0';
            this.txtCustomizationName.ReadOnly = true;
            this.txtCustomizationName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCustomizationName.SelectedText = "";
            this.txtCustomizationName.SelectionLength = 0;
            this.txtCustomizationName.SelectionStart = 0;
            this.txtCustomizationName.ShortcutsEnabled = true;
            this.txtCustomizationName.Size = new System.Drawing.Size(486, 25);
            this.txtCustomizationName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCustomizationName.TabIndex = 6;
            this.txtCustomizationName.UseSelectable = true;
            this.txtCustomizationName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCustomizationName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCompanyName.Location = new System.Drawing.Point(6, 133);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(111, 19);
            this.lblCompanyName.TabIndex = 9;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblCustomizationDescription
            // 
            this.lblCustomizationDescription.AutoSize = true;
            this.lblCustomizationDescription.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCustomizationDescription.Location = new System.Drawing.Point(36, 104);
            this.lblCustomizationDescription.Name = "lblCustomizationDescription";
            this.lblCustomizationDescription.Size = new System.Drawing.Size(81, 19);
            this.lblCustomizationDescription.TabIndex = 7;
            this.lblCustomizationDescription.Text = "Description:";
            // 
            // lblCustomizationName
            // 
            this.lblCustomizationName.AutoSize = true;
            this.lblCustomizationName.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblCustomizationName.Location = new System.Drawing.Point(69, 76);
            this.lblCustomizationName.Name = "lblCustomizationName";
            this.lblCustomizationName.Size = new System.Drawing.Size(48, 19);
            this.lblCustomizationName.TabIndex = 5;
            this.lblCustomizationName.Text = "Name:";
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtFolderName.CustomButton.Image = null;
            this.txtFolderName.CustomButton.Location = new System.Drawing.Point(462, 1);
            this.txtFolderName.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtFolderName.CustomButton.Name = "";
            this.txtFolderName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtFolderName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFolderName.CustomButton.TabIndex = 1;
            this.txtFolderName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFolderName.CustomButton.UseSelectable = true;
            this.txtFolderName.CustomButton.Visible = false;
            this.txtFolderName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtFolderName.Lines = new string[0];
            this.txtFolderName.Location = new System.Drawing.Point(127, 46);
            this.txtFolderName.MaxLength = 255;
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.PasswordChar = '\0';
            this.txtFolderName.ReadOnly = true;
            this.txtFolderName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFolderName.SelectedText = "";
            this.txtFolderName.SelectionLength = 0;
            this.txtFolderName.SelectionStart = 0;
            this.txtFolderName.ShortcutsEnabled = true;
            this.txtFolderName.Size = new System.Drawing.Size(486, 25);
            this.txtFolderName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtFolderName.TabIndex = 4;
            this.txtFolderName.TabStop = false;
            this.txtFolderName.UseSelectable = true;
            this.txtFolderName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFolderName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblFolder.Location = new System.Drawing.Point(67, 48);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(50, 19);
            this.lblFolder.TabIndex = 3;
            this.lblFolder.Text = "Folder:";
            // 
            // txtPackageId
            // 
            this.txtPackageId.BackColor = System.Drawing.SystemColors.Control;
            // 
            // 
            // 
            this.txtPackageId.CustomButton.Image = null;
            this.txtPackageId.CustomButton.Location = new System.Drawing.Point(291, 1);
            this.txtPackageId.CustomButton.Margin = new System.Windows.Forms.Padding(2);
            this.txtPackageId.CustomButton.Name = "";
            this.txtPackageId.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtPackageId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPackageId.CustomButton.TabIndex = 1;
            this.txtPackageId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPackageId.CustomButton.UseSelectable = true;
            this.txtPackageId.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtPackageId.Lines = new string[0];
            this.txtPackageId.Location = new System.Drawing.Point(127, 17);
            this.txtPackageId.MaxLength = 36;
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.PasswordChar = '\0';
            this.txtPackageId.ReadOnly = true;
            this.txtPackageId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPackageId.SelectedText = "";
            this.txtPackageId.SelectionLength = 0;
            this.txtPackageId.SelectionStart = 0;
            this.txtPackageId.ShortcutsEnabled = true;
            this.txtPackageId.ShowButton = true;
            this.txtPackageId.ShowClearButton = true;
            this.txtPackageId.Size = new System.Drawing.Size(315, 25);
            this.txtPackageId.Style = MetroFramework.MetroColorStyle.Green;
            this.txtPackageId.TabIndex = 1;
            this.txtPackageId.TabStop = false;
            this.txtPackageId.UseSelectable = true;
            this.txtPackageId.UseStyleColors = true;
            this.txtPackageId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPackageId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtPackageId.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.btnPackageFinder_Click);
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblPackageId.Location = new System.Drawing.Point(55, 20);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(62, 19);
            this.lblPackageId.TabIndex = 0;
            this.lblPackageId.Text = "Package:";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.lblLowerBorder);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(952, 81);
            this.pnlButtons.TabIndex = 3;
            // 
            // lblLowerBorder
            // 
            this.lblLowerBorder.BackColor = System.Drawing.Color.Gainsboro;
            this.lblLowerBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLowerBorder.Location = new System.Drawing.Point(0, 0);
            this.lblLowerBorder.Name = "lblLowerBorder";
            this.lblLowerBorder.Size = new System.Drawing.Size(952, 1);
            this.lblLowerBorder.TabIndex = 28;
            // 
            // btnBack
            // 
            this.btnBack.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnBack.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnBack.Highlight = true;
            this.btnBack.Location = new System.Drawing.Point(805, 15);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(68, 25);
            this.btnBack.Style = MetroFramework.MetroColorStyle.Green;
            this.btnBack.TabIndex = 25;
            this.btnBack.Text = "Back";
            this.btnBack.UseSelectable = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNext.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNext.Highlight = true;
            this.btnNext.Location = new System.Drawing.Point(879, 15);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.Style = MetroFramework.MetroColorStyle.Green;
            this.btnNext.TabIndex = 26;
            this.btnNext.Text = "Next";
            this.btnNext.UseSelectable = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(11, 13);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 0);
            this.lblProcessingFile.TabIndex = 2;
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(982, 713);
            this.Controls.Add(this.splitBase);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInputForm";
            this.Padding = new System.Windows.Forms.Padding(15, 60, 15, 16);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Web Customization";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
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
            this.pnlKendo.ResumeLayout(false);
            this.pnlKendo.PerformLayout();
            this.pnlCreateEdit.ResumeLayout(false);
            this.pnlCreateEdit.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitBase;
        private MetroFramework.Controls.MetroLabel lblStepDescription;
        private MetroFramework.Controls.MetroLabel lblStepTitle;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Panel pnlCreateEdit;
        private MetroFramework.Controls.MetroTextBox txtEula;
        private MetroFramework.Controls.MetroLabel lblEula;
        private MetroFramework.Controls.MetroTextBox txtAssembly;
        private MetroFramework.Controls.MetroLabel lblAssembly;
        private MetroFramework.Controls.MetroTextBox txtBootstrapper;
        private MetroFramework.Controls.MetroLabel lblBootstrapper;
        private MetroFramework.Controls.MetroTextBox txtVersion;
        private MetroFramework.Controls.MetroTextBox txtCompatibility;
        private MetroFramework.Controls.MetroLabel lblVersion;
        private MetroFramework.Controls.MetroLabel lblCompatibility;
        private MetroFramework.Controls.MetroTextBox txtCompanyName;
        private MetroFramework.Controls.MetroTextBox txtCustomizationDescription;
        private MetroFramework.Controls.MetroTextBox txtCustomizationName;
        private MetroFramework.Controls.MetroLabel lblCompanyName;
        private MetroFramework.Controls.MetroLabel lblCustomizationDescription;
        private MetroFramework.Controls.MetroLabel lblCustomizationName;
        private MetroFramework.Controls.MetroTextBox txtFolderName;
        private MetroFramework.Controls.MetroLabel lblFolder;
        private MetroFramework.Controls.MetroTextBox txtPackageId;
        private MetroFramework.Controls.MetroLabel lblPackageId;
        private System.Windows.Forms.Panel pnlButtons;
        private MetroFramework.Controls.MetroButton btnNext;
        private MetroFramework.Controls.MetroLabel lblProcessingFile;
        private MetroFramework.Controls.MetroTextBox txtProject;
        private MetroFramework.Controls.MetroLabel lblProject;
        private System.Windows.Forms.ToolTip tooltip;
        private MetroFramework.Controls.MetroTextBox txtModule;
        private MetroFramework.Controls.MetroLabel lblModule;
        private System.Windows.Forms.Panel pnlKendo;
        private MetroFramework.Controls.MetroButton btnBack;
        private MetroFramework.Controls.MetroLabel lblKendoVersionHelp;
        private System.Windows.Forms.LinkLabel lblKendoLink;
        private MetroFramework.Controls.MetroLabel lblKendoFolderHelp;
        private MetroFramework.Controls.MetroTextBox txtKendoFolder;
        private MetroFramework.Controls.MetroCheckBox chkKendoLicense;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblUpperBorder;
        private System.Windows.Forms.Label lblLowerBorder;
    }
}