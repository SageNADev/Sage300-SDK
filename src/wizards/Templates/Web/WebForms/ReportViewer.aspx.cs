/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Services;
using ACCPAC.Advantage;
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
using System.Threading;
using Microsoft.Win32;
using System.Web.Mvc;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;

namespace $companynamespace$.$applicationid$.Web.WebForms
{
    /// <summary>
    /// Generate Crystal Report
    /// </summary>
    public partial class ReportViewer : BaseWebPage
    {
        /// <summary>
        /// Constant definitions
        /// </summary>
        private static class Constants
        {
            public const string REPORTEXTENSION = @".rpt";
            public const string CRYSTALREPORTS_TEMPLATEFILE_INIKEY = @"crystal";
        }

        /// <summary>
        /// Create a lock object
        /// </summary>
        private static object Lock = new object();

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
            hiddenSessionId.Value = sessionId;

            var report = GetReport(token, sessionId);

            if (report == null)
            {
                errorLabel.Text = CommonResx.ReportGenFailedMessage;
                return;
            }

            CommonUtil.SetCulture(report.Context.Language);

            // Check whether this report belongs to the same user.If not don't do anything.
            if (report.Context.SessionId != sessionId)
            {
                errorLabel.Text = CommonResx.NotAuthorizedMesage;
                return;
            }
            
            report.Context.Container = ConfigurationHelper.Container;

            bool isNew;

            var reportDocument = InMemoryCacheProvider.Instance.Get<SageWebReportDocument>(reportDocumentKey);

            if (reportDocument == null)
            {
                using (var watcher = new WADLogWatcher("ReportWatcher"))
                {
                    using (var session = BusinessPoolManager.GetSession(report.Context, DBLinkType.Company, out isNew))
                    {
                        watcher.Print("Session Created.");

                        // Throw an exception if the Crystal Reports template file is not found
                        var output = GetReportTemplatePath(report.Name, session.UserLanguage);
                        var reportPath = output.Item1;
                        var filenameOnly = output.Item2;
                        if (!File.Exists(reportPath))
                        {
                            //
                            // Security Consideration:
                            //    Do not use the full path in the message. Use only the filename
                            //
                            var msg = string.Format(CommonResx.Template_ReportCouldNotBeLocated, filenameOnly);
                            throw new MissingFileException(msg);
                        }

                        ACCPAC.Advantage.Report accpacReport = session.SelectReport(report.Name, report.ProgramId, report.MenuId);
                        foreach (var parameter in report.Parameters)
                        {
                            var value = GetValue(parameter);
                            accpacReport.SetParam(parameter.Id, value);
                        }

                        var userId = session.GetSession().UserID;
                        EvictUserWatcher.AddUserIdToPauseEviction(userId);

                        try
                        {
                            // Use lock to fix CRM multiple users concurrency printing issues. 
                            lock (Lock)
                            {
                                var doc = accpacReport.GetReportDocument();
                                reportDocument = new SageWebReportDocument(doc);
                            }
                        }
                        finally
                        {
                            // Make sure we remove that from EvictUserWatcher no matter what happen
                            EvictUserWatcher.RemoveUserIdFromPauseEviction(userId);
                        }

                        watcher.Print("Report Document Created.");

                        // store the report object in the memory
                        InMemoryCacheProvider.Instance.Set(reportDocumentKey, reportDocument);
                    }
                }
            }

            if (reportDocument != null)
            {
                CrystalReportViewerSage300.ReportSource = reportDocument.CrystalReportDocument;
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
        /// <param name="sessionId">SessionId of current session</param>
        /// <returns></returns>
        [WebMethod]
        public static bool Release(string token, string sessionId)
        {
            var report = GetReport(token, sessionId);

            if (IsUserAuthenticated(sessionId) && report != null && report.Context.SessionId == sessionId)
            {
                if (report.ReportProcessType.HasFlag(ReportProcessType.OnClose))
                {
                    Process(report);
                }

                string reportDocumentKey = "ReportDocument_" + token;

                // remove it from the cache (will trigger Dispose call on the object)
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
                    CommonUtil.ValidatePathName(RegistryHelper.SharedDataDirectory);
                    CommonUtil.ValidateFileName(sessionId);

                    var path = Path.Combine(RegistryHelper.SharedDataDirectory, $"{sessionId}{Constants.REPORTEXTENSION}");
                    
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
        private void SetLogoPath(SageWebReportDocument reportDocument, Sage.CA.SBS.ERP.Sage300.Common.Models.Reports.Report report)
        {
            var hasLogo = reportDocument.CrystalReportDocument.ParameterFields.Find("LogoPath", "");
            if (hasLogo != null)
            {
                var logoPath = GetLogoUri(report);
                reportDocument.CrystalReportDocument.SetParameterValue("LogoPath", logoPath);
            }
        }

        /// <summary>
        /// Extract the two letter area designation from the view name
        /// </summary>
        /// <param name="viewName">the string representation of the view name</param>
        /// <returns>the two letter area string</returns>
        private string GetAreaFromViewName(string viewName)
        {
            if (viewName.Length > 0)
            {
                return viewName.Substring(0, 2);
            }

            return string.Empty;
        }

        /// <summary>
        /// Build the fully-qualified path to the Crystal Reports template file
        /// </summary>
        /// <param name="viewName">The name of the view associated with the report (may have an extension and square brackets)</param>
        /// <param name="userLanguageIn">The user language as a string</param>
        /// <returns>A tuple containing the fully-qualified path as well as the report filename only</returns>
        private Tuple<string, string> GetReportTemplatePath(string viewName, string userLanguageIn)
        {
            var reportName = string.Empty;
            var filenameOnly = string.Empty;
            var reportPath = string.Empty;
            var area = string.Empty;

            var installDir = RegistryHelper.Sage300InstallDirectory;
            var version = Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities.Helper.ApplicationVersion;
            //var area = GetAreaFromProgramId(programIdIn);

            // Note: viewName can have at least three variations as follows:
            // 
            // Scenario   viewName               Description
            //
            //    1       APCHK01                This is not the actual report file name but is instead the section in
            //                                   the relevant XXRPT.ini file located in the module folder.
            //
            //    2       BKCHKSTK[APCHK01.RPT]  The actual report name is contained within the square brackets
            //
            //    3       GLGSACCT[GLGSACCT]     The XXRPT.INI section name is contained within the square brackets
            //
            //    4       POLABEL[]              The XXRPT.INI section name is outside of the empty square brackets
            //
            var nameContainsExtension = viewName.ToLowerInvariant().Contains(Constants.REPORTEXTENSION);
            var leftBracketIndex = viewName.IndexOf("[");
            var rightBracketIndex = viewName.LastIndexOf("]");
            var hasBrackets = leftBracketIndex >= 0 && rightBracketIndex > leftBracketIndex;
            var emptyBrackets = hasBrackets && (rightBracketIndex == leftBracketIndex + 1);

            if (hasBrackets)
            {
                var len = viewName.Length;
                var start = leftBracketIndex + 1;
                var end = len - leftBracketIndex - 2;
                var extractedViewName = string.Empty;

                if (!emptyBrackets)
                {
                    extractedViewName = viewName.Substring(start, end);
                } 
                else
                {
                    // Scenario 4
                    extractedViewName = viewName.Substring(0, start-1);
                }

                area = GetAreaFromViewName(extractedViewName);

                if (nameContainsExtension == true)
                {
                    // Scenario 2
                    filenameOnly = extractedViewName;
                }
                else
                {
                    // Scenario 3 and 4
                    filenameOnly = GetCrystalReportFileNameFromReportIniFile(installDir, area, version, extractedViewName);
                }
            }
            else
            {
                // Scenario 1
                area = GetAreaFromViewName(viewName);
                filenameOnly = GetCrystalReportFileNameFromReportIniFile(installDir, area, version, viewName);
            }

            reportPath = Path.Combine(installDir, area + version, userLanguageIn, filenameOnly);

            // Return both the full path and the filename
            return Tuple.Create(reportPath, filenameOnly);
        }

        /// <summary>
        /// Get the fully-qualified path to the Crystal reports template file
        /// </summary>
        /// <param name="installDirIn">The root installation directory name</param>
        /// <param name="areaIn">The area name</param>
        /// <param name="versionIn">The version number</param>
        /// <param name="viewName">The name of the view</param>
        /// <returns>The fully-qualified path to the Crystal reports template file</returns>
        private string GetCrystalReportFileNameFromReportIniFile(string installDirIn, string areaIn, string versionIn, string viewName)
        {
            var filenameOnly = string.Empty;
            var iniFileName = areaIn + "rpt.ini";
            var iniPath = Path.Combine(installDirIn, areaIn + versionIn, iniFileName);
            IniManager iniManager = new IniManager(iniPath);
            if (iniManager.KeyExists(Constants.CRYSTALREPORTS_TEMPLATEFILE_INIKEY, viewName))
            {
                var reportTemplateNameFromIni = iniManager.Read(Constants.CRYSTALREPORTS_TEMPLATEFILE_INIKEY, viewName);
                filenameOnly = $"{reportTemplateNameFromIni}{Constants.REPORTEXTENSION}";
            }
            return filenameOnly;
        }
        #endregion
    }
}
