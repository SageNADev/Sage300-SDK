// The MIT License (MIT) 
// Copyright (c) 2024-2025 The Sage Group plc or its licensors.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.ProxyTester.Models;
using Sage.CA.SBS.ERP.Sage300.ProxyTester.Utility;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Controllers
{
    /// <summary> Controller for Sage 300 Proxy Tester </summary>
    public class HomeController : Controller
    {
        readonly HttpClient httpClient = new HttpClient();

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
            /// <summary> ModuleId Constant </summary>
            public const string ModuleId = "ProxyModuleId";
            /// <summary> Controller Constant </summary>
            public const string Controller = "ProxyController";
            /// <summary> Action Constant </summary>
            public const string Action = "ProxyAction";
            /// <summary> OptionalParameters Constant </summary>
            public const string OptionalParameters = "ProxyOptionalParameters";
            /// <summary> Context Token </summary>
            public const string ContextToken = "ContextToken";
            /// <summary> Id Constant </summary>
            public const string Id = "ProxyId";
            /// <summary> PdfName Constant </summary>
            public const string PdfName = "ProxyPDFName";
            /// <summary> PdfDate Constant </summary>
            public const string PdfDate = "ProxyPDFDate";


        }

        #region Constructor
        /// <summary> Default constructor </summary>
        public HomeController() { }
        #endregion

        #region Public
        public ActionResult Index()
        {
            var model = new ProxyTesterViewModel();
            PrepModel(model);
            return View(model);
        }

        /// <summary> GetMenu </summary>
        /// <returns>Menu string</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> GetMenu(ProxyTesterViewModel model)
        {
            try
            {
                // Prep model if needed
                PrepModel(model);

                // Get the proxy public Key
                await ProxyPublicKey(model);

                // Get the menu from proxy
                await ProxyMenu(model);

                // Return menu string
                return Content(model.Source);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> GetScreen </summary>
        /// <returns>URL string</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> GetScreen(ProxyTesterViewModel model)
        {
            try
            {
                // Prep model if needed
                PrepModel(model);

                // Get the proxy public Key
                await ProxyPublicKey(model);

                // Get the screen from proxy
                await ProxyScreen(model);

                // Return url string
                return Content(model.Source);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> GetPDF </summary>
        /// <returns>URL string</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> GetPDF(ProxyTesterViewModel model)
        {
            try
            {
                // Prep model if needed
                PrepModel(model);

                // Get the proxy public Key
                await ProxyPublicKey(model);

                // Get the screen from proxy
                return await ProxyPDF(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> GetPDF </summary>
        /// <returns>URL string</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> GetPDFFileName(ProxyTesterViewModel model)
        {
            try
            {
                // Prep model if needed
                PrepModel(model);

                // Get the proxy public Key
                await ProxyPublicKey(model);

                // Get the screen from proxy
                return await ProxyPDFFileName(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        /// <summary> Routine to get the Public Key from the Proxy </summary>
        /// <param name="model">View Model</param>
        private async Task ProxyPublicKey(ProxyTesterViewModel model)
        {
            try
            {
                // Public key returned from Proxy
                model.ProxyPublicKey = string.Empty;

                // Request
                var request = new HttpRequestMessage(HttpMethod.Get, $"{model.TargetServer}/ProxyPublicKey?productId={model.ProductId}");

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to get the menu for the requested module from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Source to model</returns>
        private async Task ProxyMenu(ProxyTesterViewModel model)
        {
            try
            {
                // Request
                var request = new HttpRequestMessage(HttpMethod.Get, $"{model.TargetServer}/ProxyMenu?productId={model.ProductId}");

                // Encryption for headers
                EncryptItems(model);

                // Add headers
                request.Headers.Add(RequestHeader.Credentials, model.Credentials);
                request.Headers.Add(RequestHeader.ClientPublicKey, model.ClientPublicKey);
                request.Headers.Add(RequestHeader.ServerPublicKey, model.ProxyPublicKey);
                request.Headers.Add(RequestHeader.IV, model.IV);
                request.Headers.Add(RequestHeader.ProductId, model.ProductId);
                request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);
                request.Headers.Add(RequestHeader.Id, model.Id);

                // Await response
                using (var response = await httpClient.SendAsync(request))
                {
                    // If successful get the menu payload
                    if (response.IsSuccessStatusCode)
                    {
                        // Assign menu payload to model
                        model.Source = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to get the screen from the Proxy </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Source/Redirect to model</returns>
        private async Task<ActionResult> ProxyScreen(ProxyTesterViewModel model)
        {
            try
            {
                // Validations here

                // Request
                var request = new HttpRequestMessage(HttpMethod.Get, $"{model.TargetServer}/ProxyScreen?productId={model.ProductId}");

                // Encryption for headers
                EncryptItems(model);

                // Add headers
                request.Headers.Add(RequestHeader.Credentials, model.Credentials);
                request.Headers.Add(RequestHeader.ClientPublicKey, model.ClientPublicKey);
                request.Headers.Add(RequestHeader.ServerPublicKey, model.ProxyPublicKey);
                request.Headers.Add(RequestHeader.IV, model.IV);
                request.Headers.Add(RequestHeader.ProductId, model.ProductId);
                request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);
                request.Headers.Add(RequestHeader.Controller, model.Controller);
                request.Headers.Add(RequestHeader.Action, model.Action);
                request.Headers.Add(RequestHeader.OptionalParameters, model.OptionalParameters);
                request.Headers.Add(RequestHeader.Id, model.Id);

                // Await response
                using (var response = await httpClient.SendAsync(request))
                {
                    // If successful get the screen redirect
                    if (response.IsSuccessStatusCode)
                    {
                        // model.Source = response.RequestMessage.RequestUri.AbsoluteUri;
                        var screenResponse = await response.Content.ReadAsStringAsync();

                        // Get the session id and the context token
                        var json = screenResponse.Split('|');
                        var sessionId = json[0].Replace("\"", "");
                        var contextToken = json[1].Replace("\"", "");

                        // Assign screen url to model
                        model.Source = BuildURL(model, sessionId, contextToken);
                    }
                }

                // Return content (URL) AND the source will be set to the iFrame in JavaScript with the success handler
                return Content(model.Source);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to get the pdf file </summary>
        /// <param name="model">View Model</param>
        /// <returns>Assign Source/Redirect to model</returns>
        private async Task<ActionResult> ProxyPDF(ProxyTesterViewModel model)
        {
            try
            {
                // Validations here

                // Request
                var request = new HttpRequestMessage(HttpMethod.Get, $"{model.TargetServer}/ProxyPDF?productId={model.ProductId}");

                // Encryption for headers
                EncryptItems(model);

                // Add headers
                request.Headers.Add(RequestHeader.Credentials, model.Credentials);
                request.Headers.Add(RequestHeader.ClientPublicKey, model.ClientPublicKey);
                request.Headers.Add(RequestHeader.ServerPublicKey, model.ProxyPublicKey);
                request.Headers.Add(RequestHeader.IV, model.IV);
                request.Headers.Add(RequestHeader.ProductId, model.ProductId);
                request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);
                request.Headers.Add(RequestHeader.Controller, model.Controller);
                request.Headers.Add(RequestHeader.Action, model.Action);
                request.Headers.Add(RequestHeader.OptionalParameters, model.OptionalParameters);
                request.Headers.Add(RequestHeader.Id, model.Id);
                request.Headers.Add(RequestHeader.PdfName, model.PdfFileName);

                // Await response
                using (var response = await httpClient.SendAsync(request))
                {
                    // If successful get the screen redirect
                    if (response.IsSuccessStatusCode)
                    {
                        var streamContent = response.Content as StreamContent;
                        if (streamContent != null){
                            var fileName = streamContent.Headers?.ContentDisposition?.FileName;
                            var bytes = await streamContent.ReadAsByteArrayAsync();
                            var tempFilePath = System.IO.Path.Combine(Server.MapPath("~"), "PDFs", fileName);
                            System.IO.File.WriteAllBytes(tempFilePath, bytes);
                            return Content(fileName);
                        }
                    }
                }
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to get the file names of given date </summary>
        /// <param name="model">View Model</param>
        /// <returns>List of file names</returns>
        private async Task<ActionResult> ProxyPDFFileName(ProxyTesterViewModel model)
        {
            try
            {
                // Validations here

                // Request
                var request = new HttpRequestMessage(HttpMethod.Get, $"{model.TargetServer}/ProxyPDFFileName?productId={model.ProductId}");

                // Encryption for headers
                EncryptItems(model);

                // Add headers
                request.Headers.Add(RequestHeader.Credentials, model.Credentials);
                request.Headers.Add(RequestHeader.ClientPublicKey, model.ClientPublicKey);
                request.Headers.Add(RequestHeader.ServerPublicKey, model.ProxyPublicKey);
                request.Headers.Add(RequestHeader.IV, model.IV);
                request.Headers.Add(RequestHeader.ProductId, model.ProductId);
                request.Headers.Add(RequestHeader.ModuleId, model.ModuleId);
                request.Headers.Add(RequestHeader.Controller, model.Controller);
                request.Headers.Add(RequestHeader.Action, model.Action);
                request.Headers.Add(RequestHeader.OptionalParameters, model.OptionalParameters);
                request.Headers.Add(RequestHeader.Id, model.Id);
                request.Headers.Add(RequestHeader.PdfDate, model.PdfFileDate);

                // Await response
                using (var response = await httpClient.SendAsync(request))
                {
                    // If successful get the screen redirect
                    if (response.IsSuccessStatusCode)
                    {
                        var stringContent = await response.Content.ReadAsStringAsync();
                        if (stringContent != null)
                        {
                            var fileNames = JsonConvert.DeserializeObject<string[]>(stringContent);
                            return Content(string.Join(",", fileNames));
                        }
                    }
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to encrypt items </summary>
        /// <param name="model">View Model</param>
        private void EncryptItems(ProxyTesterViewModel model)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Routine to Prep model, if needed </summary>
        /// <param name="model">View Model</param>
        private void PrepModel(ProxyTesterViewModel model)
        {
            try
            {
                // Force case
                model.User = model.User.ToUpper();
                model.Company = model.Company.ToUpper();
                model.ModuleId = model.ModuleId.ToUpper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> Build the url </summary>
        /// <param name="model">View Model</param>
        /// <param name="sessionId">Session Id</param>
        /// <param name="contextToken">Context Token</param>
        /// <returns>The proxy url to get a screen</returns>
        private string BuildURL(ProxyTesterViewModel model, string sessionId, string contextToken)
        {
            try
            {
                var url = $@"{model.TargetServer}/OnPremise/{sessionId}/{model.ModuleId}/{model.Controller}/{model.Action}" +
                    (string.IsNullOrEmpty(model.OptionalParameters) ? string.Empty : $@"?{model.OptionalParameters}");

                // Break the url down into constituant parts
                var uri = new Uri(url);
                var queryParts = uri.ParseQueryString();
                var queryStringOperator = (queryParts.Count == 0) ? "?" : "&";

                // Build the final url and return
                url += $"{queryStringOperator}productId={model.ProductId}&ContextToken={contextToken}&{RequestHeader.Id}={model.Id}";

                return url;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}