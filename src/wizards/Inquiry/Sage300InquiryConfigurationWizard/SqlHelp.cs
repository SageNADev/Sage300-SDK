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
using Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary> UI for SQL Help Modal Form </summary>
    public partial class SqlHelp : Form
    {
        #region Public Routines

        /// <summary> Modal Dialog </summary>
        public SqlHelp()
        {
            InitializeComponent();
            Localize();
        }
        #endregion

        #region Private routines
        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.InquiryConfiguration;

            tabSqlInstructions.Text = Resources.Instructions;
            tabSqlExample1.Text = Resources.Example1;
            tabSqlExample2.Text = Resources.Example2;
            tabSqlExample3.Text = Resources.Example3;

            lblInstructions.Text = Environment.NewLine + Resources.SqlInstructions1 +
                Environment.NewLine + Environment.NewLine + Resources.SqlInstructions2 +
                Environment.NewLine + Environment.NewLine + Resources.SqlInstructions3 +
                Environment.NewLine + Environment.NewLine + Resources.SqlInstructions4 +
                Environment.NewLine + Environment.NewLine + Resources.SqlInstructions5 +
                Environment.NewLine + Environment.NewLine + Resources.SqlInstructions6 + Environment.NewLine;
        }
        #endregion

    }
}
