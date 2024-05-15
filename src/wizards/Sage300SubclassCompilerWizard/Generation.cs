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

#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard.Properties;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard
{
    /// <summary> UI for Subclassing Wizard </summary>
    public partial class Generation : Form
    {
        #region Private Variables

        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> List of Configurations </summary>
        private readonly List<Configuration> _configurations = new List<Configuration>();

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        /// <summary> Compiler </summary>
        private Compiler _compiler;

        /// <summary> List of Compilers by Type </summary>
        private Dictionary<string, List<Compiler>> _compilers = new Dictionary<string, List<Compiler>>();

        /// <summary> Failure Message </summary>
        private string _failureMessage;

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
            // Localize screen, init events, set busy cursor
            InitializeComponent();
            Localize();
            InitEvents();
            Application.UseWaitCursor = true;

            // Create projects from zips, gather any configs on machine
            InitDisplay(true, Resources.SplashMessage, false);
            ProcessGeneration.CreateProjects();
            _configurations = ProcessGeneration.GetExistingConfigurations();

            // Validate if compilers exist on machine otherwise exit
            InitDisplay(false, Resources.CompilerMessage, false);
            ProcessGeneration.GetCompilers(_compilers);
            if (_compilers.Count == 0 || !SelectCompiler())
            {
                DisplayMessage(Resources.CompilerError, MessageBoxIcon.Error);
                Close();
            }

            // Re-init display (enables screen)
            InitDisplay(false, "", true);
            Application.UseWaitCursor = false;
        }

        #endregion

        #region Private Routines

        /// <summary> Select compiler </summary>
        /// <remarks>Will choose Visual Studio over .NET SDK</remarks>
        /// <returns>True if compiler found otherwise false</returns>
        private bool SelectCompiler()
        {
            _compiler = null;

            // Any Visual Studios installed?
            if (_compilers.ContainsKey(ProcessGeneration.Constants.VSKey))
            {
                // Get latest (highest) version
                _compiler = _compilers[ProcessGeneration.Constants.VSKey][_compilers[ProcessGeneration.Constants.VSKey].Count - 1];
                return true;
            }

            // Any .NET SDKs installed?
            if (_compilers.ContainsKey(ProcessGeneration.Constants.SDKKey))
            {
                // Get latest (highest) version
                _compiler = _compilers[ProcessGeneration.Constants.SDKKey][_compilers[ProcessGeneration.Constants.SDKKey].Count - 1];

                // Must be at least version 6
                var tmp = _compiler.VersionNum.Split('.');
                if (Convert.ToInt32(tmp[0]) < 6)
                {
                    _compiler = null;
                    return false;
                }
            }
            return true;
        }

        /// <summary> Init display while screen is being loaded </summary>
        /// <param name="loading">True if loading otherwise false</param>
        /// <param name="message">Message to display</param>
        /// <param name="enable">True to enable compile button otherwise false</param>
        private void InitDisplay(bool loading, string message, bool enable)
        {
            // Display message
            Processing(message);
            // If loading the screen, invoke Show method from constructor
            if (loading)
            {
                // Force display
                Show();
            }
            // Enable/disable button and refresh
            ProcessingSetup(enable);
            Refresh();
        }

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            chkIgnoreConfigurations.Enabled = enableToolbar;
            btnCompile.Enabled = enableToolbar;
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
            lblProcessingFile.Text = text;
            lblProcessingFile.Refresh();
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

            // If failure (text != null), save for display
            _failureMessage = text;
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

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.WebSubclassingCompiler;
            lblMessage.Text = Resources.UtilityMessage;
            chkIgnoreConfigurations.Text = Resources.IgnoreConfigurations;
            btnCompile.Text = Resources.Next;
        }

        /// <summary> Compile, etc. </summary>
        private void Compile()
        {
            // Setup display before processing
            ProcessingSetup(false);

            // Establish settings for processing (Validation already ocurred in each step)
            _settings = new Settings 
            {
                Configurations = _configurations,
                Compiler = _compiler,
                IgnoreConfigurations = chkIgnoreConfigurations.Checked
            };

            // Start background worker for processing (async)
            Application.UseWaitCursor = true;
            _failureMessage = string.Empty;
            wrkBackground.RunWorkerAsync(_settings);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
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
            Application.UseWaitCursor = false;
            ProcessingSetup(true);
            Processing("");

            // Processing is complete. Display applicable message
            if (string.IsNullOrEmpty(_failureMessage))
            {
                // Success
                DisplayMessage(Resources.UtilitySuccess, MessageBoxIcon.Exclamation);
            }
            else
            {
                // Failure
                DisplayMessage(string.Format(Resources.UtilityFailure, _failureMessage), MessageBoxIcon.Error);
            }
            // Close the utility
            Close();
        }

        /// <summary> Form is closing - cleanup</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void Generation_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessGeneration.CleanupProjects();
        }

        /// <summary> Compile</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        #endregion

    }
}
