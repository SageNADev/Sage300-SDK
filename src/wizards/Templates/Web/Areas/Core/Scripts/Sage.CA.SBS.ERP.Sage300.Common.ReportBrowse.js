/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

/* global kendo */
/* global globalResource */
/* exported reportBrowseUI */

var reportBrowseUI = function () {
    var dlgBrowseHtml = "<div id='dlgReportBrowseList'><div id='gridReportList'></div><section class='footer-group'><input class='btn btn-primary' id='btnReportSelect' type='button' value='" + globalResource.Select + "'><input class='btn btn-secondary' id='btnReportCancel' type='button' value='" + globalResource.Cancel + "'></section></div>";
    var btnRptSelect, gridReport, dlgReportBrowse, reportDialogInitialized = false;

    function initReportBrowseDialog(elementId) {
        if (reportDialogInitialized) {
            return;
        }

        $("body").append(dlgBrowseHtml);
        dlgReportBrowse = $("#dlgReportBrowseList");
        gridReport = $("#gridReportList");
        btnRptSelect = $("#btnReportSelect");
        btnRptSelect.prop('disabled', true);

        btnRptSelect.click(function () {
            var grid = gridReport.data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (item) {
                $("#" + elementId).val(item.reportName.toUpperCase()).change();
            }
            dlgReportBrowse.data("kendoWindow").close();
        });

        $("#btnReportCancel").click(function () {
            dlgReportBrowse.data("kendoWindow").close();
        });

        dlgReportBrowse.kendoWindow({
            title: globalResource.SelectReport,
            height: '530px',
            width: '500px',
            draggable: true,
            modal: true,
            resizable: false,
            visible: false,

            animation: {
                open: {
                    effects: "slideIn:left fadeIn",
                    duration: 500
                },
                close: {
                    effects: "slideIn:left fadeIn",
                    reverse: true,
                    duration: 500
                }
            },

            open: function () {
                this.wrapper.css({ top: 100 });
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            }
        }).data("kendoWindow");
    }

    function gridDataBound(grid) {
        var page = grid.dataSource.totalPages();
        var pager = grid.pager.element;
        (page < 2) ? pager.hide() : pager.show();
    }

    function initReportListGrid(reportListData) {
        var grid = gridReport.kendoGrid({
            dataSource: {
                data: reportListData,
                schema: {
                    model: {
                        fields: {
                            reportName: { type: "string", editable: false }
                        }
                    }
                },
                pageSize: 10
            },
            //height: 380,
            filterMenuInit: onFilterMenuInit,
            filterable: { extra: false },
            pageable: {
                input: true,
                numeric: false
            },
            editable: false,
            selectable: "row",
            change: function () {
                btnRptSelect.prop("disabled", false);
            },
            columns: [
                {
                    title: globalResource.ReportName,
                    field: "reportName",
                    width: 300,
                    template: kendo.template("<span class='report-list'>#: reportName #</span>")
                }
            ]
        }).data("kendoGrid");
        //Add ids for filter menu
        function onFilterMenuInit(e) {
            $("button.k-button.k-primary:contains('Filter')").attr("id", "btnBrowseFilterId");
            $("button.k-button:contains('Clear')").attr("id", "btnBrowseClearId");
            $("input.k-textbox[data-bind='value:filters[0].value']").attr("id", "txtFilterValueId");
        }

        grid.bind("dataBound", function () {
            gridDataBound(this);
        }),

        grid.element.on("dblclick", "tbody>tr", function () {
            btnRptSelect.trigger("click");
        });

        btnRptSelect.prop("disabled", true);

        $("span.k-icon.k-i-filter").attr("id", "btnFilterHamburgerId");
        return grid;
    }

    function showReportBrowseDialog(result) {
        var isArray = result.constructor === Array;
        if (!isArray) {
            sg.utls.showMessage(result);
            return;
        }

        var rptFileNames = result.map(function (item) { return { reportName: item }; });

        var ds, grid = gridReport.data("kendoGrid");
        if (reportDialogInitialized) {
            ds = grid.dataSource;
            ds.filter([]);
            ds.data(rptFileNames);
            ds.page(1);
        } else {
            initReportListGrid(rptFileNames);
            reportDialogInitialized = true;
            grid = gridReport.data("kendoGrid");
            grid.dataSource.filter([]);
            gridDataBound(grid);
        }

        dlgReportBrowse.data("kendoWindow").center().open();
    }

    return {
        init: initReportBrowseDialog,
        show: showReportBrowseDialog
    };

}();
