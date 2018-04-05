/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */
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
        scroll(0, 0);
        $("#firstTimeLogin").show();
        $('#screenLayout').hide();
        $('#breadcrumb').hide();
        $('#widgetLayout').show();
        $('html').addClass('home-page');
        $('#draggable').animate({
            top: 58
        }, 'slow', function () { $(this).removeAttr('style'); }); /* reset the position of #draggable-Quick Menu */
        $('#FeatureTour').show(function () {
            $(this).children().eq(0).addClass('show'); /* add class .show to the first child of #FeatureTour */
        });
        updateLayout(false);
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