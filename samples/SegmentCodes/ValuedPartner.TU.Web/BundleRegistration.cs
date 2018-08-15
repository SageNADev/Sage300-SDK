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

using System.Web.Optimization;

namespace ValuedPartner.TU.Web
{
    /// <summary>
    /// Class for bundle registration
    /// </summary>
    internal static class BundleRegistration
    {
        /// <summary>
        /// Register bundles
        /// </summary>
        /// <param name="bundles"></param> 
        internal static void RegisterBundles(BundleCollection bundles)
        {
			#region SegmentCodes
			bundles.Add(new Bundle("~/bundles/ValuedPartnerSegmentCodes").Include(
				"~/Areas/TU/Scripts/SegmentCodes/ValuedPartnerSegmentCodesBehaviour.js",
				"~/Areas/TU/Scripts/SegmentCodes/ValuedPartnerSegmentCodesKoExtn.js",
				"~/Areas/TU/Scripts/SegmentCodes/ValuedPartnerSegmentCodesRepository.js",
                "~/Areas/TU/Scripts/SegmentCodes/ValuedPartnerSegmentCodesGrid.js",
				"~/Areas/Core/Scripts/Process/Sage.CA.SBS.Sage300.Common.Process.js"));
			#endregion


        }
    }
}
