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
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums.ExportImport;
using Sage.CA.SBS.ERP.Sage300.Common.Models.ExportImport;
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;

#endregion

namespace ValuedPartner.TU.BusinessRepository
{
    /// <summary>
    /// Class SegmentCodes Repository
    /// </summary>
    public class SegmentCodesRepository : BaseHeaderDetailRepository, ISegmentCodesRepository
    {
        #region Variables

        /// <summary>
        /// Business Entity
        /// </summary>
        private IBusinessEntity _businessEntity;

        #endregion

        #region Constructor

		/// <summary>
        /// Sets Context and DBLink
        /// </summary>
        /// <param name="context">Context</param>
        public SegmentCodesRepository(Context context):base(context)
        {
            CreateBusinessEntities();
        }

        #endregion

        #region Protected/public methods

        /// <summary>
        /// Additional Access Check for Export and Import
        /// </summary>
        /// <returns>User Access</returns>
        public UserAccess GetAccessRights()
        {
            var userAccess = base.GetAccessRights(_businessEntity);
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
        /// Open business entity
        /// </summary>
        protected override void CreateBusinessEntities()
		{
            _businessEntity = OpenEntity(SegmentCodes.EntityName, true);
        }

		/// <summary>
        /// commit revision list to database 
        /// </summary>
        public void Post()
        {
            _businessEntity.Post();
        }


        #endregion

        #region Import/Export methods

        /// <summary>
        /// Get export or import business entity property
        /// </summary>
        /// <param name="option">export/import option, default to null</param>
        /// <param name="isExport">true if for export, default to false</param>
        /// <returns>Business Entity Property</returns>
        public override BusinessEntityProperty GetExportImportBusinessEntityProperty(string option = null, bool isExport = false)
        {
            return new BusinessEntityProperty(SegmentCodes.EntityName, ViewKeyType.UserSpecified);
        }

        #endregion
    }
}