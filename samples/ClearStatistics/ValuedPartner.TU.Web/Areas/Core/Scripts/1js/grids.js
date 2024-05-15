(function() {
    'use strict';

    /** General grid definition */
    let defaultGrid = {
        gridId: undefined,
        grid: undefined,
        btnDelete: undefined,
        preventBinding: false,
        checkedIds: {},
        selectedRowIndex: -1,
        needsCustomProcessing: false,
        currentCell: undefined,
        currentRow: undefined,
        currentColumnName: undefined,
        currentValue: undefined,
        currentColIndex: undefined,
        selectedColIndex: undefined,
        selectRowChange: 'gridSelectRowChange',
        serverDefaultTotalPage: -1,
        stopLazyLoad: false,
        editableGrid: true,
        lazyLoadRunning: false,

        /** Init kendo grid and set data source  */
        load: function () {          
            let self = this;

            this.grid = $("#" + this.gridId).kendoGrid({
                read: {
                    cache: false
                },
                dataSource: this.initDataSource(),
                /*{
                    //serverPaging: true,
                    data: this.loadData(),
                    pageSize: this.pageSize()                    
                },*/
                dataBinding: function(e) {
                    if (this.preventBinding) {
                        e.preventDefault();
                    }
                    this.preventBinding = false;
                },
                scrollable: this.scrollable(),
                pageable: this.pageable(),
                navigatable: true,
                reorderable: true,
                resizable: true,
                selectable: this.selectable(),
                editable: this.editable(),
                columns: this.loadColumn(),
                edit: function (elem) {
                    self.editableCell(elem);
                },

                page: async function (e) {
                    self.pageEvent(e);

                }

            }).data("kendoGrid");

            this.grid.businessObject = this.businessObject;

            // Duplicate class when init kendo grid, remove it see: AT-73082 issue 3
            $("#" + this.gridId).removeClass();

            /* left as example to remove grid table rows using jquery
            $.each(this.grid.items(), function(index, item) {
					$(item).remove();
				});
            */
            return this;
        },

        pageEvent: async function (event) {
            //override as neede - see PMContractTransactionGrid.js
        },

        initDataSourceTemplate: function () {

            return {
                serverPaging: false, //required to maintain total after initial getTotalRowCount call
                data: this.loadData(),
                pageSize: this.pageSize()
            }
        },

        /** Init data source */
        initDataSource: function () {

            if (this.getServerPaging()) {
                return this.initDataSourceForServerPaging();

            }

            return this.initDataSourceTemplate();
        },

        initDataSourceForServerPaging: function () {
            const self = this;

            let initDS = this.protoExt.initDataSourceTemplate.apply(this, arguments);

            initDS.schema = {
                total: function () {
                    return self.getTotalRowCount();
                }
            }

            return initDS;

        },

        /** page size definition */
        pageSize: function() {
            return 10;
        },

        scrollable: function() {
            return true;
        },

        /** kendo grid page definition */
        pageable: function () {
            return {
                numeric: false,
                input: true
            };
        },
		
        selectable: function() {
            return true;
        },

        editable: function() {
            return true;
        },

        //see PMReviseEstimatesPopupGrid.js how to override editableCell function
        editableCell: function (elem) {
            
            if (apputils.isUndefined(this.businessObject)) {
                return true;
            }

            let column = elem.sender.columns[elem.container.index()];

            let data = {
                viewid: elem.model.viewid,
                rowIndex: elem.model.RowIndex,
                field: column.field,
                value: elem.model[column.field]
            };

            let result = this.businessObject.checkIfUpdateAllowed(data);

            //for now throw error so devs know to implement
            if (apputils.isUndefined(result.editAllowed)){
                trace.error("checkIfUpdateAllowed - not implemented for this grid " + elem.model.viewid);
            }

            if (apputils.isUndefined(result.editAllowed) || result.editAllowed) {
                
                result.rowIndex = elem.model.RowIndex;
                result.msgid = elem.model.msgid;
                result.viewid = elem.model.viewid,

                //pass in the cell that started the editing
                result.editingCell = data;
                this.businessObject.editingGridHandle = { result: result };

                
            } else {

                this.grid.closeCell();
                result.rowIndex = elem.model.RowIndex;
                result.msgid = elem.model.msgid;
                result.viewid= elem.model.viewid,

                //pass in the cell that started the editing
                result.editingCell = data;
                this.businessObject.handleDisallowedGridFields(result);
            }
        },

        loadData: function() {
            trace.log("loadData");
        },

        loadColumn: function() {
            trace.log("loadColumn");
        },

        /**
         * Update(Refresh) grid data
         * @param {any} data The data to update
         * @param {boolean} resetPage True to reset page, false otherwise
         */
        updateGridData: function (data, resetPage) {

            if (apputils.isUndefined(this.grid)) {
                return;
            }

            this.grid.businessObject = this.businessObject;

            this.recordIdChanged();

            if (this.getServerPaging()) {
                this.updateGridDataWithServerPaging(data, resetPage);

                return;
            }

            if (resetPage){
                this.resetPage();
            }
            if (apputils.isUndefined(this.grid) || apputils.isUndefined(data)){
                return;
            }

            this.grid.dataSource.options.data = data;
            this.grid.dataSource.transport.data = data;
            this.grid.dataSource.read();
            this.afterDataBound(this.grid);
        },
        
        updateGridDataWithServerPaging: function (data, resetPage) {
            if (resetPage) {
                this.resetPage();
            }
            if (apputils.isUndefined(this.grid) || apputils.isUndefined(data)) {
                return;
            }
                        
            this.grid.businessObject = this.businessObject;
            this.grid.dataSource.options.serverPaging = true;
            this.grid.dataSource.options.schema.total();
            this.grid.dataSource.options.serverPaging = false;

            this.grid.dataSource.options.data = data;
            this.grid.dataSource.transport.data = data;
            this.grid.dataSource.read();

            if (this.serverDefaultTotalPage <= 0) {
                this.serverDefaultTotalPage = this.grid.dataSource.totalPages();
            }

            this.afterDataBound(this.grid);
            

            if (data.length > 0 && this.getTotalRowCount() > data.length) {

                $(".k-link.k-pager-nav.k-pager-last").prop("disabled", true).addClass("k-state-disabled");
                this.fetchLazyData();
            } 
        },

        /**
         * After load data, select the first row, grid focus on value column for optional field grid
         * @param {any} grid Kendo grid
         */
        afterDataBound: function (grid) {
            const length = grid.dataSource.data().length;
            $(`#${this.btnDelete}`).prop('disabled', length === 0);
            if (length > 0) {
                let colIndex = 0;
                grid.select('tr:eq(0)');
                this.selectedRowIndex = 0;
                const row = grid.select();
                const dataItem = grid.dataItem(row);

                //Focus Optional field grid value column cell
                if (dataItem && dataItem.OPTFIELD && dataItem.VALUE) {
                    colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, 'VALUE');
                    const cell = grid.tbody.find("tr").eq(0).find("td").eq(colIndex);
                    grid.current(cell);
                    grid.editCell(cell);
                    $(row).trigger('click');
                }
            }
        },

        /** reset page to first page*/
        resetPage: function () {
            
            this.grid = $("#" + this.gridId).data("kendoGrid");

            if (this.grid) {
                this.grid.businessObject = undefined;
                this.grid.dataSource.page(0);
            }
            this.selectedRowIndex = -1;
        },

        /** Reset grid data */
        resetGridData: function() {
            this.resetPage();
            this.selectedRowIndex = -1;
        },

        /**
         *  Update grid column after grid init
         * @param {any} columns grid columns array object
         */
        updateGridColumn: function (columns) {
            let options = this.grid.getOptions();
            options.columns = columns;
            this.grid.setOptions(options);

            //After setOptions, need register event handlers
            this.grid.table.on("keydown", e => this.keydownHandler(e, this.grid));
            this.grid.table.on("focus", e => this.focusHandler(e, this.grid));
        },

        /**
         *  Set grid setOptions only if necessary
         * @param {boolean} isEditable
         */
        makeGridEditable: function (isEditable) {
            if (this.editableGrid !== isEditable) {
                this.editableGrid = isEditable;

                let options = this.grid.getOptions();
                options.editable = isEditable;
                this.grid.setOptions(options);

                //After setOptions, need register event handlers
                this.grid.table.on("keydown", e => this.keydownHandler(e, this.grid));
                this.grid.table.on("focus", e => this.focusHandler(e, this.grid));
            }
        },

        /** Select grid row */
        selectRow: function() {
            this.grid = $("#" + this.gridId).data("kendoGrid");
            let dataItem = this.grid.dataItem(this.grid.select());

            if (apputils.isUndefined(dataItem) || dataItem === null) {
                return;
            }

            if (dataItem.RowIndex > -1){
                this.selectedRowIndex = dataItem.RowIndex;
            }
            else {
                throw "RowIndex not defined";
            }
        },

        //TODO; make generic
        selectCheckbox: function(ev) {
            
            var checked = this.checked,
                row = $(this).closest("tr"),
                dataItem = this.grid.dataItem(row);

            if (checked) {
                checkedIds[dataItem.RowIndex] = checked;

                //-select the row
                row.addClass("k-state-selected");

                var checkHeader = true;

                $.each(this.grid.items(), function(index, item) {
					if (!($(item).hasClass("k-state-selected"))) {
						checkHeader = false;
					}
				});

                $("#header-chb")[0].checked = checkHeader;
            } else {
                delete checkedIds[dataItem.RowIndex];
                //-remove selection
                row.removeClass("k-state-selected");
                $("#header-chb")[0].checked = false;
            }
        },

        initLoad: function() {
            this.load();
            this.initEvents();
        },

        initLoad2: function(gridViewId, businessObj) {
            this.initLoad();
            this.gridViewid = gridViewId;
            this.businessObject = businessObj;
        },

        keydownHandler: function (e, grid) {
            if (grid.select().index() === 0 && e.code === 'ArrowUp') {
                let cell = grid.tbody.find("tr:first td:first");
                grid.current(cell);
                grid.table.focus();
            }
            if ([38, 40].indexOf(e.keyCode) >= 0) {
                setTimeout(function () {
                    grid.select($(`#${grid._cellId}_active_cell`).closest("tr"));
                });
            }
        },

        focusHandler: function (e, grid) {
            const rowIndex = grid.select().index();
            const colIndex = grid._lastCellIndex;

            if (rowIndex === 0 && colIndex <= 0) {
                const cell = grid.tbody.find("tr:first td:first");
                const col = grid.columns[0];

                let editable = grid.options.editable || cell.className !== "non-editable-column";
                if (editable && col.editable && apputils.isFunction(col.editable)) {
                    const rowData = grid.dataItem(grid.select());
                    editable = col.editable(rowData);
                }
                if (editable) {
                    grid.current(cell);
                    grid.editCell(cell);
                }
            }
        },

        /** Bind grid events */
        initEvents: function() {
            var self = this;
            
            this.grid.table.on("click", ".row-checkbox", this.selectCheckbox);

            
            $("#" + this.gridId).on("click", "tbody > tr", function (e) {
                //Handle grid editor icon click to select correct row
                if (e.target.className && e.target.className.startsWith('icon pencil-edit')) {
                    let row = $(this).closest('tr');
                    self.grid.select(row);
                    MessageBus.msg.trigger(self.gridId + self.selectRowChange, { gridId: self.gridId });
                }
                self.selectRow();
            });

            //Not all grids has pager, such as summary grid, need check pager exsit
            if (this.grid.pager) {
                this.grid.pager.bind('change', function (e) {
                    const grid = self.grid;
                    const ds = grid.dataSource;

                    //const options = { data: { rows: ds.options.data, pageSize: ds.pageSize(), page: ds.page()} };
                    //self.getLazyData(options);
                    
                    const cell = grid.tbody.find("tr").eq(0).find("td").eq(0);
                    self.selectedRowIndex = (ds.page() - 1) * ds.pageSize();
                    grid.select('tr:eq(0)');
                    grid.current(cell);
                    grid.editCell(cell);
                });
            }

            $('#header-chb').on("change", function(ev) {
                var checked = ev.target.checked;
                $('.row-checkbox').each(function(idx, item) {
                    if (checked) {
                        if (!($(item).closest('tr').is('.k-state-selected'))) {
                            $(item).click();
                        }
                    } else {
                        if ($(item).closest('tr').is('.k-state-selected')) {
                            $(item).click();
                        }
                    }
                });
            });

            //Support keyboard up/down pageup/down selection, [38, 40] is key arrow code
            self.grid.table.on("keydown", e => self.keydownHandler(e, self.grid));

            //Support keyboard navigation focus and edit cell
            self.grid.table.bind("focus", e=> self.focusHandler(e,self.grid));
        },

        /**
         * Set focus to specifc cell
         * @param {any} grid Kendo grid
         * @param {int} colIndex Column index
         */
        setFocus: function (grid, colIndex) {
            const rowIndex = grid.select().index();
            if (rowIndex > -1 && typeof colIndex !== 'undefined') {
                const cell = grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex);
                grid.current(cell);
                grid.editCell(cell);
            }
        },

        getServerPaging: function () {
            return false;
        },

        recordIdChanged: function () {
            
        },

        ///source: https://docs.telerik.com/kendo-ui/knowledge-base/grid-select-row-on-different-page
        ///therefore doesn't follow 'small' function coding style

        /**
         * Select grid row, support multiple pages
         * @param {int} currentRow Current row
         * @param {int} colIndex Column index
         */
        selectGridRow: function (currentRow, colIndex) {

            let grid = this.getGrid(); //$("#" + this.gridId).data("kendoGrid");
            if (!grid) return;

            let idField = "RowIndex";
            var dataSource = grid.dataSource;
            var filters = dataSource.filter() || {};
            var sort = dataSource.sort() || {};
            var models = dataSource.data();

            // We are using a Query object to get a sorted and filtered representation of the data, without paging applied, so we can search for the row on all pages
            var query = new kendo.data.Query(models);
            var rowNum = 0;
            var modelToSelect = null;

            models = query.filter(filters).sort(sort).data;

            // Now that we have an accurate representation of data, let's get the item position
            for (var i = 0; i < models.length; ++i) {
                var model = models[i];
                if (model[idField] === currentRow) {
                    modelToSelect = model;
                    rowNum = i;
                    break;
                }
            }

            if (modelToSelect === null) {
                this.setFocus(grid, colIndex)
                return;
            }
            // If you have persistSelection = true and want to clear all existing selections first, uncomment the next line
            // grid._selectedIds = {};

            // Now go to the page holding the record and select the row
            var currentPageSize = grid.dataSource.pageSize();
            var pageWithRow = parseInt((rowNum / currentPageSize)) + 1; // pages are one-based
            //grid.dataSource.page(pageWithRow);
            grid.dataSource.query({ page: pageWithRow, pageSize: currentPageSize, serverPaging: this.getServerPaging() });
            
            var row = grid.element.find("tr[data-uid='" + modelToSelect.uid + "']");
            if (row.length > 0) {
                
                grid.select(row);

                let dataItem = grid.dataItem(grid.select());
                this.selectedRowIndex = dataItem.RowIndex;

                // Scroll to the item to ensure it is visible
                grid.content.scrollTop(grid.select().position().top);
                $(grid.tbody[0].firstChild.firstElementChild).removeClass("k-state-focused"); //Bug in kendo - remove the previous focus before assigning new one
                $(grid.select()[0].firstChild).addClass("k-state-focused");
            }

            //this.updateGridData(grid.dataSource.transport.data, false);

            // Add line and delete line set focus
            this.setFocus(grid, colIndex)

            //[RC] - Aug 30 - commented out for now since its breaking row selection
            //this.setPage();
            
            //AT-74570
            MessageBus.msg.trigger(this.gridId + "selectGridRowCompleted", {});

            return row;
        },

        /**
         * Move focus to cell by row index and column name
         * @param {int} rowIndex Row index
         * @param {String} columnName Column name 
         */
        moveToCell: function (rowIndex, columnName) {
            let row = this.selectGridRow(rowIndex);
            if (!row) return;

            let grid = this.getGrid();

            let colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
            this.currentCell = row[0].cells[colIndex];

            grid.current(this.currentCell);
            grid.editCell(this.currentCell);
        },

        /**
         * Move focus to cell and set value
         * @param {int} rowIndex row index
         * @param {String} columnName column name
         * @param {any} value value
         */
        moveToCellSetValue: function (rowIndex, columnName, value) {
            let row = this.selectGridRow(rowIndex);

            let colIndex = window.GridPreferencesHelper.getGridColumnIndex(this.getGrid(), columnName);
            this.currentCell = row[0].cells[colIndex];

            this.getGrid().current(this.currentCell);
            this.getGrid().editCell(this.currentCell);

            let selectedItem = this.getGrid().dataItem(this.getGrid().select());

            if (selectedItem) {
                selectedItem.set(columnName, value);
            }
        },

        /**
         * Move focus to next editable cell
         * @param {any} rowIndex Row index
         * @param {any} colName Column name
         * @param {any} newValue value
         */
        moveToNextCell: function (rowIndex, colName, newValue) {
            this.setGridCellAndMoveNext(this.getGrid(), rowIndex, colName, newValue);
        },

        /** re set current selection as undefined */
        resetPropagation: function () {
            this.currentRow = undefined;
            this.currentColumnName = undefined;
            this.currentValue = undefined;
            this.currentColIndex = undefined;
        },

        /** Get grid's kendo grid */
        getGrid: function () {
            this.grid = this.grid || $("#" + this.gridId).data("kendoGrid");
            
            return this.grid;
        },

        /**
         * set select cell column model data value
         * @param {String} columnName
         * @param {any} newValue
         */
        setColumnValue: function (columnName, newValue, rowIndex) {
            if (!apputils.isUndefined(rowIndex)) {
                this.selectGridRow(rowIndex);
            }
            let selectedItem = this.getGrid().dataItem(this.getGrid().select());
            if (selectedItem) {
                selectedItem.set(columnName, newValue);
            }
        },

        /**
         * set selected row model data value
         * @param {array} newValueObj
         * @param {int} currentRow Current editing row
         */
        setRowValues: function (newValueObj, currentRow, refresh = true) {
            this.selectGridRow(currentRow);

            let selectedItem = this.getGrid().dataItem(this.getGrid().select());
            
            //mostly Finder and messages popups looses grid row selection
            if (!selectedItem) {
                const row = this.selectGridRow(this.selectedRowIndex);
                if (!row) return;
            }

            selectedItem = this.getGrid().dataItem(this.getGrid().select());
            // Changing a value using set will trigger the data source change event and the whole grid will be rebound.
            // If you want to perform lots of updates you can avoid the set method and call the grid's refresh method when the update is finished.
            if (selectedItem) {
                newValueObj.forEach(data => selectedItem[data.columnName] = data.value);
                if (refresh) {
                    this.getGrid().refresh();
                    this.selectGridRow(currentRow);
                }
            }
        },

        /**
         * stop propagation
         * @param {int} rowIndex Current select row index
         * @param {String} columnName Current select cell column name
         * @param {any} newValue Value
         */
        stopPropagation: function (rowIndex, columnName, newValue) {
            this.currentRow = rowIndex;
            this.currentColumnName = columnName;
            this.currentValue = newValue;

            let selectedItem = this.getGrid().dataItem(this.getGrid().select());
            if (selectedItem) {
                selectedItem.set(columnName, selectedItem[columnName]);
            }
        },

        /**
         * Move to previous editable grid cell
         * @param {int} rowIndex Select row index
         * @param {String} columnName Select cell column name
         * @param {any} newValue Value
         */
        moveToPrevCell: function (rowIndex, columnName, newValue) {
            let row = this.selectGridRow(rowIndex);
            let colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
            this.currentCell = row[0].cells[colIndex];

            grid.editCell(this.currentCell);

            let selectedItem = grid.dataItem(grid.select());
            if (selectedItem) {
                selectedItem.set(columnName, newValue);
            }

            do {
                colIndex--;
                if (colIndex < 1) break;
                this.currentCell = row[0].cells[colIndex];
            } while (this.currentCell.className === "non-editable-column" || grid.columns[colIndex].hidden);

            //found the correct cell now reset column indexes
            this.resetPropagation();

            grid.current(this.currentCell);
            grid.editCell(this.currentCell);
        },

        /**
         * Set grid cell and move to next editable cell
         * @param {any} grid Kendo grid
         * @param {int} rowIndex Select row index
         * @param {String} columnName Select column name
         * @param {any} newValue Value
         */
        setGridCellAndMoveNext: function (grid, rowIndex, columnName, newValue) {
            
            let row = this.selectGridRow(rowIndex);
            if (!row) return;

            let colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
            this.currentCell = row[0].cells[colIndex];

            grid.current(this.currentCell);
            grid.editCell(this.currentCell);

            let selectedItem = grid.dataItem(grid.select());
            if (selectedItem && newValue) {
                selectedItem.set(columnName, newValue);
                row = this.selectGridRow(rowIndex);
            }

            if (apputils.isUndefined(newValue)) {
                newValue = selectedItem[columnName];
            }

            let editable = true;
            let col = {};
            do {
                editable = true;
                colIndex++;
                if (colIndex >= row[0].cells.length) break;
                this.currentCell = row[0].cells[colIndex];
                col = grid.columns[colIndex];
                if (col.editable && apputils.isFunction(col.editable)) {
                    const rowData = grid.dataItem(grid.select());
                    editable = col.editable(rowData);
                }
            } while (!editable || col.hidden || this.currentCell.className === "non-editable-column");

            //found the correct cell now reset column indexes
            this.resetPropagation();

            this.currentCell = row[0].cells[colIndex];
            if (!this.currentCell && rowIndex < grid._data.length-1) {
                row = this.selectGridRow(rowIndex + 1);
                colIndex = grid.columns.findIndex(c => !c.hidden);
                this.currentCell = row[0].cells[colIndex];
            }
           // Set the currently focused item to the next/previous editable cell.
            grid.current(this.currentCell);
            grid.editCell(this.currentCell);
        },

        /**
         * Add optional field grid line
         * @param {any} optColl Optional field view JS collection object
         * @param {any} entityColl Header/detail view JS collection object
         * @param {any} detailRowIndex Select detail row index. for head optional field, value is 0
         */
        addOptionalFieldLine: function (optColl, entityColl, detailRowIndex = 0) {
            const gridData = this.grid._data;
            const rows = [...gridData];
            const colIndex = window.GridPreferencesHelper.getGridColumnIndex(this.grid, 'OPTFIELD');
            const ds = this.grid.dataSource;

            let rowIndex = this.selectedRowIndex > 0 ? this.selectedRowIndex : 0; //(ds.page() - 1) * ds.pageSize() + this.grid.select().index();
            //Pager not working properly, when paging, the selectedRow index is 0
            if (rowIndex === 0 && ds.page() > 1) {
                rowIndex = (ds.page() - 1) * ds.pageSize();
            }

            if (gridData.filter(r => !r.OPTFIELD).length > 0) {
                return gridData.length;
            }

            optColl.addEmptyDetailLineToGrid(rowIndex, this.gridViewid);
            let data = entityColl.getDataForGridFromSelectedRow(this.gridViewid, detailRowIndex);
            data.forEach(r => {
                let rs = rows.filter(i => i.OPTFIELD === r.OPTFIELD);
                if (rs.length > 0) {
                    r.FDESC = rs[0].FDESC;
                    r.SWSET = rs[0].SWSET;
                    r.VALUE = rs[0].VALUE instanceof Date ? kendo.toString(new Date(rs[0].VALUE), "d") : rs[0].VALUE;
                    r.VDESC = rs[0].VDESC;
                    r.TYPE = rs[0].TYPE;
                    r.DECIMALS = rs[0].DECIMALS;
                }
            });

            this.updateGridData(data, false);
            this.selectGridRow(rowIndex + 1, colIndex);

            entityColl.rows[detailRowIndex].setReadOnlyFieldData('VALUES', this.grid._data.length.toString());

            return rowIndex + 1;
        },

        /**
         * Delete optional field grid line
         * @param {any} optColl Optional field view JS collection object
         * @param {any} entityColl Header/detail view JS collection object
         * @param {any} detailRowIndex Select detail row index. for head optional field, value is 0
         */
        deleteOptionalFieldLine: function (optColl, entityColl, detailRowIndex = 0) {
            const viewId = this.gridViewid;
            const colIndex = window.GridPreferencesHelper.getGridColumnIndex(this.grid, 'VALUE');
            const selIndex = this.grid.select().index();
            let rowIndex = this.selectedRowIndex;

            if (selIndex > -1) {
                let rowData = this.grid.dataItem(this.grid.select());
                let optValue = rowData.OPTFIELD;

                optColl.rows.every( r => {
                    if (r.getFieldValue('OPTFIELD') === optValue) {
                        rowIndex = r.getFieldValue('RowIndex');
                        return false;
                    }
                    return true;
                });

                optColl.deleteLineFromGrid(rowIndex, viewId);
                let data = entityColl.getDataForGridFromSelectedRow(this.gridViewid, detailRowIndex);
                //Update the data as input value not in collection object yet for auto inserted new record in details optional field
                data.forEach(r => r.VALUE = this.grid._data.filter(i => i.OPTFIELD === r.OPTFIELD)[0].VALUE);
                this.updateGridData(data, false);

                entityColl.rows[detailRowIndex].setReadOnlyFieldData('VALUES', this.grid._data.length.toString());

                //Select the row not marked deleted.
                let index = 0;
                /*if (rowIndex > 0) {
                    index = rowIndex - 1;
                    while (index > -1 && optColl.rows[index].CRUDReason === CRUDReasons.Deleting) {
                        index--;
                    }
                }*/

                if (optColl.rows.length > 0) {
                    let direction = rowIndex > 0? -1: 1;
                    index = rowIndex + direction;
                    while (index > -1 && optColl.rows[index] && optColl.rows[index].CRUDReason === CRUDReasons.Deleting) {
                        index += direction;
                    }
                }

                this.selectGridRow(index, colIndex);
            }
        },

        getGridEntity: function (businessObject) {

            if (businessObject.viewid === this.gridViewid) {
                return businessObject;
            }

            return businessObject.rows[0].findCollectionObj(this.gridViewid);
        },

        getBulkNavigationFilter: function () {
            const self = this;
            const filterFn = (viewid) => {
                if (viewid === self.businessObject.viewid) {

                    const idx = self.businessObject.rows.length - 1;
                    return self.businessObject.rows[idx].getBulkNavigationFilter();
                }

                const gridEntityColl = self.getGridEntity(self.businessObject); 
                if (viewid === this.gridViewid && self.businessObject.rows.length > 0 && gridEntityColl) {
                    return gridEntityColl.getBulkNavigationFilter();
                }

                return "";
            }

            return filterFn;
        },

        /**
         * update grid data after lazy fetch
         * @param {any} data data array to update the grid
         */
        updateGridLazyData: function (data) {

            if (this.stopLazyLoad) {
                $(".k-link.k-pager-nav.k-pager-last").prop("disabled", false).removeClass("k-state-disabled");
                this.stopLazyLoad = false;
                this.lazyLoadRunning = false;
                return;
            }

            this.grid.dataSource.options.data = data;
            this.grid.dataSource.transport.data = data;
            this.grid.dataSource.read();
            
            if (data.length > 0 && this.getTotalRowCount() > data.length) {
                
                this.fetchLazyData();
            } else {
                $(".k-link.k-pager-nav.k-pager-last").prop("disabled", false).removeClass("k-state-disabled");

                this.lazyLoadRunning = false;

            }
        },

        /**
         * fetch grid lazy data
         * @param {any} options Options
         */
        data: [],
        fetchLazyData: function (options) {

            this.lazyLoadRunning = true;

            let self = this;
            //const data = self.businessObject.getDataForGrid(this.gridViewid); 
            const data = this.data;

            const filterFn = self.getBulkNavigationFilter();

            const newEntityColl = new self.businessObject.constructor(); 

            const query = newEntityColl.generateGetRoot(filterFn);

            const Ok = newEntityColl._executeXSearch2(query);

            if (apputils.isUndefined(Ok)) {
                self.updateGridData(data, false);
                self.lazyLoadRunning = false;
                return;
            }

            Ok.then((result, status, xhr) => {

                if (xhr.hasError || self.stopLazyLoad) {
                    this.stopLazyLoad = false;
                    self.lazyLoadRunning = false;
                    return;
                }

                const curRowIndex = data[data.length - 1].RowIndex;
                const gridEntityColl = self.getGridEntity(self.businessObject); 
                const newGridEntityColl = self.getGridEntity(newEntityColl); 

                newGridEntityColl.adjustTotalRowCountAfterLazyRetrieve(self.getTotalRowCount());

                gridEntityColl.rows = gridEntityColl.rows.concat(newGridEntityColl.rows);

                gridEntityColl.adjustLineNumbers(curRowIndex);

                gridEntityColl.updateRowIndex("0", curRowIndex);

                if (typeof PMCommonFindersObj === 'object') PMCommonFindersObj.gridsCollectionObj = self.businessObject.rows[0].allCollectionObj[this.gridEntityObjName];

                //const fetchedData = self.businessObject.getDataForGrid(this.gridViewid);
                this.data = self.businessObject.getDataForGrid(this.gridViewid);

                //self.updateGridLazyData(fetchedData);
                self.updateGridLazyData(this.data);

            });
        },

        /**
        * Rotates a range of columns in a grid starting from a specified column to the last column in the range.
        * @param {string} startingColumnName - The name of the column from which the rotation starts.
        * @param {Array<string>} namesOfColumnsToReorder - An array of column names to rotate. 
        * These columns must appear in sequence in the grid
        * @returns {void}
        */
        rotateColumns: function (startingColumnName, namesOfColumnsToReorder) {
            const grid = this.getGrid();

            const startingColIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, startingColumnName);
            
            let lowerBoundIndex = 2000; //<-- we shouldn't have this many columns, if needed can use max grid column.
            let upperBoundIndex = -1;

            let columns = [];
            namesOfColumnsToReorder.forEach((columnName) => {
                const colIndex = window.GridPreferencesHelper.getGridColumnIndex(grid, columnName);
                if (colIndex < lowerBoundIndex) {
                    lowerBoundIndex = colIndex;
                }

                if (colIndex > upperBoundIndex) {
                    upperBoundIndex = colIndex;
                }

                columns[colIndex] = grid.columns[colIndex];
            });

            let rotateFromPostion = startingColIndex;
            let rotateToPostion = 0;
            do {

                //move grid column to this index
                grid.reorderColumn(lowerBoundIndex + rotateToPostion, columns[rotateFromPostion]);
                rotateFromPostion++;
                rotateToPostion++;

                if (rotateFromPostion > upperBoundIndex) {
                    rotateFromPostion = lowerBoundIndex;
                }

                
            } while (rotateFromPostion !== startingColIndex)

        }
                
    };

    this.baseGrid = helpers.View.extend(defaultGrid);

}).call(this);