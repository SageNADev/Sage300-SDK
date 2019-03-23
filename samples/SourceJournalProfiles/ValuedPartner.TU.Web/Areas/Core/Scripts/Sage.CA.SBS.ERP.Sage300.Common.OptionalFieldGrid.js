﻿// Copyright (c) 2019 Sage Software, Inc.  All rights reserved.

"use strict";
var sg = sg || {};

sg.optionalFieldControl = function () {

    var RowStatusEnum = {
        UPDATE: 1,
        INSERT: 2,
        NONE: 3
    },
    ValueTypeEnum = {
        Text: 1,
        Amount: 100,
        Number: 6,
        Integer: 8,
        YesNo: 9,
        Date: 3,
        Time: 4
    },
    RequestTypeEnum = {
        Create: 1,
        Insert: 2,
        Update: 3
    },
    YesNoList = [{ "Selected": false, "Text": globalResource.Yes, "Value": "1" }, { "Selected": false, "Text": globalResource.No, "Value": "0" }],
    ButtonTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}" id="btn{3}{4}">{2}</button>';

    var _setDefaultRow = {},
        _lastRowNumber = {},
        _lastColField = {},
        _lastRowStatus = {},
        _lastErrorResult = {},
        _dataChanged = {},
        _filter = {},
        _settings = {};
    /**
     * @description Get the kendo grid by name
     * @param {any} gridName The grid name
     * @return {string} return the kendo grid
     */
    function _getGrid(gridName) {
        return $("#" + gridName).data("kendoGrid");
    }

    /**
     *  @description Add new line, new line is added after the selected row and set the new line as selected
     *  @param {string} gridName The name of the grid.
     *  @return {void}  
     */
    function _addLine(gridName) {
        var grid = _getGrid(gridName),
            dataSource = grid.dataSource,
            insertedIndex = grid.select().index() + 1,
            pageSize = dataSource.pageSize(),
            currentPage = dataSource.page();

        if (insertedIndex === pageSize) {
            insertedIndex = 0;
            dataSource.page(++currentPage);
        }
        _setDefaultRow[gridName] = false;
        _sendRequest(gridName, RequestTypeEnum.Create, "");
    }

     /**
     *  @description Set current editable cell
     *  @param {Object} grid The name of the grid.
     *  @param {Number} rowIndex The slected row index.
     *  @param {string} field The column field name.
     *  @return {void}  
     */
    function _setEditCell(grid, rowIndex, field) {
        var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, field);
        grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
    }

     /**
     *   @description Set next editable cell as current cell.
     *   @param {Object} grid The name of the grid.
     *   @param {Object} model The slected row model.
     *   @param {Object} row The current row.
     *   @param {Number} startIndex The column name.
     *   @return {void}  
     */
    function _setNextEditCell(grid, model, row, startIndex) {
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

     /**
     *  @description Set grid cell as editable
     *  @param {string} gridName The name of the grid.
     *  @return {void}  
     */
    function _setGridCell(gridName) {
        var grid = _getGrid(gridName);
        var rowIndex = grid.select().index();
        var colName = _lastErrorResult[gridName].colName || _lastColField[gridName];
        _setEditCell(grid, rowIndex, colName);
    }

     /**
     * @description Generate the grid columns based on model column definitions(get from business view)
     * @param {string} gridName The grid name
     * @return {object} The column template
     */
    function _getGridColumns(gridName) {
        var columns = window[gridName + "Model"].ColumnDefinitions,
            cols = [];

        for (var i = 0, length = columns.length; i < length; i++) {
            var col = {},
                column = columns[i],
                dataType = column.DataType ? column.DataType.toLowerCase() : "string";

            col.title = column.ColumnName;
            col.field = column.FieldName;
            col.dataType = dataType;
            col.width = 180;
            col.headerWidth = 180;
            col.template = _getColumnTemplate(col);
            col.editor = function (container, options) {
                return _getColumnEditor(container, options, gridName);
            };

            cols.push(col);
        }
        return cols;
    }

     /**
     * @description Use column field masks to set the textbox attributes
     * @param {string} mask : The field mask, mask definition format
     * @return {void}
     */
    function _getTextBoxProps(mask) {
        var props = { class: "", maxLength: 20 };

        if (mask) {
            var isA = mask.indexOf("A") > 0,
                isN = mask.indexOf("N") > 0,
                isC = mask.indexOf("C") > 0,
                number = mask.substring(2, 4);

            if (isNaN(number)) {
                number = number.substring(0, 1);
            }
            props.class = isA || isC || isN ? "txt-upper" : "";
            props.maxLength = number;
        }

        return props;
    }

    /**
     * @description Check duplicate optional field
     * @param {string} gridName The name of the grid
     * @param {string} value The optional field name
     * @return {boolean} A boolean flag
     */
    function _checkDuplicateOptField(gridName, value) {
        var ds = _getGrid(gridName).dataSource.data();
        return ds.filter(function (row) { return row.OPTFIELD === value; }).length > 0;
    }

    /**
     * @description Show error message
     * @param {string} errorMessage Error message
     */
    function _showMessage(errorMessage) {
        var message = { UserMessage: { Message: "", Errors: [ { Message: errorMessage} ] } };
        sg.utls.showMessage(message);
    }

     /**
     * @description The column finder editor
     * @param {object} container The editor container
     * @param {object} options The column options
     * @param {string} gridName The grid name
     */
    function _finderEditor(container, options, gridName) {
        var field = options.field,
            model = options.model,
            isValue = field !== "OPTFIELD",
            maxLength = isValue ? 60 : 12,
            txtClass = isValue ? "" : "txt-upper grid_inpt pr25",
            finder = {},
            viewId = _settings[gridName].viewId,
            filter = _settings[gridName].filter,
            buttonId = "btnFinderGridCol" + field.toLowerCase(),
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input name="{0}" id="txtGridCol{0}" maxlength="{1}" class="{2}"/></div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{3}"/></div></div>',
            html = kendo.format(txtInput + txtFinder, field, maxLength, txtClass, buttonId);
       
        $(html).appendTo(container);

        finder.viewID = isValue ? "CS0012" : viewId;
        finder.viewOrder = 0;
        finder.filter = isValue ? "OPTFIELD=" + model.OPTFIELD : filter;
        finder.displayFieldNames = isValue ? ["VALUE", "VDESC", "TYPE"] : ["LOCATION", "OPTFIELD", "FDESC", "DEFVAL", "TYPE", "LENGTH", "DECIMALS", "SWSET", "VDESC"];
        finder.returnFieldNames = isValue ? ["VALUE", "VDESC"] : ["OPTFIELD"];
        finder.calculatePageCount = false;
        var initLocation = filter.split('=').pop();
        finder.initKeyValues = isValue ? [model.OPTFIELD, model.VALUE] : [initLocation, model.OPTFIELD];

        $("#txtGridColOPTFIELD").on("change", function (e) {
            var value = this.value.toUpperCase(),
                errorMsg = kendo.format("Duplicate Optional Field {0}", value.toUpperCase());

            e.preventDefault();
            this.value = value;
            if (_checkDuplicateOptField(gridName, value)) {
                sg.utls.showMessage(errorMsg);
                setTimeout(function () {
                    _lastColField[gridName] = field;
                    model.set(field, "");
                }, 50);
            } else {
                model.set(field, value);
            }
        });

        /**
         * @description On select finder row, set the select value
         * @param {any} options The column options
         * @param {any} retureFields The finder selected row value
         */
        function onFinderSelected(options, retureFields) {
            var model = options.model,
                field = options.field,
                value = retureFields[options.field],
                seq = pad(10, model.SEQUENCENO, ' '),
                line = model.LINENO ? pad(10, model.LINENO, ' ') : "",
                optField = pad(12, value, ' '),
                errorMsg = kendo.format("Duplicate Optional Field {0}", value.toUpperCase());

            if (field === "OPTFIELD") {
                if (_checkDuplicateOptField(gridName, value)) {
                    _showMessage(errorMsg);
                    return;
                } 
                var keyValue = seq + line + optField;
                model.set(field, value);
                model.set("KendoGridAccpacViewPrimaryKey", keyValue);
                model.set("id", keyValue);
            } else {
                model.set(field, value);
            }
        }

        /**
         * @description Pading the char
         * @param {number} width The width
         * @param {string} string The string need to be padding
         * @param {char} padding The padding char
         * @return {string} Return the padding string
         */
        function pad(width, string, padding) {
            return width <= string.length ? string : pad(width, padding + string, padding);
        }

        /**
         * @description On cancel the finder, focus the last edit cell
         * @param {any} options The column options
         */
        function onFinderCancel(options) {
            var grid = _getGrid(gridName);
            if (grid) {
                var rowIndex = grid.select().index();
                _setEditCell(grid, rowIndex, options.field);
            }
        }

        sg.viewFinderHelper.setViewFinder(buttonId, onFinderSelected.bind(null, options), finder, onFinderCancel.bind(null, options));
        _lastColField[gridName] = field;
    }
    /**
     * @description Column date editor, parse the view date value "yyyymmdd" and diplay the proper date
     * @param {any} container The column kendo container
     * @param {any} options The column options
     * @param {any} gridName The grid name
     */
    function _dateEditor(container, options, gridName) {
        var field = options.field,
            model = options.model,
            value = options.model.VALUE,
            txtId = "txt" + gridName + field,
            input = kendo.format('<input data-text-field="{0}" data-value-field="{0}" data-bind="value:{0}" id="{1}" />', field, txtId);

        if (model.hasOwnProperty('TYPE') && model.TYPE === ValueTypeEnum.Date && !isNaN(value)) {
            model.VALUE = value.toString().length === 8 ? kendo.parseDate(value.toString(), 'yyyyMMdd') : value;
        }
        $(input)
            .appendTo(container)
            .change(function (e) {
                //options.model.set(field, sg.utls.kndoUI.getDateYYYMMDDFormat(this.value));
            });
        sg.utls.kndoUI.datePicker(txtId);
        _lastColField[gridName] = field;
    }
    /**
     * @description Column time editor, set the mask for time value input
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {any} gridName The grid name
     */
    function _timeEditor(container, options, gridName) {
        var field = options.field,
            html = kendo.format('<input id="{0}" name="{0}" />', field);

        $(html)
            .appendTo(container)
            .kendoMaskedTextBox({
                mask: "12:34:34",
                unmaskOnPost: true,
                rules: {
                    "1": /[0-2]/,
                    "2": /[0-3]/,
                    "3": /[0-5]/,
                    "4": /[0-9]/
                }
            })
            .change(function (e) {
                options.model.set(field, this.value);
            });
        _lastColField[gridName] = field;
    }
    /**
     * @description Column dropdown list editor. Convert model true/false to proper display text
     * @param {any} container The column kendo editor container
     * @param {any} options The column kendo options
     * @param {any} presentationList The column text/value pairs list
     * @param {string} gridName The grid name
     * @param {boolean} isCustom The boolean flag
     */
    function _dropdownEditor(container, options, presentationList, gridName) {
        var field = options.field,
            model = options.model,
            input = kendo.format('<input name="{0}" data-bind="value:{0}" />', field);

        if (field === "VALUE") {
            model.VALUE = model.VALUE.trim();
        }
        $(input).appendTo(container)
            .kendoDropDownList({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: presentationList,
                change: function (e) {
                    model[field] = this.value();
                }
            });
        _lastColField[gridName] = field;
    }

    /**
     * @description Column text editor. Get field mask, set html element attributes
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {any} col The column object
     * @param {string} gridName The grid name
     */
    function _textEditor(container, options, col, gridName) {
        var mask = col.PresentationMask,
            maskProps = mask ? _getTextBoxProps(mask) : "",
            className = maskProps ? maskProps.class : "",
            maxlength = maskProps ? maskProps.maxLength : 64,
            field = options.field,
            html = kendo.format('<input type="text" name="{0}" class="{1}" maxlength="{2}"/>', field, className, maxlength);

        $(html).addClass('k-input k-textbox')
            .appendTo(container)
            .change(function () {
                options.model.set(field, this.value);
            });
        _lastColField[gridName] = field;
    }
    /**
     * @description Column number editor, binding kendo numeric textbox for validation and display
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {string} gridName The grid name
     */
    function _numericEditor(container, options, gridName) {
        var field = options.field,
            grid = _getGrid(gridName),
            precision = grid.columns.filter(function (c) { return c.field === field; })[0].precision,
            html = '<input name="' + field + '"/>';

        var txtNumeric = $(html).appendTo(container).kendoNumericTextBox({
            format: "n" + precision,
            spinners: false,
            decimals: precision
        });
        sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, 16);
        _lastColField[gridName] = field;
    }
     /**
     * @description Column not editable, set no editor
     * @param {any} container The column kendo editor container
     * @param {any} gridName The grid name
     */
    function _noEditor(container, gridName) {
        sg.utls.kndoUI.nonEditable($('#' + gridName).data("kendoGrid"), container);
    }
    /**
     * @description Dynamic editor, based on reference column defintion to set editor. Such as optional field default value/value column, it depend on optional field column
     * @param {any} container The column kendo container
     * @param {any} options The column options
     * @param {string} gridName The grid name
     */
    function _dynamicEditor(container, options, gridName) {
        var type = options.model.TYPE;

        if (type === ValueTypeEnum.Date) {
            _dateEditor(container, options, gridName);
        } else if (type === ValueTypeEnum.Time) {
            _timeEditor(container, options, gridName);
        } else if (type === ValueTypeEnum.Number || type === ValueTypeEnum.Integer || type === ValueTypeEnum.Amount) {
            _numericEditor(container, options, gridName);
        } else if (type === ValueTypeEnum.YesNo) {
            _dropdownEditor(container, options, YesNoList, gridName);
        } else {
            _finderEditor(container, options, gridName);
        }
    }
    /**
     * @description Get column template
     * @param {object} col The column
     * @return {object} Return column template
     */
    function _getColumnTemplate(col) {
        var f = col.field;
        if (f === "VALUE" || f === "DEFVAL") {
            return '#= sg.optionalFieldControl.getTemplate(data) #';
        } else if (f === "SWSET" || f === "SWREQUIRED" || f === "INITFLAG") {
            return $.proxy(kendo.template(sg.optionalFieldControl.getListText), YesNoList, col.field);
        }
    }
    /**
     * @description get column editor
     * @param {object} container The column kendo container.
     * @param {object} options The column kendo options.
     * @param {string} gridName The grid name.
     * @return {object} Return column editor.
     */
    function _getColumnEditor(container, options, gridName) {
        var f = options.field,
            model = options.model;

        if (f === "VALUE" || f === "DEFVAL") {
            return _dynamicEditor(container, options, gridName);
        }
        if (f === "FDESC" || f === "VDESC" || f === "SWSET" ) {
            return _noEditor(container, gridName);
        }
        if (f === "OPTFIELD") {
            return model.OPTFIELD && !model.dirty ? _noEditor(container, gridName) : _finderEditor(container, options, gridName);
        }
        if (f === "SWREQUIRED" || f === "INITFLAG" ) {
            return _dropdownEditor(container, options, YesNoList, gridName);
        }
        return _textEditor(container, options, gridName);
    }
    /**
     * @description Get request name to compose ajax request call
     * @param {number} requestType The request type
     * @return {string} The request name
     */
    function _getRequestName(requestType) {
        var requestName = "";
        switch (requestType) {
            case RequestTypeEnum.Create:
                requestName = "Create";
                break;
            case RequestTypeEnum.Insert:
                requestName = "Insert";
                break;
            case RequestTypeEnum.Update:
                requestName = "Refresh";
                break;
        }
        return requestName;
    }
    /**
     * @description When ajax request call is completed, to handle the call back json results
     * @param {Number} requestType The request type
     * @param {boolean} isSuccess The request call whether success
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {string} fieldName The field name
     * @param {boolean} isNewLine The boolean flag to indicate is a new line
     * @param {Number} insertedIndex The inserted line index
     */
    function _requestComplete(requestType, isSuccess, gridName, jsonResult, fieldName) {
        switch (requestType) {
            case RequestTypeEnum.Create:
                isSuccess ? _createSuccess(gridName, jsonResult) : _createError(gridName, jsonResult);
                break;
            case RequestTypeEnum.Insert:
                isSuccess ? _insertSuccess(gridName) : _insertError(gridName, jsonResult);
                break;
            case RequestTypeEnum.Update:
                isSuccess ? _updateSuccess(gridName, jsonResult, fieldName) : _updateError(gridName, jsonResult, fieldName);
                break;
            default:
        }
    }
     /**
     * @description Select the grid row based on unique key, used for keep the select row after refresh, read, filtering
     * @param {object} grid The kendo grid
     * @param {string} rowKeyId The row record key id
     * @return {object} Return the slelect row
     */
    function _selectGridRow(grid, rowKeyId) {
        var selectRow = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "KendoGridAccpacViewPrimaryKey", rowKeyId);
        var row = sg.utls.kndoUI.getRowForDataItem(selectRow);
        grid.select(row);
        return row;
    }
    /**
     * @description Send create request success
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     * @param {number} insertedIndex The inserted line index
     */
    function _createSuccess(gridName, jsonResult) {
        setTimeout(function () {
            var grid = $('#' + gridName).data("kendoGrid"),
                insertedIndex = grid.select().index() + 1,
                dataSource = grid.dataSource;

            dataSource.insert(insertedIndex, jsonResult.Data);
            grid.refresh();
            var row = _selectGridRow(grid, jsonResult.Data["KendoGridAccpacViewPrimaryKey"]);
            _setNextEditCell(grid, dataSource.options.schema.model, row, 0);
            _lastRowNumber[gridName] = insertedIndex;
            _lastRowStatus[gridName] = RowStatusEnum.INSERT;
        }, 100);
    }

    /**
     * @description Send create request error
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _createError(gridName, jsonResult) {
        _lastRowNumber[gridName] = -1;
        sg.utls.showMessage(jsonResult);
    }

    /**
     * @description Send insert request success
     * @param {string} gridName The grid name
     */
    function _insertSuccess(gridName) {
        _lastRowStatus[gridName] === RowStatusEnum.NONE;
        //_dataChanged[gridName] = false;
    }

    /**
     * @description Send insert request error
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _insertError(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid"),
            rowIndex = grid.select().index();

        sg.utls.showMessage(jsonResult);
        grid.select("tr:eq(" + rowIndex + ")");
        setTimeout(function () {
            _setEditCell(grid, rowIndex, _lastColField[gridName]);
        });
        //_dataChanged[gridName] = true;
    }

     /**
     * @description Send update request success, refresh the grid and select row and column
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     * @param {string} fieldName update field name
     */
    function _updateSuccess(gridName, jsonResult, fieldName) {
        var grid = $('#' + gridName).data("kendoGrid"),
            selectRow = grid.select(),
            rowIndex = selectRow.index(),
            dataItem = grid.dataItem(selectRow);

        _setModuleVariables(gridName, RowStatusEnum.None, "", rowIndex);
        for (var field in jsonResult.Data) {
            if (dataItem && dataItem.hasOwnProperty(field)) {
                dataItem[field] = jsonResult.Data[field];
            }
        }
        grid.refresh();
        var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
        var row = _selectGridRow(grid, jsonResult.Data["KendoGridAccpacViewPrimaryKey"]);
        _setNextEditCell(grid, dataItem, row, index + 1);

        if (fieldName === "OPTFIELD") {
            _sendRequest(gridName, RequestTypeEnum.Insert, "");
        }
    }
    /**
     * @description Send update request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {any} fieldName The update field name
     */
    function _updateError(gridName, jsonResult, fieldName) {
        var grid = $('#' + gridName).data("kendoGrid"),
            prevDataSource = grid.dataSource._pristineData,
            selectRow = grid.select(),
            rowIndex = selectRow.index(),
            dataItem = grid.dataItem(selectRow);

        _setModuleVariables(gridName, RowStatusEnum.UPDATE, jsonResult, rowIndex);
        sg.utls.showMessage(jsonResult);
        var prevRowData = prevDataSource[rowIndex];
        if (prevRowData) {
            dataItem[fieldName] = prevRowData[fieldName];
        }
        _setEditCell(grid, rowIndex, fieldName);
    }

    /**
     * @description Send sync ajax request
     * @param {string} gridName The grid name
     * @param {number} requestType The request type
     * @param {string} fieldName The field name
     * @param {boolean} isNewLine The boolean flag to indicate whether add new line
     * @param {number} insertedIndex The inserted line index
     */
    function _sendRequest(gridName, requestType, fieldName) {
        var grid = _getGrid(gridName),
            data = { 'viewID': $("#" + gridName).attr('viewID'), 'record': grid.dataItem(grid.select()), 'fieldName': fieldName },
            requestName = _getRequestName(requestType);

        var url = sg.utls.url.buildUrl("Core", "GridOptionalField", requestName);
        sg.utls.ajaxPostSync(url, data, function (jsonResult) {
            var isSuccess = true;
            if (jsonResult.UserMessage.Errors || jsonResult.UserMessage.Warning) {
                isSuccess = false;
            }
            _requestComplete(requestType, isSuccess, gridName, jsonResult, fieldName);
        });
    }

    /**
     * @description Set module(class) variables
     * @param {string} gridName The namr of the grid
     * @param {number} status The grid action status
     * @param {object} error The error object
     * @param {number} rowIndex The row index
     */
    function _setModuleVariables(gridName, status, error, rowIndex) {
        _lastRowStatus[gridName] = status;
        _lastErrorResult[gridName].message = error;
        _lastErrorResult[gridName].colName = _lastColField[gridName];
        _lastRowNumber[gridName] = rowIndex;
        _setDefaultRow[gridName] = false;
    }

    /**
     * @description On grid data source data changes
     * @param {string} gridName The grid name
     * @param {object} e The event object
     */
    function _onDataChanged(gridName, e) {
        _dataChanged[gridName] = e.action ? true : false;
        var grid = _getGrid(gridName);

        if (e.items.length === 0 && grid.dataSource.page() !== 1) {
            grid.dataSource.page(1);
        }
        if (e.action === "itemchange") {
            if (e.field === "OPTFIELD" && e.items[0].OPTFIELD === "") {
                return;
            }
            _sendRequest(gridName, RequestTypeEnum.Update, e.field);
        }
    }

    /**
     * @description Initialize a grid, creating dataSource to binding grid, register events handler
     * @param {string} gridName The name of the grid.
     * @param {boolean} readOnly Whether the grid allows editing(Optional).
     * @param {object} settings The optional field setting object
     */
    function init(gridName, readOnly, settings) {
        var columns = _getGridColumns(gridName),
            addTemplate = kendo.format(ButtonTemplate, 'btn-add', 'sg.optionalFieldControl.addLine(&quot;' + gridName + '&quot;)', globalResource.AddLine, gridName, "Add"),
            delTemplate = kendo.format(ButtonTemplate, 'btn-delete', 'sg.optionalFieldControl.deleteLine(&quot;' + gridName + '&quot;)', globalResource.DeleteLine, gridName, "Delete");

        readOnly = readOnly || false;
        if (readOnly) {
            settings.allowInsert = false;
            settings.allowDelete = false;
        }
        _settings[gridName] = settings;
        reset(gridName);

        $("#" + gridName).kendoGrid({
            height: 450,
            columns: columns,
            navigatable: true,
            reorderable: true,
            filterable: false,
            resizable: true,
            selectable: "row",
            editable: readOnly ? false : {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },

            change: function (e) {
                var grid = _getGrid(gridName),
                    lastRowNumber = _lastRowNumber[gridName],
                    selectedIndex = grid.select().index();

                if (_lastRowStatus[gridName] === RowStatusEnum.UPDATE && selectedIndex !== lastRowNumber) {
                    setTimeout(function () {
                        grid.select("tr:eq(" + lastRowNumber + ")");
                        sg.utls.showMessage(_lastErrorResult[gridName].message);
                    });
                } 
            },

            dataBound: function (e) {
                var grid = e.sender,
                    pageSize = grid.dataSource.pageSize() - 1;

                grid.tbody.find("tr:gt(" + pageSize + ")").hide();
                if (_setDefaultRow[gridName]) {
                    grid.select("tr:eq(0)");
                }
            },

            pageable: {
                pageSize: 10,
                numeric: false,
                buttonCount: 1,
                input: true
            },

            toolbar: [{ template: addTemplate }, { template: delTemplate } ],

            dataSource: {
                serverPaging: true,
                serverFiltering: false,
                serverSorting: false,
                schema: {
                    data: "Data",
                    total: "Total",
                    model: {
                        id: "KendoGridAccpacViewPrimaryKey",
                        fields: eval(gridName + "_fields")
                    }
                },
                batch: true,
                change: function (e) {
                    _onDataChanged(gridName, e);
                },
                transport: {
                    read: {
                        url: window.sg.utls.url.buildUrl("Core", "GridOptionalField", "Read"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax()
                    },
                    destroy: {
                        url: window.sg.utls.url.buildUrl("Core", "GridOptionalField", "Delete"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax()
                    },
                    parameterMap: function (data, operation) {
                        data.viewID = $("#" + gridName).attr('viewID');
                        if (operation === "read") {
                            reset(gridName);
                            data.filter = _filter[gridName];
                            return JSON.stringify(data);
                        }
                        else {
                            data.record = data.models[0];
                            return JSON.stringify(data);
                        }
                    }
                }
            }
        });

        $("#btn" + gridName + "Add").prop("disabled", !settings.allowInsert);
        $("#btn" + gridName + "Delete").prop("disabled", !settings.allowDelete);

        $(document).on("click", ".msgCtrl-close", function (e) {
            _setGridCell.call(this, gridName);
        });
    }

    /**
     * @description Delete grid line, for grid toolbar template, grid internal use
     * @param {string} gridName The name of the grid.
     */
    function toolbarDeleteLine(gridName) {
        var grid = _getGrid(gridName);
        var selectedIndex = grid.select().index();

        if (selectedIndex > -1) {
            sg.utls.showKendoConfirmationDialog(
                function () {
                    var dataItem = grid.dataItem(grid.select());
                    grid.dataSource.remove(dataItem);
                    if (dataItem.OPTFIELD !== "") {
                        grid.dataSource.sync();
                    }
                    grid.dataSource.read();
                },
                function () { },
                globalResource.DeleteLineMessage, window.DeleteTitle);
        }
    }

    /**
     * @description Add grid new line, for grid toolbar template, grid internal use.
     * @param {string} gridName The grid name.
     */
    function toolbarAddLine(gridName) {
        var grid = _getGrid(gridName),
            rowIndex = grid.select().index();
        // set the row status to update as it is not finished yet due to error
        if (_lastRowStatus[gridName] === RowStatusEnum.UPDATE) {
            setTimeout(function (rowIndex) {
                grid.select("tr:eq(" + rowIndex + ")");
                sg.utls.showMessage(_lastErrorResult[gridName].message);
            }.bind(null, rowIndex));
            return;
        }
        _addLine(gridName);
    }

    /**
     * @description Get dropdown list text from value, for grid column dropdown list template(internal use)
     * @param {string} field The column field name
     * @param {object} dataItem The text/value pair list
     * @return {string} return the text by value
     */
    function getListText(field, dataItem) {
        var list = this.filter(function (i) { return i.Value.toLowerCase() === dataItem[field].toString().toLowerCase(); });
        return list && list.length > 0 ? list[0].Text : dataItem[field];
    }

    /**
     * @description Get column template, for grid template intenal use
     * @param {object} data The grid column data.
     * @return {object} return column template
     */
    function getTemplate(data) {
        var type = data.TYPE,
            value = data.VALUE,
            decimals = data.DECIMALS;

        if (value === null) {
            return "";
        } else if (type === ValueTypeEnum.YesNo) {
            return value.trim() === "1" ? globalResource.Yes : globalResource.No;
        } else if (type === ValueTypeEnum.Date) {
            return sg.utls.kndoUI.getFormattedDate(value);
        } else if (type === ValueTypeEnum.Time && value.indexOf(':') < 0) {
            return value.substring(0, 2) + ":" + value.substring(2, 4) + ":" + value.substring(4, 6);
        } else if (type === ValueTypeEnum.Integer || type === ValueTypeEnum.Amount || type === ValueTypeEnum.Number) {
            return '<span style="float:right">' + sg.utls.kndoUI.getFormattedDecimalNumber(value || 0, decimals) + '</span>';
        } else {
            return value;
        }
    }

    /**
     * @description Get/Set whether a view list grid has been changed.
     * @param {any} gridName The name of grid
     * @param {any} dirtyFlag A boolean flag
     * @return {boolean} Grid dirty flag or none
     */
    function dirty(gridName, dirtyFlag) {
        if (dirtyFlag !== undefined) {
            _dataChanged[gridName] = dirtyFlag;
            return;
        }
        return _dataChanged[gridName];
    }

     /**
     * @description Set grid read only.
     * @param {string} gridName The name of the grid.
     * @param {bool} readOnly A boolean flag.
     * @return {void}
     */
    function setGridReadOnly(gridName, readOnly) {
        _getGrid(gridName).setOptions({ editable: !readOnly });
        if (readOnly) {
            $("#btn" + gridName + "Add").prop("disabled", true);
            $("#btn" + gridName + "Delete").prop("disabled", true);
        }
    }

    /**
     * @description Get/Set the current filter for a grid.
     * @param {any} gridName The name of the grid.
     * @param {any} filter The filter applied to the grid
     */
    function setFilter(gridName, filter) {
        _filter[gridName] = filter;
    }

    /**
     * @description Read data from the server side, refresh the view list display, and reset the page number to 1.
     * @param {any} gridName The name of the grid.
    */
    function refresh(gridName) {
        var grid = _getGrid(gridName);
        grid.dataSource.read();
    }

    /**
     * @description Reset the optional grid varaibales to default values
     * @param {string} gridName The name of the grid
     */
    function reset(gridName) {
        _setDefaultRow[gridName] = true;
        _lastErrorResult[gridName] = {};
        _lastRowNumber[gridName] = -1;
        _lastColField[gridName] = "";
        _lastRowStatus[gridName] = RowStatusEnum.NONE;
        _dataChanged[gridName] = false;
    }

    /**
     * @description On show the popup optional field window, execute this function(for Sage 300 internal use)
     * @param {string} gridName The name of the grid
     * @param {string} popupElementId The popup element id
     * @param {boolean} isReadOnly The booelan flag to indicate readonly property
     * @param {string} filter The filter will apply to grid
     * @param {string} parentGridName The parent grid name
     */
    function showPopUp(gridName, popupElementId, isReadOnly, filter, parentGridName) {
        if (parentGridName) {
            sg.viewList.syncCurrentRow(parentGridName);
        }
        sg.utls.openKendoWindowPopup('#' + popupElementId, null);
        if (filter) {
            setFilter(gridName, filter);
        }
        if (isReadOnly !== undefined) {
            setGridReadOnly(gridName, isReadOnly);
        }
        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.page(1);
    }

    /**
     * @description On close the popup optional field window from details grid, execute this function(for Sage 300 internal use)
     * @param {string} gridName The name of optional grid
     * @param {string} parentGridName The parent grid name
     */
    function closePopUp(gridName, parentGridName) {
        var grid = _getGrid(gridName),
            total = grid.dataSource.total(),
            parentGrid = $("#" + parentGridName).data("kendoGrid"),
            selectedItem = parentGrid.dataItem(parentGrid.select());

        //selectedItem.OptionalField = total > 0 ? globalResource.Yes : globalResource.No;
        if (selectedItem.VALUES !== total) {
            selectedItem.set("VALUES", total);
        }
        reset(gridName);
    }

    /**
     * @description Get/Set the “Insertable” property of a grid.
     * @description If the grid does not allow insert, the “Add Line” button should be disabled
     * @param {string} gridName The name of the grid
     * @param {booelan} insertable A boolean flag to indicate whether grid allow insert new line
     * @return {boolean} return true if the grid allows insert or null
     */
    function allowInsert(gridName, insertable) {
        if (insertable !== undefined) {
            $("#btn" + gridName + "Add").prop("disabled", !insertable);
            return;
        }
        var disabled = $("#btn" + gridName + "Add").prop("disabled");
        return !disabled;
    }

    /**
     * @description Get/Set the “deletable” property of a grid.
     * @description If the grid does not allow insert, the “Add Line” button should be disabled
     * @param {string} gridName The name of the grid
     * @param {booelan} deletable A boolean flag to indicate whether grid allow insert new line
     * @return {boolean} return true if the grid allows delete or null
     */
    function allowDelete(gridName, deletable) {
        if (insertable !== undefined) {
            $("#btn" + gridName + "Delete").prop("disabled", !deletable);
            return;
        }
        var disabled = $("#btn" + gridName + "Delete").prop("disabled");
        return !disabled;
    }

    /**
     * @description Get/Set the current filter for a grid.
     * @param {any} gridName The name of the grid.
     * @param {any} filter The filter applied to the grid
     * @return {string} The filter being applied to the grid or null
     */
    function filter(gridName, filter) {
        if (filter !== undefined) {
            _filter[gridName] = filter;
        }
        return _filter[gridName];
    }

    /**
     * @description Commit unsaved changes in a grid to the server.
     * @param {string} gridName The name of grid.
     * @return {boolean} Return a boolean flag to indicate success or not
     */
    function commit(gridName) {
        var result = true;
        if (gridName) {
            result = _commitGrid(gridName);
        } 
        return result;
    }

    //Expose module(class) public methods
    return {
        init: init,

        //Grid internal methods(For grid toolbar, column template use)
        addLine: toolbarAddLine,
        deleteLine: toolbarDeleteLine,
        getTemplate: getTemplate,
        getListText: getListText,

        //Undocument public methods(For Sage 300 web screens use)
        setGridReadOnly: setGridReadOnly,
        setFilter: setFilter,
        reset: reset,
        showPopUp: showPopUp,
        closePopUp: closePopUp,

        //Documented public methods
        allowInsert: allowInsert,
        allowDelete: allowDelete,
        filter: filter,
        dirty: dirty,
        commit: commit,
        refresh: refresh
    };
}();
