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

#region namespaces

using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Services.Base.Statefull;
using ValuedParter.TU.Interfaces.BusinessRepository;
using ValuedParter.TU.Interfaces.Services;
using ValuedParter.TU.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace ValuedParter.TU.Services
{
     
    /// <summary>
    ///  Class for Receipt Entity service.
    /// </summary>
    /// <typeparam name="T">Receipt Header</typeparam>
    /// <typeparam name="TU">Receipt Detail</typeparam>
    /// <typeparam name="TDetail2">Receipt Optional Field</typeparam>
    /// <typeparam name="TDetail3">Receipt Detail Optional Field</typeparam>
    /// <typeparam name="TDetail4">Receipt Detail LotNumber</typeparam>
    /// <typeparam name="TDetail5">Receipt Detail SerialNumber</typeparam>
    public class ReceiptEntityService<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> :
        SequencedHeaderDetailFiveService<T, TU, TDetail2, TDetail3, TDetail4, TDetail5, IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>, IReceiptService<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>
        where T : ReceiptHeader, new()
        where TU : ReceiptDetail, new()
        where TDetail2 : ReceiptOptionalField, new()
        where TDetail3 : ReceiptDetailOptionalField, new()
        where TDetail4 : ReceiptDetailLotNumber, new()
        where TDetail5 : ReceiptDetailSerialNumber, new()
    {
        #region Constructor

        /// <summary>
        /// To set Request Context
        /// </summary>
        /// <param name="context">Request Context</param>
        public ReceiptEntityService(Context context)
            : base(context)
        {
        }

        #endregion

        #region Public Methods
         
        /// <summary>
        /// Method to post a receipt
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="yesNo">Yes No</param>
        /// <returns>Receipt Header Model</returns>
        public virtual T Post(T model, Expression<Func<T, bool>> filter, bool yesNo)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();

            return repository.Post(model, filter, yesNo);
        }

        /// <summary>
        /// Method to save a Receipt
        /// </summary>
        /// <param name="model">Receipt model object</param>
        /// <returns>Receipt</returns>
        public override T Save(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();

            var receipt = repository.Save(model);
            return receipt;
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Receipt</returns>
        public override T Add(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            } 
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>(); 
            var receipt = repository.Add(model);
            return receipt;
        }

        /// <summary>
        /// Deletes a receipt
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>Receipt</returns>
        public override T Delete(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.Delete(filter);
        }

        /// <summary>
        /// Gets the specified page data.
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>EnumerableResponse.</returns>
        public override EnumerableResponse<T> Get(int currentPageNumber, int pageSize,
            Expression<Func<T, bool>> filter = null, OrderBy orderBy = null)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            var details = repository.Get(currentPageNumber, pageSize, filter, orderBy);
            return details;
        } 

        /// <summary>
        /// Get Item detail for the grid row for a particular item
        /// </summary>
        /// <param name="model">Receipt detail model</param>
        /// <returns>Receipt detail model</returns>
        /// <param name="eventType">eventType</param>
        /// <returns></returns>
        public virtual TU GetRowValues(TU model, int eventType)

        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            var details = repository.GetRowValues(model, eventType);
            return details;
        }

       /// <summary>
       /// Get Header Values
       /// </summary>
        /// <param name="model">Receiptmodel</param>
        /// <param name="eventType">eventType</param>
        /// <returns>Receipt header model</returns>
        public virtual T GetHeaderValues(T model, int eventType)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetHeaderValues(model, eventType);
        }

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        public virtual EnumerableResponse<TDetail2> SaveOptionalFields(IEnumerable<TDetail2> optionalFieldDetails, string receiptNumber)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.SaveOptionalFields(optionalFieldDetails, receiptNumber); 
        }

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">Is Detail Model</param>
        public virtual bool SaveDetailOptFields(IEnumerable<TDetail3> optionalFieldDetails, string receiptNumber, bool isDetail)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.SaveDetailOptFields(optionalFieldDetails, receiptNumber,isDetail);
        }

        /// <summary>
        /// Refresh Details
        /// </summary>
        /// <param name="model">Detail model</param>
        /// <param name="eventType">Type of event</param>
        /// <returns>Detail model</returns>
        public virtual T RefreshDetail(TU model, string eventType)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.RefreshDetail(model, eventType);
        }

        /// <summary>
        /// Gets optional field data
        /// </summary>
        /// <param name="pageNumber">Current PageNumber</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable Response of Receipt Detail OptionalField model</returns>
        public virtual EnumerableResponse<TDetail3> GetDetailOptFields(int pageNumber, int pageSize)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetDetailOptFields(pageNumber, pageSize);
        }

        /// <summary>
        /// Gets optional field data
        /// </summary>
        /// <param name="pageNumber">Current PageNumber</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable Response of Receipt OptionalField model</returns>
        public virtual EnumerableResponse<TDetail2> GetOptFields(int pageNumber, int pageSize)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetOptFields(pageNumber, pageSize);
        }

        /// <summary>
        /// Refreshes the header when the optional field
        /// </summary>
        /// <returns>returns optional field value</returns>
        public virtual string RefreshOptField()
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.RefreshOptField();
        }

        /// <summary>
        /// Refreshes the specified header.
        /// </summary>
        /// <param name="model">header entity.</param>
        /// <returns>returns refreshed header</returns>
        public new T Refresh(T model)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.Refresh(model);
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        public virtual TDetail2 SetOptionalFieldValue(TDetail2 optionalField)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.SetOptionalFieldValue(optionalField);
        }

        /// <summary>
        /// Set Detail Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        public virtual TDetail3 SetOptionalFieldValue(TDetail3 optionalField)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.SetOptionalFieldValue(optionalField);
        }

        /// <summary>
        /// Creates a new detail
        /// </summary>
        /// <returns>Detail record</returns>
        public override T NewDetail(int pageNumber, int pageSize, TU currentDetail)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            repository.NewDetail(pageNumber, pageSize, currentDetail);
            return repository.GetDefaultDetailOptField();
        } 

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        public virtual TDetail2 GetOptionalFieldFinderData(string optionalField)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetOptionalFieldFinderData(optionalField);
        }

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        public virtual TDetail3 GetDetailOptFieldFinderData(string optionalField)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetDetailOptFieldFinderData(optionalField);
        }

        /// <summary>
        /// Update header and read 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T ReadHeader(T model)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.ReadHeader(model);
        }

        /// <summary>
        /// Find whether Receipt record with Receipt Number passed exists
        /// </summary>
        /// <param name="id">Receipt Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        public virtual bool Exists(string id, T model)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();

            return repository.Exists(id, model);
        }


        /// <summary>
        /// Retrieves the composite rate between the given source and home currencies. 
        /// If the currencies are non-block currencies, the call functions the same as GetCurrencyRate.
        /// </summary>
        /// <param name="rateType">String param for Rate type</param>
        /// <param name="sourceCurrencyCode">String param for Source Currency Code </param>
        /// <param name="date">DateTime param for Date</param>
        /// <returns>Returns the corresponding Currency Rate object.</returns>
        public virtual CompositeCurrencyRate GetCurrencyRateComposite(string rateType,
            string sourceCurrencyCode, DateTime date)
        {
            var repository = Resolve<IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>>();
            return repository.GetCurrencyRateComposite(rateType, sourceCurrencyCode, date);
        }

        #endregion
    }
}