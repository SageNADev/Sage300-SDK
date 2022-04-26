// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> BusinessField class to hold properties for a field </summary>
    public class BusinessField
    {
        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public BusinessField()
        {
            // Set defaults
            Id = 0;
            ServerFieldName = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Type = BusinessDataType.Double;
            Size = 0;
            IsReadOnly = false;
            IsCalculated = false;
            IsRequired = false;
            IsKey = false;
            IsUpperCase = false;
            IsAlphaNumeric = false;
            IsNumeric = false;
            IsDynamicEnablement = false;
            IsTimeOnly = false;

#if ENABLE_TK_244885
            IsCommon = false;
            //AlternateName = string.Empty;
#endif
        }
#endregion

#region Public Properties
        /// <summary> Id is the ordinal value for the field </summary>
        public int Id { get; set; }
        /// <summary> ServerFieldName is the field name on the server </summary>
        public string ServerFieldName { get; set; }
        /// <summary> Name is the field name </summary>
        public string Name { get; set; }
        /// <summary> Description of the field </summary>
        public string Description { get; set; }
        /// <summary> Type is the field type </summary>
        public BusinessDataType Type { get; set; }
        /// <summary> Size is number of characters allowed for the field</summary>
        public int Size { get; set; }
        /// <summary> IsReadOnly is true if field is read only otherwise false </summary>
        public bool IsReadOnly { get; set; }
        /// <summary> IsCalculated is true if calculated field otherwise false </summary>
        public bool IsCalculated { get; set; }
        /// <summary> IsRequired is true if field is required otherwise false </summary>
        public bool IsRequired { get; set; }
        /// <summary> IsKey is true if field is a key otherwise false </summary>
        public bool IsKey { get; set; }
        /// <summary> IsUpperCase is true if field requires uppecase otherwise false </summary>
        public bool IsUpperCase { get; set; }
        /// <summary> IsAlphaNumeric is true if field accepts alpha numeric characters otherwise false </summary>
        public bool IsAlphaNumeric { get; set; }
        /// <summary> IsNumeric is true if field accepts numeric characters otherwise false </summary>
        public bool IsNumeric { get; set; }
        /// <summary> IsDynamicEnablement is true if field attribute contains 'X' otherwise false </summary>
        public bool IsDynamicEnablement { get; set; }
        /// <summary> IsTimeOnly is true if date field only cares about the time portion otherwise false </summary>
        public bool IsTimeOnly { get; set; }
        /// <summary> Precison for numeric data type</summary>
        public int Precision { get; set; }
        /// <summary> Min value</summary>
        public object MinValue { get; set; }
        /// <summary> Max value</summary>
        public object MaxValue { get; set; }
        /// <summary> PreviousName is the field name prior to any changes in the properties grid</summary>
        public string PreviousName { get; set; }


#if ENABLE_TK_244885
        /// <summary>IsCommon is true if field has been marked as being common (or shared) </summary>
        public bool IsCommon { get; set; }
        ///// <summary> AlternateName is used when the user wishes to use a different name than the default.</summary>
        //public string AlternateName { get; set; }
#endif
        #endregion

    }

}
