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
using ValuedPartner.TU.Models.Process;
using ValuedPartner.TU.Models.Enums.Process;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers.Process
{
    /// <summary>
    /// Class for Clear Statistics mapping
    /// </summary>
    /// <typeparam name="T">Clear Statistics</typeparam>
    public class ClearStatisticsMapper<T> : ModelMapper<T> where T : ClearStatistics, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public ClearStatisticsMapper(Context context)
            : base(context)
        {
        }

        #endregion

        #region ModelMapper methods

        /// <summary>
        /// Get Mapper
        /// </summary>
        /// <param name="entity">Clear Statistics Business Entity</param>
        /// <returns>Clear Statistics Mapped Model</returns>
        public override T Map(IBusinessEntity entity)
        {
            var model = base.Map(entity);

            model.FromCustomerNo = entity.GetValue<string>(ClearStatistics.Index.FromCustomerNo);
            model.ToCustomerNo = entity.GetValue<string>(ClearStatistics.Index.ToCustomerNo);
            model.FromGroupCode = entity.GetValue<string>(ClearStatistics.Index.FromGroupCode);
            model.ToGroupCode = entity.GetValue<string>(ClearStatistics.Index.ToGroupCode);
            model.FromNationalAccount = entity.GetValue<string>(ClearStatistics.Index.FromNationalAccount);
            model.ToNationalAccount = entity.GetValue<string>(ClearStatistics.Index.ToNationalAccount);
            model.FromSalesPerson = entity.GetValue<string>(ClearStatistics.Index.FromSalesPerson);
            model.ToSalesPerson = entity.GetValue<string>(ClearStatistics.Index.ToSalesPerson);
            model.FromItemNumber = entity.GetValue<string>(ClearStatistics.Index.FromItemNumber);
            model.ToItemNumber = entity.GetValue<string>(ClearStatistics.Index.ToItemNumber);
            model.ClearCustomerStatistics = (ClearCustomerStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearCustomerStatistics));
            model.ClearGroupStatistics = (ClearGroupStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearGroupStatistics));
            model.ClearNationalAcctStatistics = (ClearNationalAccountStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearNationalAcctStatistics));
            model.ClearSalesPersonStatistics = (ClearSalespersonStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearSalesPersonStatistics));
            model.ClearItemStatistics = (ClearItemStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearItemStatistics));
            model.ThroughCustomerYear = entity.GetValue<string>(ClearStatistics.Index.ThroughCustomerYear);
            model.ThroughCustomerPeriod = entity.GetValue<string>(ClearStatistics.Index.ThroughCustomerPeriod);
            model.ThroughNationalAcctYear = entity.GetValue<string>(ClearStatistics.Index.ThroughNationalAcctYear);
            model.ThroughNationalAcctPeriod = entity.GetValue<string>(ClearStatistics.Index.ThroughNationalAcctPeriod);
            model.ThroughGroupYear = entity.GetValue<string>(ClearStatistics.Index.ThroughGroupYear);
            model.ThroughGroupPeriod = entity.GetValue<string>(ClearStatistics.Index.ThroughGroupPeriod);
            model.ThroughSalesPersonYear = entity.GetValue<string>(ClearStatistics.Index.ThroughSalesPersonYear);
            model.ThroughSalesPersonPeriod = entity.GetValue<string>(ClearStatistics.Index.ThroughSalesPersonPeriod);
            model.ThroughItemYear = entity.GetValue<string>(ClearStatistics.Index.ThroughItemYear);
            model.ThroughItemPeriod = entity.GetValue<string>(ClearStatistics.Index.ThroughItemPeriod);
            return model;
        }

        /// <summary>
        /// Set Value Mapper
        /// </summary>
        /// <param name="model">Clear Statistics Model</param>
        /// <param name="entity">Clear Statistics Business Entity</param>
        public override void Map(T model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            if (model.ClearCustomerStatistics == ClearCustomerStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearCustomerStatistics,
                    model.ClearCustomerStatistics);
                entity.SetValue(ClearStatistics.Index.FromCustomerNo, model.FromCustomerNo);
                entity.SetValue(ClearStatistics.Index.ToCustomerNo, model.ToCustomerNo);
                entity.SetValue(ClearStatistics.Index.ThroughCustomerYear, model.ThroughCustomerYear);
                entity.SetValue(ClearStatistics.Index.ThroughCustomerPeriod, model.ThroughCustomerPeriod);
            }

            if (model.ClearGroupStatistics == ClearGroupStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearGroupStatistics,
                    model.ClearGroupStatistics);
                entity.SetValue(ClearStatistics.Index.FromGroupCode, model.FromGroupCode);
                entity.SetValue(ClearStatistics.Index.ToGroupCode, model.ToGroupCode);
                entity.SetValue(ClearStatistics.Index.ThroughGroupYear, model.ThroughGroupYear);
                entity.SetValue(ClearStatistics.Index.ThroughGroupPeriod,
                    model.ThroughGroupPeriod);
            }

            if (model.ClearNationalAcctStatistics == ClearNationalAccountStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearNationalAcctStatistics,
                    model.ClearNationalAcctStatistics);
                entity.SetValue(ClearStatistics.Index.FromNationalAccount, model.FromNationalAccount);
                entity.SetValue(ClearStatistics.Index.ToNationalAccount, model.ToNationalAccount);
                entity.SetValue(ClearStatistics.Index.ThroughNationalAcctYear, model.ThroughNationalAcctYear);
                entity.SetValue(ClearStatistics.Index.ThroughNationalAcctPeriod,
                    model.ThroughNationalAcctPeriod);
            }

            if (model.ClearSalesPersonStatistics == ClearSalespersonStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearSalesPersonStatistics,
                    model.ClearSalesPersonStatistics);
                entity.SetValue(ClearStatistics.Index.FromSalesPerson, model.FromSalesPerson);
                entity.SetValue(ClearStatistics.Index.ToSalesPerson, model.ToSalesPerson);
                entity.SetValue(ClearStatistics.Index.ThroughSalesPersonYear, model.ThroughSalesPersonYear);
                entity.SetValue(ClearStatistics.Index.ThroughSalesPersonPeriod,
                    model.ThroughSalesPersonPeriod);
            }
            if (model.ClearItemStatistics == ClearItemStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearItemStatistics,
                    model.ClearItemStatistics);
                entity.SetValue(ClearStatistics.Index.FromItemNumber, model.FromItemNumber);
                entity.SetValue(ClearStatistics.Index.ToItemNumber, model.ToItemNumber);
                entity.SetValue(ClearStatistics.Index.ThroughItemYear, model.ThroughItemYear);
                entity.SetValue(ClearStatistics.Index.ThroughItemPeriod,
                    model.ThroughItemPeriod);
            }
        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Clear Statistics Model</param>
        /// <param name="entity">Clear Statistics Business Entity</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void MapKey(T model, IBusinessEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
