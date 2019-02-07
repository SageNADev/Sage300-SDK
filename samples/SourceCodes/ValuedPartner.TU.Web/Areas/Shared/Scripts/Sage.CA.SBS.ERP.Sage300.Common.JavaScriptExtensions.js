/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

var _DefaultDateConstructor = Date.prototype.constructor;
Date = function (a, b, c, d, e, f, g) {

    if (arguments.length === 0) {
        // your special behavior here
        return new _DefaultDateConstructor();
    }
    else if (arguments.length === 1) {
        if (typeof a === "string") {

            if (a === "0001/01/01") {
                return new _DefaultDateConstructor(a);
            }
            var parseDate1 = kendo.parseDate(a, sg.utls.kndoUI.getDatePatterns());


            if (parseDate1 == null) {
                parseDate1 = kendo.parseDate(a, ["yyyy/MM/d", "yyyy/MM/d h:mm:ss tt", "yyyyMMd", "yyyy/MM/d HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss"]);
            }

            //ignore culture and try again
            if (parseDate1 == null) {
                parseDate1 = new _DefaultDateConstructor(a);
            }

            return parseDate1;

        } else {
            // normal default processing of single constructor argument
            return new _DefaultDateConstructor(a);
        }
    }
    else {
        // normal default processing of single constructor argument
        a = a || 0;
        b = b || 0;
        c = c || 0;
        d = d || 0;
        e = e || 0;
        f = f || 0;
        g = g || 0;
        return new _DefaultDateConstructor(a, b, c, d, e, f, g);
    }

    return null;
}

Date.prototype = _DefaultDateConstructor.prototype;
Date.parse = _DefaultDateConstructor.parse;
Date.UTC = _DefaultDateConstructor.UTC;

 Date.prototype.toJSON = toJSON;
 function toJSON() {

     var ret = "";

     if (this != null && this != undefined) {
         var offset = this.getTimezoneOffset() * 60 * 1000; // find the offset to UMT in ms
         var tmpDate = new Date(this.getTime() - offset); // create a date that has the correct date component when converted to UMT
         return tmpDate.toISOString().slice(0, 10); // remove time component from the date which at this point should be "T00:00:00.000Z"
     }

     return ret;
 }