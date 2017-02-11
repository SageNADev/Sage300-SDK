// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

"use strict";

var fiscalCalendarHelper = fiscalCalendarHelper || {};
var fiscalCalendarUtils = fiscalCalendarUtils || {};
var fiscalCalendarDropDown = fiscalCalendarDropDown || {};
var fiscalCalendarGrid = fiscalCalendarGrid || {};

fiscalCalendarHelper = {
    grid: null,
    appId: null,
    selectedYear: null,
    showStatusColumn: true,
    select: $.noop,
    optionsCount: 0,
    params: function (appId, year) {
        return {
            appId: appId,
            year: year
        };
    },
    enableDisableNevigation: function () {
        $("#btnPreviousFiscal").removeAttr('disabled');
        $("#btnNextFiscal").removeAttr('disabled');

        if (fiscalCalendarHelper.optionsCount == 0) {
            $("#btnPreviousFiscal").attr('disabled', true);
        } else {
            var countOfItem = $("#ddlFiscalYear").data("kendoDropDownList").dataSource.data().length;
            if (fiscalCalendarHelper.optionsCount == countOfItem - 1) {
                $("#btnNextFiscal").attr('disabled', true);
            }
        }
    },
    initialize: function (textbox, button, appId, year, select) {
        fiscalCalendarUtils.hide();
        var dropdownlist = $("#ddlFiscalYear").data("kendoDropDownList");
        if (typeof dropdownlist !== 'undefined') {
            dropdownlist.destroy();
        }
        var grid = $("#divFiscalCalGrid").data("kendoGrid");
        if (typeof grid !== 'undefined') {
            grid.destroy();
        }

        $(document).off('.fiscalCalCtrl');

        $(document).on('click.fiscalCalCtrl', function (e) {
            var container = $('#divFiscalCalendar');
            var ddlFiscalCal = $('#ddlFiscalYear-list');
            // if the target of the click isn't the container... nor a descendant of the container
            if (!container.is(e.target) && !button.is(e.target) && container.has(e.target).length === 0 && !ddlFiscalCal.is(e.target) && e.target.id !== "ddlFiscalYear_option_selected") {
                container.hide();
            }
        });

        var data = fiscalCalendarHelper.params(appId, year);
        fiscalCalendarHelper.selectedYear = year;
        fiscalCalendarHelper.appId = appId;
        fiscalCalendarHelper.select = select;
        fiscalCalendarHelper.initButton();
        fiscalCalendarUtils.get(data);
        fiscalCalendarUtils.show(textbox, button);
    },
    initButton: function () {
        $("#btnPreviousFiscal").unbind('click.fiscalCalCtrl');
        $("#btnPreviousFiscal").bind('click.fiscalCalCtrl', function () {
            fiscalCalendarUtils.previous();
        });
        $("#btnNextFiscal").unbind('click.fiscalCalCtrl');
        $("#btnNextFiscal").bind('click.fiscalCalCtrl', function () {
            fiscalCalendarUtils.next();
        });
    }
};

fiscalCalendarUtils = {
    hideColumns: [],
    next: function () {
        fiscalCalendarHelper.optionsCount++;
        var optionsCount = fiscalCalendarDropDown.get();
        if (fiscalCalendarHelper.optionsCount < optionsCount) {
            fiscalCalendarHelper.selectedYear = $("#ddlFiscalYear").data("kendoDropDownList").dataItem(fiscalCalendarHelper.optionsCount).FiscalYear;
            var data = fiscalCalendarHelper.params(fiscalCalendarHelper.appId, fiscalCalendarHelper.selectedYear.toString());
            fiscalCalendarUtils.post(data);
        } else {
            fiscalCalendarHelper.optionsCount = optionsCount;
        }
    },
    previous: function () {
        fiscalCalendarHelper.optionsCount--;
        if (fiscalCalendarHelper.optionsCount >= 0) {
            fiscalCalendarHelper.selectedYear = $("#ddlFiscalYear").data("kendoDropDownList").dataItem(fiscalCalendarHelper.optionsCount).FiscalYear;
            var data = fiscalCalendarHelper.params(fiscalCalendarHelper.appId, fiscalCalendarHelper.selectedYear.toString());
            fiscalCalendarUtils.post(data);
        }
        else {
            fiscalCalendarHelper.optionsCount = 0;
        }
    },

    intiControls: function (result) {
        if (result != null) {
            fiscalCalendarDropDown.init(result);
            fiscalCalendarGrid.init(result);
            fiscalCalendarHelper.enableDisableNevigation();
        }
    },
    get: function (data, isInit) {
        fiscalCalendarUtils.Call("GetFiscalYearSet", data, fiscalCalendarUtils.intiControls);
    },
    post: function (data) {
        fiscalCalendarUtils.Call("GetFiscalYearSet", data, fiscalCalendarUtils.success);
    },
    Call: function (methodName, data, successMethod) {
        var url = sg.utls.url.buildUrl("CS", "FiscalCalendar", methodName);
        sg.utls.ajaxPost(url, data, successMethod);
    },
    success: function (result) {
        fiscalCalendarDropDown.update(fiscalCalendarHelper.selectedYear);
        fiscalCalendarGrid.update(result.DataList);
        fiscalCalendarHelper.enableDisableNevigation();
    },
    hide: function () {
        $('#divFiscalCalendar').hide();
    },
    show: function (textbox, button) {
        var container = $('#divFiscalCalendar');
        // setting minimum height for first time display, as the content is loading after displaying the calendar.
        container.height(447);
        var buttonOffset = button.offset();
        var top = buttonOffset.top + button.outerHeight();
        var divWidth = container.width();
        var textboxLeft = textbox.offset().left;

        if (buttonOffset.top > container.height()) {
            top = buttonOffset.top - container.height();
        }
        if ((textboxLeft + divWidth) > 960) {
            textboxLeft = textboxLeft - ((textboxLeft + divWidth) - (buttonOffset.left + 30));
        }
        container.removeAttr("style");
        container.css({ top: top, left: textboxLeft, position: 'absolute', "z-index": "2147483647" });
        container.show();
    }
};

fiscalCalendarDropDown = {
    init: function (result) {
        $("#ddlFiscalYear").kendoDropDownList({
            dataTextField: "FiscalYear",
            dataValueField: "FiscalYear",
            dataSource: result.FiscalCalendarYears,
            value: fiscalCalendarHelper.selectedYear,
            change: fiscalCalendarDropDown.change
        });

        var dropdownlist = $("#ddlFiscalYear").data("kendoDropDownList");
        fiscalCalendarHelper.optionsCount = dropdownlist.select();
    },
    get: function (data) {
        var dropdownlist = $("#ddlFiscalYear").data("kendoDropDownList");
        return dropdownlist.dataSource.total();
    },
    update: function (data) {
        var dropdownlist = $("#ddlFiscalYear").data("kendoDropDownList");
        dropdownlist.value(data);
    },
    change: function (e) {
        var data = {
            appId: fiscalCalendarHelper.appId,
            year: this.value()
        };
        fiscalCalendarHelper.optionsCount = e.sender.selectedIndex;
        fiscalCalendarHelper.selectedYear = this.value();
        fiscalCalendarUtils.post(data);
    }
};

var fiscalCalendarHeader = {
    PeriodTitle: $(fiscalControlGridColumns.headerPeriod).text(),
    StartDateTitle: $(fiscalControlGridColumns.headerStartDate).text(),
    EndDateTitle: $(fiscalControlGridColumns.headerEndDate).text(),
    StatusTitle: $(fiscalControlGridColumns.headerStatus).text()
};

fiscalCalendarGrid = {
    columns: [
        {
            field: "Period",
            title: fiscalCalendarHeader.PeriodTitle,
            attributes: " class = w80",
            headerAttributes: "class =  w80",
            hidden: false
        },
        {
            field: "StartDate",
            title: fiscalCalendarHeader.StartDateTitle,
            template: '#= sg.utls.kndoUI.getFormattedDate(StartDate)#',
            attributes: " class = w110",
            headerAttributes: "class = w110",
            hidden: false
        },
        {
            field: "EndDate",
            title: fiscalCalendarHeader.EndDateTitle,
            template: '#= sg.utls.kndoUI.getFormattedDate(EndDate)#',
            attributes: " class = w110",
            headerAttributes: "class = w110",
            hidden: false
        },
        {
            field: "Status",
            title: fiscalCalendarHeader.StatusTitle,
            attributes: " class = w110",
            headerAttributes: "class = w110",
            hidden: false
        }
    ],
    init: function (result) {
        var dataSource = new kendo.data.DataSource({
            data: result.DataList,
            schema: {
                model: {
                    fields: {
                        Period: { type: "string" },
                        StartDate: { type: "Date" },
                        EndDate: { type: "Date" },
                        Status: { type: "string" }
                    }
                }
            }
        });

        if (!result.ShowStatusColumn) {
            fiscalCalendarUtils.hideColumns = ["Status"];
        }

        var columns = fiscalCalendarGrid.getColumns(fiscalCalendarUtils.hideColumns, result.ShowStatusColumn);

        $('#divFiscalCalGrid').kendoGrid({
            resizable: true,
            scrollable: true,
            sortable: false,
            pageable: false,
            selectable: true,
            dataSource: dataSource,
            columns: columns,
            change: fiscalCalendarGrid.onChange
        });
        if (!result.ShowStatusColumn) {
            sg.utls.kndoUI.hideGridColumns($("#divFiscalCalGrid").data("kendoGrid"), fiscalCalendarUtils.hideColumns);
        }
    },
    onChange: function (arg) {
        fiscalCalendarGrid.getSelectedRow(fiscalCalendarHelper.select);
    },
    update: function (data) {
        var grid = $('#divFiscalCalGrid').data("kendoGrid");
        grid.dataSource.data(data);
    },
    getColumns: function (columns, isHidden) {
        $.each(fiscalCalendarGrid.columns, function (index, value) {
            if ($.inArray(value.field, columns) > -1)
                fiscalCalendarGrid.columns[index].hidden = isHidden;
        });
        return fiscalCalendarGrid.columns;
    },
    getSelectedRow: function (select) {
        fiscalCalendarUtils.hide();
        var dataSelected = null;
        if ($('#divFiscalCalGrid')) {
            var grid = $("#divFiscalCalGrid").data("kendoGrid");
            dataSelected = sg.utls.kndoUI.getSelectedRowData(grid);
        }
        if (select != $.noop) {
            var number = dataSelected.Period;
            if (!isNaN(number)) {
                dataSelected.Period = number > 9 ? "" + number : "0" + number;
            }
            select(dataSelected);
        }
    }
};