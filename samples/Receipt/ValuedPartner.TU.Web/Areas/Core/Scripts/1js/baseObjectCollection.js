(function () {
    'use strict';

    /** Base object collection object */
    var domainObjectCollection = {
        rows: [],

        //This property allows to auto-bind columns to fields from update from server, user interaction or calculated fields
        //Set this flag to true for Finder since its retrieved data that will never get updated
        rowIsReadonly: false,

        /** Should be implemented by the specific object*/
        getEntity: function () {
            throw "Not Implemented: getEntity should be implemented by the some object.";
        },

        /**
         * Ajax call
         * @param {any} method Call verb
         * @param {any} url Ajax call url
         * @param {any} query Query string
         */
        ajaxCall: function (method, url, query) {

            let self = this;
            method = "Post";
            let config = query ? { async: true, url: url, type: method, data: { query: query } } : { async: true, url: url, type: method };

            return $.ajax(config).done(function (result, status, xhr) {

                if (result.indexOf("<errors>") > -1) {
                    trace.error(result);
                    ErrorEntityCollectionObj.clearError();
                    ErrorEntityCollectionObj.ParseEntity(result);
                    xhr.hasError = !ErrorEntityCollectionObj.isErrorAWarning();
                }

                if (xhr.hasError) {
                    return;
                }

                self.ParseResponse(result);

            }).fail(function (result, error, xhr) {
                trace.error(`error:${JSON.stringify(result)}`);
            });
        },

        /**
         * Parse response
         * @param {any} resp response
         */
        ParseResponse: function (resp) {
            let tempPath = this.xmlPath;
            this.xmlPath = appconfig.response.tag + this.xmlPath;

            this.ParseEntity(resp);

            this.xmlPath = tempPath;
        },

        /**
         * Parse entity
         * @param {any} data
         */
        ParseEntity: function (data) {

            if (data.length === 0) {
                return;
            }

            let parser = new DOMParser();

            let xmlDoc = parser.parseFromString(data, "text/xml");

            let iterator = xmlDoc.evaluate(this.xmlPath, xmlDoc, null, XPathResult.UNORDERED_NODE_ITERATOR_TYPE, null);

            try {
                let thisNode = iterator.iterateNext();

                while (thisNode) {
                    this.populate(thisNode);

                    thisNode = iterator.iterateNext();
                }
            } catch (e) {
                trace.throwError(e);
                alert('Error: Document tree modified during iteration ' + e);
            } finally {
                MessageBus.msg.trigger("serverCallComplete", true);
            }
        },

        /**
         *  Populate entity object
         * @param {any} xml Xml dom object
         * @param {any} rowIndex Current row index
         */
        populate: function (xml, rowIndex) {
            this.rows = [];
            rowIndex = rowIndex || "";

            apputils.each(xml.childNodes, (child) => {
                let entity = this.getEntity();

                //Oct 2 - since populate data from db or views set default action
                //this default flag will be changed when template data is 
                //loaded by the entity collections objects
                entity.CRUDReason = CRUDReasons.ExistingData;

                //entity.populateCompositions(child, this.rows.length);
                entity.populateCompositions(child, this.rows.length, this.rows.length + rowIndex);
                this.rows.push(entity);

            });
        },

        /**
         * Find row by index and make it the only row, deleting others, then bind the found row to UI by entity calling column triggers
         * @param {any} rowIndex The row index
         * @param {any} prefix Namespace prefix
         */
        selectDataByRowIndex: function (rowIndex, prefix) {
            this.rows = this.findRowByIndex(rowIndex);
            this.bindThisRowToUI(prefix);
        },

        /**
         * Find Row by row index
         * @param {any} rowIndex The row index
         */
        findRowByIndex: function (rowIndex) {
            let indexRow = apputils.filter(this.rows, (row) => {
                return !apputils.isUndefined(row.findRowByIndex(rowIndex));
            });
            return indexRow;
        },


        /**
         * Bind the found row to UI by entity calling column triggers
         * @param {any} prefix Namespace prefix
         */
        bindThisRowToUI: function (prefix) {
            let entity = this.setEntityToFirstRow();

            if (apputils.isUndefined(entity)) {
                return;
            }
            entity.selectDataByRowIndex(prefix);
        },

        /** Set entity to first row, should be only one header row for screen  */
        setEntityToFirstRow: function () {

            //there should be only one row; now we need to bind the data to UI
            if (this.rows.length === 0 || apputils.isUndefined(this.rows[0])) {
                trace.log("No row found.");
                return;
            }

            if (this.rows.length > 1) {
                throw "Cannot bind multiple rows.";
            }

            let entity = this.rows[0];

            entity.rowIsReadonly = this.rowIsReadonly;
            entity.bindListener = this.bindListener;

            return entity;
        },

        /** Load rows data for grid */
        getDataForGrid: function () {
            let data;
            apputils.each(this.rows, (entity) => {
                data = entity.getDataForGrid();
            });
            return data;
        },

        /**
         * Loads empty row from data model
         * @param {any} prefix Namespace prefix
         */
        loadDataForGrid: function (prefix = "", rowIndex = -1, length = this.rows.length) {
            let gridArray = [];

            /*apputils.each(this.rows, (entity) => {
                entity.loadDataForGrid(gridArray, prefix);
            });*/

            if (length > this.rows.length) {
                length = this.rows.length;
            }

            if (rowIndex > 0) {

                for (let i = rowIndex; i < length; i++) {
                    const entity = this.rows[i];

                    if (apputils.isDefined(entity)) {
                        entity.loadDataForGrid(gridArray, prefix);
                    }
                    
                }

            } else {
                for (const entity of this.rows) {

                    if (apputils.isDefined(entity)) {
                        entity.loadDataForGrid(gridArray, prefix);
                    }
                }
            }

            return gridArray;
        },

        /**
         * Iterate all rows then bind the rows to UI by entity calling column triggers
         * @param {any} prefix Namespace prix or index
         */
        selectRowByIndex: function (prefix) {

            apputils.each(this.rows, (entity) => {
                entity.rowIsReadonly = this.rowIsReadonly;
                entity.bindListener = this.bindListener;
                entity.init();
                entity.selectRowByIndex(prefix);
            });
        },

        /**
         * Generate Upsert data root query string
         * @param {any} verb
         */
        generateUpsertDataRoot: function (verb) {
            
            let query = apputils.rootNodeTemplate; //""<n t='' n=''>";
            
            query += this.generateUpsertData("", verb);

            if (query === apputils.rootNodeTemplate /*"<n t='' n='" + verb + "'>"*/) {
                return;
            }

            query += apputils.rootNodeClose;

            return query;
        },

        /**
         * Generate Upsert data query string
         * @param {any} parentIndex The parent index
         * @param {any} verb Action verb
         */
        generateUpsertData: function (parentIndex, verb) {
            let query = "";
            let index = 0;

            //Post - should only produce header query therefore the document must be saved first.
            if (this.rows.length === 1 && verb === CRUDReasons.PostData) {
                let id = parentIndex + "" + index;
                let entity = this.rows[0];
                return entity.buildFilterQuery(entity.rowObj.rowNodes[0], parentIndex, id, verb, "")
                
            }

            this.rows.forEach((entity) => {
                let children = entity.generateUpsertData(parentIndex, verb, index);
                if (children.length > 0) {
                    query += children;
                }
                index++;
            });

            return query;
        },

        /** Generate getTemplate root query string*/
        generateGetTemplateRoot: function () {
            //let verb = CRUDReasons.InitData; //CRUDReasons.GetTemplate; // AT-74445, in some cases, it change the rowIndex from 0 to 1
            let verb = CRUDReasons.GetTemplate;
            let query = apputils.rootNodeTemplate; //"<n t='' n='" + verb + "'>";

            query += this.generateGetTemplateQuery("", verb);

            if (query === apputils.rootNodeTemplate /*"<n t='' n='" + verb + "'>"*/) {
                //nothing to do
                return;
            }

            query += apputils.rootNodeClose;

            return query;
        },

        /**
         * Generate getTemplate query string
         * @param {any} parentIndex The parent index
         * @param {any} verb Action verb
         * @param {any} childIndex The child index
         */
        generateGetTemplateQuery: function (parentIndex, verb, childIndex = "0") {
            let query = "";
            let index = childIndex;

            let entity = this.getEntity(verb);
            let children = entity.generateGetTemplateQuery(parentIndex, verb, index);

            if (children.length > 0) {
                query += children;
            }

            return query;
        },

        /**
         * Generate get root query string, 'root' if for the main UI
         * @param {any} filterCallback
         * @param {any} depthViewId
         */
        generateGetRoot: function (filterCallback, depthViewId="") {
            let verb = CRUDReasons.Get;
            
            let query = apputils.rootNodeTemplate; //"<n t='' n='" + verb + "'>";

            let entity = this.getEntity(verb);
            let children = entity.generateGetQuery("", verb, filterCallback, depthViewId);

            if (children.length > 0) {
                query += children;
            }

            if (query === apputils.rootNodeTemplate /*"<n t='' n='" + verb + "'>"*/) {
                return;
            }

            query += apputils.rootNodeClose; //"</n>";

            return query;
        },

        /**
         * Generate get query string, used for child(details) entity
         * @param {any} parentIndex The parent index
         * @param {any} verb Action verb
         * @param {any} filterCallback Filter call back function
         * @param {any} depthViewId ViewId
         */
        generateGetQuery: function (parentIndex, verb, filterCallback, depthViewId, childIndex="1") {
            let query = "";
            verb = CRUDReasons.Get; //suppressed the generic use of this function

            let entity = this.getEntity(verb);

            let children = entity.generateGetQuery(parentIndex, verb, filterCallback, depthViewId, childIndex);

            if (children.length > 0) {
                query += children;
            }

            return query;
        },

        /**
         * Generate Navigation VCR root string, return 'root' if for the main UI
         * @param {any} filterCallback
         * @param {any} depthViewId
         * @param {any} verb
         */
        generateVCRRoot: function (filterCallback, depthViewId, verb) {
            
            let query = apputils.rootNodeTemplate; //"<n t='' n='" + verb + "'>";

            let entity = this.getEntity(verb);
            let children = entity.generateGetQuery("", verb, filterCallback, depthViewId);

            if (children.length > 0) {
                query += children;
            }

            if (query === apputils.rootNodeTemplate /*"<n t='' n='" + verb + "'>"*/) {
                //nothing to do
                return;
            }

            query += apputils.rootNodeClose;

            return query;
        },

        /** Set IsDirty falg to false*/
        setIsDirtyToFalse: function () {
            apputils.each(this.rows, (entity) => {
                entity.setIsDirtyToFalse();
            });
        },

        /** Set IsDirty falg to true*/
        setIsDirtyToTrue: function () {
            apputils.each(this.rows, (entity) => {
                entity.setIsDirtyToTrue();
            });
        },

        /**
         * Triggers user & server messages events to make sure UI and business objects are updated
         * @param {any} mappedViewId The view Id
         * @param {any} selectedRowIndex Selected row index
         */
        toMap: function (mappedViewId, selectedRowIndex) {
            apputils.each(this.rows, (entity) => {
                entity.toMap(mappedViewId, selectedRowIndex);
            });
        },

        /**
         * Triggers user & server messages events to make sure UI and business objects are updated
         * @param {any} mappedViewId The view Id
         * @param {any} selectedRowIndex Selected row index
         * @param {any} mappedObject Mapped object
         * @param {any} fieldName The field name
         * @param {any} msgid Message Id
         * @param {any} customBinding Custom binding
         */
        toMapV2: function (mappedViewId, selectedRowIndex, mappedObject, fieldName, msgid, customBinding) {
            apputils.each(this.rows, (entity) => {
                entity.toMapV2(mappedViewId, selectedRowIndex, mappedObject, fieldName, msgid, customBinding);
            });
        },

        /** Execute updates using Promise */
        executeUpdatesUsingPromise: function () {
            //using first is enough to kickoff all the awaits for Promises to finish
            return this.rows[0].executeUpdatesUsingPromise();
        },

        /**
         *  Get field value
         * @param {any} fieldName The field name
         * @param {any} rowIndex The row index
         */
        getFieldValue: function (fieldName, rowIndex=0) {
            return this.fields(fieldName, rowIndex).value; //this.rows[rowIndex].getFieldValue(fieldName);
        },
        /**
         * Get column by field name and row index
         * @param {any} fieldName The field name
         * @param {any} rowIndex The row index
         */
        getColumnByFieldName: function (fieldName, rowIndex = 0) {
            const column = this.rows[rowIndex].getColumnByFieldName(fieldName);
            column.viewid = this.viewid;

            return column;
        },
        fields: function (fieldName, rowIndex = 0) {

            if (this.rows.length <= rowIndex) {
                trace.warn(`fields - ${fieldName} not found at index - ${rowIndex}, using dataModel.`);
                return apputils.find(this.dataModel, (column) => {
                    return column.field === fieldName;
                });
            }

            let column = this.getColumnByFieldName(fieldName, rowIndex);

            if (apputils.isUndefined(column.FlushValue)) {
                //Flushes the current field value to the View, if the current field is dirty.
                //Override this function if needed
                column.FlushValue = function (verify) {
                    //maybe need to set the field with verify flag
                    //then pass to View
                };
            }

            if (apputils.isUndefined(column.RefreshValue)) {
                //Forces the UI field value to be refreshed from the View
                //Override this function if needed
                column.RefreshValue = function (v) {
                    
                };
            }

            
            return column;
        },

        /**
         * Get field previous value before field value change
         * @param {any} fieldName The field name
         * @param {any} rowIndex The row index
         */
        getFieldPreviousValue: function (fieldName, rowIndex) {
            return this.rows[rowIndex].getFieldPreviousValue(fieldName);
        },

        /**
         * Check whether field is dirty by field name and row index
         * @param {any} fieldName The field name
         * @param {any} rowIndex The row index
         */
        isFieldDirty: function (fieldName, rowIndex) {
            return this.rows[rowIndex].isFieldDirty(fieldName);
        },

        /** Load finder columns */
        loadFinderColumns: function () {
            let entity = this.getEntity();

            let finderColumns = entity.loadFinderColumns();

            entity = null;
            return finderColumns;
        },

        /**
         *  Copy row
         * @param {any} viewId The view Id
         * @param {any} rowIndex The row index
         * @param {any} parentIndex The parent index
         */
        copyRow: function (viewId, rowIndex, parentIndex) {

            parentIndex = apputils.isUndefined(parentIndex) ? "" : parentIndex + "";

            if (this.viewid === viewId && this.rows.length === 0) {
                this.populateWithEmptyEntityModel(parentIndex, -1);
                return true;
            }

            //since deleted rows cannot be selected, user has clicked add without any selection therefore add a blank row
            if (this.viewid === viewId && this.rows[rowIndex].CRUDReason === CRUDReasons.Deleting) {
                this.populateWithEmptyEntityModel(parentIndex, -1);
                return true;
            }

            if (this.viewid === viewId && rowIndex < this.rows.length) {
                this.populateNewRow(rowIndex, this.rows[rowIndex], parentIndex);
                return true;
            }

            var found = false;

            for(let i = 0; i < this.rows.length; i++) {

                found = this.rows[i].copyRow(viewId, rowIndex, i);

                if (found) break;
            }

            return found;
        },

        /**
         * Add empty row
         * @param {String} viewId The view Id
         * @param {int} rowIndex The row index
         * @param {int} parentIndex The parent index
         */
        addEmptyRow: function (viewId, rowIndex, parentIndex) {

            parentIndex = apputils.isUndefined(parentIndex) ? "" : parentIndex + "";

            if (this.viewid === viewId && this.rows.length === 0) {
                this.populateWithEmptyEntityModel(parentIndex, -1);
                return true;
            }

            //since deleted rows cannot be selected, user has clicked add without any selection therefore add a blank row
            if (this.viewid === viewId && this.rows[rowIndex].CRUDReason === CRUDReasons.Deleting) {
                this.populateWithEmptyEntityModel(parentIndex, -1);
                return true;
            }

            if (this.viewid === viewId && rowIndex < this.rows.length) {
                
                this.populateWithEmptyEntityModel(parentIndex, rowIndex);
                
                this.rows[rowIndex + 1].updateEmpty(this.rows[rowIndex]);
                return true;
            }

            var found = false;

            for (let i = 0; i < this.rows.length; i++) {

                found = this.rows[i].addEmptyRow(viewId, rowIndex, i);

                if (found) break;
            }

            return found;
        },

        /**
         * Populate empty entity model
         * @param {any} parentIndex The parent index
         * @param {any} rowIndex The row index
         */
        populateWithEmptyEntityModel: function (parentIndex, rowIndex = -1) {
            let entity = this.getEntity();
            entity.initRowObj();
            
            //OCt 2 - creating new object, set this flag
            //entity.CRUDReason = CRUDReasons.AddingNewData;
            entity.populateEmptyCompositions(this.rows.length, parentIndex);
            
            this.populateNewRow(rowIndex, entity, parentIndex);
        },

        /**
         * Populate new row by cloning existing row
         * @param {any} currentRowIndex Current row index
         * @param {any} entity
         * @param {any} parentIndex The parent index
         */
        populateNewRow: function (currentRowIndex, entity, parentIndex) {

            if (entity.CRUDReason === CRUDReasons.Deleting) {
                return false;
            }

            let cloneEntity = (currentRowIndex === -1) ? entity : apputils.cloneDeep(entity);

            currentRowIndex++;
            
            cloneEntity.updateFields();

            //Nov 5 - can't set fields to dirty unless user makes changes other we are taking default values necessarily to the server
            //cloneEntity.setIsDirtyToTrue();

            //Oct 2 - mark this new entity for insert
            cloneEntity.CRUDReason = CRUDReasons.AddingNewData;
            this.rows.splice(currentRowIndex, 0, cloneEntity);
            
            this.updateRowIndex(parentIndex, currentRowIndex);

            return true;
        },

        /**
         * Update row index
         * @param {any} id Id
         * @param {any} currentRowIndex Current row index
         */
        updateRowIndex: function (id, currentRowIndex) {
            for (currentRowIndex; currentRowIndex < this.rows.length; currentRowIndex++) {

                if (apputils.isDefined(this.rows[currentRowIndex])) {
                    this.rows[currentRowIndex].updateRowIndex(currentRowIndex + id, currentRowIndex);
                }
            }
        },

        /**
         * Find row and set deletion flag to mark for deletion
         * @param {any} viewId The view Id
         * @param {any} rowIndex Current row index
         * @param {any} deleteRecord set to true when delet record from DB
         */
        findRowAndMarkforDeletion: function (viewId, rowIndex, deleteRecord=false) {

            if (this.viewid === viewId && rowIndex < this.rows.length) {
                
                //new row not committed to DB yet
                if (this.rows[rowIndex].CRUDReason === CRUDReasons.AddingNewData) {
                    //this.rows.splice(rowIndex, 1);
                    this.rows[rowIndex].CRUDReason = CRUDReasons.Deleting;
                    this.rows[rowIndex].upsertDisabled = true;
                } else if (deleteRecord) {
                    
                    for (let ii = rowIndex; ii < this.rows.length; ii++) {
                        this.rows[ii].markforDeletion(deleteRecord);
                        //this.findRowAndMarkforDeletion(viewId, ii, deleteRecord);
                    }
                } else {

                    this.rows[rowIndex].markforDeletion();
                    
                }
                return true;
            }

            let found = false;
            for (let i = 0; i < this.rows.length; i++) {

                found = this.rows[i].findRowAndMarkforDeletion(viewId, rowIndex, i);

                if (found) break;
            }

            return found;
        },

        /** Load data for kendo Tree view control*/
        loadDataForTree: function () {
            let items = [];

            apputils.each(this.rows, (entity) => {
                items.push(entity.loadDataForTree());
            });

            return items;
        },

        /**
         * Load data for kendo Tree view items 
         * @param {any} treeData
         */
        loadDataForTreeItems: function (treeData) {
            apputils.each(this.rows, (entity) => {
                entity.loadDataForTreeItems(treeData);
            });
        },

        /**
         * Load data for kendo Tree view
         * @param {any} okArr tree view array
         * @param {any} level tree view node level
         */
        loadDataForTree2: function (okArr, level=0) {
            let items = [];

            apputils.each(this.rows, (entity) => {
                items.push(entity.loadDataForTree2(okArr, level));
            });

            return items;
        },

        /** tree node object with empty text */
        treeNode: function () {
            return {text: ""};
        },

        /**
         * Check isMarkedForDeletion flag
         * @param {any} rowIndex Current row index
         */
        isMarkedForDeletion: function (rowIndex) {

            if (apputils.isUndefined(this.rows[rowIndex])) {
                return false;
            }

            return this.rows[rowIndex].isMarkedForDeletion();
        },

        /**
         * Returns total count of entities marked for deletion
         */
        numberofEntitiesMarkedForDeletion: function () {
            let deletedCount = 0;

            this.rows.forEach(row => {
                if (row.isMarkedForDeletion()) {
                    deletedCount++;
                }
            });

            return deletedCount;
        },

        /**
         * Check whether has any row to have deleting flag for this action
         * @param {any} CRUDReason
         */
        isAnyRowMarkedForThisAction: function (CRUDReason) {

            for (let i = 0; i < this.rows.length; i++) {
                if (apputils.isUndefined(this.rows[i])) {
                    return { rowIndex: i, isAnyRowMarkedForThisAction: false };
                }

                if (this.rows[i].isMarkedForThisAction(CRUDReason)) {
                    return { rowIndex: i, isAnyRowMarkedForThisAction: true};
                }
                
            }

            return { rowIndex: -1, isAnyRowMarkedForThisAction: false };
        },

        /**
         * Find the last row that has a flag
         * @param {any} CRUDReason
         */
        findLastRowMarkedForThisAction: function (CRUDReason) {

            for (let i = this.rows.length-1; i > -1 ; i--) {
                if (apputils.isUndefined(this.rows[i])) {
                    return { rowIndex: i, isAnyRowMarkedForThisAction: false };
                }

                if (this.rows[i].isMarkedForThisAction(CRUDReason)) {
                    return { rowIndex: i, isAnyRowMarkedForThisAction: true, saveStatus: this.rows[i].saveStatus };
                }

            }
            return { rowIndex: -1, isAnyRowMarkedForThisAction: false };
        },

        /** Check the collection has any undeleted rows */
        hasRows: function () {

            if (this.rows.length === 0) {
                return false;
            }

            let deletedRows = 0;
            for (let i = 0; i < this.rows.length; i++) {
                if (this.isMarkedForDeletion(i)) {
                    deletedRows++;
                }
            }

            if (deletedRows < this.rows.length) {
                return true;
            }

            return false;
        },

        /** Check is dirty */
        isDirty: function () {

            let hasDirtyRecord = false;
            if (this.hasRows()) {

                for (let i = 0; i < this.rows.length; i++) {
                    let entity = this.rows[i];

                    if (apputils.isUndefined(entity)) {
                        break;
                    }

                    hasDirtyRecord = entity.isDirty();

                    if (hasDirtyRecord) {
                        break;
                    }
                }
            }

            return hasDirtyRecord;
        },

        /** Get this collection undelete rows length  */
        getLength: function () {
            
            if (this.rows.length === 0) {
                return 0;
            }

            let deletedRows = 0;
            for (let i = 0; i < this.rows.length; i++) {
                if (this.isMarkedForDeletion(i)) {
                    deletedRows++;
                }
            }

            return this.rows.length - deletedRows;
        },

        /**
         * Generate initOnly root query string
         * @param {any} existingEntity
         * @param {any} verb
         */
        generateInitingRoot: function (existingEntity, verb) {
            //let verb = CRUDReasons.InitDataOnly;
            let query = apputils.rootNodeTemplate; //"<n t='' n='" + verb + "'>";

            query += this.generateInitOnlyQuery("", verb, existingEntity);

            if (query === apputils.rootNodeTemplate /*"<n t='' n='" + verb + "'>"*/) {
                //nothing to do
                return;
            }

            query += apputils.rootNodeClose;

            return query;
        },

        generateInitOnlyRoot: function (existingEntity) {
            return this.generateInitingRoot(existingEntity, CRUDReasons.InitDataOnly);
        },

        generateInitRoot: function (existingEntity) {
            return this.generateInitingRoot(existingEntity, CRUDReasons.InitData);
        },

        /**
         * Generate InitOnly query string
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} existingEntity
         * @param {any} childIndex
         */
        generateInitOnlyQuery: function (parentIndex, verb, existingEntity, childIndex = "0") {

            //if (this.rows.length === 0) return "";

            let query = "";
            let index = childIndex;

            let entity = this.getEntity();
            
            let children = entity.generateInitOnlyQuery(parentIndex, verb, existingEntity, index);
            //let children = this.rows[0].generateInitOnlyQuery(parentIndex, verb, existingEntity, index);
            
            if (children.length > 0) {
                query += children;
            }

            return query;
        },

        setPreviousValueAndResetIsDirtyFlag: function (fieldName, rowIndex, value) {

            return this.rows[rowIndex].setPreviousValueAndResetIsDirtyFlag(fieldName, value);
        },

        generateMainInitRoot: function (existingEntity) {
            return this.generateMainInitingRoot(existingEntity, CRUDReasons.InitData);
        },

        /**
         * Generate initOnly root query string
         * @param {any} existingEntity
         * @param {any} verb
         */
        generateMainInitingRoot: function (existingEntity, verb) {
            let query = apputils.rootNodeTemplate; 

            query += this.generateMainInitOnlyQuery("", verb, existingEntity);

            if (query === apputils.rootNodeTemplate) {
                //nothing to do
                return;
            }

            query += apputils.rootNodeClose;

            return query;
        },

        /**
         * Generate InitOnly query string
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} existingEntity
         * @param {any} childIndex
         */
        generateMainInitOnlyQuery: function (parentIndex, verb, existingEntity, childIndex = "0") {

            let query = "";
            const index = childIndex;

            //this is for main ui therefore get zero index
            const entity = this.rows.length === 0 ? this.getEntity() : this.rows[0];

            const children = entity.generateInitOnlyQuery(parentIndex, verb, existingEntity, index);
            
            if (children.length > 0) {
                query += children;
            }

            return query;
        },

        /**
         * Generate InitOnly query string
         * @param {any} parentIndex
         * @param {any} verb
         * @param {any} existingEntity
         * @param {any} childIndex
         */
        generateMainChildInitOnlyQuery: function (parentIndex, verb, existingEntity, childIndex = "0") {

            if (apputils.isUndefined(existingEntity)) return "";

            let query = "";
            let index = 0;
            let children = "";

            if (existingEntity.rows.length > 0) {
                existingEntity.rows.forEach(entity => {
                    children += entity.generateInitOnlyQuery(parentIndex, verb, entity, index);
                    index++;
                })

            } else {
                //this is for main ui therefore get zero index
                const entity = this.getEntity();

                children = entity.generateInitOnlyQuery(parentIndex, verb, entity, index);
            }

            if (children.length > 0) {
                query += children;
            }

            return query;
        }
    };

    this.baseObjectCollection = helpers.View.extend(domainObjectCollection);
    this.baseStaticObjectCollection = helpers.View.extend({}, domainObjectCollection);

}).call(this);