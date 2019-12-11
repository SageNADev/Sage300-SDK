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
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.Properties;
using Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
//using Microsoft.Build.Evaluation;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard
{
    /// <summary> Process Upgrade Class (worker) </summary>
    internal class GenerateLanguageResources
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
		/// <summary> Start the language resource generation process </summary>
		/// <param name="settings">Settings for processing</param>
		public void Process(Settings settings)
		{
            LogSpacerLine('-');
            Log(Resources.BeginUpgradeProcess);
            LogSpacerLine();

            LaunchProcessingEvent("Adding new language resources...");

            // Save settings for local usage
            _settings = settings;

            var projects = GetProjects(_settings.Solution);

            // Iterate solution to get projects for analysis and usage
            foreach (var project in projects)
            {
                var projectName = project.Name;
                LaunchProcessingEvent(projectName);

                // Get the project path
                var projectPath = Path.GetDirectoryName(project.FullName);

                //var regex = new Regex();
                var files = Directory.GetFiles(projectPath, "*Resx.resx", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file);

                    var directoryName = fi.DirectoryName;

                    // Get the name only (without any extension)
                    var nameOnly = fi.Name.Split(new char[] {'.'})[0];

                    // Get the extension
                    var extension = fi.Extension;

                    // Build the name for the new language resource file.
                    var selectedLanguageCode = _settings.Language.Code;
                    var newName = $"{nameOnly}.{selectedLanguageCode}{extension}";

                    LaunchProcessingEvent($"{projectName} - {newName}");

                    // Build the full path to the new file
                    var newFilePath = Path.Combine(directoryName, newName);

                    // copy it!
                    File.Copy(file, newFilePath, overwrite: true);

                    // ...and add to the project
                    project.ProjectItems.AddFromFile(newFilePath);
                }
            }

            LogSpacerLine();
            Log(Resources.EndUpgradeProcess);
            LogSpacerLine('-');
        }

        #endregion

        #region Private methods

        /// <summary> Gets projects </summary>
        /// <param name="solution">Solution </param>
        /// <returns>List of projects</returns>
        private static IEnumerable<Project> GetProjects(_Solution solution)
        {
            // Locals
            var projects = solution.Projects;
            var list = new List<Project>();
            var item = projects.GetEnumerator();

            try
            {
                // Iterate 
                while (item.MoveNext())
                {
                    var project = (Project)item.Current;

                    if (project == null)
                    {
                        continue;
                    }

                    // only add project folder
                    if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    {
                        list.AddRange(GetSolutionFolderProjects(project));
                    }
                    else
                    {
                        list.Add(project);
                    }
                }
            }
            catch
            {
                // No action as it will be reviewed by caller
            }

            return list;
        }

        /// <summary> Gets projects in solution folder </summary>
        /// <param name="project">Project </param>
        /// <returns>List of projects</returns>
        private static IEnumerable<Project> GetSolutionFolderProjects(Project project)
        {
            // Locals
            var list = new List<Project>();

            // Iterate projects
            for (var projectItem = 1; projectItem <= project.ProjectItems.Count; projectItem++)
            {
                var subProject = project.ProjectItems.Item(projectItem).SubProject;

                if (subProject == null)
                {
                    continue;
                }

                // Recursion for another solution folder
                if (subProject.Kind.Equals(ProjectKinds.vsProjectKindSolutionFolder))
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }

            return list;
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
