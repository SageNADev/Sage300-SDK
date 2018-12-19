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
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    public class BorderedTextBox : UserControl
    {
        TextBox textBox;

        /// <summary>
        /// Constructor
        /// </summary>
        public BorderedTextBox()
        {
            textBox = new TextBox()
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(-1, -1),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right
            };
            Control container = new ContainerControl()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(-1)
            };
            container.Controls.Add(textBox);
            this.Controls.Add(container);

            DefaultBorderColor = SystemColors.InactiveBorder;
            FocusedBorderColor = Color.Blue;
            ErrorBorderColor = Color.Red;
            BackColor = DefaultBorderColor;
            Padding = new Padding(1);
            Size = textBox.Size;
        }

        public Color DefaultBorderColor { get; set; }

        public Color FocusedBorderColor { get; set; }

        public Color ErrorBorderColor { get; set; }

        private bool _isError;
        public void SetError(bool set)
        {
            _isError = set;
            BackColor = _isError ? ErrorBorderColor : DefaultBorderColor;
        }

        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public CharacterCasing CharacterCasing
        {
            get { return textBox.CharacterCasing; }
            set { textBox.CharacterCasing = value; }
        }

        public char PasswordChar
        {
            get { return textBox.PasswordChar; }
            set { textBox.PasswordChar = value; }
        }

        protected override void OnEnter(EventArgs e)
        {
            BackColor = _isError ? ErrorBorderColor : FocusedBorderColor;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            BackColor = _isError ? ErrorBorderColor : DefaultBorderColor;
            base.OnLeave(e);
        }

        protected override void SetBoundsCore(int x, int y,
            int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, textBox.PreferredHeight, specified);
        }
    }
}
