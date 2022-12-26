/* Copyright (c) 2021 Sage.CA.SBS.ERP.Sage300  All rights reserved. */

// The following commented line to enables TypeScript static type checking
// Disable this by removing the line.
// @ts-check

"use strict";

/**
 * @class 
 * @name AccpacGridHelper
 * @description Class that wraps common Kendo grid functionality 
 */
var AccpacGridHelper = class {

    /**
     * @constructor
     * @description The class constructor
     * @namespace AccpacGridHelper
     * @public
     *
     * @param {string} gridName The grid name
     */
    constructor(gridName) {
        this._gridName = gridName;
    }

    /**
     * @method
     * @name gridName
     * @description Get the grid name
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {string} The grid name
     */
    gridName() {
        return this._gridName;
    }

    /**
     * @method
     * @name getGrid
     * @description Get the grid reference
     * @namespace AccpacGridHelper
     * @public
     * 
     * @returns {object} The grid reference
     */
    getGrid() {
        return $('#' + this._gridName).data('kendoGrid');
    }

    //hasIncompleteRow() {
    //    const grid = this.getGrid();
    //    const ds = grid.dataSource;
    //    const gridData = ds.data();

    //    let incompleteRowIndex = -1;
    //    let currentIndex = 0;
    //    for (let row of gridData) {
    //        const emptyContract = row.CONTRACT === '';
    //        const emptyProject = row.PROJECT === '';
    //        const emptyCategory = row.CATEGORY === '';
    //        const emptyItemNumber = row.RESOURCE === '';
    //        const emptyLocation = row.LOCATION === '';
    //        if (emptyContract || emptyProject || emptyCategory || emptyItemNumber || emptyLocation) {
    //            // Incomplete row found. Let's bail.
    //            incompleteRowIndex = currentIndex;
    //            break;
    //        }
    //        currentIndex++;
    //    }

    //    return {
    //        HasIncompleteRow: incompleteRowIndex > -1 ? true : false,
    //        RowIndex: incompleteRowIndex
    //    };
    //}

    /**
     * @method
     * @name selectGridRowByIndex
     * @description Sets the selected row by index
     * @namespace AccpacGridHelper
     * @public
     */
    selectGridRowByIndex(index) {
        const grid = this.getGrid();
        if (grid) {
            grid.select(grid.tbody.find(">tr:eq(" + index + ")"));
        }
    }

    /**
     * @method
     * @name selectedRow
     * @description Gets the grids selected row
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {object} The selected row or empty object
     */
    selectedRow() {
        const grid = this.getGrid();
        if (grid) {
            return grid.select();
        }
        return {};
    }

    /**
     * @method
     * @name selectedItem
     * @description Gets the grids selected item
     * @namespace AccpacGridHelper
     * @public
     * 
     * @returns {object} The selected item or empty object
     */
    selectedItem() {
        const grid = this.getGrid();
        if (grid) {
            return grid.dataItem(this.selectedRow());
        }
        return {};
    }

    /**
     * @method
     * @name refresh
     * @description Refreshes the grid and unsets it's dirty flag
     * @namespace AccpacGridHelper
     * @public
     */
    refresh() {
        const gridName = this.gridName();
        sg.viewList.refresh(gridName);
        this.setClean();
    }

    /**
     * @method
     * @name refreshCurrentRowByFieldName
     * @description Refreshes the field in the current row by fieldName
     * @namespace AccpacGridHelper
     * @public
     *
     * @param {string} fieldName The field name
     */
    refreshCurrentRowByFieldName(fieldName) {
        const gridName = this.gridName();
        sg.viewList.refreshCurrentRow(gridName, fieldName);
    }

    /**
     * @method
     * @name readOnly
     * @description Determines the grids currently selected row index
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {object} Currently selected row index or -1
     */
    readOnly(readOnly = true) {
        sg.viewList.readOnly(this.gridName(), readOnly);
    }

    /**
     * @method
     * @name currentRowIndex
     * @description Determines the grids currently selected row index
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {object} Currently selected row index or -1
     */
    currentRowIndex() {
        const grid = this.getGrid();
        if (grid) {
            return grid.select().index();
        }
        return -1
    }

    /**
     * @method
     * @name selectedRowData
     * @description Determines the grids currently selected row data
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {object} Array of the currently selected data or empty array
     */
    selectedRowData() {
        const grid = this.getGrid();
        if (grid) {
            return sg.utls.kndoUI.getSelectedRowData(grid);
        }
        return [];
    }

    /**
     * @method
     * @name recordCount
     * @description Determines the grids current record count
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {number} The grids current record count or 0
     */
    recordCount() {
        const grid = this.getGrid();
        if (grid) {
            var total = grid.dataSource.total();
            return total;
        }
        return 0;
    }

    /**
     * @method
     * @name pageSize
     * @description Determines the grids current page size
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {number} The grids current page size or -1
     */
    pageSize() {
        const grid = this.getGrid();
        if (grid) {
            let pageSize = grid.dataSource.pageSize();
            return pageSize;
        }
        return -1;
    }

    /**
     * @method
     * @name currentPage
     * @description Determines the grids currently displayed page
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {number} The grids current page or -1
     */
    currentPage() {
        const grid = this.getGrid();
        if (grid) {
            return grid.dataSource.page();
        }
        return -1;
    }

    /**
     * @method
     * @name displayLineNumber
     * @description Determines the grids current displayed line number
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {number} The current display line number
     */
    displayLineNumber() {
        const grid = this.getGrid();
        if (grid) {
            const rowIndex = grid.select().index();
            const pageSize = this.pageSize();
            const currentPage = this.currentPage();

            const displayLineNumber = rowIndex + 1 + pageSize * (currentPage) - 1;
            return displayLineNumber;
        }
        return -1;
    }

    /**
     * @method
     * @name isDirty
     * @description Determines if the grid is dirty
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {boolean} true = Grid is dirty | false = Grid is not dirty
     */
    isDirty() {
        return sg.viewList.dirty(this.gridName());
    }

    /**
     * @method
     * @name setDirty
     * @description Sets the grids dirty flag
     * @namespace AccpacGridHelper
     * @public
     * 
     */
    setDirty() {
        sg.viewList.dirty(this.gridName(), true);
    }

    /**
     * @method
     * @name setClean
     * @description Unsets the grids dirty flag
     * @namespace AccpacGridHelper
     * @public
     * 
     */
    setClean() {
        sg.viewList.dirty(this.gridName(), false);
    }

    /**
     * @method
     * @name isEmpty
     * @description Determines if the grid is empty
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {boolean} true = Grid is empty | false = Grid is not empty
     */
    isEmpty() {
        return sg.viewList.isEmpty(this.gridName());
    }

    /**
     * @method
     * @name getData
     * @description Get the grid's data, null if grid doesn't exist
     * @namespace AccpacGridHelper
     * @public
     *
     * @returns {object} The grids data
     */
    getData() {
        const grid = this.getGrid();
        if (grid) {
            return grid.dataSource.data();
        }
        return null;
    }

    /**
     * @method
     * @name commit
     * @description Commit the grid data
     * @namespace AccpacGridHelper
     * @public
     *
     * @param {object} successCallback The success callback
     */
    commit(successCallback = null) {
        sg.viewList.commit(this.gridName(), successCallback);
    }

    /**
     * @method
     * @name firstPage
     * @description Go to the first page of a grid
     * @namespace AccpacGridHelper
     * @public
     */
    firstPage() {
        const grid = this.getGrid();
        if (grid) {
            grid.dataSource.page(1);
        }
    }

    /**
     * @method
     * @name lastPage
     * @description Go to the last page of a grid
     * @namespace AccpacGridHelper
     * @public
     */
    lastPage() {
        const grid = this.getGrid();
        if (grid) {
            const dataSource = grid.dataSource;
            let totalPages = dataSource.totalPages();
            dataSource.page(totalPages);
        }
    }

    /**
     * @method
     * @name previousPage
     * @description Go to the previous page of a grid
     * @namespace AccpacGridHelper
     * @public
     */
    previousPage() {
        const grid = this.getGrid();
        if (grid) {
            let page = grid.dataSource.page();
            page--;
            grid.dataSource.page(page);
        }
    }

    /**
     * @method
     * @name nextPage
     * @description Go to the next page of a grid
     * @namespace AccpacGridHelper
     * @public
     */
    nextPage() {
        const grid = this.getGrid();
        if (grid) {
            let page = grid.dataSource.page();
            page++;
            grid.dataSource.page(page);
        }
    }
}

