/* Copyright (c) 2018 Sage Software, Inc.  All rights reserved. */

"use strict";
var sg = sg || {};
sg.utls = sg.utls || {};
sg.utls.grid = sg.utls.grid || {};
var gridCellFocusSwitch = false;

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
    isRetrieved: false,

    /**
     * This function is used to convert the grid column fromat from backend to clientside
     * @param {any} cols grid columns list from backend
     */
    formattingColumns: function (gridId, cols, decimalPlace) {
        cols.forEach(function(col) {
            
            if (col.PresentationList && col.PresentationList.length > 0) {
                col.PresentationMap = col.PresentationList.reduce(function(map, obj) {
                        map[obj.Value] = obj.Text;
                        return map;
                    },
                    {});
                col.template = '#= sg.utls.grid.getDropdownDisplay(' + [col.field, col.columnId] + ') #';
                if (!col.IsReadOnly) {
                    col.editor = sg.utls.grid.getDropdownEditor.bind({
                        "gridId": gridId
                    });
                }
            }
            if (col.dataType === "Decimal") {
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
                col.editor = sg.utls.grid.getFinderEditor.bind({"gridId":gridId, "colInfo": col});
            }
            if (!col.IsReadOnly && !col.editor) {
                col.editor = sg.utls.grid.getDefaultEditor.bind({ "gridId": gridId });
            }
        });
        return cols;
    },

    /**
     * This function is used to set the column information for Kendo Grid
     * @param {any} gridId
     * @param {any} columns
     * @param {any} decimalPlace
     */
    setColumns: function (gridId, columns, decimalPlace, customerCols) {
        var grid = $("#" + gridId).data("kendoGrid");
        var options = grid.getOptions();
        if (!customerCols)
            customerCols = [];
        if (decimalPlace) {
            options.columns = customerCols.concat(this.formattingColumns(gridId, columns, decimalPlace));
        } else {
            options.columns = customerCols.concat(this.formattingColumns(gridId, columns, null));
        }
        grid.setOptions(options);
    },

    /**
     * This method is used to disply the text based on dropdown value
     * @param {any} val value
     * @param {any} fieldIndex field index
     * @return{string} of display
     */
    getDropdownDisplay: function(val, fieldIndex) {
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
     * @param {any} ret
     */
    onFinderSuccess: function (ret) {
        var val = ret[this.FinderReturnField];
        var prev = gridCellFocusSwitch ? "A" : "B";
        var grid = $("#" + cellValTemplate[prev].gridId).data("kendoGrid");
        var rowIndex = cellValTemplate[prev].row;
        var colIndex = cellValTemplate[prev].col;
        var field = cellValTemplate[prev].field;
        if (cellValTemplate[prev].gridId) {
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
            var row = sg.utls.kndoUI.getSelectedRowData(grid);
            if (row) {
                sg.utls.grid.isRetrieved = true;
                row.set(field, val);
                setTimeout(function () {
                    sg.utls.grid.isRetrieved = false;
                });
            }
        }
    },

    /**
     * This method is used to get the editor for numeric input box
     * @param {any} container Container
     * @param {any} options optons
     */
    getNumericEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container.context.cellIndex, options.field);
        var decimalPlace = self.decimalPlace;
        var data = $("#"+self.gridId).data("kendoGrid").dataSource.data();
        var tbId = "tbGridCell_" + options.field;
        var html = '<input id="' +
            tbId +
            '" type="text"  class="numeric"  name="' +
            options.field +
            '" />';

        var input = $(html).appendTo(container).kendoNumericTextBox({
            spinners: false,
            step: 0,
            format: "n" + decimalPlace,
            restrictDecimals: true
        }).data("kendoNumericTextBox");

        if (self.colInfo.FinderReturnField) {
            var btnId = "btnGridCell" + options.field;
            var finder = '<input class="icon btn-search" data-sage300uicontrol="type:Button,name:' +
                options.field +
                ',changed:0" id="' +
                btnId +
                '" name="' +
                options.field +
                '" tabindex="-1" type="button"></input>';
            html = input + finder;
            $(html).appendTo(container);
            sg.viewFinderHelper.setViewFinder(btnId, sg.utls.grid.onFinderSuccess, { viewID: "PM0021", viewOrder: 0, displayFieldNames: ["FMTCONTNO", "DESC", "CUSTOMER", "MANAGER", "STATUS", "STARTDATE", "CURENDDATE", "CLOSEDDATE"], returnFieldNames: ["FMTCONTNO"] });
        }
    },

    getDefaultEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container.context.cellIndex, options.field);
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

    getFinderEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container.context.cellIndex, options.field);

        var tbId = "tbGridCell_" + options.field;
        var btnId = "btnGridCell" + options.field;
        var html = '<input id="' + tbId + '" type="text"  class="pr25  txt-upper"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
        var finder = '<input class="icon btn-search" data-sage300uicontrol="type:Button,name:' + options.field + ',changed:0" id="' + btnId + '" name="' + options.field + '" tabindex="-1" type="button"></input>';
        html = html + finder;
        $(html).appendTo(container);
        if (self.colInfo.FinderReturnField === "PROJECT") {
            sg.viewFinderHelper.setViewFinder(btnId,
                sg.utls.grid.onFinderSuccess.bind(self.colInfo),
                {
                    viewID: "PM0022",
                    viewOrder: 0,
                    displayFieldNames: [
                        "PROJECT", "DESC", "CUSTOMER", "IDACCTSET", "CUSTCCY", "MULTICUST", "PONUMBER", "PROJSTAT",
                        "PROJTYPE", "REVREC", "BILLTYPE", "CLOSEBILL", "CLOSECOST", "STARTDATE", "CURENDDATE",
                        "ORJENDDATE", "CLOSEDDATE", "CODETAXGRP"
                    ],
                    returnFieldNames: ["PROJECT"]
                });
        } else {
            sg.viewFinderHelper.setViewFinder(btnId,
                sg.utls.grid.onFinderSuccess.bind(self.colInfo),
                {
                    viewID: "PM0021",
                    viewOrder: 0,
                    displayFieldNames: [
                        "FMTCONTNO", "DESC", "CUSTOMER", "MANAGER", "STATUS", "STARTDATE", "CURENDDATE", "CLOSEDDATE"
                    ],
                    returnFieldNames: ["FMTCONTNO"]
                });
        }
    },

    /**
     * This function is used to set up the Dropdown ediotr for Kendo Grid
     * @param {any} container
     * @param {any} options
     */
    getDropdownEditor: function (container, options) {
        var self = this;
        sg.utls.grid.savePreviousText(self.gridId, container.context.cellIndex, options.field);
        var grid = $('#'+self.gridId).getKendoGrid();
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
    retrievePreviousText: function() {
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
                setTimeout(function() {
                    sg.utls.grid.isRetrieved = false;
                });
            }
        }
    },

    /**
     * Save the latest correct field value
     * @param {any} gridId Gird Id
     * @param {any} colIndex Column Index
     * @param {any} fieldName Column Name
     */
    savePreviousText: function(gridId, colIndex, fieldName) {
        var grid = $("#" + gridId).data("kendoGrid");
        var row = sg.utls.kndoUI.getSelectedRowData(grid);
        var rowIndex = grid.select().index();
        var curr = gridCellFocusSwitch ? "A" : "B";
        cellValTemplate[curr].gridId = gridId;
        cellValTemplate[curr].row = rowIndex;
        cellValTemplate[curr].col = colIndex;
        cellValTemplate[curr].field = fieldName;
        cellValTemplate[curr].val = row[fieldName];
    },

    errorMsgClose: function() {
        $("#" + this).unbind("keydown");
        sg.utls.grid.retrievePreviousText();
    },

    onError: function(elementId, ret) {
        $("#" + elementId).keydown(false);
        sg.utls.showMessage(ret, this.errorMsgClose.bind(elementId), false, true);
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
    }

};
