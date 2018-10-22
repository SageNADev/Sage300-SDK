namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    partial class SqlHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlHelp));
            this.tabSqlHelp = new System.Windows.Forms.TabControl();
            this.tabSqlInstructions = new System.Windows.Forms.TabPage();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.tabSqlExample1 = new System.Windows.Forms.TabPage();
            this.tabSqlExample2 = new System.Windows.Forms.TabPage();
            this.tabSqlExample3 = new System.Windows.Forms.TabPage();
            this.txtExample1 = new System.Windows.Forms.RichTextBox();
            this.picExample2 = new System.Windows.Forms.PictureBox();
            this.tabSqlHelp.SuspendLayout();
            this.tabSqlInstructions.SuspendLayout();
            this.tabSqlExample1.SuspendLayout();
            this.tabSqlExample2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExample2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSqlHelp
            // 
            this.tabSqlHelp.Controls.Add(this.tabSqlInstructions);
            this.tabSqlHelp.Controls.Add(this.tabSqlExample1);
            this.tabSqlHelp.Controls.Add(this.tabSqlExample2);
            this.tabSqlHelp.Controls.Add(this.tabSqlExample3);
            this.tabSqlHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSqlHelp.Location = new System.Drawing.Point(0, 0);
            this.tabSqlHelp.Name = "tabSqlHelp";
            this.tabSqlHelp.SelectedIndex = 0;
            this.tabSqlHelp.Size = new System.Drawing.Size(884, 461);
            this.tabSqlHelp.TabIndex = 0;
            // 
            // tabSqlInstructions
            // 
            this.tabSqlInstructions.Controls.Add(this.lblInstructions);
            this.tabSqlInstructions.Location = new System.Drawing.Point(4, 22);
            this.tabSqlInstructions.Name = "tabSqlInstructions";
            this.tabSqlInstructions.Padding = new System.Windows.Forms.Padding(3);
            this.tabSqlInstructions.Size = new System.Drawing.Size(876, 435);
            this.tabSqlInstructions.TabIndex = 0;
            this.tabSqlInstructions.Text = "Instructions";
            this.tabSqlInstructions.UseVisualStyleBackColor = true;
            // 
            // lblInstructions
            // 
            this.lblInstructions.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblInstructions.Location = new System.Drawing.Point(19, 32);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(824, 372);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // tabSqlExample1
            // 
            this.tabSqlExample1.Controls.Add(this.txtExample1);
            this.tabSqlExample1.Location = new System.Drawing.Point(4, 22);
            this.tabSqlExample1.Name = "tabSqlExample1";
            this.tabSqlExample1.Padding = new System.Windows.Forms.Padding(3);
            this.tabSqlExample1.Size = new System.Drawing.Size(876, 435);
            this.tabSqlExample1.TabIndex = 1;
            this.tabSqlExample1.Text = "Example 1";
            this.tabSqlExample1.UseVisualStyleBackColor = true;
            // 
            // tabSqlExample2
            // 
            this.tabSqlExample2.Controls.Add(this.picExample2);
            this.tabSqlExample2.Location = new System.Drawing.Point(4, 22);
            this.tabSqlExample2.Name = "tabSqlExample2";
            this.tabSqlExample2.Size = new System.Drawing.Size(876, 435);
            this.tabSqlExample2.TabIndex = 2;
            this.tabSqlExample2.Text = "Example 2";
            this.tabSqlExample2.UseVisualStyleBackColor = true;
            // 
            // tabSqlExample3
            // 
            this.tabSqlExample3.Location = new System.Drawing.Point(4, 22);
            this.tabSqlExample3.Name = "tabSqlExample3";
            this.tabSqlExample3.Size = new System.Drawing.Size(876, 435);
            this.tabSqlExample3.TabIndex = 3;
            this.tabSqlExample3.Text = "Example 3";
            this.tabSqlExample3.UseVisualStyleBackColor = true;
            // 
            // txtExample1
            // 
            this.txtExample1.BackColor = System.Drawing.SystemColors.Window;
            this.txtExample1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtExample1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExample1.Location = new System.Drawing.Point(3, 3);
            this.txtExample1.Name = "txtExample1";
            this.txtExample1.ReadOnly = true;
            this.txtExample1.Size = new System.Drawing.Size(870, 429);
            this.txtExample1.TabIndex = 0;
            this.txtExample1.Text = resources.GetString("txtExample1.Text");
            // 
            // picExample2
            // 
            this.picExample2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExample2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picExample2.Image = ((System.Drawing.Image)(resources.GetObject("picExample2.Image")));
            this.picExample2.Location = new System.Drawing.Point(0, 0);
            this.picExample2.Name = "picExample2";
            this.picExample2.Size = new System.Drawing.Size(876, 435);
            this.picExample2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picExample2.TabIndex = 0;
            this.picExample2.TabStop = false;
            // 
            // SqlHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.tabSqlHelp);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlHelp";
            this.Text = "SQL Help";
            this.tabSqlHelp.ResumeLayout(false);
            this.tabSqlInstructions.ResumeLayout(false);
            this.tabSqlExample1.ResumeLayout(false);
            this.tabSqlExample2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExample2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSqlHelp;
        private System.Windows.Forms.TabPage tabSqlInstructions;
        private System.Windows.Forms.TabPage tabSqlExample1;
        private System.Windows.Forms.TabPage tabSqlExample2;
        private System.Windows.Forms.TabPage tabSqlExample3;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.RichTextBox txtExample1;
        private System.Windows.Forms.PictureBox picExample2;
    }
}