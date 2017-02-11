/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

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

