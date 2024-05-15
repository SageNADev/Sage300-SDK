(function () {
    'use strict';

    this.CRUDReasons = this.apputils.CRUDReasons;

    Object.freeze(this.CRUDReasons); //not allowed to change or add properties

    /** Entity domian base object */
    var domainObject = {
        viewid: undefined,
        xmlPath: undefined,
        dataModel: undefined,
        rowObj: undefined,
        gotoPage: -1,
        pageSize: -1,

        //This property allows to auto-bind columns to fields from update from server, user interaction or calculated fields
        //Set this flag to true for Finder since its retrieved data that will never get updated
        rowIsReadonly: false,
        bindTrigger: true,
        bindListener: true,

        /** Object initialize */
        initialize: function () {
            /*can override to execute init with options*/
        },

        /**
         * Object init method, should override in sub object
         * @param {any} options Options
         */
        init: function (options) {
            /*should override*/
        },

        /**Create all child entities(objects), should override in sub object */
        createAllChildEntities: function () {
            /*should override*/
        },

        /**
         *  Populate next entity
         * @param {any} child
         * @param {any} id
         */
        populateNext: function (child, id) {

            let childEntity = this.createChildEntity(child.nodeName);

            if (apputils.isUndefined(childEntity)) {
                return;
            }

            childEntity.populate(child, id);

        },

        /** Initilize row object */
        initRowObj: function () {
            if (apputils.isUndefined(this.rowObj)) {
                this.rowObj = new rowDataObj();
                this.rowObj.dataModel = this.dataModel;
                this.rowObj.viewid = this.viewid;
                this.rowObj.validationRules = this.validationRules;
                this.rowObj.caller = this;
            }
        },

        /**
         * custome call back function binding
         * @param {any} customBinding The customBinding function name
         * @param {any} column The binding column
         * @param {any} data The data
         */
        customBindingCallBack: function (customBinding, column, data) {

            if (apputils.isFunction(this[customBinding])) {
                (this[customBinding])(column, data);
            } else {
                data.previousValue = column.value;
                MessageBus.msg.trigger(customBinding, { column: column, data: data });
            }
        },

        /** Reset row object*/
        resetRowObj: function () {
            this.initRowObj();

            this.rowObj.init();
            this.rowObj.rowIsReadonly = this.rowIsReadonly;
            this.rowObj.bindTrigger = this.bindTrigger;
            this.rowObj.bindListener = this.bindListener;
        },

        /**
         *  Populate childs object 
         * @param {any} xml The xml DOM 
         * @param {any} rowIndex The row index
         * @param {any} id 
         */
        populateCompositions: function (xml, rowIndex, id) {
            var self = this;

            this.resetRowObj();

            apputils.each(xml.childNodes, function (child) {
                if (child.nodeName === "row" || child.nodeName === "rv") {
                    self.rowObj.populateColumns(child, rowIndex, id);
                }
                else {

                    self.populateNext(child, id);
                }
            });

        },

        /**
         * Load data for grid
         * @param {any} gridArray Kendo grid array objects
         * @param {any} prefix Prefix namespace
         */
        loadDataForGrid: function (gridArray, prefix="") {

            if (this.CRUDReason === CRUDReasons.Deleting || this.CRUDReason === CRUDReasons.Ignore || apputils.isUndefined(this.rowObj)) {
                return;
            }

            this.rowObj.loadDataForGrid(gridArray, prefix);

        },

        /**
         * Load data for the kendo grid, for the columns only
         * @param {any} gridArray Kendo grid array
         */
        loadDataVerticallyForGrid: function (gridArray) {

            if (this.CRUDReason === CRUDReasons.Deleting || apputils.isUndefined(this.rowObj)) {
                return;
            }

            this.rowObj.loadDataVerticallyForGrid(gridArray);

        },

        /** Load the grid data */
        loadDataForGrid2: function () {
            let gridArray = [];

            this.loadDataForGrid(gridArray);

            return gridArray;
        },

        /** Load the grid data */
        loadDataForGridVertically: function () {
            let gridArray = [];

            this.loadDataVerticallyForGrid(gridArray);

            return gridArray;
        },

        /** Load finder grid columns */
        loadFinderColumns: function () {
            this.initRowObj();
            return this.rowObj.loadFinderColumns();
        },

        /**
         * Get row object by row index
         * @param {any} rowIndex
         */
        findRowByIndex: function (rowIndex) {
            return this.rowObj.findRowByIndex(rowIndex);
        },

        /**
         * Select the first row
         * @param {any} prefix
         */
        selectTopRow: function (prefix) {
            this.rowObj.selectTopRow(prefix);
        },

        /**
         * Select row by row index
         * @param {any} prefix Prefix namespace
         */
        selectRowByIndex: function (prefix) {
            this.rowObj.bindListener = this.bindListener;

            this.rowObj.selectRowByIndex(prefix);

            apputils.each(this.allCollectionObj, obj => {
                if (!apputils.isEmpty(obj) && !obj.skipInAllQuery) {
                    obj.selectRowByIndex(prefix);
                }
            });
        },

        /**
         * Get column by field name
         * @param {any} fieldName
         */
        getColumnByFieldName: function (fieldName) {
            return this.rowObj.getColumnByFieldName(fieldName);
        },
        /**
         * get row column by field name
         * @param {any} rowNode The row node
         * @param {any} fieldName field name
         */
        getRowColumnByFieldName: function (rowNode, fieldName) {
            return this.rowObj.getRowColumnByFieldName(rowNode, fieldName);
        },

        /**
         * Set column field value
         * @param {any} args
         */
        setColumnFieldValue: function (args) {
            this.rowObj.setColumnFieldValue(args);
        },

        includeUpsertChildDataEvenIfNotDirty: function (upsert, index) {
            return "";
        },

        includeInitDataUpsert: function (index, upsert) {
            return upsert;
        },

        /**
         * Upsert child data
         * @param {any} index
         * @param {any} verb
         */
        upsertChildData: function (index, verb) {
            let upsert = "";

            //not need to get child data for posting
            /*if (verb === CRUDReasons.PostData) {
                return upsert;
            }*/

            if (verb === CRUDReasons.InitData && this.CRUDReason === CRUDReasons.Deleting) {
                return upsert;
            }

            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    upsert += obj.generateUpsertData(index, verb);
                }

            });

            upsert += this.includeUpsertChildDataEvenIfNotDirty(upsert, index);

            return upsert;
        },

        /**
         * Get child template
         * @param {any} index
         * @param {any} method
         */
        getChildTemplate: function (index, method) {
            return "";
        },

        //override this function for if entity has children see IC0200BillsofMaterialEntity.js
        getChildInitOnly: function (index, method, existingEntity) {

            let properties = "";

            if (!(apputils.isUndefined(existingEntity)) && this.viewid === existingEntity.viewid) {
                properties = existingEntity.rowObj.generateInitRowQuery(existingEntity.rowObj.rowNodes[0], index);
            }

            return properties;
        },

        /** Get filter, should be implemented by the specific bject */
        getFilter: function () {
            //"Not Implemented: should be implemented by the some object.";
            return "";
        },

        /** Process dependencies, should be implemented by the specific object */
        processDependencies: function () {
            //"Not Implemented: should be implemented by the some object.";
        },

        /**
         * Select data by row index
         * @param {any} prefix Namespace prefix
         */
        selectDataByRowIndex: function (prefix) {
            this.selectTopRow(prefix);

            apputils.each(this.allCollectionObj, obj => {
                if (!obj.skipInAllQuery) {
                    obj.rowIsReadonly = this.rowIsReadonly;
                    obj.bindTrigger = this.bindTrigger;
                    obj.bindListener = this.bindListener;
                    obj.selectRowByIndex(prefix);
                }
            });

            this.processDependencies();
        },

        /** Update the action verb to Insert */
        updateVerbToInserting: function () {
            this.CRUDReason = CRUDReasons.AddingNewData;

            apputils.each(this.allCollectionObj, obj => {
                
                if (!(apputils.isEmpty(obj) || obj.skipInAllQuery)) {
                    obj.updateVerbToInserting();
                }
            });
        },

        resetVerbIfNecessary: function () {
            //nothing this.CRUDReason = CRUDReasons.ExistingData;
        },

        /** Update the action verb to Put */
        updateVerbToPut: function () {

            this.resetVerbIfNecessary();
            
            apputils.each(this.allCollectionObj, obj => {

                if (!(apputils.isEmpty(obj) || obj.skipInAllQuery)) {
                    obj.updateVerbToPut();
                }
            });
        },

        /**
         * Should include Upsert child data, override this function in main ui entity
         * @param {any} children
         */
        shouldIncludeUpsertChildData: function (children) {

            return children.length > 0;
           
        },

        //override this function in any entity where input needs custom handling
        //example; there are cases where blank field data can't be passed when persisting record
        //see Columbus-IC\IC\Scripts\BOM\IC0200BillsofMaterialEntity.js
        executeSpecificResetsBeforePersist: function () {
            //empty
        },

        /**
         * Generate Upsert data query, used in screen save/update action
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} index
         */
        generateUpsertData: function (parentIndex, verb, index) {

            if (this.upsertDisabled) {
                return "";
            }

            if (verb === CRUDReasons.InitData && this.CRUDReason === CRUDReasons.Deleting) {
                return "";
            }

            let query = "";
            let children = "";

            //AT-74449 - now adding viewid but it increases the payload
            //let id = this.viewid + parentIndex + "" + index;
            let id = this.viewid + parentIndex + index;
           
            //apputils.each(this.rowObj.rowNodes, (row) => {
            this.rowObj.rowNodes.forEach((row) => {

                if (this.CRUDReason === CRUDReasons.Deleting) {
                    //when deleting entity object, field values not needed
                    children += " ";

                } else {

                    this.executeSpecificResetsBeforePersist();
                    children += this.rowObj.generateUpsertRowQuery(row, id);
                }

            });

            if (this.shouldIncludeUpsertChildData(children, verb)) {
                children += this.upsertChildData(id, verb);
            }

            //Note:  CRUDReasons.ProcessGet || verb === CRUDReasons.Process is enabled then test IC0189 as well
            if (children.length > 0 || verb === CRUDReasons.PostData /*|| verb === CRUDReasons.ProcessGet || verb === CRUDReasons.Process*/) {
                const tmpVerb = this.getAppropriateVerb(this.CRUDReason, verb); //this.CRUDReason || verb;

                //must have data to insert
                if (children.length === 0 && tmpVerb === CRUDReasons.AddingNewData) {
                    //do nothing
                } else {
                    query += this.buildFilterQuery(this.rowObj.rowNodes[0], parentIndex, id, tmpVerb, children, verb);
                }
            }
            
            return query;
        },

        /**
         * Get appropriate CRUDReason verb
         * @param {any} CRUDReason
         * @param {any} verb
         */
        getAppropriateVerb: function (CRUDReason, verb) {
            return CRUDReason || verb;
        },

        /**
         * Get concurrency filter. When verb is Init, just get filter
         * @param {any} row
         * @param {any} verb Query action
         */
        getConcurrencyFilter: function (row, verb) {
            //AT-77816. When verb is "Init", not need to use concurrency filter
            if (verb === CRUDReasons.InitData || verb === CRUDReasons.InitHeader ) {
                return this.getInitFilter();
            }

            let str = this.getFilter(row);

            if (verb === CRUDReasons.Verify) {
                return str;
            }

            if (this.isOptionalFieldView(row)) {
                return str;
            }

            let dbValueFilterStr = this.rowObj.getDBValueFilterIfDirty();
            if (dbValueFilterStr.length > 0) {
                if (str.length > 0) {
                    str = str.slice(0, -2);
                    str += " and " + dbValueFilterStr + "' ";
                }
                else { //getFilter is empty;
                    str = "f='" + dbValueFilterStr + "' ";
                }
            }
            
            return str;
        },

        /**
         * Check whether the view is optional field view
         * @param {any} row
         */
        isOptionalFieldView: function (row) {
            const isOptinalField = row.Columns.filter(c => c.field === 'OPTFIELD').length === 1;
            const isSWSET = row.Columns.filter(c => c.field === 'SWSET').length === 1;

            return isOptinalField && isSWSET;
        },

        /**
         * Build query with filter string
         * @param {any} row
         * @param {any} parentIndex
         * @param {any} id
         * @param {any} verb  Query action
         * @param {any} childNode
         * @param {any} oriVerb
         */
        buildFilterQuery: function (row, parentIndex, id, verb, childNode, oriVerb) {
            const processFilter = () => {
                let str = this.getConcurrencyFilter(row, oriVerb);
                return str.length > 0 ? str : "f='' ";
            }

            const filter = (this.CRUDReason === verb && verb === CRUDReasons.Verify) || this.CRUDReason === CRUDReasons.ExistingData || verb === CRUDReasons.Deleting || verb === CRUDReasons.PostData || this.CRUDReason === CRUDReasons.AddingNewData ? processFilter() : "f='' ";

            return this.buildQuery(parentIndex, id, verb, childNode, filter);
        },

        /**
         * Build query
         * @param {any} parentIndex
         * @param {any} id
         * @param {any} verb  Query action
         * @param {any} childNode
         * @param {any} filter
         */
        buildQuery: function (parentIndex, id, verb, childNode, filter) {
            //let obj = { viewId: this.viewid, filter, parentId: parentIndex, id, verb };
            let obj = { viewId: this.viewid, filter, parentId: parentIndex, id, verb, gp: this.gotoPage, ps: this.pageSize};
            return `${apputils.createRowNode(obj)}${childNode}${apputils.rowNodeClose}`;
        },

        /**
         * Generate get template query
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} childIndex
         */
        generateGetTemplateQuery: function (parentIndex, verb, childIndex= "1") {
            let id = parentIndex + childIndex;
            let children = this.getChildTemplate(id, verb);

            const filter = "f='' ";
            return this.buildQuery(parentIndex, id, verb, children, filter);
        },

        /**
         * Generate init only query
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} existingEntity
         * @param {any} childIndex
         */
        generateInitOnlyQuery: function (parentIndex, verb, existingEntity, childIndex = "1") {
            let id = parentIndex + childIndex;
            let children = this.getChildInitOnly(id, verb, existingEntity);

            const filter = "f='' ";
            
            return this.buildQuery(parentIndex, id, verb, children, filter);
        },

        /**
         * Generate get query
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} filterCallback
         * @param {any} depthViewId
         */
        generateGetQuery: function (parentIndex, verb, filterCallback, depthViewId, childIndex = "1") {
            let id = parentIndex + childIndex;
            let children = "";

            let childId = apputils.find(depthViewId, (item)=>{
                return item === this.viewid;
            });

            if (depthViewId !== "" && apputils.isUndefined(childId)) {
                return "";
            }

            if (depthViewId === "" || !(apputils.isUndefined(childId))) {
                children = this.generateChildGetQuery(id, verb, filterCallback, depthViewId);
            }

            let filter = filterCallback(this.viewid, this.parentViewId);

            if (filter.length > 0 && !filter.startsWith("f='")) {
                filter = `f='${filter}' `;
            } 

            //const filter = "f='" + filterCallback(this.viewid, this.parentViewId) + "' ";
            return this.buildQuery(parentIndex, id, verb, children, filter);
        },

        /**
         * Generate child get query string
         * @param {any} index
         * @param {any} method
         * @param {any} filterCallback
         * @param {any} depthViewId
         */
        generateChildGetQuery: function (index, method, filterCallback, depthViewId) {
            let query = "";
            let childIndex = 0;
            apputils.each(this.allCollectionObj, obj => {

                if (!obj.skipInAllQuery) {
                    query += obj.generateGetQuery(index, method, filterCallback, depthViewId, childIndex);
                }

                childIndex++;
            });

            return query;
        },

        /** set Child IsDirty flag to false*/
        setChildIsDirtyToFalse: function () {
            apputils.each(this.allCollectionObj, obj => {
                if (!obj.skipInAllQuery) {
                    obj.setIsDirtyToFalse();
                }
            });
        },

        /** set IsDirty to false */
        setIsDirtyToFalse: function () {
            this.rowObj.setIsDirtyToFalse();
            this.setChildIsDirtyToFalse();
        },

        /** remove column listener */
        removeColumnListener: function () {
            this.rowObj.removeColumnListener();
        },

        /** set child IsDirty flag to true */
        setChildIsDirtyToTrue: function () {
            apputils.each(this.allCollectionObj, obj => {
                if (!obj.skipInAllQuery) {
                    obj.setIsDirtyToTrue();
                }
            });
        },

        /** set IsDirty flag to true */
        setIsDirtyToTrue: function () {

            if (this.upsertDisabled) {
                return;
            }

            this.rowObj.setIsDirtyToTrue();
            this.setChildIsDirtyToTrue();
        },

        populateEmptyCompositions: function (rowIndex, parentIndex) {

            this.rowObj.populateEmptyCompositions(rowIndex, parentIndex);

        },

        /**
         * get column formated value by field name
         * @param {any} fieldName The field name
         */
        getColumnFormatedValueByFieldName: function (fieldName) {
            return this.rowObj.getColumnFormatedValueByFieldName(fieldName);
        },

        /**
         * Get field DB value
         * @param {any} fieldName The field name
         */
        getFieldDBValue: function (fieldName) {
            return this.rowObj.getFieldDBValue(fieldName);
        },

        /**
         * Get field value
         * @param {any} fieldName The field name
         */
        getFieldValue: function (fieldName) {
            return this.rowObj.getFieldValue(fieldName);
        },

        /**
         * Get field value using colIndex
         * @param {any} colIndex 
         */
        getFieldValueByColumnIndex: function (colIndex) {
            return this.getFieldByColumnIndex(colIndex).value;
        },

        /**
         * Get field using colIndex
         * @param {any} colIndex 
         */
        getFieldByColumnIndex: function (colIndex) {
            return this.rowObj.rowNodes[0].Columns[colIndex];

        },

        /**
         * Get field previous value before change
         * @param {any} fieldName The field name
         */
        getFieldPreviousValue: function (fieldName) {
            return this.rowObj.getFieldPreviousValue(fieldName);
        },

        /**
         * Check whether field value is dirty
         * @param {any} fieldName  The field name
         */
        isFieldDirty: function (fieldName) {
            return this.rowObj.isFieldDirty(fieldName);
        },

        /**
         * Set field dirty flag
         * @param {any} fieldName
         * @param {any} isDirty
         */
        setFieldDirty: function (fieldName, isDirty) {
            this.rowObj.setFieldDirty(fieldName, isDirty);
        },

        /**
         * Triggers user & server messages events to make sure UI and business objects are updated
         * @param {any} mappedViewId View Id
         * @param {any} selectedRowIndex Selected row index
         * @param {any} mappedObject Mapped object
         * @param {any} fieldName The field name
         * @param {any} msgid The message id
         * @param {any} customBinding Custom binding function
         */
        toMapV2: function (mappedViewId, selectedRowIndex, mappedObject, fieldName, msgid, customBinding) {
            let mappedFields = apputils.keys(mappedObject);

            apputils.each(mappedFields, (field) => {
                let mappedToField = mappedObject[field][mappedViewId];

                let setValueForThisField = apputils.isUndefined(fieldName) ? true : fieldName === mappedToField;
                if (setValueForThisField) {
                    let column = this.getColumnByFieldName(field);
                    //let msg = mappedViewId + selectedRowIndex + mappedToField;
                    let msg = mappedViewId + msgid + mappedToField;

                    MessageBus.msg.trigger(msg + apputils.EventMsgTags.usrUpdate, { rowIndex: selectedRowIndex, field: mappedToField, value: column.value, customBinding: customBinding });

                    MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, column.value, msg + apputils.EventMsgTags.svrUpdate);
                }
            });
        },

        /**
         * Createc child entity
         * @param {any} name
         */
        createChildEntity: function (name) {

            let child = apputils.find(this.allCollectionObj, function (obj) {
                return obj.node === name;
            });

            if (apputils.isUndefined(child)) {
                return;
            }

            child.rowIsReadonly = this.rowIsReadonly;
            child.bindTrigger = this.bindTrigger;
            child.bindListener = this.bindListener;

            return child;
        },

        /**
         * Find collection object by view Id
         * @param {any} viewid The view Id
         */
        findCollectionObj: function (viewid) {
            return apputils.find(this.allCollectionObj, function (obj) {
                return obj.viewid === viewid;
            });
        },

        /**
         * Get grid data by viewId and namespace prefix
         * @param {any} viewid The view Id
         * @param {any} prefix Namespace prefix
         */
        getDataForGrid: function (viewid, prefix = "") {

            let thisObj = apputils.find(this.allCollectionObj, function (obj) {
                return obj.viewid === viewid;
            });

            if (apputils.isUndefined(thisObj)) {
                return [];
            }
            return thisObj.loadDataForGrid(prefix);
        },

        /**
         * Get grid data by viewId and namespace prefix
         * @param {any} viewid The view Id
         * @param {any} rowIndex Starting index where start collecting data
         * @param {any} length Ending index where to stop collecting data
         * @param {any} prefix Namespace prefix
         */
        getDataForGridEx: function (viewid, rowIndex, length, prefix = "") {

            let thisObj = apputils.find(this.allCollectionObj, function (obj) {
                return obj.viewid === viewid;
            });

            if (apputils.isUndefined(thisObj)) {
                return;
            }
            return thisObj.loadDataForGrid(prefix, rowIndex, length);
        },

        /** Get grid data from multiple source */
        getDataForGridFromMultiSource: function () {
            let data = [];

            //handle all nested collections for this entity
            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    data.push(obj.getDataForGridFromMultiSource());
                }
            });

            //handle this entity
            data.push(this.loadDataForGrid2());
            return _.flatten(data);
        },

        /**
         * copy row 
         * @param {any} viewId The view Id
         * @param {any} rowIndex The row index
         * @param {any} parentIndex Parent index
         */
        copyRow: function (viewId, rowIndex, parentIndex) {

            if (this.viewid === viewId) {
                return true;
            }

            var found = false;
            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    found = obj.copyRow(viewId, rowIndex, parentIndex);

                    if (found) return;
                }
            });

            return found;
        },

        /**
         * Add empty row
         * @param {any} viewId The view Id
         * @param {any} rowIndex The row index
         * @param {any} parentIndex Parent index
         */
        addEmptyRow: function (viewId, rowIndex, parentIndex) {

            if (this.viewid === viewId) {
                return true;
            }

            var found = false;
            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    found = obj.addEmptyRow(viewId, rowIndex, parentIndex);

                    if (found) return;
                }
            });

            return found;
        },

        /**
         * Update row index
         * @param {any} msgId Message Id
         * @param {any} currentRowIndex Current row index
         */
        updateRowIndex: function (msgId, currentRowIndex) {
            this.rowObj.updateRowIndex(msgId, currentRowIndex);
        },

        findRowAndMarkforDeletion: function (viewId, rowIndex, parentIndex) {

            if (this.viewid === viewId) {
                return true;
            }

            var found = false;
            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    found = obj.findRowAndMarkforDeletion(viewId, rowIndex, parentIndex);

                    if (found) return;
                }

            });

            return found;
        },

        /** Mark the delete action, set dirty flag as true */
        markforDeletion: function (deleteRecord=false) {
            //OCT 2 - make sure this flag sets the correct action
            this.CRUDReason = CRUDReasons.Deleting;

            if (!deleteRecord) this.setIsDirtyToTrue();

            this.markAllChildforDeletion(deleteRecord);
        },

        /** Mark all child for deletion */
        markAllChildforDeletion: function (deleteRecord=false) {
            apputils.each(this.allCollectionObj, (obj) => {
                if (!obj.skipInAllQuery) {
                    obj.findRowAndMarkforDeletion(obj.viewid, 0, deleteRecord);
                }
            });
        },

        /** 
         * tree row processor, implemented by override object
         * @param {any} Columns
         * @param {any} treeData
         */
        treeRowProcessor: function (Columns, treeData) {
            //"Not Implemented: should be implemented by the some object.";
        },

        /** Load tree view data */
        loadDataForTree: function () {
            let treeData = this.rowObj.loadDataForTree();

            let data = this.getAllRelatedEntityDataForTree();

            if (data.length > 0) {
                treeData.items = data;
            }

            return treeData;
        },

        /**
         * Load tree view items data
         * @param {any} treeData
         */
        loadDataForTreeItems: function (treeData) {
            
            let data = this.getAllRelatedEntityDataForTree();

            if (data.length > 0) {
                treeData.items = data;
            }
        },

        /**
         * Load data for tree view
         * @param {any} okArr
         * @param {any} level
         */
        loadDataForTree2: function (okArr, level) {
            let treeData = this.rowObj.loadDataForTree2(okArr, level);

            let data = this.getAllRelatedEntityDataForTree2(okArr, level+1);

            if (data.length > 0) {
                treeData.items = data;
            }

            return treeData;
        },

        /** Get all related entity data for tree view */
        getAllRelatedEntityDataForTree: function () {

            let items = [];
            apputils.each(this.allTreeViewsObj, function (obj) {
                let data = obj.loadDataForTree();

                if (obj.treeNode().text.length > 0) {
                    let node = obj.treeNode();
                    node.items = data.slice();
                    items.push(node);
                }
                else {
                    items = data.slice();
                }
            });

            return items;
        },

        /** Get all related entity data for tree view */
        getAllRelatedEntityDataForTree2: function (okArr, level) {

            let items = [];
            apputils.each(this.allTreeViewsObj, function (obj) {
                let data = obj.loadDataForTree2(okArr, level);

                if (obj.treeNode().text.length > 0) {
                    let node = obj.treeNode();
                    node.items = data.slice();
                    items.push(node);
                }
                else {
                    items = data.slice();
                }
            });

            return items;
        },

        /**
         * Get related entity data for tree view by view Id
         * @param {any} viewid View Id
         */
        getOneRelatedEntityDataForTree: function (viewid) {
            let thisObj = apputils.find(this.allTreeViews, function (obj) {
                return obj.viewid === viewid;
            });

            if (apputils.isUndefined(thisObj)) {
                return [];
            }

            return thisObj.loadDataForTree();
        },

        /** Check whether marked delete record */
        isMarkedForDeletion: function() {
            if (apputils.isUndefined(this.CRUDReason)) {
                return false;
            }
            return this.CRUDReason === CRUDReasons.Deleting;
        },

        /**
         * Check whether is marked for this action
         * @param {any} CRUDReason
         */
        isMarkedForThisAction: function(CRUDReason) {
            if (apputils.isUndefined(this.CRUDReason)) {
                return false;
            }
            return this.CRUDReason === CRUDReason;
        },

        updateFields: function () {
            //override to implement the updates
        },

        /**
         * Check whether this field is allowed update, return true or false
         * @param {any} fieldName
         */
        isUpdateAllowed: function (fieldName) {
            let column = this.getRowColumnByFieldName(this.rowObj.rowNodes[0], fieldName);
            return this.updateAllowed(column);
        },

        /**
         * Check whether Update is allowed, return true or false
         * @param {any} data
         */
        checkIfUpdateAllowed: function (data) {

            let updateAllowed = false;

            let child = apputils.find(this.allCollectionObj, function (obj) {
                updateAllowed = obj.checkIfUpdateAllowed(data);

                if (updateAllowed || updateAllowed.editAllowed) {
                    return true;
                } else {
                    return false;
                }
               
            });

            return updateAllowed;
        },

        /** Find dirty records */
        findDirtyRecords: function () {
            return this.rowObj.findDirtyRecords();

            //apputils.each(this.allCollectionObj, (childEntity) => {
            //    if (!childEntity.skipInAllQuery) {
            //        childEntity.findDirtyRecords();
            //    }
            //});
        },

        /**
         * Update empty, implemented in override method
         * @param {any} previousEntity
         */
        updateEmpty: function (previousEntity) {
            //override if needed
        },

        /**
         * Update dirty flag2
         * @param {any} newEntity
         */
        updateDirtyFlag2: function (newEntity) {

            if (apputils.isUndefined(newEntity)) {
                return;
            }

            newEntity.CRUDReason = this.CRUDReason;
            newEntity.updateEmpty(this, true);

            this.rowObj.updateDirtyFlag2(newEntity);

            apputils.each(this.allCollectionObj, (childEntity) => {

                if (!childEntity.skipInAllQuery) {
                    let newObj = newEntity.findObjectByViewId(childEntity.viewid);
                    childEntity.updateDirtyFlag2(newObj);
                }
            });
        },

        /**
         * Adjust deleted row(object)
         * @param {any} newEntity
         */
        adjustDeleted: function (newEntity) {

            if (apputils.isUndefined(newEntity)) {
                return;
            }

            apputils.each(this.allCollectionObj, (childEntity) => {

                if (!childEntity.skipInAllQuery) {
                    let newObj = newEntity.findObjectByViewId(childEntity.viewid);
                    childEntity.adjustDeleted(newObj);
                }
            });
        },

        /**
         * Set dirty flag
         * @param {any} fieldName
         */
        setDirtyFlag: function (fieldName) {
            this.rowObj.setDirtyFlag(fieldName);
        },

        /**
         * Find object by view Id
         * @param {any} viewid
         */
        findObjectByViewId: function (viewid) {

            let collectionEntity = apputils.find(this.allCollectionObj, function (obj) {
                return obj.findObjectByViewId(viewid);
            });

            return collectionEntity;
        },

        /** Check any records isDirty */
        isDirty: function () {
            let hasDirtyRecord = this.rowObj.isDirty();

            if (hasDirtyRecord) {
                return true;
            }

            apputils.find(this.allCollectionObj, (childEntity) => {
                if (apputils.isEmpty(childEntity)) {
                    hasDirtyRecord = false;
                } else {
                    hasDirtyRecord = childEntity.isDirty();
                }
                return hasDirtyRecord;
            });

            return hasDirtyRecord;
        },

        /**
         * Set field data
         * @param {any} fieldName The field name
         * @param {any} value The value
         */
        setFieldData: function (fieldName, value, isDirty) {
            let data = { column: this.getColumnByFieldName(fieldName), value: value, isDirty: isDirty };

            this.setColumnFieldValue(data);
        },

        /**
         * Set read only field data, the isDirty flag is false
         * @param {any} fieldName
         * @param {any} value
         */
        setReadOnlyFieldData: function (fieldName, value) {
            let data = { column: this.getColumnByFieldName(fieldName), value: value, isDirty: false };

            this.setColumnFieldValue(data);
        },

        /**
         * check fields for missing data
         * @param {any} fieldName
         */
        checkFieldsForMissingData: function (fieldName) {
            let column = this.getColumnByFieldName(fieldName);
            column.required = true;

            let columnData = {
                viewid: this.viewid,
                rowIndex: column.rowIndex,
                field: column.field,
                value: column.value
            };

            return this.handleUserUpdates(column, columnData);
        },

        checkFieldsOnlyForMissingData: function (fieldName) {
            let column = this.getColumnByFieldName(fieldName);

            let columnData = {
                viewid: this.viewid,
                rowIndex: column.rowIndex,
                field: column.field,
                value: column.value
            };

            let result = this.handleUserUpdatesButDontNotifyOthers(column, columnData);
            result.isValid = true; //set to true as default since no check is required
            return result;
        },

        /**
         * Handle user updates, trigger the message
         * @param {any} column
         * @param {any} data
         */
        handleUserUpdates: function (column, data) {
            
            if (data.value && !apputils.isNumber(data.value)) {
                data.value = data.value.trim();
            }

            //ValidateField sets the correct value & flags if value has not/passed validation rules
            let isValid = this.rowObj.ValidateField(column, data.value);

            let result = this.handleUserUpdatesButDontNotifyOthers(column, data);
            result.isValid = isValid;
            result.editable.previousValue = data.previousValue;
            result.editable.hasError = data.hasError;

            if (result.editable.editingCell) {
                result.editable.editingCell.title = result.editable.editingCell.title || column.title;
            }

            if (column.required && data.value === "") {
                result.editable.invalid = true;
            }

            MessageBus.msg.trigger(this.viewid + "handleUserUpdates", result.editable);
            return result;
        },

        /**
         * Handle user updates, but dont notify others
         * @param {any} column
         * @param {any} data
         */
        handleUserUpdatesButDontNotifyOthers: function (column, data) {

            //even if valid or not, check is editing is allowed
            let columnData = {
                viewid: this.viewid,
                rowIndex: column.rowIndex,
                field: column.field,
                value: data.value
            };

            let result = this.updateAllowed(columnData);

            result.msgid = column.msgid;
            result.viewid = this.viewid;

            //pass in the cell that started the editing
            result.editingCell = columnData;

            return { editable: result };
        },

        //TODO: refactor - move the complicated "filter" logic below into fiscalCalendars object class
        
        /**
         * Update the fiscal year and period based  on date value
         * @param {any} entity
         * @param {any} date
         */
        setFiscalYearPeriod(entity, date) {
            let yearName = 'FISCALYEAR';
            let periodName = 'FISCALPER';

            let fields = Object.keys(this.dataModelObj).filter(v => v.includes('FISC') && v.includes('YEAR') && !v.startsWith('attr'));
            if (fields.length > 0) {
                yearName = fields[0];
            }

            fields = Object.keys(this.dataModelObj).filter(v => v.includes('FISC') && v.includes('PER') && !v.startsWith('attr'));
            if (fields.length > 0) {
                periodName = fields[0];
            }

            let year = new Date(date).getFullYear();
            let period = new Date(date).getMonth() + 1;
            //For warning, the new entity object is null, get fiscal year and period from FiscalCalendar by date 
            if (date) {
                let rows = apputils.fiscalCalendars.rows;
                let rs = rows.filter(r => {
                    const cols = r.rowObj.rowNodes[0].Columns;
                    for (let i = 0; i < 13; i++) {
                        let dtDate = new Date(date);
                        let startDate = new Date(cols[5 + i].value);
                        let endDate = new Date(cols[18 + i].value);
                        if (dtDate >= startDate && dtDate <= endDate) {
                            period = (i + 1).toString();
                            return r;
                            //break;
                        }
                    }
                });
                if (rs.length === 1) {
                    year = rs[0].rowObj.rowNodes[0].Columns[1].value;
                }
            } else {
                year = this.getRowColumnByFieldName(entity, yearName).value;
                period = this.getRowColumnByFieldName(entity, periodName).value;
            }
            this.setReadOnlyFieldData(yearName, year);
            this.setReadOnlyFieldData(periodName, period);
        },

        /**
         * Update the query, only include currently changed field in query and remove child insert/put verb node.
         * @param {any} query
         * @param {any} value
         */
        updateQuery: function (query, value) {
            const domParser = new DOMParser();
            const s = new XMLSerializer();
            const xmlDoc = domParser.parseFromString(query, "text/xml");
            let nodes = xmlDoc.getElementsByTagName("c");
            let update = false;

            for (const node of nodes) {
                if (node.attributes['v'].nodeValue !== value) {
                    update = true;
                    node.remove();
                }
            }

            nodes = xmlDoc.getElementsByTagName("r");
            let nodesToRemove = [];

            //Sanitizing query, remove insert/put verb nodes that not needed. 
            for (let i = 0; i < nodes.length; i++) {
                if (nodes[i].getAttribute("verb") !== 'InitOnly') {
                    nodesToRemove.push(nodes[i]);
                }
            }
            for (let i = 0; i < nodesToRemove.length; i++) {
                nodesToRemove[i].parentNode.removeChild(nodesToRemove[i]);
            }

            return update ? s.serializeToString(xmlDoc).replaceAll('"',"'") : query;
        },

        /**
         * Update transaction date field
         * @param {any} column
         * @param {any} data
         * @param {any} collectionObj
         * @param {any} useVerbPut
         * @param {any} lockLevel
         * @param {any} setFiscalYearPeriod
         */
        updateTransactionDateField: function (column, data, collectionObj, useVerbPut, lockLevel, setFiscalYearPeriod = true) {
            this.setFieldData(column.field, data.value);

            let newEntityColl = new collectionObj();
            newEntityColl.rowIsReadonly = true;
            let query = newEntityColl.generateInitOnlyRoot(this);

            if (query.length === 0) return;

            query = this.updateQuery(query, data.value);
            if (useVerbPut) {
                query = query.replaceAll('InitOnly', 'Put');
            }

            let Ok = newEntityColl._executeXSearch2(query);

            if (apputils.isUndefined(Ok)) {
                return "";
            }
            let self = this;
            Ok.then((result, status, xhr) => {
                let message = {"UserMessage": { "IsEmail": false, "IsSuccess": false, "Message": "", "Errors": null, "Warnings": null, "Info": null}};
                if (xhr.hasError) {
                    const errors = ErrorEntityCollectionObj.getErrors();
                    message.UserMessage.Errors = errors;

                    if (errors.length > 0) {
                        self.setTransactionDateValue(column.field, data.previousValue, true);
                        let msg = this.viewid + column.msgid + column.field;
                        MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.previousValue, msg + apputils.EventMsgTags.svrUpdate);

                    } else if (setFiscalYearPeriod) {
                        this.setFiscalYearPeriod(null, data.value);
                    }

                    if (lockLevel) {
                        this.showWarningLockedPeriod(column, data, lockLevel, setFiscalYearPeriod);
                    } else {
                        sg.utls.showMessage(message, () => ErrorEntityCollectionObj.clearError());
                    }

                } else {
                    if (this.rowObj.dataModel.filter(c => c.field === 'DATEBUS').length === 1 && column.field !== "DATEBUS") {
                        this.setFieldData("DATEBUS", data.value);
                    }

                    if (setFiscalYearPeriod) {
                        let entity = newEntityColl.rows[0] ? newEntityColl.rows[0].rowObj.rowNodes[0] : null;
                        this.setFiscalYearPeriod(entity, data.value);
                    }

                    if (lockLevel > 0 && useVerbPut) {
                        this.showWarningLockedPeriod(column, data, lockLevel);
                    }
                    const warnings = ErrorEntityCollectionObj.getWarnings();
                    if (warnings && warnings.length > 0) {
                        const name = lockLevel === 1 ? 'Warnings' : 'Errors';

                        warnings[0].Message = warnings[0].Message.replace(globalResource.Warning + '. ', '');
                        message.UserMessage[name] = warnings;
                        sg.utls.showMessage(message, () => ErrorEntityCollectionObj.clearError());

                        if (lockLevel === 2) {
                            self.setTransactionDateValue(column.field, data.previousValue, setFiscalYearPeriod);
                        }
                    }
                }

            });
        },
        /**
         * Set transaction date value, posting date value and fiscal year/period value
         * @param {any} fieldName
         * @param {any} value
         */
        setTransactionDateValue: function (fieldName, value, setYearPeriod) {
            this.setFieldData(fieldName, value);
            if (fieldName !== "DATEBUS") {
                if (this.rowObj.dataModel.filter(c => c.field === 'DATEBUS').length === 1) {
                    this.setFieldData("DATEBUS", value);
                }
            }
            if (setYearPeriod) {
                this.setFiscalYearPeriod(this.rowObj.rowNodes[0], value);
            }
        },

        /**
         * Show warning locked period by profile lock level settings
         * @param {any} column date field column
         * @param {any} data date 
         * @param {any} lockLevel company profile lock level value 
         * @param {any} setFiscalYearPeriod whether to set fiscal year/period boolean value
         */
        showWarningLockedPeriod: function (column, data, lockLevel, setFiscalYearPeriod) {
            let year = new Date(data.value).getFullYear().toString();
            let period = new Date(data.value).getMonth() + 1;
            let module = this.rowObj.viewid.slice(0, 2);
            let locked = false;

            let status = apputils.fiscalCalendars.entity.allCollectionObj.CS0060FiscalStatusesEntityV3Coll.rows;
            let rs = status.filter(r => r.rowObj.rowNodes[0].Columns[1].value === year && r.rowObj.rowNodes[0].Columns[2].value == module);
            if (rs.length > 0) {
                locked = rs[0].rowObj.rowNodes[0].Columns[2 + period].value === globalResource.Locked;
            }

            if (locked && lockLevel > 0) {
                let message = {};
                let msg = kendo.format(globalResource.LockedPeriod, period, year, globalResource[module]);
                if (lockLevel === 1) {
                    message = { "UserMessage": { "IsSuccess": false, "Warnings": [{ "Message": msg }] } };
                } else {
                    message = { "UserMessage": { "IsSuccess": false, "Errors": [{ "Message": msg }] } };
                }
                sg.utls.showMessage(message, () => ErrorEntityCollectionObj.clearError());
                if (lockLevel === 2) {
                    this.setTransactionDateValue(column.field, data.previousValue, setFiscalYearPeriod);
                }
            }
        },

        /**
         * Handle user change the date field(transaction/posting date)
         * @param {any} column The field column
         * @param {any} data Date
         * @param {any} viewModel View Model
         * @param {any} resources Local resource JS object
         * @param {any} collectionObj Collection object 
         * @param {any} checkPostingDateRange Boolean value to indicate whether check posting date range
         * @param {any} useVerbPut Boolean value to indicate whether is Put action
         * @param {any} setFiscalYearPeriod Boolean value to indicate whether is to set fiscal year period
         */
        baseHandleUserUpdatesForDate: function (column, data, viewModel, resources, collectionObj, checkPostingDateRange = false, useVerbPut = false, setFiscalYearPeriod = true) {
            const value = data.value;
            data.previousValue = column.value;

            let isValid = this.rowObj.ValidateField(column, data.value);
            if (!isValid) {
                let msg = this.viewid + column.msgid + column.field;
                MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.previousValue, msg + apputils.EventMsgTags.svrUpdate);
                return;
            }

            if (sg.utls.kndoUI.checkForValidDate(value)) {
                const transDate = Date.parse(value);
                const sessionDate = Date.parse(viewModel.SessionDate) || apputils.getSessionDate();
                const warningDays = viewModel.SessionWarnDays || viewModel.WarningDateRange || parseInt(apputils.getWarningDays()) || 30;
                const lockLevel = parseInt(apputils.getLockedPeriodLevel());

                //convert the date difference to days then compare with session warning days
                let checkWarning = column.field === this.dataModelObj.TRANSDATE.field || checkPostingDateRange;

                const showWarning = checkWarning && (Math.abs((transDate - sessionDate) / (1000 * 3600 * 24)) > warningDays);

                if (showWarning) {
                    sg.utls.showConfirmationDialogYesNo(() => {
                        this.updateTransactionDateField(column, data, collectionObj, useVerbPut, lockLevel, setFiscalYearPeriod)
                    }, () => {
                        let msg = this.viewid + column.msgid + column.field;
                        MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, data.previousValue, msg + apputils.EventMsgTags.svrUpdate);
                    },
                    globalResource.TransactionDateOutOfRange, globalResource.ConfirmationTitle, '', '', true);
                } else {
                    this.updateTransactionDateField(column, data, collectionObj, useVerbPut, lockLevel, setFiscalYearPeriod);
                }
            }
        },

        /**
         * Reset invalid input
         * @param {any} column
         * @param {any} data
         */
        resetInvalidInput: function (column, data) {
            let property = this.getColumnByFieldName(column.field);

            delete property.errorValue;
            delete property.validationError;
            property.value = data.value;
        },

        /** Check whether has error, return true or false */
        hasError: function () {
            if (this.rowObj.hasError()) {
                return true;
            }

            let collectionEntity = apputils.find(this.allCollectionObj, function (obj) {
                return obj.hasError() === true;
            });

            if (apputils.isUndefined(collectionEntity)) {
                return false;
            }

            return true;
        },

        /**
         * Set namespace
         * @param {any} prefix Namespace prefix
         */
        setPrefixNamespaceForAll: function (prefix) {
            this.prefixNamespace = prefix + this.viewid;
            
            this.rowObj.prefixNamespace = this.prefixNamespace;

            apputils.each(this.allCollectionObj, (relatedObj) => {
                if (!relatedObj.skipInAllQuery) {
                    relatedObj.setPrefixNamespaceForAll(prefix);
                }
            });
        },

        setPreviousValueAndResetIsDirtyFlag: function (fieldName, value) {
            let property = this.getColumnByFieldName(fieldName);

            let newValue = apputils.isUndefined(property.previousValue) ? "" : property.previousValue;

            if (value) {
                newValue = value;
            }            

            let data = { column: property, value: newValue, isDirty: false };
            
            this.setColumnFieldValue(data);
            
        },

        /**
         * Check whether column value allow update, return true or false
         * @param {any} columnData Column data
         */
        updateAllowed: function (columnData) {

            let self = this;

            let request = function (column) {
                this.column = column;
                this.next = null;
            };

            request.prototype = {
                validate: function (columnNameToValidate) {

                    if (apputils.isUndefined(columnNameToValidate)) {
                        return { editAllowed: true, invalid: false };
                    }

                    let columnToValidate = self.getRowColumnByFieldName(self.rowObj.rowNodes[0], this.column);

                    if (apputils.isUndefined(columnToValidate.value)) columnToValidate.value = "";

                    //editing and validating same field
                    let sameField = this.column === columnNameToValidate.field;

                    //if not validate return
                    if (/*columnToValidate.value &&*/ (columnToValidate.value.length === 0 || columnToValidate.validationError)) {
                        let formattedText = apputils.isFunction(columnToValidate.formattedDisplay) ? columnToValidate.formattedDisplay() : "";
                        return { editAllowed: sameField, invalid: columnToValidate.validationError, field: columnToValidate.field, title: columnToValidate.title, formattedDisplay: formattedText, errorValue: columnToValidate.errorValue, value: columnToValidate.value };
                    }

                    //validate pass to next
                    if (!sameField && this.next) {
                        return this.next.validate(columnNameToValidate);
                    }

                    return { editAllowed: true, invalid: false, field: this.column };

                },

                setNextField: function (field) {
                    this.next = field;
                },
            };

            this.fieldValidator = this.createContractObjectsAndSetHierarchy(request, columnData);
            return this.fieldValidator.validate(columnData);

        },

        /**
         * Clear dependant field value
         * @param {any} field The field
         * @param {any} isDirty isDirty flag, true or false
         */
        clearDependantField: function (field, isDirty=true) {
            let property = this.getColumnByFieldName(field);
            let updates = { value: "", column: property, isDirty: isDirty };
            this.setColumnFieldValue(updates);
        },

        /**
         * Clear read only dependant field value
         * @param {any} field
         */
        clearReadOnlyDependantField: function (field) {
            this.clearDependantField(field, false)
        },

        //override this function as needed to implement input field dependencies.
        createContractObjectsAndSetHierarchy: function(validatorObj, columnData) {
            return new validatorObj(columnData.field);
        },

        /**
         * Copy field values from another entity object
         * @param {any} fromEntity
         */
        copyFieldValuesFromAnotherEntity: function (fromEntity) {

            if (apputils.isUndefined(fromEntity)) {
                return;
            }
            const copyToThisEntity = this;

            /*Not sure if needed
            const filterCopy = this.getFilter(copyToThisEntity.rowObj.rowNodes[0]);
            const filterFrom = this.getFilter(fromEntity.rowObj.rowNodes[0]);

            if (filterCopy !== filterFrom) {
                return;
            }
            */
            fromEntity.updateEmpty(copyToThisEntity, true);
            copyToThisEntity.rowObj.copyFieldValuesFromAnotherEntity(fromEntity);
            
            apputils.each(fromEntity.allCollectionObj, (childFromEntity) => {

                const toObj = copyToThisEntity.findObjectByViewId(childFromEntity.viewid);
                toObj.copyFieldValuesFromChildEntity(childFromEntity);

            });
        },

        /** Get bulk navigation filter */
        getBulkNavigationFilter: function () {

            return this.getFilter(this.rowObj.rowNodes[0], apputils.Operators.MoreThan);
        },

        /** Return empty string for init filter */
        getInitFilter: function () {
            return "f='' ";
        },

        /**
         * Adjust total row count after lazy retrieve
         * @param {any} totalRowCount
         */
        adjustTotalRowCountAfterLazyRetrieve: function (totalRowCount) {
            const rowCountField = this.dataModelObj.TotalRowCount.field;

            if (rowCountField) {
                const field = this.getColumnByFieldName(rowCountField);
                field.value = totalRowCount;
            }           
        },

        //property to hold list of many fields query strings
        fieldQueries: "",

        //property to hold list of the last fields query string
        fieldQuery: "",

        resetInvalidInitQuery: function () {
            this.fieldQueries = this.fieldQueries.replaceAll(this.fieldQuery, "");
        },

        /**
         * Update current row refreshing entity data
         * @param {object} newEntity The entity containing new data row
         * @param {function} getRowIndexFnc This to match the starting node where data has changed
         * @param {boolean} copyChildObj This controls updating all child objects once the updated entity is found
         */
        refreshUI: function (newEntity, getRowIndexFnc, copyChildObj) {

            let excludeList = [this.dataModelObj.RowIndex.field];

            if (this.dataModelObj.LINENOForDisplay) excludeList = excludeList.concat(this.dataModelObj.LINENOForDisplay.field);
            if (this.dataModelObj.TotalRowCount) excludeList = excludeList.concat(this.dataModelObj.TotalRowCount.field);

            let propertiesToUpdate = [];
            apputils.keys(this.dataModelObj).forEach(e => {
                //if (!(apputils.isUndefined(this.dataModelObj[e].field) || [this.dataModelObj.RowIndex.field /*, this.dataModelObj.LINENOForDisplay.field, this.dataModelObj.COMMENTS.field*/].includes(this.dataModelObj[e].field))) {
                if (!(apputils.isUndefined(this.dataModelObj[e].field) || excludeList.includes(this.dataModelObj[e].field))) {
                    propertiesToUpdate.push(this.dataModelObj[e].field);
                }


            });

            propertiesToUpdate.forEach((property) => {
                const currentProperty = this.getColumnByFieldName(property);
                if (apputils.isUndefined(currentProperty.isDirty)) currentProperty.isDirty = false;

                this.setFieldData(currentProperty.field, newEntity.getRowColumnByFieldName(newEntity.rowObj.rowNodes[0], currentProperty.field).value, currentProperty.isDirty);
                currentProperty.addToInitQuery = false;
            });

            //once the collection entity is found there's no need to process other objects in allCollectionObj
            //so using entityObjfound since there's no easy way to break _.each.
            let entityObjfound = false;
            apputils.each(this.allCollectionObj, obj => {

                if (!entityObjfound) {
                    const newObj = newEntity.allCollectionObj[obj.node + 'Coll'];
                    entityObjfound = obj.refreshUI(newObj, getRowIndexFnc, copyChildObj);
                }
                
            });
        }

    };

    this.baseObject = helpers.View.extend(domainObject);
    this.baseStaticObject = helpers.View.extend({}, domainObject);

}).call(this);