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
using ValuedParter.TU.Models;
using Enums = ValuedParter.TU.Models.Enums;
using System;

#endregion

namespace ValuedParter.TU.BusinessRepository.Mappers
{
     /// <summary>
     /// Class for Receipt Optional Field mapping
     /// </summary>
     public class ReceiptOptionalFieldMapper<T> : ModelMapper<T> where T : ReceiptOptionalField, new ()
     {
          #region Constructor

          /// <summary>
          /// Constructor to set the Context
          /// </summary>
          /// <param name="context">Context</param>
          public ReceiptOptionalFieldMapper(Context context) 
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

               model.SequenceNumber = entity.GetValue<long>(ReceiptOptionalField.Index.SequenceNumber);
               model.OptionalField = entity.GetValue<string>(ReceiptOptionalField.Index.OptionalField);
               model.Value = entity.GetValue<string>(ReceiptOptionalField.Index.Value);
               model.Type = EnumUtility.GetEnum<Enums.Type>(entity.GetValue<string>(ReceiptOptionalField.Index.Type));
               model.Length = entity.GetValue<int>(ReceiptOptionalField.Index.Length);
               model.Decimals = entity.GetValue<int>(ReceiptOptionalField.Index.Decimals);
               model.AllowBlank = (entity.GetValue<string>(ReceiptOptionalField.Index.AllowBlank) == "False") ? Enums.AllowBlank.No : Enums.AllowBlank.Yes;
               model.Validate = (entity.GetValue<string>(ReceiptOptionalField.Index.Validate) == "False") ? Enums.Validate.No : Enums.Validate.Yes;
               model.ValueSet = EnumUtility.GetEnum<Enums.ValueSet>(entity.GetValue<string>(ReceiptOptionalField.Index.ValueSet));
               model.TypedValueFieldIndex = entity.GetValue<long>(ReceiptOptionalField.Index.TypedValueFieldIndex);
               model.TextValue = entity.GetValue<string>(ReceiptOptionalField.Index.TextValue);
               model.AmountValue = entity.GetValue<decimal>(ReceiptOptionalField.Index.AmountValue);
               model.NumberValue = entity.GetValue<decimal>(ReceiptOptionalField.Index.NumberValue);
               model.IntegerValue = entity.GetValue<long>(ReceiptOptionalField.Index.IntegerValue);
               model.YesNoValue = (entity.GetValue<string>(ReceiptOptionalField.Index.YesNoValue) == "False") ? Enums.YesNoValue.No : Enums.YesNoValue.Yes;
               model.DateValue = entity.GetValue<DateTime>(ReceiptOptionalField.Index.DateValue);
               model.TimeValue = entity.GetValue<DateTime>(ReceiptOptionalField.Index.TimeValue);
               model.OptionalFieldDescription = entity.GetValue<string>(ReceiptOptionalField.Index.OptionalFieldDescription);
               model.ValueDescription = entity.GetValue<string>(ReceiptOptionalField.Index.ValueDescription);

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

               entity.SetValue(ReceiptOptionalField.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptOptionalField.Index.OptionalField, model.OptionalField);

               if (model.ValueSet == Enums.ValueSet.No) return;
               switch (model.Type)
               {
                   case Enums.Type.Text:
                       entity.SetValue(ReceiptOptionalField.Index.TextValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.Integer:
                       entity.SetValue(ReceiptOptionalField.Index.IntegerValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.Amount:
                       entity.SetValue(ReceiptOptionalField.Index.AmountValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.Number:
                       entity.SetValue(ReceiptOptionalField.Index.NumberValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.Date:
                       if (model.Value == "")
                       {
                           model.Value = null;
                       }
                       entity.SetValue(ReceiptOptionalField.Index.DateValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.Time:
                       entity.SetValue(ReceiptOptionalField.Index.TimeValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
                   case Enums.Type.YesOrNo:
                       entity.SetValue(ReceiptOptionalField.Index.YesNoValue, model.Value, model.Validate == Enums.Validate.Yes);
                       break;
               }
               
          }

          /// <summary>
          /// Map Key
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void MapKey(T model, IBusinessEntity entity)
          {
               entity.SetValue(ReceiptOptionalField.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptOptionalField.Index.OptionalField, model.OptionalField);
          }

          #endregion
     }
}
