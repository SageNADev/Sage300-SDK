// The MIT License (MIT) 
// Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.
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

using System.Linq;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Models
{
    public class ReceiptViewModel : ViewModelBase<ReceiptHeader>  
    {

        /// <summary>
        /// Get or Set is AdditionalCost
        /// </summary>
        public IDictionary<string, object> Attributes { get; set; }


        /// <summary>
        /// Gets ReceiptType
        /// </summary>
        public IEnumerable<CustomSelectList> ReceiptType
        {
            get
            {
                var receiptType = EnumUtility.GetItemsList<ReceiptType>();
                return receiptType.Where(x => Convert.ToInt32(x.Value) > 1).ToList();
            }
        }

        /// <summary>
        /// Gets AdditionalCostAllocationType
        /// </summary>
        public IEnumerable<CustomSelectList> AdditionalCostAllocationType
        {
            get { return EnumUtility.GetItemsList<AddlCostonRcptReturns>(); }
        }

        /// <summary>
        /// Gets or sets VendorName  
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Gets or sets AddlCostCurrencyDescription  
        /// </summary>
        public string AddlCostCurrencyDescription { get; set; }

        /// <summary>
        /// Gets or sets ReceiptCurrencyDescription 
        /// </summary>
        public string ReceiptCurrencyDescription { get; set; }

        /// <summary>
        /// Gets or sets TotalCostCurrency
        /// </summary>
        [StringLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof (AnnotationsResx))]
        public string TotalCostCurrency { get; set; }

        /// <summary>
        /// Gets or sets ExtendedCostCurrency
        /// </summary>
        [StringLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof (AnnotationsResx))]
        public string ExtendedCostCurrency { get; set; }

        /// <summary>
        /// Gets or Sets Multicurrency
        /// </summary>
        public bool IsMulticurrency { get; set; }

        /// <summary>
        /// Get or sets ShowFinder
        /// </summary>
        public bool ShowFinder { get; set; }

        /// <summary>
        /// Get or sets IsExists
        /// </summary>
        public bool IsExists { get; set; }

        /// <summary>
        /// Property For IsFieldExists
        /// </summary>
        public bool IsFieldExists;

        /// <summary>
        /// Gets or sets Is Fractional Quantity
        /// </summary>
        public bool IsFracQty { get; set; }

        /// <summary>
        /// Gets or sets Fractional Decimals 
        /// </summary>
        public int FracDecimals { get; set; }

        /// <summary>
        /// Gets or sets Conversion Factor Decimals
        /// </summary>
        public int ConvFactorDecimal { get; set; }

        /// <summary>
        /// Gets or sets is Items at all location
        /// </summary>
        public bool IsItemsAtAllLoc { get; set; }

        /// <summary>
        /// Gets or sets is Receipt of non-stock items
        /// </summary>
        public bool IsReceiptofNonStock { get; set; }

        /// <summary>
        /// Gets or sets is Prompt to delete
        /// </summary>
        public bool IsPromptToDelete { get; set; }

        /// <summary>
        /// Gets or sets Functional Currency
        /// </summary>
        public string FuncCurrency { get; set; }

        /// <summary>
        /// Gets or sets Functional Decimals
        /// </summary>
        public string FuncDecimals { get; set; }

        /// <summary>
        ///  Gets or sets CalculatedCost
        /// </summary>
        public string CalculatedCost { get; set; }

        /// <summary>
        /// Check if the OB module has License OK
        /// </summary>
        public bool HasObLicense { get; set; }

        /// <summary>
        /// DefaultPostingDate
        /// </summary>
        public Sage.CA.SBS.ERP.Sage300.IC.Models.Enums.DefaultPostingDate DefaultPostingDate { get; set; }

        /// <summary>
        /// Disable screen
        /// </summary>
        public bool DisableScreen { get; set; }

        /// <summary>
        /// DefaultReceiptNumber
        /// </summary>
        public string DefaultReceiptNumber { get; set; }
    }

}