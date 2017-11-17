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
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using ValuedParter.TU.Models;
using ValuedParter.TU.Models.Enums;
using ACCPAC.Advantage;

#endregion

namespace ValuedParter.TU.BusinessRepository.Mappers
{
    /// <summary>
    /// Class for ReceiptDetail mapping
    /// </summary>
    public class ReceiptDetailMapper: ModelMapper<ReceiptDetail>  
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptDetailMapper(Context context)
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
        public override ReceiptDetail Map(IBusinessEntity entity)
        {
            var model = base.Map(entity);

            model.SequenceNumber = entity.GetValue<long>(ReceiptDetail.Index.SequenceNumber);
            model.LineNumber = entity.GetValue<int>(ReceiptDetail.Index.LineNumber);
            model.ItemNumber = entity.GetValue<string>(ReceiptDetail.Index.ItemNumber);
            model.ItemDescription = entity.GetValue<string>(ReceiptDetail.Index.ItemDescription);
            model.Category = entity.GetValue<string>(ReceiptDetail.Index.Category);
            model.Location = entity.GetValue<string>(ReceiptDetail.Index.Location);
            model.QuantityReceived = entity.GetValue<decimal>(ReceiptDetail.Index.QuantityReceived);
            model.QuantityReturned = entity.GetValue<decimal>(ReceiptDetail.Index.QuantityReturned);
            model.UnitOfMeasure = entity.GetValue<string>(ReceiptDetail.Index.UnitOfMeasure);
            model.ConversionFactor = entity.GetValue<decimal>(ReceiptDetail.Index.ConversionFactor);
            model.ProratedAddlCostFunc = entity.GetValue<decimal>(ReceiptDetail.Index.ProratedAddlCostFunc);
            model.ProratedAddlCostSrc = entity.GetValue<decimal>(ReceiptDetail.Index.ProratedAddlCostSrc);
            model.UnitCost = entity.GetValue<decimal>(ReceiptDetail.Index.UnitCost);
            model.AdjustedUnitCost = entity.GetValue<decimal>(ReceiptDetail.Index.AdjustedUnitCost);
            model.AdjustedCost = entity.GetValue<decimal>(ReceiptDetail.Index.AdjustedCost);
            model.AdjustedCostFunctional = entity.GetValue<decimal>(ReceiptDetail.Index.AdjustedCostFunctional);
            model.ExtendedCost = entity.GetValue<decimal>(ReceiptDetail.Index.ExtendedCost);
            model.ExtendedCostFunctional = entity.GetValue<decimal>(ReceiptDetail.Index.ExtendedCostFunctional);
            model.ReturnCost = entity.GetValue<decimal>(ReceiptDetail.Index.ReturnCost);
            model.CostingDate = entity.GetValue<DateTime>(ReceiptDetail.Index.CostingDate);
            model.CostingSequenceNo = entity.GetValue<long>(ReceiptDetail.Index.CostingSequenceNo);
            model.OriginalReceiptQty = entity.GetValue<decimal>(ReceiptDetail.Index.OriginalReceiptQty);
            model.OriginalUnitCost = entity.GetValue<decimal>(ReceiptDetail.Index.OriginalUnitCost);
            model.OriginalExtendedCost = entity.GetValue<decimal>(ReceiptDetail.Index.OriginalExtendedCost);
            model.OriginalExtendedCostFunc = entity.GetValue<decimal>(ReceiptDetail.Index.OriginalExtendedCostFunc);
            model.Comments = entity.GetValue<string>(ReceiptDetail.Index.Comments);
            model.Labels = entity.GetValue<int>(ReceiptDetail.Index.Labels);
            model.StockItem = entity.GetValue<bool>(ReceiptDetail.Index.StockItem);
            model.ManufacturersItemNumber = entity.GetValue<string>(ReceiptDetail.Index.ManufacturersItemNumber);
            model.VendorItemNumber = entity.GetValue<string>(ReceiptDetail.Index.VendorItemNumber);
            model.DetailLineNumber = entity.GetValue<int>(ReceiptDetail.Index.DetailLineNumber);
            model.QuantityReturnedToDate = entity.GetValue<decimal>(ReceiptDetail.Index.QuantityReturnedToDate);
            model.ReturnedExtCostToDate = entity.GetValue<decimal>(ReceiptDetail.Index.ReturnedExtCostToDate);
            model.ReturnedExtCostFuncToDate = entity.GetValue<decimal>(ReceiptDetail.Index.ReturnedExtCostFuncToDate);
            model.AdjustedExtCostToDate = entity.GetValue<decimal>(ReceiptDetail.Index.AdjustedExtCostToDate);
            model.AdjustedExtCostFuncToDate = entity.GetValue<decimal>(ReceiptDetail.Index.AdjustedExtCostFuncToDate);
            model.PreviousDayEndReceiptQty = entity.GetValue<decimal>(ReceiptDetail.Index.PreviousDayEndReceiptQty);
            model.PreviousDayEndUnitCost = entity.GetValue<decimal>(ReceiptDetail.Index.PreviousDayEndUnitCost);
            model.PreviousDayEndExtCost = entity.GetValue<decimal>(ReceiptDetail.Index.PreviousDayEndExtCost);
            model.PreviousDayEndExtCostFunc = entity.GetValue<decimal>(ReceiptDetail.Index.PreviousDayEndExtCostFunc);
            model.UnformattedItemNumber = entity.GetValue<string>(ReceiptDetail.Index.UnformattedItemNumber);
            model.CheckBelowZero = entity.GetValue<bool>(ReceiptDetail.Index.CheckBelowZero);
            model.RevisionListLineNumber = entity.GetValue<int>(ReceiptDetail.Index.RevisionListLineNumber);
            model.InterprocessCommID = entity.GetValue<long>(ReceiptDetail.Index.InterprocessCommID);
            model.ForcePopupSN = entity.GetValue<bool>(ReceiptDetail.Index.ForcePopupSN);
            model.PopupSN = entity.GetValue<int>(ReceiptDetail.Index.PopupSN);
            model.CloseSN = entity.GetValue<bool>(ReceiptDetail.Index.CloseSN);
            model.LTSetID = entity.GetValue<long>(ReceiptDetail.Index.LTSetID);
            model.ForcePopupLT = entity.GetValue<bool>(ReceiptDetail.Index.ForcePopupLT);
            model.PopupLT = entity.GetValue<int>(ReceiptDetail.Index.PopupLT);
            model.CloseLT = entity.GetValue<bool>(ReceiptDetail.Index.CloseLT);
            model.OptionalFields = entity.GetValue<long>(ReceiptDetail.Index.OptionalFields);
			model.ProcessCommand = (ProcessCommand)(entity.GetValue<int>(ReceiptDetail.Index.ProcessCommand));
            model.SerialQuantity = entity.GetValue<long>(ReceiptDetail.Index.SerialQuantity);
            model.LotQuantity = entity.GetValue<decimal>(ReceiptDetail.Index.LotQuantity);
            model.SerialQuantityReturned = entity.GetValue<long>(ReceiptDetail.Index.SerialQuantityReturned);
            model.LotQuantityReturned = entity.GetValue<decimal>(ReceiptDetail.Index.LotQuantityReturned);
            model.SerialLotQuantityToProcess = entity.GetValue<decimal>(ReceiptDetail.Index.SerialLotQuantityToProcess);
            model.NumberOfLotsToGenerate = entity.GetValue<decimal>(ReceiptDetail.Index.NumberOfLotsToGenerate);
            model.QuantityperLot = entity.GetValue<decimal>(ReceiptDetail.Index.QuantityperLot);
			model.ReceiptType = (ReceiptType)(entity.GetValue<int>(ReceiptDetail.Index.ReceiptType));
            model.AllocateFromSerial = entity.GetValue<string>(ReceiptDetail.Index.AllocateFromSerial);
            model.AllocateFromLot = entity.GetValue<string>(ReceiptDetail.Index.AllocateFromLot);
            model.SerialLotWindowHandle = entity.GetValue<long>(ReceiptDetail.Index.SerialLotWindowHandle);
            return model;
        }

        /// <summary>
        /// Set Mapper
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void Map(ReceiptDetail model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            entity.SystemAccess = ViewSystemAccess.Activation;
            entity.SetValue(ReceiptDetail.Index.LineNumber, model.LineNumber);
            entity.SetValue(ReceiptDetail.Index.ItemNumber, model.ItemNumber);
            entity.SetValue(ReceiptDetail.Index.ItemDescription, model.ItemDescription);
            entity.SetValue(ReceiptDetail.Index.Category, model.Category);
            entity.SetValue(ReceiptDetail.Index.Location, model.Location);
            entity.SetValue(ReceiptDetail.Index.QuantityReceived, model.QuantityReceived);
            entity.SetValue(ReceiptDetail.Index.QuantityReturned, model.QuantityReturned);
            entity.SetValue(ReceiptDetail.Index.UnitOfMeasure, model.UnitOfMeasure);
            entity.SetValue(ReceiptDetail.Index.UnitCost, model.UnitCost);
            entity.SetValue(ReceiptDetail.Index.AdjustedUnitCost, model.AdjustedUnitCost);
            entity.SetValue(ReceiptDetail.Index.AdjustedCost, model.AdjustedCost);
            entity.SetValue(ReceiptDetail.Index.ExtendedCost, model.ExtendedCost);
            entity.SetValue(ReceiptDetail.Index.ReturnCost, model.ReturnCost);
            entity.SetValue(ReceiptDetail.Index.Comments, model.Comments);
            entity.SetValue(ReceiptDetail.Index.Labels, model.Labels);
            entity.SetValue(ReceiptDetail.Index.VendorItemNumber, model.VendorItemNumber);
            entity.SetValue(ReceiptDetail.Index.DetailLineNumber, model.DetailLineNumber);
            entity.SetValue(ReceiptDetail.Index.UnformattedItemNumber, model.UnformattedItemNumber);
            entity.SetValue(ReceiptDetail.Index.CheckBelowZero, model.CheckBelowZero);
            entity.SetValue(ReceiptDetail.Index.RevisionListLineNumber, model.RevisionListLineNumber);
            entity.SetValue(ReceiptDetail.Index.InterprocessCommID, model.InterprocessCommID);
            entity.SetValue(ReceiptDetail.Index.ForcePopupSN, model.ForcePopupSN);
            entity.SetValue(ReceiptDetail.Index.PopupSN, model.PopupSN);
            entity.SetValue(ReceiptDetail.Index.CloseSN, model.CloseSN);
            entity.SetValue(ReceiptDetail.Index.LTSetID, model.LTSetID);
            entity.SetValue(ReceiptDetail.Index.ForcePopupLT, model.ForcePopupLT);
            entity.SetValue(ReceiptDetail.Index.PopupLT, model.PopupLT);
            entity.SetValue(ReceiptDetail.Index.CloseLT, model.CloseLT);
            entity.SetValue(ReceiptDetail.Index.ReceiptType, model.ReceiptType);
            entity.SetValue(ReceiptDetail.Index.ManufacturersItemNumber, model.ManufacturersItemNumber);

        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void MapKey(ReceiptDetail model, IBusinessEntity entity)
        {
            entity.SetValue(ReceiptDetail.Index.SequenceNumber, model.SequenceNumber);
            entity.SetValue(ReceiptDetail.Index.LineNumber, model.LineNumber);
        }

        #endregion
    }
}