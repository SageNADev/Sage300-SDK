"use strict";




var salesPersonColumnName = {
    Delete: "Delete",
    IsDeleted: "IsDeleted",
    SeqNo: "SeqNo",
    SalesPerson: "SalesPerson",
    SalesPersonCode: "SalesPersonCode",
    SalesSplitPercentage: "SalesSplitPercentage",
    IsNewLine: "IsNewLine",
}
var SalesPersonNewLine = function (serno) {
    if (serno == 1) {
        var primval = salesSplitGridResources.Primary
    } else { var primval = "" }

    var columns = {
        "SeqNo": serno,
        "Primary": primval,
        "SalesPersonCode": null,
        "SalesPerson": null,
        "SalesSplitPercentage": 0.00000,
        "IsNewLine": true,
        "IsDeleted": false
    };
    return columns;
}

var gridSaleColConfig = {

    checkboxEditor: function (container, options) {
        saleSplitGridUI.SeqNo = options.model.SeqNo;
        sg.utls.kndoUI.nonEditable($('#' + saleSplitGridUI.gridId).data("kendoGrid"), container);
    },
    SalesPersonEditor: function (container, options) {
        if (!saleSplitGridUI.isReadOnly) {
            saleSplitGridUI.SeqNo = options.model.SeqNo;
            var txtsalesPersonCode = '<input id="txtSalespersoncode" type="text"  maxlength="8" formatTextbox ="alphaNumeric" class="txt-upper" data-descField="' + options.model.SalespersonCode + '" name="' + options.field + '" data-bind="value:' + options.field + '" data-filter="SalespersonCode"/>';
            var findersalesPersonCode = '<input title="Finder" type="button" class="icon btn-search" id="btnSalespersonCode"/></div>';
            var html = txtsalesPersonCode + '' + findersalesPersonCode;
            var salespersonFindertitle = jQuery.validator.format(salesSplitGridResources.FinderTitle, salesSplitGridResources.SalesPersonCode);
            $(html).appendTo(container);
            sg.finderHelper.setFinder("btnSalespersonCode", sg.finder.SalesPersonFinder, saleSplitGridUI.OnSalespersonSelection, $.noop, salespersonFindertitle, sg.finderHelper.createDefaultFunction("txtSalespersoncode", "SalesPersonCode", sg.finderOperator.StartsWith), null, true);
        } else {
            gridSaleColConfig.noEditor(container, options)
        }


    },
    percentageEditor: function (container, options) {
        if (!saleSplitGridUI.isReadOnly) {
            var html = '<input id="txtSalespercentage" type="text"  class="align-right"  name="' + options.field + '" data-bind="value:' + options.field + '" />';
            $(html).appendTo(container);
            var minValue = sg.utls.getMinValue("9", 5, 8, true);
            var maxValue = sg.utls.getMaxVale("9", 5, 8, true);
            $("#txtSalespercentage").kendoNumericTextBox({
                format: "n5",
                spinners: false,
                min: minValue,
                max: maxValue,
                decimals: 5,
            });
            var txtsalesper = $('#txtSalespercentage').data("kendoNumericTextBox");
            if (txtsalesper) {
                sg.utls.kndoUI.restrictDecimals(txtsalesper, 5, 3);
            }
        } else {
            gridSaleColConfig.noEditor(container, options)
        }

    },
    noEditor: function (container, options) {
        sg.utls.kndoUI.nonEditable($('#' + saleSplitGridUI.gridId).data("kendoGrid"), container);
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
            editor: editor,
        }
        return column;
    },
    getCheckTemplate: function (isHeader) {
        var id = isHeader ? "selectAllsalChk" : "selectsalChk";
        var template = "<input type='checkbox' " + (isHeader ? "id='" : "class='") + id + "' />";
        return sg.controls.ApplyCheckboxStyle(template);
    },
    getperTemplate: function () {
        var template = null;
        template = '#= saleSplitGridUI.getFormattedValue( SalesSplitPercentage) #';
        return template;

    },
    getCodeTemplate: function () {
        var template = null;
        template = '#= saleSplitGridUI.getFormattedSalesCode( SalesPersonCode) #';
        return template;

    }
}

var saleSplitGridUI = {

    addNewSalesLine: function () {
        var newLineExist = false;
        var btnAddLineId = "btnSalesSplitAddLine";
        var grid = $('#' + saleSplitGridUI.gridId).data("kendoGrid");
        var data = grid.dataSource.data();
        if (data != null && data.length > 0) {
            $.each(data, function (index, item) {
                if ((item.SalesPersonCode === null || item.SalesPersonCode === "") && item.IsDeleted !== true) {
                    newLineExist = true;
                    return false;
                }
                saleSplitGridUI.dataIndex = data.length + 1;
            });
        }
        else {
            saleSplitGridUI.dataIndex = 1;
        }

        var currentRowGrid = sg.utls.kndoUI.getSelectedRowData(grid);
        if ((!newLineExist)) {
            if (saleSplitGridUI.dataIndex <= 5) {

                grid.dataSource.insert(saleSplitGridUI.dataIndex, SalesPersonNewLine(saleSplitGridUI.dataIndex));
                var focusIndex = window.GridPreferencesHelper.getColumnIndex('#' + saleSplitGridUI.gridId, "SalesPersonCode");
                var cell = grid.tbody.find(">tr:eq(" + (saleSplitGridUI.dataIndex-1) + ") >td:eq(" + 3 + ")");
                grid.editCell(cell);
                return true;
            }
            else {
                sg.controls.disable('#' + btnAddLineId);
            }
            sg.controls.enable("#selectSalesAllChk");
        }

    },
    deleteLine: function (gridId, chkAllId, confirmationMsg, btnDeleteId) {
        sg.utls.showKendoConfirmationDialog(
            //Click on Yes
            function () {
                var i = 0;
                var grid = $('#' + gridId).data("kendoGrid");
                var list = [];
                grid.tbody.find(":checked").closest("tr").each(function (index) {
                    i = i + 1;
                    var validSalesPerson = saleSplitGridUI.items.SalesSplitDetail.Items();
                    var salespercode = grid.dataItem($(this)).SeqNo;
                    $.each(validSalesPerson, function (key, value) {
                        if (value.SeqNo() === salespercode && (value.SalesPersonCode() != "" || value.SalesPersonCode() != null)) {
                            list.push(value);
                        }
                    });


                });
                grid.tbody.find(":checked").closest("tr").each(function (index) {
                    var row = $(this);
                        grid.removeRow(row);
                   
                });
                if (i >= 1) {
                saleSplitGridUI.deletesaleperson(ko.mapping.toJS(saleSplitGridUI.items.Data), ko.mapping.toJS(list));
                }


                // ko.mapping.fromJS(list, {}, saleSplitGridUI.items.SalesSplitDetail.Items);
                if (grid.dataSource.total() == 0) {
                    $("#" + chkAllId).attr("checked", false).parent().attr("class", "icon checkBox");
                    sg.controls.disable("#" + chkAllId);
                    $('#message').empty();
                } else {
                    sg.controls.enable("#" + chkAllId);
                }
                sg.controls.disable("#" + btnDeleteId);
            },
            // Click on No
            function () { },
            confirmationMsg, salesSplitGridResources.DeleteTitle);
        return false;
    },
    deleteMsg: function (checkId, deleteClass) {
        var confirmationMsg = null;
        if ($(deleteClass + ':checked').length > 1) {
            confirmationMsg = salesSplitGridResources.DeleteLinesConfirm;
        } else {
            confirmationMsg = salesSplitGridResources.DeleteLineConfirm;
        }
        return confirmationMsg;
    },

    getFormattedSalesCode: function (fieldValue) {
        return fieldValue != null ? fieldValue.toUpperCase() : "";
    },
    getFormattedValue: function (fieldValue) {
        if (fieldValue != null)
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(!isNaN(parseFloat(fieldValue)) ? parseFloat(fieldValue) : 0, 5);
        else {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(0, 5);
        }
        return '<span style="float:right">' + fieldValue + '</span>';
    },

    init: function (params) {


        saleSplitGridUI.gridId = params.gridId,
        saleSplitGridUI.modelData = params.modelData,
        saleSplitGridUI.items = params.items,
        saleSplitGridUI.preferencesTypeId = params.preferencesTypeId;
        saleSplitGridUI.salesperchange = params.salesperchange,
        saleSplitGridUI.deletesaleperson = params.deletesaleperson,
        saleSplitGridUI.initButton();
        saleSplitGridUI.initsalCheckBox();
    },
    gridId: "",
    btnDeleteLineId: "btnSalesSplitDeleteLine",
    btnAddLineId: "btnSalesSplitAddLine",
    modelData: null,
    modelname: null,
    preferencesTypeId: null,
    deletesaleperson: null,
    defaultColumns: null,
    SeqNo: null,
    salesperchange: null,
    items: null,
    viewModel: null,
    dataIndex: 0,
    selectAllChkId: "selectAllsalChk",
    selectChkId: "selectsalChk",
    isReadOnly: false,
    initButton: function () {
        var btnAddLineId = "btnSalesSplitAddLine";
        var btnDeleteLineId = "btnSalesSplitDeleteLine";
        var index = optionalFieldUIGrid.gridIds.indexOf(optionalFieldUIGrid.gridId);

        $('#' + btnAddLineId).on("click", function (e) {
            sg.utls.SyncExecute(saleSplitGridUI.addNewSalesLine);
        });

        $('#' + btnDeleteLineId).on("click", function (e) {
            var confirmationMsg = saleSplitGridUI.deleteMsg("selectsalChk", '.selectsalChk');
            saleSplitGridUI.deleteLine(saleSplitGridUI.gridId, "selectAllsalChk", confirmationMsg, "btnDeleteLineId");
        });

        var grid = $('#' + saleSplitGridUI.gridId).data("kendoGrid");
        saleSplitGridUI.defaultColumns = $.extend(true, {}, grid.columns);
        saleSplitGridUI.defaultColumns.length = grid.columns.length;

    },
    initsalCheckBox: function () {
        var gridId = saleSplitGridUI.gridId;
        var selectAllChk = "selectAllsalChk";
        var selectChk = "selectsalChk";
        var btnDeleteLineId = "btnSalesSplitDeleteLine";

        if ($("#" + gridId)) {
            saleSplitGridUI.selectAllChkId = selectAllChk;
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
    resetFocus: function (dataItem, columnName) {
        if (dataItem !== undefined) {
            var index = GridPreferencesHelper.getColumnIndex('#' + saleSplitGridUI.gridId, columnName);
            var row = sg.utls.kndoUI.getRowForDataItem(dataItem);
            $('#' + saleSplitGridUI.gridId).data("kendoGrid").editCell(row.find(">td").eq(index));
        }
    },
    OnSalespersonSelection: function (result) {
        if ($('#' + saleSplitGridUI.gridId)) {
            var grid = $('#' + saleSplitGridUI.gridId).data("kendoGrid");
            var gridData = sg.utls.kndoUI.getSelectedRowData(grid);

            if (gridData != undefined) {
                gridData.set("SalesPersonCode", result.SalesPersonCode);
                gridData.set("SalesPerson", result.Name);
            }
            saleSplitGridUI.resetFocus(gridData, "SalesPersonCode")
        }
    },
    disableGridButtons: function () {
        var grid = $('#' + saleSplitGridUI.gridId).data("kendoGrid");
        sg.controls.disable('#' + saleSplitGridUI.btnAddLineId);
        sg.controls.disable('#' + saleSplitGridUI.btnDeleteLineId);
        sg.controls.disable("#" + saleSplitGridUI.selectAllChkId);
        $("#" + saleSplitGridUI.selectAllChkId).attr("checked", false).parent().attr("class", "icon checkBox");
        grid.tbody.find("." + saleSplitGridUI.selectChkId).each(function () {
            $(this).attr("disabled", true);
        });
    },
    saleSplitConfig: function (area, controller, action, gridId, modelName) {
        saleSplitGridUI.gridId = gridId;

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
                        Delete: { editable: false }
                    }
                }
            },
            getParam: function () {
                var grid = $('#' + gridId).data("kendoGrid");

                var parameters = {
                    model: ko.mapping.toJS(saleSplitGridUI.modelData)
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

                    var salesData = (successData.Data !== undefined) ? successData.Data[modelName] : successData;

                    $.each(salesData.Items, function (index, value) {
                        var data = value;
                        if (index == 0) {
                            data.Primary = salesSplitGridResources.Primary;
                        }
                    }); ko.mapping.fromJS(salesData, {}, saleSplitGridUI.items[modelName]);
                    gridData.data = salesData.Items

                    gridData.totalResultsCount = salesData.TotalResultsCount; //Notify the grid which part of the returned data contains the total number of items for the grid. 

                }
                return gridData;
            },
            afterDataBind: function (e) {
                if (saleSplitGridUI.gridId !== null) {
                    var grid = $('#' + saleSplitGridUI.gridId).data("kendoGrid");

                    var hasChecked = false;
                    grid.tbody.find("." + saleSplitGridUI.selectChkId).each(function (index) {
                        if ($(this).is(':checked')) {
                            hasChecked = true;
                            return false;
                        } else {
                            return true;
                        }
                    });
                    if (hasChecked) {
                        sg.controls.enable("#" + saleSplitGridUI.btnDeleteLineId);
                    } else {
                        sg.controls.disable("#" + saleSplitGridUI.btnDeleteLineId);
                    }
                    var length = grid.dataSource.data().length;
                    if (length > 0) {
                        sg.controls.enable("#" + saleSplitGridUI.selectAllChkId);
                    } else {
                        sg.controls.disable("#" + saleSplitGridUI.selectAllChkId);
                    }
                }
                $("#" + saleSplitGridUI.selectAllChkId).attr("checked", false).parent().attr("class", "icon checkBox");
                if (grid.dataSource.data().length == 5) {
                    sg.controls.disable("#" + saleSplitGridUI.btnAddLineId);
                } else {
                    sg.controls.enable("#" + saleSplitGridUI.btnAddLineId);
                }

                if (saleSplitGridUI.isReadOnly) {
                    saleSplitGridUI.disableGridButtons(false);
                }
            },

            columnReorder: function (e) {
                GridPreferencesHelper.saveColumnOrder(e, '#' + saleSplitGridUI.gridId, saleSplitGridUI.preferencesTypeId);
            },
            columns: [
                gridSaleColConfig.getColumn(salesPersonColumnName.Delete, false, "", "first-cell", gridSaleColConfig.getCheckTemplate(false), gridSaleColConfig.getCheckTemplate(true), gridColConfig.checkboxEditor),
                { field: salesPersonColumnName.SeqNo, hidden: true, attributes: { sg_Customizable: false } },
                  { field: "Primary", title: "", hidden: false, headerTemplate: '<label for="check-all"></label>', attributes: { sg_Customizable: false }, editor: gridSaleColConfig.noEditor },
                gridSaleColConfig.getColumn(salesPersonColumnName.SalesPersonCode, false, salesSplitGridResources.SalesPersonCode, "", gridSaleColConfig.getCodeTemplate(), null, gridSaleColConfig.SalesPersonEditor),
                gridSaleColConfig.getColumn(salesPersonColumnName.SalesPerson, false, salesSplitGridResources.SalesPerson, "", null, null, gridSaleColConfig.noEditor),
                gridSaleColConfig.getColumn(salesPersonColumnName.SalesSplitPercentage, false, salesSplitGridResources.SalesSplitPercentage, "", gridSaleColConfig.getperTemplate(), null, gridSaleColConfig.percentageEditor),
                { field: salesPersonColumnName.IsNewLine, hidden: true, attributes: { sg_Customizable: false } },
                { field: salesPersonColumnName.IsDeleted, hidden: true, attributes: { sg_Customizable: false } },
            ],
            dataChange: function (changedData) {
                saleSplitGridUI.salesperchange(changedData);
            }

        };
    }
};
