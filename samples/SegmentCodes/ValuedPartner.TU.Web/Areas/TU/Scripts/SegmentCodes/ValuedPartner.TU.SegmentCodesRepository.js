
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
var segmentCodesAjax = {

    call: function (method, data, callbackMethod) {
        var url = sg.utls.url.buildUrl("TU", "SegmentCodes", method);
        sg.utls.ajaxPost(url, data, callbackMethod);
    }
};

var segmentCodesRepository = {

	/**
     * Get
	 *
	 * @method get
	 * @param id
	 * @param callbackMethod
	 */
    get: function(id, callbackMethod) {
        var data = { 'id': id };
        segmentCodesAjax.call("Get", data, callbackMethod);
    },

	/**
     * Create
	 *
	 * @method create
	 * @param callbackMethod
	 */
    create: function(callbackMethod) {
        var data = {};
        segmentCodesAjax.call("Create", data, callbackMethod);
    },

	/**
     * Delete
	 *
	 * @method delete
	 * @param id
	 * @param callbackMethod
	 */
    delete: function(id, callbackMethod) {
        var data = { 'id': id };
        segmentCodesAjax.call("Delete", data, callbackMethod);
    },

	/**
     * Add
	 *
	 * @method add
	 * @param data
	 * @param callbackMethod
	 */
    add: function(data, callbackMethod) {
        segmentCodesAjax.call("Add", data, callbackMethod);
    },

	/**
     * Update
	 *
	 * @method update
	 * @param data
	 * @param callbackMethod
	 */
    update: function(data, callbackMethod) {
        segmentCodesAjax.call("Save", data, callbackMethod);
    },

	/**
     * Post
	 *
	 * @method update
	 * @param data
	 * @param callbackMethod
	 */
    post: function(callbackMethod) {
        segmentCodesAjax.call("Post", null, callbackMethod);
    }

    // Additional methods go here
};