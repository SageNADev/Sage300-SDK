using System.Windows.Forms;

namespace WebAPISubclassWizard
{
    partial class Wizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wizard));
            this.WizardTab = new WebAPISubclassWizard.WizardTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dropdownlistModules = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPage1Next = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.listBoxClasses = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPage2Next = new System.Windows.Forms.Button();
            this.btnPage2Back = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnCreateProject = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.textBoxController = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnProjectFolder = new System.Windows.Forms.Button();
            this.textBoxProjectFolder = new System.Windows.Forms.TextBox();
            this.textBoxModule = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPage3Next = new System.Windows.Forms.Button();
            this.btnPage3Back = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.WizardTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // WizardTab
            // 
            this.WizardTab.Controls.Add(this.tabPage1);
            this.WizardTab.Controls.Add(this.tabPage2);
            this.WizardTab.Controls.Add(this.tabPage3);
            this.WizardTab.Location = new System.Drawing.Point(12, 57);
            this.WizardTab.Name = "WizardTab";
            this.WizardTab.SelectedIndex = 0;
            this.WizardTab.Size = new System.Drawing.Size(1022, 532);
            this.WizardTab.TabIndex = 0;
            this.WizardTab.TabStop = false;
            this.WizardTab.Selected += new System.Windows.Forms.TabControlEventHandler(this.WizardTab_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.dropdownlistModules);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnPage1Next);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1014, 506);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.ForeColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(17, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(1004, 1);
            this.label10.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(882, -22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(141, 119);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dropdownlistModules
            // 
            this.dropdownlistModules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownlistModules.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropdownlistModules.FormattingEnabled = true;
            this.dropdownlistModules.Location = new System.Drawing.Point(174, 123);
            this.dropdownlistModules.Name = "dropdownlistModules";
            this.dropdownlistModules.Size = new System.Drawing.Size(121, 28);
            this.dropdownlistModules.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Available Modules";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(32, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(489, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Step 1. Choose which module you want to subclass";
            // 
            // btnPage1Next
            // 
            this.btnPage1Next.Location = new System.Drawing.Point(933, 477);
            this.btnPage1Next.Name = "btnPage1Next";
            this.btnPage1Next.Size = new System.Drawing.Size(75, 23);
            this.btnPage1Next.TabIndex = 6;
            this.btnPage1Next.Text = "Next";
            this.btnPage1Next.UseVisualStyleBackColor = true;
            this.btnPage1Next.Click += new System.EventHandler(this.btnPage1Next_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.listBoxClasses);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnPage2Next);
            this.tabPage2.Controls.Add(this.btnPage2Back);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1014, 506);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Location = new System.Drawing.Point(17, 97);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1004, 1);
            this.label11.TabIndex = 11;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(882, -22);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(141, 119);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // listBoxClasses
            // 
            this.listBoxClasses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxClasses.FormattingEnabled = true;
            this.listBoxClasses.ItemHeight = 20;
            this.listBoxClasses.Location = new System.Drawing.Point(17, 128);
            this.listBoxClasses.Name = "listBoxClasses";
            this.listBoxClasses.Size = new System.Drawing.Size(986, 324);
            this.listBoxClasses.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Green;
            this.label4.Location = new System.Drawing.Point(32, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(316, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Step 2. Choose a class to extend";
            // 
            // btnPage2Next
            // 
            this.btnPage2Next.Location = new System.Drawing.Point(928, 477);
            this.btnPage2Next.Name = "btnPage2Next";
            this.btnPage2Next.Size = new System.Drawing.Size(75, 23);
            this.btnPage2Next.TabIndex = 9;
            this.btnPage2Next.Text = "Next";
            this.btnPage2Next.UseVisualStyleBackColor = true;
            this.btnPage2Next.Click += new System.EventHandler(this.btnPage2Next_Click);
            // 
            // btnPage2Back
            // 
            this.btnPage2Back.Location = new System.Drawing.Point(847, 477);
            this.btnPage2Back.Name = "btnPage2Back";
            this.btnPage2Back.Size = new System.Drawing.Size(75, 23);
            this.btnPage2Back.TabIndex = 8;
            this.btnPage2Back.Text = "Back";
            this.btnPage2Back.UseVisualStyleBackColor = true;
            this.btnPage2Back.Click += new System.EventHandler(this.btnPage2Back_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnCreateProject);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.pictureBox3);
            this.tabPage3.Controls.Add(this.textBoxController);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.textBoxCompany);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.btnProjectFolder);
            this.tabPage3.Controls.Add(this.textBoxProjectFolder);
            this.tabPage3.Controls.Add(this.textBoxModule);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.btnPage3Next);
            this.tabPage3.Controls.Add(this.btnPage3Back);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1014, 506);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnCreateProject
            // 
            this.btnCreateProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnCreateProject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateProject.Location = new System.Drawing.Point(770, 209);
            this.btnCreateProject.Name = "btnCreateProject";
            this.btnCreateProject.Size = new System.Drawing.Size(146, 26);
            this.btnCreateProject.TabIndex = 15;
            this.btnCreateProject.Text = "Create project";
            this.btnCreateProject.UseVisualStyleBackColor = false;
            this.btnCreateProject.Click += new System.EventHandler(this.btnCreateProject_Click);
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Location = new System.Drawing.Point(17, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1004, 1);
            this.label13.TabIndex = 20;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.InitialImage")));
            this.pictureBox3.Location = new System.Drawing.Point(882, -22);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(141, 119);
            this.pictureBox3.TabIndex = 18;
            this.pictureBox3.TabStop = false;
            // 
            // textBoxController
            // 
            this.textBoxController.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxController.Location = new System.Drawing.Point(159, 178);
            this.textBoxController.Name = "textBoxController";
            this.textBoxController.Size = new System.Drawing.Size(347, 26);
            this.textBoxController.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(32, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "Controller Name";
            // 
            // textBoxCompany
            // 
            this.textBoxCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCompany.Location = new System.Drawing.Point(159, 147);
            this.textBoxCompany.Name = "textBoxCompany";
            this.textBoxCompany.Size = new System.Drawing.Size(347, 26);
            this.textBoxCompany.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Company";
            // 
            // btnProjectFolder
            // 
            this.btnProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProjectFolder.Location = new System.Drawing.Point(618, 209);
            this.btnProjectFolder.Name = "btnProjectFolder";
            this.btnProjectFolder.Size = new System.Drawing.Size(146, 26);
            this.btnProjectFolder.TabIndex = 14;
            this.btnProjectFolder.Text = "Project folder...";
            this.btnProjectFolder.UseVisualStyleBackColor = true;
            this.btnProjectFolder.Click += new System.EventHandler(this.btnProjectFolder_Click);
            // 
            // textBoxProjectFolder
            // 
            this.textBoxProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxProjectFolder.Location = new System.Drawing.Point(159, 209);
            this.textBoxProjectFolder.Name = "textBoxProjectFolder";
            this.textBoxProjectFolder.Size = new System.Drawing.Size(444, 26);
            this.textBoxProjectFolder.TabIndex = 13;
            // 
            // textBoxModule
            // 
            this.textBoxModule.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxModule.Location = new System.Drawing.Point(159, 116);
            this.textBoxModule.Name = "textBoxModule";
            this.textBoxModule.Size = new System.Drawing.Size(78, 26);
            this.textBoxModule.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(32, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 20);
            this.label7.TabIndex = 9;
            this.label7.Text = "Project Folder";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(32, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Module Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(32, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(483, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "Step 3. Name your module && choose project folder";
            // 
            // btnPage3Next
            // 
            this.btnPage3Next.Location = new System.Drawing.Point(933, 477);
            this.btnPage3Next.Name = "btnPage3Next";
            this.btnPage3Next.Size = new System.Drawing.Size(75, 23);
            this.btnPage3Next.TabIndex = 15;
            this.btnPage3Next.Text = "Exit";
            this.btnPage3Next.UseVisualStyleBackColor = true;
            this.btnPage3Next.Click += new System.EventHandler(this.btnPage3Next_Click_1);
            // 
            // btnPage3Back
            // 
            this.btnPage3Back.Location = new System.Drawing.Point(852, 477);
            this.btnPage3Back.Name = "btnPage3Back";
            this.btnPage3Back.Size = new System.Drawing.Size(75, 23);
            this.btnPage3Back.TabIndex = 14;
            this.btnPage3Back.Text = "Back";
            this.btnPage3Back.UseVisualStyleBackColor = true;
            this.btnPage3Back.Click += new System.EventHandler(this.btnPage3Back_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(12, -2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(274, 32);
            this.label12.TabIndex = 1;
            this.label12.Text = "WebAPI Subclass Wizard";
            // 
            // Wizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1044, 594);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.WizardTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Wizard_FormClosing);
            this.WizardTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnPage1Next;
        private WizardTabControl WizardTab;
        //private System.Windows.Forms.TabControl WizardTab;
        private System.Windows.Forms.Button btnPage2Next;
        private System.Windows.Forms.Button btnPage2Back;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnPage3Next;
        private System.Windows.Forms.Button btnPage3Back;
        private System.Windows.Forms.ComboBox dropdownlistModules;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnProjectFolder;
        private System.Windows.Forms.TextBox textBoxProjectFolder;
        private System.Windows.Forms.TextBox textBoxModule;
        private System.Windows.Forms.ListBox listBoxClasses;
        private System.Windows.Forms.TextBox textBoxCompany;
        private System.Windows.Forms.Label label3;
        private TextBox textBoxController;
        private Label label9;
        private PictureBox pictureBox1;
        private Label label10;
        private Label label11;
        private PictureBox pictureBox2;
        private Label label13;
        private PictureBox pictureBox3;
        private Label label12;
        private Button btnCreateProject;
    }
}

