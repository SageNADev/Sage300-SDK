
"use strict";

var ISV1CopyOrderCustomUI = ISV1CopyOrderCustomUI || {};
var CopyOrdercustomViewModel;

ISV1CopyOrderCustomUI = {

    // Init
    init: function () {
        ISV1CopyOrderCustomUI.initButtons();
    },

    // Init Buttons
    initButtons: function () {
        $("#btnCustomCreate1").bind('click', function () {
            sg.utls.showKendoConfirmationDialog(function () { }, null, "ISV1 Customization button click, add Javascript code to do real work.", "Demo");
        });
    },
};

var CopyOrderCustomUICallback = {
};

// Initial Entry
$(function () {
    ISV1CopyOrderCustomUI.init();
});
