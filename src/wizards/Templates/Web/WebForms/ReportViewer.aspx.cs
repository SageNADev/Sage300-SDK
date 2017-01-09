﻿using System;
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

namespace $companynamespace$.$applicationid$.Web.WebForms
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
            string reportDocumentKey = "ReportDocument_" + token;

            // set the page hidden variable.
            hiddenToken.Value = token;

            var report = GetReport(token);

            if (report == null)
            {
                errorLabel.Text = CommonResx.ReportGenFailedMessage;
                return;
            }

            CommonUtil.SetCulture(report.Context.Language);

            //Check whether this report belongs to the same user.If not don't do anything.
            if (report.Context.SessionId != Session.SessionID)
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
            var report = GetReport(token);

            var sessionId = HttpContext.Current.Session.SessionID;

            if (IsUserAuthenticated() && report != null && report.Context.SessionId == sessionId)
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
        /// Get The report from cache based on key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Report</returns>
        private static Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report GetReport(string key)
        {
            try
            {
                if (ConfigurationHelper.IsOnPremise)
                {
                    var path = Path.Combine(RegistryHelper.SharedDataDirectory, string.Format("{0}.rpt", HttpContext.Current.Session.SessionID));
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
