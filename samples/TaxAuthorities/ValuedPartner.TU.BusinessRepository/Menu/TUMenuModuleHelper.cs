
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

using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Menu;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository.Menu;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ValuedPartner.BusinessRepository.Menu
{
    /// <summary>
    /// TU Menu Module helper
    /// </summary>
    [Export(typeof(IMenuModuleHelper))]
    [BootstrapMetadataExport("TU", new[] { BootstrapAppliesTo.Web }, 10)]
    class TUMenuModuleHelper : AbstractMenuModuleHelper
    {
        /// <summary>
        /// Return Module specified
        /// </summary>
        public override string Module
        {
            get { return "TU"; }
        }

        /// <summary>
        /// Return Name of the screen file
        /// </summary>
        public override string MenuDetailFileName
        {
            get { return "TUMenuDetails.xml"; }
        }

        /// <summary>
        /// Return MultiCurrency status
        /// </summary>
        public bool IsMultiCurrency { get; private set; }

        /// <summary>
        /// Initialize the module manager
        /// </summary>
        /// <param name="company">string</param>
        /// <param name="context">Context</param>
        public override void Initialize(string company, Context context)
        {
            PrepareDataFile(company);
        }

        /// <summary>
        /// Return Menu Items with activation filter applied
        /// </summary>
        /// <returns>List</returns>
        public override List<NavigableMenu> GetFilteredMenuItems()
        {
            var criteriaList = new List<Func<NavigableMenu, bool>>();
            return GetApplyFilteredMenuItems(criteriaList);
        }


        /// <summary>
        /// Flag to indicate whether the menu is third party development menu
        /// </summary>
        public override bool IsPlugInMenu
        {
            get { return true; }
        }
    }
}
