namespace GridJSGenerator
{
    partial class Starter
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
            this.openModelAssembly = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtModelPath = new System.Windows.Forms.TextBox();
            this.tabCreate = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnToColumns = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnToOrder = new System.Windows.Forms.Button();
            this.dataGridProperties = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnToVariables = new System.Windows.Forms.Button();
            this.dataSelectedProperties = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.txtController = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUserPref = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserPrefId = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.chkHasDelete = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtJSViewModel = new System.Windows.Forms.TextBox();
            this.txtGridId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabCreate.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProperties)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSelectedProperties)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(145, 28);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(210, 20);
            this.txtClassName.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtClassName, "Name of your model");
            // 
            // txtModelPath
            // 
            this.txtModelPath.Location = new System.Drawing.Point(145, 88);
            this.txtModelPath.Name = "txtModelPath";
            this.txtModelPath.Size = new System.Drawing.Size(210, 20);
            this.txtModelPath.TabIndex = 1;
            this.txtModelPath.Text = "F:\\Sage\\Assemblies\\Sage.CA.SBS.ERP.Sage300.GL.Models.dll";
            this.toolTip1.SetToolTip(this.txtModelPath, "Model DLL file path");
            // 
            // tabCreate
            // 
            this.tabCreate.Controls.Add(this.tabPage1);
            this.tabCreate.Controls.Add(this.tabPage2);
            this.tabCreate.Controls.Add(this.tabPage3);
            this.tabCreate.Controls.Add(this.tabPage4);
            this.tabCreate.Controls.Add(this.tabPage5);
            this.tabCreate.Controls.Add(this.tabPage6);
            this.tabCreate.Location = new System.Drawing.Point(12, 22);
            this.tabCreate.Name = "tabCreate";
            this.tabCreate.SelectedIndex = 0;
            this.tabCreate.Size = new System.Drawing.Size(509, 235);
            this.tabCreate.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnToColumns);
            this.tabPage1.Controls.Add(this.txtClassName);
            this.tabPage1.Controls.Add(this.txtModelPath);
            this.tabPage1.Controls.Add(this.btnBrowse);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(501, 209);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select Model";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Model Assembly";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Model Name";
            // 
            // btnToColumns
            // 
            this.btnToColumns.Location = new System.Drawing.Point(145, 151);
            this.btnToColumns.Name = "btnToColumns";
            this.btnToColumns.Size = new System.Drawing.Size(75, 23);
            this.btnToColumns.TabIndex = 3;
            this.btnToColumns.Text = "Next";
            this.btnToColumns.UseVisualStyleBackColor = true;
            this.btnToColumns.Click += new System.EventHandler(this.btnToColumns_Click);
            // 
            // txtModelPath
            // 
            this.txtModelPath.Location = new System.Drawing.Point(145, 88);
            this.txtModelPath.Name = "txtModelPath";
            this.txtModelPath.Size = new System.Drawing.Size(210, 20);
            this.txtModelPath.TabIndex = 9;
            this.txtModelPath.Text = "F:\\Sage\\Assemblies\\Sage.CA.SBS.ERP.Sage300.GL.Models.dll";
            this.toolTip1.SetToolTip(this.txtModelPath, "Model DLL file path");
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(361, 88);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.btnToOrder);
            this.tabPage2.Controls.Add(this.dataGridProperties);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(501, 209);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Select Columns";
            // 
            // btnToOrder
            // 
            this.btnToOrder.Location = new System.Drawing.Point(197, 361);
            this.btnToOrder.Name = "btnToOrder";
            this.btnToOrder.Size = new System.Drawing.Size(75, 23);
            this.btnToOrder.TabIndex = 10;
            this.btnToOrder.Text = "Next >";
            this.btnToOrder.UseVisualStyleBackColor = true;
            this.btnToOrder.Click += new System.EventHandler(this.btnToOrder_Click);
            // 
            // dataGridProperties
            // 
            this.dataGridProperties.AllowDrop = true;
            this.dataGridProperties.AllowUserToDeleteRows = false;
            this.dataGridProperties.AllowUserToOrderColumns = true;
            this.dataGridProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridProperties.Location = new System.Drawing.Point(17, 12);
            this.dataGridProperties.MultiSelect = false;
            this.dataGridProperties.Name = "dataGridProperties";
            this.dataGridProperties.Size = new System.Drawing.Size(462, 343);
            this.dataGridProperties.TabIndex = 9;
            this.dataGridProperties.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.btnToVariables);
            this.tabPage3.Controls.Add(this.dataSelectedProperties);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(501, 209);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Select Order";
            // 
            // btnToVariables
            // 
            this.btnToVariables.Location = new System.Drawing.Point(197, 361);
            this.btnToVariables.Name = "btnToVariables";
            this.btnToVariables.Size = new System.Drawing.Size(75, 23);
            this.btnToVariables.TabIndex = 11;
            this.btnToVariables.Text = "Next >";
            this.btnToVariables.UseVisualStyleBackColor = true;
            this.btnToVariables.Click += new System.EventHandler(this.btnToVariables_Click);
            // 
            // dataSelectedProperties
            // 
            this.dataSelectedProperties.AllowDrop = true;
            this.dataSelectedProperties.AllowUserToDeleteRows = false;
            this.dataSelectedProperties.AllowUserToOrderColumns = true;
            this.dataSelectedProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataSelectedProperties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataSelectedProperties.Location = new System.Drawing.Point(17, 12);
            this.dataSelectedProperties.MultiSelect = false;
            this.dataSelectedProperties.Name = "dataSelectedProperties";
            this.dataSelectedProperties.Size = new System.Drawing.Size(462, 343);
            this.dataSelectedProperties.TabIndex = 10;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.cmbModule);
            this.tabPage4.Controls.Add(this.txtAction);
            this.tabPage4.Controls.Add(this.txtController);
            this.tabPage4.Controls.Add(this.label3);
            this.tabPage4.Controls.Add(this.chkUserPref);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.txtUserPrefId);
            this.tabPage4.Controls.Add(this.btnGenerate);
            this.tabPage4.Controls.Add(this.chkHasDelete);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.txtJSViewModel);
            this.tabPage4.Controls.Add(this.txtGridId);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.txtNameSpace);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(501, 209);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Set Variables";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Action";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Controller";
            // 
            // cmbModule
            // 
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Items.AddRange(new object[] {
            "AP",
            "AR",
            "AS",
            "CS",
            "GL",
            "IC",
            "OE",
            "PO",
            "Core",
            "Shared"});
            this.cmbModule.Location = new System.Drawing.Point(135, 91);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(128, 21);
            this.cmbModule.TabIndex = 4;
            this.cmbModule.SelectedIndexChanged += new System.EventHandler(this.cmbModule_SelectedIndexChanged);
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(135, 144);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(215, 20);
            this.txtAction.TabIndex = 6;
            // 
            // txtController
            // 
            this.txtController.Location = new System.Drawing.Point(135, 118);
            this.txtController.Name = "txtController";
            this.txtController.Size = new System.Drawing.Size(215, 20);
            this.txtController.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Module";
            // 
            // chkUserPref
            // 
            this.chkUserPref.AutoSize = true;
            this.chkUserPref.Checked = true;
            this.chkUserPref.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUserPref.Location = new System.Drawing.Point(28, 170);
            this.chkUserPref.Name = "chkUserPref";
            this.chkUserPref.Size = new System.Drawing.Size(125, 17);
            this.chkUserPref.TabIndex = 7;
            this.chkUserPref.Text = "Has User Preference";
            this.chkUserPref.UseVisualStyleBackColor = true;
            this.chkUserPref.CheckedChanged += new System.EventHandler(this.chkUserPref_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "User Prefernce ID";
            // 
            // txtUserPrefId
            // 
            this.txtUserPrefId.Location = new System.Drawing.Point(135, 193);
            this.txtUserPrefId.Name = "txtUserPrefId";
            this.txtUserPrefId.Size = new System.Drawing.Size(222, 20);
            this.txtUserPrefId.TabIndex = 8;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(167, 261);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkHasDelete
            // 
            this.chkHasDelete.AutoSize = true;
            this.chkHasDelete.Checked = true;
            this.chkHasDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasDelete.Location = new System.Drawing.Point(28, 223);
            this.chkHasDelete.Name = "chkHasDelete";
            this.chkHasDelete.Size = new System.Drawing.Size(79, 17);
            this.chkHasDelete.TabIndex = 9;
            this.chkHasDelete.Text = "Has Delete";
            this.chkHasDelete.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "JS View Model";
            // 
            // txtJSViewModel
            // 
            this.txtJSViewModel.Location = new System.Drawing.Point(135, 65);
            this.txtJSViewModel.Name = "txtJSViewModel";
            this.txtJSViewModel.Size = new System.Drawing.Size(222, 20);
            this.txtJSViewModel.TabIndex = 3;
            // 
            // txtGridId
            // 
            this.txtGridId.Location = new System.Drawing.Point(135, 13);
            this.txtGridId.Name = "txtGridId";
            this.txtGridId.Size = new System.Drawing.Size(222, 20);
            this.txtGridId.TabIndex = 1;
            this.txtGridId.Text = "Grid";
            this.txtGridId.Leave += new System.EventHandler(this.txtGridId_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Namespace";
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(135, 39);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(222, 20);
            this.txtNameSpace.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Grid ID";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage5.Controls.Add(this.richTextBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(501, 209);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Javascript";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(19, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(478, 370);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage6.Controls.Add(this.richTextBox2);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(501, 209);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "CSHTML";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(21, 15);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(478, 370);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            // 
            // Starter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 282);
            this.Controls.Add(this.tabCreate);
            this.Name = "Starter";
            this.Text = "Generate Grid javascript";
            this.tabCreate.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridProperties)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataSelectedProperties)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openModelAssembly;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabCreate;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnToColumns;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.TextBox txtModelPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnToOrder;
        private System.Windows.Forms.DataGridView dataGridProperties;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnToVariables;
        private System.Windows.Forms.DataGridView dataSelectedProperties;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.TextBox txtController;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUserPref;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserPrefId;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkHasDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtJSViewModel;
        private System.Windows.Forms.TextBox txtGridId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}

