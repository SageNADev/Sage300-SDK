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

using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Reports;
using ValuedPartner.TU.Interfaces.Services.Reports;
using ValuedPartner.TU.Models.Reports;
using ValuedPartner.Web.Areas.TU.Models.Reports;
using Sage.CA.SBS.ERP.Sage300.GL.Resources;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Reports
{
    /// <summary>
    /// Public controller for SourceJournalProfileReport
    /// </summary>
    /// <typeparam name="T">SourceJournalProfileReport</typeparam>
    public class SourceJournalProfileReportController<T> : ReportController<T, SourceJournalProfileReportViewModel<T>, 
        SourceJournalProfileReportControllerInternal<T, SourceJournalProfileReportViewModel<T>, ISourceJournalProfileReportService<T>>, 
        ISourceJournalProfileReportService<T>> where T : SourceJournalProfileReport, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfileReport
        /// </summary>
        /// <param name="container">Unity Container</param>
        public SourceJournalProfileReportController(IUnityContainer container)
            : base(container, context => new SourceJournalProfileReportControllerInternal<T, SourceJournalProfileReportViewModel<T>,
                ISourceJournalProfileReportService<T>>(context),"TUSourceJournalProfileReport")
        {
        }

        #endregion

        /// <summary>   
        /// Get default Index page
        /// </summary>
        /// <returns>Index view model which needs to be display.</returns>
        public override ActionResult Index()
        {
            try
            {
                return View(ControllerInternal.Get());
            }
            catch (BusinessException businessException)
            {
                return
                     JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException,
                        GLCommonResx.Profile));
            }
        }

    }
}
