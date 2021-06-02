
// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

#region
using ACCPAC.Advantage;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

#endregion

namespace ValuedPartner.TU.Web
{
    /// <summary> Login screen for debug </summary>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary> Login constants </summary>
        private class Constants
        {
            public const string COOKIE = "Sage300WebSDKDebug";
            public const string USER_KEY = "user";
            public const string USER_DEFAULT = "ADMIN";
            public const string COMPANY_KEY = "company";
            public const string COMPANY_DEFAULT = "SAMLTD";
            public const string SYSTEM_KEY = "system";
            public const string SYSTEM_DEFAULT = "SAMSYS";
            public const string VERSION_KEY = "version";
            public const string VERSION_DEFAULT = "69A";
        }

        /// <summary>
        /// Page load for debug
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only load controls on initial load
            if (string.IsNullOrEmpty(UserText.Text))
            {
                // Get and assign cookies
                UserText.Text = GetCookie(Constants.USER_KEY, Constants.USER_DEFAULT);
                CompanyText.Text = GetCookie(Constants.COMPANY_KEY, Constants.COMPANY_DEFAULT);
                SystemText.Text = GetCookie(Constants.SYSTEM_KEY, Constants.SYSTEM_DEFAULT);
                VersionText.Text = GetCookie(Constants.VERSION_KEY, Constants.VERSION_DEFAULT);
                ErrorLabel.Text = "";

                // Set focus to password control
                PwdText.Focus();
            }
        }

        /// <summary>
        /// Get cookie
        /// </summary>
        /// <param name="key">Cookie key</param>
        /// <param name="value">Default value</param>
        /// <returns>Default or cookie value</returns>
        private string GetCookie(string key, string value)
        {
            if (Request.Cookies[Constants.COOKIE] == null || Request.Cookies[Constants.COOKIE].Values[key] == null)
            {
                return value;
            }
            else
            {
                return Request.Cookies[Constants.COOKIE].Values[key].ToString();
            }
        }

        /// <summary>
        /// Save cookie
        /// </summary>
        /// <param name="user">User value</param>
        /// <param name="company">Company value</param>
        /// <param name="system">System value</param>
        /// <param name="version">Version value</param>
        private void SaveCookie(string user, string company, string system, string version)
        {
            var cookie = Request.Cookies[Constants.COOKIE] ?? new HttpCookie(Constants.COOKIE);

            cookie.Values[Constants.USER_KEY] = user;
            cookie.Values[Constants.COMPANY_KEY] = company;
            cookie.Values[Constants.SYSTEM_KEY] = system;
            cookie.Values[Constants.VERSION_KEY] = version;
            cookie.Expires = DateTime.Now.AddDays(30);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Validate credentials
        /// </summary>
        /// <param name="user">User value</param>
        /// <param name="password">Password value</param>
        /// <param name="company">Company value</param>
        /// <param name="version">Version value</param>
        /// <returns>True if valid otherwise false</returns>
        private bool ValidCredentials(string user, string password, string company, string version)
        {
            var valid = true;
            ErrorLabel.Text = "";

            try
            {
                // Init session to see if credentials are valid
                var session = new Session();
                session.InitEx2(null, string.Empty, "WX", "WX1000", version, 1);
                session.Open(user, password, company, DateTime.UtcNow, 0);
                session = null;
            }
            catch
            {
                valid = false;
                ErrorLabel.Text = "Invalid credentials. Please re-enter.";
            }

            return valid;
        }

        /// <summary>
        /// Login Action
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            // Force case and trim
            var user = UserText.Text.ToUpper().Trim();
            var pwd = PwdText.Text.ToUpper().Trim();
            var company = CompanyText.Text.ToUpper().Trim();
            var system = SystemText.Text.ToUpper().Trim();
            var version = VersionText.Text.ToUpper().Trim();

            var redirect = "PageUrl.txt";
            var login = "Login.aspx";

            // Validate credentials early to avoid having real Sage 300 dialog show
            if (!ValidCredentials(user, pwd, company, version))
            {
                return;
            }

            // Locals
            var authenticationManager = new AuthenticationManagerOnPremise();
            authenticationManager.Login();
            var recordId = Guid.NewGuid();

            // Save cookie before proceeding
            SaveCookie(user, company, system, version);

            // Fill context object
            var context = new Context
            {
                AspNetSessionId = HttpContext.Current.Session.SessionID,
                ApplicationUserId = user,
                Company = company,
                ProductUserId = recordId,
                TenantId = recordId,
                TenantAlias = Sage.CA.SBS.ERP.Sage300.Common.Web.AreaConstants.Core.OnPremiseTenantAlias,
                ApplicationType = ApplicationType.WebApplication,
                Language = "en",
                ScreenContext = new ScreenContext(),
                ScreenName = "None",
                Container = BootstrapTaskManager.Container
            };

            // Set session
            var sessionId = $"{context.ApplicationUserId.Trim()}-{context.Company.Trim()}";
            context.ScreenContext.ScreenName = "None";
            context.SessionId = Encoding.UTF8.Base64Encode(sessionId);

            // Set default company information
            var companies = new List<Sage.CA.SBS.ERP.Sage300.Common.Models.Organization>
                {
                    new Sage.CA.SBS.ERP.Sage300.Common.Models.Organization() { Id = company, Name = company,
                        SystemId = system, System = system,
                        IsSecurityEnabled = false }
                };

            // Perform the login
            authenticationManager.LoginResult(company, user, pwd, BootstrapTaskManager.Container, context, companies);

            // Redirect to the last generated page
            var fileUrlPath = Path.Combine(Server.MapPath("~"), redirect);
            if (File.Exists(fileUrlPath))
            {
                var url = File.ReadAllText(fileUrlPath).Trim();
                url = HttpContext.Current.Request.Url.AbsoluteUri.Replace(login, string.Empty) +
                    string.Format(url, context.SessionId);
                Response.Redirect(url);
            }
        }

    }
}