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

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Reports;
using ValuedPartner.TU.Interfaces.Services.Reports;
using ValuedPartner.TU.Models.Reports;
using ValuedPartner.Web.Areas.TU.Models.Reports;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Reports
{
    /// <summary>
    /// Internal controller for SourceJournalProfileReport
    /// </summary>
    /// <typeparam name="T">SourceJournalProfileReport</typeparam>
    /// <typeparam name="TViewModel">SourceJournalProfileReportViewModel</typeparam>
    /// <typeparam name="TService">ISourceJournalProfileReportService</typeparam>
    public class SourceJournalProfileReportControllerInternal<T, TViewModel, TService> :
        ReportControllerInternal<T, TViewModel, TService>
        where T : SourceJournalProfileReport, new()
        where TViewModel : SourceJournalProfileReportViewModel<T>, new()
        where TService : ISourceJournalProfileReportService<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfileReport
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileReportControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

    }
}
