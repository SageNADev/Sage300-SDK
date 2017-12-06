/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";

var screenUI = screenUI || {};
var updateTarget = updateTarget || { CurrencyCode: 1, ThroughDate: 2, RateType: 4, RateDate: 8, ExchangeRate: 16, All: 0xFFFFFFFF },

screenUI = {

    abortPollRequest: false,
    tokenId: null,
    koBound: false,

    viewModel: {},
    processViewModel: {},

    optionalFieldPopUpClose: true,

    init: function (model, processModel) {

        screenUI.initButtons();

        if (!screenUI.koBound) {
            screenUI.viewModel = ko.mapping.fromJS(model);
            screenUI.processViewModel = ko.mapping.fromJS(processModel);
            extendViewModel(screenUI.viewModel);
            screenUI.viewModel.dirtyFlag = new ko.dirtyFlag(screenUI.viewModel);
            screenUI.viewModel.dirtyFlag.reset();

            window.ko.applyBindings(screenUI.viewModel, document.getElementById('mainScreenContent'));
            screenUI.initOptionalFields(true);            

            //When initializing the progress UI, the ko.applyBinding must be called before initializing kendo window (performed in initProcessUI). 
            //Otherwise model binding for progress window will not work, becuase initialization of kendo window moves the html markup of the window from original location in the DOM.
            //However, calling the applybinding first requires progressUI.progressUIModel to be not null. 
            //So, assign progressUI.progressUIModel the default viewModel.
            progressUI.progressUIModel = screenUI.processViewModel;
            window.ko.applyBindings(screenUI.processViewModel, document.getElementById('processScreenContent'));
            screenUI.initProcessUI();

            var grid = $("#Grid").data("kendoGrid");
            var optionalFields = ['OptionalFieldsString'];
            if (screenUI.viewModel.IsOptionalFieldsLicenseAvailable()) {
                sg.utls.kndoUI.showGridColumns(grid, optionalFields);

            } else {
                sg.utls.kndoUI.hideGridColumns(grid, optionalFields);
            }

            screenUI.koBound = true;
        }
        else {
            ko.mapping.fromJS(model, screenUI.viewModel);
            extendViewModel(screenUI.viewModel);
            screenUI.viewModel.dirtyFlag.reset();
        }

        GridPreferencesHelper.setGrid("#Grid", screenSetup.detailsGridPreferences);
    },
    initButtons: function () {
        $("#btnProcess").click(function (e) {
            if (sg.utls.isProcessRunning) {
                return;
            }

            $('#statusWindow #btnCancel').hide();
            sg.utls.isProcessRunning = true;
            var url = window.sg.utls.url.buildUrl(screenSetup.moduleName, "RevaluationProcess", "Process");
            var data = { model: window.ko.mapping.toJS(screenUI.processViewModel) };
            window.sg.utls.ajaxPost(url, data, function (jsonResult) {
                screenUI.abortPollRequest = false;
                ko.mapping.fromJS(jsonResult.WorkflowInstanceId, {}, screenUI.processViewModel.WorkflowInstanceId);
                window.progressUI.progress();

                gridUtility.refreshData();
            });
        });
        $("#btnHistory").on('click', function () {
            var grid = $("#Grid").data("kendoGrid");
            var selectedItem = grid.dataItem(grid.select());
            
            var currencyQueryStringParam = selectedItem && selectedItem.CurrencyCode ? '?currencyCode=' + selectedItem.CurrencyCode : '';
            var revaluationHistoryUrl = sg.utls.url.buildUrl(screenSetup.moduleName, "RevaluationHistory", "Index") + currencyQueryStringParam;
            sg.utls.iFrameHelper.openWindow("revaluationHistoryPopUp", "", revaluationHistoryUrl);
            sg.utls.iFrameHelper.registerToReceiveMessage(function (e) {
                try {
                    if (e !== null && e.data.Type === 'SageKendoiFrame' && e.data.Id === 'CurrencyCode' && e.data.Data.CurrencyCode !== null) {
                        var currencyCode = e.data.Data.CurrencyCode;
                        if (currencyCode !== null) {
                            var tableRow = $("td").filter(function () {
                                return $(this).text() == currencyCode;
                            }).closest("tr");

                            grid.select(tableRow);
                        }
                    }
                }
                catch (err) {
                }
            });
        });
        $('#btnAddLine').click(function (e) {
            sg.utls.SyncExecute(gridUtility.addNewLine);
        });
        $('#btnRefresh').click(function (e) {
            sg.utls.SyncExecute(gridUtility.refreshData);
        });
        $('#btnDeleteLine').click(function (e) {

            var confirmationMsg = $('.selectChk:checked').length > 1 ?
                localResources.deleteLinesMessage :
                localResources.deleteLineMessage;

            sg.utls.showKendoConfirmationDialog(
                //Yes
                function () {
                    sg.utls.SyncExecute(gridUtility.deleteSelectedLines);
                },
                //No
                $.noop,
                confirmationMsg);
        });
        $('#btnEditColumns').on('click', function () {
            GridPreferences.initialize('#Grid', screenSetup.detailsGridPreferenceKey, $(this), gridUtility.columns);
        });
    },
    initProcessUI: function () {
        var progressUrl = window.sg.utls.url.buildUrl(screenSetup.moduleName, "RevaluationProcess", "Progress");
        window.progressUI.isMultipleProcessInvolved = true;
        window.progressUI.init(progressUrl, null, screenUI.processViewModel, screenSetup.screenTitle, function (result) {
            gridUtility.refreshData();
        });
    },
    initOptionalFields: function (initialize) {
        var params = {
            gridId: "OptFieldsGrid",
            isDefault: false,
            preferencesTypeId: screenSetup.detailsOptionalFieldsGridPreferenceKey,
            btnEditColumnsId: "btnEditOptFieldColumns",
            finder: screenSetup.optionalFieldsFinderType,
            modelData: screenUI.viewModel,
            modelName: "OptionalFieldsData",
            newLineItem: function () {
                var grid = $('#Grid').data("kendoGrid");
                var selectedRow = sg.utls.kndoUI.getSelectedRowData(grid);
                var newOptFieldLine = {
                    "CurrencyCode": selectedRow.CurrencyCode
                };
                return newOptFieldLine;
            },
            isValueSetEditable: false,
            optionalFieldFilter: function () {
                var filters = [[]];
                filters[0][1] = sg.finderHelper.createFilter("Location", sg.finderOperator.Equal, 6 /* Revaluation */);
                filters[0][1].IsMandatory = true;
                filters[0][0] = sg.finderHelper.createFilter("OptionalField", sg.finderOperator.StartsWith, optionalFieldUIGrid.optionalFieldFilterData);
                return filters;
            },
            getOptionalFieldData: function (optionalField) {
                var data = { 'optField': optionalField.OptionalField };
                window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "GetOptionalFieldData"), data, function (rowData) {

                    if (rowData.length > 0)
                        optionalFieldUIGrid.OnOptionalFieldSelection(rowData[0]);

                });
            },
            deleteUrl: sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "DeleteOptionalFields"),
            getOptionalFieldValue: function () {
            },
            //optionalFieldFilterName: "GLAccount",
            isCheckDuplicateRecord: true,
            saveOptionalField: function (model) {

                var data = {
                    model: ko.mapping.toJS(model)
                };
                sg.utls.ajaxPost(sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "SaveOptionalFields"), data,
                    function (result) {
                        if (result && result.UserMessage === undefined) {

                           gridUtility.beginRowUpdate();
                           gridUtility.selectedRow.set('ETag', result.ETag);
                           gridUtility.selectedRow.set('OptionalFieldsString', result.OptionalFieldsString);
                           gridUtility.selectedRow.set('HasChanged', false);
                           gridUtility.endRowUpdate();

                           screenUI.optionalFieldPopUpClose = true;
                           ko.mapping.fromJS([], {}, screenUI.viewModel.OptionalFieldsData);
                           $("#optionalFieldsWindow").data("kendoWindow").close();

                        } else if (result) {
                           sg.utls.showMessagePopup(result, "#windowmessage");
                        }                       
                    });
            },
            isOptionalFieldExists: function(model) {
              
                var data = {
                    optField: ko.mapping.toJS(model.OptionalField)
                };
                sg.utls.ajaxPost(sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "IsOptionalFieldExists"), data, function (result) {

                    if (result) {

                        var grid = $('#OptFieldsGrid').data("kendoGrid");
                        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "SerialNumber", optionalFieldUIGrid.serialNumber);
                        var message = $.validator.format(localResources.duplicateMessage, localResources.optionalField, currentRowGrid.OptionalField);
                        currentRowGrid.set("OptionalField", null);
                        currentRowGrid.set("OptionalFieldDescription", null);
                        optionalFieldUIGrid.showMessage(message);
                        optionalFieldUIGrid.resetFocus(currentRowGrid, 'OptionalField');
                    }
                    else {
                        sg.controls.enable("#btnAddOptionalFieldLine");
                    }

                });
                  
            },
            isPopUp: true
        };
        optionalFieldUIGrid.init(params, initialize);
        sg.utls.intializeKendoWindowPopup('#optionalFieldsWindow', localResources.optionalFields, function (e) {

           if (!screenUI.optionalFieldPopUpClose) {
               e.preventDefault();
               $("#windowmessage").empty();

               var hasModified = optionalFieldUIGrid.save();
               if (!hasModified) {
                   screenUI.optionalFieldPopUpClose = true;
                   $("#optionalFieldsWindow").data("kendoWindow").close();
               }
            } else {
               screenUI.optionalFieldPopUpClose = false;
            }
        });
    },
};

var gridConfigHeader = {
    CurrencyCodeTitle: $(gridConfigHeader.headerCurrencyCode).text(),
    CurrencyCodeHidden: $(gridConfigHeader.headerCurrencyCode).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
    ThroughDateTitle: $(gridConfigHeader.headerThroughDate).text(),
    ThroughDateHidden: $(gridConfigHeader.headerThroughDate).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
    RateTypeTitle: $(gridConfigHeader.headerRateType).text(),
    RateTypeHidden: $(gridConfigHeader.headerRateType).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
    RateDateTitle: $(gridConfigHeader.headerRateDate).text(),
    RateDateHidden: $(gridConfigHeader.headerRateDate).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
    ExchangeRateTitle: $(gridConfigHeader.headerExchangeRate).text(),
    ExchangeRateHidden: $(gridConfigHeader.headerExchangeRate).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
    OptionalFieldsStringTitle: $(gridConfigHeader.headerOptionalFieldsString).text(),
    OptionalFieldsStringHidden: $(gridConfigHeader.headerOptionalFieldsString).attr('hidden') ? $(gridConfigHeader.headerCurrencyCode).attr('hidden') : false,
};

var gridUtility = {

    //defines what field need to be udpated when a certain field changes
    updateTargetMap: {
        CurrencyCode: updateTarget.CurrencyCode,
        ThroughDate: updateTarget.CurrencyCode | updateTarget.ThroughDate,
        RateType: updateTarget.CurrencyCode | updateTarget.ThroughDate | updateTarget.RateType,
        RateDate: updateTarget.CurrencyCode | updateTarget.ThroughDate | updateTarget.RateType | updateTarget.RateDate,
        All: updateTarget.CurrencyCode | updateTarget.ThroughDate | updateTarget.RateType | updateTarget.RateDate | updateTarget.ExchangeRate
    },

    //flag to ignore dataChange and change events.
    //introduced in order to prevent multiple grid refreshes when using set method on grid line models
    ignoreChangeEvents: false,
    //currently selected row, updated from grid's change event
    selectedRow: {},
    //used together with endRowUpdate to keep row selection in the grid, while updating row data
    beginRowUpdate: function () {
        gridUtility.ignoreChangeEvents = true;
    },
    //restores selected row
    endRowUpdate: function () {
        if (gridUtility.selectedRow)
        {
            var grid = $("#Grid").data("kendoGrid");            
            grid.select(sg.utls.kndoUI.getRowForDataItem(gridUtility.selectedRow));
        }
        gridUtility.ignoreChangeEvents = false;
    },
    //refreshes grid content from the server
    refreshData: function () {
        var grid = $('#Grid').data("kendoGrid");
        grid.dataSource.read();
    },
    //deletes selected lines
    deleteSelectedLines: function(){
        var grid = $('#Grid').data("kendoGrid");
        var dataItemsToDelete = [];

        grid.tbody.find(":checked").closest("tr").each(function () {
            var dataItem = grid.dataItem($(this));
            //if line is new, delete it without going to server otherwise store it in the array
            if (!dataItem.IsNewLine) {
                dataItemsToDelete.push(dataItem);
            }
            else {
                grid.removeRow($(this));
            }
        });

        //Uncheck the selectAllChk checkbox if all rows were deleted
        if (grid.tbody.find(":checked").closest("tr").length === 0) {
            $('#selectAllChk').prop("checked", false).applyCheckboxStyle();;
        }

        //send lines to be deleted to the server
        if (dataItemsToDelete.length > 0) {
            var url = sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "Delete");
            sg.utls.ajaxPost(url, dataItemsToDelete, function (result) {
                if (!result.UserMessage.IsSuccess) {
                    sg.utls.showMessage(result);
                }
                else {
                    grid.dataSource.page(0);
                }
            });
        }
    },
    //adds a new line
    addNewLine: function() {
        var grid = $('#Grid').data("kendoGrid");
        var url = sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "Create");
        sg.utls.ajaxPost(url, null, function (result) {

            if (result && result.UserMessage === undefined) {
                screenUI.viewModel.DataList.push(ko.mapping.fromJS(result));
                var newRow = grid.dataSource.add(result);

                var newTableRow = sg.utls.kndoUI.getRowForDataItem(newRow);
                if (newTableRow)
                    grid.select(newTableRow);
            }
            else if (result) {
                sg.utls.showMessagePopup(result, "#windowmessage");
            }
        });
    },
    //indicates that a line is being saved. 
    //introduced in order to prevent multiple updates originated from dataChange and change events, 
    //or in other words when cell value changes or when selected row changes
    saveLineInProgress: false,
    //saves line data
    saveLine: function(dataItem, updateTarget, callback)
    {
        if (!dataItem.IsNewLine && !dataItem.HasChanged)
            return;

        if (gridUtility.saveLineInProgress)
            return;

        gridUtility.saveLineInProgress = true;

        var grid = $('#Grid').data("kendoGrid");
        var url = sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "Update");
        var payload = {
            header: dataItem,
            updateTarget: updateTarget
        };

        sg.utls.ajaxPost(url, payload, function (result) {

            if (result.UserMessage.IsSuccess) {

                $('#message').hide();
                gridUtility.beginRowUpdate();

                dataItem.set('CurrencyCode', result.Data.CurrencyCode);
                dataItem.set('RateType', result.Data.RateType);
                dataItem.set('ExchangeRate', result.Data.ExchangeRate);
                dataItem.set('ETag', result.Data.ETag);
                dataItem.set('OptionalFieldsString', result.Data.OptionalFieldsString);
                dataItem.set('IsNewLine', result.Data.IsNewLine);
                dataItem.set('HasChanged', result.Data.HasChanged);
                dataItem.set('IsDeleted', result.Data.IsDeleted);

                gridUtility.endRowUpdate();
            }
            else {
                sg.utls.showMessage(result);
            }

            gridUtility.saveLineInProgress = false;

            if (typeof (callback) == "function")
                callback(result);
        });
    },
    //prepares data for databinding to the grid
    buildGridData: function (data) {

        ko.mapping.fromJS(data.Items, {}, screenUI.viewModel.DataList);

        var gridData = {};
        gridData.data = data.Items;
        gridData.totalResultsCount = data.TotalResultsCount;

        return gridData;
    },
    //called when grid's data source changes - data item is inserted, updated or removed
    dataChange: function (changedData) {

        if (changedData.columnName && changedData.columnName !== 'ExchangeRate')
        {
            if (gridUtility.ignoreChangeEvents)
                return;

            var grid = $('#Grid').data("kendoGrid");
            var dataItem = sg.utls.kndoUI.getSelectedRowData(grid);
            
            gridUtility.saveLine(dataItem, gridUtility.updateTargetMap[changedData.columnName]);
        }
    },
    //called when grid's active cell changes
    change: function (e) {

        if (gridUtility.ignoreChangeEvents)
            return;

        var grid = $("#Grid").data("kendoGrid");
        var prevSelectedRow = gridUtility.selectedRow;
        gridUtility.selectedRow = grid.dataItem(grid.select());

        if (prevSelectedRow && gridUtility.selectedRow && prevSelectedRow.InternalId != gridUtility.selectedRow.InternalId)
        {
            if (prevSelectedRow.HasChanged)
            {                
                var rowExists = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), 'InternalId', prevSelectedRow.InternalId) != null;
                if (rowExists)
                {
                    gridUtility.saveLine(prevSelectedRow, gridUtility.updateTargetMap.All);
                }
            }
        }
    },
    //called after the data has been bound to the grid
    afterDataBind: function (e) {
        gridUtility.change();

        sg.controls.disable('#btnDeleteLine');
        $('#selectAllChk').prop("checked", false).applyCheckboxStyle();
    },
    //to see if there is unsaved data in the grid
    hasChanges: function()
    {
        var grid = $('#Grid').data("kendoGrid");
        var result = false;

        $.each(grid.dataSource.data(), function (index, value) {
            if (value.IsNewLine || value.HasChanged) {
                result = true;
                return;
            }
        });

        return result;
    },
    //checkbox multiselect logic for Delete column
    multiSelectInit: function (gridId, selectAllChk, selectChk, btnDeleteId) {
        if ($("#" + gridId)) {
            $(document).on("change", "#" + selectAllChk, function () {

                var grid = $('#' + gridId).data("kendoGrid");
                var checkbox = $(this);
                var rows = grid.tbody.find("tr");
                rows.find("td:first input")
                    .prop("checked", checkbox.is(":checked")).applyCheckboxStyle();
                if ($("#" + selectAllChk).is(":checked")) {
                    rows.addClass("k-state-active");
                    sg.controls.enable("#" + btnDeleteId);
                } else {
                    rows.removeClass("k-state-active");
                    sg.controls.disable("#" + btnDeleteId);
                }
            });
            $(document).on("change", "." + selectChk, function () {
                $(this).closest("tr").toggleClass("k-state-active");
                var grid = $('#' + gridId).data("kendoGrid");
                var allChecked = true;
                var hasChecked = false;
                grid.tbody.find("." + selectChk).each(function () {
                    if (!($(this).is(':checked'))) {
                        $("#" + selectAllChk).prop("checked", false).applyCheckboxStyle();
                        allChecked = false;
                        return;
                    } else {
                        hasChecked = true;
                    }
                });
                if (allChecked) {
                    $("#" + selectAllChk).prop("checked", true).applyCheckboxStyle();
                }

                if (hasChecked) {
                    sg.controls.enable("#" + btnDeleteId);
                } else {
                    sg.controls.disable("#" + btnDeleteId);
                }
            });
        }
    },
    //grdi columns definition
    columns: [{
        field: 'Delete',
        attributes: { 'class': 'first-cell', sg_Customizable: false },
        headerAttributes: { 'class': 'first-cell' },
        template: sg.controls.ApplyCheckboxStyle("<input type='checkbox' class='selectChk' />"),
        headerTemplate: sg.controls.ApplyCheckboxStyle("<input type='checkbox' id='selectAllChk' />"),
        reorderable: false,
        editor: $.noop
    }, /*{
        field: 'HasChanged',
        attributes: { 'class': 'first-cell', sg_Customizable: false },
        headerAttributes: { 'class': 'first-cell' },
        editor: $.noop
    }, {
        field: 'InternalId',
        attributes: { 'class': 'first-cell', sg_Customizable: false },
        headerAttributes: { 'class': 'first-cell' },
        editor: $.noop
    },*/ {
        field: 'CurrencyCode',
        hidden: gridConfigHeader.CurrencyCodeHidden,
        title: gridConfigHeader.CurrencyCodeTitle,
        attributes: { 'class': 'w140 txt-upper' },
        headerAttributes: { 'class': 'w140 ' },
        editor: function (container, options) {
            var grid = $('#Grid').data("kendoGrid");
            var data = sg.utls.kndoUI.getSelectedRowData(grid);
            if (!data.IsNewLine)
            {
                sg.utls.kndoUI.nonEditable(grid, container);
                return;
            }
            
            var html = gridFields.txtCurrencyCode;
            $(html).appendTo(container);

            sg.finderHelper.setFinder("btnCurrencyCode", sg.finder.CurrencyCode,

                function (rowData, key) {

                    gridUtility.beginRowUpdate();
                    data.set("CurrencyCode", rowData.CurrencyCodeId);
                    gridUtility.endRowUpdate();
                    gridUtility.saveLine(data, gridUtility.updateTargetMap.CurrencyCode);
                },
                $.noop,
                localResources.currencyCodeFinderTitle,
                sg.finderHelper.createDefaultFunction("txtCurrency", "CurrencyCode", sg.finderOperator.StartsWith));
        }
    }, {
        field: 'ThroughDate',
        hidden: gridConfigHeader.ThroughDateHidden,
        title: gridConfigHeader.ThroughDateTitle,
        attributes: { 'class': 'w140' },
        headerAttributes: { 'class': 'w140' },
        template: sg.utls.kndoUI.getDateTemplate("ThroughDate"),
        editor: function (container, options) {
            var html = gridFields.txtThroughDate;
            $(html).appendTo(container).addClass("plusFinder");
            sg.utls.kndoUI.datePicker('txtThroughDate'); 
        }
    }, {
        field: 'RateType',
        hidden: gridConfigHeader.RateTypeHidden,
        title: gridConfigHeader.RateTypeTitle,
        attributes: { 'class' : 'w140 txt-upper' },
        headerAttributes: { 'class': 'w140 ' },
        editor: function (container, options) {

            var html = gridFields.txtRateType;
            $(html).appendTo(container);
            sg.finderHelper.setFinder("btnRateType",
                sg.finder.CurrencyRateType,
                function (rowData, key) {
                    var grid = $('#Grid').data("kendoGrid");
                    var data = sg.utls.kndoUI.getSelectedRowData(grid);

                    gridUtility.beginRowUpdate();
                    data.set("RateType", rowData.RateType);
                    gridUtility.endRowUpdate();
                    gridUtility.saveLine(data, gridUtility.updateTargetMap.RateType);
                },
                $.noop,
                localResources.rateTypeFinderTitle,
                sg.finderHelper.createDefaultFunction("btnRateType", "RateType"));
        }
    }, {
        field: 'RateDate',
        hidden: gridConfigHeader.RateDateHidden,
        title: gridConfigHeader.RateDateTitle,
        attributes: { 'class': 'w140  ' },
        headerAttributes: { 'class': 'w140 ' },
        template: sg.utls.kndoUI.getDateTemplate("RateDate"),
        editor: function (container, options) {
            var html = gridFields.txtRateDate;
            $(html).appendTo(container).addClass("plusFinder");
            sg.utls.kndoUI.datePicker('txtRateDate');
        }
    }, {
        field: 'ExchangeRate',
        hidden: gridConfigHeader.ExchangeRateHidden,
        title: gridConfigHeader.ExchangeRateTitle,
        attributes: { 'class': 'w140  ' },
        headerAttributes: { 'class': 'w140 ' },
        editor: function (container, options) {

            var originalValue = options.model.ExchangeRate;
            var originalHasChanged = options.model.HasChanged;

            var html = gridFields.txtExchangeRate;
            $(html).appendTo(container).kendoNumericTextBox({
                format: "n7",
                spinners: false,
                decimals: 7,
                change: function (e) {

                    //reset HasChanged flag to prevent line data from being saved
                    //this is in order to give the user an option to revert to the previous value if Rate spread exceeds allowed Maximum.
                    options.model.HasChanged = false;

                    if (!options.model.CurrencyCode || options.model.CurrencyCode === '' || !options.model.RateType || options.model.RateType === '')
                        return;

                    var payload = {
                        sourceCurrencyCode: options.model.CurrencyCode,
                        rateType: options.model.RateType,
                        time: options.model.RateDate
                    };
                    window.sg.utls.ajaxPost(sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "GetCurrencyRateComposite"), payload, function (result) {

                        if (result && result.UserMessage === undefined) {

                            //check if Rate spread exceeds allowed Maximum
                            var rateSpreadExceeded = Math.abs(options.model.ExchangeRate - result.Rate) > result.Spread;
                            if (rateSpreadExceeded) {
                                //ask user if they want to proceed
                                sg.utls.showKendoConfirmationDialog(
                                //if Yes, update the line
                                function () {
                                    options.model.HasChanged = true;
                                    gridUtility.saveLine(options.model, gridUtility.updateTargetMap.All);
                              },
                                //if No, revert to the previous value
                                function () {
                                    gridUtility.beginRowUpdate();
                                    options.model.set('ExchangeRate', originalValue);
                                    gridUtility.endRowUpdate();
                                    gridUtility.saveLine(options.model, gridUtility.updateTargetMap.All);
                                },
                                localResources.currencySpreadRangeExceeded);
                            }
                            else {
                                options.model.HasChanged = true;
                                gridUtility.saveLine(options.model, gridUtility.updateTargetMap.All);
                            }
                        }
                        else if (!result) {
                            //Even if there is no RateComposite object available, still save the changes
                            options.model.HasChanged = true;
                            gridUtility.saveLine(options.model, gridUtility.updateTargetMap.All);
                        }
                    });                    
                }
            });

            sg.finderHelper.setFinder("btnExchangeRate",
                sg.finder.CurrencyRate,
                function (rowData, key) {

                    gridUtility.beginRowUpdate();
                    options.model.set("RateDate", rowData.RateDate);
                    options.model.set("ExchangeRate", rowData.Rate);
                    gridUtility.endRowUpdate();

                    gridUtility.saveLine(options.model, gridUtility.updateTargetMap.All);
                },
                $.noop,
                localResources.exchangeRateFinderTitle,
                function () {

                    var filters = [[]];
                    filters[0][0] = sg.finderHelper.createFilter("RateType", sg.finderOperator.Equal, options.model.RateType);
                    filters[0][0].IsMandatory = true;
                    filters[0][1] = sg.finderHelper.createFilter("ToCurrency", sg.finderOperator.Equal, screenUI.viewModel.HomeCurrency());
                    filters[0][1].IsMandatory = true;
                    filters[0][2] = sg.finderHelper.createFilter("FromCurrency", sg.finderOperator.Equal, options.model.CurrencyCode);
                    filters[0][2].IsMandatory = true;
                    filters[0][3] = sg.finderHelper.createFilter("RateDate", sg.finderOperator.GreaterThanOrEqual, sg.utls.kndoUI.getFormattedDate(options.model.RateDate));
                    filters[0][3].IsMandatory = true;
                    return filters;
                }
            );
        }
    }, {
        field: 'OptionalFieldsString',
        hidden: gridConfigHeader.OptionalFieldsStringHidden,
        title: gridConfigHeader.OptionalFieldsStringTitle,
        attributes: { 'class': 'w140  ' },
        headerAttributes: { 'class': 'w140 ' },
        editor: function (container, options) {

            var grid = $('#Grid').data("kendoGrid");
            var selectedRow = sg.utls.kndoUI.getSelectedRowData(grid);
            if (selectedRow.IsNewLine)
            {
                sg.utls.kndoUI.nonEditable(grid, container);
                return;
            }

            var html = gridFields.txtOptionalFieldsString;
            $(html).appendTo(container);
            $("#btnOptionalFieldsString").on("click", function () {

                $("#windowmessage").empty();
                screenUI.optionalFieldPopUpClose = false;

                window.sg.utls.ajaxPost(sg.utls.url.buildUrl(screenSetup.moduleName, "Revaluation", "SetHeader"), { header: selectedRow }, function (result) {

                    sg.utls.openKendoWindowPopup('#optionalFieldsWindow', null);

                    var optFieldsGrid = $('#OptFieldsGrid').data("kendoGrid");
                    optFieldsGrid.dataSource.data([]);
                    optFieldsGrid.dataSource.page(1);

                });

            });
        }
    }],
    schema: {
        model: {
            fields: {
                Delete: { editable: false }
            }
        }
    }
};

var gridConfig = {
    pageSize: sg.utls.gridPageSize,
    pageable: {
        input: true,
        numeric: false
    },
    scrollable: true,
    navigatable: true, //enable grid cell tabbing
    resizable: true,
    selectable: true,
    reorderable: sg.utls.reorderable,
    //Turns server side paging on
    isServerPaging: true,
    //Param will be null during Get and will contain the data that needs to be passed to the server on create
    param: null,
    getParam: function () {
        var grid = $('#Grid').data("kendoGrid");

        var pageNumber = grid.dataSource.page();
        var pageSize = grid.dataSource.pageSize();
        var parameters = {
            pageNumber: pageNumber - 1,
            pageSize: pageSize,
            model: ko.mapping.toJS(screenUI.viewModel.DataList)
        }
        return parameters;
    },
    //URL to get the data from the server. 
    pageUrl: sg.utls.url.buildUrl(screenSetup.moduleName, 'Revaluation', 'Get'),
    //Call back function when Get is successfull. In this, the data for the grid and the total results count are to be set along with updating knockout
    buildGridData: gridUtility.buildGridData,
    //Call back function after data is bound to the grid. Is used to set the added line as editable
    afterDataBind: gridUtility.afterDataBind,
    columns: gridUtility.columns,
    editable: {
        mode: "incell",
        confirmation: false,
        createAt: "top"
    },
    columnReorder: function (e) {
        GridPreferencesHelper.saveColumnOrder(e, '#Grid', screenSetup.detailsGridPreferenceKey);
    },
    schema: gridUtility.schema,
    // Called when the data changes in a cell. 
    dataChange: gridUtility.dataChange,
    change: gridUtility.change,
    edit: function (e) {
        $('#Grid').data("kendoGrid").select(e.container.closest("tr"));
    }
}

$(function () {

    gridUtility.multiSelectInit("Grid", "selectAllChk", "selectChk", "btnDeleteLine");

    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && gridUtility.hasChanges()) {

            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, localResources.entityName)).text();
        }
    });

    $(window).bind('unload', function () {
        if (globalResource.AllowPageUnloadEvent) {
            sg.utls.destroySession();
        }
    });

    screenUI.init(initialModel, initialProcessModel);
})

