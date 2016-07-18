/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

(function (sage, $) {
    sage.cache = {
        _data: {},

        islocalStorageSupported: function () {
            try {
                return 'sessionStorage' in window && window['sessionStorage'] !== null;
            } catch (e) {
                return false;
            }
        },

        get: function (token) {
            var val;
            if (sage.cache.islocalStorageSupported()) {
                val = sessionStorage.getItem(token);
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
            if (sage.cache.islocalStorageSupported()) {
                sessionStorage.setItem(token, val);
            } else {
                this._data[token] = val;
            }
        },
        clear: function (token) {
            if (sage.cache.islocalStorageSupported()) {
                sessionStorage.removeItem(token);
            } else {
                this._data[token] = undefined;
            }
        },
        clearAll: function () {
            if (sage.cache.islocalStorageSupported()) {
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