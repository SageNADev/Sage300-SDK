// Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved.

"use strict";
var firstTimeLoginUI = firstTimeLoginUI || {};
var InvalidYear = 1900;
var firstTimeLoginUI = {
    firsttimeloginViewModel: {},
    init: function () {

        firstTimeLoginUI.initDropDownList();
        firstTimeLoginUI.initButtons();
        firstTimeLoginUI.initFinders();
    },
    initDropDownList: function () {
        var viewFirstTimeLoginModel = ko.mapping.fromJS(firsttimeloginViewModel);
        firstTimeLoginUI.firsttimeloginViewModel = viewFirstTimeLoginModel;
        sg.utls.kndoUI.dropDownList("EnumList");
        sg.utls.kndoUI.dropDownList("CurrencyRateTypeList");
        sg.utls.kndoUI.dropDownList("dataQurterWithperiods");
    },
    initButtons: function () {
        $("#FiscalYearTxtBx").keyup(function () {
            firstTimeLoginUI.firsttimeloginViewModel.Data.FiscalYearStartingDate = $("#FiscalYearTxtBx").val();
        });
        $("#OldestFiscalYearTxtBx").keyup(function () {
            firstTimeLoginUI.firsttimeloginViewModel.Data.OldestFiscalYear = $("#OldestFiscalYearTxtBx").val();
        });
        $("#Data_CurrentFiscalYear").keyup(function () {
            firstTimeLoginUI.firsttimeloginViewModel.Data.CurrentFiscalYear = $("#Data_CurrentFiscalYear").val();
        });
        $("#saveNewTenantForm").click(function (event) {
            firstTimeLoginUI.firsttimeloginViewModel.Data.FiscalYearStartingDate = $("#FiscalYearTxtBx").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.OldestFiscalYear = $("#OldestFiscalYearTxtBx").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.CurrentFiscalYear = $("#Data_CurrentFiscalYear").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.NoOfFiscalPeriods = $("#EnumList").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.QuarterWith4Periods = $("#dataQurterWithperiods").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.MultiCurrency = $('#Data_MultiCurrency').is(":checked");
            firstTimeLoginUI.firsttimeloginViewModel.Data.FunctionalCurrency = $("#txtCurrencyCode").val();
            firstTimeLoginUI.firsttimeloginViewModel.Data.DefaultRateType = $("#CurrencyRateTypeList").val();
            
            if (firstTimeLoginUI.firsttimeloginViewModel.Data.FiscalYearStartingDate < InvalidYear) {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, NewTenantResources.InvalidYearMsg);               
            }
            else if (firstTimeLoginUI.firsttimeloginViewModel.Data.OldestFiscalYear < InvalidYear) {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, NewTenantResources.InvalidYearMsg);                
            }
            else if (firstTimeLoginUI.firsttimeloginViewModel.Data.CurrentFiscalYear < InvalidYear) {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, NewTenantResources.InvalidYearMsg);
            }
            else {
                newTenantRepository.save(firstTimeLoginUI.firsttimeloginViewModel);
            }
        });
        $("#cancelNewTenantForm").click(function () {
            $('.overlay').removeClass('hide');
            $('.confnMsgBox').removeClass('hide');
        });        
        $("#popupBtnLogout").click(function () {
            $('.overlay').addClass('hide');
            $('.confnMsgBox').addClass('hide');
            sg.utls.logOut();
        });
        $("#popupBtnNo").click(function () {
            $('.overlay').addClass('hide');
            $('.confnMsgBox').addClass('hide');
        });
        $(".msgCtrl-close").click(function () {
            $('.overlay').addClass('hide');
            $('.confnMsgBox').addClass('hide');
        });

        //to covert UTC date coming from the server to the local date
        var fiscalYearStartingDate = new Date(firstTimeLoginUI.firsttimeloginViewModel.Data.FiscalYearStartingDate());
        $("#FiscalYearTxtBx").val(kendo.toString(fiscalYearStartingDate, 'yyyy-MM-dd'));

        sg.utls.kndoUI.datePicker("FiscalYearTxtBx");
        $("#Data_MultiCurrency").change(function () {
            if (this.checked) {
                $(".defaultratetypecontainer").show();
            } else {
                $(".defaultratetypecontainer").hide();
            }
        });
        if ($("#Data_MultiCurrency").is(':checked')) {
            $("#Data_MultiCurrency").prop("disabled", true);
        } else {
            $(".defaultratetypecontainer").hide();
        }
        $("#EnumList").change(function () {
            var noOfFiscalPeriods = $('option:selected', this).text();
            if (noOfFiscalPeriods == "13") {
                $(".Qtr4Periods").show();
            } else {
                $(".Qtr4Periods").hide();
            }
        });
      
    },
    initFinders: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFinderCurrencyCodes", "txtCurrencyCode", sg.viewFinderProperties.CS.CurrencyCodes,
            firstTimeLoginUI.finderSuccess, $.noop);
    },
    finderSuccess: function (result) {
        if (result) {
            $('#txtCurrencyCode').val(result.CURID);         
        }
    }
};
var currencyCodeFilter = {
    getFilter: function () {
        var filters = [[]];
        var currencyCodeName = $("#txtCurrencyCode").val();
        filters[0][0] = sg.filterHelper.createFilter("CurrencyCode", sg.finderOperator.StartsWith, currencyCodeName);
        return filters;
    }
};

$(function () {
    firstTimeLoginUI.init();
});

var newTenantUISuccess = {
   
    saveSuccess: function (data) {
        if (data.UserMessage.IsSuccess) {
            window.location = sg.utls.url.buildUrl("Core", "NewTenant", "RedirectUser");
        }
        sg.utls.showMessage(data);
    }
};
