// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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

"use strict";

var clearStatisticsUI = clearStatisticsUI || {}
var minimumFiscalYear = 1900;

clearStatisticsUI = {
    clearStatisticsModel: {},
    computedProperties: ["bClearCustomerStatistics", "bClearGroupStatistics", "bClearNationalAccountStatistics", "bClearSalespersonStatistics"],
    customerFiscalYear: null,
    customerGroupFiscalYear: null,
    nationalAccountFiscalYear: null,
    salespersonFiscalYear: null,
    itemFiscalYear: null,
    customerFiscalPeriod: null,
    customerGroupFiscalPeriod: null,
    nationalAccountFiscalPeriod: null,
    salespersonFiscalPeriod: null,
    itemFiscalPeriod: null,

    /**
    * @name init
    * @description Initialize the controls and apply kendo bindings
    * @namespace clearStatisticsUI
    * @public
    */
    init: () => {
        // Initialize the controls and apply kendo bindings 
        clearStatisticsUI.initKendoBindings();
        clearStatisticsUI.initGrids();
        clearStatisticsUI.initTabs();
        clearStatisticsUI.initFinders();
        clearStatisticsUI.initButtons();
        clearStatisticsUI.initNumericTextboxes();
        clearStatisticsUI.initTextboxes();
        clearStatisticsUI.initTimePickers();
        clearStatisticsUI.initCheckBoxes();
        clearStatisticsUI.initBlur();
        clearStatisticsUI.initProcessUI();
        ko.applyBindings(clearStatisticsUI.clearStatisticsModel);

        clearStatisticsUI.refreshTextboxes();
    },

    /**
     * @name refreshTextboxes
     * @description Refresh the values in the period textboxes
     *              This is done as a result of an existing defect
     *              in the Kendo libraries.
     * @namespace clearStatisticsUI
     * @public
     */
    refreshTextboxes: () => {
        clearStatisticsUI.refreshTextbox('Data_ThroughCustomerPeriod');
        clearStatisticsUI.refreshTextbox('Data_ThroughGroupPeriod');
        clearStatisticsUI.refreshTextbox('Data_ThroughNationalAccountPeriod');
        clearStatisticsUI.refreshTextbox('Data_ThroughSalespersonPeriod');
        clearStatisticsUI.refreshTextbox('Data_ThroughItemPeriod');
    },

    /**
     * @name refreshTextbox
     * @description Refresh the values in a period textbox
     *              This is done as a result of an existing defect
     *              in the Kendo libraries.
     * @namespace clearStatisticsUI
     * @public
     */
    refreshTextbox: (controlId) => {
        let control = $(`#${controlId}`).data("kendoNumericTextBox");
        control.value($(`#${controlId}`).val());
    },

    /**
     * @name initKendoBindings
     * @description Initialize the Kendo bindings
     * @namespace clearStatisticsUI
     * @public
     */
    initKendoBindings: () => {
        clearStatisticsUI.clearStatisticsModel = ko.mapping.fromJS(ClearStatisticsViewModel);
        clearStatisticsUI.customerFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
        clearStatisticsUI.customerGroupFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
        clearStatisticsUI.nationalAccountFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear();
        clearStatisticsUI.salespersonFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear();
        clearStatisticsUI.itemFiscalYear = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
        clearStatisticsUtilities.customerYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
        clearStatisticsUtilities.customerGroupYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
        clearStatisticsUtilities.nationalAccountYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear();
        clearStatisticsUtilities.salespersonYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear();
        clearStatisticsUtilities.itemYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
        clearStatisticsUI.customerFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
        clearStatisticsUI.customerGroupFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
        clearStatisticsUI.nationalAccountFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountPeriod();
        clearStatisticsUI.salespersonFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonPeriod();
        clearStatisticsUI.itemFiscalPeriod = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod();
        clearStatisticsUtilities.customerPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
        clearStatisticsUtilities.customerGroupPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
        clearStatisticsUtilities.nationalAccountPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountPeriod();
        clearStatisticsUtilities.salespersonPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonPeriod();
        clearStatisticsUtilities.itemPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemPeriod();
        tuClearStatisticsKoExtn.tuClearStatisticsModelExtension(clearStatisticsUI.clearStatisticsModel);
    },

    /**
     * @name initGrids
     * @description Initialize the grids, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initGrids: () => {

    },

    /**
     * @name initTabs
     * @description Initialize the tabs, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initTabs: () => {

    },

    /**
     * @name initFinders
     * @description Initialize all of the finders on the page, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initFinders: () => {
        // Initializing Customer Statistics Finder
        clearStatisticsUI.initCustomerNumberFinder();
        // Initializing Customer Group Statistics Finder
        clearStatisticsUI.initCustomerGroupFinder();
        // Initializing National Account Statistics Finder
        clearStatisticsUI.initNationalAccountFinder();
        // Initializing Salesperson Statistics Finder
        clearStatisticsUI.initSalespersonFinder();
        // Initializing Item Statistics Finder
        clearStatisticsUI.initItemFinder();
        // Initializing Fiscal Year Finder
        clearStatisticsUI.initFiscalYearFinder();
    },

    /**
     * @name initCustomerNumberFinder
     * @description Initialize the Customer Number finders
     * @namespace clearStatisticsUI
     * @public
     */
    initCustomerNumberFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFromCustomerFinder", "Data_FromCustomerNumber", sg.viewFinderProperties.AR.Customers, onFinderSuccess.FromCustomerFinder, onFinderCancel.FromCustomerFinder);
        sg.viewFinderHelper.setViewFinderEx("btnToCustomerFinder", "Data_ToCustomerNumber", sg.viewFinderProperties.AR.Customers, onFinderSuccess.ToCustomerFinder, onFinderCancel.ToCustomerFinder);
    },

    /**
     * @name initCustomerGroupFinder
     * @description Initialize the Customer Group finders
     * @namespace clearStatisticsUI
     * @public
     */
    initCustomerGroupFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFromCustomerGroupFinder", "Data_FromGroupCode", sg.viewFinderProperties.AR.CustomerGroups, onFinderSuccess.FromCustomerGroupFinder, onFinderCancel.FromCustomerGroupFinder);
        sg.viewFinderHelper.setViewFinderEx("btnToCustomerGroupFinder", "Data_ToGroupCode", sg.viewFinderProperties.AR.CustomerGroups, onFinderSuccess.ToCustomerGroupFinder, onFinderCancel.ToCustomerGroupFinder);
    },

    /**
     * @name initNationalAccountFinder
     * @description Initialize the National Account finders
     * @namespace clearStatisticsUI
     * @public
     */
    initNationalAccountFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFromNationalAccountFinder", "Data_FromNationalAccount", sg.viewFinderProperties.AR.NationalAccounts, onFinderSuccess.FromNationalAccountFinder, onFinderCancel.FromNationalAccountFinder);
        sg.viewFinderHelper.setViewFinderEx("btnToNationalAccountFinder", "Data_ToNationalAccount", sg.viewFinderProperties.AR.NationalAccounts, onFinderSuccess.ToNationalAccountFinder, onFinderCancel.ToNationalAccountFinder);
    },

    /**
     * @name initSalespersonFinder
     * @description Initialize the Salesperson finders
     * @namespace clearStatisticsUI
     * @public
     */
    initSalespersonFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFromSalespersonFinder", "Data_FromSalesperson", sg.viewFinderProperties.AR.SalesPersons, onFinderSuccess.FromSalespersonFinder, onFinderCancel.FromSalespersonFinder);
        sg.viewFinderHelper.setViewFinderEx("btnToSalespersonFinder", "Data_ToSalesperson", sg.viewFinderProperties.AR.SalesPersons, onFinderSuccess.ToSalespersonFinder, onFinderCancel.ToSalespersonFinder);
    },

    /**
     * @name initFiscalYearFinder
     * @description Initialize the Fiscal Year finders
     * @namespace clearStatisticsUI
     * @public
     */
    initFiscalYearFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFindCustomerYear", "Data_ThroughCustomerYear", sg.viewFinderProperties.CS.FiscalCalendars, onFinderSuccess.CustomerYearFinder, onFinderCancel.CustomerYearFinder);
        sg.viewFinderHelper.setViewFinderEx("btnFindCustomerGroupYear", "Data_ThroughGroupYear", sg.viewFinderProperties.CS.FiscalCalendars, onFinderSuccess.CustomerGroupYearFinder, onFinderCancel.CustomerGroupYearFinder);
        sg.viewFinderHelper.setViewFinderEx("btnFindNationalAccountYear", "Data_ThroughNationalAccountYear", sg.viewFinderProperties.CS.FiscalCalendars, onFinderSuccess.NationalAccountYearFinder, onFinderCancel.NationalAccountYearFinder);
        sg.viewFinderHelper.setViewFinderEx("btnFindSalespersonYear", "Data_ThroughSalespersonYear", sg.viewFinderProperties.CS.FiscalCalendars, onFinderSuccess.SalespersonYearFinder, onFinderCancel.SalespersonYearFinder);
        sg.viewFinderHelper.setViewFinderEx("btnFindItemYear", "Data_ThroughItemYear", sg.viewFinderProperties.CS.FiscalCalendars, onFinderSuccess.ItemYearFinder, onFinderCancel.ItemYearFinder);
    },

    /**
     * @name initItemFinder
     * @description Initialize the Item finders
     * @namespace clearStatisticsUI
     * @public
     */
    initItemFinder: function () {
        sg.viewFinderHelper.setViewFinderEx("btnFromItemFinder", "Data_FromItem", sg.viewFinderProperties.AR.Items, onFinderSuccess.FromItemFinder, onFinderCancel.FromItemFinder);
        sg.viewFinderHelper.setViewFinderEx("btnToItemFinder", "Data_ToItem", sg.viewFinderProperties.AR.Items, onFinderSuccess.ToItemFinder, onFinderCancel.ToItemFinder);
    },

    /**
     * @name initNumericTextboxes
     * @description Initialize the numeric textboxes, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initNumericTextboxes: () => {
    },

    /**
     * @name initCheckBoxes
     * @description Initialize the click handlers for check boxes, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initCheckBoxes: () => {
        $("#Data_ClearCustomerStatistics").click(function (e) {
            if ($(this).is(':checked')) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromCustomerNumber"));
                });

            }
        });
        $(document).on("change", "#Data_ClearGroupStatistics", function () {
            if ($("#Data_ClearGroupStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromGroupCode"));
                });
            }
        });
        $(document).on("change", "#Data_ClearNationalAccountStatistics", function () {
            if ($("#Data_ClearNationalAccountStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromNationalAccount"));
                });
            }
        });
        $(document).on("change", "#Data_ClearSalespersonStatistics", function () {
            if ($("#Data_ClearSalespersonStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromSalesperson"));
                });
            }
        });
        $(document).on("change", "#Data_ClearItemStatistics", function () {
            if ($("#Data_ClearItemStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromItem"));
                });
            }
        });
    },

    /**
     * @name initBlur
     * @description Initialize the onChange handlers for various controls, if needed
     * @namespace clearStatisticsUI
     * @public
     */
    initBlur: () => {
        $("#Data_ThroughCustomerYear").on('change', function (e) {
            sg.delayOnChange("btnFindCustomerYear", $("Data_ThroughCustomerYear"), function () {
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

        $("#Data_ThroughGroupYear").on('change', function (e) {
            sg.delayOnChange("btnFindCustomerGroupYear", $("Data_ThroughGroupYear"), function () {
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

        $("#Data_ThroughNationalAccountYear").on('change', function (e) {
            sg.delayOnChange("btnFindNationalAccountYear", $("Data_ThroughNationalAccountYear"), function () {
                var year = $("#Data_ThroughNationalAccountYear").val();
                var oldYear = clearStatisticsUI.nationalAccountFiscalYear;
                if (clearStatisticsUI.clearStatisticsModel.CalendarYear()) {
                    if (year < minimumFiscalYear) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear(oldYear);
                        //clearStatisticsUtilities.revertYearValue();
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear(clearStatisticsUtilities.nationalAccountYearBackup);
                        $('#Data_ThroughNationalAccountYear').focus();
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshNationalAccountStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughNationalAccountYear").val()))) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear(clearStatisticsUtilities.nationalAccountYearBackup);
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateNationalAccountYear, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
            });
        });

        $("#Data_ThroughSalespersonYear").on('change', function (e) {
            sg.delayOnChange("btnFindSalespersonYear", $("Data_ThroughSalespersonYear"), function () {
                var year = $("#Data_ThroughSalespersonYear").val();
                var oldYear = clearStatisticsUI.salespersonFiscalYear;
                if (clearStatisticsUI.clearStatisticsModel.SalesCalendarYear()) {
                    if (year < minimumFiscalYear) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear(oldYear);
                        //clearStatisticsUtilities.revertYearValue();
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear(clearStatisticsUtilities.salespersonYearBackup);
                        $('#Data_ThroughSalespersonYear').focus();
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshSalespersonStatistic, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(clearStatisticsUI.fiscalYrExists($("#Data_ThroughSalespersonYear").val()))) {
                        clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear(clearStatisticsUtilities.salespersonYearBackup);
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateSalespersonYear, clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
            });
        });

        $("#Data_ThroughItemYear").bind('on', function (e) {
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

        $("#Data_ThroughCustomerPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateCustomerPeriod();
        });

        $("#Data_ThroughGroupPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateGroupPeriod();
        });

        $("#Data_ThroughNationalAccountPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateNationalAccountPeriod();
        });

        $("#Data_ThroughSalespersonPeriod").on('change', function (e) {
            $("#message").empty();
            // clearStatisticsUtilities.backupSalespersonPeriodValue();
            clearStatisticsUtilities.validateSalespersonPeriod();
        });

        $("#Data_ThroughItemPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateItemPeriod();
        });
    },

    /**
     * @name initTextboxes
     * @description Initialize the Kendo text boxes, if needed
     * @namespace clearStatisticsUI
     * @public
     */
    initTextboxes: () => {
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

        $("#Data_ThroughNationalAccountPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.MinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountPeriod(value);
                clearStatisticsUtilities.backupNationalAccountPeriodValue();
            }

        });

        $("#Data_ThroughSalespersonPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: clearStatisticsUI.clearStatisticsModel.SalesPersonMinimumPeriod(),
            max: clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonPeriod(value);
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

    /**
     * @name initTimePickers
     * @description Initialize the time pickers, if any
     * @namespace clearStatisticsUI
     * @public
     */
    initTimePickers: () => {
    },

    /**
     * @name initButtons
     * @description Initialize the button click handlers
     * @namespace clearStatisticsUI
     * @public
     */
    initButtons: () => {
        $("#btnProcess").click((e) => {
            sg.utls.SyncExecute(clearStatisticsUI.process);
        });
    },

    /**
     * @name process
     * @description Handler for the process button
     * @namespace clearStatisticsUI
     * @public
     */
    process: () => {
        // debugger;
        sg.utls.isProcessRunning = true;
        let processUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Process");

        var isChecked = clearStatisticsUI.clearStatisticsModel.Data.bClearCustomerStatistics()
            || clearStatisticsUI.clearStatisticsModel.Data.bClearGroupStatistics()
            || clearStatisticsUI.clearStatisticsModel.Data.bClearNationalAccountStatistics()
            || clearStatisticsUI.clearStatisticsModel.Data.bClearSalespersonStatistics()
            || clearStatisticsUI.clearStatisticsModel.Data.bClearItemStatistics();

        if (!isChecked) {
            // Do not process
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, clearStatisticsResources.NoProcessingOption);
            sg.utls.isProcessRunning = false;
        }

        // Check if form is valid
        if ($("#frmClearStatistics").valid() && sg.utls.isProcessRunning) {
            // Check Validations
            if (clearStatisticsUI.Validation()) {
                $("#message").empty();
                sg.utls.clearValidations("frmClearStatistics");
                sg.utls.isProcessRunning = true;
                let data = { model: ko.mapping.toJS(clearStatisticsUI.clearStatisticsModel, clearStatisticsUI.computedProperties) };
                sg.utls.ajaxPost(processUrl, data, onSuccess.process)
            }
        }
    },

    /**
     * @name initProcessUI
     * @description
     * @namespace clearStatisticsUI
     * @public
     */
    initProcessUI: () => {
        let progressUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Progress");
        let cancelUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Cancel");
        window.progressUI.init(progressUrl, cancelUrl, clearStatisticsUI.clearStatisticsModel, screenName, onSuccess.onProcessComplete);
    },

    /**
     * @name Validation
     * @description Page validator
     * @namespace clearStatisticsUI
     * @public
     */
    Validation: () => {
        let errorRangeMessage = "";
        let inputValid = true;

        // If FromCustomer is greater than ToCustomer, throw an exception
        if (clearStatisticsUI.clearStatisticsModel.Data.ClearCustomerStatistics() && (clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNumber() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNumber().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToCustomerNumber())) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.CustomerNumberTitle;
            sg.controls.Focus($("#Data_FromCustomerNumber"));
        }
        // If FromCustomerGroup is greater than ToCustomerGroup, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearGroupStatistics() && (clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToGroupCode())) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.CustomerGroupFinder;
            sg.controls.Focus($("#Data_FromGroupCode"));
        }
        // If FromNationalAccnt is greater than ToNationalAccnt, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearNationalAccountStatistics() && clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToNationalAccount()) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.NationalAccountNumberTitle;
            sg.controls.Focus($("#Data_FromNationalAccount"));
        }
        // If FromSalesperson is greater than ToSalesperson, throw an exception
        else if (clearStatisticsUI.clearStatisticsModel.Data.ClearSalespersonStatistics() && clearStatisticsUI.clearStatisticsModel.Data.FromSalesperson() != null &&
            clearStatisticsUI.clearStatisticsModel.Data.FromSalesperson().localeCompare(clearStatisticsUI.clearStatisticsModel.Data.ToSalesperson()) > 0) {
            inputValid = false;
            errorRangeMessage = clearStatisticsResources.SalesPersonFinderTitle;
            sg.controls.Focus($("#Data_FromSalesperson"));
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
    }
};

// Callbacks
var onSuccess = {
    /**
    * @name process
    * @description Process success
    * @namespace onSuccess
    * @public
    * 
    * @param {object} result - JSON result payload
    */
    process: (jsonResult) => {
        if (jsonResult.UserMessage.IsSuccess) {
            let model = clearStatisticsUI.clearStatisticsModel;
            window.ko.mapping.fromJS(jsonResult.WorkflowInstanceId, {}, model.WorkflowInstanceId);
            window.progressUI.progress();
        } else {
            sg.utls.showMessage(jsonResult);
        }
    },

    /**
    * @name onProcessComplete
    * @description
    * @namespace onSuccess
    * @public
    * 
    * @param {object} result - The result of the operation
    */
    onProcessComplete: (result) => {
        if (result.ProcessResult.Results.length <= 0) {
            $("#processingResultGrid").hide();
            let errorMessage = clearStatisticsResources.ProcessingComplete;
            sg.utls.showMessageInfoInCustomDivWithoutClose(sg.utls.msgType.INFO, errorMessage, 'messageDiv');
        }
    },
};

// Finder success method
var onFinderSuccess = {

    // variable for From Customer Number finder.
    FromCustomerFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ToCustomerNumber");
            const val = data.IDCUST;
            clearStatisticsUI.clearStatisticsModel.Data.FromCustomerNumber(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for To Customer Number finder.
    ToCustomerFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ThroughCustomerYear");
            const val = data.IDCUST;
            clearStatisticsUI.clearStatisticsModel.Data.ToCustomerNumber(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for From Customer Group finder.
    FromCustomerGroupFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ToGroupCode");
            const val = data.IDGRP;
            clearStatisticsUI.clearStatisticsModel.Data.FromGroupCode(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for To Customer Group finder.
    ToCustomerGroupFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ThroughGroupYear");
            const val = data.IDGRP;
            clearStatisticsUI.clearStatisticsModel.Data.ToGroupCode(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for From National Account finder.
    FromNationalAccountFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ToNationalAccount");
            const val = data.IDNATACCT;
            clearStatisticsUI.clearStatisticsModel.Data.FromNationalAccount(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for To National Account finder.
    ToNationalAccountFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ThroughNationalAccountYear");
            const val = data.IDNATACCT;
            clearStatisticsUI.clearStatisticsModel.Data.ToNationalAccount(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for From Salesperson finder.
    FromSalespersonFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ToSalesperson");
            const val = data.CODESLSP;
            clearStatisticsUI.clearStatisticsModel.Data.FromSalesperson(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for To Salesperson finder.
    ToSalespersonFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ThroughSalespersonYear");
            const val = data.CODESLSP;
            clearStatisticsUI.clearStatisticsModel.Data.ToSalesperson(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for From Item finder.
    FromItemFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ToItem");
            const val = data.IDITEM;
            clearStatisticsUI.clearStatisticsModel.Data.FromItemNumber(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for To Item finder.
    ToItemFinder: function (data) {
        if (data != null) {
            const $nextControl = $("#Data_ThroughItemYear");
            const val = data.IDITEM;
            clearStatisticsUI.clearStatisticsModel.Data.ToItemNumber(val);
            sg.controls.Focus($nextControl);
        }
    },

    // variable for Customer Year finder.
    CustomerYearFinder: function (data) {
        if (data) {
            var year = data.FSCYEAR;
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
            var year = data.FSCYEAR;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getGroupMaxPeriodForValidYear($("#Data_ThroughGroupYear").val())
            }
        }
    },

    // variable for National Account Year finder.
    NationalAccountYearFinder: function (data) {
        if (data) {
            var year = data.FSCYEAR;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getNationalAccountMaxPeriodForValidYear($("#Data_ThroughNationalAccountYear").val())
            }
        }
    },

    // variable for Salesperson Year finder.
    SalespersonYearFinder: function (data) {
        if (data) {
            var year = data.FSCYEAR;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalespersonYear").val())

            }

        }
    },

    // variable for Item Year finder.
    ItemYearFinder: function (data) {
        if (data) {
            var year = data.FSCYEAR;
            if (sg.controls.GetString(clearStatisticsUI.clearStatisticsModel.Data.Year()) !== sg.controls.GetString(year)) {
                clearStatisticsUI.clearStatisticsModel.Data.Year(year);
                clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear(year);
                //Now Validate the method year
                clearStatisticsRepository.getItemMaxPeriodForValidYear($("#Data_ThroughItemYear").val())
            }
        }
    }
};

// Utility
var clearStatisticsUtility = {
    /**
    * @name checkIsDirty
    * @description If the model data has changed, display confirmation dialog box
    * @namespace clearStatisticsUtility
    * @public
    * 
    * @param {object} yesFunctionToCall - Callback for Yes
    * @param {object} noFunctionToCall - Callback for No
    */
    checkIsDirty: (yesFunctionToCall, noFunctionToCall) => {
        if (clearStatisticsUI.clearStatisticsModel.IsKoClearStatisticsDirty && clearStatisticsUI.clearStatisticsModel.IsKoClearStatisticsDirty.isDirty()) {
            sg.utls.showKendoConfirmationDialog(
                () => { // Yes
                    yesFunctionToCall.call();
                },
                () => { // No
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
    nationalAccountYearBackup: null,
    salespersonYearBackup: null,
    itemYearBackup: null,
    customerPeriodBackup: null,
    customerGroupPeriodBackup: null,
    nationalAccountPeriodBackup: null,
    salespersonPeriodBackup: null,
    itemPeriodBackup: null,
    backupCustomerYearValue: function () {
        clearStatisticsUtilities.customerYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerYear();
    },
    backupGroupYearValue: function () {
        clearStatisticsUtilities.customerGroupYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupYear();
    },
    backupNationalAccountYearValue: function () {
        clearStatisticsUtilities.nationalAccountYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear();
    },
    backupSalespersonYearValue: function () {
        clearStatisticsUtilities.salespersonYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear();
    },
    backupItemYearValue: function () {
        clearStatisticsUtilities.itemYearBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughItemYear();
    },
    backupCustomerPeriodValue: function () {
        clearStatisticsUtilities.customerPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughCustomerPeriod();
    },
    backupGroupPeriodValue: function () {
        clearStatisticsUtilities.customerGroupPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughGroupPeriod();
    },
    backupNationalAccountPeriodValue: function () {
        clearStatisticsUtilities.nationalAccountPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountPeriod();
    },
    backupSalespersonPeriodValue: function () {
        clearStatisticsUtilities.salespersonPeriodBackup = clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonPeriod();
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
    refreshNationalAccountStatistic: function () {
        clearStatisticsUtilities.backupNationalAccountPeriodValue();
        clearStatisticsUtilities.backupNationalAccountYearValue();
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
    validateNationalAccountYear: function () {
        clearStatisticsRepository.getNationalAccountMaxPeriodForValidYear($("#Data_ThroughNationalAccountYear").val());
    },
    validateSalespersonYear: function () {
        clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalespersonYear").val());
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
    validateNationalAccountPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughNationalAccountPeriod").val();
        var oldPeriod = clearStatisticsUtilities.nationalAccountPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.MaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.CustomerStatisticsCurrentPeriod(clearStatisticsUtilities.nationalAccountPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountPeriod(clearStatisticsUtilities.nationalAccountPeriodBackup);
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshNationalAccountStatistic, clearStatisticsUtility.setFocusToFiscalYear);
        }
    },
    validateSalespersonPeriod: function () {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#Data_ThroughSalespersonPeriod").val();
        var oldPeriod = clearStatisticsUtilities.salespersonPeriodBackup;
        if ((parseInt(clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod()) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            clearStatisticsUI.clearStatisticsModel.SalesPersonStatisticsCurrentPeriod(clearStatisticsUtilities.salespersonPeriodBackup);
            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonPeriod(clearStatisticsUtilities.salespersonPeriodBackup);
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

// UI Callbacks
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
            clearStatisticsUI.clearStatisticsModel.Data.ThroughNationalAccountYear(clearStatisticsUtilities.nationalAccountYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.MaximumPeriod(result);
            clearStatisticsUtilities.backupNationalAccountYearValue();
            setTimeout(function () {
                ($("#Data_ThroughNationalAccountPeriod")).siblings('input:visible').focus();
            });
        }
        clearStatisticsUtilities.refreshNationalAcctStatistic();
    },
    fillSalespersonFiscalYear: function (result) {
        if (result == 0) {
            clearStatisticsUI.clearStatisticsModel.Data.ThroughSalespersonYear(clearStatisticsUtilities.salespersonYearBackup);
        } else {
            clearStatisticsUI.clearStatisticsModel.SalesPersonMaximumPeriod(result);
            clearStatisticsUtilities.backupSalespersonYearValue();
            setTimeout(function () {
                ($("#Data_ThroughSalespersonPeriod")).siblings('input:visible').focus();
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
        sg.controls.Focus($("#Data_FromCustomerNumber"));
    },
    ToCustomerFinder: function () {
        sg.controls.Focus($("#Data_ToCustomerNumber"));
    },
    FromCustomerGroupFinder: function () {
        //sg.controls.Focus($("#Data_FromGroupCode"));
        setTimeout(function () {
            sg.controls.Focus($("#Data_FromGroupCode"));
        })
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
        sg.controls.Focus($("#Data_FromSalesperson"));
    },
    ToSalespersonFinder: function () {
        sg.controls.Focus($("#Data_ToSalesperson"));
    },
    FromItemFinder: function () {
        sg.controls.Focus($("#Data_FromItem"));
    },
    ToItemFinder: function () {
        sg.controls.Focus($("#Data_ToItem"));
    },
    CustomerYearFinder: function () {
        setTimeout(function () {
            sg.controls.Focus($("#Data_ThroughCustomerYear"));
        })
    },
    CustomerGroupYearFinder: function () {
        setTimeout(function () {
            sg.controls.Focus($("#Data_ThroughGroupYear"));
        })
    },
    NationalAccountYearFinder: function () {
        setTimeout(function () {
            sg.controls.Focus($("#Data_ThroughNationalAccountYear"));
        })
    },
    SalespersonYearFinder: function () {

        sg.controls.Focus($("#Data_ThroughSalespersonYear"));

    },
    ItemYearFinder: function () {
        setTimeout(function () {
            sg.controls.Focus($("#Data_ThroughItemYear"));
        })
    }
};

// Initial Entry
$(() => {
    clearStatisticsUI.init();
});