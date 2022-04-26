// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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

// @ts-check

"use strict";

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
    sourceJournalLineId: null,
    previousValue: null,
    loadChangedSourceJournal: false,


    /**
     * @name init
     * @description Primary initialization
     * @namespace sourceJournalProfileUI
     * @public
     */
    init: () => {
        sg.utls.maskSourceCode("sg-mask-sourcecode");
        sourceJournalProfileUI.initGrid();
        sourceJournalProfileUI.initFinders();
        sourceJournalProfileUI.initButtons();
        sourceJournalProfileUISuccess.initialLoad(sourceJournalViewModel);
        sourceJournalProfileUI.initTextBox();
        sourceJournalProfileUI.initCheckBox();
    },

    /**
     * @name initGrid
     * @description Initialize the grid
     * @namespace sourceJournalProfileUI
     * @public
     */
    initGrid: () => {
        sg.utls.mergeGridConfiguration(["pageUrl", "getParam", "buildGridData", "afterDataBind", "dataChange"], SourceCodeGridConfig, sourceJournalProfileGrid.utility);
        sourceJournalProfileGrid.bindAllEvents();
    },
    
    /**
     * @name initGrid
     * @description Initialize the finder(s)
     * @namespace sourceJournalProfileUI
     * @public
     */
    initFinders: () => {
        let info = sg.viewFinderHelper.getFinderSettings("GL", "SourceJournalProfiles");
        let buttonId = "btnSourceJournalCodeFinder";
        let dataControlIdOrSuccessCallback = onFinderSuccess.onSourceJournalProfile;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, sourceJournalFilter.sourceJournalProfile);
    },

    /**
     * @name initTextBox
     * @description Initialize the text box
     * @namespace sourceJournalProfileUI
     * @public
     */
    initTextBox: () => {
        $("#Data_SourceJournalName").on('change', (e) => {
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.sourceJournalChange);
        });
    },

    /**
     * @name initCheckBox
     * @description Initialize the checkboxes
     * @namespace sourceJournalProfileUI
     * @public
     */
    initCheckBox: function () {

        $(document).on("change", "#selectAllChk", function () {

            let grid = $('#SourceCodeGrid').data("kendoGrid");
            grid.closeCell();
            let checkbox = $(this);
            let rows = grid.tbody.find("tr");
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

            let grid = $('#SourceCodeGrid').data("kendoGrid");

            grid.closeCell();
            $(this).closest("tr").toggleClass("k-state-active");
            let allChecked = true;
            let hasChecked = false;
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

    /**
     * @name initButtons
     * @description Initialize the buttons
     * @namespace sourceJournalProfileUI
     * @public
     */
    initButtons: () => {
        //----------------------------------------------------options link start--------------------------------------------------
        sg.exportHelper.setExportEvent("btnOptionExport", sg.dataMigration.GLSourceJournalProfile, false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", sg.dataMigration.GLSourceJournalProfile, false, $.noop);

        //----------------------------------------------------options link end--------------------------------------------------
        $("#btnSave").on('click', () => {
            $('#message').empty();
            sourceJournalProfileUI.sourceJournalModel.Data.ETag(sourceJournalProfileUI.sourceJournalEtag);
            sg.utls.SyncExecute(sourceJournalRepository.saveSourceJournal(sourceJournalProfileUI.sourceJournalModel.Data));
        });

        // Add Line in Grid
        $('#btnAddLine').on('click', () => {
            sg.utls.SyncExecute(sourceJournalProfileUtility.addNewSourceJournalLine);
        });

        // Delete Line in Grid
        $('#btnDeleteLine').on('click', () => {
            $('#message').empty();
            sg.utls.SyncExecute(sourceJournalProfileUtility.deleteSourceJournalLine);
        });

        $("#btnDeleteSourceJounalProfile").on('click', () => {
            $('#message').empty();
            if ($("#frmSourceJournalProfile").valid()) {
                let message = jQuery.validator.format(sourceJournalProfileResources.DeleteConfirmMessage, sourceJournalProfileResources.SourceJournalProfile, sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
                sg.utls.showKendoConfirmationDialog(() => {
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    sourceJournalRepository.deleteSourceJournal(sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
                },
                    null, message,
                    sourceJournalProfileResources.DeleteTitle);
            }
        });

        $("#btnNewSourceJournal").on("click", (e) => {
            sourceJournalProfileUI.checkIsDirty(sourceJournalRepository.create);
        });
    },

    /**
     * @name sourceJournalChange
     * @description Event handler for the change event for the Source Journal Name field
     * @namespace sourceJournalProfileUI
     * @public
     */
    sourceJournalChange: () => {
        let value = $("#Data_SourceJournalName").val();
        if (value) {
            sg.controls.enable("#btnAddLine");
            sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(value);
            sourceJournalProfileUI.souceJournalName = value;
            sourceJournalProfileUI.loadChangedSourceJournal = true;
            sourceJournalRepository.get(value);
        }
    },

    /**
     * @name checkIsDirty
     * @description Check the model for any changes. If there are changes, invoke a confirmation
     *              dialog box. If the user selects 'Yes', invoke the callback function passed into the function
     * @namespace sourceJournalProfileUI
     * @public
     * 
     * @param {Function} functionToCall Method to call if user selects 'Yes' in confirmation dialog
     */
    checkIsDirty: (functionToCall) => {
        if (sourceJournalProfileUI.sourceJournalModel.isModelDirty.isDirty() && sg.controls.GetString(sourceJournalProfileUI.souceJournalName) !== "") {
            sg.utls.showKendoConfirmationDialog(
                () => { // Yes
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    functionToCall.call();
                },
                () => { // No
                    if (sourceJournalProfileUI.souceJournalName !== sg.controls.GetString(sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName())) {
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

var sourceJournalFilter = {
    /**
     * @name sourceJournalProfile
     * @description Create the filter for the Source Journal finder
     * @namespace sourceJournalFilter
     * @public
     *  
     * @returns {object} The filters object
     */
    sourceJournalProfile: () => {
        let filters = [[]];
        let sourceJournalName = $("#Data_SourceJournalName").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceJournalName", sg.finderOperator.StartsWith, sourceJournalName);
        return filters;
    },

    /**
     * @name sourceCode
     * @description Create the filter for the Source Code finder
     * @namespace sourceJournalFilter
     * @public
     *
     * @returns {object} The filters object
     */
    sourceCode: () => {
        let filters = [[]];
        let sourceCode = $("#Source").val().toUpperCase();
        let splitParameters = sourceCode.split("-");
        let SourceLedger = splitParameters[0];
        let SourceType = splitParameters[1];
        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, SourceLedger);
        filters[0][1] = sg.finderHelper.createFilter("SourceType", sg.finderOperator.StartsWith, SourceType);
        return filters;
    }
};

var onFinderSuccess = {

    /**
     * @name onSourceJournalProfile
     * @description Event handler for finder selection
     * @namespace onFinderSuccess
     * @public
     * 
     * @param {object} result The JSON payload object
     */
    onSourceJournalProfile: (result) => {
        $('#message').empty();
        if (result) {
            let name = result.SRCEJRNL;
            sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName(name);
            $("#Data_SourceJournalName").val(name);
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.sourceJournalChange);
            sg.controls.Focus($("#Data_SourceJournalName"));
        }
    },

    /**
     * @name onSourceCode
     * @description Event handler for finder selection
     * @namespace onFinderSuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    onSourceCode: (data) => {
        $('#message').empty();
        let grid = $("#SourceCodeGrid").data("kendoGrid");
        if (grid) {
            let row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
            let gridData = grid.dataItem(row);

            let source = `${data.SRCELEDGER}-${data.SRCETYPE}`; 
            gridData.set("Source", source);
            gridData.set("PreviousSourceValue", source);
            gridData.set("Description", data.SRCEDESC);

            sourceJournalProfileGrid.resetFocus(grid, gridData, 1);
            sourceJournalProfileUtility.checkDuplicateRecord(gridData);
        }
    },
};

var sourceJournalProfileUISuccess = {

    /**
     * @name initialLoad
     * @description Called on initial page load
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    initialLoad: (result) => {
        $('#message').empty();
        if (result) {
            let uiMode;
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

    /**
     * @name deleteSourceJournal
     * @description Function called by repository upon successful Source Journal deletion
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    deleteSourceJournal: (result) => {
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
            sourceJournalProfileUI.sourceJournalEtag = result.Data.ETag;
            sourceJournalProfileUI.sourceJournalModel.Data.UIMode(sg.utls.OperationMode.NEW);
            sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
            let grid = $("#SourceCodeGrid").data("kendoGrid");
            grid.dataSource.page(1);
            sg.controls.Focus($("#Data_SourceJournalName"));
        }
        sg.utls.showMessage(result);
    },

    /**
     * @name update
     * @description Function called by repository upon successful Source Journal update
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    update: (result) => {
        $('#message').empty();
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
            sourceJournalProfileUI.sourceJournalEtag = sourceJournalProfileUI.sourceJournalModel.Data.ETag();
            sourceJournalProfileUI.sourceJournalModel.Data.UIMode(sg.utls.OperationMode.SAVE);
            sourceJournalProfileUI.sourceJournalModel.isModelDirty.reset();
            let grid = $("#SourceCodeGrid").data("kendoGrid");
            grid.dataSource.page(1);
        }
        sg.utls.showMessage(result);
    },

    /**
     * @name is Exists success
     * @description Function called by repository upon successful Source Journal IsExist check
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    isExistSuccess: (result) => {
        if (result.IsSourceCodeExists) {
            sg.utls.showMessage(result);
            sourceJournalProfileUISuccess.clearGridRowData();
        }
    },

    /**
     * @name clearGridRowData
     * @description Clears a grids row data
     * @namespace sourceJournalProfileUISuccess
     * @public
     */
    clearGridRowData: () => {
        let grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        let row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
        let gridData = grid.dataItem(row);
        if (gridData) {
            gridData.set("Source", null);
            gridData.set("Description", null);
            gridData.set("PreviousSourceValue", null);
            sourceJournalProfileGrid.resetFocus(grid, gridData, 1);
        }
    },

    /**
     * @name displayResult
     * @description Function Not Used
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     * @param {Function} functionToCall Parameter not used
     */
    displayResult: (result, functionToCall) => {
        if (result) {
            functionToCall(result);
            sg.utls.showMessage(result);
        }
    },

    /**
     * @name get
     * @description Function called by repository upon successful Source Journal get
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    get: (result) => {
        if (result.UserMessage && result.UserMessage.IsSuccess) {
            if (result.Data) {
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

                let grid = $('#SourceCodeGrid').data("kendoGrid");
                grid.dataSource.page(1);
                sg.controls.Focus($("#btnAddLine"));
            } else {
                sg.utls.showMessage(result);
            }
        }
    },

    /**
     * @name sourceCodeSuccess
     * @description Function called by repository upon successful Source Journal get
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     */
    sourceCodeSuccess: (jsonResult) => {
        $('#message').empty();
        let grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        let row = grid.tbody.find("tr[data-uid='" + sourceJournalProfileUI.sourceJournalLineId + "']");
        let gridData = grid.dataItem(row);

        if (jsonResult.UserMessage.IsSuccess) {
            if ($('#SourceCodeGrid')) {
                let source = jsonResult.SourceCode.SourceLedger + "-" + jsonResult.SourceCode.SourceType;
                if (gridData) {
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

    /**
     * @name isNew
     * @description Check to see if model is valid
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} model The current model
     */
    isNew: (model) => {
        if (model.SourceCodeID01() === null) {
            return true;
        }
        return false;
    },

    /**
     * @name create
     * @description Function called by repository upon successful Source Journal create
     * @namespace sourceJournalProfileUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    create: (result) => {
        sg.controls.disable("#btnAddLine");
        sourceJournalProfileUI.hasKoApplied = false;
        ko.mapping.fromJS(result, sourceJournalProfileUI.sourceJournalModel);
        // sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName("");
        sourceJournalProfileUI.souceJournalName = "";
        let grid = $('#SourceCodeGrid').data("kendoGrid");
        grid.dataSource.page(1);
        sg.controls.Focus($("#Data_SourceJournalName"));
    },
};

var sourceJournalProfileUtility = {

    currentPageNumber: 0,

    /**
     * @name newSourceJournalLineItem
     * @description Create a new Source Journal Line object
     * @namespace sourceJournalProfileUtility
     * @public
     *  
     * @returns {object} A newly created Source Journal Line object
     */
    newSourceJournalLineItem: () => {
        let newSourceJournalLine = {
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

    /**
     * @name fetchSourceJournalGrid
     * @description Get a reference to the Source Journal grid
     * @namespace sourceJournalProfileUtility
     * @public
     */
    fetchSourceJournalGrid: () => {
        return $('#SourceCodeGrid').data("kendoGrid");
    },

    /**
     * @name getSourceJournalParamPaging
     * @description Set up pagination for the grid
     * @namespace sourceJournalProfileUtility
     * @public
     * 
     * @param {object} data Not Used
     * @param {number} pageNumber The page number
     * @param {number} pageSize The page size
     * @param {number} newinsertIndex The new insertion index
     */
    getSourceJournalParamPaging: (data, pageNumber, pageSize, newinsertIndex) => {
        let currentPageNumber = sourceJournalProfileUtility.currentPageNumber;
        let model = ko.mapping.toJS(sourceJournalProfileUI.sourceJournalModel.Data);
        let sourceJournalData = model.SourceCodeList.Items; 

        sourceJournalData = sg.utls.kndoUI.assignDisplayIndex(sourceJournalData, currentPageNumber, pageSize);
        model.SourceCodeList.Items = sourceJournalData;

        SourceCodeGridConfig.param = {
            pageNumber: pageNumber,
            pageSize: pageSize,
            insertIndex: newinsertIndex,
            model: model,
            bloadSourceJournalChange: sourceJournalProfileUI.loadChangedSourceJournal
        }
    },

    /**
     * @name addNewSourceJournalLine
     * @description Add a new source journal line
     * @namespace sourceJournalProfileUtility
     * @public
     */
    addNewSourceJournalLine: () => {
        const MAXLIMIT = 50;
        let newSourceJournalExist = false;
        let sournceJournalModel = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList;
        if (sournceJournalModel.TotalResultsCount() === MAXLIMIT) {
            let limit50 = jQuery.validator.format(window.sourceJournalProfileResources.Limit50, window.sourceJournalProfileResources.SourceCode);
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, limit50);
            return;

        }
        sourceJournalProfileGrid.addLineClicked = true;
        //Do not allow user to allow a add line when a new empty line already exists
        if (sournceJournalModel.Items() && sournceJournalModel.Items().length > 0) {
            $.each(sournceJournalModel.Items(), (index, item) => {
                if ((item.Source() === null || item.Source() === "") && item.IsDeleted() !== true) {
                    newSourceJournalExist = true;
                    return;
                }
                sourceJournalProfileUI.dataIndex = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount() + 1;
            });

        } else {
            sourceJournalProfileUI.dataIndex = 1;
        }

        let grid = $('#SourceCodeGrid').data("kendoGrid");
        let currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        sourceJournalProfileUI.insertedIndex = grid.dataSource.indexOf(currentRowGrid);
        if ((!newSourceJournalExist)) {
            let isCurCreditClientSideAdditon =
                sg.utls.kndoUI.addLine(sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList, "SourceCodeGrid", sourceJournalProfileUtility.newSourceJournalLineItem, sourceJournalProfileUtility.getSourceJournalParamPaging, sourceJournalProfileUtility.currentPageNumber);
            if (isCurCreditClientSideAdditon) {
                let currCreditResultCount = sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount();
                sourceJournalProfileUI.sourceJournalModel.Data.SourceCodeList.TotalResultsCount(currCreditResultCount + 1);
            }
        }
        sg.controls.enable("#selectAllChk");
    },

    /**
     * @name deleteSourceJournalLine
     * @description Delete a source journal line
     * @namespace sourceJournalProfileUtility
     * @public
     */
    deleteSourceJournalLine: () => {
        sourceJournalProfileGrid.deleteLine();
    },

    /**
     * @name checkDuplicateRecord
     * @description Check for the existence of a grid row
     * @namespace sourceJournalProfileUtility
     * @public
     *  
     * @param {object} row The row object
     */
    checkDuplicateRecord: (row) => {
        let grid = sourceJournalProfileUtility.fetchSourceJournalGrid();
        let dataSource = grid.dataSource;
        let count = 0;
        // For uncached records
        if (dataSource.total() <= 10) {
            $.each(dataSource.data(), (key) => {
                if (dataSource.data()[key]["Source"] === row.Source) {
                    count = count + 1;
                }
            });

            if (count > 1) {
                let errorMsg = $.validator.format(sourceJournalProfileResources.RecordExist, sourceJournalProfileResources.SourceJournalProfile, row.Source);
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
                sourceJournalProfileUISuccess.clearGridRowData();
            }
        }
        else {
            // For cached records
            sourceJournalRepository.isExist(row.Source, sourceJournalProfileUI.sourceJournalModel.Data.SourceJournalName());
        }
    },
};

$(() => {
    sourceJournalProfileUI.init();
});
