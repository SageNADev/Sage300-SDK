
"use strict";

var ISV1OrderEntryCustomizationUI = ISV1OrderEntryCustomizationUI || {};

ISV1OrderEntryCustomizationUI = {

    orderNumber: "",
    total: [0, 0, 0],

    // Init
    init: function () {
        sg.utls.collapsibleScreen.setup("expandedEntry", "simpleEntry", ["pnlCustomIdHeader"], []);
        ISV1OrderEntryCustomizationUI.initIntercept();
        ISV1OrderEntryCustomizationUI.initAjaxCallIntercept();
        ISV1OrderEntryCustomizationUI.initButtons();
        ISV1OrderEntryCustomizationUI.initFinders();
        ISV1OrderEntryCustomizationUI.initTextbox();
        ISV1OrderEntryCustomizationUI.initNumericTextBox();
        ISV1OrderEntryCustomizationUI.initDropDownList();
        ISV1OrderEntryCustomizationUI.initDatePicker();
        ISV1OrderEntryCustomizationUI.initCheckBoxes();
        ISV1OrderEntryCustomizationUI.initGrid();
        ISV1OrderEntryCustomizationUI.initOtherControls();
        $("#pnlCustomIdHeader").trigger("click");
    },

    //Hijack and intercept events, before the event
    initIntercept: function () {
        //Prepayment button event intercept
        if ($('#btnPrepayment')[0] == undefined) return;
        var prepaymentHandler = null;
        var prepaymentHandlers = $('#btnPrepayment').data('events').click;
        if (prepaymentHandlers && prepaymentHandlers.length > 0) {
            prepaymentHandler = prepaymentHandlers[0].handler;
        }
        //unbind the original
        $('#btnPrepayment').unbind('click');
        //bind the modified one
        $('#btnPrepayment').click(function () {
            sg.utls.showKendoConfirmationDialog(
                function () {
                    var data = orderEntryUI.orderEntryModel.Data;
                    var amountDue = data.AmountDueLessCurrPrepayment();
                    data.AmountDueLessCurrPrepayment(amountDue + 1000);
                    prepaymentHandler();
                },
                function () {
                    return false;
                },
                "It will intercept Sage 300c Order Entry show prepayment action, add extra cost to the order prepament amount due. Are you sure you want to do that?");
        });

    },

    //Intercept before/after Ajax calls
    initAjaxCallIntercept: function () {

        $(document).ajaxStart(function () {
        });

        //Before prepayment ajax call, extra cost is added to "Amount Due" field
        $(document).ajaxSend(function (event, jqxhr, settings) {
            if (settings.url.indexOf("/OE/OrderEntry/LaunchPrepayment") > -1) {
                //window.alert("Extra cost is added to prepayment.");
                var data = orderEntryUI.orderEntryModel.Data;
                var amountDue = data.AmountDueLessCurrPrepayment();
                data.AmountDueLessCurrPrepayment(amountDue + 1000);
            }
        });

        //After get order details ajax call, calculate total amounts for order details fields, fill custom fields
        $(document).ajaxSuccess(function (event, xhr, settings) {
            if (settings.url.indexOf("/OE/OrderEntry/GetDetails") > -1) {
                
                var data = jQuery.parseJSON(settings.data);
                ISV1OrderEntryCustomizationUI.orderNumber = data.model.OrderNumber;
                if (data.model.OrderNumber && data.model.OrderNumber !== "*** NEW ***") {
                    $("#btnFromCustom").trigger('click');
                    var grid = $("#OrderDetailGrid").data("kendoGrid");
                    var gridData = grid.dataSource.view();
                    ISV1OrderEntryCustomizationUI.total[0] = 0;
                    ISV1OrderEntryCustomizationUI.total[1] = 0;
                    ISV1OrderEntryCustomizationUI.total[2] = 0;
                    for (var i = 0; i < gridData.length; i++) {
                        ISV1OrderEntryCustomizationUI.total[0] += gridData[i].ExtendedPrice;
                        ISV1OrderEntryCustomizationUI.total[1] += gridData[i].DiscountedExtendedAmount;
                        ISV1OrderEntryCustomizationUI.total[2] += gridData[i].ExtendedOrderCost;
                    }
                    $("#txtCustomCTotalCost").val(ISV1OrderEntryCustomizationUI.total[0]);
                    var comment = "Total Cost for " + data.model.OrderNumber + " is " + ISV1OrderEntryCustomizationUI.total[0];
                    $("#txtAreaCustomComments").val(comment);
                    $("#txtCustomCurrency").val("CAD");

                    //Enable custom tab page 
                    var tabStrip = $("#orderEntryTabStrip").kendoTabStrip().data("kendoTabStrip");
					tabStrip.enable(tabStrip.tabGroup.children().eq(7), true);
					
                }
				
            }
        });

        // After getOrderDetails complete, send ajax call to load cutsom order details
        $(document).ajaxComplete(function (event, xhr, settings) {
            if (settings.url.indexOf("/OE/OrderEntry/GetDetails") > -1) {
                var data = jQuery.parseJSON(settings.data);
                if (data.model.OrderNumber && data.model.OrderNumber !== "*** NEW ***") {
                    var url = sg.utls.url.buildUrl("CU", "ISV1Customization", "GetOrderDetails");
                    $.ajax({
                        type: 'get',
                        dataType: 'json',
                        cache: false,
                        //url: url,
                        //Use local web server url, when deploy to Sage 300c, use above url
                        url: 'http://localhost/ISV1.web/OnPremise/CU/ISV1Customization/GetOrderDetails',
                        data: { id: data.model.OrderNumber },
                        success: function (data) {
                            ISV1OrderEntryCustomizationUICallback.loadCustomOrder(data);
                        },
                        error: function () {
                        }
                    });
                }
            }
        });
    },

    // Init Buttons
    initButtons: function () {
        $("#btnFromSage300").bind('click', function () {
            var url = sg.utls.url.buildUrl("CS", "TaxAuthority", "GetCurrencyDescription");
            sg.utls.ajaxPost(url, { currencyCode: "CAD" }, ISV1OrderEntryCustomizationUICallback.getCustomInfo);
        });
    },
	
    // Init TextBoxs
    initTextbox: function(){
    },

    // Init Numeric TextBox
    initNumericTextBox: function () {
        $("#numericOrderAmount").kendoNumericTextBox({
            spinners : false,
            decimals: 3
        });
    },
	
    // Init Dropdowns here
    initDropDownList: function () {
        // Amount type dropdown list 
        $("#dropdownOECostTypeId").kendoDropDownList({
            dataSource: {
                data: ["Extended Price", "Discounted Extended Amount", "Extended Order Cost"]
            },
            change: function (e) {
                var idx = this.selectedIndex;
                $("#txtCustomCTotalCost").val(ISV1OrderEntryCustomizationUI.total[idx]);
            }
        });
        // Order type dropdown list
        $("#dropdownOEOrderTypeId").kendoDropDownList({
        });
		
		// Custom drop down list in custom tab page
        $("#dropdownCustomOrderType").kendoDropDownList({
        });
    },

	    // Init Date Picker
    initDatePicker: function() {
        $("#dtPickerCustom").kendoDatePicker({});
    },

    // Init Finders, if any
    initFinders: function () {
        sg.finderHelper.setFinder("btnCustomCurrencyFinder", sg.finder.TaxCurrencyFinder, ISV1OrderEntryCustomizationUICallback.currencyCode, null, "Custom Currency Finder", sg.finderHelper.createDefaultFunction("txtCustomCurrency", "CurrencyCodeId", sg.finderOperator.StartsWith), null, true);
        sg.finderHelper.setFinder("btnCustomCurrencyFinder1", sg.finder.TaxCurrencyFinder, ISV1OrderEntryCustomizationUICallback.currencyCode1, null, "Custom Currency Finder", sg.finderHelper.createDefaultFunction("txtCustomCurrency1", "CurrencyCodeId", sg.finderOperator.StartsWith), null, true);

    },
	
    //Init CheckBoxs
    initCheckBoxes: function () {
    },

    //Init kendo Grid
    initGrid: function () {
		var gridDataSource = new kendo.data.DataSource({
            data: [
                { OrderNumber: "OrdNumber0001", OrderDescription: "Custom Order Number 1", OrderStatus: "Active", OrderAmount: 12345, OrderCurrency: "CAD" },
                { OrderNumber: "OrdNumber0002", OrderDescription: "Custom Order Number 2", OrderStatus: "Active", OrderAmount: 23456.00, OrderCurrency: "CAD" },
                { OrderNumber: "OrdNumber0003", OrderDescription: "Custom Order Number 3", OrderStatus: "Active", OrderAmount: 5678.00, OrderCurrency: "CAD" },
                { OrderNumber: "OrdNumber0004", OrderDescription: "Custom Order Number 4", OrderStatus: "Active", OrderAmount: 56343.23, OrderCurrency: "CAD" }
            ],
            page: 1
        });
		$("#gridCustomOrder").kendoGrid({
		    dataSource: gridDataSource,
		    height:200,
		    resizable: true,
		    selectable: true,
		    reorderable: true,
		    editable: false,
		    pageable: {
			    input: true,
			    numeric: false,
			    refresh: false
		    },
		    columns: [
			    {
				    field: "OrderNumber",
				    title: "Order Number",
				    attributes: { "class": "w120" },
				    headerAttributes: { "class": "w120" }
			    },
			    {
				    field: "OrderDescription",
				    title: "Order Description",
				    attributes: { "class": "w200" },
				    headerAttributes: { "class": "w200" }
			    },
			    {
				    field: "OrderStatus",
				    title: "Order Status",
				    attributes: { "class": "w120" },
				    headerAttributes: { "class": "w120" },
				    editor: function(container, options) {
				    }
			    },
			    {
				    field: "OrderAmount",
				    title: "Order Amount",
				    attributes: { "class": "w160" },
				    headerAttributes: { "class": "w160" },
				    editor: function(container, options) {
				    }
			    },
			    {
				    field: "OrderCurrency",
				    title: "Currency",
				    attributes: { "class": "w120 align-left" },
				    headerAttributes: { "class": "w120" },
				    editor: function(container, options) {
				    }
			    },
		    ]
	    });
        gridDataSource.read();
    },

    //Init Other type controls
    initOtherControls: function () {
        var tabStrip = $("#orderEntryTabStrip").data("kendoTabStrip");
		tabStrip.enable(tabStrip.tabGroup.children().eq(7), false);
    },

};


var ISV1OrderEntryCustomizationUICallback = {
    
    getCustomInfo: function (result) {
        result = result.CurDescription + "-This info is get from sage300c controller";
        $('#txtFromSage300').val(result);
        //sg.utls.showKendoConfirmationDialog(function () { }, null, "Custom Text box info is get from sage 300 server ajax call");
        sg.controls.Focus($("#txtCustomTextDesc"));
    },

    currencyCode: function(data) {
        if (data) {
            $("#txtCustomCurrency").val(data.CurrencyCodeId);
            sg.controls.Focus($("#txtCustomCurrency"));
        }
    },

    currencyCode1: function(data) {
        if (data) {
            $("#txtCustomCurrency1").val(data.CurrencyCodeId);
            sg.controls.Focus($("#txtCustomCurrency1"));
        }
    },

    // Load custom order details and apply knock out bindings 
    loadCustomOrder: function(data) {
        data.Data.CustomOrderDate = new Date(parseInt(data.Data.CustomOrderDate.substr(6))).toLocaleDateString();
        var customViewModel = ko.mapping.fromJS(data);
        // apply knock out binding to custom page content 
        ko.applyBindings(customViewModel, document.getElementById("orderEntryTabStrip-8"));
        // apply to dropdown list using kendo data source
        $("#dropdownCustomOrderType").kendoDropDownList({
            dataSource: data.Data.CustomOrderType,
            change: function (e) {
            }
        });
        $("#numericOrderAmount").data("kendoNumericTextBox").value(data.Data.CustomOrdNumberAmount);
        $("#txtCustomCurrency1").val(data.Data.CustomOrderCurrency);
    }
};

// Initial Entry
$(function () {
    ISV1OrderEntryCustomizationUI.init();
});
