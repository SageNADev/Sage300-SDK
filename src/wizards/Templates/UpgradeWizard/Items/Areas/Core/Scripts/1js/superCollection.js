(function () {
    'use strict';

    let superCollectionObj = {
        viewid: "",
        gridViewid: "",
        grid: undefined,
        node: "",
        dataModel: undefined,
        UIController: "",
        entityObject: undefined,
        fromSave: true,
        rows: [],

        get allGrids() { 
            if (apputils.isUndefined(this.allGridObj)){
                this.allGridObj = {};
            }
            return this.allGridObj; 
        },

        /**
         * Invoke the get data query through the API
         * @param {String} query The action query
         */
        get: function (query) {
            if (apputils.isUndefined(query) || query.length === 0){
                //alert("nothing to retrieve");
                return;
            }

            return this.ajaxCall("Get", this.UIController + "/GetData", query);
        },

        /**
         * Invoke the GetNew query through the API
         */
        getNew: function () {
            return this.ajaxCall("Get", this.UIController + "/GetNew");
        },

        createChildEntitiesOptions: {},

        /**
         * Returns the current entity
         * */
        getEntity: function () {
            this.entity = new this.entityObject();
            this.entity.initialize();
            this.entity.rowIsReadonly = this.rowIsReadonly;
            this.entity.bindTrigger = this.bindTrigger;
            this.entity.bindListener = this.bindListener;
            this.entity.allCollectionObj = {};
            this.entity.allTreeViewsObj = {};
            //this.entity.createAllChildEntities();
            this.entity.createAllChildEntities(this.createChildEntitiesOptions);
            this.entity.parentViewId = this.parentViewId;
            return this.entity;
        },

        /**
         * Save data
         * @param {String} query The action query
         */
        upsertData: function (query) {
            if (apputils.isUndefined(query) || query.length === 0){
                trace.info("Nothing to save. If changes were made check for invalid entries.");
                return;
            }

            return  this.ajaxCall("Post", this.UIController + "/UpsertData", query);
        },

        //this is for root object at index 0 
        getDataForGrid: function(gridViewid, prefix="") {
            let data;
            apputils.each(this.rows, (entity) => {
                if (apputils.isUndefined(entity)) return [];

                if (entity.viewid === gridViewid) {
                    data = this.loadDataForGrid(prefix);

                } else {
                    data = entity.getDataForGrid(gridViewid, prefix);
                }
            });
            return data;

        },

        getDataForGridEx: function (gridViewid, rowIndex, length = this.rows.length, prefix = "") {


            if (this.viewid === gridViewid) {
                
                return this.loadDataForGrid(prefix, rowIndex, length);

            }

            let data;

            //find child collections that match the gridViewid
            for (let i = rowIndex; i < length; i++) {
                const entity = this.rows[i];

                if (apputils.isUndefined(entity)) {
                    //skip

                } else {
                    data = entity.getDataForGridEx(gridViewid, rowIndex, length, prefix);
                }

                //found the collection, it doesn't matter if it returns empty rows but we are done searching
                if (apputils.isDefined(data)) {
                    break;
                }
            }

            /*
            apputils.each(this.rows, (entity) => {
                if (apputils.isUndefined(entity)) return [];

                if (entity.viewid === gridViewid) {
                    data = this.loadDataForGrid(prefix);

                } else {
                    data = entity.getDataForGrid(gridViewid, prefix);
                }
            });
            */

            return data;
            
        },

        /**
         * Return the data for the grid from the selected row
         * @param {String} gridViewid ID for the grid view
         * @param {int} rowIndex Zero based row Index
         */
        getDataForGridFromSelectedRow: function (gridViewid, rowIndex) {
            
            if (this.rows.length === 0) {
                return [];
            }
            let entity = this.rows[rowIndex];
            if (apputils.isUndefined(entity)) return [];
            return entity.getDataForGrid(gridViewid);
            
        },

        /**
         * Return the data for the grid from multiple source
         * */
        getDataForGridFromMultiSource: function() {
            let data = [];
            apputils.each(this.rows, (entity) => {
                data.push(entity.getDataForGridFromMultiSource());
            });

            return data;
        },

        /**
         * Load the data from the finder to the UI
         */
        loadDataFromFinder: function () {
            //Oct 2 - not used
            //this.CRUDReason = this.CRUDReason || CRUDReasons.ExistingData;
            this.bindThisRowToUI("");

            this.bindThisRowToGrid();            
        },

        /**
         * Bind the row to the grid and reset
         * */
        bindThisRowToGrid: function () {
            this.bindThisRowToGridAndReset(true);
        },

        /**
         * Bind the row to the kendo grid
         * @param {boolean} reset True to reset, False otherwise
         */
        bindThisRowToGridAndReset: function (reset) {
            let popupGrids =[];

            Object.entries(this.allGrids).forEach(grid => {
                if (grid[1].screenContainerId) {
                    popupGrids.push(grid[1]);
                }
            });

            //Handle popup screen grids update only, not affected other grids
            if (popupGrids.length > 0) {
                popupGrids.forEach( grid => grid.updateGridData(this.getDataForGrid(grid.gridViewid), reset));
                return;
            }

            if (this.popupDivId) {
                let self = this;
                let gridObj = apputils.find(this.allGrids, grid => {
                    return grid.gridId.startsWith(self.popupDivId);
                });

                if (gridObj.needsCustomProcessing) {
                    MessageBus.msg.trigger(gridObj.gridId, gridObj);
                } else {
                    gridObj.updateGridData(this.getDataForGrid(gridObj.gridViewid, self.popupDivId), reset);
                }

            } else {
                apputils.each(this.allGrids, grid => {

                    if (grid.needsCustomProcessing) {
                        MessageBus.msg.trigger(grid.gridId, grid);
                    } else {
                        grid.updateGridData(this.getDataForGrid(grid.gridViewid), reset);
                    }

                });
            }
        },

        /**
         * Bind the specific row to the kendo grid
         * @param {int} rowIndex The row index to bind to
         * @param {object} grid The kendo grid
         * @param {boolean} reset True to reset, False otherwise
         */
        bindThisRowToGivenGridAndReset: function (rowIndex, grid, reset) {
            if (grid.needsCustomProcessing) {
                MessageBus.msg.trigger(grid.gridId, grid);
            } else {
                grid.updateGridData(this.getDataForGridFromSelectedRow(grid.gridViewid, rowIndex), reset);
            }

            
        },

        /**
         * Update the grid data
         * @param {int} selectedRowIndex
         */
        loadDataFromTemplate: function (selectedRowIndex) {
            //Oct 2 - handled it later
            //this.CRUDReason = CRUDReasons.AddingNewData;
            //this.selectDataByRowIndex2(selectedRowIndex, "");

            //this.rows = this.findRowByIndex(selectedRowIndex);

            let entity = this.setEntityToFirstRow();
            entity.CRUDReason = CRUDReasons.AddingNewData;

            entity.selectTemplateData();

            apputils.each(this.allGrids, grid =>{ 
                grid.updateGridData(this.getDataForGrid(grid.gridViewid), true);
            
            });

            //since adding new, mark all as dirty.
            //required fields & validation etc if done via
            //validation rules
            //this.setIsDirtyToTrue();
        },

        /**
         * Process the template data for every rows
         * */
        processTemplateDataRows: function () {
            apputils.each(this.rows, (entity) => {
                entity.CRUDReason = CRUDReasons.AddingNewData;
                entity.selectTemplateData();
            });
            
        },

        /**
         * Bind the finder to a listener
         * @param {String} finderType The type of finder
         */
        bindFinderlistener: function(finderType){
        
            let self = this;

            //object to object field  mappings are done using event mgs therefore set a
            //listener to recieve when end is called.
            let msg = this.viewid + this.gridViewid + "mapped";
            this.bindGridlistener(msg);

            Keeler.listenTo(MessageBus.msg, this.viewid + finderType, function (data) {
                self.setBindFlagsAndLoadDataFromFinder(data);
            });
        },

        /**
         * Bind the listener to a finder a load the data
         * @param {any} data Data to load
         */
        setBindFlagsAndLoadDataFromFinder: function(data) {
            this.bindListener = apputils.isUndefined(this.bindListener) ? true : false;
            this.rows = [];
            this.rows.push(data);

            //rows beyond this can be updated therefore set the flag
            this.rowIsReadonly = false;

            this.loadDataFromFinder();
        },

        /**
         * Bind the listener to the kendo grid
         * @param {String} msg Message
         */
        bindGridlistener: function(msg){
            
            Keeler.listenTo(MessageBus.msg, msg, ()=>{
                apputils.each(this.allGrids, grid =>{ 
                    grid.updateGridData(this.getDataForGrid(grid.gridViewid), true);
            
                });
            });

        },

        /**
         * Retrieve a template with default values
         * @param {any} defaultValues The default values of a template
         */
        getTemplate: function(defaultValues){
            let query = this.generateGetTemplateRoot();

            trace.log(query);

            //configure how to bind field value to listeners
            this.rowIsReadonly = this.rowIsReadonly || false;

            let Ok = this.get(query);

            if (apputils.isUndefined(Ok)){
                return;
            }

            Ok.then((resp, status, xhr)=>{
                trace.log(resp);

                if (xhr.hasError){
                    return;
                }

                //bind data to UI & Grid
                this.loadDataFromFinder();
                this.updateVerbToInserting();
                
                if (defaultValues) {
                    defaultValues.forEach(v => {
                        this.rows[0].setFieldData(v.name, v.value);
                    })
                }

                MessageBus.msg.trigger(this.viewid + "KeyValuesChanged", "DefaultValuesSet");
            });

            return Ok;
        },

        /**
         * Update the verb to inserting for each entities in the collection
         * */
        updateVerbToInserting: function () {
            apputils.each(this.rows, (entity) => {
                entity.updateVerbToInserting();
            });
        },

        /**
         * Update the verb to Put for each entities in the collection
         * */
        updateVerbToPut: function () {
            apputils.each(this.rows, (entity) => {
                entity.updateVerbToPut();
            });
        },

        /**
         * Persist the data
         * @param {String} query The query to send to the API
         * @param {boolean} showErrorMessage True to show error message, false otherwise
         */
        saveData: function (query, showErrorMessage = true) {

            return this.persistData(CRUDReasons.ExistingData, query, undefined, showErrorMessage);
        },

        /**
         * Post
         * */
        postData: function () {

            //Oct 2 - posting takes different route need to test later
            //this.CRUDReason = CRUDReasons.PostData;

            //dec 3 - AT-73621 - 1) post existing data - passed; post new data - passed, post existing header with new detail - passed
            return this.persistData(CRUDReasons.PostData);
        },

        /**
         * Process
         * */
        process: function () {

            return this.persistData(CRUDReasons.Process);
        },

        /**
         * Process Get
         * */
        processGet: function () {

            return this.persistData(CRUDReasons.ProcessGet);
        },

        /**
         * Process Put
         * */
        processPut: function () {

            return this.persistData(CRUDReasons.ProcessPut);
        },

        /**
         * Initialize data for an existing object. If no existing object, function will return
         * @param {any} existingObj The existing object
         * @param {Function} callback The callback function
         */
        initData: function (existingObj, callback = undefined) {
            
            let Ok = this.persistData(CRUDReasons.InitData, '', callback);

            if (apputils.isUndefined(Ok)) {
                return;
            }

            let self = this;
            Ok.then(function(result, status, xhr) {

                if (result.noQuery) return;
                if (xhr.hasError) return;
                
                if (apputils.isUndefined(existingObj) || existingObj === null) return;

                //TODO: need to test
                if (existingObj === self) return;
                
                existingObj.adjustDeleted(self);
                existingObj.updateDirtyFlag2(self);
            });

            return Ok;
        },

        /**
         * Adjust the object after deletion
         * @param {any} newObj The object o adjust
         */
        adjustDeleted: function (newObj) {
            for (let i = 0; i < this.rows.length; i++) {

                if (this.rows[i].CRUDReason === CRUDReasons.Deleting) {
                    newObj.rows.splice(i, 1, this.rows[i]);
                    
                    let msgid = this.rows[i].getFieldValue("msgid");
                    newObj.updateRowIndex(msgid, i);
                }

                let newEntity = newObj.rows[i];
                this.rows[i].adjustDeleted(newEntity);
            }

            
        },

        /**
         * Save the data of the collection
         * @param {any} CRUDReason The CRUD reason
         * @param {String} query Query string to invoke the API
         * @param {Function} callback Callback function
         * @param {boolean} showErrorMessage True to show error message, false otherwise
         */
        persistData: function (CRUDReason, query, callback = undefined, showErrorMessage = true){

            query = query || this.generateUpsertDataRoot(CRUDReason);

            if (!query) { //D-45063: no any changes, just save, want to show save sccess message
                if (this.fromSave) { // AT-85096 Only show when save, not show when post
                    setTimeout(() => sg.utls.showMessage({ UserMessage: { IsSuccess: true, Message: globalResource.SaveSuccessMessage } }));
                }
                return;
            };

            if (this.updateSaveQuery && typeof this.updateSaveQuery === 'function') {
                query = this.updateSaveQuery(query);
            }
            trace.info("query - " + query);

            //if (query === "<n t='' n=''></n>") {
            if (query === apputils.rootNodeTemplate + apputils.rootNodeClose) {
                return apputils.noPromiseOk;
            }

            let Ok = this.upsertData(query);

            if (apputils.isUndefined(Ok)){
                return;
            }

            let self = this;

            Ok.then((result, status, xhr) => {
                trace.info(`${status} - ${result}`);

                this.hasErrorMsg = false;
                
                if (xhr.hasError) {
                    if (showErrorMessage) {
                        this.hasErrorMsg = true;
                        let errors = ErrorEntityCollectionObj.getErrors();
                        let errorMsg = {
                            "UserMessage": {
                                "IsEmail": false, "IsSuccess": false, "Message": "", "Errors": errors, "Warnings": [], "Info": null
                            }
                        }
                        sg.utls.showMessage(errorMsg, () => {
                            ErrorEntityCollectionObj.clearError();
                            if (callback && apputils.isFunction(callback)) {
                                callback();
                            }
                        });
                    }

                     //AT-79848
                    if (CRUDReason === CRUDReasons.Deleting && apputils.isFunction(self.resetVerbForDelete)) {
                        self.resetVerbForDelete();
                    }

                    return;
                }

                if (CRUDReason === CRUDReasons.Deleting) {
                    this.deleteCallback();
                    return;
                }

                if (result.length > 0) {

                    if (result.indexOf("OK") > -1) {
                        sg.utls.showMessage({ UserMessage: { IsSuccess: true, Message: globalResource.SaveSuccessMessage } });
                    }
                    //since the record exists while updating and posting the persistDataCB should retrieve the updated record to refresh the UI
                    if (!apputils.isUndefined(this.persistDataCB) && (CRUDReason === CRUDReasons.ExistingData || CRUDReason === CRUDReasons.PostData || CRUDReason === CRUDReasons.ProcessGet)) {
                        this.persistDataCB();
                        
                    }
                    else {
                        this.ParseResponse(result);

                        if (this.rowIsReadonly) return;

                        this.loadDataFromFinder();
                    }
                    MessageBus.msg.trigger(this.viewid + 'SaveSuccess', CRUDReason);
                }
                
            });
            return Ok;
        },

        /**
         * Bind the kendo grid to a message bus
         * @param {String} gridId ID of the kendo grid
         */
        bindToCustomGrid: function(gridId){
            MessageBus.msg.trigger(gridId, this);
        },

        /**
         * If there is no error, process the data for the grid
         * @param {String} query Query string to send to the API
         * @param {any} processData
         * @param {any} next
         */
        processDataForCustomGrid: async function(query, processData, next){
            let Ok = this.get(query);
            if (apputils.isUndefined(Ok)){
                    return;
            }

            Ok.then((resp, status, xhr)=>{
                
                if (xhr.hasError){
                    return;
                }
                
                this.buildCustomArray(processData, next);
                
            });
        },

        /**
         * Returns a custom array object
         * @param {any} processData
         * @param {any} next
         */
        buildCustomArray: function(processData, next){
            let arr = this.getDataForGridFromMultiSource();
                
            let customFieldsColl = [];
            
            apputils.each(arr, (data)=>{
                let fields = {};    
                
                apputils.each(data, (obj)=>{
                    processData(obj, fields);
                });

                //there maybe data rows which doesn't need to be processed; In those cases empty object is returned
                if (apputils.keys(fields).length > 0){
                    customFieldsColl.push(fields);
                }
            });
            
            next(customFieldsColl);
        },

        /**
         * Create a custom array by converting the columns to rows
         * @param {any} processData
         * @param {any} next
         */
        buildCustomArrayByConvertingColumnsToRows: function (processData, next) {
            let arr = this.getDataForGridFromMultiSource();

            let customFieldsColl = [];

            apputils.each(arr, (data) => {
                let fields = [];

                apputils.each(data, (obj) => {
                    processData(obj, fields);
                });

                //there maybe data rows which doesn't need to be processed; In those cases empty object is returned
                if (fields.length > 0) {
                    customFieldsColl = fields;
                }
            });

            next(customFieldsColl);
        },

        getSearchFilter: function (field, values, operator) {

            if (operator) {
                return apputils.fields.formatQueryString(field.name, operator, values, field.dataType);
            }

            return apputils.fields.formatQueryString(field.name, apputils.Operators.Like, values, field.dataType);

        },

        /**
         * Create an expression to filter the field
         * @param {any} currentFilter Temp filter
         * @param {any} value Value to filter
         * @param {any} field Field to filter
         * @param {any} operator Operator to use
         */
        buildFilterFieldExpression: function(currentFilter, value, field, operator){
            let valueFilter = value.length > 0 ? this.getSearchFilter(field, value, operator) : "";
            return this.buildFilterExpression(currentFilter, valueFilter, operator);
        },

        /**
         * Create an expression to filter the collection
         * @param {any} currentFilter Temp filter
         * @param {any} value Value to filter
         * @param {any} operator Operator to use
         */
        buildFilterExpression: function(currentFilter, value, operator){
            let operatorEpression = currentFilter.length > 0 && value.length > 0 ? ` ${operator} ` : "";
            return operatorEpression + value;
        },

        /**
         * Add a detail line to a kendo grid
         * @param {int} gridSelectedRowIndex Row index to add
         * @param {String} detailEntityViewId View ID for the detail entity
         */
        addDetailLineToGrid: function (gridSelectedRowIndex, detailEntityViewId) {
            if (this.rows.length === 0) {
                return;
            }

            let currentRowIndex = gridSelectedRowIndex < 0 ? 0 : gridSelectedRowIndex;

            this.copyRow(detailEntityViewId, currentRowIndex);

        },

        /**
         * Add an empty detail line to a kendo grid
         * @param {int} gridSelectedRowIndex Row index to add
         * @param {String} detailEntityViewId View ID for the detail entity
         */
        addEmptyDetailLineToGrid: function (gridSelectedRowIndex, detailEntityViewId) {
            let currentRowIndex = gridSelectedRowIndex < 0 ? 0 : gridSelectedRowIndex;

            this.addEmptyRow(detailEntityViewId, currentRowIndex);

        },

        /**
         * Delete a line from the grid
         * @param {int} gridSelectedRowIndex Row index to delete
         * @param {String} detailEntityViewId View ID for the detail entity
         */
        deleteLineFromGrid: function (gridSelectedRowIndex, detailEntityViewId) {
            if (this.rows.length === 0 || gridSelectedRowIndex < 0) {
                return;
            }

            this.findRowAndMarkforDeletion(detailEntityViewId, gridSelectedRowIndex);

        },

        /**
         * Delete a row with a confirmation message
         * @param {any} message Confirmation message
         * @param {String} dialogTitle The title to the confirmation dialog
         */
        deleteRowWithConfirmation: function (message, dialogTitle) {

            if (this.rows.length === 0 || this.rows[0].CRUDReason === CRUDReasons.AddingNewData) {
                return;
            }
            
            sg.utls.showKendoConfirmationDialog(() => {
                this.findRowAndMarkforDeletion(this.viewid, 0, true);

                //Oct 2 - need to test when deleting
                this.CRUDReason = CRUDReasons.Deleting;
                this.persistData(CRUDReasons.Deleting);
            },
                null, message, dialogTitle);
        },

        /**
         * Delete a detail line with a confirmation message
         * @param {any} message Confirmation message
         * @param {String} dialogTitle The title to the confirmation dialog
         * @param {Function} callback The callback function
         */
        deleteDetailLineWithConfirmation: function (message, dialogTitle, callback) {

            sg.utls.showKendoConfirmationDialog(() => {
                callback();
            },
                null, message, dialogTitle);
        },

        getSearchKeyFilters: function () {
            return "";
        },

        executeXSearch: function (filterValue, filterField) {

            //configure how to bind field value to listeners
            this.rowIsReadonly = false;
            this.bindTrigger = true;
            this.bindListener = false;

            return this.executeXSearch2(filterValue, filterField);

        },

        executeSearchAndUpdateUI: function(filterValue, filterField) {

            return this.executeXSearch(filterValue, filterField);

        },

        executeXSearch2: function (filterValue, filterField) {
            
            let filter = this.getKeysAndOtherSearchFilter(filterValue, filterField);

            let filterFn = (viewid) => viewid === this.viewid ? filter : "";

            let query = this.generateGetRoot(filterFn, this.depthViewIds || "");

            return this._executeXSearch2(query);
                        
        },

        /**
         * Search from the query string
         * @param {String} query Query String to search
         */
        _executeXSearch2: function (query) {
            
            this.rows = [];
            
            //configure how to bind field value to listeners
            this.rowIsReadonly = this.rowIsReadonly ?? true;
            this.bindTrigger = this.bindTrigger ?? false;
            this.bindListener = this.bindListener ?? false;

            trace.log(query);

            let Ok = this.get(query);

            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {

                trace.log(resp);

                if (xhr.hasError) {
                    trace.log("Search Error");
                    return;
                }

                if (!apputils.isUndefined(this.callback)) {
                    this.callback(this.rows[0]);
                }
            });

            return Ok;
        },

        /**
         * Return the keys and other search filter
         * @param {any} filterValue Value to filter
         * @param {any} filterField Field to filter
         */
        getKeysAndOtherSearchFilter: function (filterValue, filterField) {

           return this.getKeysAndOtherSearchFilter2(filterValue, filterField, apputils.Operators.Equals);
        },

        /**
         * Return the keys and less than search filter
         * @param {any} filterValue Value to filter
         * @param {any} filterField Field to filter
         */
        getKeysAndLessThanSearchFilter: function (filterValue, filterField) {
            return this.getKeysAndOtherSearchFilter2(filterValue, filterField, apputils.Operators.LessThan);
        },

        /**
         * Return the keys and more than search filter
         * @param {any} filterValue Value to filter
         * @param {any} filterField Field to filter
         */
        getKeysAndMoreThanSearchFilter: function (filterValue, filterField) {
            return this.getKeysAndOtherSearchFilter2(filterValue, filterField, apputils.Operators.MoreThan);
        },

        /**
         * Return the keys and other search filter
         * @param {any} filterValue Value to filter
         * @param {any} filterField Field to filter
         */
        getKeysAndOtherSearchFilter2: function (filterValue, filterField, operator) {

            let filter = "";

            //when data model has key or foreign key filters; implement getSearchKeyFilters function; See example in finders (ICBOMFinder.js) 
            let keyFilters = this.getSearchKeyFilters();

            const filterValueAsString = filterValue + "";
            let findAll = filterValueAsString.length === 0;

            if (findAll) {
                filter = keyFilters;
            }
            else {

                filter += this.buildAnyFilter(keyFilters, filterValueAsString, filterField, operator);

            }

            return filter;

        },

        /*left as examples
        buildAndFilter: function (keyFilters, filterValue, filterField) {
            /*let filter = "";

            filter += this.buildFilterExpression(filter, keyFilters, apputils.Operators.And);

            filter += this.buildFilterFieldExpression(filter, filterValue, filterField, apputils.Operators.And);

            return filter;* /
            return this.buildAnyFilter(keyFilters, filterValue, filterField, apputils.Operators.And);
        },

        buildLessThanFilter: function (keyFilters, filterValue, filterField) {
            return this.buildAnyFilter(keyFilters, filterValue, filterField, apputils.Operators.LessThan);
        },

        buildMoreThanFilter: function (keyFilters, filterValue, filterField) {
            return this.buildAnyFilter(keyFilters, filterValue, filterField, apputils.Operators.MoreThan);
        },
        */

        /**
         * Build a filter
         * @param {any} keyFilters Key for the filter
         * @param {any} filterValue Value for the filter
         * @param {any} filterField Field for the filter
         * @param {any} operator The operator to use
         */
        buildAnyFilter: function (keyFilters, filterValue, filterField, operator) {
            let filter = "";

            filter += this.buildFilterExpression(filter, keyFilters, apputils.Operators.And);

            filter += this.buildFilterFieldExpression(filter, filterValue, filterField, operator);

            return filter;
        },

        /**
         * This is used with details popup
         * @param {any} currentRow Starting row of the grid
         * @param {any} direction Direction to navigate
         */
        navigate: function (currentRow, direction) {
            let startingRow = currentRow;
            currentRow = Number(currentRow + direction);
            direction = direction === 0 ? 1 : direction;

            for (currentRow; currentRow > -1 && currentRow < this.rows.length; currentRow += direction) {
                let deleted = this.rows[currentRow].isMarkedForDeletion();

                if (!deleted) break;
                
            }

            if ((currentRow < 0 || currentRow >= this.rows.length) && startingRow < this.rows.length) {
                currentRow = startingRow;
            }

            if (currentRow < 0 && !apputils.isUndefined(this.rows[currentRow])) {
                return -1;
            }

            if (apputils.isUndefined(this.rows[currentRow]) && this.rows.length > 0) {
                return this.rows.length-1; //-1;
            }

            if (apputils.isUndefined(this.rows[currentRow])) {
                return -1;
            }

            if (currentRow < 0 || this.rows[currentRow].isMarkedForDeletion()) {
                return -1;
            }

            return currentRow;

        },

        /**
         * Bind to the user update listener
         * @param {any} msgtoListen The message to listen
         * @param {Function} callback Callback function
         * @param {String} ctx
         */
        bindToUsrUpdatelistener: function (msgtoListen, callback, ctx='') {

            Keeler.listenTo(MessageBus.msg, msgtoListen + apputils.EventMsgTags.usrUpdate, function (data) {

                callback(data);
            }, ctx);

        },

        /**
         * Returns true if update is allowed, false otherwise
         * @param {any} data The data to check
         */
        checkIfUpdateAllowed: function (data) {
            if (this.viewid === data.viewid || this.prefixNamespace === data.viewid) {
                return this.rows[data.rowIndex] ? this.rows[data.rowIndex].isUpdateAllowed(data.field): false;
            }
            let updateAllowed = false;

            for (let i = 0; i < this.rows.length; i++) {
                updateAllowed = this.rows[i].checkIfUpdateAllowed(data);
                if (updateAllowed || updateAllowed.editAllowed) {
                    break;
                }
            };
            return updateAllowed;
        },

        /**
         * Returns true if dirty records is found, false otherwise
         * */
        findDirtyRecords: function () {
            let dirtyList = [];

            apputils.each(this.rows, (entity) => {
                dirtyList = entity.findDirtyRecords();
            });

            return dirtyList;
        },

        /**
         * Update the dirty flag
         * */
        updateDirtyFlag: function () {
            apputils.each(this.rows, (entity) => {
                entity.updateDirtyFlag();
            });
        },

        /**
         * Update the dirty flag
         * */
        updateDirtyFlag2: function (newObj) {
            for (let i = 0; i < this.rows.length; i++) {

                let newEntity = newObj.rows[i];
                this.rows[i].updateDirtyFlag2(newEntity);
            }

        },

        /**
         * Returns the object given the view ID
         * @param {String} viewid The view ID of the object
         */
        findObjectByViewId: function (viewid) {
            if (this.viewid === viewid) {
                return true;
            }

            let collectionEntity = apputils.find(this.rows, function (obj) {
                return obj.findObjectByViewId(viewid);
            });

            return collectionEntity;
        },

        /**
         * 
         * @param {String} fieldName Field name to check
         */
        checkAllRowsforFieldWithMissingData: function (fieldName) {
            let result = {};

            for (let i = 0; i < this.rows.length; i++) {

                if (this.rows[i].CRUDReason !== CRUDReasons.Deleting) {
                    result = this.checkFieldsOnlyForMissingData(i, fieldName);
                }

            }

            return result;
        },

        /**
         * Check the fields for missing data
         * @param {int} rowIndex Row index for the field
         * @param {String} fieldName Name of the field
         */
        checkFieldsForMissingData: function (rowIndex, fieldName) {
            return this.rows[rowIndex].checkFieldsForMissingData(fieldName);
        },

        //this doesn't throw warning messages
        checkFieldsOnlyForMissingData: function (rowIndex, fieldName) {
            return this.rows[rowIndex].checkFieldsOnlyForMissingData(fieldName);
        },

        /**
         * Update the line numbers
         * */
        adjustLineNumbers: function () {

            let lineNumber = 0;

            for (var i = 0; i < this.rows.length; i++) {
                if (apputils.isUndefined(this.rows[i])) {
                    ++lineNumber;

                } else if (this.rows[i].CRUDReason === CRUDReasons.Deleting) {
                    //skip

                } else {
                    this.rows[i].adjustLineNumber(++lineNumber);
                }
            }

        },

        /**
         * Returns true if there is an error, false otherwise
         * */
        hasError: function () {
            for (let i = 0; i < this.rows.length; i++) {

                if (this.rows[i].hasError()) {
                    return true;
                }
                
            }

            return false;
        },

        /**
         * Set the prefix namespace for all entity
         * @param {any} prefix Current prefix
         */
        setPrefixNamespaceForAll: function (prefix) {
            this.prefixNamespace = prefix + this.viewid;
            
            apputils.each(this.rows, (entity) => {
                entity.setPrefixNamespaceForAll(prefix);
            });
        },

        /**
         * Copy the field values from an entity to target entity
         * @param {any} fromEntity The entity to copy from
         * @param {any} currentRow Currrent entity row index
         */
        copyFieldValuesFromAnotherEntity: function (fromEntity, currentRow) {
            /*
            if (this.rows.length === 0 || !this.rows[currentRow]) {
                this.rows = fromEntity;
                return;
            }*/

            const copyToThisEntity = this.rows[currentRow];
            const childFromEntity = fromEntity.rows[0];
            copyToThisEntity.copyFieldValuesFromAnotherEntity(childFromEntity);

            /*for (let i = 0; i < fromEntity.rows.length; i++) {
                const childFromEntity = fromEntity.rows[i];
                copyToThisEntity.copyFieldValuesFromAnotherEntity(childFromEntity);
            }*/
        },

        copyFieldValuesFromChildEntity: function (fromEntity) {
            //const copyToThisEntity = this;

            //copyToThisEntity.copyFieldValuesFromAnotherEntity(fromEntity, 0);

            if (this.rows.length === 0) {
                this.rows = fromEntity.rows;
                this.rows.forEach(row => {
                    row.rowIsReadonly = false;
                    row.bindListener = true;
                    row.bindTrigger = true;
                    row.selectRowByIndex("");
                });
                return;
            }

            for (let i = 0; i < fromEntity.rows.length; i++) {
                const childFromEntity = fromEntity.rows[i];
                const copyToThisEntity = this.rows[i];

                if (copyToThisEntity) {
                    copyToThisEntity.copyFieldValuesFromAnotherEntity(childFromEntity);
                } else {
                    this.rows[i] = childFromEntity;

                    //may need this in future
                    //this.rows[i].selectRowByIndex("");
                }
            }
        },

        getBulkNavigationFilter: function () {
            if (this.rows.length === 0) return "";

            //TODO: add offset when skipping few pages
            return this.rows[this.rows.length - 1].getBulkNavigationFilter();
        },

        /**
         * Returns the number of rows
         * */
        getTotalRowCount: function () {
            
            if (this.rows.length === 0) return 0;

            const rowCountField = this.entityObject.prototype.dataModelObj.TotalRowCount;

            if (rowCountField) {
                const fieldName = rowCountField.field;
                const entityRowCount = +this.rows[this.rows.length - 1].getFieldValue(fieldName);

                //Handle deleted and inserted grid lines when max grid page count is reached.
                //if last row that is fetched from DB is deleted but not saved; it will still have a row count which is still valid
                //-1 indicates either last row is inserted by user and can be deleted as well 
                if (entityRowCount === -1) {
                    return entityRowCount;

                } else if (this.rows.length > entityRowCount) {
                    return this.rows.length - this.numberofEntitiesMarkedForDeletion();

                } else {
                    return entityRowCount - this.numberofEntitiesMarkedForDeletion();
                }
            }

            return 0;
        },

        /**
         * Update the total row count
         * @param {int} totalRowCount Number of rows
         */
        adjustTotalRowCountAfterLazyRetrieve: function (totalRowCount) {

            if (this.rows.length === 0) return;
            
            if (totalRowCount === 0) return;

            this.rows.forEach(entity => {
                entity.adjustTotalRowCountAfterLazyRetrieve(totalRowCount);
            });

        },

        /**
         * Update all entities in collection object by refreshing entity data
         * @param {object} newEntityColl The collection entity containing all new data rows
         * @param {function} getRowIndexFnc This to match the starting node where data has changed
         * @param {boolean} copyChildObj This controls updating all child objects once the updated entity is found
         */
        refreshUI: function (newEntityColl, getRowIndexFnc, copyChildObj=false) {

            //this object and newEntityColl are at the same object level
            if (this.rows.length === 0) return;

            const rowIndex = getRowIndexFnc(this.viewid);

            if (rowIndex === -1 && copyChildObj) {
                //move to next child entity
                this.refreshChildUI(newEntityColl, getRowIndexFnc);

            } else {
                copyChildObj = true;
                const entity = this.rows[rowIndex];

                //newEntityColl will always have only 1 row
                entity.refreshUI(newEntityColl.rows[0], getRowIndexFnc, copyChildObj);
            }

            return copyChildObj;
        },

        /**
         * Update all child objects by refreshing entity data
         * @param {object} newEntityColl The collection entity containing all new data rows
         * @param {function} getRowIndexFnc This to match the starting node where data has changed
         */
        refreshChildUI: function (newEntityColl, getRowIndexFnc) {
            for (let i = 0; i < this.rows.length; i++) {
                const entity = this.rows[i];

                const newEntity = newEntityColl.rows[i];
                entity.refreshUI(newEntity, getRowIndexFnc, true);
            }
        },

        Exists: function () {

            return this.getLength() > 0;
        },

        /**
         * Return filter including necessary key fields as specified by the entity at the given rowIndex in the collection object
         * @param {int} rowIndex The array index of collection object containing existing row entity
         */
        getExistsFilter: function (rowIndex = 0) {

            if (this.getLength() === 0) {
                return "";
            }

            return this.rows[rowIndex].getExistsFilter();
        },
    };

    this.superCollection = baseObjectCollection.extend(superCollectionObj);
    
}).call(this);
