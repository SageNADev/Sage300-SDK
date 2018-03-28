/* Copyright (c) 2017 Sage Software, Inc.  All rights reserved. */

(function (f) {
    if (typeof define === 'function' && define.amd) {
        define(["kendo.core"], f);
    } else {
        f();
    }
}(function () {
    (function (window, undefined) {
        var stdNumberFormat = {
            pattern: ["-n"],
            decimals: 2,
            ",": ",",
            ".": ".",
            groupSize: [3],
            percent: {
                pattern: ["-n %", "n %"],
                decimals: 2,
                ",": ",",
                ".": ".",
                groupSize: [3],
                symbol: "%"
            },
            currency: {
                pattern: ["-$n", "$n"],
                decimals: 2,
                ",": ",",
                ".": ".",
                groupSize: [3],
                symbol: "$"
            }
        }
        var cultureArray = [
            kendo.cultures["en-AU"], kendo.cultures["en-CA"], kendo.cultures["en-US"], kendo.cultures["en-ZA"],
            kendo.cultures["es"], kendo.cultures["es-ES"], kendo.cultures["fr"], kendo.cultures["fr-CA"],
            kendo.cultures["zh-Hans"], kendo.cultures["zh-CN"], kendo.cultures["zh-SG"], kendo.cultures["zh-Hant"],
            kendo.cultures["zh-HK"], kendo.cultures["zh-TW"]
        ];
        $.each(cultureArray, function(key, value) {
            if (value) {
                //value.numberFormat = stdNumberFormat;
            }
        })
    })(this);
}));