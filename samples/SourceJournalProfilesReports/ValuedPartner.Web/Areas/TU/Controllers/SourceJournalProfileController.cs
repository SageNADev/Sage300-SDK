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
    /// SourceJournalProfile Public Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class SourceJournalProfileController<T> : MultitenantControllerBase<SourceJournalProfileViewModel<T>>
        where T : SourceJournalProfile, new()
    {
        #region Public variables

        /// <summary>
        /// Gets or sets the internal controller
        /// </summary>
        public SourceJournalProfileControllerInternal<T> ControllerInternal { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SourceJournalProfile
        /// </summary>
        /// <param name="container">Unity Container</param>
        public SourceJournalProfileController(IUnityContainer container)
            : base(container,"TUSourceJournalProfile")
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
            ControllerInternal = new SourceJournalProfileControllerInternal<T>(Context);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load screen
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        public virtual ActionResult Index(string id)
        {
            SourceJournalProfileViewModel<T>  viewModel;

            try
            {
                viewModel = !string.IsNullOrEmpty(id) ? ControllerInternal.GetById(id) : ControllerInternal.Create();
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException,
                        SourceJournalProfileResx.SourceJournalName));
            }

            return View(viewModel);
        }

        /// <summary>
        /// Get SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        [HttpPost]
        public virtual JsonNetResult Get(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return JsonNet(ControllerInternal.GetById(id));
                }
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException,
                        SourceJournalProfileResx.SourceJournalName));
            }

            return JsonNet(new SourceJournalProfileViewModel<T>());
        }

        /// <summary>
        /// Add SourceJournalProfile
        /// </summary>
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
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
                        SourceJournalProfileResx.SourceJournalName));
            }
        }

        /// <summary>
        /// Create SourceJournalProfile
        /// </summary>
        /// <returns>JSON object for SourceJournalProfile</returns>
        [HttpPost]
        public virtual JsonNetResult Create()
        {
            return JsonNet(ControllerInternal.Create());
        }

        /// <summary>
        /// Update SourceJournalProfile
        /// </summary>
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
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
        /// Delete SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        [HttpPost]
        public virtual JsonNetResult Delete(string id)
        {
            try
            {
                return JsonNet(ControllerInternal.Delete(id));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException,
                        SourceJournalProfileResx.SourceJournalName));
            }
        }

        #endregion
    }
}