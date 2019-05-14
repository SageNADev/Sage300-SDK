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

using EnvDTE;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities;
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
            LogSpacerLine('-');
            Log(Resources.BeginUpgradeProcess);
            LogSpacerLine();

            // Save settings for local usage
            _settings = settings;

			// Track whether or not the AccpacDotNetVersion.props file originally existed in the Solution folder
            bool AccpacPropsFileOriginallyInSolutionfolder = false;

            //Utilities.InitSettings(_settings);
            //var commonSteps = new CommonReleaseUpgradeSteps(_settings);
            //var customSteps = new CustomReleaseUpgradeSteps(_settings);

            #region Backup Solution - Currently Disabled
            //_backupFolder = BackupSolution();
            #endregion

            // Does the AccpacDotNetVersion.props file exist in the Solution folder?
            AccpacPropsFileOriginallyInSolutionfolder = IsAccpacDotNetVersionPropsLocatedInSolutionFolder();

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
                        SyncKendoFiles(title);
                        break;

                    case 2:
                        SyncWebFiles(title);
                        break;

                    // Not necessary for 2019.2 release
					//case 3:
                    //    SyncAccpacLibraries(title, AccpacPropsFileOriginallyInSolutionfolder);
                    //    break;

                    #endregion

                    #region Release Specific Upgrade Steps

#if ENABLE_TK_244885
                    case 3:
                        ConsolidateEnumerations(title);
                        break;
#endif
                    case 3:
                        UpdateTargetedDotNetFrameworkVersion(title);
                        break;

                    #endregion
                }
            }

            LogSpacerLine();
            Log(Resources.EndUpgradeProcess);
            LogSpacerLine('-');
        }

        #endregion

        #region Private methods

        /// <summary> Synchronization of Kendo files </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncKendoFiles(string title)
        {
            // Log start of step
            LogEventStart(title);

            // Prepare Kendo source paths
            var webFolder = RegistryHelper.Sage300CWebFolder;
            var kendoFolderSource = Path.Combine(webFolder, "Scripts", "Kendo");
            var kendoFileSource = Path.Combine(kendoFolderSource, "kendo.all.min.js");

            // ... and destination paths
            var kendoFolderDest = Path.Combine(_settings.DestinationWebFolder, "Scripts", "Kendo");
            var kendoFileDest = Path.Combine(_settings.DestinationWebFolder, kendoFolderDest, "kendo.all.min.js");

            // Copy files
            File.Copy(kendoFileSource, kendoFileDest, true);
            Log($"{Resources.CopiedKendoFileFrom} '{kendoFolderSource}' {Resources.To} '{kendoFolderDest}'.");

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary> Synchronization of web project files </summary>
        /// <param name="title">Title of step being processed </param>
        private void SyncWebFiles(string title)
        {
            // Log start of step
            LogEventStart(title);

            // Do the work :)
            DirectoryCopy(_settings.SourceFolder, _settings.DestinationWebFolder, ignoreDestinationFolder: false);
            Log($"{Resources.CopiedAllFilesFrom} '{_settings.SourceFolder}' {Resources.To} '{_settings.DestinationWebFolder}'.");

            // Remove the files that are not actually part of the 'Web' bundle.
            // This is done because of the way VS2017 doesn't seem to allow embedding of zip
            // files within another zip file.
            File.Delete(Path.Combine(_settings.DestinationWebFolder, @"__TemplateIcon.ico"));
            File.Delete(Path.Combine(_settings.DestinationWebFolder, @"Items.vstemplate"));

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Backup the solution
        /// Note: Not currently used.
        /// </summary>
        private string BackupSolution()
        {
            LogEventStart($"Backing up solution...");
            LaunchProcessingEvent($"Backing up solution...");

            // Create a backup folder if it doesn't already exist.
            var solutionFolder = _settings.DestinationSolutionFolder;
            var backupFolder = CreateBackupFolder(solutionFolder);

            // Do the backup (ensuring that we don't backup the backup folder 
            // because it lives within the solution folder itself.
            DirectoryCopy(solutionFolder, backupFolder, ignoreDestinationFolder: true);

            LogEventEnd($"Backup complete.");
            Log("");

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

        /// <summary> Upgrade project reference to use new verion Accpac.Net </summary>
        /// <param name="title">The title of this step</param>
        /// <param name="accpacPropsOriginallyInSolutionFolder">
        /// Boolean flag denoting if the Accpac props file originally existed
        /// in the Solution folder.
        /// If it did, we don't need to do anything because it will have been updated by the previous
        /// wizard step. 
        /// If it didn't already exist in the Solution folder, we need to remove it.
        /// The SDK samples use a common Accpac props file located elsewhere in the
        /// SDK folder structure ("Settings")
        /// </param>
        private void SyncAccpacLibraries(string title, bool accpacPropsOriginallyInSolutionFolder)
        {
            var msg = string.Empty;

            // Log start of step
            LogEventStart(title);

            if (accpacPropsOriginallyInSolutionFolder == true)
            {
                CopyAccpacPropsFileToSolutionFolder();

                // Log detail
                msg = string.Format(Resources.UpgradeLibrary,
                                        Constants.PerRelease.FromAccpacNumber,
                                        Constants.PerRelease.ToAccpacNumber);
            }
            else
            {
                msg = Resources.AccpacPropsFileDoesNotExistInSolutionFolder;
            }

            Log(msg);

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Copy the AccpacDotNetProps file to the Solution folder
        /// </summary>
        private void CopyAccpacPropsFileToSolutionFolder()
        {
            var sourcePath = Path.Combine(_settings.PropsSourceFolder, Constants.Common.AccpacPropsFile);
            var destPath = Path.Combine(_settings.DestinationSolutionFolder, Constants.Common.AccpacPropsFile);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

        /// <summary>
        /// Is there a copy of the AccpacDotNetversion.props file in the Solution folder?
        /// </summary>
        /// <returns>
        /// true : AccpacDotNetVersion.props is in Solution folder 
        /// false: AccpacDotNetVersion.props is in not in the Solution folder 
        /// </returns>
        private bool IsAccpacDotNetVersionPropsLocatedInSolutionFolder()
        {
            return File.Exists(Path.Combine(_settings.DestinationSolutionFolder, 
                                            Constants.Common.AccpacPropsFile));
        }

        /// <summary>
        /// Remove an existing AccpacDotNetVersion.props file from the
        /// solution folder if it exists.
        /// </summary>
        private void RemoveExistingPropsFileFromSolutionFolder()
        {
            var oldPropsFile = Path.Combine(_settings.DestinationSolutionFolder, 
                                            Constants.Common.AccpacPropsFile);
            RemoveExistingFile(oldPropsFile);
        }

        /// <summary>
        /// Remove an existing AccpacDotNetVersion.props file from the
        /// Web project folder if it exists.
        /// </summary>
        private void RemoveExistingPropsFileFromWebProjectFolder()
        {
            var oldPropsFile = Path.Combine(_settings.DestinationWebFolder,
                                            Constants.Common.AccpacPropsFile);
            RemoveExistingFile(oldPropsFile);
        }

        /// <summary>
        /// Remove an existing file
        /// </summary>
        /// <param name="filePath">The file path specification</param>
        private void RemoveExistingFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Log($"{Resources.File} '{filePath}' {Resources.Exists}.");
                File.Delete(filePath);
                Log($"{Resources.File} '{filePath}' {Resources.Deleted}.");
            }
            else
            {
                Log($"{Resources.File} '{filePath}' {Resources.DoesNotExist}.");
            }
        }

        /// <summary>
        /// Process the 'aspnet_client' folder changes
        /// </summary>
        /// <param name="title">The title to display for this step</param>
        public void ProcessAspnetClientFolder(string title)
        {
            // Log start of step
            LogEventStart(title);

            // Nothing to do. This is a manual partner step :)

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Update the targeted version of the .NET Framework for
        /// all solution projects
        /// </summary>
        /// <param name="title">Title of step being processed </param>
        private void UpdateTargetedDotNetFrameworkVersion(string title)
        {
            // Log start of step
            LogEventStart(title);

            var projects = _settings.Solution.Projects;
            var dotNetTargetName = Constants.Common.TargetFrameworkMoniker;
            foreach (Project project in projects)
            {
                Log($"{DateTime.Now} - {Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion} : Upgrading {project.Name} .NET target to {dotNetTargetName}...");
                project.Properties.Item("TargetFrameworkMoniker").Value = dotNetTargetName;
            }

            // Log end of step
            LogEventEnd(title);
            Log("");
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
                    Log($"{Resources.AddReplaceFile} {filePath}");
                }
                catch (IOException e)
                {
                    // Likely just a locked file.
                    // Just log it and move on.
                    Log($"{Resources.ExceptionThrownPossibleLockedFile} : {file.FullName.ToString()}");
                    Log($"{e.Message}");
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

        /// <summary>
        /// Update log
        /// </summary>
        /// <param name="text">The message to log</param>
        /// <param name="withTimeStamp">
        /// Optional boolean flag denoting whether or not to insert a timestamp
        /// Default is true
        /// </param>
        private void Log(string text, bool withTimestamp = true)
        {
            var msg = text;
            if (withTimestamp)
            {
                msg = $"{DateTime.Now} - {text}";
            }
            LogEvent?.Invoke(msg);
        }

        /// <summary>
        /// Log a line with some characters to denote a divider.
        /// </summary>
        /// <param name="spacerCharacter">The character to use for the line</param>
        /// <param name="length">The length of the line</param>
        private void LogSpacerLine(char spacerCharacter = ' ', int length = 60)
        {
            var msg = new String(spacerCharacter, length);
            Log(msg);
        }

        /// <summary> Update Log - Event Start</summary>
        /// <param name="text">Text to log</param>
        private void LogEventStart(string text)
        {
            var s = $"{Resources.Start} {text} --";
            Log(s);
        }

        /// <summary> Update Log - Event End</summary>
        /// <param name="text">Text to log</param>
        private void LogEventEnd(string text)
        {
            var s = $"{Resources.End} {text} --";
            Log(s);
        }
        #endregion
    }
}
