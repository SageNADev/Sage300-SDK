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

#region Imports
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Win32;
using Sage.CA.SBS.ERP.Sage300.SolutionWizard.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Process = EnvDTE.Process;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.SolutionWizard
{
    /// <summary> Registry Helper Class </summary>
    public static class RegistryHelper
    {
        /// <summary>
        /// The path to the Registry Key where the name of the shared folder is stored
        /// </summary>
        private const string ConfigurationKey = "SOFTWARE\\ACCPAC International, Inc.\\ACCPAC\\Configuration";

        /// <summary>
        /// The name of the Registry Value containing the name of the shared folder
        /// </summary>
        public static string Sage300CWebFolder
        {
            get
            {
                // Get the registry key
                var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                var configurationKey = baseKey.OpenSubKey(ConfigurationKey);

                // Find path tp shared folder
                return configurationKey == null ? string.Empty : Path.Combine(configurationKey.GetValue("Programs").ToString(), @"Online\Web");
            }
        }

        /// <summary>
        /// The name of the Registry Value containing the name of the shared folder
        /// </summary>
        public static string Sage300CWebApiFolder
        {
            get
            {
                // Get the registry key
                var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                var configurationKey = baseKey.OpenSubKey(ConfigurationKey);

                // Find path tp shared folder
                return configurationKey == null ? string.Empty : Path.Combine(configurationKey.GetValue("Programs").ToString(), @"Online\WebApi");
            }
        }
    }

    /// <summary> Class for UI Wizard </summary>
    public class Sage300CUISolutionWizardUserInterface : IWizard
    {
        #region Private Constants
        private static class Constants
        {
            public const int NumberOfSupportedLanguages = 5;
        }
        #endregion

        #region Private Variables
        private string _companyName;
        private string _applicationId;
        private string _lowercaseapplicationId;
        private string _solutionFolder;
        private string _destinationFolder;
        private DTE _dte;
        private string _safeprojectname;
        private string _namespace;
        private string _sage300Webfolder;
        private string _sage300WebApifolder;
        private string _kendoFolder;
        private bool _includeEnglish;
        private bool _includeChineseSimplified;
        private bool _includeChineseTraditional;
        private bool _includeSpanish;
        private bool _includeFrench;
        private string _typeSegment;
        private string _typeRoute;
        private string _copyright;
        private bool _isWebSolution;
        #endregion


        /// <summary> Before opening file </summary>
        /// <param name="projectItem">Project Item</param>
        /// <remarks>Called before opening any item that has the OpenInEditor attribute</remarks>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        /// <summary> Project finished generating </summary>
        /// <param name="project">Project</param>
        public void ProjectFinishedGenerating(Project project)
        {
        }

        /// <summary> Project item finished generating </summary>
        /// <param name="projectItem">Project Item</param>
        /// <remarks>Called for item templates, not project templates</remarks>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        private void CreateWebSolution()
        {
            var sln = (Solution2)_dte.Solution;

            // Will set later.
            var startupProjectName = "";

            var csTemplatePath = Path.GetDirectoryName(sln.GetProjectTemplate("Sage 300 Solution Wizard", "CSharp")) + @"\..\";

            //this will create a solution
            sln.Create(_destinationFolder, _safeprojectname);

            var namespaceForProject = _companyName + "." + _applicationId + ".";

            var projects = new string[] { "Web", "BusinessRepository", "Interfaces", "Models", "Resources", "Services"};

            var parameters = "$companyname$" + "=" + _companyName + "|$applicationid$=" + _applicationId +
                                "|$companynamespace$=" + _namespace +
                                "|$sage300webfolder$=" + _sage300Webfolder +
                                "|$typesegment$=" + _typeSegment +
                                "|$typeroute$=" + _typeRoute +
                                "|$lowercaseapplicationid$=" + _lowercaseapplicationId +
                                "|$copyright$=" + _copyright;

            foreach (var proj in projects)
            {
                var templateFilename = proj + ".vstemplate";
                var sourceFilenameAndParameters = csTemplatePath + "Web." + proj + @".zip\" + templateFilename + "|" + parameters;
                if (string.Compare(proj, "Web", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //
                    // Processing Web project
                    //
                    var destFolder = Path.Combine(_destinationFolder, _namespace + "." + _applicationId + "." + proj);

                    // Before the web project is created, the props file must be manually 
                    // copied first since the web csproj file attempts to import it to 
                    // resolve ACCPAC references
                    File.WriteAllBytes(Path.Combine(_destinationFolder, "AccpacDotNetVersion.props"),
                                            Properties.Resources.AccpacDotNetVersion);

                    // Example: TrustedPartner.TU.Web
                    var projectName = $"{_namespace}.{_applicationId}.{proj}";
                    startupProjectName = projectName;

                    sln.AddFromTemplate(FileName: sourceFilenameAndParameters,
                                        Destination: destFolder,
                                        ProjectName: projectName,
                                        Exclusive: false);

                    // Newly added web project (first one added)
                    var item = sln.Projects.GetEnumerator();
                    item.MoveNext();
                    var webProject = (Project)item.Current;

                    // Add kendo commercial files here
                    var allMinFileSource = Path.Combine(_kendoFolder, "kendo.all.min.js");
                    var allMinScripts = Path.Combine("Scripts", "Kendo", "kendo.all.min.js"); ;
                    var allMinFileDest = Path.Combine(destFolder, allMinScripts);

                    // Copy files
                    File.Copy(allMinFileSource, allMinFileDest);

                    // Add to project
                    webProject.ProjectItems.AddFromFile(allMinFileDest);

                    //
                    // Update the .csproj.user file
                    // Set the start page to Login.aspx
                    //
                    webProject.Properties.Item("WebApplication.StartPageUrl").Value = "Login.aspx";
                    webProject.Properties.Item("WebApplication.DebugStartAction").Value = 1;
                }
                else
                {
                    //
                    // Processing Other (non-web) projects
                    //
                    var destination = Path.Combine(_destinationFolder, _namespace + "." + _applicationId + "." + proj);
                    var projectName = _namespace + "." + _applicationId + "." + proj;
                    sln.AddFromTemplate(FileName: sourceFilenameAndParameters,
                                        Destination: destination,
                                        ProjectName: projectName,
                                        Exclusive: false);

                    // If project is Resources, remove language resources not selected
                    if (proj.Equals("Resources"))
                    {
                        // Get resources project just added
                        var item = sln.Projects.GetEnumerator();
                        for (var i = 0; i < Constants.NumberOfSupportedLanguages; i++)
                        {
                            item.MoveNext();
                        }
                        var project = (Project)item.Current;

                        // Iterate files in resources project
                        foreach (ProjectItem projectItem in project.ProjectItems)
                        {
                            // Delete Language file? (English will always be included)
                            if ((projectItem.Name.Equals("MenuResx.zh-Hans.resx") && !_includeChineseSimplified) ||
                               (projectItem.Name.Equals("MenuResx.zh-Hant.resx") && !_includeChineseTraditional) ||
                                (projectItem.Name.Equals("MenuResx.es.resx") && !_includeSpanish) ||
                                (projectItem.Name.Equals("MenuResx.fr.resx") && !_includeFrench))
                            {
                                // remove from project
                                projectItem.Delete();
                            }
                        }
                    }
                }
            }

            // Now, set the start project for the solution
            sln.Properties.Item("StartupProject").Value = startupProjectName;
        }

        private void CreateWebApiSolution()
        {
            var sln = (Solution2)_dte.Solution;

            // Will set later.
            var startupProjectName = "";

            var csTemplatePath = Path.GetDirectoryName(sln.GetProjectTemplate("Sage 300 Solution Wizard", "CSharp")) + @"\..\";

            //this will create a solution
            sln.Create(_destinationFolder, _safeprojectname);

            var namespaceForProject = _companyName + "." + _applicationId + ".";

            var projects = new string[] { "WebApi", "Models" };

            var parameters = "$companyname$" + "=" + _companyName + "|$applicationid$=" + _applicationId +
                                "|$companynamespace$=" + _namespace +
                                "|$sage300webApifolder$=" + _sage300WebApifolder +
                                "|$lowercaseapplicationid$=" + _lowercaseapplicationId +
                                "|$copyright$=" + _copyright;

            foreach (var proj in projects)
            {
                var templateFilename = proj + ".vstemplate";
                var sourceFilenameAndParameters = csTemplatePath + "WebApi." + proj + @".zip\" + templateFilename + "|" + parameters;
                var destination = Path.Combine(_destinationFolder, _namespace + "." + _applicationId);
                var projectName = _namespace + "." + _applicationId;
                if (!proj.Equals("WebApi"))
                {
                    destination += ".WebApi." + proj;
                    projectName += ".WebApi." + proj;
                }
                else
                {
                    destination += "." + proj;
                    projectName += "." + proj;
                }
                sln.AddFromTemplate(FileName: sourceFilenameAndParameters,
                                        Destination: destination,
                                        ProjectName: projectName,
                                        Exclusive: false);

            }
        }

        public void RunFinished()
        {

            if (_isWebSolution)
            {
                CreateWebSolution();
            }
            else
            {
                CreateWebApiSolution();
            }
        }

        /// <summary> Run started </summary>
        /// <param name="automationObject">Automation Object</param>
        /// <param name="replacementsDictionary">Replacements</param>
        /// <param name="runKind">Run Kind</param>
        /// <param name="customParams">Custom Params</param>
        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {

            _dte = (DTE)automationObject;

            _solutionFolder = replacementsDictionary["$solutiondirectory$"];
            _destinationFolder = replacementsDictionary["$destinationdirectory$"];
            _safeprojectname = replacementsDictionary["$safeprojectname$"];

            try
            {
                // Display a form to the user. The form collects 
                // input for the custom message.
                var inputForm = new UserInputForm();

                // Default the location for the Kendo folder
                var webFolder = RegistryHelper.Sage300CWebFolder;
                var webApiFolder = RegistryHelper.Sage300CWebApiFolder;

                inputForm.KendoDefaultFolder = Path.Combine(webFolder, "Scripts", "Kendo");

                var res = inputForm.ShowDialog();

                // cancel wizard
                if (res != DialogResult.OK)
                    throw new WizardBackoutException();

                // save parameters
                _companyName = inputForm.ThirdPartyCompanyName.Trim();
                _applicationId = inputForm.ThirdPartyApplicationId.Trim();
                _lowercaseapplicationId = _applicationId.ToLower();
                _namespace = inputForm.CompanyNamespace.Trim();
                _isWebSolution = inputForm.IsWebSolution;

                if (_isWebSolution)
                {
                    _sage300Webfolder = webFolder;
                    _kendoFolder = inputForm.KendoFolder.Trim();
                    _includeEnglish = inputForm.IncludeEnglish;
                    _includeChineseSimplified = inputForm.IncludeChineseSimplified;
                    _includeChineseTraditional = inputForm.IncludeChineseTraditional;
                    _includeSpanish = inputForm.IncludeSpanish;
                    _includeFrench = inputForm.IncludeFrench;

                    // Payroll
                    _typeSegment = _applicationId == "PR" ? "{type}/" : "";
                    _typeRoute = _applicationId == "PR" ? "type = 0, " : "";

                    replacementsDictionary.Add("$typesegment$", _typeSegment);
                    replacementsDictionary.Add("$sage300webfolder$", _sage300Webfolder);
                    replacementsDictionary.Add("$typeroute$", _typeRoute);
                }
                else
                {
                    _sage300WebApifolder = webApiFolder;
                }

                // Logic specific for Sage developers
                _copyright = _companyName;
                if (_companyName == "Sage")
                {
                    _copyright = "The Sage Group plc or its licensors.";
                }

                // Add custom parameters.
                replacementsDictionary.Add("$companyname$", _companyName);
                replacementsDictionary.Add("$applicationid$", _applicationId);
                replacementsDictionary.Add("$companynamespace$", _namespace);
                replacementsDictionary.Add("$lowercaseapplicationid$", _lowercaseapplicationId);
                replacementsDictionary.Add("$copyright$", _copyright);
            }
            catch
            {
                // Clean up the template that was written to disk
                if (Directory.Exists(_solutionFolder))
                {
                    try
                    {
                        Directory.Delete(_solutionFolder);
                    }
                    catch
                    {
                        ; // don't care
                    }
                }

                throw;
            }

        }

        /// <summary> Should add project item </summary>
        /// <param name="filePath">File path</param>
        /// <returns>True</returns>
        /// <remarks>Called for item templates not project templates</remarks>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}