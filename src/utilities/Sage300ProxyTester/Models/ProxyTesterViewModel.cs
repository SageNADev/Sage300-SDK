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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.WindowsRuntime;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Models
{
    /// <summary> ProxyTesterViewModel </summary>
    public class ProxyTesterViewModel
    {
        #region Class Constants
        public class ClassConstants
        {
            public const string URL_PLACEHOLDER = "Current Request URL displayed here.";
        }
        #endregion

        #region Constructor(s)
        public ProxyTesterViewModel()
        {
            // Authentication Settings
            User = "ADMIN";
            Password = "";
            Company = "SAMLTD";

            // Server Configuration
            Server = "http://localhost:54445/Sage300";

            // Proxy Request
            ModuleId = "";
            Controller = "";
            Action = "";
            OptionalParameters = string.Empty;

            // Results
            MenuUrl = string.Empty;
            ScreenUrl = string.Empty;
            IsValidTokenUrl = string.Empty;
            PublicKeyUrl = string.Empty;
            LoginUrl = string.Empty;

            // Test Action
            TestAction = string.Empty;

            // Login Token
            Token = string.Empty;
            IsValidToken = false;

            // Source for iFrame
            Source = string.Empty;
        }
        #endregion

        #region Properties

        // Authentication

        /// <summary> The user </summary>
        [Required(ErrorMessage = "User is a required field.")]
        public string User { get; set; }

        /// <summary> The password </summary>
        [Required(ErrorMessage = "Password is a required field.")]
        public string Password { get; set; }

        /// <summary> The company </summary>
        [Required(ErrorMessage = "Company is a required field.")]
        public string Company { get; set; }

        // Server Configuration

        /// <summary> The server name </summary>
        [Required(ErrorMessage = "Server name is a required field.")]
        public string Server { get; set; }

        // Proxy Request

        /// <summary> The module id </summary>
        [Required]
        [StringLength(2, ErrorMessage="The Module ID cannot exceed 2 characters.")]
        public string ModuleId { get; set; }

        /// <summary> The Controller </summary>
        public string Controller { get; set; }

        /// <summary> The Action </summary>
        public string Action { get; set; }

        /// <summary> Optional request parameters </summary>
        public string OptionalParameters{ get; set; }

        /// <summary> Url to get a public key </summary>
        public string PublicKeyUrl { get; set; }

        /// <summary> Url to login </summary>
        public string LoginUrl { get; set; }

        /// <summary> The encrypted User, Password, and Company in the request header </summary>
        public string Credentials { get; set; }

        /// <summary> The client's public key </summary>
        public string ClientPublicKey { get; set; }

        /// <summary> The proxy's public key </summary>
        public string ProxyPublicKey { get; set; }

        /// <summary> The IV in the request header </summary>
        public string IV { get; set; }

        /// <summary> Url to get a modules menu </summary>
        public string MenuUrl { get; set; }

        /// <summary> Url to determine if token is valid </summary>
        public string IsValidTokenUrl { get; set; }

        /// <summary>Url to get a web screen </summary>
        public string ScreenUrl { get; set; }

        /// <summary>Test Action </summary>
        public string TestAction { get; set; }

        /// <summary>Token (from ProxyLogin) </summary>
        public string Token { get; set; }

        /// <summary>IsValidToken (from ProxyIsValidToken) </summary>
        public bool IsValidToken { get; set; }

        /// <summary>Source for iFrame from ProxyMenu or ProxyScreen </summary>
        public string Source { get; set; }
        #endregion
    }
}