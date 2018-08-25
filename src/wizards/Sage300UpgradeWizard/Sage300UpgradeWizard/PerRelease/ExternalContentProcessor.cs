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
using System;
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
    public class ExternalContentProcessor
    {
        #region Private Constants
        private static class Constants
        {
            public const string CSharpProjectExtensionName = @"csproj";
            public const string AreasFolderName = @"Areas";
            public const string ExternalContentFolderName = @"ExternalContent";
            public const string RelativePathDesignator = @"../../../..";
            public const string Pre2019Dot0ImageLocation = @"\Content\Images\nav\";

            public const string Pre2019Dot0BackgroundImageName = @"menuBackGroundImage.jpg";
            public const string Post2019Dot0BackgroundImageNameTemplate = @"bg_menu_{0}.jpg";

            public const string Pre2019Doc0IconImageName = @"menuIcon.png";
            public const string Post2019Dot0IconImageNameTemplate = @"icon_{0}.png";
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
        public ExternalContentProcessor(ISettings settings)
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
            #region Step 1 - Create ExternalContent folder under Areas\{Module} and add to web project
            var webFolder = _settings.DestinationWebFolder;
            var solutionFolder = _settings.DestinationSolutionFolder;

            // Get the ModuleID specifier from the webFolder path
            var moduleId = ExtractModuleIdFromPath(webFolder);
            var areasFolder = Path.Combine(webFolder, Constants.AreasFolderName);
            var areasModuleFolder = Path.Combine(areasFolder, moduleId);
            var externalContentFolder = Path.Combine(areasModuleFolder, Constants.ExternalContentFolderName);
            var folderAlreadyExisted = FileUtilities.CreateFolderIfNotExists(externalContentFolder);
            #endregion

            #region Step 2 - Get file names for menuIcon and menuBackGroundImage from {module}MenuDetails.xml (We will search for these later)
            var menuManager = new MenuManager(_settings.DestinationSolutionFolder);

            // Get the two filenames and their paths from the {module}MenuDetails.xml file
            var backgroundImagePathFromMenu = menuManager.GetMenuBackgroundImagePath();
            var iconImagePathFromMenu = menuManager.GetMenuIconImagePath();
            var backgroundImageFilenameFromMenu = new FileInfo(backgroundImagePathFromMenu).Name;
            var iconImageFilenameFromMenu = new FileInfo(iconImagePathFromMenu).Name;
            #endregion

            #region Step 3 - Setup the new names for the menu background and icon images
            string newBackgroundImageFilename = string.Format(Constants.Post2019Dot0BackgroundImageNameTemplate, moduleId.ToLower());
            string newIconImageFilename = string.Format(Constants.Post2019Dot0IconImageNameTemplate, moduleId.ToLower());

            // Determine if the filenames contained in the Menu XML file are the old format 
            // or the new 2019.0 introduced format
            var compareType = StringComparison.InvariantCultureIgnoreCase;
            var sameBackgroundImageFilename = backgroundImageFilenameFromMenu.Equals(newBackgroundImageFilename, compareType);
            var sameIconImageFilename = iconImageFilenameFromMenu.Equals(newIconImageFilename, compareType);
            #endregion

            #region Step 4 - Update the XXMenuDetails.xml file with the new values, if necessary
            var moduleIdUpper = moduleId.ToUpper();
            var folder = Constants.ExternalContentFolderName;
            var areas = Constants.AreasFolderName;
            var relPath = Constants.RelativePathDesignator;

            if (sameBackgroundImageFilename == false)
            {
                // The background image file referenced in the menu is the pre 2019.0 image filename 
                string newBackgroundImageFilePathForMenu = $"{relPath}/{areas}/{moduleIdUpper}/{folder}/{newBackgroundImageFilename}";
                menuManager.SetMenuBackgroundImage(newBackgroundImageFilePathForMenu);

                string backgroundImageLocation = FileUtilities.EnumerateFiles(solutionFolder, backgroundImageFilenameFromMenu).ToArray()[0];
                var targetPath = Path.Combine(externalContentFolder, newBackgroundImageFilename);
                File.Move(backgroundImageLocation, targetPath);
            }
            else
            {
                // Menu background image referenced in the menu is the new one

                // Get the full path to the old background image (name and location)
                var oldImagePath = Path.Combine(webFolder,
                                        Constants.Pre2019Dot0ImageLocation,
                                        Constants.Pre2019Dot0BackgroundImageName);
                if (File.Exists(oldImagePath))
                {
                    // The old file exists in the original location. 
                    // Let's copy it over to the new location with the new name.
                    var targetPath = Path.Combine(externalContentFolder, newBackgroundImageFilename);
                    File.Move(oldImagePath, targetPath);
                }
            }

            if (sameIconImageFilename == false)
            {
                // The icon image file referenced in the menu is the pre 2019.0 icon image filename 
                string newMenuIconImageFilePathForMenu = $"{relPath}/{areas}/{moduleIdUpper}/{folder}/{newIconImageFilename}";
                menuManager.SetMenuIconImage(newMenuIconImageFilePathForMenu);

                string iconImageLocation = FileUtilities.EnumerateFiles(solutionFolder, iconImageFilenameFromMenu).ToArray()[0];
                var targetPath = Path.Combine(externalContentFolder, newIconImageFilename);
                File.Move(iconImageLocation, targetPath);
            }
            else
            {
                // Menu background image referenced in the menu is the new one

                // Get the full path to the old background image (name and location)
                var oldImagePath = Path.Combine(webFolder,
                                        Constants.Pre2019Dot0ImageLocation,
                                        Constants.Pre2019Doc0IconImageName);
                if (File.Exists(oldImagePath))
                {
                    // The old file exists in the original location. 
                    // Let's copy it over to the new location with the new name.
                    var targetPath = Path.Combine(externalContentFolder, newIconImageFilename);
                    File.Move(oldImagePath, targetPath);
                }
            }

            #endregion

            #region Step 5 - Update the Web project with the new folder and files
            // Now that the new 'ExternalContent' folder has been created and the 
            // two images have been renamed and moved there, let's add all of 
            // these to the Web project.

            // Open the Web.csproj file and insert the following:
            //
            // <ItemGroup>
            //   <Content Include="Areas\VM\ExternalContent\**" />
            // </ItemGroup>
            //

            // Get the company name from the Web folder
            var webFolderParts = webFolder.Split(new char[] { Path.DirectorySeparatorChar });
            var webFolderNameOnly = webFolderParts[webFolderParts.Length - 1];
            var webProjectName = $"{webFolderNameOnly}.{Constants.CSharpProjectExtensionName}";
            var webProjectFilePath = Path.Combine(webFolder, webProjectName);

            var allLines = File.ReadAllLines(webProjectFilePath);
            var txtLines = allLines.ToList();
            var trimLines = allLines.Select(l => l.Trim()).ToList();

            // Let's see if this line already exists. If it does, then we can skip the rest.
            var mainContentLine = $"<Content Include=\"Areas\\{moduleId.ToUpper()}\\{Constants.ExternalContentFolderName}\\**\" />";
            if (trimLines.Contains(mainContentLine) == false)
            {
                // Build the content to insert into the project file
                var sb = new StringBuilder();
                sb.AppendLine($"  <ItemGroup>");
                sb.AppendLine($"    {mainContentLine}");
                sb.Append($"  </ItemGroup>");

                // Look for the last </ItemGroup>.
                // We will insert the block just after this.
                var lastIndex = trimLines.LastIndexOf(@"</ItemGroup>");
                if (lastIndex > -1)
                {
                    var insertionIndex = lastIndex + 1;
                    txtLines.Insert(insertionIndex, sb.ToString());
                    File.WriteAllLines(webProjectFilePath, txtLines);
                }
            }
            #endregion
        }

        /// <summary>
        /// Extract the ModuleId from a path
        /// </summary>
        /// <param name="path">this is the file or folder path</param>
        /// <returns></returns>
        public string ExtractModuleIdFromPath(string path)
        {
            var moduleId = string.Empty;

            if (path.Length > 0)
            {
                var parts = path.Split(new[] { '\\' });
                var lastPart = parts[parts.Length-1];
                var finalPart = lastPart.Split(new[] { '.' });
                if (finalPart.Length == 3)
                {
                    moduleId = finalPart[1];
                }
            }
            return moduleId;
        }
        #endregion
    }
}
