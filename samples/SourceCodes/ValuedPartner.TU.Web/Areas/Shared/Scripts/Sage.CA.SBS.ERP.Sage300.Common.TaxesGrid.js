"use strict";
var taxesColumnName = {

    TaxAuthortiy: "TaxAuthority",
    Description: "Description",
    TaxClass: "TaxClass",
    TaxIncluded: "TaxIncluded",
    TaxBase: "TaxBase",
    TaxAmount: "TaxAmount",
    TaxReportingAmount: "TaxReportingAmount"
};


var gridColConfig = {

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
    },
};


var taxGridUI = {
    getFormattedValue: function (fieldValue, decimal) {
        if (fieldValue != null)
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(!isNaN(parseFloat(fieldValue)) ? parseFloat(fieldValue) : 0, decimal);
        else {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(0, decimal);
        }
        return fieldValue;
    },

    init: function (params, initialize) {

        taxGridUI.gridId = params.gridId;
        taxGridUI.modelData = params.modelData;
        taxGridUI.currentRowTaxGroup = params.currentRowTaxGroup;
        taxGridUI.btnEditColumnsId = params.btnEditColumnsId;
        taxGridUI.preferencesTypeId = params.preferencesTypeId;
        taxGridUI.taxeschange = params.taxeschange;
        taxGridUI.taxamounteditable = params.taxamounteditable;
        taxGridUI.taxbaseeditable = params.taxbaseeditable;
        taxGridUI.taxreporteditable = params.taxreporteditable;
        taxGridUI.taxfromcurrency = params.taxfromcurrency;
        taxGridUI.taxtocurrency = params.taxtocurrency;
        taxGridUI.isReadOnly = params.isReadOnly;
        taxGridUI.classType = params.classType;
        taxGridUI.transactionType = params.transactionType
        if (initialize) {
            taxGridUI.initButton();
        }
    },
    gridId: "",
    btnEditColumnsId: "",
    modelData: null,
    currentRowTaxGroup: null,
    preferencesTypeId: null,
    defaultColumns: null,
    taxeschange: null,
    taxamounteditable: false,
    taxbaseeditable: false,
    taxreporteditable: false,
    taxfromcurrency: null,
    taxtocurrency: null,
    isReadOnly: false,
    classTypeEnum: {
        Customers: 1,
        Items: 2
    },
    transactionTypeEnum: {
        Sales: 1,
        Purchases: 2
    },
    initButton: function () {

        var grid = $('#' + taxGridUI.gridId).data("kendoGrid");
        taxGridUI.defaultColumns = $.extend(true, {}, grid.columns);
        taxGridUI.defaultColumns.length = grid.columns.length;

        $('#' + taxGridUI.btnEditColumnsId).on('click', function () {
            GridPreferences.initialize('#' + taxGridUI.gridId, taxGridUI.preferencesTypeId, $(this), taxGridUI.defaultColumns);
        });
    },

    rcColumns: [
        {
            field: "ReverseChargeBaseAmt",
            title: taxesGridResources.ReverseChargeBase,
            width: "20%",
            attributes: { "class": "align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(ReverseChargeBaseAmt,taxGridUI.taxfromcurrency) #',
            reorderable: false,
            hidden: true,
            editor: function (container, options) {
                if (!taxGridUI.taxamounteditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {
                    var html = '<input id="txtReverseChargeBaseAmt" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.fromcurrency, 16, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.fromcurrency, 16, true);
                    $("#txtReverseChargeBaseAmt").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxfromcurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxfromcurrency,

                    });
                    var txtReverseChargeBaseAmt = $('#txtReverseChargeBaseAmt').data("kendoNumericTextBox");
                    if (txtReverseChargeBaseAmt) {
                        sg.utls.kndoUI.restrictDecimals(txtReverseChargeBaseAmt, taxGridUI.taxfromcurrency, 13);
                    }
                }
            }
        },
        {
            field: "ReverseChargeAmt",
            title: taxesGridResources.ReverseCharges,
            width: "20%",
            attributes: { "class": "align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(ReverseChargeAmt,taxGridUI.taxfromcurrency) #',
            reorderable: false,
            hidden: true,
            editor: function (container, options) {
                if (!taxGridUI.taxamounteditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {
                    var html = '<input id="txtReverseChargeAmt" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.fromcurrency, 16, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.fromcurrency, 16, true);
                    $("#txtReverseChargeAmt").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxfromcurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxfromcurrency,

                    });
                    var txtReverseChargeAmt = $('#txtReverseChargeAmt').data("kendoNumericTextBox");
                    if (txtReverseChargeAmt) {
                        sg.utls.kndoUI.restrictDecimals(txtReverseChargeAmt, taxGridUI.taxfromcurrency, 13);
                    }
                }
            }
        }],

    whtColumns: [
        {
            field: "TaxWithheldAmt",
            title: taxesGridResources.TaxWithheld,
            width: "20%",
            attributes: { "class": "align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(TaxWithheldAmt,taxGridUI.taxfromcurrency) #',
            reorderable: false,
            editor: function (container, options) {
                if (!taxGridUI.taxamounteditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {
                    var html = '<input id="txtTaxWithheldAmt" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.fromcurrency, 16, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.fromcurrency, 16, true);
                    $("#txtTaxWithheldAmt").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxfromcurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxfromcurrency,

                    });
                    var txtTaxWithheldAmt = $('#txtTaxWithheldAmt').data("kendoNumericTextBox");
                    if (txtTaxWithheldAmt) {
                        sg.utls.kndoUI.restrictDecimals(txtTaxWithheldAmt, taxGridUI.taxfromcurrency, 13);
                    }
                }
            }
        }],

    columns: [
        {
            field: "TaxAuthority",
            attributes: {
                sg_Customizable: false
            },
            reorderable: false,
            hidden: true,
            //reorderable: false
        },
        {
            field: "Description",
            attributes: { "class": "gird_culm_10" },
            headerAttributes: { "class": "gird_culm_10 " },
            title: taxesGridResources.TaxAuthority,
            editor: function (container, options) {
                sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);

            },
        },
        {
            field: "TaxClass",
            width: "20%",
            title: taxesGridResources.TaxClass,
            headerAttributes: { "class": "w310" },
            attributes: { "class": "w310 align-right", customizable: true },
            editor: function (container, options) {
                if (!taxGridUI.isReadOnly) {
                    options.model.TaxClass = options.model.TaxClass != null ? options.model.TaxClass : "";
                    var txtTaxClass = '<input id="txtTaxClass" type="text"  maxlength="2" formatTextbox = "numeric" class="align-right pr25" data-descField="' + options.model.TaxClass + '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="TaxClass"/>';
                    var finderTaxClass = '<input title="Finder" type="button" class="icon btn-search" id="btntaxclassfield"/></div>';
                    var html = txtTaxClass + '' + finderTaxClass;
                    $(html).appendTo(container);

                    // Tax class finder
                    const taxClassesFinderInfo = () => {
                        const property = sg.utls.deepCopy(sg.viewFinderProperties.TX.TaxClasses);
                        const selectedRowData = sg.utls.kndoUI.getSelectedRowData($(`#${taxGridUI.gridId}`).data("kendoGrid"));

                        const authority = selectedRowData.TaxAuthority;
                        const classType = taxGridUI.transactionType;
                        const classAxis = taxGridUI.classType;

                        property.initKeyValues = [authority, classType, classAxis, selectedRowData.TaxClass];
                        property.filter = $.validator.format(property["filterTemplate"], authority, classType, classAxis);
                        return property;
                    };
                    sg.viewFinderHelper.setViewFinder("btntaxclassfield", taxGridUI.OnTaxClassSelection,
                        taxClassesFinderInfo, $.noop);
                } else {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                }
            }

        },
        {
            field: "TaxIncluded",
            width: "20%",
            title: taxesGridResources.TaxIncluded,
            headerAttributes: { "class": "w310 mlm1" },
            attributes: { "class": "w310 mlm1", customizable: true },
            editor: function (container, options) {
                if (!taxGridUI.isReadOnly) {
                    var html = dropDownList.taxExemptDropdown;
                    $(html).appendTo(container)
                        .kendoDropDownList({
                            dataValueField: "Value",
                            dataTextField: "Text",
                            autoBind: false,
                            value: options.model.TaxIncludeString
                        });
                } else {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                }
            }
        },
        {
            field: "TaxBase",
            width: "20%",
            title: taxesGridResources.TaxBase,
            headerAttributes: { "class": "w310 mlm1" },
            attributes: { "class": "w310 mlm1 align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(TaxBase,taxGridUI.taxfromcurrency) #',
            editor: function (container, options) {
                if (!taxGridUI.taxbaseeditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {

                    var html = '<input id="txtTaxBase" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.taxfromcurrency, 16, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.taxfromcurrency, 16, true);
                    if (taxGridUI.taxfromcurrency == null)
                        taxGridUI.taxfromcurrency = "2";
                    $("#txtTaxBase").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxfromcurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxfromcurrency,

                    });
                    var txtbaseAmount = $('#txtTaxBase').data("kendoNumericTextBox");
                    if (txtbaseAmount) {
                        sg.utls.kndoUI.restrictDecimals(txtbaseAmount, taxGridUI.taxfromcurrency, 13);
                    }

                }
            }

        },
        {
            field: "TaxAmount",
            title: taxesGridResources.TaxAmount,
            width: "20%",
            attributes: { "class": "align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(TaxAmount,taxGridUI.taxfromcurrency) #',
            reorderable: false,
            editor: function (container, options) {
                if (!taxGridUI.taxamounteditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {
                    var html = '<input id="txtTaxAmount" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.fromcurrency, 16, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.fromcurrency, 16, true);
                    $("#txtTaxAmount").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxfromcurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxfromcurrency,

                    });
                    var txtTaxAmount = $('#txtTaxAmount').data("kendoNumericTextBox");
                    if (txtTaxAmount) {
                        sg.utls.kndoUI.restrictDecimals(txtTaxAmount, taxGridUI.taxfromcurrency, 13);
                    }
                }
            }
        },
        {
            field: "TaxReportAmount",
            width: "20%",
            title: taxesGridResources.TaxReportAmount,
            attributes: { "class": "align-right", customizable: true },
            template: '#= taxGridUI.getFormattedValue(TaxReportAmount,taxGridUI.taxtocurrency) #',
            reorderable: false,
            hidden: true,
            editor: function (container, options) {
                if (!taxGridUI.taxreporteditable || taxGridUI.isReadOnly) {
                    sg.utls.kndoUI.nonEditable($('#' + taxGridUI.gridId).data("kendoGrid"), container);
                } else {
                    var html = '<input id="txtTaxRptAmount" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
                    $(html).appendTo(container);
                    var minValue = sg.utls.getMinValue("9", taxGridUI.taxtocurrency, 13, true);
                    var maxValue = sg.utls.getMaxVale("9", taxGridUI.taxtocurrency, 13, true);
                    $("#txtTaxRptAmount").kendoNumericTextBox({
                        format: "n" + taxGridUI.taxtocurrency,
                        spinners: false,
                        min: minValue,
                        max: maxValue,
                        decimals: taxGridUI.taxtocurrency,

                    });
                    var txtTaxReportAmount = $('#txtTaxRptAmount').data("kendoNumericTextBox");
                    if (txtTaxReportAmount) {
                        sg.utls.kndoUI.restrictDecimals(txtTaxReportAmount, taxGridUI.taxtocurrency, 13);
                    }
                }
            }
        }
    ],

    OnTaxClassSelection: function (result) {
        if ($('#' + taxGridUI.gridId)) {
            var grid = $('#' + taxGridUI.gridId).data("kendoGrid");
            var gridData = sg.utls.kndoUI.getSelectedRowData(grid);

            if (gridData != undefined) {
                gridData.set("TaxClass", result.CLASS);
            }
        }
    },

    taxGridConfig: function (area, controller, action, gridId, modelName, moduleName, columnList) {
        taxGridUI.gridId = gridId;

        for (var prop in columnList) {
            taxesColumnName[prop] = columnList[prop];
        }

        return {
            autoBind: false,
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
            editable: {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },
            schema: {
                model: {
                    fields: {
                        TaxAuthority: { editable: false }
                    }
                }
            },
            getParam: function () {
                var grid = $('#' + gridId).data("kendoGrid");

                var parameters = {
                    model: ko.mapping.toJS(taxGridUI.modelData),
                    id: taxGridUI.currentRowTaxGroup()
                };
                return parameters;
            },

            edit: function (e) {
                var grid = $('#' + gridId).data("kendoGrid");
                grid.select(e.container.closest("tr"));
            },

            buildGridData: function (successData) {
                var gridData = [];
                if (successData) {
                    //Update the Knockout observable array.
                    ko.mapping.fromJS(successData, {}, successData[modelName]);
                    //Notify the grid which part of the returned data contains the items for the grid. 
                    gridData.data = successData;
                    //Notify the grid which part of the returned data contains the total number of items for the grid. 
                    gridData.totalResultsCount = 6;
                }
                return gridData;
            },


            columnReorder: function (e) {
                GridPreferencesHelper.saveColumnOrder(e, '#' + taxGridUI.gridId, taxGridUI.preferencesTypeId);
            },

            //This is a temp solution in case. we should not hack the code.
            columns: area === 'AP' || area === 'PO' ? taxGridUI.columns.concat(taxGridUI.rcColumns).concat(taxGridUI.whtColumns) : area === 'AR' ? taxGridUI.columns.concat(taxGridUI.whtColumns): taxGridUI.columns,
            dataChange: function (changedData) {
                taxGridUI.taxeschange(changedData);
            }

        };
    }
};
