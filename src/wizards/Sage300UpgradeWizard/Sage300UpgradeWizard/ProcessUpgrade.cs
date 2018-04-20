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
using System;
using System.IO;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using System.IO.Compression;
using System.Xml;
using System.Text;
#endregion

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
        public const string FromReleaseNumber = "2018.1";

        /// <summary> To Release Number </summary>
        public const string ToReleaseNumber = "2018.2";

        /// <summary> From Accpac Number </summary>
        public const string FromAccpacNumber = "6.5.0.10";

        /// <summary> To Accpac Number </summary>
        public const string ToAccpacNumber = "6.5.0.20";

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
                var title = _settings.WizardSteps[index].Title;
                LaunchProcessingEvent(title);

                // Step 0 is Main and Last two steps are Upgrade and Upgraded
                switch (index)
                {
                    case 1:
                        SyncWebFiles(title);
                        break;

                    case 2:
                        SyncAccpacLibraries(title);
                        break;

                    #region Release Specific Steps
                    case 3:
                        UpdateVendorSourceCodeAutomatically(title);
                        break;

					case 4:
						UpdateVendorMenuDetails(title);
						break;

					case 5:
                        UpdateProjectPostBuildEvent(title);
                        break;

					case 6:
						UpdateVendorSourceCodeManually(title);
						break;
						#endregion
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
			LaunchLogEventStart(title);

			// Do the work :)
			//DeleteFolder(sourceWebFolder);
			//ZipFile.ExtractToDirectory(zipFile, sourceWebFolder);
			DirectoryCopy(_settings.SourceFolder, _settings.DestinationWebFolder);

			// Remove the files that are not actually part of the 'Web' bundle.
			// This is done because of the way VS2017 doesn't seem to allow embedding of zip
			// files within another zip file.
			File.Delete(Path.Combine(_settings.DestinationWebFolder, @"__TemplateIcon.ico"));
			File.Delete(Path.Combine(_settings.DestinationWebFolder, @"AccpacDotNetVersion.props"));
			File.Delete(Path.Combine(_settings.DestinationWebFolder, @"Items.vstemplate"));

			// Log end of step
			LaunchLogEventEnd(title);
			LaunchLogEvent("");
		}

		/// <summary> Upgrade project reference to use new verion Accpac.Net </summary>
		/// <param name="title">Title of step being processed </param>
		private void SyncAccpacLibraries(string title)
        {
            // Log start of step
            LaunchLogEventStart(title);

            // Do the actual work :)
            RemoveExistingPropsFileFromSolutionFolder();
            CopyNewPropsFileToSolutionFolder();

            // Log detail
            var txt = string.Format(Resources.UpgradeLibrary, FromAccpacNumber, ToAccpacNumber);
            LaunchLogEvent($"{DateTime.Now} {txt}");

            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
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
        private void UpdateProjectPostBuildEvent(string title)
        {
            const string webProjectsOnly = @"*.Web.csproj";

            // Log start of step
            LaunchLogEventStart(title);

            // Update the PostBuildEvent entry in the Web project file
            var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);
            var csprojFiles = slnDir.EnumerateFiles(webProjectsOnly, SearchOption.AllDirectories);
            foreach (var projFile in csprojFiles)
            {
                var projectXmlDoc = new XmlDocument();
                projectXmlDoc.Load(projFile.FullName);
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

                        if (!string.IsNullOrEmpty(n.InnerText))
                        {
                            // Found an existing PostBuildEvent command, 
                            // so let's convert it to the new format
                            n.InnerText = ConvertPostBuildEventCommand(FromReleaseNumber,
                                ToReleaseNumber,
                                n.InnerText);
                            hasChanges = true;
                        }
                        else
                        {
                            // No command found so let's add it.
                            n.InnerText = BuildCommandLine(GetMenuFileNameFromProjectName(projFile));
                            hasChanges = true;
                        }
                    }
                }

                // Save the file if anything changed
                if (hasChanges)
                {
                    projectXmlDoc.Save(projFile.FullName);
                    LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdatePostBuildEvent} : {projFile.FullName}");
                }
            }
            // Log end of step
            LaunchLogEventEnd(title);
            LaunchLogEvent("");
        }

        /// <summary>
        /// Craft up a menu filename based on the project name.
        /// </summary>
        /// <param name="f">A FileInfo object</param>
        /// <returns>
        /// Return the name of a menufile in the following format [XX]MenuFile.xml
        /// where [XX] is the ModuleId
        /// </returns>
        private string GetMenuFileNameFromProjectName(FileInfo f)
        {
            var parts = f.Name.Split(new string[] { "." }, StringSplitOptions.None);
            var moduleId = parts[1];
            return $"{moduleId}MenuDetails.xml";
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
                //                                         --solutionpath="$(SolutionDir)" 
                //                                         --webprojectpath="$(ProjectDir)" 
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
            var sb = new StringBuilder();
            sb.Append($"Call ");
            sb.Append($"\"$(ProjectDir)MergeISVProject.exe\" ");
            sb.Append($"--mode=0 ");
            sb.Append($"--solutionpath=\"$(SolutionDir)\" ");
            sb.Append($"--webprojectpath=\"$(ProjectDir)\" ");
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
            const int commandStringMenuItemIndex = 4;
            if (command.Length == 0) return string.Empty;
            var parts = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // Grab the menu filename. That's all we need.
            return parts[commandStringMenuItemIndex];
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
			if (e.Name.ToLowerInvariant() == "item" && e.HasAttributes == false)
			{
				foreach (XmlElement n in e.ChildNodes)
				{
					if (n.Name.ToUpperInvariant() == "MENUITEMLEVEL")
					{
						var menuItemLevel = n.InnerText;
						if (!string.IsNullOrEmpty(menuItemLevel))
						{
							return Convert.ToInt32(menuItemLevel) == 2;
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
		private void UpdateVendorSourceCodeAutomatically(string title)
        {
            // Log start of step
            LaunchLogEventStart(title);

			// Update the file(s)
			var fileToUpdate = @"BundleRegistration.cs";
			var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);
			var sourceCodeFiles = slnDir.EnumerateFiles(fileToUpdate, SearchOption.AllDirectories);
			foreach (var file in sourceCodeFiles)
			{
				// For now, only one file needs to be updated.
				var sourceCode = File.ReadAllText(file.FullName);
				sourceCode = sourceCode.Replace(@"new ScriptBundle(", "new Bundle(");
				File.WriteAllText(file.FullName, sourceCode);
				LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateSourceCode} : {file.FullName}");
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
		private void UpdateVendorMenuDetails(string title)
		{
			const string menuDetailsFilePattern = @"*MenuDetails.xml";

			// Log start of step
			LaunchLogEventStart(title);

			var slnDir = new DirectoryInfo(_settings.DestinationSolutionFolder);
			var fileList = slnDir.EnumerateFiles(menuDetailsFilePattern, SearchOption.AllDirectories);
			foreach (var menuFile in fileList)
			{
				// Get the ModuleID from the filename
				var moduleId = menuFile.ToString().Substring(0, 2);

				// Get the Company Name
				var companyName = GetCompanyName(slnDir, moduleId);

				var xmlDoc = new XmlDocument();
				xmlDoc.Load(menuFile.FullName);
				var nodes = xmlDoc.ChildNodes[1].ChildNodes;
				var hasChanges = false;

				foreach (XmlNode node in nodes)
				{
					if (node.NodeType != XmlNodeType.Element) continue;
					var e = (XmlElement)node;
					if (!IsSecondLevelMenuItem(e)) continue;

					if (!HasIconNameElement(e))
					{
						// Couldn't find <IconName></IconName> so let's add it.
						var newNode = xmlDoc.CreateNode(XmlNodeType.Element, "IconName", null);
						newNode.InnerText = $"{companyName}/menuIcon.png";
						e.AppendChild(newNode);
						hasChanges = true;
					}

					if (!HasMenuBackGroundImageElement(e))
					{
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
					xmlDoc.Save(menuFile.FullName);
					LaunchLogEvent($"{DateTime.Now} {Resources.ReleaseSpecificTitleUpdateMenuDetails} : {menuFile.FullName}");
				}
			}
			// Log end of step
			LaunchLogEventEnd(title);
			LaunchLogEvent("");
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
				LaunchLogEvent($"{DateTime.Now} {Resources.AddReplaceFile} {filePath}");
			}

            // For recursion
            foreach (var subdir in dirs)
            {
                DirectoryCopy(subdir.FullName, Path.Combine(destinationDirectoryName, subdir.Name));
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
