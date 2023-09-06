(function () {
    'use strict';

    QUnit.config.autostart = false;
    QUnit.config.reorder = false;

    let domainLibrary = {
        id: '',
        testModule: '',
        testCount: 0,

        /**
         * Trigger a click event
         * @param {String} filter The id to identify the html element to interact with
         */
        clickElement: function (filter) {
            $(filter).trigger("click");
        },

         /**
         * Trigger a double click event
         * @param {String} filter The id to identify the html element to interact with
         */
        dblClickElement: function (filter) {
            $(filter).dblclick();
        },

        /**
         * Type into an input
         * @param {String} filter The id to identify the html element to interact with
         * @param {String} value The string to type into the input
         */
        typeInInputAndChange: function (filter, value) {
            $(filter).val(value).trigger("change");
        },

        typeInInputAndDontChange: function (filter, value) {
            $(filter).val(value);
        },

        clickElementWithCallback(filter, cb) {
            $(filter).trigger("click");
        },

        /**
         * Close the popup window
         * @param {String} closestDivId The id of the closest element that is after the <div> of the target popup window
         * @param {bool} hasMaximize True if the popup window has a maximize button, false otherwise
         */
        closePopupWindow: function (closestDivId, hasMaximize = true) {
            let close;
            if (hasMaximize) {
                close = $(closestDivId).prev("div").find("div").eq(0).find("a").eq(1);
            }
            else {
                close = $(closestDivId).prev("div").find("div").eq(0).find("a").eq(0);
            }
            close.trigger("click");
        },

        /**
         * Checks for delete confirmation
         * @param {object} assert The assert object to pass in
         * @param {bool} exists true if delete confirmation exists, false otherwise
         */
        hasDeleteConfirmation: function (assert, exists) {

            let dialogBoxTitle = $("div#deleteConfirmationParent.k-widget.k-window.k-display-inline-flex.modelBox").find("div#body-text").eq(0);
            if (exists) {
                assert.strictEqual((dialogBoxTitle.length > 0), true, 'deleteConfirmationParent found');
            }
            else {
                assert.strictEqual((dialogBoxTitle.length === 0), true, 'deleteConfirmationParent not found');
            }
        },

        /**
         * Check the content of the delete confirmation modal and either cancel or accept the confirmation
         * @param {object} assert The assert object to pass in
         * @param {String} expectedMessage The expected message to receive in the confirmation modal
         * @param {String} testDescription The description of the test
         * @param {boolean} decision true to accept, false to cancel
         */
        checkAndHandleDeleteConfirmation: function (assert, expectedMessage, testDescription, decision = true) {
            let dialogBoxTitle = $("div#deleteConfirmationParent.k-widget.k-window.k-display-inline-flex.modelBox").find("div#body-text").eq(0);
            if (dialogBoxTitle.length === 0) {
                assert.strictEqual((dialogBoxTitle.length > 0), true, 'deleteConfirmationParent found');
                return;
            }

            this.checkAssert(assert, dialogBoxTitle[0].innerText, expectedMessage, testDescription);
            if (decision) {
                $('#kendoConfirmationAcceptButton').trigger("click");
            }
            else {
                $('#kendoConfirmationCancelButton').trigger("click");
            }
        },

        ignoreDirtyCheck: function () {
            $('#kendoConfirmationAcceptButton').trigger("click");
        },

        /**
         * Check the content of the confirmation modal and either cancel or accept the confirmation
         * @param {object} assert The assert object to pass in
         * @param {String} expectedMessage The expected message to receive in the confirmation modal
         * @param {String} testDescription The description of the test
         * @param {boolean} decision true to accept, false to cancel
         */
        checkAndHandleConfirmation: function (assert, expectedMessage, testDescription, decision = true) {
            let dialogBoxTitle = $("div#confirmationDialog").find("div#body-text").eq(0);
            this.checkAssert(assert, dialogBoxTitle[0].innerText, expectedMessage, testDescription);
            if (decision) {
                $('#kendoConfirmationAcceptButton').trigger("click");
            } else {
                $('#kendoConfirmationCancelButton').trigger("click");
            }
        },

        /**
         * Check the content of the error message and dismiss the error message
         * @param {String} filter The id to identify the html element to interact with
         * @param {object} assert The assert object to pass in
         * @param {String | String[]} expectedMessage The expected message(s) to receive in the confirmation modal
         * @param {String} testDescription The description of the test
         */
        checkAndCloseErrorMessage: function (filter, assert, expectedMessage, testDescription) {
            let errorMessage;

            if (Array.isArray(expectedMessage) && expectedMessage.length > 1) {
                errorMessage = $(filter).find("div.message-control.multiError-msg").find("ul");
                for (let i = 0; i < expectedMessage.length; i++) {
                    this.checkAssert(assert, errorMessage.find("li").eq(i).text(), expectedMessage[i], testDescription);
                }
            } else {
                errorMessage = $(filter).find("div.msg-content");
                this.checkAssert(assert, errorMessage.text(), expectedMessage, testDescription);
            }
            let close = $(filter).find("span.icon.msgCtrl-close");
            close.trigger("click");
        },

        /**
         * Check if there is an error message
         * @param {String} filter The id to identify the html element to interact with
         * @param {object} assert The assert object to pass in
         * @param {boolean} hasError true if an error message is expected false otherwise
         * @param {String} testDescription The description of the test
         */
        checkForErrorMessage: function (filter, assert, hasError, testDescription) {

            let errorMessage = $(filter).find("div.msg-content");
            if (hasError) {
                this.checkAssertNotEqual(assert, errorMessage.text(), "", testDescription);
            } else {
                this.checkAssert(assert, errorMessage.text(), "", testDescription);
            }

            let close = $(filter).find("span.icon.msgCtrl-close");
            close.trigger("click");
        },

        /**
         * 
         * @param {String} filter The id to identify the html element to interact with
         * @param {object} assert The assert object to pass in
         * @param {String} expectedMessage The expected message to receive in the confirmation modal
         * @param {String} testDescription The description of the test
         */
        checkSuccessConfirmation: function (filter, assert, expectedMessage, testDescription) {

            let successMessage = $(filter).find("h3").eq(0);
            this.checkAssert(assert, successMessage.text(), expectedMessage, testDescription);

            let close = $(filter).find("span.icon.msgCtrl-close");
            close.trigger("click");
        },

        /**
         * Dismiss the message without checking
         * @param {String} filter The id to identify the html element to interact with
         */
        dimissMessage: function (filter) {
            let close = $(filter).find("span.icon.msgCtrl-close");
            close.trigger("click")
        },

        /**
         * Change the content of the grid cell
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         * @param {String} value The string to type in the input
         */
        editGridCell: function (grid, row, column, value) {
            baseStaticTestModule.setGridLineFocus(grid, row);
            row = grid.select().index();
            const columnIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);
            let td = grid.tbody.find("tr").eq(row).find("td").eq(columnIndex);

            grid.current(td);
            grid.editCell(td);
            let selectedItem = grid.dataItem(grid.select());

            if (selectedItem) {

                if (column.dataType === "Decimal") {
                    selectedItem.set(column.field, parseFloat(value));
                } else {
                    selectedItem.set(column.field, value);
                    td.find("input").eq(0).trigger("change");
                }

                td.find("input").eq(1).trigger("change");
            }
        },

        /**
         * Change the content of the grid cell that has a search button
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         * @param {String} value The string to type in the input
         */
        editGridSearchCell: function (grid, row, column, value) {
            baseStaticTestModule.setGridLineFocus(grid, row);
            row = grid.select().index();
            const columnIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);
            let td = grid.tbody.find("tr").eq(row).find("td").eq(columnIndex);
                
            grid.current(td);
            grid.editCell(td);
            let selectedItem = grid.dataItem(grid.select());

            if (selectedItem) {

                if (column.dataType === "Decimal") {
                    selectedItem.set(column.field, parseFloat(value));
                    td.find("input").eq(1).trigger("change");
                } else {
                    selectedItem.set(column.field, value);
                    td.find("input").eq(0).trigger("change");
                    if (typeof (td.find("input").eq(1)) !== 'undefined') {
                        td.find("input").eq(1).trigger("change");
                    }
                }
                //$("#" + td.find("input").eq(0)[0].id).trigger("change");
                //selectedItem.trigger("change");
            }
            //td.find("input").eq(0).trigger("change");
        },

        /**
         * Change the content of the grid cell that has a dropdown list
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         * @param {String} text The text to select to the dropdown
         */
        selectGridCell: function (grid, row, column, text) {
            baseStaticTestModule.setGridLineFocus(grid, row);
            row = grid.select().index();
            const columnIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);
            let td = grid.tbody.find("tr").eq(row).find("td").eq(columnIndex);
            grid.current(td);
            grid.editCell(td);
            let selectedItem = grid.dataItem(grid.select());

            if (selectedItem) {
                let dropdownList = td.find("input").eq(0).data("kendoDropDownList");
                dropdownList.text(text);
                td.find("input").eq(0).trigger("change");
                return dropdownList;
            }
        },

        /**
         * Find the column in the selected row and checks if editable
         * @param {object} grid The grid object to work with
         * @param {any} row The 0 based row index/name to interact with
         * @param {any} columnIndex The 0 based column index/name to interact with
         */
        checkGridCellIsEditable: function (grid, row, column, assert, expected, description) {
            let isEditable = false;
            baseStaticTestModule.setGridLineFocus(grid, row);
            const rowIndex = grid.select().index();
            const columnIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let td = grid.tbody.find("tr").eq(rowIndex).find("td").eq(columnIndex);

            grid.current(td);
            const col = grid.columns[columnIndex];
            if (!col.editable) {
                isEditable = true;
            } else if (apputils.isFunction(col.editable)) {
                const rowData = grid.dataItem(grid.select());
                isEditable = col.editable(rowData);
            }

            this.checkAssert(assert, isEditable, expected, description);
            
        },

        /**
         * Open the finder from a grid cell
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         */
        openGridCellFinder: function (grid, row, column) {
            row = grid.select().index();
            const col = qunitStaticLibrary.getGridColumnIndex(grid, column);
            console.log(`col is ${col}`);
            let rowData = baseStaticTestModule.selectGridRow(grid, row);
            let td = rowData[0].cells[col];

            //let td = grid.tbody.find("tr").eq(row).find("td").eq(column);
            grid.current(td);
            //$(td[0]).trigger("click");
            grid.editCell(td);
            let inp1 = grid.tbody.find("tr").eq(row).find("input").eq(0);
            inp1.click();
            console.log("Finder grid.editCell(td) ");
            //td.focus();
            //td.childNodes[0].value = "";
            //setTimeout(() => {
                //td.childNodes[1].click();
                let inp = grid.tbody.find("tr").eq(row).find("input").eq(1);
                console.log("Finder auto click: " + inp.id);
                inp.mousedown();
            //}, 900);
             
        },

        /**
         * Trigger the input change from an active grid cell
         * @param {String} filter The id to identify the html element to interact with
         */
        triggerDetailGridActiveCellChange(filter) {
            let td = $(filter).find("input").eq(0);
            td.trigger("change");
        },

        /**
         * Get the element from a grid cell
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         */
        getElementFromGridCell: function (grid, row, column) {
            //let td = grid.tbody.find("tr").eq(row).find("td").eq(column);
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
            return td;
        },

        /**
         * Check the contents of the grid cell
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         * @param {object} assert The assert object to pass in
         * @param {any} expected The expected value of the cell
         * @param {String} description The description of the test
         */
        checkGridCell: function (grid, row, column, assert, expected, description) {
            baseStaticTestModule.setGridLineFocus(grid, row);
            row = grid.select().index();
            grid.closeCell();
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
            
            this.checkAssert(assert, td.text(), expected, description);
        },

        /**
         * Check the contents of the grid cell does not contain the notExpected
         * @param {object} grid The grid object to work with
         * @param {int} row The 0 based row index to interact with
         * @param {int} column The 0 based column index to interact with
         * @param {object} assert The assert object to pass in
         * @param {any} notExpected The cell should not be equal to this value
         * @param {String} description The description of the test
         */
        checkGridCellNotEqual: function (grid, row, column, assert, notExpected, description) {
            baseStaticTestModule.setGridLineFocus(grid, row);
            row = grid.select().index();
            grid.closeCell();
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);
            let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
            this.checkAssertNotEqual(assert, td.text(), notExpected, description);
        },

        /**
         * Check whether any of the column contains the value expected
         * @param {object} grid
         * @param {String | int} column The column name or 0 based index
         * @param {object} assert The assert object to pass in
         * @param {any} expected The cell should be equal to this value
         * @param {String} description The description of the test
         */
        checkGridColumnContainsExact: function (grid, column, assert, expected, description) {
            let ds = grid.dataSource;
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let found = false;

            for (let row = 0; row < ds.total(); row++) {
                let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
                if (td.text() == expected) {
                    found = true;
                    break;
                }
            }

            this.checkAssert(assert, found, true, description);
        },

        /**
         * Check whether any of the column does not contains the value unexpected
         * @param {object} grid
         * @param {String | int} column The column name or 0 based index
         * @param {object} assert The assert object to pass in
         * @param {any} unexpected The cell should not be equal to this value
         * @param {String} description The description of the test
         */
        checkGridColumnContainsExactNeg: function (grid, column, assert, unexpected, description) {
            let ds = grid.dataSource;
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let found = false;

            for (let row = 0; row < ds.total(); row++) {
                let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
                if (td.text() == unexpected) {
                    found = true;
                    break;
                }
            }

            this.checkAssert(assert, found, false, description);
        },

        /**
          * Check whether any of the column contains the value expected
          * @param {object} grid
          * @param {String | int} column The column name or 0 based index
          * @param {object} assert The assert object to pass in
          * @param {any} expected The cell should be equal to this value
          * @param {String} description The description of the test
          */
        checkGridColumnContains: function (grid, column, assert, expected, description) {
            let ds = grid.dataSource;
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let found = false;

            for (let row = 0; row < ds.total(); row++) {
                let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
                if (td.text().toLowerCase().includes(expected.toLowerCase())) {
                    found = true;
                    break;
                }
            }

            this.checkAssert(assert, found, true, description);
        },

        /**
         * Check whether any of the column does not contains the value unexpected
         * @param {object} grid
         * @param {String | int} column The column name or 0 based index
         * @param {object} assert The assert object to pass in
         * @param {any} unexpected The cell should not be equal to this value
         * @param {String} description The description of the test
         */
        checkGridColumnContainsNeg: function (grid, column, assert, unexpected, description) {
            let ds = grid.dataSource;
            const cellIndex = qunitStaticLibrary.getGridColumnIndex(grid, column);

            let found = false;

            for (let row = 0; row < ds.total(); row++) {
                let td = grid.tbody.find("tr").eq(row).find("td").eq(cellIndex);
                if (td.text().toLowerCase().includes(unexpected.toLowerCase())) {
                    found = true;
                    break;
                }
            }

            this.checkAssert(assert, found, false, description);
        },

        /**
         * Check whether the actual value matches with the provided expected value
         * @param {object} assert The assert object to pass in
         * @param {any} actual The actual value to be checked against
         * @param {any} expected The expected value
         * @param {String} description The description of the test
         */
        checkAssert: function (assert, actual, expected, description) {
            assert.strictEqual(actual, expected, description);
        },

        /**
         * Check whether the actual value does not match with the provided expected value
         * @param {object} assert The assert object to pass in
         * @param {any} actual The actual value to be checked against
         * @param {any} notExpected The value should not be equal to this
         * @param {String} description The description of the test
         */
        checkAssertNotEqual: function (assert, actual, notExpected, description) {
            assert.notEqual(actual, notExpected, description);
        },

        checkAssertDeepEqual: function (assert, actual, expected, description) {
            assert.deepEqual(actual, expected, description);
        },

        /**
         * Set the value of a drop down menu
         * @param {String} filter The id to identify the html element to interact with
         * @param {String} itemValue The item value to select
         */
        setDropdownValue: function (filter, itemValue) {
            let dropdownList = $(filter).data("kendoDropDownList");
            dropdownList.select((dataItem) => {
                return dataItem.display === itemValue;
            });
            $(filter).trigger("change");
            return dropdownList;
        },

        /**
         * Set the value of the drop down menu of a finder
         * @param {String} filter The id to identify the html element to interact with
         * @param {String} item The item text to select
         */
        setDropdownText: function (filter, item) {
            let dropdownList = $(filter).data("kendoDropDownList");
            dropdownList.text(item);
            $(filter).trigger("change");
            return dropdownList;
        },

        /**
         * Get the text value of the drop down
         * @param {String} filter The id to identify the html element to interact with
         */
        getDropdownText: function (filter) {
            const dropdownList = $(filter).data("kendoDropDownList");
            return dropdownList.text();
            
        },

        /**
         * Check all dropdown display texts are present
         * @param {String} filter The id to identify the html element to interact with
         * @param {Array} texts a list of expected display texts
         */
        checkDropdownDisplayText: function (filter, texts) {
            const dropdownList = $(filter).data("kendoDropDownList");

            //if (texts.length !== dropdownList.dataSource.options.data) {
            //    return false;
            //}

            let values = [];

            for (let index in dropdownList.dataSource.options.data) {
                values.push(dropdownList.dataSource.options.data[index].display);
            }

            let contains = false;
            texts.forEach(text => contains = values.includes(text));

            return contains;

            //return _.isEqual(values, texts);
        },

        /**
         * Get the item from a tree given a traverse path starting from the root
         * @param {String} filter The id to identify the html element to interact with
         * @param {String[]} traversePath The traverse path of the tree starting from the root
         *                                0 means 1st child, 1 means 2nd child, 2 means 3rd child, etc...
         *                                The traverse path is comma delimited.
         */
        findTreeItem: function (filter, traversePath) {
            let item = $(filter).children("ul").eq(0).children("li").eq(0);
            jQuery.each(traversePath, function (i, val) {
                item = item.children("ul").eq(0).children("li").eq(val);
            });
            item = item.find("span.k-in").eq(0);
            return item;
        },

        /**
         * Return the column index of a grid
         * @param {any} grid The grid to access
         * @param {any} field The column field
         */
        getGridColumnIndex: function (grid, field) {
            if (apputils.isNumber(field)) {
                return field;
            }

            if (apputils.isObject(field)) {
                return window.GridPreferencesHelper.getGridColumnIndex(grid, field.field);
            }

            return window.GridPreferencesHelper.getGridColumnIndex(grid, field);
             
        },

        /**
         * Return the column name of a grid
         * @param {any} grid The grid to access
         * @param {any} field The column field
         */
        getGridColumnName: function (grid, field) {

            if (apputils.isNumber(field)) {
                return grid.columns[field].name;
            }

            if (apputils.isObject(field)) {
                return field.title || field.name;
            }

            return field;

        },

        /**
         * Return all the dropdown options of a dropdown list
         * @param {any} id the dropdown list id
         */
        getAllDropdownOptions: function (id) {

            let options = [];

            let ddl = $(id)[0];
            for (let i = 0; i < ddl.length; i++) {
                options.push(ddl.options[i].text);
            }

            return options;
        },

        /**
         * Checks console errors for any JavaScript runtime error encountered. Logs it and resets the error
         * @param {any} assert qunit assert object
         */
        checkJSError: function (assert) {
            if (console.errors.length > 0) {
                assert.equal(console.errors.length, 0, "JavaScript runtime error encountered: " + console.errors[0]);
                console.errors.length = 0;
            }
        },

    }

    this.qunitStaticLibrary = helpers.View.extend({}, domainLibrary);

}).call(this);