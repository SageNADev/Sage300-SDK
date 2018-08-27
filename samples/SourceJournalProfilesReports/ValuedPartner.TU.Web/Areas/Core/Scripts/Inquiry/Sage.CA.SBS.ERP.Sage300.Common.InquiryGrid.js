/* Copyright (c) 2016-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

function inquiryGrid(dataField, getParameterFunc, pageSize)
{
    this.afterDataBindHandler = null;
    this.changeHandler = null;
    this.gridInstance = null;
    this.self = null;
    this.gridConfigDataField = dataField;
    this.getParameter = getParameterFunc;
    this.pageSize = pageSize;

    this.getParam = function () {
        var grid = this.self.gridInstance();
        var parameters = {
            pageNumber: grid.dataSource.page() - 1,
            pageSize: grid.dataSource.pageSize(),
            module: inquiryUI.module,
            inquiryType: inquiryUI.inquiryType,
            inquiryFilters: inquiryUI.generateStaticFilters()
        };

        if ($.isFunction(this.self.getParameter)) {
            parameters = $.extend(parameters, this.self.getParameter());
        }

        return parameters;
    };

    // Call back function when Get is successful. In this, the data for the grid and the total results count are to be set along with updating knockout
    this.buildGridData = function(successData) {
        var gridData = null;
        if (successData) {
            if (successData.UserMessage !== null && successData.UserMessage.IsSuccess) {
                gridData = [];

                if (successData[this.self.gridConfigDataField] != null) {
                    ko.mapping.fromJS(successData[this.self.gridConfigDataField],
                        {}, inquiryUI.inquiryKoBindingModel[this.self.gridConfigDataField]);

                    gridData.totalResultsCount = successData[this.self.gridConfigDataField].TotalResultsCount;
                    if (gridData.totalResultsCount > 0) {
                        gridData.data = successData[this.self.gridConfigDataField].Items;
                    } else {
                        // If grid data is empty then set pagenumber into 0.
                        gridData.data = null;
                    }
                }
            } else {
                sg.utls.showMessage(successData);
            }
        }

        return gridData;
    };

    this.afterDataBind = function () {
        if (this.self.afterDataBindHandler) {
            this.self.afterDataBindHandler();
        }
    };

    this.change = function (arg) {
        // this is called from kendo, need to get back the config from arg object
        if (arg.sender.options.self.changeHandler) {
            arg.sender.options.self.changeHandler();
        }
    };
};