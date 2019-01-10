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
var gridUtility = gridUtility || {};

gridUtility = {
    getGrid: function () {
        return $('#SegmentCodesGrid').data('kendoGrid');
    },

    pageUrl: sg.utls.url.buildUrl("TU", "SegmentCodes", "Get"),

    getParam: function () {
        var grid = gridUtility.getGrid();
        var pageNumber = grid.dataSource.page();
        var pageSize = grid.dataSource.pageSize();
        var model = ko.mapping.toJS(segmentCodesUI.segmentCodesModel);

        var parameters = {
            model: model,
            pageNumber: pageNumber - 1,
            pageSize: pageSize,
            filters: segmentCodesUI.segmentCodesFilter(),
            index: -1,
            isCacheRemovable: false
        };
        return parameters;
    },

    buildGridData: function (result) {
        if (result && !result.UserMessage.IsSuccess) {
            sg.utls.showMessage(result);
            return null;
        }
        var gridData = { data: [], totalResultsCount: 0 };
        ko.mapping.fromJS(result, segmentCodesUI.segmentCodesModel);
        gridData.data = result.SegmentCodes.Items;
        gridData.totalResultsCount = result.SegmentCodes.TotalResultsCount;
        segmentCodesUI.segmentCodesModel.isModelDirty.reset();
        segmentCodesUI.segmentCodesModel.SegmentNameLength(segmentCodesUI.segmentLength);
        return gridData;
    },

    afterDataBind: function () {
        var selectAllCheckBox = $("#selectAllChk");
        var grid = gridUtility.getGrid();

        if (selectAllCheckBox.is(':checked')) {
            selectAllCheckBox.prop("checked", false).applyCheckboxStyle();
            $("#btnDeleteLine").attr("disabled", true);
            return;
        }
        grid.tbody.find(".selectChk").each(function () {
            if (!($(this).is(':checked'))) {
                $("#btnDeleteLine").attr("disabled", true);
                return;
            }
        });

        if (segmentCodesUIGrid.addLineClicked) {
            var editableRow = (segmentCodesUIGrid.insertedIndex + 1) % grid.dataSource.pageSize();
            var index = window.GridPreferencesHelper.getColumnIndex('#SegmentCodesGrid', "SegmentCode");
            var cell = grid.tbody.find(">tr:eq(" + editableRow + ") >td:eq(" + index + ")");
            grid.editCell(cell);
            segmentCodesUIGrid.addLineClicked = false;
        }
        var disabled = (segmentCodesUI.segmentCodesModel.SegmentCodes.Items().length === 0);
        $("#selectAllChk").attr("disabled", disabled);
    },

    bindAllEvents: function () {
        SegmentCodesGridConfig.editorEvents.SegmentCode.PreEditEvent = function (container, options) {
            // Attach edit input box
            if (options.model.IsNewLine) {
                segmentCodesUIGrid.segmentNumber = options.model.uid;
                segmentCodesUIGrid.serialNumber = (options.model.SerialNumber);
                return true;
            }
            else {
                sg.utls.kndoUI.nonEditable(gridUtility.getGrid() , container);
                return false;
            }
        };

        SegmentCodesGridConfig.editorEvents.SegmentCode.PostEditEvent = function (container, options) {
            $('#SegmentCode').attr('maxlength', segmentCodesUI.segmentLength);
        };
        
        SegmentCodesGridConfig.editorEvents.SegmentCode.OnChangeEvent = function (e) {
            var grid = gridUtility.getGrid();
            var vm = segmentCodesUI.segmentCodesModel;
            var value = e.target.value.trim();
            // segment code length validation.
            segmentCodesUIGrid.invalidCode = true;
            if (value.length <= 0) {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(segmentCodesResources.SegmentCodeBlank, segmentCodesResources.SegmentCodeTitle));
            } else if (value.length !== segmentCodesUI.segmentLength) {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(segmentCodesResources.InvalidSegmentLength, segmentCodesResources.SegmentCodeTitle, segmentCodesUI.segmentLength));
            } else {
                segmentCodesUIGrid.invalidCode = false;
                segmentCodesUIGrid.SegmentCode = value.toUpperCase();
                // check duplicate record 
                var gridData = grid.dataSource.data();
                var rows = gridData.filter(function (row) { return row.SegmentCode && row.SegmentCode.toUpperCase() === value.toUpperCase() && !row.IsDeleted; });
                if (rows.length > 0) {
                    var errMessage = jQuery.validator.format(segmentCodesResources.RecordExistsError, segmentCodesResources.SegmentCodeTitle, value);
                    var result = { IsValidSegmentCode: false, ErrorMessage: errMessage };
                    segmentCodesUISuccess.segmentCodeValid(result);
                } else {
                    var model = ko.mapping.toJS(vm.SegmentCodes);
                    var segmentNumber = $("#SegmentNameList").data("kendoDropDownList").value();
                    var data = { 'model': model, 'segmentNumber': segmentNumber, 'segmentCode': value };
                    segmentCodesRepository.exists(data);
                }
            }
            return true;

        };

        SegmentCodesGridConfig.editorEvents.Description.PreEditEvent = function (container, options) {
            segmentCodesUIGrid.segmentNumber = options.model.uid;
            return true;
        };
    }
};

var segmentCodesUIGrid =
{
    currentRow: "",
    segmentNumber: null,
    closingAccount: null,
    rowIndex: null,
    segmentGrid: null,
    addLineClicked: false,
    insertedIndex: 0,
    SegmentCode: null,

    // Setting the new line items.
    newLineItem: function () {
        return {
            "SerialNumber": sg.utls.generatekey(),
            "SegmentCode": null,
            "Description": null,
            "IsNewLine": true,
            "IsDeleted": false,
            "DisplayIndex": segmentCodesUI.dataIndex,
            "SegmentNumber": segmentCodesUI.segmentNumber
        };

    },

    // Setting the pagination.
    getParamPaging: function (data, pageNumber, pageSize, newInsertIndex) {
        var model = ko.mapping.toJS(segmentCodesUI.segmentCodesModel);
        var allocationData = model.SegmentCodes.Items;

        allocationData = sg.utls.kndoUI.assignDisplayIndex(allocationData, segmentCodesUI.currentPageNumber, pageSize);
        model.SegmentCodes.Items = allocationData;

        SegmentCodesGridConfig.param = {
            pageNumber: pageNumber,
            pageSize: pageSize,
            index: newInsertIndex,
            model: model,
            segmentNumber: segmentCodesUI.segmentNumber,
            isCacheRemovable: false
        };
    },

    // Reseting the focus on selected row cell.
    resetFocus: function (dataItem, columnName) {
        setTimeout(function () {
            var index = window.GridPreferencesHelper.getColumnIndex('#SegmentCodesGrid', columnName);
            var grid = gridUtility.getGrid();
            var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
            grid.editCell(row.find(">td:eq(" + index + ")"));
        }, 10);
    },

    // Deleteing the line
    deleteLine: function () {
        segmentCodesUIGrid.addLineClicked = false;
        var grid = gridUtility.getGrid();
        var listChecked = [];
        var vm = segmentCodesUI.segmentCodesModel;
        var data = { model: ko.mapping.toJS(vm.SegmentCodes) };

        grid.tbody.find(":checked").closest("tr").each(function () {
            var row = grid.dataItem($(this));
            row.isDirty = true;
            row.IsDeleted = true;
            segmentCodesUI.isDelete = true;
            listChecked.push(row);
        });

        if (listChecked.length > 0) {
            data.model.Items = listChecked;
            segmentCodesRepository.segmentCodeUsed(data);
        }

        if (grid.dataSource.total() === 0) {
            $("#selectAllChk").attr("checked", false).parent().attr("class", "icon checkBox");
            $("#selectAllChk").attr("disabled", true);
        }
    },

    // Add the line
    addLine: function (data) {
        segmentCodesUIGrid.addLineClicked = true;
        var grid = gridUtility.getGrid();
        var newLineExist = false;
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        var items = data.Items();
        // Check New line exist or not
        if (items && items.length > 0) {
            $.each(data.Items(), function (index, item) {
                if (!item.SegmentCode() && !item.IsDeleted()) {
                    segmentCodesUIGrid.addLineClicked = false;
                    newLineExist = true;
                    return false;
                }
            });
            segmentCodesUI.dataIndex = data.TotalResultsCount() + 1;
        } else {
            segmentCodesUI.dataIndex = 1;
        }

        var pageNumber = grid.dataSource.page();
        segmentCodesUIGrid.insertedIndex = grid.dataSource.indexOf(currentRowGrid);
        segmentCodesUI.currentPageNumber = pageNumber;

        // If new line is exist with blank segmentcode then don't add the line.
        if ((!newLineExist)) {
            var isClientSideAdditon = sg.utls.kndoUI.addLine(data, "SegmentCodesGrid", segmentCodesUIGrid.newLineItem, segmentCodesUIGrid.getParamPaging, pageNumber);
            if (isClientSideAdditon) {
                var totalResultCount = data.TotalResultsCount();
                data.TotalResultsCount(totalResultCount + 1);
            }
        }
        sg.controls.enable("#selectAllChk");
    },
};