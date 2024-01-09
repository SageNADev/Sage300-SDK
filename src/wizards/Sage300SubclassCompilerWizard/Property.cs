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

namespace Sage.CA.SBS.ERP.Sage300.SubclassCompilerWizard
{
    /// <summary> Class to hold information for property </summary>
    public class Property
    {
        #region Public Properties

        /// <summary> Field Name </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary> Id </summary>
        public int Id { get; set; } = 0;

        /// <summary> Field Type </summary>
        public FieldType FieldType { get; set; } = FieldType.Char;

        /// <summary> Property Name </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> Data Type </summary>
        public DataType DataType { get; set; } = DataType.String;

        /// <summary> Mask </summary>
        public string Mask { get; set; } = string.Empty;

        /// <summary> Size </summary>
        public int Size { get; set; } = 0;

        /// <summary> Precision </summary>
        public int Precision { get; set; } = 0;

        #endregion
    }
}
