
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

//@ts-check

"use strict";

// Ajax call to controller
var segmentCodesAjax = {

    call: function (method, data, callbackMethod) {
        var url = sg.utls.url.buildUrl("TU", "SegmentCodes", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var segmentCodesRepository = {

	/**
     * @function
	 * @name get
	 * @description Get data
	 * @namespace segmentCodesRepository
	 * @public 
	 * 
	 * @param {number} id The segment code
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    get: function(id, callbackMethod) {
        var data = { 'id': id };
        segmentCodesAjax.call("Get", data, callbackMethod);
    },

	/**
	 * @function
	 * @name create
	 * @description Create a new segment code
	 * @namespace segmentCodesRepository
	 * @public
	 *
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    create: function(callbackMethod) {
        var data = {};
        segmentCodesAjax.call("Create", data, callbackMethod);
    },

	/**
	 * @function
	 * @name delete
	 * @description Delete segment code
	 * @namespace segmentCodesRepository
	 * @public
	 *
	 * @param {number} id The segment code
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    delete: function(id, callbackMethod) {
        var data = { 'id': id };
        segmentCodesAjax.call("Delete", data, callbackMethod);
    },

	/**
	 * @function
	 * @name add
	 * @description Add segment code
	 * @namespace segmentCodesRepository
	 * @public
	 *
	 * @param {number} id The segment code
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    add: function(data, callbackMethod) {
        segmentCodesAjax.call("Add", data, callbackMethod);
    },

	/**
	 * @function
	 * @name update
	 * @description Update segment code
	 * @namespace segmentCodesRepository
	 * @public
	 *
	 * @param {data} data The segment code data
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    update: function(data, callbackMethod) {
        segmentCodesAjax.call("Save", data, callbackMethod);
    },

	/**
	 * @function
	 * @name post
	 * @description post segment code
	 * @namespace segmentCodesRepository
	 * @public
	 *
	 * @param {Function} callbackMethod Callback method to call on success
	 */
    post: function(callbackMethod) {
        segmentCodesAjax.call("Post", null, callbackMethod);
    }

    // Additional methods go here
};