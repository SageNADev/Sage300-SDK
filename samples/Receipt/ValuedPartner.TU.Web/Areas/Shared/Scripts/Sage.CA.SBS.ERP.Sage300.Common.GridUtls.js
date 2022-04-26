/* Copyright (c) 2019-2021 Sage Software, Inc.  All rights reserved. */

"use strict";
var sg = sg || {};
sg.utls = sg.utls || {};
sg.utls.grid = sg.utls.grid || {};
var gridCellFocusSwitch = false;

//TODO comments this one.

var cellValTemplate = {
    A: {
        row: null,
        col: null,
        field: null,
        val: null,
        gridId: null
    },
    B: {
        row: null,
        col: null,
        field: null,
        val: null,
        gridId: null
    }
};

sg.utls.grid = {
    valStack: {
    },
    isRetrieved: false,
    finderWasClicked: false,
    updateFailed: false,
    setDetailFailed: false,
    addingLine: false,
    insertIndex: 0,
    deepCopy: function (obj) {
        return JSON.parse(JSON.stringify(obj));
    },

    /**
     * This function is used to construct the grid columns based on information form sever-side
     * @param {any} gridId grid Id
     * @param {any} cols grid columns list from backend
     * @param {any} decimalPlace decimal place(s) for numeric 
     * @param {any} finderInfo the finder information for columns
     * @return {Array<Columns>} Kendo Grid Columns
     */
    formattingColumns: function (gridId, cols, decimalPlace, finderInfo) {
        cols.forEach(function (col) {
            var ret = sg.utls.grid.execFuncByName(col.title, window);
            if (ret) {
                col.title = ret;
            }
            col.attributes = {};
            if (col.PresentationList && col.PresentationList.length > 0) {
                col.PresentationMap = col.PresentationList.reduce(function (map, obj) {
                    map[obj.Value] = obj.Text;
                    return map;
                },
                    {});
                col.template = '#= sg.utls.grid.getDropdownDisplay(' + [col.field, col.columnId] + ') #';
                if (col.hidden) {
                    col.attributes.sg_Customizable = false;
                }
                if (!col.IsReadOnly) {
                    col.editor = sg.utls.grid.getDropdownEditor.bind({
                        "gridId": gridId
                    });
                } else {
                    col.editor = $.noop;
                }
            }
            if (col.dataType === "Decimal") {

                col.attributes = { "class": "align-right" };
                if (col.hidden) {
                    col.attributes.sg_Customizable = false;
                }
                col.template = '#= sg.utls.kndoUI.getFormattedDecimal(' + [col.field, decimalPlace] + ') #';
                if (!col.IsReadOnly) {
                    col.editor = sg.utls.grid.getNumericEditor.bind({
                        "gridId": gridId,
                        "colInfo": col,
                        "decimalPlace": decimalPlace
                    });
                }
            }
            if (!col.IsReadOnly && col.FinderReturnField && !col.editor) {
                col.attributes = { "class": "txt-upper" };
                if (col.hidden) {
                    col.attributes.sg_Customizable = false;
                }
                col.editor = sg.utls.grid.getFinderEditor.bind({
                    "gridId": gridId,
                    "colInfo": col,
                    "finderInfo": finderInfo[col.field]
                });
            }
            if (!col.IsReadOnly && !col.editor) {
                if (col.hidden) {
                    col.attributes.sg_Customizable = false;
                }
                col.editor = sg.utls.grid.getDefaultEditor.bind({ "gridId": gridId });
            }
        });
        return cols;
    },

    /**
     * This function is used to set the column information for Kendo Grid
     * @param {any} gridId grid Id
     * @param {any} columns Columns from model
     * @param {any} decimalPlace decimal Place(s) for numeric cell
     * @param {any} customerCols custom columns (not from model)
     * @param {any} finderInfo finder information
     */
    setColumns: function (gridId, columns, decimalPlace, customerCols, finderInfo) {
        var grid = $("#" + gridId).data("kendoGrid");
        var options = grid.getOptions();
        if (!customerCols)
            customerCols = [];
        if (decimalPlace) {
            options.columns = customerCols.concat(this.formattingColumns(gridId, columns, decimalPlace, finderInfo));
        } else {
            options.columns = customerCols.concat(this.formattingColumns(gridId, columns, null, finderInfo));
        }
        grid.setOptions(options);
    },

    /**
     * This method is used to disply the text based on dropdown value
     * @param {any} val value
     * @param {any} fieldIndex field index
     * @return{string} of display
     */
    getDropdownDisplay: function (val, fieldIndex) {
        var grid = $('#GridPJCDetail').getKendoGrid();
        var colInfo = grid.columns.find(function (v, i) {
            var col = grid.columns[i];
            if (col.columnId === fieldIndex) {
                return col;
            }
            return null;
        });
        return colInfo ? colInfo.PresentationMap[val] : "";
    },

    /**
     * This function is used to set the value from finder
     * @param {any} ret the object from finder select
     * @param {string} id the id of source
     */
    onFinderSuccess: function (ret, id) {
        var returnField;
        var val;
        if (this.FinderReturnField) {
            returnField = this.FinderReturnField;
        } else {
            returnField = this;
        }
        if (Array.isArray(returnField)) {
            if (returnField.length === 1) {
                val = ret[returnField[0]];
            } else {
                for (var i in returnField) {
                    val = ret[returnField[i]];
                    if (val) {
                        break;
                    }
                }
            }
        } else {
            var arr = returnField.split(",");
            if (arr.length === 1) {
                val = ret[returnField];
            } else {
                for (var i in arr) {
                    val = ret[arr[i]];
                    if (val) {
                        break;
                    }
                }
            }
        }
        var prev = gridCellFocusSwitch ? "A" : "B";
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
            var row = sg.utls.kndoUI.getSelectedRowData(grid);
            if (row) {
                row.set(field, val);
                setTimeout(function () {
                    sg.utls.grid.isRetrieved = false;
                });
            }
        }
        gridCellFocusSwitch = !gridCellFocusSwitch;
        sg.utls.grid.savePreviousText(cellValTemplate[prev].gridId,
            colIndex,
            field);
        setTimeout(function () {
            sg.utls.grid.finderWasClicked = false;
        });
    },

    onFinderCancel: function (ret) {
        var prev = sg.utls.grid.getPrevState();
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
        }
        setTimeout(function () {
            sg.utls.grid.finderWasClicked = false;
        });
    },

    /**
     * This function is used to reset the focus back 
     */
    resetFocus: function () {
        var prev = sg.utls.grid.getPrevState();
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
        }
        setTimeout(function () {
            sg.utls.grid.finderWasClicked = false;
        });
    },

    resetToPreviousFocus: function () {
        var prev = sg.utls.grid.getCurrentState();
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
        }
    },

    /**
     * This function is used to get previous state which was saved
     * @return {string} the previous state
     */
    getPrevState: function () {
        return gridCellFocusSwitch ? "A" : "B";
    },

    /**
     * This function is used to get previous state which was saved
     * @return {string} the previous state
     */
    getCurrentState: function () {
        return gridCellFocusSwitch ? "B" : "A";
    },


    /**
     * This method is used to get the editor for numeric input box
     * @param {any} container The jQuery object representing the container element.
     * @param {any} options Object
     * options.field {string} The name of the field to which the column is bound.
     * options.format {string} The format string of the column specified via the format option.
     * options.model {kendo.data.Model}The model instance to which the current table row is bound.
     * options.values {Array} Array of values specified via the values option.
     * this.gridId {string} grid Id
     * this.decimalPlace {number|function} the decmal place
     */
    getNumericEditor: function (container, options) {
        var self = this;
        var decimalPlace;
        var html;

        //Get the decimal place
        if (typeof self.decimalPlace === 'number') {
            decimalPlace = self.decimalPlace;
        } else {
            decimalPlace = sg.utls.grid.execFuncByName(self.decimalPlace, window);
        }

        var finder = sg.utls.grid.generateFinderButton(options, self.finderInfo);

        var tbId = "tbGridCell_" + options.field;

        //get the html element for cell
        if (finder) {
            html = '<input id="' +
                tbId +
                '" type="text"  class="pr25 numeric"  name="' +
                options.field +
                '" />';
        }
        else {
            html = '<input id="' +
                tbId +
                '" type="text"  class="numeric"  name="' +
                options.field +
                '" />';
        }

        var input = $(html).appendTo(container).kendoNumericTextBox({
            spinners: false,
            step: 0,
            decimals: decimalPlace,
            format: "n" + decimalPlace,
            restrictDecimals: true
        }).data("kendoNumericTextBox");

        //regist finder event
        if (finder) {
            $(finder).appendTo(container);
            sg.utls.grid.registerFinderEvent(container, options, tbId, self);
        }
    },

    /**
    * This method is used to get the editor for default input box
    * @param { any } container The jQuery object representing the container element.
    * @param { any } options Object
    * options.field { string } The name of the field to which the column is bound.
    * options.format { string } The format string of the column specified via the format option.
    * options.model { kendo.data.Model } The model instance to which the current table row is bound.
    * options.values { Array } Array of values specified via the values option.
    */
    getDefaultEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container[0].cellIndex, options.field);
        var decimalPlace = self.decimalPlace;
        var data = $("#" + self.gridId).data("kendoGrid").dataSource.data();
        var tbId = "tbGridCell_" + options.field;
        var html = '<input id="' +
            tbId +
            '" type="text"  class="pr25  txt-upper"  name="' +
            options.field +
            '" />';
        $(html).appendTo(container);
    },

    /**
     * This function is used to create finder button 
     * @param {any} options Object
     * @param {any} finderInfo finder information
     * options.field { string } The name of the field to which the column is bound.
     * finderInfo.showFinder {string|bool} provide the function name or bool value to show/hide the finder icon
     * @return {html} the button element of finder button
     */
    generateFinderButton: function (options, finderInfo) {
        var finder = "";
        if (finderInfo) {
            var btnId = sg.utls.grid.generateFinderId(options);
            var showFinder = false;
            if (finderInfo.showFinder && typeof finderInfo.showFinder === "boolean") {
                showFinder = finderInfo.showFinder;
            } else {
                //todo will clean  up displayFinder in future
                showFinder = finderInfo.showFinder
                    ? sg.utls.grid.execFuncByName(finderInfo.showFinder, window)
                    : sg.utls.grid.displayFinder(options.model, finderInfo.display);
            }
            if (showFinder) {
                finder = '<input class="icon btn-search" data-sage300uicontrol="type:Button,name:' +
                    options.field +
                    ',changed:0" id="' +
                    btnId +
                    '" name="' +
                    options.field +
                    '" tabindex="-1" type="button"></input>';
            }
        }
        return finder;
    },

    /**
     * This function is used to generate the finder button id
     * @param {any} options Object
     * @return {string} finder button id
     * options.field { string } The name of the field to which the column is bound.
     */
    generateFinderId: function (options) {
        return "btnGridCell" + options.field;
    },

    /**
     * This function is used to register finder button events listener
     * @param {any} container The jQuery object representing the container element.
     * @param {any} options Object
     * options.field { string } The name of the field to which the column is bound.
     * options.model { kendo.data.Model } The model instance to which the current table row is bound.
     * @param {any} tbId text box id
     * @param {any} finderConfiguration finder config object
     * finderConfiguration.finderInfo.setFinder {string function name} is used to selected the correct finder config from the cell which included the multiple finder configurations
     */
    registerFinderEvent: function (container, options, tbId, finderConfiguration) {
        var btnId = sg.utls.grid.generateFinderId(options);
        var finderConfig = "";
        var initKeyValues = [];
        var finderInfo = "";
        if (finderConfiguration.finderInfo) {
            finderInfo = finderConfiguration.finderInfo;
            var filter = "";
            if (finderInfo.setFinder) {
                var finder = sg.utls.grid.execFuncByName(finderInfo.setFinder, window);
                if (finder === null) {
                    finder = finderInfo.setFinder;
                }
                finderInfo = finder;
                finderConfig = finderInfo.config;

            }
            else if (finderConfiguration.finderInfo.multipleFinders) {
                //todo clean up the multipleFinders and removed the function in future
                var con = finderConfiguration.finderInfo.multipleFinders.condition;
                var name;
                do {
                    for (var k in con) {
                        name = k;
                        break;
                    }
                    if (con[name][options.model[name]]) {
                        con = con[name][options.model[name]];
                        finderInfo = con;
                        finderConfig = finderInfo.config;
                        con = "";
                    } else {
                        con = con.condition;
                    }
                } while (con);
            } else {
                finderConfig = finderInfo.config;
            }
        }
        $("#" + btnId).mousedown(function (e) {
            //save current state before finder open
            gridCellFocusSwitch = !gridCellFocusSwitch;
            sg.utls.grid.savePreviousText(finderConfiguration.gridId,
                container[0].cellIndex,
                options.field);

            //Set finder filters
            if (finderInfo.filters) {
                finderInfo.filters.forEach(function (value, index, array) {
                    if (options.model[value]) {
                        initKeyValues.push(options.model[value]);
                    }
                    else {
                        initKeyValues.push(sg.utls.grid.execFuncByName(value, window));
                    }
                });

                var template = $.validator.format(finderConfig.filterTemplate);
                finderConfig.filter = template(initKeyValues);
            }
            //Get current input value on cell
            var input = $("#" + tbId).val() || "";
            initKeyValues.push(input);
            //Set finder init key values
            finderConfig.initKeyValues = initKeyValues;

            //set view finder
            sg.viewFinderHelper.setViewFinder(btnId,
                sg.utls.grid.onFinderSuccess.bind(finderConfig.returnFieldNames),
                finderConfig,
                sg.utls.grid.onFinderCancel);
            sg.utls.grid.finderWasClicked = true;
        });

        sg.utls.findersList[tbId] = btnId;
    },

    /**
    * This function is used to construct finder editor for KendoUI grid
    * @param { any } container The jQuery object representing the container element.
    * @param { any } options Object
    * options.field { string } The name of the field to which the column is bound.
    * options.format { string } The format string of the column specified via the format option.
    * options.model { kendo.data.Model } The model instance to which the current table row is bound.
    * options.values { Array } Array of values specified via the values option.
    */
    getFinderEditor: function (container, options) {
        var self = this;
        var tbId = "tbGridCell_" + options.field;
        var html = '<input id="' +
            tbId +
            '" type="text"  class="pr25  txt-upper"  name="' +
            options.field +
            '" data-bind="value:' +
            options.field +
            '" />';
        var finder = sg.utls.grid.generateFinderButton(options, self.finderInfo);
        html = html + finder;
        $(html).appendTo(container);
        if (finder) {
            sg.utls.grid.registerFinderEvent(container, options, tbId, self);
        }
    },

    /**
     * This function is used to execute function based on the function name and current context
     * This is not only for grid
     * @param {any} name function name
     * @param {any} context current context
     * @return {any|null} the result of the function or null value if function does not exist
     */
    execFuncByName: function (name, context) {
        var arr = name.split(".");
        var func = arr.pop();
        for (var i in arr) {
            if (arr.hasOwnProperty(i)) {
                context = context[arr[i]];
            }
        }
        if (context && context[func]) {
            return context[func].apply(context);
        }
        return null;
    },

    /**
     * 
     * @param {any} model
     * @param {any} finderInfo
     */
    displayFinder: function (model, finderInfo) {
        if (!finderInfo) {
            return true;
        }
        var ret;

        if (finderInfo) {
            var condition = finderInfo.condition;
            var n;
            do {
                for (var k in condition) {
                    n = k;
                    break;
                }
                if (condition.fromUI) {
                    //Triger the function
                    var funcName = condition[n];
                    ret = sg.utls.grid.execFuncByName(funcName, window);
                    if (!condition.condition)
                        return ret;
                }
                else if (condition[n][model[n]]) {
                    ret = condition[n][model[n]];
                    if (ret !== false && ret !== true) {
                        condition = ret.condition;
                        continue;
                    }
                    return ret;
                } else {
                    condition = condition.condition;
                }
            } while (condition);
        }
        return false;
    },

    /**
     * This function is used to set up the Dropdown editor for Kendo Grid
    * @param { any } container The jQuery object representing the container element.
    * @param { any } options Object
    * options.field { string } The name of the field to which the column is bound.
    * options.format { string } The format string of the column specified via the format option.
    * options.model { kendo.data.Model } The model instance to which the current table row is bound.
    * options.values { Array } Array of values specified via the values option.
    */
    getDropdownEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container[0].cellIndex, options.field);
        var grid = $('#' + self.gridId).getKendoGrid();
        var colInfo = grid.columns.find(function (v, i) {
            var col = grid.columns[i];
            if (col.field === options.field) {
                return col;
            }
        });
        var data = colInfo.PresentationList;
        var tbId = "tbGridCell_" + options.field;
        var html = '<input id="' +
            tbId +
            '" name="' +
            options.field +
            '" />';
        $(html).appendTo(container).kendoDropDownList({
            dataSource: data,
            dataValueField: "Value",
            dataTextField: "Text",
            value: options.model[options.field]
        });
    },


    /**
     *retrieve the previous value for grid cell
    */
    retrievePreviousText: function () {
        var prev = !gridCellFocusSwitch ? "A" : "B";
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        var val = cellValTemplate[prev].val;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
            var row = sg.utls.kndoUI.getSelectedRowData(grid);
            if (row) {
                this.isRetrieved = true;
                row.set(field, val);
                setTimeout(function () {
                    sg.utls.grid.isRetrieved = false;
                    sg.utls.grid.updateFailed = false;
                    sg.utls.grid.setDetailFailed = false;
                });
            }
        }
    },

    /**
     * Save the latest correct field value
     * @param {any} gridId Gird Id
     * @param {any} colIndex Column Index
     * @param {any} fieldName Column Name
     * @param {any} val Cell Value
     */
    savePreviousText: function (gridId, colIndex, fieldName, val) {
        var grid = $("#" + gridId).data("kendoGrid");
        var row = sg.utls.kndoUI.getSelectedRowData(grid);
        var rowIndex = grid.select().index();
        var curr = sg.utls.grid.getPrevState();
        cellValTemplate[curr].gridId = gridId;
        cellValTemplate[curr].row = rowIndex;
        cellValTemplate[curr].col = colIndex;
        cellValTemplate[curr].field = fieldName;
        if (val) {
            cellValTemplate[curr].val = val;
        } else {
            cellValTemplate[curr].val = row[fieldName];
        }
    },

    errorMsgClose: function () {
        $("#" + this).unbind("keydown");
        sg.utls.grid.retrievePreviousText();
    },

    onError: function (elementId, ret) {
        $("#" + elementId).keydown(false);
        sg.utls.showMessage(ret, this.errorMsgClose.bind(elementId));
    },

    /**
     * This fucntion is used to control the tab key for cell switch
     * @param {any} grid kendo grid
     * @param {any} currentCellIndex current cell index
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
            return cell;
        }
        return null;
    },

    /**
     * This function is used to set the value from finder
     * @param {any} ret
     */
    getPreviousRow: function () {
        var prev = gridCellFocusSwitch ? "A" : "B";
        return cellValTemplate[prev].row;
    },
};
