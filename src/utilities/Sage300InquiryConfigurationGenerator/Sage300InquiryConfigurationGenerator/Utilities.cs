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
using Sage300InquiryConfigurationGenerator.Forms;
using Sage300InquiryConfigurationGenerator.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var msgBox = new ModalMessageBox();
            msgBox.Caption = Resources.Status;
            msgBox.Title = "Success";
            msgBox.Message = msg;
            msgBox.Buttons = MessageBoxButtons.OK;
            DialogResult result = msgBox.ShowDialog();
            msgBox.Dispose();
        }

        /// <summary>
        /// Display a simple error message box
        /// </summary>
        /// <param name="msg">The message text</param>
        public static void DisplayErrorMessage(string msg)
        {
            var msgBox = new ModalMessageBox();
            msgBox.Caption = Resources.Status;
            msgBox.Title = "Validation Errors";
            msgBox.Message = msg;
            msgBox.Buttons = MessageBoxButtons.OK;
            DialogResult result = msgBox.ShowDialog();
            msgBox.Dispose();
        }

        /// <summary>
        /// Display a simple confirmation dialog box
        /// </summary>
        /// <param name="msgIn">The text message to display </param>
        /// <returns>true = Proceed | false = Abort</returns>
        public static bool Confirmation(string caption, string title, string msgIn)
        {
            var message = String.Format("{0}{3}{3}{1}{3}{2}",
                                        msgIn,
                                        Resources.PressOKToProceed,
                                        Resources.PressCancelToAbort,
                                        Environment.NewLine);
            var msgBox = new ModalMessageBox();
            msgBox.Caption = caption;
            msgBox.Title = title;
            msgBox.Message = message;
            msgBox.Buttons = MessageBoxButtons.OKCancel;
            DialogResult result = msgBox.ShowDialog();
            msgBox.Dispose();
            return (result != DialogResult.Cancel);
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

        /// <summary>
        /// Get a list of all controls in a form by type
        /// </summary>
        /// <typeparam name="T">The type of control to find</typeparam>
        /// <param name="control">The root control to query</param>
        /// <returns>List of controls of a particular type</returns>
        public static IEnumerable<T> FindControls<T>(Control control) where T : Control
        {
            // we can't cast here because some controls in here will most likely not be <T>
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => FindControls<T>(ctrl))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Clear out all form text boxes
        /// </summary>
        /// <param name="form">A reference to the form</param>
        public static void ClearAllTextBoxes(MainForm form)
        {
            var controls = FindControls<BorderedTextBox>(form);
            foreach (var tb in controls)
            {
                tb.Text = String.Empty;
            }
        }

        /// <summary>
        /// Display a Browse for file dialog
        /// </summary>
        /// <param name="fileTypeFilter">The file type filter setting</param>
        /// <param name="fileExists">Check for file existence (defaults to true)</param>
        /// <param name="pathExists">Check for path existence (default to true)</param>
        /// <param name="multiselect">Allow selection of multiple files (defaults to false)</param>
        /// <returns></returns>
        public static string FileBrowser(string fileTypeFilter, 
                                         string dialogTitle = "",
                                         string initialDirectory = "",
                                         bool fileExists = true, 
                                         bool pathExists = true, 
                                         bool multiselect = false)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = fileTypeFilter,
                FilterIndex = 1,
                Multiselect = false,
                Title = dialogTitle,
                InitialDirectory = initialDirectory, 
            };

            // Show the dialog and evaluate action
            var selectedFile = String.Empty;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                selectedFile = String.Empty;
            }
            else
            {
                selectedFile = dialog.FileName.Trim();
            }

            return selectedFile;
        }
    }
}
