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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;

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
        /// The _source journal profile
        /// </summary>
        private IBusinessEntity _businessEntity;

        /// <summary>
        /// SourceLedgerFieldName
        /// </summary>
        private const string SourceLedgerFieldName = "SRCELDGR";

        /// <summary>
        /// SourceTypeFieldName
        /// </summary>
        private const string SourceTypeFieldName = "SRCETYPE";

        /// <summary>
        /// Context
        /// </summary>
        private readonly Context _context;

        /// <summary>
        /// Source Journal Profile Count
        /// </summary>
        private const int SourceJournalProfileCount = 50;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileRepository(Context context)
            : base(context, new SourceJournalProfileMapper<T>(context), ActiveFilter)
        {
            _context = context;
        }

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public SourceJournalProfileRepository(Context context, IBusinessEntitySession session)
            : base(context, new SourceJournalProfileMapper<T>(context), ActiveFilter, session)
        {
            _context = context;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// ActiveFilter Condition
        /// </summary>
        /// <value>The active filter.</value>
        private static Expression<Func<T, Boolean>> ActiveFilter
        {
            get { return null; }
        }

        /// <summary>
        /// Create entities for repository
        /// </summary>
        /// <returns>IBusinessEntity.</returns>
        protected override IBusinessEntity CreateBusinessEntities()
        {
            CreateBusinessEntitiesInternal();
            return _businessEntity;
        }

        /// <summary>
        /// Get Update Expression
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Expression</returns>
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return c => c.SourceJournalName.StartsWith(model.SourceJournalName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetById
        /// </summary>
        /// <typeparam name="TKey">Key</typeparam>
        /// <param name="id">Id</param>
        /// <returns>T</returns>
        public override T GetById<TKey>(TKey id)
        {
            _businessEntity = CreateBusinessEntities();
            CheckRights(_businessEntity, SecurityType.Inquire);

            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceJournalName, id);
            var exist = _businessEntity.Read(false);

            return new T
            {
                SourceJournalName = _businessEntity.GetValue<string>(SourceJournalProfile.Index.SourceJournalName),
                ETag = _businessEntity.ETag,
                Exist = exist
            };
        }

        /// <summary>
        /// Gets SourceJournal Detail
        /// </summary>
        /// <param name="id">sourceJournalName</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public EnumerableResponse<SourceCode> GetSourceJournal(string id, int pageNumber, int pageSize)
        {
            _businessEntity = CreateBusinessEntities();

            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceJournalName, id);
            var exist = _businessEntity.Read(false);

            var sourceJournal = new List<SourceCode>();

            int dataIndex = 1;

            var totalResultCount = 0;

            if (exist)
            {
                for (var i = 1; i <= SourceJournalProfileCount; i++)
                {
                    var iStringVal = i.ToString(CultureInfo.InvariantCulture);

                    string sourceLedgerColumnName;
                    string sourceTypeColumnName;

                    if (i <= 9)
                    {
                        sourceLedgerColumnName = SourceLedgerFieldName + "0" + iStringVal;
                        sourceTypeColumnName = SourceTypeFieldName + "0" + iStringVal;
                    }
                    else
                    {
                        sourceLedgerColumnName = SourceLedgerFieldName + iStringVal;
                        sourceTypeColumnName = SourceTypeFieldName + iStringVal;
                    }

                    var sourceLedger = _businessEntity.GetValue<string>(sourceLedgerColumnName);
                    var sourceType = _businessEntity.GetValue<string>(sourceTypeColumnName);

                    if (!string.IsNullOrEmpty(sourceLedger) && !string.IsNullOrEmpty(sourceType))
                    {
                        using (var sourceCodeRepository = Resolve<ISourceCodeEntity<SourceCode>>())
                        {
                            var sourceCode = sourceCodeRepository.GetByIds(sourceLedger, sourceType);
                            var source = sourceLedger + "-" + sourceType;
                            sourceCode.Source = source;
                            sourceCode.PreviousSourceValue = source;
                            sourceCode.SerialNumber = dataIndex;
                            sourceCode.DisplayIndex = dataIndex;
                            sourceJournal.Add(sourceCode);
                        }

                        totalResultCount = totalResultCount + 1;
                    }
                    else
                    {
                        break;
                    }

                    dataIndex = dataIndex + 1;
                }
            }

            //If Page size and Page Numer is -1 - Send all the records
            if (sourceJournal.Count > 0 && (pageSize >= 0 && pageNumber >= 0))
            {
                sourceJournal = sourceJournal.Skip(pageNumber * pageSize).Take(pageSize).ToList();
            }

            return new EnumerableResponse<SourceCode> { Items = sourceJournal, TotalResultsCount = totalResultCount };

        }

        /// <summary>
        /// Save Source Journal Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override T Save(T model)
        {
            _businessEntity = CreateBusinessEntities();

            var sourceCodes = model.SourceCodeList.Items.ToList();

            var sourceCodeCount = sourceCodes.Count;

            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceJournalName, model.SourceJournalName);
            var exist = _businessEntity.Read(false);

            CheckETag(_businessEntity, model);

            for (var i = 1; i <= sourceCodeCount; i++)
            {
                var iStringVal = i.ToString(CultureInfo.InvariantCulture);

                string sourceLedgerColumnName;
                string sourceTypeColumnName;

                if (i <= 9)
                {
                    sourceLedgerColumnName = SourceLedgerFieldName + "0" + iStringVal;
                    sourceTypeColumnName = SourceTypeFieldName + "0" + iStringVal;
                }
                else
                {
                    sourceLedgerColumnName = SourceLedgerFieldName + iStringVal;
                    sourceTypeColumnName = SourceTypeFieldName + iStringVal;
                }

                var sourceCode = sourceCodes[i - 1].Source;
                string sourceLedgerFieldValue = string.Empty;
                string sourceTypeFieldValue = string.Empty;

                if (!string.IsNullOrEmpty(sourceCode))
                {
                    if (!sourceCodes[i - 1].IsDeleted)
                    {
                        var source = sourceCode.Split('-');

                        if (source.Length > 1)
                        {
                            sourceLedgerFieldValue = source[0];
                            sourceTypeFieldValue = source[1];
                        }
                        else
                        {
                            sourceLedgerFieldValue = sourceCode;
                        }
                    }
                }

                sourceLedgerFieldValue = sourceCodes[i - 1].IsDeleted ? string.Empty : sourceLedgerFieldValue;
                sourceTypeFieldValue = sourceCodes[i - 1].IsDeleted ? string.Empty : sourceTypeFieldValue;

                _businessEntity.SetValue(sourceLedgerColumnName, sourceLedgerFieldValue);
                _businessEntity.SetValue(sourceTypeColumnName, sourceTypeFieldValue);
            }

            if (exist)
            {
                _businessEntity.Update();
            }
            else
            {
                _businessEntity.Insert();
            }

            model.ETag = _businessEntity.ETag;
            model.Exist = true;

            return model;
        }

        /// <summary>
        /// Get rights
        /// </summary>
        /// <returns>UserAccess</returns>
        public override UserAccess GetAccessRights()
        {
            var userAccess = GetAccessRights(OpenEntity(SourceJournalProfile.EntityName));
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
        /// CheckSourceCode
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>True/False</returns>
        public bool CheckValidSourceCode(string sourceLedger, string sourceType)
        {
            _businessEntity = CreateBusinessEntities();
            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceCodeID01, sourceLedger);
            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceType01, sourceType, true);
            return true;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Source Journal Name</param>
        /// <returns>SourceJournalProfile</returns>
        public T Delete(string id)
        {
            _businessEntity = CreateBusinessEntities();
            _businessEntity.SetValue(SourceJournalProfile.Fields.SourceJournalName, id);

            if (_businessEntity.Read(false))
            {
                _businessEntity.Delete();
            }
            else
            {
                throw ExceptionHelper.RowNotFoundException(CommonResx.DeleteFailedNoRecordMessage);
            }

            _businessEntity.Cancel();

            var mapper = new SourceJournalProfileMapper<T>(_context);
            return mapper.Map(_businessEntity);
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Creates the business entities
        /// </summary>
        private void CreateBusinessEntitiesInternal()
        {
            _businessEntity = OpenEntity(SourceJournalProfile.EntityName);
            _businessEntity.Compose(new[] { _businessEntity.View });
        }

        #endregion
    }
}