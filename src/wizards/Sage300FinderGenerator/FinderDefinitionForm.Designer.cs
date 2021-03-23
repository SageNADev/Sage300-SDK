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
            this.btnCreateFinderDef = new MetroFramework.Controls.MetroButton();
            this.finderDefinitionControl = new Sage300FinderGenerator.FinderDefinitionControl();
            this.SuspendLayout();
            // 
            // btnCreateFinderDef
            // 
            this.btnCreateFinderDef.Location = new System.Drawing.Point(595, 572);
            this.btnCreateFinderDef.Name = "btnCreateFinderDef";
            this.btnCreateFinderDef.Size = new System.Drawing.Size(75, 23);
            this.btnCreateFinderDef.TabIndex = 1;
            this.btnCreateFinderDef.Text = "Create";
            this.btnCreateFinderDef.UseSelectable = true;
            this.btnCreateFinderDef.Click += new System.EventHandler(this.btnCreateFinderDef_Click);
            // 
            // finderDefinitionControl
            // 
            this.finderDefinitionControl.BackColor = System.Drawing.SystemColors.Window;
            this.finderDefinitionControl.Location = new System.Drawing.Point(20, 63);
            this.finderDefinitionControl.Name = "finderDefinitionControl";
            this.finderDefinitionControl.Size = new System.Drawing.Size(682, 503);
            this.finderDefinitionControl.TabIndex = 5;
            // 
            // FinderDefinitionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 617);
            this.Controls.Add(this.finderDefinitionControl);
            this.Controls.Add(this.btnCreateFinderDef);
            this.Name = "FinderDefinitionForm";
            this.Style = MetroFramework.MetroColorStyle.Default;
            this.Text = "Finder Definition Generator";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroButton btnCreateFinderDef;
        private FinderDefinitionControl finderDefinitionControl;
    }
}