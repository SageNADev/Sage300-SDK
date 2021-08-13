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
using WebTemplateGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
#endregion

namespace WebTemplateGenerator
{
    /// <summary>
    /// This is the class where the magic happens :)
    /// </summary>
    public class Driver
	{
		#region Public Variables
		public ILogger Logger = Program.Logger;
		public CommandLineOptions _Options;
		#endregion

		#region Constructor(s)
		/// <summary>
		/// This is the primary constructor used in this program.
		/// </summary>
		/// <param name="args">This is the string[] of command-line parameters</param>
		public Driver(string[] args)
		{
			Utilities.GetAppInfo(out string appName, out string appVersion, out string buildDate, out string buildYear);
			_Options = new CommandLineOptions(appName, appVersion, buildDate, buildYear, args);

            // Display the usage text special help parameter was specified
             if (_Options.Help.OptionValue == true)
            {
                Console.WriteLine(_Options.UsageMessage);
                Console.ReadLine();
                Environment.Exit(0);
            }

            Logger.LogEmptyLine();
			Logger.LogInfo(new string('-', 60));
			Logger.LogInfo($"{Messages.Msg_Application}: {_Options.ApplicationName} V{_Options.ApplicationVersion}");
			Logger.LogEmptyLine();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// This is the main entry point
		/// </summary>
		public void Run()
		{
            if (_Options.SolutionWizard.OptionValue == true)
            {
                ProcessSolutionWizard();
            }
            else if (_Options.CustomizationWizard.OptionValue == true)
            {
                ProcessCustomizationWizard();
            }

            Logger.LogInfo("Program run completed.");
		}
        #endregion

        #region Private Methods

        /// <summary>
        /// Main processing routine for the Solution Wizard Web.vstemplate file
        /// </summary>
        private void ProcessSolutionWizard()
        {
            var methodName = "ProcessSolutionWizard";

            Logger.LogInfo($"Executing {methodName}()");
            Logger.IndentLevel = Logger.DefaultIndentLevel;

            // Ensure that paths are specified and correct.
            var sdkRoot = _Options.SDKRootFolder.OptionValue;
            var templatesFolder = Path.Combine(sdkRoot, WebTemplateGenerator.Constants.TemplateFolder_SolutionWizard);
            var webTargetFolder = Path.Combine(templatesFolder, WebTemplateGenerator.Constants.TemplateFolder_Web);
            var webSourceFolder = (_Options.WebSourceFolder.OptionValue.Length > 0) ? _Options.WebSourceFolder.OptionValue : string.Empty;

            // Start a timer
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Ensure that paths are specified and correct.
            Logger.LogInfo($"templatesFolder = {templatesFolder}");
            Logger.LogInfo($"webTargetFolder = {webTargetFolder}");

            // Step 1 - Remove the existing Web.vstemplate file
            // Or perhaps back it up instead?
            DeleteExistingTemplateFileIfExists(webTargetFolder);

            // TODO - This needs to be tested
            // Step 2 - Enumerate the source folder
            // and generate the XML file
            new WebTemplateGenerator(Logger, WebTemplateGenerator.WizardType.SolutionWizard)
            {
                WebTemplateFileName = WebTemplateGenerator.Constants.TemplateFileName,
                RootFolder = webTargetFolder,
                TargetFolder = webTargetFolder
            }.SaveXml();

            // Get the elapsed run time
            sw.Stop();
            var ts = sw.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Logger.ResetIndentLevel();
            Logger.LogInfo($"{methodName}() completed - Elapsed Time : {elapsedTime}");
        }

        /// <summary>
        /// Main processing routine for the Customization Wizard Web.vstemplate file
        /// </summary>
        private void ProcessCustomizationWizard()
        {
            var methodName = "ProcessCustomizationWizard";

            Logger.LogInfo($"Executing {methodName}()");
            Logger.IndentLevel = Logger.DefaultIndentLevel;

            // Ensure that paths are specified and correct.
            var sdkRoot = _Options.SDKRootFolder.OptionValue;
            var templatesFolder = Path.Combine(sdkRoot, WebTemplateGenerator.Constants.TemplateFolder_CustomizationWizard);
            var webTargetFolder = Path.Combine(templatesFolder, WebTemplateGenerator.Constants.TemplateFolder_Web);

            // Start a timer
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Ensure that paths are specified and correct.
            Logger.LogInfo($"templatesFolder = {templatesFolder}");
            Logger.LogInfo($"webTargetFolder = {webTargetFolder}");

            // Step 1 - Remove the existing Web.vstemplate file
            // Or perhaps back it up instead?
            DeleteExistingTemplateFileIfExists(webTargetFolder);

            // Step 2 - Enumerate the source folder
            // and generate the XML file
            new WebTemplateGenerator(Logger, WebTemplateGenerator.WizardType.CustomizationWizard)
            {
                WebTemplateFileName = WebTemplateGenerator.Constants.TemplateFileName,
                RootFolder = webTargetFolder,
                TargetFolder = webTargetFolder
            }.SaveXml();

            // Get the elapsed run time
            sw.Stop();
            var ts = sw.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Logger.ResetIndentLevel();
            Logger.LogInfo($"{methodName}() completed - Elapsed Time : {elapsedTime}");
        }

        /// <summary>
        /// Remove the existing Web.vstemplate file
        /// </summary>
        /// <param name="webTargetFolder"></param>
        private void DeleteExistingTemplateFileIfExists(string webTargetFolder)
        {
            var templateFilePath = Path.Combine(webTargetFolder, WebTemplateGenerator.Constants.TemplateFileName);
            if (File.Exists(templateFilePath))
            {
                Logger.LogInfo($"Existing template file '{templateFilePath}' deleted.");
                File.Delete(templateFilePath);
            }
        }

  //      /// <summary>
  //      /// This method will do the following:\
  //      /// 1. Conditionally clear out the SDK\src\wizards\templates\web\ folder
  //      /// 2. Conditionally copy assets from either of the following locations:
  //      ///		a. CNA2\Columbus-Web\Sage.CA.SBS.ERP.Sage300.Web\ 
  //      ///		b. The local Sage 300 online\Web\ 
  //      ///		c. Another folder
  //      ///	   into the following folder
  //      ///	     SDK\src\wizards\templates\web\
  //      ///	3. Create zip archives for the following:
  //      ///		a. BusinessRepository
  //      ///		b. Interfaces
  //      ///		c. Models
  //      ///		d. Resources
  //      ///		e. Services
  //      ///		f. Web
  //      ///		g. Sage300SolutionTemplate
  //      ///	   and store them in the following folder:
  //      ///	     SDK\src\wizards\templates\
  //      /// </summary>
  //      private void RunPrebuildProcess()
		//{
		//	Logger.LogInfo("Executing RunRebuildProcess()");
		//	Logger.IndentLevel = Logger.DefaultIndentLevel;

		//	var proceed = true;

		//	// Start a timer
		//	Stopwatch sw = new Stopwatch();
		//	sw.Start();

		//	// Ensure that paths are specified and correct.
		//	var sdkRoot = _Options.SDKRootFolder.OptionValue;
		//	var templatesFolder = Path.Combine(sdkRoot, @"src\wizards\Templates");
		//	var webTargetFolder = Path.Combine(templatesFolder, @"Web");
		//	var webSourceFolder = (_Options.WebSourceFolder.OptionValue.Length > 0) ? _Options.WebSourceFolder.OptionValue : string.Empty;

		//	if (_Options.EnableTemplateUpdates.OptionValue == true)
		//	{
		//		// If WebSourceFolder was not specified on the command-line 
		//		if (webSourceFolder.Length == 0)
		//		{
  //                  Logger.LogInfo("Attempting to use the local Sage 300 installation for web source files");
  //                  webSourceFolder = Utilities.Sage300CWebFolder;

		//			// Determine if Sage 300 is installed locally
		//			if (webSourceFolder.Length > 0)
		//			{
		//				Logger.LogInfo($"Sage 300 web screen installation found in registry: '{webSourceFolder}'");
		//			}
		//			else
		//			{
  //                      var msg = @"Sage 300 web screen installation not found in registry. You will need to either install Sage 300
  //                                  or specify another folder on the command-line.";
  //                      Logger.LogError(msg);
		//				proceed = false;
		//			}
		//		}
		//		else
		//		{
  //                  // Does the specified folder exist?
  //                  if (Directory.Exists(webSourceFolder) == false)
  //                  {
  //                      Logger.LogInfo($"The specified folder '{webSourceFolder}' does not exist. Please specify a different folder.");
  //                  }
  //                  else
  //                  {
  //                      Logger.LogInfo($"Using the following folder for sources : '{webSourceFolder}'.");
  //                  }
  //              }

		//		if (webSourceFolder.Length == 0)
		//		{
		//			Logger.LogError($"Unable to determine where web source files should come from.");
		//			Logger.LogError($"Did you specify the correct command-line parameters?");
		//			proceed = false;
		//		}
		//	}
		//	else
		//	{
		//		Logger.LogInfo($"EnableTemplateUpdates was turned off on the command-line. The Web templates folder will NOT be automatically updated.");
		//	}

		//	if (proceed)
		//	{
  //              // Only allow Web template folder updates when this flag is false
  //              if (_Options.EnableTemplateUpdates.OptionValue == true)
  //              {
  //                  UpdateWebFiles(webSourceFolder, webTargetFolder);
  //              }

		//		//// Optionally rebuild the Web.vstemplate file BEFORE
		//		//// we create the Web.zip (and other zip archives)
		//		//if (_Options.RebuildWebDotVstemplateFile.OptionValue == true)
		//		//{
		//			// RebuildWebDotVstemplate();
		//		//}

		//		// Create the template zip files
		//		CreateTemplateZipFiles();

		//		// Note: Applies to the Sage300UIWizardPackage Project Only
		//		// Copy newly created template Zip archives to the 'Project Templates' folder 
		//		CopyTemplateZipFiles();

		//		// Now, remove the Zip archives from the \src\Wizards\Templates\ folder.
		//		RemoveTemplateZipFiles();
		//	}

		//	// Get the elapsed run time
		//	sw.Stop();
		//	var ts = sw.Elapsed;
		//	string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

		//	Logger.ResetIndentLevel();
		//	Logger.LogInfo($"RunRebuildProcess() completed - Elapsed Time : {elapsedTime}");
		//}

        /// <summary>
        /// A wrapper method for deleting web files and copying over files from a different folder
        /// </summary>
        /// <param name="sourceFolder">The source folder for Web files</param>
        /// <param name="targetFolder">The target folder for the Web files</param>
        private void UpdateWebFiles(string sourceFolder, string targetFolder)
        {
            // Remove specific files and folders from the 'Web' directory
            DeleteWebFiles(targetFolder);

            // Copy over the 'Web' files
            CopyWebSources(sourceFolder, targetFolder);
        }

        /// <summary>
        /// Remove specific items from the '\src\Wizards\Templates\Web\' folder
        /// </summary>
        /// <param name="folder">The folder to remove files and sub-folders from</param>
        private void DeleteWebFiles(string webFolder)
		{
			Logger.LogInfo($"Start - DeleteWebFiles('{webFolder}')");

			List<string> lockedItems = new List<string>();

			var webSubPaths = new[] {
					@"Areas\Shared",
					@"Areas\Core",
					@"Views",
					@"Scripts",
					@"Content",
					@"Assets",
					@"Customization",
					@"WebForms",
			};

			foreach (var subPath in webSubPaths)
			{
				try
				{
					var targetFolder = Path.Combine(webFolder, subPath);
					Logger.LogInfo($"Processing '{targetFolder}' folder.");

					if (!Directory.Exists(targetFolder))
					{
						Logger.LogInfo($"'{targetFolder}' does not exist.");
					}
					else
					{
						var folder = string.Empty;
						try
						{
							// Remove all the files in this folder
							var files = Directory.GetFiles(targetFolder);
							foreach (var file in files)
							{
								File.Delete(file);
								Logger.LogInfo($"File '{file}' deleted.");
							}

							// Remove all sub-folders of this folder
							var folders = Directory.GetDirectories(targetFolder);
							foreach (string f in folders)
							{
								// Save folder name just in case an exception is thrown.
								folder = f;
								Directory.Delete(f, recursive: true);
								Logger.LogInfo($"Folder '{f}' deleted.");
							}
						}
						catch (IOException e)
						{
							// Get the filename from the exception message.
							var filename = e.Message.Replace("The process cannot access the file '", "")
													.Replace("' because it is being used by another process.", "");
							lockedItems.Add(Path.Combine(folder, filename));
						}
					}
				}
				catch (ArgumentException e)
				{
					// Possible issue with formatting of folder
					var message = e.Message;
					lockedItems.Add(message);
				}

			}

			// Were there any locked items?  They may have already been deleted.
			if (lockedItems.Count > 0)
			{
				Logger.LogWarning($"Possible locked items:");
				foreach (var item in lockedItems)
				{
					Logger.LogWarning($"{item}");
				}
			}

			Logger.LogInfo($"End - DeleteWebFiles");
		}

		/// <summary>
		/// Copy specific files from the source folder to destination folder.
		/// Also includes a step that alters some files.
		/// </summary>
		/// <param name="source">The web source directory</param>
		/// <param name="target">The web destination directory</param>
		private void CopyWebSources(string source, string target)
		{
			Logger.LogInfo($"Start - CopyWebSources('{source}', '{target}')");

			string arguments = string.Empty;
			string sourceFolder = string.Empty;
			string targetFolder = string.Empty;

			#region Step 1 - Areas\Core, Areas\Shared, Views, Content, Assets
			Logger.LogInfo(@"Step 1 - Areas\Core, Areas\Shared, Views, Content, Assets");

			var webSubPaths = new[] {
					@"Areas\Core",
					@"Areas\Shared",
					@"Views",
					@"Content",
					@"Assets",
			};

			var excludeFileTypes = new[] {
						"*.cs",
						"*.csproj",
						"*.user",
						"*.xml",
						"*.Sage300.Revaluation*.js",
						"*.IC.Common.js",
						"_wizard.cshtml",
						"packages.config",
			};
			var excludedFiles = string.Join(" ", excludeFileTypes);

			var excludeFolderTypes = new[] {
						"TU",
						"obj",
			};
			var excludedFolders = string.Join(" ", excludeFolderTypes);

			/*
			 * ROBOCOPY command line example
			 * 
			 * 
			 * robocopy /S sourceDir targetDir 
			 * /xf *.cs *.csproj *.user *.xml ... and so on
			 * /xd TU obj ... and so on
			 * 
			 * 
			*/

			foreach (var subPath in webSubPaths)
			{
				sourceFolder = Path.Combine(source, subPath);
				targetFolder = Path.Combine(target, subPath);
				arguments = $"/S \"{sourceFolder}\" \"{targetFolder}\" *.* /xf {excludedFiles} /xd {excludedFolders} ";
				Robocopy(arguments);
			}
			#endregion

			#region Step 2 - Customization
			Logger.LogInfo(@"Step 2 - Customization");
			sourceFolder = Path.Combine(source, "Customization");
			targetFolder = Path.Combine(target, "Customization");
			arguments = $"/S \"{sourceFolder}\" \"{targetFolder}\" ";
			Robocopy(arguments);
			#endregion

			#region Step 3 - Web\Scripts (with exclusions)
			Logger.LogInfo(@"Step 3 - Web\Scripts (with exclusions)");
			sourceFolder = Path.Combine(source, "Scripts");
			targetFolder = Path.Combine(target, "Scripts");
			arguments = $"/S \"{sourceFolder}\" \"{targetFolder}\" /xf kendo.all*.js Test_*.js *TestUtils.js chutzpah.json";
			Robocopy(arguments);
			#endregion

			#region Step 4 - WebForms
			Logger.LogInfo(@"Step 4 - WebForms");
			sourceFolder = Path.Combine(source, "WebForms");
			targetFolder = Path.Combine(target, "WebForms");
			arguments = $"/E \"{sourceFolder}\" \"{targetFolder}\" ";
			Robocopy(arguments);
			#endregion

			#region Step 5 - Edit some Namespaces
			Logger.LogInfo(@"Step 5 - Edit some Namespaces");
			EditNamespaces(Path.Combine(target, "WebForms"));
			#endregion

			#region Step 6 - Remove some empty folders
			Logger.LogInfo(@"Step 6 - Remove some empty folders");
			new FileUtilities(Logger).RemoveEmptyDirectories(target);
			#endregion

			#region Step 7 - Remove some extra files
			Logger.LogInfo(@"Step 7 - Remove some extra files");
			var targetFile = Path.Combine(target, @"Areas\Core\web.config");
			File.Delete(targetFile);

			targetFolder = Path.Combine(target, @"Areas\Shared\Models");
			if (Directory.Exists(targetFolder))
			{
				Directory.Delete(targetFolder, recursive: true);
			}
			#endregion

			Logger.LogInfo($"End - CopyWebSources");
		}

		/// <summary>
		/// Rebuild the Web.vstemplate file based on the current contents of the 
		/// \src\Wizards\templates\Web\ folder.
		/// </summary>
		private void RebuildWebDotVstemplate()
		{
			Logger.LogInfo($"Start - RebuildWebDotVstemplate()");

			const string TemplateFileName = "Web.vstemplate";

			// Ensure that paths are specified and correct.
			var templatesFolder = Path.Combine(_Options.SDKRootFolder.OptionValue, @"src\wizards\Templates");
			Logger.LogInfo($"templatesFolder = {templatesFolder}");
			var webFolder = Path.Combine(templatesFolder, "Web");
			Logger.LogInfo($"webFolder = {webFolder}");

			// Step 1 - Remove the existing Web.vstemplate file
			// Or perhaps back it up instead?
			var templateFilePath = Path.Combine(webFolder, TemplateFileName);
			if (File.Exists(templateFilePath))
			{
				Logger.LogInfo($"Existing template file '{templateFilePath}' deleted.");
				File.Delete(templateFilePath);
			}

			// Step 2 - Enumerate the source folder
			// and generate the XML file
			new WebTemplateGenerator(Logger)
			{
				WebTemplateFileName = "Web.vstemplate",
				RootFolder = webFolder,
				TargetFolder = webFolder
			}.SaveXml();

			Logger.LogInfo($"End - RebuildWebDotVstemplate");
		}

        /// <summary>
        /// Create the list of Zip archives from the sub-folders
        /// in the Web SDK src\Wizards\Templates folder
        /// </summary>
        private void CreateTemplateZipFiles()
		{
			Logger.LogInfo($"Start - CreateTemplateZipFiles()");

			// Ensure that paths are specified and correct.
			var templatesFolder = Path.Combine(_Options.SDKRootFolder.OptionValue, @"src\wizards\Templates");
			Logger.LogInfo($"templatesFolder = {templatesFolder}");

			// The list of folders to compress into 
			// individual zip archives
			var templateSubPaths = new[] {
					@"BusinessRepository",
					@"Interfaces",
					@"Models",
					@"Resources",
					@"Sage300SolutionTemplate",
					@"Services",
					@"Web"
			};

			var fullSourcePath = string.Empty;
			foreach (var path in templateSubPaths)
			{
				// Build the destination zip archive name
				var zipName = $"{path}.zip";
				zipName = Path.Combine(templatesFolder, zipName);

				// Delete the existing archive if it exists
				if (File.Exists(zipName))
				{
					Logger.LogInfo($"Existing Zip archive '{zipName}' deleted.");
					File.Delete(zipName);
				}

				// Create the new archive
				fullSourcePath = Path.Combine(templatesFolder, path);
				ZipFile.CreateFromDirectory(fullSourcePath, zipName);
				Logger.LogInfo($"Zip archive '{zipName}' created.");
			}

			Logger.LogInfo($"End - CreateTemplateZipFiles");
		}

		/// <summary>
		/// Copy the Zip archives from the \src\Wizards\Templates\ folder
		/// to \src\Wizards\Sage300UIWizardPackage\ProjectTemplates\ folder
		/// </summary>
		private void CopyTemplateZipFiles()
		{
			Logger.LogInfo($"Start - CopyTemplateZipFiles()");

			// Ensure that paths are specified and correct.
			var templatesFolder = Path.Combine(_Options.SDKRootFolder.OptionValue, @"src\wizards\Templates");
			var targetFolder = Path.Combine(_Options.SDKRootFolder.OptionValue, @"src\wizards\Sage300UIWizardPackage\ProjectTemplates");

			// The list of template files
			var templateNames = new[] {
					@"BusinessRepository.zip",
					@"Interfaces.zip",
					@"Models.zip",
					@"Resources.zip",
					@"Sage300SolutionTemplate.zip",
					@"Services.zip",
					@"Web.zip"
			};

			var fullSourcePath = string.Empty;
			foreach (var filename in templateNames)
			{
				// Build the destination zip archive name
				var targetFile = Path.Combine(targetFolder, filename);

				// Delete the existing archive if it exists
				if (File.Exists(targetFile))
				{
					Logger.LogInfo($"Existing Zip archive '{targetFile}' deleted.");
					File.Delete(targetFile);
				}

				var arguments = $"/S \"{templatesFolder}\" \"{targetFolder}\" {filename} ";
				Robocopy(arguments);
				Logger.LogInfo($"Zip archive '{filename}' copied from {templatesFolder} to {targetFolder}.");
			}

			Logger.LogInfo($"End - CopyTemplateZipFiles");
		}

		/// <summary>
		/// Remove the Zip archives from the \src\Wizards\Templates\ folder
		/// </summary>
		private void RemoveTemplateZipFiles()
		{
			Logger.LogInfo($"Start - RemoveTemplateZipFiles()");

			// Ensure that paths are specified and correct.
			var templatesFolder = Path.Combine(_Options.SDKRootFolder.OptionValue, @"src\wizards\Templates");

			// The list of template files
			var templateNames = new[] {
					@"BusinessRepository.zip",
					@"Interfaces.zip",
					@"Models.zip",
					@"Resources.zip",
					@"Sage300SolutionTemplate.zip",
					@"Services.zip",
					@"Web.zip"
			};

			var fullSourcePath = string.Empty;
			foreach (var filename in templateNames)
			{
				// Build the full path to the Zip archive file
				var targetFile = Path.Combine(templatesFolder, filename);

				// Delete the existing archive if it exists
				if (File.Exists(targetFile))
				{
					Logger.LogInfo($"Existing Zip archive '{targetFile}' deleted.");
					File.Delete(targetFile);
				}
			}

			Logger.LogInfo($"End - RemoveTemplateZipFiles");
		}

		/// <summary>
		/// Run Robocopy with command-line arguments
		/// </summary>
		/// <param name="arguments">Robocopy command-line arguments</param>
		private void Robocopy(string arguments)
		{
			Logger.LogInfo($"Calling robocopy {arguments}");

			using (var p = new Process())
			{
				p.StartInfo.Arguments = "/C robocopy " + arguments;
				p.StartInfo.FileName = "CMD.EXE";
				p.StartInfo.CreateNoWindow = true;
				p.StartInfo.UseShellExecute = false;
				p.Start();
				p.WaitForExit();
			}
		}

		/// <summary>
		/// Do some file content search and replace
		/// </summary>
		/// <param name="targetFolder">The folder to search in.</param>
		private void EditNamespaces(string targetFolder)
		{
			var files = Directory.GetFiles(targetFolder);
			foreach (var file in files)
			{
				string text = File.ReadAllText(file);
				text = text.Replace("Inherits=\"Sage.CA.SBS.ERP.Sage300.Web", "Inherits=\"$companynamespace$.$applicationid$.Web");
				text = text.Replace("namespace Sage.CA.SBS.ERP.Sage300.Web.WebForms", "namespace $companynamespace$.$applicationid$.Web.WebForms");
				text = text.Replace(" Common.Models.", " Sage.CA.SBS.ERP.Sage300.Common.Models.");
				text = text.Replace("(Common.Models.", "(Sage.CA.SBS.ERP.Sage300.Common.Models.");
				text = text.Replace("<Common.Models.", "<Sage.CA.SBS.ERP.Sage300.Common.Models.");
				File.WriteAllText(file, text);
			}
		}
		#endregion
	}
}
