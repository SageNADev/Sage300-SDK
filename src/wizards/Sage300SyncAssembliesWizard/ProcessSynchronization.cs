// The MIT License (MIT) 
// Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard.Properties;
using System.IO;
using System.Linq;

namespace Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard
{
    /// <summary> Process Synchronization Class (worker) </summary>
    internal class ProcessSynchronization
    {
        #region Private Vars
        /// <summary> Settings from UI </summary>
        private Settings _settings;
        #endregion

        #region Private Constants
        #endregion

        #region Public Constants
        /// <summary> SettingsKey is used as a dictionary key for settings </summary>
        public const string SettingsKey = "settings";
        #endregion

        #region Public Delegates
        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        public delegate void StatusEventHandler(string fileName);
        #endregion

        #region Public Events
        /// <summary> Event to update UI with name of file being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update UI with status of file being processed </summary>
        public event StatusEventHandler StatusEvent;
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary> Cleanup </summary>
        public void Dispose()
        {
        }
        /// <summary> Validate the settings based upon repository type</summary>
        /// <param name="settings">Settings</param>
        /// <returns>Empty if valid otherwise message</returns>
        public string ValidSettings(Settings settings)
        {
            // Source #1
            if (settings.SourceType.Equals(SourceType.Jenkins))
            {
                return Resources.InvalidSettings;
            }

            // Destination Validation #1
            if (settings.Destination.Equals(string.Empty))
            {
                return Resources.InvalidSettingRequired;
            }

            // Destination Validation #2
            if (!Directory.Exists(settings.Destination))
            {
                return Resources.InvalidDestinationSetting;
            }

            // Destination Web Validation #1
            if (settings.DestinationWeb.Equals(string.Empty))
            {
                return Resources.InvalidSettingRequired;
            }

            // Destination Web Validation #2
            if (!Directory.Exists(settings.DestinationWeb))
            {
                return Resources.InvalidDestinationSetting;
            }

            // Check Assemblies #1. There must be at least one assembly pattern selected
            return settings.Assemblies.Any(i => i.IsIncluded) ? string.Empty : Resources.InvalidSettingAssembliesCount;
        }

        /// <summary> Start the synchronization process </summary>
        /// <param name="settings">Settings for processing</param>
        public void Process(Settings settings)
        {
            try
            {
                _settings = settings;

                // Validate the settings
                if (!string.IsNullOrEmpty(ValidSettings(_settings)))
                {
                    return;
                }

                // TODO: Assign jenkins Folder
                var sourceFolder = (_settings.SourceType.Equals(SourceType.Local)) ? RegistryHelper.Sage300CWebFolder : string.Empty;

                // Initial sync will copy all files
                if (_settings.InitialSync)
                {
                    // Copy all files
                    IterateAllFiles(sourceFolder);
                }
                else
                {
                    // Iterate Assembly Patterns
                    IterateAssemblyPatterns(sourceFolder);
                }

            }
            catch
            {
                // Catch here does nothing but return to UI
                // User may have aborted wizard
            }
        }

        #endregion

        #region Private Methods

        /// <summary> Iterate the Assembly patterns </summary>
        /// <param name="sourceFolder">Source Folder for assemblies</param>
        private void IterateAssemblyPatterns(string sourceFolder)
        {
            // Iterate selected patterns
            foreach (var assemblyInfo in _settings.Assemblies)
            {
                // Proceed if included
                if (assemblyInfo.IsIncluded)
                {
                    var files = Directory.GetFiles(sourceFolder, "Sage.CA.SBS.ERP.Sage300." + assemblyInfo.AssemblyPattern + ".*");

                    // Iterate files
                    foreach (var file in files)
                    {
                        // Workers
                        var fileName = Path.GetFileName(file);
                        var ext = Path.GetExtension(file);
                        var newFilename = string.IsNullOrEmpty(fileName) ? string.Empty : Path.Combine(_settings.Destination, fileName);
                        var newWebFilename = string.IsNullOrEmpty(fileName) ? string.Empty : Path.Combine(_settings.DestinationWeb, "bin", fileName);

                        var exists = File.Exists(newFilename);
                        var existsWeb = File.Exists(newWebFilename);
                        var fileIsPdb = ext != null && 
                            (ext.ToUpper().Replace(".", string.Empty).Equals("PDB") || 
                            ext.ToUpper().Replace(".", string.Empty).Equals("CONFIG"));

                        // Skip if not copying PDB files
                        if (!_settings.IncludePdbFiles && fileIsPdb)
                        {
                            continue;
                        }

                        // Skip if not overriding files
                        if (!assemblyInfo.Override && exists)
                        {
                            continue;
                        }

                        // Update display of file being processed
                        if (ProcessingEvent != null)
                        {
                            ProcessingEvent(fileName);
                        }

                        // Copy the file
                        File.Copy(file, newFilename, true);
                        File.Copy(file, newWebFilename, true);

                        // Update status
                        LaunchStatusEvent(fileName);
                    }
                }
            }

        }

        /// <summary> Iterate all files </summary>\
        /// <param name="sourceFolder">Source Folder for assemblies</param>
        /// <remarks> Skip pre-compiled razor views</remarks>
        private void IterateAllFiles(string sourceFolder)
        {
            // Delete Assemblies target
            var target = new DirectoryInfo(_settings.Destination);
            if (Directory.Exists(target.FullName))
            {
                target.Delete(true);
            }

            // Delete Web target
            var targetWeb = new DirectoryInfo(Path.Combine(_settings.DestinationWeb, "bin"));
            if (Directory.Exists(targetWeb.FullName))
            {
                targetWeb.Delete(true);
            }

            // Get Directory Info of source folder
            var source = new DirectoryInfo(sourceFolder);

            // Copy All
            CopyAll(source, target);
            CopyAll(source, targetWeb);

            // Copy Menus
            CopyMenus(target.Parent, targetWeb.Parent);

            // Copy Areas (Scripts and Views)
            CopyAreas(target.Parent, targetWeb.Parent);
        }

        /// <summary> Copy All </summary>\
        /// <param name="source">Directory Information of source</param>
        /// <param name="target">Directory Information of target</param>
        /// <remarks> Skip pre-compiled razor views</remarks>
        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory
            foreach (var file in source.GetFiles())
            {
                // Skip Razor Views
                if (file.Name.StartsWith("App_") || file.Name.EndsWith("compiled") || file.Name.EndsWith(".CompiledViews.dll"))
	            {
		            continue;
	            }

                // Update display of file being processed
                if (ProcessingEvent != null)
                {
                    ProcessingEvent(file.Name);
                }

                // Copy
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);

                // Update status
                LaunchStatusEvent(file.Name);
            }

            // Copy each subdirectory using recursion
            foreach (var sourceSubDirectory in source.GetDirectories())
            {
                var targetSubDirectory = target.CreateSubdirectory(sourceSubDirectory.Name);
                CopyAll(sourceSubDirectory, targetSubDirectory);
            }

        }

        /// <summary> Copy Menus </summary>\
        /// <param name="source">Directory Information of source</param>
        /// <param name="target">Directory Information of target</param>
        private void CopyMenus(DirectoryInfo source, DirectoryInfo target)
        {
            CopyMenu(source, target, "Columbus-AP", "AP");
            CopyMenu(source, target, "Columbus-AR", "AR");
            CopyMenu(source, target, "Columbus-AS", "AS");
            CopyMenu(source, target, "Columbus-CS", "CS", "BK");
            CopyMenu(target, target, "Areas", "Core"); // Non-standard location for Core
            CopyMenu(source, target, "Columbus-CS", "CS");
            CopyMenu(source, target, "Columbus-GL", "GL");
            CopyMenu(source, target, "Columbus-IC", "IC");
            CopyMenu(source, target, "Columbus-KN", "KN");
            CopyMenu(source, target, "Columbus-KPI", "KPI");
            CopyMenu(source, target, "Columbus-MT", "MT");
            CopyMenu(source, target, "Columbus-OE", "OE");
            CopyMenu(source, target, "Columbus-PO", "PO");
            CopyMenu(source, target, "Columbus-TW", "TW");
            CopyMenu(source, target, "Columbus-TM", "TM");
            CopyMenu(source, target, "Columbus-TS", "TS");
            CopyMenu(source, target, "Columbus-PM", "PM");
            CopyMenu(source, target, "Columbus-CS", "CS", "TX");
        }

        /// <summary> Copy Menu </summary>\
        /// <param name="source">Directory Information of source</param>
        /// <param name="target">Directory Information of target</param>
        /// <param name="repoName">Repository Name</param>
        /// <param name="areaName">Area Name</param>
        /// <param name="module">Module Name in case different than area name (i.e. CS - BK, TX)</param>
        private void CopyMenu(DirectoryInfo source, DirectoryInfo target, string repoName, string areaName, string module = "")
        {
            // If module is not assigned then use area name
            if (string.IsNullOrEmpty(module))
            {
                module = areaName;
            }

            var sourceFile = Path.Combine(source.FullName, repoName, areaName, module + "MenuDetails.xml");
            var destFile = Path.Combine(target.FullName, "App_Data", "MenuDetail", module + "MenuDetails.xml");
            
            // Update display of file being processed
            if (ProcessingEvent != null)
            {
                ProcessingEvent(sourceFile);
            }

            // Copy
            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destFile, true);

                // Update status
                LaunchStatusEvent(sourceFile);
            }
        }

        /// <summary> Copy Areas </summary>\
        /// <param name="source">Directory Information of source</param>
        /// <param name="target">Directory Information of target</param>
        private void CopyAreas(DirectoryInfo source, DirectoryInfo target)
        {
            CopyArea(source, target, "Columbus-AP", "AP");
            CopyArea(source, target, "Columbus-AR", "AR");
            CopyArea(source, target, "Columbus-AS", "AS");
            CopyArea(source, target, "Columbus-CS", "CS");
            CopyArea(source, target, "Columbus-GL", "GL");
            CopyArea(source, target, "Columbus-IC", "IC");
            CopyArea(source, target, "Columbus-KN", "KN");
            CopyArea(source, target, "Columbus-KPI", "KPI");
            CopyArea(source, target, "Columbus-MT", "MT");
            CopyArea(source, target, "Columbus-OE", "OE");
            CopyArea(source, target, "Columbus-PO", "PO");
            CopyArea(source, target, "Columbus-TW", "TW");
            CopyArea(source, target, "Columbus-TM", "TM");
            CopyArea(source, target, "Columbus-TS", "TS");
            CopyArea(source, target, "Columbus-PM", "PM");
            CopyArea(source, target, "Columbus-VPF", "VPF");
            CopyArea(source, target, "Columbus-PR", "PR");
        }

        /// <summary> Copy Area </summary>\
        /// <param name="source">Directory Information of source</param>
        /// <param name="target">Directory Information of target</param>
        /// <param name="repoName">Repository Name</param>
        /// <param name="areaName">Area Name</param>
        private void CopyArea(DirectoryInfo source, DirectoryInfo target, string repoName, string area)
        {
            var moduleTarget = new DirectoryInfo(Path.Combine(target.FullName, "Areas", area));

            // Delete if exists
            if (moduleTarget.Exists)
            {
                moduleTarget.Delete(true);
            }

            // Copy Scripts
            CopyAll(new DirectoryInfo(Path.Combine(source.FullName, repoName, area, "Scripts")), 
                new DirectoryInfo(Path.Combine(moduleTarget.FullName, "Scripts")));

            // Copy Views
            CopyAll(new DirectoryInfo(Path.Combine(source.FullName, repoName, area, "Views")),
                new DirectoryInfo(Path.Combine(moduleTarget.FullName, "Views")));
        }

        /// <summary> Update UI </summary>
        /// <param name="fileName">name of file to be created</param>
        private void LaunchStatusEvent(string fileName)
        {
            if (StatusEvent != null)
            {
                StatusEvent(fileName);
            }
        }

        #endregion
    }
}
