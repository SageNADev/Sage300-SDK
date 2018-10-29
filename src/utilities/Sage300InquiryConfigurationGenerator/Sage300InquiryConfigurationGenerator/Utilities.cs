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
using Sage300InquiryConfigurationGenerator.Properties;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
#endregion

namespace Sage300InquiryConfigurationGenerator
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

        /// <summary>
        /// Get the name of this application, it's version number
        /// and build date
        /// Values are returned via output string parameters
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="ver">Application Version</param>
        /// <param name="buildDate">Application Build Date</param>
        /// <param name="buildYear">Application Build Year</param>
        public static void GetAppInfo(out string name, out string ver, out string buildDate, out string buildYear)
        {
            var assemblyName = typeof(Program).Assembly.GetName();
            name = assemblyName.Name + ".exe";
            ver = assemblyName.Version.ToString();
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            buildDate = linkerTime.ToString(@"dddd, MMMM dd,yyyy @ HH:mm:ss");
            buildYear = linkerTime.Year.ToString();
        }

        /// <summary>
        /// Get the application timestamp value
        /// This is an extension method for the dotnet 'Assembly' class
        /// </summary>
        /// <param name="assembly">The assembly</param>
        /// <param name="target">The target timezone. Defaults to null</param>
        /// <returns></returns>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
    }
}
