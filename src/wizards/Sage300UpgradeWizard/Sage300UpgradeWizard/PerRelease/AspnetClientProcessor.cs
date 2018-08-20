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
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Interfaces;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities;
using System.IO;
using System.Linq;
using System.Text;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.PerRelease
{
    /// <summary>
    /// This is a class to manage the following:
    /// * Create a folder called 'ExternalContent' in the Web project\Areas\{ModuleId}\ folder
    /// * Update the {ModuleId}MenuDetails.xml file with new path and filenames for
    ///   both the menu background image and menu icon image
    /// * Move the two images from their original location to the new 'ExternalContent' folder
    /// * Add this new folder (and content) to the Web.csproj file
    /// 
    /// Note: This class is specific to the 2019.0 release
    /// </summary>
    public class AspnetClientProcessor
    {
        #region Private Constants
        private static class Constants
        {
            public const string AspnetClientFolderName = @"aspnet_client";

            //public const string CSharpProjectExtensionName = @"csproj";
            //public const string AreasFolderName = @"Areas";
            //public const string ExternalContentFolderName = @"ExternalContent";
            //public const string RelativePathDesignator = @"../../../..";
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
        public AspnetClientProcessor(ISettings settings)
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
            // Step 1 - Check for the existence of the 'Aspnet_Client' folder in the Web project
            var webFolder = _settings.DestinationWebFolder;
            var aspnetClientFolder = Path.Combine(webFolder, Constants.AspnetClientFolderName);
            if (Directory.Exists(aspnetClientFolder) == false)
            {

            }
            else
            {
                // The aspnet_client folder already exists, nothing for us to do.
            }
        }
        #endregion 
    }
}
