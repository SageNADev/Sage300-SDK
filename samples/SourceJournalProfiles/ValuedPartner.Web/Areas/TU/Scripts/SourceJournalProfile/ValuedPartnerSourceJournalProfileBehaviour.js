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

var sourceJournalProfileUI = {
    ignoreIsDirtyProperties: ["SourceJournalName"],
    souceJournalName: null,
    sourceJournalEtag: null,
    sourceJournalModel: {},
    hasKoApplied: false,
    dataIndex: null,
    SerialNumber: null,
    sourceCodes: null,
    hasInvalidData: false,
    stockTransactionInquiryUIMode: { DEFAULT: 0, LOAD: 1 },
    UIMode: ko.observable(0),
    sourceJournalChange: ko.observable(""),
    sourceJournalLineId: null,
    previousValue: null,
    loadChangedSourceJournal: false,
    init: function () {
        sg.utls.maskSourceCode("sg-mask-sourcecode");
        sourceJournalProfileUI.initGrid();
        sourceJournalProfileUI.initFinders();
        sourceJournalProfileUI.initButtons();
        sourceJournalProfileUISuccess.initialLoad(sourceJournalViewModel);
        sourceJournalProfileUI.initTextBox();
        sourceJournalProfileUI.initCheckBox();
    },

    initGrid: function() {
        sg.utls.mergeGridConfiguration(["pageUrl", "getParam", "buildGridData", "afterDataBind", "dataChange"], SourceCodeGridConfig, sourceJournalProfileGrid.utility);
        sourceJournalProfileGrid.bindAllEvents();
    },
    
    initFinders: function () {
        sourceJournalProfileFinderDeclaration.initSourceJournalFinder();
    },

    initTextBox: function () {
        $("#Data_SourceJournalName").bind('change', function (e) {
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.sourceJournalChange);
        });
    },

    initCheckBox: function () {

        $(document).on("change", "#selectAllChk", function () {

            var grid = $('#SourceCodeGrid').data("kendoGrid");
            grid.closeCell();
            var checkbox = $(this);
            var rows = grid.tbody.find("tr");
            rows.find("td:first input").prop("checked", checkbox.is(":checked")).applyCheckboxStyle();

            if ($("#selectAllChk").is(":checked")) {
                rows.addClass("k-state-active");
                sg.controls.enable("#btnDeleteLine");
            } else {
                rows.removeClass("k-state-active");
                sg.controls.disable("#btnDeleteLine");
            }
        });

        $(document).on("change", ".selectChk", function () {

            var grid = $('#SourceCodeGrid').data("kendoGrid");

            grid.closeCell();
            $(this).closest("tr").toggleClass("k-state-active");
            var allChecked = true;
            var hasChecked = false;
            grid.tbody.find(".selectChk").each(function (index) {
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
            if (hasChecked) {
                sg.controls.enable("#btnDeleteLine");
            } else {
                sg.controls.disable("#btnDeleteLine");
            }
        });
    },
    initButtons: function () {
        //----------------------------------------------------options link start--------------------------------------------------
        sg.exportHelper.setExportEvent("btnOptionExport", sg.dataMigration.GLSourceJournalProfile, false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", sg.dataMigration.GLSourceJournalProfile, false, $.noop);

        //----------------------------------------------------options link end--------------------------------------------------
        $("#btnSave").bind('click', function () {
            $('#message').empty();
            sourceJournalProfileUI.sourceJournalModel.Data.ETag(sourceJournalProfileUI.sourceJournalEtag);
            sg.utls.SyncExecute(sourceJournalRepository.saveSourceJournal(sourceJournalProfileUI.sourceJournalModel.Data));
        });

        // Add Line in Grid
        $('#btnAddLine').on('click', function () {
            sg.utls.SyncExecute(sourceJournalProfileUtility.addNewSourceJournalLine);
        });

        // Delete Line in Grid
        $('#btnDeleteLine').on('click', function () {
            $('#message').empty();
            sg.utls.SyncExecute(sourceJournalProfileUtility.deleteSourceJournalLine);
        });

        $("#btnDeleteSourceJounalProfile").bind('click', function () {
            $('#message').empty();
            if ($("#frmSourceJournalProfile").valid()) {
                var message = jQuery.validator.format(sourceJournalProfileResources.DeleteConfirmMessage, sourceJournalProfileResources.SourceJournalProfile, sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    sourceJournalRepository.deleteSourceJournal(sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
                },
                    null, message,
                    sourceJournalProfileResources.DeleteTitle);
            }
        });

        $("#btnNewSourceJournal").bind("click", function (e) {
            sourceJournalProfileUI.checkIsDirty(sourceJournalRepository.create);
        });

    },

    sourceJournalChange: function () {
        var value = $("#Data_SourceJournalName").val();
        if (value != null) {
            sg.controls.enable("#btnAddLine");
            sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(value);
            sourceJournalProfileUI.souceJournalName = value;
            sourceJournalProfileUI.loadChangedSourceJournal = true;
            sourceJournalRepository.get(value);
        }
    },

    checkIsDirty: function (functionToCall) {
        if (sourceJournalProfileUI.sourceJournalModel.isModelDirty.isDirty() && sg.controls.GetString(sourceJournalProfileUI.souceJournalName) != "") {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    functionToCall.call();
                },
                function () { // No
                    if (sourceJournalProfileUI.souceJournalName != sg.controls.GetString(sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName())) {
                        sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(sourceJournalProfileUI.souceJournalName);
                    }
                    return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, sourceJournalProfileResources.SourceJournalProfile, sourceJournalProfileUI.souceJournalName));
        } else {
            functionToCall.call();
        }
    },
};

var sourceJournalProfileFinderDeclaration = {
    // Initializing the Source Journal Finder Declaration finder properties
    initSourceJournalFinder: function () {
        var title = $.validator.format(sourceJournalProfileResources.FinderTitle, sourceJournalProfileResources.SourceJournalProfile);
        sg.finderHelper.setFinder("btnSourceJournalCodeFinder", "tusourcejournalprofile", onFinderSuccess.onSourceJournalProfile, $.noop, title, sourceJournalFilter.sourceJournalProfile);
    }

};

var sourceJournalFilter = {
    sourceJournalProfile: function () {
        var filters = [[]];
        var sourceJournalName = $("#Data_SourceJournalName").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceJournalName", sg.finderOperator.StartsWith, sourceJournalName);
        return filters;
    },
    sourceCode: function () {
        var filters = [[]];
        var sourceCode = $("#Source").val().toUpperCase();
        var splitParameters = sourceCode.split("-");
        var SourceLedger = splitParameters[0];
        var SourceType = splitParameters[1];
        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, SourceLedger);
        filters[0][1] = sg.finderHelper.createFilter("SourceType", sg.finderOperator.StartsWith, SourceType);
        return filters;
    }
};

var onFinderSuccess = {
    onSourceJournalProfile: function (result) {
        $('#message').empty();
        if (result != null) {
            sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(result.SourceJournalName);
            $("#Data_SourceJournalName").val(result.SourceJournalName);
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.sourceJournalChange);
            sg.controls.Focus($("#Data_SourceJournalName"));
        }
    },
    onSourceCode: function (data) {
        $('#message').empty();
        var grid = $("#SourceCodeGrid").data("kendoGrid");
        if (grid) {
            var row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
            var gridData = grid.dataItem(row);

            var source = data.SourceLedger + "-" + data.SourceType;
            gridData.set("Source", source);
            gridData.set("PreviousSourceValue", source);
            gridData.set("Description", data.Description);

            sourceJournalProfileGrid.resetFocus(grid, gridData, 1);
            sourceJournalProfileUtility.checkDuplicateRecord(gridData);
        }
    },
};

var sourceJournalProfileUISuccess = {
    initialLoad: function (result) {
        $('#message').empty();
        if (result) {
            var uiMode;
            sourceJournalProfileUI.sourceJournalModel = ko.mapping.fromJS(result);
            if (sourceJournalProfileUISuccess.isNew(sourceJournalProfileUI.sourceJournalModel.Data)) {
                uiMode = sg.utls.OperationMode.NEW;
            }
            else {
                uiMode = sg.utls.OperationMode.SAVE;
            }
            SourceJournalObservableExtension(sourceJournalProfileUI.sourceJournalModel.Data, uiMode);
            sourceJournalProfileUI.sourceJournalModel.isModelDirty = new ko.dirtyFlag(sourceJournalProfileUI.sourceJournalModel.Data, sourceJournalProfileUI.ignoreIsDirtyProperties);
            ko.applyBindings(sourceJournalProfileUI.sourceJournalModel);
        }
        else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceJournalProfileResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#Data_SourceJournalName"));
        sg.controls.disable("#btnAddLine");
        sg.controls.disable("#btnDeleteLine");
    },
    deleteSourceJournal: function (result) {
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
            sourceJournalProfileUI.sourceJournalEtag = result.Data.ETag;
            sourceJournalProfileUI.sourceJournalModel.Data.UIMode(sg.utls.OperationMode.NEW);
            sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
            var grid = $("#SourceCodeGrid").data("kendoGrid");
            grid.dataSource.page(1);
            sg.controls.Focus($("#Data_SourceJournalName"));
        }
        sg.utls.showMessage(result);
    },
    update: function (result) {
        $('#message').empty();
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
            sourceJournalProfileUI.sourceJournalEtag = sourceJournalProfileUI.sourceJournalModel.Data.ETag();
            sourceJournalProfileUI.sourceJournalModel.Data.UIMode(sg.utls.OperationMode.SAVE);
            sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
            var grid = $("#SourceCodeGrid").data("kendoGrid");
            grid.dataSource.page(1);
        }
        sg.utls.showMessage(result);
    },
    isExistSuccess: function (result) {
        if (result.IsSourceCodeExists) {
            sg.utls.showMessage(result);
            sourceJournalProfileUISuccess.clearGridRowData();
        }
    },
    clearGridRowData: function () {
        var grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        var row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
        var gridData = grid.dataItem(row);
        if (gridData != null) {
            gridData.set("Source", null);
            gridData.set("Description", null);
            gridData.set("PreviousSourceValue", null);
            sourceJournalProfileGrid.resetFocus(grid, gridData, 1);
        }
    },
    displayResult: function (result, functionToCall) {
        if (result) {
            functionToCall(result);
            sg.utls.showMessage(result);
        }
    },
    get: function (result) {
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            if (result.Data != null) {
                sourceJournalProfileUI.sourceJournalEtag = result.Data.ETag;

                if (result.Data.Exist) {
                    ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
                    sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(result.Data.SourceJournalName);
                    sourceJournalProfileUI.sourceJournalModel.Data.Exist(result.Data.Exist);
                    sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
                } else {
                    ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
                    sourceJournalProfileUI.sourceJournalModel.Data.Exist(result.Data.Exist);
                    sourceJournalProfileUI.sourceJournalModel.isModelDirty.isDirty(true);
                }

                var grid = $('#SourceCodeGrid').data("kendoGrid");
                grid.dataSource.page(1);
                sg.controls.Focus($("#btnAddLine"));
            } else {
                sg.utls.showMessage(result);
            }
        }
    },

    sourceCodeSucess: function (jsonResult) {
        $('#message').empty();
        var grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        var row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
        var gridData = grid.dataItem(row);

        if (jsonResult.UserMessage.IsSuccess) {
            if ($('#SourceCodeGrid')) {
                var source = jsonResult.SourceCode.SourceLedger + "-" + jsonResult.SourceCode.SourceType;
                if (gridData != null) {
                    gridData.set("Source", source);
                    gridData.set("PreviousSourceValue", source);
                    gridData.set("Description", jsonResult.SourceCode.Description);
                }

                sourceJournalProfileGrid.resetFocus(grid, gridData, 0);
                sourceJournalProfileUtility.checkDuplicateRecord(gridData);
            }
        } else {
            sourceJournalProfileUISuccess.clearGridRowData();
            sg.utls.showMessage(jsonResult);
        }
    },
    isNew: function (model) {
        if (model.SourceCodeID01() == null) {
            return true;
        }
        return false;
    },
    create: function (result) {
        sg.controls.disable("#btnAddLine");
        sourceJournalProfileUI.hasKoApplied = false;
        ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
        // sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName("");
        sourceJournalProfileUI.souceJournalName = "";
        var grid = $('#SourceCodeGrid').data("kendoGrid");
        grid.dataSource.page(1);
        sg.controls.Focus($("#Data_SourceJournalName"));
    },

};

var sourceJournalProfileUtility = {

    currentPageNumber: 0,

    newSourceJournalLineItem: function () {
        var newSourceJournalLine = {
            "Source": null,
            "Description": null,
            "IsNewLine": true,
            "IsDeleted": false,
            "DisplayIndex": sourceJournalProfileUI.dataIndex,
            "SerialNumber": sg.utls.generatekey(),
            "PreviousSourceValue": null
        };

        return newSourceJournalLine;
    },

    fetchSourceJournalGrid: function () {
        return $('#SourceCodeGrid').data("kendoGrid");
    },

    // Setting the pagination.
    getSourceJournalParamPaging: function (data, pageNumber, pageSize, newinsertIndex) {
        var page = sourceJournalProfileUtility.currentPageNumber;
        var model = ko.mapping.toJS(sourceJournalProfileUI.sourceJournalModel.Data);
        var sourceJournalData = model.SourceCodeList.Items;

        sourceJournalData = sg.utls.kndoUI.assignDisplayIndex(sourceJournalData, sourceJournalProfileUtility.currentPageNumber, pageSize);
        model.SourceCodeList.Items = sourceJournalData;

        SourceCodeGridConfig.param = {
            pageNumber: pageNumber,
            pageSize: pageSize,
            insertIndex: newinsertIndex,
            model: model,
            bloadSourceJournalChange: sourceJournalProfileUI.loadChangedSourceJournal
        }
    },

    addNewSourceJournalLine: function () {
        var newSourceJournalExist = false;
        var sournceJournalModel = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList;
        if (sournceJournalModel.TotalResultsCount() == 50) {
            var limit50 = jQuery.validator.format(window.sourceJournalProfileResources.Limit50, window.sourceJournalProfileResources.SourceCode);
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, limit50);
            return;

        }
        sourceJournalProfileGrid.addLineClicked = true;
        //Do not allow user to allow a add line when a new empty line already exists
        if (sournceJournalModel.Items() != null && sournceJournalModel.Items().length > 0) {
            $.each(sournceJournalModel.Items(), function (index, item) {
                if ((item.Source() == null || item.Source() == "") && item.IsDeleted() != true) {
                    newSourceJournalExist = true;
                    return;
                }
                sourceJournalProfileUI.dataIndex = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount() + 1;
            });

        } else {
            sourceJournalProfileUI.dataIndex = 1;
        }

        var grid = $('#SourceCodeGrid').data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        sourceJournalProfileUI.insertedIndex = grid.dataSource.indexOf(currentRowGrid);
        if ((!newSourceJournalExist)) {
            var isCurCreditClientSideAdditon =
                sg.utls.kndoUI.addLine(sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList, "SourceCodeGrid", sourceJournalProfileUtility.newSourceJournalLineItem, sourceJournalProfileUtility.getSourceJournalParamPaging, sourceJournalProfileUtility.currentPageNumber);
            if (isCurCreditClientSideAdditon) {
                var currCreditResultCount = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount();
                sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount(currCreditResultCount + 1);
            }
        }
        sg.controls.enable("#selectAllChk");
    },

    deleteSourceJournalLine: function () {
        sourceJournalProfileGrid.deleteLine();
    },

    checkDuplicateRecord: function (row) {
        var grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        var dataSource = grid.dataSource;
        var count = 0;
        //For uncached records
        if (dataSource.total() <= 10) {
            $.each(dataSource.data(), function (key) {
                if (dataSource.data()[key]["Source"] == row.Source) {
                    count = count + 1;
                }
            });

            if (count > 1) {
                var errorMsg = $.validator.format(sourceJournalProfileResources.RecordExist, sourceJournalProfileResources.SourceJournalProfile, row.Source);
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
                sourceJournalProfileUISuccess.clearGridRowData();
            }
        }
        else {
            //For cached records
            sourceJournalRepository.isExist(row.Source, sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
        }
    },

};

$(function () {
    sourceJournalProfileUI.init();
});
