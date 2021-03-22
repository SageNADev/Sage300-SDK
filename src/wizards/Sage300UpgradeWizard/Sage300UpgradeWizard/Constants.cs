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

#region Imports
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    public static class Constants
    {
        /// <summary>
        /// These are per-release constants and will likely
        /// change for each release of the Upgrade Wizard
        /// </summary>
        public static class PerRelease
        {
            /// <summary> From Release Number </summary>
            public const string FromReleaseNumber = "2021.2";

            /// <summary> To Release Number </summary>
            public const string ToReleaseNumber = "2022.0";

            /// <summary> From Accpac Number </summary>
            public const string FromAccpacNumber = "6.8.0.0";

            /// <summary> To Accpac Number </summary>
            public const string ToAccpacNumber = "6.9.0.0";

            /// <summary> Flag that determines whether or not to synchronize the Kendo files. </summary>
            public const bool SyncKendoFiles = true;

            /// <summary> Flag that determines whether or not to synchronize the Web files. </summary>
            public const bool SyncWebFiles = true;

            /// <summary> Flag that determines whether or not to update the Accpac .NET library. </summary>
            public const bool UpdateAccpacDotNetLibrary = false;

            /// <summary> Flag that determines whether or not to update the .NET framework in solution projects. </summary>
            public const bool UpdateMicrosoftDotNetFramework = false;

            /// <summary> Flag that determines whether or not to execute the 'UpdateUnifyDisabled' process. </summary>
            public const bool UpdateUnifyDisabled = false;

            /// <summary> 
            /// Flag that determines whether or not to add a new file called 'BinInclude.txt'
            /// to the root of the Web project.
            /// </summary>
            public const bool AddBinIncludeFile = false;

            /// <summary> 
            /// Release 2021.2
            /// 
            /// Flag that determines whether or not to add a  
            /// new reference to the web project for handling reports
            /// </summary>
            public const bool ReportUpgrade_For_2021_2 = true;

            /// <summary> Flag that determines whether or not to remove previous versions of the various JQuery libraries. </summary>
            public const bool RemovePreviousJqueryLibraries = false;

            public const string FromJqueryCoreVersion = "1.11.3";
            public const string FromJqueryUIVersion = "1.11.4";
            public const string FromJqueryMigrateVersion = "1.2.1";

            public const string ToJqueryCoreVersion = "3.4.1";
            public const string ToJqueryUIVersion = "1.12.1";
            public const string ToJqueryMigrateVersion = "3.1.0";
        }

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

            // <summary> The name of the 'Scripts' folder </summary>
            public const string ScriptsFolderName = "Scripts";

            /// <summary> 
            /// This is the version of the Microsoft .NET Framework 
            /// to target for all projects in the solution being upgraded.
            /// </summary>
            public const string TargetedDotNetFrameworkVersion = "4.8";
            public const string TargetFrameworkMoniker = ".NETFramework,Version=v4.8";

            /// <summary>
            /// The name of the file that will contain the names of additional 
            /// files that can be deployed during build.
            /// </summary>
            public const string BinIncludeFile = "BinInclude.txt";

            /// <summary> 
            /// Name of the nuget project that shows up in the list of projects in the solution
            /// when attempting to upgrade the targeted .NET framework. An exception is
            /// generated when attempting to 'update' this project. This setting
            /// is used so we can exclude/ignore this project when processing.
            /// </summary>
            public const string NugetName = ".nuget";

            /// <summary> 
            /// Flag that determines whether or not to allow backups of the original solution and projects. 
            /// 
            /// true : The ability to perform a backup is enabled
            /// false : The ability to perform a backup is disabled (and hidden from the UI)
            /// </summary>
            public const bool EnableSolutionBackup = true;

            /// <summary>
            /// The search pattern for the solutions web project
            /// </summary>
            public const string WebProjectNamePattern = @".web.csproj";
        }
    }
}
