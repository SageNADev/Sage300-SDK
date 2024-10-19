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

using System.Collections.Generic;

namespace Sage.CA.SBS.ERP.Sage300.CodeGenerationWizard
{
    /// <summary>
    /// This class holds the attributes scraped from the Sage views. This helps
    /// the generation of the controllers and models
    /// </summary>
    public class ControllerSettings
    {
        /// <summary>
        /// Web API version
        /// </summary>
        public string ApiVersion { get; set; }

         /// <summary>
        /// The name of of the view (e.g. ARCUS)
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Specifies whether the controller/endpoint should be generated for this view
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// The view ID of the view (e.g. AR0032)
        /// </summary>
        public string ViewId { get; set; }

        public BusinessView BusinessView { get; set; }

        /// <summary>
        /// The view type
        /// </summary>
        public ViewProtocolType ViewProtocolType { get; set; }

        /// <summary>
        /// The property names in the generated model that comprises the primary key for the view
        /// </summary>
        public List<string> KeyProperties { get; set; }

        /// <summary>
        /// Whether the key is ordered or revisioned
        /// </summary>
        public ViewKeyType KeyType { get; set; }
        
        /// <summary>
        /// If this is a detail view, the PropertyName is used for the property that
        /// holds a list of this view's records in the header model
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The generated string used for the Model name
        /// </summary>
        public string ModelName { get; set; }

        public List<ControllerSettings> Details { get; set; }

        public List<string> ReferencedAppModules { get; set; }

        /// <summary> supported verbs </summary>
        public string Verbs { get; set; }

        /// <summary> supported verbs </summary>
        public string Extension { get; set; }


    }
}
