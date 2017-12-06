// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
using ValuedPartner.TU.Interfaces.BusinessRepository.Process;
using ValuedPartner.TU.Interfaces.Services.Process;
using ValuedPartner.TU.Models.Process;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Process;
#endregion

namespace ValuedPartner.TU.Services.Process
{
    /// <summary>
    /// Class for ClearStatisticsService
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="ClearStatistics"/></typeparam>
    public class ClearStatisticsService<T> : ProcessService<T, IClearStatisticsEntity<T>>,
        IClearStatisticsService<T> where T : ClearStatistics, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor for ClearStatistics
        /// </summary>
        /// <param name="context">Request Context</param>
        public ClearStatisticsService(Context context)
            : base(context, Guid.Parse("ab08bb64-73bc-4681-87fd-08dd23af21a9"))
        {
        }

        #endregion
        
        #region Override method

        /// <summary>
        /// Gets Fiscal Years
        /// </summary>
        /// <returns>Fiscal Years</returns>
        public virtual List<FiscalPeriod> GetYears()
        {
            using (var repository = Resolve<IClearStatisticsEntity<T>>())
            {
                return repository.GetYears();
            }
        }

        /// <summary>
        /// Get session date period info 
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="app">Application ID</param>
        /// <returns>Period info</returns>
        public override FiscalYearSet GetFiscalYearPeriodInfoSet(DateTime date, string app)
        {
            using (var repository = Resolve<IClearStatisticsEntity<T>>())
            {
                return repository.GetFiscalYearPeriodInfoSet(date, app);
            }
        }

        #endregion

    }
}
