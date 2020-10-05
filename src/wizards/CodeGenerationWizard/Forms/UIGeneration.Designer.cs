// The MIT License (MIT) 
// Copyright (c) 1994-2020 The Sage Group plc or its licensors.  All rights reserved.
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

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    partial class UIGeneration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIGeneration));
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.splitDesigner = new System.Windows.Forms.SplitContainer();
            this.treeEntities = new System.Windows.Forms.TreeView();
            this.grpContainers = new System.Windows.Forms.GroupBox();
            this.tbrProperties = new System.Windows.Forms.ToolStrip();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddPage = new System.Windows.Forms.ToolStripButton();
            this.picGrid = new System.Windows.Forms.PictureBox();
            this.txtPropText = new System.Windows.Forms.TextBox();
            this.picTab = new System.Windows.Forms.PictureBox();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.txtPropWidget = new System.Windows.Forms.TextBox();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDesigner)).BeginInit();
            this.splitDesigner.Panel2.SuspendLayout();
            this.splitDesigner.SuspendLayout();
            this.grpContainers.SuspendLayout();
            this.tbrProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 605);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(1031, 47);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(870, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(944, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 25);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // splitDesigner
            // 
            this.splitDesigner.Location = new System.Drawing.Point(0, 12);
            this.splitDesigner.Name = "splitDesigner";
            // 
            // splitDesigner.Panel1
            // 
            this.splitDesigner.Panel1.AllowDrop = true;
            this.splitDesigner.Panel1.BackColor = System.Drawing.Color.White;
            this.splitDesigner.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            // 
            // splitDesigner.Panel2
            // 
            this.splitDesigner.Panel2.Controls.Add(this.treeEntities);
            this.splitDesigner.Panel2.Controls.Add(this.grpContainers);
            this.splitDesigner.Size = new System.Drawing.Size(1021, 587);
            this.splitDesigner.SplitterDistance = 750;
            this.splitDesigner.TabIndex = 1;
            // 
            // treeEntities
            // 
            this.treeEntities.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeEntities.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.treeEntities.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeEntities.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeEntities.Location = new System.Drawing.Point(0, 74);
            this.treeEntities.Name = "treeEntities";
            this.treeEntities.Size = new System.Drawing.Size(267, 513);
            this.treeEntities.TabIndex = 4;
            this.treeEntities.TabStop = false;
            // 
            // grpContainers
            // 
            this.grpContainers.BackColor = System.Drawing.Color.White;
            this.grpContainers.Controls.Add(this.txtPropWidget);
            this.grpContainers.Controls.Add(this.tbrProperties);
            this.grpContainers.Controls.Add(this.picGrid);
            this.grpContainers.Controls.Add(this.txtPropText);
            this.grpContainers.Controls.Add(this.picTab);
            this.grpContainers.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpContainers.Location = new System.Drawing.Point(0, 0);
            this.grpContainers.Name = "grpContainers";
            this.grpContainers.Size = new System.Drawing.Size(267, 68);
            this.grpContainers.TabIndex = 3;
            this.grpContainers.TabStop = false;
            this.grpContainers.Text = "Toolbox";
            // 
            // tbrProperties
            // 
            this.tbrProperties.Dock = System.Windows.Forms.DockStyle.None;
            this.tbrProperties.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tbrProperties.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDelete,
            this.toolStripSeparator1,
            this.btnAddPage});
            this.tbrProperties.Location = new System.Drawing.Point(64, 22);
            this.tbrProperties.Name = "tbrProperties";
            this.tbrProperties.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.tbrProperties.Size = new System.Drawing.Size(75, 31);
            this.tbrProperties.TabIndex = 0;
            this.tbrProperties.Text = "toolStrip1";
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(28, 28);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // btnAddPage
            // 
            this.btnAddPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddPage.Image = ((System.Drawing.Image)(resources.GetObject("btnAddPage.Image")));
            this.btnAddPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPage.Name = "btnAddPage";
            this.btnAddPage.Size = new System.Drawing.Size(28, 28);
            this.btnAddPage.ToolTipText = "Add Tab Page";
            this.btnAddPage.Click += new System.EventHandler(this.btnAddPage_Click);
            // 
            // picGrid
            // 
            this.picGrid.Image = ((System.Drawing.Image)(resources.GetObject("picGrid.Image")));
            this.picGrid.Location = new System.Drawing.Point(36, 22);
            this.picGrid.Name = "picGrid";
            this.picGrid.Size = new System.Drawing.Size(25, 25);
            this.picGrid.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGrid.TabIndex = 10;
            this.picGrid.TabStop = false;
            this.picGrid.Tag = "Grid";
            // 
            // txtPropText
            // 
            this.txtPropText.Location = new System.Drawing.Point(142, 42);
            this.txtPropText.Name = "txtPropText";
            this.txtPropText.Size = new System.Drawing.Size(119, 20);
            this.txtPropText.TabIndex = 17;
            this.txtPropText.TextChanged += new System.EventHandler(this.txtPropText_TextChanged);
            // 
            // picTab
            // 
            this.picTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picTab.Image = ((System.Drawing.Image)(resources.GetObject("picTab.Image")));
            this.picTab.Location = new System.Drawing.Point(5, 22);
            this.picTab.Name = "picTab";
            this.picTab.Size = new System.Drawing.Size(25, 25);
            this.picTab.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTab.TabIndex = 4;
            this.picTab.TabStop = false;
            this.picTab.Tag = "Tab";
            // 
            // txtPropWidget
            // 
            this.txtPropWidget.Location = new System.Drawing.Point(142, 16);
            this.txtPropWidget.Name = "txtPropWidget";
            this.txtPropWidget.Size = new System.Drawing.Size(119, 20);
            this.txtPropWidget.TabIndex = 18;
            // 
            // UIGeneration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 652);
            this.Controls.Add(this.splitDesigner);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UIGeneration";
            this.Text = "UI Generation Wizard";
            this.pnlButtons.ResumeLayout(false);
            this.splitDesigner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDesigner)).EndInit();
            this.splitDesigner.ResumeLayout(false);
            this.grpContainers.ResumeLayout(false);
            this.grpContainers.PerformLayout();
            this.tbrProperties.ResumeLayout(false);
            this.tbrProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.SplitContainer splitDesigner;
        private System.Windows.Forms.GroupBox grpContainers;
        private System.Windows.Forms.PictureBox picTab;
        private System.Windows.Forms.ToolStrip tbrProperties;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddPage;
        private System.Windows.Forms.TextBox txtPropText;
        private System.Windows.Forms.TreeView treeEntities;
        private System.Windows.Forms.PictureBox picGrid;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TextBox txtPropWidget;
    }
}

