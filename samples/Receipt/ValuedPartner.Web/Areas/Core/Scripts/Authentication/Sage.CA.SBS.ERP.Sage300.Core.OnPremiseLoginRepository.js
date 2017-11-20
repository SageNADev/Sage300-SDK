/* Copyright (c) 1994-2015 Sage Software, Inc.  All rights reserved. */

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
    }
};

