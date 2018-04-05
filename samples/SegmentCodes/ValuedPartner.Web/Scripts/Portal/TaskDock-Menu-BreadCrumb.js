/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */

"use strict"

var widgetUI = widgetUI || {};

//screen Id hold value for each menu screen menu Id
var screenId = 0;

//For homepage help screen
var defaultScreenId = 0;

//For Report Screen Help
var reportScreenHelp = 1;

//Report Screen default Id
var reportScreenId = " ";

//Inquiry Screen Id for Help purposes
var inquiryScreenId = 2;

var isWidgetVisible = false;

var widgetDomain;
var tenantName;
var domain;
var OAuthWindow;

// Use to do string format, kind of like String.format in C#
String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        str = str.replace(reg, arguments[i]);
    }
    return str;
}

function clearIframes() {
    $('#screenLayout').children().find("iframe").each(function () {
        $(this).hide();
    });
};

function helpSearchForMenuItem(screenId) {
    var data = { screenId: screenId };

    sg.utls.ajaxCachePostHtml(sg.utls.url.buildUrl("Core", "Help", "Index"), data, function (result) {
        $('#searchHelpDiv').html(result);
    }, sg.utls.url.buildUrl("Core", "Help", "Index") + "_" + screenId + "_" + globalResource.Culture);
};

$(document).ready(function () {

    widgetDomain = sg.utls.url.baseUrl();
    var numberOfActiveWindows = $('#hdnNumberOfActiveWindows').val();
    var isIframeClose = false;
    var currentRank;
    var flag;
    var currentDiv;
    var currentIframeId;
    var controls = [];
    var firstControl = {};
    var isReload = true;
    var currentIframe;
    var iFrameUrl;
    var isKPI = false;
    var kpiReportName;
    var MenuLayoutCookieName = "MenuLayoutCookie";

    screenId = defaultScreenId;
    $('#searchHelpDiv').show();

    // Home button, Tools button active state
    $('#homeNav > a').addClass('active');
    $('#topMenuTools').addClass('active');

    var isWidgetEmptyLnkClicked = false;

    var menu = $("#topMenu").kendoMenu({ openOnClick: true, closeOnClick: true }).data("kendoMenu");
    menu.bind('activate', function (e) {
        //This is to fix D-33845
        //For the menu item that contains a textbox, there is an Kendo issue on Chrome
        //The workaround is to focus on an input when menu is activated.
        //https://github.com/telerik/kendo-ui-core/issues/2524
        if (e.item.is('#helpMenu')) {
            e.item.find('input').first().focus();
        }
    });

    function showMenuExpandButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').show();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').hide();
    }

    function showMenuCollapseButton() {
        $('#btnExpandMenu, #btnExpandMenuAlt').hide();
        $('#btnCollapseMenu, #btnCollapseMenuAlt').show();
    }

    showMenuExpandButton(); // by default: menu is collapsed, so show expand button

    
    /* Page Layout for Menu Expanded */
    /* ---------------------------------------------------------- */
    function menuLayoutExpanded() {
        showMenuCollapseButton();
        $('html').removeClass('page-collapsed').addClass('page-expanded');
        $('#navbarSide').removeClass('side-nav-collapsed').addClass('active');

        var modifiedCookie = "expanded";
        var cookieExpiresdate = new Date(9999, 12, 31);
        $.cookie(MenuLayoutCookieName, modifiedCookie, { path: '/', expires: cookieExpiresdate, secure: window.location.protocol === "http:" ? false : true });

        // Reload/refresh widget layout 
        updateLayout(false);
    };

    $('#btnExpandMenu, #btnExpandMenuAlt').click(menuLayoutExpanded);

    /* Page Layout for Menu Collapsed */
    /* ---------------------------------------------------------- */

    function menuLayoutCollapsed() {
        showMenuExpandButton();
        $('html').removeClass('page-expanded').addClass('page-collapsed');
        $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.std-menu.active').removeClass('active');

        var modifiedCookie = "collapsed";
        var cookieExpiresdate = new Date(9999, 12, 31);
        $.cookie(MenuLayoutCookieName, modifiedCookie, { path: '/', expires: cookieExpiresdate, secure: window.location.protocol === "http:" ? false : true });

        // Reload/refresh widget layout 
        updateLayout(false);
    };

    $('#btnCollapseMenu, #btnCollapseMenuAlt').click(menuLayoutCollapsed);

    /* Initialize Cookie for Side Menu Setting (Collapsed/Expanded) */
    /* ---------------------------------------------------------- */

    initSideMenu();

    function initSideMenu() {
        var menuCookie = $.cookie(MenuLayoutCookieName);
        if(menuCookie)
        {
            if (menuCookie === "expanded")
            {
                menuLayoutExpanded();
            }
            else if (menuCookie === "collapsed"){
                menuLayoutCollapsed();
            }
        }
    };
   
    /* open menu */
    /* ---------------------------------------------------------- */

    $("#listPrimary").hover(
      function () {
          if ($('html').hasClass('page-collapsed')) {
              $('#navbarSide').removeClass('side-nav-collapsed').addClass('active');
          } else {
              $('#navbarSide').addClass('active');
          }
      });

    $(".menu-item.top-tier").click(
      function () {
          if ($('html').hasClass('page-collapsed')) {
              $(this).parents('#navbarSide').removeClass('side-nav-collapsed').addClass('active').closest('.std-menu').addClass('active');
          } else {
              $(this).parents('#navbarSide').addClass('active').closest('.std-menu.active').removeClass('active');
          }
      }
    );
    
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

    //close submenu after opening a screen

    $(".menu-section a").click(
        function () {
            if ($('html').hasClass('page-collapsed')) {
                $('#navbarSide').removeClass('active').addClass('side-nav-collapsed').find('.top-tier.open').removeClass('open').find('.std-menu.active').removeClass('active');
            } else {
                $('#navbarSide').find('.top-tier').removeClass('open').find('.std-menu').removeClass('active');
            }
        }
    );

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
    initializeExtraMenu();

    // Initialize count of currently open UI windows (it should be 0)
    $('#spWindowCount').text($('#dvWindows').children().length);

    firstControl["control"] = "widgetLayout";
    firstControl["rank"] = 1;
    controls.push(firstControl);
    $("#draggable").draggable({ axis: "y", containment: "window", scroll: false });
    clearIframes();
    $('#dvCloseWindowErrorMessage').hide();
    $('.task_added').hide();

    recentWindowsMenu.onLoadPopulateRecentWindowsListFromStorage();

    $('#topMenu').mouseenter(function () {
        if (!$("#helpSearchfl").is(':focus')) {
            helpSearchForMenuItem(screenId);
        }
    });

    $("#windowManager").hover(function () {
        if ($('#dvWindows').children().length > 0)
            $("#windowManager > div").show();

        //reset selection
        $("#dvWindows span").removeClass('selected');

        // Find Active Window and AddClass Selected
        $("#dvWindows span").each(function (index, elem) {
            var $iframe = $('#' + $(elem).attr('frameid'));
            if ($iframe.is(':visible')) {

                $(elem).addClass('selected');
                //Getting screenId from Taskdoc which having class as selected
                screenId = $('#dvWindows div span.selected').attr("data-menuid");

                //Checking whether Taskdoc Item having a generated report from screen or not
                if (screenId === reportScreenId) {
                    screenId = reportScreenHelp;
                }

                return false;
            }
        });

    }, function () {
        $("#windowManager > div").hide();
    });

    $("#recentWindowManager").hover(
        recentWindowsMenu.hoverOn,
        recentWindowsMenu.hoverOff
    );

    $('.top_nav_drop_content').click(function () {

        isReload = false;
    });

    function ShowHomePage() {
        window.scrollTo(0, 0);
        $('html').addClass('home-page');
        $('#homeNav > a').addClass('active');
        $('#screenLayout').hide();
        $('#widgetlayout').show();
        $('#widgetHplayout').hide();

        //When footer logo is clicked
        screenId = defaultScreenId;

        $('#breadcrumb').hide();

        updateLayout(true);

        if (!$('#screenLayout').is(":visible")) {

            if ($('#widgetlayout').is(":visible")) {
                $('#widgetHplayout').hide();
            }
        }
    }


    function AreWidgetVisible() {
        $(".bodyWidgetContainer > div").each(function () {
            if ($(this).find("iframe").attr('src') != '') {
                isWidgetVisible = true;
            }
        });
    }

    function ShowCorrectLayout() {
        if (isWidgetVisible && controls.length == 1) {
            $('#widgetLayout').show();
            $('#widgetHplayout').hide();
            $('#dvAddWidget').show();
            $('#breadcrumb').hide();

        } else if (controls.length == 1) {
            $('#widgetHplayout').show();
            $('#widgetLayout').hide();
            $('#dvAddWidget').hide();
        }
    }

    function initializeExtraMenu() {
        var $menu = $(".side-nav .std-menu");

        $menu.menuAim({
            activate: activateSubmenu,
            deactivate: deactivateSubmenu,
            exitMenu: exitSubmenu
        });

        function activateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap"); //,

            // Show the submenu
            $submenu.css({
                display: "block"
            });

            $row.find("a:first").addClass("active");
        }

        function deactivateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap");
        }

        function exitSubmenu(row) {
            var $row = $(row);
        }

        $(".side-nav .std-menu li").click(function (e) {
            e.stopPropagation();
            window.scrollTo(0, 0);
        });

    }

    function initializeMainMenu() {
        var $menu = $(".side-nav .std-menu");

        $menu.menuAim({
            activate: activateSubmenu,
            deactivate: deactivateSubmenu,
            exitMenu: exitSubmenu
        });

        function activateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap"); //,

            $row.not('.sub-heading').siblings().find('.sub-menu-wrap').hide();

            // Show the submenu
            $submenu.css({
                display: 'block'
            });

            if ($row.hasClass('sub-heading') == false) {
                $row.find('a:first').addClass('active').end().siblings().find('a:first').removeClass('active');
            }
        }

        function deactivateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap");
        }

        function exitSubmenu(row) {
            var $row = $(row);
        }

        $(".side-nav .std-menu li").click(function (e) {
            e.stopPropagation();
        });

        $('.side-nav .menu-item').children('label, a, .nav-icon').click(function () {
            $(this).parent('.menu-item').toggleClass('open').find('.std-menu').toggleClass('active');
            $(this).parent('.menu-item').siblings().removeClass('open').find('.std-menu').removeClass('active');
        });
    }
    
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
        $(this).parent('.menu-item-with-icon').toggleClass('active').toggleClass('inactive')
        return false;
    });

    function iFrameLoadEvent(e, $iframe)
    {
        if (isIframeClose && $iframe.attr('id') === currentIframeId) {
            //close the screen
            if (!($('#widgetLayout').is(":visible"))) {
                $('#' + currentIframeId).hide();
            }
            if (!flag) {

                //close the maximum number of windows message box
                $('#dvWindowsExceedLimitErrorMessage').hide();

                //remove task from the window
                $("#" + $("#" + currentDiv + "").attr('id') + "").remove();

                if ($('#dvWindows').children().length > 0) {

                    //keep the task window open
                    $("#windowManager > div").show();
                    //display number of open tasks
                    $('#spWindowCount').text($('#dvWindows').children().length);
                    
                } else {
                    //close the task window
                    $("#windowManager > div").hide();
                    $('#windowManager').removeClass("zeroTaskCount");
                    //hide the breadcrumb
                    $('#breadcrumb').hide();

                    $('#spWindowCount').text("0");
                    screenId = defaultScreenId;

                    $('#screenLayout').hide();

                    $('#spnSessionDate').removeClass('disabled');
                    $('#sessionDatelabel').removeClass('disabled');
                    $('#sessionDateIcon').removeClass('disabled');
                    $('#sessionDateIcon').removeClass('glyphicon-lock');
                    $('#sessionDateIcon').addClass('glyphicon-calendar-1');

                    // show background
                    $('html').addClass('home-page');
                    // show home button as activated
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

                //this is to keep active screen selection in the Open Windows popup
                $('#windowManager').mouseenter();
                isWidgetVisible = false;
                AreWidgetVisible();
                ShowCorrectLayout();
                updateLayout(false);
            }

            if (e) {
                e.preventDefault();
            }
        }
    }

    //onload event handling on iframes
    $('#screenLayout').children().each(function () {
        var $iframe = $(this).find("iframe");
        $iframe.load(function (e) {
            iFrameLoadEvent(e, $(this));
            window.scrollTo(0, 0);
        });
    });

    $("#dvWindows").on("click", "span", function () {
        currentIframeId = $(this).attr("frameId");

        //Adding Class Selected to the Active Window
        $('div#dvWindows span').not($(this)).removeClass('selected');
        $(this).addClass('selected');

        if ($(this).attr('command') === "Remove") {
            var $currentIframeId = $("#" + currentIframeId);
            var url = $currentIframeId.attr("src");
            var isReport = url.indexOf("ReportViewer.aspx?token") > 0 || url.indexOf("CustomReportViewer.aspx?reportName") > 0 ;

            $currentIframeId.attr("src", "about:blank");
            currentDiv = ($(this).attr('controlToRemove'));

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
                    //keep the original src url, used with check whether the screen is still opened(isScreenAlreadyOpen) as select "stay" option in confirmation prompt  
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
            clearIframes();
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
            // remove background
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
            screenId = $(this).attr("data-menuid");

            //Checking the Taskdoc having a generated Report Screen or not
            if (screenId === reportScreenId) {
                screenId = reportScreenHelp;
            }
        }
    });

    $("#dvRecentWindows").on("click", "span", function () {

        // remove background when screenLayout is visible 
        $('html').removeClass('home-page');

        //Adding Class Selected to the Active Window
        $('div#dvRecentWindows span').not($(this)).removeClass('selected');
        $(this).addClass('selected');

        $('#screenLayout').show();
        $('#widgetLayout').hide();

        targetUrl = $(this).attr('data-url');

        if (isScreenAlreadyOpen(targetUrl)) return;
        if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

        if ($('#widgetHplayout').is(":visible")) {
            $('#widgetHplayout').hide();
        }
        $("#" + currentDiv + "").show();

        // Breadcrumb - Load breadcrumb on window management item selection
        var parentidVal = $(this).data('parentid');
        loadBreadCrumb(parentidVal);
        // Menu Help - Load Menu Help on window management item selection
        screenId = $(this).attr("data-menuid");
        //Checking the Taskdoc having a generated Report Screen or not
        if (screenId === reportScreenId) {
            screenId = reportScreenHelp;
        }
        clearIframes();
        assignUrl(this.innerHTML, parentidVal, screenId);
    });

    widgetUI = { NavigableMenuDetail: {} };

    function taskAdded() {
        $(".task_added").show().css({ "right": "-29px" }).animate({ "right": "77px" }, "1500");
        $(".task_added").delay(1800).css({ "right": "77px" }).animate({ "right": "-29px" }, "3000").fadeOut();
    }

    $(".kpi .btnOpenReport").on("click", function (event) {
        clearIframes();
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
        },
    };

    function assignUrl(windowText, parentid, menuid, isExcludingParameters) {
        var control = {};
        var isIframeOpen = false;
        var isScreenOpen = false;
        if ($('#widgetHplayout').is(":visible")) {
            $('#widgetHplayout').hide();
        }
        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            
            if (isExcludingParameters && sg.utls.getUrlPath($iframe.attr("src")) === sg.utls.getUrlPath(targetUrl)) {
                // Compare the full URL including the query string.
                // now that we know the path is the same, check for url with parameter, if they are not the same, lets refresh the screen and display that iframe
                if ($iframe.attr("src") !== targetUrl) {
                    $iframe.attr("src", targetUrl);
                }
                $iframe.show();
                isScreenOpen = true;

                //do not display more than one frame.
                return false;
            }
            //Compare the full URL including the query string.
            else if ($iframe.attr("src") === targetUrl) {
                $iframe.show();
                isScreenOpen = true;

                //do not display more than one frame.
                return false;
            }
        });

        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            if (!$iframe.is(':visible') && $iframe.attr("src") === '' && !isIframeOpen && !isScreenOpen) {
                isIframeOpen = true;
                isIframeClose = false;
                $iframe.addClass('screenLoading');
                $iframe.contents().find('body').html('');
                $iframe.attr("src", targetUrl);
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

                    if ($(this).attr('command') == 'Add') {
                        $(this).attr("rank", (parseInt($(this).attr("rank"), 10) + 1));
                    }
                });

                var $divWindow = $('<div id="dv' + $iframe.attr('id') + '" class = "rcbox"> <span class = "selected" data-menuid="' + menuid + '" data-parentid="' + parentid + '" frameId="' + $iframe.attr('id') + '" command="Add" rank="1">' + windowText + '</span><span data-parentid="' + parentid + '" frameId="' + $iframe.attr('id') + '" command="Remove" controlToRemove="dv' + $iframe.attr('id') + '"></span></div>');
                $('#dvWindows').append($divWindow);

                recentWindowsMenu.populateRecentWindow($iframe, menuid, parentid, targetUrl, windowText);

                $('#spWindowCount').text($('#dvWindows').children().length);
                taskAdded();

                $('html').removeClass('home-page');

                $('#spnSessionDate').addClass('disabled');
                $('#sessionDatelabel').addClass('disabled');
                $('#sessionDateIcon').addClass('disabled');
                $('#sessionDateIcon').removeClass('glyphicon-calendar-1');
                $('#sessionDateIcon').addClass('glyphicon-lock');

                window.scrollTo(0, 0);

                //called help according to screenId i.e menuid
                //Checking the Taskdoc having a generated Report Screen or not
                if (screenId !== reportScreenId) {
                    screenId = menuid;
                } else {
                    screenId = reportScreenHelp;
                }
            }
        });
    }

    $('#nav-home').kendoMenu();
    var targetUrl = "#";

    function isMaxScreenNumReachedAndNotOpen(targetUrl) {
        //Check if maximum number of screens reached
        var isScreenOpen = isScreenAlreadyOpen(targetUrl);
        if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
            $('#dvWindowsExceedLimitErrorMessage').show();
            return true;
        }
        return false;
    }

    // TO DO : Move the below piece of code to Index page where you put your frame
    // Invoked from the main menu
    $(".menu-section a").on("click", function (event) {

        // remove home button activated style
        $('#homeNav').removeClass('active').children("a").removeClass('active');
        $('html').removeClass('home-page');

        // try close the widget add/remove menu no matter what
        $(".container_popUp.Widget.widgetList").hide();

        //Sage Intelligence
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
            if (isScreenAlreadyOpen(targetUrl)) return;
            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                clearIframes();
            }

            // Load breadcrumb on menu item click and add item to windows dock
            var parentidVal = $(this).data('parentid');

            //Get menu id as screenId for Help Menu Search
            screenId = $(this).attr("data-menuid");

            loadBreadCrumb(parentidVal);

            var windowtext = $(event.target).text();

            $('#dvWindows > div').each(function () { $(this).find("span").removeClass('selected'); });

            if ($(this).attr("data-modulename") !== "" && $.parseHTML($(this).attr("data-modulename")) != null && $(this).attr("data-moduleName") != "null") {
                windowtext = portalBehaviourResources.PagetitleInManager.format($(this).attr("data-modulename"), $(event.target).text());
            }

            if ($(this).attr("data-isreport") === "true" || $(this).attr("data-isreport") === "True") {
                //windowtext = windowtext.indexOf("Report") < 0 ? windowtext + " " + portalBehaviourResources.Report : windowtext;
                windowtext = portalBehaviourResources.ReportNameTemplate.format(windowtext, portalBehaviourResources.Report);
            }

            if (targetUrl != "")
                assignUrl(windowtext, parentidVal, screenId);
        }

    });

    // Mouseover (or hover) styles on touch devices 

    //$('.side-nav .menu-item').children('label, a, .nav-icon').on('touchstart', function (e) {
    //    'use strict'; //satisfy code inspectors
    //    var link = $(this); //preselect the link
    //    if (link.hasClass('hover')) {
    //        return true;
    //    } else {
    //        link.addClass('hover');
    //        $('.side-nav .menu-item').children('label, a, .nav-icon').not(this).removeClass('hover');
    //        e.preventDefault();
    //        return false; //extra, and to make sure the function has consistent return points
    //    }
    //});

    $('.icon.msgCtrl-close').click(function () {
        $('#dvWindowsExceedLimitErrorMessage').hide();
    });

    $('#addWidgetBtnProcess').click(function () {
        $('#widgetMsgDiv').hide();
    });
    $('#btnProcess').click(function () {
        $('#msgDiv').hide();
    });

    function loadBreadCrumb(parentidVal) {
        if (!$('#widgetLayout').is(":visible")) {

            //Variables
            var html = [];
            //Add Parent to array
            jQuery.each(MenuList, function (i, val) {
                if (val.Data.MenuId == parentidVal) {
                    var menuName = val.Data.MenuName;
                    if (menuName.indexOf("'") > -1) {
                        menuName = menuName.replace("'", "&#39;");
                    }
                    html = html + "<ul class=bc><li>" + menuName + "<span class=navigation-pipe>:</span></li>";
                }
            });

            //Add Child items to array
            jQuery.each(MenuList, function (i, val) {
                if (val.Data.ParentMenuId == parentidVal && val.Data.IsGroupHeader == false) {
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

            //Invoked from breadcrumb menu
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

                if (isScreenAlreadyOpen(targetUrl)) return;
                if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

                if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                    clearIframes();
                }

                var title = $(this).attr("data-windocktitle");

                if ($(this).attr("data-moduleName") != "" && $(this).attr("data-moduleName") != "null") {
                    //title = $(this).attr("data-moduleName") + " " + $(this).attr("data-windocktitle");
                    title = portalBehaviourResources.PagetitleInManager.format($(this).attr("data-moduleName"), title);
                }

                var parentidVal = $(this).attr("data-parentid");

                //Get menu id as screenId for Help Menu Search
                screenId = $(this).attr("data-menuid");

                $('#dvWindows > div').each(function () { $(this).find("span").removeClass('selected'); });

                if ($(this).attr("data-isreport") === "true" || $(this).attr("data-isreport") === "True") {
                    //title = title.indexOf("Report") < 0 ? title + " " + portalBehaviourResources.Report : title;
                    title = portalBehaviourResources.ReportNameTemplate.format(title, portalBehaviourResources.Report);
                }

                if (targetUrl != "") {
                    assignUrl(title, parentidVal, screenId);
                }
            });

        }
    };
    //Check whether the screen is opened, use currently src url and original url 
    function isScreenAlreadyOpen(url) {
        var result = false;
        //Check if the screen is already open
        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            var srcUrl = $iframe.attr("src");
            var originalUrl = $iframe.prop("originalSrc");
            result = (srcUrl === url) || (originalUrl && originalUrl === url);
            if (result) {
                $("#dvWindows span[command='Add'][frameid='" + $iframe[0].id + "']").trigger("click");
                return false;
            }
        });

        return result;
    }

    // Function to handle messages posted from other windows (e.g. from a web screen within
    // a child IFrame).  These messages request that the portal take some action, such as
    // opening a report or screen, or showing Notes for a given entity (e.g. an AR Customer).
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

    function createInquiryURLWithParameters(inquiryParameter)
    {
        return sg.utls.formatString("{0}/?module={1}&inquiryType={2}&target={3}&value={4}&title={5}&name={6}",
            inquiryParameter.url, inquiryParameter.module, inquiryParameter.feature, inquiryParameter.target, inquiryParameter.value, encodeURIComponent(inquiryParameter.title), encodeURIComponent(inquiryParameter.name));
    }

    // Function to handle opening of Reports and Screen as a New Task Window.
    // NOTE: The caller is responsible for checking that evtData (the data from the window
    //       message presumably received from another IFrame) is a string.
    function openNewTask(evtData) {
        // Display Reports as a New Task Doc Item
        if (evtData.indexOf("isInquiry") >= 0) {
            var postMessageData = evtData.split(" ");
            var inquiryParameter = JSON.parse(decodeURI(postMessageData[1]));
            targetUrl = createInquiryURLWithParameters(inquiryParameter);
            var screenName, parentId, menuid;

            $('#screenLayout').show();
            $('#widgetLayout').hide();

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                clearIframes();
            }

            //do not show the breadcrumb for inquiry reports
            $('#breadcrumb').hide();
            
            postMessageData.splice(postMessageData.length - 1, 1);

            menuid = inquiryScreenId;

            //Method To Load Into Task Doc
            assignUrl(portalBehaviourResources.Inquiry + " - " + inquiryParameter.title, parentId, menuid);

        } else if (evtData.indexOf("isReport") >= 0) {

            var postMessageData = evtData.split(" ");
            targetUrl = postMessageData[1];
            var reportName = postMessageData.splice(0, 2);
            var screenName, parentId, menuid;

            var urlParser = $('<a>', { href: postMessageData[postMessageData.length - 1] })[0];
            var a = $("#listPrimary li > a[data-url='" + urlParser.pathname + "']");
            var isReport = a.data("isreport");

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                clearIframes();
            }

            //do not show the breadcrumb for printed reports
            $('#breadcrumb').hide();
            // Checking isKPI so that reportName is appended to show in windows doc when opened from KPI.
            if (isKPI == true) {
                var windowText = kpiReportName;
                isKPI = false;
            } else {
                postMessageData.splice(postMessageData.length - 1, 1);
                var windowText = postMessageData.join(" ");
            }

            if ((isReport === 'True' || isReport === 'true') && windowText.indexOf(portalBehaviourResources.Report) === -1) // only add Report to the window name if it is a report page and it does not have one
            {
                windowText = portalBehaviourResources.ReportNameTemplate.format(windowText, portalBehaviourResources.Report);
            }
            windowText = windowText + " - " + portalBehaviourResources.Printed;

            menuid = reportScreenHelp;

            menuid = reportScreenHelp;

            //Method To Load Into Task Doc
            assignUrl(windowText, parentId, menuid);

            //Update Help Menu after Report Generated from a Screen
            screenId = reportScreenHelp;
        }
            // Display Screen as a New Task Doc Item
        else if (evtData.indexOf("isScreen") >= 0) {
            var postMessageData = evtData.split(" ");
            targetUrl = postMessageData[1];

            if (isMaxScreenNumReachedAndNotOpen(targetUrl)) return;

            if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                clearIframes();
            }

            /*
             Sample targetUrl
             "/Sage300/OnPremise/AP/InvoiceEntry/Index?batchNumber=25&toAction=/Sage300/OnPremise/AP/InvoiceBatchList&actionType=EditBatch"
            */

            var screenName;
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

            //Method To Load Into Task Doc
            assignUrl(screenName, parentScreenID, screenID);
        }
    }

    // Function to handle showing of Notes for a given entity (e.g. for an AR Customer).
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

    // Function to handle hiding (closing) of Notes center.
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

    //Event Listener to handle post message events
    if (window.addEventListener) {
        // For standards-compliant web browsers
        window.addEventListener("message", receiveWindowMessage, false);
    } else {
        window.attachEvent("onmessage", receiveWindowMessage);
    }

    $(window).resize(function () {
        resizeLayout();
    });

    function resizeLayout() {
        //widgets layout container, widgets help layout container
        $('.body_container,#widgetHplayout').each(function () {
            var iframeHeight = $(window).height() - 184;
            $(this, '#widgetHplayout').css('min-height', iframeHeight);
        });

        //first time user layout, screen layout
        $('#firstTimeLogin').each(function () {
            var docHeight = $(document).height();
            $('#firstTimeLogin .overlay').css('min-height', docHeight);
        });
    }

    resizeLayout();

    $("#liSignOut").html('<a id="lnkSignOut" href="javascript:void(0);" class="k-link">' + portalBehaviourResources.SignOut + '</a>');
    $("#liManageUsers").html('<a id="lnkUserManagment" href="javascript:void(0)" class="k-link">' + portalBehaviourResources.ManageUsers + '</a>');

    $("#lnkSignOut").bind('click', function () {
        sg.utls.logOut();
    });

    $("#liManageUsers").bind('click', function () {
        window.open(umURL);
    });

    $("#btnIntelligence").click(function () {
        var cb = ""; //cachebuster
        if (sg.utls.isInternetExplorer()) {
            cb = "&cb=" + encodeURI((new Date()).toString() + Math.floor(Math.random() * 10000000)); // generate date + random number to make the URL unique
        }

        // attemp to signout and start SIRC afterward
        OAuthWindow = window.open(oAuthLocation + "/connect/endsession");
    });
});

window.addEventListener("message", function (e) {
    if (e.data === "SignedOut" && OAuthWindow) {
        OAuthWindow.close();
        var cb = ""; //cachebuster
        if (sg.utls.isInternetExplorer()) {
            cb = "&cb=" + encodeURI((new Date()).toString() + Math.floor(Math.random() * 10000000)); // generate date + random number to make the URL unique
        }
        window.open(sg.utls.url.buildUrl("WebApiProxy?" + cb, "", ""));
    }
}, false);

