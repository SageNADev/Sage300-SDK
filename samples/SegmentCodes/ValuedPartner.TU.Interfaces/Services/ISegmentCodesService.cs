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

using ValuedPartner.TU.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using System.Collections.Generic;

#endregion

namespace ValuedPartner.TU.Interfaces.Services
{
    /// <summary>
    /// Interface ISegmentCodesService
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public interface ISegmentCodesService<T> : IEntityService<T>, ISecurityService 
        where T : SegmentCodes, new()
    {
        /// <summary>
        /// Check whether segmentcode can be deletable or not.
        /// </summary>
        /// <param name="segmentCodes">An IEnumerable type of SegmentCode</param>
        /// <param name="deletableSegmentCodes">List of strings</param>
        /// <returns>Returns True if all of the records passed are agreed to be removed.</returns>
        bool AreSegmentCodesDeletable(IEnumerable<T> segmentCodes, ref List<string> deletableSegmentCodes);

        /// <summary>
        /// Check for segment code duplication
        /// </summary>
        /// <param name="segmentNumber">The segment number.</param>
        /// <param name="segmentCode">The segment code.</param>
        /// <returns>Returns True if duplicate segmentCode not found.</returns>
        bool Exists(string segmentNumber, string segmentCode);

        /// <summary>
        /// Save the SegmentCode
        /// </summary>
        /// <param name="model">ItemSegments</param>
        /// <returns>ItemSegments</returns>
        EnumerableResponse<T> Save(EnumerableResponse<T> model);
    }
}