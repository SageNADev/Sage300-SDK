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

"use strict";
var sourceCodeUI = sourceCodeUI || {};

sourceCodeUI = {
    sourceCodeModel: {},
    ignoreIsDirtyProperties: ["SourceCode", "SourceLedger", "SourceType", "DisableAddSave", "DisableDelete", "AddSaveTitle"],
    computedProperties: ["UIMode", "AddSaveTitle", "SourceCode", "DisableAddSave", "DisableDelete"],
    finderData: null,
    sourceCode: null,

    // Init
    init: function () {
        sg.utls.maskSourceCode("sg-mask-sourcecode");
        sourceCodeUI.initButtons();
        sourceCodeUI.initFinders();
        sourceCodeUISuccess.initialLoad(sourceCodeViewModel);
        Customization.set();
        sourceCodeUISuccess.setKey();
    },

    // Save
    saveSourceCode: function () {
        if ($("#frmSourceCodes").valid()) {

            if (sg.controls.GetString(sourceCodeUI.sourceCodeModel.Data.SourceType()) === "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.SourceTypeMessage);
                return;
            }

            var data = sg.utls.ko.toJS(sourceCodeUI.sourceCodeModel.Data, sourceCodeUI.computedProperties);
            if (sourceCodeUI.sourceCodeModel.Data.UIMode() === sg.utls.OperationMode.SAVE) {
                sourceCodeRepository.update(data);
            } else {
                sourceCodeRepository.add(data);
            }
        }
    },

    // Init Buttons
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusourcecode", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusourcecode", false, $.noop);

        // Key field with Finder
        $("#sourceCode").bind('blur', function (e) {
            //The below line of code is not correct as KO should only do this.
            //KO is not firing because Masking is there. This doesn't happens in the new version of mask plugin.
            //We can remove this once new version is applied.
            sourceCodeUI.sourceCodeModel.Data.SourceCode($("#sourceCode").val());
            sg.delayOnBlur("btnFinderSourceCode", function() {
                if (sg.controls.GetString(sourceCodeUI.sourceCodeModel.Data.SourceLedger()) !== "") {
                    if (sg.controls.GetString(sourceCodeUI.sourceCode) !== sg.controls.GetString(sourceCodeUI.sourceCodeModel.Data.SourceCode())) {
                        sourceCodeUI.checkIsDirty(sourceCodeUI.get, sourceCodeUI.sourceCode);
                    }
                }
            });
        });

        // Create New Button
        $("#btnNewSourceCode").bind('click', function () {
            sourceCodeUI.checkIsDirty(sourceCodeUI.create, sourceCodeUI.sourceCode);
        });

        // Save Button
        $("#btnSaveSourceCode").bind('click', function () {
            sg.utls.SyncExecute(sourceCodeUI.saveSourceCode);
        });

        // Delete Button
        $("#btnDeleteSourceCode").bind('click', function () {
            if ($("#frmSourceCodes").valid()) {
                var message = jQuery.validator.format(sourceCodeResources.DeleteConfirmMessage, sourceCodeResources.SourceCodeTitle, sourceCodeUI.sourceCodeModel.Data.SourceCode());
                sg.utls.showKendoConfirmationDialog(function() {
                        sg.utls.clearValidations("frmSourceCodes");
                        sourceCodeRepository.delete(sourceCodeUI.sourceCodeModel.Data.SourceLedger(), sourceCodeUI.sourceCodeModel.Data.SourceType());
                    },
                    null, message,
                    sourceCodeResources.DeleteTitle);
            }
        });

    },

    // Init Dropdowns here

    // Init Finders, if any
    initFinders: function () {
        var title = jQuery.validator.format(
            sourceCodeResources.FinderTitle,
            sourceCodeResources.SourceCodeTitle
        );
        sg.finderHelper.setFinder(
            "btnFinderSourceCode",
            "tusourcecode",
            sourceCodeUISuccess.finderSuccess,
            function () {
                sg.controls.Focus($("#sourceCode"));
            },
            title,
            sourceCodeFilter.getFilter,
            null,
            false
        );
    },

    // Get
    get: function () {
        sourceCodeRepository.get(
            sourceCodeUI.sourceCodeModel.Data.SourceLedger(),
            sourceCodeUI.sourceCodeModel.Data.SourceType()
        );
    },

    // Create
    create: function () {
        sg.utls.clearValidations("frmSourceCodes");
        sourceCodeRepository.create();
    },

    // Is Dirty check
    checkIsDirty: function (functionToCall, sourceCode) {
        if (sourceCodeUI.sourceCodeModel.isModelDirty.isDirty()) {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSourceCodes");
                    functionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(sourceCode) != sg.controls.GetString(sourceCodeUI.sourceCodeModel.Data.SourceCode())) {
                        sourceCodeUI.sourceCodeModel.Data.SourceCode(sourceCode);
                    }
                    return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, sourceCodeResources.SourceCodeTitle, sourceCode));
        } else {
            functionToCall.call();
        }
    },
};

// Callbacks
var sourceCodeUISuccess = {

    // Setkey
    setKey: function () {
        sourceCodeUI.sourceCode = sourceCodeObject.generateSourceCode(sourceCodeUI.sourceCodeModel.Data.SourceLedger(), sourceCodeUI.sourceCodeModel.Data.SourceType());
    },

    // Get
    get: function (data) {
        sourceCodeUISuccess.displayResult(data, function (result) {
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                if (result.Data != null) {
                    ko.mapping.fromJS(result, sourceCodeUI.sourceCodeModel);
                    sourceCodeUI.sourceCodeModel.Data.UIMode(sg.utls.OperationMode.SAVE);
                    sourceCodeUI.sourceCodeModel.isModelDirty.reset();
                } else {
                    sourceCodeUI.sourceCodeModel.Data.UIMode(sg.utls.OperationMode.NEW);
                }
            }
            sourceCodeUISuccess.setKey();
            sg.controls.Select($("#description"));
        });
    },

    // Update
    update: function (data) {
        sourceCodeUISuccess.displayResult(data, function (result) {
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                ko.mapping.fromJS(result, sourceCodeUI.sourceCodeModel);
                sourceCodeUI.sourceCodeModel.Data.UIMode(sg.utls.OperationMode.SAVE);
                sourceCodeUI.sourceCodeModel.isModelDirty.reset();
                sourceCodeUISuccess.setKey();
            }
        });
    },

    // Create
    create: function (data) {
        sourceCodeUISuccess.displayResult(data, function (result) {
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                ko.mapping.fromJS(result, sourceCodeUI.sourceCodeModel);
                sourceCodeUI.sourceCodeModel.Data.UIMode(sg.utls.OperationMode.NEW);
                sourceCodeUI.sourceCodeModel.isModelDirty.reset();
                sourceCodeUISuccess.setKey();
                sg.controls.Focus($("#sourceCode"));
            }
        });
    },

    // Delete
    delete: function (data) {
        sourceCodeUISuccess.displayResult(data, function (result) {
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                ko.mapping.fromJS(result, sourceCodeUI.sourceCodeModel);
                sourceCodeUI.sourceCodeModel.Data.UIMode(sg.utls.OperationMode.NEW);
                sourceCodeUI.sourceCodeModel.isModelDirty.reset();
                sourceCodeUISuccess.setKey();
            }
        });
    },

    // Display Result
    displayResult: function (result, functionToCall) {
        if (result) {
            functionToCall(result);
            sg.utls.showMessage(result);
        }
        else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.ProcessFailedMessage);
        }
    },

    // Initial Load
    initialLoad: function (result) {
        if (result) {
            var uiMode;
            sourceCodeUI.sourceCodeModel = ko.mapping.fromJS(result);
            if (sourceCodeUISuccess.isNew(sourceCodeUI.sourceCodeModel.Data)) {
                uiMode = sg.utls.OperationMode.NEW;
            }
            else {
                uiMode = sg.utls.OperationMode.SAVE;
            }
            SourceCodeObservableExtension(sourceCodeUI.sourceCodeModel.Data, uiMode);
            sourceCodeUI.sourceCodeModel.isModelDirty = new ko.dirtyFlag(sourceCodeUI.sourceCodeModel.Data, sourceCodeUI.ignoreIsDirtyProperties);
            ko.applyBindings(sourceCodeUI.sourceCodeModel);
        }
        else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#sourceCode"));
    },

    // Finder Success
    finderSuccess: function (data) {
        if (data != null) {
            sourceCodeUI.finderData = data;
            sourceCodeUI.checkIsDirty(sourceCodeUISuccess.setFinderData, sourceCodeUI.sourceCode);
        }
    },

    // Set Finder
    setFinderData: function () {
        var data = sourceCodeUI.finderData;
        sg.utls.clearValidations("frmSourceCodes");
        sourceCodeUI.finderData = null;
        sourceCodeRepository.get(data.SourceLedger, data.SourceType);
    },

    // Is New
    isNew: function (model) {
        if (model.SourceLedger() == null && model.SourceType() == null) {
            return true;
        }
        return false;
    },

};

// Finder Filter
var sourceCodeFilter = {
    getFilter: function () {
        var filters = [[]];

        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, sourceCodeUI.sourceCodeModel.Data.SourceLedger());

        filters[0][1] = sg.finderHelper.createFilter("SourceType", sg.finderOperator.StartsWith, sourceCodeUI.sourceCodeModel.Data.SourceType());

        return filters;
    }
};

// Source Code Object
var sourceCodeObject = {
    getSourceCodeElements: function (srceCode) {
        var sourceCode = { SourceLedger: null, SourceType: null };
        var items = srceCode.split("-");
        $.each(items, function (index, chunk) {
            if (index === 0) {
                sourceCode.SourceLedger = sourceCodeObject.sourceLedger = items[0].toUpperCase();
            }
            else if (index === 1) {
                sourceCode.SourceType = sourceCodeObject.sourceType = items[1].toUpperCase();
            }
        });
       return sourceCode;
    },

    generateSourceCode: function (sourceLedger, sourceType) {
        var sourceCode = "";
        if (sourceLedger != undefined) {
            sourceCode = sourceLedger;
            if (sourceType != undefined) {
                sourceCode += '-' + sourceType;
            }
        }
        return sourceCode;
    }
};

// Initial Entry
$(function () {
    sourceCodeUI.init();
    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && sourceCodeUI.sourceCodeModel.isModelDirty.isDirty()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, sourceCodeResources.SourceCodeTitle)).text();
        }
    });

});
