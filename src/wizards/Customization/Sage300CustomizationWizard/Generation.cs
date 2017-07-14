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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.CustomizationWizard.Properties;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Sage.CA.SBS.ERP.Sage300.CustomizationWizard
{
    /// <summary> UI for Standalone Web Customization Wizard </summary>
    public partial class Generation : Form
    {
        #region Private Vars

        /// <summary> Process Generation logic </summary>
        private ProcessGeneration _generation;

        /// <summary> Information processed </summary>
        private readonly BindingList<Info> _gridInfo = new BindingList<Info>();

        /// <summary> Dynamic Query Infomation </summary>
        private readonly List<Screen> _screens = new List<Screen>();

        /// <summary> Row index for grid </summary>
        private int _rowIndex = -1;

        /// <summary> Wizard Steps </summary>
        private readonly List<WizardStep> _wizardSteps = new List<WizardStep>();

        /// <summary> Current Wizard Step </summary>
        private int _currentWizardStep;

        /// <summary> All Screens </summary>
        /// <remarks>
        /// Keys: module, module+category, module+category+screen
        /// Values: categories, screens, values
        /// </remarks>
        private Dictionary<string, List<string>> _allScreens;

        /// <summary> Mode Type for Add, Add Above, Add Below, Edit or None </summary>
        private ModeType _modeType = ModeType.None;

        /// <summary> Clicked Control Node </summary>
        private TreeNode _clickedControlTreeNode;

        /// <summary> Clicked Screen Node </summary>
        private TreeNode _clickedScreenTreeNode;

        /// <summary> Menu Item for Add Control </summary>
        private readonly MenuItem _addControlMenuItem = new MenuItem(Resources.AddControl);

        /// <summary> Menu Item for Insert Control Above </summary>
        private readonly MenuItem _insertControlAboveMenuItem = new MenuItem(Resources.InsertControlAbove);

        /// <summary> Menu Item for Insert Control Below </summary>
        private readonly MenuItem _insertControlBelowMenuItem = new MenuItem(Resources.InsertControlBelow);

        /// <summary> Menu Item for Edit Control </summary>
        private readonly MenuItem _editControlMenuItem = new MenuItem(Resources.EditControl);

        /// <summary> Menu Item for Delete Control </summary>
        private readonly MenuItem _deleteControlMenuItem = new MenuItem(Resources.DeleteControl);

        /// <summary> Menu Item for Add Screen </summary>
        private readonly MenuItem _addScreenMenuItem = new MenuItem(Resources.AddScreen);

        /// <summary> Menu Item for Edit Screen </summary>
        private readonly MenuItem _editScreenMenuItem = new MenuItem(Resources.EditScreen);

        /// <summary> Menu Item for Delete Screen </summary>
        private readonly MenuItem _deleteScreenMenuItem = new MenuItem(Resources.DeleteScreen);

        /// <summary> Menu Item for Delete Screens </summary>
        private readonly MenuItem _deleteScreensMenuItem = new MenuItem(Resources.DeleteScreens);

        /// <summary> Context Menu </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();

        /// <summary> Settings for Processing </summary>
        private Settings _settings;

        #endregion

        #region Private Constants

        /// <summary> New Control Text </summary>
        private const string NewControlText = "NewControl";

        /// <summary> New Screen Text </summary>
        private const string NewScreenText = "NewScreen";

        /// <summary> Panel Name for pnlCreateEdit </summary>
        private const string PanelCreateEdit = "pnlCreateEdit";

        /// <summary> Panel Name for pnlScreens </summary>
        private const string PanelScreens = "pnlScreens";

        /// <summary> Panel Name for pnlGenerated </summary>
        private const string PanelGenerated = "pnlGenerated";

        /// <summary> Panel Name for pnlGenerate </summary>
        private const string PanelGenerate = "pnlGenerate";

        /// <summary> Panel Name for pnlControls </summary>
        private const string PanelControls = "pnlControls";

        /// <summary> Control Type TabPage </summary>
        private const string ControlTypeTabPage = "TabPage";

        /// <summary> Control Type Panel </summary>
        private const string ControlTypePanel = "Panel";

        /// <summary> Splitter Distance </summary>
        private const int SplitterDistance = 415;

        #endregion

        #region Private Enums

        /// <summary>
        /// Enum for Clear Types
        /// </summary>
        private enum ClearType
        {
            /// <summary> Clear All Controls </summary>
            All = 0,

            /// <summary> Clear Module and downstream </summary>
            Module = 1,

            /// <summary> Clear Category and downstream </summary>
            Category = 2,

            /// <summary> Clear Screen and downstream </summary>
            Screen = 3,

            /// <summary> Clear remaining </summary>
            Other = 4
        }

        /// <summary>
        /// Enum for Mode Types
        /// </summary>
        private enum ModeType
        {
            /// <summary> No Mode </summary>
            None = 0,

            /// <summary> Add Mode </summary>
            Add = 1,

            /// <summary> Add Above Mode </summary>
            AddAbove = 2,

            /// <summary> Add Below Mode </summary>
            AddBelow = 3,

            /// <summary> Edit Mode</summary>
            Edit = 4
        }

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
            InitializeComponent();
            Localize();
            InitWizardSteps();
            InitInfo();
            InitScreenSource();
            InitScreens();
            InitControls();
            InitEvents();
            ProcessingSetup(true);
            Processing("");
        }

        #endregion

        #region Private Routines

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

        /// <summary> Setup processing display in status bar </summary>
        /// <param name="enableToolbar">True to enable otherwise false</param>
        private void ProcessingSetup(bool enableToolbar)
        {
            pnlButtons.Enabled = enableToolbar;
            pnlButtons.Refresh();
        }

        /// <summary> Initialize events for process generation class </summary>
        private void InitEvents()
        {
            // Processing Events
            _generation = new ProcessGeneration();
            _generation.ProcessingEvent += ProcessingEvent;
            _generation.StatusEvent += StatusEvent;

            // Control Step Events
            _addControlMenuItem.Click += AddControlMenuItemOnClick;
            _insertControlAboveMenuItem.Click += InsertControlAboveMenuItemOnClick;
            _insertControlBelowMenuItem.Click += InsertControlBelowMenuItemOnClick;
            _editControlMenuItem.Click += EditControlMenuItemOnClick;
            _deleteControlMenuItem.Click += DeleteControlMenuItemOnClick;

            // Screen Step Events
            _addScreenMenuItem.Click += AddScreenMenuItemOnClick;
            _editScreenMenuItem.Click += EditScreenMenuItemOnClick;
            _deleteScreenMenuItem.Click += DeleteScreenMenuItemOnClick;
            _deleteScreensMenuItem.Click += DeleteScreensMenuItemOnClick;

            // No WhiteSpace Events
            txtControlName.KeyPress += NoWhiteSpaceKeyPress;
            txtControlBinding.KeyPress += NoWhiteSpaceKeyPress;
            txtPlacementID.KeyPress += NoWhiteSpaceKeyPress;
            txtHeaderPlacementID.KeyPress += NoWhiteSpaceKeyPress;
            txtDetailPlacementID.KeyPress += NoWhiteSpaceKeyPress;
            txtMaxLength.KeyPress += NoWhiteSpaceKeyPress;
            txtControlCols.KeyPress += NoWhiteSpaceKeyPress;
            txtControlRows.KeyPress += NoWhiteSpaceKeyPress;
            txtFinderTextID.KeyPress += NoWhiteSpaceKeyPress;
        }

        /// <summary> Delete control</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteControlMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Delete tree node
            DeleteControlNode(_clickedControlTreeNode);
        }

        /// <summary> Delete Control Node</summary>
        /// <param name="treeNode">Tree Node to delete </param>
        private void DeleteControlNode(TreeNode treeNode)
        {
            // Remove from XDocument first
            var node = (XElement)treeNode.Tag;
            var screenNode = GetScreenNode(node);
            node.Remove();

            // Remove the tree node
            treeNode.Remove();

            // Control delete so ControlConfiguration may need to be updated
            UpdateScreen(screenNode);
        }

        /// <summary> Delete Screen Node</summary>
        /// <param name="treeNode">Tree Node to delete </param>
        private void DeleteScreenNode(TreeNode treeNode)
        {
            // Remove from tree controls first
            var screen = (Screen) treeNode.Tag;

            // Get key for tree control node
            var key = screen.TargetScreen;

            // Find and remove in tree controls
            foreach (TreeNode node in treeControls.Nodes[0].Nodes)
            {
                if (!node.Name.Equals(key))
                {
                    continue;
                }

                var element = (XElement)node.Tag;
                element.Remove();
                node.Remove();
            }

            // Remove from screens
            _screens.Remove(screen);

            // Remove the tree node
            treeNode.Remove();
        }

        /// <summary> Find Nodes</summary>
        /// <param name="key">Key of nodes to find </param>
        /// <returns>Nodes found</returns>
        private IEnumerable<TreeNode> FindNodes(string key)
        {
            return treeControls.Nodes.Find(key, true);
        }

        /// <summary> Edit control</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void EditControlMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Setup items for edit of control
            ControlSetup(_clickedControlTreeNode, ModeType.Edit);
        }

        /// <summary> Add control</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void AddControlMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new element
            var element = NewControlElement();

            // Build new tree node
            var treeNode = NewControlTreeNode(element);

            // Add to node clicked
            var node = (XElement)_clickedControlTreeNode.Tag;
            node.Add(element);
            _clickedControlTreeNode.Nodes.Add(treeNode);

            // Setup items for new control
            ControlSetup(treeNode, ModeType.Add);
        }

        /// <summary> Control Setup</summary>
        /// <param name="treeNode">Tree node</param>
        /// <param name="modeType">Mode Type (Add, Insert Above, insert below)</param>
        private void ControlSetup(TreeNode treeNode, ModeType modeType)
        {
            // If not edit mode
            if (!modeType.Equals(ModeType.Edit))
            {
                // Expand clicked node
                _clickedControlTreeNode.ExpandAll();

                // Add to tree
                treeControls.SelectedNode = treeNode;
            }

            // Set color of node
            SetNodeColor(treeNode, true);

            // Disable tree control and related
            EnableControlsControls(false);

            // Set mode type and clear controls
            _modeType = modeType;
            ClearControlControls();

            // Enable control controls
            EnableControlControls(true);

            // If edit mode
            if (modeType.Equals(ModeType.Edit))
            {
                // Load controls from attributes
                LoadControlControls();
            }

            // Set focus to control name
            txtControlName.Focus();
        }

        /// <summary> Screen Setup</summary>
        /// <param name="treeNode">Tree node</param>
        /// <param name="modeType">Mode Type (Add)</param>
        private void ScreenSetup(TreeNode treeNode, ModeType modeType)
        {
            // If not edit mode
            if (!modeType.Equals(ModeType.Edit))
            {
                // Expand clicked node
                _clickedScreenTreeNode.ExpandAll();

                // Add to tree
                treeScreens.SelectedNode = treeNode;
            }

            // Set color of node
            SetNodeColor(treeNode, true);

            // Disable tree control and related
            EnableScreensControls(false);

            // Set mode type and clear controls
            _modeType = modeType;
            ClearScreenControls(ClearType.All);

            // Enable screen controls
            EnableScreenControls(true);

            // If edit mode
            if (modeType.Equals(ModeType.Edit))
            {
                // Load controls from screen
                LoadScreenControls();
            }

            // Set focus to module id
            cboModuleId.Focus();
        }

        /// <summary> New Control Element</summary>
        private static XElement NewControlElement()
        {
            // Build new element
            var element = new XElement(NewControlText);
            element.Add(new XAttribute(ProcessGeneration.AttributeId, NewControlText));
            element.Add(new XAttribute(ProcessGeneration.AttributeType, ""));

            return element;
        }

        /// <summary> New control tree node</summary>
        /// <param name="element">XDocument Element</param>
        private static TreeNode NewControlTreeNode(XElement element)
        {
            // Build new tree node
            var name = BuildControlNodeName(element);
            var text = BuildControlText(element); 
            var treeNode = new TreeNode(text)
            {
                Tag = element,
                Name = name
            };

            return treeNode;
        }

        /// <summary> New Screen</summary>
        private static Screen NewScreen()
        {
            // Build new screen
            return new Screen {TargetScreen = NewScreenText};
        }

        /// <summary> New screen tree node</summary>
        /// <param name="screen">Screen</param>
        private static TreeNode NewScreenTreeNode(Screen screen)
        {
            // Build new tree node
            var name = BuildScreenNodeName(screen);
            var text = name;
            var treeNode = new TreeNode(text)
            {
                Tag = screen,
                Name = name
            };

            return treeNode;
        }


        /// <summary> Set node color when tree does not have focuus</summary>
        /// <param name="treeNode">Tree node to act upon </param>
        /// <param name="setSelected">true for highligh color otherwise standard color </param>
        private static void SetNodeColor(TreeNode treeNode,  bool setSelected)
        {
            if (setSelected)
            {
                treeNode.BackColor = Color.FromKnownColor(KnownColor.Highlight);
                treeNode.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

            }
            else
            {
                treeNode.BackColor = Color.FromKnownColor(KnownColor.Window);
                treeNode.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
            }
        }

        /// <summary> Insert control below node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void InsertControlBelowMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new element
            var element = NewControlElement();

            // Build new tree node
            var treeNode = NewControlTreeNode(element);

            // Add below node clicked and expand 
            var node = (XElement)_clickedControlTreeNode.Tag;
            node.AddAfterSelf(element);

            if (_clickedControlTreeNode.NextNode == null)
            {
                _clickedControlTreeNode.Parent.Nodes.Add(treeNode);
            }
            else
            {
                _clickedControlTreeNode.Parent.Nodes.Insert(_clickedControlTreeNode.Index + 1, treeNode);
            }

            // Setup items for new control
            ControlSetup(treeNode, ModeType.AddBelow);
        }

        /// <summary> Insert control above node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void InsertControlAboveMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new element
            var element = NewControlElement();

            // Build new tree node
            var treeNode = NewControlTreeNode(element);

            // Add above node clicked and expand 
            var node = (XElement)_clickedControlTreeNode.Tag;
            node.AddBeforeSelf(element);

            _clickedControlTreeNode.Parent.Nodes.Insert(_clickedControlTreeNode.Index, treeNode);

            // Setup items for new control
            ControlSetup(treeNode, ModeType.AddAbove);
        }

        /// <summary> Update processing display in status bar </summary>
        /// <param name="text">Text to display in status bar</param>
        private void Processing(string text)
        {
            lblProcessingFile.Text = string.IsNullOrEmpty(text) ? text : string.Format(Resources.GeneratingFile, text);

            pnlButtons.Refresh();
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

            _gridInfo.Add(info);

            // rebind to grid
            grdInfo.DataSource = _gridInfo;

            // Incrememnt row
            _rowIndex++;

            // Set status and text into tool tip for cell
            grdInfo.CurrentCell = grdInfo[Info.StatusColumnNo, _rowIndex];
            grdInfo.CurrentCell.ToolTipText = text;

            grdInfo.Refresh();
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

        /// <summary> Initialize panel </summary>
        /// <param name="panel">Panel to initialize</param>
        private void InitPanel(Panel panel)
        {
            panel.Visible = false;
            panel.Dock = DockStyle.None;
        }

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.WebCustomization;

            btnSave.Text = Resources.Save;
            btnCancel.Text = Resources.Cancel;
            btnBack.Text = Resources.Back;
            btnNext.Text = Resources.Next;

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

            tooltip.SetToolTip(btnPackageFinder, Resources.PackageFinderTip);
            tooltip.SetToolTip(btnNew, Resources.PackageNewTip);
            tooltip.SetToolTip(btnFolder, Resources.FolderFinderTip);
            tooltip.SetToolTip(btnEula, Resources.EulaFinderTip);
            tooltip.SetToolTip(btnDeleteEula, Resources.EulaRemoveTip);
            tooltip.SetToolTip(btnDeleteBootstrapper, Resources.BootstrapperRemoveTip);
            tooltip.SetToolTip(btnDeleteAssembly, Resources.AssemblyRemoveTip);

            // Step Screens
            lblModuleId.Text = Resources.ModuleId;
            tooltip.SetToolTip(lblModuleId, Resources.ModuleIdTip);

            lblCategory.Text = Resources.Category;
            tooltip.SetToolTip(lblCategory, Resources.CategoryTip);

            lblTargetScreen.Text = Resources.TargetScreen;
            tooltip.SetToolTip(lblTargetScreen, Resources.TargetScreenTip);

            lblDescription.Text = Resources.Description;
            tooltip.SetToolTip(lblDescription, Resources.CustomDescriptionTip);

            lblScreenName.Text = Resources.ScreenName;
            tooltip.SetToolTip(lblScreenName, Resources.CustomNameTip);

            lblControlsConfig.Text = Resources.ControlsConfiguration;
            tooltip.SetToolTip(lblControlsConfig, Resources.ControlsConfigTip);

            lblControlsBehavior.Text = Resources.ControlsBehavior;
            tooltip.SetToolTip(lblControlsBehavior, Resources.ControlsBehaviorTip);

            // Step Controls
            lblControlName.Text = Resources.ControlName;
            tooltip.SetToolTip(lblControlName, Resources.ControlNameTip);

            lblControlType.Text = Resources.ControlType;
            tooltip.SetToolTip(lblControlType, Resources.ControlTypeTip);

            lblControlLabel.Text = Resources.ControlLabel;
            tooltip.SetToolTip(lblControlLabel, Resources.ControlLabelTip);

            lblControlBinding.Text = Resources.ControlBinding;
            tooltip.SetToolTip(lblControlBinding, Resources.ControlBindingTip);

            lblPlacementID.Text = Resources.PlacementId;
            tooltip.SetToolTip(lblPlacementID, Resources.ControlPlacementIdTip);

            lblHeaderPlacementID.Text = Resources.HeaderPlacementID;
            tooltip.SetToolTip(lblHeaderPlacementID, Resources.ControlPlacementIdTip);

            lblDetailPlacementID.Text = Resources.DetailPlacementID;
            tooltip.SetToolTip(lblDetailPlacementID, Resources.ControlPlacementIdTip);

            chkBeforeID.Text = Resources.PlacementBeforeID;
            tooltip.SetToolTip(chkBeforeID, Resources.ControlCheckPlacementTip);

            chkBeforeHeaderID.Text = Resources.PlacementBeforeID;
            tooltip.SetToolTip(chkBeforeHeaderID, Resources.ControlCheckPlacementTip);

            chkBeforeDetailID.Text = Resources.PlacementBeforeID;
            tooltip.SetToolTip(chkBeforeDetailID, Resources.ControlCheckPlacementTip);

            lblMaxLength.Text = Resources.MaxLength;
            tooltip.SetToolTip(lblMaxLength, Resources.ControlMaxLengthTip);

            lblControlCols.Text = Resources.ControlColumns;
            tooltip.SetToolTip(lblControlCols, Resources.ControlColsTip);

            lblControlRows.Text = Resources.ControlRows;
            tooltip.SetToolTip(lblControlRows, Resources.ControlRowsTip);

            lblFinderTextID.Text = Resources.FinderTextID;
            tooltip.SetToolTip(lblFinderTextID, Resources.ControlFinderTextIdTip);

            // Step Generate
            lblGenerateManifest.Text = Resources.ManifestToGenerateTip;

            tooltip.SetToolTip(txtSettingsToGenerate, Resources.SettingsToGenerateTip);

            // Step Generated
            lblGenerated.Text = Resources.Generated;
        }

        /// <summary> Initialize info and modify grid display </summary>
        private void InitInfo()
        {
            // Assign binding to datasource (two binding)
            grdInfo.DataSource = _gridInfo;

            // Assign widths and localized text
            GenericInit(grdInfo, Info.FileNameColumnNo, 700, Resources.FileName, true, true);
            GenericInit(grdInfo, Info.StatusColumnNo, 50, Resources.Status, true, true);
        }

        /// <summary> Initialize screen source </summary>
        private void InitScreenSource()
        {
            // Define source
            _allScreens = new Dictionary<string, List<string>>();

            // ScreeName constants from application with attributes
            var modelsDll =
                Assembly.LoadFile(Path.Combine(RegistryHelper.Sage300CWebFolder,
                    @"Sage.CA.SBS.ERP.Sage300.Common.Models.dll"));
            var screenNameConstant = modelsDll.GetType("Sage.CA.SBS.ERP.Sage300.Common.Models.Enums.ScreenName");
            var screenNameAttribute =
                modelsDll.GetType("Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes.ScreenNameAttribute");

            // Get constants
            var fields = (from field in screenNameConstant.GetFields(BindingFlags.Public | BindingFlags.Static)
                let attrs = field.GetCustomAttributes(screenNameAttribute, false)
                where attrs.Length > 0
                select field);

            // Iterate constants for loading into source
            foreach (var field in fields)
            {
                // Get attribute values
                var attributes = field.GetCustomAttributes(screenNameAttribute).ToArray();
                dynamic module = attributes[0];

                // Ignore customization flag as all screens will be allowed to be customized
                // var customization = (bool)(module.GetType().GetProperty("Customization").GetValue(module, null));

                // Load screen source
                var moduleId = (string) (module.GetType().GetProperty(ProcessGeneration.PropertyModuleId).GetValue(module, null));
                var category = (string) (module.GetType().GetProperty(ProcessGeneration.PropertyCategory).GetValue(module, null));
                var screenName = (string) (module.GetType().GetProperty(ProcessGeneration.PropertyName).GetValue(module, null));
                var value = field.Name;

                // Skip these module(s)
                if (moduleId.Equals("XXX"))
                {
                    continue;
                }

                // Add module with category
                BuildScreen(moduleId, category);
                // Add module + category with screenName
                BuildScreen(moduleId + category, screenName);
                // Add module + category + screenName with value (constant itself)
                BuildScreen(moduleId + category + screenName, value);
                // Add "Manifest"+ moduleId + category + value with screenName (for reverse lookup)
                BuildScreen(ProcessGeneration.Manifest + moduleId + category + value, screenName);
            }
        }

        /// <summary> Initialize wizard steps </summary>
        private void InitWizardSteps()
        {
            // Default
            btnNext.Text = Resources.Next;
            btnBack.Enabled = false;

            // Current Step
            _currentWizardStep = -1;

            // Init wizard steps
            _wizardSteps.Clear();

            // Init Panels
            InitPanel(pnlCreateEdit);
            InitPanel(pnlScreens);
            InitPanel(pnlControls);
            InitPanel(pnlGenerate);
            InitPanel(pnlGenerated);

            // Assign steps for wizard
            AddStep(Resources.StepTitleCreateEdit, Resources.StepDescriptionCreateEdit, pnlCreateEdit);
            AddStep(Resources.StepTitleScreens, Resources.StepDescriptionScreens, pnlScreens);
            AddStep(Resources.StepTitleControls, Resources.StepDescriptionControls, pnlControls);
            AddStep(Resources.StepTitleGenerate, Resources.StepDescriptionGenerate, pnlGenerate);
            AddStep(Resources.StepTitleGenerated, Resources.StepDescriptionGenerated, pnlGenerated);

            // Display first step
            NextStep();
        }

        /// <summary> Next/Generate Navigation </summary>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void NextStep()
        {
            // Finished?
            if (!_currentWizardStep.Equals(-1) && _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
            {
                Close();
            }
            else
            {
                // Proceed to next wizard step or start generation if last step
                if (!_currentWizardStep.Equals(-1) &&
                    _wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                {
                    // Setup display before processing
                    _gridInfo.Clear();
                    ProcessingSetup(false);
                    grdInfo.DataSource = _gridInfo;
                    grdInfo.Refresh();

                    _rowIndex = -1;

                    // Start background worker for processing (async)
                    wrkBackground.RunWorkerAsync(_settings);
                }
                else
                {
                    // Proceed to next step
                    if (!_currentWizardStep.Equals(-1))
                    {
                        // Before proceeding to next step, ensure current step is valid
                        if (!ValidateStep())
                        {
                            return;
                        }

                        btnBack.Enabled = true;

                        ShowStep(false);
                    }

                    _currentWizardStep++;

                    // if Step is Controls, expand tree control
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelControls))
                    {
                        treeControls.ExpandAll();
                    }

                    // if Step is Screens, expand tree control
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelScreens))
                    {
                        treeScreens.ExpandAll();
                    }

                    // Update Generate Controls if Step is Generate
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                    {
                        // Load controls based upon screens in the previous steps
                        ClearGenerateControls();

                        // Generate JSON Manifest
                        var manifest = GenerateManifest();
                        txtManifestToGenerate.Text = manifest.ToString();

                        // Generate XML Settings
                        txtSettingsToGenerate.Text = treeControls.Nodes[0].Tag.ToString();

                        // Establish settings for processing (Validation already ocurred in each step)
                        _settings = new Settings
                        {
                            FolderName = txtFolderName.Text.Trim(),
                            Manifest = manifest,
                            XmlSettings = (XDocument) treeControls.Nodes[0].Tag,
                            EulaFolder = string.IsNullOrEmpty(txtEula.Text.Trim()) ?
                            string.Empty : 
                            Path.GetDirectoryName(txtEula.Text.Trim())
                        };
                    }

                    ShowStep(true);

                    // Update text of Next button?
                    if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerate))
                    {
                        btnNext.Text = Resources.Generate;
                    }

                    // Update title and text for step
                    ShowStepTitle();
                }
            }
        }

        /// <summary> Back Navigation </summary>
        /// <remarks>Back wizard step</remarks>
        private void BackStep()
        {
            // Proceed if not on first step
            if (!_currentWizardStep.Equals(0))
            {
                ShowStep(false);

                // Proceed back a step or home if done
                if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelGenerated))
                {
                    btnBack.Text = Resources.Back;
                    btnNext.Text = Resources.Next;
                    _currentWizardStep = 0;
                    ClearAll();
                }
                else
                {
                    btnNext.Text = Resources.Next;
                    _currentWizardStep--;
                }

                ShowStep(true);

                // Enable back button?
                if (_currentWizardStep.Equals(0))
                {
                    btnBack.Enabled = false;
                    btnNext.Enabled = true;
                }

                // Update title and text for step
                ShowStepTitle();
            }
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
            }

            // Screens Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelScreens))
            {
                valid = ValidScreensStep();
            }

            // Controls Step
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelControls))
            {
                valid = ValidControlsStep();
            }

            if (!string.IsNullOrEmpty(valid))
            { 
                // Something is invalid
                DisplayMessage(valid, MessageBoxIcon.Error);
            }

            return string.IsNullOrEmpty(valid);
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Next/Generate button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Next wizard step or Generate if last step</remarks>
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        /// <summary> Back button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        /// <remarks>Back wizard step</remarks>
        private void btnBack_Click(object sender, EventArgs e)
        {
            BackStep();
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
            ProcessingSetup(true);
            Processing("");

            ShowStep(false);

            _currentWizardStep++;

            ShowStep(true);

            // Update title and text for step
            ShowStepTitle();

            // Display final step
            btnBack.Text = Resources.Home;
            btnNext.Text = Resources.Finish;
        }

        /// <summary> Show Step </summary>
        /// <param name="visible">True to show otherwise false </param>
        private void ShowStep(bool visible)
        {
            _wizardSteps[_currentWizardStep].Panel.Dock = visible ? DockStyle.Fill : DockStyle.None;
            _wizardSteps[_currentWizardStep].Panel.Visible = visible;
            splitSteps.SplitterDistance = SplitterDistance;
        }

        /// <summary> Show Step Title</summary>
        private void ShowStepTitle()
        {
            lblStepTitle.Text = Resources.Step + (_currentWizardStep + 1).ToString("#0") + Resources.Dash +
                                _wizardSteps[_currentWizardStep].Title;
            lblStepDescription.Text = _wizardSteps[_currentWizardStep].Description;
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

        /// <summary> New customization</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            NewManifest();
        }

        /// <summary> Folder search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnFolder_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Display manifest selected
            txtFolderName.Text = dialog.SelectedPath;
        }

        /// <summary> Existing JSON Manifest</summary>
        /// <param name="fileName">File name </param>
        private void ExistingManifest(string fileName)
        {
            // Get the manifest 
            var manifest = JObject.Parse(File.ReadAllText(fileName));

            // Properties
            txtPackageId.Text = (string) manifest.SelectToken(ProcessGeneration.PropertyPackageId);
            txtCustomizationName.Text = (string) manifest.SelectToken(ProcessGeneration.PropertyName);
            txtCustomizationDescription.Text = (string) manifest.SelectToken(ProcessGeneration.PropertyDescription);
            txtCompanyName.Text = (string) manifest.SelectToken(ProcessGeneration.PropertyBusinessPartnerName);
            txtCompatibility.Text = (string) manifest.SelectToken(ProcessGeneration.PropertySageCompatibility);
            txtVersion.Text = (string) manifest.SelectToken(ProcessGeneration.PropertyVersion);

            txtBootstrapper.Text = (string)manifest.SelectToken(ProcessGeneration.PropertyBootstrapper);
            txtAssembly.Text = (string)manifest.SelectToken(ProcessGeneration.PropertyAssembly);

            // Get location from folder where found
            var path = Path.GetDirectoryName(fileName);
            txtFolderName.Text = path;

            var eula = (string) manifest.SelectToken(ProcessGeneration.PropertyEula);
            if (!string.IsNullOrEmpty(eula))
            {
                if (path != null)
                {
                    eula = Path.Combine(path, eula);
                }
            }
            txtEula.Text = eula;


            // Read JSON to get the Screen Controls
            var webScreens =
                from json in manifest[ProcessGeneration.PropertyWebScreens]
                select new
                {
                    ScreenName = (string) json[ProcessGeneration.PropertyScreenName],
                    ScreenDescription = (string) json[ProcessGeneration.PropertyScreenDescription],
                    TargetScreen = (string) json[ProcessGeneration.PropertyTargetScreen],
                    Category = (string) json[ProcessGeneration.PropertyCategory],
                    ControlsConfiguration = (string) json[ProcessGeneration.PropertyControlsConfiguration],
                    ControlsBehavior = (string) json[ProcessGeneration.PropertyControlsBehavior],
                    Module = (string) json[ProcessGeneration.PropertyModule]
                };

            // Clear binding list bound to grid
            _screens.Clear();

            // Iterate and add to binding
            foreach (var webScreen in webScreens)
            {
                _screens.Add(new Screen
                {
                    ModuleId = (ModuleType) Enum.Parse(typeof(ModuleType), webScreen.Module),
                    Category = webScreen.Category != null ? webScreen.Category : "Transactions",
                    TargetScreen = webScreen.TargetScreen,
                    Description = webScreen.ScreenDescription,
                    ScreenName = webScreen.ScreenName,
                    ControlsConfiguration = webScreen.ControlsConfiguration,
                    ControlsBehavior = webScreen.ControlsBehavior
                });

            }

            // Load tree based upon screens
            LoadScreenTree();

            // Clear screen display
            ClearScreenControls(ClearType.All);

            // Load tree based upon screens
            LoadControlTree(CreateCombinedDocument());

            // Clear control display
            ClearControlControls();
        }

        /// <summary> New JSON Manifest</summary>
        private void NewManifest()
        {
            ClearAll();
            txtPackageId.Text = Guid.NewGuid().ToString();
            txtCompatibility.Text = Resources.CompatibilityDefault;
            txtVersion.Text = Resources.VersionDefault;
        }

        /// <summary> Clear All inputs</summary>
        private void ClearAll()
        {
            // Default Properties
            txtPackageId.Text = string.Empty;
            txtCustomizationName.Text = string.Empty;
            txtCustomizationDescription.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtCompatibility.Text = string.Empty;
            txtVersion.Text = string.Empty;

            txtEula.Text = string.Empty;

            txtBootstrapper.Text = string.Empty;
            txtAssembly.Text = string.Empty;

            txtFolderName.Text = string.Empty;

            // Screen Controls
            _screens.Clear();

            // Load control tree based upon screens
            LoadScreenTree();

            ClearScreenControls(ClearType.All);

            // Load control tree based upon screens
            LoadControlTree(CreateCombinedDocument());

            // Clear control display
            ClearControlControls();

            // Set to package id
            txtPackageId.Focus();
        }

        /// <summary> Generate JSON Manifest</summary>
        private JObject GenerateManifest()
        {
            // Add properties
            var manifest = new JObject
            {
                new JProperty(ProcessGeneration.PropertyGeneratedMessage, Resources.GeneratedMessage),
                new JProperty(ProcessGeneration.PropertyGeneratedWarning, Resources.GeneratedWarning),
                new JProperty(ProcessGeneration.PropertyBusinessPartnerName, txtCompanyName.Text.Trim()),
                new JProperty(ProcessGeneration.PropertyPackageId, txtPackageId.Text.Trim()),
                new JProperty(ProcessGeneration.PropertySageCompatibility, txtCompatibility.Text.Trim()),
                new JProperty(ProcessGeneration.PropertyName, txtCustomizationName.Text.Trim()),
                new JProperty(ProcessGeneration.PropertyDescription, txtCustomizationDescription.Text.Trim()),
                new JProperty(ProcessGeneration.PropertyVersion, txtVersion.Text.Trim())
            };

            // Add EULA if specified
            if (!string.IsNullOrEmpty(txtEula.Text.Trim()))
            {
                var eula = Path.GetFileName(txtEula.Text.Trim());
                manifest.Add(new JProperty(ProcessGeneration.PropertyEula, eula));
            }

            // Add Bootstrapper if specified
            if (!string.IsNullOrEmpty(txtBootstrapper.Text.Trim()))
            {
                manifest.Add(new JProperty(ProcessGeneration.PropertyBootstrapper, txtBootstrapper.Text.Trim()));
            }

            // Add Assembly if specified
            if (!string.IsNullOrEmpty(txtAssembly.Text.Trim()))
            {
                manifest.Add(new JProperty(ProcessGeneration.PropertyAssembly, txtAssembly.Text.Trim()));
            }

            // Create array of screens
            var jArray = new JArray();
            
            foreach (var screen in _screens)
            {
                var key = screen.TargetScreen;

                var webScreens = new JObject
                {
                    new JProperty(ProcessGeneration.PropertyScreenName, screen.ScreenName),
                    new JProperty(ProcessGeneration.PropertyScreenDescription, screen.Description),
                    new JProperty(ProcessGeneration.PropertyCategory, screen.Category),
                    new JProperty(ProcessGeneration.PropertyTargetScreen, key)
                };

                // Only add if there are controls for this screen
                var treeNodes = FindNodes(key);

                // Find node for this screen and evaluate if controls
                if (treeNodes.Where(treeNode => treeNode.Name.Equals(key)).Any(treeNode => treeNode.Nodes.Count > 0))
                {
                    webScreens.Add(new JProperty(ProcessGeneration.PropertyControlsConfiguration, screen.ControlsConfiguration));
                }

                webScreens.Add(new JProperty(ProcessGeneration.PropertyControlsBehavior, screen.ControlsBehavior));
                webScreens.Add(new JProperty(ProcessGeneration.PropertyModule, screen.ModuleId.ToString()));

                jArray.Add(webScreens);
            }

            manifest.Add(new JProperty(ProcessGeneration.PropertyWebScreens, jArray));

            return manifest;
        }

        /// <summary> Create Combined XML Document</summary>
        /// <returns>xDocument to hold multiple screen layouts (XML Documents)</returns>
        private XDocument CreateCombinedDocument()
        {
            // New combined document for binding to tree
            var xDocument = new XDocument(new XElement(ProcessGeneration.ElementScreens));
            // Create element to hold screen(s) for customization
            var screensElements = xDocument.Descendants(ProcessGeneration.ElementScreens).First();

            // Iterate screen(s) to load XML(s), if available, and load into a single document
            foreach (var screen in _screens)
            {
                // Load Document for screen
                var screenDocument = LoadXml(txtFolderName.Text.Trim(), screen);
                // Get descendants of Screen
                var screenElements = screenDocument.Descendants(ProcessGeneration.ElementScreen);
                // Add these decendants to Screens decendants
                screensElements.Add(screenElements);
            }

            // Combined document
            return xDocument;
        }

        /// <summary> Enable or disable screens controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableScreensControls(bool enable)
        {
            btnNext.Enabled = enable;
            btnBack.Enabled = enable;
        }

        /// <summary> Enable or disable screen controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableScreenControls(bool enable)
        {
            btnSave.Visible = enable;
            btnCancel.Visible = enable;
            pnlScreenControls.Enabled = enable;
        }

        /// <summary> Enable or disable controls controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableControlsControls(bool enable)
        {
            btnNext.Enabled = enable;
            btnBack.Enabled = enable;
        }

        /// <summary> Enable or disable control controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableControlControls(bool enable)
        {
            btnSave.Visible = enable;
            btnCancel.Visible = enable;
            splitControlsBase.Panel2.Enabled = enable;
        }

        /// <summary> Enable control controls</summary>
        private void EnableControlControls()
        {
            txtControlLabel.Enabled = true;
            txtControlBinding.Enabled = true;
            txtPlacementID.Enabled = true;
            txtHeaderPlacementID.Enabled = true;
            txtDetailPlacementID.Enabled = true;
            chkBeforeID.Enabled = true;
            chkBeforeHeaderID.Enabled = true;
            chkBeforeDetailID.Enabled = true;
            txtMaxLength.Enabled = true;
            txtControlCols.Enabled = true;
            txtControlRows.Enabled = true;
            txtFinderTextID.Enabled = true;
        }

        /// <summary> Enable control controls</summary>
        /// <param name="controlType">Control type to determine enabled state </param>
        private void EnableControlControls(ControlType controlType)
        {
            txtControlLabel.Enabled = !controlType.Equals(ControlType.Grid);
            txtControlBinding.Enabled =
                !(controlType.Equals(ControlType.TabPage) || controlType.Equals(ControlType.Panel));
            txtPlacementID.Enabled = !controlType.Equals(ControlType.TabPage);
            txtHeaderPlacementID.Enabled = controlType.Equals(ControlType.TabPage);
            txtDetailPlacementID.Enabled = controlType.Equals(ControlType.TabPage);
            chkBeforeID.Enabled = !controlType.Equals(ControlType.TabPage);
            chkBeforeHeaderID.Enabled = controlType.Equals(ControlType.TabPage);
            chkBeforeDetailID.Enabled = controlType.Equals(ControlType.TabPage);
            txtMaxLength.Enabled = controlType.Equals(ControlType.TextArea) || controlType.Equals(ControlType.TextBox);
            txtControlCols.Enabled = controlType.Equals(ControlType.TextArea);
            txtControlRows.Enabled = controlType.Equals(ControlType.TextArea);
            txtFinderTextID.Enabled = controlType.Equals(ControlType.Finder);
        }

        /// <summary> Add a screen</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void AddScreenMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new screen
            var screen = NewScreen();

            // Build new tree node
            var treeNode = NewScreenTreeNode(screen);

            // Add to screens
            _screens.Add(screen);
            _clickedScreenTreeNode.Nodes.Add(treeNode);

            ScreenSetup(treeNode, ModeType.Add);
        }

        /// <summary> Delete screen</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteScreenMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Delete tree node
            DeleteScreenNode(_clickedScreenTreeNode);
        }

        /// <summary> Delete all screens</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteScreensMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var treeNodes = _clickedScreenTreeNode.Nodes;

            for (var i = treeNodes.Count - 1; i > -1; i--)
            {
                DeleteScreenNode(treeNodes[i]);
            }
        }

        /// <summary> Generic init for grid </summary>
        /// <param name="grid">Grid control</param>
        /// <param name="column">Column Number</param>
        /// <param name="width">Column Width</param>
        /// <param name="text">Header Text</param>
        /// <param name="visible">True for visible otherwise False</param>
        /// <param name="readOnly">True for read only otherwise False</param>
        private static void GenericInit(DataGridView grid, int column, int width, string text, bool visible,
            bool readOnly)
        {
            grid.Columns[column].Width = width;
            grid.Columns[column].HeaderText = text;
            grid.Columns[column].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[column].Visible = visible;
            grid.Columns[column].ReadOnly = readOnly;

            // Show read only in InactiveCaption color
            if (readOnly)
            {
                grid.Columns[column].DefaultCellStyle.BackColor = SystemColors.Window;
            }
        }

        /// <summary> Initialize screen info and modify grid display </summary>
        private void InitScreens()
        {
            // Add items from enum to drop down list (with a blank row to start with)
            cboModuleId.Items.Add(string.Empty);
            foreach (
                var moduleType in
                Enum.GetValues(typeof(ModuleType))
                    .Cast<ModuleType>())
            {
                cboModuleId.Items.Add(moduleType);
            }

            ClearScreenControls(ClearType.All);

            EnableScreenControls(false);

        }

        /// <summary> Initialize control info </summary>
        private void InitControls()
        {
            // Add items from enum to drop down list (with a blank row to start with)
            cboControlType.Items.Add(string.Empty);
            foreach (
                var controlType in
                Enum.GetValues(typeof(ControlType))
                    .Cast<ControlType>())
            {
                cboControlType.Items.Add(controlType);
            }

            treeControls.Enabled = true;

            ClearControlControls();

            splitControlsBase.Panel2.Enabled = false;

        }

        /// <summary> Clear Screen Controls </summary>
        /// <param name="clearType">Enumeration to clear controls</param>
        private void ClearScreenControls(ClearType clearType)
        {
            // Hierarchical approach

            if (clearType == ClearType.All ||
                clearType == ClearType.Module)
            {
                cboModuleId.SelectedIndex = 0;
            }

            if (clearType == ClearType.All ||
                clearType == ClearType.Module ||
                clearType == ClearType.Category)
            {
                cboCategory.Items.Clear();
                cboCategory.Items.Add(string.Empty);
                cboCategory.SelectedIndex = 0;
            }

            if (clearType == ClearType.All ||
                clearType == ClearType.Module ||
                clearType == ClearType.Category ||
                clearType == ClearType.Screen)
            {
                cboTargetScreen.Items.Clear();
                cboTargetScreen.Items.Add(string.Empty);
                cboTargetScreen.SelectedIndex = 0;
            }

            if (clearType == ClearType.All ||
                clearType == ClearType.Module ||
                clearType == ClearType.Category ||
                clearType == ClearType.Screen ||
                clearType == ClearType.Other)
            {
                txtDescription.Clear();
                txtScreenName.Clear();
                txtControlsConfig.Clear();
                txtControlsBehavior.Clear();
            }

        }

        /// <summary> Clear Control Controls </summary>
        private void ClearControlControls()
        {
            txtControlName.Clear();
            cboControlType.SelectedIndex = 0;
            txtControlLabel.Clear();

            txtControlBinding.Clear();

            txtPlacementID.Clear();
            txtHeaderPlacementID.Clear();
            txtDetailPlacementID.Clear();
            chkBeforeID.Checked = false;
            chkBeforeHeaderID.Checked = false;
            chkBeforeDetailID.Checked = false;

            txtMaxLength.Clear();
            txtControlCols.Clear();
            txtControlRows.Clear();

            txtFinderTextID.Clear();
        }

        /// <summary> Clear Generate Controls </summary>
        private void ClearGenerateControls()
        {
            txtManifestToGenerate.Clear();
        }

        /// <summary> Set Filter</summary>
        /// <param name="control">Control to set</param>
        /// <param name="key">key to list </param>
        private void SetFilter(ComboBox control, string key)
        {
            // Get appropriate list for filtering
            var filter = GetFilter(key);

            // Bind
            control.Items.Clear();
            control.Items.Add(string.Empty);

            // Iterate filter and add one by one
            foreach (var item in filter)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    control.Items.Add(item);
                }
            }

            // Set first item
            if (filter.Count > 0)
            {
                control.SelectedIndex = 0;
            }

        }

        /// <summary> Set Defaults for manual fields</summary>
        /// <param name="key">Key to list </param>
        private void SetDefaults(string key)
        {
            var filter = GetFilter(key);

            if (filter.Count <= 0)
            {
                return;
            }

            // filter[0] is constant name in ScreenName.cs
            txtDescription.Text = filter[0] + ProcessGeneration.CustomDescriptionSuffix;
            txtScreenName.Text = filter[0] + ProcessGeneration.CustomNameSuffix;
            txtControlsConfig.Text = filter[0] + ProcessGeneration.XmlFileNameSuffix;
            txtControlsBehavior.Text = filter[0] + ProcessGeneration.JavaScriptFileNameSuffix;
        }

        /// <summary> Get Filter</summary>
        /// <param name="key">key to list </param>
        private List<string> GetFilter(string key)
        {
            // Sort in alpha order
            return _allScreens[key].OrderBy(q => q).ToList();
        }

        /// <summary> Build Screens</summary>
        /// <param name="key">Key to dictionary</param>
        /// <param name="value">Value for List in dictionary</param>
        private void BuildScreen(string key, string value)
        {
            // If the key already exists
            if (_allScreens.ContainsKey(key))
            {
                // But value is not in list
                if (!_allScreens[key].Contains(value))
                {
                    // Add value to list
                    _allScreens[key].Add(value);
                }
            }
            else
            {
                // Add key and value
                _allScreens.Add(key, new List<string> {value});
            }
        }

        /// <summary> Get Target Screen</summary>
        /// <param name="key">Key to dictionary</param>
        /// <remarks>
        /// Manifest stores value (constant) in TargetScreen tag
        /// Wizard uses name of screen in attribute and therefore needs to 
        /// translate back and forth to read/store properly
        /// </remarks>
        /// <returns>Target Screen</returns>
        private string GetTargetScreen(string key)
        {
            return _allScreens.ContainsKey(key) ? _allScreens[key][0] : string.Empty;
        }

        /// <summary> Load XML if it exists otherwise create a new document as a template</summary>
        /// <param name="folderName">Folder name </param>
        /// <param name="screen">Screen properties </param>
        /// <remarks>If creating a newa customization, file will not exist yet</remarks>
        private XDocument LoadXml(string folderName, Screen screen)
        {
            XDocument xDocument;

            try
            {
                // Combine path and name (if error, then create manually - existing customization screen
                // with no controls and thus XML was not created)
                var fileName = Path.Combine(folderName, screen.ControlsConfiguration);

                // Open file
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    // Load the XML into the document
                    xDocument = XDocument.Load(fileStream);
                }
            }
            catch (Exception)
            {
                // File does not yet exist. So, genernate the screen element
                var key = screen.TargetScreen; //GetTargetScreen(screen.ModuleId.ToString() + screen.Category + screen.TargetScreen);
                xDocument = new XDocument(new XElement(ProcessGeneration.ElementScreen));
                var screenElement = xDocument.Descendants(ProcessGeneration.ElementScreen).First();
                screenElement.Add(new XAttribute(ProcessGeneration.AttributeName, key));
            }

            return xDocument;
        }

        /// <summary> Load the screens into the screen tree</summary>
        private void LoadScreenTree()
        {
            // Clear tree first
            treeScreens.Nodes.Clear();

            // Add top level node
            var screensNode = new TreeNode(ProcessGeneration.ElementScreens) {Name = ProcessGeneration.ElementScreens};
            treeScreens.Nodes.Add(screensNode);

            // Iterate screens and add
            foreach (var screen in _screens)
            {
                // Build new tree node
                var name = BuildScreenNodeName(screen);
                var text = BuildScreenText(screen);
                var newTreeNode = new TreeNode(text)
                {
                    Tag = screen,
                    Name = name
                };
                screensNode.Nodes.Add(newTreeNode);
            }

        }

        /// <summary> Load the combined document into the control tree</summary>
        /// <param name="xDocument">XML Document </param>
        private void LoadControlTree(XDocument xDocument)
        {
            // Clear tree first
            treeControls.Nodes.Clear();

            // Add top level node
            var screensNode = new TreeNode(ProcessGeneration.ElementScreens) {Tag = xDocument};
            treeControls.Nodes.Add(screensNode);

            BuildNodes(screensNode, xDocument.Root);
        }

        /// <summary> Build node for treeview</summary>
        /// <param name="treeNode">Incoming tree node </param>
        /// <param name="element">Incoming element </param>
        private static void BuildNodes(TreeNode treeNode, XContainer element)
        {
            // Iterate nodes in element
            foreach (var node in element.Nodes())
            {
                // Must be an element node
                if (!node.NodeType.Equals(XmlNodeType.Element))
                {
                    continue;
                }

                // Build new tree node
                var name = BuildControlNodeName(node);
                var text = BuildControlText(node);
                var newTreeNode = new TreeNode(text)
                {
                    Tag = node,
                    Name = name
                };
                treeNode.Nodes.Add(newTreeNode);
                BuildNodes(newTreeNode, (XElement) node);
            }
        }

        /// <summary> Build Control Node Name</summary>
        /// <param name="node">XML Node </param>
        /// <returns>Name for Node</returns>
        private static string BuildControlNodeName(XNode node)
        {
            var element = (XElement) node;
            var nodeName = string.Empty;

            // Different name based upon screen or control
            if (element.Name.LocalName.Equals(ProcessGeneration.ElementScreen))
            {
                var nameAttribute = element.Attributes(ProcessGeneration.AttributeName).FirstOrDefault();
                if (nameAttribute != null)
                {
                    nodeName = nameAttribute.Value;
                }
            }
            else
            {
                var idAttribute = element.Attributes(ProcessGeneration.AttributeId).FirstOrDefault();
                if (idAttribute != null)
                {
                    nodeName = idAttribute.Value;
                }
            }

            return nodeName;
        }

        /// <summary> Get Screen Node for Control</summary>
        /// <param name="node">XML Node </param>
        /// <returns>Node of screen for control</returns>
        private static XNode GetScreenNode(XNode node)
        {
            while (true)
            {
                var element = (XElement) node;

                // If node is for a screen, then return the name (from the attribute)
                if (element != null && element.Name.LocalName.Equals(ProcessGeneration.ElementScreen))
                {
                    return element;
                }

                if (element != null)
                {
                    node = element.Parent;
                }
            }
        }

        /// <summary> Update Screen if controls are or are not present</summary>
        /// <param name="node">XML Node </param>
        private void UpdateScreen(XNode node)
        {
            var element = (XElement)node;

            // Get screen name
            var nameAttribute = element.Attributes(ProcessGeneration.AttributeName).First();
            var screenName = nameAttribute.Value;

            // Find tree node in tree screen 
            foreach (TreeNode treeNode in treeScreens.Nodes[0].Nodes)
            {
                // Node does not match screen
                if (!treeNode.Name.Equals(screenName))
                {
                    continue;
                }

                // Get screen out of tag and update it
                var screen = (Screen)treeNode.Tag;
                var hasControls = element.HasElements;

                screen.ControlsConfiguration = hasControls ? screenName + ProcessGeneration.XmlFileNameSuffix : string.Empty;
                treeNode.Text = BuildScreenText(screen);

                break;
            }

        }

        /// <summary> Build Screen Node Name</summary>
        /// <param name="screen">Screen </param>
        /// <returns>Name for Node</returns>
        private static string BuildScreenNodeName(Screen screen)
        {
            return screen.TargetScreen;
        }

        /// <summary> Build Control Text from attributes</summary>
        /// <param name="node">XML Node to build text from</param>
        /// <returns>Text</returns>
        private static string BuildControlText(XNode node)
        {
            var element = (XElement) node;

            // Start with element name
            var text = "<" + element.Name.LocalName + " ";
            var containerOrScreen = element.Name.LocalName == ProcessGeneration.ElementScreen;

            // Add attributes
            foreach (var attribute in element.Attributes())
            {
                var attributeName = attribute.Name.LocalName;

                // Special cases
                if (attributeName.Equals(ProcessGeneration.AttributeXsi))
                {
                    attributeName = ProcessGeneration.AttributeXmlnsXsi;
                }
                else if (attributeName.Equals(ProcessGeneration.AttributeNoNamespaceSchemaLocation))
                {
                    attributeName = ProcessGeneration.AttributeXsiNoNamespaceSchemaLocation;
                }

                text += attributeName + "=\"" + attribute.Value + "\" ";

                // If already determined if container or screen, do not need to keep checking
                if (containerOrScreen)
                {
                    continue;
                }
                // Need to determine if container for closing of tooltip
                if (attribute.Name == ProcessGeneration.AttributeType)
                {
                    containerOrScreen = attribute.Value == ControlTypeTabPage || attribute.Value == ControlTypePanel;
                }
            }

            // Trim and end with closing 
            text = text.Trim() + (containerOrScreen ? string.Empty : "/") + ">";

            return text;
        }

        /// <summary> Build Screen Text from screen</summary>
        /// <param name="screen">Screen to build text from</param>
        /// <returns>Text</returns>
        private static string BuildScreenText(Screen screen)
        {
            var text = ProcessGeneration.PropertyModule + "=\"" + screen.ModuleId.ToString() + "\" ";
            text += ProcessGeneration.PropertyCategory + "=\"" + screen.Category + "\" ";
            text += ProcessGeneration.PropertyScreen + "=\"" + screen.TargetScreen + "\" ";
            text += ProcessGeneration.PropertyDescription + "=\"" + screen.Description + "\" ";
            text += ProcessGeneration.PropertyName + "=\"" + screen.ScreenName + "\" ";
            text += ProcessGeneration.PropertyXml + "=\"" + screen.ControlsConfiguration + "\" ";
            text += ProcessGeneration.PropertyJs + "=\"" + screen.ControlsBehavior + "\" ";

            return text;
        }

        /// <summary> Module has changed, so update remaining controls</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboModuleId_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then clear other controls downstream
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                ClearScreenControls(ClearType.Category);
            }
            else
            {
                // Get module selected and populate the category list
                var moduleId = ((ComboBox) sender).SelectedItem.ToString();
                SetFilter(cboCategory, moduleId);
                ClearScreenControls(ClearType.Screen);
            }
        }

        /// <summary> Category has changed, so update remaining controls</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then clear other controls downstream
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                ClearScreenControls(ClearType.Screen);
            }
            else
            {
                // Get module + category selected and populate the screen list
                var moduleId = cboModuleId.SelectedItem.ToString();
                var category = ((ComboBox) sender).SelectedItem.ToString();
                SetFilter(cboTargetScreen, moduleId + category);
                ClearScreenControls(ClearType.Other);
            }
        }

        /// <summary> Screen has changed, so update remaining controls</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboTargetScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then clear other controls downstream
            if (((ComboBox) sender).SelectedIndex == 0)
            {
                ClearScreenControls(ClearType.Other);
            }
            else
            {
                // Get module + category + screen selected and set defaults
                var moduleId = cboModuleId.SelectedItem.ToString();
                var category = cboCategory.SelectedItem.ToString();
                var targetScreen = ((ComboBox) sender).SelectedItem.ToString();
                SetDefaults(moduleId + category + targetScreen);
            }
        }

        /// <summary> Screen has changed, so update</summary>
        private void SaveScreen()
        {
            // Validation
            var success = ValidScreen(cboModuleId.SelectedItem.ToString(),
                cboCategory.SelectedItem.ToString(),
                cboTargetScreen.SelectedItem.ToString(),
                txtDescription.Text,
                txtScreenName.Text);
            if (!string.IsNullOrEmpty(success))
            {
                DisplayMessage(success, MessageBoxIcon.Error);
                return;
            }

            var node = _modeType == ModeType.Add ? _clickedScreenTreeNode.LastNode : _clickedScreenTreeNode;
            var screen = (Screen)node.Tag;

            var currentkey = _modeType == ModeType.Add ? string.Empty : screen.TargetScreen;

            // Get valued from controls and add to object
            screen.ModuleId = (ModuleType) Enum.Parse(typeof(ModuleType), cboModuleId.SelectedItem.ToString());
            screen.Category = cboCategory.SelectedItem.ToString();
            screen.TargetScreen = GetTargetScreen(screen.ModuleId.ToString() + screen.Category + cboTargetScreen.SelectedItem);
            screen.Description = txtDescription.Text.Trim();
            screen.ScreenName = txtScreenName.Text.Trim();
            screen.ControlsConfiguration = _modeType == ModeType.Add ? string.Empty : txtControlsConfig.Text.Trim();
            screen.ControlsBehavior = txtControlsBehavior.Text.Trim();

            // Add/Update to tree
            node.Name = BuildScreenNodeName(screen);
            node.Text = BuildScreenText(screen);
            node.Tag = screen;

            string key;

            switch (_modeType)
            {
                case ModeType.Add:

                    // Update tree in next step
                    key = screen.TargetScreen;

                    var xDocument = (XDocument)treeControls.Nodes[0].Tag;
                    var screensElement = xDocument.Descendants(ProcessGeneration.ElementScreens).First();

                    var screenElement = new XElement(ProcessGeneration.ElementScreen);
                    screenElement.Add(new XAttribute(ProcessGeneration.AttributeName, key));
                    screensElement.Add(screenElement);

                    treeControls.Nodes[0].Nodes.Add(NewControlTreeNode(screenElement));

                    break;
                case ModeType.Edit:

                    // Update tree in next step (in case screen for this row changed)
                    key = screen.TargetScreen;

                    foreach (TreeNode treeNode in treeControls.Nodes[0].Nodes)
                    {
                        if (treeNode.Name.Equals(currentkey))
                        {
                            var element = (XElement) treeNode.Tag;
                            if (element.HasAttributes)
                            {
                                var nameAttribute = element.Attributes(ProcessGeneration.AttributeName).FirstOrDefault();
                                if (nameAttribute != null)
                                {
                                    if (nameAttribute.Value.Equals(currentkey))
                                    {
                                        nameAttribute.Value = key;
                                    }
                                }
                            }
                            treeNode.Name = BuildControlNodeName(element);
                            treeNode.Text = BuildControlText(element);
                            treeNode.Tag = element;

                        }
                    }

                    break;
                case ModeType.None:
                case ModeType.AddAbove:
                case ModeType.AddBelow:
                    // No action
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Reset mode
            _modeType = ModeType.None;

            // Reset selected color
            SetNodeColor(node, false);

            // Enable screens controls
            EnableScreensControls(true);

            // Clear individual screen controls
            ClearScreenControls(ClearType.All);

            // Disable screen controls
            EnableScreenControls(false);

            // Set focus back to tree
            treeScreens.Focus();
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

            return string.Empty;
        }

        /// <summary> Valid Screens Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidScreensStep()
        {
            // Must have at least one screen
            if (_screens.Count == 0)
            {
                return Resources.InvalidSettingRequiredScreen;
            }

            // Validate each screen
            foreach (var screen in _screens)
            {
                var valid = ValidScreen(screen.ModuleId.ToString(), screen.Category, screen.TargetScreen,
                    screen.Description, screen.ScreenName);
                if (!string.IsNullOrEmpty(valid))
                {
                    return valid;
                }
            }

            return string.Empty;
        }

        /// <summary> Valid Controls Step</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private static string ValidControlsStep()
        {
            // Controls are validated in-line because of dynamic setup
            return string.Empty;
        }

        /// <summary> Valid Screen</summary>
        /// <param name="moduleId">Module ID</param>
        /// <param name="category">Category</param>
        /// <param name="targetScreen">Target Screen</param>
        /// <param name="description">Description</param>
        /// <param name="screenName">Scree Name</param>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private static string ValidScreen(string moduleId, string category, string targetScreen, string description, string screenName)
        {
            // Module ID
            if (string.IsNullOrEmpty(moduleId))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ModuleId.Replace(":", ""));
            }

            // Category
            if (string.IsNullOrEmpty(category))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Category.Replace(":", ""));
            }

            // Target Screen
            if (string.IsNullOrEmpty(targetScreen))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.TargetScreen.Replace(":", ""));
            }

            // Description
            if (string.IsNullOrEmpty(description.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Description.Replace(":", ""));
            }

            // Screen Name
            if (string.IsNullOrEmpty(screenName.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ScreenName.Replace(":", ""));
            }

            return string.Empty;
        }

        /// <summary> Valid Control</summary>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidControl()
        {
            // Control Name
            if (string.IsNullOrEmpty(txtControlName.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ControlName.Replace(":", ""));
            }

            // Control Type - No value
            if (string.IsNullOrEmpty(cboControlType.SelectedItem.ToString()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ControlType.Replace(":", ""));
            }

            // Control Type - Cannot change from 'container' type to 'standard' type
            if (_modeType.Equals(ModeType.Edit))
            {
                // Get existing element and type
                var element = (XElement) _clickedControlTreeNode.Tag;
                var typeAttribute = element.Attributes(ProcessGeneration.AttributeType).FirstOrDefault();

                if (typeAttribute != null)
                {
                    var oldType = typeAttribute.Value;
                    var newType = cboControlType.SelectedItem.ToString();

                    // Compare old type to new type
                    // NOTE: Okay to go from a non-container to container
                    if (oldType.Equals(ControlTypeTabPage) || oldType.Equals(ControlTypePanel))
                    {
                        if (!newType.Equals(ControlTypeTabPage) || !newType.Equals(ControlTypePanel))
                        {
                            return Resources.InvalidSettingControlTypeChange;
                        }
                    }
                }
            }

            // Placement ID
            if (txtPlacementID.Enabled && string.IsNullOrEmpty(txtPlacementID.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.PlacementId.Replace(":", ""));
            }

            // Header Placement ID
            if (txtHeaderPlacementID.Enabled && string.IsNullOrEmpty(txtHeaderPlacementID.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.HeaderPlacementID.Replace(":", ""));
            }

            // Placement ID
            if (txtDetailPlacementID.Enabled && string.IsNullOrEmpty(txtDetailPlacementID.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.DetailPlacementID.Replace(":", ""));
            }

            // Max Length
            if (txtMaxLength.Enabled)
            {
                if (txtMaxLength.Text.Trim().Length > 0)
                {
                    try
                    {
                        if (Convert.ToInt32(txtMaxLength.Text.Trim()) < 0)
                        {
                            return string.Format(Resources.InvalidSettingNumericValue,
                                Resources.MaxLength.Replace(":", ""));
                        }
                    }
                    catch (Exception)
                    {
                        return string.Format(Resources.InvalidSettingNumericField,
                            Resources.MaxLength.Replace(":", ""));
                    }
                }
            }

            // Cols
            if (txtControlCols.Enabled)
            {
                if (string.IsNullOrEmpty(txtControlCols.Text.Trim()))
                {
                    return string.Format(Resources.InvalidSettingRequiredField,
                        Resources.ControlColumns.Replace(":", ""));
                }
                try
                {
                    if (Convert.ToInt32(txtControlCols.Text.Trim()) < 0)
                    {
                        return string.Format(Resources.InvalidSettingNumericValue,
                            Resources.ControlColumns.Replace(":", ""));
                    }
                }
                catch (Exception)
                {
                    return string.Format(Resources.InvalidSettingNumericField,
                        Resources.ControlColumns.Replace(":", ""));
                }
            }

            // Rows
            if (txtControlRows.Enabled)
            {
                if (string.IsNullOrEmpty(txtControlRows.Text.Trim()))
                {
                    return string.Format(Resources.InvalidSettingRequiredField,
                        Resources.ControlRows.Replace(":", ""));
                }
                try
                {
                    if (Convert.ToInt32(txtControlRows.Text.Trim()) < 0)
                    {
                        return string.Format(Resources.InvalidSettingNumericValue,
                            Resources.ControlRows.Replace(":", ""));
                    }
                }
                catch (Exception)
                {
                    return string.Format(Resources.InvalidSettingNumericField,
                        Resources.ControlRows.Replace(":", ""));
                }
            }

            // Finder Text
            if (txtFinderTextID.Enabled && string.IsNullOrEmpty(txtFinderTextID.Text.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.FinderTextID.Replace(":", ""));
            }

            return string.Empty;
        }

        /// <summary> Cancel any screen changes</summary>
        private void CancelScreen()
        {
            // For added screen, remove last node as this is where the next node was placed
            if (_modeType == ModeType.Add)
            {
                // Remove from screen list first
                var screen = (Screen)_clickedScreenTreeNode.LastNode.Tag;
                _screens.Remove(screen);

                // Remove from tree
                _clickedScreenTreeNode.Nodes.Remove(_clickedScreenTreeNode.LastNode);
            }

            // For edit, reset color
            if (_modeType == ModeType.Edit)
            {
                SetNodeColor(_clickedScreenTreeNode, false);
            }

            // Reset mode
            _modeType = ModeType.None;

            // Enable screens controls
            EnableScreensControls(true);

            // Clear inidividual screen controls
            ClearScreenControls(ClearType.All);

            // Disable screen controls
            EnableScreenControls(false);

            // Set focus back to tree
            treeScreens.Focus();

        }

        /// <summary> Edit Screen</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void EditScreenMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Setup items for edit of screen
            ScreenSetup(_clickedScreenTreeNode, ModeType.Edit);
        }

        /// <summary> Show menu for control node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void treeControls_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Treeview control was right clicked
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            // Do nothing if Screens was clicked
            if (e.Node.Text.Equals(ProcessGeneration.ElementScreens))
            {
                return;
            }

            // Do nothing (no context menus) if mode is not none (i.e. it is in an edit or add state)
            if (!_modeType.Equals(ModeType.None))
            {
                return;
            }

            // Locals
            var node = (XElement)e.Node.Tag;
            var nodeName = node.Name.LocalName;
            var type = string.Empty;
            var attribute = node.Attribute(ProcessGeneration.AttributeType);
            if (attribute != null)
            {
                type = attribute.Value;
            }

            // Show Add Control menu if Screen was clicked
            if (nodeName.Equals(ProcessGeneration.ElementScreen))
            {
                // Context menu to contain "Add"
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_addControlMenuItem);

                // Save node clicked
                _clickedControlTreeNode = e.Node;

                // Show menu
                _contextMenu.Show(treeControls, e.Location);
                return;
            }

            // Show Add, Edit, Delete menu if Container Control (Tab Page/Panel) was clicked
            if (nodeName.Equals(ProcessGeneration.ElementControl) && (type.Equals(ControlTypeTabPage) || type.Equals(ControlTypePanel)))
            {
                // Context menu to contain "Add, Edit, Delete
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_addControlMenuItem);
                _contextMenu.MenuItems.Add(_editControlMenuItem);
                _contextMenu.MenuItems.Add(_deleteControlMenuItem);

                // Save node clicked
                _clickedControlTreeNode = e.Node;

                // Show menu
                _contextMenu.Show(treeControls, e.Location);
                return;
            }

            // Show Insert Before, Insert After, Edit, Delete menu if Control was clicked
            if (nodeName.Equals(ProcessGeneration.ElementControl))
            {
                // Context menu to contain "Insert Before, Insert After, Edit, Delete
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_insertControlAboveMenuItem);
                _contextMenu.MenuItems.Add(_insertControlBelowMenuItem);
                _contextMenu.MenuItems.Add(_editControlMenuItem);
                _contextMenu.MenuItems.Add(_deleteControlMenuItem);

                // Save node clicked
                _clickedControlTreeNode = e.Node;

                // Show menu
                _contextMenu.Show(treeControls, e.Location);
            }
        }

        /// <summary> Control Type is changing</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If selecting blank, then all controls are enabled
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                // Enable all controls
                EnableControlControls();
            }
            else
            {
                // Enable controls based upon selected control type
                EnableControlControls((ControlType) ((ComboBox) sender).SelectedItem);
            }

        }

        /// <summary> Control has changed, so update remaining controls</summary>
        private void SaveControl()
        {
            // Validation
            var success = ValidControl();
            if (!string.IsNullOrEmpty(success))
            {
                DisplayMessage(success, MessageBoxIcon.Error);
                return;
            }

            TreeNode treeNode = null; 

            // New, insert or edit?
            switch (_modeType)
            {
                case ModeType.Add:
                    treeNode = _clickedControlTreeNode.LastNode;
                    break;
                case ModeType.AddAbove:
                    treeNode = _clickedControlTreeNode.PrevNode;
                    break;
                case ModeType.AddBelow:
                    treeNode = _clickedControlTreeNode.NextNode;
                    break;
                case ModeType.Edit:
                    treeNode = _clickedControlTreeNode;
                    break;
            }

            if (treeNode != null)
            {
                var element = (XElement)treeNode.Tag;

                // Remove all attributes
                element.RemoveAttributes();

                // Control Name
                element.Add(new XAttribute(ProcessGeneration.AttributeId, txtControlName.Text.Trim()));

                // Control Type
                element.Add(new XAttribute(ProcessGeneration.AttributeType, cboControlType.SelectedItem.ToString()));

                // Control Label
                if (txtControlLabel.Enabled && txtControlLabel.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeLabel, txtControlLabel.Text.Trim()));
                }

                // Control Binding
                if (txtControlBinding.Enabled && txtControlBinding.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeBinding, txtControlBinding.Text.Trim()));
                }

                // Placement ID
                if (txtPlacementID.Enabled && txtPlacementID.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute((chkBeforeID.Checked ? 
                        ProcessGeneration.AttributeBeforeId : 
                        ProcessGeneration.AttributeAfterId), txtPlacementID.Text.Trim()));
                }

                // Header Placement ID
                if (txtHeaderPlacementID.Enabled && txtHeaderPlacementID.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute((chkBeforeHeaderID.Checked ? 
                        ProcessGeneration.AttributeHeaderBeforeId : 
                        ProcessGeneration.AttributeHeaderAfterId), txtHeaderPlacementID.Text.Trim()));
                }

                // Detail Placement ID
                if (txtDetailPlacementID.Enabled && txtDetailPlacementID.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute((chkBeforeDetailID.Checked ? 
                        ProcessGeneration.AttributeDetailBeforeId : 
                        ProcessGeneration.AttributeDetailAfterId), txtDetailPlacementID.Text.Trim()));
                }

                // Max Length
                if (txtMaxLength.Enabled && txtMaxLength.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeMaxLength, txtMaxLength.Text.Trim()));
                }

                // Cols
                if (txtControlCols.Enabled && txtControlCols.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeCols, txtControlCols.Text.Trim()));
                }

                // Rows
                if (txtControlRows.Enabled && txtControlRows.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeRows, txtControlRows.Text.Trim()));
                }

                // Finder Text ID
                if (txtFinderTextID.Enabled && txtFinderTextID.Text.Trim().Length > 0)
                {
                    element.Add(new XAttribute(ProcessGeneration.AttributeFinderTextId, txtFinderTextID.Text.Trim()));
                }

                // Update tree node name
                element.Name = ProcessGeneration.ElementControl;
                treeNode.Text = BuildControlText(element);
                treeNode.Tag = element;

                // Need to sync screen tree
                if (_modeType != ModeType.Edit)
                {
                    // Control added so ControlConfiguration may need to be updated
                    var screenNode = GetScreenNode(element);
                    UpdateScreen(screenNode);
                }
            }

            // Reset mode
            _modeType = ModeType.None;

            // Reset selected color
            SetNodeColor(treeNode, false);

            // Enable controls controls
            EnableControlsControls(true);

            // Clear individual control controls
            ClearControlControls();

            // Disable control controls
            EnableControlControls(false);

            // Set focus back to tree
            treeControls.Focus();

        }

        /// <summary> Load Control Controls from Attributes of node clicked</summary>
        private void LoadControlControls()
        {
            // Get the node clicked
            var treeNode = _clickedControlTreeNode;
            var element = (XElement)treeNode.Tag;

            // Control Name
            var attribute = element.Attribute(ProcessGeneration.AttributeId);
            if (attribute != null)
            {
                txtControlName.Text = attribute.Value;
            }

            // Control Type
            attribute = element.Attribute(ProcessGeneration.AttributeType);
            if (attribute != null)
            {
                cboControlType.Text = attribute.Value;
            }

            // Control Label
            attribute = element.Attribute(ProcessGeneration.AttributeLabel);
            if (attribute != null)
            {
                txtControlLabel.Text = attribute.Value;
            }

            // Control Binding
            attribute = element.Attribute(ProcessGeneration.AttributeBinding);
            if (attribute != null)
            {
                txtControlBinding.Text = attribute.Value;
            }

            // Placement ID
            attribute = element.Attribute(ProcessGeneration.AttributeBeforeId);
            if (attribute != null)
            {
                txtPlacementID.Text = attribute.Value;
                chkBeforeID.Checked = true;
            }

            attribute = element.Attribute(ProcessGeneration.AttributeAfterId);
            if (attribute != null)
            {
                txtPlacementID.Text = attribute.Value;
                chkBeforeID.Checked = false;
            }

            // Header Placement ID
            attribute = element.Attribute(ProcessGeneration.AttributeHeaderBeforeId);
            if (attribute != null)
            {
                txtHeaderPlacementID.Text = attribute.Value;
                chkBeforeHeaderID.Checked = true;
            }

            attribute = element.Attribute(ProcessGeneration.AttributeHeaderAfterId);
            if (attribute != null)
            {
                txtHeaderPlacementID.Text = attribute.Value;
                chkBeforeHeaderID.Checked = false;
            }

            // Detail Placement ID
            attribute = element.Attribute(ProcessGeneration.AttributeDetailBeforeId);
            if (attribute != null)
            {
                txtDetailPlacementID.Text = attribute.Value;
                chkBeforeDetailID.Checked = true;
            }

            attribute = element.Attribute(ProcessGeneration.AttributeDetailAfterId);
            if (attribute != null)
            {
                txtDetailPlacementID.Text = attribute.Value;
                chkBeforeDetailID.Checked = false;
            }

            // Max Length
            attribute = element.Attribute(ProcessGeneration.AttributeMaxLength);
            if (attribute != null)
            {
                txtMaxLength.Text = attribute.Value;
            }

            // Cols
            attribute = element.Attribute(ProcessGeneration.AttributeCols);
            if (attribute != null)
            {
                txtControlCols.Text = attribute.Value;
            }

            // Rows
            attribute = element.Attribute(ProcessGeneration.AttributeRows);
            if (attribute != null)
            {
                txtControlRows.Text = attribute.Value;
            }

            // Finder Text ID
            attribute = element.Attribute(ProcessGeneration.AttributeFinderTextId);
            if (attribute != null)
            {
                txtFinderTextID.Text = attribute.Value;
            }

        }

        /// <summary> Load Screen Controls from Screen of node clicked</summary>
        private void LoadScreenControls()
        {
            // Get the node clicked
            var treeNode = _clickedScreenTreeNode;
            var screen = (Screen)treeNode.Tag;

            // Assign to controls
            cboModuleId.Text = screen.ModuleId.ToString();
            cboCategory.Text = screen.Category;
            cboTargetScreen.Text =
                GetTargetScreen(ProcessGeneration.Manifest + screen.ModuleId.ToString() + screen.Category +
                                screen.TargetScreen);
            txtDescription.Text = screen.Description;
            txtScreenName.Text = screen.ScreenName;
            txtControlsConfig.Text = screen.ControlsConfiguration;
            txtControlsBehavior.Text = screen.ControlsBehavior;
        }

        /// <summary> Cancel any control changes</summary>
        private void CancelControl()
        {
            // For added control, remove last node as this is where the next node was placed
            if (_modeType == ModeType.Add)
            {
                // Remove from XDocument first
                var node = (XElement)_clickedControlTreeNode.LastNode.Tag;
                node.Remove();

                // Remove from tree
                _clickedControlTreeNode.Nodes.Remove(_clickedControlTreeNode.LastNode);
            }

            // For added above control, remove previous node
            if (_modeType == ModeType.AddAbove)
            {
                // Remove from XDocument first
                var node = (XElement)_clickedControlTreeNode.PrevNode.Tag;
                node.Remove();

                // Remove from tree
                _clickedControlTreeNode.PrevNode.Remove();
            }

            // For added below control, remove next node
            if (_modeType == ModeType.AddBelow)
            {
                // Remove from XDocument first
                var node = (XElement)_clickedControlTreeNode.NextNode.Tag;
                node.Remove();

                // Remove from tree
                _clickedControlTreeNode.NextNode.Remove();
            }

            // For edit, eset color
            if (_modeType  == ModeType.Edit)
            {
                SetNodeColor(_clickedControlTreeNode, false);

            }

            // Need to sync screen tree
            if (_modeType != ModeType.Edit)
            {
                // Control cancel so ControlConfiguration may need to be updated
                var screenNode = GetScreenNode((XNode)_clickedControlTreeNode.Tag);
                UpdateScreen(screenNode);
            }

            // Reset mode
            _modeType = ModeType.None;

            // Enable controls controls
            EnableControlsControls(true);

            // Clear individual control controls
            ClearControlControls();

            // Disable control controls
            EnableControlControls(false);

            // Set focus back to tree
            treeControls.Focus();
        }

        /// <summary> Do not allow space characters in content</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private static void NoWhiteSpaceKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        /// <summary> EULA search dialog</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnEula_Click(object sender, EventArgs e)
        {
            // Init dialog
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.FilterAll,
                FilterIndex = 1,
                Multiselect = false
            };

            // Show the dialog and evaluate action
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Display eula selected
            txtEula.Text = dialog.FileName.Trim();
        }

        /// <summary> Save Screen or Control changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Screens
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelScreens))
            {
                SaveScreen();
            }

            // Controls
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelControls))
            {
                SaveControl();
            }
        }

        /// <summary> Cancel Screen or control changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Screens
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelScreens))
            {
                CancelScreen();
            }

            // Controls
            if (_wizardSteps[_currentWizardStep].Panel.Name.Equals(PanelControls))
            {
                CancelControl();
            }

        }

        /// <summary> Show menu for screen node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void treeScreens_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Treeview control was right clicked
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            // Do nothing (no context menus) if mode is not none (i.e. it is in an edit or add state)
            if (!_modeType.Equals(ModeType.None))
            {
                return;
            }

            // Show Add and Delete All menu if Screens was clicked
            if (e.Node.Name.Equals(ProcessGeneration.ElementScreens))
            {
                // Context menu to contain "Add, Delete All"
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_addScreenMenuItem);
                _contextMenu.MenuItems.Add(_deleteScreensMenuItem);
            }
            else
            {
                // Show Edit, Delete menu for screen
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_editScreenMenuItem);
                _contextMenu.MenuItems.Add(_deleteScreenMenuItem);
            }

            // Save node clicked
            _clickedScreenTreeNode = e.Node;

            // Show menu
            _contextMenu.Show(treeScreens, e.Location);
        }

        /// <summary> Remove Eula from field since field is read only</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteEula_Click(object sender, EventArgs e)
        {
            txtEula.Text = string.Empty;
        }

        /// <summary> Remove Assembly from field since field is read only</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteAssembly_Click(object sender, EventArgs e)
        {
            txtAssembly.Text = string.Empty;
        }

        /// <summary> Remove Bootstrapper from field since field is read only</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteBootstrapper_Click(object sender, EventArgs e)
        {
            txtBootstrapper.Text = string.Empty;
        }

        #endregion

    }
}
