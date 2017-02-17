
// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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

//TODO: Add/Edit JavaScript code to implement business logic for customizing the screen. For details, see document and samples

"use strict";

var ISV1CustomizationUI = ISV1CustomizationUI || {};
var ISV1customizationViewModel;

ISV1CustomizationUI = {
    //Sample test base url, when integrate with Sage 300c, should use correct sage 300c url, like "http://{HostName}/Sage300/OnPremise/CU/ISV1Customization/"
    baseUrl: "http://localhost:51959/OnPremise/CU/ISV1Customization/",
    //baseUrl: "http://localhost/Sage300/OnPremise/CU/ISV1Customization/",
    viewModel: null,
    dropdownListId: "",
    detailGridId: "",
    detailModel: null,
    tabPageStripId: "",
    saveActionName: "",
    deleteActionName: "",

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
        $("#btnDemoSave").on('click', function () {
            ISV1CustomizationUI.save();
        });

        $("#btnDemoDelete").on('click', function () {
            sg.utls.showKendoConfirmationDialog(function () {
                ISV1CustomizationUI.deleteById();
            }, null, "Are you sure to delete?", "Delete record");
        });

    },

    // Init TextBoxs
    initTextbox: function () {
        $('#txtOrderNumber').prop('disabled', true);
        $('#txtCustomerNumber').prop('disabled', true);
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
            ISV1CustomizationUI.deleteActionName = "DeleteBySage300CSQuery";
            ISV1CustomizationUI.saveActionName = "SaveBySage300CSQuery";
        } else if (tabPageId === "tabPageSageView") {
            actionName = "GetAllBySage300View";
            ISV1CustomizationUI.dropdownListId = "customerList";
            ISV1CustomizationUI.tabPageStripId = "orderEntryTabStrip-9";
            ISV1CustomizationUI.deleteActionName = "DeleteBySage300View";
            ISV1CustomizationUI.saveActionName = "SaveBySage300View";
        } else if (tabPageId === "tabPageEntityFramework") {
            actionName = "GetAllByEntityFramework";
            ISV1CustomizationUI.dropdownListId = "arCustomerList";
            ISV1CustomizationUI.tabPageStripId = "orderEntryTabStrip-8";
            ISV1CustomizationUI.deleteActionName = "DeleteByEntityFramework";
            ISV1CustomizationUI.saveActionName = "SaveByEntityFramework";
        }

        if (actionName) {
            var url = ISV1CustomizationUI.baseUrl + actionName;
            ISV1CustomizationUI.ajaxCall(url, 'get', {}, ISV1CustomizationUICallback.populateDropDownList);
        }
    },

    getById : function(id, gridId, actionName ) {
        ISV1CustomizationUI.detailGridId = gridId;
        var url = ISV1CustomizationUI.baseUrl + actionName;
        ISV1CustomizationUI.ajaxCall(url, 'get', { id: id }, ISV1CustomizationUICallback.getDetails);
    },

    deleteById: function () {
        var id = $('#' + ISV1CustomizationUI.dropdownListId).val();
        var url = ISV1CustomizationUI.baseUrl + ISV1CustomizationUI.deleteActionName;
        ISV1CustomizationUI.ajaxCall(url, 'post', { id: id }, ISV1CustomizationUICallback.deleteById)
    },

    save: function (e) {
        var id = $('#' + ISV1CustomizationUI.dropdownListId).val();
        var url = ISV1CustomizationUI.baseUrl + ISV1CustomizationUI.saveActionName;
        var modelData = ko.mapping.toJS(ISV1CustomizationUI.viewModel.Data);
        ISV1CustomizationUI.ajaxCall(url, 'post', { model: modelData }, ISV1CustomizationUICallback.save)
    },
};

// Ajax call back functions
var ISV1CustomizationUICallback = {
    populateDropDownList: function (data) {
        //set dropdown list data source, select the first item and load details
        var dropdownlist = $('#' + ISV1CustomizationUI.dropdownListId).data("kendoDropDownList");
        dropdownlist.setDataSource(data);
        dropdownlist.select(0);
        dropdownlist.trigger("change")
    },

    getDetails: function (data) {
        // using Knock out mapping to get observable view model for two way bindings
        var viewModel = ko.mapping.fromJS(data);
        // apply knock out binding to custom page content 
        ko.applyBindings(viewModel, document.getElementById(ISV1CustomizationUI.tabPageStripId));
        ISV1CustomizationUI.viewModel = viewModel;
        // set detail grid data source
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
    },

    deleteById: function (data) {
        if (!data.UserMessage) {
            sg.utls.showMessageInfo(sg.utls.msgType.INFO, data);
        } else {
            sg.utls.showMessage(data);
        }

        var value = $('#' + ISV1CustomizationUI.dropdownListId).val();
        var dropdownlist = $('#' + ISV1CustomizationUI.dropdownListId).data("kendoDropDownList");
        var list = dropdownlist.dataSource.data().filter(function (item) {
            return item != value;
        });
        
        dropdownlist.setDataSource(list);
        dropdownlist.select(0);
        dropdownlist.trigger("change")
    },

    save: function (data) {
        if (!data.UserMessage) {
            sg.utls.showMessageInfo(sg.utls.msgType.INFO, "Data save successfully!");
        } else {
            sg.utls.showMessage(data);
        }
    },
};
// Initial Entry
$(function () {
    ISV1CustomizationUI.init();
});
