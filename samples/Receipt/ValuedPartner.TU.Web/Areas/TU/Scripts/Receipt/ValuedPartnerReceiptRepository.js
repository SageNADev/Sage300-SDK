// The MIT License (MIT) 
// Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved.
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

// @ts-check

/*
 * The following are global objects external to this source file
 */
/*global ko*/
/*global receiptUI*/
/*global receiptUISuccess*/
 
"use strict";

let receiptAjax = { 
    ConstructFindOptionsObject: function (searchFinder, simpleFilter) {
        let finderOptions = {
            SearchFinder: searchFinder,
            PageNumber: 0,
            PageSize: 1,
            SortAsc: false,
            SimpleFilter: null,
            AdvancedFilter: simpleFilter
        };

        return finderOptions;
    }
};

let receiptRepository = {

    /**
     * @function
     * @name receiptExists
     * @description Check for the existence of a receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {string} receiptNumber The receipt number
     * @param {object} model The model data
     * 
     * @returns {object} JSON object containing the results
     */
    receiptExists: function (receiptNumber, model) {
        let data = { 'id': receiptNumber, 'model': model };
        let result = null;
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Exists");
        sg.utls.ajaxPostSync(url, data, function (jsonResult) {
            result = jsonResult;
        });

        return result;
    },

    /**
     * @function
     * @name deleteDetail
     * @description Delete the details for an existing receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {number} pageNumber The page number
     * @param {number} pageSize The page size
     * @param {object} model The model data
     */
    deleteDetail: function (pageNumber, pageSize, model) {
        let data = { 'pageNumber': pageNumber, 'pageSize': pageSize, 'model': model }; 
        let url = sg.utls.url.buildUrl("TU", "Receipt", "DeleteDetails");
        sg.utls.ajaxPost(url, data, receiptUISuccess.DeleteDetailSucces);
    },

    /**
     * @function
     * @name create
     * @description Create a new receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {string} receiptNumber The receipt number
     */
    create: function (receiptNumber) {
        let data = { 'receiptNumber': receiptNumber }; 
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Create");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "create"));
    },

    /**
     * @function
     * @name deleteReceipt
     * @description Delete an existing receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {string} receiptNumber The receipt number
     * @param {number} sequenceNumber The sequence number
     */
    deleteReceipt: function (receiptNumber, sequenceNumber) {
        let data = { 'receiptNumber': receiptNumber, 'sequenceNumber': sequenceNumber };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Delete");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "delete"));
    },

    /**
     * @function
     * @name add
     * @description Add a new receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {object} data The model data
     */
    add: function (data) {
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Add");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "add"));
    },

    /**
     * @function
     * @name update
     * @description Update/Save an existing receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {object} data The model data
     */
    update: function (data) {
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Save");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "update")); 
    }, 

    /**
     * @function
     * @name getItemType
     * @description Get the item type
     * @namespace receiptRepository
     * @public
     *
     * @param {string} itemNumber The item number
     */
    getItemType: function (itemNumber) {
        let data = { 'itemId': itemNumber };
        let url = sg.utls.url.buildUrl("TU", "Item", "GetItemType");
        sg.utls.ajaxPost(url, data, receiptUISuccess.getItemTypeSuccess);
    },

    /**
     * @function
     * @name get
     * @description Get an existing receipt by receipt number
     * @namespace receiptRepository
     * @public
     *
     * @param {string} receiptNumber The receipt number
     * @param {boolean} disableScreen The disable screen flag
     */
    get: function (receiptNumber, disableScreen) {
        let data = { 'id': receiptNumber, 'oldRecordDeleted': false, 'isCalledAsPopup': disableScreen };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetById");
        sg.utls.ajaxPost(url, data, receiptUISuccess.getResult);
    },

    /**
     * @function
     * @name post
     * @description Post an existing receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {object} model The model data
     * @param {boolean} sequenceNumber The sequence number
     * @param {boolean} yesNo The yes/no flag
     */
    post: function (model, sequenceNumber, yesNo) {
        let data = {
            'headerModel':ko.mapping.toJS(model), 'sequenceNumber': sequenceNumber, 'yesNo': yesNo
        };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Post");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "post"));
    },

    /**
     * @function
     * @name checkDate
     * @description Validate the date of an existing receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {string} date The date
     * @param {string} eventType The event type
     */
    checkDate: function (date, eventType) {
        let data = { 'date': date, 'eventType': eventType };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "ValidateDate");
        sg.utls.ajaxPost(url, data, receiptUISuccess.checkDate);
    },

    /**
     * @function
     * @name getVendorDetails
     * @description Get the vendor details for a receipt
     * @namespace receiptRepository
     * @public
     * 
     * @param {string} vendorNumber The vendor number
     */
    getVendorDetails: function (vendorNumber) {
        let data = { 'vendorNumber': vendorNumber };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetVendorDetail");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.getVendorDetailsSuccess);
    },

    /**
     * @function
     * @name GetHeaderValues
     * @description Get the header values for a receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {object} modelData The model data
     * @param {object} eventType The event type
     */
    GetHeaderValues: function (modelData, eventType) {
        let data = { 'model': modelData, 'eventType': eventType };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetHeaderValues");
        sg.utls.ajaxPost(url, data, receiptUISuccess.GetHeaderValues);
    },

    /**
     * @function
     * @name refresh
     * @description Refresh the receipt data
     * @namespace receiptRepository
     * @public
     *
     * @param {object} model The model data
     */
    refresh: function (model) {
        let data = {
            model: ko.mapping.toJS(model)
        };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Refresh");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.refresh);
    },

    /**
     * @function
     * @name setOptionalFieldValue
     * @description Set an optional field value
     * @namespace receiptRepository
     * @public
     *
     * @param {object} model The model data
     */
    setOptionalFieldValue: function (model) {
        model.IsDetailOptionalField = false;
        let data = {
            model: model
        };
        let url = sg.utls.url.buildUrl("TU", "Receipt", receiptUI.isDetailOptionalField ? "SetOptionalFieldValue" : "SetHeaderOptFieldValue");
        sg.utls.ajaxPost(url, data, receiptUISuccess.setOptionalFieldValue);
    },

    /**
     * @function
     * @name checkRateSpread
     * @description Check the rate spread for a receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {string} rateType The rate type
     * @param {string} fromCurrency The 'From' currency
     * @param {date} rateDate The rate date
     * @param {number} rate The rate
     * @param {string} tocurrency The 'To' currency
     */
    checkRateSpread: function(rateType, fromCurrency, rateDate, rate, tocurrency) {
        let data = {
            'rateType': rateType,
            'fromCurrency': fromCurrency,
            'rateDate':sg.utls.kndoUI.getFormattedDate(rateDate),
            'rate': rate,
            'tocurrency': tocurrency
        };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "CheckRateSpread");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.getRateSpread);
    },

    /**
     * @function
     * @name GetExchangeRate
     * @description Get the exchange rate for a receipt
     * @namespace receiptRepository
     * @public
     *
     * @param {string} rateType The rate type
     * @param {string} fromCurrency The 'From' currency
     * @param {date} rateDate The rate date
     * @param {number} rate The rate
     * @param {string} tocurrency The 'To' currency
     */
    GetExchangeRate: function (rateType, fromCurrency, rateDate, rate, tocurrency) {
        let data = {
            'rateType': rateType,
            'fromCurrency': fromCurrency,
            'rateDate':sg.utls.kndoUI.getFormattedDate(rateDate),
            'rate': rate,
            'tocurrency': tocurrency
        };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "CheckRateSpread");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.GetExchangeRate);
    }, 

    /**
     * @function
     * @name saveReceiptDetails
     * @description Save the receipt details
     * @namespace receiptRepository
     * @public
     *
     * @param {object} model The model data
     */
    saveReceiptDetails: function (model) {
        let data = {
            model: ko.mapping.toJS(model)
        }
        let url = sg.utls.url.buildUrl("TU", "Receipt", "SaveDetails");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.onSaveDetailsCompleted);
    }
}
