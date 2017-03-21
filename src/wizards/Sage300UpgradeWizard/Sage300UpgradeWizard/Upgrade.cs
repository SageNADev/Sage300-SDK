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
        #region Private Vars

        private int _currentWizardStep;
        private string _destination = "";
        private string _sourceItemsFolder = "";
        private string _destinationWebFolder = "";
        private string _viewsFolder = "";
		private readonly string _templatePath;

		private static readonly  StringBuilder _sbLog =  new StringBuilder();
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
                chkConvert.Visible = false;
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

            chkConvert.ForeColor = _sageColor;
            lblInformation.ForeColor = _sageColor;


            picProcess.Visible = false;
			lnkResxBlog.Visible = false;
            ShowStepInfo();
        }

		/// <summary>
		/// Show wizard step information
		/// </summary>
        private void ShowStepInfo()
        {
            btnBack.Visible = (_currentWizardStep > 0);
            btnNext.Text = (_currentWizardStep == 8) ? "Upgrade" : "Next";
            chkConvert.Visible = (_currentWizardStep == 7);
            var format = (_currentWizardStep == 0) ? "{1}" : "Step {0} - {1}"; 
            lblStepTitle.Text = string.Format(format, _currentWizardStep, Info.titles[_currentWizardStep]);
            lblInformation.Text = Info.messages[_currentWizardStep];
			lnkResxBlog.Visible = (_currentWizardStep == 4);
			lnkResxBlog.Top = 280;
            lblInformation.Height = (_currentWizardStep == 7) ? 140: 444;
            chkConvert.Top = (_currentWizardStep == 7) ? lblInformation.Bottom + 20 : 470 ;
        }

        /// <summary>
        /// Process PU2 Upgrade
        /// </summary>
        private void ProcessUpgrade()
        {
            SyncWebFiles();
            ProcessR2R3Changes();
            UpgradeObsoletedMethods();
            UpgradeMergeIsvProject();
            RemoveDotInBundleName();
			UpgradeAccpacReference();
            ConvertWebProject();
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
			//Update the web files from Sage300c PU2 Web folder
			var zipFile = Path.Combine(_sourceItemsFolder, "Web.zip");
			var sourceWebFolder= Path.Combine(_sourceItemsFolder,"Web");
			if (!Directory.Exists(sourceWebFolder))
			{
				ZipFile.ExtractToDirectory(zipFile, sourceWebFolder);
			}
			
            DirectoryCopy(sourceWebFolder, _destinationWebFolder);
			DeleteFiles(_destinationWebFolder);

            // Update WebForms C# file for report project
            if( Directory.Exists(Path.Combine(_destinationWebFolder, "WebForms")))
            {
                var file = Path.Combine(_destinationWebFolder, @"WebForms\BaseWebPage.cs");
                if (File.Exists(file))
	            {
                    var contents = File.ReadAllText(file);
                    contents = contents.Replace("using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;", "using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;");
                    contents = contents.Replace("Utilities.", "SignOnHelper.");
                    File.WriteAllText(file, contents);
					_sbLog.AppendLine(DateTime.Now + " Replace namespace 'Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities' with 'Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities' in " + file);
					_sbLog.AppendLine(DateTime.Now + " Replace object 'Utilities' with 'SignOnHelper' in " + file);
	            }
            }
        }

        /// <summary>
        /// Delete obsolete files
        /// </summary>
        private void DeleteFiles(string webFolder)
        {
            string[] filePaths = { @"Areas\Core\Scripts\Sage.CA.SBS.ERP.Sage300.Common.CurrencyFormatter.js", @"Areas\Core\Views\Inquiry\Partials\_InquirySecondGrid.cshtml" };
            foreach (var f in filePaths)
            {
                var file = Path.Combine(webFolder, f);
                if(File.Exists(file))
                {
                    File.Delete(file);
                    _sbLog.AppendLine(DateTime.Now + " Delete obsolete file: " + file);
                }
            }
        }
        /// <summary>
        /// R2/R3 Layout Changes
        /// </summary>
        private void ProcessR2R3Changes()
        {
            if (!string.IsNullOrEmpty(_destinationWebFolder))
            {
                var areaFolder = Path.Combine(_destinationWebFolder, "Areas");
                var folderName = Directory.GetDirectories(areaFolder).FirstOrDefault(dir => !(dir.ToLower().Contains("core") || dir.ToLower().Contains("shared")));

				if (string.IsNullOrEmpty(folderName))
				{
					return;
				}
				_viewsFolder = Path.Combine(folderName, "Views");

                if (Directory.Exists(_viewsFolder))
                {
                    var paths = Directory.GetFiles(_viewsFolder, "*.cshtml", SearchOption.AllDirectories);
                    var isFileEdit = false;
                    foreach (var path in paths)
                    {
                        var file = File.ReadAllText(path);
                        try
                        {
                            RefactorLocalizedLayout(ref isFileEdit, ref file, path);
                            RefactorContainer16(ref isFileEdit, ref file, path);
                            RefactorHeadersGroup(ref file, ref isFileEdit, path);
                            RefactorGridButtons(ref file, ref isFileEdit, path);

							RefactorPatternMatch(@"\bcontrols-group no-label\b", "ctrl-group", ref file, ref isFileEdit, path);
                            RefactorPatternMatch(@"\bctrl-group no-label\b", "ctrl-group", ref file, ref isFileEdit, path);
                            RefactorPatternMatch(@"\bcontrols-group\b", "ctrl-group", ref file, ref isFileEdit, path);

                            if (Regex.IsMatch(file, @"\btextarea-group\b"))
                            {
                                if (!Regex.IsMatch(file, @"\btextarea-group xlarge\b"))
                                {
                                    file = Regex.Replace(file, @"\btextarea-group\b", "textarea-group xlarge", RegexOptions.IgnoreCase);
                                    isFileEdit = true;
									_sbLog.AppendLine(DateTime.Now + " Replace 'textarea-group' with 'textarea-group xlarge' in " + file );
                                }
                            }
                            //All wrapper-group should have clearfix
                            if (Regex.IsMatch(file, @"\bwrapper-group\b"))
                            {
                                if (!Regex.IsMatch(file, @"\bwrapper-group clearfix\b"))
                                {
                                    file = Regex.Replace(file, @"\bwrapper-group\b", "wrapper-group clearfix", RegexOptions.IgnoreCase);
                                    isFileEdit = true;
									_sbLog.AppendLine(DateTime.Now + " Replace 'wrapper-group' with 'wrapper-group clearfix' in " + file);
                                }
                            }

                            RefactorFiscalGroup(ref file, ref isFileEdit, path);
                            RefactorButtons(ref file, ref isFileEdit, path);
                            RefactorPatternMatch(@"\bnumeric-group with-checkbox\b", "input-group with-checkbox", ref file, ref isFileEdit, path);
                            RefactorPatternMatch(@"\bgo-group with-checkbox\b", "input-group with-checkbox", ref file, ref isFileEdit, path);
                            RefactorPatternMatch(@"\binput-group no-label\b", "input-group", ref file, ref isFileEdit, path);
                            RefactorAlignRight(ref file, ref isFileEdit, path);
                            RefactorLabelTag(ref file, ref isFileEdit, path);
                        }
                        catch (Exception ex)
                        {
							_sbLog.AppendLine(DateTime.Now + " Error in R2/R3 Layout update: " + ex.Message);
                        }
                        // Update the file if it was modified.
                        if (isFileEdit)
                        {
                            File.WriteAllText(path, file);
                            isFileEdit = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process Obsoleted Methods, like Session lock/unlock method changes
        /// </summary>
        private void UpgradeObsoletedMethods()
        {
            //Define dictionary for adding obsoleted and new methods name 
            var methods = new Dictionary<string, string>();
            methods.Add(".ResetLocks()", ".SessionLock()");
            methods.Add(".Unlock()", ".SessionUnlock()");

            //Replace obsoleted methods in cs file
            var codeFiles = Directory.GetFiles(_destination, "*.cs", SearchOption.AllDirectories);
            foreach (var file in codeFiles)
	        {
		        var strCode = File.ReadAllText(file);
				var bReplace = false;
                
				foreach (var m in methods)
                {
					if (strCode.Contains(m.Key))
					{
						strCode = strCode.Replace(m.Key, m.Value);
						bReplace = true;
						_sbLog.AppendLine(string.Format("Replace {0} with {1} in " + file, m.Key, m.Value));
					}
                }
				if(bReplace) 
				{
					File.WriteAllText(file, strCode);
				}
            }
        }

        /// <summary>
        /// New version MergeISVProject, add parameter to specify whether compile the views and deploy to local Sage 300c application
        /// </summary>
        private void UpgradeMergeIsvProject()
        {
			var sourceFile = Path.Combine(_sourceItemsFolder, "MergeISVProject.exe");
            var destinationFile = Path.Combine(_destinationWebFolder, "MergeISVProject.exe");
            File.Copy(sourceFile, destinationFile, true);
			_sbLog.AppendLine(DateTime.Now + " Upgrade MergeISVProject.exe file in the " + _destinationWebFolder + " folder");
        }

        /// <summary>
        /// Correct incorrect usages with dot in bundle name
        /// </summary>
        private void RemoveDotInBundleName()
        {
            //Remove dot in bundle name in BundleRegistration.cs
            var filePath = Path.Combine(_destinationWebFolder, "BundleRegistration.cs");
            var fileContent = File.ReadAllText(filePath);
			var indexStart = fileContent.IndexOf("/bundles/", StringComparison.CurrentCultureIgnoreCase);
			var indexEnd = fileContent.IndexOf(").Include", StringComparison.CurrentCultureIgnoreCase);
			if (indexStart > 0 && indexEnd > indexStart + 10)
            {
                var bundleName = fileContent.Substring(indexStart + 9, indexEnd - indexStart - 10);
				if (bundleName.Contains("."))
				{
					var replaceName = bundleName.Replace(".", "");
					var updatedFileContent = fileContent.Substring(0, indexStart + 9) + replaceName + fileContent.Substring(indexEnd - 1);
					File.WriteAllText(filePath, updatedFileContent);
					_sbLog.AppendLine(DateTime.Now + " Remove the dot in bundle name in " + filePath);
				}
            }

            //Remove dot in bundle name in all index.cshtml files
            var indexFiles = Directory.GetFiles(_viewsFolder, "index.cshtml", SearchOption.AllDirectories);
            foreach (var file in indexFiles)
	        {
		        var lines = File.ReadAllLines(file);
                var line = lines.FirstOrDefault(l => l.Contains("/bundles/"));
                if (!string.IsNullOrEmpty(line))
	            {
                    var indexLine = Array.IndexOf(lines, line);
                    if(indexLine > 0)
                    {
		                indexStart = line.IndexOf("/bundles/", StringComparison.CurrentCultureIgnoreCase);
						var bundleName = line.Substring(indexStart);
						if (bundleName.Contains("."))
						{
							var replaceLine = line.Substring(0, indexStart) + bundleName.Replace(".", "");
							lines[indexLine] = replaceLine;
							File.WriteAllLines(file, lines);
							_sbLog.AppendLine(DateTime.Now + " Remove the dot in bundle name in " + file);
						}
                    }
                }
            }
        }   

		/// <summary>
		/// Upgrade project reference to use new verion Accpac.Net
		/// </summary>
		private void UpgradeAccpacReference()
		{
			var file = Path.Combine(_destinationWebFolder, "AccpacDotNetVersion.props");
			if (File.Exists(file))
			{
				//Copy new AccpacDotNetVersion.props, update the refrence to this props
				var srcFilePath = Path.Combine(_sourceItemsFolder, "AccpacDotNetVersion.props");
				File.Copy(srcFilePath, file, true);
				var files = Directory.EnumerateFiles(_destination, "*.csproj", SearchOption.AllDirectories);
				foreach (var f in files)
				{
					var lines = File.ReadAllLines(f);
					var isChanged = false;
					var length = lines.Length;
					for (int i = 0; i < length; i++)
					{
						if (lines[i].Contains("ACCPAC.Advantage,"))
						{
							lines[i] = "<Reference Include=\"$(RefAccpacAdvantage)\">";
							isChanged = true;
						}
						if (lines[i].Contains("ACCPAC.Advantage.Types,"))
						{
							lines[i] = "<Reference Include=\"$(RefAccpacAdvantageTypes)\">";
							isChanged = true;
						}
					}
					if (isChanged)
					{
						File.WriteAllLines(f, lines);
						_sbLog.AppendLine(DateTime.Now + " Update the project reference to new Accpac.Net version in project " + file);
					}
				}
			}
			else
			{
				//Update project reference directly, only "ACCPAC.Advantage.dll" version is changed
				var files = Directory.EnumerateFiles(_destination, "*.csproj", SearchOption.AllDirectories);
				foreach (var f in files)
				{
					var fileContent = File.ReadAllText(f);
					if (fileContent.Contains("ACCPAC.Advantage, Version=6.4.0.0"))
					{
						File.WriteAllText(f, fileContent.Replace("ACCPAC.Advantage, Version=6.4.0.0", "ACCPAC.Advantage, Version=6.4.0.20"));
						_sbLog.AppendLine(DateTime.Now + " Update the project reference to new Accpac.Net version in project " + f);
					}
				}
			}	
		}

        /// <summary>
        /// Module name included in web project  
        /// </summary>
        private void ConvertWebProject()
        {
            if (chkConvert.Checked)
            {
                // Get project folder to get company and module name
                var dir = new DirectoryInfo(_destination);
                var dirs = dir.GetDirectories();
                var dirName = dirs.FirstOrDefault(d => d.Name.Contains(".BusinessRepository")).Name;
                if(!string.IsNullOrEmpty(dirName) && dirName.IndexOf('.') > -1)
                {
                    var companyName = dirName.Split('.')[0];
                    var module = dirName.Split('.')[1];
                    
                    // Check whether conatain module name in web project
                    if (!_destinationWebFolder.Contains("."+ module + "."))
	                {
						var webDirName = companyName + ".Web";
						var moduleWebDirName = companyName + "." + module + ".Web";

						// Rename web project folder to contain module
						var newWebFolder = Path.Combine(_destination, moduleWebDirName);
						if (!Directory.Exists(newWebFolder))
						{
							try
							{
								System.Threading.Thread.Sleep(3000);
								Directory.Move(_destinationWebFolder, newWebFolder);
								_sbLog.AppendLine(DateTime.Now + " Rename the web project folder as " + newWebFolder + " to contain module name in " + _destination + " folder");
							}
							catch (Exception ex)
							{
								_sbLog.AppendLine(DateTime.Now + " Error: Rename the web project folder to contain module name " + ex.Message);
								return;
							}
						}

						// Rename web project name to contain module name
						var oldProjFile = Path.Combine(newWebFolder, webDirName + ".csproj");
						if (File.Exists(oldProjFile))
						{
							var newProjFile = Path.Combine(newWebFolder, moduleWebDirName + ".csproj");
							if (!File.Exists(newProjFile))
							{
								try
								{
									File.Move(oldProjFile, newProjFile);
									_sbLog.AppendLine(DateTime.Now + " Rename the web project file name as " + newProjFile + " to contain module name in " + newWebFolder + " folder.");

								}
								catch (Exception ex)
								{
									_sbLog.AppendLine(DateTime.Now + " Error: Rename the web project file name: " + ex.Message);
									return;
								}
							}
						}

						// Replace all files project references and namespace to use new name except obj and log directory files
						var files = Directory.EnumerateFiles(_destination, "*.*", SearchOption.AllDirectories).
							Where(s => s.EndsWith(".cs") || s.EndsWith(".csproj") || s.EndsWith(".sln") || s.EndsWith(".cshtml") || s.EndsWith(".xml") || s.EndsWith(".asax") || s.EndsWith(".aspx"));
						foreach (var file in files)
						{
							if (file.Contains(@"\Logs\") || file.Contains(@"\obj\"))
							{
								continue;
							}
							var fileContent = File.ReadAllText(file);
							if (fileContent.Contains(webDirName))
							{
								File.WriteAllText(file, fileContent.Replace(webDirName, moduleWebDirName));
								_sbLog.AppendLine(DateTime.Now + " Update the web project reference or namespace to contains module name in " + file + " file");
							}
						}
					}
                }
            }
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

        #region "R2/R3 Upgrade Methods"

		/// <summary>
		/// Refactoring LocalizedLayout with Shared.GlobalLayout in Razor View
		/// </summary>
		/// <param name="isFileEdit"></param>
		/// <param name="file"></param>
		/// <param name="path"></param>
        private static void RefactorLocalizedLayout(ref bool isFileEdit, ref string file, string path)
        {
            try
            {
                if (file.Contains("Shared.LocalizedLayout;")) // ";" ensures it doesnt override the change.
                {
                    file = file.Replace("Shared.LocalizedLayout;", "Shared.GlobalLayout;");
                    isFileEdit = true;
					_sbLog.AppendLine(DateTime.Now + " Upgrade to use new Shared.GlobalLayout in " + path);
                }
                else if (file.Contains("Shared.LocalizedLayoutR2;"))
                {
                    file = file.Replace("Shared.LocalizedLayoutR2;", "Shared.GlobalLayout;");
                    isFileEdit = true;
					_sbLog.AppendLine(DateTime.Now + " Upgrade to use new Shared.GlobalLayout in " + path);
                }
                else if (file.Contains("Shared.LocalizedLayoutR3;"))
                {
                    file = file.Replace("Shared.LocalizedLayoutR3;", "Shared.GlobalLayout;");
                    isFileEdit = true;
					_sbLog.AppendLine(DateTime.Now + " Upgrade to use new Shared.GlobalLayout in " + path);
                }
            }
            catch (Exception e)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new Shared.GlobalLayout: " + e.Message);
            }
        }

        /// <summary>
		/// Refactoring Container in Razor View
        /// </summary>
        /// <param name="isFileEdit"></param>
        /// <param name="file"></param>
        /// <param name="path"></param>
        private static void RefactorContainer16(ref bool isFileEdit, ref string file, string path)
        {
            try
            {
                if (Regex.IsMatch(file, @"\bcontainer_16\b"))
                {
                    if (!Regex.IsMatch(file, @"\bcontainer_16 popupWindow\b"))
                    {
                        file = Regex.Replace(file, @"\bcontainer_16\b", "form-screen", RegexOptions.IgnoreCase);
                        isFileEdit = true;
						_sbLog.AppendLine(DateTime.Now + " Replace 'container_16' with 'form-screen' in " + path);
                    }
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to replace 'container_16': " + ex.Message);
            }
        }

        /// <summary>
		/// Refactoring Razor View Headers Group
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isFileEdit"></param>
        /// <param name="path"></param>
        private static void RefactorHeadersGroup(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                string pattern = "header-group";
                var idx = file.IndexOf(pattern, StringComparison.CurrentCultureIgnoreCase); //assuming there will be only 1 header group in a file.
                if (idx != -1)
                {
                    string content = "";
                    content = file.Substring(idx);
                    var containerContent = GetContainerContent(content, "section");
                    var indexOfContainerStart = file.LastIndexOf("<section", idx, StringComparison.CurrentCultureIgnoreCase);
                    var containterStartCont = file.Substring(indexOfContainerStart, idx - indexOfContainerStart);
                    containerContent = containterStartCont + containerContent;

                    if (!string.IsNullOrEmpty(containerContent) && !containerContent.Contains("header-group-1"))
                    {
                        //Get the Header Label
                        string matchPattern = "@Html.SageHeader";
                        List<string> matches = GetRazorPatternMatch(containerContent, matchPattern);
                        if (matches.Count != 1) //In this case we only want one match or the page is written different way.
                            throw new Exception();

                        //Get the remaining Content
                        var remainingContStartIdx = containerContent.IndexOf(matches[0], StringComparison.Ordinal) + matches[0].Length;
                        var remainingContEndIdx = containerContent.LastIndexOf("</section>", StringComparison.Ordinal);
                        var remainingContent = containerContent.Substring(remainingContStartIdx, remainingContEndIdx - remainingContStartIdx);

                        //Contruct header-wrapper
                        var headerWrapper = "<div class=\"header-wrapper\">\n"
                            + "    <div class=\"header-headline\">\n"
                            + "        " + matches[0].Trim() + "\n"
                            + "    </div>\n";

                        //Remove @htmlPartial if it exists and add to header Wrapper
                        string matchPattern2 = "@Html.Partial(Core.OptionsMenu";
                        List<string> matches2 = GetRazorPatternMatch(remainingContent, matchPattern2);
                        if (matches2.Count != 0)
                        {
                            remainingContent = remainingContent.Replace(matches2[0], string.Empty);
							headerWrapper += "    @Html.Partial(Core.OptionsMenu, Model.UserAccess, new ViewDataDictionary{{OptionsMenu.CssBinding, true}, {OptionsMenu.UseLessCss, true}})\n";
                        }

                        if (remainingContent.Trim().Length != 0)
                        {
							headerWrapper += "    <div class=\"header-options\">\n"
                            + remainingContent.Trim() + "\n"
                            + "    </div>\n";
                        }

						headerWrapper += "</div>\n";

                        //required-group
                        var requiredGroupRemainingContent = "";
                        string pattern2 = "required-group";
                        var idx2 = file.IndexOf(pattern2, StringComparison.CurrentCulture); //assuming there will be only 1 required group in a file.
                        if (idx2 != -1)
                        {
                            var contentRequiredGroup = file.Substring(idx2);
							var containerContentRequiredgroup = GetContainerContent(contentRequiredGroup, "section");
                            var indexOfrequiredgroupContainerStart = file.LastIndexOf("<section", idx2, StringComparison.CurrentCulture);
                            var requiredgroupStartCont = file.Substring(indexOfrequiredgroupContainerStart, idx2 - indexOfrequiredgroupContainerStart);
							containerContentRequiredgroup = requiredgroupStartCont + containerContentRequiredgroup;

							var requiredGroupContStartIdx = containerContentRequiredgroup.IndexOf('>') + 1;
							var requiredGroupContEndIdx = containerContentRequiredgroup.LastIndexOf("</section>", StringComparison.CurrentCulture);
							requiredGroupRemainingContent = containerContentRequiredgroup.Substring(requiredGroupContStartIdx, requiredGroupContEndIdx - requiredGroupContStartIdx);

                            //Remove Required-Group from the file
							file = file.Replace(containerContentRequiredgroup, string.Empty);
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new header group in " + path);
                        }

                        //Contruct flag-required
                        string flagRequired = "";
                        if (requiredGroupRemainingContent.Trim().Length != 0)
                        {
							flagRequired = "<div class=\"flag-required\">\n"
                            + requiredGroupRemainingContent.Trim() + "\n"
                            + "</div>\n";
                        }

                        //Contruct Complete Header
                        string refactorHeader = "<header>\n"
                            + "    <section class=\"header-group-1\">\n"
							+ "        " + headerWrapper
							+ "        " + flagRequired
                            + "</section>\n"
                            + "</header>";

                        file = file.Replace(containerContent, refactorHeader);
                        isFileEdit = true;
						_sbLog.AppendLine(DateTime.Now + " Upgrade to use new header group in " + path);
					}
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new header group in : " + ex.Message);
            }
        }

		/// <summary>
		/// Refactoring Grid Buttons in Razor View
		/// </summary>
		/// <param name="file"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorGridButtons(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                string patternGridControlsGroup = "gridcontrols-group";
                for (int idx = 0; ; idx += patternGridControlsGroup.Length)
                {
                    idx = file.IndexOf(patternGridControlsGroup, idx, StringComparison.CurrentCulture);
                    if (idx == -1)
                        break;

                    string gridControlsGroupContainer = "", content = "";
                    var indexOfLessThan = file.LastIndexOf('<', idx); // Index of < just before patternGridControlsGroup to find out what container it is.
                    content = file.Substring(indexOfLessThan);

                    var typeOfContainer = GetContainerType(file, indexOfLessThan);
                    if (typeOfContainer == null) //can't get the container type
                        throw new Exception();

                    string startTag = "<" + typeOfContainer;
                    string endTag = "</" + typeOfContainer + ">";
                    for (int index = 0; ; index += endTag.Length)
                    {
                        var previous = index;
                        index = content.IndexOf(endTag, index, StringComparison.CurrentCulture);
                        if (index == -1)
                            break;

                        var subContent = content.Substring((previous == 0) ? startTag.Length : previous, index - previous + endTag.Length);
                        if (!subContent.Contains("<div"))
                        {
                            gridControlsGroupContainer = content.Substring(0, index + endTag.Length);
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(gridControlsGroupContainer))
                        break;

                    var buttonMatches = GetRazorPatternMatch(gridControlsGroupContainer, "@Html.KoSageButton");
                    var newContainerContent = gridControlsGroupContainer;
                    if (buttonMatches.Count != 0)
                    {
                        foreach (string buttonMatch in buttonMatches)
                        {
                            if (buttonMatch.Contains("@Html.KoSageButtonNoName"))
                            {
                                continue;
                            }
                            var newButtonMatch = "";
                            var valueAttribute = GetRazorAttributeMatch(buttonMatch, "@value");
                            if (string.IsNullOrEmpty(valueAttribute))
                            {
                                valueAttribute = GetRazorAttributeMatch(buttonMatch, "value");
                            }
                            var newClassAttribute = "@class = \"btn btn-default btn-grid-control ";
                            if (!string.IsNullOrEmpty(valueAttribute))
                            {
                                var ix = valueAttribute.IndexOf("=", StringComparison.CurrentCulture);
                                var valueAttributeValue = valueAttribute.Substring(ix + 1).Trim();

                                //updateFirstAttributeOfButton
                                var openTag = buttonMatch.IndexOf('(');
                                var seprator = buttonMatch.IndexOf(',');
								var firstAttribute = buttonMatch.Substring(openTag + 1, seprator - openTag - 1);

                                Regex rg = new Regex(firstAttribute);
								newButtonMatch = rg.Replace(buttonMatch, valueAttributeValue, 1);

                                if (newButtonMatch.Contains(valueAttribute + ","))
                                {
                                    newButtonMatch = newButtonMatch.Replace(valueAttribute + ",", string.Empty);
                                }
                                else
                                {
                                    newButtonMatch = newButtonMatch.Replace(valueAttribute, string.Empty);
                                }

								if (valueAttributeValue.ToLower().Contains("edit"))
                                {
                                    newClassAttribute += "btn-edit-column \"";
                                }
								else if (valueAttributeValue.ToLower().Contains("delete"))
                                {
                                    newClassAttribute += "btn-delete \"";
                                }
								else if (valueAttributeValue.ToLower().Contains("add"))
                                {
                                    newClassAttribute += "btn-add \"";
                                }
								else if (valueAttributeValue.ToLower().Contains("refresh"))
                                {
                                    newClassAttribute += "btn-refresh \"";
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }

                            var classAttribute = GetRazorAttributeMatch(buttonMatch, "@class");
                            if (!string.IsNullOrEmpty(classAttribute) && !string.IsNullOrEmpty(newButtonMatch))
                            {
                                newButtonMatch = newButtonMatch.Replace(classAttribute, newClassAttribute);
                            }
                            else
                            {
                                throw new Exception();
                            }

                            newContainerContent = newContainerContent.Replace(buttonMatch, newButtonMatch);
                        }
                    }

                    newContainerContent = Regex.Replace(newContainerContent, @"\bHtml.KoSageButton\b", "Html.KoSageButtonNoName", RegexOptions.IgnoreCase);
                    if (typeOfContainer.Equals("content"))
                    {
                        newContainerContent = Regex.Replace(newContainerContent, @"\bcontent\b", "div", RegexOptions.IgnoreCase);
                    }
                    else if (typeOfContainer.Equals("section"))
                    {
                        newContainerContent = Regex.Replace(newContainerContent, @"\section\b", "div", RegexOptions.IgnoreCase);
                    }

                    file = file.Replace(gridControlsGroupContainer, newContainerContent);
                    isFileEdit = true;
					_sbLog.AppendLine(DateTime.Now + " Upgrade to use new grid buttons in " + path);
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new grid buttons in " + path + ":" + ex.Message);
            }
        }

		/// <summary>
		/// Refactoring FiscalGroup in Razor View
		/// </summary>
		/// <param name="file"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorFiscalGroup(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                string pattern10 = "fiscal-group";
                for (int idx = 0; ; idx += pattern10.Length)
                {
                    idx = file.IndexOf(pattern10, idx, StringComparison.CurrentCulture);
	                if (idx == -1)
	                {
						break;
	                }

                    var content = file.Substring(idx);
                    var divContent = GetContainerContent(content, "div");

	                if (string.IsNullOrEmpty(divContent))
	                {
						break;		                
	                }

                    List<string> buttonMatches = GetRazorPatternMatch(divContent, "@Html.SageTextBox");
                    foreach (var sageTextBoxContent in buttonMatches)
                    {
                        if (sageTextBoxContent.Contains("seperator"))
                        {
                            var newSageTextBoxContent = Regex.Replace(sageTextBoxContent, @"\bseperator\b", "separator", RegexOptions.IgnoreCase);
                            var newDivContent = divContent.Replace(sageTextBoxContent, newSageTextBoxContent);
                            file = file.Replace(divContent, newDivContent);
                            divContent = newDivContent; //This is required for local change to next step in accordance to last step.
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new fiscal group in " + path);
                        }
                    }

                    List<string> buttonMatches2 = GetRazorPatternMatch(divContent, "@Html.KoSageTextBox");
                    foreach (var sageTextBoxContent in buttonMatches2)
                    {
                        if (sageTextBoxContent.Contains("rightbox") && sageTextBoxContent.Contains("w45"))
                        {
                            var newSageTextBoxContent = Regex.Replace(sageTextBoxContent, @"\b w45\b", "", RegexOptions.IgnoreCase);
                            var newDivContent = divContent.Replace(sageTextBoxContent, newSageTextBoxContent);
                            file = file.Replace(divContent, newDivContent);
                            divContent = newDivContent; //This is required for local change to next step in accordance to last step.
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new fiscal group in " + path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new fiscal group in " + path + ":" + ex.Message);
            }
        }

		/// <summary>
		/// Refactoring Buttons in Razor View
		/// </summary>
		/// <param name="file"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorButtons(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                List<string> buttonMatches = GetRazorPatternMatch(file, "@Html.KoSageButton");
                foreach (var buttonContent in buttonMatches)
                {
                    if (buttonContent.Contains("btn-primary"))
                    {
                        if (!Regex.IsMatch(buttonContent, @"\bbtn btn-primary\b"))
                        {
                            var newButtonContent = Regex.Replace(buttonContent, @"\bbtn-primary\b", "btn btn-primary", RegexOptions.IgnoreCase);
                            file = file.Replace(buttonContent, newButtonContent);
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new button element in " + path);
                        }
                    }
                    if (buttonContent.Contains("btn-secondary"))
                    {
                        if (!Regex.IsMatch(buttonContent, @"\bbtn btn-secondary\b"))
                        {
                            var newButtonContent = Regex.Replace(buttonContent, @"\bbtn-secondary\b", "btn btn-secondary", RegexOptions.IgnoreCase);
                            file = file.Replace(buttonContent, newButtonContent);
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new button element in " + path);
                        }
                    }
                    if (buttonContent.Contains("btn-default"))
                    {
                        if (!Regex.IsMatch(buttonContent, @"\bbtn btn-default\b"))
                        {
                            var newButtonContent = Regex.Replace(buttonContent, @"\bbtn-default\b", "btn btn-default", RegexOptions.IgnoreCase);
                            file = file.Replace(buttonContent, newButtonContent);
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new button element in " + path);
						}
                    }
                    if (buttonContent.Contains("btn-tertiary"))
                    {
                        if (!Regex.IsMatch(buttonContent, @"\bbtn btn-tertiary\b"))
                        {
                            var newButtonContent = Regex.Replace(buttonContent, @"\bbtn-tertiary\b", "btn btn-tertiary", RegexOptions.IgnoreCase);
                            file = file.Replace(buttonContent, newButtonContent);
                            isFileEdit = true;
							_sbLog.AppendLine(DateTime.Now + " Upgrade to use new button element in " + path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new button element in " + path + ":" + ex.Message);
            }
        }

		/// <summary>
		/// Refactoring AlignRight in Razor Wiew
		/// </summary>
		/// <param name="file"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorAlignRight(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                List<string> matchesNumericBox = GetRazorPatternMatch(file, "@Html.KoSageNumericBoxFor");
                List<string> matchesTexBox = GetRazorPatternMatch(file, "@Html.KoSageTextBoxFor");
                List<string> matchesTexBox2 = GetRazorPatternMatch(file, "@Html.SageTextBoxFor");
                List<string> matchesTexBox3 = GetRazorPatternMatch(file, "@Html.SageTextBox");
                List<string> matchesTexBox4 = GetRazorPatternMatch(file, "@Html.KoSageTextBox");

                var totalMAtches = matchesNumericBox.Concat(matchesTexBox).Concat(matchesTexBox2).Concat(matchesTexBox3).Concat(matchesTexBox4).ToList();

                foreach (var match in totalMAtches)
                {
                    var matchClassAttribute = GetRazorAttributeMatch(match, "@class");
                    if (!string.IsNullOrEmpty(matchClassAttribute) && matchClassAttribute.Contains("align-right"))
                    {
                        var newMatchContent = match.Replace("align-right", "numeric");
                        file = file.Replace(match, newMatchContent);
                        isFileEdit = true;
						_sbLog.AppendLine(DateTime.Now + " Upgrade to use new align right css in " + path);
                    }
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new align right css in " + path + " : " + ex.Message);
            }
        }

		/// <summary>
		/// Refactoring Razor View Label Tag
		/// </summary>
		/// <param name="file"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorLabelTag(ref string file, ref bool isFileEdit, string path)
        {
            try
            {
                var matches = GetRazorPatternMatch(file, "@Html.SageLabel");
                foreach (var label in matches)
                {
                    var labelClass = GetRazorAttributeMatch(label, "@class");
					if (labelClass.Contains("left"))
                    {
						var newLabelClass = labelClass.Replace("left", string.Empty);
						var newLabel = label.Replace(labelClass, newLabelClass);
                        file = file.Replace(label, newLabel);
                        isFileEdit = true;
						_sbLog.AppendLine(DateTime.Now + " Upgrade to use new label css in " + path);
                    }
                }
            }
            catch (Exception ex)
            {
				_sbLog.AppendLine(DateTime.Now + " Error: Upgrade to use new label element css in " + path + " : " + ex.Message);
            }
        }

		/// <summary>
		/// Find Specified Razor Match Helper
		/// </summary>
		/// <param name="content"></param>
		/// <param name="patternSage"></param>
		/// <returns></returns>
        private static List<string> GetRazorPatternMatch(string content, string patternSage)
        {
            List<string> matches = new List<string>();

            for (int ix = 0; ; ix += patternSage.Length)
            {
                ix = content.IndexOf(patternSage, ix, StringComparison.CurrentCulture);
                if (ix == -1)
                    break;

                string sageRazorContent = "";
                var divContentSubContent = content.Substring(ix);
				var endIdx = divContentSubContent.IndexOf(')');
				sageRazorContent = divContentSubContent.Substring(0, endIdx + 1);

                matches.Add(sageRazorContent);
            }
            return matches;
        }

		/// <summary>
		///	Get Razor View Attribute Match Content 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="attribute"></param>
		/// <returns></returns>
        private static string GetRazorAttributeMatch(string content, string attribute)
        {
            string sageRazorAttributeContent = "";
            var ix = content.IndexOf(attribute, StringComparison.CurrentCulture);
            if (ix == -1)
                return "";

            var divContentSubContent = content.Substring(ix);
            bool openingTag = false;

            foreach (char c in divContentSubContent)
            {
                if (c.CompareTo('"') == 0)
                {
                    if (!openingTag)
                    {
                        openingTag = true;
                    }
                    else
                    {
                        sageRazorAttributeContent += c;
                        break;
                    }
                }
                else if (c.CompareTo(',') == 0 || c.CompareTo('}') == 0)
                {
                    break;
                }

                sageRazorAttributeContent += c;
            }

            return sageRazorAttributeContent;
        }
		
		/// <summary>
		/// Find Container with Specified Type 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="contType"></param>
		/// <returns></returns>
        private static string GetContainerContent(string content, string contType)
        {
            string contContent = "";
            string contEndTag = "</" + contType + ">"; //Example: </section>

            for (int index = 0; ; index += contEndTag.Length)
            {
                var previous = index;
                index = content.IndexOf(contEndTag, index,StringComparison.CurrentCulture);
	            if (index == -1)
	            {
					break;		            
	            }
                var subContent = content.Substring(previous, index - previous + contEndTag.Length);
                if (!subContent.Contains("<" + contType))
                {
                    contContent = content.Substring(0, index + contEndTag.Length);
                    break;
                }
            }

            return contContent;
        }

        /// <summary>
		/// Get ContainerType in Razor View
        /// </summary>
        /// <param name="file"></param>
        /// <param name="indexOfLessThan"></param>
        /// <returns></returns>
        private static string GetContainerType(string file, int indexOfLessThan)
        {
            var content = file.Substring(indexOfLessThan);
            string typeOfContent;
            switch (content.Split(' ').First().ToLower())
            {
                case "<div":
                    typeOfContent = "div";
                    break;
                case "<content":
                    typeOfContent = "content";
                    break;
                case "<section":
                    typeOfContent = "section";
                    break;
                default:
                    typeOfContent = null;
                    break;
            }
            return typeOfContent;
        }

		/// <summary>
		/// Pattern Match Helper method
		/// </summary>
		/// <param name="matchPattern"></param>
		/// <param name="replaceWord"></param>
		/// <param name="content"></param>
		/// <param name="isFileEdit"></param>
		/// <param name="path"></param>
        private static void RefactorPatternMatch(string matchPattern, string replaceWord, ref string content, ref bool isFileEdit, string path)
        {
            if (Regex.IsMatch(content, matchPattern))
            {
                content = Regex.Replace(content, matchPattern, replaceWord, RegexOptions.IgnoreCase);
                isFileEdit = true;
            }
        }

        #endregion

		private void lnkResxBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.lnkResxBlog.LinkVisited = true;
			// Navigate to a URL.
			System.Diagnostics.Process.Start("https://jthomas903.wordpress.com/2017/01/24/sage-300-optional-resource-files/");
		}
    }
}
