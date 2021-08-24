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

#region Namespaces
using System.Collections.Generic;
using System;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Class to store enumerations </summary>
    public class EnumHelper
    {
        #region Public Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EnumHelper()
        {
            Values = new Dictionary<string, object>();
        }
        #endregion

        #region Public Properties
        /// <summary> Field name for dictionary </summary>
        public string Name { get; set; }
        /// <summary> Enumeration values for field </summary>
        public Dictionary<string, Object> Values { get; set; }

#if ENABLE_TK_244885
        /// <summary> If field is marked as common (or shared) this is set to true, otherwise false </summary>
        public bool IsCommon { get; set; }
        ///// <summary> An optional alternate name for the field </summary>
        //public string AlternateName { get; set; }
#endif
        #endregion
    }
}