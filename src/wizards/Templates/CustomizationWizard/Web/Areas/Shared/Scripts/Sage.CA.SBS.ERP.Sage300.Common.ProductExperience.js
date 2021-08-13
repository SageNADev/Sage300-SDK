/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

// Product Experience - Pendo

(function (apiKey) {
    if (apiKey) {
        (function (p, e, n, d, o) {
            var v, w, x, y, z; o = p[d] = p[d] || {}; o._q = [];
            v = ['initialize', 'identify', 'updateOptions', 'pageLoad']; for (w = 0, x = v.length; w < x; ++w)(function (m) {
                o[m] = o[m] || function () { o._q[m === v[0] ? 'unshift' : 'push']([m].concat([].slice.call(arguments, 0))); };
            })(v[w]);
            y = e.createElement(n); y.async = !0; y.src = 'https://cdn.pendo.io/agent/static/' + apiKey + '/pendo.js';
            z = e.getElementsByTagName(n)[0]; z.parentNode.insertBefore(y, z);
        })(window, document, 'script', 'pendo');

        // Call this whenever information about your visitors becomes available
        // Please use Strings, Numbers, or Bools for value types.
        pendo.initialize({
            visitor: {
                // email:        // Optional
                // role:         // Optional

                // Sage 300 Visitor (User)
                id: sessionStorage.getItem("visitor_id"),   // Required if user is logged in
                company: sessionStorage.getItem("visitor_company"),
                role: sessionStorage.getItem("visitor_role"),
                user_language: sessionStorage.getItem("visitor_language"),
                user_locale: sessionStorage.getItem("visitor_locale")

                // You can add any additional visitor level key-values here,
                // as long as it's not one of the above reserved names.
            },

            account: {
                // name:         // Optional
                // planLevel:    // Optional
                // planPrice:    // Optional
                // creationDate: // Optional

                // Sage 300 (Account)
                id: sessionStorage.getItem("account_id"), // Highly recommended
                region: sessionStorage.getItem("account_region"),
                serialNumber: sessionStorage.getItem("account_serialNumber"),
                edition: sessionStorage.getItem("account_edition"),
                version: sessionStorage.getItem("account_version"),
                lanpakCount: sessionStorage.getItem("account_lanpakCount"),
                product: sessionStorage.getItem("account_product"),

                // You can add any additional account level key-values here,
                // as long as it's not one of the above reserved names.
            }
        });
    }
})(sessionStorage.getItem("engagementApiKey"));
