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
using System.Collections;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using System.Collections.Generic;
#endregion

namespace ValuedPartner.Web.Areas.TU.Models
{
    /// <summary>
    /// Class for SegmentCodesViewModel
    /// </summary>
    /// <typeparam name="T">Where T is type of <see cref="SegmentCodes"/></typeparam>
    public class SegmentCodesViewModel<T> : ViewModelBase<T> 
        where T : SegmentCodes, new()
    {

        public SegmentCodesViewModel()
        {
            Segments = new List<SegmentName>();
            SegmentCodes = new EnumerableResponse<T> { Items = new List<T>() };
        }
        /// <summary>
        /// SegmentNumber list
        /// </summary>
        public List<SegmentName> Segments { get; set; }
        
        /// <summary>
        /// Gets or sets the Segment Codes used
        /// </summary>

        public EnumerableResponse<T> SegmentCodes { get; set; }

        /// <summary>
        /// Gets or sets the Segment name length
        /// </summary>
        public int SegmentNameLength { get; set; }

        /// <summary>
        /// Gets or sets the Segment number
        /// </summary>
        public string SegmentNumber { get; set; }

        /// <summary>
        /// Property For IsValidSegmentCode
        /// </summary>
        public bool IsValidSegmentCode { get; set; }

        /// <summary>
        /// Property For IsValidAccount
        /// </summary>
        public bool IsSegmentCodeUsed { get; set; }

        /// <summary>
        /// Duplicate error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Duplicate error message
        /// </summary>
        public List<string> DeletedSegmentCodes { get; set; }
    }

    /// <summary>
    /// Segment List
    /// </summary>
    public class SegmentName
    {
        /// <summary>
        /// Gets or sets Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets Segment Number
        /// </summary>
        public string SegmentNumber { get; set; }

        /// <summary>
        /// Gets or sets Segment Length
        /// </summary>
        public int SegmentLength { get; set; }
    }
}