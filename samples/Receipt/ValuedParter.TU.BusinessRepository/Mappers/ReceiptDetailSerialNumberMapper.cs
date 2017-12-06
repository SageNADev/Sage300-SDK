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

using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedParter.TU.Models;

#endregion

namespace ValuedParter.TU.BusinessRepository.Mappers
{
     /// <summary>
     /// Class for Receipt Detail Serial Number mapping
     /// </summary>
     public class ReceiptDetailSerialNumberMapper<T> : ModelMapper<T> where T : ReceiptDetailSerialNumber, new ()
     {
          #region Constructor

          /// <summary>
          /// Constructor to set the Context
          /// </summary>
          /// <param name="context">Context</param>
          public ReceiptDetailSerialNumberMapper(Context context) 
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

               model.SequenceNumber = entity.GetValue<long>(ReceiptDetailSerialNumber.Index.SequenceNumber);
               model.LineNumber = entity.GetValue<int>(ReceiptDetailSerialNumber.Index.LineNumber);
               model.SerialNumber = entity.GetValue<string>(ReceiptDetailSerialNumber.Index.SerialNumber);
               model.SerialReturned = entity.GetValue<bool>(ReceiptDetailSerialNumber.Index.SerialReturned);
               model.TransactionQuantity = entity.GetValue<long>(ReceiptDetailSerialNumber.Index.TransactionQuantity);
               model.SerialQuantityReturned = entity.GetValue<long>(ReceiptDetailSerialNumber.Index.SerialQuantityReturned);

               return model;
          }

          /// <summary>
          /// SetValue Mapper
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void Map(T model, IBusinessEntity entity)
          {
               if (model == null)
               {
                    return;
               }

               entity.SetValue(ReceiptDetailSerialNumber.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailSerialNumber.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailSerialNumber.Index.SerialNumber, model.SerialNumber);
               entity.SetValue(ReceiptDetailSerialNumber.Index.SerialReturned, model.SerialReturned);
               entity.SetValue(ReceiptDetailSerialNumber.Index.TransactionQuantity, model.TransactionQuantity);
               entity.SetValue(ReceiptDetailSerialNumber.Index.SerialQuantityReturned, model.SerialQuantityReturned);
          }

          /// <summary>
          /// Map Key
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void MapKey(T model, IBusinessEntity entity)
          {
               entity.SetValue(ReceiptDetailSerialNumber.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailSerialNumber.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailSerialNumber.Index.SerialNumber, model.SerialNumber);
          }

          #endregion
     }
}
