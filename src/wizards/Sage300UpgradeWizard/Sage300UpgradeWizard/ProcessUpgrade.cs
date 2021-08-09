// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using VSLangProj;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> Process Upgrade Class (worker) </summary>
    internal class ProcessUpgrade
	{
	#region Private Variables
		/// <summary> Settings from UI </summary>
		private Settings _settings;
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
		/// <summary> Start the solution upgrade process </summary>
		/// <param name="settings">Settings for processing</param>
		public void Process(Settings settings)
		{
            // Developer Note: This number is one less than the number of steps in the main
            //                 switch statement below.
            const int WORKINGSTEPS = 6;

            LogSpacerLine('-');
            Log(Resources.BeginUpgradeProcess);
            LogSpacerLine();

            // Save settings for local usage
            _settings = settings;

            if (Constants.Common.EnableSolutionBackup)
            {
                DoOptionalSolutionBackup(backupSelected: _settings.WizardSteps[0].CheckboxValue, 
                                         solutionFolder: _settings.DestinationSolutionFolder);
            }

            // Track whether or not the AccpacDotNetVersion.props file originally existed in the Solution folder
            bool AccpacPropsFileOriginallyInSolutionfolder = false;

            // Does the AccpacDotNetVersion.props file exist in the Solution folder?
            AccpacPropsFileOriginallyInSolutionfolder = PropsFileManager.IsAccpacDotNetVersionPropsLocatedInSolutionFolder(_settings);

            // Start at step 1 and ignore last two steps
            for (var index = 0; index < _settings.WizardSteps.Count; index++)
			{
				var title = _settings.WizardSteps[index].Title;
				LaunchProcessingEvent(title);

                // Insert a spacer line for each case statement below
                if (index >= 1 && index <= WORKINGSTEPS) { LogSpacerLine('-'); }

                // Step 0 is Main and Last two steps are Upgrade and Upgraded
                switch (index)
				{
                    //
                    // Developer Note:
                    //   Ensure the constant WORKINGSTEPS, defined at start of function,  has been 
                    //   updated if steps are added or removed from the following switch statement.
                    //
                    case 1: if (Constants.PerRelease.SyncKendoFiles) { SyncKendoFiles(title); } break;
                    case 2: if (Constants.PerRelease.SyncWebFiles) { SyncWebFiles(title); } break;

                    // Release specific Steps
                    //case 3: if (Constants.PerRelease.ReportUpgrade_For_2021_2) { ReportUpgrade_For_2021_2(title); } break;

                    case 3: if (Constants.PerRelease.UpdateAccpacDotNetLibrary) { SyncAccpacLibraries(title, AccpacPropsFileOriginallyInSolutionfolder); } break;
                    case 4: if (Constants.PerRelease.RemovePreviousJqueryLibraries) { RemovePreviousJqueryLibraries(title); } break;
                    case 5: if (Constants.PerRelease.NamespaceAndWebProjectUpdates) { NamespaceAndWebProjectUpdates(title); } break;
                    case 6: if (Constants.PerRelease.FinderAlterations) { FinderAlterations(title); } break;
                    case 7: if (Constants.PerRelease.JavascriptMinificationUpdates) { JavascriptMinificationUpdates(title); } break;

                        //
                        // Developer Note: The following are optional steps from previous 
                        //                 incarnations of the Upgrade Wizard
                        //                 They have been left in place for future reference.
                        //

                        //case 5: if (Constants.PerRelease.UpdateMicrosoftDotNetFramework) { UpdateTargetedDotNetFrameworkVersion(title); } break;
                        //case 6: if (Constants.PerRelease.UpdateUnifyDisabled) { UpdateUnifyDisabled(title); } break;
                        //case 7: if (Constants.PerRelease.AddBinIncludeFile) { AddBinIncludeFile(title); } break;

#if ENABLE_TK_244885
                    case X: ConsolidateEnumerations(title); break;
#endif
                }
            }

            LogSpacerLine();
            Log(Resources.EndUpgradeProcess);
            LogSpacerLine('-');
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Do an optional backup of the Visual Studio solution
        /// </summary>
        /// <param name="backupSelected">Boolean flag representing whether or not the backup should be executed</param>
        /// <param name="solutionFolder">Fully-qualified string representing the solution path</param>
        private void DoOptionalSolutionBackup(bool backupSelected, string solutionFolder)
        {
            if (backupSelected)
            {
                LaunchProcessingEvent(Resources.BackupStarting);

                LogSpacerLine();
                LogSpacerLine('-');
                LogEventStart(Resources.BackupStarting);

                // Do the backup
                var backupFolder = SolutionBackupManager.BackupSolution(solutionFolder);

                // Log the results
                if (Directory.Exists(backupFolder))
                {
                    Log(string.Format(Resources.Template_SolutionBackupCompleted, backupFolder));

                    LaunchProcessingEvent(Resources.BackupCompleted);

                    LogEventEnd(Resources.BackupCompleted);
                }
            }
        }

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

            msg = string.Format(Resources.Template_UpgradeLibrary,
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
        /// <param name="title">The title of this step</param>
        private void UpdateUnifyDisabled(string title)
        {
            LogEventStart(title);

            // Nothing to do. This is a manual partner step :)
            var msg = Resources.UpdatesToUnifyDisabledAreAManualStep;
            Log(msg);

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Finder Alterations (2022.0)
        /// </summary>
        /// <param name="title">The title of this step</param>
        private void FinderAlterations(string title)
        {
            LogEventStart(title);

            // Nothing to do. This is a manual partner step :)
            var msg = Resources.UpdatesToFinderAlterationsAreAManualStep;
            Log(msg);

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Javascript Minification Updates (2022.0)
        /// </summary>
        /// <param name="title">The title of this step</param>
        private void JavascriptMinificationUpdates(string title)
        {
            LogEventStart(title);

            // Nothing to do. This is a manual partner step :)
            var msg = Resources.JavascriptMinificationUpdatesAreAManualStep;
            Log(msg);

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Namespace and Web Project Updates (2022.0)
        /// </summary>
        /// <param name="title">The title of this step</param>
        private void NamespaceAndWebProjectUpdates(string title)
        {
            LogEventStart(title);

            //// Nothing to do. This is a manual partner step :)
            //var msg = Resources.FinalAlterationsAreAManualStep;
            //Log(msg);

            //
            // Step 1 : Open the *WebBootstrapper.cs file and extract the namespace 
            //
            var theNamespace = String.Empty;
            var webBootstrapperFiles = System.IO.Directory.GetFiles(_settings.DestinationWebFolder, Constants.Common.WebBootstrapper);
            if (webBootstrapperFiles.Length == 1)
            {
                using (StreamReader sr = File.OpenText(webBootstrapperFiles[0]))
                {
                    var line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith("namespace "))
                        {
                            // Found the namespace line. 
                            // Let's get the actual namespace value
                            var parts = line.Split(new string[] { " " }, StringSplitOptions.None);
                            theNamespace = parts[1];
                            break;
                        }
                    }
                }

                // Log the namespace (or lackthereof)
                if (theNamespace.Length > 0)
                {
                    Log($"{Resources.Namespace} = {theNamespace}");
                }
                else
                {
                    Log($"{Resources.UnableToDetermineNamespace}");
                }
            }

            if (theNamespace.Length > 0)
            {
                //
                // Step 2 : Replace the placeholder text $safeprojectname$ in the following files
                //
                //          Login.cs, Login.aspx, Login.aspx.designer.cs
                //          Global.asax, Global.asax.cs
                //
                //          with the namespace name from Step 1
                //
                List<string> fileList = new List<string>
                {
                    "Login.aspx",
                    "Login.aspx.cs",
                    "Login.aspx.designer.cs",
                    "Global.asax",
                    "Global.asax.cs"
                };

                foreach (var name in fileList)
                {
                    var filePath = Path.Combine(_settings.DestinationWebFolder, name);
                    FileUtilities.ReplaceTextInFile(filePath, Constants.Common.SafeProjectNamePlaceholder, theNamespace);

                    // Log the result of the namespace replacement
                    var msg = String.Format(Resources.Template_SuccessfullyUpdatedNamespaceInFile, filePath);
                    Log(msg);
                }

                //
                // Step 3 : Add Login.cs, Login.aspx and Login.aspx.designer.cs to the Web project
                //
                var solution = _settings.Solution;
                var projects = solution.Projects;
                foreach (Project project in projects)
                {
                    var webProjectName = project.FullName;
                    if (IsWebProject(webProjectName))
                    {
                        try
                        {
                            var web = _settings.DestinationWebFolder;

                            List<string> files = new List<string>
                            {
                                "Login.aspx",
                                "Login.aspx.cs",
                                "Login.aspx.designer.cs",
                            };

                            foreach (var file in files)
                            {
                                project.ProjectItems.AddFromFile(Path.Combine(web, file));
                                var msg = String.Format(Resources.Template_SuccessfullyAddedFileToWebProject, file, webProjectName);
                                Log(msg);
                            }
                        }
                        catch (Exception e)
                        {
                            Log(e.Message);
                        }

                        // No need to iterate the rest of the projects
                        // We've found the Web project already.
                        break;
                    }
                }
            }

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Add a reference to the new ExportReport.exe located in the Local Sage 300 installation
        /// </summary>
        /// <param name="title">The title of this step</param>
        private void ReportUpgrade_For_2021_2(string title)
        {
            LogEventStart(title);

            string webProjectName = string.Empty;

            // Check for the existence of the following file:
            //
            // [Sage300Installation]\Web\Online\bin\Sage.CA.SBS.ERP.Sage300.ExportReport.exe
            //
            // If this file exists, add it as a reference in the web.csproj file
            //
            var exportReportExePath = Path.Combine(RegistryHelper.Sage300CWebFolder, @"bin\Sage.CA.SBS.ERP.Sage300.ExportReport.exe");
            if (File.Exists(exportReportExePath))
            {
                Log($"Found {exportReportExePath}");

                var projects = _settings.Solution.Projects;
                foreach (Project project in projects)
                {
                    // Find the Web project in the solution
                    webProjectName = project.FullName;
                    if (IsWebProject(webProjectName))
                    {
                        Log($"Found web project {webProjectName}");

                        // Add the reference to the report exe
                        //
                        // Developer Note: This will add the reference with a normal path specification,
                        //                 NOT one using the normal $(Sage300WebDir) macro.
                        //                 This is a known limitation of the DTE interface.
                        //                 https://developercommunity.visualstudio.com/t/adding-a-reference-to-vslangproj-with-a-path-based/1029534
                        //
                        //                 Changing the path to use the macro is a manual step if the end user wishes to do this.
                        //                 It's not required.
                        //
                        var vsproj = (VSProject)project.Object;
                        var pathText = exportReportExePath;
                        vsproj.References.Add(pathText);

                        Log($"Added {exportReportExePath} reference to {webProjectName}");

                        // No need to iterate the rest of the projects
                        // We've found the Web project already.
                        break;
                    }
                }
            }
            else
            {
                Log($"{exportReportExePath} not found.");
                Log($"Reference to {exportReportExePath} was NOT added to {webProjectName}");
            }

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Add the new 'BinInclude.txt' file to the Web project
        /// </summary>
        /// <param name="title">The title of this step</param>
        private void AddBinIncludeFile(string title)
        {
            LogEventStart(title);

            var binInclude = Path.Combine(_settings.DestinationWebFolder, Constants.Common.BinIncludeFile);
            if (!File.Exists(binInclude))
            {
                // File doesn't yet exist. Let's create an empty one and add it to the Web project
                FileUtilities.CreateEmptyFile(binInclude);

                // Add this file to the project definition (if it was actually created successfully)
                if (File.Exists(binInclude))
                {
                    var solution = _settings.Solution;
                    var projects = solution.Projects;
                    foreach (Project project in projects)
                    {
                        var fullName = project.FullName;
                        if (IsWebProject(fullName))
                        {
                            project.ProjectItems.AddFromFile(binInclude);

                            // No need to iterate the rest of the projects
                            // We've found the Web project already.
                            break;
                        }
                    }
                }
            }

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Check the full name of a Visual Studio project for the existence 
        /// of the Web project search pattern
        /// </summary>
        /// <param name="projectPath">The fully-qualified path to the Visual Studio project</param>
        /// <returns>true = path contains the search pattern | false = path does not contain the search pattern</returns>
        private bool IsWebProject(string projectPath)
        {
            return projectPath.ToLowerInvariant().Contains(Constants.Common.WebProjectNamePattern);
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

            try
            {
                var projects = _settings.Solution.Projects;
                var dotNetTargetName = Constants.Common.TargetFrameworkMoniker;
                foreach (Project project in projects)
                {
                    var projectName = project.Name;
                    if (projectName != Constants.Common.NugetName)
                    {
                        Log($"{Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion} : Attempting to upgrading {project.Name} .NET target to {dotNetTargetName}...");
                        project.Properties.Item("TargetFrameworkMoniker").Value = dotNetTargetName;
                        Log($"{Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion} : Upgrade successful.");
                    }
                }
            }
            catch (Exception e)
            {
                Log($"{Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion} : Exception caught: {e.Message}");
            }

            // Log end of step
            LogEventEnd(title);
            Log("");
        }

        /// <summary>
        /// Remove instances of previous jQuery libraries from project folders and any references in the .csproj files
        /// Developer Note: 
        ///   Updated Jquery files will be included in web project automatically via wildcard mechanism as follows:
        ////       <Content Include="Scripts\**" />
        /// </summary>
        /// <param name="title">Title of step being processed </param>
        private void RemovePreviousJqueryLibraries(string title)
        {
            // Log start of step
            LogEventStart(title);

            var webScriptsFolder = Path.Combine(_settings.DestinationWebFolder, Constants.Common.ScriptsFolderName);

            // 
            // Step 1 - Remove old versions of jQuery libraries
            //
            var filesToDelete = new List<string>
            {
                // jQuery Core
                "jquery-3.4.1.js",
                "jquery-3.4.1.min.js",
                "jquery-3.4.1.min.map",

                // jQuery Migrate
                "jquery-migrate-3.1.0.js",
                "jquery-migrate-3.1.0.min.js",
            };

            foreach (var filename in filesToDelete)
            {
                var filePath = Path.Combine(webScriptsFolder, filename);
                FileUtilities.RemoveExistingFile(filePath);
                Log($"Removed {filePath}");
            }

            //
            // Step 2 - Update some jQuery references to the latest version
            //
            List<string> fileList = new List<string>
            {
                "CustomReportViewer.aspx",
                "ReportViewer.aspx"
            };

            var previousVersion = "3.4.1";
            var newVersion = "3.6.0";

            foreach (var name in fileList)
            {
                var filePath = Path.Combine(_settings.DestinationWebFolder, Constants.Common.WebFormsFolderName, name);
                FileUtilities.ReplaceTextInFile(filePath, $"jquery-{previousVersion}.js", $"jquery-{newVersion}.js");

                // Log the result of the namespace replacement
                var msg = String.Format(Resources.Template_SuccessfullyUpdatedJQueryVersionInFile, previousVersion, newVersion, filePath);
                Log(msg);
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
