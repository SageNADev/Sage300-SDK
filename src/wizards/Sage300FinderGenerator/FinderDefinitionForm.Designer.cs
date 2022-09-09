namespace Sage300FinderGenerator
{
    partial class FinderDefinitionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinderDefinitionForm));
            this.btnCreateFinderDef = new MetroFramework.Controls.MetroButton();
            this.finderDefinitionControl = new Sage300FinderGenerator.FinderDefinitionControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateFinderDef
            // 
            this.btnCreateFinderDef.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnCreateFinderDef.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnCreateFinderDef.Highlight = true;
            this.btnCreateFinderDef.Location = new System.Drawing.Point(579, 575);
            this.btnCreateFinderDef.Name = "btnCreateFinderDef";
            this.btnCreateFinderDef.Size = new System.Drawing.Size(75, 23);
            this.btnCreateFinderDef.Style = MetroFramework.MetroColorStyle.Green;
            this.btnCreateFinderDef.TabIndex = 1;
            this.btnCreateFinderDef.Text = "Create";
            this.btnCreateFinderDef.UseSelectable = true;
            this.btnCreateFinderDef.Click += new System.EventHandler(this.btnCreateFinderDef_Click);
            // 
            // finderDefinitionControl
            // 
            this.finderDefinitionControl.BackColor = System.Drawing.SystemColors.Window;
            this.finderDefinitionControl.Location = new System.Drawing.Point(2, 103);
            this.finderDefinitionControl.Name = "finderDefinitionControl";
            this.finderDefinitionControl.Size = new System.Drawing.Size(661, 450);
            this.finderDefinitionControl.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sage300FinderGenerator.Properties.Resources.sage300_logo_sq;
            this.pictureBox1.Location = new System.Drawing.Point(551, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 69);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // FinderDefinitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(667, 622);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.finderDefinitionControl);
            this.Controls.Add(this.btnCreateFinderDef);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FinderDefinitionForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Finder Definition Generator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroButton btnCreateFinderDef;
        private FinderDefinitionControl finderDefinitionControl;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}