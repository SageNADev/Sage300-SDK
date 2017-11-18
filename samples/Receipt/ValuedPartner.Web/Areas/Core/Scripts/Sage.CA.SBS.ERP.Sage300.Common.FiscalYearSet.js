// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

"use strict";

var fiscalyearSetUI = {
    appID: null,
    fiscalYearGlobal: null,
    updateFiscalYear: null,
    showStatusColumn: true,
    hideStatus: ["Status"],
    fiscalyearSetModel: ko.observableArray([]),
    initbuttons: function () {
        $("#btnPreviousFiscal").bind('click', function () {
            var data = {
                appId: fiscalyearSetUI.appID,
                year: parseInt(fiscalyearSetUI.updateFiscalYear) - 1
            }
            fiscalyearSetUI.updateFiscalYear = parseInt(fiscalyearSetUI.updateFiscalYear) - 1;
            window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl("CS", "FiscalCalendar", "GetFiscalYearSet"), data, OnSuccess.FiscalLoad)
        });

        $("#btnNextFiscal").bind('click', function () {
            var data = {
                appId: fiscalyearSetUI.appID,
                year: parseInt(fiscalyearSetUI.updateFiscalYear) + 1
            }
            fiscalyearSetUI.updateFiscalYear = parseInt(fiscalyearSetUI.updateFiscalYear) + 1;
            window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl("CS", "FiscalCalendar", "GetFiscalYearSet"), data, OnSuccess.FiscalLoad)
        });
    }
};

(function (sg, $) {
    fiscalyearSetUI.initbuttons();
    sg.fiscalHelper = {
        pageSize: 5,
        sortDir: false,
        cancelFuncCall: $.noop,
        fiscalYearToUpdate: null,
        setFiscalyear: function (id, appId, fiscalYear, selectedYear, selectedPeriod) {
            fiscalyearSetUI.appID = appId;
            fiscalyearSetUI.fiscalYearGlobal = fiscalYear;
            var data = {
                appId: appId,
                year: fiscalyearSetUI.fiscalYearGlobal
            }
            var fiscalData = {
                selectedYear: selectedYear,
                selectedPeriod: selectedPeriod
            }
            $("#" + id).FiscalyearSet({
                id: id,
                data: data,
                fiscalData: fiscalData
            });
        },
    };
}(this.sg = this.sg || {}, jQuery));

(function ($, window, document, undefined) {
    $.widget("sageuiwidgets.FiscalyearSet", {
        divFiscalDialogId: '',
        isFinderClicked: false,
        options: {
            select: $.noop,
            cancel: $.noop,
            title: "",
            id: "",
            data: $.noop,
            fiscalData: $.noop
        },
        _create: function () {
            var that = this;
            $(that.element).bind('click', function () {
                if (!that.isFinderClicked) {
                    that.isFinderClicked = true;
                    that._doAjax(that);
                }
            });
        },
        _doAjax: function (that) {
            fiscalyearSetUI.updateFiscalYear = fiscalyearSetUI.fiscalYearGlobal.call();
            var data = {
                appId: fiscalyearSetUI.appID,
                year: fiscalyearSetUI.updateFiscalYear
            };
            window.sg.utls.ajaxPost(window.sg.utls.url.buildUrl("CS", "FiscalCalendar", "GetFiscalYearSet"), data, function (successData) {
                that._showFiscalScreen(that, successData);
                that.isFinderClicked = false;
                if (!successData.IsPrevYearExist) {
                    $("#btnPreviousFiscal").enable(false);
                } else {
                    $("#btnPreviousFiscal").enable(true);
                }

                if (!successData.IsNextYearExist) {
                    $("#btnNextFiscal").enable(false);
                } else {
                    $("#btnNextFiscal").enable(true);
                }
                if (!successData.ShowStatusColumn) {
                    sg.utls.kndoUI.hideGridColumns($('#divFiscalGridControl').data("kendoGrid"), fiscalyearSetUI.hideStatus);
                } else {
                    sg.utls.kndoUI.showGridColumns($('#divFiscalGridControl').data("kendoGrid"), fiscalyearSetUI.hideStatus);
                }
            }),

          function (jqXhr, textStatus, errorThrown) {
              that.isFinderClicked = false;
              sg.utls.ajaxErrorHandler(jqXhr, textStatus, errorThrown);
          };
        },
        _showFiscalScreen: function (that, data) {
            ko.mapping.fromJS(fiscalyearSetUI.fiscalyearSetModel, data.FiscalYearSet);
            that.divFiscalDialogId = 'divFiscalCalender';
            var kendoWindow = $("#" + that.divFiscalDialogId).kendoWindow({
                // var kendoWindow = $(that.divFiscalDialogId).html("<span class='sage_loading'></span>").kendoWindow({
                modal: true,
                title: "Year",
                resizable: false,
                visible: false,
                navigatable: true,
                width: 0,
                minHeight: 0,
                maxHeight: 0,
                activate: sg.utls.kndoUI.onActivate
            }).data("kendoWindow");

            kendoWindow.bind("close", function () {
                that._triggerChange();
                kendoWindow.destroy();
                var cancel = that.options.cancel;
                if (cancel) {
                    cancel();
                }
            });

            kendoWindow.center();
            kendoWindow.open();
            var dataSource = new kendo.data.DataSource({
                serverPaging: true,
                serverFiltering: true,
                pageSize: 14,
                data: data.FiscalYearSet,

                schema: {
                    model: {
                        fields: {
                            Period: { type: "string" },
                            StartDate: { type: "Date" },
                            EndDate: { type: "Date" },
                            Status: { type: "string" },
                        }
                    },
                },
            });

            var FiscalControlHeader = {
                PeriodTitle: $(fiscalControlGridColumns.headerPeriod).text(),
                StartDateTitle: $(fiscalControlGridColumns.headerStartDate).text(),
                EndDateTitle: $(fiscalControlGridColumns.headerEndDate).text(),
                StatusTitle: $(fiscalControlGridColumns.headerStatus).text(),
            };

            var grid = $('#divFiscalGridControl').data("kendoGrid");
            $('#divFiscalGridControl').kendoGrid({
                scrollable: true,
                sortable: false,
                pageable: false,
                selectable: "row",
                dataSource: dataSource,
                pageSize: 14,
                columns:
                    [
                            {
                                field: "Period",
                                title: FiscalControlHeader.PeriodTitle,
                                attributes: " class = w80",
                                headerAttributes: "class =  w80",
                            },
                            {
                                field: "StartDate",
                                title: FiscalControlHeader.StartDateTitle,
                                template: '#= sg.utls.kndoUI.getFormattedDate(StartDate)#',
                                attributes: " class = w110",
                                headerAttributes: "class = w110",
                            },
                            {
                                field: "EndDate",
                                title: FiscalControlHeader.EndDateTitle,
                                template: '#= sg.utls.kndoUI.getFormattedDate(EndDate)#',
                                attributes: " class = w110",
                                headerAttributes: "class = w110",
                            },
                            {
                                field: "Status",
                                title: FiscalControlHeader.StatusTitle,
                                attributes: " class = w110",
                                headerAttributes: "class = w110",
                            },

                    ]
            });

            $("#select").on('click', function () {
                sg.fiscalHelper.cancelFuncCall = $.noop;
                sg.delayVariables.IsInProgress = false;
                that._getSelectedRow(that);
            });

            $("#divFiscalGridControl .k-grid-content").delegate("tbody>tr", "dblclick", function () {
                sg.fiscalHelper.cancelFuncCall = $.noop;
                that._getSelectedRow(that);
            });
        },

        _triggerChange: function () {
            if (sg.delayVariables.IsInProgress) {
                sg.delayVariables.IsInProgress = false;
                if (sg.delayVariables.RowData.Length > 0) {
                    var data = sg.delayVariables.RowData[sg.delayVariables.ColumnName];
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, "");
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, data);
                }
                if (sg.delayVariables.TextBoxElement) {
                    sg.delayVariables.TextBoxElement.change();
                }
            }
            if (sg.fiscalHelper.cancelFuncCall != $.noop) {
                sg.fiscalHelper.cancelFuncCall();
                sg.fiscalHelper.cancelFuncCall = $.noop;
            }
        },

        _selectGrid: function () {
            var grid, row, data;
            if ($('#divFiscalGridControl')) {
                grid = $('#divFiscalGridControl').data("kendoGrid");
                row = grid.select();
                data = grid.dataItem(row);
            }
            return data;
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _isNullOrUndefined: function (variable) {
            if (variable === null || typeof variable === "undefined") {
                return false;
            }
            return true;
        },

        _getSelectedRow: function (that) {
            var dataSelected = that._selectGrid();
            if (that._isNullOrUndefined(dataSelected)) {
                $('.selectKendoGrid').attr("disabled", false).removeClass("btnStyle2Disabled");
                var select = that.options.select;
                if (select) {
                    that.options.fiscalData.selectedYear(fiscalyearSetUI.updateFiscalYear);
                    that.options.fiscalData.selectedPeriod(dataSelected.Period);
                }
                var finderWin = $("#" + that.divFiscalDialogId).data("kendoWindow");
                if (finderWin != undefined) {
                    finderWin.destroy();
                }
            }
        },
    });
})(jQuery, window, document);

var OnSuccess = {
    FiscalLoad: function (successData) {
        $('#divFiscalGridControl').data("kendoGrid").dataSource.data(successData.FiscalYearSet);
        if (!successData.IsPrevYearExist) {
            $("#btnPreviousFiscal").enable(false);
        } else {
            $("#btnPreviousFiscal").enable(true);
        }

        if (!successData.IsNextYearExist) {
            $("#btnNextFiscal").enable(false);
        } else {
            $("#btnNextFiscal").enable(true);
        }
        if (!successData.ShowStatusColumn) {
            sg.utls.kndoUI.hideGridColumns($('#divFiscalGridControl').data("kendoGrid"), fiscalyearSetUI.hideStatus);
        } else {
            sg.utls.kndoUI.showGridColumns($('#divFiscalGridControl').data("kendoGrid"), fiscalyearSetUI.hideStatus);
        }
    }
};

sg.fiscalYearmodule = {
    GeneralLedger: "GL",
    CommonServices: "CS",
    AccountsPayable: "AP",
    AccountsReceivable: "AR",
    AdministrativeServices: "AS",
    InventoryControl: "IC"
};