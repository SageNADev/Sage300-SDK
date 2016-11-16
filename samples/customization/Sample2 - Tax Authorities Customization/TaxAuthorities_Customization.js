
"use strict";

var customTaxAuthoritiesUI = customTaxAuthoritiesUI || {};
var customViewModel;

customTaxAuthoritiesUI = {
    // Init
    init: function () {
        customTaxAuthoritiesUI.initAddCustomControls();
        customTaxAuthoritiesUI.initIntercept();
        customTaxAuthoritiesUI.initAjaxCallIntercept();
        customTaxAuthoritiesUI.initButtons();
        customTaxAuthoritiesUI.initFinders();
        customTaxAuthoritiesUI.initTextbox();
        customTaxAuthoritiesUI.initNumericTextBox();
        customTaxAuthoritiesUI.initDropDownList();
        customTaxAuthoritiesUI.initDatePicker();
        customTaxAuthoritiesUI.initCheckBoxes();
        customTaxAuthoritiesUI.initGrid();
    },

    // Add custom controls from client side JavaScript
    initAddCustomControls: function () {
    },

    //Hijack and intercept events, before the event
    initIntercept: function () {
    },

    //Intercept before/after Ajax calls
    initAjaxCallIntercept: function () {

        $(document).ajaxStart(function () {
        });

        $(document).ajaxSend(function (event, jqxhr, settings) {
        });

        $(document).ajaxSuccess(function (event, xhr, settings) {
        });

        $(document).ajaxComplete(function (event, xhr, settings) {
        });
    },

    // Init Buttons
    initButtons: function () {
        // custom button 
        $("#btnCustom").bind('click', function () {
            sg.utls.showKendoConfirmationDialog(function() {}, null, "Customization button click, please add Javascript code to do real work.", "Demo");
        });
		// send ajax call to get currency description 
        $("#btnCustom1").bind('click', function () {
            var url = sg.utls.url.buildUrl("CS", "TaxAuthority", "GetCurrencyDescription");
            sg.utls.ajaxPost(url, { currencyCode: "CAD" }, customTaxAuthoritiesUICallback.getCustomInfo);
        });
    },

    // Init TextBoxs
    initTextbox: function(){
    },

    // Init Numeric TextBox
    initNumericTextBox: function () {
    },

    // Init Dropdowns here
    initDropDownList: function () {
        $("#dropdownCustom").kendoDropDownList({
            change: function (e) {
                var value = this.value();
                // Use the value of the widget
            }
        });
        $("#dropdownCustom1").kendoDropDownList({
        });
    },

    // Init Date Picker
    initDatePicker: function() {
        $("#dtPickerCustom").kendoDatePicker({});
    },

    // Init Finders, if any
    initFinders: function () {
        sg.finderHelper.setFinder("btnCustomCurrencyFinder", sg.finder.TaxCurrencyFinder, customTaxAuthoritiesUICallback.currencyCode, null, "Custom Currency Finder", sg.finderHelper.createDefaultFunction("txtCustomCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, true);
    },

    //Init CheckBoxs
    initCheckBoxes: function () {
    },

    //Init Grid
    initGrid: function () {

        var gridDataSource = new kendo.data.DataSource({
            data: [
                { TaxAuthority: "Canada BC Province GST", TaxClass: 1, TaxIncluded: "Yes", TaxBase: "Canada, BC", TaxAmount: 200000000.00 },
                { TaxAuthority: "Canada BC Province PST", TaxClass: 1, TaxIncluded: "Yes", TaxBase: "Canada, BC", TaxAmount: 300000000.00 },
                { TaxAuthority: "US California State Tax", TaxClass: 1, TaxIncluded: "No", TaxBase: "USA, CA", TaxAmount: 1200000000.00 },
                { TaxAuthority: "US California County Tax", TaxClass: 1, TaxIncluded: "NO", TaxBase: "USA, CA", TaxAmount: 20000000000.00 }
            ]
        });

        $("#gridCustom").kendoGrid({
            dataSource: gridDataSource,
            height:250,
            resizable: true,
            selectable: true,
            reorderable: true,
            editable: {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },
            columns: [
                {
                    field: "TaxAuthority",
                    title: "TaxAuthority",
                    attributes: { disabled: "true", "class": "w160"},
                    headerAttributes: { "class": "gird_culm_12"}
                },
                {
                    field: "TaxClass",
                    title: "TaxClass",
                    attributes: { "class": "w80 align-right" },
                    editable: true,
                    headerAttributes: { "class": "w80" },
                    editor: function(container, options) {
                    }
                },
                {
                    field: "TaxIncluded",
                    title: "TaxIncluded",
                    attributes: { "class": "w120" },
                    headerAttributes: { "class": "w120" },
                    editor: function(container, options) {
                    }
                },
                {
                    field: "TaxBase",
                    title: "TaxBase",
                    attributes: { "class": "w160" },
                    headerAttributes: { "class": "w160" },
                    editor: function(container, options) {
                    }
                },
                {
                    field: "TaxAmount",
                    title: "TaxAmount",
                    attributes: { "class": "w160 align-right" },
                    headerAttributes: { "class": "w160 align-right" },
                    editor: function(container, options) {
                    }
                },
                {
                    field: "CanIncludeTax",
                    hidden: true
                }
            ]
        });

        gridDataSource.read();
    }

};


var customTaxAuthoritiesUICallback = {
    
    getCustomInfo: function (result) {
        result = result.CurDescription + "-This info is get from sage300c controller";
        $('#txtCustomTextDesc2').val(result);
        sg.utls.showKendoConfirmationDialog(function () { }, null, "Custom Text box info is get from sage 300 server ajax call");
        sg.controls.Focus($("#txtCustomTextDesc2"));
    },

    currencyCode: function (data) {
    if (data) {
        $("#txtCustomCurrency").val(data.CurrencyCodeId);
        sg.controls.Focus($("#txtCustomCurrency"));
    }
}
};

// Initial Entry
$(function () {
    customTaxAuthoritiesUI.init();
});
