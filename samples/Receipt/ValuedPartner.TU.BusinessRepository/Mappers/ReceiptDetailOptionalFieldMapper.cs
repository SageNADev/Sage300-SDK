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
using System;
using ValuedParter.TU.Models;
using TypeEnum = ValuedParter.TU.Models.Enums;

#endregion

namespace ValuedParter.TU.BusinessRepository.Mappers
{
     /// <summary>
     /// Class for Receipt Detail Optional Field mapping
     /// </summary>
    public class ReceiptDetailOptionalFieldMapper: ModelMapper<ReceiptDetailOptionalField>  
     {
          #region Constructor

          /// <summary>
          /// Constructor to set the Context
          /// </summary>
          /// <param name="context">Context</param>
          public ReceiptDetailOptionalFieldMapper(Context context) 
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
          public override ReceiptDetailOptionalField Map(IBusinessEntity entity)
          {
               var model = base.Map(entity);

               model.SequenceNumber = entity.GetValue<long>(ReceiptDetailOptionalField.Index.SequenceNumber);
               model.LineNumber = entity.GetValue<int>(ReceiptDetailOptionalField.Index.LineNumber);
               model.OptionalField = entity.GetValue<string>(ReceiptDetailOptionalField.Index.OptionalField);
               model.Value = entity.GetValue<string>(ReceiptDetailOptionalField.Index.Value);
               model.Type = EnumUtility.GetEnum<TypeEnum.Type>(entity.GetValue<string>(ReceiptDetailOptionalField.Index.Type));
               model.Length = entity.GetValue<int>(ReceiptDetailOptionalField.Index.Length);
               model.Decimals = entity.GetValue<int>(ReceiptDetailOptionalField.Index.Decimals);
               model.AllowBlank = (TypeEnum.AllowBlank)(entity.GetValue<int>(ReceiptDetailOptionalField.Index.AllowBlank));
               model.Validate = (TypeEnum.Validate)(entity.GetValue<int>(ReceiptDetailOptionalField.Index.Validate));
               model.ValueSet = (TypeEnum.ValueSet)(entity.GetValue<int>(ReceiptDetailOptionalField.Index.ValueSet));
               model.TypedValueFieldIndex = entity.GetValue<long>(ReceiptDetailOptionalField.Index.TypedValueFieldIndex);
               model.TextValue = entity.GetValue<string>(ReceiptDetailOptionalField.Index.TextValue);
               model.AmountValue = entity.GetValue<decimal>(ReceiptDetailOptionalField.Index.AmountValue);
               model.NumberValue = entity.GetValue<decimal>(ReceiptDetailOptionalField.Index.NumberValue);
               model.IntegerValue = entity.GetValue<long>(ReceiptDetailOptionalField.Index.IntegerValue);
               model.YesNoValue = (TypeEnum.YesNoValue)(entity.GetValue<int>(ReceiptDetailOptionalField.Index.YesNoValue));
               model.DateValue = entity.GetValue<DateTime>(ReceiptDetailOptionalField.Index.DateValue);
               model.TimeValue = entity.GetValue<DateTime>(ReceiptDetailOptionalField.Index.TimeValue);
               model.OptionalFieldDescription = entity.GetValue<string>(ReceiptDetailOptionalField.Index.OptionalFieldDescription);
               model.ValueDescription = entity.GetValue<string>(ReceiptDetailOptionalField.Index.ValueDescription);

               return model;
          }

          /// <summary>
          /// SetValue Mapper
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void Map(ReceiptDetailOptionalField model, IBusinessEntity entity)
          {
               if (model == null)
               {
                    return;
               }

               entity.SetValue(ReceiptDetailOptionalField.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailOptionalField.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailOptionalField.Index.OptionalField, model.OptionalField);
               
               if (model.ValueSet == TypeEnum.ValueSet.No) return;
               switch (model.Type)
               {
                   case Models.Enums.Type.Text:
                       entity.SetValue(ReceiptDetailOptionalField.Index.TextValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.Integer:
                       entity.SetValue(ReceiptDetailOptionalField.Index.IntegerValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.Amount:
                       entity.SetValue(ReceiptDetailOptionalField.Index.AmountValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.Number:
                       entity.SetValue(ReceiptDetailOptionalField.Index.NumberValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.Date:
                       if (string.IsNullOrEmpty(model.Value))
                       {
                           model.Value = null;
                       }
                       entity.SetValue(ReceiptDetailOptionalField.Index.DateValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.Time:
                       entity.SetValue(ReceiptDetailOptionalField.Index.TimeValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
                   case Models.Enums.Type.YesOrNo:
                       entity.SetValue(ReceiptDetailOptionalField.Index.YesNoValue, model.Value, model.Validate == TypeEnum.Validate.Yes);
                       break;
               }
          }

          /// <summary>
          /// Map Key
          /// </summary>
          /// <param name="model">Model</param>
          /// <param name="entity">Business Entity</param>
          public override void MapKey(ReceiptDetailOptionalField model, IBusinessEntity entity)
          {
               entity.SetValue(ReceiptDetailOptionalField.Index.SequenceNumber, model.SequenceNumber);
               entity.SetValue(ReceiptDetailOptionalField.Index.LineNumber, model.LineNumber);
               entity.SetValue(ReceiptDetailOptionalField.Index.OptionalField, model.OptionalField);
          }

          #endregion
     }
}
