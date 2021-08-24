// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// Enum for Data Types
    /// </summary>
    public enum BusinessDataType
    {
        /// <summary> double </summary>
        [EnumValue("double")] 
        Double = 0,

        /// <summary> long </summary>
        [EnumValue("long")]
        Long = 1,

        /// <summary> string </summary>
        [EnumValue("string")]
        String = 2,

        /// <summary> DateTime </summary>
        [EnumValue("DateTime")]
        DateTime = 3,

        /// <summary> int </summary>
        [EnumValue("int")]
        Integer = 4,

        /// <summary> decimal </summary>
        [EnumValue("decimal")]
        Decimal = 5,

        /// <summary> bool </summary>
        [EnumValue("bool")]
        Boolean = 6,

        /// <summary> TimeSpan </summary>
        [EnumValue("TimeSpan")]
        TimeSpan = 7,

        /// <summary> byte[] </summary>
        [EnumValue("byte[]")]
        Byte = 8,

        /// <summary> enumeration </summary>
        [EnumValue("enumeration")]
        Enumeration = 9,

        /// <summary> short </summary>
        [EnumValue("short")]
        Short = 10,
    }
}
