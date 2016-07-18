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

#endregion

namespace ValuedPartner.TU.Models
{
    /// <summary>
    /// Contains list of SegmentCodes Constants
    /// </summary>
    public partial class SegmentCodes
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "IC0620";


        #region Properties

        /// <summary>
        /// Contains list of SegmentCodes Field Constants
        /// </summary>
        public class Fields
        {
            /// <summary>
            /// Property for SegmentNumber
            /// </summary>
            public const string SegmentNumber = "SEGMENT";

            /// <summary>
            /// Property for SegmentCode
            /// </summary>
            public const string SegmentCode = "SEGVAL";

            /// <summary>
            /// Property for Description
            /// </summary>
            public const string Description = "DESC";

        }

        #endregion
        #region Properties

        /// <summary>
        /// Contains list of SegmentCodes Index Constants
        /// </summary>
        public class Index
        {
            /// <summary>
            /// Property Indexer for SegmentNumber
            /// </summary>
            public const int SegmentNumber = 1;

            /// <summary>
            /// Property Indexer for SegmentCode
            /// </summary>
            public const int SegmentCode = 2;

            /// <summary>
            /// Property Indexer for Description
            /// </summary>
            public const int Description = 3;


        }

        /// <summary>
        /// To get integer value for unique id for grid
        /// </summary>
        public long SerialNumber { get; set; }

        #endregion

    }
}