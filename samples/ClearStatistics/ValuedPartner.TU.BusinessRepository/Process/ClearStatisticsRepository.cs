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
using System.Collections.Generic;
using ValuedPartner.TU.BusinessRepository.Mappers.Process;
using ValuedPartner.TU.Interfaces.BusinessRepository.Process;
using ValuedPartner.TU.Models.Process;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Process
{
    /// <summary>
    /// Class for Clear Statistics Repository
    /// </summary>
    /// <typeparam name="T">Clear Statistics</typeparam>
    public class ClearStatisticsRepository<T> : ProcessingRepository<T>,
        IClearStatisticsEntity<T> where T : ClearStatistics, new()
    {
        #region Business Entity

        /// <summary>
        /// Clear Statistics Business Entity
        /// </summary>
        private IBusinessEntity _clearStatisticsBusinessEntity;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Clear Statistics Repository
        /// </summary>
        /// <param name="context">Request Context</param>
        public ClearStatisticsRepository(Context context)
            : base(context, new ClearStatisticsMapper<T>(context), ObjectPoolType.Default, BusinessEntitySessionParams.Get(context))
        {
        }

        /// <summary>
        /// Constructor for Clear Statistics Repository
        /// </summary>
        /// <param name="context">Request Context</param>
        /// <param name="session">Business Entity Session</param>
        public ClearStatisticsRepository(Context context, IBusinessEntitySession session)
            : base(context, new ClearStatisticsMapper<T>(context), session)
        {
        }

        #endregion

        #region Override method

        /// <summary>
        /// Create Business Entity
        /// </summary>
        /// <returns>Business Entity</returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            _clearStatisticsBusinessEntity = OpenEntity(ClearStatistics.EntityName);
            return _clearStatisticsBusinessEntity;
        }

        #endregion

        #region Public method

        /// <summary>
        /// To get the fiscal calender years
        /// </summary>
        /// <returns>fiscal years</returns>
        public virtual List<FiscalPeriod> GetYears()
        {
            var fiscalCalendar = Session.GetFiscalCalendar();
            var years = new List<FiscalPeriod>();

            if (!string.IsNullOrWhiteSpace(fiscalCalendar.FirstYear.Year))
            {
                var value = Convert.ToInt32(fiscalCalendar.FirstYear.Year);

                while (value <= Convert.ToInt32(fiscalCalendar.LastYear.Year))
                {
                    var year = Session.GetYear(Convert.ToString(value));

                    fiscalCalendar.FirstYear.Year = Convert.ToString(value);
                    years.Add(new FiscalPeriod
                    {
                        Year = fiscalCalendar.FirstYear.Year,
                        Period = (short)year.Periods
                    });
                    value++;
                    while (Session.GetYear(Convert.ToString(value)) == null &&
                           value <= Convert.ToInt32(fiscalCalendar.LastYear.Year))
                    {
                        value++;
                        fiscalCalendar.FirstYear.Year = Convert.ToString(value);
                    }
                }
            }
            return years;
        }

        #endregion
    }
}
