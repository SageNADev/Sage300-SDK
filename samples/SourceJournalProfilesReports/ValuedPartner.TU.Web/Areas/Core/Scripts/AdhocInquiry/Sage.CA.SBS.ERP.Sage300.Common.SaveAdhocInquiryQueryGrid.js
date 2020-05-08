// Copyright (c) 2020 Sage Software, Inc.  All rights reserved.
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
                    template: '#: savedQueryGridUI.getTypeName(Data.InquiryQueryType) #'
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
