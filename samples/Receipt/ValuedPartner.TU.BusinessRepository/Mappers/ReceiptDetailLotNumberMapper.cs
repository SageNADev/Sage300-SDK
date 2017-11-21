// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using System;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers
{
     /// <summary>
     /// Class for ReceiptDetailLotNumber mapping
     /// </summary>
     public class ReceiptDetailLotNumberMapper<T> : ModelMapper<T> where T : ReceiptDetailLotNumber, new ()
     {
          #region Constructor

          /// <summary>
          /// Constructor to set the Context
          /// </summary>
          /// <param name="context">Context</param>
          public ReceiptDetailLotNumberMapper(Context context) 
               : base(context)
          {
          }

          #endregion

          #region IModelMapper methods

          /// <summary>
          /// Get Mapper
          /// </summary>
          /// <param name="entity">Business Entity</param>
          /// <returns>Mapped Model</returns>
          public override T Map(IBusinessEntity entity)
          {
               var model = base.Map(entity);

               model.SequenceNumber = entity.GetValue<long>(ReceiptDetailLotNumber.Index.SequenceNumber);
               model.LineNumber = entity.GetValue<int>(ReceiptDetailLotNumber.Index.LineNumber);
               model.LotNumber = entity.GetValue<string>(ReceiptDetailLotNumber.Index.LotNumber);
               model.ExpiryDate = entity.GetValue<DateTime>(ReceiptDetailLotNumber.Index.ExpiryDate);
               model.TransactionQuantity = entity.GetValue<decimal>(ReceiptDetailLotNumber.Index.TransactionQuantity);
               model.LotQuantityInStockingUOM = entity.GetValue<decimal>(ReceiptDetailLotNumber.Index.LotQuantityInStockingUOM);
               model.LotQuantityReturned = entity.GetValue<decimal>(ReceiptDetailLotNumber.Index.LotQuantityReturned);
               model.LotQtyReturnedInStockingUOM = entity.GetValue<decimal>(ReceiptDetailLotNumber.Index.LotQtyReturnedInStockingUOM);

               return model;
          }

          /// <summary>
          /// Se tValue Mapper
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void Map(T model, IBusinessEntity entity)
          {
               if (model == null)
               {
                    return;
               }

               entity.SetValue(ReceiptDetailLotNumber.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailLotNumber.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailLotNumber.Index.LotNumber, model.LotNumber);
               entity.SetValue(ReceiptDetailLotNumber.Index.ExpiryDate, model.ExpiryDate);
               entity.SetValue(ReceiptDetailLotNumber.Index.TransactionQuantity, model.TransactionQuantity);
               entity.SetValue(ReceiptDetailLotNumber.Index.LotQuantityInStockingUOM, model.LotQuantityInStockingUOM);
               entity.SetValue(ReceiptDetailLotNumber.Index.LotQuantityReturned, model.LotQuantityReturned);
               entity.SetValue(ReceiptDetailLotNumber.Index.LotQtyReturnedInStockingUOM, model.LotQtyReturnedInStockingUOM);
          }

          /// <summary>
          /// Map Key
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void MapKey(T model, IBusinessEntity entity)
          {
               entity.SetValue(ReceiptDetailLotNumber.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailLotNumber.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailLotNumber.Index.LotNumber, model.LotNumber);
          }

          #endregion
     }
}
