/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */
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