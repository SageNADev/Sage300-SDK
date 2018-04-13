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