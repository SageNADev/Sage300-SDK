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

"use strict";

var clearStatisticsUI = clearStatisticsUI || {}
var minimumFiscalYear = 1900;

clearStatisticsUI = {
    clearStatisticsModel: {},
    computedProperties: ["bClearCustomerStatistics", "bClearGroupStatistics", "bClearNationalAcctStatistics", "bClearSalespersonStatistics"],
    customerFiscalYear: null,
    customerGroupFiscalYear: null,
    nationalAcctFiscalYear: null,
    salespersonFiscalYear: null,
    itemFiscalYear: null,
    customerFiscalPeriod: null,
    customerGroupFiscalPeriod: null,
    nationalAcctFiscalPeriod: null,
    salespersonFiscalPeriod: null,
    itemFiscalPeriod: null,
    init: function () {     
        // Initialize the controls and apply kendo bindings 
        clearStatisticsUI.initKendoBindings();          
        clearStatisticsUI.initFinder();
        clearStatisticsUI.initButtons();
        clearStatisticsUI.initTextBox();
        clearStatisticsUI.initCheckBox();
        clearStatisticsUI.initBlur();
        clearStatisticsUI.initProcessUI();
        ko.applyBindings(clearStatisticsUI.clearStatisticsModel);
        //if (clearStatisticsUI.clearStatisticsModel.CalendarYear()) {
        //    $("#btnFindCustomerYear").hide();
        //} else {
        //    $("#btnFindCustomerYear").show();
        //}
    },
    
    initKendoBindings: function () {
        clearStatisticsUI.clearStatisticsModel = ko.mapping.fromJS(ClearStatisticsViewModel);       
        clearStatisticsUI.customerFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
        clearStatisticsUI.customerGroupFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
        clearStatisticsUI.nationalAcctFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear();
        clearStatisticsUI.salespersonFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear();
        clearStatisticsUI.itemFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
        clearStatisticsUtilities.customerYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
        clearStatisticsUtilities.customerGroupYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
        clearStatisticsUtilities.nationalAcctYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear();
        clearStatisticsUtilities.salespersonYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear();
        clearStatisticsUtilities.itemYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
        clearStatisticsUI.customerFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
        clearStatisticsUI.customerGroupFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
        clearStatisticsUI.nationalAcctFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctPeriod();
        clearStatisticsUI.salespersonFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonPeriod();
        clearStatisticsUI.itemFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod();
        clearStatisticsUtilities.customerPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
        clearStatisticsUtilities.customerGroupPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
        clearStatisticsUtilities.nationalAcctPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctPeriod();
        clearStatisticsUtilities.salespersonPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonPeriod();
        clearStatisticsUtilities.itemPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod();
        tuClearStatisticsKoExtn.tuClearStatisticsModelExtension(clearStatisticsUI.clearStatisticsModel);
    },

    initFinder: function() {
        // Initializing Customer Statistics Finder
        clearStatisticsUI.initCustomerNumberFinder();
        // Initializing Customer Group Statistics Finder
        clearStatisticsUI.initCustomerGroupFinder();
        // Initializing National Account Statistics Finder
        clearStatisticsUI.initNationalAcctFinder();
        // Initializing Salesperson Statistics Finder
        clearStatisticsUI.initSalespersonFinder();
        // Initializing Item Statistics Finder
        clearStatisticsUI.initItemFinder();
        // Initializing Fiscal Year Finder
        clearStatisticsUI.initFiscalYearFinder();
    },

    initCustomerNumberFinder: function() {
        var title = jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.CustomerNumberTitle);
        //From Customer
        sg.finderHelper.setFinder("btnFromCustomerFinder", sg.finder.ARCustomerFinder, onFinderSuccess.FromCustomerFinder, onFinderCancel.FromCustomerFinder, title,
            sg.finderHelper.createDefaultFunction("Data_FromCustomerNo", "CustomerNumber", sg.finderOperator.StartsWith), null, true);
        //To Customer
        sg.finderHelper.setFinder("btnToCustomerFinder", sg.finder.ARCustomerFinder, onFinderSuccess.ToCustomerFinder, onFinderCancel.ToCustomerFinder, title,
            sg.finderHelper.createDefaultFunction("Data_ToCustomerNo", "CustomerNumber", sg.finderOperator.StartsWith), null, true);
    },

    initCustomerGroupFinder: function() {
        var title = jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.CustomerGroupFinder);
        //From Customer Group
        sg.finderHelper.setFinder("btnFromCustomerGroupFinder", sg.finder.CustomerGroup, onFinderSuccess.FromCustomerGroupFinder, onFinderCancel.FromCustomerGroupFinder, title,
            sg.finderHelper.createDefaultFunction("Data_FromGroupCode", "GroupCode", sg.finderOperator.StartsWith), null, true);
        //To Customer
        sg.finderHelper.setFinder("btnToCustomerGroupFinder", sg.finder.CustomerGroup, onFinderSuccess.ToCustomerGroupFinder, onFinderCancel.ToCustomerGroupFinder, title,
            sg.finderHelper.createDefaultFunction("Data_ToGroupCode", "GroupCode", sg.finderOperator.StartsWith), null, true);
    },
    
    initNationalAcctFinder: function () {
        var title = jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.NationalAccountNumberTitle);
        //From National Account
        sg.finderHelper.setFinder("btnFromNationalAcctFinder", sg.finder.NationalAccounts, onFinderSuccess.FromNationalAccountFinder,onFinderCancel.FromNationalAccountFinder , title,
            sg.finderHelper.createDefaultFunction("Data_FromNationalAccount", "NationalAccountNumber", sg.finderOperator.StartsWith), null, true);
        //To National Account
        sg.finderHelper.setFinder("btnToNationalAcctFinder", sg.finder.NationalAccounts, onFinderSuccess.ToNationalAccountFinder, onFinderCancel.ToNationalAccountFinder, title,
            sg.finderHelper.createDefaultFunction("Data_ToNationalAccount", "NationalAccountNumber", sg.finderOperator.StartsWith), null, true);
    },
    
    initSalespersonFinder: function () {
        var title = jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.SalesPersonFinderTitle);
        //From Salesperson
        sg.finderHelper.setFinder("btnFromSalespersonFinder", sg.finder.SalesPersonFinder, onFinderSuccess.FromSalespersonFinder, onFinderCancel.FromSalespersonFinder, title,
            sg.finderHelper.createDefaultFunction("Data_FromSalesPerson", "SalesPersonCode", sg.finderOperator.StartsWith), null, true);
        //To Salesperson
        sg.finderHelper.setFinder("btnToSalespersonFinder", sg.finder.SalesPersonFinder, onFinderSuccess.ToSalespersonFinder, onFinderCancel.ToSalespersonFinder, title,
            sg.finderHelper.createDefaultFunction("Data_ToSalesPerson", "SalesPersonCode", sg.finderOperator.StartsWith), null, true);
    },

    initFiscalYearFinder: function () {
      var title = window.jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.FiscalYear);
      sg.finderHelper.setFinder("btnFindCustomerYear", sg.finder.FiscalYear, onFinderSuccess.CustomerYearFinder, onFinderCancel.CustomerYearFinder, title, sg.finderHelper.createDefaultFunction("Data_ThroughCustomerYear", "FiscalYear", sg.finderOperator.StartsWith), null, true);
      sg.finderHelper.setFinder("btnFindCustomerGroupYear", sg.finder.FiscalYear, onFinderSuccess.CustomerGroupYearFinder, onFinderCancel.CustomerGroupYearFinder, title, sg.finderHelper.createDefaultFunction("Data_ThroughGroupYear", "FiscalYear", sg.finderOperator.StartsWith), null, true);
      sg.finderHelper.setFinder("btnFindNationalAcctYear", sg.finder.FiscalYear, onFinderSuccess.NationalAcctYearFinder, onFinderCancel.NationalAcctYearFinder, title, sg.finderHelper.createDefaultFunction("Data_ThroughNationalAcctYear", "FiscalYear", sg.finderOperator.StartsWith), null, true);
      sg.finderHelper.setFinder("btnFindSalespersonYear", sg.finder.FiscalYear, onFinderSuccess.SalespersonYearFinder, onFinderCancel.SalespersonYearFinder, title, sg.finderHelper.createDefaultFunction("Data_ThroughSalesPersonYear", "FiscalYear", sg.finderOperator.StartsWith), null, true);
      sg.finderHelper.setFinder("btnFindItemYear", sg.finder.FiscalYear, onFinderSuccess.ItemYearFinder, onFinderCancel.ItemYearFinder, title, sg.finderHelper.createDefaultFunction("Data_ThroughItemYear", "FiscalYear", sg.finderOperator.StartsWith), null, true);
    },
    
    initItemFinder: function () {
        var title = jQuery.validator.format(clearStatisticsResources.FinderTitle, clearStatisticsResources.ItemFinderTitle);
        //From Item
        sg.finderHelper.setFinder("btnFromItemFinder", sg.finder.ARItemNumberFinder, onFinderSuccess.FromItemFinder, onFinderCancel.FromItemFinder, title,
            sg.finderHelper.createDefaultFunction("Data_FromItem", "ItemNumber", sg.finderOperator.StartsWith), null, true);
        //To Item
        sg.finderHelper.setFinder("btnToItemFinder", sg.finder.ARItemNumberFinder, onFinderSuccess.ToItemFinder, onFinderCancel.ToItemFinder, title,
            sg.finderHelper.createDefaultFunction("Data_ToItem", "ItemNumber", sg.finderOperator.StartsWith), null, true);
    },

    initCheckBox: function () {

        $("#Data_ClearCustomerStatistics").click(function (e) {
            if ($(this).is(':checked')) {
                setTimeout(function() {
                    sg.controls.Focus($("#Data_FromCustomerNo"));
                })
               
            }
        });       
        $(document).on("change", "#Data_ClearGroupStatistics", function () {
            if ($("#Data_ClearGroupStatistics").is(":checked")) {
                setTimeout(function() {
                    sg.controls.Focus($("#Data_FromGroupCode"));
                })
            }
        });
        $(document).on("change", "#Data_ClearNationalAcctStatistics", function () {
            if ($("#Data_ClearNationalAcctStatistics").is(":checked")) {
                setTimeout(function() {
                    sg.controls.Focus($("#Data_FromNationalAccount"));
                })
            }
        });
        $(document).on("change", "#Data_ClearSalesPersonStatistics", function () {
            if ($("#Data_ClearSalesPersonStatistics").is(":checked")) {
                setTimeout(function() {
                    sg.controls.Focus($("#Data_FromSalesPerson"));
                })
            }
        });
        $(document).on("change", "#Data_ClearItemStatistics", function () {
            if ($("#Data_ClearItemStatistics").is(":checked")) {
                setTimeout(function() {
                    sg.controls.Focus($("#Data_FromItem"));
                })
            }
        });
    },
    
    initBlur:function() {
        $("#Data_ThroughCustomerYear").bind('change', function(e) {
            sg.delayOnChange("btnFindCustomerYear", $("Data_ThroughCustomerYear"), function() {
                var year = $("#Data_ThroughCustomerYear").val();
                var oldYear = clearStatisticsUI.customerFiscalYear;
                if (clearStatisticsUI.clearStatisticsModel.CalendarYear()) {
                    if (year < minimumFiscalYear) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear(oldYear);
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear(clearStatisticsUtilities.customerYearBackup);
                        //clearStatisticsUtilities.revertYearValue();
                        $('#Data_ThroughCustomerYear').focus();
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshCustomerStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                } else {
                    if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughCustomerYear").val()))) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear(clearStatisticsUtilities.customerYearBackup);
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateCustomerYear, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
            });
        });

        $("#Data_ThroughGroupYear").bind('change', function (e) {
            sg.delayOnChange("btnFindCustomerGroupYear", $("Data_ThroughGroupYear"), function() {
            var year = $("#Data_ThroughGroupYear").val();
            var oldYear = clearStatisticsUI.customerGroupFiscalYear;
            if (clearStatisticsUI.clearStatisticsModel.CalendarYear()) {
                if (year < minimumFiscalYear) {
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(oldYear);
                    //clearStatisticsUtilities.revertYearValue();
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(clearStatisticsUtilities.customerGroupYearBackup);
                    $('#Data_ThroughGroupYear').focus();
                } else {
                    clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshGroupStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                }
            }
            else {
                if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughGroupYear").val()))) {
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(clearStatisticsUtilities.customerGroupYearBackup);
                } else {
                    clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateGroupYear, clearStatisticsUtility.setFocusToFiscalYear);
                }
            }
            });
        });
        
            $("#Data_ThroughNationalAcctYear").bind('change', function (e) {
                sg.delayOnChange("btnFindNationalAcctYear", $("Data_ThroughNationalAcctYear"), function() {
            var year = $("#Data_ThroughNationalAcctYear").val();
            var oldYear = clearStatisticsUI.nationalAcctFiscalYear;
            if (clearStatisticsUI.clearStatisticsModel.CalendarYear()) {
                if (year < minimumFiscalYear) {
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear(oldYear);
                    //clearStatisticsUtilities.revertYearValue();
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear(clearStatisticsUtilities.nationalAcctYearBackup);
                    $('#Data_ThroughNationalAcctYear').focus();
                } else {
                    clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshNationalAcctStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                }
            }
            else {
                if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughNationalAcctYear").val()))) {
                    clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear(clearStatisticsUtilities.nationalAcctYearBackup);
                } else {
                    clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateNationalAcctYear, clearStatisticsUtility.setFocusToFiscalYear);
                }
              }
            });
        });

            $("#Data_ThroughSalesPersonYear").bind('change', function (e) {
                sg.delayOnChange("btnFindSalespersonYear", $("Data_ThroughSalesPersonYear"), function () {
                    var year = $("#Data_ThroughSalesPersonYear").val();
                    var oldYear = clearStatisticsUI.salespersonFiscalYear;
                    if (clearStatisticsUI.clearStatisticsModel.SalesCalendarYear()) {
                        if (year < minimumFiscalYear) {
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear(oldYear);
                            //clearStatisticsUtilities.revertYearValue();
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear(clearStatisticsUtilities.salespersonYearBackup);
                            $('#Data_ThroughSalesPersonYear').focus();
                        } else {
                            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshSalespersonStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                        }
                    }
                    else {
                        if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughSalesPersonYear").val()))) {
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear(clearStatisticsUtilities.salespersonYearBackup);
                        } else {
                            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateSalespersonYear, clearStatisticsUtility.setFocusToFiscalYear);
                        }
                    }
                });
            });

            $("#Data_ThroughItemYear").bind('change', function (e) {
                sg.delayOnChange("btnFindItemYear", $("Data_ThroughItemYear"), function () {
                    var year = $("#Data_ThroughItemYear").val();
                    var oldYear = clearStatisticsUI.itemFiscalYear;
                    if (clearStatisticsUI.clearStatisticsModel.ItemCalendarYear()) {
                        if (year < minimumFiscalYear) {
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(oldYear);
                            //clearStatisticsUtilities.revertYearValue();
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(clearStatisticsUtilities.itemYearBackup);
                            $('#Data_ThroughItemYear').focus();
                        } else {
                            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshItemStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                        }
                    }
                    else {
                        if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughItemYear").val()))) {
                            clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(clearStatisticsUtilities.itemYearBackup);
                        } else {
                            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateItemYear, clearStatisticsUtility.setFocusToFiscalYear);
                        }
                    }
                });
            });
        
            $("#Data_ThroughCustomerPeriod").bind('change', function (e) {
                $("#message").empty();               
                clearStatisticsUtilities.validateCustomerPeriod();
            });
        
            $("#Data_ThroughGroupPeriod").bind('change', function (e) {
                $("#message").empty();
                clearStatisticsUtilities.validateGroupPeriod();
            });
        
            $("#Data_ThroughNationalAcctPeriod").bind('change', function (e) {
                $("#message").empty();
                clearStatisticsUtilities.validateNationalAcctPeriod();
            });
        
            $("#Data_ThroughSalesPersonPeriod").bind('change', function (e) {
                $("#message").empty();
               // clearStatisticsUtilities.backupSalespersonPeriodValue();
                clearStatisticsUtilities.validateSalespersonPeriod();
            });
        
            $("#Data_ThroughItemPeriod").bind('change', function (e) {
                $("#message").empty();
                clearStatisticsUtilities.validateItemPeriod();
            });
    },    

    initTextBox: function ()
    {
        $("#Data_ThroughCustomerPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.MinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod(value);                
                clearStatisticsUtilities.backupCustomerPeriodValue();
            }
        });

        $("#Data_ThroughGroupPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.MinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod(value);                
                clearStatisticsUtilities.backupGroupPeriodValue();
            }
        });

        $("#Data_ThroughNationalAcctPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.MinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctPeriod(value);
                clearStatisticsUtilities.backupNationalAcctPeriodValue();
            }
            
        });

        $("#Data_ThroughSalesPersonPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.SalesPersonMinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonPeriod(value);
                clearStatisticsUtilities.backupSalespersonPeriodValue();
            }
        });
        
        $("#Data_ThroughItemPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.ItemMinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.ItemMaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod(value);
                clearStatisticsUtilities.backupItemPeriodValue();
            }
        });

    },
    
    initButtons: function () {
        $("#btnProcess").click(function (e) {
            debugger;
            sg.utls.SyncExecute(clearStatisticsUI.process);
        });
    },
    
    process: function () {
        debugger;
        sg.utls.isProcessRunning = true;
        var processUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Process");
        
        var isChecked = (clearStatisticsUI.clearStatisticsModel.Data.bClearCustomerStatistics()
          || clearStatisticsUI.clearStatisticsModel.Data.bClearGroupStatistics()
          || clearStatisticsUI.clearStatisticsModel.Data.bClearNationalAcctStatistics()
          || clearStatisticsUI.clearStatisticsModel.Data.bClearSalespersonStatistics()
          || clearStatisticsUI.clearStatisticsModel.Data.bClearItemStatistics());

        if (!isChecked) {
            //Do not process
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, clearStatisticsResources.NoProcessingOption);
            sg.utls.isProcessRunning = false;
        }
        //else {
        //    if (sg.utls.isProcessRunning) {
        //        return;
        //    }
       // }
        //Check if form is valid
        if ($("#frmClearStatistics").valid() && sg.utls.isProcessRunning) {
            //Check Validations
            if (clearStatisticsUI.Validation()) {
                $("#message").empty();
                sg.utls.clearValidations("frmClearStatistics");
                sg.utls.isProcessRunning = true;
                var data = { model: ko.mapping.toJS(clearStatisticsUI.clearStatisticsModel, clearStatisticsUI.computedProperties) };
                sg.utls.ajaxPost(processUrl, data, onSuccess.process)
            }
        }
    },
    
    initProcessUI: function() {
        var progressUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Progress");
        var cancelUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Cancel");
        window.progressUI.init(progressUrl, cancelUrl, clearStatisticsUI.clearStatisticsModel, screenName, onSuccess.onProcessComplete);
    },

    Validation: function () {
        var errorRangeMessage = "";
        var inputValid = true;
            // If FromCustomer is greater than ToCustomer, throw an exception
        if (clearStatisticsUI.clearStatisticsModel.Data.ClearCustomerStatistics() && (clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNo() != null &&
          clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNo().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToCustomerNo())) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.CustomerNumberTitle;
            sg.controls.Focus($("#Data_FromCustomerNo"));
        }
            // If FromCustomerGroup is greater than ToCustomerGroup, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearGroupStatistics() && (clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode() != null &&
        clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToGroupCode())) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.CustomerGroupFinder;
            sg.controls.Focus($("#Data_FromGroupCode"));
        }
            // If FromNationalAccnt is greater than ToNationalAccnt, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearNationalAcctStatistics() && clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToNationalAccount()) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.NationalAccountNumberTitle;
            sg.controls.Focus($("#Data_FromNationalAccount"));
        }
            // If FromSalesPerson is greater than ToSalesPerson, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearSalesPersonStatistics() && clearStatisticsUI.clearStatisticsModel.Data.FromSalesPerson() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromSalesPerson().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToSalesPerson()) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.SalesPersonFinderTitle;
            sg.controls.Focus($("#Data_FromSalesPerson"));
        }
            // If FromItem is greater than ToItem, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearItemStatistics() && clearStatisticsUI.clearStatisticsModel.Data.FromItemNumber() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromItemNumber().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToItemNumber()) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.ItemFinderTitle;
            sg.controls.Focus($("#Data_FromItem"));
        }

        if (!inputValid) {
            if (errorRangeMessage != "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(clearStatisticsResources.ErrorFromToValueMessage, errorRangeMessage));
            } else if (errorMessage != "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
            }
        }

        return inputValid;
    },
    fiscalYrExists: function (Year) {
        if (clearStatisticsUI.clearStatisticsModel.FiscalCalendars() !== null) {
            for (var i = 0; i < clearStatisticsUI.clearStatisticsModel.FiscalCalendars().length; i++) {
                if (Year === clearStatisticsUI.clearStatisticsModel.FiscalCalendars()[i].Year()) {
                    return true;
                }
            }
        }
        return false;
    },

};

var onSuccess = {
    process: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            var model = clearStatisticsUI.clearStatisticsModel;
            window.ko.mapping.fromJS(jsonResult.WorkflowInstanceId, {}, model.WorkflowInstanceId);
            window.progressUI.progress();
        } else {
            sg.utls.showMessage(jsonResult);
        }
    },
    onProcessComplete: function (result) {
        if (result.ProcessResult.Results.length <= 0) {
            $("#processingResultGrid").hide();
            var errorMessage = clearStatisticsResources.ProcessingComplete;
            sg.utls.showMessageInfoInCustomDivWithoutClose(sg.utls.msgType.INFO, errorMessage, 'messageDiv');
        }
    },
};

// Finder success method
var onFinderSuccess = {
    // variable for From Customer Number finder.
    FromCustomerFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNo(data.CustomerNumber);
            //setTimeout(function () {
            //    ($("#Data_ToCustomerNo")).siblings('input:visible').focus();
            //});
            sg.controls.Focus($("#Data_ToCustomerNo"));
        }
    },
    // variable for To Customer Number finder.
    ToCustomerFinder: function (data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.ToCustomerNo(data.CustomerNumber);
            sg.controls.Focus($("#Data_ThroughCustomerYear"));
        }       
    },
    // variable for From Customer Group finder.
    FromCustomerGroupFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode(data.GroupCode);
            //setTimeout(function () {
            //    ($("#Data_ToGroupCode")).siblings('input:visible').focus();
            //});
            sg.controls.Focus($("#Data_ToGroupCode"));
        }
    },
    // variable for To Customer Group finder.
    ToCustomerGroupFinder: function (data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.ToGroupCode(data.GroupCode);
            sg.controls.Focus($("#Data_ThroughGroupYear"));
        }   
    },
    // variable for From National Account finder.
    FromNationalAccountFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount(data.NationalAccountNumber);
            //setTimeout(function () {
            //    ($("#Data_ToNationalAccount")).siblings('input:visible').focus();
            //});
            sg.controls.Focus($("#Data_ToNationalAccount"));
        }
    },
    // variable for To National Account finder.
    ToNationalAccountFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.ToNationalAccount(data.NationalAccountNumber);
            //setTimeout(function () {
            //    ($("#Data_ToNationalAccount")).siblings('input:visible').focus();
            //});
            sg.controls.Focus($("#Data_ThroughNationalAcctPeriod"));
        }
    },
    // variable for From Salesperson finder.
    FromSalespersonFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.FromSalesPerson(data.SalesPersonCode);
            sg.controls.Focus($("#Data_ToSalesPerson"));
        }
    },
    // variable for To Salesperson finder.
    ToSalespersonFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.ToSalesPerson(data.SalesPersonCode);
            sg.controls.Focus($("#Data_ThroughSalesPersonYear"));
        }
    },
    // variable for From Item finder.
    FromItemFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.FromItemNumber(data.ItemNumber);
            sg.controls.Focus($("#Data_ToItem"));
        }
    },
    // variable for To Item finder.
    ToItemFinder: function(data) {
        if (data != null) {
            clearStatisticsUI.clearStatisticsModel.Data.ToItemNumber(data.ItemNumber);
            sg.controls.Focus($("#Data_ThroughItemYear"));
        }
    },
    // variable for Customer Year finder.
    CustomerYearFinder: function(data) {
        if (data) {
            var year = data.FiscalYear;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getCustomerMaxPeriodForValidYear($("#Data_ThroughCustomerYear").val())               
            }
        }
    
    },
    // variable for Customer Group Year finder.
    CustomerGroupYearFinder: function (data) {
        if (data) {
            var year = data.FiscalYear;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getGroupMaxPeriodForValidYear($("#Data_ThroughGroupYear").val())               
            }
        }
    },
    // variable for National Acct Year finder.
    NationalAcctYearFinder: function (data) {
        if (data) {
            var year = data.FiscalYear;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getNationalAcctMaxPeriodForValidYear($("#Data_ThroughNationalAcctYear").val())               
            }
        }
    },
    // variable for Salesperson Year finder.
    SalespersonYearFinder: function (data) {
        if (data) {
            var year = data.FiscalYear;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear(year);               
                //Now Validate the method year
                clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalesPersonYear").val())
                
            }
           
        }      
    },
    // variable for Item Year finder.
    ItemYearFinder: function (data) {
        if (data) {
            var year = data.FiscalYear;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getItemMaxPeriodForValidYear($("#Data_ThroughItemYear").val())               
            }
        }
    },
};

var clearStatisticsUtility = {   
    checkIsDirty: function (yesFunctionToCall, noFunctionToCall) {
        if (clearStatisticsUI.clearStatisticsModel.IsKoStatisticsDirty && clearStatisticsUI.clearStatisticsModel.IsKoStatisticsDirty.isDirty()) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    yesFunctionToCall.call();
                },
                function () { // No
                    noFunctionToCall.call();
                },
                $.validator.format(globalResource.SaveConfirm));
        } else {
            yesFunctionToCall.call();
        }
    },
    setFocusToFiscalYear: function () {
        sg.utls.focus("Data_ThroughCustomerYear");
    },
};

var clearStatisticsUtilities = {
    customerYearBackup: null,
    customerGroupYearBackup: null,
    nationalAcctYearBackup: null,
    salespersonYearBackup: null,
    itemYearBackup: null,
    customerPeriodBackup: null,
    customerGroupPeriodBackup: null,
    nationalAcctPeriodBackup: null,
    salespersonPeriodBackup: null,
    itemPeriodBackup: null,
    backupCustomerYearValue: function () {
        clearStatisticsUtilities.customerYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
    },
    backupGroupYearValue: function() {
        clearStatisticsUtilities.customerGroupYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
    },
    backupNationalAcctYearValue: function () {
        clearStatisticsUtilities.nationalAcctYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear();
    },
    backupSalespersonYearValue: function () {
        clearStatisticsUtilities.salespersonYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear();
    },
    backupItemYearValue: function () {
        clearStatisticsUtilities.itemYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
    },
    backupCustomerPeriodValue: function() {
        clearStatisticsUtilities.customerPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
    },
    backupGroupPeriodValue: function () {
        clearStatisticsUtilities.customerGroupPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
    },
    backupNationalAcctPeriodValue: function () {
        clearStatisticsUtilities.nationalAcctPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctPeriod();
    },
    backupSalespersonPeriodValue: function () {
        clearStatisticsUtilities.salespersonPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonPeriod();
    },
    backupItemPeriodValue: function () {
        clearStatisticsUtilities.itemPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod();
    },
    refreshCustomerStatistic: function () {
        clearStatisticsUtilities.backupCustomerPeriodValue();
        clearStatisticsUtilities.backupCustomerYearValue();
    },
    refreshGroupStatistic: function () {
        clearStatisticsUtilities.backupGroupPeriodValue();
        clearStatisticsUtilities.backupGroupYearValue();
    },
    refreshNationalAcctStatistic: function () {
        clearStatisticsUtilities.backupNationalAcctPeriodValue();
        clearStatisticsUtilities.backupNationalAcctYearValue();
    },
    refreshSalespersonStatistic: function () {
        clearStatisticsUtilities.backupSalespersonPeriodValue();
        clearStatisticsUtilities.backupSalespersonYearValue();
    },
    refreshItemStatistic: function () {
        clearStatisticsUtilities.backupItemPeriodValue();
        clearStatisticsUtilities.backupItemYearValue();
    },  
    validateCustomerYear: function () {
        clearStatisticsRepository.getCustomerMaxPeriodForValidYear($("#Data_ThroughCustomerYear").val());
    },
    validateGroupYear: function () {
        clearStatisticsRepository.getGroupMaxPeriodForValidYear($("#Data_ThroughGroupYear").val());
    },
    validateNationalAcctYear: function () {
        clearStatisticsRepository.getNationalAcctMaxPeriodForValidYear($("#Data_ThroughNationalAcctYear").val());
    },
    validateSalespersonYear: function () {
        clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalesPersonYear").val());
    },
    validateItemYear: function () {
        clearStatisticsRepository.getItemMaxPeriodForValidYear($("#Data_ThroughItemYear").val());
    },
    validateCustomerPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughCustomerPeriod").val();
        var oldPeriod = clearStatisticsUtilities.customerPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.MaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.CustomerStatisticsCurrentPeriod(clearStatisticsUtilities.customerPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod(clearStatisticsUtilities.customerPeriodBackup);           
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshCustomerStatistic, clearStatisticsUtility.setFocusToFiscalYear);
        }
    },
    validateGroupPeriod: function () {
    sg.utls.clearValidations("frmClearStatistics");
    var period = $("#Data_ThroughGroupPeriod").val();
    var oldPeriod = clearStatisticsUtilities.customerGroupPeriodBackup;
    if ((parseInt(clearStatisticsUI.clearStatisticsModel.MaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
        clearStatisticsUI.clearStatisticsModel.CustomerStatisticsCurrentPeriod(clearStatisticsUtilities.customerGroupPeriodBackup);
        clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod(clearStatisticsUtilities.customerGroupPeriodBackup);
    } else if (oldPeriod != period) {
        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshGroupStatistic, clearStatisticsUtility.setFocusToFiscalYear);
    }
    },
    validateNationalAcctPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughNationalAcctPeriod").val();
        var oldPeriod = clearStatisticsUtilities.nationalAcctPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.MaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.CustomerStatisticsCurrentPeriod(clearStatisticsUtilities.nationalAcctPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctPeriod(clearStatisticsUtilities.nationalAcctPeriodBackup);
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshNationalAcctStatistic, clearStatisticsUtility.setFocusToFiscalYear);
        }
    },
    validateSalespersonPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughSalesPersonPeriod").val();
        var oldPeriod = clearStatisticsUtilities.salespersonPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.SalesPersonStatisticsCurrentPeriod(clearStatisticsUtilities.salespersonPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonPeriod(clearStatisticsUtilities.salespersonPeriodBackup);
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshSalespersonStatistic, clearStatisticsUtility.setFocusToFiscalYear);
        }
    },
    validateItemPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughItemPeriod").val();
        var oldPeriod = clearStatisticsUtilities.itemPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.ItemMaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.ItemStatisticsCurrentPeriod(clearStatisticsUtilities.itemPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod(clearStatisticsUtilities.itemPeriodBackup);
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshItemStatistic, clearStatisticsUtility.setFocusToFiscalYear);
        }
    }
    
};

var clearStatisticsUISuccess = {    
    fillCustomerFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear(clearStatisticsUtilities.customerYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.MaximumPeriod(result);
            clearStatisticsUtilities.backupCustomerYearValue();
            setTimeout(function () {
                ($("#Data_ThroughCustomerPeriod")).siblings('input:visible').focus();
            });
        }
        clearStatisticsUtilities.refreshCustomerStatistic();
    },
    fillGroupFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(clearStatisticsUtilities.customerGroupYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.MaximumPeriod(result);
            clearStatisticsUtilities.backupGroupYearValue();
            setTimeout(function () {
                ($("#Data_ThroughGroupPeriod")).siblings('input:visible').focus();
            });
        }
        clearStatisticsUtilities.refreshGroupStatistic();
    },
    fillNationalAcctFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAcctYear(clearStatisticsUtilities.nationalAcctYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.MaximumPeriod(result);
            clearStatisticsUtilities.backupNationalAcctYearValue();
            setTimeout(function () {
                ($("#Data_ThroughNationalAcctPeriod")).siblings('input:visible').focus();
            });
        }
        clearStatisticsUtilities.refreshNationalAcctStatistic();
    },
    fillSalespersonFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalesPersonYear(clearStatisticsUtilities.salespersonYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod(result);
            clearStatisticsUtilities.backupSalespersonYearValue();
            setTimeout(function () {
            ($("#Data_ThroughSalesPersonPeriod")).siblings('input:visible').focus();
            });
        }
        
        clearStatisticsUtilities.refreshSalespersonStatistic();
        
    },
    fillItemFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(clearStatisticsUtilities.itemYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.ItemMaximumPeriod(result);
            clearStatisticsUtilities.backupItemYearValue();
            setTimeout(function () {
                ($("#Data_ThroughItemPeriod")).siblings('input:visible').focus();
            });
        }
        clearStatisticsUtilities.refreshItemStatistic();
    }
    
};

// Finder Cancel method
var onFinderCancel = {
    FromCustomerFinder: function () {
        sg.controls.Focus($("#Data_FromCustomerNo"));
    },
    ToCustomerFinder: function () {
        sg.controls.Focus($("#Data_ToCustomerNo"));
    },
    FromCustomerGroupFinder: function () {
        sg.controls.Focus($("#Data_FromGroupCode"));
    },
    ToCustomerGroupFinder: function () {
        sg.controls.Focus($("#Data_ToGroupCode"));
    },
    FromNationalAccountFinder: function () {
        sg.controls.Focus($("#Data_FromNationalAccount"));
    },
    ToNationalAccountFinder: function () {
        sg.controls.Focus($("#Data_ToNationalAccount"));
    },
    FromSalespersonFinder: function () {
        sg.controls.Focus($("#Data_FromSalesPerson"));
    },
    ToSalespersonFinder: function () {
        sg.controls.Focus($("#Data_ToSalesPerson"));
    },
    FromItemFinder: function () {
        sg.controls.Focus($("#Data_FromItem"));
    },
    ToItemFinder: function () {
        sg.controls.Focus($("#Data_ToItem"));
    },
    CustomerYearFinder: function () {
        setTimeout(function() {
            sg.controls.Focus($("#Data_ThroughCustomerYear"));
        })
    },
    CustomerGroupYearFinder: function () {
        setTimeout(function() {
            sg.controls.Focus($("#Data_ThroughGroupYear"));
        })
    },
    NationalAcctYearFinder: function () {
        setTimeout(function() {
            sg.controls.Focus($("#Data_ThroughNationalAcctYear"));
        })
    },
    SalespersonYearFinder: function () {
        
            sg.controls.Focus($("#Data_ThroughSalesPersonYear"));
        
    },
    ItemYearFinder: function () {
        setTimeout(function() {
            sg.controls.Focus($("#Data_ThroughItemYear"));
        })
    }    
};


$(function () {
    clearStatisticsUI.init();
});