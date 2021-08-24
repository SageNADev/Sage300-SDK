namespace CodeGenerationTester
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmdLaunch = new System.Windows.Forms.Button();
            this.lblSolution = new System.Windows.Forms.Label();
            this.txtSolution = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdLaunch
            // 
            this.cmdLaunch.Location = new System.Drawing.Point(602, 77);
            this.cmdLaunch.Name = "cmdLaunch";
            this.cmdLaunch.Size = new System.Drawing.Size(97, 32);
            this.cmdLaunch.TabIndex = 0;
            this.cmdLaunch.Text = "Launch";
            this.cmdLaunch.UseVisualStyleBackColor = true;
            this.cmdLaunch.Click += new System.EventHandler(this.cmdLaunch_Click);
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(15, 39);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(87, 13);
            this.lblSolution.TabIndex = 1;
            this.lblSolution.Text = "Solution to Load:";
            // 
            // txtSolution
            // 
            this.txtSolution.Location = new System.Drawing.Point(108, 36);
            this.txtSolution.Name = "txtSolution";
            this.txtSolution.Size = new System.Drawing.Size(591, 20);
            this.txtSolution.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 126);
            this.Controls.Add(this.txtSolution);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.cmdLaunch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Code Generation Test Harness";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdLaunch;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.TextBox txtSolution;
    }
}

