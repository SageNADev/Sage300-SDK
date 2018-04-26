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

var SessionManager = {
    TimeoutInterval: 0,
    TimerIds: [],
    IdleStartTime: null,
    IsSessionTimedOut: false,
    WarningMinutes: 3,
    WarningTimerId: null,

    Initialize: function (timeoutInterval) {
        //initialize the variables
        SessionManager.TimeoutInterval = (timeoutInterval - SessionManager.WarningMinutes) * 60 * 1000;
        SessionManager.IdleStartTime = new Date();
        //multiple events can be defined here separated by space
        SessionManager.IsSessionTimedOut = false;
        SessionManager.TimerIds.push(setTimeout(SessionManager.WarningTimerHandler, SessionManager.TimeoutInterval));
    },

    ResetSessionTimer: function () {
        SessionManager.ClearAllTimeout();
        SessionManager.IdleStartTime = new Date();
        SessionManager.TimerIds.push(setTimeout(SessionManager.WarningTimerHandler, SessionManager.TimeoutInterval));
    },

    WarningTimerHandler: function () {
        SessionManager.ClearAllTimeout();
        SessionManager.WarningTimerId = setTimeout(SessionManager.PageExpiredHandler, SessionManager.WarningMinutes * 60 * 1000);
        sg.utls.showMessageDialog(SessionManager.ResetWarningTimer, null, globalResource.SessionExpired, sg.utls.DialogBoxType.OK, "", sg.utls.getFormatedDialogHtml("kendoConfirmationAcceptButtonTimeout", "kendoConfirmationCancelledButtonTimeout"), "kendoConfirmationAcceptButtonTimeout", "kendoConfirmationCancelledButtonTimeout");
    },

    PageExpiredHandler: function () {
        $(document).unbind('.idleTimer');
        SessionManager.IsSessionTimedOut = true;
        sg.utls.RedirectToTimeoutLanding()
    },

    ResetWarningTimer: function () {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Home", "Ping"), {}, $.noop()); // ping server with empty json
        clearTimeout(SessionManager.WarningTimerId);
        SessionManager.ResetSessionTimer();
    },

    ClearAllTimeout: function () {
        SessionManager.TimerIds = jQuery.grep(SessionManager.TimerIds, function (value) {
            clearTimeout(value);
            return false;
        });
    }
};
