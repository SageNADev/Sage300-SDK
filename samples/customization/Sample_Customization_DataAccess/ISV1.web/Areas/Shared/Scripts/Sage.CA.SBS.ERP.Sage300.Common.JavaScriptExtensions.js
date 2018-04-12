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