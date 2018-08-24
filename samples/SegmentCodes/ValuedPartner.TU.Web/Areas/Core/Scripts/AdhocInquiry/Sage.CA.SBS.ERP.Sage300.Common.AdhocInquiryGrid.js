// Copyright (c) 2018 Sage Software, Inc.  All rights reserved.
"use strict";

var adhocInquiryGridUI = adhocInquiryGridUI ||
{
    initGrid: function() {
        $("#adhocInquiryGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: function (options) {
                        adhocInquiryUI.apply(options);
                    }
                },
                schema: {
                    data: 'Items',
                    total: 'TotalResultsCount',
                },
                serverPaging: true,
                pageSize: 20,
                aggregate: [
                    { field: "DocTotal", aggregate: "sum" },
                    { field: "DocTotal", aggregate: "min" },
                    { field: "DocTotal", aggregate: "max" }
                ]
            },
            autoBind: false,
            height: 543,
            columnMenu: true,
            filterable: true,
            sortable: {
                mode: "multiple",
                allowUnsort: true,
                showIndexes: true
            },
            selectable: true,
            scrollable: true,
            resizable: true,
            reorderable: true,
            groupable: true,
            pageable: {
                input: true,
                numeric: false,
                refresh: true
            },
            columns: adhocInquiryGridUI.getColumns(ko.mapping.toJS(adhocInquiryUI.inquiryModel.Data.InquiryResultDefinitions())),
            editable: false,

        });
    },

    getColumns: function(queryResultDefinitions) {
        return queryResultDefinitions.map(function(property) {
            return { field: property.Field, title: nameToDisplayName[property.Field] };
        });
    },

    getGrid: function() {
        return $("#adhocInquiryGrid").data("kendoGrid");
    },

    refreshGridData: function (result) {
        var gridData = adhocInquiryGridUI.getGrid().dataSource;
        gridData.error();// Stop ongoing grid reading event
        gridData.read();
    }
}
