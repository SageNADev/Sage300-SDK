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
using System.Linq;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
//using Microsoft.Practices.RecipeFramework.Library.DteHelper;
using Microsoft.Build.Framework;
using VSConstants = EnvDTE.Constants;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    public enum ProjectItemTypeEnum
    {
        Unknown = 0,
        Project,
        Folder,
        File,
    }

    public class ProjectItemContainer
    {
        public ProjectItem BaseProjectItem { get; set; }
        public ProjectItemTypeEnum Type { get; set; }
        public string TypeName
        {
            get
            {
                if (Type == ProjectItemTypeEnum.Project) return "Project";
                if (Type == ProjectItemTypeEnum.Folder) return "Folder";
                if (Type == ProjectItemTypeEnum.File) return "File";
                return "Unknown";
            }
        }
        public string Name { get; set; }
        public bool HasChildItems { get; set; }
        public Project ContainingProject { get; set; }
        public string ContainingProjectName { get { return BaseProjectItem.ContainingProject.Name; } }
        public ProjectItems ProjectItems { get { return BaseProjectItem.ProjectItems; } }
        //public ProjectItem Parent => (ProjectItem)ProjectItems.Parent;
        public Object Parent { get { return BaseProjectItem.Collection.Parent; } }
        public string ParentItemName
        {
            get
            {
                Type parentType = Parent.GetType();
                string name = string.Empty;

                if (parentType == typeof(Project))
                {
                    name = ((Project)Parent).Name;
                }
                else if (parentType == typeof(ProjectItem))
                {
                    name = ((ProjectItem)Parent).Name;
                }
                return name;
            }
        }
        public ProjectItemTypeEnum ParentItemType
        {
            get
            {
                ProjectItemTypeEnum type = ProjectItemTypeEnum.Project;
                var parentType = Parent.GetType();

                if (parentType == typeof(Project))
                {
                    type = ProjectItemTypeEnum.Project;
                }
                else if (parentType == typeof(ProjectItem))
                {
                    type = SolutionManager.GetProjectItemTypeEnum(((ProjectItem)Parent).Kind);
                }
                return type;
            }
        }
        public string ParentItemTypeName
        {
            get
            {
                if (ParentItemType == ProjectItemTypeEnum.Project) return "Project";
                if (ParentItemType == ProjectItemTypeEnum.Folder) return "Folder";
                if (ParentItemType == ProjectItemTypeEnum.File) return "File";
                return "Unknown";
            }
        }
    }


    public class SolutionManager
    {
        #region Private Variables
        private string _solutionPath;
        private Solution2 _solution;
        private IList<Project> _projects;
        #endregion

        #region Constructor
        public SolutionManager(string solutionPath, Solution2 sol)
        {
            _solutionPath = solutionPath;
            _solution = sol;
            _projects = Projects();
        }
        #endregion

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public DTE2 GetActiveIDE()
        {
            // Get an instance of currently running Visual Studio IDE.
            DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
            return dte2;
        }

        public IList<Project> Projects()
        {
            //Projects projects = GetActiveIDE().Solution.Projects;
            Projects projects = _solution.Projects;
            List<Project> list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public Project GetProject(string projectName)
        {
            return _projects.Where(p => p.Name.Equals(projectName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// TODO
        /// Example Call:
        /// AddFolderToProject("SuperConsulting.SC.Web", "Areas\SC\ExternalContent");
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="folderPath"></param>
        public void AddFolderToProject(string projectName, string folderPath)
        {
            // Get the correct project
            var project = GetProject(projectName);

            // Split up the path into individual folder names if necessary
            var folderParts = folderPath.Split(new[] { '\\' });

            var baseProjectFolder = Path.Combine(_solutionPath, projectName);

            var itemExists = ProjectItemExists(projectName, null, folderPath);

            foreach (var folderName in folderParts)
            {
                //// Does the physical folder exist
                //var folder = Path.Combine(baseProjectFolder, folderName);
                //if (Directory.Exists(folder) == true)
            }
        }

        /// <summary>
        /// TODO
        /// Example Call:
        /// ProjectItemExists("SuperConsulting.SC.Web", "Areas\SC\ExternalContent")
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private bool ProjectItemExists(string projectName, ProjectItem item, string itemName)
        {
            var project = GetProject(projectName);
            var projectItems = project.ProjectItems;

            // Split up the path into individual folder names if necessary
            var folderParts = itemName.Split(new[] { '\\' });

            if (folderParts.Length == 0)
            {
                return false;
            }

            for (int x = 0; x < folderParts.Length; x++)
            {
                var folderName = folderParts[x];

                if (item == null)
                {
                    foreach (ProjectItem projectItem in project.ProjectItems)
                    {
                        if (projectItem.Name == folderName &&
                            projectItem.Kind.Equals(VSConstants.vsProjectItemKindPhysicalFolder))
                        {
                            return ProjectItemExists(projectName, projectItem, BuildRemainingPath(folderParts, x + 1));
                        }
                    }
                }
                else
                {
                    foreach (ProjectItem projectItem in item.ProjectItems)
                    {
                        if (projectItem.Name == folderName &&
                            projectItem.Kind.Equals(VSConstants.vsProjectItemKindPhysicalFolder))
                        {
                            return ProjectItemExists(projectName, projectItem, BuildRemainingPath(folderParts, x + 1));
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IEnumerable<ProjectItemContainer> GetAllProjectItemsSimplified(string projectName)
        {
            var outputList = new List<ProjectItemContainer>();
            IEnumerable<ProjectItem> projectItems = this.GetAllProjectItemsRaw(projectName);
            foreach (ProjectItem pi in projectItems)
            {
                var item = new ProjectItemContainer()
                {
                    BaseProjectItem = pi,
                    Name = pi.Name,
                    HasChildItems = pi.ProjectItems.Count > 0,
                    Type = GetProjectItemTypeEnum(pi.Kind),
                    ContainingProject = pi.ContainingProject,
                };

                outputList.Add(item);
            }
            return outputList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public IEnumerable<ProjectItem> GetAllProjectItemsRaw(string projectName)
        {
            Project project = GetProject(projectName);
            return GetAllProjectItemsRecursive(project.ProjectItems);
        }

        public IEnumerable<ProjectItem> GetAllProjectItemsRecursive(ProjectItems projectItems)
        {
            foreach (ProjectItem projectItem in projectItems)
            {
                foreach (ProjectItem subItem in GetAllProjectItemsRecursive(projectItem.ProjectItems))
                {
                    yield return subItem;
                }

                yield return projectItem;
            }
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private string BuildRemainingPath(string[] parts, int startIndex)
        {
            string rval = string.Empty;
            for (int x = startIndex; x < parts.Length; x++)
            {
                rval += parts[x] + "\\";
            }
            return rval;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="filePath"></param>
        public void AddFileToProject(string projectName, string filePath)
        {
            List<string> folderParts = new List<string>();
            string filename = string.Empty;

            // Get the correct project
            var project = GetProject(projectName);

            // Split up the path and file into individual names if necessary
            var allParts = filePath.Split(new[] { '\\' });
            for (int x = 0; x < allParts.Length-2; x++)
            {
                folderParts.Add(allParts[x]);
            }
            filename = folderParts[allParts.Length - 1];

            foreach (var folder in folderParts)
            {
                //foreach (ProjectItem item in project)
                //{
                //    string itemName = item.Name;
                //    string itemKind = item.Kind;
                //    if (itemName == folder && itemKind == VSConstants.vsProjectItemKindPhysicalFolder)
                //    {

                //    }
                //}
            }
        }

        public void TestAddFileToProject(string projectName, string filename)
        {
            // Get the correct project
            var project = GetProject(projectName);
            //var item = project.ProjectItems.AddFolder("ExternalContent");
            project.ProjectItems.AddFromFileCopy(Path.Combine(@"C:\Users\GrGagnaux\source\repos\Test102\SuperConsulting.SC.Web\Areas\SC\ExternalContent\bg_menu_sc.jpg"));
        }

        public static ProjectItemTypeEnum GetProjectItemTypeEnum(string kind)
        {
            return (kind == VSConstants.vsProjectItemKindPhysicalFolder) ? ProjectItemTypeEnum.Folder : ProjectItemTypeEnum.File;
        }

        public IEnumerable<ProjectItemContainer> GetRootProjectFolders(string projectName)
        {
            List<ProjectItemContainer> topLevelProjectFolders = new List<ProjectItemContainer>();

            Project project = GetProject(projectName);
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (item.Kind == VSConstants.vsProjectItemKindPhysicalFolder)
                {
                    topLevelProjectFolders.Add(new ProjectItemContainer()
                    {
                        BaseProjectItem = item,
                        Name = item.Name,
                        Type = GetProjectItemTypeEnum(item.Kind),
                    });
                }
            }
            return topLevelProjectFolders;
        }

        public IEnumerable<ProjectItemContainer> GetProjectItemSubFolders(ProjectItem itemIn)
        {
            List<ProjectItemContainer> list = new List<ProjectItemContainer>();

            foreach (ProjectItem item in itemIn.ProjectItems)
            {
                if (item.Kind == VSConstants.vsProjectItemKindPhysicalFolder)
                {
                    list.Add(new ProjectItemContainer()
                    {
                        BaseProjectItem = item,
                        Name = item.Name,
                        Type = GetProjectItemTypeEnum(item.Kind),
                    });
                }
            }
            return list;
        }

        public bool RootProjectFolderExists(string projectName, string folderName)
        {
            var folders = GetRootProjectFolders(projectName);
            var exists = folders.Where(item => item.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return (exists != null);
        }

        private IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }
    }
}
