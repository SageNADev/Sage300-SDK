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
            this.btnNext = new System.Windows.Forms.Button();
            this.lblProcessingFile = new System.Windows.Forms.Label();
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
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
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
            this.splitBase.Size = new System.Drawing.Size(959, 554);
            this.splitBase.SplitterDistance = 90;
            this.splitBase.TabIndex = 7;
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblStepDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepDescription.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 40);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(929, 50);
            this.lblStepDescription.TabIndex = 15;
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
            this.lblStepTitle.TabIndex = 14;
            this.lblStepTitle.Text = "This is the title of the step";
            // 
            // splitSteps
            // 
            this.splitSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSteps.IsSplitterFixed = true;
            this.splitSteps.Location = new System.Drawing.Point(0, 0);
            this.splitSteps.Name = "splitSteps";
            this.splitSteps.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSteps.Panel1
            // 
            this.splitSteps.Panel1.Controls.Add(this.pnlCreateEdit);
            // 
            // splitSteps.Panel2
            // 
            this.splitSteps.Panel2.Controls.Add(this.pnlButtons);
            this.splitSteps.Size = new System.Drawing.Size(959, 460);
            this.splitSteps.SplitterDistance = 415;
            this.splitSteps.TabIndex = 0;
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
            this.pnlCreateEdit.Location = new System.Drawing.Point(16, 24);
            this.pnlCreateEdit.Name = "pnlCreateEdit";
            this.pnlCreateEdit.Size = new System.Drawing.Size(891, 375);
            this.pnlCreateEdit.TabIndex = 1;
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(100, 257);
            this.txtModule.MaxLength = 2;
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(58, 20);
            this.txtModule.TabIndex = 18;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(49, 260);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(45, 13);
            this.lblModule.TabIndex = 17;
            this.lblModule.Text = "Module:";
            // 
            // txtProject
            // 
            this.txtProject.Location = new System.Drawing.Point(100, 283);
            this.txtProject.Name = "txtProject";
            this.txtProject.Size = new System.Drawing.Size(316, 20);
            this.txtProject.TabIndex = 20;
            this.txtProject.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProject_KeyPress);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(51, 286);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(43, 13);
            this.lblProject.TabIndex = 19;
            this.lblProject.Text = "Project:";
            // 
            // txtEula
            // 
            this.txtEula.BackColor = System.Drawing.SystemColors.Control;
            this.txtEula.Location = new System.Drawing.Point(100, 207);
            this.txtEula.Name = "txtEula";
            this.txtEula.ReadOnly = true;
            this.txtEula.Size = new System.Drawing.Size(487, 20);
            this.txtEula.TabIndex = 16;
            // 
            // lblEula
            // 
            this.lblEula.AutoSize = true;
            this.lblEula.Location = new System.Drawing.Point(58, 210);
            this.lblEula.Name = "lblEula";
            this.lblEula.Size = new System.Drawing.Size(38, 13);
            this.lblEula.TabIndex = 15;
            this.lblEula.Text = "EULA:";
            // 
            // txtAssembly
            // 
            this.txtAssembly.BackColor = System.Drawing.SystemColors.Control;
            this.txtAssembly.Location = new System.Drawing.Point(100, 352);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.ReadOnly = true;
            this.txtAssembly.Size = new System.Drawing.Size(316, 20);
            this.txtAssembly.TabIndex = 24;
            // 
            // lblAssembly
            // 
            this.lblAssembly.AutoSize = true;
            this.lblAssembly.Location = new System.Drawing.Point(41, 355);
            this.lblAssembly.Name = "lblAssembly";
            this.lblAssembly.Size = new System.Drawing.Size(54, 13);
            this.lblAssembly.TabIndex = 23;
            this.lblAssembly.Text = "Assembly:";
            // 
            // txtBootstrapper
            // 
            this.txtBootstrapper.BackColor = System.Drawing.SystemColors.Control;
            this.txtBootstrapper.Location = new System.Drawing.Point(100, 324);
            this.txtBootstrapper.Name = "txtBootstrapper";
            this.txtBootstrapper.ReadOnly = true;
            this.txtBootstrapper.Size = new System.Drawing.Size(316, 20);
            this.txtBootstrapper.TabIndex = 22;
            // 
            // lblBootstrapper
            // 
            this.lblBootstrapper.AutoSize = true;
            this.lblBootstrapper.Location = new System.Drawing.Point(29, 327);
            this.lblBootstrapper.Name = "lblBootstrapper";
            this.lblBootstrapper.Size = new System.Drawing.Size(67, 13);
            this.lblBootstrapper.TabIndex = 21;
            this.lblBootstrapper.Text = "Boostrapper:";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.SystemColors.Control;
            this.txtVersion.Location = new System.Drawing.Point(100, 181);
            this.txtVersion.MaxLength = 18;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(316, 20);
            this.txtVersion.TabIndex = 14;
            // 
            // txtCompatibility
            // 
            this.txtCompatibility.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompatibility.Location = new System.Drawing.Point(100, 153);
            this.txtCompatibility.MaxLength = 60;
            this.txtCompatibility.Name = "txtCompatibility";
            this.txtCompatibility.ReadOnly = true;
            this.txtCompatibility.Size = new System.Drawing.Size(316, 20);
            this.txtCompatibility.TabIndex = 12;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(49, 184);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(45, 13);
            this.lblVersion.TabIndex = 13;
            this.lblVersion.Text = "Version:";
            // 
            // lblCompatibility
            // 
            this.lblCompatibility.AutoSize = true;
            this.lblCompatibility.Location = new System.Drawing.Point(26, 156);
            this.lblCompatibility.Name = "lblCompatibility";
            this.lblCompatibility.Size = new System.Drawing.Size(68, 13);
            this.lblCompatibility.TabIndex = 11;
            this.lblCompatibility.Text = "Compatibility:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompanyName.Location = new System.Drawing.Point(100, 127);
            this.txtCompanyName.MaxLength = 60;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Size = new System.Drawing.Size(487, 20);
            this.txtCompanyName.TabIndex = 10;
            // 
            // txtCustomizationDescription
            // 
            this.txtCustomizationDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtCustomizationDescription.Location = new System.Drawing.Point(100, 99);
            this.txtCustomizationDescription.MaxLength = 255;
            this.txtCustomizationDescription.Name = "txtCustomizationDescription";
            this.txtCustomizationDescription.ReadOnly = true;
            this.txtCustomizationDescription.Size = new System.Drawing.Size(487, 20);
            this.txtCustomizationDescription.TabIndex = 8;
            // 
            // txtCustomizationName
            // 
            this.txtCustomizationName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCustomizationName.Location = new System.Drawing.Point(100, 71);
            this.txtCustomizationName.MaxLength = 60;
            this.txtCustomizationName.Name = "txtCustomizationName";
            this.txtCustomizationName.ReadOnly = true;
            this.txtCustomizationName.Size = new System.Drawing.Size(487, 20);
            this.txtCustomizationName.TabIndex = 6;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(9, 130);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(85, 13);
            this.lblCompanyName.TabIndex = 9;
            this.lblCompanyName.Text = "Company Name:";
            // 
            // lblCustomizationDescription
            // 
            this.lblCustomizationDescription.AutoSize = true;
            this.lblCustomizationDescription.Location = new System.Drawing.Point(31, 102);
            this.lblCustomizationDescription.Name = "lblCustomizationDescription";
            this.lblCustomizationDescription.Size = new System.Drawing.Size(63, 13);
            this.lblCustomizationDescription.TabIndex = 7;
            this.lblCustomizationDescription.Text = "Description:";
            // 
            // lblCustomizationName
            // 
            this.lblCustomizationName.AutoSize = true;
            this.lblCustomizationName.Location = new System.Drawing.Point(56, 74);
            this.lblCustomizationName.Name = "lblCustomizationName";
            this.lblCustomizationName.Size = new System.Drawing.Size(38, 13);
            this.lblCustomizationName.TabIndex = 5;
            this.lblCustomizationName.Text = "Name:";
            // 
            // txtFolderName
            // 
            this.txtFolderName.BackColor = System.Drawing.SystemColors.Control;
            this.txtFolderName.Location = new System.Drawing.Point(100, 45);
            this.txtFolderName.MaxLength = 255;
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.ReadOnly = true;
            this.txtFolderName.Size = new System.Drawing.Size(487, 20);
            this.txtFolderName.TabIndex = 4;
            this.txtFolderName.TabStop = false;
            // 
            // lblFolder
            // 
            this.lblFolder.AutoSize = true;
            this.lblFolder.Location = new System.Drawing.Point(55, 48);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Size = new System.Drawing.Size(39, 13);
            this.lblFolder.TabIndex = 3;
            this.lblFolder.Text = "Folder:";
            // 
            // btnPackageFinder
            // 
            this.btnPackageFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPackageFinder.BackgroundImage")));
            this.btnPackageFinder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPackageFinder.FlatAppearance.BorderSize = 0;
            this.btnPackageFinder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPackageFinder.Location = new System.Drawing.Point(422, 17);
            this.btnPackageFinder.Name = "btnPackageFinder";
            this.btnPackageFinder.Size = new System.Drawing.Size(25, 25);
            this.btnPackageFinder.TabIndex = 2;
            this.btnPackageFinder.UseVisualStyleBackColor = true;
            this.btnPackageFinder.Click += new System.EventHandler(this.btnPackageFinder_Click);
            // 
            // txtPackageId
            // 
            this.txtPackageId.BackColor = System.Drawing.SystemColors.Control;
            this.txtPackageId.Location = new System.Drawing.Point(100, 17);
            this.txtPackageId.MaxLength = 36;
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.ReadOnly = true;
            this.txtPackageId.Size = new System.Drawing.Size(316, 20);
            this.txtPackageId.TabIndex = 1;
            this.txtPackageId.TabStop = false;
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.Location = new System.Drawing.Point(41, 20);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(53, 13);
            this.lblPackageId.TabIndex = 0;
            this.lblPackageId.Text = "Package:";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.lblProcessingFile);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(959, 41);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(879, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(68, 25);
            this.btnNext.TabIndex = 25;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.AutoSize = true;
            this.lblProcessingFile.Location = new System.Drawing.Point(11, 13);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(0, 13);
            this.lblProcessingFile.TabIndex = 2;
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 554);
            this.Controls.Add(this.splitBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}