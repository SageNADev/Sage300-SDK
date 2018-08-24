// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.
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