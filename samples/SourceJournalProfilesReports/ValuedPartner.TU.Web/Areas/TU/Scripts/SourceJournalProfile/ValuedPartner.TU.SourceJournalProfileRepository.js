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

// Ajax call to controller
var sourceJournalProfileAjax = {

    call: function (method, data, callbackMethod) {
        var url = sg.utls.url.buildUrl("TU", "SourceJournalProfile", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var sourceJournalProfileRepository = {

    // Get
    get: function(id, callbackMethod) {
        var data = { 'id': id };
        sourceJournalProfileAjax.call("Get", data, callbackMethod);
    },

    // Create
    create: function(callbackMethod) {
        var data = {};
        sourceJournalProfileAjax.call("Create", data, callbackMethod);
    },

    // Delete
    delete: function(id, callbackMethod) {
        var data = { 'id': id };
        sourceJournalProfileAjax.call("Delete", data, callbackMethod);
    },

    // Add
    add: function(data, callbackMethod) {
        sourceJournalProfileAjax.call("Add", data, callbackMethod);
    },

    // Update
    update: function(data, callbackMethod) {
        sourceJournalProfileAjax.call("Save", data, callbackMethod);
    }

    // Additional methods go here

};