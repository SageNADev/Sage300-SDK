// The MIT License (MIT) 
// Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard.Properties;
using System.IO;
using System.Xml;
using MetroFramework.Forms;
using ACCPAC.Advantage;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard
{
    /// <summary> UI for View Field Attributes Wizard </summary>
    public partial class Generation : MetroForm
    {
        #region Private Variables

        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> Information processed </summary>
        private readonly BindingList<Info> _gridInfo = new BindingList<Info>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        #endregion

        #region Private Constants
        private class Constants
        {
            /// <summary> Version Default </summary>
            public const string VersionDefault = "71A";

            /// <summary> Compile element in csproj files </summary>
            public const string CompileElement = "Compile";

            /// <summary> Panel Name for pnlWizardSummary </summary>
            public const string PanelWizardSummary = "pnlWizardSummary";

            /// <summary> Panel Name for pnlFolderCreds </summary>
            public const string PanelFolderCreds = "pnlFolderCreds";

            /// <summary> Panel Name for pnlGenerated </summary>
            public const string PanelGenerated = "pnlGenerated";

            /// <summary> Panel Name for pnlGenerate </summary>
            public const string PanelGenerate = "pnlGenerate";

            /// <summary> Splitter Distance </summary>
            public const int SplitterDistance = 415;
        }
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
            InitEvents();
            ProcessingSetup(true);
            Processing("");
        }

        #endregion

        #region Private Routines

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
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.ProcessingFile, text);

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
        private void InitPanel(Panel panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.ViewFieldAttributesWizard;

            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

            // Step Wizard Summary
            lblWizardSummary.Text = Resources.WizardSummary;

            // Step Folder & Credentials
            lblFolder.Text = Resources.Folder;
            tooltip.SetToolTip(lblFolder, Resources.FolderNameTip);

            lblUser.Text = Resources.User;
            tooltip.SetToolTip(lblUser, Resources.UserTip);

            lblPassword.Text = Resources.Password;
            tooltip.SetToolTip(lblPassword, Resources.PasswordTip);

            lblVersion.Text = Resources.Version;
            tooltip.SetToolTip(lblVersion, Resources.VersionTip);
            txtVersion.Text = Constants.VersionDefault;

            lblCompany.Text = Resources.Company;
            tooltip.SetToolTip(lblCompany, Resources.CompanyTip);

            // Step Generated
            lblGenerated.Text = Resources.Generated;
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
            InitPanel(pnlWizardSummary);
            InitPanel(pnlFolderCreds);
            InitPanel(pnlGenerate);
            InitPanel(pnlGenerated);

            // Assign steps for wizard
            AddStep(Resources.StepTitleWizardSummary, Resources.StepDescriptionWizardSummary, pnlWizardSummary);
            AddStep(Resources.StepTitleFolderCreds, Resources.StepDescriptionFolderCreds, pnlFolderCreds);
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
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelGenerated))
            {
                Close();
            }
            else
            {
                // Proceed to next wizard step or start modification if last step
                if (!_currentWizardStep.Equals(-1) &&
                    _wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelGenerate))
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
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelGenerate))
                    {
                        // Load controls based upon screens in the previous steps
                        txtFilesToModify.Clear();

                        // Generate list of files
                        var files = GenerateFiles();
                        lblGenerateFiles.Text = string.Format(Resources.FilesToModifyTip, files.Count);
                        txtFilesToModify.Text = string.Join(Environment.NewLine, files);

                        // Establish settings for processing (Validation already ocurred in each step)
                        _settings = new Settings
                        {
                            FolderName = txtFolderName.Text.Trim(),
                            UserName = txtUser.Text.Trim(),
                            UserKey = txtPassword.Text.Trim(),
                            Version = txtVersion.Text.Trim(),
                            CompanyId = txtCompany.Text.Trim(),
                            Files = files
                        };
                    }

                    ShowStep(true);

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelGenerate))
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
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelGenerated))
                {
                    btnBack.Text = Resources.Back;
                    btnNext.Text = Resources.Next;
                    _currentWizardStep = 0;
                    txtFolderName.Focus();
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

            // Wizard Summary Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelWizardSummary))
            {
                valid = ValidWizardSummaryStep();
            }

            // FolderCreds Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(Constants.PanelFolderCreds))
            {
                valid = ValidFolderCredsStep();
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
            splitSteps.SplitterDistance = Constants.SplitterDistance;
        }

        /// <summary> Show Step Title</summary>
        private void ShowStepTitle()
        {
            //// Update title and text for step
            var currentStep = (_currentWizardStep + 1).ToString("#0");
            var stepText = (_currentWizardStep + 1).Equals(0)
                            ? string.Empty
                            : $"{Resources.Step} {currentStep} {Resources.Dash} ";

            lblStepTitle.Text = stepText + _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
        }

        /// <summary> Folder search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnFolder_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Display manifest selected
            txtFolderName.Text = dialog.SelectedPath;
        }

        /// <summary> Generate list from files specified in the *.Models.csproj project files</summary>
        /// <returns>List of model files</returns>
        private List<string> GenerateFiles()
        {
            var files = new List<string>();

            // Get projects specified by the pattern "*.Models.csproj"
            var projects = Directory.GetFiles(txtFolderName.Text.Trim(), "*.Models.csproj", SearchOption.AllDirectories);

            // Iterate projects to get list of model files
            foreach (var project in projects)
            {
                files.AddRange(ParseXml(project));
            }

            return files;
        }

        /// <summary> Parse the xml file and return list of model files</summary>
        /// <param name="fileName">Project file name</param>
        /// <returns>List of file names</returns>
        private List<string> ParseXml(String fileName)
        {
            var files = new List<string>();
            var path = Path.GetDirectoryName(fileName);

            // Open file (csproj)
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            // Get all elements of element type Compile
            var elementList = xmlDocument.GetElementsByTagName(Constants.CompileElement);

            // Iterate list and filter out reports, enums, non-cs files, etc.
            for (int i = 0; i < elementList.Count; i++)
            {
                var file = elementList[i].Attributes["Include"].Value;

                // Skip non *.cs files
                if (!file.EndsWith(".cs"))
                {
                    continue;
                }
                // Skip Enums, Fields, Properties, Reports folders
                if (file.StartsWith(@"Enums\") || file.StartsWith(@"Fields\") || 
                    file.StartsWith(@"Properties\") || file.StartsWith(@"Reports\"))
                {
                    continue;
                }

                // All other files to be included
                files.Add(Path.Combine(path, file));
            }

            return files;
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
                grid.Columns[column].DefaultCellStyle.BackColor = SystemColors.Window;
            }
        }

        /// <summary> Valid Wizard Summaryt Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidWizardSummaryStep()
        {
            return string.Empty;
        }

        /// <summary> Valid Folder Credentials Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidFolderCredsStep()
        {
            var sessionValid = string.Empty;

            // Folder
            if (string.IsNullOrEmpty(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Folder.Replace(":", ""));
            }

            // Folder does not exist
            if (!Directory.Exists(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingFolderExists, txtFolderName.Text.Trim());
            }

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

            // Init session to see if credentials are valid
            try
            {
                var session = new Session();
                session.InitEx2(null, string.Empty, "WX", "WX1000", txtVersion.Text.Trim(), 1);
                session.Open(txtUser.Text.Trim(), txtPassword.Text.Trim(), txtCompany.Text.Trim(), DateTime.UtcNow, 0);
            }
            catch (Exception ex)
            {
                sessionValid = Resources.InvalidSettingCredentials;
            }

            return sessionValid;
        }
        #endregion
    }
}
