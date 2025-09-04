/* Copyright (c) $year$ $companyname$.  All rights reserved. */

using Unity;
using Unity.Injection;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Controller;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using $safeprojectname$.Areas.$module$.Controllers;

namespace $safeprojectname$
{
    /// <summary>
    /// $module$ Bootstrapper Class
    /// </summary>
    [Export(typeof(IBootstrapperTask))]
    [BootstrapMetadataExport("$module$", new[] { BootstrapAppliesTo.Web }, 20)]
    public class $module$WebBootstrapper : IBootstrapperTask
    {
        /// <summary>
        /// Bootstrap activity execution
        /// </summary>
        /// <param name="container">The Unity container</param>
        public void Execute(IUnityContainer container)
        {
            RegisterController(container);
            RegisterFinder(container);
        }

        /// <summary>
        /// Register controllers
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterController(IUnityContainer container)
        {
            UnityUtil.RegisterType<IController, $project$CustomizationController>(container, "$module$$project$Customization");

        }

        /// <summary>
        /// Register finders
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterFinder(IUnityContainer container)
        {
        }

    }
}
