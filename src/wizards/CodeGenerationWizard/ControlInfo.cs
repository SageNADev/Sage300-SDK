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
using System.Windows.Forms;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Class to hold properties for dragged business property </summary>
    public class ControlInfo
    {
        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public ControlInfo()
        {
            Text = string.Empty;
            BusinessField = null;
            ParentNodeName = string.Empty;
        }
        #endregion
        #region Public Properties
        /// <summary> Control </summary>
        public Control Control { get; set; }
        /// <summary> Text for non-businessField controls </summary>
        public string Text { get; set; }
        /// <summary> Business Field for model property </summary>
        public BusinessField BusinessField { get; set; }
        /// <summary> Widget </summary>
        public string Widget { get; set; }
        /// <summary> Selected tree node's parent name </summary>
        public string ParentNodeName { get; set; }
        /// <summary> Selected node </summary>
        public TreeNode Node { get; set; }
        /// <summary> Finder configuration name </summary>
        public string FinderFileName { get; set; }
        /// <summary> Finder file name </summary>
        public string FinderName { get; set; }
        /// <summary> Finder display field </summary>
        public string FinderDisplayField { get; set; }
        #endregion
    }
}
