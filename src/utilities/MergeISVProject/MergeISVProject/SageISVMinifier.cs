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
        private const string NUGLIFY_DLL = @"NUglify.dll";
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

                // Ensure that the Nuglify.dll file is available
                var tempPathToNUglify = Path.Combine(currentExePath, NUGLIFY_DLL);
                if (!File.Exists(tempPathToNUglify))
                {
                    _Logger.Log($"It looks like NUglify cannot be found.");

                    var msg = string.Format(Messages.Error_UnableToFindTheProgram, NUGLIFY_DLL, tempPathToNUglify);
                    throw new Exception(msg);
                }

                var pathToWG = Path.Combine(currentExePath, NUGLIFY_DLL);
                var workingFolder = _Folders.Staging.AreasScripts;
                var jsFolder = workingFolder;

                _Logger.Log($"jsFolder = {jsFolder}");
                if (Directory.Exists(jsFolder))
                {
                    // NUglify only does contents, so iteration is here (
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
                        Parallel.ForEach(Directory.GetFiles(dir, "*.js"), file =>
                        {
                            var content = File.ReadAllText(file);
                            var minified = NUglify.Uglify.Js(content);
                            if (minified.HasErrors)
                            {
                                foreach (var e in minified.Errors)
                                {
                                    _Logger.Log(e.ToString());
                                }
                            }
                            File.WriteAllText(file, minified.Code);
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
    }
}
