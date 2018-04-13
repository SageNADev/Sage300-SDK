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
