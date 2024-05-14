namespace Sage.CA.SBS.ERP.Sage300.FinderGenerator
{
    partial class FinderDefinitionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtViewId = new MetroFramework.Controls.MetroTextBox();
            this.cboKeys = new MetroFramework.Controls.MetroComboBox();
            this.chklstReturnFields = new System.Windows.Forms.CheckedListBox();
            this.chklstDisplayFields = new System.Windows.Forms.CheckedListBox();
            this.lblDisplayField = new MetroFramework.Controls.MetroLabel();
            this.lblReturnField = new MetroFramework.Controls.MetroLabel();
            this.btnConfirm = new MetroFramework.Controls.MetroButton();
            this.txtCompany = new MetroFramework.Controls.MetroTextBox();
            this.mskPassword = new MetroFramework.Controls.MetroTextBox();
            this.txtUserName = new MetroFramework.Controls.MetroTextBox();
            this.definitionFileTxt = new MetroFramework.Controls.MetroTextBox();
            this.separatorLbl = new MetroFramework.Controls.MetroLabel();
            this.lstFinder = new System.Windows.Forms.ListBox();
            this.lblFinderList = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.txtFinderName = new MetroFramework.Controls.MetroTextBox();
            this.txtFinderModule = new MetroFramework.Controls.MetroTextBox();
            this.btnNew = new MetroFramework.Controls.MetroButton();
            this.btmInsert = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // txtViewId
            // 
            this.txtViewId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtViewId.CustomButton.Image = null;
            this.txtViewId.CustomButton.Location = new System.Drawing.Point(194, 1);
            this.txtViewId.CustomButton.Name = "";
            this.txtViewId.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtViewId.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtViewId.CustomButton.TabIndex = 1;
            this.txtViewId.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtViewId.CustomButton.UseSelectable = true;
            this.txtViewId.CustomButton.Visible = false;
            this.txtViewId.Lines = new string[0];
            this.txtViewId.Location = new System.Drawing.Point(242, 176);
            this.txtViewId.MaxLength = 32767;
            this.txtViewId.Name = "txtViewId";
            this.txtViewId.PasswordChar = '\0';
            this.txtViewId.PromptText = "View ID";
            this.txtViewId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtViewId.SelectedText = "";
            this.txtViewId.SelectionLength = 0;
            this.txtViewId.SelectionStart = 0;
            this.txtViewId.ShortcutsEnabled = true;
            this.txtViewId.Size = new System.Drawing.Size(218, 25);
            this.txtViewId.Style = MetroFramework.MetroColorStyle.Green;
            this.txtViewId.TabIndex = 10;
            this.txtViewId.UseSelectable = true;
            this.txtViewId.WaterMark = "View ID";
            this.txtViewId.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtViewId.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtViewId.Click += new System.EventHandler(this.txtViewId_Click);
            this.txtViewId.Leave += new System.EventHandler(this.txtViewId_Leave);
            // 
            // cboKeys
            // 
            this.cboKeys.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cboKeys.FormattingEnabled = true;
            this.cboKeys.ItemHeight = 19;
            this.cboKeys.Location = new System.Drawing.Point(242, 218);
            this.cboKeys.Name = "cboKeys";
            this.cboKeys.PromptText = "Finder View Order";
            this.cboKeys.Size = new System.Drawing.Size(218, 25);
            this.cboKeys.Style = MetroFramework.MetroColorStyle.Green;
            this.cboKeys.TabIndex = 11;
            this.cboKeys.UseSelectable = true;
            // 
            // chklstReturnFields
            // 
            this.chklstReturnFields.AllowDrop = true;
            this.chklstReturnFields.CheckOnClick = true;
            this.chklstReturnFields.FormattingEnabled = true;
            this.chklstReturnFields.Location = new System.Drawing.Point(466, 275);
            this.chklstReturnFields.Name = "chklstReturnFields";
            this.chklstReturnFields.Size = new System.Drawing.Size(184, 154);
            this.chklstReturnFields.TabIndex = 12;
            this.chklstReturnFields.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox_DragDrop);
            this.chklstReturnFields.DragOver += new System.Windows.Forms.DragEventHandler(this.listBox_DragOver);
            this.chklstReturnFields.Leave += new System.EventHandler(this.chklstReturnFields_Leave);
            this.chklstReturnFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // chklstDisplayFields
            // 
            this.chklstDisplayFields.AllowDrop = true;
            this.chklstDisplayFields.CheckOnClick = true;
            this.chklstDisplayFields.FormattingEnabled = true;
            this.chklstDisplayFields.Location = new System.Drawing.Point(242, 275);
            this.chklstDisplayFields.Name = "chklstDisplayFields";
            this.chklstDisplayFields.Size = new System.Drawing.Size(184, 154);
            this.chklstDisplayFields.TabIndex = 11;
            this.chklstDisplayFields.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox_DragDrop);
            this.chklstDisplayFields.DragOver += new System.Windows.Forms.DragEventHandler(this.listBox_DragOver);
            this.chklstDisplayFields.Leave += new System.EventHandler(this.chklstDisplayFields_Leave);
            this.chklstDisplayFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            // 
            // lblDisplayField
            // 
            this.lblDisplayField.AutoSize = true;
            this.lblDisplayField.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblDisplayField.Location = new System.Drawing.Point(242, 253);
            this.lblDisplayField.Name = "lblDisplayField";
            this.lblDisplayField.Size = new System.Drawing.Size(100, 19);
            this.lblDisplayField.TabIndex = 13;
            this.lblDisplayField.Text = "Display Fields";
            // 
            // lblReturnField
            // 
            this.lblReturnField.AutoSize = true;
            this.lblReturnField.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblReturnField.Location = new System.Drawing.Point(466, 253);
            this.lblReturnField.Name = "lblReturnField";
            this.lblReturnField.Size = new System.Drawing.Size(95, 19);
            this.lblReturnField.TabIndex = 14;
            this.lblReturnField.Text = "Return Fields";
            // 
            // btnConfirm
            // 
            this.btnConfirm.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnConfirm.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnConfirm.Highlight = true;
            this.btnConfirm.Location = new System.Drawing.Point(564, 12);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(86, 23);
            this.btnConfirm.Style = MetroFramework.MetroColorStyle.Green;
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "Login";
            this.btnConfirm.UseSelectable = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtCompany
            // 
            this.txtCompany.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtCompany.CustomButton.Image = null;
            this.txtCompany.CustomButton.Location = new System.Drawing.Point(148, 1);
            this.txtCompany.CustomButton.Name = "";
            this.txtCompany.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCompany.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCompany.CustomButton.TabIndex = 1;
            this.txtCompany.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCompany.CustomButton.UseSelectable = true;
            this.txtCompany.CustomButton.Visible = false;
            this.txtCompany.Lines = new string[0];
            this.txtCompany.Location = new System.Drawing.Point(367, 12);
            this.txtCompany.MaxLength = 32767;
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.PasswordChar = '\0';
            this.txtCompany.PromptText = "Company";
            this.txtCompany.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCompany.SelectedText = "";
            this.txtCompany.SelectionLength = 0;
            this.txtCompany.SelectionStart = 0;
            this.txtCompany.ShortcutsEnabled = true;
            this.txtCompany.Size = new System.Drawing.Size(170, 23);
            this.txtCompany.Style = MetroFramework.MetroColorStyle.Green;
            this.txtCompany.TabIndex = 3;
            this.txtCompany.UseSelectable = true;
            this.txtCompany.WaterMark = "Company";
            this.txtCompany.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCompany.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // mskPassword
            // 
            // 
            // 
            // 
            this.mskPassword.CustomButton.Image = null;
            this.mskPassword.CustomButton.Location = new System.Drawing.Point(152, 1);
            this.mskPassword.CustomButton.Name = "";
            this.mskPassword.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.mskPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.mskPassword.CustomButton.TabIndex = 1;
            this.mskPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.mskPassword.CustomButton.UseSelectable = true;
            this.mskPassword.CustomButton.Visible = false;
            this.mskPassword.Lines = new string[0];
            this.mskPassword.Location = new System.Drawing.Point(187, 12);
            this.mskPassword.MaxLength = 32767;
            this.mskPassword.Name = "mskPassword";
            this.mskPassword.PasswordChar = '*';
            this.mskPassword.PromptText = "Password";
            this.mskPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.mskPassword.SelectedText = "";
            this.mskPassword.SelectionLength = 0;
            this.mskPassword.SelectionStart = 0;
            this.mskPassword.ShortcutsEnabled = true;
            this.mskPassword.Size = new System.Drawing.Size(174, 23);
            this.mskPassword.Style = MetroFramework.MetroColorStyle.Green;
            this.mskPassword.TabIndex = 2;
            this.mskPassword.UseSelectable = true;
            this.mskPassword.WaterMark = "Password";
            this.mskPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.mskPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtUserName
            // 
            this.txtUserName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            // 
            // 
            // 
            this.txtUserName.CustomButton.Image = null;
            this.txtUserName.CustomButton.Location = new System.Drawing.Point(147, 1);
            this.txtUserName.CustomButton.Name = "";
            this.txtUserName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtUserName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtUserName.CustomButton.TabIndex = 1;
            this.txtUserName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtUserName.CustomButton.UseSelectable = true;
            this.txtUserName.CustomButton.Visible = false;
            this.txtUserName.Lines = new string[0];
            this.txtUserName.Location = new System.Drawing.Point(12, 12);
            this.txtUserName.MaxLength = 32767;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.PasswordChar = '\0';
            this.txtUserName.PromptText = "User Name";
            this.txtUserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUserName.SelectedText = "";
            this.txtUserName.SelectionLength = 0;
            this.txtUserName.SelectionStart = 0;
            this.txtUserName.ShortcutsEnabled = true;
            this.txtUserName.Size = new System.Drawing.Size(169, 23);
            this.txtUserName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtUserName.TabIndex = 1;
            this.txtUserName.UseSelectable = true;
            this.txtUserName.WaterMark = "User Name";
            this.txtUserName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtUserName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // definitionFileTxt
            // 
            // 
            // 
            // 
            this.definitionFileTxt.CustomButton.Image = null;
            this.definitionFileTxt.CustomButton.Location = new System.Drawing.Point(614, 1);
            this.definitionFileTxt.CustomButton.Name = "";
            this.definitionFileTxt.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.definitionFileTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.definitionFileTxt.CustomButton.TabIndex = 1;
            this.definitionFileTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.definitionFileTxt.CustomButton.UseSelectable = true;
            this.definitionFileTxt.Lines = new string[0];
            this.definitionFileTxt.Location = new System.Drawing.Point(12, 51);
            this.definitionFileTxt.MaxLength = 32767;
            this.definitionFileTxt.Name = "definitionFileTxt";
            this.definitionFileTxt.PasswordChar = '\0';
            this.definitionFileTxt.PromptText = "Definition File";
            this.definitionFileTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.definitionFileTxt.SelectedText = "";
            this.definitionFileTxt.SelectionLength = 0;
            this.definitionFileTxt.SelectionStart = 0;
            this.definitionFileTxt.ShortcutsEnabled = true;
            this.definitionFileTxt.ShowButton = true;
            this.definitionFileTxt.Size = new System.Drawing.Size(638, 25);
            this.definitionFileTxt.Style = MetroFramework.MetroColorStyle.Green;
            this.definitionFileTxt.TabIndex = 5;
            this.definitionFileTxt.UseSelectable = true;
            this.definitionFileTxt.WaterMark = "Definition File";
            this.definitionFileTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.definitionFileTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.definitionFileTxt.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.fileBtn_Click);
            this.definitionFileTxt.Click += new System.EventHandler(this.definitionFileTxt_Click);
            // 
            // separatorLbl
            // 
            this.separatorLbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLbl.Location = new System.Drawing.Point(12, 95);
            this.separatorLbl.Name = "separatorLbl";
            this.separatorLbl.Size = new System.Drawing.Size(638, 1);
            this.separatorLbl.TabIndex = 19;
            // 
            // lstFinder
            // 
            this.lstFinder.FormattingEnabled = true;
            this.lstFinder.Location = new System.Drawing.Point(12, 139);
            this.lstFinder.Name = "lstFinder";
            this.lstFinder.Size = new System.Drawing.Size(198, 290);
            this.lstFinder.TabIndex = 7;
            this.lstFinder.SelectedIndexChanged += new System.EventHandler(this.lstFinder_SelectedIndexChanged);
            // 
            // lblFinderList
            // 
            this.lblFinderList.AutoSize = true;
            this.lblFinderList.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblFinderList.Location = new System.Drawing.Point(12, 117);
            this.lblFinderList.Name = "lblFinderList";
            this.lblFinderList.Size = new System.Drawing.Size(77, 19);
            this.lblFinderList.TabIndex = 21;
            this.lblFinderList.Text = "Finder List";
            // 
            // metroLabel1
            // 
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(472, 190);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(178, 53);
            this.metroLabel1.TabIndex = 22;
            this.metroLabel1.Text = "* Use right mouse button to drag to reorder fields";
            this.metroLabel1.WrapToLine = true;
            // 
            // txtFinderName
            // 
            // 
            // 
            // 
            this.txtFinderName.CustomButton.Image = null;
            this.txtFinderName.CustomButton.Location = new System.Drawing.Point(121, 1);
            this.txtFinderName.CustomButton.Name = "";
            this.txtFinderName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtFinderName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFinderName.CustomButton.TabIndex = 1;
            this.txtFinderName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFinderName.CustomButton.UseSelectable = true;
            this.txtFinderName.CustomButton.Visible = false;
            this.txtFinderName.Lines = new string[0];
            this.txtFinderName.Location = new System.Drawing.Point(315, 135);
            this.txtFinderName.MaxLength = 32767;
            this.txtFinderName.Name = "txtFinderName";
            this.txtFinderName.PasswordChar = '\0';
            this.txtFinderName.PromptText = "FinderName";
            this.txtFinderName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFinderName.SelectedText = "";
            this.txtFinderName.SelectionLength = 0;
            this.txtFinderName.SelectionStart = 0;
            this.txtFinderName.ShortcutsEnabled = true;
            this.txtFinderName.Size = new System.Drawing.Size(145, 25);
            this.txtFinderName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtFinderName.TabIndex = 9;
            this.txtFinderName.UseSelectable = true;
            this.txtFinderName.WaterMark = "FinderName";
            this.txtFinderName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFinderName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtFinderModule
            // 
            // 
            // 
            // 
            this.txtFinderModule.CustomButton.Image = null;
            this.txtFinderModule.CustomButton.Location = new System.Drawing.Point(43, 1);
            this.txtFinderModule.CustomButton.Name = "";
            this.txtFinderModule.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtFinderModule.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFinderModule.CustomButton.TabIndex = 1;
            this.txtFinderModule.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFinderModule.CustomButton.UseSelectable = true;
            this.txtFinderModule.CustomButton.Visible = false;
            this.txtFinderModule.Lines = new string[0];
            this.txtFinderModule.Location = new System.Drawing.Point(242, 135);
            this.txtFinderModule.MaxLength = 2;
            this.txtFinderModule.Name = "txtFinderModule";
            this.txtFinderModule.PasswordChar = '\0';
            this.txtFinderModule.PromptText = "Module";
            this.txtFinderModule.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFinderModule.SelectedText = "";
            this.txtFinderModule.SelectionLength = 0;
            this.txtFinderModule.SelectionStart = 0;
            this.txtFinderModule.ShortcutsEnabled = true;
            this.txtFinderModule.Size = new System.Drawing.Size(67, 25);
            this.txtFinderModule.Style = MetroFramework.MetroColorStyle.Green;
            this.txtFinderModule.TabIndex = 8;
            this.txtFinderModule.UseSelectable = true;
            this.txtFinderModule.WaterMark = "Module";
            this.txtFinderModule.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFinderModule.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtFinderModule.Click += new System.EventHandler(this.txtFinderModule_Click);
            // 
            // btnNew
            // 
            this.btnNew.Enabled = false;
            this.btnNew.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnNew.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnNew.Highlight = true;
            this.btnNew.Location = new System.Drawing.Point(472, 135);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(86, 25);
            this.btnNew.Style = MetroFramework.MetroColorStyle.Green;
            this.btnNew.TabIndex = 12;
            this.btnNew.Text = "New";
            this.btnNew.UseSelectable = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btmInsert
            // 
            this.btmInsert.Enabled = false;
            this.btmInsert.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btmInsert.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btmInsert.Highlight = true;
            this.btmInsert.Location = new System.Drawing.Point(564, 135);
            this.btmInsert.Name = "btmInsert";
            this.btmInsert.Size = new System.Drawing.Size(86, 25);
            this.btmInsert.Style = MetroFramework.MetroColorStyle.Green;
            this.btmInsert.TabIndex = 13;
            this.btmInsert.Text = "Insert";
            this.btmInsert.UseSelectable = true;
            this.btmInsert.Click += new System.EventHandler(this.btmInsert_Click);
            // 
            // FinderDefinitionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btmInsert);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtFinderModule);
            this.Controls.Add(this.txtFinderName);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.lblFinderList);
            this.Controls.Add(this.lstFinder);
            this.Controls.Add(this.separatorLbl);
            this.Controls.Add(this.definitionFileTxt);
            this.Controls.Add(this.txtCompany);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.mskPassword);
            this.Controls.Add(this.lblReturnField);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblDisplayField);
            this.Controls.Add(this.chklstDisplayFields);
            this.Controls.Add(this.chklstReturnFields);
            this.Controls.Add(this.cboKeys);
            this.Controls.Add(this.txtViewId);
            this.Name = "FinderDefinitionControl";
            this.Size = new System.Drawing.Size(661, 449);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroTextBox txtViewId;
        private MetroFramework.Controls.MetroComboBox cboKeys;
        private System.Windows.Forms.CheckedListBox chklstReturnFields;
        private System.Windows.Forms.CheckedListBox chklstDisplayFields;
        private MetroFramework.Controls.MetroLabel lblDisplayField;
        private MetroFramework.Controls.MetroLabel lblReturnField;
        private MetroFramework.Controls.MetroButton btnConfirm;
        private MetroFramework.Controls.MetroTextBox txtCompany;
        private MetroFramework.Controls.MetroTextBox mskPassword;
        private MetroFramework.Controls.MetroTextBox txtUserName;
        private MetroFramework.Controls.MetroTextBox definitionFileTxt;
        private MetroFramework.Controls.MetroLabel separatorLbl;
        private System.Windows.Forms.ListBox lstFinder;
        private MetroFramework.Controls.MetroLabel lblFinderList;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox txtFinderName;
        private MetroFramework.Controls.MetroTextBox txtFinderModule;
        private MetroFramework.Controls.MetroButton btnNew;
        private MetroFramework.Controls.MetroButton btmInsert;
    }
}
