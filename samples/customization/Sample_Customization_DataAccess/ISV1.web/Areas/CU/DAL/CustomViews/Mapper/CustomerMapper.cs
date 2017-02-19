
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using ISV1.web.Areas.CU.DAL.CustomViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;


namespace ISV1.web.Areas.CU.DAL.CustomViews.Mapper
{
    public class CustomerMapper<T> : ModelMapper<T> where T : Customer, new()
    {
        public CustomerMapper(Context context) : base(context)
        {
        }

        public override void MapKey(T model, IBusinessEntity entity)
        {
            entity.SetValue(FieldsIndex.CustomerNumber, model.CustomerNumber);
        }

        public override T Map(IBusinessEntity entity)
        {
            T model = base.Map(entity);

            model.CustomerNumber = entity.GetValue<string>(FieldsIndex.CustomerNumber);
            model.ShortName = entity.GetValue<string>(FieldsIndex.ShortName);
            model.GroupCode = entity.GetValue<string>(FieldsIndex.GroupCode);
            model.CustomerName = entity.GetValue<string>(FieldsIndex.CustomerName);
            model.AddressLine1 = entity.GetValue<string>(FieldsIndex.AddressLine1);
            model.City = entity.GetValue<string>(FieldsIndex.City);
            model.StateOrProv = entity.GetValue<string>(FieldsIndex.StateOrProv);
            model.ZipOrPostalCode = entity.GetValue<string>(FieldsIndex.ZipOrPostalCode);
            model.Country = entity.GetValue<string>(FieldsIndex.Country);
            model.ContactName = entity.GetValue<string>(FieldsIndex.ContactName);
            model.PhoneNumber = entity.GetValue<string>(FieldsIndex.PhoneNumber);
            model.FaxNumber = entity.GetValue<string>(FieldsIndex.FaxNumber);
            model.Email = entity.GetValue<string>(FieldsIndex.Email);

            return model;
        }

        /// <summary>
        /// Map model fields to business entity view fields
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        public override void Map(T model, IBusinessEntity entity)
        {
            entity.SetValue(FieldsIndex.CustomerNumber, model.CustomerNumber);
            entity.SetValue(FieldsIndex.ShortName, model.ShortName);
            entity.SetValue(FieldsIndex.GroupCode, model.GroupCode);
            entity.SetValue(FieldsIndex.CustomerName, model.CustomerName);
            entity.SetValue(FieldsIndex.AddressLine1, model.AddressLine1);
            entity.SetValue(FieldsIndex.City, model.City);
            entity.SetValue(FieldsIndex.StateOrProv, model.StateOrProv);
            entity.SetValue(FieldsIndex.ZipOrPostalCode, model.ZipOrPostalCode);
            entity.SetValue(FieldsIndex.Country, model.Country);
            entity.SetValue(FieldsIndex.ContactName, model.ContactName);
            entity.SetValue(FieldsIndex.PhoneNumber, model.PhoneNumber);
            entity.SetValue(FieldsIndex.FaxNumber, model.FaxNumber);
            entity.SetValue(FieldsIndex.Email, model.Email);
        }
    }

    public class CustomerOptionalFieldMapper<T> : ModelMapper<T> where T : CustomerOptionalField, new()
    {
        public CustomerOptionalFieldMapper(Context context)
            : base(context)
        {
        }

        public override void MapKey(T model, IBusinessEntity entity)
        {
            entity.SetValue(FieldsIndex.CustomerNumber, model.CustomerNumber);
            entity.SetValue(FieldsIndex.OptionalField, model.OptionalField);
        }

        public override T Map(IBusinessEntity entity)
        {
            T model = base.Map(entity);

            model.CustomerNumber = entity.GetValue<string>(FieldsIndex.CustomerNumber);
            model.OptionalField = entity.GetValue<string>(FieldsIndex.OptionalField);
            model.OptionalFieldDescription = entity.GetValue<string>(FieldsIndex.OptionalFieldDescription);
            model.Value = entity.GetValue<string>(FieldsIndex.Value);
            model.ValueDescription = entity.GetValue<string>(FieldsIndex.ValueDescription);
            model.ValueSet = entity.GetValue<string>(FieldsIndex.AddressLine1);
            model.Decimals = entity.GetValue<Int16>(FieldsIndex.Decimals);
            model.Validate =  entity.GetValue<Int16>(FieldsIndex.Validate);

            return model;
        }

        /// <summary>
        /// Map model fields to business entity view fields
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        public override void Map(T model, IBusinessEntity entity)
        {
            entity.SetValue(FieldsIndex.CustomerNumber, model.CustomerNumber);
            entity.SetValue(FieldsIndex.OptionalField, model.OptionalField);
            entity.SetValue(FieldsIndex.OptionalFieldDescription, model.OptionalFieldDescription);
            entity.SetValue(FieldsIndex.Value, model.Value);
            entity.SetValue(FieldsIndex.ValueDescription, model.ValueDescription);
            entity.SetValue(FieldsIndex.ValueSet, model.ValueSet);
            entity.SetValue(FieldsIndex.Decimals, model.Decimals);
            entity.SetValue(FieldsIndex.Validate, model.Validate);
        }
    }

}