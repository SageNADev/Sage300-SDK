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

using System;
using System.Web.Mvc;
using System.Linq;

using Microsoft.Web.Administration;
using Microsoft.Practices.Unity;
using System.Data.Entity;

using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Bootstrap;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Attributes;

using ISV1.web.Areas.CU.Models;
using ISV1.web.Areas.CU.DAL.CustomViews;
using ISV1.web.Areas.CU.DAL.CustomViews.Repository;
using ISV1.web.Areas.CU.DAL.CustomViews.Model;
using ISV1.web.Areas.CU.DAL.CustomViews.ViewModel;

using ISV1.web.Areas.CU.DAL.CSQuery.Repository;
using ISV1.web.Areas.CU.DAL.CSQuery.Model;
using ISV1.web.Areas.CU.DAL.CSQuery.ViewModel;

using ISV1.web.Areas.CU.DAL.ADO_EF.Model;
using ISV1.web.Areas.CU.DAL.ADO_EF.Repository;
using ISV1.web.Areas.CU.DAL.ADO_EF.ViewModel;

using Sage.CA.SBS.ERP.Sage300.AP.Models;
using Sage.CA.SBS.ERP.Sage300.AP.Services;
using Sage.CA.SBS.ERP.Sage300.AP.Web;
using Sage.CA.SBS.ERP.Sage300.AP.Web.Controllers;

namespace ISV1.web.Areas.CU.Controllers
{
    /// <summary>
    /// Controller needs to be registered in CUWebBootstrapper
    /// TODO: Add more custom controller actions
    /// </summary>
    public class ISV1CustomizationController<T> : MultitenantControllerBase<CustomerViewModel<T>>
        where T : Customer, new()
    {
        public ISV1CustomizationController()
        {
        }


        /// <summary>
        /// Default index view
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            ViewBag.Sage300Url = "http://localhost/Sage300";
            return View();
        }

        #region Action methods using Sage 300 View

        /// <summary>
        /// Get list batch number by using Sage 300c services and endpoints
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllBySage300c()
        {
            SetUnityContainer(Context);

            //using Sage300c service to get data
            var apService = new InvoiceBatchListEntityService<InvoiceBatch>(Context);
            var modelData = apService.Get(0, 50);
            var batchNumberList = modelData.Items.Select(i => i.BatchNumber);
 
            return JsonNet(batchNumberList);
        }

        /// <summary>
        /// Get entity by using Sage300c service
        /// </summary>
        /// <param name="id">customer number</param>
        /// <returns>Customer</returns>
        [HttpGet]
        public virtual JsonNetResult GetBySage300c(string id)
        {
            SetUnityContainer(Context);

            //using Sage300c service to get data
            var apService = new InvoiceBatchListEntityService<InvoiceBatch>(Context);
            var modelData = apService.GetById(id);
            var viewModel = new ViewModelBase<InvoiceBatch>();
            viewModel.Data = modelData;

            return JsonNet(viewModel);
        }

        /// <summary>
        /// Save/Update entity by using Sage300c/Custom view
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveBySage300c(Customer model)
        {
            SetUnityContainer(Context);
            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new CustomerRepository<Customer>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }

        /// <summary>
        /// Delete entity by using Sage300c/Custom view 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteBySage300c(string id)
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion Call Sage 300 Controller Actions

        #region Action Methods using Sage 300 CSQuery

        /// <summary>
        /// Get Entity using Sage300 CS Query 
        /// </summary>
        /// <param name="id">entity id or key </param>
        /// <returns></returns>
        public virtual JsonNetResult GetBySage300CSQuery(string id)
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            var modelData = repository.GetById(id);

            // Set view model fields that required by UI
            var customerViewModel = new OrderViewModel<Order>();
            customerViewModel.Data = (Order)modelData;
            customerViewModel.OrderCurrencyCode = "Canadian Dollar";
            customerViewModel.UserMessage = new UserMessage(modelData);

            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Get All Entity Keys using Sage300 CS Query 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllBySage300CSQuery()
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            var modelData = repository.GetAll();

            return JsonNet(modelData);
        }

        /// <summary>
        /// Save/Update Entity using Sage300 CS Query
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveBySage300CSQuery(Order model)
        {
            SetUnityContainer(Context);

            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new OrderRepository<Order>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }
        /// <summary>
        /// Delete entity using Sage300 CS Query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteBySage300CSQuery(string id)
        {
            SetUnityContainer(Context);

            var repository = new OrderRepository<Order>(Context);
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion 

        #region Action Methods using Custom View

        /// <summary>
        /// Get all entities keys by using Custom view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllByCustomView()
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            var modelData = repository.GetAll();

            return JsonNet(modelData);
        }

        public virtual JsonNetResult GetByCustomView(string id)
        {
            SetUnityContainer(Context);

            var repository = new CustomerRepository<Customer>(Context);
            var modelData = repository.GetById(id);

            // Set view model fields that required by UI
            var customerViewModel = new CustomerViewModel<T>();
            customerViewModel.Data = (T)modelData;
            customerViewModel.CurrencyCodeDescription = "Canadian Dollar";
            customerViewModel.IsMultiCurrency = true;
            customerViewModel.UserMessage = new UserMessage(modelData);


            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Save entity by using Custom View 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveByCustomView(T model)
        {
            SetUnityContainer(Context);

            ViewModelBase<ModelBase> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }
            var repository = new CustomerRepository<Customer>(Context);

            try
            {
                return JsonNet(repository.Save(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "Customer"));
            }
        }

        /// <summary>
        /// Delete entity by using Custom View 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteByCustomView(string id)
        {
            SetUnityContainer(Context);
 
            var repository = new CustomerRepository<Customer>(Context);
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }

        }

        #endregion 

        #region Action Methods using Entity Framework to Acccess DB
        
        /// <summary>
        /// Get model data by id using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetByEntityFramework(string id)
        {
            //using generic reposity to get data
            var custRepository = new GenericRepository<ARCustomer>();
            var custData = custRepository.GetByID(id);
            var custOptRepository = new GenericRepository<ARCustomerOptionalField>();
            var custOptData = custOptRepository.GetListByFilter(e => e.CustomerNumber == id);
            custData.ARCustomerOptionalFields = custOptData.ToList();
            
            //using Entity Framework API to get Data
            using(var ctx = new CustomDbContext())
            {
                 var data = ctx.ARCustomer.Where(c => c.CustomerNumber == id).Include(e => e.ARCustomerOptionalFields);
            }

            // Set View Model Fields that UI needs
            var customerViewModel = new ARCustomerViewModel<ARCustomer>();
            customerViewModel.Data = (ARCustomer)custData;
            customerViewModel.CurrencyCodeDescription = "Canadian Dollar";

            return JsonNet(customerViewModel);
        }

        /// <summary>
        /// Get All Entities keys using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual JsonNetResult GetAllByEntityFramework()
        {
            var repository = new GenericRepository<ARCustomer>();
            var modelData = repository.GetAll().Select(r => r.CustomerNumber.Trim());
            return JsonNet(modelData);
        }

        /// <summary>
        /// Add Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult AddByEntityFramework(ARCustomer model)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Insert(model);
                return JsonNet("Added successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, "ARCustomer"));
            }
        }

        /// <summary>
        /// Save/Update Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult SaveByEntityFramework(ARCustomer model)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Update(model);
                return JsonNet("Save successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, "ARCustomer"));
            }
        }

        /// <summary>
        /// Delete Entity using Entity Framework direcly access SQL server DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoAntiForgeryCheckAttribute]
        public virtual JsonNetResult DeleteByEntityFramework(string id)
        {
            var repository = new GenericRepository<ARCustomer>();
            try
            {
                repository.Delete(id);
                return JsonNet("Delete successfully !");
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, "Customer"));
            }
        }

        #endregion 

        /// <summary>
        /// Set unity container for integrated with sage 300c 
        /// </summary>
        /// <param name="Context"></param>
        private void SetUnityContainer(Context Context)
        {
            if (Context != null && Context.Container == null)
            {
                Context.Container = BootstrapTaskManager.Container;
            }
        }
    }

}