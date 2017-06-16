// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

using System.ComponentModel.DataAnnotations;
using Sage.CA.SBS.ERP.Sage300.AR.Resources.Forms;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using ValuedPartner.TU.Models.Enums.Process;
using ValuedPartner.TU.Resources.Process;

#endregion

namespace ValuedPartner.TU.Models.Process
{
    /// <summary>
    /// Partial class for Clear Statistics
    /// </summary>
    public partial class ClearStatistics : ModelBase
    {
        /// <summary>
        /// Gets or sets From Customer No
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string FromCustomerNo { get; set; }

        /// <summary>
        /// Gets or sets To Customer No
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ToCustomerNo { get; set; }

        /// <summary>
        /// Gets or sets From Group Code
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string FromGroupCode { get; set; }

        /// <summary>
        /// Gets or sets To Group Code
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ToGroupCode { get; set; }

        /// <summary>
        /// Gets or sets From National Account
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string FromNationalAccount { get; set; }

        /// <summary>
        /// Gets or sets To National Account
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ToNationalAccount { get; set; }

        /// <summary>
        /// Gets or sets From Sales Person
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string FromSalesPerson { get; set; }

        /// <summary>
        /// Gets or sets To Sales Person
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ToSalesPerson { get; set; }

        /// <summary>
        /// Gets or sets From Item Number
        /// </summary>
        [StringLength(16, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string FromItemNumber { get; set; }

        /// <summary>
        /// Gets or sets To Item Number
        /// </summary>
        [StringLength(16, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ToItemNumber { get; set; }

        /// <summary>
        /// Gets or sets Clear Customer Statistics
        /// </summary>
        [Display(Name = "CustomerStatistics", ResourceType = typeof(ClearStatisticsResx))]
        public ClearCustomerStatistics ClearCustomerStatistics { get; set; }

        /// <summary>
        /// Gets or sets Clear Group Statistics
        /// </summary>
        [Display(Name = "CustomerGroupStatistics", ResourceType = typeof(ARCommonResx))]
        public ClearGroupStatistics ClearGroupStatistics { get; set; }

        /// <summary>
        /// Gets or sets Clear National Acct Statistics
        /// </summary>
        [Display(Name = "NationalAccountStatistics", ResourceType = typeof(ClearStatisticsResx))]
        public ClearNationalAccountStatistics ClearNationalAcctStatistics { get; set; }

        /// <summary>
        /// Gets or sets Clear Salesperson Statistics
        /// </summary>
        [Display(Name = "SalespersonStatistics", ResourceType = typeof(ARCommonResx))]
        public ClearSalespersonStatistics ClearSalesPersonStatistics { get; set; }

        /// <summary>
        /// Gets or sets Clear Item Statistics
        /// </summary>
        [Display(Name = "ItemStatistics", ResourceType = typeof(ARCommonResx))]
        public ClearItemStatistics ClearItemStatistics { get; set; }

        /// <summary>
        /// Gets or sets Through Customer Year
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughCustomerYear { get; set; }

        /// <summary>
        /// Gets or sets Through Customer Period
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughCustomerPeriod { get; set; }

        /// <summary>
        /// Gets or sets Through National Acct Year
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughNationalAcctYear { get; set; }

        /// <summary>
        /// Gets or sets Through National Acct Period
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughNationalAcctPeriod { get; set; }

        /// <summary>
        /// Gets or sets Through Group Year
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughGroupYear { get; set; }

        /// <summary>
        /// Gets or sets Through Group Period
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughGroupPeriod { get; set; }

        /// <summary>
        /// Gets or sets Through Salesperson Year
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughSalesPersonYear { get; set; }

        /// <summary>
        /// Gets or sets Through Salesperson Period
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughSalesPersonPeriod { get; set; }

        /// <summary>
        /// Gets or sets Through Item Year
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughItemYear { get; set; }

        /// <summary>
        /// Gets or sets Through Item Period
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        public string ThroughItemPeriod { get; set; }

        #region UI Strings

        /// <summary>
        /// Gets Clear Customer Statistics string value
        /// </summary>
        public string ClearCustomerStatisticsString
        {
            get { return EnumUtility.GetStringValue(ClearCustomerStatistics); }
        }

        /// <summary>
        /// Gets Clear Group Statistics string value
        /// </summary>
        public string ClearGroupStatisticsString
        {
            get { return EnumUtility.GetStringValue(ClearGroupStatistics); }
        }

        /// <summary>
        /// Gets Clear National Acct Statistics string value
        /// </summary>
        public string ClearNationalAcctStatisticsString
        {
            get { return EnumUtility.GetStringValue(ClearNationalAcctStatistics); }
        }

        /// <summary>
        /// Gets Clear Salesperson Statistics string value
        /// </summary>
        public string ClearSalesPersonStatisticsString
        {
            get { return EnumUtility.GetStringValue(ClearSalesPersonStatistics); }
        }

        /// <summary>
        /// Gets Clear Item Statistics string value
        /// </summary>
        public string ClearItemStatisticsString
        {
            get { return EnumUtility.GetStringValue(ClearItemStatistics); }
        }

        #endregion
    }
}
