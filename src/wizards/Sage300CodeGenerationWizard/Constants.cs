// The MIT License (MIT) 
// Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved.
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

#region Imports
#endregion

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// Public constants shared among CodeGenerationWizard files
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary> The version of this wizard </summary>
        public static string Version = "2025.0";

        /// <summary> The Accpac Net Version </summary>
        public static string AccpacDotNetVersion = "72A";

        public const string ResourceFileExtension = @".resx";

        public static class LanguageExtensions
        {
            public const string English = @"";
            public const string Spanish = @"es";
            public const string French = @"fr";
            public const string ChineseSimplified = @"zh-Hans";
            public const string ChineseTraditional = @"zh-Hant";
        }

        public static class LanguageIndex
        {
            public const int English = 0;
            public const int Spanish = 1;
            public const int French = 2;
            public const int ChineseSimplified = 3;
            public const int ChineseTraditional = 4;
        }
    }
}
