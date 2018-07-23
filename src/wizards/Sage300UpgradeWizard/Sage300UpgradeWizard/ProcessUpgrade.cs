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
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
	/// <summary> Process Upgrade Class (worker) </summary>
	internal class ProcessUpgrade
	{
	#region Private Vars
		/// <summary> Settings from UI </summary>
		private Settings _settings;
		private string _backupFolder = String.Empty;
	#endregion

	#region Public constants
		/// <summary> From Release Number </summary>
		public const string FromReleaseNumber = "2018.2";

		/// <summary> To Release Number </summary>
		public const string ToReleaseNumber = "2019.0";

		/// <summary> From Accpac Number </summary>
		public const string FromAccpacNumber = "6.5.0.20";

		/// <summary> To Accpac Number </summary>
		public const string ToAccpacNumber = "6.6.0.0";

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

			// Track whether or not the AccpacDotNetVersion.props file originally existed in the Web folder.
			// If it does/did, then we will just update it in place instead of relocating it to the Solution folder.
			bool AccpacPropsFileOriginallyInWebFolder = false;

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
					case 1:
						SyncWebFiles(title, out AccpacPropsFileOriginallyInWebFolder);
						break;

					case 2:
						SyncAccpacLibraries(title, AccpacPropsFileOriginallyInWebFolder);
						break;

	#region Release Specific Steps
	/*
					case 3:
						UpdateVendorSourceCodeAutomatically(title, _backupFolder);
						break;

					case 4:
						UpdateVendorMenuDetails(title, _backupFolder);
						break;

					case 5:
						UpdateProjectPostBuildEvent(title, _backupFolder);
						break;

					case 6:
						UpdateVendorSourceCodeManually(title);
						break;
	*/
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
			var txt = string.Format(Resources.UpgradeLibrary, FromAccpacNumber, ToAccpacNumber);
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
            var oldPropsFile = Path.Combine(_settings.DestinationSolutionFolder, AccpacPropsFile);
            if (File.Exists(oldPropsFile)) { File.Delete(oldPropsFile); }
        }

        /// <summary>
        /// Copy the new AccpacDotNetVersion.props file to the Solution
        /// folder
        /// </summary>
        private void CopyNewPropsFileToSolutionFolder()
        {
            var file = Path.Combine(_settings.DestinationSolutionFolder, AccpacPropsFile);
            var srcFilePath = Path.Combine(_settings.SourceFolder, AccpacPropsFile);
            File.Copy(srcFilePath, file, true);
        }

        /// <summary>Update the command to be run in the Web project PostBuildEvent</summary>
        /// <param name="title">Title of step being processed</param>
        private void UpdateProjectPostBuildEvent(string title, string backupFolder = @"")
        {
            const string fileTypeFilter = @"*.Web.csproj";

            // Log start of step
            LaunchLogEventStart(title);

            // Update the PostBuildEvent entry in the Web project file
            var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);

			// Get the list of files to process, based on the fileTypeFilter
			// and optional list of folders to ignore, if found
			var listToProcess = EnumerateFiles(slnDir, fileTypeFilter, ignoreDirectories: new List<string> { backupFolder });

			// Extra Logging
			LaunchLogEvent($"{Resources.SolutionDirectory} : {slnDir.FullName.ToString()}");
			LaunchLogEvent($"{Resources.SolutionProjects}:");
			foreach (var filename in listToProcess)
			{
				LaunchLogEvent($"Project: {filename}");
			}

			foreach (var projFile in listToProcess)
            {
				LaunchLogEvent($"{Resources.Processing} {projFile}");
                var projectXmlDoc = new XmlDocument();
                projectXmlDoc.Load(projFile);
                var nodes = projectXmlDoc.ChildNodes[1].ChildNodes;
                var hasChanges = false;

                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType != XmlNodeType.Element) continue;

                    var e = (XmlElement)node;

                    if (!IsPropertyGroupWithoutAttributes(e)) continue;

                    foreach (XmlElement n in e.ChildNodes)
                    {
                        if (!IsPostBuildEventElement(n)) continue;

						// Let's just rebuild the whole PostBuildEvent entry from scratch
						var menuFileName = GetMenuFileName();
						LaunchLogEvent($"{Resources.FoundPostBuildEventEntry}.");
						n.InnerText = BuildCommandLine(menuFileName);
						LaunchLogEvent($"{Resources.InsertedCommand}: {n.InnerText}");
						hasChanges = true;
					}
				}

                // Save the file if anything changed
                if (hasChanges)
                {
                    projectXmlDoc.Save(projFile);
                    LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdatePostBuildEvent} : {projFile}");
                }
            }
            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
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
		private string GetMenuFileName(string backupFolder = @"")
		{
			string fileTypeFilter = @"*MenuDetails.xml";
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
			string menuFileTemplate = "{0}MenuDetails.xml";
			return String.Format(menuFileTemplate, moduleId);
		}

		/// <summary>
		/// Convert the Web project PostBuildEvent command string
		/// from one version to a new version
		/// </summary>
		/// <param name="fromVersion">The product version we're converting from</param>
		/// <param name="toVersion">The product version we're converting to</param>
		/// <param name="existingCommand"></param>
		/// <returns>The updated PostBuildEvent command string</returns>
		private string ConvertPostBuildEventCommand(string fromVersion,
                                                    string toVersion,
                                                    string existingCommand)
        {
            var outputCommand = existingCommand;

            if (fromVersion == FromReleaseNumber && toVersion == ToReleaseNumber)
            {
                // 2018.1 command format
                // Call "$(ProjectDir)MergeISVProject.exe" "$(SolutionPath)"  
                //                                         "$(ProjectDir)\"  
                //                                         PMMenuDetails.xml 
                //                                         $(ConfigurationName) 
                //                                         $(FrameworkDir)

                // 2018.2 command format
                // Call "$(ProjectDir)MergeISVProject.exe" --mode=0 
                //                                         --solutionpath="$(SolutionDir)\" 
                //                                         --webprojectpath="$(ProjectDir)\" 
                //                                         --menufilename="[ModuleName]MenuDetails.xml" 
                //                                         --buildprofile="$(ConfigurationName)" 
                //                                         --dotnetframeworkpath="$(FrameworkDir)$(FrameworkVersion)" 
                //                                         --minify 
                //                                         --log

                outputCommand = BuildCommandLine(ExtractMenuFileName(existingCommand));
            }

            return outputCommand;
        }

        /// <summary>
        /// Returns the new PostBuildEvent command-line string
        /// </summary>
        /// <param name="menuName">The menu file name [XXMenuDetails.xml] where XX == Module Id</param>
        /// <returns>The PostBuildEvent command-line string</returns>
        private string BuildCommandLine(string menuName)
        {
			// Note: The SolutionDir and ProjectDir below
			// require the extra \ at the end.
            var sb = new StringBuilder();
            sb.Append($"Call ");
            sb.Append($"\"$(ProjectDir)MergeISVProject.exe\" ");
            sb.Append($"--mode=0 ");
			sb.Append($"--solutionpath=\"$(SolutionDir)\\\" ");
            sb.Append($"--webprojectpath=\"$(ProjectDir)\\\" ");
            sb.Append($"--menufilename=\"{menuName}\" ");
            sb.Append($"--buildprofile=\"$(ConfigurationName)\" ");
            sb.Append($"--dotnetframeworkpath=\"$(FrameworkDir)$(FrameworkVersion)\" ");
            sb.Append($"--minify ");
            sb.Append($"--log ");
            return sb.ToString();
        }

        /// <summary>
        /// Extract the module menu file name from the existing PostBuildEvent command string
        /// </summary>
        /// <param name="command">The existing PostBuildEvent command string</param>
        /// <returns>The menu file name [XXMenuDetails.xml] where XX == Module Id</returns>
        private string ExtractMenuFileName(string command)
        {
			string result = string.Empty;
            const int commandStringMenuItemIndex = 4;
            if (command.Length == 0) return string.Empty;
            var parts = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			// Remove any extra quotes
			result = parts[commandStringMenuItemIndex].Replace("\"", "");

			// Grab the menu filename. That's all we need.
			return result;
        }

        /// <summary>
        /// Inspect an XmlElement node to determine if it's
        /// a PropertyGroup without any attributes
        /// </summary>
        /// <param name="e">The XmlElement item to inspect</param>
        /// <returns>
        /// true = PropertyGroup node without any attributes
        /// false = PropertyGroup node with attributes
        /// </returns>
        private bool IsPropertyGroupWithoutAttributes(XmlElement e)
        {
            return e.Name == "PropertyGroup" && e.HasAttributes == false;
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
		private bool HasIconNameElement(XmlElement e)
		{
			// This as already been done but better to be safe than sorry!
			if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
			{
				foreach (XmlElement n in e.ChildNodes)
				{
					if (n.Name.ToLowerInvariant() == "iconname")
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Does this node contain an element called <MenuBackGoundImage></MenuBackGoundImage>
		/// </summary>
		/// <param name="e">The XML Element in question</param>
		/// <returns>true = MenuBackGoundImage element found </returns>
		private bool HasMenuBackGroundImageElement(XmlElement e)
		{
			// This as already been done but better to be safe than sorry!
			if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
			{
				foreach (XmlElement n in e.ChildNodes)
				{
					if (n.Name.ToLowerInvariant() == "menubackgoundimage")
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Determine whether or not an XmlElement is a PostBuildEvent
		/// </summary>
		/// <param name="e"></param>
		/// <returns>
		/// true = XmlElement is a PostBuildEvent
		/// false = XmlElement is not a PostBuildEvent
		/// </returns>
		private static bool IsPostBuildEventElement(XmlElement e) => e.Name.ToUpperInvariant() == "POSTBUILDEVENT";

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
		/// Update any source code:
		/// 
		/// ...Web\BundleRegistration.cs
		///     Rename instances of 'new ScriptBundle(' with 'new Bundle('
		/// 
		/// </summary>
		/// <param name="title">Title of step being processed</param>
		private void UpdateVendorSourceCodeAutomatically(string title, string backupFolder = @"")
        {
            // Log start of step
            LaunchLogEventStart(title);

			// Update the file(s)
			var fileTypeFilter = @"BundleRegistration.cs";
			var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);

			// Get the list of files to process, based on the fileTypeFilter
			// and optional list of folders to ignore, if found
			var listToProcess = EnumerateFiles(slnDir, fileTypeFilter, ignoreDirectories: new List<string> { backupFolder });

			foreach (var filepath in listToProcess)
			{
				// For now, only one file needs to be updated.
				var sourceCode = File.ReadAllText(filepath);
				sourceCode = sourceCode.Replace(@"new ScriptBundle(", "new Bundle(");
				File.WriteAllText(filepath, sourceCode);
				LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateSourceCode} : {filepath}");
			}

			// Log end of step
			LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

		/// <summary>
		/// Display some text informing the user that they will need to manually update
		/// some of their source code.
		/// </summary>
		/// <param name="title">Title of step being processed</param>
		private void UpdateVendorSourceCodeManually(string title)
		{
			// Log start of step
			LaunchLogEventStart(title);

			// Just display a message
			LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateSourceCodeManually}");

			// Log end of step
			LaunchLogEventEnd(title);
			LaunchLogEvent("");
		}

		/// <summary>
		/// Update the XXMenuDetails.xml icon and background images
		/// </summary>
		/// <param name="title">Title of step being processed</param>
		private void UpdateVendorMenuDetails(string title, string backupFolder = @"")
		{
			const string fileTypeFilter = @"*MenuDetails.xml";

			// Log start of step
			LaunchLogEventStart(title);

			var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);

			// Get the list of files to process, based on the fileTypeFilter
			// and optional list of folders to ignore, if found
			var listToProcess = EnumerateFiles(slnDir, fileTypeFilter, ignoreDirectories: new List<string> { backupFolder });

			// Extra Logging
			LaunchLogEvent($"{Resources.SolutionDirectory}: {slnDir.FullName.ToString()}");
			LaunchLogEvent($"{Resources.SolutionProjectsToBeProcessed}:");
			foreach (var filepath in listToProcess) { LaunchLogEvent($"{Resources.Project}: {filepath}"); }

			foreach (var filepath in listToProcess)
			{
				LaunchLogEvent($"{Resources.Processing} {filepath}");

				// Get the ModuleID from the filename
				var moduleId = filepath.Substring(0, 2);
				LaunchLogEvent($"{Resources.ExtractedModuleId} = {moduleId}");

				// Get the Company Name
				var companyName = GetCompanyName(slnDir, moduleId);
				LaunchLogEvent($"{Resources.CompanyName} = {companyName}");

				var xmlDoc = new XmlDocument();
				xmlDoc.Load(filepath);

				// Now find the <Navigation> node.
				var navigationNode = FindNavigationNode(xmlDoc);
				var hasChanges = false;

				foreach (XmlNode node in navigationNode)
				{
					if (node.NodeType != XmlNodeType.Element) continue;
					var e = (XmlElement)node;
					if (!IsSecondLevelMenuItem(e)) continue;

					if (!HasIconNameElement(e))
					{
						LaunchLogEvent($"{Resources.MainMenuNodeFoundButDoesNotHaveExistingIconNameEntry}.");

						// Couldn't find <IconName></IconName> so let's add it.
						var newNode = xmlDoc.CreateNode(XmlNodeType.Element, "IconName", null);
						newNode.InnerText = $"{companyName}/menuIcon.png";
						e.AppendChild(newNode);
 						hasChanges = true;
					}

					if (!HasMenuBackGroundImageElement(e))
					{
						LaunchLogEvent($"{Resources.MainMenuNodeFoundButDoesNotHaveExistingMenuBackGoundEntry}.");

						// Couldn't find <MenuBackGoundImage></MenuBackGoundImage> so let's add it.
						var newNode = xmlDoc.CreateNode(XmlNodeType.Element, "MenuBackGoundImage", null);
						newNode.InnerText = $"{companyName}/menuBackGroundImage.jpg";
						e.AppendChild(newNode);
						hasChanges = true;
					}
				}

				// Save the file if anything changed
				if (hasChanges)
				{
					xmlDoc.Save(filepath);
					LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateMenuDetails} : {filepath}");
				}
			}
			// Log end of step
			LaunchLogEventEnd(title);
			LaunchLogEvent("");
		}


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
		/// </summary>
		/// <param name="solution">A DirectoryInfo object holding the visual studio solution information</param>
		/// <param name="moduleId">The two letter module designation</param>
		/// <returns>The extracted company name </returns>
		private string GetCompanyName(DirectoryInfo solution, string moduleId)
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
		private void DeleteFolder(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

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
