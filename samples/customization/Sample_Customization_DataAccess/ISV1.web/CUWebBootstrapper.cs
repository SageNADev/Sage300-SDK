// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Controller;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using ISV1.web.Areas.CU.Controllers;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Landlord;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Landlord;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Landlord;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service.Landlord;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Web;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using ISV1.web.Areas.CU.DAL.SageViews.Model;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Audit;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;

namespace ISV1.web
{
    /// <summary>
    /// CU Bootstrapper Class
    /// </summary>
    [Export(typeof(IBootstrapperTask))]
    [BootstrapMetadataExport("CU", new[] { BootstrapAppliesTo.Web }, 20)]
    public class CUWebBootstrapper : IBootstrapperTask
    {
        /// <summary>
        /// Bootstrap activity execution
        /// </summary>
        /// <param name="container">The Unity container</param>
        public void Execute(IUnityContainer container)
        {
            RegisterController(container);
            RegisterFinder(container);
            RegisterLandlordRepository(container);
        }

        /// <summary>
        /// Register controllers
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterController(IUnityContainer container)
        {
            UnityUtil.RegisterType<IController, ISV1CustomizationController<Customer>>(container, "CUISV1Customization");

        }

        /// <summary>
        /// Register finders
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterFinder(IUnityContainer container)
        {
        }

        private void RegisterLandlordRepository(IUnityContainer container)
        {
            //UnityUtil.RegisterType(container, typeof(ISession), typeof(SessionEntity));
            UnityUtil.RegisterType(container, typeof(IBusinessEntity), typeof(BusinessEntity));
            UnityUtil.RegisterType(container, typeof(ISessionCacheProvider), typeof(SessionProvider));
            UnityUtil.RegisterType<ILandlordService, LandlordService>(container);
            UnityUtil.RegisterType<ILandlordRepository, LandlordRepositoryOnPremise>(container);

        }
    }
}
