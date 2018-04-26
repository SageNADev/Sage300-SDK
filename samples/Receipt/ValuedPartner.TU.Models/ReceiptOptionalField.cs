// The MIT License (MIT) 
// Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.
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

#region Namespace

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using ValuedPartner.TU.Resources.Forms;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace ValuedPartner.TU.Models
{
     /// <summary>
     /// Partial class for Receipt OptionalField
     /// </summary>
     public partial class ReceiptOptionalField : ModelBase
     {
          /// <summary>
          /// Gets or sets SequenceNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "SequenceNumber", ResourceType = typeof(ReceiptDetailResx))] 
          public long SequenceNumber {get; set;}

          /// <summary>
          /// Gets or sets OptionalField
          /// </summary>
          [Key]
          [StringLength(12, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "OptionalField", ResourceType = typeof(ReceiptDetailResx))]   
          public string OptionalField {get; set;}

          /// <summary>
          /// Gets or sets Value
          /// </summary>
          [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "Value", ResourceType = typeof(ReceiptDetailResx))]   
          public string Value {get; set;}

          /// <summary>
          /// Gets or sets Type
          /// </summary>
          [Display(Name = "Type", ResourceType = typeof(ReceiptDetailResx))]
          public Enums.Type Type { get; set; }

          /// <summary>
          /// Gets or sets Length
          /// </summary>
          [Display(Name = "Length", ResourceType = typeof(ReceiptDetailResx))] 
          public int Length {get; set;}

          /// <summary>
          /// Gets or sets Decimals
          /// </summary>
          [Display(Name = "Decimals", ResourceType = typeof(ReceiptDetailResx))]  
          public int Decimals {get; set;}

          /// <summary>
          /// Gets or sets AllowBlank
          /// </summary>
          [Display(Name = "AllowBlank", ResourceType = typeof(ReceiptDetailResx))]
          public Enums.AllowBlank AllowBlank { get; set; }

          /// <summary>
          /// Gets or sets Validate
          /// </summary>
          [Display(Name = "Validate", ResourceType = typeof(ReceiptDetailResx))]
          public Enums.Validate Validate { get; set; }

          /// <summary>
          /// Gets or sets ValueSet
          /// </summary>
          [Display(Name = "ValueSet", ResourceType = typeof(ReceiptDetailResx))]
          public Enums.ValueSet ValueSet { get; set; }

          /// <summary>
          /// Gets or sets TypedValueFieldIndex
          /// </summary>
          [Display(Name = "TypedValueFieldIndex", ResourceType = typeof(ReceiptDetailResx))]
          public long TypedValueFieldIndex {get; set;}

          /// <summary>
          /// Gets or sets TextValue
          /// </summary>
          [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "TextValue", ResourceType = typeof(ReceiptDetailResx))] 
          public string TextValue {get; set;}

          /// <summary>
          /// Gets or sets AmountValue
          /// </summary>
          [Display(Name = "AmountValue", ResourceType = typeof(ReceiptDetailResx))]  
          public decimal AmountValue {get; set;}

          /// <summary>
          /// Gets or sets NumberValue
          /// </summary>
          [Display(Name = "NumberValue", ResourceType = typeof(ReceiptDetailResx))]           
          public decimal NumberValue {get; set;}

          /// <summary>
          /// Gets or sets IntegerValue
          /// </summary>
          [Display(Name = "IntegerValue", ResourceType = typeof(ReceiptDetailResx))]                
          public long IntegerValue {get; set;}

          /// <summary>
          /// Gets or sets YesNoValue
          /// </summary>
          [Display(Name = "YesNoValue", ResourceType = typeof(ReceiptDetailResx))]
          public Enums.YesNoValue YesNoValue { get; set; }

          /// <summary>
          /// Gets or sets DateValue
          /// </summary>
          [ValidateDateFormat(ErrorMessage="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "DateValue", ResourceType = typeof(ReceiptDetailResx))]  
          public DateTime? DateValue {get; set;}

          /// <summary>
          /// Gets or sets TimeValue
          /// </summary>
          [Display(Name = "TimeValue", ResourceType = typeof(ReceiptDetailResx))]            
          public DateTime? TimeValue { get; set; }

          /// <summary>
          /// Gets or sets OptionalFieldDescription
          /// </summary>
          [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "OptionalFieldDescription", ResourceType = typeof(ReceiptDetailResx))]            
          public string OptionalFieldDescription {get; set;}

          /// <summary>
          /// Gets or sets ValueDescription
          /// </summary>
          [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "ValueDescription", ResourceType = typeof(ReceiptDetailResx))]            
          public string ValueDescription {get; set;}

          /// <summary>
          /// Gets or sets Line Number.
          /// </summary>
          [Display(Name = "LineNumber", ResourceType = typeof(ReceiptDetailResx))]
          // [IgnoreExportImport]
          public int LineNumber { get; set; }
     }
}
