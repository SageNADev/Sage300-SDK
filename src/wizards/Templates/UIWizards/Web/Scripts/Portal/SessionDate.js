/* Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved. */

//@ts-check
"use strict";

var SessionDateCookieSetup = function () {

    var constants = {
        CONTROL_ID: "#datePicker",
        DATE_FORMAT: 'MMM dd, yyyy',
        CALENDARCLOSE_DELAY_MS: 300
    };

    var verifySessionDateUrl = sg.utls.url.buildUrl("Core", "Home", "isInvalidSessionDate");
    var warningType = { Inactive: 0, Locked: 1, NotInCalender: 2 };
    var isDatePickerOpen = false;

    // Verification of Session Date Succeeds
    var onSuccess = {
        process: function(jsonResult) {
            if (jsonResult) {
                var portalWnd = window.top ? window.top : window;
                var title = '';
                switch (jsonResult.type) {
                    case warningType.NotInCalender:
                        title = portalWnd.globalResource.SessionDateNotInCalendar_Title;
                        break;

                    case warningType.Inactive:
                        title = portalWnd.globalResource.SessionDateinInactiveYear_Title;
                        break;

                    case warningType.Locked:
                        title = portalWnd.globalResource.SessionDateinLockedPeriod_Title;
                        break;
                }

                sg.utls.showMessageDialog(null, null, jsonResult.Message, sg.utls.DialogBoxType.Continue,
                    title, sg.utls.getFormatedDialogHtml(), null, null, true);
            }
       }
    };

    /**
     * @name isSessionDateSelectorEnabled
     * @description Check to see if the session date selector is enabled or disabled
     * @returns {boolean} true : enabled | false : disabled
     */
    function isSessionDateSelectorEnabled() {
        var flag = $("#sessionDateIcon").hasClass("disabled");
        return !flag;
    }

    /**
     * @name saveSessionDate
     * @description Modifying session date in session date cookie
     */
    function saveSessionDate() {
        if ($("#frmPortal").valid()) {
            var selectedDateValue = _public.getControl().value();

            if (selectedDateValue) {
                // NOTE!!! Has to be done this way because the session date format in the cookie is fixed
                // between server and client, bad design, but oh well ....
                var selectedDate = selectedDateValue.getMonth() + 1 + "/" + selectedDateValue.getDate() + "/" + selectedDateValue.getFullYear();
                var todayDate = new Date();
                var sessionDateString = (selectedDate + " " + todayDate.getHours() + ":" + todayDate.getMinutes() + ":" + todayDate.getSeconds()).toString();
                var todayDateString = (todayDate.getMonth() + 1 + "/" + todayDate.getDate() + "/" + todayDate.getFullYear() + " " + todayDate.getHours() + ":" + todayDate.getMinutes() + ":" + todayDate.getSeconds()).toString();
                var modifiedCookie = sessionDateString + "|" + todayDateString;
                $("#divDatePicker").hide();
                $(".last_container").css("top", "-28px");
                $.cookie.raw = true;
                var cookieExpiresdate = new Date(9999, 12, 31);
                $.cookie(sg.utls.SessionCookieName, modifiedCookie, { path: '/', expires: cookieExpiresdate, secure: window.location.protocol === "http:" ? false : true });

                var spanSessionDate = $("#spnSessionDate").text();
                var formattedDate = kendo.toString(selectedDateValue, constants.DATE_FORMAT); 
                if (formattedDate !== spanSessionDate) {
                    sg.utls.ajaxPost(verifySessionDateUrl, null, onSuccess.process);
                }
                $("#spnSessionDate").html(formattedDate);

                // Put the new session date into local storage and let the message handler
                // update the session date on any other open tabs for this user/company.
                var key = "ALLSESSIONS_UpdatePortalSessionDate";
                sage.cache.local.set(key, formattedDate);

                sg.utls.showMessageInfo(sg.utls.msgType.WARNING, portalBehaviourResources.SessionDateChangedWarning);

            }
        }
    }

    /**
     * @name showSuccessMessage
     * @description Display the session date updated successfully message
     */
    function showSuccessMessage() {
        var result = {
            UserMessage: {
                Message: portalBehaviourResources.SessionSuccessMsg,
                IsSuccess: true
            }
        };
        sg.utls.showMessage(result);
    }

    /**
     * @name readSessionDateCookie
     * @description Formatting session date before display
     */
    function readSessionDateCookie() {
        var formattedDate = kendo.toString(parseCookieDate(), constants.DATE_FORMAT);
        $("#spnSessionDate").html(formattedDate);
    }

    /**
     * @name parseCookieDate
     * @description Reads session date from cookie and parses it
     * @returns {Date} The parsed date from the cookie
     */
    function parseCookieDate() {
        var selectedDateValue = $.cookie(sg.utls.SessionCookieName).split('|')[0].split(' ')[0];
        var d = selectedDateValue.split('/');
        // NOTE!!! Has to be done this way because the session date format in the cookie is fixed
        // between server and client, bad design, but oh well ....
        return (d.length === 3) ? kendo.parseDate(d[2] + '-' + d[0] + '-' + d[1]) : kendo.parseDate(new Date());
    }

    /**
     * @name sessionDateClick
     * @description Session date click handler
     */
    function sessionDateClick() {
        $(".last_container.session-date").on("click", function (e) {
            sessionDateClickCommon(e);
        });
    }

    /**
     * @name sessionDateClickCommon
     * @description Click handler for session date selector
     * @param {any} e - The event
     */
    function sessionDateClickCommon(e) {

        if (!isSessionDateSelectorEnabled()) {
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, portalBehaviourResources.SessionDateDisabledInfo);
            return;
        }

        // If DatePicker is open, prevent it from opening again.
        if (isDatePickerOpen) {
            return;
        }

        _public.getControl().value(parseCookieDate());
        $(constants.CONTROL_ID).focus();

        if ($("#divDatePicker").css("display") === "none") {
            openCalendar();
            initCalendarCloseHandler(e);
        }
    }

    /**
     * @name initCalendarCloseHandler
     * @description Hookup the session date selector close handler
     * @param {any} event - The event
     */
    function initCalendarCloseHandler(event) {
        var datePicker = _public.getControl();
        datePicker.bind("close", function (event) {

            // See if the Session Date selector has been disabled by another active screen
            if (isSessionDateSelectorEnabled()) {
                sg.utls.clearValidations("frmPortal");
                var $textBox = $(constants.CONTROL_ID);
                var formValid = $("#frmPortal").valid();
                var value = $textBox.data('currentValue');

                if (formValid && value && value.length > 0) {
                    datePicker.value(value);
                    $textBox.data('currentValue', '');
                }

                var currentDate = datePicker.value();

                // NOTE!!! Has to be done this way because the session date format in the cookie is fixed
                // between server and client, bad design, but oh well ....
                var array = $.cookie(sg.utls.SessionCookieName).split("|");
                if (typeof array[0] !== 'undefined') {
                    var dt = array[0].split(' ');
                    if (dt.length === 2) {
                        var d = dt[0].split('/');
                        if (d.length === 3) {
                            // Save session date if date in cookie is different from the current date
                            if (parseInt(d[0], 10) !== (currentDate.getMonth() + 1) ||
                                parseInt(d[1], 10) !== currentDate.getDate() ||
                                parseInt(d[2], 10) !== currentDate.getFullYear()) {
                                saveSessionDate();
                            }
                        }
                    }
                }
            } else {
                // D-39978
                // Session date selector is disabled so we won't allow selection of a new date
                // even though date popup is visible. The session date selector may have been
                // disabled by the opening of a screen in another tab while the date selector
                // was visible. Let's just close the calendar window instead
            }

            // Code below will close the date picker
            datePicker.unbind('close');
            $("#divDatePicker").hide();
            $(".last_container").css("top", "-28px");

            // Delay 300 milliseconds to allow Kendo DatePicker to close properly
            setTimeout(function () {
                isDatePickerOpen = false;
            }, constants.CALENDARCLOSE_DELAY_MS);

        });
    }

    /**
     * @name openCalendar
     * @description TODO - Add description here
     */
    function openCalendar() {

        $("#divDatePicker").show();
        $(".last_container").css("top", "-1px");
        var datePicker = _public.getControl();

        // NOTE!!! Has to be done this way because the session date format in the cookie is fixed
        // between server and client, bad design, but oh well ....
        var array = $.cookie(sg.utls.SessionCookieName).split("|");
        if (typeof array[0] !== 'undefined') {
            var dt = array[0].split(' ');
            if (dt.length === 2) {
                var d = dt[0].split('/');
                if (d.length === 3) {
                    datePicker.value(new Date(d[2], d[0] - 1, d[1]));
                }
            }
        }
        datePicker.open();
        isDatePickerOpen = true;
    }

    var _public = {

        id: constants.CONTROL_ID,

        /**
         * @name init
         * @description Main initialization function
         * @public
         */
        init: function () {
            readSessionDateCookie();
            sessionDateClick();

            sg.utls.ajaxPost(verifySessionDateUrl, null, onSuccess.process);

        },

        /**
         * @name getControl
         * @description Get a reference to the kendo calendar control
         * @public
         * @returns {object} The kendo calendar control reference
         */
        getControl: function() {
            return $(constants.CONTROL_ID).data("kendoDatePicker");
        }
    };

    return _public;
};

// Declare this at global scope so it's available elsewheree.
var sessionDateCookieSetup;
$(function () {

    $("#frmPortal").submit(function (event) {
        // prevent the default submission of the form
        event.preventDefault();
    });

    sessionDateCookieSetup = new SessionDateCookieSetup();
    if (sessionDateCookieSetup) {
        sessionDateCookieSetup.init();
    }

    // Keep track of changes made in the date picker textbox
    $(sessionDateCookieSetup.id).on('input', function () {
        $(this).data('currentValue', $(this).val());
    });
});
