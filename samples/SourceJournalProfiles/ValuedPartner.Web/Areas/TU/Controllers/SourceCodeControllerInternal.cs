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
using ValuedPartner.Web.Areas.TU.Models;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers
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
            var sourceCode = Service.GetByIds(sourceLedger, sourceType);

            return new SourceCodeViewModel<T>
            {
                Data = sourceCode,
                UserMessage = new UserMessage(sourceCode)
            };
        }


        /// <summary>
        /// Create Vendor
        /// </summary>
        /// <returns>Json object for Source Code </returns>
        internal SourceCodeViewModel<T> Create()
        {
            return new SourceCodeViewModel<T>();
        }

        /// <summary>
        /// Updates a Source Code
        /// </summary>
        /// <param name="model">Source Code Model</param>
        /// <returns>Json object for Source Code </returns>
        internal SourceCodeViewModel<T> Save(T model)
        {
            var data = Service.Save(model);

            return new SourceCodeViewModel<T>
            {
                Data = data,
                UserMessage = new UserMessage(data, CommonResx.SaveSuccessMessage)
            };
        }

        /// <summary>
        /// Add Source Code
        /// </summary>
        /// <param name="model">Source Code Model</param>
        /// <returns>Json object for Source Code </returns>
        internal SourceCodeViewModel<T> Add(T model)
        {
            var data = Service.Add(model);

            return new SourceCodeViewModel<T>
            {
                Data = data,
                UserMessage = new UserMessage(data, string.Format(CommonResx.AddSuccessMessage, SourceCodeResx.SourceCode, data.SourceLedger + "-" + data.SourceType))
            };
        }

        /// <summary>
        /// Deletes a Source Code for an Source Ledger and Source Type
        /// </summary>
        /// <param name="sourceLedger">Source Ledger</param>
        /// <param name="sourceType">Source Type</param>
        /// <returns>Json object for Source Code </returns>
        internal SourceCodeViewModel<T> Delete(string sourceLedger, string sourceType)
        {
            Expression<Func<T, bool>> filter = param => param.SourceLedger == sourceLedger && param.SourceType == sourceType;
            var sourceCodes = Service.Delete(filter);

            return new SourceCodeViewModel<T>
            {
                Data = sourceCodes,
                UserMessage = new UserMessage(sourceCodes, string.Format(CommonResx.DeleteSuccessMessage, SourceCodeResx.SourceCode, sourceCodes.SourceLedger + "-" + sourceCodes.SourceType))
            };
        }
        #endregion
    }
}