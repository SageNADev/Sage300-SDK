/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.GridPreferences.js" />
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.KendoHelpers.js" />
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.Cache.js" />
/// <reference path="../TestUtil/GlobalTestUtils.js" />
/// <reference path="../../Areas/Shared/Scripts/Sage.CA.SBS.ERP.Sage300.Common.Global.js" />
/// <reference path="../TestUtil/FiscalCalendarTestUtils.js" />

/* Target scripts*/
/// <reference path="../../Areas/Core/Scripts/Sage.CA.SBS.ERP.Sage300.Common.FiscalCalendar.js"/>

describe("FiscalCalendar.js tests", function () {

    var textBox = $(testCalendarInputField);
    var button = $(testCalendarButton);
    var appId = "AP";
    var year = "2020";
    var select = function (result) {
        selectResult = result;
    };
    var selectResult = null;

    beforeEach(function () {
        $(testCalendarHTMLStr).appendTo("body");

        sg.utls.ajaxPost = function (url, data, successMethod) {
            var result = {
                FiscalCalendarYears: fiscalCalendarYearsData,
                DataList: year2020Data,
                ShowStatusColumn: false
            };
            successMethod(result);
        };

        // init the calendar
        fiscalCalendarHelper.initialize(textBox, button, appId, year, select);
    });

    afterEach(function () {
        // something to clean up after test
        selectResult = null;
        $("#divFiscalCalGrid").empty();
    });

    it("Test FiscalCalendar.fiscalCalendarHeader", function () {
        expect(fiscalCalendarHeader.PeriodTitle).toBe(testHeaderData.period);
        expect(fiscalCalendarHeader.StartDateTitle).toBe(testHeaderData.startDate);
        expect(fiscalCalendarHeader.EndDateTitle).toBe(testHeaderData.endDate);
        expect(fiscalCalendarHeader.StatusTitle).toBe(testHeaderData.status);
    });

    it("Test FiscalCalendar.fiscalCalendarHelper.initialize", function () {
        // validate the result
        expect(fiscalCalendarHelper.selectedYear).toBe(year);
        expect(fiscalCalendarHelper.appId).toBe(appId);
        expect(fiscalCalendarHelper.select).toBe(select);

        // initial selected value should be the one on the list with FiscalYear of previously defined (2020)
        expect(fiscalCalendarHelper.optionsCount).toBe(fiscalCalendarYearsData.indexOf($.grep(fiscalCalendarYearsData, function (e) { return e.FiscalYear === year; })[0]));

        // ShowStatusColumn is set to false, so it should be hidden
        expect(fiscalCalendarUtils.hideColumns.length).toBe(1);
        expect(fiscalCalendarUtils.hideColumns[0]).toBe("Status");

        // check status in fiscalCalendarGrid.columns is hidden (!!!!Note: currently it is broken as the code is wrong, it will always return false)
        expect($.grep(fiscalCalendarGrid.columns, function (e) { return e.field === "Status" })[0].hidden).toBeFalsy();

        // index 3 is the status column's index
        expect($("#divFiscalCalGrid").data("kendoGrid").columns[3].attributes['sg_Customizable']).toBeFalsy();

        // make sure the buttons are in proper state
        expect($("#btnNextFiscal").prop('disabled')).toBeTruthy();
        expect($("#btnPreviousFiscal").prop('disabled')).toBeFalsy(false);
    });

    it("Test FiscalCalendar.fiscalCalendarUtils.next (without request)", function () {
        var oldCount = fiscalCalendarHelper.optionsCount;

        fiscalCalendarUtils.next();

        expect(fiscalCalendarHelper.optionsCount).toBe(oldCount + 1);

        // make sure the buttons are in proper state
        expect($("#btnNextFiscal").prop('disabled')).toBeTruthy();
        expect($("#btnPreviousFiscal").prop('disabled')).toBeFalsy();
    });

    it("Test FiscalCalendar.fiscalCalendarUtils.next (with request)", function () {
        fiscalCalendarHelper.optionsCount -= 2;

        sg.utls.ajaxPost = function (url, data, successMethod) {
            var result = {
                FiscalCalendarYears: fiscalCalendarYearsData,
                DataList: year2019Data,
                ShowStatusColumn: false
            };
            successMethod(result);
        };

        fiscalCalendarUtils.next();

        expect($("#ddlFiscalYear").data("kendoDropDownList").value()).toBe("2019");
        expect($("#divFiscalCalGrid").data("kendoGrid").dataSource.data()[0].Year).toBe("2019");

        // make sure the buttons are in proper state
        expect($("#btnNextFiscal").prop('disabled')).toBeFalsy();
        expect($("#btnPreviousFiscal").prop('disabled')).toBeFalsy();
    });

    it("Test FiscalCalendar.fiscalCalendarUtils.previous", function () {

        sg.utls.ajaxPost = function (url, data, successMethod) {
            var result = {
                FiscalCalendarYears: fiscalCalendarYearsData,
                DataList: year2019Data,
                ShowStatusColumn: false
            };
            successMethod(result);
        };

        fiscalCalendarUtils.previous();

        expect(fiscalCalendarHelper.optionsCount).toBe(9);

        expect($("#ddlFiscalYear").data("kendoDropDownList").value()).toBe("2019");
        expect($("#divFiscalCalGrid").data("kendoGrid").dataSource.data()[0].Year).toBe("2019");

        // make sure the buttons are in proper state
        expect($("#btnNextFiscal").prop('disabled')).toBeFalsy();
        expect($("#btnPreviousFiscal").prop('disabled')).toBeFalsy();
    });

    it("Test FiscalCalendar.fiscalCalendarGrid.onChange", function () {
        var grid = $("#divFiscalCalGrid").data("kendoGrid");
        var lastRowIndex = grid.dataSource.view().length - 1;

        // This will trigger the onChange call
        grid.select(grid.tbody.find(">tr:eq(" + lastRowIndex + ")"));

        expect(selectResult).not.toBeNull();
        expect(selectResult.Period).toBe("12");
        expect(selectResult.Year).toBe("2020");
    });
});