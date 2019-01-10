// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

var clearStatisticsConstants = (function (self) {
    return {
        MODULEID: 'TU',
        ACTION: 'ClearStatistics',
        MINIMUMFISCALYEAR: 1900,
    };
})(clearStatisticsConstants || {});

var clearStatisticsUI = (function (self, $) {

    var _initialized = false;

    var _model = {};
    var computedProperties = ["bClearCustomerStatistics", "bClearGroupStatistics",
                              "bClearNationalAcctStatistics", "bClearSalespersonStatistics"];
    var customerFiscalYear = null;
    var customerGroupFiscalYear = null;
    var nationalAcctFiscalYear = null;
    var salespersonFiscalYear = null;
    var itemFiscalYear = null;
    var customerFiscalPeriod = null;
    var customerGroupFiscalPeriod = null;
    var nationalAcctFiscalPeriod = null;
    var salespersonFiscalPeriod = null;
    var itemFiscalPeriod = null;

    /**
     * @name initKendoBindings
     * @desc Initialize the Kendo bindings
     * @private
     */
    function initKendoBindings() {
        // Reduce code noise
        var utils = clearStatisticsUtilities;

        _model = ko.mapping.fromJS(ClearStatisticsViewModel);

        customerFiscalYear = _model.Data.ThroughCustomerYear();
        customerGroupFiscalYear = _model.Data.ThroughGroupYear();
        nationalAcctFiscalYear = _model.Data.ThroughNationalAcctYear();
        salespersonFiscalYear = _model.Data.ThroughSalesPersonYear();
        itemFiscalYear = _model.Data.ThroughItemYear();

        utils.customerYearBackup = _model.Data.ThroughCustomerYear();
        utils.customerGroupYearBackup = _model.Data.ThroughGroupYear();
        utils.nationalAcctYearBackup = _model.Data.ThroughNationalAcctYear();
        utils.salespersonYearBackup = _model.Data.ThroughSalesPersonYear();
        utils.itemYearBackup = _model.Data.ThroughItemYear();

        customerFiscalPeriod = _model.Data.ThroughCustomerPeriod();
        customerGroupFiscalPeriod = _model.Data.ThroughGroupPeriod();
        nationalAcctFiscalPeriod = _model.Data.ThroughNationalAcctPeriod();
        salespersonFiscalPeriod = _model.Data.ThroughSalesPersonPeriod();
        itemFiscalPeriod = _model.Data.ThroughItemPeriod();

        utils.customerPeriodBackup = _model.Data.ThroughCustomerPeriod();
        utils.customerGroupPeriodBackup = _model.Data.ThroughGroupPeriod();
        utils.nationalAcctPeriodBackup = _model.Data.ThroughNationalAcctPeriod();
        utils.salespersonPeriodBackup = _model.Data.ThroughSalesPersonPeriod();
        utils.itemPeriodBackup = _model.Data.ThroughItemPeriod();

        tuClearStatisticsKoExtn.tuClearStatisticsModelExtension(_model);
    }

    /**
     * @name initFinders
     * @desc Initialize all of the finders on the page
     * @private
     */
    function initFinders() {
        initCustomerNumberFinders();
        initCustomerGroupFinders();
        initNationalAcctFinders();
        initSalespersonFinders();
        initItemFinders();
        initFiscalYearFinders();
    }

    /**
     * @name initCustomerNumberFinders
     * @desc Initialize the Customer Number finders
     * @private
     */
    function initCustomerNumberFinders() {
        var props = sg.viewFinderProperties.ARCustomers;
        var controls = [
            { buttonId: "btnFromCustomerFinder", dataControlId: "Data_FromCustomerNo" },
            { buttonId: "btnToCustomerFinder", dataControlId: "Data_ToCustomerNo" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initCustomerGroupFinders
     * @desc Initialize the Customer Group finders
     * @private
     */
    function initCustomerGroupFinders() {
        var props = sg.viewFinderProperties.ARCustomerGroups;
        var controls = [
            { buttonId: "btnFromCustomerGroupFinder", dataControlId: "Data_FromGroupCode" },
            { buttonId: "btnToCustomerGroupFinder", dataControlId: "Data_ToGroupCode" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initNationalAcctFinders
     * @desc Initialize the National Accounts finders
     * @private
     */
    function initNationalAcctFinders() {
        var props = sg.viewFinderProperties.ARNationalAccounts;
        var controls = [
            { buttonId: "btnFromNationalAcctFinder", dataControlId: "Data_FromNationalAccount" },
            { buttonId: "btnToNationalAcctFinder", dataControlId: "Data_ToNationalAccount" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initSalespersonFinders
     * @desc Initialize the National Accounts finder
     * @private
     */
    function initSalespersonFinders() {
        var props = sg.viewFinderProperties.ARSalespersons;
        var controls = [
            { buttonId: "btnFromSalespersonFinder", dataControlId: "Data_FromSalesPerson" },
            { buttonId: "btnToSalespersonFinder", dataControlId: "Data_ToSalesPerson" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initItemFinders
     * @desc Initialize the Item finders
     * @private
     */
    function initItemFinders() {
        var props = sg.viewFinderProperties.ARItems;
        var controls = [
            { buttonId: "btnFromItemFinder", dataControlId: "Data_FromItem" },
            { buttonId: "btnToItemFinder", dataControlId: "Data_ToItem" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initFiscalYearFinders
     * @desc Initialize the Fiscal Year finders
     * @private
     */
    function initFiscalYearFinders() {

        var props = sg.viewFinderProperties.CSFiscalCalendars;
        var controls = [
            { buttonId: "btnFindCustomerYear", dataControlId: "Data_ThroughCustomerYear" },
            { buttonId: "btnFindCustomerGroupYear", dataControlId: "Data_ThroughGroupYear" },
            { buttonId: "btnFindNationalAcctYear", dataControlId: "Data_ThroughNationalAcctYear" },
            { buttonId: "btnFindSalespersonYear", dataControlId: "Data_ThroughSalesPersonYear" },
            { buttonId: "btnFindItemYear", dataControlId: "Data_ThroughItemYear" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name _initFinderGroup
     * @desc Generic routine to initialize a group of finders
     * @private
     * @param {array} controls - Array of objects containing button and data control names
     * @param {object} info - Object containing various settings for the finder
     * @param {object} filter = "" | The optional filter used to filter the finder results
     * @param {number} height = null | The optional height of the finder window
     * @param {number} top = null | The optional top location of the finder window
     */
    function _initFinderGroup(controls, info, filter = "", height = null, top = null) {
        for (var i = 0; i < controls.length; i++) {
            _initFinder(controls[i].buttonId, controls[i].dataControlId, info, filter, height, top);
        }
    }

    /**
     * @name _initFinder
     * @desc Generic routine to initialize an individual finder
     * @private
     * @param {string} buttonId - The Id of the button used to invoke the finder
     * @param {string} controlId - The underlying control that will receive the selected item
     * @param {object} info - Object containing various settings for the finder
     * @param {object} filter = "" | The optional filter used to filter the finder results
     * @param {number} height = null | The optional height of the finder window
     * @param {number} top = null | The optional top location of the finder window
     */
    function _initFinder(buttonId, controlId, info, filter = "", height = null, top = null) {

        let initKeyValues = [$("#" + controlId).val()];

        let initFinder = function (viewFinder) {
            viewFinder.viewID = info.viewID;
            viewFinder.viewOrder = info.viewOrder;
            viewFinder.displayFieldNames = info.displayFieldNames;
            viewFinder.returnFieldName = info.returnFieldName;

            // Optional 
            //     If omitted, the starting value is blank.
            viewFinder.initKeyValues = initKeyValues;

            // Optional
            //     Only useful for UIs such as Invoice Entry finder where you 
            //     want to restrict the entries to a specific batch
            viewFinder.filter = filter;
        };

        // Note:
        //   There are two different ways to initialize the finder:
        //   1. Specify a callback method to handle more complicated processing
        //   OR
        //   2. Specify a simple control id where the selected finder value will be sent.
        //
        // Scenario #1
        //
        //let onFinderOK = function (val) {
        //    if (val != null) {
        //        dataEntity(val);
        //        sg.controls.Focus($("#" + nextControlIdForFocus));
        //    }
        //}
        //sg.viewFinderHelper.setViewFinder(buttonId, onFinderOK, initFinder, height, top);

        // Scenario #2
        sg.viewFinderHelper.setViewFinder(buttonId, controlId, initFinder, height, top);
    }

    /**
     * @name initCheckBox
     * @desc Initialize the click handlers for check boxes
     * @private
     */
    function initCheckBox() {

        $("#Data_ClearCustomerStatistics").click(function (e) {
            if ($(this).is(':checked')) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromCustomerNo"));
                })
            }
        });

        $(document).on("change", "#Data_ClearGroupStatistics", function () {
            if ($("#Data_ClearGroupStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromGroupCode"));
                })
            }
        });

        $(document).on("change", "#Data_ClearNationalAcctStatistics", function () {
            if ($("#Data_ClearNationalAcctStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromNationalAccount"));
                })
            }
        });

        $(document).on("change", "#Data_ClearSalesPersonStatistics", function () {
            if ($("#Data_ClearSalesPersonStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromSalesPerson"));
                })
            }
        });

        $(document).on("change", "#Data_ClearItemStatistics", function () {
            if ($("#Data_ClearItemStatistics").is(":checked")) {
                setTimeout(function () {
                    sg.controls.Focus($("#Data_FromItem"));
                })
            }
        });
    }

    /**
     * @name initBlur
     * @desc Initialize the onChange handlers for various controls
     * @private
     */
    function initBlur() {
        $("#Data_FromCustomerNo").on('change', function (e) { 
            sg.controls.Focus($("#Data_ToCustomerNo"));
        });

        $("#Data_ToCustomerNo").on('change', function (e) {
            sg.controls.Focus($("#Data_ThroughCustomerYear"));
        });

        $("#Data_ThroughCustomerYear").on('change', function (e) {
            let $control = $("#Data_ThroughCustomerYear");
            sg.delayOnChange("btnFindCustomerYear", $control, function () {
                var validatePeriodForYear = true;
                var year = $control.val();
                var oldYear = customerFiscalYear;
                if (_model.CalendarYear()) {
                    if (year < clearStatisticsConstants.MINIMUMFISCALYEAR) {
                        _model.Data.ThroughCustomerYear(oldYear);
                        _model.Data.ThroughCustomerYear(clearStatisticsUtilities.customerYearBackup);
                        $control.focus();
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshCustomerStatistic,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                } else {
                    if (!(fiscalYrExists(year))) {
                        _model.Data.ThroughCustomerYear(clearStatisticsUtilities.customerYearBackup);
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateCustomerYear,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }

                if (validatePeriodForYear) {
                    clearStatisticsUtilities.validateCustomerYear();
                }
            });
        });

        $("#Data_FromGroupCode").on('change', function (e) {
            sg.controls.Focus($("#Data_ToGroupCode"));
        });

        $("#Data_ToGroupCode").on('change', function (e) {
            sg.controls.Focus($("#Data_ThroughGroupYear"));
        });

        $("#Data_ThroughGroupYear").on('change', function (e) {
            let $control = $("#Data_ThroughGroupYear");
            sg.delayOnChange("btnFindCustomerGroupYear", $control, function () {
                var validatePeriodForYear = true;
                var year = $control.val();
                var oldYear = customerGroupFiscalYear;
                if (_model.CalendarYear()) {
                    if (year < clearStatisticsConstants.MINIMUMFISCALYEAR) {
                        _model.Data.ThroughGroupYear(oldYear);
                        _model.Data.ThroughGroupYear(clearStatisticsUtilities.customerGroupYearBackup);
                        $control.focus();
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshGroupStatistic,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(fiscalYrExists(year))) {
                        _model.Data.ThroughGroupYear(clearStatisticsUtilities.customerGroupYearBackup);
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateGroupYear,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }

                if (validatePeriodForYear) {
                    clearStatisticsUtilities.validateGroupYear();
                }
            });
        });

        $("#Data_FromNationalAccount").on('change', function (e) {
            sg.controls.Focus($("#Data_ToNationalAccount"));
        });

        $("#Data_ToNationalAccount").on('change', function (e) {
            sg.controls.Focus($("#Data_ThroughNationalAcctYear"));
        });

        $("#Data_ThroughNationalAcctYear").on('change', function (e) {
            let $control = $("#Data_ThroughNationalAcctYear");
            sg.delayOnChange("btnFindNationalAcctYear", $control, function () {
                var validatePeriodForYear = true;
                var year = $control.val();
                var oldYear = nationalAcctFiscalYear;
                if (_model.CalendarYear()) {
                    if (year < clearStatisticsConstants.MINIMUMFISCALYEAR) {
                        _model.Data.ThroughNationalAcctYear(oldYear);
                        _model.Data.ThroughNationalAcctYear(clearStatisticsUtilities.nationalAcctYearBackup);
                        $control.focus();
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshNationalAcctStatistic,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(fiscalYrExists(year))) {
                        _model.Data.ThroughNationalAcctYear(clearStatisticsUtilities.nationalAcctYearBackup);
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateNationalAcctYear,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }

                if (validatePeriodForYear) {
                    clearStatisticsUtilities.validateNationalAcctYear();
                }
            });
        });

        $("#Data_FromSalesPerson").on('change', function (e) {
            sg.controls.Focus($("#Data_ToSalesPerson"));
        });

        $("#Data_ToSalesPerson").on('change', function (e) {
            sg.controls.Focus($("#Data_ThroughSalesPersonYear"));
        });

        $("#Data_ThroughSalesPersonYear").on('change', function (e) {
            let $control = $("#Data_ThroughSalesPersonYear");
            sg.delayOnChange("btnFindSalespersonYear", $control, function () {
                var validatePeriodForYear = true;
                var year = $control.val();
                var oldYear = salespersonFiscalYear;
                if (_model.SalesCalendarYear()) {
                    if (year < clearStatisticsConstants.MINIMUMFISCALYEAR) {
                        _model.Data.ThroughSalesPersonYear(oldYear);
                        _model.Data.ThroughSalesPersonYear(clearStatisticsUtilities.salespersonYearBackup);
                        $control.focus();
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshSalespersonStatistic,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(fiscalYrExists(year))) {
                        _model.Data.ThroughSalesPersonYear(clearStatisticsUtilities.salespersonYearBackup);
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateSalespersonYear,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }

                if (validatePeriodForYear) {
                    clearStatisticsUtilities.validateSalespersonYear();
                }
            });
        });

        $("#Data_FromItem").on('change', function (e) {
            sg.controls.Focus($("#Data_ToItem"));
        });

        $("#Data_ToItem").on('change', function (e) {
            sg.controls.Focus($("#Data_ThroughItemYear"));
        });

        $("#Data_ThroughItemYear").on('change', function (e) {
            let $control = $("#Data_ThroughItemYear");
            sg.delayOnChange("btnFindItemYear", $control, function () {
                var validatePeriodForYear = true;
                var year = $control.val();
                var oldYear = itemFiscalYear;
                if (_model.ItemCalendarYear()) {
                    if (year < clearStatisticsConstants.MINIMUMFISCALYEAR) {
                        _model.Data.ThroughItemYear(oldYear);
                        _model.Data.ThroughItemYear(clearStatisticsUtilities.itemYearBackup);
                        $control.focus();
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.refreshItemStatistic,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }
                else {
                    if (!(fiscalYrExists(year))) {
                        _model.Data.ThroughItemYear(clearStatisticsUtilities.itemYearBackup);
                        validatePeriodForYear = false;
                    } else {
                        clearStatisticsUtility.checkIsDirty(clearStatisticsUtilities.validateItemYear,
                            clearStatisticsUtility.setFocusToFiscalYear);
                    }
                }

                if (validatePeriodForYear) {
                    clearStatisticsUtilities.validateItemYear();
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

        $("#Data_ThroughNationalAcctPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateNationalAcctPeriod();
        });

        $("#Data_ThroughSalesPersonPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateSalespersonPeriod();
        });

        $("#Data_ThroughItemPeriod").on('change', function (e) {
            $("#message").empty();
            clearStatisticsUtilities.validateItemPeriod();
        });
    }

    /**
     * @name initTextBox
     * @desc Initialize the Kendo text boxes
     * @private
     */
    function initTextBox() {
        $("#Data_ThroughCustomerPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: _model.MinimumPeriod(),
            max: _model.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                _model.Data.ThroughCustomerPeriod(value);
                clearStatisticsUtilities.backupCustomerPeriodValue();
            }
        });

        $("#Data_ThroughGroupPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: _model.MinimumPeriod(),
            max: _model.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                _model.Data.ThroughGroupPeriod(value);
                clearStatisticsUtilities.backupGroupPeriodValue();
            }
        });

        $("#Data_ThroughNationalAcctPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: _model.MinimumPeriod(),
            max: _model.MaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                _model.Data.ThroughNationalAcctPeriod(value);
                clearStatisticsUtilities.backupNationalAcctPeriodValue();
            }

        });

        $("#Data_ThroughSalesPersonPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: _model.SalesPersonMinimumPeriod(),
            max: _model.SalesPersonMaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                _model.Data.ThroughSalesPersonPeriod(value);
                clearStatisticsUtilities.backupSalespersonPeriodValue();
            }
        });

        $("#Data_ThroughItemPeriod").kendoNumericTextBox({
            format: "00",
            spinners: true,
            min: _model.ItemMinimumPeriod(),
            max: _model.ItemMaximumPeriod(),
            step: "1",
            type: "number",
            spin: function () {
                var value = this.value();
                _model.Data.ThroughItemPeriod(value);
                clearStatisticsUtilities.backupItemPeriodValue();
            }
        });

    }

    /**
     * @name initButtons
     * @desc Initialize the button click handlers
     * @private
     */
    function initButtons() {
        $("#btnProcess").click(function (e) {
            debugger;
            sg.utls.SyncExecute(process);
        });
    }

    /**
     * @name process
     * @desc Handler for the process button
     * @private
     */
    function process() {
        debugger;
        sg.utls.isProcessRunning = true;
        var processUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Process");

        var isChecked = (_model.Data.bClearCustomerStatistics()
            || _model.Data.bClearGroupStatistics()
            || _model.Data.bClearNationalAcctStatistics()
            || _model.Data.bClearSalespersonStatistics()
            || _model.Data.bClearItemStatistics());

        if (!isChecked) {
            // Do not process
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, clearStatisticsResources.NoProcessingOption);
            sg.utls.isProcessRunning = false;
        }

        // Check if form is valid
        if ($("#frmClearStatistics").valid() && sg.utls.isProcessRunning) {
            // Check Validations
            if (Validation()) {
                $("#message").empty();
                sg.utls.clearValidations("frmClearStatistics");
                sg.utls.isProcessRunning = true;
                var data = { model: ko.mapping.toJS(_model, computedProperties) };
                sg.utls.ajaxPost(processUrl, data, onSuccess.process)
            }
        }
    }

    /**
     * @name initProcessUI
     * @desc 
     * @private
     */
    function initProcessUI() {
        var progressUrl = sg.utls.url.buildUrl(clearStatisticsConstants.MODULEID, clearStatisticsConstants.ACTION, "Progress");
        var cancelUrl = sg.utls.url.buildUrl(clearStatisticsConstants.MODULEID, clearStatisticsConstants.ACTION, "Cancel");
        // @ts-ignore
        window.progressUI.init(progressUrl, cancelUrl, _model, screenName, onSuccess.onProcessComplete);
    }

    /**
     * @name Validation
     * @desc Page validator
     * @private
     */
    function Validation() {
        var errorRangeMessage = "";
        var inputValid = true;
        // @ts-ignore
        var resources = clearStatisticsResources;

        // If FromCustomer is greater than ToCustomer, throw an exception
        if (_model.Data.ClearCustomerStatistics() && (_model.Data.FromCustomerNo() != null &&
            _model.Data.FromCustomerNo().localeCompare(_model.Data.ToCustomerNo())) > 0) {
            inputValid = false;
            errorRangeMessage = resources.CustomerNumberTitle;
            sg.controls.Focus($("#Data_FromCustomerNo"));
        }

        // If FromCustomerGroup is greater than ToCustomerGroup, throw an exception
        else if (_model.Data.ClearGroupStatistics() && (_model.Data.FromGroupCode() != null &&
            _model.Data.FromGroupCode().localeCompare(_model.Data.ToGroupCode())) > 0) {
            inputValid = false;
            errorRangeMessage = resources.CustomerGroupFinder;
            sg.controls.Focus($("#Data_FromGroupCode"));
        }

        // If FromNationalAccnt is greater than ToNationalAccnt, throw an exception
        else if (_model.Data.ClearNationalAcctStatistics() && _model.Data.FromNationalAccount() != null &&
            _model.Data.FromNationalAccount().localeCompare(_model.Data.ToNationalAccount()) > 0) {
            inputValid = false;
            errorRangeMessage = resources.NationalAccountNumberTitle;
            sg.controls.Focus($("#Data_FromNationalAccount"));
        }

        // If FromSalesPerson is greater than ToSalesPerson, throw an exception
        else if (_model.Data.ClearSalesPersonStatistics() && _model.Data.FromSalesPerson() != null &&
            _model.Data.FromSalesPerson().localeCompare(_model.Data.ToSalesPerson()) > 0) {
            inputValid = false;
            errorRangeMessage = resources.SalesPersonFinderTitle;
            sg.controls.Focus($("#Data_FromSalesPerson"));
        }

        // If FromItem is greater than ToItem, throw an exception
        else if (_model.Data.ClearItemStatistics() && _model.Data.FromItemNumber() != null &&
            _model.Data.FromItemNumber().localeCompare(_model.Data.ToItemNumber()) > 0) {
            inputValid = false;
            errorRangeMessage = resources.ItemFinderTitle;
            sg.controls.Focus($("#Data_FromItem"));
        }

        if (!inputValid) {
            if (errorRangeMessage != "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(resources.ErrorFromToValueMessage, errorRangeMessage));
            } else if (errorMessage != "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
            }
        }

        return inputValid;
    }

    /**
     * @name fiscalYrExists
     * @desc Check to see if a year is a FiscalYear
     * @private
     * @param Year
     * @returns {boolean} true | false
     */
    function fiscalYrExists(Year) {
        if (_model.FiscalCalendars() === null) {
            return false;
        }

        for (var i = 0; i < _model.FiscalCalendars().length; i++) {
            if (Year === _model.FiscalCalendars()[i].Year()) {
                return true;
            }
        }
    }

    /**
     * @name setInitialized
     * @desc set or unset the _initialized flag
     * @private
     * @param {boolean} true | false
     */
    function setInitialized(init) {
        _initialized = init;
    }

    return {

        /**
         * @name init
         * @desc Initialize the controls and apply kendo bindings
         * @public
         */
        init: function () {
            initKendoBindings();
            initFinders();
            initButtons();
            initTextBox();
            initCheckBox();
            initBlur();
            initProcessUI();
            ko.applyBindings(_model);
            setInitialized(true);
        },

        /**
         * @name getInitialized
         * @desc Check to see if page has been initialized
         * @public
         * @returns {boolean} _initialized
         */
        getInitialized: function () {
            return _initialized;
        },

        /**
         * @name getModel
         * @desc Get the _model property
         * @public
         * @returns {object} _model
         */
        getModel: function () {
            return _model;
        },
    };

})(clearStatisticsUI || {}, jQuery);

var onSuccess = (function (self, $) {

    return {
        /**
         * @name process
         * @desc
         * @public
         * @param {object} jsonResult - The result of the operation
         */
        process: function (jsonResult) {
            if (jsonResult.UserMessage.IsSuccess) {
                var model = clearStatisticsUI.getModel();
                window.ko.mapping.fromJS(jsonResult.WorkflowInstanceId, {}, model.WorkflowInstanceId);
                window.progressUI.progress();
            } else {
                sg.utls.showMessage(jsonResult);
            }
        },

        /**
         * @name onProcessComplete
         * @desc
         * @public
         * @param {object} result - The result of the operation
         */
        onProcessComplete: function (result) {
            if (result.ProcessResult.Results.length <= 0) {
                $("#processingResultGrid").hide();
                var errorMessage = clearStatisticsResources.ProcessingComplete;
                sg.utls.showMessageInfoInCustomDivWithoutClose(sg.utls.msgType.INFO, errorMessage, 'messageDiv');
            }
        },
    };
})(onSuccess || {}, jQuery);

var clearStatisticsUtility = (function (self, $) {
    return {
        /**
         * @name checkIsDirty
         * @desc
         * @public
         * @param {object} yesFunctionToCall - Callback for Yes
         * @param {object} noFunctionToCall - Callback for No
         */
        checkIsDirty: function (yesFunctionToCall, noFunctionToCall) {
            if (clearStatisticsUI.getModel().IsKoStatisticsDirty && clearStatisticsUI.getModel().IsKoStatisticsDirty.isDirty()) {
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

        /**
         * @name setFocusToFiscalYear
         * @desc Set the current cursor focus to the 'Through Customer Year' field
         * @public
         */
        setFocusToFiscalYear: function () {
            sg.utls.focus("Data_ThroughCustomerYear");
        },
    };
})(clearStatisticsUtility || {}, jQuery);

var clearStatisticsUtilities = (function (self, $) {

    var customerYearBackup = null;
    var customerGroupYearBackup = null;
    var nationalAcctYearBackup = null;
    var salespersonYearBackup = null;
    var itemYearBackup = null;
    var customerPeriodBackup = null;
    var customerGroupPeriodBackup = null;
    var nationalAcctPeriodBackup = null;
    var salespersonPeriodBackup = null;
    var itemPeriodBackup = null;

    /**
     * @name _validationPeriod
     * @description Method used by other validation methods in this object
     * @private
     * @param {string} dataItemId
     * @param {number} periodBackup
     * @param {number} maxPeriod
     * @param {object} modelCurrentPeriodMethod - Method Name
     * @param {object} throughPeriodMethod - Method Name
     * @param {object} refreshStatisticMethod - Method Name
     */
    function _validatePeriod(dataItemId, periodBackup, maxPeriod, modelCurrentPeriodMethod, throughPeriodMethod, refreshStatisticMethod) {
        sg.utls.clearValidations("frmClearStatistics");
        var period = $("#" + dataItemId).val();
        var oldPeriod = periodBackup;
        if ((parseInt(maxPeriod) < parseInt(period)) || (period == "") || (period == "00") || ((period == "0"))) {
            modelCurrentPeriodMethod(periodBackup);
            throughPeriodMethod(periodBackup);
        } else if (oldPeriod != period) {
            clearStatisticsUtility.checkIsDirty(refreshStatisticMethod, clearStatisticsUtility.setFocusToFiscalYear);
        }
    }

    return {

        customerYearBackup: customerYearBackup,
        customerGroupYearBackup: customerGroupYearBackup,
        nationalAcctYearBackup: nationalAcctYearBackup,
        salespersonYearBackup: salespersonYearBackup,
        itemYearBackup: itemYearBackup,
        customerPeriodBackup: customerPeriodBackup,
        customerGroupPeriodBackup: customerGroupPeriodBackup,
        nationalAcctPeriodBackup: nationalAcctPeriodBackup,
        salespersonPeriodBackup: salespersonPeriodBackup,
        itemPeriodBackup: itemPeriodBackup,

        /**
         * @name backupCustomerYearValue
         * @description 
         * @private
         */
        backupCustomerYearValue: function () { 
            customerYearBackup = clearStatisticsUI.getModel().Data.ThroughCustomerYear();
        },

        /**
         * @name backupGroupYearValue
         * @description 
         * @private
         */
        backupGroupYearValue: function () {
            customerGroupYearBackup = clearStatisticsUI.getModel().Data.ThroughGroupYear();
        },

        /**
         * @name backupNationalAcctYearValue
         * @description 
         * @private
         */
        backupNationalAcctYearValue: function () {
            nationalAcctYearBackup = clearStatisticsUI.getModel().Data.ThroughNationalAcctYear();
        },

        /**
         * @name backupSalespersonYearValue
         * @description 
         * @private
         */
        backupSalespersonYearValue: function () {
            salespersonYearBackup = clearStatisticsUI.getModel().Data.ThroughSalesPersonYear();
        },

        /**
         * @name backupItemYearValue
         * @description 
         * @private
         */
        backupItemYearValue: function () {
            itemYearBackup = clearStatisticsUI.getModel().Data.ThroughItemYear();
        },

        /**
         * @name backupCustomerPeriodValue
         * @description 
         * @private
         */
        backupCustomerPeriodValue: function () {
            customerPeriodBackup = clearStatisticsUI.getModel().Data.ThroughCustomerPeriod();
        },

        /**
         * @name backupGroupPeriodValue
         * @description 
         * @private
         */
        backupGroupPeriodValue: function () {
            customerGroupPeriodBackup = clearStatisticsUI.getModel().Data.ThroughGroupPeriod();
        },

        /**
         * @name backupNationalAcctPeriodValue
         * @description 
         * @private
         */
        backupNationalAcctPeriodValue: function () {
            nationalAcctPeriodBackup = clearStatisticsUI.getModel().Data.ThroughNationalAcctPeriod();
        },

        /**
         * @name backupSalespersonPeriodValue
         * @description 
         * @private
         */
        backupSalespersonPeriodValue: function () {
            salespersonPeriodBackup = clearStatisticsUI.getModel().Data.ThroughSalesPersonPeriod();
        },

        /**
         * @name backupItemPeriodValue
         * @description 
         * @private
         */
        backupItemPeriodValue: function () {
            itemPeriodBackup = clearStatisticsUI.getModel().Data.ThroughItemPeriod();
        },

        /**
         * @name refreshCustomerStatistic
         * @description 
         * @private
         */
        refreshCustomerStatistic: function () {
            clearStatisticsUtilities.backupCustomerPeriodValue();
            clearStatisticsUtilities.backupCustomerYearValue();
        },

        /**
         * @name refreshGroupStatistic
         * @description 
         * @private
         */
        refreshGroupStatistic: function () {
            clearStatisticsUtilities.backupGroupPeriodValue();
            clearStatisticsUtilities.backupGroupYearValue();
        },

        /**
         * @name refreshNationalAcctStatistic
         * @description 
         * @private
         */
        refreshNationalAcctStatistic: function () {
            clearStatisticsUtilities.backupNationalAcctPeriodValue();
            clearStatisticsUtilities.backupNationalAcctYearValue();
        },

        /**
         * @name refreshSalespersonStatistic
         * @description 
         * @private
         */
        refreshSalespersonStatistic: function () {
            clearStatisticsUtilities.backupSalespersonPeriodValue();
            clearStatisticsUtilities.backupSalespersonYearValue();
        },

        /**
         * @name refreshItemStatistic
         * @description 
         * @private
         */
        refreshItemStatistic: function () {
            clearStatisticsUtilities.backupItemPeriodValue();
            clearStatisticsUtilities.backupItemYearValue();
        },

        /**
         * @name validateCustomerYear
         * @description 
         * @private
         */
        validateCustomerYear: function () {
            clearStatisticsRepository.getCustomerMaxPeriodForValidYear($("#Data_ThroughCustomerYear").val());
        },

        /**
         * @name validateGroupYear
         * @description 
         * @private
         */
        validateGroupYear: function () {
            clearStatisticsRepository.getGroupMaxPeriodForValidYear($("#Data_ThroughGroupYear").val());
        },

        /**
         * @name validateNationalAcctYear
         * @description 
         * @private
         */
        validateNationalAcctYear: function () {
            clearStatisticsRepository.getNationalAcctMaxPeriodForValidYear($("#Data_ThroughNationalAcctYear").val());
        },

        /**
         * @name validateSalespersonYear
         * @description 
         * @private
         */
        validateSalespersonYear: function () {
            clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalesPersonYear").val());
        },

        /**
         * @name validateItemYear
         * @description 
         * @private
         */
        validateItemYear: function () {
            clearStatisticsRepository.getItemMaxPeriodForValidYear($("#Data_ThroughItemYear").val());
        },

        /**
         * @name validateCustomerPeriod
         * @description 
         * @private
         */
        validateCustomerPeriod: function () {
            _validatePeriod("Data_ThroughCustomerPeriod", customerPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughCustomerPeriod,
                clearStatisticsUtilities.refreshCustomerStatistic);
        },

        /**
         * @name validateGroupPeriod
         * @description 
         * @private
         */
        validateGroupPeriod: function () {
            _validatePeriod("Data_ThroughGroupPeriod", customerGroupPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughGroupPeriod,
                clearStatisticsUtilities.refreshGroupStatistic);
        },

        /**
         * @name validateNationalAcctPeriod
         * @description 
         * @private
         */
        validateNationalAcctPeriod: function () {
            _validatePeriod("Data_ThroughNationalAcctPeriod", nationalAcctPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughNationalAcctPeriod,
                clearStatisticsUtilities.refreshNationalAcctStatistic);
        },

        /**
         * @name validateSalespersonPeriod
         * @description 
         * @private
         */
        validateSalespersonPeriod: function () {
            _validatePeriod("Data_ThroughSalesPersonPeriod", salespersonPeriodBackup, clearStatisticsUI.getModel().SalesPersonMaximumPeriod(),
                clearStatisticsUI.getModel().SalesPersonStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughSalesPersonPeriod,
                clearStatisticsUtilities.refreshSalespersonStatistic);
        },

        /**
         * @name validateItemPeriod
         * @description 
         * @private
         */
        validateItemPeriod: function () {
            _validatePeriod("Data_ThroughItemPeriod", itemPeriodBackup, clearStatisticsUI.getModel().ItemMaximumPeriod(),
                clearStatisticsUI.getModel().ItemStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughItemPeriod,
                clearStatisticsUtilities.refreshItemStatistic);
        },
    };

})(clearStatisticsUtilities || {}, jQuery);

var clearStatisticsUISuccess = (function (self, $) {

    /**
     * @name _commonHandler
     * @description Method used by the public methods in this object
     * @private
     * @param {any} result
     * @param {any} dataItemId
     * @param {any} throughYearMethod
     * @param {any} yearBackup
     * @param {any} maxPeriodMethod
     * @param {any} backupYearMethod
     * @param {any} refreshStatisticMethod
     */
    function _commonHandler(result, dataItemId, throughYearMethod, yearBackup,
                            maxPeriodMethod, backupYearMethod, refreshStatisticMethod) {
        if (result == 0) {
            throughYearMethod(yearBackup);
        } else {
            maxPeriodMethod(result);
            backupYearMethod();
            setTimeout(function () {
                ($("#" + dataItemId)).siblings('input:visible').focus();
            });
        }
        refreshStatisticMethod();
    }

    return {

        /**
         * @name fillCustomerFiscalYear
         * @description
         * @private
         * @param result
         */
        fillCustomerFiscalYear: function (result) {
            _commonHandler(result, "Data_ThroughCustomerPeriod", clearStatisticsUI.getModel().Data.ThroughCustomerYear,
                clearStatisticsUtilities.customerYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupCustomerYearValue, clearStatisticsUtilities.refreshCustomerStatistic);
        },

        /**
         * @name fillGroupFiscalYear
         * @description
         * @private
         * @param result
         */
        fillGroupFiscalYear: function (result) {
            _commonHandler(result, "Data_ThroughGroupPeriod", clearStatisticsUI.getModel().Data.ThroughGroupYear,
                clearStatisticsUtilities.customerGroupYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupGroupYearValue, clearStatisticsUtilities.refreshGroupStatistic);
        },

        /**
         * @name fillNationalAcctFiscalYear
         * @description
         * @private
         * @param result
         */
        fillNationalAcctFiscalYear: function (result) {
            _commonHandler(result, "Data_ThroughNationalAcctPeriod", clearStatisticsUI.getModel().Data.ThroughNationalAcctYear,
                clearStatisticsUtilities.nationalAcctYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupNationalAcctYearValue, clearStatisticsUtilities.refreshNationalAcctStatistic);
        },

        /**
         * @name fillSalespersonFiscalYear
         * @description
         * @private
         * @param result
         */
        fillSalespersonFiscalYear: function (result) {
            _commonHandler(result, "Data_ThroughSalesPersonPeriod", clearStatisticsUI.getModel().Data.ThroughSalesPersonYear,
                clearStatisticsUtilities.salespersonYearBackup, clearStatisticsUI.getModel().SalesPersonMaximumPeriod,
                clearStatisticsUtilities.backupSalespersonYearValue, clearStatisticsUtilities.refreshSalespersonStatistic);
        },

        /**
         * @name fillItemFiscalYear
         * @description
         * @private
         * @param result
         */
        fillItemFiscalYear: function (result) {
            _commonHandler(result, "Data_ThroughItemPeriod", clearStatisticsUI.getModel().Data.ThroughItemYear,
                clearStatisticsUtilities.itemYearBackup, clearStatisticsUI.getModel().ItemMaximumPeriod,
                clearStatisticsUtilities.backupItemYearValue, clearStatisticsUtilities.refreshItemStatistic);
        }
    };

})(clearStatisticsUISuccess || {}, jQuery);

$(function () {
    clearStatisticsUI.init();
});