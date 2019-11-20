/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */

using CrystalDecisions.CrystalReports.Engine;
using Sage.CA.SBS.ERP.Sage300.Core.Interfaces;
using System;

namespace $companynamespace$.$applicationid$.Web
{
    /// <summary>
    /// A wrapper class or Crystal report document
    /// </summary>
    public class SageWebReportDocument : IClosable, IDisposable
    {
        /// <summary>
        /// Private Crystal report document
        /// </summary>
        public ReportDocument ReportDocument { get; private set; }

        /// <summary>
        /// Constructor with Crystal report document
        /// </summary>
        /// <param name="rp"></param>
        public SageWebReportDocument(ReportDocument rp)
        {
            ReportDocument = rp;
        }

        /// <inheritdoc />
        public void Close()
        {
            ReportDocument?.Close();
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
                    ReportDocument?.Dispose();
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