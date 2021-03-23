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

using System.Collections.Generic;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    class GridFinder
    {
    }
    /// <summary>
    /// Column definition
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Column header in the grid
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Corresponding ACCPAC view fieldname. If blank, it is a calculated field
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Field Type
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Determine whether the column is read only or not
        /// </summary>
        public bool IsEditable { get; set; } = true;

        /// <summary>
        /// Set it to true if the column needs to launch a finder
        /// </summary>
        public bool? HasFinder { get; set; } = null;

        /// <summary>
        /// Grid Column finder definition
        /// </summary>
        public FinderDefinition Finder { get; set; }

        /// <summary>
        /// Is optional field
        /// </summary>
        public bool? IsOptionalField { get; set; } = null;

        /// <summary>
        /// Is line number field
        /// </summary>
        public bool? IsLineNumber { get; set; } = null;

        /// <summary>
        /// Add cutsome functions
        /// </summary>
        public List<CustomFunction> CustomFunctions { get; set; }

    }
    /// <summary>
    /// Column level custom function defintions
    /// </summary>
    public class CustomFunction
    {
        public string OptionalField { get; set; }
        public string columnBeforeFinder { get; set; }
        public string columnBeforeDisplay { get; set; }
        public string columnFinderFocus { get; set; }

    }

    /// <summary>
    /// Grid definition
    /// </summary>
    public class GridDefinition
    {
        /// <summary>
        /// View ID associated with the grid
        /// </summary>
        public string ViewID { get; set; }

        /// <summary>
        /// ViewOrder, default 0
        /// </summary>
        public int ViewOrder { get; set; } = 0;

        /// <summary>
        /// Grid type: 0-Standard Grid, 1-OptionalField Grid, 2-Other type Grid 
        /// </summary>
        public int GridType { get; set; }

        /// <summary>
        /// Read only grid
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Grid page size
        /// </summary>
        public int PageSize { get; set; }

        public List<GridCustomFunction> CustomFunctions { get; set; }
        /// <summary>
        /// Define all the columns in the grid
        /// </summary>
        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }

    /// <summary>
    /// Grid level custom function defintions
    /// </summary>
    public class GridCustomFunction
    {
        public string gridAfterLoadData { get; set; }
        public string gridAfterSetActiveRecord { get; set; }
        public string gridChanged { get; set; }
        public string gridUpdated { get; set; }
        public string gridBeforeDelete { get; set; }
        public string gridAfterDelete { get; set; }
        public string gridBeforeCreate { get; set; }
        public string gridAfterCreate { get; set; }
        public string gridAfterInsert { get; set; }
    }

    /// <summary>
    /// Finder definition
    /// </summary>
    public class FinderDefinition
    {
        /// <summary>
        /// View ID
        /// </summary>
        public string ViewID { get; set; }
        /// <summary>
        /// ViewOrder, default 0
        /// </summary>
        public int ViewOrder { get; set; } = 0;
        /// <summary>
        /// Filter for the finder. A complete browse filter is created in conjunction of the column filter 
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// Display filed names in UI grid
        /// </summary>
        public string[] DisplayFieldNames { get; set; }
        /// <summary>
        /// Finder select return fileds name in UI grid
        /// </summary>
        public string[] ReturnFieldNames { get; set; }
        /// <summary>
        /// Grid fields name for finder keys, used set filter and initKeyValues
        /// </summary>
        public string[] InitKeyFieldNames { get; set; }
    }

}