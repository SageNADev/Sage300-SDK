// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

/*jshint -W097 */
/*global ko*/
/*global TaxAuthoritiesViewModel*/
/*global taxAuthoritiesRepository*/
/*global taxAuthoritiesResources*/
/*global globalResource*/
/*global taxAuthoritiesObservableExtension*/

// @ts-check

"use strict";

var modelData;
var taxAuthoritiesUI = taxAuthoritiesUI || {};
taxAuthoritiesUI = {
    taxAuthoritiesModel: {},
    ignoreIsDirtyProperties: ["TaxAuthority", "LastMaintainedString", "UIMode"],
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,
    taxAuthority: null,
    acctDescName: "",
    acctDescId: "",

    /**
     * @function
     * @name init
     * @description Primary initialization routine
     * @namespace taxAuthoritiesUI
     * @public
     */
    init: function () {
        taxAuthoritiesUI.initButtons();
        taxAuthoritiesUI.initFinders();
        taxAuthoritiesUI.initTextbox();
        taxAuthoritiesUISuccess.initialLoad(TaxAuthoritiesViewModel);
        taxAuthoritiesUI.initNumericTextBox();
        taxAuthoritiesUI.initDropDownList();
        taxAuthoritiesUI.initCheckBoxes();
        taxAuthoritiesUI.disableControls();

        $("#taxAuthoritiestabstrip").kendoTabStrip({
            animation: {
                open: {
                    effects: "fadeIn"
                }
            }
        });
        sg.utls.kndoUI.selectTab("taxAuthoritiestabstrip", "tabProfile");
        taxAuthoritiesUISuccess.setkey();
    },

    /**
     * @function
     * @name save
     * @description Invoke tax authority add or update functionality
     * @namespace taxAuthoritiesUI
     * @public
     */
    saveTaxAuthorities: function () {
        if ($("#frmTaxAuthorities").valid()) {
            let data = sg.utls.ko.toJS(modelData, taxAuthoritiesUI.computedProperties);
            if (modelData.UIMode() === sg.utls.OperationMode.SAVE) {
                taxAuthoritiesRepository.update(data, taxAuthoritiesUISuccess.update);
            } else {
                taxAuthoritiesRepository.add(data, taxAuthoritiesUISuccess.update);
            }
        }
    },

    /**
     * @function
     * @name initButtons
     * @description Initialize the buttons
     * @namespace taxAuthoritiesUI
     * @public
     */
    initButtons: function () {

        // Key field with Finder
        $("#txtTaxAuthority").on('blur', function () {
            modelData.TaxAuthority($("#txtTaxAuthority").val());
            sg.delayOnBlur("btnFinderTaxAuthority", function () {
                if (sg.controls.GetString(modelData.TaxAuthority() !== "")) {
                    if (sg.controls.GetString(taxAuthoritiesUI.taxAuthority) !== sg.controls.GetString(modelData.TaxAuthority())) {
                        taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUI.get, taxAuthoritiesUI.taxAuthority);
                    }
                }
            });
        });

        // Create New Button
        $("#btnNew").on('click', function () {
            taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUI.create, taxAuthoritiesUI.taxAuthority);
        });

        // Save Button
        $("#btnSave").on('click', function () {
            sg.utls.SyncExecute(taxAuthoritiesUI.saveTaxAuthorities);
        });

        // Delete Button
        $("#btnDelete").on('click', function () {
            if ($("#frmTaxAuthorities").valid()) {
                let message = jQuery.validator.format(taxAuthoritiesResources.DeleteConfirmationMessage, taxAuthoritiesResources.TaxAuthorityTitle, modelData.TaxAuthority());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmTaxAuthorities");
                    taxAuthoritiesRepository.delete(modelData.TaxAuthority(), taxAuthoritiesUISuccess.delete);
                }, null, message, taxAuthoritiesResources.DeleteTitle);
            }
        });

    },

    /**
     * @function
     * @name initTextbox
     * @description Initialize the textboxes
     * @namespace taxAuthoritiesUI
     * @public
     */
    initTextbox: function(){
        $("#txtRecoRate").val(100);
        $('#txtTaxAuthority').on("blur", function () {
            sg.delayOnBlur("btntaxAuthorityFinder", function () {
                if ($('#txtTaxAuthority').val() !== "") {
                    sg.controls.Focus($("#txtTaxDescription"));
                }
            });
        });

        $('#txtTaxReportingCurrency').on('change', function () {
            let currencyCode = $('#txtTaxReportingCurrency').val();
            sg.delayOnBlur("btnCurrencyFinder", function () {
                if (currencyCode){
                    currencyCode = currencyCode.toUpperCase();
                    taxAuthoritiesRepository.getCurrencyDescription({ id: currencyCode }, taxAuthoritiesUISuccess.displayCurrencyDescription);
                } else {
                    $('#txtTaxReportingCurrencydesc').val("");
                }
            });
        });

        $('#txtTaxRecoverable').on("change", function () {
            let accountNumber = $('#txtTaxRecoverable').val();
            taxAuthoritiesUI.acctDescName = "RecoverableTaxAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtTaxRecoverable";
            sg.delayOnChange("btnTaxRecoverableFinder", $('#txtTaxRecoverable'), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });

        $('#txtTaxLiabilityAccount').on("change", function () {
            let accountNumber = $('#txtTaxLiabilityAccount').val();
            taxAuthoritiesUI.acctDescName = "LiabilityAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtTaxLiabilityAccount";
            sg.delayOnChange("btnLiabilityAccountFinder", $("#txtTaxLiabilityAccount"), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });

        $("#txtExpenseAccount").on("change", function () {
            let accountNumber = $("#txtExpenseAccount").val();
            taxAuthoritiesUI.acctDescName = "ExpenseAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtExpenseAccount";
            sg.delayOnChange("btnExpenseAccountFinder", $("#txtExpenseAccount"), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });
    },

    /**
     * @function
     * @name initTextBoxValue
     * @description Initialize the numeric textboxes
     * @namespace taxAuthoritiesUI
     * @public
     * 
     * @param {string] id The textbox identifier string
     * @param {number} value The value to set in the textbox
     */
    initTextBoxValue: function (id, value) {
        let numerictextbox = $('#' + id).data("kendoNumericTextBox");
        numerictextbox.value(value);
    },

    /**
     * @function
     * @name numericBoxChange
     * @description Event handler for textbox changes
     * @namespace taxAuthoritiesUI
     * @public
     *
     * @param {object} e The event object
     * @param {string} id The textbox identifier string
     */
    numericBoxChange: function (e, id) {
        let value = e.sender._value;
        if (value) {
            let expression = parseInt(value);
            $(this).val(expression.toString());
        } else {
            taxAuthoritiesUI.initTextBoxValue(id, 0);
        }
    },

    /**
     * @function
     * @name initNumericTextBox
     * @description Initialize the numeric textboxes
     * @namespace taxAuthoritiesUI
     * @public
     */
    initNumericTextBox: function () {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        let curdecimal = vm.CurrencyDecimals();
        let beforedecimal = 13;
        let maxAllow = Array(beforedecimal + 1).join("9");

        $("#txtMaximumTaxAllowable").val(maxAllow);
        vm.Data.MaximumTaxAllowable(maxAllow);

        $("#txtMaximumTaxAllowable").kendoNumericTextBox({
            format: "n" + curdecimal,
            spinners: false,
            step: 0,
            min: 0,
            decimals: beforedecimal,
            change: function (e) {
                taxAuthoritiesUI.numericBoxChange(e, "txtMaximumTaxAllowable");
            }
        });

        let maximumTax = $("#txtMaximumTaxAllowable").data("kendoNumericTextBox");
        $(maximumTax.element).unbind("input");
        sg.utls.kndoUI.restrictDecimals(maximumTax, curdecimal, beforedecimal);

        $("#txtNoTaxChargedBelow").kendoNumericTextBox({
            format: "n" + curdecimal,
            step: 0,
            spinners: false,
            min: 0,
            decimals: beforedecimal,
            change: function (e) {
                taxAuthoritiesUI.numericBoxChange(e, "txtNoTaxChargedBelow");
            }
        });

        let NochargeTxtBox = $("#txtNoTaxChargedBelow").data("kendoNumericTextBox");
        $(NochargeTxtBox.element).unbind("input");
        sg.utls.kndoUI.restrictDecimals(NochargeTxtBox, curdecimal, beforedecimal);

        $("#txtRecoRate").kendoNumericTextBox({
            format: "n5",
            step: 0,
            spinners: false,
            min: 0,
            decimals: 5,
            change: function (e) {
                taxAuthoritiesUI.numericBoxChange(e, "txtRecoRate");
            }
        });
        let RecoverRateTxtBox = $("#txtRecoRate").data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(RecoverRateTxtBox, 5, 3);

    },

    /**
     * @function
     * @name initDropDownList
     * @description Initialize the dropdown listboxes
     * @namespace taxAuthoritiesUI
     * @public
     */
    initDropDownList: function () {
        sg.utls.kndoUI.dropDownList("ddlreportTaxRetainage");
        sg.utls.kndoUI.dropDownList("ddltaxBase");
        sg.utls.kndoUI.dropDownList("ddlreportLevel");
        let data = taxAuthoritiesUI.taxAuthoritiesModel.Data;

        $("#ddlreportTaxRetainage").on('change', function () {
            let selIndex = $('#ddlreportTaxRetainage').data("kendoDropDownList").selectedIndex;
            $("#txtMaximumTaxAllowable").data("kendoNumericTextBox").enable(selIndex === 0);
            $("#txtNoTaxChargedBelow").data("kendoNumericTextBox").enable(selIndex === 0);
            data.ReportTaxonRetainageDocument(selIndex);
        });

        $("#ddltaxBase").on('change', function () {
            let selIndex = $('#ddltaxBase').data("kendoDropDownList").value();
            data.TaxBase(selIndex);
        });
        $("#ddlreportLevel").on('change', function () {
            let selIndex = $('#ddlreportLevel').data("kendoDropDownList").value();
            data.ReportLevel(selIndex);
        });
    },

    /**
     * @function
     * @name initFinders
     * @description Initialize the finders
     * @namespace taxAuthoritiesUI
     * @public
     */
    initFinders: function () {
        let info = sg.viewFinderProperties.TX.TaxAuthorities;
        let buttonId = "btnFinderTaxAuthority";
        let dataControlIdOrSuccessCallback = onFinderSuccess.taxAuthority;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, taxAuthoritiesFilter.getFilter);

        info = sg.viewFinderProperties.CS.CurrencyCodes;
        buttonId = "btnCurrencyFinder";
        dataControlIdOrSuccessCallback = onFinderSuccess.currencyCode;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info);

        info = sg.viewFinderProperties.GL.Accounts;
        buttonId = "btnLiabilityAccountFinder";
        dataControlIdOrSuccessCallback = onFinderSuccess.liabilityAccount;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info);

        info = sg.viewFinderProperties.GL.Accounts;
        buttonId = "btnExpenseAccountFinder";
        dataControlIdOrSuccessCallback = onFinderSuccess.expenseAccount;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info);

        info = sg.viewFinderProperties.GL.Accounts;
        buttonId = "btnTaxRecoverableFinder";
        dataControlIdOrSuccessCallback = onFinderSuccess.recoverableAccount;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info);
    },

    /**
     * @function
     * @name initCheckBoxes
     * @description Set up the change events for check boxes
     * @namespace taxAuthoritiesUI
     * @public
     */
    initCheckBoxes: function () {
        $("#chktaxRecoverable").on("change", function () {
            if (!this.checked) {
                taxAuthoritiesUI.taxAuthoritiesModel.Data.RecoverableTaxAccount("");
                taxAuthoritiesUI.taxAuthoritiesModel.RecoverableTaxAccountDescription("");
                $("#txtRecoRate").data("kendoNumericTextBox").value("100");
            }
            taxAuthoritiesUI.recochk = this.checked;
            $("#txtRecoRate").data("kendoNumericTextBox").enable(this.checked);
            $("#txtTaxRecoverable").enable(this.checked);
            $("#btnLoadrecoverable").enable(this.checked);
            $("#btnTaxRecoverableFinder").enable(this.checked);
        });

        $("#chkExpenseSeparately").on("change", function () {
            if (!this.checked) {
                taxAuthoritiesUI.taxAuthoritiesModel.Data.ExpenseAccount("");
                taxAuthoritiesUI.taxAuthoritiesModel.ExpenseAccountDescription("");
            }
            taxAuthoritiesUI.expnsechk = this.checked;
            $("#txtExpenseAccount").enable(this.checked);
            $("#btnLoadexpense").enable(this.checked);
            $("#btnExpenseAccountFinder").enable(this.checked);

        });

        $("#chkallowTax").on("change", function () {
            taxAuthoritiesUI.allowTaxInPricechk = this.checked;
        });
    },
    
    /**
     * @function
     * @name disableControls
     * @description Disable a select group of controls
     * @namespace taxAuthoritiesUI
     * @public
     */
    disableControls: function () {
        $("#txtTaxRecoverable").enable(false);
        $("#btnLoadrecoverable").enable(false);
        $("#txtRecoTxAcctDesc").enable(false);
        $("#Data.RecoverableRate").enable(false);
        $("#btnTaxRecoverableFinder").enable(false);
        $("#txtExpenseAccount").enable(false);
        $("#btnExpenseAccountFinder").enable(false);
        $("#txtExpAcctDesc").enable(false);
        $("#btnLoadexpense").enable(false);
    },

    /**
     * @function
     * @name get
     * @description Invoke the tax authority get functionality
     * @namespace taxAuthoritiesUI
     * @public
     */
    get: function () {
        taxAuthoritiesRepository.get(modelData.TaxAuthority(), taxAuthoritiesUISuccess.get);
    },

    /**
     * @function
     * @name create
     * @description Invoke the tax authority create functionality
     * @namespace taxAuthoritiesUI
     * @public
     */
    create: function () {
        sg.utls.clearValidations("frmTaxAuthorities");
        taxAuthoritiesRepository.create(taxAuthoritiesUISuccess.create);
    },

    /**
     * @function
     * @name checkIsDirty
     * @description Check if model as been changed. If it has, display a confirmation dialog box.
     *              and invoke the specified callback function if the user selects 'Yes'
     * @namespace taxAuthoritiesUI
     * @public 
     * 
     * @param {Function} functionToCall Callback function 
     * @param {string} taxAuthority The tax authority specification
     */
    checkIsDirty: function (functionToCall, taxAuthority) {
        if (taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.isDirty() && taxAuthority) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmTaxAuthorities");
                    functionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(taxAuthority) !== sg.controls.GetString(modelData.TaxAuthority())) {
                        modelData.TaxAuthority(taxAuthority);
                   }
                   return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, taxAuthoritiesResources.TaxAuthorityTitle, taxAuthority));
        } else {
            functionToCall.call();
        }
    }

};

// Callbacks
var taxAuthoritiesUISuccess = {

    /**
     * @function
     * @name setkey
     * @description Set the tax authority key
     * @namespace taxAuthoritiesUISuccess
     * @public
     */
    setkey: function () {
        taxAuthoritiesUI.taxAuthority = modelData.TaxAuthority();
    },

    /**
     * @function
     * @name get
     * @description Event handler for successful tax authority get 
     * @namespace taxAuthoritiesUISuccess
     * @public
     * 
     * @param {object} jsonResult JSON payload object
     */
    get: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data) {
                taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
                taxAuthoritiesUISuccess.enableCurrency(false);
            } else {
                modelData.UIMode(sg.utls.OperationMode.NEW);
                modelData.Description("");
                taxAuthoritiesUISuccess.enableCurrency(true);
            }
            taxAuthoritiesUISuccess.setkey();
        }
    },

    /**
     * @function
     * @name update
     * @description Event handler for successful tax authority update
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} jsonResult JSON payload object
     */
    update: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            taxAuthoritiesUISuccess.setkey();
            taxAuthoritiesUISuccess.enableCurrency(false);
        }
        sg.utls.showMessage(jsonResult);
    },

    /**
     * @function
     * @name create
     * @description Event handler for successful tax authority create
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} jsonResult JSON payload object
     */
    create: function (jsonResult) {
        taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.reset();
        taxAuthoritiesUISuccess.setkey();
        taxAuthoritiesUISuccess.enableCurrency(true);
        sg.controls.Focus($("#txtTaxAuthority"));
    },

    /**
     * @function
     * @name delete
     * @description Event handler for successful tax authority deletion
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} jsonResult JSON payload object
     */
    delete: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.reset();
            taxAuthoritiesUISuccess.enableCurrency(true);
            taxAuthoritiesUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    /**
     * @function
     * @name displayResult
     * @description Display the results of an ajax call
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} jsonResult JSON payload object
     * @param {number} uiMode The UI mode
     */
    displayResult: function (jsonResult, uiMode) {
        if (jsonResult) {
            if (!taxAuthoritiesUI.hasKoBindingApplied) {
                taxAuthoritiesUI.taxAuthoritiesModel = ko.mapping.fromJS(jsonResult);
                taxAuthoritiesUI.hasKoBindingApplied = true;
                modelData = taxAuthoritiesUI.taxAuthoritiesModel.Data;
                taxAuthoritiesObservableExtension(taxAuthoritiesUI.taxAuthoritiesModel, uiMode);
                taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty = new ko.dirtyFlag(modelData, taxAuthoritiesUI.ignoreIsDirtyProperties);
                ko.applyBindings(taxAuthoritiesUI.taxAuthoritiesModel);
            } else {
                ko.mapping.fromJS(jsonResult, taxAuthoritiesUI.taxAuthoritiesModel);
                modelData.UIMode(uiMode);
                if (uiMode !== sg.utls.OperationMode.NEW) {
                    taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.reset();
                } 
            }

            if (!taxAuthoritiesUI.isKendoControlNotInitialised) {
                taxAuthoritiesUI.isKendoControlNotInitialised = true;
            } else {
                let data = taxAuthoritiesUI.taxAuthoritiesModel.Data;
                let selIndex = data.ReportTaxonRetainageDocument();
                $("#ddlreportTaxRetainage").data("kendoDropDownList").value(selIndex);
                $("#ddltaxBase").data("kendoDropDownList").value(data.TaxBase());
                $("#ddlreportLevel").data("kendoDropDownList").value(data.ReportLevel());

                $("#chktaxRecoverable").prop("checked", data.TaxRecover()).applyCheckboxStyle();
                $("#chkExpenseSeparately").prop("checked", data.ExpenseSeparate()).applyCheckboxStyle();
                $("#chkallowTax").prop("checked", data.AllowTaxPrice()).applyCheckboxStyle();

                let maxAllow = (uiMode === sg.utls.OperationMode.NEW) ? "9999999999999" : data.MaximumTaxAllowable();
                $("#txtMaximumTaxAllowable").data("kendoNumericTextBox").value(maxAllow);
                $("#txtNoTaxChargedBelow").data("kendoNumericTextBox").value(data.NoTaxChargedBelow());
                $("#txtRecoRate").data("kendoNumericTextBox").value(data.RecoverableRate());

                if ($("#chktaxRecoverable").is(":checked")) {
                    $("#txtRecoRate").data("kendoNumericTextBox").enable(true);
                    $("#txtTaxRecoverable").enable(true);
                    $("#btnLoadrecoverable").enable(true);
                    $("#btnTaxRecoverableFinder").enable(true);
                }
                if ($("#chkExpenseSeparately").is(":checked")) {
                    $("#txtExpenseAccount").enable(true);
                    $("#btnLoadexpense").enable(true);
                    $("#btnExpenseAccountFinder").enable(true);
                }

                $("#txtMaximumTaxAllowable").data("kendoNumericTextBox").enable(selIndex === 0);
                $("#txtNoTaxChargedBelow").data("kendoNumericTextBox").enable(selIndex === 0);

                let isTaxRecoverable = $("#chktaxRecoverable").prop("checked", data.TaxRecover()).applyCheckboxStyle()[0].checked;
                $("#txtTaxRecoverable").enable(isTaxRecoverable);
                $("#btnLoadrecoverable").enable(isTaxRecoverable);
                $("#btnTaxRecoverableFinder").enable(isTaxRecoverable);
                $("#txtRecoRate").data("kendoNumericTextBox").enable(isTaxRecoverable);

                let isExpenseAccount = $("#chkExpenseSeparately").prop("checked", data.ExpenseSeparate()).applyCheckboxStyle()[0].checked;
                $("#txtExpenseAccount").enable(isExpenseAccount);
                $("#btnLoadexpense").enable(isExpenseAccount);
                $("#btnExpenseAccountFinder").enable(isExpenseAccount);
            }
        }
    },

    /**
     * @function
     * @name initialLoad
     * @description Called during initial page load
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} result JSON payload object
     */
    initialLoad: function (result) {
        if (result) {
            taxAuthoritiesUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, taxAuthoritiesResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtTaxAuthority"));
    },

    /**
     * @function
     * @name setFinderData
     * @description Set the finder data
     * @namespace taxAuthoritiesUISuccess
     * @public
     */
    setFinderData: function () {
        let authority = taxAuthoritiesUI.finderData.AUTHORITY;
        sg.utls.clearValidations("frmTaxAuthorities");
        taxAuthoritiesUI.finderData = null;
        taxAuthoritiesRepository.get(authority, taxAuthoritiesUISuccess.get);
    },

    /**
     * @function
     * @name isNew
     * @description Determine if we're creating a new tax authority
     * @namespace taxAuthoritiesUISuccess
     * @public
     */
    isNew: function (model) {
        if (!model.TaxAuthority()) {
           return true;
        }
        return false;
    },

    /**
     * @function
     * @name displayAccountDescription
     * @description Display the account description message
     * @namespace taxAuthoritiesUISuccess
     * @public
     * 
     * @param {object} result JSON payload object
     */
    displayAccountDescription: function (result) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (typeof result === "string" || result instanceof String) {
            vm[taxAuthoritiesUI.acctDescName](result);
        }
        else {
            vm[taxAuthoritiesUI.acctDescName]("");
            sg.controls.Focus($('#'+ taxAuthoritiesUI.acctDescId));
        }
        sg.utls.showMessage(result);
    },

    /**
     * @function
     * @name displayCurrencyDescription
     * @description Display the currency description message
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {object} result JSON payload object
     */
    displayCurrencyDescription: function (result) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (typeof result === "string" || result instanceof String) {
            vm.CurrencyDescription(result);
        }
        else {
            vm.CurrencyDescription("");
            sg.controls.Focus($('#txtTaxReportingCurrency'));
        }
        sg.utls.showMessage(result);
    },

    /**
     * @function
     * @name enableCurrency
     * @description Display the currency description message
     * @namespace taxAuthoritiesUISuccess
     * @public
     *
     * @param {boolean} enable true = enable | false = disable
     */
    enableCurrency: function (enable) {
        $("#txtTaxReportingCurrency").enable(enable);
        $("#btnLoadcurrency").enable(enable);
        $("#btnCurrencyFinder").enable(enable);
    }
};

// Finder Filter
var taxAuthoritiesFilter = {
    /**
     * @function
     * @name getFilter
     * @description Get the finder filter
     * @namespace taxAuthoritiesFilter
     * @public
     */
    getFilter: function () {
        let filters = [[]];
        let taxAuthoritiesName = $("#txtTaxAuthority").val();
        filters[0][0] = sg.finderHelper.createFilter("TaxAuthority", sg.finderOperator.StartsWith, taxAuthoritiesName);
        return filters;
    }
};

var onFinderSuccess = {

    /**
     * @function
     * @name taxAuthority
     * @description 
     * @namespace onFinderSuccess
     * @public
     * 
     * @param {object} data JSON payload object  
     */
    taxAuthority: function (data) {
        if (data) {
            if (sg.controls.GetString(taxAuthoritiesUI.taxAuthority) !== data.AUTHORITY) {
                taxAuthoritiesUI.finderData = data;
                taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUISuccess.setFinderData, taxAuthoritiesUI.taxAuthority);
            }
        }
    },

    /**
     * @function
     * @name currencyCode
     * @description Event handler for currency code finder
     *              This method will get selected currency id and name,
     *              set the appropriate model fields and set the form focus
     * @namespace onFinderSuccess
     * @public
     *
     * @param {object} data JSON payload object
     */
    currencyCode: function (data) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.TaxReportingCurrency(data.CURID);
            vm.CurrencyDescription(data.CURNAME);
            sg.controls.Focus($("#txtTaxReportingCurrency"));
        }
    },

    /**
     * @function
     * @name currencyCode
     * @description Event handler for liability account finder
     *              This method will get selected account id and description
     *              set the appropriate model fields and set the form focus
     * @namespace onFinderSuccess
     * @public
     *
     * @param {object} data JSON payload object
     */
    liabilityAccount: function (data) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.TaxLiabilityAccount(data.ACCTFMTTD);
            vm.LiabilityAccountDescription(data.ACCTDESC);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Liabilityconstant;
        }
    },

    /**
     * @function
     * @name expenseAccount
     * @description Event handler for expense account finder
     *              This method will get selected account id and description
     *              set the appropriate model fields and set the form focus
     * @namespace onFinderSuccess
     * @public
     *
     * @param {object} data JSON payload object
     */
    expenseAccount: function (data) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.ExpenseAccount(data.ACCTFMTTD);
            vm.ExpenseAccountDescription(data.ACCTDESC);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Expconstant;
        }
    },

    /**
     * @function
     * @name recoverableAccount
     * @description Event handler for recoverable account finder
     *              This method will get selected account id and description
     *              set the appropriate model fields and set the form focus
     * @namespace onFinderSuccess
     * @public
     *
     * @param {object} data JSON payload object
     */
    recoverableAccount: function (data) {
        let vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.RecoverableTaxAccount(data.ACCTFMTTD);
            vm.RecoverableTaxAccountDescription(data.ACCTDESC);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Recoconstant;
        }
    },
};
 
// Initial Entry
$(function () {
    taxAuthoritiesUI.init();

    $(window).on('beforeunload', function () {
        let dirty = taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.isDirty();
        if (sg.utls.isPageUnloadEventEnabled(dirty)) {
            return sg.utls.getDirtyMessage(taxAuthoritiesResources.TaxAuthorityTitle);
        }
    });
});
