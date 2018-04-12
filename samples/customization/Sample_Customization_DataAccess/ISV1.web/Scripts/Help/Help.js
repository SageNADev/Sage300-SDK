// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

"use strict";

var navUrl;
$(document).ready(function () {
    $("#helpSearch").validate({
        onkeyup: false,
    });
    $('.k-animation-container').focusout(function () { $("#helpSearch").valid(); });
    $('#helpSearchbtn').click(function (event) {
        navUrl = helpUrl;
        var searchValue = $("#helpSearchfl").val().toString();
        $("#helpSearchfl").val('');
        var url = navUrl.replace("#search-", "#search-" + searchValue);
        event.preventDefault();
        openInNewTab(url);
    });

    function openInNewTab(url) {
        var win = window.open(url, '_blank');
        win.focus();
    };

    $("#featureTourLink").click(function () {
        $("#firstTimeLogin").show();
        $('#screenLayout').hide();
        $('#widgetHplayout').hide();
        $('#dvAddWidget').show();
        $('#breadcrumb').hide();
        $('#widgetLayout').show();
        $('#draggable').animate({
            top: 30
        }, 'slow', function () { $(this).removeAttr('style'); }); /* reset the position of #draggable-Quick Menu */
        $('#FeatureTour').show(function () {
            $(this).children().eq(0).addClass('show'); /* add class .show to the first child of #FeatureTour */
        });
    });

    $('#FeatureTour').find('.btn-tertiary').click(function () {
        if ($(this).closest('.ft-step').removeClass('show').next().length > 0) { /* remove class .show from .ft-step */
            $(this).closest('.ft-step').next().addClass('show'); /* add class .show to the next .ft-step */
        }
        else {
            $(this).closest('#firstTimeLogin').hide();
        }
    });

    $('#FeatureTour .step-indicator-group').find('li').click(function () {
        var i = 0;
        i = $(this).index();

        $('#FeatureTour').find('.ft-step').removeClass('show').eq(i).addClass('show'); /* steps indicator: add and remove class .show from .ft-step */
    });

    $("#FeatureTour .msgCtrl-close").click(function () {
        $('#firstTimeLogin').find('.ft-step').removeClass('show').end().hide(); /* close button */
    });
});