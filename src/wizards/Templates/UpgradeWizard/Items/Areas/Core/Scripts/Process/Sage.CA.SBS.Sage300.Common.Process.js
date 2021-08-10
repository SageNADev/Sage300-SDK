/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

var progressUI = progressUI || {};
progressUI = {
    progressUIModel: {},
    progressUrl: "",
    cancelUrl: "",
    abortPollRequest: false,
    onProcessComplete: null,
    isMultipleProcessInvolved: false,

    init: function (urlProgress, urlCancel, model, title, onProcessComplete) {
        $("#processingResultGrid").hide();
        progressUI.cancelUrl = urlCancel;
        progressUI.progressUrl = urlProgress;
        progressUI.progressUIModel = model;
        progressUI.onProcessComplete = onProcessComplete;
        progressUI.initKendoWindow(title);
        progressUI.initButtons();
    },
    initKendoWindow: function (title) {
        kendo.ui.Window.fn._keydown = function (originalFn) {
            var KEY_ESC = 27;
            return function (e) {
                //Disable ESC key for processing popup
                if (e.currentTarget.id !== "statusWindow" && this.element[0].id !== "statusWindow" || e.which !== KEY_ESC) {
                    originalFn.call(this, e);
                }
            };
        }(kendo.ui.Window.fn._keydown);
        window.sg.utls.intializeKendoWindowPopup('#statusWindow', title);
        $("#statusWindow").kendoWindow({
            modal: true,
            actions: ["Close"],
            close: function () {
                progressUI.onKendoWindowClose();
            },
            //Open Kendo Window in center of the Viewport. Also set title bar color
            open: sg.utls.kndoUI.onOpen,
            //custom function to suppot focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        }).data("kendoWindow").center();
    },
    onKendoWindowClose: function () {
        $(".processingResultGrid-wrapper").hide(); //wrapper for "processingResultGrid"
        $("#processingResultGrid").hide();
        $("#messageDiv").hide();
    },
    initButtons: function () {
        $('#btnCancel').click(function () {
            progressUI.cancel();
        });
    },
    progress: function () {
        progressUI.abortPollRequest = false;
        sg.utls.progressBarControl("#progressBarForProcessing", 0);
        $(".processingResultGrid-wrapper").hide(); //wrapper for "processingResultGrid"
        $("#processingResultGrid").hide();
        $("#message").hide();
        $("#statusWindow").show();
        $("#statusWindow").kendoWindow("open");
        $(".k-window-action").hide();
        var data = { tokenId: progressUI.progressUIModel.WorkflowInstanceId() };
        window.sg.utls.recursiveAjaxPost(progressUI.progressUrl, data, progressUIOnSuccess.progress, progressUIOnSuccess.abort);
    },

    cancel: function () {
        var data = { tokenId: progressUI.progressUIModel.WorkflowInstanceId() };
        window.sg.utls.ajaxPost(progressUI.cancelUrl, data, progressUIOnSuccess.cancel);
    },
    gridOption: {
        scrollable: false,
        sortable: false,
        pageable: false,
        editable: false,
        selectable: true,
        resizable: true,
        columns: [
            { title: globalResource.Index, template: "#= ++rowNumber   #", width: 30 },
            { field: "PriorityString", title: globalResource.Priority },
            { field: "Message", title: globalResource.Description, width: 600 }
        ],
        dataBinding: function () {
            rowNumber = 0;
        }
    }
};
var progressUIOnSuccess = {
    progress: function (jsonResult) {
        var model = progressUI.progressUIModel;
        window.ko.mapping.fromJS(jsonResult.ProcessResult, {}, model.ProcessResult);
        if (model.ProcessResult.ProcessStatus() == 2 || model.ProcessResult.ProcessStatus() == 3) { //Error or Completed
            progressUI.abortPollRequest = true;
            $(".k-window-action").show();
            if (progressUI.progressUIModel.ProcessResult.Results().length == 1) {
                var messageType = progressUI.progressUIModel.ProcessResult.Results()[0].Priority();
                var message = progressUI.progressUIModel.ProcessResult.Results()[0].Message();
                window.sg.utls.showProcessMessageInfo(messageType, message, 'messageDiv');
            } else if (progressUI.isMultipleProcessInvolved == true) {
                var userMessage = sg.utls.convertEntityErrorsToUserMessage(ko.mapping.toJS(progressUI.progressUIModel.ProcessResult.Results));
                sg.utls.showMessageInEnumerableResponse(userMessage, "#messageDiv", false);
                window.top.sg.utls.isMultipleProcessInvolved = false;
            } else {
                $(".processingResultGrid-wrapper").show(); //wrapper for "processingResultGrid"

                //Binding the data again in case of env. not binding the model properly.
                //This is a work around and it makes sure its binded, but still need to find out why the data is not binded properly in case of processing grid.
                $("#processingResultGrid").data("kendoGrid").dataSource.data(jsonResult.ProcessResult.Results);
                $("#processingResultGrid").show();
            }
            if (progressUI.onProcessComplete != null || progressUI.onProcessComplete!=undefined) {
                progressUI.onProcessComplete(jsonResult);
            }
        }
        var percentageValue = model.ProcessResult.ProgressMeter.Percent();
        sg.utls.progressBarControl("#progressBarForProcessing", percentageValue);
    },
    cancel: function (jsonResult) {
        console.log("Process is cancelled successfully");
    },
    abort: function () {
        return progressUI.abortPollRequest;
    }

};