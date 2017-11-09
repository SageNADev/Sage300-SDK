// The MIT License (MIT) 
// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.
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

using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service.Base.Statefull;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace ValuedPartner.TU.Interfaces.Services
{
    /// <summary>
    /// Interface for IReceiptService
    /// </summary>
    /// <typeparam name="T">Receipt Header</typeparam>
    /// <typeparam name="TU">Receipt Detail</typeparam>
    /// <typeparam name="TDetail2">Receipt Optional Field</typeparam>
    /// <typeparam name="TDetail3">Receipt Detail Optional Field</typeparam>
    /// <typeparam name="TDetail4">Receipt Detail LotNumber</typeparam>
    /// <typeparam name="TDetail5">Receipt Detail SerialNumber</typeparam>
    public interface IReceiptService<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> :
        ISequencedHeaderDetailFiveService<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> where T : ReceiptHeader, new()
        where TU : ReceiptDetail, new()
        where TDetail2 : ReceiptOptionalField, new()
        where TDetail3 : ReceiptDetailOptionalField, new()
        where TDetail4 : ReceiptDetailLotNumber, new()
        where TDetail5 : ReceiptDetailSerialNumber, new()
    {
        /// <summary>
        /// Interface method to Post model
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="filter">Filter</param>
        /// <param name="yesNo">Yes / No</param>
        /// <returns>Receipt Header Model</returns>
        T Post(T model, Expression<Func<T, Boolean>> filter, bool yesNo);

        /// <summary>
        /// Get Item detail for the grid row for a particular item
        /// </summary>
        /// <param name="model">Receipt Detail Model</param>
        /// <returns>Receipt Detail Model</returns>
        /// <param name="eventType">eventType</param>
        /// <returns></returns>
        TU GetRowValues(TU model, int eventType);

        /// <summary>
        /// Get Header Values
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="eventType">eventType</param>
        /// <returns>Receipt Header view Model</returns>
        T GetHeaderValues(T model, int eventType);

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        EnumerableResponse<TDetail2> SaveOptionalFields(IEnumerable<TDetail2> optionalFieldDetails, string receiptNumber);

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">Is Detail Model</param>
        bool SaveDetailOptFields(IEnumerable<TDetail3> optionalFieldDetails, string receiptNumber, bool isDetail);

        /// <summary>
        /// Refresh the Detail
        /// </summary>
        /// <param name="detail">TU model</param>
        /// <param name="eventType">Property that changed</param>
        /// <returns>TU model</returns>
        T RefreshDetail(TU detail, string eventType);

        /// <summary>
        /// Gets Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable response of Receipt Detail Optional Field</returns>
        EnumerableResponse<TDetail3> GetDetailOptFields(int pageNumber, int pageSize);

        /// <summary>
        /// Gets Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable response of Receipt Optional Field</returns>
        EnumerableResponse<TDetail2> GetOptFields(int pageNumber, int pageSize);

        /// <summary>
        /// Refreshes the header when the optional field
        /// </summary>
        /// <returns>returns optional field value</returns>
        string RefreshOptField();

        /// <summary>
        /// Refreshes the specified header.
        /// </summary>
        /// <param name="header">header entity.</param>
        /// <returns>returns refreshed header</returns>
        new T Refresh(T header);

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        TDetail2 SetOptionalFieldValue(TDetail2 optionalField);

        /// <summary>
        /// Set Detail Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        TDetail3 SetOptionalFieldValue(TDetail3 optionalField);



        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        TDetail2 GetOptionalFieldFinderData(string optionalField);

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        TDetail3 GetDetailOptFieldFinderData(string optionalField);

        /// <summary>
        /// Read Header
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        T ReadHeader(T model); 

        /// <summary>
        /// Find whether Receipt record with Receipt Number passed exists
        /// </summary>
        /// <param name="id">Receipt Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        bool Exists(string id, T model);

        /// <summary>
        /// Retrieves the composite rate between the given source and home currencies. 
        /// If the currencies are non-block currencies, the call functions the same as GetCurrencyRate.
        /// </summary>
        /// <param name="rateType">String param for Rate type</param>
        /// <param name="sourceCurrencyCode">String param for Source Currency Code </param>
        /// <param name="date">Date</param>
        /// <returns>Returns the corresponding Currency Rate object.</returns>
        CompositeCurrencyRate GetCurrencyRateComposite(string rateType,
            string sourceCurrencyCode, DateTime date);
    }
}
