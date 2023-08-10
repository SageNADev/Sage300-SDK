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
    Section: 9,
    Grid: 10
});

var ValueTypeEnum = {
    Text: '1',
    Amount: '100',
    Number: '6',
    Integer: '8',
    YesNo: '9',
    Date: '3',
    Time: '4'
};

var ContainerControlEnum = {
    FromText: "divFromText",
    OptionalField: "divOptFld",
    FromDate: "divFromDate",
    FromDropdown: "divFromDropdown",
    ToText: "divToText",
    ToDate: "divToDate",
    ToDropdown: "divToDropdown",
    SpanOptional: "spanOptFld",
    SpanFrom: "spanFromField",
    SpanTo: "spanToField"
};

var InputControlIdEnum = {
    OptionalTextField: "txtGridColOptField",
    OptionalButtonField: "btnFinderGridColOptField",
    FromButton: "btnFinderGridColFrom",
    ToButton: "btnFinderGridColTo",
    FromTextField: "txtGridColFrom",
    ToTextField: "txtGridColTo",
    FromDateField: "fromDate",
    FromDropdown: "fromDropdown",
    ToDate: "toDate",
    ToDropdown: "toDropdown",
    ColumnType: "hdnGridColType",
    ColumnDecimal: "hdnGridColDecimal",
    FromFinder: "txtFromFinder",
    ToFinder: "txtToFinder"
};

var ControlNameEnum = {
    OptionalField: "optField",
    FromField: "fromText",
    ToField: "toText",
    FromColumn: "gridColFrom",
    ToColumn: "gridColTo",
    FromDateColumn: "gridColFromDatePicker",
    ToDateColumn: "gridColToDatePicker",
    FromDropdown: "gridColFromDropDown",
    ToDropdown: "gridColToDropDown",
    AddButton: "gridColAddLineButton",
    DeleteButton: "gridColDeleteLineButton",
    OptFldFinderButton: "gridColOptFldFinderButton",
    SpanSelectedValue: "spanSelectedValue"
};

var Keycodes = {
    Dash: 189,
    Minus: 45,
    DecimalPoint: 46,
    Period: 190,
    Add: 107,
    NumPadPlus: 187,
    Delete: 46,
    Backspace: 8,
    Tab: 9,
    Escape: 27,
    Enter: 13,
    KeyValueA: 65,
    End: 35,
    ArrowRight: 39,
    ArrowDown: 40,
    Zero: 48,
    Nine: 57,
    NumpadZero: 96,
    NumPadNine: 105,
    KeyValueZ: 90,
    Plus: 43,
}

var HdnFieldEnum = {
    OptField: "hdnSELOPTFLD",
    OptDisplayFld: "hdnSELOPTDEC",
    FromDisplayValue: "hdnSELOPTFRDISP",
    ToDisplayValue: "hdnSELOPTTODISP",
    FromFilterValue: "hdnSELOPTFRVAL",
    ToFilterValue: "hdnSELOPTTOVAL",
    OptFldType: "hdnSELOPTTYPE"
}

var tabIndexFrom = "1";
var tabIndexTo = "2";
const integerZero = "0";
var moduleId = "";
const numericTextBoxMaxLength = 15;
const intTextBoxMaxLength = 11;
const decimalZero = "0.000"
var currentIndex = "";
const enterKeyCode = 13;
const tabKeyCode = 9;
const roleForCancelButton = "dialog";
const yesButtonInConfirmationPopup = "kendoConfirmationAcceptButton";
const btnPrint = "btnPrint";
const optFldName = "OPTFIELD";
const requestFetch = "Fetch";
const dataVal = 'val';
var locationId = "";
const maxGridRows = 3;
var isFinderCancelEvent = false;

var declarativeReportUI = declarativeReportUI || {}
declarativeReportUI = {
    computedProperties: [],
    parameters: null,

    // Controls whether or not the validation() function in this file is called
    pageIsValid: true,
    doFrameworkValidation: true, // Default this to true
    fieldProperties: {},
    yesNoList: {},
    noValue: {},
    optionalFieldColumn: {},
    fromColumn: {},
    toColumn: {},
    typeColumn: {},
    addButton: {},
    deleteButton: {},

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
        moduleId = DeclarativeReportViewModel.ModuleId;
        locationId = DeclarativeReportViewModel.LocationId;

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
                case ControlTypeEnum.Grid:
                default:
                    // Inspect the dom for this control to see if it has the 'txt-upper' class
                    const isUppercase = $(controlId).hasClass('txt-upper');

                    let val = $(controlId).val();

                    if (typeof val !== "undefined") {
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
                    }

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

                    const fromValue = $(`#${controlFrom.ID}`).val();
                    const fromVal = fromValue ? fromValue.toUpperCase() : fromValue;

                    const toValue = $(`#${controlTo.ID}`).val();
                    const toVal = toValue ? toValue.toUpperCase() : toValue;

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

                // Kendo Grid
                case ControlTypeEnum.Grid:
                    declarativeReportUI.fieldProperties = declarativeReportUI.getFunction(param.FinderDefinition);
                    declarativeReportUI.optionalFieldColumn = param.ColumnsHeader.OptionalField;
                    declarativeReportUI.fromColumn = param.ColumnsHeader.From;
                    declarativeReportUI.toColumn = param.ColumnsHeader.To;
                    declarativeReportUI.typeColumn = param.ColumnsHeader.Type;
                    declarativeReportUI.yesNoList = param.DropdownSource;
                    declarativeReportUI.addButton = param.GridButton.AddLine;
                    declarativeReportUI.deleteButton = param.GridButton.DeleteLine;

                    var htmlString = '<div class="edit-cell input-text" id="{0}" name="{0}" style="width : 92; padding : 2px;"><input name="{1}"id = "{2}"  type = "text" name = "{1}" style = "width : 100%;height:30px;" class="textFinder"/><input type="button" name="{1}"class="icon btn-search" id="{4}" style="right: 35px;background-size:925px 93px;background-color: transparent;border:none" /></div ><div id="{5}" name="{0}" class="datepicker small k-input" style="width : 92; padding : 2px;display:none;"><input type="date" name="{6}" id="{7}" formattextbox="date" maxlength="10" placeholder="M/d/yyyy"style="width : 100%;" class="dateValue"/></div><div id="{8}" name="{0}" style="width : 92; padding : 2px;display:none;"><input id="{9}" name="{10}"  style="width : 100%;" /></div><span id="{11}" name="{12}" style="color:black;" />'
                    var fromField = kendo.format(htmlString, ContainerControlEnum.FromText, ControlNameEnum.FromColumn, InputControlIdEnum.FromTextField, tabIndexFrom, InputControlIdEnum.FromButton, ContainerControlEnum.FromDate, ControlNameEnum.FromDateColumn, InputControlIdEnum.FromDateField, ContainerControlEnum.FromDropdown, InputControlIdEnum.FromDropdown, ControlNameEnum.FromDropdown, ContainerControlEnum.SpanFrom, ControlNameEnum.SpanSelectedValue)
                    var toField = kendo.format(htmlString, ContainerControlEnum.ToText, ControlNameEnum.ToColumn, InputControlIdEnum.ToTextField, tabIndexTo, InputControlIdEnum.ToButton, ContainerControlEnum.ToDate, ControlNameEnum.ToDateColumn, InputControlIdEnum.ToDate, ContainerControlEnum.ToDropdown, InputControlIdEnum.ToDropdown, ControlNameEnum.ToDropdown, ContainerControlEnum.SpanTo, ControlNameEnum.SpanSelectedValue)

                    var addButtonTemplate = '<button class="btn btn-default btn-grid-control btn-add" type="button"  id="btnAdd" name = "' + ControlNameEnum.AddButton + '">' + declarativeReportUI.addButton + '</button>';
                    var deleteButtonTemplate = '<button class="btn btn-default btn-grid-control btn-delete" type="button"  id="btnDelete" name = "' + ControlNameEnum.DeleteButton + '">' + declarativeReportUI.deleteButton + '</button>';


                    $(controlId).kendoGrid({
                        dataSource: param.DataSource,
                        scrollable: true,
                        navigatable: false,
                        reorderable: true,
                        resizable: true,
                        selectable: "row",
                        isFinderButton: false,

                        pageable: {
                            pageSize: 10,
                            numeric: false,
                            buttonCount: 1,
                            input: true
                        },

                        toolbar: [
                            { template: addButtonTemplate },
                            { template: deleteButtonTemplate }
                        ],
                        columns: [
                            {
                                title: declarativeReportUI.optionalFieldColumn,
                                attributes: {
                                    "class": "CellClickHandler",
                                },
                                template: '<div name="' + ControlNameEnum.OptionalField + '" class= "edit-cell input-text" id="' + ContainerControlEnum.OptionalField + '" style = "width : 92;padding : 2px;background-size:925px 93px;text-transform: uppercase;"><input name="' + ControlNameEnum.OptionalField + '" id="' + InputControlIdEnum.OptionalTextField + '" type="text"  class="txtOptn" maxlength = "12" formatTextbox = "alphaNumeric" style="width:100%;height:30px;border:none;text-transform: uppercase;"><input class="icon btn-search" id="' + InputControlIdEnum.OptionalButtonField + '" name = "' + ControlNameEnum.OptFldFinderButton + '" style="right: 35px;border: none;background-color: transparent;background-size:925px 93px" ></div><span name="' + ControlNameEnum.SpanSelectedValue + '" id="' + ContainerControlEnum.SpanOptional + '" style="color:black;" />',
                            },
                            {
                                title: declarativeReportUI.fromColumn,
                                attributes: {
                                    "class": "CellClickHandler",
                                },
                                template: fromField
                            },
                            {
                                title: declarativeReportUI.toColumn,
                                attributes: {
                                    "class": "CellClickHandler",
                                },
                                template: toField
                            },
                            {
                                title: declarativeReportUI.typeColumn,
                                hidden: true,
                                template: '<div class="edit-cell input-text"><input name="Type" id="' + InputControlIdEnum.ColumnType + '" type="text"></div>'
                            },
                            {
                                title: declarativeReportUI.decimals,
                                hidden: true,
                                template: '<div class="edit-cell input-text"><input name="Decimal" id="' + InputControlIdEnum.ColumnDecimal + '" type="text"></div>'
                            }
                        ],
                        editable:
                        {
                            createAt: "bottom",
                        }
                    })

                    // adding a new row in the grid and grid functionalities to bind the controls to the grid  
                    $('#btnAdd').click(function (e) {
                        let grid = $('#gdOptFields').data("kendoGrid");
                        var gridData = grid.dataSource.data();
                        var girdTotalRows = gridData.length;
                       
                        if (girdTotalRows <= maxGridRows) {
                            var selectedOptions = [];
                            var selectedFromOptions = [];
                            var selectedToOptions = [];
                            var selectedToDate = [];
                            var selectedType = [];
                            var selectedDecimal = [];
                            var selectedFromDate = [];
                            var selectedFromDropDownValue = [];
                            var selectedToDropDownValue = [];
                             
                            for (var i = 0; i < girdTotalRows; i++) {
                                //Push the selected datas in array
                                selectedOptions.push($("#" + InputControlIdEnum.OptionalTextField + i).val());
                                selectedType.push($("#" + InputControlIdEnum.ColumnType + i).val());
                                selectedDecimal.push($("#" + InputControlIdEnum.ColumnDecimal + i).val());
                                selectedFromOptions.push($("#" + InputControlIdEnum.FromTextField + i).val());
                                selectedToOptions.push($('#' + InputControlIdEnum.ToTextField + i).val());
                                selectedToDate.push($("#" + InputControlIdEnum.ToDate + i).val());
                                selectedFromDate.push($("#" + InputControlIdEnum.FromDateField + i).val());
                                selectedFromDropDownValue.push($("#" + InputControlIdEnum.FromDropdown + i).val());
                                selectedToDropDownValue.push($("#" + InputControlIdEnum.ToDropdown + i).val());

                                //Empty optionalfield textbox validation
                                var txtOpt = $("#" + InputControlIdEnum.OptionalTextField + i);
                                if (txtOpt.val() == null || txtOpt.val() == '') {
                                    if ((txtOpt.val() == null || txtOpt.val() == '' && girdTotalRows > 0)) {
                                        var urlMsg = sg.utls.url.buildUrl("PR", "Common", "EmptyOptionalField");
                                        sg.utls.ajaxGet(urlMsg, {}, (msg) => {
                                            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, msg, () => setOptionalFieldFocus(i));
                                        });
                                        return
                                    }
                                }
                            }
                            //Allow to add only three rows in grid
                            if (girdTotalRows < maxGridRows) {
                                grid.addRow();
                            }
                            //Disable add button if grid total row is 3
                            if (gridData.length == maxGridRows) { 
                                $('#btnAdd').enable(false);
                            }

                            //binding values to control
                            for (var i = 0; i <= girdTotalRows; i++) {
                                dynamicID(i);
                                setFinderEvent(i, changeControlType);
                                var txtGridColOptField = $("#" + InputControlIdEnum.OptionalTextField + i);
                                if (i == girdTotalRows) {
                                    $("#" + ContainerControlEnum.OptionalField + i).show()
                                    $("#" + ContainerControlEnum.SpanOptional + i).hide();
                                    txtGridColOptField.focus();
                                }
                                setFinders(i, selectedType[i]);
                                $('#' + InputControlIdEnum.FromTextField + i).attr("autocomplete", "off");
                                $('#' + InputControlIdEnum.ToTextField + i).attr("autocomplete", "off");

                                //Rebind the selected values in textbox,datepicker and dropdown after click the add button
                                txtGridColOptField.val(selectedOptions[i])
                                txtGridColOptField.data(dataVal, selectedOptions[i]);
                                $("#" + ContainerControlEnum.SpanOptional + i)[0].textContent = selectedOptions[i];
                                $("#" + InputControlIdEnum.ColumnType + i).val(selectedType[i])
                                $("#" + InputControlIdEnum.ColumnDecimal + i).val(selectedDecimal[i])
                                $("#" + InputControlIdEnum.FromTextField + i).val(selectedFromOptions[i])

                                optionalFinderInfo(ControlNameEnum.FromColumn, InputControlIdEnum.ToTextField);
                                $("#" + InputControlIdEnum.ToTextField + i).val(selectedToOptions[i])
                                $("#" + InputControlIdEnum.ToDate + i).kendoDatePicker({
                                    value: selectedToDate[i],
                                    change: onToDateChange
                                });
                                $("#" + InputControlIdEnum.FromDateField + i).kendoDatePicker({
                                    value: selectedFromDate[i],
                                    change: onFromDateChange
                                });
                                $("#" + InputControlIdEnum.FromDropdown + i).val(selectedFromDropDownValue[i]);
                                $("#" + InputControlIdEnum.ToDropdown + i).val(selectedToDropDownValue[i])

                                changeControlType(i, selectedType[i], "#" + ContainerControlEnum.OptionalField + i);

                                $("#" + InputControlIdEnum.OptionalButtonField + i).bind('click', function (e) {
                                    isFinderCancelEvent = true;
                                });

                                //onchange event for optional field column invalid input validation
                                $(txtGridColOptField).bind('change', function (e) {
                                    var sender = e.currentTarget.id;
                                    var index = sender.charAt(sender.length - 1);
                                    var spanOptionalFieldVal = $("#" + ContainerControlEnum.SpanOptional + (index))[0].textContent;
                                    sg.delayOnChange(InputControlIdEnum.OptionalButtonField + index, $("#" + InputControlIdEnum.OptionalTextField + index), function () {
                                        if (isFinderCancelEvent) {
                                            if (spanOptionalFieldVal != "") {
                                                $("#" + InputControlIdEnum.OptionalTextField + index).val(spanOptionalFieldVal);
                                            }
                                            else if ($("#" + InputControlIdEnum.ColumnType + index).val() == "") {
                                                $("#" + InputControlIdEnum.OptionalTextField + index).val('');
                                            }
                                            $("#" + ContainerControlEnum.OptionalField + index).focus();
                                        }
                                        else {
                                            _sendRequest(e, changeControlType, index);
                                        }
                                        isFinderCancelEvent = false;
                                    });
                                });
                            }
                        }
                        //set the focus                     
                        optionalFinderInfoFocus();
                        $("#btnDelete").enable(true);// show delete button in toolbar
                    });

                    /**
                      * @function
                      * @name _requestComplete
                      * @param {any} jsonResult The request call back result
                      * @param {Function} changeControlType control type has been changed based on the optional field type
                      * @param {number} index row index
                      * @description When ajax request call is completed, to handle the call back json results
                      */
                    function _requestComplete(jsonResult, changeControlType, index) {
                        if (jsonResult.Data.length > 0) {
                            setOptionalFieldValue(jsonResult.Data[0], changeControlType, index)
                        }
                    }

                  /**
                 * @function
                 * @name _sendRequest
                 * @param {object} e control event
                 * @param {Function} changeControlType control type has been changed based on the optional field type
                 * @param {number} index row index
                 * @description checks the invalid input in optional field 
                 */
                    function _sendRequest(e, changeControlType, index) {
                        let property = declarativeReportUI.fieldProperties.OptionalFields;
                        property.viewID = sg.viewFinderHelper.entityContextReplacement(property.viewID, moduleId);
                        index = e.target.id.charAt(e.target.id.length - 1);
                        property.filter = $.validator.format(property["filterTemplate"], locationId) + $.validator.format(" AND OPTFIELD = \"{0}\"", e.target.value.toUpperCase());
                        var urlData = sg.utls.url.buildUrl("Core", "ViewFinder", "RefreshGrid");
                        sg.utls.ajaxPostSync(urlData, property, function (jsonResult) {
                            var isSuccess = true;
                            if (jsonResult.Data.length < 1) {
                                isSuccess = false;
                            } if (!isSuccess) {
                                if (e.target.value == "") {
                                    var urlMsg = sg.utls.url.buildUrl("Core", "Common", "EmptyOptionalField");
                                } else {
                                    var urlMsg = sg.utls.url.buildUrl("Core", "Common", "InvalidOptionalFieldInput");
                                }
                                sg.utls.ajaxGet(urlMsg, {}, (msg) => {
                                    sg.utls.showMessageInfo(sg.utls.msgType.ERROR, $.validator.format(msg, e.target.value.toUpperCase()));
                                }); sg.utls.showMessageInfo(sg.utls.msgType.ERROR, urlMsg, () => setOptionalFieldFocus(index));
                                setTimeout(function () {
                                    $("#" + e.target.id).val($("#" + e.target.id).data('val'));
                                    $("#" + ContainerControlEnum.SpanOptional + index)[0].textContent = $("#" + e.target.id).data('val');
                                }, 100);
                                return;
                            }
                            _requestComplete(jsonResult, changeControlType, index);
                        });
                    } 

                    /**
                  * @function
                  * @name setOptionalFieldFocus
                  * @param {number} index row index
                  * @description set the focus in optional field
                  */
                    function setOptionalFieldFocus(index) {
                        setTimeout(function () {
                            $("#" + ContainerControlEnum.SpanOptional + index).hide();
                            $("#" + ContainerControlEnum.OptionalField + index).show();
                            $('#' + InputControlIdEnum.OptionalTextField + index).focus();

                        }, 100);
                    }

                    var grid = $('#gdOptFields').data("kendoGrid");
                    grid.table.on("focus", ".CellClickHandler", function (e) {
                        var row = $(this);
                        grid.select(row.closest("tr"));
                    });

                    /**
                    * @function
                    * @name keydown function for delete
                    * @param {object} event control event
                    * @description set focus to optionalfield grid first cell on tab key press
                    */
                    $("#btnDelete").on("keydown", function (e) {
                        if (e.keyCode === tabKeyCode) {
                            var gridData = grid.dataSource.data();
                            var girdTotalRows = gridData.length;
                            if (girdTotalRows >= 1) {//checks the row length is 1 then set the focus to the first row first cell in optional field grid
                                $("#" + ContainerControlEnum.OptionalField + "0").show();
                                $("#" + ContainerControlEnum.SpanOptional + "0").hide();
                                $('#' + InputControlIdEnum.txtGridColOptField + "0").focus();
                            }
                        }
                    });

                    /**
                    * @function
                    * @name on click function for print
                    * @param {object} event control event
                    * @description set focus to last cell in optional field grid on shift + tab key press
                    */
                    $("#btnPrint").on("keydown", function (e) {
                        if (e.shiftKey && e.keyCode === tabKeyCode) {
                            var gridData = grid.dataSource.data();
                            var girdLastRowIndex = gridData.length - 1;
                            if (girdLastRowIndex >= 0) {
                                var type = $("#" + InputControlIdEnum.ColumnType + girdLastRowIndex).val();
                                if (type == '') {
                                    type = ValueTypeEnum.Text;
                                }
                                changeControlType(girdLastRowIndex, type, 'opt', false, true);//"opt" hardcoded since focus will always move from Optional field to "To" column.
                            }
                        }
                    });

                    grid.table.on("keydown", ".CellClickHandler", function (e) {
                        navigateToNextCtl(e, changeControlType);
                    });

                    grid.table.on("keydown", "td", function (e) {
                        navigateToNextCtl(e, changeControlType);
                    });

                    /**
                   * @function
                   * @name on click function
                   * @param {object} event control event
                   * @description Click other than Optionalfld, From and To Dropdown, text, datepicker, Addline, deleteline,print or confirmationpopup yes button in grid then the grid has to be reset
                   */
                    $(document).on("click", function (e) {
                        if (e.target.tagName != "TD" && !Object.values(ControlNameEnum).includes(e.currentTarget.activeElement.name)
                            && !e.currentTarget.activeElement.parentElement.id.includes(ContainerControlEnum.FromDropdown) && !e.currentTarget.activeElement.parentElement.id.includes(ContainerControlEnum.ToDropdown)
                            && !e.target.id.includes(ContainerControlEnum.SpanTo) && !e.target.id.includes(ContainerControlEnum.SpanFrom) && !e.target.id.includes(ContainerControlEnum.SpanOptional)
                            && (e.target.parentNode.parentNode.role != roleForCancelButton) && (e.target.id != yesButtonInConfirmationPopup) && (e.target.id != btnPrint))
                            resetGrid();
                    });

                    grid.table.on("click", ".CellClickHandler", function (e) {
                        if (e.target.tagName == "SPAN" && (e.target.id == '' || !e.target.id.toLowerCase().startsWith("span"))) return;
                        if (e.target.tagName != "INPUT") {
                            hideAllGridInputs();
                            var targetId = '';
                            var targetBtnId = '';
                            $('span[name="' + ControlNameEnum.SpanSelectedValue + '"]').show();

                            var index;
                            if (e.target.tagName == "SPAN") {
                                //get the control id based on the optional type
                                var index = e.target.parentElement.children[0].id.charAt(e.target.parentElement.children[0].id.length - 1);
                                var hdnValue = $("#" + InputControlIdEnum.ColumnType + index).val();
                                if (hdnValue == ValueTypeEnum.Date && !e.target.id.includes(ContainerControlEnum.SpanOptional)) {
                                    targetId = e.target.parentElement.children[1].id; //Date control parent id 
                                }
                                else if (hdnValue == ValueTypeEnum.YesNo && !e.target.id.includes(ContainerControlEnum.SpanOptional)) {
                                    targetId = e.target.parentElement.children[2].id; // Yes/No dropdown parent id
                                }
                                else {
                                    targetId = e.target.parentElement.children[0].id;
                                }
                                $('#' + e.target.id).hide();
                            }
                            else {
                                index = e.target.children[0].id.charAt(e.target.children[0].id.length - 1);
                                if (index == '')
                                    index = e.target.id.charAt(e.target.id.length - 1);
                                var hdnValue = $("#" + InputControlIdEnum.ColumnType + index).val();
                                var target = e.target;

                                if (e.target.tagName == "TD") {
                                    target = target.children[0];
                                }

                                if (e.target.cellIndex && e.target.cellIndex != 0 && hdnValue == ValueTypeEnum.Date) {
                                    targetId = e.target.children[1].id;

                                }
                                else if (e.target.cellIndex && e.target.cellIndex != 0 && hdnValue == ValueTypeEnum.YesNo) {
                                    targetId = e.target.children[2].id;
                                }
                                else {
                                    targetId = target.id;
                                }
                            }

                            if (e.target.cellIndex == 0 || e.target.parentElement.cellIndex == 0)
                                $("#" + ContainerControlEnum.SpanOptional + index).hide();
                            else if (e.target.cellIndex == 1 || e.target.parentElement.cellIndex == 1)
                                $("#" + ContainerControlEnum.SpanFrom + index).hide();
                            else if (e.target.cellIndex == 2 || e.target.parentElement.cellIndex == 2)
                                $("#" + ContainerControlEnum.SpanTo + index).hide();

                            //for clearing span value after deleting the value 
                            var type = $("#" + InputControlIdEnum.ColumnType + index).val();
                            if (type != "" || e.target.cellIndex == 0) {
                                $('#' + targetId).show();
                            }

                            if (type == ValueTypeEnum.Date) {
                                if ($("#" + InputControlIdEnum.FromDateField + index).val() == "") {
                                    $("#" + ContainerControlEnum.SpanFrom + index)[0].textContent = "";
                                }
                                if ($("#" + InputControlIdEnum.ToDate + index).val() == "") {
                                    $("#" + ContainerControlEnum.SpanTo + index)[0].textContent = "";
                                }
                            }
                            //format the amount textbox in cellclick
                            else if (type == ValueTypeEnum.Amount || type == ValueTypeEnum.Integer
                                || type == ValueTypeEnum.Number || type == ValueTypeEnum.Time) {
                                var index;
                                if (e.target.tagName == "SPAN") {
                                    index = e.target.id.charAt(e.target.id.length - 1);
                                }
                                else {
                                    index = e.target.children[0].id.charAt(e.target.children[0].id.length - 1);
                                }

                                var previousIndex = index;
                                if (currentIndex != index) {
                                    previousIndex = currentIndex;
                                    currentIndex = index;
                                }

                                if (previousIndex == index) {
                                    formattedTextBox(index, type);
                                }
                                else {
                                    formattedTextBox(previousIndex, $("#" + InputControlIdEnum.ColumnType + previousIndex).val());
                                }
                            }
                            else if (type == ValueTypeEnum.Text) {
                                if ($("#" + InputControlIdEnum.FromTextField + index).val() == "") {
                                    $("#" + ContainerControlEnum.SpanFrom + index)[0].textContent = "";
                                }
                                if ($("#" + InputControlIdEnum.ToTextField + index).val() == "") {
                                    $("#" + ContainerControlEnum.SpanTo + index)[0].textContent = "";
                                }
                            }

                            if (targetBtnId.includes('btn'))
                                $('#' + targetBtnId).show();

                            if (targetId.includes("Dropdown")) {
                                var input = $("#" + targetId).find("input")[0];
                                var dropdown = $(input).data("kendoDropDownList");
                                dropdown.focus();
                                setTimeout(function () {
                                    sg.controls.Focus(dropdown);
                                }, 100);
                            }
                            else {
                                setTimeout(function () {
                                    sg.controls.Focus($($('#' + targetId)).find("input")[0]);
                                }, 100);
                            }
                        }
                    });

                     //Format the textfinder focus out will get triggered
                    $(document).on('blur', ".textFinder", function (e) {
                        $("#" + e.target.id).closest('td')[0].childNodes[3].textContent = $("#" + e.target.id).val();
                        var index = e.target.id.charAt(e.target.id.length - 1);
                        formattedTextBox(index, $("#" + InputControlIdEnum.ColumnType + index).val());
                    });

                    //Format the date focus out will get triggered
                    $(document).on('blur', ".dateValue", function (e) {
                        $("#" + e.target.id).closest('td')[0].childNodes[3].textContent = $("#" + e.target.id).val();
                        var index = e.target.id.charAt(e.target.id.length - 1);
                        formattedTextBox(index, $("#" + InputControlIdEnum.ColumnType + index).val());
                    });

                    /**
                   * @function
                   * @name optionalFinderInfoFocus
                   * @description optionalfiedls set control focus               
                   */
                    function optionalFinderInfoFocus() {
                        optionalFinderInfo(ControlNameEnum.OptionalField, InputControlIdEnum.FromTextField);
                        optionalFinderInfo(ControlNameEnum.FromColumn, InputControlIdEnum.ToTextField);
                        optionalFinderInfo(ControlNameEnum.OptionalField, InputControlIdEnum.FromDateField);
                        optionalFinderInfo(ControlNameEnum.FromDateColumn, InputControlIdEnum.ToDate);
                        optionalFinderInfo(ControlNameEnum.FromDropdown, InputControlIdEnum.ToDropdown);
                        optionalFinderInfo(ControlNameEnum.ToColumn, InputControlIdEnum.OptionalTextField, true);
                    }

                    /**
                  * @function
                  * @name KeyRestriction
                  * @param {object} event control event
                  * @description keyrestriction for textbox
                  */
                    function KeyRestriction(event, type) {
                        if (type != ValueTypeEnum.Time) {
                            minusKeyValidations(event);
                        }

                        if (event.which === Keycodes.Plus) {
                            event.preventDefault();
                        }

                        if (type == ValueTypeEnum.Number || type == ValueTypeEnum.Amount) {
                            //Allow decimal point
                            if (event.which === Keycodes.Period || event.which === Keycodes.DecimalPoint) {
                                if (event.target.value.includes('.')) { event.preventDefault(); }
                            }
                            var keyCodes = [Keycodes.Delete, Keycodes.Backspace, Keycodes.Tab, Keycodes.Escape, Keycodes.Enter, Keycodes.Minus, Keycodes.Dash, Keycodes.Period, Keycodes.DecimalPoint]
                        }
                        if (type == ValueTypeEnum.Integer) {
                            var keyCodes = [Keycodes.Tab, Keycodes.Escape, Keycodes.Enter, Keycodes.Minus, Keycodes.Dash]
                        }
                        else if (type == ValueTypeEnum.Time) {
                            var keyCodes = [Keycodes.Tab, Keycodes.Escape, Keycodes.Enter, Keycodes.Backspace, Keycodes.Delete]
                        }
                        if (keyCodes.indexOf(event.keyCode) > -1 ||
                            (event.keyCode == Keycodes.KeyValueA && event.ctrlKey === true)) {
                            return;
                        }
                        else {
                            if (event.shiftKey || ((event.keyCode != Keycodes.Plus && event.keyCode < Keycodes.Zero) || event.keyCode > Keycodes.Nine)) {
                                event.preventDefault();
                            }
                        }
                    }

                    /**
                 * @function
                 * @name minusKeyValidations
                 * @param {object} event control event
                 * @description Minus key restriction for amount, number and integer textboxes
                 */
                    function minusKeyValidations(event) {
                        //Minus"-" symbol should allow only in prefix for negative values
                        if (event.which === Keycodes.Dash || event.which === Keycodes.Minus) {
                            if ((event.target.value.indexOf('-') >= 0 || event.target.selectionStart > 0)) { event.preventDefault(); }

                        }
                        else {
                            if ((event.target.selectionStart > 0 && event.target.selectionEnd != event.target.value.length)) {
                                if ((event.target.value.indexOf('-') == event.target.value.length)) {
                                    event.preventDefault();
                                }
                            }
                        }
                        //restricting Minus symbol in suffix for numeric digits
                        if ((event.which === Keycodes.Dash || event.which === Keycodes.Minus) && event.target.value.includes('-') && (event.target.selectionStart > 0)) {
                            event.preventDefault();
                        }
                        //allow backspace and delete while whole input field value is selected
                        if ((event.target.value.indexOf('-') >= 0) && (event.which === Keycodes.Dash || event.which === Keycodes.Minus) && (event.target.selectionStart <= event.target.value.indexOf('-')) && (event.which != Keycodes.Backspace && event.which != Keycodes.Delete)) {
                            event.preventDefault();
                        }
                    }
                    /**
                   * @function
                   * @name optionalFinderInfo
                   * @description setfocus for controls
                   * @param {string} sourceField setfocus
                   * @param {string} targetField is next control focus
                   * @param {boolean} isNextRowFocus
                   */
                    var optionalFinderInfo = (sourceField, targetField, isNextRowFocus = false) => {
                        $('input[name="' + sourceField + '"]').keypress(function (event) {
                            // check for hyphen    
                            if (event.which === Keycodes.Tab) {
                                if ($('input[name="' + sourceField + '"]').val() == "-") {
                                    $('input[name="' + sourceField + '"]').val("");
                                }
                                event.preventDefault();
                                var index = event.target.id.charAt(event.target.id.length - 1);
                                if (isNextRowFocus) {
                                    index = parseInt(index) + 1;
                                }
                                setTimeout(function () {
                                    sg.controls.Focus($("#" + targetField + index));
                                }, 200);
                            }
                            var index = event.target.id.charAt(event.target.id.length - 1);
                            var type = $("#" + InputControlIdEnum.ColumnType + index).val();

                           //Keycode restriction for Amount, Integer, Number,Time
                            if ((type == ValueTypeEnum.Integer) && (sourceField == ControlNameEnum.FromColumn || sourceField == ControlNameEnum.ToColumn)) {
                                $("#" + InputControlIdEnum.FromTextField + index).attr("maxlength", intTextBoxMaxLength)
                                $("#" + InputControlIdEnum.ToTextField + index).attr("maxlength", intTextBoxMaxLength)
                            }
                            if (type != ValueTypeEnum.Text && (sourceField == ControlNameEnum.FromColumn || sourceField == ControlNameEnum.ToColumn)) {
                                KeyRestriction(event, type);
                            }
                            if (type == ValueTypeEnum.Text && (sourceField == ControlNameEnum.FromColumn || sourceField == ControlNameEnum.ToColumn)) {
                                $("#" + InputControlIdEnum.FromTextField + index).attr("formatTextbox", "alphaNumeric");
                                $("#" + InputControlIdEnum.ToTextField + index).attr("formatTextbox", "alphaNumeric");
                            }
                        });
                    }

                    /** @name dynamicID
                        @description generating the dynamic id generation 
                        @param { number } i index
                     */
                    function dynamicID(i) {
                        let grid = $('#gdOptFields').data("kendoGrid");
                        var gridRow = grid.element[0].children[3].children[0].children[1].children[i];

                        //optionalfield index value
                        var optionalFieldDiv = gridRow.children[0].children[0].children;
                        gridRow.children[0].children[0].id = ContainerControlEnum.OptionalField + i;
                        optionalFieldDiv[1].id = InputControlIdEnum.OptionalButtonField + i;
                        optionalFieldDiv[0].id = InputControlIdEnum.OptionalTextField + i;
                        var optionalFieldSpan = gridRow.children[0].children[1];
                        optionalFieldSpan.id = optionalFieldSpan.id + i;

                        //from index value
                        var fromColumnDiv = gridRow.children[1].children;
                        var fromOptControls = fromColumnDiv[0].children;
                        fromOptControls[1].id = InputControlIdEnum.FromButton + i;
                        fromOptControls[0].id = InputControlIdEnum.FromTextField + i;
                        fromColumnDiv[3].id = fromColumnDiv[3].id + i;
                        fromColumnDiv[0].id = ContainerControlEnum.FromText + i;
                        fromColumnDiv[1].id = ContainerControlEnum.FromDate + i;
                        fromColumnDiv[2].id = ContainerControlEnum.FromDropdown + i;
                        fromColumnDiv[1].children[0].id = InputControlIdEnum.FromDateField + i;
                        fromColumnDiv[2].children[0].id = InputControlIdEnum.FromDropdown + i;

                        //to index value
                        var toColumnDiv = gridRow.children[2].children;
                        var optControls = toColumnDiv[0].children;
                        optControls[1].id = InputControlIdEnum.ToButton + i;
                        optControls[0].id = InputControlIdEnum.ToTextField + i;
                        toColumnDiv[3].id = toColumnDiv[3].id + i;
                        toColumnDiv[0].id = ContainerControlEnum.ToText + i;
                        toColumnDiv[1].id = ContainerControlEnum.ToDate + i;
                        toColumnDiv[2].id = ContainerControlEnum.ToDropdown + i;
                        toColumnDiv[1].children[0].id = InputControlIdEnum.ToDate + i;
                        toColumnDiv[2].children[0].id = InputControlIdEnum.ToDropdown + i;
                        gridRow.children[3].children[0].children[0].id = InputControlIdEnum.ColumnType + i;
                        gridRow.children[4].children[0].children[0].id = InputControlIdEnum.ColumnDecimal + i;
                    }

                    /**
                   * @function
                   * @name resetGrid
                   * @description For reseting the grid and keep all the 
                   * */
                    function resetGrid() {
                        $('span[name="' + ControlNameEnum.SpanSelectedValue + '"]').show();
                        hideAllGridInputs();
                    }

                    /**
                     * @function
                     * @name addRowElementVisiblity
                     * @description control visiblity check while add row.
                     * @param { string } element input control
                     * @param { string } displayElement span controls
                     * @param { number } index index
                     * @param { boolean } isShiftTab boolean value 
                     * @param { string } sourceElementId event raised control id 
                     */
                    function addRowElementVisiblity(element = null, displayElement = null, index = -1, isShiftTab = false, sourceElementId = '') {
                        resetGrid();
                        let grid = $('#gdOptFields').data("kendoGrid");
                        var gridData = grid.dataSource.data();
                        var girdTotalRows = gridData.length;
                        var rowIndex = parseInt(index);
                        if (isShiftTab) {
                            if (rowIndex == -1 && sourceElementId.toLowerCase().includes("opt")) {
                                setTimeout(function () {
                                    sg.controls.Focus($('#btnDelete'));
                                }, 10);
                                return;
                            }
                        }
                        else {
                            if (girdTotalRows == rowIndex + 1 && displayElement.toLowerCase().includes("opt")) {
                                setTimeout(function () {
                                    sg.controls.Focus($('#btnPrint'));
                                }, 10);
                                return;
                            }
                        }
                        var txtOpt = $("#" + InputControlIdEnum.OptionalTextField + (index));
                        if (txtOpt.val() != "") {
                            $("#" + displayElement).hide();
                            if (element) {
                                $("#" + element).show();
                                $("#" + element).focus();
                                if ($("#" + element)[0].firstChild.id == '') {
                                    if (element.includes("Dropdown")) {
                                        var input = $("#" + element).find("input")[0];
                                        var dropdown = $(input).data("kendoDropDownList");
                                        setTimeout(function () {
                                            sg.controls.Focus(dropdown);
                                        }, 100);
                                    }
                                    else
                                        $("#" + $('#' + element)[0].firstChild.firstChild.firstChild.id)[0].focus();
                                }
                                else {
                                    var childElement = $("#" + $('#' + element)[0].firstChild.id);
                                    childElement[0].focus();
                                    setTimeout(function () {
                                        sg.controls.Focus(childElement);
                                    }, 100);
                                }

                            }
                        }
                        else {
                            let grid = $('#gdOptFields').data("kendoGrid");
                            var gridData = grid.dataSource.data();
                            setTimeout(function () {
                                $("#" + ContainerControlEnum.OptionalField + (gridData.length - 1)).show();
                                $("#" + ContainerControlEnum.OptionalField + (gridData.length - 1)).focus();
                                sg.controls.Focus($("#" + InputControlIdEnum.OptionalTextField + (gridData.length - 1)));
                            }, 100);
                            $("#" + ContainerControlEnum.SpanOptional + (gridData.length - 1)).hide();
                        }
                    }

                    /**
                     * @function
                     * @name changecontroltype
                     * @description optionalfields controltypes
                     * @param {number} index index
                     * @param {string} optionalFieldType control type
                     * @param {string} elementId as string
                     * @param {boolean} isTypeChange as boolean
                     * @param {boolean} isShiftTab as boolean
                     */
                    function changeControlType(index, optionalFieldType, elementId = '', isTypeChange = false, isShiftTab = false) {
                        var element = '';
                        var displayElement = '';
                        var decimal = '';
                        var digits = '';
                        var numberdefaultval = "9"

                        if (optionalFieldType == ValueTypeEnum.Number) {
                            var decimals = $("#" + InputControlIdEnum.ColumnDecimal + index).val();
                            decimal = numberdefaultval.repeat(decimals).toString();
                            digits = numberdefaultval.repeat(15 - decimals).toString();
                        }

                        var defaultValues = [
                            [ValueTypeEnum.Text, '', 'ZZZZZZZZZZZZZZZZZZZZZZZZZZZZ'],
                            [ValueTypeEnum.Number, '-' + digits + "." + decimal, digits + "." + decimal], //MaxLength For Number should be 15. Since decimal points placed we can enter only 12
                            [ValueTypeEnum.Time, '00:00:00', '23:59:59'],
                            [ValueTypeEnum.Integer, '-2147483647', '2147483647'],
                            [ValueTypeEnum.Amount, '-999999999999.999', '999999999999.999'],
                            [ValueTypeEnum.Date, '', '12/31/9999']
                        ]
                        var txtGridColFromVal = '';
                        var txtGridColToVal = '';

                        //set the default values for textboxes
                        for (var i = 0; i < defaultValues.length; i++) {
                            if (defaultValues[i][0] == optionalFieldType) {
                                txtGridColFromVal = defaultValues[i][1];
                                txtGridColToVal = defaultValues[i][2];
                                break;
                            }
                        }

                        //Move the focus to Optional Field from From field if i click shift tab
                        if (isShiftTab) {
                            if (elementId.toLowerCase().indexOf("from") != -1) {
                                element = ContainerControlEnum.OptionalField + index;
                                displayElement = ContainerControlEnum.SpanOptional + index;
                            }
                        }
                        //Move the focus to Optional Field from To field if i click tab
                        else {
                            if (elementId.toLowerCase().indexOf("to") != -1) {
                                var nextRowIndex = parseInt(index) + 1;
                                element = ContainerControlEnum.OptionalField + nextRowIndex;
                                displayElement = ContainerControlEnum.SpanOptional + nextRowIndex;
                            }
                        }

                        /**
                        * @function
                        * @name setTimePattern
                        * @description Set the pattern for time
                        * @param {string} ctrlId Control Id
                        */
                        function setTimePattern(ctrlId) {
                            $("#" + ctrlId + index).mask('Hh:Mm:Ss', {
                                'translation': {
                                    H: { pattern: /[0-9]/ },//allowing only to enter 0 to 2 for hours
                                    h: { pattern: /[0-9]/ },//allowing only to enter 0 to 9 for hours
                                    M: { pattern: /[0-5]/ },//allowing only to enter 0 to 5 for Mins
                                    m: { pattern: /[0-9]/ },//allowing only to enter 0 to 9 for Mins
                                    S: { pattern: /[0-5]/ },//allowing only to enter 0 to 5 for Mins
                                    s: { pattern: /[0-9]/ } //allowing only to enter 0 to 9 for Mins
                                }
                            });
                        }

                        switch (optionalFieldType) {
                            case ValueTypeEnum.Text:
                            case ValueTypeEnum.Number:
                            case ValueTypeEnum.Time:
                            case ValueTypeEnum.Integer:
                            case ValueTypeEnum.Amount:

                                if (optionalFieldType == ValueTypeEnum.Time) {
                                    setTimePattern(InputControlIdEnum.FromTextField);
                                    setTimePattern(InputControlIdEnum.ToTextField);
                                }
                                else {
                                    $("#" + InputControlIdEnum.FromTextField + index).unmask();
                                    $("#" + InputControlIdEnum.ToTextField + index).unmask();
                                }

                                if (isTypeChange) {
                                    $("#" + InputControlIdEnum.ToTextField + index).val(txtGridColToVal);
                                    $("#" + InputControlIdEnum.FromTextField + index).val(txtGridColFromVal);
                                }

                                if (isShiftTab) {
                                    ({ element, displayElement } = getShiftTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromText, ContainerControlEnum.ToText));
                                }
                                else {
                                    ({ element, displayElement } = getTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromText, ContainerControlEnum.ToText));
                                }

                                if (optionalFieldType == ValueTypeEnum.Integer || optionalFieldType == ValueTypeEnum.Number || optionalFieldType == ValueTypeEnum.Amount || optionalFieldType == ValueTypeEnum.Time) {
                                    formattedTextBox(index, optionalFieldType);
                                }
                                else {
                                    $("#" + ContainerControlEnum.SpanFrom + index)[0].textContent = $("#" + InputControlIdEnum.FromTextField + index).val();
                                    $("#" + ContainerControlEnum.SpanTo + index)[0].textContent = $("#" + InputControlIdEnum.ToTextField + index).val();
                                }
                                if (optionalFieldType == ValueTypeEnum.Number || optionalFieldType == ValueTypeEnum.Amount) {
                                    initGridNumericTextboxes(index)
                                }
                                break;
                            case ValueTypeEnum.Date:
                                if (isTypeChange) {
                                    $("#" + InputControlIdEnum.FromDateField + index).val('');
                                    $("#" + InputControlIdEnum.ToDate + index).val('12/31/9999');
                                }

                                if (isShiftTab) {
                                    ({ element, displayElement } = getShiftTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromDate, ContainerControlEnum.ToDate));
                                } else {
                                    ({ element, displayElement } = getTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromDate, ContainerControlEnum.ToDate));
                                }

                                    $("#" + ContainerControlEnum.SpanFrom + (index))[0].textContent = $("#" + InputControlIdEnum.FromDateField + index).val();
                                    $("#" + ContainerControlEnum.SpanTo + (index))[0].textContent = $("#" + InputControlIdEnum.ToDate + index).val();
                              
                                break;
                            case ValueTypeEnum.YesNo:
                                declarativeReportUI.yesNoList.sort(function (x, y) {
                                    let a = x.Text.toUpperCase(),
                                        b = y.Text.toUpperCase();
                                    return a == b ? 0 : a > b ? 1 : -1;
                                });
                                $("#" + InputControlIdEnum.FromDropdown + index).kendoDropDownList({
                                    dataTextField: "Text",
                                    dataValueField: "Value",
                                    change: onFromYesNoChange,
                                    dataSource: declarativeReportUI.yesNoList
                                });
                                $("#" + InputControlIdEnum.ToDropdown + index).kendoDropDownList({
                                    dataTextField: "Text",
                                    dataValueField: "Value",
                                    value: declarativeReportUI.yesNoList[declarativeReportUI.yesNoList.length - 1].Value,
                                    change: onToYesNoChange,
                                    dataSource: declarativeReportUI.yesNoList
                                });

                                if (isShiftTab) {
                                    ({ element, displayElement } = getShiftTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromDropdown, ContainerControlEnum.ToDropdown));
                                }
                                else {
                                    ({ element, displayElement } = getTabElementDetails(elementId, element, index, displayElement, ContainerControlEnum.FromDropdown, ContainerControlEnum.ToDropdown));

                                }

                                $("#" + ContainerControlEnum.SpanFrom + (index))[0].textContent = $("#" + InputControlIdEnum.FromDropdown + index).val();
                                $("#" + ContainerControlEnum.SpanTo + (index))[0].textContent = $("#" + InputControlIdEnum.ToDropdown + index).val();
                                break;
                        }
                        $("#" + ContainerControlEnum.SpanOptional + (index))[0].textContent = $("#" + InputControlIdEnum.OptionalTextField + index).val().toUpperCase();
                        addRowElementVisiblity(element, displayElement, index, isShiftTab, elementId);
                    }

                    /**
                     * @function
                     * @name onFromYesNoChange
                     * @description hide and show for yes or no dropdown
                     * @param {object} e control event
                     */
                    function onFromYesNoChange(e) {
                        var index = e.sender.element[0].id.charAt(e.sender.element[0].id.length - 1);
                        $("#" + ContainerControlEnum.SpanFrom + (index))[0].textContent = e.sender.element.val()
                        $("#" + ContainerControlEnum.FromDropdown + index).hide();
                        $("#" + ContainerControlEnum.SpanFrom + (index)).show();
                        $("#" + ContainerControlEnum.ToDropdown + index).show();
                        $("#" + ContainerControlEnum.SpanTo + (index)).hide();
                        $("#" + InputControlIdEnum.ToDropdown + index).focus();
                        setTimeout(function () {
                            sg.controls.Focus($("#" + InputControlIdEnum.ToDropdown + index));
                        }, 200);
                    }

                    /**
                     * @function
                     * @name onToYesNoChange
                     * @description hide and show for yes or no dropdown
                     * @param {object} e control event
                     */
                    function onToYesNoChange(e) {
                        var index = e.sender.element[0].id.charAt(e.sender.element[0].id.length - 1);
                        $("#" + ContainerControlEnum.SpanTo + (index))[0].textContent = e.sender.element.val()
                        $("#" + ContainerControlEnum.ToDropdown + index).hide();
                        $("#" + ContainerControlEnum.SpanTo + (index)).show();
                    }

                    /**
                     * @function
                     * @name onFromDateChange
                     * @description hide and show for from date change 
                     * @param {object} e control event
                     */
                    function onFromDateChange(e) {
                        var index = e.sender.element[0].id.charAt(e.sender.element[0].id.length - 1);
                        $("#" + ContainerControlEnum.SpanFrom + (index))[0].textContent = e.sender.element.val()
                        $("#" + ContainerControlEnum.FromDate + index).hide();
                        $("#" + ContainerControlEnum.ToDate + index).show();
                        $("#" + ContainerControlEnum.SpanFrom + (index)).show();
                        $("#" + ContainerControlEnum.SpanTo + (index)).hide();
                        $("#" + InputControlIdEnum.ToDate + index).focus();
                    }

                    /**
                     * @function
                     * @name onToDateChange
                     * @description hide and show for to date change
                     * @param {object} e control event
                     */
                    function onToDateChange(e) {
                        var index = e.sender.element[0].id.charAt(e.sender.element[0].id.length - 1);
                        $("#" + ContainerControlEnum.SpanTo + (index))[0].textContent = e.sender.element.val();
                        $("#" + ContainerControlEnum.ToDate + index).hide();
                        $("#" + ContainerControlEnum.SpanTo + (index)).show();

                    }

                    //delete selected row from the grid
                    $('#btnDelete').click(function (e) {
                        let grid = $('#gdOptFields').data("kendoGrid");
                        var gridData = grid.dataSource.data();

                        if (gridData.length == 0) {
                            $("#btnDelete").enable(false);
                        }
                        else {
                            var selectedIndex = grid.select().index();
                            if (selectedIndex > -1) {
                                sg.utls.showKendoConfirmationDialog(
                                    function () {
                                        var dataItem = grid.dataItem(grid.select());
                                        var selectedOptions = [];
                                        var selectedFromOptions = [];
                                        var selectedToOptions = [];
                                        var selectedToDate = [];
                                        var selectedType = [];
                                        var selectedDecimal = [];
                                        var selectedFromDate = [];
                                        var selectedFromDropDownValue = [];
                                        var selectedToDropDownValue = [];
                                        //push the selected value in the grid 
                                        for (var i = 0; i < gridData.length; i++) {
                                            selectedOptions.push($("#" + InputControlIdEnum.OptionalTextField + i).val());
                                            selectedType.push($("#" + InputControlIdEnum.ColumnType + i).val());
                                            selectedFromOptions.push($("#" + InputControlIdEnum.FromTextField + i).val());
                                            selectedToOptions.push($("#" + InputControlIdEnum.ToTextField + i).val());
                                            selectedToDate.push($("#" + InputControlIdEnum.ToDate + i).val());
                                            selectedFromDate.push($("#" + InputControlIdEnum.FromDateField + i).val());
                                            selectedFromDropDownValue.push($("#" + InputControlIdEnum.FromDropdown + i).val());
                                            selectedToDropDownValue.push($("#" + InputControlIdEnum.ToDropdown + i).val());
                                            selectedDecimal.push($("#" + InputControlIdEnum.ColumnDecimal + i).val());
                                        }

                                        //get the row index and removing the selected row
                                        var rowIndex = grid.select()[0].rowIndex;
                                        var firstDataItem = gridData[rowIndex];
                                        grid.refresh(firstDataItem);
                                        grid.dataSource.remove(firstDataItem);

                                        selectedOptions.splice(rowIndex, 1);
                                        selectedFromOptions.splice(rowIndex, 1);
                                        selectedToOptions.splice(rowIndex, 1);
                                        selectedType.splice(rowIndex, 1);
                                        selectedDecimal.splice(rowIndex, 1);
                                        selectedFromDate.splice(rowIndex, 1);
                                        selectedToDate.splice(rowIndex, 1);

                                        //clearing the existing datas in hidden parameter
                                        for (var i = 0; i < gridData.length + 1; i++) {
                                            $("#" + HdnFieldEnum.OptField + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.FromDisplayValue + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.ToDisplayValue + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.FromFilterValue + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.ToFilterValue + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.OptDisplayFld + (i + 1)).val('');
                                            $("#" + HdnFieldEnum.OptFldType + (i + 1)).val('');
                                        }

                                        for (var i = 0; i < gridData.length; i++) {
                                            dynamicID(i);
                                            setFinderEvent(i, changeControlType);
                                            setFinders(i, selectedType[i]);
                                            var txtGridColOptField = $("#" + InputControlIdEnum.OptionalTextField + i);

                                            //Rebind the selected values in textbox,datepicker and dropdown after click the add button
                                            txtGridColOptField.val(selectedOptions[i])
                                            txtGridColOptField.data(dataVal, selectedOptions[i]);
                                            $("#" + InputControlIdEnum.ColumnType + i).val(selectedType[i])
                                            $("#" + InputControlIdEnum.ColumnDecimal + i).val(selectedDecimal[i])
                                            $("#" + ContainerControlEnum.SpanOptional + i)[0].textContent = selectedOptions[i];
                                            $("#" + InputControlIdEnum.FromTextField + i).val(selectedFromOptions[i])

                                            optionalFinderInfo(ControlNameEnum.FromColumn, InputControlIdEnum.ToTextField);

                                            $("#" + InputControlIdEnum.ToTextField + i).val(selectedToOptions[i])
                                            $("#" + InputControlIdEnum.ToDate + i).kendoDatePicker({
                                                value: selectedToDate[i]
                                            });
                                            $("#" + InputControlIdEnum.FromDateField + i).kendoDatePicker({
                                                value: selectedFromDate[i]
                                            });
                                            $("#" + InputControlIdEnum.FromDropdown + i).val(selectedFromDropDownValue[i]);
                                            $("#" + InputControlIdEnum.ToDropdown + i).val(selectedToDropDownValue[i])

                                            changeControlType(i, selectedType[i], "#" + ContainerControlEnum.OptionalField + i);

                                            $("#" + InputControlIdEnum.OptionalButtonField + i).bind('click', function (e) {
                                                isFinderCancelEvent = true;
                                            });

                                            //onchange event for optional field column invalid input validation
                                            $(txtGridColOptField).bind('change', function (e) {
                                                var sender = e.currentTarget.id;
                                                var index = sender.charAt(sender.length - 1);
                                                var spanOptionalFieldVal = $("#" + ContainerControlEnum.SpanOptional + (index))[0].textContent;
                                                sg.delayOnChange(InputControlIdEnum.OptionalButtonField + index, $("#" + InputControlIdEnum.OptionalTextField + index), function () {
                                                    if (isFinderCancelEvent) {
                                                        if (spanOptionalFieldVal != "") {
                                                            $("#" + InputControlIdEnum.OptionalTextField + index).val(spanOptionalFieldVal);
                                                        }
                                                        else if ($("#" + InputControlIdEnum.ColumnType + index).val() == "") {
                                                            $("#" + InputControlIdEnum.OptionalTextField + index).val('');
                                                        }
                                                        $("#" + ContainerControlEnum.OptionalField + index).focus();
                                                    }
                                                    else {
                                                        _sendRequest(e, changeControlType, index);
                                                    }
                                                    isFinderCancelEvent = false;
                                                });
                                            });
                                        }
                                        $("#btnDelete").enable(true);// show delete button in toolbar

                                        if (dataItem.KendoGridAccpacViewIsRecordNew) {
                                            grid.dataSource.remove(dataItem);
                                        }
                                        if (gridData.length == 0) {
                                            $("#btnDelete").enable(false);
                                        }
                                        else {
                                            setOptionalFieldFocus(gridData.length - 1);
                                        }
                                        $('#btnAdd').enable(true);
                                        addRowElementVisiblity();
                                    },
                                    function () { },
                                    globalResource.DeleteLineMessage, window.DeleteTitle);
                            }
                        }

                    });

                    $("#btnDelete").enable(false);// hide delete button in toolbar
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
            window.sg.utls.openReport(result.ReportToken, null, declarativeReportOnSuccess.onClose);
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

/**
  * @function
  * @name getTabElementDetails
  * @description 
  * @param {string} elementId event raised control id
  * @param {string} element next focus element id
  * @param {number} index row index
  * @param {string} displayElement span value display control
  * @param {string} fromElementName From element name
  * @param {string} toElementName To element name
  */
function getTabElementDetails(elementId, element, index, displayElement, fromElementName, toElementName) {
    if (elementId.toLowerCase().indexOf("from") != -1) {
        element = toElementName + index;
        displayElement = ContainerControlEnum.SpanTo + index;
    }
    else if (elementId.toLowerCase().indexOf("opt") != -1) {
        element = fromElementName + index;
        displayElement = ContainerControlEnum.SpanFrom + index;
    }
    return { element, displayElement };
}

/**
  * @function
  * @name getShiftTabElementDetails
  * @description
   * @param {string} elementId event raised control id
  * @param {string} element next focus element id
  * @param {number} index row index
  * @param {string} displayElement span value display control
  * @param {string} fromElementName From element name
  * @param {string} toElementName To element name
  */
function getShiftTabElementDetails(elementId, element, index, displayElement, fromElementName, toElementName) {
    if (elementId.toLowerCase().indexOf("to") != -1) {
        element = fromElementName + index;
        displayElement = ContainerControlEnum.SpanFrom + index;
    }
    else if (elementId.toLowerCase().indexOf("opt") != -1) {
        element = toElementName + index;
        displayElement = ContainerControlEnum.SpanTo + index;
    }
    return { element, displayElement };
}
/**
  * @funct
  * @name navigateToNextCtl
  * @description focus for grid controls
  * @param { object } e control event
  * @param { Function } changeControlType
 */
function navigateToNextCtl(e, changeControlType) {
    if (e.keyCode === tabKeyCode || e.keyCode === enterKeyCode) {
        var targetId = e.target.id;
        var index = e.target.id.charAt(e.target.id.length - 1);
        if (index == '') {
            var input = $(e.target).find("input")[0];
            targetId = input.id;
            index = targetId.charAt(targetId.length - 1);
        }

        var typeIndex = parseInt(index);
        if (e.shiftKey && targetId.toLowerCase().indexOf("opt") != -1) {
            typeIndex -= 1; //moving to the previous row control
        }

        var type = $("#" + InputControlIdEnum.ColumnType + typeIndex).val();
        if (type == '') {
            type = ValueTypeEnum.Text;
        }

        changeControlType(typeIndex, type, targetId, false, e.shiftKey);
        isFinderCancelEvent = false;
    }
}

/**
  * @function
  * @name hideAllGridInputs
  * @description hide all grid controls
 */
function hideAllGridInputs() {
    $('div[name="' + ControlNameEnum.OptionalField + '"]').hide();
    $('div[name="' + ContainerControlEnum.FromText + '"]').hide();
    $('div[name="' + ContainerControlEnum.ToText + '"]').hide();
    $('div[name="' + ControlNameEnum.FromColumn + '"]').hide();
}

/** @name setFinderEvent
    @description set the finders for optionalfield and binding the values in textbox
    @param { number } i index
    @param { Function } changeControlType change the control type function
 */
function setFinderEvent(i, changeControlType) {
    var optionalFinderInfo = () => {
        let property = declarativeReportUI.fieldProperties.OptionalFields;
        const optionalFieldValue = $("#" + InputControlIdEnum.OptionalTextField + (i)).val();
        property.initKeyValues = [locationId, optionalFieldValue];
        property.filter = $.validator.format(property["filterTemplate"], locationId);
        property.viewID = sg.viewFinderHelper.entityContextReplacement(property.viewID, moduleId);
        return property;
    };
    sg.viewFinderHelper.setViewFinder(InputControlIdEnum.OptionalButtonField + i,
        function (result) {
            if (result) {
                setOptionalFieldValue(result, changeControlType, i, true)
            }
        }, optionalFinderInfo,
        function () {
            $('#' + InputControlIdEnum.OptionalTextField + i).focus();
        });
}

/**@name setOptionalFieldValue
   @description set the optional field value and change control type
   @param { object } result Opional field object
   @param { Function } changeControlType change the control type based on the optional field type
   @param { number } index row index
   */
function setOptionalFieldValue(result, changeControlType, index,isFinder = false) {
    if (result) {

        var optionalfield = result.OPTFIELD;
        var optionalFieldType = result.TYPE;

        if (!isFinder) {
            var optionalFieldType = ValueTypeEnum[optionalFieldType.replaceAll('/', '')];
            $('#' + InputControlIdEnum.ColumnType + index).val(optionalFieldType);
        } else {
            $('#' + InputControlIdEnum.ColumnType + index).val(result.TYPE);
        }
        $('#' + InputControlIdEnum.ColumnDecimal + index).val(result.DECIMALS);
        $("#" + ContainerControlEnum.OptionalField + index).hide();
        $("#" + ContainerControlEnum.SpanOptional + index).show();
        $("#" + ContainerControlEnum.SpanOptional + index)[0].textContent = optionalfield.toUpperCase();
        $("#" + InputControlIdEnum.OptionalTextField + index).val(optionalfield);
        $("#" + InputControlIdEnum.OptionalTextField + index).data(dataVal, $("#" + InputControlIdEnum.OptionalTextField + index).val());
        changeControlType(index, optionalFieldType.toString(), "#" + InputControlIdEnum.OptionalTextField + index, true);
        setFinders(index, optionalFieldType.toString());
    }
}
/**
 * @name initNumericTextboxes
 *  @param { number } index row index
 * @description Initialize the numeric textboxes, if any
*/
function initGridNumericTextboxes(index) {
    var decimals = $('#' + InputControlIdEnum.ColumnDecimal + index).val();
    var length = numericTextBoxMaxLength - decimals;
    sg.utls.kndoUI.restrictDecimals($("#" + InputControlIdEnum.FromTextField + index), decimals, length);
    sg.utls.kndoUI.restrictDecimals($("#" + InputControlIdEnum.ToTextField + index), decimals, length);
}

/**@name setFinders
    @description set the finders for all controls
    @param { number } index index
    @param { string } optionalFieldType Type for controls
 */
function setFinders(index, optionalFieldType) {
    var property = {};
    switch (optionalFieldType) {
        case ValueTypeEnum.Text:
            property = declarativeReportUI.fieldProperties.TextOptionalFieldValue;
            break;
        case ValueTypeEnum.Time:
            property = declarativeReportUI.fieldProperties.TimeOptionalFieldValue;
            break;
        case ValueTypeEnum.Amount:
            property = declarativeReportUI.fieldProperties.AmountOptionalFieldValue;
            break;
        case ValueTypeEnum.Integer:
            property = declarativeReportUI.fieldProperties.IntegerOptionalFieldValue;
            break;
        case ValueTypeEnum.Number:
            property = declarativeReportUI.fieldProperties.NumberOptionalFieldValue;
            break;
    }

    /** @name fromOptionalFinderInfo
        @description From Text box Finder 
     */
    var fromOptionalFinderInfo = () => {
        const optionalFieldValue = $("#" + InputControlIdEnum.OptionalTextField + (index)).val();
        if (optionalFieldValue != "") {
            const fromValue = $("#" + InputControlIdEnum.FromTextField + (index)).val();
            property.initKeyValues = [optionalFieldValue, fromValue];
            property.filter = $.validator.format(property["filterTemplate"], optionalFieldValue);
        }
        return property;
    };

    /** @name toOptionalFinderInfo
        @description To Text box Finder
     */
    var toOptionalFinderInfo = () => {
        const optionalFieldValue = $("#" + InputControlIdEnum.OptionalTextField + (index)).val();
        if (optionalFieldValue != "") {
            const toValue = $("#" + InputControlIdEnum.ToTextField + (index)).val();
            property.initKeyValues = [optionalFieldValue, toValue];
            property.filter = $.validator.format(property["filterTemplate"], optionalFieldValue);
        }
        return property;
    };

    sg.viewFinderHelper.setViewFinder(InputControlIdEnum.FromButton + index,
        function (result, sender) {
            if (result) {
                var fromValue = result[property.returnFieldNames[0]];
                var index = sender.charAt(sender.length - 1);
                $("#" + ContainerControlEnum.FromText + (index)).hide();
                $("#" + ContainerControlEnum.ToText + (index)).show();
                $("#" + ContainerControlEnum.SpanFrom + index).show();
                $("#" + ContainerControlEnum.SpanTo + index).hide();

                if ($("#" + InputControlIdEnum.ColumnType + (index)).val() == ValueTypeEnum.Time) {
                    fromValue = fromValue.substr(0, 2) + ":" + fromValue.substr(2, 2) + ":" + fromValue.substr(4, 2);
                }
                $("#" + ContainerControlEnum.SpanFrom + (index))[0].textContent = fromValue;
                $("#" + InputControlIdEnum.FromTextField + (index)).val(fromValue);
                $("#" + InputControlIdEnum.ToTextField + (index)).focus()
            }
        }, fromOptionalFinderInfo);

    sg.viewFinderHelper.setViewFinder(InputControlIdEnum.ToButton + index,
        function (result, sender) {
            if (result) {
                var toValue = result[property.returnFieldNames[0]];
                var index = sender.charAt(sender.length - 1);
                $("#" + ContainerControlEnum.SpanTo + index).show();
                $("#" + ContainerControlEnum.ToText + (index)).hide();
                $("#" + InputControlIdEnum.ToTextField + (index)).val(toValue);
                if ($("#" + InputControlIdEnum.ColumnType + (index)).val() == ValueTypeEnum.Time) {
                    toValue = toValue.substr(0, 2) + ":" + toValue.substr(2, 2) + ":" + toValue.substr(4, 2);
                }
                $("#" + ContainerControlEnum.SpanTo + (index))[0].textContent = toValue;
                $("#" + InputControlIdEnum.ToTextField + (index)).val(toValue);
                $("#" + InputControlIdEnum.ToTextField + (index)).focus()
            }
        }
        , toOptionalFinderInfo);
}

/**@name showErrorMessage
      @description displays error message window
      @param { string } moduleName module name
      @param { string } controller controller name
      @param { string } methodName method name
      @param { number } index row index
   */
function showErrorMessage(moduleName, controller, methodName, index) {
    var optionalFieldValue = $("#" + InputControlIdEnum.OptionalTextField + index).val();
    var urlMsg = sg.utls.url.buildUrl(moduleName, controller, methodName);
    sg.utls.ajaxGet(urlMsg, {}, (msg) => {
        sg.utls.showMessageInfo(sg.utls.msgType.ERROR, optionalFieldValue + msg);
    });
    e.stopImmediatePropagation()
}

/**@name gridControlsValidations
   @description comparing all the from and to values and validate and setting the values for hidden parameters
 **/
function gridControlsValidations() {

    /**@name optTextValue
       @description set values for from and to hidden parameter
       @param { string } fromValue From Value 
       @param { string } toValue To Value
       @param { string } optFldType Type field value
       @param { string } fromFilterValue From filter value 
       @param { string } toFilterValue To value 
     */
    function setValueToParameters(fromValue, toValue, optFldType, fromFilterValue = "", toFilterValue = "") {
        if (optFldType != ValueTypeEnum.Text) {
            $("#" + HdnFieldEnum.FromFilterValue + j).val(fromFilterValue);
            $("#" + HdnFieldEnum.ToFilterValue + j).val(toFilterValue);
        }
        else {
            $("#" + HdnFieldEnum.FromFilterValue + j).val(fromValue);
            $("#" + HdnFieldEnum.ToFilterValue + j).val(toValue);
        }
        $("#" + HdnFieldEnum.FromDisplayValue + j).val(fromValue);
        $("#" + HdnFieldEnum.ToDisplayValue + j).val(toValue);
        $("#" + HdnFieldEnum.OptFldType + j).val(optFldType);
    }

    /**@name formatDecimalValue
       @description formatting the value for integer, amount and number From and To textbox For Report
       @param { string } decimalValue decimal values
       @param { string } defaultValue int & decimal type default value
       @param { string } optFldType Type field value
     */
    function formatDecimalValue(decimalValue, defaultValue, optFldType) {
        var formattedFromOptionalField = sg.utls.kndoUI.getFormattedDecimalNumber(fromValue, decimalValue);
        var formattedToOptionalField = sg.utls.kndoUI.getFormattedDecimalNumber(toValue, decimalValue);

        var fromFieldValue = fromValue == "-" ? defaultValue : formattedFromOptionalField;
        var toFieldValue = toValue == "-" ? defaultValue : formattedToOptionalField;

        setValueToParameters(fromFieldValue, toFieldValue, optFldType, fromValue, toValue);
    }

    // Grid validation
    let grid = $('#gdOptFields').data("kendoGrid");
    var gridData = grid.dataSource.data();
    var girdTotalRows = gridData.length;
    var j = '0';
    const minDate = "1/1/1000"; //since the default min value is 1/1/0001, we are setting the default min value to 1/1/1000 

    //To compare all the rows from and to value app iterating all the rows using below loop
    for (var i = 0; i < girdTotalRows; i++) {
        j = i + 1;
        var type = ($("#" + InputControlIdEnum.ColumnType + i).val());
        var decimals = ($("#" + InputControlIdEnum.ColumnDecimal + i).val());
        var fromValue = $("#" + InputControlIdEnum.FromTextField + i).val();
        var toValue = $("#" + InputControlIdEnum.ToTextField + i).val();
        var invalidEntry = "";

        //set value for optionalfield
        var optionalField = $("#" + InputControlIdEnum.OptionalTextField + i).val();
        $("#" + HdnFieldEnum.OptField + j).val(optionalField);
        $("#" + HdnFieldEnum.OptDisplayFld + j).val(optionalField);

        //To identify the type date,text,number,amount,integer,time,yesorno
        switch (type) {
            case ValueTypeEnum.Date:
                var isFromValidDate = sg.utls.kndoUI.checkForValidDateNull($("#" + InputControlIdEnum.FromDateField + i).val(), false) != null;
                var fromDate = new Date($("#" + InputControlIdEnum.FromDateField + i).val());

                var isToValidDate = sg.utls.kndoUI.checkForValidDateNull($("#" + InputControlIdEnum.ToDate + i).val(), false) != null;
                var toDate = new Date($("#" + InputControlIdEnum.ToDate + i).val());

                var fromDateVal = $("#" + InputControlIdEnum.FromDateField + i).val();
                var toDateVal = $("#" + InputControlIdEnum.ToDate + i).val();

                //invalid date validation
                if (!isFromValidDate || fromDate < new Date(minDate)) {
                    invalidEntry = "InvalidFromDateFormat";
                }
                else if (!isToValidDate || toDate < new Date(minDate)) {
                    invalidEntry = "InvalidToDateFormat";
                }
                else if ((fromDate > toDate) || (fromDateVal != "") && (toDateVal == "")) {
                    invalidEntry = "InvalidEntryMessage";
                }
                else {
                    var fromOptionalField = $("#" + InputControlIdEnum.FromDateField + i).val();
                    var toOptionalField = $("#" + InputControlIdEnum.ToDate + i).val();
                    //From and To Date Display Value
                    var fromDateDisplayValue = fromOptionalField == "" ? "" : sg.utls.formatDate(fromOptionalField);
                    var toDateDisplayValue = toOptionalField == "" ? "" : sg.utls.formatDate(toOptionalField);
                    //From and To Date Filter Value
                    var fromDateFilterValue = fromOptionalField == "" ? "" : sg.utls.formatDate(fromOptionalField, "yyyyMMdd");
                    var toDateFilterValue = toOptionalField == "" ? "" : sg.utls.formatDate(toOptionalField, "yyyyMMdd");

                    setValueToParameters(fromDateDisplayValue, toDateDisplayValue, ValueTypeEnum.Date, fromDateFilterValue, toDateFilterValue);
                }
                break;
            case ValueTypeEnum.Text:
                if (fromValue > toValue || (fromValue != "") && (toValue == "")) {
                    invalidEntry = "InvalidEntryMessage";
                }
                else {
                    setValueToParameters(fromValue, toValue, ValueTypeEnum.Text);
                }
                break;
            case ValueTypeEnum.YesNo:
                var fromValue = $("#" + InputControlIdEnum.FromDropdown + i).val();
                var toValue = $("#" + InputControlIdEnum.ToDropdown + i).val();
                var fromFilterValue = fromValue == "Yes" ? "1" : "0";
                var toFilterValue = toValue == "Yes" ? "1" : "0";

                if (fromValue > toValue || (fromValue != "") && (toValue == "")) {
                    invalidEntry = "InvalidEntryMessage";
                }
                else {
                    setValueToParameters(fromValue, toValue, ValueTypeEnum.YesNo, fromFilterValue, toFilterValue);
                }
                break;
            case ValueTypeEnum.Amount:
            case ValueTypeEnum.Number:
                if (parseFloat(fromValue) > parseFloat(toValue) || (fromValue != "") && (toValue == "")) {
                    invalidEntry = "InvalidEntryMessage";
                }
                if (type == ValueTypeEnum.Number) {
                    formatDecimalValue(decimals, integerZero, ValueTypeEnum.Number); // decimal value for number and integer
                }
                else {
                    formatDecimalValue(decimals, decimalZero, ValueTypeEnum.Amount); // decimal value for amount
                }
                break;
            case ValueTypeEnum.Integer:
            case ValueTypeEnum.Time:
                var fromFiltervalue = sg.utls.checkIfValidTimeFormat($("#" + InputControlIdEnum.FromTextField + i).val()).replaceAll(':', '');
                var toFilterValue = sg.utls.checkIfValidTimeFormat($("#" + InputControlIdEnum.ToTextField + i).val()).replaceAll(':', '');
                if (fromFiltervalue > toFilterValue || (fromValue != "") && (toValue == "")) {
                    invalidEntry = "InvalidEntryMessage";
                }
                else {
                    if (type == ValueTypeEnum.Integer)
                        formatDecimalValue(decimals, integerZero, ValueTypeEnum.Integer); // decimal value for number and integer
                    else if (type == ValueTypeEnum.Time) {
                        setValueToParameters(sg.utls.checkIfValidTimeFormat(fromValue), sg.utls.checkIfValidTimeFormat(toValue), ValueTypeEnum.Time, fromFiltervalue, toFilterValue);
                    }
                }
                break;
        }
        if (invalidEntry != "")
            showErrorMessage("PR", "Common", invalidEntry, i);
    }
}

/**@name formattedTextBox
   @description Format the textboxes for integer, amount, time and number
   @param { number } index value of row index from optional grid
   @param { string } optionalFieldType Type
   */
function formattedTextBox(index, optionalFieldType) {
    var decimals = $("#" + InputControlIdEnum.ColumnDecimal + index).val();
    var userEnteredFromValue = $("#" + InputControlIdEnum.FromTextField + index).val();
    var userEnteredToValue = $("#" + InputControlIdEnum.ToTextField + index).val();
    //Formatted Textbox For From and To Date
    if (optionalFieldType == ValueTypeEnum.Date) {
        var userEnteredFromDateValue = $("#" + InputControlIdEnum.FromDateField + index).val();
        var userEnteredToDateValue = $("#" + InputControlIdEnum.ToDate + index).val();

        var isFromValidDate = sg.utls.kndoUI.checkForValidDateNull(userEnteredFromDateValue, false);
        if (!isFromValidDate) {
            showErrorMessage("PR", "Common", "InvalidFromDateFormat", index)
        }
        else if (userEnteredFromDateValue != "") {
            $("#" + InputControlIdEnum.FromDateField + index).val(isFromValidDate);
            $("#" + ContainerControlEnum.SpanFrom + index)[0].textContent = isFromValidDate;
        }

        var isToValidDate = sg.utls.kndoUI.checkForValidDateNull(userEnteredToDateValue, false);
        if (!isToValidDate) {
            showErrorMessage("PR", "Common", "InvalidToDateFormat", index)
        }
        else if (userEnteredToDateValue != "") {
            $("#" + InputControlIdEnum.ToDate + index).val(isToValidDate);
            $("#" + ContainerControlEnum.SpanTo + index)[0].textContent = isToValidDate;
        }
    }
    //Formatted Textbox For From and To Time 
    if (optionalFieldType == ValueTypeEnum.Time) {
        var formattedFromTime = sg.utls.checkIfValidTimeFormat($("#" + InputControlIdEnum.FromTextField + index).val());
        $("#" + InputControlIdEnum.FromTextField + index).val(formattedFromTime);
        $("#" + ContainerControlEnum.SpanFrom + index)[0].textContent = formattedFromTime;

        var formattedToTime = sg.utls.checkIfValidTimeFormat($("#" + InputControlIdEnum.ToTextField + index).val());
        $("#" + InputControlIdEnum.ToTextField + index).val(formattedToTime);
        $("#" + ContainerControlEnum.SpanTo + index)[0].textContent = formattedToTime;
    }
    else if (optionalFieldType == ValueTypeEnum.Integer || optionalFieldType == ValueTypeEnum.Amount || optionalFieldType == ValueTypeEnum.Number) {
        //fromatting the From Input value to Integer, Number and Amount 
        if (userEnteredFromValue == "") {
            setNumberZeroFormatted(InputControlIdEnum.FromTextField, ContainerControlEnum.SpanFrom, index);
        }
        else {
            setNumberFormatted(InputControlIdEnum.FromTextField, userEnteredFromValue, ContainerControlEnum.SpanFrom, index);
        }

        //fromatting the To Input value to integer ,Number ,amount 
        if (userEnteredToValue == "") {
            setNumberZeroFormatted(InputControlIdEnum.ToTextField, ContainerControlEnum.SpanTo, index);
        }
        else {
            setNumberFormatted(InputControlIdEnum.ToTextField, userEnteredToValue, ContainerControlEnum.SpanTo, index);
        }

        /**@name setNumberZeroFormatted
         @description set the value zero for number,amount and integer textbox
         @param { string } txtField control id
         @param { string } spanCtl span control id
         @param { number } index row index
         */
        function setNumberZeroFormatted(txtField, spanCtl, index) {
            var textField;
            if (optionalFieldType == ValueTypeEnum.Amount) {
                textField = $("#" + txtField + index).val(decimalZero);
            }
            else {
                textField = $("#" + txtField + index).val(integerZero);
            }
            $("#" + spanCtl + index)[0].textContent = textField.val();
        }


        /**@name setNumberFormatted
         @description set the formatted value for number,amount and integer textbox
         @param { string } txtField control id
         @param { string } userEnteredValue User entered from and to value 
         @param { string } spanCtl span control id
         @param { number } index row index
         */
        function setNumberFormatted(txtField, userEnteredValue, spanCtl, index) {
            var formattedValueWithDecimals;
            if (optionalFieldType == ValueTypeEnum.Amount || optionalFieldType == ValueTypeEnum.Number) {
                userEnteredValue = userEnteredValue == "-" ? decimalZero : userEnteredValue;
                formattedValueWithDecimals = sg.utls.kndoUI.getFormattedDecimalNumber(userEnteredValue, decimals);
            }
            else {
                userEnteredValue = userEnteredValue == "-" ? integerZero : userEnteredValue;
                formattedValueWithDecimals = sg.utls.kndoUI.getFormattedDecimalNumber(userEnteredValue);
            }

            if (formattedValueWithDecimals == decimalZero && optionalFieldType == ValueTypeEnum.Amount) {
                $("#" + txtField + index).val(decimalZero);
            }
            else if (formattedValueWithDecimals == integerZero) {
                $("#" + txtField + index).val(integerZero);
            }

            $("#" + spanCtl + index)[0].textContent = formattedValueWithDecimals;

        }
    }
}