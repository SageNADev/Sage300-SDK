/* Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved. */
"use strict";
var progressUI = function () {
    let defaultProgressUrl;
    let defaultCancelUrl;
    let currentCancelUrl;
    let defaultOnProcessComplete;
    let currentOnProcessComplete;
    let abortPollRequest = false;
    let showResult = false; // On successful completion of the process, the result is for showing to the user (as opposed to being for post-processing)
    const onSuccess = function (jsonResult) {
        const processResult = progressUI.progressUIModel.ProcessResult;
        window.ko.mapping.fromJS(jsonResult.ProcessResult, {}, processResult);
        const error = processResult.ProcessStatus() == 3;
        const completed = processResult.ProcessStatus() == 2;
        if (error || completed) {
            abortPollRequest = true;
            if (showResult || error) {
                $(".k-window-action").show();
                const resultList = processResult.Results();
                if (resultList && resultList.length > 0) {
                    if (resultList.length == 1) {
                        const resultEntry = resultList[0];
                        window.sg.utls.showProcessMessageInfo(resultEntry.Priority(), resultEntry.Message(), 'messageDiv');
                    } else if (progressUI.isMultipleProcessInvolved) {
                        const userMessage = sg.utls.convertEntityErrorsToUserMessage(jsonResult.ProcessResult.Results);
                        sg.utls.showMessageInEnumerableResponse(userMessage, "#messageDiv", false);
                        window.top.sg.utls.isMultipleProcessInvolved = false; // Probably obsolete. If someone knows what this is supposed to achieve, please let us know
                    } else {
                        $(".processingResultGrid-wrapper").show(); //wrapper for "processingResultGrid"
                        //Binding the data again in case of env. not binding the model properly.
                        //This is a work around and it makes sure its bound, but still need to find out why the data is not bound properly in case of processing grid.
                        $("#processingResultGrid").data("kendoGrid").dataSource.data(jsonResult.ProcessResult.Results);
                        $("#processingResultGrid").show();
                    }
                }
            } else {
                // If the caller wants the result for processing then auto-close this progress UI.
                const sWin = $("#statusWindow");
                if (sWin) {
                    sWin.hide();
                    sWin.kendoWindow("close");
                }
                $("#processingResultGrid").hide();
            }
            if (currentOnProcessComplete != null || currentOnProcessComplete!=undefined) {
                currentOnProcessComplete(jsonResult);
            }
        }
        let percentageValue = 0;
        if (processResult.ProgressMeter && processResult.ProgressMeter.Percent) {
            percentageValue = processResult.ProgressMeter.Percent();
        }
        sg.utls.progressBarControl("#progressBarForProcessing", percentageValue);
    };
    const resetMeter = function () {
        if (progressUI.progressUIModel && progressUI.progressUIModel.ProcessResult) {
            const meter = progressUI.progressUIModel.ProcessResult.ProgressMeter;
            if (meter) {
                meter.Caption("");
                meter.Label("");
                meter.Percent(0);
            }
        }
    };
    return {
        // Some UIs currently set progressUIModel into their model, before passing it to progressUI.init
        // I do not know why.
        progressUIModel: {},
        // Some code sets this true, to get different processing of results.
        isMultipleProcessInvolved: false,
        // The standard function called by any UI that wishes to use the progress UI
        init: function (urlProgress, urlCancel, model, title, onProcessComplete) {
            $("#processingResultGrid").hide();
            defaultProgressUrl = urlProgress;
            defaultCancelUrl = urlCancel;
            defaultOnProcessComplete = onProcessComplete;
            progressUI.progressUIModel = model;
            progressUI.initKendoWindow(title);
            progressUI.initButtons();
        },
        // a couple of places call this directly. I do not know why.
        initKendoWindow: function (title) {
            kendo.ui.Window.fn._keydown = function (originalFn) {
                const KEY_ESC = 27;
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
                //custom function to support focus within kendo window
                activate: sg.utls.kndoUI.onActivate
            }).data("kendoWindow").center();
        },
        // A couple of places call this directly, for special processing.
        onKendoWindowClose: function () {
            $(".processingResultGrid-wrapper").hide(); //wrapper for "processingResultGrid"
            $("#processingResultGrid").hide();
            $("#messageDiv").hide();
            resetMeter();
        },
        initButtons: function () {
            $('#btnCancel').click(function () {
                progressUI.cancel();
            });
        },
        progressUow: function (module, controller, onProcessComplete, jsonResult, hideResult) {
            window.ko.mapping.fromJS(jsonResult.WorkflowInstanceId, {}, progressUI.progressUIModel.WorkflowInstanceId);
            const progressUrl = window.sg.utls.url.buildUrl(module, controller, "Progress");
            const cancelUrl = window.sg.utls.url.buildUrl(module, controller, "Cancel");
            progressUI.progress(progressUrl, cancelUrl, onProcessComplete, hideResult);
        },
        // The standard polling function
        progress: function (progressUrl, cancelUrl, onProcessComplete, hideResult) {
            if (!progressUrl) {
                progressUrl = defaultProgressUrl;
            }
            if (cancelUrl) {
                currentCancelUrl = cancelUrl;
            } else {
                currentCancelUrl = defaultCancelUrl;
            }
            if (onProcessComplete) {
                currentOnProcessComplete = onProcessComplete;
            } else {
                currentOnProcessComplete = defaultOnProcessComplete;
            }
            showResult = !hideResult;
            abortPollRequest = false;
            sg.utls.progressBarControl("#progressBarForProcessing", 0);
            $(".processingResultGrid-wrapper").hide(); //wrapper for "processingResultGrid"
            $("#processingResultGrid").hide();
            $("#message").hide();
            $("#statusWindow").show();
            $("#statusWindow").kendoWindow("open");
            $(".k-window-action").hide();
            const data = { tokenId: progressUI.progressUIModel.WorkflowInstanceId() };
            window.sg.utls.recursiveAjaxPost(progressUrl, data, onSuccess, function () {return abortPollRequest;});
        },
        // The standard cancel function is not supposed to be called directly, but I cannot currently make it private; doing so messes up the UI rendering
        cancel: function () {
            const data = { tokenId: progressUI.progressUIModel.WorkflowInstanceId() };
            window.sg.utls.ajaxPost(currentCancelUrl, data, function (jsonResult) {console.log("Process is cancelled successfully");});
        },
        // The grid options, referenced by _ProcessingStatus.cshtml
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
    }
}();