/* Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

/**
 * Requires global (or parent scoped) variables:
 * sg.utls.localStorageKeys.RECENT_WINDOWS_BASE
 * LOGGED_IN_TENANT_CONST
 * LOGGED_IN_USERNAME_CONST
 * menuUrlList
 * screenId
 * reportScreenHelp
 */

var recentWindowsMenu = recentWindowsMenu || {};
recentWindowsMenu = {
    RECENTWINDOWS_DIV_SELECTOR_CONST: '#recentWindowManager > div',
    DV_RECENTWINDOWS_SELECTOR_CONST: "#dvRecentWindows",
    DV_RECENTWINDOWS_SPAN_SELECTOR_CONST: "#dvRecentWindows span",
    DATAMENUID_SELECTOR_CONST: 'data-menuid',
    PARENTID_SELECTOR_CONST: 'data-parentid',
    IS_REPORT_IDX_CONST: "ReportViewer.aspx?token",

    populateRecentWindow: function ($iframe, menuid, parentid, targetUrl, windowText) {
        var url = $iframe.attr("src");
        var isReport = url.indexOf(recentWindowsMenu.IS_REPORT_IDX_CONST) > 0;

        // Only modify recent windows list if it is not visible.
        if (!$(recentWindowsMenu.RECENTWINDOWS_DIV_SELECTOR_CONST).is(':visible')
            && !isReport) {
            // Element for recent window without the 'x' button at the end.
            // 'RW' in the id of div stands for Recent Window to distinguish 
            // from Open Window
            var $divRecentWindow = $('<div id="dvRW' +
                $iframe.attr('id') +
                '" class = "rcbox"> <span data-menuid="' +
                menuid +
                '" data-parentid="' +
                parentid +
                '" data-url="' +
                targetUrl +
                '" frameId="' +
                $iframe.attr('id') +
                '" command="Add" rank="1">' +
                windowText +
                '</span></div>');

            recentWindowsMenu.removeRecentWindowsDuplicatedOrAboveLimit(menuid);

            $(recentWindowsMenu.DV_RECENTWINDOWS_SELECTOR_CONST).prepend($divRecentWindow);

            recentWindowsMenu.storeRecentWindowToLocalStorageForUser();
        }
    },

    onLoadPopulateRecentWindowsListFromStorage: function () {
        // On first sign-in when recent windows is not populated.
        var recentWindowHtmlVal =
            sage.cache.getLocalStorage(recentWindowsMenu.getRecentWindowKey());
        $(recentWindowsMenu.DV_RECENTWINDOWS_SELECTOR_CONST).html(recentWindowHtmlVal);

        // remove non-permitted menu items
        recentWindowsMenu.removeNonPermittedItems();
        // Clear up excess windows if recent window size is configured smaller
        recentWindowsMenu.removeRecentWindowsDuplicatedOrAboveLimit('');
    },

    getRecentWindowKey: function () {
        // recent window storage key name <base-><user>-<tenant>
        return sg.utls.localStorageKeys.RECENT_WINDOWS_BASE
            + LOGGED_IN_TENANT_CONST + "-"
            + LOGGED_IN_USERNAME_CONST;
    },

    storeRecentWindowToLocalStorageForUser: function() {
        // store recent windows list html value to localstorage
        var recentWindows = $(recentWindowsMenu.DV_RECENTWINDOWS_SELECTOR_CONST).html();
        sage.cache.setLocalStorage(recentWindowsMenu.getRecentWindowKey(), recentWindows);
    },

    removeNonPermittedItems: function () {
        $(recentWindowsMenu.DV_RECENTWINDOWS_SPAN_SELECTOR_CONST).each(
            function(index, elem) {
                var currentMenuId = $($(elem).attr(recentWindowsMenu.DATAMENUID_SELECTOR_CONST));

                // Remove any recent windows over the limit
                if (currentMenuId) {
                    recentWindowsMenu.removeNonPermittedMenuItems("", elem, currentMenuId);
                }
            }
        );
    },

    /**
     * Removes Recent Windows excess menu items or duplicated if menuId param defined.
     * @param {string} menuId added to menu, if empty, removes excess items
     * @return none
     */
    removeRecentWindowsDuplicatedOrAboveLimit: function (menuId) {
        // calculate limit of recent windows to display, default is 10
        var recentWindowsLength = $(recentWindowsMenu.DV_RECENTWINDOWS_SPAN_SELECTOR_CONST).length;
        var recentWindowsLimit = RECENT_WIN_LIMIT_CONST;
        if (!recentWindowsLimit) {
            recentWindowsLimit = 10;
        }
        var recentWindowsToRemove = recentWindowsLength - recentWindowsLimit;

        // Remove items in reverse order from the list - FIFO
        $($(recentWindowsMenu.DV_RECENTWINDOWS_SPAN_SELECTOR_CONST).get().reverse()).each(
            function(index, elem) {
                var currentMenuId = $($(elem).attr(recentWindowsMenu.DATAMENUID_SELECTOR_CONST));

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
                    if (currentMenuId.selector === menuId) {
                        $(elem).closest("div").remove();
                        // breaking out of the .each call.
                        return false;
                    }
                }
            }
        );
    },

    /**
     * Removes Recent Windows non-permitted menu items
     * Requires menuUrlList parent scope variable
     * @param {string} menuId - menu id added to menu
     * @param {jquery fn} elem - the html element
     * @param {string} currentMenuId - current menuId from data attribute
     * @return true if elem removed, false otherwise
     */
    removeNonPermittedMenuItems: function (menuId, elem, currentMenuId) {
        // menuUrlList is a global var

        // Only on initial load.
        if ("" === menuId) {
            var currentParentId = $($(elem).attr(recentWindowsMenu.PARENTID_SELECTOR_CONST));

            // Remove screen name if user has no rights to it
            if (currentMenuId && currentParentId) {
                var permitted = false;

                // menuUrlList read from ViewBag. Iterate through list of allowed screens.
                for (var i = 0; i < menuUrlList.length; i++) {
                    if (menuUrlList[i].Data.MenuId === currentMenuId.selector &&
                        menuUrlList[i].Data.ParentMenuId === currentParentId.selector) {
                        permitted = true;
                        break;
                    }
                }
                // current recently used window entry not permitted
                if (!permitted) {
                    $(elem).closest("div").remove();
                    return true;
                }
            }
        }
        return false;
    },

    // UI actions

    hoverOn: function () {
        // screenId,reportScreenHelp is at global/parent scode

        if ($(recentWindowsMenu.DV_RECENTWINDOWS_SELECTOR_CONST).children().length > 0) {
            $(recentWindowsMenu.RECENTWINDOWS_DIV_SELECTOR_CONST).show();
        }

        //reset selection
        $(recentWindowsMenu.DV_RECENTWINDOWS_SPAN_SELECTOR_CONST).removeClass('selected');

        // Find Active Window and AddClass Selected
        $(recentWindowsMenu.DV_RECENTWINDOWS_SPAN_SELECTOR_CONST).each(function (index, elem) {
            var $iframe = $('#' + $(elem).attr('frameid'));
            if ($iframe.is(':visible')) {
                //Checking whether Taskdoc Item having a generated report from screen or not
                if (screenId === reportScreenId) {
                    screenId = reportScreenHelp;
                }
                return false;
            }
        });
    },

    hoverOff: function() {
        $(recentWindowsMenu.RECENTWINDOWS_DIV_SELECTOR_CONST).hide();
    },

    onClick: function() {
        // screenId is at global/parent scode

        // TODO: Requires further refactor of TaskDock-Menu-BreadCrumb.js
        // to deal with function calls within the document ready scope:
        // isMaxScreenNumReachedAndNotOpen, loadBreadCrumb, clearIframes, assignUrl
    }
}
