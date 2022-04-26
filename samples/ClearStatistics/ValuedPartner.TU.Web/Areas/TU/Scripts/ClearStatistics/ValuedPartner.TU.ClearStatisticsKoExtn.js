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

var tuClearStatisticsKoExtn = ((self) => {

    /**
     * @name convertToBoolean
     * @description Converts a value into a boolean type
     * @private
     * 
     * @param {any} value The value to convert to boolean
     * 
     * @returns The converted boolean value
     */
    function convertToBoolean(value) {
        switch (value.toString().toLowerCase()) {
            case "true": case "yes": case "1": return true;
            case "false": case "no": case "0": case null: return false;
            default: return Boolean(value);
        }
    }

    // Publicly exposed objects
    return {

        /**
         * @name tuClearStatisticsModelExtension
         * @description
         * @public
         * 
         * @param {Object} model
         */
        tuClearStatisticsModelExtension: (model) => {
            let modelData = model.Data;
            modelData.Year = ko.observable("");

            modelData.IsKoStatisticsDirty = new ko.dirtyFlag(model);

            modelData.bClearCustomerStatistics = ko.computed({
                read: () => {
                    // return original value
                    return convertToBoolean(model.Data.ClearCustomerStatistics());
                },
                write: (newValue) => {
                    // convert boolean to appropriate enum and assign
                    var parsedValue = newValue ? 1 : 0;
                    model.Data.ClearCustomerStatistics(parsedValue);
                },
                owner: this
            });

            modelData.bClearGroupStatistics = ko.computed({
                read: () => {
                    // return original value
                    return convertToBoolean(model.Data.ClearGroupStatistics());
                },
                write: (newValue) => {
                    // convert boolean to appropriate enum and assign
                    var parsedValue = newValue ? 1 : 0;
                    model.Data.ClearGroupStatistics(parsedValue);
                },
                owner: this
            });

            modelData.bClearNationalAcctStatistics = ko.computed({
                read: () => {
                    // return original value
                    return convertToBoolean(model.Data.ClearNationalAcctStatistics());
                },
                write: (newValue) => {
                    //convert boolean to appropriate enum and assign
                    var parsedValue = newValue ? 1 : 0;
                    model.Data.ClearNationalAcctStatistics(parsedValue);
                },
                owner: this
            });

            modelData.bClearSalespersonStatistics = ko.computed({
                read: () => {
                    // return original value
                    return convertToBoolean(model.Data.ClearSalesPersonStatistics());
                },
                write: (newValue) => {
                    // convert boolean to appropriate enum and assign
                    var parsedValue = newValue ? 1 : 0;
                    model.Data.ClearSalesPersonStatistics(parsedValue);
                },
                owner: this
            });

            modelData.bClearItemStatistics = ko.computed({
                read: () => {
                    // return original value
                    return convertToBoolean(model.Data.ClearItemStatistics());
                },
                write: (newValue) => {
                    // convert boolean to appropriate enum and assign
                    var parsedValue = newValue ? 1 : 0;
                    model.Data.ClearItemStatistics(parsedValue);
                },
                owner: this
            });
        }
    };

})(tuClearStatisticsKoExtn || {});