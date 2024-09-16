// The MIT License (MIT) 
// Copyright (c) 2024 The Sage Group plc or its licensors.  All rights reserved.
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
using Newtonsoft.Json;
using Sage.CA.SBS.ERP.Sage300.Common.Web.HtmlHelperExtension;
using Sage.CA.SBS.ERP.Sage300.ProxyTester.Models;
using Sage.CA.SBS.ERP.Sage300.ProxyTester.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.HtmlControls;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Controllers
{
    /// <summary> Controller for Sage 300 Proxy Tester </summary>
    public class HomeController : Controller
    {
        HttpClient httpClient = new HttpClient();

        /// <summary> Constants for Request Header </summary>
        public class RequestHeader
        {
            /// <summary> Credentials Constant </summary>
            public const string Credentials = "ProxyCredentials";
            /// <summary> User Constant </summary>
            public const string User = "ProxyUser";
            /// <summary> Password Constant </summary>
            public const string Password = "ProxyPassword";
            /// <summary> Company Constant </summary>
            public const string Company = "ProxyCompany";
            /// <summary> ClientPublicKey Constant </summary>
            public const string ClientPublicKey = "ProxyClientPublicKey";
            /// <summary> ServerPublicKey Constant </summary>
            public const string ServerPublicKey = "ProxyServerPublicKey";
            /// <summary> IV Constant </summary>
            public const string IV = "ProxyIV";
            /// <summary> ProductId Constant </summary>
            public const string ProductId = "ProxyProductId";
            /// <summary> TokenId Constant </summary>
            public const string TokenId = "ProxyToken";
            /// <summary> ModuleId Constant </summary>
            public const string ModuleId = "ProxyModuleId";
            /// <summary> Controller Constant </summary>
            public const string Controller = "ProxyController";
            /// <summary> Action Constant </summary>
            public const string Action = "ProxyAction";
            /// <summary> OptionalParameters Constant </summary>
            public const string OptionalParameters = "ProxyOptionalParameters";
        }

        #region Constructor
        /// <summary> Default constructor </summary>
        public HomeController() { }
        #endregion

        #region Public
        public ActionResult Index()
        {
            var model = new ProxyTesterViewModel();
            BuildRequestUrl(model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> TheForm(ProxyTesterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BuildRequestUrl(model);

                // Get the proxy public Key
                await ProxyPublicKey(model);

                // Login via proxy if a public key has been retrieved
                if (!string.IsNullOrEmpty(model.ProxyPublicKey))
                {
                    // Perform login. Test to see if already logged in
                    await ProxyIsValidToken(model);
                    if (!model.IsValidToken)
                    {
                        await ProxyLogin(model);
                    }

                    // Login via proxy if a public key has been retrieved
                    if (!string.IsNullOrEmpty(model.Token))
                    {
                        // Test Menu or Screen?
                        if (model.TestAction == "Menu")
                        {
                            // Test menu from proxy
                            await ProxyMenu(model);
                        }
                        else
                        {
                            // Test screen from proxy
                            await ProxyScreen(model);
                        }
                    }
                }

            }

            return View("Index", model);
        }
        #endregion
        /// <summary> Routine to get the Public Key from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign ProxyPublicKey to model</returns>
        private async Task ProxyPublicKey(ProxyTesterViewModel model)
        {
            // Public key returned from Proxy
            model.ProxyPublicKey = string.Empty;

            // Request
            var request = new HttpRequestMessage(HttpMethod.Get, model.PublicKeyUrl);

            // Await response
            using (var response = await httpClient.SendAsync(request))
            {
                // If successful get the public key
                if (response.IsSuccessStatusCode)
                {
                    model.ProxyPublicKey = await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary> Routine to login to the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Token to model</returns>
        private async Task ProxyLogin(ProxyTesterViewModel model)
        {
            // Token returned from Proxy
            model.Token = string.Empty;

            // Create key pair
            EllipticCurveDiffieHellman.CreateKeyPair(out byte[] privateKey, out byte[] publicKey);

            // Encrypt credentials
            var credentials = new
            {
                ProxyUser = model.User,
                ProxyPassword = model.Password,
                ProxyCompany = model.Company
            };
            model.Credentials = EllipticCurveDiffieHellman.Encrypt(JsonConvert.SerializeObject(credentials),
                privateKey, Convert.FromBase64String(model.ProxyPublicKey), out byte[] iv);

            model.ClientPublicKey = Convert.ToBase64String(publicKey);
            model.IV = Convert.ToBase64String(iv);

            // Request
            var request = new HttpRequestMessage(HttpMethod.Get, model.LoginUrl);

            // Add headers
            request.Headers.Add(RequestHeader.Credentials, model.Credentials);
            request.Headers.Add(RequestHeader.ClientPublicKey, model.ClientPublicKey);
            request.Headers.Add(RequestHeader.ServerPublicKey, model.ProxyPublicKey);
            request.Headers.Add(RequestHeader.IV, model.IV);
            request.Headers.Add(RequestHeader.ProductId, "PROXY");

            // Await response
            using (var response = await httpClient.SendAsync(request))
            {
                // If successful get the login token
                if (response.IsSuccessStatusCode)
                {
                    model.Token = await response.Content.ReadAsStringAsync();
                }
            }

        }

        /// <summary> Routine to get the menu for the requested module from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Source to model</returns>
        private async Task ProxyMenu(ProxyTesterViewModel model)
        {
            // Request
            var request = new HttpRequestMessage(HttpMethod.Post, model.MenuUrl);

            // Add headers
            request.Headers.Add(RequestHeader.TokenId, model.Token);
            request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);

            // Await response
            using (var response = await httpClient.SendAsync(request))
            {
                // If successful get the menu payload
                if (response.IsSuccessStatusCode)
                {
                    model.Source = await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary> Routine to determine if the Token (Login) is still valid from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign IsValidToken to model</returns>
        private async Task ProxyIsValidToken(ProxyTesterViewModel model)
        {
            // Init
            model.IsValidToken = false;

            // Request
            var request = new HttpRequestMessage(HttpMethod.Post, model.IsValidTokenUrl);

            request.Headers.Add(RequestHeader.TokenId, model.Token);

            // Await response
            using (var response = await httpClient.SendAsync(request))
            {
                // If successful get whether token is still valid
                if (response.IsSuccessStatusCode)
                {
                    model.IsValidToken = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                }
            }

        }

        /// <summary> Routine to get the screen from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Source/Redirect to model</returns>
        private async Task ProxyScreen(ProxyTesterViewModel model)
        {
            // Request
            var request = new HttpRequestMessage(HttpMethod.Post, model.ScreenUrl);

            // Add headers
            request.Headers.Add(RequestHeader.TokenId, model.Token);
            request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);
            request.Headers.Add(RequestHeader.Controller, model.Controller);
            request.Headers.Add(RequestHeader.Action, model.Action);
            request.Headers.Add(RequestHeader.OptionalParameters, model.OptionalParameters);
            request.Headers.Add(RequestHeader.ProductId, "PROXY");

            // Await response
            using (var response = await httpClient.SendAsync(request))
            {
                // If successful get the screen redirect
                if (response.IsSuccessStatusCode)
                {
                    model.Source = await response.Content.ReadAsStringAsync();
                }
            }

        }

        /// <summary> Routine to build URLs for the Proxy </summary>
        /// <param name="model">View Model</param>
        private void BuildRequestUrl(ProxyTesterViewModel model)
        {
            // Force case
            model.User = model.User.ToUpper();
            model.Company = model.Company.ToUpper();
            model.ModuleId = model.ModuleId.ToUpper();

            model.PublicKeyUrl = $"{model.Server}/ProxyPublicKey";
            model.LoginUrl = $"{model.Server}/ProxyLogin";
            model.MenuUrl = $"{model.Server}/ProxyMenu";
            model.IsValidTokenUrl = $"{model.Server}/ProxyIsValidToken";
            model.ScreenUrl = $"{model.Server}/ProxyScreen";
        }

    }
}