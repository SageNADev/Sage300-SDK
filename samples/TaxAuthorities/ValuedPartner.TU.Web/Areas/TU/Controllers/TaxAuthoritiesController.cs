// The MIT License (MIT) 
// Copyright (c) 1994-2025 The Sage Group plc or its licensors.  All rights reserved.
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

using Unity;
using System.Web.Mvc;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// TaxAuthority Public Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="TaxAuthorities"/></typeparam>
    public class TaxAuthoritiesController<T> : MultitenantControllerBase<TaxAuthoritiesViewModel<T>>
        where T : TaxAuthorities, new()
    {
        #region Public variables

        /// <summary>
        /// Gets or sets the internal controller
        /// </summary>
        public TaxAuthoritiesControllerInternal<T> ControllerInternal { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TaxAuthority
        /// </summary>
        /// <param name="container">Unity Container</param>
        public TaxAuthoritiesController(IUnityContainer container)
            : base(container,"TUTaxAuthorities")
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
            ControllerInternal = new TaxAuthoritiesControllerInternal<T>(Context);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load screen
        /// </summary>
        /// <param name="id">Id for TaxAuthority</param>
        /// <returns>JSON object for TaxAuthority</returns>
        public virtual ActionResult Index(string id)
        {
            return ViewWithCatch(() => ControllerInternal.GetById(id),
                CommonResx.GetFailedMessage, TaxAuthoritiesResx.TaxAuthority);
        }

        /// <summary>
        /// Get TaxAuthority
        /// </summary>
        /// <param name="id">Id for TaxAuthority</param>
        /// <returns>JSON object for TaxAuthority</returns>
        [HttpPost]
        public virtual JsonNetResult Get(string id)
        {
            return CallWithCatch(() => ControllerInternal.GetById(id),
                CommonResx.GetFailedMessage, TaxAuthoritiesResx.TaxAuthority);
        }

        /// <summary>
        /// Add TaxAuthority
        /// </summary>
        /// <param name="model">Model for TaxAuthority</param>
        /// <returns>JSON object for TaxAuthority</returns>
        [HttpPost]
        public virtual JsonNetResult Add(T model)
        {
            if (!ValidateModelState(ModelState, out ViewModelBase<ModelBase> viewModel))
            {
                return JsonNet(viewModel);
            }

            return CallWithCatch(() => ControllerInternal.Add(model),
                CommonResx.AddFailedMessage, TaxAuthoritiesResx.TaxAuthority);
        }

        /// <summary>
        /// Create TaxAuthority
        /// </summary>
        /// <returns>JSON object for TaxAuthorities</returns>
        [HttpPost]
        public virtual JsonNetResult Create()
        {
            return CallWithCatch(() => ControllerInternal.Create(),
                CommonResx.UnhandledExceptionMessage);
        }

        /// <summary>
        /// Update TaxAuthority
        /// </summary>
        /// <param name="model">Model for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        [HttpPost]
        public virtual JsonNetResult Save(T model)
        {
            if (!ValidateModelState(ModelState, out ViewModelBase<ModelBase> viewModel))
            {
                return JsonNet(viewModel);
            }

            return CallWithCatch(() => ControllerInternal.Save(model),
                CommonResx.SaveFailedMessage);
        }

        /// <summary>
        /// Delete TaxAuthority
        /// </summary>
        /// <param name="id">Id for TaxAuthorities</param>
        /// <returns>JSON object for TaxAuthorities</returns>
        [HttpPost]
        public virtual JsonNetResult Delete(string id)
        {
            return CallWithCatch(() => ControllerInternal.Delete(id),
                CommonResx.DeleteFailedMessage, TaxAuthoritiesResx.TaxAuthority);
        }

        /// <summary>
        /// Get Account
        /// </summary>
        /// <param name="id">account number</param>
        /// <returns>account description</returns>
        [HttpPost]
        public JsonNetResult GetAccount(string id)
        {
            return CallWithCatch(() => ControllerInternal.GetGlAccount(id).Description,
                CommonResx.GetFailedMessage, id);
        }

        /// <summary>
        /// Get currency description
        /// </summary>
        /// <param name="id">currency code</param>
        /// <returns>currency description</returns>
        [HttpPost]
        public JsonNetResult GetCurrencyDescription(string id)
        {
            return CallWithCatch(() => ControllerInternal.GetCurrencyDescription(id),
                CommonResx.GetFailedMessage, id);
        }

        #endregion
    }
}