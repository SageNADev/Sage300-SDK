/* Copyright(c) 2022 The Sage Group plc or its licensors.All rights reserved. */

// Enable the following commented line to enable TypeScript static type checking
// @ts-check

"use strict";

// This list should match DeclarativeControlType on the server-side
var ControlTypeEnum = ControlTypeEnum || Object.freeze({
    Textbox: 0,
    Finder: 1,
    Dropdown: 2,
    Checkbox: 3,
    Hidden: 4,
    Numeric: 5,
    Datepicker: 6,
    RadioButtonGroup: 7,
    Currency: 8,
    Section: 9
});

var declarativeReportUI = declarativeReportUI || {}

declarativeReportUI = {
    computedProperties: [],
    parameters: null,

    // Controls whether or not the validation() function in this file is called
    pageIsValid: true,
    doFrameworkValidation: true, // Default this to true

    /**
     * @function
     * @name init
     * @description Primary Initialization Routine
     * @namespace declarativeReportUI
     * @public
     */
    init: function () {
        // Initialize the controls
        declarativeReportUI.getParameters(DeclarativeReportViewModel.Data);
        declarativeReportUI.initButtons();

        // Set focus to first available control in the form
        declarativeReportUI.setInitialFocus();
    },

    /**
     * @function
     * @name setInitialFocus
     * @description Determine and set initial control focus
     * @namespace declarativeReportUI
     * @public
     */
    setInitialFocus: function () {
        // Reduce noise just a bit :)
        let params = declarativeReportUI.parameters.Parameters;
        const FOCUSDELAY = 200;

        let foundFocusableControl = false;
        for (let index = 0; index < params.length; index++) {
            let param = params[index];
            let isHidden = param.Hidden;
            let controlType = param.ControlType;

            // Active control and not hidden
            if (isHidden == false && controlType != ControlTypeEnum.Hidden && controlType != ControlTypeEnum.Section) {
                declarativeReportUI.setFocusByControlType(param, FOCUSDELAY);
                foundFocusableControl = true;
                break;
            }
        }

        // If necessary, set focus to the print button
        if (foundFocusableControl === false) {
            setTimeout(function () {
                sg.controls.Focus($('#btnPrint'));
            }, FOCUSDELAY);
        }
    },

    /**
     * @function
     * @name setFocusByControlType
     * @description Set the control focus based on control type
     * @namespace declarativeReportUI
     * @public
     * @param {object} param The parameter object
     * @param {number} delay The delay before focus is set
     */
    setFocusByControlType: function (param, delay) {
        setTimeout(function () {

            const controlType = param.ControlType;

            switch (controlType) {
                case ControlTypeEnum.Dropdown:
                    let dropdownlist = $(`#${param.ID}`).data("kendoDropDownList");
                    dropdownlist.focus();
                    break;

                case ControlTypeEnum.RadioButtonGroup:
                    // Find all the radio buttons by name (Id for the group is null)
                    var firstRadioButton = $(`[name='${param.Name}']`)[0];
                    firstRadioButton.focus();
                    break;

                default:
                    sg.controls.Focus($(`#${param.ID}`));
                    break;
            }

        }, delay);
    },

    /**
     * @function
     * @name initButtons
     * @description Initialize the buttons
     * @namespace declarativeReportUI
     * @public
     */
    initButtons: function () {
        $("#btnPrint").on('click', function () {
            sg.utls.SyncExecute(declarativeReportUI.print);
        });
    },
    
    /**
     * @function
     * @name print
     * @description Print 
     * @namespace declarativeReportUI
     * @public
     */
    print: function () {
        if (sg.utls.isProcessRunning) {
            return;
        }

        declarativeReportUI.mapValuesToModel();

        // Check if form is valid
        if ($("#frmDeclarativeReport").valid()) {

            // Check Validations
            let isValid = declarativeReportUI.pageIsValid;
            if (declarativeReportUI.doFrameworkValidation && isValid) {
                isValid &= declarativeReportUI.validation();
            }

            if (isValid) {
                $("#message").empty();
                sg.utls.clearValidations("frmDeclarativeReport");
                sg.utls.isProcessRunning = true;

                declarativeReportRepository.execute(declarativeReportUI.parameters);

                if (declarativeReportUI.postPrintFunction) {
                    declarativeReportUI.postPrintFunction();
                }
            }
        }
    },

    /**
     * @function
     * @name mapValuesToModel
     * @description Update the Value field of the model to pass back to the server
     * @namespace declarativeReportUI
     * @public
     */
    mapValuesToModel: () => {
        declarativeReportUI.parameters.Parameters.forEach((param) => {

            const controlId = `#${param.ID}`;

            switch (param.ControlType) {

                case ControlTypeEnum.Checkbox:
                    const checked = $(controlId).is(':checked');
                    const isVisible = $(controlId).is(':visible');
                    if (isVisible) {
                        param.Value = checked ? param.DataSource[1].Value : param.DataSource[0].Value;
                    } else {
                        // checkbox isn't visible so just assume it's unchecked.
                        param.Value = param.DataSource[0].Value;
                    }
                    break;

                case ControlTypeEnum.RadioButtonGroup:
                    param.Value = $(`input[name="${param.Name}"]:checked`).val();
                    break;

                case ControlTypeEnum.Finder:
                case ControlTypeEnum.Dropdown:
                case ControlTypeEnum.Hidden:
                case ControlTypeEnum.Numeric:
                case ControlTypeEnum.Currency:
                case ControlTypeEnum.Datepicker:
                case ControlTypeEnum.Textbox:
                default:
                    // Inspect the dom for this control to see if it has the 'txt-upper' class
                    const isUppercase = $(controlId).hasClass('txt-upper');

                    let val = $(controlId).val();

                    // Convert text to uppercase if necessary
                    if (isUppercase) {
                        val = val.toUpperCase();
                    }

                    // Strip time from Datepicker value if necessary
                    if (param.ControlType === ControlTypeEnum.Datepicker) {
                        const dateVal = $(controlId).data("kendoDatePicker").value();
                        if (dateVal != null) {
                            var date = new Date(val);
                            val = sg.utls.formatDate(date, "yyyyMMdd");
                        }
                    }
                    param.Value = val;
                    break;
            }
        });
    },

    /**
     * @function
     * @name validation
     * @description Validation
     * @namespace declarativeReportUI
     * @public
     */
    validation: function () {
        var errorRangeMessage = "";
        var errorMessage = "";

        if (declarativeReportUI.parameters.Ranges) {
            // validate range values are From > To
            declarativeReportUI.parameters.Ranges.forEach((range) => {
                var controlFrom = declarativeReportUI.parameters.Parameters.find(x => x.Name === range.From);
                var controlTo = declarativeReportUI.parameters.Parameters.find(x => x.Name === range.To);
                if (controlFrom && controlTo) {
                    const isDateValue = controlFrom.ControlType === ControlTypeEnum.Datepicker;

                            const v1 = $(`#${controlFrom.ID}`).val();
                            const fromVal = v1 ? v1.toUpperCase() : v1;

                            const v2 = $(`#${controlTo.ID}`).val();
                            const toVal = v2 ? v2.toUpperCase() : v2;

                            let showErrorMessage = isDateValue && fromVal && toVal ? Date.parse(fromVal) > Date.parse(toVal) : fromVal > toVal;
                            if (showErrorMessage) {
                                if (range.LabelFunction) {
                                    const isFunc = declarativeReportUI.getFunction(range.LabelFunction);
                                    errorRangeMessage = (isFunc instanceof Function) ? isFunc() : range.LabelText;
                                }
                                else {
                                    errorRangeMessage = range.LabelText;
                                }
                    }
                }
            });
        }

        if (errorRangeMessage != "") {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(declarativeReportResources.ErrorFromToValueMessage, errorRangeMessage));
        } else if (errorMessage != "") {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
        }

        return !errorMessage && !errorRangeMessage;
    },

    /**
     * @description Get function from function name string, including the namespace
     * @param {string} functionName The function full name, including namespace
     * @return {function} The function expression
     */
    getFunction: function (functionName) {
        //todo move it into frmework??
        //throw error if function name is null?
        if (functionName) {
            var ns = functionName.split('.');
            return ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[functionName];
        }
    },

    /**
     * @function
     * @name getParameters
     * @description Show the generated report parameters
     * @namespace declarativeReportUI
     * @public
     * @param {object} result the generated parameters
     */
    getParameters: (result) => {
        if (result) {
            declarativeReportUI.parameters = result;
            $('#declarativeReportBody').html(result.HtmlTemplate);
            declarativeReportUI.setTitle(result);
            declarativeReportUI.initParameters(result);
            declarativeReportUI.hideControls(result);
        }
    },

    /**
     * @function
     * @name initParameters
     * @description Initialize the generated parameters
     * @namespace declarativeReportUI
     * @public
     * @param {object} setting the generated parameters
     */
    initParameters: (setting) => {
        setting.Parameters.forEach((param) => {

            const controlName = param.ID;
            const controlId = `#${param.ID}`;
            const defaultValue = param.DefaultValue;

            // May or may not be used
            let decimalPlaces = 0;
            let showSpinners = false;
            let stepIncrement = 0;
            let maxDigits = 18;
            let minValue = 0;
            let maxValue = 1;

            declarativeReportUI.setParameterLabel(param);

            switch (param.ControlType) {

                case ControlTypeEnum.Finder:
                    sg.viewFinderHelper.setViewFinder(param.ButtonID, param.ID, declarativeReportUI.getFunction(param.FinderDefinition));
                    $(controlId).val(defaultValue);
                    break;

                case ControlTypeEnum.Dropdown:
                    $(controlId).kendoDropDownList({
                        dataTextField: "LabelText",
                        dataValueField: "Value",
                        dataSource: param.DataSource
                    });

                    $(controlId).data("kendoDropDownList").select((dataItem) => {
                        return dataItem.Value === defaultValue;
                    });
                    break;

                case ControlTypeEnum.Checkbox:
                    $(controlId).on('change', function (e) {
                        var checked = $(controlId).is(':checked');
                        $(controlId).val(checked);
                    });
                    // for checkbox control, 2nd value in DataSource is the 'yes' value
                    if (defaultValue === param.DataSource[1].Value) {
                        $(controlId).prop('checked', true);
                    }
                    break;

                case ControlTypeEnum.Numeric:
                    decimalPlaces = param.DecimalPlaces;
                    showSpinners = param.ShowSpinners;
                    stepIncrement = param.StepIncrement;
                    maxDigits = param.MaxDigits;
                    minValue = param.MinValue;
                    maxValue = param.MaxValue;
                    sg.utls.initNumericTextBox(param.ID, decimalPlaces, showSpinners, stepIncrement, maxDigits, minValue, maxValue);
                    $(controlId).val(defaultValue);

                    // Forces a refresh of the control so the value is displayed
                    sg.utls.setNumericTextBox(param.ID);
                    break;

                case ControlTypeEnum.Currency:

                    // TODO - Need to get the following from the viewmodel
                    decimalPlaces = 3;
                    maxDigits = 18;
                    minValue = 0;
                    maxValue = 999999999999.999;

                    showSpinners = false;
                    stepIncrement = 0;
                    sg.utls.initNumericTextBox(param.ID, decimalPlaces, showSpinners, stepIncrement, maxDigits, minValue, maxValue);
                    $(controlId).val(defaultValue);

                    // Forces a refresh of the control so the value is displayed
                    sg.utls.setNumericTextBox(param.ID);
                    break;

                case ControlTypeEnum.Datepicker:
                    sg.utls.kndoUI.datePicker(controlName);
                    $(controlId).val(defaultValue === '12/31/9999' ? new Date().toLocaleDateString(globalResource.Culture) : defaultValue);
                    break;

                case ControlTypeEnum.RadioButtonGroup:
                    $(`input[name="${param.Name}"][value="${param.DefaultValue}"]`).prop('checked', true);
                    break;

                case ControlTypeEnum.Section:
                    // No need to do anything
                    break;

                case ControlTypeEnum.Textbox:
                default:
                    $(controlId).val(defaultValue);
                    break;
            }
        });
    },

    /**
     * @function
     * @name hideControls
     * @description Initialize the generated parameters
     * @namespace declarativeReportUI
     * @public
     * @param {object} setting the generated parameters
     */
    hideControls: (setting) => {
        const isOBActive = DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'OB').length > 0;
        setting.Parameters.forEach((param) => {
            if (param.Hidden || (param.IsOptionalFields && !isOBActive)) {
                if (param.ControlType === ControlTypeEnum.Checkbox) {
                    $(`#${param.ID}`).closest('div').hide();
                }
                else if (param.ControlType === ControlTypeEnum.Section) {
                    $(`#${param.ID}`).hide();
                }
                else {
                    $(`#${param.ID}`).closest('.form-group').hide();
                }
            }
        });
    },

    /**
     * @function
     * @name setTitle
     * @description Set report title
     * @namespace declarativeReportUI
     * @public
     * @param {object} setting the generated parameters
     */
    setTitle: (setting) => {
        let title = setting.ScreenNameText;

        if (setting.ScreenNameFunction) {
            const isFunc = declarativeReportUI.getFunction(setting.ScreenNameFunction);
            if (isFunc instanceof Function) {
                title = isFunc();
            }
        }

        $('#lblReportTitle').text(title);
    },

    /**
     * @function
     * @name setParameterLabel
     * @description Set parameter label
     * @namespace declarativeReportUI
     * @public
     * @param {object} param report parameter
     */
    setParameterLabel: (param) => {
        if (param.LabelFunction) {
            const isFunc = declarativeReportUI.getFunction(param.LabelFunction);
            if (isFunc instanceof Function) {
                // use for attribute to locate label, else get the first label
                if ($(`label[for='${param.ID}'`).length > 0) {
                    $(`label[for='${param.ID}'`).text(isFunc());
                }
                else {
                    $(`#${param.ID}`).parent().find('label').text(isFunc());
                }
            }
        }

        if (param.DataSource && param.DataSource.length > 0) {
            param.DataSource.forEach((data) => {
                if (data.LabelFunction) {
                    const isFunc = declarativeReportUI.getFunction(data.LabelFunction);
                    if (isFunc instanceof Function) {
                        data.LabelText = isFunc();
                    }
                }
            });
        }
    },

    /**
     * @function
     * @name bindPrePrintClick
     * @description Add click event before print click
     * @namespace declarativeReportUI
     * @public
     * @param {object} callback click event to run before print
     */
    bindPrePrintClick: (callback) => {
        const printClickEvent = $._data($("#btnPrint")[0], 'events').click[0];
        // remove print event
        $("#btnPrint").off('click');
        // add callback first
        $("#btnPrint").on('click', (e) => {
            callback(e);
        });
        // re-add print event
        $("#btnPrint").on('click', printClickEvent.handler);
    },

    /**
     * @function
     * @name bindPostPrintClick
     * @description Add click event after print click
     * @namespace declarativeReportUI
     * @public
     * @param {object} callback click event to run after print
     */
    bindPostPrintClick: (callback) => {
        declarativeReportUI.postPrintFunction = callback;
    }
};

// Callbacks
var declarativeReportOnSuccess = {
    /**
     * @function
     * @name execute
     * @description Open the report result or display a message
     * @namespace declarativeReportOnSuccess
     * @public
     *
     * @param {object} result The JSON result payload
     */
    execute: function (result) {
        if (result !== null && result.UserMessage.IsSuccess) {
            window.sg.utls.openReport(result.ReportToken);
        } else {
            sg.utls.showMessage(result);
        }
    }
};

// Primary page entry point
$(function () {
    declarativeReportUI.init();
    sg.utls.registerDestroySession();
});
