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
using Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.Properties;
using System;
using System.IO;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard
{
    /// <summary>
    /// Class that contains methods that are common to all release cycles
    /// </summary>
    public class CommonReleaseUpgradeSteps
    {
        #region Private Variables
        /// <summary> Settings from UI </summary>
        private Settings _settings;
        private string _backupFolder = String.Empty;
        #endregion

        #region Constructor(s)
        public CommonReleaseUpgradeSteps(Settings settings)
        {
            _settings = settings;
        }
        #endregion

        #region Public Methods
        /// <summary> Synchronization of web project files </summary>
        /// <param name="title">Title of step being processed </param>
        public void SyncWebFiles(string title, out bool accpacPropsInWebFolder)
        {
            // Log start of step
            Utilities.LaunchLogEventStart(title);

            // Check to see if the AccpacDotNetVersion.props file
            // already exists in the Web folder.
            // If it does, then just update it and do not relocate it to the Project folder
            accpacPropsInWebFolder = IsAccpacDotNetVersionPropsLocatedInWebFolder();

            // Do the work :)
            Utilities.DirectoryCopy(_settings.SourceFolder, _settings.DestinationWebFolder, ignoreDestinationFolder: false);

            // Remove the files that are not actually part of the 'Web' bundle.
            // This is done because of the way VS2017 doesn't seem to allow embedding of zip
            // files within another zip file.
            File.Delete(Path.Combine(_settings.DestinationWebFolder, @"__TemplateIcon.ico"));
            File.Delete(Path.Combine(_settings.DestinationWebFolder, @"Items.vstemplate"));

            if (!accpacPropsInWebFolder)
            {
                File.Delete(Path.Combine(_settings.DestinationWebFolder, @"AccpacDotNetVersion.props"));
            }

            // Log end of step
            Utilities.LaunchLogEventEnd(title);
            Utilities.LaunchLogEvent("");
        }

        /// <summary> Upgrade project reference to use new verion Accpac.Net </summary>
        /// <param name="title">Title of step being processed </param>
        public void SyncAccpacLibraries(string title, bool accpacPropsInWebFolder)
        {
            // Log start of step
            Utilities.LaunchLogEventStart(title);

            // Only do this if the AccpacDotNetVersion.props file was not originally in the Web folder.
            if (!accpacPropsInWebFolder)
            {
                // Do the actual work :)
                RemoveExistingPropsFileFromSolutionFolder();
                CopyNewPropsFileToSolutionFolder();
            }

            // Log detail
            var txt = string.Format(Resources.UpgradeLibrary, 
                                    Constants.PerRelease.FromAccpacNumber,
                                    Constants.PerRelease.ToAccpacNumber);
            Utilities.LaunchLogEvent($"{DateTime.Now} {txt}");

            // Log end of step
            Utilities.LaunchLogEventEnd(title);
            Utilities.LaunchLogEvent("");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Is there a copy of the AccpacDotNetversion.props file in the Web project folder?
        /// </summary>
        /// <returns>
        /// true : AccpacDotNetVersion.props is in Web project folder 
        /// false: AccpacDotNetVersion.props is in not in the Web project folder 
        /// </returns>
        private bool IsAccpacDotNetVersionPropsLocatedInWebFolder()
        {
            return File.Exists(Path.Combine(_settings.DestinationWebFolder, Constants.Common.AccpacPropsFile));
        }

        /// <summary>
        /// Remove an existing AccpacDotNetVersion.props file from the
        /// solution folder if it exists.
        /// </summary>
        private void RemoveExistingPropsFileFromSolutionFolder()
        {
            var oldPropsFile = Path.Combine(_settings.DestinationSolutionFolder, Constants.Common.AccpacPropsFile);
            if (File.Exists(oldPropsFile)) { File.Delete(oldPropsFile); }
        }

        /// <summary>
        /// Copy the new AccpacDotNetVersion.props file to the Solution
        /// folder
        /// </summary>
        private void CopyNewPropsFileToSolutionFolder()
        {
            var file = Path.Combine(_settings.DestinationSolutionFolder, Constants.Common.AccpacPropsFile);
            var srcFilePath = Path.Combine(_settings.SourceFolder, Constants.Common.AccpacPropsFile);
            File.Copy(srcFilePath, file, true);
        }
        #endregion
    }
}
