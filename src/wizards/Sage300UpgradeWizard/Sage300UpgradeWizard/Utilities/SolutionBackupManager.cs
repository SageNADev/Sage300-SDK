// The MIT License (MIT) 
// Copyright (c) 1994-2020 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities
{
    public static class SolutionBackupManager
    {
        /// <summary>
        /// Backup the solution
        /// Note: Not currently used.
        /// </summary>
        /// <param name="solutionFolder">A string representing the solution folder</param>
        /// <returns>The string representing the fully-qualified path to the backup folder</returns>
        public static string BackupSolution(string solutionFolder)
        {
            //LogEventStart($"Backing up solution...");
            //LaunchProcessingEvent($"Backing up solution...");

            // Create a backup folder if it doesn't already exist.
            //var solutionFolder = _settings.DestinationSolutionFolder;
            var backupFolder = CreateBackupFolder(solutionFolder);

            // Do the backup (ensuring that we don't backup the backup folder 
            // because it lives within the solution folder itself.
            FileUtilities.DirectoryCopy(solutionFolder, backupFolder, ignoreDestinationFolder: true);

            // Now, move the backup folder OUTSIDE of the solution folder
            var destinationFolder = FileUtilities.MoveDirectoryUpOneLevel(solutionFolder, backupFolder);

            //LogEventEnd($"Backup complete.");
            //Log("");

            return destinationFolder;
        }

        /// <summary>
        /// Create a new folder for the backup
        /// </summary>
        /// <param name="currentFolder">This is the folder in which we wish to create the backup folder</param>
        /// <returns>The string representing the fully-qualified path to the backup folder</returns>
        private static string CreateBackupFolder(string currentFolder)
        {
            string BackupFolderName = CreateBackupFolderName(currentFolder);
            var backupFolder = Path.Combine(currentFolder, BackupFolderName);
            if (!Directory.Exists(backupFolder))
            {
                new DirectoryInfo(backupFolder).Create();
            }
            return backupFolder;
        }

        /// <summary>
        /// Create a name for the backup folder based on the current solution folder name and the current date & time
        /// 
        /// Note: The output is only the backup folder name, not the full path to it.
        /// 
        /// Example input  : "C:\projects\Sage300-SDK\2020.2 (Read Only)\samples\SegmentCodes"
        /// Example output : "SegmentCodes-Backup-20200513-111532"
        /// </summary>
        /// <returns>A string representing the name of the backup folder</returns>
        private static string CreateBackupFolderName(string currentFolder)
        {
            var parts = currentFolder.Split('\\');
            var solutionName = parts[parts.Length - 1].Trim();
            var dateStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            return $"{solutionName}-Backup-{dateStamp}";
        }
    }
}
