(function () {
    'use strict';

    QUnit.config.autostart = false;
	QUnit.config.reorder = false;

//    var registeredTestModules = [];
    let idleQ = [];
    
    let domainTestModule = {
        id: '',
        testModule: '',
        testCount: 0,
        notStarted: true,
        steps: [],
        qLib: qunitStaticLibrary,
        assert: undefined,
        testCollection: [],
        testName: "",

        /*
        inputDataViaServerMsg: function(dataRows){
            this.waitForServerCallsToComplete(()=>{
                console.log("inputDataViaServerMsg proceeding after idle");

                apputils.each(dataRows, (data)=>{
                    let msg = data.viewid + data.rowIndex + data.field;

                    MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.value, msg + apputils.EventMsgTags.svrUpdate);
                });
            });
        },

        awaitOnServerEvents: function(callback, ctx){

            console.log("awaitOnServerEvents started for " + ctx);

            Keeler.listenTo(MessageBus.msg, "queuedUpdatesOnupdateMappedFieldsComplete", callback);
        },

        doneWaitingOnServerEvents: function(callback, msg){

            Keeler.stopListening(MessageBus.msg, msg, callback);
            
        },
        */

        selectGridRow: function (grid, currentRow) {

            //let grid = this.getGrid(); //$("#" + this.gridId).data("kendoGrid");
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
                //this.setFocus(grid, colIndex)
                return;
            }
            // If you have persistSelection = true and want to clear all existing selections first, uncomment the next line
            // grid._selectedIds = {};

            // Now go to the page holding the record and select the row
            var currentPageSize = grid.dataSource.pageSize();
            var pageWithRow = parseInt((rowNum / currentPageSize)) + 1; // pages are one-based
            //grid.dataSource.page(pageWithRow);
            grid.dataSource.query({ page: pageWithRow, pageSize: currentPageSize, serverPaging: false });

            var row = grid.element.find("tr[data-uid='" + modelToSelect.uid + "']");
            if (row.length > 0) {

                grid.select(row);

                let dataItem = grid.dataItem(grid.select());
                //this.selectedRowIndex = dataItem.RowIndex;

                // Scroll to the item to ensure it is visible
                grid.content.scrollTop(grid.select().position().top);
                $(grid.tbody[0].firstChild.firstElementChild).removeClass("k-state-focused"); //Bug in kendo - remove the previous focus before assigning new one
                $(grid.select()[0].firstChild).addClass("k-state-focused");
            }

            // Add line and delete line set focus
            //this.setFocus(grid, colIndex)

            return row;
        },

        /**
         * Set focus on the line of the provided grid
         * @param {object} grid The grid object to work with
         * @param {int} lineIndex The 0 based line index to interact with
         */
        setGridLineFocus: function (grid, lineIndex) {

            const currentPageSize = grid.dataSource.pageSize();

            if (lineIndex >= currentPageSize) {
                //lineIndex = lineIndex > currentPageSize ? lineIndex - currentPageSize - 1 : lineIndex;

                this.selectGridRow(grid, lineIndex);
            } else {
                let tr = grid.tbody.find("tr").eq(lineIndex);
                grid.select(tr);
                grid.tbody.find("tr").eq(lineIndex).find("td").eq(0).trigger("click");
            }

            /*
            let tr = grid.tbody.find("tr").eq(lineIndex);
            grid.select(tr);
            */

            //setTimeout(() => { 
            //grid.select("tr:eq(0)"); //kendo needs this for some reason
            //grid.select(`tr:eq(${line})`);
            /*$.map(grid.select(), function (item) {
                $(item).click();
            });*/

            //},1000);
        },

        /**
         * Set focus on the cell of the provided grid
         * @param {object} grid The grid object to work with
         * @param {int} rowIndex The 0 based row index to interact with
         * @param {any} columnName The 0 based column index/name to interact with
         */
        setGridCellFocus: function (grid, rowIndex, columnName) {
            
            /*const currentPageSize = grid.dataSource.pageSize();
            rowIndex = rowIndex > currentPageSize ? rowIndex - currentPageSize - 1: rowIndex;

            let td = grid.tbody.find("tr").eq(rowIndex).find("td").eq(columnIndex);
            grid.current(td);
            $(td[0]).trigger("click");

            return td;*/
            const columnIndex = this.qLib.getGridColumnIndex(grid, columnName);
            rowIndex = grid.select().index();
            if (rowIndex > -1 && typeof columnIndex !== 'undefined') {
                const cell = grid.tbody.find("tr").eq(rowIndex).find("td").eq(columnIndex);
                grid.current(cell);
                //grid.editCell(cell);

                return cell;
            }
        },

        /**
         * Executes the callback when the server call is completed
         * @param {Function} callback The callback function to execute
         */
        inputDataCustom: function(callback){
            
            this.waitForServerCallsToComplete(()=>{
                console.log("inputDataCustom proceeding  after idle");

                callback();
            });
        },

        /**
         * Executes the callback when the server call is completed
         * @param {Function} callback The callback function to execute
         */
        waitUntilPreviousActionsComplete: function (callback) {

            this.inputDataCustom(callback);
        },

        /**
         * Pushes the processes to a queue, wait for the server calls to complete, then execute the next process
         * @param {Function} proceed This is the steps that will be executed
         */
        waitForServerCallsToComplete: function(proceed){
            idleQ.push(proceed);

            const handleIdleRetry = function () {
                console.log("executing after idle retry: ");
                handleIdle();
            }

            const handleIdle = function () {

                //this waits for async AJAX to complete
                if (jQuery.active > 0) {

                    setTimeout(handleIdleRetry, 200);

                    return;
                }

                /*if (idleQ.length > 0) {
                    const one = idleQ.shift();
                    one();
                    console.log("processing idle Q " + one + " remaining: " + idleQ.length);
                }*/

                for (let i = 0; i < idleQ.length; i++) {
                    idleQ[i]();
                    idleQ = idleQ.slice(1);

                    console.log("processing idle Q " + idleQ.length);
                }
            }
                        
            $(document).idle({
                onIdle: function () {
                    handleIdle();

                },
                idle: 200, //1000,
                keepTracking: idleQ.length > 0 ? true : false

            });
            
        },

        msg: "",
        stepRunning: false,

        /**
         * Execute the automation steps
         * @param {int} i The current step number
         * @param {object[]} steps The automation step
         * @param {Function} done This is the callback function
         */
        executeSteps: function (i, assert) {

            
            if (i >= this.steps.length) return;

            let done = assert.async();

            try {
                //this.steps[i].fnc();
                const step = this.steps.shift();
                step.fnc();
                this.stepRunning = true;

                setTimeout(() => {

                    //this waits for async AJAX to complete by adding new 'empty' step on top. 
                    //This will pause another 200ms for AJAX to complete
                    if (jQuery.active > 0) {

                        this.steps.unshift(this.step(() => console.log("......waiting for jQuery.active completed......"), 600));
                        console.log("...........jQuery.active wait added....");
                    }

                    done();
                    this.stepRunning = false;

                    this.msg = baseStaticTestModule.testName + "...." + this.steps.length + "...steps remaining...";

                    console.log(this.msg);

                    this.executeSteps(0, assert);

                }, step.ms);
            } catch (e) {
                done();
                console.error("On Error at " + baseStaticTestModule.testName + ": continue..." + e.stack);
                this.executeSteps(0, assert);
            }
        },

        /**
         * A step is a callback function that fires after certain milliseconds
         * @param {Function} fnc The function that is part of the step
         * @param {int} ms The timeout in milliseconds
         */
        step: function (fnc, ms = 200) {
            return { fnc: fnc, ms: ms };
        },

        /**
         * Execute the steps sequentially in their respective order
         * @param {object} assert The assert object to pass in
         * @param {Function} steps The steps to be executed sequentially
         */
        processSteps: function (assert, testSteps) {
            //assert = baseStaticTestModule.assert;

            const startTest = this.steps.length === 0 && !this.stepRunning; //this.steps.length === 0;

            this.steps = this.steps.concat(testSteps);
            //this.steps.push(this.step(() => console.log(this.steps.length + "...." + baseStaticTestModule.testName + "...steps remaining...")));

            //kick off only first test, then only fill test queue 
            if (startTest) this.executeSteps(0, assert);
        },

        registerTestSuite: function (testSuite) {
            this.testCollection.push(testSuite);
        },

        configTestSuite: function () {
            this.testCollection.forEach(ts => {
                ts.config(this);
            });
        },

        QWrapper: function (name, testFnc) {
            let self = this;

            //now can control more :)
            //appconfig.trace.disabled = true;
            //if (!this.errorLoggingRunning) this.startLoggingErrors();

            QUnit.test(name, function (assert) {
                self.startLoggingErrors();

                baseStaticTestModule.testName = name;
                baseStaticTestModule.assert = assert;
                baseStaticTestModule.configTestSuite();

                testFnc(assert);
               
            });
            
        },

        QOnlyWrapper: function (name, testFnc) {
            let self = this;

            QUnit.only(name, function (assert) {
                self.startLoggingErrors();

                baseStaticTestModule.testName = name;
                baseStaticTestModule.assert = assert;
                baseStaticTestModule.configTestSuite();

                testFnc(assert);

            });

        },

        origError: console.error,
        errorLoggingRunning: false,

        startLoggingErrors: function () {
            if (this.errorLoggingRunning) return;

            console.log("logging started");

            console.stderror = console.error.bind(console);
            console.errors = [];
            console.error = function () {
                
                console.errors.push(Array.from(arguments));
                
                console.stderror.apply(console, arguments);
            }

            this.errorLoggingRunning = true;
            let self = this;

            QUnit.jUnitDone(function (data) {
                console.errors.length = 0;
                console.error = self.origError;
                self.errorLoggingRunning = false;

                console.log("logging stopped");
            });
        }
    }

    this.baseStaticTestModule = helpers.View.extend({}, domainTestModule);


}).call(this);