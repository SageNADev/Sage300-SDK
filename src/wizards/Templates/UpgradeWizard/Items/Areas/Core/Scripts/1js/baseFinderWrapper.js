/* Copyright (c) 1994-2022 Sage Software, Inc.  All rights reserved. */

(function () {
    'use strict';

    /** Screen finder object */
    let finderWrapper = {

        viewId: "",
        textBoxId: undefined,
        fieldId: undefined,
        fieldsObj: undefined,
        fieldsId: [],
        entityCollObj: undefined,
        finderFilter: '',
        finderPopupWinId: undefined,
        selectRowData: undefined,

        /**
         * Init popup finder screen
         * @param {any} finderPopupWinId finder popup div id
         */

        initFinder: function (finderPopupWinId = 'divFinderPopup') {
            this.finderPopupWinId = finderPopupWinId;
            sg.utls.initializeKendoWindowPopupWithMaximize('#' + finderPopupWinId, `Finder Screen`);

            $(".clsValueDropDown").hide();

            $("#finderOperatorDropdown").kendoDropDownList({
                autoBind: false,
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: []
            });

            $("#finderValueTextBox").val("");
            $('#finderValueDropDown').kendoDropDownList({
                autoBind: false,
                dataTextField: "display",
                dataValueField: "value",
                dataSource: []
            });

            $("#finderColumnDropdown").kendoDropDownList({
                autoBind: false,
                dataTextField: "title",
                dataValueField: "field",
                dataSource: [],
                change: () => this.onFieldChange()
            });

            $("#finderNumericTextBox").kendoNumericTextBox({
                spinners: false,
                decimals: 3,
                restrictDecimals: true
            });

            $('#finderDateTextBox').kendoDatePicker();
            $('#finderDateTextBox').on('change', e => {
                let currentValue = sg.utls.kndoUI.checkForValidDate($(`#${e.target.id}`).val());
                let defaultValue = new Date().toLocaleDateString();
                let value = currentValue || defaultValue;
                $("#" + e.target.id).val(value);
            }),
            
            $('#divFinderGrid').kendoGrid({
                dataSource: {},
                columns: [],
                editable: false,
                navigatable: false,
                selectable: true,
                scrollable: true,
                resizable: true,
                reorderable: true,
                pageable: {
                    pageSize: 10,
                    numeric: false
                },
                dataBound: () => {
                    $("#divFinderGrid").off("dblclick");
                    $("#divFinderGrid").on("dblclick", "tbody>tr", function () {
                        $("#btnFinderSelect").trigger('click')
                    });
                }
            });
            
            $("#btnFinderSelect").on("click", () => {
                const finderWin = $("#" + this.finderPopupWinId).data("kendoWindow");
                const grid = $('#divFinderGrid').data('kendoGrid');
                const selectedItem = grid.dataItem(grid.select());
                const value = selectedItem[this.fieldId];

                $('#' + this.textBoxId).val(value);
                this.selectRowData = selectedItem;

                finderWin.close();
                $('#' + this.textBoxId).trigger('focus');
            });

            $("#btnFinderCancel").on("click", () => {
                let finderWin = $("#" + this.finderPopupWinId).data("kendoWindow");
                finderWin.close();
                this.selectRowData = {};
                $('#' + this.textBoxId).trigger('focus');
            });

            $("#btnFinderSearch").on("click", () => {
                this.loadFinderData(true);
            });

            $("#btnFinderPrefEditCols").on("click", () => {
                const prefGrid = $('#divFinderGrid').data('kendoGrid');
                GridPreferencesHelper.initialize('#divFinderGrid', this.viewId + 'FinderPreferencesUniqueId', $(btnFinderPrefEditCols), prefGrid.columns);
                $('#divGridPrefEditCols').css({ top: 88, left: 14, position: 'absolute', "z-index": "1000" });
            });
        },

        /**
         * Show finder popup screen
         * @param {any} finderOptions finder options parameters
         */

        showFinder: function (finderOptions) {
            this.viewId = finderOptions.viewId;
            this.textBoxId = finderOptions.txtBoxId;
            this.fieldId = finderOptions.fieldId;
            this.fieldsObj = finderOptions.fieldsObj;
            this.entityCollObj = finderOptions.entityCollObj;
            this.finderFilter = finderOptions.filter || '';

            let title = this.fieldsObj[this.fieldId].title;
            let finderTitle = `${globalResource.Select} ${title}`;

            this.setColumns();
            const finderWin = sg.utls.openKendoWindowPopup("#" + this.finderPopupWinId, null);
            finderWin.title(finderTitle);

            this.initControls();
            this.loadFinderData();

            return finderWin;
        },

        /**Init finder screen controls */
        initControls: function () {
            $('#divValueDropDown').hide();
            $("#divNumericTextBox").hide();
            $("#divDateTextBox").hide();
            $("#divOperatorDropdown").show();
            $("#finderValueTextBox").show();
            $("#finderOperatorDropdown").data('kendoDropDownList').value('');
            $("#finderValueTextBox").val('');
            $('#divFinderPrefEditCols').hide();

            this.disableControls(true);
        },

        /** Set finder screen grid columns */
        setColumns: function () {
            const numericType = [sg.finderDataType.Integer, sg.finderDataType.Decimal, sg.finderDataType.Amount, sg.finderDataType.Number, sg.finderDataType.SmallInteger];
            const ddlCols = $("#finderColumnDropdown").data("kendoDropDownList");
            const finderGrid = $('#divFinderGrid').data('kendoGrid');

            let gridColumns = [];
            let gridOptions = finderGrid.getOptions();
            let columns = [{ title: ShowAllRecords, field: "ShowAllRecords" }];
            let fieldsId = [];
            for (const prop in this.fieldsObj) {
                const fieldObj = this.fieldsObj[prop];
                if (fieldObj.useInfinder) {
                    columns.push({ title: fieldObj.title, field: fieldObj.field });
                    let col = { title: fieldObj.title, field: fieldObj.field, width: fieldObj.width };
                    if (numericType.includes(fieldObj.dataType.toLowerCase())) {
                        col.attributes = { style: 'text-align: right' };
                    }
                    gridColumns.push(col);
                    fieldsId.push(fieldObj.id);
                }
            }
            this.fieldsId = fieldsId;
            gridOptions.columns = gridColumns;
            finderGrid.setOptions(gridOptions);

            ddlCols.dataSource.data(columns);
            ddlCols.select(0);
        },

        /** Build view filter string */
        buildSearchFilter: function () {
            const operator = $("#finderOperatorDropdown").data('kendoDropDownList').value();
            const column = $("#finderColumnDropdown").data('kendoDropDownList').value();
            const ddlValue = $("#finderValueDropDown").data('kendoDropDownList').value();
            const txtValue = $("#finderValueTextBox").val();
            const numValue = $("#finderNumericTextBox").val();
            const dtValue = kendo.toString(new Date($('#finderDateTextBox').val()), 'yyyyMMdd');

            const fieldObj = this.fieldsObj[column];
            let dataType = this.fieldsObj[column].dataType.toLowerCase();
            if (fieldObj.formatList && fieldObj.length > 0) {
                dataType = 'dropdownList';
            }

            let filter = '';
            switch (dataType) {
                case 'char':
                    filter = operator === sg.finderOperator.StartsWith ? `${column} LIKE "${txtValue}%"` : `${column} LIKE "%${txtValue}%"`;
                    break;
                case sg.finderDataType.Boolean:
                case 'dropdownList':
                    filter = `${column} = ${ddlValue}`;
                    break;
                case sg.finderDataType.Date:
                case sg.finderDataType.Time:
                    filter = `${column} ${operator} ${dtValue}`;
                    break;
                case sg.finderDataType.Integer:
                case sg.finderDataType.Decimal:
                case sg.finderDataType.Amount:
                case sg.finderDataType.Number:
                case sg.finderDataType.SmallInteger:
                    filter = `${column} ${operator} ${numValue}`;
                    break;
                default:
            }
            return apputils.escape(filter);
        },

        /**
         * Load data from view based on filter
         * @param {any} forSearch Specify whether search button is click in finder screen
         */
        loadFinderData: function (forSearch = false) {
            const grid = $('#divFinderGrid').data('kendoGrid');
            let filter = this.finderFilter;

            if (forSearch) {
                let searchFilter = this.buildSearchFilter();
                if (searchFilter) {
                    filter = filter ? `${filter} AND ${searchFilter}` : searchFilter;
                }
            }
            const ids = this.fieldsId.length > 0 ? this.fieldsId.join(',') : '';
            const finderQuery = `<n t='' n=''><r i='${this.viewId}' f='${filter}' p='' id='0' verb='Get' ids='${ids}'></r></n>`;

            let Ok = this.entityCollObj._executeXSearch2(finderQuery);
            Ok.then((result, status, xhr) => {
                if (xhr.hasError) {
                    return;
                }
                let gridData = this.entityCollObj.loadDataForGrid("");
                grid.dataSource.data(gridData);

                if (gridData.length > 0) {
                    const value = $('#' + this.textBoxId).val();
                    if (value && !forSearch) {
                        this.selectGridRow(value, grid, this.fieldId); //go to the page and select the row
                    } else {
                        grid.dataSource.page(1);
                        grid.select("tr:eq(0)");
                    }
                }
            });
        },

        /**
         * disbable/enable controls
         * @param {any} disabled control disable flag
         */
        disableControls: function (disabled = true) {
            $("#finderOperatorDropdown").data('kendoDropDownList').enable(!disabled);
            $("#btnFinderSearch").prop('disabled', disabled);
            $("#finderValueTextBox").prop('disabled', disabled);
        },

        /**Field drop down list box select change handler */
        onFieldChange: function () {
            const dropdownlist = $("#finderColumnDropdown").data("kendoDropDownList");
            const selectedValue = dropdownlist.value();
            const selectedText = dropdownlist.text();

            $("#finderValueTextBox").prop('disabled', false);

            if (selectedText === ShowAllRecords) {
                $("#finderOperatorDropdown").data('kendoDropDownList').value('');
                this.resetValueTextBox();
                this.disableControls(true);
                this.loadFinderData();
            } else if (selectedValue.length > 0) {
                let field = this.fieldsObj[selectedValue];
                let dataType = field.dataType.toLowerCase();
                this.initOperatorDropdown(field);

                if (dataType === sg.finderDataType.Boolean || field.formatList) {
                    dataType = 'dropdownList';
                    if (field.formatList) {
                        let ddlValue = $('#finderValueDropDown').data('kendoDropDownList');
                        ddlValue.dataSource.data(field.formatList);
                    }
                }

                this.resetValueTextBox(dataType);
                $("#btnFinderSearch").prop("disabled", false);
            } else {
                this.resetValueTextBox();
            }
            dropdownlist.focus();
        },

        /**
         * Reset the screen controls initial value
         * @param {any} dataType
         */
        resetValueTextBox: function (dataType = 'char') {
            const id = "#finderValueTextBox";
            const divOperator = "#divOperatorDropdown";
            const divValueDropDown = "#divValueDropDown";
            const divNumericTextBox = "#divNumericTextBox";
            const divDateTextBox = "#divDateTextBox";

            $(id).val("");
            $(id).show();
            $(divOperator).show();
            $(divValueDropDown).hide();
            $(divNumericTextBox).hide();
            $(divDateTextBox).hide()

            switch (dataType) {
                case 'char':
                    break;
                case sg.finderDataType.Boolean:
                case 'dropdownList':
                    $(id).hide();
                    $(divOperator).hide();
                    $(divValueDropDown).show();
                    break;
                case sg.finderDataType.Date:
                case sg.finderDataType.Time:
                    $(id).hide();
                    $(divDateTextBox).show();
                    let date = new Date().toLocaleDateString();
                    $("#finderDateTextBox").val(date);
                    break;
                case sg.finderDataType.Integer:
                case sg.finderDataType.Decimal:
                case sg.finderDataType.Amount:
                case sg.finderDataType.Number:
                case sg.finderDataType.SmallInteger:
                    $(id).hide();
                    $(divNumericTextBox).show();
                    break;
                default:
            }
        },

        /**
         * Init operator dropdown list value based on field type 
         * @param {any} field selected field
         */
        initOperatorDropdown: function (field) {
            let operatorDatasource = [];
            let dataType = 'char';

            if (field != null) {
                dataType = field.dataType.toLowerCase();
                let list = field.formatList;
                if (list && list.length > 0) {
                    list.forEach(l => operatorDatasource.push({ Text: l.display, Value: l.value }));
                    dataType = 'dropdownList';
                } else if (dataType === sg.finderDataType.Boolean) {
                    operatorDatasource = [
                        { Text: globalResource.Yes, Value: true },
                        { Text: globalResource.No, Value: false }
                    ];
                } else if (dataType === sg.finderDataType.Date || dataType === sg.finderDataType.Integer || dataType === sg.finderDataType.Decimal || dataType === sg.finderDataType.Amount || dataType === sg.finderDataType.Number || dataType === sg.finderDataType.Time || dataType === sg.finderDataType.SmallInteger) {
                    operatorDatasource = [
                        { Text: globalResource.Equal, Value: "=" },
                        { Text: globalResource.GreaterThan, Value: ">" },
                        { Text: globalResource.GreaterThanOrEqual, Value: ">=" },
                        { Text: globalResource.LessThan, Value: "<" },
                        { Text: globalResource.LessThanOrEqual, Value: "<=" },
                        { Text: globalResource.NotEqual, Value: "!=" }
                    ];
                }
            }

            if (operatorDatasource.length === 0 ) {
                operatorDatasource = [
                    { Text: globalResource.StartsWith, Value: sg.finderOperator.StartsWith },
                    { Text: globalResource.Contains, Value: sg.finderOperator.Contains }
                ];
            }

            let ddlOperator = $('#finderOperatorDropdown').data('kendoDropDownList');
            ddlOperator.enable(true);
            ddlOperator.dataSource.data(operatorDatasource);
            ddlOperator.select(0);
            $('#finderValueTextBox').prop('disabled', false);
        },

        /**
         * Select the grid row
         * @param {any} searchValue Seach value, initial textbox value when finder screen popup
         * @param {any} grid finder screen kendo grid
         * @param {any} field field name
         */
        selectGridRow: function(searchValue, grid, field) {
            let dataSource = grid.dataSource;
            let filters = dataSource.filter() || {};
            let sort = dataSource.sort() || {};
            let models = dataSource.data();
            let query = new kendo.data.Query(models);
            let rowNum = 0;
            let modelToSelect = null;
            let value = searchValue || $('#' + this.textBoxId).val();

            models = query.filter(filters).sort(sort).data;
            for (let i = 0; i < models.length; ++i) {
                let model = models[i];
                if (model[field] >= value) {
                    modelToSelect = model;
                    rowNum = i;
                    break;
                }
                rowNum = i;
            }

            let currentPageSize = grid.dataSource.pageSize();
            let pageWithRow = parseInt((rowNum / currentPageSize)) + 1; // pages are one-based
            grid.dataSource.page(pageWithRow);

            var row = grid.element.find("tr[data-uid='" + modelToSelect.uid + "']");
            if (row.length > 0) {
                grid.select(row);
                grid.content.scrollTop(grid.select().position().top);
            }
        }
    }

    this.baseFinderWrapper = helpers.View.extend({}, finderWrapper);

}).call(this);