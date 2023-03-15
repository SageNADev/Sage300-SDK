'use strict';

this.ErrorModelObj = {
    RowIndex: {
        id: -1,
        field: "RowIndex",
        title: "Row Index",
        dataType: "int",
        hidden: true,
        value: 0
    },
    ViewMsgId: {
        id: 1,
        field: "ViewMsgId",
        title: "ViewMsgId",
        dataType: "string",
        value: ""
    },
    MSG: {
        id: 2,
        field: "msg",
        title: "msg",
        dataType: "string",
        value: ""
    },
    ErrorAsWarning: {
        id: 3,
        field: "errorAsWarning",
        title: "errorAsWarning",
        dataType: "string",
        value: "False"
    }
    
};

var Errormodel = [];
apputils.keys(ErrorModelObj).forEach(e => Errormodel.push(ErrorModelObj[e]));
this.ErrorModelObj.viewId = "errors";
this.ErrorModelObj.node = "errors/error";
this.ErrorModelObj.model = Errormodel;

/** Define general error object */
var ErrorEntity = helpers.View.extend({
    dataModelObj: ErrorModelObj,
    get node() { return this.dataModelObj.node; },
    get dataModel() { return this.dataModelObj.model; },
    get viewid() { return this.dataModelObj.viewId; },
    model: {},

    /**
     * Add error message
     * @param {any} rowIndex Row index value
     * @param {any} viewid Accpac View Id
     * @param {any} errorAsWarning Error as Warning
     * @param {any} msg Message
     */
    add: function (rowIndex, viewid, errorAsWarning, msg) {

        this.model = apputils.cloneDeep(this.dataModelObj); //clone(this.dataModelObj);
        this.model.RowIndex.value = rowIndex;
        this.model.ViewMsgId.value = viewid.value;
        this.model.MSG.value = msg;
        this.model.ErrorAsWarning.value = errorAsWarning.value;
    },

    /** Get error message object */
    getMessage: function () {
        return { "Message": this.model.MSG.value, "Priority": 3, "PriorityString": "Error", "Tag": null }
    },
    /** Error message is warning message */
    isErrorAWarning: function() {
        return this.model.ErrorAsWarning.value === "True";
    },

    /** Message string value*/
    message: function () {
        return this.model.MSG.value;
    },
});

/** Errors collection object */
var ErrorEntityCollectionObj = helpers.View.extend({},{
    entityObject: ErrorEntity,
    get viewid() { return this.entityObject.viewid; },
    get dataModel() { return this.entityObject.dataModel; },
    get node() { return this.entityObject.node; },
    get xmlPath() { return "" + this.entityObject.node; },
    rows: [],

    /**
     * Get errors from xml response data
     * @param {any} data
     */
    ParseEntity: function (data) {
        if (data.length === 0) {
            return;
        }

        let parser = new DOMParser();

        let xmlDoc = parser.parseFromString(data, "text/xml");

        let iterator = xmlDoc.evaluate(appconfig.response.tag + '//errors/error', xmlDoc, null, XPathResult.ANY_TYPE, null);

        try {
            let thisNode = iterator.iterateNext();
            this.rows = [];

            while (thisNode) {
                this.populate(thisNode);

                thisNode = iterator.iterateNext();
            }
        } catch (e) {
            trace.throwError(e);
            
        }

        iterator = null;
        xmlDoc = null;
        parser = null;
    },

    /**
     * populate error object messages by xml node
     * @param {any} node
     */
    populate: function (node) {
        this.addError(node.attributes.viewid, node.attributes.msg, node.attributes.errorAsWarning);
    },

    /**
     * Add error object
     * @param {any} viewid entity view id
     * @param {any} msg Message
     * @param {any} errorAsWarning Error as warning boolean value
     */
    addError: function (viewid, msg, errorAsWarning=false) {
        let errMsg = msg.value || msg;

        if (!errMsg.includes) {
            return;
        }

        //suppress system level message
        if (errMsg.includes("ExceptionUtil::innerException") || errMsg.includes("Error HRESULT E_FAIL") || errMsg.includes("BusinessAccess.ExceptionUtil") || errMsg.includes("Attempted to access an unloaded appdomain") || errMsg.includes("pstbm-")) {
            return;
        }

        const messageExists = this.rows.find((ele) => ele.message() === errMsg);
        if (messageExists) {
            return;
        }

        let entity = new this.entityObject();
        this.rows.push(entity);

        entity.add(this.rows.length, viewid, errorAsWarning, errMsg);
    },

    /**
     * Add error message
     * @param {any} viewid View Id
     * @param {any} msg Message string
     */
    addErrorMsg: function (viewid, msg) {
        this.addError(viewid, msg);
    },

    /** Clear error */
    clearError: function () {
        this.rows = [];
    },

    /** Get errors */
    getErrors: function () {
        let errorMsgs = [];
        apputils.each(this.rows, (entity) => {
            if (!entity.isErrorAWarning()) {
                errorMsgs.push(entity.getMessage());
            }
        });

        return errorMsgs;
    },
    /**
     * Error is warning 
     * @param {any} index error index
     */
    errorAsWarning: function (index) {
        if (this.rows.length === 0 || apputils.isUndefined(this.rows[index])) {
            return false;
        }

        return this.rows[index].isErrorAWarning();
    },

    /** Is error as warning */
    isErrorAWarning: function() {
        return this.errorAsWarning(0);
    },

    /** Get warnings */
    getWarnings: function () {
        let warningMsgs = [];
        apputils.each(this.rows, (entity) => {
            if (entity.isErrorAWarning()) {
                warningMsgs.push(entity.getMessage());
            }
        });
        return warningMsgs;
    },
});