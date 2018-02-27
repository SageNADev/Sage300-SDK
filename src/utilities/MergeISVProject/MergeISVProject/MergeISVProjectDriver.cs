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
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
#endregion

namespace MergeISVProject
{
	/// <summary>
	/// This is the primary driver class for the application
	/// </summary>
	public class MergeISVProjectDriver
	{
		#region Constants
		const string BUILD_PROFILE_RELEASE = @"release";
		const bool OVERWRITE = true;
		#endregion

		#region Enumerations
		private enum AppMode
		{
			FullSolution = 0,
			SingleProject,
		}
		#endregion

		#region Private Variables
		private readonly string[] Languages = { "es", "fr", "zh-Hans", "zh-Hant" };
		private readonly ICommandLineOptions _Options = null;
		private readonly ILogger _Logger = null;
		private readonly FolderManager _FolderManager = null;
		private readonly string _Sage300Path = string.Empty;
		#endregion

		#region Constructor(s)
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">An instance of the CommandLineOptions object</param>
		/// <param name="logger">An instance of the Logger object</param>
		public MergeISVProjectDriver(ICommandLineOptions options, ILogger logger)
		{
			_Options = options;
			_Logger = logger;

			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			_Sage300Path = GetSage300Path();
			_FolderManager = new FolderManager(logger, 
										      _Options.WebProjectPath.OptionValue, 
										      _Sage300Path, 
										      _Options.ModuleId);
			_Logger.Log(_FolderManager.GenerateLogOutput());
			VerifyCorrectBuildProfileSpecified();

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Ensure the user provided the correct build profile to use
		/// Throws an exception if wrong profile specified
		/// </summary>
		private void VerifyCorrectBuildProfileSpecified()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

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
		private void  StageFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
			{
				// Steps that involve just file copying
				Stage_Bin();
				Stage_BootstrapperToFinal();
				Stage_MenuDetailsToFinal();
				Stage_Areas();

				// Steps that involve compilation and minification
				CompileStagedViews();
				MinifyScripts();
				CopyCompiledAssetsToFinal();
				RemoveNonVendorRelatedFilesFromFinalBin();

				Stage_ResourceSatelliteFilesToFinal();
			}
			else if (_Options.Mode.OptionValue == (int) AppMode.SingleProject)
			{
				Stage_Bin();
				Stage_BootstrapperToFinal();
				CopyStagedBinToFinalBin();
				RemoveNonVendorRelatedFilesFromFinalBin();
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Minify the javascript files
		/// </summary>
		private void MinifyScripts()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			try
			{
				if (_Options.Minify.OptionValue)
				{
					var minifier = new SageISVMinifier(_Logger, _FolderManager, _Options.ModuleId);
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
		private void CopyCompiledAssetsToFinal()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var source = _FolderManager.Compiled.Root;
			var dest = _FolderManager.Final.Root;
			FileSystem.CopyDirectory(source, dest);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		private void CopyStagedBinToFinalBin()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var source = _FolderManager.Staging.Bin;
			var dest = _FolderManager.Final.Bin;
			FileSystem.CopyDirectory(source, dest);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}


		/// <summary>
		/// Remove non-essential files from the Final bin folder
		/// These are Microsoft and Sage files that were only necessary
		/// for the compilation step and are no longer necessary
		/// They are already installed in the live Sage 300 installation.
		/// </summary>
		private void RemoveNonVendorRelatedFilesFromFinalBin()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			string[] patternFilesToRemove = {
				"System.*.dll",
				"Sage.CA.SBS.ERP.*.dll",
				"*.Web.Infrastructure.dll"
			};
			DeleteFilesFromFolderBasedOnPatternList(_FolderManager.Final.Bin, patternFilesToRemove);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Delete files from a particular folder based on 
		/// a pattern list
		/// </summary>
		/// <param name="workingFolder">This the fully-qualified directory to delete the files from</param>
		/// <param name="patterns">This is the list of file types to look for</param>
		private void DeleteFilesFromFolderBasedOnPatternList(string workingFolder, string[] patterns)
		{
			foreach (var pattern in patterns)
			{
				var fileList = Directory.GetFiles(workingFolder, pattern);
				foreach (var file in fileList)
				{
					FileSystem.DeleteFile(file);
				}
			}
		}

		/// <summary>
		/// Copy the Areas folder (and all subfolders)
		/// Source:      ...XX.Web/Areas/* 
		/// Destination: ...XX.Web/__Deploy/Areas/
		/// </summary>
		private void Stage_Areas()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			const string webConfigName = @"Web.config";
			var source = _FolderManager.RootSource;
			var destination = _FolderManager.Staging.Areas;

			// Copy original Web.config to it's staging location
			var sourceWebConfig = Path.Combine(source, webConfigName);
			var destWebConfig = Path.Combine(destination, webConfigName);
			File.Copy(sourceWebConfig, destWebConfig);

			// Areas/XX/Views
			var d1 = _FolderManager.Originals.AreasViews;
			var d2 = _FolderManager.Staging.AreasViews;
			FileSystem.CopyDirectory(d1, d2);
			_Logger.Log($"Copying directory...");
			_Logger.Log($"Source: {d1}");
			_Logger.Log($"Destination: {d2}");

			// Areas/XX/Scripts
			d1 = _FolderManager.Originals.AreasScripts;
			d2 = _FolderManager.Staging.AreasScripts;
			FileSystem.CopyDirectory(d1, d2);
			_Logger.Log($"Copying directory...");
			_Logger.Log($"Source: {d1}");
			_Logger.Log($"Destination: {d2}");

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy a subset of files from the original Web project bin folder
		/// to the Deploy/Staging/bin folder.
		/// Some of these files are only used during the aspnet_compile step
		/// and will end up being removed from the final bin folder prior
		/// to live deployment (if enabled)
		/// </summary>
		private void Stage_Bin()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			string[] inclusionPatterns = null;

			if (_Options.Mode.OptionValue == (int) AppMode.FullSolution)
			{
				inclusionPatterns = new []{
					"System.*.dll",
					"Sage.CA.SBS.ERP.*.dll",
					"*.Web.Infrastructure.dll",
					"*.Web.dll",
					$"*.{_Options.ModuleId}.*.dll"
				};
			}
			else if (_Options.Mode.OptionValue == (int) AppMode.SingleProject)
			{
				inclusionPatterns = new[]{
					"*.*.Web.dll",
				};
			}

			CopyFilesBasedOnPatternList(_FolderManager.Originals.Bin, _FolderManager.Staging.Bin, inclusionPatterns);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Compile the Razor views
		/// </summary>
		private void CompileStagedViews()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var p = new Process
			{
				StartInfo =
				{
					UseShellExecute = false,
					FileName = Path.Combine(_Options.DotNetFrameworkPath.OptionValue, "aspnet_compiler.exe"),
					Arguments = $" -v / -p \"{_FolderManager.Staging.Root}\" -fixednames \"{_FolderManager.Compiled.Root}\""
				}
			};
			p.Start();
			p.WaitForExit();

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy files from one directory to another
		/// using a pattern list
		/// </summary>
		/// <param name="sourceDir">The fully-qualified source directory</param>
		/// <param name="destDir">The fully-qualified destination directory</param>
		/// <param name="patterns">The list of file patterns to look for</param>
		private void CopyFilesBasedOnPatternList(string sourceDir, string destDir, string[] patterns)
		{
			foreach (var pattern in patterns)
			{
				CopyFiles(false, sourceDir, pattern, destDir, true);
			}
		}

		/// <summary>
		/// Copy Module bootstrapper file from original folder to final deployment folder
		/// </summary>
		private void Stage_BootstrapperToFinal()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			if (_Options.Mode.OptionValue == (int) AppMode.FullSolution)
			{
				var bootstrapFileName = $"{_Options.ModuleId}bootstrapper.xml";
				var sourceFile = Path.Combine(_FolderManager.RootSource, bootstrapFileName);
				var destFile = Path.Combine(_FolderManager.Final.Root, bootstrapFileName);
				File.Copy(sourceFile, destFile, overwrite: OVERWRITE);
			} 
			else if (_Options.Mode.OptionValue == (int) AppMode.SingleProject)
			{
				var filePattern = $"*bootstrapper.xml";
				var files = Directory.GetFiles(_FolderManager.RootSource, filePattern);
				foreach (var sourceFile in files)
				{
					var destFile = Path.Combine(_FolderManager.Final.Root, new FileInfo(sourceFile).Name);
					File.Copy(sourceFile, destFile, overwrite: OVERWRITE);
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy XXMenuDetails.xml to Final staging folder
		/// </summary>
		private void Stage_MenuDetailsToFinal()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			Directory.CreateDirectory(Path.Combine(_FolderManager.Final.Root, 
												   FolderNameConstants.APPDATA));
			Directory.CreateDirectory(Path.Combine(_FolderManager.Final.Root, 
												   FolderNameConstants.APPDATA, 
												   FolderNameConstants.MENUDETAIL));

			var sourceFile = Path.Combine(_FolderManager.RootSource, _Options.MenuFilename.OptionValue);
			var destFile = Path.Combine(_FolderManager.Final.Root, 
										FolderNameConstants.APPDATA, 
										FolderNameConstants.MENUDETAIL,
										_Options.MenuFilename.OptionValue);
			File.Copy(sourceFile, destFile, overwrite: OVERWRITE);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Get Sage 300 installation path
		/// </summary>
		/// <returns>Sage 300 installation path</returns>
		private string GetSage300Path()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			const string sageRegKey = @"SOFTWARE\ACCPAC International, Inc.\ACCPAC\Configuration";
			const string sage64RegKey = @"SOFTWARE\WOW6432Node\ACCPAC International, Inc.\ACCPAC\Configuration";
			var key = Registry.LocalMachine.OpenSubKey(sageRegKey) ?? Registry.LocalMachine.OpenSubKey(sage64RegKey);
			_Logger.Log($"{Messages.Msg_CheckingForRegistryKeys}:");
			_Logger.Log(sageRegKey);
			_Logger.Log(sage64RegKey);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());

			return key?.GetValue("Programs").ToString() ?? string.Empty;
		}

		/// <summary>
		/// Copy bootStrapper, menuDetails, scripts, views and resource files to the Sage Online folder
		/// </summary>
		/// <returns>True if successful otherwise false</returns>
		private void DeployFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");
			_Logger.Log(DeployFilesStartupMessage());

			if (!Directory.Exists(_FolderManager.Live.Web))
			{
				throw new MergeISVProjectException(_Logger, Messages.Error_Sage300WebFolderMissing);
			}

			if (_Options.Mode.OptionValue == (int) AppMode.FullSolution)
			{
				Deploy_Bootstrapper();
				Deploy_MenuDetails();
				Deploy_AreaScripts();
				Deploy_CompiledViews();
				Deploy_BinFolders();
				Deploy_ResourceSatelliteFiles();
			}
			else if (_Options.Mode.OptionValue == (int) AppMode.SingleProject)
			{
				Deploy_Bootstrapper();
				Deploy_BinFolders();
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Create a logging message for the DeployFiles method
		/// Uses the TestDeploy flag to determine whether deployment
		/// will be simulated or not.
		/// </summary>
		/// <returns>The startup message</returns>
		private string DeployFilesStartupMessage()
		{
			return _Options.TestDeploy.OptionValue ? Messages.Msg_SimulatedDeploymentOnly
												   : Messages.Msg_LiveDeployment;
		}

		/// <summary>
		/// Deploy the Bootstrapper.xml files to the Sage 300 installation (Web and Worker)
		/// (Only if TestDeploy is not enabled)
		/// </summary>
		private void Deploy_Bootstrapper()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			const string searchPattern = "*bootstrapper.xml";
			var bootFiles = Directory.GetFiles(_FolderManager.Final.Root, searchPattern);
			foreach (var src in bootFiles)
			{
				var filename = Path.GetFileName(src);
				if (filename.IsNullOrWhiteSpace())
				{
					continue;
				}
				var dest = Path.Combine(_FolderManager.Live.Web, filename);
				CopyFile(_Options.TestDeploy.OptionValue, src, dest, OVERWRITE);

				if (_Options.Mode.OptionValue == (int) AppMode.FullSolution)
				{
					dest = Path.Combine(_FolderManager.Live.Worker, filename);
					CopyFile(_Options.TestDeploy.OptionValue, src, dest, OVERWRITE);
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the XXMenuDetails.xml files to the Sage 300 installation
		/// </summary>
		private void Deploy_MenuDetails()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var menuName = _Options.MenuFilename.OptionValue;

			// Copy menu file to App_Data menuDetails and all sub directories
			var pathMenuFrom = Path.Combine(_FolderManager.Final.Root, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL, menuName);
			var pathMenuDir = Path.Combine(_FolderManager.Live.Web, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL);
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
		private void Deploy_AreaScripts()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var moduleId = _Options.ModuleId;

			var pathScripts = Path.Combine(_FolderManager.Final.Root, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);
			if (Directory.Exists(pathScripts))
			{
				foreach (var sourceFolder in Directory.GetDirectories(pathScripts))
				{
					var paths = sourceFolder.Split('\\');
					var moduleName = paths[paths.Length - 1];
					var pathTargetSubArea = Path.Combine(_FolderManager.Live.Web, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);
					var pathToScripts = Path.Combine(pathTargetSubArea, moduleName);
					CopyDirectory(_Options.TestDeploy.OptionValue, sourceFolder, pathToScripts, OVERWRITE);
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Deploy the compiled view files to the Sage 300 installation
		/// </summary>
		private void Deploy_CompiledViews()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			// Copy compiled files from deploy folder to sage online web area and bin directory
			var pathBuildView = Path.Combine(_FolderManager.Final.Root, 
											 FolderNameConstants.AREAS, 
											_Options.ModuleId, 
											FolderNameConstants.VIEWS);
			var pathSageView = Path.Combine(_FolderManager.Live.Root, 
										    FolderNameConstants.WEB, 
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
		/// Deploy the Vendor's binary assets to the Sage 300 installtion (Web and Worker)
		/// </summary>
		private void Deploy_BinFolders()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var simulateCopy = _Options.TestDeploy.OptionValue;

			// Source folder
			var pathBuildBin = _FolderManager.Final.Bin;

			// Desination folders (Web\Bin and Worker)
			var pathLiveWebBin = Path.Combine(_FolderManager.Live.Web, FolderNameConstants.BIN);
			var pathLiveWorker = _FolderManager.Live.Worker;

			if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
			{
				string[] validWebFiles =
				{
					"*.compiled",
					"App_Web_*.dll",
					"*.Web.dll",
					$"*.{_Options.ModuleId}.*.dll"
				};

				foreach (var pattern in validWebFiles)
				{
					CopyFiles(simulateCopy, pathBuildBin, pattern, pathLiveWebBin, OVERWRITE);
				}

				string[] validWorkerFiles = { $"*.{_Options.ModuleId}.*.dll" };
				foreach (var pattern in validWorkerFiles)
				{
					CopyFiles(simulateCopy, pathBuildBin, pattern, pathLiveWorker, OVERWRITE, copyWebFile: false);
				}
			}
			else if (_Options.Mode.OptionValue == (int)AppMode.SingleProject)
			{
				string[] validWebFiles =
				{
					"*.*.Web.dll",
				};

				foreach (var pattern in validWebFiles)
				{
					CopyFiles(simulateCopy, pathBuildBin, pattern, pathLiveWebBin, OVERWRITE);
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy resource satellite dlls to web bin and worker folder
		/// </summary>
		private void Deploy_ResourceSatelliteFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var pathBinFrom = _FolderManager.Final.Bin;
			var pathWebBinTo = Path.Combine(_FolderManager.Live.Web, FolderNameConstants.BIN);
			var pathWorkerTo = _FolderManager.Live.Worker;
			var pattern = $"*.{_Options.ModuleId}.*.dll";
			bool testOnly = _Options.TestDeploy.OptionValue;

			foreach (var language in Languages)
			{
				var fromFolder = Path.Combine(pathBinFrom, language);
				var toWebFolder = Path.Combine(pathWebBinTo, language);
				var toWorkerFolder = Path.Combine(pathWorkerTo, language);

				// If the source folder doesn't exist, then just continue
				if (!Directory.Exists(fromFolder))
					continue;

				// Source folder actually exists, so let's proceed.
				foreach (var file in Directory.GetFiles(fromFolder, pattern))
				{
					var fileName = Path.GetFileName(file);
					if (!string.IsNullOrEmpty(fileName))
					{
						var pathFile = Path.Combine(toWebFolder, fileName);
						CopyFile(testOnly, file, pathFile, OVERWRITE);

						pathFile = Path.Combine(toWorkerFolder, fileName);
						CopyFile(testOnly, file, pathFile, OVERWRITE);
					}
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Staging - Copy resource satellite dlls to /Deploy/Final/bin/[LANGUAGE] folders
		/// </summary>
		private void Stage_ResourceSatelliteFilesToFinal()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var pathBinFrom = _FolderManager.Originals.Bin;
			var pathWebBinTo = _FolderManager.Final.Bin;
			var pattern = $"*.{_Options.ModuleId}.*.dll";

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
						CopyFile(testOnly: false, source: file, dest: destinationFile, overwrite: OVERWRITE);
					}
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy files from one folder to another.
		/// </summary>
		/// <param name="testOnly">true = Simulate file copy | false = Real file copy</param>
		/// <param name="pathFrom">The fully-qualified source folder</param>
		/// <param name="pattern">The pattern used to determine what files to copy</param>
		/// <param name="pathTo">The fully-qualified destination folder</param>
		/// <param name="overwrite">True to overwrite otherwise false</param>
		/// <param name="copyWebFile">True to copy *.web.dll otherwise false</param>
		private void CopyFiles(bool testOnly, string pathFrom, string pattern, string pathTo, bool overwrite, bool copyWebFile = true)
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			_Logger.Log($"{Messages.Msg_SourceFolder}: {pathFrom}");
			_Logger.Log($"{Messages.Msg_DestinationFolder}: {pathTo}");

			foreach (var file in Directory.GetFiles(pathFrom, pattern))
			{
				var fileName = Path.GetFileName(file);
				if (fileName.IsNullOrWhiteSpace() ||
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
		/// <param name="testOnly">true = Simulate file copy | false = Real file copy</param>
		/// <param name="source">The fully-qualified path to the source file</param>
		/// <param name="dest">The fully-qualified path to the destination file</param>
		/// <param name="overwrite">true = Overwrite file | false = Do not overwrite file</param>
		private void CopyFile(bool testOnly, string source, string dest, bool overwrite)
		{
			try
			{
				var simulationText = testOnly ? $"[{Messages.Msg_Simulation}] " : "";
				var sourceFilenameOnly = new FileInfo(source).Name;

				_Logger.Log($"{simulationText}{Messages.Msg_CopyingFile} {sourceFilenameOnly}.");
				if (!testOnly) { File.Copy(source, dest, overwrite); }
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
		/// <param name="testOnly">true = Test copy only | false = do actual file copy</param>
		/// <param name="source">This is the fully-qualified source folder</param>
		/// <param name="dest">This is the fully-qualified destination folder</param>
		/// <param name="overwrite">true = overwrite files | false = do not overwrite files</param>
		private void CopyDirectory(bool testOnly, string source, string dest, bool overwrite)
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var simulationText = testOnly ? $"[{Messages.Msg_Simulation}] " : "";
			if (testOnly)
			{
				_Logger.Log($"{Messages.Msg_Simulation} {Messages.Msg_Only}");
			}
			_Logger.Log($"{Messages.Msg_SourceFolder}: {source}");
			_Logger.Log($"{Messages.Msg_DestinationFolder}: {dest}");
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
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var runLiveDeployment = !_Options.NoDeploy.OptionValue;

			try
			{
				StageFiles();

				if (runLiveDeployment)
				{
					DeployFiles();
					_Logger.Log(Messages.Msg_FilesHaveBeenDeployedToLocalSage300Directory);
				}
				else
				{
					_Logger.Log(Messages.Msg_DeploymentToSage300InstallationDisabled);
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
