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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties;
using ACCPAC.Advantage;
using System.Xml.Linq;

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

        /// <summary> Entity Fields </summary>
        private readonly BindingList<BusinessField> _entityFields = new BindingList<BusinessField>();

        /// <summary> Entity Compositions </summary>
        private readonly BindingList<Composition> _entityCompositions = new BindingList<Composition>();

        /// <summary> Reports </summary>
        private readonly Dictionary<string, BindingList<BusinessField>> _reports =
            new Dictionary<string, BindingList<BusinessField>>();

        /// <summary> Entities </summary>
        private readonly List<BusinessView> _entities = new List<BusinessView>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep = -1;

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

        /// <summary> Include English Resource </summary>
        private bool _includeEnglish;

        /// <summary> Include Chinese Simplified Resource </summary>
        private bool _includeChineseSimplified;

        /// <summary> Include Chinese Traditional Resource </summary>
        private bool _includeChineseTraditional;

        /// <summary> Include Spanish Resource </summary>
        private bool _includeSpanish;

        /// <summary> Include French Resource </summary>
        private bool _includeFrench;

        /// <summary> Menu Item for Add Entity </summary>
        private readonly MenuItem _addEntityMenuItem = new MenuItem(Resources.AddEntity);

        /// <summary> Menu Item for Edit Entity </summary>
        private readonly MenuItem _editEntityMenuItem = new MenuItem(Resources.EditEntity);

        /// <summary> Menu Item for Delete Entity </summary>
        private readonly MenuItem _deleteEntityMenuItem = new MenuItem(Resources.DeleteEntity);

        /// <summary> Menu Item for Delete Entities </summary>
        private readonly MenuItem _deleteEntitiesMenuItem = new MenuItem(Resources.DeleteEntities);

        /// <summary> Menu Item for Edit Container Name </summary>
        private readonly MenuItem _editContainerName = new MenuItem(Resources.EditContainerName);

        /// <summary> Mode Type for Add, Add Above, Add Below, Edit or None </summary>
        private ModeType _modeType = ModeType.None;

        /// <summary> Clicked Entity Node </summary>
        private TreeNode _clickedEntityTreeNode;

        /// <summary> Context Menu </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();

        /// <summary> XDocument for processing to understand hierarchy </summary>
        private XDocument _xmlEntities;

        /// <summary> Entities Container Name </summary>
        private string _entitiesContainerName;

        /// <summary> Header node in the tree </summary>
        private XElement _headerNode;

        /// <summary> All compositions checkbox </summary>
        private CheckBox _allCompositions;

        /// <summary> Skip Click in Compositions header </summary>
        private bool _skipAllCompositionsClick = false;

        /// <summary> Processing in progress </summary>
        private bool _processingInProgress = false;
        #endregion

        #region Private Constants
        /// <summary> Splitter Distance </summary>
        private const int SplitterDistance = 415;

        /// <summary> Panel Name for pnlCodeType </summary>
        private const string PanelCodeType = "pnlCodeType";

        /// <summary> Panel Name for pnlEntities </summary>
        private const string PanelEntities = "pnlEntities";

        /// <summary> Panel Name for pnlGenerated </summary>
        private const string PanelGenerated = "pnlGeneratedCode";

        /// <summary> Panel Name for pnlGenerate </summary>
        private const string PanelGenerateCode = "pnlGenerateCode";
        #endregion

        #region Private Enums
        /// <summary>
        /// Enum for Mode Types
        /// </summary>
        private enum ModeType
        {
            /// <summary> No Mode </summary>
            None = 0,

            /// <summary> Add Mode </summary>
            Add = 1,

            /// <summary> Edit Mode</summary>
            Edit = 2
        }
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
            Localize();
            InitEvents();
            InitInfo();
            //ProcessingSetup(true);
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

        /// <summary> Setup the entities tree</summary>
        private void SetupEntitiesTree()
        {
            // Ensure all nodes are cleaned up first
            _entities.Clear();

            // Clear tree and container name first
            treeEntities.Nodes.Clear();
            _entitiesContainerName = null;

            // Add top level node
            var entitiesNode = new TreeNode(BuildEntitiesText()) { Name = ProcessGeneration.ElementEntities };
            treeEntities.Nodes.Add(entitiesNode);

            // Disable entity controls
            EnableEntityControls(false);
        }

        /// <summary> Validate Step before proceeding to next step </summary>
        /// <returns>True for valid step other wise false</returns>
        private bool ValidateStep()
        {
            // Locals
            var valid = string.Empty;

            // Code Type Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelCodeType))
            {
                valid = ValidCodeTypeStep();
            }

            // Entities Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelEntities))
            {
                valid = ValidEntitiesStep();
            }

            if (!string.IsNullOrEmpty(valid))
            {
                // Something is invalid
                DisplayMessage(valid, MessageBoxIcon.Error);
            }

            return string.IsNullOrEmpty(valid);
        }

        /// <summary> Valid CodeType Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidCodeTypeStep()
        {
            // Session - for code types that need to open a session to authenticate credentials
            // If code type doesn't need to authenticate, this is automatically OK
            var sessionValid = string.Empty;

            // Module - check for all code types
            if (string.IsNullOrEmpty(cboModule.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Module.Replace(":", ""));
            }

            // Only perform additional validation on code types that require credentials
            // Currently, this excludes Dynamic Query and Report types
            if (grpCredentials.Enabled)
            {
                // User ID
                if (string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.User.Replace(":", ""));
                }

                // Version
                if (string.IsNullOrEmpty(txtVersion.Text.Trim()))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Version.Replace(":", ""));
                }

                // Company
                if (string.IsNullOrEmpty(txtCompany.Text.Trim()))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Company.Replace(":", ""));
                }

                try
                {
                    // Init session to see if credentials are valid
                    var session = new Session();
                    session.InitEx2(null, string.Empty, "WX", "WX1000", txtVersion.Text.Trim(), 1);
                    session.Open(txtUser.Text.Trim(), txtPassword.Text.Trim(), txtCompany.Text.Trim(), DateTime.UtcNow, 0);
                }
                catch
                {
                    sessionValid = Resources.InvalidSettingCredentials;
                }
            }

            return sessionValid;
        }

        /// <summary> Valid Entities Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidEntitiesStep()
        {
            // Since the entity validated when added to the tree, we will just ensure that there is 
            // at least one entity added in order to proceed
            if (_entities.Count == 0)
            {
                return Resources.InvalidEntitiesCount;
            }

            // Container Name
            var repositoryType = GetRepositoryType();
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                // Check for no value
                if (string.IsNullOrEmpty(_entitiesContainerName))
                {
                    return Resources.ContainerNameRequired;
                }

                // Iterate existing entities and ensure Entity Name is not the same as Container Name
                var dupeFound = false;
                foreach (var businessView in _entities)
                {
                    if (businessView.Properties[BusinessView.EntityName].Equals(_entitiesContainerName))
                    {
                        dupeFound = true;
                        break;
                    }
                }

                if (dupeFound)
                {
                    return Resources.InvalidSettingContainerName;
                }
            }

            // Entity compositions
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                // Ensure entity compositions, if specified, are in the list of entities
                string entityName;
                foreach (var businessView in _entities)
                {
                    // Proceed if any compositions
                    if (businessView.Compositions.Count > 0)
                    {
                        // Iternate compositions to ensure the entity for the view exists
                        foreach (var composition in businessView.Compositions)
                        {
                            if (composition.Include)
                            {
                                // Attempt to locate entity in list
                                entityName = EntityComposition(composition.ViewId);
                                // Can stop looking if a view is not matched to an entity
                                if (string.IsNullOrEmpty(entityName))
                                {
                                    return Resources.InvalidSettingCompositionNotAnEntity;
                                }
                                else
                                {
                                    // Update entity name into composition
                                    composition.EntityName = entityName;
                                }
                            }
                        }
                    }
                }

                // find the header node
                if (FindHeaderNode(BuildXDocument()) == null)
                {
                    return Resources.HeaderNodeDefinition;
                }
            }

            return string.Empty;
        }

        /// <summary> EntityComposition</summary>
        /// <param name="viewId">The composed view</param>
        /// <returns>entity name cooresponding to view otherwise empty</returns>
        /// <remarks>The composed view must have an entity specified for it</remarks>
        private string EntityComposition(string viewId)
        {
            var entityName = string.Empty;

            // Iterate entities to see if the entity has been specified for the view
            foreach (var businessView in _entities)
            {
                if (businessView.Properties[BusinessView.ViewId].Contains(viewId))
                {
                    entityName = businessView.Properties[BusinessView.EntityName];
                    break;
                }
            }

            return entityName;
        }

        /// <summary> Valid Entity</summary>
        /// <param name="resxName">The name of the Resx to validate</param>
        /// <param name="viewId">The name of the View ID to validate</param>
        /// <param name="entityName">The name of the entity to validate</param>
        /// <param name="modelName">The name of the model to validate</param>
        /// <param name="repositoryType">The name of the repository</param>
        /// <param name="reportKeys">The name of the report to validate if report type</param>
        /// <param name="programId">The name of the program to validate if report type</param>
        /// <param name="entityFields">The fields/properties list to validate</param>
        /// <param name="uniqueDescriptions">Dictionary of unique descriptions</param>
        /// <param name="entityCompositions">The compositions list to validate</param>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidEntity(string resxName, string viewId, string entityName, string modelName,
            RepositoryType repositoryType, string reportKeys, string programId, List<BusinessField> entityFields,
            Dictionary<string, bool> uniqueDescriptions, List<Composition> entityCompositions)
        {
            // View ID
            if (string.IsNullOrEmpty(viewId))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ViewId.Replace(":", ""));
            }

            // Entity Name
            if (string.IsNullOrEmpty(entityName))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.EntityName.Replace(":", ""));
            }

            // Model Name
            if (string.IsNullOrEmpty(modelName))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ModelName.Replace(":", ""));
            }

            // Report fields only
            if (repositoryType.Equals(RepositoryType.Report))
            {
                // Report
                if (string.IsNullOrEmpty(reportKeys))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.ReportKeys.Replace(":", ""));
                }

                // Program ID
                if (string.IsNullOrEmpty(programId))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.ReportProgramId.Replace(":", ""));
                }
            }

            // Resx name
            if (string.IsNullOrEmpty(resxName))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ResxName.Replace(":", ""));
            }

            // Resx name must end with Resx
            if (!resxName.EndsWith(Resources.Resx))
            {
                return Resources.InvalidResxName;
            }

            // At least one field must be specified
            if (entityFields.Count == 0)
            {
                return Resources.InvalidCount;
            }

            // Check for dupes only in add mode since in edit mode the view id cannot be changed
            var dupeFound = false;
            if (_modeType.Equals(ModeType.Add))
            {
                // Iterate existing entities specified thus far
                foreach (var businessView in _entities)
                {
                    if (!businessView.Text.Equals(ProcessGeneration.NewEntityText) && businessView.Properties[BusinessView.EntityName].Equals(entityName))
                    {
                        dupeFound = true;
                        break;
                    }
                }

                // If a dupe is found, this is invalid
                if (dupeFound)
                {
                    return Resources.InvalidEntityDuplicate;
                }
            }

            // Validate content of fields
            var validFieldsMessage = ProcessGeneration.ValidateFields(entityFields, uniqueDescriptions, repositoryType);
            if (!string.IsNullOrEmpty(validFieldsMessage))
            {
                return validFieldsMessage;
            }

            // Ensure model is not named the same as any fields
            var validFields = !entityFields.ToList().Any(t => t.Name.Equals(modelName));
            if (!validFields)
            {
                return Resources.InvalidSettingModel;
            }

            // Ensure 'EntityName' is not used in any fields
            validFields = !entityFields.ToList().Any(t => t.Name.Equals(ProcessGeneration.ConstantEntityName));
            if (!validFields)
            {
                return Resources.InvalidSettingEntityName;
            }

            // Entity Compositions
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                // Proceed if any compositions
                if (entityCompositions.Count > 0)
                {
                    foreach (var composition in entityCompositions)
                    {
                        if (composition.Include)
                        {
                            // Attempt to locate entity in list
                            var name = EntityComposition(composition.ViewId);
                            if (!string.IsNullOrEmpty(name))
                            {
                                // Update entity name into composition
                                composition.EntityName = name;
                            }
                        }
                    }

                }
            }

            return string.Empty;
        }


        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.CodeGeneration;

            btnSave.Text = Resources.Save;
            btnCancel.Text = Resources.Cancel;
            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

            // Code Type Step
            lblRepositoryType.Text = Resources.CodeType;
            tooltip.SetToolTip(lblRepositoryType, Resources.CodeTypeTip);

            lblCodeTypeDescriptionHelp.Text = Resources.CodeTypeDescriptionTip;
            lblUnknownCodeTypeFilesHelp.Text = Resources.UnknownCodeFilesTip;
            lblCodeTypeFilesHelp.Text = Resources.CodeTypeFilesTip;

            lblUser.Text = Resources.User;
            tooltip.SetToolTip(lblUser, Resources.UserTip);

            lblPassword.Text = Resources.Password;
            tooltip.SetToolTip(lblPassword, Resources.PasswordTip);

            lblVersion.Text = Resources.Version;
            tooltip.SetToolTip(lblVersion, Resources.VersionTip);

            lblCompany.Text = Resources.Company;
            tooltip.SetToolTip(lblCompany, Resources.CompanyTip);

            lblModule.Text = Resources.Module;
            tooltip.SetToolTip(lblModule, Resources.ModuleTip);

            // Entities Step
            lblEntities.Text = string.Format(Resources.EntitiesInstructions, ProcessGeneration.ElementEntities);

            lblViewID.Text = Resources.ViewId;
            tooltip.SetToolTip(lblViewID, Resources.ViewIdTip);

            lblReportIniFile.Text = Resources.ReportIniFile;
            tooltip.SetToolTip(lblReportIniFile, Resources.ReportIniFileTip);

            tooltip.SetToolTip(btnIniDialog, Resources.ReportIniDialogTip);

            lblReportKeys.Text = Resources.ReportKeys;
            tooltip.SetToolTip(lblReportKeys, Resources.ReportKeysTip);

            lblReportProgramId.Text = Resources.ReportProgramId;
            tooltip.SetToolTip(lblReportProgramId, Resources.ReportProgramIdTip);

            lblEntityName.Text = Resources.EntityName;
            tooltip.SetToolTip(lblEntityName, Resources.EntityNameTip);

            lblModelName.Text = Resources.ModelName;
            tooltip.SetToolTip(lblModelName, Resources.ModelNameTip);

            lblResxName.Text = Resources.ResxName;
            tooltip.SetToolTip(lblResxName, Resources.ResxNameTip);

            chkGenerateFinder.Text = Resources.GenerateFinder;
            tooltip.SetToolTip(chkGenerateFinder, Resources.GenerateFinderTip);

            chkGenerateDynamicEnablement.Text = Resources.GenerateDynamicEnablement;
            tooltip.SetToolTip(chkGenerateDynamicEnablement, Resources.GenerateDynamicEnablementTip);

            chkGenerateClientFiles.Text = Resources.GenerateClientFiles;
            tooltip.SetToolTip(chkGenerateClientFiles, Resources.GenerateClientFilesTip);

            chkGenerateIfExist.Text = Resources.GenerateIfExist;
            tooltip.SetToolTip(chkGenerateIfExist, Resources.GenerateIfExistTip);

            chkGenerateEnumsInSingleFile.Text = Resources.GenerateEnumsInSingleFile;
            tooltip.SetToolTip(chkGenerateEnumsInSingleFile, Resources.GenerateEnumsInSingleFileTip);

            tooltip.SetToolTip(tbrEntity, Resources.EntityGridTip);
            btnRowAdd.ToolTipText = Resources.AddRow;
            btnDeleteRow.ToolTipText = Resources.DeleteRow;
            btnDeleteRows.ToolTipText = Resources.DeleteRows;

            tabPage1.Text = Resources.Entity;
            tabPage1.ToolTipText = Resources.EntityTip;

            tabPage2.Text = Resources.Options;
            tabPage2.ToolTipText = Resources.OptionsTip;

            tabPage3.Text = Resources.Properties;
            tabPage3.ToolTipText = Resources.PropertiesTip;

            tabPage4.Text = Resources.Composition;
            tabPage4.ToolTipText = Resources.CompositionTip;

            tooltip.SetToolTip(grdEntityCompositions, Resources.EntityCompositionGridTip);
            
            // Generate Step
            lblGenerateHelp.Text = Resources.GenerateTip;

        }
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
                        _companyNamespace = projectName.Substring(0,
                            projectName.IndexOf(module + "." + key, StringComparison.InvariantCulture) - 1);
                    }

                    // Determine which language resources to include
                    if (key.Equals(ProcessGeneration.ResourcesKey))
                    {
                        // Iterate files in resources project
                        foreach (ProjectItem projectItem in project.ProjectItems)
                        {
                            // Add which language files based upon solutions menu files
                            if (projectItem.Name.Equals("MenuResx.resx"))
                            {
                                // Add to project
                                _includeEnglish = true;
                            }
                            else if (projectItem.Name.Equals("MenuResx.zh-Hans.resx"))
                            {
                                // Add to project
                                _includeChineseSimplified = true;
                            }
                            else if (projectItem.Name.Equals("MenuResx.zh-Hant.resx"))
                            {
                                // Add to project
                                _includeChineseTraditional = true;
                            }
                            else if (projectItem.Name.Equals("MenuResx.es.resx"))
                            {
                                // Add to project
                                _includeSpanish = true;
                            }
                            else if (projectItem.Name.Equals("MenuResx.fr.resx"))
                            {
                                // Add to project
                                _includeFrench = true;
                            }
                        }
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
                foreach (
                    var parsedLine in from line in lines where line.Contains("AssemblyCopyright") select line.Split('"')
                )
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

            // Entity Step Events
            _addEntityMenuItem.Click += AddEntityMenuItemOnClick;
            _editEntityMenuItem.Click += EditEntityMenuItemOnClick;
            _deleteEntityMenuItem.Click += DeleteEntityMenuItemOnClick;
            _deleteEntitiesMenuItem.Click += DeleteEntitiesMenuItemOnClick;
            _editContainerName.Click += EditContainerNameMenuItemOnClick;

            // Check box for all compositions
            _allCompositions = new CheckBox();
            _allCompositions.Checked = true;
            _allCompositions.CheckedChanged += AllCompositionsCheckedChanged;

            // Default to Flat Repository
            cboRepositoryType.SelectedIndex = Convert.ToInt32(RepositoryType.Flat);
        }

        /// <summary> New Entity</summary>
        private static BusinessView NewEntity()
        {
            // Build new entity
            return new BusinessView { Text = ProcessGeneration.NewEntityText };
        }

        /// <summary> New entity tree node</summary>
        /// <param name="businessView">Business View</param>
        private static TreeNode NewEntityTreeNode(BusinessView businessView)
        {
            // Build new tree node
            var name = BuildEntityNodeName(businessView);
            var text = name;
            var treeNode = new TreeNode(text)
            {
                Tag = businessView,
                Name = name
            };

            return treeNode;
        }

        /// <summary> Build Entity Node Name</summary>
        /// <param name="businessView">Business View </param>
        /// <returns>Name for Node</returns>
        private static string BuildEntityNodeName(BusinessView businessView)
        {
            return businessView.Text;
        }

        /// <summary> Entity Setup</summary>
        /// <param name="treeNode">Tree node</param>
        /// <param name="modeType">Mode Type (Add)</param>
        private void EntitySetup(TreeNode treeNode, ModeType modeType)
        {
            // If not edit mode
            if (!modeType.Equals(ModeType.Edit))
            {
                // Expand clicked node
                _clickedEntityTreeNode.ExpandAll();

                // Add to tree
                treeEntities.SelectedNode = treeNode;
            }

            // Set color of node
            SetNodeColor(treeNode, true);

            // Disable entities controls and related
            EnableEntitiesControls(false);

            // Set mode type and clear controls
            _modeType = modeType;
            ClearEntityControls();

            // Enable entity controls
            EnableEntityControls(true);

            // Load controls from entity or set defaults
            LoadEntityControls();

            // Set focus to view
            if (txtViewID.Enabled)
            {
                txtViewID.Focus();
            }
        }

        /// <summary> Set node color when tree does not have focuus</summary>
        /// <param name="treeNode">Tree node to act upon </param>
        /// <param name="setSelected">true for highligh color otherwise standard color </param>
        private static void SetNodeColor(TreeNode treeNode, bool setSelected)
        {
            if (setSelected)
            {
                treeNode.BackColor = Color.FromKnownColor(KnownColor.Highlight);
                treeNode.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

            }
            else
            {
                treeNode.BackColor = Color.FromKnownColor(KnownColor.Window);
                treeNode.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }
        }

        /// <summary> Enable or disable entities controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableEntitiesControls(bool enable)
        {
            btnNext.Enabled = enable;
            btnBack.Enabled = enable;
        }

        /// <summary> Clear Entity Controls </summary>
        private void ClearEntityControls()
        {
            txtViewID.Clear();

            txtReportIniFile.Clear();
            cboReportKeys.Text = string.Empty;
            cboReportKeys.Items.Clear();
            txtReportProgramId.Clear();
            txtEntityName.Clear();
            txtModelName.Clear();
            txtResxName.Clear();

            DeleteRows();
            DeleteCompositionRows();
            _reports.Clear();
        }

        /// <summary> Enable or disable entity controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableEntityControls(bool enable)
        {
            btnSave.Visible = enable;
            btnCancel.Visible = enable;
            splitEntities.Panel2.Enabled = enable;

            tabEntity.SelectTab(0);

            var repositoryType = GetRepositoryType();

            var enableReportControls = (repositoryType.Equals(RepositoryType.Report) && enable);
            txtReportIniFile.Enabled = enableReportControls;
            cboReportKeys.Enabled = enableReportControls;
            txtReportProgramId.Enabled = enableReportControls;
            btnIniDialog.Enabled = enableReportControls;

            var enableCompositionControls = (repositoryType.Equals(RepositoryType.HeaderDetail) && enable);
            grdEntityCompositions.Enabled = enableCompositionControls;

            txtViewID.Enabled = _modeType.Equals(ModeType.Add) && enable;

            // If enabled AND Report or Dynamic Query then disable
            if (txtViewID.Enabled)
            {
                if (repositoryType.Equals(RepositoryType.DynamicQuery) || repositoryType.Equals(RepositoryType.Report))
                {
                    txtViewID.Enabled = false;
                }
            }

            chkGenerateFinder.Enabled = (!repositoryType.Equals(RepositoryType.Report) &&
                                !repositoryType.Equals(RepositoryType.Process) &&
                                !repositoryType.Equals(RepositoryType.DynamicQuery) &&
                                // Inquiry type should be able to generate finder but disable for now
                                !repositoryType.Equals(RepositoryType.Inquiry));

            chkGenerateDynamicEnablement.Enabled = (!repositoryType.Equals(RepositoryType.HeaderDetail) && enable);
            chkGenerateClientFiles.Enabled = (!repositoryType.Equals(RepositoryType.HeaderDetail) && enable);

            // If not enabled, then uncheck it
            if (!chkGenerateFinder.Enabled)
            {
                chkGenerateFinder.Checked = false;
            }
            if (!chkGenerateDynamicEnablement.Enabled)
            {
                chkGenerateDynamicEnablement.Checked = false;
            }
            if (!chkGenerateClientFiles.Enabled)
            {
                chkGenerateClientFiles.Checked = false;
            }

        }

        /// <summary> Load Entity Controls from Business View of node clicked</summary>
        private void LoadEntityControls()
        {
            var repositoryType = GetRepositoryType();

            if (_modeType.Equals(ModeType.Add))
            {
                // Set CS0120 for Dynamic Query Repository
                if (repositoryType.Equals(RepositoryType.DynamicQuery))
                {
                    txtViewID.Text = "CS0120";
                }
                // Set new guid for Report Repository
                else if (repositoryType.Equals(RepositoryType.Report))
                {
                    txtViewID.Text = Guid.NewGuid().ToString();
                }

                // Options defaults
                chkGenerateFinder.Checked = !repositoryType.Equals(RepositoryType.Report) &&
                                                !repositoryType.Equals(RepositoryType.Process) &&
                                                !repositoryType.Equals(RepositoryType.DynamicQuery) &&
                                                // Inquiry type should be able to generate finder but disable for now
                                                !repositoryType.Equals(RepositoryType.Inquiry) &&
                                                !repositoryType.Equals(RepositoryType.HeaderDetail);

                // Finder default for Header-Detail should be checked for header entity only otherwise unchecked
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    // Checked if header entity
                    chkGenerateFinder.Checked = _clickedEntityTreeNode.Name.Equals(ProcessGeneration.ElementEntities) && 
                        _clickedEntityTreeNode.Nodes.Count.Equals(1);
                }

                chkGenerateDynamicEnablement.Checked = !repositoryType.Equals(RepositoryType.HeaderDetail);
                chkGenerateClientFiles.Checked = !repositoryType.Equals(RepositoryType.HeaderDetail);
                chkGenerateIfExist.Checked = !repositoryType.Equals(RepositoryType.HeaderDetail);
                chkGenerateEnumsInSingleFile.Checked = false;

                // If not enabled, then uncheck it
                if (!chkGenerateFinder.Enabled)
                {
                    chkGenerateFinder.Checked = false;
                }
                if (!chkGenerateDynamicEnablement.Enabled)
                {
                    chkGenerateDynamicEnablement.Checked = false;
                }
                if (!chkGenerateClientFiles.Enabled)
                {
                    chkGenerateClientFiles.Checked = false;
                }

                return;
            }

            // Get the node clicked
            var treeNode = _clickedEntityTreeNode;
            var businessView = (BusinessView)treeNode.Tag;

            // Assign to controls
            txtViewID.Text = businessView.Properties[BusinessView.ViewId];

            txtReportIniFile.Text = businessView.Properties[BusinessView.ReportIni];
            if (repositoryType.Equals(RepositoryType.Report))
            {
                AddReports(txtReportIniFile.Text);
                cboReportKeys.Text = businessView.Properties[BusinessView.ReportKey];
                txtReportProgramId.Text = businessView.Properties[BusinessView.ProgramId];

                // Delete Rows as the selectedIndex Change method caused to add also. They will be added below
                DeleteRows();
                DeleteCompositionRows();
            }

            txtEntityName.Text = businessView.Properties[BusinessView.EntityName];
            txtModelName.Text = businessView.Properties[BusinessView.ModelName];
            txtResxName.Text = businessView.Properties[BusinessView.ResxName];

            // Options tab
            chkGenerateFinder.Checked = businessView.Options[BusinessView.GenerateFinder];
            chkGenerateDynamicEnablement.Checked = businessView.Options[BusinessView.GenerateDynamicEnablement];
            chkGenerateClientFiles.Checked = businessView.Options[BusinessView.GenerateClientFiles];
            chkGenerateIfExist.Checked = businessView.Options[BusinessView.GenerateIfAlreadyExists];
            chkGenerateEnumsInSingleFile.Checked = businessView.Options[BusinessView.GenerateEnumsInSingleFile];

            // Assign to the grids
            AssignGrids(businessView);
        }

        /// <summary> Assign grids </summary>
        /// <param name="businessView">Business View</param>
        private void AssignGrids(BusinessView businessView)
        {
            var repositoryType = GetRepositoryType();

            // Assign to the properties grid
            foreach (var field in businessView.Fields)
            {
                // Add to collection for data binding to grid
                _entityFields.Add(field);
            }

            // Assign to the compositions grid
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                // Determine how to set header included checkbox
                var allIncluded = true;

                foreach (var composition in businessView.Compositions)
                {
                    // If if has not been set to false, someone is not included,
                    // then keep investigating every row
                    if (allIncluded)
                    {
                        allIncluded = composition.Include;
                    }

                    // Attempt to locate entity in list
                    var name = EntityComposition(composition.ViewId);
                    if (!string.IsNullOrEmpty(name))
                    {
                        // Update entity name into composition
                        composition.EntityName = name;
                    }

                    // Add to collection for data binding to grid
                    _entityCompositions.Add(composition);
                }

                // Set header included checkbox, if there are any compositions
                if (businessView.Compositions.Count > 0)
                {
                    // If all rows are included, all compositions checkbox will be checked
                    if (allIncluded)
                    {
                        // Only check if it is not already checked
                        if (!_allCompositions.Checked)
                        {
                            _skipAllCompositionsClick = true;
                            _allCompositions.Checked = true;
                        }
                    }
                    // Since either all are not included or some are, all compositions checkbox will be unchecked
                    else
                    {
                        // Only uncheck if it is not already unchecked
                        if (_allCompositions.Checked)
                        {
                            _skipAllCompositionsClick = true;
                            _allCompositions.Checked = false;
                        }
                    }
                }


            }
        }

        /// <summary> Delete Entity Node</summary>
        /// <param name="treeNode">Tree Node to delete </param>
        private void DeleteEntityNode(TreeNode treeNode)
        {
            // Remove from tree
            var businessView = (BusinessView)treeNode.Tag;

            // Remove from entities
            _entities.Remove(businessView);

            // Remove the tree node
            treeNode.Remove();
        }

        /// <summary> Delete Entity Nodes</summary>
        /// <param name="treeNodes">Tree Nodes to delete </param>
        private void DeleteEntityNodes(TreeNodeCollection treeNodes)
        {
            for (var i = treeNodes.Count - 1; i > -1; i--)
            {
                DeleteEntityNode(treeNodes[i]);
            }
        }

        /// <summary> Cancel any entity changes</summary>
        private void CancelEntity()
        {
            // For added entity, remove last node as this is where the next node was placed
            if (_modeType == ModeType.Add)
            {
                // Remove from entities list first
                var businessView = (BusinessView)_clickedEntityTreeNode.LastNode.Tag;
                _entities.Remove(businessView);

                // Remove from tree
                _clickedEntityTreeNode.Nodes.Remove(_clickedEntityTreeNode.LastNode);
            }

            // For edit, reset color
            if (_modeType == ModeType.Edit)
            {
                SetNodeColor(_clickedEntityTreeNode, false);
            }

            // Reset mode
            _modeType = ModeType.None;

            // Enable entities controls
            EnableEntitiesControls(true);

            // Clear inidividual entity controls
            ClearEntityControls();

            // Disable entity controls
            EnableEntityControls(false);

            // Set focus back to tree
            treeEntities.Focus();
        }

        /// <summary> Entity has changed, so update</summary>
        private void SaveEntity()
        {
            // Validation
            var repositoryType = GetRepositoryType();
            var uniqueDescriptions = new Dictionary<string, bool>();

            // Ensure upper case for compositions
            foreach (var composition in _entityCompositions)
            {
                composition.ViewId = composition.ViewId.ToUpper();
            }

            var success = ValidEntity(txtResxName.Text, txtViewID.Text, txtEntityName.Text, txtModelName.Text,
                repositoryType, cboReportKeys.Text, txtReportProgramId.Text, _entityFields.ToList(),
                uniqueDescriptions, _entityCompositions.ToList());
            if (!string.IsNullOrEmpty(success))
            {
                DisplayMessage(success, MessageBoxIcon.Error);
                return;
            }

            // Get business view from clicked node
            var node = _modeType == ModeType.Add ? _clickedEntityTreeNode.LastNode : _clickedEntityTreeNode;
            var businessView = (BusinessView)node.Tag;

            // Get valued from controls and add to object
            businessView.Properties[BusinessView.ModuleId] = cboModule.Text;
            businessView.Properties[BusinessView.ViewId] = txtViewID.Text;
            businessView.Properties[BusinessView.ReportIni] = txtReportIniFile.Text;
            businessView.Properties[BusinessView.ReportKey] = cboReportKeys.Text;
            businessView.Properties[BusinessView.ProgramId] = txtReportProgramId.Text;
            businessView.Properties[BusinessView.EntityName] = txtEntityName.Text;
            businessView.Properties[BusinessView.ModelName] = txtModelName.Text;
            businessView.Properties[BusinessView.ResxName] = txtResxName.Text;

            businessView.Properties[BusinessView.WorkflowKindId] = (repositoryType.Equals(RepositoryType.Process)) ? Guid.NewGuid().ToString() : Guid.Empty.ToString();

            businessView.Options[BusinessView.GenerateFinder] = chkGenerateFinder.Checked;
            businessView.Options[BusinessView.GenerateDynamicEnablement] = chkGenerateDynamicEnablement.Checked;
            businessView.Options[BusinessView.GenerateClientFiles] = chkGenerateClientFiles.Checked;
            businessView.Options[BusinessView.GenerateIfAlreadyExists] = chkGenerateIfExist.Checked;
            businessView.Options[BusinessView.GenerateEnumsInSingleFile] = chkGenerateEnumsInSingleFile.Checked;

            businessView.Fields = _entityFields.ToList();
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                businessView.Compositions = _entityCompositions.ToList();
            }

            // Add/Update to tree
            businessView.Text = businessView.Properties[BusinessView.EntityName];

            node.Name = BuildEntityNodeName(businessView);
            node.Text = BuildEntityText(businessView);
            node.Tag = businessView;

            // Reset mode
            _modeType = ModeType.None;

            // Reset selected color
            SetNodeColor(node, false);

            // Enable entities controls
            EnableEntitiesControls(true);

            // Clear individual entity controls
            ClearEntityControls();

            // Disable entity controls
            EnableEntityControls(false);

            // Set focus back to tree
            treeEntities.Focus();
        }

        /// <summary> Build Entity Text from Entity</summary>
        /// <param name="businessView">Business View to build text from</param>
        /// <returns>Text</returns>
        private string BuildEntityText(BusinessView businessView)
        {
            var repositoryType = GetRepositoryType();

            var text = ProcessGeneration.PropertyEntity + "=\"" + businessView.Properties[BusinessView.EntityName] + "\" ";
            text += ProcessGeneration.PropertyModule + "=\"" + businessView.Properties[BusinessView.ModuleId] + "\" ";

            // Show view id if not a report
            if (!repositoryType.Equals(RepositoryType.Report))
            {
                text += ProcessGeneration.PropertyViewId + "=\"" + businessView.Properties[BusinessView.ViewId] + "\" ";
            }

            // Show program id if a report
            if (repositoryType.Equals(RepositoryType.Report))
            {
                text += ProcessGeneration.PropertyProgramId + "=\"" + businessView.Properties[BusinessView.ProgramId] + "\" ";
            }

            // Show workflow id if a process
            if (repositoryType.Equals(RepositoryType.Process))
            {
                text += ProcessGeneration.PropertyWorkflowId + "=\"" + businessView.Properties[BusinessView.WorkflowKindId] + "\" ";
            }

            text += ProcessGeneration.PropertyProperties + "=\"" + businessView.Fields.Count.ToString() + "\" ";

            // Show compositions if a header-detail
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                text += ProcessGeneration.PropertyComps + "=\"" + businessView.Compositions.Where(x => x.Include).Count() + "\" ";
            }

            // Show Finder and Dynamic Enablement if not a report/dynamic query
            if (!repositoryType.Equals(RepositoryType.Report) && !repositoryType.Equals(RepositoryType.DynamicQuery))
            {
                text += ProcessGeneration.PropertyFinder + "=\"" + businessView.Options[BusinessView.GenerateFinder].ToString() + "\" ";
                text += ProcessGeneration.PropertyEnablement + "=\"" + businessView.Options[BusinessView.GenerateDynamicEnablement].ToString() + "\" ";
            }

            text += ProcessGeneration.PropertyClientFiles + "=\"" + businessView.Options[BusinessView.GenerateClientFiles].ToString() + "\" ";
            text += ProcessGeneration.PropertyIfExists + "=\"" + businessView.Options[BusinessView.GenerateIfAlreadyExists].ToString() + "\" ";
            text += ProcessGeneration.PropertySingleFile + "=\"" + businessView.Options[BusinessView.GenerateEnumsInSingleFile].ToString() + "\" ";

            return text;
        }

        /// <summary> Concat entities text from container name </summary>
        /// <returns>Text</returns>
        private string BuildEntitiesText()
        {
            var repositoryType = GetRepositoryType();
            var text = ProcessGeneration.ElementEntities;

            // Only for header-detail at this point
            if (repositoryType.Equals(RepositoryType.HeaderDetail))
            {
                // Show container name else show required text
                text += " (" + (string.IsNullOrEmpty(_entitiesContainerName) ? Resources.ContainerNameRequired : _entitiesContainerName) + ")";
            }

            return text;
        }

        /// <summary> Add Entity</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void AddEntityMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new Business View
            var businessView = NewEntity();

            // Build new tree node
            var treeNode = NewEntityTreeNode(businessView);

            // Add to entities
            _entities.Add(businessView);
            _clickedEntityTreeNode.Nodes.Add(treeNode);

            EntitySetup(treeNode, ModeType.Add);
        }

        /// <summary> Edit Container Name</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void EditContainerNameMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Modal form for input
            EditContainerName();
        }

        /// <summary> Edit Entity</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void EditEntityMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Setup items for edit of entity
            EntitySetup(_clickedEntityTreeNode, ModeType.Edit);
        }

        /// <summary> Delete Entity</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteEntityMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Delete tree node
            DeleteEntityNode(_clickedEntityTreeNode);
        }

        /// <summary> Delete all entities</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteEntitiesMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var treeNodes = _clickedEntityTreeNode.Nodes;

            DeleteEntityNodes(treeNodes);
        }

        #region Toolbar Events

        /// <summary> Next/Generate toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!_processingInProgress)
            {
                NextStep();
            }
        }

        /// <summary> Back toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Back wizard step</remarks>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!_processingInProgress)
            {
                BackStep();
            }
        }

        #endregion

        /// <summary> Edit Container Name</summary>
        private void EditContainerName()
        {
            // Modal form for input
            var dialog = new ContainerName(_entitiesContainerName);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _entitiesContainerName = dialog.ContainerNameProperty;
                var entitiesNode = treeEntities.Nodes[0];
                entitiesNode.Text = BuildEntitiesText();
            }
        }

        /// <summary> Get Repository Type </summary>
        private RepositoryType GetRepositoryType()
        {
            return (RepositoryType)Enum.Parse(typeof(RepositoryType), cboRepositoryType.SelectedIndex.ToString());
        }

        /// <summary>
        /// Find the header node in the tree. 
        /// </summary>
        /// <param name="doc">The tree in XDocument format</param>
        /// <returns>If there is one and only one header node defined, return the node otherwise NULL</returns>
        private static XElement FindHeaderNode(XDocument doc)
        {
            XElement headerNode = null;

            if (doc.Root == null)
            {
                return null;
            }

            foreach (var x in doc.Root.Elements())
            {
                if (!x.Descendants().Any(e => e.Name == ProcessGeneration.PropertyEntity))
                {
                    continue;
                }
                if (headerNode != null)
                {
                    return null;
                }
                headerNode = x;
            }
            return headerNode;
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            var repositoryType = GetRepositoryType();

            // Finished?
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
            {
                _generation.Dispose();
                Close();
            }
            else
            {
                // Proceed to next wizard step or start generation if last step
                if (!_currentWizardStep.Equals(-1) &&
                    _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerateCode))
                {
                    // Build settings
                    var settings = BuildSettings();
                    // Setup display before processing
                    _gridInfo.Clear();
                    _processingInProgress = true;
                    //ProcessingSetup(false);
                    grdResourceInfo.DataSource = _gridInfo;
                    grdResourceInfo.Refresh();

                    _rowIndex = -1;

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(settings);
                }
                else
                {
                    // Proceed to next step

                    if (!_currentWizardStep.Equals(-1))
                    {
                        // Before proceeding to next step, ensure current step is valid
                        if (!ValidateStep())
                        {
                            return;
                        }

                        btnBack.Enabled = true;

                        ShowStep(false);
                    }

                    _currentWizardStep++;

                    // if Step is Screens, expand tree control
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelEntities))
                    {
                        treeEntities.ExpandAll();
                    }

                    // Create XML if Step is Generate
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerateCode))
                    {
                        _xmlEntities = BuildXDocument();
                        txtEntitiesToGenerate.Text = _xmlEntities.ToString();

                        // for header-detail type, mark each entity in the header-detail tree
                        if (repositoryType.Equals(RepositoryType.HeaderDetail))
                        {
                            // find the header node
                            _headerNode = FindHeaderNode(_xmlEntities);

                            
                            var headerDetailEntities = _headerNode.DescendantsAndSelf().Where(e => e.Name == ProcessGeneration.PropertyEntity);

                            // mark entity in _entities 
                            foreach (var entity in _entities)
                            {
                                // ReSharper disable once PossibleMultipleEnumeration
                                entity.IsPartofHeaderDetailComposition = headerDetailEntities.Any(p => p.Attribute(ProcessGeneration.PropertyViewId).Value.Equals(entity.Properties[BusinessView.ViewId]));
                            }
                        }
                    }

                    ShowStep(true);

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerateCode))
                    {
                        btnNext.Text = Resources.Generate;
                    }

                    // Update title and text for step
                    ShowStepTitle();
                }
            }
        }

        /// <summary> Recursive routine to build XML from tree nodes </summary>
        /// <param name="treeNode">Tree node </param>
        /// <param name="element">XElement </param>
        /// <param name="repositoryType">Repository Type </param>
        private void BuildXmlFromTreeNodes(TreeNode treeNode, XElement element, RepositoryType repositoryType)
        {
            // Iterate the tree nodes  
            foreach (TreeNode entityTreeNode in treeNode.Nodes)
            {
                // Create element named Entity 
                var entityElement = new XElement(ProcessGeneration.PropertyEntity);
                // Get business view from tag so can iterate fields and options
                var businessView = (BusinessView)entityTreeNode.Tag;

                // Make certain properties into attributes
                entityElement.Add(new XAttribute(ProcessGeneration.PropertyEntity, businessView.Properties[BusinessView.EntityName]));
                entityElement.Add(new XAttribute(ProcessGeneration.PropertyModule, businessView.Properties[BusinessView.ModuleId]));

                // Show view id if not a report
                if (!repositoryType.Equals(RepositoryType.Report))
                {
                    entityElement.Add(new XAttribute(ProcessGeneration.PropertyViewId, businessView.Properties[BusinessView.ViewId]));
                }

                // Show program id if a report
                if (repositoryType.Equals(RepositoryType.Report))
                {
                    entityElement.Add(new XAttribute(ProcessGeneration.PropertyProgramId, businessView.Properties[BusinessView.ProgramId]));
                }

                // Show workflow id if a process
                if (repositoryType.Equals(RepositoryType.Process))
                {
                    entityElement.Add(new XAttribute(ProcessGeneration.PropertyWorkflowId, businessView.Properties[BusinessView.WorkflowKindId]));
                }

                entityElement.Add(new XAttribute(ProcessGeneration.PropertyResxName, businessView.Properties[BusinessView.ResxName]));

                // Add Options to this element via the business view's Options
                var optionsElement = new XElement(ProcessGeneration.PropertyOptions);
                var optionElement = new XElement(ProcessGeneration.PropertyOption);

                optionElement.Add(new XAttribute(ProcessGeneration.PropertyFinder, businessView.Options[BusinessView.GenerateFinder].ToString()));
                optionElement.Add(new XAttribute(ProcessGeneration.PropertyEnablement, businessView.Options[BusinessView.GenerateDynamicEnablement].ToString()));
                optionElement.Add(new XAttribute(ProcessGeneration.PropertyClientFiles, businessView.Options[BusinessView.GenerateClientFiles].ToString()));
                optionElement.Add(new XAttribute(ProcessGeneration.PropertyIfExists, businessView.Options[BusinessView.GenerateIfAlreadyExists].ToString()));
                optionElement.Add(new XAttribute(ProcessGeneration.PropertySingleFile, businessView.Options[BusinessView.GenerateEnumsInSingleFile].ToString()));

                optionsElement.Add(optionElement);
                entityElement.Add(optionsElement);


                // Add Fields to this element via the business view's Fields
                var fieldsElement = new XElement(ProcessGeneration.PropertyFields);

                // Iterate fields
                foreach (var businessField in businessView.Fields)
                {
                    var fieldElement = new XElement(ProcessGeneration.PropertyField);

                    fieldElement.Add(new XAttribute(ProcessGeneration.PropertyFieldName, businessField.ServerFieldName));
                    fieldElement.Add(new XAttribute(ProcessGeneration.PropertyPropertyName, businessField.Name));
                    fieldElement.Add(new XAttribute(ProcessGeneration.PropertyType, businessField.Type.ToString()));
                    fieldElement.Add(new XAttribute(ProcessGeneration.PropertySize, businessField.Size.ToString()));

                    fieldsElement.Add(fieldElement);
                }
                entityElement.Add(fieldsElement);

                // Show compositions if a header-detail
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    // Add Compositions to this element via the business view's Compositions
                    var compositionsElement = new XElement(ProcessGeneration.PropertyCompositions);

                    // Iterate compositions
                    foreach (var composition in businessView.Compositions)
                    {
                        var compositionElement = new XElement(ProcessGeneration.PropertyComposition);

                        compositionElement.Add(new XAttribute(ProcessGeneration.PropertyViewId, composition.ViewId));
                        compositionElement.Add(new XAttribute(ProcessGeneration.PropertyEntity, composition.EntityName));
                        compositionElement.Add(new XAttribute(ProcessGeneration.PropertyInclude, composition.Include.ToString()));

                        compositionsElement.Add(compositionElement);
                    }
                    entityElement.Add(compositionsElement);
                }

                // If this node has nodes (children), do them recusively
                if (entityTreeNode.Nodes.Count != 0)
                {
                    // Recursion
                    BuildXmlFromTreeNodes(entityTreeNode, entityElement, repositoryType);
                }

                // Done with this element so add it to the parent (entered) element
                element.Add(entityElement);
            }
        }

        /// <summary> Build XML Document from tree control </summary>
        private XDocument BuildXDocument()
        {
            var xDocument = new XDocument();
            var repositoryType = GetRepositoryType();

            // Build XML from tree nodes  
            foreach (TreeNode treeNode in treeEntities.Nodes)
            {
                // The first node is the Entities node
                var element = new XElement(treeNode.Name);
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    element.Add(new XAttribute(ProcessGeneration.PropertyContainer, _entitiesContainerName));
                }

                // Start recursion
                BuildXmlFromTreeNodes(treeNode, element, repositoryType);
                // Add finally to the XDocument
                xDocument.Add(element);
            }

            return xDocument;
        }
        /// <summary> Back Navigation </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            // Proceed to next wizard step or start code generation if last step
            if (!_currentWizardStep.Equals(0))
            {
                // Proceed back a step
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
                {
                    btnNext.Text = Resources.Generate;
                }
                else
                {
                    btnNext.Text = Resources.Next;
                }

                ShowStep(false);

                _currentWizardStep--;

                ShowStep(true);

                // Enable back button?
                if (_currentWizardStep.Equals(0))
                {
                    btnBack.Enabled = false;
                    btnNext.Enabled = true;
                }

                // Update title and text for step
                ShowStepTitle();
            }
        }

        /// <summary> Show Step </summary>
        /// <param name="visible">True to show otherwise false </param>
        private void ShowStep(bool visible)
        {
            // Adjust size
            if (visible)
            {
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelCodeType))
                {
                    // Adjust to smaller size
                    var location = btnBack.Location;
                    location.X = 320;
                    btnBack.Location = location;

                    location = btnNext.Location;
                    location.X = 394;
                    btnNext.Location = location;

                    var size = ClientSize;
                    size.Width = 474;
                    ClientSize = size;
                    // CenterToScreen();
                }
                else
                {
                    // Adjust to larger size
                    var location = btnBack.Location;
                    location.X = 805;
                    btnBack.Location = location;

                    location = btnNext.Location;
                    location.X = 879;
                    btnNext.Location = location;

                    var size = ClientSize;
                    size.Width = 959;
                    ClientSize = size;
                    // CenterToScreen();
                }
            }

            _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
            _wizardSteps[_currentWizardStep].Panel.Visible = visible;
            splitSteps.SplitterDistance = SplitterDistance;

        }

        /// <summary> Show Step Title</summary>
        private void ShowStepTitle()
        {
            var repositoryType = GetRepositoryType().ToString();

            lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash +
                                string.Format(_wizardSteps[_currentWizardStep].Title, repositoryType);
            lblStepDescription.Text = string.Format(_wizardSteps[_currentWizardStep].Description, repositoryType);
        }
        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            var repositoryType = GetRepositoryType();

            // Default
            btnBack.Enabled = false;

            // Do not perform some steps if simply changing the code type and not the initial load
            if (_currentWizardStep == -1)
            {
                // Init wizard steps
                _wizardSteps.Clear();

                // Init Panels
                InitPanel(pnlCodeType);
                InitPanel(pnlEntities);
                InitPanel(pnlGenerateCode);
                InitPanel(pnlGeneratedCode);

                // Add steps
                AddStep(Resources.StepTitleCodeType, Resources.StepDescriptionCodeType, pnlCodeType);
                AddStep(Resources.StepTitleEntities, Resources.StepDescriptionEntities, pnlEntities);

                AddStep(Resources.StepTitleGenerateCode, Resources.StepDescriptionGenerateCode, pnlGenerateCode);
                AddStep(Resources.StepTitleGeneratedCode, Resources.StepDescriptionGeneratedCode, pnlGeneratedCode);
            }


            grpCredentials.Enabled = !(repositoryType.Equals(RepositoryType.DynamicQuery) || repositoryType.Equals(RepositoryType.Report));

            SetupEntitiesTree();

            InitEntityFields(repositoryType);
            InitEntityCompositions(repositoryType);

            // Display first step
            if (_currentWizardStep == -1)
            {
                NextStep();
            }

        }

        /// <summary> Reset wizard steps </summary>
        private void ResetWizardSteps()
        {
            var repositoryType = GetRepositoryType();

            // Default
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            grpCredentials.Enabled = !(repositoryType.Equals(RepositoryType.DynamicQuery) || repositoryType.Equals(RepositoryType.Report));

            SetupEntitiesTree();

            InitEntityFields(repositoryType);

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

        /// <summary> Initialize entity info and modify grid display </summary>
        /// <param name="repositoryType">Repository Type</param>
        private void InitEntityFields(RepositoryType repositoryType)
        {
            // Clear data
            DeleteRows();
            _reports.Clear();

            if (grdEntityFields.DataSource == null)
            {
                // Assign binding to datasource (two binding)
                grdEntityFields.DataSource = _entityFields;
                grdEntityFields.ScrollBars = ScrollBars.Both;

                // Assign widths and localized text
                GenericInit(grdEntityFields, 0, 50, Resources.ID, false, false);
                GenericInit(grdEntityFields, 1, 125, Resources.ServerField, true, true);
                GenericInit(grdEntityFields, 2, 150, Resources.Field, true, false);
                GenericInit(grdEntityFields, 3, 290, Resources.Description, false, false);

                // Remove and re-add as combobox column
                grdEntityFields.Columns.Remove("Type");
                var column = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "Type",
                    HeaderText = Resources.Type,
                    DropDownWidth = 100,
                    Width = 100,
                    FlatStyle = FlatStyle.Flat
                };
                grdEntityFields.Columns.Insert(4, column);
                grdEntityFields.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdEntityFields.Columns[4].Visible = !repositoryType.Equals(RepositoryType.Report);

                // Continue with assign widths and localized text
                GenericInit(grdEntityFields, 5, 40, Resources.Size, true, false);
                GenericInit(grdEntityFields, 6, 75, Resources.IsReadOnly, false, false);
                GenericInit(grdEntityFields, 7, 75, Resources.IsCalculated, false, false);
                GenericInit(grdEntityFields, 8, 75, Resources.IsRequired, false, false);
                GenericInit(grdEntityFields, 9, 75, Resources.IsKey, false, false);
                GenericInit(grdEntityFields, 10, 75, Resources.IsUpperCase, false, false);
                GenericInit(grdEntityFields, 11, 75, Resources.IsAlphaNumeric, false, false);
                GenericInit(grdEntityFields, 12, 75, Resources.IsNumeric, false, false);
                GenericInit(grdEntityFields, 13, 75, Resources.IsDynamicEnablement, false, false);
            }

            // Show/Hide Fieldname based upon code type
            GenericInit(grdEntityFields, 1, 125, Resources.ServerField, !repositoryType.Equals(RepositoryType.DynamicQuery), true);

            // Droplist items wmay be different based upon repository type
            var typeColumn = (DataGridViewComboBoxColumn)grdEntityFields.Columns[4];
            typeColumn.Items.Clear();

            // Add data types
            if (repositoryType.Equals(RepositoryType.Report))
            {
                // Only add string type
                typeColumn.Items.Add(BusinessDataType.String);
            }
            else if (repositoryType.Equals(RepositoryType.DynamicQuery))
            {
                // Add all types but enumeration
                foreach (
                    var businessDataType in
                    Enum.GetValues(typeof(BusinessDataType))
                        .Cast<BusinessDataType>()
                        .Where(businessDataType => !businessDataType.Equals(BusinessDataType.Enumeration)))
                {
                    typeColumn.Items.Add(businessDataType);
                }
            }
            else
            {
                // Add all types
                foreach (
                    var businessDataType in
                    Enum.GetValues(typeof(BusinessDataType))
                        .Cast<BusinessDataType>())
                {
                    typeColumn.Items.Add(businessDataType);
                }
            }

        }

        /// <summary> Initialize entity compositions and modify grid display </summary>
        /// <param name="repositoryType">Repository Type</param>
        private void InitEntityCompositions(RepositoryType repositoryType)
        {
            // Clear data
            DeleteCompositionRows();

            if (grdEntityCompositions.DataSource == null)
            {
                // Assign binding to datasource (two binding)
                grdEntityCompositions.DataSource = _entityCompositions;
                grdEntityCompositions.ScrollBars = ScrollBars.Both;

                // Assign widths and localized text
                GenericInit(grdEntityCompositions, 0, 125, Resources.CompositeView, true, true);
                GenericInit(grdEntityCompositions, 1, 150, Resources.Entity, true, true);
                GenericInit(grdEntityCompositions, 2, 125, Resources.Include, true, false);

                // Place checkbox into header
                //var rect = grdEntityCompositions.GetCellDisplayRectangle(2, -1, true);
                _allCompositions.Size = new Size(18, 18);
                //_allCompositions.Location = rect.Location;
                grdEntityCompositions.Controls.Add(_allCompositions);
            }
        }

        /// <summary> Generic init for grid </summary>
        /// <param name="grid">Grid control</param>
        /// <param name="column">Column Number</param>
        /// <param name="width">Column Width</param>
        /// <param name="text">Header Text</param>
        /// <param name="visible">True for visible otherwise False</param>
        /// <param name="readOnly">True for read only otherwise False</param>
        private static void GenericInit(DataGridView grid, int column, int width, string text, bool visible,
            bool readOnly)
        {
            grid.Columns[column].Width = width;
            grid.Columns[column].HeaderText = text;
            grid.Columns[column].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[column].Visible = visible;
            grid.Columns[column].ReadOnly = readOnly;
            grid.Columns[column].Resizable = DataGridViewTriState.False;

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
            pnlButtons.Enabled = enableToolbar;
            pnlButtons.Refresh();
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.GeneratingFile, text);
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
            var repositoryType = GetRepositoryType();

            return new Settings
            {
                RepositoryType = repositoryType,
                User = txtUser.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Version = txtVersion.Text.Trim(),
                Company = txtCompany.Text.Trim(),
                ModuleId = cboModule.Text.Trim(),

                Entities = _entities,
                XmlEntities = _xmlEntities,

                PromptIfExists = false,
                Projects = _projects,
                Copyright = _copyright,
                CompanyNamespace = _companyNamespace,
                Extension = GetExtension(repositoryType),
                ResourceExtension = GetResourceExtension(repositoryType),
                DoesAreasExist = _doesAreasExist,
                WebProjectIncludesModule = _webProjectIncludesModule,
                includeEnglish = _includeEnglish,
                includeChineseSimplified = _includeChineseSimplified,
                includeChineseTraditional = _includeChineseTraditional,
                includeSpanish = _includeSpanish,
                includeFrench = _includeFrench,
                EntitiesContainerName = _entitiesContainerName,
                HeaderNode = _headerNode
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

        /// <summary> Build Module Lists </summary>
        private void BuildModules()
        {
            // Clear first
            cboModule.Items.Clear();

            // Add empty item at top of list
            cboModule.Items.Add(string.Empty);

            // Iterate "Models" project(s) for modules belonging to the solution
            foreach (var projectInfo in _projects[ProcessGeneration.ModelsKey])
            {
                cboModule.Items.Add(projectInfo.Key);

                if (!_projects[ProcessGeneration.ModelsKey].Count.Equals(1))
                {
                    continue;
                }

                // Default if only 1 module is discovered
                cboModule.SelectedIndex = 1;
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
            _processingInProgress = false;
            // ProcessingSetup(true);
            Processing("");
            _generation.Dispose();

            ShowStep(false);

            _currentWizardStep++;

            ShowStep(true);

            // Update title and text for step
            ShowStepTitle();

            // Display final step
            btnNext.Text = Resources.Finish;
        }

        /// <summary> Add a row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            var repositoryType = GetRepositoryType();

            if (!repositoryType.Equals(RepositoryType.Report))
            {
                _entityFields.Add(new BusinessField());
            }
        }


        /// <summary> Delete the current row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            var repositoryType = GetRepositoryType();

            if (!repositoryType.Equals(RepositoryType.Report))
            {
                if (grdEntityFields.CurrentRow != null)
                {
                    grdEntityFields.Rows.Remove(grdEntityFields.CurrentRow);
                }
            }
        }

        /// <summary> Delete all rows toolbar button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRows_Click(object sender, EventArgs e)
        {
            var repositoryType = GetRepositoryType();

            if (!repositoryType.Equals(RepositoryType.Report))
            {
                DeleteRows();
            }
        }

        /// <summary> Delete all rows</summary>
        private void DeleteRows()
        {
            // Iterate grid
            for (int i = grdEntityFields.Rows.Count - 1; i >= 0; i--)
            {
                grdEntityFields.Rows.Remove(grdEntityFields.Rows[i]);
            }
        }

        /// <summary> Delete all composition rows</summary>
        private void DeleteCompositionRows()
        {
            // Iterate grid
            for (int i = grdEntityCompositions.Rows.Count - 1; i >= 0; i--)
            {
                grdEntityCompositions.Rows.Remove(grdEntityCompositions.Rows[i]);
            }
        }

        /// <summary> Enable/Disable tab pages based upon selection</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboRepositoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitWizardSteps();
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
            DeleteRows();
            DeleteCompositionRows();
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
            var reportName = ((ComboBox)sender).Text;

            if (!string.IsNullOrEmpty(reportName))
            {
                DeleteRows();
                DeleteCompositionRows();

                foreach (var businessField in _reports[reportName])
                {
                    _entityFields.Add(businessField);
                }
                txtReportProgramId.Text = reportName.ToUpper();
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

        /// <summary> Validate the view id and convert it to a business view</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disable next button</remarks>
        private void txtViewID_Leave(object sender, EventArgs e)
        {
            var errorCondition = false;

            try
            {
                if (!string.IsNullOrEmpty(txtViewID.Text))
                {
                    // Get business view from clicked node
                    var node = _modeType == ModeType.Add ? _clickedEntityTreeNode.LastNode : _clickedEntityTreeNode;
                    var businessView = (BusinessView)node.Tag;

                    ProcessGeneration.GetBusinessView(businessView, txtUser.Text.Trim(), txtPassword.Text.Trim(),
                        txtCompany.Text.Trim(), txtVersion.Text.Trim(), txtViewID.Text, cboModule.Text);

                    // Assign to entity and model fields
                    txtEntityName.Text = businessView.Properties[BusinessView.EntityName];
                    txtModelName.Text = businessView.Properties[BusinessView.ModelName];

                    // Assign to control
                    txtResxName.Text = txtEntityName.Text.Trim() + "Resx";

                    // Clear before assigning
                    DeleteRows();
                    DeleteCompositionRows();

                    // Assign to the grids
                    AssignGrids(businessView);
                }
            }
            catch (Exception ex)
            {
                // Error received attempting to get view
                DisplayMessage((ex.InnerException == null) ? ex.Message : ex.InnerException.Message, MessageBoxIcon.Error);
                txtEntityName.Text = string.Empty;
                txtModelName.Text = string.Empty;
                errorCondition = true;
            }

            // Send back to control?
            if (!errorCondition)
            {
                return;
            }

            // Clear field and send back to control
            txtViewID.Text = string.Empty;
            txtViewID.Focus();
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

        /// <summary> Show edit menu for entity node double clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void treeEntities_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Treeview control was not left double clicked
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            // Do nothing (no context menus) if mode is not none (i.e. it is in an edit or add state)
            if (!_modeType.Equals(ModeType.None))
            {
                return;
            }

            var repositoryType = GetRepositoryType();

            // Double click will enter edit mode
            if (e.Node.Name.Equals(ProcessGeneration.ElementEntities))
            {
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    // Show modal dialog
                    EditContainerName();
                }
                else
                {
                    // No edit mode on this node
                    return;
                }
            }
            else
            {
                // Edit entity
                _contextMenu.MenuItems.Clear();

                // Save node clicked
                _clickedEntityTreeNode = e.Node;

                // Setup items for edit of entity
                EntitySetup(_clickedEntityTreeNode, ModeType.Edit);
            }

        }

        /// <summary> Show menu for entity node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void treeEntities_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Treeview control was not right clicked
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            // Do nothing (no context menus) if mode is not none (i.e. it is in an edit or add state)
            if (!_modeType.Equals(ModeType.None))
            {
                return;
            }

            var repositoryType = GetRepositoryType();

            // Show Add and Delete All menu if Entities was clicked and if header-detail (for now), 
            // also the Edit Container Name
            if (e.Node.Name.Equals(ProcessGeneration.ElementEntities))
            {
                // Context menu to contain Edit Container Name (if header-detail), Add, Delete All
                _contextMenu.MenuItems.Clear();
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    _contextMenu.MenuItems.Add(_editContainerName);
                }
                _contextMenu.MenuItems.Add(_addEntityMenuItem);
                _contextMenu.MenuItems.Add(_deleteEntitiesMenuItem);
            }
            else
            {
                // Show Edit, Delete menu for entity and Add only if code type is header detail
                _contextMenu.MenuItems.Clear();
                if (repositoryType.Equals(RepositoryType.HeaderDetail))
                {
                    _contextMenu.MenuItems.Add(_addEntityMenuItem);
                }
                _contextMenu.MenuItems.Add(_editEntityMenuItem);
                _contextMenu.MenuItems.Add(_deleteEntityMenuItem);
            }

            // Save node clicked
            _clickedEntityTreeNode = e.Node;

            // Show menu
            _contextMenu.Show(treeEntities, e.Location);
        }

        /// <summary> Cancel Entity changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelEntity();
        }

        /// <summary> Save Entity changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveEntity();
        }

        /// <summary> Replace any invalid chars</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtReportProgramId_Leave(object sender, EventArgs e)
        {
            txtReportProgramId.Text = BusinessViewHelper.Replace(txtReportProgramId.Text);
        }

        /// <summary> Replace any invalid chars</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtEntityName_Leave(object sender, EventArgs e)
        {
            txtEntityName.Text = BusinessViewHelper.Replace(txtEntityName.Text);
            txtResxName.Text = txtEntityName.Text + "Resx";
        }

        /// <summary> Replace any invalid chars</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtModelName_Leave(object sender, EventArgs e)
        {
            txtModelName.Text = BusinessViewHelper.Replace(txtModelName.Text);
        }

        /// <summary> Force uppercase for compositions grid</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdEntityCompositions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                // Only applies to the View id column
                if (e.ColumnIndex.Equals(0))
                {
                    e.Value = e.Value.ToString().ToUpper();
                    e.FormattingApplied = true;
                }
            }

        }

        /// <summary> All compositions checkbox changed</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void AllCompositionsCheckedChanged(object sender, EventArgs e)
        {
            // Only act on event if user click not programatic click
            if (!_skipAllCompositionsClick)
            {
                // Update all rows to match header
                for (int row = 0; row < grdEntityCompositions.RowCount; row++)
                {
                    grdEntityCompositions[2, row].Value = _allCompositions.Checked;
                }
                grdEntityCompositions.EndEdit();
            }
            _skipAllCompositionsClick = false;
        }

        /// <summary> Re-position All compositions checkbox </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void tabEntity_Click(object sender, EventArgs e)
        {
            // If compositions tab is selected
            if (((TabControl)sender).SelectedTab.TabIndex.Equals(3))
            {
                // Re-position
                var rect = grdEntityCompositions.GetCellDisplayRectangle(2, -1, true);
                _allCompositions.Location = new Point(rect.Location.X + 18, rect.Location.Y + 2);
            }

        }

        #endregion

    }
}
