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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Interfaces;
using EnvDTE;
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.PerRelease
{
    public class CSharpProject
    {
        public string ProjectFileName { get; set; }
        public string ProjectDirectory { get; set; }
        public string ProjectFilePath { get; set; }
        public string ModuleId { get; set; }
    }

    public class ExternalContentProcessor
    {
        #region Private Constants
        private static class Constants
        {
            public const string AreasFolderName = @"Areas";
            public const string ExternalContentFolderName = @"ExternalContent";
        }
        #endregion

        #region Private Variables
        /// <summary> Settings from UI </summary>
        private Settings _settings;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">An instance of the Settings object</param>
        public ExternalContentProcessor(ISettings settings)
        {
            _settings = (Settings)settings;
        }
        #endregion

        public void Process()
        {
            //•	Create ExternalContent folder under Areas\{ Module} and add to web project
            //•	Get files names for menuIcon and menuBackGroundImage from { module} MenuDetails.XML
            //•	Rename file to icon_{ module(in lower case)}.png and move to Areas\{ module}\ExternalContent
            //•	Rename file to bg_menu_{ module(in lower case)}.jpg and move to Areas\{ module}\ExternalContent
            //•	Any other partner created files and/ or folders will need to be moved manually by partner(documentation step)
            //•	Change {module}MenuDetails two elements to
            //      ../../../../ Areas /{ module}/ ExternalContent / icon_{ module(lower case)}.png
            //      ../../../../ Areas /{ module}/ ExternalContent / bg_menu_{ module(lower case)}.jpg
            //      Partner will be responsible for changing any customer deployment’s and their installation from 
            //      SageInstallation\Online\Web\External\Content\Images\nav\{$companynamespace$ to 
            //      SageInstallation\Online\Web\Areas\{ module}\ExternalContent(documentation step)


            #region Step 1 - Create ExternalContent folder under Areas\{Module} and add to web project

            //var webFolder = _settings.DestinationWebFolder;
            //var solutionFolder = _settings.DestinationSolutionFolder;

            //// Get the ModuleID specifier from the webFolder path
            //// C:\Users\GrGagnaux\source\repos\Test102\SuperConsulting.SC.Web
            //// \Areas\SC
            //var moduleId = ExtractModuleIdFromPath(webFolder);
            //var areasFolder = Path.Combine(webFolder, Constants.AreasFolderName);
            //var areasModuleFolder = Path.Combine(areasFolder, moduleId);
            //var externalContentFolder = Path.Combine(areasModuleFolder, Constants.ExternalContentFolderName);
            //if (Directory.Exists(externalContentFolder) == false)
            //{
            //    //Directory.CreateDirectory(externalContentFolder);
            //}

            //// Now add the directory to the Web project

            //// PseudoCode
            //// AddFolderToProject(string projectName, "Areas\SC\ExternalContent");
            //// AddFileToProject(string fileName, "Areas\SC\ExternalContent\menuBackgroundImage.png");

            ////List<ProjectItemContainer> list = new List<ProjectItemContainer>();
            //var solutionManager = new SolutionManager(_settings.DestinationSolutionFolder, _settings.Solution);
            //var allProjectItems = solutionManager.GetAllProjectItemsSimplified("SuperConsulting.SC.Web");

            //var list = new List<string>();
            //foreach (var item in allProjectItems)
            //{
            //    var name = item.Name;
            //    var type = item.TypeName;
            //    var parentName = item.ParentItemName;
            //    var parentType = item.ParentItemTypeName;
            //    parentName = parentName.Length == 0 ? "N/A" : parentName;
            //    var line = $"{type} : {name} : {parentName} : {parentType}";
            //    list.Add(line);
            //}

            //var count = list.Count();

            ////solutionManager.GetRootProjectFolders("SuperConsulting.SC.Web");

            ////var areaItem = allProjectItems.Select(item => item.Name == "Areas" && item.Type == ProjectItemTypeEnum.Folder).SingleOrDefault();
            ////var areaItem = allProjectItems.Where(item => item.Name == "Areas" && item.Type == ProjectItemTypeEnum.Folder).SingleOrDefault();
            ////var childItems = areaItem.ChildItems;


            ////var areaModuleItem = allProjectItems


            ////solutionManager.AddFolderToProject("SuperConsulting.SC.Web", @"Areas\SC\ExternalContent");






            ////var list = new List<string>();
            ////Solution2 solution = _settings.Solution;
            ////Projects projects = solution.Projects;
            ////foreach (Project project in projects)
            ////{
            ////    if (project.Name.Equals("SuperConsulting.SC.Web"))
            ////    {
            ////        ProjectItems items = project.ProjectItems;
            ////        foreach (ProjectItem item in items)
            ////        {
            ////            if (item.Kind == VSConstants.vsProjectItemKindPhysicalFolder)

            ////            var kind = item.Kind;
            ////            var name = item.Name;
            ////            var combined = $"{name}:{kind}";
            ////            list.Add(combined);
            ////        }
            ////        //ProjectItem newItem = items.AddFolder(@"Areas\SC\ExternalContent");
            ////    }
            ////}

            ////var projectData = new CSharpProject();
            ////GetProjectInfo(_settings.DestinationWebFolder, ref projectData);
            ////var projectFilePath = projectData.ProjectFilePath;
            ////var project = new Project(projectFilePath);

            ////var projectNames = new List<String>();

            ////var projects = SolutionProjects.Projects();
            ////foreach (var project in projects)
            ////{
            ////    var name = project.Name;
            ////    projectNames.Add(name);
            ////}
            #endregion

            #region Step 2 - Get file names for menuIcon and menuBackGroundImage from {module}MenuDetails.xml
            var menuManager = new MenuManager(_settings);
            var menufilename = menuManager.GetMenuFileName();
            #endregion
        }



        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ExtractModuleIdFromPath(string path)
        {
            var moduleId = string.Empty;

            if (path.Length > 0)
            {
                var parts = path.Split(new[] { '\\' });
                var lastPart = parts[parts.Length-1];
                var finalPart = lastPart.Split(new[] { '.' });
                if (finalPart.Length == 3)
                {
                    moduleId = finalPart[1];
                }
            }
            return moduleId;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ExtractProjectFileNameFromPath(string path)
        {
            const string ExtensionName = @"csproj";
            var filename = string.Empty;

            if (path.Length > 0)
            {
                var parts = path.Split(new[] { '\\' });
                var lastPart = parts[parts.Length - 1];
                filename = lastPart + "." + ExtensionName;
            }
            return filename;
        }

        public void GetProjectInfo(string projectFolder, ref CSharpProject project)
        {
            project.ProjectDirectory = projectFolder;
            project.ModuleId = ExtractModuleIdFromPath(projectFolder);
            project.ProjectFileName = ExtractProjectFileNameFromPath(projectFolder);
            project.ProjectFilePath = Path.Combine(projectFolder, project.ProjectFileName);
        }
    }
}
