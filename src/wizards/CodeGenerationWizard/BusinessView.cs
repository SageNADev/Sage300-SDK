// The MIT License (MIT) 
// Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Linq;
using System.Collections.Generic;
using ACCPAC.Advantage;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> BusinessView class to hold properties for a view </summary>
    [System.SerializableAttribute]
    public class BusinessView
    {
        #region Public Constants
        public static class Constants
        {
            public const string ViewId = "ViewId";
            public const string ModelName = "ModelName";
            public const string ModuleId = "ModuleId";
            public const string EntityName = "EntityName";
            public const string ReportIni = "ReportIni";
            public const string ReportKey = "ReportKey";
            public const string ProgramId = "ProgramId";
            public const string ResxName = "ResxName";
            public const string GenerateFinder = "GenerateFinder";
            public const string HasGrid = "HasGrid";
            public const string GenerateGridModel = "GenerateGridModel";
            public const string SeqenceRevisionList = "SequenceRevisionList";
            public const string GenerateDynamicEnablement = "GenerateDynamicEnablement";
            public const string GenerateClientFiles = "GenerateClientFiles";
            public const string GenerateIfAlreadyExists = "GenerateIfAlreadyExists";
            public const string GenerateEnumsInSingleFile = "GenerateEnumsInSingleFile";
            public const string WorkflowKindId = "WorkflowKindId";
            public const string ForGrid = "ForGrid";
#if ENABLE_TK_244885
            public const string CustomCommonResxName = "CustomCommonResxName";
#endif
        }
        #endregion

        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public BusinessView()
        {
            Properties = new Dictionary<string, string>();
            Fields = new List<BusinessField>();            
            Enums = new Dictionary<string, EnumHelper>();
            Options = new Dictionary<string, bool>();
            Compositions = new List<Composition>();
        }
#endregion

#region Public Properties
        /// <summary> Properties is the collection of business view properties </summary>
        public Dictionary<string, string> Properties { get; set; }

        /// <summary> Fields is the collection of business fields </summary>
        public List<BusinessField> Fields { get; set; }

        /// <summary> Enums is the collection of business enumerations </summary>
        public Dictionary<string, EnumHelper> Enums { get; set; }

        /// <summary> Keys is the collection of keys </summary>
        public List<string> Keys
        {
            get
            {
                return (from item in Fields
                        where item.IsKey == true
                        select item.Name).ToList();
            }
        }

        /// <summary> Text for tree display </summary>
        public string Text { get; set; }

        /// <summary> Options is the collection of business view options </summary>
        public Dictionary<string, bool> Options { get; set; }

        /// <summary> Compositions is the collection of entity compositions </summary>
        public List<Composition> Compositions { get; set; }

        /// <summary> View Protocol </summary>
        public ViewProtocol Protocol { get; set; }

        /// <summary> Is this part of a HeaderDetail Composition? </summary>
        public bool IsPartofHeaderDetailComposition { get; set; }

		/// <summary> Is this entity for grid </summary>
        public bool ForGrid { get; set; } = false;
        
#endregion

    }

}
