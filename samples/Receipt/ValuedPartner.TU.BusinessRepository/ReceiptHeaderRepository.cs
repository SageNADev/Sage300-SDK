// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

using System;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
<<<<<<< HEAD
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Interfaces.BusinessRepository;

#endregion

namespace ValuedPartner.TU.BusinessRepository
=======
using ValuedParter.TU.BusinessRepository.Mappers;
using ValuedParter.TU.Models;
using ValuedParter.TU.Interfaces.BusinessRepository;

#endregion

namespace ValuedParter.TU.BusinessRepository
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
{
    /// <summary>
    /// Class Receipt Header Repository
    /// </summary>
    public class ReceiptHeaderRepository : FlatRepository<ReceiptHeader>, IReceiptHeaderEntity
    {
        #region Variables

        /// <summary>
        /// IC0580 - Receipt Header entity
        /// </summary>
        private IBusinessEntity _businessEntity;

        /// <summary>
        /// Record Filter
        /// </summary>
        private static readonly Func<ReceiptHeader, bool> RecordFilter = receiptHeader => !string.IsNullOrEmpty(receiptHeader.ReceiptNumber);

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for ReceiptHeader
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptHeaderRepository(Context context)
            : base(context, new ReceiptHeaderMapper(context), null)
        {
            ValidRecordFilter = RecordFilter;
        }

        /// <summary>
        /// Constructor for ReceiptHeader
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public ReceiptHeaderRepository(Context context, IBusinessEntitySession session)
            : base(context, new ReceiptHeaderMapper(context), null, session)
        {
            ValidRecordFilter = RecordFilter;
        }

        #endregion

        #region Protected/public methods

        /// <summary>
        /// Additional Access Check for Export and Import
        /// </summary>
        /// <returns>User Access</returns>
        public override UserAccess GetAccessRights()
        {
            var userAccess = base.GetAccessRights();
            if (SecurityCheck(Security.TUImport))
            {
                userAccess.SecurityType |= SecurityType.Import;
            }
            if (SecurityCheck(Security.TUExport))
            {
                userAccess.SecurityType |= SecurityType.Export;
            }
            return userAccess;
        }

        /// <summary>
        /// Create entities for repository
        /// </summary>
        /// <returns>Business Entity</returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            CreateBusinessEntitiesInternal();
            return _businessEntity;
        }

        /// <summary>
        /// Get Update Expression
        /// </summary>
        /// <param name="model">Model for ReceiptHeader</param>
        /// <returns>Expression</returns>
        protected override Expression<Func<ReceiptHeader, bool>> GetUpdateExpression(ReceiptHeader model)
        {
            return receiptNumber =>
                (receiptNumber.ReceiptNumber == (model.ReceiptNumber));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create Business Entities Internal
        /// </summary>
        private void CreateBusinessEntitiesInternal()
        {
            _businessEntity = OpenEntity(ReceiptHeader.EntityName);
        }

        #endregion

    }
}