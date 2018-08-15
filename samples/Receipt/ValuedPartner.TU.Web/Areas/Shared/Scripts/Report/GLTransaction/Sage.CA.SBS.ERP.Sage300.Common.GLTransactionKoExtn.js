// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
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

var GLTransactionKoExtn = GLTransactionKoExtn || {};
GLTransactionKoExtn = {
    GLTransactionModelExtn: function (model) {

        //Disable all report controls for Bank Module
        model.DisableReportControls = ko.computed(function () {
            if (!GLTransactionUI.hasKoApplied) {
                if (model.SourceApplication() === GLTransactionUI.Module.Bank && model.ThroughPostingSequence() === 0) {
                    return true;
                }
                return false;
            }
        });

        model.IsSortByAmountVisible = ko.computed(function () {

            if (model.ReportFormat() === GLTransactionUI.ReportFormat.Summary) {
                return true;
            } else {
                return false;
            }
        });

        model.IsSortByDropDownVisible = ko.computed(function () {

            if (model.ReportFormat() === GLTransactionUI.ReportFormat.Detail) {
                return true;
            } else {
                return false;
            }
        });

        //Display the finder for Bank Module
        model.IsFinderVisible = ko.computed(function () {

            if (model.SourceApplication() === GLTransactionUI.Module.Bank) {
                return true;
            } else {
                return false;
            }
        });

    }
};