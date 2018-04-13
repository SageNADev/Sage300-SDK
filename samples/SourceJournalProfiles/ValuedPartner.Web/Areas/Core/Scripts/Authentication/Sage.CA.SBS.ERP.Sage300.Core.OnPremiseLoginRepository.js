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

var loginRepository = {

    // Invoke controller to validate credentials and attempt login
    login: function (data, callback) {
        var url = sg.utls.url.buildUrl("Core", "Authentication", "LoginResultOnPremise");
        sg.utls.ajaxPost(url, data, callback);
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
                $('#ajaxSpinner').fadeIn(1);
                sg.utls.showMessagesInViewPort();
            },
            complete: function () {
                $('#ajaxSpinner').fadeOut(1);
                sg.utls.ajaxRunning = false;
                sg.utls.isProcessRunning = false;
            },
        }).then(function (data) {
            var d = $.Deferred();
            if (!data.UserMessage.IsSuccess) {
                return d.reject(data);
            }
            return d.resolve(data);
        });
    },
};