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

using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Services;
using ACCPAC.Advantage;
using Microsoft.Win32;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Practices.Unity;
//using Sage.CA.SBS.ERP.Sage300.Common;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using CrystalDecisions.Shared;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Landlord;

namespace ValuedPartner.Web.WebForms
{
   /// <summary>
   /// Generate Custom Crystal Report
   /// </summary>
    public partial class CustomReportViewer : BaseWebPage
    {
        /// <summary>
        /// Execute the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            var reportName = Request.QueryString["reportName"].ToString();

            if (string.IsNullOrEmpty(reportName))
            {
                errorLabel.Text = CommonResx.ReportGenFailedMessage;
                return;
            }

            var report = new Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report();
            report.Context = JsonSerializer.Deserialize<Sage.CA.SBS.ERP.Sage300.Common.Models.Context>(Session["Context"].ToString());
            CommonUtil.SetCulture(report.Context.Language);

            if (report.Context.SessionId != Session.SessionID)
            {
                errorLabel.Text = CommonResx.NotAuthorizedMesage;
                return;
            }
            report.Context.Container = ConfigurationHelper.Container;

            ReportDocument reportDocument;
            bool isNew;
            using (var session = BusinessPoolManager.GetSession(report.Context, DBLinkType.Company, out isNew))
            {
                var repository = report.Context.Container.Resolve<ILandlordRepository>();
                var reportPath = repository.GetSharedDirectory(report.Context.TenantId);
                var reportFullName = Path.Combine(reportPath, "Reports", reportName);
                var accpacReport = session.SelectReport(reportFullName, "", null);
                reportDocument = accpacReport.GetReportDocument();
            }

            if (reportDocument != null)
            {
                CrystalReportViewerSage300.ReportSource = reportDocument;
                CrystalReportViewerSage300.DataBind();
            }
        }
    }
}
