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
using ACCPAC.Advantage;
using Newtonsoft.Json.Linq;

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

        /// <summary> Property for Captions </summary>
        public const string PropertyCaptions = "Captions";
        /// <summary> Property for List </summary>
        public const string PropertyFilters = "List";
        /// <summary> Property for Parameters </summary>
        public const string PropertyParameters = "Parameters";
        /// <summary> Property for Selected </summary>
        public const string PropertySelected = "Selected";
        /// <summary> Property for Language </summary>
        public const string PropertyLanguage = "Language";
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
        /// <summary> Property for ConfigurationFileNameSuffix </summary>
        public const string PropertyConfigurationFileNameSuffix = "InquiryConfiguration.json";
        /// <summary> Property for TemplateFileNameSuffix </summary>
        public const string PropertyTemplateFileNameSuffix = "InquiryTemplate.json";
        /// <summary> Property for Fields </summary>
        public const string PropertyFields = "Fields";
        /// <summary> Property for DisplayFields </summary>
        public const string PropertyDisplayFields = "DisplayFields";
        /// <summary> Property for Name </summary>
        public const string PropertyName = "Name";
        /// <summary> Property for FileName </summary>
        public const string PropertyFileName = "FileName";
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
        /// <summary> Property for isFilterable </summary>
        public const string PropertyIsFilterable = "IsFilterable";
        /// <summary> Property for IsDrilldown </summary>
        public const string PropertyIsDrilldown = "IsDrilldown";
        /// <summary> Property for IsDisplayable </summary>
        public const string PropertyIsDisplayable = "IsDisplayable";
        /// <summary> Property for Field </summary>
        public const string PropertyField = "Field";
        /// <summary> Property for DisplayField </summary>
        public const string PropertyDisplayField = "DisplayField";
        /// <summary> Property for FieldIndex </summary>
        public const string PropertyFieldIndex = "FieldIndex";
        /// <summary> Property for DrilldownUrl </summary>
        public const string PropertyDrilldownUrl = "DrilldownUrl";
        /// <summary> Property for DataType </summary>
        public const string PropertyDataType = "DataType";
        /// <summary> Property for DataTypeEnumeration </summary>
        public const string PropertyDataTypeEnumeration = "DataTypeEnumeration";
        /// <summary> Property for Sql </summary>
        public const string PropertySql = "Sql";
        /// <summary> Property for WhereClause </summary>
        public const string PropertyWhereClause = "WhereClause";
        /// <summary> Property for OrderByClause </summary>
        public const string PropertyOrderByClause = "OrderByClause";
        /// <summary> Property for IsColumnInView </summary>
        public const string PropertyIsColumnInView = "IsSQLColumnInView";
        /// <summary> Property for ColumnNameInView </summary>
        public const string PropertyColumnNameInView = "ColumnNameInView";
        /// <summary> Property for AppId </summary>
        public const string PropertyAppId = "WX";
        /// <summary> Property for ProgramName </summary>
        public const string PropertyProgramName = "WX1000";
        /// <summary> Property for Reserved </summary>
        public const string PropertyReserved = "RESERVED";
        /// <summary> Property for NotApplicable </summary>
        public const string PropertyNotApplilcable = @"N/A";
        /// <summary> Property for None </summary>
        public const string PropertyNone = "None";
        /// <summary> Property for English </summary>
        public const string PropertyEnglish = "ENG";
        /// <summary> Property for French </summary>
        public const string PropertyFrench = "FRA";
        /// <summary> Property for Spanish </summary>
        public const string PropertySpanish= "ESN";
        /// <summary> Property for ChineseSimplified </summary>
        public const string PropertyChineseSimplified = "CHN";
        /// <summary> Property for ChineseTraditional </summary>
        public const string PropertyChineseTraditional = "CHT";
        /// <summary> Property for Configuration </summary>
        public const string PropertyConfiguration = "Configuration";
        /// <summary> Property for Template </summary>
        public const string PropertyTemplate = "Template";
        /// <summary> Property for Where </summary>
        public const string PropertyWhere = "WHERE ";
        /// <summary> Property for OrderBy </summary>
        public const string PropertyOrderBy = "ORDER BY ";
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

            // Create json files
            CreateJsonFile(_settings.ConfigurationFileName, _settings.ConfigurationJson);
            CreateJsonFile(_settings.TemplateFileName, _settings.TemplateJson);
        }

        /// <summary> Get source </summary>
        /// <param name="source">Source structure for Business View</param>
        public static void GetSource(Source source)
        {
            // Locals
            var session = new Session();

            // Init session
            session.CreateSession(null, PropertyAppId, PropertyProgramName, source.Properties[Source.Version],
                source.Properties[Source.User], source.Properties[Source.Password], 
                source.Properties[Source.Company], DateTime.UtcNow);

            // Attempt to open a view
            var dbLink = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
            var view = dbLink.OpenView(source.Properties[Source.ViewId]);

            // Clear out source columns
            source.SourceColumns.Clear();

            source.Properties[Source.ViewDescription] = view.Description;

            GenerateFieldsAndEnums(source, view);

            // Clean up
            try
            {
                view.Dispose();
                dbLink.Dispose();
                session.Dispose();
            }
            catch
            {
                // Swallow error, if any        
            }

        }

        /// <summary> Generate enums </summary>
        /// <param name="source">Source for Business View</param>
        /// <param name="view">Accpadc Business View</param>
        private static void GenerateFieldsAndEnums(Source source, View view)
        {
            // Iterate Accpac View
            for (var i = 0; i < view.Fields.Count; i++)
            {
                // Ignore those fields having description "RESERVED"
                if (view.Fields[i].Description.ToUpper() == PropertyReserved)
                {
                    continue;
                }

                var field = view.Fields[i];
                var sourceColumn = new SourceColumn
                {
                    Id = field.ID,
                    Name = field.Name,
                    Description = field.Description,
                    Type = FieldType(field),
                    ViewId = source.Properties[Source.ViewId]
                };

                if (field.PresentationType == ViewFieldPresentationType.List)
                {
                    for (var j = 0; j < field.PresentationList.Count; j++)
                    {
                        if (field.PresentationList.PredefinedString(j) != PropertyNotApplilcable || 
                            !string.IsNullOrEmpty(field.PresentationList.PredefinedString(j)))
                        {
                            var desc = field.PresentationList.PredefinedString(j);
                            var key = SourceHelper.Replace(desc);
                            var value = GetValue(i, j, view);

                            // If the value coming from the presentation list is blank, assign it to None
                            if (string.IsNullOrEmpty(key))
                            {
                                key = PropertyNone;
                                desc = PropertyNone;
                            }

                            sourceColumn.Filters.Add(value.ToString(), new Filter()
                            {
                                Text = key,
                                Value = value.ToString()
                            });
                        }
                    }

                }

                // Add to collection
                source.SourceColumns.Add(sourceColumn.Name, sourceColumn);
            }
        }

        /// <summary>
        /// Get value from presentation list
        /// </summary>
        /// <param name="i">Outer Loop</param>
        /// <param name="j">Inner Loop</param>
        /// <param name="view">Accpac Business View</param>
        private static Object GetValue(int i, int j, View view)
        {
            int intVal;
            bool boolVal;
            if (Int32.TryParse(view.Fields[i].PresentationList.PredefinedValue(j).ToString(), out intVal))
            {
                return intVal;
            }
            if (bool.TryParse(view.Fields[i].PresentationList.PredefinedValue(j).ToString(), out boolVal))
            {
                return boolVal ? 1 : 0;
            }

            return Convert.ToChar(view.Fields[i].PresentationList.PredefinedValue(j));
        }

        /// <summary>
        /// Get field's data type
        /// </summary>
        /// <param name="field">The field the data type is to be determined</param>
        /// <returns>Data type of the field</returns>
        private static SourceDataType FieldType(ViewField field)
        {
            if (field.PresentationType == ViewFieldPresentationType.List)
            {
                return SourceDataType.Enumeration;
            }

            // Need to use enum field.Type.HasFlag
            var viewfiledType = field.Type;

            if (viewfiledType.GetHashCode() == ViewFieldType.Long.GetHashCode())
            {
                return SourceDataType.Long;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Char.GetHashCode())
            {
                return SourceDataType.String;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Date.GetHashCode())
            {
                return SourceDataType.DateTime;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Int.GetHashCode())
            {
                return SourceDataType.Integer;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Decimal.GetHashCode())
            {
                return SourceDataType.Decimal;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Bool.GetHashCode())
            {
                return SourceDataType.Boolean;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Time.GetHashCode())
            {
                return SourceDataType.TimeSpan;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Byte.GetHashCode())
            {
                return SourceDataType.Byte;
            }

            return SourceDataType.Double;
        }

        #endregion

        #region Private methods

        /// <summary> Create the json file </summary>
        /// <param name="jsonFileName">Name of file to be created</param>
        /// <param name="json">JSON content</param>
        private void CreateJsonFile(string jsonFileName, JObject json)
        {
            // Locals
            
            var fileName = BuildFileName(jsonFileName);

            try
            {
                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Delete if file exists
                DeleteFile(fileName);

                // Save the file
                File.WriteAllText(fileName, json.ToString());

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
        /// <param name="fileName">Name of file to be created</param>
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
