// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> UI for Container Name Modal Entry </summary>
    public partial class ContainerName : Form
    {
        #region Public Properties
        /// <summary> Container Name Property</summary>
        public string ContainerNameProperty { get; set; }
        #endregion

        #region Constructor
        /// <summary> Specifies container name </summary>
        public ContainerName(string containerName)
        {
            InitializeComponent();
            Localize();

            // Assign value if any
            txtContainerName.Text = containerName;
        }
        #endregion

        #region Private Routines
        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.CodeGeneration;

            btnSave.Text = Resources.Save;
            btnCancel.Text = Resources.Cancel;

            // Code Type Step
            lblContainerName.Text = Resources.ContainerName;
            tooltip.SetToolTip(lblContainerName, Resources.ContainerNameTip);

        }

        /// <summary> Save Container Name</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ContainerNameProperty = txtContainerName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary> Save Container Name</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary> Ensure no invalid characters</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtContainerName_Leave(object sender, EventArgs e)
        {
            txtContainerName.Text = BusinessViewHelper.Replace(txtContainerName.Text);
        }
        #endregion
    }
}
