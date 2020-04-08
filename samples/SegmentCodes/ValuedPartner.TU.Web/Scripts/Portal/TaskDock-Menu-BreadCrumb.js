/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */

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
    var _screenId = 0;

    // To be used to record extra parameter in order to screen with specific record
    var _globalSearchDrillDownParameter = null;

    var _oAuthWindow = null;

    // Private Functions

    /**
     * @name hideIframes
     * @description TODO - Add description
     * @private
     */
    function hideIframes() {
        $('#screenLayout').children().find("iframe").each(function () {
            $(this).hide();
        });
    }

    /**
     * @name helpSearchForMenuItem
     * @description TODO - Add description
     * @private
     * @param {any} id - TODO Add description
     * 
     */
    function helpSearchForMenuItem(id) {
        var data = { screenId: id };
        var helpUrl = sg.utls.url.buildUrl("Core", "Help", "Index");
        var key = helpUrl + "_" + id + "_" + globalResource.Culture;
        var callBack = function (result) { $('#searchHelpDiv').html(result); };

        sg.utls.ajaxCachePostHtml(helpUrl, data, callBack, key);
    }

    /**
     * @name ShowHomePage
     * @description TODO - Add description
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
        _screenId = constants.DEFAULT_SCREENID;

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
     * @description
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
     * @description TODO - Add description
     * @private
     */
    function initializeMainMenu() {
        var $menu = $(".side-nav .std-menu");
        var $subLevelMenu = $(".side-nav .std-menu ul");

        $menu.menuAim({
            activate: function (row) {
                var $row = $(row);

                // Move on to the first menu (non-header) item if user is moused to header
                if ($row.hasClass("sub-heading")) {
                    $row = $row.next();
                }
                $row.find(".sub-menu-wrap").first().show();

                // Clear the possibility of first menu to be active
                $row.siblings("li:not(.sub-heading)").first().find('a:first').removeClass('active');
                $row.find("a:first").addClass("active");
            },
            deactivate: function (row) {
                var $row = $(row);

                // move on to the first menu (non-header) item if user is moused to header
                if ($row.hasClass("sub-heading")) {
                    $row = $row.next();
                }
                $row.find(".sub-menu-wrap").first().hide();
                $row.find("a:first").removeClass('active');
            }
        });

        $subLevelMenu.menuAim({
            activate: function (row) {
                var $submenu = $(row).find(".sub-menu-wrap").first();
                $submenu.addClass("child");

                if ($(row).find("div:first").length) {
                    $(row).find("a:first").addClass("active");
                }
                
                $submenu.show();
            },
            deactivate: function (row) {
                var $submenu = $(row).find(".sub-menu-wrap").first();
                $submenu.hide();
                $submenu.removeClass("child");
                $(row).find("a:first").removeClass("active");
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
            $(this).parent('.menu-item').toggleClass('open').find('.std-menu').toggleClass('active');
            $(this).parent('.menu-item').siblings().removeClass('open').find('.std-menu').removeClass('active');
        });
    }

    /**
     * @name iFrameLoadEvent
     * @description TODO - Add description
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

                // Close the maximum number of windows message box
                $('#dvWindowsExceedLimitErrorMessage').hide();

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
                    _screenId = constants.DEFAULT_SCREENID;

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
     * @description TODO - Add description
     * @private
     */
    function taskAdded() {
        $(".task_added").show().css({ "right": "-29px" }).animate({ "right": "77px" }, "1500");
        $(".task_added").delay(1800).css({ "right": "77px" }).animate({ "right": "-29px" }, "3000").fadeOut();
    }

    /**
     * @name assignUrl
     * @description - TODO description
     * @param {string} windowText - TODO description
     * @param {string} parentid - TODO description
     * @param {string} menuid - TODO descripion
     * @param {bool} isExcludingParameters - TODO descripion
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

                $iframe.load(function () {
                    // remove the loading/spinner after the page is loaded
                    $(this).removeClass('screenLoading');
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
                    _screenId = menuid;
                } else {
                    _screenId = constants.REPORT_SCREEN_HELP;
                }
            }
        });
    }

    /**
     * @name isMaxScreenNumReachedAndNotOpen
     * @description TODO - Add description
     * @private
     * @param {string} targetUrl - TODO Add description
     * @returns {boolean} true | false
     */
    function isMaxScreenNumReachedAndNotOpen(targetUrl) {

        // Check if maximum number of screens reached
        var isScreenOpen = isScreenAlreadyOpen(targetUrl);
        var activeScreenCount = recentWindowsMenu.activeScreenCount();
        if (!isScreenOpen && activeScreenCount >= constants.MAXIMUM_ALLOWABLE_ACTIVE_WINDOWS) {
            $('#dvWindowsExceedLimitErrorMessage').show();
            return true;
        }
        return false;
    }

    /**
     * @name screenLauncher
     * @description TODO - Add description
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

            // Increment the open screen count and notify sibling tabs to disable
            // their session date pickers
            incrementOpenScreenCounterAndNotifySiblings();

            if (activeScreenCountWithinMaximumAllowable()) {
                hideIframes();
            }

            // Load breadcrumb on menu item click and add item to windows dock
            var parentidVal = $(this).data('parentid');

            // Get menu id as screenId for Help Menu Search
            _screenId = $(this).attr("data-menuid");

            // screen could be launched from level 2 menu from an "li", so keep searching
            if (!_screenId && $(this).is("li")) {
                _screenId = $(this).find("a").attr("data-menuid");
            }

            loadBreadCrumb(parentidVal);

            var windowtext = $(event.target).text();

            $('#dvWindows > div').each(function () { $(this).find("span").removeClass('selected'); });

            if ($(this).attr("data-modulename") !== "" && $.parseHTML($(this).attr("data-modulename")) != null && $(this).attr("data-moduleName") != "null") {
                windowtext = portalBehaviourResources.PagetitleInManager.format($(this).attr("data-modulename"), $(event.target).text());
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
     * @description TODO - Add description
     * @private
     * @param {any} parentidVal - TODO Add description
     */
    function loadBreadCrumb(parentidVal) {
        if (!$('#widgetLayout').is(":visible")) {
            var html = [];
            // Merge plugin menus to the MenuList            
            if (PluginMenuList && PluginMenuList.length > 0 && (!pluginMenuMerged)) {
                Array.prototype.push.apply(MenuList, PluginMenuList);
                pluginMenuMerged = true;
            }
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

            $(".innerdd").hover(function () {

                $(this).find("ul").show();
            },
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

                // Increment the open screen count and notify sibling tabs to disable
                // their session date pickers
                incrementOpenScreenCounterAndNotifySiblings();

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
                _screenId = $(this).attr("data-menuid");

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
     * @description Check whether the screen is opened, use currently src url and original url
     * @param {string} url - TODO Add description
     * @returns {boolean} true | false
     */
    function isScreenAlreadyOpen(url) {
        var result = false;
        // Check if the screen is already open
        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            var srcUrl = $iframe.attr("src");
            var originalUrl = $iframe.prop("originalSrc");
            result = (sg.utls.getUrlPath(srcUrl) === url && _globalSearchDrillDownParameter === null) || (originalUrl && originalUrl === url);
            if (result) {
                $("#dvWindows span[command='Add'][frameid='" + $iframe[0].id + "']").trigger("click");
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
     * @description TODO - Add Description
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
     * @description TODO - Add description
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

            // Increment the open screen count and notify sibling tabs to disable
            // their session date pickers
            incrementOpenScreenCounterAndNotifySiblings();

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

            // Increment the open screen count and notify sibling tabs to disable
            // their session date pickers
            incrementOpenScreenCounterAndNotifySiblings();

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
            _screenId = constants.REPORT_SCREEN_HELP;

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

            // Increment the open screen count and notify sibling tabs to disable
            // their session date pickers
            incrementOpenScreenCounterAndNotifySiblings();

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
     * @name decrementOpenScreenCounterAndNotifySiblings
     * @description TODO - Add description
     * @private
     */
    function decrementOpenScreenCounterAndNotifySiblings() {
        if (openScreenCountManager.decrementCounter() === 0) {
            sg.utls.enablePortalSessionDatePicker();

            // Notify sibling tabs to enable their session date pickers
            var key = "ALLSESSIONS_EnablePortalSessionDatePicker";
            var randomValue = sg.utls.makeRandomString(5);
            sage.cache.local.set(key, randomValue);
        }
    }

    /**
     * @name incrementOpenScreenCounterAndNotifySiblings
     * @description TODO - Add description
     * @private
     */
    function incrementOpenScreenCounterAndNotifySiblings() {
        if (openScreenCountManager.incrementCounter() > 0) {
            sg.utls.disablePortalSessionDatePicker();

            // Notify sibling tabs to disable their session date pickers
            var key = "ALLSESSIONS_DisablePortalSessionDatePicker";
            var randomValue = sg.utls.makeRandomString(5);
            sage.cache.local.set(key, randomValue);
        }
    }

    /**
     * @name initSideMenu
     * @description TODO - Add description
     * @private
     */
    function initSideMenu() {
        sg.utls.getUserPreferences(menuUserPreferenceKey, function (result) {
            if (result) {
                if (result === "expanded") {
                    menuLayoutExpanded();
                }
                else if (result === "collapsed") {
                    menuLayoutCollapsed();
                }
            }
        });
    }

    /**
     * @name menuLayoutCollapsed
     * @description TODO - Add description
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
     * @description TODO - Add description
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
     * @description TODO - Add description
     * @private
     */
    function showMenuExpandButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').show();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').hide();
    }

    /**
     * @name showMenuCollapseButton
     * @description TODO - Add description
     * @private
     */
    function showMenuCollapseButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').hide();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').show();
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

        OAuthWindow: _oAuthWindow,

        setDefaultScreenId: function () {
            _screenId = constants.DEFAULT_SCREENID;
        },

        init: function () {

            _screenId = constants.DEFAULT_SCREENID;

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
                // https://github.com/telerik/kendo-ui-core/issues/2524
                if (e.item.is('#helpMenu')) {
                    e.item.find('input').first().focus();
                }
            });

            showMenuExpandButton(); // by default: menu is collapsed, so show expand button

            $('#btnExpandMenu, #btnExpandMenuAlt').click(menuLayoutExpanded);
            $('#btnCollapseMenu, #btnCollapseMenuAlt').click(menuLayoutCollapsed);

            /* Initialize for Side Menu Setting (Collapsed/Expanded) */
            /* ---------------------------------------------------------- */

            initSideMenu();

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

            /* hiding tooltip */
            /* ---------------------------------------------------------- */

            //$(".std-menu").hover(
            //    function () {
            //        if (mainToolTip) {
            //            mainToolTip.hide();
            //        }
            //    }
            //);

            /* close menu */
            /* ---------------------------------------------------------- */

            $(".portal-main-body, header, #draggable").hover(
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

            $('#topMenu').mouseenter(function () {
                if (!$("#helpSearchfl").is(':focus')) {
                    helpSearchForMenuItem(_screenId);
                }
            });

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

            $("#windowManager").hover(function () {
                if ($('#dvWindows').children().length > 0)
                    $("#windowManager > div").show();

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

                        return false;
                    }
                });
            }, function () {
                $("#windowManager > div").hide();
            });

            $("#recentWindowManager").hover(recentWindowsMenu.show, recentWindowsMenu.hide);

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

            $('#topMenuTools .k-link').click(function () {
                $('.quick-menu').toggle();
                $(this).parent('.menu-item-with-icon').toggleClass('inactive');
                return false;
            });

            // onload event handling on iframes
            $('#screenLayout').children().each(function () {
                var $iframe = $(this).find("iframe");
                $iframe.load(function (e) {
                    iFrameLoadEvent(e, $(this));
                    window.scrollTo(0, 0);
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

                    // Decrement the current open window counter
                    // If active window count drops to zero (or below),
                    // notify all other tabs
                    decrementOpenScreenCounterAndNotifySiblings();

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
                            var iframeHtml = sg.utls.formatString("<iframe scrolling='no' sandbox='allow-forms allow-popups allow-pointer-lock allow-same-origin allow-scripts allow-top-navigation' id='{0}' src='' class='screenIframe' style='display: none;'></iframe>",
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

                var isScreenOpen = isScreenAlreadyOpen(targetUrl);
                var isMaxScreenCountReachedAndNotOpen = isMaxScreenNumReachedAndNotOpen(targetUrl);
                if (isScreenOpen || isMaxScreenCountReachedAndNotOpen) {
                    return;
                }

                // Increment the open screen count and notify sibling tabs to disable
                // their session date pickers
                incrementOpenScreenCounterAndNotifySiblings();

                if ($('#widgetHplayout').is(":visible")) {
                    $('#widgetHplayout').hide();
                }
                $("#" + currentDiv + "").show();

                // Breadcrumb - Load breadcrumb on window management item selection
                var parentidVal = $(this).data('parentid');
                loadBreadCrumb(parentidVal);

                // Menu Help - Load Menu Help on window management item selection
                _screenId = $(this).attr("data-menuid");

                // Checking the Taskdoc having a generated Report Screen or not
                if (_screenId === constants.REPORT_SCREEN_ID) {
                    _screenId = constants.REPORT_SCREEN_HELP;
                }

                hideIframes();
                assignUrl(this.innerHTML, parentidVal, _screenId);
            });

            widgetUI = { NavigableMenuDetail: {} };

            $(".kpi .btnOpenReport").on("click", function (event) {
                hideIframes();
                iFrameUrl = $(this).closest(".kpi").find("iframe").attr("src");
                if (iFrameUrl.indexOf("AgedPayable") > 0) {
                    $('#screenLayout').show();
                    $('#widgetLayout').hide();
                    sg.utls.ajaxPost(sg.utls.url.buildUrl("KPI", "AgedPayablesReport", "Execute"), {}, loadOptions.executeAgedPayableReport);
                } else if (iFrameUrl.indexOf("AgedReceivable") > 0) {
                    $('#screenLayout').show();
                    $('#widgetLayout').hide();
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

            $('.icon.msgCtrl-close').click(function () {
                $('#dvWindowsExceedLimitErrorMessage').hide();
            });

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

            $("#btnIntelligence").click(function () {
                // Attempt to signout and start SIRC afterward
                var cb = sg.utls.url.buildCacheBuster();
                var url = oAuthLocation + "connect/endsession" + cb;
                var win = window.open(url, "_blank", "");
                _oAuthWindow = win;
            });
        },

        // Property Getters and Setters

        getScreenId: function () {
            return _screenId;
        },

        setScreenId: function (id) {
            _screenId = id;
        },

        getGlobalSearchDrillDownParameter: function () {
            return _globalSearchDrillDownParameter;
        },

        setGlobalSearchDrillDownParameter: function (param) { 
            _globalSearchDrillDownParameter = param;
        },

        getOAuthWindow: function () {
            return _oAuthWindow;
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

window.addEventListener("message", function (e) {
    if (e.data === "SignedOut") {
        var win = taskDockMenuBreadCrumbManager.getOAuthWindow();
        if (win) {
            win.close();

            var cb = sg.utls.url.buildCacheBuster();
            var partialUrl = $("#hdnUrl").val().split('/').filter(function (el) { return el; });
            var url = sg.utls.url.buildUrl("WebApiProxy?" + cb + "&session=" + partialUrl[partialUrl.length - 1], "", "");
            win = window.open(url, "_blank", "");
        }
    }
}, false);
