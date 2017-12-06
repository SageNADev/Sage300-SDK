/* Copyright (c) 2017 $companyname$.  All rights reserved. */

using System;
using System.Web.Mvc;
using System.Linq;
using $safeprojectname$.Areas.$module$.Models;
using Microsoft.Web.Administration;

namespace $safeprojectname$.Areas.$module$.Controllers
{
    /// <summary>
    /// Controller needs to be registered in $module$WebBootstrapper
	/// TODO: add custom controller actions
    /// </summary>
    public class $project$CustomizationController : Controller
    {
        public virtual ActionResult Index()
        {
            ViewBag.Sage300Url = "http://localhost/Sage300";
            return View();
        }
    }
}