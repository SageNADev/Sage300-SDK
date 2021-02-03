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

var modelData;
var sourceCodeUI = sourceCodeUI || {};

sourceCodeUI = {
    sourceCodeModel: {},
    ignoreIsDirtyProperties: ["SourceLedger"],
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,
    sourceLedger: null,

    /**
     * @function
     * @name init
     * @description Primary Initialization function
     * @namespace sourceCodeUI
     * @public
     */
    init: function () {
        sourceCodeUI.initButtons();
        sourceCodeUI.initFinders();
        sourceCodeUISuccess.initialLoad(SourceCodeViewModel);
        sourceCodeUISuccess.setkey();
    },

    /**
     * @function
     * @name saveSourceCode
     * @description Invoke the Source code add or update functionality
     * @namespace sourceCodeUI
     * @public
     */
    saveSourceCode: function () {
        if ($("#frmSourceCode").valid()) {
            let data = sg.utls.ko.toJS(modelData, sourceCodeUI.computedProperties);
            if (modelData.UIMode() === sg.utls.OperationMode.SAVE) {
                sourceCodeRepository.update(data, sourceCodeUISuccess.update);
            } else {
                sourceCodeRepository.add(data, sourceCodeUISuccess.update);
            }
        }
    },

    /**
     * @function
     * @name initButtons
     * @description Initialize the buttons
     * @namespace sourceCodeUI
     * @public
     */
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusourcecode", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusourcecode", false, $.noop);

        // Key field with Finder
        $("#txtSourceLedger").on('blur', function (e) {
            modelData.SourceLedger($("#txtSourceLedger").val());
            sg.delayOnBlur("btnFinderSourceLedger", function () {
                if (sg.controls.GetString(modelData.SourceLedger() !== "")) {
                    if (sg.controls.GetString(sourceCodeUI.sourceLedger) !== sg.controls.GetString(modelData.SourceLedger())) {
                        sourceCodeUI.checkIsDirty(sourceCodeUI.get, sourceCodeUI.sourceLedger);
                    }
                }
            });
        });

        // Create New Button
        $("#btnNew").on('click', function () {
            sourceCodeUI.checkIsDirty(sourceCodeUI.create, sourceCodeUI.sourceLedger);
        });

        // Save Button
        $("#btnSave").on('click', function () {
            sg.utls.SyncExecute(sourceCodeUI.saveSourceCode);
        });

        // Delete Button
        $("#btnDelete").on('click', function () {
            if ($("#frmSourceCode").valid()) {
                let message = jQuery.validator.format(sourceCodeResources.DeleteConfirmMessage, sourceCodeResources.SourceLedgerTitle, modelData.SourceLedger());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmSourceCode");
                    sourceCodeRepository.delete(modelData.SourceLedger(), sourceCodeUISuccess.delete);
                }, null, message, sourceCodeResources.DeleteTitle);
            }
        });

    },

    /**
     * @function
     * @name initFinders
     * @description Initialize the finder(s)
     * @namespace sourceCodeUI
     * @public
     */
    initFinders: function () {
        let info = sg.viewFinderProperties.GL.SourceCodes;
        let buttonId = "btnFinderSourceLedger";
        let dataControlIdOrSuccessCallback = sourceCodeUISuccess.finderSuccess;
        sg.viewFinderHelper.initFinder(buttonId, dataControlIdOrSuccessCallback, info, sourceCodeFilter.getFilter);
    },

    /**
     * @function
     * @name get
     * @description Invoke the Source code get functionality
     * @namespace sourceCodeUI
     * @public
     */
    get: function () {
        sourceCodeRepository.get(modelData.SourceLedger(), sourceCodeUISuccess.get);
    },

    /**
     * @function
     * @name create
     * @description Invoke the Source code create functionality
     * @namespace sourceCodeUI
     * @public
     */
    create: function () {
        sg.utls.clearValidations("frmSourceCode");
        sourceCodeRepository.create(sourceCodeUISuccess.create);
    },

    /**
     * @function
     * @name checkIsDirty
     * @description Check the model for any changes. If there are changes, invoke a confirmation
     *              dialog box. If the user selects 'Yes', invoke the callback function passed into the function
     * @namespace sourceCodeUI
     * @public
     *
     * @param {Function} functionToCall Method to call if user selects 'Yes' in confirmation dialog
     */
    checkIsDirty: function (functionToCall, sourceLedger) {
        if (sourceCodeUI.sourceCodeModel.isModelDirty.isDirty() && sourceLedger !== null && sourceLedger !== "") {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSourceCode");
                    functionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(sourceLedger) !== sg.controls.GetString(modelData.SourceLedger())) {
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

    /**
     * @function
     * @name setkey
     * @description Set the source code key
     * @namespace sourceCodeUISuccess
     * @public
     */
    setkey: function () {
        sourceCodeUI.sourceLedger = modelData.SourceLedger();
    },

    /**
     * @function
     * @name get
     * @description Function called by repository upon successful Source Code get
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     */
    get: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data) {
                sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            } else {
                modelData.UIMode(sg.utls.OperationMode.NEW);
            }
            sourceCodeUISuccess.setkey();
        }
    },

    /**
     * @function
     * @name update
     * @description Function called by repository upon successful Source Code update
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     */
    update: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            sourceCodeUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    /**
     * @function
     * @name create
     * @description Function called by repository upon successful Source Code create
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     */
    create: function (jsonResult) {
        sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        sourceCodeUI.sourceCodeModel.isModelDirty.reset();
        sourceCodeUISuccess.setkey();
        sg.controls.Focus($("#txtSourceLedger"));
    },

    /**
     * @function
     * @name deleteSourceJournal
     * @description Function called by repository upon successful Source Code deletion
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     */
    delete: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceCodeUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            sourceCodeUI.sourceCodeModel.isModelDirty.reset();
            sourceCodeUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    /**
     * @function
     * @name displayResult
     * @description Display the results of a server-side call
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} jsonResult The JSON payload object
     * @param {number} uiMode The UI mode
     */
    displayResult: function (jsonResult, uiMode) {
        if (jsonResult) {
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
                if (uiMode !== sg.utls.OperationMode.NEW) {
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

    /**
     * @function
     * @name initialLoad
     * @description Called on initial page load
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    initialLoad: function (result) {
        if (result) {
            sourceCodeUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceCodeResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtSourceLedger"));
    },

    /**
     * @function
     * @name finderSuccess
     * @description Event handler for finder selection
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} result The JSON payload object
     */
    finderSuccess: function (data) {
        if (data) {
            sourceCodeUI.finderData = data;
            sourceCodeUI.checkIsDirty(sourceCodeUISuccess.setFinderData, sourceCodeUI.sourceLedger);
        }
    },

    /**
     * @function
     * @name setFinderData
     * @description Set the finder data
     * @namespace sourceCodeUISuccess
     * @public
     */
    setFinderData: function () {
        let data = sourceCodeUI.finderData;
        sg.utls.clearValidations("frmSourceCode");
        sourceCodeUI.finderData = null;
        sourceCodeRepository.get(data.SourceLedger, sourceCodeUISuccess.get);
    },

    /**
     * @function
     * @name isNew
     * @description Check to see if model is valid
     * @namespace sourceCodeUISuccess
     * @public
     *
     * @param {object} model The current model
     */
    isNew: function (model) {
        if (model.SourceLedger() === null) {
           return true;
        }
        return false;
    }

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
     * @returns {object} The filters object
     */
    getFilter: function () {
        let filters = [[]];
        let sourceCodeName = $("#txtSourceLedger").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceLedger", sg.finderOperator.StartsWith, sourceCodeName);
        return filters;
    }
};

// Initial Entry
$(function () {
    sourceCodeUI.init();

    $(window).on('beforeunload', function () {
        let dirty = sourceCodeUI.sourceCodeModel.isModelDirty.isDirty();
        if (sg.utls.isPageUnloadEventEnabled(dirty)) {
            return sg.utls.getDirtyMessage(sourceCodeResources.SourceLedgerTitle);
        }
    });
});
