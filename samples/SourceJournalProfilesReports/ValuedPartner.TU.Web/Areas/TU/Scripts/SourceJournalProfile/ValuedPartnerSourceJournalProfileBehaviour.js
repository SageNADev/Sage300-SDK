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

var modelData;
var sourceJournalProfileUI = sourceJournalProfileUI || {};

sourceJournalProfileUI = {
    sourceJournalProfileModel: {},
    ignoreIsDirtyProperties: ["SourceJournalName"],
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,
    sourceJournalName: null,

    // Init
    init: function () {
    sourceJournalProfileUI.initButtons();
    sourceJournalProfileUI.initFinders();
    sourceJournalProfileUISuccess.initialLoad(SourceJournalProfileViewModel);
    sourceJournalProfileUISuccess.setkey();
    },

    // Save
    saveSourceJournalProfile: function () {
        if ($("#frmSourceJournalProfile").valid()) {
            var data = sg.utls.ko.toJS(modelData, sourceJournalProfileUI.computedProperties);
            if (modelData.UIMode() === sg.utls.OperationMode.SAVE) {
                sourceJournalProfileRepository.update(data, sourceJournalProfileUISuccess.update);
            } else {
                sourceJournalProfileRepository.add(data, sourceJournalProfileUISuccess.update);
            }
        }
    },

    // Init Buttons
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusourcejournalprofile", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusourcejournalprofile", false, $.noop);

        // Key field with Finder
        $("#txtSourceJournalName").bind('blur', function (e) {
            modelData.SourceJournalName($("#txtSourceJournalName").val());
            sg.delayOnBlur("btnFinderSourceJournalName", function () {
                if (sg.controls.GetString(modelData.SourceJournalName() != "")) {
                    if (sg.controls.GetString(sourceJournalProfileUI.sourceJournalName) != sg.controls.GetString(modelData.SourceJournalName())) {
                        sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.get, sourceJournalProfileUI.sourceJournalName);
                    }
                }
            });
        });

        // Create New Button
        $("#btnNew").bind('click', function () {
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUI.create, sourceJournalProfileUI.sourceJournalName);
        });

        // Save Button
        $("#btnSave").bind('click', function () {
            sg.utls.SyncExecute(sourceJournalProfileUI.saveSourceJournalProfile);
        });

        // Delete Button
        $("#btnDelete").bind('click', function () {
            if ($("#frmSourceJournalProfile").valid()) {
                var message = jQuery.validator.format(sourceJournalProfileResources.DeleteConfirmMessage, sourceJournalProfileResources.SourceJournalNameTitle, modelData.SourceJournalName());
                sg.utls.showKendoConfirmationDialog(function () {
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    sourceJournalProfileRepository.delete(modelData.SourceJournalName(), sourceJournalProfileUISuccess.delete);
                }, null, message, sourceJournalProfileResources.DeleteTitle);
            }
        });

    },

    // Init Dropdowns here

    // Init Finders, if any
    initFinders: function () {
        var title = jQuery.validator.format(sourceJournalProfileResources.FinderTitle, sourceJournalProfileResources.SourceJournalNameTitle);
        sg.finderHelper.setFinder("btnFinderSourceJournalName", "tusourcejournalprofile", sourceJournalProfileUISuccess.finderSuccess, $.noop, title, sourceJournalProfileFilter.getFilter, null, true);
    },

    // Get
    get: function () {
        sourceJournalProfileRepository.get(modelData.SourceJournalName(), sourceJournalProfileUISuccess.get);
    },

    // Create
    create: function () {
        sg.utls.clearValidations("frmSourceJournalProfile");
        sourceJournalProfileRepository.create(sourceJournalProfileUISuccess.create);
    },

    // Is Dirty check
    checkIsDirty: function (functionToCall, sourceJournalName) {
        if (sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty.isDirty() && sourceJournalName != null && sourceJournalName != "") {
            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    sg.utls.clearValidations("frmSourceJournalProfile");
                    functionToCall.call();
                },
                function () { // No
                    if (sg.controls.GetString(sourceJournalName) != sg.controls.GetString(modelData.SourceJournalName())) {
                        modelData.SourceJournalName(sourceJournalName);
                   }
                   return;
                },
                jQuery.validator.format(globalResource.SaveConfirm, sourceJournalProfileResources.SourceJournalNameTitle, sourceJournalName));
        } else {
            functionToCall.call();
        }
    }

};

// Callbacks
var sourceJournalProfileUISuccess = {

    // Setkey
    setkey: function () {
        sourceJournalProfileUI.sourceJournalName = modelData.SourceJournalName();
    },

    // Get
    get: function (jsonResult) {
        if (jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
            if (jsonResult.Data != null) {
                sourceJournalProfileUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            } else {
                modelData.UIMode(sg.utls.OperationMode.NEW);
            }
            sourceJournalProfileUISuccess.setkey();
        }
    },

    // Update
    update: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceJournalProfileUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
            sourceJournalProfileUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    // Create
    create: function (jsonResult) {
        sourceJournalProfileUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
        sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty.reset();
        sourceJournalProfileUISuccess.setkey();
        sg.controls.Focus($("#txtSourceJournalName"));
    },

    // Delete
    delete: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            sourceJournalProfileUISuccess.displayResult(jsonResult, sg.utls.OperationMode.NEW);
            sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty.reset();
            sourceJournalProfileUISuccess.setkey();
        }
        sg.utls.showMessage(jsonResult);
    },

    // Display Result
    displayResult: function (jsonResult, uiMode) {
        if (jsonResult != null) {
            if (!sourceJournalProfileUI.hasKoBindingApplied) {
                sourceJournalProfileUI.sourceJournalProfileModel = ko.mapping.fromJS(jsonResult);
                sourceJournalProfileUI.hasKoBindingApplied = true;
                modelData = sourceJournalProfileUI.sourceJournalProfileModel.Data;
                sourceJournalProfileObservableExtension(sourceJournalProfileUI.sourceJournalProfileModel, uiMode);
                sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty = new ko.dirtyFlag(modelData, sourceJournalProfileUI.ignoreIsDirtyProperties);
                ko.applyBindings(sourceJournalProfileUI.sourceJournalProfileModel);
            } else {
                ko.mapping.fromJS(jsonResult, sourceJournalProfileUI.sourceJournalProfileModel);
                modelData.UIMode(uiMode);
                if (uiMode != sg.utls.OperationMode.NEW) {
                    sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty.reset();
                }
            }

            if (!sourceJournalProfileUI.isKendoControlNotInitialised) {
                sourceJournalProfileUI.isKendoControlNotInitialised = true;
            } else {
                // 
            }
        }
    },

    // Initial Load
    initialLoad: function (result) {
        if (result) {
            sourceJournalProfileUISuccess.displayResult(result, sg.utls.OperationMode.NEW);
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, sourceJournalProfileResources.ProcessFailedMessage);
        }
        sg.controls.Focus($("#txtSourceJournalName"));
    },

    // Finder Success
    finderSuccess: function (data) {
        if (data != null) {
            sourceJournalProfileUI.finderData = data;
            sourceJournalProfileUI.checkIsDirty(sourceJournalProfileUISuccess.setFinderData, sourceJournalProfileUI.sourceJournalName);
        }
    },

    // Set Finder
    setFinderData: function () {
        var data = sourceJournalProfileUI.finderData;
        sg.utls.clearValidations("frmSourceJournalProfile");
        sourceJournalProfileUI.finderData = null;
        sourceJournalProfileRepository.get(data.SourceJournalName, sourceJournalProfileUISuccess.get);
    },

    // Is New
    isNew: function (model) {
        if (model.SourceJournalName() === null) {
           return true;
        }
        return false;
    }

};

// Finder Filter
var sourceJournalProfileFilter = {
    getFilter: function () {
        var filters = [[]];
        var sourceJournalProfileName = $("#txtSourceJournalName").val();
        filters[0][0] = sg.finderHelper.createFilter("SourceJournalName", sg.finderOperator.StartsWith, sourceJournalProfileName);
        return filters;
    }
};

// Initial Entry
$(function () {
    sourceJournalProfileUI.init();
    $(window).bind('beforeunload', function () {
        if (globalResource.AllowPageUnloadEvent && sourceJournalProfileUI.sourceJournalProfileModel.isModelDirty.isDirty()) {
            return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, sourceJournalProfileResources.SourceJournalNameTitle)).text();
        }
    });

});
