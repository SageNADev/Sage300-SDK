// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using MetroFramework.Forms;
using System.Windows.Forms;
using EnvDTE;
using System.Drawing;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.LanguageResourceWizard
{
    /// <summary> UI for Sage 300 Language Resource Wizard </summary>
    public partial class WizardForm : MetroForm
    {
        #region Private Variables

        /// <summary> The solution object </summary>
        private Solution2 _solution;

        /// <summary> Process Upgrade logic </summary>
        private GenerateLanguageResources _upgrade;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        /// <summary> Log file </summary>
        private readonly StringBuilder _log = new StringBuilder();

        /// <summary>
        /// TODO - Add Description
        /// </summary>
        private Language _selectedLanguage = new Language();

        #endregion
        
        #region Private Constants
        private static class Constants
        {
            public const string PanelWelcome = "pnlWelcome";
            public const string PanelSelectLanguage = "pnlSelectLanguage";
            public const string PanelReview = "pnlReview";

            public const string InvalidLanguageCode = "--";

            public const int StartingStep = -1;
        }
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

        /// <summary>
        /// The constructor for WizardForm
        /// </summary>
        /// <param name="solution">A reference to the solution we'll be targeting</param>
        public WizardForm(Solution2 solution)
		{
            _solution = solution;

			InitializeComponent();
			Localize();
            InitLanguageList();
			InitWizardSteps();
			InitEvents();
			ProcessingSetup(true);
			Processing("");

            ResetBackgroundColors();

            // Display first step
            InitialStep();
        }
        #endregion

        /// <summary>
        /// Reset panel background colors to transparent
        /// Developer Note: Background colors are set to aid design and development only
        /// </summary>
        private void ResetBackgroundColors()
        {
            // Reset the colors
            splitBase.BackColor = Color.Transparent;
            splitBase.Panel1.BackColor = Color.Transparent;
            splitStep.Panel1.BackColor = Color.Transparent;
            splitStep.Panel2.BackColor = Color.Transparent;
            splitSteps.Panel2.BackColor = Color.Transparent;
            splitSteps.BackColor = Color.Transparent;
        }

        /// <summary> Are the prerequisites valid for executing the wizard </summary>
        /// <param name="solution">Solution</param>
        /// <remarks>Solution must be a Sage 300 solution with known projects</remarks>
        /// <returns>True if valid otherwise false</returns>
        public bool ValidPrerequisites(Solution solution)
        {
            // TODO - Get these working...

            // Validate solution
            if (!ValidSolution(solution))
            {
                DisplayMessage(Resources.InvalidSolution, MessageBoxIcon.Error);
                return false;
            }

            //// Validate projects
            //if (!ValidProjects(solution))
            //{
            //    //DisplayMessage(Resources.InvalidProjects, MessageBoxIcon.Error);
            //    return false;
            //}

            //// Build modules dropdowns for validations
            ////BuildModules();

            return true;
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Determine if the Solution is valid </summary>
        /// <param name="solution">Solution </param>
        /// <returns>true if valid otherwise false</returns>
        private static bool ValidSolution(_Solution solution)
        {
            // Validate solution
            return (solution != null);
        }

        ///// <summary> Determine if Projects in Solution are valid </summary>
        ///// <param name="solution">Solution </param>
        ///// <returns>True if valid otherwise false</returns>
        //private bool ValidProjects(_Solution solution)
        //{
        //    try
        //    {
        //        // Locals
        //        //var solutionFolder = Path.GetDirectoryName(solution.FullName);
        //        var projects = GetProjects(solution);

        //        // Iterate solution to get projects for analysis and usage
        //        foreach (var project in projects)
        //        {
        //            var projectName = Path.GetFileNameWithoutExtension(project.FullName);
        //            if (string.IsNullOrEmpty(projectName))
        //            {
        //                continue;
        //            }

        //            var segments = projectName.Split('.');
        //            var key = segments[segments.Length - 1];
        //            var module = segments[segments.Length - 2];

        //            // Grab the company namespace from the Business Repository project
        //            if (key.Equals(ProcessGeneration.Constants.BusinessRepositoryKey))
        //            {
        //                _companyNamespace = projectName.Substring(0,
        //                    projectName.IndexOf(module + "." + key, StringComparison.InvariantCulture) - 1);
        //            }

        //            // Determine which language resources to include
        //            if (key.Equals(ProcessGeneration.Constants.ResourcesKey))
        //            {
        //                SetLanguageFlagsBasedOnExistingProjectResourceFiles(project.ProjectItems);
        //            }

        //            // The Web project name is different from other ones. It should be derived from the folder name
        //            if (key.Equals(ProcessGeneration.Constants.WebKey))
        //            {
        //                var parts = project.FullName.Split('\\');
        //                // Last part is web project name
        //                projectName = parts[parts.Length - 1].Replace(".csproj", string.Empty);

        //                // Need to determine if area structure vs. non-area structure
        //                var projectFolder = Path.GetDirectoryName(project.FullName);
        //                if (!string.IsNullOrEmpty(projectFolder))
        //                {
        //                    var areaPath = Path.Combine(projectFolder, "Areas");
        //                    var areaStructure = Directory.Exists(areaPath);

        //                    // Store for later use in generation
        //                    _doesAreasExist = areaStructure;
        //                    _webProjectIncludesModule = false;

        //                    if (areaStructure)
        //                    {
        //                        // Iterate directories looking for the "module folder
        //                        var directories = Directory.GetDirectories(areaPath);
        //                        foreach (var moduleParts in from directory in directories
        //                                                    let modulePath = Path.Combine(directory, "Constants")
        //                                                    where Directory.Exists(modulePath)
        //                                                    select directory.Split('\\'))
        //                        {
        //                            module = moduleParts[moduleParts.Length - 1];

        //                            // Determine if module is in project name
        //                            _webProjectIncludesModule = projectName.Contains("." + module + ".");

        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Non-Area structure project
        //                        module = parts[parts.Length - 2];
        //                    }
        //                }

        //                // We will use the copyright from this project for all generated files
        //                _copyright = GetCopyright(project);
        //            }

        //            var projectInfo = new ProjectInfo
        //            {
        //                ProjectFolder = Path.GetDirectoryName(project.FullName),
        //                ProjectName = projectName,
        //                Project = project
        //            };

        //            // Add to list of projects by type and module
        //            if (_projects.ContainsKey(key))
        //            {
        //                _projects[key].Add(module, projectInfo);
        //            }
        //            else
        //            {
        //                _projects.Add(key, new Dictionary<string, ProjectInfo> { { module, projectInfo } });
        //            }
        //        }

        //    }
        //    catch
        //    {
        //        // No action as will be reviewed by caller                    
        //    }

        //    // Must have all projects to be valid
        //    return (_projects.ContainsKey(ProcessGeneration.Constants.BusinessRepositoryKey) &&
        //            _projects.ContainsKey(ProcessGeneration.Constants.InterfacesKey) &&
        //            _projects.ContainsKey(ProcessGeneration.Constants.ModelsKey) &&
        //            _projects.ContainsKey(ProcessGeneration.Constants.ResourcesKey) &&
        //            _projects.ContainsKey(ProcessGeneration.Constants.ServicesKey) &&
        //            _projects.ContainsKey(ProcessGeneration.Constants.WebKey));
        //}

        ///// <summary> Gets projects </summary>
        ///// <param name="solution">Solution </param>
        ///// <returns>List of projects</returns>
        //private static IEnumerable<Project> GetProjects(_Solution solution)
        //{
        //    // Locals
        //    var projects = solution.Projects;
        //    var list = new List<Project>();
        //    var item = projects.GetEnumerator();

        //    try
        //    {
        //        // Iterate 
        //        while (item.MoveNext())
        //        {
        //            var project = (Project)item.Current;

        //            if (project == null)
        //            {
        //                continue;
        //            }

        //            // only add project folder
        //            if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
        //            {
        //                list.AddRange(GetSolutionFolderProjects(project));
        //            }
        //            else
        //            {
        //                list.Add(project);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // No action as it will be reviewed by caller
        //    }

        //    return list;
        //}

        ///// <summary> Gets projects in solution folder </summary>
        ///// <param name="project">Project </param>
        ///// <returns>List of projects</returns>
        //private static IEnumerable<Project> GetSolutionFolderProjects(Project project)
        //{
        //    // Locals
        //    var list = new List<Project>();

        //    // Iterate projects
        //    for (var projectItem = 1; projectItem <= project.ProjectItems.Count; projectItem++)
        //    {
        //        var subProject = project.ProjectItems.Item(projectItem).SubProject;

        //        if (subProject == null)
        //        {
        //            continue;
        //        }

        //        // Recursion for another solution folder
        //        if (subProject.Kind.Equals(ProjectKinds.vsProjectKindSolutionFolder))
        //        {
        //            list.AddRange(GetSolutionFolderProjects(subProject));
        //        }
        //        else
        //        {
        //            list.Add(subProject);
        //        }
        //    }

        //    return list;
        //}

        ///// <summary> Get copyright of project </summary>
        ///// <param name="project">Project </param>
        ///// <returns>Copyright from AssemblyInfo of project</returns>
        //private static string GetCopyright(Project project)
        //{
        //    var retVal = string.Empty;

        //    try
        //    {
        //        // Iterate project looking for the AssemblyInfo class
        //        foreach (ProjectItem projectItem in project.ProjectItems)
        //        {
        //            if (projectItem.Name.Equals("Properties"))
        //            {
        //                foreach (ProjectItem assemblyItem in projectItem.ProjectItems)
        //                {
        //                    retVal = GetCopyrightAttribute(assemblyItem.Properties.Item("LocalPath").Value.ToString());
        //                    break;
        //                }

        //            }

        //            // Break early out of outer loop
        //            if (!string.IsNullOrEmpty(retVal))
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // Swallow error
        //    }

        //    return retVal;
        //}

        ///// <summary> Get copyright attribute </summary>
        ///// <param name="fileName">Assembly Info file name </param>
        ///// <returns>Copyright from AssemblyInfo of project</returns>
        //private static string GetCopyrightAttribute(string fileName)
        //{
        //    // Locals
        //    var retVal = string.Empty;

        //    try
        //    {
        //        // Read resource info file
        //        var lines = File.ReadAllLines(@fileName);

        //        // Iterate and search for "AssemblyCopyright attribute
        //        foreach (
        //            var parsedLine in from line in lines where line.Contains("AssemblyCopyright") select line.Split('"')
        //        )
        //        {
        //            if (parsedLine.Length > 0)
        //            {
        //                retVal = parsedLine[1];
        //            }
        //            break;
        //        }
        //    }
        //    catch
        //    {
        //        // Swallow error
        //    }

        //    return retVal;
        //}


        /// <summary> Initialize panel </summary>
        private void InitPanel(Panel panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
        }

        /// <summary>
        /// Hide all wizard panels
        /// </summary>
        private void HidePanels()
        {
            InitPanel(pnlWelcome);
            InitPanel(pnlSelectLanguage);
            InitPanel(pnlReview);
        }

        /// <summary>
        /// Set up the language drop down list
        /// </summary>
        private void InitLanguageList()
        {
            List<Language> languages = new List<Language>();
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = "- Select Language -" });
            languages.Add(new Language { Code = "bg", Name = "Bulgarian" });
            languages.Add(new Language { Code = "cs", Name = "Czech" });
            languages.Add(new Language { Code = "da", Name = "Danish" });
            languages.Add(new Language { Code = "nl", Name = "Dutch" });
            languages.Add(new Language { Code = "fi", Name = "Finnish" });
            languages.Add(new Language { Code = "de", Name = "German" });
            languages.Add(new Language { Code = "it", Name = "Italian" });
            languages.Add(new Language { Code = "no", Name = "Norwegian" });
            languages.Add(new Language { Code = "pl", Name = "Polish" });
            languages.Add(new Language { Code = "pt", Name = "Portuguese" });
            languages.Add(new Language { Code = "ro", Name = "Romanian" });
            languages.Add(new Language { Code = "ru", Name = "Russian" });
            languages.Add(new Language { Code = "sv", Name = "Swedish" });
            languages.Add(new Language { Code = "uk", Name = "Ukrainian" });

            cboLanguage.DataSource = languages;
            cboLanguage.DisplayMember = "Name";
            cboLanguage.ValueMember = "Code";
        }

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
            _currentWizardStep = Constants.StartingStep;

            // Init wizard steps
            _wizardSteps.Clear();

            HidePanels();

            SetupSteps();

            // Hide this checkbox. Not sure why it's on the form.
            checkBox.Visible = false;
        }

        /// <summary>
        /// Initialize all wizard steps
        /// </summary>
        private void SetupSteps()
        {
            AddStep(Resources.Introduction_Title,
                    String.Empty,
                    pnlWelcome);

            AddStep(Resources.SelectLanguage_Title,
                    Resources.SelectLanguage_Description,
                    pnlSelectLanguage);

            AddStep(Resources.Review_Title,
                    String.Empty,
                    pnlReview);
        }

        /// <summary> Add wizard step </summary>
        /// <param name="title">Title for wizard step</param>
        /// <param name="description">Description for wizard step</param>
        /// <param name="content">Content for wizard step</param>
        /// <param name="showCheckbox">Optional. True to show checkbox otherwise false</param>
        /// <param name="checkboxText">Optional. Checkbox text</param>
        /// <param name="checkboxValue">Optional. Checkbox value</param>
        private void AddStep
            (
            string title, 
            string description, 
            Panel panel
            )
        {
            _wizardSteps.Add(new WizardStep
            {
                Title = title,
                Description = description,
                Panel = panel
            });
        }

        /// <summary>
        /// Execute the initial wizard step
        /// </summary>
        private void InitialStep()
        {
            if (_currentWizardStep == Constants.StartingStep)
            {
                // Buttons
                DisableBackButton();
                btnNext.Text = Resources.Begin;
                EnableNextButton();
                btnNext.Focus();

                // Increment step
                _currentWizardStep++;

                // Update title, text and panel for the step
                ShowStep(true);
            }
        }

        /// <summary>
        /// Is this the initial wizard step?
        /// </summary>
        /// <returns>true : initial step | false : not the initial step</returns>
        private bool IsInitialStep()
        {
            return _currentWizardStep == Constants.StartingStep;
        }

        /// <summary>
        /// Is this the last wizard step?
        /// </summary>
        /// <returns>true : last step | false : not the last step</returns>
        private bool IsLastStep()
        {
            var totalSteps = _wizardSteps.Count;
            return _currentWizardStep == totalSteps;
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            var totalSteps = _wizardSteps.Count;

            // Finished?
            if (IsLastStep())
            {
                Close();
            }
            else
            {
                // Proceed to next wizard step or start processing if processing step
                if (!IsInitialStep() && _currentWizardStep.Equals(totalSteps - 1))
                {
                    // Setup display before processing
                    ProcessingSetup(false);

                    _settings = new Settings
                    {
                        Solution = _solution,
                        Language = _selectedLanguage
                    };

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(_settings);
                }
                else
                {
                    // Proceeding to next step

                    // Set the focus on the 'Next' button on the first page
                    if (IsInitialStep())
                    {
                        btnNext.Focus();
                    }

                    ShowStep(false);

                    // Increment step
                    _currentWizardStep++;

                    //if (_currentWizardStep == 2)
                    //{
                    //    GetSelectedLanguage();

                    //    var contentText = Resources.Review_Content_Template;
                    //    contentText = contentText.Replace("{0}", _selectedLanguage.Name);
                    //    lblReview_ContentTemplate.Text = contentText;
                    //}

                    // Update title, text and panel for the step
                    ShowStep(true);

                    UpdateButtons();
                }
            }
        }

        /// <summary>
        /// Get the selected language from the language combobox
        /// </summary>
        private void GetSelectedLanguage()
        {
            _selectedLanguage.Code = cboLanguage.SelectedValue.ToString();
            _selectedLanguage.Name = cboLanguage.Text;
        }

        /// <summary>
        /// Back Navigation 
        /// </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            var totalSteps = _wizardSteps.Count;

            // Proceed if not on first step
            if (_currentWizardStep > 0)
            {
                ShowStep(false);

                _currentWizardStep--;

                ShowStep(true);

                UpdateButtons();
            }
        }

        /// <summary>
        /// Determine if a valid language was selected
        /// </summary>
        /// <returns>true : Valid language selected | false : Invalid language selected</returns>
        private bool IsValidLanguageSelected()
        {
            GetSelectedLanguage();
            return _selectedLanguage.Code != Constants.InvalidLanguageCode;
        }

        /// <summary>
        /// Update the buttons (enable/disable/set label text)
        /// </summary>
        private void UpdateButtons()
        {
            if (_currentWizardStep == 0)
            {
                btnNext.Text = Resources.Begin;
                DisableBackButton();
                EnableNextButton();
            }
            else if (_currentWizardStep == 1)
            {
                btnNext.Text = Resources.Next;
                EnableBackButton();

                if (IsValidLanguageSelected())
                {
                    EnableNextButton();
                } 
                else
                {
                    DisableNextButton();
                }
                
            }
            else if (_currentWizardStep == 2)
            {
                btnNext.Text = Resources.Generate;
                EnableBackButton();
                EnableNextButton();
            }
        }

        /// <summary>
        /// Enable the 'Back' button
        /// </summary>
        private void EnableBackButton() => EnableOrDisableButton(btnBack, true);

        /// <summary>
        /// Disable the 'Back' button
        /// </summary>
        private void DisableBackButton() => EnableOrDisableButton(btnBack, false);

        /// <summary>
        /// Enable the 'Next' button
        /// </summary>
        private void EnableNextButton() => EnableOrDisableButton(btnNext, true);

        /// <summary>
        /// Disable the 'Next' button
        /// </summary>
        private void DisableNextButton() => EnableOrDisableButton(btnNext, false);

        /// <summary>
        /// Enable or Disable a button
        /// </summary>
        /// <param name="btn">The button</param>
        /// <param name="enable">Enable/Disable flag</param>
        private void EnableOrDisableButton(Button btn, bool enable) => btn.Enabled = enable;

        /// <summary> Show Step Page</summary>
        private void ShowStep(bool visible)
        {
            SetStepTitleAndDescription();

            if (_currentWizardStep > -1)
            {
                _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
                _wizardSteps[_currentWizardStep].Panel.Visible = visible;

                if (_currentWizardStep == 0)
                {
                    lblWelcome_Content.Text = Resources.WelcomeContent;
                }
                else if (_currentWizardStep == 1)
                {
                    lblLanguage.Text = Resources.Language;
                }
                else if (_currentWizardStep == 2)
                {
                    GetSelectedLanguage();

                    var contentText = Resources.Review_Content_Template;
                    contentText = contentText.Replace("{0}", _selectedLanguage.Name);
                    lblReview_ContentTemplate.Text = contentText;
                }
            }
        }

        /// <summary>
        /// Set the current step title and description text
        /// </summary>
        private void SetStepTitleAndDescription()
        {
            if (_currentWizardStep > -1)
            {
                // Update title and text for step
                var currentStep = _currentWizardStep.ToString("#0");
                var step = _currentWizardStep.Equals(0)
                                ? string.Empty
                                : $"{Resources.Step} {currentStep}{Resources.Dash}";

                lblStepTitle.Text = step + _wizardSteps[_currentWizardStep].Title;
                lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
            }
        }

        /// <summary>
        /// Write log file to upgrade solution folder
        /// </summary>
        private void WriteLogFile()
        {
            //var logFilePath = Path.Combine(_destinationFolder, Constants.Common.LogFileName);
            //File.WriteAllText(logFilePath, _log.ToString());
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
            ShowStep(true);

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
            // Set the main wizard title
            Text = Resources.WizardTitle;

            btnBack.Text = Resources.Back;
            DisableBackButton();

            btnNext.Text = Resources.Next;
        }

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            // Processing Events
            _upgrade = new GenerateLanguageResources();
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
            //_wizardSteps[_currentWizardStep].CheckboxValue = checkBox.Checked;
        }
        #endregion

        /// <summary>
        /// TODO - Add Description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedValue.ToString() == Constants.InvalidLanguageCode)
            {
                DisableNextButton();
            } 
            else
            {
                EnableNextButton();
            }
        }
    }
}
