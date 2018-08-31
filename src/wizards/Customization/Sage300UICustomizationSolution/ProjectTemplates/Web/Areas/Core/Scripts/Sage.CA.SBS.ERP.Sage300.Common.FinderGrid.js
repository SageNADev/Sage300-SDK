/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */
"use strict";
var kendoHelper = {
    CreateSchemaModelFields: function (model) {
        var localItems = {};
        $.each(model, function (index, value) {
            localItems[value.field] = { type: value.type };
        });
        return localItems;
    }
};
var finderTypeEnum = {
    Simple: "Simple",
    Advanced: "Advanced",
    None: "None",
    Default: "Default"
};
var FinderGridHelper = {
    searchFinderModel: null,
    fields: null,
    columns: null,
    viewModel: null,
    finderModel: null,
    totalRecordsCount: 0,
    isGridInitialised: false,
    filterData: {},
    simpleFilterData: [[]],
    filterType: finderTypeEnum.Default,
    init: function () {
        FinderGridHelper.isGridInitialised = false;
        FinderGridHelper.filterType = finderTypeEnum.Simple;
        FinderGridHelper.searchFinderModel = searchFinder;
        FinderGridHelper.fields = kendoHelper.CreateSchemaModelFields(finderSchemaModelType);
        FinderGridHelper.columns = finderModelColumnsList;
        FinderGridHelper.InitialiseFinderGrid();
        FinderGridHelper.InitColumnGrid();
        FinderGridHelper.HideFilterControls();
        FinderGridHelper.InitFinderValues();
        FinderGridHelper.ButtonClickEvent();
        FinderGridHelper.OnBlur();
    },
    HideFilterControls: function () {

        $(".clsValueDropDown").hide();
        $("#OperatorDropdown").attr("disabled", true);
        $('#OperatorDropdown').kendoDropDownList({}).data("kendoDropDownList");
        $("#ValueTextBox").attr("disabled", true);
        $("#ValueDropDown").hide();
        $("#btnSearch").attr("disabled", true);
        $("#NumericTextBox").hide();
        $("#NumericTextBoxDiv").hide();
    },
    InitiateFilterDataToPost: function () {
        FinderGridHelper.simpleFilterData = { "Field": { "field": "", "title": "" }, "Operator": "", "Value": "" };
    },

    // determine whether the finder name is in hidePageNavigationFinderList array.
    HideFinderPageNavigation: function (finderName) {

        var index = $.inArray(finderName, sg.finder.hidePageNavigationFinderList);
        return (index >= 0);

    },
    InitialiseFinderGrid: function () {
        FinderGridHelper.InitiateFilterDataToPost();
        FinderGridHelper.finderModel = finderModelDetail;
        FinderGridHelper.totalRecordsCount = parseInt(finderTotalRecordsCount, 10) > 0 ? finderTotalRecordsCount : 0;
        sg.keys = FinderGridHelper.GetKeyFieldObject();

        //initialize grid only with preferences columns Defect D-13975
        var gridColumns = $.grep(FinderGridHelper.columns, function (a) {
            return a.FinderDisplayType !== sg.FinderDisplayType.Filter;
        });
        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            serverFiltering: true,
            pageSize: sg.finderHelper.pageSize,
            transport: {
                read: function (options) {
                    if (FinderGridHelper.totalRecordsCount != 0 && FinderGridHelper.finderModel) {
                        FinderGridHelper.ShowInitialisedGrid(options);
                    } else {
                        if (FinderGridHelper.isGridInitialised) {
                            FinderGridHelper.RefreshFinderGrid(options);
                        } else {
                            FinderGridHelper.ShowInitialisedGrid(options);
                        }
                    }
                }
            },
            data: FinderGridHelper.finderModel,
            schema: {
                model: {
                    fields: FinderGridHelper.fields
                },
                total: 'totalRecCount',
                data: 'data'
            }
        });

        $('#div_finder_grid').kendoGrid({
            dataSource: dataSource,
            columns: gridColumns,
            editable: false,
            navigatable: false,
            selectable: true,
            scrollable: true,
            resizable: true,
            reorderable: sg.utls.reorderable,
            pageable: {
                input: true,
                numeric: false,
                refresh: true
            },
            change: FinderGridHelper.EnabledDialogButton,
            columnReorder: function (e) {
                var grid = $('#div_finder_grid').data('kendoGrid');
                var data = { searchFinder: FinderGridHelper.searchFinderModel, fieldFrom: grid.columns[e.oldIndex].field, fieldTo: grid.columns[e.newIndex].field }
                sg.utls.ajaxPostHtml(sg.utls.url.buildUrl("Core", "Find", "ReorderUserPreferences"), data, function (finderPreferencesHTML) {
                    $("#tblTBodyFinderPref").html(finderPreferencesHTML);
                });
            }
        });
    },
    ShowInitialisedGrid: function (options) {
        sg.utls.kndoUI.selectGridRow();
        options.success({ data: FinderGridHelper.finderModel, totalRecCount: FinderGridHelper.totalRecordsCount });
        FinderGridHelper.totalRecordsCount = 0;
        FinderGridHelper.finderModel = null;
        FinderGridHelper.isGridInitialised = true;
    },
    EnabledDialogButton: function () {
        $('.selectKendoGrid').attr("disabled", false).removeClass("btnStyle2Disabled");
    },
    ConstructObjectForServer: function () {
        var kendoDataSource = $("#div_finder_grid").data("kendoGrid").dataSource;
        var objectForServer = new Object();
        objectForServer.PageSize = kendoDataSource.pageSize();
        objectForServer.PageNumber = kendoDataSource.page() - 1; // -1 since the page count starts from 0 on server side and 1 on client side.
        objectForServer.TotalResultsCount = kendoDataSource.options.total;
        objectForServer.SearchFinder = FinderGridHelper.searchFinderModel;
        objectForServer.SortDirection = false;
        return objectForServer;
    },
    RefreshFinderGrid: function (options) {
        var objectForServer = FinderGridHelper.ConstructObjectForServer();
        var simpleFilter;
        var advancedFilter;
        if (FinderGridHelper.filterType == finderTypeEnum.None) {
            advancedFilter = sg.mandatoryFilterData;
            simpleFilter = null;
        } else if (FinderGridHelper.filterType == finderTypeEnum.Default) {
            advancedFilter = sg.filterData;
            simpleFilter = null;
        } else {
            advancedFilter = sg.mandatoryFilterData;
            simpleFilter = FinderGridHelper.simpleFilterData;
        }
        sg.filterData[0][0] = FinderGridHelper.simpleFilterData;

        var finderOptions = {
            SearchFinder: objectForServer.SearchFinder,
            PageNumber: objectForServer.PageNumber,
            PageSize: objectForServer.PageSize,
            SortAsc: objectForServer.SortDirection,
            SimpleFilter: simpleFilter,
            AdvancedFilter: advancedFilter
        };
        //Required when post back happens when user click on Apply or Restore Table Default
        var data = { finderOptions: finderOptions };
        sg.finderOptions = finderOptions;
        sg.filterType = FinderGridHelper.filterType;
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Find", "RefreshGrid"), data, function (successData) {
            options.success({ data: successData.Data, totalRecCount: successData.TotalRecordCount });
        });
    },
    //This will set column, operator and values as per parent screen
    InitFinderValues: function () {
        var filter = sg.filterData[0][0];
        //Only when postback is happend  when user click on Apply or Restore Table Default
        if (sg.isPreferencesPostback) {
            if (sg.filterType != null) {
                FinderGridHelper.filterType = sg.filterType;
                sg.filterType = null;
            }
            FinderGridHelper.simpleFilterData = filter;
        }
        var field = null;
        if (filter != null) {
            field = FinderGridHelper.GetFieldObject(filter.Field.field);
        }
        $("#btnSearch").attr("disabled", false);
        var columnDropdown = $("#ColumnDropdown").data("kendoDropDownList");
        if (field == null || FinderGridHelper.filterType == finderTypeEnum.None
            || (columnDropdown.options != null && columnDropdown.options.dataSource.length === 1 && columnDropdown.options.dataSource[0].field === "")) { //Show All record is selected
            columnDropdown.value("ShowAllRecords");
            $('#OperatorDropdown').kendoDropDownList({}).data("kendoDropDownList");
            FinderGridHelper.simpleFilterData = null;
            FinderGridHelper.InitOperatorDropdown(field, "#OperatorDropdown");
            $("#div_finder_grid").data("kendoGrid").dataSource.page(1);
            FinderGridHelper.filterType = finderTypeEnum.None;
            FinderGridHelper.HideFilterControls();
            $("#ValueTextBox").show();
        }
        else if (field.PresentationList == undefined) {
            FinderGridHelper.filterType = finderTypeEnum.Simple;
            FinderGridHelper.simpleFilterData = filter;
            //TextBox Scenario - select the column, operator, and pur the initial value in textbox
            columnDropdown.value(filter.Field.field);
            FinderGridHelper.InitOperatorDropdown(field, "#OperatorDropdown");
            $("#OperatorDropdown").data("kendoDropDownList").value(filter.Operator);

            if (field.dataType === sg.finderDataType.Amount || field.dataType === sg.finderDataType.Number || field.dataType === sg.finderDataType.Integer || field.dataType === sg.finderDataType.SmallInteger) {
                $("#NumericTextBox").val(filter.Value);
                //This needs to be reviewed
                FinderGridHelper.AddCustomAttributeToTextBox(field, "#ValueTextBox");
                $('#NumericTextBox').data('kendoNumericTextBox').value(filter.Value);
            } else {
                FinderGridHelper.AddCustomAttributeToTextBox(field, "#ValueTextBox");
                if (field.dataType === sg.finderDataType.Date) {
                    $("#ValueTextBox").attr("disabled", false);
                    $("#ValueTextBox").show();
                    $('#ValueTextBox').data('kendoDatePicker').value(filter.Value);
                } else if (field.dataType === sg.finderDataType.TIME) {
                    $("#ValueTextBox").attr("disabled", false);
                    $("#ValueTextBox").show();
                    $("#ValueTextBox").val(sg.utls.checkIfValidTimeFormat(filter.Value));
                } else {
                    $("#ValueTextBox").show();
                    $("#ValueTextBox").val(filter.Value);
                }
            }
        } else {
            //Dropdown Scenario
            columnDropdown.value(filter.Field.field);
            FinderGridHelper.InitOperatorDropdown(field, "#OperatorDropdown");
            $("#OperatorDropdown").data("kendoDropDownList").value(filter.Value);
            $("#ValueTextBox").val("");
            $("#ValueTextBox").show();
            $("#NumericTextBox").val("0");
        }
        //set default select value for drop done list
        if (!columnDropdown.text()) {
            columnDropdown.select(0);
        }
        var operatorDropdown = $("#OperatorDropdown").data("kendoDropDownList");
        if (!operatorDropdown.text()) {
            operatorDropdown.select(0);
        }

    },
    InitColumnGrid: function () {
        var dropdownDatasource = $.grep(FinderGridHelper.columns, function (gridField) { return !gridField.IgnorePreferences && gridField.FinderDisplayType !== sg.FinderDisplayType.Grid; });
        dropdownDatasource = [{ title: ShowAllRecords, field: "ShowAllRecords" }].concat(dropdownDatasource);
        $("#ColumnDropdown").kendoDropDownList({
            autoBind: false,
            dataTextField: "title",
            dataValueField: "field",
            dataSource: dropdownDatasource,
            change: FinderGridHelper.OnChange,
        });
    },
    OnChange: function (data) {
        FinderGridHelper.filterType = finderTypeEnum.Simple;
        var dropdownlist = $("#ColumnDropdown").data("kendoDropDownList");
        var selectedValue = dropdownlist.value();
        var selectedText = dropdownlist.text();

        if (selectedText == ShowAllRecords) {
            FinderGridHelper.HideFilterControls();
            FinderGridHelper.filterType = finderTypeEnum.None;
            $("#div_finder_grid").data("kendoGrid").dataSource.page(1);
            $("#ValueTextBox").show();
        } else if (selectedValue.length > 0) {

            var field = FinderGridHelper.GetFieldObject(selectedValue);
            FinderGridHelper.InitOperatorDropdown(field, "#OperatorDropdown");
            if ((field.PresentationList == undefined && field.PresentationList == null)) {
                FinderGridHelper.InitValueGridDropdownOrTextBox(field, "#ValueDropDown", "#ValueTextBox", ".clsValueDropDown");
            }
            $("#btnSearch").attr("disabled", false);
        } else {
            FinderGridHelper.HideFilterControls();
            var emptyData = { Field: { field: "" }, Operator: "", Value: "" };
            FinderGridHelper.simpleFilterData = emptyData;
            $("#div_finder_grid").data("kendoGrid").dataSource.page(1);
            $("#ValueTextBox").show();
        }
        dropdownlist.focus();
    },
    InitOperatorDropdown: function (field, operatorDiv) {
        var operatorDatasource;
        var $operatorDropDown = $(operatorDiv);
        $operatorDropDown.show();
        $operatorDropDown.removeAttr('disabled');
        if (field != null) {
            if (field.dataType === sg.finderDataType.Boolean || (field.PresentationList != undefined && field.PresentationList != null && field.PresentationList.length > 0)) {
                operatorDatasource = field.PresentationList;
                $("#NumericTextBoxDiv").hide();
                $("#ValueTextBox").show();
                $("#ValueTextBox").val("");
                FinderGridHelper.RemoveCustomAttributesInTextBox("#ValueTextBox");
                $("#ValueTextBox").attr('disabled', true);
            } else if (field.dataType === sg.finderDataType.Date || field.dataType === sg.finderDataType.Integer || field.dataType === sg.finderDataType.Decimal || field.dataType === sg.finderDataType.Amount || field.dataType === sg.finderDataType.Number || field.dataType === sg.finderDataType.Time || field.dataType === sg.finderDataType.SmallInteger) {
                operatorDatasource = [
                    { Text: globalResource.Equal, Value: sg.finderOperator.Equal },
                    { Text: globalResource.GreaterThan, Value: sg.finderOperator.GreaterThan },
                    { Text: globalResource.GreaterThanOrEqual, Value: sg.finderOperator.GreaterThanOrEqual },
                    { Text: globalResource.LessThan, Value: sg.finderOperator.LessThan },
                    { Text: globalResource.LessThanOrEqual, Value: sg.finderOperator.LessThanOrEqual },
                    { Text: globalResource.NotEqual, Value: sg.finderOperator.NotEqual }
                ];

            }
        }
        if (operatorDatasource == undefined) {
            operatorDatasource = [
                { Text: globalResource.StartsWith, Value: sg.finderOperator.StartsWith },
                { Text: globalResource.Contains, Value: sg.finderOperator.Contains }
            ];
        }

        $operatorDropDown.kendoDropDownList({
            autoBind: true,
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: operatorDatasource
        });
        $operatorDropDown.data('kendoDropDownList').select(0);
    },
    InitValueGridDropdownOrTextBox: function (field, valueDropdownId, valueTextboxId, valueDropdownClass) {
        if (field.PresentationList != undefined && field.PresentationList != null && field.PresentationList.length > 0) {
            var $valueDropDown = $(valueDropdownId);
            $valueDropDown.kendoDropDownList({
                autoBind: false,
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: field.PresentationList
            });
            $valueDropDown.data('kendoDropDownList').value(1);
            FinderGridHelper.EnableValueDropDown(true, valueDropdownId, valueTextboxId, valueDropdownClass);
            return true;
        }
        else {
            $(valueTextboxId).val("");
            FinderGridHelper.EnableValueDropDown(false, valueDropdownId, valueTextboxId, valueDropdownClass);
            FinderGridHelper.AddCustomAttributeToTextBox(field, valueTextboxId);
            return false;
        }
    },
    EnableValueDropDown: function (enable, valueDropdownId, valueTextboxId, valueDropdownClass) {
        var $valueDropDown = $(valueDropdownId);
        var $valueTextBox = $(valueTextboxId);
        $(valueDropdownClass).toggle(enable);
        $valueDropDown.toggle(false);
        $valueTextBox.toggle((!enable));
        $("#btnSearch").attr('disabled', false);
    },

    //Removes the DatePicker and the custom attributes for ValueTextBox
    RemoveCustomAttributesInTextBox: function (valueTextboxId) {
        var $valueTextBox = $(valueTextboxId);
        $valueTextBox.unbind("keydown, focusout");
        sg.utls.unmask(valueTextboxId);
        sg.utls.kndoUI.removeDatePicker(valueTextboxId.substring(1));
        var attributes = $valueTextBox[0].attributes;
        var count = attributes.length;
        var attributesString = "";
        for (var i = 0; i < count; i++) {
            if (attributes[i].name != null && attributes[i].name != "id" && attributes[i].name != "name" && attributes[i].name != "type") {
                attributesString = attributesString + " " + attributes[i].name;
            }
        }
        $valueTextBox.removeAttr(attributesString);
    },

    RemoveCustomStyle: function(){
        var element = $("#NumericTextBoxDiv").children()[0];
        if (element) {
            $(element).show();
        }
    },

    AddCustomAttributeToTextBox: function (field, valueTextboxId) {
        var $valueTextBox = $(valueTextboxId);
        var numericTextBox;
        FinderGridHelper.RemoveCustomAttributesInTextBox(valueTextboxId);

        //Add all the attributes to the textbox, which is specified
        if (field.customAttributes != null) {
            $.each(field.customAttributes, function (key, value) {
                $valueTextBox.attr(key, value);
            });
        }

        if (sg.utls.isMozillaFirefox() == true) {
            $valueTextBox.removeAttr('maxlength');
        }

        var numericTextbox = $("#NumericTextBox").data("kendoNumericTextBox");
        if (numericTextbox) {
            numericTextbox.destroy();
        }

        //If phoneNumber attribute is defined
        var formatTextboxAttr = FinderGridHelper.getValue($valueTextBox, 'formatTextbox', null);
        if (formatTextboxAttr == 'phoneNumber' && sg.utls.isPhoneNumberFormatRequired == true) {
            sg.utls.addPlaceHolder("#ValueTextBox", "(   )    -");
            sg.utls.addMaxLength("#ValueTextBox", "34");
            sg.utls.maskPhoneNo("#ValueTextBox");
        }

        $("#ValueTextBox").val("");
        $("#NumericTextBox").val("");
        $("#NumericTextBoxDiv").hide();
        $("#ValueTextBox").hide();
        if (field.dataType === sg.finderDataType.Date) {
            $("#ValueTextBox").attr("disabled", false);
            $("#ValueTextBox").show();
            sg.utls.kndoUI.datePicker(valueTextboxId.substring(1));
            $("#ValueTextBox").addClass("datepicker");
        } else if (field.dataType === sg.finderDataType.Time) {
            $("#ValueTextBox").attr("disabled", false);
            $("#ValueTextBox").show();
            sg.utls.maskTimeformat(valueTextboxId);
        } else if (field.dataType === sg.finderDataType.Amount) {
            $("#NumericTextBoxDiv").show();
            var maxLength = FinderGridHelper.getValue($valueTextBox, 'maxlength', 16);
            var decimalVal = 3;
            var minVal = sg.utls.getMinValue("9", decimalVal, maxLength, true);
            var maxValue = sg.utls.getMaxVale("9", decimalVal, maxLength, true);

            numericTextBox = $("#NumericTextBox").kendoNumericTextBox({
                format: "n3",
                step: 0,
                min: minVal, //Do not remove - Added to fix defect D-24894 
                max: maxValue, //Do not remove - Added to fix defect D-24894 
                decimals: decimalVal,
                spinners: false,
            });
            sg.utls.kndoUI.restrictDecimals(numericTextBox, decimalVal, maxLength - decimalVal);
            FinderGridHelper.RemoveCustomStyle();
        }
        else if (field.dataType === sg.finderDataType.Number) {
            $("#NumericTextBoxDiv").show();
            var decimalVal = FinderGridHelper.getValue($valueTextBox, 'decimal', 0);
            var maxLength = FinderGridHelper.getValue($valueTextBox, 'maxlength', 16);

            var minVal = sg.utls.getMinValue("9", decimalVal, maxLength, true);
            var maxValue = sg.utls.getMaxVale("9", decimalVal, maxLength, true);
            var formateVal = "n" + decimalVal;

            numericTextBox = $("#NumericTextBox").kendoNumericTextBox({
                step: 0,
                min: minVal,
                max: maxValue,
                decimals: decimalVal,
                format: formateVal,
                spinners: false
            });
            sg.utls.kndoUI.restrictDecimals(numericTextBox, decimalVal, maxLength - decimalVal);
            FinderGridHelper.RemoveCustomStyle();
        }
        else if (field.dataType === sg.finderDataType.Integer) {
            var maxLength = FinderGridHelper.getValue($valueTextBox, 'maxlength', 10);
            $("#NumericTextBoxDiv").show();
            numericTextBox = $("#NumericTextBox").kendoNumericTextBox({
                format: "#",
                step: 0,
                decimals: 0,
                min: -2147483647,
                max: 2147483647,
                spinners: false,
            });
            sg.utls.kndoUI.restrictDecimals(numericTextBox, 0, maxLength);
            FinderGridHelper.RemoveCustomStyle();
        }

        else if (field.dataType === sg.finderDataType.SmallInteger) {
            var maxLength = FinderGridHelper.getValue($valueTextBox, 'maxlength', 5);
            $("#NumericTextBoxDiv").show();
            numericTextBox = $("#NumericTextBox").kendoNumericTextBox({
                format: "#",
                step: 0,
                decimals: 0,
                min: -32768,
                max: 32767,
                spinners: false,
            });
            sg.utls.kndoUI.restrictDecimals(numericTextBox, 0, maxLength);
            FinderGridHelper.RemoveCustomStyle();
        }
        else {
            $("#ValueTextBox").show();
        }
    },
    getValue: function (control, attribute, defaultValue) {
        var attributeValue = control.attr(attribute);
        if (attributeValue != undefined) {
            return attributeValue;
        }
        return defaultValue;
    },
    GetFieldObject: function (item) {
        for (var columnItemIndex in FinderGridHelper.columns) {
            var columnItem = FinderGridHelper.columns[columnItemIndex];
            if (columnItem.field == item) {
                return columnItem;
            }
        }
        return null;
    },
    GetKeyFieldObject: function () {
        var keys = [];
        for (var columnItemIndex in FinderGridHelper.columns) {
            var columnItem = FinderGridHelper.columns[columnItemIndex];
            if (columnItem.IsKey) {
                keys.push(columnItem);
            }
        }
        return keys;
    },

    ExecuteSimpleFilter: function () {
        FinderGridHelper.filterType = finderTypeEnum.Simple;
        var dropdownlist = $("#ColumnDropdown").data("kendoDropDownList");
        var selectedValue = dropdownlist.value();
        var field = FinderGridHelper.GetFieldObject(selectedValue);
        var value;
        if (field != null) {
            if (field.PresentationList != undefined && field.PresentationList != null && field.PresentationList.length > 0) {
                value = $("#OperatorDropdown").val();
                var data = { Field: field, Operator: sg.finderOperator.Equal, Value: value };
                FinderGridHelper.simpleFilterData = data;
            } else {
                var dateInvalid = false;
                if (field.dataType === sg.finderDataType.Date) {
                    var date = $("#ValueTextBox").val();
                    if (date) {
                        var currentDate = sg.utls.kndoUI.checkForValidDate(date);
                        if (currentDate == null) {
                            dateInvalid = true;
                        } else {
                            $("#ValueTextBox").val(currentDate);
                        }
                    }
                }
                var valueTextbox = $("#ValueTextBox");
                var attr = $("#ValueTextBox").attr('prefixZero');
                var maxLength = $("#ValueTextBox").attr('maxLength');
                if (attr == "true") {
                    if (valueTextbox.val().length < maxLength) {
                        valueTextbox.val(sg.utls.kndoUI.getTextValue(maxLength - valueTextbox.val().length) + valueTextbox.val());
                    }
                }
                if (field.dataType === sg.finderDataType.Integer || field.dataType === sg.finderDataType.Amount || field.dataType == sg.finderDataType.Number || field.dataType === sg.finderDataType.SmallInteger) {
                    var numerictextbox = $("#NumericTextBox").data("kendoNumericTextBox");
                    value = numerictextbox.value();
                }
                else if (field.dataType === sg.finderDataType.Date) {
                    if (dateInvalid) {
                        //Default to the below date if value is invalid. This should result with no records.
                        value = sg.utls.kndoUI.checkForValidDate("1801-01-01");
                    } else {
                        value = ($("#ValueTextBox").val());
                    }
                }
                else if (valueTextbox.attr('formatTextbox') == 'phoneNumber' && sg.utls.isPhoneNumberFormatRequired == true) {
                    value = valueTextbox.cleanVal();
                }
                else if ($("#ValueTextBox").attr('class') == "txt-upper") {
                    value = valueTextbox.val().toUpperCase();
                } else {
                    value = valueTextbox.val();
                }
                var data = { Field: field, Operator: $("#OperatorDropdown").val(), Value: value };
                FinderGridHelper.simpleFilterData = data;
            }

        } else {
            var emptyData = { Field: { field: "" }, Operator: "", Value: "" };
            FinderGridHelper.simpleFilterData = emptyData;
        }
        $("#div_finder_grid").data("kendoGrid").dataSource.page(1);
    },
    ButtonClickEvent: function () {
        $("#btnSearch").click(function () {
            FinderGridHelper.ExecuteSimpleFilter();
        });
    },
    OnBlur: function () {
        $("#ValueTextBox").bind('change', function (e) {
            FinderGridHelper.ExecuteSimpleFilter();
        });
        $("#NumericTextBox").bind('change', function (e) {
            FinderGridHelper.ExecuteSimpleFilter();
        });
    },
};