// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Sage.CA.SBS.ERP.Sage300.CustomizationWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.CustomizationWizard
{
    /// <summary> Process Generation Class (worker) </summary>
    internal class ProcessGeneration
    {

        #region Private Variables

        /// <summary> Settings from UI </summary>
        private Settings _settings;

        #endregion

        #region Public constants
        public class Constants
        {
            /// <summary> Property for Module ID </summary>
            public const string PropertyModuleId = "ModuleId";

            /// <summary> Property for Name </summary>
            public const string PropertyName = "Name";

            /// <summary> Property for PackageId </summary>
            public const string PropertyPackageId = "PackageId";

            /// <summary> Property for Description </summary>
            public const string PropertyDescription = "Description";

            /// <summary> Property for GeneratedMessage </summary>
            public const string PropertyGeneratedMessage = "GeneratedMessage";

            /// <summary> Property for GeneratedWarning </summary>
            public const string PropertyGeneratedWarning = "GeneratedWarning";

            /// <summary> Property for BusinessPartnerName </summary>
            public const string PropertyBusinessPartnerName = "BusinessPartnerName";

            /// <summary> Property for SageCompatibility </summary>
            public const string PropertySageCompatibility = "SageCompatibility";

            /// <summary> Property for EULA </summary>
            public const string PropertyEula = "EULA";

            /// <summary> Property for Bootstrapper </summary>
            public const string PropertyBootstrapper = "Bootstrapper";

            /// <summary> Property for Assembly </summary>
            public const string PropertyAssembly = "Assembly";

            /// <summary> Property for Version </summary>
            public const string PropertyVersion = "Version";

            /// <summary> Property for WebScreens </summary>
            public const string PropertyWebScreens = "WebScreens";

            /// <summary> Property for ScreenName </summary>
            public const string PropertyScreenName = "ScreenName";

            /// <summary> Property for ScreenDescription </summary>
            public const string PropertyScreenDescription = "ScreenDescription";

            /// <summary> Property for TargetScreen </summary>
            public const string PropertyTargetScreen = "TargetScreen";

            /// <summary> Property for ControlsConfiguration </summary>
            public const string PropertyControlsConfiguration = "ControlsConfiguration";

            /// <summary> Property for ControlsBehavior </summary>
            public const string PropertyControlsBehavior = "ControlsBehavior";

            /// <summary> Property for Module </summary>
            public const string PropertyModule = "Module";

            /// <summary> Property for Category </summary>
            public const string PropertyCategory = "Category";

            /// <summary> Property for Screen </summary>
            public const string PropertyScreen = "Screen";

            /// <summary> Property for XML </summary>
            public const string PropertyXml = "XML";

            /// <summary> Property for JS </summary>
            public const string PropertyJs = "JS";

            /// <summary> Prefix for dictionary to provide a reverse lookup of screen name </summary>
            public const string Manifest = "Manifest";

            /// <summary> Suffix for XML file name </summary>
            public const string XmlFileNameSuffix = "_Settings.xml";

            /// <summary> Suffix for JavaScript file name </summary>
            public const string JavaScriptFileNameSuffix = "_Customization.js";

            /// <summary> File name for XSD file </summary>
            public const string XsdFileName = "screenConfig.xsd";

            /// <summary> File name for JSON file </summary>
            public const string JsonFileName = "Manifest.json";

            /// <summary> Suffix for Custom Description </summary>
            public const string CustomDescriptionSuffix = " Custom Description";

            /// <summary> Suffix for Custom Screen Name </summary>
            public const string CustomNameSuffix = " Custom";

            /// <summary> Attribute for ID </summary>
            public const string AttributeId = "ID";

            /// <summary> Attribute for Type </summary>
            public const string AttributeType = "Type";

            /// <summary> Attribute for Label </summary>
            public const string AttributeLabel = "Label";

            /// <summary> Attribute for Binding </summary>
            public const string AttributeBinding = "Binding";

            /// <summary> Attribute for BeforeID </summary>
            public const string AttributeBeforeId = "BeforeID";

            /// <summary> Attribute for AfterID </summary>
            public const string AttributeAfterId = "AfterID";

            /// <summary> Attribute for HeaderBeforeID </summary>
            public const string AttributeHeaderBeforeId = "HeaderBeforeID";

            /// <summary> Attribute for HeaderAfterID </summary>
            public const string AttributeHeaderAfterId = "HeaderAfterID";

            /// <summary> Attribute for DetailBeforeID </summary>
            public const string AttributeDetailBeforeId = "DetailBeforeID";

            /// <summary> Attribute for DetailAfterID </summary>
            public const string AttributeDetailAfterId = "DetailAfterID";

            /// <summary> Attribute for MaxLength </summary>
            public const string AttributeMaxLength = "MaxLength";

            /// <summary> Attribute for Cols </summary>
            public const string AttributeCols = "Cols";

            /// <summary> Attribute for Rows </summary>
            public const string AttributeRows = "Rows";

            /// <summary> Attribute for Name </summary>
            public const string AttributeName = "Name";

            /// <summary> Attribute for FinderTextId </summary>
            public const string AttributeFinderTextId = "FinderTextId";

            /// <summary> Attribute for xsi </summary>
            public const string AttributeXsi = "xsi";

            /// <summary> Attribute for xmlns:xsi </summary>
            public const string AttributeXmlnsXsi = "xmlns:xsi";

            /// <summary> Attribute for xmlns:xsi </summary>
            public const string AttributeNoNamespaceSchemaLocation = "noNamespaceSchemaLocation";

            /// <summary> Attribute for xmlns:xsi </summary>
            public const string AttributeXsiNoNamespaceSchemaLocation = "xsi:noNamespaceSchemaLocation";

            /// <summary> Element Name for Screens </summary>
            public const string ElementScreens = "Screens";

            /// <summary> Element Name for Screen </summary>
            public const string ElementScreen = "Screen";

            /// <summary> Element Name for Control </summary>
            public const string ElementControl = "Control";

            /// <summary> Token for Company Name </summary>
            public const string CompanyNameToken = "$companyname$";

            /// <summary> Token for Screen Name </summary>
            public const string ScreenNameToken = "$screenName$";

            /// <summary> Token for Customization Name </summary>
            public const string CustomizationNameToken = "$customizationName$";

            /// <summary> Token for Generated Message </summary>
            public const string GeneratedMessageToken = "$generatedMessage$";

            /// <summary> Token for Generated Warning </summary>
            public const string GeneratedWarningToken = "$generatedWarning$";
        }
        #endregion

        #region Public Delegates

        /// <summary> Delegate to update UI with name of file being processed </summary>
        /// <param name="text">Text for UI</param>
        public delegate void ProcessingEventHandler(string text);

        /// <summary> Delegate to update UI with status of file being processed </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="statusType">Status Type</param>
        /// <param name="text">Text for UI</param>
        public delegate void StatusEventHandler(string fileName, Info.StatusType statusType, string text);

        #endregion

        #region Public Events

        /// <summary> Event to update UI with name of file being processed </summary>
        public event ProcessingEventHandler ProcessingEvent;

        /// <summary> Event to update UI with status of file being processed </summary>
        public event StatusEventHandler StatusEvent;

        #endregion

        #region Public Methods

        /// <summary> Start the generation process </summary>
        /// <param name="settings">Settings for processing</param>
        public void Process(Settings settings)
        {
            // Begin process (validation already performed on every step)
            _settings = settings;

            // Create Manifest.json
            CreateManifestFile();

            // Create {screen}_Settings.xml file(s), if screen has controls
            CreateSettingsFiles();

            // Create screenConfig.xsd, if screen has controls
            CreateScreenConfig();

            // Create {screen}_Customization.js file(s)
            CreateJavaScriptFiles();

            // Copy EULA file, if specified
            CopyEulaFile();
        }

        #endregion

        #region Private methods

        /// <summary> Create the manifest file </summary>
        /// <remarks>File is Manifest.json</remarks>
        private void CreateManifestFile()
        {
            // Locals
            var fileName = BuildFileName(Constants.JsonFileName);

            try
            {
                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Delete if file exists
                DeleteFile(fileName);

                // Save the file
                File.WriteAllText(fileName, _settings.Manifest.ToString());

                // Success. Update status
                LaunchStatusEvent(true, fileName);
            }
            catch (Exception)
            {
                // Failure. Update status
                LaunchStatusEvent(false, fileName);
            }
        }

        /// <summary> Create the settings file(s) </summary>
        /// <remarks>File(s) are {screen}_Settings.xml</remarks>
        private void CreateSettingsFiles()
        {
            // Read JSON to get the Screen Controls
            var webScreens =
                from json in _settings.Manifest[Constants.PropertyWebScreens]
                select new
                {
                    TargetScreen = (string)json[Constants.PropertyTargetScreen],
                    ControlsConfiguration = (string)json[Constants.PropertyControlsConfiguration]
                };

            // Iterate and get screens that will create an XML file
            foreach (var webScreen in webScreens)
            {
                // If property has not been specified
                if (string.IsNullOrEmpty(webScreen.ControlsConfiguration))
                {
                    continue;
                }

                // Locals
                var targetScreen = webScreen.TargetScreen;
                var fileName = BuildFileName(webScreen.ControlsConfiguration);

                try
                {
                    // Update display of file being processed
                    LaunchProcessingEvent(fileName);

                    // Delete if file exists
                    DeleteFile(fileName);

                    // Get Screen Node from XDocument
                    var elements = _settings.XmlSettings.Descendants(Constants.ElementScreen);
                    foreach (var element in elements)
                    {
                        // Screen node not located
                        if (!element.Name.LocalName.Equals(Constants.ElementScreen))
                        {
                            continue;
                        }

                        // Screen node is not for this screen
                        var nameAttribute = element.Attributes(Constants.AttributeName).FirstOrDefault();
                        if (nameAttribute != null)
                        {
                            if (!nameAttribute.Value.Equals(targetScreen))
                            {
                                continue;
                            }
                        }

                        // Get template resource file and update name to targetName
                        var xml = Resources.ScreenSettings.Replace(Constants.GeneratedMessageToken, 
                            Resources.GeneratedMessage).Replace(Constants.GeneratedWarningToken, Resources.GeneratedWarning);

                        var templateSettingsFile = XDocument.Parse(xml);
                        var screenElement = templateSettingsFile.Descendants(Constants.ElementScreen).First();
                        nameAttribute = screenElement.Attributes(Constants.AttributeName).FirstOrDefault();
                        if (nameAttribute != null)
                        {
                            nameAttribute.Value = targetScreen;
                        }

                        // Add controls to screen
                        screenElement.Add(element.Nodes());

                        // Save the file
                        templateSettingsFile.Save(fileName);

                        // Success. Update status
                        LaunchStatusEvent(true, fileName);

                        // Exit loop as xml file has been created
                        break;
                    }
                }
                catch (Exception)
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName);
                }

            }
        }

        /// <summary> Create the screenConfig.xsd file </summary>
        /// <remarks>File is screenConfig.xsd</remarks>
        private void CreateScreenConfig()
        {
            // Read JSON to get the Screen Controls
            var webScreens =
                from json in _settings.Manifest[Constants.PropertyWebScreens]
                select new
                {
                    ControlsConfiguration = (string)json[Constants.PropertyControlsConfiguration]
                };

            // Iterate and get screens that will create an XML file
            foreach (var webScreen in webScreens)
            {
                // If property has not been specified
                if (string.IsNullOrEmpty(webScreen.ControlsConfiguration))
                {
                    continue;
                }

                // Locals
                var fileName = BuildFileName(Constants.XsdFileName);

                try
                {
                    // Update display of file being processed
                    LaunchProcessingEvent(fileName);

                    // Delete if file exists
                    DeleteFile(fileName);

                    // Get template xsd file
                    var screenConfig = Resources.screenConfig;

                    // Save the file
                    File.WriteAllText(fileName, screenConfig);

                    // Success. Update status
                    LaunchStatusEvent(true, fileName);

                    // Exit loop as xsd file has been created and only 1 is required 
                    // regardless of how many screens may exist
                    break;
                }
                catch (Exception)
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName);
                }
            }
        }

        /// <summary> Create the JavaScript file(s) </summary>
        /// <remarks>File(s) are {screen}_Customization.js</remarks>
        private void CreateJavaScriptFiles()
        {
            var companyName = ((string)_settings.Manifest.SelectToken(Constants.PropertyBusinessPartnerName)).Trim().Replace(" ", "").Replace(".", "");
            var customizationName = ((string)_settings.Manifest.SelectToken(Constants.PropertyName)).Trim().Replace(" ", "").Replace(".", "");

            // Read JSON to get the Screen Controls
            var webScreens =
                from json in _settings.Manifest[Constants.PropertyWebScreens]
                select new
                {
                    TargetScreen = (string)json[Constants.PropertyTargetScreen],
                    ControlsBehavior = (string)json[Constants.PropertyControlsBehavior]
                };

            // Iterate and get screens that will create an XML file
            foreach (var webScreen in webScreens)
            {
                // Locals
                var targetScreen = webScreen.TargetScreen;
                var fileName = BuildFileName(webScreen.ControlsBehavior);

                try
                {
                    // Update display of file being processed
                    LaunchProcessingEvent(fileName);

                    // Delete if file exists
                    DeleteFile(fileName);

                    // Get template xsd file
                    var templateJavaScript = Resources.ScreenCustomization.Replace(Constants.GeneratedMessageToken,
                            Resources.GeneratedMessage).Replace(Constants.GeneratedWarningToken, Resources.GeneratedWarning);

                    // Replace tokens
                    templateJavaScript = templateJavaScript
                        .Replace(Constants.CompanyNameToken, companyName)
                        .Replace(Constants.ScreenNameToken, targetScreen)
                        .Replace(Constants.CustomizationNameToken, customizationName);

                    // Save the file
                    File.WriteAllText(fileName, templateJavaScript);

                    // Success. Update status
                    LaunchStatusEvent(true, fileName);

                }
                catch (Exception)
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName);
                }
            }
        }

        /// <summary> Copy the EULA file, if specified </summary>
        private void CopyEulaFile()
        {
            var eulaFile = (string)_settings.Manifest.SelectToken(Constants.PropertyEula);

            // Return if Eula does not exist
            if (string.IsNullOrEmpty(eulaFile))
            {
                return;
            }

            var fileName = BuildFileName(eulaFile);
            var eulaFileName = Path.Combine(_settings.EulaFolder, eulaFile);

            try
            {
                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Copy if source of eula is not source for customization (existing customization)
                if (!_settings.FolderName.Equals(_settings.EulaFolder))
                {
                    // Delete if file exists
                    DeleteFile(fileName);

                    // Copy the file
                    File.Copy(eulaFileName, fileName);
                }

                // Success. Update status
                LaunchStatusEvent(true, fileName);

            }
            catch (Exception)
            {
                // Failure. Update status
                LaunchStatusEvent(false, fileName);
            }
        }

        /// <summary> Build File Name </summary>
        /// <returns>Full path file name</returns>
        private string BuildFileName(string fileName)
        {
            return Path.Combine(_settings.FolderName, fileName);
        }

        /// <summary> Update UI </summary>
        /// <param name="success">True/False based upon creation</param>
        /// <param name="fileName">Name of file to be created</param>
        private void LaunchStatusEvent(bool success, string fileName)
        {
            // Return if no subscriber
            if (StatusEvent == null)
            {
                return;
            }

            // Update according to success or failure
            if (success)
            {
                StatusEvent(fileName, Info.StatusType.Success, string.Empty);
            }
            else
            {
                StatusEvent(fileName, Info.StatusType.Error, string.Format(Resources.ErrorCreatingFile, fileName));
            }
        }

        /// <summary> Update UI </summary>
        /// <param name="fileName">Name of file to be created</param>
        private void LaunchProcessingEvent(string fileName)
        {
            // Event if subscriber
            if (ProcessingEvent == null)
            {
                return;
            }

            ProcessingEvent(fileName);
        }

        /// <summary> Delete file if exists </summary>
        /// <param name="fileName">Name of file to be deleted</param>
        private static void DeleteFile(string fileName)
        {
            // Delete if file exists
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        #endregion

    }
}
