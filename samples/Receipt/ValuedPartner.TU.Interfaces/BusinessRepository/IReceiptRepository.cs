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

#region Namespaces

using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository.Base.Statefull;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Repository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Service;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using ValuedPartner.TU.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace ValuedPartner.TU.Interfaces.BusinessRepository
{
    /// <summary>
    ///  Interface for IReceipt Entity 
    /// </summary>
    public interface IReceiptRepository : ISecurity, ISecurityService, IImportExport
    {
        /// <summary>
        /// Interface method to Post model
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="filter">Filter</param>
        /// <param name="yesNo">Yes / No</param>
        /// <returns>Receipt Header Model</returns>
        ReceiptHeader Post(ReceiptHeader model, Expression<Func<ReceiptHeader, Boolean>> filter, bool yesNo);

        /// <summary>
        /// Interface method to get Item detail for the grid row for a particular item
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <returns>Receipt Detail Model</returns>
        /// <param name="eventType">eventType</param>
        /// <returns></returns>
        ReceiptDetail GetRowValues(ReceiptDetail model, int eventType);

        /// <summary>
        /// Get Header Values
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="eventType">eventType</param>
        /// <returns>Receipt Header view Model</returns>
        ReceiptHeader GetHeaderValues(ReceiptHeader model, int eventType);

        ReceiptHeader GetById<TKey>(TKey id);

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        EnumerableResponse<ReceiptOptionalField> SaveOptionalFields(IEnumerable<ReceiptOptionalField> optionalFieldDetails, string receiptNumber);

        /// <summary>
        /// Save Detail Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">Is Detail Model</param>
        bool SaveDetailOptFields(IEnumerable<ReceiptDetailOptionalField> optionalFieldDetails, string receiptNumber, bool isDetail);

        /// <summary>
        /// Refresh the Detail
        /// </summary>
        /// <param name="detail">ReceiptDetail model</param>
        /// <param name="eventType">Property that changed</param>
        /// <returns>ReceiptDetail model</returns>
        ReceiptHeader RefreshDetail(ReceiptDetail detail, string eventType);

        /// <summary>
        /// Gets Detail Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable response of Receipt Detail Optional Field</returns>
        EnumerableResponse<ReceiptDetailOptionalField> GetDetailOptFields(int pageNumber, int pageSize);

        /// <summary>
        /// Gets Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Enumerable response of Receipt Optional Field</returns>
        EnumerableResponse<ReceiptOptionalField> GetOptFields(int pageNumber, int pageSize);

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
        ReceiptHeader Refresh(ReceiptHeader header);

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        ReceiptOptionalField SetOptionalFieldValue(ReceiptOptionalField optionalField);

        /// <summary>
        /// Set Detail Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        ReceiptDetailOptionalField SetOptionalFieldValue(ReceiptDetailOptionalField optionalField);

        /// <summary>
        /// Get the Default Optional Field
        /// </summary>
        /// <returns>Header with default optional field</returns>
        ReceiptHeader GetDefaultDetailOptField();


        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        ReceiptOptionalField GetOptionalFieldFinderData(string optionalField);

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        ReceiptDetailOptionalField GetDetailOptFieldFinderData(string optionalField);

        /// <summary>
        /// Read Header
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        ReceiptHeader ReadHeader(ReceiptHeader model);

        /// <summary>
        /// Find whether Receipt record with Receipt Number passed exists
        /// </summary>
        /// <param name="id">Receipt Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        bool Exists(string id, ReceiptHeader model);

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

        /// <summary>
        ///  Add new record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReceiptHeader Add(ReceiptHeader model);

        /// <summary>
        ///  Save record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReceiptHeader Save(ReceiptHeader model);

        /// <summary>
        ///  Delete record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ReceiptHeader Delete(Expression<Func<ReceiptHeader, bool>> filter);

        /// <summary>
        /// Creates a new Detail
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="currentDetail">The current detail.</param>
        /// <returns>New detail</returns>
        ReceiptHeader NewDetail(int pageNumber, int pageSize, ReceiptDetail currentDetail);

        /// <summary>
        /// Creates a new header
        /// </summary>
        /// <returns>New header</returns>
        ReceiptHeader NewHeader();

        /// <summary>
        ///  Save Details
        /// </summary>
        /// <param name="details">Details</param>
        /// <returns>True if successfully saved, false otherwise</returns>
        bool SaveDetails(IEnumerable<ReceiptDetail> details);

        /// <summary>
        /// Save for detail Entry
        /// </summary>
        /// <param name="detail">Detail model</param>
        /// <returns>Saved detail</returns>
        ReceiptHeader SaveDetail(ReceiptDetail detail);


        /// <summary>
        /// Gets the Details
        /// </summary>
        /// <param name="pageNumber">Current Page Number</param>
        /// <param name="pageSize">Number of records to be retrieved per page</param>
        /// <param name="filter">Filter expression</param>
        /// <param name="orderBy">Sorting order of the records</param>
        /// <returns>List of details</returns>
        EnumerableResponse<ReceiptDetail> GetDetail(int currentPageNumber, int pageSize, Expression<Func<ReceiptDetail, Boolean>> filter = null, OrderBy orderBy = null);

        /// <summary>
        /// Sets pointer to the current Detail
        /// </summary>
        /// <param name="currentDetail">The current detail.</param>
        /// <returns>Model with newly set detail</returns>
        ReceiptHeader SetDetail(ReceiptDetail detail);
    }
}

