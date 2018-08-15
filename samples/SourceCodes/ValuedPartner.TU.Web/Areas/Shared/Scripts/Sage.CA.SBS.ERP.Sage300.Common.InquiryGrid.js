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

