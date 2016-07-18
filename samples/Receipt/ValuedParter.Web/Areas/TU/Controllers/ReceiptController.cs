// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

#region Namespace

using Microsoft.Practices.Unity;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using ValuedParter.TU.Models;
using ValuedParter.TU.Resources.Forms;
using ValuedParter.Web.Areas.TU.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;

#endregion

namespace ValuedParter.Web.Areas.TU.Controllers
{
    /// <summary>
    ///  Controller for Receipt view
    /// </summary>
    /// <typeparam name="T">Receipt Header</typeparam>
    /// <typeparam name="TU">Receipt Detail</typeparam>
    /// <typeparam name="TDetail2">Receipt Optional Field</typeparam>
    /// <typeparam name="TDetail3">Receipt Detail Optional Field</typeparam>
    /// <typeparam name="TDetail4">Receipt Detail LotNumber</typeparam>
    /// <typeparam name="TDetail5">Receipt Detail SerialNumber</typeparam>
    public class ReceiptController<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> :
        MultitenantControllerBase<ReceiptViewModel<T>> where T : ReceiptHeader, new()
        where TU : ReceiptDetail, new()
        where TDetail2 : ReceiptOptionalField, new()
        where TDetail3 : ReceiptDetailOptionalField, new()
        where TDetail4 : ReceiptDetailLotNumber, new()
        where TDetail5 : ReceiptDetailSerialNumber, new()
    {
        #region Private Variables

        /// <summary>
        /// Variable for controller internal.
        /// </summary>
        public ReceiptControllerInternal<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> ControllerInternal;
         

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Receipt Controller
        /// </summary>
        /// <param name="container"></param>
        public ReceiptController(IUnityContainer container) : base(container, ScreenName.ICReceipt)
        { 
        }

        #endregion

        #region Initialize MultitenantControllerBase

        /// <summary>
        /// Initializes the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ControllerInternal = new ReceiptControllerInternal<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>(Context);
        }

        #endregion

        #region Actions

        /// <summary>
        ///  Gets default Index Page
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Index(string id = null, bool disableAll = false)
        {
            ReceiptViewModel<T> receiptViewModel = !string.IsNullOrEmpty(id) ? ControllerInternal.GetById(id, false, disableAll) : ControllerInternal.Create(); 
            //Added this make the optionalFields uncheck in UI 
            receiptViewModel.Data.OptionalFields = 0;
            receiptViewModel.Attributes = ControllerInternal.GetDynamicAttributesOfHeader();
            receiptViewModel.UserAccess = ControllerInternal.GetAccessRights();
            ViewBag.UserAccess = ControllerInternal.GetAccessRights();
            receiptViewModel.DisableScreen = disableAll;
            return View(receiptViewModel);
        }

        /// <summary>
        /// Get receipt details
        /// </summary>
        /// <param name="id">receipt Number</param> 
        /// <param name="oldRecordDeleted">receipt Number</param> 
        /// <param name="isCalledAsPopup">receipt Number</param> 
        /// <returns>Receipt View Model</returns>
        [HttpPost]
        public virtual JsonNetResult GetById(string id, bool oldRecordDeleted = false,bool isCalledAsPopup=false)
        {
            try
            {
                return JsonNet(ControllerInternal.GetById(id, oldRecordDeleted, isCalledAsPopup));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        }

        /// <summary>
        /// Get paged receipt details
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="model">Receipt Model</param>
        /// <param name="filters">Filters</param>
        /// <returns>Receipt data</returns>
        [HttpPost]
        public virtual JsonNetResult GetPagedReceiptDetails(int pageNumber, int pageSize, T model, IList<IList<Sage.CA.SBS.ERP.Sage300.Common.Models.Filter>> filters)
        {
            try
            {
                var receiptDetail = ControllerInternal.GetPagedDetail(pageNumber, pageSize, model, filters);
                return JsonNet(receiptDetail);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        }

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        public JsonNetResult GetOptionalFieldFinderData(string optionalField)
        {
            try
            {
                return JsonNet(ControllerInternal.GetOptionalFieldFinderData(optionalField));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage2, businessException,
                        CommonResx.OptionalField));
            }
        }

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        public JsonNetResult GetDetailOptFieldFinderData(string optionalField)
        {
            try
            {
                return JsonNet(ControllerInternal.GetDetailOptFieldFinderData(optionalField));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage2, businessException,
                        CommonResx.OptionalField));
            }
        }

        /// <summary>
        /// Save detail
        /// </summary>
        /// <param name="detail">The detail model</param>
        /// <returns>returns  Receipt Viewmodel</returns>
        [HttpPost]
        public virtual JsonNetResult SaveDetail(TU detail)
        {
            try
            {
                return JsonNet(ControllerInternal.SaveDetail(detail));
            }
            catch (BusinessException businessException)
            {
                var receiptHeader = new ReceiptHeader { ReceiptDetail = new EnumerableResponse<ReceiptDetail> { Items = new List<ReceiptDetail> { detail } } };
                return JsonNet(BuildErrorModelBase(receiptHeader, CommonResx.RefreshDetailsFailedMessage, businessException, "ReceiptView"));
            }
        }

        /// <summary>
        /// Refresh the header
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>Refreshed header model</returns>
        public virtual JsonNetResult Refresh(T model)
        {
            try
            {
                return JsonNet(ControllerInternal.Refresh(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="model">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        [HttpPost]
        public virtual JsonNetResult SetOptionalFieldValue(TDetail3 model)
        {
            try
            {
                return JsonNet(ControllerInternal.SetOptionalFieldValue(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, CommonResx.OptionalField));
            }
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="model">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        [HttpPost]
        public virtual JsonNetResult SetHeaderOptFieldValue(TDetail2 model)
        {
            try
            {
                return JsonNet(ControllerInternal.SetOptionalFieldValue(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, CommonResx.OptionalField));
            }
        }


        /// <summary>
        /// Read Header Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="setHeaderValue"></param>
        /// <returns></returns>
        public virtual JsonNetResult ReadHeader(T model, bool setHeaderValue)
        {
            var response = new ViewModelBase<T> { UserMessage = new UserMessage { IsSuccess = true } };
            try
            {
                response.Data = ControllerInternal.ReadHeader(model, setHeaderValue);
                return JsonNet(response);
            }
            catch (BusinessException businessException)
            { 
                return JsonNet(BuildErrorModelBase(businessException));
            }
        }

        /// <summary>
        /// Save the details
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Receipt View model - that contains the info about the saved details</returns>
        [HttpPost]
        public virtual JsonNetResult SaveDetails(T model)
        {
            var response = new ViewModelBase<T> { UserMessage = new UserMessage { IsSuccess = true } };
            try
            { 
                return JsonNet(ControllerInternal.SaveDetails(model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.RefreshDetailsFailedMessage, businessException, "Receipt Detail"));
            }
        }

        #endregion

        #region Action Create Methods

        /// <summary>
        /// Create Receipt
        /// </summary>
        /// <returns>Receipt View Model</returns>
        [HttpPost]
        public virtual JsonNetResult Create()
        {
            try
            {
                return JsonNet(ControllerInternal.Create());
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Create Receipt Detail
        /// </summary>
        /// <param name="model">Receipt Model</param>
        /// <param name="index">Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageNumber">Page Number</param>
        /// <returns>Receipt Header</returns>
        [HttpPost]
        public virtual JsonNetResult CreateDetail(T model, int index, int pageSize, int pageNumber)
        {
            try
            {
                return JsonNet(ControllerInternal.CreateDetail(model, index, pageSize, pageNumber));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, "Receipt Number"));
            }
        }

        /// <summary>
        /// Delete Receipt Details
        /// </summary>
        /// <param name="model">Receipt Header model</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Receipt View Model</returns>
        [HttpPost]
        public virtual JsonNetResult DeleteDetails(T model, int pageNumber, int pageSize)
        {
            try
            {
                return JsonNet(ControllerInternal.DeleteDetails(model, pageNumber, pageSize));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Adds new Receipt
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>Receipt view model</returns>
        [HttpPost]
        public virtual JsonNetResult Add(T model)
        {
            ViewModelBase<ReceiptViewModel<T>> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }

            try
            {
                var receiptAdded = ControllerInternal.Add(model);
                return JsonNet(receiptAdded);
            }

            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.AddFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Check exchange Rate 
        /// </summary>
        /// <param name="rateType">rateType</param>
        /// <param name="fromCurrency">fromCurrency</param>
        /// <param name="rateDate">rateDate</param>
        /// <param name="rate">rate</param>
        /// <param name="toCurrency">toCurrency</param>
        /// <returns>Exchange rate</returns>
        public JsonNetResult CheckRateSpread(string rateType, string fromCurrency, DateTime rateDate, decimal rate,
            string toCurrency)
        {
            try
            {
                return
                    JsonNet(ControllerInternal.CheckRateSpread(rateType, fromCurrency, rateDate, rate, toCurrency));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, "Exchange Rate"));
            } 
        }

        public JsonNetResult GetVendorDetail(string vendorNumber) 
        {
            try
            {
                return JsonNet(ControllerInternal.GetVendorDetail(vendorNumber));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.VendorNumber));
            } 
        } 

        /// <summary>
        /// Check Bank Rate Date/Posting Date/Payment Date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public JsonNetResult ValidateDate(string date)
        {
            try
            {
                return JsonNet(ControllerInternal.ValidateDate(DateUtil.GetDate(date, DateUtil.GetMinDate())));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }


        }

        #endregion

        #region Action Save Methods

        /// <summary>
        /// Updates Receipt
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>Receipt</returns>
        [HttpPost]
        public virtual JsonNetResult Save(T model)
        {
            ViewModelBase<ReceiptViewModel<T>> viewModel;

            if (!ValidateModelState(ModelState, out viewModel))
            {
                return JsonNet(viewModel);
            }

            try
            {
                var receiptUpdated = ControllerInternal.Save(model);
                return JsonNet(receiptUpdated);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException));
            }
        }

        /// <summary>
        /// Post Receipt
        /// </summary>
        /// <param name="headerModel">Receipt Header model</param>
        /// <param name="sequenceNumber">Sequence Number</param>
        /// <param name="yesNo">Check Yes No confirmation selection</param>
        /// <returns>Receipt</returns>
        [HttpPost]
        public virtual JsonNetResult Post(T headerModel, long sequenceNumber, bool yesNo)
        {
            try
            {
                return JsonNet(ControllerInternal.Post(headerModel, sequenceNumber, yesNo));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.PostingFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        } 

        /// <summary>
        /// Save detail optional field.
        /// </summary>
        /// <param name="receiptOptionalField">Receipt Optional Field</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">True / False</param>
        /// <returns>Receipt Detail Optional Field</returns>
        [HttpPost]
        public virtual JsonNetResult SaveDetailOptFields(List<TDetail3> receiptOptionalField, string receiptNumber, bool isDetail)
        {
            try
            {
                return JsonNet(ControllerInternal.SaveOptionalFields(receiptOptionalField, receiptNumber, isDetail));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage2, businessException, CommonResx.OptionalField));
            }
        }

        /// <summary>
        /// Delete Optional Field  
        /// </summary>
        /// <param name="model">Enumerable Response of Receipts Optional Field</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable Response of Receipts Optional Field</returns>
        [HttpPost]
        public virtual JsonNetResult DeleteOptionalFields(EnumerableResponse<TDetail3> model, int pageNumber,
            int pageSize)
        {
            try
            {
                return JsonNet(ControllerInternal.DeleteOptionalFields(model, pageNumber, pageSize, false));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, CommonResx.OptionalField));
            }
        }

        /// <summary>
        /// Delete Optional Field  
        /// </summary>
        /// <param name="model">Enumerable Response of Receipts Detail Optional Field</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable Response of Receipts Detail Optional Field</returns>
        [HttpPost]
        public virtual JsonNetResult DeleteDetailOptFields(EnumerableResponse<TDetail3> model, int pageNumber,
            int pageSize)
        {
            try
            {
                return JsonNet(ControllerInternal.DeleteOptionalFields(model, pageNumber, pageSize, true));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, CommonResx.OptionalField));
            }
        }


        /// <summary>
        /// Gets Receipt Detail optional fields
        /// </summary>
        /// <param name="pageNumber">Current pageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="model">Receipt Detail OptionalField model</param>
        /// <returns>Receipt Detail OptionalFields</returns>
        [HttpPost]
        public virtual JsonNetResult GetDetailOptFields(int pageNumber, int pageSize, EnumerableResponse<TDetail3> model)
        {
            try
            {
                var detailOptionalFields = ControllerInternal.GetDetailOptFields(pageNumber, pageSize, model);
                return JsonNet(detailOptionalFields);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Gets Receipt optional fields.
        /// </summary>
        /// <param name="pageNumber">Current pageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="model">List of ReceiptOptionalField model</param>
        /// <returns>returns Enumerable Response of ReceiptOptionalField</returns>
        [HttpPost]
        public virtual JsonNetResult GetOptFields(int pageNumber, int pageSize, EnumerableResponse<TDetail3> model)
        {
            try
            {
                var receiptOptionalFields = ControllerInternal.GetOptFields(pageNumber, pageSize, model);
                return JsonNet(receiptOptionalFields);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Set detail to current row
        /// </summary>
        /// <param name="currentDetail">Current TU model</param>
        /// <returns>returns TU model</returns>
        [HttpPost]
        public virtual JsonNetResult SetDetail(TU currentDetail)
        {
            try
            {
                return JsonNet(ControllerInternal.SetDetail(currentDetail));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.SaveFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Refreshes the detail when the data is changed.
        /// </summary>
        /// <param name="model">Detail model</param>
        /// <param name="eventType">Type of event</param>
        /// <returns>Receipt View Model</returns>
        [HttpPost]
        public virtual JsonNetResult RefreshDetail(TU model, string eventType)
        {
            try
            {
                return JsonNet(ControllerInternal.RefreshDetail(model, eventType));
            }
            catch (BusinessException businessException)
            {
                return
                    JsonNet(BuildErrorModelBase(CommonResx.RefreshDetailsFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        }

        /// <summary>
        /// Refreshes the header optional fields.
        /// </summary>
        /// <returns>returns optional field value</returns>
        [HttpPost]
        public virtual JsonNetResult RefreshOptField()
        {
            try
            {
                return JsonNet(ControllerInternal.RefreshOptField());
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.RefreshDetailsFailedMessage, businessException, ReceiptHeaderResx.Receipt));
            }
        }

        /// <summary>
        /// Find whether Receipt record with Receipt Number passed exists
        /// </summary>
        /// <param name="id">Receipt Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns a JsonNetResult object</returns>
        public virtual JsonNetResult Exists(string id, T model)
        {
            try
            {
                return JsonNet(ControllerInternal.Exists(id, model));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.ReceiptNumber));
            }
        }

        #endregion

        #region Action Delete Methods

        /// <summary>
        /// Deletes Receipt
        /// </summary>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="sequenceNumber">Sequence Number</param>
        /// <returns>Receipt View Model</returns>
        [HttpPost]
        public virtual JsonNetResult Delete(string receiptNumber, long sequenceNumber)
        {
            try
            {
                return JsonNet(ControllerInternal.Delete(receiptNumber, sequenceNumber));
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.DeleteFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// et Receipt Item details for Grid rows based on Item
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonNetResult GetRowValues(TU model, int eventType)
        {
            try
            {
                var result = ControllerInternal.GetRowValues(model, eventType);
                var modelBase = new ViewModelBase<ModelBase>
                {
                    Data = result,
                    UserMessage = new UserMessage {IsSuccess = true,},
                };

                return JsonNet(modelBase);
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        /// <summary>
        /// Get Header Values
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="eventType">eventType</param>
        /// <returns>Receipt view model</returns>
        [HttpPost]
        public virtual JsonNetResult GetHeaderValues(T model, int eventType)
        {
            try
            {
                return JsonNet(ControllerInternal.GetHeaderValues(model, eventType)); 
            }
            catch (BusinessException businessException)
            {
                return JsonNet(BuildErrorModelBase(CommonResx.GetFailedMessage, businessException, ReceiptHeaderResx.Receipts));
            }
        }

        #endregion
    }
}
