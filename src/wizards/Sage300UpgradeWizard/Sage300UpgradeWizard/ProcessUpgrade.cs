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
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.PerRelease;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using System;
using System.IO;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> Process Upgrade Class (worker) </summary>
    internal class ProcessUpgrade
	{
	#region Private Variables
		/// <summary> Settings from UI </summary>
		private Settings _settings;
		private string _backupFolder = String.Empty;
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

			// Track whether or not the AccpacDotNetVersion.props file originally existed in the Web folder.
			// If it does/did, then we will just update it in place instead of relocating it to the Solution folder.
			bool AccpacPropsFileOriginallyInWebFolder = false;

            //Utilities.InitSettings(_settings);
            //var commonSteps = new CommonReleaseUpgradeSteps(_settings);
            //var customSteps = new CustomReleaseUpgradeSteps(_settings);

	#region Backup Solution - Currently Disabled
			//_backupFolder = BackupSolution();
	#endregion

			// Start at step 1 and ignore last two steps
			for (var index = 0; index < _settings.WizardSteps.Count; index++)
			{
				var title = _settings.WizardSteps[index].Title;
				LaunchProcessingEvent(title);

				// Step 0 is Main and Last two steps are Upgrade and Upgraded
				switch (index)
				{
                    #region Common Upgrade Steps
                    case 1:
                        //commonSteps.SyncWebFiles(title, out AccpacPropsFileOriginallyInWebFolder);
                        SyncWebFiles(title, out AccpacPropsFileOriginallyInWebFolder);
                        break;

					case 2:
                        //commonSteps.SyncAccpacLibraries(title, AccpacPropsFileOriginallyInWebFolder);
                        SyncAccpacLibraries(title, AccpacPropsFileOriginallyInWebFolder);
                        break;

                    #endregion

                    #region Release Specific Upgrade Steps

#if ENABLE_TK_244885
                    case 3:
                        ConsolidateEnumerations(title);
                        break;
#endif
                    case 3:
                        ProcessExternalContentUpdates(title);
                        break;

                    case 4:
                        ProcessAspnetClientFolder(title);
                        break;

                    #endregion
                }
            }
		}

        #endregion

        #region Private methods

        /// <summary> Synchronization of web project files </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncWebFiles(string title, out bool accpacPropsInWebFolder)
        {
            // Log start of step
            LaunchLogEventStart(title);

            // Check to see if the AccpacDotNetVersion.props file
            // already exists in the Web folder.
            // If it does, then just update it and do not relocate it to the Project folder
            accpacPropsInWebFolder = IsAccpacDotNetVersionPropsLocatedInWebFolder();

            // Do the work :)
            DirectoryCopy(_settings.SourceFolder, _settings.DestinationWebFolder, ignoreDestinationFolder: false);

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
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

        /// <summary>
        /// Backup the solution
        /// Note: Not currently used.
        /// </summary>
        private string BackupSolution()
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

        /// <summary>
        /// Create a new folder for the backup
        /// </summary>
        /// <param name="currentFolder">This is the folder in which we wish to create the backup folder</param>
        /// <returns>The string representing the fully-qualified path to the backup folder</returns>
        private string CreateBackupFolder(string currentFolder)
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
        private string CreateBackupFolderName()
        {
            var dateStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            return $"Backup-{dateStamp}";
        }

        /// <summary> Upgrade project reference to use new verion Accpac.Net </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncAccpacLibraries(string title, bool accpacPropsInWebFolder)
        {
            // Log start of step
            LaunchLogEventStart(title);

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
            LaunchLogEvent($"{DateTime.Now} {txt}");

            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

        /// <summary>
        /// Is there a copy of the AccpacDotNetversion.props file in the Web project folder?
        /// </summary>
        /// <returns>
        /// true : AccpacDotNetVersion.props is in Web project folder 
        /// false: AccpacDotNetVersion.props is in not in the Web project folder 
        /// </returns>
        private bool IsAccpacDotNetVersionPropsLocatedInWebFolder()
        {
            return File.Exists(Path.Combine(_settings.DestinationWebFolder, @"AccpacDotNetVersion.props"));
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

        /// <summary>
        /// Process the 'ExternalContent' changes 
        /// </summary>
        /// <param name="title">The title to display for this step</param>
        public void ProcessExternalContentUpdates(string title)
        {
            // Log start of step
            LaunchLogEventStart(title);

            var processor = new ExternalContentProcessor(_settings);
            processor.Process();

            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

        /// <summary>
        /// Process the 'aspnet_client' folder changes
        /// </summary>
        /// <param name="title">The title to display for this step</param>
        public void ProcessAspnetClientFolder(string title)
        {
            // Log start of step
            LaunchLogEventStart(title);

            // Nothing to do. This is a manual partner step :)

            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

#if ENABLE_TK_244885
        /// <summary>
        /// TODO - Will implement post 2019.0 release
        /// </summary>
        /// <param name="title"></param>
        public void ConsolidateEnumerations(string title)
        {
            // Log start of step
            LaunchLogEventStart(title);

            //// Only do this if the AccpacDotNetVersion.props file was not originally in the Web folder.
            //if (!accpacPropsInWebFolder)
            //{
            //    // Do the actual work :)
            //    RemoveExistingPropsFileFromSolutionFolder();
            //    CopyNewPropsFileToSolutionFolder();
            //}

            //// Log detail
            //var txt = string.Format(Resources.UpgradeLibrary,
            //                        Constants.PerRelease.FromAccpacNumber,
            //                        Constants.PerRelease.ToAccpacNumber);
            //Utilities.LaunchLogEvent($"{DateTime.Now} {txt}");

            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }
#endif

        /// <summary> Copy folder and files </summary>
        /// <param name="sourceDirectoryName">Source directory name</param>
        /// <param name="destinationDirectoryName">Destination directory name</param>
        private void DirectoryCopy(string sourceDirectoryName, string destinationDirectoryName, bool ignoreDestinationFolder = true)
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

        /// <summary> Update UI </summary>
        /// <param name="text">Step name</param>
        private void LaunchProcessingEvent(string text) => ProcessingEvent?.Invoke(text);

        /// <summary> Update Log </summary>
        /// <param name="text">Text to log</param>
        private void LaunchLogEvent(string text) => LogEvent?.Invoke(text);

        /// <summary> Update Log - Event Start</summary>
        /// <param name="text">Text to log</param>
        private void LaunchLogEventStart(string text)
        {
            var s = $"{DateTime.Now} -- {Resources.Start} {text} --";
            LogEvent?.Invoke(s);
        }

        /// <summary> Update Log - Event End</summary>
        /// <param name="text">Text to log</param>
        private void LaunchLogEventEnd(string text)
        {
            var s = $"{DateTime.Now} -- {Resources.End} {text} --";
            LogEvent?.Invoke(s);
        }
        #endregion
    }
}
