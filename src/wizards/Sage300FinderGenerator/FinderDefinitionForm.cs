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

using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sage.CA.SBS.ERP.Sage300.FinderGenerator
{
    public partial class FinderDefinitionForm : MetroFramework.Forms.MetroForm
    {
        private IDictionary<string, Project> projectLookup = new Dictionary<string, Project>();

        public FinderDefinitionForm(_Solution solution)
        {
            InitializeComponent();

            // Store the solution's path
            finderDefinitionControl.SolutionPath = Path.GetFullPath(solution.FileName);

            try
            {
                foreach (Project project in solution.Projects)
                {
                    var pattern = new Regex(@"^(?<company>\w+)\.(?<module>\w+)\.(?<projectName>\w+)");
                    var match = pattern.Match(project.Name);
                    var module = match.Groups["module"].ToString();
                    var projectName = match.Groups["projectName"].ToString();

                    if (projectName == "Web")
                    {
                        projectLookup.Add(module, project);
                    }
                }

                
            }
            catch
            { 
                //TODO log something???
            }
        }

        private void btnCreateFinderDef_Click(object sender, EventArgs e)
        {
            var finderDefinitionJObject = finderDefinitionControl.GetFinderDefinition();

            if (finderDefinitionJObject != null)
            {
                var finderFileString = 
                    @"/* Copyright (c) 2019-2024 Sage Software, Inc.  All rights reserved. */
// @ts-check
""use strict"";

                (function(sg, props, $) {

                    /**
                     * 
                     * Predefined setViewFinder() {@link SetViewFinderProperties} instances.
                     * 
                     * */
                    sg.viewFinderProperties = " + finderDefinitionJObject.ToString() + @";
})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);";

                var destinationFilePath = finderDefinitionControl.GetFinderDefinitionFilePath();

                if (!string.IsNullOrEmpty(destinationFilePath))
                {
                    File.WriteAllText(destinationFilePath, finderFileString);

                    MessageBox.Show($"New finder definition is created, please check file in {destinationFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No finder definition file is set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter all values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
