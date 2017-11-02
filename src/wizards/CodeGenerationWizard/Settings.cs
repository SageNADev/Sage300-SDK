// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary> Settings class to hold info UI Settings </summary>
    [System.SerializableAttribute]
    public class Settings
    {
        #region Constructor
        /// <summary> Constructor setting defaults </summary>
        public Settings()
        {
            Entities = new List<BusinessView>();
            RepositoryType = RepositoryType.Flat;
            XmlEntities = new XDocument();
        }
        #endregion

        #region Public Properties
        /// <summary> User for Business View </summary>
        public string User { get; set; }
        /// <summary> Password for Business View </summary>
        public string Password { get; set; }
        /// <summary> Version for Business View </summary>
        public string Version { get; set; }
        /// <summary> Company for Business View </summary>
        public string Company { get; set; }
        /// <summary> RepositoryType for Business View </summary>
        public RepositoryType RepositoryType { get; set; }
        /// <summary> Business Views (Entities) </summary>
        public List<BusinessView> Entities { get; set; }
        /// <summary> XDocument of Business Views (Entities) </summary>
        public XDocument XmlEntities { get; set; }
        /// <summary> Prompt if Exists </summary>
        public bool PromptIfExists { get; set; }
        /// <summary> Module ID </summary>
        public string ModuleId { get; set; }
        /// <summary> Projects in solution </summary>
        public Dictionary<string, Dictionary<string, ProjectInfo>> Projects { get; set; }
        /// <summary> Copyright </summary>
        public string Copyright { get; set; }
        /// <summary> Company Namespace </summary>
        public string CompanyNamespace { get; set; }
        /// <summary> Extension - .Process, .Reports or string.Empty </summary>
        public string Extension { get; set; }
        /// <summary> Resource Extension - .Process, .Reports or .Forms </summary>
        public string ResourceExtension { get; set; }
        /// <summary> Area Structure for Web - True if Areas exist otherwise false </summary>
        public bool DoesAreasExist { get; set; }
        /// <summary> Enumeration Helper </summary>
        public EnumHelper EnumHelper { get; set; }
        /// <summary> Resource keys </summary>
        public List<string> ResourceKeys { get; set; }
        /// <summary> Web Project Includes Module </summary>
        public bool WebProjectIncludesModule { get; set; }
        /// <summary> Include English </summary>
        public bool includeEnglish { get; set; }
        /// <summary> Include Chinese Simplified </summary>
        public bool includeChineseSimplified { get; set; }
        /// <summary> Include Chinese Traditional </summary>
        public bool includeChineseTraditional { get; set; }
        /// <summary> Include Spanish </summary>
        public bool includeSpanish { get; set; }
        /// <summary> Include French </summary>
        public bool includeFrench { get; set; }
        /// <summary> Entities Container Name </summary>
        public string EntitiesContainerName { get; set; }
        #endregion
    }

}
