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

using System;
using System.Linq;
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
using System.Collections.Generic;

#endregion

namespace ValuedPartner.TU.BusinessRepository
{
    /// <summary>
    /// Class SegmentCodes Repository
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class SegmentCodesRepository<T> : FlatRepository<T>, ISegmentCodesEntity<T>
        where T : SegmentCodes, new()
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
        /// Constructor for SegmentCodes
        /// </summary>
        /// <param name="context">Context</param>
        public SegmentCodesRepository(Context context)
            : base(context, new SegmentCodesMapper<T>(context), ActiveFilter)
        {
            SetFilter(context);
        }

        /// <summary>
        /// Constructor for SegmentCodes
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Business Entity Session</param>
        public SegmentCodesRepository(Context context, IBusinessEntitySession session)
            : base(context, new SegmentCodesMapper<T>(context), ActiveFilter, session)
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
        /// <param name="model">Model for SegmentCodes</param>
        /// <returns>Expression</returns>
        protected override Expression<Func<T, bool>> GetUpdateExpression(T model)
        {
            return entity => 
                (entity.SegmentNumber.Equals(model.SegmentNumber) &&
                entity.SegmentCode.StartsWith(model.SegmentCode));
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
            _businessEntity = OpenEntity(SegmentCodes.EntityName);
        }

        /// <summary>
        /// Set Filter
        /// </summary>
        /// <param name="context">Context</param>
        private void SetFilter(Context context)
        {
            ValidRecordFilter = null;
            ValidRecordFilter = (model => 
                !model.SegmentNumber.Equals(0) &&
                !string.IsNullOrEmpty(model.SegmentCode));

            _mapper = new SegmentCodesMapper<T>(context);
        }

        /// <summary>
        /// Check whether segmentcode can be deletable or not.
        /// </summary>
        /// <param name="segmentCodes">An IEnumerable type of SegmentCode</param>
        /// <param name="deletableSegmentCodes">List of Segment Codes which are allowed to be deleted</param>
        /// <returns>Returns True if all of the records passed are agreed to be removed.</returns>
        public virtual bool AreSegmentCodesDeletable(IEnumerable<T> segmentCodes, ref List<string> deletableSegmentCodes)
        {
            var entity = CreateBusinessEntities();

            foreach (var segment in segmentCodes.Where(segment => !segment.IsNewLine))
            {
                var segmentCodesMapper = new SegmentCodesMapper<T>(Context);

                segmentCodesMapper.MapKey(segment, entity);

                entity.Read(false);

                entity.Delete();

                deletableSegmentCodes.Add(segment.SegmentCode);
            }

            // If segment code deleted without any exception then return true.
            // If exception occurs then it will skip below line..
            return true;
        }

        /// <summary>
        /// Check for segment code duplication
        /// </summary>
        /// <param name="segmentNumber">The segment number</param>
        /// <param name="segmentCode">The segment code</param>
        /// <returns>Returns True if duplicate segmentCode found</returns>
        public virtual bool Exists(string segmentNumber, string segmentCode)
        {
            var entity = CreateBusinessEntities();

            entity.SetValue(SegmentCodes.Index.SegmentNumber, segmentNumber);
            entity.SetValue(SegmentCodes.Index.SegmentCode, segmentCode);

            // Return true is duplicate segmentcode is not present in database.
            return entity.Exists;
        }

        /// <summary>
        /// Save segment code functionality.
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Returns an IEnumerable object</returns>
        public virtual IEnumerable<T> Save(IEnumerable<T> model)
        {
            var entity = CreateBusinessEntities();

            CheckRights(entity, SecurityType.Modify);

            var segmentCodes = model as T[] ?? model.ToArray();
            var segmentCodesMapper = new SegmentCodesMapper<T>(Context);

            foreach (var segmentCode in segmentCodes.Where(segment => segment.IsDeleted))
            {
                segmentCodesMapper.MapKey(segmentCode, entity);

                entity.Read(false);

                if (segmentCode.IsDeleted)
                {
                    entity.Delete();
                }
            }

            foreach (var segmentCode in segmentCodes.Where(segment => !segment.IsDeleted))
            {
                segmentCodesMapper.MapKey(segmentCode, entity);

                entity.Read(false);

                segmentCodesMapper.Map(segmentCode, entity);

                if (entity.Exists && !segmentCode.IsNewLine)
                {
                    entity.Update();
                }
                else
                {
                    entity.Insert();
                }
            }

            entity.Post();

            return segmentCodes;
        }

        #endregion
    }
}