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
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    public static class ReadConfigurationSetting
    {
        public static LogRecord ReadInquiryConfigurationSetting(string InquiryConfigurationSettingFileName, string Suite, ref List<InquiryConfigurationDefinition> InquiryConfigurationList)
        {
            DateTime start = DateTime.Now;
            string status = "Success";
            string error = "";

            try
            {
                ExcelPackage excelFile = EPPlusExcel.openExcel(InquiryConfigurationSettingFileName);

                var worksheet = excelFile.Workbook.Worksheets[Suite];

                #region readdata
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    InquiryConfigurationDefinition ss = new InquiryConfigurationDefinition();

                    //ss.DatasourceId = Guid.NewGuid().ToString();
                    ss.DatasourceId = "";
                    ss.TemplateId = "";
                    ss.ConfigSettingFile = InquiryConfigurationSettingFileName;
                    ss.ShowAggregates = "";
                    ss.LicenceCheck = "";
                    ss.SkipTableName = "N";
                    ss.SQL = "";
                    ss.DisplayOrderList = "";

                    string sortFields = "";
                    string sortOrder = "";

                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Included" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Included = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DatasourceID" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DatasourceId = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "TemplateID" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.TemplateId = EPPlusExcel.excelCellValue(worksheet, row, col);

                        #region Template
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Module" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Module = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DatasourceName" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DatasourceName = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DisplayList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DisplayList = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DisplayOrder" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DisplayOrderList = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SecurityResource" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SecurityResource = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SecurityResourceName" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SecurityResourceName = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "WhereClause" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.WhereClause = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "OrderByClause" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.OrderByClause = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SortFields" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            sortFields = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SortOrders" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            sortOrder = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "ShowAggregates")
                            ss.ShowAggregates = EPPlusExcel.excelCellValue(worksheet, row, col);
                        //if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "ProcessLog")
                        //    ss.ProcessLog = Path.Combine(MasterPath, EPPlusExcel.excelCellValue(worksheet, row, col));
                        #endregion

                        #region Datasource
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SelectedList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SelectedList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SQL" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SQL = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SkipTableName" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SkipTableName = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "ViewID" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.ViewID = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "ViewName" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.ViewName = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LicenceCheck" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.LicenceCheck = EPPlusExcel.excelCellValue(worksheet, row, col);

                        #endregion

                        #region Translation
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Name" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Name = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "NameFra" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.NameFra = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "NameEsn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.NameEsn = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "NameChn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.NameChn = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "NameCht" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.NameCht = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Description" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Description = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DescriptionFra" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DescriptionFra = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DescriptionEsn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DescriptionEsn = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DescriptionChn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DescriptionChn = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DescriptionCht" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DescriptionCht = EPPlusExcel.excelCellValue(worksheet, row, col);
                        #endregion
                    }

                    string[] fields = sortFields.Split(',');
                    string[] orders = sortOrder.Split(',');
                    List<string> sortFieldsStr = new List<string>();

                    for (int j = 0; j < fields.Count(); j++)
                    {
                        ss.SortFields.Add(new SortField(fields[j], orders[j]));
                        sortFieldsStr.Add(string.Format("{0} {1}", fields[j], orders[j]));
                    }
                    ss.SortFieldsStr = string.Join(",", sortFieldsStr.ToArray()).Trim();
                    InquiryConfigurationList.Add(ss);

                }
                #endregion

                excelFile.Dispose();
            }
            catch (Exception evt)
            {
                status = "Fail";
                error = evt.Message;
            }

            LogRecord logrec = new LogRecord(start, status, string.Format("Read Inquiry Configuration Setting Excel for {0}", Suite), InquiryConfigurationSettingFileName, error);

            return logrec;
        }

        public static LogRecord ReadInquiryConfigurationColumnSetting(string InquiryConfigurationColumnSettingFileName, string Suite, ref List<ConfigurationColumnSettingDefinition> ConfigurationColumnList, string controllerParamDefFileName = null)
        {
            DateTime start = DateTime.Now;
            string status = "Success";
            string error = "";

            try
            {
                ExcelPackage excelFile = EPPlusExcel.openExcel(InquiryConfigurationColumnSettingFileName);
                List<string> excelFileSheetNames = new List<string>();

                foreach (var worksheet1 in excelFile.Workbook.Worksheets)
                {
                    excelFileSheetNames.Add(worksheet1.Name);
                }
                if (excelFileSheetNames.FirstOrDefault(s => s == Suite) == null)
                {
                    LogRecord lr = new LogRecord(start, "Skip", string.Format("Read Inquiry Configuration Column Setting Excel for {0}", Suite), InquiryConfigurationColumnSettingFileName, string.Format("{0} column setting is not available", Suite));

                    return lr;
                }

                var worksheet = excelFile.Workbook.Worksheets[Suite];

                var ControllerParamDefList = new List<ControllParamDef>();
                if (File.Exists(controllerParamDefFileName))
                {
                    ControllerParamDefList = JsonConvert.DeserializeObject<List<ControllParamDef>>(File.ReadAllText(controllerParamDefFileName));
                }

                #region readdata
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    ConfigurationColumnSettingDefinition ss = new ConfigurationColumnSettingDefinition
                    {
                        Field = "",
                        Included = "Y",
                        IsDummy = "N",
                        YesNoPresentation = "N",
                        OverridePrList = "",
                        FieldAlias = "",
                        TableName = "",
                        IsFilterable = "Y",
                        IsSortable = "Y",
                        SortFieldsStr = "",
                        IsDrilldown = "N",
                        IsGroupBy = "N",
                        Controller_Type = "",
                        Controller_SrceAppl = "",
                        Controller_SrceApplList = "",
                        Controller_AreaList = "",
                        Controller_ControllerList = "",
                        Controller_TypeList = "",
                        Controller_ActionList = "",
                        ParameterName = "",
                        ParameterField = "",
                        AggregatesStr = "",
                        LicenceCheck = "",
                    };

                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Field")
                            ss.Field = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "IsDummy" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.IsDummy = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Included" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Included = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "YesNoPresentation" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.YesNoPresentation = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "OverridePrList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.OverridePrList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "FieldAlias" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.FieldAlias = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Label" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Captions.Add(new Translation("ENG", EPPlusExcel.excelCellValue(worksheet, row, col)));
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LabelFra" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Captions.Add(new Translation("FRA", EPPlusExcel.excelCellValue(worksheet, row, col)));
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LabelEsn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Captions.Add(new Translation("ESN", EPPlusExcel.excelCellValue(worksheet, row, col)));
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LabelChn" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Captions.Add(new Translation("CHN", EPPlusExcel.excelCellValue(worksheet, row, col)));
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LabelCht" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Captions.Add(new Translation("CHT", EPPlusExcel.excelCellValue(worksheet, row, col)));
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "TableName")
                            ss.TableName = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "IsFilterable" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.IsFilterable = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "IsSortable" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.IsSortable = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "SortFieldsStr" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.SortFieldsStr = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "IsGroupBy" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.IsGroupBy = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Aggregation" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.AggregatesStr = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "LicenceCheck" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.LicenceCheck = EPPlusExcel.excelCellValue(worksheet, row, col);

                        // Drilldown Controller & Parameters
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "IsDrilldown" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.IsDrilldown = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-AreaList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_AreaList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-ControllerList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_ControllerList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-TypeList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_TypeList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-Type" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_Type = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-SrceApplList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_SrceApplList = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-SrceAppl" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_SrceAppl = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-ActionList" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.Controller_ActionList = EPPlusExcel.excelCellValue(worksheet, row, col);

                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-ParamName" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.ParameterName = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "Controller-ParamField" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.ParameterField = EPPlusExcel.excelCellValue(worksheet, row, col);

                        #region ReadDrilldownCondition
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DrilldownCondition-Field" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DrilldownCondition.Field = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DrilldownCondition-Value" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DrilldownCondition.Value = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DrilldownCondition-Operator" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DrilldownCondition.Operator = EPPlusExcel.excelCellValue(worksheet, row, col);
                        #endregion

                        #region ReadDisplayCondition
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DisplayCondition-Field" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DisplayCondition.Field = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DisplayCondition-Value" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DisplayCondition.Value = EPPlusExcel.excelCellValue(worksheet, row, col);
                        if (EPPlusExcel.excelCellValue(worksheet, 1, col) == "DisplayCondition-Operator" && EPPlusExcel.excelCellValue(worksheet, row, col) != "")
                            ss.DisplayCondition.Operator = EPPlusExcel.excelCellValue(worksheet, row, col);
                        #endregion

                    }

                    #region ProcessDrillDownURL
                    //ss.DrilldownUrl = new DrillDownURL();
                    if (ss.IsDrilldown == "Y")
                    {
                        ss.DrilldownUrl = new DrillDownURL();
                        // Process controller only if isDrillDown is set to "Y"
                        ss.DrilldownUrl.TypeField = ss.Controller_Type;
                        ss.DrilldownUrl.SrceApplField = ss.Controller_SrceAppl;
                        string[] areaList = ss.Controller_AreaList.Split(',');
                        string[] controllerList = ss.Controller_ControllerList.Split(',');
                        string[] typeList = ss.Controller_TypeList.Split(',');
                        string[] srceapplList = ss.Controller_SrceApplList.Split(',');
                        string[] actionList = ss.Controller_ActionList.Split(',');

                        List<Param> parameters = new List<Param>();

                        if (ss.ParameterField != string.Empty && ss.ParameterName != string.Empty && ss.ParameterField != null && ss.ParameterName != null)
                        {
                            string[] paramNameList = ss.ParameterName.Split(',');
                            string[] paramFieldList = ss.ParameterField.Split(',');
                            for (int k = 0; k < paramNameList.Count(); k++)
                            {
                                parameters.Add(new Param(paramNameList[k], paramFieldList[k]));
                            }
                        }

                        if (controllerList.Count() > 1 && controllerList.Count() == typeList.Count() && controllerList.Count() == areaList.Count() && controllerList.Count() == actionList.Count())
                        {
                            // more than one controller are defined, and the document type are defined
                            for (int cnt = 0; cnt < controllerList.Count(); cnt++)
                            {
                                ControllerDef c = new ControllerDef(areaList[cnt], controllerList[cnt], actionList[cnt]);

                                // find the controller param list from the Controller Param Def file
                                ControllParamDef cpf = new ControllParamDef();
                                if (ControllerParamDefList.Exists(cp => (cp.Area == areaList[cnt] && cp.Controller == controllerList[cnt])))
                                {
                                    cpf = ControllerParamDefList.First(cp => (cp.Area == areaList[cnt] && cp.Controller == controllerList[cnt]));
                                }
                                
                                // GVG - 20181026
                                // If the Controller Param Def file didn't exist,
                                // we need to do this check.
                                // This will prevent the previously encountered exceptions from being generated.
                                if (cpf.parameters != null)
                                {
                                    var paramNameList = cpf.parameters.Split(',');
                                    foreach (string pn in paramNameList)
                                    {
                                        if (parameters.Exists(p => p.Name == pn))
                                            c.parameters.Add(parameters.First(p => p.Name == pn));
                                    }
                                }

                                //c.parameters = parameters;
                                ControllerTypeMapping ctm = new ControllerTypeMapping(typeList[cnt], c);
                                if (srceapplList.Count() == typeList.Count())
                                {
                                    ctm.SrceAppl = srceapplList[cnt];
                                }

                                ss.DrilldownUrl.ControllerList.Add(ctm);

                                if (ss.DrilldownCondition.Field != "")
                                {
                                    ss.DrilldownUrl.DrilldownCondition = ss.DrilldownCondition;
                                }
                                else
                                {
                                    ss.DrilldownUrl.DrilldownCondition = null;
                                }
                            }
                        }
                        else if (controllerList.Count() == 1)
                        {
                            ControllerDef c = new ControllerDef(areaList[0], controllerList[0], actionList[0]);
                            c.parameters = parameters;
                            ControllerTypeMapping ctm = new ControllerTypeMapping("-1", c);
                            ss.DrilldownUrl.ControllerList.Add(ctm);
                            if (ss.DrilldownCondition.Field != "")
                                ss.DrilldownUrl.DrilldownCondition = ss.DrilldownCondition;
                            else
                                ss.DrilldownUrl.DrilldownCondition = null;
                        }

                    }
                    #endregion

                    ConfigurationColumnList.Add(ss);
                }
                #endregion

                excelFile.Dispose();
            }
            catch (Exception evt)
            {
                status = "Fail";
                error = evt.Message;
            }

            LogRecord logrec = new LogRecord(start, status, string.Format("Read Inquiry Configuration Column Setting Excel for {0}", Suite), InquiryConfigurationColumnSettingFileName, error);

            return logrec;
        }
    }
}
