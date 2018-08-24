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
