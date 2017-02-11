/* Copyright (c) 2016 ISV1.  All rights reserved. */

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
