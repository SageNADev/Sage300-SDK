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

#region Imports
using ACCPAC.Advantage;
using Newtonsoft.Json;
using Sage300InquiryConfigurationGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AccpacView = ACCPAC.Advantage.View;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    public static class Generation
    {
        /// <summary>
        /// A reference to the parent window/object
        /// </summary>
        public static MainForm Parent { get; set; }

        public enum LanguageEnum
        {
            ENG = 0,
            FRA,
            ESN,
            CHT,
            CHN
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

        /// <summary>
        /// Get value from presentation list
        /// </summary>
        /// <param name="i">Outer Loop</param>
        /// <param name="j">Inner Loop</param>
        /// <param name="view">Accpac Business View</param>
        private static Object GetValue(int i, int j, AccpacView view)
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
        /// Process Sage 300 View Column Setting XLSX file and generate Sage 300 View Column Definition JSON file
        /// </summary>
        /// <param name="cr">InquiryConfigurationDefinition</param>
        /// <param name="ConfigurationColumnList">List of Sage 300 View Column Settings</param>
        public static void ProcessView(Company companySetting, 
                                       InquiryConfigurationDefinition cr, 
                                       List<ConfigurationColumnSettingDefinition> ConfigurationColumnList, 
                                       List<OverridePresentationList> OverridePresentationList)
        {
            const string methodName = "ProcessView";
            LogMethodStart(methodName);

            string TestTitle = "Process Sage 300 View";

            List<LogRecord> _ProcessLogs = new List<LogRecord>();
            LogRecord tranRec = null;

            string today = DateTime.Now.ToString("yyyyMMdd");

            DateTime start = DateTime.Now;

            List<FieldDefinition> ColumnFields = new List<FieldDefinition>();

            #region Process
            try
            {
                start = DateTime.Now;

                #region OpenSession
                Session session, sessionFra, sessionEsn, sessionChn, sessionCht;
                DBLink dbLink, dbLinkFra, dbLinkEsn, dbLinkChn, dbLinkCht;
                AccpacView view, viewFra, viewEsn, viewChn, viewCht;

                var viewId = cr.ViewID.Substring(0, 6);
                OpenAccpacView(companySetting, LanguageEnum.ENG, viewId, out session, out dbLink, out view);
                OpenAccpacView(companySetting, LanguageEnum.FRA, viewId, out sessionFra, out dbLinkFra, out viewFra);
                OpenAccpacView(companySetting, LanguageEnum.ESN, viewId, out sessionEsn, out dbLinkEsn, out viewEsn);
                OpenAccpacView(companySetting, LanguageEnum.CHN, viewId, out sessionChn, out dbLinkChn, out viewChn);
                OpenAccpacView(companySetting, LanguageEnum.CHT, viewId, out sessionCht, out dbLinkCht, out viewCht);
                #endregion

                #region ProcessViewField
                for (var i = 0; i < view.Fields.Count; i++)
                {
                    ViewField field, fieldFra, fieldEsn, fieldChn, fieldCht;   

                    field = view.Fields[i];
                    fieldFra = companySetting.IncludeFra ? viewFra.Fields[i] : null;
                    fieldEsn = companySetting.IncludeEsn ? viewEsn.Fields[i] : null;
                    fieldChn = companySetting.IncludeChn ? viewChn.Fields[i] : null;
                    fieldCht = companySetting.IncludeCht ? viewCht.Fields[i] : null;

                    ConfigurationColumnSettingDefinition r = null;
                    if (ConfigurationColumnList.Exists(c => c.Field == field.Name))
                        r = ConfigurationColumnList.First(c => c.Field == field.Name);

                    // Ignore those fields having description "RESERVED" or not in the ConfigurationColumnSetting file
                    if (view.Fields[i].Description.ToUpper() == "RESERVED" || r == null)
                    {
                        continue;
                    }

                    SourceDataType sdt = FieldType(field);

                    #region SetFieldList
                    // Only apply aggregation to Amount field
                    if (r.AggregatesStr != "N" && sdt.ToString() == "Decimal")
                    {
                        r.Aggregates.Add("count");
                        r.Aggregates.Add("max");
                        r.Aggregates.Add("min");
                        r.Aggregates.Add("avg");
                        r.Aggregates.Add("sum");
                    }

                    var sourceColumn = new FieldDefinition
                    {
                        FieldIndex = field.ID,
                        Field = field.Name,
                        FieldAlias = r.FieldAlias,
                        TableName = (r.TableName == "") ? cr.ViewName : r.TableName,
                        DataTypeEnumeration = (int)sdt,
                        DataType = sdt.ToString(),
                        ViewID = cr.ViewID,
                        ViewName = cr.ViewName,
                        Precision = field.Precision,
                        IsSQLColumnInView = false,
                        ColumnNameInView = "",
                        Included = (r.Included == "Y"),
                        IsFilterable = (r.IsFilterable == "Y"),
                        IsDummy = (r.IsDummy == "Y"),
                        IsSortable = (r.IsSortable == "Y"),
                        SortFieldsStr = r.SortFieldsStr,
                        IsDrilldown = (r.IsDrilldown == "Y"),
                        IsGroupable = (r.IsGroupBy == "Y"),
                        IsAggregate = (r.AggregatesStr == "N") ? false : true,
                        DrilldownURL = r.DrilldownUrl,
                        Aggregates = r.Aggregates,
                        LicenceCheck = r.LicenceCheck
                    };


                    sourceColumn.IsAggregate = (r.Aggregates.Count() > 0);

                    if (r.DisplayCondition.Field != string.Empty)
                    {
                        sourceColumn.DisplayCondition = r.DisplayCondition;
                    }

                    if (r.Captions.Count() > 0)
                    {
                        sourceColumn.Captions = r.Captions;
                    }
                    else
                    {
                        // Capitalize first letter of a word
                        TextInfo ti = new CultureInfo("en", false).TextInfo;
                        sourceColumn.Captions.Add(new Translation("ENG", ti.ToTitleCase(field.Description)));

                        if (companySetting.IncludeFra)
                        {
                            ti = new CultureInfo("fr", false).TextInfo;
                            sourceColumn.Captions.Add(new Translation("FRA", SourceHelper.FR_Replace(ti.ToTitleCase(fieldFra.Description))));
                        }

                        if (companySetting.IncludeEsn)
                        {
                            ti = new CultureInfo("es", false).TextInfo;
                            sourceColumn.Captions.Add(new Translation("ESN", SourceHelper.ES_Replace(ti.ToTitleCase(fieldEsn.Description))));
                        }

                        if (companySetting.IncludeChn)
                        {
                            sourceColumn.Captions.Add(new Translation("CHN", fieldChn.Description));
                        }

                        if (companySetting.IncludeCht)
                        {
                            sourceColumn.Captions.Add(new Translation("CHT", fieldCht.Description));
                        }
                    }

                    #region SetPresentationList

                    //if (r.YesNoPresentation == "Y")
                    if (r.OverridePrList != "")
                    {
                        //sourceColumn.PresentationList = YesNoPresentationList;
                        OverridePresentationList op = new OverridePresentationList();
                        if (OverridePresentationList.Exists(cp => (cp.Type == r.OverridePrList)))
                            op = OverridePresentationList.First(cp => (cp.Type == r.OverridePrList));
                        sourceColumn.PresentationList = op.PresentationList;
                        sourceColumn.DataType = "Enumeration";
                    }
                    else if (field.PresentationType == ViewFieldPresentationType.List)
                    {
                        for (var j = 0; j < field.PresentationList.Count; j++)
                        {
                            PresentationList pl = new PresentationList(false, "");

                            if (field.PresentationList.PredefinedString(j) != @"N/A" ||
                                !string.IsNullOrEmpty(field.PresentationList.PredefinedString(j)))
                            {
                                var value = GetValue(i, j, view);

                                pl.Selected = (j == 0) ? true : false;
                                pl.Value = value.ToString();

                                TextInfo ti = new CultureInfo("en", false).TextInfo;
                                pl.Captions.Add(new Translation("ENG", SourceHelper.Replace(ti.ToTitleCase(field.PresentationList.PredefinedString(j)))));

                                if (companySetting.IncludeFra)
                                {
                                    ti = new CultureInfo("fr", false).TextInfo;
                                    pl.Captions.Add(new Translation("FRA", SourceHelper.FR_Replace(ti.ToTitleCase(fieldFra.PresentationList.PredefinedString(j)))));
                                }

                                if (companySetting.IncludeEsn)
                                {
                                    ti = new CultureInfo("es", false).TextInfo;
                                    pl.Captions.Add(new Translation("ESN", SourceHelper.ES_Replace(ti.ToTitleCase(fieldEsn.PresentationList.PredefinedString(j)))));
                                }

                                if (companySetting.IncludeChn)
                                {
                                    pl.Captions.Add(new Translation("CHN", fieldChn.PresentationList.PredefinedString(j)));
                                }

                                if (companySetting.IncludeCht)
                                {
                                    pl.Captions.Add(new Translation("CHT", fieldCht.PresentationList.PredefinedString(j)));
                                }
                            }
                            sourceColumn.PresentationList.Add(pl);
                        }

                        //if (sourceColumn.ViewID == "AR0032" && sourceColumn.Field == "TEXTTRX")
                        //{
                        //    sourceColumn.PresentationList.Add(InterestPresentationList[0]);
                        //}

                        // If there's override presentation list, override it
                        //if (r.OverridePrList != "")
                        //{
                        //    OverridePresentationList op = new OverridePresentationList();
                        //    if (OverridePresentationList.Exists(cp => (cp.Type == r.OverridePrList)))
                        //        op = OverridePresentationList.First(cp => (cp.Type == r.OverridePrList));
                        //    sourceColumn.PresentationList = op.PresentationList;
                        //}

                    }
                    #endregion
                    #endregion

                    //inquiryConfigurationRec.Fields.Add(sourceColumn);
                    ColumnFields.Add(sourceColumn);
                }

                #endregion

                #region CloseSession
                view.Dispose();
                dbLink.Dispose();
                session.Dispose();

                if (companySetting.IncludeFra)
                {
                    viewFra?.Dispose();
                    dbLinkFra?.Dispose();
                    sessionFra?.Dispose();
                }

                if (companySetting.IncludeEsn)
                {
                    viewEsn?.Dispose();
                    dbLinkEsn?.Dispose();
                    sessionEsn?.Dispose();
                }

                if (companySetting.IncludeChn)
                {
                    viewChn?.Dispose();
                    dbLinkChn?.Dispose();
                    sessionChn?.Dispose();
                }

                if (companySetting.IncludeCht)
                {
                    viewCht?.Dispose();
                    dbLinkCht?.Dispose();
                    sessionCht?.Dispose();
                }
                #endregion
            }
            catch (Exception evt)
            {
                LogLine(evt.Message);
                tranRec = new LogRecord(start, "Fail", string.Format("{0} {1}", TestTitle, today), string.Format("Test Case File: {0} ", cr.ViewName), evt.Message);
                _ProcessLogs.Add(tranRec);
            }
            #endregion

            List<FieldDefinition> ColumnFields_Final = new List<FieldDefinition>();

            // Re-organize the ColumnFields to following the field sequence of the Sage300 View Configuration Column Setting
            foreach (ConfigurationColumnSettingDefinition r in ConfigurationColumnList)
            {

                if (r.IsDummy == "Y")
                {
                    #region DummyField
                    var f = new FieldDefinition
                    {
                        FieldIndex = 0,
                        Field = r.Field,
                        FieldAlias = r.FieldAlias,
                        TableName = r.TableName,
                        DataTypeEnumeration = 0,
                        DataType = "",
                        ViewID = "",
                        ViewName = "",
                        Precision = 0,
                        IsSQLColumnInView = false,
                        ColumnNameInView = "",
                        IsFilterable = false,
                        IsDummy = true,
                        IsSortable = false,
                        SortFieldsStr = "",
                        IsDrilldown = true,
                        IsGroupable = false,
                        IsAggregate = false,
                        DrilldownURL = r.DrilldownUrl,
                        Aggregates = r.Aggregates,
                        Included = true
                    };
                    f.Captions = r.Captions;
                    if (r.DrilldownUrl == null)
                        f.IsDrilldown = false;
                    ColumnFields_Final.Add(f);
                    #endregion
                }
                else
                {
                    FieldDefinition f = null;
                    if (ColumnFields.Exists(c => c.Field == r.Field))
                    {
                        f = ColumnFields.First(c => c.Field == r.Field);
                        ColumnFields_Final.Add(f);
                    }
                }
            }

            using (var file = new StreamWriter(Path.Combine(cr.OutputPath, "View", string.Format("{0}.json", cr.ViewID)), false))
            {
                //file.Write(JsonConvert.SerializeObject(ColumnFields, Formatting.Indented));
                file.Write(JsonConvert.SerializeObject(ColumnFields_Final, Formatting.Indented));
            }

            // export test run log
            using (var file = new StreamWriter(Path.Combine(Path.Combine(cr.OutputPath, "View"), string.Format(@"{0}.json", string.Format("ProcessLog-{0}.json", today))), false))
            {
                file.Write(JsonConvert.SerializeObject(_ProcessLogs, Formatting.Indented));
            }

            LogMethodEnd(methodName);
        }

        /// <summary>
        /// Open an Accpac View
        /// </summary>
        /// <param name="company">The company name</param>
        /// <param name="lang">The language enumeration</param>
        /// <param name="viewId">The viewId to open</param>
        /// <param name="session">The Accpac Session (output)</param>
        /// <param name="dbLink">The DBLink object (output)</param>
        /// <param name="view">The ACCPAC View (output)</param>
        public static void OpenAccpacView(Company company, 
                                          LanguageEnum lang, 
                                          string viewId,
                                          out Session session, 
                                          out DBLink dbLink, 
                                          out AccpacView view)
        {
            string version = company.Version;
            string companyName = company.CompanyName;

            bool proceed = true;
            string username = company.Username;
            string password = company.Password;

            switch (lang)
            {
                case LanguageEnum.FRA:
                    username = company.UsernameFra;
                    password = company.PasswordFra;
                    proceed = company.IncludeFra;
                    break;

                case LanguageEnum.ESN:
                    username = company.UsernameEsn;
                    password = company.PasswordEsn;
                    proceed = company.IncludeEsn;
                    break;

                case LanguageEnum.CHT:
                    username = company.UsernameCht;
                    password = company.PasswordCht;
                    proceed = company.IncludeCht;
                    break;

                case LanguageEnum.CHN:
                    username = company.UsernameChn;
                    password = company.PasswordChn;
                    proceed = company.IncludeChn;
                    break;
            }

            if (proceed)
            {
                session = new Session();
                session.CreateSession(null, "WX", "WX1000", version, username, password, companyName, DateTime.UtcNow);
                dbLink = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);
                view = dbLink.OpenView(viewId);
            }
            else
            {
                session = null;
                dbLink = null;
                view = null;
            }
        }

        /// <summary>
        /// Generate Sage 300 Inquiry Configuration and Template JSON files
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="ConfigurationColumnList"></param>
        /// 


        /// <summary>
        /// Generate Sage 300 Inquiry Configuration and Template JSON files
        /// </summary>
        /// <param name="companySetting">The company settings object</param>
        /// <param name="cr">The InquiryConfigurationDefinition object</param>
        /// <param name="DSViewList"></param>
        /// <param name="TemplateTranslationList"></param>
        /// <param name="DatasourceList"></param>
        public static void GenerateInquiryConfigurationAndTemplate(Company companySetting, 
                                                                   InquiryConfigurationDefinition cr, 
                                                                   List<InquiryConfigurationDefinition> DSViewList, 
                                                                   List<InquiryConfigurationDefinition> TemplateTranslationList, 
                                                                   List<InquiryConfigurationDefinition> DatasourceList)
        {
            const string methodName = "GenerateInquiryConfigurationAndTemplate";
            LogMethodStart(methodName);
            LogLine(string.Format("Start Processing {0}: ", cr.Name));

            string today = DateTime.Now.ToString("yyyyMMdd");

            DateTime start = DateTime.Now;

            // Find the template translation record from template translation records TemplateTranslation
            InquiryConfigurationDefinition tr = null;
            if (TemplateTranslationList.Exists(c => c.TemplateId == cr.TemplateId))
                tr = TemplateTranslationList.First(c => c.TemplateId == cr.TemplateId);

            // Find the datasource record from datasource records DatasourceList
            InquiryConfigurationDefinition dr = null;
            if (DatasourceList.Exists(c => c.DatasourceId == cr.DatasourceId))
                dr = DatasourceList.First(c => c.DatasourceId == cr.DatasourceId);

            #region SetInquiryConfigurationRec
            InquiryConfiguration inquiryConfigurationRec = new InquiryConfiguration
            {
                GeneratedMessage = "This file was generated by a tool.",
                GeneratedWarning = "Changes to this file may cause incorrect behavior and will be lost if the file is regenerated.",
                DatasourceId = cr.DatasourceId,
                FileName = string.Format("{0}-Datasource.json", dr.Name),
                Name = dr.Name,
                Module = cr.Module,
                ViewID = dr.ViewID,
                ViewName = dr.ViewName,
                SQL = dr.SQL,
                LicenseCheck = cr.LicenceCheck
            };

            InquiryTemplate inquiryTemplateRec = new InquiryTemplate
            {
                GeneratedMessage = "This file was generated by a tool.",
                GeneratedWarning = "Changes to this file may cause incorrect behavior and will be lost if the file is regenerated.",
                TemplateId = tr.TemplateId,
                DatasourceId = cr.DatasourceId,
                FileName = string.Format("{0}-Template-{1}.json", dr.Name, tr.Name.Replace(" ", string.Empty)),
                WhereClause = cr.WhereClause,
                SortFields = cr.SortFields,
                SortFieldsStr = cr.SortFieldsStr,
                ShowAggregates = cr.ShowAggregates
            };

            inquiryTemplateRec.Name.Add(new Translation("ENG", tr.Name));
            inquiryTemplateRec.Description.Add(new Translation("ENG", tr.Description));
            inquiryConfigurationRec.Description.Add(new Translation("ENG", dr.Description));

            if (companySetting.IncludeFra)
            {
                inquiryTemplateRec.Name.Add(new Translation("FRA", tr.NameFra));
                inquiryTemplateRec.Description.Add(new Translation("FRA", tr.DescriptionFra));
                inquiryConfigurationRec.Description.Add(new Translation("FRA", dr.DescriptionFra));
            }

            if (companySetting.IncludeEsn)
            {
                inquiryTemplateRec.Name.Add(new Translation("ESN", tr.NameEsn));
                inquiryTemplateRec.Description.Add(new Translation("ESN", tr.DescriptionEsn));
                inquiryConfigurationRec.Description.Add(new Translation("ESN", dr.DescriptionEsn));
            }

            if (companySetting.IncludeChn)
            {
                inquiryTemplateRec.Name.Add(new Translation("CHN", tr.NameChn));
                inquiryTemplateRec.Description.Add(new Translation("CHN", tr.DescriptionChn));
                inquiryConfigurationRec.Description.Add(new Translation("CHN", dr.DescriptionChn));
            }

            if (companySetting.IncludeCht)
            {
                inquiryTemplateRec.Name.Add(new Translation("CHT", tr.NameCht));
                inquiryTemplateRec.Description.Add(new Translation("CHT", tr.DescriptionCht));
                inquiryConfigurationRec.Description.Add(new Translation("CHT", dr.DescriptionCht));
            }
            #endregion

            #region SetSecurityResource
            string[] srIDs = dr.SecurityResource.Split(',');
            string[] srNames = dr.SecurityResourceName.Split(',');

            for (int i = 0; i < srIDs.Count(); i++)
            {
                SecurityResource sr = new SecurityResource(srIDs[i], srNames[i]);
                inquiryConfigurationRec.SecurityResource.Add(sr);
            }

            #endregion

            //string[] viewIDs = cr.ViewID.Split(',');
            //List<string> viewNames = new List<string>();
            //List<string> displayColumnsList = new List<string>();

            #region ProcessView
            foreach (InquiryConfigurationDefinition dsView in DSViewList)
            {
                // only process the list of views linked to the inquiry configuration file using DatasourceId
                if (cr.DatasourceId != dsView.DatasourceId)
                    continue;

                // only process the list of views included in the ViewID of the inquiry configuration file
                //if (!viewIDs.Contains(dsView.ViewID))
                //    continue;

                //viewNames.Add(dsView.ViewName);
                var viewColumns = new List<FieldDefinition>();
                var filePath = Path.Combine(cr.OutputPath, "View", string.Format("{0}.json", dsView.ViewID));
                if (File.Exists(filePath))
                {
                    viewColumns = JsonConvert.DeserializeObject<List<FieldDefinition>>(File.ReadAllText(filePath));
                }
                #region ProcessDisplayFields
                if (dsView.SelectedList == "ALL")
                {
                    inquiryConfigurationRec.Fields.AddRange(viewColumns);
                }
                else
                {
                    // only include the display columns of the Sage 300 View into the Inquiry Configuration JSON file
                    if (dsView.SelectedList != string.Empty)
                    {
                        string[] columns = dsView.SelectedList.Split(',');

                        foreach (string s in columns)
                        {
                            if (viewColumns.Exists(c => c.Field == s))
                            {
                                inquiryConfigurationRec.Fields.Add(viewColumns.First(c => c.Field == s));
                            }
                        }
                    }
                }
                #endregion

            }

            #endregion

            #region resortDisplayFields
            if (cr.DisplayOrderList != string.Empty && cr.DisplayOrderList != null)
            {
                List<FieldDefinition> sortedFields = new List<FieldDefinition>();
                string[] sortedcolumns = cr.DisplayOrderList.Split(',');
                foreach (string s in sortedcolumns)
                {
                    if (inquiryConfigurationRec.Fields.Exists(c => c.Field == s))
                        sortedFields.Add(inquiryConfigurationRec.Fields.First(c => c.Field == s));
                    else if (inquiryConfigurationRec.Fields.Exists(c => c.FieldAlias == s))
                        sortedFields.Add(inquiryConfigurationRec.Fields.First(c => c.FieldAlias == s));
                    else
                    {
                        // handle special case e.g. the display order list contains dummy field
                        string[] ss = s.Split(' ');
                        if (ss.Count() == 3)
                        {
                            if (inquiryConfigurationRec.Fields.Exists(c => c.Field == ss[2]))
                                sortedFields.Add(inquiryConfigurationRec.Fields.First(c => c.Field == ss[2]));
                            else if (inquiryConfigurationRec.Fields.Exists(c => c.FieldAlias == ss[2]))
                                sortedFields.Add(inquiryConfigurationRec.Fields.First(c => c.FieldAlias == ss[2]));
                        }

                    }

                }
                inquiryConfigurationRec.Fields = sortedFields;
            }
            #endregion

            List<string> selectedColumnsList = new List<string>();
            foreach (FieldDefinition f in inquiryConfigurationRec.Fields)
            {
                //if (f.Included == false)
                //    continue;

                if (dr.SkipTableName == "Y")
                {
                    selectedColumnsList.Add((f.FieldAlias == string.Empty) ? f.Field : f.FieldAlias);
                }
                else
                {
                    if (f.FieldAlias == string.Empty)
                        selectedColumnsList.Add(string.Format("{0}.[{1}]", f.TableName, f.Field));
                    else
                        selectedColumnsList.Add(string.Format("{0}.[{1}] as [{2}]", f.TableName, f.Field, f.FieldAlias));
                }
            }

            inquiryTemplateRec.DisplayFieldsStr = cr.DisplayList;
            inquiryTemplateRec.SelectFieldsStr = string.Join(",", selectedColumnsList.ToArray()).Trim();

            if (dr.SkipTableName == "Y" && cr.DisplayOrderList != inquiryTemplateRec.SelectFieldsStr)
                inquiryTemplateRec.SelectFieldsStr = cr.DisplayOrderList;

            using (var file = new StreamWriter(Path.Combine(cr.OutputPath, string.Format(@"{0}", inquiryConfigurationRec.FileName.Replace("/", ""))), false))
            {
                file.Write(JsonConvert.SerializeObject(inquiryConfigurationRec, Formatting.Indented));
            }

            // export InquiryTemplate.JSON
            using (var file = new StreamWriter(Path.Combine(cr.OutputPath, string.Format(@"{0}", inquiryTemplateRec.FileName.Replace("/", ""))), false))
            {
                file.Write(JsonConvert.SerializeObject(inquiryTemplateRec, Formatting.Indented));
            }

            LogMethodEnd(methodName);
        }

        public static void GenerateDBScript(string Option, 
                                            string SQLScriptName, 
                                            string OutputPath, 
                                            List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, 
                                            List<InquiryConfigurationDefinition> DatasourceList, 
                                            string actionType)
        {
            const string methodName = "GenerateDBScript";
            LogMethodStart(methodName);

            var fileName = string.Empty;
            var sqlLine = string.Empty;
            var filePath = string.Empty;
            var tempFileName = string.Empty;
            var tempFilePath = string.Empty;

            //fileName = $"{actionType}_Inquiry_DataSource-{SQLScriptName}.sql";
            fileName = string.Format("{0}_Inquiry_DataSource-{1}.sql", actionType, SQLScriptName);
            filePath = Path.Combine(OutputPath, fileName);
            LogLine(filePath);

            using (var file = new StreamWriter(filePath, false))
            {
                file.WriteLine(Constants.SQLCommentLineSageCopyright);
                file.WriteLine(string.Format(@"-- {0}", fileName));
                file.WriteLine("");

                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition ds in DatasourceList)
                {
                    var dsID = ds.DatasourceId;
                    var dsName = ds.Name;

                    if ((dsName != null && dsName.Length > 0) && 
                        (dsID != null && dsID.Length > 0))
                    {
                        dsName = dsName.Replace("/", "");

                        if (actionType == "Create")
                        {
                            var dsModuleID = ds.Module;

                            sqlLine = String.Format(@"INSERT INTO [dbo].[InquiryDataSource] ([DataSourceId], [Name], [DataSource], [Module], [App]) VALUES ('{0}', N'{1}', 0x01, '{2}', '{3}');",
                                                dsID, dsName, dsModuleID, Option);
                            file.WriteLine(sqlLine);
                            LogLine(sqlLine);

                            file.WriteLine("");
                        }

                        if (actionType == "Create" || actionType == "Update")
                        {
                            tempFileName = string.Format(@"{0}-Datasource.json", dsName);
                            tempFilePath = Path.Combine(OutputPath, tempFileName);
                            sqlLine = String.Format(@"UPDATE InquiryDataSource SET DataSource = BulkColumn FROM OPENROWSET(BULK '{0}', SINGLE_BLOB) AS DATA WHERE [DataSourceId] = '{1}';",
                                                    tempFilePath,
                                                    dsID);
                            file.WriteLine(sqlLine);
                            LogLine(sqlLine);

                            file.WriteLine("");
                        }

                        if (actionType == "Delete")
                        {
                            tempFileName = string.Format(@"{0}-Datasource.json", dsName);
                            tempFilePath = Path.Combine(OutputPath, tempFileName);
                            sqlLine = String.Format(@"DELETE FROM InquiryDataSource WHERE [DataSourceId] = '{1}';",
                                                    tempFilePath,
                                                    dsID);
                            file.WriteLine(sqlLine);
                            LogLine(sqlLine);

                            file.WriteLine("");
                        }
                    }
                    else
                    {
                        LogLine(string.Format("Error in {0}", methodName));
                        LogLine("   td.DatasourceName is null and/or empty.");
                        LogLine("   td.Name is null and/or empty.");
                    }
                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");
                file.WriteLine();

                file.WriteLine(@"");
                file.WriteLine(@"BEGIN");

                foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
                {
                    var tdTemplateId = td.TemplateId;
                    var tdDatasourceId = td.DatasourceId;
                    var tdDatasourceName = td.DatasourceName;
                    var tdName = td.Name;
                    var tdModule = td.Module;

                    if (actionType == "Create")
                    {
                        sqlLine = String.Format(@"INSERT INTO [dbo].[InquiryTemplate] ([TemplateId], [DataSourceId], [Name], [UserId], [Module], [Type], [Template], [DateModified]) VALUES ('{0}', '{1}', N'{2}', null, '{3}', 'Template', 0x01, null);",
                                                tdTemplateId,
                                                tdDatasourceId,
                                                tdName,
                                                tdModule);
                        file.WriteLine(sqlLine);
                        LogLine(sqlLine);

                        file.WriteLine("");
                    }
                    if (actionType == "Create" || actionType == "Update")
                    {
                        if ((tdDatasourceName != null && tdDatasourceName.Length > 0) && 
                            (tdName != null & tdName.Length > 0))
                        {
                            tdDatasourceName = tdDatasourceName.Replace("/", "");
                            tdName = tdName.Replace(" ", string.Empty).Replace("/", "");

                            tempFileName = string.Format("{0}-Template-{1}.json", tdDatasourceName, tdName);
                            tempFilePath = Path.Combine(OutputPath, tempFileName);
                            sqlLine = String.Format(@"UPDATE InquiryTemplate SET Template = BulkColumn FROM OPENROWSET(BULK '{0}', SINGLE_BLOB) AS DATA WHERE [TemplateId] = '{1}';",
                                                    tempFilePath,
                                                    tdTemplateId);
                            file.WriteLine(sqlLine);
                            LogLine(sqlLine);

                            file.WriteLine("");
                        }
                        else
                        {
                            LogLine(string.Format("Error in {0}", methodName));
                            LogLine("   td.DatasourceName is null and/or empty.");
                            LogLine("   td.Name is null and/or empty.");
                        }
                    }
                    if (actionType == "Delete")
                    {
                        sqlLine = String.Format(@"DELETE InquiryTemplate WHERE [TemplateId] = '{0}';", 
                                                tdTemplateId);
                        file.WriteLine(sqlLine);
                        LogLine(sqlLine);

                        file.WriteLine("");
                    }
                }

                file.WriteLine(@"END");
                file.WriteLine(@"GO");

                LogMethodEnd(methodName);
            }
        }

        public static void GenerateDeleteDBScript(string Option, 
                                                  string SQLScriptName, 
                                                  string OutputPath, 
                                                  List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, 
                                                  List<InquiryConfigurationDefinition> DatasourceList)
        {
            const string methodName = "GenerateDeleteDBScript";
            LogMethodStart(methodName);

            var sqlLine = string.Empty;
            var fileName = string.Format("Delete_Inquiry_DataSource-{0}.sql", SQLScriptName);
            var filePath = Path.Combine(OutputPath, fileName);
            var tempFileName = string.Empty;
            var tempFilePath = string.Empty;

            using (var file = new StreamWriter(Path.Combine(OutputPath, filePath), false))
            {
                file.WriteLine(Constants.SQLCommentLineSageCopyright);
                file.WriteLine(string.Format(@"-- {0}", fileName));
                file.WriteLine("");

                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
                {
                    sqlLine = String.Format(@"DELETE InquiryTemplate WHERE [TemplateId] = '{0}';", 
                                            td.TemplateId);
                    file.WriteLine(sqlLine);
                    LogLine(sqlLine);

                    file.WriteLine("");
                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");
                file.WriteLine();

                file.WriteLine(@"");
                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition ds in DatasourceList)
                {
                    sqlLine = String.Format(@"DELETE FROM InquiryDataSource WHERE [DataSourceId] = '{0}';", ds.DatasourceId);

                    file.WriteLine(sqlLine);
                    LogLine(sqlLine);

                    file.WriteLine("");

                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");
            }

            LogMethodEnd(methodName);
        }

        public static void GenerateSQLScript(string Option, 
                                             string SQLScriptName, 
                                             string OutputPath, 
                                             List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, 
                                             List<InquiryConfigurationDefinition> DatasourceList)
        {
            const string methodName = "GenerateSQLScript";
            LogMethodStart(methodName);

            foreach (InquiryConfigurationDefinition ds in DatasourceList)
            {
                var dsName = ds.Name;
                if (dsName != null && dsName.Length > 0)
                {
                    dsName = dsName.Replace("/", "");

                    // Input file and contents
                    var inputFileName = string.Format(@"{0}-Datasource.json", dsName);
                    var inputFilePath = Path.Combine(OutputPath, inputFileName);
                    var content = File.ReadAllText(inputFilePath);
                    content = content.Replace("'", "''");

                    // Output file
                    var outputFileName = string.Format("Sage300-{0}-InsertDataSource-{1}.sql", SQLScriptName, dsName);
                    var outputFilePath = Path.Combine(OutputPath, outputFileName.Replace("/", ""));

                    using (var sqlscript = new StreamWriter(outputFilePath, false))
                    {
                        sqlscript.WriteLine(Constants.SQLCommentLineSageCopyright);
                        sqlscript.WriteLine(@"-- {0}", outputFileName);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("SET ANSI_NULLS ON");
                        sqlscript.WriteLine("SET QUOTED_IDENTIFIER ON");
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("IF NOT EXISTS (SELECT * FROM [dbo].[InquiryDataSource] WHERE [DataSourceId] = '{0}')", ds.DatasourceId);
                        sqlscript.WriteLine("BEGIN");
                        sqlscript.WriteLine(@"INSERT INTO [dbo].[InquiryDataSource] ([DataSourceId], [Name], [DataSource], [Module], [App]) VALUES (");
                        sqlscript.WriteLine(@"'{0}', N'{1}', ", ds.DatasourceId, ds.Name);
                        sqlscript.WriteLine(@"CAST(CAST (N'{0}' AS nvarchar(MAX)) AS varbinary(MAX)),", content);
                        sqlscript.WriteLine("'{0}', '{1}')", ds.Module, Option);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("END");
                        sqlscript.WriteLine("ELSE");
                        sqlscript.WriteLine("BEGIN");
                        sqlscript.WriteLine(@"UPDATE [dbo].[InquiryDataSource] set [DataSource] = ");
                        sqlscript.WriteLine(@"CAST(CAST (N'{0}' AS nvarchar(MAX)) AS varbinary(MAX))", content);
                        sqlscript.WriteLine("where [DataSourceId] = '{0}'", ds.DatasourceId);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("END");

                        sqlscript.WriteLine("");
                        sqlscript.WriteLine(@"GO");
                    }
                }
                else
                {
                    LogLine(string.Format("Error in {0}", methodName));
                    LogLine("   ds.Name is null and/or empty.");
                }
            }

            foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
            {
                var tdName = td.Name;
                var tdDatasourceName = td.DatasourceName;

                if ((tdName != null && tdName.Length > 0) && 
                    (tdDatasourceName != null && tdDatasourceName.Length > 0))
                {
                    // Input file and contents
                    tdName = tdName.Replace(" ", string.Empty).Replace("/", "");
                    tdDatasourceName = tdDatasourceName.Replace("/", "");

                    var inputFileName = string.Format("{0}-Template-{1}.json", tdDatasourceName, tdName);
                    var inputFilePath = Path.Combine(OutputPath, inputFileName);
                    var content = File.ReadAllText(inputFilePath);
                    content = content.Replace("'", "''");

                    // Output file
                    var outputFileName = string.Format("Sage300-{0}-InsertTemplate-{1}.sql", SQLScriptName, tdName);
                    var outputFilePath = Path.Combine(OutputPath, outputFileName.Replace("/", ""));

                    using (var sqlscript = new StreamWriter(outputFilePath, false))
                    {
                        sqlscript.WriteLine(Constants.SQLCommentLineSageCopyright);
                        sqlscript.WriteLine(@"-- {0}", outputFileName);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("SET ANSI_NULLS ON");
                        sqlscript.WriteLine("SET QUOTED_IDENTIFIER ON");
                        sqlscript.WriteLine("");

                        sqlscript.WriteLine("IF NOT EXISTS (SELECT * FROM [dbo].[InquiryTemplate] WHERE [TemplateId] = '{0}')", td.TemplateId);
                        sqlscript.WriteLine("BEGIN");
                        sqlscript.WriteLine(@"INSERT INTO [dbo].[InquiryTemplate] ([TemplateId], [DataSourceId], [Name], [UserId], [Module], [Type], [Template], [DateModified]) VALUES (");
                        sqlscript.WriteLine(@"'{0}', '{1}', N'{2}', null, '{3}', 'Template', ", td.TemplateId, td.DatasourceId, td.Name, td.Module);
                        sqlscript.WriteLine(@"CAST(CAST (N'{0}' AS nvarchar(MAX)) AS varbinary(MAX)),null)", content);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("END");
                        sqlscript.WriteLine("ELSE");
                        sqlscript.WriteLine("BEGIN");
                        sqlscript.WriteLine(@"UPDATE [dbo].[InquiryTemplate] set [Template] = ");
                        sqlscript.WriteLine(@"CAST(CAST (N'{0}' AS nvarchar(MAX)) AS varbinary(MAX))", content);
                        sqlscript.WriteLine("where [TemplateId] = '{0}'", td.TemplateId);
                        sqlscript.WriteLine("");
                        sqlscript.WriteLine("END");
                        sqlscript.WriteLine(@"GO");
                    }
                }
                else
                {
                    LogLine(string.Format("Error in {0}", methodName));
                    LogLine("   td.Name is null and/or empty.");
                    LogLine("   td.DatasourceName is null and/or empty.");
                }
            }

            LogMethodEnd(methodName);
        }

        /// <summary>
        /// Wrapper method to call LogLine method in parent
        /// </summary>
        /// <param name="msg">The message text to log</param>
        private static void LogLine(string msg)
        {
            Parent?.RunOnUIThread(() => { Parent?.LogLine(msg); });
        }

        /// <summary>
        /// Log the start of a method
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        private static void LogMethodStart(string methodName)
        {
            LogLine(String.Format("Start - {0}()", methodName));
        }

        /// <summary>
        /// Log the end of a method
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        private static void LogMethodEnd(string methodName)
        {
            LogLine(String.Format("End - {0}()", methodName));
        }
    }
}
