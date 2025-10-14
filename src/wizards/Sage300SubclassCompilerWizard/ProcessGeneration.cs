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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard.Properties;
using Newtonsoft.Json.Linq;

namespace Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard
{
    /// <summary> Process Generation Class (worker) </summary>
    internal class ProcessGeneration
    {
        #region Private Variables

        /// <summary> Settings from UI </summary>
        private Settings _settings;

        #endregion

        #region Public Constants
        public class Constants
        {
            /// <summary> Property for Id </summary>
            public const string PropertyId = "Id";

            /// <summary> Property for Name </summary>
            public const string PropertyName = "Name";

            /// <summary> Property for BusinessPartnerName </summary>
            public const string PropertyBusinessPartnerName = "BusinessPartnerName";

            /// <summary> Property for Module ID </summary>
            public const string PropertyModuleId = "ModuleId";

            /// <summary> Property for Model </summary>
            public const string PropertyModel = "Model";

            /// <summary> Moduel Segment </summary>
            public const int ModuleSegment = 5;

            /// <summary> Projects Folder </summary>
            public const string ProjectsFolder = "Sage300Projects";

            /// <summary> Build Folder </summary>
            public const string BuildFolder = "Sage300Build";

            /// <summary> Backup Folder </summary>
            public const string BackupFolder = "Sage300Backups";

            ///<summary>Visual Studio Build file name</summary>
            public const string VSBuildFileName = "VSBuild.bat";

            ///<summary>.NET SDK Build file name</summary>
            public const string SDKBuildFileName = "SDKBuild.bat";

            ///<summary>Build file extension</summary>
            public const string BuildFileExt = "Build.bat";

            ///<summary>Build log file</summary>
            public const string BuildLogFile = "BuildModel.log";

            ///<summary>Visual Studio Compilers batch file</summary>
            public const string VSCompilers = "VSCompilers.bat";

            ///<summary>.NET SDK Compilers batch file</summary>
            public const string SDKCompilers = "SDKCompilers.bat";

            ///<summary>Zip extension</summary>
            public const string ExtensionZip = ".zip";

            ///<summary>csproj extension</summary>
            public const string ExtensionCsProj = "csproj";

            ///<summary>dll extension</summary>
            public const string ExtensionDll = "dll";

            ///<summary>csproj extension</summary>
            public const string ExtensionAllCsProj = "*.csproj";

            ///<summary>csproj extension</summary>
            public const string ExtensionModelsCsProj = ".Models.csproj";

            ///<summary>cs extension</summary>
            public const string ExtensionCs = ".cs";

            ///<summary>json extension</summary>
            public const string ExtensionAllJson = "*.json";

            ///<summary><WebHintPath></summary>
            public const string ElementWebHintStart = @"<WebHintPath>";

            ///<summary></WebHintPath></summary>
            public const string ElementWebHintEnd = @"</WebHintPath>";

            /// <summary> Property for Properties </summary>
            public const string PropertyProperties = "Properties";

            /// <summary> Property for FieldName </summary>
            public const string PropertyFieldName = "FieldName";

            /// <summary> Property for FieldType </summary>
            public const string PropertyFieldType = "FieldType";

            /// <summary> Property for DataType </summary>
            public const string PropertyDataType = "DataType";

            /// <summary> Property for Mask </summary>
            public const string PropertyMask = "Mask";

            /// <summary> Property for Size </summary>
            public const string PropertySize = "Size";

            /// <summary> Property for Precision </summary>
            public const string PropertyPrecision = "Precision";

            /// <summary> Token for public </summary>
            public const string TokenPublic = "public";

            /// <summary> Token for class </summary>
            public const string TokenClass = "class";

            /// <summary> Token for comment </summary>
            public const string TokenComment = @"//";

            /// <summary> Token for brace </summary>
            public const string TokenBrace = @"{";

            /// <summary> CommentCompanyName </summary>
            public const string CommentCompanyName = @"// Subclassing for ";

            /// <summary> CommentConfigName </summary>
            public const string CommentConfigName = " with configuration ";

            /// <summary> CodeAttribute </summary>
            public const string CodeAttribute = "[ViewField(Name = \"{0}\", Id = {1}, FieldType = EntityFieldType.{2}, Size = {3}, Precision = {4}, Mask = \"{5}\", Extended = true)]";
			
            /// <summary> CodeProperty </summary>
            public const string CodeProperty = "public {0} {1} ";

            /// <summary> CodePropertyGetSet </summary>
            public const string CodePropertyGetSet = "{ get; set; }";

            /// <summary> Visual Studio Compiler search path </summary>
            public const string VSCompilerPath = @"C:\Program Files (x86)\Microsoft Visual Studio\Installer";

            /// <summary> Visual Studio Token </summary>
            public const string VSToken = "resolvedInstallationPath";

            /// <summary> Visual Studio Key </summary>
            public const string VSKey = "VS";

            /// <summary> .NET SDK Compiler search path </summary>
            public const string SDKCompilerPath = @"C:\Program Files\dotnet\sdk";

            /// <summary> SDK Start Token </summary>
            public const string SDKStartToken = ".NET SDKs installed:";

            /// <summary> SDK End Token </summary>
            public const string SDKEndToken = ".NET runtimes installed:";

            /// <summary> SDK Key </summary>
            public const string SDKKey = "SDK";

            /// <summary> MSBuild SubFolders </summary>
            public const string MSBuildSubFolders = @"\MSBuild\Current\Bin";

            /// <summary> Models.Attributes assembly reference </summary>  
            public const string UsingAttributes = "using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;";

            /// <summary> Namespace line </summary>  
            public const string NamespaceLine = "namespace";
        }
        #endregion

        #region Public Delegates

        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        public delegate void StatusEventHandler(string fileName, Info.StatusType statusType, string text);

        #endregion

        #region Public Events

        /// <summary> Event to update UI with name of file being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update UI with status of file being processed </summary>
        public event StatusEventHandler StatusEvent;

        #endregion

        #region Public Methods

        /// <summary> Get compilers on machine </summary>
        /// <param name="compilers">Dictionary of available compilers</param>
        /// <remarks>Presedence will be to use VS compiler (msbuild) over 
        /// .NET SDK compiler (dotnet build). Compiler in .NET Framework is 
        /// insufficient (< 6 version)</remarks>
        public static void GetCompilers(Dictionary<string, List<Compiler>> compilers)
        {
            // Clear first
            compilers.Clear();

            // Get Projects folder
            var path = GetProjectsFolder();

            // Look for Visual Studio compilers
            if (Directory.Exists(Constants.VSCompilerPath))
            {
                // This folder exists and therefore, Visual Studio 'should' as well
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(path, Constants.VSCompilers))
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        Arguments = $"\"{Constants.VSCompilerPath}\""
                    };

                    Process process = new Process
                    {
                        StartInfo = startInfo
                    };
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();

                    // Gather Visual Studio verion(s)
                    compilers.Add(Constants.VSKey, FindVSVersions(output));
                }
                catch
                {
                    // Swallow any errors at this point
                }
            }

            // Look for .NET SDK compilers
            if (Directory.Exists(Constants.SDKCompilerPath))
            {
                // This folder exists and therefore, .NET SDK(s) 'should' as well
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(path, Constants.SDKCompilers))
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Minimized,
                        Arguments = $"\"{Constants.SDKCompilerPath}\""
                    };

                    Process process = new Process
                    {
                        StartInfo = startInfo
                    };
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();

                    // Gather .NET SDK verion(s)
                    compilers.Add(Constants.SDKKey, FindSDKVersions(output));
                }
                catch
                {
                    // Swallow any errors at this point
                }
            }

        }

        /// <summary> Start the generation process </summary>
        /// <param name="settings">Settings for processing</param>
        public void Process(Settings settings)
        {
            // Begin process
            _settings = settings;

            // Projects folder and model projects in the folder
            var path = GetProjectsFolder();
            var projects = GetModelProjects(path);

            // Get backup folder name
            var backupFolder = GetBackupFolder();

            // Get build folder name
            var buildFolder = GetBuildFolder();
            CreateDirectory(buildFolder, true, true);

            // Iterate projects 
            foreach (var project in projects)
            {
                // Update display of file being processed
                LaunchProcessingEvent(Path.GetFileName(project));
                var valid = string.Empty;

                // Modify model to add configuration(s)
                // If subclassing configurations are to be ignored, then 
                // there is no reasonn to modify the project as it will be
                // simply be built with Sage only properties
                if (!_settings.IgnoreConfigurations)
                {
                    valid = ModifyModel(project);
                    if (!string.IsNullOrEmpty(valid))
                    {
                        // Failure. Update status
                        LaunchStatusEvent(false, Path.GetFileName(project), valid);
                        break;
                    }
                }

                // Build project
                valid = BuildProject(project, path, buildFolder);
                if (!string.IsNullOrEmpty(valid))
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, Path.GetFileName(project), valid);
                    break;
                }

                // Deploy assembly
                valid = DeployAssembly(project, backupFolder, buildFolder);
                if (!string.IsNullOrEmpty(valid))
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, Path.GetFileName(project), valid);
                    break;
                }

                // Success. Update status
                LaunchStatusEvent(true, Path.GetFileName(project));
            }

            // Done. Start the worker service
            WorkerService.Start();
        }

        /// <summary> Create projects </summary>
        public static void CreateProjects()
        {
            // Get Projects folder
            var path = GetProjectsFolder();

            // Create Projects folder in machine's temp folder
            CreateDirectory(path);

            // Save SDKCompilers.bat and VSCompilers.bat to path
            File.WriteAllText(Path.Combine(path, Constants.SDKCompilers), Resources.SDKCompilers);
            File.WriteAllText(Path.Combine(path, Constants.VSCompilers), Resources.VSCompilers);

            // Save build files to path
            File.WriteAllText(Path.Combine(path, Constants.VSBuildFileName), Resources.VSBuild);
            File.WriteAllText(Path.Combine(path, Constants.SDKBuildFileName), Resources.SDKBuild);

            // Iterate modules and unzip Model Project to path
            UnzipAndSave(path);

            // Get all project files for all modules to prep for hint path
            PrepWebHintPath(path);
        }

        /// <summary> Get existing JSON Configurations</summary>
        /// <returns>List of configurations</returns>
        public static List<Configuration> GetExistingConfigurations()
        {
            // Local
            var configurations = new List<Configuration>();

            // Create directory if does not exist
            CreateDirectory(RegistryHelper.Sage300SubclassingFolder, false);

            // Get any configurations found
            var files = GetFiles(RegistryHelper.Sage300SubclassingFolder, Constants.ExtensionAllJson);

            // Iterate through each configuration file found
            foreach (var file in files)
            {
                // Get the configuration
                var json = JObject.Parse(File.ReadAllText(file));
                // Build configuration object
                var configuration = new Configuration()
                {
                    Id = (Guid)json.SelectToken(Constants.PropertyId),
                    Name = (string)json.SelectToken(Constants.PropertyName),
                    CompanyName = (string)json.SelectToken(Constants.PropertyBusinessPartnerName),
                    ModuleId = (string)json.SelectToken(Constants.PropertyModuleId),
                    Model = (string)json.SelectToken(Constants.PropertyModel)
                };

                var properties =
                    from property in json[Constants.PropertyProperties]
                    select new
                    {
                        FieldName = (string)property[Constants.PropertyFieldName],
                        Id = (int)property[Constants.PropertyId],
                        FieldType = (string)property[Constants.PropertyFieldType],
                        Name = (string)property[Constants.PropertyName],
                        DataType = (string)property[Constants.PropertyDataType],
                        Mask = (string)property[Constants.PropertyMask],
                        Size = (int)property[Constants.PropertySize],
                        Precision = (int)property[Constants.PropertyPrecision]
                    };

                // Iterate and add to binding
                foreach (var property in properties)
                {
                    configuration.Properties.Add(new Property
                    {
                        FieldName = property.FieldName,
                        Id = property.Id,
                        FieldType = (FieldType)Enum.Parse(typeof(FieldType), property.FieldType),
                        Name = property.Name,
                        DataType = (DataType)Enum.Parse(typeof(DataType), property.DataType),
                        Mask = property.Mask,
                        Size = property.Size,
                        Precision = property.Precision
                    });

                }

                // Add to list of configurations
                configurations.Add(configuration);
            }

            return configurations;
        }

        /// <summary> Cleanup projects folder </summary>
        public static void CleanupProjects()
        {
            // Get Projects folder
            var path = GetProjectsFolder();
            // Delete only
            CreateDirectory(path, true, false);
        }
        #endregion

        #region Private methods
        /// <summary> Create Directory</summary>
        /// <param name="path">Folder to create</param>
        /// <param name="delete">True to delete otherwise false</param>
        /// <param name="create">True to create otherwise false</param>
        /// <remarks>Delete if exists first (as may be stale)</remarks>
        public static void CreateDirectory(string path, bool delete = true, bool create = true)
        {
            if (Directory.Exists(path) && delete)
            {
                // Delete first as may be old/stale/whatever
                Directory.Delete(path, true);
            }
            if (!Directory.Exists(path) && create)
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary> Unzip and Save project files</summary>
        /// <param name="path">Folder to create</param>
        private static void UnzipAndSave(string path)
        {
            foreach (var moduleType in Enum.GetValues(typeof(ModuleType)).Cast<ModuleType>())
            {
                // Unzip Model Project to path
                var fileName = Path.Combine(path, moduleType + Constants.ExtensionZip);
                // BK and TX are included in CS projects, so no project to unzip
                // Do not include PM for 2026
                if (!moduleType.Equals(ModuleType.BK) & !moduleType.Equals(ModuleType.TX)
                    & !moduleType.Equals(ModuleType.PM))
                {
                    // Save zip file
                    File.WriteAllBytes(fileName, GetProjectZip(moduleType));
                    // Extract zip file
                    ZipFile.ExtractToDirectory(fileName, path);
                    // Delete zip file
                    File.Delete(fileName);
                }
            }
        }

        /// <summary> Prep WebHintPath</summary>
        /// <param name="path">Folder to create</param>
        private static void PrepWebHintPath(string path)
        {
            // Get all project files for all modules to prep for hint path
            var files = GetFiles(path, Constants.ExtensionAllCsProj);
            foreach (var file in files)
            {
                // Read the file
                var lines = File.ReadAllLines(file);
                var newLines = new List<string>();

                // Iterate project file
                foreach (var line in lines)
                {
                    // Look for specific line
                    if (line.Contains(Constants.ElementWebHintEnd))
                    {
                        // Replace <WebHintPath> with Sage's Online\Web\Bin folder
                        newLines.Add("\t" + Constants.ElementWebHintStart + RegistryHelper.Sage300WebFolder + Constants.ElementWebHintEnd);
                    }
                    else
                    {
                        newLines.Add(line);
                    }
                }
                // Update file with new <WebHintPath> contents
                File.WriteAllLines(file, newLines);
            }
        }

        /// <summary> Get Projects folder in machine's temp folder </summary>
        /// <returns>Projects folder</returns>
        private static string GetProjectsFolder()
        {
            return Path.Combine(Path.GetTempPath(), Constants.ProjectsFolder);
        }

        /// <summary> Get Backup folder in machine's temp folder </summary>
        /// <returns>Backup folder</returns>
        private static string GetBackupFolder()
        {
            return Path.Combine(Path.GetTempPath(), 
                Constants.BackupFolder + DateTime.Now.ToString("yyyyMMddHHmm"));
        }

        /// <summary> Get Build folder in machine's temp folder </summary>
        /// <returns>Build folder</returns>
        private static string GetBuildFolder()
        {
            return Path.Combine(Path.GetTempPath(), Constants.BuildFolder);
        }

        /// <summary> Get files </summary>
        /// <param name="path">Temp projects folder</param>
        /// <param name="searchPattern">Search Pattern</param>
        /// <returns>Project files</returns>
        public static string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
        }

        /// <summary> Get model projects </summary>
        /// <param name="path">Temp projects folder</param>
        /// <param name="searchPattern">Search Pattern</param>
        /// <returns>Project files</returns>
        private static List<string> GetModelProjects(string path)
        {
            // Get all project files in a specific order!
            var projects = new List<string>();
            foreach (
                var moduleType in
                Enum.GetValues(typeof(ModuleType))
                    .Cast<ModuleType>())
            {
                var project = GetFiles(path, 
                    "*." + moduleType + Constants.ExtensionModelsCsProj).FirstOrDefault();
                if (project != null)
                {
                    projects.Add(project);
                }
            }
            return projects;
        }

        /// <summary> Get project zip for requested module </summary>
        /// <param name="moduleType">Module Type </param>
        /// <returns>Byte array for zip file</returns>
        private static byte[] GetProjectZip(ModuleType moduleType)
        {
            // Local
            byte[] ret;

            // Conditional for Module Types
            switch (moduleType)
            {
                case ModuleType.AP:
                    ret = Resources.AP;
                    break;
                case ModuleType.AR:
                    ret = Resources.AR;
                    break;
                case ModuleType.AS:
                    ret = Resources.AS;
                    break;
                case ModuleType.CS:
                    ret = Resources.CS;
                    break;
                case ModuleType.GL:
                    ret = Resources.GL;
                    break;
                case ModuleType.IC:
                    ret = Resources.IC;
                    break;
                case ModuleType.KN:
                    ret = Resources.KN;
                    break;
                case ModuleType.KPI:
                    ret = Resources.KPI;
                    break;
                case ModuleType.MT:
                    ret = Resources.MT;
                    break;
                case ModuleType.OE:
                    ret = Resources.OE;
                    break;
                // Do not deliver PM for 2026
                //case ModuleType.PM:
                //    ret = Resources.PM;
                //    break;
                case ModuleType.PO:
                    ret = Resources.PO;
                    break;
                case ModuleType.PR:
                    ret = Resources.PR;
                    break;
                case ModuleType.TM:
                    ret = Resources.TM;
                    break;
                case ModuleType.TS:
                    ret = Resources.TS;
                    break;
                case ModuleType.TW:
                    ret = Resources.TW;
                    break;
                case ModuleType.VPF:
                    ret = Resources.VPF;
                    break;
                default:
                    ret = Resources.AP;
                    break;
            }
            return ret;
        }

        /// <summary> Modify the model with any configuration(s) for it </summary>
        /// <param name="project">Project</param>
        private string ModifyModel(string project)
        {
            // Local
            string ret = string.Empty;

            try
            {
                // Get DLL Name to see if it exists
                var assemblyName = Path.GetFileName(project).Replace(Constants.ExtensionCsProj, Constants.ExtensionDll);
                if (!File.Exists(Path.Combine(RegistryHelper.Sage300WebFolder, assemblyName)))
                {
                    // Model assembly does not exist and therefore cannot modify it
                    return ret;
                }

                // Get Module for project and any configurations
                var tmp = assemblyName.Split('.');
                var module = tmp[Constants.ModuleSegment];
                var configurations = _settings.Configurations.Where(configuration => configuration.ModuleId == module);

                // If no configurations for module, then continue to next project
                if (!configurations.Any())
                {
                    return ret;
                }

                // Iterate configurations for this module if any
                foreach (var configuration in configurations)
                {
                    // Get model name and contents
                    var modelName = Path.Combine(Path.GetDirectoryName(project), 
                        configuration.Model + Constants.ExtensionCs);
                    var file = File.ReadAllLines(modelName).ToList();
                    if (file == null)
                    {
                        continue;
                    }

                    // Iterate file to insert snippet
                    var classDefFound = false;
                    var bracketFound = false;
                    var indent = 0;
                    var usingFound = false;
                    var usingInsertIndex = 0;

                    foreach (var line in file)
                    {
                        // Search for class definition
                        if (!classDefFound && line.Contains(Constants.TokenPublic) && 
                            line.Contains(Constants.TokenClass) && 
                            !line.StartsWith(Constants.TokenComment))
                        {
                            // Class definition located
                            classDefFound = true;
                            continue;
                        }

                        // Search for using statement for Attributes
                        if (!usingFound && line.Contains(Constants.UsingAttributes) &&
                            !line.StartsWith(Constants.TokenComment))
                        {
                            // Using statement located
                            usingFound = true;
                            continue;
                        }

                        if (!usingFound && line.Trim().StartsWith(Constants.NamespaceLine))
                        {
                            // Using statement not located, but namespace is located
                            // Therefore, we will insert using statement just before namespace statement
                            usingInsertIndex = file.IndexOf(line);
                            continue;
                        }

                        // Search for place to insert snippet
                        if (classDefFound && !bracketFound && line.Trim().StartsWith(Constants.TokenBrace))
                        {
                            // Start of class code area found
                            bracketFound = true;
                            indent = line.IndexOf(Constants.TokenBrace);
                            continue;
                        }

                        // Insert Snippet
                        if (classDefFound && bracketFound)
                        {
                            // Now that code area has been found, insert extended properties
                            var newLines = new StringBuilder();

                            // Comment for company name and configuration name
                            newLines.AppendLine(Environment.NewLine);
                            newLines.AppendLine(new string(' ', indent * 2) +
                                Constants.CommentCompanyName + configuration.CompanyName +
                                Constants.CommentConfigName + configuration.Name);

                            // Iterate properties to insert
                            foreach (var property in configuration.Properties)
                            {
                                // Attribute with elements
                                var newLine = new string(' ', indent * 2) + Constants.CodeAttribute;
                                newLines.AppendLine(string.Format(newLine, property.FieldName, property.Id, 
                                    property.FieldType, property.Size, property.Precision, property.Mask));

                                // Property definition
                                newLine = new string(' ', indent * 2) + Constants.CodeProperty;

                                // Force case for c# data type
                                var dataType = (property.DataType == DataType.DateTime || 
                                    property.DataType == DataType.TimeSpan) ? property.DataType.ToString() : 
                                    property.DataType.ToString().ToLower();

                                newLines.Append(string.Format(newLine, dataType, property.Name));
                                newLines.AppendLine(Constants.CodePropertyGetSet);

                                // New line
                                newLines.AppendLine(Environment.NewLine);
                            }

                            // Insert into file
                            file.Insert(file.IndexOf(line), newLines.ToString());

                            // If using statement not found, then insert it now
                            if (usingInsertIndex != 0)
                            {
                                // Using statement not located
                                file.Insert(usingInsertIndex, Constants.UsingAttributes);
                            }

                            // Update file
                            File.Delete(modelName);
                            File.WriteAllLines(modelName, file);
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        /// <summary> Build project </summary>
        /// <param name="project">Project</param>
        /// <param name="path">Projects path</param>
        /// <param name="buildFolder">Build folder</param>
        private string BuildProject(string project, string path, string buildFolder)
        {
            // Local
            string ret = string.Empty;
            var buildFilePath = @_settings.Compiler.PathName;
            var buildFile = Path.Combine(path, _settings.Compiler.CompilerType + Constants.BuildFileExt);

            // Get DLL Name to see if it exists
            var assemblyName = Path.GetFileName(project).Replace("csproj", "dll");
            if (!File.Exists(Path.Combine(RegistryHelper.Sage300WebFolder, assemblyName)))
            {
                // Model assembly does not exist and therefore cannot build it
                return ret;
            }

            // Get log file to record and check for errors in build
            var logFile = Path.Combine(buildFolder, Constants.BuildLogFile);

            try
            {
                var arguments = $"\"{project}\" \"/t:Rebuild\" \"/fl\" \"/flp:logfile={logFile};errorsonly\" \"/p:Configuration=Release\" \"/p:OutputPath={buildFolder}\" \"/noconlog\" \"/nologo\" \"{buildFilePath}\"";

                ProcessStartInfo startInfo = new ProcessStartInfo(buildFile)
                {
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = arguments
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };
                process.Start();
                process.WaitForExit();
                process.Close();

                // Determine if build failed (no error thrown)
                var file = File.ReadAllText(logFile);
                if (!string.IsNullOrEmpty(file))
                {
                    ret = Resources.BuildFailed;
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        /// <summary> Deploy Assembly </summary>
        /// <param name="project">Project</param>
        /// <param name="backupFolder">Backup folder</param>
        /// <param name="buildFolder">Build Folder</param>
        private string DeployAssembly(string project, string backupFolder, string buildFolder)
        {
            // Local
            string ret = string.Empty;

            // Get DLL Name to see if it exists
            var assemblyName = Path.GetFileName(project).Replace("csproj", "dll");
            if (!File.Exists(Path.Combine(RegistryHelper.Sage300WebFolder, assemblyName)))
            {
                // Model assembly does not exist and therefore cannot deploy it
                return ret;
            }

            try
            {
                // Stop the worker service
                WorkerService.Stop();

                assemblyName = Path.GetFileName(project).Replace("csproj", "dll");
                var fromFile = Path.Combine(buildFolder, assemblyName);
                var toWebFile = Path.Combine(RegistryHelper.Sage300WebFolder, assemblyName);
                var toWorkerFile = Path.Combine(RegistryHelper.Sage300WorkerFolder, assemblyName);
                var toWebAPIFile = Path.Combine(RegistryHelper.Sage300WebAPIFolder, assemblyName);
                var toWebFRFile = Path.Combine(RegistryHelper.Sage300WebFRFolder, assemblyName);

                // Backup first. Create folder if needed
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                // Only need to backup the web file since all other files (Worker, WebAPI, WebFR are identical)
                var toBackupfile = Path.Combine(backupFolder, assemblyName);
                File.Copy(toWebFile, toBackupfile);

                if (File.Exists(fromFile))
                {
                    // Copy Online\Web\bin
                    ret = CopyFile(fromFile, toWebFile);
                    if (string.IsNullOrEmpty(ret))
                    {
                        // Copy Online\Worker
                        ret = CopyFile(fromFile, toWorkerFile);
                        if (string.IsNullOrEmpty(ret))
                        {
                            // Copy Online\WebAPI
                            ret = CopyFile(fromFile, toWebAPIFile);
                            if (string.IsNullOrEmpty(ret))
                            {
                                // Copy Online\WebFR
                                ret = CopyFile(fromFile, toWebFRFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }

            return ret;
        }

        /// <summary> Copy file with retry logic </summary>
        /// <param name="fromFile">Copy from file</param>
        /// <param name="toFile">Copy to file</param>
        /// <returns>string.empty if successful else message</returns>
        private string CopyFile (string fromFile, string toFile)
        {
            // Local
            string ret = string.Empty;
            bool success = false;
            int retries = 0;

            // Only copy the file if the "to" file exists
            if (File.Exists(toFile))
            {
                do
                {
                    try
                    {
                        File.Copy(fromFile, toFile, true);
                        retries = 4;
                        success = true;
                    }
                    catch
                    {
                        // Sleep and retry
                        Thread.Sleep(500);
                        retries++;
                    }
                } while (retries < 4);
            }
            else
            {
                success = true;
            }

            // Determine if copy was successful
            if (!success)
            {
                ret = string.Format(Resources.ErrorCopyingFile, toFile);
            }

            return ret;
        }

        /// <summary> Update UI </summary>
        /// <param name="success">True/False based upon creation</param>
        /// <param name="fileName">Name of file to be created</param>
        /// <param name="failureMessage">Failure message</param>
        private void LaunchStatusEvent(bool success, string fileName, string failureMessage = null)
        {
            // Return if no subscriber
            if (StatusEvent == null)
            {
                return;
            }

            // Update according to success or failure
            if (success)
            {
                StatusEvent(fileName, Info.StatusType.Success, string.Empty);
            }
            else
            {
                StatusEvent(fileName, Info.StatusType.Error, failureMessage);
            }
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

        /// <summary> Find Visual Studio Versions on machine </summary>
        /// <param name="source">Source to search for token</param>
        private static List<Compiler> FindVSVersions(string source)
        {
            // Locals
            var ret = new List<Compiler>();
            int index = 0;

            // Iterate source looking for token(s)
            while (index != -1)
            {
                index = source.IndexOf(Constants.VSToken, index);
                if (index != -1)
                {
                    // Token found, now parse
                    var startPos = index + Constants.VSToken.Length + 2;
                    var installPath = source.Substring(startPos,
                        source.IndexOf(Environment.NewLine, startPos) - startPos);
                    var tmp = installPath.Split('\\');
                    ret.Add(new Compiler() 
                    {
                        CompilerType = Constants.VSKey,
                        VersionNum = tmp[3],
                        PathName = installPath + Constants.MSBuildSubFolders
                    });
                    index++;
                }
            }
            
            return ret;
        }

        /// <summary> Find .NET SDK Versions on machine </summary>
        /// <param name="source">Source to search for token</param>
        private static List<Compiler> FindSDKVersions(string source)
        {
            // Locals
            var ret = new List<Compiler>();
            var lines = source.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var found = false;

            // Iterate source looking for token(s)
            foreach (var line in lines)
            {
                // If we find the start of the installed SDKs, set flag and continue
                if (line.Contains(Constants.SDKStartToken))
                {
                    found = true;
                    continue;
                }
                
                // If start was found AND we are done with the section, then bail
                if (found && line.Contains(Constants.SDKEndToken))
                {
                    break;
                }

                // If start was found, then process SDK
                if (found)
                {
                    var tmp = line.Split('[');
                    ret.Add(new Compiler()
                    {
                        CompilerType = Constants.SDKKey,
                        VersionNum = tmp[0].Trim(),
                        PathName = Directory.GetParent(tmp[1].Trim().Replace("]", "")).FullName
                    });
                }
            }

            return ret;
        }
        #endregion
    }
}
