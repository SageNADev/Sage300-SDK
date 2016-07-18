// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;

#endregion

namespace ValuedPartner.TU.BusinessRepository
{
    /// <summary>
    /// Class SourceCode Repository
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceCode"/></typeparam>
    public class SourceCodeRepository<T> : FlatRepository<T>, ISourceCodeEntity<T>
        where T : SourceCode, new()
    {
        #region Variables

        /// <summary>
        /// GL0002 - Header entity
        /// </summary>
        private IBusinessEntity _businessEntity;

        /// <summary>
        /// Record Filter
        /// </summary>
        private static readonly Func<T, bool> RecordFilter = sourceCodes => !string.IsNullOrEmpty(sourceCodes.SourceLedger);

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceCode
        /// </summary>
        /// <param name="context">Context</param>
        public SourceCodeRepository(Context context)
            : base(context, new SourceCodeMapper<T>(context), null)
        {
            ValidRecordFilter = RecordFilter;
        }

        /// <summary>
        /// Constructor for SourceCode
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public SourceCodeRepository(Context context, IBusinessEntitySession session)
            : base(context, new SourceCodeMapper<T>(context), null, session)
        {
            ValidRecordFilter = RecordFilter;
        }

        #endregion

        #region Protected/public methods

        /// <summary>
        /// Get the source code based on primary key
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Source Code</returns>
        public T GetByIds(string sourceLedger, string sourceType)
        {
            _businessEntity = CreateBusinessEntities();
            CheckRights(_businessEntity, SecurityType.Inquire);

            _businessEntity.SetValue(SourceCode.Fields.SourceLedger, sourceLedger);
            _businessEntity.SetValue(SourceCode.Fields.SourceType, sourceType);
            if (!_businessEntity.Read(false))
            {
                return null;
            }
            var sourceCodeMapper = new SourceCodeMapper<T>(Context);
            return sourceCodeMapper.Map(_businessEntity);
        }

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
        /// <param name="model">Model for SourceCode</param>
        /// <returns>Expression</returns>
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return sourceCode =>
                (sourceCode.SourceLedger == (model.SourceLedger) &&
                sourceCode.SourceType == (model.SourceType));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create Business Entities Internal
        /// </summary>
        private void CreateBusinessEntitiesInternal()
        {
            _businessEntity = OpenEntity(SourceCode.EntityName);
        }

        #endregion
    }
}