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

var savedQueryGridUI = {
    initGrid: function() {
        $("#savedQueryGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: function () {
                            //Generate random url on each request to prevent IE from caching the request result.
                            var random = "?random=" + sg.utls.guid();
                            return sg.utls.url.buildUrl("Core", "AdhocInquiry", "GetSavedQuery") + random;
                        }
                    }
                },
            },
            height: 219,
            pageable: false,
            scrollable: true,
            sortable: true,
            selectable: true,
            resizable: true,
            columns: [
                {
                    field: "Data.Name",
                    title: savedQueryResources.QueryName,
                    width: 180
                },
                {
                    field: "Data.InquiryQueryType",
                    title: savedQueryResources.Type,
                    width: 80,
                    template: '#= savedQueryGridUI.getTypeName(Data.InquiryQueryType) #'
                },
                {
                    field: "Data.DateModified",
                    title: savedQueryResources.DateModified,
                    template: '#= sg.utls.kndoUI.getFormattedDate(Data.DateModified) #',
                }
            ],
            editable: false,
            change: function(e) {
                var grid = $("#savedQueryGrid").data("kendoGrid");
                var selectedRowData = sg.utls.kndoUI.getSelectedRowData(grid);
                $("#SaveQueryPanel_QueryName").val(selectedRowData.Data.Name);
                var radioBtnId = selectedRowData.Data.InquiryQueryType === 1 ? "rdbPublicQueryType" : "rdbPrivateQueryType";
                $("#" + radioBtnId).click();
                $("#SaveQueryPanel_QueryDescription").val(selectedRowData.Data.Description);
            }
        });
    },

    getTypeName: function (inquiryType)
    {
        if (inquiryType === 1)
            return savedQueryResources.Public;
        else if (inquiryType === 2)
            return savedQueryResources.Private;
        else
            return inquiryType;
    },
}
