/* Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved. */
"use strict";

var sg = sg || {};
sg.viewList = function () {

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
        Update: 3,
        Refresh: 4,
        Delete: 5,
        MoveTo: 6,
        RefreshRow :7
    },
    BtnTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}" id="btn{3}{4}">{2}</button>';

    var _setDefaultRow = {},
        _lastRowNumber = {},
        _lastColField = {},
        _lastRowStatus = {},
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
        _gridList = [];

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
    function _addLine(gridName, rowData) {
        var grid = _getGrid(gridName),
            dataSource = grid.dataSource,
            insertedIndex = grid.select().index() + 1,
            pageSize = dataSource.pageSize(),
            total = dataSource.total(),
            currentPage = dataSource.page();
            rowData = rowData || grid.dataItem(grid.select());

        // If the current record reaches maximum of current page but not the last record, go to next page
        if (insertedIndex === pageSize && total / pageSize !== currentPage) {
            insertedIndex = 0;
            dataSource.page(++currentPage);
            // Paging send extra moveto request, skip to send the request
            _skipMoveTo[gridName] = true;
        }
        if (insertedIndex > pageSize) {
            insertedIndex = 0;
        }
        _setDefaultRow[gridName] = false;
        //If the record is dirty, update the current row first before adding a new row
        if (rowData && rowData.dirty) {
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
        var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
        grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
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
            if (col.field && col.field.hidden !== true) {
                var colField = model.fields[col.field];
                if (colField && colField.editable) {
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
     * @return {boolean} A boolean flag to indicate the current grid valid status
     */
    function _commitGrid(gridName, callBack) {
        var grid = _getGrid(gridName),
            rowData = grid.dataItem(grid.select());

        if (rowData) {
            if (!_newLine[gridName] || !rowData.isNewLine) {
                _sendRequest(gridName, RequestTypeEnum.Update, undefined, undefined, undefined, undefined, callBack);
                return true;
            }
            _sendRequest(gridName, RequestTypeEnum.Insert, undefined, undefined, undefined, undefined, callBack);
        }

        return _valid[gridName];
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
            cols = [{field: "isNewLine", hidden : true}],
            numbers = ["int32", "int64", "int16", "int", "integer", "long", "byte", "real", "decimal"];

        for (var i = 0, length = columns.length; i < length; i++) {
            var col = {},
                column = columns[i],
                dataType = column.DataType ? column.DataType.toLowerCase() : "string",
                list = column.PresentationList,
                attr = numbers.indexOf(dataType) > -1 && list === null ? "align-right " : "align-left ";

            //custom call back column definitions
            _columnSettingCallback(gridName, "columnBeforeDisplay", column);

            col.title = column.ColumnName;
            col.isPrimaryKeyField = column.IsPrimaryKeyField;
            col.field = column.FieldName;
            col.dataType = dataType;
            col.width = column.Width || 180;
            col.fieldSize = column.FieldSize;
            col.headerWidth = col.width;
            col.attributes = { "class": attr };
            col.headerAttributes = { "class": attr };
            col.precision = column.Precision;
            col.hidden = column.IsHidden || false;
            col.presentationList = list;
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
            mask = col.PresentationMask,
            field = options.field,
            model = options.model,
            buttonId = "btnFinderGridCol" + field.toLowerCase(),
            maskProps = _getTextBoxProps(mask),
            className = maskProps.class,
            maxlength = col.FieldSize || maskProps.maxLength,
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input name="{0}" id="{0}" maxlength="{1}" class="{2}"/></div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{3}"/></div></div>',
            html = kendo.format(txtInput + txtFinder, field, maxlength, className, buttonId);
       
        $(html).appendTo(container);
        finder.viewID = finder.ViewID;
        finder.viewOrder = finder.ViewOrder;
        finder.displayFieldNames = finder.DisplayFieldNames;
        finder.returnFieldNames = finder.ReturnFieldNames;
        finder.initKeyValues = [];

        var refKey = col.ReferenceField;
        if (refKey) {
            finder.filter = kendo.format("{0}={1}", refKey, model[refKey]);
        }

        //Finder has filter, get the filter, set finder calculatePageCount as false
        if (finder.Filter) {
            finder.filter = getFilter();
            finder.calculatePageCount = false;
        }

        //Custom define filter
        if (finder.CustomFinder) {
            finder = _getFunction(finder.CustomFinder)(finder);
        } 

        //Custom plug in for 'columnStartEdit' and 'columnEndEdit'
        _columnCallBackEdit(gridName, field);

        /**
         * @description Parse the filter expression, dynamically set the filter value, such as filter expression: "ITEMNO=UNFMTITMNO", the "UNFMTITMNO" should the item value like 'A11030'
         * @return {object} return the parsed filters 
         */
        function getFilter() {
            var filters = finder.Filter.toUpperCase();
            var exprs = filters.split(' AND ').join(',').split(' OR ').join(',').split(',');
            exprs.forEach(function (expr) {
                var field = expr.split('=').pop().trim();
                if (model.hasOwnProperty(field)) {
                    filters = filters.replace(field, model[field]);
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
            var returnValue = value[Object.keys(value)[0]];
            _sendChange[gridName] = true;
            options.model.set(options.field, returnValue);
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
            _sendChange[gridName] = true;
        });

        $("#" + buttonId).mousedown(function (e) {
            //Set finder initial values
            _sendChange[gridName] = false;
            var value = $("#" + field).val().toUpperCase();
            var length = finder.InitKeyFieldNames ? finder.InitKeyFieldNames.length : 0;
            finder.initKeyValues = [];
            if (length > 0) {
                for (var i = 0; i < length; i++) {
                    var initField = finder.InitKeyFieldNames[i];
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
        var mask = col.PresentationMask,
            field = options.field,
            maskProps = mask ? _getTextBoxProps(mask) : "",
            className = maskProps ? maskProps.class : "",
            maxlength = col.FieldSize || maskProps ? maskProps.maxLength : 64,
            html = kendo.format('<input type="text" id="{0}" name="{0}" class="{1}" maxlength="{2}" />', field, className, maxlength);

        $(html).addClass('k-input k-textbox')
            .appendTo(container)
            .change(function () {
                options.model.set(field, this.value);
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
        if (container.context.cellIndex === 0 && sg.utls.isShiftKeyPressed) {
            var prevRowIndex = sg.utls.kndoUI.getSelectedRowIndex(grid) - 1;
            if (prevRowIndex >= 0) {
                grid.select(grid.tbody.find(">tr:eq(" + prevRowIndex + ")"));
            }
        } 
        sg.utls.kndoUI.skipTab(grid, container.context.cellIndex);
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
                   '<div class="edit-cell inpt-finder"><input class="icon btn-search" id="btnDetailOptionalField" type="button"></div></div></div>';

        $(html).appendTo(container);
        options.model.OptionalField = options.model.VALUES > 0 ? globalResource.Yes : globalResource.No;
        var callback = _getFunction(col.CustomFunctions[0].OptionalField);
        //$("#btnDetailOptionalField").on("click", callback());
        $("#btnDetailOptionalField").on("click", function () {
            if (typeof callback === "function") {
                callback();
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
            dataType = Array.isArray(columns) ? field.type.toLowerCase(): columns.DataType,
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

        if (col.HasFinder) {
            return col.Finder ? _finderEditor(container, options, col, gridName) : null;
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
            case RequestTypeEnum.RefreshRow:
                _refreshRow(gridName, jsonResult, fieldName);
                break;
            default:
        }

        if(callBack && typeof callBack === "function"){
            callBack(isSuccess);
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
        // If the current record reaches maximum of current page, make it a dummy record and go to next page
        if (index === pageSize) {
            data.skipCommit = true;
            dataSource.insert(index, data);
            dataSource.query({ pageSize: pageSize, page: ++currentPage });
            _skipMoveTo[gridName] = true;
        } else {
            data.isNewLine = true;
            dataSource.insert(index, data);
            grid.refresh();
        }
        var row = _selectGridRow(grid, keyValue);
        _setModuleVariables(gridName, RowStatusEnum.INSERT, "", index, true, false, true);
        _setNextEditCell(grid, grid.dataSource.options.schema.model, row, 0);
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
        sg.utls.showMessage(jsonResult);
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
        } else {
            grid.dataSource.data().forEach(function (row) { row.isNewLine = false;});
        }

        // could be just warnings, but still need to show to users
        if(jsonResult && jsonResult.UserMessage && 
            (jsonResult.UserMessage.Warnings && jsonResult.UserMessage.Warnings ||
             jsonResult.UserMessage.Errors && jsonResult.UserMessage.Errors)){
            sg.utls.showMessage(jsonResult);
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
        sg.utls.showMessage(jsonResult);
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
            dataItem = grid.dataItem(selectRow);

        _gridCallback(gridName, "gridChanged", jsonResult.Data, fieldName);
        _setModuleVariables(gridName, status, "", rowIndex, true, false);

        if (dataItem) {
            for (var field in jsonResult.Data) {
                if (dataItem.hasOwnProperty(field)) {
                    dataItem[field] = jsonResult.Data[field];
                }
            }
        }

        var lastCellIndex = grid._lastCellIndex;
        var selectRowChanged = dataItem.uid !== _selectRowUid[gridName];
        grid.refresh();

        // After grid refresh, use kendo grid row unique id and column index to select grid row and column
        var uid = _selectRowUid[gridName] || dataItem.uid;
        var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
        var colIndex = selectRowChanged ? lastCellIndex : index + 1; 
        var row = grid.table.find("[data-uid=" + uid + "]");

        if (row.length === 1) {
            grid.select(row);
            _setNextEditCell(grid, dataItem, row, colIndex);
        }
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

        // could be just warnings, but still need to show to users
        if(jsonResult && jsonResult.UserMessage && 
            (jsonResult.UserMessage.Warnings && jsonResult.UserMessage.Warnings ||
             jsonResult.UserMessage.Errors && jsonResult.UserMessage.Errors)){
            sg.utls.showMessage(jsonResult);
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
            rowIndex = selectRow.index(),
            newLine = _newLine[gridName],
            dataItem = grid.dataItem(selectRow);

        //Return previous valid value when update error
        dataItem[fieldName] = _lastErrorResult[gridName][fieldName + "Value"];
        _setModuleVariables(gridName, RowStatusEnum.UPDATE, "", rowIndex, false, false, newLine);
        sg.utls.showMessage(jsonResult);
    
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
        _skipMoveTo[gridName] = true;
    }

    /**
    * @description Send delete request success, refresh the grid, reset error object
    * @param {string} gridName The grid name
    * @param {object} jsonResult The request call back result
    * @param {string} fieldName update field name
    */
    function _deleteSuccess(gridName, jsonResult) {
        _lastErrorResult[gridName] = {};
        _dataChanged[gridName] = true;

        var grid = $('#' + gridName).data("kendoGrid"),
            ds = grid.dataSource,
            page = ds.page(),
            length = ds.data().length;

        page > 1 ? ds.page(length > 1 ? page : page - 1) : ds.page(1);
    }
    /**
     * @description Send delete request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     */
    function _deleteError(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.read();
        sg.utls.showMessage(jsonResult);
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
            uid = refreshRowData.uid,
            status = _lastRowStatus[gridName] === RowStatusEnum.UPDATE ? RowStatusEnum.NONE : _lastRowStatus[gridName];

        if (refreshRowData) {
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
        var grid = _getGrid(gridName),
            record = rowData || grid.dataItem(grid.select());

        if (grid) {
            var data = { 'viewID': $("#" + gridName).attr('viewID'), 'record': record, 'fieldName': fieldName },
                requestName = _getRequestName(requestType),
                url = sg.utls.url.buildUrl("Core", "Grid", requestName);

            if (requestType === RequestTypeEnum.Refresh) {
                data.isNewRecord = isNewLine || false;
            }
                
            sg.utls.ajaxPostSync(url, data, function (jsonResult) {
                var isSuccess = true;
                if (jsonResult && jsonResult.UserMessage.Errors && jsonResult.UserMessage.Errors.length > 0 ) {
                    isSuccess = false;
                }
                _requestComplete(requestType, isSuccess, gridName, jsonResult, fieldName, isNewLine, insertedIndex, rowData ? rowData.uid : "", callBack);
            });
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
                            value = record[field];

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
                                callback(record, finder);
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
     * @return {boolean} The boolean flag to indicate whether continue or stop
     */
    function _gridCallback(gridName, functionName, record, fieldName) {
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
                            case "gridChanged":
                            case "gridUpdated":
                               callback(record, fieldName);
                               return true;
                            case "gridAfterSetActiveRecord":
                            case "gridAfterDelete":
                            case "gridAfterCreate":
                            case "gridAfterInsert":
                               callback(record);
                               return true;
                            case "gridBeforeCreate":
                            case "gridBeforeDelete":
                               callback(record, event);
                               return event.isProceed();
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
     * @description Initialize a grid, creating dataSource to binding grid, register events handler
     * @param {string} gridName The name of the grid.
     * @param {boolean} readOnly Whether the grid allows editing(Optional).
     * @param {object} updateColumnDefs Function or funcion name. To update the column definitions before build grid columns
     * @return {object} Return kendo grid object
     */
    function init(gridName, readOnly, updateColumnDefs) {

        if (updateColumnDefs) {
            typeof updateColumnDefs === "function" ? updateColumnDefs() : _getFunction(updateColumnDefs)();
        }

        window.addEventListener("message", _receiveMessage, false);
        var model = window[gridName + "Model"],
            columns = _getGridColumns(gridName),
            addTemplate = kendo.format(BtnTemplate, 'btn-add', 'sg.viewList.addLine(&quot;' + gridName + '&quot;)', globalResource.AddLine, gridName, "Add"),
            delTemplate = kendo.format(BtnTemplate, 'btn-delete', 'sg.viewList.deleteLine(&quot;' + gridName + '&quot;)', globalResource.DeleteLine, gridName, "Delete"),
            editTemplate = kendo.format(BtnTemplate, 'btn-edit-column', 'sg.viewList.editColumnSettings(&quot;' + gridName + '&quot;)', globalResource.EditColumns, gridName, "EditCol");

        readOnly = readOnly || model.ReadOnly,
        _initModuleVariables(gridName);

        if (_gridList.indexOf(gridName) < 0) { _gridList.push(gridName); }

        $("#" + gridName).kendoGrid({
            height: model.Height || 430,
            columns: columns,
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
                        sg.utls.showMessage(error);
                    }.bind(null, errorMessage));

                    if(!errorMessage.UserMessage.IsSuccess){
                        return;
                    }
                }
                //Skip insert or update if the record is out of range of the page size
                if (lastRowNumber > -1 && lastRowNumber < pageSize) {
                    if (selectedIndex !== lastRowNumber) {
                        var lastRowData = grid.dataSource.data()[lastRowNumber];
                        if (lastRowData.isNewLine) {
                            _sendRequest(gridName, RequestTypeEnum.Insert, "", false, -1, lastRowData);
                        } else if (lastRowData.dirty) {
                            _sendRequest(gridName, RequestTypeEnum.Update, "", false, -1, lastRowData);
                        }
                    }
                }
                if (record) {
                    if (record.uid !== _selectRowUid[gridName]) {
                        // Paging and refresh/update error cause send extra moveto request, skip to send this request
                        if (_skipMoveTo[gridName]) {
                            _skipMoveTo[gridName] = false;
                        } else {
                            var rowData = grid.dataItem(grid.select());
                            _sendRequest(gridName, RequestTypeEnum.MoveTo, "", rowData.isNewLine, -1, rowData);
                        }
                    }
                    _selectRowUid[gridName] = record.uid;
                }
            },

            dataBound: function (e) {
                var grid = e.sender,
                    rows = this.items(),
                    page = grid.dataSource.page(),
                    pageSize = grid.dataSource.pageSize(),
                    ps = pageSize - 1;

                //Generate display index for sequence number column
                $(rows).each(function () {
                    var index = $(this).index() + 1 + pageSize * (page - 1);
                    var rowLabel = $(this).find(".displayIndex");
                    $(rowLabel).html(index);
                });

                //only show page size record
                grid.tbody.find("tr:gt(" + ps + ")").hide();

                //Set default select row
                if (_setDefaultRow[gridName]) {
                    grid.select("tr:eq(0)");
                }
                //Custom plug in for 'columnDoubleClick' 
                grid.tbody.find("td").dblclick(function (grid, e) {
                    var col = grid.columns[e.target.cellIndex];
                    if (col) {
                        _columnCallback(gridName, "columnDoubleClick", col.field);
                    }
                }.bind(null, grid));
            },

            //Custom plug in for 'columnBeforeEdit'
            beforeEdit: function (e) {
                var field = _getFieldNameByCellIndex(e.sender, e.sender._lastCellIndex);
                var isProceed = _columnCallback(gridName, "columnBeforeEdit", field);
                if (!isProceed) {
                    e.preventDefault();
                }
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
                pageSize: model.PageSize || 10,
                numeric: false,
                buttonCount: 1,
                input: true
            },

            toolbar: readOnly ? [{ template: editTemplate }] : [{ template: addTemplate }, { template: delTemplate }, { template: editTemplate }],

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
                    var grid = _getGrid(gridName),
                        count = grid.dataSource.total();

                    var disabled = count === 0 || !_allowDelete[gridName];
                    $("#btn" + gridName + "Delete").prop("disabled", disabled);

                    if (e.action && e.action !== "sync") {
                        _dataChanged[gridName] = true;
                    }

                    if (e.items.length === 0 && count === 0 && grid.dataSource.page() !== 1) {
                        grid.dataSource.page(1);
                    }

                    if (e.action === "itemchange") {
                        e.preventDefault();
                        if (e.field === "OptionalField" || e.field === "OptionalFieldString") {
                            return;
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
                    //Here we will add the actual line on the new page
                    if (e.action === undefined && e.items.length === 0 && count !== 0) {
                        _addLine(gridName);
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
                            _setModuleVariables(gridName, RowStatusEnum.NONE, "", -1, true, true, false);
                            data.fieldNames = eval(gridName + "_viewFieldNames");
                            data.filter = _filter[gridName];
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
                    var row = e.sender.data()[e.sender.pageSize()];
                    var skipCommit = row && row.hasOwnProperty("skipCommit") && row["skipCommit"];

                    if (_newLine[gridName] && e.type === "read" && _currentPage[gridName] !== this.page() && !skipCommit) {
                        e.preventDefault();
                        if (_commitGrid(gridName)) {
                            _newLine[gridName] = false;
                            _currentPage[gridName] = this.page();
                            this.page(this.page());
                        } 
                        return;
                    }
                    _currentPage[gridName] = this.page();
                }

            }
        });

        //binding the drilldown popup window
        $("#" + gridName).delegate("tbody > tr > td > a", "click", _initShowPopup);

        $("#" + gridName).delegate("tbody > tr > td > img", "click", _initShowPopup);

        //When close the pop up error message, focus the last edit cell
        $(document).on("click", ".msgCtrl-close", function (e) {
            _setGridCell.call(this, gridName);
        });

        //Set grid use preferences, such as show/hide column, order and width
        GridPreferencesHelper.setGrid("#" + gridName, model.GridColumnSettings);

        return _getGrid(gridName);
    }

    /**
     * @description Delete grid line, for grid toolbar template, grid internal use
     * @param {string} gridName The name of the grid.
     */
    function toolbarDeleteLine(gridName) {
        var grid = _getGrid(gridName),
            select = grid.select(),
            selectedIndex = select.index(),
            rowData = grid.dataItem(select);

        var isProceed = _gridCallback(gridName, "gridBeforeDelete", rowData);
        if (!isProceed) {
            return;
        }

        if (selectedIndex > -1) {
            sg.utls.showKendoConfirmationDialog(
                function () {
                    if (!rowData.isNewLine) {
                        _sendRequest(gridName, RequestTypeEnum.Delete, "");
                    } else {
                        var ds = grid.dataSource;
                        ds.remove(rowData);
                        _newLine[gridName] = false;
                        // Set the correct current page
                        var page = ds.data().length === 0 && _currentPage[gridName] > 1 ? _currentPage[gridName] - 1 : _currentPage[gridName];
                        ds.page(page);
                    }
                    _gridCallback(gridName, "gridAfterDelete", rowData);
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
            rowData = grid.dataItem(grid.select());
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
        rowData && rowData.isNewLine ? _sendRequest(gridName, RequestTypeEnum.Insert, "", true) : _addLine(gridName);

        //custom plugin after create new line
        _gridCallback(gridName, "gridAfterCreate");
    }

    /**
     * @description Edit grid settings, for grid toolbar template, grid internal use.
     * @param {any} gridName The grid name
     */
    function toolbarEditColumn(gridName) {
        var grid = _getGrid(gridName);
        var btnEditElement = $("#" + gridName + " .k-grid-toolbar .btn-edit-column");
        GridPreferencesHelper.initialize('#' + gridName, window[gridName + "Model"].UserPreferencesUniqueId, $(btnEditElement), grid.columns);
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
            return value;
        }
    }

    /**
     * @description Get dropdown list text from value, for grid column dropdown list template(internal use)
     * @param {string} field The column field name
     * @param {object} dataItem The text/value pair list
     * @return {string} return the text by value
     */
    function getListText(field, dataItem) {
        var list = this.filter(function (i) { return i.Value.toLowerCase() === (dataItem[field] ? dataItem[field].toString().toLowerCase() : ""); });
        return list && list.length > 0 ? list[0].Text : (dataItem[field] ? dataItem[field] : "");
    }

    /**
 *  @description Sync the current grid select row with server, move the server entity pointer to current entity
 *  @description Used for parent/details grid(popup)
 *  @param {any} gridName The name of the grid
 */
    function moveToCurrentRow(gridName) {
        var grid = _getGrid(gridName),
            rowData = grid.dataItem(grid.select()),
            data = { 'viewID': $("#" + gridName).attr('viewID'), "record": rowData },
            url = sg.utls.url.buildUrl("Core", "Grid", "MoveTo");

        sg.utls.ajaxPostSync(url, data, function () { });
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
     * @return {booean} A boolean flag or none
     */
    function showColumn(gridName, columnName, visible) {
        var grid = _getGrid(gridName) || init(gridName);
        if (grid) {
            if (visible !== undefined) {
                visible ? grid.showColumn(columnName) : grid.hideColumn(columnName);
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
     * Update current select row
     * @param {any} gridName The grid name
     */
    function updateCurrentRow(gridName) {
        _sendRequest(gridName, RequestTypeEnum.Update);
    }

    //Module(class) public methods
    return {
        init: init,
        //Grid internal methods(For grid toolbar, column template use)
        addLine: toolbarAddLine,
        deleteLine: toolbarDeleteLine,
        editColumnSettings: toolbarEditColumn,
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
        columnCount: columnCount,
        columnTitle: columnTitle,
        columnEditable: columnEditable,
        isValid: isValid,
        dirty: dirty,
        commit: commit,
        refresh: refresh,
        refreshCurrentRow: refreshCurrentRow,
        updateCurrentRow: updateCurrentRow,
        isEmpty: isEmpty,
        cancel: cancel
    };
}();
