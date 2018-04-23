// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

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