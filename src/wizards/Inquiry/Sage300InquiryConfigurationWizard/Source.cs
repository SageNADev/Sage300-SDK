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

#region Namespaces
using System.Collections.Generic;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.InquiryConfigurationWizard
{
    /// <summary> Source class </summary>
    [System.SerializableAttribute]
    public class Source
    {
        #region Public Constants
        public const string InquiryId = "InquiryId";
        public const string FolderName = "FolderName";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string User = "User";
        public const string Password = "Password";
        public const string Version = "Version";
        public const string Company = "Company";
        public const string ViewId = "ViewId";
        public const string ViewDescription = "ViewDescription";
        public const string SqlStatement = "SqlStatement";
        public const string WhereClause = "WhereClause";
        public const string OrderByClause = "OrderByClause";
        public const string IsBusinessView = "IsBusinessView";
        #endregion

        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public Source()
        {
            Properties = new Dictionary<string, string>();
            SourceColumns = new SortedList<string, SourceColumn>();
            Options = new Dictionary<string, bool>();
        }
        #endregion

        #region Public Properties
        /// <summary> Properties is the collection of business view or SQL properties </summary>
        public Dictionary<string, string> Properties { get; set; }
        /// <summary> SourceColumns is the collection of source columns </summary>
        public SortedList<string, SourceColumn> SourceColumns { get; set; }
        /// <summary> Options is the collection of options </summary>
        public Dictionary<string, bool> Options { get; set; }
        #endregion

    }

}
