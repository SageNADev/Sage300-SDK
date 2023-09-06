/* Copyright (c) 2019-2021 Sage Software, Inc.  All rights reserved. */
"use strict";

var ViewFinderGridHelper = {
    finderOptions: {},
    fields: null,
    columns: null,
    viewModel: null,
    finderModel: null,
    isGridInitialized: false,
    columnFilter: {},

    CreateSchemaModelFields: function (model) {
        var localItems = {};
        $.each(model, function (index, value) {
            localItems[value.field] = { type: value.type };
        });
        return localItems;
    },

    init: function (finderOptions) {
        this.finderOptions = finderOptions;
        this.isGridInitialized = false;
        this.fields = this.CreateSchemaModelFields(finderSchemaModelType);
        this.columns = finderModelColumnsList;
        this.InitializeFinderGrid();
        this.InitColumnGrid();
        this.HideFilterControls();
        this.ButtonClickEvent();
        this.OnBlur();
    },
    HideFilterControls: function () {

        $(".clsValueDropDown").hide();
        $("#OperatorDropdown").attr("disabled", true);
        $('#OperatorDropdown').kendoDropDownList({}).data("kendoDropDownList");
        $("#ValueTextBox").val("");
        $("#ValueTextBox").attr("disabled", true);
        $("#ValueDropDown").hide();
        $("#btnSearch").attr("disabled", true);
        $("#NumericTextBox").hide();
        $("#NumericTextBoxDiv").hide();
    },
    InitiateFilterDataToPost: function () {
        this.columnFilter = null;
    },
    
    InitializeFinderGrid: function () {
        this.InitiateFilterDataToPost();
        this.finderModel = finderModelDetail;
        sg.keys = this.GetKeyFieldObject();

        //initialize grid only with preferences columns Defect D-13975
        var gridColumns = $.grep(this.columns, function (a) {
            return a.FinderDisplayType !== sg.FinderDisplayType.Filter;
        });
        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            serverFiltering: true,
            pageSize: sg.viewFinderHelper.pageSize,
            transport: {
                read: function (options) {
                    if (ViewFinderGridHelper.finderModel) {
                        ViewFinderGridHelper.ShowInitializedGrid(options);
                    }
                }
            },
            data: ViewFinderGridHelper.finderModel,
            schema: {
                model: {
                    fields: ViewFinderGridHelper.fields
                },
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
            reorderable: false,
            pageable: false,
            change: ViewFinderGridHelper.EnabledDialogButton
            });
    },
    ShowInitializedGrid: function (options) {
        options.success({ data: this.finderModel });

        let grid = $('#div_finder_grid').data("kendoGrid");
        let noMoreData = $("#last").attr('noMoreData');

        // If there is no more data, select the last row
        if (noMoreData) {
            grid.select("tr:last");
        } else {
            grid.select("tr:eq(0)");
        }

        this.finderModel = null;
        this.isGridInitialized = true;
    },
    EnabledDialogButton: function () {
        $('.selectKendoGrid').attr("disabled", false).removeClass("btnStyle2Disabled");
    },

    ReloadFinderGrid: function () {
        let grid = $("#div_finder_grid").data("kendoGrid");
        if (ViewFinderGridHelper.finderOptions) {
            ViewFinderGridHelper.finderOptions.record = null;
            ViewFinderGridHelper.finderOptions.isBackward = false;

            ViewFinderGridHelper.RefreshFinderGrid(grid);
        }
    },

    RefreshFinderGrid: function (grid) {
        var data = { finderOptions: this.finderOptions };
        data.finderOptions.ColumnFilter = this.columnFilter;

        let selectRow = grid.select();
        const rowIndex = selectRow.index();

        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ViewFinder", "RefreshGrid"), data, function (successData) {
            grid.dataSource.data(successData.Data);

            let pageInfo = successData.PageInfo;

            sg.controls.enableDisable("#first", pageInfo.IsFirstPage);
            sg.controls.enableDisable("#previous", pageInfo.IsFirstPage);
            sg.controls.enableDisable("#next", pageInfo.IsLastPage);
            sg.controls.enableDisable("#last", pageInfo.IsLastPage);

            if (pageInfo.IsPrevPageFull) {
                $("#previous").removeAttr("toFirst");
            } else {
                $("#previous").attr("toFirst", "toFirst");
            }

            grid.select("tr:eq(" + rowIndex+ ")");
            selectRow = grid.select();
            if (selectRow.length === 0) {
                grid.select("tr:eq(0)");
            }
        });
    },

    //This will set column, operator and values as per parent screen
    InitFinderValues: function (filter) {
       
        this.columnFilter = filter;
        var field = null;
        if (filter!= null && filter.Field != null) {
            field = this.GetFieldObject(filter.Field.field);
        }

        $("#btnSearch").attr("disabled", false);
        var columnDropdown = $("#ColumnDropdown").data("kendoDropDownList");
        if (field == null) {
            columnDropdown.value("ShowAllRecords");
            $('#OperatorDropdown').kendoDropDownList({}).data("kendoDropDownList");
            this.InitOperatorDropdown(field, "#OperatorDropdown");
            this.HideFilterControls();
            $("#ValueTextBox").show();
        }
        else if (field.PresentationList == undefined && field.dataType !== sg.finderDataType.Boolean) {
            //TextBox Scenario - select the column, operator, and pur the initial value in textbox
            columnDropdown.value(filter.Field.field);
            this.InitOperatorDropdown(field, "#OperatorDropdown");
            $("#OperatorDropdown").data("kendoDropDownList").value(filter.Operator);

            if (field.dataType === sg.finderDataType.Amount || field.dataType === sg.finderDataType.Number || field.dataType === sg.finderDataType.Integer || field.dataType === sg.finderDataType.SmallInteger) {
                $("#NumericTextBox").val(filter.Value);
                //This needs to be reviewed
                this.AddCustomAttributeToTextBox(field, "#ValueTextBox");
                $('#NumericTextBox').data('kendoNumericTextBox').value(filter.Value);
            } else {
                this.AddCustomAttributeToTextBox(field, "#ValueTextBox");
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
            // In the case of boolean and presentation lists, the numeric text box should be hidden
            // and the value text box should be disabled
            columnDropdown.value(filter.Field.field);
            this.InitOperatorDropdown(field, "#OperatorDropdown");
            $("#OperatorDropdown").data("kendoDropDownList").value(filter.Value);
            $("#ValueTextBox").val("");
            $("#ValueTextBox").show();
            $("#NumericTextBox").val("0");
        }
        //set default select value for drop done list
        if (!columnDropdown.text()) {
            columnDropdown.select(0);
        }
    },
    InitColumnGrid: function () {
        var dropdownDatasource = $.grep(this.columns, function (gridField) { return !gridField.IgnorePreferences && gridField.FinderDisplayType !== sg.FinderDisplayType.Grid; });
        dropdownDatasource = [{ title: ShowAllRecords, field: "ShowAllRecords" }].concat(dropdownDatasource);
        $("#ColumnDropdown").kendoDropDownList({
            autoBind: false,
            dataTextField: "title",
            dataValueField: "field",
            dataSource: dropdownDatasource,
            change: this.OnChange,
        });
    },
    OnChange: function (data) {
        var dropdownlist = $("#ColumnDropdown").data("kendoDropDownList");
        var selectedValue = dropdownlist.value();
        var selectedText = dropdownlist.text();

        if (selectedText == ShowAllRecords) {
            ViewFinderGridHelper.HideFilterControls();
            // remove column filter
            ViewFinderGridHelper.InitiateFilterDataToPost();

            ViewFinderGridHelper.finderOptions.SavePreferenceType = sg.viewFinderHelper.savePreferenceType.Filter;
            ViewFinderGridHelper.ReloadFinderGrid();
            ViewFinderGridHelper.finderOptions.SavePreferenceType = sg.viewFinderHelper.savePreferenceType.None;
            $("#ValueTextBox").show();
        } else if (selectedValue.length > 0) {
            var field = ViewFinderGridHelper.GetFieldObject(selectedValue);
            ViewFinderGridHelper.InitOperatorDropdown(field, "#OperatorDropdown");
            if (field.dataType !== sg.finderDataType.Boolean && (field.PresentationList == undefined && field.PresentationList == null)) {
                ViewFinderGridHelper.InitValueGridDropdownOrTextBox(field, "#ValueDropDown", "#ValueTextBox", ".clsValueDropDown");
            }
            $("#btnSearch").attr("disabled", false);
        } else {
            ViewFinderGridHelper.HideFilterControls();
            var emptyData = { Field: { field: "" }, Operator: "", Value: "" };
            ViewFinderGridHelper.simpleFilterData = emptyData;
            ViewFinderGridHelper.ReloadFinderGrid();
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
            if (field.PresentationList != undefined && field.PresentationList != null && field.PresentationList.length > 0) {
                operatorDatasource = field.PresentationList;
                $("#NumericTextBoxDiv").hide();
                $("#ValueTextBox").show();
                $("#ValueTextBox").val("");
                this.RemoveCustomAttributesInTextBox("#ValueTextBox");
                $("#ValueTextBox").attr('disabled', true);
            } else if (field.dataType === sg.finderDataType.Boolean) {
                operatorDatasource = [
                    { Text: globalResource.Yes, Value: true },
                    { Text: globalResource.No, Value: false }
                ];
                $("#NumericTextBoxDiv").hide();
                $("#ValueTextBox").show();
                $("#ValueTextBox").val("");
                this.RemoveCustomAttributesInTextBox("#ValueTextBox");
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
            this.EnableValueDropDown(true, valueDropdownId, valueTextboxId, valueDropdownClass);
            return true;
        }
        else {
            $(valueTextboxId).val("");
            this.EnableValueDropDown(false, valueDropdownId, valueTextboxId, valueDropdownClass);
            this.AddCustomAttributeToTextBox(field, valueTextboxId);
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
        this.RemoveCustomAttributesInTextBox(valueTextboxId);

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
            // Reference: https://docs.telerik.com/kendo-ui/knowledge-base/numerictextbox-error-on-initialization
            // When destroying a kendo numeric text box, the wrapper that contains the additional input element
            // will need to be removed before the reinitialization
            const origin = numericTextbox.element.show();
            origin.insertAfter(numericTextbox.wrapper);

            numericTextbox.destroy();
            numericTextbox.wrapper.remove();
        }

        //If phoneNumber attribute is defined
        var formatTextboxAttr = this.getValue($valueTextBox, 'formatTextbox', null);
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
            var maxLength = this.getValue($valueTextBox, 'maxlength', 16);
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
            this.RemoveCustomStyle();
        }
        else if (field.dataType === sg.finderDataType.Number) {
            $("#NumericTextBoxDiv").show();
            var decimalVal = this.getValue($valueTextBox, 'decimal', 0);
            var maxLength = this.getValue($valueTextBox, 'maxlength', 16);

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
            this.RemoveCustomStyle();
        }
        else if (field.dataType === sg.finderDataType.Integer) {
            var maxLength = this.getValue($valueTextBox, 'maxlength', 10);
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
            this.RemoveCustomStyle();
        }

        else if (field.dataType === sg.finderDataType.SmallInteger) {
            var maxLength = this.getValue($valueTextBox, 'maxlength', 5);
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
            this.RemoveCustomStyle();
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
        for (var columnItemIndex in this.columns) {
            var columnItem = this.columns[columnItemIndex];
            if (columnItem.field == item) {
                return columnItem;
            }
        }
        return null;
    },
    GetKeyFieldObject: function () {
        var keys = [];
        for (var columnItemIndex in this.columns) {
            var columnItem = this.columns[columnItemIndex];
            if (columnItem.IsKey) {
                keys.push(columnItem);
            }
        }
        return keys;
    },

    _getRecordKeyValues: function (grid, first) {
        let keyValues = {};
        let data = grid.dataSource.data()[first ? 0 : grid.dataSource.data().length - 1];
        if (data) {
            sg.keys.forEach(column => {
                let cellVal = data[column.field];
                let val;
                // check data type
                if (column.PresentationList !== null) {
                    var pval = $.grep(column.PresentationList, function (p) {
                        return p.Text === cellVal;
                    });

                    if (pval.length > 0) {
                        val = pval[0].Value;
                    }
                }
                else if (column.dataType === sg.finderDataType.Date) {
                    val = sg.utls.kndoUI.getFormattedDate(cellVal);
                }
                else {
                    val = cellVal;
                }

                keyValues[column.field] = val;
            });
        }

        return keyValues;
    },

    ExecuteSimpleFilter: function () {
        var dropdownlist = $("#ColumnDropdown").data("kendoDropDownList");
        var selectedValue = dropdownlist.value();
        var field = this.GetFieldObject(selectedValue);
        var value;
        if (field != null) {
            if (field.dataType === sg.finderDataType.Boolean || field.PresentationList != undefined && field.PresentationList != null && field.PresentationList.length > 0) {
                value = $("#OperatorDropdown").val();
                var data = { Field: field, Operator: sg.finderOperator.Equal, Value: value };
                this.columnFilter = data;
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
                    var txtValue = $("#NumericTextBox").val();
                    value = (txtValue) ? kendo.parseFloat(txtValue) : txtValue;
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
                    if (value.indexOf(':') > -1) {
                        var timeValue = value.replace(/:/g, '');
                        if (!isNaN(timeValue)) {
                            value = timeValue;
                        }
                    }
                }
                data = { Field: field, Operator: $("#OperatorDropdown").val(), Value: value };
                this.columnFilter = data;
            }

        } else {
            var emptyData = { Field: { field: "" }, Operator: "", Value: "" };
            this.columnFilter = emptyData;
        }
        ViewFinderGridHelper.finderOptions.SavePreferenceType = sg.viewFinderHelper.savePreferenceType.Filter;
        ViewFinderGridHelper.ReloadFinderGrid();
        ViewFinderGridHelper.finderOptions.SavePreferenceType = sg.viewFinderHelper.savePreferenceType.None;
    },
    ButtonClickEvent: function () {
        $("#btnSearch").click(function () {
            ViewFinderGridHelper.ExecuteSimpleFilter();
        });
        $("#first").click(function (e) {
            let isDisabled = $(this).attr('disabled');

            // Disable click functionality when the button has disabled attribute due to the button being an <a> tag
            if (!isDisabled) {
                ViewFinderGridHelper.ReloadFinderGrid();
            }

            // Prevent the finder from shifting down
            e.preventDefault();
        });
        $("#next").click(function (e) {
            let isDisabled = $(this).attr('disabled');

            // Disable click functionality when the button has disabled attribute due to the button being an <a> tag
            if (!isDisabled) {
                let grid = $("#div_finder_grid").data("kendoGrid");
                ViewFinderGridHelper.finderOptions.record = ViewFinderGridHelper._getRecordKeyValues(grid, false);
                ViewFinderGridHelper.finderOptions.isBackward = false;

                ViewFinderGridHelper.RefreshFinderGrid(grid);
            }

            // Prevent the finder from shifting down
            e.preventDefault();
        });
        $("#previous").click(function (e) {
            let isDisabled = $(this).attr('disabled');

            // Disable click functionality when the button has disabled attribute due to the button being an <a> tag
            if (!isDisabled) {
                if ($(this).attr('toFirst')) {
                    ViewFinderGridHelper.ReloadFinderGrid();
                } else {
                    let grid = $("#div_finder_grid").data("kendoGrid");
                    ViewFinderGridHelper.finderOptions.record = ViewFinderGridHelper._getRecordKeyValues(grid, true);
                    ViewFinderGridHelper.finderOptions.isBackward = true;

                    ViewFinderGridHelper.RefreshFinderGrid(grid);
                }
            }

            // Prevent the finder from shifting down
            e.preventDefault();
        });
        $("#last").click(function (e) {
            let isDisabled = $(this).attr('disabled');

            // Disable click functionality when the button has disabled attribute due to the button being an <a> tag
            if (!isDisabled) {
                let grid = $("#div_finder_grid").data("kendoGrid");
                ViewFinderGridHelper.finderOptions.record = null;
                ViewFinderGridHelper.finderOptions.isBackward = true;

                ViewFinderGridHelper.RefreshFinderGrid(grid);
            }

            // Prevent the finder from shifting down
            e.preventDefault();
        });
    },

    OnBlur: function () {
        $("#ValueTextBox").on('change keypress', function (e) {
            if (e.type == 'change' || (e.type == 'keypress' && e.which == 13)) {
                ViewFinderGridHelper.ExecuteSimpleFilter();
            }
        });
        $("#NumericTextBox").on('change keypress', function (e) {
            if (e.type == 'change' || (e.type == 'keypress' && e.which == 13)) {
                ViewFinderGridHelper.ExecuteSimpleFilter();
            }
        });
    },

    ExecuteFinder: function (finderOptions, callback) {
        const data = { finderOptions: finderOptions };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ViewFinder", "RefreshGrid"), data, successData => {
            if (callback && successData) {
                callback(successData);
            }
        });
    }
    
};