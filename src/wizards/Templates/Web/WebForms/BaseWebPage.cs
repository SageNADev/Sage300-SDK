// Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.

using System;
using System.IO;
using System.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Authentication;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using Sage.CA.SBS.ERP.Sage300.Core.Configuration;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;

namespace $companynamespace$.$applicationid$.Web.WebForms
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
