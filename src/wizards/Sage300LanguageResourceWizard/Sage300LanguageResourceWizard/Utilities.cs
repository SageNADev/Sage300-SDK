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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
//using static Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.ProcessUpgrade;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard
{
    public static class Utilities
    {
        //#region Public Delegates
        ///// <summary> Delegate to update UI with name of the step being processed </summary>
        ///// <param name="text">Text for UI</param>
        //private delegate void ProcessingEventHandler(string text);

        ///// <summary> Delegate to update log with status of the step being processed </summary>
        ///// <param name="text">Text for UI</param>
        //private delegate void LogEventHandler(string text);
        //#endregion

        #region Private Variables
        private static Settings _settings;
        #endregion

        /// <summary> Event to update UI with name of the step being processed </summary>
        private static event Delegates.ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update log with status of the step being processed </summary>
        private static event Delegates.LogEventHandler LogEvent;

        #region Public Methods
        public static void InitSettings(Settings settings)
        {
            _settings = settings;
        }
        #endregion

        /// <summary>
        /// Inspect an XmlElement node to determine if it's
        /// a PropertyGroup without any attributes
        /// </summary>
        /// <param name="e">The XmlElement item to inspect</param>
        /// <returns>
        /// true = PropertyGroup node without any attributes
        /// false = PropertyGroup node with attributes
        /// </returns>
        public static bool IsPropertyGroupWithoutAttributes(XmlElement e)
        {
            return e.Name == "PropertyGroup" && e.HasAttributes == false;
        }

        /// <summary>
        /// Get a reference to the <Navigation> node in the XXMenuDetail.xml file
        /// </summary>
        /// <param name="doc">A reference to the XmlDocument</param>
        /// <returns>A reference to the Navigation node</returns>
        public static XmlNode FindNavigationNode(XmlDocument doc)
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
        /// </summary>
        /// <param name="solution">A DirectoryInfo object holding the visual studio solution information</param>
        /// <param name="moduleId">The two letter module designation</param>
        /// <returns>The extracted company name </returns>
        public static string GetCompanyName(DirectoryInfo solution, string moduleId)
        {
            string name = string.Empty;
            var projectList = solution.EnumerateFiles($"*.Web.csproj", SearchOption.AllDirectories);
            foreach (var projFile in projectList)
            {
                // The company name is the first part of the string (if split on each '.')
                name = projFile.ToString().Split(new char[] { '.' })[0];
                break;
            }

            return name;
        }

        /// <summary>
        /// Delete a folder, if it exists
        /// </summary>
        /// <param name="folder">The fully-qualified path to the folder</param>
        public static void DeleteFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        /// <summary> Copy folder and files </summary>
        /// <param name="sourceDirectoryName">Source directory name</param>
        /// <param name="destinationDirectoryName">Destination directory name</param>
        public static void DirectoryCopy(string sourceDirectoryName, string destinationDirectoryName, bool ignoreDestinationFolder = true)
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
                try
                {
                    var filePath = Path.Combine(destinationDirectoryName, file.Name);
                    file.CopyTo(filePath, true);

                    // Log detail
                    LaunchLogEvent($"{DateTime.Now} {Resources.AddReplaceFile} {filePath}");
                }
                catch (IOException e)
                {
                    // Likely just a locked file.
                    // Just log it and move on.
                    LaunchLogEvent($"{Resources.ExceptionThrownPossibleLockedFile} : {file.FullName.ToString()}");
                    LaunchLogEvent($"{e.Message}");
                }
            }

            // For recursion
            foreach (DirectoryInfo subdir in dirs)
            {
                var subdirectoryName = subdir.FullName;
                if (ignoreDestinationFolder)
                {
                    if (subdirectoryName != destinationDirectoryName)
                    {
                        DirectoryCopy(subdirectoryName, Path.Combine(destinationDirectoryName, subdir.Name));
                    }
                }
                else
                {
                    DirectoryCopy(subdirectoryName, Path.Combine(destinationDirectoryName, subdir.Name));
                }
            }
        }

        /// <summary>
        /// Build a list of filepaths based on a fileTypeFilter and an optional list of directories to ignore.
        /// This method is a wrapper for DirectoryInfo.EnumerateFiles()
        /// </summary>
        /// <param name="startingDirectory">Where shall this file search start?</param>
        /// <param name="fileTypeFilter">What types of files shall we look for?</param>
        /// <param name="ignoreDirectories">This is a list directories that we wish to ignore.</param>
        /// <returns>A list of files matching the fileTypeFilter with optionally removed directories</returns>
        public static IEnumerable<string> EnumerateFiles(DirectoryInfo startingDirectory,
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
        /// Create a new folder for the backup
        /// </summary>
        /// <param name="currentFolder">This is the folder in which we wish to create the backup folder</param>
        /// <returns>The string representing the fully-qualified path to the backup folder</returns>
        public static string CreateBackupFolder(string currentFolder)
        {
            string BackupFolderName = CreateBackupFolderName();
            var backupFolder = Path.Combine(currentFolder, BackupFolderName);
            if (!Directory.Exists(backupFolder))
            {
                new DirectoryInfo(backupFolder).Create();
            }
            return backupFolder;
        }

        /// <summary>
        /// Create a name for the backup folder based on the current date and time
        /// </summary>
        /// <returns>A string representing the name of the backup folder</returns>
        public static string CreateBackupFolderName()
        {
            var dateStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            return $"Backup-{dateStamp}";
        }

        /// <summary>
        /// Backup the solution
        /// Note: Not currently used.
        /// </summary>
        public static string BackupSolution()
        {
            LaunchLogEventStart($"Backing up solution...");
            LaunchProcessingEvent($"Backing up solution...");

            // Create a backup folder if it doesn't already exist.
            var solutionFolder = _settings.DestinationSolutionFolder;
            var backupFolder = CreateBackupFolder(solutionFolder);

            // Do the backup (ensuring that we don't backup the backup folder 
            // because it lives within the solution folder itself.
            DirectoryCopy(solutionFolder, backupFolder, ignoreDestinationFolder: true);

            LaunchLogEventEnd($"Backup complete.");
            LaunchLogEvent("");

            return backupFolder;
        }


        /// <summary> Update UI </summary>
        /// <param name="text">Step name</param>
        public static void LaunchProcessingEvent(string text) => ProcessingEvent?.Invoke(text);

        /// <summary> Update Log </summary>
        /// <param name="text">Text to log</param>
        public static void LaunchLogEvent(string text) => LogEvent?.Invoke(text);

        /// <summary> Update Log - Event Start</summary>
        /// <param name="text">Text to log</param>
        public static void LaunchLogEventStart(string text)
        {
            var s = $"{DateTime.Now} -- {Resources.Start} {text} --";
            LogEvent?.Invoke(s);
        }

        /// <summary> Update Log - Event End</summary>
        /// <param name="text">Text to log</param>
        public static void LaunchLogEventEnd(string text)
        {
            var s = $"{DateTime.Now} -- {Resources.End} {text} --";
            LogEvent?.Invoke(s);
        }
    }
}
