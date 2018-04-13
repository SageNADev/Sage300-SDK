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
using ACCPAC.Advantage;

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

        /// <summary> Source for config</summary>
        private Source _source = new Source();

        /// <summary> Source for viwe column</summary>
        private Source _viewSource = new Source();

        /// <summary> All Columns in grid for inclusion</summary>
        private readonly BindingList<SourceColumn> _sourceColumns = new BindingList<SourceColumn>();

        /// <summary> All SQL Columns in grid for inclusion</summary>
        private readonly BindingList<SourceColumn> _sourceSqlColumns = new BindingList<SourceColumn>();

        /// <summary> Captions in grid </summary>
        private readonly BindingList<Caption> _captions = new BindingList<Caption>();

        /// <summary> Parameters in grid </summary>
        private readonly BindingList<Parameter> _parameters = new BindingList<Parameter>();

        /// <summary> Filters in grid </summary>
        private readonly BindingList<Filter> _filters = new BindingList<Filter>();

        /// <summary> Aggregation types check box group </summary>
        private readonly BindingList<string> _aggregationTypes = new BindingList<string>();

        /// <summary> Included columns in grid </summary>
        private readonly BindingList<SourceColumn> _includedColumns = new BindingList<SourceColumn>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        /// <summary> Mode Type for Edit or None </summary>
        private ModeType _modeType = ModeType.None;

        /// <summary> Clicked Column </summary>
        private SourceColumn _clickedColumn;

        /// <summary> Existing JSON </summary>
        private JObject _json;

        /// <summary> Drag and Drop - for Mouse Down </summary>
        private Rectangle _dragBoxFromMouseDown;

        /// <summary> Drag and Drop - selected row </summary>
        private int _rowIndexFromMouseDown;

        /// <summary> Drag and Drop - dropped row </summary>
        private int _rowIndexOfItemUnderMouseToDrop;

        #endregion

        #region Private Constants

        /// <summary> Panel Name for pnlCreateEdit </summary>
        private const string PanelCreateEdit = "pnlCreateEdit";

        /// <summary> Panel Name for pnlSourceView </summary>
        private const string PanelSourceView = "pnlSourceView";

        /// <summary> Panel Name for pnlSourceSql </summary>
        private const string PanelSourceSql = "pnlSourceSql";

        /// <summary> Panel Name for pnlColumns </summary>
        private const string PanelColumns = "pnlColumns";

        /// <summary> Panel Name for pnlGenerate </summary>
        private const string PanelGenerate = "pnlGenerate";

        /// <summary> Panel Name for pnlGenerated </summary>
        private const string PanelGenerated = "pnlGenerated";

        /// <summary> Splitter Distance </summary>
        private const int SplitterDistance = 415;

        #endregion

        #region Private Enums
        /// <summary>
        /// Enum for Mode Types
        /// </summary>
        private enum ModeType
        {
            /// <summary> No Mode </summary>
            None = 0,

            /// <summary> Edit Mode</summary>
            Edit = 1
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

        #region Public Routines

        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            Localize();
            InitWizardSteps();
            InitInfo();
            InitColumnGrid(grdSourceColumns, _sourceColumns);
            InitSqlColumnGrid(grdSqlColumns, _sourceSqlColumns);
            InitIncludedColumnsGrid(grdIncludedColumns, _includedColumns);
            InitCaptionGrid(grdCaptions, _captions);
            InitParameterGrid(grdParameters, _parameters);
            InitFilterGrid(grdFilters, _filters);
            InitEvents();
            ProcessingSetup(true);
            Processing("");
        }

        #endregion

        #region Private Routines

        /// <summary> Delete all rows</summary>
        /// <param name="grid">Grid to delete from</param>
        private void DeleteRows(DataGridView grid)
        {
            // Iterate grid
            for (var i = grid.Rows.Count - 1; i >= 0; i--)
            {
                if (!grid.Rows[i].IsNewRow)
                {
                    grid.Rows.Remove(grid.Rows[i]);
                }
            }
        }

        /// <summary> Add Captions</summary>
        /// <param name="sourceColumn">Source Column</param>
        /// <param name="language">Language</param>
        private void AddCaptions(SourceColumn sourceColumn)
        {
            // Do not add if already present (SQl column name change scenario)
            if (sourceColumn.Captions.Count > 0)
            {
                return;
            }

            AddCaption(sourceColumn, ProcessGeneration.PropertyEnglish, sourceColumn.DescriptionENG);
            AddCaption(sourceColumn, ProcessGeneration.PropertyFrench, sourceColumn.DescriptionFRA);
            AddCaption(sourceColumn, ProcessGeneration.PropertySpanish, sourceColumn.DescriptionESN);
            AddCaption(sourceColumn, ProcessGeneration.PropertyChineseSimplified, sourceColumn.DescriptionCHN);
            AddCaption(sourceColumn, ProcessGeneration.PropertyChineseTraditional, sourceColumn.DescriptionCHT);
        }

        /// <summary> Add Caption</summary>
        /// <param name="sourceColumn">Source Column</param>
        /// <param name="language">Language for caption</param>
        /// <param name="text">Text if any</param>
        private void AddCaption(SourceColumn sourceColumn, string language, string text = "")
        {
            sourceColumn.Captions.Add(language, new Caption
            {
                Language = language,
                Text = text
            });
        }

        /// <summary> Clear and Set Datasource </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void ClearAndSetDatasource<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data
            DeleteRows(grid);

            // Assign binding to datasource (two binding)
            grid.DataSource = datasource;
            grid.ScrollBars = ScrollBars.Vertical;
        }

        /// <summary> Initialize Columns grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitColumnGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 50, Resources.Index.Replace(":", ""), true, true);
            GenericInit(grid, 1, 125, Resources.Column.Replace(":", ""), true, true);
            GenericInit(grid, 2, 200, Resources.InquiryDescription.Replace(":", ""), true, true);
            GenericInit(grid, 3, 100, Resources.DataType.Replace(":", ""), true, true);
            GenericInit(grid, 4, 75, Resources.Include, true, false);
            GenericInit(grid, 5, 50, "", false, true);
            GenericInit(grid, 6, 50, "", false, true);
            GenericInit(grid, 7, 50, "", false, true);
            GenericInit(grid, 8, 50, "", false, true);
            GenericInit(grid, 9, 50, "", false, true);
            GenericInit(grid, 10, 50, "", false, true);
            GenericInit(grid, 11, 50, "", false, true);
            GenericInit(grid, 12, 50, "", false, true);
            GenericInit(grid, 13, 50, "", false, true);
            GenericInit(grid, 14, 50, "", false, true);
            GenericInit(grid, 15, 50, "", false, true);
            GenericInit(grid, 16, 50, "", false, true);
            GenericInit(grid, 17, 50, "", false, true);
            GenericInit(grid, 18, 50, "", false, true);
            GenericInit(grid, 19, 50, "", false, true);
        }

        /// <summary> Initialize SQL Columns grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitSqlColumnGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 50, "", false, true);
            GenericInit(grid, 1, 200, Resources.Column.Replace(":", ""), true, false);
            GenericInit(grid, 2, 200, Resources.InquiryDescription.Replace(":", ""), true, false);

            // Remove and re-add as combobox
            grid.Columns.Remove("Type");
            var column = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = Resources.DataType.Replace(":", ""),
                DropDownWidth = 100,
                Width = 75,
                FlatStyle = FlatStyle.Flat
            };
            // Add enums to drop down list
            foreach (var sourceDataType in Enum.GetValues(typeof(SourceDataType)).Cast<SourceDataType>())
            {
                column.Items.Add(sourceDataType);
            }

            // Re-add column
            grid.Columns.Insert(3, column);
            GenericInit(grid, 3, 100, Resources.DataType.Replace(":", ""), true, false);

            GenericInit(grid, 4, 75, "", false, true);
            GenericInit(grid, 5, 50, "", false, true);
            GenericInit(grid, 6, 50, "", false, true);
            GenericInit(grid, 7, 50, "", false, true);
            GenericInit(grid, 8, 50, "", false, true);
            GenericInit(grid, 9, 50, "", false, true);
            GenericInit(grid, 10, 50, "", false, true);
            GenericInit(grid, 11, 50, "", false, true);
            GenericInit(grid, 12, 50, "", false, true);
            GenericInit(grid, 13, 50, "", false, true);
            GenericInit(grid, 14, 50, "", false, true);
            GenericInit(grid, 15, 50, "", false, true);
            GenericInit(grid, 16, 50, "", false, true);
            GenericInit(grid, 17, 50, "", false, true);
            GenericInit(grid, 18, 50, "", false, true);
            GenericInit(grid, 19, 50, "", false, true);
        }

        /// <summary> Initialize Included Columns grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitIncludedColumnsGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 50, Resources.Index.Replace(":", ""), true, true);
            GenericInit(grid, 1, 125, Resources.Column.Replace(":", ""), true, true);
            GenericInit(grid, 2, 200, Resources.InquiryDescription.Replace(":", ""), true, true);
            GenericInit(grid, 3, 100, Resources.DataType.Replace(":", ""), true, true);
            GenericInit(grid, 4, 50, "", false, true);
            GenericInit(grid, 5, 75, Resources.Display, true, true);
            GenericInit(grid, 6, 50, "", false, true);
            GenericInit(grid, 7, 50, "", false, true);
            GenericInit(grid, 8, 75, Resources.DrilldownTab, true, true);
            GenericInit(grid, 9, 50, "", false, true);
            GenericInit(grid, 10, 50, "", false, true);
            GenericInit(grid, 11, 50, "", false, true);
            GenericInit(grid, 12, 50, "", false, true);
            GenericInit(grid, 13, 75, Resources.FilterableColumn, true, true);
            GenericInit(grid, 14, 50, "", false, true);
            GenericInit(grid, 15, 50, "", false, true);
            GenericInit(grid, 16, 50, "", false, true);
            GenericInit(grid, 17, 50, "", false, true);
            GenericInit(grid, 18, 75, Resources.GroupByColumn, true, false);
            GenericInit(grid, 19, 75, Resources.AggregationColumn, true, true);
        }

        /// <summary> Initialize grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitCaptionGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 100, Resources.Language, true, true);
            GenericInit(grid, 1, 200, Resources.Caption, true, false);
        }

        /// <summary> Initialize grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitParameterGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 100, Resources.Parameter, true, false);
        }

        /// <summary> Initialize grid and display </summary>
        /// <param name="grid">Grid requiring initialization</param>
        /// <param name="datasource">Datasource for grid</param>
        private void InitFilterGrid<T>(DataGridView grid, BindingList<T> datasource)
        {
            // Clear data and set datasource
            ClearAndSetDatasource(grid, datasource);

            // Config and localize
            GenericInit(grid, 0, 100, Resources.Text, true, false);
            GenericInit(grid, 1, 100, Resources.Value, true, false);
        }

        /// <summary> Add wizard step </summary>
        /// <param name="title">Title for wizard step</param>
        /// <param name="description">Description for wizard step</param>
        /// <param name="panel">Panel for wizard step</param>
        /// <param name="focusControl">Control to receive focus when step is displayed</param>
        private void AddStep(string title, string description, Panel panel, Control focusControl)
        {
            _wizardSteps.Add(new WizardStep
            {
                Title = title,
                Description = description,
                Panel = panel,
                FocusControl = focusControl
            });
        }

        /// <summary> Add wizard step at index specified </summary>
        /// <param name="title">Title for wizard step</param>
        /// <param name="description">Description for wizard step</param>
        /// <param name="panel">Panel for wizard step</param>
        /// <param name="focusControl">Control to receive focus when step is displayed</param>
        /// <param name="index">Add at index specified</param>
        private void AddStep(string title, string description, Panel panel, Control focusControl, int index)
        {
            _wizardSteps.Insert(index, new WizardStep
            {
                Title = title,
                Description = description,
                Panel = panel,
                FocusControl = focusControl
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

            btnSave.Text = Resources.Save;
            btnCancel.Text = Resources.Cancel;
            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

            // Step Create/Edit
            lblInquiryId.Text = Resources.Inquiry;
            tooltip.SetToolTip(lblInquiryId, Resources.InquiryIdTip);

            lblFolder.Text = Resources.Folder;
            tooltip.SetToolTip(lblFolder, Resources.FolderNameTip);

            lblInquiryName.Text = Resources.InquiryName;
            tooltip.SetToolTip(lblInquiryName, Resources.InquiryNameTip);

            lblInquiryDescription.Text = Resources.InquiryDescription;
            tooltip.SetToolTip(lblInquiryDescription, Resources.InquiryDescriptionTip);

            tooltip.SetToolTip(btnInquiryFinder, Resources.InquiryFinderTip);
            tooltip.SetToolTip(btnNew, Resources.InquiryNewTip);
            tooltip.SetToolTip(btnFolder, Resources.FolderFinderTip);

            grpCredentials.Text = Resources.Credentials;

            lblUser.Text = Resources.User;
            tooltip.SetToolTip(lblUser, Resources.UserTip);

            lblPassword.Text = Resources.Password;
            tooltip.SetToolTip(lblPassword, Resources.PasswordTip);

            lblVersion.Text = Resources.Version;
            tooltip.SetToolTip(lblVersion, Resources.VersionTip);

            lblCompany.Text = Resources.Company;
            tooltip.SetToolTip(lblCompany, Resources.CompanyTip);

            chkUseBusinessView.Text = Resources.UseBusinessView;
            tooltip.SetToolTip(chkUseBusinessView, Resources.UserTip);

            // Step Source - View
            lblViewID.Text = Resources.ViewId;
            tooltip.SetToolTip(lblViewID, Resources.ViewIdTip);

            tooltip.SetToolTip(grdSourceColumns, Resources.ViewColumnsTip);

            // Step Source - SQL
            lblSqlSource.Text = Resources.SqlSource;
            lblWrapperInstructions.Text = Resources.WrapperInstructions;
            tabSqlStatement.Text = Resources.SqlStatement;
            tabSqlStatement.ToolTipText = Resources.SqlStatementTabTip;

            tabWrapper.Text = Resources.WrapperColsClauses;
            tabWrapper.ToolTipText = Resources.WrapperTabTip;

            tooltip.SetToolTip(txtSQL, Resources.SqlTip);

            tooltip.SetToolTip(btnSqlHelp, Resources.SqlHelpTip);

            lblWhereClause.Text = Resources.WhereClause;
            tooltip.SetToolTip(lblWhereClause, Resources.WhereClauseTip);

            lblOrderByClause.Text = Resources.OrderByClause;
            tooltip.SetToolTip(lblOrderByClause, Resources.OrderByClauseTip);

            // Step Columns
            lblColumns.Text = Resources.ColumnInstructions;

            // Step Columns - Column
            tabColumn.Text = Resources.ColumnTab;
            tabColumn.ToolTipText = Resources.ColumnTabTip;

            lblColumn.Text = Resources.Column;
            tooltip.SetToolTip(lblColumn, Resources.ColumnTip);

            chkDisplayColumn.Text = Resources.DisplayColumn;
            tooltip.SetToolTip(chkDisplayColumn, Resources.DisplayColumnTip);

            lblDataType.Text = Resources.DataType;
            tooltip.SetToolTip(lblDataType, Resources.DataTypeTip);

            lblCaptions.Text = Resources.Captions;
            tooltip.SetToolTip(grdCaptions, Resources.LanguagesTip);

            // Step Columns - Drilldown
            tabDrilldown.Text = Resources.DrilldownTab;
            tabDrilldown.ToolTipText = Resources.DrilldownTabTip;

            chkDrilldown.Text = Resources.DrilldownFromColumn;
            tooltip.SetToolTip(chkDrilldown, Resources.DrilldownFromColumnTip);

            lblArea.Text = Resources.Area;
            tooltip.SetToolTip(lblArea, Resources.AreaTip);

            lblController.Text = Resources.Controller;
            tooltip.SetToolTip(lblController, Resources.ControllerTip);

            splitSqlColumns.Text = Resources.Action;
            tooltip.SetToolTip(splitSqlColumns, Resources.ActionTip);

            lblParameters.Text = Resources.Parameters;
            tooltip.SetToolTip(lblParameters, Resources.ParametersTip);

            // Step Columns - Filtering
            tabFiltering.Text = Resources.FilteringTab;
            tabFiltering.ToolTipText = Resources.FilteringTabTip;

            chkFilterable.Text = Resources.Filterable;
            tooltip.SetToolTip(chkFilterable, Resources.FilterableTip);

            chkColumnInView.Text = Resources.ColumnView;
            tooltip.SetToolTip(chkColumnInView, Resources.ColumnViewTip);

            lblFilterViewId.Text = Resources.ViewId;
            tooltip.SetToolTip(lblFilterViewId, Resources.ViewIdTip);

            lblFilterColumn.Text = Resources.Column;
            tooltip.SetToolTip(lblFilterColumn, Resources.ColumnNameTip);

            lblFilters.Text = Resources.Filters;
            tooltip.SetToolTip(lblFilters, Resources.FiltersTip);

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
            InitPanel(pnlSourceView);
            InitPanel(pnlSourceSql);
            InitPanel(pnlColumns);
            InitPanel(pnlGenerate);
            InitPanel(pnlGenerated);

            // Assign steps for wizard
            AddStep(Resources.StepTitleCreateEdit, Resources.StepDescriptionCreateEdit, pnlCreateEdit, txtInquiryId);

            // Step specific to View or Sql
            if (chkUseBusinessView.Checked)
            {
                AddStep(Resources.StepTitleSource, Resources.StepDescriptionSourceView, pnlSourceView, txtViewID);
            }
            else
            {
                AddStep(Resources.StepTitleSource, Resources.StepDescriptionSourceSql, pnlSourceSql, txtSQL);
            }

            AddStep(Resources.StepTitleColumns, Resources.StepDescriptionColumns, pnlColumns, grdIncludedColumns);
            AddStep(Resources.StepTitleGenerate, Resources.StepDescriptionGenerate, pnlGenerate, btnNext);
            AddStep(Resources.StepTitleGenerated, Resources.StepDescriptionGenerated, pnlGenerated, btnNext);

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
                        var name = _source.Properties[Source.Name];
                        var configurationFileName = BuildConfigurationFileName(name);
                        var templateFileName = BuildTemplateFileName(name);
                        ClearGenerateControls(configurationFileName, templateFileName);

                        // Generate JSONs
                        var configurationJson = GenerateConfigurationJson(configurationFileName);
                        txtConfigurationToGenerate.Text = configurationJson.ToString();

                        var templateJson = GenerateTemplateJson(templateFileName);
                        txtTemplateToGenerate.Text = templateJson.ToString();

                        // Establish settings for processing (Validation already ocurred in each step)
                        _settings = new Settings
                        {
                            FolderName = txtFolderName.Text.Trim(),
                            ConfigurationJson = configurationJson,
                            ConfigurationFileName = configurationFileName,
                            TemplateJson = templateJson,
                            TemplateFileName = templateFileName
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

            // Source Step - View
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelSourceView))
            {
                valid = ValidSourceStepView();
            }

            // Source Step - Sql
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelSourceSql))
            {
                valid = ValidSourceStepSql();
            }

            // Columns Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelColumns))
            {
                valid = ValidColumnsStep();
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

            // Set focus?
            if (visible && _wizardSteps[_currentWizardStep].FocusControl != null)
            {
                try
                {
                    _wizardSteps[_currentWizardStep].FocusControl.Focus();
                }
                catch
                {
                }
            }
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
            ExistingConfiguration(dialog.FileName.Trim());
        }

        /// <summary> New customization</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewConfiguration();
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

        /// <summary> Existing Configuration </summary>
        /// <param name="fileName">File name </param>
        private void ExistingConfiguration(string fileName)
        {
            try
            {
                // Get the configuration 
                _json = JObject.Parse(File.ReadAllText(fileName));

                // Clear all controls
                ClearCreateEditControls();
                ClearSourceViewControls();
                ClearSourceSqlControls();
                ClearColumnsControls();
                ClearColumnControls();

                // Init Source
                _source.Properties.Clear();
                _source.Options.Clear();
                _source.SourceColumns.Clear();

                // Step - Create/Edit
                _source.Properties[Source.InquiryId] = (string)_json.SelectToken(ProcessGeneration.PropertyInquiryId);
                _source.Properties[Source.FolderName] = Path.GetDirectoryName(fileName);
                _source.Properties[Source.Name] = (string)_json.SelectToken(ProcessGeneration.PropertyName);
                _source.Properties[Source.Description] = (string)_json.SelectToken(ProcessGeneration.PropertyDescription);
                _source.Options[Source.IsBusinessView] = (string.IsNullOrEmpty((string)_json.SelectToken(ProcessGeneration.PropertySql)));

                txtInquiryId.Text = _source.Properties[Source.InquiryId];
                txtFolderName.Text = _source.Properties[Source.FolderName];
                txtInquiryName.Text = _source.Properties[Source.Name];
                txtInquiryDescription.Text = _source.Properties[Source.Description];
                chkUseBusinessView.Checked = _source.Options[Source.IsBusinessView];

                // Step - Source View
                _source.Properties[Source.ViewId] = (string)_json.SelectToken(ProcessGeneration.PropertyViewName);
                txtViewID.Text = _source.Properties[Source.ViewId];

                // Step - Source SQL
                _source.Properties[Source.SqlStatement] = (string)_json.SelectToken(ProcessGeneration.PropertySql);
                _source.Properties[Source.WhereClause] = (string)_json.SelectToken(ProcessGeneration.PropertyWhereClause);
                _source.Properties[Source.OrderByClause] = (string)_json.SelectToken(ProcessGeneration.PropertyOrderByClause);

                txtSQL.Text = _source.Properties[Source.SqlStatement];
                txtWhereClause.Text = _source.Properties[Source.WhereClause];
                txtOrderByClause.Text = _source.Properties[Source.OrderByClause];

                // Step - Columns
                // If using a business view, this is delayed until the ValidCreateEdit Step where valid 
                // business view credentials have been entered else load fields here since there is not a
                // business view that needs to be accessed to get the full columns
                if (!_source.Options[Source.IsBusinessView])
                {
                    // Load the configuration now
                    LoadConfiguration();
                }

                // Load the template json file just to be sure it is there and loads. But, no need at this point
                var templateFilename = fileName.Replace(ProcessGeneration.PropertyConfiguration, ProcessGeneration.PropertyTemplate);
                var templateJson = JObject.Parse(File.ReadAllText(templateFilename));

                // Parse and store template file here if needed

            }
            catch (Exception ex)
            {
                // Error received attempting to load JSON
                DisplayMessage((ex.InnerException == null) ? ex.Message : ex.InnerException.Message, MessageBoxIcon.Error);
            }

        }

        /// <summary> New Configuration</summary>
        private void NewConfiguration()
        {
            // Clear source container which holds all data for JSON creation
            ClearSource(_source);

            // Clear all controls
            ClearCreateEditControls();
            ClearSourceViewControls();
            ClearSourceSqlControls();
            ClearColumnsControls();
            ClearColumnControls();

            // Generate GUID for configuration and store in source
            txtInquiryId.Text = Guid.NewGuid().ToString();
            _source.Properties[Source.InquiryId] = txtInquiryId.Text;

            // Set focus to folder field
            txtFolderName.Focus();
        }

        /// <summary> Clear Source</summary>
        /// <param name="source">Source object </param>
        private void ClearSource(Source source)
        {
            source.Properties.Clear();
            source.Options.Clear();
            source.SourceColumns.Clear();
        }


        /// <summary> Clear Create Edit Controls</summary>
        private void ClearCreateEditControls()
        {
            txtFolderName.Text = string.Empty;
            txtInquiryName.Text = string.Empty;
            txtInquiryDescription.Text = string.Empty;
            chkUseBusinessView.Checked = true;
        }

        /// <summary> Clear Source View Controls</summary>
        private void ClearSourceViewControls()
        {
            txtViewID.Text = string.Empty;
            DeleteRows(grdSourceColumns);
        }

        /// <summary> Clear Source SQL Controls</summary>
        private void ClearSourceSqlControls()
        {
            txtSQL.Text = string.Empty;
            DeleteRows(grdSqlColumns);
            txtWhereClause.Text = string.Empty;
            txtOrderByClause.Text = string.Empty;
        }

        /// <summary> Clear Columns Congtrols</summary>
        private void ClearColumnsControls()
        {
            DeleteRows(grdIncludedColumns);
        }

        /// <summary> Clear Column Congtrols</summary>
        private void ClearColumnControls()
        {
            // Column
            txtColumn.Text = string.Empty;
            chkDisplayColumn.Checked = false;
            txtDataType.Text = SourceDataType.None.ToString();
            DeleteRows(grdCaptions);

            // Drilldown
            chkDrilldown.Checked = false;
            txtArea.Text = string.Empty;
            txtController.Text = string.Empty;
            txtAction.Text = string.Empty;
            DeleteRows(grdParameters);
            EnableDrilldown(chkDrilldown.Checked);

            // Filtering
            chkFilterable.Checked = true;
            chkColumnInView.Checked = false;
            txtFilterViewId.Text = string.Empty;
            cboFilterColumn.Items.Clear();
            DeleteRows(grdFilters);
            EnableColumnInView(chkColumnInView.Checked);
            EnableFilters(false);
        }

        /// <summary> Column Setup</summary>
        /// <param name="modeType">Mode Type (Add)</param>
        private void ColumnSetup(ModeType modeType)
        {
            // Disable buttons
            EnableNavigationButtons(false);

            // Set mode type and clear controls
            _modeType = modeType;
            ClearColumnControls();

            // Load controls from column or set defaults
            LoadColumnControls();

            // Enable column controls
            EnableColumnControls(true);
        }

        /// <summary> Enable or disable navigation buttons</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableNavigationButtons(bool enable)
        {
            btnNext.Enabled = enable;
            btnBack.Enabled = enable;
        }

        /// <summary> Enable or disable column controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableColumnControls(bool enable)
        {
            btnSave.Visible = enable;
            btnCancel.Visible = enable;
            grdIncludedColumns.Enabled = !enable;
            tabIncludedColumn.Enabled = enable;
            tabIncludedColumn.Visible = enable;

            var useBusinessView = _source.Options[Source.IsBusinessView];
            var dataType = (SourceDataType)Enum.Parse(typeof(SourceDataType), txtDataType.Text.Trim());


            // Do not enable column in view controls in certain scenarios
            chkColumnInView.Enabled = enable && !useBusinessView && (dataType.Equals(SourceDataType.Enumeration));
            EnableColumnInView(chkColumnInView.Checked && enable && !useBusinessView);

            // Do not enable filters grid in certain scenarios
            EnableFilters(!chkColumnInView.Checked && enable && 
                !useBusinessView && (dataType.Equals(SourceDataType.Enumeration)));

            // Drilldown is based upon drilldown checkbox
            EnableDrilldown(chkDrilldown.Checked);

            // Select the first tab
            tabIncludedColumn.SelectTab(0);
        }

        /// <summary> Load Column Controls from included column</summary>
        private void LoadColumnControls()
        {
            // Column
            txtColumn.Text = _clickedColumn.Name;
            chkDisplayColumn.Checked = _clickedColumn.IsDisplayable;
            txtDataType.Text = _clickedColumn.Type.ToString();

            foreach (var caption in _clickedColumn.Captions.Values)
            {
                _captions.Add(caption);
            }

            // Drilldown
            chkDrilldown.Checked = _clickedColumn.IsDrilldown;
            txtArea.Text = _clickedColumn.AreaName;
            txtController.Text = _clickedColumn.ControllerName;
            txtAction.Text = _clickedColumn.ActionName;

            foreach (var param in _clickedColumn.Params.Values)
            {
                _parameters.Add(param);
            }

            // Filtering
            chkFilterable.Checked = _clickedColumn.IsFilterable;
            chkColumnInView.Checked = _clickedColumn.IsColumnInView;
            txtFilterViewId.Text = _clickedColumn.ViewId;
            txtFilterViewId.Tag = txtFilterViewId.Text;
            cboFilterColumn.SelectedText = _clickedColumn.ViewColumnName;

            foreach (var filter in _clickedColumn.Filters.Values)
            {
                _filters.Add(filter);
            }

            // Aggregation
            //foreach (var aggregationType in _clickedColumn.Aggregation.Keys)
            //{
            //    _aggregationTypes.Add(aggregationType);

            //}

            for (int i = 0; i < _clickedColumn.Aggregation.Keys.Count; i++)
            {
                //if (_clickedColumn.Aggregation.)
                //{

                //}

                chkAggregation.SetItemChecked(i, true);
            }

            return;
        }

        /// <summary> Cancel any column changes</summary>
        private void CancelColumn()
        {
            // Reset mode
            _modeType = ModeType.None;

            // Disable buttons
            EnableNavigationButtons(true);

            // Clear inidividual column controls
            ClearColumnControls();

            // Disable column controls
            EnableColumnControls(false);

            // Set focus back to grid
            grdIncludedColumns.Focus();
        }

        /// <summary> Column has changed, so update</summary>
        private void SaveColumn()
        {
            // Validation first
            var dataType = (SourceDataType)Enum.Parse(typeof(SourceDataType), txtDataType.Text.Trim());
            var success = ValidColumn(dataType, _captions.ToList(), chkDrilldown.Checked,
                txtArea.Text.Trim(), txtController.Text.Trim(), txtAction.Text.Trim(),
                chkColumnInView.Checked, txtFilterViewId.Text.Trim(), cboFilterColumn.Text,
                _filters.ToList());
            if (!string.IsNullOrEmpty(success))
            {
                DisplayMessage(success, MessageBoxIcon.Error);
                return;
            }

            // Column
            _clickedColumn.IsDisplayable = chkDisplayColumn.Checked;
            _clickedColumn.Type = (SourceDataType)Enum.Parse(typeof(SourceDataType), txtDataType.Text.Trim());
            _clickedColumn.Captions.Clear();

            foreach (var caption in _captions)
            {
                _clickedColumn.Captions.Add(caption.Language, caption);
            }

            // Drilldown
            _clickedColumn.IsDrilldown = chkDrilldown.Checked;
            _clickedColumn.AreaName = txtArea.Text.Trim();
            _clickedColumn.ControllerName = txtController.Text.Trim();
            _clickedColumn.ActionName = txtAction.Text.Trim();
            _clickedColumn.Params.Clear();

            foreach (var param in _parameters)
            {
                _clickedColumn.Params.Add(param.Name, param);
            }

            // Aggregation
            foreach (var checkedItem in chkAggregation.CheckedItems)
            {
                _clickedColumn.Aggregation.Add(checkedItem.ToString(), true);
            }

            for (int i = 0; i < chkAggregation.Items.Count; i++)
            {
                chkAggregation.SetItemChecked(i, false);
            }

            // Filtering
            _clickedColumn.IsFilterable = chkFilterable.Checked;
            _clickedColumn.IsColumnInView = chkColumnInView.Checked;
            _clickedColumn.ViewId = txtFilterViewId.Text.Trim();
            _clickedColumn.ViewColumnName = cboFilterColumn.Text.Trim();
            _clickedColumn.Filters.Clear();

            foreach (var filter in _filters)
            {
                _clickedColumn.Filters.Add(filter.Value, filter);
            }

            // Reset mode
            _modeType = ModeType.None;

            // Enable buttons
            EnableNavigationButtons(true);

            // Clear individual column controls
            ClearColumnControls();

            // Disable column controls
            EnableColumnControls(false);

            // Set focus back to grid
            grdIncludedColumns.Focus();
        }

        /// <summary> Generate JSON</summary>
        /// <param name="key">Key to properties dictionary </param>
        /// <returns>Content otherwise string.empty</returns>
        private object GetContent(string key)
        {
            return _source.Properties.ContainsKey(key) ? _source.Properties[key] : string.Empty;
        }

        /// <summary> Generate Configuration JSON</summary>
        /// <param name="fileName">File name</param>
        private JObject GenerateConfigurationJson(string fileName)
        {
            // Add properties
            var json = new JObject
            {
                new JProperty(ProcessGeneration.PropertyGeneratedMessage, Resources.GeneratedMessage),
                new JProperty(ProcessGeneration.PropertyGeneratedWarning, Resources.GeneratedWarning),
                new JProperty(ProcessGeneration.PropertyInquiryId, GetContent(Source.InquiryId)),
                new JProperty(ProcessGeneration.PropertyFileName, fileName),
                new JProperty(ProcessGeneration.PropertyName, GetContent(Source.Name)),
                new JProperty(ProcessGeneration.PropertyDescription, GetContent(Source.Description)),
                new JProperty(ProcessGeneration.PropertyViewName, GetContent(Source.ViewId)),
                new JProperty(ProcessGeneration.PropertySql, GetContent(Source.SqlStatement)),
                new JProperty(ProcessGeneration.PropertyWhereClause, GetContent(Source.WhereClause)),
                new JProperty(ProcessGeneration.PropertyOrderByClause, GetContent(Source.OrderByClause))
            };

            // Create array of security rights - HARDCODED FOR NOW
            var securityRightsArray = new JArray();

            var securityRights = new JObject();
            securityRights.Add(new JProperty("Name", "AP1"));
            securityRights.Add(new JProperty("Code", "AP1"));
            securityRightsArray.Add(securityRights);

            json.Add(new JProperty(ProcessGeneration.PropertySecurityRights, securityRightsArray));

            // Create array of fields
            var fieldsArray = new JArray();
            
            foreach (var sourceColumn in _source.SourceColumns.Values.OrderBy(column => column.DisplayOrder))
            {
                // Skip column if not included
                if (!sourceColumn.IsIncluded)
                {
                    continue;
                }

                var fields = new JObject();

                fields.Add(new JProperty(ProcessGeneration.PropertyField, sourceColumn.Name));
                fields.Add(new JProperty(ProcessGeneration.PropertyFieldIndex, sourceColumn.Id));

                // Add captions
                var captionsArray = new JArray();

                // Iterate captions
                foreach (var caption in sourceColumn.Captions)
                {
                    var captionObj = new JObject
                        {
                            new JProperty(ProcessGeneration.PropertyLanguage, caption.Value.Language),
                            new JProperty(ProcessGeneration.PropertyValue, caption.Value.Text)
                        };

                    captionsArray.Add(captionObj);
                }
                fields.Add(new JProperty(ProcessGeneration.PropertyCaptions, captionsArray));

                // Is column in a view (for SQL columns)
                fields.Add(new JProperty(ProcessGeneration.PropertyIsColumnInView, sourceColumn.IsColumnInView));
                fields.Add(new JProperty(ProcessGeneration.PropertyViewName, sourceColumn.ViewId));
                fields.Add(new JProperty(ProcessGeneration.PropertyColumnNameInView, sourceColumn.ViewColumnName));

                fields.Add(new JProperty(ProcessGeneration.PropertyDataTypeEnumeration, sourceColumn.Type));
                fields.Add(new JProperty(ProcessGeneration.PropertyDataType, EnumValue.GetValue(sourceColumn.Type)));

                // If there are filters, then add filter values
                if (sourceColumn.Filters.Count > 0)
                {
                    // Create array of filters
                    var filtersArray = new JArray();
                    var count = 0;

                    // Iterate enums
                    foreach (var filter in sourceColumn.Filters)
                    {
                        // Increment count for selected check
                        count++;

                        var enumObj = new JObject
                        {
                            new JProperty(ProcessGeneration.PropertySelected, count == 1),
                            new JProperty(ProcessGeneration.PropertyText, filter.Value.Text),
                            new JProperty(ProcessGeneration.PropertyValue, filter.Value.Value)
                        };

                        filtersArray.Add(enumObj);
                    }

                    fields.Add(new JProperty(ProcessGeneration.PropertyFilters, filtersArray));
                }

                fields.Add(new JProperty(ProcessGeneration.PropertyIsDisplayable, sourceColumn.IsDisplayable));
                fields.Add(new JProperty(ProcessGeneration.PropertyIsFilterable, sourceColumn.IsFilterable));
                fields.Add(new JProperty(ProcessGeneration.PropertyIsDrilldown, sourceColumn.IsDrilldown));

                // If drilldown, then add drill down url
                if (sourceColumn.IsDrilldown)
                {
                    var drilldownProperties = new JObject
                    {
                        new JProperty(ProcessGeneration.PropertyArea, sourceColumn.AreaName),
                        new JProperty(ProcessGeneration.PropertyController, sourceColumn.ControllerName),
                        new JProperty(ProcessGeneration.PropertyAction, sourceColumn.ActionName)
                    };

                    // If parameters for drill down
                    if (sourceColumn.Params.Count > 0)
                    {
                        // Create array of parameters
                        var paramArray = new JArray();

                        // Iternate parameters
                        foreach (var param in sourceColumn.Params)
                        {
                            var paramObj = new JObject
                            {
                                new JProperty(ProcessGeneration.PropertyName, param.Value.Name)
                            };

                            paramArray.Add(paramObj);
                        }

                        drilldownProperties.Add(new JProperty(ProcessGeneration.PropertyParameters, paramArray));
                    }

                    fields.Add(new JProperty(ProcessGeneration.PropertyDrilldownUrl, drilldownProperties));
                }

                fields.Add(new JProperty(ProcessGeneration.PropertyIsGroupBy, sourceColumn.IsGroupBy));

                // Aggregation
                if (sourceColumn.Aggregation.Count > 0)
                {
                    var aggregateArray = new JArray();

                    foreach (var aggregationType in sourceColumn.Aggregation)
                    {
                        var aggregateObj = new JObject
                        {
                            new JProperty(aggregationType.Key, aggregationType.Value)
                        };

                        aggregateArray.Add(aggregateObj);
                    }

                    fields.Add(ProcessGeneration.PropertyAggregation, aggregateArray);
                }

                fieldsArray.Add(fields);
            }

            json.Add(new JProperty(ProcessGeneration.PropertyFields, fieldsArray));

            return json;
        }

        /// <summary> Generate Template JSON</summary>
        /// <param name="fileName">File name</param>
        private JObject GenerateTemplateJson(string fileName)
        {
            // Add properties
            var json = new JObject
            {
                new JProperty(ProcessGeneration.PropertyGeneratedMessage, Resources.GeneratedMessage),
                new JProperty(ProcessGeneration.PropertyGeneratedWarning, Resources.GeneratedWarning),
                new JProperty(ProcessGeneration.PropertyInquiryId, GetContent(Source.InquiryId)),
                new JProperty(ProcessGeneration.PropertyFileName, fileName),
                new JProperty(ProcessGeneration.PropertyName, GetContent(Source.Name)),
                new JProperty(ProcessGeneration.PropertyDescription, GetContent(Source.Description)),
            };

            // Create array of display fields
            var displayFieldsArray = new JArray();

            foreach (var sourceColumn in _source.SourceColumns.Values.OrderBy(column => column.DisplayOrder))
            {
                // Only add if it is included and is displayable
                if (sourceColumn.IsIncluded && sourceColumn.IsDisplayable)
                {
                    var displayField = new JObject();
                    displayField.Add(new JProperty(ProcessGeneration.PropertyDisplayField, sourceColumn.Name));
                    displayFieldsArray.Add(displayField);
                }
            }

            json.Add(new JProperty(ProcessGeneration.PropertyDisplayFields, displayFieldsArray));

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
        /// <param name="configurationFileName">Configuration file name</param>
        /// <param name="templateFileName">Template file name</param>
        private void ClearGenerateControls(string configurationFileName, string templateFileName)
        {
            lblGenerateJson.Text = string.Format(Resources.JsonToGenerateTip, configurationFileName, templateFileName);

            txtConfigurationToGenerate.Clear();
            txtTemplateToGenerate.Clear();
        }

        /// <summary> Build Configuration File Name </summary>
        /// <param name="name">Configuration name</param>
        /// <returns>File name {name}InquiryConfiguration.json</returns>
        private static string BuildConfigurationFileName(string name)
        {
            return name + ProcessGeneration.PropertyConfigurationFileNameSuffix;
        }

        /// <summary> Build Template File Name </summary>
        /// <param name="name">Template name</param>
        /// <returns>File name {name}InquiryTemplate.json</returns>
        private static string BuildTemplateFileName(string name)
        {
            return name + ProcessGeneration.PropertyTemplateFileNameSuffix;
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

            // Folder not specified
            if (string.IsNullOrEmpty(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Folder.Replace(":", ""));
            }

            // Folder not exists
            if (!Directory.Exists(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingDoesNotExist, Resources.Folder.Replace(":", ""));
            }

            // Name
            if (string.IsNullOrEmpty(txtInquiryName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.InquiryName.Replace(":", ""));
            }


            // Description
            if (string.IsNullOrEmpty(txtInquiryDescription.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.InquiryDescription.Replace(":", ""));
            }

            // User ID
            if (string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.User.Replace(":", ""));
            }

            // Password
            //if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            //{
            //    return string.Format(Resources.InvalidSettingRequiredField, Resources.Password.Replace(":", ""));
            //}

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

            // Session
            try
            {
                // Init session to see if credentials are valid
                var session = new Session();
                session.CreateSession(null, ProcessGeneration.PropertyAppId, ProcessGeneration.PropertyProgramName, 
                    txtVersion.Text.Trim(), txtUser.Text.Trim(), 
                    txtPassword.Text.Trim(), txtCompany.Text.Trim(), DateTime.UtcNow);
                session.Dispose();
            }
            catch
            {
                return Resources.InvalidSettingCredentials;
            }

            // The step is valid, so store information from this step into the source
            _source.Properties[Source.InquiryId] = txtInquiryId.Text.Trim();
            _source.Properties[Source.FolderName] = txtFolderName.Text.Trim();
            _source.Properties[Source.Name] = txtInquiryName.Text.Trim();
            _source.Properties[Source.Description] = txtInquiryDescription.Text.Trim();
            _source.Properties[Source.User] = txtUser.Text.Trim();
            _source.Properties[Source.Password] = txtPassword.Text.Trim();
            _source.Properties[Source.Version] = txtVersion.Text.Trim();
            _source.Properties[Source.Company] = txtCompany.Text.Trim();
            _source.Options[Source.IsBusinessView] = chkUseBusinessView.Checked;

            // Before proceeding to the next step, if an existing configuration is loaded
            // AND the configuration is using a business view, the loading of the remainder 
            // of the JSON has been delayed until this step is valid (credentials). Thus, 
            // finish loading the JSON
            LoadConfiguration();

            return string.Empty;
        }

        /// <summary> Valid Source Step - View</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidSourceStepView()
        {
            // Was a view specified?
            if (string.IsNullOrEmpty(txtViewID.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ViewId.Replace(":", ""));
            }

            // At least 1 row must be included 
            if (!_sourceColumns.Any(i => i.IsIncluded))
            {
                return Resources.InvalidSettingRequiredColumn;
            }

            // Delete rows first
            DeleteRows(grdIncludedColumns);

            // Need to add included rows for display order grid

            // Get Max display order in case adding newly unincluded rows
            var max = _sourceColumns.Max(i => i.DisplayOrder);
            var temp = new SortedList<int, SourceColumn>();

            // Iterate each column and assign based upon not included or included (and if already have a value)
            foreach (var sourceColumn in _sourceColumns)
            {
                // If not included, ensure display order is 0
                if (!sourceColumn.IsIncluded)
                {
                    sourceColumn.DisplayOrder = 0;
                    continue;
                }

                // It is included, assign display order UNLESS it is already assigned
                if (sourceColumn.DisplayOrder.Equals(0))
                {
                    sourceColumn.DisplayOrder = ++max;
                }

                // Add to temp sorted list
                temp.Add(sourceColumn.DisplayOrder, sourceColumn);
            }

            // All included columns are in the temp sort, but there may be gaps
            var displayOrder = 0;
            foreach (var sourceColumn in temp.Values)
            {
                // Increment for re-assignment if needed
                if (!sourceColumn.DisplayOrder.Equals(++displayOrder))
                {
                    // Assign proper number without gaps
                    sourceColumn.DisplayOrder = displayOrder;

                    // Update back to source list of all columns
                    _sourceColumns[_sourceColumns.IndexOf(sourceColumn)] = sourceColumn;
                }

                // Update source for included grid
                _includedColumns.Add(sourceColumn);
            }
            grdIncludedColumns.Refresh();

            // The step is valid, so store information from this step into the source
            // View and columns are already in the source! No action needed.

            return string.Empty;
        }

        /// <summary> Valid Source Step SQL</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidSourceStepSql()
        {
            // Was SQL specified?
            if (string.IsNullOrEmpty(txtSQL.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Sql.Replace(":", ""));
            }

            // SQL Columns
            if (_sourceSqlColumns.Count == 0)
            {
                return Resources.InvalidSettingRequiredColumns;
            }

            // SQL Columns have duplicate names
            var duplicates = _sourceSqlColumns.GroupBy(x => x.Name)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
            if (duplicates.Count > 0)
            {
                return Resources.InvalidSettingDuplicateCols;
            }

            // SQL Columns have invalid names
            var spaces = _sourceSqlColumns.Any(x => x.Name.Contains(" "));
            if (spaces)
            {
                return Resources.InvalidSettingColumnSpaces;
            }

            // Where clause
            if (txtWhereClause.Text.Trim().ToUpper().Contains(ProcessGeneration.PropertyWhere))
            {
                return Resources.InvalidSettingWhereClause;
            }

            // Order by clause
            if (string.IsNullOrEmpty(txtOrderByClause.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.OrderByClause.Replace(":", ""));
            }

            if (txtOrderByClause.Text.Trim().ToUpper().Contains(ProcessGeneration.PropertyOrderBy))
            {
                return Resources.InvalidSettingOrderByClause;
            }

            // Order by clause has to have valid columns found in sql columns
            var orderbyCols = txtOrderByClause.Text.Trim().Split(',');
            foreach (var colName in orderbyCols)
            {
                // Look up column in sql columns
                var found = _sourceSqlColumns.Any(x => x.Name.Equals(colName));
                if (!found)
                {
                    return string.Format(Resources.InvalidSettingOrderByCol, colName, Resources.OrderByClause.Replace(":", ""));
                }
            }

            // Delete included rows first
            DeleteRows(grdIncludedColumns);

            // Add/Delete/Update all columns based upon columns in sql columns grid
            var displayOrder = 0;
            foreach (var sourceColumn in _sourceSqlColumns)
            {
                // Assign proper display order
                sourceColumn.DisplayOrder = ++displayOrder;
                sourceColumn.Id = displayOrder;
                sourceColumn.IsIncluded = true;

                // If already in all columns, update it
                if (_source.SourceColumns.ContainsKey(sourceColumn.Name))
                {
                    _source.SourceColumns[sourceColumn.Name] = sourceColumn;
                    _sourceColumns[_sourceColumns.IndexOf(sourceColumn)] = sourceColumn;
                }
                else
                {
                    // Add it since it does not exist
                    AddCaptions(sourceColumn);
                    _source.SourceColumns.Add(sourceColumn.Name, sourceColumn);
                    _sourceColumns.Add(sourceColumn);
                }

                // Add to included columns
                _includedColumns.Add(sourceColumn);
            }

            // Need to determine if any previously added all columns are not in the SQL columns
            var toBeRemoved = new List<SourceColumn>();
            foreach (var sourceColumn in _sourceColumns)
            {
                // Remove a column if it does not exist in the source SQL columns
                var exists = _sourceSqlColumns.ToList().Any(t => t.Name.Equals(sourceColumn.Name));
                if (!exists)
                {
                    // To be removed outside of iteration
                    toBeRemoved.Add(sourceColumn);
                }
            }
            // Anything to be removed?
            if (toBeRemoved.Count > 0)
            {
                foreach (var sourceColumn in toBeRemoved)
                {
                    _sourceColumns.Remove(sourceColumn);
                }
            }

            grdIncludedColumns.Refresh();

            // The step is valid, so store information from this step into the source
            _source.Properties[Source.SqlStatement] = txtSQL.Text.Trim();
            _source.Properties[Source.WhereClause] = txtWhereClause.Text.Trim();
            _source.Properties[Source.OrderByClause] = txtOrderByClause.Text.Trim();

            return string.Empty;
        }

        /// <summary> Valid Columns Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidColumnsStep()
        {
            // Iterate the included columns
            foreach (var includedColumn in _includedColumns)
            {
                var success = ValidColumn(includedColumn.Type, includedColumn.Captions.Values.ToList(), includedColumn.IsDrilldown,
                    includedColumn.AreaName, includedColumn.ControllerName, includedColumn.ActionName,
                    includedColumn.IsColumnInView, includedColumn.ViewId, includedColumn.ViewColumnName,
                    includedColumn.Filters.Values.ToList());
                if (!string.IsNullOrEmpty(success))
                {
                    return success;
                }
            }

            // The step is valid, so store information from this step into the source
            // View and columns are already in the source! No action needed.

            return string.Empty;

        }

        /// <summary> Valid Column </summary>
        /// <param name="dataType">Data Type of column </param>
        /// <param name="captions">Captions of column </param>
        /// <param name="isDrilldown">True if drilldonw otherwise false </param>
        /// <param name="areaName">Area name for drilldown </param>
        /// <param name="controllerName">Controller name for drilldown </param>
        /// <param name="actionName">Action name for drilldown </param>
        /// <param name="isColumnInView">True if column is in view otherwise false </param>
        /// <param name="viewId">View Id for column in view </param>
        /// <param name="columnInView">Column in view </param>
        /// <param name="filters">Filters of column </param>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidColumn(SourceDataType dataType, List<Caption> captions, bool isDrilldown,
            string areaName, string controllerName, string actionName, bool isColumnInView,
            string viewId, string columnInView, List<Filter> filters)
        {
            // Data Type
            if (dataType.Equals(SourceDataType.None))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.DataType.Replace(":", ""));
            }

            // Ensure that at least the source language is not null in captions
            if (captions.Any(x => x.Language.Equals(_source.Language) && string.IsNullOrEmpty(x.Text)))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Caption);
            }

            // If drilldown...
            if (isDrilldown)
            {
                // Area
                if (string.IsNullOrEmpty(areaName))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Area.Replace(":", ""));
                }

                // Controller
                if (string.IsNullOrEmpty(controllerName))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Controller.Replace(":", ""));
                }

                // Action
                if (string.IsNullOrEmpty(actionName))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Action.Replace(":", ""));
                }

                // No validation on parameters needed

            }

            // Columnin View?
            if (isColumnInView)
            {
                // View Id
                if (string.IsNullOrEmpty(viewId))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.ViewId.Replace(":", ""));
                }

                // Column Name
                if (string.IsNullOrEmpty(columnInView))
                {
                    return string.Format(Resources.InvalidSettingRequiredField, Resources.Column.Replace(":", ""));
                }

            }

            // Filters if specified, must have text and value filled out
            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.Text) || string.IsNullOrEmpty(filter.Value))
                {
                    return Resources.InvalidSettingFilter;
                }
            }

            return string.Empty;
        }

        /// <summary> Load configuration</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string LoadConfiguration()
        {
            // Exit if no JSON to load
            if (_json == null)
            {
                return string.Empty;
            }

            // Flag for if a business view or not
            var isBusinessView = _source.Options[Source.IsBusinessView];

            try
            {
                // Get the view and then update the columns with information from JSON else
                // if SQL then the data will be loaded from the JSON
                if (isBusinessView)
                {
                    GetSource();
                }

                // Read JSON to get the Fields
                var fields =
                    from field in _json[ProcessGeneration.PropertyFields]
                    select new
                    {
                        FieldName = (string)field[ProcessGeneration.PropertyField],
                        FieldIndex = (string)field[ProcessGeneration.PropertyFieldIndex],
                        Captions = (JArray)field[ProcessGeneration.PropertyCaptions],
                        IsSQLColumnInView = (bool)field[ProcessGeneration.PropertyIsColumnInView],
                        ViewName = (string)field[ProcessGeneration.PropertyViewName],
                        ColumnNameInView = (string)field[ProcessGeneration.PropertyColumnNameInView],
                        DataTypeEnumeration = (string)field[ProcessGeneration.PropertyDataTypeEnumeration],
                        // DataType = (string)field[ProcessGeneration.PropertyDataType],
                        Filters = (JArray)field[ProcessGeneration.PropertyFilters],
                        IsDisplayable = (bool)field[ProcessGeneration.PropertyIsDisplayable],
                        IsFilterable = (bool)field[ProcessGeneration.PropertyIsFilterable],
                        IsDrilldown = (bool)field[ProcessGeneration.PropertyIsDrilldown],
                        IsGroupBy = (bool)field[ProcessGeneration.PropertyIsGroupBy],
                        DrillDownUrl = (JObject)field[ProcessGeneration.PropertyDrilldownUrl]
                    };

                // Iterate fields in JSON and update columns in source
                var displayOrder = 0;

                foreach (var field in fields)
                {
                    SourceColumn sourceColumn;

                    // If a business view, get column from business view first
                    if (isBusinessView)
                    {
                        sourceColumn = _source.SourceColumns[field.FieldName];
                    }
                    else
                    {
                        // If not a business view, need to new one first
                        sourceColumn = new SourceColumn
                        {
                            Id = Convert.ToInt32(field.FieldIndex),
                            Name = field.FieldName
                        };
                        _source.SourceColumns.Add(sourceColumn.Name, sourceColumn);
                    }

                    // Update display order (order in JSON) and included (if in JSON it is included)
                    sourceColumn.DisplayOrder = ++displayOrder;
                    sourceColumn.IsIncluded = true;

                    // Update based upon JSON content
                    sourceColumn.Type = (SourceDataType)Enum.Parse(typeof(SourceDataType), field.DataTypeEnumeration);
                    sourceColumn.IsDisplayable = field.IsDisplayable;
                    sourceColumn.IsFilterable = field.IsFilterable;
                    sourceColumn.IsColumnInView = field.IsSQLColumnInView;
                    sourceColumn.ViewId = field.ViewName;
                    sourceColumn.ViewColumnName = field.ColumnNameInView;

                    // Captions
                    sourceColumn.Captions.Clear();
                    var captions =
                        from caption in field.Captions
                        select new
                        {
                            Language = (string)caption[ProcessGeneration.PropertyLanguage],
                            Value = (string)caption[ProcessGeneration.PropertyValue]
                        };
                    foreach (var caption in captions)
                    {
                        sourceColumn.Captions.Add(caption.Language, new Caption() { Language = caption.Language, Text = caption.Value });
                    }

                    // Drilldown
                    sourceColumn.IsDrilldown = field.IsDrilldown;
                    if (field.DrillDownUrl != null)
                    {
                        var tokenPath = field.DrillDownUrl.Path;
                        sourceColumn.AreaName = (string)_json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyArea);
                        sourceColumn.ControllerName = (string)_json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyController);
                        sourceColumn.ActionName = (string)_json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyAction);

                        // Parameters
                        var parameters = (JArray)_json.SelectToken(tokenPath + "." + ProcessGeneration.PropertyParameters);
                        if (parameters != null)
                        {
                            sourceColumn.Params.Clear();
                            var parms =
                                from param in parameters
                                select new
                                {
                                    Name = Text = (string)param[ProcessGeneration.PropertyName]
                                };
                            foreach (var param in parms)
                            {
                                sourceColumn.Params.Add(param.Name, new Parameter() { Name = param.Name });
                            }
                        }
                    }

                    // GroupBy
                    sourceColumn.IsGroupBy = field.IsGroupBy;

                    // Filters
                    sourceColumn.Filters.Clear();
                    if (field.Filters != null)
                    {
                        var filters =
                            from filter in field.Filters
                            select new
                            {
                                Text = (string)filter[ProcessGeneration.PropertyText],
                                Value = (string)filter[ProcessGeneration.PropertyValue]
                            };
                        foreach (var filter in filters)
                        {
                            sourceColumn.Filters.Add(filter.Value, new Filter() { Value = filter.Value, Text = filter.Text });
                        }
                    }

                }

                // Clear _json
                _json = null;

            }
            catch
            {
                return Resources.InvalidSettingJsonFile;
            }

            return string.Empty;
        }

        /// <summary> Get Source Business View </summary>
        private void GetSource()
        {
            // Init properties
            _source.Properties[Source.ViewId] = txtViewID.Text.Trim();

            ProcessGeneration.GetSource(_source);

            // Clear and assign
            DeleteRows(grdSourceColumns);
            foreach (var sourceColumn in _source.SourceColumns.Values)
            {
                _sourceColumns.Add(sourceColumn);

                // Assign captions
                AddCaptions(sourceColumn);
            }
        }

        /// <summary> Validate the view id and convert it to a source </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disable next button</remarks>
        private void txtViewID_Leave(object sender, EventArgs e)
        {
            var errorCondition = false;

            try
            {
                // Someting to validate
                if (!string.IsNullOrEmpty(txtViewID.Text))
                {
                    // Do not validate if already the same view 
                    if (_source.Properties.ContainsValue(txtViewID.Text.Trim()))
                    {
                        return;
                    }

                    GetSource();
                }
            }

            catch (Exception ex)
            {
                // Error received attempting to get view
                DisplayMessage((ex.InnerException == null) ? ex.Message : ex.InnerException.Message, MessageBoxIcon.Error);
                errorCondition = true;
            }

            // Send back to control?
            if (!errorCondition)
            {
                return;
            }

            // Clear field and send back to control
            txtViewID.Text = string.Empty;
            _source.Properties[Source.ViewId] = string.Empty;
            txtViewID.Focus();
        }

        /// <summary> Enable and disable based upon selection</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void chkUseBusinessView_CheckedChanged(object sender, EventArgs e)
        {
            var selected = chkUseBusinessView.Checked;

            // remove step first
            _wizardSteps.RemoveAt(1);

            // Step specific to View or Sql
            if (chkUseBusinessView.Checked)
            {
                AddStep(Resources.StepTitleSource, Resources.StepDescriptionSourceView, pnlSourceView, txtViewID, 1);
            }
            else
            {
                AddStep(Resources.StepTitleSource, Resources.StepDescriptionSourceSql, pnlSourceSql, txtSQL, 1);
            }

        }

        /// <summary> Enable/Disable Drilldown</summary>
        /// <param name="selected">True if selected othwise false </param>
        private void EnableDrilldown(bool selected)
        {
            txtArea.Enabled = selected;
            txtController.Enabled = selected;
            txtAction.Enabled = selected;
            grdParameters.Enabled = selected;
        }

        /// <summary> Enable and disable based upon selection</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void chkDrilldown_CheckedChanged(object sender, EventArgs e)
        {
            EnableDrilldown(chkDrilldown.Checked);
        }

        /// <summary> Enable/Disable ColumnInView</summary>
        /// <param name="selected">True if selected othwise false </param>
        private void EnableColumnInView(bool selected)
        {
            txtFilterViewId.Enabled = selected;
            cboFilterColumn.Enabled = selected;
        }

        /// <summary> Enable/Disable Filters</summary>
        /// <param name="selected">True if selected othwise false </param>
        private void EnableFilters(bool selected)
        {
            grdFilters.Enabled = selected;

            // If enabled, set the grid to allow add and edit else set to not allow these
            grdFilters.AllowUserToAddRows = selected;
            grdFilters.AllowUserToDeleteRows = selected;
        }

        /// <summary> Enable and disable based upon selection</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void chkColumnInView_CheckedChanged(object sender, EventArgs e)
        {
            var dataType = (SourceDataType)Enum.Parse(typeof(SourceDataType), txtDataType.Text.Trim());

            EnableColumnInView(chkColumnInView.Checked);
            EnableFilters(!chkColumnInView.Checked && dataType.Equals(SourceDataType.Enumeration));

            // If selected AND is not business view, then clear rows
            if (chkColumnInView.Checked && chkUseBusinessView.Checked)
            {
                DeleteRows(grdFilters);
            }
            else if (!chkColumnInView.Checked)
            {
                // In case it was already specified, need to clear
                txtFilterViewId.Text = string.Empty;
                cboFilterColumn.Items.Clear();
            }
        }

        /// <summary> Drag/Drop for included columns</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_MouseDown(object sender, MouseEventArgs e)
        {

            // Get the index of the item below the mouse
            _rowIndexFromMouseDown = grdIncludedColumns.HitTest(e.X, e.Y).RowIndex;

            if (_rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                var dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(
                          new Point(
                            e.X - (dragSize.Width / 2),
                            e.Y - (dragSize.Height / 2)),
                      dragSize);
            }
            else
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        /// <summary> Drag/Drop for included columns</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_MouseMove(object sender, MouseEventArgs e)
        {
            // Only on left mouse 
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag
                if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the item                    
                    var dropEffect = grdIncludedColumns.DoDragDrop(grdIncludedColumns.Rows[_rowIndexFromMouseDown], 
                        DragDropEffects.Move);
                }
            }
        }

        /// <summary> Drag/Drop for included columns</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary> Drag/Drop for included columns</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_DragDrop(object sender, DragEventArgs e)
        {

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            var clientPoint = grdIncludedColumns.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item below the mouse
            _rowIndexOfItemUnderMouseToDrop = grdIncludedColumns.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                var removeAtIndex = ((DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow))).Index;
                var columnToMove = _includedColumns[removeAtIndex];

                _includedColumns.RemoveAt(removeAtIndex);
                _includedColumns.Insert(_rowIndexOfItemUnderMouseToDrop, columnToMove);

                // Reset Display Order 
                var displayOrder = 0;
                foreach (var includedColumn in _includedColumns)
                {
                    includedColumn.DisplayOrder = ++displayOrder;
                }
            }
        }

        /// <summary> Edit an included column</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Get selected row
            var includedColumn = (SourceColumn)((DataGridView)sender).CurrentRow.DataBoundItem;

            if (includedColumn != null)
            {
                // Setup column for editing
                _clickedColumn = includedColumn;
                ColumnSetup(ModeType.Edit);
            }

        }

        /// <summary> No whitespaces allows</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtInquiryName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        /// <summary> Edit a column</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdIncludedColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary> Cancel a column edit</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelColumn();
        }

        /// <summary> Save a column edit</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveColumn();
        }

        /// <summary> Validate the view id and convert it to a source </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtFilterViewId_Leave(object sender, EventArgs e)
        {
            var errorCondition = false;

            try
            {
                // Something to validate
                if (!string.IsNullOrEmpty(txtFilterViewId.Text))
                {
                    // Do not validate if already the same view 
                    if (txtFilterViewId.Tag != null && txtFilterViewId.Tag.ToString().Equals(txtFilterViewId.Text.Trim()))
                    {
                        return;
                    }

                    // Init properties
                    _viewSource.Properties.Clear();
                    _viewSource.Properties[Source.ViewId] = txtFilterViewId.Text.Trim();
                    _viewSource.Properties[Source.User] = txtUser.Text.Trim();
                    _viewSource.Properties[Source.Password] = txtPassword.Text.Trim();
                    _viewSource.Properties[Source.Version] = txtVersion.Text.Trim();
                    _viewSource.Properties[Source.Company] = txtCompany.Text.Trim();

                    ProcessGeneration.GetSource(_viewSource);

                    // Clear and assign
                    cboFilterColumn.Items.Clear();
                    foreach (var filterColumn in _viewSource.SourceColumns.Values)
                    {
                        cboFilterColumn.Items.Add(filterColumn.Name);
                    }

                }
            }

            catch (Exception ex)
            {
                // Error received attempting to get view
                DisplayMessage((ex.InnerException == null) ? ex.Message : ex.InnerException.Message, MessageBoxIcon.Error);
                errorCondition = true;
            }

            // Send back to control?
            if (!errorCondition)
            {
                return;
            }

            // Clear field and send back to control
            txtFilterViewId.Text = string.Empty;
            cboFilterColumn.Items.Clear();
            txtFilterViewId.Focus();
        }

        /// <summary> Add filters if any </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboFilterColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Does the column have a list?
            // Clear and assign
            _filters.Clear();

            var column = (SourceColumn)_viewSource.SourceColumns[cboFilterColumn.Text];
            if (column != null && column.Filters.Values.Count > 0)
            {
                foreach (var filter in column.Filters.Values)
                {
                    _filters.Add(filter);
                }
            }

        }

        /// <summary> Show SQL Help modally </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSqlHelp_Click(object sender, EventArgs e)
        {
            var help = new SqlHelp().ShowDialog();
        }

        #endregion
    }
}
