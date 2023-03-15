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

    this.apputils.createRowNode = function ({ viewId, filter = "", parentId = "", id = "0", verb = apputils.CRUDReasons.Get }) {
        filter = filter.length > 0 ? filter : "f=''";
        filter = filter.includes("f=") ? filter : `f='${filter}'`;

        return `<r i='${viewId}' ${filter} p='${parentId}' id='${id}' verb='${verb}'>`;

    };

    this.apputils.rowNodeClose = "</r>";
    Object.freeze(this.apputils.rowNodeClose);

    this.apputils.createFieldNode = function ({ fieldId, value, parentId = "0" }) {
        return `<c f='${fieldId}' v='${value}' p='${parentId}'/>`;

    };

    this.apputils.EventMsgTags = {
        svrUpdate: "svrUpdate",
        usrUpdate: "usrUpdate",
        svrValid: "svrValid",
        svrInvalid: "svrInvalid",
        userctx: "userctx"
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
        Verify: "Verify"
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
        Int: "int",
        Date: "Date",
        Bool: "Bool",
        Amount: "amount",
        Money: "money"
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

        if (apputils.isUndefined(apputils.currencyCodes)) {
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

    this.apputils.formatNumber = function (value, precision) {
        //Some value always use '.' as decimal seperator, it will cause issue in localization.
        const decimalSeparator = kendo.culture().numberFormat['.'];
        value = value ? value.toString() : "0";

        if (value && value.includes('.')) {
            value = value.replace('.', decimalSeparator);
        }

        let number = kendo.parseFloat(value);

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
        let userDecimals = apputils.countDecimals(number);
        if (+precision === 0) {
            let result = Math.pow(10, userDecimals);
            return kendo.toString((value * result), "n0");
        }
        return kendo.toString(number, `n${precision}`);
    };

    //Some numeric textbox need min value settings, like QUANTITY field. Numeric default value should be minVal or 0, can't be empty(null)
    this.apputils.initNumericTextBoxAndSetValue = function (id, val, minVal, decimal) {
        let precision = apputils.isUndefined(decimal) ? apputils.decimalPlaceOfFunctionalCurrency(): decimal;

        let numericTxtBox = $("#" + id).kendoNumericTextBox({
            decimals: precision,
            format: "n" + precision,
            spinners: false,
            min: minVal,
            value: val
        });
        sg.utls.kndoUI.restrictDecimals(numericTxtBox, precision, precision ? 16 - precision : 14);
    };

    this.apputils.formatKendoNumericTextBoxForDecimals = function (ctrId, data, minVal, decimal) {
        let kendoBox = $("#" + ctrId).data("kendoNumericTextBox");

        if (kendoBox && kendoBox.value) {
            let precision = apputils.isUndefined(decimal) ? apputils.decimalPlaceOfFunctionalCurrency(): decimal;
            kendoBox.setOptions({
                value: data,
                decimals: precision,
                format: "n" + precision,
                min: minVal
            });
            sg.utls.kndoUI.restrictDecimals(kendoBox, precision, precision ? 16 - precision : 14);
        } else {
            apputils.initNumericTextBoxAndSetValue(ctrId, data, minVal, decimal);
        }
    };

    this.apputils.InitNoNullItemsArray = function () {
        let arrObj = [];
        arrObj.addItems = function (value) {
            if (value) { this.push(value); }
        };

        return arrObj;
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
    }

    if (typeof define === 'function' && define.amd) {
        define('apputils', [], function () {
            return apputils;
        });
    }

}).call(this);