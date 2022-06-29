// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACCPAC.Advantage;
using Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard.Properties;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;

#region Sage 300 Models
// Sage 300 Models - Added to ensure relection is successful
using Sage.CA.SBS.ERP.Sage300.AP.Models;
using Sage.CA.SBS.ERP.Sage300.AR.Models;
using Sage.CA.SBS.ERP.Sage300.AS.Models;
using Sage.CA.SBS.ERP.Sage300.CS.Models;
using Sage.CA.SBS.ERP.Sage300.GL.Models;
using Sage.CA.SBS.ERP.Sage300.IC.Models;
using Sage.CA.SBS.ERP.Sage300.KN.Models;
using Sage.CA.SBS.ERP.Sage300.KPI.Models;
using Sage.CA.SBS.ERP.Sage300.MT.Models;
using Sage.CA.SBS.ERP.Sage300.OE.Models;
using Sage.CA.SBS.ERP.Sage300.PM.Models;
using Sage.CA.SBS.ERP.Sage300.PO.Models;
using Sage.CA.SBS.ERP.Sage300.PR.Models;
using Sage.CA.SBS.ERP.Sage300.VPF.Models;
#endregion

#region Partner Models
// Partner Models - To be added to ensure relection is successful
// TBD - Partners to add references to their models to ensure reflection is
//       able to access all types included in assembly. See examples of this
//       in the above 'Sage 300 Models' section
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ViewFieldAttrWizard
{
    /// <summary> Process Generation Class (worker) </summary>
    internal class ProcessGeneration
    {
        #region Private Variables
        /// <summary> Settings from UI </summary>
        private Settings _settings;
        #endregion

        #region Private Constants
        private const string TOKEN_NAMESPACE = "namespace";
        private const string TOKEN_PUBLIC = "public";
        private const string TOKEN_PARTIAL_CLASS = " partial class";
        private const string TOKEN_DOT = ".";
        private const string TOKEN_ENTITY_NAME = "EntityName";
        private const string TOKEN_VIEW_NAME = "ViewName";
        private const string TOKEN_US_ENTITY_NAME = "USEntityName";
        private const string TOKEN_INDEX = "+Index";
        private const string TOKEN_FIELDS = "+Fields";
        private const string TOKEN_DLL = ".dll";
        private const string TOKEN_COMMENT = @"//";
        private const string TOKEN_APPID = "WX";
        private const string TOKEN_PROGRAM_NAME = "WX1000";
        private const string TOKEN_VIEW_FIELD = "[ViewField";
        private const string TOKEN_VIEW_FIELD_NAME = "(Name = Fields.";
        private const string TOKEN_ID = ", Id = Index.";
        private const string TOKEN_FIELD_TYPE = ", FieldType = EntityFieldType.";
        private const string TOKEN_SIZE = ", Size = ";
        private const string TOKEN_PRECISION = ", Precision = ";
        private const string TOKEN_MASK = ", Mask = ";
        private const string TOKEN_QUOTE = "\"";
        private const string TOKEN_BRACKETS = "()";
        private const string TOKEN_END_BRACKET = ")]";
        private const string TOKEN_COMMON_MODELS = "using Sage.CA.SBS.ERP.Sage300.Common.Models;";
        private const string TOKEN_COMMON_MODELS_ATTRS = "using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;";
        private const string TOKEN_COMMON_MODELS_COMMENT = @"// Added to support ViewField Attributes;";
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

            // Iterate files to be modified
            foreach (var file in _settings.Files)
            {
                ModifyModel(file);
            }
        }
        #endregion

        #region Private methods
        /// <summary> Update UI </summary>
        /// <param name="success">True/False based upon creation</param>
        /// <param name="fileName">Name of file to be created</param>
        /// <param name="failureMessage">Failure message</param>
        private void LaunchStatusEvent(bool success, string fileName, string failureMessage = null)
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
                StatusEvent(fileName, Info.StatusType.Error, failureMessage);
            }
        }

        /// <summary> Update UI </summary>
        /// <param name="fileName">Name of file to be created</param>
        private void LaunchProcessingEvent(string fileName)
        {
            // Event if subscriber
            ProcessingEvent?.Invoke(fileName);
        }

        /// <summary> Model the model file with viewField attributes </summary>
        /// <param name="fileName">Model file name</param>
        private void ModifyModel(string fileName)
        {
            try
            {
                // Update display of file being processed
                LaunchProcessingEvent(fileName);

                // Open the source file and read it's contents
                var file = GetFileContents(fileName);
                if (file == null)
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName, Resources.ErrorFile);
                    return;
                }

                // Get namespace and class name
                var nameSpace = GetToken(file, TOKEN_NAMESPACE, 1);
                var className = GetToken(file, TOKEN_PUBLIC + TOKEN_PARTIAL_CLASS, 3);
                if (string.IsNullOrEmpty(nameSpace) || string.IsNullOrEmpty(className))
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName, Resources.ErrorNamespaceClass);
                    return;
                }

                // Load assembly discovered in namespace
                var assembly = GetAssembly(nameSpace);
                if (assembly == null)
                {
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName, string.Format(Resources.ErrorAssembly, nameSpace));
                    return;
                }

                // Get the model from the assembly
                var model = GetType(assembly, nameSpace + TOKEN_DOT + className);
                if (model == null)
                {
                    Cleanup(assembly);
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName, 
                        string.Format(Resources.ErrorModel, nameSpace + TOKEN_DOT + className));
                    return;
                }

                // Get the entity name and view name (some process models) from the model for opening the Accpac View
                var entityName = GetFieldValue(model, TOKEN_ENTITY_NAME);
                var viewName = GetFieldValue(model, TOKEN_VIEW_NAME);
                var uSentityName = GetFieldValue(model, TOKEN_US_ENTITY_NAME);
                if (string.IsNullOrEmpty(entityName) && string.IsNullOrEmpty(viewName) && string.IsNullOrEmpty(uSentityName))
                {
                    Cleanup(assembly, model);
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName,
                        string.Format(Resources.ErrorEntityViewName, nameSpace + TOKEN_DOT + className));
                    return;
                }
                // Special logic for early PR screens where USEntityName is used
                if (string.IsNullOrEmpty(entityName) && !string.IsNullOrEmpty(uSentityName))
                {
                    entityName = uSentityName;
                }

                // Open the Accpac view
                var view = GetView(entityName ?? viewName);
                if (view == null)
                {
                    Cleanup(assembly, model);
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName,
                        string.Format(Resources.ErrorView, entityName ?? viewName));
                    return;
                }

                // Get index class for members of the Accpac View
                var index = GetType(assembly, nameSpace + TOKEN_DOT + className + TOKEN_INDEX);
                if (index == null)
                {
                    Cleanup(assembly, model, view);
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName,
                        string.Format(Resources.ErrorModelIndex, nameSpace + TOKEN_DOT + className + TOKEN_INDEX));
                    return;
                }

                // Get fields class for members of the Accpac View
                var fields = GetType(assembly, nameSpace + TOKEN_DOT + className + TOKEN_FIELDS);
                if (fields == null)
                {
                    Cleanup(assembly, model, view, index);
                    // Failure. Update status
                    LaunchStatusEvent(false, fileName,
                        string.Format(Resources.ErrorModelFields, nameSpace + TOKEN_DOT + className + TOKEN_FIELDS));
                    return;
                }

                // Iterate the file looking for the properties
                if (ViewFieldAttrs(file, index, fields, view, className))
                {
                    // Update the file with the changes
                    UpdateFile(fileName, file);
                }

                Cleanup(assembly, model, view, index, fields);

                // Success. Update status
                LaunchStatusEvent(true, fileName);
            }
            catch (Exception ex)
            {
                // Failure. Update status
                var failureMessage = ex.Message;
                if (string.IsNullOrEmpty(failureMessage) && ex.InnerException != null)
                {
                    failureMessage = ex.InnerException.Message;
                }
                LaunchStatusEvent(false, fileName, failureMessage);
            }

        }

        /// <summary> Gets the file </summary>
        /// <param name="fileName">Fully qualified file name</param>
        /// <returns>File contents</returns>
        private List<string> GetFileContents(string fileName)
        {
            return File.ReadAllLines(fileName).ToList();
        }

        /// <summary> Get the assembly</summary>
        /// <param name="nameSpace">Namespace discovered in file</param>
        /// <returns>Assembly</returns>
        private System.Reflection.Assembly GetAssembly(string nameSpace)
        {
            // Namespace for assembly must first strip off any subfolders
            var assemblyNamespace = nameSpace;

            if (!assemblyNamespace.EndsWith(".Models"))
            {
                assemblyNamespace = nameSpace.Substring(0, nameSpace.IndexOf(".Models.")) + ".Models";
            }
            return System.Reflection.Assembly.LoadFile(Path.Combine(RegistryHelper.Sage300CWebFolder,
                assemblyNamespace + TOKEN_DLL));
        }

        /// <summary> Get the type from the string</summary>
        /// <param name="assembly">Assembly containing type</param>
        /// <param name="type">Discovered type</param>
        /// <returns>Type in the assembly</returns>
        private Type GetType(System.Reflection.Assembly assembly, string type)
        {
            return assembly.GetType(type);
        }

        /// <summary> Parse the token from the file </summary>
        /// <param name="file">File contents</param>
        /// <param name="token">Token to search for</param>
        /// <param name="tokenIndex">Index of token</param>
        /// <returns>Token value</returns>
        private string GetToken(List<string> file, string token, int tokenIndex)
        {
            // Locals
            var retVal = string.Empty;

            // Iterate file
            foreach (var line in file)
            {
                if (line.Contains(token) && !line.Trim().StartsWith("#region") && !line.Trim().StartsWith(TOKEN_COMMENT))
                {
                    var tmp = line.Trim().Split(' ');
                    retVal = tmp[tokenIndex].Trim();

                    // Strip any comment
                    var comment = retVal.IndexOf(TOKEN_COMMENT);
                    if (comment > 0)
                    {
                        retVal = retVal.Substring(0, comment - 1);
                    }
                    // Done with file
                    break;
                }
            }

            return retVal;
        }

        /// <summary> Get the Accpac view for the model (entity name) </summary>
        /// <param name="entityName">Entity name</param>
        /// <returns>Accpac View</returns>
        private View GetView(string entityName)
        {
            // Open the Accpac view
            var session = new Session();
            session.InitEx2(null, string.Empty, TOKEN_APPID, TOKEN_PROGRAM_NAME, _settings.Version, 1);
            session.Open(_settings.UserName, _settings.UserKey, _settings.CompanyId, DateTime.UtcNow, 0);

            var dbLink = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
            return dbLink.OpenView(entityName);
        }

        /// <summary> Modify the file with the attribute for ViewField </summary>
        /// <param name="file">File contents</param>
        /// <param name="index">Index class of model</param>
        /// <param name="fields">Index class of model</param>
        /// <param name="view">Accpac view</param>
        /// <param name="className">Class name of model</param>
        /// <returns>True if success otherwise false</returns>
        private bool ViewFieldAttrs(List<string> file, Type index, Type fields, View view, string className)
        {
            // If the file is modified, will need to check if a using statement needs to be added
            var isModified = false;

            // Iterate fields in the Accpac View via the Index class of the model
            foreach (var indexField in index.GetFields())
            {
                // Get the model's property name, the Id value for the view field, and the view field
                var propertyName = indexField.Name;
                ViewField field = null;
                try
                {
                    field = view.Fields.FieldByID(Convert.ToInt32(indexField.GetValue(propertyName)));
                }
                catch
                {
                    // If we are here, there are more fields in the Index class than in the view
                    // Therefore, skip this field
                    continue;
                }

                // Locate the fields class property name. It has been discovered that the fields and index
                // properties are sometimes named differently (only 25 discovered but what!!!)
                var hasFieldsProperty = false;
                try
                {
                    hasFieldsProperty = (fields.GetField(propertyName) != null);
                }
                catch
                {
                    // Already set to false
                }

                // Determine if the property has the ViewField attribute
                var hasAttr = HasToken(file, TOKEN_VIEW_FIELD + TOKEN_VIEW_FIELD_NAME + propertyName + ",");
                var attrLine = string.Empty;

                // Iterate the file looking for the property/field
                foreach (var line in file)
                {
                    // Store the [ViewField... line, if any, prior to the discovery of the
                    // property in case it needs modification as opposed to insertion
                    if (line.Contains(TOKEN_VIEW_FIELD))
                    {
                        attrLine = line;
                    }

                    // Skip class definition and constructor
                    if (line.Contains(TOKEN_PUBLIC + TOKEN_PARTIAL_CLASS) ||
                        line.Contains(TOKEN_PUBLIC + " " + className + TOKEN_BRACKETS))
                    {
                        continue;
                    }

                    // Property discovered
                    else if (line.Contains(TOKEN_PUBLIC) && line.Contains(" " + propertyName + " "))
                    {
                        // Ensure the property name was discovered and not the type of the same name
                        var tmp = line.Trim().Split(' ');
                        if (!tmp[2].Trim().Equals(propertyName))
                        {
                            continue;
                        }

                        // Build the Attribute string
                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append(line.Substring(0, line.IndexOf(TOKEN_PUBLIC)));
                        
                        // If the Fields class property was not found, then add comment to line
                        if (!hasFieldsProperty)
                        {
                            stringBuilder.Append(TOKEN_COMMENT);
                        }

                        stringBuilder.Append(TOKEN_VIEW_FIELD + TOKEN_VIEW_FIELD_NAME + propertyName +
                            TOKEN_ID + propertyName +
                            TOKEN_FIELD_TYPE + EnumUtility.GetEnum<EntityFieldType>(field.Type.ToString()));
                        // Only add size if > 0
                        if (field.Size > 0)
                        {
                            stringBuilder.Append(TOKEN_SIZE + field.Size);
                        }
                        // Only add precision if > 0
                        if (field.Precision > 0)
                        {
                            stringBuilder.Append(TOKEN_PRECISION + field.Precision);
                        }
                        // Only add mask if != ""
                        if (!string.IsNullOrEmpty(field.PresentationMask))
                        {
                            stringBuilder.Append(TOKEN_MASK + TOKEN_QUOTE + field.PresentationMask + TOKEN_QUOTE);
                        }
                        stringBuilder.Append(TOKEN_END_BRACKET);

                        // Set modified flag
                        isModified = true;

                        if (!hasAttr)
                        {
                            // Insert the line
                            file.Insert(file.IndexOf(line), stringBuilder.ToString());
                        }
                        else
                        {
                            // Modify the line
                            file[file.IndexOf(attrLine)] = stringBuilder.ToString();
                        }
                        break;
                    }
                }
            }

            // If the file has been modified, add using statement if required
            if (isModified)
            {
                AddUsingStatement(file);
            }
            return isModified;
        }

        /// <summary> Add using statement(s), if needed, to support new ViewField attribute </summary>
        /// <param name="file">File contents</param>
        private void AddUsingStatement(List<string> file)
        {
            // Determine if the file requires using statement(s) to be added
            var hasCommonModels = HasToken(file, TOKEN_COMMON_MODELS);
            var hasCommonModelsAttrs = HasToken(file, TOKEN_COMMON_MODELS_ATTRS);

            // Bail if already has both reeferences
            if (hasCommonModels && hasCommonModelsAttrs)
            {
                return;
            }

            // Iterate the file looking for the namespace and will evaluate what then needs to be added
            foreach (var line in file)
            {
                // Namespace discovered
                if (line.StartsWith(TOKEN_NAMESPACE))
                {
                    // Insert the comment
                    file.Insert(file.IndexOf(line), "");
                    file.Insert(file.IndexOf(line), TOKEN_COMMON_MODELS_COMMENT);

                    // Insert the line for common models?
                    if (!hasCommonModels)
                    {
                        file.Insert(file.IndexOf(line), TOKEN_COMMON_MODELS);
                    }
                    // Insert the line for common models attributes?
                    if (!hasCommonModelsAttrs)
                    {
                        file.Insert(file.IndexOf(line), TOKEN_COMMON_MODELS_ATTRS);
                    }
                    // Wrap in blank line for proper separation
                    file.Insert(file.IndexOf(line), "");
                    break;
                }
            }
        }

        /// <summary> Has the requested token </summary>
        /// <param name="file">File contents</param>
        /// <param name="token">Token to search for</param>
        /// <returns>True if token already exists otherwise false</returns>
        private bool HasToken(List<string> file, string token)
        {
            // Determine if the model has the token already
            return file.FirstOrDefault(line => line.Contains(token))?.Count() > 0;
        }

        /// <summary> Get the value of the entered field </summary>
        /// <param name="model">Model containing field</param>
        /// <param name="name">Field name in model</param>
        /// <returns>Value or string.empty</returns>
        private string GetFieldValue(Type model, string name)
        {
            return model.GetField(name)?.GetValue(name).ToString();
        }

        /// <summary> Update the file </summary>
        /// <param name="fileName">Fully qualified file name</param>
        /// <param name="file">File contents</param>
        private void UpdateFile(string fileName, List<string> file)
        {
            File.Delete(fileName);
            File.WriteAllLines(fileName, file);
        }

        /// <summary> Cleanup </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="model">Type for model</param>
        /// <param name="view">Business view</param>
        /// <param name="index">Type for index</param>
        /// <param name="fields">Type for fields</param>
        private void Cleanup(System.Reflection.Assembly assembly, Type model = null, 
            View view = null, Type index = null, Type fields = null)
        {
            try
            {
                assembly = null;
                model = null;
                index = null;
                fields = null;

                if (view != null)
                {
                    view.Dispose();
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}
