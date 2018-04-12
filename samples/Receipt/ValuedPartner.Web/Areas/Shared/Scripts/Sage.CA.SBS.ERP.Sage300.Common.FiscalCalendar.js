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

var fiscalCalHelper = fiscalCalHelper || {};

fiscalCalHelper = {
    grid: null,
    initialize: function (buttonId, appId, year) {
        var data = {
            appId: appId,
            year: year
        };

        sg.utls.ajaxPost(sg.utls.url.buildUrl("CS", "FiscalCalendar", "GetFiscalYearSet"), data, function (result) {
            fiscalCalHelper.initDropdown(result.FiscalYearSet);
            fiscalCalHelper.initGrid(result.FiscalYearSet);
        });
    },
    header: {
        PeriodTitle: $(fiscalControlGridColumns.headerPeriod).text(),
        StartDateTitle: $(fiscalControlGridColumns.headerStartDate).text(),
        EndDateTitle: $(fiscalControlGridColumns.headerEndDate).text(),
        StatusTitle: $(fiscalControlGridColumns.headerStatus).text()
    },
    columns: [
        {
            field: "Period",
            title: fiscalCalHelper.header.PeriodTitle,
            attributes: " class = w80",
            headerAttributes: "class =  w80",
        },
        {
            field: "StartDate",
            title: fiscalCalHelper.header.StartDateTitle,
            template: '#= sg.utls.kndoUI.getFormattedDate(StartDate)#',
            attributes: " class = w110",
            headerAttributes: "class = w110",
        },
        {
            field: "EndDate",
            title: fiscalCalHelper.header.EndDateTitle,
            template: '#= sg.utls.kndoUI.getFormattedDate(EndDate)#',
            attributes: " class = w110",
            headerAttributes: "class = w110",
        },
        {
            field: "Status",
            title: fiscalCalHelper.header.StatusTitle,
            attributes: " class = w110",
            headerAttributes: "class = w110",
        }
    ],
    initDropdown: function (data) {
        //$("#color").kendoDropDownList({
        //    dataTextField: "text",
        //    dataValueField: "value",
        //    dataSource: data,
        //    index: 0,
        //    change: onChange
        //});
    },
    initGrid: function (data) {
        var dataSource = new kendo.data.DataSource({
            data: data,
            schema: {
                model: {
                    fields: {
                        Period: { type: "string" },
                        StartDate: { type: "Date" },
                        EndDate: { type: "Date" },
                        Status: { type: "string" },
                    }
                },
            }
        });

        $('#divFiscalGridControl').kendoGrid({
            scrollable: true,
            sortable: false,
            pageable: false,
            selectable: true,
            dataSource: dataSource,
            columns: fiscalCalHelper.columns
        });
    }
};