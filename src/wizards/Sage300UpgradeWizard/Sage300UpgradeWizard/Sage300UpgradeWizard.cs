// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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
using System.IO;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> Entry Point for Upgrade Wizard </summary>
    public class Sage300Upgrade
    {
        private static class Constants
        {
            public const string ItemsZipName = @"Items.zip";
            public const string CSharpName = @"CSharp";
            public const string GithubRepoName = @"Columbus-Web";
            public const string GithubRepoWebProjectName = @"Sage.CA.SBS.ERP.Sage300.Web";
        }

		/// <summary> Execute the Upgrade Wizard </summary>
        public void Execute(Solution solution)
        {
			var payloadFileName = Constants.ItemsZipName;
			var sln = (Solution2)solution;
			var templatePath = sln.GetProjectItemTemplate(payloadFileName, Constants.CSharpName);

            if (UpgradeWizard.Constants.PerRelease.ReportUpgrade_For_2021_2)
            {
                //
                // Developer Note: Without this block of code here, the similar block
                //                 in ProcessUpgrade.cs will crash. 
                //                 Cause unknown at this point.
                //
                //                 This code does not change any project within the solution
                // 
                foreach (Project project in sln.Projects)
                {
                    // Find the Web project in the solution
                    var webProjectName = project.FullName;
                    if (IsWebProject(webProjectName))
                    {
                        break;
                    }
                }
            }

            using (var form = new Upgrade(DestinationDefault(solution), DestinationWebDefault(solution), templatePath, sln))
			{
				form.ShowDialog();
			}
		}

        /// <summary>
        /// Check a path to see if it's the Web project file
        /// </summary>
        /// <param name="projectPath">The path to check</param>
        /// <returns></returns>
        private bool IsWebProject(string projectPath)
        {
            return projectPath.ToLowerInvariant().Contains(UpgradeWizard.Constants.Common.WebProjectNamePattern);
        }

        /// <summary> Get Destination default </summary>
        /// <param name="solution">Solution</param>
        /// <returns>Destination or Empty String</returns>
        public string DestinationDefault(Solution solution)
        {
            var retVal = string.Empty;

            try
            {
                // Get destination default from project
                retVal = Directory.GetParent(Path.GetFullPath(solution.FileName)).FullName;
            }
            catch
            {
                // Ignore
            }
            return retVal;
        }

        /// <summary> Get Destination Web default </summary>
        /// <param name="solution">Solution</param>
        /// <returns>Destination Web or Empty String</returns>
        public string DestinationWebDefault(Solution solution)
        {
            var retVal = string.Empty;

            try
            {
                var solutionPath = Path.GetFullPath(solution.FileName);
                var repoName = Constants.GithubRepoName;
                var repoProjectName = Constants.GithubRepoWebProjectName;
                var solutionParent = Directory.GetParent(solutionPath).FullName;

                // Get destination web default from project
                retVal = Path.Combine(Directory.GetParent(solutionParent).FullName, repoName, repoProjectName);
            }
            catch
            {
                // Ignore
            }
            return retVal;
        }
    }
}
