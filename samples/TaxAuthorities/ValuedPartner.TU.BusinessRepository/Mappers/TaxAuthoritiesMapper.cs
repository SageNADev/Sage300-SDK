
// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers
{
    /// <summary>
    /// Class for TaxAuthorities mapping
    /// </summary>
    /// <typeparam name="T">TaxAuthorities</typeparam>
    public class TaxAuthoritiesMapper<T> : ModelMapper<T> where T : TaxAuthorities, new ()
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public TaxAuthoritiesMapper(Context context)
            : base(context)
        {
        }

        #endregion

        #region ModelMapper methods

        /// <summary>
        /// Get Mapper
        /// </summary>
        /// <param name="entity">Business Entity</param>
        /// <returns>Mapped Model</returns>
        public override T Map(IBusinessEntity entity)
        {
            var model = base.Map(entity);

            model.TaxAuthority = entity.GetValue<string>(TaxAuthorities.Index.TaxAuthority);
            model.Description = entity.GetValue<string>(TaxAuthorities.Index.Description);
            model.TaxReportingCurrency = entity.GetValue<string>(TaxAuthorities.Index.TaxReportingCurrency);
            model.MaximumTaxAllowable = entity.GetValue<decimal>(TaxAuthorities.Index.MaximumTaxAllowable);
            model.NoTaxChargedBelow = entity.GetValue<decimal>(TaxAuthorities.Index.NoTaxChargedBelow);
            model.TaxBase = (TaxBase)(entity.GetValue<int>(TaxAuthorities.Index.TaxBase));
            model.AllowTaxInPrice = (AllowTaxInPrice)(entity.GetValue<int>(TaxAuthorities.Index.AllowTaxInPrice));
            model.TaxLiabilityAccount = entity.GetValue<string>(TaxAuthorities.Index.TaxLiabilityAccount);
            model.ReportLevel = (ReportLevel)(entity.GetValue<int>(TaxAuthorities.Index.ReportLevel));
            model.TaxRecoverable = (TaxRecoverable)(entity.GetValue<int>(TaxAuthorities.Index.TaxRecoverable));
            model.RecoverableRate = entity.GetValue<decimal>(TaxAuthorities.Index.RecoverableRate);
            model.RecoverableTaxAccount = entity.GetValue<string>(TaxAuthorities.Index.RecoverableTaxAccount);
            model.ExpenseSeparately = (ExpenseSeparately)(entity.GetValue<int>(TaxAuthorities.Index.ExpenseSeparately));
            model.ExpenseAccount = entity.GetValue<string>(TaxAuthorities.Index.ExpenseAccount);
            model.LastMaintained = entity.GetValue<DateTime>(TaxAuthorities.Index.LastMaintained);
            model.TaxType = (TaxType)(entity.GetValue<int>(TaxAuthorities.Index.TaxType));
            model.ReportTaxonRetainageDocument = (ReportTaxonRetainageDocument)(entity.GetValue<int>(TaxAuthorities.Index.ReportTaxonRetainageDocument));
            return model;
        }

        /// <summary>
        /// Set Mapper
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void Map(T model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            entity.SetValue(TaxAuthorities.Index.TaxAuthority, model.TaxAuthority);
            entity.SetValue(TaxAuthorities.Index.Description, model.Description);
            entity.SetValue(TaxAuthorities.Index.TaxReportingCurrency, model.TaxReportingCurrency);
            entity.SetValue(TaxAuthorities.Index.MaximumTaxAllowable, model.MaximumTaxAllowable);
            entity.SetValue(TaxAuthorities.Index.NoTaxChargedBelow, model.NoTaxChargedBelow);
            entity.SetValue(TaxAuthorities.Index.TaxBase, model.TaxBase);
            entity.SetValue(TaxAuthorities.Index.AllowTaxInPrice, model.AllowTaxInPrice);
            entity.SetValue(TaxAuthorities.Index.TaxLiabilityAccount, model.TaxLiabilityAccount);
            entity.SetValue(TaxAuthorities.Index.ReportLevel, model.ReportLevel);
            entity.SetValue(TaxAuthorities.Index.TaxRecoverable, model.TaxRecoverable);
            entity.SetValue(TaxAuthorities.Index.RecoverableRate, model.RecoverableRate);
            entity.SetValue(TaxAuthorities.Index.RecoverableTaxAccount, model.RecoverableTaxAccount);
            entity.SetValue(TaxAuthorities.Index.ExpenseSeparately, model.ExpenseSeparately);
            entity.SetValue(TaxAuthorities.Index.ExpenseAccount, model.ExpenseAccount);
            // entity.SetValue(TaxAuthorities.Index.LastMaintained, model.LastMaintained);
            entity.SetValue(TaxAuthorities.Index.TaxType, model.TaxType);
            entity.SetValue(TaxAuthorities.Index.ReportTaxonRetainageDocument, model.ReportTaxonRetainageDocument);
        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void MapKey(T model, IBusinessEntity entity)
        {
            entity.SetValue(TaxAuthorities.Index.TaxAuthority, model.TaxAuthority);
        }

        #endregion
    }
}