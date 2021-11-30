/* Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var loginRepository = {

    // Invoke controller to validate credentials and attempt login
    login: function (data) {
        var url = sg.utls.url.buildUrl("Core", "Authentication", "LoginResultOnPremise");
        return loginRepository.ajaxCall(url, data);
    },

    // Invoke controller to validate credentials and attempt to change password
    changePassword: function (data, callback) {
        var url = sg.utls.url.buildUrl("Core", "Authentication", "ChangePassword");
        sg.utls.ajaxPost(url, data, callback);
    },

    // Invoke controller to clear resume login flag and attempt to change password
    clear: function(data, callback) {
        var url = sg.utls.url.buildUrl("Core", "Authentication", "Clear");
        sg.utls.ajaxPost(url, data, callback);
    },

    /**
     * This function is used get the company list base on user id
     * @param {} data 
     * @returns {} 
     */
    getUserListOfCompanies: function (data) {
        var url = sg.utls.url.buildUrl("Core", "Authentication", "GetUserListOfCompanies");
        return loginRepository.ajaxCall(url, data);
    },

    /**
     * This function is used trigger the ajax call and handle error
     * @param {} ajaxUrl 
     * @param {} ajaxData 
     * @returns {} 
     */
    ajaxCall: function (ajaxUrl, ajaxData) {
        sg.utls.ajaxRunning = true;
        return $.ajaxq("SageQueue", {
            url: ajaxUrl,
            data: JSON.stringify(ajaxData),
            method: "Post",
            headers: sg.utls.getHeadersForAjax(),
            dataType: "json",
            contentType: 'application/json',
            beforeSend: function () {
                $('#btnAjaxSpinner').show();
                sg.utls.showMessagesInViewPort();
            },
            complete: function () {
                $('#btnAjaxSpinner').hide();
                sg.utls.ajaxRunning = false;
                sg.utls.isProcessRunning = false;
            },
        }).then(function (data) {
            var d = $.Deferred();
            if (data && data.UserMessage && !data.UserMessage.IsSuccess) {
                return d.reject(data);
            }
            else if (data && !data.IsSuccess) {
                return d.reject(data);
            }
            return d.resolve(data);
        });
    },
};