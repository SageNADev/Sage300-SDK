// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.Finder;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.Web.Areas.TU.Controllers.Finder
{
    /// <summary>
    /// Finder class for SegmentCodes
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class FindSegmentCodesControllerInternal<T> : BaseFindControllerInternal<T, ISegmentCodesService<T>>, IFinder
        where T : SegmentCodes, new()
    {
        #region Private variables

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for SegmentCodes
        /// </summary>
        /// <param name="context">Context</param>
        public FindSegmentCodesControllerInternal(Context context)
            : base(context)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get first or default SegmentCodes
        /// </summary>
        /// <param name="id">Id for SegmentCodes</param>
        /// <returns>Get first or default SegmentCodes</returns>
        public virtual ModelBase Get(string id)
        {
            Expression<Func<T, bool>> filter = param => param.SegmentNumber.ToString() == id;
            Service.IsReadOnly = true;
            var model = Service.FirstOrDefault(filter);
            Service.IsReadOnly = false;
			return model;
        }

        /// <summary>
        /// Get the default columns
        /// </summary>
        /// <returns>Default columns</returns>
        public override List<string> GetDefaultColumns()
        {
            // TODO: All columns have been added and must be reduced to only default columns
            // TODO: Delete TODO statements when complete
            return new List<string> 
            {
                "SegmentNumberString",
                "SegmentCode",
                "Description"
             };
       }

        /// <summary>
        /// Get all columns
        /// </summary>
        /// <returns>All columns</returns>
        public override IEnumerable<ModelBase> GetAllColumns()
        {

            var columns = new List<ModelBase>
            {
                new GridField
                {
                    field = "SegmentNumberString",
                    title = SegmentCodesResx.SegmentNumber,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    PresentationList = EnumUtility.GetItemsList<SegmentNumber>()
                },
                new GridField
                {
                    field = "SegmentCode",
                    title = SegmentCodesResx.SegmentCode,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "24"},
                            {"class", FinderConstant.CssClassTxtUpper},
                            {FinderConstant.CustomAtrributeFormatTextBox, FinderConstant.AlphaNumeric}
                        }
                },
                new GridField
                {
                    field = "Description",
                    title = SegmentCodesResx.Description,
                    attributes = FinderConstant.CssClassGridColumn10,
                    headerAttributes = FinderConstant.CssClassGridColumn10,
                    dataType = FinderConstant.DataTypeString,
                    customAttributes = 
                        new Dictionary<string, string>
                        {
                            {FinderConstant.CustomAttributeMaximumLength, "60"}
                        }
                }
            };

            return columns.AsEnumerable();
        }
        #endregion

    }
}