/* Copyright (c) 1994-2023 Sage Software, Inc.  All rights reserved. */
"use strict";

var sg = sg || {};
sg.viewList = function () {

    const FIELD_DISABLED_ATTRIBUTE = 4;
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
        Invalid: -1,
        Create: 1,
        Insert: 2,
        Update: 3,
        Refresh: 4,
        Delete: 5,
        MoveTo: 6,
        RefreshRow: 7,
        ResetRow: 8,
        ClearNewRow: 9,
        GetParentData: 10
    },
    PageByKeyTypeEnum = {
        FirstPage: 0,
        PreviousPage: 1,
        NextPage: 2,
        LastPage: 3,
        Refresh: 4,
    },
    GridTypeEnum = {
        AccpacView: 0,
        OptionalField: 1,
        Custom: 2
    },
    BtnTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}" id="btn{3}{4}">{2}</button>';
    const BtnShowHideTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}" id="btn{3}{4}"  style="{5}">{2}</button>';

    var _defaultPageSize = 10;

    var _setDefaultRow = {},
        _lastRowNumber = {},
        _lastColField = {},
        _lastRowStatus = {},
        _lastGridAction = {},
        _newLine = {},
        _lastErrorResult = {},
        _valid = {},
        _dataChanged = {},
        _allowInsert = {},
        _allowDelete = {},
        _readOnlyColumns = {},
        _currentPage = {},
        _filter = {},
        _sendChange = {},
        _selectRowUid = {},
        _skipMoveTo = {},
        _pagingRowData = {},
        _pageByKeyType = {},
        _refreshKey = {},
        _showDetails = {},
        _showWarnings = {},
        _showErrors = {},
        _gridList = [],
        _forms = {},
        _selectedRow = {},
        _commitDetail = {},
        _lastRecord = {};

     /**
     *   @description get kendo grid.
     *   @param {string} gridName The name of the grid.
     *   @return {object} The kendo grid  
     */
    function _getGrid(gridName) {
        return $("#" + gridName).data("kendoGrid");
    }
     /**
     *   @description Add new line, new line is added after the selected row and set the new line as selected
     *   @param {string} gridName The name of the grid.
     *   @param {object} rowData The current select row data
     *   @return {void}  
     */
    function _addLine(gridName, rowData, commitDetail) {
        var grid = _getGrid(gridName),
            dataSource = grid.dataSource,
            insertedIndex = grid.select().index() + 1,
            pageSize = dataSource.pageSize(),
            total = dataSource.total(),
            currentPage = dataSource.page();
            rowData = rowData || grid.dataItem(grid.select());

        // If the current record reaches maximum of current page but not the last record, go to next page, keep rowData for next page create new record to use 
        if (insertedIndex === pageSize) {
            _pagingRowData[gridName] = rowData;
        }
        if (insertedIndex === pageSize && total / pageSize !== currentPage) {
            insertedIndex = 0;
            dataSource.page(++currentPage);
            // Paging send extra moveto request, skip to send the request
            _skipMoveTo[gridName] = true;
        }
        if (insertedIndex > pageSize) {
            insertedIndex = 0;
            rowData = _pagingRowData[gridName];
        }
        _setDefaultRow[gridName] = false;
        const formId = _forms[gridName];
        let commitRecord = true;
        //Skip update/insert detail if detail is not dirty based on the value we passed in.
        if (formId && $(`#${formId}`).is(":visible") && !_commitDetail[gridName]) {
            commitRecord = false;
        }
        //If the record is dirty, update the current row first before adding a new row
        if (rowData && rowData.dirty && commitRecord) {
            _sendRequest(gridName, RequestTypeEnum.Update, "", false, insertedIndex, rowData);
        } else {
            _sendRequest(gridName, RequestTypeEnum.Create, "", false, insertedIndex, rowData);
        }
    }

     /**
     *   @description Set current editable cell
     *   @param {Object} grid The name of the grid.
     *   @param {Number} rowIndex The selected row index.
     *   @param {string} columnName The column name.
     *   @return {void}  
     */
    function _setEditCell(grid, rowIndex, columnName) {
        // focus current form control if it's visible
        var formControl = $(`[data-bind*="value:${columnName}"]`);
        var formId = _forms[grid.element.attr('id')];
        if (formId && formControl && $(`#${formId}`).is(":visible")) {
            formControl.focus();
        }
        // focus current grid cell
        else {
            var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
            grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
        }
    }

    /**
    *   @description Set focus on next editable grid-cell
    *   @param {Object} gridName The name of the grid.
    *   @param {Number} startIndex The column index to start with
    *   @return {void}  
    */
    function setNextEditCellByCol(gridName, startIndex) {
        var grid = $('#' + gridName).data("kendoGrid");
        if (-1 < grid.select().index()) {
            for (var i = startIndex; i < grid.columns.length; i++) {
                var col = grid.columns[i];
                if (col.field && col.hidden !== true && col.editable !== undefined && col.editable()) {
                    var colField = grid.dataSource.options.schema.model.fields[col.field];
                    if (colField && colField.editable) {
                        setTimeout(function () {
                            const currentRowIndex = grid.select().index();
                            const currentColumnIndex = i;
                            const currentCell = grid.tbody.find(`tr[role='row']:eq(${currentRowIndex}) td:eq(${currentColumnIndex})`);
                            grid.editCell(currentCell);
                        });
                        break;
                    }
                }
            }
        }
    }

     /**
     *   @description Set next editable cell as current cell.
     *   @param {Object} grid The name of the grid.
     *   @param {Object} model The selected row model.
     *   @param {Object} row The current row.
     *   @param {Number} startIndex The column name.
     *   @return {void}  
     */
    function _setNextEditCell(grid, model, row, startIndex) {
        for (var i = startIndex; i < grid.columns.length; i++) {
            var col = grid.columns[i];
            if (col.field && col.hidden !== true && col.editable !== undefined && col.editable()) {
                var colField = model.fields[col.field];
                if (colField && colField.editable) {
                    // focus next form control if it's visible
                    var formControl = $(`[data-bind*="value:${col.field}"]`);
                    var formId = _forms[grid.element.attr('id')];
                    if (formId && formControl && $(`#${formId}`).is(":visible")) {
                        setTimeout(function () {
                            formControl.focus();
                        });
                    }
                    // focus next grid cell
                    else {
                        grid.editRow(row);
                        setTimeout(function () {
                            grid.editCell(row.find(">td").eq(i));
                        });
                    }
                    break;
                }
            }
        }
    }

     /**
     *   @description Set grid cell as editable
     *   @param {string} gridName The name of the grid.
     *   @return {void}  
     */
    function _setGridCell(gridName) {
        var grid = _getGrid(gridName);
        var rowIndex = grid.select().index();
        var colName = _lastErrorResult[gridName].colName || _lastColField[gridName];
        _setEditCell(grid, rowIndex, colName);
    }

    /**
    *   @description Set variables value after insert request call successful
    *   @param {string} gridName The grid name
    */
    function _setInsertNewLine(gridName) {
        _lastRowNumber[gridName] = -1;
        _lastRowStatus[gridName] === RowStatusEnum.NONE;
        _valid[gridName] = true;
        _newLine[gridName] = true;
    }

    /**
    *   @description Reserved, for future use
    *   @param {object} event the event object
    */
    function _receiveMessage(event) {
        var eventData = event.data;
        if (!eventData || eventData.action !== "" && !eventData.gridName) {
            return;
        }
    }

    /**
     * @description Save the the current line to send ajax request
     * @param {string} gridName The current grid name
     * @param {function} callBack The callBack function after the action is completed
     * @return {boolean} A boolean flag to indicate the current grid valid status. True if no current line selected (nothing to save).
     */
    function _commitGrid(gridName, callBack) {
        var grid = _getGrid(gridName),
            rowData = grid.dataItem(grid.select());

        const fieldName = _lastColField[gridName];

        if (fieldName) {
            const value = $(`input[name=${fieldName}]`).val();
            if (value && rowData[fieldName] != value) {
                rowData[fieldName] = value;
            }
        }

        if (rowData) {
            if ((!_newLine[gridName] || !rowData.isNewLine) && !rowData.dirty) {
                if (callBack && typeof callBack === "function") {
                    callBack(true);
                }
                return true;
            }
            else {
                const type = (!_newLine[gridName] || !rowData.isNewLine) ? RequestTypeEnum.Update : RequestTypeEnum.Insert;

                _sendRequest(gridName, type, ...Array(3), rowData, callBack);
                if (_valid[gridName]) {
                    _newLine[gridName] = false;
                    //NOTE: Wont be dirty after commit successful, we may need to reset the dirty flag as well
                    //_dataChanged[gridName] = false;
                }
                return _valid[gridName];
            }
        }
        else {
            if (callBack && typeof callBack === "function") {
                callBack(true);
            }
            return true;
        }
    }

    /**
     * @description Initialize and show drill down popup.
     * @param {object} e The event object.
     * @return {void}
     */
    function _initShowPopup(e) {
        e.preventDefault();
        var gridName = e.delegateTarget.id;
        var grid = _getGrid(gridName);
        if (grid.dataSource.data().length <= 0) {
            return false;
        }
        var colIndex = $(this).closest("td")[0].cellIndex;
        if (colIndex < 0) {
            return false;
        }
        var row = $(this).closest("tr"),
            rowData = grid.dataItem(row),
            col = grid.columns[colIndex];

        if (col.drillDownUrl) {
            var urls = col.drillDownUrl.split("/");
            if (urls.length > 2) {
                var url = sg.utls.url.buildUrl(urls[0], urls[1], urls[2]);
                if (col.parameters) {
                    var params = col.parameters;
                    var paramsStr = "";
                    for (var i = 0, length = params.length; i < length; i++) {
                        var param = params[i];
                        var value = rowData[param.Field] || param.Field || rowData[col.field];
                        paramsStr += kendo.format("{0}={1}&", param.Name, value);
                    }
                    if (paramsStr) {
                        paramsStr = paramsStr.slice(0, -1);
                    }
                    url = url + "?" + paramsStr;
                }
                sg.utls.iFrameHelper.openWindow("GridDetailPopup", "", url);
            }
        }
    }

    /**
     * @description Get grid column template for display value properly
     * @param {object} col The grid column object
     * @return {object} The column template
     */
    function _getColumnTemplate(col) {
        var datetimeTemplate = '#if({0} === null || {0} == ""){##}else{# #= kendo.toString(kendo.parseDate({0}), "d") #  #}#',
            type = col.dataType ? col.dataType.toLowerCase() : "string",
            mask = col.presentationMask,
            template;

        if (col.referenceField) {
            template = '#= sg.viewList.getTemplate(data) #';
        }

        if (col.finder && mask && mask.indexOf('ANC') > 0 ) {
            template = kendo.format("#= {0}.toUpperCase() #", col.field);
        }
        if (type === "decimal") {
            template = '#= kendo.toString(' + col.field + ', "n' + col.precision + '") #';
        }
        if (col.presentationList){
            template = $.proxy(kendo.template(sg.viewList.getListText), col.presentationList, col.field);
        }
        if (col.drillDownUrl) {
            template = kendo.format("<a href=''>#={0}#</a>", col.field);
        }
        if (col.isLineNumber) {
            template = "<span class='displayIndex'></span>";
        }
        if (type === "date" || type === "datetime") {
            template = kendo.format(datetimeTemplate, col.field);
        }
        if (col.isOptionalField) {
            template = function (dataItem) {
                return dataItem.VALUES > 0 ? globalResource.Yes : globalResource.No;
            };
        }

        return template;
    }
    
    /**
     * @description Generate the grid columns based on model column definitions(get from business view)
     * @param {string} gridName The grid name
     * @return {object} The column template
     */
    function _getGridColumns(gridName) {
        var columns = window[gridName + "Model"].ColumnDefinitions,
            cols = [{field: "isNewLine", hidden: true, isInternal: true}],
            numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"];

        for (let i = 0, length = columns.length; i < length; i++) {
            var col = {},
                column = columns[i],
                dataType = column.DataType ? column.DataType.toLowerCase() : "string",
                list = column.PresentationList,
                attr = numbers.indexOf(dataType) > -1 && list === null ? "align-right " : "align-left ";

            //Allow white spaces displayed
            if (!column.PresentationMask && dataType === "char") {
                attr = attr + "space-preserved ";
            }

            //custom call back column definitions
            _columnSettingCallback(gridName, "columnBeforeDisplay", column);

            let title = column.ColumnName;
            if (title && title.includes('Resx.')) {
                title = title.split('.').reduce((obj, i) => obj[i] , window);
            }
            col.title = title;
            col.isPrimaryKeyField = column.IsPrimaryKeyField;
            col.field = column.GridFieldName ? column.GridFieldName : column.FieldName;
            col.dataType = dataType;
            col.width = column.Width || 180;
            col.fieldSize = column.FieldSize;
            col.headerWidth = col.width;
            col.attributes = { "class": attr };
            col.headerAttributes = { "class": attr };
            col.precision = column.Precision;
            col.hidden = column.IsHidden || false;
            col.isInternal = column.IsInternal || false;
            col.locked = column.Locked;
            col.presentationList = list;
            col.displayType = column.DisplayType;
            col.presentationMask = column.PresentationMask;
            col.finder = column.Finder;
            col.drillDownUrl = column.DrillDownUrl;
            col.parameters = column.Parameters;
            col.isLineNumber = column.IsLineNumber;
            col.isVirtualField = column.IsVirtualField;
            col.isOptionalField = column.IsOptionalField;
            col.customFunctions = column.CustomFunctions;
            col.referenceField = column.ReferenceField;
            col.template = column.Template || _getColumnTemplate(col);
            col.editor = function (container, options) {
                return _getColumnEditor(container, options, columns, gridName);
            };
            col.editable = () => {
                //Custom plug in for 'columnBeforeEdit'
                return _columnCallback(gridName, "columnBeforeEdit", columns[i].FieldName);
            };

            cols.push(col);
        }
        return cols;
    }

    /**
     * @description Use column field masks (from the business logic view) to set the textbox attributes
     * @param {string} mask : The field mask
     * 
     * Note:
     * Handles the simple cases of a single mask section.  See the 
     * Accpac SDK help for more information on this.
     * 
     * This code handles all the simple cases (single segment, no fixed
     * characters).  A mask of (%-3d) %-3d-%-4d (the phone number example in 
     * the Accpac SDK documentation) will require completion of this implementation,
     * or special handling.
     *
     * TODO: handle the more general case (as VB does).
     *
     * @return {property attributes}
     */
    function _getTextBoxProps(mask) {
        var props = { class: "", maxLength: 20 };
        if (mask) {
            const matched = mask.match(/^\%(-?[0-9]+)([AaCcDdNn])$/);
            if (null != matched) {
                let classes = '';
                let formatTextBox = '';
                let length = matched[1];  // first capture group
                let format_code = matched[2];
                switch (format_code) {
                    case 'A':
                        classes = 'txt-upper';
                    case 'a':
                        formatTextBox = 'alpha';
                        break;
                    case 'C':
                        classes = 'txt-upper';
                    case 'c':
                        break;
                    case 'D':
                    case 'd':
                        formatTextBox = 'numeric';
                        break;
                    case 'N':
                        classes = 'txt-upper';
                    case 'n':
                        formatTextBox = 'alphaNumeric'
                        break;
                }
                props.class = classes;
                props.maxLength = length;
                props.formattextbox = formatTextBox;
            }
        }
        return props;
    }

    /**
     * @description Get function from function name string, including the namespace
     * @param {string} functionName The function full name, including namespace
     * @return {function} The function expression
     */
    function _getFunction(functionName) {
        var ns = functionName.split('.');
        return ns.length > 1 ? ns.reduce(function (obj, i) { return obj[i]; }, window) : window[functionName];
    }

    /**
     * @description Set editor initial value
     * @param {string} gridName The name of the grid
     * @param {any} options Editor options object
     */
    function _setEditorInitialValue(gridName, options) {
        var field = options.field;
        _lastColField[gridName] = field;
        _lastErrorResult[gridName][field + "Value"] = options.model[field];
    }

    /**
     * @description The column finder editor
     * @param {object} container The editor container
     * @param {object} options The column options
     * @param {object} col The column
     * @param {string} gridName The grid name
     */
    function _finderEditor(container, options, col, gridName) {
        var finder = col.Finder,
            numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"],
            field = options.field,
            model = options.model,
            mask = col.mask,
            buttonId = "btnFinderGridCol" + field.toLowerCase();
        if (model["PresentationMasks"]) {
            mask = model.PresentationMasks[field] || col.mask;
        }
        var maskProps = _getTextBoxProps(mask),
            className = maskProps.class, 
            formattextbox = maskProps.formattextbox,
            maxlength = col.FieldSize || maskProps.maxLength,
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input name="{0}" id="{0}" type="text" maxlength="{1}" class="{2}" formattextbox="{4}"/></div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{3}"/></div></div>',
            html = kendo.format(txtInput + txtFinder, field, maxlength, className, buttonId, formattextbox);

        if (numbers.indexOf(col.DataType) > -1) {
            var grid = _getGrid(gridName),
                precision = grid.columns.filter(function (c) { return c.field === field; })[0].precision,
                dataType = col.DataType.toLowerCase(),
                size = col.FieldSize,
                maxLength, max, min = 0;

            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input id="{0}" name="{0}" class="{2} pr25" /></div>';
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{1}"/></div></div>';
            html = kendo.format(txtInput + txtFinder, field, buttonId, className);

            switch (dataType) {
                case "int":
                case "integer":
                case "int16":
                    min = -32768;
                    max = 32767;
                    maxLength = 5;
                    break;
                case "long":
                case "int32":
                    min = -2147483648;
                    max = 2147483647;
                    maxLength = 10;
                    break;
                case "byte":
                    min = 0;
                    max = 255;
                    maxLength = 3;
                    break;
                case "decimal":
                    max = Math.pow(10, size * 2) - 1;
                    min = - 1 * max;
                    maxLength = 2 * size;
                    if (maxLength > 16) {
                        maxLength = 16;
                    }
            }
            $(html).appendTo(container);

            var txtNumeric = $("#"+field).kendoNumericTextBox({
                format: "n" + precision,
                spinners: false,
                min: min,
                max: max,
                decimals: precision
            });

            sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, maxLength - precision);
            _setEditorInitialValue(gridName, options);
        }
        else {
            $(html).appendTo(container);
        }
        finder.viewID = finder.ViewID;
        finder.viewOrder = finder.ViewOrder;
        finder.displayFieldNames = finder.DisplayFieldNames;
        finder.returnFieldNames = finder.ReturnFieldNames;
        finder.initKeyValues = [];
        finder.buttonId = buttonId;

        if (finder.CustomFinderProperties) {
            let customPropertiesFunc = _getFunction(finder.CustomFinderProperties);
            if (typeof customPropertiesFunc === 'function') {
                finder = customPropertiesFunc(container, options);
            }
            else{
                finder = sg.utls.deepCopy(customPropertiesFunc);
            }
        }

        var refKey = col.ReferenceField;
        if (refKey) {
            finder.filter = kendo.format("{0}={1}", refKey, model[refKey]);
        }

        $('#' + field).focus(function () {
			_columnCallback(gridName, "columnFinderFocus", field, finder);
		})
		
        //Finder has filter, get the filter, set finder calculatePageCount as false
        if (finder.filter || finder.Filter) {
            finder.filter = getFilter();
            finder.calculatePageCount = false;
        }

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);

        function replaceLast(find, replace, string) {
            var lastIndex = string.lastIndexOf(find);
            if (lastIndex === -1) {
                return string;
            }
            var beginString = string.substring(0, lastIndex);
            var endString = string.substring(lastIndex + find.length);
            return beginString + replace + endString;
        }

        /**
         * @description Parse the filter expression, dynamically set the filter value, such as filter expression: "ITEMNO=UNFMTITMNO", the "UNFMTITMNO" should the item value like 'A11030'
         * @return {object} return the parsed filters 
         */
        function getFilter() {
            finder.filter = finder.Filter || finder.filter;
            var filters = finder.filter.toUpperCase();
            var exprs = filters.split(' AND ').join(',').split(' OR ').join(',').split(',');
            exprs.forEach(function (expr) {
                var field = expr.split('=').pop().trim();
                if (model.hasOwnProperty(field)) {
                    let value = col.DataType == 'Char' ? `"${model[field]}"` : model[field];
                    filters = replaceLast(field, value, filters);
                }
            });
            return filters;
        }

        /**
         * @description Set custom defined finder value
         * @param {object} options The column options
         * @param {object} col The column
         * @param {any} returnValue The return value
         */
        function setCustomFinderValue(options, col, returnValue) {
            var callback = _getFunction(col.customData[0] || col.CustomFunctions[0]);
            var key = callback(true);
            var rowId = options.model[key];
            callback(false, rowId, false, returnValue, field);
            var grid = _getGrid(gridName);
            if (grid) {
                var rowIndex = grid.select().index();
                grid.refresh();
                setTimeout(function () {
                    grid.select("tr:eq(" + rowIndex + ")");
                    _setEditCell(grid, rowIndex, options.field);
                }, 50);
            }
        }

        /**
         * @description On select finder row, set the select value
         * @param {any} options The column options
         * @param {any} col The column object
         * @param {any} value The finder selected row value
         */
        function onFinderSelected(options, col, value) {
            const returnValue = value[Object.keys(value)[0]];
            const field = options.field;
            const isNewLine = options.model.isNewLine;
            const sendRequest = options.model[field] === returnValue;

            _sendChange[gridName] = true;
            options.model.set(field, returnValue);
            // When finder selected value is the same as options model field value, the set method 
            // not trigger the change, need manually send refresh request to update view
            if (sendRequest & !window[gridName + "Model"].CustomGridMapperDefinitions) {
                // If grid had Custom mapper, avoid to refresh due to mis-matched fields/columns 
                // between client / server side. 
                _sendRequest(gridName, RequestTypeEnum.Refresh, field, isNewLine); // Previously RequestTypeEnum.RefreshRow
            }
        }
        
        /**
         * @description On cancel the finder, focus the last edit cell
         * @param {any} options The column options
         */
        function onFinderCancel(options) {
            var grid = _getGrid(gridName);
            _sendChange[gridName] = true;
            if (grid) {
                var rowIndex = grid.select().index();
                _setEditCell(grid, rowIndex, options.field);
            }
        }

        $("#" + field).on("keydown", function (e) {
            if (e.altKey && e.keyCode === sg.constants.KeyCodeEnum.DownArrow) {
                _sendChange[gridName] = false;
            } else {
                _sendChange[gridName] = true;
            }
        });

        $("#" + buttonId).mousedown(function (e) {
            // do not set grid finder when it is opened from outside the grid with hotkey
            const formId = _forms[gridName];
            if (formId && $(`#${formId}`).is(":visible")) {
                return;
            }

            //Set finder initial values
            _sendChange[gridName] = false;
            var value = $("#" + field).val().toUpperCase();
            finder.initKeyFieldNames = finder.initKeyFieldNames || finder.InitKeyFieldNames;
            var length = finder.initKeyFieldNames ? finder.initKeyFieldNames.length : 0;
            finder.initKeyValues = [];
            if (length > 0) {
                for (var i = 0; i < length; i++) {
                    var initField = finder.initKeyFieldNames[i];
                    var initValue = model.hasOwnProperty(initField) ? model[initField] : initField;
                    if (length === 1 || field === initField) {
                        initValue = value;
                    }
                    finder.initKeyValues.push(initValue);
                }
            } else {
                finder.initKeyValues = [value];
            }
            //Custom plug in for 'columnBeforeFinder', custom function set the column finder options
            _columnCallback(gridName, "columnBeforeFinder", field, finder);

            sg.viewFinderHelper.setViewFinder(buttonId, onFinderSelected.bind(null, options, col), finder, onFinderCancel.bind(null, options));
        });
        sg.utls.findersList[field] = buttonId;

        _setEditorInitialValue(gridName, options);
    }

    /**
     * @description Column date editor, parse the view date value "yyyymmdd" and display the proper date
     * @param {any} container The column kendo container
     * @param {any} options The column options
     * @param {any} gridName The grid name
     */
    function _dateEditor(container, options, gridName) {
        var field = options.field,
            value = options.model.VALUE,
            input = kendo.format('<input data-text-field="{0}" data-value-field="{0}" data-bind="value:{0}" id="{0}" />', field);

        if (options.model.hasOwnProperty('TYPE') && options.model.TYPE === ValueTypeEnum.Date && !isNaN(value)) {
            options.model.VALUE = value.toString().length === 8 ? kendo.parseDate(value.toString(), 'yyyyMMdd') : value;
        }
        $(input)
            .appendTo(container)
            .change(function (e) {
            });
        sg.utls.kndoUI.datePicker(field);
        _setEditorInitialValue(gridName, options);

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);
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
                    "2": /[0-9]/,
                    "3": /[0-5]/,
                    "4": /[0-9]/
                }
            })
            .change(function (e) {
                options.model.set(field, this.value.replace(/:/g,''));
            });
        _setEditorInitialValue(gridName, options);

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);
    }

    /**
     * @description Column dropdown list editor. Convert model true/false to proper display text
     * @param {any} container The column kendo editor container
     * @param {any} options The column kendo options
     * @param {any} presentationList The column text/value pairs list
     * @param {string} gridName The grid name
     * @param {boolean} isCustom The boolean flag
     */
    function _dropdownEditor(container, options, presentationList, gridName, isCustom) {
        var field = options.field,
            model = options.model,
            html = kendo.format('<input id="{0}" name="{0}" />', field);

        if (model[field] === true) {
            model[field] = "True";
        }
        if (model[field] === false) {
            model[field] = "False";
        }
        if (isCustom) {
            $(html).appendTo(container)
                .kendoDropDownList({
                    dataSource: presentationList
                });
        } else {
            $(html).appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: presentationList
                });
        }
        _setEditorInitialValue(gridName, options);

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);
    }

    /**
     * @description Column text editor. Get field mask, set html element attributes
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {any} col The column object
     * @param {string} gridName The grid name
     */
    function _textEditor(container, options, col, gridName) {
        var field = options.field,
            mask = options.model.PresentationMasks[field] || col.mask,
            maskProps = mask ? _getTextBoxProps(mask) : undefined,
            className = maskProps ? maskProps.class : "",
            formattextbox = maskProps ? maskProps.formattextbox : "",
            maxlength = col.FieldSize || (maskProps ? maskProps.maxLength : 64),
            // If you need to test D-44810, remove the 'formattextbox' attribute from this formatting statement
            html = kendo.format('<input type="text" id="{0}" name="{0}" class="{1}" maxlength="{2}" formattextbox="{3}" />', field, className, maxlength, formattextbox);
        options.model.isUpperCase = className.includes('txt-upper');

        $(html).addClass('k-input k-textbox')
            .appendTo(container)
            .change(function () {
                const value = options.model.isUpperCase ? (this.value || '').toUpperCase() : this.value;
                options.model.set(field, value);
            });

        _setEditorInitialValue(gridName, options);

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);
    }

    /**
     * @description Column number editor, binding kendo numeric textbox for validation and display
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {any} col The edit column
     * @param {string} gridName The grid name
     */
    function _numericEditor(container, options, col, gridName) {
        var field = options.field,
            grid = _getGrid(gridName),
            precision = grid.columns.filter(function (c) { return c.field === field; })[0].precision,
            dataType = col.DataType.toLowerCase(),
            size = col.FieldSize,
            maxLength, max, min = 0,
            html = kendo.format('<input id="{0}" name="{0}" />', field);

        switch (dataType) {
            case "int":
            case "integer":
            case "int16":
                min = -32768;
                max = 32767;
                maxLength = 5;
                break;
            case "long":
            case "int32":
                min = -2147483648;
                max = 2147483647;
                maxLength = 10;
                break;
            case "byte":
                min = 0;
                max = 255;
                maxLength = 3;
                break;
            case "decimal":
                max = Math.pow(10, size * 2) - 1;
                min = - 1 * max;
                maxLength = 2 * size;
                if (maxLength > 16) {
                    maxLength = 16;
                }
        }

        var txtNumeric = $(html).appendTo(container).kendoNumericTextBox({
            format: "n" + precision,
            spinners: false,
            min: min, 
            max: max,
            decimals: precision
        });

        sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, maxLength - precision);
        _setEditorInitialValue(gridName, options);

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);
    }

     /**
     * @description Column not editable, set no editor
     * @param {any} container The column kendo editor container
     * @param {any} gridName The grid name
     */
    function _noEditor(container, gridName) {
        var grid = $('#' + gridName).data("kendoGrid");
        grid.closeCell();
        if (container[0].cellIndex === 0 && sg.utls.isShiftKeyPressed) {
            var prevRowIndex = sg.utls.kndoUI.getSelectedRowIndex(grid) - 1;
            if (prevRowIndex >= 0) {
                grid.select(grid.tbody.find(">tr:eq(" + prevRowIndex + ")"));
            }
        } 
        sg.utls.kndoUI.skipTab(grid, container[0].cellIndex);
    }

    /**
     * @description Optional field column editor, model use VALUES field to keep the optional field grid records count
     * @param {any} container The column kendo editor container
     * @param {any} options The column kendo options
     * @param {any} col The column
     * @param {string} gridName The name of the grid
     */
    function _optionalFieldEditor(container, options, col, gridName) {
        var html = '<div class="edit-container"><div class="wrapper"><div class="edit-cell inpt-text"><input class="grid_inpt" data-bind="value:OptionalField" disabled></div>' +
                   '<div class="edit-cell inpt-finder"><input class="icon pencil-edit" id="btnDetailOptionalField" type="button"></div></div></div>';

        $(html).appendTo(container);
        options.model.OptionalField = options.model.VALUES > 0 ? globalResource.Yes : globalResource.No;
        var callback = _getFunction(col.CustomFunctions[0].OptionalField);
        //$("#btnDetailOptionalField").on("click", callback());
        $("#btnDetailOptionalField").on("click", function () {
            if (typeof callback === "function") {
                callback(options, gridName);
            }
        });
    }

    /**
     * @description Custom define editor, from custom define column and call back function to set dynamic editor
     * @param {any} container The column kendo editor container
     * @param {any} options The column kendo options
     * @param {any} col The column Object
     * @param {string} gridName The grid name
     */
    function _customEditor(container, options, col, gridName) {
        /**
         * Internal function forcall back
         * @param {string} rowId The row id
         * @param {string} field The field name
         * @param {any} e The event object
         */
        function onChange(rowId, field, e) {
            e.preventDefault();
            e.stopPropagation();
            callback(false, rowId, false, e.target.value, field);
        }
        
        var callback = _getFunction(col.customFunctions[0] || col.CustomFunctions[0]),
            key = callback(true),
            rowId = options.model[key],
            customCol = callback(false, rowId, false),
            value = customCol.Value,
            colDef = customCol.ColumnDef || col,
            field = options.field;

        if (!colDef.IsEditable) {
            return;
        }

        options.model[field] = value;
        $(this).val(value);

        if (colDef.PresentationList) {
            _dropdownEditor(container, options, colDef.PresentationList, gridName, true);
        } else if (colDef.Finder) {
            col.Finder = colDef.Finder;
            _finderEditor(container, options, col, gridName);
        } else if (colDef.DataType === "Decimal") {
            _numericEditor(container, options, col, gridName);
        } else if (colDef.DataType === "DateTime") {
            _dateEditor(container, options, gridName);
        } else {
            _textEditor(container, options, col, gridName);
        } 
        $(kendo.format("input[name='{0}']", field)).on("change", onChange.bind(null, rowId, field));
        _setEditorInitialValue(gridName, options);
    }

    /**
     * @description Dynamic editor, based on reference column defintion to set editor. Such as optional field default value/value column, it depend on optional field column
     * @param {any} container The column kendo container
     * @param {any} options The column options
     * @param {any} col The column
     * @param {string} gridName The grid name
     */
    function _dynamicEditor(container, options, col, gridName) {
        var grid = $('#' + gridName).data("kendoGrid"),
            refField = col.ReferenceField,
            type = options.model.TYPE;

        if (options.model[refField]) {
            if (type === ValueTypeEnum.Date) {
                _dateEditor(container, options, gridName);
            } else if (type === ValueTypeEnum.Time) {
                _timeEditor(container, options, gridName);
            } else if (type === ValueTypeEnum.Number || type === ValueTypeEnum.Integer || type === ValueTypeEnum.Amount) {
                _numericEditor(container, options, col, gridName);
            } else if (type === ValueTypeEnum.YesNo) {
                var presentationList = [{ "Selected": false, "Text": globalResource.Yes, "Value": "1" }, { "Selected": false, "Text": globalResource.No, "Value": "0" }];
                _dropdownEditor(container, options, presentationList, gridName, false);
            } else {
                _finderEditor(container, options, col, gridName);
            }
         } else {
            grid.closeCell(container);
        }
    }

    /**
     * @description get column editor
     * @param {object} container The column kendo container.
     * @param {object} options The column kendo options.
     * @param {object} columns The grid columns definitions.
     * @param {string} gridName The grid name.
     * @return {object} Return column editor.
     */
    function _getColumnEditor(container, options, columns, gridName) {
        var numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"],
            field = options.model.fields[options.field],
            dataType = Array.isArray(columns) ? field.type.toLowerCase() : columns.DataType,
            col = Array.isArray(columns) ? columns.filter(function (c) { return c.FieldName === options.field; })[0] : columns;

        if (col.ReferenceField) {
            return _dynamicEditor(container, options, col, gridName);
        }

        if (!col.IsEditable) {
            return _noEditor(container, gridName);
        }
        //Primary key column only editable for new line
        if (col.IsPrimaryKeyField && !options.model.isNewLine) {
            return _noEditor(container, gridName);
        }

        if (col.IsOptionalField) {
            return _optionalFieldEditor(container, options, col, gridName);
        }

        if (col.Finder) {
            return _finderEditor(container, options, col, gridName);
        }

        // DisplayType 0: Yes/No dropdown list, 1: True/False dropdown list
        if (["boolean", "bool"].includes(col.DataType.toLowerCase()) && col.DisplayType) {
            if (parseInt(col.DisplayType) < 2) {
                const txtTrue = col.DisplayType === '0' ? globalResource.Yes : globalResource.True;
                const txtFalse = col.DisplayType === '0' ? globalResource.No : globalResource.False;
                col.PresentationList = [{ Selected: false, Text: txtFalse, Value: false }, { Selected: false, Text: txtTrue, Value: true }];
            }
        }

        if (col.PresentationList) {
            return _dropdownEditor(container, options, col.PresentationList, gridName);
        }

        if (dataType === "date" || dataType === "datetime" ) {
            return _dateEditor(container, options, gridName);
        }

        if (dataType === "time" ) {
            return _timeEditor(container, options, gridName);
        }

        if (numbers.indexOf(dataType) > -1 ) {
            return _numericEditor(container, options, col, gridName);
        }

        return _textEditor(container, options, col, gridName);
    }

    /**
     * @description Show/hide grid multiple columns
     * @param {string} gridName The grid name
     * @param {array} columns The show/hide columns
     * @param {string} methodName The kendo method name (showColumn/hideColumn)
     */
    function _showHideColumns(gridName, columns, methodName) {
        if (methodName === "showColumn" || methodName === "hideColumn"  ) {
            if (gridName && columns) {
                var grid = _getGrid(gridName);
                if (grid) {
                    if (columns instanceof Array) {
                        for (var i = 0, length = columns.length; i < length; i++) {
                            grid[methodName](columns[i]);
                        }
                    } else {
                        grid[methodName](columns);
                    }
                }
            }
        }
    }

    /**
     * @description Post message, for future use
     * @param {string} gridName The grid name
     * @param {string} actionName The action name(Create, Insert, Update, Delete)
     * @param {object} record The working on row data
     * @param {string} field The update field name
     */
    function _postMessage(gridName, actionName, record, field) {
        window.postMessage({ "action": actionName, "gridName": gridName, "record": record, "field": field }, window.location);
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
            case RequestTypeEnum.Refresh:
                requestName = "Refresh";
                break;
            case RequestTypeEnum.Update:
                requestName = "Update";
                break;
            case RequestTypeEnum.Delete:
                requestName = "Delete";
                break;
            case RequestTypeEnum.MoveTo:
                requestName = "MoveTo";
                break;
            case RequestTypeEnum.RefreshRow:
                requestName = "RefreshCurrentRecord";
                break;
            case RequestTypeEnum.ResetRow:
                requestName = "ResetCurrentRecord";
                break;
            case RequestTypeEnum.ClearNewRow:
                requestName = "ClearNewRecord";
                break;
            case RequestTypeEnum.GetParentData:
                requestName = "GetParentData";
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
     * @param {string} uid The unique id used to identify update row  
     * @param {function} callBack The callBack function after the action is completed
     */
    function _requestComplete(requestType, isSuccess, gridName, jsonResult, fieldName, isNewLine, insertedIndex, uid, callBack) {
        switch (requestType) {
            case RequestTypeEnum.Create:
                isSuccess ? _createSuccess(gridName, jsonResult, insertedIndex) : _createError(gridName, jsonResult);
                break;
            case RequestTypeEnum.Insert:
                isSuccess ? _insertSuccess(gridName, jsonResult, isNewLine) : _insertError(gridName, jsonResult);
                break;
            case RequestTypeEnum.Refresh:
                isSuccess ? _refreshSuccess(gridName, jsonResult, fieldName) : _updateError(gridName, jsonResult, fieldName);
                break;
            case RequestTypeEnum.Update:
                isSuccess ? _updateSuccess(gridName, jsonResult, uid, insertedIndex) : _updateError(gridName, jsonResult, fieldName, uid);
                break;
            case RequestTypeEnum.Delete:
                isSuccess ? _deleteSuccess(gridName, jsonResult) : _deleteError(gridName, jsonResult);
                break;
            case RequestTypeEnum.MoveTo:
                if (isSuccess) _moveToSuccess(gridName, jsonResult);
                break;
            case RequestTypeEnum.RefreshRow:
                _refreshRow(gridName, jsonResult, fieldName);
                break;
            case RequestTypeEnum.ResetRow:
                _resetRow(gridName, jsonResult);
                break;
            case RequestTypeEnum.ClearNewRow:
                isSuccess ? _createSuccess(gridName, jsonResult, insertedIndex) : _createError(gridName, jsonResult);
                break;
            case RequestTypeEnum.GetParentData:
                break;
            default:
        }

        //check is any message popped up, as VB, if any message popped up, the following event would be termiated. 
        //we may consider the update/insert only happened if grid is dirty, it wont be a stopper.
        const isPoppedMessage = jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Warnings || jsonResult.UserMessage.Errors || jsonResult.UserMessage.Info)
        if(callBack && typeof callBack === "function"){
            callBack(isSuccess && !isPoppedMessage);
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
     * @description Insert new row in grid after create request success
     * @param {string} gridName The grid name
     * @param {object} data The inserted row data
     * @param {string} keyValue The row unique primary key value
     * @param {number} index The inserted line index
     */
    function _insertRow(gridName, data, keyValue, index) {
        var grid = $('#' + gridName).data("kendoGrid"),
            dataSource = grid.dataSource,
            currentPage = dataSource.page(),
            pageSize = dataSource.pageSize();

        _lastGridAction[gridName] = RequestTypeEnum.Create;

        // If the current record reaches maximum of current page, make it a dummy record and go to next page
        if (index === pageSize) {
            data.skipCommit = true;
            dataSource.insert(index, data);
            dataSource.query({ pageSize: pageSize, page: ++currentPage });
            _skipMoveTo[gridName] = true;
        } else {
            data.isNewLine = true;
            _skipMoveTo[gridName] = false;
            dataSource.insert(index, data);
            grid.refresh();
        }

        //D-41977 (Firefox Issue)
        if (grid.scrollables) {
            for (var i = 0; i < grid.scrollables.length; i++) { //in theory there should be only 2 one for header and one for body
                grid.scrollables[i].scrollLeft = 0; //reset the scroll position
            }
        }

        var row = _selectGridRow(grid, keyValue);
        _setModuleVariables(gridName, RowStatusEnum.INSERT, "", index, true, false, true);
        _setNextEditCell(grid, grid.dataSource.options.schema.model, row, 0);

        // bind form to current line
        const currentRow = grid.dataItem(grid.select());
        bindToForm(gridName, currentRow);

        _gridCallback(gridName, "gridAfterCreate");
    }

    /**
     * @description Send create request success
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     * @param {number} insertedIndex The inserted line index
     */
    function _createSuccess(gridName, jsonResult, insertedIndex) {
        var data = jsonResult.Data,
            page = $('#' + gridName).data('kendoGrid').dataSource.page(),
            keyValue = data["KendoGridAccpacViewPrimaryKey"];

        if (insertedIndex === 0 && page > 1) {
            setTimeout(function () {
                _insertRow(gridName, data, keyValue, 0);
            }, 100);
        } else {
            _insertRow(gridName, data, keyValue, insertedIndex);
        }
        _postMessage(gridName, "AddLine", data, "");
    }

    /**
     * @description Send create request error
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _createError(gridName, jsonResult) {
        _lastRowNumber[gridName] = -1;
        _valid[gridName] = false;
        setTimeout(() => {
            if (_showErrors[gridName]) {
                sg.utls.showMessage(jsonResult);
            }
        }, 100);
    }

    /**
     * @description Send insert request success
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     * @param {boolean} isNewLine The boolean flag to indicator new line
     */
    function _insertSuccess(gridName, jsonResult, isNewLine) {
        var grid = $('#' + gridName).data("kendoGrid"),
            keyId = jsonResult.Data["KendoGridAccpacViewPrimaryKey"],
            rowData = sg.utls.kndoUI.getRowByKey(grid.dataSource.data(), "KendoGridAccpacViewPrimaryKey", keyId);

        if (rowData) {
            rowData.isNewLine = false;
            rowData.dirty = false;
            sg.utls.fieldCopy(jsonResult.Data, rowData);
        } else {
            grid.dataSource.data().forEach(function (row) { row.isNewLine = false;});
        }

        //seprate the logic from showMessage
        const showError = jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Errors);
        // Show warnings or not based on settings(pjc require improvements)
        const showMessage = _showWarnings[gridName] ? jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Warnings || jsonResult.UserMessage.Errors || jsonResult.UserMessage.Info)
            : showError;

        if (showMessage) {
             //Due to showMessage not handle UserMessage.Info as normal, implement the extra condition to disply Info from view, can be revert if we update shwoMessage function
            if (jsonResult.UserMessage.Info) {
                var infoHTML = sg.utls.generateList(jsonResult.UserMessage.Info, null);
                sg.utls.showMessageInfo(sg.utls.msgType.INFO, infoHTML);
            }
            else {
                sg.utls.showMessage(jsonResult);
            }
        }

        const formId = _forms[gridName];
        //Show changes saved message for detail pop-up
        if (!showError && formId && $(`#${formId}`).is(":visible")) {
            _showChangesSavedMessage();
        }


        _setInsertNewLine(gridName);
            if (isNewLine) {
            _addLine(gridName);
        }
        _gridCallback(gridName, "gridAfterInsert");
        _postMessage(gridName, "InsertLine", jsonResult.data, "");
    }

    /**
     * @description Send insert request error
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _insertError(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid"),
            rowIndex = grid.select().index();

       // _setModuleVariables(gridName, RowStatusEnum.INSERT, jsonResult, rowIndex, false, false, false);
        _lastErrorResult[gridName].message = jsonResult;
        _lastErrorResult[gridName].colName = _lastColField[gridName];
        _skipMoveTo[gridName] = true;
        setTimeout(() => {
            if (_showErrors[gridName]) {
                sg.utls.showMessage(jsonResult);
            }
        }, 100);
        grid.select("tr:eq(" + rowIndex + ")");
        setTimeout(function () {
            _setEditCell(grid, rowIndex, _lastColField[gridName]);
        });
        _valid[gridName] = false;
    }

    /**
 * @description Refresh request success, refresh the grid and select row and column
 * @param {string} gridName The grid name
 * @param {object} jsonResult The request call back result
 * @param {string} fieldName The changed field name
 */
    function _refreshSuccess(gridName, jsonResult, fieldName) {
        var grid = $('#' + gridName).data("kendoGrid"),
            selectRow = grid.select(),
            rowIndex = selectRow.index(),
            status = _lastRowStatus[gridName] === RowStatusEnum.UPDATE ? RowStatusEnum.NONE : _lastRowStatus[gridName],
            dataItem = grid.dataItem(selectRow),
            formId = _forms[gridName];

        // bind form to updated line
        if (formId) {
            const currentRow = Object.assign(dataItem, jsonResult.Data);
            bindToForm(gridName, currentRow);
        }


        //seprate the logic from showMessage
        const showError = jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Errors);
        // Show warnings or not based on settings(pjc require improvements)
        const showMessage = _showWarnings[gridName] ? jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Warnings || jsonResult.UserMessage.Errors || jsonResult.UserMessage.Info)
            : showError;
        if (showMessage) {
            //Due to showMessage not handle UserMessage.Info as normal, implement the extra condition to disply Info from view, can be revert if we update shwoMessage function
            if (jsonResult.UserMessage.Info) {
                var infoHTML = sg.utls.generateList(jsonResult.UserMessage.Info, null);
                sg.utls.showMessageInfo(sg.utls.msgType.INFO, infoHTML, () => {
                    _gridCallback(gridName, "gridChanged", jsonResult.Data, fieldName, null, jsonResult);
                    //todo temp changes, due to time consuming, did not figure out the lines below have any side-effect with gridCallback, if not, removed all lines below and improve it later
                    _setModuleVariables(gridName, status, "", rowIndex, true, false);
                    _skipMoveTo[gridName] = false;

                    if (dataItem) {
                        for (var field in jsonResult.Data) {
                            dataItem[field] = jsonResult.Data[field];
                        }
                    }

                    var lastCellIndex = grid._lastCellIndex;
                    var selectRowChanged = dataItem.uid !== _selectRowUid[gridName];
                    grid.refresh();

                    // After grid refresh, use kendo grid row unique id and column index to select grid row and column
                    var uid = dataItem.uid || _selectRowUid[gridName];
                    var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
                    var colIndex = selectRowChanged ? lastCellIndex : index + 1;
                    var row = grid.table.find("[data-uid=" + uid + "]");

                    if (row.length === 1) {
                        grid.select(row);
                        _setNextEditCell(grid, dataItem, row, colIndex);
                    }
                });
            }
            else {
                sg.utls.showMessage(jsonResult, () => {
                    _gridCallback(gridName, "gridChanged", jsonResult.Data, fieldName, null, jsonResult);
                    //todo temp changes, due to time consuming, did not figure out the lines below have any side-effect with gridCallback, so copy whole things and improve it later
                    _setModuleVariables(gridName, status, "", rowIndex, true, false);
                    _skipMoveTo[gridName] = false;

                    if (dataItem) {
                        for (var field in jsonResult.Data) {
                            dataItem[field] = jsonResult.Data[field];
                        }
                    }

                    var lastCellIndex = grid._lastCellIndex;
                    var selectRowChanged = dataItem.uid !== _selectRowUid[gridName];
                    grid.refresh();

                    // After grid refresh, use kendo grid row unique id and column index to select grid row and column
                    var uid = dataItem.uid || _selectRowUid[gridName];
                    var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
                    var colIndex = selectRowChanged ? lastCellIndex : index + 1;
                    var row = grid.table.find("[data-uid=" + uid + "]");

                    if (row.length === 1) {
                        grid.select(row);
                        _setNextEditCell(grid, dataItem, row, colIndex);
                    }
                });
            }
        }
        else {
            _gridCallback(gridName, "gridChanged", jsonResult.Data, fieldName, null, jsonResult);
            //todo temp changes, due to time consuming, did not figure out the lines below have any side-effect with gridCallback, so copy whole things and improve it later
            _setModuleVariables(gridName, status, "", rowIndex, true, false);
            _skipMoveTo[gridName] = false;

            if (dataItem) {
                for (var field in jsonResult.Data) {
                    dataItem[field] = jsonResult.Data[field];
                }
            }

            var lastCellIndex = grid._lastCellIndex;
            var selectRowChanged = dataItem.uid !== _selectRowUid[gridName];
            grid.refresh();

            // After grid refresh, use kendo grid row unique id and column index to select grid row and column
            var uid = dataItem.uid || _selectRowUid[gridName];
            var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
            var colIndex = selectRowChanged ? lastCellIndex : index + 1;
            var row = grid.table.find("[data-uid=" + uid + "]");

            if (row.length === 1) {
                grid.select(row);
                _setNextEditCell(grid, dataItem, row, colIndex);
            }
        }
    }

    /**
    * @description shoa changes saved message
    */
    function _showChangesSavedMessage() {
        //Construct User Message
        const message = {
            UserMessage: {
                IsSuccess: true,
                Message: globalResource.SaveSuccessMessage,
            }
        };
        //The message will be dislayed after the page redering, nornaml behavior as other function in new grid
        setTimeout(() => { sg.utls.showMessage(message) });
    }

    /**
     * @description Update request success, set update row dirty flag as false
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     * @param {string} uid The unique id used to identify update row
     * @param {short} insertedIndex A flag to indicate whether a insert request is pending
     */
    function _updateSuccess(gridName, jsonResult, uid, insertedIndex) {
        var grid = $('#' + gridName).data("kendoGrid"),
            rowIndex = grid.select().index(),
            status = _lastRowStatus[gridName] === RowStatusEnum.UPDATE ? RowStatusEnum.NONE : _lastRowStatus[gridName];

        _gridCallback(gridName, "gridUpdated", jsonResult.Data, "");
        _setModuleVariables(gridName, status, "", rowIndex, true, false);


        //seprate the logic from showMessage
        const showError = jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Errors);
        // Show warnings or not based on settings(pjc require improvements)
        const showMessage = _showWarnings[gridName] ? jsonResult && jsonResult.UserMessage && (jsonResult.UserMessage.Warnings || jsonResult.UserMessage.Errors || jsonResult.UserMessage.Info)
            : showError;

        if (showMessage) {
            //Due to showMessage not handle UserMessage.Info as normal, implement the extra condition to disply Info from view, can be revert if we update shwoMessage function
            if (jsonResult.UserMessage.Info) {
                var infoHTML = sg.utls.generateList(jsonResult.UserMessage.Info, null);
                sg.utls.showMessageInfo(sg.utls.msgType.INFO, infoHTML);
            }
            else {
                sg.utls.showMessage(jsonResult);
            }
        }

        const formId = _forms[gridName];
        //Show changes saved message for detail pop-up
        if (!showError && formId && $(`#${formId}`).is(":visible")) {
            _showChangesSavedMessage();
        }
        

        //Update row is not the same as select row when update, find update row
        if (uid) {
            var updateRow = grid.table.find("[data-uid=" + uid + "]");
            if (updateRow.length === 1) {
                var dataItem = grid.dataItem(updateRow);
                dataItem.dirty = false;
            }
        }
        if (insertedIndex !== null && insertedIndex > -1) {
            _sendRequest(gridName, RequestTypeEnum.Create, "", false, insertedIndex, jsonResult.Data);
        }
    }

    /**
     * @description Send update request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {any} fieldName The update field name
     * @param {string} uid The unique id used to identify the updated row
     */
    function _updateError(gridName, jsonResult, fieldName, uid) {
        var grid = $('#' + gridName).data("kendoGrid"),
            selectRow = grid.select(),
            rowIndex = selectRow.index();
            
        //Return previous valid value when update error
        if (0 <= rowIndex ) {
            var newLine = _newLine[gridName],
                dataItem = grid.dataItem(selectRow);

            _setModuleVariables(gridName, RowStatusEnum.UPDATE, "", rowIndex, false, false, newLine);
            setTimeout(() => {
                if (_showErrors[gridName]) {
                    sg.utls.showMessage(jsonResult);
                }
            }, 100);

            setTimeout(() => {
                if (_showErrors[gridName]) {
                    sg.utls.showMessage(jsonResult);
                }
            }, 100);

            if (typeof fieldName !== 'undefined') {
                dataItem[fieldName] = _lastErrorResult[gridName][fieldName + "Value"];

                var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName, true);
                var row;
                if (uid) {
                    row = grid.table.find("[data-uid=" + uid + "]");
                    _lastRowNumber[gridName] = row.index();
                    grid.select(row);
                } else {
                    row = _selectGridRow(grid, dataItem["KendoGridAccpacViewPrimaryKey"]);
                }
                grid._lastCellIndex = index - 1;
                grid.editCell(row.find(">td").eq(index));
            }
        }
    }

    /**
     * @description MoveTo request success
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _moveToSuccess(gridName, jsonResult) {
        // bind form to current line
        const grid = _getGrid(gridName);
        const currentRow = grid.dataItem(grid.select());
        bindToForm(gridName, currentRow);

        _gridCallback(gridName, "gridAfterSetActiveRecordCompleted", jsonResult.Data, "");
    }

    /**
    * @description Send delete request success, refresh the grid, reset error object
    * @param {string} gridName The grid name
    * @param {object} jsonResult The request call back result
    * @param {string} fieldName update field name
    */
    function _deleteSuccess(gridName, jsonResult) {
        _lastErrorResult[gridName] = {};
        _dataChanged[gridName] = false;
        _skipMoveTo[gridName] = false;

        var grid = $('#' + gridName).data("kendoGrid"),
            ds = grid.dataSource,
            page = ds.page(),
            length = ds.data().length;

        _lastGridAction[gridName] = RequestTypeEnum.Delete;

        page > 1 ? ds.page(length > 1 ? page : page - 1) : ds.page(1);
        _gridCallback(gridName, "gridAfterDelete");
    }
    /**
     * @description Send delete request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     */
    function _deleteError(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.read();
        setTimeout(() => {
            if (_showErrors[gridName]) {
                sg.utls.showMessage(jsonResult);
            }
        }, 100);
    }

    /**
     * @description Refresh current select row
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {string} fieldName The changed field name
     */
    function _refreshRow(gridName, jsonResult, fieldName) {
        var grid = $('#' + gridName).data("kendoGrid"),
            refreshRowData = grid.dataItem(grid.select()),
            status = _lastRowStatus[gridName] === RowStatusEnum.UPDATE ? RowStatusEnum.NONE : _lastRowStatus[gridName];

        if (refreshRowData) {
            var uid = refreshRowData.uid;
            for (var field in jsonResult.Data) {
                if (refreshRowData.hasOwnProperty(field)) {
                    refreshRowData[field] = jsonResult.Data[field];
                }
            }
            var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName) + 1;
            grid.refresh();
            var row = grid.table.find("[data-uid=" + uid + "]");
            grid.select(row);
            var rowIndex = grid.select().index();
            if (colIndex) {
                _setNextEditCell(grid, refreshRowData, row, colIndex);
            }
            _setModuleVariables(gridName, status, "", rowIndex, true, false);
        }

        _gridCallback(gridName, "gridAfterRefreshRow");
    }

    /**
     * @description Reset current select row
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     */
    function _resetRow(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid"),
            refreshRowData = grid.dataItem(grid.select()),
            status = _lastRowStatus[gridName] === RowStatusEnum.UPDATE ? RowStatusEnum.NONE : _lastRowStatus[gridName];

        if (refreshRowData) {
            for (var field in jsonResult.Data) {
                if (refreshRowData.hasOwnProperty(field)) {
                    refreshRowData[field] = jsonResult.Data[field];
                }
            }
            grid.refresh();
            var rowIndex = grid.select().index();
            _setModuleVariables(gridName, status, "", rowIndex, true, false);
        }
    }

    /**
     * @description: Get custom controller url
     * @param {string} gridName: grid name
     * @param {string} requestName: request name
     * @returns {string} return custom controller url
     */
    function _getRequestUrl(gridName, requestName) {
        var url = sg.utls.url.buildUrl("Core", "Grid", requestName);
        var controllerUrl = window[gridName + "Model"].GridDataServiceController;

        if (controllerUrl && controllerUrl.indexOf('/') > -1) {
            if (controllerUrl.endsWith("Controller")) {
                controllerUrl = controllerUrl.replace("Controller", "");
            }
            var defaultUrl = controllerUrl.startsWith("/") ? "/Core/Grid" : "Core/Grid";
            url = url.replace(defaultUrl, controllerUrl);
        }
        return url;
    }

    /**
     * @description Send sync ajax request
     * @param {string} gridName The grid name
     * @param {number} requestType The request type
     * @param {string} fieldName The field name
     * @param {boolean} isNewLine The boolean flag to indicate whether add new line
     * @param {number} insertedIndex The inserted line index
     * @param {object} rowData The grid row data
     * @param {function} callBack The callBack function after the action is completed
     */
    function _sendRequest(gridName, requestType, fieldName, isNewLine, insertedIndex, rowData, callBack) {
        var grid = _getGrid(gridName);
        var record = rowData || grid.dataItem(grid.select());

        // Do not call server for custom grid
        if (window[gridName + "Model"].GridType === GridTypeEnum.Custom) {
            // client determines behaviours through callbacks
            if (callBack && typeof callBack === "function") {
                callBack();
            }
            return;
        }

        if (record) {
            record.IsSequenceRevisionList = window[gridName + 'Model'].IsSequenceRevisionList;
        }
 
        // D-40982
        removeDisabledFieldsFromAjaxPayload(record);

        // Only pass grid visible(configure) columns 
        if (requestType === RequestTypeEnum.Insert) {
            record = removeHideFieldsFromAjaxPayload(record, grid);
        }

        if (grid) {
            var data = {
                'viewID': $("#" + gridName).attr('viewID'),
                'record': record,
                'fieldName': fieldName
            };
            var requestName = _getRequestName(requestType);
            var url = _getRequestUrl(gridName, requestName);

            if (requestType === RequestTypeEnum.Refresh) {
                const model = window[gridName + "Model"];
                data.cols = model.ColumnDefinitions.map(x => (
                    {
                        "FieldName": x.FieldName,
                        "ViewID": x.ViewID,
                        "PrimaryKeys": x.PrimaryKeys,
                        "ForeignKeys": x.ForeignKeys,
                        "GridFieldName": x.GridFieldName
                    }
                ));
                data.columnsFromConfig = model.ColumnsFromConfig;
            }

            if (requestType === RequestTypeEnum.ClearNewRow) {
                var ds = grid.dataSource;
                const lastRecordIndex = _lastRowNumber[gridName] - 1;
                let lastRecord = null;
                insertedIndex = 0;
                if (lastRecordIndex > 0) {
                    lastRecord = ds.at(lastRecordIndex);
                    insertedIndex = lastRecordIndex + 1;
                }
                else if (grid.dataSource.total() > 1 && _lastRecord[gridName]) {
                    //because the datasource doesn't contain last page's rows, get lastrecord from there it doesnt work
                    lastRecord = _lastRecord[gridName];
                    insertedIndex = lastRecordIndex + 1;
                }

                ds.remove(record);
                data = {
                    'viewID': $("#" + gridName).attr('viewID'),
                    'record': lastRecord
                };
            }

            sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                var isSuccess = true;
                if (jsonResult && jsonResult.UserMessage.Errors && jsonResult.UserMessage.Errors.length > 0) {
                    isSuccess = false;
                }
                _requestComplete(requestType, isSuccess, gridName, jsonResult, fieldName, isNewLine, insertedIndex, rowData ? rowData.uid : "", callBack);
            });
        }
    }

    /**
     * @description Remove hide fields(except key fields) from Ajax payload
     * @param {object} record The record object
     */
    function removeHideFieldsFromAjaxPayload(record, grid) {
        let newRecord = {};
        const colNames = grid.columns.filter(c => !c.isOptionalField).map(c => c.field);
        for (const field in record) {
            if (colNames.includes(field)) {
                newRecord[field] = record[field];
            }
        }
        return newRecord;
    }

    /**
     * @description D-40982 - Remove disabled fields from Ajax payload
     * @param {object} record The record object
     */
    function removeDisabledFieldsFromAjaxPayload(record) {
        var ATTRIBUTE_DISABLED = 4;

        if (record && record.AccpacViewFieldAttributes) {
            // Remove any fields marked as disabled from the record object BEFORE
            // we make the ajaxPostSync call.

            // 1. Find any fields whose attribute is marked as 4 (disabled)
            var disabledFields = [];
            for (var property in record.AccpacViewFieldAttributes) {
                var propertyValue = record.AccpacViewFieldAttributes[property];
                if (propertyValue === ATTRIBUTE_DISABLED) {
                    disabledFields.push(property);
                }
            }
            // 2. If disabled property found, delete it
            if (disabledFields.length > 0) {
                // Note: The next line will not pass unit testing
                //disabledFields.forEach(element => delete record[element]);
                // Note: The following block WILL PASS unit testing
                for (var i = 0; i < disabledFields.length; i++) {
                    var name = disabledFields[i];
                    delete record[name];
                }
            }
        }
    }

    /**
     * @description Get event object for custom callback
     * @return {object} Return event object
     */
    function _getEventObject() {
        var _isProceed = true;
        /** inner function */
        function preventDefault() {
            _isProceed = false;
        }
        /** inner function */
        function proceed() {
            _isProceed = true;
        }
        /** 
         *  inner function 
         *  @return {booean} Return a booean flag to indicate whether continue or stop.
         */
        function isProceed() {
            return _isProceed;
        }
        return {
            preventDefault: preventDefault,
            proceed: proceed,
            isProceed: isProceed
        };
    }

    /**
     * @description Attach column level edit call back handler
     * @param {any} gridName The name of grid
     * @param {any} field The column field
     * @param {any} editor The editor
     */
    function _columnCallBackEdit(gridName, field) {
        //Custom plug in for 'columnEndEdit', custom function when editor lost focus
        $("#" + field).blur(function () {
            _columnCallback(gridName, "columnEndEdit", field, "", this);
        });
    }

    /**
     * @description Custom plugin, column level custom call back
     * @param {string} gridName The grid name
     * @param {string} functionName The call back function name
     * @param {string} field The field Name
     * @param {object} finder The column finder object
     * @param {object} editor The column editor object
     * @return {boolean} The boolean flag to indicate whether continue or stop
     */
    function _columnCallback(gridName, functionName, field, finder, editor) {
        var grid = _getGrid(gridName);
        if (!grid) {
            return true;
        }
            var col = grid.columns.filter(function (c) { return c.field === field; })[0];
        if (!col) {
            return true;
        }
        var data = col.customFunctions;
        if (data && data instanceof Array) {
            var cbFunctions = data.filter(function (o) { return o.hasOwnProperty(functionName); });
            if (cbFunctions.length > 0) {
                var cbFuncName = cbFunctions[0][functionName];
                if (cbFuncName) {
                    var callback = _getFunction(cbFuncName);
                    if (callback && typeof callback === "function") {
                        var event = _getEventObject(),
                            record = grid.dataItem(grid.select()),
                            value = record ? record[field] : '';

                        switch (functionName) {
                            case "columnDoubleClick":
                            case "columnBeforeEdit":
                            case "columnChanged":
                                callback(record, event, field);
                                return event.isProceed();
                            case "columnStartEdit":
                            case "columnEndEdit":
                                callback(record, event, field, editor);
                                return event.isProceed();
                            case "columnBeforeDisplay":
                                callback(value, event);
                                return event.isProceed();
                            case "columnBeforeFinder":
                            case "columnFinderFocus":
                                callback(record, finder, event, field);
                                return true;
                        }
                    }
                }
            }
        }
        return true;
    }

    /**
     * @description Custom plugin, column level settings custom call back
     * @param {string} gridName The grid name
     * @param {string} functionName The call back function name
     * @param {object} column The updated column object
     * @return {boolean} The boolean flag to indicate whether continue or stop
     */
    function _columnSettingCallback(gridName, functionName, column) {
        var data = column.CustomFunctions;
        if (data && data instanceof Array) {
            var cbFunctions = data.filter(function (o) { return o.hasOwnProperty(functionName); });
            if (cbFunctions.length > 0) {
                var cbFuncName = cbFunctions[0][functionName];
                if (cbFuncName) {
                    var callback = _getFunction(cbFuncName);
                    if (callback && typeof callback === "function") {
                        callback(null, column);
                    }
                }
            }
        }
        return true;
    }

     /**
     * @description Custom plugin, grid level custom call back
     * @param {string} gridName The grid name
     * @param {string} functionName The call back function name
     * @param {string} record The selected row data
     * @param {string} fieldName The changed field name
     * @param {int} actionType The last grid action
     * @param {object} jsonResult The result object coming from the server side
     * @return {boolean} The boolean flag to indicate whether continue or stop
     */
    function _gridCallback(gridName, functionName, record, fieldName, actionType, jsonResult) {
        var viewModel = window[gridName + "Model"];
        var data = viewModel.CustomFunctions;
        if (data && data instanceof Array ) {
            var cbFunctions = data.filter(function (o) { return o.hasOwnProperty(functionName); });
            if (cbFunctions.length > 0) {
                var cbFuncName = cbFunctions[0][functionName];
                if (cbFuncName) {
                    var callback = _getFunction(cbFuncName);
                    if (callback && typeof callback === "function") {
                        var grid = _getGrid(gridName),
                            event = _getEventObject();

                        record = record || grid.dataItem(grid.select());
 
                        switch (functionName) {
                            case "gridAfterLoadData":
                                callback(grid.dataSource.data(), actionType);
                                _lastGridAction[gridName] = undefined;
                                return true;
                            case "gridChanged":
                                callback(record, fieldName, jsonResult);
                                return true;
                            case "gridUpdated":
                                callback(record, fieldName);
                                return true;
                            case "gridAfterSetActiveRecordCompleted":
                            case "gridAfterSetActiveRecord":
                            case "gridAfterCreate":
                            case "gridAfterInsert":
                                callback(record);
                                return true;
                            case "gridAfterRefreshRow":
                                callback();
                                return true;
                            case "gridAfterDelete":
                                const isEmpty = grid.dataSource.total() - 1 === 0;
                                callback(record, isEmpty);
                                return true;
                            case "gridBeforeCreate":
                            case "gridBeforeDelete":
                               callback(record, event);
                                return event.isProceed();
                            case "gridAfterError":
                                callback();
                                return true;
                        }
                    }
                }
            }
        }
        return true;
    }

    /**
     * @description Set module(class) variabels
     * @param {string} gridName The grid name
     * @param {number} status The grid action status (Update, create, none)
     * @param {object} error The error object
     * @param {number} rowIndex The row index
     * @param {boolean} isValid The boolen flag, grid data is valid
     * @param {boolean} isSetDefaultRow The boolean flag, whether set default select grid row 
     * @param {boolean} isNewLine The boolen flag
     */
    function _setModuleVariables(gridName, status, error, rowIndex, isValid, isSetDefaultRow, isNewLine) {
        _lastRowStatus[gridName] = status;
        _lastRowNumber[gridName] = rowIndex;

        if (!_lastErrorResult[gridName]) {
            _lastErrorResult[gridName] = {};
        }
        _lastErrorResult[gridName].message = error;
        _lastErrorResult[gridName].colName = _lastColField[gridName];

        if (isNewLine !== undefined) {
            _newLine[gridName] = isNewLine;
        }
        if (isValid !== undefined) {
            _valid[gridName] = isValid;
        }
        if (isSetDefaultRow !== undefined) {
            _setDefaultRow[gridName] = isSetDefaultRow;
        }
    }

    /**
     * @description Initial module(class) variabels
     * @param {string} gridName The grid name
     */
    function _initModuleVariables(gridName) {
        _setDefaultRow[gridName] = true;
        _lastRowNumber[gridName] = -1;
        _lastColField[gridName] = "";
        _lastRowStatus[gridName] = RowStatusEnum.NONE;
        _newLine[gridName] = false;
        _lastErrorResult[gridName] = {};
        _valid[gridName] = true;
        _dataChanged[gridName] = false;
        _allowInsert[gridName] = true,
        _allowDelete[gridName] = true,
        _currentPage[gridName] = 1;
        _filter[gridName] = "";
        _readOnlyColumns[gridName] = [];
        _sendChange[gridName] = true;
        _selectRowUid[gridName] = "";
        _pageByKeyType[gridName] = PageByKeyTypeEnum.FirstPage;
        _refreshKey[gridName] = null;
        _showWarnings[gridName] = true;
        _showErrors[gridName] = true;
        _selectedRow[gridName] = -1;
    }

    /**
     * @description For paging with key field, track which button was clicked
     * @param {string} gridName The grid name
     */
    function _initPageByKeyPageClick(gridName) {

        var model = window[gridName + "Model"];
        if (model.PageByKey) {
            var gridPager = _getGrid(gridName).pager.element;
            gridPager.find(".k-pager-first").click(function () {
                _pageByKeyType[gridName] = PageByKeyTypeEnum.FirstPage;
            });
            gridPager.find(".k-i-arrow-60-left").closest(".k-pager-nav").click(function () {
                _pageByKeyType[gridName] = PageByKeyTypeEnum.PreviousPage;
            });
            gridPager.find(".k-i-arrow-60-right").closest(".k-pager-nav").click(function () {
                _pageByKeyType[gridName] = PageByKeyTypeEnum.NextPage;
            });
            gridPager.find(".k-pager-last").click(function () {
                _pageByKeyType[gridName] = PageByKeyTypeEnum.LastPage;
            });

            // total is hidden since total and page are not available
            gridPager.find(".k-pager-info").hide();
        }
    }

    /**
     * @description For paging with key field, Kendo grid cannot use the page number and total to determine whether to disable paging buttons.
     * If page is the first page, disable the first and previous page buttons. If page is the last page, disable the next and last page buttons.
     * @param {string} gridName The grid name
     * @param {bool} isFirstPage Is the current page the first page
     * @param {bool} isLastPage Is the current page the last page
     */
    function _setPageByKeyPagerDisable(gridName, isFirstPage, isLastPage) {
        var model = window[gridName + "Model"];
        if (model.PageByKey) {
            var gridPager = _getGrid(gridName).pager.element;
            var disabledClass = 'k-state-disabled';
            if (isFirstPage) {
                gridPager.find(".k-pager-first").addClass(disabledClass);
                gridPager.find(".k-i-arrow-60-left").closest(".k-pager-nav").addClass(disabledClass);
            }
            else {
                gridPager.find(".k-pager-first").removeClass(disabledClass);
                gridPager.find(".k-i-arrow-60-left").closest(".k-pager-nav").removeClass(disabledClass);
            }
            if (isLastPage) {
                gridPager.find(".k-pager-last").addClass(disabledClass);
                gridPager.find(".k-i-arrow-60-right").closest(".k-pager-nav").addClass(disabledClass);
            }
            else {
                gridPager.find(".k-pager-last").removeClass(disabledClass);
                gridPager.find(".k-i-arrow-60-right").closest(".k-pager-nav").removeClass(disabledClass);
            }
        }
    }

    /**
     * @description Get column name(field name) by select grid cell index
     * @param {object} grid The kendo grid
     * @param {number} cellIndex The selected cell index
     * @return {string} return column field name
     */
    function _getFieldNameByCellIndex(grid, cellIndex) {
        var viewColumns = grid.columns.filter(function (c) {
            return c.hidden === false;
        });
        return cellIndex > -1 && cellIndex < viewColumns.length ? viewColumns[cellIndex].field : "";
    }

    /**
     * deselect grid row
     * @param {any} gridName
     */
    function clearSelection(gridName) {
        let grid = $("#" + gridName).getKendoGrid();
        grid.clearSelection();
        _lastRowNumber[gridName] = -1;
        _selectRowUid[gridName] = "";
        _refreshKey[gridName] = null;
    }

    /**
    * @description Initialize a grid without auto-select after databound, creating dataSource to binding grid, register events handler
    * @param {string} gridName The name of the grid.
    * @param {boolean} readOnly Whether the grid allows editing(Optional).
    * @param {object} updateColumnDefs Function or funcion name. To update the column definitions before build grid column
    * @param {boolean} showDetailButton Whether the grid toolbar show detail/tax button or not.
    * @param {string} showDetails The name of the show details function.
    * @param {string} formName The name of the detail form.
    * @param {boolean} showEditButton Whether the grid toolbar show edit button or not.
    * @param {number} gridHeight Grid height
    * @return {object} Return kendo grid object
    */
    function initGridNoDefaultSelect(gridName, readOnly, updateColumnDefs, showDetailButton = false, showDetails = null, formName = null, showEditButton = true, gridHeight = 430) {
        sg.viewList.init(gridName, readOnly, updateColumnDefs, showDetailButton, showDetails, formName, showEditButton, gridHeight, false);
    }

    /**
     * @description Initialize a grid, creating dataSource to binding grid, register events handler
     * @param {string} gridName The name of the grid.
     * @param {boolean} readOnly Whether the grid allows editing(Optional).
     * @param {object} updateColumnDefs Function or funcion name. To update the column definitions before build grid column
     * @param {boolean} showDetailButton Whether the grid toolbar show detail/tax button or not.
     * @param {string} showDetails The name of the show details function.
     * @param {string} formName The name of the detail form.
     * @param {boolean} showEditButton Whether the grid toolbar show edit button or not.
     * @param {number} gridHeight Grid height
     * @param {boolean} autoSelect auto select first row after databound
     * @return {object} Return kendo grid object
     */
    function init(gridName, readOnly, updateColumnDefs, showDetailButton = false, showDetails = null, formName = null, showEditButton = true, gridHeight = 430, autoSelect = true) {

        const self = this;

        if (updateColumnDefs) {
            typeof updateColumnDefs === "function" ? updateColumnDefs() : _getFunction(updateColumnDefs)();
        }

        if (showDetails) {
            _showDetails[gridName] = showDetails;
        }
        if (formName) {
            _forms[gridName] = formName;

            // add error update handlers on each input in the form
            $(`#${formName} :input`).focus(function () {
                const databind = $(this).attr("data-bind");
                if (databind) {
                    let options = {};
                    options.model = {};
                    // get value binding property name
                    options.field = getBindingProperty(databind, 'value');
                    options.model[options.field] = this.value;
                    // save the value and column to revert to on error
                    _setEditorInitialValue(gridName, options);
                }
            });
        }

        window.addEventListener("message", _receiveMessage, false);
        var model = window[gridName + "Model"],
            columns = _getGridColumns(gridName),
            addTemplate = kendo.format(BtnTemplate, 'btn-add', 'sg.viewList.addLine(&quot;' + gridName + '&quot;)', globalResource.AddLine, gridName, "Add"),
            delTemplate = kendo.format(BtnTemplate, 'btn-delete', 'sg.viewList.deleteLine(&quot;' + gridName + '&quot;)', globalResource.DeleteLine, gridName, "Delete"),
            editTemplate = kendo.format(BtnTemplate, 'btn-edit-column', 'sg.viewList.editColumnSettings(&quot;' + gridName + '&quot;)', globalResource.EditColumns, gridName, "EditCol"),
            detailTemplate = kendo.format(BtnShowHideTemplate, 'btn-details', 'sg.viewList.showDetails(&quot;' + gridName + '&quot;)', globalResource.ViewDetails, gridName, "ShowDetails", showDetailButton ? "" : "display:none");

        readOnly = readOnly || model.ReadOnly,
        _initModuleVariables(gridName);

        if (_gridList.indexOf(gridName) < 0) { _gridList.push(gridName); }

        $("#" + gridName).kendoGrid({
            height: model.Height ||gridHeight,
            columns: columns,
            autoBind: false,
            navigatable: true,
            reorderable: true,
            filterable: false,
            resizable: true,
            selectable: true,
            sortable: model.PageSize && model.PageSize > 20 ? true : false,
            persistSelection: true,
            editable: readOnly ? false : {
                mode: "incell",
                confirmation: false,
                createAt: "bottom"
            },
            //grid select change
            change: function (e) {
                var grid = _getGrid(gridName),
                    lastRowNumber = _lastRowNumber[gridName],
                    errorMessage = _lastErrorResult[gridName].message,
                    row = grid.select(),
                    selectedIndex = row.index(),
                    record = grid.dataItem(row),
                    pageSize = grid.dataSource.pageSize();

                //custom call back when select new row
                if (selectedIndex !== lastRowNumber) {
                    _gridCallback(gridName, "gridAfterSetActiveRecord", record);
                }

                //show last error
                if (selectedIndex !== lastRowNumber && errorMessage) {
                    setTimeout(function (error) {
                        grid.select("tr:eq(" + lastRowNumber + ")");
                        _gridCallback(gridName, "gridAfterError");
                        sg.utls.showMessage(error);
                    }.bind(null, errorMessage));

                    if (!errorMessage.UserMessage.IsSuccess) {
                        const formId = _forms[gridName];
                        //reset error message for detail pop-up due we we have a option to move line without save.
                        if (formId && $(`#${formId}`).is(":visible")) {
                            _lastErrorResult[gridName].message = "";
                        }
                        return;
                    }
                }
                else if (errorMessage) {
                    _gridCallback(gridName, "gridAfterError");
                }

                //Skip insert or update if the record is out of range of the page size
                if (lastRowNumber > -1 && lastRowNumber < pageSize && !window[gridName + "Model"].CustomGridMapperDefinitions) {
                    if (selectedIndex !== lastRowNumber) {
                        var lastRowData = grid.dataSource.data()[lastRowNumber];
                        const formId = _forms[gridName];
                        let commitRecord = true;
                        //Skip update/insert detail if detail is not dirty based on the value we passed in.
                        if (formId && $(`#${formId}`).is(":visible") && !_commitDetail[gridName]) {
                            commitRecord = false;
                        }
                        if (commitRecord && lastRowData) {
                            if (lastRowData.isNewLine) {
                            	_sendRequest(gridName, RequestTypeEnum.Insert, "", false, -1, lastRowData);
                            } else if (lastRowData.dirty) {
                                _sendRequest(gridName, RequestTypeEnum.Update, "", false, -1, lastRowData);
                            }
                        }
                    }
                }
                if (record) {
                    if (record.uid !== _selectRowUid[gridName]) {
                        // Paging and refresh/update error cause send extra moveto request, skip to send this request
                        if (_skipMoveTo[gridName]) {
                            _skipMoveTo[gridName] = false;
                        } else {
                            let rowData = grid.dataItem(grid.select());
                            let isNewLine = rowData.isNewLine || rowData.KendoGridAccpacViewIsRecordNew;

                            if (!isNewLine) {
                                _sendRequest(gridName, RequestTypeEnum.MoveTo, "", rowData.isNewLine, -1, rowData);
                            }
                        }
                    }
                    _selectRowUid[gridName] = record.uid;
                }
            },

            dataBound: function (e) {
                var grid = e.sender,
                    rows = this.items(),
                    page = _currentPage[gridName], //Dev Note grid.dataSource.page() returns wrong page if there is error before paging and switch focus to the detail popup in between
                    pageSize = grid.dataSource.pageSize(),
                    ps = pageSize - 1,
                    formId = _forms[gridName];

                //Generate display index for sequence number column
                $(rows).each(function () {
                    var index = $(this).index() + 1 + pageSize * (page - 1);
                    var rowLabel = $(this).find(".displayIndex");
                    $(rowLabel).html(index);
                });

                //only show page size record
                grid.tbody.find("tr:gt(" + ps + ")").hide();

                //Set default select row
                if (_setDefaultRow[gridName] && autoSelect) {
                    grid.select("tr:eq(0)");
                }

                //Custom plug in for 'columnDoubleClick' 
                grid.tbody.find("td").dblclick(function (grid, e) {
                    var col = grid.columns[e.target.cellIndex];
                    if (col) {
                        _columnCallback(gridName, "columnDoubleClick", col.field);
                    }
                }.bind(null, grid));

                // form add new line if empty after delete
                if (_lastGridAction[gridName] === RequestTypeEnum.Delete &&
                    formId && $(`#${formId}`).is(":visible") && grid.dataSource.total() === 0) {
                    toolbarAddLine(gridName);
                }

                // select row after paging or refresh
                if (_selectedRow[gridName] > 0 && autoSelect) {
                    // select last row if specified row doesn't exist
                    if (_selectedRow[gridName] > grid.dataSource.data().length) {
                        _selectedRow[gridName] = grid.dataSource.data().length - 1;
                    }
                    grid.select(grid.tbody.find(`>tr:eq(${_selectedRow[gridName]})`));
                    _selectedRow[gridName] = -1;
                }

                //Custom plug in for 'gridAfterLoadData' call back
                _gridCallback(gridName, "gridAfterLoadData", "", "", _lastGridAction[gridName]);
            },

            edit: function (e) {
                var readOnlyColumns = _readOnlyColumns[gridName],
                    column = this.columns[e.container.index()],
                    grid = e.sender,
                    field = column.field;

                // custom plug in for 'columnStartEdit'
                var isEditable = _columnCallback(gridName, "columnStartEdit", field, "", e.container.find("[name='" + field + "']"));

                if (readOnlyColumns.indexOf(field) > -1 || !isEditable) {
                    var curCell = $('#' +gridName).find(".k-edit-cell");
                    grid.editCell(curCell.next());
                }
            },

            pageable: {
                pageSize: model.PageSize || _defaultPageSize,
                numeric: false,
                buttonCount: 1,
                input: !model.PageByKey, // hide page input for paging with key field
            },

            toolbar: readOnly ? [{ template: detailTemplate }, { template: editTemplate }] : [{ template: addTemplate }, { template: delTemplate }, { template: detailTemplate}, { template: editTemplate }],

            dataSource: {
                serverPaging: true,
                serverFiltering: false,
                serverSorting: false,
                schema: {
                    data: "Data",
                    total: "Total",
                    isFirstPage: "IsFirstPage", // custom field for paging with key field
                    isLastPage: "IsLastPage", // custom field for paging with key field
                    model: {
                        id: "KendoGridAccpacViewPrimaryKey",
                        fields: eval(gridName + "_fields")
                    }
                },
                batch: true,
                change: function (e) {
                    var customGridMapperDefinitions = window[gridName + "Model"].CustomGridMapperDefinitions;
                    var grid = _getGrid(gridName),
                        count = grid.dataSource.total();

                    if (customGridMapperDefinitions) {
                        let column = e.field;
                        let selectedItem = grid.dataItem(grid.select());
                        if (column) {
                            let val = selectedItem[column];
                            var dataRows = grid.items();
                            const rowIndex = dataRows.index(grid.select());
                            const field = customGridMapperDefinitions[column][rowIndex];
                            var data = {
                                'viewID': $("#" + gridName).attr('viewID'),
                                'fieldName': field,
                                'value': val
                            };
                            var url = window.sg.utls.url.buildUrl("Core", "Grid", "SetValue");
                            sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                                var isSuccess = true;
                                if (jsonResult && jsonResult.UserMessage.Errors && jsonResult.UserMessage.Errors.length > 0) {
                                    isSuccess = false;
                                }
                                var lastRowData = grid.dataSource.data()[rowIndex];
                                if (!isSuccess) {
                                    //Return previous valid value when update error
                                    lastRowData.dirty = false;
                                    selectedItem[column] = _lastErrorResult[gridName][column + "Value"];
                                } else {
                                    _dataChanged[gridName] = true;
                                }

                                isSuccess ? _gridCallback(gridName, "gridUpdated", jsonResult.Data, "") : _updateError(gridName, jsonResult, fieldName, lastRowData.uid);
                                var updateRow = grid.table.find("[data-uid=" + lastRowData.uid + "]");
                                _lastRowNumber[gridName] = rowIndex - 1;
                                grid.select(updateRow);
                            });
                        }
                        return;
                    }

                    var disabled = count === 0 || !_allowDelete[gridName];
                    $("#btn" + gridName + "Delete").prop("disabled", disabled);
                    $("#btn" + gridName + "ShowDetails").prop("disabled", count === 0);

                    if (e.action && e.action !== "sync") {
                        _dataChanged[gridName] = true;
                        // this should have been set but seems to be getting set 
                        // incorrectly (AT-83035), so we force it here.
                        _lastColField[gridName] = e.field;
                    }

                    if (e.items.length === 0 && count === 0 && grid.dataSource.page() !== 1) {
                        grid.dataSource.page(1);
                    }

                    if (e.action === "itemchange") {
                        e.preventDefault();
                        if (e.field === "OptionalField" || e.field === "OptionalFieldString" || e.field === 'id'  || e.field ==='KendoGridAccpacViewPrimaryKey') {
                            return;
                        }

                        //Get key field, for ordered revision list. update the key value
                        let keyField = e.field;
                        let keyFields = window[gridName + 'Model'].ColumnDefinitions.filter(c => c.IsPrimaryKeyField);
                        if (keyFields.length > 0) {
                            keyField = keyFields[0].FieldName;
                        }

                        if (e.field === keyField && !window[gridName + 'Model'].IsSequenceRevisionList) {
                            let keyValue = e.items[0][keyField];
                            e.items[0].KendoGridAccpacViewPrimaryKey = keyValue;
                            e.items[0].id = keyValue;
                        }

                        if (!_sendChange[gridName]) {
                            _sendChange[gridName] = true;
                            return;
                        }
                        //Custom plugin for 'columnChanged'
                        var isProceed = _columnCallback(gridName, "columnChanged", e.field);
                        if (!isProceed) {
                            return;
                        }
                        var newLine = e.items && e.items.length > 0 && e.items[0].isNewLine;
                        _sendRequest(gridName, RequestTypeEnum.Refresh, e.field, newLine);
                    }

                    //Adding line at the end of a page will create a dummy line to allow navigating to the next page
                    //Here we will add the actual line on the new page, the count should be page * pageSize, such as 10, 20, 30, ...
                    if (e.action === undefined && e.items && e.items.length === 0 && count !== 0 && count % grid.dataSource.pageSize() === 0) {
                       _addLine(gridName);
                    }
                },

                transport: {
                    read: {
                        url: _getRequestUrl(gridName, "Read"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax(),
                        complete: function (response) {
                            // callback from post
                            _setPageByKeyPagerDisable(gridName, response.responseJSON.IsFirstPage, response.responseJSON.IsLastPage);
                        }
                    },
                    destroy: {
                        url: window.sg.utls.url.buildUrl("Core", "Grid", "Delete"),
                        contentType: "application/json",
                        type: "POST",
                        headers: sg.utls.getHeadersForAjax()
                    },
                    parameterMap: function (data, operation) {
                        data.viewID = $("#" + gridName).attr('viewID');
                        data.columns = model.ColumnDefinitions.map(x => (
                            {
                                "FieldName": x.FieldName,
                                "ViewID": x.ViewID,
                                "PrimaryKeys": x.PrimaryKeys,
                                "ForeignKeys": x.ForeignKeys,
                                "GridFieldName": x.GridFieldName
                            }
                        ));
                        data.columnsFromConfig = model.ColumnsFromConfig;
                        if (operation === "read") {
                            _setModuleVariables(gridName, RowStatusEnum.NONE, "", -1, true, true, false);
                            data.filter = _filter[gridName];
                            data.pageByKey = model.PageByKey;

                            // Pass which page button is clicked and start record for paging with key field
                            if (data.pageByKey) {
                                data.pageType = _pageByKeyType[gridName];
                                var items = _getGrid(gridName).dataSource.data();
                                switch (data.pageType) {
                                    case PageByKeyTypeEnum.Refresh:
                                        data.record = _refreshKey[gridName];
                                        _refreshKey[gridName] = null;
                                        break;
                                    case PageByKeyTypeEnum.PreviousPage:
                                        if (items.length > 0)
                                            data.record = items[0];
                                        break;
                                    case PageByKeyTypeEnum.NextPage:
                                        if (items.length > 0)
                                            data.record = items[items.length - 1];
                                        break;
                                    default:
                                        break;
                                }
                            }

                            return JSON.stringify(data);
                        }
                        else {
                            data.record = data.models[0];
                            return JSON.stringify(data);
                        }
                    }
                },
                //When paging, save the unsaved row
                requestStart: function (e) {
                    var skipCommit = false;
                    const formId = _forms[gridName];
                    //Skip Commit if detail is not dirty for detail pop-up, otherwise, check with row data.
                    if (formId && $(`#${formId}`).is(":visible")) {
                        skipCommit = !_commitDetail[gridName];
                    }
                    else {
                        const row = e.sender.data()[e.sender.pageSize()];
                        skipCommit = row && row.hasOwnProperty("skipCommit") && row["skipCommit"];
                    }

                    // Commit any grid changes if there is a new line or data has changed while pagination
                    if ((_newLine[gridName] && e.type === "read" || _dataChanged[gridName]) && _currentPage[gridName] !== this.page() && !skipCommit) {
                        e.preventDefault();
                        _commitGrid(gridName, (isSuccess) => {
                            if (isSuccess) {
                                _dataChanged[gridName] = false;
                                _newLine[gridName] = false;
                                _currentPage[gridName] = this.page();
                                this.page(this.page());
                            } else {
                                // "this.page" is already set to the new page number when coming into "requestStart". However if _commmitGrid
                                // fails the page switch doesn't happen, therefore "this.page" should be reset back to the "_currentPage"!
                                this.page(_currentPage[gridName]);
                            }
                        });
                        return;
                    }
                    _currentPage[gridName] = this.page();
                }

            }
        });

        //binding the drilldown popup window
        $("#" + gridName).on("click", "tbody > tr > td > a", _initShowPopup);

        $("#" + gridName).on("click", "tbody > tr > td > img", _initShowPopup);

        //When close the pop up error message, focus the last edit cell
        $(document).on("click", ".msgCtrl-close", function (e) {
            _setGridCell.call(this, gridName);
        });

        //Set grid use preferences, such as show/hide column, order and width
        GridPreferencesHelper.setGrid("#" + gridName, model.GridColumnSettings, false, _getGridColumns(gridName));

        _initPageByKeyPageClick(gridName);

        sg.utls.initGridKeyboardHandlers(gridName, true,
        {
            addLine: self.addLine,
            deleteLine: self.deleteLine,
            showDetails: self.showDetails,
            editColumnSettings: self.editColumnSettings,
        },
        {
            addLineButtonName: `btn${gridName}Add`,
            deleteLineButtonName: `btn${gridName}Delete`,
            viewEditDetailsButtonName: '',
            editColumnSettingsButtonName: ''
        });

        // hide toobar buttons for custom grid
        if (window[gridName + "Model"].GridType === GridTypeEnum.Custom) {
            _getGrid(gridName).setOptions({
                toolbar: null
            });
        }

        //Hide the Edit button if showbuttonDetail is false 
        if (!showEditButton) {
            var editButton = $("#btn" + gridName + "EditCol");
            editButton[0].parentElement.style.display = "none";
        }
        return _getGrid(gridName);
    }

    /**
     * @description Delete grid line, for grid toolbar template, grid internal use
     * @param {string} gridName The name of the grid.
     * @param {boolean} showConfirmation Show a confirmation dialog.
     * @param {function} callback The callBack function after the action is completed, when deleting from form.
     */
    function toolbarDeleteLine(gridName, showConfirmation = true, callback = null) {
        var grid = _getGrid(gridName),
            select = grid.select(),
            selectedIndex = select.index(),
            rowData = grid.dataItem(select);

        var isProceed = _gridCallback(gridName, "gridBeforeDelete", rowData);
        if (!isProceed) {
            return;
        }

        var doDelete = () => {
            if (!rowData.isNewLine) {
                _sendRequest(gridName, RequestTypeEnum.Delete, "");
                _dataChanged[gridName] = true;
            } else {
                var ds = grid.dataSource;
                _gridCallback(gridName, "gridAfterDelete", rowData);
                ds.remove(rowData);
                _newLine[gridName] = false;
                _dataChanged[gridName] = false;
                // Set the correct current page
                var page = ds.data().length === 0 && _currentPage[gridName] > 1 ? _currentPage[gridName] - 1 : _currentPage[gridName];
                _lastGridAction[gridName] = RequestTypeEnum.Delete;
                const formId = _forms[gridName];
                //Show changes saved message for detail pop-up
                if (formId && $(`#${formId}`).is(":visible") && callback) {
                    //Reset the delete flags for unsaved line. 
                    _setModuleVariables(gridName, RowStatusEnum.NONE, "", -1, true, true, false);
                    _lastErrorResult[gridName] = {};
                    _dataChanged[gridName] = false;
                    _skipMoveTo[gridName] = false;
                    callback();
                }
                else {
                    ds.page(page);
                }
                
            }
        }

        if (selectedIndex > -1) {
            if (showConfirmation) {
                sg.utls.showKendoConfirmationDialog(
                    function () {
                        doDelete();
                        //_gridCallback(gridName, "gridAfterDelete", rowData);
                    },
                    function () { },
                    globalResource.DeleteLineMessage, // message
                    window.DeleteTitle, // typeOfAction
                    undefined, // isMessageEncoded
                    undefined, // callbackCancel
                    gridName); // gridName (When specified, set focus back to grid when confirmation dialog is closed)
            }
            else {
                doDelete();
            }
        }
    }

    /**
     * @description Add grid new line, for grid toolbar template, grid internal use.
     * @param {string} gridName The grid name.
     * @param {boolean} commitDetail Commit row before adding record.
     * @param {object} lastRecord Record for new line
     */
    function toolbarAddLine(gridName, commitDetail, lastRecord) {
        var grid = _getGrid(gridName),
            rowData = grid.dataItem(grid.select());
        const formId = _forms[gridName];
        //Show changes saved message for detail pop-up
        if (formId && $(`#${formId}`).is(":visible")) {
            _commitDetail[gridName] = commitDetail;
            if (lastRecord) {
                _lastRecord[gridName] = lastRecord;
            }
        }
        //has update error, show error message and return
        if (_lastRowStatus[gridName] === RowStatusEnum.UPDATE && _lastErrorResult[gridName].message !== "") {
            setTimeout(function () {
                grid.select("tr:eq(" + _lastRowNumber[gridName] + ")");
                sg.utls.showMessage(_lastErrorResult[gridName].message);
            });
            return;
        }
        //custom plugin before create new line
        var isProceed = _gridCallback(gridName, "gridBeforeCreate");
        if (!isProceed) {
            return;
        }

        //add new line or send insert request and add new line
        rowData && rowData.isNewLine ? _sendRequest(gridName, RequestTypeEnum.Insert, "", true) : _addLine(gridName, undefined, commitDetail);
    }

    /**
     * @description Edit grid settings, for grid toolbar template, grid internal use.
     * @param {any} gridName The grid name
     */
    function toolbarEditColumn(gridName) {
        var btnEditElement = $("#" + gridName + " .k-grid-toolbar .btn-edit-column");
        GridPreferencesHelper.initialize('#' + gridName, window[gridName + "Model"].UserPreferencesUniqueId, $(btnEditElement), _getGridColumns(gridName));
        $('#chkGridPrefSelectAll').focus();
    }

    /**
    * @description Show grid popup details screen, for grid toolbar template, grid internal use.
    * @param {any} gridName The grid name
    */
    function toolbarShowDetails(gridName) {
        const showDetails = _showDetails[gridName];
        if (typeof showDetails === "function") {
            showDetails.call();
        }
    }

    /**
     * @description Get column template, for grid template intenal use
     * @param {object} data The grid column data.
     * @return {object} return column template
     */
    function getTemplate(data) {
        var type = data.TYPE,
            value = data.VALUE,
            decimals = 3;

        if (value === null) {
            return "";
        } else if (type === ValueTypeEnum.YesNo) {
            return value === "1" || value === 1 ? globalResource.Yes : globalResource.No;
        } else if (type === ValueTypeEnum.Date) {
            return sg.utls.kndoUI.getFormattedDate(value);
        } else if (type === ValueTypeEnum.Time) {
            return value.substring(0, 2) + ":" + value.substring(2, 4) + ":" + value.substring(4, 6);
        } else if (type === ValueTypeEnum.Integer || type === ValueTypeEnum.Amount || type === ValueTypeEnum.Number) {
            return '<span style="float:right">' + sg.utls.kndoUI.getFormattedDecimalNumber(value || 0, decimals) + '</span>';
        } else {
            return sg.utls.htmlEncode(value);
        }
    }

    /**
     * @description Get dropdown list text from value, for grid column dropdown list template(internal use)
     * @param {string} field The column field name
     * @param {object} dataItem The text/value pair list
     * @return {string} return the text by value
     */
    function getListText(field, dataItem) {
        var list = this.filter(function (i) {
            return i.Value.toLowerCase() === ((dataItem[field] || dataItem[field] === 0) ?
                dataItem[field].toString().toLowerCase() : "");
        });
        return list && list.length > 0 ?
            list[0].Text : ((dataItem[field] || dataItem[field] === 0) ?
            dataItem[field] : "");
    }

    /**
 *  @description Sync the current grid select row with server, move the server entity pointer to current entity
 *  @description Used for parent/details grid(popup)
 *  @param {any} gridName The name of the grid
 *  @param {function} callback (optional) callback after move
 */
    function moveToCurrentRow(gridName, callback = null) {
        const grid = _getGrid(gridName),
            rowData = grid.dataItem(grid.select()),
            data = { 'viewID': $("#" + gridName).attr('viewID'), "record": rowData },
            url = sg.utls.url.buildUrl("Core", "Grid", "MoveTo");
        const onCompletion = null != callback ? callback : function() { };
        sg.utls.ajaxPostSync(url, data, onCompletion);
    }

    /**
      * @description Get the grid validation status.
      * @param {any} gridName The name of the grid
      * @return {boolean} Grid valid status
      */    
    function isValid(gridName) {
        return _valid[gridName];
    }

    /**
     * @description Get/Set whether a view list grid has been changed.
     * Note: This flag handles dirty state for the whole grid level.
     *       For line level dirty flag please use sg.viewList.currentRecord(gridName).dirty.
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
     * @description Commit unsaved changes in a grid to the server.
     * @param {string} gridName The name of grid. If omitted, commit changes in all view list grids to the server.
     * @param {function} callBack The callBack function after the action is completed
     * @return {boolean} Return a boolean flag to indicate success or not
     */
    function commit(gridName, callBack) {
        var result = true;
        if (gridName) {
            if (!isEmpty(gridName)) {
                result = _commitGrid(gridName, callBack);
            } else if (callBack && typeof callBack === "function") {
                callBack(true);
            }
        } else {
            for (var i = 0, length = _gridList.length; i < length; i++) {
                if (!isEmpty(_gridList[i])) {
                    result = _commitGrid(_gridList[i], callBack);
                    if (!result) {
                        break;
                    }
                } else if (callBack && typeof callBack === "function") {
                    callBack(true);
                }
            }
        }
        return result;
    }

    /**
     * @description Read data from the server side, refresh the view list display, and reset the page number to 1.
     * @param {any} gridName The name of the grid(optional parameter). If omitted, refresh all view list grids in the screen to the server.
     */
    function refresh(gridName) {
        /**
         * Inner function, for refresh one grid
         * @param {string} gridName The name of grid
         */
        function refreshGrid(gridName) {
            var grid = _getGrid(gridName);
            if (grid) {
                grid.dataSource.page(1);
            }
        }

        if (gridName) {
            refreshGrid(gridName);
        } else {
            for (var i = 0, length = _gridList.length; i < length; i++) {
                refreshGrid(_gridList[i]);
            }
        }
    }

    /**
     * @description Read data from the server side, refresh the view list display, and reset the focus to the current row.
     * @param {string} gridName The name of the grid.
     */
    function refreshAndMoveToCurrent(gridName) {
        if (gridName) {
            const grid = _getGrid(gridName);
            if (grid) {
                _selectedRow[gridName] = grid.select().index(); // set _selectedRow[gridName] so after refreshing, the grid sets focus to this row
                grid.dataSource.read(); // refresh grid
            }
        }
    }

    /**
     * @description Get/Set the insertable property of a grid.
     * @description If the grid does not allow insert, the “Add Line” button should be disabled
     * @param {string} gridName The name of the grid
     * @param {booelan} insertable A boolean flag to indicate whether grid allow insert new line
     * @return {boolean} return true if the grid allows insert or null
     */
    function allowInsert(gridName, insertable) {
        if (insertable !== undefined) {
            $("#btn" + gridName + "Add").prop("disabled", !insertable);
            _allowInsert[gridName] = insertable;
            return;
        }
        return _allowInsert[gridName];
    }
    
    /**
     * @description Get/Set the deletable property of a grid.
     * @description If the grid does not allow insert, the “Delete Line” button should be disabled
     * @param {string} gridName The name of the grid
     * @param {booelan} deletable A boolean flag to indicate whether grid allow insert new line
     * @return {boolean} return true if the grid allows delete or null
     */
    function allowDelete(gridName, deletable) {
        if (deletable !== undefined) {
            $("#btn" + gridName + "Delete").prop("disabled", !deletable);
            _allowDelete[gridName] = deletable;
            return;
        }
        return _allowDelete[gridName];
    }

    /**
     * @description Get/Set grid column visiblity.
     * @param {string} gridName The name of the grid
     * @param {string} columnName The column name
     * @param {boolean} visible A booelan flag
     * @param {boolean} updateConfig Optional. Default true to update grid preferences
     * @return {booean} A boolean flag or none
     */
    function showColumn(gridName, columnName, visible, updateConfig = true) {
        var grid = _getGrid(gridName) || init(gridName);
        if (grid) {
            if (visible !== undefined) {
                // change kendo grid column visibility
                visible ? grid.showColumn(columnName) : grid.hideColumn(columnName);

                // update column definition for grid preferences
                const gridModel = window[gridName + "Model"];
                if (updateConfig && gridModel && gridModel.ColumnDefinitions) {
                    const columns = gridModel.ColumnDefinitions;
                    const column = columns.find(x => x.FieldName == columnName);
                    if (column) {
                        column.IsHidden = !visible;
                    }
                }

                return;
            }
            var column = grid.columns.filter(function (col) { return col.field === columnName; })[0];
            return !column.hidden;
        }
        return;
    }

    /**
     * @description Get/Set grid column template
     * @param {string} gridName The name of grid
     * @param {string} columnName The name of the column(the internal column name, not the title)
     * @param {object} template The column template(kendo template format)
     * @return {object} Return the template definition for the column or null
     */
    function columnTemplate(gridName, columnName, template) {
        var grid = _getGrid(gridName) || init(gridName), column;
        if (grid) {
            var columns = grid.columns.filter(function (col) { return col.field === columnName; });
            if (columns.length === 0) {
                return;
            } else {
                column = columns[0];
            }

            if (template !== undefined) {
                var options = grid.getOptions();
                column = options.columns.filter(function (col) { return col.field === columnName; })[0];
                column.template = template;
                grid.setOptions(options);
                return;
            }
            return column.template;
        }
        return;
    }

    /**
     * @description Get the current record in the grid.
     * @param {string} gridName The name of the grid
     * @return {object} Return object contains fieldname/value pairs for all the fields in the business entity (not only the fields visible in the grid).
     */
    function currentRecord(gridName) {
        var grid = _getGrid(gridName);
        return grid ? grid.dataItem(grid.select()) : null;
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
      * @description Get/Set the read-only property of a grid.
      * @param {any} gridName The name of the grid
      * @param {any} readOnly a boolean flag indicate the grid is read only or not
      * @return {boolean} A Boolean flag or null
      */    
    function readOnly(gridName, readOnly) {
        var grid = _getGrid(gridName) || init(gridName);
        if (readOnly !== undefined) {
            grid.setOptions({ editable: !readOnly });

            $("#btn" + gridName + "Add").prop("disabled", readOnly);
            $("#btn" + gridName + "Delete").prop("disabled", readOnly);
            _allowDelete[gridName] = !readOnly;
            _allowInsert[gridName] = !readOnly;

            return;
        }
        var editable = grid.options.editable;
        return typeof editable === "boolean" ? !editable : false;
    }

    /**
     * @description Get the column count of the grid.
     * @param {string} gridName The name of the grid
     * @return {number} return count of columns in the grid (visible or hidden).
     */
    function columnCount(gridName) {
        var grid = _getGrid(gridName) || init(gridName);
        return grid.columns.length;
    }

    /**
     * @description Get the field name corresponding to the column index in a grid.
     * @param {any} gridName The name of the grid
     * @param {any} columnIndex The column indexc
     * @return {string} The field name of column
     */
    function fieldName(gridName, columnIndex) {
        var grid = _getGrid(gridName) || init(gridName);
        return grid.columns[columnIndex].field;
    }
    
    /**
     * @description Get/Set the column title in a grid.
     * @param {string} gridName The name of the grid
     * @param {object} column A zero-based integer or a field name.
     * @param {string} title The column title(caption)
     * @return {string } Return column title or null.
     */
    function columnTitle(gridName, column, title) {
        var grid = _getGrid(gridName) || init(gridName), columns, col;

        if (typeof column === "string") {
            columns = grid.columns.filter(function (c) { return c.field === column; });
            if (columns.length === 0) {
                return;
            }
        }
        col = typeof column === "string" ? columns[0] : grid.columns[column];

        if (title !== undefined) {
            var options = grid.getOptions();
            columns = options.columns;
            col = typeof column === "string" ? columns.filter(function (c) { return c.field === column; })[0] : columns[column];
            col.title = title;
            grid.setOptions(options);
            return;
        }
        return col ? col.title : "";
    }

    /**
      * @description Get/Set the editable property of a column in a grid.
      * @param {any} gridName The name of the grid
      * @param {any} column A zero-based integer or a field name.
      * @param {booean} editable A Boolean flag, optional parameter
      * @return {booean} A Boolean flag or None.
      */    
    function columnEditable(gridName, column, editable) {
        var grid = _getGrid(gridName) || init(gridName), columns;

        if (typeof column === "string") {
            columns = grid.columns.filter(function (c) { return c.field === column; });
            if (columns.length === 0) {
                return;
            }
        }

        var schemaFields = grid.dataSource.options.schema.model.fields;
        var field = typeof column === "string" ? column : grid.columns[column].field;
        if (editable !== undefined) {
            schemaFields[field].editable = editable;
            return;
        }

        return schemaFields[field].editable;
    }

    /**
     * @description Change the field value in the current record and sync the change to the server.
     * @param {any} gridName The name of the grid.
     * @param {any} fieldname The field name
     * @param {any} value The new value
     */
    function setValue(gridName, fieldname, value) {
        var grid = _getGrid(gridName),
            selectRow = grid.dataItem(grid.select());

        selectRow.set(fieldname, value);
    }

    /**
     * @description Return bool value to indicate if the grid is empty
     * @param {any} gridName The name of the grid.
     * @return {booean} A Boolean flag
     */
    function isEmpty(gridName) {
        var grid = _getGrid(gridName);
        var dataSource = grid.dataSource;

        return dataSource.view().length === 0;
    }
    /**
     * Reset grid status, cancel last row edit
     * @param {any} gridName The grid name
     * @param {boolean} ignoreRead The flag to determinate if grid data read should be executed
     */
    function cancel(gridName, ignoreRead) {
        var grid = _getGrid(gridName);
        _setModuleVariables(gridName, RowStatusEnum.NONE, "", -1, true, false, false);
        grid.clearSelection();

        if(!ignoreRead){
            grid.dataSource.read();
        }
    }
    /**
     * Refresh current select row
     * @param {string} gridName The grid name
     * @param {string} fieldName The field name
     */
    function refreshCurrentRow(gridName, fieldName) {
        _sendRequest(gridName, RequestTypeEnum.RefreshRow, fieldName);
    }

    /**
    * Resets the current selected row, reverting any changes
    * @param {string} gridName The grid name
    * @param {function} callback The callBack function after the action is completed
    */
    function resetCurrentRow(gridName, callback) {
        _sendRequest(gridName, RequestTypeEnum.ResetRow, ...Array(4), callback);
    }

    /**
    * Clear New added row
    * @param {string} gridName The grid name
    */
    function clearNewRow(gridName) {
        _sendRequest(gridName, RequestTypeEnum.ClearNewRow);
    }


    /**
     * Update current select row
     * @param {any} gridName The grid name
     */
    function updateCurrentRow(gridName) {
        _sendRequest(gridName, RequestTypeEnum.Update);
    }

    /**
     * Clear all records in the grid
     * @param {any} gridName The grid name
     */
    function clear(gridName) {
        var grid = _getGrid(gridName);
        var dataSource = grid.dataSource;
        dataSource.data([]);
    }

    /**
     * When PageByKey setting is true, reads data from server from specified record
     * @param {string} gridName The grid name
     * @param {any} record The record to search, containing key fields and values
     */
    function refreshByKey(gridName, record) {
        var model = window[gridName + "Model"];
        if (model.PageByKey && record) {
            _refreshKey[gridName] = record;
            _pageByKeyType[gridName] = PageByKeyTypeEnum.Refresh;
            refresh(gridName);
        }
    }

    /**
    * Show warnings when insert/update record successfully
    * @param {string} gridName The grid name
    * @param {boolean} show The flag to specify whether to show warnings when insert/update record successfully.
    */
    function showWarnings(gridName, show = true) {
        _showWarnings[gridName] = show;
    }

    /**
    * Show errors when insert/update/create/delete record fails
    * @param {string} gridName The grid name
    * @param {boolean} show The flag to specify whether to show errors when insert/update/create/delete record fails.
    */
    function showErrors(gridName, show = true) {
        _showErrors[gridName] = show;
    }

    /**
    *
    * @param {string} gridName The grid name
    */
    function currentPage(gridName) {
        return _currentPage[gridName];
    }

    /**
    * Gets the display line number of selected row
    * @param {string} gridName The grid name
    */
    function getCurrentLineNumber(gridName) {
        var grid = _getGrid(gridName);
        return grid.select().index() + 1 + grid.dataSource.pageSize() * (_currentPage[gridName] - 1);
    }

    /**
    * Selects specified row, paging if necessary
    * @param {string} gridName The grid name
    * @param {number} lineNumber The line number to select
    * @param {boolean} commitDetail True for update/insert record to show 'changes saved' message for detail popup
    */
    function moveToRow(gridName, lineNumber, commitDetail) {
        const grid = _getGrid(gridName);
        const pageSize = grid.dataSource.pageSize();
        const formId = _forms[gridName];
        let nextPage = (lineNumber % pageSize === 0) ? Math.floor(lineNumber / pageSize) : Math.floor(lineNumber / pageSize) + 1; // page containing selected row
        let selectedIndex = (lineNumber % pageSize === 0) ? pageSize - 1 : (lineNumber % pageSize) - 1; // index in page
        //Show changes saved message for detail pop-up
        if (formId && $(`#${formId}`).is(":visible")) {
            _commitDetail[gridName] = commitDetail;
        }
        if (!isNaN(nextPage) && !isNaN(selectedIndex)) {
            // change page and select row in gridAfterLoadData
            if (_currentPage[gridName] !== nextPage) {
                _selectedRow[gridName] = selectedIndex;
                grid.dataSource.page(nextPage);
            // same page, select row
            } else {
                grid.select(grid.tbody.find(`>tr:eq(${selectedIndex})`));
            }
        }
    }

    /**
    * Selects row on right grid-page
    * @param {event} e The grid page change event
    */
    function _selectOnPage(e) {
        const gridName = _gridList.find(e => _getGrid(e).dataSource === this);
        if (gridName !== undefined) {
            if (_refreshKey.hasOwnProperty(gridName)) {
                const grid = _getGrid(gridName);
                const record = _refreshKey[gridName];
                const maxPage = Math.floor(this.total() / this.pageSize()) + 1;
                // see if we are on right page
                for (var i = 0; i < this.data().length; i++) {
                    var rec = this.data()[i];
                    var match = true;
                    for (const key in record) {
                        if (record.hasOwnProperty(key) && rec.hasOwnProperty(key)) {
                            if (record[key] !== rec[key]) {
                                match = false;
                                break;
                            }
                        }
                    }
                    if (match) {
                        // match found, we are done, clean-up and select row
                        _refreshKey[gridName] = null;
                        grid.dataSource.unbind('change', _selectOnPage);
                        grid.select(grid.tbody.find(`>tr:eq(${i})`));
                        return;
                    }
                }
                if (this.page() < maxPage) {
                    // not there yet, move to next page
                    _currentPage[gridName] = this.page() + 1;
                    this.page(this.page() + 1);
                    return;
                } else {
                    // no match found, select the very last row on (last) grid-page
                    _refreshKey[gridName] = null;
                    grid.dataSource.unbind('change', _selectOnPage);
                    grid.select(grid.tbody.find(`>tr:eq(${this.data().length - 1})`));
                    return;
                }

            }
        }
        // something went wrong, abandon endeavour
        grid.dataSource.unbind('change', _selectOnPage);
    }

    /**
    * Selects row as specified by the key(s), paging if necessary
    * @param {string} gridName The grid name
    * @param {any} record The record to search, containing key fields and values
    * @param {boolean} commitDetail True for update/insert record to show 'changes saved' message for detail popup
    */
    function moveToRecord(gridName, record, commitDetail) {

        // check grid for existence and validity
        const grid = _getGrid(gridName);
        if (grid !== null && grid !== undefined) {

            // check record for existence
            if (record === null || record === undefined) {
                return;
            }

            // check record for validity
            for (const key in record) {
                if (record.hasOwnProperty(key)) {
                    if (record[key] === "") {
                        return;
                    }
                } else {
                    return;
                }
            }

            //Show changes saved message for detail pop-up
            const formId = _forms[gridName];
            if (formId && $(`#${formId}`).is(":visible")) {
                _commitDetail[gridName] = commitDetail;
            }

            if (1 <= grid.dataSource.total()) {
                // with luck we are already on correct page
                for (var i = 0; i < grid.dataSource.data().length; i++) {
                    var rec = grid.dataSource.data()[i];
                    var match = true;
                    for (const key in record) {
                        if (record.hasOwnProperty(key) && rec.hasOwnProperty(key)) {
                            if (record[key] !== rec[key]) {
                                match = false;
                                break;
                            }
                        } else {
                            // records mismatch, bail-out
                            return;
                        }
                    }
                    if (match) {
                        if (grid.select() && i !== grid.select().index()) {
                            // select other row on the current grid-page
                            grid.select(grid.tbody.find(`>tr:eq(${i})`));
                        }
                        return;
                    }
                }

                const maxPage = Math.floor(grid.dataSource.total() / grid.dataSource.pageSize()) + 1;
                if (1 < maxPage) {
                    grid.dataSource.bind('change', _selectOnPage);
                    _refreshKey[gridName] = record;
                    if (grid.dataSource.page() === 1) {
                        // no luck on first page, continue with next page
                        _currentPage[gridName] = 2;
                        grid.dataSource.page(2);
                    } else {
                        // no luck on middle/last page(s), start from beginning
                        _currentPage[gridName] = 1;
                        grid.dataSource.page(1);
                    }
                } else {
                    // no matching record found, select the very last row on (last) grid-page
                    grid.select(grid.tbody.find(`>tr:eq(${grid.dataSource.data().length - 1})`));
                }
            }
        }
    }

    /**
    * Binds the specified row to the grid's form
    * @param {string} gridName The grid name
    * @param {object} row Grid row to bind the form to
    */
    function bindToForm(gridName, row) {
        const formId = _forms[gridName];
        if (formId) {
            const grid = _getGrid(gridName);
            const selectedRow = addDisabledProperties(gridName, grid.columns, row);
            kendo.bind($(`#${formId}`), selectedRow);
        }
    }

    /**
    * Adds disabled properties to the grid row, used for data binding.
    * The 'value' data-bind attribute must be set.
    * The property name for the 'disabled' data-bind attribute is either passed in, or [FieldName]Disabled if not defined
    * @param {string} gridName The grid name
    * @param {any} columns The grid columns
    * @param {any} record The grid row data
    * @return {any} The grid row data with a disabled property for each column.
    */
    function addDisabledProperties(gridName, columns, record) {
        if (record) {
            columns.forEach((column) => {
                let disabledBinding = `${column.field}Disabled`;
                const formControl = $(`[data-bind*="value:${column.field}"]`);
                if (formControl && formControl.length > 0) {
                    // get disabled binding property name
                    disabledBinding = getBindingProperty(formControl.attr('data-bind'), 'disabled');
                }
                record[disabledBinding] = readOnly(gridName) || isFieldDisabled(record, column.field);
            });

            // add properties for always disabled, and grid readOnly
            record['GridReadOnly'] = readOnly(gridName);
            record['Disabled'] = true;
        }
        return record;
    }

    /**
    * Parses the data-bind string to get the binding property name
    * @param {string} dataBindString The data-bind string
    * @param {string} name The attribute to get
    * @return {string} The binding property name
    */
    function getBindingProperty(dataBindString, name) {
        for (let dataBind of dataBindString.split(',')) {
            const dataBindValue = dataBind.split(':');
            if (dataBindValue.length > 0) {
                if (dataBindValue[0] === name) {
                    return dataBindValue[1];
                }
            }
        };
        return '';
    }

    /**
    * Checks the field attributes of a row to determine if a field is read only
    * @param {any} record The grid row data
    * @param {string} field The field name
    * @return {boolean} True if field is disabled, false otherwise
    */
    function isFieldDisabled (record, field) {
        if (record) {
            const attr = record.AccpacViewFieldAttributes[field];
            if ((attr & FIELD_DISABLED_ATTRIBUTE) === 0) {
                return true;
            }
        }
        return false;
    }

    /**
     * Get the parent view data from provided by ParentViewIDs
     * @param {string} gridName The grid name
     * @param {function} callBack The callBack function after the action is completed
     */
    function getParentData(gridName, callback) {
        const data = {
            'parentViewIDs': $("#" + gridName).attr('parentViewIDs'),
        };

        const requestName = _getRequestName(RequestTypeEnum.GetParentData);
        const url = _getRequestUrl(gridName, requestName);

        sg.utls.ajaxPost(url, data, (jsonResult) => {
            if (typeof callback === 'function' && jsonResult && jsonResult.UserMessage) {
                if (jsonResult.UserMessage.IsSuccess) {
                    callback(jsonResult);
                } else {
                    sg.utls.showMessage(jsonResult);
                }
            }
        });
    }

    /**
     * Build grid filter string by filter object
     * @param {string} gridName The grid name
     * @param filter filter JS object. the object schema as
          {
           logic: "AND",
           filters: [
              { field: "x", operator: "=", value: "1", dataType: "string"},
              { field: "y", operator: ">", value: 2, dataType: "int"},
              { 
                logic: "OR", 
                filters: [
                    { field: "z", operator: "StartsWith",value: "3", dataType: "string" },
                    { field: "z", operator: ">=",value: 4, dataType: "long" }
                  ]
              }
           ]
         }
     */

    function buildFilterString(gridName, filter) {
        //Convert kendo filter operator to accpac view filter operator
        function convertOperator(operator) {
            switch (operator) {
                case 'eq':
                    operator = '=';
                    break;
                case 'neq':
                    operator = '!=';
                    break;
                case 'gt':
                    operator = '>';
                    break;
                case 'gte':
                    operator = '>=';
                    break;
                case 'lt':
                    operator = '<';
                    break;
                case 'lte':
                    operator = '<=';
                    break;
                case 'startswith':
                case 'endswith':
                case 'contains':
                    operator = 'LIKE';
                    break;
                default:
            }
            return operator;
        }

        //Build single filter expression
        function buildExpression(gridName, expr) {
            const operators = ['=', 'eq', '!=', 'neq', '>', 'gt', '>=', 'gte', '<', 'lt', '<=', 'lte', 'startswith', 'endswith', 'contains'];
            const fields = $('#' + gridName).data('kendoGrid').columns.map(c => c.field);
            const field = expr.field.toUpperCase();
            let operator = expr.operator.toLowerCase();

            let isValidField = fields.includes(field);
            let isValidOperator = operators.includes(operator);

            let expression = '';

            if (isValidField && isValidOperator) {
                const dataType = expr.dataType ? expr.dataType.toLowerCase() : 'char';
                let value = expr.value;
                let isText = (dataType === 'char' || dataType === 'string' || dataType === 'text');
                let oriOperator = operator;
                operator = convertOperator(operator);

                if (isText) {
                    value = value.replace(/\"/g, '\\"');
                    switch (oriOperator) {
                        case 'startswith':
                            value = `${value}%`;
                            break;
                        case 'endswith':
                            value = `$%{value}`;
                            break;
                        case 'contains':
                            value = `$%{value}%`;
                        default:
                    }
                }
                if (dataType === 'date' || dataType === 'datetime') {
                    value = sg.utls.kndoUI.checkForValidDate(value) ? kendo.toString(new Date(value), 'yyyyMMdd'): 0;
                }
                expression = isText ? `${field} ${operator} "${value}"` : `${field} ${operator} ${value}`;
            }

            return expression;
        }

        function buildFilter(gridName, filter) {
            let filterString = '';
            let logic = filter.logic ? filter.logic.toUpperCase() : 'AND';

            if (['AND', 'OR'].includes(logic)) { //Check valid logic operator
                filter.filters.forEach(f => {
                    let fs = '';
                    if (f.field && f.operator) {
                        fs = buildExpression(gridName, f);
                        if (fs) {
                            filterString += filterString ? ` ${logic} ${fs}` : fs;
                        }
                    }
                    if (f.filters) {
                        fs = buildFilter(gridName, f);
                        filterString += filterString ? ` ${logic} (${fs})` : `(${fs})`;
                    }
                });
            }

            return filterString;
        }

        return buildFilter(gridName, filter);
    }

    //Module(class) public methods
    return {
        init: init,
        initGridNoDefaultSelect: initGridNoDefaultSelect,
        //Grid internal methods(For grid toolbar, column template use)
        addLine: toolbarAddLine,
        clearSelection: clearSelection,
        deleteLine: toolbarDeleteLine,
        editColumnSettings: toolbarEditColumn,
        showDetails: toolbarShowDetails,
        getListText: getListText,
        getTemplate: getTemplate,
        moveToCurrentRow: moveToCurrentRow,
        //Documented public methods
        showColumn: showColumn,
        allowInsert: allowInsert,
        allowDelete: allowDelete,
        columnTemplate: columnTemplate,
        currentRecord: currentRecord,
        filter: filter,
        readOnly: readOnly,
        fieldName: fieldName,
        clearNewRow: clearNewRow,
        columnCount: columnCount,
        columnTitle: columnTitle,
        columnEditable: columnEditable,
        isValid: isValid,
        dirty: dirty,
        commit: commit,
        refresh: refresh,
        refreshAndMoveToCurrent: refreshAndMoveToCurrent,
        refreshCurrentRow: refreshCurrentRow,
        resetCurrentRow: resetCurrentRow,
        updateCurrentRow: updateCurrentRow,
        isEmpty: isEmpty,
        cancel: cancel,
        clear: clear,
        refreshByKey: refreshByKey,
        showWarnings: showWarnings,
        showErrors: showErrors,
        currentPage: currentPage,
        getCurrentLineNumber: getCurrentLineNumber,
        moveToRow: moveToRow,
        moveToRecord: moveToRecord,
        isFieldDisabled: isFieldDisabled,
        getParentData: getParentData,
        buildFilterString: buildFilterString,
        setNextEditCell: setNextEditCellByCol
    };
}();
