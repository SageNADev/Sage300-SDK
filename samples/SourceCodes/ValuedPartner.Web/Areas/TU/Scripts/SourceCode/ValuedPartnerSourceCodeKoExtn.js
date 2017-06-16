// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
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

function SourceCodeObservableExtension(sourceCodeModel, uiMode) {
    var model = sourceCodeModel;
    model.UIMode = ko.observable(uiMode);

    // Computed Fields go here

    model.SourceCode = ko.computed({
        read: function() {
            var sourceCodeVal = "";
            if (model.SourceLedger() != undefined) {
                sourceCodeVal = model.SourceLedger();

                if (model.SourceType() != undefined) {
                    sourceCodeVal += '-' + model.SourceType();
                }
            }
            return sourceCodeVal;

        },
        write: function (value) {
            if (value != undefined) {
                var scData = sourceCodeObject.getSourceCodeElements(value);
                model.SourceLedger(scData.SourceLedger);
                model.SourceType(scData.SourceType);
            }
        }
    });

    model.DisableAddSave = ko.computed(function () {
        if (sg.controls.GetString(model.SourceCode()) === "" || model.SourceCode() === "-") {
            return true;
        }
        return false;
    });

    model.DisableDelete = ko.computed(
        function() {
            if (sg.controls.GetString(model.SourceCode()) === "" || model.SourceCode() === "-" || model.UIMode() === sg.utls.OperationMode.NEW) {
                return true;
            }
            return false;
        });

};