/* Copyright (c) 2017 $companyname$.  All rights reserved. */

using System.Web.Mvc;
using System.Web.Optimization;

namespace $safeprojectname$
{
    /// <summary>
    /// Class $module$AreaRegistration.
    /// </summary>
    public class $module$AreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        /// <value>The name of the area.</value>
        /// <returns>The name of the area to register.</returns>
        public override string AreaName
        {
            get { return "$module$"; }
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
            context.MapRoute("$module$_Tenant", "{tenantAlias}/$module$/{controller}/{action}/{id}",
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