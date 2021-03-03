// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Core.Web.Controllers;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using System.Collections.Generic;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SegmentCodes Public Controller
    /// </summary>
    public class SegmentCodesGridController : BaseDataServiceController
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="container"></param>
        public SegmentCodesGridController(IUnityContainer container) : base(container)
        {
        }
        /// <summary>
        /// Initializes the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewID"></param>
        /// <param name="record"></param>
        public override ActionResult Create(string viewID, IDictionary<string, string> record)
        {
            try
            {
                // get the business entity from the session pool
                var entity = GetBusinessEntityFromSessionPool(viewID);
                CreateNewRecord(entity, record);
                
                if (record.Count != 0)
                {
                    entity.SetValue("SEGMENT", record["SEGMENT"], true);
                }
                     
                return SerializeCurrentRecord(entity);

            }
            catch (BusinessException e)
            {
                return ConstructJsonErrors(e.Errors);
            }
            catch
            {
                return ConstructJsonErrors(new List<EntityError> { new EntityError() { Message = CommonResx.GridCreateNewRecordFailed } });
            }
        }
    }
}