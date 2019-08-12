/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";

var documentHistory = documentHistory || {};

var documentHistoryColumnName = {
    DocumentTypeString: "",
    CheckNumber: "",
    PostingDate: "",
    ReferenceDocumentNo: "",
    PaymentAmount: "",
    PayerCustomerNumber: "",
    PayerCustomerName: "",
    TransactionTypeString: ""
};

var documentHistoryEnum = documentHistoryEnum || {};
documentHistoryEnum.moduleName = {
    AP: "AP",
    AR: "AR"
};

var gridColConfig = {

    noEditor: function (container, options) {
        sg.utls.kndoUI.nonEditable($('#' + documentHistoryUIGrid.gridId).data("kendoGrid"), container);
    },

    getColumn: function (fieldName, isHidden, caption, columnClass, templateExp, headerTemplateExp, editor) {
        var column = {
            field: fieldName,
            hidden: isHidden,
            title: caption,
            attributes: isHidden ? { sg_Customizable: false } : { "class": columnClass },
            headerAttributes: { "class": columnClass },
            template: templateExp,
            headerTemplate: headerTemplateExp,
            editor: editor,
        }
        return column;
    },
};

//Document History Grid 
var documentHistoryUIGrid =
{
    init: function (params) {

        documentHistoryUIGrid.gridId = params.gridId,
        documentHistoryUIGrid.modelData = params.modelData,
        documentHistoryUIGrid.currentRowDocumentNumber = params.currentRowDocumentNumber,
        documentHistoryUIGrid.currentNumber = params.currentNumber
        documentHistoryUIGrid.btnEditColumnsId = params.btnEditColumnsId,
        documentHistoryUIGrid.preferencesTypeId = params.preferencesTypeId

        documentHistoryUIGrid.initButton();

    },

    initButton: function () {

        var grid = $('#' + documentHistoryUIGrid.gridId).data("kendoGrid");
        documentHistoryUIGrid.defaultColumns = $.extend(true, {}, grid.columns);
        documentHistoryUIGrid.defaultColumns.length = grid.columns.length;

        $('#' + documentHistoryUIGrid.btnEditColumnsId).on('click', function () {
            GridPreferences.initialize('#' + documentHistoryUIGrid.gridId, documentHistoryUIGrid.preferencesTypeId, $(this), documentHistoryUIGrid.defaultColumns);
        });
    },

    getFormattedValue: function (fieldValue, decimal) {
        if (fieldValue != null)
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(!isNaN(parseFloat(fieldValue)) ? parseFloat(fieldValue) : 0, decimal);
        else {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(0, decimal);
        }
        return fieldValue;
    },

    getFormattedDate: function (fieldValue) {
        if (fieldValue != null) {
            return sg.utls.kndoUI.getFormattedDate(fieldValue);
        }
        return fieldValue;
    },

    gridId: "",
    btnEditColumnsId: "",
    modelData: null,
    currentRowDocumentNumber: null,
    currentNumber: null,
    preferencesTypeId: null,
    defaultColumns: null,
    decimalValue: 2,

    documentHistoryGridConfig: function (area, controller, action, gridId, modelName, moduleName, columnList) {

        documentHistoryUIGrid.gridId = gridId;

        var isHide = false;
        var isAR = false;

        if (moduleName == documentHistoryEnum.moduleName.AP) {
            isHide = true;
            isAR = false;
        } else {
            isHide = false;
            isAR = true;
        }

        for (var prop in columnList) {
            documentHistoryColumnName[prop] = columnList[prop];
        }

        return {
            autoBind: false,
            pageSize: sg.utls.gridPageSize,
            scrollable: true,
            reorderable: sg.utls.reorderable,
            navigatable: true, //enable grid cell tabbing for safari browser
            resizable: true,
            selectable: true,
            //reorderable: sg.utls.reorderable,
            isServerPaging: true,
            //Param will be null during Get and will contain the data that needs to be passed to the server on create
            param: null,
            //URL to get the data from the server. 
            pageUrl: sg.utls.url.buildUrl(area, controller, action),
            pageable: {
                input: true,
                numeric: false
            },
            getParam: function () {
                var grid = $('#' + gridId).data("kendoGrid");

                var parameters = {
                    pageNumber: grid.dataSource.page() - 1,
                    pageSize: sg.utls.gridPageSize,
                    model: ko.mapping.toJS(documentHistoryUIGrid.modelData),
                    documentNumber: documentHistoryUIGrid.currentRowDocumentNumber(),
                    number: documentHistoryUIGrid.currentNumber(),
                };
                return parameters;
            },
            buildGridData: function (successData) {
                var gridData = null;

                if (successData == null) {
                    return;
                }

                if ((successData.UserMessage && successData.UserMessage.IsSuccess) || successData.Items !== undefined) {
                    gridData = [];

                    var documentNumberData = (successData.Data !== undefined) ? successData.Data[modelName] : successData;
                    ko.mapping.fromJS(documentNumberData, {}, documentHistoryUIGrid.modelData[modelName]);
                    gridData.data = documentNumberData.Items
                    gridData.totalResultsCount = documentNumberData.TotalResultsCount;

                } else {
                    sg.utls.showMessagePopupWithoutClose(successData);
                }

                return gridData;
            },

            columnReorder: function (e) {
                GridPreferencesHelper.saveColumnOrder(e, '#' + documentHistoryUIGrid.gridId, documentHistoryUIGrid.preferencesTypeId);
            },

            columns: [
                 gridColConfig.getColumn(documentHistoryColumnName.DocumentTypeString, false, documentHistoryGridResources.TransactionType, "w220", null, null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.CheckNumber, false, documentHistoryGridResources.CheckNumber, "w220", null, null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.PostingDate, false, documentHistoryGridResources.PostingDate, "w220", '#= documentHistoryUIGrid.getFormattedDate(PostingDate) #', null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.PaymentAmount, false, documentHistoryGridResources.AppliedAmount, "w220", isAR ? '#= documentHistoryUIGrid.getFormattedValue(CustReceiptAmount,documentHistoryUIGrid.decimalValue) #' : '#= documentHistoryUIGrid.getFormattedValue(VendorPaymentAmount,documentHistoryUIGrid.decimalValue) #', null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.PayerCustomerNumber, (isHide), documentHistoryGridResources.PayerCustomerNumber, "w220", null, null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.PayerCustomerName, (isHide), documentHistoryGridResources.PayerCustomerName, "w220", null, null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.TransactionTypeString, false, documentHistoryGridResources.TransactionDescription, "w220", null, null, gridColConfig.noEditor),
                 gridColConfig.getColumn(documentHistoryColumnName.ReferenceDocumentNo, false, documentHistoryGridResources.RefDocumentNumber, "w220", null, null, gridColConfig.noEditor),
            ],
        }
    }
};

