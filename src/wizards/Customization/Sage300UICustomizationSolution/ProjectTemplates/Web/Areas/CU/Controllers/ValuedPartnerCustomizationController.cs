/* Copyright (c) $year$ $companyname$.  All rights reserved. */

using System;
using System.Web.Mvc;
using System.Linq;
using $safeprojectname$.Areas.$module$.Models;
using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Web;

namespace $safeprojectname$.Areas.$module$.Controllers
{
    /// <summary>
    /// Controller needs to be registered in $module$WebBootstrapper
	/// TODO: add custom controller actions
    /// </summary>
    public class $project$CustomizationController : MultitenantControllerBase<$project$CustomizationViewModel>
    {
        /// <summary>
        /// Constructor with container parameter
        /// </summary>
        /// <param name="container">The unity container</param>
        public $project$CustomizationController(IUnityContainer container)
            : base(container, "UniqueScreenName")
        {
        }

        /// <summary>
        /// Index action method
        /// </summary>
        public virtual ActionResult Index()
        {
            ViewBag.Sage300Url = "http://localhost/Sage300";
            return View();
        }
    }
}