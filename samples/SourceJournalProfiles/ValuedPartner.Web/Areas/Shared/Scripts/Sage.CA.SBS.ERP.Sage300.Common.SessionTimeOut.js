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
        sg.utls.showMessageDialog(SessionManager.ResetWarningTimer, null, globalResource.SessionExpired, sg.utls.DialogBoxType.OK, "", sg.utls.getFormatedDialogHtml());
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
