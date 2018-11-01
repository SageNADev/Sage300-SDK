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

#region Imports
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationGenerator.Forms
{
    public partial class ModalMessageBox : Form
    {
        public string Caption
        {
            set
            {
                this.Text = value;
            }
            get
            {
                return this.Text;
            }
        }

        public string Title
        {
            set
            {
                this.lblTitle.Text = value;
            }
            get
            {
                return this.lblTitle.Text;
            }
        }

        public string Message
        {
            set
            {
                this.txtMessage.Text = value;
            }
            get
            {
                return this.txtMessage.Text;
            }
        }

        public MessageBoxButtons Buttons
        {
            set
            {
                switch (value)
                {
                    case MessageBoxButtons.OK:
                        btn1.Text = "OK";
                        btn1.DialogResult = DialogResult.OK;
                        btn2.Visible = false;
                        break;

                    case MessageBoxButtons.OKCancel:
                        btn2.Text = "OK";
                        btn2.DialogResult = DialogResult.OK;
                        btn1.Text = "Cancel";
                        btn1.DialogResult = DialogResult.Cancel;
                        break;
                }
            }
        }

        public ModalMessageBox()
        {
            InitializeComponent();
            btn3.Visible = false;
            btn1.Cursor = Cursors.Hand;
            btn2.Cursor = Cursors.Hand;
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}
