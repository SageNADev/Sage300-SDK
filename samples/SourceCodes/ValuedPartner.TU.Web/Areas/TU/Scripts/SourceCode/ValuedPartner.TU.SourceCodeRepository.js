// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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

//@ts-check

"use strict";

// Ajax call to controller
var sourceCodeAjax = {

    /**
     * @name call
     * @description Invoke ajax call
     * @namespace sourceCodeAjax
     * @public 
     *  
     * @param {string} method The method name
     * @param {object} data The data payload
     * @param {Function} callbackMethod The function to call on success
     */
    call: (method, data, callbackMethod) => {
        let url = sg.utls.url.buildUrl("TU", "SourceCode", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var sourceCodeRepository = {

    /**
     * @name create
     * @description Invoke ajax call to create a source code
     * @namespace sourceCodeRepository
     * @public
     */
    create: () => {
        let data = {};
        sourceCodeAjax.call("Create", data, sourceCodeUISuccess.create);
    },

    /**
     * @name get
     * @description Invoke ajax call to create a source code
     * @namespace sourceCodeRepository
     * @public
     * 
     * @param {string} sourceLedger The source ledger specification
     * @param {string} sourceType the source type specification
     */
    get: (sourceLedger, sourceType) => {
        let data = { 'sourceLedger': sourceLedger, 'sourceType': sourceType };
        sourceCodeAjax.call("Get", data, sourceCodeUISuccess.get);
    },

    /**
     * @name delete
     * @description Invoke ajax call to delete a source code
     * @namespace sourceCodeRepository
     * @public
     *
     * @param {string} sourceLedger The source ledger specification
     * @param {string} sourceType the source type specification
     */
    delete: (sourceLedger, sourceType) => {
        let data = { 'sourceLedger': sourceLedger, 'sourceType': sourceType };
        sourceCodeAjax.call("Delete", data, sourceCodeUISuccess.delete);
    },

    /**
     * @name add
     * @description Invoke ajax call to add a source code
     * @namespace sourceCodeRepository
     * @public
     *
     * @param {string} sourceLedger The source ledger specification
     * @param {string} sourceType the source type specification
     */
    add: (data) => {
        sourceCodeAjax.call("Add", data, sourceCodeUISuccess.update);
    },

    /**
     * @name update
     * @description Invoke ajax call to save/update a source code
     * @namespace sourceCodeRepository
     * @public
     *
     * @param {object} data The source code data
     */
    update: (data) => {
        sourceCodeAjax.call("Save", data, sourceCodeUISuccess.update);
    },

    // Add Additional methods here
};