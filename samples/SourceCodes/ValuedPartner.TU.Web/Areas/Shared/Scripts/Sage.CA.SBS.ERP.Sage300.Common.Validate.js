/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

(function ($) {
    $.validator.addMethod("dateformat", function (value) {
        return sg.utls.kndoUI.checkForValidDate(value, false) != null;
    });

    $.validator.addMethod("dateformatallownull", function (value) {
        return sg.utls.kndoUI.checkForValidDateNull(value, false) != null;
    });
    $.validator.addMethod("fiscalperiodcheck", function (value, element, param) {
        if (isNaN(value) && param.module === "GL") {
            if (value.toUpperCase() === "ADJ" || value.toUpperCase() === "CLS") {
                return true;
            }
            return false;
        }
        if (value.length > 2) {
            return false;
        }
        return true;
    });

    $.validator.unobtrusive.adapters.add("dateformat", function (options) {
        options.rules["dateformat"] = "";
        options.messages["dateformat"] = options.message;
    });

    $.validator.unobtrusive.adapters.add("dateformatallownull", function (options) {
        options.rules["dateformatallownull"] = "";
        options.messages["dateformatallownull"] = options.message;
    });
    $.validator.unobtrusive.adapters.add("fiscalperiodcheck", ['module'], function (options) {
        options.rules["fiscalperiodcheck"] = options.params;
        options.messages["fiscalperiodcheck"] = options.message;

    });

}(jQuery));

