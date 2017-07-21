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
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.splitSteps = new System.Windows.Forms.SplitContainer();
            this.pnlKendo = new System.Windows.Forms.Panel();
            this.pnlCreateEdit = new System.Windows.Forms.Panel();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.txtProject = new System.Windows.Forms.TextBox();
            this.lblProject = new System.Windows.Forms.Label();
            this.txtEula = new System.Windows.Forms.TextBox();
            this.lblEula = new System.Windows.Forms.Label();
            this.txtAssembly = new System.Windows.Forms.TextBox();
            this.lblAssembly = new System.Windows.Forms.Label();
            this.txtBootstrapper = new System.Windows.Forms.TextBox();
            this.lblBootstrapper = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtCompatibility = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCompatibility = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCustomizationDescription = new System.Windows.Forms.TextBox();
            this.txtCustomizationName = new System.Windows.Forms.TextBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCustomizationDescription = new System.Windows.Forms.Label();
            this.lblCustomizationName = new System.Windows.Forms.Label();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.lblFolder = new System.Windows.Forms.Label();
            this.btnPackageFinder = new System.Windows.Forms.Button();
            this.txtPackageId = new System.Windows.Forms.TextBox();
            this.lblPackageId = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblProcessingFile = new System.Windows.Forms.Label();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.lblKendoVersionHelp = new System.Windows.Forms.Label();
            this.lblKendoLink = new System.Windows.Forms.LinkLabel();
            this.lblKendoFolderHelp = new System.Windows.Forms.Label();
            this.txtKendoFolder = new System.Windows.Forms.TextBox();
            this.lblKendoFolder = new System.Windows.Forms.Label();
            this.chkKendoLicense = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
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
            this.splitBase.Location = new System.Drawing.Point(0, 0);
            this.splitBase.Margin = new System.Windows.Forms.Padding(4);
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
            this.splitBase.Size = new System.Drawing.Size(1279, 682);
            this.splitBase.SplitterDistance = 110;
            this.splitBase.SplitterWidth = 5;
            this.splitBase.TabIndex = 7;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(16, 49);
            this.lblStepDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(1239, 62);
            this.lblStepDescription.TabIndex = 15;
            this.lblStepDescription.Text = "This is the detailed description";
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.AutoSize = true;
            this.lblStepTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblStepTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepTitle.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepTitle.Location = new System.Drawing.Point(16, 11);
            this.lblStepTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(263, 28);
            this.lblStepTitle.TabIndex = 14;
            this.lblStepTitle.Text = "This is the title of the step";
            // 
            // splitSteps
            // 
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
            this.splitSteps.Panel1.Controls.Add(this.pnlKendo);
            this.splitSteps.Panel1.Controls.Add(this.pnlCreateEdit);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(1279, 567);
            this.splitSteps.SplitterDistance = 415;
            this.splitSteps.SplitterWidth = 5;
            this.splitSteps.TabIndex = 0;
            // 
            // pnlKendo
            // 
            this.pnlKendo.Controls.Add(this.lblKendoVersionHelp);
            this.pnlKendo.Controls.Add(this.lblKendoLink);
            this.pnlKendo.Controls.Add(this.lblKendoFolderHelp);
            this.pnlKendo.Controls.Add(this.txtKendoFolder);
            this.pnlKendo.Controls.Add(this.lblKendoFolder);
            this.pnlKendo.Controls.Add(this.chkKendoLicense);
            this.pnlKendo.Location = new System.Drawing.Point(47, 31);
            this.pnlKendo.Name = "pnlKendo";
            this.pnlKendo.Size = new System.Drawing.Size(963, 392);
            this.pnlKendo.TabIndex = 26;
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
            this.pnlCreateEdit.Controls.Add(this.btnPackageFinder);
            this.pnlCreateEdit.Controls.Add(this.txtPackageId);
            this.pnlCreateEdit.Controls.Add(this.lblPackageId);
            this.pnlCreateEdit.Location = new System.Drawing.Point(47, 31);
            this.pnlCreateEdit.Margin = new System.Windows.Forms.Padding(4);
            this.pnlCreateEdit.Name = "pnlCreateEdit";
            this.pnlCreateEdit.Size = new System.Drawing.Size(1103, 462);
            this.pnlCreateEdit.TabIndex = 1;
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(133, 316);
            this.txtModule.Margin = new System.Windows.Forms.Padding(4);
            this.txtModule.MaxLength = 2;
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(76, 22);
            this.txtModule.TabIndex = 18;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(65, 320);
            this.lblModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(58, 17);
            this.lblModule.TabIndex = 17;
            this.lblModule.Text = "Module:";
            // 
            // txtProject
            // 
            this.txtProject.Location = new System.Drawing.Point(133, 348);
            this.txtProject.Margin = new System.Windows.Forms.Padding(4);
            this.txtProject.Name = "txtProject";
            this.txtProject.Size = new System.Drawing.Size(420, 22);
            this.txtProject.TabIndex = 20;
            this.txtProject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProject_KeyPress);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(68, 352);
            this.lblProject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(56, 17);
            this.lblProject.TabIndex = 19;
            this.lblProject.Text = "Project:";
            // 
            // txtEula
            // 
            this.txtEula.BackColor = System.Drawing.SystemColors.Control;
            this.txtEula.Location = new System.Drawing.Point(133, 255);
            this.txtEula.Margin = new System.Windows.Forms.Padding(4);
            this.txtEula.Name = "txtEula";
            this.txtEula.ReadOnly = true;
            this.txtEula.Size = new System.Drawing.Size(648, 22);
            this.txtEula.TabIndex = 16;
            // 
            // lblEula
            // 
            this.lblEula.AutoSize = true;
            this.lblEula.Location = new System.Drawing.Point(77, 258);
            this.lblEula.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEula.Name = "lblEula";
            this.lblEula.Size = new System.Drawing.Size(48, 17);
            this.lblEula.TabIndex = 15;
            this.lblEula.Text = "EULA:";
            // 
            // txtAssembly
            // 
            this.txtAssembly.BackColor = System.Drawing.SystemColors.Control;
            this.txtAssembly.Location = new System.Drawing.Point(133, 433);
            this.txtAssembly.Margin = new System.Windows.Forms.Padding(4);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.ReadOnly = true;
            this.txtAssembly.Size = new System.Drawing.Size(420, 22);
            this.txtAssembly.TabIndex = 24;
            // 
            // lblAssembly
            // 
            this.lblAssembly.AutoSize = true;
            this.lblAssembly.Location = new System.Drawing.Point(55, 437);
            this.lblAssembly.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAssembly.Name = "lblAssembly";
            this.lblAssembly.Size = new System.Drawing.Size(72, 17);
            this.lblAssembly.TabIndex = 23;
            this.lblAssembly.Text = "Assembly:";
            // 
            // txtBootstrapper
            // 
            this.txtBootstrapper.BackColor = System.Drawing.SystemColors.Control;
            this.txtBootstrapper.Location = new System.Drawing.Point(133, 399);
            this.txtBootstrapper.Margin = new System.Windows.Forms.Padding(4);
            this.txtBootstrapper.Name = "txtBootstrapper";
            this.txtBootstrapper.ReadOnly = true;
            this.txtBootstrapper.Size = new System.Drawing.Size(420, 22);
            this.txtBootstrapper.TabIndex = 22;
            // 
            // lblBootstrapper
            // 
            this.lblBootstrapper.AutoSize = true;
            this.lblBootstrapper.Location = new System.Drawing.Point(39, 402);
            this.lblBootstrapper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBootstrapper.Name = "lblBootstrapper";
            this.lblBootstrapper.Size = new System.Drawing.Size(90, 17);
            this.lblBootstrapper.TabIndex = 21;
            this.lblBootstrapper.Text = "Boostrapper:";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.SystemColors.Control;
            this.txtVersion.Location = new System.Drawing.Point(133, 223);
            this.txtVersion.Margin = new System.Windows.Forms.Padding(4);
            this.txtVersion.MaxLength = 18;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(420, 22);
            this.txtVersion.TabIndex = 14;
            // 
            // txtCompatibility
            // 
            this.txtCompatibility.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompatibility.Location = new System.Drawing.Point(133, 188);
            this.txtCompatibility.Margin = new System.Windows.Forms.Padding(4);
            this.txtCompatibility.MaxLength = 60;
            this.txtCompatibility.Name = "txtCompatibility";
            this.txtCompatibility.ReadOnly = true;
            this.txtCompatibility.Size = new System.Drawing.Size(420, 22);
            this.txtCompatibility.TabIndex = 12;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(65, 226);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(60, 17);
            this.lblVersion.TabIndex = 13;
            this.lblVersion.Text = "Version:";
            // 
            // lblCompatibility
            // 
            this.lblCompatibility.AutoSize = true;
            this.lblCompatibility.Location = new System.Drawing.Point(35, 192);
            this.lblCompatibility.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompatibility.Name = "lblCompatibility";
            this.lblCompatibility.Size = new System.Drawing.Size(91, 17);
            this.lblCompatibility.TabIndex = 11;
            this.lblCompatibility.Text = "Compatibility:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompanyName.Location = new System.Drawing.Point(133, 156);
            this.txtCompanyName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCompanyName.MaxLength = 60;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Size = new System.Drawing.Size(648, 22);
            this.txtCompanyName.TabIndex = 10;
            // 
            // txtCustomizationDescription
            // 
            this.txtCustomizationDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtCustomizationDescription.Location = new System.Drawing.Point(133, 122);
            this.txtCustomizationDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomizationDescription.MaxLength = 255;
            this.txtCustomizationDescription.Name = "txtCustomizationDescription";
            this.txtCustomizationDescription.ReadOnly = true;
            this.txtCustomizationDescription.Size = new System.Drawing.Size(648, 22);
            this.txtCustomizationDescription.TabIndex = 8;
            // 
            // txtCustomizationName
            // 
            this.txtCustomizationName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCustomizationName.Location = new System.Drawing.Point(133, 87);
            this.txtCustomizationName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomizationName.MaxLength = 60;
            this.txtCustomizationName.Name = "txtCustomizationName";
            this.txtCustomizationName.ReadOnly = true;
            this.txtCustomizationName.Size = new System.Drawing.Size(648, 22);
            this.txtCustomizationName.TabIndex = 6;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(12, 160);
            this.lblCompanyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(112, 17);
            this.lblCompanyName.TabIndex = 9;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblCustomizationDescription
            // 
            this.lblCustomizationDescription.AutoSize = true;
            this.lblCustomizationDescription.Location = new System.Drawing.Point(41, 126);
            this.lblCustomizationDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomizationDescription.Name = "lblCustomizationDescription";
            this.lblCustomizationDescription.Size = new System.Drawing.Size(83, 17);
            this.lblCustomizationDescription.TabIndex = 7;
            this.lblCustomizationDescription.Text = "Description:";
            // 
            // lblCustomizationName
            // 
            this.lblCustomizationName.AutoSize = true;
            this.lblCustomizationName.Location = new System.Drawing.Point(75, 91);
            this.lblCustomizationName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomizationName.Name = "lblCustomizationName";
            this.lblCustomizationName.Size = new System.Drawing.Size(49, 17);
            this.lblCustomizationName.TabIndex = 5;
            this.lblCustomizationName.Text = "Name:";
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Control;
            this.txtFolderName.Location = new System.Drawing.Point(133, 55);
            this.txtFolderName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFolderName.MaxLength = 255;
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.ReadOnly = true;
            this.txtFolderName.Size = new System.Drawing.Size(648, 22);
            this.txtFolderName.TabIndex = 4;
            this.txtFolderName.TabStop = false;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(73, 59);
            this.lblFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(52, 17);
            this.lblFolder.TabIndex = 3;
            this.lblFolder.Text = "Folder:";
            // 
            // btnPackageFinder
            // 
            this.btnPackageFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPackageFinder.BackgroundImage")));
            this.btnPackageFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPackageFinder.FlatAppearance.BorderSize = 0;
            this.btnPackageFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPackageFinder.Location = new System.Drawing.Point(563, 21);
            this.btnPackageFinder.Margin = new System.Windows.Forms.Padding(4);
            this.btnPackageFinder.Name = "btnPackageFinder";
            this.btnPackageFinder.Size = new System.Drawing.Size(33, 31);
            this.btnPackageFinder.TabIndex = 2;
            this.btnPackageFinder.UseVisualStyleBackColor = true;
            this.btnPackageFinder.Click += new System.EventHandler(this.btnPackageFinder_Click);
            // 
            // txtPackageId
            // 
            this.txtPackageId.BackColor = System.Drawing.SystemColors.Control;
            this.txtPackageId.Location = new System.Drawing.Point(133, 21);
            this.txtPackageId.Margin = new System.Windows.Forms.Padding(4);
            this.txtPackageId.MaxLength = 36;
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.ReadOnly = true;
            this.txtPackageId.Size = new System.Drawing.Size(420, 22);
            this.txtPackageId.TabIndex = 1;
            this.txtPackageId.TabStop = false;
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.Location = new System.Drawing.Point(55, 25);
            this.lblPackageId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(67, 17);
            this.lblPackageId.TabIndex = 0;
            this.lblPackageId.Text = "Package:";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1279, 147);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(1073, 9);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(91, 31);
            this.btnBack.TabIndex = 25;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1172, 9);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(91, 31);
            this.btnNext.TabIndex = 26;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(15, 16);
            this.lblProcessingFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 17);
            this.lblProcessingFile.TabIndex = 2;
            // 
            // lblKendoVersionHelp
            // 
            this.lblKendoVersionHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKendoVersionHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoVersionHelp.Location = new System.Drawing.Point(139, 152);
            this.lblKendoVersionHelp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKendoVersionHelp.Name = "lblKendoVersionHelp";
            this.lblKendoVersionHelp.Size = new System.Drawing.Size(391, 44);
            this.lblKendoVersionHelp.TabIndex = 12;
            this.lblKendoVersionHelp.Text = "The Kendo UI version used in these projects is v2016.2.714";
            // 
            // lblKendoLink
            // 
            this.lblKendoLink.AutoSize = true;
            this.lblKendoLink.Location = new System.Drawing.Point(141, 121);
            this.lblKendoLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKendoLink.Name = "lblKendoLink";
            this.lblKendoLink.Size = new System.Drawing.Size(448, 17);
            this.lblKendoLink.TabIndex = 11;
            this.lblKendoLink.TabStop = true;
            this.lblKendoLink.Text = "http://www.telerik.com/purchase/license-agreement/kendo-ui-complete";
            // 
            // lblKendoFolderHelp
            // 
            this.lblKendoFolderHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKendoFolderHelp.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblKendoFolderHelp.Location = new System.Drawing.Point(137, 88);
            this.lblKendoFolderHelp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKendoFolderHelp.Name = "lblKendoFolderHelp";
            this.lblKendoFolderHelp.Size = new System.Drawing.Size(396, 33);
            this.lblKendoFolderHelp.TabIndex = 10;
            this.lblKendoFolderHelp.Text = "The Kendo UI Commercial License may be obtained at:";
            // 
            // txtKendoFolder
            // 
            this.txtKendoFolder.Enabled = false;
            this.txtKendoFolder.Location = new System.Drawing.Point(141, 53);
            this.txtKendoFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtKendoFolder.Name = "txtKendoFolder";
            this.txtKendoFolder.Size = new System.Drawing.Size(403, 22);
            this.txtKendoFolder.TabIndex = 9;
            // 
            // lblKendoFolder
            // 
            this.lblKendoFolder.AutoSize = true;
            this.lblKendoFolder.Enabled = false;
            this.lblKendoFolder.Location = new System.Drawing.Point(41, 57);
            this.lblKendoFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKendoFolder.Name = "lblKendoFolder";
            this.lblKendoFolder.Size = new System.Drawing.Size(97, 17);
            this.lblKendoFolder.TabIndex = 8;
            this.lblKendoFolder.Text = "Kendo Folder:";
            // 
            // chkKendoLicense
            // 
            this.chkKendoLicense.AutoSize = true;
            this.chkKendoLicense.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkKendoLicense.Location = new System.Drawing.Point(15, 19);
            this.chkKendoLicense.Margin = new System.Windows.Forms.Padding(4);
            this.chkKendoLicense.Name = "chkKendoLicense";
            this.chkKendoLicense.Size = new System.Drawing.Size(285, 23);
            this.chkKendoLicense.TabIndex = 7;
            this.chkKendoLicense.Text = "Purchased Kendo UI Commercial License?";
            this.chkKendoLicense.UseVisualStyleBackColor = true;
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 682);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Customization";
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel1.PerformLayout();
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.SplitContainer splitSteps;
        private System.Windows.Forms.Panel pnlCreateEdit;
        private System.Windows.Forms.TextBox txtEula;
        private System.Windows.Forms.Label lblEula;
        private System.Windows.Forms.TextBox txtAssembly;
        private System.Windows.Forms.Label lblAssembly;
        private System.Windows.Forms.TextBox txtBootstrapper;
        private System.Windows.Forms.Label lblBootstrapper;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.TextBox txtCompatibility;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCompatibility;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtCustomizationDescription;
        private System.Windows.Forms.TextBox txtCustomizationName;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCustomizationDescription;
        private System.Windows.Forms.Label lblCustomizationName;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Button btnPackageFinder;
        private System.Windows.Forms.TextBox txtPackageId;
        private System.Windows.Forms.Label lblPackageId;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblProcessingFile;
        private System.Windows.Forms.TextBox txtProject;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.Panel pnlKendo;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblKendoVersionHelp;
        private System.Windows.Forms.LinkLabel lblKendoLink;
        private System.Windows.Forms.Label lblKendoFolderHelp;
        private System.Windows.Forms.TextBox txtKendoFolder;
        private System.Windows.Forms.Label lblKendoFolder;
        private System.Windows.Forms.CheckBox chkKendoLicense;
    }
}