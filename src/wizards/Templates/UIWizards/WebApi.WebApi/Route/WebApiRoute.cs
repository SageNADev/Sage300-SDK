/* Copyright (c) $year$ $copyright$. All rights reserved. */

using System.Web.Http;
using Sage.CA.SBS.ERP.Sage300.Common.WebApi.Interfaces;
using Sage.CA.SBS.ERP.Sage300.Common.WebApi.Versioning;

namespace $safeprojectname$.Route
{
    /// <summary>
    /// Construct module's OData route.
    /// </summary>
    public class WebApiRoute : IWebApiRoute
    {
        /// <summary>
        /// Module Id for this route.
        /// </summary>
        private const string ModuleId = "$applicationid$";

        /// <summary>
        /// Create route for this module.
        /// </summary>
        /// <param name="config">HttpConfiguration instance.</param>
        /// <returns><see cref="DefaultSageVersionedRouteBuilder"/></returns>
        public static DefaultSageVersionedRouteBuilder GetRoute(HttpConfiguration config)
        {
            return VersionedRouteBuilderFactory
                    .GetSage300DefaultVersionedRouteBuilder(ModuleId, config);
        }
    }
}
