namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    partial class ContainerName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContainerName));
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.lblContainerName = new MetroFramework.Controls.MetroLabel();
            this.txtContainerName = new MetroFramework.Controls.MetroTextBox();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnSave.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnSave.Highlight = true;
            this.btnSave.Location = new System.Drawing.Point(215, 115);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 25);
            this.btnSave.Style = MetroFramework.MetroColorStyle.Green;
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnCancel.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnCancel.Highlight = true;
            this.btnCancel.Location = new System.Drawing.Point(293, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.Style = MetroFramework.MetroColorStyle.Green;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblContainerName
            // 
            this.lblContainerName.AutoSize = true;
            this.lblContainerName.BackColor = System.Drawing.Color.Transparent;
            this.lblContainerName.Location = new System.Drawing.Point(25, 74);
            this.lblContainerName.Name = "lblContainerName";
            this.lblContainerName.Size = new System.Drawing.Size(109, 19);
            this.lblContainerName.TabIndex = 0;
            this.lblContainerName.Text = "Container Name:";
            // 
            // txtContainerName
            // 
            // 
            // 
            // 
            this.txtContainerName.CustomButton.Image = null;
            this.txtContainerName.CustomButton.Location = new System.Drawing.Point(205, 2);
            this.txtContainerName.CustomButton.Name = "";
            this.txtContainerName.CustomButton.Size = new System.Drawing.Size(19, 19);
            this.txtContainerName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtContainerName.CustomButton.TabIndex = 1;
            this.txtContainerName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtContainerName.CustomButton.UseSelectable = true;
            this.txtContainerName.CustomButton.Visible = false;
            this.txtContainerName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtContainerName.Lines = new string[0];
            this.txtContainerName.Location = new System.Drawing.Point(134, 73);
            this.txtContainerName.MaxLength = 32767;
            this.txtContainerName.Name = "txtContainerName";
            this.txtContainerName.PasswordChar = '\0';
            this.txtContainerName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtContainerName.SelectedText = "";
            this.txtContainerName.SelectionLength = 0;
            this.txtContainerName.SelectionStart = 0;
            this.txtContainerName.ShortcutsEnabled = true;
            this.txtContainerName.Size = new System.Drawing.Size(227, 24);
            this.txtContainerName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtContainerName.TabIndex = 1;
            this.txtContainerName.UseSelectable = true;
            this.txtContainerName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtContainerName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtContainerName.Leave += new System.EventHandler(this.txtContainerName_Leave);
            // 
            // ContainerName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(384, 163);
            this.ControlBox = false;
            this.Controls.Add(this.txtContainerName);
            this.Controls.Add(this.lblContainerName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ContainerName";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Code Generation Wizard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnSave;
        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Controls.MetroLabel lblContainerName;
        private MetroFramework.Controls.MetroTextBox txtContainerName;
        private System.Windows.Forms.ToolTip tooltip;
    }
}