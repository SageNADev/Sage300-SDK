// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

"use strict";

var clearStatisticsRepository = (function (parent) {
    const MODULEID = "TU";
    const ACTION = "ClearStatistics";

    function _commonHandler(year, method, defaultCallback, overrideCallback = null) {
        var data = {'year': year };

        var callback = defaultCallback;
        if (overrideCallback !== null) {
            callback = overrideCallback;
        }

        sg.utls.ajaxPostHtml(sg.utls.url.buildUrl(MODULEID, ACTION, method), data, callback);
    }

    // Publicly exposed methods
    return {
        getCustomerMaxPeriodForValidYear: function(year, successCallback = null) {
        _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillCustomerFiscalYear, successCallback);
        },

        getGroupMaxPeriodForValidYear: function(year, successCallback = null) {
            _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillGroupFiscalYear, successCallback);
        },

        getNationalAcctMaxPeriodForValidYear: function(year, successCallback = null) {
            _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillNationalAcctFiscalYear, successCallback);
        },

        getSalespersonMaxPeriodForValidYear: function(year, successCallback = null) {
            _commonHandler(year, "GetSalesPersonMaxPeriodForValidYear", clearStatisticsUISuccess.fillSalespersonFiscalYear, successCallback);
        },

        getItemMaxPeriodForValidYear: function(year, successCallback = null) {
            _commonHandler(year, "GetItemMaxPeriodForValidYear", clearStatisticsUISuccess.fillItemFiscalYear, successCallback);
        }
    };
})(clearStatisticsRepository || {});
