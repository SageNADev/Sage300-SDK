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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.Utilities
{
    /// <summary>
    /// This is a class to help with the management of Sage300 MenuDetail.xml files
    /// </summary>
    public class MenuManager
    {
        #region Private Constants
        private static class Constants
        {
            public const string BaseMenuName = @"MenuDetails.xml";
            public const string WebProjectFileNameFilter = @"*.Web.csproj";
            public const string IconImageElementName = @"IconName";
            public const int ModuleIdLength = 2;


            // Note: Misspelling issue is known
            public const string MenuBackgroundImageElementName = @"MenuBackGoundImage";
        }
        #endregion

        #region Private Variables
        private readonly string _solutionFolder = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// The Module ID
        /// </summary>
        public string ModuleId { get { return this.MenuFilename.Substring(0, Constants.ModuleIdLength); } }

        /// <summary>
        /// The Menu filename
        /// </summary>
        public string MenuFilename { get; private set; }

        /// <summary>
        /// The Menu filename and path
        /// </summary>
        public string MenuFilePath { get; private set; }
        #endregion

        #region Constructor
        public MenuManager(string solutionFolder)
        {
            _solutionFolder = solutionFolder;

            // Sets some properties
            GetMenuFilePathAndName();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get the name of the menu file located in the Web project folder
        /// It is of the format XXMenuDetails.xml where XX is a two character module id
        /// </summary>
        private void GetMenuFileName(string backupFolder = @"")
        {
            var path = this.MenuFilePath;
            var di = new DirectoryInfo(path);
            var fi = new FileInfo(path);

            var filename = "TUMenuDetails.xml";

            //string fileTypeFilter = @"*" + Constants.BaseMenuName;
            //var filename = EnumerateFiles(new DirectoryInfo(this._solutionFolder),
            //                              fileTypeFilter,
            //                              ignoreDirectories: new List<string> { backupFolder }).SingleOrDefault();

            this.MenuFilename = filename;
        }

        /// <summary>
        /// Get the name of the menu file located in the Web project folder
        /// It is of the format XXMenuDetails.xml where XX is a two character module id
        /// </summary>
        private void GetMenuFilePathAndName(string backupFolder = @"")
        {
            string fileTypeFilter = @"*" + Constants.BaseMenuName;
            var filename = FileUtilities.EnumerateFiles(new DirectoryInfo(this._solutionFolder),
                                          fileTypeFilter,
                                          ignoreDirectories: new List<string> { backupFolder }).SingleOrDefault();
            FileInfo fi = new FileInfo(filename);
            this.MenuFilePath = fi.FullName;
            this.MenuFilename = fi.Name;
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

        /// <summary>
        /// Extract the company name from the name of the Web project (csproj)
        /// Note: Via the Solution Folder Name
        /// </summary>
        /// <returns>The extracted company name </returns>
        private string GetCompanyName()
        {
            string name = string.Empty;
            DirectoryInfo solution = new DirectoryInfo(this._solutionFolder);
            var projectList = solution.EnumerateFiles(Constants.WebProjectFileNameFilter, SearchOption.AllDirectories);
            foreach (var projFile in projectList)
            {
                // The company name is the first part of the string (if split on each '.')
                name = projFile.ToString().Split(new char[] { '.' })[0];
                break;
            }

            return name;
        }

        /// <summary>
        /// Inspect an XmlElement node to determine if it's
        /// a second level menu <item> element
        /// </summary>
        /// <param name="e">The XmlElement item to inspect</param>
        /// <returns>
        /// true = second level menu item node
        /// false = not a second level menu item node
        /// </returns>
        private bool IsSecondLevelMenuItem(XmlElement e)
        {
            int MenuLevelToLookFor = 2;
            if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
            {
                foreach (XmlNode node in e.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Element) continue;

                    var element = (XmlElement)node;
                    if (element.Name.ToLowerInvariant() == "menuitemlevel")
                    {
                        var menuItemLevel = element.InnerText;
                        if (!string.IsNullOrEmpty(menuItemLevel))
                        {
                            return Convert.ToInt32(menuItemLevel) == MenuLevelToLookFor;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Does this node contain an element called <IconName></IconName>
        /// </summary>
        /// <param name="e">The XML Element in question</param>
        /// <returns>true = IconName element found </returns>
        private bool HasMenuIconNameElement(XmlElement e) => ContainsElement(e, Constants.IconImageElementName);

        /// <summary>
        /// Does this node contain an element called <MenuBackGoundImage></MenuBackGoundImage>
        /// </summary>
        /// <param name="e">The XML Element in question</param>
        /// <returns>true = MenuBackGoundImage element found </returns>
        private bool HasMenuBackGroundImageElement(XmlElement e) => ContainsElement(e, Constants.MenuBackgroundImageElementName);

        /// <summary>
        /// Does this menu item node contain the designated element?
        /// </summary>
        /// <param name="e">The menu item node to search in</param>
        /// <param name="elementName">The name of the element</param>
        /// <returns></returns>
        private bool ContainsElement(XmlElement e, string elementName)
        {
            // This as already been done but better to be safe than sorry!
            if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
            {
                foreach (XmlElement n in e.ChildNodes)
                {
                    if (n.Name.Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the value of the element specified
        /// </summary>
        /// <param name="e">The menu item node to search in</param>
        /// <param name="elementName">The name of the element</param>
        /// <returns></returns>
        private string GetItemValue(XmlElement e, string elementName)
        {
            var itemValue = string.Empty;

            // This as already been done but better to be safe than sorry!
            if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
            {
                foreach (XmlElement n in e.ChildNodes)
                {
                    if (n.Name.Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        itemValue = n.InnerText;
                        break;
                    }
                }
            }
            return itemValue;
        }

        /// <summary>
        /// Set the value of a menu item node element
        /// </summary>
        /// <param name="elementName">The name of the element to set it's value</param>
        /// <param name="value">The value to set the element to</param>
        private void SetItemValue(string elementName, string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(this.MenuFilePath);

            // Now find the <Navigation> node.
            var navigationNode = FindNavigationNode(xmlDoc);
            var hasChanges = false;

            foreach (XmlNode node in navigationNode)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                var e = (XmlElement)node;
                if (!IsSecondLevelMenuItem(e)) continue;

                // Element exists so we can set it's value
                var element = GetElement(e, elementName);
                if (element != null)
                {
                    element.InnerText = value;
                    hasChanges = true;
                }
            }

            // Save the file if anything changed
            if (hasChanges)
            {
                xmlDoc.Save(this.MenuFilePath);
                //LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateMenuDetails} : {filePath}");
            }
        }

        /// <summary>
        /// Get the value from the specified menu item node element
        /// </summary>
        /// <param name="e">The menu item node to look in</param>
        /// <param name="elementName">The name of the element to query</param>
        /// <returns></returns>
        private XmlNode GetElement(XmlElement e, string elementName)
        {

            // This as already been done but better to be safe than sorry!
            if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
            {
                foreach (XmlElement n in e.ChildNodes)
                {
                    if (n.Name.Equals(elementName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return n;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the value of the <IconName></IconName> item
        /// </summary>
        /// <param name="e">The menu item node to look in</param>
        /// <returns>The path value from the element specified</returns>
        private string GetMenuIconImagePath(XmlElement e) => GetItemValue(e, Constants.IconImageElementName);

        /// <summary>
        /// Returns the value of the <MenuBackGoundImage></MenuBackGoundImage> item
        /// Note: The spelling mistake is known.
        /// </summary>
        /// <param name="e">The menu item node to look in</param>
        /// <returns>The path value from the element specified</returns>
        private string GetMenuBackgroundImagePath(XmlElement e) => GetItemValue(e, Constants.MenuBackgroundImageElementName);
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the value of the <IconName></IconName> item
        /// </summary>
        /// <returns>The path value from the element specified</returns>
        public string GetMenuIconImagePath()
        {
            var path = string.Empty;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(this.MenuFilePath);

            // Now find the <Navigation> node.
            var navigationNode = FindNavigationNode(xmlDoc);

            foreach (XmlNode node in navigationNode)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                var e = (XmlElement)node;
                if (!IsSecondLevelMenuItem(e)) continue;

                if (HasMenuIconNameElement(e))
                {
                    path = GetMenuIconImagePath(e);
                    break;
                }
            }

            return path;
        }

        /// <summary>
        /// Returns the value of the <MenuBackGoundImage></MenuBackGoundImage> item
        /// </summary>
        /// <returns>The path value from the element specified</returns>
        public string GetMenuBackgroundImagePath()
        {
            var path = string.Empty;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(this.MenuFilePath);

            // Now find the <Navigation> node.
            var navigationNode = FindNavigationNode(xmlDoc);

            foreach (XmlNode node in navigationNode)
            {
                if (node.NodeType != XmlNodeType.Element) continue;
                var e = (XmlElement)node;
                if (!IsSecondLevelMenuItem(e)) continue;

                if (HasMenuBackGroundImageElement(e))
                {
                    path = GetMenuBackgroundImagePath(e);
                    break;
                }
            }

            return path;
        }

        /// <summary>
        /// Set the value of the <MenuBackGoundImage></MenuBackGoundImage> element
        /// </summary>
        /// <param name="path">The path value to set it to</param>
        public void SetMenuBackgroundImage(string path) => SetItemValue(Constants.MenuBackgroundImageElementName, path);

        /// <summary>
        /// Set the value of the <IconName></IconName> element
        /// </summary>
        /// <param name="path">The path value to set it to</param>
        public void SetMenuIconImage(string path) => SetItemValue(Constants.IconImageElementName, path);
        #endregion
    }
}
