
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

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models.Enums
{
    /// <summary>
    /// Enum for TaxBase
    /// </summary>
    public enum TaxBase
    {
        /// <summary>
        /// Gets or sets Sellingprice
        /// </summary>
        [EnumValue("Sellingprice", typeof(TaxAuthoritiesResx))]
        Sellingprice = 1,

        /// <summary>
        /// Gets or sets Standardcost
        /// </summary>
        [EnumValue("Standardcost", typeof(TaxAuthoritiesResx))]
        Standardcost = 2,

        /// <summary>
        /// Gets or sets Mostrecentcost
        /// </summary>
        [EnumValue("Mostrecentcost", typeof(TaxAuthoritiesResx))]
        Mostrecentcost = 3,

        /// <summary>
        /// Gets or sets Alternateamount1
        /// </summary>
        [EnumValue("Alternateamount1", typeof(TaxAuthoritiesResx))]
        Alternateamount1 = 4,

        /// <summary>
        /// Gets or sets Alternateamount2
        /// </summary>
        [EnumValue("Alternateamount2", typeof(TaxAuthoritiesResx))]
        Alternateamount2 = 5

    }
}