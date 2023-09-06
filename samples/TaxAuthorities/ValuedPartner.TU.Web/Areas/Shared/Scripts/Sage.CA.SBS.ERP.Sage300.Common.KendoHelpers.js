/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

"use strict";
/**
@module sg
*/
var sg = sg || {};
/**
@class sg.utls
*/
sg.utls = sg.utls || {};
/**
@class sg.utls.kndoUI
*/
sg.utls.kndoUI = sg.utls.kndoUI || {};
/**
@class sg.utls.kndoUI.timePicker
*/
sg.utls.kndoUI.timePicker = sg.utls.kndoUI.timePicker || {};

$.extend(sg.utls.kndoUI, {
    /**
     * To stop tabbing move to parent page for kendo window
     * @method onActivate
     * @param {} e - event
     * @return 
     */
    onActivate: function (e) {
        const windowElement = e.sender.wrapper;
        const windowContent = e.sender.element;

        if (windowElement) {
            // D-42917 Have to force kendo window focus here, ever since we upgrade jQuery to 3.6.0.
            // Current kendo version v2021.1.224 only (officially) support jQuery up to 3.5.1
            windowElement[0].focus();
        }

        $(document).off("keydown.kendoWindow").on("keydown.kendoWindow", function (e) {
            var focusedElement = $(document.activeElement);
            if (e.keyCode == kendo.keys.TAB && focusedElement.closest(windowElement).length == 0) {
                windowContent.focus();
            }
        });
    },


    /**
     * @method onOpen
     * @description Open Kendo Window in center of the Viewport. Also set title bar color
     */
    onOpen: function () {
        sg.utls.setKendoWindowPosition(this);
        sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
    },

    //Kendo grid helpers
    /**
     * Used to hide the columns of a grid
     * @method hideGridColumns
     * @param {} grid - Instance of the grid
     * @param {} colsArrayToHide - Array of columns to be hidden
     * @return 
     */
    hideGridColumns: function (grid, colsArrayToHide) {
        if (grid) {
            $.each(colsArrayToHide, function (index, value) {
                grid.hideColumn(value);
                var colIndex = GridPreferencesHelper.getGridColumnIndex(grid, value);
                //This is set to make sure that the column that is not visible doesn't appear in 'Edit Columns'
                if (grid.columns[colIndex]) {
                    grid.columns[colIndex].attributes['sg_Customizable'] = false;
                }
            });
        }
    },
    /**
     * @method showGridColumns
     * @description Used to show the columns of a grid
     * @param {object} grid - Instance of the grid
     * @param {array} colsArrayToHide - Array of columns to be hidden
     */
    showGridColumns: function (grid, colsArrayToHide) {
        if (grid) {
            $.each(colsArrayToHide, function (index, value) {
                grid.showColumn(value);
                var colIndex = GridPreferencesHelper.getGridColumnIndex(grid, value);
                if (grid.columns[colIndex]) {
                    grid.columns[colIndex].attributes['sg_Customizable'] = true;
                }
            });
        }
    },
    /**
     * @method changeColumnName
     * @description Used to change the name of a column in a grid
     * @param {string} gridName - Name of the grid (not the instance)
     * @param {string} columnName - Name of column
     * @param {string} headerValue - Name to which it should be changed to
     */
    changeColumnName: function (gridName, columnName, headerValue) {
        $("#" + gridName + " th[data-field=" + columnName + "]").html(headerValue);
        var grid = $("#" + gridName).data('kendoGrid');
        var colIndex = GridPreferencesHelper.getGridColumnIndex(grid, columnName);
        grid.columns[colIndex].title = headerValue;
    },
    /**
     * Gets the index of the selected row(the row user is on)
     * @method getSelectedRowIndex
     * @param {} grid - Instance of the grid
     * @return Index of the selected row as int
     */
    getSelectedRowIndex: function (grid) {
        return sg.utls.kndoUI.getSelectedRow(grid).index();
    },
    /**
     * Gets the data of the selected row(the row user is on)
     * @method getSelectedRowData
     * @param {} grid - instance of the grid
     * @return Row data as JSON
     */
    getSelectedRowData: function (grid) {
        return grid.dataItem(sg.utls.kndoUI.getSelectedRow(grid));
    },
    /**
     * Gets the data of the row by the unique key (defined in CSHTML)
     * @method getRowByKey
     * @param {} dataItems - All the rows of the grid (usually grid.dataSource.data())
     * @param {} key - Key that uniquely defines the row
     * @param {} value - The value of the key column to be retrieved
     * @return Row data as JSON
     */
    getRowByKey: function (dataItems, key, value) {
        var row = null;
        $.each(dataItems, function (index, item) {
            if (item[key] === value) {
                row = item;
            }
        });
        return row;
    },
    /**
     * Gets the selected row
     * @method getSelectedRow
     * @param {} grid - Instance of the grid
     * @return Selected row (not the data)
     */
    getSelectedRow: function (grid) {
        return grid.select();
    },
    /**
     * Gets the column of the grid
     * @method getColumn
     * @param {} grid - Instance of the grid
     * @param {} columnName - Name of the column to be retrieved
     * @return Column that is to be retrieved
     */
    getColumn: function (grid, columnName) {
        return $.grep(grid.columns, function (item) {
            return item.field === columnName;
        })[0];
    },
    /**
     * Gets the table row element containing a specified uid.
     * @method getRowForDataItem
     * @param {} dataItem - An object with a 'uid' property that will be used to match the row.
     * @return A CallExpression containing the matched table row element.
     */
    getRowForDataItem: function (dataItem) {
        return $("tr[data-uid='" + dataItem.uid + "']");
    },
    /**
     * Gets the current cell user is on
     * @method getCurrentCell
     * @param {} $elem - The element user is on
     * @return CallExpression
     */
    getCurrentCell: function ($elem) {
        return sg.utls.kndoUI.getContainingGrid($elem).current();
    },
    /**
     * Gets the current row user is on
     * @method getContainingDataItem
     * @param {} $elem - The element user is on
     * @return CallExpression
     */
    getContainingDataItem: function ($elem) {
        return sg.utls.kndoUI.getDataItemForRow(sg.utls.kndoUI.getContainingRow($elem));
    },
    /**
     * Gets the current cell user is closest to
     * @method getContainingCell
     * @param {} $elem - The element user is on
     * @return CallExpression
     */
    getContainingCell: function ($elem) {
        return $elem.closest("td[role='gridcell']");
    },
    /**
     * Gets the closest row user is on
     * @method getContainingRow
     * @param {} $elem - The element user is on
     * @return CallExpression
     */
    getContainingRow: function ($elem) {
        return $elem.closest("tr[role='row']");
    },
    /**
     * Gets the grid by div
     * @method getContainingGrid
     * @param {} $elem - The div element
     * @return CallExpression
     */
    getContainingGrid: function ($elem) {
        return $elem.closest("div[data-role='grid']").data("kendoGrid");
    },
    /**
     * Get the grid using the row
     * @method getGridForDataItem
     * @param {} dataItem - The row
     * @return CallExpression
     */
    getGridForDataItem: function (dataItem) {
        return sg.utls.kndoUI.getContainingGrid(sg.utls.kndoUI.getRowForDataItem(dataItem));
    },
    /**
     * Get the data item based on the current row
     * @method getDataItemForRow
     * @param {} $row - Instance of the row
     * @param {} $grid - Instance of the grid
     * @return CallExpression
     */
    getDataItemForRow: function ($row, $grid) {
        if (!$grid) $grid = sg.utls.kndoUI.getContainingGrid($row);
        return $grid.dataItem($row);
    },

    /**
     * Sets the grid column non editable and also makes sure that the tab doesn't go to this column
     * @method nonEditable
     * @param {} grid - Instance of the grid
     * @param {} container - The container, usually available through the editor      
     */
    nonEditable: function (grid, container) {
        grid.closeCell();

        if (container[0].cellIndex == 0 && sg.utls.isShiftKeyPressed) {
            var prevRowIndex = sg.utls.kndoUI.getSelectedRowIndex(grid) - 1;
            if (prevRowIndex >= 0) {
                grid.select(grid.tbody.find(">tr:eq(" + prevRowIndex + ")"));
            }
        } else {
            grid.select(container.closest("tr"));
        }

        sg.utls.kndoUI.skipTab(grid, container[0].cellIndex);
    },
    /**
     * Skips the tab from the column
     * @method skipTab
     * @param {} grid - Instance of the grid
     * @param {} currentCellIndex - The column index the user is currently on
     * @return 
     */
    skipTab: function (grid, currentCellIndex) {
        var nextIndex = (currentCellIndex);
        if (sg.utls.isShiftKeyPressed) {
            nextIndex = (nextIndex - 1);
        } else {
            nextIndex = (nextIndex + 1);
        }
        var cell = grid.tbody.find(">tr:eq(" + sg.utls.kndoUI.getSelectedRowIndex(grid) + ") >td:eq(" + nextIndex + ")");

        // D-27332 - the next or previous cell could be hidden, need to find the next or previous visible cell
        while (cell.length > 0 && !$(cell).is(':visible')) {
            if (sg.utls.isShiftKeyPressed) {
                nextIndex--;
            } else {
                nextIndex++;
            }
            cell = grid.tbody.find(">tr:eq(" + sg.utls.kndoUI.getSelectedRowIndex(grid) + ") >td:eq(" + nextIndex + ")");
        }
        if (cell.length > 0) {
            grid.current(cell);
            grid.editCell(cell);
        }
    },

    /**
     * Initialize checkBox selection for multiple grid row deletions
     * @method multiSelectInit
     * @param {} gridId - Name of the grid
     * @param {} selectAllChk - Name of the selectAll checkBox
     * @param {} selectChk - Name of the delete row checkBox
     * @param {} btnDeleteId - Name of delete button
     * @param {} isDisabledDelete - ko.observable to check if delete is allowed
     */
    multiSelectInit: function (gridId, selectAllChk, selectChk, btnDeleteId, isDisabledDelete) {
        if ($("#" + gridId)) {
            sg.controls.disable("#" + btnDeleteId);
            $(document).on("change", "#" + selectAllChk, function () {

                var grid = $('#' + gridId).data("kendoGrid");
                var checkbox = $(this);
                var rows = grid.tbody.find("tr");
                rows.find("td:first input")
                    .prop("checked", checkbox.is(":checked")).applyCheckboxStyle();
                if ($("#" + selectAllChk).is(":checked") && (!isDisabledDelete || !isDisabledDelete())) {
                    rows.addClass("k-state-active");
                    sg.controls.enable("#" + btnDeleteId);
                } else {
                    rows.removeClass("k-state-active");
                    sg.controls.disable("#" + btnDeleteId);
                }
            });
            $(document).on("change", "#" + selectChk, function () {
                $(this).closest("tr").toggleClass("k-state-active");
                var grid = $('#' + gridId).data("kendoGrid");
                var allChecked = true;
                var hasChecked = false;
                grid.tbody.find(".selectChk").each(function () {
                    if (!($(this).is(':checked'))) {
                        $("#" + selectAllChk).prop("checked", false).applyCheckboxStyle();
                        allChecked = false;
                        return;
                    } else {
                        hasChecked = true;
                    }
                });
                if (allChecked) {
                    $("#" + selectAllChk).prop("checked", true).applyCheckboxStyle();
                }

                if (hasChecked && (!isDisabledDelete || !isDisabledDelete())) {
                    sg.controls.enable("#" + btnDeleteId);
                } else {
                    sg.controls.disable("#" + btnDeleteId);
                }
            });
        }
    },

    /**
     * Instantiate Numberic text box
     * @method numericTextbox
     * @param {} id
     * @param {} format
     * @param {} spinners
     * @param {} min
     * @param {} max
     * @param {} decimals
     * @return 
    */
    numericTextbox: function (id, format, spinners, min, max, decimals) {
        if (min != null && max != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                min: min,
                max: max,
            });
        } else if (min != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                min: min
            });
        } else if (max != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                max: max
            });
        } else if (decimals != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                decimals: decimals
            });
        } else {
            $("#" + id).kendoNumericTextBox({
                //format: format,
                culture: kendo.culture(),
                spinners: spinners
            });
        }
    },

    /**
     * Instantiate a Numeric text box if it doesn't yet exist
     * Note:
     *   fieldId is expected to contain a "#" character
     * @method createNumericTextBoxIfNotExists
     * @param fieldId The Id of the html control
     * @param options
     * @returns A reference to the newly created control
     */
    createNumericTextBoxIfNotExists: function (fieldId, options) {

        var control = $(fieldId).data("kendoNumericTextBox");
        //if (control) { sg.utls.kndoUI.destroyNumericTextBox(fieldId) }

        if (!control) {
            control = $(fieldId).kendoNumericTextBox({
                value: options.value,
                min: options.minValue,
                max: options.maxValue,
                format: options.format,
                spinners: options.spinners,
                step: options.step,
                decimals: options.decimalPlaces,
                restrictDecimals: options.restrictDecimals,
                change: options.onChangeHandler,
            }).data("kendoNumericTextBox");
        } else {
            control.value(options.value);
        }

        return control;
    },

    /**
     * Destroy an existing Numeric text box control
     * Note:
     *   fieldId is expected to contain a "#" character
     * @method destroyNumericTextBox
     * @param fieldId
     */
    destroyNumericTextBox: function (fieldId) {
        var textBox = $(fieldId).data("kendoNumericTextBox");
        if (textBox) {
            // Textbox exists...
            var origin = textBox.element.show();
            origin.insertAfter(textBox.wrapper);
            textBox.destroy();
            textBox.wrapper.remove();
        }
    },

    /**
     * Instantiate Decimal text box
     * @method decimalTextbox
     * @param {} id
     * @param {} format
     * @param {} spinners
     * @param {} min
     * @param {} max
     * @param {} decimals
     * @return 
     */
    decimalTextbox: function (id, format, spinners, min, max, decimals) {
        if (min != null && max != null && decimals != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                min: min,
                max: max,
                decimals: decimals
            });
        } else if (min != null && decimals != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                min: min,
                decimals: decimals
            });
        } else if (max != null && decimals != null) {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                max: max,
                decimals: decimals
            });
        } else {
            $("#" + id).kendoNumericTextBox({
                format: format,
                spinners: spinners,
                decimals: decimals
            });
        }
    },

    /**
     * Instantiates a Kendo DropDownList widget, and binds the "change" event of the underlying DOM 
     * element (which gets triggered by Knockout) to code that will “force” the widget to update and 
     * refresh the UI.
     * 
     * @method dropDownList
     * @param {string} id The CSS id of the element to configure as a DropDownList.
     * @returns {object} The kendoDropDownList state of the dropdown.
     */
    dropDownList: function (id) {
        // Create the control
        var dropDown = $("#" + id).kendoDropDownList();

        // https://www.telerik.com/blogs/kendo-ui-mvvm-and-knockoutjs
        // This will bind the "change" event of the underlying DOM element 
        // (which gets triggered by Knockout) to code that will “force” a Kendo UI widget 
        // to update and refresh the UI. With this code added, Kendo UI will "listen" 
        // for changes Knockout triggers in the HTML and the update the rich Kendo UI widget.
        $("#" + id).on("change", function () {
            $(this).data("kendoDropDownList").select(this.selectedIndex);
        });

        return dropDown;
    },

    /**
     * Instantiate Drop Down List (Alternate)
     * @method dropDownList2
     * @param {} id
     * @return The newly created drop down list control instance
     */
    dropDownList2: function (id) {
        // Create the control only. Do not bind change event like dropDownList above.
        var ddlControl = $("#" + id).kendoDropDownList().data("kendoDropDownList");
        return ddlControl;
    },

    /**
     * Instantiate Drop Down List For Grid
     * @method dropDownListForGrid
     * @param {} container
     * @param {} dataSource
     * @param {} value
     * @return 
     */
    dropDownListForGrid: function (container, dataSource, value) {
        return container.kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: ko.mapping.toJS(dataSource),
            value: value,
            //dataBound: function () {
            //    this.select(this.selectedIndex);
            //    this.trigger("change");
            //}
        });
    },

    /**
     * Remove Date Picker
     * @method removeDatePicker
     * @param {} id
     * @return 
     */
    removeDatePicker: function (id) {
        var datePicker = $("#" + id).data("kendoDatePicker");
        if (datePicker != null) {
            var popup = datePicker.dateView.popup;
            var element = popup.wrapper[0] ? popup.wrapper : popup.element;
            element.remove();
            var input = datePicker.element.show();
            input.removeClass("k-input");
            input.insertBefore(datePicker.wrapper);
            datePicker.wrapper.remove();
            input.removeData("kendoDatePicker");
        }
    },
    /**
     * Set Date picker options
     * @method datepickerOptions
     * @return ObjectExpression
     */
    datepickerOptions: function () {
        return {
            format: sg.utls.kndoUI.getDateFormat(),
            parseFormats: sg.utls.kndoUI.getDatePatterns(),
            footer: globalResource.Today + " - #: kendo.toString(data, '" + globalResource.DateFormat + "') #",
            min: new Date(1000, 1, 1),
            max: new Date(9999, 12, 31)
        };
    },
    /**
     * Instantiate Date Picker
     * @method datePicker
     * @param {} id
     * @return 
     */
    datePicker: function (id) {
        $("#" + id).kendoDatePicker(sg.utls.kndoUI.datepickerOptions());
        $("#" + id).attr("placeholder", sg.utls.kndoUI.getDisplayDateFormat());
        $("#" + id).attr("formatTextbox", "date");
        $("#" + id).attr("maxlength", "10");
    },
    /**
     * Gets the proper formatted date
     * @method getFormattedDate
     * @param {} val
     * @return value
     */
    getFormattedDate: function (val) {
        var value = sg.utls.kndoUI.checkForValidDate(val, true);
        if (value == null) {
            return "";
        }
        return value;
    },

    /*
    Get the string of Date Value and converts it DateTime object
    @dateValue string of Date value
    @return Date object
    */
    convertStringToDate: function (dateValue) {
        var initialDateValue = dateValue;
        var displayValue;
        if ((initialDateValue instanceof Date) == false && initialDateValue != null && initialDateValue != "") {
            //DataTime values initally not converted to date objects.
            //Remove the time component from string. This will always in format "2014-07-09T06:04:04.1939422-07:00" or "2020-05-08T00:00:00"
            initialDateValue = initialDateValue.substring(0, 10);
            //Safari do not understand the format "2014-07-09" or "2020-05-08", we need to convert it to "2014/07/09" or "2020/05/08"
            initialDateValue = initialDateValue.replace(/-/g, "/");
            displayValue = new Date(initialDateValue);
        }
        else {
            displayValue = initialDateValue;
        }
        return displayValue;
        //return dateValue;
    },

    /**
    * Gets the proper formatted number
    * @method getFormattedNumber
    * @param {} val
    * @return value
    */
    getFormattedNumber: function (val) {
        if (typeof val === 'string' || val instanceof String) {
            val = parseFloat(val);
        }
        return kendo.toString(val, "n0");
    },

    /**
     * @name getFormattedDecimalNumber
     * @desc Gets the proper formatted number for decimal with thousand separator
     * @param {any} val Value to parse
     * @param {any} decimal Number of decimal places
     * @return {string} value
     */
    getFormattedDecimalNumber: function (val, decimal) {
        if (!decimal) {
            decimal = "0";
        }
        if (!val) {
            val = "0";
        }
        val = parseFloat(val);
        return kendo.toString(val, "n" + decimal);
    },

    /**
     * Checks if the date is valid - DEPRECATED for external use. Use getFormattedDate
     * @method checkForValidDate
     * @param {any} val Value to parse
     * @param {boolean} checkforUtcDate True if UtcDate
     * @return {any} date object
     */
    checkForValidDate: function (val, checkforUtcDate) {
        if (val === null || val === "") {
            return null;
        }
        var date = kendo.parseDate(val, sg.utls.kndoUI.getDatePatterns());
        if (date === null) {
            if (checkforUtcDate) {
                return sg.utls.kndoUI.getDate(val);
            }
            return null;
        }
        // Let's skip dark ages
        const firstMillennium = 1000;
        if ((date.getFullYear() < firstMillennium) ||
            (date.getMonth() === 0 && date.getDate() === 1 && date.getFullYear() === 1000)) {
            return null;
        }
        return sg.utls.kndoUI.getDate(date);
    },

    /**
     * Checks if the date is null or valid - DEPRECATED for external use. Use getFormattedDate
     * @method checkForValidDateNull
     * @param {any} val Value to parse
     * @param {boolean} checkforUtcDate True if UtcDate
     * @return {any} date/true/null object
     */
    checkForValidDateNull: function (val, checkforUtcDate) {
        if (val === null || val === "") {
            return true;
        }
        var date = kendo.parseDate(val, sg.utls.kndoUI.getDatePatterns());
        if (date === null) {
            if (checkforUtcDate) {
                return sg.utls.kndoUI.getDate(val);
            }
            return null;
        }
        // Let's skip dark ages
        const firstMillennium = 1000;
        if ((date.getFullYear() < firstMillennium) ||
            (date.getMonth() === 0 && date.getDate() === 1 && date.getFullYear() === 1000)) {
            return null;
        }
        return sg.utls.kndoUI.getDate(date);
    },

    /**
     * Checks if the date is valid return date in formt YYYYMMDD else empty
     * @method checkForValidDate
     * @param {} val
     * @return Date in YYYMMDD
     */
    getDateYYYMMDDFormat: function (val) {
        val = kendo.parseDate(val, sg.utls.kndoUI.getDatePatterns());
        if (val != null) {
            var dt = new Date(val);
            if ((dt instanceof Date)) {
                var year = dt.getFullYear();
                var month = (dt.getMonth() + 1);
                var date = dt.getDate();

                if (month < 10) {
                    month = "0" + month;
                }

                if (date < 10) {
                    date = "0" + date;
                }
                return "" + year + month + date;
            }
        }
        return "";
    },
    /**
     * Gets Date
     * @method getDate
     * @param {} val
     * @return CallExpression
     */
    getDate: function (val) {
        if (val == null) {
            return null;
        }
        var dt = new Date(val);
        if (dt == "Invalid Date") {
            return null;
        }
        return kendo.toString(dt, sg.utls.kndoUI.getDateFormat());
    },
    /**
     * INTERNAL - Gets the date format, use only if necessary
     * @method getDateFormat
     * @return MemberExpression
     */
    getDateFormat: function () {
        return globalResource.DateFormat;
    },
    /**
     * INTERNAL - Used to get the date patters
     * @method getDatePatterns
     * @return ArrayExpression
     */
    getDatePatterns: function () {
        return [globalResource.DateFormat, globalResource.DateFormat + " h:mm:ss tt", globalResource.DatePattern, globalResource.DateFormat + " HH:mm:ss", 'yyyy-MM-dd', 'yyyy-MM-ddTHH:mm:ss'];
    },
    /**
     * Used for templates in grid with date field
     * @method getDateTemplate
     * @param {} field
     * @return BinaryExpression
     */
    getDateTemplate: function (field) {
        return "#= sg.utls.kndoUI.getFormattedDate(" + field + ") #";
    },

    /**
        * INTERNAL - Gets the date format from resource file based on localization
        * @method getDisplayDateFormat
        * @return MemberExpression
        */
    getDisplayDateFormat: function () {
        return globalResource.DisplayDateFormat;
    },


    /**
     * Used for templates in grid with number field
     * @method getNumberTemplate
     * @param {} field
     * @return BinaryExpression
     */
    getNumberTemplate: function (field) {
        return "#= sg.utls.kndoUI.getFormattedNumber(" + field + ") #";
    },

    /**
     * Used for templates in grid with number field
     * @method getNumberDecimalTemplate
     * @param {} field
     * @return BinaryExpression
     */
    getNumberDecimalTemplate: function (field) {
        return "#= sg.utls.kndoUI.getFormattedNumber(" + field + ") #";
    },

    /**
     * Used for templates in grid with drilldown pencil icon with null check
     * @param {} field - this value will display on the grid
     * @param {} text - this text will be append to the button class
     * @returns {} 
     */
    getPencilIconTemplate: function (field, text) {
        if (field == null || field == '') {
            return "";
        }
        return '<div class="pencil-wrapper"><span class="pencil-txt">' + kendo.htmlEncode(field) + '</span><span class="pencil-icon"><input type="button" class="icon edit-field btn' + text + '"/></span></div>';
    },

    /**
     * Used for templates in grid with special columns to hide zero value
     * @param {} field - this value will display on the grid
     * @returns {} 
     */
    getHideZeroTemplate: function (field) {
        //target both string type and integer type
        if (field == 0) {
            return "";
        }
        return field;
    },

    /**
     * Instantiate Kendo tab
     * @method initTab
     * @param {} id
     * @return 
     */
    initTab: function (id) {
        $("#" + id).kendoTabStrip({
            animation: {
                open: {
                    effects: "fadeIn"
                }
            }
        });
    },

    /**
     * Select a tab
     * @method selectTab
     * @param {} id
     * @param {} tab
     * @return 
     */
    selectTab: function (id, tab) {
        var ts = $("#" + id).data("kendoTabStrip");
        ts.select(ts.tabGroup.children("li[id='" + tab + "']"), true);
    },

    /**
     * Disable all tabs
     * @method disableAllTabs
     * @param {} id
     * @param {} isDisable
     * @return 
     */
    disableAllTabs: function (id, isDisable) {
        var tabstrip = $("#" + id).data('kendoTabStrip');
        if (isDisable)
            tabstrip.disable(tabstrip.items());
        else
            tabstrip.enable(tabstrip.items());
    },

    /**
     * Select tab Event call back
     * @method tabSelectEvent
     * @param {} id
     * @param {} onSelectCallBack
     * @return 
     */
    initTabSelectEvent: function (id, onSelectCallBack) {
        $("#" + id).data("kendoTabStrip").bind("select", onSelectCallBack);
    },


    /**
    * Hide the tabstrips
    * @method hidelAllTabs
    * @param {} id
    * @return 
    */
    hideAllTabStrip: function (id) {

        $("#" + id + " .k-tabstrip-items").hide();
    },


    /**
     * Prepends with 0 mentioned in length
     * @method getTextValue
     * @param {} len
     * @return inputValue
     */
    getTextValue: function (len) {
        var inputValue = 0;
        for (var i = 0; i < len - 1; i++) {
            inputValue = "0" + inputValue;
        }
        return inputValue;
    },
    /**
     * Gets the number with correct decimal places with thousand separator due to latest chagne
     * @method getFormattedDecimal
     * @param {} amount
     * @param {} decimalPlaces - If decimal places is passed as null, it will use the home currency decimals
     * @return 
     */
    getFormattedDecimal: function (amount, decimalPlaces) {
        if (amount === "" || amount === null)
            amount = 0;
        if (typeof amount === 'string' || amount instanceof String) {
            amount = parseFloat(amount);
        }
        if (decimalPlaces != null) {
            //Using kendo UI native funciton is better choice, due to it would be easy to handle Culture formating
            return kendo.toString(amount, "n" + decimalPlaces);
            //return parseFloat(amount).toFixed(decimalPlaces);
        } else {
            //Using kendo UI native funciton is better choice, due to it would be easy to handle Culture formating
            return kendo.toString(amount, "n" + sg.utls.homeCurrency.Decimals);
            //return parseFloat(amount).toFixed(sg.utls.homeCurrency.Decimals);
        }
    },

    restrictDecimals: function (numericTextBoxData, numberOfDecimals, numberOfNumerals) {
        if (typeof numericTextBoxData === "undefined") {
            return;
        }
        var numericTextBoxDataValue;
        if (numericTextBoxData.element) {
            numericTextBoxDataValue = numericTextBoxData.element;
        } else {
            numericTextBoxDataValue = numericTextBoxData;
        }
        $(numericTextBoxDataValue).off("input keydown");
        $(numericTextBoxDataValue).on("input", function (e) {
            const numeric = $(numericTextBoxDataValue).data('kendoNumericTextBox');
            var val = numericTextBoxDataValue.val();
            var decimalSeparator = kendo.culture().numberFormat['.'];
            var parts = val.split(decimalSeparator);

            if (val.indexOf(decimalSeparator) !== -1) {
                if (parts[1].length > numberOfDecimals) {
                    numericTextBoxDataValue.val(val.substr(0, val.length - 1));
                }
            }
            if (numberOfNumerals) {
                var numeralLength = parts[0].length;
                if (parts[0].indexOf("-") > -1) {
                    numeralLength--;
                }

                if (numeralLength > numberOfNumerals) {
                    if (parts.length > 1) {
                        numericTextBoxDataValue.val(val.substr(0, numeralLength - 1) + decimalSeparator + parts[1]);
                    } else {
                        numericTextBoxDataValue.val(val.substr(0, numeralLength - 1));
                    }
                }
            }
        });

        $(numericTextBoxDataValue).keydown(e => {
            const numeric = $(numericTextBoxDataValue).data('kendoNumericTextBox');
            const cultureDecimalCharacter = kendo.culture().numberFormat['.'];
            let minValue = numeric.options.min;
            let validChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', 'Tab', 'Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Shift', 'Home', 'End'];
            if (minValue < 0) {
                validChars.push('-');
            }
            // Crtl+a (Select All) is a valid combination
            if (e.key == 'a' && e.ctrlKey) {
                return;
            }
            // allow copy-paste.
            if ((e.key == 'c'||e.key == 'v') && e.ctrlKey) {
                return;
            }

            // Add decimal 'point' (for whichever culture) to
            // list of valid characters
            if (numberOfDecimals > 0) {
                validChars.push(cultureDecimalCharacter);
            };

            // Determine invalid scenarios
            const isInvalidCharacter = !validChars.includes(e.key);
            const isMinusSignNotInitialCharacter = (e.key === '-' && (e.currentTarget.selectionStart > 0 || e.target.value.includes('-')));
            const isDecimal = (e.key === cultureDecimalCharacter && e.target.value.includes(cultureDecimalCharacter));

            if (isInvalidCharacter || isMinusSignNotInitialCharacter || isDecimal ) {
                e.preventDefault();
            }
        });
    },

    /**
     * Description
     * @method selectGridRow
     * @return 
     */
    selectGridRow: function () {
        var target = $('.k-grid-content');
        target.attr("tabindex", 0);

        target.keydown(function (ke) {
            var kGrid, curRow, newRow;

            kGrid = $(this).closest('.k-grid').data("kendoGrid");
            if (!$(kGrid).attr('editable')) {

                curRow = kGrid.select();
                if (!curRow.length)
                    return;
                if (ke.which == 38) {
                    newRow = curRow.prev();
                } else if (ke.which == 40) {
                    newRow = curRow.next();
                } else {
                    return;
                }
                if (!newRow.length)
                    return;
                kGrid.select(newRow);
            }
        });
    },
    /**
     * Used for masking phone number
     * @method maskPhoneNo
     * @param {} selector
     * @param {} config
     * @return 
     */
    maskPhoneNo: function (selector, config) {
        var phoneNoConfig = config || {
            mask: "(000) 000-000000000000000000000000",
            rules: {
                "0": /./
            }
        };
        if (selector.length > 0) {
            $(selector).kendoMaskedTextBox(phoneNoConfig);
        }
    },
    /**
     * Clears the masking for phone number
     * @method destroyPhoneNoMask
     * @param {} selector
     * @return 
     */
    destroyPhoneNoMask: function (selector) {
        if (selector.length > 0) {
            $(selector).data("kendoMaskedTextBox").destroy();
        }
    },
    /**
      * Set the row for the Grid cell
      * @method setDataForGridCell
      * @param {} cellId - grid cell 
      * @param {} gridId -grid
      * @param {} value - value to be set
      */
    setDataForGridCell: function (cellId, gridId, value) {
        var grid = $("#" + gridId).data("kendoGrid");
        var dataItem = sg.utls.kndoUI.getSelectedRowData(grid);
        dataItem.set(cellId, sg.utls.checkIfValidTimeFormat(value));
    },

    /**
      * Add line to Grid on the client side
      * @method addLine
      * @param {} data - pass knockout datasource bound to the grid
      * @param {} gridId -ID of the grid as string 
      * @param {} newLineFunctionCall - function that return the JSON model
       * @param {} paramFunctionCall - function that sets the param for the Grid (This function will have to take data, page number, page size and inserted index as inputs)
      */
    addLine: function (data, gridId, newLineFunctionCall, paramFunctionCall, currentPageNumber) {
        var grid = $("#" + gridId).data("kendoGrid");
        var pageNumber = grid.dataSource.page();
        var pageSize = grid.dataSource.pageSize();
        var dataRows = grid.items();
        var insertedIndex = dataRows.index(grid.select());

        if (insertedIndex < 0) {
            insertedIndex = 0;
        }

        if (data.Items().length == pageSize) {

            var newinsertIndex = (pageNumber - 1) * pageSize + insertedIndex;
            if (newinsertIndex < 0) {
                newinsertIndex = 1;
            } else {
                newinsertIndex = newinsertIndex + 1;
            }

            if (newinsertIndex == (pageSize * currentPageNumber)) {
                paramFunctionCall(data, pageNumber, pageSize, newinsertIndex);
                grid.dataSource.query({
                    page: pageNumber + 1,
                    pageSize: sg.utls.gridPageSize
                });
            } else {
                paramFunctionCall(data, pageNumber - 1, pageSize, newinsertIndex);
                grid.dataSource.read();
            }
            return false;
        } else {
            var newLine = newLineFunctionCall.call();
            grid.dataSource.insert(insertedIndex + 1, newLine);
            return true;
        }

    },
    /**
      * Add last line to Grid on the client side
      * @method addLine
      * @param {} data - pass knockout datasource bound to the grid
      * @param {} gridId -ID of the grid as string 
      * @param {} newLineFunctionCall - function that return the JSON model
       * @param {} paramFunctionCall - function that sets the param for the Grid (This function will have to take data, page number, page size and inserted index as inputs)
       
      */
    addLastLine: function (data, gridId, newLineFunctionCall, paramFunctionCall, currentPageNumber, index) {
        var grid = $("#" + gridId).data("kendoGrid");
        var pageNumber = grid.dataSource.page();
        var pageSize = grid.dataSource.pageSize();
        var dataRows = grid.items();
        var insertedIndex = dataRows.index(grid.select());

        if (index && index > 0) {
            insertedIndex = index;
        }

        if (insertedIndex < 0) {
            insertedIndex = 0;
        }

        if (data.Items().length == pageSize) {

            var newinsertIndex = (pageNumber - 1) * pageSize + insertedIndex;
            if (newinsertIndex < 0) {
                newinsertIndex = 1;
            } else {
                newinsertIndex = newinsertIndex + 1;
            }

            if (newinsertIndex == (pageSize * currentPageNumber)) {
                paramFunctionCall(data, pageNumber, pageSize, newinsertIndex);
                grid.dataSource.query({
                    page: pageNumber + 1,
                    pageSize: sg.utls.gridPageSize
                });
            } else {
                paramFunctionCall(data, pageNumber - 1, pageSize, newinsertIndex);
                grid.dataSource.read();
            }
            return false;
        } else {
            var newLine = newLineFunctionCall.call();
            grid.dataSource.insert(insertedIndex + 1, newLine);
            return true;
        }

    },
    /**
     * Assigns display index to the model
     * @method addLine
     * @param {} data - pass the Items object of the knockout datasource bound to the grid
     * @param {} currentPageNumber -Current page number - from the locally stored variable
     * @param {} pageSize - The page size      
     */
    assignDisplayIndex: function (data, currentPageNumber, pageSize) {
        var displayIndex = (currentPageNumber - 1) * pageSize + 1;

        if (data != null && data.length > 0) {
            $.each(data, function (index) {
                data[index].DisplayIndex = displayIndex++;
            });

            return data;

        }
    },

    /**
      * Gets the page number of the grid
      * @method getPageNumber      
      * @param {} gridId -ID of the grid as string             
      */
    getPageNumber: function (gridId) {
        var grid = $("#" + gridId).data("kendoGrid");
        var pageNumber = grid.dataSource.page();
        return pageNumber;
    },

    /**
     * Show the tool tip on mouse hover
     * @method showMouseOverToolTip      
     * @param {} id
     * @param {} text - tool tip message
     * @return
     */
    showMouseOverToolTip: function (id, text) {
        $("#" + id).kendoTooltip({
            autoHide: false,
            width: 200,
            position: "right",
            content: function () {
                return text; // set the element text as content of the tooltip
            }
        });
    },
    getTimeFormate: function (date) {
        if (date != null && date != undefined && date != "") {
            return date.substring(11, 19);
        }
    },

    /**
    * Used to hide the grid footer page navigation.
    * @method hidePageNavigation
    * @param {} gridId - grid Name
    * @return 
    */
    hidePageNavigation: function (gridId) {

        $(gridId).find('.k-pager-first').remove();
        $(gridId).find('.k-pager-last').remove();
        $(gridId).find('.k-pager-input').remove();
        $(gridId).find('.k-pager-info').remove();
    },

    getOrderBy: function (grid) {
        var data = grid.dataSource;

        if (data.sort !== undefined && data.sort !== null && typeof data.sort === "function" &&
            data.sort() !== undefined && data.sort().length > 0) {
            return {
                PropertyName: data.sort()[0].field,
                SortDirection: data.sort()[0].dir === "asc" ? 0 : 1
            };
        }
        else
            return null;
    },

    /**
    * Used to select the row that has been navigated to by pressing Up or Down key
    * @method gridKeyUpDownNavigation
    * @param {} grid - grid instance
    * @return 
    */
    gridKeyUpDownNavigation: function (grid) {
        var arrows = [38, 40];    //38: up arrow, 40: down arrow
        grid.table.on("keydown", function (e) {
            if (arrows.indexOf(e.keyCode) >= 0) {
                grid.select(grid.current().closest("tr"));
            }
        });
    },
});

$.extend(sg.utls.kndoUI.timePicker, {

    /**
     * @name getControlById
     * @param {string} controlId The string identifier for the control
     * @returns {object} A reference to the Kendo timepicker control
     */
    getControlById: function (controlId) {
        return $(controlId).data("kendoTimePicker");
    },

    /**
     * @name setModelTime
     * @description Take the contents of a TimePicker control
     *              and put into the correct model property in the correct format
     * @param {any} timeModelItem Reference to Time model data item
     * @param {any} timeValue Value for Time
     */
    setModelTime: function (timeModelItem, timeValue) {
        // 1. Get the time from the timepicker control
        // 2. Set the model Time property based on the previously gotten timepicker value
        // Developer Note: This is necessary because on the client-side, the binding is to the 
        //                 TimeTimeSpan, not Time. These property names may be different in your implementation.
        //                 We need to manually move the data over so the server-side can pick it up correctly.
        timeModelItem('1899-12-30T' + timeValue);
    },

    /**
     * @name getReasonableTime
     * @desc Calculate a reasonable time for the TimeBegin and TimeEnd fields
     *       if they're entered incompletely.
     * @param {string} controlId The controlId specifier
     * @returns {string} A reasonable TimeBegin or TimeEnd value
     */
    getReasonableTime: function (controlId) {

        var DEFAULT_STARTTIME = '09:00:00';
        var DEFAULT_ENDTIME = '17:00:00';

        var control = $(controlId).data("kendoMaskedTextBox");

        var fixedTime = '';
        var val = control.value();
        if (val !== '') {
            var parts = val.split(":");

            var validHour = true;
            var validMinute = true;
            var validSecond = true;
            var validHour0 = true;
            var validHour1 = true;
            var validMinute0 = true;
            var validMinute1 = true;
            var validSecond0 = true;
            var validSecond1 = true;

            var hour = parts[0];
            var minute = parts[1];
            var second = parts[2];

            var hour0 = hour[0];
            var hour1 = hour[1];

            var minute0 = minute[0];
            var minute1 = minute[1];

            var second0 = second[0];
            var second1 = second[1];

            if (hour[0].includes("_")) { validHour0 = false; }
            if (hour[1].includes("_")) { validHour1 = false; }
            if (minute[0].includes("_")) { validMinute0 = false; }
            if (minute[1].includes("_")) { validMinute1 = false; }
            if (second[0].includes("_")) { validSecond0 = false; }
            if (second[1].includes("_")) { validSecond1 = false; }

            var pattern = new RegExp("[0-9]");
            var result_h0 = pattern.test(hour[0]);
            var result_h1 = pattern.test(hour[1]);

            var result_m0 = pattern.test(minute[0]);
            var result_m1 = pattern.test(minute[1]);

            var result_s0 = pattern.test(second[0]);
            var result_s1 = pattern.test(second[1]);

            validHour = result_h0 && result_h1;
            validMinute = result_m0 && result_m1;
            validSecond = result_s0 && result_s1;

            if (!validHour) {
                if (!validHour0 && !validHour1) {
                    hour0 = '0';
                    hour1 = '9';
                } else if (!validHour0 && validHour1) {
                    hour0 = '0';
                } else if (validHour0 && !validHour1) {

                    // Move the hour0 value into hour1
                    // Set hour0 to zero
                    //hour1 = hour0;
                    //hour0 = '0';

                    // Reverted behaviour back to original
                    hour1 = '0';
                }
            }

            if (!validMinute) {
                if (!validMinute0 && !validMinute1) {
                    minute0 = '0';
                    minute1 = '0';
                } else if (!validMinute0 && validMinute1) {
                    minute0 = '0';
                } else if (validMinute0 && !validMinute1) {
                    // Move the minute0 value into minute1
                    // Set minute0 to zero
                    //minute1 = minute0;
                    //minute0 = '0';

                    // Reverted behaviour back to original
                    minute1 = '0';
                }
            }

            if (!validSecond) {
                if (!validSecond0 && !validSecond1) {
                    second0 = '0';
                    second1 = '0';
                } else if (!validSecond0 && validSecond1) {
                    second0 = '0';
                } else if (validSecond0 && !validSecond1) {
                    // Move the second0 value into second1
                    // Set second0 to zero
                    //second1 = second0;
                    //second0 = '0';

                    // Reverted behaviour back to original
                    second1 = '0';
                }
            }

            fixedTime = hour0 + hour1 + ':' + minute0 + minute1 + ':' + second0 + second1;
        } else {
            if (controlId === '#txtTimeBegin') {
                fixedTime = DEFAULT_STARTTIME;
            } else if (controlId === '#txtTimeEnd') {
                fixedTime = DEFAULT_ENDTIME;
            }
        }

        return fixedTime;
    },

    /**
     * @name init
     * @description Create and initialize the kendo timepicker control
     * @param {string} controlId The string controlId
     * @param {object} modelDataItemReference The reference to the model item associated with the control
     * @param {object} dirtyManager The reference to the DirtyManager object (Called in 'change' event)
     * @param {object} optionsIn The reference to the object containing optional parameters
     */
    init: function (controlId, modelDataItemReference, dirtyManager, optionsIn) {

        var twentyPlus = false;
        var timepicker = $(controlId);

        let options = {
            FormatMask: "HH:mm:ss",
            IntervalInMinutes: 15
        };
        if (optionsIn !== null && optionsIn !== 'undefined') {
            options = optionsIn;
        }

        timepicker.kendoMaskedTextBox({
            promptChar: "_",
            mask: "ab:cd:ef",
            rules: {
                "a": function (char) {
                    var digit = parseInt(char);

                    // Reject non-numeric characters
                    if (isNaN(digit)) {
                        return false;
                    }

                    // First digit can only be 0, 1 or 2
                    if (digit >= 0 && digit <= 2) {

                        // if first digit is a 2, then 
                        // set flag so we know about it 
                        // when processing the next digit
                        if (digit === 2) {
                            twentyPlus = true;
                        } else {
                            twentyPlus = false;
                        }
                        return true;
                    } else {
                        return false;
                    }
                },

                "b": function (char) {
                    var digit = parseInt(char);

                    // if first digit is a two 
                    // and second digit is greater than 3, reject it.
                    if (twentyPlus === true) {
                        if (digit > 3) {
                            return false;
                        }
                    }

                    return true;
                },

                "c": /[0-5]/,
                "d": /[0-9]/,
                "e": /[0-5]/,
                "f": /[0-9]/
            }
        });

        timepicker.kendoTimePicker({
            format: options.FormatMask,
            interval: options.IntervalInMinutes
        });

        timepicker.closest(".k-timepicker")
            .add(timepicker)
            .removeClass("k-textbox");

        // Set 'blur' event handler
        $(controlId).on('blur', function (e) {
            var fixedTime = sg.utls.kndoUI.timePicker.getReasonableTime(controlId);
            modelDataItemReference(fixedTime);
        });

        if (dirtyManager !== null && dirtyManager !== 'undefined') {
            // Set 'change' event handler. Set the dirty flag
            $(controlId).on('change', function () {
                dirtyManager.isDirty(true);
            });
        }
    }
});

var SageNumericTextBoxPlugin = (function (init) {
    return kendo.ui.NumericTextBox.extend({
        options: {
            name: 'NumericTextBox'
        },
        init: function (_element, _options) {
            var tagName = "data-sage300uicontrol";
            var style = $(_element).attr("style");
            var kendoNumericTextBox = $("#" + _element.id).data("kendoNumericTextBox");
            if (!kendoNumericTextBox) {
                init.call(this, _element, _options);
                var wrapper = $(this.wrapper[0]);
                var element = wrapper.find('[' + tagName + ']');
                var sageStyle = wrapper.attr("sage-style");
                if (!sageStyle) {
                    wrapper.attr("style", style);
                } else {
                    wrapper.attr("style", sageStyle);
                }

                if (element.length > 0) {
                    var value = element.attr(tagName);
                    element.removeAttr(tagName);
                    wrapper.attr(tagName, value);
                    wrapper.attr("sage-style", !style ? "display" : style);
                }
            } else {
                kendoNumericTextBox.setOptions(_options);
            }
        }
    });
})(kendo.ui.NumericTextBox.fn.init);
