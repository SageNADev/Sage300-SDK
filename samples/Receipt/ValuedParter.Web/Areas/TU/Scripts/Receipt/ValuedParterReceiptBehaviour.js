// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

/*global receiptRepository*/
/*global receiptResources*/
/*global ko*/
/*global kendo*/
/*global globalResource*/
/*global optionalFieldUIGrid*/
/*global GridPreferencesHelper*/
/*global optionalFieldsResources*/
/*global LabelMenuHelper*/
/*global GridPreferences*/
/*global receiptViewModel*/
/*global receiptDetailFields*/
/*global receiptUserPreferences*/
/*global receiptObservableExtension*/

"use strict";

//Detail Columns Index 
var receiptDetailColumnIndex = {
    get ItemNumber() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "ItemNumber"); },
    get Location() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "Location"); },
    get QuantityReceived() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "QuantityReceived"); },
    get UnitOfMeasure() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "UnitOfMeasure"); },
    get UnitCost() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "UnitCost"); },
    get ExtendedCost() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "ExtendedCost"); },
    get Labels() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "Labels"); },
    get Comments() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "Comments"); },
    get ManufacturersItemNumber() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "ManufacturersItemNumber"); },
    get OptionalFields() {
        return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "OptionalFieldString");
    },
    get Category() { return window.GridPreferencesHelper.getColumnIndex('#DetailOptionalFieldGrid', "Category"); },
    FirstEditableColumn: 2
};

var receiptUI = receiptUI || {};
var receiptTypeEnum = {
    Text: 1,
    Amount: 100,
    Number: 6,
    Integer: 8,
    YesOrNo: 9,
    Date: 3,
    Time: 4
};

var dateChanged = {
    receiptDate: 1,
    postingDate: 2
};

var gridRowFields = {
    ItemNumber: 3,
    Location: 6,
    QuantityReceived: 7,
    UnitOfMeasure: 9,
    QuantityReturned: 8,
    AdjustedUnitCost: 14,
    AdjustedCost: 15,
    ExtendedCost: 17,
    UnitCost: 13,
    Comments: 26,
    Labels: 27,
    ManufacturersItemNumber: 29
};

var headerFields = {
    ReceiptType: 8,
    AdditionalCost: 16,
    RequireLabels: 26,
    ReceiptCurrency: 11,
    AdditionalCostCurrency: 19,
    VendorNumber: 10,
    ExchangeRate: 12,
    RateType: 13
};
var type = type || {};
    type = {
    RECEIPT: 1,
    RETURN: 2,
    ADJUSTMENT: 3,
    COMPLETE: 4
};
var statusType = { Yes: 1, No: 0 };

var recordStatus = {
    ENTERED: 1,
    POSTED: 2,
    COSTED: 3,
    DAYENDCOMPLETED: 20
};

receiptUI = {
    receiptModel: {},
    hasKoBindingApplied: false,
    ignoreIsDirtyProperties: ["ReceiptNumber", "isControlsDisabledOnReadMode", "UIMode", "IsOptionalFields", "IsRequireLabel", "ReceiptDetail", "ReceiptOptionalField", "ReceiptDetailOptionalField", "disableAdditionalCost", "disableExchangeRate"],
    computedProperties: ["UIMode", "ComputedYearPeriod", "isControlsDisabledOnReadMode", "ClearRecordStatusDescription"],
    isFromReceiptFinder: false,
    isOptionalField: false,

    btnAddLineID: "",
    IsValidate: "",
    duplicateCheckGrid: null,
    optionalFieldPopUpClose: false,
    lineNumber: 0,
    isDetailOptionalField: false,

    //--- Receipt detail related variables  --//
    addLineClicked: false,
    isPostValid: true,
    isKendoControlNotInitialised: false,
    createNewLine: false,
    skipLineEditable: false,
    setFirstLineEditable: false,
    insertedIndex: 0,
    createNewRecord: false,
    moveToNextPage: false,
    isEditable: ko.observable(true),
    manufactureItemNumber: null,
    isVendorNumberCorrect: false,
    itemfinderlineNumber: null,
    isWrongRateType: false,
    RateTypeOldValue: null,
    receiptNumber: null,
    isWrongExchangeRate: false,
    exchangeRateOldValue: null,
    isVendorNumberChanged: false,
    additionalCost: null,
    additionalCostCurrencyClicked: false,
    cursorField: null,
    oldAdditionalCostCurrency: null,
    createNewButtonClicked: false,
    cursorforFinder: null,
    oldUnitOfMeasure: null,
    manufactureItemNumner: null,
    refreshClicked: false,
    indexOfPage: 0,

    //--- Receipt detail --//
    checkValidDateType: {
        ReceiptDate: 0,
        PostingDate: 1
    },
    dateChangeBy: null,
    isReceiptDateModified: false,
    isReceiptCurrency: false,
    previousReceiptDate: null,
    previousPostingDate: null,
    controlToBeFocused: "#txtReceiptNumber",

    //All init Methods here
    init: function () {
        receiptUI.initButtons();
        receiptUI.initDropDownList();
        receiptUI.receiptModel = receiptViewModel;
        receiptUI.initFinders();
        receiptUI.initPopUps();
        receiptUI.initHamburgers();
        receiptUI.initGrids();
        receiptUISuccess.initialLoad(receiptViewModel);
        if (!receiptUI.hasKoBindingApplied) {
            receiptObservableExtension(receiptUI.receiptModel, sg.utls.OperationMode.NEW);
        }
        receiptUISuccess.setkey();
        receiptUI.initDetailOptionalFields(true);
        receiptUI.initOptionalFields(true);
        receiptUI.showHideColumns(receiptUI.receiptModel.Data.ReceiptType());
        receiptUI.cursorField = null;
        receiptUI.RateTypeOldValue = null;
        GridPreferencesHelper.setGrid("#ReceiptGrid", receiptUserPreferences.receiptDetailGrid);
    },


    //Init buttons
    initButtons: function () {

        $("#btnRefresh").on("click", function () {
            var grid = $('#ReceiptGrid').data("kendoGrid");
            if (!$(this).is(':disabled') && grid.dataSource.data().length > 0) {
                receiptUI.refreshClicked = true;
                receiptUI.addLineClicked = false;
                receiptRepository.saveReceiptDetails(receiptUI.receiptModel.Data);
            }
        });

        $("#txtReceiptNumber").on('change', function () {
            var data = receiptUI.receiptModel.Data;
            data.ReceiptNumber($("#txtReceiptNumber").val());
            if (sg.controls.GetString(data.ReceiptNumber() !== "")) {
                if (sg.controls.GetString(receiptUI.receiptNumber) !== sg.controls.GetString(data.ReceiptNumber())) {
                    receiptUI.checkIsDirty(receiptUI.get);
                }
            }
        });

        $("#txtReceiptDate").on('change', function () {
            receiptUI.previousReceiptDate = null;
            var receiptDate = $("#txtReceiptDate").val();
            var data = receiptUI.receiptModel.Data;
            receiptUI.previousReceiptDate = data.ReceiptDate();
            var validDate = sg.utls.kndoUI.checkForValidDate(receiptDate);
            if (validDate) {
                receiptUI.dateChangeBy = dateChanged.receiptDate;
                receiptDate = sg.utls.kndoUI.convertStringToDate(validDate);
                if (receiptDate) {
                    receiptUI.controlToBeFocused = "#txtReceiptDate";
                    receiptRepository.checkDate(receiptDate);
                }
                receiptUI.enableReceiptType(false);
            } else {
                data.ReceiptDate(receiptUI.previousReceiptDate);
            }
        });

        $("#txtDescription").on('change', function () {
            receiptUI.enableReceiptType(false);
        });

        $("#txtPostingDate").on('change', function () {
            var postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
            var data = receiptUI.receiptModel.Data;
            receiptUI.previousPostingDate = data.PostingDate();
            var validDate = sg.utls.kndoUI.checkForValidDate(postingDate);
            if (validDate) {
                receiptUI.dateChangeBy = dateChanged.postingDate;
                postingDate = sg.utls.kndoUI.convertStringToDate(validDate);
                receiptUI.postingDate = data.PostingDate();
                if (postingDate) {
                    receiptUI.controlToBeFocused = "#txtPostingDate";
                    receiptRepository.checkDate(postingDate);
                }
                receiptUI.enableReceiptType(false);
            } else {
                data.PostingDate(receiptUI.previousPostingDate);
            }
        });

        //Create New Receipt 
        $("#btnNewReceipt").bind('click', function () {
            receiptUI.createNewButtonClicked = true;
            receiptUI.checkIsDirty(receiptUI.create);
            receiptUI.isWrongExchangeRate = false;
            receiptUI.prevExRate = "";
            receiptUI.exchangeRateOldValue = "";
            receiptUI.isVendorNumberCorrect = false;
            receiptUI.createNewButtonClicked = false;
        });

        //Save receipt
        $("#btnSave").bind('click', function () {
            sg.utls.SyncExecute(receiptUI.save);
        });

        //Receipt Post Functionality
        $("#btnPost").bind('click', function () {

            if ($("#frmReceipt").valid()) {
                receiptUI.validateGridonPost();
                if (receiptUI.receiptModel.IsPromptToDelete() && receiptUI.isPostValid === true) {
                    sg.utls.showKendoConfirmationDialog(
                        function () { // Yes
                            sg.utls.clearValidations("frmReceipt");
                            var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                            receiptRepository.post(data, receiptUI.receiptModel.Data.SequenceNumber(), true);
                        },
                        function () { // No
                            sg.utls.clearValidations("frmReceipt");
                            var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                            receiptRepository.post(data, receiptUI.receiptModel.Data.SequenceNumber(), false);
                        },
                        jQuery.validator.format(sg.utls.htmlEncode(receiptResources.PostWarning1),
                            sg.utls.htmlEncode(receiptResources.ReceiptLower))
                        + "<br/><br/>" +
                        jQuery.validator.format(sg.utls.htmlEncode(receiptResources.PostWarning2), sg.utls.htmlEncode(receiptResources.ReceiptLower))
                        + "<br/> <br/>"
                        + jQuery.validator.format(sg.utls.htmlEncode(receiptResources.PostWarning3), sg.utls.htmlEncode(receiptResources.ReceiptLower)), false, true);
                } else if (receiptUI.isPostValid === true) {
                    receiptRepository.post(receiptUI.receiptModel.Data, receiptUI.receiptModel.Data.SequenceNumber(), false);
                }
            }
        });

        //Delete receipt
        $("#btnDelete").bind('click', function () {
            if ($("#frmReceipt").valid()) {
                var message = jQuery.validator.format(receiptResources.DeleteConfirmMessage, receiptResources.ReceiptNumber, receiptUI.receiptModel.Data.ReceiptNumber());

                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmReceipt");
                    receiptRepository.deleteReceipt(receiptUI.receiptModel.Data.ReceiptNumber(), receiptUI.receiptModel.Data.SequenceNumber());
                }, null, message, receiptResources.DeleteTitle);
            }
        });

        //Optional field click
        $("#three").bind('click', function () {
            receiptUI.openOptionalFields();
        });

        // Detail Grid add line
        $("#btnDetailAddLine").on("click", function () {
            if (!$(this).is(':disabled')) {
                if ($("#frmReceipt").valid()) {
                    $("#message").empty();
                    sg.utls.SyncExecute(receiptGridUtility.createDetailLine);
                    sg.controls.enable("#selectAllChk");
                    sg.controls.disable(this);
                }
            }
        });

        // Detail Grid delete line
        $("#btnDetailDeleteLine").on("click", function () {
            receiptGridUtility.deleteLine("ReceiptGrid", "selectAllChk", "Message", "btnDetailDeleteLine");
        });

        // Detail Grid Edit user preferences
        $('#btnDetailEditColumns').on('click', function () {
            GridPreferences.initialize('#ReceiptGrid', "53538E45-3265-4BA3-B5B2-CD0582ADEF99", $(this), receiptGrid.config.columns);
        });

        //VendorNumber change events
        $("#Data_VendorNumber").bind('change', function () {
            $("#message").empty();
            sg.delayOnChange("btnVendorNumberFinder", $("Data_VendorNumber"), function () {
                receiptUI.isVendorNumberChanged = true;
                receiptUI.RefreshHeader();
            });
        });

        //Receipt currency change events
        $("#Data_ReceiptCurrency").bind('change', function () {
            $("#message").empty();
            sg.delayOnChange("btnReceiptCurrencyFinder", $("Data_ReceiptCurrency"), function () {
                receiptUI.currencySelected = 1;
                receiptUI.RefreshHeader();
                if (receiptUI.isReceiptCurrency) {
                    return;
                } else {
                    sg.controls.Focus($("#Data_ReceiptCurrency"));
                }
            });
        });

        //AdditionalCostCurrency change events
        $("#Data_AdditionalCostCurrency").change(function () {
            $("#message").empty();
            sg.delayOnBlur("btnAddlCostCurrencyFinder", function () {
                receiptUI.currencySelected = 2;
                receiptUI.additionalCostCurrencyClicked = true;
                receiptUI.RefreshHeader();
            });
        });

        //On Change for Rate Type
        $("#Data_RateType").bind('change', function () {
            $("#message").empty();
            receiptUI.isWrongRateType = true;
            var rateType = $("#Data_RateType").val();
            var data = receiptUI.receiptModel.Data;
            sg.delayOnChange("btnRateTypeFinder", $("Data_RateType"), function () {
                if (rateType) {
                    data.ExchangeRate(1);
                    receiptRepository.refresh(data);
                } else {
                    data.RateType(rateType);
                    var jsData = ko.mapping.toJS(data, receiptUI.ignoreIsDirtyProperties);
                    receiptRepository.getHeaderValues(jsData, headerFields.RateType);
                }
            });

        });

        //On Change for exchange rate type
        $("#Data_ExchangeRate").bind('change', function () {
            receiptUI.exchangeRateChange("#Data_ExchangeRate");
        });

        //On change for pop up exchange rate
        $("#txtpopupExchangeRate").bind('change', function () {
            receiptUI.exchangeRateChange("#txtpopupExchangeRate");
            var rate = $("#txtpopupExchangeRate").val();
            if (!rate) {
                receiptUI.receiptModel.Data.ExchangeRate(0);
                $("#txtpopupExchangeRate").val(0);
            }
        });
    },

    exchangeRateChange: function(id){
        $("#message").empty();
        receiptUI.isWrongRateType = false;
        receiptUI.isWrongExchangeRate = true;
        var rate = $(id).val();
        var data = receiptUI.receiptModel.Data;
        if (rate && data.ReceiptType() === type.RECEIPT) {
            receiptUI.exchangeRateOldValue = data.ExchangeRate();
            receiptRepository.checkRateSpread(data.RateType(), data.ReceiptCurrency(), data.RateDate(), rate, data.HomeCurrency());
        } else {
            data.ExchangeRate(rate);
            receiptUI.RefreshHeader();
        }
    },
    // Initialize the kendo numeric controls
    initNumericTextBox: function () {

        // kendoNumericTextBox for AdditionalCost
        var textBox = $("#txtAddlCost").data("kendoNumericTextBox");
        var mData = receiptUI.receiptModel.Data;

        if (textBox) {
            textBox.destroy();
        }
        var decimal = (mData.AdditionalCostCurrency() === mData.ReceiptCurrency()) ? mData.ReceiptCurrencyDecimals() : receiptUI.receiptModel.FuncDecimals();

        //kendo NumericTextBox for AdditionalCost 
        var ctrlAddlCost = $("#txtAddlCost").kendoNumericTextBox({
            format: "n" + decimal,
            spinners: false,
            step: 0,
            min: 0,
            decimals: 13,
            change: function () {
                receiptUI.previousAdditionalCost = mData.AdditionalCost();
                receiptUI.enableReceiptType(false);
                receiptUI.isWrongRateType = false;
                var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.getHeaderValues(data, headerFields.AdditionalCost);
            }
        }).data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(ctrlAddlCost, decimal, 13);

        //kendo NumericTextBox for exchange rate, popup exchange rate
        $.each(["#Data_ExchangeRate", "#txtpopupExchangeRate"], function (index, value) {
            textBox = $(value).data("kendoNumericTextBox");
            if (textBox) textBox.destroy();

            var exchangeRate = $(value).kendoNumericTextBox({
                format: "n7",
                spinners: false,
                step: 0,
                decimals: 7,
                min: 0.0000000,
                value: mData.ExchangeRate()
            }).data("kendoNumericTextBox");

            $(exchangeRate.element).unbind("input");
            sg.utls.kndoUI.restrictDecimals(exchangeRate, 7, 8);
        });

        //cost numeric boxes
        $.each(["#txtTotalCost", "#txtTotalReturnCost", "#txtTotalAdjustmentCost"], function (index, value) {
            $(value).kendoNumericTextBox({
                format: "n" + decimal,
                spinners: false,
                step: 0,
                min: 0,
                decimals: 13
            });
        });
    },

    //Init dropdown list
    initDropDownList: function () {
        var fields = ["Data_ReceiptType", "Data_AdditionalCostAllocationType"];
        $.each(fields, function (index, field) {
            sg.utls.kndoUI.dropDownList(field);
        });

        $("#Data_ReceiptType").data("kendoDropDownList").bind("change", receiptUI.typeSelectionChanged);
        $("#Data_AdditionalCostAllocationType").data("kendoDropDownList").bind("change", receiptUI.costSelectionChanged);
    },

    updateFiscalYearPeriod: function (date) {
        var model = receiptUI.receiptModel.Data;
        var changedDate = new Date(date);
        var fiscalYear = changedDate.getFullYear();
        var fiscalPeriod = changedDate.getMonth() + 1;
        model.FiscalPeriod(parseInt(fiscalPeriod));
        model.FiscalYear(fiscalYear);
    },

    //Receipt Create Functionality
    create: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptRepository.create(receiptUI.receiptModel.Data.ReceiptNumber());
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    //Receipt Save Functionality
    save: function () {
        if ($("#frmReceipt").valid()) {
            receiptUI.validateGrid();
            receiptUI.receiptSave();
        }
    },

    validateGrid: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
        var gridData = grid.dataSource.data();

        if (currentRowGrid || grid.dataSource.total() > 0) {
            for (var i = 0, length = gridData.length ; i < length; i++) {
                var item = gridData[i];
                if (item ) {
                    if (item.ItemNumber === "" || item.ItemNumber === null) {
                        grid.dataSource.remove(item);
                        item.IsNewLine = false;
                        sg.utls.ko.removeDeletedRows(receiptUI.receiptModel.Data.ReceiptDetail.Items);
                    } else if (item.ItemNumber !== "" && (item.Location === "" || (item.QuantityReceived === "" || item.QuantityReceived === 0)) && item.IsNewLine === true) {
                        if (item.Location === "" || item.Location === null) {
                            receiptGridUtility.resetFocus(currentRowGrid, "Location");
                            receiptUI.receiptSave();
                            return false;
                        }
                        if (item.QuantityReceived === "" || item.QuantityReceived === 0) {
                            receiptGridUtility.resetFocus(currentRowGrid, "QuantityReceived");
                            receiptUI.receiptSave();
                            return false;
                        }
                    }
                }
            }
        }
    },

    validateGridonPost: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
        var gridData = grid.dataSource.data();

        if (currentRowGrid || grid.dataSource.total() > 0) {
            for (var i = 0; i < gridData.length; i++) {
                var item = gridData[i];
                if (item ) {
                    if (item.ItemNumber === "" || item.ItemNumber === null) {
                        grid.dataSource.remove(item);
                        item.IsNewLine = false;
                        sg.utls.ko.removeDeletedRows(receiptUI.receiptModel.Data.ReceiptDetail.Items);

                    } else if (item.ItemNumber !== "" && (item.Location === "" || (item.QuantityReceived === "" || item.QuantityReceived === 0)) && item.IsNewLine === true) {
                        if (item.Location === "" || item.Location === null) {
                            receiptUI.isPostValid = false;
                            receiptGridUtility.resetFocus(currentRowGrid, "Location");
                            receiptUI.receiptSave();
                            return false;
                        }
                        if (item.QuantityReceived === "" || item.QuantityReceived === 0) {
                            receiptUI.isPostValid = false;
                            receiptGridUtility.resetFocus(currentRowGrid, "QuantityReceived");
                            receiptUI.receiptSave();
                            return false;
                        }
                    } else {
                        receiptUI.isPostValid = true;
                    }
                }
            }
        }
    },

    validateGridRow: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
        if (currentRowGrid || grid.dataSource.total() > 0) {
            if (currentRowGrid) {
                if (currentRowGrid.ItemNumber !== "" && currentRowGrid.Location !== "" && (currentRowGrid.QuantityReceived !== "" || currentRowGrid.QuantityReceived !== 0)) {
                    receiptRepository.readHeader(receiptUI.receiptModel.Data, true);
                }
            }
        }
    },

    receiptSave: function () {
        var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.SAVE) {
            data.RecordStatus = 1;
            receiptRepository.update(data);
        } else {
            data.RecordStatus = 1;
            receiptRepository.add(data);
        }
    },

    //Check Is Dirty Functionality
    checkIsDirty: function (functionToCall) {
        var model = receiptUI.receiptModel;
        var exists = true;
        if (model.UIMode() === sg.utls.OperationMode.NEW && !receiptUI.isFromReceiptFinder) {
            if (model.isModelDirty.isDirty() || model.isModelDirty.isGridDataDirty() && receiptUI.receiptNumber) {
                var data = ko.mapping.toJS(model.Data, receiptUI.ignoreIsDirtyProperties);
                exists = receiptRepository.isExists(model.Data.ReceiptNumber(), data);
            }
        }

        receiptUI.isFromReceiptFinder = false;
        if (model.UIMode() === sg.utls.OperationMode.NEW && !exists && !receiptUI.createNewButtonClicked)
            return;

        if ((model.isModelDirty.isDirty() || model.isModelDirty.isGridDataDirty() && receiptUI.receiptNumber)
            && (!(model.UIMode() === sg.utls.OperationMode.NEW) || exists)) {
            sg.utls.showKendoConfirmationDialog(
            function () { // Yes
                sg.utls.clearValidations("frmReceipt");
                functionToCall.call();
            },
            function () { // No
                if (sg.controls.GetString(receiptUI.receiptNumber) !== sg.controls.GetString(model.Data.ReceiptNumber())) {
                    model.Data.ReceiptNumber(receiptUI.receiptNumber);
                }
                return;
            },
            jQuery.validator.format(globalResource.SaveConfirm, receiptResources.Receipt, receiptUI.receiptNumber));
        }
        else {
            functionToCall.call();
        }
    },

    //Optional field load popup
    openOptionalFields: function () {
        receiptUI.isOptionalField = true;
        sg.utls.openKendoWindowPopup('#optionalField', null);
        optionalFieldUIGrid.isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();
        $("#windowmessage1").empty();
        receiptUI.initOptionalFields(false);
        var grid = $('#OptionalFieldGrid').data("kendoGrid");
        grid.dataSource.data([]);
        grid.dataSource.page(1);
    },

    //ExchangeRate load popup
    openExchangeRate: function () {
        receiptUI.isWrongExchangeRate = true;
        receiptUI.RateTypeOldValue = receiptUI.receiptModel.Data.RateType();
        receiptUI.exchangeRateOldValue = receiptUI.receiptModel.Data.ExchangeRate();
        sg.utls.openKendoWindowPopup('#exchangeRate', null);
        $("#windowmessage").empty();

        $("#exchangeRate").data('kendoWindow').unbind('activate').bind('activate', function () {
            sg.controls.Focus($("#Data_RateType"));
        });
    },

    //Init all finders
    initFinders: function () {

        var receiptNumberTitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.ReceiptNumber);
        sg.finderHelper.setFinder("btnReceiptNumberFinder", sg.finder.ReceiptNumberFinder, receiptUISuccess.receiptNumberfinderSuccess, receiptUISuccess.receiptFinderCancel, receiptNumberTitle, receiptFilter.getReceiptFilter, null, false);

        var finderVendorTitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.VendorNumber);
        sg.finderHelper.setFinder("btnVendorNumberFinder", sg.finder.Vendor, receiptUISuccess.vendorResult, $.noop, finderVendorTitle,
            receiptFilter.getVendorFilter, null, true);

        var receiptCurrencyFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.ReceiptCurr);
        sg.finderHelper.setFinder("btnReceiptCurrencyFinder", sg.finder.CurrencyCode, receiptUISuccess.ReceiptCurrencyResult, $.noop(), receiptCurrencyFindertitle, sg.finderHelper.createDefaultFunction("Data_ReceiptCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, false);

        var costCurrencyFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.AddCostCurr);
        sg.finderHelper.setFinder("btnAddlCostCurrencyFinder", sg.finder.CurrencyCode, receiptUISuccess.CostCurrencyResult, $.noop(), costCurrencyFindertitle, sg.finderHelper.createDefaultFunction("Data_AdditionalCostCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, false);

        var rateTypeFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.RateTypes);
        sg.finderHelper.setFinder("btnRateTypeFinder", sg.finder.CurrencyRateType, receiptUISuccess.rateTypeFinderSuccess, $.noop(), rateTypeFindertitle,
        receiptFilter.getRateTypeFilter, null, false);

        var exchangeRateFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.CurrencyRates);
        sg.finderHelper.setFinder("btnExchangeRateFinder", sg.finder.CurrencyRate, receiptUISuccess.currencyRateFinderSuccess, $.noop(), exchangeRateFindertitle, receiptFilter.exchangeRateFilter, null, false);

        var finderTitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.ManufacturerItemNumberTitle);
        sg.finderHelper.setFinder("btnManufacturerItemFinder", sg.finder.ICManufacturerItemNumber, receiptUISuccess.manufacturerItem, receiptFilter.onFinderCancel, finderTitle, receiptFilter.getManufacturerFieldFilter);
    },

    //Init all pop ups screens
    initPopUps: function () {

        // Header optional field pop up
        sg.utls.intializeKendoWindowPopup('#optionalField', receiptResources.OptionalFields, function (e) {
            if (!receiptUI.optionalFieldPopUpClose) {
                e.preventDefault();
                $("#windowmessage1").empty();

                var hasModified = optionalFieldUIGrid.save();
                if (!hasModified) {
                    if (!receiptUI.receiptModel.DisableScreen()) {
                        receiptRepository.refreshOptField();
                    }
                    receiptUI.optionalFieldPopUpClose = true;
                    $("#optionalField").data("kendoWindow").close();
                }
            } else {
                receiptUI.optionalFieldPopUpClose = false;
            }
        });

        //Detail optional field pop up
        sg.utls.intializeKendoWindowPopup('#detailOptionalField', receiptResources.OptionalFields, function (e) {
            if (!receiptUI.optionalFieldPopUpClose) {
                e.preventDefault();
                $("#windowmessage").empty();

                var hasModified = optionalFieldUIGrid.save();
                if (!hasModified) {
                    var cell = receiptGridUtility.getCurrentRowCell(receiptDetailColumnIndex.OptionalFields);
                    var grid = $('#ReceiptGrid').data("kendoGrid");
                    grid.editCell(cell);
                    var data = sg.utls.kndoUI.getSelectedRowData(grid);
                    receiptRepository.refreshDetail(data);
                    receiptUI.optionalFieldPopUpClose = true;
                    $("#detailOptionalField").data("kendoWindow").close();
                }
            } else {
                receiptUI.optionalFieldPopUpClose = false;
            }
        });

        //ExchangeRate pop up
        sg.utls.intializeKendoWindowPopup('#exchangeRate', receiptResources.RateSelection);
    },

    //Init Hamburgers
    initHamburgers: function () {
        var listExchangeRate = [sg.utls.labelMenuParams("lnkOpenExchangeRate", receiptResources.EditLink, receiptUI.openExchangeRate, "sagedisable:IsFuncCurrencyDisable")];
        var listOptionalField = [sg.utls.labelMenuParams("lnkOpenOptionalFields", receiptResources.AddOrEditLink, receiptUI.openOptionalFields, null)];
        LabelMenuHelper.initialize(listExchangeRate, "lnkExchangeRateThree", "receiptUI.receiptModel");
        LabelMenuHelper.initialize(listOptionalField, "lnkOptionalField", "receiptUI.receiptModel");
    },

    //Init receipt grid 
    initGrids: function () {
        receiptGrid.init();
    },

    //ReceiptType Dropdown changed Event
    typeSelectionChanged: function () {
        var dropdown = $("#Data_ReceiptType").data("kendoDropDownList");
        var selectedValue = dropdown.value();

        if (selectedValue) {
            receiptUI.showHideColumns(selectedValue);
            receiptUI.receiptModel.Data.ReceiptType(selectedValue);
            receiptRepository.refresh(receiptUI.receiptModel.Data);
        }
    },

    //AdditionalCost  Dropdown changed Event
    costSelectionChanged: function () {
        var dropdown = $("#Data_AdditionalCostAllocationType").data("kendoDropDownList");
        var selectedvalue = dropdown.value();

        if (selectedvalue !== "") {
            receiptUI.receiptModel.Data.AdditionalCostAllocationType(selectedvalue);
            receiptUI.enableReceiptType(false);
        }
    },

    //Show hide columns 
    showHideColumns: function (value) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        value = parseInt(value);
        if (receiptUI.receiptModel) {
            if (!grid) return;
            var i;
            var index1 = GridPreferencesHelper.getColumnIndex('#ReceiptGrid', "AdjustedCost");
            var index2 = GridPreferencesHelper.getColumnIndex('#ReceiptGrid', "AdjustedUnitCost");
            var index3 = GridPreferencesHelper.getColumnIndex('#ReceiptGrid', "QuantityReturned");
            var index4 = GridPreferencesHelper.getColumnIndex('#ReceiptGrid', "ReturnCost");
            var index = [index1, index2, index3, index4];

            if (value === type.ADJUSTMENT) {
                for (i = 0; i < 4; i++) {
                    if (i === 0 || i === 1) {
                        grid.showColumn(index[i]);
                    } else {
                        grid.hideColumn(index[i]);
                    }
                    grid.columns[index[i]].attributes['sg_Customizable'] = (i < 2);
                }
                return;
            }
            if (value === type.RETURN) {
                for (i = 0; i < 4; i++) {
                    if (i === 0 || i === 1) {
                        grid.hideColumn(index[i]);
                    } else {
                        grid.showColumn(index[i]);
                    }
                    grid.columns[index[i]].attributes['sg_Customizable'] = (i > 1);
                }
                return;
            }
            for (i = 0; i < 4; i++) {
                grid.hideColumn(index[i]);
                grid.columns[index[i]].attributes['sg_Customizable'] = false;
            }
        }
    },

    //Get receipt number
    get: function () {
        var model = receiptUI.receiptModel;
        var receiptNumber = model.Data.ReceiptNumber();
        model.UIMode(sg.utls.OperationMode.LOAD);
        receiptRepository.get(receiptNumber, model.DisableScreen());
    },

    populateDropDownList: function () {
        var data = receiptUI.receiptModel.Data;
        $("#Data_ReceiptType").data("kendoDropDownList").value(data.ReceiptType());
        $("#Data_AdditionalCostAllocationType").data("kendoDropDownList").value(data.AdditionalCostAllocationType());
    },

    initDetailOptionalFields: function (initialize) {
        if (!optionalFieldUIGrid.modelData) {
            optionalFieldUIGrid.modelData = receiptUI.receiptModel.Data;
        }
        receiptUI.isDetailOptionalField = true;
        var params = {
            gridId: "DetailOptionalFieldGrid",
            isDefault: false,
            btnEditColumnsId: "btnDetailEditOptFieldColumns",
            finder: "ICOptionalFields",
            modelData: receiptUI.receiptModel.Data,
            modelName: "ReceiptDetailOptionalField",
            newLineItem: receiptGridUtility.newDetailOptionalFieldLineItem,
            isValueSetEditable: false,
            optionalFieldFilter: receiptUI.detailOptionalFieldFilter,
            getOptionalFieldData: receiptRepository.getDetailOptionalFieldFinderData,
            getOptionalFieldValue: receiptRepository.setOptionalFieldValue,
            deleteUrl: sg.utls.url.buildUrl("TU", "Receipt", "DeleteDetailOptFields"),
            isCheckDuplicateRecord: true,
            deleteFromServer: true,
            saveOptionalField: receiptRepository.saveOptionalFields,
            // Added this to cover the integration scenario with vendor optional fields.
            OnOptionalFieldSelection: receiptRepository.getDetailOptionalFieldFinderData,
            onDeleteSuccess: receiptUISuccess.onDetailDeleteSuccess,
            isPopUp: true,
            isOptionalFieldModel: true
        };
        optionalFieldUIGrid.init(params, initialize);
    },

    initOptionalFields: function (initialize) {
        if (!optionalFieldUIGrid.modelData) {
            optionalFieldUIGrid.modelData = receiptUI.receiptModel.Data;
        }
        receiptUI.isDetailOptionalField = false;
        var params = {
            gridId: "OptionalFieldGrid",
            isDefault: false,
            btnEditColumnsId: "btnEditOptFieldColumns",
            finder: "ICOptionalFields",
            modelData: receiptUI.receiptModel.Data,
            modelName: "ReceiptOptionalField",
            newLineItem: receiptGridUtility.newOptionalFieldLineItem,
            isValueSetEditable: false,
            optionalFieldFilter: receiptUI.OptionalFieldFilter,
            getOptionalFieldData: receiptRepository.getOptionalFieldFinderData,
            getOptionalFieldValue: receiptRepository.setOptionalFieldValue,
            deleteUrl: sg.utls.url.buildUrl("TU", "Receipt", "DeleteOptionalFields"),
            isCheckDuplicateRecord: true,
            deleteFromServer: true,
            saveOptionalField: receiptRepository.saveOptionalFields,
            // Added this to cover the integration scenario with vendor optional fields.
            OnOptionalFieldSelection: receiptRepository.getOptionalFieldFinderData,
            onDeleteSuccess: receiptUISuccess.onDeleteSuccess,
            isPopUp: true,
            isOptionalFieldModel: true
        };
        optionalFieldUIGrid.init(params, initialize);
    },

    checkDuplicateRecord: function (grid, field, checkItem) {
        var count = 0;
        var errorMsg = $.validator.format(optionalFieldsResources.duplicateMessage, optionalFieldsResources.optionalFieldsTitle, checkItem.toUpperCase());
        var messageBoxId = "#windowmessage";
        if (receiptUI.isDetailOptionalField) {
            messageBoxId = "#windowmessage1";
        }
        $(messageBoxId).empty();
        var dataSource = $("#" + grid).data("kendoGrid").dataSource;
        var row = sg.utls.kndoUI.getSelectedRowData($("#" + grid).data("kendoGrid"));

        $.each(dataSource.data(), function (key) {
            if (dataSource.data()[key][field] === checkItem) {
                count += 1;
            }
        });
        if (count > 1) {
            if (optionalFieldUIGrid.isPopUp) {
                var msg = {};
                msg.UserMessage = {};
                msg.Data = {};
                msg.UserMessage.Message = optionalFieldsResources.ProcessFailedMessage;
                msg.UserMessage.Errors = [{ Message: errorMsg }];
                sg.utls.showMessagePopupWithoutClose(msg, messageBoxId);
            } else {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
            }
            return;
        }
        if (dataSource.total() > 10 && optionalFieldUIGrid.isOptionalFieldExists !== null) {
            if (optionalFieldUIGrid.modelData.AccountNumber) {
                var accountNumber = optionalFieldUIGrid.modelData.AccountNumber();
                var data = { fieldName: field, fieldValue: checkItem, accountNumber: accountNumber };
                optionalFieldUIGrid.isOptionalFieldExists(data);
            } else if (optionalFieldUIGrid.modelData.Location) {
                var location = optionalFieldUIGrid.modelData.Location();
                optionalFieldUIGrid.isOptionalFieldExists(row.OptionalField, location);
            } else {
                optionalFieldUIGrid.isOptionalFieldExists(row);
            }
        }
        optionalFieldUIGrid.resetFocus(row, 'OptionalField');
    },

    OptionalFieldFilter: function () {
        var filters = [[]];
        filters[0][1] = sg.finderHelper.createFilter("Location", sg.finderOperator.Equal, "Receipts");
        filters[0][1].IsMandatory = true;
        filters[0][0] = sg.finderHelper.createFilter("OptionalField", sg.finderOperator.StartsWith, optionalFieldUIGrid.optionalFieldFilterData);
        return filters;
    },

    detailOptionalFieldFilter: function () {
        var filters = [[]];
        filters[0][1] = sg.finderHelper.createFilter("Location", sg.finderOperator.Equal, "ReceiptDetails");
        filters[0][1].IsMandatory = true;
        filters[0][0] = sg.finderHelper.createFilter("OptionalField", sg.finderOperator.StartsWith, optionalFieldUIGrid.optionalFieldFilterData);
        return filters;
    },

    enableReceiptType: function (isEnable) {
        var ctrl = $("#Data_ReceiptType").data("kendoDropDownList");
        if (ctrl) {
            ctrl.wrapper.show();
            ctrl.enable(isEnable);
        }
    },

    setExchangeRate: function (jsonResult) {
        receiptUI.prevExRate = null;
        receiptUI.prevExRate = jsonResult.Data.Rate;
        receiptUI.receiptModel.Data.ExchangeRate(jsonResult.Data.Rate);
        $("#Data_ExchangeRate").data("kendoNumericTextBox").value(jsonResult.Data.Rate);
        $("#txtpopupExchangeRate").data("kendoNumericTextBox").value(jsonResult.Data.Rate);
        receiptUI.exchangeRateOldValue = jsonResult.Data.Rate;
    },

    constructErrorMessage: function (optionalFieldKey, data, modelName, messageBoxId) {
        var errorMsg = $.validator.format(optionalFieldsResources.duplicateMessage, optionalFieldsResources.optionalFieldsTitle, optionalFieldKey.toUpperCase());
        var msg = {};
        msg.UserMessage = {};
        msg.Data = {};
        msg.UserMessage.Message = optionalFieldsResources.ProcessFailedMessage;
        msg.UserMessage.Errors = [{ Message: errorMsg }];
        sg.utls.showMessagePopupWithoutClose(msg, messageBoxId);
        data.set("OptionalField", null);
        data.set("OptionalFieldDescription", null);
        optionalFieldUIGrid.resetFocus(data, "OptionalField");
    },

    setExchangeRateValue: function () {
        receiptRepository.GetExchangeRate(receiptUI.receiptModel.Data.RateType(),
            receiptUI.receiptModel.Data.ReceiptCurrency(), receiptUI.receiptModel.Data.RateDate(),
            receiptUI.receiptModel.Data.ExchangeRate(), receiptUI.receiptModel.Data.HomeCurrency());
    },

    RefreshHeader: function () {
        var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        receiptRepository.refresh(data);
    }
};

var receiptUISuccess = {
    setkey: function () {
        receiptUI.receiptNumber = receiptUI.receiptModel.Data.ReceiptNumber();
    },

    onSaveDetailsCompleted: function (result) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var data = receiptUI.receiptModel.Data;
        receiptUI.receiptModel.Data.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
        receiptGridUtility.updateNumericTextBox("txtTotalCost", data.TotalCostReceiptAdditional());
        receiptGridUtility.updateNumericTextBox("txtTotalReturnCost", data.TotalReturnCost());
        receiptGridUtility.updateNumericTextBox("txtTotalAdjustmentCost", data.TotalAdjCostReceiptAddl());
        grid.dataSource.read();
    },

    refreshReceiptDetail: function (result) {
        receiptGridUtility.isDataRefreshInProgress = true;
        if (!result.UserMessage.Errors) {
            ko.mapping.fromJS(result.Data.ReceiptDetail, {}, receiptUI.receiptModel.Data.ReceiptDetail);
        }
        sg.utls.showMessage(result);
        receiptGridUtility.isDataRefreshInProgress = false;

        var grid = $('#ReceiptGrid').data("kendoGrid");
        grid.dataSource.read();
    },

    deleteDetailsSuccess: function () {
        var grid = $("#ReceiptGrid").data("kendoGrid");
        grid.dataSource.read();
    },

    getVendorDetailsSuccess: function (result) {
        if (result.Data) {
            if (result.Data.RateType) {
                receiptUI.receiptModel.Data.RateType(result.Data.RateType);
                $("#Data_RateType").val(result.Data.RateType);
            }
        }
    },

    GetExchangeRate: function (jsonResult) {
        if (jsonResult.UserMessage.Message) {
            receiptUI.setExchangeRate(jsonResult);
        }
        if (receiptUI.isReceiptDateModified === true || receiptUI.isWrongRateType === true) {
            receiptUI.setExchangeRate(jsonResult);
        }
    },

    getRateSpread: function (jsonResult) {
        if (jsonResult.UserMessage.Message) {
            sg.utls.showKendoConfirmationDialog(
                //click on Yes
                function () {
                    var eRate = $("#txtpopupExchangeRate").val();
                    receiptUI.prevExRate = null;
                    receiptUI.prevExRate = receiptUI.receiptModel.Data.ExchangeRate();
                    receiptUI.exchangeRateOldValue = eRate;
                    if (receiptUI.receiptModel.isModelDirty.isGridDataDirty()) {
                        var mData = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                        receiptRepository.refresh(mData);
                    }
                },
                //click on No
                function () {
                    if (receiptUI.exchangeRateOldValue) {
                        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.exchangeRateOldValue);
                        receiptUI.prevExRate = receiptUI.exchangeRateOldValue;
                    }
                    if (receiptUI.exchangeRateOldValue === null && receiptUI.prevExRate) {
                        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.prevExRate);
                    }
                },
                jsonResult.UserMessage.Message);

        } else {
            var exchangeRate = $("#txtpopupExchangeRate").val();
            receiptUI.prevExRate = null;
            receiptUI.prevExRate = receiptUI.receiptModel.Data.ExchangeRate();
            receiptUI.exchangeRateOldValue = exchangeRate;
            if (receiptUI.receiptModel.isModelDirty.isGridDataDirty()) {
                var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.refresh(data);
            }
        }
    },

    getItemTypeSuccess: function (jsonResult) {
        receiptUISuccess.setItemTypeResponse(jsonResult, "#btnManufacturerItemFinder", receiptUISuccess.manufacturerItem);
    },

    getResult: function (jsonResult) {
        var data = receiptUI.receiptModel.Data;
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            if (jsonResult.IsExists === true) {
                receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);

                if (data.ReceiptType() === type.RECEIPT || data.ReceiptType() === type.ADJUSTMENT) {
                    sg.controls.Focus($("#txtDescription"));
                } else {
                    sg.controls.KendoDropDownFocus($("#Data_ReceiptType"));
                }
            } else {
                receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            }
            receiptUISuccess.setkey();
        } else {
            data.ReceiptNumber(receiptUI.receiptNumber);
            if (jsonResult) {
                data.TotalCostReceiptAdditionalDecimal(jsonResult.TotalCostReceiptAdditionalDecimal);
                data.TotalReturnCostDecimal(jsonResult.TotalReturnCostDecimal);
            }
        }
        sg.utls.showMessage(jsonResult);
        receiptGrid.setFirstLineEditable = false;
    },

    createSuccess: function (jsonResult) {
        receiptUI.addLineClicked = false;
        receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        receiptUI.receiptModel.isModelDirty.reset();
        receiptUISuccess.setkey();
        sg.utls.showMessage(jsonResult);
        sg.controls.Select($("#txtReceiptNumber"));
    },

    updateSuccess: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
        receiptGrid.setFirstLineEditable = false;
    },

    addSuccess: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        if (jsonResult.UserMessage.Warnings != null && jsonResult.UserMessage.Warnings != "") {
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, jsonResult.UserMessage.Warnings[0].Message);
        }
        sg.utls.showMessage(jsonResult);
        receiptGrid.setFirstLineEditable = false;
    },

    deleteSuccess: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    postSuccess: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    getHeaderValues: function (result) {
        var data = receiptUI.receiptModel.Data;
        if (result && result.UserMessage) {
            receiptUI.isVendorNumberCorrect = false;
            if (receiptUI.isWrongExchangeRate) {
                receiptUI.isWrongExchangeRate = false;
                if (receiptUI.exchangeRateOldValue) {
                    data.ExchangeRate(receiptUI.exchangeRateOldValue);
                    receiptUI.prevExRate = receiptUI.exchangeRateOldValue;
                }
                if (receiptUI.exchangeRateOldValue === null && receiptUI.prevExRate) {
                    data.ExchangeRate(receiptUI.prevExRate);
                }
                receiptUI.exchangeRateOldValue = null;
            }

            if (receiptUI.isWrongRateType) {
                receiptUI.isWrongRateType = false;
                if (receiptUI.RateTypeOldValue) {
                    data.RateType(receiptUI.RateTypeOldValue);
                }
            }
            if (receiptUI.isVendorNumberChanged) {
                receiptUI.isWrongRateType = false;
                data.VendorNumber("");
                sg.controls.Focus($("#Data_VendorNumber"));
            }
            sg.utls.showMessage(result);
            receiptUI.isReceiptCurrency = false;

        } else {
            receiptUI.isReceiptCurrency = true;
            receiptUI.isVendorNumberChanged = false;

            if (result) {
                if (result.TotalCostReceiptAdditional) {
                    data.TotalCostReceiptAdditional(result.TotalCostReceiptAdditional);
                }
                if (result.TotalReturnCost) {
                    data.TotalReturnCost(result.TotalReturnCost);
                }
                if (result.TotalAdjCostReceiptAddl) {
                    data.TotalAdjCostReceiptAddl(result.TotalAdjCostReceiptAddl);
                }
                if (result.TotalExtendedCostSource) {
                    data.TotalExtendedCostSource(result.TotalExtendedCostSource);
                }
                if (result.TotalExtendedCostAdjusted) {
                    data.TotalExtendedCostAdjusted(result.TotalExtendedCostAdjusted);
                }
                if (result.ReceiptCurrencyDecimals) {
                    data.ReceiptCurrencyDecimals(result.ReceiptCurrencyDecimals);
                }
                if (result.TotalCostReceiptAdditionalDecimal) {
                    receiptUI.receiptModel.Data.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
                }
                if (result.TotalReturnCostDecimal) {
                    data.TotalReturnCostDecimal(result.TotalReturnCostDecimal);
                }
            }
            if (receiptUI.isWrongRateType) {
                if (result.ExchangeRate) {
                    data.ExchangeRate(result.ExchangeRate);
                }
                if (result.RateType) {
                    receiptUI.RateTypeOldValue = result.RateType;
                }
                if (result.ExchangeRate) {
                    receiptUI.exchangeRateOldValue = result.ExchangeRate;
                }
            }
            receiptGridUtility.updateTextBox();
            if (result.VendorExists && result.VendorNumber) {
                if (result.VendorExists === 0 && result.VendorNumber) {
                    sg.utls.showMessageInfo(sg.utls.msgType.WARNING, $.validator.format(receiptResources.RecordDoesNotExist, receiptResources.VendorNumber, receiptUI.receiptModel.Data.VendorNumber()));
                    sg.controls.Focus($("#Data_VendorNumber"));
                }
            }
        }
        sg.utls.showMessage(result);
    },

    // Alternate Item Finder Success
    manufacturerItem: function (result, manufacturerItemNumber) {

        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);

        if (result) {
            var itemnumber = null;
            if (result.ItemNumber === true || result.ItemNumber === undefined) {
                itemnumber = result.FormattedItemNumber;
            } else {
                itemnumber = result.ItemNumber;
            }
            receiptUI.manufactureItemNumner = null;

            if (itemnumber) {
                currentRowGrid.set("ItemNumber", itemnumber);
            }

            if (result.ManufacturersItemNumber) {
                currentRowGrid.set("ManufacturersItemNumber", result.ManufacturersItemNumber.toUpperCase());
                receiptUI.manufactureItemNumner = result.ManufacturersItemNumber.toUpperCase();
            }
            else if (manufacturerItemNumber !== "btnManufacturerItemFinder") {
                currentRowGrid.set("ManufacturersItemNumber", manufacturerItemNumber);
                receiptUI.manufactureItemNumner = manufacturerItemNumber;
            }
            if (itemnumber) {
                currentRowGrid.ItemNumber = itemnumber;
            }
        }
        receiptRepository.setItemGridValuesModel(currentRowGrid, gridRowFields.ItemNumber);
    },

    checkDate: function (jsonResult) {
        var receiptDate = sg.utls.kndoUI.getFormattedDate($("#txtReceiptDate").val());
        var postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
        var data = receiptUI.receiptModel.Data;

        if (jsonResult.UserMessage.Message && jsonResult.UserMessage.IsSuccess) {
            sg.utls.showKendoConfirmationDialog(
                //click on Yes
                function () {
                    receiptRepository.refresh(data);
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        receiptUI.isReceiptDateModified = true;
                        data.ReceiptDate(receiptDate);
                        data.RateDate(receiptDate);
                        receiptUI.setExchangeRateValue();

                        if (receiptUI.receiptModel.DefaultPostingDate() === 1) {
                            data.PostingDate(receiptDate);
                            receiptUI.updateFiscalYearPeriod(receiptDate);
                        }
                        if (data.ReceiptType() !== type.RECEIPT) {
                            receiptUI.enableReceiptType(false);
                        }
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        data.PostingDate(postingDate);
                        receiptUI.updateFiscalYearPeriod(postingDate);
                    }
                },
                //click on No
                function () {
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        data.ReceiptDate(receiptUI.previousReceiptDate);
                        receiptUI.updateFiscalYearPeriod(receiptUI.previousReceiptDate);
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        data.PostingDate(receiptUI.previousPostingDate);
                        receiptUI.updateFiscalYearPeriod(receiptUI.previousPostingDate);
                    }
                },
                jsonResult.UserMessage.Message);
        } else {
            if (receiptUI.dateChangeBy === dateChanged.receiptDate) {
                receiptUI.isReceiptDateModified = true;
                receiptUI.updateFiscalYearPeriod(receiptDate);
                data.PostingDate(receiptDate);
                data.RateDate(receiptDate);
                receiptUI.setExchangeRateValue();
                if (data.ReceiptType() !== type.RECEIPT) {
                    receiptUI.enableReceiptType(false);
                }
            } else {
                receiptUI.updateFiscalYearPeriod(postingDate);
            }
        }
    },

    displayResult: function (jsonResult, uiMode, isOperation) {
        if (jsonResult) {

            if (!receiptUI.hasKoBindingApplied) {
                receiptUI.receiptModel = ko.mapping.fromJS(jsonResult);
                receiptObservableExtension(receiptUI.receiptModel, uiMode);
                receiptUI.receiptModel.UIMode(uiMode);
                receiptUI.hasKoBindingApplied = true;
                receiptUI.receiptModel.isModelDirty = new ko.dirtyFlag(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                window.ko.applyBindings(receiptUI.receiptModel);
            } else {
                ko.mapping.fromJS(jsonResult, receiptUI.receiptModel);
            }
            receiptUI.receiptModel.UIMode(uiMode);

            if (!receiptUI.isKendoControlNotInitialised) {
                receiptUI.isKendoControlNotInitialised = true;
                receiptUI.initNumericTextBox();
                receiptUI.initDropDownList();
            }
            receiptGridUtility.refreshReceiptDetailGrid();
            receiptGridUtility.updateTextBox();
            receiptUI.populateDropDownList();
            receiptUI.showHideColumns(receiptUI.receiptModel.Data.ReceiptType());

            if (!isOperation) {
                sg.utls.showMessage(jsonResult);
            }
            
            if (uiMode !== sg.utls.OperationMode.NEW) {
                receiptUI.receiptModel.isModelDirty.reset();
            }
        }
    },

    initialLoad: function (result) {
        if (result) {
            receiptUISuccess.displayResult(result);
        }
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    receiptNumberfinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUI.isFromReceiptFinder = true;
            receiptUI.checkIsDirty(receiptUISuccess.setReceiptNumberFinderData);
        }
    },

    setReceiptNumberFinderData: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptUI.receiptModel.Data.ReceiptNumber(receiptUI.finderData.ReceiptNumber);
        receiptUI.get(receiptUI.finderData.ReceiptNumber, receiptUI.finderData.SequenceNumber);
    },

    setItemTypeResponse: function (itemTypeResult, manufacturerFinderControlId, callbackToPopulateItem) {
        if (itemTypeResult) {
            if (itemTypeResult.NoOfManufacturerItemsMoreThan1) {
                //open finder for manufacturer items
                $(manufacturerFinderControlId).trigger("click");
                return;
            }
            if (itemTypeResult.UserMessage !== null) {
                sg.utls.showMessage(itemTypeResult);
            }
            //populate item 
            callbackToPopulateItem(itemTypeResult.Item, itemTypeResult.ManufacturerItemNumber);
        }
    },

    rateTypeFinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUISuccess.setRateTypeFinderData();
        }
    },

    setRateTypeFinderData: function () {
        if (receiptUI.finderData.RateType) {
            receiptUI.receiptModel.Data.RateType(receiptUI.finderData.RateType);
            receiptUI.isWrongRateType = true;
            receiptUI.receiptModel.Data.ExchangeRate(1);
            receiptRepository.refresh(receiptUI.receiptModel.Data);
        }
    },

    currencyRateFinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUISuccess.setCurrencyRateFinderData();
        }
    },

    setCurrencyRateFinderData: function () {
        var data = receiptUI.receiptModel.Data;
        data.ExchangeRate(receiptUI.finderData.Rate);
        if (data.ReceiptType() === type.RECEIPT) {
            data.RateDate(receiptUI.finderData.RateDate);
        }
        var grid = $('#ReceiptGrid').data("kendoGrid");
        if (grid.dataSource.data().length > 0) {
            receiptUI.isWrongExchangeRate = true;
            receiptRepository.refresh(data);
        }
    },

    vendorResult: function (result) {
        var data = receiptUI.receiptModel.Data;
        if (result.VendorNumber) {
            data.VendorNumber(result.VendorNumber);
            $("#Data_VendorNumber").val(result.VendorNumber);
        }
        if (result.ShortName) {
            data.VendorShortName(result.ShortName);
            $("#Data_VendorName").val(result.ShortName);
        }
        if (result.RateType) {
            data.RateType(result.RateType);
            $("#Data_RateType").val(result.RateType);
        }
        receiptRepository.refresh(data);
    },

    locationGridFinderSuccess: function (data) {
        if (data) {
            $("#message").empty();
            receiptUI.finderData = data;
            var grid = $('#ReceiptGrid').data("kendoGrid");
            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
            // Setting Location in grid row .
            currentRowGrid.set("Location", data.LOCATION);
            receiptUI.cursorforFinder = "";
            receiptUI.cursorforFinder = "Location";
            receiptRepository.setItemGridValuesModel(currentRowGrid, gridRowFields.Location);
        }
    },

    uomGridFinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUISuccess.setUOMGridFinderData();
        }
    },
    setUOMGridFinderData: function () {
        receiptGridUtility.setUOMRow(receiptUI.finderData);
    },

    itemGridFinderSuccess: function (data) {
        if (data) {
            $("#message").empty();
            receiptUI.finderData = data;
            var grid = $('#ReceiptGrid').data("kendoGrid");
            var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
            currentRowGrid.set("ItemNumber", data.ItemNumber);
            receiptUI.cursorforFinder = "";
            receiptUI.cursorforFinder = "ItemNumber";
            receiptRepository.getItemType(data.ItemNumber);
        }
    },

    setItemGridFinderData: function () {
        receiptGridUtility.setItemAndDescriptionRow(receiptUI.finderData);
    },

    receiptFinderCancel: function () {
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    ItemGridDescriptionSuccess: function (jsonResult) {
        var description = (jsonResult && jsonResult.Data.length) ? jsonResult.Data[0].Description : "";
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        currentRowGrid.set("ItemDescription", description);
    },

    ReceiptCurrencyResult: function (result) {
        var model = receiptUI.receiptModel;
        var data = model.Data;
        if (result) {
            if (result.Data && result.Data.length > 0) {
                var description = result.Data[0].Description || "";
                var currencyCodeId = result.Data[0].CurrencyCodeId || "";
                var decimal = result.Data[0].DecimalPlacesString;
                model.ReceiptCurrencyDescription(description);
                model.ReceiptCurrDecimal(decimal);
                model.FuncDecimals(decimal);
                $("#txtReceiptCurrencyDescription").val(description);
                $("#txtExtendedCostCurrency").val(currencyCodeId);
            } else {
                data.ReceiptCurrency(result.CurrencyCodeId);
                model.ReceiptCurrDecimal(result.DecimalPlacesString);
                model.ReceiptCurrencyDescription(result.Description);
                $("#txtReceiptCurrencyDescription").val(result.Description);
            }
            receiptRepository.GetExchangeRate(data.RateType(), data.ReceiptCurrency(), data.RateDate(), data.ExchangeRate(), data.HomeCurrency());
        }
    },

    CostCurrencyResult: function (result) {
        var model = receiptUI.receiptModel;
        if (result) {
            if (result.Data && result.Data.length > 0) {
                var description = result.Data[0].Description || "";
                var currencyCodeId = result.Data[0].CurrencyCodeId || "";
                model.AddlCostCurrencyDescription(description);
                model.AddCostCurrDecimal(result.Data[0].DecimalPlacesString);
                $("#txtAddlCostCurrencyDescription").val(description);
                $("#txtTotalCostCurrency").val(currencyCodeId);
            } else {
                model.Data.AdditionalCostCurrency(result.CurrencyCodeId);
                model.AddlCostCurrencyDescription(result.Description);
                model.AddCostCurrDecimal(result.DecimalPlacesString);
                $("#txtAddlCostCurrencyDescription").val(result.Description);
            }
        }
    },

    unitOfMeasure: function (result) {
        if (result) {
            $("#message").empty();
            var grid = $('#ReceiptGrid').data("kendoGrid");
            var gridData = window.sg.utls.kndoUI.getSelectedRowData(grid);
            if (gridData) {
                if (result.UnitOfMeasure) {
                    gridData.set("UnitOfMeasure", result.UnitOfMeasure);
                } else if (result.Data && result.Data.length > 0) {
                    gridData.set("UnitOfMeasure", result.Data[0].UnitOfMeasure);
                } else {
                    receiptGridUtility.resetFocus(gridData, "UnitOfMeasure");
                }
            }
        }
    },

    //Check isExistsDetailField
    isExistsDetailField: function (rowdata) {
        if (rowdata.IsFieldExists) {
            setTimeout(function () { sg.utls.showMessageInfo(sg.utls.msgType.ERROR, rowdata.UserMessage.Errors[0].Message); }, 25);
            var grid = $(receiptUI.duplicateCheckGrid.selector).data("kendoGrid");
            var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", rowdata.DisplayIndex);

            currentRowGrid.set("OptionalField", "");
            currentRowGrid.set("OptionalFieldDescription", "");
            currentRowGrid.set("Value", "");
            currentRowGrid.set("ValueDescription", "");
            currentRowGrid.set("ValueSet", "0");
        } else {
            $('#message').empty();
            $("#" + receiptUI.btnAddLineID).attr("disabled", false);
        }
    },

    //save OptionalFields method
    saveOptionalFields: function (result) {
        ko.mapping.fromJS([], {}, receiptUI.receiptModel.Data.ReceiptDetailOptionalField.Items);
        ko.mapping.fromJS([], {}, receiptUI.receiptModel.Data.ReceiptOptionalField.Items);
        if (result && result.UserMessage === undefined) {
            receiptUI.OptionalFieldError = false;
            receiptUI.optionalFieldPopUpClose = true;
            if (receiptUI.isOptionalField) {
                receiptRepository.refreshOptField();
                $("#optionalField").data("kendoWindow").close();
                receiptUI.isOptionalField = false;
            }
            if (receiptUI.isDetailOptionalField) {
                $("#detailOptionalField").data("kendoWindow").close();
                receiptUI.isDetailOptionalField = false;
                var cell = receiptGridUtility.getCurrentRowCell(receiptDetailColumnIndex.OptionalFields);
                var grid = $('#ReceiptGrid').data("kendoGrid");
                grid.editCell(cell);
                var gridRow = sg.utls.kndoUI.getSelectedRowData(grid);
                gridRow.set("OptionalFieldString", "");
            }

        } else {
            if (result) {
                receiptUI.OptionalFieldError = true;
                receiptUI.optionalFieldPopUpClose = true;
                sg.utls.showMessage(result);
                if (receiptUI.isDetailOptionalField) {
                    $("#detailOptionalField").data("kendoWindow").close();
                } else {
                    $("#optionalField").data("kendoWindow").close();
                }
            } else {
                receiptUI.optionalFieldPopUpClose = true;
                if (receiptUI.isDetailOptionalField) {
                    $("#detailOptionalField").data("kendoWindow").close();
                } else {
                    $("#optionalField").data("kendoWindow").close();
                }
            }
        }
    },

    //Just a success for setting detail pointer to current row. No action is needed currently.
    setDetail: function (resultData) {
        if (resultData && resultData.Data) {
        }
    },

    //Refresh the Detail
    refreshDetail: function (result) {
        receiptGridUtility.isDataRefreshInProgress = false;
        if (result && result.UserMessage && result.UserMessage.IsSuccess) {
            var grid = $('#ReceiptGrid').data("kendoGrid");
            var detailData = result.Data.ReceiptDetail.Items[0];
            var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", result.Data.ReceiptDetail.Items[0].DisplayIndex);
            currentRowGrid.set("OptionalFields", sg.controls.GetString(detailData.OptionalFields));
            currentRowGrid.set("OptionalFieldString", sg.controls.GetString(detailData.OptionalFieldString));
            currentRowGrid.set("ItemNumber", sg.controls.GetString(detailData.ItemNumber));
            currentRowGrid.set("ItemDescription", sg.controls.GetString(detailData.ItemDescription));
            currentRowGrid.set("UnitOfMeasure", sg.controls.GetString(detailData.UnitOfMeasure));
            currentRowGrid.set("Category", sg.controls.GetString(detailData.Category));
            if (detailData.Comments) {
                currentRowGrid.set("Comments", sg.controls.GetString(detailData.Comments));
            }
            currentRowGrid.set("Labels", sg.controls.GetString(detailData.Labels));

            if (detailData.Location) {
                currentRowGrid.set("Location", sg.controls.GetString(detailData.Location));
            }

            currentRowGrid.set("UnitCost", sg.controls.GetString(detailData.UnitCost));
            currentRowGrid.set("ExtendedCost", sg.controls.GetString(detailData.ExtendedCost));
            if (receiptUI.manufactureItemNumner !== null) {
                currentRowGrid.set("ManufacturersItemNumber", receiptUI.manufactureItemNumner);
            }
            currentRowGrid.set("ReturnCost", sg.controls.GetString(detailData.ReturnCost));
            currentRowGrid.set("AdjustedCost", sg.controls.GetString(detailData.AdjustedCost));
            currentRowGrid.set("AdjustedUnitCost", sg.controls.GetString(detailData.AdjustedUnitCost));
            currentRowGrid.set("ReturnCost", sg.controls.GetString(detailData.ReturnCost));
            currentRowGrid.set("StockItem", sg.controls.GetString(detailData.StockItem));
        }

        if (result.UserMessage.IsSuccess && result.Data.ReceiptDetail.Warnings.length > 0) {
            $.each(result.Data.Invoices.Warnings, function (index, value) {
                sg.utls.showMessageInfo(sg.utls.msgType.WARNING, value.Message);
            });
        } else {
            sg.utls.showMessage(result);
        }
    },

    getItemValuesResult: function (jsonResult) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);

        if (jsonResult&& jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                sg.controls.enable($("#btnDetailAddLine"));
            }
            // Setting Item Number Details all other columns in grid row based on value of Item.  
            currentRowGrid.set("ItemNumber", sg.controls.GetString(jsonResult.Data.ItemNumber));
            currentRowGrid.set("ItemDescription", sg.controls.GetString(jsonResult.Data.ItemDescription));
            currentRowGrid.set("Category", sg.controls.GetString(jsonResult.Data.Category));

            if (jsonResult.Data.Location) {
                currentRowGrid.set("Location", sg.controls.GetString(jsonResult.Data.Location));
            }
            currentRowGrid.set("QuantityReceived", sg.controls.GetString(jsonResult.Data.QuantityReceived));
            currentRowGrid.set("UnitOfMeasure", sg.controls.GetString(jsonResult.Data.UnitOfMeasure));
            receiptUI.oldUnitOfMeasure = jsonResult.Data.UnitOfMeasure;

            currentRowGrid.set("UnitCost", jsonResult.Data.UnitCost);
            currentRowGrid.set("Comments", sg.controls.GetString(jsonResult.Data.Comments));
            //currentRowGrid.set("Labels", sg.controls.GetString(jsonResult.Data.Labels));
            if (receiptUI.receiptModel.Data.RequireLabels() === 1 && jsonResult.Data.QuantityReceived) {
                currentRowGrid.set("Labels", jsonResult.Data.QuantityReceived);
            }
            currentRowGrid.set("ExtendedCost", jsonResult.Data.ExtendedCost);

            if (receiptUI.manufactureItemNumner) {
                currentRowGrid.set("ManufacturersItemNumber", receiptUI.manufactureItemNumner);
            }
            else if (jsonResult.Data.ManufacturersItemNumber) {
                currentRowGrid.set("ManufacturersItemNumber", jsonResult.Data.ManufacturersItemNumber);
            }
            currentRowGrid.set("ReturnCost", sg.controls.GetString(jsonResult.Data.ReturnCost));
            currentRowGrid.set("AdjustedCost", sg.controls.GetString(jsonResult.Data.AdjustedCost));
            currentRowGrid.set("AdjustedUnitCost", jsonResult.Data.AdjustedUnitCost);
            currentRowGrid.set("QuantityReturned", jsonResult.Data.QuantityReturned);
            currentRowGrid.set("StockItem", jsonResult.Data.StockItem);
            currentRowGrid.set("IsNewLine", jsonResult.Data.IsNewLine);

        } else {
            setTimeout(function () {
                receiptGridUtility.resetFocus(currentRowGrid, receiptUI.cursorField);
            });
        }
        if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning ) {

            if (receiptUI.cursorField === "UnitOfMeasure" || receiptUI.cursorforFinder === "UnitOfMeasure") {
                currentRowGrid.set("UnitOfMeasure", sg.controls.GetString(receiptUI.oldUnitOfMeasure));
            }
            sg.utls.showMessage(jsonResult);
            setTimeout(function () {
                receiptGridUtility.resetFocus(currentRowGrid, receiptUI.cursorField);
            });
        }
        if (receiptUI.cursorforFinder === "ItemNumber" || receiptUI.cursorforFinder === "Location" || receiptUI.cursorforFinder === "UnitOfMeasure") {
            receiptGridUtility.resetFocus(currentRowGrid, receiptUI.cursorforFinder);
            receiptUI.cursorforFinder = "";
        }
    },

    getItemValues: function (jsonResult) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
        var value = currentRowGrid.QuantityReceived;

        if (jsonResult && jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            currentRowGrid.set("UnitOfMeasure", sg.controls.GetString(jsonResult.Data.UnitOfMeasure));
            currentRowGrid.set("UnitCost", jsonResult.Data.UnitCost);
            currentRowGrid.set("ExtendedCost", jsonResult.Data.ExtendedCost);
            currentRowGrid.set("ReturnCost", sg.controls.GetString(jsonResult.Data.ReturnCost));
            var requireLabel = (receiptUI.receiptModel.Data.RequireLabels() === 1) || $("#chkRequireLabel").prop('checked');
            if (requireLabel && value) {
                currentRowGrid.set("Labels", value);
            }
            //if (receiptUI.receiptModel.Data.RequireLabels() === 1 && jsonResult.Data.QuantityReceived) {
            //    currentRowGrid.set("Labels", jsonResult.Data.QuantityReceived);
            //}
        }
    },

    readHeader: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data !== null) {
                var data = receiptUI.receiptModel.Data;
                data.TotalCostReceiptAdditional(jsonResult.Data.TotalCostReceiptAdditional);
                data.TotalReturnCost(jsonResult.Data.TotalReturnCost);
                data.TotalAdjCostReceiptAddl(jsonResult.Data.TotalAdjCostReceiptAddl);
                data.TotalExtendedCostSource(jsonResult.Data.TotalExtendedCostSource);
                data.TotalExtendedCostAdjusted(jsonResult.Data.TotalExtendedCostAdjusted);
                data.TotalCostReceiptAdditionalDecimal(jsonResult.Data.TotalCostReceiptAdditionalDecimal);
                receiptGridUtility.updateNumericTextBox("txtTotalCost", data.TotalCostReceiptAdditional());
                receiptGridUtility.updateNumericTextBox("txtTotalReturnCost", data.TotalReturnCost());
                receiptGridUtility.updateNumericTextBox("txtTotalAdjustmentCost", data.TotalAdjCostReceiptAddl());
                if (receiptUI.refreshClicked === true && receiptUI.createNewLine === false) {
                    receiptUI.refreshClicked = false;
                    receiptGridUtility.refreshReceiptDetailGrid();
                }
            }
        }
        if (jsonResult.UserMessage.Warnings || jsonResult.UserMessage.Errors) {
            sg.utls.showMessage(jsonResult);
        }
        receiptUI.refreshClicked = false;
    },

    refresh: function (result) {
        $("#message").empty();
        if (result && !result.UserMessage.IsSuccess) {
            receiptUI.isVendorNumberCorrect = false;
            if (receiptUI.isWrongExchangeRate === true || receiptUI.isWrongRateType === true) {
                if (receiptUI.exchangeRateOldValue) {
                    receiptUI.receiptModel.Data.ExchangeRate(receiptUI.exchangeRateOldValue);
                    receiptUI.prevExRate = receiptUI.exchangeRateOldValue;
                }
                if (receiptUI.exchangeRateOldValue === null && receiptUI.prevExRate) {
                    receiptUI.receiptModel.Data.ExchangeRate(receiptUI.prevExRate);
                }
                if (receiptUI.RateTypeOldValue) {
                    receiptUI.receiptModel.Data.RateType(receiptUI.RateTypeOldValue);
                }
                receiptUI.exchangeRateOldValue = null;
                receiptUI.isWrongRateType = false;
            }
            if (receiptUI.additionalCostCurrencyClicked && receiptUI.oldAdditionalCostCurrency) {
                receiptUI.additionalCostCurrencyClicked = false;
                receiptUI.receiptModel.Data.AdditionalCostCurrency(receiptUI.oldAdditionalCostCurrency);
            }
            sg.utls.showMessage(result);
            receiptUI.isReceiptCurrency = false;
        } else {
            receiptUI.isVendorNumberChanged = false;
            if (receiptUI.isVendorNumberChanged && result.VendorExists === 0) {
                receiptUI.isVendorNumberCorrect = false;
                receiptUI.isVendorNumberChanged = false;
            } else {
                receiptUI.isVendorNumberCorrect = true;
            }

            receiptUI.isReceiptCurrency = true;
            var details = ko.mapping.toJS(receiptUI.receiptModel.Data.ReceiptDetail);
            ko.mapping.fromJS(result.Data, {}, receiptUI.receiptModel.Data);
            ko.mapping.fromJS(result, {}, receiptUI.receiptModel);
            ko.mapping.fromJS(details, {}, receiptUI.receiptModel.Data.ReceiptDetail);

            var data = receiptUI.receiptModel.Data;
            if (data.ExchangeRate()) {
                receiptUI.exchangeRateOldValue = data.ExchangeRate();
            }

            if (receiptUI.additionalCostCurrencyClicked && data.AdditionalCostCurrency()) {
                receiptUI.oldAdditionalCostCurrency = data.AdditionalCostCurrency();
            }
            if (data.RateType()) {
                receiptUI.RateTypeOldValue = data.RateType();
            }
            if (receiptUI.receiptModel.UIMode() !== sg.utls.OperationMode.SAVE) {

                if (data.VendorExists() === 0 && data.VendorNumber()) {
                    sg.utls.showMessageInfo(sg.utls.msgType.WARNING, $.validator.format(receiptResources.RecordDoesNotExist, receiptResources.VendorNumber, data.VendorNumber()));
                    sg.controls.Focus($("#Data_VendorNumber"));
                }
            }

            //Updating the kendo numeric text box values
            receiptGridUtility.updateTextBox();
            if (result) {
                data.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
                data.TotalReturnCostDecimal(result.TotalReturnCostDecimal);
            }
            if (result.Warnings) {
                sg.utls.showMessageInfo(sg.utls.msgType.WARNING, result.Warnings[0].Message);
            }
        }
    },

    //Set OptionalField Value on response
    setOptionalFieldValue: function (result) {
        optionalFieldUIGrid.isDataRefreshInProgress = false;
        var gridId = (receiptUI.isDetailOptionalField) ? "#DetailOptionalFieldGrid" : "#OptionalFieldGrid";
        var grid = $(gridId).data("kendoGrid");
        var data = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", result.DisplayIndex);
        var isNumericType;

        if (receiptUI.isDetailOptionalField) {
            $("#windowmessage").empty();
            if (result.UserMessage ) {
                if (!result.UserMessage.IsSuccess) {
                    if (data !== null && !optionalFieldUIGrid.valueChanged) {
                        optionalFieldUIGrid.preventRecursive = true;
                        isNumericType = [6, 8, 100].indexOf(data.Type) > -1;
                        data.set("Value", isNumericType ? 0 : "");
                        data.set("ValueDescription", "");
                    }
                }
            } else {
                data.set("ValueDescription", result.ValueDescription);
                data.set("ValueSetString", result.ValueSetString || "");
                if (result.Type === receiptTypeEnum.Time && result.TimeValue) {
                    data.set("Value", result.Value);
                }
            }
            grid.refresh();
            sg.utls.showMessagePopupWithoutClose(result, "#windowmessage");
        } else if (receiptUI.isOptionalField) {
            $("#windowmessage1").empty();
            if (result.UserMessage) {
                if (!result.UserMessage.IsSuccess) {
                    if (data !== null && !optionalFieldUIGrid.valueChanged) {
                        optionalFieldUIGrid.preventRecursive = true;
                        isNumericType = [6, 8, 100].indexOf(data.Type) > -1;
                        data.set("Value", isNumericType ? 0 : "");
                        data.set("ValueDescription", "");
                    }
                }
            } else {
                data.set("ValueDescription", result.ValueDescription);
                data.set("ValueSetString", result.ValueSetString ||"");
                if (result.Type === receiptTypeEnum.Time && result.TimeValue) {
                    data.set("Value", result.Value);
                }
            }
            grid.refresh();
            sg.utls.showMessagePopupWithoutClose(result, "#windowmessage1");
        }
    },

    fillOptionalFieldFinderData: function (result) {
        optionalFieldUIGrid.isDataRefreshInProgress = false;
        var gridId = receiptUI.isDetailOptionalField ? "#DetailOptionalFieldGrid" : "#OptionalFieldGrid";
        var grid = $(gridId).data("kendoGrid");
        var data = sg.utls.kndoUI.getSelectedRowData(grid);

        if (result.UserMessage) {
            if (!result.UserMessage.IsSuccess) {
                receiptUI.optionalFieldPopUpClose = true;
                if (data && !optionalFieldUIGrid.valueChanged) {
                    data.set("OptionalField", "");
                    data.set("IsNewLine", true);
                    data.set("OptionalFieldDescription", "");
                    data.set("Type", "");
                    data.set("Length", "");
                    data.set("ValueSet", "");
                    data.set("ValueSetString", "");
                    data.set("Value", "");
                    data.set("ValueDescription", "");
                    data.set("Decimals", "");
                    data.set("Validate", "");
                    sg.utls.showMessagePopupWithoutClose(result, receiptUI.isDetailOptionalField ? "#windowmessage" : "#windowmessage1");
                    optionalFieldUIGrid.resetFocus(data, 'OptionalField');
                }

            }
        } else if (data) {
            if (receiptUI.isDetailOptionalField) {
                $("#windowmessage").empty();
            } else {
                $("#windowmessage1").empty();
            }
            $("#message").empty();
            var count = 0;
            var optionalFieldKey = result.OptionalField;
            data.set("OptionalField", result.OptionalField);
            receiptUI.optionalFieldPopUpClose = false;

            $.each(grid.dataSource.data(), function (key, value) {
                if (value.OptionalField === optionalFieldKey) {
                    count += 1;
                }
            });
            if (count > 1) {
                if (receiptUI.isDetailOptionalField) {
                    receiptUI.constructErrorMessage(optionalFieldKey, data, receiptResources.ReceiptDetailOptField, "#windowmessage");
                } else {
                    receiptUI.constructErrorMessage(optionalFieldKey, data, receiptResources.ReceiptOptField, "#windowmessage1");
                }
                return;
            }
            data.set("IsNewLine", true);
            data.set("OptionalField", result.OptionalField);
            data.set("OptionalFieldDescription", result.OptionalFieldDescription);
            data.set("Type", result.Type);
            data.set("Length", result.Length);
            data.set("ValueSet", result.ValueSet);
            data.set("ValueSetString", result.ValueSetString);
            if (data["Value"] !== result.Value) {
                optionalFieldUIGrid.preventRecursive = true;
                if (result.Type === 3) {
                    data.set("Value", sg.utls.kndoUI.convertStringToDate(result.Value));
                } else {
                    data.set("Value", result.Value);
                }
            }

            data.set("ValueDescription", result.ValueDescription);
            data.set("Decimals", result.Decimals);
            data.set("Validate", result.Validate);
            sg.utls.showMessagePopupWithoutClose(result, "#windowmessage1");
            optionalFieldUIGrid.resetFocus(data, 'Value');
        }
        receiptUI.optionalFieldPopUpClose = false;
    },

    onDeleteSuccess: function () {
        var grid = $('#OptionalFieldGrid').data("kendoGrid");
        if (grid._data.length > 0) {
            for (var i = 0; i < grid._data.length; i++) {
                grid._data[i].IsDeleted = false;
                grid._data[i].IsNewLine = false;
                grid._data[i].HasChanged = false;
            }
        }
        grid.refresh();
        ko.mapping.fromJS(grid._data, [], receiptUI.receiptModel.Data.ReceiptOptionalField.Items);
        receiptUI.receiptModel.Data.ReceiptOptionalField.TotalResultsCount(grid._data.length);
    },

    onDetailDeleteSuccess: function () {
        var grid = $('#DetailOptionalFieldGrid').data("kendoGrid");
        if (grid._data.length > 0) {
            for (var i = 0; i < grid._data.length; i++) {
                grid._data[i].IsDeleted = false;
                grid._data[i].IsNewLine = false;
                grid._data[i].HasChanged = false;
            }
        }
        grid.refresh();
        ko.mapping.fromJS(grid._data, [], receiptUI.receiptModel.Data.ReceiptDetailOptionalField.Items);
        receiptUI.receiptModel.Data.ReceiptDetailOptionalField.TotalResultsCount(grid._data.length);
    }
};

var receiptFilter = {

    getReceiptFilter: function () {
        var filters = [[]];
        var data = receiptUI.receiptModel.Data;
        if (sg.controls.GetString(data.ReceiptNumber() !== "")) {
            if (sg.controls.GetString(receiptUI.receiptModel.DefaultReceiptNumber()) !== sg.controls.GetString(data.ReceiptNumber())) {
                filters[0][0] = sg.finderHelper.createFilter("ReceiptNumber", sg.finderOperator.StartsWith, data.ReceiptNumber());
            } else {
                filters[0][0] = sg.finderHelper.createFilter("ReceiptNumber", sg.finderOperator.StartsWith, "");
            }
        }
        return filters;
    },

    //Location filter
    locationFilter: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        var filters = [[]];
        var unformattedItem = currentRowGrid.ItemNumber.replace(/[^a-zA-Z0-9]/g, '');
        filters[0][1] = sg.finderHelper.createFilter("ItemNumber", sg.finderOperator.Equal, unformattedItem);
        filters[0][0] = sg.finderHelper.createFilter("LOCATION", sg.finderOperator.StartsWith, currentRowGrid.Location);
        return filters;
    },

    //uom  filter
    uomFilter: function () {
        var filters = [[]];
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        var unformattedItem = currentRowGrid.ItemNumber.replace(/[^a-zA-Z0-9]/g, '');
        filters[0][0] = sg.finderHelper.createFilter("UnitOfMeasure", sg.finderOperator.StartsWith, currentRowGrid.UnitOfMeasure);
        filters[0][1] = sg.finderHelper.createFilter("ItemNumber", sg.finderOperator.Equal, unformattedItem);
        filters[0][1].IsMandatory = true;
        return filters;
    },

    getitemNumberFilter: function () {
        var filters = [[]];
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var selectedRowData = window.sg.utls.kndoUI.getSelectedRowData(grid);

        filters[0][0] = sg.finderHelper.createFilter("ItemNumber", sg.finderOperator.StartsWith, selectedRowData.ItemNumber.toUpperCase());
        return filters;
    },

    onFinderCancel: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var selectedRowData = window.sg.utls.kndoUI.getSelectedRowData(grid);
        receiptRepository.getDefaultItemNumber(selectedRowData.ItemNumber.toUpperCase(), receiptUISuccess.manufacturerItem);
    },

    onItemCancel: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var selectedRowData = window.sg.utls.kndoUI.getSelectedRowData(grid);
        receiptGridUtility.resetFocus(selectedRowData, "ItemNumber");
    },

    onLocationCancel: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var selectedRowData = window.sg.utls.kndoUI.getSelectedRowData(grid);
        receiptGridUtility.resetFocus(selectedRowData, "Location");
    },

    onUOMCancel: function () {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var selectedRowData = window.sg.utls.kndoUI.getSelectedRowData(grid);
        receiptGridUtility.resetFocus(selectedRowData, "UnitOfMeasure");
    },

    //Exchange rate filter
    exchangeRateFilter: function () {
        var data = receiptUI.receiptModel.Data;
        var rateDate = data.RateDate();
        var currency = data.ReceiptCurrency();
        var rateType = data.RateType();

        var filters = [[]];
        filters[0][0] = sg.finderHelper.createFilter("RateType", sg.finderOperator.Equal, rateType.toUpperCase());
        filters[0][0].IsMandatory = true;
        filters[0][1] = sg.finderHelper.createFilter("FromCurrency", sg.finderOperator.Equal, currency);
        filters[0][1].IsMandatory = true;
        filters[0][2] = sg.finderHelper.createFilter("ToCurrency", sg.finderOperator.Equal, sg.utls.homeCurrency.Code);
        filters[0][2].IsMandatory = true;
        filters[0][3] = sg.finderHelper.createFilter("RateDate", sg.finderOperator.GreaterThanOrEqual, sg.utls.kndoUI.getFormattedDate(rateDate));
        filters[0][3].IsMandatory = true;
        return filters;
    },

    // Set the Manufacturers ItemNumber as a filter value.
    getManufacturerFieldFilter: function () {
        var manufacturerItemFilters = [[]];
        var itemNumber = receiptUI.manufactureItemNumber;
        var filter = sg.finderHelper.createFilter("ManufacturersItemNumber", sg.finderOperator.StartsWith, itemNumber);
        manufacturerItemFilters[0][0] = filter;
        manufacturerItemFilters[0][0].IsMandatory = true;
        return manufacturerItemFilters;
    },

    getRateTypeFilter: function () {
        var filters = [[]];
        var rateType = $("#Data_RateType").val().toUpperCase();
        filters[0][0] = sg.finderHelper.createFilter("RateType", sg.finderOperator.StartsWith, rateType);
        return filters;
    },

    getVendorFilter: function () {
        var filters = [[]];
        var vendorNumberFilter = $('#Data_VendorNumber').val();
        filters[0][0] = window.sg.finderHelper.createFilter("VendorNumber", window.sg.finderOperator.StartsWith, vendorNumberFilter);
        return filters;
    },
};

/********************************************************************receipt Detail Grid Start*********************************************************/
var receiptGridUtility = {
    isCellEditable: true,
    isDataRefreshInProgress: false,
    selectedIndex: 0,

    updateNumericTextBox: function (id, value) {
        var numericTextbox = $("#" + id).data("kendoNumericTextBox");
        var decimal;
        var data = receiptUI.receiptModel.Data;

        if (data.AdditionalCostCurrency() === data.ReceiptCurrency()) {
            decimal = data.ReceiptCurrencyDecimals();
        }
        else {
            decimal = receiptUI.receiptModel.FuncDecimals();
        }
        if (id === "txtTotalCost") {
            decimal = data.TotalCostReceiptAdditionalDecimal();
        }
        if (id === "txtTotalReturnCost") {
            decimal = data.ReceiptCurrencyDecimals();
        }
        if (id === "txtTotalAdjustmentCost") {
            decimal = data.TotalCostReceiptAdditionalDecimal();
        }

        if (numericTextbox ) {
            if (id === "txtpopupExchangeRate" || id === 'Data_ExchangeRate') {
                numericTextbox.options.format = "n7";
                numericTextbox.options.decimals = 7;
                sg.utls.kndoUI.restrictDecimals(numericTextbox, 7, 8);
                numericTextbox.value(value);
            }

            var ids = ["txtAddlCost", "txtTotalCost", "txtTotalReturnCost", "txtTotalAdjustmentCost"];
            if (ids.indexOf(id) > -1) {
                numericTextbox.options.format = "n" + decimal;
                numericTextbox.options.decimals = decimal;
                sg.utls.kndoUI.restrictDecimals(numericTextbox, decimal, 13);
                numericTextbox.value(value);
            }
        }
    },

    updateTextBox: function () {
        var data = receiptUI.receiptModel.Data;
        receiptGridUtility.updateNumericTextBox("txtAddlCost", data.AdditionalCost());
        receiptGridUtility.updateNumericTextBox("txtpopupExchangeRate", data.ExchangeRate());
        receiptGridUtility.updateNumericTextBox("Data_ExchangeRate", data.ExchangeRate());
        receiptGridUtility.updateNumericTextBox("txtTotalCost", data.TotalCostReceiptAdditional());
        receiptGridUtility.updateNumericTextBox("txtTotalReturnCost", data.TotalReturnCost());
        receiptGridUtility.updateNumericTextBox("txtTotalAdjustmentCost", data.TotalAdjCostReceiptAddl());
    },

    //Set Grid data once get paged called the response will bind the grid
    setGridData: function (result) {
        if (result.UserMessage) {
            var isDirty = receiptUI.receiptModel.isModelDirty.isDirty();
            sg.utls.showMessage(result);
            var grid = $('#ReceiptGrid').data("kendoGrid");
            var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptGridUtility.selectedIndex);
            if (currentRowGrid) {
                receiptGridUtility.resetFocus(currentRowGrid, "1");
                grid.closeCell();
            }
            else {
                if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                    sg.controls.enable($("#btnDetailAddLine"));
                }
                return;
            }
            if (!isDirty) {
                receiptUI.receiptModel.isModelDirty.reset();
            }
        }
        var gridData = null;

        if (!result.Data) {
            if (result && result.TotalResultsCount > 0) {
                gridData = [];
                ko.mapping.fromJS(result, {}, receiptUI.receiptModel.Data.ReceiptDetail);
                gridData.data = result.Items;
                gridData.totalResultsCount = result.TotalResultsCount;
            } else if (result.ReceiptDetail && result.ReceiptDetail.TotalResultsCount > 0) {
                gridData = [];
                ko.mapping.fromJS(result.ReceiptDetail, {}, receiptUI.receiptModel.Data.ReceiptDetail);
                gridData.data = result.ReceiptDetail.Items;
                gridData.totalResultsCount = result.ReceiptDetail.TotalResultsCount;
            } else {
                gridData = [];
                ko.mapping.fromJS(result, {}, receiptUI.receiptModel.Data.ReceiptDetail);
                gridData.data = result.Items;
                gridData.totalResultsCount = result.TotalResultsCount;
            }
        }
        else if (result.Data.ReceiptDetail) {
            gridData = [];
            ko.mapping.fromJS(result.Data.ReceiptDetail, {}, receiptUI.receiptModel.Data.ReceiptDetail);
            gridData.data = result.Data.ReceiptDetail.Items;
            gridData.totalResultsCount = result.Data.ReceiptDetail.TotalResultsCount;
        }

        $("#selectAllChk").attr("checked", false).parent().attr("class", "icon checkBox");
        sg.controls.enable("#selectAllChk");
        sg.controls.disable("#btnDetailDeleteLine");
        if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
            sg.controls.enable($("#btnDetailAddLine"));
        } else {
            sg.controls.disable("#btnDetailAddLine");
        }

        receiptUI.validateGridRow();
        sg.utls.showMessage(result);

        return gridData;
    },

    //Set line editable 
    setLineEditable: function () {

        receiptGrid.setLineEditable = true;
        var gridDiv = $("#ReceiptGrid");
        var detailGrid = gridDiv.data("kendoGrid");
        var cell = detailGrid.tbody.find(">tr:first >td:eq(" + 1 + ")");
        //To disable the header checkbox when there items in the grid are cleared using Clear button
        if (receiptUI.receiptModel.Data.ReceiptDetail.Items() && receiptUI.receiptModel.Data.ReceiptDetail.Items().length === 0) {
            $("#selectAllChk").attr("disabled", true);
        } else {
            sg.controls.enable("#selectAllChk");
        }

        //Make the first column editable and first line selected when Add line is clicked
        if (receiptUI.addLineClicked) {
            var editableRow = receiptUI.insertedIndex + 1;
            if (detailGrid.dataSource.data().length === 1) {
                editableRow = 0;
            }
            if (receiptUI.indexOfPage === 10) {
                editableRow = 0;
            }

            cell = detailGrid.tbody.find(">tr:eq(" + editableRow + ") >td:eq(" + 1 + ")");
            detailGrid.editCell(cell);
            receiptUI.addLineClicked = false;
        }

        if (receiptGrid.moveToNextPage) {
            receiptGrid.moveToNextPage = false;
            receiptGrid.createNewRecord = true;
            detailGrid.dataSource.page(detailGrid.dataSource.page() + 1);
        }
        if (receiptGrid.createNewRecord) {
            var total = receiptUI.receiptModel.Data.ReceiptDetail.TotalResultsCount();
            receiptUI.receiptModel.Data.ReceiptDetail.TotalResultsCount(total - 1);
            receiptGrid.createDetail();
            receiptGrid.createNewRecord = false;
            detailGrid.editCell(cell);
        }
        if (receiptGrid.setFirstLineEditable) {
            detailGrid.editCell(cell);
            receiptGrid.setFirstLineEditable = false;
        }

        var selectAllCheckBox = $("#selectAllChk");
        if (selectAllCheckBox.is(':checked')) {
            selectAllCheckBox.prop("checked", false).applyCheckboxStyle();
            $("#btnDetailDeleteLine").attr("disabled", true);
            return;
        }

        detailGrid.tbody.find(".selectChk").each(function () {
            if (!($(this).is(':checked'))) {
                $("#btnDetailDeleteLine").attr("disabled", true);
                return;
            }
        });
        if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
            sg.controls.enable($("#btnDetailAddLine"));
        }

    },

    //Remove invalid row
    removeInvalidRow: function (data) {
        var nullItem = sg.utls.ko.arrayFirstItemOf(data(), function (item) {
            return ((item.ItemNumber === "" || item.ItemNumber === null) || (item.Location === "" || item.Location === null) || (item.QuantityReceived === "" || item.QuantityReceived === 0));
        });
        data.remove(nullItem);
        return data;
    },

    // Reset focus of columns in grid
    resetFocus: function (dataItem, columnName) {
        var index = window.GridPreferencesHelper.getColumnIndex('#ReceiptGrid', columnName);
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
        grid.closeCell();
        receiptGrid.setLineEditable = true;
        grid.editCell(row.find(">td").eq(index));
    },

    //Set column non editable
    setColumnNonEditable: function (container) {
        receiptGridUtility.isCellEditable = false;
        var grid = $('#ReceiptGrid').data("kendoGrid");
        sg.utls.kndoUI.nonEditable(grid, container);
    },

    //Formate the column value
    getFormattedValue: function (fieldValue) {
        var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.ConvFactorDecimal() : 0;
        if (fieldValue) {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(!isNaN(parseFloat(fieldValue)) ? parseFloat(fieldValue) : 0, decimal);
        } else {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(0, decimal);
        }
        return fieldValue;
    },

    //Refresh the receipt detail grid
    refreshReceiptDetailGrid: function () {
        var grid = $("#ReceiptGrid").data("kendoGrid");
        grid.dataSource.data([]);
        grid.dataSource.page(1);
    },

    // Convert zero or one to no or yes
    getConvertedStatusString: function (value) {
        return (value > 0) ? statusType.Yes : statusType.No;
    },

    // set the Item And Item description column value
    setItemAndDescriptionRow: function (result) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        // Setting Item Number and item description in grid row based on selection of item finder.
        currentRowGrid.set("ItemNumber", result.ItemNumber);
        currentRowGrid.set("ItemDescription", result.Description);
    },

    // set the Category column value
    setCategoryRow: function (result) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        // Setting Category in grid row based on selection of category finder.
        currentRowGrid.set("Category", result.CategoryCode);
    },

    // set the Location column value
    setLocationRow: function (result) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        // Setting Category in grid row based on selection of category finder.
        currentRowGrid.set("Location", result.Location);
    },

    // set the unit of measure column value
    setUOMRow: function (result) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        // Setting Category in grid row based on selection of category finder.
        currentRowGrid.set("UnitOfMeasure", result.UnitOfMeasure);
        receiptUI.cursorforFinder = "";
        receiptUI.cursorforFinder = "UnitOfMeasure";
        receiptUI.oldUnitOfMeasure = null;
        receiptUI.oldUnitOfMeasure = result.UnitOfMeasure;
        receiptRepository.setItemGridValuesModel(currentRowGrid, gridRowFields.UnitOfMeasure);
    },


    // Set the check boxes of grid to check and select all and to disable the delete button if grid rows not selected 
    multiSelectInit: function (gridId, selectAllChk, selectChk, btnDetailDeleteId) {
        if ($("#" + gridId)) {

            $(document).on("change", "#" + selectAllChk, function () {
                if ($("#frmReceipt").valid()) {

                    if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var row = grid.tbody.find("tr[data-uid='" + receiptGrid.currentRow + "']");
                        var selectedRow = sg.utls.kndoUI.getDataItemForRow(row, grid);
                        var gridData = grid.dataSource.data();
                        if (selectedRow || grid.dataSource.total() > 0) {
                            for (var i = 0; i < gridData.length; i++) {
                                var item = gridData[i];
                                if (item) {
                                    if (item.ItemNumber !== "" && (item.Location === "" || (item.QuantityReceived === "" || item.QuantityReceived === 0)) && item.IsNewLine === true) {
                                        grid.dataSource.read();
                                        $(this).prop('checked', false).applyCheckboxStyle();
                                        return;
                                    }
                                }
                            }
                        }

                        receiptGridUtility.removeInvalidRow(receiptUI.receiptModel.Data.ReceiptDetail.Items);

                        var checkbox = $(this);
                        var rows = grid.tbody.find("tr");
                        rows.find("td:first input").prop("checked", checkbox.is(":checked")).applyCheckboxStyle();
                        if ($("#" + selectAllChk).is(":checked")) {
                            rows.addClass("k-state-active");
                            sg.controls.enable("#" + btnDetailDeleteId);
                        } else {
                            rows.removeClass("k-state-active");
                            sg.controls.disable("#" + btnDetailDeleteId);
                        }
                    } else {
                        sg.controls.disable("#" + selectAllChk);
                        $("#" + selectAllChk).prop("checked", false).applyCheckboxStyle();
                    }
                }
            });

            $(document).on("change", "." + selectChk, function () {
                if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                    var grid = $('#ReceiptGrid').data("kendoGrid");
                    //validate if invalid row show messages
                    var checkedRow = $(this).closest("tr");
                    checkedRow.toggleClass("k-state-selected");
                    var row = grid.tbody.find("tr[data-uid='" + receiptGrid.currentRow + "']");
                    var selectedRow = sg.utls.kndoUI.getDataItemForRow(row, grid);
                    var gridData = grid.dataSource.data();
                    if (selectedRow || grid.dataSource.total() > 0) {
                        for (var i = 0; i < gridData.length; i++) {
                            var item = gridData[i];
                            if (item) {
                                if (item.ItemNumber !== "" && (item.Location === "" || (item.QuantityReceived === "" || item.QuantityReceived === 0)) && item.IsNewLine === true) {
                                    grid.dataSource.read();
                                    $(this).prop('checked', false).applyCheckboxStyle();
                                    return;
                                }
                            }
                        }
                    }

                    $(this).closest("tr").toggleClass("k-state-active");
                    var allChecked = true;
                    var hasChecked = false;
                    grid.tbody.find("." + selectChk).each(function () {
                        if (!($(this).is(':checked'))) {
                            $("#" + selectAllChk).prop("checked", false).applyCheckboxStyle();

                            allChecked = false;
                            return;
                        } else {
                            hasChecked = true;
                        }
                    });
                    if (allChecked) {
                        $("#" + selectAllChk).prop("checked", true).applyCheckboxStyle();
                    }

                    if (hasChecked) {
                        sg.controls.enable("#" + btnDetailDeleteId);
                    } else {
                        sg.controls.disable("#" + btnDetailDeleteId);
                    }
                } else {
                    sg.controls.disable("." + selectChk);
                    $("." + selectChk).prop("checked", false).applyCheckboxStyle();
                }
            });
        }
    },

    //---- Receipt Detail Grid Button Functions -- START-- //

    //Create detail line check
    createDetailLine: function () {
        var createNewLine = false;
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "DisplayIndex", receiptUI.itemfinderlineNumber);
        var gridData = grid.dataSource.data();

        if (currentRowGrid || grid.dataSource.total() > 0) {
            var length = gridData.length;
            for (var i = 0; i < length; i++) {
                var item = gridData[i];
                if (item ) {
                    if (!item.ItemNumber) {
                        receiptGridUtility.resetFocus(currentRowGrid, "ItemNumber");
                        sg.controls.enable($("#btnDetailAddLine"));
                        return false;
                    } else if (item.ItemNumber !== "" && (item.Location === "" || (item.QuantityReceived === "" || item.QuantityReceived === 0)) && item.IsNewLine === true) {
                        if (!item.Location) {
                            receiptGridUtility.resetFocus(currentRowGrid, "Location");
                            grid.dataSource.read();
                            return false;
                        }
                        if (item.QuantityReceived === "" || item.QuantityReceived === 0) {
                            receiptGridUtility.resetFocus(currentRowGrid, "QuantityReceived");
                            grid.dataSource.read();
                            return false;
                        }
                    } else {
                        createNewLine = true;
                    }
                }
            }
            if (length === 0) {
                createNewLine = true;
            }

            if (createNewLine) {
                receiptGridUtility.addLine();
            }

        } else {
            receiptGridUtility.addLine();
        }
    },

    //Create a new line in the detail grid
    addLine: function () {
        receiptUI.addLineClicked = true;
        receiptUI.createNewLine = true;
        receiptGrid.createDetail();
    },

    // Delete line in the detail grid
    deleteLine: function (gridId, chkAllId, confirmationMsg, btnDetailDeleteId) {
        var resetToFirstPage = $("#selectAllChk").is(":checked");
        if ($('.selectChk:checked').length > 1) {
            confirmationMsg = receiptResources.DeleteLinesMessage;
        } else {
            confirmationMsg = receiptResources.DeleteLineMessage;
        }

        sg.utls.showKendoConfirmationDialog(
            //Click on Yes
            function () {
                var grid = $('#' + gridId).data("kendoGrid");
                grid.tbody.find(":checked").closest("tr").each(function () {
                    grid.removeRow($(this));
                });

                if (gridId === "ReceiptGrid") {
                    var pageNumber = grid.dataSource.page();
                    var pageSize = grid.dataSource.pageSize();
                    var total = grid.dataSource.total();

                    receiptGridUtility.removeInvalidRow(receiptUI.receiptModel.Data.ReceiptDetail.Items);
                    var model = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                    model.ReceiptDetail = ko.mapping.toJS(receiptUI.receiptModel.Data.ReceiptDetail);

                    receiptRepository.deleteDetail(pageNumber, pageSize, model);
                    receiptRepository.readHeader(receiptUI.receiptModel.Data, true);
                    $("#message").empty();
                    grid.dataSource.read();
                    if (resetToFirstPage) {
                        if (pageNumber > (total / pageSize)) {
                            grid.dataSource.page(grid.dataSource.page() - 1);
                        } else {
                            grid.dataSource.page(grid.dataSource.page());
                        }
                    }
                }
                if (grid.dataSource.total() === 0) {
                    sg.controls.disable("#" + chkAllId);
                } else {
                    sg.controls.enable("#" + chkAllId);
                }
                $("#" + chkAllId).attr("checked", false).parent().attr("class", "icon checkBox");
                sg.controls.disable("#" + btnDetailDeleteId);
            },
            // Click on No
            function () { },
            confirmationMsg, window.DeleteTitle);
        return false;
    },

    //---------------------------------------Receipt Option Field Grid Button Functions Start---------------------------------  //
    newOptionalFieldLineItem: function () {
        var sequenceNumber = receiptUI.receiptModel.Data.SequenceNumber();

        var newOptFieldLine = {
            "SequenceNumber": sequenceNumber,
            "DisplayIndex": optionalFieldUIGrid.dataIndex,
            "LineNumber": -1
        };
        return newOptFieldLine;
    },
    newDetailOptionalFieldLineItem: function () {
        var sequenceNumber = receiptUI.receiptModel.Data.SequenceNumber();
        var newOptFieldLine = {
            "SequenceNumber": sequenceNumber,
            "LineNumber": receiptUI.lineNumber,
            "DisplayIndex": optionalFieldUIGrid.dataIndex
        };
        return newOptFieldLine;
    },
    getCurrentRowCell: function (detailItem) {
        var grid = $('#ReceiptGrid').data("kendoGrid");
        var dataRows = grid.items();
        var index = dataRows.index(grid.select());
        return grid.tbody.find(">tr:eq(" + index + ") >td:eq(" + detailItem + ")");
    }
    //---------------------------------------Receipt Option Field Grid Button End---------------------------------  //
};

//Receipt grid settings
var receiptGrid = {
    isCellEditable: true,
    currentRow: null,
    setLineEditable: false,
    currentDataRow: null,
    //Init Receipt grid
    init: function () {
        receiptGridUtility.multiSelectInit("ReceiptGrid", "selectAllChk", "selectChk", "btnDetailDeleteLine");
    },

    //Create detail in the Grid
    createDetail: function () {
        var data = receiptUI.receiptModel.Data;
        if (!receiptUI.skipLineEditable) {
            receiptGrid.setFirstLineEditable = true;
        } else {
            receiptUI.skipLineEditable = false;
        }
        var detailsGrid = $('#ReceiptGrid').data("kendoGrid");
        var pageNumber = detailsGrid.dataSource.page();
        var pageSize = detailsGrid.dataSource.pageSize();
        var dataRows = detailsGrid.items();
        receiptUI.insertedIndex = dataRows.index(detailsGrid.select());

        if (receiptUI.insertedIndex < 0) {
            receiptUI.insertedIndex = 0;
        }
        if (receiptUI.receiptModel.isModelDirty && receiptUI.receiptModel.isModelDirty.isDataDirty && receiptUI.receiptModel.isModelDirty.isDataDirty()) {
            receiptUI.receiptModel.Data.HasChanged(true);
        }

        receiptGrid.config.pageUrl = sg.utls.url.buildUrl("TU", "Receipt", "CreateDetail");
        receiptGrid.config.param = {
            pageNumber: pageNumber - 1,
            pageSize: pageSize,
            index: receiptUI.insertedIndex,
            model: ko.mapping.toJS(data, receiptUI.ignoreIsDirtyProperties)
        };

        if (receiptGrid.createNewRecord) {
            receiptGrid.config.pageUrl = sg.utls.url.buildUrl("TU", "Receipt", "CreateDetail");
            receiptGrid.config.param = {
                pageNumber: pageNumber - 1,
                pageSize: pageSize,
                index: pageSize,
                model: ko.mapping.toJS(data, receiptUI.ignoreIsDirtyProperties)
            };
        }

        if (receiptUI.insertedIndex === (pageSize - 1)) {
            receiptGrid.moveToNextPage = true;
        }
        receiptUI.indexOfPage = receiptGrid.config.param.index;
        detailsGrid.dataSource.read();
        detailsGrid.tbody.find(".selectChk").each(function () {
            if (!($(this).is(':checked'))) {
                sg.controls.disable("#btnDetailDeleteLine");
                return;
            } else {
                sg.controls.enable("#btnDetailDeleteLine");
            }
        });
    },

    //Receipt grid configuration settings
    config: {
        autoBind: false,
        isServerPaging: true,
        createNewRecord: false,
        moveToNextPage: false,
        pageSize: sg.utls.gridPageSize,
        scrollable: true,
        selectable: true,
        resizable: true,
        sortable: false,
        navigatable: true,
        pageable: {
            input: true,
            numeric: false,
            refresh: false,
        },
        setFirstLineEditable: true,
        filters: null,
        reorderable: sg.utls.reorderable,
        param: null,
        getParam: function () {
            var model = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
            model.ReceiptDetail = ko.mapping.toJS(receiptUI.receiptModel.Data.ReceiptDetail);

            var grid = $('#ReceiptGrid').data("kendoGrid");
            var parameters = {
                pageNumber: grid.dataSource.page() - 1,
                pageSize: grid.dataSource.pageSize(),
                model: model,
                filter: null
            };
            return parameters;
        },

        //URL to get the data from the server. 
        pageUrl: sg.utls.url.buildUrl("TU", "Receipt", "GetPagedReceiptDetails"),

        //Call back function when Get is successful. In this, the data for the grid and the total results count are to be set along with updating knockout
        buildGridData: receiptGridUtility.setGridData,

        //Call back function after data is bound to the grid. Is used to set the added line as editable
        afterDataBind: receiptGridUtility.setLineEditable,

        //Grid columns
        columns: [
        {
            field: "Delete",
            attributes: { "class": "first-cell", sg_Customizable: false },
            headerAttributes: { "class": "first-cell" },
            template: sg.controls.ApplyCheckboxStyle("<input type='checkbox' class='selectChk newcontrol' />"),
            headerTemplate: sg.controls.ApplyCheckboxStyle("<input type='checkbox' id='selectAllChk' />"),
            reorderable: false,
            editor: function (container) {
                sg.utls.kndoUI.nonEditable($('#ReceiptGrid').data("kendoGrid"), container);
            }
        },
        {
            field: "DisplayIndex",
            title: receiptResources.LineNumber,
            attributes: { "class": " align-right" },
            headerAttributes: { "class": " align-right" },
            width: 140,
            editor: function (container) {
                sg.utls.kndoUI.nonEditable($('#ReceiptGrid').data("kendoGrid"), container);
            }
        },
        {
            field: "ItemNumber",
            title: receiptResources.ItemNumber,
            sortable: false,
            headerAttributes: { class: " k-rmain-header" },
            attributes: { class: "txt-upper" },
            width: 140,
            editor: function (container, options) {

                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.ItemNumber = options.model.ItemNumber || "";
                    var txtItemNumber = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtItemNumber" type="text"  ' +
                    'maxlength="24" formatTextbox = "" class="txt-upper grid_inpt pr25" data-bind="value:' + options.field + '"/></div>';
                    var finderItemNumber = '<div class="edit-cell inpt-finder"> <input title="' + receiptResources.Finder + '" type="button" class="icon btn-search" ' +
                    'id="btnItemNumberfield"/></div></div>';
                    receiptUI.itemfinderlineNumber = options.model.DisplayIndex;
                    receiptUI.cursorField = "ItemNumber";
                    var html = txtItemNumber + '' + finderItemNumber;

                    var itemNumberFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.ItemNumber);
                    $(html).appendTo(container);

                    sg.finderHelper.setFinder("btnItemNumberfield", sg.finder.Items, receiptUISuccess.itemGridFinderSuccess, receiptFilter.onItemCancel, itemNumberFindertitle, receiptFilter.getitemNumberFilter, null, true);

                    $("#txtItemNumber").bind('change', function (e) {
                        var itemNumber = e.target.value.toUpperCase();
                        receiptUI.manufactureItemNumber = itemNumber;

                        sg.delayOnChange("btnItemNumberfield", $("#txtItemNumber"), function () {
                            $("#message").empty();
                            var receiptGrid = $('#ReceiptGrid').data("kendoGrid");
                            var gridData = sg.utls.kndoUI.getSelectedRowData(receiptGrid);
                            receiptUI.cursorField = "ItemNumber";
                            receiptUI.itemfinderlineNumber = gridData.DisplayIndex;
                            receiptRepository.getItemType(itemNumber);
                        });
                    });
                }
            }
        },
        {
            field: "ItemDescription",
            title: receiptResources.ItemDescription,
            sortable: false,
            editable: false,
            headerAttributes: { class: " k-rmain-header" },
            attributes: { class: "" },
            width: 200,
            editor: function (container) {
                receiptGridUtility.setColumnNonEditable(container);
            }
        },

        {
            field: "Location",
            title: receiptResources.Location,
            sortable: false,
            headerAttributes: { class: " k-rmain-header" },
            attributes: { class: "" },
            width: 140,
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.Location = options.model.Location || "";
                    var txtLocation = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtLocation" ' +
                    'type="text"  maxlength="6" formatTextbox = "alphaNumeric" class="txt-upper grid_inpt" data-descField="' +
                    options.model.Location + '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="Location"/></div>';
                    var finderLocation = '<div class="edit-cell inpt-finder"> <input title="' + receiptResources.Finder + '" type="button" class="icon btn-search" ' +
                    'id="btnLocationfield"/></div></div>';

                    receiptUI.itemfinderlineNumber = options.model.DisplayIndex;
                    receiptUI.cursorField = "Location";

                    var html = txtLocation + '' + finderLocation;
                    var locationFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.Location);
                    $(html).appendTo(container);
                    sg.finderHelper.setFinder("btnLocationfield", sg.finder.LocationQuantity, receiptUISuccess.locationGridFinderSuccess, receiptFilter.onLocationCancel,
                    locationFindertitle, receiptFilter.locationFilter, null, true);

                    $("#txtLocation").bind('change', function (e) {
                        var location = e.target.value;
                        $("#message").empty();
                        sg.delayOnChange("btnLocationfield", $("#txtLocation"), function () {
                            receiptUI.cursorField = null;
                            receiptUI.itemfinderlineNumber = null;
                            var grid = $('#ReceiptGrid').data("kendoGrid");
                            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                            receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                            currentRowGrid.set("Location", location);
                            receiptUI.cursorField = "Location";
                            receiptRepository.setItemValues(currentRowGrid, gridRowFields.Location);
                            return false;
                        });
                    });
                }
            }
        },

        {
            field: "QuantityReceived",
            title: receiptResources.QtyReceived,
            attributes: { "class": "  align-right" },
            headerAttributes: { "class": " align-right" },
            width: 140,
            template: function (dataItem) {
                var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.FracDecimals() : 0;
                var format = "n" + decimal;
                var decValue = kendo.toString(dataItem.QuantityReceived || 0, format);
                return decValue;
            },
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                } else {
                    options.model.QuantityReceived = options.model.QuantityReceived || "";
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" maxlength="17" class="align-right" data-bind="value:' + options.field + '"/>');
                    var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.FracDecimals() : 0;
                    var ctrlUnitCost = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + decimal,
                        spinners: false,
                        step: 0,
                        decimals: decimal
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlUnitCost, decimal, 12);

                    $("#QuantityReceived").bind('change', function (e) {
                        var quantityReceived = e.target.value;
                        $("#message").empty();
                        var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.ConvFactorDecimal() : 0;
                        var value = kendo.toString(quantityReceived || 0, "n" + decimal);
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        currentRowGrid.set("QuantityReceived", value);
                        if (receiptUI.receiptModel.Data.RequireLabels() === 1) {
                            currentRowGrid.set("Labels", this.value || 0);                            
                        }
                        receiptUI.cursorField = "QuantityReceived";
                        receiptRepository.setItemValues(currentRowGrid, gridRowFields.QuantityReceived);
                        return false;
                    });
                }
            }
        },

        {
            field: "UnitOfMeasure",
            title: receiptResources.UnitOfMeasure,
            headerAttributes: { class: " k-rmain-header" },
            attributes: { class: "" },
            width: 140,
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.UnitOfMeasure = options.model.UnitOfMeasure || "";
                    var txtUnitOfMeasure = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtUnitOfMeasure" type="text"  ' +
                    'maxlength="5" class="grid_inpt" data-descField="' + options.model.UnitOfMeasure +
                    '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="UnitOfMeasure"/></div>';
                    var finderUnitOfMeasure = '<div class="edit-cell inpt-finder"> <input title="' + receiptResources.Finder + '" type="button" class="icon btn-search" ' +
                    'id="btnUnitOfMeasurefield"/></div></div>';
                    receiptUI.itemfinderlineNumber = options.model.DisplayIndex;
                    receiptUI.cursorField = "UnitOfMeasure";
                    var html = txtUnitOfMeasure + '' + finderUnitOfMeasure;
                    var unitOfMeasureFindertitle = jQuery.validator.format(receiptResources.FinderTitle, receiptResources.UnitsOfMeasure);
                    $(html).appendTo(container);
                    sg.finderHelper.setFinder("btnUnitOfMeasurefield", sg.finder.ICItemUnitOfMeasure, receiptUISuccess.uomGridFinderSuccess, receiptFilter.onUOMCancel,
                    unitOfMeasureFindertitle, receiptFilter.uomFilter, null, false);

                    $("#txtUnitOfMeasure").bind('change', function (e) {
                        var unitOfMeasure = e.target.value;
                        $("#message").empty();
                        sg.delayOnChange("btnUnitOfMeasurefield", $("#txtUnitOfMeasure"), function () {
                            var grid = $('#ReceiptGrid').data("kendoGrid");
                            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                            receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                            currentRowGrid.set("UnitOfMeasure", unitOfMeasure);
                            receiptUI.cursorField = "UnitOfMeasure";
                            receiptRepository.setItemValues(currentRowGrid, gridRowFields.UnitOfMeasure);
                            return false;
                        });
                    });
                }
            }
        },
        {
            field: "UnitCost",
            title: receiptResources.UnitCost,
            attributes: {
                "class": " align-right"
            },
            headerAttributes: { "class": " align-right" },
            width: 140,
            template: function (dataItem) {
                var format = "n" + 6;
                var decValue = kendo.toString(dataItem.UnitCost ? dataItem.UnitCost : 0, format);
                return decValue;
            },
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                } else if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                    options.model.UnitCost = options.model.UnitCost || "";
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" maxlength="17" class="align-right" data-bind="value:' + options.field + '"/>');
                    var ctrlUnitCost = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + 6,
                        spinners: false,
                        step: 0,
                        decimals: 6
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlUnitCost, 6, 10);

                    $("#UnitCost").bind('change', function (e) {
                        var unitCost = e.target.value;
                        $("#message").empty();

                        if (unitCost) {
                            var value = kendo.toString(unitCost, "n" + 6);
                            var grid = $('#ReceiptGrid').data("kendoGrid");
                            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                            receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                            currentRowGrid.set("UnitCost", value);
                            receiptUI.cursorField = "UnitCost";
                            receiptRepository.setItemValues(currentRowGrid, gridRowFields.UnitCost);
                            return false;
                        }
                    });
                } else {
                    receiptGridUtility.setColumnNonEditable(container);
                }
            }
        },

        {
            field: "ExtendedCost",
            title: receiptResources.ExtendedCost,
            attributes: {
                "class": "align-right"
            },
            headerAttributes: { "class": "align-right" },
            width: 140,
            template: function (dataItem) {
                var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() || 0;
                var format = "n" + decimal;
                var decValue = kendo.toString(dataItem.ExtendedCost ? dataItem.ExtendedCost : 0, format);
                return decValue;
            },
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                } else {
                    options.model.ExtendedCost = options.model.ExtendedCost !== null ? options.model.ExtendedCost : "";
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" maxlength="16" class="align-right" data-bind="value:' + options.field + '"/>');
                    var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() || 0;
                    var ctrlUnitCost = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + decimal,
                        spinners: false,
                        step: 0,
                        decimals: decimal,
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlUnitCost, decimal, 13);

                    $("#ExtendedCost").bind('change', function (e) {
                        var extendedCost = e.target.value;
                        $("#message").empty();

                        if (extendedCost) {
                            var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() || 0;
                            var value = kendo.toString(extendedCost, "n" + decimal);
                            var grid = $('#ReceiptGrid').data("kendoGrid");
                            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                            receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                            currentRowGrid.set("ExtendedCost", value);
                            receiptUI.cursorField = "ExtendedCost";
                            receiptRepository.setItemValues(currentRowGrid, gridRowFields.ExtendedCost);
                            return false;
                        }
                    });
                }
            }
        },
        {
            field: "Labels",
            title: receiptResources.Labels,
            attributes: {
                "class": " align-right"
            },
            headerAttributes: { "class": " align-right" },
            width: 140,
            template: function (dataItem) {
                var value = kendo.toString(parseInt(dataItem.Labels) || 0, "n0");
                return value;
            },
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.Labels = options.model.Labels || "";
                    var html = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtLabels" type="text"  maxlength="5" formatTextbox = "numeric" class="grid_inpt align-right" data-descField="' + options.model.Labels + '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="Labels"/></div>';
                    $(html).appendTo(container);

                    $("#txtLabels").bind('change', function (e) {
                        e.preventDefault();
                        $("#message").empty();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        currentRowGrid.set("Labels", this.value);
                        receiptUI.cursorField = "Labels";
                        return false;
                    });
                }
            }
        },
        {
            field: "Comments",
            title: receiptResources.Comments,
            attributes: { "class": "" },
            headerAttributes: {
                "class": ""
            }, width: 200,
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() === type.COMPLETE || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.Comments = options.model.Comments || "";
                    var html = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtComments" type="text"  maxlength="250" class="grid_inpt" data-descField="' + options.model.Comments + '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="Comments"/></div>';
                    $(html).appendTo(container);

                    $("#txtComments").bind('change', function (e) {
                        e.preventDefault();
                        $("#message").empty();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        currentRowGrid.set("Comments", this.value);
                        receiptUI.cursorField = "Comments";
                        return false;
                    });
                }
            }
        },

        {
            field: "QuantityReturned",
            title: receiptResources.QtyReturned,
            attributes: {
                "class": "  align-right"
            },
            headerAttributes: {
                "class": " align-right"
            },
            width: 140,
            template: function (dataItem) {
                var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.FracDecimals() : 0;
                var format = "n" + decimal;
                var decValue = kendo.toString(dataItem.QuantityReturned || 0, format);
                return decValue;
            },
            editor: function (container, options) {

                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RETURN || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    receiptUI.enableReceiptType(false);
                    options.model.QuantityReturned = options.model.QuantityReturned || "";
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" class="align-right" data-bind="value:' + options.field + '"/>');
                    var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.FracDecimals() : 0;
                    var ctrlQtyReturn = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + decimal,
                        spinners: false,
                        step: 0,
                        decimals: decimal
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlQtyReturn, decimal, 12);
                    $("#QuantityReturned").bind('change', function (e) {
                        var qtyReturn = e.target.value;
                        $("#message").empty();
                        e.preventDefault();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        var decimal = receiptUI.receiptModel.IsFracQty() ? receiptUI.receiptModel.FracDecimals() : 0;
                        var value = kendo.toString(qtyReturn || 0, "n" + decimal);
                        currentRowGrid.set("QuantityReturned", value);
                        receiptUI.cursorField = "QuantityReturned";
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        return false;
                    });
                }
            }
        },

        {
            field: "ReturnCost",
            title: receiptResources.ReturnedCost,
            attributes: {
                "class": "align-right"
            },
            headerAttributes: { "class": "align-right" },
            width: 140,
            template: function (dataItem) {
                var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() || 0;
                var format = "n" + decimal;
                var decValue = kendo.toString(dataItem.ReturnCost || 0, format);
                return decValue;
            },
            editor: function (container) {
                receiptGridUtility.setColumnNonEditable(container);
            }
        },

        {
            field: "AdjustedUnitCost",
            title: receiptResources.AdjustedCost,
            attributes: {
                "class": " align-right"
            },
            headerAttributes: { "class": "align-right" },
            width: 140,
            template: function (dataItem) {
                var format = "n" + 6;
                var decValue = kendo.toString(dataItem.AdjustedUnitCost || 0, format);
                return decValue;
            },
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.ADJUSTMENT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else if (options.model.QuantityReceived === 0) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.AdjustedUnitCost = options.model.AdjustedUnitCost || "";
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" maxlength="17" class="align-right" data-bind="value:' + options.field + '"/>');
                    var decimal = 6;
                    var ctrlAdjUnitCost = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + decimal,
                        spinners: false,
                        step: 0,
                        decimals: decimal
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlAdjUnitCost, decimal, 10);

                    $("#AdjustedUnitCost").bind('change', function (e) {
                        var adjustedUnitCost = e.target.value;
                        receiptUI.enableReceiptType(false);
                        $("#message").empty();
                        e.preventDefault();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        var value = kendo.toString(adjustedUnitCost || 0, "n" + 6);
                        currentRowGrid.set("AdjustedUnitCost", value);
                        receiptUI.cursorField = "AdjustedUnitCost";
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        currentRowGrid.ReceiptType = receiptUI.receiptModel.Data.ReceiptType();
                        return false;
                    });
                }
            }
        },

        {
            field: "AdjustedCost",
            title: receiptResources.TotalAdjusted,
            attributes: {
                "class": "align-right"
            },
            template: function (dataItem) {
                var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() ? receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() : 0;
                var format = "n" + decimal;
                var decValue = kendo.toString(dataItem.AdjustedCost || 0, format);
                return decValue;
            },
            headerAttributes: { "class": "align-right" },
            width: 140,
            editor: function (container, options) {
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.ADJUSTMENT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else if (options.model.QuantityReceived === 0) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.AdjustedCost = options.model.AdjustedCost || "";
                    var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() ? receiptUI.receiptModel.Data.ReceiptCurrencyDecimals() : 0;
                    var html = $('<input name="' + options.field + '" id="' + options.field + '" maxlength="17" class="align-right" data-bind="value:' + options.field + '" />');
                    var ctrlAdjustedCost = $(html).appendTo(container).kendoNumericTextBox({
                        format: "n" + decimal,
                        spinners: false,
                        step: 0,
                        decimals: decimal,
                    }).data("kendoNumericTextBox");
                    sg.utls.kndoUI.restrictDecimals(ctrlAdjustedCost, decimal, 13);

                    $("#AdjustedCost").bind('change', function (e) {
                        var adjustedCost = e.target.value;

                        receiptUI.enableReceiptType(false);
                        e.preventDefault();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        var value = kendo.toString(adjustedCost || 0, "n" + decimal);
                        currentRowGrid.set("AdjustedCost", value);
                        receiptUI.cursorField = "AdjustedCost";
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        receiptRepository.setItemGridValuesModel(currentRowGrid, gridRowFields.AdjustedCost);
                        return false;
                    });
                }
            }
        },

        {
            field: "ManufacturersItemNumber",
            title: receiptResources.ManufacturersItemNumber,
            attributes: { "class": "txt-upper" },
            headerAttributes: {
                "class": ""
            },
            width: 140,
            editor: function (container, options) {

                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT || receiptUI.receiptModel.DisableScreen()) {
                    receiptGridUtility.setColumnNonEditable(container);
                }
                else {
                    options.model.Comments = options.model.Comments || "";
                    var html = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="txtManufacturersItemNumber" type="text"  ' +
                    'maxlength="250" class="txt-upper grid_inpt" data-descField="' + options.model.ManufacturersItemNumber + '" name="' + options.field +
                    '" data-bind="value:' + options.field + '" data-filter="ManufacturersItemNumber"/></div>';
                    $(html).appendTo(container);

                    $("#txtManufacturersItemNumber").bind('change', function (e) {
                        e.preventDefault();
                        var grid = $('#ReceiptGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                        receiptUI.itemfinderlineNumber = currentRowGrid.DisplayIndex;
                        receiptUI.cursorField = "ManufacturersItemNumber";
                        currentRowGrid.set("ManufacturersItemNumber", this.value);
                        receiptRepository.setItemGridValuesModel(currentRowGrid, gridRowFields.ManufacturersItemNumber);
                        return false;
                    });
                }
            }
        },
        {
            field: "OptionalFieldString",
            title: receiptResources.OptionalFields,
            sortable: false,
            headerAttributes: { "class": "" },
            attributes: { class: "" },
            width: 140,
            editor: function (container, options) {
                var html = receiptDetailFields.txtOptionalFieldString + receiptDetailFields.finderOptionalFieldString;
                receiptGridUtility.isCellEditable = true;
                $(html).appendTo(container);

                $("#btnDetailOptionalfield").on("click", function () {
                    receiptUI.isDetailOptionalField = true;
                    receiptUI.optionalFieldPopUpClose = false;
                    $("#message").hide();
                    var grid = $('#ReceiptGrid').data("kendoGrid");
                    var selectedData = sg.utls.kndoUI.getSelectedRowData(grid);
                    optionalFieldUIGrid.paramIndex = selectedData.DetailLineNumber;
                    if (selectedData ) {
                        receiptUI.lineNumber = options.model.LineNumber;
                    }
                    receiptRepository.setDetail(selectedData);
                    sg.utls.openKendoWindowPopup('#detailOptionalField', null);
                    optionalFieldUIGrid.isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();
                    $("#windowmessage").empty();
                    receiptUI.initDetailOptionalFields(false);
                    var optionalGrid = $('#DetailOptionalFieldGrid').data("kendoGrid");
                    optionalGrid.dataSource.page(1);
                });
            }
        },
        {
            field: "LineNumber",
            title: receiptResources.LineNumber,
            attributes: { "class": "", sg_Customizable: false },
            headerAttributes: { "class": "" },
            hidden: true,
            width: 140,
            editor: function (container) {
                sg.utls.kndoUI.nonEditable($('#ReceiptGrid').data("kendoGrid"), container);
            }
        },
        {
            field: "SequenceNumber",
            hidden: true,
            attributes: { sg_Customizable: false }
        },
        {
            field: "Category",
            attributes: { sg_Customizable: false },
            hidden: true
        }

        ],

        editable: {
            mode: "incell",
            confirmation: false,
            createAt: "bottom"
        },

        edit: function (e) {
            this.select(e.container.closest("tr"));
            receiptGrid.currentRow = e.sender.select().attr("data-uid");
        },

        dataChange: function (changedData) {
            if (changedData.columnName === "OptionalFieldString" && changedData.cellData === "") {
                receiptUI.isDetailChanged = true;
                receiptGridUtility.isDataRefreshInProgress = true;
                receiptRepository.refreshDetail(changedData.rowData);
            }
        },
        change: function (e) {
            if (receiptGrid.currentRow && receiptGrid.currentRow !== e.sender.select().attr("data-uid") && !receiptGrid.setLineEditable) {
                var grid = $('#ReceiptGrid').data("kendoGrid");
                var row = grid.tbody.find("tr[data-uid='" + receiptGrid.currentRow + "']");
                if (row) {
                    var selectedRowData = sg.utls.kndoUI.getDataItemForRow(row, $("#ReceiptGrid").data("kendoGrid"));
                    if (selectedRowData) {
                        if (selectedRowData.IsNewLine || selectedRowData.HasChanged) {
                            receiptGridUtility.selectedIndex = selectedRowData.DisplayIndex;
                            grid.dataSource.read();
                        }
                    }
                }
            }
            receiptGrid.currentRow = e.sender.select().attr("data-uid");
        }
    }
};

/*******************************************************************receipt Detail Grid End*********************************************************/

$(document).ready(function () {

    receiptUI.init();

    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && receiptUI.receiptModel.isModelDirty.isDirty() && !receiptUI.receiptModel.DisableScreen()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2,
            receiptResources.Receipts)).text();
        }
    });

    $(window).bind('unload', function () {
        if (globalResource.AllowPageUnloadEvent) {
            sg.utls.destroySession();
        }
    });
});

