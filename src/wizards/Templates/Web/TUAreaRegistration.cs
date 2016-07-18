/* Copyright (c) $year$ $companyname$.  All rights reserved. */

using System.Web.Mvc;
using System.Web.Optimization;

namespace $companynamespace$.Web
{
    /// <summary>
    /// Class $applicationid$AreaRegistration.
    /// </summary>
    public class $applicationid$AreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        /// <value>The name of the area.</value>
        /// <returns>The name of the area to register.</returns>
        public override string AreaName
        {
            get { return "$applicationid$"; }
        }

        /// <summary>
        /// Registers Area
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterRoutes(context);
            RegisterBundles();
        }

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="context">The context.</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute("$applicationid$_Tenant", "{tenantAlias}/$applicationid$/{controller}/{action}/{id}",
            new { action = "Index", id = UrlParameter.Optional });
        }

        /// <summary>
        /// Register bundles
        /// </summary>
        private void RegisterBundles()
        {
            BundleRegistration.RegisterBundles(BundleTable.Bundles);
        }
    }
}