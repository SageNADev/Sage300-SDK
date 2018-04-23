/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

//TODO: to be replaced later using above.
var URL = {

    BuildUrl: function (action) {
        return url = location.href + '/' + action;
    },

    BuildFinderUrl: function (area, controller, action) {
        var baseUrl = URL.getBaseURL();
        return url = baseUrl + '/' + area + '/' + controller + '/' + action;
    },

    getBaseURL: function () {
        var url = location.protocol + "//" + location.hostname + location.pathname;
        var pathname = location.pathname;
        var index1 = url.indexOf(pathname);
        var index2 = url.indexOf("/", index1 + 1);
        var baseLocalUrl = url.substr(0, index2);
        return baseLocalUrl;
    }
};