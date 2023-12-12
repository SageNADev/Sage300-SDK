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

#region Namespaces
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.SubclassPrep.Properties;
using MetroFramework.Forms;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.SubclassPrep
{
    /// <summary> UI for Subclassing Wizard </summary>
    public partial class Generation : MetroForm
    {
        #region Private Variables
        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> List of Models by Module </summary>
        private string _lastProcessed = string.Empty;
        #endregion

        #region Delegates
        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        private delegate void ProcessingCallback(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="text">Text for UI</param>
        private delegate void StatusCallback(string fileName, string text);
        #endregion

        #region Public Routines
        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            Localize();
            InitEvents();
            ProcessingSetup(true);
            Processing("");

            Application.UseWaitCursor = false;
        }
        #endregion

        #region Private Routines

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enable">True to enable otherwise false</param>
        private void ProcessingSetup(bool enable)
        {
            btnGenerate.Enabled = enable;
            btnGenerate.Refresh();
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
            lblProcessing.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.ProcessingFile, text);
            lblProcessing.Refresh();
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
        /// <param name="text">Text for UI</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void Status(string fileName, string text)
        {
            // Store last processed
            _lastProcessed = text;
        }

        /// <summary> Update status display </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="text">Text for UI</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void StatusEvent(string fileName, string text)
        {
            var callBack = new StatusCallback(Status);
            Invoke(callBack, fileName, text);
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.WebSubclassPrep;
            lblInformation.Text = Resources.Info;
            btnGenerate.Text = Resources.Generate;
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
            _generation.Process();
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

            // Determine success or failure
            if (string.IsNullOrEmpty(_lastProcessed))
            {
                DisplayMessage(Resources.Success, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                DisplayMessage(string.Format(Resources.Failure, _lastProcessed), MessageBoxIcon.Error);
                // Do not close and allow user to close by (X)
            }
        }

        /// <summary> Generate </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Validate CNA2_SOURCE_ROOT environment variable
            var webSourceRoot = ProcessGeneration.WebSourceRoot();
            if (string.IsNullOrEmpty(webSourceRoot))
            {
                DisplayMessage( string.Format(Resources.SourceRootEnvVarError,
                    ProcessGeneration.Constants.CNA2_SOURCE_ROOT, 
                    ProcessGeneration.Constants.CNA2_SOURCE_ROOT_SUGGESTION), 
                    MessageBoxIcon.Error);
                return;
            }

            // Validate SDK_SOURCE_ROOT environment variable
            var sdkSourceRoot = ProcessGeneration.SDKSourceRoot();
            if (string.IsNullOrEmpty(sdkSourceRoot))
            {
                DisplayMessage(string.Format(Resources.SourceRootEnvVarError,
                    ProcessGeneration.Constants.SDK_SOURCE_ROOT, 
                    ProcessGeneration.Constants.SDK_SOURCE_ROOT_SUGGESTION),
                    MessageBoxIcon.Error);
                return;
            }

            // Start background worker for processing (async)
            ProcessingSetup(false);
            Application.UseWaitCursor = true;
            _lastProcessed = string.Empty;
            wrkBackground.RunWorkerAsync();
        }

        #endregion
    }
}
