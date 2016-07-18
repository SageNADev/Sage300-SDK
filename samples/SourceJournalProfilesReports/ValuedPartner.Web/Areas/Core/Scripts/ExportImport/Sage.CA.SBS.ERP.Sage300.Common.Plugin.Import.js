/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */
"use strict";
var kendoWindow = null;
(function (sg, $) {
    sg.importHelper = {
        importModel: {},
        abortPolling: false,
        gridOption: {
            scrollable: false,
            sortable: false,
            pageable: false,
            editable: false,
            selectable: true,
            resizable: true,
            columns: [
                { title: globalResource.Index, template: "#= ++importResultRowNumber   #", width: 30 },
                { field: "PriorityString", title: globalResource.Priority },
                { field: "Message", title: globalResource.Description, width: 600, template: "#= sg.utls.formatMessageText(Message) #" }
            ],
            dataBinding: function () {
                importResultRowNumber = 0;
            }
        },
        setImportEvent: function (id, importName, hasOptions, importKeys, callbackFunc) {
            $("#" + id).Import({
                name: importName,
                exportImportOptions: hasOptions,
                keys: importKeys,
                ok: callbackFunc
            });
        },
        showImportResult: function (that) {
            sg.importHelper.abortPolling = true;
            var model = sg.importHelper.importModel;
            $("#importResult").show();
            $("#importMessageDiv").hide();
            $(".k-window-action").show();

            if (model.ImportResponse.Results().length == 1) {
                var messageType = model.ImportResponse.Results()[0].Priority();
                var message = model.ImportResponse.Results()[0].Message();
                window.sg.utls.showProcessMessageInfo(messageType, message, 'importResultMessageDiv');
            } else {
                $("#resultgrid").show();
            }
            $("#btnClose").show();
        }
    };

}(sg = sg || {}, jQuery));

(function ($, window, document, undefined) {
    $.widget("sageuiwidgets.Import", {
        divImportDialogId: '',

        options: {
            id: "",
            name: "",
            title: globalResource.ImportWindowTitle,
            exportImportOptions: false,
            keys: $.noop,
            ok: $.noop,
            cancel: $.noop,
            fileName: "",
            modelData: {}
        },

        _create: function () {
            var that = this;
            $(that.element).bind('click', function () {
                that._doAjax(that);
            });
        },

        _doAjax: function (that) {
            //If import options are not present
            if (that.options.exportImportOptions == false) {
                that._setUpImportWindow(that);
            } else {
                that._setUpExportImportOptionWindow(that);
            }
        },

        _setUpExportImportOptionWindow: function (that) {
            var data = {
                viewModel: { Name: that.options.name, ImportRequest: { Name: that.options.name } }
            };

            that.divImportDialogId = 'div_' + that.options.name + '_optionDialog';
            $('<div id="' + that.divImportDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divImportDialogId;

            kendoWindow = $(dialogId).kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: false,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: 820,
                minHeight: 100,
                maxHeight: 600,
                actions: ["Close"],
                close: function () {
                    that._destroyKendoWindow();
                },
                //Open Kendo Window in center of the Viewport
                open: function () {
                    sg.utls.setKendoWindowPosition(this);
                },
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate
            }).data("kendoWindow");

            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ExportImport", "ExportImportOptions"), data, function (successData) {
                $(dialogId).html(successData);

                //KO winding within a div (importScreen) not on DOM
                sg.importHelper.importModel = window.ko.mapping.fromJS(exportImportModelData);
                window.ko.applyBindings(sg.importHelper.importModel, $("#exportImportOptionScreen")[0]);

                window.sg.utls.kndoUI.dropDownList("exportImportOption");

                $(document).off('click', '#frmImportOptions');
                $(document).on('click', '#frmImportOptions', function () {
                    that._destroyKendoWindow();
                    that._setUpImportWindow(that, ko.mapping.toJS(sg.importHelper.importModel));
                });
            });

            //first Center and then open the window
            kendoWindow.open();
        },

        _setUpImportWindow: function (that, viewModel) {
            if (viewModel == undefined) {
                viewModel = { Name: that.options.name, ImportRequest: { Name: that.options.name } };
            }

            var data = {
                viewModel: viewModel
            };

            that.divImportDialogId = 'div_' + that.options.name + '_dialog';
            $('<div id="' + that.divImportDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divImportDialogId;

            kendoWindow = $(dialogId).kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: false,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: 647,
                minHeight: 220,
                maxHeight: 400,
                actions: ["Close"],
                close: function () {
                    that._destroyKendoWindow();
                },
                //Open Kendo Window in center of the Viewport
                open: function () {
                    sg.utls.setKendoWindowPosition(this);
                },
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate
            }).data("kendoWindow");

            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ExportImport", "ImportIndex"), data, function (successData) {
                that._showImportScreen(that, successData, dialogId);
            });
        },

        _showImportScreen: function (that, data, dialogId) {
            $(dialogId).html(data);

            //clear all events;
            $(document).off('click.plugin.import');
            $(document).off('change.plugin.import');

            sg.importHelper.importModel = window.ko.mapping.fromJS(importModelData);
            window.ko.applyBindings(sg.importHelper.importModel, $("#importScreen")[0]);

            kendoWindow.open();

            window.sg.utls.kndoUI.dropDownList("FileTypes");
            window.sg.utls.kndoUI.dropDownList("ImportTypes");

            $("#hiddenFileUploadButton").on('click.plugin.import', function () {
                that._doImport(that);
            });

            $("#btnImportCancel").on('click.plugin.import', function () {
                that._destroyKendoWindow();
            });

            $("#btnImportOk").on('click.plugin.import', function () {
                that._destroyKendoWindow();
                if (that.options.ok !== $.noop) {
                    that.options.ok.call();
                }
            });

            $(document).on('change.plugin.import', '#btnFileImport', function (e) {
                var files = e.target.files;
                var selectedFIle = files[0];
                $('#uploadFileTextBox').val(selectedFIle.name);
                sg.utls.clearValidations("frmImport");
            });

        },

        _destroyKendoWindow: function (e) {
            kendoWindow.destroy();
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _doImport: function (that) {
            sg.importHelper.abortPolling = false;
            if (that.options.keys != $.noop) {
                sg.importHelper.importModel.ImportRequest.Keys(that.options.keys.call());
            }
            sg.importHelper.importModel.Name(that.options.name);
            sg.importHelper.importModel.ImportRequest.FileName(uplodeUI.fileName);
            var data = { viewModel: ko.mapping.toJS(sg.importHelper.importModel) };
            $("#uploadResult").hide();
            $("#importResult").show();
            $("#btnClose").hide();
            $(".k-window-action").hide();
            sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "Import"), data, function (result) {
                ko.mapping.fromJS(result.Data.ImportResponse, {}, sg.importHelper.importModel.ImportResponse);
                var data = { viewModel: ko.mapping.toJS(sg.importHelper.importModel) };
                window.sg.utls.recursiveAjaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "ImportProgress"), data, that._progress, that._abort);
            });
        },
        _progress: function (result) {
            ko.mapping.fromJS(result.ImportResponse, {}, sg.importHelper.importModel.ImportResponse);
            var model = sg.importHelper.importModel;

            if (model.ImportResponse.Status() === 2 || model.ImportResponse.Status() === 3) { //Error or Completed
                sg.importHelper.showImportResult();
            }
        },
        _abort: function (that) {
            return sg.importHelper.abortPolling;
        }
    });

})(jQuery, window, document);