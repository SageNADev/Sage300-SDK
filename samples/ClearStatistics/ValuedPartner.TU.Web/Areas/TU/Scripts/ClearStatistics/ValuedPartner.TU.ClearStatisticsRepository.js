// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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

"use strict";

var clearStatisticsRepository = ((parent) => {

    // Private method
    function _commonHandler(year, method, defaultCallback, overrideCallback = null) {
        let data = {'year': year };

        let callback = defaultCallback;
        if (overrideCallback !== null) {
            callback = overrideCallback;
        }

        let url = sg.utls.url.buildUrl(clearStatisticsConstants.MODULEID, clearStatisticsConstants.ACTION, method);
        sg.utls.ajaxPostHtml(url, data, callback);
    }

    // Publicly exposed objects
    return {
        /**
         * @name getCustomerMaxPeriodForValidYear
         * @description
         * @public
         * 
         * @param {Number} year
         * @param {Function} successCallback = null
         */
        getCustomerMaxPeriodForValidYear: (year, successCallback = null) => {
            _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillCustomerFiscalYear, successCallback);
        },

        /**
         * @name getGroupMaxPeriodForValidYear
         * @description
         * @public
         *
         * @param {Number} year
         * @param {Function} successCallback = null
         */
        getGroupMaxPeriodForValidYear: (year, successCallback = null) => {
            _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillGroupFiscalYear, successCallback);
        },

        /**
         * @name getNationalAcctMaxPeriodForValidYear
         * @description
         * @public
         *
         * @param {Number} year
         * @param {Function} successCallback = null
         */
        getNationalAcctMaxPeriodForValidYear: (year, successCallback = null) => {
            _commonHandler(year, "GetMaxPeriodForValidYear", clearStatisticsUISuccess.fillNationalAcctFiscalYear, successCallback);
        },

        /**
         * @name getSalespersonMaxPeriodForValidYear
         * @description
         * @public
         *
         * @param {Number} year
         * @param {Function} successCallback = null
         */
        getSalespersonMaxPeriodForValidYear: (year, successCallback = null) => {
            _commonHandler(year, "GetSalesPersonMaxPeriodForValidYear", clearStatisticsUISuccess.fillSalespersonFiscalYear, successCallback);
        },

        /**
         * @name getItemMaxPeriodForValidYear
         * @description
         * @public
         *
         * @param {Number} year
         * @param {Function} successCallback = null
         */
        getItemMaxPeriodForValidYear: (year, successCallback = null) => {
            _commonHandler(year, "GetItemMaxPeriodForValidYear", clearStatisticsUISuccess.fillItemFiscalYear, successCallback);
        }
    };
})(clearStatisticsRepository || {});
