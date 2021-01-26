// The MIT License (MIT) 
// Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved.
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
     * @name isExists
     * @description TODO - Add descripton
     * @public
     * 
     * @param {string} receiptNumber TODO - Add description
     * @param {object} model TODO - Add description
     * 
     * @returns {object} TODO - Add description
     */
    isExists: function (receiptNumber, model) {
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
     * @description TODO - Add description
     * @public
     * 
     * @param {number} pageNumber TODO - Add description
     * @param {number} pageSize TODO - Add description
     * @param {object} model TODO - Add description
     */
    deleteDetail: function (pageNumber, pageSize, model) {
        let data = { 'pageNumber': pageNumber, 'pageSize': pageSize, 'model': model }; 
        let url = sg.utls.url.buildUrl("TU", "Receipt", "DeleteDetails");
        sg.utls.ajaxPost(url, data, receiptUISuccess.DeleteDetailSucces);
    },

    /**
     * @function
     * @name create
     * @description TODO - Add description
     * @public
     * 
     * @param {string} receiptNumber TODO - Add description
     */
    create: function (receiptNumber) {
        let data = { 'receiptNumber': receiptNumber }; 
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Create");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "create"));
    },

    /**
     * @function
     * @name deleteReceipt
     * @description TODO - Add description
     * @public
     * 
     * @param {string} receiptNumber TODO - Add description
     * @param {number} sequenceNumber TODO - Add description
     */
    deleteReceipt: function (receiptNumber, sequenceNumber) {
        let data = { 'receiptNumber': receiptNumber, 'sequenceNumber': sequenceNumber };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Delete");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "delete"));
    },

    /**
     * @function
     * @name add
     * @description TODO - Add description
     * @public
     *
     * @param {object} data TODO - Add description
     */
    add: function (data) {
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Add");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "add"));
    },

    /**
     * @function
     * @name update
     * @description TODO - Add description
     * @public
     *
     * @param {object} data TODO - Add description
     */
    update: function (data) {
        let url = sg.utls.url.buildUrl("TU", "Receipt", "Save");
        sg.utls.ajaxPost(url, data, receiptUISuccess.actionSuccess.bind(this, "update")); 
    }, 

    /**
     * @function
     * @name getItemType
     * @description TODO - Add description
     * @public
     *
     * @param {string} itemNumber TODO - Add description
     */
    getItemType: function (itemNumber) {
        let data = { 'itemId': itemNumber };
        let url = sg.utls.url.buildUrl("TU", "Item", "GetItemType");
        sg.utls.ajaxPost(url, data, receiptUISuccess.getItemTypeSuccess);
    },

    /**
     * @function
     * @name get
     * @description TODO - Add description
     * @public
     *
     * @param {string} receiptNumber TODO - Add description
     * @param {boolean} disableScreen TODO - Add description
     */
    get: function (receiptNumber, disableScreen) {
        let data = { 'id': receiptNumber, 'oldRecordDeleted': false, 'isCalledAsPopup': disableScreen };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetById");
        sg.utls.ajaxPost(url, data, receiptUISuccess.getResult);
    },

    /**
     * @function
     * @name post
     * @description TODO - Add description
     * @public
     *
     * @param {object} model TODO - Add description
     * @param {boolean} sequenceNumber TODO - Add description
     * @param {boolean} yesNo TODO - Add description
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
     * @description TODO - Add description
     * @public 
     * 
     * @param {string} date TODO - Add description
     * @param {string} eventType TODO - Add description
     */
    checkDate: function (date, eventType) {
        let data = { 'date': date, 'eventType': eventType };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "ValidateDate");
        sg.utls.ajaxPost(url, data, receiptUISuccess.checkDate);
    },

    /**
     * @function
     * @name getVendorDetails
     * @description TODO - Add description
     * @public
     * 
     * @param {string} id TODO - Add description
     */
    getVendorDetails: function (id) {
        let data = { 'vendorNumber': id };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetVendorDetail");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.getVendorDetailsSuccess);
    },

    /**
     * @function
     * @name GetHeaderValues
     * @description TODO - Add description
     * @public
     *
     * @param {object} modelData TODO - Add description
     * @param {object} eventType TODO - Add description
     */
    GetHeaderValues: function (modelData, eventType) {
        let data = { 'model': modelData, 'eventType': eventType };
        let url = sg.utls.url.buildUrl("TU", "Receipt", "GetHeaderValues");
        sg.utls.ajaxPost(url, data, receiptUISuccess.GetHeaderValues);
    },

    /**
     * @function
     * @name refresh
     * @description TODO - Add description
     * @public
     *
     * @param {object} model TODO - Add description
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
     * @description TODO - Add description
     * @public
     *
     * @param {object} model TODO - Add description
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
     * @description TODO - Add description
     * @public
     *
     * @param {string} rateType TODO - Add description
     * @param {string} fromCurrency TODO - Add description
     * @param {date} rateDate TODO - Add description
     * @param {number} rate TODO - Add description
     * @param {string} tocurrency TODO - Add description
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
     * @description TODO - Add description
     * @public
     *
     * @param {string} rateType TODO - Add description
     * @param {string} fromCurrency TODO - Add description
     * @param {date} rateDate TODO - Add description
     * @param {number} rate TODO - Add description
     * @param {string} tocurrency TODO - Add description
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
     * @description TODO - Add description
     * @public
     *
     * @param {object} model TODO - Add description
     */
    saveReceiptDetails: function (model) {
        let data = {
            model: ko.mapping.toJS(model)
        }
        let url = sg.utls.url.buildUrl("TU", "Receipt", "SaveDetails");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.onSaveDetailsCompleted);
    }
}
