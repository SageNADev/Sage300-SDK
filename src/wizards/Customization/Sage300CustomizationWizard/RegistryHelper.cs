// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

using System.IO;
using Microsoft.Win32;

namespace Sage.CA.SBS.ERP.Sage300.CustomizationWizard
{
    /// <summary> Registry Helper Class </summary>
    public static class RegistryHelper
    {
        /// <summary>
        /// The path to the Registry Key where the name of the shared folder is stored
        /// </summary>
        private const string ConfigurationKey = "SOFTWARE\\ACCPAC International, Inc.\\ACCPAC\\Configuration";

        /// <summary>
        /// The name of the Registry Value containing the name of the shared folder
        /// </summary>
        public static string Sage300CWebFolder
        {
            get
            {
                // Get the registry key
                var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                var configurationKey = baseKey.OpenSubKey(ConfigurationKey);

                // Find path tp shared folder
                return configurationKey == null ? string.Empty : Path.Combine(configurationKey.GetValue("Programs").ToString(), @"Online\Web\bin");
            }
        }
    }
}
