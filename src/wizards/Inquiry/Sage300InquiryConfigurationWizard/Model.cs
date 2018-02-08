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
    /// <summary> Class to hold information for Model </summary>
    public class Model
    {
        #region Constructor
        public Model()
        {
            Properties = new Dictionary<string, Property>();
        }
        #endregion

        #region Public Properties

        /// <summary> Id of Inquiry </summary>
        public string Id { get; set; }

        /// <summary> Description </summary>
        public string Description { get; set; }

        /// <summary> Name of Model </summary>
        public string Name { get; set; }

        /// <summary> Full Name of Model </summary>
        public string FullName { get; set; }

        /// <summary> Entity Name of Model </summary>
        public string EntityName { get; set; }

        /// <summary> Manifest Module Name </summary>
        public string ManifestModuleName { get; set; }

        /// <summary> List of Properties </summary>
        public Dictionary<string, Property> Properties { get; set; }

        /// <summary> Display Name for Dropdown </summary>
        public string DisplayName { get; set; }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
