// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

var sourceCodeUI = sourceCodeUI || {};
sourceCodeUI = {
    sourceCodeModel: {},
    ignoreIsDirtyProperties: ["SourceCode", "SourceLedger", "SourceType", "DisableAddSave", "DisableDelete", "AddSaveTitle"],
    computedProperties: ["UIMode", "AddSaveTitle", "SourceCode", "DisableAddSave", "DisableDelete"],
    finderData: null,
    sourceCode: null,

    /**
     * @function
     * @name init
     * @description Primary initialization routine
     * @namespace sourceCodeUI
     * @public
     */
    init: function () {
        sg.utls.maskSourceCode("sg-mask-sourcecode");
        sourceCodeUI.initButtons();
        sourceCodeUI.initFinders();
        sourceCodeUISuccess.initialLoad(sourceCodeViewModel);
        Customization.set();
        sourceCodeUISuccess.setKey();
    },

    /**
     * @function
     * @name saveSourceCode
     * @description Save a source code entry
     * @namespace sourceCodeUI
     * @public
     */
    saveSourceCode: function () {
        if ($("#frmSourceCodes").valid()) {

            if (sg.controls.GetString(sourceCodeUI.sourceCodeModel.Data.SourceType()) === "") {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.SourceTypeMessage);
                return;
            }

            let data = sg.utls.ko.toJS(sourceCodeUI.sourceCodeModel.Data, sourceCodeUI.computedProperties);
            if (sourceCodeUI.sourceCodeModel.Data.UIMode() === sg.utls.OperationMode.SAVE) {
                sourceCodeRepository.update(data);
            } else {
                sourceCodeRepository.add(data);
            }
        }
    },

    /**
     * @function
     * @name initButtons
     * @description Initialize buttons
     * @namespace sourceCodeUI
     * @public
     */
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusourcecode", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusourcecode", false, $.noop);

        // Key field with Finder
        $("#sourceCode").on('blur', function (e) {
            // The below line of code is not correct as KO should only do this.
            // KO is not firing because Masking is there. This doesn't happens in the new version of mask plugin.
            // We can remove this once new version is applied.
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
        $("#btnNewSourceCode").on('click', function () {
            sourceCodeUI.checkIsDirty(sourceCodeUI.create, sourceCodeUI.sourceCode);
        });

        // Save Button
        $("#btnSaveSourceCode").on('click', function () {
            sg.utls.SyncExecute(sourceCodeUI.saveSourceCode);
        });

        // Delete Button
        $("#btnDeleteSourceCode").on('click', function () {
            if ($("#frmSourceCodes").valid()) {
                let message = jQuery.validator.format(sourceCodeResources.DeleteConfirmMessage, sourceCodeResources.SourceCodeTitle, sourceCodeUI.sourceCodeModel.Data.SourceCode());
                sg.utls.showKendoConfirmationDialog(function() {
                        sg.utls.clearValidations("frmSourceCodes");
                        sourceCodeRepository.delete(sourceCodeUI.sourceCodeModel.Data.SourceLedger(), sourceCodeUI.sourceCodeModel.Data.SourceType());
                    },
                    null, message,
                    sourceCodeResources.DeleteTitle);
            }
        });
    },

    /**
     * @function
     * @name initFinders
     * @description Initialize the finders
     * @namespace sourceCodeUI
     * @public
     */
    initFinders: function () {
        let info = sg.viewFinderProperties.GL.SourceCodes;
        let buttonId = "btnFinderSourceCode";
        let dataControlIdOrSuccessCallback = sourceCodeUISuccess.finderSuccess;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, sourceCodeFilter.getFilter);
    },

    /**
     * @function
     * @name get
     * @description Get an existing source code
     * @namespace sourceCodeUI
     * @public
     */
    get: function () {
        sourceCodeRepository.get(
            sourceCodeUI.sourceCodeModel.Data.SourceLedger(),
            sourceCodeUI.sourceCodeModel.Data.SourceType()
        );
    },

    /**
     * @function
     * @name create
     * @description Create a new source code
     * @namespace sourceCodeUI
     * @public
     */
    create: function () {
        sg.utls.clearValidations("frmSourceCodes");
        sourceCodeRepository.create();
    },

    /**
     * @function
     * @name checkIsDirty
     * @description Check to see if model has been altered
     * @namespace sourceCodeUI
     * @public
     * 
     * @param {Function} functionToCall Function called in event model is not dirty OR
     *                                  user selects Yes in confirmation dialog box
     * @param {string} sourceCode The source code specification
     */
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

    /**
     * @function
     * @name setKey
     * @description Set the current key
     * @namespace sourceCodeUISuccess
     * @public
     */
    setKey: function () {
        sourceCodeUI.sourceCode = sourceCodeObject.generateSourceCode(sourceCodeUI.sourceCodeModel.Data.SourceLedger(), sourceCodeUI.sourceCodeModel.Data.SourceType());
    },

    /**
     * @function
     * @name get
     * @description Event handler for successful get
     * @namespace sourceCodeUISuccess
     * @public
     *  
     * @param {object} data The returned data
     */
    get: function (data) {
        sourceCodeUISuccess.displayResult(data, function (result) {
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                if (result.Data) {
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

    /**
     * @function
     * @name update
     * @description Event handler for successful update
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} data The returned data
     */
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

    /**
     * @function
     * @name update
     * @description Event handler for successful create
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} data The returned data
     */
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

    /**
     * @function
     * @name update
     * @description Event handler for successful delete
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} data The returned data
     */
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

    /**
     * @function
     * @name displayResult
     * @description Event handler for successful displayResult call
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} result JSON payload object
     * @param {Function} functionToCall Function to call on successful result
     */
    displayResult: function (result, functionToCall) {
        if (result) {
            functionToCall(result);
            sg.utls.showMessage(result);
        }
        else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.ProcessFailedMessage);
        }
    },

    /**
     * @function
     * @name initialLoad
     * @description Method called on initial page load
     * @namespace sourceCodeUISuccess
     * @public
     * 
     * @param {object} result JSON payload object
     */
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

    /**
     * @function
     * @name finderSuccess
     * @description Event handler for successful finder call
     * @namespace sourceUISuccess
     * @public 
     * 
     * @param {object} data JSON payload object
     */
    finderSuccess: function (data) {
        if (data) {
            sourceCodeUI.finderData = data;
            sourceCodeUI.checkIsDirty(sourceCodeUISuccess.setFinderData, sourceCodeUI.sourceCode);
        }
    },

    /**
     * @function
     * @name setFinderData
     * @description 
     * @namespace sourceUISuccess
     * @public
     */
    setFinderData: function () {
        var data = sourceCodeUI.finderData;
        var sourceLedger = data.SRCELEDGER;
        var sourceType = data.SRCETYPE;

        sg.utls.clearValidations("frmSourceCodes");
        sourceCodeUI.finderData = null;
        sourceCodeRepository.get(sourceLedger, sourceType);
    },

    /**
     * @function
     * @name isNew
     * @description Determines if model data is populated
     * @namespace sourceUISuccess
     * @public 
     * @param {object} model Model data
     *  
     * @returns {boolean} true = New record | false = existing record
     */
    isNew: function (model) {
        if (!model.SourceLedger() && !model.SourceType()) {
            return true;
        }
        return false;
    },

};

// Finder Filter
var sourceCodeFilter = {
    /**
     * @function
     * @name getFilter
     * @description Create the finder filter
     * @namespace sourceCodeFilter
     * @public 
     * 
     * @returns {Array} The filters array
     */
    getFilter: function () {
        var filters = [[]];
        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, sourceCodeUI.sourceCodeModel.Data.SourceLedger());
        filters[0][1] = sg.finderHelper.createFilter("SourceType", sg.finderOperator.StartsWith, sourceCodeUI.sourceCodeModel.Data.SourceType());
        return filters;
    }
};

// Source Code Object
var sourceCodeObject = {
    /**
     * @function
     * @name getSourceCodeElements
     * @description
     * @namespace sourceCodeObject
     * @public 
     * 
     * @param {string} srceCode The source code specification
     * 
     * @returns {object} The source code object
     */
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

    /**
     * @function
     * @name generateSourceCode
     * @description Generate a source code based on Source Ledger and Source Type
     * @namespace sourceCodeObject
     * @public 
     * 
     * @param {string} sourceLedger The source ledger specification
     * @param {string} sourceType The source type specification
     * 
     * @returns {string} The generated source code specification
     */
    generateSourceCode: function (sourceLedger, sourceType) {
        var sourceCode = "";
        if (sourceLedger) {
            sourceCode = sourceLedger;
            if (sourceType) {
                sourceCode += '-' + sourceType;
            }
        }
        return sourceCode;
    }
};

// Initial Entry
$(function () {
    sourceCodeUI.init();
    $(window).on('beforeunload', function () {
        var dirty = sourceCodeUI.sourceCodeModel.isModelDirty.isDirty();
        if (sg.utls.isPageUnloadEventEnabled(dirty)) {
            return sg.utls.getDirtyMessage(sourceCodeResources.SourceCodeTitle);
        }
    });
});
