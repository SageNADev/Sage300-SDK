// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

// @ts-check

"use strict";

var modelData;
var sourceCodeUI = sourceCodeUI || {};

sourceCodeUI = {
    sourceCodeModel: {},
    ignoreIsDirtyProperties: ["SourceLedger"],
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,
    sourceLedger: null,

    // Init
    init: function () {
        sourceCodeUI.initButtons();
        sourceCodeUI.initFinders();
        sourceCodeUISuccess.initialLoad(SourceCodeViewModel);
        sourceCodeUISuccess.setkey();
    },

    // Save
    saveSourceCode: function () {
        if ($("#frmSourceCode").valid()) {
            var data = sg.utls.ko.toJS(modelData, sourceCodeUI.computedProperties);
            if (modelData.UIMode() === sg.utls.OperationMode.SAVE) {
                sourceCodeRepository.update(data, sourceCodeUISuccess.update);
            } else {
                sourceCodeRepository.add(data, sourceCodeUISuccess.update);
            }
        }
    },

    // Init Buttons
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusourcecode", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusourcecode", false, $.noop);

        // Key field with Finder
        $("#txtSourceLedger").bind('blur', function (e) {
            modelData.SourceLedger($("#txtSourceLedger").val());
            sg.delayOnBlur("btnFinderSourceLedger", function () {
                if (sg.controls.GetString(modelData.SourceLedger() != "")) {
                    if (sg.controls.GetString(sourceCodeUI.sourceLedger) != sg.controls.GetString(modelData.SourceLedger())) {
                        sourceCodeUI.checkIsDirty(sourceCodeUI.get, sourceCodeUI.sourceLedger);
                    }
                }
            });
        });

        // Create New Button
        $("#btnNew").bind('click', function () {
            sourceCodeUI.checkIsDirty(sourceCodeUI.create, sourceCodeUI.sourceLedger);
        });

        // Save Button
        $("#btnSave").bind('click', function () {
            sg.utls.SyncExecute(sourceCodeUI.saveSourceCode);
        });

        // Delete Button
        $("#btnDelete").bind('click', function () {
            if ($("#frmSourceCode").valid()) {
                var message = jQuery.validator.format(sourceCodeResources.DeleteConfirmMessage, sourceCodeResources.SourceLedgerTitle, modelData.SourceLedger());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmSourceCode");
                    sourceCodeRepository.delete(modelData.SourceLedger(), sourceCodeUISuccess.delete);
                }, null, message, sourceCodeResources.DeleteTitle);
            }
        });

    },

    // Init Dropdowns here

    // Init Finders, if any
    initFinders: function () {
        var info = sg.viewFinderProperties.GL.SourceCodes;
        var buttonId = "btnFinderSourceLedger";
        var dataControlIdOrSuccessCallback = sourceCodeUISuccess.finderSuccess;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, sourceCodeFilter.getFilter);
    },

    // Get
    get: function () {
        sourceCodeRepository.get(modelData.SourceLedger(), sourceCodeUISuccess.get);
    },

    // Create
    create: function () {
        sg.utls.clearValidations("frmSourceCode");
        sourceCodeRepository.create(sourceCodeUISuccess.create);
    },

    // Is Dirty check
    checkIsDirty: function (functionToCall, sourceLedger) {
        if (sourceCodeUI.sourceCodeModel.isModelDirty.isDirty() && sourceLedger != null && sourceLedger != "") {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSourceCode");
                    functionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(sourceLedger) != sg.controls.GetString(modelData.SourceLedger())) {
                        modelData.SourceLedger(sourceLedger);
                   }
                   return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, sourceCodeResources.SourceLedgerTitle, sourceLedger));
        } else {
            functionToCall.call();
        }
    }

};

// Callbacks
var sourceCodeUISuccess = {

    // Setkey
    setkey: function () {
        sourceCodeUI.sourceLedger = modelData.SourceLedger();
    },

    // Get
    get: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data != null) {
                sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            } else {
                modelData.UIMode(sg.utls.OperationMode.NEW);
            }
            sourceCodeUISuccess.setkey();
        }
    },

    // Update
    update: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            sourceCodeUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    // Create
    create: function (jsonResult) {
        sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        sourceCodeUI.sourceCodeModel.isModelDirty.reset();
        sourceCodeUISuccess.setkey();
        sg.controls.Focus($("#txtSourceLedger"));
    },

    // Delete
    delete: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            sourceCodeUI.sourceCodeModel.isModelDirty.reset();
            sourceCodeUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    // Display Result
    displayResult: function (jsonResult, uiMode) {
        if (jsonResult != null) {
            if (!sourceCodeUI.hasKoBindingApplied) {
                sourceCodeUI.sourceCodeModel = ko.mapping.fromJS(jsonResult);
                sourceCodeUI.hasKoBindingApplied = true;
                modelData = sourceCodeUI.sourceCodeModel.Data;
                sourceCodeObservableExtension(sourceCodeUI.sourceCodeModel, uiMode);
                sourceCodeUI.sourceCodeModel.isModelDirty = new ko.dirtyFlag(modelData, sourceCodeUI.ignoreIsDirtyProperties);
                ko.applyBindings(sourceCodeUI.sourceCodeModel);
            } else {
                ko.mapping.fromJS(jsonResult, sourceCodeUI.sourceCodeModel);
                modelData.UIMode(uiMode);
                if (uiMode != sg.utls.OperationMode.NEW) {
                    sourceCodeUI.sourceCodeModel.isModelDirty.reset();
                }
            }

            if (!sourceCodeUI.isKendoControlNotInitialised) {
                sourceCodeUI.isKendoControlNotInitialised = true;
            } else {
                // 
            }
        }
    },

    // Initial Load
    initialLoad: function (result) {
        if (result) {
            sourceCodeUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtSourceLedger"));
    },

    // Finder Success
    finderSuccess: function (data) {
        if (data != null) {
            sourceCodeUI.finderData = data;
            sourceCodeUI.checkIsDirty(sourceCodeUISuccess.setFinderData, sourceCodeUI.sourceLedger);
        }
    },

    // Set Finder
    setFinderData: function () {
        var data = sourceCodeUI.finderData;
        sg.utls.clearValidations("frmSourceCode");
        sourceCodeUI.finderData = null;
        sourceCodeRepository.get(data.SourceLedger, sourceCodeUISuccess.get);
    },

    // Is New
    isNew: function (model) {
        if (model.SourceLedger() === null) {
           return true;
        }
        return false;
    }

};

// Finder Filter
var sourceCodeFilter = {
    getFilter: function () {
        var filters = [[]];
        var sourceCodeName = $("#txtSourceLedger").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, sourceCodeName);
        return filters;
    }
};

// Initial Entry
$(function () {
    sourceCodeUI.init();
    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && sourceCodeUI.sourceCodeModel.isModelDirty.isDirty()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, sourceCodeResources.SourceLedgerTitle)).text();
        }
    });

});
