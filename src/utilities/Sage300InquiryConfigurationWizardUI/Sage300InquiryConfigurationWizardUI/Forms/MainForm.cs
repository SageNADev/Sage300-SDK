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

#region Imports
using Newtonsoft.Json;
using Sage300InquiryConfigurationWizardUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    public partial class MainForm : Form
    {
        #region Private Enumerations
        private enum OptionTypeEnum
        {
            Adhoc = 0,
            CRM,
            Inquiry
        }
        #endregion

        #region Private Variables
        private List<BorderedTextBox> _TextboxControls;
        private Settings _settings;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Primary form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            BuildTextBoxList();
            SetTextboxFocusedBorderColor();
            Localize();
            LoadSettings();
            InitOptionButtons();
            InitLanguageSupport();

            // Set the values of the controls based on the INI settings
            InitSettingsSection();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Build a list of all TextBox controls
        /// </summary>
        private void BuildTextBoxList()
        {
            _TextboxControls = new List<BorderedTextBox>
            {
                txtUser,
                txtPassword,
                txtCompany,
                txtVersion,

                txtLanguageSupportUserFra,
                txtLanguageSupportUserEsn,
                txtLanguageSupportUserCht,
                txtLanguageSupportUserChn,
                txtLanguageSupportPasswordFra,
                txtLanguageSupportPasswordEsn,
                txtLanguageSupportPasswordCht,
                txtLanguageSupportPasswordChn,

                txtRootPath,
                txtOutputPath,
                txtSQLScriptName,
                txtTemplateConfigurationFile,
                txtDatasourceConfigurationFile,
            };
        }

        /// <summary>
        /// Set the border color of all TextBox controls
        /// for when they have focus
        /// </summary>
        private void SetTextboxFocusedBorderColor()
        {
            var color = Constants.TextBoxBorderColor_Focus;
            foreach (var tb in _TextboxControls)
            {
                tb.FocusedBorderColor = color;
            }
        }

        /// <summary>
        /// Click event handler for the 'Close' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Display a simple confirmation dialog box
        /// </summary>
        /// <param name="msgIn">The text message to display </param>
        /// <returns>true = Proceed | false = Abort</returns>
        private bool Confirmation(string msgIn)
        {
            var caption = Resources.Confirmation;
            var message = String.Format("{0}{3}{3}{1}{3}{2}", 
                                        msgIn, 
                                        Resources.PressOKToProceed, 
                                        Resources.PressCancelToAbort, 
                                        Environment.NewLine);
            var result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            return (result != DialogResult.Cancel);
        }

        /// <summary>
        /// Generate the output files!
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == false)
            {
                var errorLine1 = Resources.ValidationErrors1;
                var errorLine2 = Resources.ValidationErrors2;
                var errorText = String.Format("{0}{1}{1}{2}", errorLine1, Environment.NewLine, errorLine2);
                Utilities.DisplayErrorMessage(errorText);
                return;
            }

            if (Confirmation(Resources.AreYouSureYouWishToProceed) == false)
            {
                return;
            }

            ClearLog();
            LogLine("Starting Generation...");

            // Get the currently specified settings
            PopulateSettingsFromForm();

            // Build the true output path 
            var outputPath = _settings.TrueOutputPath;

            var companySetting = new Company();
            companySetting.CompanyName = _settings.Company;
            companySetting.Version = _settings.Version;
            companySetting.Username = txtUser.Text;
            companySetting.Password = txtPassword.Text;

            var usernameEng = txtUser.Text;
            var passwordEng = txtPassword.Text;

            companySetting.IncludeFra = _settings.IncludeFra;
            companySetting.IncludeEsn = _settings.IncludeEsn;
            companySetting.IncludeCht = _settings.IncludeCht;
            companySetting.IncludeChn = _settings.IncludeChn;

            if (companySetting.IncludeFra)
            {
                if (txtLanguageSupportUserFra.Text.Length == 0)
                {
                    companySetting.UsernameFra = usernameEng;
                    companySetting.PasswordFra = passwordEng;
                }
                else
                {
                    companySetting.UsernameFra = txtLanguageSupportUserFra.Text;
                    companySetting.PasswordFra = txtLanguageSupportPasswordFra.Text;
                }
            }

            if (companySetting.IncludeEsn)
            {
                companySetting.IncludeFra = true;

                if (txtLanguageSupportUserEsn.Text.Length == 0)
                {
                    companySetting.UsernameEsn = usernameEng;
                    companySetting.PasswordEsn = passwordEng;
                }
                else
                {
                    companySetting.UsernameEsn = txtLanguageSupportUserEsn.Text;
                    companySetting.PasswordEsn = txtLanguageSupportPasswordEsn.Text;
                }
            }

            if (companySetting.IncludeChn)
            {
                if (txtLanguageSupportUserChn.Text.Length == 0)
                {
                    companySetting.UsernameChn = usernameEng;
                    companySetting.PasswordChn = passwordEng;
                }
                else
                {
                    companySetting.UsernameChn = txtLanguageSupportUserChn.Text;
                    companySetting.PasswordChn = txtLanguageSupportPasswordChn.Text;
                }
            }

            if (companySetting.IncludeCht)
            {
                if (txtLanguageSupportUserCht.Text.Length == 0)
                {
                    companySetting.UsernameCht = usernameEng;
                    companySetting.PasswordCht = passwordEng;
                }
                else
                {
                    companySetting.UsernameCht = txtLanguageSupportUserCht.Text;
                    companySetting.PasswordCht = txtLanguageSupportPasswordCht.Text;
                }
            }

            // Test Run Logs
            var _RunLogs = new List<LogRecord>();

            LogRecord tranRec = null;

            CreateOutputPaths();

            // Read Override presentation list
            var OverridePresentationList = new List<OverridePresentationList>();
            if (File.Exists(Path.Combine(_settings.RootPath, "OverridePresentationListJSON.JSON")))
            {
                OverridePresentationList = JsonConvert.DeserializeObject<List<OverridePresentationList>>(File.ReadAllText(Path.Combine(_settings.RootPath, "OverridePresentationListJSON.JSON")));
            }

            #region ReadSage300ViewConfigurationFile

            var DatasourceConfigurationFile = _settings.DatasourceConfigurationFile;
            var Sage300ViewInquiryConfigurationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(DatasourceConfigurationFile, "Sage300View", ref Sage300ViewInquiryConfigurationList);
            _RunLogs.Add(tranRec);
            #endregion

            #region ProcessSage300View
            foreach (var cr in Sage300ViewInquiryConfigurationList)
            {
                if (cr.Included == "Y")
                {
                    cr.OutputPath = _settings.TrueOutputPath;
                    #region ProcessSage300View
                    LogLine(string.Format("Read Configuration Column Setting File: {0}", cr.ConfigSettingFile));

                    var ConfigurationColumnList = new List<ConfigurationColumnSettingDefinition>();
                    tranRec = ReadConfigurationSetting.ReadInquiryConfigurationColumnSetting(cr.ConfigSettingFile, cr.ViewID, ref ConfigurationColumnList, Path.Combine(_settings.RootPath, "Controller.JSON"));
                    _RunLogs.Add(tranRec);

                    if (ConfigurationColumnList.Count() > 0)
                    {
                        Generation.ProcessView(this, companySetting, cr, ConfigurationColumnList, OverridePresentationList);
                    }
                    #endregion
                }
            }
            #endregion

            // Generate Datasource and Template JSON files
            #region ReadTemplateConfigurationFile

            //Console.WriteLine(string.Format("Read Inquiry Template Configuration File: {0}, tab: Template", TemplateConfigurationFile));

            //TemplateConfigurationFile = Path.Combine(MasterPath, TemplateConfigurationFile);
            var TemplateConfigurationFile = _settings.TemplateConfigurationFile;
            var TemplateInquiryConfigurationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "Template", ref TemplateInquiryConfigurationList);
            _RunLogs.Add(tranRec);

            LogLine(string.Format("Read Inquiry Datasource Inquiry Configuration File: {0}, tab: DatasourceSage300ViewMapping", TemplateConfigurationFile));

            var DSViewMappingList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "DatasourceSage300ViewMapping", ref DSViewMappingList);
            _RunLogs.Add(tranRec);

            LogLine(string.Format("Read Inquiry Datasource Inquiry Configuration File: {0}, tab: DatasourceSage300ViewMapping", TemplateConfigurationFile));

            var TemplateTranslationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "Translation", ref TemplateTranslationList);
            _RunLogs.Add(tranRec);

            LogLine(string.Format("Read Datasource Configuration File: {0}, tab: Datasource", TemplateConfigurationFile));

            var DatasourceList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "Datasource", ref DatasourceList);
            _RunLogs.Add(tranRec);

            #endregion

            #region ProcessDataSourceAndTemplateInquiryConfiguration
            foreach (var cr in TemplateInquiryConfigurationList)
            {
                if (cr.Included == "Y")
                {
                    cr.OutputPath = _settings.TrueOutputPath;

                    if (DSViewMappingList.Count() > 0)
                    {
                        Generation.GenerateInquiryConfigurationAndTemplate(this, companySetting, cr, DSViewMappingList, TemplateTranslationList, DatasourceList);
                    }
                }
            }
            #endregion

            #region GenerateDBScript

            Generation.GenerateDBScript(_settings.Option, 
                                        _settings.SQLScriptName, 
                                        _settings.TrueOutputPath, 
                                        TemplateInquiryConfigurationList, 
                                        DatasourceList, 
                                        "Create");

            Generation.GenerateDBScript(_settings.Option, 
                                        _settings.SQLScriptName, 
                                        _settings.TrueOutputPath, 
                                        TemplateInquiryConfigurationList, 
                                        DatasourceList, 
                                        "Update");

            Generation.GenerateDeleteDBScript(_settings.Option, 
                                              _settings.SQLScriptName, 
                                              _settings.TrueOutputPath, 
                                              TemplateInquiryConfigurationList, 
                                              DatasourceList);

            Generation.GenerateSQLScript(_settings.Option, 
                                         _settings.SQLScriptName, 
                                         _settings.TrueOutputPath, 
                                         TemplateInquiryConfigurationList, 
                                         DatasourceList);
            #endregion

            // export test run log
            var fileFormat = "txt";
            var logFilePath = Path.Combine(_settings.RootPath, string.Format(@"{0}.{1}", string.Format("{0}-{1}", "RunLog", DateTime.Now.ToString("yyyyMMdd")), fileFormat));
            using (var file = new StreamWriter(logFilePath, false))
            {
                file.Write(JsonConvert.SerializeObject(_RunLogs, Formatting.Indented));
            }

            // Display the success message
            MessageBox.Show(Resources.ConfigurationGenerationSuccessful, Resources.Status, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            // Show the output folder and the log file
            Process.Start(_settings.TrueOutputPath);
            Process.Start("notepad.exe", logFilePath);

            LogLine("Program run completed.");
        }

        /// <summary>
        /// Recreate (or create) the output folders
        /// </summary>
        private void CreateOutputPaths()
        {
            // Create Output Path
            if (Directory.Exists(_settings.TrueOutputPath))
            {
                LogLine(string.Format("Attempting to delete 'Output' folder if it already exists: {0}", _settings.TrueOutputPath));
                try
                {
                    Directory.Delete(_settings.TrueOutputPath, true);
                }
                catch (Exception)
                {
                    Directory.Delete(_settings.TrueOutputPath, true);

                }
            }

            LogLine(string.Format("Create 'Output' folder: {0}", _settings.TrueOutputPath));
            try
            {
                Directory.CreateDirectory(_settings.TrueOutputPath);
            }
            catch (Exception)
            {
                Directory.CreateDirectory(_settings.TrueOutputPath);
            }

            try
            {
                Directory.CreateDirectory(Path.Combine(_settings.TrueOutputPath, "View"));
            }
            catch (Exception)
            {
                Directory.CreateDirectory(Path.Combine(_settings.TrueOutputPath, "View"));
            }
        }

        /// <summary>
        /// Click handler for the 'Adhoc' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionAdhoc_Click(object sender, EventArgs e)
        {
            SetButtonSelectedColors(btnOptionAdhoc);
            SetButtonUnselectedColors(btnOptionCrm);
            SetButtonUnselectedColors(btnOptionInquiry);
        }

        /// <summary>
        /// Click handler for the 'CRM' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionCrm_Click(object sender, EventArgs e)
        {
            SetButtonSelectedColors(btnOptionCrm);
            SetButtonUnselectedColors(btnOptionAdhoc);
            SetButtonUnselectedColors(btnOptionInquiry);
        }

        /// <summary>
        /// Click handler for the 'Inquiry' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionInquiry_Click(object sender, EventArgs e)
        {
            SetButtonSelectedColors(btnOptionInquiry);
            SetButtonUnselectedColors(btnOptionAdhoc);
            SetButtonUnselectedColors(btnOptionCrm);
        }

        /// <summary>
        /// Set the background and foreground colors for the selected option button
        /// </summary>
        /// <param name="btn">A reference to the button control</param>
        private void SetButtonSelectedColors(Button btn)
        {
            btn.ForeColor = Constants.OptionButtonSelected_ForegroundColor;
            btn.BackColor = Constants.OptionButtonSelected_BackgroundColor;
        }

        /// <summary>
        /// Set the background and foreground colors for an 'unselected' option button
        /// </summary>
        /// <param name="btn">A reference to the button control</param>
        private void SetButtonUnselectedColors(Button btn)
        {
            btn.ForeColor = Constants.OptionButtonUnselected_ForegroundColor;
            btn.BackColor = Constants.OptionButtonUnselected_BackgroundColor;
        }

        /// <summary>
        /// Localize all of the dialog control text
        /// </summary>
        private void Localize()
        {
            // Window Title
            Text = Resources.InquiryConfiguration;

            // Application Credentials
            grpCredentials.Text = Resources.ApplicationCredentials;
            lblUser.Text = Resources.User + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblUser, Resources.UserTip);

            lblPassword.Text = Resources.Password + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblPassword, Resources.PasswordTip);

            lblVersion.Text = Resources.Version + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblVersion, Resources.VersionTip);

            lblCompany.Text = Resources.Company + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblCompany, Resources.CompanyTip);

            // Language Support
            grpLanguageSupport.Text = Resources.LanguageSupport;
            lblLanguageSupportUser.Text = Resources.User + Constants.LabelPostFixCharacter;
            lblLanguageSupportPassword.Text = Resources.Password + Constants.LabelPostFixCharacter;
            chkLanguageFra.Text = Resources.French;
            chkLanguageEsn.Text = Resources.Spanish;
            chkLanguageCht.Text = Resources.ChineseTraditional;
            chkLanguageChn.Text = Resources.ChineseSimplified;

            // Settings
            grpSettings.Text = Resources.Settings;
            lblSQLScriptName.Text = Resources.SQLScriptName + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblSQLScriptName, Resources.SQLScriptNameTip);

            lblRootPath.Text = Resources.RootPath + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblRootPath, Resources.RootPathTip);

            lblOutputPath.Text = Resources.OutputPath + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblOutputPath, Resources.OutputPathTip);

            lblOption.Text = Resources.Option + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblOption, Resources.OptionTip);

            lblDatasourceConfigurationFile.Text = Resources.DatasourceConfigurationFile + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblDatasourceConfigurationFile, Resources.DatasourceConfigurationFileTip);

            lblTemplateConfigurationFile.Text = Resources.TemplateConfigurationFile + Constants.LabelPostFixCharacter;
            toolTip.SetToolTip(lblTemplateConfigurationFile, Resources.TemplateConfigurationFileTip);

            btnOptionAdhoc.Text = Resources.Adhoc;
            btnOptionCrm.Text = Resources.CRM;
            btnOptionInquiry.Text = Resources.Inquiry;

            // Buttons
            btnClose.Text = Resources.Close;
            toolTip.SetToolTip(btnClose, Resources.CloseTip);

            btnSaveSettings.Text = Resources.SaveSettings;
            toolTip.SetToolTip(btnSaveSettings, Resources.SaveSettingsTip);

            btnGenerate.Text = Resources.Generate;
            toolTip.SetToolTip(btnGenerate, Resources.GenerateTip);
        }

        /// <summary>
        /// Load the settings from the application INI file
        /// </summary>
        private void LoadSettings()
        {
            _settings = new Settings();

            var currentFolder = GetCurrentExecutableFolder();
            string inifilePath = Path.Combine(currentFolder, Constants.DefaultIniFileName);
            _settings.IniFilePath = inifilePath;

            IniFile ini = new IniFile();
            if (ini.FileExists(inifilePath))
            {
                ini.Load(inifilePath);

                _settings.Option = ini["SETTINGS"]["Option"].ToString();
                _settings.RootPath = ini["SETTINGS"]["RootPath"].ToString();
                _settings.SQLScriptName = ini["SETTINGS"]["SQLScriptName"].ToString();
                _settings.OutputPath = ini["SETTINGS"]["OutputPath"].ToString();
                _settings.DatasourceConfigurationFile = ini["SETTINGS"]["DatasourceConfigurationFile"].ToString();
                _settings.TemplateConfigurationFile = ini["SETTINGS"]["TemplateConfigurationFile"].ToString();

                _settings.Company = ini["COMPANY"]["Company"].ToString();
                _settings.Version = ini["COMPANY"]["Version"].ToString();

                _settings.IncludeFra = ini["COMPANY"]["IncludeFra"].ToBool();
                _settings.IncludeEsn = ini["COMPANY"]["IncludeEsn"].ToBool();
                _settings.IncludeCht = ini["COMPANY"]["IncludeCht"].ToBool();
                _settings.IncludeChn = ini["COMPANY"]["IncludeChn"].ToBool();
            }
            else
            {
                // File doesn't exist
            }
        }

        /// <summary>
        /// Initialize the 'Options' buttons
        /// </summary>
        /// <param name="selectedOption"></param>
        private void InitOptionButtons(OptionTypeEnum selectedOption = OptionTypeEnum.Adhoc)
        {
            switch (selectedOption)
            {
                case OptionTypeEnum.Adhoc:
                    btnOptionAdhoc_Click(null, null);
                    break;

                case OptionTypeEnum.CRM:
                    btnOptionCrm_Click(null, null);
                    break;

                case OptionTypeEnum.Inquiry:
                    btnOptionInquiry_Click(null, null);
                    break;
            }
        } 

        /// <summary>
        /// Initialize the Language Support section
        /// </summary>
        private void InitLanguageSupport()
        {
            chkLanguageFra.Checked = _settings.IncludeFra;
            chkLanguageEsn.Checked = _settings.IncludeEsn;
            chkLanguageCht.Checked = _settings.IncludeCht;
            chkLanguageChn.Checked = _settings.IncludeChn;

            txtLanguageSupportUserFra.Visible = chkLanguageFra.Checked;
            txtLanguageSupportUserEsn.Visible = chkLanguageEsn.Checked;
            txtLanguageSupportUserCht.Visible = chkLanguageCht.Checked;
            txtLanguageSupportUserChn.Visible = chkLanguageChn.Checked;

            txtLanguageSupportPasswordFra.Visible = chkLanguageFra.Checked;
            txtLanguageSupportPasswordEsn.Visible = chkLanguageEsn.Checked;
            txtLanguageSupportPasswordCht.Visible = chkLanguageCht.Checked;
            txtLanguageSupportPasswordChn.Visible = chkLanguageChn.Checked;
        }

        /// <summary>
        /// Enter Focus handler for a group box.
        /// Set it's background color as 'Active'
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void group_Enter(object sender, EventArgs e)
        {
            (sender as GroupBox).BackColor = Constants.GroupActive_BackgroundColor;
        }

        /// <summary>
        /// Leave Focus handler for a group box.
        /// Set it's background color as 'Inactive'
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void group_Leave(object sender, EventArgs e)
        {
            (sender as GroupBox).BackColor = Constants.GroupInactive_BackgroundColor;
        }

        /// <summary>
        /// Click handler for the 'Root Path finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnRootPathFinder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = Resources.RootFolderBrowserDescription,
                ShowNewFolderButton = true, 
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var selectedFolder = dialog.SelectedPath.Trim();

            txtRootPath.Text = selectedFolder;
            txtOutputPath.Text = Path.Combine(selectedFolder, Constants.DefaultOutputFolderName);
            txtDatasourceConfigurationFile.Text = Path.Combine(selectedFolder, Constants.DefaultInquiryFolderName);
            txtTemplateConfigurationFile.Text = Path.Combine(selectedFolder, Constants.DefaultInquiryFolderName);

            ShowOrHideSettingsSectionControls(true);
        }

        /// <summary>
        /// Get the currently executing application folder
        /// </summary>
        /// <returns></returns>
        private string GetCurrentExecutableFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Click event handler for the 'Output Path finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOutputPathFinder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = Resources.OutputFolderBrowserDescription,
                ShowNewFolderButton = true,
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var selectedFolder = dialog.SelectedPath.Trim();

            txtOutputPath.Text = selectedFolder;
        }

        /// <summary>
        /// Click event handler for the 'Datasource Configuration File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnDatasourceConfigurationFileFinder_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.DatasourceColumnSettingFileFilter,
                FilterIndex = 1,
                Multiselect = false,
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var selectedFile = dialog.FileName.Trim();

            txtDatasourceConfigurationFile.Text = selectedFile;
        }

        /// <summary>
        /// Click event handler for the 'Template Configuration File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnTemplateConfigurationFileFinder_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.ConfigurationFileFilter,
                FilterIndex = 1,
                Multiselect = false,
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var selectedFile = dialog.FileName.Trim();

            txtTemplateConfigurationFile.Text = selectedFile;
        }

        /// <summary>
        /// Initialize the 'Settings' section on the form
        /// </summary>
        private void InitSettingsSection()
        {
            Enum.TryParse(_settings.Option, out OptionTypeEnum optionType);
            InitOptionButtons(optionType);

            txtSQLScriptName.Text = _settings.SQLScriptName;
            txtRootPath.Text = _settings.RootPath;
            if (txtRootPath.Text.Length > 0)
            {
                txtOutputPath.Text = _settings.OutputPath;
                txtDatasourceConfigurationFile.Text = _settings.DatasourceConfigurationFile;
                txtTemplateConfigurationFile.Text = _settings.TemplateConfigurationFile;
                txtCompany.Text = _settings.Company;
                txtVersion.Text = _settings.Version;
            } 
            else
            {
                ShowOrHideSettingsSectionControls(false);
            }
        }

        /// <summary>
        /// Show or hide some controls if the root path has not yet been set.
        /// </summary>
        /// <param name="show"></param>
        private void ShowOrHideSettingsSectionControls(bool show = true)
        {
            lblOutputPath.Visible = show;
            txtOutputPath.Visible = show;
            btnOutputPathFinder.Visible = show;

            grpConfigurationFiles.Visible = show;
        }

        /// <summary>
        /// Click event handler for the 'Save Settings' button 
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            var iniFilePath = _settings.IniFilePath;
            var ini = new IniFile();

            PopulateSettingsFromForm();

            ini["COMPANY"]["Company"] = _settings.Company;
            ini["COMPANY"]["Version"] = _settings.Version;

            ini["COMPANY"]["IncludeFra"] = _settings.IncludeFra;
            ini["COMPANY"]["IncludeEsn"] = _settings.IncludeEsn;
            ini["COMPANY"]["IncludeCht"] = _settings.IncludeCht;
            ini["COMPANY"]["IncludeChn"] = _settings.IncludeChn;

            ini["SETTINGS"]["Option"] = _settings.Option;
            ini["SETTINGS"]["SQLScriptName"] = _settings.SQLScriptName;
            ini["SETTINGS"]["RootPath"] = _settings.RootPath;
            ini["SETTINGS"]["OutputPath"] = _settings.OutputPath;
            ini["SETTINGS"]["DatasourceConfigurationFile"] = _settings.DatasourceConfigurationFile;
            ini["SETTINGS"]["TemplateConfigurationFile"] = _settings.TemplateConfigurationFile;

            ini.Save(iniFilePath);

            Utilities.DisplaySuccessMessage(Resources.SettingsSavedSuccessfully);
        }

        /// <summary>
        /// Transfer current form data to the Settings object
        /// </summary>
        private void PopulateSettingsFromForm()
        {
            _settings.Company = txtCompany.Text;
            _settings.Version = txtVersion.Text;

            _settings.IncludeFra = chkLanguageFra.Checked;
            _settings.IncludeEsn = chkLanguageEsn.Checked;
            _settings.IncludeCht = chkLanguageCht.Checked;
            _settings.IncludeChn = chkLanguageChn.Checked;

            _settings.Option = GetSelectedOption();
            _settings.RootPath = txtRootPath.Text;
            _settings.OutputPath = txtOutputPath.Text;
            _settings.DatasourceConfigurationFile = txtDatasourceConfigurationFile.Text;
            _settings.TemplateConfigurationFile = txtTemplateConfigurationFile.Text;
            _settings.SQLScriptName = txtSQLScriptName.Text;
        }

        /// <summary>
        /// Get the currently selected Option value
        /// </summary>
        /// <returns>The string representation of the currently selected option</returns>
        private string GetSelectedOption()
        {
            Color selectedColor = Constants.OptionButtonSelected_BackgroundColor;
            if (btnOptionAdhoc.BackColor == selectedColor) return OptionTypeEnum.Adhoc.ToString();
            if (btnOptionCrm.BackColor == selectedColor) return OptionTypeEnum.CRM.ToString();
            if (btnOptionInquiry.BackColor == selectedColor) return OptionTypeEnum.Inquiry.ToString();
            return String.Empty;
        }

        /// <summary>
        /// Process the language checkbox for non-english languages only
        /// </summary>
        /// <param name="textBoxUsername">Reference to Username textbox control for language</param>
        /// <param name="textBoxPassword">Reference to Password textbox control for language</param>
        /// <param name="checkBox">Reference to checkbox control for language</param>
        private void ProcessCheckBoxNonEnglish(BorderedTextBox textBoxUsername, BorderedTextBox textBoxPassword, CheckBox checkBox)
        {
            textBoxUsername.Visible = checkBox.Checked;
            textBoxPassword.Visible = checkBox.Checked;
            textBoxUsername.Enabled = checkBox.Checked;
            textBoxPassword.Enabled = checkBox.Checked;
        }

        /// <summary>
        /// Checked Changed handler for the 'Language French' checkbox
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void chkLanguageFra_CheckedChanged(object sender, EventArgs e)
        {
            var textBoxUsername = txtLanguageSupportUserFra;
            var textBoxPassword = txtLanguageSupportPasswordFra;
            var checkBox = sender as CheckBox;
            ProcessCheckBoxNonEnglish(textBoxUsername, textBoxPassword, checkBox);
        }

        /// <summary>
        /// Checked Changed handler for the 'Language Spanish' checkbox
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void chkLanguageEsn_CheckedChanged(object sender, EventArgs e)
        {
            var textBoxUsername = txtLanguageSupportUserEsn;
            var textBoxPassword = txtLanguageSupportPasswordEsn;
            var checkBox = sender as CheckBox;
            ProcessCheckBoxNonEnglish(textBoxUsername, textBoxPassword, checkBox);
        }

        /// <summary>
        /// Checked Changed handler for the 'Language Chinese Traditional' checkbox
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void chkLanguageCht_CheckedChanged(object sender, EventArgs e)
        {
            var textBoxUsername = txtLanguageSupportUserCht;
            var textBoxPassword = txtLanguageSupportPasswordCht;
            var checkBox = sender as CheckBox;
            ProcessCheckBoxNonEnglish(textBoxUsername, textBoxPassword, checkBox);
        }

        /// <summary>
        /// Checked Changed event handler for the 'Language Chinese Simplified' checkbox
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void chkLanguageChn_CheckedChanged(object sender, EventArgs e)
        {
            var textBoxUsername = txtLanguageSupportUserChn;
            var textBoxPassword = txtLanguageSupportPasswordChn;
            var checkBox = sender as CheckBox;
            ProcessCheckBoxNonEnglish(textBoxUsername, textBoxPassword, checkBox);
        }

        #region 'Validating' Event Handlers
        private void txtUser_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtPassword_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtCompany_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtVersion_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtRootPath_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtSQLScriptName_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtOutputPath_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtDatasourceConfigurationFile_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtTemplateConfigurationFile_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private bool ValidateControl(object sender, CancelEventArgs e)
        {
            var status = true;
            var control = sender as BorderedTextBox;
            var errorText = string.Empty;
            switch (control.Name)
            {
                case "txtUser": errorText = Resources.UsernameIsRequired; break;
                case "txtPassword": errorText = Resources.PasswordIsRequired; break;
                case "txtCompany": errorText = Resources.CompanyIsRequired; break;
                case "txtVersion": errorText = Resources.VersionIsRequired; break;
                case "txtRootPath": errorText = Resources.RootPathIsRequired; break;
                case "txtOutputPath": errorText = Resources.OutputPathIsRequired; break;
                case "txtSQLScriptName": errorText = Resources.SQLScriptNameIsRequired; break;
                case "txtDatasourceConfigurationFile": errorText = Resources.DatasourceConfigurationFileIsRequired; break;
                case "txtTemplateConfigurationFile": errorText = Resources.TemplateConfigurationFileIsRequired; break;
            }

            if (control.Text.Length == 0)
            {
                errorProvider.SetError(control, errorText);
                control.SetError(true);
                e.Cancel = true;
                status = false;
            }
            else
            {
                errorProvider.SetError(control, string.Empty);
                control.SetError(false);
                e.Cancel = false;
            }
            return status;
        }
        #endregion

        /// <summary>
        /// Validate the entire form
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            return this.ValidateChildren(ValidationConstraints.Enabled);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Insert a line of text into the logging console
        /// </summary>
        /// <param name="msg"></param>
        public void LogLine(string msg)
        {
            txtLogWindow.AppendText(Environment.NewLine);
            txtLogWindow.AppendText(msg);
        }

        /// <summary>
        /// Empty out the logging console
        /// </summary>
        public void ClearLog() => txtLogWindow.Clear();
        #endregion
    }
}
