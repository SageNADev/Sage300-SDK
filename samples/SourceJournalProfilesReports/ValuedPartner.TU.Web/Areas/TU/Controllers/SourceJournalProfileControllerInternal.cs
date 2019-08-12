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

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SourceJournalProfile Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceJournalProfile"/></typeparam>
    public class SourceJournalProfileControllerInternal<T> : BaseExportImportControllerInternal<T, ISourceJournalProfileService<T>>
        where T : SourceJournalProfile, new()
    {
        #region Private variables

        #endregion

        #region Constructor

        /// <summary>
        /// New instance of <see cref="SourceJournalProfileControllerInternal{T}"/>
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Create a SourceJournalProfile
        /// </summary>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Create()
        {
            var viewModel = GetViewModel(new T(), null);

            viewModel.UserAccess = GetAccessRights();

            return viewModel;
        }

        /// <summary>
        /// Get a SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> GetById(string id)
        {
            var data = Service.GetById(id);
            var userMessage = new UserMessage(data);

            return GetViewModel(data, userMessage);
        }

        /// <summary>
        /// Add a SourceJournalProfile
        /// </summary>
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Add(T model)
        {
            var data = Service.Add(model);

            var userMessage = new UserMessage(data,
                string.Format(CommonResx.AddSuccessMessage, SourceJournalProfileResx.SourceJournalName, data.SourceJournalName));

            return GetViewModel(data, userMessage);
       }

        /// <summary>
        /// Update a SourceJournalProfile
        /// </summary>
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Save(T model)
        {
            var data = Service.Save(model);
            var userMessage = new UserMessage(data, CommonResx.SaveSuccessMessage);

            return GetViewModel(data, userMessage);
        }

        /// <summary>
        /// Delete a SourceJournalProfile
        /// </summary>
        /// <param name="id">Id for SourceJournalProfile</param>
        /// <returns>JSON object for SourceJournalProfile</returns>
        internal SourceJournalProfileViewModel<T> Delete(string id)
        {
            Expression<Func<T, bool>> filter = param => param.SourceJournalName == id;
            var data = Service.Delete(filter);

            var userMessage = new UserMessage(data,
                string.Format(CommonResx.DeleteSuccessMessage, SourceJournalProfileResx.SourceJournalName, data.SourceJournalName));

            return GetViewModel(data, userMessage);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generic routine to return a view model for SourceJournalProfile
        /// </summary>
        /// <param name="model">Model for SourceJournalProfile</param>
        /// <param name="userMessage">User Message for SourceJournalProfile</param>
        /// <returns>View Model for SourceJournalProfile</returns>
        private SourceJournalProfileViewModel<T> GetViewModel(T model, UserMessage userMessage)
        {
            return new SourceJournalProfileViewModel<T>
            {
                Data = model,
                UserMessage = userMessage
            };
        }

        #endregion

	}
}