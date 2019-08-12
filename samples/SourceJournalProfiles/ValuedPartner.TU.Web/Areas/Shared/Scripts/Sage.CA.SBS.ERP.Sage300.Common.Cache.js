/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */

(function (sage, $) {
    sage.cache = {

        constants: {
            // Flag to allow bypassing of browser storage mechanisms
            // even if both localStorage and sessionStorage are available
            // Note: Normally just leave this set to false.
            BYPASSBROWSERSTORAGE: false,
        },

        // Flag to force fallback storage into behaving like browser storage.
        // Browser storage (localStorage and sessionStorage) stores data as strings only
        _fallbackStorageBehavesLikeBrowserStorage: false,

        /**
         * @name fallbackStorageBehavesLikeBrowserStorage
         * @description Force the fallback storage to behave like browser storage
         * @param {boolean} flag - true or false
         */
        fallbackStorageBehavesLikeBrowserStorage: function (flag) {
            sage.cache._fallbackStorageBehavesLikeBrowserStorage = flag;
        },

        /**
         * @name isSessionStorageSupported
         * @description Check to see if browser supports session storage
         * @returns {boolean} true = session storage supported | false = session storage not supported
         */
        isSessionStorageSupported: function () {
            if (sage.cache.constants.BYPASSBROWSERSTORAGE === true) {
                return false;
            }

            var x = "sage-test-string";
            try {
                sessionStorage.setItem(x, x);
                sessionStorage.removeItem(x);
                return true;
            } catch (e) {
                return false;
            }
        },

        /**
         * @name isLocalStorageSupported
         * @description Check to see if browser supports local storage (Persists between sessions)
         * @returns {boolean} true = local storage supported | false = local storage not supported
         */
        isLocalStorageSupported: function () {
            if (sage.cache.constants.BYPASSBROWSERSTORAGE === true) {
                return false;
            }

            try {
                var f1 = 'localStorage' in window;
                var f2 = window['localStorage'] !== null;
                var f3 = window.localStorage !== undefined;
                return f1 && f2 && f3;
            } catch (e) {
                return false;
            }
        },

        local: {
            // Object for when local storage not supported
            _data: {},

            /**
             * @name get
             * @description Get an item from browser local storage
             * @param {string} key - The key for the local storage item
             * @return {string} The data item from local storage
             */
            get: function (key) {
                var val = null;

                // Get the data from either local storage
                // or local object
                if (sage.cache.isLocalStorageSupported()) {
                    val = localStorage.getItem(key);
                }
                else {
                    if (this._data) {
                        if (this._data[key] !== undefined) {
                            val = this._data[key];
                        } else {
                            val = null;
                        }
                    }
                }

                // Try/Catch is used to handle situation where
                // text being read from localStorage was NOT stringified when added to localStorage
                // An example of this is the recently used window entries.
                // This type of string will throw an exception when attempting to JSON.parse it
                // Not pretty, but it works.
                var rval;
                try { rval = val ? JSON.parse(val) : null; } catch (e) { rval = val; }
                return rval;
            },

            /**
             * @name set
             * @description Gets an item from browser local storage
             * @param {string} key - The key for the session storage item
             * @param {string} payload - The data to store
             */
            set: function (key, payload) {

                // Because payload can accept boolean values (true | false), we cannot
                // just do a simple "if (payload) { }" statement to check to see if payload
                // has been defined.
                if (typeof payload === "boolean" ||
                    typeof payload === "number" ||
                    typeof payload === "string" && payload.length > 0 ||
                    typeof payload === "object") {

                    var val = payload;

                    if (sage.cache.isLocalStorageSupported()) {

                        // Only stringify if payload is NOT a string
                        if (typeof payload !== "string") {
                            val = JSON.stringify(payload);
                        }

                        localStorage.setItem(key, val);
                    } else {
                        if (this._data) {
                            if (sage.cache._fallbackStorageBehavesLikeBrowserStorage) {

                                // Only stringify if payload is NOT a string
                                if (typeof payload !== "string") {
                                    val = JSON.stringify(payload);
                                }

                                this._data[key] = val;
                            } else {
                                this._data[key] = val;
                            }
                        }
                    }
                }
            },

            /**
             * @name remove
             * @description Removes an item from browser local storage
             * @param {string} key - The key for the local storage item to be removed
             */
            remove: function (key) {
                if (sage.cache.isLocalStorageSupported()) {
                    localStorage.removeItem(key);
                } else {
                    delete this._data[key];
                }
            },

            /**
             * @name clearAll
             * @description Removes all items from browser local storage
             */
            clearAll: function () {
                if (sage.cache.isLocalStorageSupported()) {
                    localStorage.clear();
                } else {
                    this._data = {};
                }
            }
        },

        session: {
            // Object for when session storage not supported
            _data: {},

            /**
             * @name get
             * @description Gets an item from browser session storage
             * @param {string} key - The key for the session storage item
             * @returns {string} - The data for the specified session storage key
             */
            get: function (key) {
                // Get the data from either session storage
                // or local object
                if (sage.cache.isSessionStorageSupported()) {
                    val = sessionStorage.getItem(key);
                }
                else {
                    if (this._data) {
                        if (this._data[key] !== undefined) {
                            val = this._data[key];
                        } else {
                            val = null;
                        }
                    }
                }

                // Try/Catch is used to handle situation where
                // text being read from localStorage was NOT stringified when added to localStorage
                // An example of this is the recently used window entries.
                // This type of string will throw an exception when attempting to JSON.parse it
                // Not pretty, but it works.
                var rval;
                try { rval = val ? JSON.parse(val) : null; } catch (e) { rval = val; }
                return rval;
            },

            /**
             * @name set
             * @description Saves an item to browser session storage
             * @param {string} key - The key for the session storage item
             * @param {string} payload - The data to store
             */
            set: function (key, payload) {
                if (typeof payload === "boolean" ||
                    typeof payload === "number" ||
                    typeof payload === "string" && payload.length > 0 ||
                    typeof payload === "object") {

                    var val = payload;

                    if (sage.cache.isSessionStorageSupported()) {

                        // Only stringify if payload is NOT a string
                        if (typeof payload !== "string") {
                            val = JSON.stringify(payload);
                        }

                        sessionStorage.setItem(key, val);
                    } else {
                        if (this._data) {
                            if (sage.cache._fallbackStorageBehavesLikeBrowserStorage) {

                                // Only stringify if payload is NOT a string
                                if (typeof payload !== "string") {
                                    val = JSON.stringify(payload);
                                }

                                this._data[key] = val;
                            } else {
                                this._data[key] = val;
                            }
                        }
                    }
                }
            },

            /**
             * @name remove
             * @description Removes an item from browser session storage
             * @param {string} key - The key for the session storage item to be removed
             *                       Note: This is an alias for clear()
             */
            remove: function (key) {
                if (sage.cache.isSessionStorageSupported()) {
                    sessionStorage.removeItem(key);
                } else {
                    delete this._data[key];
                }
            },

            /**
             * @name clearAll
             * @description Removes all items from browser session storage
             *              Note: This is an alias for removeAll()
             */
            clearAll: function () {
                if (sage.cache.isSessionStorageSupported()) {
                    sessionStorage.clear();
                } else {
                    this._data = {};
                }
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
            var cachedData = sage.cache.session.get(cacheKey);
            if (cachedData) {
                options.success(cachedData);
                return;
            }
            var callerOptions = $.extend({ cache: false }, that.dataDefaults, options);
            callerOptions.success = function (data) {
                sage.cache.session.set(cacheKey, data);
                options.success(data);
            };
            $.ajax(callerOptions);
        }
    };

}(this.sage = this.sage || {}, jQuery));

