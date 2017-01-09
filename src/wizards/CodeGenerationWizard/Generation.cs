// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> UI for Code Generation Wizard </summary>
    public partial class Generation : Form
    {
        #region Private Vars

        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> Information processed </summary>
        private readonly BindingList<Info> _gridInfo = new BindingList<Info>();

        /// <summary> Dynamic Query Infomation </summary>
        private readonly BindingList<BusinessField> _dynamicQueryFields = new BindingList<BusinessField>();

        /// <summary> Report Infomation </summary>
        private BindingList<BusinessField> _reportFields = new BindingList<BusinessField>();

        /// <summary> Reports </summary>
        private readonly Dictionary<string, BindingList<BusinessField>> _reports =
            new Dictionary<string, BindingList<BusinessField>>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Sage color </summary>
        private readonly Color _sageColor = Color.FromArgb(3, 130, 104);

        /// <summary> Projects by Type within a Module </summary>
        private readonly Dictionary<string, Dictionary<string, ProjectInfo>> _projects = 
            new Dictionary<string, Dictionary<string, ProjectInfo>>();

        /// <summary> Single Copyright for generated files </summary>
        private string _copyright = string.Empty;

        /// <summary> Base Company Namespace for generated files </summary>
        private string _companyNamespace = string.Empty;

        /// <summary> Area Structure exists for Web Project </summary>
        private bool _doesAreasExist;

        /// <summary> Web Project includes Module </summary>
        private bool _webProjectIncludesModule;
        #endregion

        #region Delegates

        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        private delegate void ProcessingCallback(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        private delegate void StatusCallback(string fileName, Info.StatusType statusType, string text);

        #endregion

        #region Constructor

        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            InitWizardSteps(RepositoryType.Flat);
            InitInfo();
            InitDynamicQueryFields();
            InitReportFields();
            InitEvents();
            ProcessingSetup(true);
            Processing("");

            cboRepositoryType.Focus();
        }

        #endregion

        #region Public Methods

        /// <summary> Are the prerequisites valid for executing the wizard </summary>
        /// <param name="solution">Solution </param>
        /// <remarks>Solution must be a Sage 300 solution with known projects</remarks>
        /// <returns>True if valid otherwise false</returns>
        public bool ValidPrerequisites(Solution solution)
        {
            // Validate solution
            if (!ValidSolution(solution))
            {
                DisplayMessage(Resources.InvalidSolution, MessageBoxIcon.Error);
                return false;
            }

            // Validate projects
            if (!ValidProjects(solution))
            {
                DisplayMessage(Resources.InvalidProjects, MessageBoxIcon.Error);
                return false;
            }

            // Build modules dropdowns for validations
            BuildModules();

            return true;
        }


        #endregion

        #region Private Methods/Routines/Events
        /// <summary> Determine if the Solution is valid </summary>
        /// <param name="solution">Solution </param>
        /// <returns>True if valid otherwise false</returns>
        private static bool ValidSolution(_Solution solution)
        {
            // Validate solution
            return (solution != null);
        }
        
        /// <summary> Determine if Projects in Solution are valid </summary>
        /// <param name="solution">Solution </param>
        /// <returns>True if valid otherwise false</returns>
        private bool ValidProjects(_Solution solution)
        {
            try
            {
                // Locals
                //var solutionFolder = Path.GetDirectoryName(solution.FullName);
                var projects = GetProjects(solution);

                // Iterate solution to get projects for analysis and usage
                foreach (var project in projects)
                {
                    var projectName = Path.GetFileNameWithoutExtension(project.FullName);
                    if (string.IsNullOrEmpty(projectName))
                    {
                        continue;
                    }

                    var segments = projectName.Split('.');
                    var key = segments[segments.Length - 1];
                    var module = segments[segments.Length - 2];

                    // Grab the company namespace from the Business Repository project
                    if (key.Equals(ProcessGeneration.BusinessRepositoryKey))
                    {
                        _companyNamespace = projectName.Substring(0, projectName.IndexOf(module + "." + key, StringComparison.InvariantCulture) - 1);
                    }

                    // The Web project name is different from other ones. It should be derived from the folder name
                    if (key.Equals(ProcessGeneration.WebKey))
                    {
                        var parts = project.FullName.Split('\\');
                        // Last part is web project name
                        projectName = parts[parts.Length - 1].Replace(".csproj", string.Empty);

                        // Need to determine if area structure vs. non-area structure
                        var projectFolder = Path.GetDirectoryName(project.FullName);
                        if (!string.IsNullOrEmpty(projectFolder))
                        {
                            var areaPath = Path.Combine(projectFolder, "Areas");
                            var areaStructure = Directory.Exists(areaPath);

                            // Store for later use in generation
                            _doesAreasExist = areaStructure;
                            _webProjectIncludesModule = false;

                            if (areaStructure)
                            {
                                // Iterate directories looking for the "module folder
                                var directories = Directory.GetDirectories(areaPath);
                                foreach (var moduleParts in from directory in directories
                                                            let modulePath = Path.Combine(directory, "Constants")
                                                            where Directory.Exists(modulePath)
                                                            select directory.Split('\\'))
                                {
                                    module = moduleParts[moduleParts.Length - 1];
                                    
                                    // Determine if module is in project name
                                    _webProjectIncludesModule = projectName.Contains("." + module + ".");

                                    break;
                                }
                            }
                            else
                            {
                                // Non-Area structure project
                                module = parts[parts.Length - 2];
                            }
                        }

                        // We will use the copyright from this project for all generated files
                        _copyright = GetCopyright(project);
                    }

                    var projectInfo = new ProjectInfo
                    {
                        ProjectFolder = Path.GetDirectoryName(project.FullName),
                        ProjectName = projectName,
                        Project = project
                    };

                    // Add to list of projects by type and module
                    if (_projects.ContainsKey(key))
                    {
                        _projects[key].Add(module, projectInfo);
                    }
                    else
                    {
                        _projects.Add(key, new Dictionary<string, ProjectInfo> { { module, projectInfo } });
                    }
                }

            }
            catch
            {
                // No action as will be reviewed by caller                    
            }

            // Must have all projects to be valid
            return (_projects.ContainsKey(ProcessGeneration.BusinessRepositoryKey) &&
                    _projects.ContainsKey(ProcessGeneration.InterfacesKey) &&
                    _projects.ContainsKey(ProcessGeneration.ModelsKey) &&
                    _projects.ContainsKey(ProcessGeneration.ResourcesKey) &&
                    _projects.ContainsKey(ProcessGeneration.ServicesKey) &&
                    _projects.ContainsKey(ProcessGeneration.WebKey));
        }

        /// <summary> Gets projects </summary>
        /// <param name="solution">Solution </param>
        /// <returns>List of projects</returns>
        private static IEnumerable<Project> GetProjects(_Solution solution)
        {
            // Locals
            var projects = solution.Projects;
            var list = new List<Project>();
            var item = projects.GetEnumerator();

            try
            {
                // Iterate 
                while (item.MoveNext())
                {
                    var project = (Project)item.Current;

                    if (project == null)
                    {
                        continue;
                    }

                    // only add project folder
                    if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    {
                        list.AddRange(GetSolutionFolderProjects(project));
                    }
                    else
                    {
                        list.Add(project);
                    }
                }
            }
            catch
            {
                // No action as it will be reviewed by caller
            }

            return list;
        }

        /// <summary> Gets projects in solution folder </summary>
        /// <param name="project">Project </param>
        /// <returns>List of projects</returns>
        private static IEnumerable<Project> GetSolutionFolderProjects(Project project)
        {
            // Locals
            var list = new List<Project>();

            // Iterate projects
            for (var projectItem = 1; projectItem <= project.ProjectItems.Count; projectItem++)
            {
                var subProject = project.ProjectItems.Item(projectItem).SubProject;

                if (subProject == null)
                {
                    continue;
                }

                // Recursion for another solution folder
                if (subProject.Kind.Equals(ProjectKinds.vsProjectKindSolutionFolder))
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

        /// <summary> Get copyright of project </summary>
        /// <param name="project">Project </param>
        /// <returns>Copyright from AssemblyInfo of project</returns>
        private static string GetCopyright(Project project)
        {
            var retVal = string.Empty;

            try
            {
                // Iterate project looking for the AssemblyInfo class
                foreach (ProjectItem projectItem in project.ProjectItems)
                {
                    if (projectItem.Name.Equals("Properties"))
                    {
                        foreach (ProjectItem assemblyItem in projectItem.ProjectItems)
                        {
                            retVal = GetCopyrightAttribute(assemblyItem.Properties.Item("LocalPath").Value.ToString());
                            break;
                        }

                    }

                    // Break early out of outer loop
                    if (!string.IsNullOrEmpty(retVal))
                    {
                        break;
                    }
                }
            }
            catch
            {
                // Swallow error
            }

            return retVal;
        }

        /// <summary> Get copyright attribute </summary>
        /// <param name="fileName">Assembly Info file name </param>
        /// <returns>Copyright from AssemblyInfo of project</returns>
        private static string GetCopyrightAttribute(string fileName)
        {
            // Locals
            var retVal = string.Empty;

            try
            {
                // Read resource info file
                var lines = File.ReadAllLines(@fileName);

                // Iterate and search for "AssemblyCopyright attribute
                foreach (var parsedLine in from line in lines where line.Contains("AssemblyCopyright") select line.Split('"'))
                {
                    if (parsedLine.Length > 0)
                    {
                        retVal = parsedLine[1];
                    }
                    break;
                }
            }
            catch
            {
                // Swallow error
            }

            return retVal;
        }

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            _generation = new ProcessGeneration();
            _generation.ProcessingEvent += ProcessingEvent;
            _generation.StatusEvent += StatusEvent;

            // Default to Flat Repository
            cboRepositoryType.SelectedIndex = Convert.ToInt32(RepositoryType.Flat);
        }

        #region Toolbar Events

        /// <summary> Next/Generate toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        /// <summary> Back toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Back wizard step</remarks>
        private void btnBack_Click(object sender, EventArgs e)
        {
            BackStep();
        }

        #endregion

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            var repositoryType =
                (RepositoryType)Enum.Parse(typeof(RepositoryType), cboRepositoryType.SelectedIndex.ToString());

            // Finished?
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlGeneratedCode"))
            {
                _generation.Dispose();
                Close();
            }
            else
            {
                // Proceed to next wizard step or start code generation if last step
                if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlGenerateCode"))
                {
                    // Build settings and validate that settings have been selected 
                    // (view, resx name, user, password, version, company))
                    txtViewID.Text = txtViewID.Text.ToUpper();
                    txtViewID.Refresh();

                    var settings = BuildSettings();
                    var invalidSetting = ValidSettings(settings);
                    if (string.IsNullOrEmpty(invalidSetting))
                    {
                        // Setup display before processing
                        _gridInfo.Clear();
                        ProcessingSetup(false);
                        grdResourceInfo.DataSource = _gridInfo;
                        grdResourceInfo.Refresh();

                        _rowIndex = -1;

                        // Start background worker for processing (async)
                        wrkBackground.RunWorkerAsync(settings);
                    }
                    else
                    {
                        // Something is invalid
                        DisplayMessage(invalidSetting, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Proceed to next step
                    if (!_currentWizardStep.Equals(-1))
                    {
                        btnBack.Enabled = true;

                        _wizardSteps[_currentWizardStep].Panel.Visible = false;
                        _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.None;
                    }

                    _currentWizardStep++;

                    // Enable disable the next button for View step
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlBusinessView"))
                    {
                        btnNext.Enabled = !string.IsNullOrEmpty(txtViewID.Text);
                    }

                    // Enable disable finder option
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlOptions"))
                    {
                        chkGenerateFinder.Enabled = (!repositoryType.Equals(RepositoryType.Report) &&
                                                     !repositoryType.Equals(RepositoryType.Process) &&
                                                     !repositoryType.Equals(RepositoryType.DynamicQuery));
                        // If not enabled, then uncheck it
                        if (!chkGenerateFinder.Enabled)
                        {
                            chkGenerateFinder.Checked = false;
                        }
                    }

                    // Attempt to default Resx Name
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlResource"))
                    {
                        var defaultName = string.Empty;

                        // Resx name based upon type
                        switch (repositoryType)
                        {
                            case RepositoryType.DynamicQuery:
                                if (!string.IsNullOrEmpty(txtDynamicQueryName.Text))
                                {
                                    defaultName = txtDynamicQueryName.Text.Trim() + "Resx";
                                }
                                break;
                            case RepositoryType.Report:
                                if (!string.IsNullOrEmpty(txtReportName.Text))
                                {
                                    defaultName = txtReportName.Text.Trim() + "Resx";
                                }
                                break;
                            default:
                                defaultName = txtViewName.Text.Trim() + "Resx";
                                break;
                        }

                        // Assign to control
                        txtResxName.Text = defaultName;
                    }

                    _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.Fill;
                    _wizardSteps[_currentWizardStep].Panel.Visible = true;

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlGenerateCode"))
                    {
                        btnNext.Text = Resources.Generate;
                    }

                    // Update title and text for step
                    lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash + _wizardSteps[_currentWizardStep].Title;
                    lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;

                    splitBase.Panel2.Refresh();
                }
            }
        }

        /// <summary> Back Navigation </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            // Proceed to next wizard step or start code generation if last step
            if (!_currentWizardStep.Equals(0))
            {
                // Proceed back a step
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlGeneratedCode"))
                {
                    btnNext.Text = Resources.Generate;
                }
                else
                {
                    btnNext.Text = Resources.Next;
                }

                _wizardSteps[_currentWizardStep].Panel.Visible = false;
                _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.None;

                _currentWizardStep--;

                _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.Fill;
                _wizardSteps[_currentWizardStep].Panel.Visible = true;

                // Enable back button?
                if (_currentWizardStep.Equals(0))
                {
                    btnBack.Enabled = false;
                    btnNext.Enabled = true;
                }

                // Update title and text for step
                lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash + _wizardSteps[_currentWizardStep].Title;
                lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;

                splitBase.Panel2.Refresh();

            }
        }

        /// <summary> Initialize wizard steps </summary>
        /// <param name="repositoryType">Repository Type</param>
        private void InitWizardSteps(RepositoryType repositoryType)
        {
            // Default
            btnNext.Text = Resources.Next;
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlCodeType);
            InitPanel(pnlBusinessView);
            InitPanel(pnlDynamicQuery);
            InitPanel(pnlReport);
            InitPanel(pnlResource);
            InitPanel(pnlOptions);
            InitPanel(pnlGenerateCode);
            InitPanel(pnlGeneratedCode);

            // Test color for helpful labels
            lblCodeTypeDescriptionHelp.ForeColor = _sageColor;
            lblUnknownCodeTypeFilesHelp.ForeColor = _sageColor;
            lblCodeTypeFilesHelp.ForeColor = _sageColor;

            lblBusinessViewHelp.ForeColor = _sageColor;
            lblViewModuleHelp.ForeColor = _sageColor;
            lblCredentialsHelp.ForeColor = _sageColor;
            lblViewNameHelp.ForeColor = _sageColor;

            lblDynamicQueryViewHelp.ForeColor = _sageColor;
            lblDynamicQueryModuleHelp.ForeColor = _sageColor;
            lblDynamicQueryNameHelp.ForeColor = _sageColor;
            lblDynamicQueryModelNameHelp.ForeColor = _sageColor;
            lblDynamicQueryGridHelp.ForeColor = _sageColor;

            lblReportsHelp.ForeColor = _sageColor;
            lblReportModuleHelp.ForeColor = _sageColor;
            lblReportNameHelp.ForeColor = _sageColor;
            lblReportModelNameHelp.ForeColor = _sageColor;
            lblReportProgramNameHelp.ForeColor = _sageColor;

            lblResourceNameDefaultHelp.ForeColor = _sageColor;
            lblResourceNameSuffixHelp.ForeColor = _sageColor;
            lblResourceNameFilesHelp.ForeColor = _sageColor;
            lblResourceNameOtherFilesHelp.ForeColor = _sageColor;

            lblOptionsFinderHelp.ForeColor = _sageColor;
            lblOptionsDynamicEnableHelp.ForeColor = _sageColor;
            lblOptionsPrompIfExistsHelp.ForeColor = _sageColor;

            lblGenerateHelp.ForeColor = _sageColor;

            // Every repository type will have the first step
            AddStep(Resources.StepTitleCodeType, Resources.StepDescriptionCodeType, pnlCodeType);

            // Assign steps based upon repository type
            switch (repositoryType)
            {
                case RepositoryType.Flat:
                case RepositoryType.Inquiry:
                case RepositoryType.Process:
                    AddStep(Resources.StepTitleBusinessView, Resources.StepDescriptionBusinessView, pnlBusinessView);
                    break;

                case RepositoryType.DynamicQuery:
                    AddStep(Resources.StepTitleDynamicQuery, Resources.StepDescriptionDynamicQuery, pnlDynamicQuery);
                    break;

                case RepositoryType.Report:
                    AddStep(Resources.StepTitleReport, Resources.StepDescriptionReport, pnlReport);
                    break;
            }

            // Every repository type will have the first step
            AddStep(Resources.StepTitleResource, Resources.StepDescriptionResource, pnlResource);
            AddStep(Resources.StepTitleOptions, Resources.StepDescriptionOptions, pnlOptions);
            AddStep(Resources.StepTitleGenerateCode, Resources.StepDescriptionGenerateCode, pnlGenerateCode);
            AddStep(Resources.StepTitleGeneratedCode, Resources.StepDescriptionGeneratedCode, pnlGeneratedCode);

            // Display first step
            NextStep();
        }

        /// <summary> Add wizard steps </summary>
        /// <param name="title">Title for wizard step</param>
        /// <param name="description">Description for wizard step</param>
        /// <param name="panel">Panel for wizard step</param>
        private void AddStep(string title, string description, Panel panel)
        {
            _wizardSteps.Add(new WizardStep
            {
                Title = title,
                Description = description,
                Panel = panel
            });
        }

        /// <summary> Initialize panel </summary>
        private void InitPanel(Panel panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
        }

        /// <summary> Initialize info and modify grid display </summary>
        private void InitInfo()
        {
            // Assign binding to datasource (two binding)
            grdResourceInfo.DataSource = _gridInfo;

            // Assign widths and localized text
            grdResourceInfo.Columns[Info.FileNameColumnNo].Width = 350;
            grdResourceInfo.Columns[Info.FileNameColumnNo].HeaderText = Resources.FileName;

            grdResourceInfo.Columns[Info.StatusColumnNo].Width = 50;
            grdResourceInfo.Columns[Info.StatusColumnNo].ReadOnly = true;
            grdResourceInfo.Columns[Info.StatusColumnNo].HeaderText = Resources.Status;
        }

        /// <summary> Initialize dynamic query info and modify grid display </summary>
        private void InitDynamicQueryFields()
        {
            // Assign binding to datasource (two binding)
            grdDynamicQueryFields.DataSource = _dynamicQueryFields;

            // Assign widths and localized text
            GenericInit(grdDynamicQueryFields, 0, 50, Resources.ID, false, false);
            GenericInit(grdDynamicQueryFields, 1, 150, Resources.Field, false, false);
            GenericInit(grdDynamicQueryFields, 2, 150, Resources.Field, true, false);
            GenericInit(grdDynamicQueryFields, 3, 290, Resources.Description, false, false);

            // Remove and re-add as combobox column
            grdDynamicQueryFields.Columns.Remove("Type");
            var column = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = Resources.Type,
                DropDownWidth = 100,
                Width = 75,
                FlatStyle = FlatStyle.Flat
            };
            // Add enums to drop down list
            foreach (var businessDataType in Enum.GetValues(typeof(BusinessDataType)).Cast<BusinessDataType>().Where(businessDataType => !businessDataType.Equals(BusinessDataType.Enumeration)))
            {
                column.Items.Add(businessDataType);
            }

            // Re-add column
            grdDynamicQueryFields.Columns.Insert(4, column);
            grdDynamicQueryFields.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            GenericInit(grdDynamicQueryFields, 5, 50, Resources.Size, true, false);
            GenericInit(grdDynamicQueryFields, 6, 75, Resources.IsReadOnly, false, false);
            GenericInit(grdDynamicQueryFields, 7, 75, Resources.IsCalculated, false, false);
            GenericInit(grdDynamicQueryFields, 8, 75, Resources.IsRequired, false, false);
            GenericInit(grdDynamicQueryFields, 9, 75, Resources.IsKey, false, false);
            GenericInit(grdDynamicQueryFields, 10, 75, Resources.IsUpperCase, false, false);
            GenericInit(grdDynamicQueryFields, 11, 75, Resources.IsAlphaNumeric, false, false);
            GenericInit(grdDynamicQueryFields, 12, 75, Resources.IsNumeric, false, false);
            GenericInit(grdDynamicQueryFields, 13, 75, Resources.IsDynamicEnablement, false, false);
        }

        /// <summary> Initialize report info and modify grid display </summary>
        private void InitReportFields()
        {
            // Assign binding to datasource (two binding)
            grdReportFields.DataSource = _reportFields;

            // Assign widths and localized text
            GenericInit(grdReportFields, 0, 50, Resources.ID, false, false);
            GenericInit(grdReportFields, 1, 150, Resources.ServerField, true, true);
            GenericInit(grdReportFields, 2, 150, Resources.Field, true, false);
            GenericInit(grdReportFields, 3, 290, Resources.Description, false, false);

            // Remove and re-add as combobox column
            grdReportFields.Columns.Remove("Type");
            var column = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = Resources.Type,
                DropDownWidth = 100,
                Width = 75,
                FlatStyle = FlatStyle.Flat
            };
            // Only add string type
            column.Items.Add(BusinessDataType.String);

            // Re-add column
            grdReportFields.Columns.Insert(4, column);
            grdReportFields.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdReportFields.Columns[4].Visible = false;

            GenericInit(grdReportFields, 5, 50, Resources.Size, true, false);
            GenericInit(grdReportFields, 6, 75, Resources.IsReadOnly, false, false);
            GenericInit(grdReportFields, 7, 75, Resources.IsCalculated, false, false);
            GenericInit(grdReportFields, 8, 75, Resources.IsRequired, false, false);
            GenericInit(grdReportFields, 9, 75, Resources.IsKey, false, false);
            GenericInit(grdReportFields, 10, 75, Resources.IsUpperCase, false, false);
            GenericInit(grdReportFields, 11, 75, Resources.IsAlphaNumeric, false, false);
            GenericInit(grdReportFields, 12, 75, Resources.IsNumeric, false, false);
            GenericInit(grdReportFields, 13, 75, Resources.IsDynamicEnablement, false, false);
        }

        /// <summary> Generic init for grid </summary>
        /// <param name="grid">Grid control</param>
        /// <param name="column">Column Number</param>
        /// <param name="width">Column Width</param>
        /// <param name="text">Header Text</param>
        /// <param name="visible">True for visible otherwise False</param>
        /// <param name="readOnly">True for read only otherwise False</param>
        private static void GenericInit(DataGridView grid, int column, int width, string text, bool visible, bool readOnly)
        {
            grid.Columns[column].Width = width;
            grid.Columns[column].HeaderText = text;
            grid.Columns[column].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[column].Visible = visible;
            grid.Columns[column].ReadOnly = readOnly;

            // Show read only in InactiveCaption color
            if (readOnly)
            {
                grid.Columns[column].DefaultCellStyle.BackColor = SystemColors.InactiveCaption;
            }
        }

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            tbrMain.Enabled = enableToolbar;
            tbrMain.Refresh();
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.GeneratingFile, text);

            tbrMain.Refresh();
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display in status bar</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void ProcessingEvent(string text)
        {
            var callBack = new ProcessingCallback(Processing);
            Invoke(callBack, text);
        }

        /// <summary> Update status display </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void Status(string fileName, Info.StatusType statusType, string text)
        {
            // Add to info list
            var info = new Info()
            {
                FileName = fileName
            };

            info.SetStatus(statusType);

            _gridInfo.Add(info);

            // rebind to grid
            grdResourceInfo.DataSource = _gridInfo;

            // Incrememnt row
            _rowIndex++;

            // Set status and text into tool tip for cell
            grdResourceInfo.CurrentCell = grdResourceInfo[Info.StatusColumnNo, _rowIndex];
            grdResourceInfo.CurrentCell.ToolTipText = text;

            grdResourceInfo.Refresh();
        }

        /// <summary> Update status display </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void StatusEvent(string fileName, Info.StatusType statusType, string text)
        {
            var callBack = new StatusCallback(Status);
            Invoke(callBack, fileName, statusType, text);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Build settings for background worker </summary>
        /// <returns>Settings</returns>
        private Settings BuildSettings()
        {
            var businessView = new BusinessView();
            var repositoryType =
                (RepositoryType) Enum.Parse(typeof (RepositoryType), cboRepositoryType.SelectedIndex.ToString());

            // Dynamic Query is not based upon ACCPAC view
            if (repositoryType.Equals(RepositoryType.DynamicQuery))
            {
                businessView.Properties.Add(BusinessView.ViewId, txtDynamicQueryViewID.Text.Trim());
                businessView.Properties.Add(BusinessView.ModuleId, cboDynamicQueryModule.Text.Trim());
                businessView.Properties.Add(BusinessView.ModelName, txtDynamicQueryModelName.Text.Trim());
                businessView.Properties.Add(BusinessView.EntityName, txtDynamicQueryName.Text.Trim());
                businessView.Fields = _dynamicQueryFields.ToList();
            }

            // Report is not based upon ACCPAC view
            else if (repositoryType.Equals(RepositoryType.Report))
            {
                var reportKey = cboReportKeys.Text.Trim();

                businessView.Properties.Add(BusinessView.ViewId, Guid.NewGuid().ToString());
                businessView.Properties.Add(BusinessView.ReportKey, reportKey);
                businessView.Properties.Add(BusinessView.ModuleId, cboReportModule.Text.Trim());
                businessView.Properties.Add(BusinessView.ModelName, txtReportModelName.Text.Trim());
                businessView.Properties.Add(BusinessView.EntityName, txtReportName.Text.Trim());
                businessView.Properties.Add(BusinessView.ProgramId, txtReportProgramId.Text.Trim().ToUpper());
                businessView.Fields = _reportFields.ToList();
            }

            // ACCPAC view
            else
            {
                businessView.Properties.Add(BusinessView.EntityName, txtViewName.Text.Trim());
            }

            // Append resx suffix if not supplied
            if (!txtResxName.Text.Trim().EndsWith(Resources.Resx))
            {
                txtResxName.Text = txtResxName.Text + Resources.Resx;
            }

            return new Settings
            {
                ViewId = txtViewID.Text.Trim(),
                User = txtUser.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Version = txtVersion.Text.Trim(),
                Company = txtCompany.Text.Trim(),
                GenerateFinder = chkGenerateFinder.Checked,
                RepositoryType = repositoryType,
                BusinessView = businessView,
                GenerateDynamicEnablement = chkGenerateDynamicEnablement.Checked,
                ResxName = txtResxName.Text.Trim(),
                PromptIfExists = chkPromptIfExists.Checked,
                ModuleId = cboViewModule.Text.Trim(),
                Projects = _projects,
                Copyright = _copyright,
                CompanyNamespace = _companyNamespace,
                Extension = GetExtension(repositoryType),
                ResourceExtension = GetResourceExtension(repositoryType),
                DoesAreasExist = _doesAreasExist,
                WorkflowKindId = (repositoryType.Equals(RepositoryType.Process)) ? Guid.NewGuid() : Guid.Empty,
                WebProjectIncludesModule = _webProjectIncludesModule
            };
        }

        /// <summary> Get Extension </summary>
        /// <param name="repositoryType">Repository Type</param>
        /// <returns>.Process or .Reports or string.Empty</returns>
        private static string GetExtension(RepositoryType repositoryType)
        {
            var extension = string.Empty;

            if (repositoryType.Equals(RepositoryType.Process))
            {
                extension = ".Process";
            }
            else if (repositoryType.Equals(RepositoryType.Report))
            {
                extension = ".Reports";
            }

            return extension;
        }

        /// <summary> Get Resource Extension </summary>
        /// <param name="repositoryType">Repository Type</param>
        /// <returns>.Process or .Reports or .Forms</returns>
        private static string GetResourceExtension(RepositoryType repositoryType)
        {
            var extension = ".Forms";

            if (repositoryType.Equals(RepositoryType.Process))
            {
                extension = ".Process";
            }
            else if (repositoryType.Equals(RepositoryType.Report))
            {
                extension = ".Reports";
            }

            return extension;
        }

        /// <summary> Validate the settings </summary>
        /// <param name="settings">Settings</param>
        /// <returns>Empty if valid otherwise message</returns>
        private string ValidSettings(Settings settings)
        {
            var view = new BusinessView();
            return _generation.ValidSettings(settings, ref view);
        }

        /// <summary> Build Module Lists </summary>
        private void BuildModules()
        {
            // Clear first
            cboViewModule.Items.Clear();
            cboDynamicQueryModule.Items.Clear();
            cboReportModule.Items.Clear();

            // Add empty item at top of list
            cboViewModule.Items.Add(string.Empty);
            cboDynamicQueryModule.Items.Add(string.Empty);
            cboReportModule.Items.Add(string.Empty);

            // Iterate "Models" project(s) for modules belonging to the solution
            foreach (var projectInfo in _projects[ProcessGeneration.ModelsKey])
            {
                cboViewModule.Items.Add(projectInfo.Key);
                cboDynamicQueryModule.Items.Add(projectInfo.Key);
                cboReportModule.Items.Add(projectInfo.Key);

                if (!_projects[ProcessGeneration.ModelsKey].Count.Equals(1))
                {
                    continue;
                }

                // Default if only 1 module is discovered
                cboViewModule.SelectedIndex = 1;
                cboDynamicQueryModule.SelectedIndex = 1;
                cboReportModule.SelectedIndex = 1;
            }

        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            _generation.Process((Settings)e.Argument);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingSetup(true);
            Processing("");
            _generation.Dispose();

            _wizardSteps[_currentWizardStep].Panel.Visible = false;
            _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.None;

            _currentWizardStep++;

            _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.Fill;
            _wizardSteps[_currentWizardStep].Panel.Visible = true;

            // Update title and text for step
            lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash + _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;

            splitBase.Panel2.Refresh();

            // Display final step
            btnNext.Text = Resources.Finish;
        }

        /// <summary> Add a row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            _dynamicQueryFields.Add(new BusinessField());
        }

        /// <summary> Delete the current row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdDynamicQueryFields.CurrentRow != null)
            {
                grdDynamicQueryFields.Rows.Remove(grdDynamicQueryFields.CurrentRow);
            }
        }

        /// <summary> Delete all rows toolbar button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRows_Click(object sender, EventArgs e)
        {
            foreach (var row in grdDynamicQueryFields.Rows)
            {
                grdDynamicQueryFields.Rows.Remove((DataGridViewRow)row);
            }
        }

        /// <summary> Enable/Disable tab pages based upon selection</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboRepositoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitWizardSteps(
                (RepositoryType) Enum.Parse(typeof (RepositoryType), ((ComboBox) sender).SelectedIndex.ToString()));
        }

        /// <summary> Get report info from file </summary>
        /// <param name="fileName">Report ini file name</param>
        private void AddReports(string fileName)
        {
            // Validate name first
            if (string.IsNullOrEmpty(fileName))
            {
                // Report INI file is invalid
                DisplayMessage(Resources.InvalidReportSetting, MessageBoxIcon.Error);
                return;
            }

            // Initialize first
            cboReportKeys.Text = string.Empty;
            cboReportKeys.Items.Clear();
            _reportFields.Clear();
            _reports.Clear();
            

            try
            {
                // Read report ini file
                var lines = File.ReadAllLines(@fileName);
                var reportFound = false;
                var inReportFields = false;
                BindingList<BusinessField> bindingList = null;
                var reportName = string.Empty;

                // Iterate and look for reports
                foreach (var line in lines)
                {
                    // If a report was found [xxxxxxxx], look for the fields
                    if (reportFound)
                    {
                        // If fields were found #=..., process the fields
                        if (inReportFields)
                        {
                            // Evaluate to determine field, non-field or end of report fields

                            // End of report?
                            if (string.IsNullOrEmpty(line))
                            {
                                // Add to dictionary and reset
                                _reports.Add(reportName, bindingList);
                                reportFound = false;
                                inReportFields = false;
                                bindingList = null;
                                reportName = string.Empty;
                            }
                            else
                            {
                                // Add the field
                                AddReportField(line, bindingList);
                            }

                        }
                        else
                        {
                            // Is this the start of the report fields?
                            if (line.StartsWith("2="))
                            {
                                // The start of the report fields
                                inReportFields = true;

                                // Add the field
                                AddReportField(line, bindingList);
                            }
                        }
                    }
                    else
                    {
                        // Is this a report line?
                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            // A report was found
                            reportFound = true;
                            reportName = line.Replace("[", "").Replace("]", "");
                            bindingList = new BindingList<BusinessField>();
                        }
                    }

                    // Get next line
                }

                // Set reports found into control
                var list = _reports.Keys.ToList();
                list.Sort();

                foreach (var key in list)
                {
                    cboReportKeys.Items.Add(key);
                }
            }
            catch
            {
                DisplayMessage(Resources.InvalidReportSetting, MessageBoxIcon.Error);
            }

        }

        /// <summary> Get report field info from line </summary>
        /// <param name="line">Line in file to be parsed</param>
        /// <param name="bindingList">Binding list containing field</param>
        private static void AddReportField(string line, ICollection<BusinessField> bindingList)
        {
            try
            {
                // Split based upon '=' delimiter
                var parsedLine = line.Split('=');

                // Ensure it is a field (i.e. [0] will be numeric)
                if (Convert.ToInt32(parsedLine[0].Trim()) > 0)
                {
                    var parsedField = parsedLine[1].Split(' ');
                    var fieldName = parsedField[0].Trim().Replace("?", "");
                    var newFieldName = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1).ToLower();

                    var businessField = new BusinessField
                    {
                        Id = Convert.ToInt32(parsedLine[0].Trim()),
                        ServerFieldName = fieldName,
                        Name = newFieldName,
                        Type = BusinessDataType.String
                    };

                    bindingList.Add(businessField);
                }
            }
            catch
            {
                // Non-numeric field ends up here and will be skipped
            }

        }

        /// <summary> Display fields for selected report</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboReportKeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Locals
            var reportName = ((ComboBox) sender).Text;

            if (!string.IsNullOrEmpty(reportName))
            {
                _reportFields = _reports[reportName];
                grdReportFields.DataSource = _reportFields;
                txtReportProgramId.Text = reportName;
            }
        }
        
        /// <summary> Get Report Info from report.ini file</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnLoadIniFile_Click(object sender, EventArgs e)
        {
            // Get report information from ini file
            AddReports(txtReportIniFile.Text.Trim());
        }

        /// <summary> Report INI search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnIniDialog_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.Filter,
                FilterIndex = 1,
                Multiselect = false
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtReportIniFile.Text = dialog.FileName.Trim();
            // Get report information from ini file
            AddReports(txtReportIniFile.Text);
        }

        /// <summary> Add gradient to top panel</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void splitBase_Panel1_Paint(object sender, PaintEventArgs e)
        {
            FillGradient(e);
        }

        /// <summary> Add gradient to toolbar</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void tbrMain_Paint(object sender, PaintEventArgs e)
        {
            FillGradient(e);
        }

        /// <summary> Add gradient</summary>
        /// <param name="e">Event Args </param>
        private void FillGradient(PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(ClientRectangle,
                                                           _sageColor,
                                                           Color.White,
                                                           LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        /// <summary> Text Changed for Business View Step</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disable next button</remarks>
        private new void TextChanged(object sender, EventArgs e)
        {
            // Disable Next Button as text is changing in certain controls
            btnNext.Enabled = false;
        }

        /// <summary> Text Changed for View Name</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Enable next button</remarks>
        private void txtViewName_TextChanged(object sender, EventArgs e)
        {
            // Enable Next Button as text is changing 
            btnNext.Enabled = true;
        }

        /// <summary> Validate the view id and set default view name</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disable next button</remarks>
        private void txtViewID_Leave(object sender, EventArgs e)
        {
            txtViewName.Text = ProcessGeneration.GetDefaultName(txtUser.Text.Trim(), txtPassword.Text.Trim(),
                txtCompany.Text.Trim(), txtVersion.Text.Trim(), txtViewID.Text.Trim().ToUpper());

            btnNext.Enabled = (!string.IsNullOrEmpty(txtViewName.Text));
        }

        /// <summary> Help Button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disabled help until DPP wiki is available</remarks>
        private void Generation_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            // Display wiki link
            // System.Diagnostics.Process.Start(Resources.Browser, Resources.WikiLink);
        }

     #endregion



    }
}
