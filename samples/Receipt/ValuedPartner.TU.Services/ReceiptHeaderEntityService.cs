/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

#region Namespace
using ValuedParter.TU.Models;
using ValuedParter.TU.Interfaces.Services;
using ValuedParter.TU.Interfaces.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Base;
using System;
using System.Linq.Expressions;

#endregion

namespace ValuedParter.TU.Interfaces.Services
{
    /// <summary>
    /// A Class for TU Receipt service.
    /// </summary>
    /// <typeparam name="T">Model of type ReceiptHeader</typeparam>
    public class ReceiptHeaderEntityService : FlatService<ReceiptHeader, IReceiptHeaderEntity>, IReceiptHeaderService 
    {
        #region Constructor

        /// <summary>
        /// To set request context
        /// </summary>
        /// <param name="context">Request Context</param>    
        public ReceiptHeaderEntityService(Context context)
            : base(context)
        {
            
        }

        #endregion

    }
}
