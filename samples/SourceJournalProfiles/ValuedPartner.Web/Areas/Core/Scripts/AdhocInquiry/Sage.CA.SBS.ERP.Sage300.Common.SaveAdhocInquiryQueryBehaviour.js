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
