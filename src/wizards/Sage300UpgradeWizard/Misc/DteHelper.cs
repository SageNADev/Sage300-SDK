/* **********************************************************************************
*
* Copyright (c) Microsoft Corporation. All rights reserved.
*
* This source code is subject to terms and conditions of the Shared Source License
* for DSL Editor PowerToy. A copy of the license can be found in the License.htm file
* at the root of this distribution. If you can not locate the Shared Source License
* for DSL Editor PowerToy, please obtain a copy from: http://www.codeplex.com/dsltreegrideditor/Project/License.aspx.
* By using this source code in any fashion, you are agreeing to be bound by
* the terms of the Shared Source License for DSL Editor PowerToy.
*
* You must not remove this notice, or any other, from this software.
*
* **********************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using GATLib = Microsoft.Practices.RecipeFramework.Library;
using VSConstants = EnvDTE.Constants;

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    public static class DteHelper
    {
        #region Public Methods
        /// <summary>
        /// Returns the project item from the project given a specified relative path
        /// </summary>
        /// <param name="project"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ProjectItem FindProjectItemByName(Project project, string relativePath,
            ProjectItemType itemType)
        {
            //Get file from project
            return FindItemByName(project.ProjectItems, relativePath, itemType);
        }
        /// <summary>
        /// Returns list of folder or file path parts
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPathParts(string path)
        {
            List<string> parts = new List<string>();
            parts.AddRange(path.Split(new char[] { Path.DirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries));

            return parts;
        }
        /// <summary>
        /// Gets the value of an existing variable, 
        /// in the specified global collection
        /// </summary>
        [CLSCompliant(false)]
        public static string GetVariableFromGlobals(EnvDTE.Globals globals, string name)
        {
            if (false == IsGlobalsVariableExists(globals, name))
                throw new ArgumentOutOfRangeException("Name");
            else
                return globals[name].ToString();
        }
        /// <summary>
        /// Adds a new variable, or sets value of existing variable, 
        /// in the specified global collection
        /// </summary>
        [CLSCompliant(false)]
        public static void SetGlobalsVariable(EnvDTE.Globals globals, string name, string value)
        {
            //Set globals property
            globals[name] = value;
            globals.set_VariablePersists(name, true);
        }
        /// <summary>
        /// Determines if the specified globals collection contains the variable
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static bool IsGlobalsVariableExists(EnvDTE.Globals globals, string name)
        {
            return globals.get_VariableExists(name);
        }
        /// <summary>
        /// Removes the variable from the globals collection
        /// </summary>
        [CLSCompliant(false)]
        public static void RemoveGlobalsVariable(EnvDTE.Globals globals, string name)
        {
            globals[name] = null;
            globals.set_VariablePersists(name, false);
        }
        /// <summary>
        /// Returns the path of the primary build output target
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static string GetProjectTargetPath(Project project)
        {
            //Get build configuration OutputPath
            string outputPath = (string)GetProjectConfigurationProperty(project, "OutputPath");
            if (null == outputPath)
                return null;

            //Get project build path
            string buildPath = (string)GetProjectProperty(project, "LocalPath");
            if (null == buildPath)
                return null;

            //Get project build path
            string targetName = (string)GetProjectProperty(project, "OutputFileName");
            if (null == targetName)
                return null;

            //Create target path
            return Path.Combine(buildPath, Path.Combine(outputPath, targetName));
        }
        /// <summary>
        /// Returns the property of the project
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static object GetProjectProperty(Project project, string propertyName)
        {
            //Find property
            EnvDTE.Properties properties = project.Properties;
            if (null == properties)
                return null;

            foreach (Property property in properties)
            {
                try
                {
                    if (0 == string.Compare(property.Name, propertyName, true))
                        return property.Value;
                }
                catch
                {
                    //Ignore access exception
                }
            }

            return null;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Finds the project item within the current project heirarchy
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="name"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private static ProjectItem FindItemByName(ProjectItems projectItems, string itemPath,
            ProjectItemType itemType)
        {
            //Get list of file parts
            List<string> partNames = GetPathParts(itemPath);

            //Locate matching file
            ProjectItems currentProjectItems = projectItems;
            ProjectItem subItem = null;
            foreach (string partName in partNames)
            {
                //Find sub item
                subItem = GATLib.DteHelper.FindItemByName(currentProjectItems, partName, false);
                if (null == subItem)
                    return null;

                //Ensure only folders (if we want only folders)
                if ((ProjectItemType.Folder == itemType)
                    && (VSConstants.vsProjectItemKindPhysicalFolder != subItem.Kind))
                    return null;

                //Iterate to sub item
                currentProjectItems = subItem.ProjectItems;
            }

            //Ensure we got a file (if we wanted files)
            if ((ProjectItemType.File == itemType)
                && (VSConstants.vsProjectItemKindPhysicalFile != subItem.Kind))
                return null;

            return subItem;
        }
        /// <summary>
        /// Returns the property of the project
        /// </summary>
        /// <returns></returns>
        private static object GetProjectConfigurationProperty(Project project, string propertyName)
        {
            //Get the target output property
            if ((null == project.ConfigurationManager)
                || (null == project.ConfigurationManager.ActiveConfiguration))
                return null;

            //Find property
            EnvDTE.Properties properties = project.ConfigurationManager.ActiveConfiguration.Properties;
            foreach (Property property in properties)
            {
                try
                {
                    if (0 == string.Compare(property.Name, propertyName, true))
                        return property.Value;
                }
                catch
                {
                    //Ignore access exception
                }
            }

            return null;
        }
        #endregion

        #region Enums
        public enum ProjectItemType
        {
            File,
            Folder
        }
        #endregion
    }

}