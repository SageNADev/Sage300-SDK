
// $generatedMessage$ 
// $generatedWarning$ 

"use strict";

var $companyname$$customizationName$$screenName$CustomizationUI = $companyname$$customizationName$$screenName$CustomizationUI || {};
var $companyname$$customizationName$$screenName$customizationViewModel;

$companyname$$customizationName$$screenName$CustomizationUI = {

    // Init
    init: function () {
        $companyname$$customizationName$$screenName$CustomizationUI.initIntercept();
        $companyname$$customizationName$$screenName$CustomizationUI.initAjaxCallIntercept();
        $companyname$$customizationName$$screenName$CustomizationUI.initButtons();
        $companyname$$customizationName$$screenName$CustomizationUI.initFinders();
        $companyname$$customizationName$$screenName$CustomizationUI.initTextbox();
        $companyname$$customizationName$$screenName$CustomizationUI.initNumericTextBox();
        $companyname$$customizationName$$screenName$CustomizationUI.initDropDownList();
        $companyname$$customizationName$$screenName$CustomizationUI.initDatePicker();
        $companyname$$customizationName$$screenName$CustomizationUI.initCheckBoxes();
        $companyname$$customizationName$$screenName$CustomizationUI.initGrid();
        $companyname$$customizationName$$screenName$CustomizationUI.initOtherControls();
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
        //sg.viewFinderHelper.setViewFinderEx("btnCustomCurrencyFinder", "txtCustomCurrency", sg.viewFinderProperties.CS.CurrencyCodes, ValuedPartnerBeforePaymentCodeCustomizationUICallback.currencyCode.currencyCode, null);
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


var $companyname$$customizationName$$screenName$CustomizationUICallback = {
};

// Initial Entry
$(function () {
    $companyname$$customizationName$$screenName$CustomizationUI.init();
});
