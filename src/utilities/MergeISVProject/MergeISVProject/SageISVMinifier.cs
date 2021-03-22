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

using MergeISVProject.CustomExceptions;
using MergeISVProject.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
//using DouglasCrockford.JsMin;
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
        string WG_COMMAND_ARGUMENT_TEMPLATE = @"-m -in:""{0}"" -out:""{1}""\";
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
                _Logger.LogMethodHeader(methodName);

                if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException();

                var files = GetListOfUnminifiedJavascriptFiles(folder);
                var count = files.Count();
                _Logger.Log(string.Format(Messages.Msg_FolderEquals, folder));
                _Logger.Log(string.Format(Messages.Msg_FilesDotCount, count));

                if (count == 0) return;

                foreach (var file in files)
                {
                    File.Delete(file);
                    _Logger.Log(string.Format(Messages.Msg_DeleteFile, new FileInfo(file).Name));
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
        /// Delete the unminified javascript files in a folder based on the list passed into method
        /// This method will delete the unminified version of a file ONLY if a minified version 
        /// exists in the same folder, otherwise the unminified version will be left alone.
        /// This may happen if WebGrease cannot minify a file.
        /// </summary>
        /// <param name="unminifiedFiles">The string array of unminified filenames</param>
        private void RemoveUnminifiedJavascriptFiles(string[] unminifiedFiles)
        {
            string methodName = string.Empty;
            try
            {
                methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
                _Logger.LogMethodHeader(methodName);

                //var files = GetListOfUnminifiedJavascriptFiles(folder);
                var count = unminifiedFiles.Count();
                _Logger.Log(string.Format(Messages.Msg_FilesDotCount, count));

                if (count == 0) return;

                foreach (var unminifiedFile in unminifiedFiles)
                {
                    // For each file in this list, look for the minified version in this directory
                    var minifiedFilePath = MakeMinifiedName(unminifiedFile);

                    // If the minified version of the file exists
                    // then remove the unminified version.
                    if (File.Exists(minifiedFilePath))
                    {
                        File.Delete(unminifiedFile);
                        _Logger.Log(string.Format(Messages.Msg_DeleteFile, new FileInfo(unminifiedFile).Name));
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
        /// 
        /// </summary>
        /// <param name="unminifiedFilename"></param>
        /// <returns></returns>
        private string MakeMinifiedName(string unminifiedFilename)
        {
            return unminifiedFilename.Replace(".js", ".min.js");
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

                // Get all files from a single directory as WebGrease does not do subfolders (iteration of subfolders
                // is in MinifyJavaScriptFilesAndCleanup
                var allFiles = Directory.GetFiles(folder, JAVASCRIPT_FILE_FILTER);

                // Now filter based on whether were looking for minified or unminified javascript files
                var results = (minified) ? allFiles.Where(f => f.EndsWith(MINIFIED_SOURCE_PATTERN))
                                         : allFiles.Where(f => !f.EndsWith(MINIFIED_SOURCE_PATTERN));

                _Logger.Log($"{results.Count()} {Messages.Msg_FilesFound}.");

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
                _Logger.LogMethodHeader(methodName);

                if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException();

                var minifiedJavascriptFiles = GetListOfMinifiedJavascriptFiles(folder);
                var count = minifiedJavascriptFiles.Count();

                _Logger.Log(string.Format(Messages.Msg_FolderEquals, folder));
                _Logger.Log(string.Format(Messages.Msg_FilesDotCount, count));

                if (count == 0) return;

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
        /// <param name="programToRun">The program to run</param>
        /// <param name="workingDirectory">The working directory</param>
        /// <param name="arguments">Arguments to pass to program</param>
        /// <param name="hiddenWindow">Visible or hidden window flag</param>
        private void ExecuteCommand(string programToRun, string workingDirectory, string arguments, bool hiddenWindow = true)
        {
            var methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
            _Logger.LogMethodHeader(methodName);

            var p = new Process
            {
                StartInfo =
                {
                    WindowStyle = hiddenWindow ? System.Diagnostics.ProcessWindowStyle.Hidden
                                               : System.Diagnostics.ProcessWindowStyle.Normal,
                    WorkingDirectory = workingDirectory,
                    FileName = programToRun,
                    Arguments = arguments,
                }
            };
            p.Start();
            p.WaitForExit(60000);

            _Logger.LogMethodFooter(methodName);
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

                // Ensure that the WG.exe file is available
                var tempPathToWG = Path.Combine(currentExePath, WG_EXE);
                if (!File.Exists(tempPathToWG))
                {
                    _Logger.Log($"It looks like WG cannot be found.");

                    var msg = string.Format(Messages.Error_UnableToFindTheProgram, WG_EXE, tempPathToWG);
                    throw new Exception(msg);
                }

                var pathToWG = Path.Combine(currentExePath, WG_EXE);
                var workingFolder = _Folders.Staging.AreasScripts;
                var jsFolder = workingFolder;

                _Logger.Log($"jsFolder = {jsFolder}");
                if (Directory.Exists(jsFolder))
                {
                    // WebGrease does not do subfolders, so iteration is here (
                    foreach (var dir in Directory.GetDirectories(jsFolder, "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        _Logger.Log($"Processing directory '{dir}'");

                        var files = Directory.GetFiles(dir);
                        if (files.Count() == 0)
                        {
                            _Logger.Log($"No files found in folder '{dir}'. Skipping to next directory in list.");
                            continue;
                        }

                        var arguments = string.Format(WG_COMMAND_ARGUMENT_TEMPLATE, dir, dir);
                        _Logger.Log(string.Format(Messages.Msg_BeginningMinificationProcessOnDirectory, dir));
                        _Logger.Log(string.Format(Messages.Msg_RunningCommand, arguments));
                        ExecuteCommand(pathToWG, workingFolder, arguments);
                        _Logger.Log(Messages.Msg_MinificationComplete);

                        _Logger.Log(Messages.Msg_RenamingJavascriptFilesBackToUsableState);
                        RemoveUnminifiedJavascriptFiles(files);
                        RenameMinifiedJavascriptFiles(dir);
                        _Logger.Log(Messages.Msg_RenamingComplete);
                    }
                }
                else
                {
                    error = true;
                    _Logger.Log($"The directory '{jsFolder}' does not exist. There are no files to minify.");
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


        ///// <summary>
        ///// Minify the javascript files and rename back to usable names
        ///// Example: TrustedVendor.PM.PaymentCodesBehaviour.min.js --> TrustedVendor.PM.PaymentCodesBehaviour.js
        ///// </summary>
        //public void MinifyJavascriptFilesAndCleanup2()
        //{
        //    _Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

        //    var error = false;

        //    try
        //    {
        //        var jsCompressor = new JsMinifier(); // Douglas Crockford

        //        var workingFolder = _Folders.Staging.AreasScripts;
        //        var jsFolder = workingFolder;

        //        _Logger.Log($"jsFolder = {jsFolder}");
        //        if (Directory.Exists(jsFolder))
        //        {
        //            //var folders = Directory.EnumerateDirectories(jsFolder, "*.js", System.IO.SearchOption.AllDirectories);
        //            var folders = Directory.EnumerateDirectories(jsFolder);
        //            foreach (var folder in folders)
        //            {
        //                // Get a list of all files in the working javascript folder (Staging.AreasScripts)
        //                var jsFiles = Directory.EnumerateFiles(folder);
        //                foreach (var jsFile in jsFiles)
        //                {
        //                    var fileContentsMinified = string.Empty;

        //                    _Logger.Log($"Reading the contents of {jsFile}.");
        //                    var fileContents = File.ReadAllText(jsFile);

        //                    _Logger.Log($"Minifying the text...");
        //                    fileContentsMinified = jsCompressor.Minify(fileContents);

        //                    _Logger.Log($"Writing the minified content to {jsFile}.");
        //                    File.WriteAllText(jsFile, fileContentsMinified);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            error = true;
        //            _Logger.Log($"The directory '{jsFolder}' does not exist. There are no files to minify.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error = true;
        //        var msg = $"{Messages.Error_MinificationFailed}{Environment.NewLine}{ex.Message}";
        //        throw new MergeISVProjectException(_Logger, msg);
        //    }
        //    finally
        //    {
        //        if (!error)
        //        {
        //            _Logger.Log(Messages.Msg_MinificationSuccessful);
        //        }
        //        _Logger.LogMethodFooter(Utilities.GetCurrentMethod());
        //    }
        //}

        /// <summary>
        /// Copy minified javascript files to the Final Staging folder
        /// </summary>
        public void CopyToFinalStagingLocation()
        {
            var source = _Folders.Staging.Areas;
            var dest = _Folders.FinalWeb.Areas;
            FileSystem.CopyDirectory(source, dest, overwrite: true);
        }

        #endregion
    }
}
