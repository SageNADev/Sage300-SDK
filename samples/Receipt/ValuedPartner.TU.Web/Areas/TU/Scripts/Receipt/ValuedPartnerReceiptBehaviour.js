// The MIT License (MIT) 
// Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved.
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

const HeaderFieldsEnum = {
    ReceiptType: 8,
    AdditionalCost: 16,
    RequireLabels: 26,
    ReceiptCurrency: 11,
    AdditionalCostCurrency: 19,
    VendorNumber: 10,
    ExchangeRate: 12,
    RateType: 13
};

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
     * @public
     *
     * @param {any} colName TODO - Add parameter description
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
     * @public
     *
     * @param {any} intTime TODO - Add parameter description
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} e TODO - Add parameter description
     * @param {any} column TODO - Add parameter description
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
     * @param {any} e TODO - Add parameter description
     * @param {any} column TODO - Add parameter description
     */
    showGridCommentColumn: function (e, column) {
        column.Template = '#:receiptUI.showCommentBasedOnItem(data.COMMENTS, data.ITEMNO)#';
    },

    /**
     * @function
     * @name showCommentBasedOnItem
     * @description Show comments column based on item value. If item value is start 'A', 
     *              show comments as comment value + "The item name is starts 'A', other 
     *              wise just show empty.
     * @public
     *
     * @param {any} comments TODO - Add parameter description
     * @param {any} itemNo TODO - Add parameter description
     *  
     * @returns {string} TODO - Add return value description
     */
    showCommentBasedOnItem: function (comments, itemNo) {
        return itemNo.startsWith("A") ? comments + " The item name is starts with 'A'" : "";
    },

    /**
     * @function
     * @name updateFinderFilter
     * @description TODO - Add 
     * @public
     *
     * @param {any} record TODO - Add parameter description
     * @param {any} finder TODO - Add parameter description
     */
    updateFinderFilter: function (record, finder) {
        //finder.Filter = "RECPQTY=111";
    },

    /**
     * @function
     * @name showCustomIcon
     * @description Show detail optional field in popup window
     * @public
     *
     * @param {any} e TODO - Add parameter description
     * @param {any} column TODO - Add parameter description
     */
    showCustomIcon: function (e, column) {
        column.Template = kendo.template('<span style="padding-right:50px;">Yes</span><input class="icon pencil-edit" type="button">');
    },

    /**
     * @function
     * @name showIcons
     * @description Show detail optional field in popup window
     * @public
     *
     * @param {any} e TODO - Add parameter description
     * @param {any} column TODO - Add parameter description
     */
    showIcons: function (e, column) {
        // Data is current record, such data.VENDNAME
        column.Template = kendo.template('<input class="icon pencil-edit" type="button"><input value="AA" id="btnAA" type="button"><input value="BB" id="btnBB" type="button">');
    },

    /**
     * @function
     * @name ShowDetailOptionalField
     * @description Show detail optional field in popup window
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
     * @name showOptonalField
     * @description Show optional field in popup window
     * @public
     */
    showOptionalField: function () {
        let isReadOnly = receiptUI.receiptModel.IsDisableOnlyComplete();
        sg.optionalFieldControl.showPopUp("rptOptionalFieldGrid", "optionalField", isReadOnly);
    },

    /**
     * @function
     * @name initPopUps
     * @description TODO - Add a detailed description of this functions intent
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

    // TODO: Custom call back functions
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

    customGridAfterInsert: function (value) {
    },

    customColumnChanged: function (currentValue, value, event) {
    },

    customColumnBeforeDisplay: function (value, properties) {
    },

    customColumnDoubleClick: function (value, event) {
    },

    customColumnBeforeFinder: function (value, options) {
    },

    customColumnBeforeEdit: function (value, event, fieldName) {
        event.preventDefault();
    },

    customColumnStartEdit: function (value, editor) {
        $(editor).after('<input class="icon pencil-edit" style="margin-left:-20px;" id="btnDetailJobs" tabindex="-1" type="button" />');
        $("input.icon").click(function () {
            //sg.utls.showMessageInfo(sg.utls.msgType.INFO, "editor button click");
        });
    },

    customColumnEndEdit: function (value, editor) {
    },

    /**
     * @function
     * @name init
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @name create
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *  
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} functionToCall TODO - Add a detailed description of this parameter
     */
    checkIsDirty: function (functionToCall) {
        let modelData = receiptUI.receiptModel.Data;
        let gridDirty = sg.viewList.dirty("receiptGrid");
        let modelDirty = receiptUI.receiptModel.isModelDirty.isDirty();
        let exists = true;

        if (receiptUI.receiptModel.UIMode() === sg.utls.OperationMode.NEW && !receiptUI.isFromReceiptFInder) {
            if (modelDirty || gridDirty && receiptUI.receiptNumber) {
                let data = ko.mapping.toJS(modelData, receiptUI.ignoreIsDirtyProperties);
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
     * 
     * @returns {object} TODO - Add return value description
     */
    initGridLocationFinder: function (viewFinder) {
        // Add custom defined properties here, grid generic editor will call it
        viewFinder.calculatePageCount = false;
        return viewFinder;
    },

    /**
     * @function
     * @name initReceiptNumberFinder
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
     */
    initReceiptCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    /**
     * @function
     * @name initAddlCostCurrencyFinder
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
     */
    initAddlCostCurrencyFinder: function (viewFinder) {
        receiptUI.initCurrencyCodeFinderCommon(viewFinder);
    },

    /**
     * @function
     * @name initRateTypeFinder
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} viewFinder TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     */
    initFinders: function () {
        //let helper = sg.viewFinderHelper;
        sg.viewFinderHelper.setViewFinder("btnReceiptNumberFinder", "txtReceiptNumber", this.initReceiptNumberFinder);
        sg.viewFinderHelper.setViewFinder("btnVendorNumberFinder", "Data_VendorNumber", this.initVendorNumberFinder);
        sg.viewFinderHelper.setViewFinder("btnReceiptCurrencyFinder", "Data_ReceiptCurrency", this.initReceiptCurrencyFinder);
        sg.viewFinderHelper.setViewFinder("btnAddlCostCurrencyFinder", "Data_AdditionalCostCurrency", this.initAddlCostCurrencyFinder);
        sg.viewFinderHelper.setViewFinder("btnRateTypeFinder", "Data_RateType", this.initRateTypeFinder);
        sg.viewFinderHelper.setViewFinder("btnExchangeRateFinder", "txtpopupExchangeRate", this.initExchangeRateFinder);
    },

    /**
     * @function
     * @name initHamburgers
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} e TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} field TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} field TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} value TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} e TODO - Add a detailed description of this parameter
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
     * @description Get receipt number
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} isEnable TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     */
    setExchangeRateValue: function () {
        receiptRepository.GetExchangeRate(receiptUI.receiptModel.Data.RateType(),
            receiptUI.receiptModel.Data.ReceiptCurrency(), receiptUI.receiptModel.Data.RateDate(),
            receiptUI.receiptModel.Data.ExchangeRate(), receiptUI.receiptModel.Data.HomeCurrency());
    },

    /**
     * @function
     * @name refreshHeader
     * @description TODO - Add a detailed description of this functions intent
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
    /**
     * @function
     * @name setkey
     * @description TODO - Add a detailed description of this functions intent
     * @public
     */
    setkey: function () {
        receiptUI.receiptNumber = receiptUI.receiptModel.Data.ReceiptNumber();
    },

    /**
     * @function
     * @name onSaveDetailsCompleted
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
     */
    DeleteDetailSuccess: function (result) {
        let grid = $("#receiptGrid").data("kendoGrid");
        grid.dataSource.read();
    },

    /**
     * @function
     * @name getVendorDetailsSuccess
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
     */
    rateTypeSelect: function (result) {
        if (!result) return;
    },

    /**
     * @function
     * @name getExchangeRate
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
    getExchangeRate: function (jsonResult) {
        if (jsonResult.UserMessage.Message) {
            receiptUI.setExchangeRate(jsonResult);
        }
        if (receiptUI.isReceiptDateModified === true || receiptUI.isWrongRateType === true) {
            receiptUI.setExchangeRate(jsonResult);
        }
    },

    /**
     * @function
     * @name getRateSpread
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
    getRateSpread: function (jsonResult) {
        if (jsonResult.UserMessage.Message) {
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
                jsonResult.UserMessage.Message);
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
    getItemTypeSuccess: function (jsonResult) {
        sg.ic.utls.setItemTypeResponse(jsonResult, "#btnManufacturerItemFinder", receiptUISuccess.manufacturerItem);
    },

    /**
     * @function
     * @name getResult
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
    getResult: function (jsonResult) {
        let modelData = receiptUI.receiptModel.Data;
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            receiptUI.addLineClicked = false;
            if (jsonResult.IsExists === true) {
                receiptUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
                if (modelData.ReceiptType() === TypeEnum.RECEIPT || modelData.ReceiptType() === TypeEnum.ADJUSTMENT) {
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
        if (modelData.ReceiptType() === TypeEnum.RETURN) {
            sg.controls.disable($("#btnDetailAddLine"));
        }
    },

    /**
     * @function
     * @name actionSuccess
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} action TODO - Add a detailed description of this parameter
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
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

    /**
     * @function
     * @name getHeaderValues
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     */
    checkDate: function (jsonResult) {
        let receiptDate = sg.utls.kndoUI.getFormattedDate($("#txtReceiptDate").val());
        let postingDate = sg.utls.kndoUI.getFormattedDate($("#txtPostingDate").val());
        if (jsonResult.UserMessage.Message && jsonResult.UserMessage.IsSuccess) {
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
                    receiptUI.updateFiscalYearPeriod(jsonResult);
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
                jsonResult.UserMessage.Message);
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
            receiptUI.updateFiscalYearPeriod(jsonResult);
        }
    },

    /**
     * @function
     * @name displayResult
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} jsonResult TODO - Add a detailed description of this parameter
     * @param {any} uiMode TODO - Add a detailed description of this parameter
     */
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
            sg.utls.showMessage(jsonResult);
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} data TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} data TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     */
    setRateTypeFinderData: function () {
        if (receiptUI.finderData.RateType) {
            receiptUI.receiptModel.Data.RateType(receiptUI.finderData.RateType);
            receiptUI.isWrongRateType = true;
            receiptUI.receiptModel.Data.ExchangeRate(1);
            let data = ko.mapping.toJS(receiptUI.receiptModel.Data, receiptUI.ignoreIsDirtyProperties);
            receiptRepository.refresh(receiptUI.receiptModel.Data);
        }
    },

    /**
     * @function
     * @name currentyRateFinderSuccess
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} data TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     *
     * @param {any} result TODO - Add a detailed description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public 
     * 
     * @param {any} result
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
 * TODO - Add a description of this object
 */
let receiptGridUtility = {
    isCellEditable: true,
    isDataRefreshInProgress: false,
    selectedIndex: 0,

    /**
     * @function
     * @name updateNumericTextBox
     * @description TODO - Add a detailed description of this functions intent
     * @public
     * 
     * @param {any} id TODO - Add a description of this parameter
     * @param {any} value TODO - Add a description of this parameter
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
     * @description TODO - Add a detailed description of this functions intent
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
     * @description TODO - Add a detailed description of this functions intent
     * @public
     * 
     * @param {any} colIndex TODO - Add a detailed description of this parameter
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

