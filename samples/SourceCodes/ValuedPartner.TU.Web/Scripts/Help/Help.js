/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */
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

    function ShowHomePage() {
        window.scrollTo(0, 0);
        $('html').addClass('home-page');
        $('#homeNav > a').addClass('active');
        $('#screenLayout').hide();
        $('#widgetlayout').show();
        $('#widgetHplayout').hide();

        //When footer logo is clicked
        taskDockMenuBreadCrumbManager.setDefaultScreenId();

        $('#breadcrumb').hide();

        updateLayout(true);

        if (!$('#screenLayout').is(":visible")) {

            if ($('#widgetlayout').is(":visible")) {
                $('#widgetHplayout').hide();
            }
        }
    }

    $("#featureTourLink").click(function () {
        scroll(0, 0);
        ShowHomePage();
        updateLayout(false);
        $("#firstTimeLogin").show();
        $('#draggable').animate({
            top: 58
        }, 'slow', function () { $(this).removeAttr('style'); }); /* reset the position of #draggable-Quick Menu */
        $('#topMenuTools').removeClass('inactive');
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