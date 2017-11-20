/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

#region Namespace
<<<<<<< HEAD
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Interfaces.BusinessRepository;
=======
using ValuedParter.TU.Models;
using ValuedParter.TU.Interfaces.Services;
using ValuedParter.TU.Interfaces.BusinessRepository;
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Base;
using System;
using System.Linq.Expressions;

#endregion

<<<<<<< HEAD
namespace ValuedPartner.TU.Interfaces.Services
=======
namespace ValuedParter.TU.Interfaces.Services
>>>>>>> ff0042d533a7308467f0048872236ad8afb584d2
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
