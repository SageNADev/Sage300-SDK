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

using System;
using System.IO;
using System.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Authentication;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;

namespace ValuedPartner.Web.WebForms
{
    /// <summary>
    /// Class BaseWebPage.
    /// </summary>
    public class BaseWebPage : System.Web.UI.Page
    {
        /// <summary>
        /// Authenticated User
        /// </summary>
        protected UserTenantInfo AuthenticatedUser { get; private set; }

        /// <summary>
        /// OnInit 
        /// Authentication is implemented in this method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            AuthenticatedUser = SignOnHelper.GetStoredUserSignOnResult();
            if (ConfigurationHelper.IsOnPremise)
            {
                var path = Path.Combine(RegistryHelper.SharedDataDirectory, string.Format("{0}.auth", HttpContext.Current.Session.SessionID));
                if (File.Exists(path))
                {
                    var userTenantInfo = File.ReadAllText(path);
                    File.Delete(path);
                    AuthenticatedUser = JsonSerializer.Deserialize<UserTenantInfo>(userTenantInfo);
                }
            }

            if (AuthenticatedUser == null)
            {
                Response.RedirectToRoute(new { area = "Core", controller = "Authentication", action = "Login"});
                Response.End();
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Determines whether [is user authenticated].
        /// </summary>
        /// <returns></returns>
        public static bool IsUserAuthenticated()
        {
            var userTenantInfo = SignOnHelper.GetStoredUserSignOnResult();

            return (userTenantInfo != null);
        }
    }
}