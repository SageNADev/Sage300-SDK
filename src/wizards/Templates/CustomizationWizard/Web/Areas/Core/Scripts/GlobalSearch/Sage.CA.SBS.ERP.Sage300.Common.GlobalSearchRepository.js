// Copyright (c) 2018 Sage Software, Inc.  All rights reserved.
"use strict";

var globalSearchAjax = {
    area: "Core",
    screenName: "GlobalSearch",
    generateURL: function (method) {
        return sg.utls.url.buildUrl(globalSearchAjax.area, globalSearchAjax.screenName, method);
    },
    ajaxCall: function (method, data, callbackMethod) {
        var url = globalSearchAjax.generateURL(method);
        var dataItems = ko.mapping.toJS(data);
        sg.utls.ajaxPost(url, dataItems, callbackMethod);
    }
};

var globalSearchRepository = {
    Search: function(data, callbackMethod) {
        globalSearchAjax.ajaxCall("Search", data, callbackMethod)
    },
    ResetCompany: function (data, callbackMethod) {
        globalSearchAjax.ajaxCall("ResetCompany", data, callbackMethod)
    }
}
