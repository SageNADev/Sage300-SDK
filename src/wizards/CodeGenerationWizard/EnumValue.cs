// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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
using System;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Class to get and set Enum value </summary>
    public class EnumValue : Attribute
    {
        #region Public Properties
        /// <summary> Gets or sets the value </summary>
        public string Value { get; set; }
        #endregion

        #region Public Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValue"/> class.
        /// </summary>
        /// <param name="value">the enum value</param>
        public EnumValue(string value)
        {
            Value = value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Value for enumeration
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Value from EnumValue</returns>
        public static string GetValue(Enum value)
        {
            // Locals
            string output = null;
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(EnumValue), false) as EnumValue[];

                if (attrs != null && attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }
                return output;
            }
            return string.Empty;
        }
        #endregion
    }
}