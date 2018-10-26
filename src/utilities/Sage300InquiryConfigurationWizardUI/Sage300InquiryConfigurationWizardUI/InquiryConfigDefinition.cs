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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    /// <summary>
    /// Enum for Data Types
    /// </summary>
    public enum SourceDataType
    {
        /// <summary> none </summary>
        [EnumValue("none")]
        None = 0,
        /// <summary> double </summary>
        [EnumValue("double")]
        Double = 1,
        /// <summary> long </summary>
        [EnumValue("long")]
        Long = 2,
        /// <summary> string </summary>
        [EnumValue("string")]
        String = 3,
        /// <summary> DateTime </summary>
        [EnumValue("DateTime")]
        DateTime = 4,
        /// <summary> int </summary>
        [EnumValue("int")]
        Integer = 5,
        /// <summary> decimal </summary>
        [EnumValue("decimal")]
        Decimal = 6,
        /// <summary> bool </summary>
        [EnumValue("bool")]
        Boolean = 7,
        /// <summary> TimeSpan </summary>
        [EnumValue("TimeSpan")]
        TimeSpan = 8,
        /// <summary> byte[] </summary>
        [EnumValue("byte[]")]
        Byte = 9,
        /// <summary> enumeration </summary>
        [EnumValue("enumeration")]
        Enumeration = 10
    }

    public static class SourceHelper
    {
        #region Private Constants

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper method that removes and replaces unwanted characters
        /// </summary>
        /// <param name="value">Input string</param>
        /// <returns>Replaced string</returns>
        public static string Replace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Convert to Pascal Case First, but only if there are spaces in value (else it has already been done)
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var pascalCase = value.Contains(" ") ? textInfo.ToTitleCase(value) : value;

            var newString = pascalCase
                .Replace("Add'l", "Additional")
                .Replace("Addt'l", "Additional")
                .Replace("Ret'd", "Returned")
                .Replace("State/Prov.", "StateProvince")
                .Replace("Company/Org.", "CompanyOrganization")
                .Replace("Distrib.", "Distribution")
                .Replace("Insuff.", "Insufficient")
                .Replace("Prepay.", "Prepayment")
                .Replace("Unreal.", "Unrealized")
                .Replace("Alloc.", "Allocated")
                .Replace("Avail.", "Available")
                .Replace("Jrnls.", "Journals")
                .Replace("Quant.", "Quantity")
                .Replace("Reval.", "Revaluation")
                .Replace("Recon.", "Reconcilation")
                .Replace("Sched.", "Schedule")
                .Replace("Trans.", "Transaction")
                .Replace("Acct.", "Account")
                .Replace("Auth.", "Authority")
                .Replace("Calc.", "Calculation")
                .Replace("Curr.", "Currency")
                .Replace("Cust.", "Customer")
                .Replace("Desc.", "Description")
                .Replace("Dest.", "Destination")
                .Replace("Dist.", "Distribution")
                .Replace("Fisc.", "Fiscal")
                .Replace("Func.", "Functional")
                .Replace("incl.", "Included")
                .Replace("Exch.", "Exchange")
                .Replace("excl.", "Excluded")
                .Replace("Info.", "Information")
                .Replace("Lgst.", "Largest")
                .Replace("Larg.", "Largest")
                .Replace("Misc.", "Miscellaneous")
                .Replace("Orig.", "Original")
                .Replace("Prov.", "Provisional")
                .Replace("Rcpt.", "Receipt")
                .Replace("Rtng.", "Retainage")
                .Replace("Srce.", "Source")
                .Replace("Stats.", "Statistic")
                .Replace("Vend.", "Vendor")
                .Replace("Warr.", "Warranty")
                .Replace("Adj.", "Adjustment")
                .Replace("Amt.", "Amount")
                .Replace("Bal.", "Balance")
                .Replace("Chk.", "Check")
                .Replace("Clr.", "Clearing")
                .Replace("Cur.", "Currency")
                .Replace("Dep.", "Deposit")
                .Replace("Doc.", "Document")
                .Replace("Exp.", "Expense")
                .Replace("Fwd.", "Forward")
                .Replace("diff.", "Difference")
                .Replace("Inc.", "Include")
                .Replace("Inv.", "Invoice")
                .Replace("Int.", "Interest")
                .Replace("Opt.", "Optional")
                .Replace("Ord.", "Ordered")
                .Replace("Per.", "Period")
                .Replace("Qty.", "Quantity")
                .Replace("Rtg.", "Retainage")
                .Replace("Src.", "Source")
                .Replace("Sep.", "Separately")
                .Replace("Seq.", "Sequence")
                .Replace("Sys.", "System")
                .Replace("Cr.", "Credit")
                .Replace("Dr.", "Debit")
                .Replace("Ex.", "Exchange")
                .Replace("No.", "Number")
                .Replace("Pd.", "Paid")
                .Replace("Yr.", "Year")
                .Replace("C.C.", "CreditCard")
                .Replace("w/ ", "With")
                .Replace("w/o ", "Without")
                .Replace("->", "To")
                //.Replace(" ", "")
                //.Replace("/", "")
                .Replace(@"\", "")
                .Replace("*", "")
                .Replace("#", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace("'", "")
                .Replace(":", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(",", "")
                .Replace("&", "");

            if (newString.Length > 0)
            {
                var num = newString.ToArray()[0];
                if (char.IsNumber(num))
                {
                    newString = "Num" + newString;
                }

            }

            if (string.CompareOrdinal(newString, "OptionalFields") == 0)
            {
                return "NumberOfOptionalFields";
            }

            return newString;
        }

        public static string FR_Replace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Convert to Pascal Case First, but only if there are spaces in value (else it has already been done)
            //var textInfo = new CultureInfo("fr", false).TextInfo;
            //var pascalCase = value.Contains(" ") ? textInfo.ToTitleCase(value) : value;

            var newString = value
                .Replace("Cl?Ture Jou", "ClôTure Jour)")
                .Replace("Cl?ture Jou", "ClôTure Jour)")
                .Replace("cl?ture Jou", "ClôTure Jour)")
                .Replace("cl?Ture Jou", "ClôTure Jour)")
                .Replace("Imp?T", "Impôts")
                .Replace("Imp?t", "Impôts")
                .Replace("imp?t", "Impôts")
                .Replace("imp?T", "Impôts")
                .Replace("Re?U", "Reçu")
                .Replace("Re?u", "Reçu")
                .Replace("re?u", "Reçu")
                .Replace("re?U", "Reçu")
                .Replace("Co?T", "Coût")
                .Replace("Co?t", "Coût")
                .Replace("co?t", "coût")
                .Replace("co?T", "coût")
                .Replace("Contr?Le", "Contrôle")
                .Replace("Contr?le", "Contrôle")
                .Replace("contr?Le", "contrôle")
                .Replace("contr?le", "contrôle")
                ;

            return newString;
        }

        public static string ES_Replace(string value)
        {
            if (value == string.Empty)
            {
                return string.Empty;
            }

            // Convert to Pascal Case First, but only if there are spaces in value (else it has already been done)
            //var textInfo = new CultureInfo("fr", false).TextInfo;
            //var pascalCase = value.Contains(" ") ? textInfo.ToTitleCase(value) : value;

            var newString = value
                .Replace("Último Fin", "Último Final Del Día)")
                .Replace("A?O", "Año")
                .Replace("A?o", "Año")
                .Replace("a?o", "Año")
                .Replace("a?O", "Año")
                ;

            return newString;
        }
        #endregion

        #region Private Methods
        #endregion
    }

    public class Company
    {
        public enum LanguageEnum
        {
            ENG = 0,
            FRA,
            ESN,
            CHT,
            CHN
        }

        public string CompanyName { get; set; }
        public string Version { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IncludeFra { get; set; }
        public string UsernameFra { get; set; }
        public string PasswordFra { get; set; }
        public bool IncludeEsn { get; set; }
        public string UsernameEsn { get; set; }
        public string PasswordEsn { get; set; }
        public bool IncludeChn { get; set; }
        public string UsernameChn { get; set; }
        public string PasswordChn { get; set; }
        public bool IncludeCht { get; set; }
        public string UsernameCht { get; set; }
        public string PasswordCht { get; set; }
    }
    /// <summary>
    /// Master Configuration Setting Definition
    /// </summary>
    public class InquiryConfigurationDefinition
    {
        public string Included { get; set; }
        public string TemplateId { get; set; }
        public string DatasourceId { get; set; }
        public string Module { get; set; }
        public string ViewID { get; set; }
        public string ViewName { get; set; }
        public string SecurityResource { get; set; }
        public string SecurityResourceName { get; set; }
        public string LicenceCheck { get; set; }
        public string DatasourceName { get; set; }
        public string Name { get; set; }
        public string NameFra { get; set; }
        public string NameEsn { get; set; }
        public string NameChn { get; set; }
        public string NameCht { get; set; }
        public string DisplayList { get; set; }
        public string DisplayOrderList { get; set; }
        public string Description { get; set; }
        public string DescriptionFra { get; set; }
        public string DescriptionEsn { get; set; }
        public string DescriptionChn { get; set; }
        public string DescriptionCht { get; set; }
        public string OutputPath { get; set; }
        //public string RunLog { get; set; }
        //public string ProcessLog { get; set; }
        public string SQL { get; set; }
        public string SkipTableName { get; set; }
        public string WhereClause { get; set; }
        public string OrderByClause { get; set; }

        // Used to link Sage 300 View Columns Setting XLSX file
        public string ConfigSettingFile { get; set; }

        // Used in Selected Columns Setting XLSX file
        public string SelectedList { get; set; }

        // Used in InquiryTemplate JSON file
        public List<SortField> SortFields = new List<SortField>();

        public string SortFieldsStr { get; set; }
        public string ShowAggregates { get; set; }
    }

    /// <summary>
    /// Sage 300 View Column Setting Definition
    /// </summary>
    public class ConfigurationColumnSettingDefinition
    {
        public string TableName { get; set; }
        public string Field { get; set; }
        public string Included { get; set; }
        public string OverridePrList { get; set; }
        public string IsDummy { get; set; }
        public string YesNoPresentation { get; set; }
        public string FieldAlias { get; set; }

        public List<Translation> Captions = new List<Translation>();
        public string IsFilterable { get; set; }
        public string IsSortable { get; set; }
        public string SortFieldsStr { get; set; }
        public string IsGroupBy { get; set; }
        public string AggregatesStr { get; set; }

        public List<string> Aggregates = new List<string>();
        public string IsDrilldown { get; set; }
        public string Controller_Type { get; set; }
        public string Controller_SrceAppl { get; set; }
        public string Controller_AreaList { get; set; }
        public string Controller_ControllerList { get; set; }
        public string Controller_TypeList { get; set; }
        public string Controller_SrceApplList { get; set; }
        public string Controller_ActionList { get; set; }
        public string ParameterName { get; set; }
        public string ParameterField { get; set; }
        public DrillDownURL DrilldownUrl { get; set; }

        public DisplayConditionDef DisplayCondition = new DisplayConditionDef();
        public DisplayConditionDef DrilldownCondition = new DisplayConditionDef();
        public string LicenceCheck { get; set; }

    }

    public class Translation
    {
        [JsonProperty(Order = 1)]
        public string Language { get; set; }

        [JsonProperty(Order = 2)]
        public string Value { get; set; }

        [JsonProperty(Order = 3)]
        public string ColumnWidth { get; set; }
        public Translation(string language, string value)
        {
            this.Language = language;
            this.Value = (string.IsNullOrEmpty(value)) ? "" : value;
            this.ColumnWidth = "200px";
        }
    }

    public class SecurityResource
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public SecurityResource(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }

    public class ControllerDef
    {

        [JsonProperty(Order = 1)]
        public string Area { get; set; }

        [JsonProperty(Order = 2)]
        public string Controller { get; set; }

        [JsonProperty(Order = 3)]
        public string Action { get; set; }

        [JsonProperty(Order = 4)]
        public List<Param> parameters = new List<Param>();

        public ControllerDef(string area, string controller, string action)
        {
            this.Area = area;
            this.Controller = controller;
            this.Action = action;
        }

        public ControllerDef()
        {

        }
    }
    public class ControllerTypeMapping
    {
        [JsonProperty(Order = 1)]
        public string Type { get; set; }

        [JsonProperty(Order = 2)]
        public string SrceAppl { get; set; }

        [JsonProperty(Order = 3)]
        public ControllerDef Controller = new ControllerDef();

        public ControllerTypeMapping(string type, ControllerDef controller)
        {
            this.Type = type;
            this.Controller = controller;
        }
    }

    public class ControllParamDef
    {
        [JsonProperty(Order = 1)]
        public string Controller { get; set; }

        [JsonProperty(Order = 2)]
        public string Area { get; set; }

        [JsonProperty(Order = 3)]
        public string Action { get; set; }

        [JsonProperty(Order = 4)]
        public string parameters { get; set; }

    }
    public class Param
    {
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Field { get; set; }

        public Param(string name, string field)
        {
            this.Name = name;
            this.Field = field;
        }
    }
    public class DrillDownURL
    {
        [JsonProperty(Order = 1)]
        public string TypeField { get; set; }

        [JsonProperty(Order = 2)]
        public string SrceApplField { get; set; }

        [JsonProperty(Order = 3)]
        public List<ControllerTypeMapping> ControllerList = new List<ControllerTypeMapping>();

        [JsonProperty(Order = 4)]
        public DisplayConditionDef DrilldownCondition = new DisplayConditionDef();

        public DrillDownURL(string typefield, List<ControllerTypeMapping> controllerlist, List<Param> parameters, string srceapplfield = "", DisplayConditionDef drilldowncondition = null)
        {
            this.TypeField = typefield;
            this.ControllerList = controllerlist;
            this.DrilldownCondition = drilldowncondition;
            this.SrceApplField = srceapplfield;
            //this.Params = parameters;
        }

        public DrillDownURL()
        {
        }
    }
    public class DisplayConditionDef
    {
        [JsonProperty(Order = 1)]
        public string Field { get; set; }

        [JsonProperty(Order = 2)]
        public string Value { get; set; }

        [JsonProperty(Order = 3)]
        public string Operator { get; set; }

        public DisplayConditionDef(string field, string value, string operatorstr)
        {
            this.Field = field;
            this.Value = value;
            this.Operator = operatorstr;
        }

        public DisplayConditionDef()
        {
        }
    }
    public class PresentationList
    {
        [JsonProperty(Order = 1)]
        public bool Selected { get; set; }

        [JsonProperty(Order = 2)]
        public string Value { get; set; }

        [JsonProperty(Order = 3)]
        public List<Translation> Captions = new List<Translation>();

        public PresentationList(bool selected, string value)
        {
            this.Selected = selected;
            this.Value = value;
        }
    }
    public class FieldDefinition
    {
        [JsonProperty(Order = 0)]
        public bool Included { get; set; }

        [JsonProperty(Order = 1)]
        public string Field { get; set; }

        [JsonProperty(Order = 2)]
        public bool IsDummy { get; set; }

        [JsonProperty(Order = 3)]
        public string FieldAlias { get; set; }

        [JsonProperty(Order = 4)]
        public int FieldIndex { get; set; }

        [JsonProperty(Order = 5)]
        public string ViewID { get; set; }

        [JsonProperty(Order = 6)]
        public string ViewName { get; set; }

        [JsonProperty(Order = 7)]
        public string TableName { get; set; }

        [JsonProperty(Order = 8)]
        public List<Translation> Captions = new List<Translation>();

        [JsonProperty(Order = 9)]
        public int Precision { get; set; }

        [JsonProperty(Order = 10)]
        public bool IsSQLColumnInView { get; set; }

        [JsonProperty(Order = 11)]
        public string ColumnNameInView { get; set; }

        [JsonProperty(Order = 12)]
        public int DataTypeEnumeration { get; set; }

        [JsonProperty(Order = 13)]
        public string DataType { get; set; }

        [JsonProperty(Order = 14)]
        public bool IsDisplayable { get; set; }

        [JsonProperty(Order = 15)]
        public bool IsFilterable { get; set; }

        [JsonProperty(Order = 16)]
        public bool IsSortable { get; set; }

        [JsonProperty(Order = 17)]
        public string SortFieldsStr { get; set; }

        [JsonProperty(Order = 18)]
        public bool IsDrilldown { get; set; }

        [JsonProperty(Order = 19)]
        public bool IsGroupable { get; set; }

        [JsonProperty(Order = 20)]
        public bool IsAggregate { get; set; }

        [JsonProperty(Order = 21)]
        public List<string> Aggregates = new List<string>();

        [JsonProperty(Order = 22)]
        public DrillDownURL DrilldownURL = new DrillDownURL();

        //[JsonProperty(Order = 19)]
        //public ControllerDef DtlDrilldownController = new ControllerDef();

        [JsonProperty(Order = 23)]
        public List<PresentationList> PresentationList = new List<PresentationList>();

        [JsonProperty(Order = 24)]
        public DisplayConditionDef DisplayCondition = new DisplayConditionDef();

        [JsonProperty(Order = 25)]
        public string LicenceCheck { get; set; }
    }

    public class OverridePresentationList
    {
        [JsonProperty(Order = 1)]
        public string Type { get; set; }

        [JsonProperty(Order = 2)]
        public List<PresentationList> PresentationList = new List<PresentationList>();

    }
    public class InquiryConfiguration
    {
        [JsonProperty(Order = 1)]
        public string GeneratedMessage { get; set; }

        [JsonProperty(Order = 2)]
        public string GeneratedWarning { get; set; }

        [JsonProperty(Order = 3)]
        public string DatasourceId { get; set; }

        [JsonProperty(Order = 4)]
        public string FileName { get; set; }

        [JsonProperty(Order = 5)]
        public string Name { get; set; }

        [JsonProperty(Order = 6)]
        public List<Translation> Description = new List<Translation>();

        [JsonProperty(Order = 7)]
        public string Module { get; set; }

        [JsonProperty(Order = 8)]
        public string ViewID { get; set; }

        [JsonProperty(Order = 9)]
        public string ViewName { get; set; }

        [JsonProperty(Order = 10)]
        public List<SecurityResource> SecurityResource = new List<SecurityResource>();

        [JsonProperty(Order = 11)]
        public string LicenseCheck { get; set; }

        [JsonProperty(Order = 12)]
        public string SQL { get; set; }

        [JsonProperty(Order = 13)]
        public List<FieldDefinition> Fields = new List<FieldDefinition>();

        public InquiryConfiguration()
        {

        }
    }

    public class SortField
    {
        public string Name { get; set; }
        public string SortOrder { get; set; }

        public SortField(string name, string sortOrder)
        {
            this.Name = name;
            this.SortOrder = sortOrder;
        }
    }

    public class InquiryTemplate
    {
        [JsonProperty(Order = 1)]
        public string GeneratedMessage { get; set; }

        [JsonProperty(Order = 2)]
        public string GeneratedWarning { get; set; }

        [JsonProperty(Order = 3)]
        public string TemplateId { get; set; }

        [JsonProperty(Order = 4)]
        public string DatasourceId { get; set; }

        [JsonProperty(Order = 5)]
        public string FileName { get; set; }

        [JsonProperty(Order = 6)]
        public List<Translation> Name = new List<Translation>();

        [JsonProperty(Order = 7)]
        public List<Translation> Description = new List<Translation>();

        [JsonProperty(Order = 8)]
        public string DisplayFieldsStr { get; set; }

        [JsonProperty(Order = 9)]
        public string SelectFieldsStr { get; set; }

        [JsonProperty(Order = 10)]
        public string DisplayOrderStr { get; set; }

        [JsonProperty(Order = 11)]
        public string WhereClause { get; set; }

        [JsonProperty(Order = 12)]
        public List<SortField> SortFields = new List<SortField>();

        [JsonProperty(Order = 13)]
        public string SortFieldsStr { get; set; }

        [JsonProperty(Order = 14)]
        public string ShowAggregates { get; set; }
    }
}
