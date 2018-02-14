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

using MergeISVProject.CustomExceptions;
using MergeISVProject.Errors;
using MergeISVProject.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace MergeISVProject
{
	public class SageISVMinifier
	{
		#region Constants
		private const string WG_EXE = @"WG.EXE";
		private const string WG_COMMAND_TEMPLATE = @"{0}\{1} -m -in:{2} -out:{3}";
		#endregion

		#region Private Variables
		private ILogger _Logger = null;
		private FolderManager _Folders;
		private string _ModuleId = string.Empty;
		#endregion

		#region Constructor(s)
		public SageISVMinifier(ILogger logger, FolderManager folders, string moduleId)
		{
			_Logger = logger;
			_Folders = folders;
			_ModuleId = moduleId;
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Delete the unminified javascript files in a folder
		/// </summary>
		/// <param name="folder"></param>
		private void RemoveUnminifiedJavascriptFiles(string folder)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var files = GetListOfUnminifiedJavascriptFiles(folder);
			_Logger.Log("folder = " + folder);
			_Logger.Log($"files.Count() = {files.Count()}");

			if (files.Count() > 0)
			{
				foreach (var file in files)
				{
					File.Delete(file);
					_Logger.Log("Delete file " + file);
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// TODO - Add Summary
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		private IEnumerable<string> GetListOfUnminifiedJavascriptFiles(string folder)
		{
			const string SourcePattern = ".min.js";
			var filesToSearchFilter = @"*.js";
			return Directory.GetFiles(folder,
									  filesToSearchFilter,
									  System.IO.SearchOption.AllDirectories).Where(f => !f.EndsWith(SourcePattern));
		}

		/// <summary>
		/// TODO - Add Summary
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		private IEnumerable<string> GetListOfMinifiedJavascriptFiles(string folder)
		{
			const string SourcePattern = ".min.js";
			var filesToSearchFilter = @"*.js";
			return Directory.GetFiles(folder,
									  filesToSearchFilter,
									  System.IO.SearchOption.AllDirectories).Where(f => f.EndsWith(SourcePattern));
		}

		/// <summary>
		/// Rename javascript files of the format *.min.js to *.js
		/// </summary>
		/// <param name="folder"></param>
		private void RenameMinifiedJavascriptFiles(string folder)
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			const string SourcePattern = ".min.js";
			const string TargetPattern = ".js";
			var files = GetListOfMinifiedJavascriptFiles(folder);

			_Logger.Log($"folder = {folder}");
			_Logger.Log($"files.Count() = {files.Count()}");

			if (files.Count() > 0)
			{
				foreach (var file in files)
				{
					var newName = file.Replace(SourcePattern, TargetPattern);
					File.Move(file, newName);
					var f1 = GetFilenameFromPath(file);
					var f2 = GetFilenameFromPath(newName);
					_Logger.Log($"Rename {f1} to {f2}");
				}
			}
			_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
		}

		/// <summary>
		/// Given a path, return the filename only
		/// </summary>
		/// <param name="dir">A file path</param>
		/// <returns>The filename only</returns>
		private string GetFilenameFromPath(string dir)
		{
			var result = string.Empty;
			if (dir.Length > 0)
			{
				result = new FileInfo(dir).Name;
			}
			return result;
		}

		/// <summary>
		/// Execute command line for minified JS files
		/// </summary>
		/// <param name="workingDirectory"></param>
		/// <param name="command">Command(s) to execute</param>
		/// <param name="hiddenWindow">Visible or hidden window flag</param>
		private void ExecuteCommand(string workingDirectory, string command, bool hiddenWindow = true)
		{
			var p = new Process
			{
				StartInfo =
				{
					WindowStyle = hiddenWindow ? System.Diagnostics.ProcessWindowStyle.Hidden
											   : System.Diagnostics.ProcessWindowStyle.Normal,
					WorkingDirectory = workingDirectory,
					FileName = "cmd.exe",
					Arguments = "/C " + command,
				}
			};
			p.Start();
			p.WaitForExit(60000);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Minify the javascript files and rename back to usable names
		/// Example: TrustedVendor.PM.PaymentCodesBehaviour.min.js --> TrustedVendor.PM.PaymentCodesBehaviour.js
		/// </summary>
		/// <param name="pathWebProj">Web project path</param>
		/// <param name="moduleId">The module id</param>
		public void MinifyJavascriptFilesAndCleanup()
		{
			_Logger.LogMethodHeader(Utilities.GetCurrentMethod());

			var error = false;

			try
			{
				var currentExePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
				var tempPathToWG = Path.Combine(currentExePath, WG_EXE);
				if (!File.Exists(tempPathToWG))
				{
					throw new Exception($"Unable to find {WG_EXE}. Path={tempPathToWG}");
				}

				var pathToWG = currentExePath;

				var workingFolder = _Folders.Compiled.AreasScripts;
				var jsFolder = workingFolder;

				_Logger.Log($"jsFolder = {jsFolder}");

				foreach (var dir in Directory.GetDirectories(jsFolder))
				{
					var command = string.Format(WG_COMMAND_TEMPLATE, pathToWG, WG_EXE, dir, dir);
					_Logger.Log($"Beginning minification process on directory {dir}");
					_Logger.Log("  Running command : " + command);
					ExecuteCommand(workingFolder, command);
					_Logger.Log("Minification complete.");

					_Logger.Log("Renaming javascipt files back to usable state");
					RemoveUnminifiedJavascriptFiles(dir);
					RenameMinifiedJavascriptFiles(dir);
					_Logger.Log("Renaming complete!");
				}
			}
			catch (Exception ex)
			{
				error = true;
				var msg = $"{ErrorMessages.MinificationFailed}{Environment.NewLine}{ex.Message}";
				throw new MergeISVProjectException(_Logger, msg);
			}
			finally
			{
				if (!error)
				{
					_Logger.Log("Minification successful!");
				}
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}

		}

		/// <summary>
		/// Copy minified javascript files to the Final 
		/// Staging folder
		/// </summary>
		public void CopyToFinalStagingLocation()
		{
			var source = _Folders.Staging.Areas;
			var dest = _Folders.Final.Areas;
			FileSystem.CopyDirectory(source, dest, overwrite: true);
		}

		#endregion
	}
}
