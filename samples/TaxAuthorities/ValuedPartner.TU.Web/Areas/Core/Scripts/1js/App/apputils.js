(function () {
    'use strict';
    if (this.apputils) {
        return;
    }

    var apputils = this.apputils = this.apputils || {};

    this.apputils.noPromiseOk = { then: (cb) => cb({ noQuery: true }, null, { hasError: false }) }
    this.apputils.nullable = "null";

    this.apputils.MessageIdColumn = { field: "msgid", title: "msg id", dataType: "string", hidden: false, value: undefined };

    this.apputils.createRootNode = function (typename = "", nodeId = "") {
        return `<n t='${typename}' n='${nodeId}'>`;

    };

    this.apputils.rootNodeTemplate = this.apputils.createRootNode();

    this.apputils.rootNodeClose = "</n>";
    Object.freeze(this.apputils.rootNodeClose);

    this.apputils.createRowNode = function ({ viewId, filter = "", parentId = "", id = "0", verb = apputils.CRUDReasons.Get, gp = "-1", ps = '-1' }) {
        filter = filter.length > 0 ? filter : "f=''";
        filter = filter.includes("f=") ? filter : `f='${filter}'`;

        return `<r i='${viewId}' ${filter} p='${parentId}' id='${id}' verb='${verb}' gp='${gp}' ps='${ps}'>`;

    };

    this.apputils.rowNodeClose = "</r>";
    Object.freeze(this.apputils.rowNodeClose);

    this.apputils.createFieldNode = function ({ fieldId, value, verify=false, parentId = "0" }) {
        return `<c f='${fieldId}' v='${value}' p='${parentId}' vfy='${verify}'/>`;

    };

    this.apputils.EventMsgTags = {
        svrUpdate: "svrUpdate",
        usrUpdate: "usrUpdate",
        svrValid: "svrValid",
        svrInvalid: "svrInvalid",
        userctx: "userctx",
        onBeforeStartEdit: "onBeforeStartEdit",
        finder: "Finder"
    };
    Object.freeze(this.apputils.EventMsgTags);

    this.apputils.NavigationAction = {
        None: 0,
        First: 1,
        Previous: 2,
        Next: 3,
        Last: 4
    };
    Object.freeze(this.apputils.NavigationAction);

    this.apputils.fieldAttributes = {
        FLD_KEY: "6"
    },
    
    this.apputils.pStatus = {
        STATUS_UNKNOWN: "STATUS_UNKNOWN",
        STATUS_OK: "STATUS_OK",
        STATUS_CANCEL: "STATUS_CANCEL",
        BUTTON_STATUS_CANCEL_WITH_TABAWAY: "BUTTON_STATUS_CANCEL_WITH_TABAWAY",
        BUTTON_STATUS_CANCEL: "BUTTON_STATUS_CANCEL",
        BUTTON_STATUS_OK: "BUTTON_STATUS_OK"
    },
    
    this.apputils.eReason = {
        RSN_FIELDCHANGE: "RSN_FIELDCHANGE",
        RSN_DELETE: "RSN_DELETE",
        RSN_BLKPUT: "RSN_BLKPUT",

    },

    this.apputils.CRUDReasons = {
        InitDataOnly: "InitOnly",
        InitData: "Init",
        InitHeader: "InitHeader",
        AddingNewData: "Insert",
        PostData: "Post",
        Ignore: "Ignore",

        ExistingData: "Put", //retrieve => update
        Deleting : "Delete",
        Process: "Process",
        GetTemplate: "GetTemplate",
        Get: "Get",
        GotoFirstRecord: "GoFirst",
        GotoPreviousRecord: "GoPrev",
        GotoNextRecord: "GoNext",
        GotoLastRecord: "GoLast",
        ProcessGet: "ProcessGet",
        ProcessPut: "ProcessPut",
        Verify: "Verify",
        Exists: "Exists"
    };
    Object.freeze(this.apputils.CRUDReasons);

    this.apputils.Operators = {
        StartsWith: "startswith",
        EndsWith: "endswith",
        Contains : "contains",
        Equals: "=",
        NotEquals: "!=",
        LessThan: "&lt;", //"<",
        MoreThan: "&gt;", //">",
        Like: "like",
        And: "and",
        Or: "or",
        LessThanOrEqualTo: "<&lt;",
        MoreThanOrEqualTo: "&gt;>"
    };
    Object.freeze(this.apputils.Operators);

    this.apputils.HotKeys = {
        F9: 120,
        Insert: 45,
        Delete: 46,
        AltC: 67,
        Home: 36,
        PgUp: 38,
        PgDn: 40,
        End: 35
    };
    Object.freeze(this.apputils.HotKeys);

    this.apputils.DataType = {
        Decimal: "Decimal",
        Long: "Long",
        Char : "Char",
        Int: "Int",
        Date: "Date",
        Bool: "Bool",
        Amount: "Amount",
        Money: "Money"
    };
    Object.freeze(this.apputils.DataType);

    this.apputils.numericType = [
        apputils.DataType.Decimal, apputils.DataType.Decimal.toLowerCase(),
        apputils.DataType.Int, apputils.DataType.Int.toLowerCase(),
        apputils.DataType.Long, apputils.DataType.Long.toLowerCase(),
        apputils.DataType.Amount, apputils.DataType.Amount.toLowerCase(),
        apputils.DataType.Money, apputils.DataType.Money.toLowerCase()
    ];
    Object.freeze(this.apputils.numericType);

    //events can be triggered by user eg: tabbing out of input box or by system eg: Finder
    this.apputils.EventTrigger = {
        System: "system",
        User: "user"
    };
    Object.freeze(this.apputils.Navigation);

    //credit https://stackoverflow.com/questions/27082377/get-number-of-decimal-places-with-javascript
    this.apputils.simpleFormatterForNow = function (defaultValue, value) {

        let precision = apputils.countDecimals(defaultValue);

        return apputils.formatNumber(value, precision);
    };

    this.apputils.countDecimals = function (value) {
        value = apputils.isNumber(value) ? value + "" : value;
        let text = value.toString();
        // verify if number 0.000005 is represented as "5e-6"
        if (text.indexOf('e-') > -1) {
            let [base, trail] = text.split('e-');
            let deg = parseInt(trail, 10);
            return deg;
        }

        // count decimals for number in representation like "0.123456"
        const decimalSeparator = value && value.toString().includes('.') ? '.' : kendo.culture().numberFormat['.'];
        if (Math.floor(value) !== value && value.toString().split(decimalSeparator)[1]) {
            return value.toString().split(decimalSeparator)[1].length || 0;
        }
        return 0;
    };

    this.apputils.formatForMultiCurrencyCompany = function (value) {
        if (apputils.isUndefined(apputils.companyProfile) || apputils.isUndefined(apputils.companyProfile.isMultiCurrencyCompany())) {
            return apputils.formatNumber(value, 2);
        }

        let precision = apputils.companyProfile.isMultiCurrencyCompany() ? 3 : 2;
        return apputils.formatNumber(value, precision);
    };

    this.apputils.formatUsingFunctionalCurrency = function (value) {

        if (apputils.isUndefined(apputils.companyProfile)) {
            return value;
        }

        let homeCurrency = apputils.companyProfile.getHomeCurrency();

        return apputils.formatUsingCurrencyCode(homeCurrency, value);
    };

    this.apputils.decimalPlaceOfFunctionalCurrency = function () {
        
        if (apputils.isUndefined(apputils.companyProfile) || apputils.isUndefined(apputils.currencyCodes)) {
            return 2;
        }

        let homeCurrency = apputils.companyProfile.getHomeCurrency();

        return apputils.currencyCodes.getDecimalPlace(homeCurrency);
    };

    this.apputils.formatUsingCurrencyCode = function (currencyCode, value) {

        if (apputils.isUndefined(apputils.currencyCodes) || apputils.isUndefined(currencyCode) || currencyCode === "") {
            return value;
        }

        let precision = apputils.currencyCodes.getDecimalPlace(currencyCode);

        return apputils.formatNumber(value, precision);
    };

    this.apputils.formatQuantityUsingCurrencyCode = function (currencyCode, defaultValue, value) {
        switch (currencyCode) {
            case ("JPN"):
                return apputils.formatUsingCurrencyCode(currencyCode, value);
            default:
                return apputils.simpleFormatterForNow(defaultValue, value);
        }
    };

    this.apputils.formatUnwantedChars = function (unformattedValue) {
        return unformattedValue.replace(/\W+/g, "");
    };

    this.apputils.formatUsingHomeCurrencyAndICOptionsFractional = function (value) {
        if (apputils.icOptions) {
            let decimal = apputils.icOptions.isFractionalQuantityAllowed() ? 4 : 0;
            return apputils.formatNumberForQuantity(value, decimal);
        }

        if (!apputils.companyProfile) return value;

        let currencyCode = apputils.companyProfile.getHomeCurrency();

        return apputils.formatUsingCurrencyCode(currencyCode, value);
    };

    this.apputils.formatUsingCurrencyRateTableCode = function (currencyCode, value) {
        if (apputils.isUndefined(apputils.currencyTable)) {
            return value;
        }

        let precision = apputils.currencyTable.getDecimalPlace(currencyCode, rateType);

        return apputils.formatNumber(value, precision);
    };

    this.apputils.formatQuantity = function (currencyCode, defaultValue, value) {
        //if IC not available, format based on PO
        if (apputils.isUndefined(apputils.icOptions)) {
            return apputils.formatQuantityBasedOnPOOptions(currencyCode, defaultValue, value);
        }
        let decimal = apputils.icOptions.isFractionalQuantityAllowed() ? 4 : 0;
        return apputils.formatNumberForQuantity(value, decimal);
    };

    //get currency here not when calling the func
    this.apputils.getCurrencyAndFormatQuantity = function (defaultValue, value) {
        if (apputils.isUndefined(apputils.companyProfile)) {
            return value;
        }
        let currencyCode = apputils.companyProfile.getHomeCurrency();

        return apputils.formatQuantity(currencyCode, defaultValue, value);
    };

    this.apputils.formatQuantityBasedOnPOOptions = function (currencyCode, defaultValue, value) {
        //if PO not available, format based on Currency code
        if (apputils.isUndefined(apputils.poOptions)) {
            return apputils.formatQuantityUsingCurrencyCode(currencyCode, defaultValue, value);
        }
        let decimal = apputils.poOptions.isFractionalQuantityAllowed() ? 4 : 0;
        return apputils.formatNumberForQuantity(value, decimal);
    };

    //Provides VB style number Format function
    //see https://docs.telerik.com/kendo-ui/globalization/intl/numberformatting
    this.apputils.customNumberFormats = function (value, formatSpecifier) {
        return kendo.toString(+value, formatSpecifier, kendo.culture().name);
    };

    this.apputils.formatNumber = function (value, precision) {
        //Some value always use '.' as decimal seperator, it will cause issue in localization.
        const decimalSeparator = kendo.culture().numberFormat['.'];
        value = value ? value.toString() : "0";

        if (value && value.includes('.')) {
            value = value.replace('.', decimalSeparator);
        }

        let number = kendo.parseFloat(value);
        //do this to handle rounding up of -239.495 to -239.49
        if (number < 0) {
            let negative = kendo.parseFloat(kendo.toString(0 - number, `n${precision}`));
            return kendo.toString(0 - negative, `n${precision}`);
        }
        return kendo.toString(number, `n${precision}`);
    };

    this.apputils.formatNumberForQuantity = function (value, precision) {
        const decimalSeparator = kendo.culture().numberFormat['.'];
        value = value ? value.toString() : "0";

        if (value && value.includes('.')) {
            value = value.replace('.', decimalSeparator);
        }
        let number = kendo.parseFloat(value);
        //if user entered decimals when no decimal was allowed...
        /*let userDecimals = apputils.countDecimals(number);
        if (+precision === 0) {
            let result = Math.pow(10, userDecimals);
            return kendo.toString((value * result), "n0");
        }*/
        return kendo.toString(number, `n${precision}`);
    };

    //Some numeric textbox need min value settings, like QUANTITY field. Numeric default value should be minVal or 0, can't be empty(null)
    this.apputils.initNumericTextBoxAndSetValue = function (id, val, minVal, decimal, numberOfNumerals = 0, round = true) {
        let precision = apputils.isUndefined(decimal) ? apputils.decimalPlaceOfFunctionalCurrency(): decimal;

        let numericTxtBox = $("#" + id).kendoNumericTextBox({
            decimals: precision,
            format: "n" + precision,
            round: round,
            spinners: false,
            min: minVal,
            value: val
        });
        if (numberOfNumerals === 0) {
            numberOfNumerals = precision ? 16 - precision : 14;
        }
        sg.utls.kndoUI.restrictDecimals(numericTxtBox, precision, numberOfNumerals);
        //sg.utls.kndoUI.restrictDecimals(numericTxtBox, precision, precision ? 16 - precision : 14);
    };

    this.apputils.formatKendoNumericTextBoxForDecimals = function (ctrId, data, minVal, decimal, numberOfNumerals = 0, round = true) {
        let kendoBox = $("#" + ctrId).data("kendoNumericTextBox");

        if (kendoBox && kendoBox.value) {
            let precision = apputils.isUndefined(decimal) ? apputils.decimalPlaceOfFunctionalCurrency(): decimal;
            kendoBox.setOptions({
                value: data,
                decimals: precision,
                format: "n" + precision,
                round: round,
                min: minVal
            });
            if (numberOfNumerals === 0) {
                numberOfNumerals = precision ? 16 - precision : 14;
            }
            sg.utls.kndoUI.restrictDecimals(kendoBox, precision, numberOfNumerals);
            //sg.utls.kndoUI.restrictDecimals(kendoBox, precision, precision ? 16 - precision : 14);
        } else {
            apputils.initNumericTextBoxAndSetValue(ctrId, data, minVal, decimal, numberOfNumerals, round);
        }
    };

    this.apputils.InitNoNullItemsArray = function () {
        let arrObj = [];
        arrObj.addItems = function (value) {
            if (value) { this.push(value); }
        };

        return arrObj;
    };

    this.apputils.initObjectForDisplay = function (obj) {
        return apputils.cloneDeep(obj);
    };

    this.apputils.getLockedPeriodLevel = function () {
        return apputils.companyProfile.getLockedPeriodLevel();
    };

    this.apputils.getWarningDays = function () {
        return apputils.companyProfile.getWarningDays();
    };

    this.apputils.getPeriodNumber = function () {
        return apputils.companyProfile.getPeriodNumber();
    };

    this.apputils.getSessionDate = function () {
        let sessionDate = new Date();
        let strDate = $.cookie(sg.utls.SessionCookieName);
        if (strDate) {
            sessionDate = new Date(strDate.split(' ')[0]);
        }
        return sessionDate;
    };

    this.apputils.formatDateyyyymmdd = function (value) {
        if (value.length === 0) {
            return value;
        }

        const dt = new Date(value);
        let mm = dt.getMonth() + 1; // getMonth() is zero-based
        let dd = dt.getDate();

        return [dt.getFullYear(),
        (mm > 9 ? '' : '0') + mm,
        (dd > 9 ? '' : '0') + dd
        ].join('');

    };

    this.apputils.formatTime = function (value) {
        let timeParts = value.split(' ');
        let ts = timeParts[1];

        if (value === '' || value === '0') {
            return '00:00:00';
        }

        if (timeParts.length > 2) {
            if (ts === '12:00:00' && timeParts[2].endsWith('AM') ) {
                return '00:00:00';
            }

            if (timeParts[2].endsWith('PM')) {
                let s = ts.split(':');
                let hh = parseInt(s[0]);
                if (hh < 12) {
                    let h = (hh + 12).toString();
                    ts = `${h}:${s[1]}:${s[2]}`;
                }
            }
        }

        if (!ts) {
            return timeParts[0];
        }

        if (ts.length < 8) {
            ts = `0${ts}`;
        }

        return ts;
    };

    this.apputils.displayMessage = async function (message, isWarning = false) {
        let messages = [{ Message: message, Priority: 3, PriorityString: 'Error', Tag: null }];

        let errorMsg = {
            "UserMessage": {
                "IsEmail": false, "IsSuccess": false, "Errors": isWarning ? [] : messages, "Warnings": isWarning ? messages : [], "Info": null
            }
        }

        return await new Promise((resolve) => {
            sg.utls.showMessage(errorMsg, () => {

                resolve(true);
            });
        });
    },

    this.apputils.stdErrorHandler = async function (isWarning = false) {
        let messages = ErrorEntityCollectionObj.getErrors();
        let errorMsg = {
            "UserMessage": {
                "IsEmail": false, "IsSuccess": false, "Errors": isWarning ? [] : messages, "Warnings": isWarning ? messages : [], "Info": null
            }
        }

        return await new Promise((resolve) => {
            sg.utls.showMessage(errorMsg, () => {

                ErrorEntityCollectionObj.clearError();
                
                resolve(true);
            });
        });
    };

    this.apputils.stdConfirmationHandler = async function (message) {

        return await new Promise((resolve) => {

            sg.utls.showKendoConfirmationDialog(
                function () { // Yes
                    
                    resolve(true);
                },
                function () { // No
                    resolve(false);
                },
                message);
        });
    };

    this.apputils.activeElementId = "";

    this.apputils.setFinderActiveElementId = function (btnId, txtBoxId) {
        $('#' + btnId).mousedown(() => {
            apputils.activeElementId = txtBoxId;
            
        });
    };

    this.apputils.getFinderActiveElementId = function (btnId, txtBoxId) {
        if (btnId) {
            return apputils.activeElementId === txtBoxId ? apputils.activeElementId : "";
        }

        return apputils.activeElementId;
    };

    /**
     * Inversion of Control (IoC) to abstract external utility functions that should be fully managed by this apputils js
     **/
    const lodashMethods = ['isUndefined', 'each', 'clone', 'cloneDeep', 'uniqueId', 'find', 'isFunction', 'escape', 'isDate', 'extend', 'isNumber', 'functions', 'isNull', 'filter', 'where', 'unescape', 'keys', 'has', 'isEqual', 'once', 'compose', 'isObject', 'isEmpty', 'sortBy'];
    lodashMethods.forEach(method => {
        this.apputils[method] = function () {
            return _[method].apply(this, arguments);
        }
    });

    this.apputils.compose = function () {
        return _.flowRight.apply(this, arguments);
    };

    this.apputils.isDefined = function (value) {
        return !apputils.isUndefined(value);
    };

    //see https://stackoverflow.com/questions/45589902/equivalent-in-javascript-to-datetime-ticks
    this.apputils.TimeTicks = function () {
        const d = new Date();
        return (d.getTime() * 10000) + 621355968000000000;
    };

    //function to pad '0's based on model template default value
    this.apputils.toSimpleFormat = function (field, defaultValue) {
        switch (field.dataType) {
            //case ("Date"):
            case ("Decimal"):
            case ("int"): {
                return apputils.simpleFormatterForNow(defaultValue, field.value);
            }
            default:
                return "";
        }
    };

    //Kendo doesn't handle enable well: https://github.com/telerik/kendo-angular/issues/2006
    this.apputils.enableDropDownList = function (ddList, enable) {
        const dropDownList = ddList.data("kendoDropDownList");

        if (apputils.isUndefined(dropDownList)) {
            return;
        }

        dropDownList.enable(enable);

        ddList[0].readOnly = !enable;
    };

    this.apputils.DayOfWeek = {
        Sunday: 0,
        Monday: 1,
        Tuesday: 2,
        Wednesday: 3,
        Thursday: 4,
        Friday: 5,
        Saturday: 6,
    };
    Object.freeze(this.apputils.DayOfWeek);

    /**
     *  getFormatedDate - returns date as string in correct locale formatting.
     */
    this.apputils.getFormatedDate = function (value) {

        if (typeof value === Date) {
            return kendo.toString(value, "d");

        } else {
            return kendo.toString(new Date(value), "d");
        }
    };

    /**
     *  getDateArray - returns a filled array of dates from startDate to endDate.
     */
    this.apputils.getDateArray = function (startDate, endDate) {
        let arr = new Array();
        let start = new Date(startDate);
        const end = new Date(endDate);
        while (start <= end) {
            const formattedDate = apputils.getFormatedDate(start);
            arr.push(formattedDate);

            start.setDate(start.getDate() + 1);
        }
        return arr;
    }

    /**
     *  getDatefromDay - passing the day [e.g.: apputils.DayOfWeek.Monday] as input this function will return the date
     *  that falls on this day within the provided start and end date range.
     */
    this.apputils.getDatefromDay = function (dayOfWeek, startDate, endDate) {
        let givenDate;

        const dateRange = apputils.getDateArray(startDate, endDate);

        dateRange.every(date => {
            if (new Date(date).getDay() === dayOfWeek) {
                givenDate = date;
            }

            return apputils.isUndefined(givenDate);
        });

        return givenDate;
    }

    /**
     *  getDayfromDate - passing the date "3/21/2023" and array of [e.g.: apputils.DayOfWeek.Monday] as inputs this function will return the day
     *  that falls on this date.
     */
    this.apputils.getDayfromDate = function (date, allDaysOfWeek) {
        let givenDay;

        allDaysOfWeek.every(obj => {
            if (new Date(date).getDay() === obj.dayOfWeek) {
                givenDay = obj;
            }

            return apputils.isUndefined(givenDay);
        });

        return givenDay;
    }

    this.apputils.getDateRangeAsDays = function (start, end) {
        let arr = []
        for (let dt = new Date(start); dt <= new Date(end); dt.setDate(dt.getDate() + 1)) {
            arr.push((new Date(dt)).getDay());
        }
        return arr;
    };

    /**
     * @name getTimePicker
     * @description Create and initialize the kendo timepicker control
     * @param {string} controlId The string controlId
     */
    this.apputils.getTimePicker = function (controlId) {
        let twentyPlus = false;
        let timepicker = $(controlId);

        if (!apputils.isDefined(timepicker)) {
            return;
        }

        timepicker.kendoMaskedTextBox({
            promptChar: "0",
            mask: "ab:cd:ef",
            rules: {
                "a": function (char) {
                    const digit = parseInt(char);

                    // Reject non-numeric characters
                    if (isNaN(digit)) {
                        return false;
                    }

                    // First digit can only be 0, 1 or 2
                    if (digit >= 0 && digit <= 2) {

                        // if first digit is a 2, then 
                        // set flag so we know about it 
                        // when processing the next digit
                        if (digit === 2) {
                            twentyPlus = true;
                        } else {
                            twentyPlus = false;
                        }
                        return true;
                    } else {
                        return false;
                    }
                },

                "b": function (char) {
                    const digit = parseInt(char);

                    // Reject non-numeric characters
                    if (isNaN(digit)) {
                        return false;
                    }

                    // if first digit is a two 
                    // and second digit is greater than 3, reject it.
                    if (twentyPlus === true) {
                        if (digit > 3) {
                            return false;
                        }
                    }

                    return true;
                },

                "c": /[0-5]/,
                "d": /[0-9]/,
                "e": /[0-5]/,
                "f": /[0-9]/
            }
        });

        timepicker.closest(".k-timepicker")
            .add(timepicker)
            .removeClass("k-textbox");
    };

    this.apputils.unFormatPJCFmtcontno = function (val) {

        if (!val || val === "") {
            return "";
        }

        return val.replace(/[^a-zA-Z0-9$&%<>]/g, '');
    };

    this.apputils.Trim = function (v) {

        return v.trim();
    };

    this.apputils.EscapeQuotes = function (v) {

        return v;
    };

    this.apputils.Left = function (t, l) {
        return t.substring(0, l);
    };

    this.apputils.Right = function (t, l) {
        return t.substring((t.length) - l, (t.length));
    };
    
    this.apputils.getScreenNameByRotoId = function(id) {
        switch (id) {
            //AP
            case ("AP2100"): 
                return "InvoiceEntry";
            case ("AP3100"):
                return "PaymentEntry";
            case ("AP4100"):
                return "AdjustmentEntry";
             
            //AR
            case ("AR2100"):
                return "InvoiceEntry";
            case ("AR3100"):
                return "ReceiptEntry";
            case ("AR4100"):
                return "AdjustmentEntry";
            
            //CP
            case ("CP2350"):
                return "CheckInquiry";

            //UP
            case ("UP2350"):
                return "CheckInquiry";

            //PO
            case ("PO1210"):
                return "PurchaseOrderEntry";
            case ("PO1400"):
                return "InvoiceEntry";
            case ("PO1310"):
                return "ReceiptEntry";
            case ("PO1320"):
                return "ReturnEntry";
            case ("PO1500"):
                return "CreditDebitNoteEntry";

            //OE
            case ("OE1100"):
                return "OrderEntry";
            case ("OE1900"):
                return "InvoiceEntry";
            case ("OE2200"):
                return "ShipmentEntry";
            case ("OE1600"):
                return "CreditDebitNoteEntry";

            //PM
            case ("PM2010"):
                return "Costs";
            case ("PM2040"):
                return "MaterialUsages";
            case ("PM2050"):
                return "MaterialReturns";
            case ("PM2070"):
                return "EquipmentUsage";
            case ("PM2080"):
                return "Charges";
            case ("PM2100"):
                return "Timecards";
            case ("PM2200"):
                return "Adjustments";
            case ("PM2320"):
                return "OpeningBalances";
            default:
                return ""; //throw an error at usage if required
        }
    };

    /**
    * Example taken from https://www.freecodecamp.org/news/javascript-debounce-example/
    * This function calls a function initially and any subsequent calls within the timeout
    * period is ignored and the timeout resets.
    * @param {function} func
    * @param {numer} timeout
    * @returns function
    */
    this.apputils.throttle = function (func, timeout = 300) {
        let timer;
        return (...args) => {
            if (!timer) {
                func.apply(this, args);
            }
            clearTimeout(timer);
            timer = setTimeout(() => {
                timer = undefined;
            }, timeout);
        };
    };

    //add code above this line

    if (typeof define === 'function' && define.amd) {
        define('apputils', [], function () {
            return apputils;
        });
    }

}).call(this);