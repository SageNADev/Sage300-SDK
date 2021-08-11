/* Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved. */

"use strict";
var optionalFieldEnum = optionalFieldEnum || {};
optionalFieldEnum.Type = {
    Text: 1,
    Amount: 100,
    Number: 6,
    Integer: 8,
    YesNo: 9,
    Date: 3,
    Time: 4
};

// gvg - 20191112
// Deprecate the enumeration above when we've located all usage instances
var optionalFieldTypeEnum = optionalFieldTypeEnum || {};
optionalFieldTypeEnum = {
    Text: 1,
    Amount: 100,
    Number: 6,
    Integer: 8,
    YesNo: 9,
    Date: 3,
    Time: 4
};

// Temporary added, because of there are several place to using location as a http request parameter. Remove it once confirm
var optionalFieldSourceEnum = optionalFieldSourceEnum || {};
optionalFieldSourceEnum = {
    OE: 1
};

var optionalFieldColumnName = {
    Delete: "Delete",
    IsDeleted: "IsDeleted",
    SerialNumber: "SerialNumber",
    OptionalField: "OptionalField",
    OptionalFieldDescription: "OptionalFieldDescription",
    ValueSet: "ValueSet",
    ValueSetString: "ValueSetString",
    DefaultValue: "DefaultValue",
    Value: "Value",
    DefaultValueDescription: "DefaultValueDescription",
    ValueDescription: "ValueDescription",
    Required: "Required",
    AutoInsert: "AutoInsert",
    IsNewLine: "IsNewLine",
    LineNumber: "LineNumber",
    Type: "Type",
    Settings: "Settings"
}

var optFldGridUtils = {
    getParamPaging: function (data, pageNumber, pageSize, newinsertIndex) {
        var model = ko.mapping.toJS(optionalFieldUIGrid.modelData);
        if (model && model.hasOwnProperty(optionalFieldUIGrid.modelName)) {
            var currentPageNumber = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid").dataSource.page();
            var optionalFieldData = model[optionalFieldUIGrid.modelName].Items;

            optionalFieldData = sg.utls.kndoUI.assignDisplayIndex(optionalFieldData, currentPageNumber, pageSize);
            model[optionalFieldUIGrid.modelName].Items = optionalFieldData;
            optionalFieldUIGrid.paramIndex = newinsertIndex;
        }
    },

    checkDuplicateRecord: function (dataSource, field, checkItem, row) {
        var count = 0;
        var errorMsg = $.validator.format(optionalFieldsResources.duplicateMessage, optionalFieldsResources.optionalFieldTitle, checkItem.toUpperCase());
        $("#message").empty();
        $.each(dataSource.data(), function (key, value) {
            if (dataSource.data()[key][field] == checkItem) {
                count += 1;
            }
        });
        optionalFieldUIGrid.retrivableCellOldVal = checkItem.toUpperCase();
        if (count > 1) {
            optionalFieldUIGrid.retrivableCellOldVal = null;
            if (optionalFieldUIGrid.isPopUp) {
                optionalFieldUIGrid.showMessage(errorMsg);
            } else {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
            }
            row.set("OptionalField", "");
            row.set("OptionalFieldDescription", "");
            row.set("ValueSet", "0");
            return;
        }
        if (dataSource.total() > 10 && optionalFieldUIGrid.isOptionalFieldExists != null) {
            ////temporary added, because of there are several place to using location as a http request parameter. Remove it once confirm. (original code in default) 
            switch (optionalFieldUIGrid.optFldSrcName) {
                case optionalFieldSourceEnum.OE:
                    optionalFieldUIGrid.isOptionalFieldExists(row);
                    break;
                default:
                    if (optionalFieldUIGrid.modelData.AccountNumber != undefined) {
                        var accountNumber = optionalFieldUIGrid.modelData.AccountNumber();
                        var data = { fieldName: field, fieldValue: checkItem, accountNumber: accountNumber };
                        optionalFieldUIGrid.isOptionalFieldExists(data);
                    } else if (optionalFieldUIGrid.modelData.Location != undefined) {
                        var location = optionalFieldUIGrid.modelData.Location();
                        optionalFieldUIGrid.isOptionalFieldExists(row.OptionalField, location);
                    } else {
                        optionalFieldUIGrid.isOptionalFieldExists(row);
                    }
            }
        }
        optionalFieldUIGrid.resetFocus(row, 'OptionalField');
    },

    setRowData: function (gridData, rowdata, prefix) {
        switch (parseInt(rowdata.TYPE, 10)) {
            case optionalFieldEnum.Type.Text:
                gridData.set(prefix + "TextValue", rowdata.DVIFTEXT);
                break;
            case optionalFieldEnum.Type.Integer:
                gridData.set(prefix + "IntegerValue", rowdata.DVIFLONG || 0);
                break;
            case optionalFieldEnum.Type.Number:
                gridData.set(prefix + "NumberValue", rowdata.DVIFNUM || 0);
                break;
            case optionalFieldEnum.Type.Date:
                //Because we do not have default date value for web screen 
                gridData.set(prefix + "DateValue", rowdata.DVIFDATE || null);
                gridData.set(prefix + "Value", rowdata.DVIFDATE || null);
                break;
            case optionalFieldEnum.Type.YesNo:
                gridData.set(prefix + "YesOrNoValue", rowdata.DVIFBOOL === "True" ? "1" : "0");
                break;
            case optionalFieldEnum.Type.Amount:
                gridData.set(prefix + "AmountValue", rowdata.DVIFMONEY || 0);
                break;
            case optionalFieldEnum.Type.Time:
                if (rowdata.DVIFTIME != null) {
                    gridData.set(prefix + "TimeValue", rowdata.DVIFTIME);
                    gridData.set(prefix + "Value", rowdata.DVIFTIME.substr(-8));
                } else {
                    gridData.set(prefix + "TimeValue", null);
                    gridData.set(prefix + "Value", null);
                }
                break;
        }
    },

    setServerRowData: function (gridData, rowdata, prefix) {
        switch (rowdata.Type) {
            case optionalFieldEnum.Type.Text:
                gridData.set(prefix + "TextValue", rowdata.DefaultTextValue);
                break;
            case optionalFieldEnum.Type.Integer:
                gridData.set(prefix + "IntegerValue", rowdata.DefaultIntegerValue || 0);
                break;
            case optionalFieldEnum.Type.Number:
                gridData.set(prefix + "NumberValue", rowdata.DefaultNumberValue || 0);
                break;
            case optionalFieldEnum.Type.Date:
                //Because we do not have default date value for web screen 
                gridData.set(prefix + "DateValue", null);
                gridData.set(prefix + "Value", null);
                break;
            case optionalFieldEnum.Type.YesNo:
                gridData.set(prefix + "YesOrNoValue", rowdata.DefaultYesOrNoValue);
                break;
            case optionalFieldEnum.Type.Amount:
                gridData.set(prefix + "AmountValue", rowdata.DefaultAmountValue || 0);
                break;
            case optionalFieldEnum.Type.Time:
                if (rowdata.DefaultValue != null) {
                    gridData.set(prefix + "TimeValue", rowdata.DefaultTimeValue);
                    gridData.set(prefix + "Value", rowdata.DefaultTimeValue.substr(-8));
                } else {
                    gridData.set(prefix + "TimeValue", null);
                    gridData.set(prefix + "Value", null);
                }
                break;
        }
    }
};

var gridColConfig = {
    optionalFieldFilterDataChanged: false,
    finderSelect: false,

    checkboxEditor: function (container, options) {
        optionalFieldUIGrid.serialNumber = (options.model.SerialNumber) ? options.model.SerialNumber : options.model.DisplayIndex;
        sg.utls.kndoUI.nonEditable($('#' + optionalFieldUIGrid.gridId).data("kendoGrid"), container);
    },

    optionalFieldEditor: function (container, options) {
        optionalFieldUIGrid.serialNumber = (options.model.SerialNumber) ? options.model.SerialNumber : options.model.DisplayIndex;
        if (options.model.IsNewLine) {
            var html = optionalFieldFields.txtOptionalField + optionalFieldFields.finderOptionalField;
            $(html).appendTo(container);
            optionalFieldUIGrid.optionalFieldFilterData = options.model.OptionalField || "";
            gridColConfig.optionalFieldFilterDataChanged = false;
            if (optionalFieldUIGrid.optionalFieldFilterName !== null && optionalFieldUIGrid.optionalFieldFilterName !== "") {
                optionalFieldUIGrid.optionalFieldFilterValue = options.model[optionalFieldUIGrid.optionalFieldFilterName];
            }
            optionalFieldUIGrid.optionalFieldLineId = options.model.uid;
            sg.viewFinderHelper.setViewFinderEx("optFieldFinder", "txtoptionalfield", optionalFieldUIGrid.finder,
                optionalFieldUIGrid.OnOptionalFieldSelection, optionalFieldUIGrid.optionalFieldCancel);
        } else {
            var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
            grid.closeCell();
            grid.select(container.closest("tr"));
        }

        $("#txtoptionalfield").bind('change', function (e) {
            var value = $("#txtoptionalfield").val();
            e.target.value = e.target.value.toUpperCase();
            optionalFieldUIGrid.IsSuccess = true;
            optionalFieldUIGrid.serialNumber = (options.model.SerialNumber) ? options.model.SerialNumber : options.model.DisplayIndex;
            optionalFieldUIGrid.optionalFieldFilterData = e.target.value;
            gridColConfig.optionalFieldFilterDataChanged = true;

            sg.delayOnChange("optFieldFinder", $("#txtoptionalfield"), function () {
                if (optionalFieldUIGrid.getOptionalFieldData != null) {
                    var location = 0;
                    ////temporary added, because of there are several place to using location as a http request parameter. Remove it once confirm. (original code in default) 

                    switch (optionalFieldUIGrid.optFldSrcName) {
                        case optionalFieldSourceEnum.OE:
                            optionalFieldUIGrid.getOptionalFieldData(options.model);
                            break;
                        default:
                            if (optionalFieldUIGrid.modelData.Location !== undefined) {
                                location = optionalFieldUIGrid.modelData.Location();
                                optionalFieldUIGrid.getOptionalFieldData(value.toUpperCase(), location, options.model);
                            } else if (optionalFieldUIGrid.modelData.AccountNumber !== undefined) {
                                location = (optionalFieldUIGrid.isValueSetEditable) ? 1 : 0;
                                optionalFieldUIGrid.getOptionalFieldData(value.toUpperCase(), location);
                            } else {
                                optionalFieldUIGrid.getOptionalFieldData(options.model);
                            }
                    }
                }
            });
        });
    },

    noEditor: function (container, options) {
        sg.utls.kndoUI.nonEditable($('#' + optionalFieldUIGrid.gridId).data("kendoGrid"), container);
    },

    getValidValue: function (container, options, passValue) {
        var optionalFieldValue = options.model.DefaultValue || options.model.Value || "";
        if (!optionalFieldUIGrid.isPopUp) {
            if (passValue) {
                var numericType = [optionalFieldEnum.Type.Integer, optionalFieldEnum.Type.Number, optionalFieldEnum.Type.Amount];
                if (numericType.indexOf(options.model.Type) > -1 && !optionalFieldValue) {
                    optionalFieldValue = 0;
                }
                optionalFieldUIGrid.getOptionalFieldValue(options.model.OptionalField, optionalFieldValue, options.model.Type);
            } else {
                optionalFieldUIGrid.getOptionalFieldValue(options.model.OptionalField);
            }
        } else {
            optionalFieldUIGrid.getOptionalFieldValue(options.model);
        }
    },

    valueSetEditor: function (container, options) {
        var gridId = optionalFieldUIGrid.gridId;
        if (!optionalFieldUIGrid.isValueSetEditable) {
            sg.utls.kndoUI.nonEditable($('#' + gridId).data("kendoGrid"), container);
        } else {
            optionalFieldUIGrid.serialNumber = (options.model.SerialNumber) ? options.model.SerialNumber : options.model.DisplayIndex;
            optionalFieldUIGrid.optionalFieldLineId = options.model.uid;
            var html = optionalFieldFieldsDropdown.optionalFieldValuesetDrpDown;
            $(html).attr("data-bind", "value:" + options.field)
                .appendTo(container).kendoDropDownList({
                    value: options.model.ValueSet,
                    change: function (e) {
                        if ($('#' + gridId)) {
                            var grid = $('#' + gridId).data("kendoGrid");
                            var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
                            var fldValue = (currentRowGrid.Value === undefined) ? "DefaultValue" : "Value";
                            var fldValueDesc = (currentRowGrid.ValueDescription === undefined) ? "DefaultValueDescription" : "ValueDescription";
                            var isGetValidValue = (optionalFieldUIGrid.modelData.Location !== undefined);
                            currentRowGrid.set("ValueSet", e.sender.selectedIndex);
                            var numericType = [optionalFieldEnum.Type.Integer, optionalFieldEnum.Type.Number, optionalFieldEnum.Type.Amount];
                            var isNumeric = (numericType.indexOf(options.model.Type) > -1) && e.sender.selectedIndex == 1;
                            var isDefaultTime = (options.model.Type == optionalFieldEnum.Type.Time) && e.sender.selectedIndex == 1;
                            if (currentRowGrid.Validate === 1 && e.sender.selectedIndex == 1 && isGetValidValue) {
                                if (optionalFieldUIGrid.getOptionalFieldValue !== null) {
                                    gridColConfig.getValidValue(container, options, false);
                                }
                            } else {
                                currentRowGrid.set(fldValue, isNumeric ? 0 : "");
                                if (isDefaultTime) {
                                    currentRowGrid.set(fldValue, "00:00:00");
                                }
                                currentRowGrid.set(fldValueDesc, "");
                            }
                            if (currentRowGrid.Validate === 0 && e.sender.selectedIndex == 1 && options.model.Type === optionalFieldEnum.Type.Amount) {
                                currentRowGrid.set(fldValue, "0.000");
                            }
                            if (options.model.Type == optionalFieldEnum.Type.YesNo) {
                                currentRowGrid.set(fldValue, e.sender.selectedIndex == 1 ? 0 : null);
                            }
                        }
                    }
                });
        }
    },

    valueEditor: function (container, options) {
        if (optionalFieldUIGrid.isReadOnly) {
            gridColConfig.noEditor(container, options);
            return;
        }
        var gridId = optionalFieldUIGrid.gridId;
        optionalFieldUIGrid.serialNumber = (options.model.SerialNumber) ? options.model.SerialNumber : options.model.DisplayIndex;
        optionalFieldUIGrid.LineNumber = options.model.LineNumber;
        optionalFieldUIGrid.optionalFieldFilterData = options.model.OptionalField || "";
        optionalFieldUIGrid.optionalFieldValueFilterData = (options.model.DefaultValue === undefined) ? options.model.Value : options.model.DefaultValue;
        optionalFieldUIGrid.optionalFieldLineId = options.model.uid;
        var grid = $('#' + gridId).data("kendoGrid");
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        var html = ((options.field == "DefaultValue") ? optionalFieldFields.txtDefaultValue : optionalFieldFields.txtValue) + optionalFieldFields.finderValue;

        if (options.model.OptionalField !== null && options.model.OptionalField !== "") {
            //Tmp fixed 
            optionalFieldUIGrid.pristineData.val = currentRowGrid.Value;
            optionalFieldUIGrid.pristineData.description = currentRowGrid.ValueDescription;
            if (options.model.Type == optionalFieldEnum.Type.Date) {
                $(html).appendTo(container).addClass("dp-and-finder");
                sg.utls.kndoUI.datePicker("txtOptFieldValue");
            } else if (options.model.Type === optionalFieldEnum.Type.Number || options.model.Type === optionalFieldEnum.Type.Integer || options.model.Type === optionalFieldEnum.Type.Amount) {
                gridColConfig.numericTextBox(container, options, html);
            } else if (options.model.Type === optionalFieldEnum.Type.Time) {
                $(html).appendTo(container);
                $("#txtOptFieldValue").bind("blur", function () {
                    if (currentRowGrid.Type !== optionalFieldEnum.Type.Time) {
                        return;
                    }
                    var timeValue = sg.utls.checkIfValidTimeFormat($("#txtOptFieldValue").val());
                    var fldValue = (currentRowGrid.Value === undefined) ? "DefaultValue" : "Value";
                    if (currentRowGrid[fldValue]) {
                        options.model[fldValue] = timeValue;
                        currentRowGrid.set("ValueSet", 1);
                        currentRowGrid.set("ValueSetString", 1);
                    }
                    optionalFieldUIGrid.updateValueInGrid(null, timeValue);
                    sg.delayOnChange("optFieldValueFinder", $("#txtOptFieldValue"), function () {
                        if (options.model.AllowBlank && (timeValue == null || timeValue.length == 0)) {
                            $("#message").empty();
                            return false;
                        }
                        if (options.model.ValueSet == 1 && !gridColConfig.finderSelect) {
                            gridColConfig.getValidValue(container, options, true);
                        }
                        gridColConfig.finderSelect = false;
                    });
                });
                sg.utls.maskTimeformat("#txtOptFieldValue");
            } else if (options.model.Type === optionalFieldEnum.Type.YesNo) {
                var val = $.trim(options.model[options.field]);
                options.model[options.field] = (val == "") ? "0" : val;
                html = optionalFieldFieldsDropdown['optionalField' + options.field + 'DrpDown'];
                $(html).attr("data-bind", "value:" + options.field).appendTo(container).kendoDropDownList({
                    value: $.trim(options.model[options.field]),
                    change: function (e) {
                        if ($('#' + gridId)) {
                            optionalFieldUIGrid.updateValueInGrid(options.model.OptionalField, e.sender.selectedIndex);
                        }
                    },
                    open: function (e) {
                        if (currentRowGrid) {
                            var fldValue = (currentRowGrid.Value === undefined) ? "DefaultValue" : "Value";
                            currentRowGrid.set("ValueSet", 1);
                            currentRowGrid.set("ValueSetString", 1);
                            if (currentRowGrid[fldValue] === null) {
                                currentRowGrid.set(fldValue, 0);
                            }
                        }
                    }
                });
            } else {
                $(html).appendTo(container);
                $("#txtOptFieldValue").addClass('pr25');
            }

            if (options.model.Type !== optionalFieldEnum.Type.Time) {
                $("#txtOptFieldValue").unmask();
                if (options.model.Type === optionalFieldEnum.Type.Text) {
                    $('#txtOptFieldValue').attr('maxlength', options.model.Length);
                }
            }

            if (options.model.Type == optionalFieldEnum.Type.Date) {
                $("#txtOptFieldValue").removeClass("pr25");
                $("#txtOptFieldValue").parents().removeClass("pr25");
            }

            const optionalFieldValueFinderInfo = () => {
                const optionalField = options.model;
                const selectedOptionalField = optionalField.OptionalField;
                let property = sg.utls.deepCopy(sg.viewFinderProperties.CS.OptionalFieldValue);
                let value = typeof optionalField.DefaultValue !== 'undefined' ? optionalField.DefaultValue : optionalField.Value;
                switch (optionalField.Type) {
                    case optionalFieldEnum.Type.Text:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.TextOptionalFieldValue);
                        break;
                    case optionalFieldEnum.Type.Amount:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.AmountOptionalFieldValue);
                        break;
                    case optionalFieldEnum.Type.Number:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.NumberOptionalFieldValue);
                        break;
                    case optionalFieldEnum.Type.Integer:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.IntegerOptionalFieldValue);
                        break;
                    case optionalFieldEnum.Type.Date:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.DateOptionalFieldValue);
                        value = sg.utls.kndoUI.getDateYYYMMDDFormat(value);
                        break;
                    case optionalFieldEnum.Type.Time:
                        property = sg.utls.deepCopy(sg.viewFinderProperties.CS.TimeOptionalFieldValue);
                        value = value.replaceAll(':', '');
                        break;
                }
                property.initKeyValues = [selectedOptionalField, value];
                property.filter = $.validator.format(property["filterTemplate"], selectedOptionalField);
                return property;
            };
            sg.viewFinderHelper.setViewFinder("optFieldValueFinder", optionalFieldUIGrid.OnOptionalFieldValueSelection,
                optionalFieldValueFinderInfo, optionalFieldUIGrid.optionalFieldValueCancel);

            if (options.model.Type !== optionalFieldEnum.Type.YesNo && options.model.Type !== optionalFieldEnum.Type.Time) {
                var numericType = [optionalFieldEnum.Type.Integer, optionalFieldEnum.Type.Number, optionalFieldEnum.Type.Amount];
                var editBox = (numericType.indexOf(options.model.Type) > -1) ? $("#txtOptFieldValue").data("kendoNumericTextBox") : $('#txtOptFieldValue');
                editBox.bind("change", function () {
                    optionalFieldUIGrid.optionalFieldValueFilterData = (typeof this.value === "string") ? this.value : this.value();
                    if (options.model.Type === optionalFieldEnum.Type.Date) {
                        optionalFieldUIGrid.optionalFieldValueFilterData = kendo.parseDate(optionalFieldUIGrid.optionalFieldValueFilterData, sg.utls.kndoUI.getDatePatterns());
                        if (isNaN(Date.parse(optionalFieldUIGrid.optionalFieldValueFilterData))) {
                            optionalFieldUIGrid.optionalFieldValueFilterData = null;
                        } else {
                            var year = new Date(optionalFieldUIGrid.optionalFieldValueFilterData).getFullYear();
                            if (year < 1900) {
                                optionalFieldUIGrid.optionalFieldValueFilterData = null;
                            }
                        }
                    }
                    optionalFieldUIGrid.updateValueInGrid(options.model.OptionalField, optionalFieldUIGrid.optionalFieldValueFilterData);
                    sg.delayOnChange("optFieldValueFinder", $("#txtOptFieldValue"), function () {
                        if (options.model.Validate && options.model.AllowBlank && !optionalFieldUIGrid.optionalFieldValueFilterData) {
                            if (currentRowGrid) {
                                currentRowGrid.set("ValueDescription", "");
                                currentRowGrid.set("DefaultValueDescription", "");
                            }
                            return false;
                        }
                        if (optionalFieldUIGrid.getOptionalFieldValue !== null) {
                            gridColConfig.getValidValue(container, options, true);
                        }
                    });
                });
            };
        } else {
            $('#' + gridId).data("kendoGrid").closeCell(container);
        }
    },

    dropdownListEditor: function (container, options) {
        if (optionalFieldUIGrid.isReadOnly) {
            gridColConfig.noEditor(container, options);
            return;
        }
        optionalFieldUIGrid.optionalFieldLineId = options.model.uid;
        var fldName = options.field;
        var html = optionalFieldFieldsDropdown['optionalField' + fldName + 'DrpDown'];
        $(html).attr("data-bind", "value:" + options.field)
            .appendTo(container).kendoDropDownList({
                value: options.model[fldName],
                change: function (e) {
                    var gridId = optionalFieldUIGrid.gridId;
                    var currentRowGrid = sg.utls.kndoUI.getSelectedRowData($('#' + gridId).data("kendoGrid"));
                    currentRowGrid.set(fldName, e.sender.selectedIndex);
                    if (fldName === "Required" && e.sender.selectedIndex == 1) {
                        currentRowGrid.set("AutoInsert", e.sender.selectedIndex);
                    }
                }
            });
    },

    settingsEditor: function (container, options) {
        if (optionalFieldUIGrid.settingsEditor) {
            optionalFieldUIGrid.settingsEditor(container, options);
        }
    },

    registerSettingsEvent: function () {
        $('.btnOptionalFieldSettings').click(function (e) {
            var rowIndex = parseInt($(this).attr('tag'));
            var grid = $('#OptionalFieldGrid').data("kendoGrid");
            var gridData = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "SerialNumber", rowIndex);
            var row = sg.utls.kndoUI.getRowForDataItem(gridData);
            if (optionalFieldUI) {
                optionalFieldUI.btnSettingClicked = true;
            }
            var colIndex = window.GridPreferencesHelper.getColumnIndex('#OptionalFieldGrid', "Settings");
            grid.editCell(row.find(">td:eq(" + colIndex + ")"));
        });
    },

    // Methods
    numericTextBox: function (container, options, html) {
        var minValue, maxValue, maxLength, formatValue, decimalValue;
        switch (options.model.Type) {
            case optionalFieldEnum.Type.Integer:
                decimalValue = 0;
                maxLength = 10;
                formatValue = "#";
                minValue = -2147483648;
                maxValue = 2147483647;
                break;
            case optionalFieldEnum.Type.Number:
                decimalValue = parseInt(options.model.Decimals);
                maxLength = (16) - (decimalValue);
                formatValue = "n" + options.model.Decimals;
                minValue = sg.utls.getMinValue("9", decimalValue, 16, true);
                maxValue = sg.utls.getMaxValue("9", decimalValue, 16, true);
                break;
            case optionalFieldEnum.Type.Amount:
                decimalValue = 3;
                maxLength = 13;
                formatValue = "n3";
                minValue = sg.utls.getMinValue("9", 3, 16, true);
                maxValue = sg.utls.getMaxValue("9", 3, 16, true);
                break;
        }
        $(html).appendTo(container);
        var numericTxtBox = $("#txtOptFieldValue").kendoNumericTextBox({
            decimals: decimalValue,
            format: formatValue,
            spinners: false,
            step: 0,
            min: minValue,
            max: maxValue
        });

        var $inputControl = $("#txtOptFieldValue");
        $inputControl.addClass('grid_inpt align-right pr25');

        // Add alignment to parent of parent of parent 
        var $targetControl = $inputControl.parent().parent().parent();
        $targetControl.addClass('align-right');

        sg.utls.kndoUI.restrictDecimals(numericTxtBox, options.model.Decimals, maxLength);
    },

    getCheckTemplate: function (isHeader) {
        var id = isHeader ? "selectAllOptChk" : "selectOptChk";
        var index = optionalFieldUIGrid.gridIds.indexOf(optionalFieldUIGrid.gridId);
        if (index > 0) {
            id = id + index.toString();
        }
        var template = "<input type='checkbox' " + (isHeader ? "id='" : "class='") + id + "' />";
        return sg.controls.ApplyCheckboxStyle(template);
    },

    getColTemplate: function (fieldName, isHidden) {

        // Note:
        //     '#: ' - Kendo will html encode the text
        //     '#= ' - Kendo will NOT encode the text
        var encodedTemplateStart = '#: ';
        var nonencodedTemplateStart = '#= ';
        var templateEnd = ' #';
        var template = '';
        if (fieldName === "Value" || fieldName === "DefaultValue") {
            template = encodedTemplateStart + 'optionalFieldUIGrid.getValue(Type, ' + fieldName + ', ValueSet, Decimals)' + templateEnd;
            return template;
        }
        else if (fieldName === "Settings") {
            template = nonencodedTemplateStart + 'optionalFieldUIGrid.getSettings(SerialNumber)' + templateEnd;
            return isHidden ? null : template;
        }
        else {
            template = encodedTemplateStart + 'optionalFieldUIGrid.getYesNoValue(' + fieldName + ')' + templateEnd;
            return isHidden ? null : template;
        }
    },

    getColumn: function (fieldName, isHidden, caption, columnClass, templateExp, headerTemplateExp, editor) {
        var column = {
            field: fieldName,
            hidden: isHidden,
            title: caption,
            attributes: isHidden ? { sg_Customizable: false } : { "class": columnClass },
            headerAttributes: { "class": columnClass },
            template: templateExp,
            headerTemplate: headerTemplateExp,
            editor: editor
        };
        return column;
    }
};

// Optional Fields Grid 
var optionalFieldUIGrid =
{
    gridId: "OptionalFieldGrid",
    btnDeleteLineId: "btnDeleteOptionalFieldLine",
    btnAddLineId: "btnAddOptionalFieldLine",
    btnEditColumnsId: "btnEditColumns",
    selectAllCheckId: "selectAllOptChk",
    selectCheckId: "selectOptChk",
    preferencesTypeId: null,
    finder: "Csoptionalfields",
    modelData: null,
    modelName: "OptionalFields",
    newLineItem: null,
    pageFilter: null,
    isValueSetEditable: false,
    getOptionalFieldData: null,
    getOptionalFieldValue: null,
    isOptionalFieldExists: null,
    isCheckDuplicateRecord: false,
    checkDuplicateRecord: null,
    optionalFieldFilterName: null,
    isOptionalFieldUsed: null,
    saveOptionalField: null,
    deleteUrl: null,
    deleteFromServer: false,
    onDeleteSuccess: null,
    pageUrl: null,
    hasParamModel: true,
    paramIndex: -1,
    currentPageNumber: 0,
    currentRow: "",
    optionalFieldLineId: null,
    serialNumber: null,
    LineNumber: 0,
    optionalFieldFilterData: {},
    optionalFieldFilterValue: null,
    optionalFieldValueFilterData: {},
    addLineClicked: false,
    moveToNextPage: false,
    insertedIndex: 0,
    isEditMode: false,
    dataIndex: 0,
    gridIds: [],
    isReadOnly: false,
    isPopUp: false,
    isDefault: false,
    locationArray: [],
    settingsEditor: null,
    isCacheRemovable: false,
    isNewRecord: false,
    isOptionalFieldModel: false,
    defaultColumns: null,
    hasInvalidData: false,
    showSettings: false,
    isCheckValueSet: false,
    messageDivId: null,
    disableButtons: false,
    isAddNewLineInClientSide: true,
    hasErrorMessage: null,
    // temporary added, because of there are several place to using location as a http request parameter. Remove it once confirm
    optFldSrcName: null,
    retrivableCellOldVal: null,
    pristineData: { val: null, description: null },
    afterDataBindEvent: null,

    baselineItem: {
        "OptionalField": null,
        "OptionalFieldDescription": null,
        "Type": "1",
        "TypeString": null,
        "Length": "0",
        "Decimals": "0",
        "AllowBlank": "1",
        "Validate": "0",
        "ValueSet": "0",
        "ValueSetString": "0",
        "IsNewLine": true,
        "IsDeleted": false,
    },

    lineItem: {
        "Value": null,
        "TypedValueFieldIndex": "0",
        "TextValue": null,
        "AmountValue": "0",
        "NumberValue": "0",
        "IntegerValue": "0",
        "YesOrNoValue": "1",
        "DateValue": null,
        "TimeValue": null,
        "ValueDescription": null,
    },

    defaultLineItem: {
        "DefaultValue": null,
        "AutoInsert": "0",
        "Required": "0",
        "TypedDefaultValueFieldIndex": "0",
        "DefaultTextValue": null,
        "DefaultAmountValue": "0",
        "DefaultNumberValue": "0",
        "DefaultIntegerValue": "0",
        "DefaultYesOrNoValue": "1",
        "DefaultDateValue": null,
        "DefaultTimeValue": null,
        "DefaultValueDescription": null,
        "ValueDescription": null,
    },

    init: function (params, initialize) {
        // Temporary added, because of there are several place to using location as a http 
        // request parameter.Remove it once confirm
        if (!params.optFldSrcName)
            optionalFieldUIGrid.optFldSrcName = null;
        for (var prop in params) {
            optionalFieldUIGrid[prop] = params[prop];
            if (prop === "gridId") {
                if (optionalFieldUIGrid.gridIds.indexOf(params[prop]) === -1) {
                    optionalFieldUIGrid.gridIds.push(params[prop]);
                }
            }
        }
        // Temporary fix to address the issue in the screens that removed the optional field UI 
        // objects when license is not present.Such screens fail in the init functions
        var gridObj = $('#' + optionalFieldUIGrid.gridId);
        var grid;
        if (gridObj) {
            grid = gridObj.data("kendoGrid");
        }
        if (initialize && grid) {
            optionalFieldUIGrid.initButton();
            optionalFieldUIGrid.initCheckBox();
        } else {
            optionalFieldUIGrid.getButtonIds();
        }
    },

    initButton: function () {
        var btnAddLineId;
        var btnDeleteLineId;
        // Following condition is used for multiple optional field popup
        if (optionalFieldUIGrid.btnAddLineId != "btnAddOptionalFieldLine") {
            btnAddLineId = optionalFieldUIGrid.btnAddLineId;
            optionalFieldUIGrid.btnAddLineId = 'btnAddOptionalFieldLine';
        } else {
            btnAddLineId = "btnAddOptionalFieldLine";
        }

        if (optionalFieldUIGrid.btnDeleteLineId != "btnDeleteOptionalFieldLine") {
            btnDeleteLineId = optionalFieldUIGrid.btnDeleteLineId;
            optionalFieldUIGrid.btnDeleteLineId = "btnDeleteOptionalFieldLine";
        } else {
            btnDeleteLineId = "btnDeleteOptionalFieldLine";
        }

        var index = optionalFieldUIGrid.gridIds.indexOf(optionalFieldUIGrid.gridId);
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        optionalFieldUIGrid.defaultColumns = $.extend(true, {}, grid.columns);
        optionalFieldUIGrid.defaultColumns.length = grid.columns.length;

        if (btnAddLineId == "btnAddOptionalFieldLine" && btnDeleteLineId == "btnDeleteOptionalFieldLine") {
            if (index > 0) {
                index = index.toString();
                btnAddLineId += index;
                btnDeleteLineId += index;
            }
        }

        $('#' + btnAddLineId).on("click", function (e) {
            optionalFieldUIGrid.retrivableCellOldVal = null;
            sg.utls.SyncExecute(optionalFieldUIGrid.addNewLine);
        });

        $('#' + btnDeleteLineId).on("click", function (e) {
            optionalFieldUIGrid.deleteLine();
        });

        $('#' + optionalFieldUIGrid.btnEditColumnsId).on('click', function () {
            GridPreferences.initialize('#' + optionalFieldUIGrid.gridId, optionalFieldUIGrid.preferencesTypeId, $(this), optionalFieldUIGrid.defaultColumns);
            if (optionalFieldUIGrid.showSettings) {
                gridColConfig.registerSettingsEvent();
            }
        });
    },

    initCheckBox: function () {
        var gridId = optionalFieldUIGrid.gridId;
        var selectAllChk = "selectAllOptChk";
        var selectChk = "selectOptChk";
        var btnDeleteLineId = "btnDeleteOptionalFieldLine";
        var index = optionalFieldUIGrid.gridIds.indexOf(gridId);
        if (index > 0) {
            index = index.toString();
            selectAllChk += index;
            selectChk += index;
            btnDeleteLineId += index;
        }

        if ($("#" + gridId)) {
            optionalFieldUIGrid.selectAllCheckId = selectAllChk;
            $(document).on("change", "#" + selectAllChk, function () {
                var grid = $('#' + gridId).data("kendoGrid");
                var checkbox = $(this);
                var rows = grid.tbody.find("tr");
                rows.find("td:first input").prop("checked", checkbox.is(":checked")).applyCheckboxStyle();
                if ($("#" + selectAllChk).is(":checked")) {
                    rows.addClass("k-state-active");
                    sg.controls.enable("#" + btnDeleteLineId);
                } else {
                    rows.removeClass("k-state-active");
                    sg.controls.disable("#" + btnDeleteLineId);
                }
            });

            $(document).on("change", "." + selectChk, function () {
                $(this).closest("tr").toggleClass("k-state-active");
                var grid = $('#' + gridId).data("kendoGrid");
                var allChecked = true;
                var hasChecked = false;
                grid.tbody.find("." + selectChk).each(function (idx) {
                    if (!($(this).is(':checked'))) {
                        $("#" + selectAllChk).prop("checked", false).applyCheckboxStyle();
                        allChecked = false;
                        return;
                    } else {
                        hasChecked = true;
                    }
                });
                if (allChecked) {
                    $("#" + selectAllChk).prop("checked", true).applyCheckboxStyle();
                }

                if (hasChecked) {
                    sg.controls.enable("#" + btnDeleteLineId);
                } else {
                    sg.controls.disable("#" + btnDeleteLineId);
                }
            });
        }
    },

    getButtonIds: function () {
        var index = optionalFieldUIGrid.gridIds.indexOf(optionalFieldUIGrid.gridId);
        var btnDeleteLineId = "btnDeleteOptionalFieldLine";
        var btnAddLineId = "btnAddOptionalFieldLine";
        var selectAllCheckId = "selectAllOptChk";
        var selectCheckId = "selectOptChk";
        if (index > 0) {
            index = index.toString();
            btnDeleteLineId += index;
            btnAddLineId += index;
            selectAllCheckId += index;
            selectCheckId += index;
        }
        optionalFieldUIGrid.btnDeleteLineId = btnDeleteLineId;
        optionalFieldUIGrid.btnAddLineId = btnAddLineId;
        optionalFieldUIGrid.selectAllCheckId = selectAllCheckId;
        optionalFieldUIGrid.selectCheckId = selectCheckId;
    },

    getYesNoValue: function (fieldValue) {
        if (isNaN(fieldValue)) {
            return fieldValue === "Yes" ? optionalFieldsResources.yes : optionalFieldsResources.no;
        }
        else {
            return (fieldValue === "1" || fieldValue === 1) ? optionalFieldsResources.yes : optionalFieldsResources.no;
        }
    },

    getValue: function (type, fieldValue, valueSet, decimals) {
        if (type === optionalFieldEnum.Type.YesNo && fieldValue !== null) {
            if (valueSet !== 0) {
                var val = $.trim(fieldValue);
                return (val === "1" || val === 1) ? optionalFieldsResources.yes : optionalFieldsResources.no;
            } else {
                return "";
            }
        } else if (type == optionalFieldEnum.Type.Date && fieldValue !== null) {
            val = sg.utls.kndoUI.getFormattedDate(fieldValue);
            return (val === null) ? $.trim(fieldValue) : val;
        } else if ((type === optionalFieldEnum.Type.Integer || type === optionalFieldEnum.Type.Amount || type === optionalFieldEnum.Type.Number) && fieldValue !== null) {
            if (!fieldValue) {
                fieldValue = 0;
            }
            val = sg.utls.kndoUI.getFormattedDecimalNumber(fieldValue, decimals);
            if (valueSet == 0) {
                val = "";
            }

            //return '<span style="float:right">' + val + '</span>';
            return val;

        } else if (fieldValue !== null) {
            return (valueSet == 1) ? $.trim(fieldValue) : "";

        } else {
            return " ";
        }
    },

    getSettings: function (container) {
        var div = $(optionalFieldFields.settingsField);
        var button = div.find('input[type=button]')[0];
        button.id = button.id + container;
        $(button).attr('tag', container);

        if (optionalFieldUIGrid.modelData.Location !== undefined) {
            var loc = parseInt(optionalFieldUIGrid.modelData.Location());
            var arrLoc = optionalFieldUIGrid.locationArray.map(Number);
            $(button).attr("disabled", arrLoc.indexOf(loc) === -1);
            if (arrLoc.indexOf(loc) === -1) {
                $(button).addClass("pencil-edit-disabled");
            }
        }
        return div.html();
    },

    resetFocus: function (dataItem, columnName) {
        if (dataItem !== undefined) {
            var index = GridPreferencesHelper.getColumnIndex('#' + optionalFieldUIGrid.gridId, columnName);
            var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
            $('#' + optionalFieldUIGrid.gridId).data("kendoGrid").editCell(row.find(">td").eq(index));
        }
    },

    optionalFieldCancel: function (data) {
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var selectedData = sg.utls.kndoUI.getSelectedRowData(grid);

        if (selectedData !== undefined && selectedData.OptionalField != "" && selectedData.OptionalField != null) {
            if (selectedData.IsNewLine || selectedData.HasChanged) {
                if (optionalFieldUIGrid.getOptionalFieldData != null) {
                    var location = 0;
                    if (optionalFieldUIGrid.modelData.Location !== undefined) {
                        location = optionalFieldUIGrid.modelData.Location();
                        optionalFieldUIGrid.getOptionalFieldData(selectedData.OptionalField.toUpperCase(), location, selectedData);
                    } else if (optionalFieldUIGrid.modelData.AccountNumber !== undefined) {
                        location = (optionalFieldUIGrid.isValueSetEditable) ? 1 : 0;
                        optionalFieldUIGrid.getOptionalFieldData(selectedData.OptionalField.toUpperCase(), location);
                    } else {
                        optionalFieldUIGrid.getOptionalFieldData(selectedData);
                    }
                }
            }
        }
        optionalFieldUIGrid.resetFocus(selectedData, 'OptionalField');
    },

    optionalFieldValueCancel: function (data) {
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var selectedData = sg.utls.kndoUI.getSelectedRowData(grid);
        var value = optionalFieldUIGrid.isDefault ? "DefaultValue" : "Value";
        optionalFieldUIGrid.resetFocus(selectedData, value);
    },

    deleteMsg: function (deleteClass) {
        var confirmationMsg = ($(deleteClass + ':checked').length > 1) ? optionalFieldsResources.deleteLinesConfirm : optionalFieldsResources.deleteLineConfirm;
        return confirmationMsg;
    },

    showMessage: function (message) {
        var msg = {};
        msg.UserMessage = {};
        msg.Data = {};
        msg.UserMessage.Message = optionalFieldsResources.ProcessFailedMessage;
        msg.UserMessage.Errors = [{ Message: message }];
        if (optionalFieldUIGrid.messageDivId) {
            sg.utls.showMessagePopup(msg, "#" + optionalFieldUIGrid.messageDivId);
        } else {
            sg.utls.showMessagePopup(msg, "#windowmessage");
            if ($("#windowmessage1")) {
                sg.utls.showMessagePopupWithoutClose(msg, "#windowmessage1");
            }
        }
    },

    validateData: function () {
        if ($("#windowmessage")) { $("#windowmessage").empty(); }
        if ($("#windowmessage1")) { $("#windowmessage1").empty(); }

        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var data = grid.dataSource.data();
        data = data.filter(function (value) {
            return (value.IsNewLine || value.HasChanged) && value.Validate && !value.Value && !value.AllowBlank;
        });
        if (data && data.length > 0 && optionalFieldUIGrid.isPopUp && $("#windowmessage")) {
            var message = $.validator.format(optionalFieldsResources.InvalidInput, data[0].OptionalField);
            optionalFieldUIGrid.showMessage(message);
        }
    },

    mergeNewLine: function () {
        var item = (typeof optionalFieldUIGrid.newLineItem === "function") ? optionalFieldUIGrid.newLineItem() : optionalFieldUIGrid.newLineItem;
        var sn = sg.utls.generatekey();
        $.extend(item, optionalFieldUIGrid.baselineItem);
        $.extend(item, optionalFieldUIGrid.isDefault ? optionalFieldUIGrid.defaultLineItem : optionalFieldUIGrid.lineItem);
        $.extend(item, { "SerialNumber": sn });
        return item;
    },

    addClientSideLine: function (newLineFunctionCall) {
        var grid = $("#" + optionalFieldUIGrid.gridId).data("kendoGrid");
        var dataSource = grid.dataSource;
        var dataRows = grid.items();
        var pageNumber = dataSource.page();
        var pageSize = dataSource.pageSize();
        var insertedIndex = dataRows.index(grid.select());
        var newLine = newLineFunctionCall.call();


        if (insertedIndex < 0) {
            insertedIndex = 0;
        }
        if (insertedIndex + 1 == pageSize) {
            optionalFieldUIGrid.paramIndex = 0;
            dataSource.query({ page: pageNumber + 1, pageSize: sg.utls.gridPageSize });
            setTimeout(function () {
                dataSource.insert(0, newLine);
                var cell = grid.tbody.find(">tr:eq(0) >td:eq(2)");
                grid.editCell(cell);
            }, 1000);
        } else {
            dataSource.options.serverPaging = (dataSource.page() === 1) ? false : true;
            dataSource.insert(insertedIndex + 1, newLine);
        }
        return true;
    },

    checkValueSet: function (data) {
        var isInValidValueSet = false;
        var optField = "";
        $.each(data, function (index, row) {
            if (row["ValueSet"] == 0) {
                isInValidValueSet = true;
                optField = row["OptionalField"];
                return false;
            }
        });
        if (isInValidValueSet) {
            var errorMsg = $.validator.format(optionalFieldsResources.InvalidValueSetMsg, optField.toUpperCase());
            if (optionalFieldUIGrid.isPopUp) {
                optionalFieldUIGrid.showMessage(errorMsg);
            } else {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
            }
            return true;
        }
        return false;
    },

    isDuplicateCode: function (data) {
        var isDuplicate = false;
        var optValue = "";
        $.each(data, function (index, item) {
            optValue = item.OptionalField;
            var optfld = data.filter(function (val) { return val.OptionalField == optValue; });
            isDuplicate = (optfld.length > 1);
            return !isDuplicate;
        });
        if (isDuplicate) {
            var errorMsg = $.validator.format(optionalFieldsResources.duplicateMessage, optionalFieldsResources.optionalFieldTitle, optValue.toUpperCase());
            if (optionalFieldUIGrid.isPopUp) {
                optionalFieldUIGrid.showMessage(errorMsg);
            } else {
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMsg);
            }
        }
        return isDuplicate
    },

    addNewLine: function (e) {
        if (optionalFieldUIGrid.hasInvalidData) {
            return false;
        }
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var newLineExist = false;
        optionalFieldUIGrid.isEditMode = false;

        var data = grid.dataSource.data();
        if (data != null && data.length > 0) {
            $.each(data, function (index, item) {
                if ((item.OptionalField === null || item.OptionalField === "") && item.IsDeleted !== true) {
                    newLineExist = true;
                    return false;
                }
                optionalFieldUIGrid.dataIndex = data.length + 1;
            });
        }
        else {
            optionalFieldUIGrid.dataIndex = 1;
        }

        if (newLineExist || optionalFieldUIGrid.isDuplicateCode(data)) {
            return;
        }
        if (optionalFieldUIGrid.isCheckValueSet) {
            if (optionalFieldUIGrid.checkValueSet(data)) {
                return;
            }
        }

        optionalFieldUIGrid.addLineClicked = true;
        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        optionalFieldUIGrid.insertedIndex = grid.dataSource.indexOf(currentRowGrid);

        if ((!newLineExist)) {
            var pageUrl = optionalFieldUIGrid.pageUrl;
            optionalFieldUIGrid.pageUrl = null;
            optionalFieldUIGrid.hasParamModel = true;
            optionalFieldUIGrid.paramIndex = -1;

            var model = optionalFieldUIGrid.modelData[optionalFieldUIGrid.modelName];
            if (typeof model === "function") {
                model = model();
            }
            var isClientSideAdditon;
            if (optionalFieldUIGrid.isPopUp && optionalFieldUIGrid.isAddNewLineInClientSide) {
                isClientSideAdditon = optionalFieldUIGrid.addClientSideLine(optionalFieldUIGrid.mergeNewLine);
            } else {
                isClientSideAdditon = sg.utls.kndoUI.addLine(model, optionalFieldUIGrid.gridId, optionalFieldUIGrid.mergeNewLine, optFldGridUtils.getParamPaging, grid.dataSource.page());
            }
            optionalFieldUIGrid.paramIndex = -1;
            optionalFieldUIGrid.pageUrl = pageUrl;

            if (isClientSideAdditon) {
                var totalResultCount = model.TotalResultsCount();
                model.TotalResultsCount(totalResultCount + 1);
            }
        }
        sg.controls.enable("#" + optionalFieldUIGrid.selectAllCheckId);
    },

    deleteLineWithCheck: function (grid, isOptionalFieldUsed) {
        if (typeof isOptionalFieldUsed === "function") {
            var list = [];
            var model = ko.mapping.toJS(optionalFieldUIGrid.modelData);
            grid.tbody.find(":checked").closest("tr").each(function (index) {
                list.push(grid.dataItem($(this)));
            });
            model[optionalFieldUIGrid.modelName].Items = list;
            optionalFieldUIGrid.hasParamModel = true;
            isOptionalFieldUsed(model);
        }
    },

    setDeleteLineFlag: function (grid) {
        var data = optionalFieldUIGrid.modelData[optionalFieldUIGrid.modelName];
        if (typeof data === "function") {
            data = data();
        }
        var items = (data.Items !== undefined) ? data.Items() : data;
        grid.tbody.find(":checked").closest("tr").each(function (index) {
            if (grid.dataItem($(this)).IsNewLine) {
                return true;
            }
            var optValue = grid.dataItem($(this)).OptionalField;
            var item = items.filter(function (value) { return value.OptionalField() == optValue; });
            if (item && item.length > 0) {
                item[0].IsDeleted(true);
            }
        });
    },

    deleteLineWithUrl: function (grid, pageUrl) {
        optionalFieldUIGrid.setDeleteLineFlag(grid);
        //Cause delete/add row issue, Becasue the js always pass by ref if the type is object
        //grid.tbody.find(":checked").closest("tr").each(function (index) {
        //    grid.removeRow($(this));
        //});
        if (optionalFieldUIGrid.deleteFromServer) {
            var data = optionalFieldUIGrid.modelData[optionalFieldUIGrid.modelName];
            if (typeof data === "function") {
                data = data();
            }
            var items = (data.Items !== undefined) ? data.Items() : data;
            var modelData = {
                model: { items: ko.mapping.toJS(items) },
                pageNumber: grid.dataSource.page(),
                pageSize: grid.dataSource.pageSize()
            }
            sg.utls.ajaxPost(pageUrl, modelData, optionalFieldUIGrid.onDeleteSuccess);
        }
        else {
            grid.tbody.find(":checked").closest("tr").each(function (index) {
                grid.removeRow($(this));
            });
        }
    },

    updateGrid: function (grid, chkAllId, btnDeleteId) {
        if (grid.dataSource.total() == 0) {
            sg.controls.disable("#" + chkAllId);
        } else {
            sg.controls.enable("#" + chkAllId);
        }
        if (grid._data.length <= grid.dataSource.total()) {
            var pageNumber = grid.dataSource.page();
            var pageSize = grid.dataSource.pageSize();
            var retrievePage = pageNumber - 1;
            if (retrievePage >= (grid.dataSource.total() / pageSize)) {
                retrievePage = retrievePage - 1;
            }
            optFldGridUtils.getParamPaging(null, retrievePage, pageSize, -1);
            grid.dataSource.page(retrievePage + 1);
        }

        $("#" + chkAllId).prop("checked", false);
        sg.controls.disable("#" + btnDeleteId);
        sg.controls.enable("#" + optionalFieldUIGrid.btnAddLineId);
    },

    deleteLine: function () {
        var gridId = optionalFieldUIGrid.gridId;
        var chkAllId = optionalFieldUIGrid.selectAllCheckId;
        var chkClassId = optionalFieldUIGrid.selectCheckId;
        var btnDeleteId = optionalFieldUIGrid.btnDeleteLineId;
        var isOptionalFieldUsed = optionalFieldUIGrid.isOptionalFieldUsed;
        var pageUrl = optionalFieldUIGrid.deleteUrl;
        var grid = $('#' + gridId).data("kendoGrid");
        var confirmationMsg = optionalFieldUIGrid.deleteMsg('.' + chkClassId);

        sg.utls.showKendoConfirmationDialog(
            function () {
                if ($("#message")) { $("#message").empty(); }
                if ($("#windowmessage")) { $("#windowmessage").empty(); }
                if ($("#windowmessage1")) { $("#windowmessage1").empty(); }

                if (isOptionalFieldUsed !== null) {
                    optionalFieldUIGrid.deleteLineWithCheck(grid, isOptionalFieldUsed);
                    return;
                }
                if (pageUrl !== null) {
                    optionalFieldUIGrid.deleteLineWithUrl(grid, pageUrl);
                } else {
                    optionalFieldUIGrid.setDeleteLineFlag(grid);
                    grid.tbody.find(":checked").closest("tr").each(function (index) {
                        grid.removeRow($(this));
                        $.each(grid.dataSource.data(), function (key, value) {
                            var optValue = value.OptionalField;
                            var data = grid.dataSource.data().filter(function (val) { return val.OptionalField == optValue; });
                            optionalFieldUIGrid.hasInvalidData = (data.length > 1) ? true : false;
                            return !optionalFieldUIGrid.hasInvalidData;
                        });
                    });
                }
                optionalFieldUIGrid.updateGrid(grid, chkAllId, btnDeleteId);
            },
            function () { }, confirmationMsg, optionalFieldsResources.deleteTitle);
    },

    save: function () {
        if (optionalFieldUIGrid.isReadOnly) {
            return false;
        }

        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var data = Object.create(grid.dataSource.data());
        var modelData = optionalFieldUIGrid.modelData[optionalFieldUIGrid.modelName];

        modelData = (typeof modelData === "function") ? modelData() : modelData;
        modelData = (typeof modelData.Items === "function") ? modelData.Items() : modelData.Items;
        modelData = ko.mapping.toJS(modelData);
        modelData = modelData.filter(function (value) { return value.IsDeleted; });

        // Add this function to fix the continue push the elements into 
        // array even the data has already delete from server. 
        var hasUpdates;
        if (modelData && optionalFieldUIGrid.deleteFromServer) {
            hasUpdates = true;
        }
        else {// the origenal function inside else
            for (var i = 0, len = modelData.length; i < len; i++) {
                data.push(modelData[i]);
            }
            hasUpdates = ko.utils.arrayFirst(data, function (item) {
                return item.HasChanged || item.IsNewLine || item.IsDeleted;
            });
        }

        if (hasUpdates && optionalFieldUIGrid.saveOptionalField !== null) {
            var nullItem = ko.utils.arrayFirst(data, function (item) {
                return item.OptionalField === null || item.OptionalField === "";
            });
            if (nullItem !== null) {
                data.remove(nullItem);
            }
            optionalFieldUIGrid.saveOptionalField(data);
            optionalFieldUIGrid.createNewLine = false;
            return true;
        }
        return false;
    },

    setOptionalFieldValue: function (value, description, fromFinder) {
        if ($("#message")) { $("#message").empty(); }
        if ($("#windowmessage")) { $("#windowmessage").empty(); }
        if ($("#windowmessage1")) { $("#windowmessage1").empty(); }

        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var selectRow = sg.utls.kndoUI.getSelectedRowData(grid);
        var data = optionalFieldUIGrid.modelData[optionalFieldUIGrid.modelName];
        var items = (typeof data === "function") ? data().Items() : data.Items();
        if (items.length > 0) {
            var dataRow = items.filter(function (val) { return val.OptionalField() == selectRow.OptionalField; });
            dataRow = (dataRow && dataRow.length > 0) ? dataRow[0] : null;
        }
        selectRow.set("ValueSet", 1);
        selectRow.set("ValueSetString", 1);
        if (selectRow.valueSet) {
            selectRow.valueSet.value = 1;
        }
        if (!value && !fromFinder) {
            var fldValueDesc = (selectRow.ValueDescription === undefined) ? "DefaultValueDescription" : "ValueDescription";
            selectRow.set(fldValueDesc, "");
        }
        if (fromFinder) {
            if (selectRow.ValueDescription !== undefined) {
                selectRow.set("ValueDescription", description);
            }
            if (selectRow.DefaultValueDescription !== undefined) {
                selectRow.set("DefaultValueDescription", description);
            }
        }
        var prefix = (selectRow.Value === undefined) ? "Default" : "";
        var numericType = [optionalFieldEnum.Type.Integer, optionalFieldEnum.Type.Number, optionalFieldEnum.Type.Amount];
        var isDefaultNumeric = (numericType.indexOf(selectRow.Type) > -1 && value === null);
        selectRow.set(prefix + "Value", isDefaultNumeric ? 0 : value);
        selectRow.set("HasChanged", true);

        if (!dataRow) {
            return;
        }
        if (dataRow["HasChanged"] && typeof dataRow["HasChanged"] == "function") {
            dataRow["HasChanged"](1);
        }
        dataRow[prefix + "Value"](isDefaultNumeric ? 0 : value);
        switch (selectRow.Type) {
            case optionalFieldEnum.Type.Text:
                dataRow[prefix + "TextValue"](value);
                break;
            case optionalFieldEnum.Type.Integer:
                dataRow[prefix + "IntegerValue"](value || 0);
                break;
            case optionalFieldEnum.Type.Number:
                dataRow[prefix + "NumberValue"](value || 0);
                break;
            case optionalFieldEnum.Type.Amount:
                dataRow[prefix + "AmountValue"](value || 0);
                break;
            case optionalFieldEnum.Type.Date:
                if (fromFinder) {
                    value = sg.utls.kndoUI.convertStringToDate(value);
                }
                dataRow[prefix + "DateValue"](value);
                break;
            case optionalFieldEnum.Type.YesNo:
                dataRow[prefix + "YesOrNoValue"](value);
                break;
            case optionalFieldEnum.Type.Time:
                gridColConfig.finderSelect = fromFinder;
                dataRow[prefix + "TimeValue"](value);
                break;
        }
    },

    updateValueInGrid: function (optionalField, value) {
        optionalFieldUIGrid.setOptionalFieldValue(value, "", false);
    },

    disableGridButtons: function () {
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        sg.controls.disable('#' + optionalFieldUIGrid.btnAddLineId);
        sg.controls.disable('#' + optionalFieldUIGrid.btnDeleteLineId);
        sg.controls.disable("#" + optionalFieldUIGrid.selectAllCheckId);
        $("#" + optionalFieldUIGrid.selectAllCheckId).prop("checked", false);
        grid.tbody.find("." + optionalFieldUIGrid.selectCheckId).each(function () {
            $(this).attr("disabled", true);
        });
    },

    OnOptionalFieldSelection: function (rowdata) {
        if ($("#windowmessage")) { $("#windowmessage").empty(); }
        if ($("#windowmessage1")) { $("#windowmessage1").empty(); }

        var gridId = optionalFieldUIGrid.gridId;
        if ($('#' + gridId)) {
            var grid = $('#' + gridId).data("kendoGrid");
            var selectRow = sg.utls.kndoUI.getSelectedRowData(grid);
            optionalFieldUIGrid.serialNumber = (selectRow.SerialNumber) ? selectRow.SerialNumber : selectRow.DisplayIndex;

            var optionalField = rowdata.OPTFIELD;
            const type = parseInt(rowdata.TYPE, 10);

            selectRow.set("OptionalField", optionalField);
            selectRow.set("OptionalFieldDescription", rowdata.FDESC);
            selectRow.set("Type", type);
            selectRow.set("Length", rowdata.LENGTH);
            selectRow.set("Decimals", rowdata.DECIMALS);
            var fldValue = (selectRow.DefaultValue === undefined) ? "Value" : "DefaultValue";
            if (rowdata.DEFVAL) {
                selectRow.set(fldValue, rowdata.DEFVAL);
            } else {
                var defaultValue = ([6, 8, 100].indexOf(type) > -1) ? 0 : "";
                selectRow.set(fldValue, defaultValue);
            }
            var fldValueDesc = (selectRow.DefaultValueDescription) ? "ValueDescription" : "DefaultValueDescription";
            if (rowdata.VDESC !== undefined) {
                selectRow.set("ValueDescription", rowdata.VDESC);
                selectRow.set("DefaultValueDescription", rowdata.VDESC);
            } else {
                selectRow.set(fldValueDesc, "");
            }
            var valueSet = rowdata.SWSET;
            if (type === optionalFieldEnum.Type.Text || type === optionalFieldEnum.Type.Date) {
                //selectRow.set("AllowBlank", rowdata.AllowBlankValue);
                // Comments the above line because AllowBlankValue only exists in IC and CS model. 
                // However this function also need to handle the other models as OE, AP
                // I just put a condition here, After the determine where the allowBlankVaule has been used. Please Update this change

                const convertToBool = (val) => {
                    let str = String(val).toLowerCase();
                    if (str === 'true') {
                        return 1;
                    }
                    else if (str === 'false') {
                        return 0;
                    }
                    return str;
                }

                var allowBlank = convertToBool(rowdata.ALLOWNULL);
                selectRow.set("AllowBlank", allowBlank);

                var validate = convertToBool(rowdata.VALIDATE);
                // Just in case we support the integer value as flag. 1: checked, 0: unchecked
                if (validate <= allowBlank) {
                    valueSet = 1;
                }
                selectRow.set("Validate", validate);
            } else {
                selectRow.set("AllowBlank", 0);
            }
            if (valueSet !== undefined) {
                selectRow.set("ValueSet", valueSet);
                selectRow.set("ValueSetString", valueSet);
            } else {
                selectRow.set("ValueSet", 0);
            }

            optFldGridUtils.setRowData(selectRow, rowdata, (selectRow.Value === undefined) ? "Default" : "");

            if (optionalFieldUIGrid.isCheckDuplicateRecord) {
                if (optionalFieldUIGrid.checkDuplicateRecord == null)
                    optFldGridUtils.checkDuplicateRecord(grid.dataSource, "OptionalField", optionalField, selectRow);
                else {
                    var id = "";
                    if (optionalFieldUIGrid.modelData.AccountNumber !== undefined) {
                        id = optionalFieldUIGrid.modelData.AccountNumber();
                    }
                    optionalFieldUIGrid.checkDuplicateRecord(gridId, "OptionalField", rowdata.OPTFIELD, "btnAddOptionalFieldLine", optionalFieldsResources.optionalFieldTitle, optionalFieldUIGrid.optionalFieldLineId, id);
                }
            }
            if (optionalFieldUIGrid.settingsEditor) {
                gridColConfig.registerSettingsEvent();
            }

            optionalFieldUIGrid.hasInvalidData = false;
            optionalFieldUIGrid.resetFocus(selectRow, 'OptionalField');
        }
    },

    // This is the overwritten method for repository calls
    OnOptionalFieldSuccess: function (rowdata) {
        if ($("#windowmessage")) $("#windowmessage").empty();
        if ($("#windowmessage1")) $("#windowmessage1").empty();

        var gridId = optionalFieldUIGrid.gridId;
        if ($('#' + gridId)) {
            var grid = $('#' + gridId).data("kendoGrid");
            var selectRow = sg.utls.kndoUI.getSelectedRowData(grid);
            optionalFieldUIGrid.serialNumber = (selectRow.SerialNumber) ? selectRow.SerialNumber : selectRow.DisplayIndex;

            var optionalField = (rowdata.OptionalFieldKey == null) ? rowdata.OptionalField : rowdata.OptionalFieldKey;
            var optionalFieldDesc = (rowdata.Description == null) ? rowdata.OptionalFieldDescription : rowdata.Description;

            selectRow.set("OptionalField", optionalField);
            selectRow.set("OptionalFieldDescription", optionalFieldDesc);
            selectRow.set("Type", rowdata.Type);
            selectRow.set("Length", rowdata.Length);
            selectRow.set("Decimals", rowdata.Decimals);
            var fldValue = (selectRow.DefaultValue === undefined) ? "Value" : "DefaultValue";
            if (rowdata.DefaultValue) {
                selectRow.set(fldValue, rowdata.DefaultValue);
            } else {
                var defaultValue = ([6, 8, 100].indexOf(rowdata.Type) > -1) ? 0 : "";
                selectRow.set(fldValue, defaultValue);
            }
            var fldValueDesc = (selectRow.DefaultValueDescription === undefined) ? "ValueDescription" : "DefaultValueDescription";
            if (rowdata.DefaultValueDescription !== undefined) {
                selectRow.set("ValueDescription", rowdata.DefaultValueDescription);
                selectRow.set("DefaultValueDescription", rowdata.DefaultValueDescription);
            } else if (rowdata.ValueDescription !== undefined) {
                selectRow.set(fldValueDesc, rowdata.ValueDescription);
            } else {
                selectRow.set(fldValueDesc, "");
            }
            var valueSet = rowdata.ValueSet;
            var allowBlank;
            if (rowdata.Type != optionalFieldEnum.Type.YesNo) {
                // Comments the above line because AllowBlankValue only exists in IC and CS model. 
                // However this function also need to handle the other models as OE, AP
                // I just put a condition here, After the determine where the allowBlankVaule has been used. Please Update this changes
                allowBlank = rowdata.AllowBlankValue ? rowdata.AllowBlankValue : rowdata.AllowBlank;
                selectRow.set("AllowBlank", allowBlank);

                var validate;
                if ((rowdata.IsValidate === undefined) && (rowdata.Validate === undefined)) {
                    validate = 0;
                } else if (rowdata.Validate !== undefined) {
                    validate = rowdata.Validate;
                } else if (rowdata.IsValidate !== undefined) {
                    validate = rowdata.IsValidate ? 1 : 0;
                }
                // Just in case we support the integer value as flag. 1: checked, 0: unchecked
                if (validate <= allowBlank) {
                    valueSet = 1;
                }
                selectRow.set("Validate", validate);
            } else {
                selectRow.set("AllowBlank", 0);
            }
            if (valueSet !== undefined) {
                selectRow.set("ValueSet", valueSet);
                selectRow.set("ValueSetString", valueSet);
            } else {
                selectRow.set("ValueSet", 0);
            }

            optFldGridUtils.setServerRowData(selectRow, rowdata, (selectRow.Value === undefined) ? "Default" : "");

            if (optionalFieldUIGrid.isCheckDuplicateRecord) {
                if (optionalFieldUIGrid.checkDuplicateRecord == null)
                    optFldGridUtils.checkDuplicateRecord(grid.dataSource, "OptionalField", optionalField, selectRow);
                else {
                    var id = "";
                    if (optionalFieldUIGrid.modelData.AccountNumber !== undefined) {
                        id = optionalFieldUIGrid.modelData.AccountNumber();
                    }
                    optionalFieldUIGrid.checkDuplicateRecord(gridId, "OptionalField", rowdata.OptionalField, "btnAddOptionalFieldLine", optionalFieldsResources.optionalFieldTitle, optionalFieldUIGrid.optionalFieldLineId, id);
                }
            }
            if (optionalFieldUIGrid.settingsEditor) {
                gridColConfig.registerSettingsEvent();
            }

            optionalFieldUIGrid.hasInvalidData = false;
            optionalFieldUIGrid.resetFocus(selectRow, 'OptionalField');
        }
    },

    OnOptionalFieldValueSelection: function (rowdata) {
        var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
        var selectRow = sg.utls.kndoUI.getSelectedRowData(grid);
        var prefix = (selectRow.Value === undefined) ? "Default" : "";

        const type = parseInt(rowdata.TYPE, 10);
        let value = rowdata.VALUE;

        if (type === optionalFieldEnum.Type.Time) {
            value = sg.utls.kndoUI.getTimeFormate(rowdata.VALIFTIME);
        } else if (type === optionalFieldEnum.Type.Date) {
            value = sg.utls.kndoUI.convertStringToDate(rowdata.VALIFDATE);
        }

        optionalFieldUIGrid.setOptionalFieldValue(value, rowdata.VDESC, true);
        optionalFieldUIGrid.hasInvalidData = false;
        optionalFieldUIGrid.resetFocus(selectRow, prefix + "Value");
    },

    GetSelectedOptField: function () {
        if ($('#' + optionalFieldUIGrid.gridId)) {
            var grid = $('#' + optionalFieldUIGrid.gridId).data("kendoGrid");
            var row = grid.tbody.find("tr[data-uid='" + optionalFieldUIGrid.optionalFieldLineId + "']");
            var gridData = grid.dataItem(row);
            return gridData;
        }
        return null;
    },

    optionalFieldConfig: function (area, controller, action, gridId, isDefault, isDefaultValueDesc, isValueSet, modelName, showSettings, isDetailOptField, disableAutoBind) {
        var autoBind = true;
        if (disableAutoBind !== null && disableAutoBind !== undefined) {
            autoBind = !disableAutoBind;
        }

        if (optionalFieldUIGrid.gridIds.indexOf(gridId) === -1) {
            optionalFieldUIGrid.gridIds.push(gridId);
        }
        if (isDetailOptField === undefined) {
            isDetailOptField = false;
        }
        optionalFieldUIGrid.gridId = gridId;
        optionalFieldUIGrid.showSettings = showSettings;
        optionalFieldUIGrid.isDefault = isDefault;
        return {
            pageUrl: sg.utls.url.buildUrl(area, controller, action),
            isServerPaging: true,
            pageSize: sg.utls.gridPageSize,
            reorderable: sg.utls.reorderable,
            scrollable: true,
            resizable: true,
            navigatable: true,
            selectable: true,
            sortable: false,
            param: null,
            autoBind: autoBind,

            pageable: {
                input: true,
                numeric: false,
                change: function (e) {
                    optionalFieldUIGrid.validateData();
                }
            },
            schema: {
                model: {
                    fields: {
                        Delete: { editable: false }
                    }
                }
            },
            editable: {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },

            edit: function (e) {
                var grid = $('#' + gridId).data("kendoGrid");
                optionalFieldUIGrid.currentRow = e.sender.select().attr("data-uid");
                grid.select(e.container.closest("tr"));
            },

            buildGridData: function (successData) {
                var gridData = null;
                var grid = $('#' + gridId).data("kendoGrid");
                var dataSource = grid.dataSource;
                if (successData == null) {
                    return;
                }
                grid.dataSource.options.serverPaging = true;
                if ((successData.UserMessage && successData.UserMessage.IsSuccess) || successData.Items !== undefined) {
                    optionalFieldUIGrid.hasErrorMessage = false;
                    gridData = [];

                    var optionalField = (successData.Data !== undefined) ? successData.Data[modelName] : successData;
                    $.each(optionalField.Items, function (index, value) {
                        var data = value;
                        if (data.Type === optionalFieldEnum.Type.Date && data.DefaultValue !== null && data.DefaultValue !== "") {
                            var dateValue = sg.utls.kndoUI.convertStringToDate(data.DefaultDateValue);
                            data.DefaultValue = dateValue;
                            data.DefaultDateValue = dateValue;
                        }
                    });
                    ko.mapping.fromJS(optionalField, {}, optionalFieldUIGrid.modelData[modelName]);
                    gridData.data = optionalField.Items;
                    gridData.totalResultsCount = optionalField.TotalResultsCount;

                } else {
                    if (optionalFieldUIGrid.isPopUp) {
                        optionalFieldUIGrid.hasErrorMessage = true;
                        sg.utls.showMessagePopupWithoutClose(successData, "#windowmessage");
                    } else {
                        sg.utls.showMessage(successData);
                    }
                }

                optionalFieldUIGrid.currentPageNumber = dataSource.page();
                optionalFieldUIGrid.isNewRecord = successData.IsNewRecord;
                return gridData;
            },

            getPageUrl: function () {
                return optionalFieldUIGrid.pageUrl;
            },

            getParam: function () {
                var grid = $('#' + gridId).data("kendoGrid");
                var pageSize = grid.dataSource.pageSize();
                var model = {};
                var items;

                //sg.controls.disable("#" + optionalFieldUIGrid.btnAddLineId);
                if (optionalFieldUIGrid.modelData != null && optionalFieldUIGrid.hasParamModel) {
                    if (typeof (optionalFieldUIGrid.modelData[modelName]) === "function") {
                        var data = optionalFieldUIGrid.modelData[modelName]();
                        if (data.Items !== undefined) {
                            items = (typeof data.Items === "function") ? data.Items() : data.Items;
                            var gridItemsLenth = grid.dataSource.data().length;
                            if (items.length > gridItemsLenth || gridItemsLenth === 0) {
                                items = ko.mapping.toJS(items);
                                items = sg.utls.kndoUI.assignDisplayIndex(items, optionalFieldUIGrid.currentPageNumber, pageSize);
                                model = items;
                            } else {
                                model = grid.dataSource.data();
                            }
                        }

                    } else {
                        model = ko.mapping.toJS(optionalFieldUIGrid.isOptionalFieldModel ? optionalFieldUIGrid.modelData[modelName] : optionalFieldUIGrid.modelData);
                        items = optionalFieldUIGrid.isOptionalFieldModel ? model.Items : model[modelName].Items;
                        if (items != null) {
                            $.each(items, function (key, item) {
                                if (item.IsNewLine === true && (item.OptionalField === "" || item.OptionalField === null)) {
                                    item.IsDeleted = true;
                                }
                                if (item.Type === optionalFieldEnum.Type.Date) {
                                    if (item.Value && typeof (item.Value) === "string") {
                                        item.Value = sg.utls.kndoUI.convertStringToDate(item.Value);
                                    }
                                    if (item.DefaultValue && typeof (item.DefaultValue) === "string") {
                                        item.DefaultValue = sg.utls.kndoUI.convertStringToDate(item.DefaultValue);
                                    }
                                }
                            });
                        }
                        items = sg.utls.kndoUI.assignDisplayIndex(items, optionalFieldUIGrid.currentPageNumber, pageSize);
                        if (optionalFieldUIGrid.isOptionalFieldModel) {
                            model.Items = items;
                        } else {
                            model[modelName].Items = items;
                        }
                    }
                }

                var paramPaging = {
                    pageNumber: grid.dataSource.page() - 1,
                    pageSize: pageSize,
                    index: optionalFieldUIGrid.paramIndex,
                    model: model,
                    filters: (optionalFieldUIGrid.pageFilter) ? optionalFieldUIGrid.pageFilter() : null,
                    isCacheRemovable: optionalFieldUIGrid.isCacheRemovable,
                    isDetailOptField: isDetailOptField,
                };
                return paramPaging;
            },

            afterDataBind: function (e) {
                if (optionalFieldUIGrid.gridId !== null) {
                    var grid = $("#" + optionalFieldUIGrid.gridId).data("kendoGrid");
                    var gridIndex = GridPreferencesHelper.getColumnIndex("#" + optionalFieldUIGrid.gridId, "OptionalField");
                    if (optionalFieldUIGrid.addLineClicked) {
                        var editableRow = (optionalFieldUIGrid.insertedIndex + 1) % grid.dataSource.pageSize();
                        var cell = grid.tbody.find(">tr:eq(" + editableRow + ") >td:eq(" + gridIndex + ")");
                        grid.editCell(cell);
                        optionalFieldUIGrid.addLineClicked = false;
                        if (optionalFieldUIGrid.insertedIndex + 1 == grid.dataSource.pageSize()) {
                            optionalFieldUIGrid.moveToNextPage = true;
                        }
                    } else {
                        if (!optionalFieldUIGrid.moveToNextPage) {
                            setTimeout(function () {
                                sg.controls.Focus($("#" + optionalFieldUIGrid.btnAddLineId));
                            }, 350);
                        } else {
                            optionalFieldUIGrid.moveToNextPage = false;
                        }
                    }
                    var hasChecked = false;
                    grid.tbody.find("." + optionalFieldUIGrid.selectCheckId).each(function (index) {
                        if ($(this).is(':checked')) {
                            hasChecked = true;
                            return false;
                        } else {
                            return true;
                        }
                    });
                    if (hasChecked) {
                        sg.controls.enable("#" + optionalFieldUIGrid.btnDeleteLineId);
                    } else {
                        sg.controls.disable("#" + optionalFieldUIGrid.btnDeleteLineId);
                    }
                    var length = grid.dataSource.data().length;
                    if (length > 0) {
                        sg.controls.enable("#" + optionalFieldUIGrid.selectAllCheckId);
                    } else {
                        sg.controls.disable("#" + optionalFieldUIGrid.selectAllCheckId);
                    }

                    //
                    // B-99599
                    // Ensure that the cell containing the default value for rows that 
                    // represent numeric optional fields are right aligned.
                    //

                    // Get the grid data
                    var data = grid.dataSource.data();
                    $.each(data, function (index, row) {

                        var rowType = row.Type;
                        var $row = grid.tbody.find(">tr:eq(" + index + ")");
                        var isRowTypeNumeric =  rowType === optionalFieldTypeEnum.Amount ||
                                                rowType === optionalFieldTypeEnum.Number ||
                                                rowType === optionalFieldTypeEnum.Integer;

                        if (isRowTypeNumeric) {
                            // Iterate each cell in the row
                            $row.find('td').each(function () {
                                var $td = $(this);
                                var cellValue = $(this).html();

                                // Convert cell content to a number (if possible)
                                var cellValueAsNumber = '';
                                var temp = Number(cellValue);
                                if (!isNaN(temp)) {
                                    cellValueAsNumber = temp;
                                }

                                // Add the alignment class for cells containing numbers only.
                                if (typeof cellValueAsNumber === 'number') {
                                    $td.addClass('align-right');
                                }
                            });
                        }
                    });

                    //
                    // End B-99599
                    //

                    // Invoke after data bind event if specified
                    if (optionalFieldUIGrid.afterDataBindEvent) {
                       optionalFieldUIGrid.afterDataBindEvent(e);
                    }
                }

                $("#" + optionalFieldUIGrid.selectAllCheckId).prop("checked", false);
                sg.controls.enable("#" + optionalFieldUIGrid.btnAddLineId);

                if (optionalFieldUIGrid.isReadOnly || optionalFieldUIGrid.disableButtons) {
                    optionalFieldUIGrid.disableGridButtons(false);
                }
                if (showSettings) {
                    gridColConfig.registerSettingsEvent();
                }
            },

            columnReorder: function (e) {
                GridPreferencesHelper.saveColumnOrder(e, '#' + optionalFieldUIGrid.gridId, optionalFieldUIGrid.preferencesTypeId);
            },

            columns: [
                gridColConfig.getColumn(optionalFieldColumnName.Delete, false, "", "first-cell", gridColConfig.getCheckTemplate(false), gridColConfig.getCheckTemplate(true), gridColConfig.checkboxEditor),
                    { field: optionalFieldColumnName.SerialNumber, hidden: true, attributes: { sg_Customizable: false } },
                gridColConfig.getColumn(optionalFieldColumnName.OptionalField, false, optionalFieldsResources.optionalFieldTitle, "w200", null, null, gridColConfig.optionalFieldEditor),
                gridColConfig.getColumn(optionalFieldColumnName.Settings, !showSettings, optionalFieldsResources.settingsTitle, "w90", gridColConfig.getColTemplate(optionalFieldColumnName.Settings, !showSettings), null, gridColConfig.settingsEditor),
                gridColConfig.getColumn(optionalFieldColumnName.OptionalFieldDescription, false, optionalFieldsResources.optionalFieldDescriptionTitle, "w220", null, null, gridColConfig.noEditor),
                gridColConfig.getColumn(isValueSet ? optionalFieldColumnName.ValueSet : optionalFieldColumnName.ValueSetString, false, optionalFieldsResources.valueSetTitle, "w120", gridColConfig.getColTemplate(isValueSet ? optionalFieldColumnName.ValueSet : optionalFieldColumnName.ValueSetString, false), null, gridColConfig.valueSetEditor),
                gridColConfig.getColumn(isDefault ? optionalFieldColumnName.DefaultValue : optionalFieldColumnName.Value, false, isDefault ? optionalFieldsResources.defaultValueTitle : optionalFieldsResources.valueTitle, "w160", gridColConfig.getColTemplate(isDefault ? "DefaultValue" : "Value", null), null, gridColConfig.valueEditor),
                gridColConfig.getColumn(isDefaultValueDesc ? optionalFieldColumnName.DefaultValueDescription : optionalFieldColumnName.ValueDescription, false, optionalFieldsResources.valueDescriptionTitle, "w160", null, null, gridColConfig.noEditor),
                gridColConfig.getColumn(optionalFieldColumnName.Required, !isDefault, optionalFieldsResources.requiredTitle, "w120", gridColConfig.getColTemplate(optionalFieldColumnName.Required, !isDefault), null, gridColConfig.dropdownListEditor),
                gridColConfig.getColumn(optionalFieldColumnName.AutoInsert, !isDefault, optionalFieldsResources.autoInsertTitle, "w120", gridColConfig.getColTemplate(optionalFieldColumnName.AutoInsert, !isDefault), null, gridColConfig.dropdownListEditor),
                    { field: optionalFieldColumnName.IsNewLine, hidden: true, attributes: { sg_Customizable: false } },
                    { field: optionalFieldColumnName.LineNumber, hidden: true, attributes: { sg_Customizable: false } },
                    { field: optionalFieldColumnName.Type, hidden: true, attributes: { sg_Customizable: false } },
                    { field: optionalFieldColumnName.IsDeleted, hidden: true, attributes: { sg_Customizable: false } },
            ]
        };
    }
};

