namespace Sage.CA.SBS.ERP.Sage300.SubclassPrep
{
    partial class Generation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generation));
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.btnGenerate = new MetroFramework.Controls.MetroButton();
            this.lblProcessing = new MetroFramework.Controls.MetroLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblInformation = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // wrkBackground
            // 
            this.wrkBackground.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wrkBackground_DoWork);
            this.wrkBackground.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wrkBackground_RunWorkerCompleted);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Highlight = true;
            this.btnGenerate.Location = new System.Drawing.Point(577, 150);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 25);
            this.btnGenerate.Style = MetroFramework.MetroColorStyle.Green;
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseSelectable = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Location = new System.Drawing.Point(10, 150);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(74, 19);
            this.lblProcessing.TabIndex = 1;
            this.lblProcessing.Text = "Processing:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sage.CA.SBS.ERP.Sage300.SubclassPrep.Properties.Resources.sage300_logo_sq;
            this.pictureBox1.Location = new System.Drawing.Point(549, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(103, 86);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblInformation
            // 
            this.lblInformation.Location = new System.Drawing.Point(35, 74);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(491, 66);
            this.lblInformation.TabIndex = 9;
            this.lblInformation.Text = resources.GetString("lblInformation.Text");
            this.lblInformation.WrapToLine = true;
            // 
            // Generation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(675, 192);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblProcessing);
            this.Controls.Add(this.btnGenerate);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Generation";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Web Subclassing Prep";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private MetroFramework.Controls.MetroButton btnGenerate;
        private MetroFramework.Controls.MetroLabel lblProcessing;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroLabel lblInformation;
    }
}

