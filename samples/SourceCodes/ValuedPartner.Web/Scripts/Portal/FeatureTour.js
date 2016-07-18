/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

$(document).ready(function () {
    getUserPreferenceCall();
});

/* First Time Login Experience */
function nextSlide(x, y) {
    $(".tourArrow").fadeOut('slow');
    $(".completeTour>div:nth-of-type(" + x + ")").animate({ "marginLeft": '-=250px' }, 500).fadeOut(200);
    $(".completeTour>div:nth-of-type(" + y + ")").delay(700).fadeIn(800).animate({ "margin-left": "-=250px" });
    $(".completeTour>div:nth-of-type(" + y + ") div.tourArrow").delay(1000).fadeIn(700);
    if (y === 1) {
        $(".completeTour>div").css("margin-left", "250px");
    }
    if (y === 2) {
        $(".completeTour>div").css("margin-left", "250px");
    }
    if (y === 3) {
        $(".completeTour>div").css("margin-left", "250px");
    }
    if (y === 4) {
        $(".completeTour>div").css("margin-left", "250px");
    }
}

$(function () {
    $(".completeTour > div:nth-of-type(1)").show();
    $(".tourClose,#doneTour").click(function () {
        $("#firstTimeLogin").hide();
        $("#firstTimeLogin > .completeTour > div").css("margin-left", "0").hide();
        $("#firstTimeLogin > .completeTour > div:first-child").show();
    });
});


/* Get/Set First time user Preference */
/*fetching first time user info from user perference table*/
function getUserPreferenceCall() {
    var result = featureTourElements.firstTimeUser;
    if (result === "True") {
        $("#firstTimeLogin").show();
    } else if (result === "False") {
        $("#firstTimeLogin").hide();
    }
}

/*fetching first time user info from user perference table*/
function setUserPreferenceCall() {
    var data = { UserId: "user" };
    sg.utls.ajaxPostHtml(sg.utls.url.buildUrl("Core", "Help", "DisableFirstTimeUser"), data, function (result) {

    });
}