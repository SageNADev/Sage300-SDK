
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
    /// Partial class for TaxAuthorities
    /// </summary>
    public partial class TaxAuthorities : ModelBase
    {
        /// <summary>
        /// Gets or sets TaxAuthority
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [StringLength(12, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "TaxAuthority", ResourceType = typeof (TaxAuthoritiesResx))]
        public string TaxAuthority { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Description", ResourceType = typeof (TaxAuthoritiesResx))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets TaxReportingCurrency
        /// </summary>
        [StringLength(3, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "TaxReportingCurrency", ResourceType = typeof (TaxAuthoritiesResx))]
        public string TaxReportingCurrency { get; set; }

        /// <summary>
        /// Gets or sets MaximumTaxAllowable
        /// </summary>
        [Display(Name = "MaximumTaxAllowable", ResourceType = typeof (TaxAuthoritiesResx))]
        public decimal MaximumTaxAllowable { get; set; }

        /// <summary>
        /// Gets or sets NoTaxChargedBelow
        /// </summary>
        [Display(Name = "NoTaxChargedBelow", ResourceType = typeof (TaxAuthoritiesResx))]
        public decimal NoTaxChargedBelow { get; set; }

        /// <summary>
        /// Gets or sets TaxBase
        /// </summary>
        [Display(Name = "TaxBase", ResourceType = typeof (TaxAuthoritiesResx))]
        public TaxBase TaxBase { get; set; }

        /// <summary>
        /// Gets or sets AllowTaxInPrice
        /// </summary>
        [Display(Name = "AllowTaxInPrice", ResourceType = typeof (TaxAuthoritiesResx))]
        public AllowTaxInPrice AllowTaxInPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow tax price].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow tax price]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "AllowTaxInPrice", ResourceType = typeof(TaxAuthoritiesResx))]
        public bool AllowTaxPrice
        {
            get { return AllowTaxInPrice != AllowTaxInPrice.No; }
            set
            {
                AllowTaxInPrice = value
                    ? AllowTaxInPrice.Yes
                    : AllowTaxInPrice.No;
            }
        }

        /// <summary>
        /// Gets or sets TaxLiabilityAccount
        /// </summary>
        [StringLength(45, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "TaxLiabilityAccount", ResourceType = typeof (TaxAuthoritiesResx))]
        public string TaxLiabilityAccount { get; set; }

        /// <summary>
        /// Gets or sets ReportLevel
        /// </summary>
        [Display(Name = "ReportLevel", ResourceType = typeof (TaxAuthoritiesResx))]
        public ReportLevel ReportLevel { get; set; }

        /// <summary>
        /// Gets or sets TaxRecoverable
        /// </summary>
        [Display(Name = "TaxRecoverable", ResourceType = typeof (TaxAuthoritiesResx))]
        public TaxRecoverable TaxRecoverable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [recover tax].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [recover tax]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "TaxRecoverable", ResourceType = typeof(TaxAuthoritiesResx))]
        public bool TaxRecover
        {
            get { return TaxRecoverable != TaxRecoverable.No; }
            set
            {
                TaxRecoverable = value
                    ? TaxRecoverable.Yes
                    : TaxRecoverable.No;
            }
        }

        /// <summary>
        /// Gets or sets RecoverableRate
        /// </summary>
        [Display(Name = "RecoverableRate", ResourceType = typeof (TaxAuthoritiesResx))]
        public decimal RecoverableRate { get; set; }

        /// <summary>
        /// Gets or sets RecoverableTaxAccount
        /// </summary>
        [StringLength(45, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "RecoverableTaxAccount", ResourceType = typeof (TaxAuthoritiesResx))]
        public string RecoverableTaxAccount { get; set; }

        /// <summary>
        /// Gets or sets ExpenseSeparately
        /// </summary>
        [Display(Name = "ExpenseSeparately", ResourceType = typeof (TaxAuthoritiesResx))]
        public ExpenseSeparately ExpenseSeparately { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [expense separate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [expense separate]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "ExpenseSeparately", ResourceType = typeof(TaxAuthoritiesResx))]
        public bool ExpenseSeparate
        {
            get { return ExpenseSeparately != ExpenseSeparately.No; }
            set
            {
                ExpenseSeparately = value
                    ? ExpenseSeparately.Yes
                    : ExpenseSeparately.No;
            }
        }

        /// <summary>
        /// Gets or sets ExpenseAccount
        /// </summary>
        [StringLength(45, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ExpenseAccount", ResourceType = typeof (TaxAuthoritiesResx))]
        public string ExpenseAccount { get; set; }

        /// <summary>
        /// Gets or sets LastMaintained
        /// </summary>
        [ValidateDateFormat(ErrorMessageResourceName="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "LastMaintained", ResourceType = typeof (TaxAuthoritiesResx))]
        public DateTime LastMaintained { get; set; }

        /// <summary>
        /// Gets or sets TaxType
        /// </summary>
        [Display(Name = "TaxType", ResourceType = typeof (TaxAuthoritiesResx))]
        public TaxType TaxType { get; set; }

        /// <summary>
        /// Gets or sets ReportTaxonRetainageDocument
        /// </summary>
        [Display(Name = "ReportTaxonRetainageDocument", ResourceType = typeof (TaxAuthoritiesResx))]
        public ReportTaxonRetainageDocument ReportTaxonRetainageDocument { get; set; }

        /// <summary>
        /// Get or set for  IsMultiCurrency
        /// </summary>
        public bool IsMultiCurrency { get; set; }

        #region UI Strings

        /// <summary>
        /// Gets TaxBase string value
        /// </summary>
        public string TaxBaseString
        {
         get { return EnumUtility.GetStringValue(TaxBase); }
        }

        /// <summary>
        /// Gets AllowTaxInPrice string value
        /// </summary>
        public string AllowTaxInPriceString
        {
         get { return EnumUtility.GetStringValue(AllowTaxInPrice); }
        }

        /// <summary>
        /// Gets ReportLevel string value
        /// </summary>
        public string ReportLevelString
        {
         get { return EnumUtility.GetStringValue(ReportLevel); }
        }

        /// <summary>
        /// Gets TaxRecoverable string value
        /// </summary>
        public string TaxRecoverableString
        {
         get { return EnumUtility.GetStringValue(TaxRecoverable); }
        }

        /// <summary>
        /// Gets ExpenseSeparately string value
        /// </summary>
        public string ExpenseSeparatelyString
        {
         get { return EnumUtility.GetStringValue(ExpenseSeparately); }
        }

        /// <summary>
        /// Gets TaxType string value
        /// </summary>
        public string TaxTypeString
        {
         get { return EnumUtility.GetStringValue(TaxType); }
        }

        /// <summary>
        /// Gets ReportTaxonRetainageDocument string value
        /// </summary>
        public string ReportTaxonRetainageDocumentString
        {
         get { return EnumUtility.GetStringValue(ReportTaxonRetainageDocument); }
        }

        #endregion
    }
}