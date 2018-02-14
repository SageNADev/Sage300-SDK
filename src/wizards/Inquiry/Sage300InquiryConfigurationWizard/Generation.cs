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
using System.Linq;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard.Properties;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary> UI for Inquiry Configuration Wizard </summary>
    public partial class Generation : Form
    {
        #region Private Vars

        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> Information processed </summary>
        private readonly BindingList<Info> _gridInfo = new BindingList<Info>();

        /// <summary> Properties in grid </summary>
        private readonly BindingList<Property> _properties = new BindingList<Property>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        /// <summary> Models in Assembly </summary>
        private readonly SortedDictionary<string, Model> _models = new SortedDictionary<string, Model>();

        #endregion

        #region Private Constants

        /// <summary> Panel Name for pnlCreateEdit </summary>
        private const string PanelCreateEdit = "pnlCreateEdit";

        /// <summary> Panel Name for pnlGenerated </summary>
        private const string PanelGenerated = "pnlGenerated";

        /// <summary> Panel Name for pnlGenerate </summary>
        private const string PanelGenerate = "pnlGenerate";

        /// <summary> Splitter Distance </summary>
        private const int SplitterDistance = 415;

        #endregion

        #region Private Enums

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

        #region Public Routines

        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            Localize();
            InitWizardSteps();
            InitInfo();
            InitAssemblySource();
            InitGridProperties();
            InitEvents();
            ProcessingSetup(true);
            Processing("");
        }

        #endregion

        #region Private Routines

        /// <summary> Assign grid </summary>
        /// <param name="model">Model selected</param>
        private void AssignGrid(Model model)
        {
            // Assign to the properties grid
            foreach (var property in model.Properties.Values)
            {
                // Add to collection for data binding to grid
                _properties.Add(property);
            }

        }

        /// <summary> Delete all rows</summary>
        private void DeleteRows()
        {
            // Iterate grid
            for (var i = grdProperties.Rows.Count - 1; i >= 0; i--)
            {
                grdProperties.Rows.Remove(grdProperties.Rows[i]);
            }
        }

        /// <summary> Initialize property info and grid display </summary>
        private void InitGridProperties()
        {
            // Clear data
            DeleteRows();

            if (grdProperties.DataSource != null)
            {
                return;
            }

            // Assign binding to datasource (two binding)
            grdProperties.DataSource = _properties;
            grdProperties.ScrollBars = ScrollBars.Both;

            // Assign widths and localized text
            GenericInit(grdProperties, 0, 50, Resources.Index, true, true);
            GenericInit(grdProperties, 1, 125, Resources.Property, true, true);
            GenericInit(grdProperties, 2, 100, Resources.Field, true, true);
            GenericInit(grdProperties, 3, 75, Resources.Type, true, true);
            GenericInit(grdProperties, 4, 100, Resources.FullTypeName, false, true);
            GenericInit(grdProperties, 5, 75, Resources.Include, true, false);
            GenericInit(grdProperties, 6, 75, Resources.Filterable, true, false);
            GenericInit(grdProperties, 7, 75, Resources.Drilldown, true, false);
            GenericInit(grdProperties, 8, 50, Resources.Area, true, false);
            GenericInit(grdProperties, 9, 125, Resources.Controller, true, false);
            GenericInit(grdProperties, 10, 125, Resources.Action, true, false);
            GenericInit(grdProperties, 11, 100, Resources.Enums, false, true);
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

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            pnlButtons.Enabled = enableToolbar;
            pnlButtons.Refresh();
        }

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            // Processing Events
            _generation = new ProcessGeneration();
            _generation.ProcessingEvent += ProcessingEvent;
            _generation.StatusEvent += StatusEvent;
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.GeneratingFile, text);

            pnlButtons.Refresh();
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
            grdInfo.DataSource = _gridInfo;

            // Incrememnt row
            _rowIndex++;

            // Set status and text into tool tip for cell
            grdInfo.CurrentCell = grdInfo[Info.StatusColumnNo, _rowIndex];
            grdInfo.CurrentCell.ToolTipText = text;

            grdInfo.Refresh();
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

        /// <summary> Initialize panel </summary>
        /// <param name="panel">Panel to initialize</param>
        private static void InitPanel(Panel panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.InquiryConfiguration;

            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

            // Step Create/Edit
            lblInquiryId.Text = Resources.Inquiry;
            tooltip.SetToolTip(lblInquiryId, Resources.InquiryIdTip);

            lblFolder.Text = Resources.Folder;
            tooltip.SetToolTip(lblFolder, Resources.FolderNameTip);

            lblInquiryDescription.Text = Resources.InquiryDescription;
            tooltip.SetToolTip(lblInquiryDescription, Resources.InquiryDescriptionTip);

            lblAssembly.Text = Resources.Assembly;
            tooltip.SetToolTip(lblAssembly, Resources.AssemblyTip);

            lblModel.Text = Resources.Model;
            tooltip.SetToolTip(lblModel, Resources.ModelTip);

            tooltip.SetToolTip(btnInquiryFinder, Resources.InquiryFinderTip);
            tooltip.SetToolTip(btnNew, Resources.InquiryNewTip);
            tooltip.SetToolTip(btnFolder, Resources.FolderFinderTip);

        }

        /// <summary> Initialize info and modify grid display </summary>
        private void InitInfo()
        {
            // Assign binding to datasource (two binding)
            grdInfo.DataSource = _gridInfo;

            // Assign widths and localized text
            GenericInit(grdInfo, Info.FileNameColumnNo, 700, Resources.FileName, true, true);
            GenericInit(grdInfo, Info.StatusColumnNo, 50, Resources.Status, true, true);
        }

        /// <summary> Initialize assembly source </summary>
        private void InitAssemblySource()
        {
            // Get *.models.dll assemblies from ...\web\bin folder
            var files = Directory.GetFiles(RegistryHelper.Sage300CWebFolder,
                ProcessGeneration.PropertyModelsSearchPattern);

            // Add a blank row to start with
            cboAssembly.Items.Add(string.Empty);
            cboModel.Items.Add(string.Empty);

            // Add files to dropdown list
            foreach (var file in files)
            {
                // Only add if the assembly is not one of these
                if (!file.EndsWith(ProcessGeneration.PropertyExcludeCommon) &&
                    !file.EndsWith(ProcessGeneration.PropertyExcludeKpi) &&
                    !file.EndsWith(ProcessGeneration.PropertyExcludeWorkflow) &&
                    !file.EndsWith(ProcessGeneration.PropertyExcludeVpf))
                {
                    cboAssembly.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            cboAssembly.SelectedIndex = 0;
            cboModel.SelectedIndex = 0;
        }

        /// <summary> Build display name from components </summary>
        /// <param name="modelName">Model Name</param>
        /// <param name="entityName">Entity or View Name</param>
        /// <returns>Display Name</returns>
        private static string BuildDisplayName(string modelName, string entityName)
        {
            return modelName + "  (" + entityName + ")";
        }

        /// <summary> Initialize model source </summary>
        /// <param name="assemblyName">Assembly Name from dropdown</param>
        private void InitModelSource(string assemblyName)
        {
            // Add a blank row to start with
            cboModel.Items.Clear();
            cboModel.Items.Add(string.Empty);

            // Init
            _models.Clear();

            // Load assembly 
            var assembly = Assembly.LoadFile(Path.Combine(RegistryHelper.Sage300CWebFolder,
                string.Concat(assemblyName, ProcessGeneration.PropertyAssemblyExtension)));

            // Gather types from assembly
            Type[] types;

            try
            {
                // No external references
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Some external references so gather here
                types = (Type[]) ex.Types.Where(t => t != null);
            }

            // Iterate types to gather and build structure needed for grid
            foreach (var type in types)
            {
                // If Interface then skip (someone created an interface in Models library!)
                if (type.IsInterface)
                {
                    continue;
                }

                // If no type or model does not inherit from ModelBase (Reports, Enums), do not add to dropdown
                if (type.BaseType != null && !type.BaseType.Name.Equals(ProcessGeneration.PropertyModelBase))
                {
                    continue;
                }

                // While Process models inherit from ModelBase, do not add to dropdown
                if (type.FullName != null && type.FullName.Contains(ProcessGeneration.PropertyModelsProcess))
                {
                    continue;
                }

                // Locals
                var declaredProperties = type.GetProperties();
                var indexNestedType = type.GetNestedType(ProcessGeneration.PropertyIndex);
                var fieldsNestedType = type.GetNestedType(ProcessGeneration.PropertyFields);

                // If it is base upon ModelBase, but no Index class, it is a nested class and is skipped
                if (indexNestedType == null)
                {
                    continue;
                }

                // Start building the model
                var model = new Model()
                {
                    Name = type.Name,
                    FullName = type.FullName,
                    ManifestModuleName = type.Assembly.ManifestModule.Name
                };

                // Get the EntityName constant's value
                foreach (var field in type.GetFields())
                {
                    if (!field.Name.Equals(ProcessGeneration.PropertyEntityName) &&
                        !field.Name.Equals(ProcessGeneration.PropertyViewName))
                    {
                        continue;
                    }

                    model.EntityName = (string) field.GetRawConstantValue();

                    // Build a display name. This will be used for the key to the sorted list
                    model.DisplayName = BuildDisplayName(model.Name, model.EntityName);
                }

                // If there is no EntityName at this point, it is likely because a process
                // model is defined in the root folder!
                if (string.IsNullOrEmpty(model.EntityName))
                {
                    continue;
                }

                // Iterate properties of model and get only the properties that have an Index reference
                foreach (var declaredProperty in declaredProperties)
                {
                    // Skip if a property is in the base class
                    if (declaredProperty.DeclaringType != null && !declaredProperty.DeclaringType.Name.Equals(type.Name))
                    {
                        continue;
                    }

                    // Skip if property does not have an Index reference
                    if (indexNestedType.GetField(declaredProperty.Name) == null)
                    {
                        continue;
                    }

                    // Skip if property does not have an fields reference
                    if (fieldsNestedType.GetField(declaredProperty.Name) == null)
                    {
                        continue;
                    }

                    // Get underlying type for nullable type
                    var underlyingType = Nullable.GetUnderlyingType(declaredProperty.PropertyType) ?? declaredProperty.PropertyType;

                    // Gather the properties for the property
                    var property = new Property()
                    {
                        Name = declaredProperty.Name,
                        PropertyTypeName = underlyingType.Name,
                        PropertyTypeFullName = declaredProperty.PropertyType.FullName,
                        Index = (int) indexNestedType.GetField(declaredProperty.Name).GetRawConstantValue(),
                        FieldName = (string) fieldsNestedType.GetField(declaredProperty.Name).GetRawConstantValue()
                    };

                    // If an enum then gather enums and store
                    if (declaredProperty.PropertyType.IsEnum)
                    {
                        // Get type from assembly and the fields of the enum
                        UpdateEnumsFromAssembly(assembly, declaredProperty, property);
                    }

                    // Add the properties to the model
                    model.Properties.Add(property.Name, property);
                }

                // Add the model to the list of models
                _models.Add(model.DisplayName, model);
            }

            // Iterate the sorted list and ad to the dropdown 
            foreach (var model in _models.Values)
            {
                cboModel.Items.Add(model.DisplayName);
            }

            cboModel.SelectedIndex = 0;
        }

        /// <summary> Get Enum Type from assembly </summary>
        /// <param name="assembly">Currently opened assembly</param>
        /// <param name="propertyInfo">Property Info of type to get</param>
        /// <param name="property">Property to update</param>
        /// <remarks>If not in current assembly, then attempt to get from source assembly</remarks>
        private static void UpdateEnumsFromAssembly(Assembly assembly, PropertyInfo propertyInfo, Property property)
        {
            // Local

            try
            {
                // Null check
                if (propertyInfo == null || propertyInfo.PropertyType.FullName == null)
                {
                    return;
                }

                // Get type from current assembly first
                var type = assembly.GetType(propertyInfo.PropertyType.FullName);

                // If null then attempt getting from different assembly
                if (type == null)
                {
                    var diffAssembly = Assembly.LoadFile(Path.Combine(RegistryHelper.Sage300CWebFolder,
                        string.Concat(propertyInfo.PropertyType.Assembly.GetName().Name,
                            ProcessGeneration.PropertyAssemblyExtension)));
                    type = diffAssembly.GetType(propertyInfo.PropertyType.FullName);
                }

                // If null then enum has not been located
                if (type == null)
                {
                    return;
                }

                // Get fields of enum
                var typeFields = type.GetFields();

                // Iterate enum
                foreach (var typeField in typeFields)
                {
                    // Skip known index
                    if (typeField.Name.Equals("value__"))
                    {
                        continue;
                    }

                    // Add to property
                    property.Enums.Add(typeField.Name, typeField.GetRawConstantValue().ToString());
                }
            }
            catch
            {
                // ignored
            }

        }

        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            // Default
            btnNext.Text = Resources.Next;
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlCreateEdit);
            InitPanel(pnlGenerate);
            InitPanel(pnlGenerated);

            // Assign steps for wizard
            AddStep(Resources.StepTitleCreateEdit, Resources.StepDescriptionCreateEdit, pnlCreateEdit);
            AddStep(Resources.StepTitleGenerate, Resources.StepDescriptionGenerate, pnlGenerate);
            AddStep(Resources.StepTitleGenerated, Resources.StepDescriptionGenerated, pnlGenerated);

            // Display first step
            NextStep();
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            // Finished?
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
            {
                Close();
            }
            else
            {
                // Proceed to next wizard step or start generation if last step
                if (!_currentWizardStep.Equals(-1) &&
                    _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                {
                    // Setup display before processing
                    _gridInfo.Clear();
                    ProcessingSetup(false);
                    grdInfo.DataSource = _gridInfo;
                    grdInfo.Refresh();

                    _rowIndex = -1;

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(_settings);
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

                    // Update Generate Controls if Step is Generate
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                    {
                        // Load controls based upon the previous steps
                        var model = _models[cboModel.SelectedItem.ToString()];
                        ClearGenerateControls(model.Name);

                        // Generate JSON
                        model.Id = txtInquiryId.Text;
                        model.Description = txtInquiryDescription.Text;

                        var json = GenerateJson(model);
                        txtJsonToGenerate.Text = json.ToString();

                        // Establish settings for processing (Validation already ocurred in each step)
                        _settings = new Settings
                        {
                            FolderName = txtFolderName.Text.Trim(),
                            Json = json,
                            FileName = BuildFileName(model.Name)
                        };
                    }

                    ShowStep(true);

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                    {
                        btnNext.Text = Resources.Generate;
                    }

                    // Update title and text for step
                    ShowStepTitle();
                }
            }
        }

        /// <summary> Back Navigation </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            // Proceed if not on first step
            if (!_currentWizardStep.Equals(0))
            {
                ShowStep(false);

                // Proceed back a step or home if done
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
                {
                    btnBack.Text = Resources.Back;
                    btnNext.Text = Resources.Next;
                    _currentWizardStep = 0;
                }
                else
                {
                    btnNext.Text = Resources.Next;
                    _currentWizardStep--;
                }

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

        /// <summary> Validate Step before proceeding to next step </summary>
        /// <returns>True for valid step other wise false</returns>
        private bool ValidateStep()
        {
            // Locals
            var valid = string.Empty;

            // Create/Edit Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelCreateEdit))
            {
                valid = ValidCreateEditStep();
            }

            if (!string.IsNullOrEmpty(valid))
            { 
                // Something is invalid
                DisplayMessage(valid, MessageBoxIcon.Error);
            }

            return string.IsNullOrEmpty(valid);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Next/Generate button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        /// <summary> Back button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Back wizard step</remarks>
        private void btnBack_Click(object sender, EventArgs e)
        {
            BackStep();
        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            _generation.Process((Settings) e.Argument);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingSetup(true);
            Processing("");

            ShowStep(false);

            _currentWizardStep++;

            ShowStep(true);

            // Update title and text for step
            ShowStepTitle();

            // Display final step
            btnBack.Text = Resources.Home;
            btnNext.Text = Resources.Finish;
        }

        /// <summary> Show Step </summary>
        /// <param name="visible">True to show otherwise false </param>
        private void ShowStep(bool visible)
        {
            _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
            _wizardSteps[_currentWizardStep].Panel.Visible = visible;
            splitSteps.SplitterDistance = SplitterDistance;
        }

        /// <summary> Show Step Title</summary>
        private void ShowStepTitle()
        {
            lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash +
                                _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
        }

        /// <summary> json search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnInquiryFinder_Click(object sender, EventArgs e)
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

            // Display inquiry selected
            ExistingJson(dialog.FileName.Trim());
        }

        /// <summary> New customization</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewJson();
        }

        /// <summary> Folder search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnFolder_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Display manifest selected
            txtFolderName.Text = dialog.SelectedPath;
        }

        /// <summary> Existing JSON </summary>
        /// <param name="fileName">File name </param>
        private void ExistingJson(string fileName)
        {
            // Get the Json 
            var json = JObject.Parse(File.ReadAllText(fileName));

            // Properties
            txtInquiryId.Text = (string)json.SelectToken(ProcessGeneration.PropertyInquiryId);
            txtInquiryDescription.Text = (string)json.SelectToken(ProcessGeneration.PropertyDescription);
            var viewName = (string)json.SelectToken(ProcessGeneration.PropertyViewName);
            var modelName = (string)json.SelectToken(ProcessGeneration.PropertyModelName);
            var assembly = (string)json.SelectToken(ProcessGeneration.PropertyAssembly);

            // Get location from folder where found
            var path = Path.GetDirectoryName(fileName);
            txtFolderName.Text = path;

            // Read JSON to get the Fields
            var fields =
                from field in json[ProcessGeneration.PropertyFields]
                select new
                {
                    FieldName = (string)field[ProcessGeneration.PropertyField],
                    FieldIndex = (string)field[ProcessGeneration.PropertyFieldIndex],
                    DataType = (string)field[ProcessGeneration.PropertyDataType],
                    Name = (string)field[ProcessGeneration.PropertyName],
                    IsFilterable = (bool)field[ProcessGeneration.PropertyIsFilterable],
                    IsDrilldown = (bool)field[ProcessGeneration.PropertyIsDrilldown],
                    DrillDownUrl = (JObject)field[ProcessGeneration.PropertyDrilldownUrl]
                };

            InitModelSource(assembly);

            // Assign to assembly dropdown
            cboAssembly.SelectedItem= assembly;

            var displayName = BuildDisplayName(modelName, viewName);
            var model = _models[displayName];

            // Iterate and add to binding
            foreach (var field in fields)
            {
                var property = model.Properties[field.Name];
                property.IsFilterable = field.IsFilterable;
                property.IsDrilldown = field.IsDrilldown;
                property.IsIncluded = true;

                if (field.DrillDownUrl != null)
                {
                    var tokenPath = field.DrillDownUrl.Path;
                    property.Area = (string)json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyArea);
                    property.ControllerName = (string)json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyController);
                    property.ActionName = (string)json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyAction);
                }
            }

            // Load property grid
            cboModel.SelectedItem = model.DisplayName;

        }

        /// <summary> New JSON</summary>
        private void NewJson()
        {
            txtInquiryId.Text = Guid.NewGuid().ToString();
            txtFolderName.Text = string.Empty;
            txtInquiryDescription.Text = string.Empty;
            cboAssembly.SelectedIndex = 0;
        }

        /// <summary> Generate JSON</summary>
        /// <param name="model">Model used to generate the json file</param>
        private static JObject GenerateJson(Model model)
        {
            // Add properties
            var json = new JObject
            {
                new JProperty(ProcessGeneration.PropertyGeneratedMessage, Resources.GeneratedMessage),
                new JProperty(ProcessGeneration.PropertyGeneratedWarning, Resources.GeneratedWarning),
                new JProperty(ProcessGeneration.PropertyInquiryId, model.Id),
                new JProperty(ProcessGeneration.PropertyDescription, model.Description),
                new JProperty(ProcessGeneration.PropertyViewName, model.EntityName),
                new JProperty(ProcessGeneration.PropertyModelName, model.Name),
                new JProperty(ProcessGeneration.PropertyAssembly, model.ManifestModuleName.Replace(ProcessGeneration.PropertyAssemblyExtension, string.Empty))
            };

            // Create array of fields
            var fieldsArray = new JArray();
            
            foreach (var property in model.Properties.Values)
            {
                // Skip property if not included
                if (!property.IsIncluded)
                {
                    continue;
                }
   
                var fields = new JObject
                {
                    new JProperty(ProcessGeneration.PropertyName, property.Name),
                    new JProperty(ProcessGeneration.PropertyField, property.FieldName),
                    new JProperty(ProcessGeneration.PropertyFieldIndex, property.Index),
                    new JProperty(ProcessGeneration.PropertyDataType, property.PropertyTypeName)
                };

                // If data type is an enum, then add enum values
                if (property.Enums.Count > 0)
                {
                    // Create array of enums
                    var enumsArray = new JArray();
                    var count = 0;

                    // Iternate enums
                    foreach (var propertyEnum in property.Enums)
                    {
                        // Increment count for selected check
                        count++;

                        var enumObj = new JObject
                        {
                            new JProperty(ProcessGeneration.PropertySelected, count == 1),
                            new JProperty(ProcessGeneration.PropertyText, propertyEnum.Key),
                            new JProperty(ProcessGeneration.PropertyValue, propertyEnum.Value)
                        };

                        enumsArray.Add(enumObj);
                    }

                    fields.Add(new JProperty(ProcessGeneration.PropertyEnums, enumsArray));
                }

                fields.Add(new JProperty(ProcessGeneration.PropertyIsFilterable, property.IsFilterable));
                fields.Add(new JProperty(ProcessGeneration.PropertyIsDrilldown, property.IsDrilldown));


                // If drilldown, then add drill down url
                if (property.IsDrilldown)
                {
                    var drilldownProperties = new JObject
                    {
                        new JProperty(ProcessGeneration.PropertyArea, property.Area),
                        new JProperty(ProcessGeneration.PropertyController, property.ControllerName),
                        new JProperty(ProcessGeneration.PropertyAction, property.ActionName)
                    };
                    fields.Add(new JProperty(ProcessGeneration.PropertyDrilldownUrl, drilldownProperties));
                }

                fieldsArray.Add(fields);
            }

            json.Add(new JProperty(ProcessGeneration.PropertyFields, fieldsArray));

            return json;
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

            // Show read only in InactiveCaption color
            if (readOnly)
            {
                grid.Columns[column].DefaultCellStyle.BackColor = SystemColors.InactiveCaption;
            }
        }


        /// <summary> Clear Generate Controls </summary>
        /// <param name="name">model name</param>
        private void ClearGenerateControls(string name)
        {
            lblGenerateJson.Text = string.Format(Resources.JsonToGenerateTip, BuildFileName(name));
            txtJsonToGenerate.Clear();
        }

        /// <summary> BuildFileName </summary>
        /// <param name="name">model name</param>
        /// <returns>File name {model}InquiryConfiguration.json</returns>
        private static string BuildFileName(string name)
        {
            return name + ProcessGeneration.PropertyFileNameSuffix;
        }

        /// <summary> Assembly has changed, so update model dropdown</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboAssembly_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then clear model and grid
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                cboModel.SelectedIndex = 0;
                DeleteRows();
            }
            else
            {
                // Get assembly selected and populate the model list
                InitModelSource(((ComboBox) sender).SelectedItem.ToString());
            }
        }

        /// <summary> Model has changed, so update grid</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then clear other controls downstream
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                DeleteRows();
            }
            else
            {
                // Load the grid
                DeleteRows();
                AssignGrid(_models[((ComboBox)sender).SelectedItem.ToString()]);
            }
        }

        /// <summary> Valid Create/Edit Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidCreateEditStep()
        {
            // Inquiry Id
            if (string.IsNullOrEmpty(txtInquiryId.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Inquiry.Replace(":", ""));
            }

            // Folder
            if (string.IsNullOrEmpty(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Folder.Replace(":", ""));
            }

            // Description
            if (string.IsNullOrEmpty(txtInquiryDescription.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.InquiryDescription.Replace(":", ""));
            }

            // Assembly
            if (cboAssembly.SelectedIndex == 0)
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Assembly.Replace(":", ""));
            }

            // Model
            if (cboModel.SelectedIndex == 0)
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Model.Replace(":", ""));
            }

            // Validate grid

            // At least 1 row must be included and if drilldown is selected, the area, controller and action must be specified
            var included = false;
            var message = string.Empty;

            // Iterate grid
            foreach (var property in _properties)
            {
                // Only consider included properties
                if (!property.IsIncluded)
                {
                    continue;
                }

                // At least one row is included
                included = true;

                // Check drilldown
                if (property.IsDrilldown)
                {
                    // Drilldown area must be specified
                    if (string.IsNullOrEmpty(property.Area))
                    {
                        message = Resources.Area;
                        break;
                    }

                    // Drilldown area must be specified
                    if (string.IsNullOrEmpty(property.ControllerName))
                    {
                        message = Resources.Controller;
                        break;
                    }

                    // Drilldown area must be specified
                    if (string.IsNullOrEmpty(property.ActionName))
                    {
                        message = Resources.Action;
                        break;
                    }

                    // Next validation here, if any
                }
            }

            // Were any rows included?
            if (!included)
            {
                return Resources.InvalidSettingRequiredProperty;
            }

            // Any validation message?
            if (!string.IsNullOrEmpty(message))
            {
                return string.Format(Resources.InvalidSettingRequiredField, message.Replace(":", ""));
            }

            return string.Empty;
        }

        #endregion

    }
}
