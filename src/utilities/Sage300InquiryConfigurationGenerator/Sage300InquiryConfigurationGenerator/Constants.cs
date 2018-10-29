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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    public static class Constants
    {
        #region UI Colors
        public static readonly Color OptionButtonSelected_ForegroundColor = Color.White;
        public static readonly Color OptionButtonSelected_BackgroundColor = Color.DarkGreen;

        public static readonly Color OptionButtonUnselected_ForegroundColor = Color.White;
        public static readonly Color OptionButtonUnselected_BackgroundColor = Color.LightGray;

        public static readonly Color GroupInactive_BackgroundColor = Color.White;
        public static readonly Color GroupActive_BackgroundColor = Color.FromArgb(252, 252, 252);

        public static readonly Color ValidationError_ForegroundColor = Color.White;
        public static readonly Color ValidationError_BackgroundColor = Color.Red;

        public static readonly Color TextBoxBorderColor_Focus = Color.LimeGreen;
        public static readonly Color TextBoxBorderColor_NoFocus = SystemColors.InactiveBorder;
        public static readonly Color TextBoxBorderColor_Error = Color.Red;
        #endregion

        public const string LabelPostFixCharacter = ":";

        public const string DefaultIniFileName = "Sage300InquiryConfigurationGenerator.ini";

        public const string DefaultOutputFolderName = "Output";
        public const string DefaultInquiryFolderName = "InquiryConfiguration";
    }
}
