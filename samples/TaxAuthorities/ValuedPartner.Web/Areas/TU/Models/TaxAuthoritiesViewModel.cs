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
using System.Collections;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.CS.Models;
#endregion

namespace ValuedPartner.Web.Areas.TU.Models
{
    /// <summary>
    /// Class for TaxAuthoritiesViewModel
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="TaxAuthorities"/></typeparam>
    public class TaxAuthoritiesViewModel<T> : ViewModelBase<T> 
        where T : TaxAuthorities, new()
    {
        /// <summary>
        /// TaxBase list
        /// </summary>
        public IEnumerable TaxBases
        {
            get { return EnumUtility.GetItems<TaxBase>(); }
        }

        /// <summary>
        /// AllowTaxInPrice list
        /// </summary>
        public IEnumerable AllowTaxInPrices
        {
            get { return EnumUtility.GetItems<AllowTaxInPrice>(); }
        }

        /// <summary>
        /// ReportLevel list
        /// </summary>
        public IEnumerable ReportLevels
        {
            get { return EnumUtility.GetItems<ReportLevel>(); }
        }

        /// <summary>
        /// TaxRecoverable list
        /// </summary>
        public IEnumerable TaxRecoverables
        {
            get { return EnumUtility.GetItems<TaxRecoverable>(); }
        }

        /// <summary>
        /// ExpenseSeparately list
        /// </summary>
        public IEnumerable ExpenseSeparatelies
        {
            get { return EnumUtility.GetItems<ExpenseSeparately>(); }
        }

        /// <summary>
        /// TaxType list
        /// </summary>
        public IEnumerable TaxTypes
        {
            get { return EnumUtility.GetItems<TaxType>(); }
        }

        /// <summary>
        /// ReportTaxonRetainageDocument list
        /// </summary>
        public IEnumerable ReportTaxonRetainageDocuments
        {
            get { return EnumUtility.GetItems<ReportTaxonRetainageDocument>(); }
        }

        /// <summary>
        /// Gets or sets the currency description.
        /// </summary>
        /// <value>
        /// The currency description.
        /// </value>
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Gets or sets the liability account description.
        /// </summary>
        /// <value>
        /// The liability account description.
        /// </value>
        public string LiabilityAccountDescription { get; set; }

        /// <summary>
        /// Gets or sets the recoverable tax account description.
        /// </summary>
        /// <value>
        /// The recoverable tax account description.
        /// </value>
        public string RecoverableTaxAccountDescription { get; set; }

        /// <summary>
        /// Gets or sets the expense account description.
        /// </summary>
        /// <value>
        /// The expense account description.
        /// </value>
        public string ExpenseAccountDescription { get; set; }

        /// <summary>
        /// Formatted Last Maintained Date 
        /// </summary>
        public string LastMaintainedString
        {
            get
            {
                return Data != null ? DateUtil.GetShortDate(Data.LastMaintained, string.Empty) : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the currencydecimals.
        /// </summary>
        /// <value>
        /// The currencydecimals.
        /// </value>
        public string CurrencyDecimals { get; set; }

        /// <summary>
        /// Gets or sets Company Profile
        /// </summary>
        public CompanyProfile CompanyProfile { get; set; }
    }

}