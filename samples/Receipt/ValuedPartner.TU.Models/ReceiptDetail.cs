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

using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models
{
    /// <summary>
    /// Partial class for ReceiptDetail
    /// </summary>
    public partial class ReceiptDetail : ModelBase
    {
        /// <summary>
        /// Gets or sets SequenceNumber
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SequenceNumber", ResourceType = typeof (ReceiptDetailResx))]
        public long SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets LineNumber
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "LineNumber", ResourceType = typeof (ReceiptDetailResx))]
        public int LineNumber { get; set; }

        /// <summary>
        /// Gets or sets ItemNumber
        /// </summary>
        [StringLength(24, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ItemNumber", ResourceType = typeof (ReceiptDetailResx))]
        public string ItemNumber { get; set; }

        /// <summary>
        /// Gets or sets ItemDescription
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ItemDescription", ResourceType = typeof (ReceiptDetailResx))]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets Category
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Category", ResourceType = typeof (ReceiptDetailResx))]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Location
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Location", ResourceType = typeof (ReceiptDetailResx))]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets QuantityReceived
        /// </summary>
        [Display(Name = "QuantityReceived", ResourceType = typeof (ReceiptDetailResx))]
        public decimal QuantityReceived { get; set; }

        /// <summary>
        /// Gets or sets QuantityReturned
        /// </summary>
        [Display(Name = "QuantityReturned", ResourceType = typeof (ReceiptDetailResx))]
        public decimal QuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets UnitOfMeasure
        /// </summary>
        [StringLength(10, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "UnitOfMeasure", ResourceType = typeof (ReceiptDetailResx))]
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Gets or sets ConversionFactor
        /// </summary>
        [Display(Name = "ConversionFactor", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ConversionFactor { get; set; }

        /// <summary>
        /// Gets or sets ProratedAddlCostFunc
        /// </summary>
        [Display(Name = "ProratedAddlCostFunc", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ProratedAddlCostFunc { get; set; }

        /// <summary>
        /// Gets or sets ProratedAddlCostSrc
        /// </summary>
        [Display(Name = "ProratedAddlCostSrc", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ProratedAddlCostSrc { get; set; }

        /// <summary>
        /// Gets or sets UnitCost
        /// </summary>
        [Display(Name = "UnitCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Gets or sets AdjustedUnitCost
        /// </summary>
        [Display(Name = "AdjustedUnitCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal AdjustedUnitCost { get; set; }

        /// <summary>
        /// Gets or sets AdjustedCost
        /// </summary>
        [Display(Name = "AdjustedCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal AdjustedCost { get; set; }

        /// <summary>
        /// Gets or sets AdjustedCostFunctional
        /// </summary>
        [Display(Name = "AdjustedCostFunctional", ResourceType = typeof (ReceiptDetailResx))]
        public decimal AdjustedCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets ExtendedCost
        /// </summary>
        [Display(Name = "ExtendedCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ExtendedCost { get; set; }

        /// <summary>
        /// Gets or sets ExtendedCostFunctional
        /// </summary>
        [Display(Name = "ExtendedCostFunctional", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ExtendedCostFunctional { get; set; }

        /// <summary>
        /// Gets or sets ReturnCost
        /// </summary>
        [Display(Name = "ReturnCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ReturnCost { get; set; }

        /// <summary>
        /// Gets or sets CostingDate
        /// </summary>
        [ValidateDateFormat(ErrorMessageResourceName="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "CostingDate", ResourceType = typeof (ReceiptDetailResx))]
        public DateTime CostingDate { get; set; }

        /// <summary>
        /// Gets or sets CostingSequenceNo
        /// </summary>
        [Display(Name = "CostingSequenceNo", ResourceType = typeof (ReceiptDetailResx))]
        public long CostingSequenceNo { get; set; }

        /// <summary>
        /// Gets or sets OriginalReceiptQty
        /// </summary>
        [Display(Name = "OriginalReceiptQty", ResourceType = typeof (ReceiptDetailResx))]
        public decimal OriginalReceiptQty { get; set; }

        /// <summary>
        /// Gets or sets OriginalUnitCost
        /// </summary>
        [Display(Name = "OriginalUnitCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal OriginalUnitCost { get; set; }

        /// <summary>
        /// Gets or sets OriginalExtendedCost
        /// </summary>
        [Display(Name = "OriginalExtendedCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal OriginalExtendedCost { get; set; }

        /// <summary>
        /// Gets or sets OriginalExtendedCostFunc
        /// </summary>
        [Display(Name = "OriginalExtendedCostFunc", ResourceType = typeof (ReceiptDetailResx))]
        public decimal OriginalExtendedCostFunc { get; set; }

        /// <summary>
        /// Gets or sets Comments
        /// </summary>
        [StringLength(250, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Comments", ResourceType = typeof (ReceiptDetailResx))]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets Labels
        /// </summary>
        [Display(Name = "Labels", ResourceType = typeof (ReceiptDetailResx))]
        public int Labels { get; set; }

        /// <summary>
        /// Gets or sets StockItem
        /// </summary>
        [Display(Name = "StockItem", ResourceType = typeof (ReceiptDetailResx))]
        public bool StockItem { get; set; }

        /// <summary>
        /// Gets or sets ManufacturersItemNumber
        /// </summary>
        [StringLength(24, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ManufacturersItemNumber", ResourceType = typeof (ReceiptDetailResx))]
        public string ManufacturersItemNumber { get; set; }

        /// <summary>
        /// Gets or sets VendorItemNumber
        /// </summary>
        [StringLength(24, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "VendorItemNumber", ResourceType = typeof (ReceiptDetailResx))]
        public string VendorItemNumber { get; set; }

        /// <summary>
        /// Gets or sets DetailLineNumber
        /// </summary>
        [Display(Name = "DetailLineNumber", ResourceType = typeof (ReceiptDetailResx))]
        public int DetailLineNumber { get; set; }

        /// <summary>
        /// Gets or sets QuantityReturnedToDate
        /// </summary>
        [Display(Name = "QuantityReturnedToDate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal QuantityReturnedToDate { get; set; }

        /// <summary>
        /// Gets or sets ReturnedExtCostToDate
        /// </summary>
        [Display(Name = "ReturnedExtCostToDate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ReturnedExtCostToDate { get; set; }

        /// <summary>
        /// Gets or sets ReturnedExtCostFuncToDate
        /// </summary>
        [Display(Name = "ReturnedExtCostFuncToDate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal ReturnedExtCostFuncToDate { get; set; }

        /// <summary>
        /// Gets or sets AdjustedExtCostToDate
        /// </summary>
        [Display(Name = "AdjustedExtCostToDate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal AdjustedExtCostToDate { get; set; }

        /// <summary>
        /// Gets or sets AdjustedExtCostFuncToDate
        /// </summary>
        [Display(Name = "AdjustedExtCostFuncToDate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal AdjustedExtCostFuncToDate { get; set; }

        /// <summary>
        /// Gets or sets PreviousDayEndReceiptQty
        /// </summary>
        [Display(Name = "PreviousDayEndReceiptQty", ResourceType = typeof (ReceiptDetailResx))]
        public decimal PreviousDayEndReceiptQty { get; set; }

        /// <summary>
        /// Gets or sets PreviousDayEndUnitCost
        /// </summary>
        [Display(Name = "PreviousDayEndUnitCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal PreviousDayEndUnitCost { get; set; }

        /// <summary>
        /// Gets or sets PreviousDayEndExtCost
        /// </summary>
        [Display(Name = "PreviousDayEndExtCost", ResourceType = typeof (ReceiptDetailResx))]
        public decimal PreviousDayEndExtCost { get; set; }

        /// <summary>
        /// Gets or sets PreviousDayEndExtCostFunc
        /// </summary>
        [Display(Name = "PreviousDayEndExtCostFunc", ResourceType = typeof (ReceiptDetailResx))]
        public decimal PreviousDayEndExtCostFunc { get; set; }

        /// <summary>
        /// Gets or sets UnformattedItemNumber
        /// </summary>
        [StringLength(24, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "UnformattedItemNumber", ResourceType = typeof (ReceiptDetailResx))]
        public string UnformattedItemNumber { get; set; }

        /// <summary>
        /// Gets or sets CheckBelowZero
        /// </summary>
        [Display(Name = "CheckBelowZero", ResourceType = typeof (ReceiptDetailResx))]
        public bool CheckBelowZero { get; set; }

        /// <summary>
        /// Gets or sets RevisionListLineNumber
        /// </summary>
        [Display(Name = "RevisionListLineNumber", ResourceType = typeof (ReceiptDetailResx))]
        public int RevisionListLineNumber { get; set; }

        /// <summary>
        /// Gets or sets InterprocessCommID
        /// </summary>
        [Display(Name = "InterprocessCommID", ResourceType = typeof (ReceiptDetailResx))]
        public long InterprocessCommID { get; set; }

        /// <summary>
        /// Gets or sets ForcePopupSN
        /// </summary>
        [Display(Name = "ForcePopupSN", ResourceType = typeof (ReceiptDetailResx))]
        public bool ForcePopupSN { get; set; }

        /// <summary>
        /// Gets or sets PopupSN
        /// </summary>
        [Display(Name = "PopupSN", ResourceType = typeof (ReceiptDetailResx))]
        public int PopupSN { get; set; }

        /// <summary>
        /// Gets or sets CloseSN
        /// </summary>
        [Display(Name = "CloseSN", ResourceType = typeof (ReceiptDetailResx))]
        public bool CloseSN { get; set; }

        /// <summary>
        /// Gets or sets LTSetID
        /// </summary>
        [Display(Name = "LTSetID", ResourceType = typeof (ReceiptDetailResx))]
        public long LTSetID { get; set; }

        /// <summary>
        /// Gets or sets ForcePopupLT
        /// </summary>
        [Display(Name = "ForcePopupLT", ResourceType = typeof (ReceiptDetailResx))]
        public bool ForcePopupLT { get; set; }

        /// <summary>
        /// Gets or sets PopupLT
        /// </summary>
        [Display(Name = "PopupLT", ResourceType = typeof (ReceiptDetailResx))]
        public int PopupLT { get; set; }

        /// <summary>
        /// Gets or sets CloseLT
        /// </summary>
        [Display(Name = "CloseLT", ResourceType = typeof (ReceiptDetailResx))]
        public bool CloseLT { get; set; }

        /// <summary>
        /// Gets or sets OptionalFields
        /// </summary>
        [Display(Name = "OptionalFields", ResourceType = typeof (ReceiptDetailResx))]
        public long OptionalFields { get; set; }

        /// <summary>
        /// Gets or sets ProcessCommand
        /// </summary>
        [Display(Name = "ProcessCommand", ResourceType = typeof (ReceiptDetailResx))]
        public ProcessCommand ProcessCommand { get; set; }

        /// <summary>
        /// Gets or sets SerialQuantity
        /// </summary>
        [Display(Name = "SerialQuantity", ResourceType = typeof (ReceiptDetailResx))]
        public long SerialQuantity { get; set; }

        /// <summary>
        /// Gets or sets LotQuantity
        /// </summary>
        [Display(Name = "LotQuantity", ResourceType = typeof (ReceiptDetailResx))]
        public decimal LotQuantity { get; set; }

        /// <summary>
        /// Gets or sets SerialQuantityReturned
        /// </summary>
        [Display(Name = "SerialQuantityReturned", ResourceType = typeof (ReceiptDetailResx))]
        public long SerialQuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets LotQuantityReturned
        /// </summary>
        [Display(Name = "LotQuantityReturned", ResourceType = typeof (ReceiptDetailResx))]
        public decimal LotQuantityReturned { get; set; }

        /// <summary>
        /// Gets or sets SerialLotQuantityToProcess
        /// </summary>
        [Display(Name = "SerialLotQuantityToProcess", ResourceType = typeof (ReceiptDetailResx))]
        public decimal SerialLotQuantityToProcess { get; set; }

        /// <summary>
        /// Gets or sets NumberOfLotsToGenerate
        /// </summary>
        [Display(Name = "NumberOfLotsToGenerate", ResourceType = typeof (ReceiptDetailResx))]
        public decimal NumberOfLotsToGenerate { get; set; }

        /// <summary>
        /// Gets or sets QuantityperLot
        /// </summary>
        [Display(Name = "QuantityperLot", ResourceType = typeof (ReceiptDetailResx))]
        public decimal QuantityperLot { get; set; }

        /// <summary>
        /// Gets or sets ReceiptType
        /// </summary>
        [Display(Name = "ReceiptType", ResourceType = typeof (ReceiptDetailResx))]
        public ReceiptType ReceiptType { get; set; }

        /// <summary>
        /// Gets or sets AllocateFromSerial
        /// </summary>
        [StringLength(40, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "AllocateFromSerial", ResourceType = typeof (ReceiptDetailResx))]
        public string AllocateFromSerial { get; set; }

        /// <summary>
        /// Gets or sets AllocateFromLot
        /// </summary>
        [StringLength(40, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "AllocateFromLot", ResourceType = typeof (ReceiptDetailResx))]
        public string AllocateFromLot { get; set; }

        /// <summary>
        /// Gets or sets SerialLotWindowHandle
        /// </summary>
        [Display(Name = "SerialLotWindowHandle", ResourceType = typeof (ReceiptDetailResx))]
        public long SerialLotWindowHandle { get; set; }

        /// <summary>
        /// Get or sets ShowFinder
        /// </summary>
        [IgnoreExportImport]
        public bool ShowFinder { get; set; }

        /// <summary>
        /// Gets OptionalFieldString 
        /// </summary>
        [IgnoreExportImport]
        public string OptionalFieldString
        {
            get
            {
                return OptionalFields > 0 ? EnumUtility.GetStringValue(AllowBlank.Yes) : EnumUtility.GetStringValue(AllowBlank.No);
            }
        }

        #region UI Strings

        /// <summary>
        /// Gets ProcessCommand string value
        /// </summary>
        public string ProcessCommandString
        {
         get { return EnumUtility.GetStringValue(ProcessCommand); }
        }

        /// <summary>
        /// Gets ReceiptType string value
        /// </summary>
        public string ReceiptTypeString
        {
         get { return EnumUtility.GetStringValue(ReceiptType); }
        }

        #endregion
    }
}