﻿using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Services;
using ACCPAC.Advantage;
using Microsoft.Win32;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using CrystalDecisions.Shared;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Landlord;

namespace $companynamespace$.$applicationid$.Web.WebForms
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
