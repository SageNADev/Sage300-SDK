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

#region Namespace

using System;
using System.ComponentModel.DataAnnotations;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Attributes;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;

using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models
{
    /// <summary>
    /// Partial class for SegmentCodes
    /// </summary>
    public partial class SegmentCodes : ModelBase
    {
        /// <summary>
        /// Gets or sets SegmentNumber
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SegmentNumber", ResourceType = typeof (SegmentCodesResx))]
        //public SegmentNumber SegmentNumber { get; set; }
        public string SegmentNumber { get; set; }
        /// <summary>
        /// Gets or sets SegmentCode
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [StringLength(24, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SegmentCode", ResourceType = typeof (SegmentCodesResx))]
        [GridInfo(1, typeof(SegmentCodesResx), "SegmentCode", editorType: GridEditorEnum.Text, templateSource: "'#= sg.utls.toUpperCase({0}) #'",
            Style = "gird_culm_10", EditorHtmlClass = "txt-upper grid_inpt", IsAlphaNumericEditor = true)]
        public string SegmentCode { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "Description", ResourceType = typeof (SegmentCodesResx))]
        [GridInfo(2, typeof(SegmentCodesResx), "Description", editorType: GridEditorEnum.Text, Style = "", EditorHtmlClass = "grid_inpt left")]
        public string Description { get; set; }

    }
}