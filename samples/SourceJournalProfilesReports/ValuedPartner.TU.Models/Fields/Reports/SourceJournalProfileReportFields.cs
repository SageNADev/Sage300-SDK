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

namespace ValuedPartner.TU.Models.Reports
{
    /// <summary>
    /// Contains list of SourceJournalProfileReport Constants
    /// </summary>
    public partial class SourceJournalProfileReport
    {
        /// <summary>
        /// View Name
        /// </summary>
        public const string ViewName = "7ddf9cc7-1527-4451-bd47-a810f959ff28";

        /// <summary>
        /// Entity Name
        /// </summary>
        public const string EntityName = "GL4111";
        

        #region Properties

        /// <summary>
        /// Contains list of SourceJournalProfileReport Field Constants
        /// </summary>
        public class Fields
        {
            /// <summary>
            /// Property for Frjrnl
            /// </summary>
            public const string Frjrnl = "FRJRNL";

            /// <summary>
            /// Property for Tojrnl
            /// </summary>
            public const string Tojrnl = "TOJRNL";

        }

        #endregion
        #region Properties

        /// <summary>
        /// Contains list of SourceJournalProfileReport Index Constants
        /// </summary>
        public class Index
        {
            /// <summary>
            /// Property Indexer for Frjrnl
            /// </summary>
            public const int Frjrnl = 2;

            /// <summary>
            /// Property Indexer for Tojrnl
            /// </summary>
            public const int Tojrnl = 3;


        }

        #endregion

    }
}