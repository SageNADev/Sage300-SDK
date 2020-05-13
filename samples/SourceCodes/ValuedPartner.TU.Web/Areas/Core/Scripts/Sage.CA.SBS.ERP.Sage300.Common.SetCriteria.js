/* globals globalResource: false */
/* globals kendo: false */
/* globals exportModelData: false */

"use strict";

var setCriteriaUI = setCriteriaUI || {};

setCriteriaUI = {
    columns: [],
    criteria: "",
    currentCol: null,

    getExpression: function (operator, value, type) {
        var expr;
        switch (type) {
            case "Char":
                if (value.substring(0, 1) !== '"') {
                    value = value.replace(/\"/g, "");
                    value = '"' + value + '"';
                }
                break;
            case "Date":
                value = $.datepicker.formatDate('yymmdd', new Date(value)); 
                break;
            case "Time":
                value = new Date(value).toLocaleTimeString();
                break;
        }
        
        switch (operator) {
            case 'Contains':
                expr = 'LIKE "%' + value + '%"';
                break;
            case 'Starts with':
                expr = 'LIKE "' + value + '%"';
                break;
            case 'Ends with':
                expr = 'LIKE "%' + value + '"';
                break;
            case 'LIKE':
                if (value.indexOf("%") === -1) {
                    value = (value.lastIndexOf('"') === value.length - 1) ? value.slice(0, -1) + '%"' : value +"%" ;
                }
                expr = operator + " " + value;
                break;
            default:
                expr = operator + " " + value;
                break;
        }
        return expr;
    },

    getCriteria: function (data) {
        var gridColumns = $("#SetCriteriaGrid").data("kendoGrid").columns;
        var length = data.length;
        var row;
        var strCriteria = "";
        for (var i = 0; i < length; i++) {
            row = data[i];
            var props = Object.keys(row);
            var cols = props.filter(function (p) { return p.indexOf("field") > -1; });
            var len = cols.length;
            var cell, c, colName;
            if (len > 0) {
                cols.sort();
                strCriteria += (i > 0) ? " OR (" : "(";
                for (var j = 0; j < len; j++) {
                    colName = cols[j];
                    var hidedenCols = gridColumns.filter(function (c) { return c.field === colName; });
                    var colHidden = hidedenCols.length > 0 ? hidedenCols[0].hidden : false;
                    cell = row[colName];
                    if (cell === globalResource.ClickAddFilter || colHidden) {
                        continue;
                    }
                    if (j === 0) {
                        c = cell.replace("AND", "");
                    } else {
                        c = " " + cell;
                    }
                    strCriteria += c;
                }
                strCriteria += " )";
            }
        }

        strCriteria = strCriteria.replace(/OR \( \)/g, '');
        strCriteria = strCriteria.replace(/\( +AND/g, '\(');
        strCriteria = strCriteria.replace(/\( +\)/g, '');
        strCriteria = strCriteria.replace(/(OR +){2,}/g, " OR ");
        strCriteria = strCriteria.replace(/ +/g, " ");
        strCriteria = strCriteria.trim();
        if (strCriteria.substring(0, 2) === "OR") {
            strCriteria = strCriteria.substring(2).trim();
        }
        if (strCriteria.substring(strCriteria.length - 2) === "OR") {
            strCriteria = strCriteria.substring(0, strCriteria.length - 2).trim();
        }

        return strCriteria;
    },

    refreshResults : function(data, criteria) {
        var strCriteria = (criteria !== undefined) ? criteria : setCriteriaUI.getCriteria(data);
        strCriteria = (strCriteria.length < 5) ? "" : strCriteria;
        setCriteriaUI.criteria = strCriteria;
        exportModelData.ExportRequest.DataMigrationList[0].FilterString = strCriteria;
        sg.exportHelper.exportModel.ExportRequest.DataMigrationList()[0].FilterString(strCriteria);
        sg.exportHelper.needRefresh = true;
    },

    init: function (data, textField, valueField) {
        setCriteriaUI.initButtons();
        setCriteriaUI.initTextbox();
        setCriteriaUI.initNumericTextBox();
        setCriteriaUI.initDropDownList(data, textField, valueField);
        setCriteriaUI.initDatePicker();
        setCriteriaUI.initGrid([], []);
        setCriteriaUI.initGridCell();
    },

    initButtons: function () {
        function InitGridColumn(cols) {
            var col = {};
            col.attributes = { "class": "w50" };
            col.headerAttributes = { "class": "w50" };
            col.title = " ";
            col.field = "ColumnOR";
            cols.push(col);
        }

        $("#btnClearCriteria").hide();
        $("#btnShowCriteria").hide();

        $("#btnAddColumn").bind('click', function () {
            var grid = $("#SetCriteriaGrid").data("kendoGrid");
            var dropdown = $("#ColumnDropdown").data("kendoDropDownList");
            var headerHtml = '<span>' + dropdown.text() + '</span><div class="edit-gridcell-options"><a aria-hidden="true" data-tag="garbage" data-name="garbage" data-code=75" class="glyphicon glyphicon-garbage"></a></div> ';
            var dropdownData = dropdown.dataSource.data();
            var ds = grid.dataSource;
            var data = ds.data().slice();
            var cols = grid.columns.slice();
            var col = {};
            var newData = {};
            
            if (cols.length === 0) {
                InitGridColumn(cols);
            } else {
                cols.pop();
            }

            //Add field column
            col = {};
            col.attributes = { "class": "gridcell add-criteria" } ;
            col.attributes["data-line"] = globalResource.AddCriteria;
            col.headerAttributes = { "class": "w200" };
            col.width = 200;
            col.headerTemplate = headerHtml;
            col.title = dropdown.text();
            col.name = dropdown.value();
            col.field = "field" + cols.length.toString();
            var fieldCol = dropdownData.filter(function (r) { return r.columnName === col.name; })[0];
            col.dataType = (fieldCol.PresentationType === 76 && fieldCol.PresentationList) ? "Enum" : fieldCol.dataType;
            col.precision = fieldCol.Precision;
            col.mask = fieldCol.PresentationMask;
            col.size = fieldCol.Size;
            cols.push(col);
            cols.push({});

            //Prepare for data source data
            var objRow = (data.length === 0) ? newData : jQuery.extend(data[0], newData);
            if (data.length === 0) {
                data.push(objRow);
            } else {
                data[0] = objRow;
            }
            var len = 10 - data.length;
            var objOR = {};
            for (var i = 0; i < len; i++) {
                objOR.ColumnOR = "OR";
                data.push(objOR);
            }
            //rebind the grid
            grid.thead.remove();
            grid.destroy();
            setCriteriaUI.initGrid(data, cols);
        });

        $("#btnDeleteColumn").bind('click', function () {
            var grid = $("#SetCriteriaGrid").data("kendoGrid");
            var colIndex = 1;
            grid.hideColumn(colIndex);
            setCriteriaUI.refreshResults(grid.dataSource.data());
        });

        $("#btnClearCriteria").bind('click', function () {
            var grid = $("#SetCriteriaGrid").data("kendoGrid");
            grid.thead.remove();
            grid.destroy();
            setCriteriaUI.initGrid([], []);
            setCriteriaUI.refreshResults(null,"");
        });

        $("#btnShowCriteria").bind('click', function () {
            $("#txtCriteriaId").val(setCriteriaUI.criteria);
            $('#CriteriaStringPopup').toggleClass('show');
        });

        $("#btnExpOk").off('click');
        $("#btnExpOk").bind('click', function () {
            $("#CriteriaExprId").data("kendoWindow").close();
            var numberTypes = ["Int", "Byte", "Long", "Bool", "Real", "Decimal"];
            var grid = $("#SetCriteriaGrid").data("kendoGrid");
            var optdropdown = $("#ExpOpeartorDropdown").data("kendoDropDownList");
            var data = grid.dataSource.data();
            var selected = grid.select();
            var rowIndex = selected.closest("tr").index();
            rowIndex = (rowIndex < 0) ? 0 : rowIndex;
            var colIndex = grid.cellIndex(grid.select());
            colIndex = (colIndex < 0) ? 0 : colIndex;
            
            var field = "field" + colIndex;
            var dataType = grid.columns[colIndex].dataType;
            var operator = optdropdown.text();
            var fieldName = grid.columns[colIndex].name;
            var mask = grid.columns[colIndex].mask;
            var isFirstVisibleCol = true;

            //Check whether current column is first visible column
            for (var i = 1; i < colIndex; i++) {
                if (!grid.columns[i].hidden) {
                    isFirstVisibleCol = false;
                    break;
                }
            }
            var exprAnd = (colIndex === 1 || isFirstVisibleCol) ? "" : "AND";
            var value;
            if (dataType === "Char") {
                value = getMaskValue($("#valueTextBox").val(), mask);
            }

            if (numberTypes.indexOf(dataType) > -1) {
                value = $("#numberValueTextBox").data("kendoNumericTextBox").value();
            } 
            if (dataType === "Date" || dataType === "Time") {
                value = getValidDate($("#dateValueTextBox").val());
            }
            if ($('#chkExpField').is(':checked')) {
                value = $("#ExpColumnDropdown").val();
                dataType = "field";
            }
            if( dataType ==="Enum") {
                value = $("#EnumValueDropdown").val();
            }
            
            var expr = exprAnd + " " + fieldName + " " + setCriteriaUI.getExpression(operator, value, dataType);
            data[rowIndex][field] = expr;
            grid.refresh();
            if (expr) {
                $(selected.closest("td")).attr('data-line','');
            }
            setCriteriaUI.refreshResults(data);
        });

        $("#btnExpCancel").off('click');
        $("#btnExpCancel").bind('click', function () {
            $("#CriteriaExprId").data("kendoWindow").close();
        });

        function getMaskValue(value, mask) {
            value = value.replace(/\"/g, "");
            if ($("#valueTextBox").hasClass("txt-upper")) {
                value = value.toUpperCase();
            }
            if (mask) {
                if (mask.indexOf("d") > -1 || mask.indexOf("D") > -1) {
                    if (value && mask.length > 2) {
                        var c = mask.charAt(1);
                        var len = mask.charAt(2);
                        var length = parseInt(len) + 1 - value.length;
                        if (c === '0' && !isNaN(length) && length > 0) {
                            value = Array(length).join(c) + value;
                        }
                    }
                }
            }
            value = '"' + value + '"';
            return value;
        }

        function getValidDate(value) {
            if (value.indexOf("/") === -1 && value.length > 6) {
                value = value.substring(0, 4) + "/" + value.substring(4, 6) + "/" + value.substring(6);
            }
            var parsedDate = Date.parse(value);
            var isValid = (new Date(value) !== "Invalid Date") && !isNaN(parsedDate);
            if (!isValid) {
                value = $.datepicker.formatDate('yymmdd', new Date());
                $("#dateValueTextBox").val(value);
            }
            return value;
        }
    },

    initTextbox: function () {
        $("#valueTextBox").on("change", function () {
            $("#btnExpOk").prop('disabled', this.value.length === 0);
        });

        $("#valueTextBox").on("keypress", function () {
            $("#btnExpOk").prop('disabled', false);
        });
    },      

    maskTextBox: function (col) {
        var txtBox = $("#valueTextBox");
        var value = txtBox.val();
        var mask = col.mask;
        txtBox.attr("maxLength", col.size);
        txtBox.attr("class", "");
        txtBox.attr("formatTextbox", "");

        if (mask) {
            var isA = mask.indexOf("A") > -1;
            var isa = mask.indexOf("a") > -1;
            var isN = mask.indexOf("N") > -1;
            var isn = mask.indexOf("n") > -1;
            var isC = mask.indexOf("C") > -1;
            var isD = mask.indexOf("D") > -1;
            var isd = mask.indexOf("d") > -1;

            if (isA || isC || isN) {
                txtBox.attr("class", "txt-upper");
            }

            if (isA || isa) {
                txtBox.attr("formatTextbox", "alpha");
            }

            if (isN || isn) {
                txtBox.attr("formatTextbox", "alphaNumeric");
            }
            if (isD || isd) {
                txtBox.attr("formatTextbox", "numeric");
                if ( !value && mask.length > 2) {
                    var c = mask.charAt(1);
                    var len = mask.charAt(2);
                    var length = parseInt(len) + 1;
                    if (c === '0' && !isNaN(length) && length > 0 ) {
                        txtBox.val(Array(length).join(c));
                    }
                }
            }
        }
    },

    initNumericTextBox: function (type, precision) {
        var max = 9999999;
        var min = -9999999;
        var n = (precision !== undefined) ? precision : 0 ;
        if (type === "Bool") {
            max = 1;
            min = 0;
            n = 0;
        } else if (type === "Byte") {
            max = 255;
            min = 0;
            n = 0;
        } else if (type === "Long" || type === "Int") {
            max = 9999999;
            min = -9999999;
            n = 0;
        }


        $("#numberValueTextBox").kendoNumericTextBox({
            spinners: false,
            format: "n" + n,
            decimals: n,
            max: max,
            nin: min,
            change: function () {
                $("#btnExpOk").prop('disabled', false);
            }
        });
    },

    initDropDownList: function (data, textField, valueField) {
        $("#ColumnDropdown").kendoDropDownList({
            dataTextField: textField,
            dataValueField: valueField,
            dataSource:data,
            change: function () {
            }
        });
        $("#ExpOpeartorDropdown").kendoDropDownList({
            dataSource: [],
            change: function () {
            }
        });
        $("#ExpColumnDropdown").kendoDropDownList({
            dataTextField: textField,
            dataValueField: valueField,
            dataSource: data,
            change: function () {
                var field = this.dataSource.data()[this.selectedIndex];
                var dataType = (field.PresentationType === 76) ? "Enum" : field.dataType;
                var colType = setCriteriaUI.currentCol.dataType;
                $("#btnExpOk").prop('disabled', dataType !== colType);
            }
        });

        $("#EnumValueDropdown").kendoDropDownList({
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: [],
            change: function () {
            }
        });
    },

    initDatePicker: function () {
        $("#dateValueTextBox").kendoDatePicker({
            format: "yyyy/MM/dd",
            change: function () {
                $("#btnExpOk").prop('disabled', false);
            }
        });
        $("#dateValueTextBox").attr("formatTextbox", "date");
        $("#dateValueTextBox").attr("maxlength", "10");
        //$("#dateValueTextBox").attr("readonly", true);
    },

    initGrid: function (data, cols) {
        var ds = new kendo.data.DataSource({
            data: data
        });
        if (data.length === 0) {
            setCriteriaUI.criteria = "";
        }
        $("#SetCriteriaGrid").kendoGrid({
            dataSource: ds,
            height: 360,
            resizable: true,
            selectable: "cell",
            scrollable: true,
            columns: cols,
            dataBound: OnDataBound,
            change: function (e) {
                var length = e.sender.columns.length;
                var colIndex = e.sender.select().index();
                if (colIndex === 0 || colIndex === length -1) {
                    return;
                }
                var title = e.sender.columns[colIndex].title;
                title = globalResource.CreateFilterPopUpTitle.replace('{0}', "'" + title + "'");
                setCriteriaUI.showPopupWindow("#CriteriaExprId", title, true, colIndex, 720);
            },
        });

        $("#SetCriteriaGrid").click(function () {
            $("#CriteriaStringPopup").removeClass('show');
        });

        function OnDataBound() {
            var data = this._data;
            var length = data.length;
            var buttonHtml = '<div class="edit-gridcell-options"><a aria-hidden="true" data-tag="pencil" data-name="pencil" data-code="118" class="glyphicon glyphicon-pencil"></a><a aria-hidden="true" data-tag="garbage" data-name="garbage" data-code=75" class="glyphicon glyphicon-garbage"></a></div>';
            for (var i = 0; i < length; i++) {
                var dataItem = data[i];
                var tr = $("#SetCriteriaGrid").find("[data-uid='" + dataItem.uid + "']");
                if (tr.length > 0) {
                    var row = tr[0];
                    var len = row.cells.length;
                    var cells = row.cells;
                    for (var j = 0; j < len; j++) {
                        var cell = cells[j];
                        var text = cell.textContent.trim();
                        var html = cell.outerHTML;
                        if (text === "OR") {
                            html = html.replace("OR", '<span class="filter-txt-or">OR</span>');
                            cell.outerHTML = html;
                        }
                        if (text.length > 4) {
                            if (text.substring(0, 3) === "AND") {
                                html = html.replace("AND", '<span class="filter-txt-and">AND</span>');
                            }
                            if (html.indexOf("edit-gridcell-options") === -1) {
                                html = html.replace("</td>", buttonHtml + "</td>");
                            }
                            cell.outerHTML = html;
                        }
                    }
                }
            }
            setCriteriaUI.initGridCell();
        }
    },

    initGridCell: function () {
        var $grid = $("#SetCriteriaGrid");
        var gridCell = $grid.find(".gridcell");
        gridCell.off("hover");
        gridCell.on("mouseenter mouseleave", function () {
            var td = $(this).closest("td");
            var cellText = td.text().trim();
            if (cellText) {
                td.attr('data-line','');
            }
            $(this).children('.edit-gridcell-options').toggleClass('show');
        });

        var gridHeader = $grid.find(".k-header");
        gridHeader.off("hover");
        gridHeader.on("mouseenter mouseleave", function () {
            $(this).children('.edit-gridcell-options').toggleClass('show');
        });

        var btnCellDelete = $grid.find(".k-grid-content .glyphicon.glyphicon-garbage");
        btnCellDelete.off("click");
        btnCellDelete.click(function () {
            var grid = $grid.data("kendoGrid");
            var data = grid.dataSource.data();
            var rowIndex = $(this).closest("tr").index();
            rowIndex = (rowIndex < 0) ? 0 : rowIndex;
            var colIndex = $(this).closest("td").index();
            colIndex = (colIndex < 0) ? 0 : colIndex;
            var field = "field" + colIndex;
            data[rowIndex][field] = "";
            grid.refresh();
            $(this).closest("td").attr('data-line', globalResource.AddCriteria);
            setCriteriaUI.refreshResults(data);
        });

        var btnHeaderDelete = $grid.find(".k-grid-header .glyphicon.glyphicon-garbage");
        btnHeaderDelete.off("click");
        btnHeaderDelete.click(function () {
            var grid = $grid.data("kendoGrid");
            var data = grid.dataSource.data();
            var colIndex = $(this).closest("th").index();

            //Get the first visible column            
            var cols = grid.columns;
            var length = cols.length;
            for (var i = 1; i < length; i++) {
                if (!grid.columns[i].hidden) {
                    break;
                }
            }

            //Remove the first visible column cell value expressions starts "AND"
            if (colIndex === i) {
                do {
                    i++;
                } while (grid.columns[i].hidden && i < length);
                length = data.length;
                var field = "field" + i;
                for (var j = 0; j < length; j++) {
                    var value = data[j][field];
                    if (value && value.length > 4 && value.substring(0,3) ==="AND") {
                        value = value.replace("AND ", "");
                        data[j][field] = value;
                    }
                }
                grid.refresh();
            }

            grid.hideColumn(colIndex);
            setCriteriaUI.refreshResults(data);
        });

        var btnCellEdit = $grid.find(".glyphicon.glyphicon-pencil");
        btnCellEdit.off("click");
        btnCellEdit.click(function () {
            var grid = $grid.data("kendoGrid");
            var length = grid.columns.length;
            var rowIndex = $(this).closest("tr").index();
            var colIndex = $(this).closest("td").index();
            if (colIndex === 0 || colIndex === length - 1) {
                return;
            }
            var cell = grid.tbody.find(">tr").eq(rowIndex).find(">td:nth-child(" + (colIndex + 1) + ")");
            if (cell) {
                grid.select(cell);
            }
            var title = grid.columns[colIndex].title;
            title = globalResource.CreateFilterPopUpTitle.replace('{0}', "'" + title + "'");
            setCriteriaUI.showPopupWindow("#CriteriaExprId", title, true, colIndex, 720);
        });
    },

    showPopupWindow: function (id, title, isExprWin, colIndex, width) {
        $("#ExpColumnDropdown").closest(".k-widget").hide();
        var w = (isExprWin) ? width : 800;
        var h = (isExprWin) ? 194 : 388;
        if (isExprWin) {
            setCriteriaUI.initExprWindow(colIndex);
        }

        var popupDialog = $(id).kendoWindow({
            modal: true,
            title: title,
            draggable: true,
            navigatable: true,
            width: w,
            height: h,
            minHeight: 100,
            maxHeight: 800,
            //Open Kendo Window in center of the Viewport. Also set title bar color
            open: sg.utls.kndoUI.onOpen,
            activate: sg.utls.kndoUI.onActivate,
        }).data("kendoWindow") ;
        popupDialog.title(title);
        popupDialog.open();
    },

    getExprObj : function(expr) {
        var operators = ["!=", ">=", "<=", "LIKE", "=", ">", "<"];
        var opt = "=";
        var contains = false;
        var value = "";
        var expObj = {};
        var index = -1;

        for (var i = 0; i < 7; i++) {
            opt = operators[i];
            index = expr.indexOf(opt);
            if (index > -1) {
                contains = true;
                break;
            }
        }
        if (contains) {
            if (opt === "LIKE") {
                value = expr.substring(index + 4);
            } else if (["!=", ">=", "<="].indexOf(opt) > -1) {
                value = expr.substring(index + 2);
            } else {
                value = expr.substring(index + 1);
            }
        }
        expObj.Operator = opt;
        expObj.Value = value.trim().replace("/\"/g", "");
        return expObj;
    },

    initExprWindow: function (colIndex) {
        //hide all input controls
        function hideInputControls(input, numberInput, dateInput, enumInput) {
            input.hide();
            numberInput.closest(".k-widget").hide();
            dateInput.closest(".k-widget").hide();
            enumInput.closest(".k-widget").hide();
        }

        var grid = $("#SetCriteriaGrid").data("kendoGrid");
        var col = grid.columns[colIndex];
        var type = col.dataType;
        var numbers = ["Int", "Byte", "Long", "Bool", "Real", "Decimal"];
        var strOperator = ["=", "!=", ">", ">=", "<", "<=", "LIKE"];
        var operators = ["=", "!=", ">", ">=", "<", "<="];
        var input = $("#valueTextBox");
        var numberInput = $("#numberValueTextBox");
        var dateInput = $("#dateValueTextBox");
        var columnInput = $("#ExpColumnDropdown");
        var enumInput = $("#EnumValueDropdown");
        var optDropdown = $("#ExpOpeartorDropdown").data("kendoDropDownList");
        var btnExpOk = $("#btnExpOk");
        var chkExpField = $('#chkExpField');
        var expObj = { Operator: "=", Value: "" };

        setCriteriaUI.currentCol = col;
        btnExpOk.prop('disabled', (type === "Char") ? true : false);
        chkExpField.prop('checked', false);
        chkExpField.removeAttr('checked').applyCheckboxStyle();

        //set input init value
        var cellText = $(grid.select()).text().trim();
        if (cellText) {
            expObj = setCriteriaUI.getExprObj(cellText);
            if (type === "Char") {
                input.val(expObj.Value);
            }
            if (numbers.indexOf(type) > -1) {
                numberInput.val(expObj.Value);
            }
            if (type === "Date") {
                dateInput.val(kendo.toString(expObj.Value, "yyyy/MM/dd"));
            }
            btnExpOk.prop('disabled', false);

        } else {
            input.val("");
            numberInput.val(0);
            dateInput.val(kendo.toString(Date(), "yyyy/MM/dd"));
        }

        hideInputControls(input, numberInput, dateInput, enumInput);

        //show/hide input controls based on type
        if (type === "Char") {
            input.show();
            operators = strOperator;
            setCriteriaUI.maskTextBox(col);
        }
        if (numbers.indexOf(type) > -1) {
            numberInput.closest(".k-widget").show();
            setCriteriaUI.initNumericTextBox(type, col.precision);
            if (type === "Bool") {
                operators = ["="];
            }
        }
        if (type === "Date" || type === "Time") {
            dateInput.closest(".k-widget").show();
        }
        if (type === "Enum") {
            var enumDropdown = enumInput.data("kendoDropDownList");
            var colDropDown = $("#ColumnDropdown").data("kendoDropDownList");
            var colDropdownData = colDropDown.dataSource.data();
            enumInput.closest(".k-widget").show();
            operators = ["="];
            btnExpOk.prop('disabled', false);
            var fieldCol = colDropdownData.filter(function (r) { return r.columnName === col.name; })[0];
            enumDropdown.setDataSource(fieldCol.PresentationList);
            if (expObj.Value) {
                enumDropdown.value(expObj.Value);
            } else {
                enumDropdown.select(0);
            }
        }
        //set operator dropdown and selected 
        optDropdown.setDataSource(operators);
        var optIndex = operators.indexOf(expObj.Operator);
        optDropdown.select(optIndex);

        //bind filed check box handler
        chkExpField.off();
        chkExpField.on('change', function () {
            var columnDropdown = columnInput.data("kendoDropDownList");
            hideInputControls(input, numberInput, dateInput, enumInput);
            if (this.checked) {
                columnInput.closest(".k-widget").show();
                columnDropdown.select(0);
                optDropdown.setDataSource(["=", "!=", ">", ">=", "<", "<="]);
                optDropdown.select(0);
                btnExpOk.prop('disabled', true);
                columnDropdown.trigger("change");
            } else {
                if (type === "Char") {
                    input.show();
                }
                if (numbers.indexOf(type) > -1) {
                    numberInput.closest(".k-widget").show();
                }
                if (type === "Date" || type === "Time") {
                    dateInput.closest(".k-widget").show();
                }
                if (type === "Enum") {
                    enumInput.closest(".k-widget").show();
                }
                columnInput.closest(".k-widget").hide();
            }
        });
    }        
};

$(function () {
});
