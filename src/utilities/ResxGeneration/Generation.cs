// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sage.CA.SBS.ERP.Sage300.ResxGeneration
{
    /// <summary> UI for Rex Generation Tool </summary>
    public partial class Generation : Form
    {
        #region Private Vars
        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;
       
        /// <summary> Resource information is from a file, entered manually or a combination of both </summary>
        private BindingList<ResourceInfo> _resourceInfo = new BindingList<ResourceInfo>();
        #endregion

        #region Delegates
        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        delegate void ProcessingCallback(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Language</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <param name="rowIndex">Row Index for UI sync</param>
        delegate void StatusCallback(ResourceInfo resourceInfo, string language, ResourceInfo.StatusType statusType, 
            string text, int rowIndex);

        #endregion

        #region Constructor
        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            InitResourceInfo();
            InitEvents();
            ProcessingSetup("", 0, true);
            Processing("");
        }
        #endregion

        #region Private Methods/Routines/Events
        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            _generation = new ProcessGeneration();
            _generation.ProcessingEvent += ProcessingEvent;
            _generation.StatusEvent += StatusEvent;
        }

        #region Toolbar Events
        /// <summary> Proceed toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Process if there is something to process</remarks>
        private void btnProceed_Click(object sender, EventArgs e)
        {
            // Validate that there is something to process
            if (ValidResourceInfo())
            {
                // Build settings and validate that settings have been selected (language(s))
                var settings = _generation.BuildSettings(BuildSettingsString());
                if (ValidSettings(settings))
                {
                    // Add resource info and settings to a dictionary to be passed to processing class
                    var dictionary = new Dictionary<string, object>
                    {
                        {ProcessGeneration.ResourceInfoKey, _resourceInfo},
                        {ProcessGeneration.SettingsKey, settings}
                    };

                    // Setup display before processing
                    ProcessingSetup(Properties.Resources.ResourceFiles, _resourceInfo.Count * settings.Languages.Count, false);

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(dictionary);
                }
                else
                {
                    // No languages were selected
                    DisplayMessage(Properties.Resources.NoLanguagesSelected, MessageBoxIcon.Error);
                }

            }
            else
            {
                // No source files to process
                DisplayMessage(Properties.Resources.NothingToProcess, MessageBoxIcon.Error);
            }
        }

        /// <summary> Exit toolbar button </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        /// <remarks>Exit utility </remarks>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary> Import toolbar button </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        /// <remarks>Import resource info from a text file </remarks>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Properties.Resources.Filter,
                FilterIndex = 1,
                Multiselect = false
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Open the file and assign to resource info which is bound to the grid
                try
                {
                    _resourceInfo = _generation.GetResourceInfo(dialog.FileName);
                    // Assign binding to datasource (two binding)
                    grdResourceInfo.DataSource = _resourceInfo;
                }
                catch (Exception exception)
                {
                    DisplayMessage(exception.Message, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary> Export toolbar button </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        /// <remarks>Export resource info to a text file without status column info </remarks>
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveResourceInfo(false);
        }

        /// <summary> Save toolbar button </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        /// <remarks>Save resource info to a text file with status column info </remarks>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveResourceInfo(true);
        }

        /// <summary> Add a row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            _resourceInfo.Add(new ResourceInfo());
        }

        /// <summary> Delete the current row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdResourceInfo.CurrentRow != null && !grdResourceInfo.CurrentRow.IsNewRow)
            {
                grdResourceInfo.Rows.Remove(grdResourceInfo.CurrentRow);
            }
        }

        /// <summary> Delete all rows toolbar button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRows_Click(object sender, EventArgs e)
        {
            foreach (var row in grdResourceInfo.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow))
            {
                grdResourceInfo.Rows.Remove(row);
            }
        }

        /// <summary> Display Help toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Display wiki link
            System.Diagnostics.Process.Start(Properties.Resources.Browser, Properties.Resources.WikiLink);
        }
       #endregion

        /// <summary> Initialize resource info and modify grid display </summary>
        private void InitResourceInfo()
        {
            // Assign binding to datasource (two binding)
            grdResourceInfo.DataSource = _resourceInfo;

            // Assign widths and localized text
            grdResourceInfo.Columns[ResourceInfo.SourcePathColumn].Width = 290;
            grdResourceInfo.Columns[ResourceInfo.SourcePathColumn].HeaderText = Properties.Resources.SourcePath;

            grdResourceInfo.Columns[ResourceInfo.SourceFileColumn].Width = 100;
            grdResourceInfo.Columns[ResourceInfo.SourceFileColumn].HeaderText = Properties.Resources.SourceFile;

            grdResourceInfo.Columns[ResourceInfo.TargetPathColumn].Width = 290;
            grdResourceInfo.Columns[ResourceInfo.TargetPathColumn].HeaderText = Properties.Resources.TargetPath;

            grdResourceInfo.Columns[ResourceInfo.TargetFileColumn].Width = 215;
            grdResourceInfo.Columns[ResourceInfo.TargetFileColumn].HeaderText = Properties.Resources.TargetFile;

            grdResourceInfo.Columns[ResourceInfo.EngStatusColumn].Width = 35;
            grdResourceInfo.Columns[ResourceInfo.EngStatusColumn].ReadOnly = true;
            grdResourceInfo.Columns[ResourceInfo.EngStatusColumn].HeaderText = Properties.Resources.English;

            grdResourceInfo.Columns[ResourceInfo.FraStatusColumn].Width = 35;
            grdResourceInfo.Columns[ResourceInfo.FraStatusColumn].ReadOnly = true;
            grdResourceInfo.Columns[ResourceInfo.FraStatusColumn].HeaderText = Properties.Resources.French;

            grdResourceInfo.Columns[ResourceInfo.EsnStatusColumn].Width = 35;
            grdResourceInfo.Columns[ResourceInfo.EsnStatusColumn].ReadOnly = true;
            grdResourceInfo.Columns[ResourceInfo.EsnStatusColumn].HeaderText = Properties.Resources.Spanish;

            grdResourceInfo.Columns[ResourceInfo.ChnStatusColumn].Width = 35;
            grdResourceInfo.Columns[ResourceInfo.ChnStatusColumn].ReadOnly = true;
            grdResourceInfo.Columns[ResourceInfo.ChnStatusColumn].HeaderText = Properties.Resources.ChineseSimplified;

            grdResourceInfo.Columns[ResourceInfo.ChtStatusColumn].Width = 35;
            grdResourceInfo.Columns[ResourceInfo.ChtStatusColumn].ReadOnly = true;
            grdResourceInfo.Columns[ResourceInfo.ChtStatusColumn].HeaderText = Properties.Resources.ChineseTraditional;
        }

        /// <summary> Common save resource info to file routine </summary>
        /// <param name="includeStatuses">True to include status columns otherwise false</param>
        private void SaveResourceInfo(bool includeStatuses)
        {
            // Init dialog
            var dialog = new SaveFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                Filter = Properties.Resources.Filter,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Save resource info which is bound to the grid to a text file
                SaveResourceInfo(dialog.FileName, includeStatuses);
            }
        }

        /// <summary> Save resource info to file </summary>
        /// <param name="fileName">File name</param>
        /// <param name="includeStatuses">True to include status columns otherwise false</param>
        private void SaveResourceInfo(string fileName, bool includeStatuses)
        {
            try
            {
                var linesToWrite = _resourceInfo.Select(resourceInfo => resourceInfo.SourcePath + ";" +
                    resourceInfo.SourceFile + ";" +
                    resourceInfo.TargetPath + ";" +
                    resourceInfo.TargetFile + 
                    GetStatuses(resourceInfo, includeStatuses)).ToArray();
                File.WriteAllLines(@fileName, linesToWrite);
            }
            catch
            {
                DisplayMessage(Properties.Resources.ErrorWritingResourceFile, MessageBoxIcon.Error);
            }
        }

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        /// <param name="count">Count for progress bar</param>
        /// <param name="enableToolbar"></param>
        private void ProcessingSetup(string text, int count, bool enableToolbar)
        {
            tbrMain.Enabled = enableToolbar;

            lblProcessing.Text = text;

            proProcessing.Maximum = count;
            proProcessing.Value = 0;
            
            sbrMain.Refresh();
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = text;

            if (proProcessing.Maximum != 0)
            {
                proProcessing.Value++;
            }

            sbrMain.Refresh();
        }

        /// <summary> Update processing display </summary>
        /// <param name="text">Text to display in status bar</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void ProcessingEvent(string text)
        {
            var callBack = new ProcessingCallback(Processing);
            Invoke(callBack, new object[] { text });
        }

        /// <summary> Update status display </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Language</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <param name="rowIndex">Row Index for UI sync</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void Status(ResourceInfo resourceInfo, string language, ResourceInfo.StatusType statusType, 
            string text, int rowIndex)
        {
            // Set status and text into tool tip for cell
            grdResourceInfo.CurrentCell = grdResourceInfo[resourceInfo.SetStatus(language, statusType), rowIndex];
            grdResourceInfo.CurrentCell.ToolTipText = text;

            grdResourceInfo.Refresh();
        }

        /// <summary> Update status display </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="language">Language</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        /// <param name="rowIndex">Row Index for UI sync</param>
        /// <remarks>Invoked from threaded process</remarks>
        private void StatusEvent(ResourceInfo resourceInfo, string language, ResourceInfo.StatusType statusType, 
            string text, int rowIndex)
        {
            var callBack = new StatusCallback(Status);
            Invoke(callBack, new object[] { resourceInfo, language, statusType, text, rowIndex });
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Build settings string </summary>
        /// <returns>Settings string</returns>
        private string BuildSettingsString()
        {
            var retVal = chkOverwrite.Checked + ";";

            // English selected?
            if (chkEnglish.Checked)
            {
                retVal += Properties.Resources.English + ";";
            }

            // French selected?
            if (chkFrench.Checked)
            {
                retVal += Properties.Resources.French + ";";
            }

            // Spanish selected?
            if (chkSpanish.Checked)
            {
                retVal += Properties.Resources.Spanish + ";";
            }

            // Chinese (simplified) selected?
            if (chkSimplifiedChinese.Checked)
            {
                retVal += Properties.Resources.ChineseSimplified + ";";
            }

            // Chinese (traditional) selected?
            if (chkTraditionalChinese.Checked)
            {
                retVal += Properties.Resources.ChineseTraditional + ";";
            }

            // Remove training
            if (retVal.EndsWith(";"))
            {
                retVal = retVal.Substring(0, retVal.Length - 1);
            }

            return retVal;
        }

        /// <summary> Validate the resource info </summary>
        /// <returns>True if valid otherwise false</returns>
        /// <remarks>Resource Info must have something to process</remarks>
        private bool ValidResourceInfo()
        {
            return _generation.ValidResourceInfo(_resourceInfo);
        }

        /// <summary> Validate the settings </summary>
        /// <param name="settings">Settings</param>
        /// <returns>True if valid otherwise false</returns>
        /// <remarks>At least one language must be selected</remarks>
        private bool ValidSettings(Settings settings)
        {
            return _generation.ValidSettings(settings);
        }

        /// <summary> Get status columns </summary>
        /// <param name="resourceInfo">Resource Information</param>
        /// <param name="includeStatuses">True to include status columns otherwise false</param>
        /// <returns>String of status columns</returns>
        private string GetStatuses(ResourceInfo resourceInfo, bool includeStatuses)
        {
            var retVal = string.Empty;

            if (includeStatuses)
            {
                retVal += ";" + resourceInfo.GetStatusType(Properties.Resources.English);
                retVal += ";" + resourceInfo.GetStatusType(Properties.Resources.French);
                retVal += ";" + resourceInfo.GetStatusType(Properties.Resources.Spanish);
                retVal += ";" + resourceInfo.GetStatusType(Properties.Resources.ChineseSimplified);
                retVal += ";" + resourceInfo.GetStatusType(Properties.Resources.ChineseTraditional);
            }

            return retVal;
        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            _generation.Process((Dictionary<string, object>)e.Argument);
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayMessage(Properties.Resources.ProcessingComplete, MessageBoxIcon.Information);
            ProcessingSetup("", 0, true);
            Processing("");
        }
       #endregion

        private void Generation_Load(object sender, EventArgs e)
        {

        }

    }
}
