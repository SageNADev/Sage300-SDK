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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities
{
    public class MenuManager
    {
        private static class Constants
        {
            public const string BaseMenuName = @"MenuDetails.xml";
        }

        private ISettings _settings = null;

        public MenuManager(ISettings s)
        {
            _settings = s;
        }

        /// <summary>
        /// Build a list of filepaths based on a fileTypeFilter and an optional list of directories to ignore.
        /// This method is a wrapper for DirectoryInfo.EnumerateFiles()
        /// </summary>
        /// <param name="startingDirectory">Where shall this file search start?</param>
        /// <param name="fileTypeFilter">What types of files shall we look for?</param>
        /// <param name="ignoreDirectories">This is a list directories that we wish to ignore.</param>
        /// <returns>A list of files matching the fileTypeFilter with optionally removed directories</returns>
        private IEnumerable<string> EnumerateFiles(DirectoryInfo startingDirectory,
                                                   string fileTypeFilter,
                                                   List<string> ignoreDirectories)
        {
            var results = startingDirectory.EnumerateFiles(fileTypeFilter, SearchOption.AllDirectories)
                                                   .ToList<FileInfo>()
                                                   .ConvertAll(x => (string)x.FullName);
            results.RemoveAll(f => ignoreDirectories.Exists(i => !String.IsNullOrWhiteSpace(i) && f.Contains(i)));
            return results;
        }

        /// <summary>
        /// Get the name of the menu file located in the Web project folder
        /// It is of the format XXMenuDetails.xml where XX is a two character module id
        /// </summary>
        /// <returns>A string representing the name of the menu file</returns>
        public string GetMenuFileName(string backupFolder = @"")
        {
            string fileTypeFilter = @"*" + Constants.BaseMenuName;
            var filename = EnumerateFiles(new DirectoryInfo(_settings.DestinationSolutionFolder),
                                          fileTypeFilter,
                                          ignoreDirectories: new List<string> { backupFolder }).SingleOrDefault();
            return new FileInfo(filename).Name;
        }

        /// <summary>
        /// Craft up a menu filename based on the project name.
        /// </summary>
        /// <param name="fileInfo">A FileInfo object with information on the file.</param>
        /// <returns>
        /// Return the name of a menufile in the following format [XX]MenuFile.xml
        /// where [XX] is the ModuleId
        /// </returns>
        private string GetMenuFileNameFromProjectName(FileInfo fileInfo)
        {
            var parts = fileInfo.Name.Split(new string[] { "." }, StringSplitOptions.None);
            var moduleId = parts[1];
            return GetMenuFileNameFromModuleId(moduleId);
        }

        /// <summary>
        /// Craft up a menu filename based on the project name.
        /// </summary>
        /// <param name="filePath">A string with the filename and path</param>
        /// <returns>
        /// Return the name of a menufile in the following format [XX]MenuFile.xml
        /// where [XX] is the ModuleId
        /// </returns>
        private string GetMenuFileNameFromProjectName(string filePath)
        {
            var parts = filePath.Split(new string[] { "." }, StringSplitOptions.None);
            var moduleId = parts[1];
            return GetMenuFileNameFromModuleId(moduleId);
        }

        /// <summary>
        /// Craft up a menu filename based on the project name.
        /// </summary>
        /// <param name="moduleId">This is the two letter module id</param>
        /// <returns></returns>
        private string GetMenuFileNameFromModuleId(string moduleId)
        {
            string menuFileTemplate = "{0}" + Constants.BaseMenuName;
            return String.Format(menuFileTemplate, moduleId);
        }

        /// <summary>
        /// Determine whether or not an XmlElement is an <IconName> element
        /// </summary>
        /// <param name="e">The XmlElement in question</param>
        /// <returns>
        /// true = XmlElement is an IconName
        /// false = XmlElement is not an IconName
        /// </returns>
        private static bool IsIconNameElement(XmlElement e) => e.Name.ToUpperInvariant() == "ICONNAME";

        /// <summary>
        /// Determine whether or not an XmlElement is an <MenuBackGoundImage> element
        /// Note: The element name is currently misspelled as 'MenuBackGoundImage' instead of 'MenuBackGroundImage'
        /// This is a known issue.
        /// </summary>
        /// <param name="e">The XmlElement in question</param>
        /// <returns>
        /// true = XmlElement is a MenuBackGoundImage
        /// false = XmlElement is not an MenuBackGoundImage
        /// </returns>
        private static bool IsMenuBackGroundImageElement(XmlElement e) => e.Name.ToUpperInvariant() == "MENUBACKGOUNDIMAGE";

        /// <summary>
        /// Get a reference to the <Navigation> node in the XXMenuDetail.xml file
        /// </summary>
        /// <param name="doc">A reference to the XmlDocument</param>
        /// <returns>A reference to the Navigation node</returns>
        private XmlNode FindNavigationNode(XmlDocument doc)
        {
            XmlNode returnNode = null;
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node.Name.ToLowerInvariant() == "navigation" && node.Attributes.Count == 0)
                {
                    returnNode = node;
                    break;
                }
            }
            return returnNode;
        }


        ///// <summary>
        ///// TODO
        ///// </summary>
        ///// <param name="menuFile"></param>
        ///// <param name="backgroundImage"></param>
        ///// <param name="menuImage"></param>
        //public void GetMenuItems(string menuFile, out string backgroundImage, out string menuImage)
        //{
        //}
    }
}
