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
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Extensions;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Interfaces;
using System;
using System.Linq;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.PerRelease
{
    /// <summary>
    /// This is a class to manage the following:
    /// * Find all files within the Web project folder
    /// * Replace the version number for the Crystal reports references
    /// 
    /// Note: This class is specific to the 2019.0 release
    /// </summary>
    public class CrystalReportsVersionNumberProcessor
    {
        #region Public Constants
        public static class Constants
        {
            public const string PreviousVersionNumber = "13.0.2000.0";
            public const string NewVersionNumber = "13.0.3500.0";
        }
        #endregion

        #region Private Variables
        /// <summary> Settings from UI </summary>
        private ISettings _settings;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">An instance of the Settings object</param>
        public CrystalReportsVersionNumberProcessor(ISettings settings)
        {
            _settings = (Settings)settings;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This is the main processor for this class
        /// </summary>
        public void Process()
        {
            // Step 1 - Build a list of all files that contain the old Crystal Reports version number
            //          that we wish to update
            var startingDirectory = _settings.DestinationWebFolder;

            var allFiles = startingDirectory.GetFileNames();
            var files = allFiles.FindFilesContaining(line => line.IndexOf(Constants.PreviousVersionNumber,
                                                                StringComparison.CurrentCultureIgnoreCase) >= 0)
                                .AsParallel()
                                .ToList();

            // Step 2 - Do the replacement operation on each file in the list
            files.ReplaceTextInFiles(Constants.PreviousVersionNumber, Constants.NewVersionNumber);
        }
        #endregion
    }
}
