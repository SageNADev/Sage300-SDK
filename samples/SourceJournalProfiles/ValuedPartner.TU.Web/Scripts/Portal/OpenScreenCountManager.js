/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */

// @ts-check
"use strict";

var openScreenCountManager = (function (parent, win) {

    var constants = {
        KEY: "ALLSESSIONS_OpenScreenCount",

        USE_LOCALSTORAGE: true,
        USE_SESSIONSTORAGE: false
    };

    /**
     * @name incrementCounter
     * @description Increment the current open screen counter and
     *              return the new open screen count.
     *              If count is not yet defined, just set it to 1
     *              Note: This applies to ALL user/company logins
     * @returns {number} currentCount - The newly updated count
     */
    function incrementCounter() {
        var key = openScreenCountManager.KEY;

        var currentCount = 0;

        // get is a locally defined function
        currentCount = openScreenCountManager.get();

        if (currentCount) {
            currentCount++;
        } else {
            currentCount = 1;
        }

        // Save the updated (or new) value
        if (openScreenCountManager.constants.USE_LOCALSTORAGE) {
            sage.cache.local.set(key, currentCount);
        } else if (openScreenCountManager.constants.USE_SESSIONSTORAGE) {
            sage.cache.session.set(key, currentCount);
        }

        return currentCount;
    }

    /**
     * @name decrementCounter
     * @description Decrement the current open screen counter and
     *              return the new open screen count.
     *              If count is not yet defined, just set it to zero
     *              or if count falls below zero, reset it to zero.
     *              Note: This applies to ALL user/company logins
     * @returns {number} currentCount - The newly updated count
     */
    function decrementCounter() {
        var key = openScreenCountManager.KEY;
        
        var currentCount = 0;

        // get is a locally defined function
        currentCount = openScreenCountManager.get();

        if (currentCount) {
            currentCount--;
            if (currentCount <= 0) {
                // If count falls to zero (or below), remove the key 
                openScreenCountManager.remove();
                currentCount = 0;
            } else {
                // New value is greater than zero
                // Save the updated (or new) value
                if (openScreenCountManager.constants.USE_LOCALSTORAGE) {
                    sage.cache.local.set(key, currentCount);
                } else if (openScreenCountManager.constants.USE_SESSIONSTORAGE) {
                    sage.cache.session.set(key, currentCount);
                }
            }
        } else {
            // TODO - probably don't need this
            currentCount = 1;
        }

        return currentCount;
    }

    /**
     * @name get
     * @description Get the current open screen count
     *              Note: This applies to ALL user/company logins
     * @returns {number} currentCount - The current open screen count
     */
    function get() {
        var key = openScreenCountManager.KEY;

        if (openScreenCountManager.constants.USE_LOCALSTORAGE) {
            return sage.cache.local.get(key);
        } else if (openScreenCountManager.constants.USE_SESSIONSTORAGE) {
            return sage.cache.session.get(key);
        }
    }

    /**
     * @name set
     * @description Set the current open screen count
     *              Note: This applies to ALL user/company logins
     * @param {number} newCount - The updated count of open screens
     */
    function set(newCount) {
        var key = openScreenCountManager.KEY;

        if (openScreenCountManager.constants.USE_LOCALSTORAGE) {
            sage.cache.local.set(key, newCount);
        } else if (openScreenCountManager.constants.USE_SESSIONSTORAGE) {
            sage.cache.session.set(key, newCount);
        }
    }

    /**
     * @name remove
     * @description Remove the cached entry for OpenScreenCount
     *              Note: This applies to ALL user/company logins
     */
    function remove() {
        var key = openScreenCountManager.KEY;

        if (openScreenCountManager.constants.USE_LOCALSTORAGE) {
            sage.cache.local.remove(key);
        } else if (openScreenCountManager.constants.USE_SESSIONSTORAGE) {
            sage.cache.session.remove(key);
        }
    }

    /**
     * @name anyActiveScreens
     * @description Are there currently any active screens?
     * @returns {boolean} true - There are active screens | false - There are no active screens
     */
    function anyActiveScreens() {
        var any = false;
        var count = openScreenCountManager.get();
        any = !count ? false : count && count > 0 ? true : false; 
        return any;
    }

    /**
     * @name getCountForCurrentTab
     * @description Determine a count of the number of open screens for a given tab
     * @returns {number} The count of the number of open screens for a given tab
     */
    function getCountForCurrentTab() {
        var count = 0;
        $("#screenLayout").children().find("iframe").each(function () {
            var $item = $(this);
            var src = $item[0].src;
            if (src.length > 0) {
                var parts = src.split('/');
                var len = parts.length;
                if (len > 0) {
                    if (parts[len - 2] !== "Core" && parts[len - 1] !== "Home") {
                        count++;
                    }
                }
            }
        });
        return count;
    }


    // Publicly exposed methods
    return {
        KEY: constants.KEY,
        constants: constants,
        incrementCounter: incrementCounter,
        decrementCounter: decrementCounter,
        get: get,
        set: set,
        remove: remove,
        anyActiveScreens: anyActiveScreens,
        getCountForCurrentTab: getCountForCurrentTab
    };
})(openScreenCountManager || {}, window);
