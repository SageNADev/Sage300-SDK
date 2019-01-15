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

// @ts-check

"use strict";

var sourceJournalProfileReportUI = sourceJournalProfileReportUI || {};

sourceJournalProfileReportUI = {
    sourceJournalProfileReportModel: {},
    init: function () {
        sourceJournalProfileReportOnSucess.display(sourceJournalProfileReportModel);
        sourceJournalProfileReportUI.initButton();
        sourceJournalProfileReportUI.initFinders();

        sg.controls.Focus($("#Data_Frjrnl"));
    },

    initFinders: function () {
        var info = sg.viewFinderProperties.GL.SourceJournalProfiles;
        var buttonId = "btnFromSrcJournalFinder";
        var dataControlIdOrSuccessCallback = onFinderSuccess.FromProfile;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, finderFilter.getFromProfileFilter);

        var buttonId = "btnToSrcJournalFinder";
        var dataControlIdOrSuccessCallback = onFinderSuccess.ToProfile;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, finderFilter.getToProfileFilter);
    },

    initKendoBindings: function () {
        sourceJournalProfileReportUI.sourceJournalProfileReportModel = ko.mapping.fromJS(sourceJournalProfileReportViewModel);
    },

    initButton: function () {
        $("#btnPrint").click(function () {
            if ($("#frmSourceJournalProfileReport").valid()) {
                sg.utls.clearValidations("frmSourceJournalProfileReport");
                var model = sourceJournalProfileReportUI.sourceJournalProfileReportModel.Data;

                var errorMessage = null;
                if (model.Frjrnl() > model.Tojrnl()) {
                    errorMessage = sourceJournalProfileReportResources.FromToErrorMessage;
                    sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(sourceJournalProfileReportResources.FromToErrorMessage, sourceJournalProfileReportResources.Profile));
                }

                setTimeout(function () {
                    if (!errorMessage) {
                        $("#message").empty();
                        var data = sg.utls.ko.toJS(model, sourceJournalProfileReportUI.computedProperties);
                        SourceJournalProfileReportRepository.executeSourceJournalProfileReportRepositoryReport(model);
                    }

                }, 150);
            }
        })
    },
}


var onFinderSuccess = {

    FromProfile: function (result) {
        if (result != null) {
            var sourceJournalName = result.SRCEJRNL;
            sourceJournalProfileReportUI.sourceJournalProfileReportModel.Data.Frjrnl(sourceJournalName);
            $("#Data_Frjrnl").val(sourceJournalName);
            sg.controls.Focus($("#Data_Tojrnl"));
        }
    },

    ToProfile: function (result) {
        if (result != null) {
            var sourceJournalName = result.SRCEJRNL;
            sourceJournalProfileReportUI.sourceJournalProfileReportModel.Data.Tojrnl(sourceJournalName);
            $("#Data_Tojrnl").val(sourceJournalName);
            sg.controls.Focus($("#btnPrint"));
        }
    }
};

var finderFilter = {
    getFromProfileFilter: function () {
        var filters = [[]];
        var documentType = $("#Data_Frjrnl").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceJournalName", sg.finderOperator.StartsWith, documentType);

        return filters;
    },
    getToProfileFilter: function () {
        var filters = [[]];
        var documentType = $("#Data_Tojrnl").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceJournalName", sg.finderOperator.StartsWith, documentType);

        return filters;
    },
}
var sourceJournalProfileReportOnSucess = {
    display: function (result) {
        if (result != null) {
            if (!sourceJournalProfileReportUI.hasKoApplied) {
                sourceJournalProfileReportUI.sourceJournalProfileReportModel = window.ko.mapping.fromJS(result);
                sourceJournalProfileReportKoExtn.sourceJournalProfileReportExtension(sourceJournalProfileReportUI.sourceJournalProfileReportModel);
                ko.applyBindings(sourceJournalProfileReportUI.sourceJournalProfileReportModel);

                sourceJournalProfileReportUI.hasKoApplied = true;
            } else {
                ko.mapping.fromJS(result, sourceJournalProfileReportUI.sourceJournalProfileReportModel);
            }
        }
    },
    executeSourceJournalProfileReport: function (result) {
        if (result != null && result.UserMessage.IsSuccess) {
            sg.utls.openReport(result.ReportToken);
        } else {
            sg.utls.showMessage(result);
        }
    }
};

$(function () {
    sourceJournalProfileReportUI.init();
});