// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    public static class Constants
    {
        /// <summary>
        /// These are common constants that won't
        /// vary between releases of the Upgrade Wizard
        /// </summary>
        public static class Common
        {
            /// <summary> Web Suffix </summary>
            public const string WebSuffix = ".web";

            /// <summary> Web Folder Suffix </summary>
            public const string WebFolderSuffix = "Web";

            /// <summary> Web Zip Suffix </summary>
            public const string WebZipSuffix = "Web.zip";

            /// <summary> Upgrade Log Name </summary>
            public const string LogFileName = "UpgradeLog.txt";

            /// <summary> Accpac Property File </summary>
            public const string AccpacPropsFile = "AccpacDotNetVersion.props";

            /// <summary> Just a dummy ModuleId </summary>
            public const string DummyModuleId = "XX";

            /// <summary> 
            /// This is the version of the Microsoft .NET Framework 
            /// to target for all projects in the solution being upgraded.
            /// </summary>
            public const string TargetedDotNetFrameworkVersion = "4.7.2";
            public const string TargetFrameworkMoniker = ".NETFramework,Version=v4.7.2";
        }

        /// <summary>
        /// These are per-release constants and will likely
        /// change for each release of the Upgrade Wizard
        /// </summary>
        public static class PerRelease
        {
            /// <summary> From Release Number </summary>
            public const string FromReleaseNumber = "2019.2";

            /// <summary> To Release Number </summary>
            public const string ToReleaseNumber = "2020.0";

            /// <summary> From Accpac Number </summary>
            public const string FromAccpacNumber = "6.6.0.10";

            /// <summary> To Accpac Number </summary>
            public const string ToAccpacNumber = "6.7.0.0";
        }
    }
}
