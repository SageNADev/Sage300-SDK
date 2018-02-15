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
using MergeISVProject.Constants;
using MergeISVProject.CustomExceptions;
using MergeISVProject.Interfaces;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
#endregion

namespace MergeISVProject
{
	public class MergeISVProjectDriver
	{
		#region Constants

		const string BUILD_PROFILE_RELEASE = @"release";
		const bool OVERWRITE = true;

		#endregion

		#region Private Variables

		private static string[] Languages = { "es", "fr", "zh-Hans", "zh-Hant" };

		private ICommandLineOptions _Options = null;
		private ILogger _Logger = null;
		private FolderManager _Folders = null;
		private string _Sage300Path = string.Empty;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">Command-line options instance</param>
		/// <param name="logger">Logger instance</param>
		public MergeISVProjectDriver(ICommandLineOptions options, ILogger logger)
		{
			_Options = options;
			_Logger = logger;
			_Sage300Path = GetSage300Path();
			_Folders = new FolderManager(logger, 
										 _Options.WebProjectPath.OptionValue, 
										 _Sage300Path, 
										 _Options.ModuleId);
			_Logger.Log(_Folders.GenerateLogOutput());
			VerifyCorrectBuildProfileSpecified();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Ensure the user provided the correct build profile to use
		/// Throws an exception if wrong profile specified
		/// </summary>
		private void VerifyCorrectBuildProfileSpecified()
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			try
			{
				if (_Options.BuildProfile.OptionValue.ToLower() != BUILD_PROFILE_RELEASE.ToLower())
				{
					throw new MergeISVProjectException(_Logger, Messages.Error_InvalidBuildProfile);
				}
				else
				{
					_Logger.Log($"Valid build profile '{_Options.BuildProfile.OptionValue}' specified.");
				}
			}
			finally
			{
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}
		}

		/// <summary>
		/// Compile the views and copy compiled files to web project deploy (staging) folder.
		/// Some assets are copied to the Final deployment folder instead, as noted
		/// in their method name.
		/// </summary>
		/// <param name="pathWebProj">Web Project path</param>
		/// <param name="menuFileName">Menu file name</param>
		/// <param name="pathFramework">Framework path</param>
		private bool StageFiles(string pathWebProj, string menuFileName, string pathFramework)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var moduleId = _Options.ModuleId;

			StageBin(_Folders, moduleId);
			StageBootstrapperToFinal(_Folders, moduleId);	
			StageMenuDetailsToFinal(_Folders, menuFileName); 
			StageAreas(_Folders);

			CompileStagedViews(pathFramework, _Folders);
			MinifyScripts(_Folders);
			CopyCompiledAssetsToFinal(_Folders);

			StageResourceSatelliteFilesToFinal(_Folders, moduleId);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());

			return true;
		}

		/// <summary>
		/// Minify the javascript files
		/// </summary>
		/// <param name="folders">The Folder Manager</param>
		private void MinifyScripts(FolderManager folders)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			try
			{
				if (_Options.Minify.OptionValue)
				{
					var minifier = new SageISVMinifier(_Logger, folders, _Options.ModuleId);
					minifier.MinifyJavascriptFilesAndCleanup();
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}
		}

		/// <summary>
		/// Copy compiled assets (Views and Minified scripts)
		/// to the Final deployment folder
		/// </summary>
		/// <param name="folders">The Folder Manager</param>
		private void CopyCompiledAssetsToFinal(FolderManager folders)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var source = folders.Compiled.Root;
			var dest = folders.Final.Root;
			FileSystem.CopyDirectory(source, dest);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy the Areas folder (and all subfolders)
		/// Source:      ...XX.Web/Areas/* 
		/// Destination: ...XX.Web/__Deploy/Areas/
		/// </summary>
		/// <param name="folders">The Folder Manager</param>
		private void StageAreas(FolderManager folders)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			const string WebConfigName = @"Web.config";
			var source = folders.RootSource;
			var destination = folders.Staging.Areas;

			// Copy original Web.config to it's staging location
			var sourceWebConfig = Path.Combine(source, WebConfigName);
			var destWebConfig = Path.Combine(destination, WebConfigName);
			File.Copy(sourceWebConfig, destWebConfig);

			// Areas/XX/Views
			var d1 = folders.Originals.AreasViews;
			var d2 = folders.Staging.AreasViews;
			FileSystem.CopyDirectory(d1, d2);
			_Logger.Log($"Copying directory...");
			_Logger.Log($"Source: {d1}");
			_Logger.Log($"Destination: {d2}");

			// Areas/XX/Scripts
			d1 = folders.Originals.AreasScripts;
			d2 = folders.Staging.AreasScripts;
			FileSystem.CopyDirectory(d1, d2);
			_Logger.Log($"Copying directory...");
			_Logger.Log($"Source: {d1}");
			_Logger.Log($"Destination: {d2}");

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy a subset of files from the original Web project bin folder
		/// to the Deploy/Staging/bin folder
		/// </summary>
		/// <param name="folders">The Folder Manager</param>
		/// <param name="moduleId">The module Id</param>
		private void StageBin(FolderManager folders, string moduleId)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			string[] patterns = {
				"System.*.dll",
				"Sage.CA.SBS.ERP.*.dll",
				"*.Web.Infrastructure.dll",
				"*.Web.dll",
				"*." + moduleId + ".*.dll"
			};

			foreach (var pattern in patterns)
			{
				CopyFiles(false, folders.Originals.Bin, pattern, folders.Staging.Bin, true);
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Compile the Razor views
		/// </summary>
		/// <param name="pathFramework">Path to .NET Framework</param>
		/// <param name="folders">The folder manager</param>
		private void CompileStagedViews(string pathFramework, FolderManager folders)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var p = new Process
			{
				StartInfo =
				{
					UseShellExecute = false,
					FileName = Path.Combine(pathFramework, "aspnet_compiler.exe"),
					Arguments = $" -v / -p \"{folders.Staging.Root}\" -fixednames \"{folders.Compiled.Root}\""
				}
			};
			p.Start();
			p.WaitForExit();

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy Module bootstrapper file from original folder to final deployment folder
		/// </summary>
		/// <param name="folders"></param>
		/// <param name="moduleId"></param>
		private void StageBootstrapperToFinal(FolderManager folders, string moduleId)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var bootstrapFileName = $"{moduleId}bootstrapper.xml";
			var sourceFile = Path.Combine(folders.RootSource, bootstrapFileName);
			var destFile = Path.Combine(folders.Final.Root, bootstrapFileName);
			File.Copy(sourceFile, destFile, overwrite: OVERWRITE);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy XXMenuDetails.xml to Final staging folder
		/// </summary>
		/// <param name="folders"></param>
		/// <param name="menuFilename"></param>
		private void StageMenuDetailsToFinal(FolderManager folders, string menuFilename)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			Directory.CreateDirectory(Path.Combine(folders.Final.Root, 
												   FolderNameConstants.APPDATA));
			Directory.CreateDirectory(Path.Combine(folders.Final.Root, 
												   FolderNameConstants.APPDATA, 
												   FolderNameConstants.MENUDETAIL));

			var sourceFile = Path.Combine(folders.RootSource, menuFilename);
			var destFile = Path.Combine(folders.Final.Root, 
										FolderNameConstants.APPDATA, 
										FolderNameConstants.MENUDETAIL, 
										menuFilename);
			File.Copy(sourceFile, destFile, overwrite: OVERWRITE);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Get Sage 300 installation path
		/// </summary>
		/// <returns>Sage 300 installation path</returns>
		private string GetSage300Path()
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			const string sageRegKey = "SOFTWARE\\ACCPAC International, Inc.\\ACCPAC\\Configuration";
			const string sage64RegKey = "SOFTWARE\\WOW6432Node\\ACCPAC International, Inc.\\ACCPAC\\Configuration";

			var key = Registry.LocalMachine.OpenSubKey(sageRegKey) ?? Registry.LocalMachine.OpenSubKey(sage64RegKey);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());

			return (key == null) ? string.Empty : key.GetValue("Programs").ToString();
		}

		/// <summary>
		/// Copy bootStrapper, menuDetails, scripts, views and resource files to the Sage Online folder
		/// </summary>
		/// <param name="pathWebProj">Web Project path</param>
		/// <param name="pathSage300">Sage 300 folder</param>
		/// <param name="menuFileName">Menu file name</param>
		/// <returns>True if successful otherwise false</returns>
		private void DeployFiles(string pathWebProj, string pathSage300, string menuFileName)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			_Logger.Log(DeployFilesStartupMessage());

			// All files used for deployment will come from the /...Web/Deploy/Build folder
			var sourceRootFolder = _Folders.Final.Root;
			var pathSageOnline = _Folders.Live.Root;
			var pathOnlineWorker = _Folders.Live.Worker;
			var pathOnlineWeb = _Folders.Live.Web;

			if (!Directory.Exists(pathOnlineWeb))
			{
				throw new MergeISVProjectException(_Logger, Messages.Error_Sage300WebFolderMissing);
			}

			DeployBootstrapper(sourceRootFolder, pathOnlineWeb, pathOnlineWorker);
			DeployMenuDetails(sourceRootFolder, pathOnlineWeb, menuFileName);
			DeployAreaScripts(sourceRootFolder, pathOnlineWeb);
			DeployCompiledViews(sourceRootFolder, pathSageOnline);
			DeployBinFolders(sourceRootFolder, pathOnlineWeb, pathOnlineWorker);
			DeployResourceSatelliteFiles(sourceRootFolder, pathSage300);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Create a logging message for the DeployFiles method
		/// Uses the TestDeploy flag to determine whether deployment
		/// will do actual file copy or not.
		/// </summary>
		/// <returns>The string message</returns>
		private string DeployFilesStartupMessage()
		{
			var msg = _Options.TestDeploy.OptionValue ? @"Simulated Deployment Only.  " +
													  "No files will actually be copied to live Sage 300 installion directory."
												   : @"Live Deployment. " +
													  "Files will actually be copied to live Sage 300 installion directory.";
			return msg;
		}

		/// <summary>
		/// Deploy the Bootstrapper.xml files to the Sage 300 installation (Web and Worker)
		/// (Only if TestDeploy is not enabled)
		/// </summary>
		/// <param name="sourceDir">Source directory</param>
		/// <param name="destDirWeb">Destination directory (Web)</param>
		/// <param name="destDirWorker">Destination directory (Worker)</param>
		private void DeployBootstrapper(string sourceDir, string destDirWeb, string destDirWorker)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			const string searchPattern = "*bootstrapper.xml";
			var bootFiles = Directory.GetFiles(sourceDir, searchPattern);
			foreach (var src in bootFiles)
			{
				var filename = Path.GetFileName(src);
				if (filename == null)
				{
					continue;
				}
				var dest = Path.Combine(destDirWeb, filename);
				CopyFile(_Options.TestDeploy.OptionValue, src, dest, OVERWRITE);

				dest = Path.Combine(destDirWorker, filename);
				CopyFile(_Options.TestDeploy.OptionValue, src, dest, OVERWRITE);
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the XXMenuDetails.xml files to the Sage 300 installation
		/// </summary>
		/// <param name="sourceDir">Source directory</param>
		/// <param name="destDir">Destination directory</param>
		/// <param name="menuName">The menu filename</param>
		private void DeployMenuDetails(string sourceDir, string destDir, string menuName)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			// Copy menu file to App_Data menuDetails and all sub directories
			var pathMenuFrom = Path.Combine(sourceDir, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL, menuName);
			var pathMenuDir = Path.Combine(destDir, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL);
			var pathMenuTo = Path.Combine(pathMenuDir, menuName);

			if (Directory.Exists(pathMenuDir))
			{
				CopyFile(_Options.TestDeploy.OptionValue, pathMenuFrom, pathMenuTo, OVERWRITE);
				foreach (var dir in Directory.GetDirectories(pathMenuDir))
				{
					var pathMenuSubTo = Path.Combine(dir, menuName);
					CopyFile(_Options.TestDeploy.OptionValue, pathMenuFrom, pathMenuSubTo, OVERWRITE);
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the Area scripts to the Sage 300 installation
		/// </summary>
		/// <param name="sourceDir">Source directory</param>
		/// <param name="destDir">Destination directory</param>
		private void DeployAreaScripts(string sourceDir, string destDir)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var moduleId = _Options.ModuleId;

			var pathScripts = Path.Combine(sourceDir, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);
			if (Directory.Exists(pathScripts))
			{
				foreach (var sourceFolder in Directory.GetDirectories(pathScripts))
				{
					var paths = sourceFolder.Split('\\');
					var moduleName = paths[paths.Length - 1];
					var pathTargetSubArea = Path.Combine(destDir, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);
					var pathToScripts = Path.Combine(pathTargetSubArea, moduleName);
					CopyDirectory(_Options.TestDeploy.OptionValue, sourceFolder, pathToScripts, OVERWRITE);
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the compiled view files to the Sage 300 installation
		/// </summary>
		/// <param name="sourceDir">Source root directory</param>
		/// <param name="destDir">Destination root directory</param>
		private void DeployCompiledViews(string sourceDir, string destDir)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			// Copy compiled files from deploy folder to sage online web area and bin directory
			var pathBuildView = Path.Combine(sourceDir, FolderNameConstants.AREAS, 
														_Options.ModuleId, 
														FolderNameConstants.VIEWS);
			var pathSageView = Path.Combine(destDir, FolderNameConstants.WEB, 
													 FolderNameConstants.AREAS, 
													 _Options.ModuleId, 
													 FolderNameConstants.VIEWS);

			// Do not copy IF compile was not successful (determined by existence of folder)
			if (!Directory.Exists(pathBuildView))
			{
				throw new MergeISVProjectException(_Logger, Messages.Error_CouldNotCompileRazorViews);
			}

			CopyDirectory(_Options.TestDeploy.OptionValue, pathBuildView, pathSageView, OVERWRITE);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the binary assets to the Sage 300 installation (Web and Worker)
		/// </summary>
		/// <param name="sourceDir"></param>
		/// <param name="destDirWeb"></param>
		/// <param name="destDirWorker"></param>
		private void DeployBinFolders(string sourceDir, string destDirWeb, string destDirWorker)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var simulateCopy = _Options.TestDeploy.OptionValue;
			var pathBuildBin = Path.Combine(sourceDir, FolderNameConstants.BIN);
			var pathSageBin = Path.Combine(destDirWeb, FolderNameConstants.BIN);

			string[] webFiles = { "*.compiled", "App_Web_*.dll", "*.Web.dll", "*." + _Options.ModuleId + ".*.dll" };
			foreach (var pattern in webFiles)
			{
				CopyFiles(simulateCopy, pathBuildBin, pattern, pathSageBin, OVERWRITE);
			}

			string[] workerFiles = { "*." + _Options.ModuleId + ".*.dll" };
			foreach (var pattern in workerFiles)
			{
				CopyFiles(simulateCopy, pathBuildBin, pattern, destDirWorker, OVERWRITE, copyWebFile: false);
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy resource satellite dlls to web bin and worker folder
		/// </summary>
		/// <param name="soureDir">Source directory</param>
		/// <param name="destDir">Sage 300 folder</param>
		private void DeployResourceSatelliteFiles(string sourceDir, string destDir)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var pathBinFrom = Path.Combine(sourceDir, FolderNameConstants.BIN);
			var pathWebBinTo = Path.Combine(destDir, FolderNameConstants.ONLINE, 
													 FolderNameConstants.WEB, 
													 FolderNameConstants.BIN);
			var pathWorkerTo = Path.Combine(destDir, FolderNameConstants.ONLINE, 
													 FolderNameConstants.WORKER);
			var pattern = "*." + _Options.ModuleId + ".*.dll";

			foreach (var language in Languages)
			{
				var fromFolder = Path.Combine(pathBinFrom, language);
				var toWebFolder = Path.Combine(pathWebBinTo, language);
				var toWorkerFolder = Path.Combine(pathWorkerTo, language);

				// Does the source folder exist? 
				// Proceed if it does.
				if (Directory.Exists(fromFolder))
				{
					foreach (var file in Directory.GetFiles(fromFolder, pattern))
					{
						var fileName = Path.GetFileName(file);
						if (!string.IsNullOrEmpty(fileName))
						{
							var pathFile = Path.Combine(toWebFolder, fileName);
							CopyFile(_Options.TestDeploy.OptionValue, file, pathFile, OVERWRITE);

							pathFile = Path.Combine(toWorkerFolder, fileName);
							CopyFile(_Options.TestDeploy.OptionValue, file, pathFile, OVERWRITE);
						}
					}
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Staging - Copy resource satellite dlls to /Deploy/Final/bin/[LANGUAGE] folders
		/// </summary>
		/// <param name="pathFrom">source file path</param>
		/// <param name="pathTo">destination file path (Deploy/Final/bin/[LANGUAGE])</param>
		/// <param name="moduleId">module id</param>
		private void StageResourceSatelliteFilesToFinal(FolderManager folders, string moduleId)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var pathBinFrom = folders.Originals.Bin;
			var pathWebBinTo = folders.Final.Bin;

			var pattern = $"*.{moduleId}.*.dll";

			foreach (var language in Languages)
			{
				var fromFolder = Path.Combine(pathBinFrom, language);
				var toWebFolder = Path.Combine(pathWebBinTo, language);

				foreach (var file in Directory.GetFiles(fromFolder, pattern))
				{
					var fileName = Path.GetFileName(file);
					if (!string.IsNullOrEmpty(fileName))
					{
						// Ensure that the destination directory exists
						Directory.CreateDirectory(toWebFolder);
						var destinationFile = Path.Combine(toWebFolder, fileName);
						CopyFile(testOnly: false, source: file, dest: destinationFile, flag: OVERWRITE);
					}
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy files from one folder to another.
		/// </summary>
		/// <param name="pathFrom">Source path</param>
		/// <param name="pattern">Copy pattern</param>
		/// <param name="pathTo">Destination path</param>
		/// <param name="overwrite">True to overwrite otherwise false</param>
		/// <param name="copyWebFile">True to copy *.web.dll otherwise false</param>
		private void CopyFiles(bool testOnly, string pathFrom, string pattern, string pathTo, bool overwrite, bool copyWebFile = true)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			_Logger.Log($"Source Folder: {pathFrom}");
			_Logger.Log($"Destination Folder: {pathTo}");

			foreach (var file in Directory.GetFiles(pathFrom, pattern))
			{
				var fileName = Path.GetFileName(file);
				if (fileName == null ||
					fileName.Equals("CrystalDecisions.Web.dll") ||
					(fileName.EndsWith("Web.dll") && !copyWebFile))
				{
					continue;
				}
				var pathFile = Path.Combine(pathTo, fileName);
				CopyFile(testOnly, file, pathFile, overwrite);
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Wrapper function for calling File.Copy
		/// Adds ability to just simulate the copy process.
		/// </summary>
		/// <param name="testOnly"></param>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="flag"></param>
		private void CopyFile(bool testOnly, string source, string dest, bool flag)
		{
			try
			{
				var simulationText = testOnly ? "[Simulation] " : "";
				var sourceFilenameOnly = new FileInfo(source).Name;

				_Logger.Log($"{simulationText}Copying file {sourceFilenameOnly}.");
				if (!testOnly) { File.Copy(source, dest, flag); }
			}
			catch (IOException e)
			{
				var msg = e.Message;
				throw new MergeISVProjectException(_Logger, msg);
			}
		}

		/// <summary>
		/// Wrapper function for calling FileSystem.CopyDirectory
		/// Adds ability to just simulate the copy process.
		/// </summary>
		/// <param name="testOnly"></param>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="flag"></param>
		private void CopyDirectory(bool testOnly, string source, string dest, bool overwrite)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var simulationText = testOnly ? "[Simulation] " : "";
			if (testOnly)
			{
				_Logger.Log("Simulation Only");
			}
			_Logger.Log($"Source Directory: {source}");
			_Logger.Log($"Destination Directory: {dest}");
			if (!testOnly)
			{
				FileSystem.CopyDirectory(source, dest, overwrite);
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		#endregion // Private Methods

		#region Public Methods

		/// <summary>
		/// The main processor!
		/// </summary>
		public void Run()
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			try
			{
				if (StageFiles(_Options.WebProjectPath.OptionValue,
							   _Options.MenuFilename.OptionValue,
							   _Options.DotNetFrameworkPath.OptionValue))
				{
					if (!_Options.NoDeploy.OptionValue)
					{
						_Logger.Log("Calling DeployFiles(...)");

						DeployFiles(_Options.WebProjectPath.OptionValue,
									_Sage300Path,
									_Options.MenuFilename.OptionValue);

						_Logger.Log("Files have been deployed to local Sage 300 installation.");
					}
					else
					{
						_Logger.Log("Deployment to Sage 300 installation disabled.");
					}
				}
			}
			finally
			{
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}
		}

		#endregion
	}
}
