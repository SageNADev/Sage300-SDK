/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */
var sg = sg || {};
sg.utls = sg.utls || {};

sg.utls.URL = {

    BuildUrl: function (action) {
        return url = location.href + '/' + action;
    },

    BuildFinderUrl: function (area, controller, action) {
        var baseUrl = sg.utls.URL.getBaseURL();
        return url = baseUrl + '/' + area + '/' + controller + '/' + action;
    },

    getBaseURL: function () {
        var url = location.protocol + "//" + location.hostname + location.pathname;
        var pathname = location.pathname;
        var index1 = url.indexOf(pathname);
        var index2 = url.indexOf("/", index1 + 1);
        var baseLocalUrl = url.substr(0, index2);
        return baseLocalUrl;
	},

	/**
 	  * Get the value of the specified url parameter
	  *
	  * @param {string} param - The name of the url paramter
	  * @returns {any} The value of the specified parameter
	*/
	getUrlParameter: function (param) {
		var sPageURL = window.location.search.substring(1),
			sURLVariables = sPageURL.split('&'),
			sParameterName,
			i;

		for (i = 0; i < sURLVariables.length; i++) {
			sParameterName = sURLVariables[i].split('=');

			if (sParameterName[0] === param) {
				sParameterName[1] = decodeURIComponent(sParameterName[1]) ;
				return sParameterName[1] === undefined ? true : sParameterName[1];
			}
		}
	},
};