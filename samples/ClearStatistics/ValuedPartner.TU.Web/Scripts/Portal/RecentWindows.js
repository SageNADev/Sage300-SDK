/* Copyright (c) 1994-2024 The Sage Group plc or its licensors.  All rights reserved. */

//"use strict";

var RecentWindowsMenu = function (limitSetting, menuUrlList, recentWindowsKey, cache, menuLabelGetter, htmlEncode) {

    const constants = {
        DEFAULT_MAX_RECENT_WINDOWS: 10,
        ABSOLUTE_MAX_RECENT_WINDOWS: 100,
        RECENTWINDOWS_DIV_SELECTOR: '#recentWindowManager > div',
        DV_RECENTWINDOWS_SELECTOR: "#dvRecentWindows",
        DV_RECENTWINDOWS_SPAN_SELECTOR: "#dvRecentWindows span",
        DATAMENUID_SELECTOR: 'data-menuid',
        PARENTID_SELECTOR: 'data-parentid',
        IS_REPORT_IDX: "ReportViewer.aspx?token",
        ONPREMISE: "OnPremise",
        UNDEFINED: "undefined",
        TYPE_STRING: "string",
        TYPE_NUMBER: "number",
        TYPE_UNDEFINED: "undefined"
    };

    // Functions for processing constructor arguments, so they have to come first

    /**
     * gets the number of entries permitted in the Recent Windows List, based upon the configuration file setting
     * @param {(string|number)} setting the setting from the configuration file
     */
    function RecentWindowsLimit(setting) {
        // For security - recognize that the setting should not be trusted, since it comes from a minimally trusted server-side source
        const defaultSize = constants.DEFAULT_MAX_RECENT_WINDOWS;
        if (!setting) return defaultSize;
        let recentWindowsLimit = defaultSize;
        if (typeof setting === constants.TYPE_STRING) {
            if (!(/^([0-9]+)$/.test(setting))) return defaultSize;
            recentWindowsLimit = Number.parseInt(setting);
        } else if (typeof setting === constants.TYPE_NUMBER && Number.isInteger(setting)) {
            recentWindowsLimit = setting;
        } else {
            return defaultSize;
        }
        if (recentWindowsLimit <= 0 || recentWindowsLimit > constants.ABSOLUTE_MAX_RECENT_WINDOWS) {
            recentWindowsLimit = defaultSize;
        }
        return recentWindowsLimit;
    }

    /**
     * creates a map of sets representing the menu list, allowing easier matching. It would appear that each menu item has one and only one parent. However, that is not guaranteed and the existing code did not assume it.
     * @param {{Data: {MenuId: (number|string), ParentMenuId: (number|string)}}[]} menuList the menu list from the server as an array of Data items, each with a MenuID and ParentMenuId
     * @returns {any} a set-based representation that is easier to use for checking
     */
    function MenuMap(menuList) {
        const map = new Map();
        for (var i = 0; i < menuList.length; i++) {
            const data = menuList[i].Data;
            if (data) {
                let menuId = data.MenuId;
                if (typeof menuId === constants.TYPE_NUMBER) menuId = "" + menuId;
                let parentId = data.ParentMenuId;
                if (typeof parentId === constants.TYPE_NUMBER) parentId = "" + parentId;
                if (menuId && parentId) {
                    let set = map.get(menuId);
                    if (set === undefined) {
                        set = new Set();
                        map.set(menuId, set);
                    }
                    set.add(parentId);
                }
            }
        }
        return map;
    }

    const _menuMap = MenuMap(menuUrlList);
    const _recentWindowsLimit = RecentWindowsLimit(limitSetting);

    let _onHasVisibleFrame; // may hold one callback, to be called on show if one of the entries is visible

    /**
     * A local copy of the items in the Recent Windows List, most recent first. This is kept synchronized with the copy in the local storage and the DOM is generated from it.
     */
    const items = [];

    /**
     * gets the DOM element under which the list sits
     * @returns a jQuery object representing the parent of the list
     */
    function GetDom() {
        return $(constants.DV_RECENTWINDOWS_SELECTOR);
    }

    /**
     * @name simplifyTargetUrl
     * @description Remove all items from a url except Area, Controller, Action and any parameters
     *              Note: Depending on whether this is being run through one's debugger or from
     *                    an actual installation, the url being passed in may differ slightly.
     *
     *                    Examples:
     *
     *                    Live Installation :       "/Sage300/OnPremise/[SID]/AR/Customer"
     *                    Development Environment : "/OnPremise/[SID]/AR/Customer"
     *                    
     *                    More Elaborate Examples:
     *
     *                    Live Installation :       "/Sage300/OnPremise/[SID]/Core/InquiryGeneral/Index/?templateId=b64aa4df-a1f1-41c0-ad9e-0efac930de7e&name=Payment Inquiry (Functional Currency)&dsId=8be1f261-1208-4508-a6ca-e645a1881227"
     *                    Development Environment : "/OnPremise/[SID]/Core/InquiryGeneral/Index/?templateId=b64aa4df-a1f1-41c0-ad9e-0efac930de7e&name=Payment Inquiry (Functional Currency)&dsId=8be1f261-1208-4508-a6ca-e645a1881227"
     * @private
     * @param {string} _url - The url to simplify
     * @returns {string} - A string representing the area, controller, action and parameter(s), with a possible final separator between action and parameters
     */
    function simplifyTargetUrl(_url) {
        var areaIndex = 3;
        var controllerIndex = areaIndex + 1;
        var actionIndex = controllerIndex + 1;
        var parameterIndex = actionIndex + 1;

        var url = _url;
        if (_url && _url.length > 0) {
            var temp = _url.split("/");

            // The first character is a forward slash '/' with nothing to the left of it
            if (temp[0].length === 0) {

                if (temp[1] !== constants.ONPREMISE) {
                    // Live Installation or development environment running on local IIS
                    // instead of IIS Express. Could be 'Sage300' or 'Sage.CA.SBS.ERP.Sage300.Web'
                    // or something else?
                    areaIndex = 4;

                } else if (temp[1] === constants.ONPREMISE) {
                    // Development Environment (using IIS Express)
                    areaIndex = 3;
                }

                controllerIndex = areaIndex + 1;
                actionIndex = controllerIndex + 1;
                parameterIndex = actionIndex + 1;

                url = temp[areaIndex] + "/" + temp[controllerIndex];

                if (temp[actionIndex] && temp[actionIndex] !== constants.UNDEFINED) {
                    url += "/" + temp[actionIndex];
                }

                if (temp[parameterIndex] && temp[parameterIndex] !== constants.UNDEFINED) {
                    url += "/" + temp[parameterIndex];
                }
            }
        }
        return url;
    }

    /**
     * checks that the provided url is acceptable for display (since it may have come from an untrusted source)
     * @param {string} url the url to test
     */
    function IsAcceptableUrl(url) {
        // Confirm that the url is "area/controller[/action[/][?parameters]]"
        // simplifyURL will have removed any leading slash.
        // However, currently it will 'preserve' any trailing slash
        // It is dangerous to 'normalise' that, so instead we are careful here
        if (!url) return false;
        if (typeof url !== constants.TYPE_STRING) return false;
        if (url.trim() === "") return false;
        const split = url.split("/");
        const splitLength = split.length;
        if (splitLength < 2) return false;
        if (splitLength > 4) return false;
        const area = split[0].trim();
        if (area.length <= 0) return false;
        if (!IsSimpleId(area)) return false;
        const controller = split[1].trim();
        if (controller.length <= 0) return false;
        if (!IsSimpleId(controller)) return false;
        if (splitLength === 2) return true;
        if (splitLength === 3) {
            // This might be with action but no parameters, or it might be an action with no trailing slash, going straight into parameters
            const actionAndParameters = split[2].trim();
            if (actionAndParameters.length <= 0) return false;
            const actionSplit = actionAndParameters.split('?');
            const actionSplitLength = actionSplit.length;
            if (actionSplitLength < 1 || actionSplitLength > 2) return false;
            const action = actionSplit[0].trim();
            if (!IsSimpleId(action)) return false;
            if (actionSplitLength < 2) return true;
            const parameters = actionSplit[1].trim();
            if (parameters.length <= 0) return false;
            // TODO Do we need to test the parameters more thoroughly?
            return true;
        } else {
            // assert splitLength === 4
            const action2 = split[2].trim();
            if (action2.length <= 0) return false;
            if (!IsSimpleId(action2)) return false;
            const parameters2 = split[3].trim();
            if (parameters2.length <= 0) return false;
            // TODO Do we need to test the parameters more thoroughly?
            return (parameters2.startsWith('?'));
        }
    }

    /**
     * @name save
     * @description Store recent windows list to localStorage
     * @private
     */
    function save() {
        cache.set(recentWindowsKey, items);
    }

    /**
     * returns the index of the existing copy, if any, which must be removed from the middle of the list
     * @param {(string|number)} menuId the menu id of the entry to be found, which is the one to be added
     * @param {number} limit the number of items allowed in the list
     * @returns -1 if there is no need to remove an entry; just truncate the list before adding
     */
    function ExistingIndex(menuId, limit) {
        // The limit should not be negative or zero anyway. If it is 1 then just truncate.
        if (limit <= 1) return -1;

        const length = items.length;
        const adjustedLimit = limit - 1;
        const iterationLimit = adjustedLimit < length ? adjustedLimit : length;
        for (let index = 0; index < iterationLimit; ++index) {
            if (items[index].menuid == menuId) {
                return index;
            }
        }
        return -1;
    }

    /**
     * @name removeRecentWindowsDuplicatedOrAboveLimit
     * @description Removes Recent Windows excess menu items or duplicates of the new one.
     * @private
     * @param {string} menuId the menu item being added to to the menu
     */
    function removeRecentWindowsDuplicatedOrAboveLimit(menuId) {
        const limit = _recentWindowsLimit;
        const index = ExistingIndex(menuId, limit);
        if (index >= 0) {
            items.splice(index, 1);
        }
        if (items.length >= limit) {
            // assert limit > 0
            items.length = limit - 1;
        }
    }

    /**
     * @name checkIsPermitted
     * @description TODO - Add Description
     * @private
     * @param {number} menuId - TODO - Add Description
     * @param {number} parentId - TODO - Add Description
     * @returns {boolean} true = permitted | false = not permitted
     */
    function checkIsPermitted(menuId, parentId) {
        if (!parentId || !menuId) return true;
        // Beware types. In particular the parent id might be a string in the menuUrlList but a number in the local storage
        let strParentId = parentId;
        if (typeof parentId === constants.TYPE_NUMBER) {
            strParentId = "" + parentId;
        }
        let strMenuId = menuId;
        if (typeof menuId === constants.TYPE_NUMBER) {
            strMenuId = "" + menuId;
        }
        const set = _menuMap.get(strMenuId);
        if (!set) return false;
        return (set.has(strParentId));
    }

    /**
     * @name isVisible
     * @description Is the Recent Windows Menu currently visible?
     * @private
     * @returns {boolean} true = menu is visible | false = menu is not visible
     */
    function isVisible() {
        return $(constants.RECENTWINDOWS_DIV_SELECTOR).is(':visible');
    }

    /**
     * @name loadItems
     * @description Get the Recent Windows Menu data from local storage, including security checks, translation, etc.
     */
    function LoadItems() {
        items.length = 0;
        const loaded = cache.get(recentWindowsKey);
        if (typeof loaded == 'object' && Array.isArray(loaded)) {
            const limit = _recentWindowsLimit;
            loaded.forEach((element) => {
                if (items.length < limit 
                    && typeof element == 'object'
                    && element.hasOwnProperty('iframeId')
                    && element.hasOwnProperty('menuid')
                    && element.hasOwnProperty('targetUrl')
                    && element.hasOwnProperty('windowText')
                    && IsSimpleId(element.iframeId)
                    && IsAcceptableUrl(element.targetUrl)
                ) {
                    const menuid = element.menuid;
                    const parentid = element.parentid;
                    if (IsSimpleId(menuid)
                        && IsSimpleId(parentid)
                        && checkIsPermitted(menuid, parentid)
                    ) {
                        var menuItemText = menuLabelGetter(menuid);
                        if (menuItemText) {
                            element.windowText = menuItemText;
                        }
                        items.push(element);
                    }
                }
            });
        }
    }

    /**
     * having loaded the items from local storage, push them into the Dom
     */
    function AddItemsToDom() {
        const domEntry = GetDom();
        domEntry.empty();
        items.forEach(record => domEntry.append(MenuHTML(record)));
    }

    function IsStockReport(url) {
        // Stock report result pages are NOT stored in the 'recent windows list'
        // Custom reports are stored.
        // It is likely that this code is obsolete as a result of changing how the Web Screens handle reports.
        return url.indexOf(constants.IS_REPORT_IDX) > 0;
    }

    /**
     * tests that the id is a simple menu id etc. and so does not require html encoding into the DOM
     * @param {any} id the id to test
     * @returns true if the id is OK
     */
    function IsSimpleId(id) {
        if (!id) return true;
        const type = typeof id;
        if (type === constants.TYPE_UNDEFINED || type === constants.TYPE_NUMBER) return true;
        if (type === constants.TYPE_STRING) {
            // Strangely, there are several menu and parent entries with leading digits
            return /^([a-zA-Z0-9_-]*)$/.test(id);
        }
        return false;
    }

    /**
     * returns the HTML snippet representing the record
     * @param {any} record the record representing the recent window. This must have already been checked.
     * @returns a string containing the HTML snippet
     */
    function MenuHTML(record) {
        // Element for recent window without the 'x' button at the end.
        // 'RW' in the id of div stands for Recent Window to distinguish
        // from Open Window

        var id = 'dvRW' + record.iframeId;
        var t = '<div id="' + id + '" class="rcbox"><span data-menuid="' + record.menuid + '"data-parentid="' + record.parentid +
            '"data-url="' + htmlEncode(record.targetUrl) + '"frameId="' + record.iframeId + '"command="Add" rank="1">' + htmlEncode(record.windowText) + '</span></div>';
        return $(t);
    }

    var _public = {

        /**
         * @name populateRecentWindow
         * @description - Add a new item to the recent windows list
         * @public
         * @param {object} $iframe - the iframe object, with a src attribute and an id attribute
         * @param {string} menuid - a simple identifier indicating the menu item
         * @param {string} parentid - undefined or a simple identifier indicating the parent
         * @param {string} targetUrl - The target url for the menu item. It will be simplified and htmlEncoded.
         * @param {string} windowText - The text to display for the menu item. This should be pure text. It will be htmlEncoded.
         */
        populateRecentWindow: function($iframe, menuid, parentid, targetUrl, windowText) {

            // Only modify recent windows list if it is not visible.
            // Stock report result pages are NOT stored in the 'recent windows list'
            // Custom reports are stored.
            if (!isVisible() && !IsStockReport($iframe.attr("src"))) {
                var iframeId = $iframe.attr('id');
                if (IsSimpleId(iframeId) && IsSimpleId(menuid) && IsSimpleId(parentid)) {
                    const url = simplifyTargetUrl(targetUrl);
                    if (IsAcceptableUrl(url)) {
                        removeRecentWindowsDuplicatedOrAboveLimit(menuid);
                        const record = { iframeId: iframeId, menuid: menuid, parentid: parentid, targetUrl: url, windowText: windowText };
                        items.unshift(record);
                        save();
                        AddItemsToDom();
                    }
                }
            }
        },

        /**
         * @name onLoad
         * @description On first sign-in when recent windows is not yet populated.
         * @param {object} onHasVisibleFrame the onHasVisibleFrame callback function whose purpose I really do not understand.
         * @public
         */
        onLoad: function (onHasVisibleFrame) {
            _onHasVisibleFrame = onHasVisibleFrame;
            LoadItems();
            AddItemsToDom();
        },

        /**
         * @name show
         * @description Display the recent windows list popup
         * @public
         */
        show: function() {
            // Rebuild the list based on the current contents of localStorage
            // This is done to ensure that screens opened on other browser tabs
            // for current user / company combination show up no matter what
            // tab one is currently on.
            LoadItems();
            AddItemsToDom();
            if (items.length > 0) {
                $(constants.RECENTWINDOWS_DIV_SELECTOR).show();
            }

            // Reset selection
            $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).removeClass('selected');

            // Find Active Window and AddClass Selected
            $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).each(function (index, elem) {
                var $iframe = $('#' + $(elem).attr('frameid'));
                if ($iframe.is(':visible')) {
                    if (_onHasVisibleFrame) {
                        _onHasVisibleFrame();
                    }
                    return false;
                }
            });
        },

        /**
         * @name hide
         * @description Hide the recent windows list popup
         * @public
         */
        hide: function() {
            $(constants.RECENTWINDOWS_DIV_SELECTOR).hide();
        }
    };

    return _public;
};
// Unlike most of our Sage 300 code, we do not create a hoisted global variable for an instance of this class;
// we create the one instance we need within the one class that uses it (TaskDoc-Menu-BreadCrumb)