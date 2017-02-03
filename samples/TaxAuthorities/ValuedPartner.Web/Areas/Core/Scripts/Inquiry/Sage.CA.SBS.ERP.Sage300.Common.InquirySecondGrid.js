/* Copyright (c) 2016 Sage Software, Inc.  All rights reserved. */

"use strict";

var inquirySecondGrid = inquirySecondGrid || {};
inquirySecondGrid = {
    pageSize: 5,
    funcCurrencyColumns: [
    "ExchangeRate", "FuncReceiptAmount"
    ],
    custCurrencyColumns: [
        "CustReceiptAmount"
    ],

	gridInstance: function () {
		return $("#DocumentPaymentGrid").data("kendoGrid");
	},

	getParam: function () {
		var grid = inquirySecondGrid.gridInstance();

		var parameters = {
			pageNumber: grid.dataSource.page() - 1,
			pageSize: grid.dataSource.pageSize(),
			customerNumber: inquiryUI.getCurrentValue("CustomerNumber"),
			documentNumber: inquiryUI.getCurrentValue("DocumentNumber"),
			isIncludeExchange: inquiryUI.isFunctionalCurrency(),
			orderBy: { "PropertyName": "PostingDate" }
		};

		return parameters;
	},

	// Set up for getting  paged Customized Screen Profile Details
	pageUrl: sg.utls.url.buildUrl("AR", "Payment", "GetPaymentDetailsWithDocument"),

	// Call back function when Get is successful. In this, the data for the grid and the total results count are to be set along with updating knockout
	buildGridData: function (successData) {
		var gridData = null;

		// ReSharper disable once QualifiedExpressionMaybeNull
		if (successData !== null) {
			gridData = [];
			gridData.totalResultsCount = successData.TotalResultsCount;
			if (gridData.totalResultsCount > 0) {
				gridData.data = successData.Items;
			}
			else {
				// If grid data is empty then set pagenumber into 0.
				gridData.data = null;
			}
		}

		return gridData;
	},

	afterDataBind: function () {
		var selectAllCheckBox = null;
		//var selectAllCheckBox = $("#selectAllChkUICustomizationProfile");
		//if (selectAllCheckBox.is(':checked')) {
		//    selectAllCheckBox.prop("checked", false).applyCheckboxStyle();
		//    $("#btnDeleteLineUICustomizationProfile").attr("disabled", true);
		//    return;
		//}
		//var grid = workprofileUICustomGridUtility.fetchGrid();

		//grid.tbody.find(".selectChk").each(function (index) {
		//    if (!($(this).is(':checked'))) {
		//        $("#btnDeleteLineUICustomizationProfile").attr("disabled", true);
		//        return;
		//    }
		//});

		////To disable the header checkbox when the items in the grid are cleared 
		//if (workprofileUI.workprofileModel.Data.CustomList.Items().length === 0) {
		//    $("#selectAllChkUICustomizationProfile").attr("disabled", true);
		//} else {
		//    $("#selectAllChkUICustomizationProfile").attr("disabled", false);
		//}
	},

	dataChange: function (changedData) {
		var selectAllCheckBox = null;
		//workprofileUI.SerialNumber = changedData.rowData.SerialNumber;
	}
};