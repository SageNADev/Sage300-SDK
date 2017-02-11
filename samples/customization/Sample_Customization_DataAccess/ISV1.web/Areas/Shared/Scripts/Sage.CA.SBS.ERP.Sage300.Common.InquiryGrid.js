/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";

/* Inquiry Grid Helper */
sg.utls.InquiryGridHelper = sg.utls.InquiryGridHelper || {};

sg.utls.InquiryGridHelper = {


    afterDataBind: function (gridName, detailButtonName, doubleClickEventCallBack) {

        sg.utls.InquiryGridHelper.gridDataExist(gridName, detailButtonName);

        //// Register double click event on the each of the selected row
        //$(gridName).off("dblclick").on("dblclick", "tr.k-state-selected", function () {
        //    doubleClickEventCallBack(sg.utls.InquiryGridHelper.getSelectedRowModel(gridName));
        //});
    },

    // When any value is changed, clear the grid
    clearGridData: function (gridName) {
        $(gridName).data('kendoGrid').dataSource.data([]);
    },

    //Check whether grid have a record or not
    gridDataExist: function (gridName, detailButtonName) {
        if ($(gridName).data("kendoGrid").dataSource.total() === 0) {
            sg.controls.disable(detailButtonName);
        } else {
            sg.controls.enable(detailButtonName);
        }
    },

    gridNonEditable: function (gridName, container) {
        $(gridName).data("kendoGrid").select(container.closest("tr"));
    },

    getSelectedRowModel: function(gridName) {
        var grid = $(gridName).data("kendoGrid");

        // return the selected row.
        return sg.utls.kndoUI.getSelectedRowData(grid);
    },

    initDropDown: function (fields) {
        var kendoUi = sg.utls.kndoUI;
        $.each(fields, function (index, field) {
            kendoUi.dropDownList(field);
        });
    },
};

