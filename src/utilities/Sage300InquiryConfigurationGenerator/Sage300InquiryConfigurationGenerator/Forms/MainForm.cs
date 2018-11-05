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
using Sage300InquiryConfigurationGenerator.Extensions;
using Sage300InquiryConfigurationGenerator.Forms;
using Sage300InquiryConfigurationGenerator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationGenerator
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

        private enum ValidationRuleEnum
        {
            RequiredField,
            ValidFile,
            ValidUsername,
            ValidPassword,
        };
        #endregion

        #region Private Class(es)
        private class ValidationRule
        {
            public ValidationRule(ValidationRuleEnum rule, string message)
            {
                this.Rule = rule;
                this.Message = message;
            }

            public ValidationRuleEnum Rule { get; set; }
            public string Message { get; set; }
        }
        #endregion

        #region Private Variables
        private Settings _settings;
        private ValidationErrors _validationErrors;
        #endregion

        private Progress<int> ProgressObject = null;

        #region Constructor(s)
        /// <summary>
        /// Primary form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetTextboxFocusedBorderColor();
            Localize();
            LoadSettings();
            InitGeneralSettings();
            InitializeDebugSpecificControls();
            InitOptionButtons();
            InitLanguageSupport();

            // Set the values of the controls based on the INI settings
            InitSettingsSection();

            _validationErrors = new ValidationErrors();
            progressBar.Hide();

            ProgressObject = new Progress<int>(v => {
                progressBar.Value = v;
            });
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Depending on the build type, show/hide controls
        /// </summary>
        private void InitializeDebugSpecificControls()
        {
#if DEBUG
            btnDebugEmptyForm.Visible = true;
            btnDebugTestMessageBox.Visible = true;
#else
            btnDebugEmptyForm.Visible = false;
            btnDebugTestMessageBox.Visible = false;
#endif
        }

        /// <summary>
        /// Set the border color property of all TextBox controls
        /// for when they have focus
        /// </summary>
        private void SetTextboxFocusedBorderColor()
        {
            var color = Constants.TextBoxBorderColor_Focus;
            Utilities.FindControls<BorderedTextBox>(this)
                     .ToList()
                     .ForEach(tb => tb.FocusedBorderColor = color);
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
        /// Click event handler for the 'Empty Form' button
        /// Note: Debug version only!
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnDebugEmptyForm_Click(object sender, EventArgs e)
        {
            Utilities.ClearAllTextBoxes(this);
        }

        /// <summary>
        /// Click event handler for the 'Message Box' button
        /// Note: Debug version only!
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnDebugTestMessageBox_Click(object sender, EventArgs e)
        {
            var msgBox = new ModalMessageBox();
            msgBox.Caption = "Status";
            msgBox.Title = "Validation Errors";
            msgBox.Message = "This is the actual error message or messages.";
            DialogResult result = msgBox.ShowDialog(this);
            msgBox.Dispose();
        }

        /// <summary>
        /// Generate the output files!
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;

            // Ensure all fields are valid
            if (ValidateForm() == false)
            {
                var validationErrors = _validationErrors.GetAllAsString();
                var errorLine1 = Resources.ValidationErrors1;
                var errorText = String.Format("{0}{1}{1}{2}", errorLine1, Environment.NewLine, validationErrors);
                LogLine(errorText);
                Utilities.DisplayErrorMessage(errorText);
                _validationErrors.Clear();
                btnGenerate.Enabled = true;
                return;
            }

            // Ask for confirmation before proceeding
            if (Utilities.Confirmation(Resources.GenerateConfiguration,
                                       Resources.Confirmation,
                                       Resources.AreYouSureYouWishToProceed) == false)
            {
                btnGenerate.Enabled = true;
                return;
            }

            progressBar.Show();

            await Task.Run(() => DoProcessing());

            progressBar.Hide();
        }

        /// <summary>
        /// This is the primary processing block
        /// </summary>
        private void DoProcessing()
        {
            var progressPercentage = 0;
            var message = string.Empty;

            // Set this so we can call the logging methods in this class (to update the UI thread)
            Generation.Parent = this;

            message = "Starting Generation Process...";
            this.RunOnUIThread(() => { ClearLog(); LogLine(message); SetProgress(progressPercentage); });

            // Get the currently specified settings
            PopulateSettingsFromForm();

            // Build the true output path 
            var outputPath = _settings.TrueOutputPath;

            var company = new Company();
            company.CompanyName = _settings.Company;
            company.Version = _settings.Version;
            company.Username = txtUser.Text.Trim();
            company.Password = txtPassword.Text.Trim();

            company.IncludeFra = _settings.IncludeFra;
            company.IncludeEsn = _settings.IncludeEsn;
            company.IncludeCht = _settings.IncludeCht;
            company.IncludeChn = _settings.IncludeChn;


            // Grab the language usernames and passwords from the form
            // Note: Done to make variable names smaller :)
            var userFra = txtLanguageSupportUserFra.Text.Trim();
            var passFra = txtLanguageSupportUserFra.Text.Trim();
            var userEsn = txtLanguageSupportUserEsn.Text.Trim();
            var passEsn = txtLanguageSupportUserEsn.Text.Trim();
            var userCht = txtLanguageSupportUserCht.Text.Trim();
            var passCht = txtLanguageSupportUserCht.Text.Trim();
            var userChn = txtLanguageSupportUserChn.Text.Trim();
            var passChn = txtLanguageSupportUserChn.Text.Trim();

            SetNonEnglishUsernameAndPassword(Company.LanguageEnum.FRA, userFra, passFra, ref company);
            SetNonEnglishUsernameAndPassword(Company.LanguageEnum.ESN, userEsn, passEsn, ref company);
            SetNonEnglishUsernameAndPassword(Company.LanguageEnum.CHT, userCht, passCht, ref company);
            SetNonEnglishUsernameAndPassword(Company.LanguageEnum.CHN, userChn, passChn, ref company);

            var _RunLogs = new List<LogRecord>();

            LogRecord tranRec = null;

            CreateOutputPaths();

            List<OverridePresentationList> OverridePresentationList = LoadOverridePresentationListIfExists();

            var DatasourceConfigurationFile = _settings.DatasourceConfigurationFile;
            var Sage300ViewInquiryConfigurationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(DatasourceConfigurationFile, "Sage300View", ref Sage300ViewInquiryConfigurationList);
            _RunLogs.Add(tranRec);

                
            foreach (var cr in Sage300ViewInquiryConfigurationList)
            {
                if (cr.Included == "Y")
                {
                    cr.OutputPath = _settings.TrueOutputPath;
                    #region ProcessSage300View
                    message = string.Format("Read Configuration Column Setting File: {0}", cr.ConfigSettingFile);
                    this.RunOnUIThread(() => { LogLine(message); SetProgress(progressPercentage += 5); });

                    var file = _settings.ControllerParameterDefinitionFile;
                    var ConfigurationColumnList = new List<ConfigurationColumnSettingDefinition>();
                    tranRec = ReadConfigurationSetting.ReadInquiryConfigurationColumnSetting(cr.ConfigSettingFile, cr.ViewID, ref ConfigurationColumnList, file);
                    _RunLogs.Add(tranRec);

                    if (ConfigurationColumnList.Count() > 0)
                    {
                        Generation.ProcessView(company, cr, ConfigurationColumnList, OverridePresentationList);
                    }
                    #endregion
                }
            }

            // Generate Datasource and Template JSON files
            #region ReadTemplateConfigurationFile

            var TemplateConfigurationFile = _settings.TemplateConfigurationFile;
            var TemplateInquiryConfigurationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "Template", ref TemplateInquiryConfigurationList);
            _RunLogs.Add(tranRec);

            message = string.Format("Read Inquiry Datasource Inquiry Configuration File: {0}, tab: DatasourceSage300ViewMapping", TemplateConfigurationFile);
            this.RunOnUIThread(() => { LogLine(message); SetProgress(progressPercentage += 5); });


            var DSViewMappingList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "DatasourceSage300ViewMapping", ref DSViewMappingList);
            _RunLogs.Add(tranRec);

            message = string.Format("Read Inquiry Datasource Inquiry Configuration File: {0}, tab: DatasourceSage300ViewMapping", TemplateConfigurationFile);
            this.RunOnUIThread(() => { LogLine(message); SetProgress(progressPercentage += 5); });

            var TemplateTranslationList = new List<InquiryConfigurationDefinition>();
            tranRec = ReadConfigurationSetting.ReadInquiryConfigurationSetting(TemplateConfigurationFile, "Translation", ref TemplateTranslationList);
            _RunLogs.Add(tranRec);

            message = string.Format("Read Datasource Configuration File: {0}, tab: Datasource", TemplateConfigurationFile);
            this.RunOnUIThread(() => { LogLine(message); SetProgress(progressPercentage += 5); });

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
                        Generation.GenerateInquiryConfigurationAndTemplate(company, cr, DSViewMappingList, TemplateTranslationList, DatasourceList);
                    }
                }
            }
            #endregion

            #region GenerateDBScript

            Generation.GenerateDBScript(_settings.Option, _settings.SQLScriptName, _settings.TrueOutputPath,
                                        TemplateInquiryConfigurationList, DatasourceList, "Create");

            Generation.GenerateDBScript(_settings.Option, _settings.SQLScriptName, _settings.TrueOutputPath,
                                        TemplateInquiryConfigurationList, DatasourceList, "Update");

            Generation.GenerateDeleteDBScript(_settings.Option, _settings.SQLScriptName, _settings.TrueOutputPath,
                                                TemplateInquiryConfigurationList, DatasourceList);

            Generation.GenerateSQLScript(_settings.Option, _settings.SQLScriptName, _settings.TrueOutputPath,
                                            TemplateInquiryConfigurationList, DatasourceList);
            #endregion

            string logFilePath = WriteLogFile(_RunLogs);

            // Display the message
            var finalMessage = string.Format("{0}{1}{1}{2}", Resources.ProgramRunCompleted, Environment.NewLine, Resources.PleaseEnsureNoErrorsOccurred);
            this.RunOnUIThread(() => { LogLine(finalMessage); Utilities.DisplaySuccessMessage(finalMessage); SetProgress(100); });

            if (_settings.DisplayOutputFolderOnCompletion == true)
            {
                // Show the output folder and the log file
                message = Resources.DisplayingOutputFolder;
                this.RunOnUIThread(() => { LogLine(message); SetProgress(100); });
                Process.Start(_settings.TrueOutputPath);
            }

            if (_settings.DisplayLogFileOnCompletion == true)
            {
                message = Resources.DisplayingOutputLogFile;
                this.RunOnUIThread(() => { LogLine(message); SetProgress(100); });
                Process.Start("notepad.exe", logFilePath);
            }

            this.RunOnUIThread(() => { LogLine(finalMessage); SetProgress(100); });

            message = String.Format(Resources.TheLogFileIsLocatedHereTemplate, logFilePath);
            this.RunOnUIThread(() => { LogLine(message); SetProgress(100); btnGenerate.Enabled = true; });
        }

        /// <summary>
        /// Write out the log file
        /// </summary>
        /// <param name="_RunLogs">The List of LogRecords to write to a file</param>
        /// <returns>The fully-qualified path to the newly generated log file</returns>
        private string WriteLogFile(List<LogRecord> _RunLogs)
        {
            var fileFormat = "txt";
            var filePrefix = "RunLog";
            var dateStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var logFileName = String.Format("{0}-{1}.{2}", filePrefix, dateStamp, fileFormat);
            var logFilePath = Path.Combine(_settings.RootPath, logFileName);
            using (var file = new StreamWriter(logFilePath, false))
            {
                file.Write(JsonConvert.SerializeObject(_RunLogs, Formatting.Indented));
            }

            return logFilePath;
        }

        /// <summary>
        /// Load the optional Override Presentation list file
        /// </summary>
        /// <returns>The List of Overrides</returns>
        private List<OverridePresentationList> LoadOverridePresentationListIfExists()
        {
            var file = _settings.OverridePresentationListFile;
            var list = new List<OverridePresentationList>();

            var msg = String.Format("Attempting to read the override presentation list file '{0}'.", file);
            this.RunOnUIThread(() => { LogLine(msg); SetProgress(0); });

            if (File.Exists(file))
            {
                msg = "Override presentation list file exists.";
                this.RunOnUIThread(() => { LogLine(msg); SetProgress(0); });
                list = JsonConvert.DeserializeObject<List<OverridePresentationList>>(File.ReadAllText(file));
            }

            return list;
        }

        /// <summary>
        /// Recreate (or create) the output folders
        /// </summary>
        private void CreateOutputPaths()
        {
            var msg = String.Empty;

            // Create Output Path
            if (Directory.Exists(_settings.TrueOutputPath))
            {
                msg = string.Format("Attempting to delete 'Output' folder if it already exists: {0}", _settings.TrueOutputPath);
                this.RunOnUIThread(() => { LogLine(msg); SetProgress(0); });

                try
                {
                    Directory.Delete(_settings.TrueOutputPath, true);
                }
                catch (Exception)
                {
                    Directory.Delete(_settings.TrueOutputPath, true);

                }
            }

            msg = string.Format("Create 'Output' folder: {0}", _settings.TrueOutputPath);
            this.RunOnUIThread(() => { LogLine(msg); SetProgress(0); });
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
        /// Set the specific languages username and/or password only if that
        /// particular language has been marked for inclusion. 
        /// </summary>
        /// <param name="lang">The language to target</param>
        /// <param name="userLang">The username specified for that particular language</param>
        /// <param name="passLang">The password specified for that particular language</param>
        /// <param name="company">A reference to the company object</param>
        private void SetNonEnglishUsernameAndPassword(Company.LanguageEnum lang, string userLang, string passLang, ref Company company)
        {
            // Determine correct language inclusion flag to use
            var includeLang = false;
            if (lang == Company.LanguageEnum.FRA) includeLang = company.IncludeFra;
            if (lang == Company.LanguageEnum.ESN) includeLang = company.IncludeEsn;
            if (lang == Company.LanguageEnum.CHT) includeLang = company.IncludeCht;
            if (lang == Company.LanguageEnum.CHN) includeLang = company.IncludeChn;

            // Process
            if (includeLang == true)
            {
				#region Set Username
                if (userLang.Length == 0)
                {
                    switch (lang)
                    {
                        case Company.LanguageEnum.FRA: company.UsernameFra = company.Username; break;
                        case Company.LanguageEnum.ESN: company.UsernameEsn = company.Username; break;
                        case Company.LanguageEnum.CHT: company.UsernameCht = company.Username; break;
                        case Company.LanguageEnum.CHN: company.UsernameChn = company.Username; break;
                    }
                }
                else
                {
                    switch (lang)
                    {
                        case Company.LanguageEnum.FRA: company.UsernameFra = userLang; break;
                        case Company.LanguageEnum.ESN: company.UsernameEsn = userLang; break;
                        case Company.LanguageEnum.CHT: company.UsernameCht = userLang; break;
                        case Company.LanguageEnum.CHN: company.UsernameChn = userLang; break;
                    }
                }
				#endregion

				#region Set Password
                if (passLang.Length == 0)
                {
                    switch (lang)
                    {
                        case Company.LanguageEnum.FRA: company.PasswordFra = company.Password; break;
                        case Company.LanguageEnum.ESN: company.PasswordEsn = company.Password; break;
                        case Company.LanguageEnum.CHT: company.PasswordCht = company.Password; break;
                        case Company.LanguageEnum.CHN: company.PasswordChn = company.Password; break;
                    }
                }
                else
                {
                    switch (lang)
                    {
                        case Company.LanguageEnum.FRA: company.PasswordFra = passLang; break;
                        case Company.LanguageEnum.ESN: company.PasswordEsn = passLang; break;
                        case Company.LanguageEnum.CHT: company.PasswordCht = passLang; break;
                        case Company.LanguageEnum.CHN: company.PasswordChn = passLang; break;
                    }
                }
				#endregion
            }
        }

        /// <summary>
        /// Click handler for the 'Adhoc' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionAdhoc_Click(object sender, EventArgs e)
        {
            // Selected
            SetOptionButtonSelectedColors(btnOptionAdhoc, OptionTypeEnum.Adhoc);

            // Unselected
            SetOptionButtonUnselectedColors(btnOptionCrm, OptionTypeEnum.CRM);
            SetOptionButtonUnselectedColors(btnOptionInquiry, OptionTypeEnum.Inquiry);
        }

        /// <summary>
        /// Click handler for the 'CRM' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionCrm_Click(object sender, EventArgs e)
        {
            // Selected 
            SetOptionButtonSelectedColors(btnOptionCrm, OptionTypeEnum.CRM);

            // Unselected
            SetOptionButtonUnselectedColors(btnOptionAdhoc, OptionTypeEnum.Adhoc);
            SetOptionButtonUnselectedColors(btnOptionInquiry, OptionTypeEnum.Inquiry);
        }

        /// <summary>
        /// Click handler for the 'Inquiry' option button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOptionInquiry_Click(object sender, EventArgs e)
        {
            // Select 
            SetOptionButtonSelectedColors(btnOptionInquiry, OptionTypeEnum.Inquiry);

            // Unselected
            SetOptionButtonUnselectedColors(btnOptionAdhoc, OptionTypeEnum.Adhoc);
            SetOptionButtonUnselectedColors(btnOptionCrm, OptionTypeEnum.CRM);
        }

        /// <summary>
        /// Set the background and foreground colors for the selected option button
        /// </summary>
        /// <param name="btn">A reference to the button control</param>
        /// <param name="option">The option to target</param>
        private void SetOptionButtonSelectedColors(Button btn, OptionTypeEnum option)
        {
            Color foregroundColor = Color.White;
            Color backgroundColor = Color.Black;

            switch (option)
            {
                case OptionTypeEnum.Adhoc:
                    foregroundColor = Constants.OptionButton_Adhoc_Selected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_Adhoc_Selected_BackgroundColor;
                    break;

                case OptionTypeEnum.CRM:
                    foregroundColor = Constants.OptionButton_CRM_Selected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_CRM_Selected_BackgroundColor;
                    break;

                case OptionTypeEnum.Inquiry:
                    foregroundColor = Constants.OptionButton_Inquiry_Selected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_Inquiry_Selected_BackgroundColor;
                    break;
            }

            btn.ForeColor = foregroundColor;
            btn.BackColor = backgroundColor;
        }

        /// <summary>
        /// Set the background and foreground colors for an 'unselected' option button
        /// </summary>
        /// <param name="btn">A reference to the button control</param>
        /// <param name="option">The option to target</param>
        private void SetOptionButtonUnselectedColors(Button btn, OptionTypeEnum option)
        {
            Color foregroundColor = Color.White;
            Color backgroundColor = Color.Black;

            switch (option)
            {
                case OptionTypeEnum.Adhoc:
                    foregroundColor = Constants.OptionButton_Adhoc_Unselected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_Adhoc_Unselected_BackgroundColor;
                    break;

                case OptionTypeEnum.CRM:
                    foregroundColor = Constants.OptionButton_CRM_Unselected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_CRM_Unselected_BackgroundColor;
                    break;

                case OptionTypeEnum.Inquiry:
                    foregroundColor = Constants.OptionButton_Inquiry_Unselected_ForegroundColor;
                    backgroundColor = Constants.OptionButton_Inquiry_Unselected_BackgroundColor;
                    break;
            }

            btn.ForeColor = foregroundColor;
            btn.BackColor = backgroundColor;
        }

        /// <summary>
        /// Localize all of the dialog control text
        /// </summary>
        private void Localize()
        {
            // Window Title
            SetApplicationTitle();

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

            chkDisplayOutputFolderOnCompletion.Text = Resources.DisplayOutputFolderOnCompletion;
            chkDisplayLogFileOnCompletion.Text = Resources.DisplayLogFileOnCompletion;

            // Buttons
            btnClose.Text = Resources.Close;
            toolTip.SetToolTip(btnClose, Resources.CloseTip);

            btnSaveSettings.Text = Resources.SaveSettings;
            toolTip.SetToolTip(btnSaveSettings, Resources.SaveSettingsTip);

            btnGenerate.Text = Resources.Generate;
            toolTip.SetToolTip(btnGenerate, Resources.GenerateTip);

            lblStatus.Text = Resources.Status;
        }

        /// <summary>
        /// Set the title of the application
        /// </summary>
        private void SetApplicationTitle()
        {
            var appName = Resources.InquiryConfigurationGenerator;
            Utilities.GetAppInfo(out string name, out string ver, out string buildDate, out string buildYear);
            var appTitle = String.Format("{0} [V{1}]", appName, ver);
            Text = appTitle;
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

                _settings.ControllerParameterDefinitionFile = ini["SETTINGS"]["ControllerParameterDefinitionFile"].ToString();
                _settings.OverridePresentationListFile = ini["SETTINGS"]["OverridePresentationListFile"].ToString();

                _settings.Company = ini["COMPANY"]["Company"].ToString();
                _settings.Version = ini["COMPANY"]["Version"].ToString();

                _settings.IncludeFra = ini["COMPANY"]["IncludeFra"].ToBool();
                _settings.IncludeEsn = ini["COMPANY"]["IncludeEsn"].ToBool();
                _settings.IncludeCht = ini["COMPANY"]["IncludeCht"].ToBool();
                _settings.IncludeChn = ini["COMPANY"]["IncludeChn"].ToBool();

                _settings.DisplayLogFileOnCompletion = ini["SETTINGS"]["DisplayLogFileOnCompletion"].ToBool();
                _settings.DisplayOutputFolderOnCompletion = ini["SETTINGS"]["DisplayOutputFolderOnCompletion"].ToBool();
            }
            else
            {
                // File doesn't exist
            }
        }

        /// <summary>
        /// Initialize general-purpose controls 
        /// </summary>
        private void InitGeneralSettings()
        {
            chkDisplayLogFileOnCompletion.Checked = _settings.DisplayLogFileOnCompletion;
            chkDisplayOutputFolderOnCompletion.Checked = _settings.DisplayOutputFolderOnCompletion;
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

            textBox_Enter(txtRootPath, new EventArgs());
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

            textBox_Enter(txtOutputPath, new EventArgs());
        }

        /// <summary>
        /// Click event handler for the 'Datasource Configuration File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnDatasourceConfigurationFileFinder_Click(object sender, EventArgs e)
        {
            var textBox = txtDatasourceConfigurationFile;
            var initialDirectory = string.Empty;
            var currentPath = textBox.Text.Trim();
            if (currentPath.Length > 0)
            {
                initialDirectory = Path.GetDirectoryName(currentPath);
            }

            var dialogTitle = String.Format(Resources.BrowseForTemplate, Resources.DatasourceConfigurationFile);
            var filePath = Utilities.FileBrowser(Resources.ExcelFileFilter,
                                                 dialogTitle,
                                                 initialDirectory);
            textBox.Text = filePath.Length > 0 ? filePath : textBox.Text;
            textBox_Enter(textBox, new EventArgs());
        }

        /// <summary>
        /// Click event handler for the 'Template Configuration File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnTemplateConfigurationFileFinder_Click(object sender, EventArgs e)
        {
            var textBox = txtTemplateConfigurationFile;
            var initialDirectory = string.Empty;
            var currentPath = textBox.Text.Trim();
            if (currentPath.Length > 0)
            {
                initialDirectory = Path.GetDirectoryName(currentPath);
            }

            var dialogTitle = String.Format(Resources.BrowseForTemplate, Resources.TemplateConfigurationFile);
            var filePath = Utilities.FileBrowser(Resources.ExcelFileFilter,
                                                 dialogTitle,
                                                 initialDirectory);
            textBox.Text = filePath.Length > 0 ? filePath : textBox.Text;
            textBox_Enter(textBox, new EventArgs());
        }

        /// <summary>
        /// Click event handler for the 'Controller Parameter Definition File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnControllerParameterDefinitionFileFinder_Click(object sender, EventArgs e)
        {
            var textBox = txtControllerParameterDefinitionFile;
            var initialDirectory = string.Empty;
            var currentPath = textBox.Text.Trim();
            if (currentPath.Length > 0)
            {
                initialDirectory = Path.GetDirectoryName(currentPath);
            }

            var dialogTitle = String.Format(Resources.BrowseForTemplate, Resources.ControllerParameterDefinitionFile);
            var filePath = Utilities.FileBrowser(Resources.JSONFileFilter,
                                                 dialogTitle,
                                                 initialDirectory);
            textBox.Text = filePath.Length > 0 ? filePath : textBox.Text;
            textBox_Enter(textBox, new EventArgs());
        }

        /// <summary>
        /// Click event handler for the 'Override Presentation List File Finder' button
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void btnOverridePresentationListFinder_Click(object sender, EventArgs e)
        {
            var textBox = txtOverridePresentationList;
            var initialDirectory = string.Empty;
            var currentPath = textBox.Text.Trim();
            if (currentPath.Length > 0)
            {
                initialDirectory = Path.GetDirectoryName(currentPath);
            }

            var dialogTitle = String.Format(Resources.BrowseForTemplate, Resources.OverridePresentationListFile);
            var filePath = Utilities.FileBrowser(Resources.JSONFileFilter,
                                                 dialogTitle,
                                                 initialDirectory);
            textBox.Text = filePath.Length > 0 ? filePath : textBox.Text;
            textBox_Enter(textBox, new EventArgs());
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
            txtOutputPath.Text = _settings.OutputPath;
            txtDatasourceConfigurationFile.Text = _settings.DatasourceConfigurationFile;
            txtTemplateConfigurationFile.Text = _settings.TemplateConfigurationFile;
            txtOverridePresentationList.Text = _settings.OverridePresentationListFile;
            txtControllerParameterDefinitionFile.Text = _settings.ControllerParameterDefinitionFile;
            txtCompany.Text = _settings.Company;
            txtVersion.Text = _settings.Version;
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

            ini["SETTINGS"]["TemplateConfigurationFile"] = _settings.TemplateConfigurationFile;
            ini["SETTINGS"]["TemplateConfigurationFile"] = _settings.TemplateConfigurationFile;
            ini["SETTINGS"]["ControllerParameterDefinitionFile"] = _settings.ControllerParameterDefinitionFile;
            ini["SETTINGS"]["OverridePresentationListFile"] = _settings.OverridePresentationListFile;

            ini["SETTINGS"]["DisplayOutputFolderOnCompletion"] = _settings.DisplayOutputFolderOnCompletion;
            ini["SETTINGS"]["DisplayLogFileOnCompletion"] = _settings.DisplayLogFileOnCompletion;

            ini.Save(iniFilePath);

            Utilities.DisplaySuccessMessage(Resources.SettingsSavedSuccessfully);
        }

        /// <summary>
        /// Transfer current form data to the Settings object
        /// </summary>
        private void PopulateSettingsFromForm()
        {
            _settings.Company = txtCompany.Text.Trim();
            _settings.Version = txtVersion.Text.Trim();

            _settings.IncludeFra = chkLanguageFra.Checked;
            _settings.IncludeEsn = chkLanguageEsn.Checked;
            _settings.IncludeCht = chkLanguageCht.Checked;
            _settings.IncludeChn = chkLanguageChn.Checked;

            _settings.Option = GetSelectedOption();
            _settings.RootPath = txtRootPath.Text.Trim();
            _settings.OutputPath = txtOutputPath.Text.Trim();
            _settings.SQLScriptName = txtSQLScriptName.Text.Trim();
            _settings.DatasourceConfigurationFile = txtDatasourceConfigurationFile.Text.Trim();
            _settings.TemplateConfigurationFile = txtTemplateConfigurationFile.Text.Trim();
            _settings.ControllerParameterDefinitionFile = txtControllerParameterDefinitionFile.Text.Trim();
            _settings.OverridePresentationListFile = txtOverridePresentationList.Text.Trim();

            _settings.DisplayOutputFolderOnCompletion = chkDisplayOutputFolderOnCompletion.Checked;
            _settings.DisplayLogFileOnCompletion = chkDisplayLogFileOnCompletion.Checked;
        }

        /// <summary>
        /// Get the currently selected Option value
        /// </summary>
        /// <returns>The string representation of the currently selected option</returns>
        private string GetSelectedOption()
        {
            if (btnOptionAdhoc.BackColor == Constants.OptionButton_Adhoc_Selected_BackgroundColor) return OptionTypeEnum.Adhoc.ToString();
            if (btnOptionCrm.BackColor == Constants.OptionButton_CRM_Selected_BackgroundColor) return OptionTypeEnum.CRM.ToString();
            if (btnOptionInquiry.BackColor == Constants.OptionButton_Inquiry_Selected_BackgroundColor) return OptionTypeEnum.Inquiry.ToString();
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

        /// <summary>
        /// Focus event handler for text boxes
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The Event Arguments</param>
        private void textBox_Enter(object sender, EventArgs e)
        {
            // Clear any error conditions
            var control = sender as BorderedTextBox;
            errorProvider.SetError(control, string.Empty);
            control.SetError(false);
        }

        /// <summary>
        /// Method to update the progress bar value
        /// </summary>
        /// <param name="percentage">The percentage to set on the progress bar</param>
        private void SetProgress(int percentage)
        {
            progressBar.Value = percentage > 100 ? 100 : percentage;
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

        private void txtControllerParameterDefinitionFile_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        private void txtOverridePresentationList_Validating(object sender, CancelEventArgs e) => ValidateControl(sender, e);

        /// <summary>
        /// Do the actual field validations
        /// Note: Currently, all controls checked are text boxes.
        /// </summary>
        /// <param name="sender">The control that initiated the event</param>
        /// <param name="e">The CancelEventArgs object</param>
        /// <returns>true = field is valid | false = field is NOT valid</returns>
        private bool ValidateControl(object sender, CancelEventArgs e)
        {
            var isValid = true;
            var control = sender as BorderedTextBox;
            var errorText = string.Empty;
            var validationRules = new List<ValidationRule>();

            var username = txtUser.Text.Trim();
            var password = txtPassword.Text.Trim();
            var company = txtCompany.Text.Trim();
            var version = txtVersion.Text.Trim();

            var msg = string.Empty;
            switch (control.Name.Trim())
            {
                case "txtUser":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.User);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    msg = String.Format(Resources.IsValidUsername, Resources.User);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidUsername, msg));
                    break;

                case "txtPassword":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.Password);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    msg = String.Format(Resources.IsValidPassword, Resources.Password);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidPassword, msg));
                    break;

                case "txtCompany":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.Company);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    break;

                case "txtVersion":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.Version);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    break;

                case "txtRootPath":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.RootPath);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    break;

                case "txtOutputPath":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.OutputPath);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    errorText = Resources.OutputPathIsRequired;
                    break;

                case "txtSQLScriptName":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.SQLScriptName);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    errorText = Resources.SQLScriptNameIsRequired;
                    break;

                case "txtDatasourceConfigurationFile":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.DatasourceConfigurationFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    msg = String.Format(Resources.IsValidFileTemplate, Resources.DatasourceConfigurationFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidFile, msg));
                    break;

                case "txtTemplateConfigurationFile":
                    msg = String.Format(Resources.IsRequiredTemplate, Resources.TemplateConfigurationFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.RequiredField, msg));
                    msg = String.Format(Resources.IsValidFileTemplate, Resources.TemplateConfigurationFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidFile, msg));
                    break;
                
                case "txtControllerParameterDefinitionFile":
                    msg = String.Format(Resources.IsValidFileTemplate, Resources.ControllerParameterDefinitionFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidFile, msg));
                    break;

                case "txtOverridePresentationList":
                    msg = String.Format(Resources.IsValidFileTemplate, Resources.OverridePresentationListFile);
                    validationRules.Add(new ValidationRule(ValidationRuleEnum.ValidFile, msg));
                    break;
            }

            // Now do the actual checking
            e.Cancel = false;
            var controlText = string.Empty;
            foreach (var rule in validationRules)
            {
                controlText = control.Text.Trim();
                if (rule.Rule == ValidationRuleEnum.RequiredField)
                {
                    if (controlText.Length == 0)
                    {
                        errorProvider.SetError(control, rule.Message);
                        _validationErrors.Add(rule.Message);
                        control.SetError(true);
                        e.Cancel = true;
                        isValid = false;

                        // No point in checking remaining rules
                        break;
                    }
                    else
                    {
                        errorProvider.SetError(control, string.Empty);
                        control.SetError(false);
                        e.Cancel = false;
                    }
                }
                else if (rule.Rule == ValidationRuleEnum.ValidFile)
                {
                    // Only bother with this check if the control has some text in it.
                    if (controlText.Length > 0)
                    {
                        if (File.Exists(controlText) == false)
                        {
                            errorProvider.SetError(control, rule.Message);
                            _validationErrors.Add(rule.Message);
                            control.SetError(true);
                            e.Cancel = true;
                            isValid = false;
                        }
                        else
                        {
                            errorProvider.SetError(control, string.Empty);
                            control.SetError(false);
                            e.Cancel = false;
                        }
                    }
                }
                else if (rule.Rule == ValidationRuleEnum.ValidUsername)
                {
                    if (Utilities.ValidateCredentials(controlText, password, company, version) == false)
                    {
                        errorProvider.SetError(control, rule.Message);
                        _validationErrors.Add(rule.Message);
                        control.SetError(true);
                        e.Cancel = true;
                        isValid = false;
                    }
                    else
                    {
                        errorProvider.SetError(control, string.Empty);
                        control.SetError(false);
                        e.Cancel = false;
                    }
                }
                else if (rule.Rule == ValidationRuleEnum.ValidPassword)
                {
                    if (Utilities.ValidateCredentials(username, controlText, company, version) == false)
                    {
                        errorProvider.SetError(control, rule.Message);
                        _validationErrors.Add(rule.Message);
                        control.SetError(true);
                        e.Cancel = true;
                        isValid = false;
                    }
                    else
                    {
                        errorProvider.SetError(control, string.Empty);
                        control.SetError(false);
                        e.Cancel = false;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Does the defined rule list for a control contain a specific rule?
        /// </summary>
        /// <param name="ruleList">The list of rules defined for a control</param>
        /// <param name="rule">The rule to look for</param>
        /// <returns>true = rule exists in list | false = rule does not exist in list</returns>
        private bool IsRuleSpecified(List<ValidationRule> ruleList, ValidationRuleEnum rule)
        {
            return ruleList.Where(i => i.Rule == rule).FirstOrDefault() != null;
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

        /// <summary>
        /// Insert a method start line
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        public void LogMethodStart(string methodName)
        {
            txtLogWindow.AppendText(String.Format("Start - {0}()", methodName));
        }

        /// <summary>
        /// Insert a method end line
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        public void LogMethodEnd(string methodName)
        {
            txtLogWindow.AppendText(String.Format("End - {0}()", methodName));
        }
        #endregion
    }
}
