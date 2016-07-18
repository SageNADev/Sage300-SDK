
// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

/*jshint -W097 */
/*global ko*/
/*global segmentCodesResources*/
/*global segmentCodesRepository*/
/*global globalResource*/
/*global GridPreferencesHelper*/
/*global segmentGridFields*/
/*global SegmentCodesViewModel*/
/*global segmentCodesObservableExtension*/

"use strict";

var modelData;
var segmentCodesUI = segmentCodesUI || {};

segmentCodesUI = {
    segmentCodesModel: {},
    segmentCodeLength: [],
    segments: [],
    ignoreIsDirtyProperties: ["SegmentNumber"],
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    gridBindingsApplied: false,
    isKendoControlNotInitialised: false,
    segmentNumber: "",
    segmentNumberText: "",
    segmentLength: 0,
    invalidCode: false,
    isDelete: false,

    // Init
    init: function () {
        segmentCodesUI.initGridConfig();
        segmentCodesUI.initButtons();
        segmentCodesUISuccess.initialLoad(SegmentCodesViewModel);
        segmentCodesUI.initDropDownList();
        segmentCodesUI.initGridCheckBox();
        segmentCodesUISuccess.setkey();
        segmentCodesUI.segmentLength = segmentCodesUI.segmentCodeLength[0];
        segmentCodesUI.segmentNumber = $("#SegmentNameList").data("kendoDropDownList").value();

        // force grid to get data for the first time
        //gridUtility.getGrid().dataSource.read();
        segmentCodesUI.segmentNumber = null;
        $("#SegmentNameList").data("kendoDropDownList").trigger("change");
    },

    // Extra setup needed to be executed 
    initGridConfig: function () {
        // assign extra function call to connect grid to backend (e.g url, handle after data comes back)
        sg.utls.mergeGridConfiguration(["pageUrl", "getParam", "buildGridData", "afterDataBind"], SegmentCodesGridConfig, gridUtility);

        gridUtility.bindAllEvents();
    },

    // Init Buttons
    initButtons: function () {
        // Delete grid line
        $("#btnDeleteLine").bind("click", function () {
            $("#message").empty();
            var confirmationMsg = ($('.selectChk:checked').length > 1) ? segmentCodesResources.DeleteLinesMessage : segmentCodesResources.DeleteLineMessage;
            confirmationMsg = $.validator.format(confirmationMsg, segmentCodesResources.SegmentNumberTitle);
            sg.utls.showKendoConfirmationDialog(function () {segmentCodesUIGrid.deleteLine(); }, null, confirmationMsg, segmentCodesResources.DeleteTitle);
        });

        // Add grid Line
        $("#btnAddLine").on("click", function () {
            var data = segmentCodesUI.segmentCodesModel.SegmentCodes;
            segmentCodesUIGrid.addLine(data);
        });

        // Save 
        $("#btnSave").bind('click', function () {
            sg.utls.SyncExecute(segmentCodesUI.save);
        });
    },

    // Init Dropdowns here
    initDropDownList: function () {
        $("#SegmentNameList").kendoDropDownList({
            change: function () {
                var select = $("#SegmentNameList").data("kendoDropDownList").select();
                var value = segmentCodesUI.segments[select].Value;
                var text = $("#SegmentNameList").data("kendoDropDownList").text();
                if (segmentCodesUI.segmentNumber !== value) {
                    segmentCodesUI.checkIsDirty(segmentCodesUI.segmentNameChange, segmentCodesUI.segmentNumber);
                    segmentCodesUI.segmentLength = segmentCodesUI.segmentCodeLength[select];
                    segmentCodesUI.segmentNumber = value;
                    segmentCodesUI.segmentNumberText = text;
                }
            }
        });
    },

    // Init grid checkBox
    initGridCheckBox: function () {
        var segmentGrid = gridUtility.getGrid();
        $(document).on("change", "#selectAllChk", function () {
            var checkbox = $(this);
            segmentGrid.tbody.find("tr").find("td:first input").prop("checked", checkbox.is(":checked")).applyCheckboxStyle();
            var allChecked = $("#selectAllChk").is(":checked");
            $("#btnDeleteLine").attr("disabled", !allChecked);
            if (allChecked) {
                segmentGrid.tbody.find("tr").addClass("k-state-active");
            } else {
                segmentGrid.tbody.find("tr").removeClass("k-state-active");
            }
        });

        $(document).on("change", ".selectChk", function () {
            var allChecked = true;
            var hasChecked = false;
            $(this).closest("tr").toggleClass("k-state-active");
            segmentGrid.tbody.find(".selectChk").each(function () {
                if (!($(this).is(':checked'))) {
                    $("#selectAllChk").prop("checked", false).applyCheckboxStyle();
                    allChecked = false;
                    return;
                } else {
                    hasChecked = true;
                }
            });

            if (allChecked) {
                $("#selectAllChk").prop("checked", true).applyCheckboxStyle();
            }
            $("#btnDeleteLine").attr("disabled", !hasChecked);

        });
    },

    // Is Dirty check
    checkIsDirty: function (funcionToCall, segmentNumber) {
        var vm = segmentCodesUI.segmentCodesModel;
        if ((vm.isModelDirty && vm.isModelDirty.isDirty()) || segmentCodesUI.isDelete) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSegmentCodes");
                    funcionToCall.call();
                    segmentCodesUI.isDelete = false;
                },
                function () { // No
                    var dropdown = $("#SegmentNameList").data("kendoDropDownList");
                    if (segmentNumber !== dropdown.value()) {
                        var select = parseInt(segmentNumber) - 1;
                        dropdown.select(select);
                        dropdown.value(segmentNumber);
                        segmentCodesUI.segmentLength = segmentCodesUI.segmentCodeLength[select];
                        segmentCodesUI.segmentNumber = segmentNumber;
                   }
                   return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, segmentCodesResources.SegmentNumberTitle, segmentCodesUI.segmentNumberText));
        } else {
            funcionToCall.call();
        }
    },

    //Filter
    segmentCodesFilter: function () {
        var filters = [[]];
        var dropdownList = $("#SegmentNameList").data("kendoDropDownList");
        var segmentNumber = "";
        if (dropdownList) {
            var select = $("#SegmentNameList").data("kendoDropDownList").select();
            segmentNumber = segmentCodesUI.segments[select].Value;
        }
        filters[0][0] = sg.finderHelper.createFilter("SegmentNumber", sg.finderOperator.Equal, segmentNumber);
        return filters;
    },

    segmentNameChange: function () {
        var grid = gridUtility.getGrid();

        segmentCodesUI.segmentNumber = segmentCodesUI.selectedSegmentCodeValue;

        SegmentCodesGridConfig.param = {
            model: segmentCodesUI.ModelData,
            index: -1,
            pageNumber: grid.dataSource.page() - 1,
            pageSize: grid.dataSource.pageSize(),
            filters:  segmentCodesUI.segmentCodesFilter(),
            isCacheRemovable: true
        };

        grid.dataSource.read();
        if (grid.dataSource.page() !== 1) {
            grid.dataSource.page(1);
        }
        $("#message").empty();
    },

    // Save
    save: function () {
        if (segmentCodesUIGrid.invalidCode) {
            return;
        }
        if ($("#frmSegmentCodes").valid()) {
            segmentCodesUIGrid.addLineClicked = false;
            var data = ko.mapping.toJS(segmentCodesUI.segmentCodesModel.SegmentCodes.Items());
            var items = data.filter(function (item) { return item.SegmentCode && (item.IsNewLine || item.HasChanged || item.IsDeleted); });
            items = items.map(function (item) {
                if (!item.SegmentNumber) {
                    item.SegmentNumber = segmentCodesUI.segmentNumber;
                }
                return item;
            });
            segmentCodesRepository.save({ Items: items });
        }
    }
};

// Callbacks
var segmentCodesUISuccess = {

    // Setkey
    setkey: function () {
        segmentCodesUI.segmentNumber = modelData.SegmentNumber();
    },

    // Display Result
    displayResult: function (jsonResult, uiMode) {
        if (jsonResult !== null) {
            if (!segmentCodesUI.hasKoBindingApplied) {
                segmentCodesUI.segmentCodesModel = ko.mapping.fromJS(jsonResult);
                segmentCodesUI.segments = jsonResult.Segments;
                segmentCodesUI.segmentCodeLength = jsonResult.Segments.map(function(o) {
                    return o.SegmentLength;
                });
                segmentCodesUI.hasKoBindingApplied = true;
                modelData = segmentCodesUI.segmentCodesModel.Data;
                segmentCodesObservableExtension(segmentCodesUI.segmentCodesModel, uiMode);
                segmentCodesUI.segmentCodesModel.isModelDirty = new ko.dirtyFlag(modelData, segmentCodesUI.ignoreIsDirtyProperties);
                ko.applyBindings(segmentCodesUI.segmentCodesModel);
            } else {
                ko.mapping.fromJS(jsonResult, segmentCodesUI.segmentCodesModel);
                modelData.UIMode(uiMode);
                if (uiMode !== sg.utls.OperationMode.NEW) {
                    segmentCodesUI.segmentCodesModel.isModelDirty.reset();
                }
            }

            if (!segmentCodesUI.isKendoControlNotInitialised) {
                segmentCodesUI.isKendoControlNotInitialised = true;
            } 
        }
    },

    // Initial Load
    initialLoad: function (result) {
        if (result) {
            segmentCodesUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, segmentCodesResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtSegmentNumber"));
    },
 
    // Check the segmentcode used result.
    segmentCodeUsed: function (result) {
        $("#message").empty();
        var errorMsg;
        var firstNonDeletedSegmentCode;
        var grid = gridUtility.getGrid();
        // If all the segmentCodes used in item mumbers then straight away show the error message without any operation.
        if (result.IsSegmentCodeUsed && result.DeletedSegmentCodes.length === 0) {
            // Set the error message depends on the selected rows.
            if (grid.tbody.find(":checked").closest("tr").length > 1) {
                var firstCount = 1;
                grid.tbody.find(":checked").closest("tr").each(function () {
                    if (firstCount === 1) {
                        firstNonDeletedSegmentCode = grid.dataItem($(this)).SegmentCode;
                        firstCount++;
                    }
                });
                errorMsg = $.validator.format(segmentCodesResources.DeleteFailedMessage, segmentCodesResources.SegmentCodeTitle, firstNonDeletedSegmentCode);
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
            } else {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, result.UserMessage.Errors[0].Message);
            }
        }

        // If all the segmentCodes not used, then delete it from the grid. 
        if (!result.IsSegmentCodeUsed) {
            grid.tbody.find(":checked").closest("tr").each(function () {
                grid.removeRow($(this));
            });
            // If grid is empty then disable selectAll check box
            if (grid.dataSource.total() === 0) {
                $("#selectAllChk").attr("checked", false).parent().attr("class", "icon checkBox");
            }
            $("#selectAllChk").attr("disabled", grid.dataSource.total() === 0);

            if (grid.dataSource.page() > 0) {
                grid.dataSource.page(grid.dataSource.page());
            }
        }

        // If some of the the segmentCodes are used, then delete not used segmentCodes.
        if (result.IsSegmentCodeUsed && result.DeletedSegmentCodes.length > 0) {
            var deletedRecords = 0;
            // Get first non deleted segmentCode to set error message.
            grid.tbody.find(":checked").closest("tr").each(function () {
                if (deletedRecords === result.DeletedSegmentCodes.length) {
                    firstNonDeletedSegmentCode = grid.dataItem($(this)).SegmentCode;
                }
                deletedRecords++;
            });
            // Delete the grid till used segmentCodes.
            deletedRecords = 0;
            grid.tbody.find(":checked").closest("tr").each(function () {
                if (deletedRecords < result.DeletedSegmentCodes.length) {
                    grid.removeRow($(this));
                    deletedRecords++;
                }
            });

            errorMsg = $.validator.format(segmentCodesResources.DeleteFailedMessage, segmentCodesResources.SegmentCodeTitle, firstNonDeletedSegmentCode);
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);

            if (grid.dataSource.page() > 0) {
                grid.dataSource.page(grid.dataSource.page());
            }
        }
    },

    // Check the post segmentcode valid result.
    segmentCodeValid: function (result) {
        $("#message").empty();
        if (!(result.IsValidSegmentCode)) {
            var grid = gridUtility.getGrid();
            var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "SerialNumber", segmentCodesUIGrid.serialNumber);
            var errorMsg = $.validator.format(result.ErrorMessage, segmentCodesResources.SegmentCodeTitle, currentRowGrid.SegmentCode);
            currentRowGrid.set("SegmentCode", "");
            currentRowGrid.set("Description", "");
            segmentCodesUIGrid.resetFocus(currentRowGrid, "SegmentCode");
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, errorMsg);
        }
    },

    // Save the segment codes
    save: function (result) {
        if (result && result.UserMessage.IsSuccess) {
            ko.mapping.fromJS(result, segmentCodesUI.segmentCodesModel);
            segmentCodesUI.segmentCodesModel.isModelDirty.reset();
            var grid = gridUtility.getGrid();
            grid.dataSource.page(1);
            segmentCodesUI.isDelete = false;
        } 
        sg.utls.showMessage(result);
    },
};

// Initial Entry
$(function () {
    segmentCodesUI.init();
    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && segmentCodesUI.segmentCodesModel.isModelDirty.isDirty()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, segmentCodesResources.SegmentNumberTitle)).text();
        }
    });
});
