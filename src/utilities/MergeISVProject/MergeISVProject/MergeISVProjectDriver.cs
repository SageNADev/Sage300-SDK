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

#define ENABLE_STAGE_WEBCONFIG

#region Imports
using MergeISVProject.Constants;
using MergeISVProject.CustomExceptions;
using MergeISVProject.Interfaces;
//using Microsoft.Ajax.Utilities;
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
        const string WEB_CONFIG = @"web.config";
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
		private void StageFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			if (_Options.Mode.OptionValue == (int) AppMode.FullSolution)
			{
#if ENABLE_STAGE_WEBCONFIG
                _Stage_WebConfig();
#endif
                _Stage_Areas();
				_Stage_Bin();
				_Stage_CompileViewsAndMoveResultsToStagingFolder();
				_Stage_Bootstrapper();
				_Stage_Menus();
				_Stage_MinifyJavascripts();
				// _Stage_Images(); // Images have been moved to Areas\appid\ExternalContent
				_Stage_ResourceSatelliteFiles();
				_Stage_CopyAllToFinal();
			}
			else if (_Options.Mode.OptionValue == (int) AppMode.SingleProject)
			{
				_Stage_Bin();
				_Stage_Bootstrapper();
				//CopyStagedBinToFinalBin();
				//RemoveNonVendorRelatedFilesFromFinalBin();
				_Stage_CopyAllToFinal();
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

#if ENABLE_STAGE_WEBCONFIG
        /// <summary>
        /// Copy the Web.Config from the original Web folder
        /// to the staging folder
        /// </summary>
        private void _Stage_WebConfig()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			const string webConfigName = @"Web.config";
			var source = _FolderManager.RootSource;
			//var destination = _FolderManager.Staging.Areas;
			var destination = _FolderManager.Staging.Root;

			// Copy original Web.config to it's staging location
			var sourceWebConfig = Path.Combine(source, webConfigName);
			var destWebConfig = Path.Combine(destination, webConfigName);
			File.Copy(sourceWebConfig, destWebConfig);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}
#endif

		/// <summary>
		/// Copy the Areas folder (and all subfolders)
		/// Source:      ...XX.Web/Areas/* 
		/// Destination: ...XX.Web/[DEPLOYMENT]/Areas/*
		/// </summary>
		private void _Stage_Areas()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

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

            // Areas/XX/ExternalContent
            d1 = _FolderManager.Originals.AreasExternalContent;
            d2 = _FolderManager.Staging.AreasExternalContent;
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
		private void _Stage_Bin()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			string[] inclusionPatterns = null;

			if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
			{
				inclusionPatterns = new[]{
					"System.*.dll",
					"Sage.CA.SBS.ERP.*.dll",
					"*.Web.Infrastructure.dll",
					"*.Web.dll",
					$"*.{_Options.ModuleId}.*.dll"
				};
			}
			else if (_Options.Mode.OptionValue == (int)AppMode.SingleProject)
			{
				inclusionPatterns = new[]{
					"*.*.Web.dll",
				};
			}

			CopyFilesBasedOnPatternList(_FolderManager.Originals.Bin, _FolderManager.Staging.Bin, inclusionPatterns);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Compile the Razor views and put into the 'Compiled' folder.
		/// Move the compiled files back to the 'Staging' folder
		/// </summary>
		private void _Stage_CompileViewsAndMoveResultsToStagingFolder()
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

			// Now, move the results of the compilation back to the Staging folder

			// Clear out the Staging/Bin folder
			var fileList = Directory.GetFiles(_FolderManager.Staging.Bin, "*.*");
			foreach (var file in fileList)
			{
				FileSystem.DeleteFile(file);
			}

			// Copy compile/bin back to staging/bin
			FileSystem.CopyDirectory(_FolderManager.Compiled.Bin, _FolderManager.Staging.Bin);

			// Delete non-vendor related files from staging/bin folder
			string[] patternFilesToRemove = {
				"System.*.dll",
				"Sage.CA.SBS.ERP.*.dll",
				"*.Web.Infrastructure.dll"
			};
			DeleteFilesFromFolderBasedOnPatternList(_FolderManager.Staging.Bin, patternFilesToRemove);


			// Areas folder
			// Clear out the Staging/Areas folder
			Directory.Delete(_FolderManager.Staging.Areas, recursive: true);
			Directory.CreateDirectory(_FolderManager.Staging.Areas);

			// Copy compile/areas back to staging/areas
			FileSystem.CopyDirectory(_FolderManager.Compiled.Areas, _FolderManager.Staging.Areas);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy Module bootstrapper file from original folder to staging folder
		/// </summary>
		private void _Stage_Bootstrapper()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
			{
				var bootstrapFileName = $"{_Options.ModuleId}bootstrapper.xml";
				var sourceFile = Path.Combine(_FolderManager.RootSource, bootstrapFileName);
				var destFile = Path.Combine(_FolderManager.Staging.Root, bootstrapFileName);
				File.Copy(sourceFile, destFile, overwrite: OVERWRITE);
			}
			else if (_Options.Mode.OptionValue == (int)AppMode.SingleProject)
			{
				var filePattern = $"*bootstrapper.xml";
				var files = Directory.GetFiles(_FolderManager.RootSource, filePattern);
				foreach (var sourceFile in files)
				{
					var destFile = Path.Combine(_FolderManager.Staging.Root, new FileInfo(sourceFile).Name);
					File.Copy(sourceFile, destFile, overwrite: OVERWRITE);
				}
			}

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy XXMenuDetails.xml to staging folder
		/// </summary>
		private void _Stage_Menus()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			Directory.CreateDirectory(Path.Combine(_FolderManager.Staging.Root, FolderNameConstants.APPDATA));
			Directory.CreateDirectory(Path.Combine(_FolderManager.Staging.Root, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL));

			var sourceFile = Path.Combine(_FolderManager.RootSource, _Options.MenuFilename.OptionValue);
			var destFile = Path.Combine(_FolderManager.Staging.Root, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL, _Options.MenuFilename.OptionValue);
			File.Copy(sourceFile, destFile, overwrite: OVERWRITE);

			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Minify the javascript files
		/// </summary>
		private void _Stage_MinifyJavascripts()
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
			catch (Exception e)
			{
				throw new MergeISVProjectException(_Logger, e.Message);
			}
			finally
			{
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}
		}

		/// <summary>
		/// Staging - Copy resource satellite dlls to Staging/bin folder
		/// </summary>
		private void _Stage_ResourceSatelliteFiles() 
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var pathBinFrom = _FolderManager.Originals.Bin;
			var pathWebBinTo = _FolderManager.Staging.Bin;
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
						CopyFile(testOnly: false, source: file, dest: destinationFile, overwrite: true);
					}
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Copy all staging files to the final staging folder(s)
		/// </summary>
		private void _Stage_CopyAllToFinal()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

            // Copy everything to the 'Web' deployment folder
            FileSystem.CopyDirectory(_FolderManager.Staging.Root, _FolderManager.FinalWeb.Root);

            // D-39067
            // Because ENABLE_STAGE_WEBCONFIG has been reactivated, we have to ensure that the 
            // final web root folder does NOT have a copy of the main web.config file.
            var webConfigFilePath = Path.Combine(_FolderManager.FinalWeb.Root, WEB_CONFIG);
            if (File.Exists(webConfigFilePath))
            {
                _Logger.Log($"{WEB_CONFIG} found in final 'web' deployment folder.");
                _Logger.Log($"Deleting {webConfigFilePath}");
                File.Delete(webConfigFilePath);
            }

            // Copy a subset to the 'Worker' deployment folder
            _Stage_CopyToWorkerDeployment(); 

            _Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

        /// <summary>
        /// Copy all necessary staging files to the final worker staging folder
        /// </summary>
        private void _Stage_CopyToWorkerDeployment()
        {
            _Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

            // 1. Copy bootstrapper.xml to Worker folder
            if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
            {
                var bootstrapFileName = $"{_Options.ModuleId}bootstrapper.xml";
                var sourceFile = Path.Combine(_FolderManager.Staging.Root, bootstrapFileName);
                var destFile = Path.Combine(_FolderManager.FinalWorker.Root, bootstrapFileName);
                File.Copy(sourceFile, destFile, overwrite: OVERWRITE);
            }

            // 2. Copy DLL's to Worker folder
            //    *.*.BusinessRepository.dll
            //    *.*.Interfaces.dll
            //    *.*.Models.dll
            //    *.*.Resources.dll
            //    *.*.Services.dll

            string[] inclusionPatterns = null;

            if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
            {
                inclusionPatterns = new[]{
                    $"*.{_Options.ModuleId}.BusinessRepository.dll",
                    $"*.{_Options.ModuleId}.Interfaces.dll",
                    $"*.{_Options.ModuleId}.Models.dll",
                    $"*.{_Options.ModuleId}.Resources.dll",
                    $"*.{_Options.ModuleId}.Services.dll",
                };
            }

            CopyFilesBasedOnPatternList(_FolderManager.Staging.Bin, _FolderManager.FinalWorker.Root, inclusionPatterns);

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
		private void DeployFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");
			_Logger.Log(DeployFilesStartupMessage());

			if (!Directory.Exists(_FolderManager.Live.Web))
			{
				throw new MergeISVProjectException(_Logger, Messages.Error_Sage300WebFolderMissing);
			}

			if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
			{
				_Deploy_Bootstrapper();
				_Deploy_MenuDetails();
				// _Deploy_Images(); // Images are now in the Areas\appid\ExternalContent folder
				_Deploy_AreaScripts();
                _Deploy_AreaExternalContent();
                _Deploy_CompiledViews();
				_Deploy_BinFolders();
				_Deploy_ResourceSatelliteFiles();
			}
			else if (_Options.Mode.OptionValue == (int)AppMode.SingleProject)
			{
				_Deploy_Bootstrapper();
				_Deploy_BinFolders();
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
		private void _Deploy_Bootstrapper()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			const string searchPattern = "*bootstrapper.xml";
			var bootFiles = Directory.GetFiles(_FolderManager.FinalWeb.Root, searchPattern);
			foreach (var src in bootFiles)
			{
				var filename = Path.GetFileName(src);
				if (string.IsNullOrWhiteSpace(filename))
				{
					continue;
				}
				var dest = Path.Combine(_FolderManager.Live.Web, filename);
				CopyFile(_Options.TestDeploy.OptionValue, src, dest, OVERWRITE);

				if (_Options.Mode.OptionValue == (int)AppMode.FullSolution)
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
		private void _Deploy_MenuDetails()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var menuName = _Options.MenuFilename.OptionValue;

			// Copy menu file to App_Data menuDetails and all sub directories
			var pathMenuFrom = Path.Combine(_FolderManager.FinalWeb.Root, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL, menuName);
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
		/// Copy the menu background image and icon from 
		/// to the Staging folder
		/// </summary>
		//private void _Deploy_Images()
		//{
		//	_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

		//	var menuName = _Options.MenuFilename.OptionValue;

		//	// Get the company name, icon file name, and background image file name 
		//	//var menuFilePath = Path.Combine(pathWebProj, menuFileName);
		//	var pathMenuDir = Path.Combine(_FolderManager.Live.Web, FolderNameConstants.APPDATA, FolderNameConstants.MENUDETAIL);
		//	var menuFilePath = Path.Combine(pathMenuDir, menuName);
		//	var menuFileContent = File.ReadAllText(menuFilePath);
		//	var companyName = Regex.Match(menuFileContent, @"<IconName>(.*?)/menuIcon.png</IconName>").Groups[1].Value;
		//	var menuIconName = Regex.Match(menuFileContent, @"/(.*?)</IconName>").Groups[1].Value;
		//	var menuBackGroundImage = Regex.Match(menuFileContent, @"/(.*?)</MenuBackGoundImage>").Groups[1].Value;

		//	var pathImageFrom = Path.Combine(_FolderManager.Final.Root, @"External\Content\Images\nav", companyName);
		//	var pathImageTo = Path.Combine(_FolderManager.Live.Web, @"External\Content\Images\nav", companyName);
		//	_Logger.Log($"pathImageFrom = '{pathImageFrom}'");
		//	_Logger.Log($"pathImageTo = '{pathImageTo}'");


		//	if (!Directory.Exists(pathImageTo))
		//	{
		//		Directory.CreateDirectory(pathImageTo);
		//		_Logger.Log($"Created directory '{pathImageTo}'");
		//	}
		//	string[] imageNames = { menuIconName, menuBackGroundImage };
		//	foreach (var image in imageNames)
		//	{
		//		var pathImageFileFrom = Path.Combine(pathImageFrom, image);
		//		if (File.Exists(pathImageFileFrom))
		//		{
		//			_Logger.Log($"File '{pathImageFileFrom}' exists.");

		//			var pathImageFileTo = Path.Combine(pathImageTo, image);
		//			File.Copy(pathImageFileFrom, pathImageFileTo, true);
		//		}
		//		else
		//		{
		//			_Logger.Log($"File '{pathImageFileFrom}' does not exist.");
		//		}
		//	}

		//	_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		//}

		/// <summary>
		/// Deploy the Area scripts to the Sage 300 installation
		/// </summary>
		private void _Deploy_AreaScripts()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var moduleId = _Options.ModuleId;

			var pathScripts = Path.Combine(_FolderManager.FinalWeb.Root, FolderNameConstants.AREAS, moduleId, FolderNameConstants.SCRIPTS);
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
        /// Deploy the Area ExternalContent to the Sage 300 installation
        /// </summary>
        private void _Deploy_AreaExternalContent()
        {
            _Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

            // Copy ExternalContent from deploy folder to sage online web area and ExternalContent directory
            var pathSource = Path.Combine(_FolderManager.FinalWeb.Root,
                                             FolderNameConstants.AREAS,
                                            _Options.ModuleId,
                                            FolderNameConstants.EXTERNALCONTENT);
            var pathTo = Path.Combine(_FolderManager.Live.Root,
                                            FolderNameConstants.WEB,
                                            FolderNameConstants.AREAS,
                                            _Options.ModuleId,
                                            FolderNameConstants.EXTERNALCONTENT);

            CopyDirectory(_Options.TestDeploy.OptionValue, pathSource, pathTo, OVERWRITE);

            _Logger.LogMethodFooter(Utilities.GetCurrentMethod());
        }

        /// <summary>
        /// Deploy the compiled view files to the Sage 300 installation
        /// </summary>
        private void _Deploy_CompiledViews()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			// Copy compiled files from deploy folder to sage online web area and bin directory
			var pathBuildView = Path.Combine(_FolderManager.FinalWeb.Root, 
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
		private void _Deploy_BinFolders()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var simulateCopy = _Options.TestDeploy.OptionValue;

			// Source folder
			var pathBuildBin = _FolderManager.FinalWeb.Bin;

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
		private void _Deploy_ResourceSatelliteFiles()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var pathBinFrom = _FolderManager.FinalWeb.Bin;
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
				if (string.IsNullOrWhiteSpace(fileName) ||
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
                // Old VB-based call
                //FileSystem.CopyDirectory(source, dest, overwrite);

                var util = new FileUtilities(_Logger);
                util.DirectoryCopy(source, dest);
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

			var deployToLocalSage300Installation = !_Options.NoDeploy.OptionValue;

			try
			{
				// Copy all necessary files from the project to the Staging folder
				// Do any necessary aspnet compilation
				// Do any necessary javascript minification (if enabled)
				// Once the above has completed, copy all files to the final staging folder
				StageFiles();

				if (deployToLocalSage300Installation)
				{
					// This step simply copies the assets out of the 
					// __READYTODEPLOY__ folder (whatever it's called)
					// to the live Sage 300 installation. No other 
					// processing occurs.
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
