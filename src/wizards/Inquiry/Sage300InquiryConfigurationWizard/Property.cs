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

using System.Collections.Generic;

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary> Class to hold information for Properties </summary>
    public class Property
    {
        #region Constructor
        public Property()
        {
            Enums = new Dictionary<string, string>();
        }
        #endregion

        #region Public Properties
        /// <summary> Index of Property in Business View </summary>
        public int Index { get; set; }
        /// <summary> Name of Property </summary>
        public string Name { get; set; }
        /// <summary> Field Name of Property in Business View </summary>
        public string FieldName { get; set; }
        /// <summary> Name of Property Type </summary>
        public string PropertyTypeName { get; set; }
        /// <summary> Full Name of Property Type </summary>
        public string PropertyTypeFullName { get; set; }
        /// <summary> IsIncluded true or false </summary>
        public bool IsIncluded { get; set; }
        /// <summary> IsFilterable true or false </summary>
        public bool IsFilterable { get; set; }
        /// <summary> IsDrilldown true or false </summary>
        public bool IsDrilldown { get; set; }
        /// <summary> Area for Drilldown </summary>
        public string Area { get; set; }
        /// <summary> ControllerName for drilldown </summary>
        public string ControllerName { get; set; }
        /// <summary> ActionName for drilldown </summary>
        public string ActionName { get; set; }
        /// <summary> List of enum values for data type if an enum </summary>
        public Dictionary<string, string> Enums { get; set; }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
