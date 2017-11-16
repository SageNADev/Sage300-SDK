// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.

"use strict";

var saveAdhocInquiryUI = saveAdhocInquiryUI || {};
saveAdhocInquiryUI = {
    activeIFrameCallerId: "",
    inquiryFeatureTypeSecurity: "",

    init: function() {
        saveAdhocInquiryUI.initButton();
        saveAdhocInquiryUI.initMessageEventListener();
        saveAdhocInquiryUI.initRadioButtons();
        savedQueryGridUI.initGrid();
    },

    initButton: function() {
        $("#btnSave").click(function () {
            var data = {
                'iFrameId': saveAdhocInquiryUI.activeIFrameCallerId,
                'InquiryQuery': {
                    'Name': $("#SaveQueryPanel_QueryName").val(),
                    'InquiryQueryType': $("#rdbPrivateQueryType").is(':checked') ? 2 : 1,
                    'DateModified': sg.utls.kndoUI.getDate(new Date()),
                    'Description': $("#SaveQueryPanel_QueryDescription").val(),
                    'InquiryFeatureType': {
                        'Security': saveAdhocInquiryUI.inquiryFeatureTypeSecurity
                    }
                }
            };
            var message = sg.utls.iFrameHelper.buildData("saveQuery", data)
            window.top.postMessage(message, "*");
        });

        $("#btnCancel").click(saveAdhocInquiryUI.hideSaveQueryPanel);
    },

    initRadioButtons: function() {
        var radioButtonIdList = ["rdbPublicQueryType", "rdbPrivateQueryType"];
        sg.controls.InitSelectRadioButtonBehaviour(radioButtonIdList);
    },

    initMessageEventListener: function() {
        sg.utls.iFrameHelper.registerToReceiveMessage(saveAdhocInquiryUI.receiveWindowMessage);
    },

    receiveWindowMessage: function (e) {
        if (e.data.showSaveQueryPanel) {
            saveAdhocInquiryUI.activeIFrameCallerId = e.data.iFrameId;
            saveAdhocInquiryUI.openSaveQueryPanel();
            saveAdhocInquiryUI.inquiryFeatureTypeSecurity = e.data.inquiryFeatureTypeSecurity;
        }
        else if (e.data.hideSaveQueryPanel) {
            saveAdhocInquiryUI.hideSaveQueryPanel();
            saveAdhocInquiryUI.activeIFrameCallerId = "";
        }
    },

    openSaveQueryPanel: function () {
        $("#savedQueryGrid").data("kendoGrid").dataSource.read();
        $("#SaveQueryPanel_QueryName").val("");
        $("#rdbPublicQueryType").click();
        $("#SaveQueryPanel_QueryDescription").val("");
        $('#saveQueryPanel').addClass('slide-in');
    },

    hideSaveQueryPanel: function() {
        $('#saveQueryPanel').removeClass("slide-in").removeClass("slide-in-note-edit");
    }
}

$(function () {
    saveAdhocInquiryUI.init();
})
