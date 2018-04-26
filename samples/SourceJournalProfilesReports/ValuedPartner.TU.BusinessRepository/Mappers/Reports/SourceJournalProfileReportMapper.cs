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

#region Namespace

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Reports;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using ValuedPartner.TU.Models.Reports;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers.Reports
{
    /// <summary>
    /// Class for SourceJournalProfileReport mapping
    /// </summary>
    /// <typeparam name="T">SourceJournalProfileReport</typeparam>
    public class SourceJournalProfileReportMapper<T> : IReportMapper<T> where T : SourceJournalProfileReport, new ()
    {
        #region Private variables

        /// <summary>
        /// Current context
        /// </summary>
        private readonly Context _context;
        #endregion

        #region Constants

        /// <summary>
        /// Default report name
        /// </summary>
        private const string ReportName = "GLJP01F";

        /// <summary>
        /// The program identifier
        /// </summary>
        private const string ProgramId = "GL4111";

        /// <summary>
        /// The menu identifier
        /// </summary>
        private const string MenuId = "<MENU ID>";

        #endregion
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileReportMapper(Context context)
        {
            _context = context;
        }

        #endregion

        #region IReportMapper methods

        /// <summary>
        /// Map a report
        /// </summary>
        /// <param name="model">Model to be converted to report</param>
        /// <returns>Mapped Report</returns>
        public Report Map(T model)
        {

            var report = new Report
            {
                ProgramId = ProgramId,
                Context = _context,
                MenuId = MenuId,
                Name = ReportName
            };

            SetReportName(report, model);

            var frjrnl = SetParameter(SourceJournalProfileReport.Fields.Frjrnl, !string.IsNullOrEmpty(model.Frjrnl) ? model.Frjrnl : " ");
            report.Parameters.Add(frjrnl);

            var tojrnl = SetParameter(SourceJournalProfileReport.Fields.Tojrnl, !string.IsNullOrEmpty(model.Tojrnl) ? model.Tojrnl : " ");
            report.Parameters.Add(tojrnl);

            return report;
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Set report name
        /// </summary>
        /// <param name="report">Report</param>
        /// <param name="model">Model</param>
        private static void SetReportName(Report report, T model)
        {
            report.Name = ReportName;
        }

        /// <summary>
        /// To set all parameter id and values
        /// </summary>
        /// <param name="id">Parameter Field Id</param>
        /// <param name="value">Parameter Value</param>
        private static Parameter SetParameter(string id, string value)
        {
            var parameter = new Parameter { Id = id, Value = string.IsNullOrEmpty(value) ? string.Empty : value };
            return parameter;
        }
        #endregion

    }
}