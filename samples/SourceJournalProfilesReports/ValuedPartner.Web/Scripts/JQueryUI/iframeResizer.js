/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

$(window).bind("load", function () {
    var iFrameHeight = window.top.$('iframe.screenIframe:visible');
    var th = iFrameHeight.contents().find('body').height();
    if (iFrameHeight.contents().find('#CrystalReportViewerSage300').length) {
        th = th + 10;
    }
    iFrameHeight.css('height', th);

    // get the observer that work with all browsers
    var MutationObserver = window.MutationObserver || window.WebKitMutationObserver || window.MozMutationObserver;

    // create observer to adjust height
    var observer = new MutationObserver(function (mutations) {
        var visibleIframe = window.top.$('iframe.screenIframe:visible');
        var padding = 0;
        // put back old code logic
        if (visibleIframe.contents().find('#CrystalReportViewerSage300').length) {
            padding = 10;
        }

        if (typeof sg === "undefined" || !sg.utls.isKendoIframe()) {
            visibleIframe.css('height', $('body').height() + padding);
        }
    });

    // put to observe only if it is inside one of the iframe, that's because empty iframe has no content to observe
    if (window.self !== window.top) {
        observer.observe(document.body, {
            childList: true,
            subtree: true,
            attributes: true
        });
    }
});

