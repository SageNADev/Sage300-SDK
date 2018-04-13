// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
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