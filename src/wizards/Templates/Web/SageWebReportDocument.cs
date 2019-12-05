/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */

using CrystalDecisions.CrystalReports.Engine;
using System;

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

        /// <summary>
        /// Constructor with Crystal report document
        /// </summary>
        /// <param name="rp"></param>
        public SageWebReportDocument(ReportDocument rp)
        {
            CrystalReportDocument = rp;
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