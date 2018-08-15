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

//---------------------------------------** Initialise field for SourceCode grid **----------------------------

var glIntegrationSourceCode = {

    SerialNumberHidden: $(sourceCodeGridColumns.headerSerialNumber).attr("hidden") ? $(sourceCodeGridColumns.headerSerialNumber).attr("hidden") : true,

    TransactionTypeTitle: sourceCodeGridColumns.headerTransactionType,
    TransactionTypeHidden: $(sourceCodeGridColumns.headerTransactionType).attr("hidden") ? $(sourceCodeGridColumns.headerTransactionType).attr("hidden") : false,

    SourceLedgerTitle: sourceCodeGridColumns.headerSourceLedger,
    SourceLedgerHidden: $(sourceCodeGridColumns.headerSourceLedger).attr("hidden") ? $(sourceCodeGridColumns.headerSourceLedger).attr("hidden") : false,

    SourceTypeTitle: sourceCodeGridColumns.headerSourceType,
    SourceTypeHidden: $(sourceCodeGridColumns.headerSourceType).attr("hidden") ? $(sourceCodeGridColumns.headerSourceType).attr("hidden") : false,
};

//*********************************** GL Integration SourceCode GRID ***********************************************


var SourceCodeGridUtility = {
    fetchSourceCodeGrid: function () {
        return $('#gridGLSourceCodes').data("kendoGrid");
    },
    isNullOrEmpty: function (variable) {
        if (variable === null || typeof variable === "undefined") {
            return true;
        }
        return (variable === "");
    },
    resetFocus: function (dataItem, index) {
        if (dataItem !== undefined) {
            var grid = SourceCodeGridUtility.fetchSourceCodeGrid();
            var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
            grid.closeCell();
            grid.editCell(row.find(">td").eq(index));
            grid.select(row);
        }
    },
    glSourceCodeConfig: {
        scrollable: true,
        navigatable: true,
        resizable: true,
        sortable: false,
        selectable: true,
        reorderable: sg.utls.reorderable,
        param: null,
        pageSize: 20,//sg.utls.gridPageSize,
        page: 0,
        getPageUrl: function () {
            return glIntegrationRepositoryURL.getGLSourceCode();
        },
        isServerPaging: true,
        editable: {
            mode: "incell",
            confirmation: false
        },
        getParam: function () {
            var grid = $('#gridGLSourceCodes').data("kendoGrid");
            var parameters = {
                pageNumber: grid.dataSource.page() - 1,
                pageSize: grid.dataSource.pageSize(),
                model: ko.mapping.toJS(glIntegrationUI.glIntegrationModel)
            };
            return parameters;
        },
        buildGridData: function (successData) {
            successData = successData.GLSourceCodes;
            if (successData != null && successData.TotalResultsCount > 0)
                dirtyFlags.isGridDirty(false);
            var gridData = null;
            gridData = [];
            // Notify the grid which part of the returned data contains the items for the grid. 
            gridData.data = successData.Items;
            // Notify the grid which part of the returned data contains the total number of items for the grid. 
            //12 records available for multicurrency else 10
            gridData.totalResultsCount = successData.TotalResultsCount;
            return gridData;
        },
        save: function (e) {
            if (SourceCodeGridUtility.isNullOrEmpty(e.values.SourceType)) {
                var dataItem = e.model;
                var focusedCellIndex = this.current()[0].cellIndex;

                //display error msg
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeGridColumns.sourceTypeCannotBeBlankMessage);
                setTimeout(function () {
                    dataItem.set("SourceType", "");
                });

                //reset focus
                setTimeout(function () {
                    SourceCodeGridUtility.resetFocus(dataItem, focusedCellIndex);
                });
            } else {
                //clear existing validation msg if any
                glIntegrationUtils.clearValidationMessage();
            }
        },
        columns: [
        {
            field: "TransactionType",
            attributes: {
                "class": "w10 input_Number_Left_Align"
            },
            headerAttributes: {
                "class": "w10 input_Number_Left_Align"
            },
            hidden: glIntegrationSourceCode.TransactionTypeHidden,
            title: glIntegrationSourceCode.TransactionTypeTitle,
            width: "33%",
            editor: function (container, options) {
                var grid = $('#gridGLSourceCodes').data("kendoGrid");
                grid.select(container.closest("tr"));
                sg.utls.kndoUI.nonEditable(grid, container);
            }
        },
        {
            field: "SourceLedger",
            attributes: {
                "class": "w30 input_Number_Left_Align"
            },
            headerAttributes: {
                "class": "w30 input_Number_Left_Align"
            },
            hidden: glIntegrationSourceCode.SourceLedgerHidden,
            title: glIntegrationSourceCode.SourceLedgerTitle,
            width: "33%",
            editor: function (container, options) {
                var grid = $('#gridGLSourceCodes').data("kendoGrid");
                sg.utls.kndoUI.nonEditable(grid, container);
            }
        },
        {
            field: "SourceType",
            attributes: {
                "class": "w30 txt-upper input_Number_Left_Align"
            },
            headerAttributes: {
                "class": "w30 input_Number_Left_Align"
            },
            hidden: glIntegrationSourceCode.SourceTypeHidden,
            title: glIntegrationSourceCode.SourceTypeTitle,
            width: "33%",
            editor: function (container, options) {
                $('<input name="' + options.field + '" id="' + options.field + '" data-bind="value:' + options.field + '" maxlength="2" formatTextbox="alphaNumeric" class="txt-upper w100"/>').appendTo(container)
                    .bind("input", function (e) {
                        var isNullorEmpty = SourceCodeGridUtility.isNullOrEmpty($(this).val());
                        if (isNullorEmpty !== true) {
                            //prevant invalid charactors
                            $(this).val($(this).val().replace(/[^a-z0-9]/gi, '')).change();
                        }
                    });
            }
        },
        {
            field: "SerialNo",
            hidden: glIntegrationSourceCode.SerialNumberHidden
        }
        ]
    }
};
