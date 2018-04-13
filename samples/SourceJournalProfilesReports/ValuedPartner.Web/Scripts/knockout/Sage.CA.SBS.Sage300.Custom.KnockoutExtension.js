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
var dirtyFlags = {
    isGridDirty: ko.observable(false)
};
ko.dirtyFlag = function (root, ignoreProperties) {
    var isIgnoredProperty = function (key, value) {
        var flag = false;

        if (ignoreProperties == null) {
            return value;
        }
        $.each(ignoreProperties, function (ignoreKey, ignoreValue) {
            if (key == ignoreValue) {
                flag = true;
                return;
            }
        });

        if (flag)
            return null;
        else
            return value;
    };

    var result = function () { },
    initialState = ko.observable(ko.toJSON(root, isIgnoredProperty)),
    isDirty = ko.observable(false);

    result.isDataDirty = ko.computed({
        read: function () {
            if (isDirty()) {
                return true;
            }

            //compare
            var currentState = ko.toJSON(root, isIgnoredProperty);
            
            if (initialState() !== currentState) {
                //console.log("initialState: " + initialState());
                //console.log("currentState: " + currentState);
                isDirty(true);
                //console.log("Is Dirty: " + "true");
                return true;
            }

            return false;
        }
    });

    result.isGridDataDirty = ko.computed({
        read: function () { return dirtyFlags.isGridDirty(); }
    });

    result.isDirty = ko.computed({
        read: function () {
            if (isDirty() || dirtyFlags.isGridDirty()) {
                //console.log("Is Dirty: " + "true");
                return true;
            }
            //compare
            var currentState = ko.toJSON(root, isIgnoredProperty);


            if (initialState() !== currentState) {
                //console.log("initialState: " + initialState());
                //console.log("currentState: " + currentState);
                isDirty(true);
                //console.log("Is Dirty: " + "true");
                return true;
            }

            return false;
        },
        write: function (value) {
            isDirty(value);
        },
        owner: this
    });

    result.reset = function () {
        //console.log("reset Is Dirty");
        initialState(ko.toJSON(root, isIgnoredProperty));
        isDirty(false);
        dirtyFlags.isGridDirty(false);
    };

    return result;
};
//This is added to extend the date property of datetime picker to convert UTC date format.
ko.extenders.date = function (target) {
    return ko.computed({
        read: function () {
            var value = target();
            var date = new Date(value);
            if (date == "Invalid Date") {
                return target(value);
            }
            return target(sg.utls.kndoUI.getDate(value));
        },
        write: function () {
            return target;
        }
    });
}
//SubscribeChanged allows to get the new and previous values from an observable
ko.subscribable.fn.subscribeChanged = function (callback) {
    var oldValue;
    this.subscribe(function (_oldValue) {
        oldValue = _oldValue;
    }, this, 'beforeChange');

    var subscription = this.subscribe(function (newValue) {
        callback(newValue, oldValue);
    });

    return subscription;
};