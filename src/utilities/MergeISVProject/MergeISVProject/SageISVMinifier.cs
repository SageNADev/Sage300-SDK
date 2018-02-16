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
	/// <summary>
	/// Minification handler class
	/// </summary>
	public class SageISVMinifier
	{
		#region Constants
		private const string WG_EXE = @"WG.EXE";
		private const string WG_COMMAND_TEMPLATE = @"{0}\{1} -m -in:{2} -out:{3}";
		private const bool MINIFIED = true;
		private const bool UNMINIFIED = false;
		private const string MINIFIED_SOURCE_PATTERN = @".min.js";
		private const string JAVASCRIPT_FILE_FILTER = @"*.js";
		private const string JAVASCRIPT_FILE_EXTENSION = @".js";
		#endregion

		#region Private Variables
		private ILogger _Logger = null;
		private FolderManager _Folders;
		private string _ModuleId = string.Empty;
		#endregion

		#region Constructor(s)

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger">The instance of the logger object</param>
		/// <param name="folders">The instance FolderManager object</param>
		/// <param name="moduleId">The string representation of the Module ID</param>
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
		/// <param name="folder">The folder from which unminified files will be deleted</param>
		private void RemoveUnminifiedJavascriptFiles(string folder)
		{
			string methodName = string.Empty;
			try
			{
				methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
				//methodName = Utilities.GetCurrentMethod();
				_Logger.LogMethodHeader(methodName);

				if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException();

				var files = GetListOfUnminifiedJavascriptFiles(folder);
				_Logger.Log(string.Format(Messages.Msg_FolderEquals, folder));
				_Logger.Log(string.Format(Messages.Msg_FilesDotCount, files.Count()));

				if (files.Count() > 0)
				{
					foreach (var file in files)
					{
						File.Delete(file);
						_Logger.Log(string.Format(Messages.Msg_DeleteFile, new FileInfo(file).Name));
					}
				}
			}
			catch (ArgumentNullException ex)
			{
				var msg = string.Format(Messages.Error_MethodCalledWithInvalidParameter, methodName);
				throw new MergeISVProjectException(_Logger, msg, ex);
			}
			finally
			{
				_Logger.LogMethodFooter(methodName);
			}
		}

		/// <summary>
		/// Get a list of all unminified javascript files in a folder
		/// </summary>
		/// <param name="folder">The folder from which to look for unminified javascript files</param>
		/// <returns>The list of minified file names</returns>
		private IEnumerable<string> GetListOfUnminifiedJavascriptFiles(string folder)
		{
			return GetListOfJavascriptFiles(folder, UNMINIFIED);
		}

		/// <summary>
		/// Get a list of all minified javascript files in a folder
		/// </summary>
		/// <param name="folder">The folder from which to search for files</param>
		/// <returns>The list of minified file names</returns>
		private IEnumerable<string> GetListOfMinifiedJavascriptFiles(string folder)
		{
			return GetListOfJavascriptFiles(folder, MINIFIED);
		}

		/// <summary>
		/// Get a list of all minified or unminified javascript files from a folder
		/// </summary>
		/// <param name="folder">The folder from which to search for files</param>
		/// <param name="minified">true = minified | false = unminified</param>
		/// <returns>A list of file names</returns>
		private IEnumerable<string> GetListOfJavascriptFiles(string folder, bool minified)
		{
			string methodName = string.Empty;
			try
			{
				methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
				_Logger.LogMethodHeader(methodName);

				if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException();

				// Get list of all files in directory
				var allFiles = Directory.GetFiles(folder, JAVASCRIPT_FILE_FILTER, System.IO.SearchOption.AllDirectories);

				// Now filter based on whether were looking for minified or unminified javascript files
				var results = (minified) ? allFiles.Where(f => f.EndsWith(MINIFIED_SOURCE_PATTERN))
									     : allFiles.Where(f => !f.EndsWith(MINIFIED_SOURCE_PATTERN));
				
				_Logger.Log($"{results.Count()} {Messages.Msg_FilesFound}." );

				return results;
			}
			catch (ArgumentNullException ex)
			{
				var msg = string.Format(Messages.Error_MethodCalledWithInvalidParameter, methodName);
				throw new MergeISVProjectException(_Logger, msg, ex);
			}
			finally
			{
				_Logger.LogMethodFooter(methodName);
			}
		}

		/// <summary>
		/// Rename javascript files of the format *.min.js to *.js
		/// </summary>
		/// <param name="folder">The folder from which to rename minified javascript files</param>
		private void RenameMinifiedJavascriptFiles(string folder)
		{
			string methodName = string.Empty;
			try
			{
				methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
				//methodName = Utilities.GetCurrentMethod();
				_Logger.LogMethodHeader(methodName);

				if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException();

				var minifiedJavascriptFiles = GetListOfMinifiedJavascriptFiles(folder);
				int fileCount = minifiedJavascriptFiles.Count();

				_Logger.Log(string.Format(Messages.Msg_FolderEquals, folder));
				_Logger.Log(string.Format(Messages.Msg_FilesDotCount, fileCount));

				if (fileCount == 0)
					return;

				foreach (var file in minifiedJavascriptFiles)
				{
					var newName = file.Replace(MINIFIED_SOURCE_PATTERN, JAVASCRIPT_FILE_EXTENSION);
					File.Move(file, newName);
					var f1 = new FileInfo(file).Name;
					var f2 = new FileInfo(newName).Name;
					_Logger.Log(string.Format(Messages.Msg_Rename1To2, f1, f2));
				}
			}
			catch (ArgumentNullException ex)
			{
				var msg = string.Format(Messages.Error_MethodCalledWithInvalidParameter, methodName);
				throw new MergeISVProjectException(_Logger, msg, ex);
			}
			finally
			{
				_Logger.LogMethodFooter(methodName);
			}
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
		public void MinifyJavascriptFilesAndCleanup()
		{
			_Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

			var error = false;

			try
			{
				var currentExePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
				var tempPathToWG = Path.Combine(currentExePath, WG_EXE);
				if (!File.Exists(tempPathToWG))
				{
					var msg = string.Format(Messages.Error_UnableToFindTheProgram, WG_EXE, tempPathToWG);
					throw new Exception(msg);
				}

				var pathToWG = currentExePath;

				var workingFolder = _Folders.Compiled.AreasScripts;
				var jsFolder = workingFolder;

				_Logger.Log($"jsFolder = {jsFolder}");

				foreach (var dir in Directory.GetDirectories(jsFolder))
				{
					var command = string.Format(WG_COMMAND_TEMPLATE, pathToWG, WG_EXE, dir, dir);
					_Logger.Log(string.Format(Messages.Msg_BeginningMinificationProcessOnDirectory, dir));
					_Logger.Log(string.Format(Messages.Msg_RunningCommand, command));
					ExecuteCommand(workingFolder, command);
					_Logger.Log(Messages.Msg_MinificationComplete);

					_Logger.Log(Messages.Msg_RenamingJavascriptFilesBackToUsableState);
					RemoveUnminifiedJavascriptFiles(dir);
					RenameMinifiedJavascriptFiles(dir);
					_Logger.Log(Messages.Msg_RenamingComplete);
				}
			}
			catch (Exception ex)
			{
				error = true;
				var msg = $"{Messages.Error_MinificationFailed}{Environment.NewLine}{ex.Message}";
				throw new MergeISVProjectException(_Logger, msg);
			}
			finally
			{
				if (!error)
				{
					_Logger.Log(Messages.Msg_MinificationSuccessful);
				}
				_Logger.LogMethodFooter(Utilities.GetCurrentMethod());
			}
		}

		/// <summary>
		/// Copy minified javascript files to the Final Staging folder
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
