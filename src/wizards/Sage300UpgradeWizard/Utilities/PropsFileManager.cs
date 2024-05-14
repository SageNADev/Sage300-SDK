// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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
using Microsoft.Build.Evaluation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Utilities
{
    /// <summary>
    /// Class to handle some AccpacDotNetVersion.props file related tasks
    /// </summary>
    public static class PropsFileManager
    {
        private const string CSPROJ_FILTER = "*.csproj";

        /// <summary>
        /// Find all copies of AccpacDotNetVersion.props that live in each project folder and 
        /// update the related <import> statement in the corresponding .csproj file to look in 
        /// the solution root folder instead.
        /// </summary>
        /// <param name="propsFileList">A list of prop file locations found</param>
        public static void UpdateAccpacPropsFileReferencesInProjects(IEnumerable<string> propsFileList)
        {
            // Extract the containing folder for each entry and build the list of .csproj files for alteration
            foreach (var item in propsFileList)
            {
                // Get the directory name
                var fileInfo = new FileInfo(item);
                var directoryName = fileInfo.DirectoryName;

                // Find the .csproj file contained within this directory
                var projectFiles = FileUtilities.EnumerateFiles(directoryName, CSPROJ_FILTER).ToArray();
                if (projectFiles.Count() > 0)
                {
                    foreach (var projectFilePath in projectFiles)
                    {
                        Project proj = null;
                        try
                        {
                            proj = new Project(projectFilePath, null, null, new ProjectCollection(), ProjectLoadSettings.IgnoreMissingImports);

                        }
                        catch (Microsoft.Build.Exceptions.InvalidProjectFileException ex)
                        {
                            // Likely the <import> statement couldn't be resolved. That's ok.

                            // Just a line to resolve compiler warning.
                            var msg = ex.Message; 

                            continue;
                        }

                        var imports = proj.Imports;
                        if (imports.Count > 0)
                        {
                            // We have some import statements. Let's find the AccpacDotNetVersion.props one so we can update it.
                            foreach (var import in imports)
                            {
                                if (import.ImportingElement.Project.Contains(Constants.Common.AccpacPropsFile))
                                {
                                    import.ImportingElement.Project = "$(SolutionDir)\\" + Constants.Common.AccpacPropsFile;

                                    // Add a conditional to the import statement
                                    //import.ImportingElement.Condition = "Exists('$(SolutionDir)\\AccpacDotNetVersion.props')";
                                }
                            }
                        }
                        proj.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Remove all copies of AccpacDotNetVersion.props that live in each project folder of the solution
        /// </summary>
        /// <param name="propsFileList">A list of prop file locations found</param>
        public static void RemoveAccpacPropsFromProjectFolders(IEnumerable<string> propsFileList)
        {
            if (propsFileList.Count() > 0)
            {
                foreach (var file in propsFileList)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Copy the AccpacDotNetProps file to the Solution folder
        /// </summary>
        public static void CopyAccpacPropsFileToSolutionFolder(Settings settings)
        {
            var sourcePath = Path.Combine(settings.PropsSourceFolder, Constants.Common.AccpacPropsFile);
            var destPath = Path.Combine(settings.DestinationSolutionFolder, Constants.Common.AccpacPropsFile);
            File.Copy(sourcePath, destPath, overwrite: true);
        }

        /// <summary>
        /// Is there a copy of the AccpacDotNetversion.props file in the Solution folder?
        /// </summary>
        /// <returns>
        /// true : AccpacDotNetVersion.props is in Solution folder 
        /// false: AccpacDotNetVersion.props is in not in the Solution folder 
        /// </returns>
        public static bool IsAccpacDotNetVersionPropsLocatedInSolutionFolder(Settings settings)
        {
            return File.Exists(Path.Combine(settings.DestinationSolutionFolder,
                                            Constants.Common.AccpacPropsFile));
        }
    }
}
