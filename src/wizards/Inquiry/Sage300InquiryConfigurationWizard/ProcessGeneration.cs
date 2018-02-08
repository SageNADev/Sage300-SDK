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

using System;
using System.IO;
using Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard.Properties;

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary> Process Generation Class (worker) </summary>
    internal class ProcessGeneration
    {

        #region Private Vars

        /// <summary> Settings from UI </summary>
        private Settings _settings;

        #endregion

        #region Public constants

        /// <summary> Property for Enums </summary>
        public const string PropertyEnums = "Enums";
        /// <summary> Property for Selected </summary>
        public const string PropertySelected = "Selected";
        /// <summary> Property for Text </summary>
        public const string PropertyText = "Text";
        /// <summary> Property for Value </summary>
        public const string PropertyValue = "Value";
        /// <summary> Property for Area </summary>
        public const string PropertyArea = "Area";
        /// <summary> Property for Controller </summary>
        public const string PropertyController = "Controller";
        /// <summary> Property for Action </summary>
        public const string PropertyAction = "Action";
        /// <summary> Property for ModelsSearchPattern </summary>
        public const string PropertyModelsSearchPattern = "*.Models.dll";
        /// <summary> Property for ExcludeCommon </summary>
        public const string PropertyExcludeCommon = ".Common.Models.dll";
        /// <summary> Property for ExcludeKpi </summary>
        public const string PropertyExcludeKpi = ".KPI.Models.dll";
        /// <summary> Property for ExcludeWorkflow </summary>
        public const string PropertyExcludeWorkflow = ".Workflow.Models.dll";
        /// <summary> Property for ExcludeVpf </summary>
        public const string PropertyExcludeVpf = ".VPF.Models.dll";
        /// <summary> Property for AssemblyExtension </summary>
        public const string PropertyAssemblyExtension = ".dll";
        /// <summary> Property for FileNameSuffix </summary>
        public const string PropertyFileNameSuffix = "InquiryConfiguration.json";
        /// <summary> Property for ModelBase </summary>
        public const string PropertyModelBase = "ModelBase";
        /// <summary> Property for ModelsProcess </summary>
        public const string PropertyModelsProcess = ".Models.Process.";
        /// <summary> Property for Index </summary>
        public const string PropertyIndex = "Index";
        /// <summary> Property for Fields </summary>
        public const string PropertyFields = "Fields";
        /// <summary> Property for EntityName </summary>
        public const string PropertyEntityName = "EntityName";
        /// <summary> Property for Name </summary>
        public const string PropertyName = "Name";
        /// <summary> Property for ModelName </summary>
        public const string PropertyModelName = "ModelName";
        /// <summary> Property for Description </summary>
        public const string PropertyDescription = "Description";
        /// <summary> Property for GeneratedMessage </summary>
        public const string PropertyGeneratedMessage = "GeneratedMessage";
        /// <summary> Property for GeneratedWarning </summary>
        public const string PropertyGeneratedWarning = "GeneratedWarning";
        /// <summary> Property for InquiryId </summary>
        public const string PropertyInquiryId = "InquiryId";
        /// <summary> Property for ViewName </summary>
        public const string PropertyViewName = "ViewName";
        /// <summary> Property for Assembly </summary>
        public const string PropertyAssembly = "Assembly";
        /// <summary> Property for isFilterable </summary>
        public const string PropertyIsFilterable = "IsFilterable";
        /// <summary> Property for IsDrilldown </summary>
        public const string PropertyIsDrilldown = "IsDrilldown";
        /// <summary> Property for Field </summary>
        public const string PropertyField = "Field";
        /// <summary> Property for FieldIndex </summary>
        public const string PropertyFieldIndex = "FieldIndex";
        /// <summary> Property for DrilldownUrl </summary>
        public const string PropertyDrilldownUrl = "DrilldownUrl";
        /// <summary> Property for DataType </summary>
        public const string PropertyDataType = "DataType";
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

            // Create json
            CreateJsonFile();
        }

        #endregion

        #region Private methods

        /// <summary> Create the json file </summary>
        /// <remarks>File is {0}InquiryConfiguration.json</remarks>
        private void CreateJsonFile()
        {
            // Locals
            
            // Need to add model name to settings!
            var fileName = BuildFileName();

            try
            {
                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Delete if file exists
                DeleteFile(fileName);

                // Save the file
                File.WriteAllText(fileName, _settings.Json.ToString());

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
        private string BuildFileName()
        {
            return Path.Combine(_settings.FolderName, _settings.FileName);
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
