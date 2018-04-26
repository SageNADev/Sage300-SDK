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

//---------------------------------------** Initialise field for Posting Sequence grid **----------------------------

var glIntegrationPostingSequence = {
    SerialNoHidden: $(postingSequenceGridColumnsResource.headerSerialNo).attr("hidden") ? $(postingSequenceGridColumnsResource.headerSerialNo).attr("hidden") : true,

    SequenceTitle: postingSequenceGridColumnsResource.headerSequence,
    SequenceHidden: $(postingSequenceGridColumnsResource.headerSequence).attr("hidden") ? $(postingSequenceGridColumnsResource.headerSequence).attr("hidden") : false,

    SequenceNoTitle: postingSequenceGridColumnsResource.headerSequenceNo,
    SequenceNoHidden: $(postingSequenceGridColumnsResource.headerSequenceNo).attr("hidden") ? $(postingSequenceGridColumnsResource.headerSequenceNo).attr("hidden") : false
};


//*********************************** GL Integration Posting Sequence GRID ***********************************************


var PostingSequenceGridUtility = {
    postingSequenceData: ko.observableArray(),
    glPostingSequenceConfig: {
        scrollable: true,
        navigatable: false,
        resizable: true,
        sortable: false,
        selectable: true,
        reorderable: sg.utls.reorderable,
        page: 0,
        param: null,
        pageSize: sg.utls.gridPageSize,
        getPageUrl: function () {
            return glIntegrationRepositoryURL.getGLPostingSequence();
        },
        isServerPaging: true,
        editable: {
            mode: "incell",
            confirmation: false
        },
        getParam: function() {
            var grid = $('#gridGLPostingSequence').data("kendoGrid");
            var pageNumber = grid.dataSource.page();
            var pageSize = grid.dataSource.pageSize();
            var model = ko.mapping.toJS(glIntegrationUI.glIntegrationModel);
            var parameters = {
                pageNumber: pageNumber - 1,
                pageSize: pageSize,
                model: model
            };
            return parameters;
        },
        buildGridData: function (successData) {
            successData = successData.GLPostingSequences;
            if (successData != null && successData.TotalResultsCount > 0)
                dirtyFlags.isGridDirty(false);
            var gridData = null;
            gridData = [];
            // Notify the grid which part of the returned data contains the items for the grid. 
            gridData.data = successData.Items;
            // Notify the grid which part of the returned data contains the total number of items for the grid. 
            gridData.totalResultsCount = successData.TotalResultsCount;
            return gridData;
        },
        columns: [
            {
                field: "Sequence",
                attributes: { "class": "w10 input_Number_Left_Align" },
                headerAttributes: { "class": "w10 input_Number_Left_Align" },
                hidden: glIntegrationPostingSequence.SequenceHidden,
                title: glIntegrationPostingSequence.SequenceTitle,
                width: "50%",
                editor: function (container, options) {
                    var grid = $('#gridGLPostingSequence').data("kendoGrid");
                    grid.select(container.closest("tr"));
                    sg.utls.kndoUI.nonEditable(grid, container);
                }
            },
            {
                field: "SequenceNo",
                attributes: { "class": "w30 input_Number_Right_Align" },
                headerAttributes: { "class": "w30 input_Number_Left_Align" },
                hidden: glIntegrationPostingSequence.SequenceNoHidden,
                title: glIntegrationPostingSequence.SequenceNoTitle,
                width: "50%",
                editor: function (container, options) {
                    var grid = $('#gridGLPostingSequence').data("kendoGrid");
                    grid.select(container.closest("tr"));
                    sg.utls.kndoUI.nonEditable(grid, container);
                }
            },
            {
                field: "SerialNo",
                hidden: glIntegrationPostingSequence.SerialNoHidden
            }
        ]
    }
};

