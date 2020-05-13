// The MIT License (MIT) 
// Copyright (c) 1994-2020 The Sage Group plc or its licensors.  All rights reserved.
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
using EnvDTE80;
using Sage.CA.SBS.ERP.Sage300.UpgradeWizard.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using MetroFramework.Forms;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> UI for Sage 300 Upgrade Wizard </summary>
    public partial class Upgrade : MetroForm
    {
        #region Private Variables

        /// <summary> The solution object </summary>
        private Solution2 _solution;

        /// <summary> Process Upgrade logic </summary>
        private ProcessUpgrade _upgrade;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        /// <summary> Log file </summary>
        private readonly StringBuilder _log = new StringBuilder();

		/// <summary> Source Folder </summary>
		private readonly string _sourceFolder;

        /// <summary> Accpac props file Source Folder </summary>
        private readonly string _propsSourceFolder;

        /// <summary> Destination Folder </summary>
        private readonly string _destinationFolder;

        /// <summary> Destination Web Folder </summary>
        private readonly string _destinationWebFolder;

        /// <summary> Destination Web </summary>
        private readonly string _destinationWeb;

        #endregion

        #region Private Constants
        /// <summary> Splitter Distance </summary>
        private const int SplitterDistance = 375;
        #endregion

        #region Delegates

        /// <summary> Delegate to update UI with name of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        private delegate void ProcessingCallback(string text);

        /// <summary> Delegate to update log with status of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        private delegate void LogCallback(string text);


		#endregion

		#region Constructor

		/// <summary> Upgrade Class </summary>
		/// <param name="destination">Destination Default</param>
		/// <param name="destinationWeb">Destination Web Default</param>
		/// <param name="templatePath">Upgrade Web Items template Path </param>
		public Upgrade(string destination, string destinationWeb, string templatePath, Solution2 solution)
		{
            _solution = solution;

			InitializeComponent();
			Localize();
			InitWizardSteps();
			InitEvents();
			ProcessingSetup(true);
			Processing("");

			// Setup local vars
			_destinationFolder = destination;
			_destinationWeb = destinationWeb;
			_sourceFolder = Path.GetDirectoryName(templatePath);
            _propsSourceFolder = Utilities.FileUtilities.GetParentPathFromPath(_sourceFolder);
			_destinationWebFolder = Directory.GetDirectories(_destinationFolder).FirstOrDefault(dir => dir.ToLower().Contains(Constants.Common.WebSuffix));
		}
		#endregion

		#region Button Events

		/// <summary> Next/Upgrade toolbar button </summary>
		/// <param name="sender">Sender object </param>
		/// <param name="e">Event Args </param>
		/// <remarks>Next wizard step or Upgrade if last step</remarks>
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

        #region Private Methods/Routines/Events

        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            // Default
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            #region Common for all upgrades

            AddStep(Resources.StepTitleMain,
                    string.Format(Resources.StepDescriptionMain,
                                  Constants.PerRelease.FromReleaseNumber,
                                  Constants.PerRelease.ToReleaseNumber),
                    BuildMainContentStep());

            if (Constants.PerRelease.SyncKendoFiles == true)
            {
                AddStep(Resources.ReleaseAllTitleSyncKendoFiles,
                        Resources.ReleaseAllDescSyncKendoFiles,
                        Resources.ReleaseAllSyncKendoFiles);
            }

            if (Constants.PerRelease.SyncWebFiles == true)
            {
                AddStep(Resources.ReleaseAllTitleSyncWebFiles,
                    Resources.ReleaseAllDescSyncWebFiles,
                    Resources.ReleaseAllSyncWebFiles);
            }

            #endregion

            if (Constants.PerRelease.UpdateAccpacDotNetLibrary == true)
            {
                AddStep(Resources.ReleaseAllTitleSyncAccpacLibs,
                        Resources.ReleaseAllDescSyncAccpacLibs,
                        string.Format(Resources.ReleaseAllSyncAccpacLibs,
                                      Constants.PerRelease.FromAccpacNumber,
                                      Constants.PerRelease.ToAccpacNumber));

            }

            #region Release Specific Steps...

#if ENABLE_TK_244885
            // This will be done post 2019.1 release
            // 2019.1 : Consolidate Enumerations
            AddStep(Resources.ReleaseSpecificTitleConsolidateEnumerations,
                    Resources.ReleaseSpecificDescConsolidateEnumerations,
                    string.Format(Resources.ReleaseSpecificUpdateConsolidateEnumerations,
                                  Constants.PerRelease.FromReleaseNumber,
                                  Constants.PerRelease.ToReleaseNumber));
#endif

            if (Constants.PerRelease.RemovePreviousJqueryLibraries == true)
            {
                //
                // Remove previous version of jQuery from file system and any references
                // within the solution .csproj files.
                //
                AddStep(Resources.ReleaseSpecificTitleRemovePreviousJqueryLibraries,
                        Resources.ReleaseSpecificTitleDescRemovePreviousJqueryLibraries,
                        string.Format(Resources.Template_ReleaseSpecificRemovePreviousJqueryLibraries,
                                        Constants.PerRelease.FromJqueryCoreVersion,
                                        Constants.PerRelease.FromJqueryUIVersion,
                                        Constants.PerRelease.FromJqueryMigrateVersion));
            }

            if (Constants.PerRelease.UpdateMicrosoftDotNetFramework == true)
            {
                //
                // Update the targeted version of Microsoft .NET framework 
                //
                AddStep(Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion,
                        Resources.ReleaseSpecificTitleDescTargetedDotNetFrameworkVersion,
                        string.Format(Resources.ReleaseSpecificUpdateTargetedDotNetFrameworkVersion,
                                      Constants.Common.TargetedDotNetFrameworkVersion));
            }

            #endregion

            #region Common for all upgrades - content specific to release

            AddStep(Resources.ReleaseAllTitleConfirmation,
                    Resources.ReleaseAllDescConfirmation,
                    Resources.ReleaseAllUpgrade);

            AddStep(Resources.ReleaseAllTitleRecompile,
                    Resources.ReleaseAllDescRecompile,
                    string.Format(Resources.ReleaseAllUpgraded,
                                  Resources.ShowLog,
                                  Constants.PerRelease.ToReleaseNumber));
            #endregion

            // Display first step
            NextStep();
        }

        /// <summary>
        /// Build Main Content Step
        /// </summary>
        /// <returns>Content for main screen</returns>
        private static string BuildMainContentStep()
        {
            var content = new StringBuilder();
            var step = 0;

            // Same for all upgrades
            content.AppendLine(Resources.FollowingSteps);
            content.AppendLine("");
            content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseAllTitleSyncKendoFiles}");
            content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseAllTitleSyncWebFiles}");

            if (Constants.PerRelease.UpdateAccpacDotNetLibrary == true)
            {
                content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseAllTitleSyncAccpacLibs}");
            }

            // Begin - Specific to release
#if ENABLE_TK_244885
            content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseSpecificTitleConsolidateEnumerations}");
#endif

            if (Constants.PerRelease.RemovePreviousJqueryLibraries == true)
            {
                content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseSpecificTitleRemovePreviousJqueryLibraries}");
            }

            if (Constants.PerRelease.UpdateMicrosoftDotNetFramework == true)
            {
                content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseSpecificTitleUpdateTargetedDotNetFrameworkVersion}");
            }

            // End - Specific to release

            // Same for all upgrades
            content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseAllTitleConfirmation}");
            content.AppendLine($"{Resources.Step} {++step}. {Resources.ReleaseAllTitleRecompile}");
            content.AppendLine("");
            content.AppendLine(Resources.EnsureBackup);

            return content.ToString();
        }

        /// <summary> Add wizard step </summary>
        /// <param name="title">Title for wizard step</param>
        /// <param name="description">Description for wizard step</param>
        /// <param name="content">Content for wizard step</param>
        /// <param name="showCheckbox">Optional. True to show checkbox otherwise false</param>
        /// <param name="checkboxText">Optional. Checkbox text</param>
        /// <param name="checkboxValue">Optional. Checkbox value</param>
        private void AddStep(string title, string description, string content, 
            bool showCheckbox = false, string checkboxText = "", bool checkboxValue = false)
        {
            _wizardSteps.Add(new WizardStep
            {
                Title = title,
                Description = description,
                Content = content,
                ShowCheckbox = showCheckbox,
                CheckboxText = checkboxText,
                CheckboxValue = checkboxValue
            });
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            // Finished?
            if (!_currentWizardStep.Equals(-1) && _currentWizardStep.Equals(_wizardSteps.Count - 1))
            {
                Close();
            }
            else
            {
                // Proceed to next wizard step or start upgrade if upgrade step
                if (!_currentWizardStep.Equals(-1) &&
                    _currentWizardStep.Equals(_wizardSteps.Count - 2))
                {
                    // Setup display before processing
                    ProcessingSetup(false);

                    _settings = new Settings
                    {
                        WizardSteps = _wizardSteps,
                        SourceFolder = _sourceFolder,
                        PropsSourceFolder = _propsSourceFolder,
                        DestinationWebFolder = _destinationWebFolder,
                        DestinationSolutionFolder = Directory.GetParent(_destinationWebFolder).ToString(),
                        Solution = _solution
                    };

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(_settings);
                }
                else
                {
                    // Proceed to next step
                    if (!_currentWizardStep.Equals(-1))
                    {
                        // Enable back button
                        btnBack.Enabled = true;
                    }
                    else
                    {
                        // Set the focus on the 'Next' button on the first page
                        btnNext.Focus();
                    }

                    // Increment step
                    _currentWizardStep++;

                    // Update title and text for step
                    ShowStep();

                    // Update text of Next button?
                    if (_currentWizardStep.Equals(_wizardSteps.Count - 2))
                    {
                        btnNext.Text = Resources.Upgrade;
                    }

                    // Update text of Next and buttons?
                    if (_currentWizardStep.Equals(_wizardSteps.Count - 1))
                    {
                        btnBack.Text = Resources.ShowLog;
                        btnNext.Text = Resources.Finish;
                    }
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
                // Show the log
                if (_currentWizardStep.Equals(_wizardSteps.Count - 1))
                {
                    var logPath = Path.Combine(_destinationFolder, Constants.Common.LogFileName);
                    System.Diagnostics.Process.Start(logPath);
                    return;
                }

                btnNext.Text = Resources.Next;
                _currentWizardStep--;

                // Update title and text for step
                ShowStep();

                // Enable back button?
                if (_currentWizardStep.Equals(0))
                {
                    btnBack.Enabled = false;
                    btnNext.Enabled = true;
                }

            }
        }
        
        /// <summary> Show Step Page</summary>
        private void ShowStep()
        {
            SetStepTitleAndDescription();

            // Display information
            lblContent.Text = _wizardSteps[_currentWizardStep].Content;

            // Checkbox
            checkBox.Text = _wizardSteps[_currentWizardStep].CheckboxText;
            checkBox.Checked = _wizardSteps[_currentWizardStep].CheckboxValue;
            splitStep.Panel2Collapsed = !_wizardSteps[_currentWizardStep].ShowCheckbox;

            splitSteps.SplitterDistance = SplitterDistance;
        }

        /// <summary>
        /// Set the current step title and description text
        /// </summary>
        private void SetStepTitleAndDescription()
        {
            // Update title and text for step
            var currentStep = _currentWizardStep.ToString("#0");
            var step = _currentWizardStep.Equals(0)
                            ? string.Empty
                            : $"{Resources.Step} {currentStep}{Resources.Dash}";

            lblStepTitle.Text = step + _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
        }

        /// <summary>
        /// Write log file to upgrade solution folder
        /// </summary>
        private void WriteLogFile()
        {
            var logFilePath = Path.Combine(_destinationFolder, Constants.Common.LogFileName);
            File.WriteAllText(logFilePath, _log.ToString());
        }

        /// <summary> Setup processing display </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            pnlButtons.Enabled = enableToolbar;
            pnlButtons.Refresh();
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessing.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.ProcessingStep, text);

            pnlButtons.Refresh();
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void ProcessingEvent(string text)
        {
            var callBack = new ProcessingCallback(Processing);
            Invoke(callBack, text);
        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            _upgrade.Process((Settings)e.Argument);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingSetup(true);
            Processing("");

            _currentWizardStep++;

            // Update title and text for step
            ShowStep();

            // Display final step
            btnBack.Text = Resources.ShowLog;
            btnNext.Text = Resources.Finish;

            // Write out log file with upgrade being complete
            WriteLogFile();
        }

        /// <summary> Help Button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disabled help until DPP wiki is available</remarks>
        private void Generation_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            // Display wiki link
            //System.Diagnostics.Process.Start(Resources.Browser, Resources.WikiLink);
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = String.Format(Resources.SolutionUpgrade, Constants.PerRelease.ToReleaseNumber);

            btnBack.Text = Resources.Back;
            btnBack.Enabled = false;

            btnNext.Text = Resources.Next;
        }

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            // Processing Events
            _upgrade = new ProcessUpgrade();
            _upgrade.ProcessingEvent += ProcessingEvent;
            _upgrade.LogEvent += LogEvent;
        }

        /// <summary> Update Log </summary>
        /// <param name="text">Text for Log</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void Log(string text)
        {
            _log.AppendLine(text);
        }

        /// <summary> Log Event </summary>
        /// <param name="text">Text for log</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void LogEvent(string text)
        {
            var callBack = new LogCallback(Log);
            Invoke(callBack, text);
        }

        /// <summary> Store value selected in Wizard step</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            // Stores value in step
            _wizardSteps[_currentWizardStep].CheckboxValue = checkBox.Checked;
        }
        #endregion
    }
}
