
// Copyright (c) 2019 ValuedPartner  All rights reserved.

#region Namespace

using ValuedPartner.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using System.Linq.Expressions;
using System;
#endregion

namespace ValuedPartner.TU.Interfaces.BusinessRepository
{
    /// <summary>
    /// Interface ISegmentCodesRespository
    /// </summary>
    public interface ISegmentCodesRepository : ISecurity, ISecurityService, IImportExport 
    {
        /// <summary>
        /// Commit revision list to database 
        /// </summary>
        void Post();
    }
}
