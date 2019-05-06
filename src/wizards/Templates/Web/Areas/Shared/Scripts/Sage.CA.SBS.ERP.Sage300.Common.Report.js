/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

var reportsUI = {
    Init: function () {
        reportsUI.InitFinders();
        reportsUI.InitButton();
        reportsUI.initDatePicker();
    },

    initDatePicker: function () {
        var inputs = $('[data-ControlType="Calendar"]');
        inputs.each(function () {
            sg.utls.kndoUI.datePicker($(this).attr("id"));
        });
    },

    BaseUrl: $("#hdnReportBaseUrl").val(),

    InitFinders: function () {
        var inputs = $('[data-ControlType="Finder"]');
        inputs.each(function () {
            var data = $(this).attr("id");
            var finderId = "finder_" + data.split('_')[1];
            sg.finderHelper.setFinder(finderId, $(this).attr("data-FinderType"), reportsUI.PopulateFinder, null, $(this).attr("data_FinderDisplayName"), $.noop);
        });
    },

    InitButton: function () {
        $("#btnPrint").bind("click", function () {
            if ($("#frmReport").valid()) {
                reportsUI.Execute();
            }
        });
    },

    Execute: function () {
        var parameters = [];
        $("[data-ColumnName]").each(function () {
            if ($(this).is(':checkbox')) {
                parameters.push({
                    "Id": $(this).attr("data-ColumnName"),
                    "Value": $(this).is(':checked')
                });
            }
            else {
                parameters.push({
                    "Id": $(this).attr("data-ColumnName"),
                    "Value": $(this).val()
                });
            }
        });

        var report = {
            Parameters: parameters,
            Id: $("#Report_Id").val()
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Shared", "Report", "Execute"), report, function (result, status) {
            if (result != null && result.UserMessage.IsSuccess) {
                sg.utls.openReport(result.ReportToken);
            } else {
                sg.utls.showMessage(result);
            }
        });
    },

    PopulateFinder: function (result, id) {
        if (result != null) {
            var textId = "txt_" + id.split('_')[1];
            $("#" + textId).val(result[$("#" + textId).attr("propertyId")]);
        }
    }
};

$(function () {
    reportsUI.Init();
});




