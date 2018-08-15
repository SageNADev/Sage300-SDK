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

