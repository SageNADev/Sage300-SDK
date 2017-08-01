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
using ValuedParter.TU.Resources.Forms;

#endregion

namespace ValuedParter.TU.Models.Enums
{
    /// <summary>
    /// Enum for ProcessCommand
    /// </summary>
    public enum ProcessCommand
    {
        /// <summary>
        /// Gets or sets NothingToProcess
        /// </summary>
        [EnumValue("NothingToProcess", typeof(ReceiptDetailResx))]
        NothingToProcess = 0,

        /// <summary>
        /// Gets or sets InsertOptionalFields
        /// </summary>
        [EnumValue("InsertOptionalFields", typeof(ReceiptDetailResx))]
        InsertOptionalFields = 1,

        /// <summary>
        /// Gets or sets AutogenerateSerials
        /// </summary>
        [EnumValue("AutogenerateSerials", typeof(ReceiptDetailResx))]
        AutogenerateSerials = 21,

        /// <summary>
        /// Gets or sets AutogenerateLots
        /// </summary>
        [EnumValue("AutogenerateLots", typeof(ReceiptDetailResx))]
        AutogenerateLots = 22,

        /// <summary>
        /// Gets or sets AutoallocateSerials
        /// </summary>
        [EnumValue("AutoallocateSerials", typeof(ReceiptDetailResx))]
        AutoallocateSerials = 23,

        /// <summary>
        /// Gets or sets AutoallocateLots
        /// </summary>
        [EnumValue("AutoallocateLots", typeof(ReceiptDetailResx))]
        AutoallocateLots = 24,

        /// <summary>
        /// Gets or sets ClearSerials
        /// </summary>
        [EnumValue("ClearSerials", typeof(ReceiptDetailResx))]
        ClearSerials = 25,

        /// <summary>
        /// Gets or sets ClearLots
        /// </summary>
        [EnumValue("ClearLots", typeof(ReceiptDetailResx))]
        ClearLots = 26,

        /// <summary>
        /// Gets or sets AutoassignSerials
        /// </summary>
        [EnumValue("AutoassignSerials", typeof(ReceiptDetailResx))]
        AutoassignSerials = 27,

        /// <summary>
        /// Gets or sets AutoassignLots
        /// </summary>
        [EnumValue("AutoassignLots", typeof(ReceiptDetailResx))]
        AutoassignLots = 28,

        /// <summary>
        /// Gets or sets PostSerialsLotsToICInventory
        /// </summary>
        [EnumValue("PostSerialsLotsToICInventory", typeof(ReceiptDetailResx))]
        PostSerialsLotsToICInventory = 31

    }
}