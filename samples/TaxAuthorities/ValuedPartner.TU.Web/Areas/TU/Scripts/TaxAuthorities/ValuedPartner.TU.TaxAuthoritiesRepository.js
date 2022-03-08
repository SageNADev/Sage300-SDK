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

// @ts-check

"use strict";

/*jshint -W097 */

// Ajax call to controller
var taxAuthoritiesAjax = {

    /**
     * @name call
     * @description Common function used to invoke ajax calls
     * @namespace taxAuthoritiesAjax
     * @public 
     *  
     * @param {string} method The method to invoke
     * @param {object} data The data for the method call
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    call: (method, data, callbackMethod) => {
        let url = sg.utls.url.buildUrl("TU", "TaxAuthorities", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var taxAuthoritiesRepository = {

    /**
     * @name get
     * @description tax authority get call
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {string} id The tax authority specifier
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    get: (id, callbackMethod) => {
        var data = { 'id': id };
        taxAuthoritiesAjax.call("Get", data, callbackMethod);
    },

    /**
     * @name create
     * @description tax authority create call
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    create: (callbackMethod) => {
        let data = {};
        taxAuthoritiesAjax.call("Create", data, callbackMethod);
    },

    /**
     * @name delete
     * @description tax authority delete call
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {string} id The tax authority specifier
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    delete: (id, callbackMethod) => {
        let data = { 'id': id };
        taxAuthoritiesAjax.call("Delete", data, callbackMethod);
    },

    /**
     * @name add
     * @description tax authority add call
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {object} data The tax authority data object
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    add: (data, callbackMethod) => {
        taxAuthoritiesAjax.call("Add", data, callbackMethod);
    },

    /**
     * @name update
     * @description tax authority update call
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {object} data The tax authority data object
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    update: (data, callbackMethod) => {
        taxAuthoritiesAjax.call("Save", data, callbackMethod);
    },

    /**
     * @name getAccountDescription
     * @description tax authority get account description
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {object} data The tax authority data object
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    getAccountDescription: (data, callbackMethod) => {
        taxAuthoritiesAjax.call("GetAccount", data, callbackMethod);
    },

    /**
     * @name getCurrencyDescription
     * @description tax authority get currency description
     * @namespace taxAuthoritiesAjax
     * @public
     *
     * @param {object} data The tax authority data object
     * @param {Function} callbackMethod The callback that's called on successful ajax post
     */
    getCurrencyDescription: ( data, callbackMethod) => {
        taxAuthoritiesAjax.call("GetCurrencyDescription", data, callbackMethod);
    }    
};