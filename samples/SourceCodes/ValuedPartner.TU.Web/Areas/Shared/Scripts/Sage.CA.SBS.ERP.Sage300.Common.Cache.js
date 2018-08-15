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

(function (sage, $) {
    sage.cache = {
        _data: {},

        isSessionStorageSupported: function () {
            try {
                return 'sessionStorage' in window && window['sessionStorage'] !== null;
            } catch (e) {
                return false;
            }
        },

        // checks supported local cache which persists locally between sessions
        isLocalStorageSupported: function () {
            try {
                return 'localStorage' in window && window['localStorage'] !== null;
            } catch (e) {
                return false;
            }
        },

        get: function (token) {
            var val;
            if (sage.cache.isSessionStorageSupported()) {
                val = sessionStorage.getItem(token);
            }
            else {
                val = this._data[token];
            }

            if (val != null || typeof (val) == "object") {
                return JSON.parse(val);
            }
        },

        getLocalStorage: function (token) {
            var val;
            if (sage.cache.isLocalStorageSupported()) {
                val = localStorage.getItem(token);
            }
            else {
                val = this._data[token];
            }

            if (val != null || typeof (val) == "object") {
                return JSON.parse(val);
            }
        },

        set: function (token, payload) {
            var val = JSON.stringify(payload);
            if (sage.cache.isSessionStorageSupported()) {
                sessionStorage.setItem(token, val);
            } else {
                this._data[token] = val;
            }
        },

        setLocalStorage: function (token, payload) {
            var val = JSON.stringify(payload);
            if (sage.cache.isLocalStorageSupported()) {
                localStorage.setItem(token, val);
            } else {
                this._data[token] = val;
            }
        },

        clear: function (token) {
            if (sage.cache.isSessionStorageSupported()) {
                sessionStorage.removeItem(token);
            } else {
                this._data[token] = undefined;
            }
        },

        removeLocalStorage: function (token) {
            if (sage.cache.isLocalStorageSupported()) {
                localStorage.removeItem(token);
            } else {
                this._data[token] = undefined;
            }
        },

        clearAll: function () {
            if (sage.cache.isSessionStorageSupported()) {
                sessionStorage.clear();
            } else {
                this._data = {};
            }
        }
    };

    sage.dataManager = {
        dataDefaults: {
            dataType: 'json',
            type: 'POST'
        },

        sendRequest: function (options, key) {
            var that = sage.dataManager;
            var cacheKey = options.url + "_" + key;
            var cachedData = sage.cache.get(cacheKey);
            if (cachedData) {
                options.success(cachedData);
                return;
            }
            var callerOptions = $.extend({ cache: false }, that.dataDefaults, options);
            callerOptions.success = function (data) {
                sage.cache.set(cacheKey, data);
                options.success(data);
            };
            $.ajax(callerOptions);
        }
    };

}(this.sage = this.sage || {}, jQuery));