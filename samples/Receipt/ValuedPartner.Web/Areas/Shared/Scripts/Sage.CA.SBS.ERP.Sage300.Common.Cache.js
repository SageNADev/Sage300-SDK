/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */

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