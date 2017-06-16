// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

var sourceJournalProfileGrid = {

    addLineClicked: false,
    lastSelectedColumn: null,

    utility: {
        getParam: function () {
            var grid = $('#SourceCodeGrid').data("kendoGrid");
            var pageNumber = grid.dataSource.page();
            var pageSize = grid.dataSource.pageSize();
            var model = ko.mapping.toJS(sourceJournalProfileUI.sourceJournalModel.Data);

            var parameters = {
                pageNumber: pageNumber - 1,
                pageSize: pageSize,
                model: model,
                insertIndex: -1,
                bloadSourceJournalChange: sourceJournalProfileUI.loadChangedSourceJournal
            };

            return parameters;
        },

        // URL to get the data from the server. 
        pageUrl: sg.utls.url.buildUrl("TU", "SourceJournalProfile", "GetSourceJournal"),

        // Call back function when Get is successful. In this, the data for the grid and the total results count are to be set along with updating knockout
        buildGridData: function (successData) {

            var gridData = null;

            // ReSharper disable once QualifiedExpressionMaybeNull
            if (successData !== null && successData.UserMessage.IsSuccess) {
                gridData = [];
                if (successData.Data.SourceCodeList != null) {
                    var dirty = sourceJournalProfileUI.sourceJournalModel.isModelDirty.isDirty();
                    ko.mapping.fromJS(successData.Data.SourceCodeList, {}, sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList);
                    gridData.totalResultsCount = successData.Data.SourceCodeList.TotalResultsCount;

                    var totalRecords = successData.Data.SourceCodeList.TotalResultsCount;

                    if (totalRecords > 0) {
                        gridData.data = successData.Data.SourceCodeList.Items;
                    }
                    else {
                        // If grid data is empty then set pagenumber into 0.
                        gridData.data = null;
                    }


                    if (!dirty && !successData.Data.IsNewLine) {
                        sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
                    }

                }
            }
            else {
                sg.utls.showMessage(successData);
            }
            var grid = $('#SourceCodeGrid').data("kendoGrid");
            if (grid != null || grid != "") {
                sourceJournalProfileUtility.currentPageNumber = grid.dataSource.page();
            } else {
                sourceJournalProfileUtility.currentPageNumber = 0;
            }

            $("#selectAllChk").attr("checked", false).parent().attr("class", "icon checkBox");
            sourceJournalProfileUI.loadChangedSourceJournal = false;
            return gridData;
        },

        afterDataBind: function () {
            var selectAllCheckBox = $("#selectAllChk");
            if (selectAllCheckBox.is(':checked')) {
                selectAllCheckBox.prop("checked", false).applyCheckboxStyle();
                $("#btnDeleteLine").attr("disabled", true);
                return;
            }
            var grid = $("#SourceCodeGrid").data("kendoGrid");

            grid.tbody.find(".selectChk").each(function (index) {
                if (!($(this).is(':checked'))) {
                    $("#btnDeleteLine").attr("disabled", true);
                    return;
                }
            });
            if (sourceJournalProfileGrid.addLineClicked) {
                var editableRow = sourceJournalProfileUI.insertedIndex + 1;
                editableRow = editableRow % sg.utls.gridPageSize == 0 ? 0 : editableRow;
                if (grid.dataSource.data().length == 1) {
                    editableRow = 0;
                }

                var cell = grid.tbody.find(">tr:eq(" + editableRow + ") >td:eq(" + 1 + ")");
                grid.editCell(cell);
                sourceJournalProfileGrid.addLineClicked = false;
            }
            //To disable the header checkbox when there items in the grid are cleared using Clear button
            if (sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.Items().length === 0) {
                $("#selectAllChk").attr("disabled", true);
            } else {
                $("#selectAllChk").attr("disabled", false);
            }
        },
        
        dataChange: function (changedData) {
            sourceJournalProfileUI.SerialNumber = changedData.rowData.SerialNumber;
        },
    },
    
    resetFocus: function (grid, data, columnName) {
        var row = sg.utls.kndoUI.getRowForDataItem(data);
        grid.closeCell();
        grid.editCell(row.find(">td").eq(columnName));
    },

    resetFocus2: function (dataItem, index) {
        var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
        setTimeout(function () {
            sourceJournalProfileUtility.fetchSourceJournalGrid().editCell(row.find(">td").eq(index));
        }, 10);
    },

    deleteLine: function () {
        var confirmationMsg;
        if ($("#selectAllChk").is(":checked") || $('.selectChk:checked').length > 1) {
            confirmationMsg = sourceJournalProfileResources.DeleteLinesConfirm;
        } else {
            confirmationMsg = sourceJournalProfileResources.DeleteLineConfirm;
        }
        sg.utls.showKendoConfirmationDialog(
            //Click on Yes
            function () {
                var grid = $('#SourceCodeGrid').data("kendoGrid");
                grid.tbody.find(":checked").closest("tr").each(function () {
                    grid.removeRow($(this));
                });

                if (grid.dataSource.total() == 0) {
                    $("#selectAllChk").attr("checked", false).parent().attr("class", "icon checkBox");
                    sg.controls.disable("#selectAllChk");
                    $('#message').empty();
                }
                else {
                    sg.controls.enable("#selectAllChk");
                }

                //After deletion of row, check for paginated records exist - if yes get the paginated data
                if (grid._data.length < grid.dataSource.total()) {
                    var pageNumber = grid.dataSource.page();
                    var pageSize = grid.dataSource.pageSize();

                    var retrievePage = pageNumber - 1;
                    if (retrievePage >= (grid.dataSource.total() / pageSize)) {
                        retrievePage = retrievePage - 1;
                    }

                    sourceJournalProfileUtility.getSourceJournalParamPaging(null, retrievePage, pageSize, -1);
                    grid.dataSource.page(retrievePage + 1);


                }

                sg.controls.disable("#btnDeleteLine");
            },
            // Click on No
            function () { },
            confirmationMsg, sourceJournalProfileResources.Delete);
        return false;
    },

    bindAllEvents: function () {
        SourceCodeGridConfig.editorEvents.Source.PreEditEvent = function(container, options) {
            sourceJournalProfileUI.sourceJournalLineId = options.model.uid;
            return true;
        };
        SourceCodeGridConfig.editorEvents.Source.PostEditEvent = function(container, options) {
            $("#Source").change(function (e) {
                var sourceCode = e.target.value;
                var message = "";
                var grid = $('#SourceCodeGrid').data("kendoGrid");
                sg.delayOnBlur(["btnSource", "btnSave"], function () {
                    if (sourceCode != "" && sourceCode != null) {
                        sourceCode = sg.utls.toUpperCase(sourceCode);

                        if (sourceCode == options.model.PreviousSourceValue) {
                            return;
                        }

                        var row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
                        var gridData = grid.dataItem(row);
                        gridData.set("Source", sourceCode);
                        gridData.set("PreviousSourceValue", sourceCode);
                        gridData.set("Description", null);

                        if (sourceCode != "") {
                            var items = sourceCode.split("-");
                            if (items.length > 1 && sourceCode.length > 3) {
                                sourceJournalRepository.getSourceCodeById(items[0], items[1]);
                            } else {
                                var message = $.validator.format(sourceJournalProfileResources.NoBlank, sourceJournalProfileResources.SourceType);
                                sg.utls.showMessageInfoInCustomDiv(sg.utls.msgType.ERROR, message, "message");
                                var row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
                                var gridData = grid.dataItem(row);
                                sourceJournalProfileGrid.resetFocus(grid, gridData, 1);
                            }
                        }
                    } else {

                        var sourceCodeValue = e.target.value;
                        if (sourceCodeValue == "") {
                            sourceCodeValue = null;
                        }

                        if (sourceCodeValue == options.model.PreviousSourceValue) {
                            return;
                        }


                        var message = $.validator.format(sourceJournalProfileResources.NoBlank, sourceJournalColumns.Source);
                        sg.utls.showMessageInfoInCustomDiv(sg.utls.msgType.ERROR, message, "message");
                        sourceJournalProfileUISuccess.clearGridRowData();
                    }
                });
            });
            return true;
        };

        SourceCodeGridConfig.editorEvents.Source.FinderSelect = onFinderSuccess.onSourceCode;
        SourceCodeGridConfig.editorEvents.Source.FinderCancel = $.noop;
        SourceCodeGridConfig.editorEvents.Source.FinderFilter = sourceJournalFilter.sourceCode;
    }
};