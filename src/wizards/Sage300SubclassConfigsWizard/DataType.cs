// The MIT License (MIT) 
// Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved.
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

namespace Sage.CA.SBS.ERP.Sage300.SubclassConfigsWizard
{
    /// <summary> Enum for Data Types </summary>
    public enum DataType
    {
        /// <summary> string </summary>
        String = 1,

        /// <summary> Binary </summary>
        Byte = 2,

        /// <summary> DateTime </summary>
        DateTime = 3,

        /// <summary> TimeSpan </summary>
        TimeSpan = 4,

        /// <summary> 64-bit double-precision floating point </summary>
        Double = 5,

        /// <summary> 128-bit high precision decimal </summary>
        Decimal = 6,

        /// <summary> 16-bit signed integer </summary>
        Short = 7,

        /// <summary> 32-bit signed integer </summary>
        Int = 8,

        /// <summary> Boolean </summary>
        Bool = 9,

        /// <summary> 64-bit signed integer </summary>
        Int64 = 10
    }
}
