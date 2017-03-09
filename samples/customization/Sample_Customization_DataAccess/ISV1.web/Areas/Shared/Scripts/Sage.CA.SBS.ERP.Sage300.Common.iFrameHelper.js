// Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved.

"use strict";

sg.utls.iFrameHelper = sg.utls.iFrameHelper || {};

$.extend(sg.utls.iFrameHelper = {

    DefaultHeight: 720,
    DefaultWidth: 1100,

    postDataToParent: function (data) {
        var sourceFrameId = window.frameElement.getAttribute('data-parentiFrameId');
        var childUrl = $(location).attr('href');

        if (sourceFrameId) {
            var isContentWindow = sg.utls.isChrome() || sg.utls.isMozillaFirefox() || sg.utls.isSafari();
            var iframeWin = window.top.frames[sourceFrameId];
            if (iframeWin && isContentWindow) {
                iframeWin = iframeWin.contentWindow;
            }
            if (iframeWin) {
                iframeWin.postMessage(data, childUrl);
            } else {
                //get second level frame window
                var contentFrame = sg.utls.iFrameHelper.getContentFrame();
                isContentWindow = sg.utls.isChrome() || sg.utls.isSafari();
                iframeWin = contentFrame.frames[sourceFrameId];
                if (iframeWin && isContentWindow) {
                    iframeWin = iframeWin.contentWindow;
                }
                if (iframeWin) {
                    iframeWin.postMessage(data, childUrl);
                }
            }
        } else {
            // non portal scenario this will be used.
            window.parent.postMessage(data, childUrl)
        }
    },

    // build data- standard format to communicate the iframe messages..
    buildData: function (id, data) {
        // introduced type because, there are other messages are also posted beteween iframes, to filter custom messages this type is required. 
        var resultData = { Type: 'SageKendoiFrame', Id: id, Data: data };
        return resultData;
    },

    closeWindow: function () {
        var id = window.frameElement.id;
        var contentFrame = sg.utls.iFrameHelper.getContentFrame();
        var divCtrl = contentFrame.$("#div" + id);
        divCtrl.data("kendoWindow").trigger('close');//trigger close event
    },

    isWindowiFramePopup: function () {
        var id = window.top.$('iframe.screenIframe:visible').contents().find('.k-widget.k-window').find('iframe').attr('data-parentiframeid');
        if (id) {
            return true;
        }
        return false;
    },


    getContentFrame: function () {
        var contentFrame;
        var screeniFrame = window.top.$('iframe.screenIframe:visible');
        if (screeniFrame.length > 0) {
            contentFrame = screeniFrame[0].contentWindow;
        } else {
            contentFrame = window.top;  // if portal is not enabled then this will be used.
        }
        return contentFrame;
    },

    // register to receive message.
    registerToReceiveMessage: function (msgCallBackFunc) {
        // this should be javascript only, otherwise the values are not returned properly.
        if (window.addEventListener) {
            window.addEventListener("message", msgCallBackFunc,false);
        }
        else {
            window.attachEvent("onmessage", msgCallBackFunc, false);
        }
    },


    //Below methods are called from parent

    getBeforeUnloadEvent: function (frameWindow) {
        //return frameWindow.beforeClose_iFramePopup;
        if (frameWindow.$._data(frameWindow, 'events') != null && frameWindow.$._data(frameWindow, 'events')["beforeunload"] != null) {
            return frameWindow.$._data(frameWindow, 'events')["beforeunload"].map(function (elem) { return elem.handler; })[0];
        }
        return null;
    },

    getUnloadEvents: function (frameWindow) {
        if (frameWindow.$._data(frameWindow, 'events') != null && frameWindow.$._data(frameWindow, 'events')["unload"] != null) {
            return frameWindow.$._data(frameWindow, 'events')["unload"].map(function (elem) { return elem.handler; });
        }
        return null;
    },

    // id is divId - before opening the window, it will create a div and create a iframe window.
    openWindow: function (id, title, url, height, width, parentMsgCallBackFunc, source) {
        var htmlDiv = '<div id=div' + id + '/>';

        var contentFrame;
        var form;
        var visbleFrameContent;
        var divCtrl;
        if (source == null) {
            contentFrame = sg.utls.iFrameHelper.getContentFrame();
            form = contentFrame.$('form');

            // remove the existing div.
            visbleFrameContent = window.top.$('iframe.screenIframe:visible').contents().find('.k-widget.k-window');
            visbleFrameContent.contents().remove("#div" + id);

            // append the div
            form.append(htmlDiv);

            divCtrl = contentFrame.$("#div" + id);
        } else {
            contentFrame = source.closest('html').parent();
            form = source.closest('html').parent().find('form');

            // remove the existing div.
            visbleFrameContent = source.closest('html').parent().find('.k-widget.k-window');
            visbleFrameContent.remove("#div" + id);

            // append the div
            form.append(htmlDiv);

            divCtrl = contentFrame.find("#div" + id);
        }

        // set default height and width, if parameter is null
        if (height == null || height > sg.utls.iFrameHelper.DefaultHeight) {
            height = sg.utls.iFrameHelper.DefaultHeight;
        };
        if (width == null || width > sg.utls.iFrameHelper.DefaultWidth) {
            width = sg.utls.iFrameHelper.DefaultWidth;

            if (source == null) {
                if (width > $(contentFrame.document).width()) {
                    width = $(contentFrame.document).width() - 20;
                }
            } else {
                if (width > $(source.closest('html')).width()) {
                    width = $(source.closest('html')).width() - 20;
                }
            }
        };

        var portalHeight = 0;

        // if portal is enabled then set the portal height.
        if (sg.utls.isPortalIntegrated()) {
            portalHeight = sg.utls.portalHeight;
        }

        // get the height of the window and reduce the top & bottom size.
        var maxHeight;
        if (source == null) {
            maxHeight = $(contentFrame.document).height() - portalHeight; // reducing height based on portal.
        } else {
            maxHeight = $(source.closest('html')).height() - portalHeight; // reducing height based on portal.
        }

        divCtrl.kendoWindow({
            modal: true,
            title: title,
            resizable: false,
            draggable: true,
            scrollable: true,
            visible: false,
            iframe: true,
            content: sg.utls.urlEncode(url),
            minWidth: 800,
            minHeight: 300,
            maxHeight: maxHeight,
            height: height,
            width: width,
            activate: sg.utls.kndoUI.onActivate,
            close: function (e) {

                var frameWindow;

                if (sg.utls.isChrome() || sg.utls.isSafari()) {
                    frameWindow = sg.utls.iFrameHelper.getContentFrame().frames[id].contentWindow;
                } else {
                    frameWindow = sg.utls.iFrameHelper.getContentFrame().frames[id];
                }

                if (frameWindow) {
                    //Call iframe beforeunload event if it exists.
                    //This event is registered with Child window.
                    var beforeIframeCloseEvent = sg.utls.iFrameHelper.getBeforeUnloadEvent(frameWindow);
                    if (beforeIframeCloseEvent != null && typeof beforeIframeCloseEvent !== 'undefined' && $.isFunction(beforeIframeCloseEvent)) {
                        if (sg.utls.iFrameHelper.beforeCloseEvent(frameWindow, divCtrl)) {
                            e.preventDefault();
                            return;
                        }
                    }

                    // On Kendo window (X) close button click, to post data back to parent page, below function is used.
                    // on all child popup screen where you want to return value to parent page should have function - close_iFramePopup
                    // This event is registered with Child window
                    var closeiframePopup = frameWindow.close_iFramePopup;
                    if (typeof closeiframePopup !== 'undefined' && $.isFunction(closeiframePopup)) {
                        closeiframePopup();
                    }
                }

                //Call parent callback function
                if (parentMsgCallBackFunc != null && typeof parentMsgCallBackFunc !== 'undefined' && $.isFunction(parentMsgCallBackFunc)) {
                    parentMsgCallBackFunc();
                }

                //This is not required. Destroy calls the page unload event.
                /*if (frameWindow) {
                    //Call iframe unload event if it exists.
                    //This event is registered with Child window.
                    var unloadEvents = sg.utls.iFrameHelper.getUnloadEvents(frameWindow);
                    if (unloadEvents != null) {
                        $.each(unloadEvents, function (index, evt) {
                            //console.log("event: " + evt);
                            evt();
                        });
                    }
                }*/

                // destroy the window on close.
                divCtrl.data("kendoWindow").destroy();
            },
            refresh: function () {
                // refresh function will get called after the page load is complete, we get height after the page is loaded.
                //var contentFrame = sg.utls.iFrameHelper.getContentFrame();

                var iframeContent = window.top.$('iframe.screenIframe:visible').contents().find('#' + id);
                var contentHeight = iframeContent.contents().find('body').height();

                if (contentHeight == null) {
                    iframeContent = $('#' + id);
                    contentHeight = iframeContent.contents().height();
                }

                // based on the height setting the scroll (by default it will be enabled, disabed based on the page height).
                if (contentHeight < maxHeight) {
                    iframeContent.contents().find('html').css('overflow-y', 'auto');
                };
            },

            //Open Kendo Window
            open: function () {

                this.element.addClass("popup-iframe");
                this.element.closest(".popup-iframe").css({
                    height: height - 50, // to adjust the height.
                });

                var leftPos = ($(window.top).innerWidth() - this.wrapper.width()) / 2;
                if (leftPos < 0) {
                    leftPos = 25;
                }

                var scrollPos = $(window.top).scrollTop();
                if (scrollPos > 150) {
                    scrollPos = scrollPos - 100;
                }
                this.wrapper.css({
                    top: scrollPos,
                    left: leftPos
                });
            }
        }).data("kendoWindow").open();

        // reset the name.
        var iframeElement = divCtrl.find('.k-content-frame');

        // set the iframe name.
        iframeElement.attr('name', id);
        iframeElement.attr('id', id);

        iframeElement.attr('data-callBeforeUnload', "1");

        if (window.frameElement) {
            var parentiFrameId = window.frameElement.id;

            // add parent iframeid attribute.
            if (parentiFrameId) {
                iframeElement.attr('data-parentiFrameId', parentiFrameId);
            }
        }
    },

    beforeCloseEvent: function (frameWindow, divCtrl) {
        var iframeElement = divCtrl.find('.k-content-frame');
        if (frameWindow) {
            var callBeforeUnload = iframeElement.attr('data-callBeforeUnload');
            if (callBeforeUnload !== "1") {
                return false;
            }

            //beforeunload event
            var closeEvent = sg.utls.iFrameHelper.getBeforeUnloadEvent(frameWindow);
            if (closeEvent != null && typeof closeEvent !== 'undefined' && $.isFunction(closeEvent)) {
                var message = closeEvent();

                if (message == null || message === "") {
                    return false;
                }

                frameWindow.sg.utls.showKendoConfirmationDialog(function () { // Yes
                    iframeElement.attr('data-callBeforeUnload', '0');
                    divCtrl.data("kendoWindow").trigger('close');
                }, $.noop, message);

                return true;
            }
            return false;
        }
    },

});