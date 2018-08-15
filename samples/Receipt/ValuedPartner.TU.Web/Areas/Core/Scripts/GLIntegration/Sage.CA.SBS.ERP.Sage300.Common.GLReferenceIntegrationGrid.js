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

//*********************************** GL Reference Integration GRID ***********************************************

var referenceIntegrationGridUtil = {
    isColumnHidden: function (columnId) {
        var isHidden = $(document.getElementById(columnId)).attr("hidden") ? $(document.getElementById(columnId)).attr("hidden") : false;
        return isHidden;
    }
};

var GLReferenceIntegrationUIGrid = {
    RefrenceIntegrationGrid: null,

    initGridPreferece: function (GLRefrenceIntegrationGrid, IntegrationGridRef) {
        GLReferenceIntegrationUIGrid.RefrenceIntegrationGrid = GLRefrenceIntegrationGrid;

        $('#btnEditGLIntegrationColumns').on('click', function () {
            GridPreferences.initialize('#gridGLReferenceIntegration', GLRefrenceIntegrationGrid, $(this),
                GLReferenceIntegrationUIGrid.GLReferenceIntegrationGridConfig.columns);
        });
        GridPreferencesHelper.setGrid("#gridGLReferenceIntegration", IntegrationGridRef);

    },
    GLReferenceIntegrationGridConfig: {
        autoBind: false,
        pageSize: sg.utls.gridPageSize,
        scrollable: true,
        navigatable: true,
        resizable: true,
        sortable: false,
        selectable: true,
        editable: false,
        pageable: {
            input: true,
            numeric: false
        },
        reorderable: sg.utls.reorderable,
        param: null,
        getParam: function () {
            var grid = $('#gridGLReferenceIntegration').data("kendoGrid");
            var parameters = {
                pageNumber: grid.dataSource.page() - 1,
                pageSize: grid.dataSource.pageSize(),
                model: ko.mapping.toJS(glIntegrationUI.glIntegrationModel)
            };
            return parameters;
        },
        getPageUrl: function () {
            return glIntegrationRepositoryURL.getGLReferenceIntegration();
        },
        isServerPaging: true,
        buildGridData: function (successData) {
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
                field: "SourceTransactionType",
                headerAttributes: { "class": "w200" },
                attributes: { "class": "w200" },
                title: glReferenceIntegrationGridColumns.headerTransactionType,
                hidden: referenceIntegrationGridUtil.isColumnHidden(glReferenceIntegrationGridColumns.headerTransactionType),
                template: '<div class="pencil-wrapper"><span class="pencil-txt">#= glIntegrationUtils.getSourceTransactionTypeValue(SourceTransactionType,SourceTransactionTypeList) #</span>' +
                          '<span class="pencil-icon"><input type="button" class="icon edit-field glIntegrationEditBtn"/></span></div>',
                //width: 180
            },
            {
                field: "GLEntryDescription.Example",
                headerAttributes: { "class": "w200" },
                attributes: { "class": "w200" },
                title: glReferenceIntegrationGridColumns.headerEntryDescription,
                hidden: referenceIntegrationGridUtil.isColumnHidden(glReferenceIntegrationGridColumns.headerEntryDescription),
                template: '<div class="pencil-wrapper"><span class="pencil-txt">#= glIntegrationUtils.GetValue(GLEntryDescription.Example) #</span>' +
                        '<span class="pencil-icon"><input type="button" class="icon edit-field glIntegrationEditBtn"/></span></div>',
                //width: 180
            },
            {
                field: "GLDetailReference.Example",
                headerAttributes: { "class": "w200" },
                attributes: { "class": "w200" },
                title: glReferenceIntegrationGridColumns.headerDetailReference,
                hidden: referenceIntegrationGridUtil.isColumnHidden(glReferenceIntegrationGridColumns.headerDetailReference),
                template: '<div class="pencil-wrapper"><span class="pencil-txt">#= glIntegrationUtils.GetValue(GLDetailReference.Example) #</span>' +
                       '<span class="pencil-icon"><input type="button" class="icon edit-field glIntegrationEditBtn"/></span></div>',
                //width: 180
            },
            {
                field: "GLDetailDescription.Example",
                headerAttributes: { "class": "w200" },
                attributes: { "class": "w200" },
                title: glReferenceIntegrationGridColumns.headerDetailDescription,
                hidden: referenceIntegrationGridUtil.isColumnHidden(glReferenceIntegrationGridColumns.headerDetailDescription),
                template: '<div class="pencil-wrapper"><span class="pencil-txt">#= glIntegrationUtils.GetValue(GLDetailDescription.Example) #</span>' +
                      '<span class="pencil-icon"><input type="button" class="icon edit-field glIntegrationEditBtn"/></span></div>',
                //width: 180

            },
            {
                field: "GLDetailComment.Example",
                headerAttributes: { "class": "w200" },
                attributes: { "class": "w200" },
                title: glReferenceIntegrationGridColumns.headerDetailComment,
                hidden: referenceIntegrationGridUtil.isColumnHidden(glReferenceIntegrationGridColumns.headerDetailComment),
                template: '<div class="pencil-wrapper"><span class="pencil-txt">#= glIntegrationUtils.GetValue(GLDetailComment.Example) #</span>' +
                     '<span class="pencil-icon"><input type="button" class="icon edit-field glIntegrationEditBtn"/></span></div>',
                //width: 175
            }
        ],
        columnReorder: function (e) {
            GridPreferencesHelper.saveColumnOrder(e, '#gridGLReferenceIntegration', GLReferenceIntegrationUIGrid.RefrenceIntegrationGrid);
        },
    }
};

// open popup window on edit cell click
$("#gridGLReferenceIntegration").delegate("tbody > tr > td > div > span > input.glIntegrationEditBtn", "click", glIntegrationUI.initOpenPopup);
