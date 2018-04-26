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




