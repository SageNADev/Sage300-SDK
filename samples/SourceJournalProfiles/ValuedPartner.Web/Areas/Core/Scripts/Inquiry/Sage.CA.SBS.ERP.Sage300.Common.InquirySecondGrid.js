/* Copyright (c) 2016-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var inquirySecondGrid = inquirySecondGrid || {};
inquirySecondGrid = {
    pageSize: 5,

	getParam: function () {
		var grid = inquirySecondGrid.gridInstance();

		var parameters = {
		    pageNumber: grid.dataSource.page() - 1,
		    pageSize: grid.dataSource.pageSize()
		};

        //This is defined when the partial view is generated
		if ($.isFunction(SecondGridConfigGetParameter)) {
		    parameters = $.extend(parameters, SecondGridConfigGetParameter());
		}

		return parameters;
	},

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
	}
};