// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Web.Areas.TU.Models;
#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SegmentCodes Public Controller
    /// </summary>
    public class SegmentCodesController : MultitenantControllerBase<SegmentCodesViewModel>
    {
        #region Public variables

        /// <summary>
        /// Gets or sets the internal controller
        /// </summary>
        public SegmentCodesControllerInternal ControllerInternal { get; set; }

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
            ControllerInternal = new SegmentCodesControllerInternal(Context);
        }

        #endregion

        #region Public methods

	    /// <summary>
        /// Load screen
        /// </summary>
        /// <returns>JSON object for SegmentCodes</returns>
        public virtual ActionResult Index()
        {
			ViewBag.SegmentCodesGrid = ControllerInternal.CreateGridDefinitionAndPreference(GetGridJsonFilePath("SegmentCodesGrid"));
            return ViewWithCatch(() => ControllerInternal.Create(), CommonResx.UnhandledExceptionMessage);
        }

        /// <summary>
        /// Commit revision list to database
        /// </summary>
        [HttpPost]
        public virtual JsonNetResult Post()
        {
            return CallWithCatch(() => ControllerInternal.Post(), CommonResx.SaveFailedMessage);
        }
        #endregion
    }
}