// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models
{
    /// <summary>
    /// Partial class for SourceJournalProfile
    /// </summary>
    public partial class SourceJournalProfile : ModelBase
    {
        /// <summary>
        /// Gets or sets SourceJournalName
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [StringLength(60, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceJournalName", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceJournalName { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID01
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID01", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID01 { get; set; }

        /// <summary>
        /// Gets or sets SourceType01
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType01", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType01 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID02
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID02", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID02 { get; set; }

        /// <summary>
        /// Gets or sets SourceType02
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType02", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType02 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID03
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID03", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID03 { get; set; }

        /// <summary>
        /// Gets or sets SourceType03
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType03", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType03 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID04
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID04", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID04 { get; set; }

        /// <summary>
        /// Gets or sets SourceType04
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType04", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType04 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID05
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID05", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID05 { get; set; }

        /// <summary>
        /// Gets or sets SourceType05
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType05", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType05 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID06
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID06", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID06 { get; set; }

        /// <summary>
        /// Gets or sets SourceType06
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType06", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType06 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID07
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID07", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID07 { get; set; }

        /// <summary>
        /// Gets or sets SourceType07
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType07", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType07 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID08
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID08", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID08 { get; set; }

        /// <summary>
        /// Gets or sets SourceType08
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType08", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType08 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID09
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID09", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID09 { get; set; }

        /// <summary>
        /// Gets or sets SourceType09
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType09", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType09 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID10
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID10", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID10 { get; set; }

        /// <summary>
        /// Gets or sets SourceType10
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType10", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType10 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID11
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID11", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID11 { get; set; }

        /// <summary>
        /// Gets or sets SourceType11
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType11", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType11 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID12
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID12", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID12 { get; set; }

        /// <summary>
        /// Gets or sets SourceType12
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType12", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType12 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID13
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID13", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID13 { get; set; }

        /// <summary>
        /// Gets or sets SourceType13
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType13", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType13 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID14
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID14", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID14 { get; set; }

        /// <summary>
        /// Gets or sets SourceType14
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType14", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType14 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID15
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID15", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID15 { get; set; }

        /// <summary>
        /// Gets or sets SourceType15
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType15", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType15 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID16
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID16", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID16 { get; set; }

        /// <summary>
        /// Gets or sets SourceType16
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType16", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType16 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID17
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID17", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID17 { get; set; }

        /// <summary>
        /// Gets or sets SourceType17
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType17", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType17 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID18
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID18", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID18 { get; set; }

        /// <summary>
        /// Gets or sets SourceType18
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType18", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType18 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID19
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID19", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID19 { get; set; }

        /// <summary>
        /// Gets or sets SourceType19
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType19", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType19 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID20
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID20", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID20 { get; set; }

        /// <summary>
        /// Gets or sets SourceType20
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType20", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType20 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID21
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID21", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID21 { get; set; }

        /// <summary>
        /// Gets or sets SourceType21
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType21", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType21 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID22
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID22", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID22 { get; set; }

        /// <summary>
        /// Gets or sets SourceType22
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType22", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType22 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID23
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID23", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID23 { get; set; }

        /// <summary>
        /// Gets or sets SourceType23
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType23", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType23 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID24
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID24", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID24 { get; set; }

        /// <summary>
        /// Gets or sets SourceType24
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType24", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType24 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID25
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID25", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID25 { get; set; }

        /// <summary>
        /// Gets or sets SourceType25
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType25", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType25 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID26
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID26", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID26 { get; set; }

        /// <summary>
        /// Gets or sets SourceType26
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType26", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType26 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID27
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID27", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID27 { get; set; }

        /// <summary>
        /// Gets or sets SourceType27
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType27", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType27 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID28
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID28", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID28 { get; set; }

        /// <summary>
        /// Gets or sets SourceType28
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType28", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType28 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID29
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID29", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID29 { get; set; }

        /// <summary>
        /// Gets or sets SourceType29
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType29", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType29 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID30
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID30", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID30 { get; set; }

        /// <summary>
        /// Gets or sets SourceType30
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType30", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType30 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID31
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID31", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID31 { get; set; }

        /// <summary>
        /// Gets or sets SourceType31
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType31", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType31 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID32
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID32", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID32 { get; set; }

        /// <summary>
        /// Gets or sets SourceType32
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType32", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType32 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID33
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID33", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID33 { get; set; }

        /// <summary>
        /// Gets or sets SourceType33
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType33", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType33 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID34
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID34", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID34 { get; set; }

        /// <summary>
        /// Gets or sets SourceType34
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType34", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType34 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID35
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID35", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID35 { get; set; }

        /// <summary>
        /// Gets or sets SourceType35
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType35", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType35 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID36
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID36", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID36 { get; set; }

        /// <summary>
        /// Gets or sets SourceType36
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType36", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType36 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID37
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID37", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID37 { get; set; }

        /// <summary>
        /// Gets or sets SourceType37
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType37", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType37 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID38
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID38", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID38 { get; set; }

        /// <summary>
        /// Gets or sets SourceType38
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType38", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType38 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID39
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID39", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID39 { get; set; }

        /// <summary>
        /// Gets or sets SourceType39
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType39", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType39 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID40
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID40", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID40 { get; set; }

        /// <summary>
        /// Gets or sets SourceType40
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType40", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType40 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID41
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID41", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID41 { get; set; }

        /// <summary>
        /// Gets or sets SourceType41
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType41", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType41 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID42
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID42", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID42 { get; set; }

        /// <summary>
        /// Gets or sets SourceType42
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType42", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType42 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID43
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID43", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID43 { get; set; }

        /// <summary>
        /// Gets or sets SourceType43
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType43", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType43 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID44
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID44", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID44 { get; set; }

        /// <summary>
        /// Gets or sets SourceType44
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType44", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType44 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID45
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID45", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID45 { get; set; }

        /// <summary>
        /// Gets or sets SourceType45
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType45", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType45 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID46
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID46", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID46 { get; set; }

        /// <summary>
        /// Gets or sets SourceType46
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType46", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType46 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID47
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID47", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID47 { get; set; }

        /// <summary>
        /// Gets or sets SourceType47
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType47", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType47 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID48
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID48", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID48 { get; set; }

        /// <summary>
        /// Gets or sets SourceType48
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType48", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType48 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID49
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID49", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID49 { get; set; }

        /// <summary>
        /// Gets or sets SourceType49
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType49", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType49 { get; set; }

        /// <summary>
        /// Gets or sets SourceCodeID50
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceCodeID50", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceCodeID50 { get; set; }

        /// <summary>
        /// Gets or sets SourceType50
        /// </summary>
        [StringLength(2, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "SourceType50", ResourceType = typeof (SourceJournalProfileResx))]
        public string SourceType50 { get; set; }

        /// <summary>
        /// Gets or sets RESERVEDFunctionalReportName
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "RESERVEDFunctionalReportName", ResourceType = typeof (SourceJournalProfileResx))]
        public string RESERVEDFunctionalReportName { get; set; }

        /// <summary>
        /// Gets or sets RESERVEDSourceReportName
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength",ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "RESERVEDSourceReportName", ResourceType = typeof (SourceJournalProfileResx))]
        public string RESERVEDSourceReportName { get; set; }

        #region UI Strings

        #endregion
    }
}