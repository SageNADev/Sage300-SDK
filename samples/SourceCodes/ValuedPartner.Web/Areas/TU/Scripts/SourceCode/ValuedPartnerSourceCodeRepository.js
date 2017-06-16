// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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
var sourceCodeAjax = {

    call: function (method, data, callbackMethod) {
        var url = sg.utls.url.buildUrl("TU", "SourceCode", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var sourceCodeRepository = {
    // Create
    create: function () {
        var data = {};
        sourceCodeAjax.call("Create", data, sourceCodeUISuccess.create);
    },

    // Get
    get: function (sourceLedger, sourceType) {
        var data = { 'sourceLedger': sourceLedger, 'sourceType': sourceType };
        sourceCodeAjax.call("Get", data, sourceCodeUISuccess.get);
    },

    // Delete
    delete: function (sourceLedger, sourceType) {
        var data = { 'sourceLedger': sourceLedger, 'sourceType': sourceType };
        sourceCodeAjax.call("Delete", data, sourceCodeUISuccess.delete);
    },

    // Add
    add: function (data) {
        sourceCodeAjax.call("Add", data, sourceCodeUISuccess.update);
    },

    // Update
    update: function (data) {
        sourceCodeAjax.call("Save", data, sourceCodeUISuccess.update);
    },

    // Additional methods go here

};