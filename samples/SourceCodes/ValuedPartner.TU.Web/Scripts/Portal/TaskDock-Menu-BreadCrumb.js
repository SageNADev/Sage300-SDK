/* Copyright (c) 1994-2022 Sage Software, Inc.  All rights reserved. */

"use strict";

var TaskDockMenuBreadCrumbManager = function () {

    // Constants
    var constants = Object.freeze({
        MAXIMUM_ALLOWABLE_ACTIVE_WINDOWS: Number($('#hdnNumberOfActiveWindows').val()),

        // For homepage help screen
        DEFAULT_SCREENID: 0,

        // Inquiry Screen Id for Help purposes
        INQUIRY_SCREENID: 2,

        // Global Search Screen default Id
        // Type is string so that the value can be 
        // compared as an attribute in RecentWindow.js
        GLOBALSEARCH_SCREENID: "3",

        REPORT_SCREEN_HELP: 1,

        REPORT_SCREEN_ID: " "
    });

    // Private Variables and Properties
    var targetUrl = "#";
    var isIframeClose = false;
    var currentRank;
    var controls = [];
    var firstControl = {};
    var currentIframe = "";
    var currentIframeId = "";
    var flag = false;
    var currentDiv = "";
    var isReload = true;
    var iFrameUrl;
    var isKPI = false;
    var kpiReportName;
    var pluginMenuMerged = false;
    var widgetUI = widgetUI || {};

    // Private Properties

    // Screen Id hold value for each menu screen menu Id
    var _screenId = -1;

    // To be used to record extra parameter in order to screen with specific record
    var _globalSearchDrillDownParameter = null;

    // Private Functions

    /**
     * @name hideIframes
     * @description Hide Iframe
     * @private
     */
    function hideIframes() {
        $('#screenLayout').children().find("iframe").each(function () {
            $(this).hide();
        });
    }

    /**
     * @name helpSearchForMenuItem
     * @description Search for Help Menu
     * @private
     * @param {any} id - TODO Add description
     * 
     */
    function helpSearchForMenuItem(id) {
        var data = { screenId: id };
        var helpUrl = sg.utls.url.buildUrl("Core", "Help", "Index");
        var key = helpUrl + "_" + id + "_" + globalResource.Culture;
        var callBack = function (result) { 
            if ($("#topMenu").data("kendoMenu")) 
                $("#topMenu").data("kendoMenu").destroy();
            $('#searchHelpDiv').html(result); 
            var menu = $("#topMenu").kendoMenu({ openOnClick: true, closeOnClick: true }).data("kendoMenu");
            
            menu.bind("select", function(e) {
                
                if (e.item.id == "globalSearch") {
                    // only open the global search if it is invoked by keyboard
                    if (e.sender._keyTriggered) {
                        $("#globalSearch").triggerHandler( "click" );
                         $("#topMenu").data("kendoMenu").close();
                    }
                }
                else if (e.item.id == "topMenuTools") {
                    e.preventDefault();
                    $('.quick-menu').toggle();
                    $(this).parent('.menu-item-with-icon').toggleClass('inactive');
                }
                else 
                {
                    // special handling of the help menu
                    if (e.item.parentElement.id === 'searchHelpDiv') {
                        
                        let searchBox = $(e.item).find('#helpSearchfl');
                        
                        if (searchBox.length > 0) {
                            
                            menu.setOptions({closeOnClick: false });
                            
                            searchBox.focus();
                            $("#helpSearchfl").keydown(function handleSearch( event ) {
                                if ( event.which == 13 ) {
                                    event.preventDefault();
                                    $("#helpSearchbtn").click();
                                    $("#topMenu").data("kendoMenu").close();
                                }
                            });
                        }
                        else {
                            if ($(e.item).find('#featureTourLink').length > 0) {
                                $("#featureTourLink").triggerHandler( "click" );
                            }
                            else {
                                let url = $(e.item).find("a").attr('href');
                                if (url) {
                                    e.preventDefault();
                                    let win = window.open(url, '_blank');
                                    if (win) {
                                        win.focus();
                                    }
                                }
                            }
                        }
                    }
                }
            });
        };

        sg.utls.ajaxCachePostHtml(helpUrl, data, callBack, key);
    }

    /**
     * @name ShowHomePage
     * @description Show Homepage
     * @private
     */
    function ShowHomePage() {
        window.scrollTo(0, 0);
        $('html').addClass('home-page');
        $('#homeNav > a').addClass('active');
        $('#screenLayout').hide();
        $('#widgetlayout').show();
        $('#widgetHplayout').hide();

        // When footer logo is clicked
        _public.setScreenId(constants.DEFAULT_SCREENID);
        

        $('#breadcrumb').hide();

        updateLayout(true);

        if (!$('#screenLayout').is(":visible")) {

            if ($('#widgetlayout').is(":visible")) {
                $('#widgetHplayout').hide();
            }
        }
    }

    /**
     * @name anyWidgetsVisible
     * @description Determine if any widgets are visible
     * @private
     * @returns {boolean} true : widgets are visible
     *                    false : widgets are not visible
     */
    function anyWidgetsVisible() {
        var visible = false;
        $(".bodyWidgetContainer > div").each(function () {
            if ($(this).find("iframe").attr('src').length > 0) {
                visible = true;

                // This return statement simply short-circuits 
                // the .each() loop
                return false;
            }
        });
        return visible;
    }

    /**
     * @name showCorrectLayout
     * @description Set Correct Layout 
     * @private
     */
    function showCorrectLayout() {
        if (controls.length === 1) {
            if (anyWidgetsVisible()) {
                $('#widgetLayout').show();
                $('#widgetHplayout').hide();
                $('#dvAddWidget').show();
                $('#breadcrumb').hide();
            } else {
                $('#widgetHplayout').show();
                $('#widgetLayout').hide();
                $('#dvAddWidget').hide();
            }
        }
    }

    /**
     * @name initializeMainMenu
     * @description Initialize Main Menu
     * @private
     */
    function initializeMainMenu() {
        var $menu = $(".side-nav .std-menu");
        var $subLevelMenu = $(".side-nav .std-menu ul");

        $menu.menuAim({
            activate: function (row) {
                // close submenus opened by keyboard
                $(row).parent().find('li').each(function () {
                    deactivateMenu(this);
                });

                activateMenu(row);
            },
            deactivate: function (row) {
                deactivateMenu(row);
            }
        });

        $subLevelMenu.menuAim({
            activate: function (row) {
                activateSubMenu(row);
            },
            deactivate: function (row) {
                deactivateSubMenu(row);
            },
            exitMenu: function (row) {
                // To deactivate current row
                return true;
            }
        });

        $(".side-nav .std-menu li").click(function (e) {
            e.stopPropagation();
            window.scrollTo(0, 0);
        });

        $('.side-nav .menu-item').children('label, a, .nav-icon').click(function () {
            toggleTopMenu(this);
        });
    }

    /**
     * @name toggleTopMenu
     * @description Highlight/Unhighlight the selected menu item and open/close the submenu. Unhighlight and close other submenus in the same top level menu.
     * @private
     * @param {any} link <a> element in menu item
     */
    function toggleTopMenu(link) {
        $(link).parent('.menu-item').toggleClass('open').find('.std-menu').toggleClass('active');
        $(link).parent('.menu-item').siblings().removeClass('open').find('.std-menu').removeClass('active');
    }

    /**
     * @name getFirstMenuItem
     * @description Get the first child menu item
     * @private
     * @param {any} parent <li> element menu item
     * @param {boolean} isSubMenu is sub menu
     * @returns {any} first child <li> menu item
     */
    function getFirstMenuItem(parent, isSubMenu) {
        let first = parent.find('li').first();
        // skip header li
        if (isSubMenu && first.hasClass("sub-heading")) {
            first = first.next();
        }
        return first;
    }

    /**
     * @name getLastMenuItem
     * @description Get the last child menu item
     * @private
     * @param {any} parent <li> element menu item
     * @returns {any} last child <li> menu item
     */
    function getLastMenuItem(parent) {
        return parent.children('li').last();
    }

    /**
     * @name getActiveMenuItem
     * @description Get the active child menu item, or the first if no child active menu item
     * @private
     * @param {any} parent <li> element menu item
     * @returns {any} active child <li> menu item
     */
    function getActiveMenuItem(parent, isSubMenu) {
        let active = parent.find('li > a.active').first().parent();
        if (active.length === 0) {
            active = getFirstMenuItem(parent, isSubMenu);
        }
        return active;
    }

    /**
     * @name getPreviousMenuItem
     * @description Get the previous menu item in the current level of the selected menu item. If current is the first, get the last menu item
     * @private
     * @param {any} current <li> element menu item
     * @param {boolean} isSubMenu is sub menu
     * @returns {any} previous <li> menu item
     */
    function getPreviousMenuItem(current, isSubMenu) {
        let prev = current.prevAll('li').first();
        // skip header li
        // if first item in the list, go to last
        if ((isSubMenu && prev.hasClass("sub-heading"))
            || prev.length === 0) {
            prev = getLastMenuItem(current.parent());
        }
        return prev;
    }

    /**
     * @name getNextMenuItem
     * @description Get the next menu item in the current level of the selected menu item. If current is the last, get the first menu item
     * @private
     * @param {any} current <li> element menu item
     * @param {boolean} isSubMenu is sub menu
     * @returns {any} next <li> menu item
     */
    function getNextMenuItem(current, isSubMenu) {
        let next = current.nextAll('li').first();
        // if last item in list, go to first
        if (next.length === 0) {
            next = getFirstMenuItem(current.parent(), isSubMenu);
        }
        return next;
    }

    /**
     * @name getParentMenuItem
     * @description Get parent menu item
     * @private
     * @param {any} current <li> element menu item
     * @returns {any} parent <li> menu item
     */
    function getParentMenuItem(current) {
        return current.parent().closest('li');
    }

    /**
     * @name getMenuLink
     * @description Get link in menu item
     * @private
     * @param {any} current <li> element menu item
     * @returns {any} <a> element in menu item
     */
    function getMenuLink(current) {
        return current.find('a').first()[0];
    }

    /**
     * @name disableDefaultMenuKeys
     * @description Disables keys to prevent scrolling while in the menu
     * @private
     * @param {any} e keypress event
     */
    function disableDefaultMenuKeys(e) {
        // disable default scrolling behaviour
        if (e.keyCode == sg.constants.KeyCodeEnum.Home
            || e.keyCode == sg.constants.KeyCodeEnum.End
            || e.keyCode == sg.constants.KeyCodeEnum.Enter
            || e.keyCode == sg.constants.KeyCodeEnum.Space
            || e.keyCode == sg.constants.KeyCodeEnum.LeftArrow
            || e.keyCode == sg.constants.KeyCodeEnum.RightArrow
            || e.keyCode == sg.constants.KeyCodeEnum.UpArrow
            || e.keyCode == sg.constants.KeyCodeEnum.DownArrow) {
            e.preventDefault();
        }
    }

    /**
     * @name activateMenu
     * @description 
     * @private
     * @param {any} row <li> element menu item
     */
    function activateMenu(row) {
        var $row = $(row);

        // Move on to the first menu (non-header) item if user is moused to header
        if ($row.hasClass("sub-heading")) {
            $row = $row.next();
        }
        $row.find(".sub-menu-wrap").first().show();

        // Clear the possibility of first menu to be active
        $row.siblings("li:not(.sub-heading)").first().find('a:first').removeClass('active');
        $row.find("a:first").addClass("active");
    }

    /**
     * @name deactivateMenu
     * @description
     * @private
     * @param {any} row <li> element menu item
     */
    function deactivateMenu(row) {
        var $row = $(row);

        // move on to the first menu (non-header) item if user is moused to header
        if ($row.hasClass("sub-heading")) {
            $row = $row.next();
        }
        $row.find(".sub-menu-wrap").first().hide();
        $row.find("a:first").removeClass('active');
    }

    /**
     * @name activateSubMenu
     * @description
     * @private
     * @param {any} row <li> element menu item
     */
    function activateSubMenu(row) {
        var $submenu = $(row).find(".sub-menu-wrap").first();
        $submenu.addClass("child");

        if ($(row).find("div:first").length) {
            $(row).find("a:first").addClass("active");
        }

        $submenu.show();
    }

    /**
     * @name deactivateSubMenu
     * @description
     * @private
     * @param {any} row <li> element menu item
     */
    function deactivateSubMenu(row) {
        var $submenu = $(row).find(".sub-menu-wrap").first();
        $submenu.hide();
        $submenu.removeClass("child");
        $(row).find("a:first").removeClass("active");
    }

    /**
     * @name hasValidLink
     * @description Checks if link has navigation
     * @private
     * @param {any} link <a> element in menu item
     * @returns {boolean} true if link has navigation
     */
    function hasValidLink(link) {
        const attr = $(link).attr('data-url');
        return (typeof attr !== 'undefined' && attr !== false && attr.length > 0)
            || $(link).parent()[0].id == 'homeNav'; // home has no url, but goes to homepage
    }

    /**
     * @name initMenuHotkeys
     * @description Initialize menu hot keys
     * @private
     */
    function initMenuHotkeys() {
        const topMenu = $('.side-nav > div > ul > li > a');
        const firstLevelMenu = $('.side-nav .std-menu > li > a');
        const secondLevelMenu = $('.side-nav .k-item > a')

        // Top level menu
        topMenu.keydown((e) => {
            disableDefaultMenuKeys(e);

            const current = $(e.target).parent(); // li element
            let next = undefined; // link inside li element
            switch (e.keyCode) {
                case sg.constants.KeyCodeEnum.DownArrow:
                    next = getMenuLink(getNextMenuItem(current));
                    break;
                case sg.constants.KeyCodeEnum.UpArrow:
                    next = getMenuLink(getPreviousMenuItem(current));
                    break;
                case sg.constants.KeyCodeEnum.Home:
                    next = getMenuLink(getFirstMenuItem(current.parent()));
                    break;
                case sg.constants.KeyCodeEnum.End:
                    next = getMenuLink(getLastMenuItem(current.parent()));
                    break;
                case sg.constants.KeyCodeEnum.RightArrow:
                case sg.constants.KeyCodeEnum.Enter:
                case sg.constants.KeyCodeEnum.Space:
                    const currentLink = getMenuLink(current);
                    if (hasValidLink(currentLink)) {
                        currentLink.click();
                    }
                    else {
                        // go to active or first item in 1st level menu
                        $('.side-nav').addClass('active');
                        toggleTopMenu(currentLink);
                        next = getActiveMenuItem(current, true);
                        activateMenu(next);
                        next = getMenuLink(next);
                    }
                    break;
                default:
                    break;
            }

            // focus on next item
            if (next !== undefined) {
                next.focus();
            }
        });

        // 1st level menu
        firstLevelMenu.keydown((e) => {
            disableDefaultMenuKeys(e);

            const current = $(e.target).parent(); // li element
            let next = undefined; // link inside li element
            switch (e.keyCode) {
                case sg.constants.KeyCodeEnum.DownArrow:
                    next = getMenuLink(getNextMenuItem(current, true));
                    break;
                case sg.constants.KeyCodeEnum.UpArrow:
                    next = getMenuLink(getPreviousMenuItem(current, true));
                    break;
                case sg.constants.KeyCodeEnum.LeftArrow:
                case sg.constants.KeyCodeEnum.ESC:
                    // go to parent top level menu
                    next = getMenuLink(getParentMenuItem(current));
                    toggleTopMenu(next);
                    break;
                case sg.constants.KeyCodeEnum.Home:
                    next = getMenuLink(getFirstMenuItem(current.parent(), true));
                    break;
                case sg.constants.KeyCodeEnum.End:
                    next = getMenuLink(getLastMenuItem(current.parent()));
                    break;
                case sg.constants.KeyCodeEnum.RightArrow:
                case sg.constants.KeyCodeEnum.Enter:
                case sg.constants.KeyCodeEnum.Space:
                    const currentLink = getMenuLink(current);
                    if (hasValidLink(currentLink)) {
                        currentLink.click();
                    }
                    else {
                        // go to active or first item in 2st level menu and close other submenus
                        current.parent().find('li').each(function () {
                            deactivateMenu(this);
                        });
                        activateMenu(current);
                        next = getActiveMenuItem(current, true);
                        deactivateSubMenu(next);
                        next = getMenuLink(next);
                    }
                    break;
                default:
                    break;
            }

            // focus on next item
            if (next !== undefined) {
                next.focus();
            }
        });

        // 2nd level menu
        secondLevelMenu.keydown((e) => {
            disableDefaultMenuKeys(e);

            const current = $(e.target).parent(); // li element
            let next = undefined; // link inside li element
            switch (e.keyCode) {
                case sg.constants.KeyCodeEnum.DownArrow:
                    next = getMenuLink(getNextMenuItem(current, true));
                    break;
                case sg.constants.KeyCodeEnum.UpArrow:
                    next = getMenuLink(getPreviousMenuItem(current, true));
                    break;
                case sg.constants.KeyCodeEnum.LeftArrow:
                    // go to parent 1st level menu
                    deactivateSubMenu(current);
                    next = getMenuLink(getParentMenuItem(current));
                    break;
                case sg.constants.KeyCodeEnum.Home:
                    next = getMenuLink(getFirstMenuItem(current.parent(), true));
                    break;
                case sg.constants.KeyCodeEnum.End:
                    next = getMenuLink(getLastMenuItem(current.parent()));
                    break;
                case sg.constants.KeyCodeEnum.Enter:
                case sg.constants.KeyCodeEnum.Space:
                    // open page and close menus
                    getMenuLink(current).click();
                    break;
                case sg.constants.KeyCodeEnum.ESC:
                    // go to parent top level menu
                    deactivateSubMenu(current);
                    next = getMenuLink(getParentMenuItem(getParentMenuItem(current)));
                    toggleTopMenu(next);
                    break;
                default:
                    break;
            }

            // focus on next item
            if (next !== undefined) {
                next.focus();
            }
        });
    }

    /**
     * @name iFrameLoadEvent
     * @description Load Iframe
     * @private
     * @param {any} e - TODO Add description
     * @param {any} $iframe - TODO Add description
     */
    function iFrameLoadEvent(e, $iframe) {
        var iframeId = $iframe.attr('id');
        var isCurrentIframe = iframeId === currentIframeId;

        if (isIframeClose && isCurrentIframe) {

            // Close the screen
            if (!($('#widgetLayout').is(":visible"))) {
                $('#' + currentIframeId).hide();
            }
            if (!flag) {

                // Remove task from the window
                $("#" + $("#" + currentDiv + "").attr('id') + "").remove();

                if ($('#dvWindows').children().length > 0) {

                    // Keep the task window open
                    $("#windowManager > div").show();

                    // Display number of open tasks
                    $('#spWindowCount').text($('#dvWindows').children().length);

                } else {
                    // Close the task window
                    $("#windowManager > div").hide();
                    $('#windowManager').removeClass("zeroTaskCount");

                    // Hide the breadcrumb
                    $('#breadcrumb').hide();

                    $('#spWindowCount').text("0");
                    _public.setScreenId(constants.DEFAULT_SCREENID);

                    $('#screenLayout').hide();

                    // Show background
                    $('html').addClass('home-page');

                    // Show home button as activated
                    $('#homeNav').addClass('active').children("a").addClass('active');
                }

                // Breadcrumb - Load breadcrumb on window management item removal  
                var lastID = $("#dvWindows span[controltoremove]").last().attr("frameId");

                if (currentIframeId === lastID) {
                    var secondLastParentID = $("#dvWindows span[controltoremove]").eq(-2).data("parentid");
                    loadBreadCrumb(secondLastParentID);
                } else {
                    var getLastWindowParentID = $("#dvWindows span[controltoremove]").last().data("parentid");
                    loadBreadCrumb(getLastWindowParentID);
                }

                $.each(controls, function (index, element) {
                    flag = true;
                    if (!$('#widgetLayout').is(":visible"))
                        $('#' + element["control"]).hide();
                    if (parseInt(element["rank"], 10) === currentRank) {
                        element["rank"] = 0;
                    }
                    if (parseInt(element["rank"], 10) !== 0 && parseInt(element["rank"], 10) > currentRank) {
                        element["rank"]--;
                    }
                    $('#' + element["control"]).attr("rank", element["rank"]);

                    if (parseInt(element["rank"], 10) === 1) {
                        if (element["control"] != 'widgetLayout')
                            $('#' + element["control"]).show();
                    }
                });

                $.each(controls, function (index, element) {
                    if (element) {
                        if (parseInt(element["rank"], 10) === 0) {
                            controls.splice(index, 1);
                        }
                    }
                });

                // This is to keep active screen selection in the Open Windows popup
                $('#windowManager').mouseenter();
                showCorrectLayout();
                updateLayout(false);
            }

            if (e) {
                e.preventDefault();
            }
        }
    }

    /**
     * @name taskAdded
     * @description Display 'Window opening...' popup in the Window Manager
     * @private
     */
    function taskAdded() {
        $(".task_added").show().css({ "right": "-29px" }).animate({ "right": "77px" }, "1500");
        $(".task_added").delay(1800).css({ "right": "77px" }).animate({ "right": "-29px" }, "3000").fadeOut();
    }

    /**
     * @name assignUrl
     * @description - Assign URL
     * @param {string} windowText - Window Title
     * @param {string} parentid - Parent ID
     * @param {string} menuid - Menu ID
     * @param {bool} isExcludingParameters - Determine if parameters are excluded
     */
    function assignUrl(windowText, parentid, menuid, isExcludingParameters) {
        var control = {};
        var isIframeOpen = false;
        var isScreenOpen = false;
        var iframeSrc = "";

        if ($('#widgetHplayout').is(":visible")) {
            $('#widgetHplayout').hide();
        }
        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");

            iframeSrc = $iframe.attr("src");
            var iframeSrcPath = sg.utls.getUrlPath(iframeSrc);
            var targetPath = sg.utls.getUrlPath(targetUrl);

            if (isExcludingParameters && iframeSrcPath === targetPath) {
                // Compare the full URL including the query string.
                // now that we know the path is the same, check for url with parameter, 
                // if they are not the same, lets refresh the screen and display that iframe

                if (_globalSearchDrillDownParameter !== null) {
                    $iframe.attr("src", targetUrl + "?" + _globalSearchDrillDownParameter);
                    _globalSearchDrillDownParameter = null;
                } else if (iframeSrc !== targetUrl) {
                    $iframe.attr("src", targetUrl);
                }
                $iframe.show();
                isScreenOpen = true;

                // Do not display more than one frame.
                return false;
            }

            // Compare the full URL including the query string.
            else if (iframeSrc === targetUrl) {
                $iframe.show();
                isScreenOpen = true;

                // Do not display more than one frame.
                return false;
            }
        });

        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            iframeSrc = $iframe.attr("src");

            var isIframeSrcBlank = iframeSrc === '';
            var isIframeVisible = $iframe.is(':visible');

            if (!isIframeVisible && isIframeSrcBlank && !isIframeOpen && !isScreenOpen) {
                isIframeOpen = true;
                isIframeClose = false;
                $iframe.addClass('screenLoading');
                $iframe.contents().find('body').html('');

                if (_globalSearchDrillDownParameter !== null) {
                    $iframe.attr("src", targetUrl + "?" + _globalSearchDrillDownParameter);
                    _globalSearchDrillDownParameter = null;
                } else {
                    $iframe.attr("src", targetUrl);
                }

                $iframe.on("load", function () {
                    // remove the loading/spinner after the page is loaded
                    $(this).removeClass('screenLoading');
                    initFormSize();
                });
                $iframe.show();

                $.each(controls, function (index, element) {
                    element["rank"]++;
                    $('#' + element["control"]).attr("rank", element["rank"]);
                });
                control["control"] = $iframe.attr('id');
                currentIframe = $iframe.attr('id');
                control["rank"] = 1;
                controls.push(control);
                $('#dvWindows > div').each(function () { $iframe.find("span").removeClass('selected'); });

                $('#dvWindows').find('span').each(function () {
                    if ($(this).attr('command') === 'Add') {
                        $(this).attr("rank", (parseInt($(this).attr("rank"), 10) + 1));
                    }
                });

                // this is the HTML for label in the window manager
                var $divWindow = $('<div id="dv' + $iframe.attr('id') + '" class = "rcbox"> <span class = "selected" data-menuid="' + menuid +
                    '" data-parentid="' + parentid + '" frameId="' + $iframe.attr('id') + '" command="Add" rank="1">' + windowText +
                    '</span><span data-parentid="' + parentid + '" frameId="' + $iframe.attr('id') + '" command="Remove" controlToRemove="dv' + $iframe.attr('id') + '"></span></div>');
                $('#dvWindows').append($divWindow);

                recentWindowsMenu.populateRecentWindow($iframe, menuid, parentid, targetUrl, windowText);

                $('#spWindowCount').text($('#dvWindows').children().length);
                taskAdded();

                $('html').removeClass('home-page');

                window.scrollTo(0, 0);

                // Called help according to screenId i.e menuid
                // Checking the Taskdoc having a generated Report Screen or not
                if (_screenId !== constants.REPORT_SCREEN_ID) {
                    _public.setScreenId(menuid);

                } else {
                    _public.setScreenId(constants.REPORT_SCREEN_HELP);
                }
            }
        });
    }

    /**
     * @name isMaxScreenNumReachedAndNotOpen
     * @description Determine if the maximum screen number is reached and the screen is not open
     * @private
     * @param {string} targetUrl - TODO Add description
     * @returns {boolean} true | false
     */
    function isMaxScreenNumReachedAndNotOpen(targetUrl) {

        // Check if maximum number of screens reached
        var isScreenOpen = isScreenAlreadyOpen(targetUrl);
        var activeScreenCount = recentWindowsMenu.activeScreenCount();
        if (!isScreenOpen && activeScreenCount >= constants.MAXIMUM_ALLOWABLE_ACTIVE_WINDOWS) {
            sg.utls.showMessageInfo(sg.utls.msgType.INFO, portalBehaviourResources.MaxWindowExceeded);
            return true;
        }
        return false;
    }

    /**
     * @name screenLauncher
     * @description Launch Screen
     * @param {any} event - TODO Add description
     */
    function screenLauncher(event) {
        // Remove home button activated style
        $('#homeNav').removeClass('active').children("a").removeClass('active');
        $('html').removeClass('home-page');

        // Try close the widget add/remove menu no matter what
        $(".container_popUp.Widget.widgetList").hide();

        // Sage Intelligence
        var intelligence = $(this).attr("data-isIntelligence");
        var fileurl = $(this).attr("data-url");

        if (intelligence == "True") {
            isReload = false;
            event.preventDefault();
            var url = sg.utls.url.buildUrl("Core", "Home", "Download");
            window.open(url + "?file=" + fileurl, "_self");
        } else {
            $('#screenLayout').show();
            $('#widgetLayout').hide();
            window.scrollTo(0, 0);

            if ($(event.target).data('url') != " ")
                targetUrl = $(event.target).data('url');

            var isScreenOpen = isScreenAlreadyOpen(targetUrl);
            var isMaxScreenCountReachedAndNotOpen = isMaxScreenNumReachedAndNotOpen(targetUrl);
            if (isScreenOpen || isMaxScreenCountReachedAndNotOpen) {
                return;
            }

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            // Load breadcrumb on menu item click and add item to windows dock
            var parentidVal = $(this).data('parentid');

            // Get menu id as screenId for Help Menu Search
            _public.setScreenId($(this).attr("data-menuid"));


            // screen could be launched from level 2 menu from an "li", so keep searching
            if (!_screenId && $(this).is("li")) {
                _public.setScreenId($(this).find("a").attr("data-menuid"));
            }

            loadBreadCrumb(parentidVal);

            var windowtext = $(event.target).text();

            $('#dvWindows > div').each(function () { $(this).find("span").removeClass('selected'); });

            if ($(this).attr("data-modulename") && $.parseHTML($(this).attr("data-modulename")) != null && $(this).attr("data-moduleName") != "null") {
                windowtext = portalBehaviourResources.PagetitleInManager.format($(this).attr("data-modulename"), $(event.target).text());
            } else if ($(this).find("a").attr("data-modulename")) {
                windowtext = portalBehaviourResources.PagetitleInManager.format($(this).find("a").attr("data-modulename"), $(event.target).text());
            }

            if ($(this).attr("data-isreport") === "true" || $(this).attr("data-isreport") === "True") {
                // windowtext = windowtext.indexOf("Report") < 0 ? windowtext + " " + portalBehaviourResources.Report : windowtext;
                windowtext = portalBehaviourResources.ReportNameTemplate.format(windowtext, portalBehaviourResources.Report);
            }

            if (targetUrl !== "") {
                if (_globalSearchDrillDownParameter === null) {
                    assignUrl(windowtext, parentidVal, _screenId);
                } else {
                    assignUrl(windowtext, parentidVal, _screenId, true);
                }
            }
        }
    }

    /**
     * @name loadBreadCrumb
     * @description Load Bread Crumb
     * @private
     * @param {any} parentidVal - TODO Add description
     */
    function loadBreadCrumb(parentidVal) {
        if (!$('#widgetLayout').is(":visible")) {
            var html = [];

            // Add Parent to array
            jQuery.each(MenuList, function (i, val) {
                if (val.Data.MenuId == parentidVal) {
                    var menuName = val.Data.MenuName;
                    if (menuName.indexOf("'") > -1) {
                        menuName = menuName.replace("'", "&#39;");
                    }
                    html = html + "<ul class=bc><li>" + menuName + "<span class=navigation-pipe>:</span></li>";
                }
            });

            // Add child items to array
            jQuery.each(MenuList, function (i, val) {
                if (val.Data.ParentMenuId == parentidVal && val.Data.IsGroupHeader == false && val.Data.HasSubGroup == false) {
                    var windowsDockTitle = val.Data.MenuName;
                    //   var title = val.Data.MenuName.length <= 25 ? val.Data.MenuName : val.Data.MenuName.substring(0, 25) + "...";
                    var screenurl = (val.Data.ScreenUrl == "N/A") ? "" : portalBehaviourResources.DomainUrl + val.Data.ScreenUrl;
                    var moduleName = (val.Data.ModuleName == 'null') ? "" : val.Data.ModuleName;
                    var isReport = (val.Data.isReport == 'null') ? "" : val.Data.IsReport;
                    html = html + ('<li><a data-parentid="' + val.Data.ParentMenuId + '" data-menuid="' + val.Data.MenuId + '" class="breadcrumb-page" data-url="' + screenurl + '" data-windocktitle="' + windowsDockTitle + '" data-moduleName="' + moduleName + '" data-isreport ="' + isReport + '">' + val.Data.MenuName + '</a></li>');
                }
            });

            html = html + ('</ul>');
            $('#breadcrumb').show();
            $('#breadcrumb').html(html);
            $("#breadcrumb:contains('Sequence contains no elements')").hide();

            // Breadcrumb more dropdown menu
            var postsArr = new Array();
            $('ul.bc').find('li').each(function () { postsArr.push($(this).html()); });
            var len = postsArr.length;
            var bcContent;

            if (len > 7) {
                for (var i = 0; i < len; i++) {
                    if (i == 0) bcContent = '<ul class="bc"><li>' + postsArr[i] + '</li>';
                    else if (i == 6) bcContent = bcContent + '<li class="innerdd">' + portalBehaviourResources.MoreItems + '<i class="downArrow"></i><ul><li>' + postsArr[i] + '</li>';
                    else if (i == (len - 1)) bcContent = bcContent + '<li>' + postsArr[i] + '</li></ul></li></ul>';
                    else bcContent = bcContent + '<li>' + postsArr[i] + '</li>';
                }
                $("#breadcrumb").html(bcContent);
            }

            $(".innerdd").on("mouseenter", function () {

                $(this).find("ul").show();
            }).on("mouseleave",
                function () {
                    $(this).find("ul").hide();
                });

            // Invoked from breadcrumb menu
            $('.breadcrumb-page').click(function () {
                var parentWidth = $(document).width();
                $('#screenLayout').children().each(function () {
                    var $iframe = $(this).find("iframe");
                    $iframe.width(parentWidth);
                });

                $('#screenLayout').show();
                $('#widgetLayout').hide();

                if ($.trim($(this).data('url')) != "") {
                    targetUrl = $(this).data('url');

                }

                var isScreenOpen = isScreenAlreadyOpen(targetUrl);
                var isMaxScreenCountReachedAndNotOpen = isMaxScreenNumReachedAndNotOpen(targetUrl);
                if (isScreenOpen || isMaxScreenCountReachedAndNotOpen) {
                    return;
                }

                if (activeScreenCountWithinMaximumAllowable()) {
                    hideIframes();
                }

                var title = $(this).attr("data-windocktitle");

                if ($(this).attr("data-moduleName") != "" && $(this).attr("data-moduleName") != "null") {
                    // title = $(this).attr("data-moduleName") + " " + $(this).attr("data-windocktitle");
                    title = portalBehaviourResources.PagetitleInManager.format($(this).attr("data-moduleName"), title);
                }

                var parentidVal = $(this).attr("data-parentid");

                // Get menu id as screenId for Help Menu Search
                _public.setScreenId($(this).attr("data-menuid"));


                $('#dvWindows > div').each(function () { $(this).find("span").removeClass('selected'); });

                if ($(this).attr("data-isreport") === "true" || $(this).attr("data-isreport") === "True") {
                    // title = title.indexOf("Report") < 0 ? title + " " + portalBehaviourResources.Report : title;
                    title = portalBehaviourResources.ReportNameTemplate.format(title, portalBehaviourResources.Report);
                }

                if (targetUrl.length > 0) {
                    assignUrl(title, parentidVal, _screenId);
                }
            });
        }
    }

    /**
     * @name isScreenAlreadyOpen
     * @description Determine whether or not a screen is currently open
     *              If open, the window will be activated (brought to the foreground)
     * @param {string} urlToTest - The url of the screen that we're attempting to open
     * @returns {boolean} true = Screen is currently open | false = Screen is not currently open
     */
    function isScreenAlreadyOpen(urlToTest) {
        let result = false;

        $('#screenLayout').children().each(function () {
            const $iframe = $(this).find("iframe");
            const frameId = $iframe[0].id;
            const srcUrl = $iframe.attr("src");
            const originalUrl = $iframe.prop("originalSrc");
            const urlMatch = srcUrl === urlToTest;

            const foundWindow1 = urlMatch && _globalSearchDrillDownParameter === null; 
            const foundWindow2 = (originalUrl != 'undefined' && originalUrl === urlToTest);

            result = foundWindow1 || foundWindow2;
            if (result) {
                // Activate the already opened window
                $(`#dvWindows span[command='Add'][frameid='${frameId}']`).trigger('click');

                // This will break us out of the .each() loop, 
                // not return from the function.
                return false;
            }
        });

        return result;
    }

    /**
     * @name receiveWindowMessage
     * @description Function to handle messages posted from other windows (e.g. from a web screen within
     *              a child IFrame).  These messages request that the portal take some action, such as
     *              opening a report or screen, or showing Notes for a given entity (e.g. an AR Customer).
     * @private
     * @param {any} evt - TODO Add description
     */
    function receiveWindowMessage(evt) {
        if (evt) {
            // For now we handle messages that contain strings (for opening a report or a
            // screen in a new task window) or objects (for other purposes such as showing
            // Notes for a given entity such as an AR Customer).
            // NOTE: To test that we have a string, we use typeof (in case the string happens
            //       to be ''), but to test for an object we need to use instanceof since it
            //       returns false for null and true for an object whereas typeof returns
            //       "object" for null as well as for objects.
            if (typeof evt.data === "string") {
                // We are being asked to open a report or screen as a new task window.
                openNewTask(evt.data);
            }
            else if (evt.data instanceof Object) {
                // Handle different types of messages that contain objects.  For now, the
                // only type we handle is for showing Notes, but in future we might handle
                // other types of messages too.
                if (evt.data.notesOptions) {
                    // We are being asked to show Notes for a given entity (e.g. an AR Customer).
                    openNotesCenter(evt.data.notesOptions);
                }
                // This is for closing the notes center.
                else if (evt.data.hideNotesCenter) {
                    hideNotesCenter();
                }
                // Populate multiselect widget on custom profile sliding window when a profile id is selected from UI profile popup
                else if (evt.data.populateCustomReportProfileIdsMultiSelectWidget) {
                    CustomReportUI.populateMultiSelectFromPopup(evt.data.populateCustomReportProfileIdsMultiSelectWidget);
                }
                // Refresh multiselect widget on custom profile UI when a profile id is added from the UI profile *screen*
                else if (evt.data.refreshCustomReportProfileIdsMultiSelectWidget) {
                    CustomReportUI.refreshMultiSelect();
                }
            }
        }
    }

    /**
     * @name createInquiryURLWithParameters
     * @description Create Inquiry URL with parameters
     * @private
     * @param {any} inquiryParameter - TODO add description
     * @returns {string} - TODO add description
     */
    function createInquiryURLWithParameters(inquiryParameter) {
        return sg.utls.formatString("{0}/?module={1}&inquiryType={2}&target={3}&value={4}&title={5}&name={6}",
            inquiryParameter.url, inquiryParameter.module, inquiryParameter.feature, inquiryParameter.target, inquiryParameter.value, encodeURIComponent(inquiryParameter.title), encodeURIComponent(inquiryParameter.name));
    }

    /**
     * @name createInquiryURLWithName
     * @description Create Inquiry URL with name
     * @private
     * @param {any} inquiryParameter - TODO add description
     * @returns {string} - TODO add description
     */
    function createInquiryURLWithName(inquiryParameter) {
        return sg.utls.formatString("{0}/?fileName={1}", inquiryParameter.url, inquiryParameter.fileName);
    }

    /**
     * @name openNewTask
     * @description Function to handle opening of Reports and Screen as a New Task Window.
     *              NOTE: The caller is responsible for checking that evtData (the data from the window
     *                    message presumably received from another IFrame) is a string.
     * @private
     * @param {any} evtData - TODO add description
     */
    function openNewTask(evtData) {

        var postMessageData;
        var screenName, parentId, menuid;

        // Display Reports as a New Task Doc Item
        if (evtData.indexOf("isInquiry") >= 0 || evtData.indexOf("isInquiryGeneral") >= 0) {
            postMessageData = evtData.split(" ");
            var parameter = JSON.parse(decodeURI(postMessageData[1]));
            parameter.title = kendo.htmlEncode(parameter.title);
            targetUrl = (evtData.indexOf("isInquiryGeneral") < 0) ? createInquiryURLWithParameters(parameter) : sg.utls.formatString("{0}/?templateId={1}&name={2}&dsId={3}", parameter.url, parameter.templateId, parameter.name, parameter.id);

            $('#screenLayout').show();
            $('#widgetLayout').hide();

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            // Do not show the breadcrumb for inquiry reports
            $('#breadcrumb').hide();

            postMessageData.splice(postMessageData.length - 1, 1);

            menuid = constants.INQUIRY_SCREENID;

            // Method To Load Into Task Doc
            assignUrl(portalBehaviourResources.Inquiry + " - " + parameter.title, parentId, menuid);

        } else if (evtData.indexOf("isReport") >= 0) {

            postMessageData = evtData.split(" ");
            targetUrl = postMessageData[1];
            postMessageData.splice(0, 2);

            var urlParser = $('<a>', { href: postMessageData[postMessageData.length - 1] })[0];
            var a = $("#listPrimary li > a[data-url='" + urlParser.pathname + "']");
            var isReport = a.data("isreport");

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            // Do not show the breadcrumb for printed reports
            $('#breadcrumb').hide();

            // Checking isKPI so that reportName is appended to show in windows doc when opened from KPI.
            var windowText;
            if (isKPI == true) {
                windowText = kendo.htmlEncode(kpiReportName);
                isKPI = false;
            } else {
                postMessageData.splice(postMessageData.length - 1, 1);
                windowText = kendo.htmlEncode(postMessageData.join(" "));
            }

            // only add Report to the window name if it is a report page and it does not have one
            if ((isReport === 'True' || isReport === 'true') && windowText.indexOf(portalBehaviourResources.Report) === -1) {
                windowText = portalBehaviourResources.ReportNameTemplate.format(windowText, portalBehaviourResources.Report);
            }
            windowText = windowText + " - " + portalBehaviourResources.Printed;

            menuid = constants.REPORT_SCREEN_HELP;

            // Method To Load Into Task Doc
            assignUrl(windowText, parentId, menuid);

            // Update Help Menu after Report Generated from a Screen
            _public.setScreenId(constants.REPORT_SCREEN_HELP);

        } else if (evtData.indexOf("isScreen") >= 0) {

            // Display Screen as a New Task Doc Item

            postMessageData = evtData.split(" ");
            targetUrl = postMessageData[1];

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            /*
             Sample targetUrl
             "/Sage300/OnPremise/AP/InvoiceEntry/Index?batchNumber=25&toAction=/Sage300/OnPremise/AP/InvoiceBatchList&actionType=EditBatch"
            */

            var parentScreenID;
            var screenID;
            var menuUrl = targetUrl.split("?")[0];

            // Get ScreenId, ParentScreenId and Menu Name for Requested Url
            jQuery.each(MenuList, function (i, val) {
                if (menuUrl.indexOf(val.Data.ScreenUrl) >= 0) {
                    screenID = val.Data.MenuId;
                    parentScreenID = val.Data.ParentMenuId;
                    screenName = val.Data.ModuleName == null ?
                        val.Data.MenuName :
                        portalBehaviourResources.PagetitleInManager.format(val.Data.ModuleName, val.Data.MenuName);
                }
            });

            loadBreadCrumb(parentScreenID);

            // Method To Load Into Task Doc
            assignUrl(screenName, parentScreenID, screenID);
        }
    }

    /**
     * @name openNotesCenter
     * @description Function to handle showing of Notes for a given entity (e.g. for an AR Customer).
     * @private
     * @param {any} options - TODO add description
     */
    function openNotesCenter(options) {
        // Open notes center for the given entity if this behavior is enabled and KN is
        // activated.
        // NOTE: Since there is no simple way in JS to test whether or not KN is activated,
        //       we will do an indirect test where we test for the existence of the
        //       notesCenterUI global variable.  This variable will only exist if the notes JS
        //       bundle was rendered, and that bundle is only rendered when KN is activated
        //       (as the Notes feature is only available when KN is activated).
        if (isNotesAutoLaunchEnabled && typeof notesCenterUI !== 'undefined' && notesCenterUI !== null) {
            notesCenterUI.launchNotesFromScreen(options);
        }
    }

    /**
     * @name hideNotesCenter
     * @description Function to handle hiding (closing) of Notes center.
     * @private
     */
    function hideNotesCenter() {
        // Hide notes center (or make sure it's hidden) for the given entity if this behavior is
        // enabled and KN is activated.
        // NOTE: Since there is no simple way in JS to test whether or not KN is activated,
        //       we will do an indirect test where we test for the existence of the
        //       notesCenterUI global variable.  This variable will only exist if the notes JS
        //       bundle was rendered, and that bundle is only rendered when KN is activated
        //       (as the Notes feature is only available when KN is activated).
        if (isNotesAutoLaunchEnabled && typeof notesCenterUI !== 'undefined' && notesCenterUI !== null) {
            notesCenterUI.hideNotesCenter();
        }
    }

    /**
     * @name resizeLayout
     * @description TODO - Add description
     * @private
     */
    function resizeLayout() {
        // Widgets layout container, widgets help layout container
        $('.body_container,#widgetHplayout').each(function () {
            var iframeHeight = $(window).height() - 184;
            $(this, '#widgetHplayout').css('min-height', iframeHeight);
        });

        // First time user layout, screen layout
        $('#firstTimeLogin').each(function () {
            var docHeight = $(document).height();
            $('#firstTimeLogin .overlay').css('min-height', docHeight);
        });
    }

    /**
     * @name openGlobalSearch
     * @description Open and display the Global Search page
     * @private
     * @param {any} result - TODO add description
     */
    function openGlobalSearch(result) {
        if (result && result.UserMessage && result.UserMessage.IsSuccess) {
            targetUrl = sg.utls.url.buildUrl("Core", "GlobalSearch", "Index");
            $(this).addClass('active');
            $('#screenLayout').show();
            $('#widgetLayout').hide();

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            $('#breadcrumb').hide();

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            // Method To Load Into Task Doc
            assignUrl(portalBehaviourResources.GlobalSearch, null, constants.GLOBALSEARCH_SCREENID);
        } else {
            sg.utls.showMessage(result);
        }
    }

    /**
    * @name onSDASuccess
    * @description Open SDA link
    * @private
    */
    function onSDASuccess(result) {
        if (result && !result.UserMessage) {
            window.open(result);
        } else {
            sg.utls.showMessage(result);
        }
    }

    /**
     * @name initSideMenu
     * @description Initialize Side Menu
     * @private
     */
    function initSideMenu() {
        sg.utls.getUserPreferences(menuUserPreferenceKey, function (result) {
            if (result) {
                if (result === "expanded") {
                    menuLayoutExpanded();
                }
                else {
                    menuLayoutCollapsed();
                }
            }
        });
    }

    /**
     * @name menuLayoutCollapsed
     * @description Layout for Side Menu Collapsed Status & User Preferences & showing Tooltips
     * @private
     */
    function menuLayoutCollapsed() {
        showMenuExpandButton();
        $('html').removeClass('page-expanded').addClass('page-collapsed');
        $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.std-menu.active').removeClass('active');

        sg.utls.saveUserPreferences(menuUserPreferenceKey, "collapsed");

        // Reload/refresh widget layout 
        updateLayout(false);

        // create tooltip when menu is closed
        if (!mainToolTip) {
            mainToolTip = $("#listPrimary").kendoTooltip({
                filter: "li.menu-item > div.nav-icon",
                content: kendo.template($("#tooltipTemplate").html()),
                width: 120,
                position: "right",
                animation: {
                    open: {
                        effects: "zoom"
                    }
                }
            }).data("kendoTooltip");
        }
    }

    /**
     * @name menuLayoutExpanded
     * @description Layout for Side Menu Expanded Status & User Preferences & hiding Tooptips
     * @private
     */
    function menuLayoutExpanded() {
        showMenuCollapseButton();
        $('html').removeClass('page-collapsed').addClass('page-expanded');
        $('#navbarSide').removeClass('side-nav-collapsed').addClass('active');

        sg.utls.saveUserPreferences(menuUserPreferenceKey, "expanded");

        // Reload/refresh widget layout 
        updateLayout(false);

        // remove tooltip when menu is expanded
        if (mainToolTip) {
            mainToolTip.destroy();
            mainToolTip = null;
        }
    }

    /**
     * @name showMenuExpandButton
     * @description Show Expand Button
     * @private
     */
    function showMenuExpandButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').show();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').hide();
    }

    /**
     * @name showMenuCollapseButton
     * @description Show Collapse Button
     * @private
     */
    function showMenuCollapseButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').hide();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').show();
    }

    /**
     * @name initFormSize
     * @description Initialize Form Size & Save User Preference
     * @private
     */
    function initFormSize() {
        sg.utls.getUserPreferences(formSizeUserPreferenceKey, function (result) {
            if (result) {
                if (result === "medium") {
                    formSizeMedium();
                }
                else if (result === "small") {
                    formSizeSmall();
                }
                else {
                    formSizeLarge();
                }
            }
        });
    }

    /**
     * @name formSizeHandler
     * @description To handle switch form size
     * @param {function} fn Function to show button
     * @param {string} size The size
     * @param {string} className The class name
     */
    function formSizeHandler(fn, size, className) {
        if (fn && typeof fn === "function") {
            fn();
        }

        $.each($('[id^="iFrameMenu"]').contents().find('html'), function (index, targetHTML) {
            var $targetHTML = $(targetHTML);
            if (!$targetHTML.attr(sg.utls.localFormSizeDataTag)) {
                // only apply class name if there no local form size can be found
                $targetHTML.removeClass('form-large form-medium form-small').addClass(className);
                // also select the local size select box
                var btnFormSizeUILevel = 'btnFormSizeUILevelLarge';
                switch (className) {
                    case 'form-large': btnFormSizeUILevel = 'btnFormSizeUILevelLarge';
                        break;
                    case 'form-medium': btnFormSizeUILevel = 'btnFormSizeUILevelMedium';
                        break;
                    case 'form-small': btnFormSizeUILevel = 'btnFormSizeUILevelSmall';
                        break;
                }
                var targetSelection = $targetHTML.find("#" + btnFormSizeUILevel);
                if (targetSelection.length > 0) {
                    targetSelection.siblings().removeClass("menu-active");
                    targetSelection.addClass("menu-active");
                }
            }
        });

        sg.utls.saveUserPreferences(formSizeUserPreferenceKey, size);
    }

    /**
     * @name formSizeLarge
     * @description Set Large Form Size
     * @private
     */
    function formSizeLarge() {
        formSizeHandler(showMenuLargeButton, 'large', 'form-large');
    }

    /**
     * @name formSizeMedium
     * @description Set Medium Form Size
     * @private
     */
    function formSizeMedium() {
        formSizeHandler(showMenuMediumButton, 'medium', 'form-medium');
    }

    /**
     * @name formSizeSmall
     * @description Set Small Form Size
     * @private
     */
    function formSizeSmall() {
        formSizeHandler(showMenuSmallButton, 'small', 'form-small');
    }

    /**
     * @name formSizeReset
     * @description Reset form size
     */
    function formSizeReset() {
        sg.utls.showConfirmationDialogYesNo(function () {
            sg.utls.deleteScreenLevelUserPreference();
            sg.utls.resetAllScreenSize();
            formSizeLarge();
        }, $.noop(), formSizeResetConfirmation, formSizeResetConfirmationTitle);
    }

    /**
     * @name showMenuLargeButton
     * @description Show an indicator (active status) when the Large Text Size menu is active
     * @private
     */
    function showMenuLargeButton() {
        $('#btnFormSizeMedium, #btnFormSizeSmall').removeClass('menu-active');
        $('#btnFormSizeLarge').addClass('menu-active');
    }

    /**
     * @name showMenuMediumButton
     * @description Show an indicator (active status) when the Medium Text Size menu is active
     * @private
     */
    function showMenuMediumButton() {
        $('#btnFormSizeLarge, #btnFormSizeSmall').removeClass('menu-active');
        $('#btnFormSizeMedium').addClass('menu-active');
    }

    /**
     * @name showMenuSmallButton
     * @description Show an indicator (active status) when the Small Text Size menu is active
     * @private
     */
    function showMenuSmallButton() {
        $('#btnFormSizeLarge, #btnFormSizeMedium').removeClass('menu-active');
        $('#btnFormSizeSmall').addClass('menu-active');
    }

    /**
     * @name activeScreenCountWithinMaximumAllowable
     * @description Is the current count of active screens within the
     *              allowable limit?
     * @private
     * @returns {boolean} true : within maximum allowable limit
     *                    false : outside maximum allowable limit
     */
    function activeScreenCountWithinMaximumAllowable() {
        var currentScreenCount = recentWindowsMenu.activeScreenCount();
        return currentScreenCount <= constants.MAXIMUM_ALLOWABLE_ACTIVE_WINDOWS;
    }

    // Publicly exposed methods, properties/variables
    var _public = {

        constants: constants,

        setDefaultScreenId: function () {
            _public.setScreenId(constants.DEFAULT_SCREENID);
        },

        init: function () {

            $('#logoSage300').focus();
            $('#searchHelpDiv').show();

            // Home button, Tools button active state
            $('#homeNav > a').addClass('active');
            $('#topMenuTools').addClass('active');

            sg.utls.loadCompanyColor();
            sg.utls.setBackgroundColor($("#header"));

            var isWidgetEmptyLnkClicked = false;

            var menu = $("#topMenu").kendoMenu({ openOnClick: true, closeOnClick: true }).data("kendoMenu");
            menu.bind('activate', function (e) {
                // This is to fix D-33845
                // For the menu item that contains a textbox, there is an Kendo issue on Chrome
                // The workaround is to focus on an input when menu is activated.
                //https://github.com/telerik/kendo-ui-core/issues/2524
                if (e.item.is('#helpMenu')) {
                    e.item.find('input').first().focus();
                }
            });

            _public.setScreenId(constants.DEFAULT_SCREENID);

            $("#topMenu").keydown(function (e) {
                if (e.key === 'Tab') {
                    let newKey = e.shiftKey ? 'ArrowLeft' : 'ArrowRight';
                    let newKeyCode = e.shiftKey ? 37 : 39;
                    let lastItem = $(e.target).children(".k-menu-item.k-state-focused.k-last")[0];
                    let firstItem = $(e.target).children(".k-menu-item.k-state-focused.k-first")[0];
                    if(!firstItem && !lastItem || lastItem && e.shiftKey || firstItem && !e.shiftKey) {
                        e.preventDefault();
                        $(e.target).trigger(
                            $.Event('keydown', {key: newKey, keyCode: newKeyCode, which: newKeyCode})
                        );
                    }
                }
            });

            showMenuExpandButton(); // by default: menu is collapsed, so show expand button

            $('#btnExpandMenu, #btnExpandMenuAlt').click(menuLayoutExpanded);
            $('#btnCollapseMenu, #btnCollapseMenuAlt').click(menuLayoutCollapsed);

            /* Initialize for Side Menu Setting (Collapsed/Expanded) */
            /* ---------------------------------------------------------- */

            initSideMenu();

            showMenuLargeButton(); // by default: form is large, so highlight large button

            $('#btnFormSizeLarge').click(formSizeLarge);
            $('#btnFormSizeMedium').click(formSizeMedium);
            $('#btnFormSizeSmall').click(formSizeSmall);
            $('#btnFormSizeReset').click(formSizeReset);

            /* Initialize for Form Size Setting (Regular/Small/Extra Small) */
            /* ---------------------------------------------------------- */

            initFormSize();

            /* open menu */
            /* ---------------------------------------------------------- */

            $(".menu-item.top-tier").click(
                function () {
                    if ($('html').hasClass('page-collapsed')) {
                        $(this).parents('#navbarSide').addClass('active').closest('.std-menu').addClass('active');
                    } else {
                        $(this).parents('#navbarSide').addClass('active').closest('.std-menu.active').removeClass('active');
                    }

                    var targetAnchor = $("a:first[data-url]", this)[0];
                    if (targetAnchor) {
                        screenLauncher.bind(this)({ target: targetAnchor }); // pass down the <a> object in target to support the screenLauncher call
                    }
                }
            );

            $(".portal-main-body, header, #draggable").on("mouseenter mouseleave", 
                function () {
                    if ($('html').hasClass('page-collapsed')) {
                        $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.top-tier.open').removeClass('open').find('.std-menu.active').removeClass('active');
                    } else {
                        $('#navbarSide').addClass('active').find('.top-tier.open').removeClass('open').find('.std-menu.active').removeClass('active');
                    }
                }
            );

            // Close submenu after opening a screen

            $(".menu-section a:not(.with-menu)").click(
                function () {
                    if ($('html').hasClass('page-collapsed')) {
                        $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.top-tier.open').removeClass('open').find('.std-menu.active').removeClass('active');
                    } else {
                        $('#navbarSide').find('.top-tier').removeClass('open').find('.std-menu').removeClass('active');
                    }
                }
            );

            // Add divider line to the last child of each menu group

            $("li.menu-core:last, li.menu-tc:last, li.menu-sdk:last, li.menu-ir:last").addClass("menu-group");

            $(document).click(function (e) {
                if (!isWidgetEmptyLnkClicked) {
                    $('.container_popUp.Widget.widgetList').hide();
                }
                isWidgetEmptyLnkClicked = false;
            });

            $('#spnCloseWidget').click(function () {
                $('#DivWidgetWindow').hide();
            });


            $('#lnkAddWidgets, .GoArrow,  #lblSeeIntoYourData').click(function () {
                isWidgetEmptyLnkClicked = true;
                $('#addRemoveWidget').show();
            });

            $(".top-buttons.addWidgets").click(function () {
                $('#addRemoveWidget').show();
            });

            $(".portalIcon.closeIcon").click(function () {
                $(this).closest(".container_popUp.Widget.widgetList").hide("fast");
            });

            $(".portalIcon.checkBox span.checkBox").addClass("portalIcon");

            $(".portalIcon.checkBox span.checkBox").removeClass("icon");

            initializeMainMenu();

            // add menu hotkeys
            initMenuHotkeys();

            // Initialize count of currently open UI windows (it should be 0)
            $('#spWindowCount').text($('#dvWindows').children().length);

            firstControl["control"] = "widgetLayout";
            firstControl["rank"] = 1;
            controls.push(firstControl);
            $("#draggable").draggable({ axis: "y", containment: "window", scroll: false });
            hideIframes();
            $('#dvCloseWindowErrorMessage').hide();
            $('.task_added').hide();

            recentWindowsMenu.onLoadPopulateRecentWindowsListFromStorage();


            // Update the current open window title in windows manager
            window.addEventListener('message', function (event) {
                var data = event.data;
                if (data.event_id === 'SaveTemplate' && data.inquiryTitle) {
                    var title = data.inquiryTitle;
                    $("#dvWindows span[rank]").each(function (index, elem) {
                        var $iframe = $('#' + $(elem).attr('frameid'));
                        if ($iframe.is(':visible')) {
                            $(elem).first().text(title);
                        }
                    });
                }
            });

            $("#windowManager").on("click", function () {
                if ($('#dvWindows').children().length > 0) {
                    $("#windowManager > div").show();
                }

                // Reset selection
                $("#dvWindows span").removeClass('selected');

                // Find Active Window and AddClass Selected
                $("#dvWindows span").each(function (index, elem) {
                    var $iframe = $('#' + $(elem).attr('frameid'));
                    if ($iframe.is(':visible')) {

                        $(elem).addClass('selected');

                        // Getting screenId from Taskdoc which having class as selected
                        _screenId = $('#dvWindows div span.selected').attr("data-menuid");
                        

                        // Checking whether Taskdoc Item having a generated report from screen or not
                        if (_screenId === constants.REPORT_SCREEN_ID) {
                            _screenId = constants.REPORT_SCREEN_HELP;
                        }

                        _public.setScreenId(_screenId);

                        return false;
                    }
                });

                $("#recentWindowManager > div").hide();

            }).on("mouseleave", function () {
                $("#windowManager > div").hide();
            });

            $("#recentWindowManager").on("click", recentWindowsMenu.show).on("mouseleave", recentWindowsMenu.hide);

            // D-41776 iPad issue - Closing the Window Manager when touched outside of the popup

            $("#recentWindowManager, #navbarSide, #breadcrumb, #screenLayout, #btnViewReports, #btnViewInquiries, #btnViewNotes, #globalHeader").click(function () {
                $("#windowManager > div").hide();
            });

            // D-41776 iPad issue - Closing the Recently Opened Windows when touched outside of the popup

            $("#windowManager, #navbarSide, #breadcrumb, #screenLayout, #btnViewReports, #btnViewInquiries, #btnViewNotes,  #globalHeader").click(function () {
                $("#recentWindowManager > div").hide();
            });

            $('.top_nav_drop_content').click(function () {
                isReload = false;
            });

            $("#homeNav, .logo-product").click(
                function () {

                    if ($('html').hasClass('page-collapsed')) {
                        $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.top-tier.open').removeClass('open').find('.std-menu.active').removeClass('active');
                    } else {
                        $('#navbarSide').find('.top-tier').removeClass('open').find('.std-menu').removeClass('active');
                    }

                    ShowHomePage();
                    updateLayout(false);
                }
            );

            $('.std-menu').click(function (event) {
                event.stopPropagation();
            });
/*
            $('#topMenuTools .k-link').click(function () {
                $('.quick-menu').toggle();
                $(this).parent('.menu-item-with-icon').toggleClass('inactive');
                return false;
            });

  */          // onload event handling on iframes
            $('#screenLayout').children().each(function () {
                var $iframe = $(this).find("iframe");
                $iframe.on("load", function (e) {
                    iFrameLoadEvent(e, $(this));
                    window.scrollTo(0, 0);
                    initFormSize();
                });
            });

            // Active Window Manager - click handler
            $("#dvWindows").on("click", "span", function () {
                currentIframeId = $(this).attr("frameId");

                // Adding Class Selected to the Active Window
                $('div#dvWindows span').not($(this)).removeClass('selected');
                $(this).addClass('selected');

                if ($(this).attr('command') === "Remove") {
                    var $currentIframeId = $("#" + currentIframeId);
                    var url = $currentIframeId.attr("src");
                    var isReport = url.indexOf("ReportViewer.aspx?token") > 0 || url.indexOf("CustomReportViewer.aspx?reportName") > 0;

                    $currentIframeId.attr("src", "about:blank");
                    currentDiv = $(this).attr('controlToRemove');

                    setTimeout(function () {
                        var isContentWindow = sg.utls.isChrome() || sg.utls.isMozillaFirefox() || sg.utls.isSafari();
                        var iframeWin = isContentWindow ? window[currentIframeId].contentWindow : window[currentIframeId];
                        isIframeClose = isReport || (iframeWin.name === "unloadediFrame");

                        if (isIframeClose) {
                            flag = false;
                            $.each(controls, function (index, element) {
                                if (element["control"] === currentIframeId) {
                                    currentRank = element["rank"];
                                }
                            });
                            var iframeHtml = sg.utls.formatString("<iframe scrolling='no' sandbox='allow-forms allow-popups allow-pointer-lock allow-same-origin allow-scripts allow-top-navigation allow-downloads allow-modals' id='{0}' src='' class='screenIframe' style='display: none;'></iframe>",
                                currentIframeId);
                            $currentIframeId.parent().empty().append(iframeHtml);
                            iFrameLoadEvent(null, $currentIframeId);
                        } else {
                            // Keep the original src url, used with check whether the screen is still 
                            // opened(isScreenAlreadyOpen) as select "stay" option in confirmation prompt  
                            if (!$currentIframeId.prop("originalSrc")) {
                                $currentIframeId.prop("originalSrc", url);
                            }
                        }
                    }, 100);
                } else if ($(this).attr('command') === "Add") {
                    var currentSelectedRank = $(this).attr("rank");
                    if ($('#widgetHplayout').is(":visible")) {
                        $('#widgetHplayout').hide();
                    }
                    hideIframes();
                    currentDiv = ($(this).attr('frameId'));
                    $.each(controls, function (index, element) {
                        if (currentDiv === element["control"]) {
                            element["rank"] = 1;
                        } else if (element["rank"] < currentSelectedRank) {
                            element["rank"]++;
                            $('#' + element["control"]).attr("rank", element["rank"]);
                        }
                    });

                    $('#screenLayout').show();
                    $('#widgetLayout').hide();

                    // Remove background
                    $('html').removeClass('home-page');

                    $("#" + currentDiv + "").show();
                    currentIframe = currentDiv;

                    $('#dvWindows').find('span').each(function () {
                        if ($(this).attr('command') == 'Add') {
                            if (currentIframe == $(this).attr('frameid')) {
                                $(this).attr("rank", 1);
                            } else if (parseInt($(this).attr('rank'), 10) < currentSelectedRank) {
                                $(this).attr("rank", (parseInt($(this).attr("rank"), 10) + 1));
                            }
                        }
                    });

                    // Breadcrumb - Load breadcrumb on window management item selection
                    var parentidVal = $(this).data('parentid');
                    loadBreadCrumb(parentidVal);

                    // Menu Help - Load Menu Help on window management item selection
                    _screenId = $(this).attr("data-menuid");

                    // Checking the Taskdoc having a generated Report Screen or not
                    if (_screenId === constants.REPORT_SCREEN_ID) {
                        _screenId = constants.REPORT_SCREEN_HELP;
                    }

                    _public.setScreenId(_screenId);

                    // Notify change inquiry data source 
                    var winUrl = $("#" + currentDiv).attr("src");
                    if (winUrl.indexOf("Core/InquiryGeneral/Index/?templateId") > -1) {
                        var dsId = winUrl.substring(winUrl.indexOf("&dsId") + 6);
                        window.postMessage({ event_id: 'QueryDataSourceChanged', dataSourceId: dsId }, '*');
                    }
                }
            });

            // Recent Window Manager - click handler
            $("#dvRecentWindows").on("click", "span", function () {

                // Remove background when screenLayout is visible 
                $('html').removeClass('home-page');

                // Adding Class Selected to the Active Window
                $('div#dvRecentWindows span').not($(this)).removeClass('selected');
                $(this).addClass('selected');


                $('#screenLayout').show();
                $('#widgetLayout').hide();

                targetUrl = $(this).attr('data-url');

                // Parse out the area, controller, action and parameters from the partial url
                // and pass to the buildUrl method.
                var parts = sg.utls.url.extractAreaControllerActionAndParametersFromPartialUrl(targetUrl);
                targetUrl = sg.utls.url.buildUrl(parts.AreaName, parts.ControllerName, parts.ActionName, parts.ParametersName);

                const lastScreenId = _screenId;
                var isScreenOpen = isScreenAlreadyOpen(targetUrl);
                var isMaxScreenCountReachedAndNotOpen = isMaxScreenNumReachedAndNotOpen(targetUrl);
                if (isScreenOpen || isMaxScreenCountReachedAndNotOpen) {

                    helpSearchForMenuItem(_screenId);
                    hideIframes();
                    assignUrl(this.innerHTML, parentidVal, _screenId);


                    return;
                }

                if ($('#widgetHplayout').is(":visible")) {
                    $('#widgetHplayout').hide();
                }
                $("#" + currentDiv + "").show();

                // Breadcrumb - Load breadcrumb on window management item selection
                var parentidVal = $(this).data('parentid');
                loadBreadCrumb(parentidVal);

                // Menu Help - Load Menu Help on window management item selection
                const recentScreenId = $(this).attr("data-menuid");

                // Checking the Taskdoc having a generated Report Screen or not
                if (recentScreenId === constants.REPORT_SCREEN_ID) {
                    recentScreenId = constants.REPORT_SCREEN_HELP;
                }

                _public.setScreenId(recentScreenId);

                hideIframes();
                assignUrl(this.innerHTML, parentidVal, _screenId);
            });

            widgetUI = { NavigableMenuDetail: {} };

            $(".kpi .btnOpenReport").on("click", function (event) {
                hideIframes();
                iFrameUrl = $(this).closest(".kpi").find("iframe").attr("src");
                if (iFrameUrl.indexOf("AgedPayable") > 0) {
                    $('#screenLayout').show();
                    sg.utls.ajaxPost(sg.utls.url.buildUrl("KPI", "AgedPayablesReport", "Execute"), {}, loadOptions.executeAgedPayableReport);
                } else if (iFrameUrl.indexOf("AgedReceivable") > 0) {
                     $('#screenLayout').show();
                    sg.utls.ajaxPost(sg.utls.url.buildUrl("KPI", "AgedReceivablesReport", "Execute"), {}, loadOptions.executeAgedReceivableReport);
                }
            });

            var loadOptions = {
                executeAgedPayableReport: function (result) {
                    if (result != null && result.UserMessage.IsSuccess) {
                        sg.utls.openReport(result.ReportToken);
                        isKPI = true;
                        kpiReportName = portalBehaviourResources.PayableAging;
                    } else {
                        sg.utls.showMessage(result);
                        $(window).scrollTop(0);
                    }
                },
                executeAgedReceivableReport: function (result) {
                    if (result != null && result.UserMessage.IsSuccess) {
                        sg.utls.openReport(result.ReportToken);
                        isKPI = true;
                        kpiReportName = portalBehaviourResources.ReceivableAging;
                    } else {
                        sg.utls.showMessage(result);
                        $(window).scrollTop(0);
                    }
                }
            };

            $('#nav-home').kendoMenu();

            $(".menu-section a:not(.with-menu)").on("click", screenLauncher);

            // Closing Exceed Limit Message popup
            var keyHandler = function (e) {
                //if the key press is ESC
                var KEY_ESC = 27;
                if (e.keyCode === KEY_ESC) {
                    $(".icon.msgCtrl-close").trigger("click");
                }
            };
            $(document).on("keydown", keyHandler);

            $('#addWidgetBtnProcess').click(function () {
                $('#widgetMsgDiv').hide();
            });
            $('#btnProcess').click(function () {
                $('#msgDiv').hide();
            });

            // Event Listener to handle post message events
            if (window.addEventListener) {
                // For standards-compliant web browsers
                window.addEventListener("message", receiveWindowMessage, false);
            } else {
                window.attachEvent("onmessage", receiveWindowMessage);
            }

            $(window).resize(function () {
                resizeLayout();
            });

            resizeLayout();

            $("#liSignOut").html('<a id="lnkSignOut" href="javascript:void(0);" class="k-link">' + portalBehaviourResources.SignOut + '</a>');
            $("#liManageUsers").html('<a id="lnkUserManagment" href="javascript:void(0)" class="k-link">' + portalBehaviourResources.ManageUsers + '</a>');

            $("#lnkSignOut").on('click', function () {
                sg.utls.logOut();
            });

            $("#liManageUsers").on('click', function () {
                window.open(umURL);
            });

            $("#globalSearch").on('click', function () {
                sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "GlobalSearch", "IsServiceRunning"), {}, openGlobalSearch);
            });

            $("#btnColorPickerDB").on('click', function () {
                var data = { key: "colour" };
                sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "DeleteUserPreference"), data);
                sg.utls.setBackgroundColor($("#header"));
            });

            $("#btnSDA").click(function () {
                sg.utls.ajaxPost(sg.utls.url.buildUrl("CS", "CompanyProfile", "GetSDASubdomain"), null, onSDASuccess);
            });
        },

        // Property Getters and Setters

        getScreenId: function () {
            return _screenId;
        },

        setScreenId: function (id) {
            if (_screenId != id) {
                _screenId = id;
            }
            helpSearchForMenuItem(_screenId);
        },

        getGlobalSearchDrillDownParameter: function () {
            return _globalSearchDrillDownParameter;
        },

        setGlobalSearchDrillDownParameter: function (param) { 
            _globalSearchDrillDownParameter = param;
        }
    };

    return _public;
};

var taskDockMenuBreadCrumbManager;
$(document).ready(function () {
    taskDockMenuBreadCrumbManager = new TaskDockMenuBreadCrumbManager();
    if (taskDockMenuBreadCrumbManager) {
        taskDockMenuBreadCrumbManager.init();
    }
});
