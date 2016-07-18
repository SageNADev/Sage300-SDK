// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace MergeISVProject
{
    class Program
    {
        /// <summary>
        /// Main Entry Point
        /// </summary>
        /// <remarks>args[0] is the solution file name</remarks>
        /// <remarks>args[1] is the Web project path</remarks>
        /// <remarks>args[2] is the menu file name (i.e. XXMenuDetails.xml) </remarks>
        /// <remarks>args[3] is the configuration name (i.e. debug or release)</remarks>
        /// <remarks>args[4] is the framework directory containing the aspnet_compile.exe</remarks>
        static void Main(string[] args)
        {
            // Check for arguments
            if (args.Length < 5)
            {
                if (args.Length >= 2)
                {
                    WriteErrorFile(args[1], "The post-build utility MergeISVProject must be invoked with these parameters:{solutionFileName} {WebProjectPath} {MenuFileName} {ConfigurationName} {FrameworkDir}");
                }
                // Unsuccessful
                return;
            }

            var pathFrom = args[1];
            var menuFileName = args[2];
            var configName = args[3];
            var pathFramework = args[4];
            var moduleId = menuFileName.Substring(0, 2);
            var pathSageWeb = GetSage300OnlinePath();

            // Delete Error file, if exists
            DeleteErrorFile(pathFrom);

            // Exit without error IF configuration is nor Release OR Sage Online folder does not exist (Sage 300 Web UI's not installed)
            if (configName.ToLower() != "release" || string.IsNullOrEmpty(pathSageWeb))
            {
                return;
            }

            // compile the views and copy to sage online web
            if (CopyFiles(pathFrom, pathSageWeb, menuFileName))
            {
                CompiledCopyFiles(pathFrom, moduleId, pathSageWeb, pathFramework);
            }
        }

        /// <summary>
        /// Copy files to Sage online web directories
        /// </summary>
        /// <param name="pathFrom">Source path</param>
        /// <param name="pattern">Copy pattern</param>
        /// <param name="pathTo">Destination path</param>
        /// <param name="overwrite">True to overwrite otherwise false</param>
        private static void CopyFiles(string pathFrom, string pattern, string pathTo, bool overwrite)
        {
            foreach (var file in Directory.GetFiles(pathFrom, pattern))
            {
                var fileName = Path.GetFileName(file);
                if (fileName == null || fileName.Equals("CrystalDecisions.Web.dll"))
                {
                    continue;
                }
                var pathFile = Path.Combine(pathTo, fileName);
                File.Copy(file, pathFile, overwrite);
            }
        }

        /// <summary>
        /// Compiled the views and copy compiled files to Sage online web directories
        /// </summary>
        /// <param name="pathFrom">Source path</param>
        /// <param name="moduleId">Module Id</param>
        /// <param name="pathSageWeb">Sage web path</param>
        /// <param name="pathFramework">Framework path</param>
        private static void CompiledCopyFiles(string pathFrom, string moduleId, string pathSageWeb, string pathFramework)
        {
            var pathDeploy = Path.Combine(pathFrom, "Deploy");
            if (Directory.Exists(pathDeploy))
            {
                Directory.Delete(pathDeploy, true);
            }
            Directory.CreateDirectory(pathDeploy);

            var pathSource = Path.Combine(pathDeploy, "Source");
            var pathBuild = Path.Combine(pathDeploy, "Build");
            var pathBinFrom = Path.Combine(pathFrom, "Bin");
            var pathBinTo = Path.Combine(pathSource, "Bin");
            var pathAreas = Path.Combine(pathSource, "Areas");
            var pathAreaFrom = Path.Combine(pathFrom, @"Areas\" + moduleId + @"\Views");
            var pathAreaTo = Path.Combine(pathSource, @"Areas\" + moduleId + @"\Views");

            // prepare compiled directories and files
            Directory.CreateDirectory(pathSource);
            Directory.CreateDirectory(pathBinTo);
            Directory.CreateDirectory(pathAreas);
            Directory.CreateDirectory(pathAreaTo);

            File.Copy(Path.Combine(pathFrom, "Web.config"), Path.Combine(pathSource, "Web.config"));
            FileSystem.CopyDirectory(pathAreaFrom, pathAreaTo);

            string[] patterns = { "System.*.dll", "Sage.CA.SBS.ERP.*.dll", "*.Web.Infrastructure.dll", "*.Web.dll", "*." + moduleId + ".*.dll" };
            foreach (var pattern in patterns)
            {
                CopyFiles(pathBinFrom, pattern, pathBinTo, true);
            }

            // compile the razors views 
            var p = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(pathFramework, "aspnet_compiler.exe"),
                    Arguments = string.Format(" -v / -p \"{0}\" -fixednames \"{1}\"", pathSource, pathBuild)
                }
            };
            p.Start();
            p.WaitForExit();

            //copy compiled files to sage online web area and bin directory
            var pathBuildView = Path.Combine(pathBuild, @"Areas\" + moduleId + @"\Views");
            var pathSageView = Path.Combine(pathSageWeb, @"Online\Web\Areas\" + moduleId + @"\Views");

            // Do not copy IF compile was not successful (determined by existance of folder)
            if (!Directory.Exists(pathBuildView))
            {
                WriteErrorFile(pathFrom, "The post-build utility MergeISVProject could not compile the razor view(s). While the build was successful, the deployment was unsuccessful. Therefore, check view(s) for issue(s) (i.e. localization syntax).");
                // Unsuccessful
                return;
            }

            FileSystem.CopyDirectory(pathBuildView, pathSageView, true);

            var pathBuildBin = Path.Combine(pathBuild, "bin");
            var pathSageBin = Path.Combine(pathSageWeb, @"Online\Web\bin");

            string[] ps = { "*.compiled", "App_Web_*.dll", "*.Web.dll", "*." + moduleId + ".*.dll" };
            foreach (var pattern in ps)
            {
                CopyFiles(pathBuildBin, pattern, pathSageBin, true);
            }

            // remove temp deploy build directory
            // Directory.Delete(pathDeploy, true);
        }

        /// <summary>
        /// Get Sage online installed path
        /// </summary>
        /// <returns>Sage 300 online path</returns>
        private static string GetSage300OnlinePath()
        {
            const string sageRegKey = "SOFTWARE\\ACCPAC International, Inc.\\ACCPAC\\Configuration";
            const string sage64RegKey = "SOFTWARE\\WOW6432Node\\ACCPAC International, Inc.\\ACCPAC\\Configuration";

            var key = Registry.LocalMachine.OpenSubKey(sageRegKey) ?? Registry.LocalMachine.OpenSubKey(sage64RegKey);

            return (key == null) ? string.Empty : key.GetValue("Programs").ToString();
        }

        /// <summary>
        /// Copy bootStrapper, menuDetails and scripts files
        /// </summary>
        /// <param name="pathFrom">Source path</param>
        /// <param name="pathTo">Destination path</param>
        /// <param name="menuFileName">menu file name</param>
        /// <returns>True if successful otherwise false</returns>
        private static bool CopyFiles(string pathFrom, string pathTo, string menuFileName)
        {
            const string searhPattern = "*bootstrapper.xml";
            var pathWeb = Path.Combine(pathTo, "OnLine", "Web");
            if (!Directory.Exists(pathWeb))
            {
                WriteErrorFile(pathFrom, "The post-build utility MergeISVProject could not find the Online Web folder for the Web UIs. While the build was successful, the deployment was unsuccessful. Therefore, check view(s) for issue(s) (i.e. localization syntax).");
                // Unsuccessful
                return false;
            }

            // Copy bootstrapper.xml file
            var bootFiles = Directory.GetFiles(pathFrom, searhPattern);
            foreach (var srcfile in bootFiles)
            {
                var fileName = Path.GetFileName(srcfile);
                if (fileName == null)
                {
                    continue;
                }
                var desFile = Path.Combine(pathWeb, fileName);
                File.Copy(srcfile, desFile, true);
            }

            // Copy menu file to App_Data menuDetails and all sub directory
            var pathMenuFrom = Path.Combine(pathFrom, menuFileName);
            var pathMenuDir = Path.Combine(pathWeb, "App_Data", "MenuDetail");
            var pathMenuTo = Path.Combine(pathMenuDir, menuFileName);

            if (Directory.Exists(pathMenuDir))
            {
                File.Copy(pathMenuFrom, pathMenuTo, true);
                foreach (var dir in Directory.GetDirectories(pathMenuDir))
                {
                    var pathMenuSubTo = Path.Combine(dir, menuFileName);
                    File.Copy(pathMenuFrom, pathMenuSubTo, true);
                }
            }

            // Copy areas scripts files
            var pathAreaDir = Path.Combine(pathFrom, "Areas");
            if (Directory.Exists(pathAreaDir))
            {
                foreach (var dir in Directory.GetDirectories(pathAreaDir))
                {
                    if (!dir.EndsWith("Core") && !dir.EndsWith("Shared"))
                    {
                        var paths = dir.Split('\\');
                        var pathSubArea = Path.Combine(pathWeb, "Areas", paths[paths.Length - 1]);
                        var pathFromScripts = Path.Combine(dir, "Scripts");
                        var pathToScripts = Path.Combine(pathSubArea, "Scripts");
                        FileSystem.CopyDirectory(pathFromScripts, pathToScripts, true);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Delete Compile Error Log
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Fully qualified file name</returns>
        private static string DeleteErrorFile(string path)
        {
            var fileName = Path.Combine(path, "MVCBuildViewsError.txt");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return fileName;
        }

        /// <summary>
        /// Write Compile Error Log
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="message">Error message</param>
        private static void WriteErrorFile(string path, string message)
        {
            try
            {
                var fileName = DeleteErrorFile(path);

                File.AppendAllLines(fileName, new[] { DateTime.Now + ": " + message });
            }
            catch
            {
                // Ignore error
            }

        }

    }
}
