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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#endregion

namespace MergeISVProject
{
    /// <summary>
    /// Minification handler class
    /// </summary>
    public class SageISVMinifier
    {
        #region Constants
        private const string NODEJS = @"Node.js";
        private const string TERSER = @"terser";
        private const string MinifyCommand = "node %AppData%\\npm\\node_modules\\terser\\bin {0} -o {1}";
        #endregion

        #region Private Variables
        private readonly ILogger _Logger = null;
        private readonly FolderManager _Folders;
        private readonly string _ModuleId = string.Empty;
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

        #region Public Methods

        /// <summary>
        /// Minify the javascript files
        /// </summary>
        public void MinifyJavascriptFilesAndCleanup()
        {
            _Logger.LogMethodHeader($"{this.GetType().Name}.{Utilities.GetCurrentMethod()}");

            var error = false;

            try
            {
                var currentExePath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

                // Check if Node.js is installed
                if (!ExecuteCommand("where node"))
                {
                    _Logger.Log($"It looks like Node.js cannot be found.");

                    var msg = string.Format(Messages.Error_UnableToFindTheProgram, NODEJS, string.Empty);
                    throw new Exception(msg);
                }

                // Install Terser via Node.js
                if (!ExecuteCommand("npm install -g terser"))
                {
                    _Logger.Log($"It looks like terser cannot be installed.");

                    var msg = string.Format(Messages.Error_UnableToFindTheProgram, TERSER, string.Empty);
                    throw new Exception(msg);
                }

                var workingFolder = _Folders.Staging.AreasScripts;
                var jsFolder = workingFolder;

                _Logger.Log($"jsFolder = {jsFolder}");
                if (Directory.Exists(jsFolder))
                {
                    // Terser only does files, so iteration is here (
                    foreach (var dir in Directory.GetDirectories(jsFolder, "*.*", System.IO.SearchOption.AllDirectories))
                    {
                        _Logger.Log($"Processing directory '{dir}'");

                        var files = Directory.GetFiles(dir);
                        if (files.Count() == 0)
                        {
                            _Logger.Log($"No files found in folder '{dir}'. Skipping to next directory in list.");
                            continue;
                        }

                        _Logger.Log(string.Format(Messages.Msg_BeginningMinificationProcessOnDirectory, dir));

                        Parallel.ForEach(files, file =>
                        {
                            var command = string.Format(MinifyCommand, file, file);
                            ExecuteCommand(command);
                        });

                        _Logger.Log(Messages.Msg_MinificationComplete);
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

        #region Private Methods

        /// <summary>
        /// Execute commnad line for minifying JS files
        /// </summary>
        /// <param name="command">comamnd line</param>
        private bool ExecuteCommand(string command)
        {
            var methodName = $"{this.GetType().Name}.{Utilities.GetCurrentMethod()}";
            _Logger.LogMethodHeader(methodName);

            var success = false;

            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/C " + command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                success = process.ExitCode == 0;
                if (!success)
                {
                    _Logger.Log("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                //handle your exception...
                _Logger.Log("Minify Sage300 JavaScript files failure!");
                _Logger.Log("Error: " + ex.Message);
            }

            _Logger.LogMethodFooter(methodName);

            return success;
        }

        #endregion
    }
}
