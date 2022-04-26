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
using System;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SourceCode Internal Controller
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SourceCode"/></typeparam>
    public class SourceCodeControllerInternal<T> : BaseExportImportControllerInternal<T, ISourceCodeService<T>>
        where T : SourceCode, new()
    {
        #region Private variables
        #endregion

        #region Constructor
        /// <summary>
        /// New instance of <see cref="SourceCodeControllerInternal{T}"/>
        /// </summary>
        /// <param name="context">Context</param>
        public SourceCodeControllerInternal(Context context)
            : base(context)
        {
        }
        #endregion

        #region Internal methods
        /// <summary>
        /// Get Source Code
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Json object for Source Code </returns>
        internal SourceCodeViewModel<T> Get(string sourceLedger, string sourceType)
        {
            if (!string.IsNullOrEmpty(sourceLedger) && !string.IsNullOrEmpty(sourceType))
            {
                var data = Service.GetByIds(sourceLedger, sourceType);
                var userMessage = new UserMessage(data);

                return GetViewModel(data, userMessage);
            }
            else
            {
                return Create();
            }
        }

        /// <summary>
        /// Create a SourceCode
        /// </summary>
        /// <returns>JSON object for SourceCode</returns>
        internal SourceCodeViewModel<T> Create()
        {
            var viewModel = GetViewModel(new T(), null);
            viewModel.UserAccess = GetAccessRights();

            return viewModel;
        }

        /// <summary>
        /// Add a SourceCode
        /// </summary>
        /// <param name="model">Model for SourceCode</param>
        /// <returns>JSON object for SourceCode</returns>
        internal SourceCodeViewModel<T> Add(T model)
        {
            var data = Service.Add(model);
            var userMessage = new UserMessage(data,
                string.Format(CommonResx.AddSuccessMessage, SourceCodeResx.SourceCode, data.SourceLedger + "-" + data.SourceType));

            return GetViewModel(data, userMessage);
       }

        /// <summary>
        /// Update a SourceCode
        /// </summary>
        /// <param name="model">Model for SourceCode</param>
        /// <returns>JSON object for SourceCode</returns>
        internal SourceCodeViewModel<T> Save(T model)
        {
            var data = Service.Save(model);
            var userMessage = new UserMessage(data, CommonResx.SaveSuccessMessage);

            return GetViewModel(data, userMessage);
        }

        /// <summary>
        /// Delete a SourceCode
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>JSON object for SourceCode</returns>
        internal SourceCodeViewModel<T> Delete(string sourceLedger, string sourceType)
        {
            Expression<Func<T, bool>> filter = param => param.SourceLedger == sourceLedger && param.SourceType == sourceType;
            var data = Service.Delete(filter);
            var userMessage = new UserMessage(data,
                string.Format(CommonResx.DeleteSuccessMessage, SourceCodeResx.SourceCode, data.SourceLedger + "-" + data.SourceType));

            return GetViewModel(data, userMessage);
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Generic routine to return a view model for Source Code
        /// </summary>
        /// <param name="model">Model for Source Code</param>
        /// <param name="userMessage">User Message for Source Code</param>
        /// <returns>View Model for Source Code</returns>
        private SourceCodeViewModel<T> GetViewModel(T model, UserMessage userMessage)
        {
            return new SourceCodeViewModel<T>
            {
                Data = model,
                UserMessage = userMessage
            };
        }

        #endregion

    }
}