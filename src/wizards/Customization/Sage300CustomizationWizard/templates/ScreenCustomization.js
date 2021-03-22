
// $generatedMessage$ 
// $generatedWarning$ 

"use strict";

var $companyname$$screenName$CustomizationUI = $companyname$$screenName$CustomizationUI || {};
var $companyname$$screenName$customizationViewModel;

$companyname$$screenName$CustomizationUI = {

    // Init
    init: function () {
        $companyname$$screenName$CustomizationUI.initIntercept();
        $companyname$$screenName$CustomizationUI.initAjaxCallIntercept();
        $companyname$$screenName$CustomizationUI.initButtons();
        $companyname$$screenName$CustomizationUI.initFinders();
        $companyname$$screenName$CustomizationUI.initTextbox();
        $companyname$$screenName$CustomizationUI.initNumericTextBox();
        $companyname$$screenName$CustomizationUI.initDropDownList();
        $companyname$$screenName$CustomizationUI.initDatePicker();
        $companyname$$screenName$CustomizationUI.initCheckBoxes();
        $companyname$$screenName$CustomizationUI.initGrid();
        $companyname$$screenName$CustomizationUI.initOtherControls();
    },

    //Hijack and intercept events, before the event
    initIntercept: function () {
    },

    //Intercept before/after Ajax calls
    initAjaxCallIntercept: function () {

        $(document).ajaxSend(function (event, jqxhr, settings) {
        });

        $(document).ajaxSuccess(function (event, xhr, settings) {
        });

    },

    // Init Buttons
    initButtons: function () {
    },

    // Init TextBoxs
    initTextbox: function(){
    },

    // Init Numeric TextBox
    initNumericTextBox: function () {
    },

    // Init Dropdown List
    initDropDownList: function () {
    },

    // Init Date Picker
    initDatePicker: function() {
        //$("#dtPickerCustom").kendoDatePicker({});
    },

    // Init Sage 300 Finders
    initFinders: function () {
        //sg.finderHelper.setFinder("btnCustomCurrencyFinder", sg.finder.TaxCurrencyFinder, $companyname$$screenName$CustomizationUICallback.currencyCode, null, "Customization Currency Finder", sg.finderHelper.createDefaultFunction("txtCustomCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, true);
    },

    //Init CheckBoxs
    initCheckBoxes: function () {
    },

    //Init kendo Grid
    initGrid: function () {
    },

    //Init Other type controls
    initOtherControls: function () {
    },

};


var $companyname$$screenName$CustomizationUICallback = {
};

// Initial Entry
$(function () {
    $companyname$$screenName$CustomizationUI.init();
});
