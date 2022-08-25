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

using ValuedPartner.TU.Models.Enums; // For common enumerations
using ValuedPartner.TU.Models.Enums.Process;
using ValuedPartner.TU.Resources; // For common resources
using ValuedPartner.TU.Resources.Process;

#endregion

namespace ValuedPartner.TU.Models.Process
{
    /// <summary>
    /// Partial class for ClearStatistics
    /// </summary>
    public partial class ClearStatistics : ModelBase
    {
        /// <summary>
        /// Gets or sets FromCustomerNumber
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FromCustomerNumber", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.FromCustomerNumber, Id = Index.FromCustomerNumber, FieldType = EntityFieldType.Char, Size = 12, Mask = "%-12C")]
        public string FromCustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets ToCustomerNumber
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ToCustomerNumber", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ToCustomerNumber, Id = Index.ToCustomerNumber, FieldType = EntityFieldType.Char, Size = 12, Mask = "%-12C")]
        public string ToCustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets FromGroupCode
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FromGroupCode", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.FromGroupCode, Id = Index.FromGroupCode, FieldType = EntityFieldType.Char, Size = 6, Mask = "%-6N")]
        public string FromGroupCode { get; set; }

        /// <summary>
        /// Gets or sets ToGroupCode
        /// </summary>
        [StringLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ToGroupCode", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ToGroupCode, Id = Index.ToGroupCode, FieldType = EntityFieldType.Char, Size = 6, Mask = "%-6N")]
        public string ToGroupCode { get; set; }

        /// <summary>
        /// Gets or sets FromNationalAccount
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FromNationalAccount", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.FromNationalAccount, Id = Index.FromNationalAccount, FieldType = EntityFieldType.Char, Size = 12, Mask = "%-12C")]
        public string FromNationalAccount { get; set; }

        /// <summary>
        /// Gets or sets ToNationalAccount
        /// </summary>
        [StringLength(12, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ToNationalAccount", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ToNationalAccount, Id = Index.ToNationalAccount, FieldType = EntityFieldType.Char, Size = 12, Mask = "%-12C")]
        public string ToNationalAccount { get; set; }

        /// <summary>
        /// Gets or sets FromSalesperson
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FromSalesperson", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.FromSalesperson, Id = Index.FromSalesperson, FieldType = EntityFieldType.Char, Size = 8, Mask = "%-8N")]
        public string FromSalesperson { get; set; }

        /// <summary>
        /// Gets or sets ToSalesperson
        /// </summary>
        [StringLength(8, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ToSalesperson", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ToSalesperson, Id = Index.ToSalesperson, FieldType = EntityFieldType.Char, Size = 8, Mask = "%-8N")]
        public string ToSalesperson { get; set; }

        /// <summary>
        /// Gets or sets FromItemNumber
        /// </summary>
        [StringLength(16, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "FromItemNumber", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.FromItemNumber, Id = Index.FromItemNumber, FieldType = EntityFieldType.Char, Size = 16, Mask = "%-16C")]
        public string FromItemNumber { get; set; }

        /// <summary>
        /// Gets or sets ToItemNumber
        /// </summary>
        [StringLength(16, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ToItemNumber", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ToItemNumber, Id = Index.ToItemNumber, FieldType = EntityFieldType.Char, Size = 16, Mask = "%-16C")]
        public string ToItemNumber { get; set; }

        /// <summary>
        /// Gets or sets ClearCustomerStatistics
        /// </summary>
        [Display(Name = "ClearCustomerStatistics", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ClearCustomerStatistics, Id = Index.ClearCustomerStatistics, FieldType = EntityFieldType.Int, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ClearCustomerStatistics ClearCustomerStatistics { get; set; }

        /// <summary>
        /// Gets or sets ClearGroupStatistics
        /// </summary>
        [Display(Name = "ClearGroupStatistics", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ClearGroupStatistics, Id = Index.ClearGroupStatistics, FieldType = EntityFieldType.Int, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ClearGroupStatistics ClearGroupStatistics { get; set; }

        /// <summary>
        /// Gets or sets ClearNationalAccountStatistics
        /// </summary>
        [Display(Name = "ClearNationalAccountStatistics", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ClearNationalAccountStatistics, Id = Index.ClearNationalAccountStatistics, FieldType = EntityFieldType.Int, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ClearNationalAccountStatistics ClearNationalAccountStatistics { get; set; }

        /// <summary>
        /// Gets or sets ClearSalespersonStatistics
        /// </summary>
        [Display(Name = "ClearSalespersonStatistics", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ClearSalespersonStatistics, Id = Index.ClearSalespersonStatistics, FieldType = EntityFieldType.Int, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ClearSalespersonStatistics ClearSalespersonStatistics { get; set; }

        /// <summary>
        /// Gets or sets ClearItemStatistics
        /// </summary>
        [Display(Name = "ClearItemStatistics", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ClearItemStatistics, Id = Index.ClearItemStatistics, FieldType = EntityFieldType.Int, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ClearItemStatistics ClearItemStatistics { get; set; }

        /// <summary>
        /// Gets or sets ThroughCustomerYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ThroughCustomerYear", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughCustomerYear, Id = Index.ThroughCustomerYear, FieldType = EntityFieldType.Char, Size = 4, Mask = "%04D")]
        public string ThroughCustomerYear { get; set; }

        /// <summary>
        /// Gets or sets ThroughCustomerPeriod
        /// </summary>
        [Display(Name = "ThroughCustomerPeriod", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughCustomerPeriod, Id = Index.ThroughCustomerPeriod, FieldType = EntityFieldType.Char, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ThroughCustomerPeriod ThroughCustomerPeriod { get; set; }

        /// <summary>
        /// Gets or sets ThroughNationalAccountYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ThroughNationalAccountYear", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughNationalAccountYear, Id = Index.ThroughNationalAccountYear, FieldType = EntityFieldType.Char, Size = 4, Mask = "%04D")]
        public string ThroughNationalAccountYear { get; set; }

        /// <summary>
        /// Gets or sets ThroughNationalAccountPeriod
        /// </summary>
        [Display(Name = "ThroughNationalAccountPeriod", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughNationalAccountPeriod, Id = Index.ThroughNationalAccountPeriod, FieldType = EntityFieldType.Char, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ThroughNationalAccountPeriod ThroughNationalAccountPeriod { get; set; }

        /// <summary>
        /// Gets or sets ThroughGroupYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ThroughGroupYear", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughGroupYear, Id = Index.ThroughGroupYear, FieldType = EntityFieldType.Char, Size = 4, Mask = "%04D")]
        public string ThroughGroupYear { get; set; }

        /// <summary>
        /// Gets or sets ThroughGroupPeriod
        /// </summary>
        [Display(Name = "ThroughGroupPeriod", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughGroupPeriod, Id = Index.ThroughGroupPeriod, FieldType = EntityFieldType.Char, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ThroughGroupPeriod ThroughGroupPeriod { get; set; }

        /// <summary>
        /// Gets or sets ThroughSalespersonYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ThroughSalespersonYear", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughSalespersonYear, Id = Index.ThroughSalespersonYear, FieldType = EntityFieldType.Char, Size = 4, Mask = "%04D")]
        public string ThroughSalespersonYear { get; set; }

        /// <summary>
        /// Gets or sets ThroughSalespersonPeriod
        /// </summary>
        [Display(Name = "ThroughSalespersonPeriod", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughSalespersonPeriod, Id = Index.ThroughSalespersonPeriod, FieldType = EntityFieldType.Char, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ThroughSalespersonPeriod ThroughSalespersonPeriod { get; set; }

        /// <summary>
        /// Gets or sets ThroughItemYear
        /// </summary>
        [StringLength(4, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(AnnotationsResx))]
        [Display(Name = "ThroughItemYear", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughItemYear, Id = Index.ThroughItemYear, FieldType = EntityFieldType.Char, Size = 4, Mask = "%04D")]
        public string ThroughItemYear { get; set; }

        /// <summary>
        /// Gets or sets ThroughItemPeriod
        /// </summary>
        [Display(Name = "ThroughItemPeriod", ResourceType = typeof(ClearStatisticsResx))]
        [ViewField(Name = Fields.ThroughItemPeriod, Id = Index.ThroughItemPeriod, FieldType = EntityFieldType.Char, Size = 2)]
        public ValuedPartner.TU.Models.Enums.Process.ThroughItemPeriod ThroughItemPeriod { get; set; }

        #region UI Strings

        /// <summary>
        /// Gets ClearCustomerStatistics string value
        /// </summary>
        public string ClearCustomerStatisticsString => EnumUtility.GetStringValue(ClearCustomerStatistics);

        /// <summary>
        /// Gets ClearGroupStatistics string value
        /// </summary>
        public string ClearGroupStatisticsString => EnumUtility.GetStringValue(ClearGroupStatistics);

        /// <summary>
        /// Gets ClearNationalAccountStatistics string value
        /// </summary>
        public string ClearNationalAccountStatisticsString => EnumUtility.GetStringValue(ClearNationalAccountStatistics);

        /// <summary>
        /// Gets ClearSalespersonStatistics string value
        /// </summary>
        public string ClearSalespersonStatisticsString => EnumUtility.GetStringValue(ClearSalespersonStatistics);

        /// <summary>
        /// Gets ClearItemStatistics string value
        /// </summary>
        public string ClearItemStatisticsString => EnumUtility.GetStringValue(ClearItemStatistics);

        /// <summary>
        /// Gets ThroughCustomerPeriod string value
        /// </summary>
        public string ThroughCustomerPeriodString => EnumUtility.GetStringValue(ThroughCustomerPeriod);

        /// <summary>
        /// Gets ThroughNationalAccountPeriod string value
        /// </summary>
        public string ThroughNationalAccountPeriodString => EnumUtility.GetStringValue(ThroughNationalAccountPeriod);

        /// <summary>
        /// Gets ThroughGroupPeriod string value
        /// </summary>
        public string ThroughGroupPeriodString => EnumUtility.GetStringValue(ThroughGroupPeriod);

        /// <summary>
        /// Gets ThroughSalespersonPeriod string value
        /// </summary>
        public string ThroughSalespersonPeriodString => EnumUtility.GetStringValue(ThroughSalespersonPeriod);

        /// <summary>
        /// Gets ThroughItemPeriod string value
        /// </summary>
        public string ThroughItemPeriodString => EnumUtility.GetStringValue(ThroughItemPeriod);

        #endregion
    }
}
