/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */

// @ts-check
"use strict";

var pageUnloadEventManager = (function (parent) {

    var constants = {
        KEY: "AllowPageUnloadEvent"
    };

    var _public = {
        /**
         * @name init
         * @description Object initializer
         */
        init: function () {
            if (!sage.cache.session.get(constants.KEY)) {
                _public.enable();
            }
        },

        /**
         * @name enable
         * @description Set the current status to enabled
         *              Executable code in pages that have an unload and/or beforeunload event
         *              will be run.
         */
        enable: function () {
            sage.cache.session.set(constants.KEY, true);
        },

        /**
         * @name disable
         * @description Set the current status to disabled
         *              Executable code in pages that have an unload and/or beforeunload event
         *              will NOT be run.
         */
        disable: function () {
            sage.cache.session.set(constants.KEY, false);
        },

        /**
         * @name isEnabled
         * @description Get the current enabled status of the flag
         * @returns {boolean} true : enabled | false : disabled
         */
        isEnabled: function () {
            return sage.cache.session.get(constants.KEY);
        }
    };

    // Publicly exposed methods
    return {
        init: _public.init,
        enable: _public.enable,
        disable: _public.disable,
        isEnabled: _public.isEnabled
    };
})(pageUnloadEventManager || {});

$(function () {
    pageUnloadEventManager.init();
});
