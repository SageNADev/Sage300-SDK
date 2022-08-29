// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
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

            model.FromCustomerNumber = entity.GetValue<string>(ClearStatistics.Index.FromCustomerNumber);
            model.ToCustomerNumber = entity.GetValue<string>(ClearStatistics.Index.ToCustomerNumber);
            model.FromGroupCode = entity.GetValue<string>(ClearStatistics.Index.FromGroupCode);
            model.ToGroupCode = entity.GetValue<string>(ClearStatistics.Index.ToGroupCode);
            model.FromNationalAccount = entity.GetValue<string>(ClearStatistics.Index.FromNationalAccount);
            model.ToNationalAccount = entity.GetValue<string>(ClearStatistics.Index.ToNationalAccount);
            model.FromSalesperson = entity.GetValue<string>(ClearStatistics.Index.FromSalesperson);
            model.ToSalesperson = entity.GetValue<string>(ClearStatistics.Index.ToSalesperson);
            model.FromItemNumber = entity.GetValue<string>(ClearStatistics.Index.FromItemNumber);
            model.ToItemNumber = entity.GetValue<string>(ClearStatistics.Index.ToItemNumber);
			model.ClearCustomerStatistics = (ClearCustomerStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearCustomerStatistics));
			model.ClearGroupStatistics = (ClearGroupStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearGroupStatistics));
			model.ClearNationalAccountStatistics = (ClearNationalAccountStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearNationalAccountStatistics));
			model.ClearSalespersonStatistics = (ClearSalespersonStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearSalespersonStatistics));
			model.ClearItemStatistics = (ClearItemStatistics)(entity.GetValue<int>(ClearStatistics.Index.ClearItemStatistics));
            model.ThroughCustomerYear = entity.GetValue<string>(ClearStatistics.Index.ThroughCustomerYear);
            model.ThroughCustomerPeriod = (ThroughCustomerPeriod)(entity.GetValue<int>(ClearStatistics.Index.ThroughCustomerPeriod));
            model.ThroughNationalAccountYear = entity.GetValue<string>(ClearStatistics.Index.ThroughNationalAccountYear);
            model.ThroughNationalAccountPeriod = (ThroughNationalAccountPeriod)(entity.GetValue<int>(ClearStatistics.Index.ThroughNationalAccountPeriod));
            model.ThroughGroupYear = entity.GetValue<string>(ClearStatistics.Index.ThroughGroupYear);
            model.ThroughGroupPeriod = (ThroughGroupPeriod)(entity.GetValue<int>(ClearStatistics.Index.ThroughGroupPeriod));
            model.ThroughSalespersonYear = entity.GetValue<string>(ClearStatistics.Index.ThroughSalespersonYear);
            model.ThroughSalespersonPeriod = (ThroughSalespersonPeriod)(entity.GetValue<int>(ClearStatistics.Index.ThroughSalespersonPeriod));
            model.ThroughItemYear = entity.GetValue<string>(ClearStatistics.Index.ThroughItemYear);
            model.ThroughItemPeriod = (ThroughItemPeriod)(entity.GetValue<int>(ClearStatistics.Index.ThroughItemPeriod));
            return model;
        }

        /// <summary>
        /// Set Mapper
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
                entity.SetValue(ClearStatistics.Index.FromCustomerNumber, model.FromCustomerNumber);
                entity.SetValue(ClearStatistics.Index.ToCustomerNumber, model.ToCustomerNumber);
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

            if (model.ClearNationalAccountStatistics == ClearNationalAccountStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearNationalAccountStatistics,
                    model.ClearNationalAccountStatistics);
                entity.SetValue(ClearStatistics.Index.FromNationalAccount, model.FromNationalAccount);
                entity.SetValue(ClearStatistics.Index.ToNationalAccount, model.ToNationalAccount);
                entity.SetValue(ClearStatistics.Index.ThroughNationalAccountYear, model.ThroughNationalAccountYear);
                entity.SetValue(ClearStatistics.Index.ThroughNationalAccountPeriod,
                    model.ThroughNationalAccountPeriod);
            }

            if (model.ClearSalespersonStatistics == ClearSalespersonStatistics.Yes)
            {
                entity.SetValue(ClearStatistics.Index.ClearSalespersonStatistics,
                    model.ClearSalespersonStatistics);
                entity.SetValue(ClearStatistics.Index.FromSalesperson, model.FromSalesperson);
                entity.SetValue(ClearStatistics.Index.ToSalesperson, model.ToSalesperson);
                entity.SetValue(ClearStatistics.Index.ThroughSalespersonYear, model.ThroughSalespersonYear);
                entity.SetValue(ClearStatistics.Index.ThroughSalespersonPeriod,
                    model.ThroughSalespersonPeriod);
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
