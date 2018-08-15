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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="searchIn"></param>
        /// <param name="searchFor"></param>
        /// <returns></returns>
        public static bool EnumExists(Dictionary<string, Dictionary<string, object>> _searchIn,
                                      Dictionary<string, Dictionary<string, object>> _searchFor)
        {
            bool exists = false;

            var enumName = _searchFor.Keys.ToArray()[0];
            if (_searchIn.ContainsKey(enumName))
            {
                // Key exists, now check all values
                exists = true;

                var arrValues = _searchFor[enumName];
                var arrValues2 = _searchIn[enumName];

                foreach (var val in arrValues2)
                {
                    if (!arrValues.Contains(val))
                    {
                        // Found a mismatch
                        // No need to search further.
                        exists = false;
                        break;
                    }
                }
            }

            return exists;
        }
    }
}
