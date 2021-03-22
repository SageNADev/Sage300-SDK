// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Reports;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Core.Cache;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities.Constants;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Core.Logging;
using Sage.CA.SBS.ERP.Sage300.Core.Logging.Watcher;
using Sage.CA.SBS.ERP.Sage300.CS.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.CS.Models;

namespace ValuedPartner.TU.Web.WebForms
{
    /// <summary>
    /// Generate Crystal Report
    /// </summary>
    public partial class ReportViewer : BaseWebPage
    {
        /// <summary>
        /// Execute the report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            var token = Request.QueryString["token"];
            var sessionId = Request.QueryString["session"];
            string reportDocumentKey = "ReportDocument_" + token;

            // set the page hidden variable.
            hiddenToken.Value = token;

            var report = GetReport(token, sessionId);

            if (report == null)
            {
                errorLabel.Text = CommonResx.ReportGenFailedMessage;
                return;
            }

            CommonUtil.SetCulture(report.Context.Language);

            //Check whether this report belongs to the same user.If not don't do anything.
            if (report.Context.SessionId != sessionId)
            {
                errorLabel.Text = CommonResx.NotAuthorizedMesage;
                return;
            }

            report.Context.Container = ConfigurationHelper.Container;

            bool isNew;

            var reportDocument = InMemoryCacheProvider.Instance.Get<ReportDocument>(reportDocumentKey);

            if (reportDocument == null)
            {
                using (var watcher = new WADLogWatcher("ReportWatcher"))
                {
                    using (var session = BusinessPoolManager.GetSession(report.Context, DBLinkType.Company, out isNew))
                    {
                        watcher.Print("Session Created.");
                        var accpacReport = session.SelectReport(report.Name, report.ProgramId, report.MenuId);
                        foreach (var parameter in report.Parameters)
                        {
                            var value = GetValue(parameter);
                            accpacReport.SetParam(parameter.Id, value);
                        }

                        reportDocument = accpacReport.GetReportDocument();
                        watcher.Print("Report Document Created.");

                        // store the report object in the memory
                        InMemoryCacheProvider.Instance.Set(reportDocumentKey, reportDocument);
                    }
                }
            }

            if (reportDocument != null)
            {
                CrystalReportViewerSage300.ReportSource = reportDocument;
                CrystalReportViewerSage300.DataBind();
                SetLogoPath(reportDocument, report);
            }

            if (!IsPostBack && report.ReportProcessType.HasFlag(ReportProcessType.OnLoad))
            {
                Process(report);
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
            var sessionId = HttpContext.Current.Request.RequestContext.RouteData.Values.ContainsKey("session")
                ? HttpContext.Current.Request.RequestContext.RouteData.Values["session"].ToString()
                : string.Empty;
            var report = GetReport(token, sessionId);

            if (IsUserAuthenticated(sessionId) && report != null && report.Context.SessionId == sessionId)
            {
                if (report.ReportProcessType.HasFlag(ReportProcessType.OnClose))
                {
                    Process(report);
                }

                string reportDocumentKey = "ReportDocument_" + token;
                InMemoryCacheProvider.Instance.Remove(reportDocumentKey);
                return true;
            }

            // return false if not authorized & memory (not got) released.
            return false;

        }

        #region Private Static methods

        /// <summary>
        /// Get Value based on true
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns></returns>
        private static string GetValue(Parameter parameter)
        {
            if (parameter == null || parameter.Value == null)
            {
                return string.Empty;
            }

            switch (parameter.DataType)
            {
                case DataType.Boolean:
                    return Convert.ToBoolean(parameter.Value) ? Constant.Yes : Constant.No;
                case DataType.Date:
                    return Convert.ToDateTime(parameter.Value).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                default:
                    return parameter.Value.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// This method does the processing after the report is generated.
        /// </summary>
        /// <param name="report">Report</param>
        private static void Process(Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report report)
        {
            if (string.IsNullOrEmpty(report.TypeName) || string.IsNullOrEmpty(report.AssemblyName))
            {
                return;
            }
            var qualified = string.Format("{0}, {1}", report.TypeName, report.AssemblyName);
            var processService = Type.GetType(qualified, true);
            var processObject = Activator.CreateInstance(processService, new object[] { report.Context });

            var mi = processService.GetMethod("Process");
            mi.Invoke(processObject, new object[] { report.ReportModel });
        }

        /// <summary>
        /// Get the report from cache based on key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="sessionId">Session id</param>
        /// <returns>Report</returns>
        private static Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report GetReport(string key, string sessionId)
        {
            try
            {
                if (ConfigurationHelper.IsOnPremise)
                {
                    var path = Path.Combine(RegistryHelper.SharedDataDirectory, $"{sessionId}.rpt");
                    if (File.Exists(path))
                    {
                        var report = File.ReadAllText(path);
                        File.Delete(path);
                        return JsonSerializer.Deserialize<Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report>(report);
                    }
                }

                return CacheHelper.Get<Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report>(key);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Get Report: key [{0}] not found", key), ex);
                return null;
            }
        }

        /// <summary>
        /// Get blob uri with SAS token
        /// </summary>
        /// <param name="report">report</param>
        /// <returns></returns>
        private string GetLogoUri(Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report report)
        {
            var service = report.Context.Container.Resolve<ICompanyProfileService<CompanyProfile>>(new ParameterOverride("context", report.Context));
            return service.GetCompanyLogoUri();
        }

        /// <summary>
        /// Set company logo path parameter
        /// </summary>
        /// <param name="reportDocument">reportDocument</param>
        /// <param name="report">report</param>
        private void SetLogoPath(ReportDocument reportDocument, Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report report)
        {
            var hasLogo = reportDocument.ParameterFields.Find("LogoPath", "");
            if (hasLogo != null)
            {
                var logoPath = GetLogoUri(report);
                reportDocument.SetParameterValue("LogoPath", logoPath);
            }
        }

        #endregion
    }
}
