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

using ACCPAC.Advantage;
using Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using View = ACCPAC.Advantage.View;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Process Generation Class (worker) </summary>
    class ProcessGeneration
    {
        #region Private Vars
        /// <summary> Settings from UI (view id, output file) </summary>
        private Settings _settings;

        /// <summary> View to be used to generated class files </summary>
        private static View _view;

        private static Session _session;
        private static DBLink _dbLink;

        private static readonly Dictionary<string, bool> UniqueDescriptions = new Dictionary<string, bool>();

        #endregion

        #region Private Constants
        #endregion

        #region Public Constants
        /// <summary> SettingsKey is used as a dictionary key for settings </summary>
        public const string SettingsKey = "settings";

        /// <summary> BusinessRepositoryKey is used as a dictionary key for projects </summary>
        public const string BusinessRepositoryKey = "BusinessRepository";

        /// <summary> InterfacesKey is used as a dictionary key for projects </summary>
        public const string InterfacesKey = "Interfaces";

        /// <summary> ModelsKey is used as a dictionary key for projects </summary>
        public const string ModelsKey = "Models";

        /// <summary> ResourcesKey is used as a dictionary key for projects </summary>
        public const string ResourcesKey = "Resources";

        /// <summary> ServicesKey is used as a dictionary key for projects </summary>
        public const string ServicesKey = "Services";

        /// <summary> WebKey is used as a dictionary key for projects </summary>
        public const string WebKey = "Web";

        /// <summary> SubFolderModelKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderModelKey = "Model";

        /// <summary> SubFolderModelFieldsKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderModelFieldsKey = "ModelFields";

        /// <summary> SubFolderModelEnumsKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderModelEnumsKey = "ModelEnums";

        /// <summary> SubFolderBusinessRepositoryKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderBusinessRepositoryKey = "BusinessRepository";

        /// <summary> SubFolderBusinessRepositoryMappersKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderBusinessRepositoryMappersKey = "Mappers";

        /// <summary> SubFolderBusinessRepositoryMappersKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderInterfacesBusinessRepositoryKey = "BusinessRepository";

        /// <summary> SubFolderInterfacesServicesKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderInterfacesServicesKey = "Services";

        /// <summary> SubFolderServicesKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderServicesKey = "Services";

        /// <summary> SubFolderResourcesKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderResourcesKey = "Resources";

        /// <summary> SubFolderWebIndexKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebIndexKey = "Index";

        /// <summary> SubFolderWebLocalizationKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebLocalizationKey = "Localization";

        /// <summary> SubFolderWebViewModelKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebViewModelKey = "ViewModel";

        /// <summary> SubFolderWebControllersKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebControllersKey = "Controllers";

        /// <summary> SubFolderWebFinderKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebFinderKey = "Finder";

        /// <summary> SubFolderWebScriptsKey is used as a dictionary key for subfolders </summary>
        public const string SubFolderWebScriptsKey = "Scripts";

        /// <summary> SubFolderNameFields is used as a subfolder name </summary>
        public const string SubFolderNameFields = "Fields";

        /// <summary> SubFolderNameEnums is used as a subfolder name </summary>
        public const string SubFolderNameEnums = "Enums";

        /// <summary> SubFolderNameMappers is used as a subfolder name </summary>
        public const string SubFolderNameMappers = "Mappers";

        /// <summary> SubFolderNameBusinessRepository is used as a subfolder name </summary>
        public const string SubFolderNameBusinessRepository = "BusinessRepository";

        /// <summary> SubFolderNameServices is used as a subfolder name </summary>
        public const string SubFolderNameServices = "Services";

        /// <summary> SubFolderNameForms is used as a subfolder name </summary>
        public const string SubFolderNameForms = "Forms";

        /// <summary> SubFolderNameProcess is used as a subfolder name </summary>
        public const string SubFolderNameProcess = "Process";

        /// <summary> SubFolderNameReports is used as a subfolder name </summary>
        public const string SubFolderNameReports = "Reports";

        /// <summary> SubFolderNameAreas is used as a subfolder name </summary>
        public const string SubFolderNameAreas = "Areas";

        /// <summary> SubFolderNameViews is used as a subfolder name </summary>
        public const string SubFolderNameViews = "Views";

        /// <summary> SubFolderNamePartials is used as a subfolder name </summary>
        public const string SubFolderNamePartials = "Partials";

        /// <summary> SubFolderNameModels is used as a subfolder name </summary>
        public const string SubFolderNameModels = "Models";

        /// <summary> SubFolderNameControllers is used as a subfolder name </summary>
        public const string SubFolderNameControllers = "Controllers";

        /// <summary> SubFolderNameFinder is used as a subfolder name </summary>
        public const string SubFolderNameFinder = "Finder";

        /// <summary> SubFolderNameScripts is used as a subfolder name </summary>
        public const string SubFolderNameScripts = "Scripts";

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

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary> Cleanup </summary>
        public void Dispose()
        {
            if (_view != null)
            {
                _view.Dispose();
                _view = null;
            }

            if (_dbLink != null)
            {
                _dbLink.Dispose();
                _dbLink = null;
            }

            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }

        }
        /// <summary> Validate the settings based upon repository type</summary>
        /// <param name="settings">Settings</param>
        /// <param name="view">Business View</param>
        /// <returns>Empty if valid otherwise message</returns>
        public string ValidSettings(Settings settings, ref BusinessView view)
        {
            string validSetting;
            var repositoryType = settings.RepositoryType;

            // Check Resx Name regardless of repository type
            if (settings.ResxName.Equals(string.Empty))
            {
                return Resources.InvalidSettingRequiredResource;
            }

            // Dynamic Query validation
            if (repositoryType.Equals(RepositoryType.DynamicQuery))
            {
                // Check ViewId, Module, Model Name and Entity Name
                if (settings.BusinessView.Properties[BusinessView.ViewId].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ModuleId].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ModelName].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.EntityName].Equals(string.Empty))
                {
                    return Resources.InvalidSettingRequired;
                }

                // Check Fields. There must be at least one field entered
                if (settings.BusinessView.Fields.Count == 0)
                {
                    return Resources.InvalidSettingDynamicQueryCount;
                }

                // Check Fields. For content
                var validFields = true;
                UniqueDescriptions.Clear();
                for (var i = 0; i < settings.BusinessView.Fields.Count; i++)
                {
                    // Locals
                    var field = settings.BusinessView.Fields[i];

                    // Assign id
                    field.Id = i + 1;

                    // Check Name
                    if (field.Name.Trim().Equals(string.Empty))
                    {
                        validFields = false;
                        break;
                    }

                    // Ensure Name is properly formatted
                    field.Name = BusinessViewHelper.Replace(field.Name);
                    field.ServerFieldName = field.Name;

                    // Ensure name is unique
                    if (UniqueDescriptions.ContainsKey(field.Name))
                    {
                        // Duplicate name entered
                        validFields = false;
                        break;
                    }

                    // Add for next check
                    UniqueDescriptions.Add(field.Name, false);
                }

                validSetting = (validFields ? string.Empty : Resources.InvalidSettingDynamicQueryFields);
            }

            // Report Validation
            else if (repositoryType.Equals(RepositoryType.Report))
            {
                // Check ViewId, Module Id, Model Name, Entity Name, ReportKey and ProgramId
                if (settings.BusinessView.Properties[BusinessView.ViewId].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ModuleId].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ModelName].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.EntityName].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ReportKey].Equals(string.Empty) ||
                    settings.BusinessView.Properties[BusinessView.ProgramId].Equals(string.Empty))
                {
                    return Resources.InvalidSettingRequired;
                }

                // Check Fields. There must be at least one field entered
                if (settings.BusinessView.Fields.Count == 0)
                {
                    return Resources.InvalidSettingReportCount;
                }

                // Check Fields. For content
                var validFields = true;
                UniqueDescriptions.Clear();
                foreach (var field in settings.BusinessView.Fields)
                {
                    // Check Name
                    if (field.Name.Trim().Equals(string.Empty))
                    {
                        validFields = false;
                        break;
                    }

                    // Ensure Name is properly formatted
                    field.Name = BusinessViewHelper.Replace(field.Name);

                    // Ensure name is unique
                    if (UniqueDescriptions.ContainsKey(field.Name))
                    {
                        // Duplicate name entered
                        validFields = false;
                        break;
                    }

                    // Add for next check
                    UniqueDescriptions.Add(field.Name, false);
                }

                validSetting = (validFields ? string.Empty : Resources.InvalidSettingReportFields);
            }
            else
            {
                // Check requirements for validating a view
                if (settings.ViewId.Equals(string.Empty) || settings.Company.Equals(string.Empty) || settings.Password.Equals(string.Empty) ||
                    settings.User.Equals(string.Empty) || settings.Version.Equals(string.Empty) || settings.ModuleId.Equals(string.Empty))
                {
                    return Resources.InvalidSettingRequired;
                }

                try
                {
                    // Init
                    if (_session == null)
                    {
                        _session = new Session();
                        _session.InitEx(null, string.Empty, "WX", "WX1000", settings.Version);
                        _session.Open(settings.User, settings.Password, settings.Company, DateTime.UtcNow, 0);
                    }

                }
                catch
                {
                    _session = null;
                }

                if (_session != null)
                {
                    try
                    {
                        // Clean up first
                        if (_view != null)
                        {
                            _view.Dispose();
                            _view = null;
                        }

                        // Attempt to open a view
                        _dbLink = _session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
                        _view = _dbLink.OpenView(settings.ViewId);
                    }
                    catch
                    {
                        _view = null;
                    }
                }

                validSetting = (_view != null ? string.Empty : Resources.InvalidSettingCredentials);
            }

            // Return message if failures thus far
            if (!string.IsNullOrEmpty(validSetting))
            {
                return validSetting;
            }

            // Init view and validate model
            Initialize(settings, ref view);
            if (!ValidModel(view))
            {
                validSetting = Resources.InvalidSettingModel;
            }

            return validSetting;
        }

        /// <summary> Start the generation process </summary>
        /// <param name="settings">Settings for processing</param>
        public void Process(Settings settings)
        {
            try
            {
                _settings = settings;

                // Iterate View
                IterateView();
            }
            catch
            {
                // Catch here does nothing but return to UI
                // User may have aborted wizard
            }
        }

        /// <summary> Get default view name </summary>
        /// <param name="user">User Name</param>
        /// <param name="password">User Password</param>
        /// <param name="company">Company</param>
        /// <param name="version">Version</param>
        /// <param name="viewId">View Id</param>
        /// <returns>Default view name</returns>
        public static string GetDefaultName(string user, string password, string company, string version, string viewId)
        {
            // Locals
            var session = new Session();
            var retVal = string.Empty;

            try
            {
                session.InitEx(null, string.Empty, "WX", "WX1000", version);
                session.Open(user, password, company, DateTime.UtcNow, 0);

                // Attempt to open a view
                var dbLink = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
                var view = dbLink.OpenView(viewId);

                retVal = MakeItSingular(BusinessViewHelper.Replace(view.Description));

                // Clean up
                view.Dispose();
                session.Dispose();
            }
            catch (Exception)
            {
                // Catch only
            }

            return retVal;
        }

        #endregion

        #region Private Methods

        /// <summary> Save a file </summary>
        /// <param name="view">Business View</param>
        /// <param name="fileName"> Name of file to be created</param>
        /// <param name="content">File contents</param>
        /// <param name="projectKey">Project Key for Project Info</param>
        /// <param name="subfolderKey">Subfolder Key for Project Info</param>
        /// <returns>True if successful otherwise false</returns>
        private bool SaveFile(BusinessView view, string fileName, string content, string projectKey, string subfolderKey)
        {
            // Local
            var retVal = true;
            var projectInfo = _settings.Projects[projectKey][view.Properties[BusinessView.ModuleId]];
            var filePath = BusinessViewHelper.ConcatStrings(new[] { projectInfo.ProjectFolder, projectInfo.Subfolders[subfolderKey] });
            var fullFileName = BusinessViewHelper.ConcatStrings(new[] { filePath, fileName });

            // Determine if exists
            var exists = File.Exists(fullFileName);
            var writeFile = true;

            // Prompt if file exists?
            if (exists && _settings.PromptIfExists)
            {
                var result = MessageBox.Show(string.Format(Resources.FileExists, fullFileName),
                    Resources.Confirmation, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // Evaluate
                switch (result)
                {
                    case DialogResult.Yes:
                        // Overwrite the file
                        break;
                    case DialogResult.No:
                        // Skip the file
                        writeFile = false;
                        break;
                    case DialogResult.Cancel:
                        // Abort the wizard
                        throw new Exception();
                }
            }

            // Write the file?
            if (writeFile)
            {
                try
                {
                    // Ensure the filepath exists
                    if (!exists)
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                    }

                    // Save the file
                    File.WriteAllText(fullFileName, content);
                }
                catch
                {
                    retVal = false;
                }

                // Update project if write was successful
                if (retVal)
                {
                    // Add to project
                    try
                    {
                        projectInfo.Project.ProjectItems.AddFromFile(fullFileName);
                    }
                    catch
                    {
                        retVal = false;
                    }
                }

            }
            else
            {
                retVal = false;
            }

            return retVal;
        }

        /// <summary> Build subfolders for later use by SaveFile routine </summary>
        /// <param name="view">Business View</param>
        private void BuildSubfolders(BusinessView view)
        {
            // Iterate Models
            var projects = _settings.Projects[ModelsKey];
            foreach (var project in projects)
            {
                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderModelKey, GetSubfolderName(string.Empty)},
                    {SubFolderModelFieldsKey, GetSubfolderName(SubFolderNameFields)},
                    {SubFolderModelEnumsKey, GetSubfolderName(SubFolderNameEnums)}
                };
                project.Value.Subfolders = subfolders;
            }

            // Iterate BusinessRepository
            projects = _settings.Projects[BusinessRepositoryKey];
            foreach (var project in projects)
            {
                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderBusinessRepositoryKey, GetSubfolderName(string.Empty)},
                    {SubFolderBusinessRepositoryMappersKey, GetSubfolderName(SubFolderNameMappers)}
                };
                project.Value.Subfolders = subfolders;
            }

            // Iterate Interfaces
            projects = _settings.Projects[InterfacesKey];
            foreach (var project in projects)
            {
                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderInterfacesBusinessRepositoryKey, GetSubfolderName(SubFolderNameBusinessRepository)},
                    {SubFolderInterfacesServicesKey, GetSubfolderName(SubFolderNameServices)}
                };
                project.Value.Subfolders = subfolders;
            }

            // Iterate Services
            projects = _settings.Projects[ServicesKey];
            foreach (var project in projects)
            {
                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderServicesKey, GetSubfolderName(string.Empty)}
                };
                project.Value.Subfolders = subfolders;
            }

            // Iterate Resources
            projects = _settings.Projects[ResourcesKey];

            // Forms, Process and Reports for Resources are at the same level
            var subfolderName = SubFolderNameForms;
            switch (_settings.RepositoryType)
            {
                case RepositoryType.Process:
                    subfolderName = SubFolderNameProcess;
                    break;
                case RepositoryType.Report:
                    subfolderName = SubFolderNameReports;
                    break;
            }

            foreach (var project in projects)
            {
                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderResourcesKey, subfolderName}
                };
                project.Value.Subfolders = subfolders;
            }

            // Iterate Web
            projects = _settings.Projects[WebKey];
            foreach (var project in projects)
            {
                // Need to determine web path first
                var moduleId = _settings.ModuleId;

                // Do web folders in "new" layout exist?
                var path = BusinessViewHelper.ConcatStrings(new[] { project.Value.ProjectFolder, SubFolderNameAreas, moduleId });

                if (Directory.Exists(path))
                {
                    path = BusinessViewHelper.ConcatStrings(new[] { SubFolderNameAreas, moduleId });
                }
                else
                {
                    path = project.Value.ProjectFolder;
                }

                var subfolders = new Dictionary<string, string>
                {
                    {SubFolderWebIndexKey, BusinessViewHelper.ConcatStrings(new []{path, SubFolderNameViews, view.Properties[BusinessView.EntityName]})},
                    {SubFolderWebLocalizationKey, BusinessViewHelper.ConcatStrings(new []{path, BusinessViewHelper.ConcatStrings(new []{SubFolderNameViews, view.Properties[BusinessView.EntityName], SubFolderNamePartials})})},
                    {SubFolderWebViewModelKey, GetSubfolderName(BusinessViewHelper.ConcatStrings(new []{path, SubFolderNameModels}))},
                    {SubFolderWebControllersKey, GetSubfolderName(BusinessViewHelper.ConcatStrings(new []{path, SubFolderNameControllers}))},
                    {SubFolderWebFinderKey, BusinessViewHelper.ConcatStrings(new []{path, SubFolderNameControllers, SubFolderNameFinder})},
                    {SubFolderWebScriptsKey, BusinessViewHelper.ConcatStrings(new []{path, SubFolderNameScripts, view.Properties[BusinessView.EntityName]})}
                };
                project.Value.Subfolders = subfolders;
            }

        }

        /// <summary> Get Sub Folder Name</summary>
        /// <param name="fixedName">Fixed portion of subfolder name, if any</param>
        /// <remarks>Will NOT have beginning or trailing slash</remarks>
        /// <returns>Subfolder for persistence</returns>
        private string GetSubfolderName(string fixedName)
        {
            // Locals
            var retVal = fixedName;

            // Is this a Process Type?
            retVal = retVal + ((_settings.RepositoryType.Equals(RepositoryType.Process)) ? @"\" + SubFolderNameProcess : string.Empty);

            // Is this a Report Type?
            retVal = retVal + ((_settings.RepositoryType.Equals(RepositoryType.Report)) ? @"\" + SubFolderNameReports : string.Empty);

            // Trim any begining slash
            if (retVal.StartsWith(@"\"))
            {
                retVal = retVal.Substring(1);
            }

            return retVal;
        }

        /// <summary>
        /// Make it singular or best guess at it!
        /// </summary>
        /// <param name="name">Name to make singular</param>
        /// <returns>Singular name or entered name if not plural</returns>
        private static string MakeItSingular(string name)
        {
            // Default to entered value
            var retVal = name;

            // If Options or ... then exit
            if (retVal.Equals("Options"))
            {
                return retVal;
            }

            // If it ends in an "s", make it singular
            if (retVal.EndsWith("s"))
            {
                retVal = retVal.Substring(0, retVal.Length - 1);
            }

            return retVal;

        }

        /// <summary>
        /// Get the Field Name. (Note: The Field description attribute is used for name)
        /// </summary>
        /// <param name="field">The field the name has to be determined</param>
        /// <param name="prefix">Prefix for certain potential values</param>
        /// <returns>Name of the field</returns>
        private static string FieldName(ViewField field, string prefix)
        {
            if (UniqueDescriptions != null &&
                UniqueDescriptions.ContainsKey(field.Description) &&
                UniqueDescriptions[field.Description] == false)
            {
                return field.Name;
            }

            return BusinessViewHelper.Replace(field.Description, prefix);
        }


        /// <summary>
        /// Generate unique descriptions
        /// </summary>
        private static void GenerateUniqueDescriptions()
        {
            // Clear first
            UniqueDescriptions.Clear();

            for (var i = 0; i < _view.Fields.Count; i++)
            {
                // Ignore those fields having description "RESERVED"
                if (_view.Fields[i].Description.ToUpper() == "RESERVED")
                {
                    continue;
                }

                if (!UniqueDescriptions.ContainsKey(_view.Fields[i].Description))
                {
                    UniqueDescriptions.Add(_view.Fields[i].Description, true);
                }
                else
                {
                    UniqueDescriptions[_view.Fields[i].Description] = false;
                }
            }
        }

        /// <summary>
        /// Get value from presentation list
        /// </summary>
        /// <param name="i">Outer Loop</param>
        /// <param name="j">Inner Loop</param>
        private static Object GetValue(int i, int j)
        {
            int intVal;
            bool boolVal;
            if (Int32.TryParse(_view.Fields[i].PresentationList.PredefinedValue(j).ToString(), out intVal))
            {
                return intVal;
            }
            if (bool.TryParse(_view.Fields[i].PresentationList.PredefinedValue(j).ToString(), out boolVal))
            {
                return boolVal ? 1 : 0;
            }

            return Convert.ToChar(_view.Fields[i].PresentationList.PredefinedValue(j));
        }

        /// <summary>
        /// Get field's data type
        /// </summary>
        /// <param name="field">The field the data type is to be determined</param>
        /// <returns>Data type of the field</returns>
        private static BusinessDataType FieldType(ViewField field)
        {
            if (field.PresentationType == ViewFieldPresentationType.List)
            {
                return BusinessDataType.Enumeration;
            }

            //Need to use enum field.Type.HasFlag
            var viewfiledType = field.Type;

            if (viewfiledType.GetHashCode() == ViewFieldType.Long.GetHashCode())
            {
                return BusinessDataType.Long;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Char.GetHashCode())
            {
                return BusinessDataType.String;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Date.GetHashCode())
            {
                return BusinessDataType.DateTime;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Int.GetHashCode())
            {
                return BusinessDataType.Integer;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Decimal.GetHashCode())
            {
                return BusinessDataType.Decimal;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Bool.GetHashCode())
            {
                return BusinessDataType.Boolean;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Time.GetHashCode())
            {
                return BusinessDataType.TimeSpan;
            }
            if (viewfiledType.GetHashCode() == ViewFieldType.Byte.GetHashCode())
            {
                return BusinessDataType.Byte;
            }

            return BusinessDataType.Double;
        }

        /// <summary>
        /// Generate enums
        /// </summary>
        private static void GenerateFieldsAndEnums(BusinessView businessView)
        {
            for (var i = 0; i < _view.Fields.Count; i++)
            {

                // Ignore those fields having description "RESERVED"
                if (_view.Fields[i].Description.ToUpper() == "RESERVED")
                {
                    continue;
                }

                var field = _view.Fields[i];
                var businessField = new BusinessField
                {
                    ServerFieldName = field.Name,
                    Name = FieldName(field, businessView.Properties[BusinessView.EntityName]),
                    Description = field.Description,
                    Type = FieldType(field),
                    Id = field.ID,
                    Size = field.Size,
                    IsReadOnly = !field.Attributes.HasFlag(ViewFieldAttributes.Editable),
                    IsCalculated = field.Attributes.HasFlag(ViewFieldAttributes.Calculated),
                    IsRequired = field.Attributes.HasFlag(ViewFieldAttributes.Required),
                    IsKey = field.Attributes.HasFlag(ViewFieldAttributes.Key),
                    IsUpperCase = field.PresentationMask != null && field.PresentationMask.Contains("C"),
                    IsAlphaNumeric = field.PresentationMask != null && field.PresentationMask.Contains("N"),
                    IsNumeric = field.PresentationMask != null && field.PresentationMask.Contains("D"),
                    IsDynamicEnablement = field.Attributes.HasFlag(ViewFieldAttributes.CheckEditable)
                };

                // Add to Keys if it is a key
                if (businessField.IsKey)
                {
                    businessView.Keys.Add(businessField.Name);
                }

                if (field.PresentationType == ViewFieldPresentationType.List)
                {
                    var enumObject = new EnumHelper { Name = businessField.Name };

                    var builder = new StringBuilder();
                    for (var j = 0; j < field.PresentationList.Count; j++)
                    {
                        if (field.PresentationList.PredefinedString(j) != "N/A" || !string.IsNullOrEmpty(field.PresentationList.PredefinedString(j)))
                        {
                            // Will need to prefix key with value in case of dupes (have only seen this with one view (IC0281)) and 
                            // thus will need to resolve in enum class when coding
                            var desc = field.PresentationList.PredefinedString(j);
                            var key = BusinessViewHelper.Replace(desc);
                            var value = GetValue(i, j);

                            // If the value coming from the presentation list is blank, assign it to None
                            if (string.IsNullOrEmpty(key))
                            {
                                key = "None";
                                desc = "None";
                            }

                            // Will strip out this prefix when building enum class
                            enumObject.Values.Add(value + ":" + key + ":" + desc, value);

                            builder.Append(field.PresentationList[j]);
                        }
                    }

                    businessView.Enums.Add(enumObject.Name, enumObject);
                }

                // Add to fields collection
                businessView.Fields.Add(businessField);
            }
        }

        /// <summary> Initializes the Business View </summary>
        /// <param name="settings">Settings</param>
        /// <param name="view">Business View</param>
        private static void Initialize(Settings settings, ref BusinessView view)
        {
            // Locals
            string moduleId;

            // Dynamic Query already has business view!
            if (settings.RepositoryType.Equals(RepositoryType.DynamicQuery))
            {
                view = settings.BusinessView;

                moduleId = view.Properties[BusinessView.ModuleId];

                view.Properties[BusinessView.ModelName] = MakeItSingular(BusinessViewHelper.Replace(view.Properties[BusinessView.ModelName]));
                view.Properties[BusinessView.EntityName] = BusinessViewHelper.Replace(view.Properties[BusinessView.EntityName]);
            }
            // Report already has business view!
            else if (settings.RepositoryType.Equals(RepositoryType.Report))
            {
                view = settings.BusinessView;

                moduleId = view.Properties[BusinessView.ModuleId];

                view.Properties[BusinessView.ModelName] = MakeItSingular(BusinessViewHelper.Replace(view.Properties[BusinessView.ModelName]));
                view.Properties[BusinessView.EntityName] = BusinessViewHelper.Replace(view.Properties[BusinessView.EntityName]);
                view.Properties[BusinessView.ProgramId] = BusinessViewHelper.Replace(view.Properties[BusinessView.ProgramId]);
            }
            else
            {
                moduleId = settings.ModuleId;

                view.Properties.Add(BusinessView.ViewId, _view.ViewID);
                view.Properties.Add(BusinessView.ModelName, settings.BusinessView.Properties[BusinessView.EntityName]);
                view.Properties.Add(BusinessView.ModuleId, moduleId);
                view.Properties.Add(BusinessView.EntityName, settings.BusinessView.Properties[BusinessView.EntityName]);

                GenerateUniqueDescriptions();
                GenerateFieldsAndEnums(view);
            }

            settings.ModuleId = moduleId;

        }

        /// <summary>
        /// Create a class using a T4 template
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="settings">settings</param>
        /// <param name="templateClassName">template class name</param>
        /// <returns>string </returns>
        private static string TransformTemplateToText(BusinessView view, Settings settings, string templateClassName)
        {
            // instantiate a template class
            var type = Type.GetType("Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard." + templateClassName);

            // Protection from incorrect class name
            if (type == null)
            {
                return string.Empty;
            }

            dynamic templateClassInstance = Activator.CreateInstance(type);

            templateClassInstance.Session = new Dictionary<string, object>();

            templateClassInstance.Session["view"] = view;
            templateClassInstance.Session["settings"] = settings;

            templateClassInstance.Initialize();

            return templateClassInstance.TransformText();
        }

        /// <summary> Iterate the view </summary>
        private void IterateView()
        {
            // Locals
            var view = new BusinessView();

            // Validate the settings which will also instantiate the view
            if (!string.IsNullOrEmpty(ValidSettings(_settings, ref view)))
            {
                return;
            }

            // Build the subfolders
            BuildSubfolders(view);

            // Create the Resx Files
            CreateResx(view, _settings.ResxName + ".resx", true);
            CreateResx(view, _settings.ResxName + ".es.resx", false);
            CreateResx(view, _settings.ResxName + ".fr-CA.resx", false);
            CreateResx(view, _settings.ResxName + ".zh-Hans.resx", false);
            CreateResx(view, _settings.ResxName + ".zh-Hant.resx", false);

            // Create the Model class
            CreateClass(view,
                view.Properties[BusinessView.ModelName] + ".cs",
                TransformTemplateToText(view, _settings, "Templates.Common.Class.Model"),
                ModelsKey, SubFolderModelKey);

            // Create the Model Mapper class
            if (!_settings.RepositoryType.Equals(RepositoryType.DynamicQuery))
            {
                CreateClass(view,
                    view.Properties[BusinessView.ModelName] + "Mapper.cs",
                    TransformTemplateToText(view, _settings, "Templates.Common.Class.ModelMapper"),
                    BusinessRepositoryKey, SubFolderBusinessRepositoryMappersKey);
            }


            // Create the Model Fields class
            CreateClass(view,
                view.Properties[BusinessView.ModelName] + "Fields.cs",
                TransformTemplateToText(view, _settings, "Templates.Common.Class.ModelFields"),
                ModelsKey, SubFolderModelFieldsKey);

            // Create _Index.cshtml
            if (!_settings.RepositoryType.Equals(RepositoryType.DynamicQuery))
            {
                CreateClass(view,
                    "Index.cshtml",
                    TransformTemplateToText(view, _settings, "Templates.Common.View.Index"),
                    WebKey, SubFolderWebIndexKey);
            }


            // Create _Localization.cshtml
            if (!_settings.RepositoryType.Equals(RepositoryType.DynamicQuery))
            {
                CreateClass(view,
                    "_Localization.cshtml",
                    TransformTemplateToText(view, _settings, "Templates.Common.View.Localization"),
                    WebKey, SubFolderWebLocalizationKey);
            }

            // Create the Model Enumeration class(es)
            foreach (var value in view.Enums.Values)
            {
                _settings.EnumHelper = value;
            
                CreateClass(view,
                    value.Name + ".cs",
                    TransformTemplateToText(view, _settings, "Templates.Common.Class.ModelEnums"),
                    ModelsKey, SubFolderModelEnumsKey);
            }
            
            // Create classes for Flat Repository Type
            if (_settings.RepositoryType.Equals(RepositoryType.Flat))
            {
                CreateFlatRepositoryClasses(view);
            }

            // Create classes for Process Repository Type
            if (_settings.RepositoryType.Equals(RepositoryType.Process))
            {
                CreateProcessRepositoryClasses(view);
            }

            // Create classes for Dynamic Query Repository Type
            if (_settings.RepositoryType.Equals(RepositoryType.DynamicQuery))
            {
                CreateDynamicQueryRepositoryClasses(view);
            }

            // Create classes for Report Repository Type
            if (_settings.RepositoryType.Equals(RepositoryType.Report))
            {
                CreateReportRepositoryClasses(view);
            }

            // Create classes for Inquiry Repository Type
            if (_settings.RepositoryType.Equals(RepositoryType.Inquiry))
            {
                CreateInquiryRepositoryClasses(view);
            }

            // Create class for finder
            if (_settings.GenerateFinder)
            {
                CreateClass(view,
                    "Find" + view.Properties[BusinessView.EntityName] + "ControllerInternal.cs",
                    TransformTemplateToText(view, _settings, "Templates.Common.Class.Finder"),
                    WebKey, SubFolderWebFinderKey);
            }

            // Update security class
            BusinessViewHelper.UpdateSecurityClass(view, _settings);
        }

        /// <summary> Create the class </summary>
        /// <param name="view">Business View</param>
        /// <param name="fileName"> Name of file to be created</param>
        /// <param name="content">File contents</param>
        /// <param name="projectKey">Project Key for Project Info</param>
        /// <param name="subfolderKey">Subfolder Key for Project Info</param>
        private void CreateClass(BusinessView view, string fileName, string content, string projectKey, string subfolderKey)
        {
            // Update display of file being processed
            if (ProcessingEvent != null)
            {
                ProcessingEvent(fileName);
            }

            // Save the file
            var success = SaveFile(view, fileName, content, projectKey, subfolderKey);

            // Update status
            LaunchStatusEvent(success, fileName);
        }

        /// <summary> Update UI </summary>
        /// <param name="success">True/False based upon class generation</param>
        /// <param name="fileName">name of file to be created</param>
        private void LaunchStatusEvent(bool success, string fileName)
        {
            if (StatusEvent != null)
            {
                if (success)
                {
                    StatusEvent(fileName, Info.StatusType.Success, string.Empty);
                }
                else
                {
                    StatusEvent(fileName, Info.StatusType.Error, string.Format(Resources.ErrorCreatingFile, fileName));
                }
            }
        }

        /// <summary> Create Flat Repository Classes </summary>
        /// <param name="view">Business View</param>
        private void CreateFlatRepositoryClasses(BusinessView view)
        {
            // Create the Business Repository Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Entity.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.RepositoryInterface"),
                InterfacesKey, SubFolderInterfacesBusinessRepositoryKey);


            // Create the Service Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.ServiceInterface"),
                InterfacesKey, SubFolderInterfacesServicesKey);

            // Create the Service class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "EntityService.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.Service"),
                ServicesKey, SubFolderServicesKey);

            // Create the ViewModel class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ViewModel.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.ViewModel"),
                WebKey, SubFolderWebViewModelKey);

            // Create the Internal Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ControllerInternal.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.InternalController"),
                WebKey, SubFolderWebControllersKey);

            // Create the public Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Controller.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.Controller"),
                WebKey, SubFolderWebControllersKey);

            // Create the Repository class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Repository.cs",
                TransformTemplateToText(view, _settings, "Templates.Flat.Class.Repository"),
                BusinessRepositoryKey, SubFolderBusinessRepositoryKey);

            // Create partial view.cshtml
            var fileName = "_" + view.Properties[BusinessView.EntityName] + ".cshtml";
            CreateClass(view,
                fileName,
                TransformTemplateToText(view, _settings, "Templates.Flat.View.Entity"),
                WebKey, SubFolderWebLocalizationKey);

            // Register types
            BusinessViewHelper.UpdateFlatBootStrappers(view, _settings);
            BusinessViewHelper.UpdateFlatBundles(view, _settings);

            // set the start page
            BusinessViewHelper.CreateFlatViewPageUrl(view, _settings);

            //Update the plugin menu details
            BusinessViewHelper.UpdateMenuDetails(view, _settings);

            // For javascript files, the project name does not include the .Web segment
            var projectName =
                _settings.Projects[WebKey][view.Properties[BusinessView.ModuleId]].ProjectName.Replace(".Web", string.Empty);

            // Create the Behavior JavaScript file
            CreateClass(view,
                projectName + view.Properties[BusinessView.EntityName] + "Behaviour.js",
                TransformTemplateToText(view, _settings, "Templates.Flat.Script.Behaviour"),
                WebKey, SubFolderWebScriptsKey);

            // Create the Knockout Extension JavaScript file
            CreateClass(view,
                projectName + view.Properties[BusinessView.EntityName] + "KoExtn.js",
                TransformTemplateToText(view, _settings, "Templates.Flat.Script.KoExtn"),
                WebKey, SubFolderWebScriptsKey);

            // Create the Repository JavaScript file
            CreateClass(view,
                projectName + view.Properties[BusinessView.EntityName] + "Repository.js",
                TransformTemplateToText(view, _settings, "Templates.Flat.Script.Repository"),
                WebKey, SubFolderWebScriptsKey);
        }

        /// <summary> Create Process Repository Classes </summary>
        /// <param name="view">Business View</param>
        private void CreateProcessRepositoryClasses(BusinessView view)
        {
            // Create the Business Repository Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Entity.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.RepositoryInterface"),
                InterfacesKey, SubFolderInterfacesBusinessRepositoryKey);

            // Create the Service Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.ServiceInterface"),
                InterfacesKey, SubFolderInterfacesServicesKey);

            // Create the Service class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.Service"),
                ServicesKey, SubFolderServicesKey);

            // Create the ViewModel class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ViewModel.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.ViewModel"),
                WebKey, SubFolderWebViewModelKey);

            // Create the Internal Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ControllerInternal.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.InternalController"),
                WebKey, SubFolderWebControllersKey);

            // Create the public Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Controller.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.Controller"),
                WebKey, SubFolderWebControllersKey);

            //Create the Repository class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Repository.cs",
                TransformTemplateToText(view, _settings, "Templates.Process.Class.Repository"),
                 BusinessRepositoryKey, SubFolderBusinessRepositoryKey);
        }

        /// <summary> Create Dynamic Query Repository Classes </summary>
        /// <param name="view">Business View</param>
        private void CreateDynamicQueryRepositoryClasses(BusinessView view)
        {
            // Create the Business Repository Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Entity.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.RepositoryInterface"),
                InterfacesKey, SubFolderInterfacesBusinessRepositoryKey);

            // Create the Service Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.ServiceInterface"),
                InterfacesKey, SubFolderInterfacesServicesKey);

            // Create the Service class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "EntityService.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.Service"),
                ServicesKey, SubFolderServicesKey);

            // Create the ViewModel class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ViewModel.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.ViewModel"),
                WebKey, SubFolderWebViewModelKey);

            // Create the public Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Controller.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.Controller"),
                WebKey, SubFolderWebControllersKey);

            //Create the Repository class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Repository.cs",
                TransformTemplateToText(view, _settings, "Templates.DynamicQuery.Class.Repository"),
                BusinessRepositoryKey, SubFolderBusinessRepositoryKey);

        }

        /// <summary> Create Report Repository Classes </summary>
        /// <param name="view">Business View</param>
        private void CreateReportRepositoryClasses(BusinessView view)
        {
            // Create the Business Repository Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Entity.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.RepositoryInterface"),
                InterfacesKey, SubFolderInterfacesBusinessRepositoryKey);

            // Create the Service Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.ServiceInterface"),
                InterfacesKey, SubFolderInterfacesServicesKey);

            // Create the Service class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "EntityService.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.Service"),
                ServicesKey, SubFolderServicesKey);

            // Create the ViewModel class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ViewModel.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.ViewModel"),
                WebKey, SubFolderWebViewModelKey);

            // Create the public Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Controller.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.Controller"),
                WebKey, SubFolderWebControllersKey);

            // Create the internal Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ControllerInternal.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.InternalController"),
                WebKey, SubFolderWebControllersKey);

            //Create the Repository class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Repository.cs",
                TransformTemplateToText(view, _settings, "Templates.Reports.Class.Repository"),
                BusinessRepositoryKey, SubFolderBusinessRepositoryKey);
        }

        /// <summary> Create Inquiry Repository Classes </summary>
        /// <param name="view">Business View</param>
        private void CreateInquiryRepositoryClasses(BusinessView view)
        {
            // Create the Business Repository Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Entity.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.RepositoryInterface"),
                InterfacesKey, SubFolderInterfacesBusinessRepositoryKey);

            // Create the Service Interface class
            CreateClass(view,
                "I" + view.Properties[BusinessView.EntityName] + "Service.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.ServiceInterface"),
                InterfacesKey, SubFolderInterfacesServicesKey);

            // Create the Service class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "EntityService.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.Service"),
                ServicesKey, SubFolderServicesKey);

            // Create the ViewModel class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ViewModel.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.ViewModel"),
                WebKey, SubFolderWebViewModelKey);

            // Create the Internal Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "ControllerInternal.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.InternalController"),
                WebKey, SubFolderWebControllersKey);

            // Create the public Controller class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Controller.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.Controller"),
                WebKey, SubFolderWebControllersKey);

            //Create the Repository class
            CreateClass(view,
                view.Properties[BusinessView.EntityName] + "Repository.cs",
                TransformTemplateToText(view, _settings, "Templates.Inquiry.Class.Repository"),
                BusinessRepositoryKey, SubFolderBusinessRepositoryKey);
        }

        /// <summary>
        /// Create the Resx content
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="fileName">Resx File Name</param>
        /// <param name="addDescription">True to add descriptions otherwise false</param>
        private void CreateResx(BusinessView view, string fileName, bool addDescription)
        {
            // Update display of file being processed
            if (ProcessingEvent != null)
            {
                ProcessingEvent(fileName);
            }

            // Save the file
            var success = SaveResxFile(view, fileName, addDescription);

            // Update status
            LaunchStatusEvent(success, fileName);
        }


        /// <summary>
        /// Save the Resx content
        /// </summary>
        /// <param name="view">Business View</param>
        /// <param name="fileName">Resx File Name</param>
        /// <param name="addDescription">True to add descriptions otherwise false</param>
        private bool SaveResxFile(BusinessView view, string fileName, bool addDescription)
        {
            // Vars
            var retVal = true;
            var projectInfo = _settings.Projects[ResourcesKey][view.Properties[BusinessView.ModuleId]];
            var filePath = BusinessViewHelper.ConcatStrings(new[] { projectInfo.ProjectFolder, projectInfo.Subfolders[SubFolderResourcesKey] });
            var fullFileName = BusinessViewHelper.ConcatStrings(new[] { filePath, fileName });

            // Determine if exists
            var exists = File.Exists(fullFileName);
            var writeFile = true;

            // Prompt if file exists?
            if (exists && _settings.PromptIfExists)
            {
                var result = MessageBox.Show(string.Format(Resources.FileExists, fullFileName),
                    Resources.Confirmation, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // Evaluate
                switch (result)
                {
                    case DialogResult.Yes:
                        // Overwrite the file
                        break;
                    case DialogResult.No:
                        // Skip the file
                        writeFile = false;
                        break;
                    case DialogResult.Cancel:
                        // Abort the wizard
                        throw new Exception();
                }
            }

            // Write the file?
            if (writeFile)
            {
                try
                {
                    // Ensure the filepath exists
                    if (!exists)
                    {
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                    }

                    // Create resx file
                    var resx = new ResXResourceWriter(fullFileName);
                    var uniqueList = new List<string>();

                    // Add Enity key
                    resx.AddResource(new ResXDataNode("Entity", addDescription ? view.Properties[BusinessView.ModuleId] +
                        " " + view.Properties[BusinessView.EntityName] : string.Empty));

                    // Iterate fields collection
                    foreach (var node in view.Fields.Select(field => new ResXDataNode(field.Name, addDescription ? field.Description : string.Empty)).Where(node => !uniqueList.Contains(node.Name, StringComparer.CurrentCultureIgnoreCase)))
                    {
                        resx.AddResource(node);
                        uniqueList.Add(node.Name);
                    }

                    // Iterate enumerations
                    foreach (var enumHelper in view.Enums.Values)
                    {
                        foreach (var value in enumHelper.Values)
                        {
                            // Locals - Used to split out prefix and replace invalid characters
                            var tmp = value.Key.Split(':');
                            var key = tmp[1]; // Replace function already performed
                            var description = tmp[2]; // Raw from presentation list

                            // Do not add if already present or key is blank
                            if (string.IsNullOrEmpty(key) || uniqueList.Contains(key, StringComparer.CurrentCultureIgnoreCase))
                            {
                                continue;
                            }

                            // Add to resource
                            resx.AddResource(new ResXDataNode(key, addDescription ? description : string.Empty));
                            uniqueList.Add(key);
                        }
                    }

                    _settings.ResourceKeys = uniqueList;
                    resx.Close();
                }
                catch
                {
                    retVal = false;
                }

                // Update project if write was successful
                if (retVal)
                {
                    // Add to project
                    try
                    {
                        var createdItem = projectInfo.Project.ProjectItems.AddFromFile(fullFileName);

                        // Only add code behind file for english resx file
                        if (addDescription)
                        {
                            createdItem.Properties.Item("CustomTool").Value = "PublicResXFileCodeGenerator";
                        }
                    }
                    catch
                    {
                        retVal = false;
                    }
                }

            }
            else
            {
                retVal = false;
            }

            return retVal;
        }

        /// <summary>
        /// Is model name valid
        /// </summary>
        /// <param name="view">Business View</param>
        /// <returns>True if view does not have a field the same as model name otherwise false</returns>
        private static bool ValidModel(BusinessView view)
        {
            var modelName = view.Properties[BusinessView.ModelName];

            return !view.Fields.Any(t => t.Name.Equals(modelName));
        }

        #endregion
    }
}
