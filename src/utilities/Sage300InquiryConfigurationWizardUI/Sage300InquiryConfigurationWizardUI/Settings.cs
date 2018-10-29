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
using System.IO;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    public class Settings
    {
        #region Public Properties
        public string IniFilePath { get; set; }
        public string Option { get; set; }
        public string RootPath { get; set; }
        public string DatasourceConfigurationFile { get; set; }
        public string TemplateConfigurationFile { get; set; }
        public string SQLScriptName { get; set; }
        public string OutputPath { get; set; }
        public string Company { get; set; }
        public string Version { get; set; }
        public bool IncludeFra { get; set; }
        public bool IncludeEsn { get; set; }
        public bool IncludeCht { get; set; }
        public bool IncludeChn { get; set; }

        public string TrueOutputPath
        {
            get
            {
                return Path.Combine(OutputPath, SQLScriptName);
            }
        }

        public bool DisplayOutputFolderOnCompletion { get; set; }

        public bool DisplayLogFileOnCompletion { get; set; }
        #endregion

        #region Constructor(s)
        public Settings()
        {
            IniFilePath = String.Empty;
            Option = "Adhoc";
            RootPath = String.Empty;
            DatasourceConfigurationFile = String.Empty;
            TemplateConfigurationFile = String.Empty;
            SQLScriptName = String.Empty;
            OutputPath = String.Empty;
            Company = String.Empty;
            Version = String.Empty;
            IncludeFra = false;
            IncludeEsn = false;
            IncludeCht = false;
            IncludeChn = false;
            DisplayOutputFolderOnCompletion = true;
            DisplayLogFileOnCompletion = true;
        }
        #endregion
    }
}
