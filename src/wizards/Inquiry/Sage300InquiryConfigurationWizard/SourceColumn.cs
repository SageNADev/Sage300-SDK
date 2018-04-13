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
    /// <summary> SourceColumn class to hold properties for a column </summary>
    public class SourceColumn
    {
        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public SourceColumn()
        {
            // Set defaults and initialize
            Type = SourceDataType.None;
            Captions = new Dictionary<string, Caption>();
            Params = new Dictionary<string, Parameter>();
            Filters = new Dictionary<string, Filter>();
            Aggregation = new Dictionary<string, bool>();

            IsDisplayable = true;
            IsFilterable = true;
            ViewId = string.Empty;
            ViewColumnName = string.Empty;
        }
        #endregion

        #region Public Properties from Business View
        /// <summary> Id is the ordinal value for the field </summary>
        public int Id { get; set; }
        /// <summary> Name is the field name </summary>
        public string Name { get; set; }
        /// <summary> Description of the field - English </summary>
        public string DescriptionENG { get; set; }
        /// <summary> Type is the field type </summary>
        public SourceDataType Type { get; set; }
        #endregion

        #region Public Properties - Additional
        /// <summary> Is column included in config </summary>
        public bool IsIncluded { get; set; }
        /// <summary> Is column displayed in inquiry grid </summary>
        public bool IsDisplayable { get; set; }
        /// <summary> Display order of column in grid </summary>
        public int DisplayOrder { get; set; }
        /// <summary> List of captions for column grid by language </summary>
        public Dictionary<string, Caption> Captions { get; set; }
        /// <summary> Does column allow drilldown </summary>
        public bool IsDrilldown { get; set; }
        /// <summary> AreaName (module) for Drilldown </summary>
        public string AreaName { get; set; }
        /// <summary> ControllerName for drilldown </summary>
        public string ControllerName { get; set; }
        /// <summary> ActionName (method) for drilldown </summary>
        public string ActionName { get; set; }
        /// <summary> List of parameters, if any, for drilldown </summary>
        public Dictionary<string, Parameter> Params { get; set; }
        /// <summary> Does column allow filtering in grid </summary>
        public bool IsFilterable { get; set; }
        /// <summary> Is column in a view </summary>
        public bool IsColumnInView { get; set; }
        /// <summary> View Id for column otherwise empty </summary>
        public string ViewId { get; set; }
        /// <summary> Column name in view otherwise empty </summary>
        public string ViewColumnName { get; set; }
        /// <summary> List of filter values for column </summary>
        public Dictionary<string, Filter> Filters { get; set; }
        /// <summary> Is column groupable </summary>
        public bool IsGroupBy { get; set; }
        /// <summary> List of column aggregation types </summary>
        public Dictionary<string, bool> Aggregation { get; set; }
        #endregion

        /// <summary> Description of the field - French </summary>
        public string DescriptionFRA { get; set; }
        /// <summary> Description of the field - Spanish </summary>
        public string DescriptionESN { get; set; }
        /// <summary> Description of the field - Chinese (simplified) </summary>
        public string DescriptionCHN { get; set; }
        /// <summary> Description of the field - Chinese (traditional) </summary>
        public string DescriptionCHT { get; set; }
    }
}
