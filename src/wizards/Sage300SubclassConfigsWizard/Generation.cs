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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.CA.SBS.ERP.Sage300.SubclassConfigsWizard.Properties;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MetroFramework.Forms;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.SubclassConfigsWizard
{
    /// <summary> UI for Subclassing Configurations Wizard </summary>
    public partial class Generation : MetroForm
    {
        #region Private Variables

        /// <summary> List of Configurations </summary>
        private List<Configuration> _configurations = new List<Configuration>();

        /// <summary> List of Models by Module </summary>
        private Dictionary<string, List<string>> _models = new Dictionary<string, List<string>>();

        /// <summary> Mode Type for Add, Edit or None </summary>
        private ModeType _modeType = ModeType.None;

        /// <summary> Clicked Configuration Node </summary>
        private TreeNode _clickedConfigurationTreeNode;

        /// <summary> Menu Item for Add Configuration </summary>
        private readonly MenuItem _addConfigurationMenuItem = new MenuItem(Resources.AddConfiguration);

        /// <summary> Menu Item for Edit Configuration </summary>
        private readonly MenuItem _editConfigurationMenuItem = new MenuItem(Resources.EditConfiguration);

        /// <summary> Menu Item for Delete Configuration </summary>
        private readonly MenuItem _deleteConfigurationMenuItem = new MenuItem(Resources.DeleteConfiguration);

        /// <summary> Menu Item for Delete Configurations </summary>
        private readonly MenuItem _deleteConfigurationsMenuItem = new MenuItem(Resources.DeleteConfigurations);

        /// <summary> Context Menu </summary>
        private readonly ContextMenu _contextMenu = new ContextMenu();

        /// <summary> Subclassing folder </summary>
        private string _subclassingFolder = RegistryHelper.Sage300SubclassingFolder;

        /// <summary> Configuration Fields </summary>
        private readonly BindingList<Property> _propertyFields = new BindingList<Property>();

        #endregion

        #region Public Constants
        private class Constants
        {
            /// <summary> Splitter Distance </summary>
            public const int SplitterDistance = 415;

            /// <summary> Wizard Window Width </summary>
            public const int WizardWidth = 1000;

            /// <summary> Wizard Window Height </summary>
            public const int WizardHeight = 635;

            /// <summary> Property for Id </summary>
            public const string PropertyId = "Id";

            /// <summary> Property for Name </summary>
            public const string PropertyName = "Name";

            /// <summary> Property for BusinessPartnerName </summary>
            public const string PropertyBusinessPartnerName = "BusinessPartnerName";

            /// <summary> Property for Module ID </summary>
            public const string PropertyModuleId = "ModuleId";

            /// <summary> Property for Model </summary>
            public const string PropertyModel = "Model";

            /// <summary> Property for GeneratedMessage </summary>
            public const string PropertyGeneratedMessage = "GeneratedMessage";

            /// <summary> Property for GeneratedWarning </summary>
            public const string PropertyGeneratedWarning = "GeneratedWarning";

            /// <summary> Element Name for Configurations </summary>
            public const string ElementConfigurations = "Configurations";

            /// <summary> Element Name for Configurations </summary>
            public const string JSON_EXT = ".json";

            /// <summary> Property for Properties </summary>
            public const string PropertyProperties = "Properties";

            /// <summary> Property for FieldName </summary>
            public const string PropertyFieldName = "FieldName";

            /// <summary> Property for FieldType </summary>
            public const string PropertyFieldType = "FieldType";

            /// <summary> Property for DataType </summary>
            public const string PropertyDataType = "DataType";

            /// <summary> Property for Mask </summary>
            public const string PropertyMask = "Mask";

            /// <summary> Property for Size </summary>
            public const string PropertySize = "Size";

            /// <summary> Property for Precision </summary>
            public const string PropertyPrecision = "Precision";
        }
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

            /// <summary> Clear Model and downstream </summary>
            Model = 2,

            /// <summary> Clear remaining </summary>
            Other = 4
        }

        /// <summary>
        /// Enum for Mode Types
        /// </summary>
        private enum ModeType
        {
            /// <summary> None Mode </summary>
            None = 0,

            /// <summary> Add Mode </summary>
            Add = 1,

            /// <summary> Edit Mode</summary>
            Edit = 2
        }

        #endregion

        #region Public Routines

        /// <summary> Generation Class </summary>
        public Generation()
        {
            InitializeComponent();
            Localize();
            InitConfigurations();
            InitEvents();
            InitPropertyFields();
            ProcessingSetup(true);
            GetExistingConfigurations();

            // Set the final wizard dimensions
            Width = Constants.WizardWidth;
            Height = Constants.WizardHeight;
        }

        #endregion

        #region Private Routines

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
            // Configuration Events
            _addConfigurationMenuItem.Click += AddConfigurationMenuItemOnClick;
            _editConfigurationMenuItem.Click += EditConfigurationMenuItemOnClick;
            _deleteConfigurationMenuItem.Click += DeleteConfigurationMenuItemOnClick;
            _deleteConfigurationsMenuItem.Click += DeleteConfigurationsMenuItemOnClick;

        }

        /// <summary> ValidateFields</summary>
        /// <returns> String.Empty if valid otherwise message</returns>
        private string ValidateFields()
        {
            var validFields = string.Empty;

            // Must have at least one property
            if (_propertyFields.Count == 0)
            {
                return Resources.InvalidSettingCount;
            }

            // Iterate fields
            for (var i = 0; i < _propertyFields.Count; i++)
            {
                // Locals
                var field = _propertyFields[i];

                // Field Name
                if (string.IsNullOrEmpty(field.FieldName.Trim()))
                {
                    validFields = string.Format(Resources.InvalidSettingRequiredField, Resources.FieldName);
                    break;
                }

                if (field.FieldName.Trim().Contains(" "))
                {
                    validFields = string.Format(Resources.InvalidSettingSpaces, Resources.FieldName);
                    break;
                }

                // Id
                if (field.Id <= 0)
                {
                    validFields = string.Format(Resources.InvalidSettingValue, Resources.ID);
                    break;
                }

                // Friendly Name
                if (string.IsNullOrEmpty(field.Name.Trim()))
                {
                    validFields = string.Format(Resources.InvalidSettingRequiredField, Resources.PropertyName);
                    break;
                }

                if (field.Name.Trim().Contains(" "))
                {
                    validFields = string.Format(Resources.InvalidSettingSpaces, Resources.PropertyName);
                    break;
                }

                // Size
                if (field.Size < 0)
                {
                    validFields = string.Format(Resources.InvalidSettingValue, Resources.Size);
                    break;
                }

                // Precision
                if (field.Precision < 0)
                {
                    validFields = string.Format(Resources.InvalidSettingValue, Resources.Precision);
                    break;
                }

            }

            return validFields;
        }

        /// <summary> Delete configuration Node</summary>
        /// <param name="treeNode">Tree Node to delete </param>
        private void DeleteConfigurationNode(TreeNode treeNode)
        {
            // Remove from configuration
            var configuration = (Configuration)treeNode.Tag;
            _configurations.Remove(configuration);

            // Remove the tree node
            treeNode.Remove();

            // Delete from file, if exists
            var companyPath = Path.Combine(_subclassingFolder, configuration.CompanyName.Trim().Replace(" ", ""));
            var companyModulePath = Path.Combine(companyPath, configuration.ModuleId);
            var fileName = Path.Combine(companyModulePath, configuration.Name + Constants.JSON_EXT);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Delete directory if no remaining configurations
            DeleteDirectory(companyModulePath);
            DeleteDirectory(companyPath);
        }

        /// <summary> Delete configuration path if no configurations remain</summary>
        /// <param name="path">Path to check for configurations </param>
        private void DeleteDirectory(string path)
        {
            // Delete directory if no remaining configurations
            var files = Directory.GetFiles(path, "*" + Constants.JSON_EXT, SearchOption.AllDirectories);

            if (files.Count() == 0)
            {
                Directory.Delete(path);
            }
        }

        /// <summary> Configuration Setup</summary>
        /// <param name="treeNode">Tree node</param>
        /// <param name="modeType">Mode Type (Add)</param>
        private void ConfigurationSetup(TreeNode treeNode, ModeType modeType)
        {
            // If not edit mode
            if (!modeType.Equals(ModeType.Edit))
            {
                // Expand clicked node
                _clickedConfigurationTreeNode.ExpandAll();

                // Add to tree
                treeConfigurations.SelectedNode = treeNode;
            }

            // Set color of node
            SetNodeColor(treeNode, true);

            // Set mode type and clear controls
            _modeType = modeType;
            ClearConfigurationControls(ClearType.All);

            // Enable Configuration controls
            EnableConfigurationControls(true);

            // If edit mode
            if (modeType.Equals(ModeType.Edit))
            {
                // Load controls from configuration
                LoadConfigurationControls();
            }

            // Set focus to Name
            txtName.Focus();
        }

        /// <summary> New Configuration</summary>
        /// <returns>Configuration</returns>
        private static Configuration NewConfiguration()
        {
            // New configuration
            return new Configuration {Id = Guid.NewGuid() };
        }

        /// <summary> New configuration tree node</summary>
        /// <param name="configuration">Configuration</param>
        private static TreeNode NewConfigurationTreeNode(Configuration configuration)
        {
            // Build new tree node
            var name = BuildConfigurationNodeName(configuration);
            var text = Resources.New;
            var treeNode = new TreeNode(text)
            {
                Tag = configuration,
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

        /// <summary> Localize </summary>
        private void Localize()
        {
            Text = Resources.WebSubclassing;

            lblStepTitle.Text = Resources.StepTitleConfigurations;
            lblStepDescription.Text = string.Format(Resources.StepDescriptionConfigurations, _subclassingFolder);

            btnSave.Text = Resources.Save;
            btnCancel.Text = Resources.Cancel;

            // Step - Configurations
            lblName.Text = Resources.SubclassingName;
            tooltip.SetToolTip(lblName, Resources.SubclassingNameTip);

            lblCompanyName.Text = Resources.CompanyName;
            tooltip.SetToolTip(lblCompanyName, Resources.BusinessPartnerNameTip);

            lblModuleId.Text = Resources.ModuleId;
            tooltip.SetToolTip(lblModuleId, Resources.ModuleIdTip);

            lblModel.Text = Resources.Model;
            tooltip.SetToolTip(lblModel, Resources.ModelTip);

            btnRowAdd.ToolTipText = Resources.AddRow;
            btnDeleteRow.ToolTipText = Resources.DeleteRow;
            btnDeleteRows.ToolTipText = Resources.DeleteRows;

            splitSteps.SplitterDistance = Constants.SplitterDistance;
        }

        /// <summary> Generic routine for displaying a message dialog </summary>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        /// <param name="args">Message arguments, if any</param>
        private void DisplayMessage(string message, MessageBoxIcon icon, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Text, MessageBoxButtons.OK, icon);
        }

        /// <summary> Get existing JSON Configurations</summary>
        private void GetExistingConfigurations()
        {
            // Get models from enbedded JSON
            _models = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(Encoding.UTF8.GetString(Resources.ModelsSource));

            // Clear binding list
            _configurations.Clear();

            // Create directory if does not exist
            if (!Directory.Exists(_subclassingFolder))
            {
                Directory.CreateDirectory(_subclassingFolder);
            }

            // Get any configurations found
            var files = Directory.GetFiles(_subclassingFolder, "*" + Constants.JSON_EXT, SearchOption.AllDirectories);

            // Iterate through each configuration file found
            foreach (var file in files)
            {
                // Get the configuration
                var json = JObject.Parse(File.ReadAllText(file));
                // Build configuration object
                var configuration = new Configuration()
                {
                    Id = (Guid)json.SelectToken(Constants.PropertyId),
                    Name = (string)json.SelectToken(Constants.PropertyName),
                    CompanyName = (string)json.SelectToken(Constants.PropertyBusinessPartnerName),
                    ModuleId = (string)json.SelectToken(Constants.PropertyModuleId),
                    Model = (string)json.SelectToken(Constants.PropertyModel)
                };

                var properties =
                    from property in json[Constants.PropertyProperties]
                    select new
                    {
                        FieldName = (string)property[Constants.PropertyFieldName],
                        Id = (int)property[Constants.PropertyId],
                        FieldType = (string)property[Constants.PropertyFieldType],
                        Name = (string)property[Constants.PropertyName],
                        DataType = (string)property[Constants.PropertyDataType],
                        Mask = (string)property[Constants.PropertyMask],
                        Size = (int)property[Constants.PropertySize],
                        Precision = (int)property[Constants.PropertyPrecision]
                    };

                // Iterate and add to binding
                foreach (var property in properties)
                {
                    configuration.Properties.Add(new Property
                    {
                        FieldName = property.FieldName,
                        Id = property.Id,
                        FieldType = (FieldType)Enum.Parse(typeof(FieldType), property.FieldType),
                        Name = property.Name,
                        DataType = (DataType)Enum.Parse(typeof(DataType), property.DataType),
                        Mask = property.Mask,
                        Size = property.Size,
                        Precision = property.Precision
                    });

                }

                // Add to list of configurations
                _configurations.Add(configuration);
            }

            // Load tree based upon configurations
            LoadConfigurationsTree();

            // Clear configuration and options displays
            ClearAll();
        }

        /// <summary> Clear All inputs</summary>
        private void ClearAll()
        {
            // Default Properties
            txtName.Text = string.Empty;
            txtCompanyName.Text = string.Empty;

            ClearConfigurationControls(ClearType.All);
        }

        /// <summary> Generate JSON Configuration</summary>
        private JObject GenerateConfiguration(Configuration configuration)
        {
            // Add properties
            var json = new JObject
            {
                new JProperty(Constants.PropertyGeneratedMessage, Resources.GeneratedMessage),
                new JProperty(Constants.PropertyGeneratedWarning, Resources.GeneratedWarning),
                new JProperty(Constants.PropertyId, configuration.Id.ToString()),
                new JProperty(Constants.PropertyName, configuration.Name),
                new JProperty(Constants.PropertyBusinessPartnerName, configuration.CompanyName),
                new JProperty(Constants.PropertyModuleId, configuration.ModuleId),
                new JProperty(Constants.PropertyModel, configuration.Model)
            };

            // Create array of properties
            var jArray = new JArray();

            foreach (var property in configuration.Properties)
            {
                var properties = new JObject
                {
                    new JProperty(Constants.PropertyFieldName, property.FieldName),
                    new JProperty(Constants.PropertyId, property.Id),
                    new JProperty(Constants.PropertyFieldType, property.FieldType),
                    new JProperty(Constants.PropertyName, property.Name),
                    new JProperty(Constants.PropertyDataType, property.DataType),
                    new JProperty(Constants.PropertyMask, property.Mask),
                    new JProperty(Constants.PropertySize, property.Size),
                    new JProperty(Constants.PropertyPrecision, property.Precision)
                };

                jArray.Add(properties);
            }

            json.Add(new JProperty(Constants.PropertyProperties, jArray));

            // Create company folder if does not exist
            var path = Path.Combine(_subclassingFolder, configuration.CompanyName.Trim().Replace(" ", ""), configuration.ModuleId);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Save file
            var fileName = Path.Combine(path, configuration.Name + Constants.JSON_EXT);
            File.WriteAllText(fileName, json.ToString());

            return json;
        }

        /// <summary> Enable or disable configuration controls</summary>
        /// <param name="enable">true to enable otherwise false </param>
        private void EnableConfigurationControls(bool enable)
        {
            btnSave.Visible = enable;
            btnCancel.Visible = enable;
            splitConfigurations.Panel2.Enabled = enable;

            // Special logic for edit
            if (_modeType == ModeType.Edit)
            {
                txtName.Enabled = false;
                txtCompanyName.Enabled = false;
                cboModuleId.Enabled = false;
                cboModel.Enabled = false;
            }
            else
            {
                txtName.Enabled = true;
                txtCompanyName.Enabled = true;
                cboModuleId.Enabled = true;
                cboModel.Enabled = true;
            }
        }

        /// <summary> Add a configuration</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void AddConfigurationMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Build new configuration
            var configuration = NewConfiguration();

            // Build new tree node
            var treeNode = NewConfigurationTreeNode(configuration);

            // Add to configurations
            _configurations.Add(configuration);
            _clickedConfigurationTreeNode.Nodes.Add(treeNode);

            ConfigurationSetup(treeNode, ModeType.Add);
        }

        /// <summary> Delete configuration</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteConfigurationMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Delete tree node
            DeleteConfigurationNode(_clickedConfigurationTreeNode);
        }

        /// <summary> Delete all configurations</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void DeleteConfigurationsMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var treeNodes = _clickedConfigurationTreeNode.Nodes;

            for (var i = treeNodes.Count - 1; i > -1; i--)
            {
                DeleteConfigurationNode(treeNodes[i]);
            }
        }

        /// <summary> Initialize configuration info </summary>
        private void InitConfigurations()
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

            ClearAll();
            ClearConfigurationControls(ClearType.All);
            EnableConfigurationControls(false);
        }

        /// <summary> Clear configuration Controls </summary>
        /// <param name="clearType">Enumeration to clear controls</param>
        private void ClearConfigurationControls(ClearType clearType)
        {
            // Hierarchical approach

            if (clearType == ClearType.All ||
                clearType == ClearType.Module)
            {
                cboModuleId.SelectedIndex = 0;
            }

            if (clearType == ClearType.All ||
                clearType == ClearType.Module ||
                clearType == ClearType.Model)
            {
                cboModel.Items.Clear();
                cboModel.Items.Add(string.Empty);
                cboModel.SelectedIndex = 0;
            }

            if (clearType == ClearType.All ||
                clearType == ClearType.Module ||
                clearType == ClearType.Model ||
                clearType == ClearType.Other)
            {
                InitPropertyFields();
            }

        }

        /// <summary> Load the configurations into the tree</summary>
        private void LoadConfigurationsTree()
        {
            // Clear tree first
            treeConfigurations.Nodes.Clear();

            // Add top level node
            var configurationsNode = new TreeNode(Constants.ElementConfigurations) {Name = Constants.ElementConfigurations };
            treeConfigurations.Nodes.Add(configurationsNode);

            // Iterate configurations and add
            foreach (var configuration in _configurations)
            {
                // Build new tree node
                var name = BuildConfigurationNodeName(configuration);
                var text = BuildConfigurationText(configuration);
                var newTreeNode = new TreeNode(text)
                {
                    Tag = configuration,
                    Name = name
                };
                configurationsNode.Nodes.Add(newTreeNode);
            }
        }

        /// <summary> Build configuration Node Name</summary>
        /// <param name="configuration">configuration </param>
        /// <returns>Name for Node</returns>
        private static string BuildConfigurationNodeName(Configuration configuration)
        {
            return configuration.Id.ToString();
        }

        /// <summary> Build Text from configuration</summary>
        /// <param name="configuration">Configuration to build text from</param>
        /// <returns>Text</returns>
        private static string BuildConfigurationText(Configuration configuration)
        {
            var text = Constants.PropertyModuleId + "=\"" + configuration.ModuleId + "\" ";
            text += Constants.PropertyModel + "=\"" + configuration.Model + "\" ";
            text += Constants.PropertyName + "=\"" + configuration.Name + "\" ";
            text += Constants.PropertyBusinessPartnerName + "=\"" + configuration.CompanyName + "\" ";
            text += Constants.PropertyProperties + "=\"" + configuration.Properties.Count + "\" ";

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
                ClearConfigurationControls(ClearType.Model);
            }
            else
            {
                // Get module selected and populate the model list
                var moduleId = ((ComboBox) sender).SelectedItem.ToString();
                LoadModels(moduleId);
                ClearConfigurationControls(ClearType.Other);
            }
        }

        /// <summary> Load models for the selected module</summary>
        /// <param name="moduleId">Module</param>
        private void LoadModels(string moduleId)
        {
            cboModel.Items.Clear();

            // Get models for module
            var models = _models[moduleId].ToList();
            foreach (var model in models)
            {
                cboModel.Items.Add(model);
            }
            cboModel.SelectedIndex = 0;
        }

        /// <summary> Model has changed, so update remaining controls</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void cboModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearConfigurationControls(ClearType.Other);
        }

        /// <summary> Configuration has changed, so update</summary>
        private void SaveConfiguration()
        {
            // Validation
            var success = ValidConfiguration(txtName.Text, txtCompanyName.Text,
                cboModuleId.SelectedItem.ToString(), cboModel.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(success))
            {
                DisplayMessage(success, MessageBoxIcon.Error);
                return;
            }

            var node = _modeType == ModeType.Add ? _clickedConfigurationTreeNode.LastNode : _clickedConfigurationTreeNode;
            var configuration = (Configuration)node.Tag;

            // Get values from controls and add to object
            configuration.Name = txtName.Text;
            configuration.CompanyName = txtCompanyName.Text;

            configuration.ModuleId = cboModuleId.SelectedItem.ToString();
            configuration.Model = cboModel.SelectedItem.ToString();
            configuration.Properties = _propertyFields.ToList();

            // Add/Update to tree
            node.Name = BuildConfigurationNodeName(configuration);
            node.Text = BuildConfigurationText(configuration);
            node.Tag = configuration;

            // Reset mode
            _modeType = ModeType.None;

            // Reset selected color
            SetNodeColor(node, false);

            // Clear individual configuration controls
            ClearAll();

            // Disable configuration controls
            EnableConfigurationControls(false);

            // Save configuration to disk
            GenerateConfiguration(configuration);

            // Set focus back to tree
            treeConfigurations.Focus();
        }

        /// <summary> Valid Configuration</summary>
        /// <param name="name">Name</param>
        /// <param name="companyName">Company Name</param>
        /// <param name="moduleId">Module Id</param>
        /// <param name="model">Model</param>
        /// <returns>string.Empty if valid otherwise message to display</returns>
        private string ValidConfiguration(string name, string companyName, string moduleId, string model)
        {
            // Name
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.SubclassingName.Replace(":", ""));
            }

            // Company Name
            if (string.IsNullOrEmpty(companyName.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.CompanyName.Replace(":", ""));
            }


            // Module ID
            if (string.IsNullOrEmpty(moduleId.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.ModuleId.Replace(":", ""));
            }

            // Model
            if (string.IsNullOrEmpty(model.Trim()))
            {
                return string.Format(Resources.InvalidSettingRequiredField, Resources.Model.Replace(":", ""));
            }

            // Validate properties
            string valid = ValidateFields();

            return valid;
        }

        /// <summary> Cancel any configuration changes</summary>
        private void CancelConfiguration()
        {
            // For added configuration, remove last node as this is where the next node was placed
            if (_modeType == ModeType.Add)
            {
                // Remove from configuration list first
                var configuration = (Configuration)_clickedConfigurationTreeNode.LastNode.Tag;
                _configurations.Remove(configuration);

                // Remove from tree
                _clickedConfigurationTreeNode.Nodes.Remove(_clickedConfigurationTreeNode.LastNode);
            }

            // For edit, reset color
            if (_modeType == ModeType.Edit)
            {
                SetNodeColor(_clickedConfigurationTreeNode, false);
            }

            // Reset mode
            _modeType = ModeType.None;

            // Clear inidividual configuration controls
            ClearAll();

            // Disable configuration controls
            EnableConfigurationControls(false);

            // Set focus back to tree
            treeConfigurations.Focus();
        }

        /// <summary> Edit Configuration</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="eventArgs">Event Args </param>
        private void EditConfigurationMenuItemOnClick(object sender, EventArgs eventArgs)
        {
            // Setup items for edit of configuration
            ConfigurationSetup(_clickedConfigurationTreeNode, ModeType.Edit);
        }

        /// <summary> Load Configuration Controls from Configuration of node clicked</summary>
        private void LoadConfigurationControls()
        {
            // Get the node clicked
            var treeNode = _clickedConfigurationTreeNode;
            var configuration = (Configuration)treeNode.Tag;

            // Assign to controls
            txtName.Text = configuration.Name;
            txtCompanyName.Text = configuration.CompanyName;

            cboModuleId.Text = configuration.ModuleId;
            cboModel.Text = configuration.Model;

            // Assign to the properties grid
            foreach (var property in configuration.Properties)
            {
                // Add to collection for data binding to grid
                _propertyFields.Add(property);
            }
        }

        /// <summary> Save Configuration changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        /// <summary> Cancel Configuration changes</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelConfiguration();
        }

        /// <summary> Show menu for configurations node clicked</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void treeConfigurations_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Treeview control was not right clicked
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            // Do nothing (no context menus) if mode is not none (i.e. it is in an edit or add state)
            if (!_modeType.Equals(ModeType.None))
            {
                return;
            }

            // Show Add and Delete All menu if Configurations was clicked
            if (e.Node.Name.Equals(Constants.ElementConfigurations))
            {
                // Context menu to contain "Add, Delete All"
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_addConfigurationMenuItem);
                _contextMenu.MenuItems.Add(_deleteConfigurationsMenuItem);
            }
            else
            {
                // Show Edit, Delete menu for configuration
                _contextMenu.MenuItems.Clear();
                _contextMenu.MenuItems.Add(_editConfigurationMenuItem);
                _contextMenu.MenuItems.Add(_deleteConfigurationMenuItem);
            }

            // Save node clicked
            _clickedConfigurationTreeNode = e.Node;

            // Show menu
            _contextMenu.Show(treeConfigurations, e.Location);
        }

        /// <summary> Delete all rows</summary>
        private void DeleteRows()
        {
            // Iterate grid
            for (int i = grdPropertyFields.Rows.Count - 1; i >= 0; i--)
            {
                grdPropertyFields.Rows.Remove(grdPropertyFields.Rows[i]);
            }
        }

        /// <summary> Generic init for grid </summary>
        /// <param name="grid">Grid control</param>
        /// <param name="column">Column Number</param>
        /// <param name="width">Column Width</param>
        /// <param name="text">Header Text</param>
        /// <param name="visible">True for visible otherwise False</param>
        /// <param name="readOnly">True for read only otherwise False</param>
        /// <param name="isInteger">True for integer columns otherwise False</param>
        private static void GenericInit(DataGridView grid,
                                        int column,
                                        int width,
                                        string text,
                                        bool visible,
                                        bool readOnly,
                                        bool isInteger)
        {
            grid.Columns[column].Width = width;
            grid.Columns[column].HeaderText = text;
            grid.Columns[column].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns[column].Visible = visible;
            grid.Columns[column].ReadOnly = readOnly;
            grid.Columns[column].Resizable = DataGridViewTriState.False;
            if (isInteger)
            {
                grid.Columns[column].ValueType = typeof(int);
                grid.Columns[column].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Show read only in InactiveCaption color
            if (readOnly)
            {
                grid.Columns[column].DefaultCellStyle.BackColor = SystemColors.InactiveCaption;
            }
        }

        /// <summary> Initialize property info and modify grid display </summary>
        private void InitPropertyFields()
        {
            // Clear data
            DeleteRows();

            var columnIndex = 0;
            if (grdPropertyFields.DataSource == null)
            {
                // Assign binding to datasource (two binding)
                grdPropertyFields.DataSource = _propertyFields;
                grdPropertyFields.ScrollBars = ScrollBars.Both;

                // Assign widths and localized text
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.FieldName, true, false, false);
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.ID, true, false, true);
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.FieldType, true, false, false);
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.PropertyName, true, false, false);
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.DataType, true, false, false);
                GenericInit(grdPropertyFields, columnIndex++, 125, Resources.Mask, true, false, false);
                GenericInit(grdPropertyFields, columnIndex++, 75, Resources.Size, true, false, true);
                GenericInit(grdPropertyFields, columnIndex++, 75, Resources.Precision, true, false, true);

                // Remove and re-add as combobox column
                columnIndex = 2;
                grdPropertyFields.Columns.Remove(Constants.PropertyFieldType);
                var column = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = Constants.PropertyFieldType,
                    HeaderText = Resources.FieldType,
                    DropDownWidth = 100,
                    Width = 100,
                    FlatStyle = FlatStyle.Flat
                };

                grdPropertyFields.Columns.Insert(columnIndex, column);
                grdPropertyFields.Columns[columnIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdPropertyFields.Columns[columnIndex].Visible = true;

                columnIndex = 4;
                grdPropertyFields.Columns.Remove(Constants.PropertyDataType);
                column = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = Constants.PropertyDataType,
                    HeaderText = Resources.DataType,
                    DropDownWidth = 100,
                    Width = 100,
                    FlatStyle = FlatStyle.Flat
                };

                grdPropertyFields.Columns.Insert(columnIndex, column);
                grdPropertyFields.Columns[columnIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdPropertyFields.Columns[columnIndex].Visible = true;
            }

            columnIndex = 2;
            var typeColumn = (DataGridViewComboBoxColumn)grdPropertyFields.Columns[columnIndex];
            typeColumn.Items.Clear();

            // Add all types
            foreach (
                var fieldType in
                Enum.GetValues(typeof(FieldType))
                    .Cast<FieldType>())
            {
                typeColumn.Items.Add(fieldType);
            }

            columnIndex = 4;
            typeColumn = (DataGridViewComboBoxColumn)grdPropertyFields.Columns[columnIndex];
            typeColumn.Items.Clear();

            // Add all types
            foreach (
                var dataType in
                Enum.GetValues(typeof(DataType))
                    .Cast<DataType>())
            {
                typeColumn.Items.Add(dataType);
            }

        }


        /// <summary> Add a row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnRowAdd_Click(object sender, EventArgs e)
        {
            _propertyFields.Add(new Property());
        }

        /// <summary> Delete the current row toolbar button </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (grdPropertyFields.CurrentRow != null)
            {
                grdPropertyFields.Rows.Remove(grdPropertyFields.CurrentRow);
            }
        }

        /// <summary> Delete all rows toolbar button</summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void btnDeleteRows_Click(object sender, EventArgs e)
        {
            DeleteRows();
        }

        /// <summary> Error from properties grid </summary>
        /// <param name="sender">Sender object </param>
        /// <param name="e">Event Args </param>
        private void grdPropertyFields_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DisplayMessage(e.Exception.Message, MessageBoxIcon.Error);
        }
        #endregion

    }
}
