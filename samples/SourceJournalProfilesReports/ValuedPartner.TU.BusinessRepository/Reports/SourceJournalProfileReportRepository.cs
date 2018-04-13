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

using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using ValuedPartner.TU.BusinessRepository.Mappers.Reports;
using ValuedPartner.TU.Interfaces.BusinessRepository.Reports;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Reports;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Reports
{
    /// <summary>
    /// Repository Class for SourceJournalProfileReport
    /// </summary>
    /// <typeparam name="T">SourceJournalProfileReport</typeparam>
    public class SourceJournalProfileReportRepository<T> : BaseReportRepository<T>, ISourceJournalProfileReportEntity<T>
        where T : SourceJournalProfileReport, new()
    {
        #region Variables

        /// <summary>
        /// Business Entity
        /// </summary>
        private IBusinessEntity _businessEntity;

        /// <summary>
        /// Constant for To Source Journal Profile.
        /// </summary>
        private const string ToJournalProfile = "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfileReport
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileReportRepository(Context context)
            : base(context, new SourceJournalProfileReportMapper<T>(context), SourceJournalProfileReport.EntityName)
        {
        }

        /// <summary>
        /// Constructor for SourceJournalProfileReport
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public SourceJournalProfileReportRepository(Context context, IBusinessEntitySession session)
            : base(context, new SourceJournalProfileReportMapper<T>(context), session)
        {
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Gets access rights
        /// </summary>
        /// <returns>User Access</returns>
        protected override UserAccess GetReportAccessRights()
        {
            CreateBusinessEntities();
            var userAccess = new UserAccess();
            if (_businessEntity.CanInquire)
            {
                AddSecurityType(userAccess, SecurityType.Print);
            }
            return userAccess;
        }

        /// <summary>
        /// Get default model values
        /// </summary>
        /// <returns>SourceJournalProfileReport</returns>
        protected override T GetDefaultModel()
        {
            var model = new T
            {
                Frjrnl = string.Empty,
                Tojrnl = ToJournalProfile,
            };

            return model;
        }


        /// <summary>
        /// Create business entities
        /// </summary>
        /// <returns>IBusinessEntity</returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            _businessEntity = OpenEntity(SourceJournalProfile.EntityName);
            return _businessEntity;
        }

        #endregion
    }
}
