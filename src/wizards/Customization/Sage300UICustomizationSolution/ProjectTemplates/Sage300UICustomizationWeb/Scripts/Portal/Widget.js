/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict"
var homeUI = homeUI || {}; 
var widgetUI = widgetUI || {};
var widgetConstants = widgetConstants || {};
var widgetOrder;

widgetUI = {
    NavigableMenuDetail: { WidgetOrders: "" },
    hasKoApplied: false,
    isKendoControlNotInitialised: false,
    displayOptions: function (result) {
        if (result != null) {
            if (!homeUI.hasKoApplied) {
                homeUI.homeViewModel = ko.mapping.fromJS(result);
                ko.applyBindings(homeUI.homeViewModel, $("#divDatePicker")[0]);
                homeUI.hasKoApplied = true;
            } else {
                ko.mapping.fromJS(result, homeUI.homeViewModel);
            }
        }
    },
};

homeUI = {
    homeViewModel: { WidgetOrders: "" },
    hasKoApplied: false,
    isKendoControlNotInitialised: false,
    displayOptions: function (result) {
        if (result != null) {
            if (!homeUI.hasKoApplied) {
                homeUI.homeViewModel = ko.mapping.fromJS(result);
                ko.applyBindings(homeUI.homeViewModel, $("#divDatePicker")[0]);
                homeUI.hasKoApplied = true;
            } else {
                ko.mapping.fromJS(result, homeUI.homeViewModel);
            }
        }
    },
};

widgetConstants = {
    WidgetHeight: 315,
    WidgetWidth: 460,
    WidgetMarginRight: 20,
    WidgetMarginBottom: 20,
    NumOfWidgetsMax: 6,
    NumOfWidgetsPerRow: 2,
}

var slotManager = new function () {

    this.slots = [];

    this.sortSlotsByRank = function () {
        this.slots.sort(function (a, b) {
            if (a.rank < b.rank)
                return -1;
            if (a.rank > b.rank)
                return 1;
            return 0;
        });
    }
    this.recalculateRanks = function () {

        this.sortSlotsByRank();
        var newRank = 1
        $.each(this.slots, function (i, val) {
            if (val.rank != -1) {
                val.rank = newRank++;
            }
        });
    }
    this.getNextRank = function () {
        this.sortSlotsByRank();

        var result = 0;
        $.each(this.slots, function (i, val) {
            if (val.rank != -1)
                result = val.rank > result ? val.rank : result;
        });

        result++;
        if (result > widgetConstants.NumOfWidgetsMax)
            return -1;

        return result;
    }
    this.registerSlot = function (slotId, widgetId, rank) {
        var slot = this.getSlotById(slotId);
        if (slot != null) {
            slot.rank = rank;
            slot.widgetId = widgetId;
            this.recalculateRanks();            
        }
        else {
            slot = { slotId: slotId, widgetId: widgetId, rank: rank };
            this.slots.push(slot);            
        }
        return slot;
    }
    this.unregisterSlot = function (slotId) {
        var slot = this.getSlotById(slotId);
        if (slot != null) {
            this.slots.splice(this.slots.indexOf(slot), 1);
            this.recalculateRanks();

            return true;
        }
        return false;
    }
    this.getSlotById = function (slotId) {
        var result = $.grep(this.slots, function (e) { return e.slotId == slotId; });
        if (result.length == 0)
            return null;
        return result[0];
    }
    this.getSlotByWidgetId = function (widgetId) {
        var result = $.grep(this.slots, function (e) { return e.widgetId == widgetId; });
        if (result.length == 0)
            return null;
        return result[0];
    }
    this.getActiveSlotsCount = function () {
        var result = 0;
        $.each(this.slots, function (i, val) {
            if (val.rank != -1)
                result++;
        });
        return result;
    }
    this.isWidgetAlreadyOpen = function (widgetId) {
        var slot = this.getSlotByWidgetId(widgetId);
        return slot != null && slot.rank != -1;
    }
}

function updateLayout() {

    //update slot positions
    $('.bodyWidgetContainer .kpi').each(function () {

        var slotId = $(this).attr('id');
        var slot = slotManager.getSlotById(slotId);
        if (slot == null || slot.rank == -1){            
            $(this).attr('rank', -1);
            
            $(this).addClass('hide').css('top', 0).css('left', 0);
        }
        else {
            $(this).attr('rank', slot.rank);

            var top = Math.floor((slot.rank - 1) / widgetConstants.NumOfWidgetsPerRow) * (widgetConstants.WidgetHeight + widgetConstants.WidgetMarginBottom);
            var left = ((slot.rank - 1) % widgetConstants.NumOfWidgetsPerRow) * (widgetConstants.WidgetWidth + widgetConstants.WidgetMarginRight)
            
            $(this).removeClass('hide').css('top', top).css('left', left);
        }
    });

    //set container height
    var numberOfActiveWidgets = slotManager.getActiveSlotsCount();
    $('.bodyWidgetContainer').height((widgetConstants.WidgetHeight + widgetConstants.WidgetMarginBottom) * Math.ceil(numberOfActiveWidgets / 2));

    //bodyWidgetContainer
    if (numberOfActiveWidgets > 0) {
        $('#widgetLayout').show();
        $('#widgetHplayout').hide();
        $('#dvAddWidget').show();
    } else {
        $('#widgetHplayout').show();
        $('#widgetLayout').hide();
        $('#dvAddWidget').hide();
    }
}

function loadWidgets(widgetInfos)
{
    var slotIndex = 1;
    $.each(widgetInfos, function (i, val) {
        var rank = parseInt(val.Rank);
        if (rank >= 1 && rank <= widgetConstants.NumOfWidgetsMax && slotIndex <= widgetConstants.NumOfWidgetsMax) {
            var slotId = 'widgetSlot' + slotIndex;

            addWidgetToSlot(val.WidgetId, slotId, rank);
            ++slotIndex;
        }
    });
    slotManager.recalculateRanks();
    updateLayout();
}

function displayWidget(widgetId) {

    var widgetInfo = getWidgetInfoById(widgetId);
    if (widgetInfo == null)
        return false;

    var slotId = getNextAvailableSlotId();
    if (slotId == null)
        return false;
    
    var rank = slotManager.getNextRank();
    if (rank == -1)
        return false;
    
    if (!addWidgetToSlot(widgetId, slotId, rank))
        return false;

    return true;
}

function hideSlot(slotId) {
    var slot = slotManager.getSlotById(slotId);
    if (slot == null)
        return false;

    if (slotManager.unregisterSlot(slot.slotId)) {
        var $iframe = $('#' + slot.slotId).find('iframe');
        $iframe = setWidgetFrameSource($iframe.attr('id'), "");
        $iframe.attr("IsConfigurable", "");
        $iframe.attr("IsReport", "");
        $iframe.attr("widgetid", "");
    }
    updateLayout();
    return true;
}

function addWidgetToSlot(widgetId, slotId, rank)
{
    var slot = slotManager.registerSlot(slotId, widgetId, rank);
    if (slot != null) {
        updateLayout();

        var widgetInfo = getWidgetInfoById(widgetId);
        if (widgetInfo != null)
        {
            var $iframe = $('#' + slotId).find('iframe');
            $iframe = setWidgetFrameSource($iframe.attr('id'), sg.utls.url.baseUrl() + "KPI/" + widgetInfo.ScreenUrl);
            $iframe.attr("IsConfigurable", widgetInfo.IsConfigurable);
            $iframe.attr("IsReport", widgetInfo.IsReport);
            $iframe.attr("widgetid", widgetId);

            return true;
        }
    }

    return false;
}

function getWidgetInfoById(widgetId)
{
    var result = $.grep(widgetOrder, function (e) { return e.WidgetId == widgetId; });
    if (result.length == 0)
        return null;
    return result[0];
}

function getNextAvailableSlotId()
{
    var $vacantSlots = $('.bodyWidgetContainer .kpi[rank="-1"]');
    if ($vacantSlots.length)
        return $vacantSlots.first().attr('id');

    return null;
}

//Sets widget frame source. 
//This function will recreate the iFrame in order to prevent the browser from showing history for iframes
function setWidgetFrameSource(iframeId, src)
{
    var $originalFrame = $('#' + iframeId);
    var $newFrame = $("<iframe sandbox='allow-forms allow-popups allow-pointer-lock allow-same-origin allow-scripts' scrolling='no' class='iframeContainer'></iframe>");
    $newFrame.attr('id', $originalFrame.attr('id'));
    $newFrame.attr('width', $originalFrame.attr('width'));
    $newFrame.attr('height', $originalFrame.attr('height'));
    $newFrame.attr('IsConfigurable', $originalFrame.attr('IsConfigurable'));
    $newFrame.attr('IsReport', $originalFrame.attr('IsReport'));
    $newFrame.attr('widgetid', $originalFrame.attr('widgetid'));
    $originalFrame.replaceWith($newFrame);

    $newFrame.addClass('widgetLoading');
    $newFrame.load(function () {
        $newFrame.removeClass('widgetLoading');
    });

    $newFrame.attr('src', src);

    return $newFrame;
}

//sets configuration frame source
//This function will recreate the iFrame in order to prevent the browser from showing history for iframes
function setConfigFrameSource(iframeId, src)
{
    var $originalFrame = $('#' + iframeId);
    var $newFrame = $("<iframe sandbox='allow-forms allow-popups allow-pointer-lock allow-same-origin allow-scripts' scrolling='no'></iframe>");
    $newFrame.attr('id', $originalFrame.attr('id'));
    $newFrame.attr('width', $originalFrame.attr('width'));
    $newFrame.attr('height', $originalFrame.attr('height'));
    $originalFrame.replaceWith($newFrame);

    $newFrame.addClass('widgetLoading');
    $newFrame.load(function () {
        $newFrame.removeClass('widgetLoading');
    });

    $newFrame.attr('src', src);

    return $newFrame;
}

//renders empty div inside the frame to wipe out its content
function cleanFrameContent($iframe)
{
    var doc = $iframe.get(0).contentWindow.document;
    doc.write("<div style='width: 100%; height: 100%'></div>");
}

$(document).ready(function () {
    $(".home_nav, .footer_container").click(function () {
        if (!$('#screenLayout').is(":visible")) {
            updateLayout();
        }
        $("#breadcrumb").hide();
        //When Home Menu is clicked
        screenId = defaultScreenId;
    });

    widgetOrder = $.parseJSON(NavigableMenuDetail.WidgetOrders);

    loadWidgets(widgetOrder);

    refreshWidgetMenu(widgetOrder);

    $("ul.showWidget input[type='checkbox'].chkWidget").on('change', function (event) {

        //id of the clicked checkbox in the Widgets menu
        var chkId = $(this).attr("id");
        //id of the widget selected (e.x "KPI018" or "KPI019"). 
        var widgetId = $(this).attr("widgetid");

        if ($(this).is(":checked") && !slotManager.isWidgetAlreadyOpen(widgetId))
        {
            if (slotManager.getActiveSlotsCount() < widgetConstants.NumOfWidgetsMax)
            {
                displayWidget(widgetId);
            }
            else
            {
                $('#widgetMsgDiv').fadeIn().removeClass('hide');
                $('#' + chkId).attr('checked', false);
                $('#' + chkId).parent().removeClass('selected');
            }
        }
        else
        {
            var slot = slotManager.getSlotByWidgetId(widgetId);
            if (slot != null)
            {
                hideSlot(slot.slotId);
                $('#widgetMsgDiv').fadeOut().addClass('hide');
            }
        }

        saveWidgetOrder();
        $("#addRemoveWidget").show();
    });

    DragAndDropInit();
});

function refreshWidgetMenu()
{
    $("#lstWidget li :checkbox").each(function () {
        var widgetId = $(this).attr('widgetId');
        var slot = slotManager.getSlotByWidgetId(widgetId);
        if (slot != null && slot.rank > 0) {
            $(this).prop('checked', true);
            $(this).parent().addClass('selected');
        }
    });
}

function DragAndDropInit() {

    //set up drag and drop for widget slots
    $(".kpi").each(function () {
        var elem = $(this).get()[0];
        elem.addEventListener('dragstart', myDragStart, false);
        elem.addEventListener('dragend', myDragEnd, false);

        elem.addEventListener('dragenter', myDragEnter, false);
        elem.addEventListener('dragover', myDragOver, false);
        elem.addEventListener('dragleave', myDragLeave, false);

        elem.addEventListener('drop', myDrop, false);
        elem.addEventListener('drag', myDrag, false);
    });

    //set up drag and drop for files
    //In real life you would show a fallback form if no drag and drop support.
    $(".filedrop").each(function () {
        var fileBox = $(this).get()[0];

        fileBox.addEventListener("dragover", myFileBoxOver, false);
        fileBox.addEventListener("dragleave", myFileBoxLeave, false);
        fileBox.addEventListener("drop", myFileDrop, false);
        fileBox.style.display = "block";
        fileBox.addEventListener('dragenter', myDragEnter, false);
        fileBox.addEventListener('dragleave', myDragLeave, false);
    });
}

function myFileBoxOver(e) {
    if (e.preventDefault) {
        e.preventDefault();
    }
    this.classList.add('over');
}

function myFileBoxLeave(e) {
    if (e.preventDefault) {
        e.preventDefault();
    }
    this.classList.remove('over');
}

function myFileHandler(e, file) {

    var reader = new FileReader();
    reader.onerror = function (e) {
        alert('There was an error reading the file: ' + e.target.error.code);
    };
    var target = e.target;
    $(target).empty();
    if (file.type.indexOf("text") == 0) {
        reader.onload = function (e) {
            var text = e.target.result;
            $(target).text(e.target.result);
        }
        reader.readAsText(file);
    } else if (file.type.indexOf("image") == 0) {
        $(target).append('<img id="loaded-image"/>');
        reader.onload = function (e) {
            $("#loaded-image").attr('src', e.target.result);
        }
        reader.readAsDataURL(file);
    } else {
        $(target).append("<p>file name: " + file.name + "</p>");
        $(target).append("<p>file type: " + file.type + "</p>");
        $(target).append("<p>file size: " + file.size + "</p>");
    }
}

function myFileDrop(e) {
    if (e.stopPropagation) {
        e.stopPropagation();
    }
    if (e.preventDefault) {
        e.preventDefault();
    }
    this.classList.remove('over');
    myFileSelect(e);
    return false;
}

function myFileSelect(e) {
    // get FileList object
    var files = e.target.files || e.dataTransfer.files;
    // process all File objects
    for (var i = 0; files[i]; i++) {
        myFileHandler(e, files[i]);
    }
}

function myDragOver(e) {
    if (e.preventDefault) {
        e.preventDefault();
    }
    e.dataTransfer.dropEffect = 'move';
    return false;
}

function myDragEnter(e) {
    this.classList.add('over');
    if (e.preventDefault) {
        e.preventDefault();
    }
    return true;
}

function myDragLeave(e) {
    this.classList.remove('over');
}

function myDrop(e) {

    if (e.stopPropagation) {
        e.stopPropagation();
    }
    if (e.preventDefault) {
        e.preventDefault();
    }
    
    var sourceSlotId = e.dataTransfer.getData('text');
    var destSlotId = this.id;

    var sourceSlot = slotManager.getSlotById(sourceSlotId);
    var destSlot = slotManager.getSlotById(destSlotId);

    if (sourceSlot != null && destSlot != null)
    {
        var rank = sourceSlot.rank;
        sourceSlot.rank = destSlot.rank;
        destSlot.rank = rank;

        updateLayout();

        saveWidgetOrder();        
    }

    return false;
}

function saveWidgetOrder() {
    
    //put current ranks back into the widgetOrder collection
    $.each(widgetOrder, function (i, val) {
        var slot = slotManager.getSlotByWidgetId(val.WidgetId);
        val.Rank = slot != null ? slot.rank : -1;
    });

    var data = { widgetOrder: JSON.stringify(widgetOrder)};

    sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Home", "PutWidgetOrder"), data, function (result) {

    });
    return false;
}

function myDrag(e) {
    // do things here if you need to while dragging
}

function myDragEnd(e) {

    this.style.opacity = '1.0';
    $(".kpi").each(function () {
        this.classList.remove('over');
    });
}

function myDragStart(ev) {
    this.style.opacity = '0.4';
    ev.dataTransfer.effectAllowed = 'move';
    var textData = ev.target.getAttribute('id');
    ev.dataTransfer.setData("text", textData);
    return true;
}

$(document).ready(function () {

    // Setting popup
    $(document).on('click', '.btnsettingsPopUp', function (e) {

        var $slot = $(this).closest('.kpi');
        var $iframe = $slot.find('iframe');

        $slot.find('li.btnEditSettings').toggle($iframe.attr('isConfigurable') === 'true');
        $slot.find('li.btnOpenReport').toggle($iframe.attr('isReport') === 'true');

        e.stopPropagation();

        $('.container_popUp.Widget.widgetList').hide();
        $(this).next('.container_popUp.Widget.widgetList').show();
    });
    $(document).on('click', '.top-buttons.addWidgets,.container_popUp.Widget.widgetList', function (e) {
        e.stopPropagation();
        $(this).next('.container_popUp.Widget.widgetList').fadeIn(300);
    });
    // Setting popup
    $(".portalIcon.closeIcon").click(function () {
        $(this).closest(".container_popUp.Widget.widgetList").hide("fast");
    });
    // Hide specific widget
    $(document).on('click', '#widgetLayout .btnClose', function() {

        var slotId = $(this).closest(".kpi").attr("id");
        var slot = slotManager.getSlotById(slotId);
        if (slot != null) {            

            //hide the slot
            hideSlot(slot.slotId);            
            //uncheck menu checkbox
            var $checkbox = $('#addRemoveWidget').find('#chk' + slot.widgetId);
            $checkbox.prop('checked', false)
            $checkbox.parent().removeClass('selected');

            saveWidgetOrder();
        }
    });

    //Settings Pop Up Inside an Iframe
    $(document).on('click', '.widgetSettings', function (e) {
        $('.container_popUp.Widget.widgetList').hide();
        var $iframe = $(this).closest('.kpi').find('iframe');
        var currentFrameId = $iframe.attr('id');
        var currentFrameUrl = $iframe.attr('src');

        var configurationUrl = currentFrameUrl + "/WidgetConfiguration";
        var editTitleUrl = currentFrameUrl + "/EditTitle";

        if($(e.target).hasClass('btnEditSettings')) {

            if (configurationUrl.indexOf("AgedPayable") >= 0 || configurationUrl.indexOf("AgedReceivable") >= 0 || configurationUrl.indexOf("CashPosition") >= 0 || configurationUrl.indexOf("IncomeFromOperation") >= 0 || configurationUrl.indexOf("AccountBalance") >= 0 || configurationUrl.indexOf("InventoryItemPerformance") >= 0 || configurationUrl.indexOf("SalesPerSalesperson") >= 0) {
                
                cleanFrameContent($('#iframeWidgetConfiguration1'));
                $('#configurationWindow1').show();
                $("#configurationWindow1").kendoWindow({
                    draggable: false,
                    modal: true,
                    iframe: true,
                    title: portalBehaviourResources.Settings,
                    actions: ["Close"],
                    resizable: false,
                    width: 843,
                    activate: function () {
                        setConfigFrameSource('iframeWidgetConfiguration1', configurationUrl);
                    }
                }).data("kendoWindow").center().open();
            }
            else {

                cleanFrameContent($('#iframeWidgetConfiguration'));
                $('#configurationWindow').show();
                $("#configurationWindow").kendoWindow({
                    draggable: false,
                    modal: true,
                    iframe: true,
                    title: portalBehaviourResources.Settings,
                    actions: ["Close"],
                    resizable: false,
                    width: 640,
                    activate: function () {
                        setConfigFrameSource('iframeWidgetConfiguration', configurationUrl);
                    }
                }).data("kendoWindow").center().open();
            }
        }
        else if($(e.target).hasClass('btnEditTitle')) {

            cleanFrameContent($('#iframeWidgetEditTitle'));
            $('#editTitleWindow').show();
            $("#editTitleWindow").kendoWindow({
                draggable: false,
                modal: true,
                iframe: true,
                title: portalBehaviourResources.EditTitle,
                actions: ["Close"],
                resizable: false,
                width: 640,
                activate: function () {
                    setConfigFrameSource('iframeWidgetEditTitle', editTitleUrl);
                }
            }).data("kendoWindow").center().open();
        }
        else if($(e.target).hasClass('btnRefresh')) {
            setWidgetFrameSource(currentFrameId, currentFrameUrl);
        }
    });
});

function displayMessage(evt) {
    if (typeof evt.data === "string" && evt.data.indexOf("Success") >= 0) {
        if ($("#configurationWindow").data('kendoWindow')) {
            $("#configurationWindow").data('kendoWindow').close();
            $('#configurationWindow').hide();
        }
        if ($("#configurationWindow1").data('kendoWindow')) {
            $("#configurationWindow1").data('kendoWindow').close();
            $('#configurationWindow1').hide();
        }

        if ($("#editTitleWindow").data('kendoWindow')) {
            $("#editTitleWindow").data('kendoWindow').close();
            $('#editTitleWindow').hide();
        }
        var successMessage = evt.data.split(" ");
        var length = successMessage.length;

        $("iframe").each(function () {
            var src = $(this).attr("src");
            if (src && src.indexOf(successMessage[length - 1]) >= 0) {
                var frameId = $(this).attr("id");
                var frameSrc = $(this).attr("src");
                if ((frameId.indexOf("Configuration") <= 0) || (frameId.indexOf("Settings") <= 0) || (frameId.indexOf("EditTitle") <= 0))
                {
                    setWidgetFrameSource(frameId, frameSrc);
                }
            }
        });
    }
}

if (window.addEventListener) {
    // For standards-compliant web browsers
    window.addEventListener("message", displayMessage, false);
} else {
    window.attachEvent("onmessage", displayMessage);
}

$(function () {
    homeUI.displayOptions(homeViewModel);
});