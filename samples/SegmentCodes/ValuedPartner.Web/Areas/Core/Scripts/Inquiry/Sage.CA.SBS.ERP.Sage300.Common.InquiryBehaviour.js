/* Copyright (c) 2016-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var inquiryUI = inquiryUI || {};
inquiryUI = {
    module: sg.utls.url.getParameterByName("module", location.href),
    inquiryType: sg.utls.url.getParameterByName("inquiryType", location.href),
    currencyType: { Customer: 0, Functional: 1 },
    //TODO: Remove if not required in the future
    filterOperator: {
        Between: -1,
        Equal: 1,
        GreaterThan: 2,
        LessThan: 3,
        NotEqual: 4,
        Contain: 5,
        Include: 6,
        Exclude: 7,
        ISTOD: 8,
        ISYEST: 9,
        MTODATE: 10,
        YTODATE:  11,
        PYTODATE: 12,
        GreaterThanOrEqual: 13,
        LessThanOrEqual: 14,
        START: 15,
        END: 16,
        AND: 19,
        OR: 20,
        NOT: 21,
    },
    inquiryKoBindingModel : {},
    init: function () {
        inquiryUI.initGrid();
        inquiryUI.inquiryKoBindingModel = ko.mapping.fromJS(InquiryViewModel, { /*'ignore': ["CustomerDocuments"]*/  });
        ko.applyBindings(inquiryUI.inquiryKoBindingModel);
        inquiryUI.initDropDowList();
        inquiryUI.initMultiSelect();
        inquiryUI.initButton();
        inquiryUI.initUserPreference();
        inquiryUI.initFinders();

        // kick start the reading
        inquiryGrid.gridInstance().dataSource.read();

        inquiryUI.initGridKeyBinding();
    },

    refreshSecondGrid: function () {
        // make sure the 2nd grid go back to first page
        inquirySecondGrid.gridInstance().dataSource.page(0);
    },

    isFunctionalCurrency: function () {
        var value = $("#currencyOptions").data("kendoDropDownList").value();
        return value == inquiryUI.currencyType.Functional;
    },

    getCurrentValue: function (fieldName) {
        var grid = inquiryGrid.gridInstance();
        var selectedItem = grid.dataItem(grid.select());
        if (selectedItem) {
            return selectedItem[fieldName];
        } else {
            return "";
        }
    },

    initGridKeyBinding: function () {
        var arrows = [38, 40]; // upd and down arrow keys
        var grid = inquiryGrid.gridInstance();
        grid.table.on("keydown", function (e) {
            if (arrows.indexOf(e.keyCode) >= 0) {
                grid.select(grid.current().closest("tr"));
            }
        })
    },
    initButton: function() {
        $("#btnApply").click(function () {
            //validate "Between" values
            var errorList = [];
            function ValidateBetweenValues(fromValue, toValue, fieldName) {
                if (toValue) {
                    if (toValue.localeCompare(fromValue, undefined, { numeric: true }) < 0) {
                        errorList.push(jQuery.validator.format(InquiryResources.RangeErrorFromValueGreater, fieldName));
                        return false;
                    }
                }
                return true;
            }

            if ($("#OperatorField").data("kendoDropDownList").value() == inquiryUI.filterOperator.Between) {
                ValidateBetweenValues($("#customerNumber").val(), $("#toCustomerNumber").val(), InquiryResources.CustomerNumber);
            }
            if ($("#OperatorField2").data("kendoDropDownList").value() == inquiryUI.filterOperator.Between) {
                ValidateBetweenValues($("#nationalAccountNumber").val(), $("#toNationalAccountNumber").val(), InquiryResources.NationalAccountNumber);
            }
            if (errorList.length > 0) {
                //Don't refresh grid if error occurs
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorList);
                return;
            }
            // Hide previous error messages
            $("#message").hide();
            // Refresh the grid after user clicking Apply
            inquiryGrid.gridInstance().dataSource.page(0);
        })
        $("#btnReset").click(function() {
            $("#Value").val("");
            ($('#docTypeMultiSelect').data("kendoMultiSelect")).value([]);
        });
    },
    initDropDowList: function() {
        sg.utls.kndoUI.dropDownList("InquiryField");
        sg.utls.kndoUI.dropDownList("OperatorField");
        $("#OperatorField").data("kendoDropDownList").bind("change",
            function(e) {
                if (this.value() == inquiryUI.filterOperator.Between) {
                    $("#toCustomerGroup").show();
                    $("#customerNumber").attr('placeholder', 'First');
                    $("#toCustomerNumber").attr('placeholder', 'Last');

                } else {
                    $("#toCustomerGroup").hide();
                    $("#customerNumber").removeAttr('placeholder');
                    $("#toCustomerNumber").removeAttr('placeholder');
                }
            }
        );

        sg.utls.kndoUI.dropDownList("currencyOptions");
        var currencyDropdown = $("#currencyOptions").data("kendoDropDownList");
        if (currencyDropdown) {
            currencyDropdown.bind("change", function (e) {
                inquiryGrid.displayColumnsByCurrencyType(inquiryGrid.gridInstance(), inquiryGrid.custCurrencyColumns, inquiryGrid.funcCurrencyColumns);
				inquiryGrid.displayColumnsByCurrencyType(inquirySecondGrid.gridInstance(), inquirySecondGrid.custCurrencyColumns, inquirySecondGrid.funcCurrencyColumns);
				inquiryUI.refreshSecondGrid();
            });
        }
        
        sg.utls.kndoUI.dropDownList("OperatorField2");
        $("#OperatorField2").data("kendoDropDownList").bind("change",
            function(e) {
                if (this.value() == inquiryUI.filterOperator.Between) {
                    $("#toNationalAccountGroup").show();
                    $("#nationalAccountNumber").attr('placeholder', 'First');
                    $("#toNationalAccountNumber").attr('placeholder', 'Last');
                } else {
                    $("#toNationalAccountGroup").val("").hide();
                    $("#nationalAccountNumber").removeAttr('placeholder');
                    $("#toNationalAccountNumber").removeAttr('placeholder');
                }
        });
        //Temporary initialization
        sg.utls.kndoUI.dropDownList("filter1");
        sg.utls.kndoUI.dropDownList("filter2");
        sg.utls.kndoUI.dropDownList("filter3");
        sg.utls.kndoUI.dropDownList("filter4");
        sg.utls.kndoUI.dropDownList("filter5");
        $("#filter5").data("kendoDropDownList").select(1);
        sg.utls.kndoUI.dropDownList("NationalAccountFilter");

    },
    initFinders: function() {
        finders.initCustomerNumberFinder();
        finders.initNationAccountNumberFinder();
    },
    initGrid: function () {
        sg.utls.mergeGridConfiguration(["pageUrl", "getParam", "buildGridData", "afterDataBind", "dataChange", "change"],
             CustomerDocumentGridConfig, inquiryGrid);

        sg.utls.mergeGridConfiguration(["pageUrl", "getParam", "buildGridData", "afterDataBind", "dataChange", "pageSize"],
             DocumentPaymentGridConfig, inquirySecondGrid);

        inquiryGrid.afterDataBindHandler = inquiryUI.refreshSecondGrid;
        inquiryGrid.changeHandler = inquiryUI.refreshSecondGrid;
    },

    //Initialise User Preference for Grids
    initUserPreference: function () {
        $('#btnEditGridColumns').on('click', function () {
            var preferenceKey = inquiryUI.isFunctionalCurrency()
                ? sg.utls.InquiryPreferences.FuncInquiryGridPreferenceKey
                : sg.utls.InquiryPreferences.CustInquiryGridPreferenceKey;
            GridPreferencesHelper.initialize("#CustomerDocumentGrid", preferenceKey, $(this), CustomerDocumentGridConfig.columns, 0);
        });

        $('#btnEditSecondGridColumns').on('click', function () {
            var preferenceKey = inquiryUI.isFunctionalCurrency()
                ? sg.utls.InquiryPreferences.FuncInquirySecondGridPreferenceKey
                : sg.utls.InquiryPreferences.CustInquirySecondGridPreferenceKey;
            GridPreferencesHelper.initialize("#DocumentPaymentGrid", preferenceKey, $(this), DocumentPaymentGridConfig.columns, 0);
        });

        inquiryGrid.gridInstance().bind('dataBound', function (e) {
            InquiryGridHelper.appendDrillDownLink(inquiryGrid.gridInstance(), inquiryRazorView.isOEActive);
        });

        //hide/show columns depends on Currency Type
        inquiryGrid.displayColumnsByCurrencyType(inquiryGrid.gridInstance(), inquiryGrid.custCurrencyColumns, inquiryGrid.funcCurrencyColumns);
        inquiryGrid.displayColumnsByCurrencyType(inquirySecondGrid.gridInstance(), inquirySecondGrid.custCurrencyColumns, inquirySecondGrid.funcCurrencyColumns);
    },

    initMultiSelect: function() {
        $("#docTypeMultiSelect").kendoMultiSelect({
            autoClose: false,
            select: selectWithSelectAllLogics
        }).data("kendoMultiSelect");

        function contains(value, values) {
            for (var index = 0; index < values.length; index++) {
                if (values[index] === value) {
                    return true;
                }
            }

            return false;
        }

        function selectWithSelectAllLogics(e) {
            var selectAllValue = "-1";
            var dataItemValue = this.dataSource.view()[e.item.index()].value;
            var values = this.value();

            //If an selection already exists, simply return and the widget will unselect this selection
            if (dataItemValue !== selectAllValue && contains(dataItemValue, values)) {
                return;
            }

            //Clear selections when 'All' is selected and remove 'All' when any other selection is selected
            if (dataItemValue === selectAllValue) {
                values = [];
            } else if (values.indexOf(selectAllValue) !== -1) {
                values = $.grep(values, function (value) {
                    return value !== selectAllValue;
                });
            }

            values.push(dataItemValue);
            this.value(values);
            this.trigger("change");

            e.preventDefault();
        }
    },
    generateStaticFilters: function () {
        var filters = [];
        var count = 0;
         
        var value_customerNumber = $("#customerNumber").val().toUpperCase();
        var operatorValue = $("#OperatorField").data("kendoDropDownList").value();
        if (operatorValue == inquiryUI.filterOperator.Between) {
            var value_toCustomerNumber = $("#toCustomerNumber").val().toUpperCase();
            if (value_customerNumber) {
                filters[count] = sg.finderHelper.createInquiryFilter("CustomerNumber", inquiryUI.filterOperator.GreaterThanOrEqual, value_customerNumber.toString(), false);
                ++count;
            }
            if (value_toCustomerNumber){
                filters[count] = sg.finderHelper.createInquiryFilter("CustomerNumber", inquiryUI.filterOperator.LessThanOrEqual, value_toCustomerNumber.toString(), false);
                ++count;
            } 
        } else {
            if (value_customerNumber) { // Any other operator than "between"
                filters[count] = sg.finderHelper.createInquiryFilter("CustomerNumber", operatorValue, value_customerNumber.toString(), false);
                ++count;
            }
        }

        var selection;
        //As of Kendo V2016.3.1118, $('#docTypeMultiSelect').val() doesn't always return the correct selections
        //Issue: select multiple choices, unselect one of the choices by clicking x.  The val() will be incorrect.
        var value2 = ($('#docTypeMultiSelect').data("kendoMultiSelect")).value();
        if (value2[0] === "-1") {
            selection = "(1,2,3,4,5,10,11,19)";
        } else {
            selection = "(" + value2.join() + ")";
        }
        filters[count] = sg.finderHelper.createInquiryFilter("DocumentType", inquiryUI.filterOperator.Include, selection, false);
        ++count;

        var value3 = $("#filter5").val();
        filters[count] = sg.finderHelper.createInquiryFilter("FullyPaid", inquiryUI.filterOperator.Equal, value3, false);
        ++count;

        var nationalAccountNumber = $("#nationalAccountNumber").val().toUpperCase();
        var operatorValue = $("#OperatorField2").data("kendoDropDownList").value();
        if (operatorValue == inquiryUI.filterOperator.Between) {
            var toNationalAccountNumber = $("#toNationalAccountNumber").val().toUpperCase();
            if (nationalAccountNumber) {
                filters[count] = sg.finderHelper.createInquiryFilter("NationalAccountNumber", inquiryUI.filterOperator.GreaterThanOrEqual, nationalAccountNumber.toString(), false);
                ++count;
            }
            if(toNationalAccountNumber){
                filters[count] = sg.finderHelper.createInquiryFilter("NationalAccountNumber", inquiryUI.filterOperator.LessThanOrEqual, toNationalAccountNumber.toString(), false);
                ++count;
            }
        } else {
            if (nationalAccountNumber) { 
                filters[count] = sg.finderHelper.createInquiryFilter("NationalAccountNumber", operatorValue, nationalAccountNumber.toString(), false);
                ++count;
            }
        }

        return filters;
    },

    //Returns the grid preference key associated with the grid depending on the currency type
    getPreferenceKeyByGridName: function(gridName) {
        switch(gridName) {
        case"CustomerDocumentGrid":
            return inquiryUI.isFunctionalCurrency()
                ? sg.utls.InquiryPreferences.FuncInquiryGridPreferenceKey
                : sg.utls.InquiryPreferences.CustInquiryGridPreferenceKey;
        case "DocumentPaymentGrid":
            return inquiryUI.isFunctionalCurrency()
                ? sg.utls.InquiryPreferences.FuncInquirySecondGridPreferenceKey
                : sg.utls.InquiryPreferences.CustInquirySecondGridPreferenceKey;
        default:
            return "";
        }
    },

    //Gets the saved grid user preference from the server and applis it to the grid
    setUserPreference: function (grid) {
        if (grid) {
            var gridName = grid.element[0].id;
            var key = inquiryUI.getPreferenceKeyByGridName(gridName);
            var url = sg.utls.url.buildUrl("Core", "Common", "GetGridPreferences");
            var data = { 'key': key };
            sg.utls.ajaxPost(url, data, function(result) {
                GridPreferencesHelper.setGrid("#" + gridName, result, true);
            });
        }
    }
};

var finders = {
    initCustomerNumberFinder: function () {
        var title = $.validator.format(InquiryResources.FinderTitle, InquiryResources.CustomerNumber);
        sg.finderHelper.setFinder("btnFinderCustomerNumber", sg.finder.ARCustomerFinder, finderSuccess.customerFinderSuccess, $.noop, title, finderFilters.getCustomerNumberFilter);
        sg.finderHelper.setFinder("btnFinderToCustomerNumber", sg.finder.ARCustomerFinder, finderSuccess.toCustomerFinderSuccess, $.noop, title, finderFilters.getToCustomerNumberFilter);
    },
    initNationAccountNumberFinder: function () {
        var title = $.validator.format(InquiryResources.FinderTitle, InquiryResources.NationalAccountNumber);
        sg.finderHelper.setFinder("btnFinderNationalAccountNumber", sg.finder.NationalAccounts, finderSuccess.nationalAccountFinderSuccess, $.noop, title, finderFilters.getNationalAccountNumberFilter);
        sg.finderHelper.setFinder("btnFinderToNationalAccountNumber", sg.finder.NationalAccounts, finderSuccess.toNationalAccountFinderSuccess, $.noop, title, finderFilters.getToNationalAccountNumberFilter);
    }
};

var finderFilters = {
    getCustomerNumberFilter: function () {
        var filters = [[]];
        var customerNumberFilter = $("#customerNumber").val().toUpperCase();
        filters[0][0] = sg.finderHelper.createFilter("CustomerNumber", sg.finderOperator.StartsWith, customerNumberFilter);
        return filters;
    },

    getToCustomerNumberFilter: function () {
        var filters = [[]];
        var customerNumberFilter = $("#toCustomerNumber").val().toUpperCase();
        filters[0][0] = sg.finderHelper.createFilter("CustomerNumber", sg.finderOperator.StartsWith, customerNumberFilter);
        return filters;
    },

    getNationalAccountNumberFilter: function () {
        var filters = [[]];
        var nationalAccountNumberFilter = $("#nationalAccountNumber").val().toUpperCase();
        filters[0][0] = sg.finderHelper.createFilter("NationalAccountNumber", sg.finderOperator.StartsWith, nationalAccountNumberFilter);
        return filters;
    },

    getToNationalAccountNumberFilter: function () {
        var filters = [[]];
        var nationalAccountNumberFilter = $("#toNationalAccountNumber").val().toUpperCase();
        filters[0][0] = sg.finderHelper.createFilter("NationalAccountNumber", sg.finderOperator.StartsWith, nationalAccountNumberFilter);
        return filters;
    }
};

var finderSuccess = {
    customerFinderSuccess: function(result) {
        if (result != null) {
            $("#customerNumber").val(result.CustomerNumber);
        }
    },
    toCustomerFinderSuccess: function(result) {
        if (result != null) {
            $("#toCustomerNumber").val(result.CustomerNumber);
        }
    },
    nationalAccountFinderSuccess: function(result) {
        if (result != null) {
            $("#nationalAccountNumber").val(result.NationalAccountNumber);
        }
    },
    toNationalAccountFinderSuccess: function(result) {
        if (result != null) {
            $("#toNationalAccountNumber").val(result.NationalAccountNumber);
        }
    }
};

$(function() {
    $(".accordion .panel-heading").click(function() {
        $(".panel-collapse").slideToggle("slow");
        $(".accordion a").toggleClass('collapsed');
    });

    if (InquiryViewModel) {
        inquiryUI.init();
    }
    //$(window).bind('beforeunload', function () {
    //    if (globalResource.AllowPageUnloadEvent && accountSetUI.accountSetModel.isModelDirty.isDirty()) {
    //        return jQuery('<div />').html(jQuery.validator.format(globalResource.SaveConfirm2, accountSetResources.AccountSetTitle)).text();


    //    }
    //});
});
