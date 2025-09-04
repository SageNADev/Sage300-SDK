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
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
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
        /// Controller internal
        /// </summary>
        public SourceJournalProfileControllerInternal<T> ControllerInternal;

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
        /// Index
        /// </summary>
        /// <param name="id">Source Journal</param>
        /// <returns>View</returns>
        public ActionResult Index(string id = null)
        {
            return ViewWithCatch(() => ControllerInternal.Get(id),
                CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        /// <summary>
        /// Create Source Journal 
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            return CallWithCatch(() => ControllerInternal.Create(),
                CommonResx.UnhandledExceptionMessage);
        }

        /// <summary>
        /// Get Source Journal 
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Get(string id)
        {
            return CallWithCatch(() => ControllerInternal.Get(id),
                CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        /// <summary>
        /// Get Source Journal
        /// </summary>
        /// <param name="insertIndex">insertIndex</param>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="model">model</param>
        /// <param name="bloadSourceJournalChange">bloadSourceJournalChange</param>
        /// <returns>JsonNetResult</returns>
        public virtual JsonNetResult GetSourceJournal(int insertIndex, int pageNumber, int pageSize,
            T model, bool bloadSourceJournalChange)
        {
            if (bloadSourceJournalChange)
            {
                return CallWithCatch(() => ControllerInternal.CloneSourceJournal(insertIndex, pageNumber, pageSize, model),
                    CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
            }

            return CallWithCatch(() => ControllerInternal.GetSourceJournal(insertIndex, pageNumber, pageSize, model),
                CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        /// <summary>
        /// Delete Source Journal Profile
        /// </summary>
        /// <param name="id">Source Journal Profile</param>
        /// <returns>Json object for Source Journal Profile</returns>
        [HttpPost]
        public virtual JsonNetResult Delete(string id)
        {
            return CallWithCatch(() => ControllerInternal.Delete(id),
                CommonResx.DeleteFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        /// <summary>
        /// Update Source Journal Profile
        /// </summary>
        /// <param name="model">Source Journal Profilee Model</param>
        /// <returns>Json object for Source Journal Profile</returns>
        [HttpPost]
        public virtual JsonNetResult Save(T model)
        {
            return CallWithCatch(() => ControllerInternal.Save(model),
                CommonResx.SaveFailedMessage);
        }

        /// <summary>
        /// Get Source Code by ID
        /// </summary>
        /// <param name="sourceLedger">SourceLedger</param>
        /// <param name="sourceType">SourceType</param>
        /// <returns>JsonNetResult</returns>
        public virtual JsonNetResult GetSourceCodeById(string sourceLedger, string sourceType)
        {
            return CallWithCatch(() => ControllerInternal.GetSourceCodeById(sourceLedger, sourceType),
                CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        /// <summary>
        /// IsExist
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="id">SourceJournalName</param>
        /// <returns>JsonNetResult</returns>
        [HttpPost]
        public virtual JsonNetResult IsExist(string source, string id)
        {
            return CallWithCatch(() => ControllerInternal.IsExist(source, id),
                CommonResx.GetFailedMessage, SourceJournalProfileResx.SourceJournalProfile);
        }

        #endregion
    }
}