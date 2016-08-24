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

using System.Web.Routing;
using Microsoft.Practices.Unity;
using ValuedPartner.TU.Interfaces.Services.Process;
using ValuedPartner.TU.Models.Process;
using ValuedPartner.Web.Areas.TU.Models.Process;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Process;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.AR.Resources.Forms;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Process
{
    /// <summary>
    /// Class ClearStatistics Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="ClearStatistics"/></typeparam>
    public class ClearStatisticsController<T> :
        ProcessController<T, ClearStatisticsViewModel<T> , ClearStatisticsControllerInternal<T>, IClearStatisticsService<T>>
        where T : ClearStatistics, new()
    {
        #region Private Variables

        private const string CustomerType = "customer";
        private const string SalesType = "sales";
        private const string ItemType = "items";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Clear Statistics Controller
        /// </summary>
        /// <param name="container">Unity container</param>
        public ClearStatisticsController(IUnityContainer container)
            : base(container, (context => new ClearStatisticsControllerInternal<T>(context)),
                "TUClearStatistics")
        {
        }
        #endregion

        #region Initialize MultitenantControllerBase

        /// <summary>
        /// Initializes the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ControllerInternal = new ClearStatisticsControllerInternal<T>(Context);
        }

        #endregion

        #region Action

        /// <summary>
        /// Getting Customer,Group and National Statistics maximum period for a valid year
        /// </summary>
        /// <param name="year">Fiscal year.</param>
        /// <returns>Maximum period if fiscal year is valid or else 0</returns>
        public JsonNetResult GetMaxPeriodForValidYear(string year)
        {
            try
            {
                return JsonNet(ControllerInternal.GetMaxPeriodForValidYear(year, CustomerType));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ARCommonResx.Customer));
            }
        }

        /// <summary>
        /// Getting Sales Person maximum period for a valid year
        /// </summary>
        /// <param name="year">Fiscal year.</param>
        /// <returns>Maximum period if fiscal year is valid or else 0</returns>
        public JsonNetResult GetSalesPersonMaxPeriodForValidYear(string year)
        {
            try
            {
                return JsonNet(ControllerInternal.GetMaxPeriodForValidYear(year, SalesType));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ARCommonResx.SalespersonStatistics));
            }
        }

        /// <summary>
        /// Getting Item maximum period for a valid year
        /// </summary>
        /// <param name="year">Fiscal year.</param>
        /// <returns>Maximum period if fiscal year is valid or else 0</returns>
        public JsonNetResult GetItemMaxPeriodForValidYear(string year)
        {
            try
            {
                return JsonNet(ControllerInternal.GetMaxPeriodForValidYear(year, ItemType));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ARCommonResx.ItemStatistics));
            }
        }

        #endregion
    }
}
