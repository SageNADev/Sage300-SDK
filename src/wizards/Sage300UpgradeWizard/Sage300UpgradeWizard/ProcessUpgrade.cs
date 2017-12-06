// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

using System;
using System.IO;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using System.IO.Compression;
using System.Xml;

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> Process Upgrade Class (worker) </summary>
    internal class ProcessUpgrade
    {
        #region Private Vars
        /// <summary> Settings from UI </summary>
        private Settings _settings;
        #endregion

        #region Public constants
        /// <summary> From Release Number </summary>
        public const string FromReleaseNumber = "2017.2";

        /// <summary> To Release Number </summary>
        public const string ToReleaseNumber = "2018.0";

        /// <summary> From Accpac Number </summary>
        public const string FromAccpacNumber = "6.4.0.20";

        /// <summary> To Accpac Number </summary>
        public const string ToAccpacNumber = "6.5.0.0";

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

        #endregion

        #region Public Delegates
        /// <summary> Delegate to update UI with name of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update log with status of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void LogEventHandler(string text);
        #endregion

        #region Public Events
        /// <summary> Event to update UI with name of the step being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update log with status of the step being processed </summary>
        public event LogEventHandler LogEvent;
        #endregion

        #region Public Methods
        /// <summary> Start the generation process </summary>
        /// <param name="settings">Settings for processing</param>
        public void Process(Settings settings)
        {
            // Save settings for local usage
            _settings = settings;

            // Start at step 1 and ignore last two steps
            for (var index = 0; index < _settings.WizardSteps.Count; index++)
            {
                // Step 0 is Main and Last two steps are Upgrade and Upgraded
                switch (index)
                {
                    case 1:
                        LaunchProcessingEvent(_settings.WizardSteps[index].Title);
                        SyncWebFiles(_settings.WizardSteps[index].Title);
                        break;
                    case 2:
                        LaunchProcessingEvent(_settings.WizardSteps[index].Title);
                        SyncAccpacLibraries(_settings.WizardSteps[index].Title);
                        break;
                    case 5: //Enable XML property step - if step number changes, this case must change too
                        if (_settings.WizardSteps[index].CheckboxValue)
                        {
                            LaunchProcessingEvent(_settings.WizardSteps[index].Title);
                            EnableXmlProperty(_settings.WizardSteps[index].Title);
                        }
                        break;
                        // Case n for release specific steps here
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary> Synchronization of web project files </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncWebFiles(string title)
        {
            // Log start of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.Start, title));

            // Update the web files
            var zipFile = Path.Combine(_settings.SourceFolder, WebZipSuffix);
            var sourceWebFolder = Path.Combine(_settings.SourceFolder, WebFolderSuffix);

            // Delete folder if exists
            if (Directory.Exists(sourceWebFolder))
            {
                Directory.Delete(sourceWebFolder, true);
            }

            // Extract zip to folder
            ZipFile.ExtractToDirectory(zipFile, sourceWebFolder);

            // Copy diectory
            DirectoryCopy(sourceWebFolder, _settings.DestinationWebFolder);

            // Log end of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.End, title));
            LaunchLogEvent("");
        }

        /// <summary> Upgrade project reference to use new verion Accpac.Net </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncAccpacLibraries(string title)
        {
            // Log start of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.Start, title));

            // Copy new AccpacDotNetVersion.props
            var file = Path.Combine(_settings.DestinationWebFolder, AccpacPropsFile);
            var srcFilePath = Path.Combine(_settings.SourceFolder, AccpacPropsFile);
            File.Copy(srcFilePath, file, true);
            
            // Log detail
            LaunchLogEvent(string.Format("{0} {1}", DateTime.Now, 
                string.Format(Resources.UpgradeLibrary, FromAccpacNumber, ToAccpacNumber)));

            // Log end of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.End, title));
            LaunchLogEvent("");
        }

        /// <summary> Enable XML Documentation file property of projects </summary>
        /// <param name="title">Title of step being processed</param>
        private void EnableXmlProperty(string title)
        {
            // Log start of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.Start, title));

            // turn on project generate XML documentation file
            var slnDir = Directory.GetParent(_settings.DestinationWebFolder);
            var csprojFiles = slnDir.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
            foreach (var projFile in csprojFiles)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(projFile.FullName);
                var projName = Path.GetFileNameWithoutExtension(projFile.FullName) + ".XML";
                var nodes = xmlDoc.ChildNodes[1].ChildNodes;
                var xmlDocFileName = "";
                var hasChanges = false;

                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var e = (XmlElement)node;
                        if (e.Name == "PropertyGroup" && e.HasAttributes && e.Attributes[0].Name == "Condition")
                        {
                            var hasXmlDocFileName = false;
                            foreach (XmlElement n in e.ChildNodes)
                            {
                                if (n.Name == "OutputPath" && !string.IsNullOrEmpty(n.InnerText))
                                {
                                    if (n.InnerText.StartsWith("$"))
                                    {
                                        n.InnerText = @"bin\";
                                    }
                                    xmlDocFileName = Path.Combine(n.InnerText, projName);
                                }
                                if (n.Name == "DocumentationFile" && n.InnerText != "")
                                {
                                    hasXmlDocFileName = true;
                                }
                            }
                            if (!hasXmlDocFileName && !string.IsNullOrEmpty(xmlDocFileName))
                            {
                                e.InnerXml += string.Format("<DocumentationFile>{0}</DocumentationFile>", xmlDocFileName);
                                hasChanges = true;
                            }
                        }
                    }
                }

                if (hasChanges)
                {
                    xmlDoc.Save(projFile.FullName);
                    LaunchLogEvent(string.Format("{0} {1} : {2}", DateTime.Now, Resources.ReleaseSpecificTitleEnableXmlProperty, projFile.FullName));
                }
            }
            // Log end of step
            LaunchLogEvent(string.Format("{0} -- {1} {2} --", DateTime.Now, Resources.End, title));
            LaunchLogEvent("");
        }

        /// <summary> Copy folder and files </summary>
        /// <param name="sourceDirectoryName">Source directory name</param>
        /// <param name="destinationDirectoryName">Destination directory name</param>
        private void DirectoryCopy(string sourceDirectoryName, string destinationDirectoryName)
        {
            var dir = new DirectoryInfo(sourceDirectoryName);
            var dirs = dir.GetDirectories();

            // Create directory if not exists
            if (!Directory.Exists(destinationDirectoryName))
            {
                Directory.CreateDirectory(destinationDirectoryName);
            }

            // Iterate files
            foreach (var file in dir.GetFiles())
            {
                var filePath = Path.Combine(destinationDirectoryName, file.Name);
                file.CopyTo(filePath, true);

                // Log detail
                LaunchLogEvent(string.Format("{0} {1} {2}", DateTime.Now, Resources.AddReplaceFile, filePath));
            }

            // For recursion
            foreach (var subdir in dirs)
            {
                DirectoryCopy(subdir.FullName, Path.Combine(destinationDirectoryName, subdir.Name));
            }
        }

        /// <summary> Update UI </summary>
        /// <param name="text">Step name</param>
        private void LaunchProcessingEvent(string text)
        {
            // Event if subscriber
            if (ProcessingEvent == null)
            {
                return;
            }

            ProcessingEvent(text);
        }

        /// <summary> Update Log </summary>
        /// <param name="text">Log Entry</param>
        private void LaunchLogEvent(string text)
        {
            // Event if subscriber
            if (LogEvent == null)
            {
                return;
            }

            LogEvent(text);
        }

        #endregion

    }
}
