// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
        /// <remarks>args[5] is the optional parameter /nodeploy</remarks>
        static void Main(string[] args)
        {
            // Check for arguments
            if (args.Length < 5)
            {
                if (args.Length >= 2)
                {
                    WriteErrorFile(args[1], "The post-build utility MergeISVProject must be invoked with these parameters: {solutionFileName} {WebProjectPath} {MenuFileName} {ConfigurationName} {FrameworkDir} [/nodeploy]");
                }
                // Unsuccessful
                return;
            }

            var pathWebProj = args[1];
            var menuFileName = args[2];
            var configName = args[3];
            var pathFramework = args[4];
            var moduleId = menuFileName.Substring(0, menuFileName.IndexOf("MenuDetails.xml"));
            var pathSage300 = GetSage300Path();


            var nodeploy = false;  // flag indicating if the files should be deployed [copied] to the Sage 300 online folder
            if (args.Length == 6)
            {
               nodeploy = (args[5].ToLower() == "/nodeploy");
            }

            // Delete Error file, if exists
            DeleteErrorFile(pathWebProj);

            // Exit without error IF configuration is not Release OR Sage 300 folder does not exist (Sage 300 not installed).
            if (string.IsNullOrEmpty(pathSage300))
            {
                return;
            }

            if (configName.ToLower() != "release")
            {
                // Create [or empty if already present] the Web\Deploy folder
                var pathDeploy = Path.Combine(pathWebProj, "Deploy");
                if (Directory.Exists(pathDeploy))
                {
                    Directory.Delete(pathDeploy, true);
                }
                Directory.CreateDirectory(pathDeploy);

                // compile and copy files to web project deploy folder
                string compileResult = string.Empty;
                if (StageFiles(pathWebProj, menuFileName, pathFramework, ref compileResult))
                {
                    if (!nodeploy)
                    {
                        DeployFiles(pathWebProj, pathSage300, menuFileName);
                    }
                }
            } 
            else
            {
                if (!nodeploy)
                {
                    DeployFiles(pathWebProj, pathSage300, menuFileName, configName.ToLower());
                }

            }
        }

        /// <summary>
        /// Copy files from one folder to another.
        /// </summary>
        /// <param name="pathFrom">Source path</param>
        /// <param name="pattern">Copy pattern</param>
        /// <param name="pathTo">Destination path</param>
        /// <param name="overwrite">True to overwrite otherwise false</param>
        /// <param name="copyWebFile">True to copy *.web.dll otherwise false</param>
        private static void CopyFiles(string pathFrom, string pattern, string pathTo, bool overwrite, bool copyWebFile = true)
        {
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
                File.Copy(file, pathFile, overwrite);
            }
        }

        /// <summary>
        /// Compile the views and copy compiled files to web project deploy (staging) folder.
        /// </summary>
        /// <param name="pathWebProj">Web Project path</param>
        /// <param name="menuFileName">Menu file name</param>
        /// <param name="pathFramework">Framework path</param>
        private static bool StageFiles(string pathWebProj, string menuFileName, string pathFramework, ref string compileResult)
        {
            var pathDeploy = Path.Combine(pathWebProj, "Deploy");
            var pathSource = Path.Combine(pathDeploy, "Source");
            var pathBuild = Path.Combine(pathDeploy, "Build");
            var pathBinFrom = Path.Combine(pathWebProj, "Bin");
            var pathBinTo = Path.Combine(pathSource, "Bin");
            var pathAreas = Path.Combine(pathSource, "Areas");
            var moduleId = menuFileName.Substring(0, menuFileName.IndexOf("MenuDetails.xml"));
            var pathAreaViewsFrom = Path.Combine(pathWebProj, @"Areas\" + moduleId + @"\Views");
            var pathAreaViewsTo = Path.Combine(pathSource, @"Areas\" + moduleId + @"\Views");

            // prepare compiled directories and files
            Directory.CreateDirectory(pathBuild);
            Directory.CreateDirectory(pathSource);
            Directory.CreateDirectory(pathBinTo);
            Directory.CreateDirectory(pathAreas);
            Directory.CreateDirectory(pathAreaViewsTo);

            File.Copy(Path.Combine(pathWebProj, "Web.config"), Path.Combine(pathSource, "Web.config"));
            FileSystem.CopyDirectory(pathAreaViewsFrom, pathAreaViewsTo);

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
                    Arguments = string.Format(" -nologo -v / -p \"{0}\" -fixednames \"{1}\"", pathSource, pathBuild),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            p.Start();
            compileResult = p.StandardOutput.ReadToEnd();
            Console.WriteLine(compileResult);
            p.WaitForExit();
            
#if _maybe_later

            // CONSIDER FOR A FUTURE UPDATE.  
            // THE IDEA IS TO RETASK THE DEPLOY FOLDER AS A STAGING FOLDER THAT CONTAINS ALL FILES NEEDED 
            // IN ORDER TO DEPLOY A PROJECT (VIA COPY).
            // AT PRESENT THE WEB PROJECT DEPLOY FOLDER DOES NOT CONTAIN ALL REQUIRED FILES. 
            // IT ALSO CONTAINS EXTRA FILES THAT SHOULD NOT BE DEPLOYED WITH AN ISV PROJECT.
            
            // Copy bootstrapper file
            var bootstrapFileName = moduleId + "bootstrapper.xml";
            File.Copy(Path.Combine(pathWebProj, bootstrapFileName), Path.Combine(pathBuild, bootstrapFileName), true);

            // Copy menu file to App_Data\MenuDetail
            Directory.CreateDirectory(Path.Combine(pathBuild, "App_Data"));
            Directory.CreateDirectory(Path.Combine(pathBuild, @"App_Data\MenuDetail"));
            File.Copy(Path.Combine(pathWebProj, menuFileName), Path.Combine(pathBuild, @"App_Data\MenuDetail", menuFileName), true);

            // Copy the scripts folder
            var pathAreaScriptsFrom = Path.Combine(pathWebProj, @"Areas\" + moduleId + @"\Scripts");
            var pathAreaScriptsTo = Path.Combine(pathBuild, @"Areas\" + moduleId + @"\Scripts");
            Directory.CreateDirectory(pathAreaScriptsTo);
            FileSystem.CopyDirectory(pathAreaScriptsFrom, pathAreaScriptsTo);
#endif

            return p.ExitCode == 0;
        }

        /// <summary>
        /// Get Sage 300 install path
        /// </summary>
        /// <returns>Sage 300 install path</returns>
        private static string GetSage300Path()
        {
            const string sageRegKey = "SOFTWARE\\ACCPAC International, Inc.\\ACCPAC\\Configuration";
            const string sage64RegKey = "SOFTWARE\\WOW6432Node\\ACCPAC International, Inc.\\ACCPAC\\Configuration";

            var key = Registry.LocalMachine.OpenSubKey(sageRegKey) ?? Registry.LocalMachine.OpenSubKey(sage64RegKey);

            return (key == null) ? string.Empty : key.GetValue("Programs").ToString();
        }

        /// <summary>
        /// Copy bootStrapper, menuDetails and scripts files to the Sage Online folder
        /// </summary>
        /// <param name="pathWebProj">Web Project path</param>
        /// <param name="pathSage300">Sage 300 folder</param>
        /// <param name="menuFileName">Menu file name</param>
        /// <returns>True if successful otherwise false</returns>
        private static bool DeployFiles(string pathWebProj, string pathSage300, string menuFileName, string configuration = "release")
        {
            const string searchPattern = "*bootstrapper.xml";
            var pathSageOnline = Path.Combine(pathSage300, @"Online");
            var pathOnlineWeb = Path.Combine(pathSageOnline, "Web");
            var pathOnlineWorker = Path.Combine(pathSageOnline, "Worker");
            if (!Directory.Exists(pathOnlineWeb))
            {
                WriteErrorFile(pathWebProj, "The post-build utility MergeISVProject could not find the Online Web folder for the Web UIs. While the build was successful, the deployment was unsuccessful. Therefore, check view(s) for issue(s) (i.e. localization syntax).");
                // Unsuccessful
                return false;
            }

            // Copy bootstrapper.xml file
            var bootFiles = Directory.GetFiles(pathWebProj, searchPattern);
            foreach (var srcfile in bootFiles)
            {
                var fileName = Path.GetFileName(srcfile);
                if (fileName == null)
                {
                    continue;
                }
                var desFile = Path.Combine(pathOnlineWeb, fileName);
                File.Copy(srcfile, desFile, true);

                desFile = Path.Combine(pathOnlineWorker, fileName);
                File.Copy(srcfile, desFile, true);
            }

            // Copy menu file to App_Data menuDetails and all sub directory
            var pathMenuFrom = Path.Combine(pathWebProj, menuFileName);
            var pathMenuDir = Path.Combine(pathOnlineWeb, "App_Data", "MenuDetail");
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
            var pathAreaDir = Path.Combine(pathWebProj, "Areas");
            if (Directory.Exists(pathAreaDir))
            {
                foreach (var dir in Directory.GetDirectories(pathAreaDir))
                {
                    if (!dir.EndsWith("Core") && !dir.EndsWith("Shared"))
                    {
                        var paths = dir.Split('\\');
                        var pathSubArea = Path.Combine(pathOnlineWeb, "Areas", paths[paths.Length - 1]);
                        var pathFromScripts = Path.Combine(dir, "Scripts");
                        var pathToScripts = Path.Combine(pathSubArea, "Scripts");
                        FileSystem.CopyDirectory(pathFromScripts, pathToScripts, true);
                    }
                }
            }

            //copy files from deploy folder to sage online web area and/or bin directory
            var moduleId = menuFileName.Substring(0, menuFileName.IndexOf("MenuDetails.xml"));
            var pathSageView = Path.Combine(pathSageOnline, $@"Web\Areas\{moduleId}\Views");
            var pathSageBin = Path.Combine(pathOnlineWeb, "bin");

            if (configuration == "release")
            {
                //copy compiled files from deploy folder to sage online web area and bin directory
                var pathDeploy = Path.Combine(pathWebProj, "Deploy");
                var pathSource = Path.Combine(pathDeploy, "Source");
                var pathBuild = Path.Combine(pathDeploy, "Build");
                var pathBuildView = Path.Combine(pathBuild, @"Areas\" + moduleId + @"\Views");

                // Do not copy IF compile was not successful (determined by existance of folder)
                if (!Directory.Exists(pathBuildView))
                {
                    WriteErrorFile(pathWebProj, "The post-build utility MergeISVProject could not compile the razor view(s). While the build was successful, the deployment was unsuccessful. Therefore, check view(s) for issue(s) (i.e. localization syntax).");
                    // Unsuccessful
                    return false;
                }

                FileSystem.CopyDirectory(pathBuildView, pathSageView, true);

                var pathBuildBin = Path.Combine(pathBuild, "bin");

                string[] ps = { "*.compiled", "App_Web_*.dll", "*.Web.dll", "*." + moduleId + ".*.dll" };
                foreach (var pattern in ps)
                {
                    CopyFiles(pathBuildBin, pattern, pathSageBin, true);
                }

                string[] psWorker = { "*." + moduleId + ".*.dll" };
                foreach (var pattern in psWorker)
                {
                    CopyFiles(pathBuildBin, pattern, pathOnlineWorker, true, false);
                }
            }
            else
            {
                // copy uncompiled views from deploy folder to sage online web area
                var pathWebProjView = Path.Combine(pathWebProj, $@"Areas\{moduleId}\Views");

                FileSystem.CopyDirectory(pathWebProjView, pathSageView, true);

                var pathWebProjBin = Path.Combine(pathWebProj, "bin");

                string[] ps = { "*.compiled", "App_Web_*.dll", "*.Web.dll", "*." + moduleId + ".*.dll" };
                foreach (var pattern in ps)
                {
                    CopyFiles(pathWebProjBin, pattern, pathSageBin, true);
                }

                string[] psWorker = { "*." + moduleId + ".*.dll" };
                foreach (var pattern in psWorker)
                {
                    CopyFiles(pathWebProjBin, pattern, pathOnlineWorker, true, false);
                }
            }

            // copy resource satellite dlls for localization
            CopyResourceSatelliteFiles(pathWebProj, pathSage300, moduleId);

            return true;
        }

        /// <summary>
        /// Copy resource satellite dlls to web bin and worker folder
        /// </summary>
        /// <param name="pathWebProj">source file path</param>
        /// <param name="pathSage300">Sage 300 folder</param>
        /// <param name="moduleId">module id</param>
        private static void CopyResourceSatelliteFiles(string pathFrom, string pathSage300, string moduleId)
        {
            string[] languages = { "es", "fr-CA", "zh-Hans", "zh-Hant"};
            var pathBinFrom = Path.Combine(pathFrom, "Bin");
            var pathWebBinTo = Path.Combine(pathSage300, @"Online\Web\Bin");
            var pathWorkerTo = Path.Combine(pathSage300, @"Online\Worker");
            var pattern = "*." + moduleId + ".*.dll";

            foreach (var language in languages)
            {
                var fromFolder = Path.Combine(pathBinFrom, language);
                var toWebFolder = Path.Combine(pathWebBinTo, language);
                var toWorkerFolder = Path.Combine(pathWorkerTo, language);

                foreach (var file in Directory.GetFiles(fromFolder, pattern))
                {
                    var fileName = Path.GetFileName(file);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var pathFile = Path.Combine(toWebFolder, fileName);
                        File.Copy(file, pathFile, true);
                        pathFile = Path.Combine(toWorkerFolder, fileName);
                        File.Copy(file, pathFile, true);
                    }
                }
            }
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
