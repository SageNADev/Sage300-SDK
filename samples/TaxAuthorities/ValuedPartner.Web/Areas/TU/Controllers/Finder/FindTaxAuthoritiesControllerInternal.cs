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

#region Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Finder;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Finder
{
    /// <summary>
    /// Finder class for TaxAuthorities
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="TaxAuthorities"/></typeparam>
    public class FindTaxAuthoritiesControllerInternal<T> : BaseFindControllerInternal<T, ITaxAuthoritiesService<T>>, IFinder
        where T : TaxAuthorities, new()
    {
        #region Private variables

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TaxAuthorities
        /// </summary>
        /// <param name="context">Context</param>
        public FindTaxAuthoritiesControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get first or default TaxAuthorities
        /// </summary>
        /// <param name="id">Id for TaxAuthorities</param>
        /// <returns>Get first or default TaxAuthorities</returns>
        public virtual ModelBase Get(string id)
        {
            Expression<Func<T, bool>> filter = param => param.TaxAuthority == id;
            Service.IsReadOnly = true;
            var model = Service.FirstOrDefault(filter);
            Service.IsReadOnly = false;
            return model;
        }

        /// <summary>
        /// Get the default columns
        /// </summary>
        /// <returns>Default columns</returns>
        public override List<string> GetDefaultColumns()
        {
            // TODO: All columns have been added and must be reduced to only default columns
            // TODO: Delete TODO statements when complete
            return new List<string> 
            {
                "TaxAuthority",
                "Description",
                "TaxReportingCurrency",
                "MaximumTaxAllowable",
                "NoTaxChargedBelow",
                "TaxBaseString",
                "AllowTaxInPriceString",
                "TaxLiabilityAccount",
                "ReportLevelString",
                "TaxRecoverableString",
                "RecoverableRate",
                "RecoverableTaxAccount",
                "ExpenseSeparatelyString",
                "ExpenseAccount",
                "LastMaintained",
                "TaxTypeString",
                "ReportTaxonRetainageDocumentString"
             };
       }

        /// <summary>
        /// Get all columns
        /// </summary>
        /// <returns>All columns</returns>
        public override IEnumerable<ModelBase> GetAllColumns()
        {

            var columns = new List<ModelBase>
            {
                new GridField
                {
                    field = "TaxAuthority",
                    title = TaxAuthoritiesResx.TaxAuthority,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "12"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "Description",
                    title = TaxAuthoritiesResx.Description,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "60"}
                        }
                },
                new GridField
                {
                    field = "TaxReportingCurrency",
                    title = TaxAuthoritiesResx.TaxReportingCurrency,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "3"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "MaximumTaxAllowable",
                    title = TaxAuthoritiesResx.MaximumTaxAllowable,
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            // TODO Value of 16 defaulted for max length. Modify as required
                            // TODO Delete TODO statements when complete
                            {FinderConstant.CustomAttributeMaximumLength, "16"},
                            {"class", FinderConstant.CssClassInputNumberRightAlign}
                        }
                },
                new GridField
                {
                    field = "NoTaxChargedBelow",
                    title = TaxAuthoritiesResx.NoTaxChargedBelow,
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            // TODO Value of 16 defaulted for max length. Modify as required
                            // TODO Delete TODO statements when complete
                            {FinderConstant.CustomAttributeMaximumLength, "16"},
                            {"class", FinderConstant.CssClassInputNumberRightAlign}
                        }
                },
                new GridField
                {
                    field = "TaxBaseString",
                    title = TaxAuthoritiesResx.TaxBase,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<TaxBase>()
                },
                new GridField
                {
                    field = "AllowTaxInPriceString",
                    title = TaxAuthoritiesResx.AllowTaxInPrice,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<AllowTaxInPrice>()
                },
                new GridField
                {
                    field = "TaxLiabilityAccount",
                    title = TaxAuthoritiesResx.TaxLiabilityAccount,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "45"},
                            {"class", FinderConstant.CssClassTxtUpper}
                        }
                },
                new GridField
                {
                    field = "ReportLevelString",
                    title = TaxAuthoritiesResx.ReportLevel,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<ReportLevel>()
                },
                new GridField
                {
                    field = "TaxRecoverableString",
                    title = TaxAuthoritiesResx.TaxRecoverable,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<TaxRecoverable>()
                },
                new GridField
                {
                    field = "RecoverableRate",
                    title = TaxAuthoritiesResx.RecoverableRate,
                    attributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    headerAttributes = FinderConstant.CssClassGridColumn10NumRightAlign,
                    dataType = FinderConstant.DataTypeAmount,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            // TODO Value of 16 defaulted for max length. Modify as required
                            // TODO Delete TODO statements when complete
                            {FinderConstant.CustomAttributeMaximumLength, "16"},
                            {"class", FinderConstant.CssClassInputNumberRightAlign}
                        }
                },
                new GridField
                {
                    field = "RecoverableTaxAccount",
                    title = TaxAuthoritiesResx.RecoverableTaxAccount,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "45"},
                            {"class", FinderConstant.CssClassTxtUpper}
                        }
                },
                new GridField
                {
                    field = "ExpenseSeparatelyString",
                    title = TaxAuthoritiesResx.ExpenseSeparately,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<ExpenseSeparately>()
                },
                new GridField
                {
                    field = "ExpenseAccount",
                    title = TaxAuthoritiesResx.ExpenseAccount,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "45"},
                            {"class", FinderConstant.CssClassTxtUpper}
                        }
                },
                new GridField
                {
                    field = "LastMaintained",
                    title = TaxAuthoritiesResx.LastMaintained,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeDate,
                    template = Utilities.GetGridTemplate("LastMaintained")
                },
                new GridField
                {
                    field = "TaxTypeString",
                    title = TaxAuthoritiesResx.TaxType,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<TaxType>()
                },
                new GridField
                {
                    field = "ReportTaxonRetainageDocumentString",
                    title = TaxAuthoritiesResx.ReportTaxonRetainageDocument,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<ReportTaxonRetainageDocument>()
                }
            };

            return columns.AsEnumerable();
        }
        #endregion

    }
}