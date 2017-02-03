/* Copyright (c) 2016 Sage Software, Inc.  All rights reserved. */

"use strict";

var inquiryGrid = inquiryGrid || {};
inquiryGrid = {
    funcCurrencyColumns: [
        "ExchangeRate", "FuncCurrencyInvoiceAmount", "FuncAppliedAmount", "FuncCurrencyAmountDue",
        "FuncCurrencyDiscountAmount", "FuncCurrencyTaxAmount", "FuncCurrOrigRtngAmt", "FuncCurrRetainageAmount"
    ],
    custCurrencyColumns: [
        "CustCurrencyInvoiceAmount", "CustAppliedAmount", "CustCurrencyAmountDue", "CustCurrencyDiscountAmount",
        "CustCurrencyTaxAmount", "CustCurrOrigRtngAmt", "CustCurrRetainageAmount"
    ],
    afterDataBindHandler: null,

    changeHandler: null,

    gridInstance: function () {
        return $("#CustomerDocumentGrid").data("kendoGrid");
    },

    //Hide/show the grid columns depends on the currency type, and also applis the saved user preference
    displayColumnsByCurrencyType: function (grid, custColumns, funcColumns) {
        if (inquiryUI.isFunctionalCurrency()) {
            sg.utls.kndoUI.hideGridColumns(grid, custColumns);
            sg.utls.kndoUI.showGridColumns(grid, funcColumns);
        } else {
            sg.utls.kndoUI.hideGridColumns(grid, funcColumns);
            sg.utls.kndoUI.showGridColumns(grid, custColumns);
        }
        inquiryUI.setUserPreference(grid);
    },

    getParam: function () {
        var grid = inquiryGrid.gridInstance();

        var parameters = {
            pageNumber: grid.dataSource.page() - 1,
            pageSize: grid.dataSource.pageSize(),
            module: inquiryUI.module,
            inquiryType: inquiryUI.inquiryType,
            inquiryFilters: inquiryUI.generateStaticFilters()
        };

        return parameters;
    },

    // Set up for getting  paged Customized Screen Profile Details
    pageUrl: sg.utls.url.buildUrl("Core", "Inquiry", "Get"),

    // Call back function when Get is successful. In this, the data for the grid and the total results count are to be set along with updating knockout
    buildGridData: function (successData) {
        var gridData = null;

        // ReSharper disable once QualifiedExpressionMaybeNull
        if (successData !== null) {
            gridData = [];

            if (successData.CustomerDocuments != null) {
                ko.mapping.fromJS(successData.CustomerDocuments, {}, inquiryUI.inquiryKoBindingModel.CustomerDocuments);

                gridData.totalResultsCount = successData.CustomerDocuments.TotalResultsCount;
                if (gridData.totalResultsCount > 0) {
                    gridData.data = successData.CustomerDocuments.Items;
                }
                else {
                    // If grid data is empty then set pagenumber into 0.
                    gridData.data = null;
                }
            }
        }
        else {
            sg.utls.showMessage(successData);
        }
        
        return gridData;
    },

    afterDataBind: function () {
        if (inquiryGrid.afterDataBindHandler) {
            inquiryGrid.afterDataBindHandler();
        }
    },

    dataChange: function (changedData) {
        var selectAllCheckBox = null;
        //workprofileUI.SerialNumber = changedData.rowData.SerialNumber;
    },

    change: function (arg) {
        if (inquiryGrid.changeHandler) {
            inquiryGrid.changeHandler();
        }
    }
};