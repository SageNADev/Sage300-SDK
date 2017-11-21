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

using ACCPAC.Advantage;
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
    /// Class for ReceiptHeader mapping
    /// </summary>
    public class ReceiptHeaderMapper:ModelMapper<ReceiptHeader>  
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptHeaderMapper(Context context)
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
        public override ReceiptHeader Map(IBusinessEntity entity)
        {
            var model = base.Map(entity);

            model.SequenceNumber = entity.GetValue<long>(ReceiptHeader.Index.SequenceNumber);
            model.Description = entity.GetValue<string>(ReceiptHeader.Index.Description);
            model.ReceiptDate = entity.GetValue<DateTime>(ReceiptHeader.Index.ReceiptDate);
            model.FiscalYear = entity.GetValue<string>(ReceiptHeader.Index.FiscalYear);
			model.FiscalPeriod = (Models.Enums.FiscalPeriod)(entity.GetValue<int>(ReceiptHeader.Index.FiscalPeriod));
            model.PurchaseOrderNumber = entity.GetValue<string>(ReceiptHeader.Index.PurchaseOrderNumber);
            model.Reference = entity.GetValue<string>(ReceiptHeader.Index.Reference);
			model.ReceiptType = (ReceiptType)(entity.GetValue<int>(ReceiptHeader.Index.ReceiptType));
			model.RateOperation = (RateOperation)(entity.GetValue<int>(ReceiptHeader.Index.RateOperation));
            model.VendorNumber = entity.GetValue<string>(ReceiptHeader.Index.VendorNumber);
            model.ReceiptCurrency = entity.GetValue<string>(ReceiptHeader.Index.ReceiptCurrency);
            model.ExchangeRate = entity.GetValue<decimal>(ReceiptHeader.Index.ExchangeRate);
            model.RateType = entity.GetValue<string>(ReceiptHeader.Index.RateType);
            model.RateDate = entity.GetValue<DateTime>(ReceiptHeader.Index.RateDate);
			model.RateOverride = (RateOverride)(entity.GetValue<int>(ReceiptHeader.Index.RateOverride));
            model.AdditionalCost = entity.GetValue<decimal>(ReceiptHeader.Index.AdditionalCost);
            model.OrigAdditionalCostFunc = entity.GetValue<decimal>(ReceiptHeader.Index.OrigAdditionalCostFunc);
            model.OrigAdditionalCostSource = entity.GetValue<decimal>(ReceiptHeader.Index.OrigAdditionalCostSource);
            model.AdditionalCostCurrency = entity.GetValue<string>(ReceiptHeader.Index.AdditionalCostCurrency);
            model.TotalExtendedCostFunctional = entity.GetValue<decimal>(ReceiptHeader.Index.TotalExtendedCostFunctional);
            model.TotalExtendedCostSource = entity.GetValue<decimal>(ReceiptHeader.Index.TotalExtendedCostSource);
            model.TotalExtendedCostAdjusted = entity.GetValue<decimal>(ReceiptHeader.Index.TotalExtendedCostAdjusted);
            model.TotalAdjustedCostFunctional = entity.GetValue<decimal>(ReceiptHeader.Index.TotalAdjustedCostFunctional);
            model.TotalReturnCost = entity.GetValue<decimal>(ReceiptHeader.Index.TotalReturnCost);
            model.NumberOfDetailswithCost = entity.GetValue<int>(ReceiptHeader.Index.NumberOfDetailswithCost);
			model.RequireLabels = (RequireLabels)(entity.GetValue<int>(ReceiptHeader.Index.RequireLabels));
			model.AdditionalCostAllocationType = (AdditionalCostAllocationType)(entity.GetValue<int>(ReceiptHeader.Index.AdditionalCostAllocationType));
			model.Complete = (Complete)(entity.GetValue<int>(ReceiptHeader.Index.Complete));
            model.OriginalTotalCostSource = entity.GetValue<decimal>(ReceiptHeader.Index.OriginalTotalCostSource);
            model.OriginalTotalCostFunctional = entity.GetValue<decimal>(ReceiptHeader.Index.OriginalTotalCostFunctional);
            model.AdditionalCostFunctional = entity.GetValue<decimal>(ReceiptHeader.Index.AdditionalCostFunctional);
            model.TotalCostReceiptAdditional = entity.GetValue<decimal>(ReceiptHeader.Index.TotalCostReceiptAdditional);
            model.TotalAdjCostReceiptAddl = entity.GetValue<decimal>(ReceiptHeader.Index.TotalAdjCostReceiptAddl);
            model.ReceiptCurrencyDecimals = entity.GetValue<int>(ReceiptHeader.Index.ReceiptCurrencyDecimals);
            model.VendorShortName = entity.GetValue<string>(ReceiptHeader.Index.VendorShortName);
            model.ICUniqueDocumentNumber = entity.GetValue<decimal>(ReceiptHeader.Index.ICUniqueDocumentNumber);
			model.VendorExists = (VendorExists)(entity.GetValue<int>(ReceiptHeader.Index.VendorExists));
			model.RecordDeleted = (RecordDeleted)(entity.GetValue<int>(ReceiptHeader.Index.RecordDeleted));
            model.TransactionNumber = entity.GetValue<decimal>(ReceiptHeader.Index.TransactionNumber);
			model.RecordStatus = (RecordStatus)(entity.GetValue<int>(ReceiptHeader.Index.RecordStatus));
            model.ReceiptNumber = entity.GetValue<string>(ReceiptHeader.Index.ReceiptNumber);
            model.NextDetailLineNumber = entity.GetValue<int>(ReceiptHeader.Index.NextDetailLineNumber);
			model.RecordPrinted = (RecordPrinted)(entity.GetValue<int>(ReceiptHeader.Index.RecordPrinted));
            model.PostSequenceNumber = entity.GetValue<long>(ReceiptHeader.Index.PostSequenceNumber);
            model.OptionalFields = entity.GetValue<long>(ReceiptHeader.Index.OptionalFields);
			model.ProcessCommand = (ProcessCommand)(entity.GetValue<int>(ReceiptHeader.Index.ProcessCommand));
            model.VendorName = entity.GetValue<string>(ReceiptHeader.Index.VendorName);
            model.EnteredBy = entity.GetValue<string>(ReceiptHeader.Index.EnteredBy);
            model.PostingDate = entity.GetValue<DateTime>(ReceiptHeader.Index.PostingDate);
            return model;
        }

        /// <summary>
        /// Set Mapper
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void Map(ReceiptHeader model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            entity.SystemAccess = ViewSystemAccess.Activation;
            entity.SetValue(ReceiptHeader.Index.SequenceNumber, model.SequenceNumber);
            entity.SetValue(ReceiptHeader.Index.ReceiptNumber, model.ReceiptNumber);
            entity.SetValue(ReceiptHeader.Index.Description, model.Description);
            entity.SetValue(ReceiptHeader.Index.ReceiptDate, model.ReceiptDate);
            entity.SetValue(ReceiptHeader.Index.FiscalYear, model.FiscalYear);
            entity.SetValue(ReceiptHeader.Index.FiscalPeriod, model.FiscalPeriod);
            entity.SetValue(ReceiptHeader.Index.PurchaseOrderNumber, model.PurchaseOrderNumber);
            entity.SetValue(ReceiptHeader.Index.Reference, model.Reference);
            entity.SetValue(ReceiptHeader.Index.ReceiptCurrency, model.ReceiptCurrency, true);
            entity.SetValue(ReceiptHeader.Index.AdditionalCost, model.AdditionalCost, true);
            entity.SetValue(ReceiptHeader.Index.AdditionalCostCurrency, model.AdditionalCostCurrency, true);
            entity.SetValue(ReceiptHeader.Index.VendorNumber, model.VendorNumber, true);
            entity.SetValue(ReceiptHeader.Index.RateType, model.RateType, true);
            entity.SetValue(ReceiptHeader.Index.RateDate, model.RateDate, true);
            entity.SetValue(ReceiptHeader.Index.ExchangeRate, model.ExchangeRate, true);
            entity.SetValue(ReceiptHeader.Index.RateOverride, model.RateOverride);
            entity.SetValue(ReceiptHeader.Index.RequireLabels, model.RequireLabels);
            entity.SetValue(ReceiptHeader.Index.Complete, model.Complete);
            entity.SetValue(ReceiptHeader.Index.ICUniqueDocumentNumber, model.ICUniqueDocumentNumber);
            entity.SetValue(ReceiptHeader.Index.RecordDeleted, model.RecordDeleted);
            entity.SetValue(ReceiptHeader.Index.RecordStatus, model.RecordStatus);
            entity.SetValue(ReceiptHeader.Index.RecordPrinted, model.RecordPrinted);
            entity.SetValue(ReceiptHeader.Index.EnteredBy, model.EnteredBy);
            entity.SetValue(ReceiptHeader.Index.PostingDate, model.PostingDate);
            if (model.ReceiptType != ReceiptType.Receipt)
            {
                entity.SetValue(ReceiptHeader.Index.ReceiptType, model.ReceiptType);
            }
        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void MapKey(ReceiptHeader model, IBusinessEntity entity)
        {
            entity.SetValue(ReceiptHeader.Index.SequenceNumber, model.SequenceNumber);
            entity.SetValue(ReceiptHeader.Index.ReceiptNumber, model.ReceiptNumber);
        }

        #endregion
    }
}