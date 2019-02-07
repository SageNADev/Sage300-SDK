// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.
"use strict";

var adhocInquiryAjax = {
    area: "Core",
    screenName: "AdhocInquiry",
    generateURL: function(method) {
        return sg.utls.url.buildUrl(adhocInquiryAjax.area, adhocInquiryAjax.screenName, method);
    },
    ajaxCall: function(method, data, callbackMethod) {
        var url = adhocInquiryAjax.generateURL(method);
        var dataItems = ko.mapping.toJS(data);
        sg.utls.ajaxPost(url, dataItems, callbackMethod);
    },
    ajaxSyncCall: function(method, data, callbackMethod) {
        var url = adhocInquiryAjax.generateURL(method);
        var dataItems = ko.mapping.toJS(data);
        sg.utls.ajaxPostSync(url, dataItems, callbackMethod);
    }
};

var adhocInquiryRepository = {
    SaveQuery: function(data, callbackMethod) {
        adhocInquiryAjax.ajaxCall("SaveQuery", data, callbackMethod);
    },

    DeleteQuery: function(data, callbackMethod) {
        adhocInquiryAjax.ajaxCall("DeleteQuery", data, callbackMethod);
    },

    CheckQueryExist: function (data, callbackMethod) {
        adhocInquiryAjax.ajaxCall("CheckQueryExist", data, callbackMethod);
    },

    ApplyQuery: function (data, callbackMethod) {
        adhocInquiryAjax.ajaxCall("ApplyQuery", data, callbackMethod);
    }
}
