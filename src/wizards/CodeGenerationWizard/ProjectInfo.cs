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

using System.Collections.Generic;
using EnvDTE;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Class to hold information for projects to be modified with generated code </summary>
    public class ProjectInfo
    {
        #region Public Properties
        /// <summary> Project Folder Name </summary>
        public string ProjectFolder { get; set; }

        /// <summary> ProjectName without Extension </summary>
        public string ProjectName { get; set; }

        /// <summary> Project Model</summary>
        public Project Project { get; set; }

        /// <summary> Subfolders per project</summary>
        public Dictionary<string, string> Subfolders { get; set; }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
