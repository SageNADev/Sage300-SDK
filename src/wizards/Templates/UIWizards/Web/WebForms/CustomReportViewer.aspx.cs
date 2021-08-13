/* Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved. */

using System;
using System.IO;
using ACCPAC.Advantage;
using System.Web.Services;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Landlord;
using Sage.CA.SBS.ERP.Sage300.Core.Cache;

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
            var sessionId = Request.QueryString["session"];

            var token = Request.Form["hiddenToken"];

            if (token == null)
            {
                token = Guid.NewGuid().ToString();
            }

            hiddenToken.Value = token;

            string reportDocumentKey = "ReportDocument_" + token;

            if (string.IsNullOrEmpty(reportName))
            {
                errorLabel.Text = CommonResx.ReportGenFailedMessage;
                return;
            }

            var report = new Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report();
            report.Context = JsonSerializer.Deserialize<Sage.CA.SBS.ERP.Sage300.Common.Models.Context>(Session["Context"].ToString());
            CommonUtil.SetCulture(report.Context.Language);

            report.Context.Container = ConfigurationHelper.Container;

            var reportDocument = InMemoryCacheProvider.Instance.Get<SageWebReportDocument>(reportDocumentKey);
            bool isNew;

            if (reportDocument == null)
            {
                using (var session = BusinessPoolManager.GetSession(report.Context, DBLinkType.Company, out isNew))
                {
                    var repository = report.Context.Container.Resolve<ILandlordRepository>();
                    var reportPath = repository.GetSharedDirectory(report.Context.TenantId);
                    CommonUtil.ValidatePathName(reportPath);
                    CommonUtil.ValidateFileName(reportName);
                    var reportFullName = Path.Combine(reportPath, "Reports", reportName);
                    var accpacReport = session.SelectReport(reportFullName, "", null);

                    var userId = session.GetSession().UserID;
                    EvictUserWatcher.AddUserIdToPauseEviction(userId);
                    var doc = accpacReport.GetReportDocument();
                    reportDocument = new SageWebReportDocument(doc, session, accpacReport);

                    EvictUserWatcher.RemoveUserIdFromPauseEviction(userId);
                }
                InMemoryCacheProvider.Instance.Set(reportDocumentKey, reportDocument, ConfigurationHelper.ReportCacheExpirationInMinutes);
            }

            if (reportDocument != null)
            {
                CrystalReportViewerSage300.ReportSource = reportDocument.CrystalReportDocument;
                CrystalReportViewerSage300.DataBind();
            }
        }

        /// <summary>
        /// Releases the report resource for the given token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [WebMethod]
        public static bool Release(string token)
        {
            string reportDocumentKey = "ReportDocument_" + token;

            // remove it from the cache (will trigger Dispose call on the object)
            InMemoryCacheProvider.Instance.Remove(reportDocumentKey);
            return true;
        }
    }
}
