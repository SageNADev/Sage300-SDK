
//TODO: Add/Edit JavaScript code to implement business logic for customizing the screen. For details, see document and samples

"use strict";

var ISV1CustomizationUI = ISV1CustomizationUI || {};
var ISV1customizationViewModel;

ISV1CustomizationUI = {
    baseUrl : "http://localhost:51959/OnPremise/CU/ISV1Customization/",
    dropdownListId: "",
    detailGridId: "",
    detailModel: null,
    tabPageStripId: "",

    // Init
    init: function () {
        ISV1CustomizationUI.initIntercept();
        ISV1CustomizationUI.initAjaxCallIntercept();
        ISV1CustomizationUI.initButtons();
        ISV1CustomizationUI.initFinders();
        ISV1CustomizationUI.initTextbox();
        ISV1CustomizationUI.initNumericTextBox();
        ISV1CustomizationUI.initDropDownList();
        ISV1CustomizationUI.initDatePicker();
        ISV1CustomizationUI.initCheckBoxes();
        ISV1CustomizationUI.initGrid();
        ISV1CustomizationUI.initOtherControls();
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

        $(document).ajaxComplete(function (event, xhr, settings) {

        });

    },

    // Init Buttons
    initButtons: function () {
        $("#btnFromSage300").bind('click', function () {
            var url = sg.utls.url.buildUrl("CS", "TaxAuthority", "GetCurrencyDescription");
            sg.utls.ajaxPost(url, { currencyCode: "CAD" }, ISV1CustomUICallback.getCustomInfo);
        });
    },

    // Init TextBoxs
    initTextbox: function(){
    },

    // Init Numeric TextBox
    initNumericTextBox: function () {
    },

    // Init Dropdown List
    initDropDownList: function () {
        //Use Sage300 CS query to get data  
        $("#orderNumberList").kendoDropDownList({
            change: function (e) {
                ISV1CustomizationUI.detailModel = "OrderDetails";
                ISV1CustomizationUI.getById(this.value(), "gridOrderDetail", "GetBySage300CSQuery");
            }
        });
        //Use Sage 300 view to get data
        $("#customerList").kendoDropDownList({
            change: function (e) {
                ISV1CustomizationUI.detailModel = "CustomerOptionalFields";
                ISV1CustomizationUI.getById(this.value(), "gridCustomerOptionalField", "GetBySage300View");
            }
        });

        //Use Entity Framework to get data
        $("#arCustomerList").kendoDropDownList({
            change: function (e) {
                ISV1CustomizationUI.detailModel = "ARCustomerOptionalFields";
                ISV1CustomizationUI.getById(this.value(), "gridARCustomerOptionalField", "GetByEntityFramework");
            }
        });
    },

    ajaxCall: function (url, type, data, callback) {
        // when delpoy to Sage 300c, it should use correct url
        //var url = sg.utls.url.buildUrl("CU", "DBGCCustomization", "GetOrderDetails");
        $.ajax({
            type: type,
            dataType: 'json',
            cache: false,
            url: url,
            data: data,
            success: function (data) {
                callback(data);
            },
            error: function () {
            }
        });

    },
    // Init Date Picker
    initDatePicker: function() {
        $("#dtOrderCreated").kendoDatePicker({
        });

    },

    // Init Sage 300 Finders
    initFinders: function () {
    },

    //Init CheckBoxs
    initCheckBoxes: function () {
    },

    //Init kendo Grid
    initGrid: function () {
        var gridSettings = {
            scrollable: true,
            resizable: true,
            navigatable: true,
            selectable: true,
            sortable: true,
            pageable: false
        };
        $("#gridOrderDetail").kendoGrid(gridSettings);
        $("#gridCustomerOptionalField").kendoGrid(gridSettings);
        $("#gridARCustomerOptionalField").kendoGrid(gridSettings);
    },

    //Init Other type controls
    initOtherControls: function () {
        //binding tab page select event       
        var tabStrip = $("#orderEntryTabStrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.bind("select", ISV1CustomizationUI.tabPageSelect);
    },

    tabPageSelect: function (e) {
        // set ajax call to get all ids for populate drop down list
        var tabPageId = e.item.id;
        var actionName = "";

        if (tabPageId === "tabPageCSQuery") {
            actionName = "GetAllBySage300CSQuery";
            ISV1CustomizationUI.dropdownListId = "orderNumberList";
            ISV1CustomizationUI.tabPageStripId = "orderEntryTabStrip-10";
        } else if (tabPageId === "tabPageSageView") {
            actionName = "GetAllBySage300View";
            ISV1CustomizationUI.dropdownListId = "customerList";
            ISV1CustomizationUI.tabPageStripId = "orderEntryTabStrip-9";
        } else if (tabPageId === "tabPageEntityFramework") {
            actionName = "GetAllByEntityFramework";
            ISV1CustomizationUI.dropdownListId = "arCustomerList";
            ISV1CustomizationUI.tabPageStripId = "orderEntryTabStrip-8";
        }
        var url = ISV1CustomizationUI.baseUrl + actionName;
        ISV1CustomizationUI.ajaxCall(url, 'get', {}, ISV1CustomizationUICallback.populateDropDownList);
    },

    getById : function(id, gridId, actionName ) {
        ISV1CustomizationUI.detailGridId = gridId;
        var url = ISV1CustomizationUI.baseUrl + actionName;
        ISV1CustomizationUI.ajaxCall(url, 'get', { id: id }, ISV1CustomizationUICallback.getDetails);
    },

    deleteById: function (e) {

    },

    save: function (e) {

    },
};

// Ajax call back functions
var ISV1CustomizationUICallback = {
    populateDropDownList : function (data) {
        var dropdownlist = $('#' + ISV1CustomizationUI.dropdownListId).data("kendoDropDownList");
        dropdownlist.setDataSource(data);
    },

    getDetails: function (data) {
        //format date field
        if (data.Data.OrderDate) {
            data.Data.OrderDate = new Date(parseInt(data.Data.OrderDate.substr(6))).toLocaleDateString();
        }
        
        var viewModel = ko.mapping.fromJS(data);
        // apply knock out binding to custom page content 
        ko.applyBindings(viewModel, document.getElementById(ISV1CustomizationUI.tabPageStripId));

        var detailGrid = $('#' + ISV1CustomizationUI.detailGridId).data("kendoGrid");

        var dataSource = new kendo.data.DataSource({
            data: data.Data[ISV1CustomizationUI.detailModel]
        });
        detailGrid.setDataSource(dataSource);
        // Hide some columns, it's better in server side remove these not required fields
        var columnCount = detailGrid.columns.length;
        if (columnCount > 8) {
            for (var i = 8; i < columnCount; i++) {
                detailGrid.hideColumn(i);
            }
        }
}

};
// Initial Entry
$(function () {
    ISV1CustomizationUI.init();
});
