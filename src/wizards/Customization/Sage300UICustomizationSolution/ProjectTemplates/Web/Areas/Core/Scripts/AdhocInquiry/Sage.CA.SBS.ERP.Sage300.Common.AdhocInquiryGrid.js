// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.
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
                pageSize: 10
            },
            autoBind: false,
            height: 500,
            selectable: true,
            scrollable: true,
            resizable: true,
            pageable: {
                input: true,
                numeric: false,
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
