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
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "DeleteDetails"), data, receiptUISuccess.deleteDetailsSuccess);
    },

    create: function (receiptNumber) {
        var data = { 'receiptNumber': receiptNumber }; 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Create"), data, receiptUISuccess.createSuccess);
    },

    deleteReceipt: function (receiptNumber, sequenceNumber) {
        var data = { 'receiptNumber': receiptNumber, 'sequenceNumber': sequenceNumber };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Delete"), data, receiptUISuccess.deleteSuccess);
    },

    add: function (data) {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Add"), data, receiptUISuccess.addSuccess);
    },

    update: function (data) { 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Save"), data, receiptUISuccess.updateSuccess);
    }, 

    getItemType: function (itemNumber) {
        var data = { 'itemId': itemNumber };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("IC", "Item", "GetItemType"), data, receiptUISuccess.getItemTypeSuccess);
    },

    getDefaultItemNumber: function (manufacturerItemNumber, callback) {
        var data = {
            'manufacturerItemNo': manufacturerItemNumber
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "ManufacturersItem", "GetDefaultItemNumber"), data, callback);
    },

    get: function (receiptNumber,disableScreen) {
        var data = { 'id': receiptNumber, 'oldRecordDeleted': false, 'isCalledAsPopup': disableScreen };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "GetById"), data, receiptUISuccess.getResult);
    },

    post: function (model,sequenceNumber, yesNo) {
        var data = {
            'headerModel':ko.mapping.toJS(model), 'sequenceNumber': sequenceNumber, 'yesNo': yesNo
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "Post"), data, receiptUISuccess.postSuccess);
    },

    checkDate: function (date) {
        var data = { 'date': date };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "ValidateDate"), data, receiptUISuccess.checkDate);
    },

    getVendorDescription: function (id) {
        if (id != null && id.length > 0) {
            var getfilter = sg.finderHelper.createDefaultFunction("Data_VendorNumber", "VendorNumber", sg.finderOperator.StartsWith);
            if (getfilter) {
                var filter = getfilter();
            }
            var data = { finderOptions: receiptAjax.ConstructFindOptionsObject(sg.finder.Vendor, filter) };
            var url = sg.utls.url.buildUrl("Core", "Find", "RefreshGrid");
            sg.utls.ajaxPostSync(url, data, function (result) {
                var description = (result && result.Data.length) ? result.Data[0].ShortName : ""; 
                if (description) {
                    receiptUI.receiptModel.Data.VendorShortName(description);
                    $("#Data_VendorName").val(description);
                } 
                if (result.Data[0].RateType) {
                    receiptUI.receiptModel.Data.RateType(result.Data[0].RateType);
                    $("#Data_RateType").val(result.Data[0].RateType);
                } 
            });
        }
    },

    getVendorDetails: function (id) {
        var data = { 'id': id };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "GetVendorDetail"), data, receiptUISuccess.getVendorDetailsSuccess);
    },

    getReceiptCurrencyDescription: function (id) {
        if (id != null && id.length > 0) {
            var getfilter = sg.finderHelper.createDefaultFunction("Data_ReceiptCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith);
            if (getfilter) {
                var filter = getfilter();
            }

            var data = { finderOptions: receiptAjax.ConstructFindOptionsObject(sg.finder.CurrencyCode, filter) };
            var url = sg.utls.url.buildUrl("Core", "Find", "RefreshGrid");
            sg.utls.ajaxPostSync(url, data, receiptUISuccess.ReceiptCurrencyResult);
        }
    },

    getCostCurrencyDescription: function (id) {
        if (id != null && id.length > 0) {
            var getfilter = sg.finderHelper.createDefaultFunction("Data_AdditionalCostCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith);
            if (getfilter) {
                var filter = getfilter();
            }

            var data = { finderOptions: receiptAjax.ConstructFindOptionsObject(sg.finder.CurrencyCode, filter) };
            var url = sg.utls.url.buildUrl("Core", "Find", "RefreshGrid");
            sg.utls.ajaxPostSync(url, data, receiptUISuccess.CostCurrencyResult);
        }
    },

    getUnitOfMeasure: function (itemNumber, value) {
        var filters = [[]];
        filters[0][0] = sg.finderHelper.createFilter("ItemNumber", window.sg.finderOperator.Equal, itemNumber);
        filters[0][1] = sg.finderHelper.createFilter("UnitOfMeasure", window.sg.finderOperator.Equal, value);
        var finderOptions = {
            SearchFinder: sg.finder.ICItemUnitOfMeasure,
            PageNumber: 0,
            PageSize: sg.utls.gridPageSize,
            SortAsc: false,
            AdvancedFilter: filters
        };
        var data = { finderOptions: finderOptions };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Find", "RefreshGrid"), data, receiptUISuccess.unitOfMeasure);
    }, 

    getHeaderValues: function (data, eventType) {
        data = { 'model': data, 'eventType': eventType };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "GetHeaderValues"), data, receiptUISuccess.getHeaderValues);
    },

    setItemGridValuesModel: function (data, eventType) { 
        data = { 'model': data, 'eventType': eventType };
        var url = sg.utls.url.buildUrl("TU", "Receipt", "GetRowValues");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.getItemValuesResult);
    }, 

    setItemValues: function (data, eventType) { 
        data = { 'model': data, 'eventType': eventType };
        var url = sg.utls.url.buildUrl("TU", "Receipt", "GetRowValues");
        sg.utls.ajaxPostSync(url, data, receiptUISuccess.getItemValues);
    }, 

    getOptionalFieldFinderData: function (optionalField) {
        var data = { 'optionalField': optionalField.OptionalField };
        window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl("TU", "Receipt", "GetOptionalFieldFinderData"), data, receiptUISuccess.fillOptionalFieldFinderData);
    },
    
    getDetailOptionalFieldFinderData: function (optionalField) {
        var data = { 'optionalField': optionalField.OptionalField };
        window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl("TU", "Receipt", "GetDetailOptFieldFinderData"), data, receiptUISuccess.fillOptionalFieldFinderData);
    }, 

    saveOptionalFields: function (modelData) {
        var data = {
            model: ko.mapping.toJS(modelData),
            receiptNumber: receiptUI.receiptModel.Data.ReceiptNumber(),
            isDetail:receiptUI.isDetailOptionalField
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "SaveDetailOptFields"), data, receiptUISuccess.saveOptionalFields);
    },

    setDetail: function (modelData) {
        if (modelData) {
            var data = {
                model: ko.mapping.toJS(modelData)
            };
            sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "SetDetail"), data, receiptUISuccess.setDetail);
        }

    },

    refreshDetail: function (modelData) {
        var data = {
            model: ko.mapping.toJS(modelData)
        };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "RefreshDetail"), data, receiptUISuccess.refreshDetail);
    },

    refreshOptField: function () {
        var data = {};
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "RefreshOptField"), data, function (result) {
            if (result) {
                receiptUI.receiptModel.Data.OptionalFields(result);
            }
        });
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
    
    readHeader: function (model, setHeaderValue) {
        var data = { 'model': ko.mapping.toJS(model), 'setHeaderValue': setHeaderValue }; 
        sg.utls.ajaxPost(sg.utls.url.buildUrl("TU", "Receipt", "ReadHeader"), data, receiptUISuccess.readHeader);
    },

    saveReceiptDetails: function (model) {
        var data = {
            model: ko.mapping.toJS(model)
        };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "SaveDetails"), data, receiptUISuccess.onSaveDetailsCompleted);
    },

    saveDetailOnChange: function (detail) {
        var data = { model: detail };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("TU", "Receipt", "SaveDetail"), data, receiptUISuccess.refreshReceiptDetail);
    }
}