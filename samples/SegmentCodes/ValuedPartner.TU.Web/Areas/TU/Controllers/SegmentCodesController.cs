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

using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Filter = Sage.CA.SBS.ERP.Sage300.Common.Models.Filter;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SegmentCodes Public Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class SegmentCodesController<T> : MultitenantControllerBase<SegmentCodesViewModel<T>>
        where T : SegmentCodes, new()
    {
        #region Public variables

        /// <summary>
        /// Gets or sets the internal controller
        /// </summary>
        public SegmentCodesControllerInternal<T> ControllerInternal { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SegmentCodes
        /// </summary>
        /// <param name="container">Unity Container</param>
        public SegmentCodesController(IUnityContainer container)
            : base(container,"TUSegmentCodes")
        {
        }

        #endregion

        #region Initialize MultitenantControllerBase

        /// <summary>
        /// Override Initialize method
        /// </summary>
        /// <param name="requestContext">Request Context</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ControllerInternal = new SegmentCodesControllerInternal<T>(Context);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load screen
        /// </summary>
        /// <param name="id">Id for SegmentCodes</param>
        /// <returns>JSON object for SegmentCodes</returns>
        public virtual ActionResult Index(string id)
        {
            SegmentCodesViewModel<T>  viewModel;

            try
            {
                viewModel = ControllerInternal.GetById(id);
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException,
                        SegmentCodesResx.SegmentNumber));
            }

            return View(viewModel);
        }

        /// <summary>
        /// Get segment code detail based on pageNumber
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="filters">Segment Number</param>
        /// <param name="model"></param>
        /// <param name="isCacheRemovable"></param>
        /// <returns>JsonNetResult.</returns>
        public virtual JsonNetResult Get(SegmentCodesViewModel<T> model, int index, int pageNumber, int pageSize,
            IList<IList<Filter>> filters, bool isCacheRemovable)
        {
            try
            {
                var segmentCode = ControllerInternal.Get(model, index, pageNumber, pageSize, filters, isCacheRemovable);

                return JsonNet(segmentCode);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(string.Empty, businessException));
            }
        }

        /// <summary>
        /// Check whether segmentcode can be deletable or not.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JsonNetResult</returns>

        [HttpPost]
        public virtual JsonNetResult AreSegmentCodesDeletable(EnumerableResponse<T> model)
        {
            var deletableSegmentCodes = new List<string>();
            try
            {
                return JsonNet(ControllerInternal.AreSegmentCodesDeletable(model.Items, ref deletableSegmentCodes));
            }
            catch (BusinessException businessException)
            {
                var userMessage = new UserMessage { Errors = businessException.Errors };
                var viewModel = new SegmentCodesViewModel<T>
                {
                    IsSegmentCodeUsed = true,
                    DeletedSegmentCodes = deletableSegmentCodes,
                    UserMessage = userMessage
                };
                return JsonNet(viewModel);
            }
        }

        /// <summary>
        /// Checks for the segment code duplication
        /// </summary>
        /// <param name="model"></param>
        /// <param name="segmentNumber">The segmentnumber.</param>
        /// <param name="segmentCode">The segment code.</param>
        /// <returns>JsonNetResult.</returns>
        [HttpPost]
        public virtual JsonNetResult Exists(EnumerableResponse<T> model, string segmentNumber, string segmentCode)
        {
            try
            {
                return JsonNet(ControllerInternal.Exists(model, segmentNumber, segmentCode));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(SegmentCodesResx.SegmentCode, businessException));
            }
        }

        /// <summary>
        /// Save Segment Code
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JsonNetResult.</returns>
        [HttpPost]
        public virtual JsonNetResult Save(EnumerableResponse<T> model)
        {
            try
            {
                return JsonNet(ControllerInternal.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException));
            }
        }

        #endregion
    }
}