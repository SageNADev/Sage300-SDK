/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";
var sessionDateSetup = sessionDateSetup || {};
sessionDateSetup = {

    init: function () {

        var setSessionDate = false;
        var sessionDateCookie = $.cookie(sg.utls.SessionCookieName);
        if (!sessionDateCookie) {
            setSessionDate = true;
        } else {
            var sessionDateArray = sessionDateCookie.split("|");
            if (sessionDateArray.length != 2) {
                setSessionDate = true;
            }
            else {
                var sessionDate = new Date(sessionDateArray[0]);
                var modifiedDate = new Date(sessionDateArray[1]);
                var todayDate = new Date();

                var timeOutParts = sessionDateCookieConfig.timeOutValue.split(":");
                var timeOut = (parseInt(timeOutParts[0]) * 3600 + parseInt(timeOutParts[1]) * 60 + parseInt(timeOutParts[2])) * 1000;

                if (sessionDate < todayDate && (todayDate - modifiedDate) > timeOut) {
                    setSessionDate = true;
                }
            }
        }

        if (setSessionDate) {
            sessionDateSetup.createAndSetSessionDateCookie();
        }
    },

    /*modifying session date in session date cookie*/
    createAndSetSessionDateCookie: function() {
        var todayDate = new Date();
        var todayDateString = (todayDate.getMonth() + 1 + "/" + todayDate.getDate() + "/" + todayDate.getFullYear() + " " + todayDate.getHours() + ":" + todayDate.getMinutes() + ":" + todayDate.getSeconds()).toString();
        var modifiedCookie = todayDateString + "|" + todayDateString;
        $.cookie.raw = true;
        var cookieExpiresdate = new Date(9999, 11, 31);
        $.cookie(sg.utls.SessionCookieName, modifiedCookie, { path: '/', expires: cookieExpiresdate, secure: window.location.protocol === "http:" ? false : true });
    }
}

$(function () {
    sessionDateSetup.init();
});

