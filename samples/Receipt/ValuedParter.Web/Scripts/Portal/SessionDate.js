/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {

    $("#frmPortal").submit(function (event) {
        // prevent the default submission of the form
        event.preventDefault();
    });

    sessionDateCookieSetup.init();

    //keep track of changes made in the date picker textbox
    $("#datePicker").on('input', function () {
        $(this).data('currentValue', $(this).val());
    });
});

var verifySessionDateUrl = sg.utls.url.buildUrl("Core", "Home", "isInvalidSessionDate");
var warningType = { Inactive: 0, Locked: 1, NotInCalender: 2};
var sessionDateCookieSetup = sessionDateCookieSetup || {};

// Verification of Sesssion Date Succeeds
var onSuccess = {
    process: function(jsonResult) {
        if (jsonResult) {
            switch (jsonResult.type) {
                case warningType.NotInCalender:
                    sg.utls.showMessageDialog(null, null, jsonResult.Message, sg.utls.DialogBoxType.Continue,
                        globalResource.SessionDateNotInCalendar_Title, sg.utls.getFormatedDialogHtml());
                    break;
                case warningType.Inactive:
                    sg.utls.showMessageDialog(null, null, jsonResult.Message, sg.utls.DialogBoxType.Continue,
                        globalResource.SessionDateinInactiveYear_Title, sg.utls.getFormatedDialogHtml());
                    break;
                case warningType.Locked:
                    sg.utls.showMessageDialog(null, null, jsonResult.Message, sg.utls.DialogBoxType.Continue,
                        globalResource.SessionDateinLockedPeriod_Title, sg.utls.getFormatedDialogHtml());
                    break;
            }
        }
        
   }
};

sessionDateCookieSetup = {
    init: function () {
        sessionDateCookieSetup.readSessionDateCookie();
        sessionDateCookieSetup.sessionDateClick();

        window.sg.utls.ajaxPost(verifySessionDateUrl, null, onSuccess.process);
    },

    /*modifying session date in session date cookie*/
    saveSessionDate: function() {
        if ($("#frmPortal").valid()) {
            var selectedDateValue = $("#datePicker").data("kendoDatePicker").value();

            if (selectedDateValue) {
                // NOTE!!! Has to be done this way because the session date format in the cookie is fix between server and client, bad design, but oh well ....
                var selectedDate = selectedDateValue.getMonth() + 1 + "/" + selectedDateValue.getDate() + "/" + selectedDateValue.getFullYear();
                var todayDate = new Date();
                var sessionDateString = (selectedDate + " " + todayDate.getHours() + ":" + todayDate.getMinutes() + ":" + todayDate.getSeconds()).toString();
                var todayDateString = (todayDate.getMonth() + 1 + "/" + todayDate.getDate() + "/" + todayDate.getFullYear() + " " + todayDate.getHours() + ":" + todayDate.getMinutes() + ":" + todayDate.getSeconds()).toString();
                var modifiedCookie = sessionDateString + "|" + todayDateString;
                $("#divDatePicker").hide();
                $(".last_container").css("margin-top", "26px");
                $.cookie.raw = true;
                var cookieExpiresdate = new Date(9999, 12, 31);
                $.cookie(sg.utls.SessionCookieName, modifiedCookie, { path: '/', expires: cookieExpiresdate, secure: window.location.protocol === "http:" ? false : true });

                var spanSessionDate = $("#spnSessionDate").text();
                if (kendo.toString(selectedDateValue, 'MMM dd, yyyy') !== spanSessionDate) {
                    window.sg.utls.ajaxPost(verifySessionDateUrl, null, onSuccess.process);
                }
                $("#spnSessionDate").html(kendo.toString(selectedDateValue, 'MMM dd, yyyy'));

                var result = {};
                result.UserMessage = {};
                result.UserMessage.Message = portalBehaviourResources.SessionSuccessMsg;
                result.UserMessage.IsSuccess = true;
                sg.utls.showMessage(result);                  
            }
        }    
    },

    /* formatting session date before display */
    readSessionDateCookie: function () {
        var formattedDate = kendo.toString(sessionDateCookieSetup.parseCookieDate(), 'MMM dd, yyyy');
        $("#spnSessionDate").html(formattedDate);
    },

    /* reads session date from cookie and parses it */
    parseCookieDate: function () {
        var selectedDateValue = $.cookie(sg.utls.SessionCookieName).split('|')[0].split(' ')[0];
        var d = selectedDateValue.split('/');
        // NOTE!!! Has to be done this way because the session date format in the cookie is fix between server and client, bad design, but oh well ....
        return (d.length === 3) ? kendo.parseDate(d[2] + '-' + d[0] + '-' + d[1]) : kendo.parseDate(new Date());
    },

    /* session date click */
    sessionDateClick: function () {
        $("#spnSessionDate").on("click", function (e) {
            sessionDateCookieSetup.sessionDateClickCommon(e);
        });
        $("#sessionDatelabel").on("click", function (e) {
            sessionDateCookieSetup.sessionDateClickCommon(e);
        });
        $("#sessionDateIcon").on("click", function (e) {
            sessionDateCookieSetup.sessionDateClickCommon(e);
        });
    },

    sessionDateClickCommon: function (e) {

        if ($("#sessionDateIcon").hasClass("disabled")) {
            sg.utls.showMessageInfo(sg.utls.msgType.WARNING, portalBehaviourResources.SessionDateDisabledInfo);
        } else {
            e.stopPropagation();
            $(".last_container").css("margin-top", "9px");
            $("#datePicker").data("kendoDatePicker").value(sessionDateCookieSetup.parseCookieDate());
            $("#datePicker").focus();

            if ($("#divDatePicker").css("display") === "none") {
                sessionDateCookieSetup.openCalendar();
                sessionDateCookieSetup.calendarClose();
            } else {
                $("#datePicker").data("kendoDatePicker").close();
            }
        }
    },

    calendarClose: function(event) {
        var datePicker = $("#datePicker").data("kendoDatePicker");
        datePicker.bind("close", function(event) {
            sg.utls.clearValidations("frmPortal");
            $("#divDatePicker").hide();
            $(".last_container").css("margin-top", "26px");

            var $textBox = $('#datePicker');
            var formValid = $("#frmPortal").valid();
            var value = $textBox.data('currentValue');

            if (formValid && value != null && value.length > 0) {
                datePicker.value(value);
                $textBox.data('currentValue', '');
            }

            var currentDate = datePicker.value();

            // NOTE!!! Has to be done this way because the session date format in the cookie is fix between server and client, bad design, but oh well ....
            var array = $.cookie(sg.utls.SessionCookieName).split("|");
            if (typeof array[0] !== 'undefined') {
                var dt = array[0].split(' ');
                if (dt.length === 2) {
                    var d = dt[0].split('/');
                    if (d.length === 3) {
                        //Save session date if date in cookie is different from the current date
                        if (parseInt(d[0], 10) !== (currentDate.getMonth() + 1) ||
                            parseInt(d[1], 10) !== currentDate.getDate() ||
                            parseInt(d[2], 10) !== currentDate.getFullYear()) {
                            sessionDateCookieSetup.saveSessionDate();
                        }
                    }
                }
            }
            datePicker.unbind('close');
        })
    },

    openCalendar: function () {

        $("#divDatePicker").removeClass('hide').show();
        var datePicker = $("#datePicker").data("kendoDatePicker");

        // NOTE!!! Has to be done this way because the session date format in the cookie is fix between server and client, bad design, but oh well ....
        var array = $.cookie(sg.utls.SessionCookieName).split("|");
        if (typeof array[0] !== 'undefined') {
            var dt = array[0].split(' ');
            if (dt.length === 2) {
                var d = dt[0].split('/');
                if (d.length === 3) {
                    $("#datePicker").data("kendoDatePicker").value(new Date(d[2], d[0] - 1, d[1]));
                }
            }
        }
        datePicker.open();
    }
}
