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

#region Imports
using ACCPAC.Advantage;
using Sage300InquiryConfigurationWizardUI.Properties;
using System;
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    /// <summary>
    /// General purpose Utility methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Test the Sage 300 authentication credentials
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="companyName">The company name</param>
        /// <param name="version">The version</param>
        /// <returns></returns>
        public static bool ValidateCredentials(string username, string password, string companyName, string version)
        {
            bool isValid = true;
            try
            {
                var session = new Session();
                session.CreateSession(null, "WX", "WX1000", version, username, password, companyName, DateTime.UtcNow);
                var dbLink = session.OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadOnly);

                dbLink.Dispose();
                session.Dispose();
            }
            catch (Exception)
            {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Display a simple success message box
        /// </summary>
        /// <param name="msg">The message text</param>
        public static void DisplaySuccessMessage(string msg)
        {
            MessageBox.Show(msg, Resources.Status, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Display a simple error message box
        /// </summary>
        /// <param name="msg">The message text</param>
        public static void DisplayErrorMessage(string msg)
        {
            MessageBox.Show(msg, Resources.Status, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
