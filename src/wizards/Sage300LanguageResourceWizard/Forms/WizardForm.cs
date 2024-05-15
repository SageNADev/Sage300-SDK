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
using MetroFramework;
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
        /// Destination folder for the log file
        /// </summary>
        private readonly string _destinationFolder;

        /// <summary>
        /// The selected language
        /// </summary>
        private Language _selectedLanguage = new Language();

        #endregion
        
        #region Private Constants
        private static class Constants
        {
            public const string InvalidLanguageCode = "--";

            public const string LogFileName = "Sage300LanguageResourceWizardLog.txt";

            public static Color StatusBoxForegroundColor = Color.Black;
            public static Color StatusBoxBackgroundColor = Color.White;

            /// <summary>
            /// Mnemonic names for Step indicies
            /// </summary>
            public static class Steps
            {
                public const int Initial = -1;
                public const int Start = 0;
                public const int SelectLanguage = 1;
                public const int Review = 2;
                public const int Closing = 3;
            }
        }
        #endregion

        #region Delegates

        /// <summary> Delegate to update UI with name of the step being processed </summary>
        /// <param name="text">Text for UI</param>
        /// <param name="progressAmount">The new progress value</param>
        private delegate void ProcessingCallback(string text, int progressAmount);

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
			Processing("", 0);

            ResetBackgroundColors();

            SetStatusBoxColors();

            _destinationFolder = GetSolutionFolder(solution);

            // Display first step
            InitialStep();

            progressBar.Hide();
        }
        #endregion

        /// <summary>
        /// Get the solution folder from the solution object
        /// </summary>
        /// <param name="solution">The solution object</param>
        /// <returns></returns>
        private string GetSolutionFolder(Solution2 solution)
        {
            return Path.GetDirectoryName(solution.FullName);
        }

        /// <summary>
        /// Reset panel background colors to transparent
        /// Developer Note: Background colors are set to aid design and development only
        /// </summary>
        private void ResetBackgroundColors()
        {
            splitBase.BackColor = Color.Transparent;
            splitBase.Panel1.BackColor = Color.Transparent;
            splitSteps.Panel2.BackColor = Color.Transparent;
            splitSteps.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Set the foreground and background colors for the status box
        /// </summary>
        private void SetStatusBoxColors()
        {
            textBoxStatus.ForeColor = Constants.StatusBoxForegroundColor;
            textBoxStatus.BackColor = Constants.StatusBoxBackgroundColor;
        }

        /// <summary> Are the prerequisites valid for executing the wizard </summary>
        /// <param name="solution">Solution</param>
        /// <remarks>Solution must be a Sage 300 solution with known projects</remarks>
        /// <returns>True if valid otherwise false</returns>
        public bool ValidPrerequisites(Solution solution)
        {
            // Validate solution
            if (!ValidSolution(solution))
            {
                DisplayMessage(Resources.InvalidSolution, MessageBoxIcon.Error);
                return false;
            }

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
            InitPanel(pnlFinish);
        }

        /// <summary>
        /// Set up the language drop down list
        /// Note: List source: https://www.science.co.il/language/Codes.php
        /// </summary>
        private void InitLanguageList()
        {
            var divider = new String('-', 60);
            List<Language> languages = new List<Language>();
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = "- Select Language -" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ab", Name = "Abkhazian [ab]" });
            languages.Add(new Language { Code = "aa", Name = "Afar [aa]" });
            languages.Add(new Language { Code = "af", Name = "Afrikaans [af]" });
            languages.Add(new Language { Code = "sq", Name = "Albanian [sq]" });
            languages.Add(new Language { Code = "am", Name = "Amharic [am]" });
            languages.Add(new Language { Code = "ar", Name = "Arabic [ar]" });
            languages.Add(new Language { Code = "an", Name = "Aragonese [an]" });
            languages.Add(new Language { Code = "hy", Name = "Armenian [hy]" });
            languages.Add(new Language { Code = "as", Name = "Assamese [as]" });
            languages.Add(new Language { Code = "ae", Name = "Avestan [ae]" });
            languages.Add(new Language { Code = "ay", Name = "Aymara [ay]" });
            languages.Add(new Language { Code = "az", Name = "Azerbaijani [az]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ba", Name = "Bashkir [ba]" });
            languages.Add(new Language { Code = "eu", Name = "Basque [eu]" });
            languages.Add(new Language { Code = "be", Name = "Belarusian [be]" });
            languages.Add(new Language { Code = "bn", Name = "Bengali [bn]" });
            languages.Add(new Language { Code = "bh", Name = "Bihari [bh]" });
            languages.Add(new Language { Code = "bi", Name = "Bislama [bi]" });
            languages.Add(new Language { Code = "bs", Name = "Bosnian [bs]" });
            languages.Add(new Language { Code = "br", Name = "Breton [br]" });
            languages.Add(new Language { Code = "bg", Name = "Bulgarian [bg]" });
            languages.Add(new Language { Code = "my", Name = "Burmese [my]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ca", Name = "Catalan [ca]" });
            languages.Add(new Language { Code = "ch", Name = "Chamorro [ch]" });
            languages.Add(new Language { Code = "ce", Name = "Chechen [ce]" });
            languages.Add(new Language { Code = "cu", Name = "Church Slavic; Slavonic; Old Bulgarian [cu]" });
            languages.Add(new Language { Code = "cv", Name = "Chuvash [cv]" });
            languages.Add(new Language { Code = "kw", Name = "Cornish [kw]" });
            languages.Add(new Language { Code = "co", Name = "Corsican [co]" });
            languages.Add(new Language { Code = "hr", Name = "Croatian [hr]" });
            languages.Add(new Language { Code = "cs", Name = "Czech [cs]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "da", Name = "Danish [da]" });
            languages.Add(new Language { Code = "dv", Name = "Divehi, Dhivehi, Maldivian [dv]" });
            languages.Add(new Language { Code = "nl", Name = "Dutch [nl]" });
            languages.Add(new Language { Code = "dz", Name = "Dzongkha [dz]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "eo", Name = "Esperanto [eo]" });
            languages.Add(new Language { Code = "et", Name = "Estonian [et]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "fo", Name = "Faroese [fo]" });
            languages.Add(new Language { Code = "fj", Name = "Fijian [fj]" });
            languages.Add(new Language { Code = "fi", Name = "Finnish [fi]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "gd", Name = "Gaelic, Scottish Gaelic [gd]" });
            languages.Add(new Language { Code = "gl", Name = "Galician [gl]" });
            languages.Add(new Language { Code = "ka", Name = "Georgian [ka]" });
            languages.Add(new Language { Code = "de", Name = "German [de]" });
            languages.Add(new Language { Code = "el", Name = "Greek, Modern(1453 -) [el]" });
            languages.Add(new Language { Code = "gn", Name = "Guarani [gn]" });
            languages.Add(new Language { Code = "gu", Name = "Gujarati [gu]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ht", Name = "Haitian, Haitian Creole [ht]" });
            languages.Add(new Language { Code = "ha", Name = "Hausa [ha]" });
            languages.Add(new Language { Code = "he", Name = "Hebrew [he]" });
            languages.Add(new Language { Code = "hz", Name = "Herero [hz]" });
            languages.Add(new Language { Code = "hi", Name = "Hindi [hi]" });
            languages.Add(new Language { Code = "ho", Name = "Hiri Motu [ho]" });
            languages.Add(new Language { Code = "hu", Name = "Hungarian [hu]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "is", Name = "Icelandic [is]" });
            languages.Add(new Language { Code = "io", Name = "Ido [io]" });
            languages.Add(new Language { Code = "id", Name = "Indonesian [id]" });
            languages.Add(new Language { Code = "ia", Name = "Interlingua(International Auxiliary Language Association) [ia]" });
            languages.Add(new Language { Code = "ie", Name = "Interlingue [ie]" });
            languages.Add(new Language { Code = "iu", Name = "Inuktitut [iu]" });
            languages.Add(new Language { Code = "ik", Name = "Inupiaq [ik]" });
            languages.Add(new Language { Code = "ga", Name = "Irish [ga]" });
            languages.Add(new Language { Code = "it", Name = "Italian [it]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ja", Name = "Japanese [ja]" });
            languages.Add(new Language { Code = "jv", Name = "Javanese [jv]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "kl", Name = "Kalaallisut [kl]" });
            languages.Add(new Language { Code = "kn", Name = "Kannada [kn]" });
            languages.Add(new Language { Code = "ks", Name = "Kashmiri [ks]" });
            languages.Add(new Language { Code = "kk", Name = "Kazakh [kk]" });
            languages.Add(new Language { Code = "km", Name = "Khmer [km]" });
            languages.Add(new Language { Code = "ki", Name = "Kikuyu, Gikuyu [ki]" });
            languages.Add(new Language { Code = "rw", Name = "Kinyarwanda [rw]" });
            languages.Add(new Language { Code = "ky", Name = "Kirghiz [ky]" });
            languages.Add(new Language { Code = "kv", Name = "Komi [kv]" });
            languages.Add(new Language { Code = "ko", Name = "Korean [ko]" });
            languages.Add(new Language { Code = "kj", Name = "Kuanyama, Kwanyama [kj]" });
            languages.Add(new Language { Code = "ku", Name = "Kurdish [ku]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "lo", Name = "Lao [lo]" });
            languages.Add(new Language { Code = "la", Name = "Latin [la]" });
            languages.Add(new Language { Code = "lv", Name = "Latvian [lv]" });
            languages.Add(new Language { Code = "li", Name = "Limburgan, Limburger, Limburgish [li]" });
            languages.Add(new Language { Code = "ln", Name = "Lingala [ln]" });
            languages.Add(new Language { Code = "lt", Name = "Lithuanian [lt]" });
            languages.Add(new Language { Code = "lb", Name = "Luxembourgish, Letzeburgesch [lb]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "mk", Name = "Macedonian [mk]" });
            languages.Add(new Language { Code = "mg", Name = "Malagasy [mg]" });
            languages.Add(new Language { Code = "ms", Name = "Malay [ms]" });
            languages.Add(new Language { Code = "ml", Name = "Malayalam [ml]" });
            languages.Add(new Language { Code = "mt", Name = "Maltese [mt]" });
            languages.Add(new Language { Code = "gv", Name = "Manx [gv]" });
            languages.Add(new Language { Code = "mi", Name = "Maori [mi]" });
            languages.Add(new Language { Code = "mr", Name = "Marathi [mr]" });
            languages.Add(new Language { Code = "mh", Name = "Marshallese [mh]" });
            languages.Add(new Language { Code = "mo", Name = "Moldavian [mo]" });
            languages.Add(new Language { Code = "mn", Name = "Mongolian [mn]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "na", Name = "Nauru [na]" });
            languages.Add(new Language { Code = "nv", Name = "Navaho, Navajo [nv]" });
            languages.Add(new Language { Code = "nd", Name = "Ndebele, North [nd]" });
            languages.Add(new Language { Code = "nr", Name = "Ndebele, South [nr]" });
            languages.Add(new Language { Code = "ng", Name = "Ndonga [ng]" });
            languages.Add(new Language { Code = "ne", Name = "Nepali [ne]" });
            languages.Add(new Language { Code = "se", Name = "Northern Sami [se]" });
            languages.Add(new Language { Code = "no", Name = "Norwegian [no]" });
            languages.Add(new Language { Code = "nb", Name = "Norwegian Bokmal [nb]" });
            languages.Add(new Language { Code = "nn", Name = "Norwegian Nynorsk [nn]" });
            languages.Add(new Language { Code = "ny", Name = "Nyanja, Chichewa, Chewa [ny]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "oc", Name = "Occitan(post 1500), Provencal [oc]" });
            languages.Add(new Language { Code = "or", Name = "Oriya [or]" });
            languages.Add(new Language { Code = "om", Name = "Oromo [om]" });
            languages.Add(new Language { Code = "os", Name = "Ossetian, Ossetic [os]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "pi", Name = "Pali [pi]" });
            languages.Add(new Language { Code = "pa", Name = "Panjabi [pa]" });
            languages.Add(new Language { Code = "fa", Name = "Persian [fa]" });
            languages.Add(new Language { Code = "pl", Name = "Polish [pl]" });
            languages.Add(new Language { Code = "pt", Name = "Portuguese [pt]" });
            languages.Add(new Language { Code = "ps", Name = "Pushto [ps]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "qu", Name = "Quechua [qu]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "rm", Name = "Raeto - Romance [rm]" });
            languages.Add(new Language { Code = "ro", Name = "Romanian [ro]" });
            languages.Add(new Language { Code = "rn", Name = "Rundi [rn]" });
            languages.Add(new Language { Code = "ru", Name = "Russian [ru]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "sm", Name = "Samoan [sm]" });
            languages.Add(new Language { Code = "sg", Name = "Sango [sg]" });
            languages.Add(new Language { Code = "sa", Name = "Sanskrit [sa]" });
            languages.Add(new Language { Code = "sc", Name = "Sardinian [sc]" });
            languages.Add(new Language { Code = "sr", Name = "Serbian [sr]" });
            languages.Add(new Language { Code = "sn", Name = "Shona [sn]" });
            languages.Add(new Language { Code = "yi", Name = "Sichuan [yi]" });
            languages.Add(new Language { Code = "sd", Name = "Sindhi [sd]" });
            languages.Add(new Language { Code = "si", Name = "Sinhala, Sinhalese [si]" });
            languages.Add(new Language { Code = "sk", Name = "Slovak [sk]" });
            languages.Add(new Language { Code = "sl", Name = "Slovenian [sl]" });
            languages.Add(new Language { Code = "so", Name = "Somali [so]" });
            languages.Add(new Language { Code = "st", Name = "Sotho, Southern [st]" });
            languages.Add(new Language { Code = "su", Name = "Sundanese [su]" });
            languages.Add(new Language { Code = "sw", Name = "Swahili [sw]" });
            languages.Add(new Language { Code = "ss", Name = "Swati [ss]" });
            languages.Add(new Language { Code = "sv", Name = "Swedish [sv]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "tl", Name = "Tagalog [tl]" });
            languages.Add(new Language { Code = "ty", Name = "Tahitian [ty]" });
            languages.Add(new Language { Code = "tg", Name = "Tajik [tg]" });
            languages.Add(new Language { Code = "ta", Name = "Tamil [ta]" });
            languages.Add(new Language { Code = "tt", Name = "Tatar [tt]" });
            languages.Add(new Language { Code = "te", Name = "Telugu [te]" });
            languages.Add(new Language { Code = "th", Name = "Thai [th]" });
            languages.Add(new Language { Code = "bo", Name = "Tibetan [bo]" });
            languages.Add(new Language { Code = "ti", Name = "Tigrinya [ti]" });
            languages.Add(new Language { Code = "to", Name = "Tonga(Tonga Islands) [to]" });
            languages.Add(new Language { Code = "ts", Name = "Tsonga [ts]" });
            languages.Add(new Language { Code = "tn", Name = "Tswana [tn]" });
            languages.Add(new Language { Code = "tr", Name = "Turkish [tr]" });
            languages.Add(new Language { Code = "tk", Name = "Turkmen [tk]" });
            languages.Add(new Language { Code = "tw", Name = "Twi [tw]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "ug", Name = "Uighur [ug]" });
            languages.Add(new Language { Code = "uk", Name = "Ukrainian [uk]" });
            languages.Add(new Language { Code = "ur", Name = "Urdu [ur]" });
            languages.Add(new Language { Code = "uz", Name = "Uzbek [uz]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "vi", Name = "Vietnamese [vi]" });
            languages.Add(new Language { Code = "vo", Name = "Volapuk [vo]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "wa", Name = "Walloon [wa]" });
            languages.Add(new Language { Code = "cy", Name = "Welsh [cy]" });
            languages.Add(new Language { Code = "fy", Name = "Western Frisian [fy]" });
            languages.Add(new Language { Code = "wo", Name = "Wolof [wo]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "xh", Name = "Xhosa [xh]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "yi", Name = "Yiddish [yi]" });
            languages.Add(new Language { Code = "yo", Name = "Yoruba [yo]" });
            languages.Add(new Language { Code = Constants.InvalidLanguageCode, Name = divider });
            languages.Add(new Language { Code = "za", Name = "Zhuang, Chuang [za]" });
            languages.Add(new Language { Code = "zu", Name = "Zulu [zu]" });

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
            _currentWizardStep = Constants.Steps.Initial;

            // Init wizard steps
            _wizardSteps.Clear();

            HidePanels();

            SetupSteps();
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

            AddStep(Resources.Finish_Title,
                    String.Empty,
                    pnlFinish);
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
            if (IsInitialStep())
            {
                lblStatus.Visible = false;
                textBoxStatus.Visible = false;

                SetupButtons();

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
            return _currentWizardStep == Constants.Steps.Initial;
        }

        /// <summary>
        /// Is this the last wizard step?
        /// </summary>
        /// <returns>true : last step | false : not the last step</returns>
        private bool IsLastStep()
        {
            var totalSteps = _wizardSteps.Count;
            return _currentWizardStep == totalSteps - 1;
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
                if (!IsInitialStep() && _currentWizardStep.Equals(totalSteps - 2))
                {
                    // See if there are already resource files for the language specified
                    // If there are, give the user a warning that they will be overwritten

                    var proceed = false;
                    var solutionPath = Path.GetDirectoryName(_solution.FullName);
                    var anyLanguageFiles = Directory.GetFiles(solutionPath, $"*Resx.{_selectedLanguage.Code}.resx", SearchOption.AllDirectories).Count() > 0;
                    if (anyLanguageFiles)
                    {
                        // Language files already exist confirmation dialog
                        var result1 = MetroMessageBox.Show(this,
                                                           String.Format(Resources.LanguageResourcesAlreadyExist_Template, _selectedLanguage.Name),
                                                           Resources.Confirmation,
                                                           MessageBoxButtons.OKCancel,
                                                           MessageBoxIcon.Question);

                        proceed = (result1 == DialogResult.OK);
                    } 
                    else
                    {
                        // General confirmation dialog
                        var result2 = MetroMessageBox.Show(this,
                                                           Resources.AreYouSureYouWishToProceed,
                                                           Resources.Confirmation,
                                                           MessageBoxButtons.OKCancel,
                                                           MessageBoxIcon.Question);

                        proceed = (result2 == DialogResult.OK);
                    }

                    if (proceed)
                    {
                        DisableButtons();

                        // Update the pages step title (Special case)
                        // and then disable them while doing the actual processing
                        var currentStep = _currentWizardStep.ToString("#0");
                        var step = $"{Resources.Step} {currentStep}{Resources.Dash}";
                        lblStepTitle.Text = step + Resources.GeneratingResources;
                        lblReview_ContentTemplate.Text = String.Format(Resources.PleaseWaitWhileResourcesAreGenerated_Template, 
                                                                       _selectedLanguage.Name);

                        ActivateStatusWindow();

                        _settings = new Settings
                        {
                            Solution = _solution,
                            Language = _selectedLanguage
                        };

                        // Start background worker for processing (async)
                        wrkBackground.RunWorkerAsync(_settings);
                    }
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

                    _currentWizardStep++;

                    // Update title, text and panel for the step
                    ShowStep(true);

                    SetupButtons();
                }
            }
        }

        /// <summary>
        /// Display the status label, window and progress bar
        /// </summary>
        private void ActivateStatusWindow()
        {
            lblStatus.Visible = true;
            textBoxStatus.Visible = true;
            progressBar.Show();
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
            if (_currentWizardStep > Constants.Steps.Start)
            {
                if (_currentWizardStep.Equals(totalSteps - 1))
                {
                    var logPath = Path.Combine(_destinationFolder, Constants.LogFileName);
                    System.Diagnostics.Process.Start(logPath);
                    return;
                }

                ShowStep(false);

                _currentWizardStep--;

                ShowStep(true);

                SetupButtons();
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
        /// Setup the buttons (enable/disable/set label text)
        /// </summary>
        private void SetupButtons()
        {
            if (IsInitialStep())
            {
                DisableBackButton();

                btnNext.Text = Resources.Begin;
                EnableNextButton();
                btnNext.Focus();
            }
            else if (_currentWizardStep == Constants.Steps.Start)
            {
                DisableBackButton();

                btnNext.Text = Resources.Begin;
                EnableNextButton();
            }
            else if (_currentWizardStep == Constants.Steps.SelectLanguage)
            {
                EnableBackButton();

                btnNext.Text = Resources.Next;

                if (IsValidLanguageSelected())
                {
                    EnableNextButton();
                } 
                else
                {
                    DisableNextButton();
                }
                
            }
            else if (_currentWizardStep == Constants.Steps.Review)
            {
                EnableBackButton();

                btnNext.Text = Resources.Generate;
                EnableNextButton();
            }
            else if (_currentWizardStep == Constants.Steps.Closing)
            {
                // Display final step
                EnableBackButton();
                btnBack.Text = Resources.ShowLog;

                EnableNextButton();
                btnNext.Text = Resources.Close;
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

        /// <summary>
        /// Disable the Back and Next buttons
        /// </summary>
        private void DisableButtons()
        {
            DisableBackButton();
            DisableNextButton();
        }

        /// <summary>
        /// Show Step Page
        /// </summary>
        /// <param name="visible">true = visible | false = not visible</param>
        private void ShowStep(bool visible)
        {
            SetStepTitleAndDescription();
            if (!IsInitialStep())
            {
                _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
                _wizardSteps[_currentWizardStep].Panel.Visible = visible;

                if (_currentWizardStep == Constants.Steps.Start)
                {
                    lblWelcome_Content.Text = Resources.WelcomeContent;
                }
                else if (_currentWizardStep == Constants.Steps.SelectLanguage)
                {
                    lblLanguage.Text = Resources.Language;
                }
                else if (_currentWizardStep == Constants.Steps.Review)
                {
                    GetSelectedLanguage();

                    var contentText = Resources.Review_Content_Template;
                    contentText = contentText.Replace("{0}", _selectedLanguage.Name);
                    lblReview_ContentTemplate.Text = contentText;
                }
                else if (_currentWizardStep == Constants.Steps.Closing)
                {
                    lblFinish_Content.Text = Resources.FinishContent;
                }
            }
        }

        /// <summary>
        /// Set the current step title and description text
        /// </summary>
        private void SetStepTitleAndDescription()
        {
            if (!IsInitialStep())
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
        /// Write log file to solution folder
        /// </summary>
        private void WriteLogFile()
        {
            var logFilePath = Path.Combine(_destinationFolder, Constants.LogFileName);
            File.WriteAllText(logFilePath, _log.ToString());
        }

        /// <summary>Setup processing display</summary>
        /// <param name="enable">true : enable | false : disable</param>
        private void ProcessingSetup(bool enable)
        {
            pnlButtons.Enabled = enable;
            pnlButtons.Refresh();
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text, int progressCount)
        {
            textBoxStatus.AppendText(Environment.NewLine);
            textBoxStatus.AppendText(text);
            textBoxStatus.Update();

            // Update the progress bar
            SetProgress(progressCount);
        }

        /// <summary>
        /// Method to update the progress bar value
        /// </summary>
        /// <param name="percentage">The percentage to set on the progress bar</param>
        private void SetProgress(int percentage)
        {
            progressBar.Value = percentage > 100 ? 100 : percentage;
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display</param>
        /// <param name="progressAmount">The new progress amount</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void ProcessingEvent(string text, int progressAmount)
        {
            var callBack = new ProcessingCallback(Processing);
            Invoke(callBack, text, progressAmount);
        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            Settings settings = (Settings)e.Argument;
            _upgrade.Process(settings);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingSetup(true);
            Processing("", 100);

            _currentWizardStep++;

            ShowStep(true);

            SetupButtons();

            // Write out log file with upgrade being complete
            WriteLogFile();
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
        #endregion

        /// <summary>
        /// Language combobox change event handler
        /// </summary>
        /// <param name="sender">The event source</param>
        /// <param name="e">The event arguments</param>
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
