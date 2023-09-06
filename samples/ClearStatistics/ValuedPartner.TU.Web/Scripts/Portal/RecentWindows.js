/* Copyright (c) 1994-2023 Sage Software, Inc.  All rights reserved. */

//"use strict";

/**
 * Requires global (or parent scoped) variables:
 * sg.utls.localStorageKeys.RECENT_WINDOWS_BASE
 * LOGGED_IN_TENANT_CONST
 * LOGGED_IN_USERNAME_CONST
 * menuUrlList
 * screenId
 * reportScreenHelp
 */

var RecentWindowsMenu = function () {

    var constants = {
        RECENTWINDOWS_DIV_SELECTOR: '#recentWindowManager > div',
        DV_RECENTWINDOWS_SELECTOR: "#dvRecentWindows",
        DV_RECENTWINDOWS_SPAN_SELECTOR: "#dvRecentWindows span",
        DATAMENUID_SELECTOR: 'data-menuid',
        PARENTID_SELECTOR: 'data-parentid',
        IS_REPORT_IDX: "ReportViewer.aspx?token",
        ONPREMISE: "OnPremise",
        UNDEFINED: "undefined"
    };

    /**
     * @name simplifyTargetUrl
     * @description Remove all items from an url except Area, Controller, Action and any paramters
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
     * @returns {string} - A string representing the area, controller, action and parameter(s)
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
     * @name loadRecentWindowsListFromStorage
     * @description Load recent window list from storage
     * @private
     */
    function loadRecentWindowsListFromStorage() {
        var key = getRecentWindowKey();
        var cachedMarkup = sage.cache.local.get(key);

        //
        // Now, we need to ensure that the labels used in the menu are in the correct language
        //
        $(cachedMarkup).find('span').each(function (index, value) {
            // From the cachedMarkup, get each menuId
            var item = $(this);
            var menuItemId = item[0].attributes['data-menuid'].value;
            var menuTextToBeConverted = item[0].innerHTML;

            // Now that we have a menuId, we need to find the matching menu item from the main menu.
            var menuItemText = sg.utls.getMenuLabelFromMenuItemId(menuItemId);
            if (menuItemText) {
                cachedMarkup = cachedMarkup.replace(menuTextToBeConverted, menuItemText);
            }
        });


        $(constants.DV_RECENTWINDOWS_SELECTOR).html(cachedMarkup);
    }

    /**
     * @name getRecentWindowsKey
     * @description The name of the localStorage key
     * @private
     * @returns {string} Return the name localStorage key
     */
    function getRecentWindowKey() {
        var sessionId = sg.utls.extractSessionIdFromWindow();
        var key = sessionId + sg.utls.localStorageKeys.RECENT_WINDOWS_BASE;
        return key;
    }

    /**
     * @name save
     * @description Store recent windows list html markup to localStorage
     *              [Has Unit Test]
     * @private
     */
    function save() {
        var key = getRecentWindowKey();
        var markup = getHtml();
        sage.cache.local.set(key, markup);
    }

    /**
     * @name removeNonPermittedItems
     * @description Remove all items that cause length of list to exceed the limit
     * @private
     */
    function removeNonPermittedItems() {
        $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).each(
            function (index, elem) {
                var currentMenuId = $(elem).attr(constants.DATAMENUID_SELECTOR);

                // Remove any recent windows over the limit
                if (currentMenuId) {
                    removeNonPermittedMenuItems("", elem, currentMenuId);
                }
            }
        );
    }

    /**
     * @name removeRecentWindowsDuplicatedOrAboveLimit
     * @description Removes Recent Windows excess menu items or duplicated if menuId param defined.
     * @private
     * @param {string} menuId added to menu, if empty, removes excess items
     */
    function removeRecentWindowsDuplicatedOrAboveLimit(menuId) {
        // calculate limit of recent windows to display, default is 10
        var recentWindowsLength = $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).length;
        var recentWindowsLimit = RECENT_WIN_LIMIT_CONST;
        if (!recentWindowsLimit) {
            recentWindowsLimit = 10;
        }
        var recentWindowsToRemove = recentWindowsLength - recentWindowsLimit;

        // Remove items in reverse order from the list - FIFO
        $($(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).get().reverse()).each(
            function (index, elem) {
                var currentMenuId = $(elem).attr(constants.DATAMENUID_SELECTOR);

                // Remove any recent windows over the limit
                if (currentMenuId) {

                    if (recentWindowsToRemove >= 0) {
                        // Only remove the last element when adding items
                        if (recentWindowsToRemove === 0 && menuId !== "") {
                            $(elem).closest("div").remove();
                        } else if (recentWindowsToRemove > 0) {
                            $(elem).closest("div").remove();
                        }
                        recentWindowsToRemove--;
                        // continue to the next recent window span
                        return true;
                    }

                    // Remove screen name from recent window list
                    // if a screen with matching menu id is found
                    if (currentMenuId === menuId) {
                        $(elem).closest("div").remove();
                        // breaking out of the .each call.
                        return false;
                    }
                }
            }
        );
    }

    /**
     * Removes Recent Windows non-permitted menu items
     * Requires menuUrlList parent scope variable
     * @private
     * @param {string} menuId - menu id added to menu
     * @param {jquery} elem - the html element
     * @param {string} currentMenuId - current menuId from data attribute
     * @returns {bool} true if elem removed, false otherwise
     */
    function removeNonPermittedMenuItems(menuId, elem, currentMenuId) {
        // menuUrlList is a global var

        // Only on initial load.
        if ("" === menuId) {
            var currentParentId = $(elem).attr(constants.PARENTID_SELECTOR);

            // Remove screen name if user has no rights to it
            if (currentMenuId && currentParentId) {
                var permitted = checkIsPermitted(menuUrlList, currentMenuId, currentParentId);

                // current recently used window entry not permitted
                if (!permitted) {
                    $(elem).closest("div").remove();
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * @name checkIsPermitted
     * @description TODO - Add Description
     * @private
     * @param {object} menuUrlList - TODO - Add Description
     * @param {number} menuId - TODO - Add Description
     * @param {number} parentId - TODO - Add Description
     * @returns {boolean} true = permitted | false = not permitted
     */
    function checkIsPermitted(menuUrlList, menuId, parentId) {
        var permitted = false;
        for (var i = 0; i < menuUrlList.length; i++) {
            if (menuUrlList[i].Data.MenuId === menuId &&
                menuUrlList[i].Data.ParentMenuId === parentId) {
                permitted = true;
                break;
            }
        }
        return permitted;
    }

    /**
     * @name isEmpty
     * @description Are there any items in the recent window list?
     * @private
     * @returns {boolean} true = recent window list is empty | false = recent window list is not empty
     */
    function isEmpty() {
        var items = loadItems();
        if (items && items.length > 0) {
            return false;
        }
        return true;
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
     * @description Get the Recent Windows Menu html block from local storage
     * @returns {string} Html representing the Recent Windows menu
     */
    function loadItems() {
        var key = getRecentWindowKey();
        return sage.cache.local.get(key);
    }

    /**
     * @name getHtml
     * @description Get the menu html markup from the DOM
     * @returns {string} menu html string
     */
    function getHtml() {
        return $(constants.DV_RECENTWINDOWS_SELECTOR).html();
    }

    /**
     * @name insertAtTop
     * @description Insert a new menu item to the top of the list in the DOM.
     * @param {object} menuItem JQuery object representing a new menu item
     */
    function insertAtTop(menuItem) {
        $(constants.DV_RECENTWINDOWS_SELECTOR).prepend(menuItem);
    }

    function onClick() {
        // screenId is at global/parent scode

        // TODO: Requires further refactor of TaskDock-Menu-BreadCrumb.js
        // to deal with function calls within the document ready scope:
        // isMaxScreenNumReachedAndNotOpen, loadBreadCrumb, clearIframes, assignUrl
    }

    var _public = {

        // Public constants
        constants: constants,

        // Methods publicly exposed for unit testing purposes only
        _unittestable: {
            save: save,
            getRecentWindowKey: getRecentWindowKey,
            removeNonPermittedItems: removeNonPermittedItems,
            removeRecentWindowsDuplicatedOrAboveLimit: removeRecentWindowsDuplicatedOrAboveLimit,
            simplifyTargetUrl: simplifyTargetUrl
        },

        /**
         * @name populateRecentWindow
         * @description - Add a new item to the recent windows list
         * @public
         * @param {object} $iframe - TODO - Add description
         * @param {number} menuid - TODO - Add description
         * @param {number} parentid - TODO - Add description
         * @param {string} targetUrl - The target url for the menu item
         * @param {string} windowText - The text to display for the menu item
         */
        populateRecentWindow: function($iframe, menuid, parentid, targetUrl, windowText) {
            var url = $iframe.attr("src");

            // Stock report result pages are NOT stored in the 'recent windows list'
            // Custom reports are stored.
            var isStockReport = url.indexOf(constants.IS_REPORT_IDX) > 0;

            // Only modify recent windows list if it is not visible.
            if (!isVisible() && !isStockReport) {

                // Element for recent window without the 'x' button at the end.
                // 'RW' in the id of div stands for Recent Window to distinguish 
                // from Open Window

                // Remove everything but Area and controller
                var targetUrlSimplified = kendo.htmlEncode(simplifyTargetUrl(targetUrl));

                var iframeId = $iframe.attr('id');
                var id = 'dvRW' + iframeId;
                var t = '<div id="' + id + '" class="rcbox"><span data-menuid="' + menuid + '"data-parentid="' + parentid +
                    '"data-url="' + targetUrlSimplified + '"frameId="' + iframeId + '"command="Add" rank="1">' + windowText + '</span></div>';
                var $menuItem = $(t);

                removeRecentWindowsDuplicatedOrAboveLimit(menuid);
                insertAtTop($menuItem);
                save();
            }
        },

        /**
         * @name onLoadPopulateRecentWindowsListFromStorage
         * @description On first sign-in when recent windows is not populated.
         * @public
         */
        onLoadPopulateRecentWindowsListFromStorage: function() {
            var html = loadItems();
            $(constants.DV_RECENTWINDOWS_SELECTOR).html(html);

            removeNonPermittedItems();

            // Clear up excess windows if recent window size is configured smaller
            removeRecentWindowsDuplicatedOrAboveLimit('');
        },

        /**
         * @name show
         * @description Display the recent windows list popup
         * @public
         */
        show: function() {
            // Are there any items in the recent windows list?
            if (!isEmpty()) {
                // Rebuild the list based on the current contents of localStorage
                // This is done to ensure that screens opened on other browser tabs 
                // for current user / company combination show up no matter what
                // tab one is currently on.
                loadRecentWindowsListFromStorage();

                // ...and show the popup
                $(constants.RECENTWINDOWS_DIV_SELECTOR).show();
            }

            // Reset selection
            $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).removeClass('selected');

            // Find Active Window and AddClass Selected
            $(constants.DV_RECENTWINDOWS_SPAN_SELECTOR).each(function (index, elem) {
                var $iframe = $('#' + $(elem).attr('frameid'));
                if ($iframe.is(':visible')) {
                    // Checking whether Taskdoc Item having a generated report from screen or not
                    if (taskDockMenuBreadCrumbManager.getScreenId() === taskDockMenuBreadCrumbManager.constants.REPORT_SCREEN_ID) {
                        taskDockMenuBreadCrumbManager.setScreenId(taskDockMenuBreadCrumbManager.constants.REPORT_SCREEN_HELP);
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
        },

        /**
         * @name activeScreenCount
         * @description Returns a count of how many active windows there are
         * @public
         * @return {number} The count of how many active windows there currently are
         */
        activeScreenCount: function () {
            return $('#dvWindows').children().length;
        }
    };

    return _public;
};

var recentWindowsMenu;
$(function () {
    recentWindowsMenu = new RecentWindowsMenu();
});




