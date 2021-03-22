// The MIT License (MIT) 
// Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved.
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

/*
 * The following are global objects external to this source file
 */
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

const ReceiptTypeEnum = Object.freeze({
    Text: 1,
    Date: 3,
    Time: 4,
    Number: 6,
    Integer: 8,
    YesOrNo: 9,
    Amount: 100,
});

const CurrencySelectedEnum = Object.freeze({
    receiptCurrency: 1,
    additionalCostCurrency: 2
});

const DateChangedEnum = Object.freeze({
    receiptDate: 1,
    postingDate: 2
});

const HeaderFieldsEnum = Object.freeze({
    ReceiptType: 8,
    AdditionalCost: 16,
    RequireLabels: 26,
    ReceiptCurrency: 11,
    AdditionalCostCurrency: 19,
    VendorNumber: 10,
    ExchangeRate: 12,
    RateType: 13
});

const TypeEnum = Object.freeze({
    RECEIPT: 1,
    RETURN: 2,
    ADJUSTMENT: 3,
    COMPLETE: 4
});

const StatusTypeEnum = Object.freeze({
    Yes: 1,
    No: 0
});

const RecordStatusEnum = Object.freeze({
    ENTERED: 1,
    POSTED: 2,
    COSTED: 3,
    DAYENDCOMPLETED: 20
});

var receiptUI = receiptUI || {};
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

    /**
     * @function
     * @name columnIsEditable
     * @description For using new grid and optional grid
     * @namespace receiptUI
     * @public
     *
     * @param {string} colName The column name
     */
    columnIsEditable: function (colName) {
        let model = receiptUI.receiptModel;
        let rptType = model.Data.ReceiptType();
        let enableScreen = !model.DisableScreen();
        let columns = ["ITEMNO", "LOCATION", "RECPQTY", "RECPUNIT", "UNITCOST", "RECPCOST", "LABELS", "MANITEMNO"];

        if (columns.indexOf(colName) > -1) {
            return rptType === TypeEnum.RECEIPT && enableScreen;
        }
        if (colName === "COMMENTS") {
            return rptType !== TypeEnum.COMPLETE && enableScreen;
        }
        if (colName === "RETURNCOST") {
            return false;
        }
        if (colName === "RETURNQTY") {
            return rptType === TypeEnum.RETURN && enableScreen;
        }
        if (colName === "ADJUNITCST" || colName === "ADJCOST") {
            return rptType === TypeEnum.ADJUSTMENT && enableScreen;
        }
        return true;
    },

    /**
     * @function
     * @name updateGridModel
     * @description Update grid model run time before build grid
     * @namespace receiptUI
     * @public
     */
    updateGridModel: function () {
        let vm = receiptViewModel;
        let gridColDefs = receiptGridModel.ColumnDefinitions;

        let fields = ["RECPQTY", "RETURNQTY", "UNITCOST", "ADJUNITCST", "RECPCOST", "RETURNCOST", "ADJCOST"];
        fields.forEach(function (f) {
            let precision = 0;
            if (f.indexOf("QTY") > -1) {
                precision = vm.IsFracQty ? vm.FracDecimals : 0;
            } else if (f.indexOf("UNITC") > -1) {
                precision = 6;
            } else {
                precision = vm.ReceiptCurrencyDecimals || vm.FuncDecimals || 3;
            }
            let col = gridColDefs.filter(function (c) { return c.FieldName === f; })[0];
            col.Precision = precision;
        });
    },

    /**
     * @function
     * @name updateGridColDecimal
     * @description Update column decimal place using column template
     * @namespace receiptUI
     * @public
     */
    updateGridColDecimal: function () {
        let decimal = receiptUI.receiptModel.Data.ReceiptCurrencyDecimals().toString();
        let cols = [{ "field": "RECPCOST", "decimal": decimal },
            { "field": "RETURNCOST", "decimal": decimal },
            { "field": "ADJCOST", "decimal": decimal }];
        for (var i = 0, length = cols.length; i < length; i++) {
            let template = sg.viewList.columnTemplate("receiptGrid", cols[i].field);
            template = template.replace(/[0-9]/g, cols[i].decimal);
            sg.viewList.columnTemplate("receiptGrid", cols[i].field, template);
        }
    },

    /**
     * @function
     * @name convertIntToTime
     * @description Convert integer time seconds to time string format like timecard screen
     * @namespace receiptUI
     * @public
     *
     * @param {number} The time specification
     */
    convertIntToTime: function (intTime) {
        let date = new Date(null);
        date.setSeconds(intTime);
        let timeString = date.toISOString().substr(11, 8);
        return timeString;
    },

    /**
     * @function
     * @name showGridTimeColumn
     * @description Set the column template type
     * @namespace receiptUI
     * @public
     *
     * @param {any} e The event specifier
     * @param {object} column The column specification
     */
    showGridTimeColumn: function (e, column) {
        column.Template = '#=receiptUI.convertIntToTime(data.RECPQTY)#';
    },

    /**
     * @function
     * @name showGridCommentColumn
     * @description Custom function to set column template used in Grid JSON configuration
     * @public
     *
     * @param {any} e The event specifier
     * @param {object} column The column specification
     */
    showGridCommentColumn: function (e, column) {
        column.Template = '#:receiptUI.showCommentBasedOnItem(data.COMMENTS, data.ITEMNO)#';
    },

    /**
     * @function
     * @name showCommentBasedOnItem
     * @description Show comments column based on item value. If item value is start 'A', 
     *              show comments as comment value + "The item name starts with 'A', other 
     *              wise just show empty.
     * @namespace receiptUI
     * @public
     *
     * @param {string} comments The comments string
     * @param {string} itemNo The item number string
     *  
     * @returns {string} The return string
     */
    showCommentBasedOnItem: function (comments, itemNo) {
        return itemNo.startsWith("A") ? comments + " The item name starts with 'A'" : "";
    },

    /**
     * @function
     * @name updateFinderFilter
     * @description 
     * @namespace receiptUI
     * @public
     *
     * @param {any} record 
     * @param {object} finder 
     */
    updateFinderFilter: function (record, finder) {
        //finder.Filter = "RECPQTY=111";
    },

    /**
     * @function
     * @name showCustomIcon
     * @description Show detail optional field in popup window
     * @namespace receiptUI
     * @public
     *
     * @param {any} e The event
     * @param {object} column The column specification
     */
    showCustomIcon: function (e, column) {
        column.Template = kendo.template('<span style="padding-right:50px;">Yes</span><input class="icon pencil-edit" type="button">');
    },

    /**
     * @function
     * @name showIcons
     * @description Show detail optional field in popup window
     * @namespace receiptUI
     * @public
     *
     * @param {any} e The event
     * @param {object} column The column specification
     */
    showIcons: function (e, column) {
        // Data is current record, such data.VENDNAME
        column.Template = kendo.template('<input class="icon pencil-edit" type="button"><input value="AA" id="btnAA" type="button"><input value="BB" id="btnBB" type="button">');
    },

    /**
     * @function
     * @name ShowDetailOptionalField
     * @description Show detail optional field in popup window
     * @namespace receiptUI
     * @public
     */
    showDetailOptionalField: function () {
        let grid = $('#receiptGrid').data("kendoGrid"),
            selectedRow = sg.utls.kndoUI.getSelectedRowData(grid),
            filter = kendo.format("SEQUENCENO={0} AND LINENO={1}", selectedRow.SEQUENCENO, selectedRow.LINENO),
            isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();

        sg.optionalFieldControl.showPopUp("rptDetailOptionalFieldGrid", "detailOptionalField", isReadOnly, filter, "receiptGrid");
    },

    /**
     * @function
     * @name showOptionalField
     * @description Show optional field in popup window
     * @namespace receiptUI
     * @public
     */
    showOptionalField: function () {
        let isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();
        sg.optionalFieldControl.showPopUp("rptOptionalFieldGrid", "optionalField", isReadOnly);
    },

    /**
     * @function
     * @name initPopUps
     * @description Initialize the various popup windows
     * @namespace receiptUI
     * @public
     */
    initPopUps: function () {
        sg.utls.intializeKendoWindowPopup('#optionalField', receiptResources.OptionalFields, function () {
            let count = $("#rptOptionalFieldGrid").data("kendoGrid").dataSource.total();
            receiptUI.receiptModel.Data.OptionalFields(count);
        });
        sg.utls.intializeKendoWindowPopup('#detailOptionalField', receiptResources.OptionalFields, function () {
            sg.optionalFieldControl.closePopUp('rptDetailOptionalFieldGrid', 'receiptGrid');
        });
        sg.utls.intializeKendoWindowPopup('#exchangeRate', receiptResources.RateSelection);
    },

    /**
     * @function
     * @name customGridChanged
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     * 
     * @param value
     */
    customGridChanged: function (value) {
    },

    /**
     * @function
     * @name customGridAfterSetActiveRecord
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     */
    customGridAfterSetActiveRecord: function (value) {
    },

    /**
     * @function
     * @name customGridBeforeDelete
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param event
     */
    customGridBeforeDelete: function (value, event) {
    },

    /**
     * @function
     * @name customGridAfterDelete
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     */
    customGridAfterDelete: function (value) {
    },

    /**
     * @function
     * @name customGridBeforeCreate
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param event
     */
    customGridBeforeCreate: function (event) {
    },

    /**
     * @function
     * @name customGridAfterCreate
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     */
    customGridAfterCreate: function (value) {
        // After create detail grid record, create default detail optional fields
        let url = sg.utls.url.buildUrl("IC", "Receipt", "InsertDetailOptionalField");
        sg.utls.ajaxPostSync(url, null, function () { });

        // Binding custom grid button click event after create
        setTimeout(function () {
            $("input.icon").click(function (e) {
                //sg.utls.showMessageInfo(sg.utls.msgType.INFO, "First button click");
            });

            $("#btnAA").click(function (e) {
                sg.utls.showMessageInfo(sg.utls.msgType.WARNING, "Second button click");
            });

            $("#btnBB").click(function (e) {
                sg.utls.showMessageInfo(sg.utls.msgType.INFO, "Third button click");
            });
        }, 500);

    },

    /**
     * @function
     * @name customGridAfterInsert
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     */
    customGridAfterInsert: function (value) {
    },

    /**
     * @function
     * @name customColumnChanged
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param currentValue
     * @param value
     * @param event
     */
    customColumnChanged: function (currentValue, value, event) {
    },

    /**
     * @function
     * @name customColumnBeforeDisplay
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param properties
     */
    customColumnBeforeDisplay: function (value, properties) {
    },

    /**
     * @function
     * @name customColumnDoubleClick
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param event
     */
    customColumnDoubleClick: function (value, event) {
    },

    /**
     * @function
     * @name customColumnBeforeFinder
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param options
     */
    customColumnBeforeFinder: function (value, options) {
    },

    /**
     * @function
     * @name customColumnBeforeEdit
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param event
     * @param fieldName
     */
    customColumnBeforeEdit: function (value, event, fieldName) {
        event.preventDefault();
    },

    /**
     * @function
     * @name customColumnStartEdit
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param editor
     */
    customColumnStartEdit: function (value, editor) {
        $(editor).after('<input class="icon pencil-edit" style="margin-left:-20px;" id="btnDetailJobs" tabindex="-1" type="button" />');
        $("input.icon").click(function () {
            //sg.utls.showMessageInfo(sg.utls.msgType.INFO, "editor button click");
        });
    },

    /**
     * @function
     * @name customColumnEndEdit
     * @description Custom message handler
     * @namespace receiptUI
     * @public
     *
     * @param value
     * @param editor
     */
    customColumnEndEdit: function (value, editor) {
    },

    /**
     * @function
     * @name init
     * @description Primary initialization routine
     * @namespace receiptUI
     * @public
     */
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

        // Initialize the new grid
        sg.viewList.init("receiptGrid", false, receiptUI.updateGridModel);
        sg.optionalFieldControl.init("rptOptionalFieldGrid", { "viewId": "IC0377", "filter": "LOCATION=2", "allowDelete": true, "allowInsert": true, "type": 0 }, false);
        sg.optionalFieldControl.init("rptDetailOptionalFieldGrid", { "viewId": "IC0377", "filter": "LOCATION=3", "allowDelete": true, "allowInsert": true, "type": 0 }, false);

        if (!receiptViewModel.DisableScreen && receiptViewModel.IsExists) {
            receiptUISuccess.displayResult(receiptViewModel, sg.utls.OperationMode.SAVE);
        }
    },

    /**
     * @function
     * @name initButtons
     * @description Initialize buttons
     * @namespace receiptUI
     * @public
     */
    initButtons: function () {
        sg.exportHelper.setExportEvent("btnOptionExport", sg.dataMigration.Receipt, false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", sg.dataMigration.Receipt, false, $.noop);

        $("#btnRefresh").on("click", function (e) {
            let grid = $('#receiptGrid').data("kendoGrid");
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
            let receiptDate = $("#txtReceiptDate").val();
            receiptUI.previousReceiptDate = receiptUI.receiptModel.Data.ReceiptDate();
            let validDate = sg.utls.kndoUI.checkForValidDate(receiptDate);
            if (validDate) {
                receiptUI.dateChangeBy = DateChangedEnum.receiptDate;
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
            let postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
            receiptUI.previousPostingDate = receiptUI.receiptModel.Data.PostingDate();
            let validDate = sg.utls.kndoUI.checkForValidDate(postingDate);
            if (validDate) {
                receiptUI.dateChangeBy = DateChangedEnum.postingDate;
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

        $("#btnNewReceipt").on('click', function () {
            receiptUI.createNewButtonClicked = true;
            receiptUI.checkIsDirty(receiptUI.create);
            receiptUI.isWrongexchangeRate = false;
            receiptUI.prevExRate = "";
            receiptUI.exchangeRateOldValue = "";
            receiptUI.isVendorNumberCorrect = false;
            receiptUI.createNewButtonClicked = false;
        });

        // Save receipt
        $("#btnSave").on('click', function () {
            if (sg.viewList.commit("receiptGrid")) {
                sg.utls.SyncExecute(receiptUI.save);
                sg.viewList.dirty("receiptGrid", false);
            }
        });

        // Receipt Post Functionality
        $("#btnPost").on('click', function () {
            if (sg.viewList.commit("receiptGrid") && $("#frmReceipt").valid()) {
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
                        let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                        receiptRepository.post(data, receiptUI.receiptModel.Data.SequenceNumber(), true);
                    },
                    function () { // No
                        sg.utls.clearValidations("frmReceipt");
                        let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
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
        // Delete receipt
        $("#btnDelete").on('click', function () {
            if ($("#frmReceipt").valid()) {
                let message = jQuery.validator.format(receiptResources.DeleteConfirmMessage, receiptResources.ReceiptNumber, receiptUI.receiptModel.Data.ReceiptNumber());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmReceipt");
                    receiptRepository.deleteReceipt(receiptUI.receiptModel.Data.ReceiptNumber(), receiptUI.receiptModel.Data.SequenceNumber());
                }, null, message, receiptResources.DeleteTitle);
            }
        });

        // Optional field click
        $("#three").on('click', function () {
            receiptUI.showOptionalField();
        });

        $("#Data_VendorNumber").on('change', function () {
            $("#message").empty();
            sg.delayOnChange("btnVendorNumberFinder", $("Data_VendorNumber"), function () {
                receiptUI.isVendorNumberChanged = true;
                receiptUI.RefreshHeader();
            });
        });

        $("#Data_ReceiptCurrency").on('change', function () {
            $("#message").empty();
            sg.delayOnChange("btnReceiptCurrencyFinder", $("Data_ReceiptCurrency"), function () {
                receiptUI.currencySelected = CurrencySelectedEnum.receiptCurrency;
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
                receiptUI.currencySelected = CurrencySelectedEnum.additionalCostCurrency;
                receiptUI.additionalCostCurrencyClicked = true;
                receiptUI.RefreshHeader();
            });
        });

        $("#Data_RateType").on('change', function () {
            $("#message").empty();
            receiptUI.isWrongRateType = true;
            let rateType = $("#Data_RateType").val();
            sg.delayOnChange("btnRateTypeFinder", $("Data_RateType"), function () {
                let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                if (rateType) {
                    receiptUI.receiptModel.Data.ExchangeRate(1);
                    receiptRepository.refresh(receiptUI.receiptModel.Data);
                } else {
                    receiptUI.receiptModel.Data.RateType(rateType);
                    receiptRepository.GetHeaderValues(data, HeaderFieldsEnum.RateType);
                }
            });
        });

        $("#Data_ExchangeRate").on('change', function () {
            receiptUI.isWrongRateType = false;
            $("#message").empty();
            let rate = kendo.parseFloat($("#Data_ExchangeRate").val());
            receiptUI.isWrongexchangeRate = true;
            let data = receiptUI.receiptModel.Data;
            if (rate && receiptUI.receiptModel.Data.ReceiptType() === TypeEnum.RECEIPT) {
                receiptUI.exchangeRateOldValue = data.ExchangeRate();
                receiptRepository.checkRateSpread(data.RateType(), data.ReceiptCurrency(), data.RateDate(), rate, data.HomeCurrency());
            } else {
                receiptUI.receiptModel.Data.ExchangeRate(rate);
                receiptUI.RefreshHeader();
            }
        });

        $("#Data_ExchangeRate, #txtpopupExchangeRate").on('focus', function (e) {
            e.preventDefault();
            let culture = kendo.culture();
            let symbol = culture.numberFormat['.'];
            if (symbol !== ".") {
                this.value = this.value.replace(".", symbol);
            }
            receiptUI.exitExchangeRateChange = false;
        });

        // On Change for Exchange Rate
        $("#txtpopupExchangeRate").on('change', function () {
            if (receiptUI.exitExchangeRateChange) {
                let value = receiptUI.receiptModel.Data.ExchangeRate().toString();
                value = value.replace(".", kendo.culture().numberFormat['.']);
                $("#txtpopupExchangeRate").val(value);
                return;
            }
            $("#message").empty();
            receiptUI.isWrongRateType = false;

            let rate = kendo.parseFloat($("#txtpopupExchangeRate").val());
            receiptUI.isWrongexchangeRate = true;

            if (!rate) {
                receiptUI.receiptModel.Data.ExchangeRate(0);
                $("#txtpopupExchangeRate").val(0);
            }
            let data = receiptUI.receiptModel.Data;
            if (rate && data.ReceiptType() === TypeEnum.RECEIPT) {
                receiptUI.exchangeRateOldValue = data.ExchangeRate();
                receiptRepository.checkRateSpread(data.RateType(), data.ReceiptCurrency(), data.RateDate(), rate, data.HomeCurrency());
            } else {
                receiptUI.receiptModel.Data.ExchangeRate(rate);
                receiptUI.RefreshHeader();
            }
            receiptUI.exitExchangeRateChange = !receiptUI.exitExchangeRateChange;
        });
    },

    /**
     * @function
     * @name initNumericTextBox
     * @description Initialize the Kendo numeric controls
     * @namespace receiptUI
     * @public
     */
    initNumericTextBox: function () {
        let model = receiptUI.receiptModel;
        let data = model.Data;
        let decimal = data.AdditionalCostCurrency() === data.ReceiptCurrency() ? data.ReceiptCurrencyDecimals() : model.FuncDecimals();

        let ctrlAddlCost = $("#txtAddlCost").kendoNumericTextBox({
            format: "n" + decimal,
            spinners: false,
            step: 0,
            min: 0,
            decimals: 13,
            change: function (e) {
                receiptUI.previousAdditionalCost = receiptUI.receiptModel.Data.AdditionalCost();
                receiptUI.enableReceiptType(false);
                receiptUI.isWrongRateType = false;
                let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.GetHeaderValues(data, HeaderFieldsEnum.AdditionalCost);
            }
        }).data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(ctrlAddlCost, decimal, 13);

        let exchangeRate = $("#Data_ExchangeRate").kendoNumericTextBox({
            format: "n7",
            spinners: false,
            step: 0,
            decimals: 7,
            min: 0.0000000,
            value: parseFloat(receiptUI.receiptModel.Data.ExchangeRate())
        }).data("kendoNumericTextBox");
        $(exchangeRate.element).unbind("input");
        sg.utls.kndoUI.restrictDecimals(exchangeRate, 7, 8);

        let txtpopupExchangeRate = $("#txtpopupExchangeRate").kendoNumericTextBox({
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

    /**
     * @function
     * @name initDropDownList
     * @description Initialize the drop down list boxes
     * @namespace receiptUI
     * @public
     */
    initDropDownList: function () {
        let fields = ["Data_ReceiptType", "Data_AdditionalCostAllocationType"];
        $.each(fields, function (index, field) {
            sg.utls.kndoUI.dropDownList(field);
        });

        $("#Data_ReceiptType").data("kendoDropDownList").bind("change", receiptUI.typeSelectionChanged);
        $("#Data_AdditionalCostAllocationType").data("kendoDropDownList").bind("change", receiptUI.costSelectionChanged);
    },

    /**
     * @function
     * @name updateFiscalYearPeriod
     * @description Update the fiscal year period
     * @namespace receiptUI
     * @public
     *  
     * @param {object} result The JSON results object
     */
    updateFiscalYearPeriod: function (result) {
        let model = receiptUI.receiptModel.Data;
        if (result.UserMessage.IsSuccess) {
            let fiscalYear = result.Data.FiscalYear;
            let fiscalPeriod = result.Data.FiscalPeriod;
            model.FiscalPeriod(parseInt(fiscalPeriod));
            model.FiscalYear(fiscalYear);
        } else {
            model.FiscalPeriod("");
            model.FiscalYear("");
            sg.utls.showMessage(result);
        }
    },

    /**
     * @function
     * @name create
     * @description Create a new receipt
     * @namespace receiptUI
     * @public
     */
    create: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptRepository.create(receiptUI.receiptModel.Data.ReceiptNumber());
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    /**
     * @function
     * @name save
     * @description Save a receipt
     * @namespace receiptUI
     * @public
     */
    save: function () {
        if ($("#frmReceipt").valid()) {
            receiptUI.receiptSave();
        }
    },

    /**
     * @function
     * @name receiptSave
     * @description Save a receipt
     * @namespace receiptUI
     * @public
     */
    receiptSave: function () {
        let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        data.RecordStatus = 1;
        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.SAVE) {
            receiptRepository.update(data);
        } else {
            receiptRepository.add(data);
        }
    },

    /**
     * @function
     * @name checkIsDirty
     * @description Check to see if the model data has been changed
     * @namespace receiptUI
     * @public
     *
     * @param {Function} functionToCall The callback function to invoke
     */
    checkIsDirty: function (functionToCall) {
        let modelData = receiptUI.receiptModel.Data;
        let gridDirty = sg.viewList.dirty("receiptGrid");
        let modelDirty = receiptUI.receiptModel.isModelDirty.isDirty();
        let exists = true;

        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.NEW && !receiptUI.isFromReceiptFInder) {
            if (modelDirty || gridDirty && receiptUI.receiptNumber) {
                let data = ko.mapping.toJS(modelData, receiptUI.ignoreIsDirtyProperties);
                exists = receiptRepository.receiptExists(modelData.ReceiptNumber(), data);
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
                    functionToCall.call();
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
            functionToCall.call();
        }
    },

    /**
     * @function
     * @name openExchangeRate
     * @description Open the exchange rate popup window
     * @namespace receiptUI
     * @public
     */
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

    /**
     * @function
     * @name initGridLocationFinder
     * @description 
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     * 
     * @returns {object} The altered viewFinder object
     */
    initGridLocationFinder: function (viewFinder) {
        // Add custom defined properties here, grid generic editor will call it
        viewFinder.calculatePageCount = false;
        return viewFinder;
    },

    /**
     * @function
     * @name initReceiptNumberFinder
     * @description Initialize the Receipt Number finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initReceiptNumberFinder: function (viewFinder) {
        viewFinder.viewID = "IC0590";
        viewFinder.viewOrder = 2;
        viewFinder.displayFieldNames = ["RECPNUMBER", "RECPDESC", "RECPDATE", "FISCYEAR", "FISCPERIOD", "PONUM", "REFERENCE",
            "RECPTYPE", "RATEOP", "VENDNUMBER", "RECPCUR", "RECPRATE", "RATETYPE", "RATEDATE", "RATEOVRRD", "ADDCOST",
            "ADDCOSTHM", "ADDCOSTSRC", "ADDCUR", "TOTCSTHM", "TOTCSTSRC", "NUMCSTDETL", "LABELS", "ADDCSTTYPE", "ORIGTOTSRC",
            "ORIGTOTHM", "ADDCSTHOME", "TOTALCOST", "RECPDECIML", "VENDNAME", "VENDEXISTS", "STATUS"];
        viewFinder.returnFieldNames = ["RECPNUMBER"];
        viewFinder.parentValAsInitKey = $("#txtReceiptNumber").val() === "*** NEW ***" ? false : true;
        viewFinder.filter = "DELETED = 0";
        //viewFinder.optionalFieldBindings = "IC0595, IC0377[2]";  // comment out for now as CSFND doesn't support filterCount yet
    },

    /**
     * @function
     * @name initVendorNumberFinder
     * @description Initialize the Vendor Number finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initVendorNumberFinder: function (viewFinder) {
        viewFinder.viewID = "AP0015";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["VENDORID", "VENDNAME", "SWACTV", "IDGRP", "CURNCODE", "SHORTNAME", "SWHOLD",
            "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2"];
        viewFinder.returnFieldNames = ["VENDORID"];
        viewFinder.parentValAsInitKey = true;
        //viewFinder.optionalFieldBindings = "AP0407,AP0500[0]";  // comment out for now as CSFND doesn't support filterCount yet
    },

    /**
     * @function
     * @name initCurrencyCodeFinderCommon
     * @description Initialize the Currency Code finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initCurrencyCodeFinderCommon: function (viewFinder) {
        viewFinder.viewID = "CS0003";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["CURID", "CURNAME", "SYMBOL", "DECIMALS", "SYMBOLPOS", "THOUSSEP", "DECSEP", "NEGDISP"];
        viewFinder.returnFieldNames = ["CURID"];
        viewFinder.parentValAsInitKey = true;
    },

    /**
     * @function
     * @name initReceiptCurrencyFinder
     * @description Initialize the Receipt Currenty finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initReceiptCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    /**
     * @function
     * @name initAddlCostCurrencyFinder
     * @description Initialize the Additional Cost Currency finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initAddlCostCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    /**
     * @function
     * @name initRateTypeFinder
     * @description Initialize the Rate Type finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initRateTypeFinder: function (viewFinder) {
        viewFinder.viewID = "CS0004";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["RATETYPE", "RATEDESC"];
        viewFinder.returnFieldNames = ["RATETYPE"];
        viewFinder.parentValAsInitKey = true;
    },

    /**
     * @function
     * @name initExchangeRateFinder
     * @description Initialize the Exchange Rate finder
     * @namespace receiptUI
     * @public
     *
     * @param {object} viewFinder The viewFinder object
     */
    initExchangeRateFinder: function (viewFinder) {
        viewFinder.viewID = "CS0006";
        viewFinder.viewOrder = 0;
        viewFinder.displayFieldNames = ["RATEDATE", "RATE", "SPREAD"];
        viewFinder.returnFieldNames = ["RATE"];
        viewFinder.parentValAsInitKey = true;
        let rateDate = receiptUI.receiptModel.Data.RateDate();
        let sgRateDate = rateDate.toISOString().split('T')[0].replace(/-/g, '');
        let fromCurrency = receiptUI.receiptModel.Data.ReceiptCurrency();
        let toCurrency = sg.utls.homeCurrency.Code;
        let rateType = receiptUI.receiptModel.Data.RateType();
        viewFinder.filter = kendo.format('RATETYPE={0} AND SOURCECUR={1} AND HOMECUR={2} AND RATEDATE={3}', rateType, fromCurrency, toCurrency, sgRateDate);
        viewFinder.calculatePageCount = false;
    },

    /**
     * @function
     * @name initFinders
     * @description Initialize the finders
     * @namespace receiptUI
     * @public
     */
    initFinders: function () {

        var helper = sg.viewFinderHelper;
        helper.setViewFinder("btnReceiptNumberFinder", "txtReceiptNumber", this.initReceiptNumberFinder);
        helper.setViewFinder("btnVendorNumberFinder", "Data_VendorNumber", this.initVendorNumberFinder);
        helper.setViewFinder("btnReceiptCurrencyFinder", "Data_ReceiptCurrency", this.initReceiptCurrencyFinder);
        helper.setViewFinder("btnAddlCostCurrencyFinder", "Data_AdditionalCostCurrency", this.initAddlCostCurrencyFinder);
        helper.setViewFinder("btnRateTypeFinder", "Data_RateType", this.initRateTypeFinder);
        helper.setViewFinder("btnExchangeRateFinder", "txtpopupExchangeRate", this.initExchangeRateFinder);
    },

    /**
     * @function
     * @name initHamburgers
     * @description Initialize the hamburger menus
     * @namespace receiptUI
     * @public
     */
    initHamburgers: function () {
        let listExchangeRate = [
            sg.utls.labelMenuParams("lnkOpenExchangeRate", receiptResources.EditLink, receiptUI.openExchangeRate, "sagedisable:receiptUI.receiptModel.IsFuncCurrencyDisable")];
        let listOptionalField = [
            sg.utls.labelMenuParams("lnkOpenOptionalFields", receiptResources.AddOrEditLink, receiptUI.showOptionalField, null)];

        LabelMenuHelper.initialize(listExchangeRate, "lnkExchangeRateThree", "receiptUI.receiptModel");
        LabelMenuHelper.initialize(listOptionalField, "lnkOptionalField", "receiptUI.receiptModel");
    },

    /**
     * @function
     * @name typeSelectionChanged
     * @description The Receipt Type changed event handler
     * @namespace receiptUI
     * @public
     *
     * @param {object} e The event specifier
     */
    typeSelectionChanged: function (e) {
        let selectedvalue = $('#Data_ReceiptType').data("kendoDropDownList").dataItem().value;
        if (selectedvalue) {
            receiptUI.showHideColumns(selectedvalue);
            receiptRepository.refresh(receiptUI.receiptModel.Data);
        }
    },

    /**
     * @function
     * @name showColumn
     * @description Show a grid column
     * @namespace receiptUI
     * @public
     *
     * @param {object} field The field specifier
     */
    showColumn: function (field) {
        let grid = $('#receiptGrid').data("kendoGrid");
        let colIndex = GridPreferencesHelper.getColumnIndex('#receiptGrid', field);
        grid.showColumn(colIndex);
        grid.columns[colIndex].attributes['sg_Customizable'] = true;
    },

    /**
     * @function
     * @name hideColumn
     * @description Hide a grid column
     * @namespace receiptUI
     * @public
     *
     * @param {object} field The field specifier
     */
    hideColumn: function (field) {
        let grid = $('#receiptGrid').data("kendoGrid");
        let colIndex = GridPreferencesHelper.getColumnIndex('#receiptGrid', field);
        grid.hideColumn(colIndex);
        grid.columns[colIndex].attributes['sg_Customizable'] = false;
    },

    /**
     * @function
     * @name showHideColumns
     * @description Show or hide a column
     * @namespace receiptUI
     * @public
     *
     * @param {any} value 
     */
    showHideColumns: function (value) {
        let grid = $('#receiptGrid').data("kendoGrid");
        if (!grid) { return; }

        sg.viewList.allowInsert("receiptGrid", value === TypeEnum.RECEIPT);
        sg.viewList.allowDelete("receiptGrid", value === TypeEnum.RECEIPT);

        if (receiptUI.receiptModel) {
            let cols = ["ADJCOST", "ADJUNITCST", "RETURNQTY", "RETURNCOST"];
            cols.forEach(function (colName) {
                sg.viewList.showColumn("receiptGrid", colName, false);
            });

            if (value == TypeEnum.RETURN) {
                ["RETURNQTY", "RETURNCOST"].forEach(function (colName) {
                    sg.viewList.showColumn("receiptGrid", colName, true);
                });
            } else if (value == TypeEnum.ADJUSTMENT) {
                ["ADJCOST", "ADJUNITCST"].forEach(function (colName) {
                    sg.viewList.showColumn("receiptGrid", colName, true);
                });
            }
        }
    },

    /**
     * @function
     * @name costSelectionChanged
     * @description Additional Cost Allocation change handler
     * @namespace receiptUI
     * @public
     *
     * @param {any} e The event specifier
     */
    costSelectionChanged: function (e) {
        let selectedvalue = $('#Data_AdditionalCostAllocationType').data("kendoDropDownList").dataItem().value;
        if (selectedvalue) {
            receiptUI.enableReceiptType(false);
        }
    },

    /**
     * @function
     * @name get
     * @description Get a receipt
     * @namespace receiptUI
     * @public
     */
    get: function () {
        let receiptNumber = receiptUI.receiptModel.Data.ReceiptNumber();
        receiptUI.receiptModel.UIMode(sg.utls.OperationMode.LOAD);
        receiptRepository.get(receiptNumber, receiptUI.receiptModel.DisableScreen());
    },

    /**
     * @function
     * @name enableReceiptType
     * @description Enable or disable the Receipt Type dropdownlist
     * @namespace receiptUI
     * @public
     *
     * @param {boolean} isEnable Boolean flag
     */
    enableReceiptType: function (isEnable) {
        let ctrl = $("#Data_ReceiptType").data("kendoDropDownList");
        if (ctrl) {
            ctrl.wrapper.show();
            ctrl.enable(isEnable);
        }
    },

    /**
     * @function
     * @name setExchangeRate
     * @description Set the exchange rate
     * @namespace receiptUI
     * @public
     *
     * @param {any} jsonResult The JSON result payload
     */
    setExchangeRate: function (jsonResult) {
        let rate = jsonResult.Data.Rate;
        receiptUI.prevExRate = rate;
        receiptUI.receiptModel.Data.ExchangeRate(rate);
        $("#Data_ExchangeRate").data("kendoNumericTextBox").value(rate);
        $("#txtpopupExchangeRate").data("kendoNumericTextBox").value(rate);
        receiptUI.exchangeRateOldValue = rate;
    },

    /**
     * @function
     * @name setExchangeRateValue
     * @description Set the exchange rate value
     * @namespace receiptUI
     * @public
     */
    setExchangeRateValue: function () {
        receiptRepository.GetExchangeRate(
            receiptUI.receiptModel.Data.RateType(),
            receiptUI.receiptModel.Data.ReceiptCurrency(),
            receiptUI.receiptModel.Data.RateDate(),
            receiptUI.receiptModel.Data.ExchangeRate(),
            receiptUI.receiptModel.Data.HomeCurrency()
        );
    },

    /**
     * @function
     * @name refreshHeader
     * @description Refresh the header
     * @namespace receiptUI
     * @public
     */
    RefreshHeader: function () {
        let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        receiptRepository.refresh(data);
    }
};

/*
 * TODO - Add description for this object
 */
let receiptUISuccess = {


    finderSuccess: function (data) {
        if (data) {
        }
    },

    /**
     * @function
     * @name setkey
     * @description Store the receipt number for later use
     * @namespace receiptUISuccess
     * @public
     */
    setkey: function () {
        receiptUI.receiptNumber = receiptUI.receiptModel.Data.ReceiptNumber();
    },

    /**
     * @function
     * @name onSaveDetailsCompleted
     * @description Save details completed event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    onSaveDetailsCompleted: function (result) {
        let modelData = receiptUI.receiptModel.Data;
        modelData.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
        modelData.TotalCostReceiptAdditional(result.TotalCostReceiptAdditional);
        modelData.TotalReturnCost(result.TotalReturnCost);
        modelData.TotalAdjCostReceiptAddl(result.TotalAdjCostReceiptAddl);

        let util = receiptGridUtility;
        util.updateNumericTextBox("txtTotalCost", modelData.TotalCostReceiptAdditional());
        util.updateNumericTextBox("txtTotalReturnCost", modelData.TotalReturnCost());
        util.updateNumericTextBox("txtTotalAdjustmentCost", modelData.TotalAdjCostReceiptAddl());

        let grid = $('#receiptGrid').data("kendoGrid");
        grid.dataSource.read();
    },

    /**
     * @function
     * @name refreshReceiptDetail
     * @description Refrech receipt detail event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    refreshReceiptDetail: function (result) {
        receiptGridUtility.isDataRefreshInProgress = true;
        if (!result.UserMessage.Errors) {
            ko.mapping.fromJS(result.Data.ReceiptDetail, {}, receiptUI.receiptModel.Data.ReceiptDetail);
        }
        sg.utls.showMessage(result);
        receiptGridUtility.isDataRefreshInProgress = false;
        let grid = $('#receiptGrid').data("kendoGrid");
        grid.dataSource.read();
    },

    /**
     * @function
     * @name deleteDetailSuccess
     * @description Delete detail success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    DeleteDetailSuccess: function (result) {
        let grid = $("#receiptGrid").data("kendoGrid");
        grid.dataSource.read();
    },

    /**
     * @function
     * @name getVendorDetailsSuccess
     * @description Get vendor details success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getVendorDetailsSuccess: function (result) {
        if (result.Data) {
            if (result.Data.RateType) {
                receiptUI.receiptModel.Data.RateType(result.Data.RateType);
                $("#Data_RateType").val(result.Data.RateType);
                receiptUI.rateTypeVendorDetail = result.Data.RateType;
            }
        }
    },

    /**
     * @function
     * @name rateTypeSelect
     * @description Rate type select success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    rateTypeSelect: function (result) {
        if (!result) return;
    },

    /**
     * @function
     * @name getExchangeRate
     * @description Get the exchange rate
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getExchangeRate: function (result) {
        if (result.UserMessage.Message) {
            receiptUI.setExchangeRate(result);
        }
        if (receiptUI.isReceiptDateModified === true || receiptUI.isWrongRateType === true) {
            receiptUI.setExchangeRate(result);
        }
    },

    /**
     * @function
     * @name getRateSpread
     * @description Get rate spread event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getRateSpread: function (result) {
        if (result.UserMessage.Message) {
            sg.utls.showKendoConfirmationDialog(
                // Click on Yes
                function () {
                    let modelData = receiptUI.receiptModel.Data;
                    receiptUI.prevExRate = modelData.ExchangeRate();
                    receiptUI.exchangeRateOldValue = receiptUI.prevExRate;
                    if (sg.viewList.dirty("receiptGrid")) {
                        let data = ko.mapping.toJS(modelData, receiptUI.ignoreIsDirtyProperties);
                        receiptRepository.refresh(data);
                    }
                },
                // Click on No
                function () {
                    if (receiptUI.exchangeRateOldValue) {
                        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.exchangeRateOldValue);
                        receiptUI.prevExRate = receiptUI.exchangeRateOldValue;
                    }
                    if (receiptUI.exchangeRateOldValue === null && receiptUI.prevExRate) {
                        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.prevExRate);
                    }
                },
                result.UserMessage.Message);
        } else {
            receiptUI.prevExRate = receiptUI.receiptModel.Data.ExchangeRate();
            receiptUI.exchangeRateOldValue = receiptUI.prevExRate;
            if (sg.viewList.dirty("receiptGrid")) {
                let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                receiptRepository.refresh(data);
            }
        }
    },

    /**
     * @function
     * @name getItemTypeSuccess
     * @description Get item type success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getItemTypeSuccess: function (result) {
        sg.ic.utls.setItemTypeResponse(result, "#btnManufacturerItemFinder", receiptUISuccess.manufacturerItem);
    },

    /**
     * @function
     * @name getResult
     * @description Get result success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getResult: function (result) {
        let modelData = receiptUI.receiptModel.Data;
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            if (result.IsExists === true) {
                receiptUISuccess.displayResult(result, sg.utls.OperationMode.SAVE);
                if (modelData.ReceiptType() === TypeEnum.RECEIPT || modelData.ReceiptType() === TypeEnum.ADJUSTMENT) {
                    sg.controls.Focus($("#txtDescription"));
                } else {
                    sg.controls.KendoDropDownFocus($("#Data_ReceiptType"));
                }
            } else {
                receiptUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
            }
            receiptUISuccess.setkey();
        } else {
            modelData.ReceiptNumber(receiptUI.receiptNumber);
            if (result) {
                modelData.TotalCostReceiptAdditionalDecimal(result.TotalCostReceiptAdditionalDecimal);
                modelData.TotalReturnCostDecimal(result.TotalReturnCostDecimal);
            }
        }
        sg.utls.showMessage(result);
        sg.viewList.dirty("receiptGrid", false);
        receiptGrid.setFirstLineEditable = false;
        if (modelData.ReceiptType() === TypeEnum.RETURN) {
            sg.controls.disable($("#btnDetailAddLine"));
        }
    },

    /**
     * @function
     * @name actionSuccess
     * @description Action success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {string} action The action verb
     * @param {object} result JSON result payload
     */
    actionSuccess: function (action, result) {
        if (result.UserMessage.IsSuccess) {
            if (action === "add" || action === "update" || action === "post" || action === "create") {
                receiptUI.addLineClicked = false;
            }
            receiptUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
            receiptUI.receiptModel.isModelDirty.reset();
            receiptUISuccess.setkey();
        }
        if (result.UserMessage.Warnings && result.UserMessage.Warnings.length > 0 && action === "add") {
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, result.UserMessage.Warnings[0].Message);
        }
        sg.utls.showMessage(result);
        if (action === "add" || action === "update") {
            receiptGrid.setFirstLineEditable = false;
        }
        if (action === "create") {
            sg.controls.Select($("#txtReceiptNumber"));
        }
    },

    /**
     * @function
     * @name getHeaderValues
     * @description Get header values event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getHeaderValues: function (result) {
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
                let fields = ["TotalCostReceiptAdditional", "TotalReturnCost", "TotalAdjCostReceiptAddl",
                    "TotalExtendedCostSource", "TotalExtendedCostAdjusted", "ReceiptCurrencyDecimals",
                    "TotalCostReceiptAdditionalDecimal", "TotalReturnCostDecimal"];
                for (var i = 0, length = fields.length; i < length; i++) {
                    f = fields[i];
                    let value = result[f];
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

    /**
     * @function
     * @name getRateType
     * @description Get rate type success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    getRateType: function (result) {
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

    /**
     * @function
     * @name checkDate
     * @description Check date event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    checkDate: function (result) {
        let receiptDate = sg.utls.kndoUI.getFormattedDate($("#txtReceiptDate").val());
        let postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
        if (result.UserMessage.Message && result.UserMessage.IsSuccess) {
            sg.utls.showKendoConfirmationDialog(
                // Click on Yes
                function () {
                    let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                    receiptRepository.refresh(receiptUI.receiptModel.Data);
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        receiptUI.isReceiptDateModified = true;
                        receiptUI.receiptModel.Data.ReceiptDate(receiptDate);
                        receiptUI.receiptModel.Data.RateDate(receiptDate);
                        receiptUI.setExchangeRateValue();
                        if (receiptUI.receiptModel.DefaultPostingDate() === 1) {
                            receiptUI.receiptModel.Data.PostingDate(receiptDate);
                        }
                        if (receiptUI.receiptModel.Data.ReceiptType() !== TypeEnum.RECEIPT) {
                            receiptUI.enableReceiptType(false);
                        }
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        receiptUI.receiptModel.Data.PostingDate(postingDate);
                    }
                    receiptUI.updateFiscalYearPeriod(result);
                },
                // Click on No
                function () {
                    if (receiptUI.controlToBeFocused === "#txtReceiptDate") {
                        receiptUI.receiptModel.Data.ReceiptDate(receiptUI.previousReceiptDate);
                    }
                    if (receiptUI.controlToBeFocused === "#txtPostingDate") {
                        receiptUI.receiptModel.Data.PostingDate(receiptUI.previousPostingDate);
                    }
                },
                result.UserMessage.Message);
        } else {
            if (receiptUI.dateChangeBy === DateChangedEnum.receiptDate) {
                receiptUI.isReceiptDateModified = true;
                receiptUI.receiptModel.Data.PostingDate(receiptDate);
                receiptUI.receiptModel.Data.RateDate(receiptDate);
                receiptUI.setExchangeRateValue();
                if (receiptUI.receiptModel.Data.ReceiptType() !== TypeEnum.RECEIPT) {
                    receiptUI.enableReceiptType(false);
                }
            }
            receiptUI.updateFiscalYearPeriod(result);
        }
    },

    /**
     * @function
     * @name displayResult
     * @description Display the results
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     * @param {number} uiMode The UI mode specifier
     */
    displayResult: function (result, uiMode) {
        if (result) {
            if (!receiptUI.hasKoBindingApplied) {
                receiptUI.receiptModel = ko.mapping.fromJS(result);
                receiptObservableExtension(receiptUI.receiptModel, uiMode);
                receiptUI.receiptModel.UIMode(uiMode);
                receiptUI.hasKoBindingApplied = true;
                receiptUI.receiptModel.isModelDirty = new ko.dirtyFlag(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
                window.ko.applyBindings(receiptUI.receiptModel);
            } else {
                ko.mapping.fromJS(result, receiptUI.receiptModel);
                // Get new grid data
                let grid = $("#receiptGrid").data("kendoGrid");
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
            sg.utls.showMessage(result);
            if (uiMode !== sg.utls.OperationMode.NEW) {
                receiptUI.receiptModel.isModelDirty.reset();
            }
            receiptUISuccess.updateGridButtonState();

            // Binding custom grid button click event after load
            setTimeout(function () {
                $("input.icon").click(function (e) {
                    //sg.utls.showMessageInfo(sg.utls.msgType.INFO, "First button click");
                });

                $("#btnAA").click(function (e) {
                    sg.utls.showMessageInfo(sg.utls.msgType.WARNING, "Second button click");
                });

                $("#btnBB").click(function (e) {
                    sg.utls.showMessageInfo(sg.utls.msgType.INFO, "Third button click");
                });
            }, 500);

            //sg.viewList.readOnly("receiptGrid", true);
        }
    },

    /**
     * @function
     * @name initialLoad
     * @description The initial load event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    initialLoad: function (result) {
        if (result) {
            receiptUISuccess.displayResult(result);
        }
        sg.controls.Focus($("#txtReceiptNumber"));
    },

    /**
     * @function
     * @name receiptNumberFinderSuccess
     * @description Receipt number finder success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} data The finder data
     */
    receiptNumberfinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUI.isFromReceiptFInder = true;
            receiptUI.checkIsDirty(receiptUISuccess.setReceiptNumberFinderData);
        }
    },

    /**
     * @function
     * @name setReceiptNumberFinderData
     * @description Set the Receipt number finder data
     * @namespace receiptUISuccess
     * @public
     */
    setReceiptNumberFinderData: function () {
        sg.utls.clearValidations("frmReceipt");
        receiptUI.receiptModel.Data.ReceiptNumber(receiptUI.finderData.ReceiptNumber);
        receiptUI.get(receiptUI.finderData.ReceiptNumber, receiptUI.finderData.SequenceNumber);
    },

    /**
     * @function
     * @name rateTypeFinderSuccess
     * @description Rate type finder success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} data The finder data
     */
    rateTypeFinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUISuccess.setRateTypeFinderData();
        }
    },

    /**
     * @function
     * @name setRateTypeFinderData
     * @description Set the rate type finder data
     * @namespace receiptUISuccess
     * @public
     */
    setRateTypeFinderData: function () {
        if (receiptUI.finderData.RateType) {
            receiptUI.receiptModel.Data.RateType(receiptUI.finderData.RateType);
            receiptUI.isWrongRateType = true;
            receiptUI.receiptModel.Data.ExchangeRate(1);
            let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
            receiptRepository.refresh(data);
        }
    },

    /**
     * @function
     * @name currencyRateFinderSuccess
     * @description Currency rate finder success event handler
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} data The finder data
     */
    currencyRateFinderSuccess: function (data) {
        if (data) {
            receiptUI.finderData = data;
            receiptUISuccess.setCurrencyRateFinderData();
        }
    },

    /**
     * @function
     * @name setCurrencyRateFinderData
     * @description Set the currently rate finder data
     * @namespace receiptUISuccess
     * @public
     */
    setCurrencyRateFinderData: function () {
        receiptUI.receiptModel.Data.ExchangeRate(receiptUI.finderData.Rate);
        if (receiptUI.receiptModel.Data.ReceiptType() === TypeEnum.RECEIPT) {
            receiptUI.receiptModel.Data.RateDate(receiptUI.finderData.RateDate);
        }
        let grid = $('#receiptGrid').data("kendoGrid");
        if (grid.dataSource.data().length > 0) {
            receiptUI.isWrongexchangeRate = true;
            let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
            receiptRepository.refresh(data);
        }
    },

    /**
     * @function
     * @name vendorResult
     * @description Set some vendor related fields
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
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
        let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
        receiptRepository.refresh(data);
    },

    /**
     * @function
     * @name receiptCurrencyResult
     * @description Set some receipt currency results
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    receiptCurrencyResult: function (result) {
        if (result) {
            if (result.Data && result.Data.length) {
                let description = result && result.Data.length ? result.Data[0].Description : "";
                let currencyCodeId = result && result.Data.length ? result.Data[0].CurrencyCodeId : "";
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

    /**
     * @function
     * @name costCurrencyResult
     * @description Set cost currency results
     * @namespace receiptUISuccess
     * @public
     *
     * @param {object} result JSON result payload
     */
    costCurrencyResult: function (result) {
        if (result) {
            if (result.Data && result.Data.length > 0) {
                let data = result.Data[0];
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

    /**
     * @function
     * @name updateGridButtonState
     * @description 
     * @namespace receiptUISuccess
     * @public
     */
    updateGridButtonState: function () {
        let rptType = receiptUI.receiptModel.Data.ReceiptType();
        sg.viewList.allowInsert("receiptGrid", rptType === TypeEnum.RECEIPT);
        sg.viewList.allowDelete("receiptGrid", rptType === TypeEnum.RECEIPT);
    },

    /**
     * @function
     * @name refresh
     * @description 
     * @namespace receiptUISuccess
     * @public
     * 
     * @param {object} result JSON result payload
     */
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
            let details = ko.mapping.toJS(receiptUI.receiptModel.Data.ReceiptDetail);
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

/*
 * General grid related utilities
 */
let receiptGridUtility = {
    isCellEditable: true,
    isDataRefreshInProgress: false,
    selectedIndex: 0,

    /**
     * @function
     * @name updateNumericTextBox
     * @description Update numeric textbox based on field id
     * @namespace receiptGridUtility
     * @public
     * 
     * @param {string} id The field id from the DOM
     * @param {number} value The value
     */
    updateNumericTextBox: function (id, value) {
        let numericTextbox = $("#" + id).data("kendoNumericTextBox");
        let data = receiptUI.receiptModel.Data;
        let decimal = data.AdditionalCostCurrency() === data.ReceiptCurrency() ? data.ReceiptCurrencyDecimals() : receiptUI.receiptModel.FuncDecimals();
        decimal = id === "txtTotalCost" ? data.TotalCostReceiptAdditionalDecimal() : decimal;
        decimal = id === "txtTotalReturnCost" ? data.ReceiptCurrencyDecimals() : decimal;
        decimal = id === "txtTotalAdjustmentCost" ? data.TotalCostReceiptAdditionalDecimal() : decimal;
        value = value || 0;

        if (numericTextbox) {
            let symbol = kendo.culture().numberFormat['.'];
            let length = 13;
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

    /**
     * @function
     * @name updateTextBox
     * @description Update some textboxes
     * @namespace receiptGridUtility
     * @public
     */
    updateTextBox: function () {
        let data = receiptUI.receiptModel.Data;
        let util = receiptGridUtility;
        util.updateNumericTextBox("txtAddlCost", data.AdditionalCost());
        util.updateNumericTextBox("txtpopupExchangeRate", data.ExchangeRate());
        util.updateNumericTextBox("Data_ExchangeRate", data.ExchangeRate());
        util.updateNumericTextBox("txtTotalCost", data.TotalCostReceiptAdditional());
        util.updateNumericTextBox("txtTotalReturnCost", data.TotalReturnCost());
        util.updateNumericTextBox("txtTotalAdjustmentCost", data.TotalAdjCostReceiptAddl());
    },

    /**
     * @function
     * @name getCurrentRowCell
     * @description Get the current grid row cell
     * @namespace receiptGridUtility
     * @public
     * 
     * @param {number} colIndex The column index
     */
    getCurrentRowCell: function (colIndex) {
        let grid = $('#receiptGrid').data("kendoGrid");
        let dataRows = grid.items();
        let index = dataRows.index(grid.select());
        return grid.tbody.find(">tr:eq(" + index + ") >td:eq(" + colIndex + ")");
    }
};

/*
 * This code block is executed after the rest of the DOM has finished loading
 */
$(function () {
    receiptUI.init();

    /*
     * Hook into the 'beforeunload' browser event
     */
    $(window).on('beforeunload', function () {
        let gridDirty = sg.viewList.dirty("receiptGrid");
        let model = receiptUI.receiptModel;
        let modelDirty = model.isModelDirty.isDirty();
        let dirty = (gridDirty || modelDirty) && !model.DisableScreen();

        if (sg.utls.isPageUnloadEventEnabled(dirty)) {
            return sg.utls.getDirtyMessage(receiptResources.Receipts);
        }
    });

    /*
     * Hook into the 'unload' browser event
     */
    $(window).on('unload', function () {
        sg.utls.destroySession();
    });
});

