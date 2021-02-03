// The MIT License (MIT) 
// Copyright (c) 1994-2021 The Sage Group plc or its licensors.  All rights reserved.
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

// @ts-check

"use strict";

// Ajax call to controller
var sourceCodeAjax = {

    /**
     * @function
     * @name call
     * @description Generic method for making Ajax post calls
     * @namespace sourceCodeAjax
     * @public
     * 
     * @param {string} method The name of the method to call
     * @param {object} data The data payload object
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    call: function (method, data, callbackMethod) {
        let url = sg.utls.url.buildUrl("TU", "SourceCode", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var sourceCodeRepository = {

    /**
     * @function
     * @name get
     * @description Invoke the Source Code server-side Get method
     * @namespace sourceCodeAjax
     * @public
     *
     * @param {string} id The source code specifier
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    get: function(id, callbackMethod) {
        let data = { 'id': id };
        sourceCodeAjax.call("Get", data, callbackMethod);
    },

    /**
     * @function
     * @name create
     * @description Invoke the Source Code server-side Create method
     * @namespace sourceCodeAjax
     * @public
     *
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    create: function(callbackMethod) {
        let data = {};
        sourceCodeAjax.call("Create", data, callbackMethod);
    },

    /**
     * @function
     * @name delete
     * @description Invoke the Source Code server-side Delete method
     * @namespace sourceCodeAjax
     * @public
     *
     * @param {string} id The source code specifier
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    delete: function(id, callbackMethod) {
        let data = { 'id': id };
        sourceCodeAjax.call("Delete", data, callbackMethod);
    },

    /**
     * @function
     * @name delete
     * @description Invoke the Source Code server-side Add method
     * @namespace sourceCodeAjax
     * @public
     *
     * @param {string} id The source code specifier
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    add: function(data, callbackMethod) {
        sourceCodeAjax.call("Add", data, callbackMethod);
    },

    /**
     * @function
     * @name update
     * @description Invoke the Source Code server-side Update method
     * @namespace sourceCodeAjax
     * @public
     *
     * @param {string} id The source code specifier
     * @param {Function} callbackMethod The method to call upon a successful ajax call
     */
    update: function(data, callbackMethod) {
        sourceCodeAjax.call("Save", data, callbackMethod);
    }

    // Additional methods go here
};