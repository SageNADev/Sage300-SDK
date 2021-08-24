// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

#region Namespaces
using System;
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard;
using System.Windows.Forms;
#endregion

namespace CodeGenerationTester
{
    public partial class Form1 : Form
    {
        private DTE2 dte2 = null;

        public Form1()
        {
            InitializeComponent();
            // Get instance of Visual Studio
            dte2 = (DTE2)Activator.CreateInstance(Type.GetTypeFromProgID("VisualStudio.DTE", true), true);
        }

        /// <summary>Launch code generation wizard </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void cmdLaunch_Click(object sender, EventArgs e)
        {
            // Open requested solution
            try
            {
                dte2.Solution.Open(txtSolution.Text);
            }
            catch (Exception ex)
            {
                // Error opening entered solution
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Invoke Code Generation wizard
            var wizard = new CodeGenerationWizard();
            wizard.Execute(dte2.Solution);
            // Close solution upon return from wizard
            dte2.Solution.Close();
        }

        /// <summary>Form is closing so release instance of Visual Studio </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dte2 != null)
            {
                dte2.Quit();
            }
        }
    }
}
