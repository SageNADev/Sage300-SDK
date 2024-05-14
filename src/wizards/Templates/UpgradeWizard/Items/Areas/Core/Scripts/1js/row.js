(function () {
    'use strict';

    const rowObj = {

        rowNodes: [],
        bindTrigger: true,
        bindListener: true,

        init: function(){
            this.rowNodes = [];
        },

        findXMLAttributeMatchingField: function (attributes, field) {
            let thisAttr = apputils.find(attributes, function (attr) {
                return attr.name.toLowerCase() === field.toLowerCase();
            });

            return thisAttr;
        },

        /**
         * Set the column value according to the XML attribute
         * @param {any} attributeValue The attribute value from the XML
         * @param {any} column The column of the kendo grid
         * @param {any} rowIndex The row index of the kendo grid
         * @param {any} msgid The ID of the message
         */
        setColumnValueFromXmlAttribute: function (attributeValue, column, rowIndex, msgid) {
            if (column.field === "RowIndex") {
                    column.value = rowIndex; 
            }
            else {
                let thisAttr = this.findXMLAttributeMatchingField(attributeValue, column.field);

                if (apputils.isUndefined(thisAttr)) {
                    return;
                }

                let oriValue = thisAttr.value;

                if (column.dataType === "Date") {
                    oriValue = apputils.formatDateyyyymmdd(thisAttr.value);
                }

                //DBValue must always match persisted Database value when this record was retrieved
                Object.defineProperty(column, "DBValue", {
                    value: oriValue,
                    writable: true //IC still needs this
                });

                Object.defineProperty(column, "valueEx", {
                    set(newValue) {
                        column.value = newValue; 
                        const msg = column.viewid + column.rowIndex + column.field;
                        MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, column.value, msg + apputils.EventMsgTags.svrUpdate);
                    },
                });

                column.value = this.formatValueForDisplay(thisAttr.value, column.formatList, column.dataType);
                this.addRawValueIfFormatValue(thisAttr, column);
                column.rowIndex = rowIndex; 
                column.msgid = msgid;

                if (!column.field.startsWith("attr_")) {
                    column.attributes = () => this.getFieldValue("attr_" + column.field);
                    //column["IDX_" + column.field] = column.id;
                }

                column.viewid = this.viewid;

                if (!this.rowIsReadonly) {

                    let msg = this.viewid + column.msgid + column.field;

                    if (apputils.isUndefined(this.bindTrigger) || this.bindTrigger) {
                        MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, column.value, msg + apputils.EventMsgTags.svrUpdate);
                    }

                    if (apputils.isUndefined(this.bindListener) || this.bindListener) {
                        this.bindEventListener(msg, column);
                    }
                }
            }
         
        },

        /**
         * Return a new column containing the message ID
         * @param {String} msgid Message ID to create
         */
        createMessageIdColumn: function (msgid) {
            
            let messageIdColumn = apputils.clone(apputils.MessageIdColumn);
            messageIdColumn.value = msgid;
            return messageIdColumn; //{ field: "msgid", title: "msg id", dataType: "string", hidden: false, value: msgid };

        },

        /**
         * Populate the columns
         * @param {any} xml The XML source
         * @param {any} rowIndex The row to populate
         * @param {any} msgid The ID of the message
         */
        populateColumns: function (xml, rowIndex, msgid) {
            let self = this;
            let newColumns = apputils.cloneDeep(self.dataModel); //clone(self.dataModel);
            
            apputils.each(newColumns, function (column) {
                self.setColumnValueFromXmlAttribute(xml.attributes, column, rowIndex, msgid);

            });

            newColumns.push(this.createMessageIdColumn(msgid));
            
            let data = {Columns: newColumns};
            self.rowNodes.push(data);
        },

        /**
         * Bind the message event listener
         * @param {String} msg Message
         * @param {object} column Column object
         */
        bindEventListener: function (msg, column) {
            
            if (apputils.isUndefined(column.listener)){
                
                column.listener = (data) =>{

                    this.SetUserInput(column, data);
                };
                
                // clear out all existing listeners for this message if you do this then cannot bind other listeners
                //Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, column.listener);

                //pass "bindEventListener" as context since we want to auto bind these msg and columns only ones
                Keeler.listenTo(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, column.listener, "bindEventListener");

            }
        },

        /**
         * Add raw value to the provided column object
         * @param {any} attr Attribute object
         * @param {any} column Column object with formatted value
         */
        addRawValueIfFormatValue: function (attr, column) {
            if (column.formatList /*&& column.value !== attr.value*/) {

                const obj = column.formatList.find(x => x.display == column.value)

                if (apputils.isUndefined(obj)) {
                    column.rawValue = attr.value;

                } else {
                    column.rawValue = obj.value; // attr.value;

                }
            }
        },

        /**
         * Format the value for displaying purposes
         * @param {any} value Value to be formatted
         * @param {boolean} formatList
         * @param {any} dataType The value datatype
         */
        formatValueForDisplay: function (value, formatList, dataType) {
            if (value && formatList) {
                var formatter = apputils.find(formatList, function (format) {
                    if (apputils.isNumber(value)){
                        return format.value === value;
                    }
                    return format.value.toLowerCase() === value.toLowerCase();
                });
                return apputils.isUndefined(formatter) ? value : apputils.isUndefined(formatter.display) ? value : formatter.display;
            }
            if (dataType && dataType === 'Date' && value.includes(' ')) {
                return value.split(' ')[0];
            }
            if (dataType && dataType === "Time") {
                return apputils.formatTime(value);
            }

            return value;
        },

        /**
         * Format the value for saving purposes
         * @param {any} value Value to be formatted
         * @param {boolean} formatList
         */
        formatValueForUpsert: function (value, formatList) {
            if (formatList) {
                let formatter = apputils.find(formatList, function (format) {
                    value = value.toString();
                    return (format.display.toLowerCase() === value.toLowerCase()) || (format.value.toLowerCase() === value.toLowerCase());
                });

                //this is correct syntax: Expressions - Nullish coalescing operator
                return apputils.isUndefined(formatter) ? value : formatter.upsert ?? formatter.value;
                
            }

            return value;
        },

        /**
         * Validate the field, returns true if valid, false otherwise
         * @param {any} column Column to be validated
         * @param {any} value Value to be validated
         * @param {any} result Optional parameter
         */
        ValidateFieldOnly: function (column, value, result) {
            result = result || this.validation(column, value);
            
            if (apputils.isUndefined(result)) {
                result = { approved: true };

            }

            let relatedResult;
            let validationResult = result.approved && !apputils.isUndefined(relatedResult = this.validateRelated(column)) ? relatedResult : result;

            if (validationResult.approved) {

                return true;

            } else {
                return false;

            }
        },

        //TODO: rename function since this does more than validation
        ValidateField: function (column, value, result) {
            let tempVal = column.value;

            result = result || this.validation(column, value);
            //result = { approved: true };

            if (apputils.isUndefined(result)) {
                result = { approved: true };

                trace.warn("warning validation rules not setup for view " + this.viewid + " field " + column.field);

                //Oct 26
                //return true;
            }

            //OCT 14 - now all fields have msgid
            //let msg = this.viewid+ column.rowIndex + column.field;
            let msg = this.viewid + column.msgid + column.field;

            let relatedResult;
            let validationResult = result.approved && !apputils.isUndefined(relatedResult = this.validateRelated(column)) ? relatedResult : result;

            if (validationResult.approved) {

                column.value = value;
                column.isDirty = true;
                column.errorValue = "";
                column.validationError = false;

                //this.timeStampUpdate(column);

                MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrValid, column.value, msg + apputils.EventMsgTags.svrUpdate);

                //don't notify main level objects & validateRelated objects
                //MessageBus.msg.trigger(this.viewid + apputils.EventMsgTags.svrValid, { viewId: this.viewid, rowIndex: column.rowIndex, field: column.field, value: value });
                //if (result.validateRelated){
                //    this.validateRelated(column);
                //}

                return true;

            } else {

                trace.warn("Invalid input for view " + this.viewid + " field " + column.field);
                if (tempVal !== value) {
                    column.value = tempVal;
                }

                column.errorValue = value;

                //flag data is invalid, can be used later if persistenace is disallowed on erroronus data
                column.validationError = true;
                MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrInvalid, result.errors, msg + apputils.EventMsgTags.svrUpdate);

                //don't also notify main level objects 
                //MessageBus.msg.trigger(this.viewid + apputils.EventMsgTags.svrInvalid, { viewId: this.viewid, rowIndex: column.rowIndex, field: column.field, title: column.title, value: value });

                return false;
            }
        },

        /**
         * Set the column according to the data provided by the user
         * @param {any} column The column to set
         * @param {any} data The data to set
         */
        SetUserInput: function (column, data) {
            column.previousValue = column.value;

            if (data.customBinding){
                this.caller.customBindingCallBack(data.customBinding, column, data);
            }
            else{
                this.ValidateField(column, data.value);

                //Oct 17
                //MessageBus.msg.trigger(this.viewid + "RefreshUI", column);
            }
        },

        /**
         * Validate the column base on the rules from the baseStaticObjectValidation
         * @param {any} column Column to be validated
         * @param {any} valueToValidate Value to be validated
         */
        validation: function(column, valueToValidate)
        { 
            let rules = this.validationRules();

            if (rules === null || apputils.isUndefined(rules[column.field])){
                return;
            }

            let value = this.parseValue(valueToValidate, column.dataType); 

            let result = baseStaticObjectValidation.execute(rules[column.field], value, this);

            //validate related fields only when the current field is valid.
            result.validateRelated = result.approved;

            return result;
        },

        /**
         * Validate related column
         * @param {any} column The column to validate
         */
        validateRelated: function(column)
        {
            let rules = this.validationRules();

            if (rules === null || apputils.isUndefined(rules.related)) {
                return;
            }

            if (apputils.isUndefined(rules.related[column.field])){
                return;
            }

            let relatedColumn = this.getColumnByFieldName(rules.related[column.field]);
            if (apputils.isUndefined(rules[relatedColumn.field])){
                return;
            }

            let value = this.parseValue(relatedColumn.value, relatedColumn.dataType); 
            
            let result = baseStaticObjectValidation.execute(rules[relatedColumn.field], value, this);
            result.validateRelated = false;
        
            this.ValidateField(relatedColumn, relatedColumn.value, result);

            return result;
        },

        validationRules: function () {
            throw "Not Implemented: validationRules should be implemented by inheriting object.";
             
        },

        /**
         * Load the data for the kendo grid, for both rows and columns
         * @param {any} gridArray Kendo grid array
         * @param {any} prefix Prefix namespace
         */
        loadDataForGrid: function (gridArray, prefix="") {
            /*apputils.each(this.rowNodes, (rowNode) => {
                apputils.each(rowNode, (Columns) => {
                    let data = { prefixNamespace: prefix, viewid: this.viewid };
                    apputils.each(Columns, (column) => {
                        data[column.field] = column.value;
                    });
                    gridArray.push(data);
                });
            });*/

            
            let data = { prefixNamespace: prefix, viewid: this.viewid };

            for (const column of this.rowNodes[0].Columns) {
                data[column.field] = column.value;
            }
                
            gridArray.push(data);
            
            
        },

        /**
         * Load data for the kendo grid, for the columns only
         * @param {any} gridArray Kendo grid array
         */
        loadDataVerticallyForGrid: function (gridArray) {
            apputils.each(this.rowNodes, (rowNode) => {
                apputils.each(rowNode, (Columns) => {
                    
                    apputils.each(Columns, (column) => {
                        let data = { viewid: this.viewid };
                        data[column.field] = column.value;
                        gridArray.push(data);
                    });
                    
                });
            });
        },

        /**
         * Load the columns when a finder is being opened
         * */
        loadFinderColumns: function () {
            let finderColumns = [];

            apputils.each(this.dataModel, function (column) {
                if (column.useInfinder) {
                    finderColumns.push(column);
                }
            });

            return finderColumns;
        },

        /**
         * Returns the row by the rowIndex
         * @param {any} rowIndex Row index to find
         */
        findRowByIndex: function (rowIndex) {

            let indexRow = apputils.find(this.rowNodes, (rowNode)=> {
                return apputils.find(rowNode.Columns, (column)=>{
                    return column.field === "RowIndex" && column.value === rowIndex;
            })});

            return indexRow;
        },

        /**
         * Select the top row of a kendo grid
         * @param {String} prefix
         */
        selectTopRow: function (prefix) {
            
            apputils.each(this.rowNodes, (row)=> {
                    apputils.each(row.Columns, (column)=> {

                        if (column.field === "RowIndex"){
                           //do nothing
                        } else {
                            column.rowIndex = 0;
                            //let msg = this.viewid + column.rowIndex + prefix + column.field;
                            let msg = prefix + this.viewid + column.rowIndex + column.field;

                            column.value = this.formatValueForDisplay(column.value, column.formatList);
                            this.addRawValueIfFormatValue(column, column);

                            MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, column.value, msg + apputils.EventMsgTags.svrUpdate);

                            //since this is top row, make sure all previous listener are removed - not needed.
                            //Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, msgCtx);
                            //delete column.listener;

                            this.bindEventListener(msg, column);
                        }
                        
                    });
            });
        },

        /**
         * Select the row of a kendo grid by index
         * @param {any} prefix
         */
        selectRowByIndex: function(prefix) {
            apputils.each(this.rowNodes, (row)=> {
                    apputils.each(row.Columns, (column)=> {

                        if (column.field === "RowIndex"){
                           //do nothing

                        } else {

                            //let msg = this.viewid + column.msgid + prefix + column.field;
                            let msg = prefix + this.viewid + column.msgid + column.field;

                            column.value = this.formatValueForDisplay(column.value, column.formatList);
                            this.addRawValueIfFormatValue(column, column);

                            MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, column.value, msg + apputils.EventMsgTags.svrUpdate);
                            //if (apputils.isUndefined(this.bindListener) || this.bindListener) {
                                this.bindEventListener(msg, column);
                            //}
                        }

                    });
            });

        },

        /**
         * Get the column by the field name
         * @param {String} fieldName Name of the field
         */
        getColumnByFieldName: function(fieldName){
           
            let foundColumn = {};

            apputils.find(this.rowNodes, (rowNode)=> {
                return foundColumn = this.getRowColumnByFieldName(rowNode, fieldName);
            });

            return foundColumn;
        },

        /**
         * Get the row column by the field name
         * @param {any} rowNode Row node
         * @param {String} fieldName Name of the field
         */
        getRowColumnByFieldName: function(rowNode, fieldName){   
            return apputils.find(rowNode.Columns, (column)=>{
                return column.field === fieldName;
            });            
        },

        /**
         * Set the field value for the column
         * @param {any} args
         */
        setColumnFieldValue: function (args) {
            args.column.rowIndex = args.column.rowIndex || 0;
            args.column.value = this.formatValueForDisplay(args.value, args.column.formatList, args.column.dataType);
            this.addRawValueIfFormatValue(args, args.column);
            args.column.isDirty = apputils.isUndefined(args.isDirty) ? true : args.isDirty;
            args.column.validationError = false;

            this.timeStampUpdate(args.column);

            const msg = this.viewid + args.column.rowIndex + args.column.field;
            MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, args.column.value, msg + apputils.EventMsgTags.svrUpdate);
        },

        timeStampUpdate: function (column) {
            //console.log("timeStampUpdate: " + this.viewid + " "+ column.field);

            column.updatedTime = apputils.TimeTicks();
            column.addToInitQuery = true;
        },

        /*setGridColumnFieldValue: function (args) {
            args.column.value = this.formatValueForDisplay(args.value, args.column.formatList);
            args.column.isDirty = args.isDirty;
            args.column.validationError = false;

            let msg = this.viewid + args.column.msgId + args.column.field;
            MessageBus.msg.trigger(msg + apputils.EventMsgTags.svrUpdate, args.column.value, msg + apputils.EventMsgTags.svrUpdate);
        },*/

        /**
         * Returns a query to upsert the child data
         * @param {int} index The data index
         * @param {String} method The query method
         */
        upsertChildData: function(index, method){
            let upsert = "";

            apputils.each(this.allCollectionObj, obj => {
                if (!obj.skipInAllQuery) {
                    upsert += obj.generateUpsertData(index, method);
                }
            
            });

            return upsert;
        },

        /*generateGetTemplateQuery: function (parentIndex, verb) {
            let xml = "";
            let id = parentIndex +"1";
            let children = this.getChildTemplate(id, verb);

            xml += "<r i='" + this.viewid + "' f='' ";
                    
            xml += "p='" + parentIndex  + "' id='" + id + "' verb='" + verb + "'>";
            xml += children;
            xml += "</r>";
            
            return xml;
        },*/

        /*generateGetQuery: function (parentIndex, verb, filterCallback, depthViewId) {

            let xml = "";
            let id = parentIndex +"1";
            let children = "";

            if (this.viewid !== depthViewId){
                children = this.generateChildGetQuery(id, verb, filterCallback, depthViewId);
            }
            
            let filter = filterCallback(this.viewid);

            xml += "<r i='" + this.viewid + "' f='" + filter + "' ";
                    
            xml += "p='" + parentIndex  + "' id='" + id + "' verb='" + verb + "'>";
            xml += children;
            xml += "</r>";
            
            return xml;
        },*/

        /*generateChildGetQuery: function(index, method, filterCallback, depthViewId){
            let query = "";
            apputils.each(this.allCollectionObj, obj => {
                if (!obj.skipInAllQuery) {
                    query += obj.generateGetQuery(index, method, filterCallback, depthViewId);
                }
            
            });

            return query;

        },*/

        /**
         * Set the dirty flag to false
         * */
        setIsDirtyToFalse: function (/*row*/) {
            apputils.each(this.rowNodes, (row)=> {
                
                apputils.each(row.Columns, (column)=> {
                    if (column.isDirty === true) {
                        column.isDirty = false;
                    }

                });
            });
        },

        /**
         * Removes the message listener
         * */
        removeColumnListener: function () {
            apputils.each(this.rowNodes, (row)=> {
                
                apputils.each(row.Columns, (column)=> {
                    let msg = this.viewid + column.rowIndex + column.field;
                    Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, column.listener);
                    delete column.listener;
                });

            });
        },

        /**
         * Check whether upsert is disabled for the column
         * @param {any} column The column in the kendo grid to check
         */
        upsertDisabled: function(column){
            return column.upsertDisabled ?? false;
        },

        /**
         * Set the dirty flag to true
         * */
        setIsDirtyToTrue: function () {
            
            apputils.each(this.rowNodes, (row)=> {
                
                apputils.each(row.Columns, (column)=> {
                    
                    if (column.id > -1 && !this.upsertDisabled(column)) {
                        column.isDirty = true;
                    }
                });

            });
        },

        /**
         * Populate compositions
         * @param {int} rowIndex Index of the row
         * @param {int} parentIndex Index of the parent
         */
        populateEmptyCompositions: function (rowIndex, parentIndex){

            this.rowNodes = [];
            let msgId = rowIndex + parentIndex;

            let newColumns = apputils.cloneDeep(this.dataModel); 
            newColumns.push(this.createMessageIdColumn(msgId));

            this.fillDefaultValuesFromFormFormatList(newColumns);

            let data = { Columns: newColumns};

            this.rowNodes.push(data);

        },

        fillDefaultValuesFromFormFormatList: function (columns) {
            
            columns.forEach(column => {
                this.addRawValueIfFormatValue({ value: column.value }, column);
                column.value = this.formatValueForDisplay(column.value, column.formatList, column.dataType);
            });

        },

        /**
         * Parse the value to a number
         * @param {any} value The value to parse
         * @param {String} dataType The datatype of the value
         */
        parseValue: function(value, dataType){
            switch(dataType){
                case ("Date"):
                    return value.length>0 ? new Date(value): "";
                case ("Decimal"):
                case ("int"):
                case ("Long"):
                    //Comment below line code, it will cause locale number(like 12,34) as NaN
                    //return Number(value);
                default:
                    return value;
            }
        },

        /**
         * Get the formatted value in the column by the field name
         * @param {any} name Field name
         */
        getColumnFormatedValueByFieldName: function(name){
            let column = this.getColumnByFieldName(name);
            let value = this.parseValue(column.value, column.dataType);
            return {value: value, field: column.title};
        },

        /**
         * Get the field DB value from the field name
         * @param {String} fieldName The field name of the column
         */
        getFieldDBValue: function (fieldName) {
            let column = this.getColumnByFieldName(fieldName);
            return column.DBValue;
        },

        /**
         * Get the field value from the field name
         * @param {String} fieldName The field name of the column
         */
        getFieldValue: function(fieldName){
            let column = this.getColumnByFieldName(fieldName);
            return column.value;
        },

        /**
         * Get the previous value from the field name
         * @param {String} fieldName The field name of the column
         */
        getFieldPreviousValue: function (fieldName) {
            let column = this.getColumnByFieldName(fieldName);
            return column.previousValue;
        },

        /**
         * Returns true if the field is dirty, false otherwise
         * @param {String} fieldName The name of the field
         */
        isFieldDirty: function(fieldName){
            let column = this.getColumnByFieldName(fieldName);
            return apputils.isUndefined(column.isDirty) ? false : column.isDirty;
        },

        /**
         * Set the dirty flag for the field
         * @param {String} fieldName The name of the field
         * @param {boolean} isDirty True if dirty, false otherwise
         */
        setFieldDirty: function (fieldName, isDirty) {
            let column = this.getColumnByFieldName(fieldName);
            column.isDirty = isDirty;
        },

        /**
         * Generate a query to upsert a row
         * @param {object} row The row to upsert
         * @param {String} id The ID to create
         */
        generateUpsertRowQuery: function(row, id) {
            let query = '';

            row.Columns.forEach((column) => {
           
                if (column.isDirty && !column.upsertDisabled) {
                    this.ValidateField(column, column.value);
                    let formatedValue = this.formatValueForUpsert(column.value, column.formatList);

                    if (column.value && kendo.parseDate(column.value.toString()) && column.isOptionalFieldValue && !isNaN(Date.parse(column.value)) && isNaN(column.value)) {
                        formatedValue = kendo.toString(kendo.parseDate(column.value), "yyyyMMdd");
                    }

                    //Convert local decimal separator to culture-independent separator(.)
                    if (column.dataType === "Decimal") {
                        formatedValue = kendo.parseFloat(column.value);
                    }

                    //[RC] 10/11/2022 for future reference see IC0190BillsofMaterialComponentsEntityFields.js (upsert: "0").
                    //You must update *Field.js to set custom upsert value not here.
                    //if (column.dataType === 'Bool' && (formatedValue === globalResource.Yes || formatedValue === globalResource.No)) {
                    //    formatedValue = formatedValue === globalResource.Yes;
                    //}
                    formatedValue = apputils.escape(formatedValue);
                    query += apputils.createFieldNode({ fieldId: column.id, value: formatedValue, verify: column.verify, parentId: id }); //"<c f='" + column.id + "' v='" + formatedValue + "' p='" + id + "' />";
                }
            });

            return query;
        },

        /**
         * Generate the query to initialize a row
         * @param {object} row The row to initialize
         * @param {String} id The ID to create
         */
        generateInitRowQuery: function (row, id) {
            let query = '';

            apputils.each(row.Columns, (column) => {
            
                //const numericType = ['decimal', 'int', 'long', 'amount'];
                if (column.alwaysIncludeForinit || (column.isDirty && !column.upsertDisabled && !column.initDisabled && column.addToInitQuery)) {

                    query += this.createFieldNode(column, id);
                    /*column.addToInitQuery = false;
                    let formatedValue = this.formatValueForUpsert(column.value, column.formatList);
                    if (formatedValue === '' && numericType.includes(column.dataType.toLowerCase())) {
                        formatedValue = '0';
                    }
                    query += apputils.createFieldNode({ fieldId: column.id, value: formatedValue, parentId: id }); //"<c f='" + column.id + "' v='" + formatedValue + "' p='" + id + "' />";
                    */
                }
            });

            return query;
        },

        /**
         * Generate ordered columns query to initialize a row
         * @param {object} row The row to initialize
         * @param {String} id The ID to create
         */
        generateOrderedInitRowQuery: function (row, id) {
            let query = '';

            const columnsByUpdatedTime = apputils.sortBy(row.Columns, o => o.updatedTime);
            
            columnsByUpdatedTime.forEach((column) => {
                
                if (column.addToInitQuery && !column.upsertDisabled && !column.initDisabled) {
                    //delete column.updatedTime;

                    //console.log("generateOrderedInitRowQuery: " + this.viewid + " " + column.field);

                    query += this.createFieldNode(column, id);
                }
            });

            return query;
        },

        createFieldNode: function (column, id) {

            if (apputils.isUndefined(column.value)) {
                return '';
            }

            column.addToInitQuery = false;
            let formatedValue = this.formatValueForUpsert(column.value, column.formatList);
            if (formatedValue.length === 0 && apputils.numericType.includes(column.dataType.toLowerCase())) {
                formatedValue = '0';
            }

            //string representation eg: "<c f='" + column.id + "' v='" + formatedValue + "' p='" + id + "' />";
            return apputils.createFieldNode({ fieldId: column.id, value: formatedValue, verify: column.verify, parentId: id });
        },

        //TODO: merge this function with populateEmptyColumns
        updateRowIndex: function (msgId, currentRowIndex) {
            apputils.each(this.rowNodes, (row) => {

                apputils.each(row.Columns, (column) => {

                    if (column.field === "RowIndex") {
                        column.value = currentRowIndex;
                    } else if (column.field === apputils.MessageIdColumn.field /*"msgid"*/) {
                        column.msgid = msgId;
                        column.value = msgId;
                    } /*else if (!apputils.isUndefined(column.msgid)){
                        column.msgid = msgId;
                    }*/

                    column.rowIndex = currentRowIndex;
                    column.msgid = msgId;
                    //column.isDirty = true; //cannot mark dirty here since existing can be adjusted to new RowIndex

                    let msg = this.viewid + column.msgid + column.field;
                    Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, column.listener);
                    delete column.listener;

                    this.bindEventListener(msg, column);

                });

            });
        },

        /**
         * Delete the row
         * */
        deleteRow: function () {
            apputils.each(this.rowNodes, (row) => {

                apputils.each(row.Columns, (column) => {

                    if (!apputils.isUndefined(column.listener)) {
                        let msg = this.viewid + column.msgid + column.field;

                        // clear out all existing listeners for this message
                        Keeler.stopListening(MessageBus.msg, msg + apputils.EventMsgTags.usrUpdate, column.listener);
                        delete column.listener
                    }

                });

            });

            delete this.rowNodes;
        },

        /**
         * Load the data for the tree
         * */
        loadDataForTree: function () {
            let treeData = {};

            apputils.each(this.rowNodes, (rowNode) => {
                apputils.each(rowNode, (Columns) => {
                    treeData = { viewid: this.viewid };
                    apputils.each(Columns, (column) => {
                        treeData[column.field] = column.value;
                    });
                    
                });
                this.caller.treeRowProcessor(rowNode, treeData);
            });

            return treeData;
        },

        /*not used but left as example
        loadDataForTree2: function (okArr, level) {
            let treeData = {};

            apputils.each(this.rowNodes, (rowNode) => {
                apputils.each(rowNode, (Columns) => {
                    treeData = { viewid: this.viewid };
                    apputils.each(Columns, (column) => {
                        treeData[column.field] = column.value;
                    });

                });
                this.caller.treeRowProcessor2(rowNode, treeData, okArr, level);
            });

            return treeData;
        },
        */

        findDirtyRecords: function () {
            let dirtyList = [];
            apputils.each(this.rowNodes, (row) => {
                apputils.each(row.Columns, (column) => {
                    if (column.isDirty === true) {
                        dirtyList.push(column);
                        
                    };
                });

            });

            return dirtyList;
        },

        updateDirtyFlag2: function (entity) {
            apputils.each(this.rowNodes, (row) => {
                apputils.each(row.Columns, (column) => {
                    if (column.isDirty === true) {
                        //AT-77726
                        this.reassignDBValue(entity, column);
                        entity.setDirtyFlag(column.field);
                    };
                });

            });
        },

        /**
         * Reassign database value
         * @param {any} entity The entity
         * @param {any} column The column of the entity
         */
        reassignDBValue: function (entity, column) {
            //const idx = entity.rowObj.rowNodes[0].Columns.findIndex(x => x.field === column.field);
            //entity.rowObj.rowNodes[0].Columns[idx].DBValue = column.DBValue;
            let newColumn = entity.getColumnByFieldName(column.field);
            newColumn.DBValue = column.DBValue;
        },

        /**
         * Set the dirty flag for the field name
         * @param {String} fieldName The field name of the column
         */
        setDirtyFlag: function (fieldName) {
            let column = this.getColumnByFieldName(fieldName);
            column.isDirty = true;
        },

        /**
         * Returns true if dirty, false otherwise
         * */
        isDirty: function () {
            for (let i = 0; i < this.rowNodes[0].Columns.length; i++) {

                if (this.rowNodes[0].Columns[i].upsertDisabled) {
                    //don't check isDirty flag

                } else if (this.rowNodes[0].Columns[i].isDirty) {
                    return true;
                }
            }

            return false;
        },

        /**
         * Returns true if has error, false otherwise
         * */
        hasError: function() {
            for (let i = 0; i < this.rowNodes[0].Columns.length; i++) {
                if (this.rowNodes[0].Columns[i].validationError) {
                    return true;
                }
            }

            return false;
        },

        /**
         * Get the database value filter string if fileds are dirty. For numeric value, should convert locale decimal separator to "."
         * */
        getDBValueFilterIfDirty: function () {
            const numArray = apputils.numericType; //['Decimal', 'Long', 'Int', 'Integer', 'Amount', 'Number'];

            let filter = "";
            for (let i = 0; i < this.rowNodes[0].Columns.length; i++) {
                const DBValueFilterDisabled = apputils.isUndefined(this.rowNodes[0].Columns[i].DBValueFilterDisabled) ? false : this.rowNodes[0].Columns[i].DBValueFilterDisabled;

                if (this.rowNodes[0].Columns[i].isDirty && this.rowNodes[0].Columns[i].DBValue && !DBValueFilterDisabled) {
                    const name = this.rowNodes[0].Columns[i].name;
                    const type = this.rowNodes[0].Columns[i].dataType.toLowerCase();

                    if (type === "Time") { //Skip time type value in db value filter
                        continue;
                    }

                    if (filter.length > 0) filter += " and ";

                    const isNumber = numArray.includes(type);

                    let value = apputils.escape(this.rowNodes[0].Columns[i].DBValue);
                    if (isNumber) {
                        value = kendo.parseFloat(value);
                    }
                    filter += isNumber ? `${name} = ${value}` : `${name} = "${value}"`;
                }
            }

            return filter;
        },

        /**
         * Copy the field values from an entity
         * @param {any} fromEntity The entity to copy from
         */
        copyFieldValuesFromAnotherEntity: function (fromEntity) {
            const copyToThisEntity = this;

            fromEntity.rowObj.rowNodes[0].Columns.forEach((column) => {

                if (column.field === "RowIndex" || column.field === "rowCount" || column.field === apputils.MessageIdColumn.field) return;

                const col = copyToThisEntity.getColumnByFieldName(column.field);
                //const isDirty = col.isDirty;
                //column.isDirty = col.isDirty;
                col.isDirty = apputils.isUndefined(col.isDirty) ? false : col.isDirty;
                copyToThisEntity.caller.setFieldData(column.field, column.value, col.isDirty);
                //col.isDirty = isDirty;
                
            });
        },

        //Flushes the current field value to the view, if the current field is dirty.
        FlushValue: function (pField, verify) {
            //maybe need to set the field with verify flag
            //then pass to View
        },

        //Forces the field value to be refreshed from the view
        RefreshValue: function (pField) {
            //update UI from View
        },

        /*adjustTotalRowCountAfterLazyRetrieve: function () {

            this.rowNodes[0].Columns.forEach((column) => {
                entity.adjustTotalRowCountAfterLazyRetrieve();
            });

        }*/
    }

    this.rowDataObj = helpers.View.extend(rowObj);
    this.staticRow = helpers.View.extend({}, rowObj);

}).call(this);