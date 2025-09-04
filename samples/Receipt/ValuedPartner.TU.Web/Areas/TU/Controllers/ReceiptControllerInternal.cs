// The MIT License (MIT) 
// Copyright (c) 1994-2025 Sage Software, Inc.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.AP.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.AP.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Web;
using Sage.CA.SBS.ERP.Sage300.Common.Web.AreaConstants;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Controllers.ExportImport;
using Sage.CA.SBS.ERP.Sage300.Common.Web.Utilities;
using Sage.CA.SBS.ERP.Sage300.CS.Interfaces.Services;
using Sage.CA.SBS.ERP.Sage300.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ValuedPartner.TU.Resources.Forms;
using ValuedPartner.TU.Web.Areas.TU.Models;
using ICService = Sage.CA.SBS.ERP.Sage300.IC.Interfaces.Services;
using Options = Sage.CA.SBS.ERP.Sage300.IC.Models.Options;
using PostingDate = Sage.CA.SBS.ERP.Sage300.IC.Models.Enums.DefaultPostingDate;
using Type = ValuedPartner.TU.Models.Enums.Type;

#endregion

namespace ValuedPartner.TU.Web.Areas.TU.Controllers
{
    /// <summary>
    ///  Controller for Receipt view
    /// </summary>
    public class ReceiptControllerInternal : ImportExportControllerInternal<IReceiptRepository>
    {
        #region Declaration

        /// <summary>
        /// Variable for storing context.
        /// </summary>
        private readonly Context _context;

        /// <summary>
        /// Variable for storing detail optional field cache key.
        /// </summary>
        private readonly string _detailOptFieldCacheKey;

        /// <summary>
        /// Variable for storing options.
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        private static Options _options;

        /// <summary>
        /// Variable for storing functional decimals.
        /// </summary>
        // ReSharper disable once StaticFieldInGenericType
        private static string _funcDecimals;

        private IReceiptRepository _repository
        {
            get
            {
                // return _context.Container.Resolve<IReceiptService>(UnityInjectionType.Default, new ParameterOverride("context", _context));
                return _context.Container.Resolve<IReceiptRepository>(Utilities.ContextParameter(_context));

            }
        }

        private enum Operation
        {
            Create = 0,
            Add = 1,
            Save = 2,
            Post = 3,
            Delete = 4,
            Refresh = 5,
            Get = 6
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for passing context to base.
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptControllerInternal(Context context)
            : base(context)
        {
            _context = context;
            _detailOptFieldCacheKey = CreateSessionKey<ReceiptHeader>(InventoryControl.SessionOptionalField);
            _options = GetOptions();
            var currencyService = Context.Container.Resolve<ICurrencyCodeService<CurrencyCode>>(Utilities.ContextParameter(Context));

            if (_options != null)
            {
                var currency = currencyService.GetById(_options.HomeCurrency);
                if (currency != null)
                {
                    _funcDecimals = currency.DecimalPlacesString;
                }
            }
        }

        #endregion

        #region Internal methods

        /// <summary> 
        /// Gets Receipts by Receipt Number and Sequence Number.
        /// </summary>
        /// <param name="id">Receipt Number</param> 
        /// <param name="oldRecordDeleted">Receipt Number</param>
        /// <param name="isCalledAsPopup">Receipt Number</param>
        /// <returns>Receipt View Model</returns>
        internal ReceiptViewModel GetById(string id, bool oldRecordDeleted = false, bool isCalledAsPopup = false)
        {
            var isRecordExists = true;
            var data = new ReceiptHeader();

            if (!string.IsNullOrEmpty(id))
            {
                data = _repository.GetById(id);
            }

            data.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(data.AdditionalCostCurrency));
            data.TotalReturnCostDecimal = Convert.ToInt32(GetCurrencyDecimal(data.ReceiptCurrency));
            if (data.SequenceNumber == 0)
            {
                data = _repository.NewHeader();
                data.ReceiptNumber = id;
                isRecordExists = false;
                data.Warnings = null;
            }
            if (data.RecordDeleted == RecordDeleted.Yes || oldRecordDeleted)
            {
                var warnings = new List<EntityError>
                {
                    new EntityError
                    {
                        Message = string.Format(ReceiptHeaderResx.DeletedRecord, ReceiptHeaderResx.Receipt),
                        Priority = Priority.Error
                    }
                };
                data = _repository.NewHeader();
                data.Warnings = warnings;
                isRecordExists = false;
            }

            var receiptViewModel = GetViewModel(data, data, "", Operation.Get);
            receiptViewModel.IsExists = isRecordExists;
            receiptViewModel.DisableScreen = isCalledAsPopup;

            if (isCalledAsPopup)
            {
                receiptViewModel.Data.TotalReturnCost = receiptViewModel.Data.TotalCostReceiptAdditional;
            }

            return receiptViewModel;
        }

        /// <summary>
        /// Gets paged receipt detail.
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="filters">Filters</param>
        /// <returns>Receipt details</returns>
        internal EnumerableResponse<ReceiptDetail> GetPagedDetail(int pageNumber, int pageSize, ReceiptHeader model = null, IList<IList<Sage.CA.SBS.ERP.Sage300.Common.Models.Filter>> filters = null)
        {
            if (model != null && model.ReceiptDetail != null && model.ReceiptDetail.Items != null)
            {
                var items = model.ReceiptDetail.Items.Where(x => x.ItemNumber != null && !(x.IsDeleted && x.IsNewLine));
                if (items.Any(x => x.HasChanged || x.IsNewLine || x.IsDeleted))
                {
                    _repository.SaveDetails(items);
                }
            }

            Expression<Func<ReceiptDetail, bool>> filter = null;
            if (model != null)
            {
                filter = receipt => receipt.SequenceNumber == model.SequenceNumber;
            }

            var receiptDetail = _repository.GetDetail(pageNumber, pageSize, filter);
            if (receiptDetail != null)
            {
                if (receiptDetail.Items.Any())
                {
                    int linenumber = (pageNumber * pageSize) + 1;
                    foreach (var item in receiptDetail.Items)
                    {
                        item.DisplayIndex = linenumber;
                        linenumber++;
                    }
                }

            }
            return receiptDetail;
        }

        /// <summary>
        /// Save detail for Receipt
        /// </summary>
        /// <param name="detail">The detail model</param>
        /// <returns>returns Receipt Viewmodel</returns>
        internal ReceiptViewModel SaveDetail(ReceiptDetail detail)
        {
            _options = GetOptions();
            var data = _repository.SaveDetail(detail);

            var receiptViewModel = new ReceiptViewModel
            {
                Data = data,
                Attributes = GetDynamicAttributesOfHeader(),
                UserMessage = new UserMessage(data),
                IsMulticurrency = _options.Multicurrency,
                IsFracQty = _options.FractionalQuantities,
                FracDecimals = _options.FractionalQuantityDecimals,
                ConvFactorDecimal = _options.ConversionFactorDecimals,
                IsItemsAtAllLoc = _options.AllowItemsatAllLocations,
                IsReceiptofNonStock = _options.AllowReceiptofNonstockItems,
                IsPromptToDelete = _options.PrompttoDeleteduringPosting,
                FuncCurrency = _options.HomeCurrency,
                FuncDecimals = _funcDecimals,
                DefaultPostingDate = _options != null ? _options.DefaultPostingDate : PostingDate.SessionDate,
            };

            return receiptViewModel;
        }



        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        internal ReceiptOptionalField GetOptionalFieldFinderData(string optionalField)
        {
            var updatedOptionalField = _repository.GetOptionalFieldFinderData(optionalField);
            return updatedOptionalField;
        }

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        internal ReceiptDetailOptionalField GetDetailOptFieldFinderData(string optionalField)
        {
            var updatedOptionalField = _repository.GetDetailOptFieldFinderData(optionalField);
            return updatedOptionalField;
        }

        /// <summary>
        /// Insert default detail optional field 
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        internal void InsertDetailOptionalField()
        {
            _repository.DetailProcess();
        }

        /// <summary>
        /// Creates a Receipt header 
        /// </summary>
        /// <returns>Receipt View Model</returns>
        internal ReceiptViewModel Create()
        {

            var newData = _repository.NewHeader();
            newData.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(newData.AdditionalCostCurrency));
            return GetViewModel(null, newData, "", Operation.Create);
        }
        /// <summary>
        /// Deletes Receipt Details
        /// </summary>
        /// <param name="model">Receipt Model</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Receipt View Model</returns>
        internal ReceiptViewModel DeleteDetails(ReceiptHeader model, int pageNumber, int pageSize)
        {

            if (model != null)
            {
                if (model.ReceiptDetail != null && model.ReceiptDetail.Items != null)
                {
                    var items = model.ReceiptDetail.Items.Where(x => x.ItemNumber != null);
                    var receiptDetail = items as ReceiptDetail[] ?? items.ToArray();
                    if (receiptDetail.Any(x => x.HasChanged || x.IsNewLine || x.IsDeleted))
                    {
                        _repository.SaveDetails((IEnumerable<ReceiptDetail>)receiptDetail);
                    }
                }
            }

            var pagedDetails = GetReceiptDetail(model, pageNumber, pageSize);
            if (pagedDetails != null)
            {
                model.ReceiptDetail.Items = pagedDetails.Items;
                model.ReceiptDetail.TotalResultsCount = pagedDetails.TotalResultsCount;
            }
            return new ReceiptViewModel
            {
                Data = model,
                UserMessage = new UserMessage(model)
            };

        }

        /// <summary>
        /// Get details based on Page number.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Receipt details data for the SequenceNumber</returns>
        internal EnumerableResponse<ReceiptDetail> GetReceiptDetail(ReceiptHeader model, int pageNumber, int pageSize)
        {
            Expression<Func<ReceiptDetail, bool>> receiptFilter = null;
            if (model != null)
            {
                receiptFilter = receipt => receipt.SequenceNumber == model.SequenceNumber;
            }

            var receiptDetails = _repository.GetDetail(pageNumber, pageSize, receiptFilter);
            var lineNumber = (pageSize * pageNumber) + 1;
            if (receiptDetails != null)
            {
                foreach (var item in receiptDetails.Items)
                {
                    item.DisplayIndex = lineNumber;
                    lineNumber++;
                }
            }
            return receiptDetails;
        }

        /// <summary>
        /// Creates a new Receipt detail
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="index">Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageNumber">page Number</param>
        /// <returns>Receipt model</returns>
        internal ReceiptHeader CreateDetail(ReceiptHeader model, int index, int pageSize, int pageNumber)
        {
            var details = model.ReceiptDetail.Items == null ? model.ReceiptDetail.Items as IList<ReceiptDetail> : model.ReceiptDetail.Items.ToList();
            var hasDetails = model.ReceiptDetail.Items != null && model.ReceiptDetail.Items.Any();
            var linenumber = (pageSize * pageNumber) + 1;
            if (details != null)
            {
                var currentRow = hasDetails ? details.Count > index ? details.ElementAt(index) : details.Last() : null;
                ReceiptHeader header;
                ReceiptDetail data;
                var receiptDetails = new List<ReceiptDetail>();

                // Refresh should be called only once if header model is not mapped with latest model.
                if (currentRow == null || (!currentRow.IsNewLine))
                {
                    _repository.Refresh(model);
                }

                if (hasDetails)
                {
                    if (pageSize > (index + 1))
                    {
                        if (details.Any(x => x.HasChanged))
                        {
                            if (_repository.SaveDetails((IEnumerable<ReceiptDetail>)details))
                            {
                                if (currentRow != null)
                                {
                                    currentRow.IsNewLine = false;
                                    currentRow.HasChanged = false;
                                }
                            }
                        }

                        GetPagedDetail(pageNumber, pageSize);
                        header = CreateDetail(model, pageSize, pageNumber, currentRow);
                        data = (ReceiptDetail)header.ReceiptDetail.Items.FirstOrDefault();

                        if (data != null)
                        {
                            data.IsNewLine = true;
                        }
                        if (details.Count > index)
                        {
                            details[index].IsNewLine = false;
                        }
                        else
                        {
                            details[details.Count].IsNewLine = false;
                        }
                        if (details.Any(x => x.IsNewLine))
                        {
                            _repository.SaveDetails(details as IEnumerable<ReceiptDetail>);
                        }
                        receiptDetails = model.ReceiptDetail.Items.Take(pageSize - 1).ToList();
                        foreach (var setDetail in receiptDetails)
                        {
                            setDetail.HasChanged = false;
                        }

                        receiptDetails.Insert(index + 1, data);

                        foreach (var detail in receiptDetails)
                        {
                            detail.DisplayIndex = linenumber;
                            linenumber++;
                        }
                        header.ReceiptDetail.Items = receiptDetails;
                        header.ReceiptDetail.TotalResultsCount =
                            model.ReceiptDetail.TotalResultsCount + 1;
                        return header;
                    }
                    if (details.Any(x => x.HasChanged))
                    {
                        if (_repository.SaveDetails((IEnumerable<ReceiptDetail>)details))
                        {
                            if (currentRow != null)
                            {
                                currentRow.IsNewLine = false;
                            }
                        }
                    }

                    var pagedDetails = GetPagedDetail(pageNumber, pageSize, model);
                    var totalRecords = pagedDetails.TotalResultsCount;
                    header = CreateDetail(model, pageSize, pageNumber, currentRow);
                    data = (ReceiptDetail)header.ReceiptDetail.Items.FirstOrDefault();
                    receiptDetails = new List<ReceiptDetail>(pagedDetails.Items.ToList());
                    if (data != null)
                    {
                        data.IsNewLine = true;
                        data.DisplayIndex = linenumber;
                    }

                    if (pageSize != (index + 1))
                    {
                        receiptDetails = receiptDetails.Take(pageSize - 1).ToList();
                        foreach (var detail in receiptDetails)
                        {
                            detail.HasChanged = false;
                        }
                        receiptDetails.Insert(0, data);

                    }
                    foreach (var detail in receiptDetails)
                    {
                        detail.DisplayIndex = linenumber;
                        linenumber++;
                    }

                    header.ReceiptDetail.Items = receiptDetails;
                    header.ReceiptDetail.TotalResultsCount = totalRecords + 1;
                    return header;
                }
                header = CreateDetail(model, pageSize, pageNumber, null);
                data = (ReceiptDetail)header.ReceiptDetail.Items.FirstOrDefault();

                if (data != null)
                {
                    if (data.SequenceNumber != 0)
                    {
                        data.SequenceNumber = model.SequenceNumber;
                    }

                    data.IsNewLine = true;

                    receiptDetails.Add(data);
                }
                foreach (var detail in receiptDetails)
                {
                    detail.DisplayIndex = linenumber;
                    linenumber++;
                }
                header.ReceiptDetail.Items = receiptDetails;
                header.ReceiptDetail.TotalResultsCount++;
                return header;
            }
            return null;
        }

        /// <summary>
        /// Updates/Save the Receipt
        /// </summary>
        /// <param name="model">Modified Receipt view model</param>
        /// <returns>Updated Receipt view model</returns>
        internal ReceiptViewModel Save(ReceiptHeader model)
        {
            model.RecordStatus = RecordStatus.Entered;
            var data = _repository.Save(model);
            var newData = _repository.NewHeader();
            SessionHelper.Remove(_detailOptFieldCacheKey);

            return GetViewModel(data, newData, model.ReceiptNumber, Operation.Save);
        }

        /// <summary>
        /// Post Receipt
        /// </summary>
        /// <param name="model">Receipt Header model</param>
        /// <param name="id">Receipt Sequence Number</param>
        /// <param name="yesNo">Yes or No Click in confirmation box</param>
        /// <returns>ReceiptViewModel</returns>
        internal ReceiptViewModel Post(ReceiptHeader model, long id, bool yesNo)
        {

            Expression<Func<ReceiptHeader, Boolean>> filter = receipt => receipt.SequenceNumber == id;
            var data = _repository.Post(model, filter, yesNo);
            var newData = _repository.NewHeader();

            return GetViewModel(data, newData, model.ReceiptNumber, Operation.Post);
        }

        /// <summary>
        /// Add Receipt
        /// </summary>
        /// <param name="model">Receipt Model</param>
        /// <returns>Receipt View Model</returns>
        internal ReceiptViewModel Add(ReceiptHeader model)
        {
            var data = _repository.Add(model);
            var newData = _repository.NewHeader();

            return GetViewModel(data, newData, model.ReceiptNumber, Operation.Add);
        }

        /// <summary>
        /// Deletes Receipt
        /// </summary>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="sequenceNumber">Receipt Sequence Number</param>
        /// <returns>ReceiptViewModel</returns>
        internal ReceiptViewModel Delete(string receiptNumber, long sequenceNumber)
        {
            Expression<Func<ReceiptHeader, Boolean>> filter = receipt => receipt.ReceiptNumber == receiptNumber;
            var data = _repository.Delete(filter);
            var newData = _repository.NewHeader();

            return GetViewModel(data, newData, "", Operation.Delete);
        }

        /// <summary>
        ///  Read Header
        /// </summary>
        /// <param name="model"></param>
        /// <param name="setHeaderValue"></param>
        /// <returns></returns>
        internal ReceiptHeader ReadHeader(ReceiptHeader model, bool setHeaderValue)
        {
            ReceiptHeader header = null;
            if (setHeaderValue)
            {
                header = model;
            }
            header = _repository.ReadHeader(header);
            header.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(header.AdditionalCostCurrency));
            SetHeaderDetails(model, header);
            return header;
        }

        /// <summary>
        /// Save the details
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Receipt Batch Viewmodel - that contains the info about the saved details</returns>
        internal ReceiptHeader SaveDetails(ReceiptHeader model)
        {
            _repository.SaveDetails(model.ReceiptDetail.Items as IEnumerable<ReceiptDetail>);
            var refreshedModel = _repository.Refresh(model);
            refreshedModel.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(refreshedModel.AdditionalCostCurrency));
            if (refreshedModel.ReceiptDetail.Items.Any())
            {
                foreach (var detail in refreshedModel.ReceiptDetail.Items)
                {
                    detail.IsNewLine = false;
                    detail.HasChanged = false;
                    detail.IsDeleted = false;
                }
            }
            return refreshedModel;
        }

        /// <summary>
        /// Save the Receipt Optional field
        /// </summary>
        /// <param name="optionalFields">List of Receipt optional field</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">is Detail model</param>
        /// <returns>Receipt optional field with updated info</returns>
        internal List<ReceiptDetailOptionalField> SaveOptionalFields(List<ReceiptDetailOptionalField> optionalFields, string receiptNumber, bool isDetail)
        {
            if (optionalFields != null)
            {
                foreach (var optionalField in optionalFields)
                {
                    FormatDetailOptField(optionalField);
                }
            }

            var savedOptionalFields = _repository.SaveDetailOptFields(optionalFields, receiptNumber, isDetail);

            if (!savedOptionalFields) return optionalFields;
            if (optionalFields == null)
            {
                return null;
            }

            if (optionalFields.Any())
            {
                foreach (var optionalField in optionalFields)
                {
                    optionalField.IsNewLine = false;
                    optionalField.HasChanged = false;
                    optionalField.IsDeleted = false;
                }
            }

            return optionalFields;
        }

        /// <summary>
        /// Gets item detail based on item detail in grid row.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        internal ReceiptDetail GetRowValues(ReceiptDetail model, int eventType)
        {
            var result = _repository.GetRowValues(model, eventType);
            if (model.IsNewLine == true)
            {
                result.IsNewLine = true;
            }
            return result;
        }

        /// <summary>
        /// Get Header Values
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        internal ReceiptHeader GetHeaderValues(ReceiptHeader model, int eventType)
        {
            var modelData = _repository.GetHeaderValues(model, eventType);
            modelData.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(model.AdditionalCostCurrency));
            modelData.TotalReturnCostDecimal = Convert.ToInt32(GetCurrencyDecimal(model.ReceiptCurrency));
            return modelData;
        }

        /// <summary>
        /// Get IC Options
        /// </summary>
        /// <returns>IC Options</returns>
        internal Options GetOptions()
        {
            var service =
             Context.Container.Resolve<ICService.IOptionsService<Options>>(Utilities.ContextParameter(Context));
            return service.FirstOrDefault();
        }

        /// <summary>
        /// GetDynamicAttributesOfHeader
        /// </summary>
        /// <returns></returns>
        internal IDictionary<string, object> GetDynamicAttributesOfHeader()
        {
            // not implemented, just return null
            return null;
        }

        /// <summary>
        ///  Check the Receipt Date/Posting Date Range in company profile
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal ViewModelBase<CompanyProfile> ValidateDate(DateTime date)
        {
            var companyProfile = GetEntity<CompanyProfile>(CompanyProfile.EntityName);
            var response = new ViewModelBase<CompanyProfile> { UserMessage = new UserMessage { IsSuccess = true } };
            if (companyProfile != null)
            {
                var warningDateRange = companyProfile.CompanyProfileOptions.WarningDateRange;
                var maxDate = Context.SessionDate.AddDays(warningDateRange);
                var minDate = Context.SessionDate.AddDays(-warningDateRange);
                if (date > maxDate || date < minDate)
                {
                    response.UserMessage = new UserMessage { Message = string.Format(ReceiptHeaderResx.DateOutOfRange) };
                }
            }
            return response;
        }

        /// <summary>
        /// Get the Entity
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entityName"></param>
        /// <returns></returns>
        internal TModel GetEntity<TModel>(string entityName) where TModel : ModelBase, new()
        {
            var entity = new TModel();
            var service = GetEntityService<TModel>(entityName);
            if (service != null)
            {
                var entities = service.Get();
                if (entities != null)
                {
                    return (entities.Items != null && entities.Items.Any() ? entities.Items.First() : entity);
                }
            }
            return entity;
        }

        /// <summary>
        ///  Check the Spread Range for the Exchange rate entered
        /// </summary>
        /// <param name="rateType"></param>
        /// <param name="fromCurrency"></param>
        /// <param name="rateDate"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="tocurrency"></param>
        /// <returns></returns>
        internal ViewModelBase<CurrencyRate> CheckRateSpread(string rateType, string fromCurrency, DateTime rateDate, decimal exchangeRate, string tocurrency)
        {
            var response = new ViewModelBase<CurrencyRate> { UserMessage = new UserMessage { IsSuccess = true } };
            var rateData = ValidateExchangeRate(rateType, fromCurrency, rateDate);

            if (rateData == null)
            {
                response.Data.Rate = exchangeRate;
                response.Data.RateType = rateType;
                return response;
            }
            var max = rateData.Rate + rateData.Spread;
            var min = rateData.Rate - rateData.Spread;

            response.Data.Rate = rateData.Rate;
            response.Data.RateType = rateData.RateType;
            if (exchangeRate > max || exchangeRate < min)
            {
                response.UserMessage = new UserMessage { Message = string.Format(ReceiptHeaderResx.CurrencyRateSpreadError) };
            }
            return response;
        }


        /// <summary>
        /// GetVendorDetail
        /// </summary>
        /// <param name="vendorNumber"></param>
        /// <returns></returns>
        internal ViewModelBase<Vendor> GetVendorDetail(string vendorNumber)
        {
            var response = new ViewModelBase<Vendor> { UserMessage = new UserMessage { IsSuccess = true } };

            var vendorService = Context.Container.Resolve<IVendorService<Vendor>>(Utilities.ContextParameter(Context));
            var vendor = vendorService.GetById(vendorNumber);

            response.Data = vendor;
            return response;
        }

        /// <summary>
        /// Find whether Receipt record with Receipt Number passed exists
        /// </summary>
        /// <param name="id">Receipt Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        internal bool Exists(string id, ReceiptHeader model)
        {
            return _repository.Exists(id, model);
        }

        #endregion

        #region Public Methods

        public void SetHeaderDetails(ReceiptHeader source, ReceiptHeader target)
        {
            if (source == null) return;

            target.ReceiptDetail = source.ReceiptDetail;

            if (target.ReceiptDetail.Items.Any())
            {
                foreach (var detail in target.ReceiptDetail.Items)
                {
                    detail.IsNewLine = false;
                    detail.HasChanged = false;
                    detail.IsDeleted = false;
                }
            }
            target.ReceiptOptionalField = source.ReceiptOptionalField;
            target.ReceiptDetailOptionalField = source.ReceiptDetailOptionalField;
        }

        /// <summary>
        /// Set Detail to current detail
        /// </summary>
        /// <param name="currentDetail">Current Selected Detail</param>
        /// <returns>returns ReceiptDetail model</returns>
        public virtual ReceiptDetail SetDetail(ReceiptDetail currentDetail)
        {
            var model = _repository.SetDetail(currentDetail);
            return model != null ? model.ReceiptDetail.Items.FirstOrDefault() as ReceiptDetail : null;
        }

        /// <summary>
        /// Refreshes the detail when the data is changed.
        /// </summary>
        /// <param name="detail">Detail model</param>
        /// <param name="eventType">Type of event</param>
        /// <returns>Receipt View Model</returns>
        public virtual ReceiptViewModel RefreshDetail(ReceiptDetail detail, string eventType)
        {
            var refreshDetail = _repository.RefreshDetail(detail, eventType);
            var receiptDetail = refreshDetail.ReceiptDetail.Items.FirstOrDefault();
            if (receiptDetail != null)
            {
                receiptDetail.IsNewLine = detail.IsNewLine;
            }
            var receipt = new ReceiptHeader
            {
                ReceiptDetail = new EnumerableResponse<ReceiptDetail>
                {
                    Items = new List<ReceiptDetail> { receiptDetail, }
                }
            };

            return new ReceiptViewModel
            {
                Data = receipt,
                UserMessage = new UserMessage(receiptDetail)
            };
        }

        /// <summary>
        /// Refreshes the header when the optional field
        /// </summary>
        /// <returns>Optional field value</returns>
        public virtual string RefreshOptField()
        {
            return _repository.RefreshOptField();
        }

        ///<summary>
        /// Refresh the header
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>Refreshed header model</returns>
        internal ReceiptViewModel Refresh(ReceiptHeader model)
        {
            var data = _repository.Refresh(model);
            data.TotalCostReceiptAdditionalDecimal = Convert.ToInt32(GetCurrencyDecimal(model.AdditionalCostCurrency));
            data.TotalReturnCostDecimal = Convert.ToInt32(GetCurrencyDecimal(model.ReceiptCurrency));
            GetExchangeRate(data);

            return GetViewModel(data, data, "", Operation.Refresh);
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="model">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        public virtual ReceiptOptionalField SetOptionalFieldValue(ReceiptOptionalField model)
        {
            if (model.Type == Type.Date)
            {
                model.Value = DateUtil.GetShortDate(model.Value, null);
            }
            var updatedOptionalField = _repository.SetOptionalFieldValue(model);
            if (updatedOptionalField != null)
            {
                updatedOptionalField.IsNewLine = model.IsNewLine;
                updatedOptionalField.DisplayIndex = model.DisplayIndex;
            }
            return updatedOptionalField;
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="model">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        public virtual ReceiptDetailOptionalField SetOptionalFieldValue(ReceiptDetailOptionalField model)
        {
            if (model.Type == Type.Date)
            {
                model.Value = DateUtil.GetShortDate(model.Value, null);
            }
            var updatedOptionalField = _repository.SetOptionalFieldValue(model);
            if (updatedOptionalField != null)
            {
                updatedOptionalField.IsNewLine = model.IsNewLine;
                updatedOptionalField.DisplayIndex = model.DisplayIndex;
            }
            return updatedOptionalField;
        }

        /// <summary>
        /// Delete Receipt Detail Optional Fields
        /// </summary>
        /// <param name="model">Enumerable Response of BaseInvoiceOptionalField model</param>
        /// <param name="pageNumber">Current PageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="isDetail">isDetail model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        public virtual EnumerableResponse<ReceiptDetailOptionalField> DeleteOptionalFields(
            EnumerableResponse<ReceiptDetailOptionalField> model, int pageNumber, int pageSize, bool isDetail)
        {
            _repository.SaveDetailOptFields(model.Items, null, isDetail);
            if (isDetail)
            {
                var pagedDetail = GetDetailOptFields(pageNumber, pageSize, model);
                var refreshedDetail = (List<ReceiptDetailOptionalField>)pagedDetail.Items;
                var totalResultsCount = pagedDetail.TotalResultsCount;
                var returnDetail = new EnumerableResponse<ReceiptDetailOptionalField>
                {
                    Items = refreshedDetail,
                    TotalResultsCount = totalResultsCount
                };
                return returnDetail;
            }
            else
            {
                var pagedDetail = GetOptFields(pageNumber, pageSize, model);
                var refreshedDetail = pagedDetail.Items.Cast<ReceiptDetailOptionalField>();
                var totalResultsCount = pagedDetail.TotalResultsCount;
                var returnDetail = new EnumerableResponse<ReceiptDetailOptionalField>
                {
                    Items = refreshedDetail,
                    TotalResultsCount = totalResultsCount
                };
                return returnDetail;
            }
        }

        /// <summary>
        /// Gets Receipt Detail optional fields
        /// </summary>
        /// <param name="pageNumber">Current pageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="model">Receipt Detail OptionalField model</param>
        /// <returns>Receipt Detail Optional Field</returns>
        public virtual EnumerableResponse<ReceiptDetailOptionalField> GetDetailOptFields(int pageNumber, int pageSize,
            EnumerableResponse<ReceiptDetailOptionalField> model)
        {
            if (model == null)
            {
                return _repository.GetDetailOptFields(pageNumber, pageSize);
            }

            var optionalField = model.Items;

            var optionalFieldDetails = optionalField as IList<ReceiptDetailOptionalField> ?? optionalField.ToList();
            if (!optionalFieldDetails.Any(x => x.HasChanged || x.IsNewLine || x.IsDeleted))
            {
                return _repository.GetDetailOptFields(pageNumber, pageSize);
            }

            if (optionalFieldDetails.Any(x => x.OptionalField != null))
            {
                foreach (var optField in optionalFieldDetails)
                {
                    FormatDetailOptField(optField);
                }
                _repository.SaveDetailOptFields(optionalFieldDetails, null, true);
            }

            return _repository.GetDetailOptFields(pageNumber, pageSize);
        }

        /// <summary>
        /// Gets Receipt Detail optional fields
        /// </summary>
        /// <param name="pageNumber">Current pageNumber</param>
        /// <param name="pageSize">PageSize</param>
        /// <param name="model">List of ReceiptOptionalField model</param>
        /// <returns>returns Enumerable Response of ReceiptOptionalField</returns>
        public virtual EnumerableResponse<ReceiptOptionalField> GetOptFields(int pageNumber, int pageSize,
            EnumerableResponse<ReceiptDetailOptionalField> model)
        {
            if (model == null)
                return _repository.GetOptFields(pageNumber, pageSize);
            var optionalField = model.Items;

            var optionalFieldDetails = optionalField as IList<ReceiptDetailOptionalField> ?? optionalField.ToList();
            if (!optionalFieldDetails.Any(x => x.HasChanged || x.IsNewLine || x.IsDeleted))
            {
                return _repository.GetOptFields(pageNumber, pageSize);
            }

            if (optionalFieldDetails.Any(x => x.OptionalField != null))
            {
                foreach (var optField in optionalFieldDetails)
                {
                    FormatDetailOptField(optField);
                }
                _repository.SaveDetailOptFields(optionalFieldDetails, null, false);
            }

            return _repository.GetOptFields(pageNumber, pageSize);
        }

        /// <summary>
        /// Gets the currency description.
        /// </summary>
        /// <param name="currencyCode">The currency code.</param>
        /// <returns></returns>
        public CurrencyCode GetCurrencyDescription(string currencyCode)
        {
            var currencyService = Context.Container.Resolve<ICurrencyCodeService<CurrencyCode>>(Utilities.ContextParameter(Context));
            Expression<Func<CurrencyCode, bool>> filter = code => code.CurrencyCodeId == currencyCode.ToUpper();
            var currency = currencyService.Get(filter);
            if (currency != null)
            {
                CurrencyCode firstOrDefault = currency.Items.FirstOrDefault();
                if (firstOrDefault != null) return (firstOrDefault);
            }
            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the currency Description
        /// </summary>
        /// <param name="additionalCurrencyDescription"></param>
        /// <param name="refreshModel"></param>
        /// <returns></returns>
        private CurrencyCode CurrencyDescription(out string additionalCurrencyDescription, ReceiptHeader refreshModel)
        {
            var receiptcurrencyCode = GetCurrencyDescription(refreshModel.ReceiptCurrency);
            if (refreshModel.ReceiptCurrency == refreshModel.AdditionalCostCurrency)
            {
                additionalCurrencyDescription = receiptcurrencyCode.Description;
            }
            else
            {
                var aditionalCurrencyCode = GetCurrencyDescription(refreshModel.AdditionalCostCurrency);
                additionalCurrencyDescription = aditionalCurrencyCode.Description;
            }
            return receiptcurrencyCode;
        }


        /// <summary>
        /// Creates a Receipt detail record.
        /// </summary>
        /// <param name="model">Receipt Detail Model</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageNumber">Current Page Number</param>
        /// <param name="currentRow">Current tax detail record</param>
        /// <returns>Receipt ViewModel</returns>
        private ReceiptHeader CreateDetail(ReceiptHeader model, int pageSize, int pageNumber, ReceiptDetail currentRow)
        {
            var data = _repository.NewDetail(pageNumber, pageSize, currentRow as ReceiptDetail);

            if (model.ReceiptDetail.Items != null)
            {
                // Mapping the user entered Sequence Number with the new detail item
                data.SequenceNumber = model.SequenceNumber;
                data.ReceiptDetail.Items.First().SequenceNumber = model.SequenceNumber;
                data.ReceiptDetail.IsNewLine = true;
            }

            data.IsNewLine = true;
            return data;
        }

        /// <summary>
        /// Format Optional Field Values 
        /// </summary>
        /// <param name="model"></param>
        private void FormatDetailOptField(ReceiptDetailOptionalField model)
        {
            if (model.Type == Type.Date && model.Value != null)
            {
                model.Value = DateUtil.GetShortDate(model.Value, null, true);
            }
        }

        /// <summary>
        /// Get the Entity _repository
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private IEntityService<TModel> GetEntityService<TModel>(string entityName) where TModel : ModelBase, new()
        {
            switch (entityName)
            {
                case CompanyProfile.EntityName:
                    return
                        Context.Container.Resolve<ICompanyProfileService<CompanyProfile>>(Utilities.ContextParameter(Context)) as IEntityService<TModel>;
            }
            return null;
        }

        /// <summary>
        /// Get ExchangeRate
        /// </summary>
        /// <param name="refreshModel">ExchangeRate</param>
        private void GetExchangeRate(ReceiptHeader refreshModel)
        {
            if (refreshModel.ReceiptType == ReceiptType.Receipt)
            {
                var exchangeRate = ValidateExchangeRate(refreshModel.RateType, refreshModel.ReceiptCurrency, refreshModel.RateDate);
                if (exchangeRate != null)
                {
                    refreshModel.ExchangeRate = exchangeRate.Rate;
                }
            }
        }

        /// <summary>
        ///  Validate Exchange rate 
        /// </summary>
        /// <param name="rateType">exchangeRate</param>
        /// <param name="currencyCode">CurrencyCode</param>
        /// <param name="rateDate">rateDate</param>
        /// <returns>CompositeCurrencyRate</returns>
        private CompositeCurrencyRate ValidateExchangeRate(string rateType, string currencyCode, DateTime rateDate)
        {
            return _repository.GetCurrencyRateComposite(rateType, currencyCode, rateDate);
        }

        /// <summary>
        /// Gets the currency decimal
        /// </summary>
        /// <param name="currencyCode">Currency code</param>
        /// <returns>Currency decimal for currency code</returns>
        private string GetCurrencyDecimal(string currencyCode)
        {
            var currencyService = Context.Container.Resolve<ICurrencyCodeService<CurrencyCode>>(Utilities.ContextParameter(Context));
            var currency = currencyService.GetById(currencyCode);
            return currency != null ? currency.DecimalPlacesString : "0";
        }

        private void ProcessMessage(ReceiptHeader data, out string message)
        {
            var warnings = new List<EntityError>();

            if (data.Warnings.Any())
            {
                var errorMessages = data.Warnings.ToList();
                var messages = errorMessages.FindAll(x => x.Priority == Priority.Message);
                message = (messages.Count > 0) ? messages.FirstOrDefault().Message : "";
                var warningMessage = errorMessages.FindAll(x => x.Priority == Priority.Warning);

                if (warningMessage.Count > 0)
                {
                    var warning = warningMessage.FirstOrDefault().Message;
                    if (warning != "")
                    {
                        warnings.Add(new EntityError { Message = warning, Priority = Priority.Warning });
                        data.Warnings = warnings;
                    }
                }
            }
            else
            {
                message = string.Empty;
            }
        }

        /// <summary>
        /// Get View model 
        /// </summary>
        /// <param name="data"> data</param>
        /// <param name="newData"> new data</param>
        /// <param name="receiptNumber"> receipt number</param>
        /// <param name="operation"> operation</param>
        /// <returns>view model</returns>
        private ReceiptViewModel GetViewModel(ReceiptHeader data, ReceiptHeader newData, string receiptNumber, Operation operation)
        {

            var additionalCurrencyDescription = string.Empty;
            var currencyCode = CurrencyDescription(out additionalCurrencyDescription, newData);
            var message = string.Empty;
            var defaultReceiptNumber = string.Empty;

            _options = GetOptions();

            if (_options.DefaultReceiptNumber == newData.ReceiptNumber)
            {
                defaultReceiptNumber = newData.ReceiptNumber;
            }

            if (operation == Operation.Add || operation == Operation.Post || operation == Operation.Save)
            {
                ProcessMessage(data, out message);

                if (string.IsNullOrEmpty(message))
                {
                    message = string.Format("*** TODO. NOT SURE WHY RECEIPT SDK SAMPLE HAS THIS MESSAGE*** {0}", receiptNumber);
                }
                message += "\n\nSource: " + DateTime.Now.ToString("T");
            }
            if (operation == Operation.Delete)
            {
                message = string.Format(CommonResx.DeleteSuccessMessage, ReceiptHeaderResx.ReceiptNumber, data.ReceiptNumber);
            }
            if (operation == Operation.Create || operation == Operation.Get)
            {
                message = null;
            }

            return new ReceiptViewModel
            {
                Data = newData,
                Attributes = GetDynamicAttributesOfHeader(),
                IsMulticurrency = _options.Multicurrency,
                IsFracQty = _options.FractionalQuantities,
                FracDecimals = _options.FractionalQuantityDecimals,
                ConvFactorDecimal = _options.ConversionFactorDecimals,
                IsItemsAtAllLoc = _options.AllowItemsatAllLocations,
                IsReceiptofNonStock = _options.AllowReceiptofNonstockItems,
                IsPromptToDelete = _options.PrompttoDeleteduringPosting,
                FuncCurrency = _options.HomeCurrency,
                FuncDecimals = _funcDecimals,
                DefaultReceiptNumber = defaultReceiptNumber,
                ReceiptCurrencyDescription = currencyCode.Description,
                AddlCostCurrencyDescription = additionalCurrencyDescription,
                DefaultPostingDate = _options != null ? _options.DefaultPostingDate : PostingDate.SessionDate,
                UserMessage = new UserMessage(newData, message)
            };
        }

        #endregion
    }
}