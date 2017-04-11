/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */


var CurrencyHelper = {
    Currency: {},
    SetCurrency: function (data) {
        CurrencyHelper.Currency = {
            CurrencyCode: data.code,
            Description: data.Description,
            Symbol:  data.Symbol,
            DecimalPlaces: data.Decimals,
            SymbolPosition: data.SymbolDisplay,
            ThousandsSeparator: data.ThousandSeparator,
            DecimalSeparator: data.DecimalSeparator,
            NegativeDisplay: data.NegativeDisplay,
        };
        return Currency;
    },

    Get: function (code, sucessHandler)
    {
        var data = {
            'currencyCode': code
        };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Common", "GetCurrency"), data, function (successdata) {
            //CurrencyHelper.SetCurrency(successdata);
            CurrencyHelper.SucessHandler(successdata);
        });
    },
};



