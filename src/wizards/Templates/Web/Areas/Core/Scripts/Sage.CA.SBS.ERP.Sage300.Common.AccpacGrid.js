// Copyright (c) 2018 Sage Software, Inc.  All rights reserved.
"use strict";
var accpacGridLineNumber = 0;
var accpacGrid = function () {

    var accpacGridLastRowStatusEnum = {
        UPDATE: 1,
        INSERT: 2,
        NONE: 3
    };
    var btnTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}">{2}</button>';

    var accpacGridSetDefaultRow = {},
        accpacGridLastRowNumber = {},
        accpacGridLastColField = {},
        accpacGridLastRowStatus = {},
        accpacGridNewLine = {},
        accpacGridLastErrorResult = {},
        accpacGridValid = {},
        accpacGridDataChanged = {},
        accpacGridSkipChange = {},
        accpacGridPageSize = {},
        accpacGridReadOnlyColumns = {},
        accpacGridUniqueGuidId = {};

    function accpacGridToolbarDelete(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        var selectedIndex = grid.select().index();

        if (selectedIndex > -1) {
            sg.utls.showKendoConfirmationDialog(
                // Click yes
                function () {
                    grid.removeRow('tr:eq(' + (selectedIndex + 1) + ')');
                    grid.dataSource.sync();
                    grid.dataSource.read();
                },
                // Click on No
                function () { },
                "Delete this line?", window.DeleteTitle);
        }
    }

    function accpacGridAddLine(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        var dataSource = grid.dataSource;
        var selectedItem = grid.dataItem(grid.select());
        var data = { 'viewID': $("#" + gridName).attr('viewID') };
        var insertedIndex = grid.select().index() + 1;
        var url = sg.utls.url.buildUrl("Core", "Grid", "Create");
        var pageSize = dataSource.pageSize();
        var currentPage = dataSource.page();
        var interval = 0;
        data.Data = selectedItem;
                
        if (insertedIndex === pageSize) {
            insertedIndex = 0;
            dataSource.page(++currentPage);
            interval = 100;
        }
        accpacGridSetDefaultRow[gridName] = false;

        sg.utls.ajaxPostSync(url, data, function (jsonResult) {
            if (jsonResult && jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
                setTimeout(() => {
                    var dataSource = grid.dataSource;
                    accpacGridValid[gridName] = true;
                    // insert the server create blank record to datasource
                    dataSource.insert(insertedIndex, jsonResult.Data);
                    grid.refresh();

                    var currentRowGrid = sg.utls.kndoUI.getRowByKey(dataSource.data(), "KendoGridAccpacViewPrimaryKey", jsonResult.Data["KendoGridAccpacViewPrimaryKey"]);
                    var row = sg.utls.kndoUI.getRowForDataItem(currentRowGrid);
                    grid.select(row);
                    var model = dataSource.options.schema.model;
                    setNextEditCell(grid, model, row, 0);
                    accpacGridLastRowNumber[gridName] = insertedIndex;
                    accpacGridLastRowStatus[gridName] = accpacGridLastRowStatusEnum.INSERT;
                    accpacGridNewLine[gridName] = true;
                }, interval);
            }

            if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                delete accpacGridLastRowNumber[gridName];
                sg.utls.showMessage(jsonResult);
                accpacGridValid[gridName] = false;
            }
        });
    }

    function accpacGridToolbarAdd(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        // set the row status to update as it is not finished yet due to error
        if (accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.UPDATE) {
            setTimeout(function () {
                grid.select("tr:eq(" + accpacGridLastRowNumber[gridName] + ")");
                sg.utls.showMessage(accpacGridLastErrorResult[gridName]);
            });
            return;
        }

        if (accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.INSERT) {
            // check if there is a new line being added. if so, insert the line first
            if (accpacGridLastRowNumber.hasOwnProperty(gridName)) {
                var url = sg.utls.url.buildUrl("Core", "Grid", "Insert");
                var data = { 'viewID': $("#" + gridName).attr('viewID') };
                data.Data = grid._data[accpacGridLastRowNumber[gridName]];

                sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                    // if successful, exit the edit mode, go to next
                    if (jsonResult && jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
                        // end the edit mode for the new line
                        delete accpacGridLastRowNumber[gridName];
                        accpacGridAddLine(gridName);
                        accpacGridValid[gridName] = true;
                    }
                    if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                        sg.utls.showMessage(jsonResult);
                        setEditCell(grid, grid.select().index(), accpacGridLastColField[gridName]);
                        accpacGridValid[gridName] = false;
                    }
                });
            } else {
                accpacGridAddLine(gridName);
            }
        }
        else {
            accpacGridAddLine(gridName);
        }
    }

    function setEditCell(grid, rowIndex, field) {
        var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, field);
        grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
    }

    function setNextEditCell(grid, model, row, startIndex) {
        for (var i = startIndex; i < grid.columns.length; i++) {
            var col = grid.columns[i];
            if (col.field && col.field.hidden !== true) {
                if (model.fields[col.field].editable) {
                    grid.editRow(row);
                    setTimeout(function () {
                        grid.editCell(row.find(">td").eq(i));
                    });
                    break;
                }
            }
        }
    }

    function setGridCell(gridName) {
        let grid = $("#" + gridName).data("kendoGrid");
        let rowIndex = grid.select().index();
        setEditCell(grid, rowIndex, accpacGridLastColField[gridName]);
    }

    function editColumnSettings(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        var btnEditElement = $("#" + gridName + " .k-grid-toolbar .btn-edit-column");
        GridPreferencesHelper.initialize('#' + gridName, accpacGridUniqueGuidId[gridName], $(btnEditElement), grid.columns);
    }

    function receiveMessage(event) {
        var eventData = event.data;
        if (!eventData || eventData.action !== "saveGridActiveRow" && !eventData.gridName) {
            return;
        }
        var gridName = event.data.gridName;
        var grid = $("#" + gridName).data("kendoGrid");
        var data = { 'viewID': $("#" + gridName).attr('viewID') };
        var rowIndex = grid.select().index();
        if (rowIndex < 0) {
            return;
        }
        data.Data = grid._data[rowIndex];
        if (!accpacGridNewLine[gridName]) {
            return;
        }

        var url = sg.utls.url.buildUrl("Core", "Grid", "Insert");
        sg.utls.ajaxPostSync(url, data, function (jsonResult) {
            // if failure, set the focus to the failed cell
            if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                sg.utls.showMessage(jsonResult);
                setEditCell(grid, grid.select().index(), accpacGridLastColField[gridName]);
                accpacGridValid[gridName] = false;
            } else {
                delete accpacGridLastRowNumber[gridName];
                accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.NONE;
                accpacGridValid[gridName] = true;
                accpacGridNewLine[gridName] = false;
            }
        });
    }

    function init(gridName) {
        window.addEventListener("message", receiveMessage, false);
        var model = window[gridName + "Model"];
        var readOnly = model.ReadOnly;
        var columns = getGridColumns(gridName);

        var addTemplate = kendo.format(btnTemplate, 'btn-add', 'accpacGrid.addLine(&quot;' + gridName + '&quot;)', 'Add Line');
        var delTemplate = kendo.format(btnTemplate, 'btn-delete', 'accpacGrid.deleteLine(&quot;' + gridName + '&quot;)', 'Delete Line');
        var editTemplate = kendo.format(btnTemplate, 'btn-edit-column', 'accpacGrid.editColumnSettings(&quot;' + gridName + '&quot;)', 'Column Settings');

        accpacGridUniqueGuidId[gridName] = model.UserPreferencesUniqueId;
        accpacGridSetDefaultRow[gridName] = true;
        accpacGridLastRowNumber[gridName] = -1;
        accpacGridLastColField[gridName] = "";
        accpacGridLastRowStatus[gridName] = accpacGridLastRowStatusEnum.NONE;
        accpacGridNewLine[gridName] = false;
        accpacGridLastErrorResult[gridName] = null;
        accpacGridValid[gridName] = true;
        accpacGridDataChanged[gridName] = false;
        accpacGridSkipChange[gridName] = false;
        accpacGridPageSize[gridName] = model.PageSize || 10;
        accpacGridReadOnlyColumns[gridName] = [];

        $("#" + gridName).kendoGrid({
            height: model.Height || 450,
            columns: columns,
            navigatable: true,
            reorderable: true,
            filterable: false,
            resizable: true,
            selectable: true,
            persistSelection: true,
            editable: readOnly ? false : {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },

            navigate: function(e) {
            },

            change: function (e) {
                var grid = $("#" + gridName).data("kendoGrid");
                var selectedIndex = grid.select().index() % accpacGridPageSize[gridName];

                if (accpacGridSkipChange[gridName]) {
                    accpacGridSkipChange[gridName] = false;
                    return;
                }
                if (accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.UPDATE && selectedIndex !== accpacGridLastRowNumber[gridName]) {
                    setTimeout(function () {
                        grid.select("tr:eq(" + accpacGridLastRowNumber[gridName] + ")");
                        sg.utls.showMessage(accpacGridLastErrorResult[gridName]);
                    });
                }
                else if (accpacGridLastRowNumber.hasOwnProperty(gridName) && accpacGridLastRowNumber[gridName] !== selectedIndex ) {
                    // save the newly added line
                    var url = sg.utls.url.buildUrl("Core", "Grid", "Insert");
                    var data = { 'viewID': $("#" + gridName).attr('viewID') };
                    data.Data = grid._data[accpacGridLastRowNumber[gridName]];

                    sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                        // if failure, set the focus to the previous line
                        if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                            sg.utls.showMessage(jsonResult);
                            var rowIndex = accpacGridLastRowNumber[gridName];
                            var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, accpacGridLastColField[gridName]);
                            grid.select("tr:eq(" + rowIndex + ")");
                            setTimeout(() => {
                                grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
                            });
                            accpacGridValid[gridName] = false;
                        } else { // insert successful. exit the edit mode
                            delete accpacGridLastRowNumber[gridName];
                            accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.NONE;
                            accpacGridNewLine[gridName] = false;
                            accpacGridValid[gridName] = true;
                        }
                    });
                }
            },

            dataBinding: function (e) {
                accpacGridLineNumber = (this.dataSource.page() - 1) * this.dataSource.pageSize();
            },

            dataBound: function (e) {
                //select the first row
                var grid = $("#" + gridName).data("kendoGrid");
                var pageSize = grid.dataSource.pageSize() -1;
                if (accpacGridSetDefaultRow[gridName]) {
                    setTimeout(() => grid.select(e.sender.tbody.find("tr:first")), 10);
                }
                grid.tbody.find("tr:gt(" + pageSize + ")").hide();
            },

            edit: function (e) {
                var readOnlyColumns = accpacGridReadOnlyColumns[gridName];
                var grid = this;
                var column = grid.columns[e.container.index()];
                var field = column.field;
                if (readOnlyColumns.indexOf(field) > -1) {
                    $("#" + column).attr("readonly", true);
                }
            },

            pageable: {
                pageSize: accpacGridPageSize[gridName],
                numeric: false,
                buttonCount: 1,
                input: true
            },

            toolbar: readOnly ? [{ template: editTemplate }] : [{ template: addTemplate }, { template: delTemplate }, { template: editTemplate}],

            dataSource: {
                serverPaging: true,
                serverFiltering: false,
                serverSorting: false,
                schema: {
                    data: "Data",
                    total: "Total",
                    model: { // define the model of the data source. Required for validation and property types.
                        id: "KendoGridAccpacViewPrimaryKey",
                        fields: eval(gridName + "_fields")
                    }
                },
                batch: true, // enable batch editing - changes will be saved when the user clicks the "Save changes" button
                // if any column in the grid changes, we reread from the accpac view associated with the grid and refresh this row in the grid
                change: function (e) {
                    var grid = $("#" + gridName).data("kendoGrid");
                    if (e.items.length === 0 && grid.dataSource.page() !== 1 ) {
                        grid.dataSource.page(1);
                    }
                    if (e.action) {
                        accpacGridDataChanged[gridName] = true;
                    }
                    if (e.action === "itemchange") {
                        // we need to send the changed column to the server side
                        e.preventDefault();
                        var currentRowGrid = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "KendoGridAccpacViewPrimaryKey", e.items[0]["KendoGridAccpacViewPrimaryKey"]);
                        var model = e.items[0];
                        var data = { 'Data': e.items[0], 'viewID': $("#" + gridName).attr('viewID'), 'ChangedField': e.field };
                        var url = sg.utls.url.buildUrl("Core", "Grid", "UpdateCurrentRecord");

                        sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                            if (jsonResult && jsonResult.UserMessage && jsonResult.UserMessage.IsSuccess) {
                                // reset the row status
                                if (accpacGridLastRowStatus[gridName] === accpacGridLastRowStatusEnum.UPDATE) {
                                    accpacGridLastRowStatus[gridName] = accpacGridLastRowStatusEnum.NONE;
                                }
                                delete accpacGridLastErrorResult[gridName];
                                var dataItem = grid.dataSource.get(e.items[0]["KendoGridAccpacViewPrimaryKey"]);

                                for (var field in jsonResult.Data) {
                                    if (field in dataItem) {
                                        dataItem[field] = jsonResult.Data[field];
                                    }
                                }
                                accpacGridValid[gridName] = true;
                                accpacGridSetDefaultRow[gridName] = false;
                                grid.refresh();
                                var index = window.GridPreferencesHelper.getGridColumnIndex(grid, e.field);
                                var row = sg.utls.kndoUI.getRowForDataItem(currentRowGrid);
                                grid.select(row);
                                setNextEditCell(grid, model, row, index + 1);
                            }

                            if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                                // set the row status to update as it is not finished yet due to error
                                accpacGridLastRowStatus[gridName] = accpacGridLastRowStatusEnum.UPDATE;
                                accpacGridLastErrorResult[gridName] = jsonResult;
                                accpacGridLastRowNumber[gridName] = grid.select().index();
                                accpacGridValid[gridName] = false;
                                sg.utls.showMessage(jsonResult);
                                setTimeout(function () {
                                    var index = window.GridPreferencesHelper.getGridColumnIndex(grid, e.field);
                                    var row = sg.utls.kndoUI.getRowForDataItem(currentRowGrid);
                                    grid.editCell(row.find(">td").eq(index));
                                    //Todo : this will trigger another datachange event
                                    //model.set(e.field, "");
                                });
                            }
                        });
                    }
                },

                transport: {
                    read: {
                        url: window.sg.utls.url.buildUrl("Core", "Grid", "Read"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax()
                    },

                    destroy: {
                        url: window.sg.utls.url.buildUrl("Core", "Grid", "Delete"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax()
                    },
                    parameterMap: function (data, operation) {
                        data.viewID = $("#" + gridName).attr('viewID');
                        if (operation === "read") {
                            accpacGridSetDefaultRow[gridName] = true;
                            accpacGridNewLine[gridName] = false;
                            delete accpacGridLastRowNumber[gridName];
                            delete accpacGridLastErrorResult[gridName];
                            accpacGridLastRowStatus[gridName] = accpacGridLastRowStatusEnum.NONE;
                            data.fieldNames = eval(gridName + "_viewFieldNames");
                            return JSON.stringify(data);
                        }
                        else {
                            return JSON.stringify(data);
                        }
                    }
                }
            }
        });

        function pagingHandler(gridName, isClick, that, e) {
            if (accpacGridNewLine[gridName]) {
                var grid = $("#" + gridName).data("kendoGrid");
                var ds = grid.dataSource;
                var page = ds.page();

                e.preventDefault();
                e.stopPropagation();

                if (isClick) {
                    var totalPage = Math.floor(ds.total() / ds.pageSize()) + 1;
                    var element = that.firstChild;
                    var kClass = "k-i-arrow-";
                    if ($(element).hasClass(kClass + "60-left")) {
                        --page;
                    } else if ($(element).hasClass(kClass + "60-right")) {
                        ++page;
                    } else if ($(element).hasClass(kClass + "end-left")) {
                        page = 1;
                    } else if ($(element).hasClass(kClass + "end-right")) {
                        page = totalPage;
                    }
                }
                
                window.postMessage({ action: "saveGridActiveRow", gridName: gridName }, window.location);
                setTimeout(() => {
                    if (accpacGridValid[gridName]) {
                        grid.dataSource.page(page);
                    }
                }, 100);
            }
            if (!accpacGridValid[gridName]) {
                e.preventDefault();
                e.stopPropagation();
            }
        }

        $("#" + gridName).find(".k-link.k-pager-nav").on("click", function (e) {
            pagingHandler(gridName, true, this, e);
        });

        $("#" + gridName).find(".k-pager-input").on("change", function (e) {
            pagingHandler(gridName, false, this, e);
        });

        $("#" + gridName).delegate("tbody > tr > td > a", "click", initShowPopup);

        $("#" + gridName).delegate("tbody > tr > td > img", "click", initShowPopup);

        $(document).on("click", ".msgCtrl-close", function (e) {
            setGridCell.call(this, gridName);
        });

        GridPreferencesHelper.setGrid("#" + gridName, model.GridColumnSettings);
    }

    function initShowPopup(e) {
        e.preventDefault();
        var gridName = e.delegateTarget.id;
        var grid = $("#" + gridName).data("kendoGrid");
        if (grid.dataSource.data().length <= 0) {
            return false;
        }
        var row = $(this).closest("tr");
        var colIndex = $(this).closest("td")[0].cellIndex;

        if (colIndex < 0) {
            return false;
        }
        var rowData = grid.dataItem(row);
        var col = grid.columns[colIndex];
        //TODo: get from JSON definition
        var param1 = rowData["ITEMNO"];

        if (col.drillDownUrl && param1) {
            //var childUrl = getDrillDownUrl(col, rowData);
            var urls = col.drillDownUrl.split("/");
            if (urls.length > 2) {
                var url = sg.utls.url.buildUrl(urls[0], urls[1], urls[2]) + "?id=" + param1;
                if (url) {
                    sg.utls.iFrameHelper.openWindow("GridDetailPopup", "", url, null, null, null);
                }
            }
        }
    }

    function getDrillDownUrl(col, rowData) {
        var drillDownUrl = col.drillDownUrl;
        var param1 = rowData[col.columnName];
        var url = "";

        // Drill down url is in config json
        if (col.isDrillDown && drillDownUrl) {
            var controllerList = drillDownUrl.ControllerList;
            var controller;
            if (controllerList && controllerList.length === 1) {
                controller = controllerList[0].Controller;
            } else {
                var typeName = rowData[drillDownUrl.TypeField];
                var appName = drillDownUrl.SrceApplField ? rowData[drillDownUrl.SrceApplField] : "";
                if (appName) {
                    controller = controllerList.filter(function (c) { return c.Type === typeName && c.SrceAppl === appName; })[0];
                } else {
                    controller = controllerList.filter(function (c) { return c.Type === typeName; })[0];
                }
                // if the controller type is string name, get it's type id
                if (!controller) {
                    var typeCols = grid.columns.filter(function (c) { return c.field === drillDownUrl.TypeField; });
                    if (typeCols && typeCols.length > 0) {
                        var typeCol = typeCols[0];
                        if (typeCol.presentation) {
                            var types = typeCol.presentation.filter(function (p) { return p.Text === typeName; });
                            if (types && types.length > 0) {
                                var typeId = types[0].Value;
                                if (appName) {
                                    controller = controllerList.filter(function (c) { return c.Type === typeId && c.SrceAppl === appName; })[0];
                                } else {
                                    controller = controllerList.filter(function (c) { return c.Type === typeId; })[0];
                                }
                            }
                        }
                    }
                }

                if (!controller) {
                    controller = controllerList[0];
                }
                controller = controller.Controller;
            }
            //var params = drillDownUrl.Params;
            var params = controller.Parameters;
            var length = params.length;
            var paramsStr = "";
            if (controller.Controller === "InquiryGeneral") {
                paramsStr = "?templateId=" + params[0].Field;
                if (length > 1) {
                    var values = "";
                    for (var i = 1; i < length; i++) {
                        values += rowData[params[i].Field] + ",";
                    }
                    paramsStr += "&id=" + values.slice(0, -1);
                }
            } else {
                var paramValues = InquiryGeneralViewModel.Ids;
                for (var i = 0; i < length; i++) {
                    var fieldName = params[i].Field;
                    var paramValue;
                    if (fieldName.indexOf('{') === 0 && fieldName.indexOf('}') > 1) {
                        var idx = fieldName.substring(1, 2);
                        paramValue = kendo.format(fieldName, paramValues ? paramValues[idx] : "");
                    } else {
                        paramValue = rowData[fieldName] || fieldName;
                    }
                    paramsStr += kendo.format("{0}{1}={2}", i === 0 ? "?" : "&", params[i].Name, paramValue);
                }
            }
            url = sg.utls.url.buildUrl(controller.Area, controller.Controller, controller.Action) + "/" + paramsStr;
        }
        return url;
    }

    function getColumnTemplate(col, gridName) {
        var datetimeTemplate = '#if({0} === null || {0} == ""){##}else{# #= kendo.toString(kendo.parseDate({0}), "d") #  #}#';
        var type = col.dataType ? col.dataType.toLowerCase() : "string";
        var mask = col.presentationMask;
        var template;

        if (col.finder && mask && mask.indexOf('C') > 0 ) {
            template = kendo.format("#= {0}.toUpperCase() #", col.field);
        }
        if (type === "decimal") {
            template = '#= kendo.toString(' + col.field + ', "n' + col.precision + '") #';
        }
        if (col.presentationList){
            template = $.proxy(kendo.template(accpacGrid.getListText), col.presentationList, col.field);
        }
        if (col.drillDownUrl) {
            template = kendo.format("<a href=''>#={0}#</a>", col.field);
        }
        if (col.isLineNumber) {
            template = "#= ++accpacGridLineNumber #";
            //template = $.proxy(kendo.template(accpacGrid.getLineNumber), gridName, col.field);
        }
        if (type === "date" || type === "datetime") {
            template = kendo.format(datetimeTemplate, col.field);
        }
        return template;
    }

    function getGridColumns(gridName) {
        var columns = window[gridName + "Model"].ColumnDefinitions;
        var length = columns.length;
        var cols = [];
        var numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"];
        for (var i = 0; i < length; i++) {
            var col = {};
            var column = columns[i];
            var datType = column.DataType ? column.DataType.toLowerCase() : "string";
            var list = column.PresentationList;
            var attr = numbers.indexOf(datType) > -1 && list === null ? "align-right " : "align-left ";
            col.title = column.ColumnName;
            col.field = column.AccpacFieldName;
            col.dataType = datType;
            col.width = column.Width || 180;
            col.headerWidth = col.width;
            col.attributes = { "class": attr };
            col.headerAttributes = { "class": attr };
            col.precision = column.Precision;
            col.hidden = column.IsHidden || false;
            col.presentationList = list;
            col.presentationMask = column.PresentationMask;
            col.finder = column.Finder;
            col.drillDownUrl = column.DrillDownUrl;
            col.isLineNumber = column.IsLineNumber;
            col.template = column.Template || getColumnTemplate(col, gridName);
            col.editor = function (container, options) {
                return getColumnEditor(container, options, columns, gridName);
            };

            cols.push(col);
        }
        return cols;
    }

    function getTextBoxProps(mask) {
        var props = { class: "", maxLength: 20 };

        if (mask) {
            var isA = mask.indexOf("A") > 0;
            var isN = mask.indexOf("N") > 0;
            var isC = mask.indexOf("C") > 0;
            var number = mask.substring(2, 4);

            if (isNaN(number)) {
                number = number.substring(0, 1);
            }
            props.class = isA || isC || isN ? "txt-upper" : "";
            props.maxLength = number;
        }

        return props;
    }

    function getFuntion(functionName) {
        var ns = functionName.split('.');
        return ns.length > 1 ? functionName.split('.').reduce(function (obj, i) { return obj[i]; }, window) : window[functionName];
    }

    function finderEditor(container, options, col, gridName) {
        var finder = col.Finder;
        var mask = col.PresentationMask;
        var field = options.field;
        var buttonId = "btnFinderGridCol" + field.toLowerCase();
        var maskProps = getTextBoxProps(mask);
        var className = maskProps.class;
        var maxlength = maskProps.maxLength;
        var txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input name="{0}" maxlength="{1}" class="{2}"/></div>';
        var txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{3}"/></div></div>';
        var html = kendo.format(txtInput + txtFinder, field, maxlength, className, buttonId);

        $(html).appendTo(container);
        finder.viewID = finder.ViewID;
        finder.viewOrder = finder.ViewOrder;
        finder.displayFieldNames = finder.DisplayFieldNames;
        finder.returnFieldNames = finder.ReturnFieldNames;
        // set finder init key values
        finder.initKeyValues = [];
        var length = finder.InitKeyFieldNames ? finder.InitKeyFieldNames.length : 0;
        if (length > 0) {
            for (var i = 0; i < length; i++) {
                finder.initKeyValues.push(options.model[finder.InitKeyFieldNames[i]]);
            }
        } else {
            finder.initKeyValues = [options.model[options.field]];
        }
        if (finder.Filter) {
            finder.filter = getFilter(length);
        }
        if (finder.CustomFinder) {
            finder = getFuntion(finder.CustomFinder)(finder);
        } 

        function getFilter(length) {
            var filter = finder.Filter;
            for (var i = 0; i < length; i++) {
                filter = filter.replace('{' + i + '}', '"' + finder.initKeyValues[i] + '"');
                finder.initKeyValues[i] = "";
            }
            return filter;
        }
        function onFinderSelected(options, value) {
            options.model.set(options.field, value[Object.keys(value)[0]]);
        }
        function onFinderCancel(options) {
            var grid = $("#" + gridName).data("kendoGrid");
            if (grid) {
                var rowIndex = grid.select().index();
                setEditCell(grid, rowIndex, options.field);
            }
        }

        sg.viewFinderHelper.setViewFinder(buttonId, onFinderSelected.bind(null, options), finder, onFinderCancel.bind(null, options));
        accpacGridLastColField[gridName] = options.field;
    }

    function dateEditor(container, options, gridName) {
        var field = options.field;
        var txtId = "txt" + gridName + field;
        var input = kendo.format('<input name="{0}" id="{1}" />', field, txtId);

        $(input).appendTo(container);
        sg.utls.kndoUI.datePicker(txtId);
        accpacGridLastColField[gridName] = field;
    }

    function dropdownEditor(container, options, presentationList, gridName) {
        var field = options.field;
        if (options.model[field] === true) {
            options.model[field] = "True";
        }
        if (options.model[field] === false) {
            options.model[field] = "False";
        }
        $('<input name="' + field + '" />')
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: presentationList
            });

        accpacGridLastColField[gridName] = field;
    }

    function textEditor(container, options, col, gridName) {
        var mask = col.PresentationMask;
        var maskProps = getTextBoxProps(mask);
        var className = maskProps.class;
        var maxlength = maskProps.maxLength;
        var field = options.field;
        var html = kendo.format('<input type="text" name="{0}" class="{1}" maxlength="{2}"/>', field, className, maxlength);

        $(html).addClass('k-input k-textbox')
            .appendTo(container)
            .change(function () {
                options.model.set(options.field, this.value);
            });
        accpacGridLastColField[gridName] = field;
    }

    function numericEditor(container, options, precision, gridName) {
        var field = options.field;
        var html = '<input name="' + field + '"/>';
        var txtNumeric = $(html).appendTo(container).kendoNumericTextBox({
            format: "n" + precision,
            spinners: false,
            decimals: precision
            //change: function (e) {
            //    options.model.set(field, e.sender.value() );
            //}
        });
        sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, 16);
        accpacGridLastColField[gridName] = field;
    }

    function noEditor(container, gridName) {
        sg.utls.kndoUI.nonEditable($('#' + gridName).data("kendoGrid"), container);
    }

    function getColumnEditor(container, options, columns, gridName) {
        var numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"];
        var field = options.model.fields[options.field];
        var dataType = field.type.toLowerCase();
        var col = columns.filter(function (c) { return c.AccpacFieldName === options.field; })[0];

        if (col.HasFinder) {
            return col.Finder ? finderEditor(container, options, col, gridName) : null;
        }

        if (col.PresentationList) {
            return dropdownEditor(container, options, col.PresentationList);
        }
        if (dataType === "date" || dataType === "datetime" ) {
            return dateEditor(container, options, gridName);
        }

        if (numbers.indexOf(dataType) > -1 ) {
            return numericEditor(container, options, col.Precision, gridName);
        }
        if (!col.IsEditable) {
            return noEditor(container, gridName);
        }
        return textEditor(container, options, col, gridName);
    }

    function getListText(field, dataItem) {
        var list = this.filter(function (i) { return i.Value.toLowerCase() === dataItem[field].toString().toLowerCase(); });
        return list && list.length > 0 ? list[0].Text : dataItem[field];
    }

    function valid(gridName) {
        return accpacGridValid[gridName];
    }

    function dataChanged(gridName) {
        return accpacGridDataChanged[gridName];
    }

    function showColumns(gridName, columns) {
        if (gridName && columns) {
            var grid = $("#" + gridName).data("kendoGrid");
            if (columns instanceof Array) {
                for (var i = 0, length = columns.length; i < length; i++) {
                    grid.showColumn(columns[i]);
                }
            } else {
                grid.showColumn(columns);
            }
        }
    }

    function hideColumns(gridName, columns) {
        if (gridName && columns) {
            var grid = $("#" + gridName).data("kendoGrid");
            if (columns instanceof Array) {
                for (var i = 0, length = columns.length; i < length; i++) {
                    grid.hideColumn(columns[i]);
                }
            } else {
                grid.hideColumn(columns);
            }
        }
    }

    function setGridReadOnly(gridName, readOnly) {
        $("#" + gridName).data("kendoGrid").setOptions({ editable: !readOnly });
    }

    function setColumnsReadOnly(gridName, readOnlyColumns )  {
        accpacGridReadOnlyColumns[gridName] = readOnlyColumns;
    }

    function getCurrentCellValue(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        var row = grid.dataItem(grid.select());
        var colIdx = grid.select().closest("td").index();  
        var field = grid.columns[colIdx].field;
        return row[field];
    }

    function getCurrentRowValue(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        return grid.dataItem(grid.select());
    }

    function refreshGrid(gridName) {
        var grid = $("#" + gridName).data("kendoGrid");
        grid.dataSource.read();
    }

    return {
        init: init,
        addLine: accpacGridToolbarAdd,
        deleteLine: accpacGridToolbarDelete,
        editColumnSettings: editColumnSettings,
        getListText: getListText,
        showColumns: showColumns,
        hideColumns: hideColumns,
        getCurrentCellValue: getCurrentCellValue, 
        getCurrentRowValue: getCurrentRowValue, 
        setGridReadOnly: setGridReadOnly,
        setColumnsReadOnly: setColumnsReadOnly, 
        valid: valid,
        dataChanged: dataChanged,
        refreshGrid: refreshGrid
    };
}();
