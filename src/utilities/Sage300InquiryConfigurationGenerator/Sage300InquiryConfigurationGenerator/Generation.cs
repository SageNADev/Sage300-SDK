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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    public static class Generation
    {
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
        /// Process Sage 300 View Column Setting XLSX file and generate Sage 300 View Column Definition JSON file
        /// </summary>
        /// <param name="cr">InquiryConfigurationDefinition</param>
        /// <param name="ConfigurationColumnList">List of Sage 300 View Column Settings</param>
        public static void ProcessView(MainForm parent, Company companySetting, InquiryConfigurationDefinition cr, List<ConfigurationColumnSettingDefinition> ConfigurationColumnList, List<OverridePresentationList> OverridePresentationList)
        {
            // Test Title
            string TestTitle = "Process Sage 300 View";

            List<LogRecord> _ProcessLogs = new List<LogRecord>();
            LogRecord tranRec = null;

            string today = DateTime.Now.ToString("yyyyMMdd");

            parent.LogLine(string.Format("Start Processing {0}: ", cr.ViewID));

            DateTime start = DateTime.Now;

            List<FieldDefinition> ColumnFields = new List<FieldDefinition>();

            #region Process
            try
            {
                parent.LogLine("Open Accpac Session");

                start = DateTime.Now;

                #region OpenSession
                Session session, sessionFra, sessionEsn, sessionChn, sessionCht;
                DBLink dbLink, dbLinkFra, dbLinkEsn, dbLinkChn, dbLinkCht;
                View view, viewFra, viewEsn, viewChn, viewCht;

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
                parent.LogLine(evt.Message);
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

        }

        public static void OpenAccpacView(Company company, 
                                          LanguageEnum lang, 
                                          string viewId,
                                          out Session session, 
                                          out DBLink dbLink, 
                                          out View view)
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
        public static void GenerateInquiryConfigurationAndTemplate(MainForm parent, Company companySetting, InquiryConfigurationDefinition cr, List<InquiryConfigurationDefinition> DSViewList, List<InquiryConfigurationDefinition> TemplateTranslationList, List<InquiryConfigurationDefinition> DatasourceList)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");

            parent.LogLine(string.Format("Start Processing {0}: ", cr.Name));
            
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


        }

        public static void GenerateDBScript(string Option, string SQLScriptName, string OutputPath, List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, List<InquiryConfigurationDefinition> DatasourceList, string Choice)
        {
            var filename = "";

            if (Choice == "Delete")
            {
                filename = Path.Combine(OutputPath, string.Format("Delete_Inquiry_DataSource-{0}.sql", SQLScriptName));
            }
            else if (Choice == "Update")
            {
                filename = Path.Combine(OutputPath, string.Format("Update_Inquiry_Data-{0}.sql", SQLScriptName));
            }
            else if (Choice == "Create")
            {
                filename = Path.Combine(OutputPath, string.Format("Create_Inquiry_Data-{0}.sql", SQLScriptName));
            }

            using (var file = new StreamWriter(Path.Combine(OutputPath, filename), false))
            {
                file.WriteLine(@"-- Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.");
                file.WriteLine(@"-- Create_Inquiry_Schema.sql");
                file.WriteLine("");

                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition ds in DatasourceList)
                {
                    if (Choice == "Create")
                    {
                        file.WriteLine(@"INSERT INTO [dbo].[InquiryDataSource] ([DataSourceId], [Name], [DataSource], [Module], [App]) VALUES ('{0}', N'{1}', 0x01, '{2}', '{3}');",
                            ds.DatasourceId, ds.Name, ds.Module, Option);
                        file.WriteLine("");
                    }
                    if (Choice == "Create" || Choice == "Update")
                    {
                        file.WriteLine(@"UPDATE InquiryDataSource SET DataSource = BulkColumn FROM OPENROWSET(BULK '{0}', SINGLE_BLOB) AS DATA WHERE [DataSourceId] = '{1}';", Path.Combine(OutputPath, string.Format(@"{0}-Datasource.json", ds.Name.Replace("/", ""))), ds.DatasourceId);
                        file.WriteLine("");
                    }
                    if (Choice == "Delete")
                    {
                        file.WriteLine(@"DELETE FROM InquiryDataSource WHERE [DataSourceId] = '{1}';", Path.Combine(OutputPath, string.Format(@"{0}-Datasource.json", ds.Name.Replace("/", ""))), ds.DatasourceId);
                        file.WriteLine("");
                    }

                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");
                file.WriteLine();

                file.WriteLine(@"");
                file.WriteLine(@"BEGIN");

                foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
                {
                    if (Choice == "Create")
                    {
                        file.WriteLine(@"INSERT INTO [dbo].[InquiryTemplate] ([TemplateId], [DataSourceId], [Name], [UserId], [Module], [Type], [Template], [DateModified]) VALUES ('{0}', '{1}', N'{2}', null, '{3}', 'Template', 0x01, null);",
                        td.TemplateId, td.DatasourceId, td.Name, td.Module);
                        file.WriteLine("");
                    }
                    if (Choice == "Create" || Choice == "Update")
                    {
                        file.WriteLine(@"UPDATE InquiryTemplate SET Template = BulkColumn FROM OPENROWSET(BULK '{0}', SINGLE_BLOB) AS DATA WHERE [TemplateId] = '{1}';", Path.Combine(OutputPath, string.Format("{0}-Template-{1}.json", td.DatasourceName.Replace("/", ""), td.Name.Replace(" ", string.Empty).Replace("/", ""))), td.TemplateId);
                        file.WriteLine("");
                    }
                    if (Choice == "Delete")
                    {
                        file.WriteLine(@"DELETE InquiryTemplate WHERE [TemplateId] = '{1}';", Path.Combine(OutputPath, string.Format("{0}-Template-{1}.json", td.DatasourceName.Replace("/", ""), td.Name.Replace(" ", string.Empty).Replace("/", ""))), td.TemplateId);
                        file.WriteLine("");
                    }
                }

                file.WriteLine(@"END");
                file.WriteLine(@"GO");


            }
        }
        public static void GenerateDeleteDBScript(string Option, string SQLScriptName, string OutputPath, List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, List<InquiryConfigurationDefinition> DatasourceList)
        {
            var filename = "";

            filename = Path.Combine(OutputPath, string.Format("Delete_Inquiry_DataSource-{0}.sql", SQLScriptName));

            using (var file = new StreamWriter(Path.Combine(OutputPath, filename), false))
            {
                file.WriteLine(@"-- Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.");
                file.WriteLine(@"-- Create_Inquiry_Schema.sql");
                file.WriteLine("");

                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
                {
                    file.WriteLine(@"DELETE InquiryTemplate WHERE [TemplateId] = '{1}';", Path.Combine(OutputPath, string.Format("{0}-Template-{1}.json", td.DatasourceName.Replace("/", ""), td.Name.Replace(" ", string.Empty).Replace("/", ""))), td.TemplateId);
                    file.WriteLine("");
                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");
                file.WriteLine();

                file.WriteLine(@"");
                file.WriteLine(@"BEGIN");
                foreach (InquiryConfigurationDefinition ds in DatasourceList)
                {
                    file.WriteLine(@"DELETE FROM InquiryDataSource WHERE [DataSourceId] = '{1}';", Path.Combine(OutputPath, string.Format(@"{0}-Datasource.json", ds.Name.Replace("/", ""))), ds.DatasourceId);
                    file.WriteLine("");

                }
                file.WriteLine(@"END");
                file.WriteLine(@"GO");


            }
        }

        public static void GenerateSQLScript(string Option, string SQLScriptName, string OutputPath, List<InquiryConfigurationDefinition> TemplateInquiryConfigurationList, List<InquiryConfigurationDefinition> DatasourceList)
        {

            foreach (InquiryConfigurationDefinition ds in DatasourceList)
            {
                string fname = string.Format("Sage300-{1}-InsertDataSource-{0}.sql", ds.Name, SQLScriptName);
                string content = File.ReadAllText(Path.Combine(OutputPath, string.Format(@"{0}-Datasource.json", ds.Name.Replace("/", ""))));
                content = content.Replace("'", "''");
                using (var sqlscript = new StreamWriter(Path.Combine(OutputPath, fname.Replace("/", "")), false))
                {
                    sqlscript.WriteLine(@"-- Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.");
                    sqlscript.WriteLine(@"-- {0}", fname);
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

            foreach (InquiryConfigurationDefinition td in TemplateInquiryConfigurationList)
            {
                string fname = string.Format("Sage300-{1}-InsertTemplate-{0}.sql", td.Name, SQLScriptName);
                string content = File.ReadAllText(Path.Combine(OutputPath, string.Format("{0}-Template-{1}.json", td.DatasourceName.Replace("/", ""), td.Name.Replace(" ", string.Empty).Replace("/", ""))));
                content = content.Replace("'", "''");
                using (var sqlscript = new StreamWriter(Path.Combine(OutputPath, fname.Replace("/", "")), false))
                {
                    sqlscript.WriteLine(@"-- Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.");
                    sqlscript.WriteLine(@"-- {0}", fname);
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
        }
    }
}
