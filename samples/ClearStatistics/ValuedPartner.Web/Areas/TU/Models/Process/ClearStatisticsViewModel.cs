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

using System.Collections;
using System.Collections.Generic;
using ValuedPartner.TU.Models.Process;
using ValuedPartner.TU.Models.Enums.Process;
using Sage.CA.SBS.ERP.Sage300.AR.Models;
using Sage.CA.SBS.ERP.Sage300.AR.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Models.Process;

#endregion

namespace ValuedPartner.Web.Areas.TU.Models.Process
{
    /// <summary>
    /// ClearStatisticsViewModel class
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="ClearStatistics"/></typeparam>
    public class ClearStatisticsViewModel<T> : ProcessViewModel<T>
        where T : ClearStatistics, new()
    {
        /// <summary>
        /// Clear Customer Statistics list
        /// </summary>
        public IEnumerable ClearCustomerStatistics
        {
            get { return EnumUtility.GetItems<ClearCustomerStatistics>(); }
        }

        /// <summary>
        /// Clear Group Statistics list
        /// </summary>
        public IEnumerable ClearGroupStatistics
        {
            get { return EnumUtility.GetItems<ClearGroupStatistics>(); }
        }

        /// <summary>
        /// Clear National Acct Statistics list
        /// </summary>
        public IEnumerable ClearNationalAcctStatistics
        {
            get { return EnumUtility.GetItems<ClearNationalAccountStatistics>(); }
        }

        /// <summary>
        /// Clear Salesperson Statistics list
        /// </summary>
        public IEnumerable ClearSalesPersonStatistics
        {
            get { return EnumUtility.GetItems<ClearSalespersonStatistics>(); }
        }

        /// <summary>
        /// Clear Item Statistics list
        /// </summary>
        public IEnumerable ClearItemStatistics
        {
            get { return EnumUtility.GetItems<ClearItemStatistics>(); }
        }

        /// <summary>
        /// Through Customer Period list
        /// </summary>
        public IEnumerable ThroughCustomerPeriods
        {
            get { return EnumUtility.GetItems<ThroughCustomerPeriod>(); }
        }

        /// <summary>
        /// Through National Acct Period list
        /// </summary>
        public IEnumerable ThroughNationalAcctPeriods
        {
            get { return EnumUtility.GetItems<ThroughNationalAccountPeriod>(); }
        }

        /// <summary>
        /// Through Group Period list
        /// </summary>
        public IEnumerable ThroughGroupPeriods
        {
            get { return EnumUtility.GetItems<ThroughGroupPeriod>(); }
        }

        /// <summary>
        /// Through Salesperson Period list
        /// </summary>
        public IEnumerable ThroughSalesPersonPeriods
        {
            get { return EnumUtility.GetItems<ThroughSalespersonPeriod>(); }
        }

        /// <summary>
        /// Through Item Period list
        /// </summary>
        public IEnumerable ThroughItemPeriods
        {
            get { return EnumUtility.GetItems<ThroughItemPeriod>(); }
        }

        /// <summary>
        /// Gets or sets Customer Statistics Fields values
        /// </summary>
        public CustomerStatistics CustomerStatistics { get; set; }

        /// <summary>
        /// Gets or sets Customer Group Statistics Fields values
        /// </summary>
        public CustomerGroupStatistic CustomerGroupStatistic { get; set; }

        /// <summary>
        /// Gets or sets List of AR National Account Fields values
        /// </summary>
        public NationalAccountStatistic NationalAccountStatistic { get; set; }

        /// <summary>
        /// Gets or sets List of AR Sales Person Statistic Fields values
        /// </summary>
        public SalespersonStatistic SalesPersonStatistic { get; set; }

        /// <summary>
        /// Gets or sets List of AR Item Statistic Fields values
        /// </summary>
        public ItemStatistic ItemStatistic { get; set; }

        /// <summary>
        ///  Gets or sets Customer,Customer Group and National Statistics Maximum period 
        /// </summary>
        public string MaximumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Sales Person Maximum period 
        /// </summary>
        public string SalesPersonMaximumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Item Maximum period 
        /// </summary>
        public string ItemMaximumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Customer,Customer Group and National Statistics Minimum period 
        /// </summary>
        public int MinimumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Salesperson Minimum period 
        /// </summary>
        public int SalesPersonMinimumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Item Minimum period 
        /// </summary>
        public int ItemMinimumPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Current Period for  Customer,Customer Group and National Statistics
        /// </summary>
        public string CustomerStatisticsCurrentPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Current Period for Item Statistics
        /// </summary>
        public string ItemStatisticsCurrentPeriod { get; set; }

        /// <summary>
        ///  Gets or sets Current Period for Salesperson Statistics
        /// </summary>
        public string SalesPersonStatisticsCurrentPeriod { get; set; }

        /// <summary>
        /// Gets or sets Fiscal Calendars 
        /// </summary>
        public List<FiscalPeriod> FiscalCalendars { get; set; }

        /// <summary>
        /// Date Range
        /// </summary>
        public int DateRange { get; set; }

        /// <summary>
        ///  Gets or sets Fiscal Year 
        /// </summary>
        public string FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets Customer Period 
        /// </summary>
        public StatisticsPeriodType ThroughCustomerPeriod { get; set; }

        /// <summary>
        /// Gets or sets Group Period 
        /// </summary>
        public StatisticsPeriodType ThroughGroupPeriod { get; set; }

        /// <summary>
        /// Gets or sets National Acct Period 
        /// </summary>
        public StatisticsPeriodType ThroughNationalAcctPeriod { get; set; }

        /// <summary>
        /// Gets or sets Salesperson Period 
        /// </summary>
        public StatisticsPeriodType ThroughSalesPersonPeriod { get; set; }

        /// <summary>
        /// Gets or sets Item Period 
        /// </summary>
        public StatisticsPeriodType ThroughItemPeriod { get; set; }

        /// <summary>
        /// Checks   Customer,Customer Group and National Calendar Year or not
        /// </summary>
        public bool CalendarYear { get; set; }

        /// <summary>
        /// Checks Item Calendar Year or not
        /// </summary>
        public bool ItemCalendarYear { get; set; }

        /// <summary>
        /// Checks Salesperson Calendar Year or not
        /// </summary>
        public bool SalesCalendarYear { get; set; }

        /// <summary>
        /// Checks Customer Statistics Calendar Year or not
        /// </summary>
        public bool IsCustomerCalendarYear { get; set; }

        /// <summary>
        /// Checks Customer Group Statistics Calendar Year or not
        /// </summary>
        public bool IsCustomerGroupCalendarYear { get; set; }

        /// <summary>
        /// Checks National Acct Calendar Year or not
        /// </summary>
        public bool IsNationalAcctCalendarYear { get; set; }

        /// <summary>
        /// Checks Sales Person Calendar Year or not
        /// </summary>
        public bool IsSalespersonCalendarYear { get; set; }

        /// <summary>
        /// Checks Item Calendar Year or not
        /// </summary>
        public bool IsItemCalendarYear { get; set; }

    }
}

