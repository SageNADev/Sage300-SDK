/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */

// @ts-check

// Note: 
//       Enabling 'use strict' line below seems to cause unit tests to fail.
//       Also, using some ECMAScript 6 features will cause unit tests to fail.
//       For example, declaring a variable using 'let' instead of 'var' will
//       cause unit tests to fail.
//
//"use strict";

var sg = sg || {};
sg.utls = sg.utls || {};
sg.utls.kndoUI = sg.utls.kndoUI || {};
sg.utls.ko = sg.utls.ko || {};
sg.utls.finder = sg.utls.finder || {};
sg.utls.url = sg.utls.url || {};
sg.utls.msgType = sg.utls.msgType || {};
sg.utls.collapsibleScreen = sg.utls.collapsibleScreen || {};
sg.utls.OperationMode = { LOAD: 1, SAVE: 2, NEW: 3, DELETE: 4 };
sg.utls.isShiftKeyPressed = false;
sg.utls.istabKeyPressed = false;
sg.utls.isCtrlKeyPressed = false;
sg.utls.instantUpdateKO = true;
sg.utls.regExp = sg.utls.regExp || {};

sg.utls.DialogBoxType = {
    YesNo: 0,
    OKCancel: 1,
    Close: 2,
    OK: 3,
    DeleteCancel: 4,
    Continue: 5
};

sg.utls.scrollPosition = 0;
sg.utls.isFinderClicked = false;
sg.utls.SessionCookieName = "SessionDate";
sg.utls.screenUnloadHandler = null;
sg.utls.portalHeight = 225;
sg.utls.popupTopPosition = 0;
sg.utls.GridPrefParentForm = null;

sg.utls.NotesSearchType = {
    All: 0,
    Customers: 1,
    Vendors: 2,
    InventoryItems: 3
};

sg.utls.EntityErrorPriority = {
    SevereError: 0,
    Message: 1,
    Warning: 2,
    Error: 3,
    Security: 4
};

var fnTimeout = 0;

$.extend(sg.utls.regExp, {
    TIME: "([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]",
    COMMA: "\,",
    ONLYGLOBAL: "g"
});

$.extend(sg.utls.msgType, {
    ERROR: "error", INFO: "info", SUCCESS: "success", WARNING: "warning"
});

$.extend(sg.utls.url, {
    baseUrl: function () {
        return $("#hdnUrl").val();
    },
    buildUrl: function (area, controller, action) {
        var siteUrl = sg.utls.url.baseUrl(),
            slash = "/";
        siteUrl += area.length > 0 ? area : "";
        siteUrl += controller.length > 0 ? slash + controller : "";
        siteUrl += action.length > 0 ? slash + action : "";
        return siteUrl;
    },

    getParameterByName: function (name, url) {
        if (!url) {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
});

$.extend(sg.utls, {
    functionsToCall: [],
    ajaxRunning: false,
    isProcessRunning: false,
    gridPageSize: 10,
    reorderable: false,
    hasTriedToNotify: false,
    formatFiscalPeriod: function (fiscalPeriod) {
        if (fiscalPeriod === "14") {
            return "ADJ";
        } else if (fiscalPeriod === "15") {
            return "CLS";
        } else {
            return fiscalPeriod;
        }
    },
    progressBarControl: function (id, percentageComplete) {
        var $id = $(id);
        if (percentageComplete == 0) {
            $id.hide();
        } else {
            $id.show();
        }
        var $progressBar = $id.find(".progress-bar");
        if (percentageComplete > 80) {
            $progressBar.addClass('over-80');
        } else {
            $progressBar.removeClass('over-80');
        }
        $progressBar[0].style.width = percentageComplete + '%';
        $progressBar.find(".percentage")[0].innerHTML = percentageComplete + '% ' + globalResource.Complete;
    },

    showProgressBar: function(progressBar) {
        var processCount = 0;
        var increment = 2;
        var processTimer = setInterval(function () {
            if (processCount > 50 && processCount < 80) {
                increment = 1;
            } else if (processCount >= 80 && processCount < 90) {
                increment = 0.1;
            } else if (processCount >= 90 && processCount < 95) {
                increment = 0.01;
            } else if (processCount >= 95) {
                increment = 0.001;
            }
            processCount += increment;
            if (processCount > 100) {
                processCount = 100; 
            }
            sg.utls.progressBarControl(progressBar, processCount.toFixed(2));
        }, 1000);
        return processTimer;
    },

    convertEntityErrorsToUserMessage: function (errors) {
        if (errors.length > 0) {
            var isError = false;
            var isWarning = false;

            $.each(errors, function (index, value) {
                if (value.Priority == 0 || value.Priority >= 3) {
                    isError = true;
                    return false;
                } else if (value.Priority == 2) {
                    isWarning = true;
                }
            });

            if (isError) {
                return { Errors: errors };
            }
            else if (isWarning) {
                return { Warnings: errors };
            } else {
                return { Info: errors };
            }
        }
        return self.location === top.location;
    },
    isTopPage: function () {
        return self.location === top.location;
    },

    getCookie: function (name) {
        var re = new RegExp(name + "=([^;]+)");
        var value = re.exec(document.cookie);
        return (value != null) ? unescape(value[1]) : null;
    },

    isSameOrigin: function () {
        var requestBaseUrl = sg.utls.getCookie("baseUrl");
        if (requestBaseUrl) {
            return requestBaseUrl === window.location.protocol + window.location.host;
        }
        return true;
    },

    isPortalIntegrated: function () {
        var screenHome = "999999";
        if (sg.utls.isSameOrigin()) {
            return window.top.$("#ScreenName").val() === screenHome;
        }
        return false;
    },

    isKendoIframe: function () {
        if (sg.utls.isSameOrigin()) {
            var kendoIframe = window.top.$('iframe.screenIframe:visible').contents().find('.k-content-frame:visible');
            return kendoIframe.length > 0;
        }
        return false;
    },

    openExternalLink: function (URL, name) {
        window.open(URL, name);
    },

    //This method remove \n\n and puts \n. this is done to remove a extra line break in message
    formatMessageText: function (value) {
        value = value.replace("\n\n", "\n");
        return value;
    },

    htmlEncode: function (value) {

        if ($.isFunction(value)) {
            value = value("");
        }
        //This is to check if the string is already encoded, if it is do not encode again.
        var encodedCharacter = new RegExp("&lt;|&gt;|&quot;|&#|&amp;");
        if (!value.match(encodedCharacter)) {
            value = $('<div/>').text(value).html();
        }
        return value;
    },

    htmlDecode: function (value) {
        return $('<div/>').html(value).text();
    },

    removeColon: function (text) {
        return text.replace(/:/g, "");
    },

    getFirefoxSpecialKeys: function (e) {
        var fields = [8, 9];
        // to enable copy paste in firefox
        if (sg.utls.isCtrlKeyPressed) {
            fields.push(99);
            fields.push(118);
        }
        if (e.charCode === 0) {
            fields.push(46);//Delete key
            fields.push(37);//Arrow key
            fields.push(39);//Arrow Key
            fields.push(35);//End Key
            fields.push(36);//Home Key
        }
        return fields;
    },
    /**
     * Gets a value indicating whether the current browser is Internet Explorer.
     * 
     * @returns {boolean} True if the browser is Internet Explorer, otherwise false. 
     */
    isInternetExplorer: function () {
        var ua = window.navigator.userAgent;
        if ($.browser.msie || ua.indexOf('Trident/') > 0 || ua.indexOf('Edge/') > 0) {
            return true;
        }
        return false;
    },
    /**
     * Gets a value indicating whether the current browser is Mozilla Firefox.
     * 
     * @returns {boolean} True if the browser is Mozilla Firefox, otherwise false. 
     */
    isMozillaFirefox: function () {
        if ($.browser.mozilla && sg.utls.isInternetExplorer() == false) {
            return true;
        }
        return false;
    },
    /**
     * Gets a value indicating whether the current browser is Google Chrome.
     * 
     * @returns {boolean} True if the browser is Google Chrome, otherwise false. 
     */
    isChrome: function () {

        var isChrome = navigator.userAgent.indexOf('Chrome') != -1;

        return isChrome;

    },
    /**
     * Gets a value indicating whether the current browser is Apple Safari.
     * 
     * @returns {boolean} True if the browser is Apple Safari, otherwise false. 
     */
    isSafari: function () {

        var isSafari = (navigator.userAgent.indexOf('Safari') != -1
                            && navigator.userAgent.indexOf('Chrome') == -1)

        return isSafari;

    },
    /**
     * Gets a value indicating whether the current browser is operating on a mobile device.
     * 
     * @returns {boolean} True if the browser is operating on a mobile device, otherwise false. 
     */
    isMobile: function () {
        var isMobile = navigator.userAgent.indexOf('Mobile') !== -1;
        return isMobile;
    },

    refreshContainer: function (container) {
        $(container).find("input[data-val-length-max]").each(function () {
            var $this = $(this);
            var data = $this.data();
            if (data) {
                if (data.valLengthMax)
                    $this.attr("maxlength", data.valLengthMax);
            }
        });

    },
    registerDestroySession: function () {
        var sessionPerPage = $("#SessionPerPage");
        if (sessionPerPage.length === 0 || sessionPerPage.val() === "False") {
            $(window).bind('unload', function () {
                if (globalResource.AllowPageUnloadEvent) {
                    sg.utls.destroySession();
                }
            });
        }
    },
    releaseSession: function () {
        var sessionPerPage = $("#SessionPerPage");
        if (sessionPerPage.length === 0 || sessionPerPage.val() === "False") {
            sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Session", "ReleaseSession"), {}, function () { });
        }
    },
    destroySessions: function () {
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Session", "DestroyPool"), {}, function () { });
        sage.cache.clearAll();
        sg.utls.destroyPoolForReport(false);
    },
    destroySession: function () {
        sage.cache.clearAll();
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Session", "Destroy"), {}, function () { });
    },

    logOut: function (isAdminLogout) {

        var isAdminLogout = (typeof isAdminLogout !== 'undefined') ? isAdminLogout : false;

        var signOutLink = sg.utls.url.buildUrl("Core", "Authentication", "Logout");
        var loginLink = sg.utls.url.buildUrl("Core", "Authentication", isAdminLogout ? "AdminLogin" : "Login");
        var topWnd = window.top == null ? window : window.top;

        // Note: Potential tech debt here as this event will get fired AFTER the login has been redirected
        // to. Therefore, checks have been placed in the AuthenticationController.Login method to check for this
        // use case where the data still exists in the IIS cache

        //Tmp solution for sync logout, as mentioned, there are some issue for this logOut function. Just ask the user log out first due to time consuming
        $('#dvWindows').find('span').each(function (index, element) {
                var currentIframeId = $(this).attr("frameId");
                $("#" + currentIframeId).attr("src", "about:blank");
        });

        $(topWnd).bind('unload', function () {
            sg.utls.destroyPoolForReport(true);
            sage.cache.clearAll();
            sg.utls.ajaxPost(signOutLink);
        });

        topWnd.location.href = loginLink + "?logout=true";
    },

    unbindEvent: function (eventName) {
        $(window).off(eventName);
        $.each($('iframe'), function (i, currentIFrame) {
            var currentWindow = (currentIFrame.contentWindow || currentIFrame.contentDocument);
            if (currentWindow.$) {
                currentWindow.$(currentWindow).off(eventName);
            }
        });
    },
    destroyPoolForReport: function (callAsync) {
        if (globalResource.ReportUrl === "Report") {
            var reportUrl = $("#hdnUrl").val() + "../" + globalResource.ReportUrl + "/ReportViewer.aspx/DestroyPool";
            if (callAsync !== null && callAsync !== undefined && callAsync) {
                sg.utls.ajaxPost(reportUrl, {}, function () { });
            }
            else {
                sg.utls.ajaxPostSync(reportUrl, {}, function () { });
            }
        }
    },
    modelDialog: function (title, htmlData) {
        $("#exportImportDialog").html(htmlData).dialog({
            minWidth: 400,
            modal: true,
            closeOnEscape: true,
            resizable: true,
            buttons: {
                "Close": function () {
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                    $("#exportImportDialog").hide();
                }
            },
            title: title,
            dialogClass: "no-close ui-dialog-zindex"
        });
    },
    isRequired: function (element) {
        return ($(element).attr("data-val-required") != null);
    },
    homeCurrency: null,
    isPhoneNumberFormatRequired: null,
    loadHomeCurrency: function () {
        sg.utls.ajaxCache(sg.utls.url.buildUrl("CS", "CompanyProfile", "GetApplicationConfig"), {}, ajaxSuccess.getCurrency, "HomeCurrency");
    },
    openDialog: function (ajaxUrl, title) {
        var data = { ContextToken: $("#ContextToken").val() };
        $.ajax({
            url: ajaxUrl,
            data: data,
            contentType: 'application/json',
            success: function (result) {
                sg.utls.modelDialog(title, result);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#ajaxSpinner').fadeIn();
            },
            beforeSend: function () {
                $('#ajaxSpinner').fadeIn(1);
            },
            complete: function () {
                setTimeout(function () { $('#ajaxSpinner').fadeOut(1); }, 100);
            }
        });
    },
    getHomeCurrency: function () {
        var data = {};
        if (sg.utls.homeCurrency === null) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("CS", "CompanyProfile", "GetApplicationConfig"), data, ajaxSuccess.getCurrency);
        }

    },
    openReport: function (reportToken, checkTitle, callbackOnClose) {
        var reportUrl = $("#hdnUrl").val() + "../" + globalResource.ReportUrl + "/ReportViewer.aspx?token=" + reportToken;
        if (!sg.utls.isPortalIntegrated()) {
            window.open(reportUrl);
        } else {
            //TODO: this is method to open report in Portal Windows Dock
            var reportName = $("section.header-group h3").html();
            if (reportName === undefined) {    // Transition to R3 layout
                reportName = $("section.header-group-1 h3").html();
            }
            if (checkTitle !== undefined && typeof checkTitle == 'string') {
                reportName = checkTitle
            }
            window.top.postMessage("isReport" + " " + reportUrl + " " + reportName + " " + $('form').prop('action'), "*");

            // If provided a callback, bind it to the report (crystal) window
            if (callbackOnClose !== undefined) {
                // Need to delay a bit for the window to be established
                setTimeout(function () {
                    // Get the iFrame object where the report is loaded
                    var iFrameObject = sg.utls.getReportIFrame(reportToken);
                    if (iFrameObject !== undefined) {
                        // Get the report window
                        var reportWin = iFrameObject.contentWindow;
                        // Need to bind to an event   
                        $(iFrameObject).load(function () {
                            // Bind to the before unload event
                            $(reportWin).bind("beforeunload", function () {
                                // Invoke the callback
                                callbackOnClose.call();
                            })
                        })
                    }
                }, 500);
            }

        }
    },

    // Determines which iFrame the report has been opened in
    getReportIFrame: function (reportToken) {

        // Locals
        var i;
        var id;
        var iFrameObject;

        // Iterate frames looking for report token in the source
        for (i = 1; i < 12; i++) {
            // Set id to interrogate
            id = "iFrameMenu" + i;
            // Browser specific checks looking for object
            var tmp;
            if (window.top.frames[id].src !== undefined) {
                // Non IE logic
                tmp = window.top.frames[id];
            }
            else {
                // IE logic
                tmp = window.top.document.getElementById(id);
            }
            // Is report opened here?
            if (tmp !== undefined && tmp.src.indexOf(reportToken) !== -1) {
                // Found it
                iFrameObject = tmp;
                break;
            }
        }
        return iFrameObject;
    },

    // Common helper function to ask the portal to open Notes Center to display notes (if any)
    // for the entity (e.g. AR customer) whose information is passed in via the options.
    // External callers should call the show<entity>Notes functions (e.g. showCustomerNotes)
    // rather than this common helper function.
    showNotes: function (options) {
        var data = { 'notesOptions': options };
        window.top.postMessage(data, "*");
    },
    // Ask the portal to open Notes Center to display notes (if any) for the given AR customer.
    showCustomerNotes: function (customerNumber) {
        var options = {
            notesSearchType: sg.utls.NotesSearchType.Customers,
            entityID: customerNumber,
        };
        sg.utls.showNotes(options);
    },
    // Ask the portal to open Notes Center to display notes (if any) for the given IC item.
    showInventoryItemNotes: function (itemNumber) {
        var options = {
            notesSearchType: sg.utls.NotesSearchType.InventoryItems,
            entityID: itemNumber,
        };
        sg.utls.showNotes(options);
    },
    // Ask the portal to open Notes Center to display notes (if any) for the given AP vendor.
    showVendorNotes: function (vendorNumber) {
        var options = {
            notesSearchType: sg.utls.NotesSearchType.Vendors,
            entityID: vendorNumber,
        };
        sg.utls.showNotes(options);
    },

    // Hide the notes center.
    hideNotesCenter: function () {
        var data = { 'hideNotesCenter': true };
        window.top.postMessage(data, "*");
    },

    populateCustomReportProfileIdsMultiSelectWidget: function (profileId) {
        var data = { 'populateCustomReportProfileIdsMultiSelectWidget': profileId };
        window.top.postMessage(data, "*");
    },

    refreshCustomReportProfileIdsMultiSelectWidget: function () {
        var data = { 'refreshCustomReportProfileIdsMultiSelectWidget': true };
        window.top.postMessage(data, "*");
    },

    recursiveAjax: function (ajaxUrl, ajaxData, successHandler, abortHandler, dataType, type) {
        var ajaxError = false;
        var data = ajaxData;
        data = JSON.stringify(data);
        var pollRequest;
        (function poll() {
            pollRequest = $.ajax({
                url: ajaxUrl,
                data: data,
                type: type,
                headers: sg.utls.getHeadersForAjax(),
                dataType: dataType,
                contentType: 'application/json',
                success: successHandler,
                error: function (jqXhr, textStatus, errorThrown) {
                    ajaxError = true;
                },
                complete: function (event, xhr, settings) {
                    if (abortHandler() || ajaxError) {
                        return;
                    }
                    setTimeout(function () { poll(); }, 500);
                },
                statusCode: {
                    401: function () {
                        console.log('user not authenticated');
                        window.top.location.reload();
                    }
                }
            });
        })();
    },

    getCookie: function(name) {
        var re = new RegExp(name + "=([^;]+)");
        var value = re.exec(document.cookie);
        return (value != null) ? unescape(value[1]) : null;
    },

    ajaxInternal: function (ajaxUrl, ajaxData, successHandler, dataType, type, isAsync, errorHandler) {
        sg.utls.ajaxRunning = true;
        var baseUrl = sg.utls.getCookie("baseUrl");
        if (baseUrl) {
            ajaxUrl = baseUrl + ajaxUrl;
        }
        var data = ajaxData;
        data = JSON.stringify(data);
        $.ajaxq("SageQueue", {
            url: ajaxUrl,
            data: data,
            type: type,
            headers: sg.utls.getHeadersForAjax(),
            async: isAsync,
            dataType: dataType,
            contentType: 'application/json',
            success: successHandler,
            error: errorHandler,
            beforeSend: function () {
                $('#ajaxSpinner').fadeIn(1);
                if (sg.utls.isSameOrigin()) {
                    var iFrame = window.top.$('iframe.screenIframe:visible');
                    if (iFrame && iFrame.length > 0) {
                        sg.utls.showMessagesInViewPort();
                    }
                }
            },
            complete: function () {
                $('#ajaxSpinner').fadeOut(1);
                sg.utls.ajaxRunning = false;
                sg.utls.isProcessRunning = false;
                sg.utls.fireStackedCalls();
            },
            statusCode: {
                401: function () {
                    console.log('user not authenticated');
                    window.location.reload();
                }
            }
        });
    },

    /**
     * Invokes a specified callback method after a 5 ms delay, to give any tab-out events that fire
     * (which should happen within roughly 1 ms, after which finders may be called) time to complete their respective execution.
     * 
     * @param {function} callbackFunction The callback function to invoke.
     *
     */
    SyncExecute: function (callbackFunction) {
        setTimeout(function () {
            if (sg.utls.ajaxRunning === true) {
                sg.utls.functionsToCall.push(callbackFunction);
            } else {
                callbackFunction();
            };
        }, 5);
    },
    fireStackedCalls: function () {
        while (sg.utls.functionsToCall.length > 0) {
            toCall = sg.utls.functionsToCall.pop();
            if (toCall) {
                toCall();
            }
        }
    },
    removeStackedCalls: function () {
        while (sg.utls.functionsToCall.length > 0) {
            var functionToCall = sg.utls.functionsToCall.pop();
        }
    },
    ajaxErrorHandler: function (jqXhr, textStatus, errorThrown) {
        $('#ajaxSpinner').slideUp();
        if (jqXhr != null && jqXhr.responseText != null && jqXhr.responseText != "" && jqXhr.status != "401") {
            try {
                var json = JSON.parse(jqXhr.responseText);
                sg.utls.showMessage(json);
            }
            catch (err) {
                console.log(jqXhr.responseText);
            }
        }
    },

    ajaxGet: function (ajaxUrl, ajaxData, successHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "json", "get", true, sg.utls.ajaxErrorHandler);
    },
    recursiveAjaxPost: function (ajaxUrl, ajaxData, successHandler, abortHandler) {
        return sg.utls.recursiveAjax(ajaxUrl, ajaxData, successHandler, abortHandler, "json", "post");
    },
    ajaxPost: function (ajaxUrl, ajaxData, successHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "json", "post", true, sg.utls.ajaxErrorHandler);
    },
    ajaxCrossDomainPost: function (ajaxUrl, ajaxData, successHandler, errorHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "jsonp", "post", true, errorHandler);
    },
    ajaxPostHtml: function (ajaxUrl, ajaxData, successHandler, errorHandler) {
        var customErrorHandler;
        if (errorHandler) {
            customErrorHandler = errorHandler;
        } else {
            customErrorHandler = sg.utls.ajaxErrorHandler
        }
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "html", "post", true, customErrorHandler);
    },
    ajaxPostHtmlSync: function (ajaxUrl, ajaxData, successHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "html", "post", false, sg.utls.ajaxErrorHandler);
    },
    ajaxPostSync: function (ajaxUrl, ajaxData, successHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, successHandler, "json", "post", false, sg.utls.ajaxErrorHandler);
    },
    ajaxCachePostHtml: function (ajaxUrl, ajaxData, successHandler, key) {
        sg.utls.ajaxCacheInternal(ajaxUrl, ajaxData, successHandler, "html", "post", false, key);
    },
    ajaxCache: function (ajaxUrl, ajaxData, successHandler, key) {
        sg.utls.ajaxCacheInternal(ajaxUrl, ajaxData, successHandler, "json", "post", false, key);
    },
    ajaxCacheInternal: function (ajaxUrl, ajaxData, successHandler, dataType, type, isAsync, key) {
        var data = ajaxData;
        data = JSON.stringify(data);
        var options = {
            url: ajaxUrl,
            async: isAsync,
            data: data,
            headers: sg.utls.getHeadersForAjax(),
            type: type,
            dataType: dataType,
            contentType: 'application/json',
            success: successHandler,
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        };
        sage.dataManager.sendRequest(options, key);
    },

    getHeadersForAjax: function () {
        var tokenName = sg.utls.getAntiForgeryTokenName();
        var headers = { ContextToken: $("#ContextToken").val(), ScreenName: $("#ScreenName").val() };
        headers[tokenName] = sg.utls.getAntiForgeryToken();
        return headers
    },
    saveGridPreferences: function (key, value) {
        var data = { key: key, value: value }
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "SaveGridPreferences"), data, function (successData) {
        });
    },
    getGridPreferences: function (key, successHandler) {
        var data = { key: key }
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "GetGridPreferences"), data, successHandler);
    },
    clearValidations: function (formId) {
        $("#" + formId).each(function () {
            $(this).find(".field-validation-error").empty();
        });

        $("#message").hide();
    },
    getContext: function (data) {
        data.ContextToken = $("#ContextToken").val();
        data.ScreenName = $("#ScreenName").val();
        return data;
    },
    getAntiForgeryToken: function () {
        return $("#antiforgerytoken_holder input[type='hidden'][name='__RequestVerificationToken']").val();
    },
    getAntiForgeryTokenName: function () {
        return $("#antiforgerytoken_holder[data-antiforgerycookiename]").data("antiforgerycookiename");
    },
    getFormatedDialogHtml: function (okButtonId, cancelButtonId) {

        var idOK = (typeof okButtonId !== 'undefined' && okButtonId !== null) ? okButtonId : "kendoConfirmationAcceptButton";
        var idCancel = (typeof cancelButtonId !== 'undefined' && cancelButtonId !== null) ? cancelButtonId : "kendoConfirmationCancelButton";

        return "<div id=\"dialogConfirmation\" class=\"modal-msg\">" +
	        "<div class=\"message-control multiWarn-msg\">" +
		        "<div class=\"title\">" +
			        "<span class=\"icon multiWarn-icon\"/>" +
			        "<h3 id=\"dialogConfirmation_header\"></h3>" +
		        "</div>" +
		        "<div class=\"msg-content\">" +
			        "<p id=\"dialogConfirmation_msg1\"></p>" +
			        "<p id=\"dialogConfirmation_msg2\"></p>" +
			        "<div class=\"button-group\">" +
				        "<input class=\"btn btn-primary\" id=\"" + idOK + "\" type=\"button\" value=\"OK\" />" +
                        "<input class=\"btn btn-secondary\" id=\"" + idCancel + "\" type=\"button\" value=\"Cancel\" />" +
			        "</div>" +
		        "</div>" +
	        "</div>" +
        "</div>";
    },
    showMessageDialog: function (callbackYes, callbackNo, message, dialogType, title, dialoghtml, okButtonId, cancelButtonId) {

        var idOK = (typeof okButtonId !== 'undefined' && okButtonId !== null) ? "#" + okButtonId : "#kendoConfirmationAcceptButton";
        var idCancel = (typeof cancelButtonId !== 'undefined' && cancelButtonId !== null) ? "#" + cancelButtonId : "#kendoConfirmationCancelButton";

        if (dialoghtml === null || dialoghtml === undefined) {
            var kendoWindow = $("<div class='modelWindow' id='" + "dialogConfirmation " + "' />").kendoWindow({
                title: '',
                resizable: false,
                modal: true,
                minWidth: "30%",
                maxWidth: "60%",
                Height: "30%",
                activate: sg.utls.kndoUI.onActivate
            });
            kendoWindow.data("kendoWindow").content($("#dialog-confirmation").html()).center().open();
            kendoWindow.data("kendoWindow").bind("close", function () {
                kendoWindow.data("kendoWindow").destroy();
                if (callbackNo != null)
                    callbackNo();
            });

            kendoWindow.parent().addClass('modelBox');
            kendoWindow.find("#body-text").html(sg.utls.htmlEncode(message));

            var defaultTitle = globalResource.ConfirmationTitle;

            var yesBinderArray = ["dialog-confirm"];
            var noBinderArray = ["dialog-cancel"];
        } else {
            var kendoWindow = $(dialoghtml).kendoWindow({
                title: false,
                resizable: false,
                modal: true,
                activate: sg.utls.kndoUI.onActivate
            });

            kendoWindow.data("kendoWindow").center().open();

            kendoWindow.find("#dialogConfirmation_header").html(globalResource.SessionExpiredDialogHeader);
            kendoWindow.find("#dialogConfirmation_msg1").html(globalResource.SessionExpiredDialogMsg1);
            kendoWindow.find("#dialogConfirmation_msg2").html(globalResource.SessionExpiredDialogMsg2);

            var yesBinderArray = ["msgCtrl-close", "btn-primary"];
            var noBinderArray = ["btn-secondary"];
        }

        switch (dialogType) {
            case sg.utls.DialogBoxType.YesNo:
                $(idOK).html(globalResource.Yes);
                $(idCancel).html(globalResource.No);
                break;
            case sg.utls.DialogBoxType.OKCancel:
                $(idOK).html(globalResource.OK);
                $(idCancel).html(globalResource.Cancel);
                break;
            case sg.utls.DialogBoxType.OK:
                defaultTitle = globalResource.Info;
                $(idOK).html(globalResource.OK);
                $(idCancel).hide();
                break;
            case sg.utls.DialogBoxType.Close:
                defaultTitle = globalResource.Error;
                $(idOK).hide();
                $(idCancel).html(globalResource.Close);
                break;
            case sg.utls.DialogBoxType.DeleteCancel:
                $(idOK).html(globalResource.Delete);
                $(idCanel).html(globalResource.Cancel);
                break;
            case sg.utls.DialogBoxType.Continue:
                kendoWindow.find("#dialogConfirmation_header").html(title);
                kendoWindow.find("#dialogConfirmation_msg1").html(message);
                $(idOK).hide();
                $(idCancel).val(globalResource.Continue);
                break;
        }

        title = title || defaultTitle;
        kendoWindow.find("#title-text").html(title);

        $.each(yesBinderArray, function (index, value) {
            kendoWindow.find("." + value).click(function () {
                if ($(this).hasClass(value)) {
                    if (callbackYes != null)
                        callbackYes();
                }
                kendoWindow.data("kendoWindow").destroy();
            }).end();
        });

        $.each(noBinderArray, function (index, value) {
            kendoWindow.find("." + value).click(function () {
                if ($(this).hasClass(value)) {
                    if (callbackNo != null)
                        callbackNo();
                }
                kendoWindow.data("kendoWindow").destroy();
            }).end();
        });
    },

    showKendoConfirmationDialog: function (callbackYes, callbackNo, message, typeOfAction, isMessageEncoded, callbackCancel) {

        // true : Confirmation dialog title will be positioned in the header bar of dialog box
        // false : Confirmation dialog title will be positioned just above the body text instead (original behaviour)
        var positionDialogTitleInHeader = true;

        var title = globalResource.ConfirmationTitle;
        var dialogId = 'deleteConfirmation';

        // This kendoWindow visiblity check is added for the defect D-07638
        var wnd = $("#" + dialogId).data("kendoWindow");
        if (wnd != null && !wnd.element.is(":hidden")) {
            return;
        }

        var kendoWindow = $("<div class='modelWindow' id='" + dialogId + "' />").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 400,
            maxWidth: 760,
            Height: 240,
            // Custom function to support focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        });

        kendoWindow.data("kendoWindow").content($("#delete-confirmation").html()).center().open();
        kendoWindow.data("kendoWindow").bind("close", function () {
            kendoWindow.data("kendoWindow").destroy();
            if (callbackCancel) {
                callbackCancel();
            }
            else if (callbackNo != null && typeOfAction !== "YesNoCancel") {
                callbackNo();
            }
        });

        if (!positionDialogTitleInHeader) {
            kendoWindow.find("#title-text").html(title);
        }

        kendoWindow.find("#body-text").html(isMessageEncoded ? message : sg.utls.htmlEncode(message));
        if (typeOfAction == globalResource.DeleteTitle) {
            kendoWindow.find("#kendoConfirmationCancelButton").val(globalResource.CancelTitle);
            kendoWindow.find("#kendoConfirmationAcceptButton").val(globalResource.DeleteTitle);
        }
        if (typeOfAction !== "YesNoCancel") {
            $("#kendoConfirmationCancelledButton").hide();
        }

        kendoWindow.find(".delete-confirm,.delete-cancel,.delete-cancelled")
            .click(function () {
                if ($(this).hasClass("delete-confirm")) {
                    if (callbackYes != null) {
                        callbackYes();
                    }
                    try {
                        kendoWindow.data("kendoWindow").destroy();
                    } catch (err) {
                        //don't do anything. This means window is already destroyed or does not exists.
                        //This is required for iframes
                    }
                } else if ($(this).hasClass("delete-cancel")) {
                    if (callbackNo != null)
                        callbackNo();
                    kendoWindow.data("kendoWindow").destroy();
                }
                else if ($(this).hasClass("delete-cancelled")) {
                    if (callbackCancel) {
                        callbackCancel();
                    }
                    kendoWindow.data("kendoWindow").destroy();
                }
            }).end();

        kendoWindow.parent().addClass('modelBox');
        kendoWindow.parent().attr('id', 'deleteConfirmationParent');
        var divDeleteConfirmParent = $('#deleteConfirmationParent');

        // Removed the line below because if modal is true, the z-index value increasing 
        // automatically. We cannot guarantee 999999 is the largest value on the screen.
        //divDeleteConfirmParent.css('z-index', '999999');

        divDeleteConfirmParent.css('position', 'absolute');
        divDeleteConfirmParent.css('left', ($(window).width() - divDeleteConfirmParent.width()) / 2);

        if (positionDialogTitleInHeader) {
            divDeleteConfirmParent.find('#' + dialogId + '_wnd_title').text(title);
        }

        // Setting message position to viewport top.
        sg.utls.showMessagesInViewPort();
    },

    showCommonConfirmationDialog: function (id, callbackYes, callbackNo, message) {
        var dialogId = 'div_' + id + 'confirm_dialog';
        $('<div  class="modelWindow" id="' + dialogId + '" />').appendTo('body');

        var kendoWindow = $('<div class="modelWindow" id="' + dialogId + '" />').kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 400,
            maxWidth: 760,
            Height: 240,
            //custom function to suppot focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        });

        kendoWindow.data("kendoWindow").content($(dialogId).html()).center().open();

        kendoWindow.find("#title-text").html('Confirmation required');
        kendoWindow.find("#body-text").html(sg.utls.htmlEncode(message));

        kendoWindow.find(".delete-confirm,.delete-cancel")
            .click(function () {
                if ($(this).hasClass("delete-confirm")) {
                    if (callbackYes != null)
                        callbackYes();
                } else if ($(this).hasClass("delete-cancel")) {
                    if (callbackNo != null)
                        callbackNo();
                }
                kendoWindow.data("kendoWindow").close();
            }).end();
        kendoWindow.parent().addClass('modelBox');
    },

    /**
     * @name showConfirmationDialogYesNo
     * @desc Display a confirmation dialog box
     *       Notes:
     *         - This method does not rely on an embedded x-kendo-template script element. 
     *           It will dynamically create one at run-time.
     *         - It will display the dialog title in the traditional header of the dialog box
     *           instead of in the message body area.
     *         
     * @private
     * @param {object} callbackYes - Callback when 'Yes' selected
     * @param {object} callbackNo - Callback when 'No' selected
     * @param {string} messageIn - The message to display
     * @param {string} titleIn - Optional - The dialog title to display
     * @param {string} btnYesLabelIn - Optional - The text for the 'Yes' button
     * @param {string} btnNoLabelIn - Optional - The text for the 'No' button
     */
    showConfirmationDialogYesNo: function (callbackYes, callbackNo, messageIn, titleIn, btnYesLabelIn, btnNoLabelIn) {
        var DialogID = 'confirmationDialog';
        var DialogParentID = 'confirmationParent';
        var InpageTemplateIDRoot = 'generic-confirmation';
        var randomPostfix = sg.utls.makeRandomString(5);
        var InpageTemplateID = InpageTemplateIDRoot + randomPostfix;

        var template = "<script id=\"" + InpageTemplateID + "\" type=\"text/x-kendo-template\">" +
            "<div class=\"fild_set\">" +
            "<div class=\"fild-title generic-message\" id=\"gen-message" + randomPostfix + "\">" +
            "<div id=\"title-text" + randomPostfix + "\" />" +
            "</div>" +
            "<div class=\"fild-content\">" +
            "<div id=\"body-text" + randomPostfix + "\" />" +
            "<div class=\"modelBox_controlls\">" +
            "<input type=\"button\" class=\"btn btn-secondary generic-cancel\" id=\"kendoConfirmationCancelButton" + randomPostfix + "\" value=\"@CommonResx.No\" />" +
            "<input type=\"button\" class=\"btn btn-primary generic-confirm\" id=\"kendoConfirmationAcceptButton" + randomPostfix + "\" value=\"@CommonResx.Yes\" />" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</script>";
        $(template).appendTo('body');

        // This kendoWindow visiblity check is added for the defect D-07638
        var wnd = $('#' + DialogID).data("kendoWindow");
        if (wnd != null && !wnd.element.is(":hidden")) {
            return;
        }

        var kendoWindow = $("<div class='modelWindow' id='" + DialogID + "' />").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 400,
            maxWidth: 760,
            Height: 240,
            // custom function to support focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        });

        kendoWindow.data("kendoWindow").content($("#" + InpageTemplateID).html()).center().open();

        kendoWindow.data("kendoWindow").bind("close", function () {
            kendoWindow.data("kendoWindow").destroy();
            if (callbackNo != null) {
                callbackNo();
            }
        });

        // Set the message text
        //kendoWindow.find("#body-text").html(sg.utls.htmlEncode(messageIn));
        var msg = messageIn.replace(/\n/g, '<br/>');
        kendoWindow.find("#body-text" + randomPostfix).html(msg);

        _setButtonLabels();

        _setButtonCallbacks();

        kendoWindow.parent().addClass('modelBox');
        kendoWindow.parent().attr('id', DialogParentID);
        var $divConfirmParent = $('#' + DialogParentID);
        $divConfirmParent.css('position', 'absolute');
        $divConfirmParent.css('left', ($(window).width() - $divConfirmParent.width()) / 2);

        _setDialogTitle();

        /// Setting message position to viewport top.
        sg.utls.showMessagesInViewPort2(DialogParentID);

        function _setDialogTitle() {
            var title;
            if (titleIn && titleIn.length > 0) {
                title = titleIn;
            } else {
                title = globalResource.ConfirmationTitle;
            }

            // Set the modal dialog caption (title) text
            $divConfirmParent.find('#' + DialogID + '_wnd_title').text(title);
        }

        function _setButtonLabels() {
            var yesLabel;
            if (btnYesLabelIn && btnYesLabelIn.length > 0) {
                yesLabel = btnYesLabelIn;
            } else {
                yesLabel = globalResource.Yes;
            }

            var noLabel;
            if (btnNoLabelIn && btnNoLabelIn.length > 0) {
                noLabel = btnNoLabelIn;
            } else {
                noLabel = globalResource.No;
            }

            kendoWindow.find("#kendoConfirmationAcceptButton" + randomPostfix).val(yesLabel);
            kendoWindow.find("#kendoConfirmationCancelButton" + randomPostfix).val(noLabel);
        }

        function _setButtonCallbacks() {
            kendoWindow.find(".generic-confirm, .generic-cancel")
                .click(function () {
                    if ($(this).hasClass("generic-confirm")) {
                        if (callbackYes != null) {
                            callbackYes();
                        }
                        try {
                            kendoWindow.data("kendoWindow").destroy();
                        } catch (err) {
                            // Don't do anything. This means the window is already 
                            // destroyed or does not exist.
                            // This is required for iframes
                        }
                    } else if ($(this).hasClass("generic-cancel")) {
                        if (callbackNo != null)
                            callbackNo();
                        kendoWindow.data("kendoWindow").destroy();
                    }
                }).end();
        }
    },

    /**
     * @name showMessagesInViewPort2
     * @desc Show all messages in viewport area 
     * @private
     */
    showMessagesInViewPort2: function (parentDialogID) {
        // Call the main global version first
        sg.utls.showMessagesInViewPort();

        if (!sg.utls.isSameOrigin()) {
            return;
        }
        try {
            var activeScreenIframeId = window.top.$('iframe.screenIframe:visible').attr('id');
            var activeScreenIframeContents = window.top.$('#' + activeScreenIframeId).contents();

            var divConfirmation = activeScreenIframeContents.find('#' + parentDialogID);

            _setHorizontalAndVerticalScrollPositions(divConfirmation);
        }
        catch (err) {
            // Don't do anything. This means window is already destroyed or does not exists.
            // This is required for iframes
        }

        function _setHorizontalAndVerticalScrollPositions(div) {
            if ((div !== null && div.length > 0)) {
                sg.utls.setScrollPosition(div);
                var horizontalPos = ($(window).width() - div.width()) / 2;
                div.css('left', horizontalPos);
            }
        }
    },

    /**
     * @name makeRandomString
     * @description Returns a random string of alphabetical letters of a designated length
     * @param len
     * @returns {string} The random string of designated length
     */
    makeRandomString: function (len) {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        for (var i = 0; i < len; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }
        return text;
    },

    showKendoMessageDialog: function (callbackok, message) {
        var kendoWindow = $("<div class='modelWindow' id='" + "messageDialog " + "' />").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 400,
            maxWidth: 760,
            Height: 240,
            //custom function to suppot focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        });

        kendoWindow.data("kendoWindow").content($("#message-dialog").html()).center().open();

        kendoWindow.data("kendoWindow").bind("close", function () {
            kendoWindow.data("kendoWindow").destroy();
            if (callbackok) {
                callbackok();
            }
        });

        kendoWindow.find("#title-text").html('Message');
        kendoWindow.find("#body-text").html(sg.utls.htmlEncode(message));

        kendoWindow.find(".delete-confirm")
            .click(function () {
                kendoWindow.data("kendoWindow").close();
            }).end();

        kendoWindow.parent().addClass('modelBox');
        kendoWindow.parent().attr('id', 'deleteConfirmationParent');
        var divDeleteConfirmParent = $('#deleteConfirmationParent');
        divDeleteConfirmParent.css('z-index', '999999');
        divDeleteConfirmParent.css('position', 'absolute');
        divDeleteConfirmParent.css('left', ($(window).width() - divDeleteConfirmParent.width()) / 2);

        /// Setting message position to viewport top.
        sg.utls.showMessagesInViewPort();
    },

    /**
     * Creates a unique GUID; intended for use when opening multiple kendo iframe windows, where the url should be unique.
     *
     * @returns {String} A string representing a GUID. 
     */
    guid: function () {
        function _p8(s) {
            var p = (Math.random().toString(16) + "000000000").substr(2, 8);
            return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
        }
        return _p8() + _p8(true) + _p8(true) + _p8();
    },

    /**
     * Initializes a Kendo popup window. 
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     *
     */
    initializeKendoWindowPopup: function(id, title, onClose, maxConfig) {
        var winH = $(window).height();
        var winW = $(window).width();
        var actions = maxConfig ? maxConfig.actions : ["Close"];
        var width = (maxConfig && maxConfig.width)? maxConfig.width: 980;
        var kendoWindow = $(id).kendoWindow({
            modal: true,
            title: title,
            resizable: false,
            draggable: false,
            scrollable: true,
            visible: false,
            //maxWidth: maxWidth,
            minWidth: 900,
            minHeight: 300,
            width: width,
            //maxHeight: 600,
            // Custom function to support focus within Kendo Window.
            activate: sg.utls.kndoUI.onActivate,
            close: function(data) {
                // Hide the Grid Preferences columns list for Popups. 
                // This is needed because the div element (columns list) is not hiding while closing popups with editable grids.
                if (GridPreferencesHelper) {
                    GridPreferencesHelper.hide();
                }
                if (onClose) {
                    onClose(data);
                }

                //This is to restore the kendoWindow properties on close if we can't destroy the kendow Window (Only applicable in Maximized Window)
                if (!data.isDefaultPrevented() && maxConfig && maxConfig.actions && this.options.isMaximized) {
                    this.restore();
                }
            },
            actions: actions,
            // Open the Kendo Window in the center of the Viewport.
            open: function () {
                sg.utls.setKendoWindowPosition(this);
            },
        }).data("kendoWindow");
    },

    /**
     * Initializes a Kendo popup window with minimize/maximize option.
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     */
    initializeKendoWindowPopupWithMaximize: function(id, title, onClose, width) {
        var config = {
            actions: ["Maximize", "Close"],
            width: width
        };
        this.initializeKendoWindowPopup(id, title, onClose, config);
    },

    /**
     * Initializes a Kendo popup window. 
     * 
     * @deprecated Name is misspelled!
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     *
     */
    intializeKendoWindowPopup: function (id, title, onClose) {
        this.initializeKendoWindowPopup(id, title, onClose);
    },

    openKendoWindowPopup: function (id, data, defaultWidth) {
        var kendoWindow = $(id).data("kendoWindow");

        if (data != null) {
            $(id).html(data);
            $.validator.unobtrusive.parse("form");
            sg.utls.initFormValidation();
        }
        kendoWindow.open();

        //remove horizondal scrollbar of the popup windows
        //find the max length, if maxlength ==940px then increasing the popup window body width
        //var maxWidth = 0;
        //$('.k-window-content div[class*="-group"]').each(function (i) {
        //    if (this.offsetWidth > maxWidth)
        //        maxWidth = this.offsetWidth;
        //});

        //if (maxWidth >= 920 || maxWidth <= 940) {
        //    $('.k-window-content').css("width", "960px");
        //}
        //end:remove horizondal scrollbar of the popup windows
        hasVerticalscroll = $(".k-window-content:visible").hasScroll('y');
        if (hasVerticalscroll && defaultWidth && defaultWidth != null) {
            $('.k-window-content:visible').css("width", defaultWidth + "px");
        }
        else if (hasVerticalscroll && defaultWidth == null) {
            $('.k-window-content:visible').css("width", "960px");  //remove horizondal scrollbar of the popup windows
            //console.log("scroll true");
        } else {
            $('.k-window-content:visible').css("width", "auto");
            // console.log("scroll false");
        }

        var menuLink = $(".dropDown-Menu > li");
        menuLink.find("> a").append('<span class="arrow-grey"></span>');
        menuLink.hover(function () {
            $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
            $(this).children(".sub-menu").show();
        }, function () {
            $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
            $(this).children(".sub-menu").hide();
        });

    },
    closeKendoWindowPopup: function (id, data) {
        var kendoWindow = $(id).data("kendoWindow");
        kendoWindow.close();
    },
    showConfirmationDialog: function (callbackYes, callbackNo, message) {
        if (message != null) $("#confirmDialog").text(message);
        $("#confirmDialogMessage").show();
        $("#confirmDialog").dialog({
            resizable: false,
            draggable: false,
            title: globalResource.ConfirmationTitle,
            modal: true,
            dialogClass: "no-close warningMessage",
            buttons: {
                Allow_Yes: function () {
                    if (callbackYes != null)
                        callbackYes();
                    $("#confirmDialogMessage").hide();
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                },
                Allow_No: function () {
                    if (callbackNo != null)
                        callbackNo();
                    $("#confirmDialogMessage").hide();
                    $(this).dialog("close");
                    $(this).dialog("destroy");
                }
            }
        });
    },
    showMessage: function (result, handler, isModal, isModalTransparent) {

        if (result.UserMessage != null) {
            var messageDiv = $("#message");
            var css = "message-control";
            var messageHTML = "";
            var isSuccessMessage = false;

            $("#success").stop(true, true).hide();
            $("#success").empty();
            $("#message").empty();

            //Warning
            if (result.UserMessage.Warnings != null && result.UserMessage.Warnings.length > 0) {
                messageDiv = $("#message");
                $("#message").show();
                var warnCSS = "message-control multiWarn-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Warnings, null);
                messageHTML = "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + warnHTML + " </div></div>";
                messageDiv.html(messageHTML);
            }

            //Error
            if (result.UserMessage.Errors != null && result.UserMessage.Errors.length > 0) {
                $("#message").show();
                var errorCSS = "message-control multiError-msg";
                var defaultErrorMsg = null;
                //To Stop Synchronous Function Calls stored in stack in case of any Error.
                sg.utls.removeStackedCalls();
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                var errors = result.UserMessage.Errors;
                var errorHTML = sg.utls.generateList(result.UserMessage.Errors, defaultErrorMsg);
                if (errors.length > 5) {
                    var tmp = '<div class="datagrid-group"><div class="k-grid-content"><div class="k-virtual-scrollable-wrap"><table class="k-grid gh320 k-widget">';
                    errorHTML = "";
                    for (i = 0; i < errors.length; i++) {
                        var msg = errors[i].Message;
                        if (i % 2 === 0) {
                            errorHTML = errorHTML + "<tr class='k-alt'><td>" + (i + 1) + "</td><td style='width:100%'>" + sg.utls.htmlEncode(msg) + "</td></tr>";
                        }
                        else {
                            errorHTML = errorHTML + "<tr><td>" + (i + 1) + "</td><td style='white-space: normal'>" + sg.utls.htmlEncode(msg) + "</td></tr>";
                        }
                    }
                    var end = '</table></div><div></div>';
                    errorHTML = tmp + errorHTML + end;

                    messageHTML = "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content with-grid'> " + errorHTML + " </div></div>";
                }
                else {
                    messageHTML = "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + errorHTML + " </div></div>";
                }
                messageDiv.html(messageHTML);
            }

            //Success
            if (result.UserMessage.IsSuccess) {
                clearTimeout(fnTimeout);
                $("#success").show();
                messageDiv = $("#success");
                if (result.UserMessage.Message != undefined) {
                    messageHTML = sg.utls.isSuccessMessage(sg.utls.htmlEncode(result.UserMessage.Message));
                    isSuccessMessage = true;
                } else {
                    messageHTML = "";
                }
                messageDiv.html(messageHTML);
            }

            //Info
            if (result.UserMessage.Info != null && result.UserMessage.Info.length > 0) {
                $("#message").show();
                var defaultErrorMsg = null;
                isSuccessMessage = result.UserMessage.IsSuccess;
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                css = css + " message-control multiInfo-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Info, defaultErrorMsg);
                if (isSuccessMessage) {
                    $("#success").show();
                    messageDiv = $("#success");
                    messageHTML = sg.utls.isSuccessMessage(warnHTML);
                } else {
                    $("#message").show();
                    messageDiv = $("#message");
                    //messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + warnHTML + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                    messageHTML = "<div class='" + css + "'>     <div class='title'><span class='icon multiInfo-icon'></span><h3>" + globalResource.Info + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'>" + warnHTML + "</div></div>";
                    //messageHTML = "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + warnHTML + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + errorHTML + " </div></div>";
                }
                messageDiv.html(messageHTML);
            }

            /// Setting message position to viewport top.
            sg.utls.showMessagesInViewPort();

            var showTime = (globalResource.ShowMessageTime) ? globalResource.ShowMessageTime : 5000;
            if (isSuccessMessage) {
                fnTimeout = setTimeout(function () {
                    messageDiv.fadeOut(1000);
                    messageDiv.empty();
                }, showTime);
            }

            if (isModal === true) {
                //Inject an overlay that has higher z-index than kendo widgets ( handled in css). 
                messageDiv.parent().append("<div id='injectedOverlay' class='k-overlay'></div>");
            }
            if (isModalTransparent === true) {
                messageDiv.parent().append("<div id='injectedOverlayTransparent' class='k-overlay k-overlay-transparent' ></div>");
            }

            if (handler !== undefined && handler !== null) {
                var closeHandler = function () {
                    handler();
                    $(document).off("click", ".msgCtrl-close", closeHandler);
                };
                $(document).on("click", ".msgCtrl-close", closeHandler);
            }
        }
    },
    // showMessageInEnumerableResponse : To show any messages within EnumerableResponse<T>. This method does not contain any UserMessage
    showMessageInEnumerableResponse: function (result, divId, showCloseIcon) {
        if (result != null) {
            if (!divId) {
                divId = "#message";
            }

            if (showCloseIcon != false) {
                showCloseIcon = true;
            }

            var messageDiv = $(divId);
            var css = "message-control";
            var messageHTML = "";
            var isSuccessMessage = false;

            $("#success").stop(true, true).hide();

            $("#success").empty();
            $(divId).empty();

            //Warning
            if (result.Warnings != null && result.Warnings.length > 0) {
                messageDiv = $(divId);
                $(divId).show();
                var warnCSS = "message-control multiWarn-msg";
                var warnHTML = sg.utls.generateList(result.Warnings, null);
                if (showCloseIcon) {
                    messageHTML = "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + warnHTML + " </div></div>";
                } else {
                    messageHTML = "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3></div><div class='msg-content'> " + warnHTML + " </div></div>";
                }
                messageDiv.html(messageHTML);
            }

            //Error
            if (result.Errors != null && result.Errors.length > 0) {
                $(divId).show();
                var errorCSS = "message-control multiError-msg";
                var defaultErrorMsg = null;
                //To Stop Synchronous Function Calls stored in stack in case of any Error.
                sg.utls.removeStackedCalls();
                if (!result.IsSuccess) {
                    defaultErrorMsg = result.Message;
                }
                var errorHTML = sg.utls.generateList(result.Errors, defaultErrorMsg);
                if (showCloseIcon) {
                    messageHTML = "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + errorHTML + " </div></div>";
                } else {
                    messageHTML = "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3></div><div class='msg-content'> " + errorHTML + " </div></div>";
                }
                messageDiv.html(messageHTML);
            }

            //Info
            if (result.Info != null && result.Info.length > 0) {
                $(divId).show();
                var defaultErrorMsg = null;
                isSuccessMessage = result.IsSuccess;
                if (!result.IsSuccess) {
                    defaultErrorMsg = result.Message;
                }
                css = css + " message-control multiInfo-msg";
                var warnHTML = sg.utls.generateList(result.Info, defaultErrorMsg);
                if (isSuccessMessage && !divId) {
                    $("#success").show();
                    messageDiv = $("#success");
                    messageHTML = sg.utls.isSuccessMessage(warnHTML);
                } else {
                    $(divId).show();
                    messageDiv = $(divId);
                    if (showCloseIcon) {
                        messageHTML = "<div class='" + css + "'>     <div class='title'><span class='icon multiInfo-icon'></span><h3>" + globalResource.Info + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'>" + warnHTML + "</div></div>";
                    } else {
                        messageHTML = "<div class='" + css + "'>     <div class='title'><span class='icon multiInfo-icon'></span><h3>" + globalResource.Info + "</h3></div><div class='msg-content'>" + warnHTML + "</div></div>";
                    }
                }
                messageDiv.html(messageHTML);
            }


            //Success
            if (result.IsSuccess) {
                clearTimeout(fnTimeout);
                $("#success").show();
                messageDiv = $("#success");
                if (result.Message != undefined) {
                    messageHTML = sg.utls.isSuccessMessage(sg.utls.htmlEncode(result.Message));
                    isSuccessMessage = true;
                } else {
                    messageHTML = "";
                }
                messageDiv.html(messageHTML);
            }

            /// Setting message position to viewport top.
            sg.utls.showMessagesInViewPort();

            var showTime = (globalResource.ShowMessageTime) ? globalResource.ShowMessageTime : 5000;
            if (isSuccessMessage) {
                fnTimeout = setTimeout(function () {
                    messageDiv.fadeOut(1000);
                    messageDiv.empty();
                }, showTime);

            }
        }
    },
    showMessagePopup: function (result, divId) {
        clearTimeout(fnTimeout);
        if (result.UserMessage != null) {
            var messageDiv = $(divId);
            var css = "message-control";
            var messageHTML = "";
            var isSuccessMessage = false;

            $("#success").stop(true, true).hide();

            $("#success").empty();
            $(divId).empty();

            //Success
            if (result.UserMessage.IsSuccess) {
                $("#success").show();
                if (divId == null || divId == "") {
                    messageDiv = $("#success");
                }

                // Green Color -- Success - no Error and no Warnings
                if (result.UserMessage.Message != undefined) {
                    css = css + " success-msg";
                    messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + sg.utls.htmlEncode(result.UserMessage.Message) + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                    isSuccessMessage = true;
                }
            }

            //Warning
            if (result.UserMessage.Warnings != null && result.UserMessage.Warnings.length > 0) {
                $(divId).show();
                var warnCSS = "message-control multiWarn-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Warnings, null);
                messageHTML = messageHTML + "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + warnHTML + " </div></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Warning from data Object
            if (result.Data.Warnings != null && result.Data.Warnings.length > 0) {
                $(divId).show();
                var warnHTML = sg.utls.generateList(result.Data.Warnings, null);
                css = css + " success-msg";
                messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + warnHTML + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Error
            if (result.UserMessage.Errors != null && result.UserMessage.Errors.length > 0) {
                $(divId).show();
                var errorCSS = "message-control multiError-msg";
                var defaultErrorMsg = null;
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                var errorHTML = sg.utls.generateList(result.UserMessage.Errors, defaultErrorMsg);
                messageHTML = messageHTML + "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + errorHTML + " </div></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Info
            if (result.UserMessage.Info != null && result.UserMessage.Info.length > 0) {
                $(divId).show();
                var defaultErrorMsg = null;
                isSuccessMessage = result.UserMessage.IsSuccess;
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                css = css + " success-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Info, defaultErrorMsg);
                if (isSuccessMessage) {
                    messageHTML = sg.utls.isSuccessMessage(warnHTML);
                } else {
                    messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + warnHTML + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                }
                messageDiv.html(messageHTML);
            }

            messageDiv.html(messageHTML);
            var showTime = (globalResource.ShowMessageTime) ? globalResource.ShowMessageTime : 5000;
            if (isSuccessMessage) {
                fnTimeout = setTimeout(function () {
                    messageDiv.fadeOut(1000);
                    messageDiv.empty();
                }, showTime);

            }
        }
    },
    showMessagePopupWithoutClose: function (result, divId) {
        clearTimeout(fnTimeout);
        if (result.UserMessage != null) {
            var messageDiv = $(divId);
            var css = "message-control";
            var messageHTML = "";
            var isSuccessMessage = false;

            $("#success").stop(true, true).hide();

            $("#success").empty();
            $(divId).empty();

            //Success
            if (result.UserMessage.IsSuccess) {
                $("#success").show();
                $(divId).show();
                if (divId == null || divId == "") {
                    messageDiv = $("#success");
                }

                // Green Color -- Success - no Error and no Warnings
                if (result.UserMessage.Message != undefined) {
                    css = css + " success-msg";
                    messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + sg.utls.htmlEncode(result.UserMessage.Message) + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                    isSuccessMessage = true;
                }
            }

            //Warning
            if (result.UserMessage.Warnings != null && result.UserMessage.Warnings.length > 0) {
                $(divId).show();
                var warnCSS = "message-control multiWarn-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Warnings, null);
                messageHTML = messageHTML + "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3></div><div class='msg-content'> " + warnHTML + " </div></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Warning from data Object
            if (result.Data.Warnings != null && result.Data.Warnings.length > 0) {
                $(divId).show();
                var warnCSS = "message-control multiWarn-msg";
                var warnHTML = sg.utls.generateList(result.Data.Warnings, null);
                messageHTML = messageHTML + "<div class='" + warnCSS + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3></div><div class='msg-content'> " + warnHTML + " </div></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Error
            if (result.UserMessage.Errors != null && result.UserMessage.Errors.length > 0) {
                $(divId).show();
                var errorCSS = "message-control multiError-msg";
                var defaultErrorMsg = null;
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                var errorHTML = sg.utls.generateList(result.UserMessage.Errors, defaultErrorMsg);
                messageHTML = messageHTML + "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3></div><div class='msg-content'> " + errorHTML + " </div></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            //Info
            if (result.UserMessage.Info != null && result.UserMessage.Info.length > 0) {
                $(divId).show();
                var defaultErrorMsg = null;
                if (!result.UserMessage.IsSuccess) {
                    defaultErrorMsg = result.UserMessage.Message;
                }
                css = css + " success-msg";
                var warnHTML = sg.utls.generateList(result.UserMessage.Info, defaultErrorMsg);
                messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + warnHTML + "</h3></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
                if (isSuccessMessage) {
                    isSuccessMessage = false;
                }
            }

            messageDiv.html(messageHTML);
            var showTime = (globalResource.ShowMessageTime) ? globalResource.ShowMessageTime : 5000;
            if (isSuccessMessage) {
                fnTimeout = setTimeout(function () {
                    messageDiv.fadeOut(1000);
                    messageDiv.empty();
                }, showTime);

            }
        }
    },
    showMessagePopupInfo: function (messageType, message, div) {
        sg.utls.showMessageInfoInCustomDiv(messageType, message, div);
    },
    isSuccessMessage: function (message) {
        var css = "message-control";
        css = css + " success-msg";
        var messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + message + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
        return messageHTML;
    },
    showValidationMessage: function (message) {
        var messageHTML = "";
        if (message != null) {
            $("#message").show();
            var errorCSS = "message-control multiError-msg";
            var defaultErrorMsg = message;
            var errorHTML = sg.utls.generateMessage(defaultErrorMsg);
            messageHTML = messageHTML + "<div class='" + errorCSS + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + errorHTML + " </div></div>";
            $("#message").html(messageHTML);
        }
    },
    //Converts an array to html list
    //isJSArray is default to false to handle objects in c# viewmodel format; isJSArray is set to true when handling a javascript array
    generateList: function (object, defaultErrorMsg, isJSArray) {
        var objectHtml = "";
        if (object != null) {
            if (defaultErrorMsg != null && defaultErrorMsg != "") {
                if (object.length >= 1) {
                    objectHtml = "<ul>";
                }
            }
            else {
                if (object.length > 1) {
                    objectHtml = "<ul>";
                }
            }
        }

        if (defaultErrorMsg != null && defaultErrorMsg != "") {
            if (object != null && object.length >= 1) {
                objectHtml = objectHtml + "<li>" + sg.utls.htmlEncode(defaultErrorMsg) + "</li>";
            }
        }

        if (object != null) {
            for (i = 0; i < object.length; i++) {
                var msg = (isJSArray) ? object[i] : object[i].Message;

                if (defaultErrorMsg != null && defaultErrorMsg != "") {
                    if (object.length >= 1) {
                        objectHtml = objectHtml + "<li>" + sg.utls.htmlEncode(msg) + "</li>";
                    }
                }
                else {
                    if (object.length > 1) {
                        objectHtml = objectHtml + "<li>" + sg.utls.htmlEncode(msg) + "</li>";
                    } else {
                        objectHtml = objectHtml + sg.utls.htmlEncode(msg);
                    }
                }
            }
            if (defaultErrorMsg != null && defaultErrorMsg != "") {
                if (object.length >= 1) {
                    objectHtml = objectHtml + "</ul>";
                }
            } else {
                if (object.length > 1) {
                    objectHtml = objectHtml + "</ul>";
                }
            }
        }
        return objectHtml;
    },
    generateMessage: function (defaultErrorMsg) {
        var objectHtml = "<ul>";
        if (defaultErrorMsg != null) {
            objectHtml = objectHtml + "<li>" + sg.utls.htmlEncode(defaultErrorMsg) + "</li>";
            objectHtml = objectHtml + "</ul>";
            return objectHtml;
        }
    },
    showMessageInfo: function (messageType, message) {
        sg.utls.showMessageInfoInCustomDiv(messageType, message, "message");
    },
    showProcessMessageInfo: function (messageType, message, divId) {
        sg.utls.showMessageInfoInCustomDivWithoutCloseIcon(messageType, message, divId);
    },
    showMessageInfoInCustomDivWithoutCloseIcon: function (messageType, message, divId) {
        var messageSeverity = globalResource.ShowMessageBoxTitle;
        if (messageType == 1) {
            messageSeverity = globalResource.Info;
        } else if (messageType == 2) {
            messageSeverity = globalResource.Warning;
        }
        var messageHTML = "";
        var messageDivId = "#" + divId;
        var messageDiv = $(messageDivId);
        var css = "message-control";
        if (messageType == 0 || messageType == 3) {
            css = css + " multiError-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + messageSeverity + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else if (messageType == 1) {
            css = css + " multiInfo-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiInfo-icon'></span><h3>" + messageSeverity + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else if (messageType == 2) {
            css = css + " multiWarn-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + messageSeverity + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else {
            css = css + " success-msg";
        }
        messageDiv.show();
        messageDiv.html(messageHTML);
    },
    showMessageInfoInCustomDiv: function (messageType, message, divId) {
        var messageHTML = "";
        var messageDivId = "#" + divId;
        var messageDiv = $(messageDivId);
        var css = "message-control";

        //Use generateList() to handle multiple errors scenario
        var encodedMessage = Array.isArray(message) ? sg.utls.generateList(message, null, true) : message;

        if (messageType == sg.utls.msgType.ERROR) {
            //To Stop Synchronous Function Calls stored in stack in case of any Error.
            sg.utls.removeStackedCalls();
            css = css + " multiError-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3><span class='icon msgCtrl-close' id='msgCtrlClose'>Close</span></div><div class='msg-content'> " + encodedMessage + " </div></div>";
        } else if (messageType == sg.utls.msgType.INFO) {
            css = css + " multiInfo-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiInfo-icon'></span><h3>" + globalResource.Info + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + encodedMessage + " </div></div>";
        } else if (messageType == sg.utls.msgType.SUCCESS) {
            css = css + " success-msg";
            messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + encodedMessage + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
        } else if (messageType == sg.utls.msgType.WARNING) {
            css = css + " multiWarn-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3><span class='icon msgCtrl-close'>Close</span></div><div class='msg-content'> " + encodedMessage + " </div></div>";
        } else {
            css = css + " success-msg";
        }

        messageDiv.html(messageHTML);

        /// Setting message position to viewport top.
        sg.utls.showMessagesInViewPort();

        messageDiv.show();
    },
    showMessageInfoInCustomDivWithoutClose: function (messageType, message, divId) {
        var messageHTML = "";
        var messageDivId = "#" + divId;
        var messageDiv = $(messageDivId);
        var css = "message-control";
        if (messageType == sg.utls.msgType.ERROR) {
            //To Stop Synchronous Function Calls stored in stack in case of any Error.
            sg.utls.removeStackedCalls();
            css = css + " multiError-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiError-icon'></span><h3>" + globalResource.ShowMessageBoxTitle + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else if (messageType == sg.utls.msgType.INFO) {
            css = css + " multiInfo-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiInfo-icon'></span><h3>" + globalResource.Info + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else if (messageType == sg.utls.msgType.SUCCESS) {
            css = css + " success-msg";
            messageHTML = "<div class='" + css + "'><div class='top'></div><div class='title'><span class='icon success-icon'></span><h3>" + sg.utls.htmlEncode(message) + "</h3></div><div class='msg-content'> </div></div><div class='msg-overlay'></div>";
        } else if (messageType == sg.utls.msgType.WARNING) {
            css = css + " multiWarn-msg";
            messageHTML = messageHTML + "<div class='" + css + "'><div class='title'><span class='icon multiWarn-icon'></span><h3>" + globalResource.Warning + "</h3></div><div class='msg-content'> " + sg.utls.htmlEncode(message) + " </div></div>";
        } else {
            css = css + " success-msg";
        }
        messageDiv.show();
        messageDiv.html(messageHTML);
    },
    showCustomMessagePopupInfoWithoutClose: function (messageType, message, div) {
        sg.utls.showMessageInfoInCustomDivWithoutClose(messageType, message, div);
    },
    setFinder: function (id, searchFinder, filter, onSelectCallBack, onCancelCallBack, title, field) {
        $("#" + id).Finder({
            searchFinder: searchFinder,
            pageNumber: 0,
            pageSize: 10,
            filter: filter,
            field: field,
            sortDir: false,
            select: onSelectCallBack,
            cancel: onCancelCallBack,
            title: title,
            id: id
        });
    },
    getTextBoxVal: function (id) {
        return $("#" + id).val().trim();
    },
    toUpperCase: function (val) {
        return val != null ? val.toUpperCase().trim() : "";
    },
    toLowerCase: function (val) {
        return val != null ? val.toLowerCase().trim() : "";
    },
    toTrim: function (val) {
        return val != null ? val.trim() : "";
    },
    toInt: function (val, radix) {
        if (radix) {
            return val != null ? parseInt(val, radix) : 0;
        } else {
            return val != null ? parseInt(val) : 0;
        }
    },
    toFloat: function (val) {
        return val != null ? parseFloat(val) : 0;
    },
    toFixedDown: function (number, digits) {
        var re = new RegExp("(-?\\d+\\.\\d{" + digits + "})");
        var m = number.toString().match(re);
        return m ? parseFloat(m[1]) : parseFloat(number.toFixed(digits));
    },
    isAnObject: function (val) {
        return $.isPlainObject(val);
    },
    formSubmit: function (id, path, method, params) {
        $("#" + id).remove(); // remove the new form if already exists

        method = method || "post"; // Set method to post by default if not specified.

        var form = document.createElement("form");
        form.setAttribute("method", method);
        form.setAttribute("action", path);
        form.setAttribute("id", id);

        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                var hiddenField = document.createElement("input");
                hiddenField.setAttribute("type", "hidden");
                hiddenField.setAttribute("name", key);
                hiddenField.setAttribute("value", params[key]);
                form.appendChild(hiddenField);
            }
        }

        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        // for form submission, need to use the original token name
        hiddenField.setAttribute("name", "__RequestVerificationToken");
        hiddenField.setAttribute("value", sg.utls.getAntiForgeryToken());
        form.appendChild(hiddenField);

        document.body.appendChild(form);
        form.submit();
    },
    focus: function (id) {
        $("#" + id).focus();
    },

    maskPhoneNo: function (selector) {
        $(selector).mask("(AAA) AAA-AAAAAAAAAAAAAAAAAAAAAAAA", {
            'translation': {
                A: { pattern: /./ }
            }
        });
    },
    maskPeriodStart: function (selector, separator) {
        var expression = "AA" + separator + "AA";
        $(selector).mask(expression, {
            'translation': {
                A: { pattern: /[0-9]/ }
            }

        });
    },
    maskTimeformat: function (selector) {
        $(selector).mask("00:00:00");
    },
    maskSourceCode: function (clas) {
        if (clas.length > 0) {
            $("." + clas).mask('AD-BB', {
                'translation': {
                    A: { pattern: /[A-Za-z]/ },
                    D: { pattern: /[A-Za-z ]/ },
                    B: { pattern: /[A-Za-z0-9]/ }
                }
            });
        }
    },
    maskSourceLedger: function (clas) {
        if (clas.length > 0) {
            $(".sg-mask-sourceledger").mask('AA', {
                'translation': { A: { pattern: /[A-Za-z]/ } }
            });
        }
    },
    maskRateType: function (clas) {
        if (clas.length > 0) {
            $(".sg-mask-ratetype").mask('AA', {
                'translation': { A: { pattern: /[A-Za-z0-9]/ } }
            });
        }
    },
    maskBankCode: function (clas) {
        if (clas.length > 0) {
            $(".sg-mask-bankcode").mask('AAAAAAAA', {
                'translation': { A: { pattern: /[A-Za-z0-9]/ } }
            });
        }
    },
    maskApplication: function (clas) {
        if (clas.length > 0) {
            $(".sg-mask-Application").mask('AA', {
                'translation': { A: { pattern: /[A-Za-z0-9]/ } }
            });
        }
    },
    maskBatchNumber: function (clas) {
        if (clas.length > 0) {
            $(".sg-mask-batchnumber").mask('AAAAAA', {
                'translation': { A: { pattern: /[0-9]/ } }
            });
        }
    },
    maskTwoCharInput: function (input) {
        if (input.length > 0) {
            $(input).mask('AB',
            {
                'translation': {
                    A: { pattern: /[A-Za-z]/ },
                    B: { pattern: /[A-Za-z ]/ }
                }
            });
        }
    },

    trim: function (str, chars) {
        return ltrim(rtrim(str, chars), chars);
    },
    ltrim: function (str, chars) {
        if (str != null) {
            chars = chars || "\\s";
            return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
        }
        return "";
    },
    rtrim: function (str, chars) {
        if (str != null) {
            chars = chars || "\\s";
            return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
        }
        return "";
    },
    strPad: function (i, l, s) {
        var o = i.toString();
        if (!s) {
            s = '0';
        }
        while (o.length < l) {
            o = s + o;
        }
        return o;
    },
    maxLength: function (pattern, modifiers, val) {
        var regex = new RegExp(pattern, modifiers);
        if (!regex.test(val)) {
            return true;
        } else {
            return false;
        }
    },
    removeDelimeter: function (str, chars) {
        var regex = new RegExp("^" + chars + "+$|(" + chars + ")+", "g");
        var result = str != null && str.length > 0 ? str.replace(regex, "") : "";
        return result;
    },
    replaceHtmlChar: function (str) {
        return str.replace("&#39;", "'");
    },
    formatMessage: function (name, params) {
        var data = "";
        if (name != null && name.length > 0) {
            if (params.length > 0)
                data = $.each(params, function (key, value) {
                    $.validator.format(name, value);
                });
            return data;
        } else
            return data;
    },
    addPlaceHolder: function (selector, value) {
        $(selector).prop("placeholder", value);
    },
    unmask: function (selector) {
        var maskedInput = $(selector).data('mask');
        if (maskedInput) {
            $(selector).unmask();

            //There is a defect in "jquery.mask.min.js" plugin which do not deregister the mask event.
            //Custom code to manually deregister or making it 'off'
            $(document).off('DOMNodeInserted.mask', selector);
        }
    },
    addMaxLength: function (selector, value) {
        $(selector).prop("maxlength", value);
    },
    setCharAt: function (str, index, chr) {
        if (index > str.length - 1) return str;
        return str.substr(0, index) + chr + str.substr(index + 1);
    },
    padLeadingZero: function (str, max) {
        return str.length < max ? sg.utls.padLeadingZero("0" + str, max) : str;
    },
    checkIfValidTimeFormat: function (value) {
        var hour = "00";
        var minute = "00";
        var seconds = "00";
        if (value) {
            var timeProp = value.split(':');
            switch (timeProp.length) {
                case 1:
                    hour = timeProp[0] % 24;
                    break;
                case 2:
                    hour = timeProp[0] % 24;
                    minute = timeProp[1] % 60;
                    break;
                case 3:
                    hour = timeProp[0] % 24;
                    minute = timeProp[1] % 60;
                    seconds = timeProp[2] % 60;
                    break;
            }
        }
        var formattedTimeValue = sg.utls.padLeadingZero(hour.toString(), 2) + ":" + sg.utls.padLeadingZero(minute.toString(), 2) + ":" + sg.utls.padLeadingZero(seconds.toString(), 2);
        return formattedTimeValue;
    },
    getEnumItemByValue: function (enumItems, value) {
        var result = null;
        $.each(enumItems, function (index, item) {
            if (item.Value() == value) {
                result = item;
                return;
            }
        });
        return result;
    },
    getEnumItemByText: function (enumItems, text) {
        var result = null;
        $.each(enumItems, function (index, item) {
            if (item.Text() == text) {
                result = item;
                return;
            }
        });
        return result;
    },
    getDecimalThousandFormat: function (str) {
        var parts = str.split(".");
        return parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",") + (parts[1] ? "." + parts[1] : "");
    },
    replaceComma: function (str) {
        if (str != null && str.length > 0) {
            var patt = new RegExp(sg.utls.regExp.COMMA, "gi");
            return str.replace(patt, "");
        }
        return "";
    },
    generatekey: function () {
        var date = new Date();
        return date.getTime();
    },
    padChar: function (str, max) {
        var char = "";
        for (var i = 0; char.length < max; i++) {
            char = char + str;
        }
        return char;//.length < max ? sg.utls.padChar(str + str, max) : str;
    },
    getMinValue: function (defaultChar, precisionLength, maxDigitAllowed, isDecimal) {
        var intPart = sg.utls.padChar(defaultChar, maxDigitAllowed - precisionLength);
        var decimalPart = sg.utls.padChar(defaultChar, precisionLength);

        var value = "-" + intPart + (isDecimal ? "." + decimalPart : "");
        if (isDecimal)
            return parseFloat(value)
        else
            return parseInt(value, 10);
    },
    getMaxVale: function (defaultChar, precisionLength, maxDigitAllowed, isDecimal) {
        return sg.utls.getMaxValue(defaultChar, precisionLength, maxDigitAllowed, isDecimal);
    },
    getMaxValue: function (defaultChar, precisionLength, maxDigitAllowed, isDecimal) {
        var intPart = sg.utls.padChar(defaultChar, maxDigitAllowed - precisionLength);
        var decimalPart = sg.utls.padChar(defaultChar, precisionLength);

        var value = intPart + (isDecimal ? "." + decimalPart : "");
        if (isDecimal)
            return parseFloat(value)
        else
            return parseInt(value, 10);
    },
    formatPhoneNumber: function (value, isApplyFormat) {
        if (isApplyFormat) {
            return "(" + value.substr(0, 3) + ") " + value.substr(3, 3) + "-" + value.substr(6);
        } else
            return value;
    },

    //This function is added to apply phone and fax in a grid. 
    //This is used as a template grid column.
    formatPhoneNumberForGrid: function (value, isApplyFormat) {
        if (value) {
            if (isApplyFormat) {
                return "(" + value.substr(0, 3) + ") " + value.substr(3, 3) + "-" + value.substr(6);
            } else
                return value;
        }
        return "(         )    -";
    },


    //Made a format function that takes either a collection or an array as arguments, below are example usuage.
    //format("i can speak {language} since i was {age}",{language:'javascript',age:10});
    //format("i can speak {0} since i was {1}",'javascript',10});
    formatString: function (str, col) {
        col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

        return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
            if (m == "{{") { return "{"; }
            if (m == "}}") { return "}"; }
            return col[n];
        });
    },

    //This method enables or disables kendoNumericTextbox based on id and boolean value
    enableNumericTextbox: function (id, enablement) {
        if ($("#" + id).data("kendoNumericTextBox") != undefined) {
            $("#" + id).data("kendoNumericTextBox").enable(enablement);
        }
    },

    //This method enables or disables Kendo Dropdown based on id and boolean value
    enableKendoDropdown: function (id, enablement) {
        if ($("#" + id).data("kendoDropDownList") != undefined) {
            $("#" + id).data("kendoDropDownList").enable(enablement);
        }
    },

    /// Show all messages in viewport area
    showMessagesInViewPort: function () {
        if (!sg.utls.isSameOrigin()) {
            return;
        }
        try {
            var activeScreenIframeId = window.top.$('iframe.screenIframe:visible').attr('id');
            var activeScreenIframeContents = window.top.$('#' + activeScreenIframeId).contents();
            var divSuccess = activeScreenIframeContents.find('#success');
            var divMessage = activeScreenIframeContents.find('#message');
            var divDeleteConfirm = activeScreenIframeContents.find('#deleteConfirmationParent');

            // Displaying message only when it is visible.
            if ((divSuccess !== null && divSuccess.html() != null && divSuccess.html().length > 0)) {
                sg.utls.setScrollPosition(divSuccess);
            }
            if ((divMessage !== null && divMessage.html() != null && divMessage.html().length > 0)) {
                sg.utls.setScrollPosition(divMessage);
            }
            if ((divDeleteConfirm !== null && divDeleteConfirm.length > 0)) {
                sg.utls.setScrollPosition(divDeleteConfirm);
                var horizontalPos = ($(window).width() - divDeleteConfirm.width()) / 2;
                divDeleteConfirm.css('left', horizontalPos);
            }

            var spinner = activeScreenIframeContents.find('#ajaxSpinner');
            if (spinner !== null && spinner.length > 0) {
                sg.utls.setScrollPosition(spinner);
            }
        }
        catch (err) {
            //don't do anything. This means window is already destroyed or does not exists.
            //This is required for iframes
            //console.log("Error updating iFrame");
        }
    },

    // Set Scrolling Position
    setScrollPosition: function (container) {
        //offsetPixels - Set this variable with the desired height of portal header
        var offsetPixels = sg.utls.portalHeight - 75;
        var offsetY = $(window.top).scrollTop();

        if (offsetY > offsetPixels) {
            var scrollTop = offsetY - offsetPixels;
            sg.utls.popupTopPosition = scrollTop;
            container.css('top', scrollTop);
        } else {
            container.css('top', "0px");
            sg.utls.popupTopPosition = 0;
        }
    },

    // This method enables MVC Razor data annotations for kendo controls
    enableKendoDataAnnotations: function (formId) {
        //setting ignore of hidden fields to null
        $("#" + formId).data().validator.settings.ignore = '';
    },

    // Kendo Window positioning in viewport area
    setKendoWindowPosition: function (kendoWindow) {
        kendoWindow.element.closest(".k-window").css({
            top: function () {
                if (!sg.utls.isSameOrigin()) {
                    return 20;
                }
                // Calculating top position to viewport center and reducing portal height based on scrolled position.
                var offsetPixels = sg.utls.portalHeight - 45;
                var topPos = $(window.top).scrollTop() - offsetPixels;
                if (sg.utls.isKendoIframe() || !sg.utls.isPortalIntegrated()) {
                    topPos = $(window).scrollTop();
                }

                // To make sure the popup is opening above the footer.
                var leastResolutionHeight = 768;
                var basePageHeight = $(window.top.document).height();
                var screenHeight = $(window.top).height() + $(window.top).scrollTop();
                if (basePageHeight == screenHeight && $(window.top).height() <= leastResolutionHeight) {
                    topPos = topPos - 40;
                }

                // For low resolution, top position may result to negative. So reset it to zero.
                if (topPos < 0) {
                    topPos = 0;
                }

                /// This code is added to fix the browser specific defect D-18490. 
                /// It happens only in specific version. It's not reproducible in latest version of Chrome browser. Hence commenting the code.
                //if (topPos > 800) {
                //    if (sg.utls.isChrome()) {
                //        topPos = 250;
                //    }
                //}

                return topPos;
            },
            left: ($(window).innerWidth() - kendoWindow.wrapper.width()) / 2
        });
    },
    initFormValidation: function () {
        $("input[data-val-length-max]").each(function () {
            var $this = $(this);
            var data = $this.data();
            if (data.valLengthMax)
                $this.attr("maxlength", data.valLengthMax);
        });

        //This may not be required.
        $("input[data-val-range-max]").each(function () {
            var $this = $(this);
            var data = $this.data();
            if (data.valRangeMax) {
                var val = data.valRangeMax.toString();
                $this.attr("maxlength", val.length);
            }
        });
    },

    /**
     * Parameters to initialize the hamburger menu items.
     * @method labelMenuParams
     * @param {} Id - Id of the menu.
     * @param {} Value - Display text of the menu.
     * @param {} callback - click event of the menu.
     * @param {} koAttributes - ko attributes of the menu.
     */
    labelMenuParams: function (Id, Value, callback, koAttributes) {
        var obj = { Id: Id, Value: Value, callback: callback };

        if (koAttributes) {
            obj.koAttributes = koAttributes;
        }

        return obj;
    },

    //Function to return the encoded value. 
    textEncode: function (value) {
        value = sg.controls.GetString(value);
        return encodeURIComponent(value);
    },
    //Function to encode the query string values from the URL
    urlEncode: function (url) {
        var encodedUrl;

        if (url.indexOf("?") > -1) {
            var allUrlComponents = url.split("?");
            var urlComponent = allUrlComponents[0];
            var allQueryStrings = allUrlComponents[1].split("&");

            encodedUrl = urlComponent;


            for (var i = 0; i < allQueryStrings.length; i++) {
                var queryStringPairs = allQueryStrings[i].split("=");
                var encodedQueryStringValue = sg.utls.textEncode(queryStringPairs[1]);
                var literal = (i == 0) ? "?" : "&";
                encodedUrl = encodedUrl + literal + queryStringPairs[0] + "=" + encodedQueryStringValue;
            }
        } else {
            encodedUrl = url;
        }

        return encodedUrl;
    },

    RedirectToTimeoutLanding: function () {
        // remove all beforeunload event handler to prevent dialog box block the exist
        $(window).unbind('beforeunload');

        $.each($('iframe'), function (i, currentIFrame) {
            var currentWindow = (currentIFrame.contentWindow || currentIFrame.contentDocument);
            if (currentWindow.$) {
                // have to use the instance of the JQuery inside that window/iframe
                currentWindow.$(currentWindow).unbind('beforeunload');
            }
        });

        // For Admin login, call AdminSessionExpired. Otherwise, SessionExpired. 
        window.location.replace(sg.utls.url.buildUrl("Core", "Authentication", (typeof customScreenUI !== "undefined" && customScreenUI.adminLogin) ? "AdminSessionExpired" : "SessionExpired"));
    },

    mergeGridConfiguration: function (propertiesArray, targetConfig, sourceConfig) {
        // assign all properties
        $.each(propertiesArray, function (index, value) {
            targetConfig[value] = sourceConfig[value];
        });

        //merge addtional config into the main grid configuration
        //First, loop through all additional fields base on field name
        if (sourceConfig.additionalConfig) {
            for (var fieldName in sourceConfig.additionalConfig) {

                // get the one from main grid config
                var targetColConfig = $.grep(targetConfig.columns, function (e) { return e.field === fieldName; });

                // get the addional list of properties
                var additionalProps = Object.keys(sourceConfig.additionalConfig[fieldName]);

                // copy values from additional to main grid config
                for (var j in additionalProps) {
                    targetColConfig[0][additionalProps[j]] = sourceConfig.additionalConfig[fieldName][additionalProps[j]];
                }
            }
        }
    },

    //Note: When changing the parameters, make sure to change the createInquiryURLWithParameters function under TaskDock-Menu-BreadCrumb.js as well
    getInquiryParameterData: function (url, module, feature, target, value, title) {
        return sg.utls.formatString("{\"url\":\"{0}\",\"module\":\"{1}\",\"feature\":\"{2}\",\"target\":\"{3}\", \"value\":\"{4}\", \"title\":\"{5}\"}", url, module, feature, target, value, title);
    },

    getUrlPath: function (url) {
        var parser = document.createElement('a');
        parser.href = url;

        return parser.pathname;
    },

    saveUserPreferences: function (key, value) {
        var data = { key: key, value: value }
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "SaveUserPreference"), data, function(result) {
            console.log("SaveUserPreferences: " + result); //result is either true or false
        });
    },
    getUserPreferences: function (key, successHandler) {
        var data = { key: key }
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "GetUserPreference"), data, successHandler);
    },
});

$.extend(sg.utls.ko, {
    toJS: function (model, ignore) {
        var options = {
            ignore: ignore
        };
        return ko.mapping.toJS(model, options);
    },

    arrayFirstIndexOf: function (array, predicate, predicateOwner) {
        for (var i = 0, j = array.length; i < j; i++) {
            if (predicate.call(predicateOwner, array[i])) {
                return i;
            }
        }
        return -1;
    },
    arrayFirstItemOf: function (array, predicate, predicateOwner) {
        for (var i = 0, j = array.length; i < j; i++) {
            if (predicate.call(predicateOwner, array[i])) {
                return array[i];
            }
        }
        return -1;
    },

    hasNewLine: function (observableArray) {
        var hasNewLine = ko.utils.arrayFirst(ko.utils.unwrapObservable(observableArray), function (item) {
            if ($.isFunction(item["IsNewLine"])) {
                return item["IsNewLine"]() === true;
            }
        });
        if (hasNewLine) {
            return true;
        }
        return false;
    },
    getChangedItems: function (observableArray) {
        var changedItems = [];

        ko.utils.arrayFirst(ko.utils.unwrapObservable(observableArray), function (item) {
            if ($.isFunction(item["HasChanged"])) {
                if (item["HasChanged"]() === true) {
                    changedItems.push(item);
                }
            }
        });

        return changedItems;
    },
    removeDeletedRows: function (observableArray) {
        observableArray.remove(function (item) {
            return item.IsDeleted() === true;
        });
    },
});

$.extend(sg.utls.collapsibleScreen, {

    toggleSection: function (id) {
        $(id + " + .panel-content").toggle("fast");
        $(id + ">.more").toggleClass("hide");
        $(id + ">.panel-icon").toggleClass("panel-icon-up");
    },

    expandSection: function (id) {
        if ($(id + " + .panel-content").css('display') === "none") {
            sg.utls.collapsibleScreen.toggleSection(id);
        }
    },

    collapseSection: function (id) {
        if ($(id + " + .panel-content").css('display') === "block") {
            sg.utls.collapsibleScreen.toggleSection(id);
        }
    },

    setup: function (advancedId, simpleId, advancedList, simpleList) {
        $("#" + advancedId).click(function () {
            if ($(this).hasClass("no-active")) {
                $(this).removeClass("no-active").addClass("active");
                $("#" + simpleId).removeClass("active").addClass("no-active");

                // check all items, if not already expand, expand it
                $.each($.merge($.merge([], advancedList), simpleList), function (index, value) {
                    sg.utls.collapsibleScreen.expandSection("#" + value);
                });
            }
        });

        $("#" + simpleId).click(function () {
            if ($(this).hasClass("no-active")) {
                $(this).removeClass("no-active").addClass("active");
                $("#" + advancedId).removeClass("active").addClass("no-active");

                // expand simple, collapse advance
                $.each(simpleList, function (index, value) {
                    sg.utls.collapsibleScreen.expandSection("#" + value);
                });

                $.each(advancedList, function (index, value) {
                    sg.utls.collapsibleScreen.collapseSection("#" + value);
                });
            }
        });

        $.each($.merge($.merge([], advancedList), simpleList), function (index, value) {
            var valueId = "#" + value;
            $(valueId).click(function () {
                sg.utls.collapsibleScreen.toggleSection(valueId);
            });
        });
    }
});

var ajaxSuccess = {
    getCurrency: function (data) {
        sg.utls.homeCurrency = data.Currency;
        sg.utls.isPhoneNumberFormatRequired = data.IsPhoneNumberFormatRequired;
    }
};

var AllowedInput =
{
    Numeric: [],
    Alpha: [],
    AlphaNumeric: []
};

$(document).on("keypress", "[formatTextbox='numeric']", function (e) {
    var numericKeys = AllowedInput.Numeric;
    if (numericKeys.length == 0) {
        for (var i = 48; i <= 57; i++)
            numericKeys.push(i);
        AllowedInput.Numeric = numericKeys;
    }

    var k;
    // code for Ie and Chrome browser
    if (!(navigator.userAgent.search("Firefox") > -1)) {
        k = e.which;
        if (!(numericKeys.indexOf(k) >= 0))
            e.preventDefault();
    } else {
        k = e.which == 0 ? e.keyCode : e.which;
        var fields = sg.utls.getFirefoxSpecialKeys(e);
        if (!((numericKeys.indexOf(k) >= 0) || (fields.indexOf(k) >= 0)))
            return false;
    }
});
$(document).on("keypress", "[formatTextbox='alpha']", function (e) {
    var alphaKeys = AllowedInput.Alpha;

    if (alphaKeys.length == 0) {
        for (var i = 65; i <= 90; i++)
            alphaKeys.push(i);
        for (i = 97; i <= 122; i++)
            alphaKeys.push(i);
        AllowedInput.Alpha = alphaKeys;
    }

    var k;
    // code for Ie and Chrome browser
    if (!(navigator.userAgent.search("Firefox") > -1)) {
        k = e.which;
        if (!(alphaKeys.indexOf(k) >= 0))
            e.preventDefault();
    } else {
        k = e.which == 0 ? e.keyCode : e.which;
        var fields = sg.utls.getFirefoxSpecialKeys(e);
        if (!((alphaKeys.indexOf(k) >= 0) || (fields.indexOf(k) >= 0)))
            return false;
    }
});
$(document).on("keypress", "[formatTextbox='date']", function (e) {
    var charCode = (e.which) ? e.which : e.keyCode;
    var separator = globalResource.DateSeparator;
    var separaterCode;
    switch (separator) {
        case "-":
            separaterCode = 45;
            break;
        case ".":
            separaterCode = 46;
            break;
        case "/":
            separaterCode = 47;
            break;
        default: separaterCode = 32;
    }
    if (navigator.userAgent.indexOf("Firefox") != -1) {
        var fields = sg.utls.getFirefoxSpecialKeys(e);
        if (fields.indexOf(charCode) >= 0) {
            return true;
        }
    }
    if ((charCode >= 48 && charCode <= 57)
         || (charCode === separaterCode) || (charCode === 8) || (charCode === 9) ||
        (sg.utls.isCtrlKeyPressed && (charCode === 99 || charCode === 118))) {
        return true;
    }
    return false;
});
$(document).on("keypress", "[formatTextbox='alphaNumeric']", function (e) {
    var alphaNumericKeys = AllowedInput.AlphaNumeric;

    if (alphaNumericKeys.length == 0) {
        for (var i = 48; i <= 57; i++)
            alphaNumericKeys.push(i);
        for (var i = 65; i <= 90; i++)
            alphaNumericKeys.push(i);
        for (i = 97; i <= 122; i++)
            alphaNumericKeys.push(i);
        AllowedInput.AlphaNumeric = alphaNumericKeys;
    }
    var k;
    // code for Ie and Chrome browser
    if (!(navigator.userAgent.search("Firefox") > -1)) {
        k = e.which;
        if (!(alphaNumericKeys.indexOf(k) >= 0))
            e.preventDefault();
    } else {
        k = e.which == 0 ? e.keyCode : e.which;
        var fields = sg.utls.getFirefoxSpecialKeys(e);
        if (!((alphaNumericKeys.indexOf(k) >= 0) || (fields.indexOf(k) >= 0)))
            return false;
    }
});
$(document).on("keyup", ".grid-upper", function () {
    $(this).val(sg.utls.toUpperCase($(this).val()));
});
$(document).on("focusout", "[prefixZero='true']", function (e) {
    if (e.currentTarget.value.length < e.currentTarget.maxLength) {
        this.value = sg.utls.kndoUI.getTextValue(e.currentTarget.maxLength - e.currentTarget.value.length) + e.currentTarget.value;
    }
});
$(document).on("keypress", "[formatTextbox='time']", function (e) {
    var charCode = (e.which) ? e.which : e.keyCode;
    var separaterCode = 58;
    if ((charCode >= 48 && charCode <= 57) || (charCode === separaterCode)) {
        return true;
    }
    return false;
});
$(document).on("focusout", "[formatTextbox='time']", function (e) {
    var value = $(this).val();
    var patt = new RegExp(sg.utls.regExp.TIME, sg.utls.regExp.ONLYGLOBAL);
    if (!patt.test(value)) {
        $(this).val("");
    }
});

window.onerror = function (msg, url, line) {
    // If the window has closed, sg will be undefined.
    if (sg === undefined) {
        return;
    }

    var data = {
        Message: msg,
        Url: url,
        Line: line
    };

    if (!sg.utls.hasTriedToNotify) {
        sg.utls.hasTriedToNotify = true;//to avoid recursive loop
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "LogJavascriptError"), data, function () { sg.utls.hasTriedToNotify = false });
    }
};

/**
 * Add Sage-specific functionality to the jQuery namespace.
 */
$(function () {

    if (sg.utls.isPortalIntegrated()) {
        window.top.SessionManager.ResetSessionTimer();
        var events = 'ajaxSend';
        $(document).bind($.trim((events + ' ').split(' ').join('.idleTimer ')), window.top.SessionManager.ResetSessionTimer);
    }

    window.ga = window.ga || function () { (ga.q = ga.q || []).push(arguments) }; ga.l = +new Date;
    ga('create', 'UA-47089329-1', 'auto');
    ga('send', 'pageview');

    kendo.culture(globalResource.Culture);

    var coreIndex = window.location.href.indexOf("/Core/");
    var sharedIndex = window.location.href.indexOf("/Shared/");
    var customAdmin = window.location.href.indexOf("/AS/CustomScreen");

    if ((coreIndex < 0) && sharedIndex < 0 && customAdmin < 0) {
        sg.utls.loadHomeCurrency();
    }

    var screenHome = "999999";
    var isHomePage = self.location === top.location && $("#ScreenName").val() === screenHome;

    //In Home page on mouse click, this event is executing and hence success message is not displaying, so if the page is home 
    //then don't execute this code
    if (!isHomePage) {
        $("body").on("click", function () {
            var successControl = $(this).find("#success");
            if (successControl.length > 0) {
                successControl.empty();
            }
        });
    }

    $(document).on("click", ".msgCtrl-close", function () {
        $("#message").hide();
        $("#message").empty();

        //Remove injected overlay if exists
        $("#injectedOverlay").remove();
        $("#injectedOverlayTransparent").remove();
    });

    $(document).on("click", ".dropDown-Menu ul.sub-menu > li", function () {
        $(this).parent(".sub-menu").hide();
    });

    $(document).on("mouseover", ".dropDown-Menu", function () {
        $(this).parent(".sub-menu").show();
    });

    //Set flag if shift key is pressed. Used for grid tabbing
    $(document).bind('keyup keydown', function (e) {
        sg.utls.isShiftKeyPressed = e.shiftKey;
        sg.utls.isCtrlKeyPressed = e.ctrlKey;
        var keyCode = e.keyCode || e.which;
        if (keyCode === 9) {
            sg.utls.istabKeyPressed = true;
        }

        return true;
    });

    $(document).bind('mouseup mousedown', function (e) {
        sg.utls.istabKeyPressed = false;
    });

    $("input[data-val-length-max]").each(function () {
        var $this = $(this);
        var data = $this.data();
        if (data.valLengthMax)
            $this.attr("maxlength", data.valLengthMax);
    });

    //This may not be required.
    $("input[data-val-range-max]").each(function () {
        var $this = $(this);
        var data = $this.data();
        if (data.valRangeMax) {
            var val = data.valRangeMax.toString();
            $this.attr("maxlength", val.length);
        }
    });

    var menuLink = $(".dropDown-Menu > li");
    menuLink.find("> a").append('<span class="arrow-grey"></span>');
    menuLink.hover(function () {
        $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
        $(this).children(".sub-menu").show();
    }, function () {
        $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
        $(this).children(".sub-menu").hide();
    });

    function editCell() {
        $(document).on("focus", ".k-edit-cell", function () {
            if ($(this).find("input").length > 1) {
                var parentWidth = $(this).width();
                var inputWidth = parentWidth - 30 + "px";

                $(".grid_inpt").css("width", inputWidth);
            }
        });

    };

    // Highlight all the text inside after focusing on a numeric textbox
    $(document).on('focus', '.k-input', function () {
        var input = $(this);
        //In iPad, highlight is not required but we still need to select to show keyboard
        if (sg.utls.isMobile()) {
            input.select();
        } else {
            setTimeout(function () { input.select(); });
        }
    });

    function PageUnloadHandler() {
        if (!sg.utls.isSameOrigin()) {
            return;
        }

        if (sg.utls.screenUnloadHandler !== null) {
            sg.utls.screenUnloadHandler();
            sg.utls.screenUnloadHandler = null;
        }
        else if (parent.sg && parent.sg.utls.screenUnloadHandler !== null) {
            parent.sg.utls.screenUnloadHandler();
            parent.sg.utls.screenUnloadHandler = null;
        }
    }

    if (isHomePage) {
        $(window).bind('unload', function () {
            PageUnloadHandler();
            if (globalResource.AllowPageUnloadEvent) {
                //destroy session after calling compelted
                sg.utls.destroySessions();
            }
        });
        $(window).scroll(function () {
            //console.log('Global scroll event is raised.');
            //sg.utls.scrollPosition = $(window.top).scrollTop();
            sg.utls.showMessagesInViewPort();
        });
    } else {
        var sessionPerPage = $("#SessionPerPage");
        if (sessionPerPage.length > 0 && sessionPerPage.val() === "True") {
            $(window).bind('unload', function () {
                PageUnloadHandler();
                if (globalResource.AllowPageUnloadEvent) {
                    sg.utls.destroySession();
                }
            });
        }
        else {
            $(window).bind('unload', function () {
                window.name = "unloadediFrame";
                PageUnloadHandler();
            });
        }
    }


    jQuery.validator.addMethod("format", function (source, parameters) {
        return $.validator.format(source, parameters);
    });


    //For some reason, clicking the pencil button in a grid will also trigger a close event in Mac Safari browser
    if (sg.utls.isSafari()) {
        $(".datagrid-group").bind("mousedown", function (e) {
            if ($(e.target).is('input:button')) {
                e.preventDefault();
                e.stopImmediatePropagation();
            }
        });
    }


});








//script to check the div has scrollbars
//usage
//$('div:hasScroll') //all divs that have scrollbars
//$('div').filter(':hasScroll') //same but better
//$(this).closest(':hasScroll(y)') //find the parent with the vert scrollbar
//$list.is(':hasScroll(x)') //are there any horizontal scrollbars in the list?

(function ($) {
    function hasScroll(el, index, match) {
        console.log("scroll");
        var $el = $(el),
            sX = $el.css('overflow-x'),
            sY = $el.css('overflow-y'),
            hidden = 'hidden',
            visible = 'visible',
            scroll = 'scroll',
            axis = match[3]; // regex for filter -> 3 == args to selector

        if (!axis) {
            if (sX === sY && (sY === hidden || sY === visible)) {
                return false;
            }
            if (sX === scroll || sY === scroll) { return true; }
        } else if (axis === 'x') {
            if (sX === hidden || sX === visible) { return false; }
            if (sX === scroll) { return true; }
        } else if (axis === 'y') {
            if (sY === hidden || sY === visible) { return false; }
            if (sY === scroll) { return true };
        }

        //Compare client and scroll dimensions to see if a scrollbar is needed
        return $el.innerHeight() < el.scrollHeight
            || $el.innerWidth() < el.scrollWidth;
    }
    $.expr[':'].hasScroll = hasScroll;
    $.fn.hasScroll = function (axis) {
        var el = this[0];
        if (!el) { return false; }
        return hasScroll(el, 0, [0, 0, 0, axis]);
    };
}(jQuery));