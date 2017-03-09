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

using ACCPAC.Advantage;
using ISV1.web.Areas.CU.DAL.CustomViews.Mapper;
using ISV1.web.Areas.CU.DAL.CustomViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ISV1.web.Areas.CU.DAL.CustomViews.Repository
{
    public class CustomerRepository<T> : FlatRepository<T> where T : Customer, new()
    {
        // Entity view IDs
        private const string HeaderEntityName = "AR0024";
        private const string DetailEntityName = "AR0400";

        private IBusinessEntity _header;
        private IBusinessEntity _detail;

        /// <summary>
        /// Mappers
        /// </summary>
        private ModelMapper<Customer> _headerMapper;
        private ModelMapper<CustomerOptionalField> _detailMapper;

        public CustomerRepository(Context context)
            : base(context, new CustomerMapper<T>(context), ActiveFilter)
        {
            _headerMapper = new CustomerMapper<Customer>(context);
            _detailMapper = new CustomerOptionalFieldMapper<CustomerOptionalField>(context);
        }
        /// <summary>
        /// Open entity views
        /// </summary>
        /// <returns></returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            _header = OpenEntity(HeaderEntityName, true);
            _detail = OpenEntity(DetailEntityName);
            _header.Compose(new[] { _detail.View });
            return _header;
        }

        /// <summary>
        /// Get all header records Id(key) list
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAll()
        {
            var customerEntity = OpenEntity(HeaderEntityName);
            var list = new List<string>();
            customerEntity.Top();
            while (customerEntity.Fetch(false))
            {
                list.Add(customerEntity.GetValue<string>(FieldsIndex.CustomerNumber));
            }
            return list;
        }

        /// <summary>
        /// Get header and related details data by Id(key)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Customer GetById(string id)
        {
            CreateBusinessEntities();
            var filter = "(IDCUST = \"" + id + "\")";
            _header.Browse(filter, true);
            var headerData = (_header.Fetch(false)) ? _headerMapper.Map(_header) : null;
            
            _detail.ClearRecord();
            _detail.Browse(filter, true);
            // Add details records
            while (_detail.Fetch(false))
            {
                var detailData = _detailMapper.Map(_detail);
                headerData.CustomerOptionalFields.Add(detailData);
            }

            return headerData;
        }

        /// <summary>
        /// Save Entity information
        /// Todo: For insert, need meet business rules and supply valid fields
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override T Save(T model)
        {
            var filter = "(IDCUST = \"" + model.CustomerNumber + "\")";
            CreateBusinessEntities();
            // Update/insert header field
            _header.Browse(filter, true);
            if (_header.Fetch(false))
            {
                _headerMapper.Map(model, _header);
                _header.Update();
            } 
            else
            {   
                _headerMapper.Map(model, _header);
                _header.Insert();
            }

            //Update/insert detail fields
            if (model.CustomerOptionalFields != null)
            {
                UpdateDetailFields(model);
            }

            return model;
        }

        /// <summary>
        /// Delet header reocord and related details record
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(string id)
        {
            CreateBusinessEntities();
            var filter = "(IDCUST = \"" + id + "\")";

            _detail.FilterDelete(filter, ViewFilterStrictness.Simulate);
            _header.FilterDelete(filter, ViewFilterStrictness.Simulate);
        }

        /// <summary>
        /// Update detail entity data, 
        /// Todo: For insert, need meet business rules and supply valid fields
        /// </summary>
        /// <param name="model"></param>
        private void UpdateDetailFields(T model)
        {
            if (model.CustomerOptionalFields != null)
            {
                foreach (var item in model.CustomerOptionalFields)
                {
                     var filter = "(IDCUST = \"" + item.CustomerNumber + "\") AND (OPTFIELD = \"" + item.OptionalField + "\")";
                     _detail.Browse(filter, true);

                     if (_detail.Fetch(false))
                     {
                         _detailMapper.Map(item, _detail);
                         if (model.HasChanged)
                         {
                             _detail.Update();    
                         } 
                         else if (model.IsDeleted)
                         {
                             _detail.Delete();
                         } 
                     }
                     else
                     {
                         if (model.IsNewLine)
                         {
                             _detailMapper.Map(item, _detail);
                             _detail.Insert();
                         }
                     }
                }                
            }
        }

        /// <summary>
        /// Base class override implementation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return entity => entity.CustomerNumber == model.CustomerNumber;
        }

        /// <summary>
        /// Base class filter expression
        /// </summary>
        private static Expression<Func<T, Boolean>> ActiveFilter
        {
            get { return null; }
        }

    }
}