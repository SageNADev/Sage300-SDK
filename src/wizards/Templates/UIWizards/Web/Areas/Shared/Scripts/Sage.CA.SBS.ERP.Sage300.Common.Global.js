/* Copyright (c) 1994-2023 The Sage Group plc or its licensors.  All rights reserved. */

// @ts-check

// Note: 
//       Enabling 'use strict' line below seems to cause unit tests to fail.
//       Also, using some ECMAScript 6 features will cause unit tests to fail.
//       For example, using the ?. or ?? constructs will cause unit tests to fail.
//       Using let (or const) over var is ok.
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

/**
 * Sage 300 license status
 */
sg.utls.LicenseStatus = {
    Expired: -2,
    NotFound: -1,
    OK: 0
};

sg.utls.NavigationAction = {
    None: 0,
    First: 1,
    Previous: 2,
    Next: 3,
    Last: 4
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

// Use to do string format, kind of like String.format in C#
String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        str = str.replace(reg, arguments[i]);
    }
    return str;
};

$.extend(sg.utls.url, {

    // Predefined Urls
    loginUrl: function (isAdmin) { return sg.utls.url.buildUrl("Core", "Authentication", isAdmin ? "AdminLogin" : "Login"); },
    logoutUrl: function () { return sg.utls.url.buildUrl("Core", "Authentication", "Logout"); },
    evictUrl: function() { return sg.utls.url.buildUrl("Core", "Authentication", "SessionEvicted"); },
    saveGridPreferencesUrl: function () { return sg.utls.url.buildUrl("Core", "Common", "SaveGridPreferences"); },
    getGridPreferencesUrl: function () { return sg.utls.url.buildUrl("Core", "Common", "GetGridPreferences"); },
    logJavascriptErrorUrl: function () { return sg.utls.url.buildUrl("Core", "Common", "LogJavascriptError"); },
    logJavascriptMessageUrl: function () { return sg.utls.url.buildUrl("Core", "Common", "LogJavascriptMessage"); },
    releaseSessionUrl: function () { return sg.utls.url.buildUrl("Core", "Session", "ReleaseSession"); },
    destroyPoolUrl: function () { return sg.utls.url.buildUrl("Core", "Session", "DestroyPool"); },
    destroySessionUrl: function () { return sg.utls.url.buildUrl("Core", "Session", "Destroy"); },
    getApplicationConfigUrl: function () { return sg.utls.url.buildUrl("CS", "CompanyProfile", "GetApplicationConfig"); },
    getCompanyColorUrl: function () { return sg.utls.url.buildUrl("Core", "Common", "GetCompanyColorCode"); },

    baseUrl: function () {
        return $("#hdnUrl").val();
    },

    /**
     * @name buildUrl
     * @description Build a fully-qualified url
     * @param {string} area - The string representing the area name
     * @param {string} controller  - The string representing the controller name
     * @param {string} action  - The string representing the action name
     * @param {string} parameters  - The string representing the parameters
     * @returns {string} The fully-qualified url
     */
    buildUrl: function (area, controller, action, parameters) {
        // MultSessions TODO - This will always include the session, but certain Core routes do not require it
        // Thus, will need to add session to all routes OR call a routine that removes the session from the route
        // when it is not needed
        var siteUrl = sg.utls.url.baseUrl(),
            slash = "/";

        // Ensure that parameters are valid objects
        if (!area) { area = ""; }
        if (!controller) { controller = ""; }
        if (!action) { action = ""; }
        if (!parameters) { parameters = ""; }

        // Build the rest of the url and return
        siteUrl += area.length > 0 ? area : "";
        siteUrl += controller.length > 0 ? slash + controller : "";
        siteUrl += action.length > 0 ? slash + action : "";
        siteUrl += parameters.length > 0 ? slash + parameters : "";
        return siteUrl;
    },

    /**
     * @name extractAreaAndControllerFromPartialUrl
     * @description This method will extract the area and controller names from a partial url.
     *              Example: AR/Customer -> Area = AR, Controller = Customer
     * @param {string} partialUrl - This is the partial url Ex: AR/Customer
     * @return { object } - Json object representing the extracted area and controller names
     */
    extractAreaAndControllerFromPartialUrl: function (partialUrl) {
        var payload = null;
        if (partialUrl && partialUrl.length > 0) {
            var parts = partialUrl.split("/");
            payload = { AreaName: parts[0], ControllerName: parts[1] };
        }
        return payload;
    },

    /**
     * @name extractAreaControllerActionAndParametersFromPartialUrl
     * @description This method will extract the area, controller, action and parameters from a partial url.
     *              Example: AR/Customer/Index/?id=1234 -> Area = AR, Controller = Customer, Action = Index, Parameters = ?id=1234
     * @param {string} partialUrl - This is the partial url Ex: AR/Customer/Index/?id=1234
     * @return { object } - Json object representing the extracted area, controller, action and parameters
     */
    extractAreaControllerActionAndParametersFromPartialUrl: function (partialUrl) {
        var payload = null;
        if (partialUrl && partialUrl.length > 0) {
            var parts = partialUrl.split("/");
            payload = {
                AreaName: parts[0],
                ControllerName: parts[1],
                ActionName: parts[2],
                ParametersName: parts[3]
            };
        }
        return payload;
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

    /**
     * @name isSameOrigin
     * @description Compare the original url with the current url to
     *              see if they are from the same origin
     * @returns {boolean} true = same origin | false = different origin
     */
    isSameOrigin: function () {
        var sage300Origin = $("#Sage300Origin").val();
        var currentOrigin = window.location.href;
        var a1 = $('<a>', { href: sage300Origin })[0];
        var a2 = $('<a>', { href: currentOrigin })[0];
        return a1.protocol === a2.protocol &&
            a1.hostname === a2.hostname &&
            a1.port === a2.port;
    },

    /**
     * @name urlEncode
     * @description Url encode a string
     *              Note: This method is the same as sg.utls.urlEncode
     *                    and is duplicated here for consistency purposes
     * @param {string} url - The url to encode
     * @returns {string} The encoded string
     */
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

    /**
     * @name buildCacheBuster
     * @description Create an url parameter meant to invalidate the cache
     * @returns {string} return an url parameter for cache busting
     */
    buildCacheBuster: function () {
        var cb = ""; 
        if (sg.utls.isInternetExplorer()) {
            // Generate date + random number to make the URL unique
            cb = "&cb=" + encodeURI((new Date()).toString() + Math.floor(Math.random() * 10000000));
        }
        return cb;
    }
});

$.extend(sg.utls, {
    functionsToCall: [],
    ajaxRunning: false,
    isProcessRunning: false,
    gridPageSize: 10,
    reorderable: false,
    hasTriedToNotify: false,
    findersList: {}, //List of finders for hotkey

    //Navigation control support
    bindingNavigationActions: (uiObject, keyFieldName, txtBoxId, navGroupIndex = 0, checkDirtyFuncName ='checkIsDirty', getFuncName ='get', gridName = "") => {
        //Attach navigation action handler
        const suffix = navGroupIndex == 0 ? '' : navGroupIndex.toString();
        [`btnDataFirst${suffix}`, `btnDataPrevious${suffix}`, `btnDataNext${suffix}`, `btnDataLast${suffix}`].forEach((id, index) => {
            $('#' + id).on('click', (e) => {
                uiObject[`navigationAction${suffix}`] = index + 1;
                $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}, #btnDataNext${suffix}, #btnDataLast${suffix}`).prop('disabled', true);
                if (!gridName) {
                    let func = uiObject[checkDirtyFuncName];
                    if (checkDirtyFuncName.includes('.')) {
                        const ns = checkDirtyFuncName.split('.');
                        func = ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[checkDirtyFuncName];
                    }
                    if (func && func instanceof Function) {
                        let getFunc = uiObject[getFuncName];
                        if (!getFunc && getFuncName.includes('.')) {
                            const ns = getFuncName.split('.');
                            getFunc = ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[getFuncName];
                        }
                        func(getFunc, uiObject[keyFieldName]);
                    } else {
                        const getFunc = uiObject[getFuncName];
                        if (getFunc && getFunc instanceof Function) {
                            uiObject[getFuncName]();
                        }
                    }
                }
                // detail popup
                else {
                    gridChangeLine();
                }
            });
        });
        
        if (txtBoxId) {
        //Navigation key support
            let id = txtBoxId.startsWith("#") ? txtBoxId : '#' + txtBoxId;
            $(id).on('keydown', e => {
                switch (e.key) {
                    case "ArrowLeft":
                        e.preventDefault();
                        $(e.ctrlKey ? `#btnDataFirst${suffix}` : `#btnDataPrevious${suffix}`).trigger('click');
                        break;
                    case "ArrowRight":
                        e.preventDefault();
                        $(e.ctrlKey ? `#btnDataLast${suffix}` : `#btnDataNext${suffix}`).trigger('click');
                        break;
                }
            })

            //Navigation control focus
            $(id).focus(function () {
                $(`#divNavGroup${suffix}`).addClass("focused");
            });
            $(id).focusout(function () {
                $(`#divNavGroup${suffix}`).removeClass("focused");
            });
            $(`#divNavGroup`).addClass("focused");

            if (gridName) {
                $(`#divNavGroup${suffix}`).addClass("focused");
            }

            if ((navGroupIndex > 0) && ($(id).val() == "" || $(id).val() == "0") && !gridName) {
                $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}, #btnDataNext${suffix}, #btnDataLast${suffix}`).prop('disabled', true);
            }

            // detail popup change and add line
            if (gridName) {
                $(`#${txtBoxId}`).on('blur keypress', function (e) {
                    // 1. formattextbox=numeric prevents non-digits from being entered, need to specify keypress event for enter key
                    // 2. preventDefault in keypress event from formattextbox=numeric sometimes causes change event to be cancelled due to the order of events in some browsers. Use blur event instead
                    if ((e.type === 'blur' || (e.type === 'keypress' && e.which === sg.constants.KeyCodeEnum.Enter))
                        && this.lastValue !== this.value) {
                        if (!isNaN(this.value)) {
                            uiObject[`navigationPromise${suffix}`] = new Promise((resolve) => {
                                uiObject[`navigationResolve${suffix}`] = resolve;
                                gridChangeLine();
                            }).catch(() => {
                                uiObject[`navigationPromise${suffix}`] = null;
                                uiObject[`navigationResolve${suffix}`] = null;
                            });
                        }
                    }
                });
                // save previous value to check for change in blur event
                $(`#${txtBoxId}`).on('focus', function (e) {
                    this.lastValue = this.value;
                });

                $(`#btnNew${suffix}`).on('click', function () {
                    if (uiObject[`navigationPromise${suffix}`]) {
                        uiObject[`navigationPromise${suffix}`].then(() => {
                            gridAddLine();
                            uiObject[`navigationPromise${suffix}`] = null;
                            uiObject[`navigationResolve${suffix}`] = null;
                        });
                    }
                    else {
                        gridAddLine();
                    }
                });
            }
        };

        // select grid row calculated from navigationAction
        const gridAddLine = () => {
            let func = uiObject[checkDirtyFuncName];
            if (checkDirtyFuncName.includes('.')) {
                const ns = checkDirtyFuncName.split('.');
                func = ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[checkDirtyFuncName];
            }
            if (func && func instanceof Function) {
                func().then((ret) => {
                    const currentRecord = sg.viewList.currentRecord(gridName);
                    const isNewLine = currentRecord ? currentRecord.isNewLine : false;
                    if (ret) {
                        const message = sg.viewList.currentRecord(gridName).isNewLine ? jQuery.validator.format(globalResource.AddNewLineMessage) : jQuery.validator.format(globalResource.SaveChangesMessage);
                        sg.utls.showKendoConfirmationDialog(() => {
                            sg.viewList.addLine(gridName, ret, currentRecord);
                        }, () => {
                            sg.utls.naviControlStatusFocus(uiObject, keyFieldName, txtBoxId, suffix, gridName);
                            if (isNewLine) {
                                sg.viewList.clearNewRow(gridName);
                            }
                            else {
                                sg.viewList.resetCurrentRow(gridName, () => {
                                    sg.viewList.addLine(gridName, false, currentRecord);
                                });
                            }

                        }, message);
                    }
                    else {
                        sg.viewList.addLine(gridName, ret, currentRecord);
                    }
                });
            }
            else {
                sg.viewList.addLine(gridName, false);
            }
        };

        // select grid row calculated from navigationAction
        const gridChangeLine = () => {
            const grid = $(`#${gridName}`).data('kendoGrid');
            const total = grid.dataSource.total();
            let currentLine = sg.viewList.getCurrentLineNumber(gridName);
            const lastLine = currentLine;
            switch (uiObject[`navigationAction${suffix}`]) {
                case sg.utls.NavigationAction.First:
                    currentLine = 1;
                    break;
                case sg.utls.NavigationAction.Previous:
                    if (currentLine > 1)
                        currentLine--;
                    break;
                case sg.utls.NavigationAction.Next:
                    if (currentLine < total)
                        currentLine++;
                    break;
                case sg.utls.NavigationAction.Last:
                    currentLine = total;
                    break;
                case sg.utls.NavigationAction.None:
                    let dest = parseInt($(`#${txtBoxId}`).val());
                    if (isNaN(dest)) dest = currentLine;
                    if (dest > total) {
                        $(`#${txtBoxId}`).val(currentLine);
                    }
                    else {
                        currentLine = dest;
                    }
                    break;
                default:
                    break;
            }

            if (lastLine !== currentLine) {
                let func = uiObject[checkDirtyFuncName];
                if (checkDirtyFuncName.includes('.')) {
                    const ns = checkDirtyFuncName.split('.');
                    func = ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[checkDirtyFuncName];
                }
                if (func && func instanceof Function) {
                    func().then((ret) => {
                        const currentRecord = sg.viewList.currentRecord(gridName);
                        const isNewLine = currentRecord ? currentRecord.isNewLine : false;
                        if (ret) {
                            const message = currentRecord && currentRecord.isNewLine ? jQuery.validator.format(globalResource.AddNewLineMessage) : jQuery.validator.format(globalResource.SaveChangesMessage);
                            sg.utls.showKendoConfirmationDialog(() => {
                                sg.viewList.moveToRow(gridName, currentLine, ret);
                            }, () => {
                                sg.utls.naviControlStatusFocus(uiObject, keyFieldName, txtBoxId, suffix, gridName);
                                if (isNewLine) {
                                    sg.viewList.deleteLine(gridName, false, () => {
                                        sg.viewList.moveToRow(gridName, currentLine, false);
                                    });
                                }
                                else{
                                    sg.viewList.resetCurrentRow(gridName, () => {
                                        sg.viewList.moveToRow(gridName, currentLine, false);
                                    });
                                }
                            }, message);
                        }
                        else {
                            if (isNewLine) {
                                sg.viewList.deleteLine(gridName, false, () => {
                                    sg.viewList.moveToRow(gridName, currentLine, false);
                                });
                            }
                            else {
                                sg.viewList.moveToRow(gridName, currentLine, ret);
                            }
                        }
                    });
                }
                else {
                    sg.viewList.moveToRow(gridName, currentLine, false);
                }
            }
        };
    },

    // Navigation control focus, buttons enable/disabled after navigate
    naviControlStatusFocus: (uiObject, keyFieldName, txtBoxId, navGroupIndex = 0, gridName = "") => {
        const suffix = navGroupIndex == 0 ? '' : navGroupIndex.toString();
        // header
        if (!gridName) {
            if (uiObject[`navigationAction${suffix}`]) {
                $('#' + txtBoxId).focus();
                $(`#divNavGroup${suffix}`).addClass("focused");
            }

            let sameValue = $('#' + txtBoxId).val() == uiObject[keyFieldName];
            const navigationKeyValue = $('#' + txtBoxId).data('navigationKeyValue');
            if (navigationKeyValue) {
                sameValue = navigationKeyValue == uiObject[keyFieldName];
            }

            let disabled = uiObject[`navigationAction${suffix}`] === sg.utls.NavigationAction.First || (uiObject[`navigationAction${suffix}`] === sg.utls.NavigationAction.Previous && sameValue);
            $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}`).prop('disabled', disabled);

            disabled = uiObject[`navigationAction${suffix}`] === sg.utls.NavigationAction.Last || (uiObject[`navigationAction${suffix}`] === sg.utls.NavigationAction.Next && sameValue);
            $(`#btnDataLast${suffix}, #btnDataNext${suffix}`).prop('disabled', disabled);

            if (navGroupIndex > 0 && ($('#' + txtBoxId).val() == "" || $('#' + txtBoxId).val() == "0")) {
                $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}, #btnDataNext${suffix}, #btnDataLast${suffix}`).prop('disabled', true);
            }
        }
        // detail popup
        else {
            const grid = $(`#${gridName}`).data('kendoGrid');
            const lineNo = sg.viewList.getCurrentLineNumber(gridName);
            $(`#${txtBoxId}`).val(lineNo);
            $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}`).prop('disabled', lineNo === 1);
            $(`#btnDataLast${suffix}, #btnDataNext${suffix}`).prop('disabled', lineNo === grid.dataSource.total());
            $(`#${txtBoxId}`).focus();
        }
        uiObject[`navigationAction${suffix}`] = sg.utls.NavigationAction.None;

        // resolve promise to do actions after navigate, such as add line
        if (uiObject[`navigationResolve${suffix}`]) {
            uiObject[`navigationResolve${suffix}`]();
        }
    },

    // Reset navigation control focus and status
    resetNaviControlStatusFocus: (uiObject, navGroupIndex = 0, reset = false, gridName = "") => {
        const suffix = navGroupIndex == 0 ? '' : navGroupIndex.toString();
        if (uiObject[`navigationAction${suffix}`] !== sg.utls.NavigationAction.None || reset) {
            $(`#divNavGroup`).addClass("focused");
            uiObject[`navigationAction${suffix}`] = sg.utls.NavigationAction.None;
            if (!gridName) {
                $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}, #btnDataNext${suffix}, #btnDataLast${suffix}`).prop('disabled', false);
            }
            else {
                const grid = $(`#${gridName}`).data('kendoGrid');
                const lineNo = sg.viewList.getCurrentLineNumber(gridName);
                $(`#btnDataFirst${suffix}, #btnDataPrevious${suffix}`).prop('disabled', lineNo === 1);
                $(`#btnDataLast${suffix}, #btnDataNext${suffix}`).prop('disabled', lineNo === grid.dataSource.total());
            }
        }
    },

    /**
     * @description Initialize the keyboard navigation handlers for the grid
     * @param {string} gridName The name of the grid.
     * @param {boolean} isNewGrid true = Using new grid | false = Using previous grid
     * @param {object} eventHandlers Click handlers for Insert, Delete, F9 and Alt+c
     * @param {object} buttonNames The names of the Insert, Delete, View/Edit Details and Configure grid buttons
     */
    initGridKeyboardHandlers: function (gridName, isNewGrid, eventHandlers, buttonNames) {

        // 20211109 - Remove this after code freeze
        return;

        const gridId = `#${gridName}`;

        // Event handlers
        const { addLine, deleteLine, showDetails, editColumnSettings} = eventHandlers;

        // Button Names
        const { addLineButtonName, deleteLineButtonName, viewEditDetailsButtonName, editColumnSettingsButtonName } = buttonNames;

        // Hotkey to give focus to the grid
        $(document.body).keydown(function (e) {
            // Alt + G
            if (e.altKey && e.keyCode == sg.constants.KeyCodeEnum.G) {
                sg.utls.gridFocusByName(gridName);
            }
        });

        // This stuff only seems to work with the new grid code (AccpacGrid.js)
        // Note: Also, this only works when focus is INSIDE the grid
        // If outside of main grid, the keydown handler below this block is used instead.
        if (isNewGrid) {
            // Override the default Kendo Home and End behaviour (Only for new grid)
            kendo.ui.Grid.fn._handleHome = function () {
                new AccpacGridHelper(gridName).firstPage();
            };
            kendo.ui.Grid.fn._handleEnd = function () {
                new AccpacGridHelper(gridName).lastPage();
            };
        }

        $(`#${gridName}`).on('keydown', function (e) {

            const gridHelper = new AccpacGridHelper(gridName);
            const keyCodes = sg.constants.KeyCodeEnum;
            const code = e.keyCode;
            const altPressed = e.altKey;
            const isEditMode = $(gridId).find('.k-grid-edit-row').length > 0;
            const isPagerFocused = $(gridId).find(".k-pager-input > input.k-textbox").is(":focus");

            let insertButtonDisabled = true;
            let deleteButtonDisabled = true;
            insertButtonDisabled = $(`#${addLineButtonName}`).prop('disabled');
            deleteButtonDisabled = $(`#${deleteLineButtonName}`).prop('disabled');

            if (code === keyCodes.Insert) {

                if (!insertButtonDisabled && addLine) {
                    if (isNewGrid) {
                        sg.viewList.commit(gridName);
                        addLine(gridName);
                    } else {
                        addLine();
                    }
                }

            } else if (code === keyCodes.Delete) {

                if (!deleteButtonDisabled && deleteLine && !isEditMode && !isPagerFocused) {
                    isNewGrid ? deleteLine(gridName) : deleteLine();
                }

            } else if (code === keyCodes.F9) {
                if (showDetails) {
                    isNewGrid ? showDetails(gridName) : showDetails();
                }

            } else if (altPressed && code === keyCodes.C) {
                if (editColumnSettings) {
                    isNewGrid ? editColumnSettings(gridName) : editColumnSettings();
                }

            } else if (code === keyCodes.Home) {
                if (!isEditMode) {
                    gridHelper.firstPage();
                }

            } else if (code === keyCodes.End) {
                if (!isEditMode) {
                    gridHelper.lastPage();
                }

            } else if (code === keyCodes.PgUp) {
                if (!isEditMode) {
                    gridHelper.previousPage();
                }

            } else if (code === keyCodes.PgDn) {
                if (!isEditMode) {
                    gridHelper.nextPage();
                }

            } else if (code === keyCodes.ESC) {

                // If row/cell is in edit mode then close cell 
                // but don't allow downstream handler to be called otherwise 
                // Optional Field popup will also close.
                if (isEditMode) {
                    $(gridId).data('kendoGrid').closeCell();
                    sg.utls.gridFocusByName(gridName);
                    e.preventDefault();
                    e.stopImmediatePropagation();
                }
            }

        });
    },

    /**
     * @function
     * @name gridFocusByName
     * @description Set the focus to the grid table specifying the grid name
     * @namespace sg.utls
     * @public
     * 
     * @param {string} gridName The grid name
     */
    gridFocusByName: function (gridName) {
        if (gridName && gridName.length > 0) {
            $(`#${gridName}`).data('kendoGrid').table.focus();
        }
    },

    /**
     * @function
     * @name removePrependedHashtag
     * @description Remove the preceding hashtag from a string if it exists
     *              If the string doesn't start with an hashtag, just return 
     *              the original input string.
     * @namespace sg.utls
     * @public
     *
     * @param {string} nameWithHashtag The string containing the possible preceding hashtag to remove
     */
    removePrependedHashtag: function (nameWithHashtag) {
        let rval = nameWithHashtag;
        if (nameWithHashtag && nameWithHashtag.length > 0) {
            let hashtagIndex = nameWithHashtag.indexOf('#');
            if (hashtagIndex === 0) {
                rval = nameWithHashtag.substring(1);
            }
        }
        return rval;
    },

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

    getViewFinderOptions: function (id) {
        for (var i = 0; i < $(":sageuiwidgets-ViewFinder").length; i++) {
            var widget = $(":sageuiwidgets-ViewFinder")[i];
            if (id === widget.id) {
                for (var prop in Object.keys(widget)) {
                    var name = Object.keys(widget)[prop];
                    var obj = widget[name];
                    if (obj["sageuiwidgets-ViewFinder"] !== undefined) {
                        return obj["sageuiwidgets-ViewFinder"].options;
                    }
                }
            }
        }
        return null;
    },

    setFinderSearchResult: function (jsonResult) {
        if (jsonResult !== null) {
            if (jsonResult.Response.RecordExists === false) {    // invalid input typed
                $("#" + jsonResult.Response.FieldId).focus();    // set focus back on control (in case of tabbed-out)
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jsonResult.UserMessage.Errors[0].Message, () => {
                    $("#" + jsonResult.Response.FieldId).val(jsonResult.Response.FieldPrev); // reset control to previous value
                    $("#" + jsonResult.Response.FieldId).focus(); // error-message exit removes highlight, so set it back
                });
            } else if (jsonResult.Response.FocusedId === "@") {  // finder pop-up selection
                $("#" + jsonResult.Response.FieldId).focus();
            } 
        }
    },

    isFinderSearchSupported: function (jsonResult) {
        return window.location.href.includes("/PR/");
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
        var url = window.location.href;
        if (window.name === 'CRMFrame') {
            return false;
        }
        if (sessionStorage["productId"] || url.indexOf("productId") > 0) {
            return false;
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
        var isPortal = window.top.$('iframe.screenIframe:visible');
        if (sg.utls.isSameOrigin() && isPortal) {
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

    /**
     * @name getFirefoxSpecialKeys
     * @description TODO Enter a description
     * @param {object} e - TODO Enter a description
     * @returns {array} TODO Enter a description
     */
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
     * @name isInternetExplorer
     * @description Gets a value indicating whether the current browser is Internet Explorer.
     * @returns {boolean} True if the browser is Internet Explorer, otherwise false. 
     */
    isInternetExplorer: function () {
        var ua = window.navigator.userAgent;
        var tridentFound = ua.indexOf('Trident/') > 0;
        return tridentFound;
    },

    /**
     * @name isMozillaFirefox
     * @description Gets a value indicating whether the current browser is Mozilla Firefox.
     * @returns {boolean} True if the browser is Mozilla Firefox, otherwise false. 
     */
    isMozillaFirefox: function () {
        var isMozilla = window.navigator.userAgent.indexOf('Firefox/') > 0;
        return isMozilla;
    },

    /**
     * @name isChrome
     * @description Gets a value indicating whether the current browser is Google Chrome.
     * @returns {boolean} True if the browser is Google Chrome, otherwise false. 
     */
    isChrome: function () {
        var _isChrome = navigator.userAgent.indexOf('Chrome') != -1;
        return _isChrome;
    },

    /**
     * @name isSafari
     * @description Gets a value indicating whether the current browser is Apple Safari.
     * @returns {boolean} True if the browser is Apple Safari, otherwise false. 
     */
    isSafari: function () {

        var _isSafari = (navigator.userAgent.indexOf('Safari') != -1
            && navigator.userAgent.indexOf('Chrome') == -1);
        return _isSafari;
    },

    /**
     * @name isMobile
     * @description Gets a value indicating whether the current browser is operating on a mobile device.
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
            $(window).on('unload', function () {
                sg.utls.destroySession();
            });
        }
    },

    releaseSession: function () {
        var sessionPerPage = $("#SessionPerPage");
        if (sessionPerPage.length === 0 || sessionPerPage.val() === "False") {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Session", "ReleaseSession"));
        }
    },

    destroySessions: function () {
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Session", "DestroyPool"));
        sage.cache.session.clearAll();
        sg.utls.destroyPoolForReport(false);
    },

    destroySession: function () {
        // Do not want to remove session from storage (will do this when page closes/logs out/etc.)
        var sessionId = sage.cache.session.get("session");
        sage.cache.session.clearAll();
        sage.cache.session.set("session", sessionId);
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Session", "Destroy"));
    },

    /**
     * @name userEviction
     * @description User is being evicted
     */
    userEviction: function () {
        var isAdminLogout = location.href.indexOf("AS/CustomScreen") > 0;

        var portalWnd = sg.utls.getPortalWindow();
        var sessionId = sg.utls.getSessionId();
        sg.utls.logMessage("Notifying user in sg.utls.userEviction() of eviction for context session id = '" + sessionId + "'");

        // We want to disable beforeunload event handling
        portalWnd.pageUnloadEventManager.disable();

        var logoutLink = sg.utls.url.logoutUrl();
        var evictUrl = sg.utls.url.evictUrl();
        if (isAdminLogout) {
            evictUrl += "?isAdminLogin=true";
        }
        // Note: Potential tech debt here as this event will get fired AFTER the login has been redirected to.
        // Therefore, checks have been placed in the AuthenticationController.Login method to check for this
        // use case where the data still exists in the IIS cache

        // Tmp solution for sync logout, as mentioned, there are some issue for this logOut function. 
        // Just ask the user to log out first due to time consuming
        
        if (!isAdminLogout) {

            // We need to reduce the number of active screens (or modules) 
            // from the overall count because we're in the process of
            // logging out.
            sg.utls.updateOpenScreenCount();
            if (!openScreenCountManager.anyActiveScreens()) {
                // There are currently no active screens running so let's
                // notify sibling tabs to enable their session date pickers
                key = "ALLSESSIONS_EnablePortalSessionDatePicker";
                var randomValue = sg.utls.makeRandomString(5);
                sage.cache.local.set(key, randomValue);
            }
        }

        sg.utls.destroyPoolForReport(true);
        sage.cache.session.clearAll();

        // Log out on server side, server side cleanup MUST happen before redirection or the login page 
        // will load with an incorrect hdnUrl, leading to problems if signing in again as a different user.
        sg.utls.ajaxPostWithPromise(logoutLink).done(function () {
            portalWnd.location.href = evictUrl;
        });

        sg.utls.initiateEvictInOtherTabsForSession(sessionId);
    },

    /**
     * @name logOut
     * @description Log the current user out
     * @param {boolean} isAdminLogout - Are we logging out of the administration page?
     */
	logOut: function (isAdminLogout) {

        var portalWnd = sg.utls.getPortalWindow();
        var globalRes = sg.utls.getPortalGlobalResource();

        var sessionId = sg.utls.getSessionId();
        sg.utls.logMessage("Calling sg.utls.logOut() for context session id = '" + sessionId + "'");

        var currentCompany = sg.utls.getCurrentCompanyName();
        var message = isAdminLogout ? globalRes.AdminSignOutConfirmation : sg.utls.formatString(globalRes.MultiSessionSignOutConfirmationTemplate, kendo.htmlEncode(currentCompany));
        var title = globalRes.SignOutConfirmation;
        var btnYes = globalRes.SignOut;
        var btnNo = globalRes.Cancel;

        var callbackYes = function () {

            // We want to disable beforeunload event handling
            portalWnd.pageUnloadEventManager.disable();

            sg.utls.logMessage("Log out confirmed for context session id = '" + sessionId + "'");

            isAdminLogout = typeof isAdminLogout !== 'undefined' ? isAdminLogout : false;

            var logoutLink = sg.utls.url.logoutUrl();
            var loginLink = sg.utls.url.loginUrl(isAdminLogout);

            // Note: Potential tech debt here as this event will get fired AFTER the login has been redirected to.
            // Therefore, checks have been placed in the AuthenticationController.Login method to check for this
            // use case where the data still exists in the IIS cache

            // Tmp solution for sync logout, as mentioned, there are some issue for this logOut function. 
            // Just ask the user log out first due to time consuming

            // Set the src attribute for all iframes (where screens live)
            // This will initiate the 'beforunload' event in each active screen/module
            //sg.utls.clearSrcAttributeFromAllActiveModuleIFrames();

            if (!isAdminLogout) {

                // We need to reduce the number of active screens (or modules) 
                // from the overall count because we're in the process of
                // logging out.
                sg.utls.updateOpenScreenCount();
                if (!openScreenCountManager.anyActiveScreens()) {
                    // There are currently no active screens running so let's
                    // notify sibling tabs to enable their session date pickers
                    key = "ALLSESSIONS_EnablePortalSessionDatePicker";
                    var randomValue = sg.utls.makeRandomString(5);
                    sage.cache.local.set(key, randomValue);
                }
            }

            sg.utls.destroyPoolForReport(true);
            sage.cache.session.clearAll();

            // Log out on server side, server side cleanup MUST happen before redirection or the login page 
            // will load with an incorrect hdnUrl, leading to problems if signing in again as a different user.
            sg.utls.ajaxPostWithPromise(logoutLink).done(function () {
                portalWnd.location.href = loginLink + "?logout=true"; // Redirect to login page on client side
            });

			sg.utls.initiateLogoutInOtherTabsForSession(sessionId);
        };

        var callbackNo = function () {
            // Just reset the flag back to it's default
            portalWnd.pageUnloadEventManager.enable();
            return;
        };

        sg.utls.showConfirmationDialogYesNo(callbackYes, callbackNo, message, title, btnYes, btnNo);
    },

    /**
     * @name initiateLogoutInOtherTabsForSession
     * @description Set a localStorage cache entry to facilitate log out
     *              for other tabs that share the same sessionId
     * @param {string} sessionId - The session id
     */
    initiateLogoutInOtherTabsForSession: function (sessionId) {
        var key = sessionId + "_LogoutInitiated";
        sage.cache.local.set(key, new Date().getTime().toString());
    },

    /**
     * @name initiateEvictInOtherTabsForSession
     * @description Set a localStorage cache entry to facilitate evict user
     *              for other tabs that share the same sessionId
     * @param {string} sessionId - The session id
     */
    initiateEvictInOtherTabsForSession: function (sessionId) {
        var key = sessionId + "_EvictInitiated";
        sage.cache.local.set(key, new Date().getTime().toString());
    },

    /**
     * @name clearSrcAttributeFromAllActiveModuleIFrames
     * @description Get the iframe ID's for all currently active screens
     *              from the Window Manager popup and set the src attribute
     *              for each to 'about:blank'
     */
    clearSrcAttributeFromAllActiveModuleIFrames: function() {
        $('#dvWindows').find("[command='Add']").each(function (index, element) {
            var currentIframeId = $(this).attr("frameId");
            $("#" + currentIframeId).attr("src", "about:blank");
        });
    },

    /**
     * @name updateOpenScreenCount
     * @description Ensure that the open screen count is reduced 
     *              (or eliminated) if the tab/browser is closed.
     */
    updateOpenScreenCount: function() {

        // Get the current active module count for this tab
        var openScreenCountForTab = openScreenCountManager.getCountForCurrentTab();

        // Get the overall active module count (all tabs)
        var overallCount = openScreenCountManager.get();

        // Overall active module count AFTER current tab is closed
        var updatedCount = overallCount - openScreenCountForTab;

        if(updatedCount > 0) {
            openScreenCountManager.set(updatedCount);
        } else {
            // There are no more open modules so we can remove the cached number
            openScreenCountManager.remove();
        }
    },

    /**
     * @name extractSessionidFromWindow
     * @description Extract the context session id from the url
     * @returns {string} The extracted context session id
     */
    extractSessionIdFromWindow: function () {

        var sessionId = "";
        var paths = sg.utls.getPortalWindow().location.pathname.split('/');

        // Get the 'SessionID', well it's actually User-Company to be more correct
        var index = paths.indexOf("OnPremise") + 1 || paths.indexOf("Core") - 1;
        sessionId = paths[index];
        return sessionId;
    },

    /**
     * @name getSessionId
     * @description Get the context session id from the hidden field
     * @returns {string} sessionId
     */
    getSessionId: function () {
        var sessionId = $("#ContextSessionId").val();
        return sessionId;
    },

    /**
     * @name getCurrentCompanyName
     * @description Get the current company name from the top level menu via jquery
     * @returns {string} The current company name
     */
    getCurrentCompanyName: function () {
        if (window.location !== window.parent.location) { // in iframe (within a page), go up one level
            return parent.sg.utls.getCurrentCompanyName();
        } 

        var companyName = "";
        $("#companyNameMenu").find('.txt-top-menu').each(function (index, element) {
            companyName = $(this).text();
        });

        return companyName.trim();
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

    loadCompanyColor: function () {
        sg.utls.ajaxCache(sg.utls.url.getCompanyColorUrl(), {}, $.noop, "CompanyColor");
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
        var reportUrl = kendo.format(sg.utls.url.buildUrl("Core", "ExportReport", "ExportDialog") + "?token={0}", reportToken);
        var reportWindow = window.open(reportUrl);
        if (sg.utls.isFunction(callbackOnClose)) {
            setTimeout(function () {
                if (reportWindow !== undefined) {
                    $(reportWindow).on("unload", function () {
                        callbackOnClose.call();
                    });
                }
            }, 500);
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
                $("#ajaxSpinner").css("display", "block");
                if (sg.utls.isSameOrigin()) {
                    var iFrame = window.top.$('iframe.screenIframe:visible');
                    if (iFrame && iFrame.length > 0) {
                        sg.utls.showMessagesInViewPort();
                    }
                }
            },
            complete: function () {
                $("#ajaxSpinner").css("display", "none");
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
     * @name SyncExecute
     * @description Invokes a specified callback method after a 5 ms delay, 
     *              to give any tab-out events that fire (which should happen 
     *              within roughly 1 ms, after which finders may be called) 
     *              time to complete their respective execution.
     * 
     * @param {function} callback The callback function to invoke.
     *
     */
    SyncExecute: function (callback) {
        var DELAY_MS = 5;
        setTimeout(function () {
            if (sg.utls.ajaxRunning === true) {
                sg.utls.functionsToCall.push(callback);
            } else {
                if (sg.utls.isFunction(callback)) {
                    callback();
                } 
            }
        }, DELAY_MS);
    },

    fireStackedCalls: function () {
        while (sg.utls.functionsToCall.length > 0) {
            var toCall = sg.utls.functionsToCall.pop();
            if (sg.utls.isFunction(toCall)) {
                toCall();
            }
        }
    },

    removeStackedCalls: function () {
        while (sg.utls.functionsToCall.length > 0) {
            var functionToCall = sg.utls.functionsToCall.pop();
        }
    },

    // TODO: This is ripped from TM and TS, should go through TM and TS and replace those calls with this
    // one in global instead of having copies in global, TM.Common and TS.Common. In the long term, we
    // should revisit all the ajax helper wrappers and consolidate/remove some of them. This one is more useful
    // than the old ajaxPost because it returns a promise
    ajaxPostWithPromise: function (url, data) {
        var dataJson = JSON.stringify(data);
        return $.ajaxq("SageQueue", {
            url: url,
            data: dataJson,
            type: "post",
            headers: sg.utls.getHeadersForAjax(),
            async: true,
            dataType: "json",
            contentType: "application/json",
            beforeSend: function () {
                $("#ajaxSpinner").fadeIn(1);
                sg.utls.showMessagesInViewPort();
            },

            error: function () {
                // If the url that led to the error was the logout url, redirect them anyways
                // i.e. if the browser cache is wiped
                if (url === sg.utls.url.logoutUrl()) {
                    var portalWnd = sg.utls.getPortalWindow();
                    var loginLink = sg.utls.url.loginUrl();
                    portalWnd.location.href = loginLink + "?logout=true"; // Redirect to login page on client side
                }
            },

            complete: function () {
                $("#ajaxSpinner").fadeOut(1);
            }
        }).then(sg.utls.sagePromise, sg.utls.sageAjaxError);
    },

    sagePromise: function (response) {
        var d = $.Deferred();
        var errors = true;
        var userMessage = response.UserMessage;
        if (userMessage) {
            if (userMessage.IsSuccess) {
                errors = false;
            }
        }
        else {
            errors = false;
        }
        errors ? d.reject(response) : d.resolve(response);
        return d;
    },

    sageAjaxError: function (ret) {
        //TODO redirection 
        sg.utls.showMessage(ret, $.noop, false, false);
    },

    getJsonResultHandler: function (externalHandler) {
        return function (result) {
            if (externalHandler && typeof externalHandler === "function") {
                // this is the only way to test if result can be convert to JSON
                var callbackValue = null;
                try {
                    callbackValue = JSON.parse(result);
                } catch(err) {
                    console.warn("Error parsing value: " + result + " to JSON");
                } finally {
                    externalHandler(callbackValue);
                }
            }
        };
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
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, sg.utls.getJsonResultHandler(successHandler), "text", "get", true, sg.utls.ajaxErrorHandler);
    },

    recursiveAjaxPost: function (ajaxUrl, ajaxData, successHandler, abortHandler) {
        return sg.utls.recursiveAjax(ajaxUrl, ajaxData, sg.utls.getJsonResultHandler(successHandler), abortHandler, "text", "post");
    },

    ajaxPost: function (ajaxUrl, ajaxData, successHandler) {
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, sg.utls.getJsonResultHandler(successHandler), "text", "post", true, sg.utls.ajaxErrorHandler);
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
        sg.utls.ajaxInternal(ajaxUrl, ajaxData, sg.utls.getJsonResultHandler(successHandler), "text", "post", false, sg.utls.ajaxErrorHandler);
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
        var headers = { ContextToken: $("#ContextToken").val(), ScreenName: $("#ScreenName").val(), product: sessionStorage.getItem("productId") };
        headers[tokenName] = sg.utls.getAntiForgeryToken();
        return headers;
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

    getFormattedDialogHtml: function(okButtonId, cancelButtonId) {
        return this.getFormatedDialogHtml(okButtonId, cancelButtonId);
    },

    // Spelling error.
    getFormatedDialogHtml: function (okButtonId, cancelButtonId) {

        let idOK = (typeof okButtonId !== 'undefined' && okButtonId !== null) ? okButtonId : "kendoConfirmationAcceptButton";
        let idCancel = (typeof cancelButtonId !== 'undefined' && cancelButtonId !== null) ? cancelButtonId : "kendoConfirmationCancelButton";
        let text = 
            `<div id="dialogConfirmation" class="modal-msg">
	            <div class="message-control multiWarn-msg"> 
		        <div class="title"> 
			        <span class="icon multiWarn-icon"></span> 
			        <h3 id="dialogConfirmation_header" />
		        </div>
		        <div class="msg-content">
			        <p id="dialogConfirmation_msg1"></p>
			        <p id="dialogConfirmation_msg2"></p> 
			        <div class="button-group">
				        <input class="btn btn-primary" id="${idOK}" type="button" value="OK" />
                        <input class="btn btn-secondary" id="${idCancel}" type="button" value="Cancel" />
			        </div> 
		        </div> 
	        </div> 
        </div>`;

        return text;
    },

    showMessageDialog: function (callbackYes, callbackNo, message, dialogType, title, dialoghtml, okButtonId, cancelButtonId, isSessionWarning) {

        var idOK = (typeof okButtonId !== 'undefined' && okButtonId !== null) ? "#" + okButtonId : "#kendoConfirmationAcceptButton";
        var idCancel = (typeof cancelButtonId !== 'undefined' && cancelButtonId !== null) ? "#" + cancelButtonId : "#kendoConfirmationCancelButton";

        if (dialoghtml === null || dialoghtml === undefined) {
            var kendoWindow = $("<div class='modelWindow' id='dialogConfirmation' />").kendoWindow({
                title: '',
                resizable: false,
                modal: true,
                minWidth: 500,
                maxWidth: 760,
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

            if (typeof isSessionWarning !== 'undefined' && isSessionWarning !== null && isSessionWarning === true) {
                kendoWindow.find("#dialogConfirmation_header").text(globalResource.SessionExpiredDialogHeader);
                kendoWindow.find("#dialogConfirmation_msg1").text(globalResource.SessionExpiredDialogMsg1);
                kendoWindow.find("#dialogConfirmation_msg2").text(globalResource.SessionExpiredDialogMsg2);
            }
            else {
                kendoWindow.find("#dialogConfirmation_header").text(title);
                kendoWindow.find("#dialogConfirmation_msg1").text(message);
            }

            var yesBinderArray = ["msgCtrl-close", "btn-primary"];
            var noBinderArray = ["btn-secondary"];
        }

        switch (dialogType) {
            case sg.utls.DialogBoxType.YesNo:
                $(idOK).attr('value', globalResource.Yes)
                $(idCancel).attr('value', globalResource.No);
                break;
            case sg.utls.DialogBoxType.OKCancel:
                $(idOK).attr('value', globalResource.OK);
                $(idCancel).attr('value', globalResource.Cancel);
                break;
            case sg.utls.DialogBoxType.OK:
                defaultTitle = globalResource.Info;
                $(idOK).attr('value', globalResource.OK);
                $(idCancel).hide();
                break;
            case sg.utls.DialogBoxType.Close:
                defaultTitle = globalResource.Error;
                $(idOK).hide();
                $(idCancel).attr('value', globalResource.Close);
                break;
            case sg.utls.DialogBoxType.DeleteCancel:
                $(idOK).attr('value', globalResource.Delete);
                $(idCanel).attr('value', globalResource.Cancel);
                break;
            case sg.utls.DialogBoxType.Continue:
                kendoWindow.find("#dialogConfirmation_header").text(title);
                kendoWindow.find("#dialogConfirmation_msg1").text(message);
                $(idOK).hide();
                $(idCancel).val(globalResource.Continue);
                break;
        }
        title = title || defaultTitle;
        kendoWindow.find("#title-text").text(title);

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

    showKendoConfirmationDialog: function (callbackYes, callbackNo, message, typeOfAction, isMessageEncoded, callbackCancel, gridName = '') {

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
            minWidth: 500,
            maxWidth: 760,
            open: function () {
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },
            // Custom function to support focus within kendo window
            activate: (e) => {
                sg.utls.kndoUI.onActivate(e);

                // Set the initial control focus to 'Yes' or 'Ok' button
                // Note: Seem to need small delay before this works correctly.
                let $focusButton = $('#kendoConfirmationAcceptButton');
                if ($focusButton) {
                    const delay = 500;
                    setTimeout(() => {
                        $focusButton.focus();
                    }, delay);
                }
            }
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

            // If gridName has been specified, set focus back to grid
            if (gridName.length > 0) {
                sg.utls.gridFocusByName(gridName);
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
                try {
                    kendoWindow.data("kendoWindow").destroy();
                } catch (err) {
                    //don't do anything. This means window is already destroyed or does not exists.
                    //This is required for iframes
                }
                if ($(this).hasClass("delete-confirm")) {
                
                    if (callbackYes != null) {
                        callbackYes();
                    }

                } else if ($(this).hasClass("delete-cancel")) {
                    if (callbackNo != null)
                        callbackNo();
                    
                }
                else if ($(this).hasClass("delete-cancelled")) {
                    if (callbackCancel) {
                        callbackCancel();
                    }
                }

                // If gridName has been specified, set focus back to grid
                if (gridName.length > 0) {
                    sg.utls.gridFocusByName(gridName);
                }
            }).end();

        kendoWindow.parent().addClass('modelBox');
        kendoWindow.parent().attr('id', 'deleteConfirmationParent');
        var divDeleteConfirmParent = $('#deleteConfirmationParent');

        // Removed the line below because if modal is true, the z-index value increasing 
        // automatically. We cannot guarantee 999999 is the largest value on the screen.

        // There is a defect in the behaviour of Kendo Window,
        // when a modal is opened on another modal, the z-index is not calculated correctly.
        divDeleteConfirmParent.css('z-index', '999999');

        divDeleteConfirmParent.css('position', 'absolute');
        divDeleteConfirmParent.css('left', ($(window).width() - divDeleteConfirmParent.width()) / 2);

        if (positionDialogTitleInHeader) {
            divDeleteConfirmParent.find('#' + dialogId + '_wnd_title').html(title);
        }

        // Setting message position to viewport top.
        sg.utls.showMessagesInViewPort();
    },

    showCommonConfirmationDialog: function (id, callbackYes, callbackNo, message) {
        var dialogId = 'div_' + id + 'confirm_dialog';
        $('<div  class="modelWindow" id="' + dialogId + '" ></div>').appendTo('body');

        var kendoWindow = $('<div class="modelWindow" id="' + dialogId + '" ></div>').kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 500,
            maxWidth: 760,
            open: function () {
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },
            //custom function to suppot focus within kendo window
            activate: sg.utls.kndoUI.onActivate,
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
    showConfirmationDialogYesNo: function (callbackYes, callbackNo, messageIn, titleIn, btnYesLabelIn, btnNoLabelIn, noPostFix = false) {

        var DialogID = 'confirmationDialog';
        var DialogParentID = 'confirmationParent';
        var InpageTemplateIDRoot = 'generic-confirmation';
        var randomPostfix = noPostFix ? '' : sg.utls.makeRandomString(5);
        var InpageTemplateID = InpageTemplateIDRoot + randomPostfix;

        messageIn = sg.utls.htmlEncode(messageIn);
        titleIn = sg.utls.htmlEncode(titleIn);

        var template = 
            `<script id="${InpageTemplateID}" type="text/x-kendo-template">
                <div class="fild_set">
                    <div class="fild-title generic-message" id="gen-message${randomPostfix}">
                        <div id="title-text${randomPostfix}"></div>
                    </div>
                    <div class="fild-content">
                        <div id="body-text${randomPostfix}"></div>
                        <div class="modelBox_controlls">
                            <input type="button" class="btn btn-secondary generic-cancel" id="kendoConfirmationCancelButton${randomPostfix}" value="@CommonResx.No" />
                            <input type="button" class="btn btn-primary generic-confirm" id="kendoConfirmationAcceptButton${randomPostfix}" value="@CommonResx.Yes" />
                        </div>
                    </div>
                </div>
            </script>`;

        $(template).appendTo('body');

        // This kendoWindow visibility check is added for the defect D-07638
        var wnd = $('#' + DialogID).data("kendoWindow");
        if (wnd != null && !wnd.element.is(":hidden")) {
            return;
        }

        var kendoWindow = $("<div class='modelWindow' id='" + DialogID + "' />").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 600,
            maxWidth: 960,
            open: function () {
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },
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
        var msg = messageIn.replace(/\n/g, '<br/>');
        kendoWindow.find("#body-text" + randomPostfix).html(msg);

        _setButtonLabels();

        _setButtonCallbacks();

        // Little x button gets shifted upwards when
        // following line is enabled. Looks funny.
        //kendoWindow.parent().addClass('modelBox');

        kendoWindow.parent().attr('id', DialogParentID);
        var $divConfirmParent = $('#' + DialogParentID);
        $divConfirmParent.css('position', 'absolute');
        $divConfirmParent.css('left', ($(window).width() - $divConfirmParent.width()) / 2);

        _setDialogTitle();

        // Set message position to viewport top.
        sg.utls.showMessagesInViewPort2(DialogParentID);

        function _setDialogTitle() {
            var title;
            if (titleIn && titleIn.length > 0) {
                title = titleIn;
            } else {
                title = globalResource.ConfirmationTitle;
            }

            // Set the modal dialog caption (title) text
            $divConfirmParent.find('#' + DialogID + '_wnd_title').html(title);
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

            kendoWindow.find("#kendoConfirmationAcceptButton" + randomPostfix).val($("<div>").html(yesLabel).text());
            kendoWindow.find("#kendoConfirmationCancelButton" + randomPostfix).val($("<div>").html(noLabel).text());
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
     * @name showConfirmationDialogYesNoCancel
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
     * @param {object} callbackCancel - Callback when 'Cancel' selected
     * @param {string} messageIn - The message to display
     * @param {string} titleIn - Optional - The dialog title to display
     * @param {string} btnYesLabelIn - Optional - The text for the 'Yes' button
     * @param {string} btnNoLabelIn - Optional - The text for the 'No' button
     * @param {string} btnCancelLabelIn - Optional - The text for the 'Cancel' button
     * @param {boolean} noPostFix - Optional - an random Postfix
     */
    showConfirmationDialogYesNoCancel: function (callbackYes, callbackNo, callbackCancel, messageIn, titleIn, btnYesLabelIn, btnNoLabelIn, btnCancelLabelIn,noPostFix = false) {

        var DialogID = 'confirmationDialogYesNoCancel';
        var DialogParentID = 'confirmationParentYesNoCancel';
        var InpageTemplateIDRoot = 'generic-confirmationYesNoCancel';
        var randomPostfix = noPostFix ? '' : sg.utls.makeRandomString(5);
        var InpageTemplateID = InpageTemplateIDRoot + randomPostfix;

        messageIn = sg.utls.htmlEncode(messageIn);
        titleIn = sg.utls.htmlEncode(titleIn);

        var template =
            `<script id="${InpageTemplateID}" type="text/x-kendo-template">
                <div class="fild_set">
                    <div class="fild-title generic-message" id="gen-message${randomPostfix}">
                        <div id="title-text${randomPostfix}"></div>
                    </div>
                    <div class="fild-content">
                        <div id="body-text${randomPostfix}"></div>
                        <div class="modelBox_controlls">
                            <input type="button" class="btn btn-secondary generic-cancel" id="kendoConfirmationCancelButton${randomPostfix}" value="@CommonResx.Cancel" />
                            <input type="button" class="btn btn-primary generic-no" id="kendoConfirmationNoButton${randomPostfix}" value="@CommonResx.No" />
                            <input type="button" class="btn btn-primary generic-confirm" id="kendoConfirmationAcceptButton${randomPostfix}" value="@CommonResx.Yes" />
                        </div>
                    </div>
                </div>
            </script>`;

        $(template).appendTo('body');

        // This kendoWindow visibility check is added for the defect D-07638
        var wnd = $('#' + DialogID).data("kendoWindow");
        if (wnd != null && !wnd.element.is(":hidden")) {
            return;
        }

        var kendoWindow = $("<div class='modelWindow' id='" + DialogID + "' />").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 600,
            maxWidth: 960,
            open: function () {
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },
            // custom function to support focus within kendo window
            activate: sg.utls.kndoUI.onActivate
        });

        kendoWindow.data("kendoWindow").content($("#" + InpageTemplateID).html()).center().open();

        kendoWindow.data("kendoWindow").bind("close", function () {
            kendoWindow.data("kendoWindow").destroy();
            if (callbackCancel != null) {
                callbackCancel();
            }
        });

        // Set the message text
        var msg = messageIn.replace(/\n/g, '<br/>');
        kendoWindow.find("#body-text" + randomPostfix).html(msg);

        _setButtonLabels();

        _setButtonCallbacks();

        // Little x button gets shifted upwards when
        // following line is enabled. Looks funny.
        //kendoWindow.parent().addClass('modelBox');

        kendoWindow.parent().attr('id', DialogParentID);
        var $divConfirmParent = $('#' + DialogParentID);
        $divConfirmParent.css('position', 'absolute');
        $divConfirmParent.css('left', ($(window).width() - $divConfirmParent.width()) / 2);

        _setDialogTitle();

        // Set message position to viewport top.
        sg.utls.showMessagesInViewPort2(DialogParentID);

        function _setDialogTitle() {
            var title;
            if (titleIn && titleIn.length > 0) {
                title = titleIn;
            } else {
                title = globalResource.ConfirmationTitle;
            }

            // Set the modal dialog caption (title) text
            $divConfirmParent.find('#' + DialogID + '_wnd_title').html(title);
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

            var cancelLabel;
            if (btnCancelLabelIn && btnCancelLabelIn.length > 0) {
                cancelLabel = btnCancelLabelIn;
            } else {
                cancelLabel = globalResource.Cancel;
            }

            kendoWindow.find("#kendoConfirmationAcceptButton" + randomPostfix).val($("<div>").html(yesLabel).text());
            kendoWindow.find("#kendoConfirmationCancelButton" + randomPostfix).val($("<div>").html(cancelLabel).text());
            kendoWindow.find("#kendoConfirmationNoButton" + randomPostfix).val($("<div>").html(noLabel).text());
        }

        function _setButtonCallbacks() {
            kendoWindow.find(".generic-confirm, .generic-cancel, .generic-no")
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
                    } else if ($(this).hasClass("generic-no")) {
                        if (callbackNo != null)
                            callbackNo();
                        kendoWindow.data("kendoWindow").destroy();
                    }
                    else if ($(this).hasClass("generic-cancel")) {
                        if (callbackCancel != null)
                            callbackCancel();
                        kendoWindow.data("kendoWindow").destroy();
                    }
                }).end();
        }
    },

    /**
     * @name showMessagesInViewPort2
     * @desc Show all messages in viewport area
     * @private
     * @param {string} parentDialogID - The id of the parent dialog
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
     * @param {number} len - The string length to return
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
        var kendoWindow = $("<div class='modelWindow' id='" + "messageDialog " + "' ></div>").kendoWindow({
            title: '',
            resizable: false,
            modal: true,
            minWidth: 500,
            maxWidth: 760,
            open: function () {
                // For custom theme color
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },            //custom function to suppot focus within kendo window
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
     * Creates a unique GUID; intended for use when opening multiple kendo iframe windows, 
     * where the url should be unique.
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
     * @function
     * @name initializeKendoWindowPopup
     * @description Initializes a Kendo popup window
     * @namespace sg.utls
     * @public
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     * @param {function} maxConfig popup window width
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
            draggable: true,
            scrollable: true,
            visible: false,
            width: width,
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
                sg.utls.mobileKendoAdjustment(this.element, ".k-window-content", "popupMobile");
                sg.utls.setBackgroundColor($(this.element[0].previousElementSibling));
            },
        }).data("kendoWindow");
        if (maxConfig && maxConfig.height) {
            kendoWindow.setOptions({ height: maxConfig.height });
        }
    },

    /**
     * @name mobileKendoAdjustment
     * @description Positioning a Kendo popup window to the top in Mobile/iPad (Fix for D-39462).
     * @param {any} element TODO - Add description
     * @param {string} contentDiv find the CSS id of the window's content div.
     * @param {string} kendowWindowId Add the value for the window's id attribute.
     */
    mobileKendoAdjustment: function (element, contentDiv, kendowWindowId) {
        if (sg.utls.isMobile()) {
            element.closest(contentDiv).parent().attr('id', kendowWindowId);
            $(window.top).scrollTop(0);
        }
    },

    /**
     * Initializes a Kendo popup window with minimize/maximize option.
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     * @param {number|string} width Specifies width of the popup.
     * @param {number|string} height Specifies height of the popup.
     */
    initializeKendoWindowPopupWithMaximize: function(id, title, onClose, width, height) {
        var config = {
            actions: ["Maximize", "Close"],
            width: width,
            height: height
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

    /**
     * Initializes a Kendo popup window with specify width parameter. 
     * 
     * @deprecated Name is misspelled!
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     * @param {function} width popup window width
     *
     */
    intializeKendoWindowPopupWithWidth: function (id, title, onClose, width) {
        this.initializeKendoWindowPopup(id, title, onClose, {width: width});
    },

    /**
     * @function
     * @name initializeKendoWindowPopupWithWidth
     * @description Initializes a Kendo popup window with a specified width parameter. 
     * @namespace sg.utls
     * @public
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {string} title The value for the window's title.
     * @param {function} onClose Handler for the popup's close event.
     * @param {function} width popup window width
     */
    initializeKendoWindowPopupWithWidth: function (id, title, onClose, width) {
        this.initializeKendoWindowPopup(id, title, onClose, { width: width });
    },

    /**
     * @function
     * @name openKendoWindowPopup
     * @description 
     * @namespace sg.utls
     * @public
     * 
     * @param {string} id The value for the window's CSS id attribute.
     * @param {object} data 
     * @param {number} defaultWidth
     */
     openKendoWindowPopup: function (id, data, defaultWidth, height) {
        $(id + " .menu-with-submenu").remove();    // to remove Text sizing option, this is because the popup is not from iFrame ...

        var kendoWindow = $(id).data("kendoWindow");
        if (!kendoWindow) {
            console.log('Sage.CA.SBS.ERP.Sage300.Common.global.js -> openKendoWindowPopup() -> kendoWindow has not been initialized.')
        }

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
        let hasVerticalscroll = $(".k-window-content:visible").hasScroll('y');
        if (hasVerticalscroll && defaultWidth && defaultWidth != null) {
            $('.k-window-content:visible').css("width", defaultWidth + "px");
        }
        else if (hasVerticalscroll && defaultWidth == null) {
            $('.k-window-content:visible').css("width", "960px");  //remove horizondal scrollbar of the popup windows
            //console.log("scroll true");
        }
        else if (height) {
            $('.k-window-content:visible').css("height", height + "px");
        } else {
            $('.k-window-content:visible').css("width", "auto");
            // console.log("scroll false");
        }

        var menuLink = $(".dropDown-Menu > li");
        menuLink.find("> a").append('<span class="arrow-grey"></span>');
        menuLink.on("mouseenter", function () {
            $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
            $(this).children(".sub-menu").show();
        }).on("mouseleave", function () {
            $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
            $(this).children(".sub-menu").hide();
        });

         return kendoWindow;
    },

    closeKendoWindowPopup: function (id, data) {
        var kendoWindow = $(id).data("kendoWindow");
        kendoWindow.close();
    },

    isKendoWindowPopupOpen: (id) => {
        let kendoWindow = $(id).data("kendoWindow");
        return kendoWindow ? !kendoWindow.element.is(":hidden") : false;
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

            var errMsgKeyHandler = function (e) {
                if (e.keyCode === sg.constants.KeyCodeEnum.ESC) {
                    $(".msgCtrl-close").trigger("click");
                    $(document).off("keyup keydown", errMsgKeyHandler);
                } else if ($('#injectedOverlay').length !== 0) {
                    return false;
                }
            };

            if ($("#message").length && $("#message").html().length > 0) {
                if (!(isModal === false)) {
                    if (isModalTransparent === true && $('#injectedOverlayTransparent').length === 0) {
                        //Inject an overlay that is transparent. 
                        messageDiv.parent().append("<div id='injectedOverlayTransparent' class='k-overlay k-overlay-transparent' ></div>");
                        $('#injectedOverlayTransparent').css('z-index', '999998');
                        $(document).on("keyup keydown", errMsgKeyHandler);
                    }
                    else if ($('#injectedOverlay').length === 0){
                        //Inject an overlay that is semi-opaque. 
                        messageDiv.parent().append("<div id='injectedOverlay' class='k-overlay'></div>");
                        $('#injectedOverlay').css('z-index', '999998');
                        $(document).on("keyup keydown", errMsgKeyHandler);
                    }
                }
            }

            if (handler !== undefined && handler !== null) {
                var closeHandler = function () {
                    handler();
                    $(document).off("click", ".msgCtrl-close", closeHandler);
                    $(document).off("keyup keydown", errMsgKeyHandler);
                };
                $(document).on("click", ".msgCtrl-close", closeHandler);
            }
        }
    },

    // showMessageInEnumerableResponse : To show any messages within EnumerableResponse<T>. 
    // This method does not contain any UserMessage
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

    /**
     * Hide Message dialog
     * */
    hideMessage: () =>{
        $("#message").hide();
        $("#message").empty();

        //Remove injected overlay if exists
        $("#injectedOverlay").remove();
        $("#injectedOverlayTransparent").remove();
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

    showMessagePopupInfo: function (messageType, message, div, handler) {
        sg.utls.showMessageInfoInCustomDiv(messageType, message, div, handler);
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
			
					
           if ($('#injectedOverlay').length === 0){
				//Inject an overlay that is semi-opaque. 
				$("#message").parent().append("<div id='injectedOverlay' class='k-overlay'></div>");
				$('#injectedOverlay').css('z-index', '999998');
			}
        }
    },

    // Converts an array to html list
    // isJSArray is default to false to handle objects in c# viewmodel format; 
    // isJSArray is set to true when handling a javascript array
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

    showMessageInfo: function (messageType, message, handler) {
        sg.utls.showMessageInfoInCustomDiv(messageType, message, "message", handler);
    },

    /**
   * Date format for Declarative Framework Reports
   * @param {any} date date to be formatted
   * @param {string} format date format 
   */
    formatDate: function (date, format) {
        if (format === undefined) format = 'MM/dd/yyyy';
        return kendo.toString(new Date(date), format);
    },

    /**
     * Show the message with costom html document
     * @param {any} htmlMsg HTML document
     */
    showMessageWithCustomHtml: function (htmlMsg) {
        const messageHTML = htmlMsg;
        const messageDivId = "#message";
        var messageDiv = $(messageDivId);
        messageDiv.html(messageHTML);

        /// Setting message position to viewport top.
        sg.utls.showMessagesInViewPort();

        messageDiv.show();

        if ($('#injectedOverlay').length === 0) {
            //Inject an overlay that is semi-opaque. 
            messageDiv.parent().append("<div id='injectedOverlay' class='k-overlay'></div>");
            $('#injectedOverlay').css('z-index', '999998');
        }
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

    showMessageInfoInCustomDiv: function (messageType, message, divId, handler) {
        var messageHTML = "";
        var messageDivId = "#" + divId;
        var messageDiv = $(messageDivId);
        var css = "message-control";

        //Use generateList() to handle multiple errors scenario
        var encodedMessage = Array.isArray(message) ? sg.utls.generateList(message, null, true) : sg.utls.htmlEncode(message);

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

        var infoMsgKeyHandler = function (e) {
            if (e.keyCode === sg.constants.KeyCodeEnum.ESC) {
                $(".msgCtrl-close").trigger("click");
                $(document).off("keyup keydown", infoMsgKeyHandler);
            } else if ($('#injectedOverlay').length !== 0) {
                return false;
            }
        };

        if ($('#injectedOverlay').length === 0) {
			//Inject an overlay that is semi-opaque. 
            messageDiv.parent().append("<div id='injectedOverlay' class='k-overlay'></div>");
            $('#injectedOverlay').css('z-index', '999998');
            $(document).on("keyup keydown", infoMsgKeyHandler);
        }

        // Added callback to perform action when message box is closed
        if (handler !== undefined && handler !== null) {
            var closeHandler = function () {
                handler();
                $(document).off("click", ".msgCtrl-close", closeHandler);
                $(document).off("keyup keydown", infoMsgKeyHandler);
            };
            $(document).on("click", ".msgCtrl-close", closeHandler);
        }
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
        var element = $("#" + id);
        element.Finder({
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

        sg.utls.registerFinderHotkey(element, id);
    },

    getFindersListKeyByValue: function (value) {
        return Object.keys(sg.utls.findersList).find(key => sg.utls.findersList[key] === value);
    },

    registerFinderHotkey: function (element, finderId) {
        if (element[0]) {
            var textbox = $(element[0].parentElement).find("input:text")[0];
            if (!textbox && element[0].parentElement) {
                textbox = $(element[0].parentElement.parentElement).find("input:text")[0];
            }
            if (!textbox && element[0].parentElement.parentElement) {
                textbox = $(element[0].parentElement.parentElement.parentElement).find("input:text")[0];
            }
            if (textbox && !sg.utls.findersList[textbox.id]) {
                //Kendo numeric textbox has two html input elements and only the second one contains the id
                if (!textbox.id) {
                    textbox = textbox.nextSibling;
                }
                sg.utls.findersList[textbox.id] = finderId;
            }
        }
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

    /**
    * SSN number mask generation
    * @param {string} selector fieldname
    * @param {string} type either "UP" or "CP" expected
    */
    maskSSNNo: function (selector, type) {
        if (type === "UP") {
            $(selector).mask("AAA-AA-AAAA", {
                'translation': {
                    A: { pattern: /[0-9\*]/ }
                }
            });
        } else {
            $(selector).mask("AAA-AAA-AAA", {
                'translation': {
                    A: { pattern: /[0-9\*]/ }
                }
            });
        }
    },

    /**
    * SSN number formatting for US or CDN
    * @param {string} fieldname name of the field to format
    * @param {string} type either "UP" or "CP" expected
    */
    maskSSNNum: function (fieldname, type) {
        var field = "#" + fieldname;
        sg.utls.unmask(field);
        if (type === "UP")
            sg.utls.addPlaceHolder(field, "   -  -");
        else
            sg.utls.addPlaceHolder(field, "   -   -");
        sg.utls.addMaxLength(field, "11");
        sg.utls.maskSSNNo(field, type);
    },

    /**
    * ZIP code mask generation
    * @param {string} selector fieldname
    * @param {string} type "UP" or "CP" expected (not guarrantied)
    */
    maskZIPCo: function (selector, type) {
        if (type === "UP") {
            $(selector).mask("AAAAA-AAAA", {
                'translation': {
                    A: { pattern: /[0-9]/ }
                }
            });
        } else if (type === "CP") {
            $(selector).mask("BAB ABA", {
                'translation': {
                    A: { pattern: /[0-9]/ },
                    B: { pattern: /[A-Za-z]/ }
                },
                onKeyPress: function (value, event) {
                    event.currentTarget.value = value.toUpperCase();
                }
            });
        } else {
            $(selector).mask("AAAAAAAAAAAAAAAAAAAA", {
                'translation': {
                    A: { pattern: /./ }
                }
            });
        }
    },

    /**
    * ZIP Code formatting
    * @param {string} fieldname name of the field to format
    * @param {string} type "UP" or "CP" expected (not guarrantied)
    */
    maskZIPCode: function (fieldname, type) {
        var field = "#" + fieldname;
        sg.utls.unmask(field);
        if (type === "UP") {
            sg.utls.addPlaceHolder(field, "     -");
            sg.utls.addMaxLength(field, "10");
        } else if (type === "CP") {
            sg.utls.addPlaceHolder(field, "       ");
            sg.utls.addMaxLength(field, "7");
        } else {
            sg.utls.addPlaceHolder(field, "                    ");
            sg.utls.addMaxLength(field, "20");
        }
        sg.utls.maskZIPCo(field, type);
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

    getTimeValue: function (value) {
        return value.length > 12 ? value.substring(11) : "";
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

    // Init numeric textbox
    initNumericTextBox: function (id, decimals, spinners, step, maxDigits, minValue, maxValue) {
        $("#" + id).kendoNumericTextBox({
            format: "n" + decimals,
            spinners: spinners,
            step: step,
            decimals: decimals
        }).data("kendoNumericTextBox");

        // Get newly created config
        var numericTextbox = $("#" + id).data("kendoNumericTextBox");
        
        // Use or generate minimum value
        if (minValue === undefined) {
            minValue = sg.utls.getMinValue("0", decimals, maxDigits, decimals > 0);
        }
        // Use or generate maximum value
        if (maxValue === undefined) {
            maxValue = sg.utls.getMaxValue("9", decimals, maxDigits, decimals > 0);
        }

        numericTextbox.options.min = minValue;
        numericTextbox.options.max = maxValue;
        numericTextbox.options.upArrowText = globalResource.Next;
        numericTextbox.options.downArrowText = globalResource.Previous;
    },

    // Set numeric textbox
    setNumericTextBox: function (id) {
        var numericTextbox = $("#" + id).data("kendoNumericTextBox");
        if (numericTextbox !== undefined) {
            numericTextbox.value($("#" + id).val());
        }
    },

    formatPhoneNumber: function (value, isApplyFormat) {
        if (isApplyFormat) {
            return "(" + value.substr(0, 3) + ") " + value.substr(3, 3) + "-" + value.substr(6);
        } else
            return value;
    },

    // This function is added to apply phone and fax in a grid. 
    // This is used as a template grid column.
    formatPhoneNumberForGrid: function (value, isApplyFormat) {
        if (value) {
            if (isApplyFormat) {
                return "(" + value.substr(0, 3) + ") " + value.substr(3, 3) + "-" + value.substr(6);
            } else
                return value;
        }
        return "(         )    -";
    },

    // Made a format function that takes either a collection or an array as arguments, below are example usuage.
    // format("i can speak {language} since i was {age}",{language:'javascript',age:10});
    // format("i can speak {0} since i was {1}",'javascript',10});
    formatString: function (str, col) {
        col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

        return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
            if (m == "{{") { return "{"; }
            if (m == "}}") { return "}"; }
            return col[n];
        });
    },

    // This method enables or disables kendoNumericTextbox based on id and boolean value
    enableNumericTextbox: function (id, enablement) {
        if ($("#" + id).data("kendoNumericTextBox") != undefined) {
            $("#" + id).data("kendoNumericTextBox").enable(enablement);
        }
    },

    // This method enables or disables Kendo Dropdown based on id and boolean value
    enableKendoDropdown: function (id, enablement) {
        if ($("#" + id).data("kendoDropDownList") != undefined) {
            $("#" + id).data("kendoDropDownList").enable(enablement);
        }
    },

    // Show all messages in viewport area
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
     * @name labelMenuParams
     * @description Parameters to initialize the hamburger menu items.
     * @param {any} Id - Id of the menu.
     * @param {any} Value - Display text of the menu.
     * @param {function} callback - click event of the menu.
     * @param {object} koAttributes - ko attributes of the menu.
     * @returns {object} TODO - Add Description Here
     */
    labelMenuParams: function (Id, Value, callback, koAttributes) {
        var obj = { Id: Id, Value: Value, callback: callback };

        if (koAttributes) {
            obj.koAttributes = koAttributes;
        }

        return obj;
    },

    // Function to return the encoded value. 
    textEncode: function (value) {
        value = sg.controls.GetString(value);
        return encodeURIComponent(value);
    },

    // Function to encode the query string values from the URL
    urlEncode: function (url) {
       return sg.utls.url.urlEncode(url);
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
        window.location.replace(sg.utls.url.buildUrl("Core", "Authentication",
            (typeof customScreenUI !== "undefined" && customScreenUI.adminLogin) ? "AdminSessionExpired" : "SessionExpired"));
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

    // Note: When changing the parameters, make sure to change the createInquiryURLWithParameters 
    // function under TaskDock- Menu - BreadCrumb.js as well
    getInquiryParameterData: function (url, module, feature, target, value, title) {
        return sg.utls.formatString("{\"url\":\"{0}\",\"module\":\"{1}\",\"feature\":\"{2}\",\"target\":\"{3}\", \"value\":\"{4}\", \"title\":\"{5}\"}", url, module, feature, target, value, title);
    },

    getUrlPath: function (url) {
        var parser = document.createElement('a');
        parser.href = url;

        return parser.pathname;
    },

    saveUserPreferences: function (key, value) {
        var data = { key: key, value: value };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "SaveUserPreference"), data, () => { });
    },

    saveScreenLevelUserPreferences: function (key, value) {
        var data = { key: key, value: value };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "SaveScreenLevelUserPreference"), data, () => { });
    },

    deleteScreenLevelUserPreference: function(){
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "DeleteScreenLevelUserPreference"), {}, () => { });
    },

    getUserPreferences: function (key, successHandler) {
        var data = { key: key };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("Core", "Common", "GetUserPreference"), data, successHandler);
    },
    
    /**
     * @name logMessage
     * @description Log a message to the Sage 300 Webscreen server log file
     * @param {string} message - The message to log to the server
     */
    logMessage: function (message) {
        var data = {
            Message: message,
            Url: "",
            Line: ""
        };

        var url = sg.utls.url.logJavascriptMessageUrl();
        sg.utls.ajaxPost(url, data, $.noop);
    },

    /**
     * @name getPortalWindow
     * @description Get a reference to the top-level portal window
     *              Parent window for all iframes
     * @returns {object} Reference to the top-level portal window
     */
    getPortalWindow: function () {
        var portalWnd = window.top ? window.top : window;
        return portalWnd;
    },

    /**
     * @name getPortalGlobalResource
     * @description Get a reference to the top-level globalResource javascript object
     * @returns {object} Reference to the top-level globalResource javascript object
     */
    getPortalGlobalResource: function () {
        var portalWnd = sg.utls.getPortalWindow();
        return portalWnd.globalResource;
    },

    /**
    /**
     * @name isFunction
     * @description Determine if the supplied object is a valid javascript function
     * @param {function} o - The object to check
     * @returns {boolean} true : object is a function | false : object is not a function
     */
    isFunction: function (o) {
        return o && typeof o === "function";
    },

    /**
     * @name tabKeyHandler
     * @description Handler for when tab key is pressed
     * @param {object} e - Event arguments
     */
    tabKeyHandler: function(e) {
        var code = e.keyCode || e.which;
        if(code === sg.constants.KeyCodeEnum.Tab) {
            sg.utls.hideNotesCenter();
        }
    },

    /**
     * @name isPageUnloadEventEnabled
     * @description Get the Page unload event flag
     * @param {boolean} isDirty - Dirty flag(s) from behaviour file
     * @returns {boolean} true - Allow page unload event | false - Disallow page unload event
     */
    isPageUnloadEventEnabled: function (isDirty) {
        //For CRM Sage 300 pages, always return false 
        var url = window.location.href;
        if (sessionStorage["productId"] || url.indexOf("productId") > 0) {
            return false;
        }
        var pageUnloadEventFlag = sg.utls.getPortalWindow().pageUnloadEventManager.isEnabled();
        return pageUnloadEventFlag && isDirty;
    },

    /**
     * @name getDirtyMessage
     * @description Build and return a string that represents the default dirty text
     * @param {string} title - The title of the message
     * @returns {string} The full message text
     */
    getDirtyMessage: function (title) {
        var globalRes = sg.utls.getPortalGlobalResource();
        var text = $.validator.format(globalRes.SaveConfirm2, title);
        return sg.utls.buildMessageText(text);
    },

    /**
     * @name buildMessageText
     * @description Build a message
     * @param {string} text - The message text to use
     * @returns {string} The message text
     */
    buildMessageText: function (text) {
        return $('<div />').html(text).text();
    },
    /**
     * @name setBackgroundColor
     * @description Set the background color from user preference for the header section,
     *   also change the font color depending on the brightness
     * @param {object} element - The html object to be set color with
     */
    setBackgroundColor: function (element) {
        if (element && $.isFunction(element.css) && element.css("background-color")) {
            sg.utls.ajaxCache(sg.utls.url.getCompanyColorUrl(), {}, function (result) {
                sg.utls.setBackgroundColorHex(element, result);
            }, "CompanyColor");
        }
    },
    /**
     * @name setBackgroundColorHex
     * @description Set the background color from user preference for the header section,
     *   also change the font color depending on the brightness
     * @param {object} element - The html object to be set color with
     * @param {string} color - A Hex color code
     */
    setBackgroundColorHex: function (element, color) {
        if (element && color) {
            element.css("background-color", color);
            var hexcolor = color.replace("#", "");
            var r = parseInt(hexcolor.substr(0, 2), 16);
            var g = parseInt(hexcolor.substr(2, 2), 16);
            var b = parseInt(hexcolor.substr(4, 2), 16);

            // https://www.w3.org/TR/AERT/#color-contrast
            var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;

            if (yiq > 125) {
                // set font to black
                element.removeClass("dark-bg");
                element.addClass("light-bg");
            } else {
                //set font to white
                element.removeClass("light-bg");
                element.addClass("dark-bg");
            }
        }
    },
    /**
     * @name getMenuLabelFromMenuItemId
     * @description Get the current menu item text based on it's menu item id
     * @param {string} menuItemId - The menuItemId
     * @returns {string} The menu item label text
     */
    getMenuLabelFromMenuItemId: function (menuItemId) {

        var menuItemText = '';
        var isReport = false;
        try {
            var menuItem = $('li').find('[data-menuid="' + menuItemId + '"]');
            isReport = menuItem.attr("data-isreport") === "True";
            menuItemText = portalBehaviourResources.PagetitleInManager.format(menuItem.attr("data-modulename"), menuItem[0].text.trim());
        } catch (e) {
            menuItemText = '';
        }
        return isReport ? '' : menuItemText;
    },

    localFormSizeDataTag: "data-local-form-size",

    /**
     * @name localFormSizeHandler
     * @description To handle add form-<size> to current HTML tag
     * @param {object} src The current object
     * @param {string} size Size to set
     * @param {string} preferenceKey The preference key used to be saved
     * @param {bool} isSkipSavePreference Flag to indicate if it should save the preference
     */
    localFormSizeHandler: function (src, size, preferenceKey, isSkipSavePreference) {
        var classToSet = "form-large";
        switch (size) {
            case "large": classToSet = "form-large"; break;
            case "medium": classToSet = "form-medium"; break;
            case "small": classToSet = "form-small"; break;
        }

        $(src).parent().siblings().removeClass("menu-active");
        $(src).parent().addClass("menu-active");
        $(src).parents("HTML").removeClass("form-large form-medium form-small").addClass(classToSet);

        if (!isSkipSavePreference) {
            // save value to user preference
            sg.utls.saveScreenLevelUserPreferences(preferenceKey, size);
            // mark HTML as it has local setting
            $(src).parents("HTML").attr(sg.utls.localFormSizeDataTag, size);
        }
    },

    /**
     * @name resetAllScreenSize
     */
    resetAllScreenSize: function(){
        $.each($('[id^="iFrameMenu"]').contents().find('html'), function (index, targetHTML) {
            var $targetHTML = $(targetHTML);
            // remote data tag
            $targetHTML.removeAttr(sg.utls.localFormSizeDataTag);
            
        });
    },

    deepCopy: function (obj) {
        return JSON.parse(JSON.stringify(obj));
    },

    /**
     * Copy value field by field from source to target
     * @param {object} source object copying from
     * @param {object} target object copying to
     */
    fieldCopy: function (source, target) {
        for (let key in source) {
            if (target[key] !== null && target[key] !== undefined &&   // make sure it has value
                target[key] !== source[key]) {                         // and only if they are different
                target[key] = source[key];
            }
        }
    }


    //initBackgroundImageCycling: function () {
    //    var TIMEOUT_MS = 2000;
    //    var timer;
    //    var body = $('body');
    //    var backgrounds = new Array(
    //        'url(../../../../Assets/images/login/image1.jpg) ',
    //        'url(../../../../Assets/images/login/image2.jpg) ',
    //        'url(../../../../Assets/images/login/image3.jpg) ');
    //    var index = 0;
    //    setTimeout(nextBackground, TIMEOUT_MS);

    //    function nextBackground() {
    //        index = ++index % backgrounds.length;
    //        var imagePath = backgrounds[index];
    //        body.css('background', imagePath);
    //        if (index > 2) {
    //            index = 0;
    //        }
    //        timer = setTimeout(nextBackground, TIMEOUT_MS);
    //    }
    //}
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
        for (var i = sg.constants.KeyCodeEnum.Zero; i <= sg.constants.KeyCodeEnum.Nine; i++)
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

        // Upper case
        for (var i = sg.constants.KeyCodeEnum.A; i <= sg.constants.KeyCodeEnum.Z; i++)
            alphaKeys.push(i);

        // Lower case
        for (i = sg.constants.KeyCodeEnum.a; i <= sg.constants.KeyCodeEnum.z; i++)
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
    var separatorCode;
    switch (separator) {
        case "-":
            separatorCode = sg.constants.KeyCodeEnum.Dash;
            break;

        case ".":
            separatorCode = sg.constants.KeyCodeEnum.Period;
            break;

        case "/":
            separatorCode = sg.constants.KeyCodeEnum.ForwardSlash;
            break;

        default:
            separatorCode = sg.constants.KeyCodeEnum.Space;
            break;
    }
    if (navigator.userAgent.indexOf("Firefox") != -1) {
        var fields = sg.utls.getFirefoxSpecialKeys(e);
        if (fields.indexOf(charCode) >= 0) {
            return true;
        }
    }

    var isNumeric = (charCode >= sg.constants.KeyCodeEnum.Zero &&
                     charCode <= sg.constants.KeyCodeEnum.Nine);
    var isBackspace = (charCode === sg.constants.KeyCodeEnum.Backspace);
    var isTab = (charCode === sg.constants.KeyCodeEnum.Tab);
    var isSeparator = (charCode === separatorCode);
    var isCorV = (charCode === sg.constants.KeyCodeEnum.c || charCode === sg.constants.KeyCodeEnum.v);

    return (isNumeric || isSeparator || isBackspace || isTab || (sg.utls.isCtrlKeyPressed && isCorV));
});

$(document).on("keypress", "[formatTextbox='alphaNumeric']", function (e) {
    var key = e.key;
    var pattern = XRegExp('^[\\p{L}\\d]+$');
    if (pattern.test(key))
    {
        return true;
    }
    return false;
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
    var isNumeric = (charCode >= sg.constants.KeyCodeEnum.Zero && charCode <= sg.constants.KeyCodeEnum.Nine);
    var isSeparator = (charCode === sg.constants.KeyCodeEnum.Colon);
    return (isNumeric || isSeparator);
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
        sg.utls.hasTriedToNotify = true; // To avoid recursive loop
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "LogJavascriptError"), data, function () { sg.utls.hasTriedToNotify = false });
    }
};


/************ NOTE!!!! This block of code here is to support Knockout before the upgrade, once it is done, the following code should be removed ************/

var nodeNames = "abbr|article|aside|audio|bdi|canvas|data|datalist|details|figcaption|figure|footer|" +
    "header|hgroup|mark|meter|nav|output|progress|section|summary|time|video";
var rtbody = /<tbody/i;
var rhtml = /<|&#?\w+;/;
var rxhtmlTag = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi;
var rtagName = /<([\w:]+)/;
var wrapMap = {
    option: [1, "<select multiple='multiple'>", "</select>"],
    legend: [1, "<fieldset>", "</fieldset>"],
    thead: [1, "<table>", "</table>"],
    tr: [2, "<table><tbody>", "</tbody></table>"],
    td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
    col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
    area: [1, "<map>", "</map>"],
    _default: [0, "", ""]
};
var rleadingWhitespace = /^\s+/;
var rcheckableType = /^(?:checkbox|radio)$/;
var rscriptType = /\/(java|ecma)script/i;

// Used in clean, fixes the defaultChecked property
function fixDefaultChecked(elem) {
    if (rcheckableType.test(elem.type)) {
        elem.defaultChecked = elem.checked;
    }
}

function createSafeFragment(document) {
    var list = nodeNames.split("|"),
        safeFrag = document.createDocumentFragment();

    if (safeFrag.createElement) {
        while (list.length) {
            safeFrag.createElement(
                list.pop()
            );
        }
    }
    return safeFrag;
}

jQuery.clean = function (elems, context, fragment, scripts) {
    var i, j, elem, tag, wrap, depth, div, hasBody, tbody, len, handleScript, jsTags,
        safe = context === document && safeFragment,
        ret = [];

    // Ensure that context is a document
    if (!context || typeof context.createDocumentFragment === "undefined") {
        context = document;
    }

    // Use the already-created safe fragment if context permits
    for (i = 0; (elem = elems[i]) != null; i++) {
        if (typeof elem === "number") {
            elem += "";
        }

        if (!elem) {
            continue;
        }

        // Convert html string into DOM nodes
        if (typeof elem === "string") {
            if (!rhtml.test(elem)) {
                elem = context.createTextNode(elem);
            } else {
                // Ensure a safe container in which to render the html
                safe = safe || createSafeFragment(context);
                div = context.createElement("div");
                safe.appendChild(div);

                // Fix "XHTML"-style tags in all browsers
                elem = elem.replace(rxhtmlTag, "<$1></$2>");

                // Go to html and back, then peel off extra wrappers
                tag = (rtagName.exec(elem) || ["", ""])[1].toLowerCase();
                wrap = wrapMap[tag] || wrapMap._default;
                depth = wrap[0];
                div.innerHTML = wrap[1] + elem + wrap[2];

                // Move to the right depth
                while (depth--) {
                    div = div.lastChild;
                }

                // Remove IE's autoinserted <tbody> from table fragments
                if (!jQuery.support.tbody) {

                    // String was a <table>, *may* have spurious <tbody>
                    hasBody = rtbody.test(elem);
                    tbody = tag === "table" && !hasBody ?
                        div.firstChild && div.firstChild.childNodes :

                        // String was a bare <thead> or <tfoot>
                        wrap[1] === "<table>" && !hasBody ?
                            div.childNodes :
                            [];

                    for (j = tbody.length - 1; j >= 0; --j) {
                        if (jQuery.nodeName(tbody[j], "tbody") && !tbody[j].childNodes.length) {
                            tbody[j].parentNode.removeChild(tbody[j]);
                        }
                    }
                }

                // IE completely kills leading whitespace when innerHTML is used
                if (!jQuery.support.leadingWhitespace && rleadingWhitespace.test(elem)) {
                    div.insertBefore(context.createTextNode(rleadingWhitespace.exec(elem)[0]), div.firstChild);
                }

                elem = div.childNodes;

                // Take out of fragment container (we need a fresh div each time)
                div.parentNode.removeChild(div);
            }
        }

        if (elem.nodeType) {
            ret.push(elem);
        } else {
            jQuery.merge(ret, elem);
        }
    }

    // Fix #11356: Clear elements from safeFragment
    if (div) {
        elem = div = safe = null;
    }

    // Reset defaultChecked for any radios and checkboxes
    // about to be appended to the DOM in IE 6/7 (#8060)
    if (!jQuery.support.appendChecked) {
        for (i = 0; (elem = ret[i]) != null; i++) {
            if (jQuery.nodeName(elem, "input")) {
                fixDefaultChecked(elem);
            } else if (typeof elem.getElementsByTagName !== "undefined") {
                jQuery.grep(elem.getElementsByTagName("input"), fixDefaultChecked);
            }
        }
    }

    // Append elements to a provided document fragment
    if (fragment) {
        // Special handling of each script element
        handleScript = function (elem) {
            // Check if we consider it executable
            if (!elem.type || rscriptType.test(elem.type)) {
                // Detach the script and store it in the scripts array (if provided) or the fragment
                // Return truthy to indicate that it has been handled
                return scripts ?
                    scripts.push(elem.parentNode ? elem.parentNode.removeChild(elem) : elem) :
                    fragment.appendChild(elem);
            }
        };

        for (i = 0; (elem = ret[i]) != null; i++) {
            // Check if we're done after handling an executable script
            if (!(jQuery.nodeName(elem, "script") && handleScript(elem))) {
                // Append to fragment and handle embedded scripts
                fragment.appendChild(elem);
                if (typeof elem.getElementsByTagName !== "undefined") {
                    // handleScript alters the DOM, so use jQuery.merge to ensure snapshot iteration
                    jsTags = jQuery.grep(jQuery.merge([], elem.getElementsByTagName("script")), handleScript);

                    // Splice the scripts into ret after their former ancestor and advance our index beyond them
                    ret.splice.apply(ret, [i + 1, 0].concat(jsTags));
                    i += jsTags.length;
                }
            }
        }
    }

    return ret;
};

/************ END OF THE BLOCK  ************/


/**
 * Add Sage-specific functionality to the jQuery namespace.
 */
// @ts-ignore
$(function () {
    if (sg.utls.isPortalIntegrated()) {
        window.top.SessionManager.ResetSessionTimer();
        var events = 'ajaxSend';
        $(document).on($.trim((events + ' ').split(' ').join('.idleTimer ')), window.top.SessionManager.ResetSessionTimer); 

        var sessionId = sg.utls.getSessionId();
        sage.cache.local.set(sessionId + "_LoggedIn", new Date().getTime().toString());
    }

    kendo.culture(globalResource.Culture);

    var coreIndex = window.location.href.indexOf("/Core/");
    var sharedIndex = window.location.href.indexOf("/Shared/");
    var customAdmin = window.location.href.indexOf("/AS/CustomScreen");


    if (coreIndex < 0 && sharedIndex < 0 && customAdmin < 0
        && !document.getElementById("frmOnPremiseLogin")) { // don't ask for currency info in login page as users are not login yet
        sg.utls.loadHomeCurrency();
    }

    var screenHome = "999999";
    var isHomePage = self.location === top.location && $("#ScreenName").val() === screenHome;

    // In Home page on mouse click, this event is executing and hence success message is not displaying, so if the page is home 
    // then don't execute this code
    if (!isHomePage) {
        $("body").on("click", function () {
            var successControl = $(this).find("#success");
            if (successControl.length > 0) {
                successControl.empty();
            }
        });
    }

    var keyHandler = function (e) {
        if (e.keyCode === sg.constants.KeyCodeEnum.ESC) {
            $(".msgCtrl-close").trigger("click");
            $(document).off("keydown", keyHandler);
        }
    };
    $(document).on("keydown", keyHandler);

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

    // Set flag if shift key is pressed. Used for grid tabbing
    $(document).on('keyup keydown', function (e) {

        sg.utls.isShiftKeyPressed = e.shiftKey;
        sg.utls.isCtrlKeyPressed = e.ctrlKey;
        var keyCode = e.keyCode || e.which;
        if (keyCode === sg.constants.KeyCodeEnum.Tab) {
            sg.utls.istabKeyPressed = true;
        }

        return true;
    });

    // Open sibling finder when textbox is focused and Alt+DownArrow are pressed
    $(document).on('keyup keydown', function (e) {
        if ((e.altKey || e.ctrlKey) && e.keyCode === sg.constants.KeyCodeEnum.DownArrow) {
            // Do NOT open sibling finder when textbox error-message is showing and Alt+DownArrow are pressed
            if ($('#injectedOverlay').length === 0 && $('#injectedOverlayTransparent').length === 0) {
                var textbox = $(document.activeElement);
                if (textbox.attr('type') === "text") {
                    var finderId = sg.utls.findersList[textbox.attr('id')];
                    if (finderId) {

                        // if there is a calendar widget, return if ALT + DOWN
                        if (textbox.attr('data-role') === 'datepicker') {
                            // there is a calendar control, ignore ALT +DOWN
                            if (e.altKey)
                                return;
                        }

                        var finder = $("#" + finderId);
                        if (finder.is(':visible') && !finder.is(':disabled')) {
                            finder.focus();
                            //Some finders are initialized on mousedown
                            finder.trigger('mousedown');
                            finder.trigger('click');
                        }
                    }
                }
            }
        }
    });

    // Mapping Left/Right Arrow key when numerictextbox is focused
    $(document).on('keyup', ".data-nav input[data-role='numerictextbox']", function (e) {
        const numericTextBox = $(document.activeElement).data("kendoNumericTextBox");
        if (e.keyCode === sg.constants.KeyCodeEnum.LeftArrow) {
            numericTextBox._step(-1);
        } else if (e.keyCode === sg.constants.KeyCodeEnum.RightArrow) {
            numericTextBox._step(1);
        }
    });

    $(document).on('mouseup mousedown', function (e) {
        sg.utls.istabKeyPressed = false;
    });

    $("input[data-val-length-max]").each(function () {
        var $this = $(this);
        var data = $this.data();
        if (data.valLengthMax)
            $this.attr("maxlength", data.valLengthMax);
    });

    // This may not be required.
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
    menuLink.on("mouseenter", function () {
        $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
        $(this).children(".sub-menu").show();
    }).on("mouseleave", function () {
        $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
        $(this).children(".sub-menu").hide();
    });

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

    /**
     * @name pageUnloadHandler
     * @description TODO - Add method descripiton
     */
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
        //
        // Portal Page
        //
		sessionStorage["productId"] = "";        $(window).on('unload', function () {
            PageUnloadHandler();
            sg.utls.updateOpenScreenCount();
        });

        $(window).scroll(function () {
            sg.utls.showMessagesInViewPort();
        });
    } else {
        //
        // Non-Portal Page
        //
        var sessionPerPage = $("#SessionPerPage");
        if (sessionPerPage.length > 0 && sessionPerPage.val() === "True") {
            $(window).on('unload', function () {
                PageUnloadHandler();
                sg.utls.destroySession();
            });
        } else {
            $(window).on('unload', function (e) {
                window.name = "unloadediFrame";
                PageUnloadHandler();
            });
        }
    }

    jQuery.validator.addMethod("format", function (source, parameters) {
        return $.validator.format(source, parameters);
    });

    // For some reason, clicking the pencil button in a grid will also trigger a close event in Mac Safari browser
    if (sg.utls.isSafari()) {
        $(".datagrid-group").on("mousedown", function (e) {
            if ($(e.target).is('input:button')) {
                e.preventDefault();
                e.stopImmediatePropagation();
            }
        });
    }
   
    /**
     * Add receive message and post message for set product key from outside application, such as CRM
     * @param {any} e: message 
     */
    function receiveMessage(e) {
        // Set product key to session storage to identify request from outside application iframe
        if ( e.data && e.data.id === "SetProductKey") {
            sessionStorage["productId"] = e.data.key;
        }
        // Reset Sage300 client side timer to prevent UI side timeout 
        if (e.data && e.data.id === "ResetSage300Timer") {
            var sessionId = sg.utls.getSessionId();
            var key = sessionId + "_ResetSessionTimer";
            localStorage[key] = new Date().getTime().toString();
        }
    }
    window.addEventListener('message', receiveMessage);
    window.parent.postMessage({ id: 'Sage300Loaded' }, "*");

    function redirectInCurrentTab(sessionIdFromKey, message) {
        var url = location.href;
        var isAdminTab = url.indexOf("AS/CustomScreen") > 0;
        var sessionId = sg.utls.getSessionId();

        // Only redirect if the sessionId from the cache message is the same as the current sessionId
        if (sessionIdFromKey === sessionId) {

            // Get the index of the sessionId from the url
            var idx = url.indexOf(sessionId);

            // If sessionId was found in the url
            if (idx > 0) {
                // This will allow us to bypass the 'beforeunload' event handling in all behaviour 
                // files in response to logging out in a different tab page(same user and company) 
                // This will avoid displaying the ugly little browser confirmation dialog box.
                // Note: This block is only relevant when running in Sage 300 Portal, not CRM
                if (sg && sg.utls && sg.utls.getPortalWindow) {
                    var portalWnd = sg.utls.getPortalWindow();
                    if (portalWnd.pageUnloadEventManager) {
                        portalWnd.pageUnloadEventManager.disable();
                    }
                }

                var redirectUrl = "";
                if (message === "EvictInitiated") {
                    redirectUrl = sg.utls.url.evictUrl();
                    if (isAdminTab) {
                        redirectUrl += "?isAdminLogin=true";
                    }
                } else {
                    var index = url.indexOf('OnPremise');
                    redirectUrl = url.substring(0, index > 0 ? index : idx);
                    if (isAdminTab) {
                        redirectUrl += "admin";
                    }
                }

                // Log out, post message to outside application that use Sage 300 screens
                if (sessionStorage["productId"]) {
                    // Running in CRM
                    window.parent.postMessage({ id: 'Sage300Logout' }, "*");
                } else {
                    // Running in Portal, for KPI widget windows should not set to login page
                    if (location.href.indexOf('/KPI/') < 0) {
                        location.href = redirectUrl;
                    }
                }
            }
        }
    }

    // Multiple tab pages sign out as one of tab page logout
    $(window).on('storage', function (e) {
        if (e && e.originalEvent && e.originalEvent.key) {
            var key = e.originalEvent.key;

            if (key === "modernizr") {
                // We don't care about this so just return
                return;
            } else {
                // Extract the sessionId and message from the key
                var parts = key.split('_');
                var sessionIdFromKey = parts[0];
                var message = parts[1];

                if (message === "UpdatePortalSessionDate") {
                    var sessionDateFromLocalStorage = sage.cache.local.get(key);
                    var dt = kendo.toString(sessionDateFromLocalStorage, 'MMM dd, yyyy');
                    // Set session date in top menu
                    $("#spnSessionDate").html(dt);
                    return;
                } else if (message === "Sage300Timeout") {
                    window.parent.postMessage({ id: 'Sage300Logout' }, "*");
                    return;
                } else if (message === "LogoutInitiated" || message === "EvictInitiated") {
                    // This code will be executed for each active tab/browser 
                    redirectInCurrentTab(sessionIdFromKey, message);
                }
            }
        }
    });

    //sg.utls.initBackgroundImageCycling();

    // Outside request(like CRM) not increase count, prevent cross domain issues
    if (!sg.utls.isSameOrigin()) {
        return;
    }
});

(function ($) {
    function hasScroll(el, index, match) {
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
            if (sY === scroll) { return true; }
        }

        // Compare client and scroll dimensions to see if a scrollbar is needed
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
