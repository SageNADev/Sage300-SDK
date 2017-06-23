// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
using System.IO;
using System.Windows.Forms;
using Sage300UICustomizationWizard.Properties;
using Newtonsoft.Json.Linq;

namespace Sage300UICustomizationWizard
{
    public partial class UserInputForm : Form
    {
        #region Private Vars

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        #endregion

        #region Private Constants

        /// <summary> Panel Name for pnlCreateEdit </summary>
        private const string PanelCreateEdit = "pnlCreateEdit";

        /// <summary> Bootstrapper Suffix </summary>
        private const string BootstrapperSuffix = "Bootstrapper.xml";

        /// <summary> Assembly Suffix </summary>
        private const string AssemblySuffix = ".Web.dll";

        /// <summary> Customization Module </summary>
        private const string CustomizationModule = "CU";

        /// <summary> Application Modules </summary>
        private const string ApplicationModules = "AP,AR,AS,BK,TX,CS,GL,IC,OE,PO,";

        #endregion

        #region Public vars
        /// <summary> Business Partner Name ($companyname$) </summary>
        public string BusinessPartnerName { get; set; }
        /// <summary> Project Name ($project$) </summary>
        public string ProjectName { get; set; }
        /// <summary> Module ($module$) </summary>
        public string ModuleName { get; set; }
        /// <summary> Assembly Name </summary>
        public string AssemblyName { get; set; }
        /// <summary> Customization Manifest </summary>
        public JObject CustomizationManifest { get; set; }
        /// <summary> Customization FileName </summary>
        public string CustomizationFileName { get; set; }

        #endregion

        #region Constructor
        /// <summary> Constructor </summary>
        public UserInputForm()
        {
            InitializeComponent();
            Localize();
            InitWizardSteps();
            InitEvents();
        }
        #endregion

        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            // Default
            btnNext.Text = Resources.Generate;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlCreateEdit);

            // Assign steps for wizard
            AddStep(Resources.StepTitleCreateEdit, Resources.StepDescriptionCreateEdit, pnlCreateEdit);

            // Display first step
            NextStep();
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            // Proceed to next step
            if (!_currentWizardStep.Equals(-1))
            {
                // Before proceeding to next step, ensure current step is valid
                if (!ValidateStep())
                {
                    return;
                }

                // Set flag indicating wizard screen closed normally as close is flag to run solution creation
                DialogResult = DialogResult.OK;
                
                // Set vars for use in solution creation
                BusinessPartnerName = txtCompanyName.Text.Trim();
                ProjectName = txtProject.Text.Trim().Replace(".", "");
                ModuleName = txtModule.Text.Trim().ToUpper();
                AssemblyName = txtAssembly.Text.Trim().Replace(".dll", "");

                // Close screen and kick off solution creation
                Close();
                return;
            }

            _currentWizardStep++;

            _wizardSteps[_currentWizardStep].Panel.Dock = DockStyle.Fill;
            _wizardSteps[_currentWizardStep].Panel.Visible = true;

            // Update title and text for step
            lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash +
                                _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;

            splitBase.Panel2.Refresh();
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
                if (!string.IsNullOrEmpty(valid))
                {
                    DisplayMessage(valid, MessageBoxIcon.Error);
                }
            }

            return string.IsNullOrEmpty(valid);
        }

        /// <summary> Valid Create/Edit Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidCreateEditStep()
        {
            // Package Id
            if (string.IsNullOrEmpty(txtPackageId.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Package.Replace(":", ""));
            }

            // Folder
            if (string.IsNullOrEmpty(txtFolderName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Folder.Replace(":", ""));
            }

            // Customization Name
            if (string.IsNullOrEmpty(txtCustomizationName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.CustomizationName.Replace(":", ""));
            }

            // Customization Description
            if (string.IsNullOrEmpty(txtCustomizationDescription.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.CustomizationDescription.Replace(":", ""));
            }

            // Company Name
            if (string.IsNullOrEmpty(txtCompanyName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.CompanyName.Replace(":", ""));
            }

            // Module
            if (string.IsNullOrEmpty(txtModule.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Module.Replace(":", ""));
            }

            // Module must be 2 characters
            if (txtModule.Text.Trim().Length != 2)
            {
                return Resources.InvalidModuleLength;
            }

            // Module cannot be an application module
            txtModule.Text = txtModule.Text.ToUpper();
            if (ApplicationModules.Contains(txtModule.Text.Trim()))
            {
                return Resources.InvalidModuleUsage;
            }

            // Project
            if (string.IsNullOrEmpty(txtProject.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Project.Replace(":", ""));
            }

            return string.Empty;
        }

        /// <summary> Initialize panel </summary>
        /// <param name="panel">Panel to initialize</param>
        private static void InitPanel(Panel panel)
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


        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Manifest.json search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnPackageFinder_Click(object sender, EventArgs e)
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

            // Display manifest selected
            ExistingManifest(dialog.FileName.Trim());

        }

        /// <summary> Existing JSON Manifest</summary>
        /// <param name="fileName">File name </param>
        private void ExistingManifest(string fileName)
        {
            // Get the manifest and store for later use
            CustomizationManifest  = JObject.Parse(File.ReadAllText(fileName));

            // Properties
            txtPackageId.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyPackageId);
            txtCustomizationName.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyName);
            txtCustomizationDescription.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyDescription);
            txtCompanyName.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyBusinessPartnerName);
            txtCompatibility.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertySageCompatibility);
            txtVersion.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyVersion);

            txtBootstrapper.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyBootstrapper);
            txtAssembly.Text = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyAssembly);

            // Assign name or default name
            if (string.IsNullOrEmpty(txtAssembly.Text.Trim()))
            {
                // Default module
                txtModule.Text = CustomizationModule;
                // Default project company concatenated with customization name
                txtProject.Text = txtCompanyName.Text.Replace(" ", "") +
                                  Resources.Dot +
                                  txtCustomizationName.Text.Replace(" ", "");
            }
            else
            {
                // Get segments of assembly
                var segments = txtAssembly.Text.Trim().Split('.');
                // Pull out module
                txtModule.Text = segments[segments.Length - 3];
                // Build project
                var project = string.Empty;
                for (int i = 0; i < segments.Length - 3; i++)
                {
                    // Append dot before next segment
                    if (!string.IsNullOrEmpty(project))
                    {
                        project += Resources.Dot;
                    }
                    project += segments[i];
                }
                txtProject.Text = project;
            }

            // Get location from folder where found
            var path = Path.GetDirectoryName(fileName);
            txtFolderName.Text = path;

            var eula = (string)CustomizationManifest.SelectToken(Sage300UICustomizationUserInterface.PropertyEula);
            txtEula.Text = eula;

            // Store the file name for later update
            CustomizationFileName = fileName;
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.WebCustomization;

            btnNext.Text = Resources.Generate;

            // Step Create/Edit
            lblPackageId.Text = Resources.Package;
            tooltip.SetToolTip(lblPackageId, Resources.PackageIdTip);

            lblFolder.Text = Resources.Folder;
            tooltip.SetToolTip(lblFolder, Resources.FolderNameTip);

            lblCustomizationName.Text = Resources.CustomizationName;
            tooltip.SetToolTip(lblCustomizationName, Resources.CustomizationNameTip);

            lblCustomizationDescription.Text = Resources.CustomizationDescription;
            tooltip.SetToolTip(lblCustomizationDescription, Resources.CustomizationDescriptionTip);

            lblCompanyName.Text = Resources.CompanyName;
            tooltip.SetToolTip(lblCompanyName, Resources.BusinessPartnerNameTip);

            lblCompatibility.Text = Resources.Compatibility;
            tooltip.SetToolTip(lblCompatibility, Resources.CompatibilityTip);

            lblVersion.Text = Resources.Version;
            tooltip.SetToolTip(lblVersion, Resources.VersionTip);

            lblEula.Text = Resources.EULA;
            tooltip.SetToolTip(lblEula, Resources.EULATip);

            lblBootstrapper.Text = Resources.Bootstrapper;
            tooltip.SetToolTip(lblBootstrapper, Resources.BootstrapperTip);

            lblAssembly.Text = Resources.Assembly;
            tooltip.SetToolTip(lblAssembly, Resources.AssemblyTip);

            lblModule.Text = Resources.Module;
            tooltip.SetToolTip(lblModule, Resources.ModuleTip);

            lblProject.Text = Resources.Project;
            tooltip.SetToolTip(lblProject, Resources.ProjectTip);

            tooltip.SetToolTip(btnPackageFinder, Resources.PackageFinderTip);

        }

        /// <summary> Do not allow space characters in content</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void txtProject_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        /// <summary> Next button selected</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        /// <summary> Update contents of Bootstrapper and Assembly based upon module/project content</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void ModuleProjectTextChanged(object sender, EventArgs e)
        {
            // Bootstrapper
            txtBootstrapper.Text = txtProject.Text.Trim().Replace(Resources.Dot, "") + txtModule.Text.Trim() + BootstrapperSuffix;

            // Assembly
            txtAssembly.Text = txtProject.Text.Trim() + Resources.Dot + txtModule.Text.Trim() + AssemblySuffix;
        }
        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            txtModule.TextChanged += ModuleProjectTextChanged;
            txtProject.TextChanged += ModuleProjectTextChanged;
        }

    }
}
