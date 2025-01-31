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
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Models
{
    /// <summary> ProxyTesterViewModel </summary>
    public class ProxyTesterViewModel
    {
        #region Constructor(s)
        public ProxyTesterViewModel()
        {
            // Default Authentication Settings
            User = "ADMIN"; // Partner to specify the default user if desired
            Password = "";  // Partner to specify the default password if desired
            Company = "SAMINC"; // Partner to specify the default company if desired

            // Default Server Configurations - Partner to specify the target Sage server and this ProxyTester server
            TargetServer = ""; // Partner to specify the target Sage server (i.e. http://localhost:54445 or http://localhost/Sage300)
            ProxyTesterServer = ""; // Partner to specify this ProxyTester server (i.e. https://localhost:44347)

            // Proxy Request
            ModuleId = ""; // Partner to specify the default ModuleId if desired
            Controller = ""; // Partner to specify the default Controller if desired
            Action = ""; // Partner to specify the default Action if desired
            OptionalParameters = string.Empty; // Partner to specify the default OptionalParameters if desired
            ProductId = "PROXY"; // DO NOT CHANGE THIS VALUE

            // Source for iFrame
            Source = string.Empty; // Value for the iFrame source

            // Id for ProxyManager uniqueness
            Id = Guid.NewGuid().ToString(); // Per session and not per request
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

        // Server Configurations

        /// <summary> The target server name </summary>
        public string TargetServer { get; set; }

        /// <summary> The Proxy Tester server name </summary>
        public string ProxyTesterServer { get; set; }

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

        /// <summary> Product Id </summary>
        public string ProductId { get; set; }

        /// <summary> The encrypted User, Password, and Company in the request header </summary>
        public string Credentials { get; set; }

        /// <summary> The client's public key </summary>
        public string ClientPublicKey { get; set; }

        /// <summary> The proxy's public key </summary>
        public string ProxyPublicKey { get; set; }

        /// <summary> The IV in the request header </summary>
        public string IV { get; set; }

        /// <summary>Source for iFrame from ProxyMenu or ProxyScreen </summary>
        public string Source { get; set; }

        /// <summary> Id (Guid) to make key unique in ProxyManager</summary>
        public string Id { get; set; }

        #endregion
    }
}