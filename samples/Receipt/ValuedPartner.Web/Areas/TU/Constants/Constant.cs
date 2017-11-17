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

namespace ValuedParter.Web.Areas.TU.Constants
{
    /// <summary>
    /// Constants used across TU module
    /// </summary>
    public static class Constants
    {
        #region Finder
        
        /// <summary>
        ///  Receipt finder
        /// </summary>
        public const string ReceiptFinder = "tureceiptfinder";

        #endregion

        #region Export/Import

        public const string ReceiptExportImport = "tureceipt";
        
        #endregion

        public const string AppId = "TU";

        #region Grid Preferences

        #endregion

        #region Cache Key

        #endregion

        #region Views path

        public const string ReceiptViewPath = "~/Areas/TU/Views/Receipt/Partials/_Receipt.cshtml";
        public const string ReceiptGridViewPath = "~/Areas/TU/Views/Receipt/Partials/_ReceiptGrid.cshtml";
        public const string ReceiptOptionalFieldsViewPath = "~/Areas/TU/Views/Receipt/Partials/_OptionalFieldGrid.cshtml";
        public const string ReceiptDetailOptionalFieldsViewPath = "~/Areas/TU/Views/Receipt/Partials/_DetailGridOptionalField.cshtml";
        public const string ReceiptExchangeRateViewPath = "~/Areas/TU/Views/Receipt/Partials/_ExchangeRate.cshtml";
        public const string ReceiptLocalizationPath = "~/Areas/TU/Views/Receipt/Partials/_Localization.cshtml";
        #endregion
    }
}