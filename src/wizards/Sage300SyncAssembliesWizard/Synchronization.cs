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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.SyncAssembliesWizard
{
    /// <summary> UI for Sync Assemblies Wizard </summary>
    public partial class Synchronization : Form
    {
        #region Private Vars

        /// <summary> Process Synchronization logic </summary>
        private ProcessSynchronization _synchronization;

        /// <summary> Information processed </summary>
        private readonly List<Info> _gridInfo = new List<Info>();

        /// <summary> Assembly Information </summary>
        private BindingList<AssemblyInfo> _assemblies = new BindingList<AssemblyInfo>();

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Sage color </summary>
        private readonly Color _sageColor = SystemColors.HotTrack;

        #endregion

        #region Delegates

        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        private delegate void ProcessingCallback(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        private delegate void StatusCallback(string fileName);

        #endregion

        #region Constructor

        /// <summary> Synchronization Class </summary>
        /// <param name="destination">Destination Default</param>
        /// <param name="destinationWeb">Destination Web Default</param>
        public Synchronization(string destination, string destinationWeb)
        {
            InitializeComponent();
            InitWizardSteps(destination, destinationWeb);
            InitAssemblyFields();
            InitEvents();
            ProcessingSetup(true);
            Processing("");

            cboSourceType.Focus();
        }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods/Routines/Events
        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            _synchronization = new ProcessSynchronization();
            _synchronization.ProcessingEvent += ProcessingEvent;
            _synchronization.StatusEvent += StatusEvent;

            // Default to Local Installation
            cboSourceType.SelectedIndex = Convert.ToInt32(SourceType.Local);
        }

        #region Toolbar Events

        /// <summary> Next/Sync toolbar button </summary>
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

        /// <summary> Next/Sync Navigation </summary>
        /// <remarks>Next wizard step or Sync if last step</remarks>
        private void NextStep()
        {
            // Finished?
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlSyncedAssemblies"))
            {
                _synchronization.Dispose();
                Close();
            }
            else
            {
                // Proceed to next wizard step or start synchronization if last step
                if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlSyncAssemblies"))
                {
                    // Build settings and validate that settings have been selected 
                    var settings = BuildSettings();

                    // Prompt if Inital Sync
                    if (settings.InitialSync)
                    {
                        if (DisplayQuestion(Resources.InitialSync, MessageBoxIcon.Question).Equals(DialogResult.Cancel))
                        {
                            return;
                        }
                    }

                    var invalidSetting = ValidSettings(settings);
                    if (string.IsNullOrEmpty(invalidSetting))
                    {
                        // Setup display before processing
                        _gridInfo.Clear();
                        ProcessingSetup(false);

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

                    _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.Fill;
                    _wizardSteps[_currentWizardStep].Panel.Visible = true;

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlSyncAssemblies"))
                    {
                        btnNext.Text = Resources.Sync;
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
            // Proceed to next wizard step or start synchronization if last step
            if (!_currentWizardStep.Equals(0))
            {
                // Proceed back a step
                btnNext.Text = _wizardSteps[_currentWizardStep].Panel.Name.Equals("pnlSyncedAssemblies") ? Resources.Sync : Resources.Next;

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
        /// <param name="destination">Destination Default</param>
        /// <param name="destinationWeb">Destination Web Default</param>
        private void InitWizardSteps(string destination, string destinationWeb)
        {
            // Default
            btnNext.Text = Resources.Next;
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlSourceType);
            InitPanel(pnlDestination);
            InitPanel(pnlAssemblies);
            InitPanel(pnlSyncAssemblies);
            InitPanel(pnlSyncedAssemblies);

            // Test color for helpful labels
            lblSourceTypeDescriptionHelp.ForeColor = _sageColor;
            lblIncludePDBFilesHelp.ForeColor = _sageColor;
            lblSourceTypeDescriptionHelp.ForeColor = _sageColor;

            lblDestinationHelp.ForeColor = _sageColor;
            lblDestinationWebHelp.ForeColor = _sageColor;

            lblSyncAssembliesHelp.ForeColor = _sageColor;

            txtDestination.Text = destination;
            txtDestinationWeb.Text = destinationWeb;

            // Add steps
            AddStep(Resources.StepTitleSourceType, Resources.StepDescriptionSourceType, pnlSourceType);
            AddStep(Resources.StepTitleDestination, Resources.StepDescriptionDestination, pnlDestination);
            AddStep(Resources.StepTitleAssemblies, Resources.StepDescriptionAssemblies, pnlAssemblies);
            AddStep(Resources.StepTitleSyncAssemblies, Resources.StepDescriptionSyncAssemblies, pnlSyncAssemblies);
            AddStep(Resources.StepTitleSyncedAssemblies, Resources.StepDescriptionSyncedAssemblies, pnlSyncedAssemblies);

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

        /// <summary> Initialize assembly info and modify grid display </summary>
        private void InitAssemblyFields()
        {
            // Load Assembly Patterns
            LoadAssemblyPattern("AP");
            LoadAssemblyPattern("AR");
            LoadAssemblyPattern("AS");
            LoadAssemblyPattern("Common");
            LoadAssemblyPattern("Core");
            LoadAssemblyPattern("CS");
            LoadAssemblyPattern("GL");
            LoadAssemblyPattern("IC");
            LoadAssemblyPattern("KN");
            LoadAssemblyPattern("KPI");
            LoadAssemblyPattern("MT");
            LoadAssemblyPattern("OE");
            LoadAssemblyPattern("PO");
            LoadAssemblyPattern("Shared");
            LoadAssemblyPattern("TW");
            LoadAssemblyPattern("TM");
            LoadAssemblyPattern("TS");
            LoadAssemblyPattern("PM");
            LoadAssemblyPattern("VPF");
            LoadAssemblyPattern("Web");
            LoadAssemblyPattern("Workflow");
            LoadAssemblyPattern("CP");
            LoadAssemblyPattern("PR");
            LoadAssemblyPattern("UP");

            // Assign binding to datasource (two binding)
            grdAssemblies.DataSource = _assemblies;

            // Assign widths and localized text
            GenericInit(grdAssemblies, 0, 100, Resources.Include, true, false);
            GenericInit(grdAssemblies, 1, 100, Resources.Pattern, true, true);
            GenericInit(grdAssemblies, 2, 100, Resources.Override, true, false);
        }

        /// <summary> LoadAssemblyPattern </summary>
        /// <param name="pattern">Assembly Pattern</param>
        private void LoadAssemblyPattern(string pattern)
        {
            // Load Assembly Patterns
            _assemblies.Add(new AssemblyInfo()
            {
                IsIncluded = false,
                AssemblyPattern = pattern,
                Override = true
            });
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
            pnlButtons.Enabled = enableToolbar;
            pnlButtons.Refresh();
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.SynchronizingFile, text);
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
        /// <remarks>Invoked from threaded process</remarks>
        private void Status(string fileName)
        {
            // Add to info list
            _gridInfo.Add(new Info() { FileName = fileName });
        }

        /// <summary> Update status display </summary>
        /// <param name="fileName">File Name</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void StatusEvent(string fileName)
        {
            var callBack = new StatusCallback(Status);
            Invoke(callBack, fileName);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private DialogResult DisplayQuestion(string message, MessageBoxIcon icon, params object[] args)
        {
            return MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OKCancel, icon);
        }

        /// <summary> Build settings for background worker </summary>
        /// <returns>Settings</returns>
        private Settings BuildSettings()
        {
            // Ensure Assemblies grid is complete
            grdAssemblies.EndEdit(DataGridViewDataErrorContexts.Commit);
            grdAssemblies.Update();
            grdAssemblies.Refresh();

            return new Settings
            {
                SourceType = (SourceType) Enum.Parse(typeof (SourceType), cboSourceType.SelectedIndex.ToString()),
                IncludePdbFiles = chkIncludePDBFiles.Checked,
                Destination = txtDestination.Text.Trim(),
                Assemblies = _assemblies,
                InitialSync = btnInitialSync.Checked,
                DestinationWeb = txtDestinationWeb.Text.Trim()
            };
        }

        /// <summary> Validate the settings </summary>
        /// <param name="settings">Settings</param>
        /// <returns>Empty if valid otherwise message</returns>
        private string ValidSettings(Settings settings)
        {
            return _synchronization.ValidSettings(settings);
        }


        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            _synchronization.Process((Settings)e.Argument);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblCompleted.Text = string.Format(Resources.Completed, _gridInfo.Count);

            ProcessingSetup(true);
            Processing("");

            try
            {
                _synchronization.Dispose();
            }
            catch { }

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

        /// <summary> Destination search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDestinationDialog_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new FolderBrowserDialog();

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtDestination.Text = dialog.SelectedPath;
        }

        /// <summary> Destination Web search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDestinationWebDialog_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new FolderBrowserDialog();

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtDestinationWeb.Text = dialog.SelectedPath;
        }

        /// <summary> Help Button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disabled help until DPP wiki is available</remarks>
        private void Generation_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            // Display wiki link
            System.Diagnostics.Process.Start(Resources.Browser, Resources.WikiLink);
        }

        /// <summary> Include All</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>True will include all otherwise include none</remarks>
        private void btnIncludeAll_Click(object sender, EventArgs e)
        {
            grdAssemblies.EndEdit(DataGridViewDataErrorContexts.Commit);
            grdAssemblies.Update();
            
            // Turn all on or off depending upon state
            foreach (var assembly in _assemblies)
            {
                assembly.IsIncluded = btnIncludeAll.Checked;
            }
            grdAssemblies.DataSource = _assemblies;
            grdAssemblies.Refresh();
        }

        /// <summary> Override All</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>True will override all otherwise include none</remarks>
        private void btnOverrideAll_Click(object sender, EventArgs e)
        {
            grdAssemblies.EndEdit(DataGridViewDataErrorContexts.Commit);
            grdAssemblies.Update();

            // Turn all on or off depending upon state
            foreach (var assembly in _assemblies)
            {
                assembly.Override = btnOverrideAll.Checked;
            }
            grdAssemblies.DataSource = _assemblies;
            grdAssemblies.Refresh();
        }

        /// <summary> Override All</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>True will override all otherwise include none</remarks>
        private void btnInitialSync_Click(object sender, EventArgs e)
        {
            grdAssemblies.EndEdit(DataGridViewDataErrorContexts.Commit);
            grdAssemblies.Update();

            if (btnInitialSync.Checked)
            {
                // Set these to true
                btnIncludeAll.Checked = true;
                btnOverrideAll.Checked = true;
                tbrAssemblies.Refresh();

                // Turn all on or off depending upon state
                foreach (var assembly in _assemblies)
                {
                    if (btnInitialSync.Checked)
                    {
                        assembly.IsIncluded = btnIncludeAll.Checked;
                        assembly.Override = btnOverrideAll.Checked;
                    }
                }
            }

            grdAssemblies.DataSource = _assemblies;
            grdAssemblies.Refresh();

            // Enable/disable
            grdAssemblies.Enabled = !btnInitialSync.Checked;
            btnIncludeAll.Enabled = !btnInitialSync.Checked;
            btnOverrideAll.Enabled = !btnInitialSync.Checked;
        }



        #endregion

    }
}
