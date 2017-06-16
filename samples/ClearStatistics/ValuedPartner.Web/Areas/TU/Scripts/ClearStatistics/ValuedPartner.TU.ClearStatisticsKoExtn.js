// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

var tuClearStatisticsKoExtn = tuClearStatisticsKoExtn || {};

tuClearStatisticsKoExtn = {
    tuClearStatisticsModelExtension: function (modelData) {
        var model = modelData.Data;
        model.Year = ko.observable("");

        model.IsKoStatisticsDirty = new ko.dirtyFlag(clearStatisticsUI.clearStatisticsModel);

        model.bClearCustomerStatistics = ko.computed({
            read: function () {
                //return original value
                return tuClearStatisticsKoExtn.convertToBoolean(clearStatisticsUI.clearStatisticsModel.Data.ClearCustomerStatistics());
            },
            write: function (newValue) {
                //convert boolean to appropriate enum and assign
                var parsedValue = newValue ? 1 : 0;
                clearStatisticsUI.clearStatisticsModel.Data.ClearCustomerStatistics(parsedValue);
            },
            owner: this
        });

        model.bClearGroupStatistics = ko.computed({
            read: function () {
                //return original value
                return tuClearStatisticsKoExtn.convertToBoolean(clearStatisticsUI.clearStatisticsModel.Data.ClearGroupStatistics());
            },
            write: function (newValue) {
                //convert boolean to appropriate enum and assign
                var parsedValue = newValue ? 1 : 0;
                clearStatisticsUI.clearStatisticsModel.Data.ClearGroupStatistics(parsedValue);
            },
            owner: this
        });

        model.bClearNationalAcctStatistics = ko.computed({
            read: function () {
                //return original value
                return tuClearStatisticsKoExtn.convertToBoolean(clearStatisticsUI.clearStatisticsModel.Data.ClearNationalAcctStatistics());
            },
            write: function (newValue) {
                //convert boolean to appropriate enum and assign
                var parsedValue = newValue ? 1 : 0;
                clearStatisticsUI.clearStatisticsModel.Data.ClearNationalAcctStatistics(parsedValue);
            },
            owner: this
        });

        model.bClearSalespersonStatistics = ko.computed({
            read: function () {
                //return original value
                return tuClearStatisticsKoExtn.convertToBoolean(clearStatisticsUI.clearStatisticsModel.Data.ClearSalesPersonStatistics());
            },
            write: function (newValue) {
                //convert boolean to appropriate enum and assign
                var parsedValue = newValue ? 1 : 0;
                clearStatisticsUI.clearStatisticsModel.Data.ClearSalesPersonStatistics(parsedValue);
            },
            owner: this
        });

        model.bClearItemStatistics = ko.computed({
            read: function () {
                //return original value
                return tuClearStatisticsKoExtn.convertToBoolean(clearStatisticsUI.clearStatisticsModel.Data.ClearItemStatistics());
            },
            write: function (newValue) {
                //convert boolean to appropriate enum and assign
                var parsedValue = newValue ? 1 : 0;
                clearStatisticsUI.clearStatisticsModel.Data.ClearItemStatistics(parsedValue);
            },
            owner: this
        });    
    },

    /*convert value to bool (True/False)*/
    convertToBoolean: function (value) {
        switch (value.toString().toLowerCase()) {
            case "true": case "yes": case "1": return true;
            case "false": case "no": case "0": case null: return false;
            default: return Boolean(value);
        }
    }
};