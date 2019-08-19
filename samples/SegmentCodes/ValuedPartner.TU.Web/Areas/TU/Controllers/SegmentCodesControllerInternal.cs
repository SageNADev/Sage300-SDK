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
using Microsoft.Practices.Unity;
using System.Linq.Expressions;
using System.Collections.Generic;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
using Sage.CA.SBS.ERP.Sage300.IC.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.IC.Models;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    /// SegmentCodes Internal Controller
    /// </summary>
    public class SegmentCodesControllerInternal : ImportExportControllerInternal<ISegmentCodesRepository>
    {
        #region Private variables
         
        /// <summary>
        /// Variable for storing context.
        /// </summary>
        private readonly Context _context;

        private ISegmentCodesRepository _repository
        {
            get {
                return _context.Container.Resolve<ISegmentCodesRepository>(new ParameterOverride("context", _context));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// New instance of <see cref="SegmentCodesControllerInternal"/>
        /// </summary>
        /// <param name="context">Context</param>
        public SegmentCodesControllerInternal(Context context)
            : base(context)
        {
            _context = context;
        }

        #endregion

		#region Internal methods

        /// <summary>
        /// Create a SegmentCodes
        /// </summary>
        /// <returns>view model for  SegmentCodes</returns>
        internal SegmentCodesViewModel Create()
        {
	        var viewModel = GetViewModel(new SegmentCodes(), null);

            var itemSegmentService =
              Context.Container.Resolve<IItemSegmentService<ItemSegment>>(new ParameterOverride("context", Context));

            var segmentname = itemSegmentService.Get();

            if (segmentname.Items != null && segmentname.Items.Any())
            {
                foreach (var items in segmentname.Items)
                {
                    viewModel.Segments.Add(new SegmentName
                    {
                        Text = items.Description,
                        Value = items.SegmentNumber,
                        SegmentLength = items.Length,
                        SegmentNumber = items.SegmentNumber
                    });
                }
            }

            viewModel.UserAccess = GetAccessRights();

            return viewModel;
        }

        /// <summary>
        /// commit the revision list to database
        /// </summary>
        internal SegmentCodesViewModel Post()
        {
            _repository.Post();

			var userMessage = new UserMessage(null, CommonResx.SaveSuccessMessage);
            return GetViewModel(null, userMessage);
        }
		
        #endregion

		#region Private methods

        /// <summary>
        /// Generic routine to return a view model for SegmentCodes
        /// </summary>
        /// <param name="model">Model for SegmentCodes</param>
        /// <param name="userMessage">User Message for SegmentCodes</param>
        /// <returns>View Model for SegmentCodes</returns>
        private SegmentCodesViewModel GetViewModel(SegmentCodes model, UserMessage userMessage)
        {
            return new SegmentCodesViewModel
            {
                Segments = new List<SegmentName>(),
                Data = model,
                UserMessage = userMessage
            };
        }

        #endregion

	}
}