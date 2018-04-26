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

/*jshint -W097 */
/*global ko*/
/*global TaxAuthoritiesViewModel*/
/*global taxAuthoritiesRepository*/
/*global taxAuthoritiesResources*/
/*global globalResource*/
/*global taxAuthoritiesObservableExtension*/

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
    // Init
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

    // Save
    saveTaxAuthorities: function () {
        if ($("#frmTaxAuthorities").valid()) {
            var data = sg.utls.ko.toJS(modelData, taxAuthoritiesUI.computedProperties);
            if (modelData.UIMode() === sg.utls.OperationMode.SAVE) {
                taxAuthoritiesRepository.update(data, taxAuthoritiesUISuccess.update);
            } else {
                taxAuthoritiesRepository.add(data, taxAuthoritiesUISuccess.update);
            }
        }
    },

    // Init Buttons
    initButtons: function () {

        // Key field with Finder
        $("#txtTaxAuthority").bind('blur', function () {
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
        $("#btnNew").bind('click', function () {
            taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUI.create, taxAuthoritiesUI.taxAuthority);
        });

        // Save Button
        $("#btnSave").bind('click', function () {
            sg.utls.SyncExecute(taxAuthoritiesUI.saveTaxAuthorities);
        });

        // Delete Button
        $("#btnDelete").bind('click', function () {
            if ($("#frmTaxAuthorities").valid()) {
                var message = jQuery.validator.format(taxAuthoritiesResources.DeleteConfirmationMessage, taxAuthoritiesResources.TaxAuthorityTitle, modelData.TaxAuthority());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmTaxAuthorities");
                    taxAuthoritiesRepository.delete(modelData.TaxAuthority(), taxAuthoritiesUISuccess.delete);
                }, null, message, taxAuthoritiesResources.DeleteTitle);
            }
        });

    },
    // Init TextBoxs
    initTextbox: function(){
        $("#txtRecoRate").val(100);
        $('#txtTaxAuthority').bind("blur", function () {
            sg.delayOnBlur("btntaxAuthorityFinder", function () {
                if ($('#txtTaxAuthority').val() !== "") {
                    sg.controls.Focus($("#txtTaxDescription"));
                }
            });
        });

        $('#txtTaxReportingCurrency').bind('change', function () {
            var currencyCode = $('#txtTaxReportingCurrency').val();
            sg.delayOnBlur("btnCurrencyFinder", function () {
                if (currencyCode){
                    currencyCode = currencyCode.toUpperCase();
                    taxAuthoritiesRepository.getCurrencyDescription({ id: currencyCode }, taxAuthoritiesUISuccess.displayCurrencyDescription);
                } else {
                    $('#txtTaxReportingCurrencydesc').val("");
                }
            });
        });

        $('#txtTaxRecoverable').bind("change", function () {
            var accountNumber = $('#txtTaxRecoverable').val();
            taxAuthoritiesUI.acctDescName = "RecoverableTaxAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtTaxRecoverable";
            sg.delayOnChange("btnTaxRecoverableFinder", $('#txtTaxRecoverable'), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });
        $('#txtTaxLiabilityAccount').bind("change", function () {
            var accountNumber = $('#txtTaxLiabilityAccount').val();
            taxAuthoritiesUI.acctDescName = "LiabilityAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtTaxLiabilityAccount";
            sg.delayOnChange("btnLiabilityAccountFinder", $("#txtTaxLiabilityAccount"), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });
        $("#txtExpenseAccount").bind("change", function () {
            var accountNumber = $("#txtExpenseAccount").val();
            taxAuthoritiesUI.acctDescName = "ExpenseAccountDescription";
            taxAuthoritiesUI.acctDescId = "txtExpenseAccount";
            sg.delayOnChange("btnExpenseAccountFinder", $("#txtExpenseAccount"), function () {
                taxAuthoritiesRepository.getAccountDescription({ id: accountNumber }, taxAuthoritiesUISuccess.displayAccountDescription);
            });
        });
    },

    // Init Numeric TextBox

    initTextBoxValue: function (id, value) {
        var numerictextbox = $('#' + id).data("kendoNumericTextBox");
        numerictextbox.value(value);
    },

    numericBoxChange: function (e, id) {
        var value = e.sender._value;
        if (value) {
            var expression = parseInt(value);
            $(this).val(expression.toString());
        } else {
            taxAuthoritiesUI.initTextBoxValue(id, 0);
        }
    },

    initNumericTextBox: function () {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        var curdecimal = vm.CurrencyDecimals();
        var beforedecimal = 13;
        var maxAllow = Array(beforedecimal + 1).join("9");

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

        var maximumTax = $("#txtMaximumTaxAllowable").data("kendoNumericTextBox");
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

        var NochargeTxtBox = $("#txtNoTaxChargedBelow").data("kendoNumericTextBox");
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
        var RecoverRateTxtBox = $("#txtRecoRate").data("kendoNumericTextBox");
        sg.utls.kndoUI.restrictDecimals(RecoverRateTxtBox, 5, 3);

    },
    // Init Dropdowns here
    initDropDownList: function () {
        sg.utls.kndoUI.dropDownList("ddlreportTaxRetainage");
        sg.utls.kndoUI.dropDownList("ddltaxBase");
        sg.utls.kndoUI.dropDownList("ddlreportLevel");
        var data = taxAuthoritiesUI.taxAuthoritiesModel.Data;

        $("#ddlreportTaxRetainage").change(function () {
            var selIndex = $('#ddlreportTaxRetainage').data("kendoDropDownList").selectedIndex;
            $("#txtMaximumTaxAllowable").data("kendoNumericTextBox").enable(selIndex === 0);
            $("#txtNoTaxChargedBelow").data("kendoNumericTextBox").enable(selIndex === 0);
            data.ReportTaxonRetainageDocument(selIndex);
        });

        $("#ddltaxBase").change(function () {
            var selIndex = $('#ddltaxBase').data("kendoDropDownList").value();
            data.TaxBase(selIndex);
        });
        $("#ddlreportLevel").change(function () {
            var selIndex = $('#ddlreportLevel').data("kendoDropDownList").value();
            data.ReportLevel(selIndex);
        });

    },
    // Init Finders, if any
    initFinders: function () {
        var title = jQuery.validator.format(taxAuthoritiesResources.FinderTitle, taxAuthoritiesResources.TaxAuthorityTitle);
        sg.finderHelper.setFinder("btnFinderTaxAuthority", "tutaxauthorities", onFinderSuccess.taxAuthority, onFinderCancel.taxAuthority, title, taxAuthoritiesFilter.getFilter);

        title = jQuery.validator.format(taxAuthoritiesResources.FinderTitle, taxAuthoritiesResources.Reportingcurrency);
        sg.finderHelper.setFinder("btnCurrencyFinder", sg.finder.TaxCurrencyFinder, onFinderSuccess.currencyCode, onFinderCancel.currencyCode, title, sg.finderHelper.createDefaultFunction("txtTaxReportingCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, true);

        title = $.validator.format(taxAuthoritiesResources.FinderTitle, taxAuthoritiesResources.LiabilityAccount);
        sg.finderHelper.setFinder("btnLiabilityAccountFinder", "tutaxauthoritiesaccount", onFinderSuccess.liabilityAccount, onFinderCancel.liabilityAccount, title, sg.finderHelper.createDefaultFunction("txtTaxLiabilityAccount", "AccountNumber", sg.finderOperator.StartsWith));

        title = $.validator.format(taxAuthoritiesResources.FinderTitle, taxAuthoritiesResources.ExpenseAccount);
        sg.finderHelper.setFinder("btnExpenseAccountFinder", "tutaxauthoritiesaccount", onFinderSuccess.expenseAccount, onFinderCancel.expenseAccount, title, sg.finderHelper.createDefaultFunction("txtExpenseAccount", "AccountNumber", sg.finderOperator.StartsWith));

        title = $.validator.format(taxAuthoritiesResources.FinderTitle, taxAuthoritiesResources.RecoverabletaxAccount);
        sg.finderHelper.setFinder("btnTaxRecoverableFinder", "tutaxauthoritiesaccount", onFinderSuccess.recoverableAccount, onFinderCancel.recoverableAccount, title, sg.finderHelper.createDefaultFunction("txtTaxRecoverable", "AccountNumber", sg.finderOperator.StartsWith));

    },
    //Init CheckBoxs
    initCheckBoxes: function () {
        $("#chktaxRecoverable").bind("change", function () {
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

        $("#chkExpenseSeparately").bind("change", function () {
            if (!this.checked) {
                taxAuthoritiesUI.taxAuthoritiesModel.Data.ExpenseAccount("");
                taxAuthoritiesUI.taxAuthoritiesModel.ExpenseAccountDescription("");
            }
            taxAuthoritiesUI.expnsechk = this.checked;
            $("#txtExpenseAccount").enable(this.checked);
            $("#btnLoadexpense").enable(this.checked);
            $("#btnExpenseAccountFinder").enable(this.checked);

        });

        $("#chkallowTax").bind("change", function () {
            taxAuthoritiesUI.allowTaxInPricechk = this.checked;
        });
    },
    
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

    // Get
    get: function () {
        taxAuthoritiesRepository.get(modelData.TaxAuthority(), taxAuthoritiesUISuccess.get);
    },

    // Create
    create: function () {
        sg.utls.clearValidations("frmTaxAuthorities");
        taxAuthoritiesRepository.create(taxAuthoritiesUISuccess.create);
    },

    // Is Dirty check
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

    // Setkey
    setkey: function () {
        taxAuthoritiesUI.taxAuthority = modelData.TaxAuthority();
    },

    // Get
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

    // Update
    update: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            taxAuthoritiesUISuccess.setkey();
            taxAuthoritiesUISuccess.enableCurrency(false);
        }
        sg.utls.showMessage(jsonResult);
    },

    // Create
    create: function (jsonResult) {
        taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.reset();
        taxAuthoritiesUISuccess.setkey();
        taxAuthoritiesUISuccess.enableCurrency(true);
        sg.controls.Focus($("#txtTaxAuthority"));
    },

    // Delete
    delete: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            taxAuthoritiesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.reset();
            taxAuthoritiesUISuccess.enableCurrency(true);
            taxAuthoritiesUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    // Display Result
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
                var data = taxAuthoritiesUI.taxAuthoritiesModel.Data;
                var selIndex = data.ReportTaxonRetainageDocument();
                $("#ddlreportTaxRetainage").data("kendoDropDownList").value(selIndex);
                $("#ddltaxBase").data("kendoDropDownList").value(data.TaxBase());
                $("#ddlreportLevel").data("kendoDropDownList").value(data.ReportLevel());

                $("#chktaxRecoverable").prop("checked", data.TaxRecover()).applyCheckboxStyle();
                $("#chkExpenseSeparately").prop("checked", data.ExpenseSeparate()).applyCheckboxStyle();
                $("#chkallowTax").prop("checked", data.AllowTaxPrice()).applyCheckboxStyle();

                var maxAllow = (uiMode === sg.utls.OperationMode.NEW) ? "9999999999999" : data.MaximumTaxAllowable();
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

                var isTaxRecoverable = $("#chktaxRecoverable").prop("checked", data.TaxRecover()).applyCheckboxStyle()[0].checked;
                $("#txtTaxRecoverable").enable(isTaxRecoverable);
                $("#btnLoadrecoverable").enable(isTaxRecoverable);
                $("#btnTaxRecoverableFinder").enable(isTaxRecoverable);
                $("#txtRecoRate").data("kendoNumericTextBox").enable(isTaxRecoverable);

                var isExpenseAccount = $("#chkExpenseSeparately").prop("checked", data.ExpenseSeparate()).applyCheckboxStyle()[0].checked;
                $("#txtExpenseAccount").enable(isExpenseAccount);
                $("#btnLoadexpense").enable(isExpenseAccount);
                $("#btnExpenseAccountFinder").enable(isExpenseAccount);
            }
        }
    },

    // Initial Load
    initialLoad: function (result) {
        if (result) {
            taxAuthoritiesUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, taxAuthoritiesResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtTaxAuthority"));
    },

    // Finder Success
    finderSuccess: function (data) {
        if (data) {
            if (sg.controls.GetString(taxAuthoritiesUI.taxAuthority) !== data.TaxAuthority) {
                taxAuthoritiesUI.finderData = data;
                taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUI.get, taxAuthoritiesUI.taxAuthority);
            }
        }
    },

    // Set Finder
    setFinderData: function () {
        var data = taxAuthoritiesUI.finderData;
        sg.utls.clearValidations("frmTaxAuthorities");
        taxAuthoritiesUI.finderData = null;
        taxAuthoritiesRepository.get(data.TaxAuthority, taxAuthoritiesUISuccess.get);
    },

    // Is New
    isNew: function (model) {
        if (model.TaxAuthority() === null) {
           return true;
        }
        return false;
    },

    displayAccountDescription: function (result) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (typeof result === "string" || result instanceof String) {
            vm[taxAuthoritiesUI.acctDescName](result);
        }
        else {
            vm[taxAuthoritiesUI.acctDescName]("");
            sg.controls.Focus($('#'+ taxAuthoritiesUI.acctDescId));
        }
        sg.utls.showMessage(result);
    },

    displayCurrencyDescription: function (result) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (typeof result === "string" || result instanceof String) {
            vm.CurrencyDescription(result);
        }
        else {
            vm.CurrencyDescription("");
            sg.controls.Focus($('#txtTaxReportingCurrency'));
        }
        sg.utls.showMessage(result);
    },

    enableCurrency: function (e) {
        $("#txtTaxReportingCurrency").enable(e);
        $("#btnLoadcurrency").enable(e);
        $("#btnCurrencyFinder").enable(e);
    }
};

// Finder Filter
var taxAuthoritiesFilter = {
    getFilter: function () {
        var filters = [[]];
        var taxAuthoritiesName = $("#txtTaxAuthority").val();
        filters[0][0] = sg.finderHelper.createFilter("TaxAuthority", sg.finderOperator.StartsWith, taxAuthoritiesName);
        return filters;
    }
};
var onFinderSuccess = {

    taxAuthority: function (data) {
        if (data) {
            if (sg.controls.GetString(taxAuthoritiesUI.taxAuthority) !== data.TaxAuthority) {
                taxAuthoritiesUI.finderData = data;
                taxAuthoritiesUISuccess.setFinderData();
                taxAuthoritiesUI.checkIsDirty(taxAuthoritiesUI.get, taxAuthoritiesUI.taxAuthority);
                sg.utls.clearValidations("frmtaxAuthorities");
            }
        }
    },

    currencyCode: function (data) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.TaxReportingCurrency(data.CurrencyCodeId);
            vm.CurrencyDescription(data.Description);
            sg.controls.Focus($("#txtTaxReportingCurrency"));

        }
    },

    liabilityAccount: function (data) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.TaxLiabilityAccount(data.AccountNumber);
            vm.LiabilityAccountDescription(data.Description);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Liabilityconstant;
        }
    },

    expenseAccount: function (data) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.ExpenseAccount(data.AccountNumber);
            vm.ExpenseAccountDescription(data.Description);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Expconstant;
        }
    },

    recoverableAccount: function (data) {
        var vm = taxAuthoritiesUI.taxAuthoritiesModel;
        if (data) {
            vm.Data.RecoverableTaxAccount(data.AccountNumber);
            vm.RecoverableTaxAccountDescription(data.Description);
            taxAuthoritiesUI.type = taxAuthoritiesResources.Recoconstant;
        }
    },
};

var onFinderCancel = {

    taxAuthority: function () {
        sg.controls.Focus($("#txtTaxAuthority"));
    },

    currencyCode: function () {
        sg.controls.Focus($("#txtTaxReportingCurrency"));
    },

    liabilityAccount: function () {
        sg.controls.Focus($("#txtTaxLiabilityAccount"));
    },

    expenseAccount: function () {
        sg.controls.Focus($("#txtExpenseAccount"));
    },

    recoverableAccount: function () {
        sg.controls.Focus($("#txtTaxRecoverable"));
    },
};
 
// Initial Entry
$(function () {
    taxAuthoritiesUI.init();
    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && taxAuthoritiesUI.taxAuthoritiesModel.isModelDirty.isDirty()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, taxAuthoritiesResources.TaxAuthorityTitle)).text();
        }
    });
});
