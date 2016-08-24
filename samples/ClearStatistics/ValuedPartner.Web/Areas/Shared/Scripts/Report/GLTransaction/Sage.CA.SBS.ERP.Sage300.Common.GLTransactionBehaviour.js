/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */
"use strict";

var GLTransactionUI = GLTransactionUI || {};
GLTransactionUI = {
    hasKoApplied: false,
    isKendoControlNotInitialised: false,
    GLTransactionViewModel: {},
    ReportFormat: { Detail: 0, Summary: 1 },
    Module: { Bank: "BK", CommonServices: "CS", InventoryControl: "IC", OrderEntry: "OE", PostingOrder: "PO" },
    computedProperties: ["DisableReportControls", "IsSortByAmountVisible", "IsSortByDropDownVisible", "IsFinderVisible"],

    initGLTransaction: function () {

        GLTransactionUI.initButton();       
        onSuccess.getGLTransactionDetails(GLTransactionViewModel);
        if (GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() == GLTransactionUI.Module.InventoryControl || GLTransactionUI.Module.PostingOrder == GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() || GLTransactionUI.Module.OrderEntry == GLTransactionUI.GLTransactionViewModel.Data.SourceApplication()) {
            $("#divNumericgoGroup").removeClass('numeric-go-group');
            $("#divNumericgoGroup").addClass("numeric-group");
        }
       $('#Data_ThroughPostingSequence').siblings('input:visible').focus();
    },

    //Finder for Bank Module
    initFinder: function () {
        var bankPostingJournalTitle = jQuery.validator.format(GLTransactionResources.FinderTitle, GLTransactionResources.PostingSequence);
        sg.finderHelper.setFinder(
            "btnBankPostingJournalFinder",
            sg.finder.BankPostingJournalFinder,
            onSuccess.throughPostingSequence,
            $.noop,
            bankPostingJournalTitle,
            filters.throughPostingSequenceFilter,
            null, true
            );
    },

    initButton: function () {
        $("#btnPrintGLTransaction").click(function () {
            if ($("#GLTransactionForm").valid()) {
                $("#message").empty();

                var value = GLTransactionUI.getTextBoxValue("Data_ThroughPostingSequence");
                
                //always update the through posting sequence value with numeric textbox value.
                //since numeric textbox value sets to min value(1)|| number in text when invalid input is given(copy pasted text, value 0) but the model property is not updated 
                GLTransactionUI.GLTransactionViewModel.Data.ThroughPostingSequence(value);

                var data = sg.utls.ko.toJS(GLTransactionUI.GLTransactionViewModel.Data, GLTransactionUI.computedProperties);
                GLTransactionRepository.executeGLTransaction(data);
            }
        });
    },

    initDropDownList: function () {
        var fields = ["Data_ReportFormatList", "Data_SortByList", "Data_CurrencyTypeList"];
        $.each(fields, function (index, field) {
            sg.utls.kndoUI.dropDownList(field);
        });

        $("#Data_ReportFormatList").kendoDropDownList({
            change: function (e) {
                var reportFormat = GLTransactionUI.GLTransactionViewModel.Data.ReportFormat();
                if (reportFormat === GLTransactionUI.ReportFormat.Detail) {
                    GLTransactionUI.GLTransactionViewModel.Data.SortBy(0);
                    var dropdownlist = $("#Data_SortByList").data("kendoDropDownList");
                    dropdownlist.value(0);
                }
                else if (reportFormat === GLTransactionUI.ReportFormat.Summary) {
                    GLTransactionUI.GLTransactionViewModel.Data.SortBy(0);
                }
            }
        });
    },

    //To disable dropdowns for Bank Module
    disableDropDowns: function () {
        var fields = ["Data_ReportFormatList", "Data_SortByList", "Data_CurrencyTypeList"];
        $.each(fields, function (index, field) {
            var dropdownlist = $('#' + field).data("kendoDropDownList");
            dropdownlist.enable(false);
        });
    },

    //Numeric Text box for Bank Module
    initBKNumericTextbox: function () {
        $("#Data_ThroughPostingSequence").kendoNumericTextBox({
            format: "#",
            spinners: false,
            step: 0,
            decimals: 0,
            min: 1,
        }).data("kendoNumericTextBox");
        var throughPostingSequence = $("#Data_ThroughPostingSequence").data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(throughPostingSequence, 0, 9);
    },
    
    //Numeric Text box for IC Module
    initICNumericTextbox: function () {
        $("#Data_ThroughPostingSequence").kendoNumericTextBox({
            format: "n0",
            spinners: false,
            step: 0,
            decimals: 0,
            min: 1,
        }).data("kendoNumericTextBox");
        var throughPostingSequence = $("#Data_ThroughPostingSequence").data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(throughPostingSequence, 0, 15);
    }
    ,

    setTextBoxValue: function (id, value) {
        var numerictextbox = $('#' + id).data("kendoNumericTextBox");
        numerictextbox.value(value);
    },

    getTextBoxValue: function (id) {
        if (GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() == GLTransactionUI.Module.InventoryControl || GLTransactionUI.Module.PostingOrder == GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() || GLTransactionUI.Module.OrderEntry == GLTransactionUI.GLTransactionViewModel.Data.SourceApplication()) {
            
            return GLTransactionUI.GLTransactionViewModel.Data.ThroughPostingSequence();
        } else {
            var numerictextbox = $('#' + id).data("kendoNumericTextBox");
            return numerictextbox.value();
      }
      
    },

};

//Finder filters for Bank Module
var filters = {
    throughPostingSequenceFilter: function () {
        var filters = [[]];
        var sequenceNumber = GLTransactionUI.getTextBoxValue("Data_ThroughPostingSequence");
        filters[0][0] = sg.finderHelper.createFilter("PostingSequence", sg.finderOperator.Equal, sequenceNumber);
        filters[0][1] = sg.finderHelper.createFilter("TypeGorLBatchTransferred", sg.finderOperator.Equal, 0);
        filters[0][2] = sg.finderHelper.createFilter("TypePostingStatus", sg.finderOperator.NotEqual, 1);
        filters[0][1].IsMandatory = true;
        filters[0][2].IsMandatory = true;
        return filters;
    }
};

var onSuccess = {
    getGLTransactionDetails: function (result) {
        if (result != null) {

            if (!GLTransactionUI.hasKoApplied) {
                GLTransactionUI.GLTransactionViewModel = ko.mapping.fromJS(result);
                GLTransactionKoExtn.GLTransactionModelExtn(GLTransactionUI.GLTransactionViewModel.Data);
                ko.applyBindings(GLTransactionUI.GLTransactionViewModel);
                GLTransactionUI.hasKoApplied = true;
            }
            else {
                ko.mapping.fromJS(result, GLTransactionUI.GLTransactionViewModel);
                GLTransactionUI.initDropDownList();
            }

            if (!GLTransactionUI.isKendoControlNotInitialised) {
                GLTransactionUI.isKendoControlNotInitialised = true;
                GLTransactionUI.initDropDownList();
            }

            //For Bank Module, initialize finder and numeric textbox
            if (GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() === GLTransactionUI.Module.Bank) {
                GLTransactionUI.initFinder();
                GLTransactionUI.initBKNumericTextbox();

                //disable all controls and display information when necessary
                if (!result.UserMessage.IsSuccess) {
                    GLTransactionUI.disableDropDowns();
                    sg.utls.showMessageInfo(sg.utls.msgType.INFO, jQuery.validator.format(result.UserMessage.Message));
                }
            }
            
            if (GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() === GLTransactionUI.Module.InventoryControl || GLTransactionUI.Module.PostingOrder === GLTransactionUI.GLTransactionViewModel.Data.SourceApplication() || GLTransactionUI.Module.OrderEntry === GLTransactionUI.GLTransactionViewModel.Data.SourceApplication()) {
                GLTransactionUI.initFinder();
                GLTransactionUI.initICNumericTextbox();

                //disable all controls and display information when necessary
                if (!result.UserMessage.IsSuccess) {
                    GLTransactionUI.disableDropDowns();
                    sg.utls.showMessageInfo(sg.utls.msgType.INFO, jQuery.validator.format(result.UserMessage.Message));
                }
            }
        }
    },

    executeGLTransaction: function (result) {
        if (result != null && result.UserMessage.IsSuccess) {
            sg.utls.openReport(result.ReportToken);
        } else {
            sg.utls.showMessage(result);
        }
    },

    //Setting selected finder value to the model
    throughPostingSequence: function (result) {
        GLTransactionUI.GLTransactionViewModel.Data.ThroughPostingSequence(result.PostingSequence);
        GLTransactionUI.setTextBoxValue("Data_ThroughPostingSequence", result.PostingSequence);
    }

};

$(function () {
    GLTransactionUI.initGLTransaction();
    sg.utls.enableKendoDataAnnotations("GLTransactionForm");
});
