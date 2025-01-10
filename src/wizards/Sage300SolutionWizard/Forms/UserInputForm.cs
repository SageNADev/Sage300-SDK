// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.SolutionWizard.Properties;
using MetroFramework.Forms;
using Microsoft.ServiceHub.Resources;
using VSLangProj;

namespace Sage.CA.SBS.ERP.Sage300.SolutionWizard
{
    public partial class UserInputForm : MetroForm
    {
        #region Private Variables
        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Generate </summary>
        private bool _generate = false;

        /// <summary>
        /// width of the form
        /// </summary>
        private const int FORM_WIDTH = 688;

        /// <summary>
        /// height of the form
        /// </summary>
        private const int FORM_HEIGHT = 467;


        #endregion

        #region Private Constants
        private static class Constants
		{
            public const string KendoLicenseUrl = @"http://www.telerik.com/purchase/license-agreement/kendo-ui-complete";

			/// <summary> Splitter Distance </summary>
			public const int SplitterDistance = 237;

			/// <summary> Single space </summary>
			public const string SingleSpace = " ";

            /// <summary> The names of all of the panels </summary>
            public const string PanelProjectType = "pnlProjectType";
            public const string PanelGenerateSolution = "pnlGenerateSolution";
            public const string PanelInfo = "pnlInfo";
            public const string PanelKendo = "pnlKendo";
            public const string PanelResourceFiles = "pnlResourceFiles";
        }
        #endregion

        #region Public Properties
        public string ThirdPartyCompanyName { get; set; }
        public string ThirdPartyApplicationId { get; set; }
        public string CompanyNamespace { get; set; }
        public string KendoFolder { get; set; }
        public string KendoDefaultFolder { set { txtKendoFolder.Text = value; }}
        public bool IncludeEnglish { get; set; }
        public bool IncludeChineseSimplified { get; set; }
        public bool IncludeChineseTraditional { get; set; }
        public bool IncludeSpanish { get; set; }
        public bool IncludeFrench { get; set; }

        public bool IsWebSolution => radioButtonWeb.Checked;
        #endregion

        #region Constructor
        public UserInputForm()
        {
            InitializeComponent();
            Localize();
            InitWizardSteps();
            txtCompanyName.Focus();
        }
        #endregion

        /// <summary> Localize </summary>
        private void Localize()
        {
            // Not going to display any kind of version number in the dialog title.
            Text = Resources.SolutionGeneration;

            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

            radioButtonWeb.Text = Resources.ProjectWeb;
            radioButtonWebApi.Text = Resources.ProjectWebApi;

            // Main Step
            lblCompanyName.Text = Resources.CompanyName;
            tooltip.SetToolTip(lblCompanyName, Resources.CompanyNameTip);

            lblModuleId.Text = Resources.ModuleId;
            tooltip.SetToolTip(lblModuleId, Resources.ModuleIdTip);

            lblNamespace.Text = Resources.NamespaceName;
            tooltip.SetToolTip(lblNamespace, Resources.NamespaceNameTip);

            // Kendo Step
            chkKendoLicense.Text = Resources.KendoLicense;
            tooltip.SetToolTip(chkKendoLicense, Resources.KendoLicenseTip);

            //lblKendoFolder.Text = Resources.KendoFolder;
            //tooltip.SetToolTip(lblKendoFolder, Resources.KendoFolderTip);

            //tooltip.SetToolTip(btnKendoDialog, Resources.KendoFolderDialog);

            lblKendoFolderHelp.Text = Resources.KendoFolderLinkTip;

            // Resource Step
            chkEnglish.Text = Resources.English;
            tooltip.SetToolTip(chkEnglish, Resources.EnglishTip);

            chkSpanish.Text = Resources.Spanish;
            tooltip.SetToolTip(chkSpanish, string.Format(Resources.NonEnglishTip, Resources.Spanish));

            chkFrench.Text = Resources.French;
            tooltip.SetToolTip(chkFrench, string.Format(Resources.NonEnglishTip, Resources.French));

            chkChineseSimplified.Text = Resources.ChineseSimplified;
            tooltip.SetToolTip(chkChineseSimplified, string.Format(Resources.NonEnglishTip, Resources.ChineseSimplified));

            chkChineseTraditional.Text = Resources.ChineseTraditional;
            tooltip.SetToolTip(chkChineseTraditional, string.Format(Resources.NonEnglishTip, Resources.ChineseTraditional));

            // Generate Step
            lblGenerateHelp.Text = Resources.GenerateTip;

            // set the form size
            Size = new Size(FORM_WIDTH, FORM_HEIGHT);

        }

        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            // Default
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlProjectType);
            InitPanel(pnlInfo);
            InitPanel(pnlKendo);
            InitPanel(pnlResourceFiles);
            InitPanel(pnlGenerateSolution);
            AddStep(Resources.SelectSolutionTypeStepTitle, Resources.SelectSolutionTypeStepDesc, pnlProjectType);

            // Display first step
            NextStep();
        }

        /// <summary> Initialize panel </summary>
        private static void InitPanel(Control panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
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

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            // Start code generation if last step
            if (!_currentWizardStep.Equals(-1) && IsCurrentPanel(Constants.PanelGenerateSolution))
            {
                _generate = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
				// Proceed to next wizard step

				if (!_currentWizardStep.Equals(-1))
                {
                    // Before proceeding to next step, ensure current step is valid
                    if (!ValidSettings())
                    {
                        return;
                    }

                    btnBack.Enabled = true;

                    ShowStep(false);

                    if (IsCurrentPanel(Constants.PanelProjectType))
                    {
                        _wizardSteps.Clear();
                        AddStep(Resources.SelectSolutionTypeStepTitle, Resources.SelectSolutionTypeStepDesc, pnlProjectType);
                        AddStep(Resources.EnterInformationStepTitle, Resources.EnterInformationStepDesc, pnlInfo);
                        if (radioButtonWeb.Checked)
                        {
                            AddStep(Resources.StepTitleKendo, Resources.StepDescriptionKendo, pnlKendo);
                            AddStep(Resources.StepTitleResourceFiles, Resources.StepDescriptionResourceFiles,
                                pnlResourceFiles);
                        }

                        AddStep(Resources.StepTitleGenerate, Resources.StepDescriptionGenerate, pnlGenerateSolution);
                        _currentWizardStep = 0;
                    }
                }

                _currentWizardStep++;

                ShowStep(true);

                // Update text of Next button?
                if (IsCurrentPanel(Constants.PanelGenerateSolution))
                {
                    btnNext.Text = Resources.Generate;
                }

                // Update title and text for step
                ShowStepTitle();
            }
        }

        /// <summary> Back Navigation </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            if (!_currentWizardStep.Equals(0))
            {
                // Proceed back a step
                if (IsCurrentPanel(Constants.PanelGenerateSolution))
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
            _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
            _wizardSteps[_currentWizardStep].Panel.Visible = visible;
            splitSteps.SplitterDistance = Constants.SplitterDistance;
        }

        /// <summary> Show Step Title</summary>
        private void ShowStepTitle()
        {
			// Build up the dialog box step title and description
            // Example of label
            // Step 1 - This is the step title
			var label = Resources.Step + 
						Constants.SingleSpace + 
						(_currentWizardStep + 1).ToString("#0") +
                        Constants.SingleSpace +
                        Resources.Dash +
                        Constants.SingleSpace +
                        _wizardSteps[_currentWizardStep].Title;

			lblStepTitle.Text = label;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
        }

        /// <summary> Valid Settings </summary>
        /// <returns>True if settings are valid otherwise false</returns>
        private bool ValidSettings()
        {
            var valid = string.Empty;

            if (IsCurrentPanel(Constants.PanelInfo))
            {
                valid = ValidPnlInfo();
            }

            if (IsCurrentPanel(Constants.PanelKendo))
            {
                valid = ValidPnlKendo();
            }

            if (IsCurrentPanel(Constants.PanelResourceFiles))
            {
                // Not a validation but set flags for language resources
                IncludeEnglish = chkEnglish.Checked;
                IncludeChineseSimplified = chkChineseSimplified.Checked;
                IncludeChineseTraditional = chkChineseTraditional.Checked;
                IncludeSpanish = chkSpanish.Checked;
                IncludeFrench = chkFrench.Checked;
            }

			// Were there any validation errors? 
			if (!string.IsNullOrEmpty(valid))
            {
                DisplayMessage(valid, MessageBoxIcon.Error);
            }

            return string.IsNullOrEmpty(valid);
        }

        /// <summary>
        /// Valid first step (Company info and module)
        /// </summary>
        /// <returns>Empty string if valid, otherwise the appropriate error message</returns>
        private string ValidPnlInfo()
        {
            // Company Name Validation
            ThirdPartyCompanyName = txtCompanyName.Text.Trim();
            if (string.IsNullOrEmpty(ThirdPartyCompanyName))
            {
                return Resources.CompanyNameInvalid;
            }

            // Module ID Validation
            ThirdPartyApplicationId = txtApplicationID.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(ThirdPartyApplicationId) || ThirdPartyApplicationId.Contains(" "))
            {
                return Resources.ModuleIdInvalid;
            }

            // Namespace Validation
            CompanyNamespace = txtNamespace.Text.Trim();
            if (string.IsNullOrEmpty(CompanyNamespace) || CompanyNamespace.Contains(" "))
            {
                return Resources.NamespaceInvalid;
            }

            return string.Empty;
        }

        /// <summary>
        /// Valid second step (Kendo)
        /// </summary>
        /// <returns>Empty string if valid, otherwise the appropriate error message</returns>
        private string ValidPnlKendo()
        {
            // Kendo License Validation
            KendoFolder = txtKendoFolder.Text.Trim();
            if (!chkKendoLicense.Checked)
            {
                return Resources.KendoLicenseInvalid;
            }

            // Kendo Folder Validation
            if (string.IsNullOrEmpty(KendoFolder))
            {
                return Resources.KendoFolderInvalid;
            }

            return string.Empty;
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Defaults Namespace </summary>
        private void SetNamespace()
        {
            ThirdPartyCompanyName = txtCompanyName.Text.Trim();

            txtNamespace.Text = !string.IsNullOrEmpty(ThirdPartyCompanyName) ? ThirdPartyCompanyName.Replace(" ", "") : "";
        }

        /// <summary> 
        /// Set and display an example namespace 
        /// For Example: SageValuedPartner.TU.Web
        /// </summary>
        private void SetExampleNamespace()
        {
            // Define some constants
            const string DetailsNotYetSpecified = @"Please enter the above details first";
            const string SampleAppId = @"XX";
            const string NamespaceMask = @"{0}.{1}.{2}";

            // Get the current values from all three fields
            var companyName = txtCompanyName.Text.Trim();
            var appId = txtApplicationID.Text.Trim();
            var theNamespace = txtNamespace.Text.Trim();

            // Now massage them a bit
            companyName = !string.IsNullOrEmpty(companyName) ? companyName.Replace(" ", "") : "";
            appId = appId.Length > 0 ? appId : SampleAppId;
            var exampleNamespace = string.Empty;
            if (companyName.Length > 0 && appId.Length > 0 && theNamespace.Length > 0)
            {
                exampleNamespace = string.Format(NamespaceMask, theNamespace, appId, IsWebSolution ? "Web" : "WebApi");
            }
            else
            {
                exampleNamespace = DetailsNotYetSpecified;
            }

            // Set the example namespace field
            txtNamespaceExample.Text = exampleNamespace;
        }

        /// <summary> Company Name Text Changed </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Defaults namespace</remarks>
        private void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            SetNamespace();
            SetExampleNamespace();
        }

        /// <summary> Application ID Text Changed </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Defaults namespace</remarks>
        private void txtApplicationID_TextChanged(object sender, EventArgs e)
        {
            SetExampleNamespace();
        }

        /// <summary> Namespace Text Changed </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Defaults namespace</remarks>
        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            SetExampleNamespace();
        }

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

        /// <summary> Kendo Folder search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnKendoDialog_Click(object sender, EventArgs e)
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

            txtKendoFolder.Text = dialog.SelectedPath.Trim();
        }

        /// <summary> Help Button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Disabled help until DPP wiki is available</remarks>
        private void UserInputForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            // Display wiki link
            // System.Diagnostics.Process.Start(Resources.Browser, Resources.WikiLink);
        }

        /// <summary> Close Button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void UserInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_generate)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary> License Checkbox</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void chkKendoLicense_CheckedChanged(object sender, EventArgs e)
        {
            var flag = chkKendoLicense.Checked;
            txtKendoFolder.Enabled = flag;
        }

        /// <summary> Kendo License Link</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void lblKendoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            lblKendoLink.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start(Constants.KendoLicenseUrl);
        }

        /// <summary>
        /// Helper method to check the currently visible panel
        /// </summary>
        /// <param name="panelName">The name of the panel as a string</param>
        /// <returns>
        /// true = current panel is the one specified
        /// false = current panel is not the one specified
        /// </returns>
        private bool IsCurrentPanel(string panelName)
        {
            return _wizardSteps[_currentWizardStep].Panel.Name.Equals(panelName);
        }

        /// <summary> Do not allow punctuation chars in company name </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void txtCompanyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar))
            {
                // Swallow key stroke for punctuation chars
                e.Handled = true;
            }
        }
    }
}
