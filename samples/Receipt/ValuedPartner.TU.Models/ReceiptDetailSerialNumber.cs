// The MIT License (MIT) 
// Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using System.ComponentModel.DataAnnotations;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models
{
     /// <summary>
     /// Partial class for Receipt Detail SerialNumber
     /// </summary>
     public partial class ReceiptDetailSerialNumber : ModelBase
     {
          /// <summary>
          /// Gets or sets SequenceNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "SequenceNumber", ResourceType = typeof(ReceiptHeaderResx))]            
          public long SequenceNumber {get; set;}

          /// <summary>
          /// Gets or sets LineNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "LineNumber", ResourceType = typeof(ReceiptHeaderResx))]            
          public int LineNumber {get; set;}

          /// <summary>
          /// Gets or sets SerialNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [StringLength(40, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "SerialNumber", ResourceType = typeof(ReceiptHeaderResx))]                
          public string SerialNumber {get; set;}

          /// <summary>
          /// Gets or sets SerialReturned
          /// </summary>
          [Display(Name = "SerialReturned", ResourceType = typeof(ReceiptHeaderResx))]           
          public bool SerialReturned {get; set;}

          /// <summary>
          /// Gets or sets TransactionQuantity
          /// </summary>
          [Display(Name = "TransactionQuantity", ResourceType = typeof(ReceiptHeaderResx))]             
          public long TransactionQuantity {get; set;}

          /// <summary>
          /// Gets or sets SerialQuantityReturned
          /// </summary>
          [Display(Name = "SerialQuantityReturned", ResourceType = typeof(ReceiptHeaderResx))]             
          public long SerialQuantityReturned {get; set;}
     }
}
