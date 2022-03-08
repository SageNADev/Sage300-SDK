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

var clearStatisticsConstants = ((self) => {
    return {
        MODULEID: 'TU',
        ACTION: 'ClearStatistics',
        MINIMUMFISCALYEAR: 1900,
    };
})(clearStatisticsConstants || {});

var clearStatisticsUI = ((self, $) => {

    let _initialized = false;

    let _model = {};
    let computedProperties = ["bClearCustomerStatistics", "bClearGroupStatistics",
                              "bClearNationalAcctStatistics", "bClearSalespersonStatistics"];
    let customerFiscalYear = null;
    let customerGroupFiscalYear = null;
    let nationalAcctFiscalYear = null;
    let salespersonFiscalYear = null;
    let itemFiscalYear = null;
    let customerFiscalPeriod = null;
    let customerGroupFiscalPeriod = null;
    let nationalAcctFiscalPeriod = null;
    let salespersonFiscalPeriod = null;
    let itemFiscalPeriod = null;

    /**
     * @name initKendoBindings
     * @description Initialize the Kendo bindings
     * @namespace clearStatisticsUI
     * @private
     */
    function initKendoBindings() {
        // Reduce code noise
        let utils = clearStatisticsUtilities;

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
     * @description Initialize all of the finders on the page
     * @namespace clearStatisticsUI
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
     * @description Initialize the Customer Number finders
     * @namespace clearStatisticsUI
     * @private
     */
    function initCustomerNumberFinders() {
        let props = sg.viewFinderProperties.AR.Customers;
        let controls = [
            { buttonId: "btnFromCustomerFinder", dataControlId: "Data_FromCustomerNo" },
            { buttonId: "btnToCustomerFinder", dataControlId: "Data_ToCustomerNo" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initCustomerGroupFinders
     * @description Initialize the Customer Group finders
     * @namespace clearStatisticsUI
     * @private
     */
    function initCustomerGroupFinders() {
        let props = sg.viewFinderProperties.AR.CustomerGroups;
        let controls = [
            { buttonId: "btnFromCustomerGroupFinder", dataControlId: "Data_FromGroupCode" },
            { buttonId: "btnToCustomerGroupFinder", dataControlId: "Data_ToGroupCode" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initNationalAcctFinders
     * @description Initialize the National Accounts finders
     * @namespace clearStatisticsUI
     * @private
     */
    function initNationalAcctFinders() {
        let props = sg.viewFinderProperties.AR.NationalAccounts;
        let controls = [
            { buttonId: "btnFromNationalAcctFinder", dataControlId: "Data_FromNationalAccount" },
            { buttonId: "btnToNationalAcctFinder", dataControlId: "Data_ToNationalAccount" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initSalespersonFinders
     * @description Initialize the National Accounts finder
     * @namespace clearStatisticsUI
     * @private
     */
    function initSalespersonFinders() {
        let props = sg.viewFinderProperties.AR.SalesPersons;
        let controls = [
            { buttonId: "btnFromSalespersonFinder", dataControlId: "Data_FromSalesPerson" },
            { buttonId: "btnToSalespersonFinder", dataControlId: "Data_ToSalesPerson" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initItemFinders
     * @description Initialize the Item finders
     * @namespace clearStatisticsUI
     * @private
     */
    function initItemFinders() {
        let props = sg.viewFinderProperties.AR.Items;
        let controls = [
            { buttonId: "btnFromItemFinder", dataControlId: "Data_FromItem" },
            { buttonId: "btnToItemFinder", dataControlId: "Data_ToItem" },
        ];
        _initFinderGroup(controls, props);
    }

    /**
     * @name initFiscalYearFinders
     * @description Initialize the Fiscal Year finders
     * @namespace clearStatisticsUI
     * @private
     */
    function initFiscalYearFinders() {
        let props = sg.viewFinderProperties.CS.FiscalCalendars;
        let controls = [
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
     * @description Generic routine to initialize a group of finders
     * @namespace clearStatisticsUI
     * @private
     * 
     * @param {array} controls - Array of objects containing button and data control names
     * @param {object} info - Object containing various settings for the finder
     * @param {object} filter = "" | The optional filter used to filter the finder results
     * @param {number} height = null | The optional height of the finder window
     * @param {number} top = null | The optional top location of the finder window
     */
    function _initFinderGroup(controls, info, filter = "", height = null, top = null) {
        for (var i = 0; i < controls.length; i++) {
            sg.viewFinderHelper.initFinder(controls[i].buttonId, controls[i].dataControlId, info, filter, height, top);
        }
    }

    /**
     * @name initCheckBox
     * @description Initialize the click handlers for check boxes
     * @namespace clearStatisticsUI
     * @private
     */
    function initCheckBox() {
        $("#Data_ClearCustomerStatistics").click((e) => {
            if ($(this).is(':checked')) {
                setTimeout(() => {
                    sg.controls.Focus($("#Data_FromCustomerNo"));
                })
            }
        });

        $(document).on("change", "#Data_ClearGroupStatistics", () => {
            if ($("#Data_ClearGroupStatistics").is(":checked")) {
                setTimeout(() => {
                    sg.controls.Focus($("#Data_FromGroupCode"));
                })
            }
        });

        $(document).on("change", "#Data_ClearNationalAcctStatistics", () => {
            if ($("#Data_ClearNationalAcctStatistics").is(":checked")) {
                setTimeout(() => {
                    sg.controls.Focus($("#Data_FromNationalAccount"));
                })
            }
        });

        $(document).on("change", "#Data_ClearSalesPersonStatistics", () => {
            if ($("#Data_ClearSalesPersonStatistics").is(":checked")) {
                setTimeout(() => {
                    sg.controls.Focus($("#Data_FromSalesPerson"));
                })
            }
        });

        $(document).on("change", "#Data_ClearItemStatistics", () => {
            if ($("#Data_ClearItemStatistics").is(":checked")) {
                setTimeout(() => {
                    sg.controls.Focus($("#Data_FromItem"));
                })
            }
        });
    }

    /**
     * @name initBlur
     * @description Initialize the onChange handlers for various controls
     * @namespace clearStatisticsUI
     * @private
     */
    function initBlur() {
        $("#Data_FromCustomerNo").on('change', (e) => { 
            sg.controls.Focus($("#Data_ToCustomerNo"));
        });

        $("#Data_ToCustomerNo").on('change', (e) => {
            sg.controls.Focus($("#Data_ThroughCustomerYear"));
        });

        $("#Data_ThroughCustomerYear").on('change', (e) => {
            let $control = $("#Data_ThroughCustomerYear");
            sg.delayOnChange("btnFindCustomerYear", $control, () => {
                let validatePeriodForYear = true;
                let year = $control.val();
                let oldYear = customerFiscalYear;
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

        $("#Data_FromGroupCode").on('change', (e) => {
            sg.controls.Focus($("#Data_ToGroupCode"));
        });

        $("#Data_ToGroupCode").on('change', (e) => {
            sg.controls.Focus($("#Data_ThroughGroupYear"));
        });

        $("#Data_ThroughGroupYear").on('change', (e) => {
            let $control = $("#Data_ThroughGroupYear");
            sg.delayOnChange("btnFindCustomerGroupYear", $control, () => {
                let validatePeriodForYear = true;
                let year = $control.val();
                let oldYear = customerGroupFiscalYear;
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

        $("#Data_FromNationalAccount").on('change', (e) => {
            sg.controls.Focus($("#Data_ToNationalAccount"));
        });

        $("#Data_ToNationalAccount").on('change', (e) => {
            sg.controls.Focus($("#Data_ThroughNationalAcctYear"));
        });

        $("#Data_ThroughNationalAcctYear").on('change', (e) => {
            let $control = $("#Data_ThroughNationalAcctYear");
            sg.delayOnChange("btnFindNationalAcctYear", $control, () => {
                let validatePeriodForYear = true;
                let year = $control.val();
                let oldYear = nationalAcctFiscalYear;
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

        $("#Data_FromSalesPerson").on('change', (e) => {
            sg.controls.Focus($("#Data_ToSalesPerson"));
        });

        $("#Data_ToSalesPerson").on('change', (e) => {
            sg.controls.Focus($("#Data_ThroughSalesPersonYear"));
        });

        $("#Data_ThroughSalesPersonYear").on('change', (e) => {
            let $control = $("#Data_ThroughSalesPersonYear");
            sg.delayOnChange("btnFindSalespersonYear", $control, () => {
                let validatePeriodForYear = true;
                let year = $control.val();
                let oldYear = salespersonFiscalYear;
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

        $("#Data_FromItem").on('change', (e) => {
            sg.controls.Focus($("#Data_ToItem"));
        });

        $("#Data_ToItem").on('change', (e) => {
            sg.controls.Focus($("#Data_ThroughItemYear"));
        });

        $("#Data_ThroughItemYear").on('change', (e) => {
            let $control = $("#Data_ThroughItemYear");
            sg.delayOnChange("btnFindItemYear", $control, () => {
                let validatePeriodForYear = true;
                let year = $control.val();
                let oldYear = itemFiscalYear;
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

        $("#Data_ThroughCustomerPeriod").on('change', (e) => {
            $("#message").empty();
            clearStatisticsUtilities.validateCustomerPeriod();
        });

        $("#Data_ThroughGroupPeriod").on('change', (e) => {
            $("#message").empty();
            clearStatisticsUtilities.validateGroupPeriod();
        });

        $("#Data_ThroughNationalAcctPeriod").on('change', (e) => {
            $("#message").empty();
            clearStatisticsUtilities.validateNationalAcctPeriod();
        });

        $("#Data_ThroughSalesPersonPeriod").on('change', (e) => {
            $("#message").empty();
            clearStatisticsUtilities.validateSalespersonPeriod();
        });

        $("#Data_ThroughItemPeriod").on('change', (e) => {
            $("#message").empty();
            clearStatisticsUtilities.validateItemPeriod();
        });
    }

    /**
     * @name initTextBox
     * @description Initialize the Kendo text boxes
     * @namespace clearStatisticsUI
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
     * @description Initialize the button click handlers
     * @namespace clearStatisticsUI
     * @private
     */
    function initButtons() {
        $("#btnProcess").click((e) => {
            debugger;
            sg.utls.SyncExecute(process);
        });
    }

    /**
     * @name process
     * @description Handler for the process button
     * @namespace clearStatisticsUI
     * @private
     */
    function process() {
        debugger;
        sg.utls.isProcessRunning = true;
        let processUrl = sg.utls.url.buildUrl("TU", "ClearStatistics", "Process");

        let isChecked = (_model.Data.bClearCustomerStatistics()
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
                let data = { model: ko.mapping.toJS(_model, computedProperties) };
                sg.utls.ajaxPost(processUrl, data, onSuccess.process)
            }
        }
    }

    /**
     * @name initProcessUI
     * @description
     * @namespace clearStatisticsUI
     * @private
     */
    function initProcessUI() {
        let progressUrl = sg.utls.url.buildUrl(clearStatisticsConstants.MODULEID, clearStatisticsConstants.ACTION, "Progress");
        let cancelUrl = sg.utls.url.buildUrl(clearStatisticsConstants.MODULEID, clearStatisticsConstants.ACTION, "Cancel");
        // @ts-ignore
        window.progressUI.init(progressUrl, cancelUrl, _model, screenName, onSuccess.onProcessComplete);
    }

    /**
     * @name Validation
     * @description Page validator
     * @namespace clearStatisticsUI
     * @private
     */
    function Validation() {
        let errorRangeMessage = "";
        let inputValid = true;
        // @ts-ignore
        let resources = clearStatisticsResources;

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
     * @description Check to see if a year is a FiscalYear
     * @namespace clearStatisticsUI
     * @private
     * 
     * @param {number} Year
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
     * @description set or unset the _initialized flag
     * @namespace clearStatisticsUI
     * @private
     * 
     * @param {boolean} init true | false
     */
    function setInitialized(init) {
        _initialized = init;
    }

    // Public Methods
    return {

        /**
         * @name init
         * @description Initialize the controls and apply kendo bindings
         * @namespace clearStatisticsUI
         * @public
         */
        init: () => {
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
         * @description Check to see if page has been initialized
         * @namespace clearStatisticsUI
         * @public
         * 
         * @returns {boolean} _initialized
         */
        getInitialized: () => {
            return _initialized;
        },

        /**
         * @name getModel
         * @description Get the _model property
         * @namespace clearStatisticsUI
         * @public
         * 
         * @returns {object} _model
         */
        getModel: () => {
            return _model;
        },
    };

})(clearStatisticsUI || {}, jQuery);

var onSuccess = ((self, $) => {

    return {
        /**
         * @name process
         * @description
         * @namespace onSuccess
         * @public
         * 
         * @param {object} result - JSON result payload
         */
        process: (result) => {
            if (result.UserMessage.IsSuccess) {
                let model = clearStatisticsUI.getModel();
                window.ko.mapping.fromJS(result.WorkflowInstanceId, {}, model.WorkflowInstanceId);
                window.progressUI.progress();
            } else {
                sg.utls.showMessage(result);
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
                var errorMessage = clearStatisticsResources.ProcessingComplete;
                sg.utls.showMessageInfoInCustomDivWithoutClose(sg.utls.msgType.INFO, errorMessage, 'messageDiv');
            }
        },
    };
})(onSuccess || {}, jQuery);

var clearStatisticsUtility = ((self, $) => {
    return {
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
            if (clearStatisticsUI.getModel().IsKoStatisticsDirty && clearStatisticsUI.getModel().IsKoStatisticsDirty.isDirty()) {
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

        /**
         * @name setFocusToFiscalYear
         * @description Set the current cursor focus to the 'Through Customer Year' field
         * @namespace clearStatisticsUtility
         * @public
         */
        setFocusToFiscalYear: () => {
            sg.utls.focus("Data_ThroughCustomerYear");
        },
    };
})(clearStatisticsUtility || {}, jQuery);

var clearStatisticsUtilities = ((self, $) => {

    let customerYearBackup = null;
    let customerGroupYearBackup = null;
    let nationalAcctYearBackup = null;
    let salespersonYearBackup = null;
    let itemYearBackup = null;
    let customerPeriodBackup = null;
    let customerGroupPeriodBackup = null;
    let nationalAcctPeriodBackup = null;
    let salespersonPeriodBackup = null;
    let itemPeriodBackup = null;

    /**
     * @name _validationPeriod
     * @description Method used by other validation methods in this object
     * @namespace clearStatisticsUtilities
     * @private
     * 
     * @param {string} dataItemId
     * @param {number} periodBackup
     * @param {number} maxPeriod
     * @param {object} modelCurrentPeriodMethod - Method Name
     * @param {object} throughPeriodMethod - Method Name
     * @param {object} refreshStatisticMethod - Method Name
     */
    function _validatePeriod(dataItemId, periodBackup, maxPeriod, modelCurrentPeriodMethod, throughPeriodMethod, refreshStatisticMethod) {
        sg.utls.clearValidations("frmClearStatistics");
        let period = $("#" + dataItemId).val();
        let oldPeriod = periodBackup;
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
         * @namespace clearStatisticsUtilities
         * 
         * @public
         */
        backupCustomerYearValue: () => {
            customerYearBackup = clearStatisticsUI.getModel().Data.ThroughCustomerYear();
        },

        /**
         * @name backupGroupYearValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupGroupYearValue: () => {
            customerGroupYearBackup = clearStatisticsUI.getModel().Data.ThroughGroupYear();
        },

        /**
         * @name backupNationalAcctYearValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupNationalAcctYearValue: () => {
            nationalAcctYearBackup = clearStatisticsUI.getModel().Data.ThroughNationalAcctYear();
        },

        /**
         * @name backupSalespersonYearValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupSalespersonYearValue: () => {
            salespersonYearBackup = clearStatisticsUI.getModel().Data.ThroughSalesPersonYear();
        },

        /**
         * @name backupItemYearValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupItemYearValue: () => {
            itemYearBackup = clearStatisticsUI.getModel().Data.ThroughItemYear();
        },

        /**
         * @name backupCustomerPeriodValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupCustomerPeriodValue: () => {
            customerPeriodBackup = clearStatisticsUI.getModel().Data.ThroughCustomerPeriod();
        },

        /**
         * @name backupGroupPeriodValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupGroupPeriodValue: () => {
            customerGroupPeriodBackup = clearStatisticsUI.getModel().Data.ThroughGroupPeriod();
        },

        /**
         * @name backupNationalAcctPeriodValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupNationalAcctPeriodValue: () => {
            nationalAcctPeriodBackup = clearStatisticsUI.getModel().Data.ThroughNationalAcctPeriod();
        },

        /**
         * @name backupSalespersonPeriodValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupSalespersonPeriodValue: () => {
            salespersonPeriodBackup = clearStatisticsUI.getModel().Data.ThroughSalesPersonPeriod();
        },

        /**
         * @name backupItemPeriodValue
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        backupItemPeriodValue: () => {
            itemPeriodBackup = clearStatisticsUI.getModel().Data.ThroughItemPeriod();
        },

        /**
         * @name refreshCustomerStatistic
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        refreshCustomerStatistic: () => {
            clearStatisticsUtilities.backupCustomerPeriodValue();
            clearStatisticsUtilities.backupCustomerYearValue();
        },

        /**
         * @name refreshGroupStatistic
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        refreshGroupStatistic: () => {
            clearStatisticsUtilities.backupGroupPeriodValue();
            clearStatisticsUtilities.backupGroupYearValue();
        },

        /**
         * @name refreshNationalAcctStatistic
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        refreshNationalAcctStatistic: () => {
            clearStatisticsUtilities.backupNationalAcctPeriodValue();
            clearStatisticsUtilities.backupNationalAcctYearValue();
        },

        /**
         * @name refreshSalespersonStatistic
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        refreshSalespersonStatistic: () => {
            clearStatisticsUtilities.backupSalespersonPeriodValue();
            clearStatisticsUtilities.backupSalespersonYearValue();
        },

        /**
         * @name refreshItemStatistic
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        refreshItemStatistic: () => {
            clearStatisticsUtilities.backupItemPeriodValue();
            clearStatisticsUtilities.backupItemYearValue();
        },

        /**
         * @name validateCustomerYear
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateCustomerYear: () => {
            clearStatisticsRepository.getCustomerMaxPeriodForValidYear($("#Data_ThroughCustomerYear").val());
        },

        /**
         * @name validateGroupYear
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateGroupYear: () => {
            clearStatisticsRepository.getGroupMaxPeriodForValidYear($("#Data_ThroughGroupYear").val());
        },

        /**
         * @name validateNationalAcctYear
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateNationalAcctYear: () => {
            clearStatisticsRepository.getNationalAcctMaxPeriodForValidYear($("#Data_ThroughNationalAcctYear").val());
        },

        /**
         * @name validateSalespersonYear
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateSalespersonYear: () => {
            clearStatisticsRepository.getSalespersonMaxPeriodForValidYear($("#Data_ThroughSalesPersonYear").val());
        },

        /**
         * @name validateItemYear
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateItemYear: () => {
            clearStatisticsRepository.getItemMaxPeriodForValidYear($("#Data_ThroughItemYear").val());
        },

        /**
         * @name validateCustomerPeriod
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateCustomerPeriod: () => {
            _validatePeriod("Data_ThroughCustomerPeriod", customerPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughCustomerPeriod,
                clearStatisticsUtilities.refreshCustomerStatistic);
        },

        /**
         * @name validateGroupPeriod
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateGroupPeriod: () => {
            _validatePeriod("Data_ThroughGroupPeriod", customerGroupPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughGroupPeriod,
                clearStatisticsUtilities.refreshGroupStatistic);
        },

        /**
         * @name validateNationalAcctPeriod
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateNationalAcctPeriod: () => {
            _validatePeriod("Data_ThroughNationalAcctPeriod", nationalAcctPeriodBackup, clearStatisticsUI.getModel().MaximumPeriod(),
                clearStatisticsUI.getModel().CustomerStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughNationalAcctPeriod,
                clearStatisticsUtilities.refreshNationalAcctStatistic);
        },

        /**
         * @name validateSalespersonPeriod
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateSalespersonPeriod: () => {
            _validatePeriod("Data_ThroughSalesPersonPeriod", salespersonPeriodBackup, clearStatisticsUI.getModel().SalesPersonMaximumPeriod(),
                clearStatisticsUI.getModel().SalesPersonStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughSalesPersonPeriod,
                clearStatisticsUtilities.refreshSalespersonStatistic);
        },

        /**
         * @name validateItemPeriod
         * @description 
         * @namespace clearStatisticsUtilities
         *
         * @public
         */
        validateItemPeriod: () => {
            _validatePeriod("Data_ThroughItemPeriod", itemPeriodBackup, clearStatisticsUI.getModel().ItemMaximumPeriod(),
                clearStatisticsUI.getModel().ItemStatisticsCurrentPeriod, clearStatisticsUI.getModel().Data.ThroughItemPeriod,
                clearStatisticsUtilities.refreshItemStatistic);
        },
    };

})(clearStatisticsUtilities || {}, jQuery);

var clearStatisticsUISuccess = ((self, $) => {

    /**
     * @name _commonHandler
     * @description Method used by the public methods in this object
     * @namespace clearStatisticsUISuccess
     * @private
     * 
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
            setTimeout(() => {
                ($("#" + dataItemId)).siblings('input:visible').focus();
            });
        }
        refreshStatisticMethod();
    }

    return {

        /**
         * @name fillCustomerFiscalYear
         * @description
         * @namespace clearStatisticsUISuccess
         * @public
         * 
         * @param {Object} result The JSON result object
         */
        fillCustomerFiscalYear: (result) => {
            _commonHandler(result, "Data_ThroughCustomerPeriod", clearStatisticsUI.getModel().Data.ThroughCustomerYear,
                clearStatisticsUtilities.customerYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupCustomerYearValue, clearStatisticsUtilities.refreshCustomerStatistic);
        },

        /**
         * @name fillGroupFiscalYear
         * @description
         * @namespace clearStatisticsUISuccess
         * @public
         * 
         * @param {Object} result The JSON result object
         */
        fillGroupFiscalYear: (result) => {
            _commonHandler(result, "Data_ThroughGroupPeriod", clearStatisticsUI.getModel().Data.ThroughGroupYear,
                clearStatisticsUtilities.customerGroupYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupGroupYearValue, clearStatisticsUtilities.refreshGroupStatistic);
        },

        /**
         * @name fillNationalAcctFiscalYear
         * @description
         * @namespace clearStatisticsUISuccess
         * @public
         *
         * @param {Object} result The JSON result object
         */
        fillNationalAcctFiscalYear: (result) => {
            _commonHandler(result, "Data_ThroughNationalAcctPeriod", clearStatisticsUI.getModel().Data.ThroughNationalAcctYear,
                clearStatisticsUtilities.nationalAcctYearBackup, clearStatisticsUI.getModel().MaximumPeriod,
                clearStatisticsUtilities.backupNationalAcctYearValue, clearStatisticsUtilities.refreshNationalAcctStatistic);
        },

        /**
         * @name fillSalespersonFiscalYear
         * @description
         * @namespace clearStatisticsUISuccess
         * @public
         *
         * @param {Object} result The JSON result object
         */
        fillSalespersonFiscalYear: (result) => {
            _commonHandler(result, "Data_ThroughSalesPersonPeriod", clearStatisticsUI.getModel().Data.ThroughSalesPersonYear,
                clearStatisticsUtilities.salespersonYearBackup, clearStatisticsUI.getModel().SalesPersonMaximumPeriod,
                clearStatisticsUtilities.backupSalespersonYearValue, clearStatisticsUtilities.refreshSalespersonStatistic);
        },

        /**
         * @name fillItemFiscalYear
         * @description
         * @namespace clearStatisticsUISuccess
         * @public
         *
         * @param {Object} result The JSON result object
         */
        fillItemFiscalYear: (result) => {
            _commonHandler(result, "Data_ThroughItemPeriod", clearStatisticsUI.getModel().Data.ThroughItemYear,
                clearStatisticsUtilities.itemYearBackup, clearStatisticsUI.getModel().ItemMaximumPeriod,
                clearStatisticsUtilities.backupItemYearValue, clearStatisticsUtilities.refreshItemStatistic);
        }
    };

})(clearStatisticsUISuccess || {}, jQuery);

$(() => {
    clearStatisticsUI.init();
});