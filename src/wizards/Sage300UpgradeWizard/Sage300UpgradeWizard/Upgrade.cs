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

using Sage.CA.SBS.ERP.Sage300.UpgradeWizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace Sage.CA.SBS.ERP.Sage300.UpgradeWizard
{
    /// <summary> UI for Sage 300 Upgrade Wizard </summary>
    public partial class Upgrade : Form
    {

        #region Const string
        const string ImportExportUrl = "https://jthomas903.wordpress.com/2017/01/23/sage-300-javascript-bundle-names/";
        const string PaginationUrl = "https://jthomas903.wordpress.com/2017/01/24/sage-300-optional-resource-files/";
        #endregion

        #region Private Vars

        private int _currentWizardStep;
        private string _destination = "";
        private string _sourceItemsFolder = "";
        private string _destinationWebFolder = "";
        private string _viewsFolder = "";
        private readonly string _templatePath;

        private static readonly StringBuilder _sbLog = new StringBuilder();
        /// <summary> Sage color </summary>
        private readonly Color _sageColor = Color.FromArgb(3, 130, 104);

        #endregion

        #region Constructor

        /// <summary> Upgrade Class </summary>
        /// <param name="destination">Destination Default</param>
        /// <param name="destinationWeb">Destination Web Default</param>
        /// <param name="templatePath">Upgrade Web Items template Path </param>
        public Upgrade(string destination, string destinationWeb, string templatePath)
        {
            InitializeComponent();
            _templatePath = templatePath;
            InitWizardSteps(destination, destinationWeb);
        }

        #endregion

        #region Private Methods/Routines/Events

        #region Toolbar Events

        /// <summary> Next/Upgrade toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Next wizard step or Upgrade if last step</remarks>
        private void btnNext_Click(object sender, EventArgs e)
        {
            _currentWizardStep++;
            if (btnNext.Text == "Next")
            {
                ShowStepInfo();
            }
            else if (btnNext.Text == "Upgrade")
            {
                picProcess.Visible = true;
                lblStepTitle.Text = @"Process Upgrade";
                lblInformation.Text = @"Upgrade ...";
				btnBack.Visible = false;
				btnNext.Visible = false;
                wrkBackground.RunWorkerAsync();
            } 
            else if (btnNext.Text == "Finish")
            {
                Close();
            }
        }

        /// <summary> Back toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Back wizard step</remarks>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (btnBack.Text == @"Show Log")
            {
                var logPath = Path.Combine(_destination, "UpgradeLog.txt");
                System.Diagnostics.Process.Start(logPath);
            }
            else
            {
                _currentWizardStep--;
                ShowStepInfo();
            }
        }

        #endregion

        /// <summary> Initialize wizard steps </summary>
        /// <param name="destination">Destination Default</param>
        /// <param name="destinationWeb">Destination Web Default</param>
        private void InitWizardSteps(string destination, string destinationWeb)
        {
            _currentWizardStep = 0;
            _destination = destination;
            _sourceItemsFolder = Path.GetDirectoryName(_templatePath);
            _destinationWebFolder = Directory.GetDirectories(_destination).FirstOrDefault(dir => dir.ToLower().Contains(".web"));

            lblInformation.ForeColor = _sageColor;
            picProcess.Visible = false;
            lnkBlog.Visible = false;
            ShowStepInfo();
        }

        /// <summary>
        /// Show wizard step information
        /// </summary>
        private void ShowStepInfo()
        {
            btnBack.Visible = (_currentWizardStep > 0);
            btnNext.Text = (_currentWizardStep == 3) ? "Upgrade" : "Next";
            var format = (_currentWizardStep == 0) ? "{1}" : "Step {0} - {1}";
            lblStepTitle.Text = string.Format(format, _currentWizardStep, Info.titles[_currentWizardStep]);
            lblInformation.Text = Info.messages[_currentWizardStep];
        }

        /// <summary>
        /// Process PU2 Upgrade
        /// </summary>
        private void ProcessUpgrade()
        {
            SyncWebFiles();
            UpgradeAccpacReference();
            WriteLogFile();
        }

        /// <summary>
        /// Write log file to upgrade solution folder
        /// </summary>
        private void WriteLogFile()
        {
            var logFilePath = Path.Combine(_destination, "UpgradeLog.txt");
            File.WriteAllText(logFilePath, _sbLog.ToString());
        }

        /// <summary>
        /// Synchronization of web project files
        /// </summary>
        private void SyncWebFiles()
        {
            //Update the web files
            var zipFile = Path.Combine(_sourceItemsFolder, "Web.zip");
            var sourceWebFolder = Path.Combine(_sourceItemsFolder, "Web");

            if (Directory.Exists(sourceWebFolder))
            {
                Directory.Delete(sourceWebFolder, true);            
            }

            ZipFile.ExtractToDirectory(zipFile, sourceWebFolder);
            _sbLog.AppendLine(DateTime.Now + " -- Synchronize web files --");
            DirectoryCopy(sourceWebFolder, _destinationWebFolder);
            _sbLog.AppendLine(DateTime.Now + " -- End of synchronize web files --");
            _sbLog.AppendLine("");
        }


        /// <summary>
        /// Upgrade project reference to use new verion Accpac.Net
        /// </summary>
        private void UpgradeAccpacReference()
        {
            //Copy new AccpacDotNetVersion.props
            var file = Path.Combine(_destinationWebFolder, "AccpacDotNetVersion.props");
            var srcFilePath = Path.Combine(_sourceItemsFolder, "AccpacDotNetVersion.props");
            File.Copy(srcFilePath, file, true);
            _sbLog.AppendLine(DateTime.Now + " Update AccpacDotNetVersion.props file");
        }

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            tbrMain.Enabled = enableToolbar;
            tbrMain.Refresh();
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            tbrMain.Refresh();
        }

        /// <summary> Background worker started event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker will run process</remarks>
        private void wrkBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessUpgrade();
        }

        /// <summary> Background worker completed event </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Background worker has completed process</remarks>
        private void wrkBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessingSetup(true);
            Processing("");
            splitBase.Panel2.Refresh();

            // Display final step
            picProcess.Visible = false;
            lblStepTitle.Text = @"Upgrade Completed";
            lblInformation.Text = Info.messages[_currentWizardStep];
            btnNext.Visible = true;
            btnNext.Text = @"Finish";
            btnBack.Visible = true;
            btnBack.Text = @"Show Log";
        }

        /// <summary> Add gradient to top panel</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void splitBase_Panel1_Paint(object sender, PaintEventArgs e)
        {
            FillGradient(e);
        }

        /// <summary> Add gradient to toolbar</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void tbrMain_Paint(object sender, PaintEventArgs e)
        {
            FillGradient(e);
        }

        /// <summary> Add gradient</summary>
        /// <param name="e">Event Args </param>
        private void FillGradient(PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(ClientRectangle,
                                                           _sageColor,
                                                           Color.White,
                                                           LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
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

        /// <summary>
        /// Copy folder and files
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        private static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                var filePath = Path.Combine(destDirName, file.Name);
                file.CopyTo(filePath, true);
                _sbLog.AppendLine(DateTime.Now + " Add/Replace file " + filePath);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                DirectoryCopy(subdir.FullName, Path.Combine(destDirName, subdir.Name));
            }
        }

        #endregion
    }    
}
