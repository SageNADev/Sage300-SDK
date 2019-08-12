// The MIT License (MIT) 
// Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved.
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

var receiptAjax = { 
    ConstructFindOptionsObject: function (searchFinder, simpleFilter) {
        var finderOptions = {
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

var receiptRepository = {
    isExists: function (receiptNumber, model) {
        var data = { 'id': receiptNumber, 'model': model };
        var result = null;

        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "Exists"), data, function (jsonResult) {
            result = jsonResult;
        });

        return result;
    },

    deleteDetail: function (pageNumber, pageSize, model) {
        var data = { 'pageNumber': pageNumber, 'pageSize': pageSize, 'model': model }; 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "DeleteDetails"), data, receiptUISuccess.DeleteDetailSucces);
    },

    create: function (receiptNumber) {
        var data = { 'receiptNumber': receiptNumber }; 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Create"), data, receiptUISuccess.actionSuccess.bind(this, "create"));
    },

    deleteReceipt: function (receiptNumber, sequenceNumber) {
        var data = { 'receiptNumber': receiptNumber, 'sequenceNumber': sequenceNumber };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Delete"), data, receiptUISuccess.actionSuccess.bind(this, "delete"));
    },

    add: function (data) {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Add"), data, receiptUISuccess.actionSuccess.bind(this, "add"));
    },

    update: function (data) { 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Save"), data, receiptUISuccess.actionSuccess.bind(this, "update")); 
    }, 

    getItemType: function (itemNumber) {
        var data = { 'itemId': itemNumber };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Item", "GetItemType"), data, receiptUISuccess.getItemTypeSuccess);
    },


    get: function (receiptNumber,disableScreen) {
        var data = { 'id': receiptNumber, 'oldRecordDeleted': false, 'isCalledAsPopup': disableScreen };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "GetById"), data, receiptUISuccess.getResult);
    },

    post: function (model,sequenceNumber, yesNo) {
        var data = {
            'headerModel':ko.mapping.toJS(model), 'sequenceNumber': sequenceNumber, 'yesNo': yesNo
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Post"), data, receiptUISuccess.actionSuccess.bind(this, "post"));
    },

    checkDate: function (date, eventType) {
        var data = { 'date': date, 'eventType': eventType };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "ValidateDate"), data, receiptUISuccess.checkDate);
    },

    getVendorDetails: function (id) {
        var data = { 'vendorNumber': id };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "GetVendorDetail"), data, receiptUISuccess.getVendorDetailsSuccess);
    },

    GetHeaderValues: function (data, eventType) {
        var data = { 'model': data, 'eventType': eventType };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "GetHeaderValues"), data, receiptUISuccess.GetHeaderValues);
    },

    refresh: function (model) {
        var data = {
            model: ko.mapping.toJS(model)
        };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "Refresh"), data, receiptUISuccess.refresh);
    },

    setOptionalFieldValue: function (model) {
        model.IsDetailOptionalField = false;
        var data = {
            model: model
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", receiptUI.isDetailOptionalField ? "SetOptionalFieldValue" : "SetHeaderOptFieldValue"), data, receiptUISuccess.setOptionalFieldValue);
    },
     
    checkRateSpread: function(rateType, fromCurrency, rateDate, rate, tocurrency) {
        var data = {
            'rateType': rateType,
            'fromCurrency': fromCurrency,
            'rateDate':sg.utls.kndoUI.getFormattedDate(rateDate),
            'rate': rate,
            'tocurrency': tocurrency
        };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "CheckRateSpread"), data, receiptUISuccess.getRateSpread);
    },

    GetExchangeRate: function (rateType, fromCurrency, rateDate, rate, tocurrency) {
        var data = {
            'rateType': rateType,
            'fromCurrency': fromCurrency,
            'rateDate':sg.utls.kndoUI.getFormattedDate(rateDate),
            'rate': rate,
            'tocurrency': tocurrency
        };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "CheckRateSpread"), data, receiptUISuccess.GetExchangeRate);
    }, 
    
    saveReceiptDetails: function (model) {
        var data = {
            model: ko.mapping.toJS(model)
        }
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "SaveDetails"), data, receiptUISuccess.onSaveDetailsCompleted);
    }
}