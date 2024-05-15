namespace Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generation));
            this.tbrControls = new System.Windows.Forms.ToolStrip();
            this.wrkBackground = new System.ComponentModel.BackgroundWorker();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCompile = new System.Windows.Forms.Button();
            this.lblProcessingFile = new System.Windows.Forms.Label();
            this.chkIgnoreConfigurations = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbrControls
            // 
            this.tbrControls.Location = new System.Drawing.Point(0, 0);
            this.tbrControls.Name = "tbrControls";
            this.tbrControls.Size = new System.Drawing.Size(100, 25);
            this.tbrControls.TabIndex = 0;
            // 
            // wrkBackground
            // 
            this.wrkBackground.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wrkBackground_DoWork);
            this.wrkBackground.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.wrkBackground_RunWorkerCompleted);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(618, 139);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = resources.GetString("lblMessage.Text");
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(600, 196);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(67, 22);
            this.btnCompile.TabIndex = 0;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // lblProcessingFile
            // 
            this.lblProcessingFile.Location = new System.Drawing.Point(23, 199);
            this.lblProcessingFile.Name = "lblProcessingFile";
            this.lblProcessingFile.Size = new System.Drawing.Size(531, 18);
            this.lblProcessingFile.TabIndex = 1;
            this.lblProcessingFile.Text = "Processing File";
            // 
            // chkIgnoreConfigurations
            // 
            this.chkIgnoreConfigurations.AutoSize = true;
            this.chkIgnoreConfigurations.Location = new System.Drawing.Point(15, 170);
            this.chkIgnoreConfigurations.Name = "chkIgnoreConfigurations";
            this.chkIgnoreConfigurations.Size = new System.Drawing.Size(643, 17);
            this.chkIgnoreConfigurations.TabIndex = 2;
            this.chkIgnoreConfigurations.Text = "By selecting this option, any discovered subclassing configurations will be ignor" +
    "e when compiling the model assemblies";
            this.chkIgnoreConfigurations.UseVisualStyleBackColor = true;
            // 
            // Generation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 226);
            this.Controls.Add(this.chkIgnoreConfigurations);
            this.Controls.Add(this.lblProcessingFile);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Generation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Subclassing Compilation and Deployment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Generation_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker wrkBackground;
        private System.Windows.Forms.ToolStrip tbrControls;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Label lblProcessingFile;
        private System.Windows.Forms.CheckBox chkIgnoreConfigurations;
    }
}

