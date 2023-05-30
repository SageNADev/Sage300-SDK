/* Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved. */

var SessionManager = {

    TimeoutInterval: 0,
    SessionTimer: null,
    IdleStartTime: null,
    IsSessionTimedOut: false,
    WarningMinutes: 1,
    WarningTimer: null,

    Initialize: function (timeoutInterval) {
        // Initialize the variables
        SessionManager.TimeoutInterval = (timeoutInterval - SessionManager.WarningMinutes) * 60 * 1000;
        SessionManager.IdleStartTime = new Date();

        // Multiple events can be defined here separated by space
        SessionManager.IsSessionTimedOut = false;
        SessionManager.SessionTimer = setTimeout(SessionManager.WarningTimerHandler, SessionManager.TimeoutInterval);
    },

    // Notify all tab pages to reset session/warning timer with same user/company
    NotifyResetTimer: function (isSession) {
        var sessionId = sg.utls.getSessionId();
        if (sessionId.length > 0) {
            var key = sessionId + (isSession ? "_ResetSessionTimer" : "_ResetWarningTimer");
            sage.cache.local.set(key, new Date().getTime().toString());
        }
    },

    // Reset log out timer
    ResetSessionTimer: function () {
        clearTimeout(SessionManager.SessionTimer);
        SessionManager.IdleStartTime = new Date();
        SessionManager.SessionTimer = setTimeout(SessionManager.WarningTimerHandler, SessionManager.TimeoutInterval);
        SessionManager.NotifyResetTimer(true);
    },

    // Show warning message that user will be logged out soon
    WarningTimerHandler: function () {
        clearTimeout(SessionManager.SessionTimer);
        SessionManager.WarningTimer = setTimeout(SessionManager.PageExpiredHandler, SessionManager.WarningMinutes * 60 * 1000);
        sg.utls.showMessageDialog(SessionManager.ResetWarningTimer, null, globalResource.SessionExpired, sg.utls.DialogBoxType.OK, "",
            sg.utls.getFormatedDialogHtml("kendoConfirmationAcceptButtonTimeout", null), "kendoConfirmationAcceptButtonTimeout", null, true);
    },

    // Redirect to timed out page
    PageExpiredHandler: function () {
        $(document).off('.idleTimer');
        SessionManager.IsSessionTimedOut = true;

        // Set timeout to localStorage trigger storage change, it post message to outside application, like CRM to relogin Sage300
        var sessionId = sg.utls.getSessionId();
        var key = sessionId + "_Sage300Timeout";
        localStorage[key] = new Date().getTime().toString();

        sg.utls.RedirectToTimeoutLanding();
    },

    // Reset warning timer
    ResetWarningTimer: function () {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Home", "Ping"), {}, $.noop()); // ping server with empty json
        clearTimeout(SessionManager.WarningTimer);
        SessionManager.ResetSessionTimer();
        SessionManager.NotifyResetTimer(false);
    }
};

// Multiple tab pages reset session timeout
(function () {
    $(window).on('storage', function (e) {
        if (e && e.originalEvent && e.originalEvent.key) {
            var keys = e.originalEvent.key.split('_');
            if (keys.length === 2) {
                var userCompany = keys[0];
                var url = location.href;
                var idx = url.indexOf(userCompany);
                var timeDiff = e.originalEvent.newValue - e.originalEvent.oldValue;

                // idx > 0 checks for same company
                if (idx > 0 && keys[1] === "LoggedIn") {
                    // for the scenario where the warning timer popup is already shown in tab 1 when logging into tab 2 (for the same company)
                    $("#kendoConfirmationAcceptButtonTimeout").trigger('click');
                } else if (idx > 0 && timeDiff > 1000) {
                    if (keys[1] === "ResetSessionTimer") {
                        SessionManager.ResetSessionTimer();
                    }

                    if (keys[1] === "ResetWarningTimer") {
                        SessionManager.ResetWarningTimer();
                        // clicking OK in one warning timer popup will dismiss the prompt in all tabs (for the same company)
                        $("#kendoConfirmationAcceptButtonTimeout").trigger('click');
                    }
                }
            }
        }
    });
})();
