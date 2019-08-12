/* Copyright (c) 1994-2015 Sage Software, Inc.  All rights reserved. */
"use strict";

var wizardUI = {
    
    doneDoneBtn: null,
    newTenantViewModel: null,
    msgDivId: "wizardMessage",

    init: function ()
    {
        wizardUI.doneDoneBtn = $("section.step-done input:button.btn-primary");
        
        $("#EnumList").change(function () {
            var noOfFiscalPeriods = $('option:selected', this).text();
            if (noOfFiscalPeriods == "13") {
                $(".Qtr4Periods").show();
            } else {
                $(".Qtr4Periods").hide();
            }
        });

        $("#Data_MultiCurrency").change(function () {
            if (this.checked) {
                $("#defaultRateTypeGroup").show();
            } else {
                $("#defaultRateTypeGroup").hide();
            }
        });

        $('input[name="isSkip"]:radio').change(function () {
            if (this.value === "isSkip") {
                wizardUI.newTenantViewModel.IsSkipSetup(true);
                $(".staticgrid-group-wizard").hide();
            }
            else {
                $(".staticgrid-group-wizard").show();
                wizardUI.newTenantViewModel.IsSkipSetup(false);
            }
        });
    
        $(".btn-primary").click(function () {
            if (wizardUI.doneDoneBtn[0] !== $(this)[0])
            {
                $(this).closest(".modal-body").removeClass("active").addClass("prev").next().addClass("active");
            }
        });

        $(".btn-secondary").click(function () {
            $(this).closest(".modal-body").removeClass("active prev").prev().addClass("active");
        });

        $("#txtCurrencyCode").blur(function () {
            var currencyCode = $("#txtCurrencyCode").val().toUpperCase();
            if (currencyCode !== "" && newTenantModel.CurrencyCodes.indexOf(currencyCode) === -1) {
                wizardUI.showInValidMessage($.validator.format(wizardResources.InvalidFunctionalCurrencyMsg, [currencyCode]));
            }
        });

        wizardUI.doneDoneBtn.click(function () {
            var returnModel = ko.mapping.toJS(wizardUI.newTenantViewModel);
            returnModel.AccountDataSet = null; // reduce size to return and effort to parse on server side
            returnModel.CurrencyCodes = null; // reduce size to return and effort to parse on server side
            returnModel.ConfirmedAccountDataSetString = JSON.stringify($("#accountGrid").data().kendoGrid.dataSource.data().toJSON());

            // to validate user input

            if (returnModel.Data.FiscalYearStartingDate === null || returnModel.Data.FiscalYearStartingDate === undefined) {
                wizardUI.showInValidMessage(wizardResources.InvalidStartingDate);
            }
            else if (returnModel.Data.OldestFiscalYear > returnModel.Data.CurrentFiscalYear) {
                wizardUI.showInValidMessage($.validator.format(wizardResources.InvalidDateSequence, [$("label[for='OldestFiscalYear']").text(), $("label[for='CurrentFiscalYear']").text()]));
            }
            else if (returnModel.Data.FunctionalCurrency === null || returnModel.Data.FunctionalCurrency === "") {
                wizardUI.showInValidMessage(wizardResources.EmptyCurrencyMsg);
            }
            else if (newTenantModel.CurrencyCodes.indexOf(returnModel.Data.FunctionalCurrency) === -1) {
                wizardUI.showInValidMessage($.validator.format(wizardResources.InvalidFunctionalCurrencyMsg, [returnModel.Data.FunctionalCurrency]));
            }
            else if (returnModel.Data.OldestFiscalYear < 1900 || returnModel.Data.CurrentFiscalYear < 1900) {
                wizardUI.showInValidMessage(wizardResources.InvalidYearMsg);
            } else {
                $('#ajaxSpinner').fadeIn();
                sg.utls.ajaxInternal(sg.utls.url.buildUrl("Core", "Home", "SaveNewTenant"), returnModel, wizardUI.saveSuccess, "json", "post", true, wizardUI.saveFail);
            }
        });
    },

    showInValidMessage: function(msg)
    {
        sg.utls.showMessageInfoInCustomDiv(sg.utls.msgType.ERROR, msg, wizardUI.msgDivId);

        $(".btn-primary, .btn-secondary").prop('disabled', true);

        $(".msgCtrl-close").click(function () {
            $(".btn-primary, .btn-secondary").prop('disabled', false);
            $("#" + wizardUI.msgDivId).hide();
        });
    },

    saveSuccess: function (e) {
        wizardUI.beforeLeave();
    },

    saveFail: function (e) {
        wizardUI.beforeLeave();
    },

    beforeLeave: function () {
        $('#ajaxSpinner').fadeOut();
        //$(".btn-primary").unbind();
        //$(".btn-secondary").unbind();
        $(".setup-wizard").remove();
    },

    initKoBinding: function() {
        wizardUI.newTenantViewModel = ko.mapping.fromJS(newTenantModel);
        ko.applyBindings(wizardUI.newTenantViewModel, $("div.setup-wizard")[0]);
    },

    initDropDownBinding: function () {
        sg.utls.kndoUI.dropDownList("EnumList");
        sg.utls.kndoUI.dropDownList("dataQurterWithperiods");
        sg.utls.kndoUI.dropDownList("CurrencyRateTypeList");

        $("#EnumList").data("kendoDropDownList").value(wizardUI.newTenantViewModel.Data.NoOfFiscalPeriods());
        $("#dataQurterWithperiods").data("kendoDropDownList").value(wizardUI.newTenantViewModel.Data.QuarterWith4Periods());
        $("#CurrencyRateTypeList").data("kendoDropDownList").value(wizardUI.newTenantViewModel.Data.DefaultRateType());
    },

    initFinders: function () {
        var title = jQuery.validator.format(wizardResources.FinderTitle, wizardResources.CurrencyCodeTitle);
        
        sg.finderHelper.setFinder(
            "btnFinderCurrencyCodes",
            sg.finder.CurrencyCode,
            wizardUI.finderSuccess,
            $.noop(),
            title,
            sg.finderHelper.createDefaultFunction(
            "txtCurrencyCode", "CurrencyCodeId",
            sg.finderOperator.StartsWith), null, true, 550);
    },

    finderSuccess: function (result) {
        if (result !== null && result.CurrencyCodeId !== null) {
            wizardUI.newTenantViewModel.Data.FunctionalCurrency(result.CurrencyCodeId);
        }
    },

    initAccountGrid: function () {
        var dataSource = new kendo.data.DataSource({
            data: newTenantModel.AccountDataSet.Accounts,
            batch: true,
            schema: {
                model: {
                    id: "ACCTID",
                    fields: {
                        ACCTID: { type: "number", editable: false, nullable: false },
                        ACCTDESC: { type: "string", editable: true, nullable: false }
                    }
                }
            }
        });

        $("#accountGrid").kendoGrid({
            dataSource: dataSource,
            pageable: false,
            columns: [
                { field: "ACCTID", title: wizardResources.AccountColumnHeader },
                { field: "ACCTDESC", title: wizardResources.DescriptionColumnHeader }],
            editable: true
        });
    },
};


(function (wizardUI) {
    wizardUI.init();
    wizardUI.initKoBinding();
    wizardUI.initDropDownBinding();
    wizardUI.initFinders();
    wizardUI.initAccountGrid();
}(wizardUI));
