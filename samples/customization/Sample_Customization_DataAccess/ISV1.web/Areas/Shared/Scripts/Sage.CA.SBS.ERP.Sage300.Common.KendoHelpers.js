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


$.extend(sg.utls.kndoUI, {
    /**
     * To stop tabbing move to parent page for kendo window
     * @method onActivate
     * @param {} e - event
     * @return 
     */
    onActivate: function (e) {
        var windowElement = this.wrapper,
            windowContent = this.element;

        $(document).off("keydown.kendoWindow").on("keydown.kendoWindow", function (e) {
            var focusedElement = $(document.activeElement);
            if (e.keyCode == kendo.keys.TAB && focusedElement.closest(windowElement).length == 0) {
                windowContent.focus();
            }
        });
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
* Used to show the columns of a grid
* @method showGridColumns
* @param {} grid - Instance of the grid
* @param {} colsArrayToHide - Array of columns to be hidden
* @return 
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
     * Used to change the name of a column in a grid
     * @method changeColumnName
     * @param {} gridName - Name of the grid (not the instance)
     * @param {} columnName - Name of column
     * @param {} headerValue - Name to which it should be changed to
     * @return 
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
     * Get the row for the data item
     * @method getRowForDataItem
     * @param {} dataItem - Data Item
     * @return CallExpression
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

        if (container.context.cellIndex == 0 && sg.utls.isShiftKeyPressed) {
            var prevRowIndex = sg.utls.kndoUI.getSelectedRowIndex(grid) - 1;
            if (prevRowIndex >= 0) {
                grid.select(grid.tbody.find(">tr:eq(" + prevRowIndex + ")"));
            }
        } else {
            grid.select(container.closest("tr"));
        }

        sg.utls.kndoUI.skipTab(grid, container.context.cellIndex);
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
     * Instantiate Drop Down List
     * @method dropDownList
     * @param {} id
     * @return 
     */
    dropDownList: function (id) {
        return $("#" + id).kendoDropDownList({
            //dataBound: function () {
            //    this.select(this.selectedIndex);
            //    this.trigger("change");
            //}
        });
    },

    /**
     * Instantiate Drop Down List For Grid
     * @method dropDownList
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
            footer: globalResource.Today + " - #: kendo.toString(data, '" + globalResource.DateFormat+ "') #",
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
        return kendo.toString(val, "n0");
    },

    /**
    * Gets the proper formatted number for decimal
    * @method getFormattedDecimalNumber
    * @param {} val,decimal
    * @return value
    */
    getFormattedDecimalNumber: function (val, decimal) {
        val = parseFloat(val);
        return kendo.toString(val, "n" + decimal);
    },

    /**
     * Checks if the date is valid - DEPRECATED for external use. Use getFormattedDate
     * @method checkForValidDate
     * @param {} val
     * @param {} checkforUtcDate
     * @return CallExpression
     */
    checkForValidDate: function (val, checkforUtcDate) {
        if (val == null || val === "") {
            return null;
        }
        var date = kendo.parseDate(val, sg.utls.kndoUI.getDatePatterns());
        if (date == null) {
            if (checkforUtcDate) {
                return sg.utls.kndoUI.getDate(val);
            }
            return null;
        }
        if (date.getMonth() == 0 && date.getDate() == 1 && date.getFullYear() == 1) {
            return null;
        }
        return sg.utls.kndoUI.getDate(date);
    },

    /**
   * Checks if the date is null or valid - DEPRECATED for external use. Use getFormattedDate
   * @method checkForValidDateNull
   * @param {} val
   * @param {} checkforUtcDate
   * @return CallExpression
   */
    checkForValidDateNull: function (val, checkforUtcDate) {
        if (val == null || val === "") {
            return true;
        }
        var date = kendo.parseDate(val, sg.utls.kndoUI.getDatePatterns());
        if (date == null) {
            if (checkforUtcDate) {
                return sg.utls.kndoUI.getDate(val);
            }
            return null;
        }
        if (date.getMonth() == 0 && date.getDate() == 1 && date.getFullYear() == 1) {
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
     * Gets the number with correct decimal places
     * @method getFormattedDecimal
     * @param {} amount
     * @param {} decimalPlaces - If decimal places is passed as null, it will use the home currency decimals
     * @return 
     */
    getFormattedDecimal: function (amount, decimalPlaces) {
        if (amount === "" || amount === null)
            amount = 0;
        if (decimalPlaces != null) {
            return parseFloat(amount).toFixed(decimalPlaces);
        } else {
            return parseFloat(amount).toFixed(sg.utls.homeCurrency.Decimals);
        }
    },

    restrictDecimals: function (numericTextBoxData, numberOfDecimals, numberOfNumerals) {
        var numericTextBoxDataValue;
        if (numericTextBoxData.element) {
            numericTextBoxDataValue = numericTextBoxData.element;
        } else {
            numericTextBoxDataValue = numericTextBoxData;
        }
        $(numericTextBoxDataValue).off("input");
        $(numericTextBoxDataValue).on("input", function (e) {
            var val = numericTextBoxDataValue.val();
            var parts = val.split(".");
            if (val.indexOf(".") !== -1) {
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
                        numericTextBoxDataValue.val(val.substr(0, numeralLength - 1) + "." + parts[1]);
                    } else {
                        numericTextBoxDataValue.val(val.substr(0, numeralLength - 1));
                    }

                }
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
            width: 120,
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
                wrapper.attr("sage-style", (!style) ? "display": style);
            }
        }
    });
})(kendo.ui.NumericTextBox.fn.init);
