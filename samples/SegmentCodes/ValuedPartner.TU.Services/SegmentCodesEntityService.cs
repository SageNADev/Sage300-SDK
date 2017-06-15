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

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Base;
using System.Collections.Generic;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Interfaces.Services;
using ValuedPartner.TU.Models;

#endregion

namespace ValuedPartner.TU.Services
{
    /// <summary>
    /// Class for SegmentCodesEntityService
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class SegmentCodesEntityService<T> : FlatService<T, ISegmentCodesEntity<T>>, ISegmentCodesService<T>
        where T : SegmentCodes, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor for SegmentCodes
        /// </summary>
        /// <param name="context">Request Context</param>
        public SegmentCodesEntityService(Context context)
            : base(context)
        {
        }

        #endregion

        #region IEntityService Methods

        /// <summary>
        /// Is Account Used
        /// </summary>
        /// <param name="model"></param>
        /// <param name="canDeleteSegmentCodes"></param>
        /// <returns></returns>
        public virtual bool AreSegmentCodesDeletable(IEnumerable<T> model, ref List<string> canDeleteSegmentCodes)
        {
            using (var repository = Resolve<ISegmentCodesEntity<T>>())
            {
                return !repository.AreSegmentCodesDeletable(model, ref canDeleteSegmentCodes);
            }
        }

        /// <summary>
        /// Checks for the segment code duplication
        /// </summary>
        /// <param name="segmentNumber">The segment number.</param>
        /// <param name="segmentCode">The segment code.</param>
        /// <returns>Returns True if duplicate segmentCode not found.</returns>
        public virtual bool Exists(string segmentNumber, string segmentCode)
        {
            using (var repository = Resolve<ISegmentCodesEntity<T>>())
            {
                return !repository.Exists(segmentNumber, segmentCode);
            }
        }

        /// <summary>
        /// Save the SegmentCode
        /// </summary>
        /// <param name="model">ItemSegments</param>
        /// <returns>ItemSegments</returns>
        public EnumerableResponse<T> Save(EnumerableResponse<T> model)
        {
            using (var repository = Resolve<ISegmentCodesEntity<T>>())
            {
                model.Items = repository.Save(model.Items);

                return model;
            }
        }

        #endregion
    }
}