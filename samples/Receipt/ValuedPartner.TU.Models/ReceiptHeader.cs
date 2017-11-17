// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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

#region Namespace

using System;
using System.ComponentModel.DataAnnotations;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;

using ValuedParter.TU.Models.Enums;
using ValuedParter.TU.Resources.Forms;

#endregion

namespace ValuedParter.TU.Models
{
    /// <summary>
    /// Partial class for ReceiptHeader
    /// </summary>
    public partial class ReceiptHeader : ModelBase
    {
        /// <summary>
        /// This constructor initializes EnumerableResponses/Lists to be empty.
        /// This avoids the problem of serializing null collections.
        /// </summary>
        public ReceiptHeader()
        {
            ReceiptDetail = new EnumerableResponse<ReceiptDetail>();
            ReceiptOptionalField = new EnumerableResponse<ReceiptOptionalField>();
            ReceiptDetailOptionalField = new EnumerableResponse<ReceiptDetailOptionalField>();
            // Casts from List to IList.
        }

        /// <summary>
        /// Gets or sets SequenceNumber
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SequenceNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public long SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Description", ResourceType = typeof (ReceiptHeaderResx))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets ReceiptDate
        /// </summary>
        [ValidateDateFormat(ErrorMessageResourceName="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ReceiptDate", ResourceType = typeof (ReceiptHeaderResx))]
        public DateTime ReceiptDate { get; set; }

        /// <summary>
        /// Gets or sets FiscalYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FiscalYear", ResourceType = typeof (ReceiptHeaderResx))]
        public string FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets FiscalPeriod
        /// </summary>
        [Display(Name = "FiscalPeriod", ResourceType = typeof (ReceiptHeaderResx))]
        public Enums.FiscalPeriod FiscalPeriod { get; set; }

        /// <summary>
        /// Gets or sets PurchaseOrderNumber
        /// </summary>
        [StringLength(22, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "PurchaseOrderNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets Reference
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Reference", ResourceType = typeof (ReceiptHeaderResx))]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets ReceiptType
        /// </summary>
        [Display(Name = "ReceiptType", ResourceType = typeof (ReceiptHeaderResx))]
        public ReceiptType ReceiptType { get; set; }

        /// <summary>
        /// Gets or sets RateOperation
        /// </summary>
        [Display(Name = "RateOperation", ResourceType = typeof (ReceiptHeaderResx))]
        public RateOperation RateOperation { get; set; }

        /// <summary>
        /// Gets or sets VendorNumber
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "VendorNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public string VendorNumber { get; set; }

        /// <summary>
        /// Gets or sets ReceiptCurrency
        /// </summary>
        [StringLength(3, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ReceiptCurrency", ResourceType = typeof (ReceiptHeaderResx))]
        public string ReceiptCurrency { get; set; }

        /// <summary>
        /// Gets or sets ExchangeRate
        /// </summary>
        [Display(Name = "ExchangeRate", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Gets or sets RateType
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "RateType", ResourceType = typeof (ReceiptHeaderResx))]
        public string RateType { get; set; }

        /// <summary>
        /// Gets or sets RateDate
        /// </summary>
        [ValidateDateFormat(ErrorMessageResourceName="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "RateDate", ResourceType = typeof (ReceiptHeaderResx))]
        public DateTime RateDate { get; set; }

        /// <summary>
        /// Gets or sets RateOverride
        /// </summary>
        [Display(Name = "RateOverride", ResourceType = typeof (ReceiptHeaderResx))]
        public RateOverride RateOverride { get; set; }

        /// <summary>
        /// Gets or sets AdditionalCost
        /// </summary>
        [Display(Name = "AdditionalCost", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal AdditionalCost { get; set; }

        /// <summary>
        /// Gets or sets OrigAdditionalCostFunc
        /// </summary>
        [Display(Name = "OrigAdditionalCostFunc", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal OrigAdditionalCostFunc { get; set; }

        /// <summary>
        /// Gets or sets OrigAdditionalCostSource
        /// </summary>
        [Display(Name = "OrigAdditionalCostSource", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal OrigAdditionalCostSource { get; set; }

        /// <summary>
        /// Gets or sets AdditionalCostCurrency
        /// </summary>
        [StringLength(3, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "AdditionalCostCurrency", ResourceType = typeof (ReceiptHeaderResx))]
        public string AdditionalCostCurrency { get; set; }

        /// <summary>
        /// Gets or sets TotalExtendedCostFunctional
        /// </summary>
        [Display(Name = "TotalExtendedCostFunctional", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalExtendedCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets TotalExtendedCostSource
        /// </summary>
        [Display(Name = "TotalExtendedCostSource", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalExtendedCostSource { get; set; }

        /// <summary>
        /// Gets or sets TotalExtendedCostAdjusted
        /// </summary>
        [Display(Name = "TotalExtendedCostAdjusted", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalExtendedCostAdjusted { get; set; }

        /// <summary>
        /// Gets or sets TotalAdjustedCostFunctional
        /// </summary>
        [Display(Name = "TotalAdjustedCostFunctional", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalAdjustedCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets TotalReturnCost
        /// </summary>
        [Display(Name = "TotalReturnCost", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalReturnCost { get; set; }

        /// <summary>
        /// Gets or sets NumberOfDetailswithCost
        /// </summary>
        [Display(Name = "NumberOfDetailswithCost", ResourceType = typeof (ReceiptHeaderResx))]
        public int NumberOfDetailswithCost { get; set; }

        /// <summary>
        /// Gets or sets RequireLabels
        /// </summary>
        [Display(Name = "RequireLabels", ResourceType = typeof (ReceiptHeaderResx))]
        public RequireLabels RequireLabels { get; set; }

        /// <summary>
        /// Gets or sets AdditionalCostAllocationType
        /// </summary>
        [Display(Name = "AdditionalCostAllocationType", ResourceType = typeof (ReceiptHeaderResx))]
        public AdditionalCostAllocationType AdditionalCostAllocationType { get; set; }

        /// <summary>
        /// Gets or sets Complete
        /// </summary>
        [Display(Name = "Complete", ResourceType = typeof (ReceiptHeaderResx))]
        public Complete Complete { get; set; }

        /// <summary>
        /// Gets or sets OriginalTotalCostSource
        /// </summary>
        [Display(Name = "OriginalTotalCostSource", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal OriginalTotalCostSource { get; set; }

        /// <summary>
        /// Gets or sets OriginalTotalCostFunctional
        /// </summary>
        [Display(Name = "OriginalTotalCostFunctional", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal OriginalTotalCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets AdditionalCostFunctional
        /// </summary>
        [Display(Name = "AdditionalCostFunctional", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal AdditionalCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets TotalCostReceiptAdditional
        /// </summary>
        [Display(Name = "TotalCostReceiptAdditional", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalCostReceiptAdditional { get; set; }

        /// <summary>
        /// Gets or sets TotalAdjCostReceiptAddl
        /// </summary>
        [Display(Name = "TotalAdjCostReceiptAddl", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TotalAdjCostReceiptAddl { get; set; }

        /// <summary>
        /// Gets or sets ReceiptCurrencyDecimals
        /// </summary>
        [Display(Name = "ReceiptCurrencyDecimals", ResourceType = typeof (ReceiptHeaderResx))]
        public int ReceiptCurrencyDecimals { get; set; }

        /// <summary>
        /// Gets or sets VendorShortName
        /// </summary>
        [StringLength(10, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "VendorShortName", ResourceType = typeof (ReceiptHeaderResx))]
        public string VendorShortName { get; set; }

        /// <summary>
        /// Gets or sets ICUniqueDocumentNumber
        /// </summary>
        [Display(Name = "ICUniqueDocumentNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal ICUniqueDocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets VendorExists
        /// </summary>
        [Display(Name = "VendorExists", ResourceType = typeof (ReceiptHeaderResx))]
        public VendorExists VendorExists { get; set; }

        /// <summary>
        /// Gets or sets RecordDeleted
        /// </summary>
        [Display(Name = "RecordDeleted", ResourceType = typeof (ReceiptHeaderResx))]
        public RecordDeleted RecordDeleted { get; set; }

        /// <summary>
        /// Gets or sets TransactionNumber
        /// </summary>
        [Display(Name = "TransactionNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public decimal TransactionNumber { get; set; }

        /// <summary>
        /// Gets or sets RecordStatus
        /// </summary>
        [Display(Name = "RecordStatus", ResourceType = typeof (ReceiptHeaderResx))]
        public RecordStatus RecordStatus { get; set; }

        /// <summary>
        /// Gets or sets ReceiptNumber
        /// </summary>
        [StringLength(22, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ReceiptNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public string ReceiptNumber { get; set; }

        /// <summary>
        /// Gets or sets NextDetailLineNumber
        /// </summary>
        [Display(Name = "NextDetailLineNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public int NextDetailLineNumber { get; set; }

        /// <summary>
        /// Gets or sets RecordPrinted
        /// </summary>
        [Display(Name = "RecordPrinted", ResourceType = typeof (ReceiptHeaderResx))]
        public RecordPrinted RecordPrinted { get; set; }

        /// <summary>
        /// Gets or sets PostSequenceNumber
        /// </summary>
        [Display(Name = "PostSequenceNumber", ResourceType = typeof (ReceiptHeaderResx))]
        public long PostSequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets OptionalFields
        /// </summary>
        [Display(Name = "OptionalFields", ResourceType = typeof (ReceiptHeaderResx))]
        public long OptionalFields { get; set; }

        /// <summary>
        /// Gets or sets ProcessCommand
        /// </summary>
        [Display(Name = "ProcessCommand", ResourceType = typeof (ReceiptHeaderResx))]
        public ProcessCommand ProcessCommand { get; set; }

        /// <summary>
        /// Gets or sets VendorName
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "VendorName", ResourceType = typeof (ReceiptHeaderResx))]
        public string VendorName { get; set; }

        /// <summary>
        /// Gets or sets EnteredBy
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "EnteredBy", ResourceType = typeof (ReceiptHeaderResx))]
        public string EnteredBy { get; set; }

        /// <summary>
        /// Gets or sets PostingDate
        /// </summary>
        [ValidateDateFormat(ErrorMessageResourceName="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "PostingDate", ResourceType = typeof (ReceiptHeaderResx))]
        public DateTime PostingDate { get; set; }

        /// <summary>
        /// Gets Use ReceiptType string value
        /// </summary>
        [IgnoreExportImport]
        public string ReceiptTypeInText
        {
            get { return EnumUtility.GetStringValue(ReceiptType); }
        }

        /// <summary>
        /// Gets or sets Home currency 
        /// </summary>
        [IgnoreExportImport]
        public string HomeCurrency { get; set; }

        [IgnoreExportImport]
        public EnumerableResponse<ReceiptDetail> ReceiptDetail { get; set; }

        /// <summary>
        /// Gets or sets ReceiptOptionalField
        /// </summary>
        [IgnoreExportImport]
        public EnumerableResponse<ReceiptOptionalField> ReceiptOptionalField { get; set; }

        /// <summary>
        /// Gets or sets ReceiptDetailOptionalField
        /// </summary>
        [IgnoreExportImport]
        public EnumerableResponse<ReceiptDetailOptionalField> ReceiptDetailOptionalField { get; set; }

        /// <summary>
        /// IsOptionalFields is for validating the OptionalFields checkbox
        /// </summary> 
        [IgnoreExportImport]
        public bool IsOptionalFields { get; set; }

        /// <summary>
        /// IsRequireLabel is for validating the RequireLabels checkbox
        /// </summary> 
        [IgnoreExportImport]
        public bool IsRequireLabel { get; set; }

        /// <summary>
        /// Gets or sets IsHeader
        /// </summary> 
        [IgnoreExportImport]
        public bool IsHeader { get; set; }

        /// <summary>
        /// TotalCostReceiptAdditionalDecimal
        /// </summary>
        public int TotalCostReceiptAdditionalDecimal { get; set; }

        /// <summary>
        /// TotalReturnCostDecimal
        /// </summary>
        public int TotalReturnCostDecimal { get; set; }

        #region UI Strings

        /// <summary>
        /// Gets FiscalPeriod string value
        /// </summary>
        public string FiscalPeriodString
        {
         get { return EnumUtility.GetStringValue(FiscalPeriod); }
        }

        /// <summary>
        /// Gets ReceiptType string value
        /// </summary>
        public string ReceiptTypeString
        {
         get { return EnumUtility.GetStringValue(ReceiptType); }
        }

        /// <summary>
        /// Gets RateOperation string value
        /// </summary>
        public string RateOperationString
        {
         get { return EnumUtility.GetStringValue(RateOperation); }
        }

        /// <summary>
        /// Gets RateOverride string value
        /// </summary>
        public string RateOverrideString
        {
         get { return EnumUtility.GetStringValue(RateOverride); }
        }

        /// <summary>
        /// Gets RequireLabels string value
        /// </summary>
        public string RequireLabelsString
        {
         get { return EnumUtility.GetStringValue(RequireLabels); }
        }

        /// <summary>
        /// Gets AdditionalCostAllocationType string value
        /// </summary>
        public string AdditionalCostAllocationTypeString
        {
         get { return EnumUtility.GetStringValue(AdditionalCostAllocationType); }
        }

        /// <summary>
        /// Gets Complete string value
        /// </summary>
        public string CompleteString
        {
         get { return EnumUtility.GetStringValue(Complete); }
        }

        /// <summary>
        /// Gets VendorExists string value
        /// </summary>
        public string VendorExistsString
        {
         get { return EnumUtility.GetStringValue(VendorExists); }
        }

        /// <summary>
        /// Gets RecordDeleted string value
        /// </summary>
        public string RecordDeletedString
        {
         get { return EnumUtility.GetStringValue(RecordDeleted); }
        }

        /// <summary>
        /// Gets RecordStatus string value
        /// </summary>
        public string RecordStatusString
        {
         get { return EnumUtility.GetStringValue(RecordStatus); }
        }

        /// <summary>
        /// Gets RecordPrinted string value
        /// </summary>
        public string RecordPrintedString
        {
         get { return EnumUtility.GetStringValue(RecordPrinted); }
        }

        /// <summary>
        /// Gets ProcessCommand string value
        /// </summary>
        public string ProcessCommandString
        {
         get { return EnumUtility.GetStringValue(ProcessCommand); }
        }

        #endregion
    }
}