// mob object that Global.js needs
globalResource = { Culture: "en-US", Name: "name", Type: "type", Show: "show" };

var sage = sage || {};

if (sage && sage.cache && sage.cache.isSessionStorageSupported) {
    sage.cache.isSessionStorageSupported = function () { return false; };
}

// override the default ajax call at the beginning of loading Global.js
$.ajax = function (options) {
    var data = { "Currency": { "Code": "CAD", "Decimals": 2, "DecimalSeparator": ".", "Description": "Canadian Dollars", "NegativeDisplay": 3, "Symbol": "$   ", "SymbolDisplay": 1, "ThousandSeparator": "," }, "IsPhoneNumberFormatRequired": true };
    options.success(data);
};
