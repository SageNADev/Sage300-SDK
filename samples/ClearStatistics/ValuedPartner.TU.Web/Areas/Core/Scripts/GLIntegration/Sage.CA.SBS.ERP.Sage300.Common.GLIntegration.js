/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";
var glIntegrationKoExtn = glIntegrationKoExtn || {};
var glIntegrationUI = glIntegrationUI || {};
var glIntegrationUtils = glIntegrationUtils || {};
var glIntegrationOnSuccess = glIntegrationOnSuccess || {};

var glReferenceIntegrationFields = [
    { fieldName: "SourceTransactionType", fieldIndex: 0, enumValue: -1 },
    { fieldName: "GLEntryDescription.Example", fieldIndex: 1, enumValue: 0 },
    { fieldName: "GLDetailReference.Example", fieldIndex: 2, enumValue: 1 },
    { fieldName: "GLDetailDescription.Example", fieldIndex: 3, enumValue: 2 },
    { fieldName: "GLDetailComment.Example", fieldIndex: 4, enumValue: 3 }
];

var GRID_GLINTEGRATION_INDEX = {
    TRANSACTION_TYPE: 0
};

var GLTransactionDetail = function () {
    this.koSourceTransactionType = ko.observable(-1);
    this.koSeparator = ko.observable(-1);
    this.koGLTransactionField = ko.observable(-1);
    this.koExample = ko.observable("");
    this.SourceTransactionTypeList = ko.observableArray();
};

var separators = [
    { Text: "*", Value: 0 },
    { Text: "-", Value: 1 },
    { Text: "/", Value: 2 },
    { Text: "\\", Value: 3 },
    { Text: ".", Value: 4 },
    { Text: "(", Value: 5 },
    { Text: ")", Value: 6 },
    { Text: "#", Value: 7 },
    { Text: " ", Value: 8 }
];

var isDirtyRequiredForIntegrationGrid = true;

glIntegrationUI = {
    hasKoApplied: false,
    isKendoControlInitialised: false,
    ignoreIsDirtyProperties: ["koSourceTransactionType", "koSeparator", "koGLTransactionField", "koExample", "koIsGLTransactionDetailDirty"],
    computedProperties: [],
    glIntegrationModel: {},
    includedSegment: {
        IncludedSegment1: 1,
        IncludedSegment2: 2,
        IncludedSegment3: 3,
        IncludedSegment4: 4,
        IncludedSegment5: 5
    },
    glTransactionField: {
        GLEntryDescription: 0,
        GLDetailReference: 1,
        GLDetailDescription: 2,
        GLDetailComment: 3
    },

    MAX_SEGMENT: 5,
    MAX_TRANSACTION_FIELD: 4,
    fromSegmentList: null,
    toSegmentList: null,
    sourceTransactionTypeList: null,
    segmentSeparatorTypeList: null,
    glTransactionFieldList: null,
    selectedGLReferenceDetail: null,
    glReferenceIntegrationList: null,
    initGLIntegration: function (model) {
        glIntegrationUI.initLabelText();
        glIntegrationUI.initButtons();
        // Load model data
        glIntegrationOnSuccess.loadGLIntegration(model);
        //init Controls after ko apply binding
        glIntegrationUI.initPopupWindow();
    },
    initLabelText: function () {
        //Set label text/title from localizaction
        $('#lblDeferGLTransactions').html(glIntegrationResources.lblDeferGLTransactionsText);
        $('#lblCreateGLTransactionsBy').html(glIntegrationResources.lblCreateGLTransactionsByText);
        $('#lblConsolidateGLTransactions').html(glIntegrationResources.lblConsolidateGLTransactionsText);
    },
    initDropdownList: function () {
        //initilize kendo Dropdown list
        var kendoUi = sg.utls.kndoUI;
        var fields = ["Data_DeferGLTransactions", "Data_CreateGLTransactionsBy", "Data_ConsolidateGLTransactions"]; // -> Option Tab
        $.each(fields, function (index, field) {
            kendoUi.dropDownList(field);
        });
    },
    initSourceDropDownList: function () {
        var kendoUi = sg.utls.kndoUI;
        kendoUi.dropDownList("Data_SourceTransactionType"); // init dropDownList
        var ddDataSourceTransactionType = $("#Data_SourceTransactionType").data("kendoDropDownList");
        ddDataSourceTransactionType.bind("select", function (e) {
            var data = this.dataItem(e.item.index());
            var oldValue = this.value();  // this will have the old selected value 
            var newValue = data.value; //this will have the new selected value
            if (parseInt(oldValue) !== parseInt(newValue)) {
                glIntegrationUI.onDropDownSelectChange(e);
            }
        });
    },
    initPopupDropdownList: function (selectedGLTransactionField) {
        //initilize kendo Dropdown list
        var kendoUi = sg.utls.kndoUI;
        var fields = ["Data_GLTransactionField", "Data_Separator"]; // -> GL Integration Details Popup Window
        $.each(fields, function (index, field) {
            kendoUi.dropDownList(field);
        });

        $("#Data_Separator").bind("change", function (e) {
            glIntegrationUtils.setExample();
            // set GLTransactionDetail Dirty
            glIntegrationUtils.setGLReferenceIntegrationDirty();
        });

        var ddDataGLTransactionField = $("#Data_GLTransactionField").data("kendoDropDownList");
        ddDataGLTransactionField.bind("select", function (e) {
            if (e.item) {
                var data = this.dataItem(e.item.index());
                var oldValue = this.value();  // this will have the old selected value 
                var newValue = data.value; //this will have the new selected value
                if (parseInt(oldValue) !== parseInt(newValue)) {
                    glIntegrationUI.onDropDownSelectChange(e);
                }
            } else {
                glIntegrationUI.onDropDownSelectChange(e);
            }

        });

        //set default value
        ddDataGLTransactionField.select(function (dataItem) {
            return parseInt(dataItem.value) === parseInt(selectedGLTransactionField);
        });
    },
    initKendoListView: function (listViewId, dataToBind) {
        var kendoListview = $("#" + listViewId).kendoListView({
            dataSource: dataToBind,
            selectable: "single",
            template: kendo.template($("#" + listViewId + "_Template").html()),
        }).data("kendoListView");
        return kendoListview;
    },
    intiListviewDataSource: function (dataList) {
        var dataSource = new kendo.data.DataSource({
            data: dataList
        });
        return dataSource;
    },
    initBrowserBackIsDirty: function () {
        $(window).bind('beforeunload', function () {
            if (glIntegrationUI.glIntegrationModel.isModelDirty.isDirty()) {
                return $('<div />').html($.validator.format(globalResource.SaveConfirm2, glIntegrationResources.Title)).text();
            }
        });
    },
    initPopupWindow: function () {
        //sg.utls.intializeKendoWindowPopup('#window', "GL Integration Details");
        //extend popup behaviours
        var popupWindow = $("#window").kendoWindow({
            modal: true,
            title: glIntegrationDetailResource.glIntegrationDetailTitle,
            resizable: false,
            draggable: true,
            scrollable: false,
            visible: false,
            width: "1020px",
            minHeight: 300,
            //custom function to suppot focus within kendo window
            activate: sg.utls.kndoUI.onActivate,
            close: function (e) {
                if (glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty() === true) {
                    e.preventDefault();
                    sg.utls.showKendoConfirmationDialog(
                        function () { // Yes
                            // reset GLTransactionDetail Dirty
                            glIntegrationUtils.resetGLReferenceIntegrationDirty();
                            $("#window").data("kendoWindow").close();
                        },
                        function () { // No
                            //do nothing
                        },
                        $.validator.format(glIntegrationDetailResource.saveConfirm, glIntegrationDetailResource.glIntegrationDetailTitle));
                }
            }
        }).data("kendoWindow");

        popupWindow.center();
    },
    initOpenPopup: function () {
        var grid = $("#gridGLReferenceIntegration").data("kendoGrid");
        if (grid.dataSource.length <= 0) {
            return false;
        }

        var selectedRow = $(this).closest("tr");  // Get selected row
        grid.select(selectedRow); // set selected
        var selectedCell = sg.utls.kndoUI.getCurrentCell(selectedRow); // Get selected cell
        if (selectedCell == undefined || selectedCell.index() < 0) {
            selectedCell = selectedRow.find('td:first'); // Get first cell
        }

        var selectedRowData = grid.dataItem(selectedRow); // Get the data item corresponding to this row

        if (selectedCell.index() >= 0) {
            var selectedColumnIndex = selectedCell.index();
            var selectedField = grid.columns[selectedColumnIndex].field;
            var selectedFieldName = glIntegrationUtils.getValidGLTransactionFieldName(selectedField, selectedRowData.GLEntryDescription.GLTransactionFields);

            if (selectedFieldName) {
                glIntegrationUI.preparePopupWindow(selectedFieldName, selectedRowData);
            }
            // else Could not find a valid cell in this row. exit
        }
    },
    initButtons: function () {
        $("#btnInclude").click(function (e) {
            glIntegrationUtils.moveSegmentItem(glIntegrationUI.fromSegmentList, glIntegrationUI.toSegmentList, true);
            // set G/L TransactionDetail Dirty
            glIntegrationUtils.setGLReferenceIntegrationDirty();
        });
        $("#btnExclude").click(function (e) {
            glIntegrationUtils.moveSegmentItem(glIntegrationUI.toSegmentList, glIntegrationUI.fromSegmentList, false);
            // set G/L TransactionDetail Dirty
            glIntegrationUtils.setGLReferenceIntegrationDirty();
        });
        $("#fromSegmentList").delegate("div", "dblclick", function (e) {
            glIntegrationUtils.moveSegmentItem(glIntegrationUI.fromSegmentList, glIntegrationUI.toSegmentList, true);
            // set G/L TransactionDetail Dirty
            glIntegrationUtils.setGLReferenceIntegrationDirty();
        });
        $("#toSegmentList").delegate("div", "dblclick", function (e) {
            glIntegrationUtils.moveSegmentItem(glIntegrationUI.toSegmentList, glIntegrationUI.fromSegmentList, false);
            // set G/L TransactionDetail Dirty
            glIntegrationUtils.setGLReferenceIntegrationDirty();
        });
        $("#btnDetailClose").bind('click', function (e) {
            $("#window").data("kendoWindow").close();
        });
        $("#btnDetailSave").bind('click', function (e) {

            var selectedSourceTransactionType = parseInt(glIntegrationUI.glIntegrationModel.Data.koSourceTransactionType());
            var selectedGLTransactionField = parseInt(glIntegrationUI.glIntegrationModel.Data.koGLTransactionField());
            var item = ko.utils.arrayFilter(glIntegrationUI.glIntegrationModel.ReferenceDetails.Items(), function (referenceIntegration) {
                return (referenceIntegration.SourceTransactionType() === selectedSourceTransactionType);
            });
            item = item[0];

            if (item == null) return; // if Reference Detail null the return

            var model;
            //get G/L Reference Integaration model
            switch (selectedGLTransactionField) {
                case glIntegrationUI.glTransactionField.GLEntryDescription:
                    model = item.GLEntryDescription;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailReference:
                    model = item.GLDetailReference;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailDescription:
                    model = item.GLDetailDescription;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailComment:
                    model = item.GLDetailComment;
                    break;
                default:
                    break;
            }

            if (model == null) return; // if model null the return
            model = ko.mapping.toJS(model);

            //update changes to the model
            model.Separator = parseInt(glIntegrationUI.glIntegrationModel.Data.koSeparator());
            model.Example = glIntegrationUI.glIntegrationModel.Data.koExample();

            model.IncludedSegment1Value = 0;
            model.IncludedSegment2Value = 0;
            model.IncludedSegment3Value = 0;
            model.IncludedSegment4Value = 0;
            model.IncludedSegment5Value = 0;

            for (var index = 0; index < glIntegrationUI.toSegmentList.dataSource.data().length; index++) {
                var segmentValue = glIntegrationUI.toSegmentList.dataSource.data()[index].SegmentValue();
                switch (index + 1) {
                    case glIntegrationUI.includedSegment.IncludedSegment1:
                        model.IncludedSegment1Value = segmentValue;
                        break;
                    case glIntegrationUI.includedSegment.IncludedSegment2:
                        model.IncludedSegment2Value = segmentValue;
                        break;
                    case glIntegrationUI.includedSegment.IncludedSegment3:
                        model.IncludedSegment3Value = segmentValue;
                        break;
                    case glIntegrationUI.includedSegment.IncludedSegment4:
                        model.IncludedSegment4Value = segmentValue;
                        break;
                    case glIntegrationUI.includedSegment.IncludedSegment5:
                        model.IncludedSegment5Value = segmentValue;
                        break;
                    default:
                        break;
                }
            }

            //send G/L Reference Integaration model to update/verify
            glIntegrationExtend.updateReferenceIntegration(model);
        });
    },
    preparePopupWindow: function (fieldName, selectedGridData) {
        var glTransactionFieldValue = 0;
        var glSourceTransactionTypeList = ko.mapping.toJS(selectedGridData.SourceTransactionTypeList);

        glTransactionFieldValue = glIntegrationUtils.getGLTransactionFieldByName(fieldName).enumValue;
        glIntegrationUI.glIntegrationModel.GetSourceTransactionType(glSourceTransactionTypeList);

        glIntegrationUI.setWindowData(selectedGridData.SourceTransactionType, glTransactionFieldValue);

        //display popup window
        glIntegrationUI.displayPopupWindow();
    },
    displayPopupWindow: function () {
        var popupWindow = $("#window");

        //init Source Transaction Type dropdown
        glIntegrationUI.initSourceDropDownList();

        // reset GLTransactionDetail Dirty to false
        glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty(false);

        //open popup-window
        popupWindow.data("kendoWindow").open().center().toFront();
    },
    setWindowData: function (sourceTransactionType, glTransactionField) {

        if (sourceTransactionType !== null || typeof sourceTransactionType !== "undefined" && glTransactionField !== null || typeof glTransactionField !== "undefined") {
            //Get ReferenceDetail based on sourceTransactionType and glTransactionField
            var detail = glIntegrationUtils.getReferenceDetail(sourceTransactionType, glTransactionField);
            if (detail.selectedItem == null) return;

            //clear if any error
            $("#windowmessage").empty();
            $("#windowmessage").hide();

            //get convert Objects to json data 
            var glTransactionFields = ko.mapping.toJS(detail.selectedItem.GLTransactionFields());
            var selectedField = glIntegrationUtils.getGLTransactionFieldByValue(glTransactionField);
            var validFieldName = glIntegrationUtils.getValidGLTransactionFieldName(selectedField.fieldName, glTransactionFields);

            if (selectedField.fieldName !== validFieldName) {
                // if selected Field not valid ,set next fieldValue
                var validField = glIntegrationUtils.getGLTransactionFieldByName(validFieldName);
                glIntegrationUI.setWindowData(sourceTransactionType, validField.enumValue);
                return;
            }

            //update value to ko-custom model
            glIntegrationUI.glIntegrationModel.Data.koSourceTransactionType(sourceTransactionType);
            glIntegrationUI.glIntegrationModel.Data.koGLTransactionField(glTransactionField);
            glIntegrationUI.glIntegrationModel.Data.koSeparator(detail.selectedItem.Separator());
            glIntegrationUI.glIntegrationModel.Data.koExample(detail.selectedItem.Example());

            //update dropdown list values
            glIntegrationUI.glIntegrationModel.GetGLTransactionField(glTransactionFields);

            //bind/rebind dropdown list
            glIntegrationUI.initPopupDropdownList(glTransactionField);



            //initializing from and to segment Listviews with data
            glIntegrationUI.fromSegmentList = glIntegrationUI.initKendoListView("fromSegmentList", glIntegrationUI.intiListviewDataSource(detail.selectedItem.SegmentList()));
            glIntegrationUI.toSegmentList = glIntegrationUI.initKendoListView("toSegmentList", glIntegrationUI.intiListviewDataSource(detail.selectedItem.SelectedSegment()));

            //looping all the available segment values of the currenct selected item, to move from fromList to toList.
            for (var i = 0; i <= glIntegrationUI.MAX_SEGMENT - 1; i++) {

                //getting IncludedSegment value for currenct index
                var includeSegmentValue = glIntegrationUtils.getIncludeSegmentValue(detail.selectedItem, i + 1);

                if (includeSegmentValue !== 0) {

                    for (var j = 0; j <= glIntegrationUI.fromSegmentList.dataSource.data().length - 1; j++) {

                        //checking for saved segment value in from list
                        if (glIntegrationUI.fromSegmentList.dataSource.data()[j].SegmentValue() === includeSegmentValue) {
                            glIntegrationUI.fromSegmentList.select(glIntegrationUI.fromSegmentList.element.children()[j]);

                            //moving selected segment value from fromList to toList.
                            glIntegrationUtils.moveSegmentItem(glIntegrationUI.fromSegmentList, glIntegrationUI.toSegmentList, true);
                            break;
                        }
                    }
                }
            }

            var fromLength = glIntegrationUI.fromSegmentList.dataSource.data().length;
            var toLength = glIntegrationUI.toSegmentList.dataSource.data().length;

            if (fromLength > 0) {
                glIntegrationUI.fromSegmentList.select(glIntegrationUI.fromSegmentList.element.children().first());
            }
            if (toLength > 0) {
                glIntegrationUI.toSegmentList.select(glIntegrationUI.toSegmentList.element.children().first());
            }
            var includeEnable = false;
            //checking whether tolist is having more than 5 items if contains then we should not alow to include any more items. 
            // var includeEnable = toLength >= glIntegrationUI.MAX_SEGMENT;

            // if from list empty then disable include
            // includeEnable = fromLength <= 0;

            if (toLength >= glIntegrationUI.MAX_SEGMENT || fromLength <= 0) {
                includeEnable = true;
            }

            //checking whether fromList having any record if no then have to disable Exclude
            var excludeEnable = toLength <= 0;

            sg.controls.enableDisable("#btnInclude", includeEnable);
            sg.controls.enableDisable("#btnExclude", excludeEnable);
            //reset dirty
            glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty(false);
        }
    },
    assignSegmentData: function (modifiedData) {

        for (var index = 0; index < glIntegrationUI.toSegmentList.dataSource.data().length; index++) {

            var segmentValue = glIntegrationUI.toSegmentList.dataSource.data()[index].SegmentValue();

            switch (index + 1) {
                case glIntegrationUI.includedSegment.IncludedSegment1:
                    modifiedData.IncludedSegment1 = segmentValue;
                    break;
                case glIntegrationUI.includedSegment.IncludedSegment2:
                    modifiedData.IncludedSegment2 = segmentValue;
                    break;
                case glIntegrationUI.includedSegment.IncludedSegment3:
                    modifiedData.IncludedSegment3 = segmentValue;
                    break;
                case glIntegrationUI.includedSegment.IncludedSegment4:
                    modifiedData.IncludedSegment4 = segmentValue;
                    break;
                case glIntegrationUI.includedSegment.IncludedSegment5:
                    modifiedData.IncludedSegment5 = segmentValue;
                    break;
                default:
                    break;
            }
        }

        return modifiedData;
    },
    setReferenceDetail: function (modifiedData) {
        //filter and update model
        var item = ko.utils.arrayFilter(glIntegrationUI.glIntegrationModel.ReferenceDetails.Items(), function (referenceIntegration) {
            return (referenceIntegration.SourceTransactionType() === modifiedData.SourceTransactionType);
        });
        item = item[0];

        if (item) {
            switch (modifiedData.GLTransactionField) {
                case glIntegrationUI.glTransactionField.GLEntryDescription:
                    item.GLEntryDescription.Example(modifiedData.Example);
                    item.GLEntryDescription.Separator(modifiedData.Seperator);
                    item.GLEntryDescription.IncludedSegment1Value(modifiedData.IncludedSegment1);
                    item.GLEntryDescription.IncludedSegment2Value(modifiedData.IncludedSegment2);
                    item.GLEntryDescription.IncludedSegment3Value(modifiedData.IncludedSegment3);
                    item.GLEntryDescription.IncludedSegment4Value(modifiedData.IncludedSegment4);
                    item.GLEntryDescription.IncludedSegment5Value(modifiedData.IncludedSegment5);
                    break;
                case glIntegrationUI.glTransactionField.GLDetailReference:
                    item.GLDetailReference.Example(modifiedData.Example);
                    item.GLDetailReference.Separator(modifiedData.Seperator);
                    item.GLDetailReference.IncludedSegment1Value(modifiedData.IncludedSegment1);
                    item.GLDetailReference.IncludedSegment2Value(modifiedData.IncludedSegment2);
                    item.GLDetailReference.IncludedSegment3Value(modifiedData.IncludedSegment3);
                    item.GLDetailReference.IncludedSegment4Value(modifiedData.IncludedSegment4);
                    item.GLDetailReference.IncludedSegment5Value(modifiedData.IncludedSegment5);
                    break;
                case glIntegrationUI.glTransactionField.GLDetailDescription:
                    item.GLDetailDescription.Example(modifiedData.Example);
                    item.GLDetailDescription.Separator(modifiedData.Seperator);
                    item.GLDetailDescription.IncludedSegment1Value(modifiedData.IncludedSegment1);
                    item.GLDetailDescription.IncludedSegment2Value(modifiedData.IncludedSegment2);
                    item.GLDetailDescription.IncludedSegment3Value(modifiedData.IncludedSegment3);
                    item.GLDetailDescription.IncludedSegment4Value(modifiedData.IncludedSegment4);
                    item.GLDetailDescription.IncludedSegment5Value(modifiedData.IncludedSegment5);
                    break;
                case glIntegrationUI.glTransactionField.GLDetailComment:
                    item.GLDetailComment.Example(modifiedData.Example);
                    item.GLDetailComment.Separator(modifiedData.Seperator);
                    item.GLDetailComment.IncludedSegment1Value(modifiedData.IncludedSegment1);
                    item.GLDetailComment.IncludedSegment2Value(modifiedData.IncludedSegment2);
                    item.GLDetailComment.IncludedSegment3Value(modifiedData.IncludedSegment3);
                    item.GLDetailComment.IncludedSegment4Value(modifiedData.IncludedSegment4);
                    item.GLDetailComment.IncludedSegment5Value(modifiedData.IncludedSegment5);
                    break;
                default:
                    break;
            }

        }
    },
    onDropDownSelectChange: function (e) {
        var ddList = e.sender; // get dropdown reference
        var oldValue = ddList.value();  // this will have the old selected value 
        window.setTimeout(function () {
            glIntegrationUtils.checkIsDetailDirty(ddList, oldValue);
        });
    }

};

glIntegrationOnSuccess = {
    updateReferenceIntegration: function (model) {
        if (model === null) return;
        if (model.UserMessage.IsSuccess) {
            //clear if any error
            $("#windowmessage").empty();
            $("#windowmessage").hide();

            var selectedSourceTransactionType = parseInt(model.Data.SourceTransactionTypeValue);
            var selectedGLTransactionField = parseInt(model.Data.GLTransactionField);
            var item = ko.utils.arrayFilter(glIntegrationUI.glIntegrationModel.ReferenceDetails.Items(), function (referenceIntegration) {
                return (referenceIntegration.SourceTransactionType() === selectedSourceTransactionType);
            });
            item = item[0];

            if (item == null) return; // if Reference Detail null the return

            //get G/L Reference Integaration model
            switch (selectedGLTransactionField) {
                case glIntegrationUI.glTransactionField.GLEntryDescription:
                    if (item.GLEntryDescription.ETag() != model.Data.ETag) {
                        item.GLEntryDescription.ETag(model.Data.ETag);
                    }
                    break;
                case glIntegrationUI.glTransactionField.GLDetailReference:
                    if (item.GLDetailReference.ETag() != model.Data.ETag) {
                        item.GLDetailReference.ETag(model.Data.ETag);
                    }
                    break;
                case glIntegrationUI.glTransactionField.GLDetailDescription:
                    if (item.GLDetailDescription.ETag() != model.Data.ETag) {
                        item.GLDetailDescription.ETag(model.Data.ETag);
                    }
                    break;
                case glIntegrationUI.glTransactionField.GLDetailComment:
                    if (item.GLDetailComment.ETag() != model.Data.ETag) {
                        item.GLDetailComment.ETag(model.Data.ETag);
                    }
                    break;
                default:
                    break;
            }
            glIntegrationOnSuccess.saveReferenceIntegration();

        } else {
            sg.utls.showMessagePopup(model, "#windowmessage");
        }
    },
    loadGLIntegration: function (result) {
        if (result === null) return;
        if (result.UserMessage.IsSuccess) {
            //get Enum list to parse text
            glIntegrationUI.sourceTransactionTypeList = result.GetSourceTransactionType;
            glIntegrationUI.segmentSeparatorTypeList = result.GetSeparator;
            glIntegrationUI.glTransactionFieldList = result.GetGLTransactionField;
            glIntegrationUI.glReferenceIntegrationList = result.ReferenceDetails.Items;
            //load data
            glIntegrationOnSuccess.displayGLIntegration(result, sg.utls.OperationMode.LOAD); // 1 - Load Options
        }
    },
    saveGLIntegration: function (result) {
        if (result == null) return;
        if (result.UserMessage.IsSuccess) {
            glIntegrationOnSuccess.displayGLIntegration(result, sg.utls.OperationMode.SAVE); // 2 - Save GLIntegration          
        }
    },
    saveReferenceIntegration: function (model) {
        var grid = $("#gridGLReferenceIntegration").data("kendoGrid");


        //update selected row data
        for (var index = 0; index < grid.dataSource.data().length; index++) {
            var data = grid.dataSource.data()[index];
            if (data.SourceTransactionType === glIntegrationUI.glIntegrationModel.Data.koSourceTransactionType()) {
                //var updateColumnIndex = parseInt(glIntegrationUI.glIntegrationModel.Data.koGLTransactionField()) + 1;
                var updateField = glIntegrationUtils.getGLTransactionFieldByValue(glIntegrationUI.glIntegrationModel.Data.koGLTransactionField());
                var fieldName = glIntegrationUtils.getRefrenceIntegrationGridColumn(updateField.fieldName).field; //grid.columns[updateField.fieldIndex].field;
                //used to maintain dirty flag in for the 1st tab data change
                var isFirstTabDirty = glIntegrationUI.glIntegrationModel.isModelDirty.isDirty();
                data.set(fieldName, glIntegrationUI.glIntegrationModel.Data.koExample());
                if (!isFirstTabDirty && !isDirtyRequiredForIntegrationGrid) {
                    glIntegrationUI.glIntegrationModel.isModelDirty.reset();
                }
                break;
            }
        }

        var modifiedGLReferenceIntegrationData = {
            SourceTransactionType: glIntegrationUI.glIntegrationModel.Data.koSourceTransactionType(),
            GLTransactionField: glIntegrationUI.glIntegrationModel.Data.koGLTransactionField(),
            Example: glIntegrationUI.glIntegrationModel.Data.koExample(),
            Seperator: glIntegrationUI.glIntegrationModel.Data.koSeparator(),
            IncludedSegment1: 0,
            IncludedSegment2: 0,
            IncludedSegment3: 0,
            IncludedSegment4: 0,
            IncludedSegment5: 0
        };

        modifiedGLReferenceIntegrationData = glIntegrationUI.assignSegmentData(modifiedGLReferenceIntegrationData);
        glIntegrationUI.setReferenceDetail(modifiedGLReferenceIntegrationData);

        // reset GLTransactionDetail Dirty to false
        glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty(false);

    },
    displayGLIntegration: function (result, uiMode) {
        if (result !== null) {
            if (!glIntegrationUI.hasKoApplied) {
                glIntegrationUI.glIntegrationModel = ko.mapping.fromJS(result);
                //check is G/L Integration KO Extn required.
                if (typeof glIntegrationExtend !== "undefined" && typeof (glIntegrationExtend.extnModel) == "function") {
                    glIntegrationExtend.extnModel(glIntegrationUI.glIntegrationModel, uiMode);
                }
                else {
                    glIntegrationKoExtn.glIntegrationModelExtension(glIntegrationUI.glIntegrationModel, uiMode); // ViewModel.Data
                }

                glIntegrationUI.glIntegrationModel.isModelDirty = new ko.dirtyFlag(glIntegrationUI.glIntegrationModel.Data, glIntegrationUI.ignoreIsDirtyProperties);

                ko.applyBindings(glIntegrationUI.glIntegrationModel);

                // Set to true so that binding should happen once for a page
                glIntegrationUI.hasKoApplied = true;
                if ($("#glIntegrationTabStrip").length) {
                    $("#glIntegrationTabStrip").show()
                }
            } else {
                ko.mapping.fromJS(result, glIntegrationUI.glIntegrationModel);
                glIntegrationUI.glIntegrationModel.Data.UIMode(uiMode);
                glIntegrationUI.glIntegrationModel.isModelDirty.reset();
            }

            if (!glIntegrationUI.isKendoControlInitialised) {
                glIntegrationUI.isKendoControlInitialised = true;
                glIntegrationUI.initDropdownList();
            }
        }
    }
};

glIntegrationUtils = {
    clearValidationMessage: function () {
        $("#success").empty();
        $("#message").empty();

        // Hide the messages
        $("#success").hide();
        $("#message").hide();
    },
    getSourceTransactionTypeValue: function (fieldValue, sourceTransactionTypeList) {
        //wrap enum to text
        //var result = $.grep(glIntegrationUI.sourceTransactionTypeList, function (n) {
        //    return n["Value"] === parseInt(fieldValue) ? n["Text"] : "";
        //});

        var result = $.grep(sourceTransactionTypeList, function (n) {
            return n["Value"] === parseInt(fieldValue);
        });
        return result[0]["Text"];
    },
    getSeparatorValue: function (fieldValue) {
        //wrap enum to text
        var result = $.grep(glIntegrationUI.segmentSeparatorTypeList, function (n) {
            return n["Value"] === parseInt(fieldValue);
        });
        return result[0]["Text"];
    },
    getGLTransactionFieldValue: function (fieldValue) {
        //wrap enum to text
        var result = $.grep(glIntegrationUI.glTransactionFieldList, function (n) {
            return n["Value"] === parseInt(fieldValue);
        });
        return result[0]["Text"];
    },
    getGLTransactionFieldString: function (fieldText) {
        //wrap text to enum
        var result = $.grep(glIntegrationUI.glTransactionFieldList, function (n) {
            return (n["Text"] === fieldText.toString());
        });
        return result[0]["Value"];
    },
    getGLTransactionFieldByValue: function (value) {
        //wrap object
        var result = $.grep(glReferenceIntegrationFields, function (n) {
            return n.enumValue === parseInt(value);
        });
        return result[0];
    },
    getGLTransactionFieldByName: function (fieldName) {
        //wrap object
        var result = $.grep(glReferenceIntegrationFields, function (n) {
            return n.fieldName === fieldName;
        });
        return result[0];
    },
    getGLTransactionFieldByIndex: function (index) {
        //wrap object
        var result = $.grep(glReferenceIntegrationFields, function (n) {
            return n.fieldIndex === parseInt(index);
        });
        return result[0];
    },
    getValidGLTransactionFieldName: function (selectedField, glTransactionFieldList) {
        //get active index from selected field.
        var activeColumnIndex = glIntegrationUtils.getGLTransactionFieldByName(selectedField).fieldIndex;
        var activeGLTransactionField;
        var glTransactionField;
        var colIndex = 0;
        var selectedFieldName = "";
        do {
            //get active G/L Transaction field by active column index.
            activeGLTransactionField = glIntegrationUtils.getGLTransactionFieldByIndex(activeColumnIndex);
            //get valid G/L Transaction field
            glTransactionField = glIntegrationUtils.getGLTransactionField(glTransactionFieldList, activeGLTransactionField.enumValue);
            //if G/L Transaction field found break, otherwise continue
            if (glTransactionField) {
                selectedFieldName = glTransactionField.fieldName;
                break;
            }
            activeColumnIndex++;
            //to search from begin, if column index = 5 then reset to 0.
            if (activeColumnIndex === 5) {
                activeColumnIndex = 0;
            }
            colIndex++;
        } while (colIndex < 5)

        return selectedFieldName;
    },
    getGLTransactionField: function (glTransactionFieldList, fieldValue) {
        var glTransactionField;
        $.each(glTransactionFieldList, function (index, field) {
            if (field.Value === fieldValue) {
                glTransactionField = glIntegrationUtils.getGLTransactionFieldByValue(fieldValue);
                return false;
            }
        });
        return glTransactionField;
    },
    getReferenceDetail: function (sourceTransactionType, glTransactionField) {
        var result = {
            selectedItem: null, index: 0
        };
        var item = $.grep(glIntegrationUI.glIntegrationModel.ReferenceDetails.Items(), function (referenceIntegration) {
            return (referenceIntegration.SourceTransactionType() == sourceTransactionType);
        });
        item = item[0];
        if (item) {
            switch (glTransactionField) {
                case glIntegrationUI.glTransactionField.GLEntryDescription:
                    result.selectedItem = item.GLEntryDescription;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailReference:
                    result.selectedItem = item.GLDetailReference;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailDescription:
                    result.selectedItem = item.GLDetailDescription;
                    break;
                case glIntegrationUI.glTransactionField.GLDetailComment:
                    result.selectedItem = item.GLDetailComment;
                    break;
                default:
                    break;
            }
        }
        return result;
    },
    getIncludeSegmentValue: function (selectedItem, segmentId) {
        var includeSegmentValue = String.empty;

        switch (segmentId) {
            case glIntegrationUI.includedSegment.IncludedSegment1:
                includeSegmentValue = selectedItem.IncludedSegment1Value();
                break;
            case glIntegrationUI.includedSegment.IncludedSegment2:
                includeSegmentValue = selectedItem.IncludedSegment2Value();
                break;
            case glIntegrationUI.includedSegment.IncludedSegment3:
                includeSegmentValue = selectedItem.IncludedSegment3Value();
                break;
            case glIntegrationUI.includedSegment.IncludedSegment4:
                includeSegmentValue = selectedItem.IncludedSegment4Value();
                break;
            case glIntegrationUI.includedSegment.IncludedSegment5:
                includeSegmentValue = selectedItem.IncludedSegment5Value();
                break;
            default:
                break;
        }
        return includeSegmentValue;
    },
    getRefrenceIntegrationGridColumn: function (fieldName) {
        var grid = $("#gridGLReferenceIntegration").data("kendoGrid");
        var column = $.grep(grid.columns, function (c) {
            return c.field === fieldName;
        });
        return column[0];
    },
    moveSegmentItem: function (fromList, toList, isInclude) {

        var fromLength = fromList.dataSource.data().length;
        var toLength = toList.dataSource.data().length;

        //checking whether tolist is having more than 5 items if contains then we should not alow to include any more items. 
        var includeEnable = isInclude ? toLength >= glIntegrationUI.MAX_SEGMENT : false;

        //if fromList doesn't contains any item then we should not continue to any include/exclude.
        if (fromLength <= 0 || includeEnable) {
            return;
        }

        var selected = fromList.select();
        var index = selected.index();

        if (selected.length <= 0) return;

        var items = [];

        //collocting all the select items from fromList
        $.each(selected, function (idx, elem) {
            items.push(fromList.dataSource.at(selected.index()));
        });
        var fromDS = fromList.dataSource;
        var toDS = toList.dataSource;

        //removing already selected items from fromList and adding same to toList
        $.each(items, function (idx, elem) {
            toDS.add(elem);
            fromDS.remove(elem);
        });

        toDS.sync();
        fromDS.sync();

        fromLength = fromList.dataSource.data().length;
        toLength = toList.dataSource.data().length;

        if (fromLength > 0) {

            //if selected row is the last record then we need to take the previous record from last else same index record.
            var fromListIndextoSelect = (index > fromLength - 1) ? fromLength - 1 : index;

            fromList.select(fromList.element.children()[fromListIndextoSelect]);
        }
        if (toList.length = 1) {
            toList.select(toList.element.children().first());
        }

        //checking whether tolist is having more than 5 items if contains then we should not alow to include any more items. 
        includeEnable = isInclude ? toLength >= glIntegrationUI.MAX_SEGMENT : fromLength >= glIntegrationUI.MAX_SEGMENT;

        if (!includeEnable) {
            //diable if from list empty.
            includeEnable = isInclude ? fromLength <= 0 : false;
        }

        //checking whether fromList having any record if no then have to disable Exclude
        var excludeEnable = isInclude ? toLength <= 0 : fromLength <= 0;

        sg.controls.enableDisable("#btnInclude", includeEnable)
        sg.controls.enableDisable("#btnExclude", excludeEnable)


        glIntegrationUtils.setExample();
    },
    setExample: function () {

        if (glIntegrationUI.toSegmentList == null) return;
        var example = "";
        var toLength = glIntegrationUI.toSegmentList.dataSource.data().length;
        var selectedSeperator = $("#Data_Separator").data("kendoDropDownList").value();

        var seperator = $.grep(separators, function (n) {
            return n['Value'] === parseInt(selectedSeperator);
        });

        var index = 0;
        //looping toList to form example string
        $.each(glIntegrationUI.toSegmentList.dataSource.data(), function (idx, item) {
            index += 1;
            if (item == null) return;
            example += item.SegmentName();
            //skiping seperator to add for last item value
            if (index < toLength) {
                example += seperator[0].Text;
            }
        });

        //$("#txtExample").val(example);
        glIntegrationUI.glIntegrationModel.Data.koExample(example);
    },
    shiftGLEntryDescription: function (flag) {
        var glTransactionFieldList = ko.mapping.toJS(glIntegrationUI.glTransactionFieldList);
        var bResetDropdownIndex = false;
        if (flag !== null || typeof flag !== "undefined") {
            if (flag === true) {
                //get temp data
                // Remove the fristItem from the list (0) which exist in the array
                glTransactionFieldList.shift();
            }
        }
        glIntegrationUI.glIntegrationModel.GetGLTransactionField(glTransactionFieldList);
    },
    setGLReferenceIntegrationDirty: function () {
        //set GLReferenceIntegration Dirty = true
        glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty(true);
    },
    resetGLReferenceIntegrationDirty: function () {
        //reset GLReferenceIntegration Dirty = false
        glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty(false);
    },
    checkIsDetailDirty: function (ddList, oldValue) {
        var selectedSourceTransactionType = parseInt($("#Data_SourceTransactionType").data("kendoDropDownList").value());
        var selectedGLTransactionField = parseInt($("#Data_GLTransactionField").data("kendoDropDownList").value());
        if (glIntegrationUI.glIntegrationModel.Data.koIsGLTransactionDetailDirty() === true) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    glIntegrationUI.setWindowData(selectedSourceTransactionType, selectedGLTransactionField);
                },
                function () { // No
                    ddList.select(function (dataItem) {
                        return parseInt(dataItem.value) === parseInt(oldValue);
                    });
                    //get latest values and reset
                    selectedSourceTransactionType = parseInt($("#Data_SourceTransactionType").data("kendoDropDownList").value());
                    selectedGLTransactionField = parseInt($("#Data_GLTransactionField").data("kendoDropDownList").value());
                    glIntegrationUI.glIntegrationModel.Data.koSourceTransactionType(selectedSourceTransactionType);
                    glIntegrationUI.glIntegrationModel.Data.koGLTransactionField(selectedGLTransactionField);
                },
                $.validator.format(glIntegrationDetailResource.saveConfirm, glIntegrationDetailResource.glIntegrationDetailTitle));
        } else {
            glIntegrationUI.setWindowData(selectedSourceTransactionType, selectedGLTransactionField);
        }
    },
    isNullOrEmpty: function (value) {
        if (value === null || typeof value === "undefined") {
            return true;
        }
        return (value === "");
    },
    GetValue: function (value) {
        if (glIntegrationUtils.isNullOrEmpty(value)) {
            return "";
        }
        return value;
    }

};

glIntegrationKoExtn = {
    glIntegrationModelExtension: function (model, uiMode) {
        model.Data.UIMode = ko.observable(uiMode);

        //extended custom model for binding popup window controls
        model.Data.GLTransactionDetail = ko.observable(new GLTransactionDetail());

        //used to maintain dirty flag in the popup window data change
        model.Data.koIsGLTransactionDetailDirty = ko.observable(false);

        model.Data.koSourceTransactionType = ko.computed({
            read: function () {
                return model.Data.GLTransactionDetail().koSourceTransactionType();
            },
            write: function (value) {
                model.Data.GLTransactionDetail().koSourceTransactionType(parseInt(value));
            },
            owner: this
        });

        model.Data.koGLTransactionField = ko.computed({
            read: function () {
                return model.Data.GLTransactionDetail().koGLTransactionField();
            },
            write: function (value) {
                model.Data.GLTransactionDetail().koGLTransactionField(parseInt(value));
            },
            owner: this
        });

        model.Data.koSeparator = ko.computed({
            read: function () {
                return model.Data.GLTransactionDetail().koSeparator();
            },
            write: function (value) {
                model.Data.GLTransactionDetail().koSeparator(parseInt(value));
            },
            owner: this
        });

        model.Data.koExample = ko.computed({
            read: function () {
                return model.Data.GLTransactionDetail().koExample();
            },
            write: function (value) {
                model.Data.GLTransactionDetail().koExample(value);
            },
            owner: this
        });
    }
};
