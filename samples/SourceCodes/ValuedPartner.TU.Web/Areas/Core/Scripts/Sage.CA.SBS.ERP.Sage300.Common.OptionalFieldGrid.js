// Copyright (c) 2019-2023 Sage Software, Inc.  All rights reserved.

"use strict";
var sg = sg || {};

sg.optionalFieldControl = function () {

    var GridUtls = {
        addingLine: false,
        updateFailed: false
    };

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
            Delete: 4,
            Refresh: 5,
            Save: 6,
            Put: 7
        },
        YesNoList = [{ "Selected": false, "Text": globalResource.Yes, "Value": "1" }, { "Selected": false, "Text": globalResource.No, "Value": "0" }],
        ButtonTemplate = '<button class="btn btn-default btn-grid-control {0}" type="button" onclick="{1}" id="btn{3}{4}">{2}</button>';

    var _setDefaultRow = {},
        _lastRowNumber = {},
        _lastColField = {},
        _lastRowStatus = {},
        _lastErrorResult = {},
        _currentPage = {},
        _dataChanged = {},
        _filter = {},
        _sendChange = {},
        _lastLine = -1,
        _gridName = "",
        _isFinderButton = false,
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
            total = dataSource.total(),
            currentPage = dataSource.page();

        if (insertedIndex === pageSize) {
            if (total / pageSize === currentPage) {
                // Insert new row to have 1 more page, then to next page to remove this new insert row
                dataSource.insert(insertedIndex, dataSource.data()[0]);
                dataSource.query({ pageSize: pageSize, page: currentPage + 1 });
            } else {
                insertedIndex = 0;
                dataSource.page(++currentPage);
            }
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
        grid.select("tr:eq(" + rowIndex + ")");
        grid.editCell(grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex));
        setTimeout(function () {
            GridUtls.updateFailed = false;
        });
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
    *  @description Blank out grid cell (usually description fields on invalid OPTFIELD and VALUE inputs)
    *  @param {string} gridName The name of the grid.
    *  @param {Number} rowIndex The slected row index.
    *  @param {object} dataItem The text/value pair list
    *  @param {string} fieldName The name of the grid column to blank out.
    *  @param {any} value The value to blank out cell with
    *  @return {void}  
    */
    function _blankGridCell(gridName, rowIndex, dataItem, fieldName, value) {
        if (dataItem[fieldName] !== value) {
            dataItem[fieldName] = value;
            // 'refresh' grid-cell without trip to server for value validation
            var grid = $('#' + gridName).data("kendoGrid");
            var colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
            var cell = grid.tbody.find(">tr:eq(" + rowIndex + ") >td:eq(" + colIndex + ")");
            if (cell !== null && cell !== undefined && 0 < cell.length) {
                cell[0].innerText = value;
            }
        }
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
            //col.width = 180;
            //col.headerWidth = 180;
            if (i < length - 1) {
                col.width = 180;
                col.headerWidth = 180;
            }

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
     * @description Save the the current line to send ajax request. For optional field, it's already send insert request to view, just reture true.
     * @param {string} gridName The current grid name
     * @param {function} callBack The callBack function after the action is completed
     * @return {boolean} A boolean flag to indicate the current grid valid status
    */
    function _commitGrid(gridName, callBack) {

        var grid = _getGrid(gridName),
            selectedRowData = grid.dataItem(grid.select());

        if (selectedRowData && selectedRowData.dirty) {
            _sendRequest(gridName, RequestTypeEnum.Save, "", callBack && typeof callBack === "function" ? callBack : null);
        }
        else {
            if (callBack && typeof callBack === "function") {
                callBack(true);
            }
        }
		
        // always return TRUE for backward compatibility. A caller should use a callback function 
        // to check whether commit is successful
        return true;

    }

    /**
    * @description Set editor initial value
    * @param {string} gridName The name of the grid
    * @param {any} options Editor options object
    */
    function _setEditorInitialValue(gridName, options) {
        var field = options.field,
            swset = options.model["SWSET"],
            type = options.model["TYPE"];
        if (field === "VALUE" && swset === 0) {
            options.model[field] = "";
            if ([6, 8, 100].indexOf(type) > -1) {
                options.model[field] = 0;
            }
        }
        _lastColField[gridName] = field;
        _lastErrorResult[gridName][field + "Value"] = options.model[field];
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
            maxLength = isValue ? model.LENGTH : 12,
            txtClass = isValue ? "" : "txt-upper grid_inpt pr25",
            finder = {},
            viewId = _settings[gridName].viewId,
            filter = _settings[gridName].filter,
            buttonId = "btnFinderGridCol" + field.toLowerCase(),
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text"><input name="{0}" id="txtGridCol{0}" type="text" maxlength="{1}" class="{2}"/></div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="{3}"/></div></div>',
            isFinderButton = false,
            html = kendo.format(txtInput + txtFinder, field, maxLength, txtClass, buttonId);

        $(html).appendTo(container);

        finder.viewID = isValue ? "CS0012" : viewId;
        finder.viewOrder = 0;
        finder.filter = isValue ? "OPTFIELD=" + model.OPTFIELD : filter;
        finder.displayFieldNames = isValue ? ["VALIFTEXT", "VDESC"] : ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"];
        finder.returnFieldNames = isValue ? ["VALUE", "VDESC"] : ["OPTFIELD"];

        $("#txtGridColOPTFIELD, #txtGridColVALUE").on("change", function (e) {
            _sendChange[gridName] = !isFinderButton;
        });

        $("#" + buttonId).mousedown(function (e) {
            isFinderButton = true;
            _isFinderButton = true;
        });

        $("#txtGridColOPTFIELD, #txtGridColVALUE").on("keydown", function (e) {
            _sendChange[gridName] = false;
            if (e.keyCode === 9 || e.keyCode === 13) {
                var value = field === "VALUE" ? this.value : this.value.toUpperCase();
                _sendChange[gridName] = true;
                model.set(field, value);
            } else if (e.altKey && e.keyCode === sg.constants.KeyCodeEnum.DownArrow) {
                isFinderButton = true;
                _isFinderButton = true;
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
                seq = model.SEQUENCENO ? pad(10, model.SEQUENCENO, ' ') : "",
                line = model.LINENO ? pad(10, model.LINENO, ' ') : "",
                optField = pad(12, value, ' '),
                errorMsg = kendo.format(globalResource.DuplicateOptionalField, value.toUpperCase());

            _isFinderButton = false;
            _sendChange[gridName] = true;
            if (field === "OPTFIELD") {
                var keyValue = seq + line + optField;
                model.set(field, value);
                model["KendoGridAccpacViewPrimaryKey"] = keyValue;
                //model.set("id", keyValue);
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
            _isFinderButton = false;
            if (options.field === "OPTFIELD") {
                options.model[options.field] = "";
            }

            if (grid) {
                var rowIndex = grid.select().index();
                _setEditCell(grid, rowIndex, options.field);
            }
        }

        $("#" + buttonId).mousedown(function (e) {
            var value = $("#txtGridCol" + field).val().toUpperCase();
            _isFinderButton = true;
            var initLocation = filter.split('=').pop();
            finder.initKeyValues = isValue ? [model.OPTFIELD, value || model.VALUE] : [initLocation, model.OPTFIELD || value];
            sg.viewFinderHelper.setViewFinder(buttonId, onFinderSelected.bind(null, options), finder, onFinderCancel.bind(null, options));
        });
        sg.utls.findersList["txtGridCol" + field] = buttonId;

        _setEditorInitialValue(gridName, options);
    }

    /**
     * @description The optional field value column finder editor
     * @param {object} options The column options
     * @param {string} gridName The grid name
     * @param {string} btnFinderId The finder button ID
     */
    function _setValueFinderEditor(options, gridName, btnFinderId) {
        var model = options.model,
            swset = options.model["SWSET"],
            finder = {};

        finder.viewID = "CS0012";
        finder.viewOrder = 0;
        finder.filter = "OPTFIELD=" + model.OPTFIELD;
        var value = swset === 0 && [6, 8, 100].indexOf(model.TYPE) > -1 ? 0 : model.VALUE;

        switch (model.TYPE) {
            case ValueTypeEnum.Date:
                finder.displayFieldNames = ["VALIFDATE", "VDESC"];
                //convert the value to specified format for finder query
                if (model.VALUE) {
                    value = kendo.toString(model.VALUE, 'yyyyMMdd');
                }
                break;
            case ValueTypeEnum.Integer:
                finder.displayFieldNames = ["VALIFLONG", "VDESC"];
                break;
            case ValueTypeEnum.Amount:
                finder.displayFieldNames = ["VALIFMONEY", "VDESC"];
                break;
            case ValueTypeEnum.Time:
                finder.displayFieldNames = ["VALIFTIME", "VDESC"];
                break;
            case ValueTypeEnum.Number:
                finder.displayFieldNames = ["VALIFNUM", "VDESC"];
                break;
            default:
                finder.displayFieldNames = ["VALUE", "VDESC", "TYPE"];
        }
        finder.returnFieldNames = ["VALUE", "VDESC"];
        finder.initKeyValues = [model.OPTFIELD, value];

        /**
         * @description On select finder row, set the select value
         * @param {any} options The column options
         * @param {any} retureFields The finder selected row value
         */
        function onFinderSelected(options, retureFields) {
            var model = options.model,
                field = options.field,
                value = retureFields[field];
            let type = model.TYPE;

            //the same logic once init edit template
            if (type === ValueTypeEnum.Date) {
                value = value === "00000000" ? "" : sg.utls.kndoUI.getFormattedDate(value);
            } else if (type === ValueTypeEnum.Time && value.indexOf(':') < 0) {
                value = value.substring(0, 2) + ":" + value.substring(2, 4) + ":" + value.substring(4, 6);
            }

            _isFinderButton = false;
            model.set(field, value);
        }

        /**
         * @description On cancel the finder, focus the last edit cell
         * @param {any} options The column options
         */
        function onFinderCancel(options) {
            var grid = _getGrid(gridName);
            _isFinderButton = false;
            if (grid) {
                var rowIndex = grid.select().index();
                _setEditCell(grid, rowIndex, options.field);
            }
        }
        sg.viewFinderHelper.setViewFinder(btnFinderId, onFinderSelected.bind(null, options), finder, onFinderCancel.bind(null, options));
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
            input = kendo.format('<input data-text-field="{0}" data-value-field="{0}" data-bind="value:{0}" id="{1}" />', field, txtId),
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text">' + input + '</div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="btnFinderId"/></div></div>',
            html = txtInput + txtFinder;

        if (model.hasOwnProperty('TYPE') && model.TYPE === ValueTypeEnum.Date && !isNaN(value) && value !== null) {
            model.VALUE = value.toString().length === 8 ? kendo.parseDate(value.toString(), 'yyyyMMdd') : value;
        }
        $(html).appendTo(container);
        sg.utls.kndoUI.datePicker(txtId);
        _setValueFinderEditor(options, gridName, "btnFinderId");
        _setEditorInitialValue(gridName, options);
        $("#btnFinderId").mousedown(function () {
            _isFinderButton = true;
        });
    }
    /**
     * @description Column time editor, set the mask for time value input
     * @param {any} container The column kendo editor container
     * @param {any} options The column options
     * @param {any} gridName The grid name
     */
    function _timeEditor(container, options, gridName) {
        var field = options.field,
            input = kendo.format('<input id="{0}" name="{0}" />', field),
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text">' + input + '</div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="btnFinderId"/></div></div>',
            html = txtInput + txtFinder;

        $(html).appendTo(container);

        $("#" + field).kendoMaskedTextBox({
            mask: "14:34:34",
            unmaskOnPost: true,
            rules: {
                "1": /[0-2]/,
                "2": /[0-3]/,
                "3": /[0-5]/,
                "4": /[0-9]/
            }
        });

        $("#btnFinderId").unbind();
        _setValueFinderEditor(options, gridName, "btnFinderId");
        _setEditorInitialValue(gridName, options);
        $("#btnFinderId").mousedown(function () {
            _isFinderButton = true;
        });
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
                },
                close: function (e) {
                    //save the value
                    _sendRequest("" + _gridName, RequestTypeEnum.Save, "");
                }
            });
        _setEditorInitialValue(gridName, options);
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
        _setEditorInitialValue(gridName, options);
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
            precision = options.model.DECIMALS,
            input = kendo.format('<input name="{0}" id="txt{0}" />', field),
            txtInput = '<div class="edit-container"><div class="edit-cell inpt-text">' + input + '</div>',
            txtFinder = '<div class="edit-cell inpt-finder"><input type="button" class="icon btn-search" id="btnFinderId"/></div></div>',
            html = txtInput + txtFinder;

        $(html).appendTo(container);
        var txtNumeric = $("#txtVALUE").kendoNumericTextBox({
            format: "n" + precision,
            spinners: false,
            decimals: precision
        });

        //15 decimal restriction for amount type else 16
        sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, options.model.TYPE === ValueTypeEnum.Amount ? 15 : 16);

        _setValueFinderEditor(options, gridName, "btnFinderId");
        _setEditorInitialValue(gridName, options);

        var editBox = $("#txtVALUE").data("kendoNumericTextBox");
        var mouseClick = false;
        _isFinderButton = false;
        $("#btnFinderId").mousedown(function () {
            mouseClick = true;
            _isFinderButton = true;
        });
        editBox.bind("change", function () {
            $("#btnFinderId").focus();
            if (mouseClick) {
                $("#btnFinderId").trigger("click");
            }
        });
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
        if (f === "FDESC" || f === "VDESC" || f === "SWSET") {
            return _noEditor(container, gridName);
        }
        if (f === "OPTFIELD") {
            return model.OPTFIELD ? _noEditor(container, gridName) : _finderEditor(container, options, gridName);
        }
        if (f === "SWREQUIRED" || f === "INITFLAG") {
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
            case RequestTypeEnum.Delete:
                requestName = "Delete";
                break;
            case RequestTypeEnum.Save:
                requestName = "Save";
                break;
            case RequestTypeEnum.Put:
                requestName = "Put";
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
            case RequestTypeEnum.Put:
                isSuccess ? _updateSuccess(gridName, jsonResult, fieldName) : _updateError(gridName, jsonResult, fieldName);
                break;
            case RequestTypeEnum.Delete:
                isSuccess ? _deleteSuccess(gridName, jsonResult) : _deleteError(gridName, jsonResult);
                break;
            case RequestTypeEnum.Save:
                isSuccess ? _saveSuccess(gridName, jsonResult) : _insertError(gridName, jsonResult);
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
        GridUtls.addingLine = true;
        var grid = $('#' + gridName).data("kendoGrid"),
            insertedIndex = grid.select().index() + 1,
            dataSource = grid.dataSource;
        if (insertedIndex >= grid.dataSource.pageSize()) {
            insertedIndex = 0;
        }
        //Due to we set the delay 100 here, the related delay will be 100 as well
        setTimeout(function () {
            dataSource.insert(insertedIndex, jsonResult.Data);
            grid.refresh();
            var row = _selectGridRow(grid, jsonResult.Data["KendoGridAccpacViewPrimaryKey"]);
            _setNextEditCell(grid, dataSource.options.schema.model, row, 0);
            _lastRowStatus[gridName] = RowStatusEnum.INSERT;
            _lastRowNumber[gridName] = insertedIndex;
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
        GridUtls.updateFailed = false;
        _lastRowStatus[gridName] === RowStatusEnum.NONE;
        //_dataChanged[gridName] = false;
    }

    /**
     * @description Send insert request error
     * @param {string} gridName The grid name
     * @param {object} jsonResult The request call back result
     */
    function _insertError(gridName, jsonResult) {
        GridUtls.updateFailed = true;
        var grid = $('#' + gridName).data("kendoGrid");
        var rowIndex = grid.select().index();
        sg.utls.showMessage(jsonResult, errorMsgClose.bind(gridName, _lastLine, _lastColField[gridName]));
        _lastRowNumber[gridName] = _lastLine;
    }

    function errorMsgClose(rowNum, colName) {
        var grid = $('#' + this).data("kendoGrid");
        $("#" + this).unbind("keydown");
        _setEditCell(grid, rowNum, colName);
    }

    /**
    * @description Send update request success, refresh the grid and select row and column
    * @param {string} gridName The grid name
    * @param {object} jsonResult The request call back result
    * @param {string} fieldName update field name
    */
    function _updateSuccess(gridName, jsonResult, fieldName) {
        //todo reset the value when error happened
        GridUtls.updateFailed = false;
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
        //For Yes/No type, make sure the value not empty, default value get from VALIFBOOL field
        if (dataItem.TYPE === ValueTypeEnum.YesNo && dataItem.VALUE === "") {
            dataItem.VALUE = dataItem.VALIFBOOL ? "1" : "0";
        }

        //Grid refresh will remove text editor, it will trigger text editor blur event will cause error. We pre remove the editor
        var textEditor = $('input[name=VALUE]');
        if (textEditor && textEditor.length > 0) {
            textEditor.remove();
        }

        //Try...catch used for unpredicted behaviours that we don't want to break the code
        try {
            grid.refresh();
        } catch (e) {
            console.log(e);
        }

        var index = window.GridPreferencesHelper.getGridColumnIndex(grid, fieldName);
        var row = _selectGridRow(grid, jsonResult.Data["KendoGridAccpacViewPrimaryKey"]);
        _setNextEditCell(grid, dataItem, row, index + 1);

        if (fieldName === "OPTFIELD") {
            if (dataItem.TYPE === ValueTypeEnum.Time && (dataItem.VALUE === "" || dataItem.VALUE === "000000")) {
                dataItem.SWSET = 0;
                dataItem.VALUE = "";
                dataItem.VDESC = "";
                return;
            }
            var isDefault = dataItem.TYPE === ValueTypeEnum.Text || dataItem.TYPE === ValueTypeEnum.Date ? dataItem.VALIDATE && !dataItem.ALLOWNULL : dataItem.VALIDATE;
            if (isDefault) {
                _getDefaultValue(gridName, dataItem.OPTFIELD);
                return;
            }
        }
    }

    /**
     * Get default value for insert record, send insert request 
     * @param {string} gridName The grid name
     * @param {string} optionalField The optional field
     */
    function _getDefaultValue(gridName, optionalField) {
        var grid = $('#' + gridName).data("kendoGrid"),
            selectRow = grid.select(),
            dataItem = grid.dataItem(selectRow),
            url = sg.utls.url.buildUrl("Core", "ViewFinder", "RefreshGrid"),
            finder = {};

        finder.viewID = "CS0012";
        finder.viewOrder = 0;
        finder.filter = "OPTFIELD=" + optionalField;
        finder.displayFieldNames = ["VALUE", "VDESC"];
        finder.returnFieldNames = ["VALUE", "VDESC"];

        sg.utls.ajaxPostSync(url, finder, function (jsonResult) {
            if (jsonResult.Data && jsonResult.Data.length > 0) {
                dataItem.VALUE = jsonResult.Data[0].VALUE;
                dataItem.VDESC = jsonResult.Data[0].VDESC;
                //dataItem.SWSET = 1;
                _sendRequest(gridName, RequestTypeEnum.Insert, "");
            }
        });
    }
    /**
     * @description Send update request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {any} fieldName The update field name
     */
    function _updateError(gridName, jsonResult, fieldName) {
        GridUtls.updateFailed = true;
        var grid = $('#' + gridName).data("kendoGrid"),
            selectRow = grid.select(),
            rowIndex = selectRow.index(),
            dataItem = grid.dataItem(selectRow);

        _setModuleVariables(gridName, RowStatusEnum.UPDATE, jsonResult, rowIndex);

        sg.utls.showMessage(jsonResult, AfterUpdateError.bind(sg.optionalFieldControl, gridName, dataItem, fieldName, rowIndex));
    }

    function AfterUpdateError(gridName, dataItem, fieldName, rowIndex) {
        console.log("AfterUpdateError");

        var grid = $('#' + gridName).data("kendoGrid");

        _lastErrorResult[gridName].message = "";
        //_lastRowNumber[gridName] = -1;
        if (dataItem.TYPE === ValueTypeEnum.Time) {
           dataItem[fieldName] = "";
        } else {
           dataItem[fieldName] = _lastErrorResult[gridName][fieldName + "Value"];
        }

        if (dataItem[fieldName] === "") {
            // clear description fields residual data
            switch (fieldName) {
                case "OPTFIELD":
                    _blankGridCell(gridName, rowIndex, dataItem, "FDESC", "");
                    break;
                case "VALUE":
                    dataItem["SWSET"] = 0;
                    _blankGridCell(gridName, rowIndex, dataItem, "VDESC", "");
                    break;
                default:
                    break;
            }
        }

        _setEditCell(grid, rowIndex, fieldName);
    }
    /**
* @description Send delete request success, refresh the grid
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

    function _saveSuccess(gridName, jsonResult) {
        GridUtls.addingLine = false;
    }

    /**
     * @description Send delete request error
     * @param {string} gridName The grid name
     * @param {any} jsonResult The request call back result
     * @param {any} fieldName The update field name
     */
    function _deleteError(gridName, jsonResult) {
        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.read();
        sg.utls.showMessage(jsonResult);
    }

    /**
     * @description Send sync ajax request
     * @param {string} gridName The grid name
     * @param {number} requestType The request type
     * @param {string} fieldName The field name
     */
    function _sendRequest(gridName, requestType, fieldName, callback) {
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

            if (callback)
                callback(isSuccess);
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
     * @description Reset the optional grid varaibales to default values
     * @param {string} gridName The name of the grid
     */
    function _reset(gridName) {
        _setDefaultRow[gridName] = true;
        _lastErrorResult[gridName] = {};
        _lastRowNumber[gridName] = -1;
        _lastColField[gridName] = "";
        _lastRowStatus[gridName] = RowStatusEnum.NONE;
        _sendChange[gridName] = true;
    }

    /**
     * @description On grid data source data changes
     * @param {string} gridName The grid name
     * @param {object} e The event object
     */
    function _onDataChanged(gridName, e) {

        const deleteButtonId = `#btn${gridName}Delete`;

        if (e.action) {
            _dataChanged[gridName] = true;
        }
        var grid = _getGrid(gridName),
            count = grid.dataSource.total();

        if (count === 0) {
            disableButton(deleteButtonId);

        } else if (e.action === "add") {
            enableButton(deleteButtonId);

        } else {
            // To enable the delete button if records loaded by default
            //enableButton(deleteButtonId);

            // AT-67206
            const allowDelete = _settings[gridName].allowDelete;
            enableDisableButton(deleteButtonId, allowDelete);
        }

        if (e.items.length === 0 && grid.dataSource.page() !== 1 && count === 0) {
            grid.dataSource.page(1);
        }
        if (e.action === "itemchange") {
            var type = RequestTypeEnum.Put;
            if (e.items[0].OPTFIELD === "" || !_sendChange[gridName]) {
                _sendChange[gridName] = true;
                return;
            }
            if (e.field === "OPTFIELD") {
                type = RequestTypeEnum.Update;
                //update the adding line
                GridUtls.addingLine = false;
            }
            if (e.field === "VALUE") {
                var sel = grid.select();
                if (sel !== null) {
                    var r = grid.dataItem(sel);
                    if (r.TYPE === ValueTypeEnum.Time) {
                        var puncts = (r.VALUE.match(/_/g) || []).length;
                        if (0 < puncts && puncts < 6) {
                            grid.dataItem(sel).VALUE = r.VALUE.replace(/_/g, "0");
                        }
                    }
                    if ((r.VALUE === null || r.VALUE === "") && 0 < sel.length) {
                        grid.dataItem(sel).SWSET = 0;
                        _blankGridCell(gridName, sel[0].rowIndex, grid.dataItem(sel), "VDESC", "");
                        return;
                    }
                }
            }
            _sendRequest(gridName, type, e.field);
        }
    }

    /**
     * @description Initialize a grid, creating dataSource to binding grid, register events handler
     * @param {string} gridName The name of the grid.
     * @param {object} settings The optional field settings object
     * @param {boolean} readOnly Whether the grid allows editing(Optional).
     */
    function init(gridName, settings, readOnly) {

        const self = this;

        var columns = _getGridColumns(gridName),
            addTemplate = kendo.format(ButtonTemplate, 'btn-add', 'sg.optionalFieldControl.addLine(&quot;' + gridName + '&quot;)', globalResource.AddLine, gridName, "Add"),
            delTemplate = kendo.format(ButtonTemplate, 'btn-delete', 'sg.optionalFieldControl.deleteLine(&quot;' + gridName + '&quot;)', globalResource.DeleteLine, gridName, "Delete");

        readOnly = readOnly || false;
        _gridName = gridName;
        if (readOnly) {
            settings.allowInsert = false;
            settings.allowDelete = false;
        }
        _settings[gridName] = settings;
        _reset(gridName);
        _filter[gridName] = "";
        _dataChanged[gridName] = false;
		_currentPage[gridName] = 1;

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
                _lastLine = lastRowNumber;
                _gridName = gridName;
                //todo need to know the line exists, otherwise inter it
                if (lastRowNumber > -1 && lastRowNumber !== selectedIndex && !GridUtls.addingLine && !GridUtls.updateFailed) {
                    _lastRowNumber[gridName] = selectedIndex;
                    _sendRequest(gridName, RequestTypeEnum.Save, "");
                }
            },

            dataBound: function (e) {
                var grid = e.sender,
                    pageSize = grid.dataSource.pageSize() - 1;

                grid.tbody.find("tr:gt(" + pageSize + ")").hide();
                if (_setDefaultRow[gridName]) {
                    grid.select("tr:eq(0)");
                }
                setTimeout(function () {
                    GridUtls.addingLine = false;
                }, 100);
            },

            pageable: {
                pageSize: 10,
                numeric: false,
                buttonCount: 1,
                input: true
            },

            toolbar: [{ template: addTemplate }, { template: delTemplate }],

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
                            _reset(gridName);
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
                    // Commit any grid changes if there is a new line or data has changed while pagination
                    if (_dataChanged[gridName] && _currentPage[gridName] !== this.page()) {
                        e.preventDefault();
                        _commitGrid(gridName, (isSuccess) => {
                            if (isSuccess) {
                                _dataChanged[gridName] = false;
                                _currentPage[gridName] = this.page();
                                this.page(this.page());
                            }
                        });
                        return;
                    }
                    _currentPage[gridName] = this.page();
                }
            }
        });

        const addButtonId = `#btn${gridName}Add`;
        const deleteButtonId = `#btn${gridName}Delete`;
        enableDisableButton(addButtonId, settings.allowInsert);
        enableDisableButton(deleteButtonId, settings.allowDelete);

        $(document).on("click", ".msgCtrl-close", function (e) {
            _setGridCell.call(this, gridName);
        });

        sg.utls.initGridKeyboardHandlers(gridName, true,
            {
                addLine: self.addLine,
                deleteLine: self.deleteLine,
                showDetails: null, // Not needed
                editColumnSettings: null, // Not needed
            },
            {
                addLineButtonName: `btn${gridName}Add`,
                deleteLineButtonName: `btn${gridName}Delete`,
                viewEditDetailsButtonName: '',
                editColumnSettingsButtonName: ''
            }
        );
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

                    if (dataItem.KendoGridAccpacViewIsRecordNew) {
                        grid.dataSource.remove(dataItem);
                    } else {
                        _sendRequest(gridName, RequestTypeEnum.Delete, "");
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

        var grid = _getGrid(gridName);

        var selectedRowData = null;

        if (_lastRowNumber[gridName] !== -1) {
            selectedRowData = grid.dataItem(grid.select());

            if (selectedRowData.dirty) {

                // Save the row
                _sendRequest(gridName, RequestTypeEnum.Save, "", function (isSuccess) {
                    console.log("add line call back");
                    if (isSuccess) {
                        _addLine(gridName);
                    }
                });

                return;
            }
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

        if (value === null || data.SWSET === 0) {
            return "";
        } else if (type === ValueTypeEnum.YesNo) {
            return value.trim() === "1" ? globalResource.Yes : globalResource.No;
        } else if (type === ValueTypeEnum.Date) {
            return value === "00000000" ? "" : sg.utls.kndoUI.getFormattedDate(value);
        } else if (type === ValueTypeEnum.Time && value.indexOf(':') < 0) {
            return value.substring(0, 2) + ":" + value.substring(2, 4) + ":" + value.substring(4, 6);
        } else if (type === ValueTypeEnum.Integer || type === ValueTypeEnum.Amount || type === ValueTypeEnum.Number) {
            return '<span style="float:right">' + sg.utls.kndoUI.getFormattedDecimalNumber(value || 0, decimals) + '</span>';
        } else {
            return sg.utls.htmlEncode(value);
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
    function readOnly(gridName, readOnly) {
        const grid = _getGrid(gridName);

        grid.setOptions({ editable: !readOnly });
        allowDelete(gridName, !readOnly);
        allowInsert(gridName, !readOnly);
    }

    /**
     * @description Read data from the server side, refresh the view list display, and reset the page number to 1.
     * @param {any} gridName The name of the grid.
    */
    function refresh(gridName) {
        var grid = _getGrid(gridName);
        grid.dataSource.page(1);
    }

    /**
     * @description On show the popup optional field window, execute this function
     * @param {string} gridName The name of the grid
     * @param {string} popupElementId The popup element id
     * @param {boolean} isReadOnly The booelan flag to indicate readonly property
     * @param {string} filter The filter will apply to grid
     * @param {string} parentGridName The parent grid name
     */
    function showPopUp(gridName, popupElementId, isReadOnly, filter, parentGridName) {
        sg.utls.openKendoWindowPopup('#' + popupElementId, null);
        if (filter) {
            _filter[gridName] = filter;
        }
        if (isReadOnly !== undefined) {
            readOnly(gridName, isReadOnly);
        }
        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.page(1);

        const btnWinClose = $('#' + popupElementId).closest('.k-window').find('.k-icon.k-i-close').closest('a');
        btnWinClose.off('click');
        btnWinClose.on("click", function (e) {
            const grid = _getGrid(gridName);
            const row = grid.select();
            const rowData = grid.dataItem(row);
            const value = $('input[name=VALUE]').val();

            if (value && rowData && rowData.VALUE != value) {
                rowData.VALUE = value;
                _sendRequest(gridName, RequestTypeEnum.Put, 'VALUE');
                rowData.dirty = true;
                dirty(gridName, true);
            }

            if (rowData && rowData.dirty) {
                _sendRequest(gridName, RequestTypeEnum.Save);
            }

            if (parentGridName && _dataChanged[gridName]) {
                setTimeout(() => sg.viewList.commit(parentGridName), 100);
            }
        });
    }

    /**
     * @description On close the popup optional field window from details grid, execute this function
     * @param {string} gridName The name of optional grid
     * @param {string} parentGridName The parent grid name
     */
    function closePopUp(gridName, parentGridName) {
        if (parentGridName.length > 0) {
            let parentGrid = $("#" + parentGridName).data("kendoGrid");
            if (parentGrid) {

                let selectedItem = parentGrid.dataItem(parentGrid.select());

                if (sg.optionalFieldControl.dirty(gridName)) {
                    // Note - Changing the traditional use of set call, instead directly assigning it because kendo doesn't allow "set" call if the field property
                    // is not editable. Which is a bit of concern for other fields, if we try to manually update the dataSource fields which is read only
                    // (most likely we don't even have to update the read only properties directly if they are non-editable)
                    sg.viewList.refreshCurrentRow(parentGridName, 'VALUES');
                    selectedItem.dirty = true;
                }
            }
        }

        _reset(gridName);
    }

    /**
     * @description Get/Set the “Insertable” property of a grid.
     * @description If the grid does not allow insert, the “Add Line” button should be disabled
     * @param {string} gridName The name of the grid
     * @param {booelan} insertable A boolean flag to indicate whether grid allow insert new line
     * @return {boolean} return true if the grid allows insert or null
     */
    function allowInsert(gridName, insertable) {

        const addButtonId = `#btn${gridName}Add`;

        if (insertable !== undefined) {
            _settings[gridName].allowInsert = insertable;

            enableDisableButton(addButtonId, insertable);
            return;
        }
        var disabled = $(addButtonId).prop("disabled");
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

        const deleteButtonId = `#btn${gridName}Delete`;

        if (deletable !== undefined) {
            _settings[gridName].allowDelete = deletable;

            enableDisableButton(deleteButtonId, deletable);
            return;
        }
        var disabled = $(deleteButtonId).prop("disabled");
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
     * @param {function} callBack The callBack function after the action is completed
     * @return {boolean} Return a boolean flag to indicate success or not
     */
    function commit(gridName, callBack) {
		var result = true;
        if (gridName) {
            result = _commitGrid(gridName, callBack);
        }

        return result;
    }


    // Developer Note: Functions below are meant to be private and only consumed within this file.

    /**
     * @function
     * @name enableButton
     * @description Enable a button based on it's id
     * @namespace sg.optionalFieldControl
     * @private
     *
     * @param {string } _id The button id
     */
    function enableButton(_id) {
        enableDisableButton(_id, true);
    };

    /**
     * @function
     * @name disableButton
     * @description Disable a button based on it's id
     * @namespace sg.optionalFieldControl
     * @private
     *
     * @param {string } _id The button id
     */
    function disableButton(_id) {
        enableDisableButton(_id, false);
    };

    /**
     * @function
     * @name enableDisableButton
     * @description Enable or disable a button based on it's id
     * @namespace sg.optionalFieldControl
     * @private
     *
     * @param {string } _id The button id
     * @param {boolean} _enable true = Enable | false = Disable
     */
    function enableDisableButton(_id, _enableOrDisable) {
        // Ensure there's a '#' prefixed to id
        let id = prependHashTag(_id);

        if (_enableOrDisable) {
            // Enable
            $(id).prop('disabled', false);
        } else {
            // Disable
            $(id).prop('disabled', true);
        }
    }

    /**
     * @function
     * @name prependHashTag
     * @description Given an id string, prepend a '#' character if it doesn't yet exist
     * @namespace sg.optionalFieldControl
     * @private
     *   
     * @returns {string} A string that begins with a '#' character
     */
    function prependHashTag(_id) {
        let id = _id;

        // Check for # character. Prepend if it doesn't exist
        if (id.length > 0 && id.charAt(0) !== '#') {
            id = `#${_id}`;
        }

        return id;
    };

    // Expose module(class) public methods
    return {
        init: init,

        // Grid internal methods(For grid toolbar, column template use)
        addLine: toolbarAddLine,
        deleteLine: toolbarDeleteLine,
        getTemplate: getTemplate,
        getListText: getListText,
        showPopUp: showPopUp,
        closePopUp: closePopUp,

        // Documented public methods
        allowInsert: allowInsert,
        allowDelete: allowDelete,
        readOnly: readOnly,
        filter: filter,
        dirty: dirty,
        commit: commit,
        refresh: refresh,
    };
}();
