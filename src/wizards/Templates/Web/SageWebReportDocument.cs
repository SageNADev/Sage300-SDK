/* Copyright (c) 2020 Sage Software, Inc.  All rights reserved. */

using CrystalDecisions.CrystalReports.Engine;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using System;
using ACCPAC.Advantage;
using Sage.CA.SBS.ERP.Sage300.Core.Logging;

namespace $companynamespace$.$applicationid$.Web
{
    /// <summary>
    /// A wrapper around the Crystal's ReportDocument. We had problem about ReportDocuments not being closed properly when 
    /// the web report screens are closed. This could result reaching the Crystal's 75 reports limit on a given time.
    /// With this wrapper, we can guarantee that when this object is disposed, ReportDocument will be closed and disposed as well.
    /// </summary>
    public class SageWebReportDocument: IDisposable
    {
        /// <summary>
        /// Private Crystal report document
        /// </summary>
        public ReportDocument CrystalReportDocument { get; private set; }
        private IBusinessEntitySession Session { get; set; }
        private ACCPAC.Advantage.Report AccpacReport { get; set; }

        /// <summary>
        /// Constructor with Crystal report document
        /// </summary>
        /// <param name="rp"></param>
        /// <param name="session"></param>
        /// <param name="accpacReport"></param>
        public SageWebReportDocument(ReportDocument rp, IBusinessEntitySession session, ACCPAC.Advantage.Report accpacReport)
        {
            CrystalReportDocument = rp;
            Session = session;
            AccpacReport = accpacReport;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Clean up Crystal ReportDocument
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CrystalReportDocument?.Close();
                    CrystalReportDocument?.Dispose();
                    AccpacReport.Dispose();
                    Session?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
         ~SageWebReportDocument() {
           Dispose(false);
         }

        /// <summary>
        /// To call Dispose(bool) method to clean up
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}