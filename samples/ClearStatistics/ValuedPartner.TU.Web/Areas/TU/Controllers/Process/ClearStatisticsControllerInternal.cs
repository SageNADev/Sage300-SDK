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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Unity;
using ValuedPartner.TU.Interfaces.Services.Process;
using ValuedPartner.TU.Models.Process;
using ValuedPartner.TU.Web.Areas.TU.Models.Process;
using ValuedPartner.TU.Models.Enums.Process;
using Options = Sage.CA.SBS.ERP.Sage300.AR.Models.Options;
using Sage.CA.SBS.ERP.Sage300.AR.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.AR.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.AR.Models.Enums.Reports;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Process;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Process;
using Sage.CA.SBS.ERP.Sage300.CS.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.CS.Models;
using Sage.CA.SBS.ERP.Sage300.CS.Models.Enums;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers.Process
{
    /// <summary>
    /// ClearStatistics Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="ClearStatistics"/></typeparam>
    public class ClearStatisticsControllerInternal<T> :
        ProcessControllerInternal<T, ClearStatisticsViewModel<T>, IClearStatisticsService<T>>
        where T : ClearStatistics, new()
    {
        #region Private variables

        /// <summary>
        /// Variable for storing context.
        /// </summary>
        private readonly Context _context;

        /// <summary>
        /// Variable for storing Options.
        /// </summary>ClearStatisticsViewModel
        private Options _options;

        /// <summary>
        /// Constant for Minimum Period
        /// </summary>
        private const int MinimumPeriod = 1;

        /// <summary>
        /// Constant for Default Period
        /// </summary>
        private const int DefaultPeriod = 0;

        /// <summary>
        /// Constant for To Customer 
        /// </summary>
        private const string ToCustomer = "ZZZZZZZZZZZZ";

        /// <summary>
        /// Constant for To Group Code 
        /// </summary>
        private const string ToGroupCode = "ZZZZZZ";

        /// <summary>
        /// Constant for To National Account 
        /// </summary>
        private const string ToNationalAccount = "ZZZZZZZZZZZZ";

        /// <summary>
        /// Constant for To Sales Person 
        /// </summary>
        private const string ToSalesperson = "ZZZZZZZZ";

        /// <summary>
        /// Constant for To Item Number 
        /// </summary>
        private const string ToItemNumber = "ZZZZZZZZZZZZZZZZ";

        /// <summary>
        /// Defining Total Period Count 
        /// </summary>
        private int _totalPeriodCount;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Clear Statistics Controller Internal
        /// </summary>
        /// <param name="context">Where context is type of current "User Session".</param>
        public ClearStatisticsControllerInternal(Context context)
            : base(context)
        {
            _context = context;
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Method to Get the View Model Data.
        /// </summary>
        /// <returns>Clear Statistics View Model</returns>
        public override ClearStatisticsViewModel<T> Get()
        {
            _options = GetOptions();
            var clearStatistics = Service.Get();
            int dateRange = 0;
            int maximumPeriod;
            int salespersonMaximumPeriod;
            int itemMaximumPeriod;
            var customerStatisticsCurrentPeriod = GetCurrentPeriod(_options.CustStatisticsYearType,
                _options.CustStatisticsPeriodType, Context.SessionDate,
                out maximumPeriod);
            var itemStatisticsCurrentPeriod = GetCurrentPeriod(_options.ItemStatisticsYearType,
                _options.ItemStatisticsPeriodType, Context.SessionDate,
                out itemMaximumPeriod);
            var salespersonStatisticsCurrentPeriod = GetCurrentPeriod(_options.SalesStatisticsYearType,
                _options.SalesStatisticsPeriodType, Context.SessionDate,
                out salespersonMaximumPeriod);

            var sessionDateYear = Context.SessionDate.Year.ToString(CultureInfo.InvariantCulture);
            clearStatistics.ThroughCustomerYear = sessionDateYear;
            clearStatistics.ThroughNationalAccountYear = sessionDateYear;
            clearStatistics.ThroughGroupYear = sessionDateYear;
            clearStatistics.ThroughSalespersonYear = sessionDateYear;
            clearStatistics.ThroughItemYear = sessionDateYear;

            clearStatistics.ThroughCustomerPeriod = EnumUtility.GetEnum<ThroughCustomerPeriod>(string.IsNullOrEmpty(customerStatisticsCurrentPeriod) ? "0" : customerStatisticsCurrentPeriod);

            string customerPeriod = EnumUtility.EnumToString(clearStatistics.ThroughCustomerPeriod);
            clearStatistics.ThroughCustomerPeriod = EnumUtility.GetEnum<ThroughCustomerPeriod>(customerPeriod.PadLeft(2, '0'));

            clearStatistics.ThroughGroupPeriod = EnumUtility.GetEnum<ThroughGroupPeriod>(string.IsNullOrEmpty(customerStatisticsCurrentPeriod) ? "0" : customerStatisticsCurrentPeriod);

            string customerGroupPeriod = EnumUtility.EnumToString(clearStatistics.ThroughGroupPeriod);
            clearStatistics.ThroughGroupPeriod = EnumUtility.GetEnum<ThroughGroupPeriod>(customerGroupPeriod.PadLeft(2, '0'));

            clearStatistics.ThroughNationalAccountPeriod = EnumUtility.GetEnum<ThroughNationalAccountPeriod>(string.IsNullOrEmpty(customerStatisticsCurrentPeriod) ? "0" : customerStatisticsCurrentPeriod);

            string nationalAcctPeriod = EnumUtility.EnumToString(clearStatistics.ThroughNationalAccountPeriod);
            clearStatistics.ThroughNationalAccountPeriod = EnumUtility.GetEnum<ThroughNationalAccountPeriod>(nationalAcctPeriod.PadLeft(2, '0'));

            clearStatistics.ThroughSalespersonPeriod = EnumUtility.GetEnum<ThroughSalespersonPeriod>(string.IsNullOrEmpty(salespersonStatisticsCurrentPeriod) ? "0" : salespersonStatisticsCurrentPeriod);

            string salespersonPeriod = EnumUtility.EnumToString(clearStatistics.ThroughSalespersonPeriod);
            clearStatistics.ThroughSalespersonPeriod = EnumUtility.GetEnum<ThroughSalespersonPeriod>(salespersonPeriod.PadLeft(2, '0'));

            clearStatistics.ThroughItemPeriod = EnumUtility.GetEnum<ThroughItemPeriod>(string.IsNullOrEmpty(itemStatisticsCurrentPeriod) ? "0" : itemStatisticsCurrentPeriod);

            string itemPeriod = EnumUtility.EnumToString(clearStatistics.ThroughItemPeriod);
            clearStatistics.ThroughItemPeriod = EnumUtility.GetEnum<ThroughItemPeriod>(itemPeriod.PadLeft(2, '0'));

            clearStatistics.ToCustomerNumber = ToCustomer;
            clearStatistics.ToGroupCode = ToGroupCode;
            clearStatistics.ToNationalAccount = ToNationalAccount;
            clearStatistics.ToSalesperson = ToSalesperson;
            clearStatistics.ToItemNumber = ToItemNumber;
            var companyProfile = GetCompanyProfile();
            if (companyProfile != null)
                dateRange = companyProfile.CompanyProfileOptions.WarningDateRange;
            var viewModel = new ClearStatisticsViewModel<T>
            {
                Data = clearStatistics,
                UserMessage = new UserMessage(clearStatistics),
                ProcessResult = new ProcessResult {ProgressMeter = new ProgressMeter()},
                MaximumPeriod = maximumPeriod.ToString(CultureInfo.InvariantCulture),
                SalesPersonMaximumPeriod = salespersonMaximumPeriod.ToString(CultureInfo.InvariantCulture),
                ItemMaximumPeriod = itemMaximumPeriod.ToString(CultureInfo.InvariantCulture),
                MinimumPeriod = maximumPeriod > DefaultPeriod ? MinimumPeriod : DefaultPeriod,
                SalesPersonMinimumPeriod = salespersonMaximumPeriod > DefaultPeriod ? MinimumPeriod : DefaultPeriod,
                ItemMinimumPeriod = itemMaximumPeriod > DefaultPeriod ? MinimumPeriod : DefaultPeriod,
                FiscalYear = Context.SessionDate.Year.ToString(CultureInfo.InvariantCulture),
                CustomerStatisticsCurrentPeriod = customerStatisticsCurrentPeriod,
                ItemStatisticsCurrentPeriod = itemStatisticsCurrentPeriod,
                SalesPersonStatisticsCurrentPeriod = salespersonStatisticsCurrentPeriod,
                CalendarYear = _options.CustStatisticsYearType == StatisticsAccumulateYearType.CalendarYear,
                ItemCalendarYear = _options.ItemStatisticsYearType == StatisticsAccumulateYearType.CalendarYear,
                SalesCalendarYear = _options.SalesStatisticsYearType == StatisticsAccumulateYearType.CalendarYear,
                ThroughCustomerPeriod = _options.CustStatisticsPeriodType,
                ThroughGroupPeriod = _options.CustStatisticsPeriodType,
                ThroughNationalAcctPeriod = _options.CustStatisticsPeriodType,
                ThroughSalesPersonPeriod = _options.SalesStatisticsPeriodType,
                ThroughItemPeriod = _options.ItemStatisticsPeriodType,
                FiscalCalendars = Service.GetYears(),
                DateRange = dateRange,
            };
            return viewModel;
        }

        /// <summary>
        /// Method to do Process
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Clear Statistics View Model</returns>
        public override ClearStatisticsViewModel<T> Process(T model)
        {
            var temp = EnumUtility.EnumToString(model.ThroughCustomerPeriod).PadLeft(2, '0');
            model.ThroughCustomerPeriod = EnumUtility.GetEnum<ThroughCustomerPeriod>(temp);

            temp = EnumUtility.EnumToString(model.ThroughGroupPeriod).PadLeft(2, '0');
            model.ThroughGroupPeriod = EnumUtility.GetEnum<ThroughGroupPeriod>(temp);

            temp = EnumUtility.EnumToString(model.ThroughNationalAccountPeriod).PadLeft(2, '0');
            model.ThroughNationalAccountPeriod = EnumUtility.GetEnum<ThroughNationalAccountPeriod>(temp);

            temp = EnumUtility.EnumToString(model.ThroughSalespersonPeriod).PadLeft(2, '0');
            model.ThroughSalespersonPeriod = EnumUtility.GetEnum<ThroughSalespersonPeriod>(temp);

            temp = EnumUtility.EnumToString(model.ThroughItemPeriod).PadLeft(2, '0');
            model.ThroughItemPeriod = EnumUtility.GetEnum<ThroughItemPeriod>(temp);

            return new ClearStatisticsViewModel<T>
            {
                WorkflowInstanceId = Service.Process(model),
                UserMessage = new UserMessage {IsSuccess = true}
            };
        }

        #endregion

        #region private methods

        /// <summary>
        /// Method to get the option
        /// </summary>
        /// <returns>option</returns>
        private Options GetOptions()
        {
            var optionsService =
                Context.Container.Resolve<IOptionsService<Options>>(new ParameterOverride("context", Context));
            return optionsService.Get().Items.FirstOrDefault();
        }

        /// <summary>
        /// Get Company Profile
        /// </summary>
        /// <returns>Company Profile</returns>
        private CompanyProfile GetCompanyProfile()
        {
            var companyProfileRepository =
                Context.Container.Resolve<ICompanyProfileService<CompanyProfile>>(new ParameterOverride("context",
                    Context));
            var companyProfile = companyProfileRepository.Get();
            return (companyProfile != null && companyProfile.Items != null && companyProfile.Items.Any()
                ? companyProfile.Items.First()
                : new CompanyProfile());
        }

        /// <summary>
        /// Method to get current period for Customer,Customer Group,National Account,Sales Person and Item Statistics
        /// </summary>
        /// <param name="yearType">Year Type</param>
        /// <param name="periodType">Period Type</param>
        /// <param name="sessionDate">Session Date</param>
        /// <param name="totalPeriodCount">Total Period Count</param>
        /// <returns>Current Period</returns>
        private string GetCurrentPeriod(StatisticsAccumulateYearType yearType, StatisticsPeriodType periodType,
            DateTime sessionDate, out int totalPeriodCount)
        {
            string currentPeriod;
            totalPeriodCount = 0;
            if ((yearType == StatisticsAccumulateYearType.FiscalYear) &&
                (periodType == StatisticsPeriodType.FiscalPeriod))
            {
                var fiscalCalendarService =
                    Context.Container.Resolve<ICompanyProfileService<CompanyProfile>>(new ParameterOverride("context",
                        Context));
                var fiscalCalendar = fiscalCalendarService.Get().Items.FirstOrDefault();
                if (fiscalCalendar != null)
                    totalPeriodCount = fiscalCalendar.CompanyProfileOptions.NumberofFiscalPeriods ==
                                       NumberofFiscalPeriods.Num12
                        ? 12
                        : 13;
                currentPeriod = sessionDate.Month.ToString(CultureInfo.InvariantCulture);
                return currentPeriod;
            }
            var dayOfYear = sessionDate.DayOfYear;
            switch (periodType)
            {
                case StatisticsPeriodType.Weekly:
                    totalPeriodCount = (int) PeriodCount.Weekly;
                    DayOfWeek week = (new DateTime(sessionDate.Year, 1, 1)).DayOfWeek;
                    int extraDays = week - DayOfWeek.Sunday;
                    currentPeriod =
                        Math.Ceiling(((double) dayOfYear + extraDays)/7).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Sevendays:
                    totalPeriodCount = (int) PeriodCount.Sevendays;
                    currentPeriod = Math.Ceiling((double) dayOfYear/7).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Biweekly:
                    totalPeriodCount = (int) PeriodCount.Biweekly;
                    currentPeriod = Math.Ceiling((double) dayOfYear/14).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Fourweeks:
                    totalPeriodCount = (int) PeriodCount.Fourweeks;
                    currentPeriod = Math.Ceiling((double) dayOfYear/28).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Monthly:
                    totalPeriodCount = (int) PeriodCount.Monthly;
                    currentPeriod = sessionDate.Month.ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Bimonthly:
                    totalPeriodCount = (int) PeriodCount.Bimonthly;
                    currentPeriod =
                        Math.Ceiling((Convert.ToDecimal(sessionDate.Month)/2)).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Quarterly:
                    totalPeriodCount = (int) PeriodCount.Quarterly;
                    currentPeriod = Math.Ceiling((double) sessionDate.Month/3).ToString(CultureInfo.InvariantCulture);
                    break;
                case StatisticsPeriodType.Semiannually:
                    totalPeriodCount = (int) PeriodCount.Semiannually;
                    currentPeriod = Math.Ceiling((double) sessionDate.Month/6).ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    totalPeriodCount = 0;
                    currentPeriod = "0";
                    break;
            }
            return currentPeriod;
        }

        #endregion

        #region internal method

        /// <summary>
        /// Gets the fiscal year for Customer,Customer Group,National Account,Sales Person and Item Statistics
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="type">Type</param>
        /// <returns>Maximum period if fiscal year is valid or else 0</returns>
        internal int GetMaxPeriodForValidYear(string year, string type)
        {
            var maxPeriod = 0;
            _totalPeriodCount = 0;
            var fiscalCalendarService =
                Context.Container.Resolve<IFiscalCalendarService<Sage.CA.SBS.ERP.Sage300.CS.Models.FiscalCalendar>>(
                    new ParameterOverride("context", Context));
            if (fiscalCalendarService.IsValid(year))
            {
                var options = GetOptions();
                if (type == "sales")
                {
                    GetCurrentPeriod(options.SalesStatisticsYearType, options.SalesStatisticsPeriodType,
                        Context.SessionDate, out _totalPeriodCount);
                }
                else if (type == "items")
                {
                    GetCurrentPeriod(options.ItemStatisticsYearType, options.ItemStatisticsPeriodType,
                        Context.SessionDate, out _totalPeriodCount);
                }
                else
                {
                    GetCurrentPeriod(options.CustStatisticsYearType, options.CustStatisticsPeriodType,
                        Context.SessionDate, out _totalPeriodCount);
                }

                maxPeriod = _totalPeriodCount;
            }

            return maxPeriod;
        }

        #endregion
    }
}
