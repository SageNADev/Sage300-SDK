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

