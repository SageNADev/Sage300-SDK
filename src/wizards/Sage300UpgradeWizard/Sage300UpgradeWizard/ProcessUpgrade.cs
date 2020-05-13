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
using EnvDTE;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities;
using System;
using System.Collections.Generic;
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

            #region Backup Solution - Currently Disabled
            //_backupFolder = SolutionBackupManager.BackupSolution(_settings.DestinationSolutionFolder);
            #endregion

            // Does the AccpacDotNetVersion.props file exist in the Solution folder?
            AccpacPropsFileOriginallyInSolutionfolder = PropsFileManager.IsAccpacDotNetVersionPropsLocatedInSolutionFolder(_settings);

            // Start at step 1 and ignore last two steps
            for (var index = 0; index < _settings.WizardSteps.Count; index++)
			{
				var title = _settings.WizardSteps[index].Title;
				LaunchProcessingEvent(title);

				// Step 0 is Main and Last two steps are Upgrade and Upgraded
				switch (index)
				{
                    case 1:
                        LogSpacerLine('-');
                        if (Constants.PerRelease.SyncKendoFiles == true)
                        {
                            SyncKendoFiles(title);
                        }
                        break;

                    case 2:
                        LogSpacerLine('-');
                        if (Constants.PerRelease.SyncWebFiles == true)
                        {
                            SyncWebFiles(title);
                        }
                        break;

                    #region Release Specific Upgrade Steps

                    case 3:
                        LogSpacerLine('-');
                        if (Constants.PerRelease.UpdateAccpacDotNetLibrary == true)
                        {
                            SyncAccpacLibraries(title, AccpacPropsFileOriginallyInSolutionfolder);
                        }
                        break;

                    case 4:
                        LogSpacerLine('-');
                        if (Constants.PerRelease.RemovePreviousJqueryLibraries == true)
                        {
                            RemovePreviousJqueryLibraries(title);
                        }
                        break;

                    case 5:
                        LogSpacerLine('-');
                        if (Constants.PerRelease.UpdateMicrosoftDotNetFramework == true)
                        {
                            UpdateTargetedDotNetFrameworkVersion(title);
                        }
                        break;

                    case 6:
                        LogSpacerLine('-');
                        UpdateUnifyDisabled(title);
                        break;

#if ENABLE_TK_244885
                    case X:
                        ConsolidateEnumerations(title);
                        break;
#endif

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
            FileUtilities.DirectoryCopy(_settings.SourceFolder, _settings.DestinationWebFolder, ignoreDestinationFolder: false);
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

            // Was the Accpac props file originally found in the solution folder?
            if (accpacPropsOriginallyInSolutionFolder == false)
            {
                msg = String.Format(Resources.Template_AccpacPropsFileNotFoundInRootOfSolutionFolder, Constants.Common.AccpacPropsFile);
                Log(msg);

                msg = Resources.SearchingInAllProjectDirectoriesInstead;
                Log(msg);

                // Accpac Props file is likely located in one or more folders OTHER THAN the solution root.
                // We will then find them and update the csproj file located in the same folder to point to a
                // version of the props file that will live in the solution root instead.
                IEnumerable<string> list = FileUtilities.EnumerateFiles(_settings.DestinationSolutionFolder, Constants.Common.AccpacPropsFile);
                var fileCount = ((List<string>)list).Count;
                if (fileCount > 0)
                {
                    msg = String.Format(Resources.Template_XCopiesOfPropsFileWereFound, fileCount, Constants.Common.AccpacPropsFile);
                    Log(msg);

                    foreach (var file in list)
                    {
                        msg = $"     {file}\n";
                        Log(msg);
                    }

                    msg = String.Format(Resources.Template_AttemptingToUpdateAllCsprojFiles, Constants.Common.AccpacPropsFile);
                    Log(msg);

                    PropsFileManager.UpdateAccpacPropsFileReferencesInProjects(list);

                    msg = String.Format(Resources.Template_RemovingAllCopiesOfAccpacPropsFile, Constants.Common.AccpacPropsFile);
                    Log(msg);

                    PropsFileManager.RemoveAccpacPropsFromProjectFolders(list);
                }
            }
            else
            {
                // Accpac props file was found in the root of the solution folder.
                // Not necessary to look elsewhere for it :)
            }


            // Always copy the new props file to the solution root
            PropsFileManager.CopyAccpacPropsFileToSolutionFolder(_settings);

            msg = string.Format(Resources.UpgradeLibrary,
                                Constants.PerRelease.FromAccpacNumber,
                                Constants.PerRelease.ToAccpacNumber);
            Log(msg);

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Unify html 'disabled' attribute
        /// </summary>
        /// <param name="title"></param>
        private void UpdateUnifyDisabled(string title)
        {
            LogEventStart(title);

            // Nothing to do. This is a manual partner step :)
            var msg = Resources.UpdatesToUnifyDisabledAreAManualStep;

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Remove an existing AccpacDotNetVersion.props file from the
        /// solution folder if it exists.
        /// </summary>
        private void RemoveExistingPropsFileFromSolutionFolder()
        {
            var oldPropsFile = Path.Combine(_settings.DestinationSolutionFolder, 
                                            Constants.Common.AccpacPropsFile);
            FileUtilities.RemoveExistingFile(oldPropsFile);
        }

        /// <summary>
        /// Remove an existing AccpacDotNetVersion.props file from the
        /// Web project folder if it exists.
        /// </summary>
        private void RemoveExistingPropsFileFromWebProjectFolder()
        {
            var oldPropsFile = Path.Combine(_settings.DestinationWebFolder,
                                            Constants.Common.AccpacPropsFile);
            //RemoveExistingFile(oldPropsFile);
            FileUtilities.RemoveExistingFile(oldPropsFile);
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

        /// <summary>
        /// Remove instances of previous jQuery libraries from project folders and any references in the .csproj files
        /// </summary>
        /// <param name="title">Title of step being processed </param>
        private void RemovePreviousJqueryLibraries(string title)
        {
            // Log start of step
            LogEventStart(title);

            var webScriptsFolder = Path.Combine(_settings.DestinationWebFolder, Constants.Common.ScriptsFolderName);

            //
            // jQuery Core
            //
            var rootFilename = $"jquery-{Constants.PerRelease.FromJqueryCoreVersion}";
            var filename = $"{rootFilename}.js";
            var filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            filename = $"{rootFilename}.intellisense.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            filename = $"{rootFilename}.min.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            filename = $"{rootFilename}.min.map";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            //
            // jQuery UI
            //
            rootFilename = $"jquery-ui-{Constants.PerRelease.FromJqueryUIVersion}";
            filename = $"{rootFilename}.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            filename = $"{rootFilename}.min.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            //
            // jQuery Migrate
            //
            rootFilename = $"jquery-migrate-{Constants.PerRelease.FromJqueryMigrateVersion}";
            filename = $"{rootFilename}.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

            filename = $"{rootFilename}.min.js";
            filePath = Path.Combine(webScriptsFolder, filename);
            FileUtilities.RemoveExistingFile(filePath);

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
