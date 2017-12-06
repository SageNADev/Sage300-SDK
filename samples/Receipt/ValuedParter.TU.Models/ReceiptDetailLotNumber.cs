// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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
using ValuedParter.TU.Resources;
using ValuedParter.TU.Resources.Forms;
using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace ValuedParter.TU.Models
{
     /// <summary>
     /// Partial class for Receipt Detail LotNumber
     /// </summary>
     public partial class ReceiptDetailLotNumber : ModelBase
     {
          /// <summary>
          /// Gets or sets SequenceNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "SequenceNumber", ResourceType = typeof(ReceiptDetailResx))] 
          public long SequenceNumber {get; set;}

          /// <summary>
          /// Gets or sets LineNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "LineNumber", ResourceType = typeof(ReceiptDetailResx))]  
          public int LineNumber {get; set;}

          /// <summary>
          /// Gets or sets LotNumber
          /// </summary>
          [Key]
          [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [StringLength(40, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "LotNumber", ResourceType = typeof(ReceiptDetailResx))]           
          public string LotNumber {get; set;}

          /// <summary>
          /// Gets or sets ExpiryDate
          /// </summary>
          [ValidateDateFormat(ErrorMessage="DateFormat", ErrorMessageResourceType = typeof(AnnotationsResx))]
          [Display(Name = "ExpiryDate", ResourceType = typeof(ReceiptDetailResx))]            
          public DateTime ExpiryDate {get; set;}

          /// <summary>
          /// Gets or sets TransactionQuantity
          /// </summary>
          [Display(Name = "TransactionQuantity", ResourceType = typeof(ReceiptDetailResx))]            
          public decimal TransactionQuantity {get; set;}

          /// <summary>
          /// Gets or sets LotQuantityInStockingUOM
          /// </summary>
          [Display(Name = "LotQuantityInStockingUOM", ResourceType = typeof(ReceiptDetailResx))]             
          public decimal LotQuantityInStockingUOM {get; set;}

          /// <summary>
          /// Gets or sets LotQuantityReturned
          /// </summary>
          [Display(Name = "LotQuantityReturned", ResourceType = typeof(ReceiptDetailResx))]             
          public decimal LotQuantityReturned {get; set;}

          /// <summary>
          /// Gets or sets LotQtyReturnedInStockingUOM
          /// </summary>
          [Display(Name = "LotQtyReturnedInStockingUOM", ResourceType = typeof(ReceiptDetailResx))]             
          public decimal LotQtyReturnedInStockingUOM {get; set;}
     }
}
