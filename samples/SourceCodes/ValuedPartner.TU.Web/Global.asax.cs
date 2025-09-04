
// The MIT License (MIT) 
// Copyright (c) 1994-2025 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Services;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Core.Logging;
using Sage.CA.SBS.ERP.Sage300.Core.Web;
using Sage.CA.SBS.ERP.Sage300.Web;
using Sage.CA.SBS.ERP.Sage300.Web.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
#endregion

namespace ValuedPartner.TU.Web
{
    /// <summary>
    /// MVC application class that provides start and end functionality for application and user sessions
    /// </summary>
    public class MvcApplication : HttpApplication
    {

        /// <summary>
        /// MVC appliction start event
        /// </summary>
        protected void Application_Start()
        {
            // Register areas and routes
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            // Register global filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register scripts and css bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register unity configuration
            BootstrapConfig.Register();

            //Register providers
            ProvidersConfig.Register();

            // Register custom flag enum model binder
            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();

            AsyncManagerConfig.Register();

            ActiveSessionManager.StartTimer(spanTicks: TimeSpan.FromHours(6).Ticks); // token expire time is 6 hours

            // Proxy Manager to keep track of requests for proxy since requests are multi-step requests
            ProxyManager.Start(Sage.CA.SBS.ERP.Sage300.Common.Utilities.Constants.Constant.ProxyMangerLifecycle);

            // Entity Context Manager to keep track of requests for entity context since requests are multi-step requests, if from proxy
            EntityContextManager.Start(240);

            EvictUserWatcher.Instance.Start();        }

        /// <summary>
        /// Log the application error
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var url = @"~\Core\Error";
            const int errorRequestTimeout = unchecked((int)0x80004005);

            var exception = Server.GetLastError();

            if (!string.IsNullOrEmpty(exception.Message) &&
                exception.Message.Contains("SSONotify"))
            {
                // No need to log this SSONotify error as it is a known issue
                return;
            }

            Logger.Error(LoggingConstants.ApplicationError, LoggingConstants.ModuleGlobal, null, exception);

            var exType = exception.GetType();
            if (exType == typeof(HttpException) && exception.HResult.Equals(errorRequestTimeout))
            {
                // Display request timeout message
                url = $@"~\Core\Error\CustomError?resourceName={HttpUtility.UrlEncode(nameof(CommonResx.RequestTimeout))}";
                Response.Redirect(url);
                return;
            }
            else if (exType == typeof(HttpUnhandledException) ||
                     exType == typeof(NoLicenseException) ||
                     exType == typeof(UnauthorizedAccessException))
            {
                var message = exception.Message;

                if (exType == typeof(HttpUnhandledException))
                {
                    var innerExceptionType = exception.InnerException.GetType();
                    if (innerExceptionType == typeof(MissingFileException))
                    {
                        message = exception.InnerException.Message;
                    }
                }

                message = HttpUtility.UrlEncode(message);

                url = $@"~\Core\Error\CustomErrorWithFormattedText?message={message}";
                Response.Redirect(url);
                return;
            }

            // Default handler
            Response.Redirect(url);
        }

        /// <summary>
        /// Set Access-Control-Allow-Origin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Request.Headers.Add("X-Content-Type-Options", "nosniff");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");    
            /* HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://SageCRM.com"); */
        }

        /// <summary>
        /// Application EndRequest event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Enable the cookie's 'secure' flag only if the HTTP protocol is 
            // detected as secure (using HTTPS).
            // This is the code alternative to setting the Web.config file
            // <system.web> with <httpCookies requireSSL="true" />, which is
            // more restrictive and requires the webscreens to be hosted as HTTPS.


            if (Context.Items["Context"] != null)
            {
                var context = Context.Items["Context"] as Sage.CA.SBS.ERP.Sage300.Common.Models.Context;

                //remove condition if we decide to always to clean-up Guid with all 0's 
                if (context.ContextCachedToken.Count() > 0)
                {
                    foreach (Guid contextToken in context.ContextCachedToken)
                    {
                        CommonService.ReleaseCachedContext(context, contextToken);
                    }
                    //clean up the Guid with all 0's
                    CommonService.ReleaseCachedContext(context, new Guid());
                }
                context.ContextCachedToken.Clear();
            }

            if (Response.Cookies.Count > 0)
            {
                var updateFRCookies = false;
                // Intercept all cookies and set their Secure flag.
                foreach (string cookieKey in Response.Cookies.AllKeys)
                {
                    // Allow cross site cookie for external application like CRM and Sage exchange credit card
                    var requestUrl = Request.Url.AbsoluteUri;

                    var containsProductId = requestUrl.Contains("productId");
                    var notFR = !requestUrl.Contains("productId=FR");
                    if (containsProductId && notFR)
                    {
                        // Coming from the proxy, so set accordingly
                        Response.Cookies[cookieKey].SameSite = SameSiteMode.None;
                        Response.Cookies[cookieKey].Secure = true;
                    }

                    if (Request.IsSecureConnection)
                    {
                        Response.Cookies[cookieKey].Secure = true;
                    }
                    if (!notFR && cookieKey == "ASP.NET_SessionId")
                    {
                        updateFRCookies = true;
                    }
                }

                if (updateFRCookies)
                {
                    RemoveDuplicateCookieItem(Response, "ASP.NET_SessionId");
                }
            }
        }

        /// <summary>
        /// Remove the duplicate key from cookie, only keep the first non-empty value
        /// </summary>
        /// <param name="response">HttpResponse</param>
        /// <param name="key">Key name</param>
        private static void RemoveDuplicateCookieItem(HttpResponse response, string key)
        {
            //find keys based on key name
            var keys = response.Cookies.AllKeys
                .Select((n, i) => new { n, i })
                .Where(x => x.n == key);
            var valFound = string.Empty;
            //find duplicated keys
            if (keys.Count() > 1)
            {
                foreach (var val in keys)
                {
                    if (val != null)
                    {
                        var value = response.Cookies.Get(val.i).Value;
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            valFound = value;
                            break;
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(valFound))
                {
                    //clean up duplicated key from cookie
                    response.Cookies.Remove(key);
                    response.Cookies.Add(new HttpCookie(key)
                    {
                        Value = valFound,
                        Expires = DateUtil.GetMaxDate(),
                        HttpOnly = true
                    });
                }
            }
        }

        /// <summary>
        /// For Sage 300 login request set asp.net session cookie SameSite as Lax.
        /// For cross site request, like CRM, set asp.net session cookie SameSite as None
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Session_Start(object sender, EventArgs e)
        {
            var containsProductId = Request.Url.AbsoluteUri.Contains("productId");
            var notFR = !Request.Url.AbsoluteUri.Contains("productId=FR");

            var isExternalApp = containsProductId && notFR;
            if (!isExternalApp)
            {
                Response.Cookies["ASP.NET_SessionId"].SameSite = SameSiteMode.Lax;
                if (Request.IsSecureConnection)
                {
                    Response.Cookies["ASP.NET_SessionId"].Secure = true;
                }
            }
            else
            {
                // Coming from the proxy, so set accordingly
                Response.Cookies["ASP.NET_SessionId"].SameSite = SameSiteMode.None;
                Response.Cookies["ASP.NET_SessionId"].Secure = true;
            }
        }

        /// <summary>
        /// Event triggered when Session expires/ends
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Session_End(object sender, EventArgs e)
        {
            //This will never be called if sessions are stored in azure cache
            ActiveSessionManager.RemoveSession(Session.SessionID);
        }

        /// <summary>
        /// MVC application end event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            CommonService.ClearSessionLogs();
            ActiveSessionManager.StopTimer();
        }
    }
}