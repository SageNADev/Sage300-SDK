// The MIT License (MIT) 
// Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved.
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

var currencySelected = {
    receiptCurrency: 1,
    additionalCostCurrency: 2
};

var dateChanged = {
    receiptDate: 1,
    postingDate: 2
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

var type = {
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
    previousAdditionalCost: "",
    previousPopupExchangeRate: "",
    previousExchangeRate: "",
    hasKoBindingApplied: false,
    ignoreIsDirtyProperties: ["ReceiptNumber", "isControlsDisabledOnReadMode", "UIMode", "IsOptionalFields", "IsRequireLabel", "ReceiptDetail", "ReceiptOptionalField", "ReceiptDetailOptionalField", "disableAdditionalCost", "disableExchangeRate"],
    computedProperties: ["UIMode", "ComputedYearPeriod", "isControlsDisabledOnReadMode", "ClearRecordStatusDescription"],
    fetchFirstRecord: false,
    rateTypeVendorDetail: null,
    eventTypeDateValidation: { ReceiptDate: 3, PostingDate: 49 },
    isFromReceiptFInder: false,
    btnAddLineID: "",
    IsValidate: "",
    gridLineId: "",
    duplicateCheckGrid: null,
    optionalFieldPopUpClose: false,
    lineNumber: 0,
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
    isItemSuccess: false,
    manufactureItemNumber: null,
    isVendorNumberCorrect: false,
    itemfinderlineNumber: null,
    isWrongRateType: false,
    RateTypeOldValue: null, isCurrencyChanged: false,
    receiptNumber: null,
    isWrongexchangeRate: false,
    exchangeRateOldValue: null,
    isVendorNumberChanged: false,
    vendorNumberOldValue: null,
    additionalCost: null,
    additionalCostCurrencyClicked: false,
    cursorField: null,
    oldAdditionalCostCurrency: null,
    createNewButtonClicked: false,
    cursorforFinder: null, oldUnitOfMeasure: null,
    manufactureItemNumner: null,
    refreshClicked: false, indexOfPage: 0,

    checkValidDateType: {
        ReceiptDate: 0,
        PostingDate: 1
    },
    dateChangeBy: null,
    isLineDeleted: false,
    isReceiptDateModified: false,
    isReceiptCurrency: false,
    rateTypeValue: null,
    previousReceiptDate: null,
    previousPostingDate: null,
    controlToBeFocused: "#txtReceiptNumber",
    isRefreshTotalCost: false,
    exitExchangeRateChange: false,

    //For using new grid and optional grid
    columnIsEditable: function (colName) {
        var model = receiptUI.receiptModel;
        var rptType = model.Data.ReceiptType();
        var enableScreen = !model.DisableScreen();
        var columns = ["ITEMNO", "LOCATION", "RECPQTY", "RECPUNIT", "UNITCOST", "RECPCOST", "LABELS", "MANITEMNO"];

        if (columns.indexOf(colName) > -1) {
            return rptType === type.RECEIPT && enableScreen;
        }
        if (colName === "COMMENTS") {
            return rptType !== type.COMPLETE && enableScreen;
        }
        if (colName === "RETURNCOST") {
            return false;
        }
        if (colName === "RETURNQTY") {
            return rptType === type.RETURN && enableScreen;
        }
        if (colName === "ADJUNITCST" || colName === "ADJCOST") {
            return rptType === type.ADJUSTMENT && enableScreen;
        }
        return true;
    },

    updateGridModel: function() {
        var vm = receiptViewModel;
        var gridColDefs = receiptGridModel.ColumnDefinitions;

        var fields = ["RECPQTY", "RETURNQTY", "UNITCOST", "ADJUNITCST", "RECPCOST", "RETURNCOST", "ADJCOST" ];
        fields.forEach(function (f) {
            if (f.indexOf("QTY") > -1) {
                var pricison = vm.IsFracQty ? vm.FracDecimals : 0;
            } else if (f.indexOf("UNITC") > -1) {
                pricison = 6;
            } else {
                pricison = vm.ReceiptCurrencyDecimals || vm.FuncDecimals || 3;
            } 
            var col = gridColDefs.filter(function (c) { return c.FieldName === f; })[0];
            col.Precision = pricison;
        });
    },

    updateGridColDecimal: function () {
        var decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals();
        var colOptions = [{ "field": "RECPCOST", "precision": decimal }, { "field": "RETURNCOST", "precision": decimal }, { "field": "ADJCOST", "precision": decimal }];
        sg.viewList.setColumnsOptions("receiptGrid", colOptions);
    },

    showDetailOptionalField: function () {
        var grid = $('#receiptGrid').data("kendoGrid"),
            selectedRow = sg.utls.kndoUI.getSelectedRowData(grid),
            filter = kendo.format("SEQUENCENO={0} AND LINENO={1}", selectedRow.SEQUENCENO, selectedRow.LINENO),
            isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();

        sg.optionalFieldControl.showPopUp("rptDetailOptionalFieldGrid", "detailOptionalField", isReadOnly, filter, "receiptGrid");
    },

    showOptionalField: function () {
        var isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();
        sg.optionalFieldControl.showPopUp("rptOptionalFieldGrid", "optionalField", isReadOnly);
    },

    //Init all pop ups screens
    initPopUps: function () {
        sg.utls.intializeKendoWindowPopup('#optionalField', receiptResources.OptionalFields, function () {
            var count = $("#rptOptionalFieldGrid").data("kendoGrid").dataSource.total();
            receiptUI.receiptModel.Data.OptionalFields(count);
        });
        sg.utls.intializeKendoWindowPopup('#detailOptionalField', receiptResources.OptionalFields, function () {
            sg.optionalFieldControl.closePopUp('rptDetailOptionalFieldGrid', 'receiptGrid');
        });
        sg.utls.intializeKendoWindowPopup('#exchangeRate', receiptResources.RateSelection);
    },

    //ToDO: Custom call back functions
    customGridChanged: function (value) {
    },
    customGridAfterSetActiveRecord: function (value) {
    },
    customGridBeforeDelete: function (value, event) {
    },
    customGridAfterDelete: function (value) {
    },
    customGridBeforeCreate: function (event) {
    },
    customGridAfterCreate: function (value) {
    },
    customGridAfterInserte: function (value) {
    },
    customColumnChanged: function (currentValue, value, event) {
    },
    customColumnBeforeDisplay: function (value, properties) {
    },
    customColumnDoubleClick: function (value, event) {
    },
    customColumnBeforeFinder: function (value, options) {
    },
    customColumnBeforeEdit: function (value, event) {
    },
    customColumnStartEdit: function (value, editor) {
    },
    customColumnEndEdit: function (value, editor) {
    },

    //All init Methods here
    init: function () {
        receiptUI.initButtons();
        receiptUI.initDropDownList();
        receiptUI.receiptModel = receiptViewModel;
        receiptUI.initFinders();
        receiptUI.initPopUps();
        receiptUI.initHamburgers();
        receiptUISuccess.initialLoad(receiptViewModel);
        if (!receiptUI.hasKoBindingApplied) {
            receiptObservableExtension(receiptUI.receiptModel, sg.utls.OperationMode.NEW);
        }
        receiptUISuccess.setkey();
        receiptUI.cursorField === null;
        receiptUI.RateTypeOldValue = null;

        //initialize the new grid
        sg.viewList.init("receiptGrid", false, receiptUI.updateGridModel);
        sg.optionalFieldControl.init("rptOptionalFieldGrid", { "viewId": "IC0377", "filter": "LOCATION=2", "allowDelete": true, "allowInsert": true, "type": 0 }, false);
        sg.optionalFieldControl.init("rptDetailOptionalFieldGrid", { "viewId": "IC0377", "filter": "LOCATION=3", "allowDelete": true, "allowInsert": true, "type": 0 }, false);

        if (!receiptViewModel.DisableScreen && receiptViewModel.IsExists) {
            receiptUISuccess.displayResult(receiptViewModel, sg.utls.OperationMode.SAVE);
        }
    },

    //Init buttons
    initButtons: function () {
        sg.exportHelper.setExportEvent("btnOptionExport", sg.dataMigration.Receipt, false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", sg.dataMigration.Receipt, false, $.noop);

        $("#btnRefresh").on("click", function (e) {
            var grid = $('#receiptGrid').data("kendoGrid");
            if (!$(this).is(':disabled') && grid.dataSource.data().length > 0) {
                receiptUI.refreshClicked = true;
                receiptUI.addLineClicked = false;
                receiptRepository.saveReceiptDetails(receiptUI.receiptModel.Data);
            }
        });

        $("#txtReceiptNumber").on('change', function (e) {
            receiptUI.receiptModel.Data.ReceiptNumber($("#txtReceiptNumber").val());
            if (sg.controls.GetString(receiptUI.receiptModel.Data.ReceiptNumber() !== "")) {
                if (sg.controls.GetString(receiptUI.receiptNumber) !== sg.controls.GetString(receiptUI.receiptModel.Data.ReceiptNumber())) {
                    receiptUI.checkIsDirty(receiptUI.get);
                }
            }
        });

        $("#txtReceiptDate").on('change', function () {
            receiptUI.previousReceiptDate = null;
            var receiptDate = $("#txtReceiptDate").val();
            receiptUI.previousReceiptDate = receiptUI.receiptModel.Data.ReceiptDate();
            var validDate = sg.utls.kndoUI.checkForValidDate(receiptDate);
            if (validDate) {
                receiptUI.dateChangeBy = dateChanged.receiptDate;
                receiptDate = sg.utls.kndoUI.convertStringToDate(validDate);
                if (receiptDate) {
                    receiptUI.controlToBeFocused = "#txtReceiptDate";
                    receiptRepository.checkDate(receiptDate, receiptUI.eventTypeDateValidation.ReceiptDate);
                }
                receiptUI.enableReceiptType(false);
            } else {
                receiptUI.receiptModel.Data.ReceiptDate(receiptUI.previousReceiptDate);
            }
        });

        $("#txtDescription").on('change', function () {
            receiptUI.enableReceiptType(false);
        });

        $("#txtPostingDate").on('change', function () {
            var postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
            receiptUI.previousPostingDate = receiptUI.receiptModel.Data.PostingDate();
            var validDate = sg.utls.kndoUI.checkForValidDate(postingDate);
            if (validDate) {
                receiptUI.dateChangeBy = dateChanged.postingDate;
                postingDate = sg.utls.kndoUI.convertStringToDate(validDate);
                receiptUI.postingDate = receiptUI.receiptModel.Data.PostingDate();
                if (postingDate) {
                    receiptUI.controlToBeFocused = "#txtPostingDate";
                    receiptRepository.checkDate(postingDate, receiptUI.eventTypeDateValidation.PostingDate);
                }
                receiptUI.enableReceiptType(false);
            } else {
                receiptUI.receiptModel.Data.PostingDate(receiptUI.previousPostingDate);
            }
        });

        $("#btnNewReceipt").bind('click', function () {
            receiptUI.createNewButtonClicked = true;
            receiptUI.checkIsDirty(receiptUI.create);
            receiptUI.isWrongexchangeRate = false;
            receiptUI.prevExRate = "";
            receiptUI.exchangeRateOldValue = "";
            receiptUI.isVendorNumberCorrect = false;
            receiptUI.createNewButtonClicked = false;
        });

        //Save receipt
        $("#btnSave").bind('click', function () {
            if (sg.viewList.commit()) {
                sg.utls.SyncExecute(receiptUI.save);
                sg.viewList.dirty("receiptGrid", false);
            }
        });

        //Receipt Post Functionality
        $("#btnPost").bind('click', function () {
            if (sg.viewList.commit() && $("#frmReceipt").valid()) {
                receiptPost();
                sg.viewList.dirty("receiptGrid", false);
            }
        });
        /**
         Receipt post function
        */
        function receiptPost() {
            if (receiptUI.receiptModel.IsPromptToDelete()) {
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
            } else {
                receiptRepository.post(receiptUI.receiptModel.Data, receiptUI.receiptModel.Data.SequenceNumber(), false);
            }
        }
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
            receiptUI.showOptionalField();
        });

        $("#Data_VendorNumber").bind('change', function () {
            $("#message").empty();
            sg.delayOnChange("btnVendorNumberFinder", $("Data_VendorNumber"), function () {
                receiptUI.isVendorNumberChanged = true;
                receiptUI.RefreshHeader();
            });
        });

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

        $("#Data_AdditionalCostCurrency").change(function () {
            $("#message").empty();
            sg.delayOnBlur("btnAddlCostCurrencyFinder", function () {
                receiptUI.currencySelected = 2;
                receiptUI.additionalCostCurrencyClicked = true;
                receiptUI.RefreshHeader();
            });
        });

        $("#Data_RateType").bind('change', function () {
            $("#message").empty();
            receiptUI.isWrongRateType = true;
            var rateType = $("#Data_RateType").val();
            sg.delayOnChange("btnRateTypeFinder", $("Data_RateType"), function () {
                var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                if (rateType) {
                    receiptUI.receiptModel.Data.ExchangeRate(1);
                    receiptRepository.refresh(receiptUI.receiptModel.Data);
                } else {
                    receiptUI.receiptModel.Data.RateType(rateType);
                    receiptRepository.GetHeaderValues(data, headerFields.RateType);
                }
            });
        });

        $("#Data_ExchangeRate").on('change', function () {
            receiptUI.isWrongRateType = false;
            $("#message").empty();
            var rate = kendo.parseFloat($("#Data_ExchangeRate").val());
            receiptUI.isWrongexchangeRate = true;
            var data = receiptUI.receiptModel.Data;
            if (rate && receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
                receiptUI.exchangeRateOldValue = data.ExchangeRate();
                receiptRepository.checkRateSpread(data.RateType(), data.ReceiptCurrency(), data.RateDate(), rate, data.HomeCurrency());
            } else {
                receiptUI.receiptModel.Data.ExchangeRate(rate);
                receiptUI.RefreshHeader();
            }
        });

        $("#Data_ExchangeRate, #txtpopupExchangeRate").on('focus', function (e) {
            e.preventDefault();
            var culture = kendo.culture();
            var symbol = culture.numberFormat['.'];
            if (symbol !== ".") {
                this.value = this.value.replace(".", symbol);
            }
            receiptUI.exitExchangeRateChange = false;
        });

        //On Change for Exchange Rate
        $("#txtpopupExchangeRate").on('change', function () {
            if (receiptUI.exitExchangeRateChange) {
                var value = receiptUI.receiptModel.Data.ExchangeRate().toString();
                value = value.replace(".", kendo.culture().numberFormat['.']);
                $("#txtpopupExchangeRate").val(value);
                return;
            }
            $("#message").empty();
            receiptUI.isWrongRateType = false;

            var rate = kendo.parseFloat($("#txtpopupExchangeRate").val());
            receiptUI.isWrongexchangeRate = true;

            if (!rate) {
                receiptUI.receiptModel.Data.ExchangeRate(0);
                $("#txtpopupExchangeRate").val(0);
            }
            var data = receiptUI.receiptModel.Data;
            if (rate && data.ReceiptType() === type.RECEIPT) {
                receiptUI.exchangeRateOldValue = data.ExchangeRate();
                receiptRepository.checkRateSpread(data.RateType(), data.ReceiptCurrency(), data.RateDate(), rate, data.HomeCurrency());
            } else {
                receiptUI.receiptModel.Data.ExchangeRate(rate);
                receiptUI.RefreshHeader();
            }
            receiptUI.exitExchangeRateChange = !receiptUI.exitExchangeRateChange;
        });
    },

    // Initialize the kendo numeric controls
    initNumericTextBox: function () {
        var model = receiptUI.receiptModel;
        var data = model.Data;
        var decimal = data.AdditionalCostCurrency() === data.ReceiptCurrency() ? data.ReceiptCurrencyDecimals() : model.FuncDecimals();

        var ctrlAddlCost = $("#txtAddlCost").kendoNumericTextBox({
            format: "n" + decimal,
            spinners: false,
            step: 0,
            min: 0,
            decimals: 13,
            change: function (e) {
                receiptUI.previousAdditionalCost = receiptUI.receiptModel.Data.AdditionalCost();
                receiptUI.enableReceiptType(false);
                receiptUI.isWrongRateType = false;
                var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.GetHeaderValues(data, headerFields.AdditionalCost);
            }
        }).data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(ctrlAddlCost, decimal, 13);

        var exchangeRate = $("#Data_ExchangeRate").kendoNumericTextBox({
            format: "n7",
            spinners: false,
            step: 0,
            decimals: 7,
            min: 0.0000000,
            value: parseFloat(receiptUI.receiptModel.Data.ExchangeRate())
        }).data("kendoNumericTextBox");
        $(exchangeRate.element).unbind("input");
        sg.utls.kndoUI.restrictDecimals(exchangeRate, 7, 8);

        var txtpopupExchangeRate = $("#txtpopupExchangeRate").kendoNumericTextBox({
            format: "n7",
            spinners: false,
            step: 0,
            decimals: 7,
            min: 0.0000000,
            value: parseFloat(receiptUI.receiptModel.Data.ExchangeRate())
        }).data("kendoNumericTextBox");
        $(txtpopupExchangeRate.element).unbind("input");
        sg.utls.kndoUI.restrictDecimals(txtpopupExchangeRate, 7, 8);

        $("#txtTotalCost, #txtTotalReturnCost, #txtTotalAdjustmentCost").kendoNumericTextBox({
            format: "n" + decimal,
            spinners: false,
            step: 0,
            min: 0,
            decimals: 13
        });
    },

    initDropDownList: function () {
        var fields = ["Data_ReceiptType", "Data_AdditionalCostAllocationType"];
        $.each(fields, function (index, field) {
            sg.utls.kndoUI.dropDownList(field);
        });

        $("#Data_ReceiptType").data("kendoDropDownList").bind("change", receiptUI.typeSelectionChanged);
        $("#Data_AdditionalCostAllocationType").data("kendoDropDownList").bind("change", receiptUI.costSelectionChanged);
    },

    updateFiscalYearPeriod: function (result) {
        var model = receiptUI.receiptModel.Data;
        if (result.UserMessage.IsSuccess) {
            var fiscalYear = result.Data.FiscalYear;
            var fiscalPeriod = result.Data.FiscalPeriod;
            model.FiscalPeriod(parseInt(fiscalPeriod));
            model.FiscalYear(fiscalYear);
        } else {
            model.FiscalPeriod("");
            model.FiscalYear("");
            sg.utls.showMessage(result);
        }
    },

    create: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptRepository.create(receiptUI.receiptModel.Data.ReceiptNumber());
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    save: function () {
        if ($("#frmReceipt").valid()) {
            receiptUI.receiptSave();
        }
    },

    receiptSave: function () {
        var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        data.RecordStatus = 1;
        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.SAVE) {
            receiptRepository.update(data);
        } else {
            receiptRepository.add(data);
        }
    },

    checkIsDirty: function (funcionToCall) {
        var modelData = receiptUI.receiptModel.Data;
        var gridDirty = sg.viewList.dirty("receiptGrid");
        var modelDirty = receiptUI.receiptModel.isModelDirty.isDirty();
        var exists = true;

        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.NEW && !receiptUI.isFromReceiptFInder) {
            if (modelDirty || gridDirty && receiptUI.receiptNumber) {
                var data = ko.mapping.toJS(modelData, receiptUI.ignoreIsDirtyProperties);
                exists = receiptRepository.isExists(modelData.ReceiptNumber(), data);
            }
        }

        receiptUI.isFromReceiptFInder = false;
        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.NEW && !exists && !receiptUI.createNewButtonClicked)
            return;

        if ((modelDirty || gridDirty && receiptUI.receiptNumber)
            && (!(receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.NEW) || exists)) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmReceipt");
                    funcionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(receiptUI.receiptNumber) !== sg.controls.GetString(modelData.ReceiptNumber())) {
                        modelData.ReceiptNumber(receiptUI.receiptNumber);
                    }
                    return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, receiptResources.Receipt, receiptUI.receiptNumber));
        }
        else {
            funcionToCall.call();
        }
    },

    openExchangeRate: function () {
        receiptUI.isWrongexchangeRate = true;
        receiptUI.RateTypeOldValue = receiptUI.receiptModel.Data.RateType();
        receiptUI.exchangeRateOldValue = receiptUI.receiptModel.Data.ExchangeRate();
        sg.utls.openKendoWindowPopup('#exchangeRate', null);
        $("#windowmessage").empty();
        $("#exchangeRate").data('kendoWindow').unbind('activate').bind('activate', function (e) {
            sg.controls.Focus($("#Data_RateType"));
        });
    },

    initGridLocationFinder: function (viewFinder) {
        //add custom defined properties here, grid generic editor will call it
        viewFinder.calculatePageCount = false;
        return viewFinder;
    },

    initReceiptNumberFinder: function (viewFinder) {
        viewFinder.viewID = "IC0590";
        viewFinder.viewOrder = 2;
        viewFinder.displayFieldNames = ["RECPNUMBER", "RECPDESC", "RECPDATE", "FISCYEAR", "FISCPERIOD", "PONUM", "REFERENCE", "RECPTYPE", "RATEOP", "VENDNUMBER", "RECPCUR", "RECPRATE", "RATETYPE", "RATEDATE", "RATEOVRRD", "ADDCOST", "ADDCOSTHM", "ADDCOSTSRC", "ADDCUR", "TOTCSTHM", "TOTCSTSRC", "NUMCSTDETL", "LABELS", "ADDCSTTYPE", "ORIGTOTSRC", "ORIGTOTHM", "ADDCSTHOME", "TOTALCOST", "RECPDECIML", "VENDNAME", "VENDEXISTS", "STATUS"];
        viewFinder.returnFieldNames = ["RECPNUMBER"];
        viewFinder.parentValAsInitKey = $("#txtReceiptNumber").val() === "*** NEW ***" ? false : true;
        viewFinder.filter = "DELETED = 0";
        //viewFinder.optionalFieldBindings = "IC0595, IC0377[2]";  // comment out for now as CSFND doesn't support filterCount yet
    },

    initVendorNumberFinder: function (viewFinder) {
        viewFinder.viewID = "AP0015";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["VENDORID", "VENDNAME", "SWACTV", "IDGRP", "CURNCODE", "SHORTNAME", "SWHOLD", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2"];
        viewFinder.returnFieldNames = ["VENDORID"];
        viewFinder.parentValAsInitKey = true;
        //viewFinder.optionalFieldBindings = "AP0407,AP0500[0]";  // comment out for now as CSFND doesn't support filterCount yet
    },

    initCurrencyCodeFinderCommon: function (viewFinder) {
        viewFinder.viewID = "CS0003";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["CURID", "CURNAME", "SYMBOL", "DECIMALS", "SYMBOLPOS", "THOUSSEP", "DECSEP", "NEGDISP"];
        viewFinder.returnFieldNames = ["CURID"];
        viewFinder.parentValAsInitKey = true;
    },

    initReceiptCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    initAddlCostCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    initRateTypeFinder: function (viewFinder) {
        viewFinder.viewID = "CS0004";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["RATETYPE", "RATEDESC"];
        viewFinder.returnFieldNames = ["RATETYPE"];
        viewFinder.parentValAsInitKey = true;
    },
    initExchangeRateFinder: function (viewFinder) {
        viewFinder.viewID = "CS0006";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["RATEDATE", "RATE", "SPREAD"];
        viewFinder.returnFieldNames = ["RATE"];
        viewFinder.parentValAsInitKey = true;
        var rateDate = receiptUI.receiptModel.Data.RateDate();
        var sgRateDate = rateDate.toISOString().split('T')[0].replace(/-/g, '');
        var fromCurrency = receiptUI.receiptModel.Data.ReceiptCurrency();
        var toCurrency = sg.utls.homeCurrency.Code;
        var rateType = receiptUI.receiptModel.Data.RateType();
        viewFinder.filter = kendo.format('RATETYPE={0} AND SOURCECUR={1} AND HOMECUR={2} AND RATEDATE={3}', rateType, fromCurrency, toCurrency, sgRateDate);
        viewFinder.calculatePageCount = false;
    },

    //Init all finders
    initFinders: function () {
        sg.viewFinderHelper.setViewFinder("btnReceiptNumberFinder", "txtReceiptNumber", this.initReceiptNumberFinder);
        sg.viewFinderHelper.setViewFinder("btnVendorNumberFinder", "Data_VendorNumber", this.initVendorNumberFinder);
        sg.viewFinderHelper.setViewFinder("btnReceiptCurrencyFinder", "Data_ReceiptCurrency", this.initReceiptCurrencyFinder);
        sg.viewFinderHelper.setViewFinder("btnAddlCostCurrencyFinder", "Data_AdditionalCostCurrency", this.initAddlCostCurrencyFinder);
        sg.viewFinderHelper.setViewFinder("btnRateTypeFinder", "Data_RateType", this.initRateTypeFinder);
        sg.viewFinderHelper.setViewFinder("btnExchangeRateFinder", "txtpopupExchangeRate", this.initExchangeRateFinder);
    },
    
    //Init Hamburgers
    initHamburgers: function () {
        var listExchangeRate = [
            sg.utls.labelMenuParams("lnkOpenExchangeRate", receiptResources.EditLink, receiptUI.openExchangeRate, "sagedisable:receiptUI.receiptModel.IsFuncCurrencyDisable")];
        var listOptionalField = [
            sg.utls.labelMenuParams("lnkOpenOptionalFields", receiptResources.AddOrEditLink, receiptUI.showOptionalField, null)];

        LabelMenuHelper.initialize(listExchangeRate, "lnkExchangeRateThree", "receiptUI.receiptModel");
        LabelMenuHelper.initialize(listOptionalField, "lnkOptionalField", "receiptUI.receiptModel");
    },

    typeSelectionChanged: function (e) {
        var selectedvalue = $('#Data_ReceiptType').data("kendoDropDownList").dataItem().value;
        if (selectedvalue ) {
            receiptUI.showHideColumns(selectedvalue);
            receiptUI.receiptModel.Data.ReceiptType(selectedvalue);
            receiptRepository.refresh(receiptUI.receiptModel.Data);
        }
    },

    showColumn: function (field) {
        var grid = $('#receiptGrid').data("kendoGrid");
        var colIndex = GridPreferencesHelper.getColumnIndex('#receiptGrid', field);
        grid.showColumn(colIndex);
        grid.columns[colIndex].attributes['sg_Customizable'] = true;
    },

    hideColumn: function (field) {
        var grid = $('#receiptGrid').data("kendoGrid");
        var colIndex = GridPreferencesHelper.getColumnIndex('#receiptGrid', field);
        grid.hideColumn(colIndex);
        grid.columns[colIndex].attributes['sg_Customizable'] = false;
    },    

    showHideColumns: function (value) {
        var grid = $('#receiptGrid').data("kendoGrid");
        if (!grid) { return; }

        sg.viewList.allowInsert("receiptGrid", value === type.RECEIPT);
        sg.viewList.allowDelete("receiptGrid", value === type.RECEIPT);

        if (receiptUI.receiptModel) {
            sg.viewList.hideColumns("receiptGrid", ["ADJCOST", "ADJUNITCST", "RETURNQTY", "RETURNCOST"]);
            if (value == type.RETURN) {
                sg.viewList.showColumns("receiptGrid", ["RETURNQTY", "RETURNCOST"]);
            } else if (value == type.ADJUSTMENT) {
                sg.viewList.showColumns("receiptGrid", ["ADJCOST", "ADJUNITCST"]);
            }
        }
    },

    costSelectionChanged: function (e) {
        var selectedvalue = $('#Data_AdditionalCostAllocationType').data("kendoDropDownList").dataItem().value;
        if (selectedvalue) {
            receiptUI.receiptModel.Data.AdditionalCostAllocationType(selectedvalue);
            receiptUI.enableReceiptType(false);
        }
    },

    //Get receipt number
    get: function () {
        var receiptNumber = receiptUI.receiptModel.Data.ReceiptNumber();
        receiptUI.receiptModel.UIMode(sg.utls.OperationMode.LOAD);
        receiptRepository.get(receiptNumber, receiptUI.receiptModel.DisableScreen());
    },

    populateDropDownList: function () {
        $("#Data_ReceiptType").data("kendoDropDownList").value(receiptUI.receiptModel.Data.ReceiptType());
        $("#Data_AdditionalCostAllocationType").data("kendoDropDownList").value(receiptUI.receiptModel.Data.AdditionalCostAllocationType());
    },

    enableReceiptType: function (isEnable) {
        var ctrl = $("#Data_ReceiptType").data("kendoDropDownList");
        if (ctrl) {
            ctrl.wrapper.show();
            ctrl.enable(isEnable);
        }
    },

    setExchangeRate: function (jsonResult) {
        var rate = jsonResult.Data.Rate;
        receiptUI.prevExRate = rate;
        receiptUI.receiptModel.Data.ExchangeRate(rate);
        $("#Data_ExchangeRate").data("kendoNumericTextBox").value(rate);
        $("#txtpopupExchangeRate").data("kendoNumericTextBox").value(rate);
        receiptUI.exchangeRateOldValue = rate;
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
        var modelData = receiptUI.receiptModel.Data;
        modelData.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
        modelData.TotalCostReceiptAdditional(result.TotalCostReceiptAdditional);
        modelData.TotalReturnCost(result.TotalReturnCost);
        modelData.TotalAdjCostReceiptAddl(result.TotalAdjCostReceiptAddl);
        receiptGridUtility.updateNumericTextBox("txtTotalCost", modelData.TotalCostReceiptAdditional());
        receiptGridUtility.updateNumericTextBox("txtTotalReturnCost", modelData.TotalReturnCost());
        receiptGridUtility.updateNumericTextBox("txtTotalAdjustmentCost", modelData.TotalAdjCostReceiptAddl());
        var grid = $('#receiptGrid').data("kendoGrid");
        grid.dataSource.read();
    },

    refreshReceiptDetail: function (result) {
        receiptGridUtility.isDataRefreshInProgress = true;
        if (!result.UserMessage.Errors) {
            ko.mapping.fromJS(result.Data.ReceiptDetail, {}, receiptUI.receiptModel.Data.ReceiptDetail);
        }
        sg.utls.showMessage(result);
        receiptGridUtility.isDataRefreshInProgress = false;
        var grid = $('#receiptGrid').data("kendoGrid");
        grid.dataSource.read();
    },

    DeleteDetailSucces: function (result) {
        var grid = $("#receiptGrid").data("kendoGrid");
        grid.dataSource.read();
    },

    getVendorDetailsSuccess: function (result) {
        if (result.Data) {
            if (result.Data.RateType) {
                receiptUI.receiptModel.Data.RateType(result.Data.RateType);
                $("#Data_RateType").val(result.Data.RateType);
                receiptUI.rateTypeVendorDetail = result.Data.RateType;
            }
        }
    },

    rateTypeSelect: function (result) {
        if (result === null) return;
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
                    var modelData = receiptUI.receiptModel.Data;
                    receiptUI.prevExRate = modelData.ExchangeRate();
                    receiptUI.exchangeRateOldValue = receiptUI.prevExRate;
                    if (sg.viewList.dirty("receiptGrid")) {
                        var data = ko.mapping.toJS(modelData, receiptUI.ignoreIsDirtyProperties);
                        receiptRepository.refresh(data);
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
            receiptUI.prevExRate = receiptUI.receiptModel.Data.ExchangeRate();
            receiptUI.exchangeRateOldValue = receiptUI.prevExRate;
            if (sg.viewList.dirty("receiptGrid")) {
                var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.refresh(data);
            }
        }
    },

    getItemTypeSuccess: function (jsonResult) {
        sg.ic.utls.setItemTypeResponse(jsonResult, "#btnManufacturerItemFinder", receiptUISuccess.manufacturerItem);
    },

    getResult: function (jsonResult) {
        var modelData = receiptUI.receiptModel.Data;
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            if (jsonResult.IsExists === true) {
                receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
                if (modelData.ReceiptType() === type.RECEIPT || modelData.ReceiptType() === type.ADJUSTMENT) {
                    sg.controls.Focus($("#txtDescription"));
                } else {
                    sg.controls.KendoDropDownFocus($("#Data_ReceiptType"));
                }
            } else {
                receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            }
            receiptUISuccess.setkey();
        } else {
            modelData.ReceiptNumber(receiptUI.receiptNumber);
            if (jsonResult) {
                modelData.TotalCostReceiptAdditionalDecimal(jsonResult.TotalCostReceiptAdditionalDecimal);
                modelData.TotalReturnCostDecimal(jsonResult.TotalReturnCostDecimal);
            }
        }
        sg.utls.showMessage(jsonResult);
        sg.viewList.dirty("receiptGrid", false);
        receiptGrid.setFirstLineEditable = false;
        if (modelData.ReceiptType() === type.RETURN) {
            sg.controls.disable($("#btnDetailAddLine"));
        }
    },

    actionSuccess: function (action, jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            if (action === "add" || action === "update" || action === "post" || action === "create") {
                receiptUI.addLineClicked = false;
            }
            receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        if (jsonResult.UserMessage.Warnings && jsonResult.UserMessage.Warnings.length > 0 && action === "add") {
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, jsonResult.UserMessage.Warnings[0].Message);
        }
        sg.utls.showMessage(jsonResult);
        if (action === "add" || action === "update") {
            receiptGrid.setFirstLineEditable = false;
        }
        if (action === "create") {
            sg.controls.Select($("#txtReceiptNumber"));
        }
    },

    GetHeaderValues: function (result) {
        if (result && result.UserMessage) {
            receiptUI.isVendorNumberCorrect = false;
            if (receiptUI.isWrongexchangeRate) {
                receiptUI.isWrongexchangeRate = false;
                if (receiptUI.exchangeRateOldValue) {
                    receiptUI.receiptModel.Data.ExchangeRate(receiptUI.exchangeRateOldValue);
                    receiptUI.prevExRate = receiptUI.exchangeRateOldValue;
                }
                if (receiptUI.exchangeRateOldValue === null && receiptUI.prevExRate) {
                    receiptUI.receiptModel.Data.ExchangeRate(receiptUI.prevExRate);
                }
                receiptUI.exchangeRateOldValue = null;
            }

            if (receiptUI.isWrongRateType) {
                receiptUI.isWrongRateType = false;
                if (receiptUI.RateTypeOldValue) {
                    receiptUI.receiptModel.Data.RateType(receiptUI.RateTypeOldValue);
                }
            }
            if (receiptUI.isVendorNumberChanged) {
                receiptUI.isWrongRateType = false;
                receiptUI.receiptModel.Data.VendorNumber("");
                sg.controls.Focus($("#Data_VendorNumber"));
            }
            sg.utls.showMessage(result);
            receiptUI.isReceiptCurrency = false;
        } else {
            receiptUI.isReceiptCurrency = true;
            receiptUI.isRefreshTotalCost = false;
            receiptUI.isVendorNumberChanged = false;
            if (result) {
                var fields = ["TotalCostReceiptAdditional", "TotalReturnCost", "TotalAdjCostReceiptAddl", "TotalExtendedCostSource", "TotalExtendedCostAdjusted", "ReceiptCurrencyDecimals", "TotalCostReceiptAdditionalDecimal", "TotalReturnCostDecimal"];
                for (var i = 0, length = fields.length; i < length; i++) {
                    f = fields[i];
                    var value = result[f];
                    if (value) {
                        receiptUI.receiptModel.Data[f](value);
                    }
                }
            }
            if (receiptUI.isWrongRateType) {
                if (result.ExchangeRate) {
                    receiptUI.receiptModel.Data.ExchangeRate(result.ExchangeRate);
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

    GetRateType: function (result) {
        if (result && result.UserMessage) {
            sg.utls.showMessage(result);
        } else {
            if (result.RateType) {
                receiptUI.receiptModel.Data.RateType(result.RateType);
                $("#Data_RateType").val(result.RateType);
                receiptUI.rateTypeValue = result.RateType;
            }
        }
        sg.utls.showMessage(result);
    },

    checkDate: function (jsonResult) {
        var receiptDate = sg.utls.kndoUI.getFormattedDate($("#txtReceiptDate").val());
        var postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
        if (jsonResult.UserMessage.Message && jsonResult.UserMessage.IsSuccess) {
            sg.utls.showKendoConfirmationDialog(
                //click on Yes
                function () {
                    var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                    receiptRepository.refresh(receiptUI.receiptModel.Data);
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        receiptUI.isReceiptDateModified = true;
                        receiptUI.receiptModel.Data.ReceiptDate(receiptDate);
                        receiptUI.receiptModel.Data.RateDate(receiptDate);
                        receiptUI.setExchangeRateValue();
                        if (receiptUI.receiptModel.DefaultPostingDate() === 1) {
                            receiptUI.receiptModel.Data.PostingDate(receiptDate);
                        }
                        if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT) {
                            receiptUI.enableReceiptType(false);
                        }
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        receiptUI.receiptModel.Data.PostingDate(postingDate);
                    }
                    receiptUI.updateFiscalYearPeriod(jsonResult);
                },
                //click on No
                function () {
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        receiptUI.receiptModel.Data.ReceiptDate(receiptUI.previousReceiptDate);
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        receiptUI.receiptModel.Data.PostingDate(receiptUI.previousPostingDate);
                    }
                },
                jsonResult.UserMessage.Message);
        } else {
            if (receiptUI.dateChangeBy === dateChanged.receiptDate) {
                receiptUI.isReceiptDateModified = true;
                receiptUI.receiptModel.Data.PostingDate(receiptDate);
                receiptUI.receiptModel.Data.RateDate(receiptDate);
                receiptUI.setExchangeRateValue();
                if (receiptUI.receiptModel.Data.ReceiptType() !== type.RECEIPT) {
                    receiptUI.enableReceiptType(false);
                }
            }
            receiptUI.updateFiscalYearPeriod(jsonResult);
        }
    },

    displayResult: function (jsonResult, uiMode) {
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
                //get new grid data
                var grid = $("#receiptGrid").data("kendoGrid");
                receiptUI.updateGridColDecimal();
                grid.dataSource.read();
                grid = $("#rptOptionalFieldGrid").data("kendoGrid");
                grid.dataSource.read();
                grid = $("#rptDetailOptionalFieldGrid").data("kendoGrid");
                grid.dataSource.read();
            }

            receiptUI.receiptModel.UIMode(uiMode);

            if (!receiptUI.isKendoControlNotInitialised) {
                receiptUI.isKendoControlNotInitialised = true;
                receiptUI.initNumericTextBox();
                receiptUI.initDropDownList();
            }

            receiptGridUtility.updateTextBox();
            receiptUI.populateDropDownList();
            sg.utls.showMessage(jsonResult);
            if (uiMode !== sg.utls.OperationMode.NEW) {
                receiptUI.receiptModel.isModelDirty.reset();
            }
            receiptUISuccess.updateGridButtonState();
        }
    },

    initialLoad: function (result) {
        if (result) {
            receiptUISuccess.displayResult(result);
        }
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    receiptNumberfinderSuccess: function (data) {
        if (data !== null) {
            receiptUI.finderData = data;
            receiptUI.isFromReceiptFInder = true;
            receiptUI.checkIsDirty(receiptUISuccess.setReceiptNumberFinderData);
        }
    },

    setReceiptNumberFinderData: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptUI.receiptModel.Data.ReceiptNumber(receiptUI.finderData.ReceiptNumber);
        receiptUI.get(receiptUI.finderData.ReceiptNumber, receiptUI.finderData.SequenceNumber);
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
            var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
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
        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.finderData.Rate);
        if (receiptUI.receiptModel.Data.ReceiptType() === type.RECEIPT) {
            receiptUI.receiptModel.Data.RateDate(receiptUI.finderData.RateDate);
        }
        var grid = $('#receiptGrid').data("kendoGrid");
        if (grid.dataSource.data().length > 0) {
            receiptUI.isWrongexchangeRate = true;
            var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
            receiptRepository.refresh(data);
        }
    },

    vendorResult: function (result) {
        if (result.VendorNumber) {
            receiptUI.receiptModel.Data.VendorNumber(result.VendorNumber);
            $("#Data_VendorNumber").val(result.VendorNumber);
        }
        if (result.ShortName) {
            receiptUI.receiptModel.Data.VendorShortName(result.ShortName);
            $("#Data_VendorName").val(result.ShortName);
        }
        if (result.RateType) {
            receiptUI.receiptModel.Data.RateType(result.RateType);
            $("#Data_RateType").val(result.RateType);
        }
        var data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        receiptRepository.refresh(data);
    },

    ReceiptCurrencyResult: function (result) {
        if (result) {
            if (result.Data && result.Data.length) {
                var description = result && result.Data.length ? result.Data[0].Description : "";
                var currencyCodeId = result && result.Data.length ? result.Data[0].CurrencyCodeId : "";
                receiptUI.receiptModel.ReceiptCurrencyDescription(description);
                receiptUI.receiptModel.ReceiptCurrDecimal(result.Data[0].DecimalPlacesString);
                receiptUI.receiptModel.FuncDecimals(result.Data[0].DecimalPlacesString);
                $("#txtReceiptCurrencyDescription").val(description);
                $("#txtExtendedCostCurrency").val(currencyCodeId);

            } else {
                receiptUI.receiptModel.Data.ReceiptCurrency(result.CurrencyCodeId);
                receiptUI.receiptModel.ReceiptCurrDecimal(result.DecimalPlacesString);
                receiptUI.receiptModel.ReceiptCurrencyDescription(result.Description);
                $("#txtReceiptCurrencyDescription").val(result.Description);
            }
            receiptRepository.GetExchangeRate(receiptUI.receiptModel.Data.RateType(),
                receiptUI.receiptModel.Data.ReceiptCurrency(), receiptUI.receiptModel.Data.RateDate(),
                receiptUI.receiptModel.Data.ExchangeRate(), receiptUI.receiptModel.Data.HomeCurrency());
        }
    },

    CostCurrencyResult: function (result) {
        if (result) {
            if (result.Data && result.Data.length > 0) {
                var data = result.Data[0];
                receiptUI.receiptModel.AddlCostCurrencyDescription(data.Description || "");
                receiptUI.receiptModel.AddCostCurrDecimal(data.DecimalPlacesString);
                $("#txtAddlCostCurrencyDescription").val(data.Description || "");
                $("#txtTotalCostCurrency").val(data.CurrencyCodeId);

            } else {
                receiptUI.receiptModel.Data.AdditionalCostCurrency(result.CurrencyCodeId);
                receiptUI.receiptModel.AddlCostCurrencyDescription(result.Description);
                receiptUI.receiptModel.AddCostCurrDecimal(result.DecimalPlacesString);
                $("#txtAddlCostCurrencyDescription").val(result.Description);
            }
        }
    },

    updateGridButtonState: function () {
        var rptType = receiptUI.receiptModel.Data.ReceiptType();
        sg.viewList.allowInsert("receiptGrid", rptType === type.RECEIPT);
        sg.viewList.allowDelete("receiptGrid", rptType === type.RECEIPT);
    },

    refresh: function (result) {
        $("#message").empty();
        if (result && !result.UserMessage.IsSuccess) {
            receiptUI.isVendorNumberCorrect = false;
            if (receiptUI.isWrongexchangeRate || receiptUI.isWrongRateType) {
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

            if (receiptUI.receiptModel.Data.ExchangeRate()) {
                receiptUI.exchangeRateOldValue = receiptUI.receiptModel.Data.ExchangeRate();
            }
            if (receiptUI.additionalCostCurrencyClicked && receiptUI.receiptModel.Data.AdditionalCostCurrency()) {
                receiptUI.oldAdditionalCostCurrency = receiptUI.receiptModel.Data.AdditionalCostCurrency();
            }
            if (receiptUI.receiptModel.Data.RateType()) {
                receiptUI.RateTypeOldValue = receiptUI.receiptModel.Data.RateType();
            }
            if (receiptUI.receiptModel.UIMode() !== sg.utls.OperationMode.SAVE) {
                if (receiptUI.receiptModel.Data.VendorExists() === 0 && receiptUI.receiptModel.Data.VendorNumber()) {
                    sg.utls.showMessageInfo(sg.utls.msgType.WARNING, $.validator.format(receiptResources.RecordDoesNotExist, receiptResources.VendorNumber, receiptUI.receiptModel.Data.VendorNumber()));
                    sg.controls.Focus($("#Data_VendorNumber"));
                }
            }

            receiptGridUtility.updateTextBox();
            if (result) {
                receiptUI.receiptModel.Data.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
                receiptUI.receiptModel.Data.TotalReturnCostDecimal(result.TotalReturnCostDecimal);
            }
            if (result.Warnings) {
                sg.utls.showMessageInfo(sg.utls.msgType.WARNING, result.Warnings[0].Message);
            }
            receiptUI.updateGridColDecimal();
        }
        receiptUISuccess.updateGridButtonState();
    }
};

var receiptGridUtility = {
    isCellEditable: true,
    isDataRefreshInProgress: false,
    selectedIndex: 0,

    updateNumericTextBox: function (id, value) {
        var numericTextbox = $("#" + id).data("kendoNumericTextBox");
        var data = receiptUI.receiptModel.Data;
        var decimal = data.AdditionalCostCurrency() === data.ReceiptCurrency() ? data.ReceiptCurrencyDecimals() : receiptUI.receiptModel.FuncDecimals();
        decimal = id === "txtTotalCost" ? data.TotalCostReceiptAdditionalDecimal() : decimal;
        decimal = id === "txtTotalReturnCost" ? data.ReceiptCurrencyDecimals() : decimal;
        decimal = id === "txtTotalAdjustmentCost" ? data.TotalCostReceiptAdditionalDecimal() : decimal;
        value = value || 0;

        if (numericTextbox) {
            var symbol = kendo.culture().numberFormat['.'];
            var length = 13;
            if (id === "txtpopupExchangeRate" || id === 'Data_ExchangeRate') {
                decimal = 7;
                length = 8;
            }
            numericTextbox.options.format = "n" + decimal;
            numericTextbox.options.decimals = decimal;
            sg.utls.kndoUI.restrictDecimals(numericTextbox, decimal, length);
            value = value.toString().replace('.', symbol);
            numericTextbox.value(value);
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

    getCurrentRowCell: function (colIndex) {
        var grid = $('#receiptGrid').data("kendoGrid");
        var dataRows = grid.items();
        var index = dataRows.index(grid.select());
        return grid.tbody.find(">tr:eq(" + index + ") >td:eq(" + colIndex + ")");
    }
};

$(document).ready(function () {
    receiptUI.init();
    $(window).bind('beforeunload', function () {
        var gridDirty = sg.viewList.dirty("receiptGrid");
        var model = receiptUI.receiptModel;
        var modelDirty = model.isModelDirty.isDirty();
        if (globalResource.AllowPageUnloadEvent && (gridDirty || modelDirty) && !model.DisableScreen()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, receiptResources.Receipts)).text();
        }
    });
    $(window).bind('unload', function () {
        if (globalResource.AllowPageUnloadEvent) {
            sg.utls.destroySession();
        }
    });
});

