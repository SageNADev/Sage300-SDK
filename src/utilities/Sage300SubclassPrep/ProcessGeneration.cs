// The MIT License (MIT) 
// Copyright (c) 1994-2025 The Sage Group plc or its licensors.  All rights reserved.
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

using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace Sage.CA.SBS.ERP.Sage300.SubclassPrep
{
    /// <summary> Process Generation Class (worker) </summary>
    internal class ProcessGeneration
    {
        #region Public constants
        public class Constants
        {
            /// <summary> Property for Id </summary>
            public const string ELEMENT_COMPILE = "Compile";

            /// <summary> Module Segment </summary>
            public const int MODEL_SEGMENT = 5;

            /// <summary> Environment web source var </summary>
            public const string CNA2_SOURCE_ROOT = "CNA2_SOURCE_ROOT";

            /// <summary> Environment sdk source var </summary>
            public const string SDK_SOURCE_ROOT = "SDK_SOURCE_ROOT";

            /// <summary> Environment suggestion for web source var i.e.,  C:\Development\Branches\Dev\ </summary>
            public const string CNA2_SOURCE_ROOT_SUGGESTION = @"C:\Development\Branches\Dev\";

            /// <summary> Environment suggestion for sdk source var i.e.,  C:\Development\Branches\SDK\ </summary>
            public const string SDK_SOURCE_ROOT_SUGGESTION = @"C:\Development\Branches\SDK\";

            /// <summary> Repo Prefix </summary>
            public const string REPO_PREFIX = "Columbus-";

            /// <summary> Repo Framework </summary>
            public const string REPO_FRAMEWORK = "Framework";

            /// <summary> Namespace Segments </summary>
            public const string NAMESPACE_SEGMENTS = "Sage.CA.SBS.ERP.Sage300.";

            /// <summary> Models Segment </summary>
            public const string MODELS_SEGMENT = ".Models";

            /// <summary> Project extension </summary>
            public const string PROJECT_EXTENSION = "*.csproj";

            /// <summary> Zip extension </summary>
            public const string ZIP_EXTENSION = ".zip";

            /// <summary> CS extension </summary>
            public const string CS_EXTENSION = ".cs";

            /// <summary> SDK folder for JSON </summary>
            public const string SDK_FOLDER = @"Sage300-SDK\src\wizards\Sage300SubclassConfigsWizard\Resources";

            /// <summary> Tools folder for zip files </summary>
            public const string TOOLS_FOLDER = @"Sage300-SDK\src\wizards\Sage300SubclassCompilerWizard\Resources";

            /// <summary> JSON file name </summary>
            public const string SDK_JSON_NAME = "ModelsSource.json";

            /// <summary> Include attribute </summary>
            public const string INCLUDE_ATTR = "Include";

            /// <summary> SKIP enums </summary>
            public const string SKIP_ENUMS = @"Enums\";

            /// <summary> SKIP fields </summary>
            public const string SKIP_FIELDS = @"Fields\";

            /// <summary> SKIP properties </summary>
            public const string SKIP_PROPERTIES = @"Properties\";

            /// <summary> Bin folder </summary>
            public const string BIN_FOLDER = "bin";

            /// <summary> Obj folder </summary>
            public const string OBJ_FOLDER = "obj";

        }
        #endregion

        #region Public Delegates

        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="text">Text for UI</param>
        public delegate void StatusEventHandler(string fileName, string text);

        #endregion

        #region Public Events

        /// <summary> Event to update UI with name of file being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update UI with status of file being processed </summary>
        public event StatusEventHandler StatusEvent;

        #endregion

        #region Public Methods

        /// <summary> Get the CNA2_SOURCE_ROOT environment variable </summary>
        public static string WebSourceRoot()
        {
            return Environment.GetEnvironmentVariable(Constants.CNA2_SOURCE_ROOT);
        }

        /// <summary> Get the SDK_SOURCE_ROOT environment variable </summary>
        public static string SDKSourceRoot()
        {
            return Environment.GetEnvironmentVariable(Constants.SDK_SOURCE_ROOT);
        }

        /// <summary> Start the generation process </summary>
        public void Process()
        {
            // Get all project files
            var projects = Projects();
            var models = new Dictionary<string, List<string>>();

            // Iterate projects 
            foreach (var project in projects)
            {
                // Project being processed
                var fileName = Path.GetFileName(project);
                var tmp = fileName.Split('.');
                var module = tmp[Constants.MODEL_SEGMENT];

                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Generate zip file for application tool Sage300SubclassCompiler
                var ret = ZipProject(project, module);
                if (!string.IsNullOrEmpty(ret))
                {
                    // Failure. Update status
                    LaunchStatusEvent(fileName, ret);
                    break;
                }

                // Build JSON for SDK Utility Sage300SubclassConfigsWizard
                ret = BuildModelsSource(project, module, models);
                if (!string.IsNullOrEmpty(ret))
                {
                    // Failure. Update status
                    LaunchStatusEvent(fileName, ret);
                    break;
                }

                // Success. Update status
                LaunchStatusEvent(fileName, string.Empty);
            }

            // Done. Now serialize to JSON and save to SDK folder for Sage300SubclassConfigsWizard
            var json = JsonConvert.SerializeObject(models);
            var path = Path.Combine(SDKSourceRoot(), Constants.SDK_FOLDER, Constants.SDK_JSON_NAME);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            File.WriteAllText(path, json);
        }

        #endregion

        #region Private methods

        /// <summary> Zip project </summary>
        /// <param name="project">Project</param>
        /// <param name="module">Module</param>
        /// <returns>string.Empty if success else error message</returns>
        private static string ZipProject(string project, string module)
        {
            // Local
            string ret = string.Empty;

            try
            {
                var fileInfo = new FileInfo(project);
                var sourcePath = fileInfo.DirectoryName;
                var toFile = Path.Combine(SDKSourceRoot(), Constants.TOOLS_FOLDER, module + Constants.ZIP_EXTENSION);
                
                // Delete if To file exists
                if (File.Exists(toFile))
                {
                    File.Delete(toFile);
                }

                // Before zipping, ensure any obj or bin folders are deleted
                var binFolder = Path.Combine(sourcePath, Constants.BIN_FOLDER);
                if (Directory.Exists(binFolder))
                {
                    Directory.Delete(binFolder, true);
                }
                var objFolder = Path.Combine(sourcePath, Constants.OBJ_FOLDER);
                if (Directory.Exists(objFolder))
                {
                    Directory.Delete(objFolder, true);
                }

                // Create zip file
                ZipFile.CreateFromDirectory(sourcePath, toFile, CompressionLevel.Optimal, true);

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        /// <summary> Build Models source </summary>
        /// <param name="project">Project</param>
        /// <param name="module">Module</param>
        /// <param name="models">Dictionary of models</param>
        /// <returns>string.Empty if success else error message</returns>
        /// <remarks>models is being built on every call per project</remarks>
        private static string BuildModelsSource(string project, string module,
            Dictionary<string, List<string>> models)
        {
            // Local
            string ret = string.Empty;

            try
            {
                var files = new List<string>();

                // Open file (csproj)
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(project);

                // Get all elements of element type Compile
                var elementList = xmlDocument.GetElementsByTagName(Constants.ELEMENT_COMPILE);

                // Iterate list and filter out enums, non-cs files, etc.
                for (int i = 0; i < elementList.Count; i++)
                {
                    var file = elementList[i].Attributes[Constants.INCLUDE_ATTR].Value;

                    // Skip non *.cs files
                    if (!file.EndsWith(Constants.CS_EXTENSION))
                    {
                        continue;
                    }

                    // Skip Enums, Fields, Properties folders
                    if (file.StartsWith(Constants.SKIP_ENUMS) || 
                        file.StartsWith(Constants.SKIP_FIELDS) || 
                        file.StartsWith(Constants.SKIP_PROPERTIES))
                    {
                        continue;
                    }

                    // All other files to be included
                    files.Add(file.Replace(Constants.CS_EXTENSION, ""));
                }

                // Sort and add to dictionary
                files.Sort();
                models.Add(module, files);

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        /// <summary> Gets projects for various usages </summary>
        /// <returns>List of Sage 300 projects (csproj files)</returns>
        private static List<string> Projects()
        {
            var projects = new List<string>();

            // Iterate modules and get Models project 
            foreach (
                var moduleType in
                Enum.GetValues(typeof(ModuleType))
                    .Cast<ModuleType>())
            {
                // BK and TX are included in CS projects, so no project
                // Also, do not include PM in 2026
                if (!moduleType.Equals(ModuleType.BK) & !moduleType.Equals(ModuleType.TX)
                    & !moduleType.Equals(ModuleType.PM))
                {
                    var path = Path.Combine(WebSourceRoot(), Constants.REPO_PREFIX +
                    moduleType, Constants.NAMESPACE_SEGMENTS + moduleType + Constants.MODELS_SEGMENT);
                    projects.AddRange(Directory.GetFiles(path, Constants.PROJECT_EXTENSION));
                }
            }

            return projects;
        }


        /// <summary> Update UI </summary>
        /// <param name="fileName">Name of file to be created</param>
        /// <param name="failureMessage">Failure message</param>
        private void LaunchStatusEvent(string fileName, string failureMessage = null)
        {
            // Return if no subscriber
            if (StatusEvent == null)
            {
                return;
            }

            // Update
            StatusEvent(fileName, failureMessage);
        }

        /// <summary> Update UI </summary>
        /// <param name="fileName">Name of file to be created</param>
        private void LaunchProcessingEvent(string fileName)
        {
            // Event if subscriber
            if (ProcessingEvent == null)
            {
                return;
            }

            ProcessingEvent(fileName);
        }

        #endregion
    }
}
