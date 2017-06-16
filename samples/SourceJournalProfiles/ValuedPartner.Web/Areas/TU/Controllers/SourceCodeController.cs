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

using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.Web.Areas.TU.Models;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SourceCode Public Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceCode"/></typeparam>
    public class SourceCodeController<T> : MultitenantControllerBase<SourceCodeViewModel<T>>
        where T : SourceCode, new()
    {
        #region Public variables

        /// <summary>
        /// Gets or sets the internal controller
        /// </summary>
        public SourceCodeControllerInternal<T> ControllerInternal { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceCode
        /// </summary>
        /// <param name="container">Unity Container</param>
        public SourceCodeController(IUnityContainer container)
            : base(container,"TUSourceCode")
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
            ControllerInternal = new SourceCodeControllerInternal<T>(Context);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get default Index page
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Source Code View</returns>
        public virtual ActionResult Index(string sourceLedger, string sourceType)
        {
            SourceCodeViewModel<T> model;
            if (string.IsNullOrEmpty(sourceLedger) && string.IsNullOrEmpty(sourceType))
            {
                model = new SourceCodeViewModel<T> { UserAccess = ControllerInternal.GetAccessRights() };
            }
            else
            {
                model = ControllerInternal.Get(sourceLedger, sourceType);
                model.UserAccess = ControllerInternal.GetAccessRights();
            }
            return View(model);
        }

        /// <summary>
        /// Get Source Codes
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Json object for Source Code Model</returns>
        [HttpPost]
        
        public virtual JsonNetResult Get(string sourceLedger, string sourceType)
        {
            try
            {
                return JsonNet(ControllerInternal.Get(sourceLedger, sourceType));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException,
                        SourceCodeResx.SourceCode));
            }
        }

        /// <summary>
        /// Add new Source Code
        /// </summary>
        /// <param name="model">Source Code Model</param>
        /// <returns>Json object for Source Code</returns>
        [HttpPost]
        
        public virtual JsonNetResult Add(T model)
        {
            try
            {
                ViewModelBase<ModelBase> viewModel;
                return ValidateModelState(ModelState, out viewModel)
                    ? JsonNet(ControllerInternal.Add(model))
                    : JsonNet(viewModel);
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException,
                        SourceCodeResx.SourceCode));
            }
        }

        /// <summary>
        /// Create Vendor
        /// </summary>
        /// <returns>Vendor</returns>
        [HttpPost]
        
        public virtual JsonNetResult Create()
        {
            return JsonNet(ControllerInternal.Create());
        }

        /// <summary>
        /// Update Source Code
        /// </summary>
        /// <param name="model">Source Code Model</param>
        /// <returns>Json object for Source Code</returns>
        [HttpPost]
        
        public virtual JsonNetResult Save(T model)
        {
            try
            {
                ViewModelBase<ModelBase> viewModel;
                return ValidateModelState(ModelState, out viewModel)
                    ? JsonNet(ControllerInternal.Save(model))
                    : JsonNet(viewModel);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException));
            }
        }

        /// <summary>
        /// Delete Source Code
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Json object for Source Code</returns>
        [HttpPost]
        
        public virtual JsonNetResult Delete(string sourceLedger, string sourceType)
        {
            try
            {
                return JsonNet(ControllerInternal.Delete(sourceLedger, sourceType));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException,
                        SourceCodeResx.SourceCode));
            }
        }

        #endregion
    }
}