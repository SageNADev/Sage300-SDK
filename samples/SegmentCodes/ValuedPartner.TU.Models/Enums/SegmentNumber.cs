// Copyright © 2016 ValuedPartner

#region Namespace

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Resources.Forms;

#endregion

namespace ValuedPartner.TU.Models.Enums
{
    /// <summary>
    /// Enum for SegmentNumber
    /// </summary>
    public enum SegmentNumber
    {
        /// <summary>
        /// Gets or sets Category
        /// </summary>
        [EnumValue("Category", typeof(SegmentCodesResx))]
        Category = 1,

        /// <summary>
        /// Gets or sets Style
        /// </summary>
        [EnumValue("Style", typeof(SegmentCodesResx))]
        Style = 2,

        /// <summary>
        /// Gets or sets Color
        /// </summary>
        [EnumValue("Color", typeof(SegmentCodesResx))]
        Color = 3,

        /// <summary>
        /// Gets or sets Model
        /// </summary>
        [EnumValue("Model", typeof(SegmentCodesResx))]
        Model = 4,

        /// <summary>
        /// Gets or sets Series
        /// </summary>
        [EnumValue("Series", typeof(SegmentCodesResx))]
        Series = 5,

        /// <summary>
        /// Gets or sets Long
        /// </summary>
        [EnumValue("Long", typeof(SegmentCodesResx))]
        Long = 6

    }
}