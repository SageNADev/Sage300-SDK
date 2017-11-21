/* Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved. */

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
    var menuPinned = true;

    //to stop spinner
    $('#screenLayout').children().find("iframe").load(function () {
        $(this).removeClass('screenLoading');
    });

    screenId = defaultScreenId;
    $('#searchHelpDiv').show();

    $('#xmlMenuDiv').show();

    var isWidgetEmptyLnkClicked = false;

    var menu = $("#topMenu").kendoMenu({ open: onOpen, close: onClose }).data("kendoMenu");

    function onOpen(e) { if ($(e.item).children(".k-link").text() == '') $(".main-search input").css("opacity", "0.3").attr("disabled", "disabled"); }

    function onClose(e) { if ($(e.item).children(".k-link").text() == '') $(".main-search input").css("opacity", "1").removeAttr("disabled"); }

    var isPinMenuClicked = false;

    /*
    $('#lnkLogo').click(function () {
        $('#dvWindows > div').each(function () {
            $(this).find("span").removeClass('selected');
        });
        ShowHomePage();
    });
    */

    // Collapsible header
    $(window).scroll(function () {
        if ($(this).scrollTop() >= 47) {
            if (menuPinned) {
                $('html').addClass('collapsed');
                $('.feature_nav').removeClass('active');

            }
        } else {
            $('html').removeClass('collapsed');
        }
    });

    // Collapsible Header - enabling / disabling toggle
    //$('#topMenuSettings').click(function () {
    //    menuPinned = !menuPinned;
    //});

    $('#pinMainNav').hide(); // by default: pinned, so hide pin menu item

    $('#pinMainNav').click(function () {
        menuPinned = true;
        setTimeout(hidePinMenu, 500);
    });
    function hidePinMenu() {
        $('#pinMainNav').hide();
        $('#unpinMainNav').show();
    }

    $('#unpinMainNav').click(function () {
        menuPinned = false;
        setTimeout(hideUnpinMenu, 500);
    });
    function hideUnpinMenu() {
        $('#pinMainNav').show();
        $('#unpinMainNav').hide();
    }


    $(document).click(function (e) {
        if (!isWidgetEmptyLnkClicked) {
            $('.container_popUp.Widget.widgetList').hide();
        }
        isWidgetEmptyLnkClicked = false;
    });

    $('#spnCloseWidget').click(function () {
        $('#DivWidgetWindow').hide();
    });

    $("#home_nav").kendoMenu({

    });

    $(".home_nav").click(function () {
        ShowHomePage();
    });

    $("ul#home_nav li.main").children().addClass("mainNav");

    $('#lnkAddWidgets, .GoArrow,  #lblSeeIntoYourData').click(function () {
        isWidgetEmptyLnkClicked = true;
        $('#addRemoveWidget').show();
    });

    $(".top-buttons.addWidgets").click(function () {
        $('#addRemoveWidget').show();
    });

    $(".portalIcon.closeIcon").click(function () {
        $("this").closest(".container_popUp.Widget.widgetList").hide("fast");
    });

    $("ul#home_nav").children().children().children().addClass("k-iconNone");

    $(".portalIcon.checkBox span.checkBox").addClass("portalIcon");

    $(".portalIcon.checkBox span.checkBox").removeClass("icon");

    initializeMainMenu();
    initializeExtraMenu();

    // Initialize count of currently open UI windows (it should be 0)
    $('#spWindowCount').text($('#dvWindows').children().length);

    firstControl["control"] = "widgetLayout";
    firstControl["rank"] = 1;
    controls.push(firstControl);
    $("#draggable").draggable({ axis: "y", containment: "window" });
    clearIframes();
    $('#dvCloseWindowErrorMessage').hide();
    $('.task_added').hide();
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

    $('.top_nav_drop_content').click(function () {

        isReload = false;
    });

    $(window).bind('beforeunload', function () {
        var numOfOpenScreens = $('#dvWindows').children().length;
        if (isReload && numOfOpenScreens > 0) {
            return sg.utls.htmlDecode(portalBehaviourResources.PageRefreshError);
        }
    });

    function ShowHomePage() {
        $('#screenLayout').hide();
        $('#widgetLayout').show();

        //When footer logo is clicked
        screenId = defaultScreenId;

        $('#breadcrumb').hide();

        if (!$('#screenLayout').is(":visible")) {
            updateLayout();
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
        var $menu = $(".navigation .std-menu");

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

            $row.find("span:first").addClass("active");
        }

        function deactivateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap");

            // Hide the submenu and remove the row's highlighted look
            $submenu.css("display", "none");
            $row.find("span:first").removeClass("active");
        }

        function exitSubmenu(row) {
            var $row = $(row);
            $row.find(".sub-menu-wrap").hide().eq(0).show();
        }

        $(".navigation .std-menu li").click(function (e) {
            e.stopPropagation();
        });

        $menu.find(".menu-section li:not('.sub-heading')").click(function () {
            $(".std-menu").addClass("deactive").find("> li:not(:first-child) .sub-menu-wrap").css("display", "none");
            $(".nav-menu span.active").removeClass("active");
        });

        $(".navigation .feature_nav").hover(
            function () {
                $(this).find(".active").removeClass("active");
                $(this).find("li:first span:first").addClass("active");
                $(this).find(".deactive").removeClass("deactive");
            },
            function () {
                $(this).find("li:first span:first").removeClass("active");
            }
        );
    }

    function initializeMainMenu() {
        var $menu = $(".nav-menu .std-menu");

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

            $row.find("span:first").addClass("active");
        }

        function deactivateSubmenu(row) {
            var $row = $(row),
                $submenu = $row.find(".sub-menu-wrap");

            // Hide the submenu and remove the row's highlighted look
            $submenu.css("display", "none");
            $row.find("span:first").removeClass("active");
        }

        function exitSubmenu(row) {
            var $row = $(row);
            $row.find(".sub-menu-wrap").hide().eq(0).show();
        }

        $(".nav-menu .std-menu li").click(function (e) {
            e.stopPropagation();
        });

        $menu.find(".menu-section li:not('.sub-heading')").click(function () {
            $(".std-menu").addClass("deactive").find("> li:not(:first-child) .sub-menu-wrap").css("display", "none");
            $(".nav-menu span.active").removeClass("active");
        });

        $(".nav-menu .top-tier").hover(
            function () {
                $(this).find(".active").removeClass("active");
                $(this).find("li:first span:first").addClass("active");
                $(this).find(".deactive").removeClass("deactive");
            },
            function () {
                $(this).find("li:first span:first").removeClass("active");
            }
        );
    }

    $('#homeNav').click(function () {
        $('.feature_nav').removeClass('active');

        $(this).addClass('active');

        $('#dvWindows > div').each(function () {
            $(this).find("span").removeClass('selected');
        });
    });

    $('#homeNav').hover(function () {
        $('.feature_nav').removeClass('active');
    });

    $('.global_nav').hover(function () {
        $('.feature_nav').removeClass('active');
    });

    $("#btnIntelligence")
        .click(function () {
            $('.feature_nav').removeClass("active");
    });

    $("#SIR")
        .hover(function () {
            $(this).addClass("active");
        }, function () {
            $(this).removeClass("active");
    });

    $(".feature_nav")
        .hover(function () {
            $(this).addClass("active");
        }, function () {
            $(this).removeClass("active");
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

                    $('#spnSessionDate').removeClass('disabled');
                    $('#sessionDatelabel').removeClass('disabled');
                    $('#sessionDateIcon').removeClass('disabled');
                    $('#sessionDateIcon').removeClass('glyphicon-lock');
                    $('#sessionDateIcon').addClass('glyphicon-calendar-1');
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
        });
    });

    $("#dvWindows").on("click", "span", function () {
        currentIframeId = $(this).attr("frameId");

        //Adding Class Selected to the Active Window
        $('div#dvWindows span').not($(this)).removeClass('selected');
        $(this).addClass('selected');

        if ($(this).attr('command') === "Remove") {
            isIframeClose = true;
            currentDiv = ($(this).attr('controlToRemove'));
            flag = false;
            $.each(controls, function (index, element) {
                if (element["control"] === currentIframeId) {
                    currentRank = element["rank"];
                }
            });
            
            var iframeHtml = sg.utls.formatString("<iframe scrolling='no' sandbox='allow-forms allow-popups allow-pointer-lock allow-same-origin allow-scripts allow-top-navigation' id='{0}' src='' class='screenIframe' style='display: none;'></iframe>",
                currentIframeId);

            $("#" + currentIframeId).attr("src", "about:blank");
            $("#" + currentIframeId).parent().empty().append(iframeHtml);

            iFrameLoadEvent(null, $("#" + currentIframeId));

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
                $('#spWindowCount').text($('#dvWindows').children().length);
                taskAdded();

                $('#spnSessionDate').addClass('disabled');
                $('#sessionDatelabel').addClass('disabled');
                $('#sessionDateIcon').addClass('disabled');
                $('#sessionDateIcon').removeClass('glyphicon-calendar-1');
                $('#sessionDateIcon').addClass('glyphicon-lock');

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

    $('#home_nav').kendoMenu();
    var targetUrl = "#";

    // TO DO : Move the below piece of code to Index page where you put your frame
    // Invoked from the main menu
    $(".menu-section a").on("click", function (event) {

        // try close the widget add/remove menu no matter what
        $(".container_popUp.Widget.widgetList").hide();
        $('.feature_nav').removeClass("active");

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
            if ($(event.target).data('url') != " ")
                targetUrl = $(event.target).data('url');


            //Check if maximum number of screens reached
            var isScreenOpen = isScreenAlreadyOpen(targetUrl);
            if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
                $('#dvWindowsExceedLimitErrorMessage').show();
                return;
            }

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

        //close menu
        $(".nav-menu .top-tier").each(function () {
            $(this).find(".active").removeClass("active");
        });

    });

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

                //Check if maximum number of screens reached
                var isScreenOpen = isScreenAlreadyOpen(targetUrl);
                if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
                    $('#dvWindowsExceedLimitErrorMessage').show();
                    return;
                }

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

    function isScreenAlreadyOpen(url) {
        var result = false;
        //Check if the screen is already open
        $('#screenLayout').children().each(function () {
            var $iframe = $(this).find("iframe");
            if (sg.utls.getUrlPath($iframe.attr("src")) === sg.utls.getUrlPath(url)) {
                result = true;
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
        return sg.utls.formatString("{0}/?module={1}&inquiryType={2}&target={3}&value={4}",
            inquiryParameter.url, inquiryParameter.module, inquiryParameter.feature, inquiryParameter.target, inquiryParameter.value);
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
            

            //Check if maximum number of screens reached
            var isScreenOpen = isScreenAlreadyOpen(targetUrl);
            if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
                $('#dvWindowsExceedLimitErrorMessage').show();
                return;
            }

            if ($('#dvWindows').children().length <= numberOfActiveWindows) {
                clearIframes();
            }

            //do not show the breadcrumb for inquiry reports
            $('#breadcrumb').hide();
            
            postMessageData.splice(postMessageData.length - 1, 1);

            menuid = inquiryScreenId;

            //Method To Load Into Task Doc
            assignUrl(portalBehaviourResources.Inquiry, parentId, menuid, true);

        } else if (evtData.indexOf("isReport") >= 0) {

            var postMessageData = evtData.split(" ");
            targetUrl = postMessageData[1];
            var reportName = postMessageData.splice(0, 2);
            var screenName, parentId, menuid;

            var urlParser = $('<a>', { href: postMessageData[postMessageData.length - 1] })[0];
            var a = $("#xmlMenuDiv li > a[data-url='" + urlParser.pathname + "']");
            var isReport = a.data("isreport");

            //Check if maximum number of screens reached
            var isScreenOpen = isScreenAlreadyOpen(targetUrl);
            if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
                $('#dvWindowsExceedLimitErrorMessage').show();
                return;
            }

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

            //Check if maximum number of screens reached
            var isScreenOpen = isScreenAlreadyOpen(targetUrl);
            if (!isScreenOpen && $('#dvWindows').children().length >= numberOfActiveWindows) {
                $('#dvWindowsExceedLimitErrorMessage').show();
                return;
            }

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

        // call WebApiProxy to get temp token
        sg.utls.ajaxGet(sg.utls.url.buildUrl("WebApiProxy?generateSession=true" + cb, "", ""), {}, function (result) {
            if (result) {
                if (!sIRCLocation) {
                    // if no SIRC location is defined, assume it would be on the same location with https (ie https://<current server>/)
                    sIRCLocation = "https://" + location.hostname + ":" + SIRCPort + "/";
                }

                var win = window.open(sIRCLocation + '?tempCode=' + result.TempSessionId + '&locale=' + result.UserLanguage, '_blank');
            }
        });
    });
});
