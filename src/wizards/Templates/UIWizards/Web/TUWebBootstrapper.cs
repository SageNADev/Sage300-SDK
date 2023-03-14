/* Copyright (c) $year$ $copyright$  All rights reserved. */

using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Controller;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System.ComponentModel.Composition;
using System.Web.Mvc;

using Constants=$companynamespace$.$applicationid$.Web.Areas.$applicationid$.Constants;

namespace $companynamespace$.$applicationid$.Web
{
    /// <summary>
    /// $applicationid$ Bootstrapper Class
    /// </summary>
    [Export(typeof(IBootstrapperTask))]
    [BootstrapMetadataExport("$applicationid$", new[] { BootstrapAppliesTo.Web }, 20)]
    public class $applicationid$WebBootstrapper : IBootstrapperTask
    {
        /// <summary>
        /// Bootstrap activity execution
        /// </summary>
        /// <param name="container">The Unity container</param>
        public void Execute(IUnityContainer container)
        {
            RegisterController(container);
            RegisterExportImportController(container);
        }
        
        /// <summary>
        /// Register controllers
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterController(IUnityContainer container)
        {
        }

        /// <summary>
        /// Register import/export controllers
        /// </summary>
        /// <param name="container">The Unity container</param>
        private void RegisterExportImportController(IUnityContainer container)
        {
        }
    }
}
