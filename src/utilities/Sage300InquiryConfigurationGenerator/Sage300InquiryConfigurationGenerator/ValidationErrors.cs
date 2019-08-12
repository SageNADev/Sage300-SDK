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

#region Imports
using System.Collections.Generic;
using System.Text;
#endregion

namespace Sage300InquiryConfigurationGenerator
{
    /// <summary>
    /// Simple class to manage validation errors
    /// </summary>
    public class ValidationErrors
    {
        #region Private Variables
        private List<string> _errors;
        #endregion

        #region Public Constructor(s)
        public ValidationErrors()
        {
            _errors = new List<string>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a new error message
        /// </summary>
        /// <param name="text"></param>
        public void Add(string text)
        {
            _errors.Add(text);
        }

        /// <summary>
        /// Clear out all error messages
        /// </summary>
        public void Clear()
        {
            _errors.Clear();
        }

        /// <summary>
        /// Get all error messages as a List
        /// </summary>
        /// <returns></returns>
        public List<string> GetAll()
        {
            return _errors;
        }

        /// <summary>
        /// Get all error messages as a single, multi-line string
        /// Note: They will be in reverse tab order
        /// </summary>
        /// <returns>String representation of all error messages</returns>
        public string GetAllAsString(bool reverseOrder = true)
        {
            var sb = new StringBuilder();

            if (reverseOrder)
            {
                _errors.Reverse();
            }

            foreach (var s in _errors)
            {
                sb.AppendLine(s);
            }
            return sb.ToString();
        }

        #endregion
    }
}
