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
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;

#endregion

namespace ValuedPartner.TU.BusinessRepository
{
    /// <summary>
    /// Class SourceJournalProfile Repository
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class SourceJournalProfileRepository<T> : FlatRepository<T>, ISourceJournalProfileEntity<T>
        where T : SourceJournalProfile, new()
    {
        #region Variables

        /// <summary>
        /// Mapper
        /// </summary>
        private ModelMapper<T> _mapper;

        /// <summary>
        /// Business Entity
        /// </summary>
        private IBusinessEntity _businessEntity;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileRepository(Context context)
            : base(context, new SourceJournalProfileMapper<T>(context), ActiveFilter)
        {
            SetFilter(context);
        }

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public SourceJournalProfileRepository(Context context, IBusinessEntitySession session)
            : base(context, new SourceJournalProfileMapper<T>(context), ActiveFilter, session)
        {
            SetFilter(context);
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
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <returns>Expression</returns>
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return entity => 
                (entity.SourceJournalName.StartsWith(model.SourceJournalName));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// ActiveFilter Condition
        /// </summary>
        /// <value>The active filter</value>
        private static Expression<Func<T, bool>> ActiveFilter
        {
            get { return null; }
        }

        /// <summary>
        /// Creates the business entities
        /// </summary>
        private void CreateBusinessEntitiesInternal()
        {
            _businessEntity = OpenEntity(SourceJournalProfile.EntityName);
        }

        /// <summary>
        /// Set Filter
        /// </summary>
        /// <param name="context">Context</param>
        private void SetFilter(Context context)
        {
            ValidRecordFilter = null;
            ValidRecordFilter = (model => 
                !string.IsNullOrEmpty(model.SourceJournalName));

            _mapper = new SourceJournalProfileMapper<T>(context);
        }

        #endregion
    }
}