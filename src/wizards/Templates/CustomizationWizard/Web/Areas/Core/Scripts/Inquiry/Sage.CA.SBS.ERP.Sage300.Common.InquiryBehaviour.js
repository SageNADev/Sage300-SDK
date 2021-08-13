/* Copyright (c) 2016-2021 Sage Software, Inc.  All rights reserved. */

"use strict";
var invoiceTypeUI = { Item: 0, Summary: 1, Item_JobRelated: 2, Item_Retainage: 3, Item_JobRelated_Retainage: 4, Summary_JobRelated: 5, Summary_Retainage: 6, Summary_JobRelated_Retainage: 7 };
var inquiryUI = inquiryUI || {};
inquiryUI = {
    module: sg.utls.url.getParameterByName("module", location.href),
    inquiryType: sg.utls.url.getParameterByName("inquiryType", location.href),
    value: sg.utls.url.getParameterByName("value", location.href),
    target: sg.utls.url.getParameterByName("target", location.href),
    currencyType: { Customer: 0, Functional: 1 },
    mainGrid: null,
    detailGrid: null,
    // ----- these varaibles will be switching when different invoice type is selected  --- 
    detailGridConfigFunctionalGridColumns: null,
    detailGridConfigCustomerGridColumns: null,
    detailGridConfigFunctionalCurrencyPrefKey: null,
    detailGridConfigCustomerCurrencyPrefKey: null,
    // ------------------------------------------------------------------------------------
    filterType: {
        Finder: 0,
        Enum: 1,
        MultiSelect: 2,
        Date: 3,
        Text: 4,  
    },
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
    detailGridPageSize: 5,
    inquiryKoBindingModel : {},
    init: function () {
        inquiryUI.mainGrid = new inquiryGrid(FirstGridConfigDataField);
        inquiryUI.mainGrid.self = inquiryUI.mainGrid; // able to get a reference for itself
        if (typeof DetailGridSummaryConfigDataField !== 'undefined') {
            inquiryUI.detailGrid = new inquiryGrid(DetailGridSummaryConfigDataField, DetailGridSummaryConfigGetParameter, inquiryUI.detailGridPageSize);
            inquiryUI.detailGrid.self = inquiryUI.detailGrid; // able to get a reference for itself
        }
        
        inquiryUI.initGrid();
        inquiryUI.inquiryKoBindingModel = ko.mapping.fromJS(InquiryViewModel, { /*'ignore': ["CustomerDocuments"]*/  });
        ko.applyBindings(inquiryUI.inquiryKoBindingModel);
        inquiryUI.initRenderedAttributes();
        inquiryUI.initStaticDropDownList();
        inquiryUI.initButton();
        inquiryUI.initUserPreference();
        inquiryUI.initGridKeyBinding();

        //handle FilterCollapse show/hide based on if the value and target are defined on page load
        if (inquiryUI.value && inquiryUI.value != 'undefined' && inquiryUI.value !== "null" && inquiryUI.target && inquiryUI.target != 'undefined') {
            $("#collapseOne").hide();

            // kick start the reading
            inquiryUI.mainGrid.gridInstance().dataSource.read();
        } else {
            $("#collapseOne").show();
        }
    },

    initRenderedAttributes: function () {
        for (var i = 0; i < InquiryFilterControlList.length; i++) 
        {
            var control = InquiryFilterControlList[i];

            for (var j = 0; j < control.renderedObjList.length; j++) 
            {
                var renderedObj = control.renderedObjList[j];
                switch(renderedObj.objectType) {
                    case "dropdown":
                        inquiryUI.initDropDownList(renderedObj.Id, control);
                        break;

                    case "fromtextbox":
                        if (renderedObj.Id.indexOf(inquiryUI.target) > -1 && inquiryUI.value !== "null") {
                            $("#" + renderedObj.Id).val(inquiryUI.value);
                        }
                        break;

                    case "fromfinder":
                        var textBox = $.grep(control.renderedObjList, function (obj) {
                            return obj.objectType == "fromtextbox"; 
                        })[0];
                        inquiryUI.initFinder(renderedObj.Id,
                            control.inquiryFilterControl.Module,
                            control.inquiryFilterControl.FinderName,
                            control.inquiryFilterControl.FinderField,
                            textBox.Id);
                        break;

                    case "tofinder":
                        var textBox = $.grep(control.renderedObjList, function (obj) {
                            return obj.objectType == "totextbox";
                        })[0];
                        inquiryUI.initFinder(renderedObj.Id,
                            control.inquiryFilterControl.Module,
                            control.inquiryFilterControl.FinderName,
                            control.inquiryFilterControl.FinderField,
                            textBox.Id);
                        break;

                    case "multiselect":
                        inquiryUI.initMultiSelect(renderedObj.Id);
                        break;
                }
            }
        }
    },

    initDropDownList: function (id, control) {
        sg.utls.kndoUI.dropDownList(id);
        var dropdown = $("#" + id).data("kendoDropDownList");
        //dropdown.select(1);
        var isFinderType = control.inquiryFilterControl.FilterType === inquiryUI.filterType.Finder;
        if (isFinderType) {
            dropdown.bind("change", function (e) {
                var controlRenderedObjList = control.renderedObjList;
                var toGroupId = $.grep(controlRenderedObjList, function (renderedObj) {
                    return renderedObj.objectType === "finderGroup";
                })[0].Id;

                var fromBoxId = $.grep(controlRenderedObjList, function (renderedObj) {
                    return renderedObj.objectType === "fromtextbox";
                })[0].Id;

                var toBoxId = $.grep(controlRenderedObjList, function (renderedObj) {
                    return renderedObj.objectType === "totextbox";
                })[0].Id;

                if (this.value() == inquiryUI.filterOperator.Between) {
                    $("#" + toGroupId).show();
                    $("#" + fromBoxId).attr('placeholder', 'First');
                    $("#" + toBoxId).attr('placeholder', 'Last');
                } else {
                    $("#" + toGroupId).hide();
                    $("#" + fromBoxId).removeAttr('placeholder');
                     $("#" + toBoxId).removeAttr('placeholder');
                }
            });
        }
    },

    initFinder: function (id, module, finderName, fieldName, textBoxId) {
        var onSuccess = function (result) {
            if (result) {
                $("#" + textBoxId).val(result[fieldName]);
            }
        };

        sg.viewFinderHelper.setViewFinderEx(id, textBoxId, sg.viewFinderProperties[module][finderName],
            onSuccess, $.noop);
    },

    initMultiSelect: function (id) {
        $("#" + id).kendoMultiSelect({
            autoClose: false,
            select: selectWithSelectAllLogics
        }).data("kendoMultiSelect");

        var ms = $("#" + id).data("kendoMultiSelect");
        var dataItem = ms.dataSource.view()[0];
        if (dataItem) {
            ms.value(dataItem.value);
        }

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

            //Prevent Kendo default select event
            e.preventDefault();
        }
    },

    refreshGrids: function () {
        // if there is data in first grid, refresh 2nd one (and detail), otherwise, clear the grid
        if (inquiryUI.mainGrid.gridInstance().dataSource.data().length !== 0) {

            // make sure the 2nd grid go back to first page
            if (typeof SecondGridConfig !== 'undefined') {
                inquirySecondGrid.gridInstance().dataSource.page(0);
            }
            
            // switch to the right invoice type columns
            inquiryUI.handleDetailGridColumnDisplay(
                inquiryUI.getCurrentValue("InvoiceType"),
                inquiryUI.getCurrentValue("JobRelated"),
                inquiryUI.getCurrentValue("HasRetainage"));

            if (inquiryUI.detailGrid) {
                // apply columns visibility base on currency selection
                inquiryUI.displayColumnsByCurrencyType(inquiryUI.detailGrid.gridInstance(), inquiryUI.detailGridConfigCustomerGridColumns, inquiryUI.detailGridConfigFunctionalGridColumns);

                inquiryUI.detailGrid.gridInstance().dataSource.page(0);
            }
        }
        else {
            if (typeof SecondGridConfig !== 'undefined') {
                inquirySecondGrid.gridInstance().dataSource.data([]);
            }
            if (inquiryUI.detailGrid) {
                inquiryUI.detailGrid.gridInstance().dataSource.data([]);
            }
        }
    },

    handleDetailGridColumnDisplay: function (invoiceType, jobRelated, hasRetainage) {
        if (invoiceType === InvoiceType.Item) {
            if (jobRelated === JobRelated.Yes) {
                if (hasRetainage === HasRetainage.Yes) {
                    if (typeof DetailGridItem_JobRelated_RetainageConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Item_JobRelated_Retainage);
                        inquiryUI.handleColumnSwitch(DetailGridItem_JobRelated_RetainageConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                } else {
                    if (typeof DetailGridItem_JobRelatedConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Item_JobRelated);
                        inquiryUI.handleColumnSwitch(DetailGridItem_JobRelatedConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                }
            } else {
                if (hasRetainage === HasRetainage.Yes) {
                    if (typeof DetailGridItem_RetainageConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Item_Retainage);
                        inquiryUI.handleColumnSwitch(DetailGridItem_RetainageConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                } else {
                    if (typeof DetailGridItemConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Item);
                        inquiryUI.handleColumnSwitch(DetailGridItemConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                }
            }
        } else if (invoiceType === InvoiceType.Summary) {
            if (jobRelated === JobRelated.Yes) {
                if (hasRetainage === HasRetainage.Yes) {
                    if (typeof DetailGridSummary_JobRelated_RetainageConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Summary_JobRelated_Retainage);
                        inquiryUI.handleColumnSwitch(DetailGridSummary_JobRelated_RetainageConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                } else {
                    if (typeof DetailGridSummary_JobRelatedConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Summary_JobRelated);
                        inquiryUI.handleColumnSwitch(DetailGridSummary_JobRelatedConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                }
            } else {
                if (hasRetainage === HasRetainage.Yes) {
                    if (typeof DetailGridSummary_RetainageConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Summary_Retainage);
                        inquiryUI.handleColumnSwitch(DetailGridSummary_RetainageConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                } else {
                    if (typeof DetailGridSummaryConfig !== 'undefined') {
                        inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Summary);
                        inquiryUI.handleColumnSwitch(DetailGridSummaryConfig.columns, inquiryUI.detailGrid.gridInstance());
                    }
                }
            }
        }
    },

    handleColumnSwitch: function(newColumns, targetGrid)
    {
        var gridId = targetGrid.element.attr('id');
        var options = targetGrid.options;
        targetGrid.destroy();
        options.columns = newColumns;
        $("#" + gridId).empty().kendoGrid(options);
    },

    isFunctionalCurrency: function () {
        var dropdown = $("#currencyOptions").data("kendoDropDownList");
        if (dropdown) {
            return dropdown.value() == inquiryUI.currencyType.Functional;
        }
        return false;
    },

    getCurrentValue: function (fieldName) {
        var grid = inquiryUI.mainGrid.gridInstance();
        var selectedItem = grid.dataItem(grid.select());
        if (selectedItem) {
            return selectedItem[fieldName];
        } else {
            return "";
        }
    },

    initGridKeyBinding: function () {
        var arrows = [38, 40]; // upd and down arrow keys
        var grid = inquiryUI.mainGrid.gridInstance();
        grid.table.on("keydown", function (e) {
            if (arrows.indexOf(e.keyCode) >= 0) {
                grid.select(grid.current().closest("tr"));
            }
        })
    },
    initButton: function() {
        $("#btnApply").click(function () {
            var errorList = [];

            //Validate the between values
            function ValidateBetweenValues(fromValue, toValue, fieldName) {
                if (toValue) {
                    if (toValue.localeCompare(fromValue, undefined, { numeric: true }) < 0) {
                        return jQuery.validator.format(InquiryResources.RangeErrorFromValueGreater, fieldName);
                    }
                }
                return null;
            };

            var filteredObjects = $.grep(InquiryFilterControlList,
                function(elem) {
                    if (elem.inquiryFilterControl.FilterType == inquiryUI.filterType.Finder) {
                        var operator = $.grep(elem.renderedObjList,
                            function(e) {
                                return e.Id.indexOf("OperatorField") > -1;
                            })[0];
                        var operatorValue = $("#" + operator.Id).data("kendoDropDownList").value();
                        return operatorValue == inquiryUI.filterOperator.Between;
                    } else {
                        return false;
                    }
                });
            
            $.each(filteredObjects, function(i, obj) {
                var fromTextBox = $.grep(obj.renderedObjList, function(e) {
                    return e.objectType == "fromtextbox";
                })[0];
                var toTextBox = $.grep(obj.renderedObjList, function(e) {
                    return e.objectType == "totextbox";
                })[0];
                var error = ValidateBetweenValues($("#" + fromTextBox.Id).val(),
                    $("#" + toTextBox.Id).val(),
                    obj.inquiryFilterControl.Title);
                if (error) {
                    errorList.push(error);
                }
            });

            if (errorList.length > 0) {
                //Don't refresh grid if error occurs
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorList);
                return;
            }

            // Hide previous error messages
            $("#message").hide();

            // Refresh the grid after user clicking Apply
            inquiryUI.mainGrid.gridInstance().dataSource.page(0);
        })
    },

    initStaticDropDownList: function () {
        sg.utls.kndoUI.dropDownList("currencyOptions");
        var currencyDropdown = $("#currencyOptions").data("kendoDropDownList");
        if (currencyDropdown) {
            currencyDropdown.bind("change", function (e) {
                inquiryUI.displayColumnsByCurrencyType(inquiryUI.mainGrid.gridInstance(), FirstGridConfigCustomerGridColumns, FirstGridConfigFunctionalGridColumns);

                if (typeof DetailGridSummaryConfig !== 'undefined') {
                    inquiryUI.displayColumnsByCurrencyType(inquiryUI.detailGrid.gridInstance(), inquiryUI.detailGridConfigCustomerGridColumns, inquiryUI.detailGridConfigFunctionalGridColumns);
                }

                if (typeof SecondGridConfig !== 'undefined') {
                    inquiryUI.displayColumnsByCurrencyType(inquirySecondGrid.gridInstance(), inquirySecondGrid.custCurrencyColumns, inquirySecondGrid.funcCurrencyColumns);
                    inquiryUI.refreshGrids();
                }
            });
        }
    },
 
    initGrid: function () {
        FirstGridConfig.initGrid();
        inquiryUI.mainGrid.gridInstance = FirstGridConfig.getGridInstance;
        sg.utls.mergeGridConfiguration(["getParam", "buildGridData", "afterDataBind", "change", "self", "gridConfigDataField"],
             FirstGridConfig, inquiryUI.mainGrid);

        if (typeof SecondGridConfig !== 'undefined') {
            SecondGridConfig.initGrid();
            inquirySecondGrid.gridInstance = SecondGridConfig.getGridInstance;
            inquirySecondGrid.funcCurrencyColumns = SecondGridConfigFunctionalGridColumns
            inquirySecondGrid.custCurrencyColumns = SecondGridConfigCustomerGridColumns
            inquirySecondGrid.funcCurrencyPrefKey = SecondGridConfigFunctionalCurrencyPrefKey;
            inquirySecondGrid.custCurrencyPrefKey = SecondGridConfigCustomerCurrencyPrefKey;
            sg.utls.mergeGridConfiguration(["getParam", "buildGridData", "pageSize"],
                 SecondGridConfig, inquirySecondGrid);
            inquiryUI.mainGrid.afterDataBindHandler = inquiryUI.refreshGrids;
            inquiryUI.mainGrid.changeHandler = inquiryUI.refreshGrids;
        }

        // Base on InquiryUtilss.cs definition 
        if (typeof DetailGridSummaryConfig !== 'undefined') {
            DetailGridSummaryConfig.initGrid();

            inquiryUI.detailGrid.gridInstance = DetailGridSummaryConfig.getGridInstance;
            sg.utls.mergeGridConfiguration(["getParam", "buildGridData", "self", "gridConfigDataField", "pageSize"],
                 DetailGridSummaryConfig, inquiryUI.detailGrid);

            inquiryUI.setInvoiceTypeReference(invoiceTypeUI.Summary);
        }
    },

    //Initialise User Preference for Grids
    initUserPreference: function () {
        $('#btnEditGridColumns').on('click', function () {
            var preferenceKey = inquiryUI.isFunctionalCurrency()
                ? FirstGridConfigFunctionalCurrencyPrefKey
                : FirstGridConfigCustomerCurrencyPrefKey;
            GridPreferencesHelper.initialize("#" + inquiryUI.mainGrid.gridInstance().element[0].id, preferenceKey, $(this), FirstGridConfig.columns, 0);
        });

        inquiryUI.mainGrid.gridInstance().bind('dataBound', function () {
            InquiryGridHelper.appendDrillDownLink(inquiryUI.mainGrid.gridInstance(), inquiryUI.inquiryType, inquiryRazorView.isOEActive);
        });

        //hide/show columns depends on Currency Type
        inquiryUI.displayColumnsByCurrencyType(inquiryUI.mainGrid.gridInstance(), FirstGridConfigCustomerGridColumns, FirstGridConfigFunctionalGridColumns);

        if (typeof SecondGridConfig !== 'undefined') {
            $('#btnEditSecondGridColumns').on('click', function () {
                var preferenceKey = inquiryUI.isFunctionalCurrency()
                    ? inquirySecondGrid.funcCurrencyPrefKey
                    : inquirySecondGrid.custCurrencyPrefKey;
                GridPreferencesHelper.initialize("#" + inquirySecondGrid.gridInstance().element[0].id, preferenceKey, $(this), SecondGridConfig.columns, 0);
            });

            inquiryUI.displayColumnsByCurrencyType(inquirySecondGrid.gridInstance(), inquirySecondGrid.custCurrencyColumns, inquirySecondGrid.funcCurrencyColumns);
        }

        // Detail grid preference 
        if (typeof DetailGridSummaryConfig !== 'undefined') {
            $('#btnEditDetailSummaryGridColumns').on('click', function () {
                var preferenceKey = inquiryUI.isFunctionalCurrency()
                    ? inquiryUI.detailGridConfigFunctionalCurrencyPrefKey
                    : inquiryUI.detailGridConfigCustomerCurrencyPrefKey;
                GridPreferencesHelper.initialize("#" + inquiryUI.detailGrid.gridInstance().element[0].id, preferenceKey, $(this), DetailGridSummaryConfig.columns, 0);
            });

            inquiryUI.displayColumnsByCurrencyType(inquiryUI.detailGrid.gridInstance(), inquiryUI.detailGridConfigCustomerGridColumns, inquiryUI.detailGridConfigFunctionalGridColumns);
        }
    },

    // set all the necessary varaibles base on invoice type
    setInvoiceTypeReference: function (type) {
        if (type === invoiceTypeUI.Item) {
            if (typeof DetailGridItemConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridItemConfigFunctionalGridColumns; }
            if (typeof DetailGridItemConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridItemConfigCustomerGridColumns; }
            if (typeof DetailGridItemConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridItemConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridItemConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridItemConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Summary) {
            if (typeof DetailGridSummaryConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridSummaryConfigFunctionalGridColumns; }
            if (typeof DetailGridSummaryConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridSummaryConfigCustomerGridColumns; }
            if (typeof DetailGridSummaryConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridSummaryConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridSummaryConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridSummaryConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Item_JobRelated) {
            if (typeof DetailGridItem_JobRelatedConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridItem_JobRelatedConfigFunctionalGridColumns; }
            if (typeof DetailGridItem_JobRelatedConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridItem_JobRelatedConfigCustomerGridColumns; }
            if (typeof DetailGridItem_JobRelatedConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridItem_JobRelatedConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridItem_JobRelatedConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridItem_JobRelatedConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Item_Retainage) {
            if (typeof DetailGridItem_RetainageConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridItem_RetainageConfigFunctionalGridColumns; }
            if (typeof DetailGridItem_RetainageConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridItem_RetainageConfigCustomerGridColumns; }
            if (typeof DetailGridItem_RetainageConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridItem_RetainageConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridItem_RetainageConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridItem_RetainageConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Item_JobRelated_Retainage) {
            if (typeof DetailGridItem_JobRelated_RetainageConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridItem_JobRelated_RetainageConfigFunctionalGridColumns; }
            if (typeof DetailGridItem_JobRelated_RetainageConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridItem_JobRelated_RetainageConfigCustomerGridColumns; }
            if (typeof DetailGridItem_JobRelated_RetainageConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridItem_JobRelated_RetainageConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridItem_JobRelated_RetainageConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridItem_JobRelated_RetainageConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Summary_JobRelated) {
            if (typeof DetailGridSummary_JobRelatedConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridSummary_JobRelatedConfigFunctionalGridColumns; }
            if (typeof DetailGridSummary_JobRelatedConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridSummary_JobRelatedConfigCustomerGridColumns; }
            if (typeof DetailGridSummary_JobRelatedConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridSummary_JobRelatedConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridSummary_JobRelatedConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridSummary_JobRelatedConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Summary_Retainage) {
            if (typeof DetailGridSummary_RetainageConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridSummary_RetainageConfigFunctionalGridColumns; }
            if (typeof DetailGridSummary_RetainageConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridSummary_RetainageConfigCustomerGridColumns; }
            if (typeof DetailGridSummary_RetainageConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridSummary_RetainageConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridSummary_RetainageConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridSummary_RetainageConfigCustomerCurrencyPrefKey; }
        }
        else if (type === invoiceTypeUI.Summary_JobRelated_Retainage) {
            if (typeof DetailGridSummary_JobRelated_RetainageConfigFunctionalGridColumns !== 'undefined') { inquiryUI.detailGridConfigFunctionalGridColumns = DetailGridSummary_JobRelated_RetainageConfigFunctionalGridColumns; }
            if (typeof DetailGridSummary_JobRelated_RetainageConfigCustomerGridColumns !== 'undefined') { inquiryUI.detailGridConfigCustomerGridColumns = DetailGridSummary_JobRelated_RetainageConfigCustomerGridColumns; }
            if (typeof DetailGridSummary_JobRelated_RetainageConfigFunctionalCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigFunctionalCurrencyPrefKey = DetailGridSummary_JobRelated_RetainageConfigFunctionalCurrencyPrefKey; }
            if (typeof DetailGridSummary_JobRelated_RetainageConfigCustomerCurrencyPrefKey !== 'undefined') { inquiryUI.detailGridConfigCustomerCurrencyPrefKey = DetailGridSummary_JobRelated_RetainageConfigCustomerCurrencyPrefKey; }
        }
    },

    //Hide/show the grid columns depends on the currency type, and also applis the saved user preference
    displayColumnsByCurrencyType: function (grid, custColumns, funcColumns) {
        if (inquiryUI.isFunctionalCurrency()) {
            sg.utls.kndoUI.hideGridColumns(grid, custColumns);
            sg.utls.kndoUI.showGridColumns(grid, funcColumns);
        } else {
            sg.utls.kndoUI.hideGridColumns(grid, funcColumns);
            sg.utls.kndoUI.showGridColumns(grid, custColumns);
        }
        inquiryUI.setUserPreference(grid);
    },

    getDetailFilters: function (fieldName) {
        var value = inquiryUI.getCurrentValue(fieldName);
        return [ sg.finderHelper.createInquiryFilter(fieldName, inquiryUI.filterOperator.Equal, value, false) ];
    },

    generateStaticFilters: function() {
        var filters = [];
        var count = 0;
        
        for (var i = 0; i < InquiryFilterControlList.length; i++) {
            var control = InquiryFilterControlList[i];

            switch(control.inquiryFilterControl.FilterType) {
                case inquiryUI.filterType.Finder:
                {
                    var operatorField = $.grep(control.renderedObjList, function (obj) {
                        return obj.Id.indexOf("OperatorField") > -1;
                    })[0];

                    var fromTextBox = $.grep(control.renderedObjList, function (obj) {
                        return obj.objectType == "fromtextbox";
                    })[0];

                    var toTextBox = $.grep(control.renderedObjList, function (obj) {
                        return obj.objectType == "totextbox";
                    })[0];

                    var value_fromTextBox = $("#" + fromTextBox.Id).val().toUpperCase();
                    var operatorValue = $("#" + operatorField.Id).data("kendoDropDownList").value();

                    if (operatorValue == inquiryUI.filterOperator.Between) {
                        var value_toTextBox = $("#" + toTextBox.Id).val().toUpperCase();
                        if (value_fromTextBox) {
                            filters[count] = sg.finderHelper.createInquiryFilter(control.inquiryFilterControl.Field, inquiryUI.filterOperator.GreaterThanOrEqual, value_fromTextBox.toString(), false);
                            ++count;
                        }
                        if (value_toTextBox) {
                            filters[count] = sg.finderHelper.createInquiryFilter(control.inquiryFilterControl.Field, inquiryUI.filterOperator.LessThanOrEqual, value_toTextBox.toString(), false);
                            ++count;
                        }
                    } else {
                        if (value_fromTextBox) { // Any other operator than "between"
                            filters[count] = sg.finderHelper.createInquiryFilter(control.inquiryFilterControl.Field, operatorValue, value_fromTextBox.toString(), false);
                            ++count;
                        }
                    }
                }
                break;

                case inquiryUI.filterType.MultiSelect:
                {
                    var multiSelectObj = $.grep(control.renderedObjList, function (obj) {
                        return obj.Id.indexOf("multiselect") > -1;
                    })[0];

                    var selection;
                    //As of Kendo V2016.3.1118, $('#docTypeMultiSelect').val() doesn't always return the correct selections
                    //Issue: select multiple choices, unselect one of the choices by clicking x.  The val() will be incorrect.
                    var valueMultiSelect = $('#' + multiSelectObj.Id).data("kendoMultiSelect").value();
                    if (valueMultiSelect[0] === "-1") {
                        var allValues = $('#' + multiSelectObj.Id).data("kendoMultiSelect").dataSource.data();
                        selection = allValues.filter(function (elem) {
                                        return elem.value !== "-1";
                                    }).map(function (elem) {
                                        return elem.value;
                                    }).join();
                    } else {
                        selection = valueMultiSelect.join();
                    }
                    filters[count] = sg.finderHelper.createInquiryFilter(control.inquiryFilterControl.Field, inquiryUI.filterOperator.Include, selection, false);
                    ++count;
                }
                break;

                case inquiryUI.filterType.Enum:
                {
                    var dropDownSelect = $.grep(control.renderedObjList, function (obj) {
                        return obj.Id.indexOf("YesNo") > -1;
                    })[0];
                    var value_dropDownSelect = $("#" + dropDownSelect.Id).data("kendoDropDownList").value();
                    filters[count] = sg.finderHelper.createInquiryFilter("FullyPaid", inquiryUI.filterOperator.Equal, value_dropDownSelect, false);
                    ++count;
                }
            }                
        }

        return filters;
    },

    //Returns the grid preference key associated with the grid depending on the currency type
    getPreferenceKeyByGridId: function(gridId) {
        if (gridId === inquiryUI.mainGrid.gridInstance().element[0].id) {
            return inquiryUI.isFunctionalCurrency()
                ? FirstGridConfigFunctionalCurrencyPrefKey
                : FirstGridConfigCustomerCurrencyPrefKey;
        } else if (inquiryUI.detailGrid && gridId === inquiryUI.detailGrid.gridInstance().element[0].id) {
            return inquiryUI.isFunctionalCurrency()
                ? inquiryUI.detailGridConfigFunctionalCurrencyPrefKey
                : inquiryUI.detailGridConfigCustomerCurrencyPrefKey;
        } else {
            return inquiryUI.isFunctionalCurrency()
                ? inquirySecondGrid.funcCurrencyPrefKey
                : inquirySecondGrid.custCurrencyPrefKey;
        }
    },

    //Gets the saved grid user preference from the server and applis it to the grid
    setUserPreference: function (grid) {
        if (grid) {
            var gridId = grid.element[0].id;
            var key = inquiryUI.getPreferenceKeyByGridId(gridId);
            var url = sg.utls.url.buildUrl("Core", "Common", "GetGridPreferences");
            var data = { 'key': key };
            sg.utls.ajaxPost(url, data, function(result) {
                GridPreferencesHelper.setGrid("#" + gridId, result, true);
            });
        }
    }
};

$(function() {
    $(".accordion .panel-heading").click(function() {
        $(".panel-collapse").slideToggle("slow");
        $(".accordion a").toggleClass('collapsed');
    });

    if (InquiryViewModel && InquiryFilterControlList) {
        inquiryUI.init();
    }

});
