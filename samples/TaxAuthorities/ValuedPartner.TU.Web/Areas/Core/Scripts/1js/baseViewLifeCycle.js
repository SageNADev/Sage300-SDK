/* Copyright (c) 1994-2022 Sage Software, Inc.  All rights reserved. */

(function () {
    'use strict';

    const NavigationAction = apputils.NavigationAction; 

    let domainViewLifeCycle = {
        screenContainerId: undefined,
        mainCollectionObj: undefined,
        eventListeners: [],
        gridElements: [],
        treeElements: [],
        recordTitle: "",
        recordTitleNumberId: "",
        importExportEventName: undefined,
        detailsGrid: undefined,
        keysControlIds:[],
        navId: '',
        //TODO: these need to move into baseView but the scripts need to move into core project first
        CS0001CompanyProfileEntityColl: undefined, 
        CS0003CurrencyCodesEntityV3Coll: undefined,
        CS0002FiscalCalendarsEntityV3Coll: undefined,
        CS0005CurrencyTablesEntityV3Coll: undefined,
        IC0380ICOptionsEntityV3Coll: undefined,
        PO0600PurchaseOrderOptionsEntityV3Coll: undefined,
        CS0011OptionalFieldsEntityV3Coll: undefined,

        previousKeyValue: '',
        navigationAction: NavigationAction.None,
        VCRWorkerObj: undefined,
        VCRWorkerFunc: undefined,
        showSaveMessage: true,

        /** Initialize the main object */
        initMainObject: function () {

            //D-45048 fix
            function isConstructor(value) {
                return typeof value === 'function' && !!value.prototype && value.prototype.constructor === value;
            }

            this.mainCollectionObj = new this.mainObj();
            this.VCRWorkerObj = apputils.isDefined(this.VCRWorkerFunc) && isConstructor(this.VCRWorkerFunc) ? new this.VCRWorkerFunc() : new this.mainObj();

            //make proto object ready
            if (this.protoExt) {
                this.protoExt.mainCollectionObj = this.mainCollectionObj;
                this.protoExt.VCRWorkerObj = this.VCRWorkerObj;
            }

        },

        windowBeforeUnload: function () {
            
            $(window).on('beforeunload', (event) => {
                if (apputils.isUndefined(this.mainCollectionObj) || this.mainCollectionObj === null) {
                    return;
                }

                if (this.mainCollectionObj.isDirty()) {
                    return "As of 2021, for security reasons, it is no longer possible to display a custom message in the beforeunload popup";
                }
                
            });
        },

        UIctx: "",

        /** Bind all the finder listeners */
        bindAllFinderListeners: function () {
            apputils.each(this.eventListeners, (listener) =>{
                this.mainCollectionObj[listener](this.UIctx);
            });
        },

        /** Assign grid elements to the grid */
        assignGrids: function () {
            apputils.each(this.gridElements, (grid) => {
                this.mainCollectionObj[grid]();
            });
        },

        /** Assign tree elements to the grid */
        assignTrees: function () {
            apputils.each(this.treeElements, (tree) => {
                this.mainCollectionObj[tree]();
            });
        },

        event: {
            id: '.js-btnLogin', //this can to id, class etc
            evt: 'click', //any event; change, blur etc
            action: 'login' //function to execute when the event occurs
        },

        events: [],
        hotKeyEvents: [],

        /** Bind the events */
        bindEvents: function () {
            let self = this;
            let handler = function (event, data) {
                data.preventDefault();
                if (event.override) {
                    self[event.action](data.currentTarget, event);
                } else {
                    self.mainCollectionObj[event.action](data.currentTarget);
                }
            };

            let controlEvents = apputils.isFunction(this.events) ? this.events() : this.events;

            apputils.each(controlEvents, (event) => {
                $(event.id).on(event.evt, (data) => {
                    handler(event, data);
                });

                if (event.hotKey) {
                    self.hotKeyEvents.push(event);
                }
            });
        },

        /** Bind the hot key events */
        bindHotKeyEvents: function () {
            let self = this;

            if (apputils.isFunction(this.detailsGrid)) {
                $("#" + this.detailsGrid().gridId).on('keydown', function (e) {
                    var grid = $("#" + e.currentTarget.id).data("kendoGrid");
                    if (grid && e.which === apputils.HotKeys.Home) //Grid focus hotkey home is different
                    {
                        e.preventDefault();
                        grid.wrapper.children(".k-grid-pager").find(".k-pager-first").trigger('click');
                    }
                    else if (grid && e.which === apputils.HotKeys.End) {
                        e.preventDefault();
                        grid.wrapper.children(".k-grid-pager").find(".k-pager-last").trigger('click');
                    }
                    else {
                        let cellFocus = e.target.nodeName === 'INPUT';
                        let event = apputils.find(self.hotKeyEvents, evt => evt.hotKey === e.which)
                        if (!apputils.isUndefined(event) && !$(event.id).prop('disabled') && !cellFocus) {
                            e.preventDefault();
                            $(event.id).trigger(event.evt);
                        }
                    }
                });
            }

            if (this.mainUIVCRBindings.length > 0) {
                $("#" + this.mainUIVCRBindings[0].htmlCtrId).on('keydown', function (e) {
                    let event = apputils.find(self.hotKeyEvents, evt => evt.hotKey === e.which && evt.id.includes(e.currentTarget.id))

                    if (!apputils.isUndefined(event) && !$(event.id).prop('disabled')) {
                        e.preventDefault();
                        $(event.id).trigger(event.evt);
                    }
                });
            }
        },

        setImportExport: function () {
            if (this.importExportEventName) {
                sg.exportHelper.setExportEvent("btnOptionExport", this.importExportEventName, false, $.noop);
                sg.importHelper.setImportEvent("btnOptionImport", this.importExportEventName, false, $.noop);
            }
        },

        dropdownBoxes: [],

        /** Render the dropdown in the UI */
        renderDropdown: function () {
            let controls = apputils.isFunction(this.dropdownBoxes) ? this.dropdownBoxes() : this.dropdownBoxes;

            apputils.each(controls, (dropdown) => {
                $(dropdown.id).kendoDropDownList({
                    dataTextField: "display",
                    dataValueField: "value",
                    dataSource: dropdown.dataSource,
                }).data("kendoDropDownList").enable(dropdown.isEnabled);
            });
        },

        dateTimePickers: [],

        /** Render the date time pickers in the UI */
        renderDateTimePickers: function () {
            apputils.each(this.dateTimePickers, (dateTimePicker) => {
                $(dateTimePicker.id).kendoDatePicker(); //.data("kendoDatePicker").enable(dateTimePicker.isEnabled);
            });
        },

        hamBurgers: [],

        /** Render the hamburger button in the UI */
        renderHamburgers: function () {
            apputils.each(this.hamBurgers, (hb) => {
                const name = [sg.utls.labelMenuParams(hb.name, hb.caption)];
                LabelMenuHelper.initialize(name, hb.link);
            });
        },

        hasScreenContainerId: function () {
            return (apputils.isDefined(this.screenContainerId) && this.screenContainerId.length > 0);
        },

        getContainerBasedId: function (id, selector = "#") {
            if (this.hasScreenContainerId()) {
                return `${this.screenContainerId} ${selector}${id}`;
            } else {
                return id;
            }
        },

        appendScreenContainerId: function (name) {

            // Regular expression used to split strings by space.
            const nameSplitter = /\s+/;
            const selector = "";
            
            let str = "";

            if (nameSplitter.test(name)) {
                const names = name.split(nameSplitter);
                for (let i = 0; i < names.length; i++) {
                    //const spacer = i + 1 === names.length ? "" : " ";
                    //str +=  spacer + this.getContainerBasedId(names[i]);
                    if (this.hasScreenContainerId()){
                        str += " #" + this.getContainerBasedId(names[i], selector);
                    } else {
                        str += " " + this.getContainerBasedId(names[i], selector);
                    }
                }

                str = str.trim();

            } else if (name.length > 0) {
                str += "#" + this.getContainerBasedId(name, selector);
            }

            return str;

        },

        htmlControls: [],
        defaultRowIndex: 0,

        /** bind screen Html Controls */
        bindHtmlControls: function () {
            
            let controls = apputils.isFunction(this.htmlControls) ? this.htmlControls() : this.htmlControls;
            
            apputils.each(controls, (htmlControl) => {

                baseStaticControlfixtures.init(htmlControl.type, this.getContainerBasedId(htmlControl.id), `${htmlControl.viewid}${this.defaultRowIndex}`, htmlControl.field, htmlControl.defaultValue, htmlControl.customBinding);

            });
        },

        htmlControlsToggle: [],

        /** Bind the HTML controls toggle */
        bindHtmlControlsToggle: function () {
            let controls = apputils.isFunction(this.htmlControlsToggle) ? this.htmlControlsToggle() : this.htmlControlsToggle;

            apputils.each(controls, (htmlControl) => {
                //$(htmlControl.ids).prop(htmlControl.prop, htmlControl.condition);
                $(this.appendScreenContainerId(htmlControl.ids)).prop(htmlControl.prop, htmlControl.condition);
                
            });
        },

        pasteHandlerBindings: [],

        /** Bind the Paste controls toggle */
        setPasteHandlerBindings: function () {
            let self = this;
            apputils.each(this.pasteHandlerBindings, (htmlControl) => {
                self.bindPasteHandler(htmlControl.ids);
                
            });
        },

        customEventBinding: [],
        /** Bind to custom event */
        setCustomEventBindings: function () {
            let self = this;
            let ceb = apputils.isFunction(this.customEventBinding) ? this.customEventBinding() : this.customEventBinding;

            apputils.each(ceb, (obj) => {
                Keeler.listenTo(MessageBus.msg, obj.message, function (args) {
                    obj.callback(self, args);
                    //self[obj.callback].apply(args);
                });
            });
        },
         
        mainUIVCRBindings: [],
        /** Screen VCR navigation bottons bind click event handler */
        setMainUIVCRBindings: function () {
            let self = this;

            for (let i = 0; i < this.mainUIVCRBindings.length; i++) {
                let ctrIds = this.mainUIVCRBindings[i].htmlCtrId; //.htmlCtrId;
                let fields = this.mainUIVCRBindings[i].field;
                let navId = this.mainUIVCRBindings[i].navId || "";

                let btnFirstId = `#${this.getContainerBasedId('btnDataFirst')}${navId}`;
                let btnPrevId = `#${this.getContainerBasedId('btnDataPrevious')}${navId}`;
                let btnNextId = `#${this.getContainerBasedId('btnDataNext')}${navId}`;
                let btnLastId = `#${this.getContainerBasedId('btnDataLast')}${navId}`;

                // Handle screen key field is composite keys(BOM screen: [ItemNo, BomNo])
                if (!Array.isArray(ctrIds)) ctrIds = [ctrIds];
                if (!Array.isArray(fields)) fields = [fields];

                $(btnFirstId).on("click", function () {
                    apputils.activeElementId = ctrIds[0];
                    self.mainUIVCRGoFirst(ctrIds, fields);
                });
                self.hotKeyEvents.push({ id: btnFirstId, evt: 'click', hotKey: apputils.HotKeys.Home });
                $(btnFirstId).mousedown(() => {
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.First;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', true);
                });

                $(btnPrevId).on("click", function () {
                    apputils.activeElementId = ctrIds[0];
                    self.mainUIVCRGoPrevious(ctrIds, fields);
                });
                self.hotKeyEvents.push({ id: btnPrevId, evt: 'click', hotKey: apputils.HotKeys.PgUp });
                $(btnPrevId).mousedown(() => {
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.Previous;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', true);
                });

                $(btnNextId).on("click", function () {
                    apputils.activeElementId = ctrIds[0];
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.Next;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', true);
                    self.mainUIVCRGoNext(ctrIds, fields);

                });
                self.hotKeyEvents.push({ id: btnNextId, evt: 'click', hotKey: apputils.HotKeys.PgDn });
                $(btnNextId).mousedown(() => {
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.Next;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', true);
                });

                $(btnLastId).on("click", function () {
                    apputils.activeElementId = ctrIds[0];
                    self.mainUIVCRGoLast(ctrIds, fields);
                });
                self.hotKeyEvents.push({ id: btnLastId, evt: 'click', hotKey: apputils.HotKeys.End });
                $(btnLastId).mousedown(() => {
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.Last;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', true);
                });

                //by default set VCR naigation to none since VCR textbox has focus but VCR nigation has not started therefore user can either tab out or click any navigation button
                $('#' + ctrIds[0]).mousedown(() => {
                    self.mainCollectionObj.rows[0].navigationAction = NavigationAction.None;
                    $(`#${ctrIds[0]}`).data('offchangeEvent', false);
                });
            }
        },

        /**
         * Screen navigation first button click handler
         * @param {any} ctrIds navigation textbox ids array
         * @param {any} fields field ids array
         */
        mainUIVCRGoFirst: function (ctrIds, fields) {
            //GoFirst always pass empty filter
            this.mainUIVCRSearch(CRUDReasons.GotoFirstRecord, '');
            this.navigationAction = NavigationAction.First;
        },

        /**
         * Screen navigation previous button click handler
         * @param {any} ctrIds navigation textbox ids array
         * @param {any} fields field ids array
         */
        mainUIVCRGoPrevious: function (ctrIds, fields) {
            let filter = "";
            if (ctrIds.length === 1) {
                const formattedValue = this.formatVCRInputValue($(`#${ctrIds[0]}`).val());
                filter = this.mainCollectionObj.getKeysAndLessThanSearchFilter(formattedValue, fields[0]);
            } else {
                if ($('#' + ctrIds[0]).val()) {
                    filter = ctrIds.map(x => $('#' + x).val().replace(/[^a-zA-Z0-9$&%<>]/g, '')).join(',');
                }
            }
            let self = this;
            let btnFirstId = `#${this.getContainerBasedId('btnDataFirst')}${this.navId}`;
            let btnPrevId = `#${this.getContainerBasedId('btnDataPrevious')}${this.navId}`;

            //must create callback only once
            if (apputils.isUndefined(this.VCRGoPrevious)) {
                this.VCRGoPrevious = () => {
                    let disable = this.mainCollectionObj.noData;
                    $(btnFirstId).prop('disabled', disable);
                    $(btnPrevId).prop('disabled', disable);

                    $(`#${this.getContainerBasedId(self.recordTitleNumberId)}`).trigger("focus");

                    //AT-76352 - bypass ctrIds values and go to very first record
                    if (this.mainCollectionObj.noData) self.mainUIVCRGoFirst(null, fields);
                }
            }
            this.mainUIVCRSearch(CRUDReasons.GotoPreviousRecord, filter, this.VCRGoPrevious);
            this.navigationAction = NavigationAction.Previous;
        },

        /**
         * Screen navigation next button click handler
         * @param {any} ctrIds navigation textbox ids array
         * @param {any} fields field ids array
         */
        mainUIVCRGoNext: function (ctrIds, fields) {
            
            let filter = '';
            if (ctrIds.length === 1) {
                const formattedValue = this.formatVCRInputValue($(`#${ctrIds[0]}`).val());
                filter = this.mainCollectionObj.getKeysAndMoreThanSearchFilter(formattedValue, fields[0]);
            } else {
                if ($('#' + ctrIds[0]).val()) {
                    filter = ctrIds.map(x => $('#' + x).val().replace(/[^a-zA-Z0-9$&%]/g, '')).join(',');
                }
            }
            let self = this;
            let btnNextId = `#${this.getContainerBasedId('btnDataNext')}${this.navId}`;
            let btnLastId = `#${this.getContainerBasedId('btnDataLast')}${this.navId}`;
            //must create callback only once
            if (apputils.isUndefined(this.VCRGoNext)) {
                this.VCRGoNext = () => {
                    let disable = this.mainCollectionObj.noData;

                    $(btnNextId).prop('disabled', disable);
                    $(btnLastId).prop('disabled', disable);
                    $(`#${this.getContainerBasedId(self.recordTitleNumberId)}`).trigger("focus");

                    //AT-76352
                    if (this.mainCollectionObj.noData) self.mainUIVCRGoLast(ctrIds, fields);
                }
            }
            this.mainUIVCRSearch(CRUDReasons.GotoNextRecord, filter, this.VCRGoNext);
            this.navigationAction = NavigationAction.Next;
        },

        /**
         * Screen navigation last button click handler
         * @param {any} ctrIds navigation textbox ids array
         * @param {any} fields field ids array
         */
        mainUIVCRGoLast: function (ctrIds, fields) {
            //Should always go to last record even if the one passed doesn't exsist.
            let filter = ctrIds.length === 1 ? this.mainCollectionObj.getKeysAndLessThanSearchFilter("", fields[0]) : "";

            this.mainUIVCRSearch(CRUDReasons.GotoLastRecord, filter);
            this.navigationAction = NavigationAction.Last;
        },

        /** Get new template data */
        getNewTemplate: function () {
            this.getNewTemplateWithDirtyCheck();
        },

        /** Get screen key fields value, multiple key values join with delimiter '/' char */
        getKeysValue: function () {
            if (this.keysControlIds.length === 0) {
                this.keysControlIds.push(this.recordTitleNumberId);
            }
            let value = '';
            this.keysControlIds.forEach(id => {
                value += $('#' + id).val() + '/';
            });
            value = value.slice(0, -1);
            return value;
        },

        /** Delete record */
        deleteRow: function () {
            let self = this;
            let keyId = self.getKeysValue();

            let deleteCallback = function () {
                self.mainCollectionObj.getNewTemplate();
                sg.utls.showMessage({ UserMessage: { IsSuccess: true, Message: kendo.format(globalResource.DeleteSuccessMessage, keyId) } });
            };

            this.mainCollectionObj.deleteCallback = deleteCallback;
            this.mainCollectionObj.deleteRow();
        },

        /** Save record, show save successful message */
        saveRecord: function () {
            ErrorEntityCollectionObj.clearError();

            let self = this;
            
            let CRUDReason = this.mainCollectionObj.rows[0].CRUDReason;

            let Ok = this.mainCollectionObj.saveRecord();

            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((result, status, xhr) => {
                const errors = ErrorEntityCollectionObj.getErrors();

                if (xhr.hasError || errors.length > 0) {
                    const errorMsg = { "UserMessage": {"IsSuccess": false, "Errors": errors} };
                    sg.utls.showMessage(errorMsg, () => ErrorEntityCollectionObj.clearError());
                } else if(self.showSaveMessage) {
                    let message = CRUDReason === CRUDReasons.ExistingData ? globalResource.SaveSuccessMessage : kendo.format(globalResource.AddSuccessMessage, self.recordTitle, self.getKeysValue());
                    sg.utls.showMessage({ UserMessage: { IsSuccess: true, Message: message } });
                    MessageBus.msg.trigger('SaveSuccessful', {});
                }

            });

            return Ok;
        },

        /** Post saves the record first. If already saved, just do post
         * Errors/warning messages during saving are displayed and posting stops. 
         * If no error occur during saving then the record is posted. 
         */
        postData: function () {
            /**
             * Inner function for post
             * @param {any} self this context
             */
            function post(self) {
                let Ok = self.mainCollectionObj.postData();
                if (apputils.isUndefined(Ok)) {
                    return;
                }
                Ok.then((result, status, xhr) => {
                    if (!xhr.hasError) {
                        sg.utls.showMessage({ UserMessage: { IsSuccess: true, Message: jQuery.validator.format(globalResource.PostSuccessMessage, self.recordTitle, $(`#${self.recordTitleNumberId}`).val()) } });
                    }
                });
            }

            let self = this;
            let OkSave = this.mainCollectionObj.saveRecord(false);

            if (apputils.isUndefined(OkSave)) {
                return post(self);
            }

            OkSave.then((result, status, xhr) => {
                if (xhr.hasError) return;
                post(self);
            });
        },

        /**
         * Screen VCR navigation search function
         * @param {any} CRUDReason Navigation action
         * @param {any} filter fitler string
         * @param {any} callBack call back function
         */
        mainUIVCRSearch: function (CRUDReason, filter, callBack) {
            let self = this;
            let dirtyCheck = false;

            //AT-75830: for composite keys dirty checking
            if (this.dirtyCheckFieldName) {
                dirtyCheck = this.mainCollectionObj.rows[0].isFieldDirty(this.dirtyCheckFieldName);
            }

            if ((this.mainCollectionObj && !this.mainCollectionObj.noData && this.mainCollectionObj.isDirty()) || dirtyCheck) {
                sg.utls.showKendoConfirmationDialog(
                    function () { // Yes
                        self.doVCR(CRUDReason, filter, callBack);
                    },
                    function () { // No
                        return;
                    },
                    jQuery.validator.format(globalResource.SaveConfirm, self.recordTitle, $(`#${self.recordTitleNumberId}`).val()));
            } else {
                this.doVCR(CRUDReason, filter, callBack);
            }

        },

        /** Get screen new template data with screen dirty check */
        getNewTemplateWithDirtyCheck: function () {
            let self = this;
            if (this.mainCollectionObj.isDirty()) {
                sg.utls.showKendoConfirmationDialog(
                    function () { // Yes
                        ErrorEntityCollectionObj.clearError(); //clear the session errors from previous edits
                        self.isbtnApplyClicked = false; //reset flag
                        self.mainCollectionObj.getNewTemplate();
                    },
                    function () { // No
                        return;
                    },
                    jQuery.validator.format(globalResource.SaveConfirm, self.recordTitle, ($(`#${self.recordTitleNumberId}`).val())));
            } else {
                this.mainCollectionObj.getNewTemplate();
            }
        },

        executeYesAfterUserConfirmation: function () {
            
        },

        executeNoAfterUserConfirmation: function () {
            
        },

        executeWhenNotDirty: function () {
        },

        /** Execute multiple call back functions with confirmation */
        executeCallBacks: function () {
            let len = this.waitForUserConfirmation.length;

            for (let i = 0; i < len; i++) {
                let fc = this.waitForUserConfirmation[i];
                fc();
                this.waitForUserConfirmation.shift();
            }

            delete this.waitForUserConfirmation;
        },

        /**
         * Execute multiple call back functions with confirmation and dirty check
         * @param {any} condition Screen dirty result
         * @param {any} recordValue field value
         */
        executeUserChangesWithDirtyCheck: function (condition, recordValue) {
            let self = this;
            if (condition /*this.mainCollectionObj.isDirty()*/) {
                this.waitForUserConfirmation = [];
                let value = recordValue || $(`#${self.recordTitleNumberId}`).val();

                sg.utls.showKendoConfirmationDialog(
                    function () { // Yes
                        self.executeYesAfterUserConfirmation();
                        self.executeCallBacks(true);
                    },
                    function () { // No
                        self.executeNoAfterUserConfirmation();
                        self.executeCallBacks(false);
                        return;
                    },
                    jQuery.validator.format(globalResource.SaveConfirm, self.recordTitle, (value)));
            } else {
                self.executeWhenNotDirty();
            }
        },

        /**
         * Execute VCR navigation actions
         * @param {any} CRUDReason Navigation action
         * @param {any} filter Filter string
         * @param {any} callBack Call back function
         * @param {any} callBackEx Extra call back function. Used for screen to execute extra function 
         */
        doVCR: function (CRUDReason, filter, callBack, callBackEx) {
            /*let filterFn = (viewid) => viewid === this.mainCollectionObj.viewid ? filter : "";

            let query = this.mainCollectionObj.generateVCRRoot(filterFn, "", CRUDReason);
            
            this.VCRWorkerObj.callback = this.mainCollectionObj.callback;
            */

            let filterFn = (viewid) => viewid === this.VCRWorkerObj.viewid ? filter : "";

            let query = this.VCRWorkerObj.generateVCRRoot(filterFn, "", CRUDReason);

            this.VCRWorkerObj.callback = this.VCRWorkerObj.callback || this.mainCollectionObj.callback;

            this.VCRWorkerObj._executeXSearch2(query).then(() => {
                //this is to handle VCR after record is fetched
                if (!apputils.isUndefined(callBack)) {
                    callBack();
                }
                if (!apputils.isUndefined(callBackEx)) {
                    callBackEx();
                }
            });
        },

        //override this function to implement other features. see PMReviseEstimatesScreen.js
        VCRCallback: function () {
            this.mainCollectionObj.callback = (data) => {
                this.mainCollectionObj.rows = [];
                this.mainCollectionObj.rows.push(data);

                if (apputils.isUndefined(data)) {
                    return;
                }
                //rows beyond this can be updated therefore set the flag
                this.mainCollectionObj.rowIsReadonly = false;
                this.mainCollectionObj.loadDataFromFinder();
            };
        },

        /**
         * handle navigation buttons status: enabled/disabled.
         * @param {any} navId
         * @param {any} focus
         */
        handleNavigationButtons: function (navId, focus = true) {
            navId = navId || '';
            const btnFirstId = `#${this.getContainerBasedId('btnDataFirst')}${navId}`;
            const btnPrevId = `#${this.getContainerBasedId('btnDataPrevious')}${navId}`;
            const btnNextId = `#${this.getContainerBasedId('btnDataNext')}${navId}`;
            const btnLastId = `#${this.getContainerBasedId('btnDataLast')}${navId}`;
            const keyId = this.getContainerBasedId(this.recordTitleNumberId);

            $(btnFirstId).prop('disabled', this.navigationAction === NavigationAction.First);
            $(btnPrevId).prop('disabled', this.navigationAction === NavigationAction.First);

            $(btnNextId).prop('disabled', this.navigationAction === NavigationAction.Last);
            $(btnLastId).prop('disabled', this.navigationAction === NavigationAction.Last);

            const disable = $(`#${keyId}`).val() === this.previousKeyValue;
            if (this.navigationAction === NavigationAction.Next) {
                $(btnNextId).prop('disabled', disable);
                $(btnLastId).prop('disabled', disable)
            }
            if (this.navigationAction === NavigationAction.Previous) {
                $(btnFirstId).prop('disabled', disable);
                $(btnPrevId).prop('disabled', disable);
            }

            if (focus) {
                $(`#${keyId}`).trigger("focus");
            }
            //reset after use
            this.navigationAction = NavigationAction.None;
            $(`#${keyId}`).data('offchangeEvent', false);
        },

        /**
         * Binding Navigation action to hot key. Navigation by hot key
         * @param {any} navId Navigation action
         * @param {any} keyFieldId key field textbox id
         */
        bindNavigationButtons: function (navId, keyFieldId) {
            navId = navId || '';
            const btnFirstId = `#${this.getContainerBasedId('btnDataFirst')}${navId}`;
            const btnPrevId = `#${this.getContainerBasedId('btnDataPrevious')}${navId}`;
            const btnNextId = `#${this.getContainerBasedId('btnDataNext')}${navId}`;
            const btnLastId = `#${this.getContainerBasedId('btnDataLast')}${navId}`;

            $(`#${keyFieldId}`).on('keydown', e => {
                switch (e.key) {
                    case "ArrowLeft":
                        e.preventDefault();
                        $(e.ctrlKey ? btnFirstId : btnPrevId).trigger('click');
                        break;
                    case "ArrowRight":
                        e.preventDefault();
                        $(e.ctrlKey ? btnLastId : btnNextId).trigger('click');
                        break;
                }
            })
        },

        bindParams: function () { },

        //TODO: lets see how many kendo/control type functions are needed before we move them into their object
        /**
         * Update kendo DropDown List Data
         * @param {any} ctrlId Control html element id
         * @param {any} data data
         */
        updatekendoDropDownListData: function (ctrlId, data) {
            let ddList = $("#" + ctrlId).data("kendoDropDownList");
            ddList.dataSource.options.data = data;
            ddList.dataSource.transport.data = data;
            ddList.dataSource.read();
        },

        /** Load company profile record data */
        loadCompanyProfile: function () {
            //don't load again
            if (apputils.companyProfile) return apputils.noPromiseOk;

            let self = this;

            this.CS0001CompanyProfileEntityColl.rowIsReadonly = true;
            let Ok = this.CS0001CompanyProfileEntityColl.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {

                if (xhr.hasError) {
                    trace.log("CS0001CompanyProfileEntity - Search Error");
                    return;
                }
                //attach to apputils so that all object can access same companyProfile object
                apputils.companyProfile = self.CS0001CompanyProfileEntityColl;
            });

            return Ok;
        },

        /** Load currency codes data */
        loadCurrencyCodes: function () {

            //don't load again
            if (apputils.currencyCodes) return apputils.noPromiseOk;

            let self = this;

            this.CS0003CurrencyCodesEntityV3Coll.rowIsReadonly = true;
            let Ok = this.CS0003CurrencyCodesEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {
                if (xhr.hasError) {
                    trace.log("CS0003CurrencyCodesEntityV3Coll - load Error");
                    return;
                }

                //attach to apputils so that all object can access same currencyCodes object
                apputils.currencyCodes = self.CS0003CurrencyCodesEntityV3Coll;
            });
            return Ok;
        },

        /** Load Fiscal Calendars data */
        loadFiscalCalendars: function () {

            //don't load again
            if (apputils.fiscalCalendars) return apputils.noPromiseOk;

            let self = this;

            this.CS0002FiscalCalendarsEntityV3Coll.rowIsReadonly = true;
            let Ok = this.CS0002FiscalCalendarsEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {

                if (xhr.hasError) {
                    trace.log("CS0002FiscalCalendarsEntityV3Coll - load Error");
                    return;
                }
                //attach to apputils so that all object can access same fiscalCalendars object
                apputils.fiscalCalendars = self.CS0002FiscalCalendarsEntityV3Coll;
            });
            return Ok;
        },

        /** Load Currency Table data */
        loadCurrencyTable: function () {
            //don't load again
            if (apputils.currencyTable) return apputils.noPromiseOk;

            let self = this;

            this.CS0005CurrencyTablesEntityV3Coll.rowIsReadonly = true;
            let Ok = this.CS0005CurrencyTablesEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {

                if (xhr.hasError) {
                    trace.log("CS0005CurrencyTablesEntityV3Coll - load Error");
                    return;
                }
                //attach to apputils so that all object can access same fiscalCalendars object
                apputils.currencyTable = self.CS0005CurrencyTablesEntityV3Coll;
            });
            return Ok;
        },

        /** Load IC module settings options */
        loadICOptions: function () {

            //don't load again
            if (apputils.icOptions) return apputils.noPromiseOk;

            let self = this;

            this.IC0380ICOptionsEntityV3Coll.rowIsReadonly = true;
            let Ok = this.IC0380ICOptionsEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {

                if (xhr.hasError) {
                    trace.log("IC0380ICOptionsEntityV3Coll - Search Error");
                    return;
                }

                //attach to apputils so that all object can access same IC OptionsEntity object
                apputils.icOptions = self.IC0380ICOptionsEntityV3Coll;
            });
            return Ok;
        },

        /** Load PO module settings options */
        loadPOOptions: function () {
            //don't load again
            if (apputils.poOptions) return apputils.noPromiseOk;

            let self = this;

            this.PO0600PurchaseOrderOptionsEntityV3Coll.rowIsReadonly = true;
            let Ok = this.PO0600PurchaseOrderOptionsEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {
                if (xhr.hasError) {
                    trace.log("PO0600PurchaseOrderOptionsEntityV3Coll - Search Error");
                    return;
                }
                //attach to apputils so that all object can access same PO OptionsEntity object
                apputils.poOptions = self.PO0600PurchaseOrderOptionsEntityV3Coll;
            });
            return Ok;
        },

        /** Load CS module Optional fields */
        loadCSOptionalFields: function () {

            //don't load again
            if (apputils.CSOptionalFields) return apputils.noPromiseOk;

            let self = this;

            this.CS0011OptionalFieldsEntityV3Coll.rowIsReadonly = true;
            let Ok = this.CS0011OptionalFieldsEntityV3Coll.executeXSearch2("", "");
            if (apputils.isUndefined(Ok)) {
                return;
            }

            Ok.then((resp, status, xhr) => {
                if (xhr.hasError) {
                    trace.log("CS0011OptionalFieldsEntityV3Coll - Search Error");
                    return;
                }
                //attach to apputils so that all object can access same this object
                apputils.CSOptionalFields = self.CS0011OptionalFieldsEntityV3Coll;
            });
            return Ok;
        },

        /**
         * Paste handler function
         * @param {any} id Textbox id
         */
        bindPasteHandler: (id) => {
            $(id).bind("paste", function (e) {
                const pasteText = (e.originalEvent || e).clipboardData.getData('text/plain');
                if (pasteText.match(/[_\W]/)) {
                    e.preventDefault();
                }
            });
        },

        /**
         * Kendo Tree View context menu handler function
         * @param {any} treeviewId
         */
        treeViewContextMenuHandler: (treeviewId) => {
            const treeview = $("#" + treeviewId).data("kendoTreeView");
            $("#treeviewMenu").kendoContextMenu({
                target: "#" + treeviewId,
                filter: ".k-in",
                select: function (e) {
                    const button = $(e.item);
                    const cmd = button.text();
                    switch (cmd) {
                        case globalResource.ExpandAll:
                            treeview.expand(".k-item");
                            break;
                        case globalResource.CollapseAll:
                            treeview.collapse(".k-item");
                            setTimeout(() => {
                                treeview.select(".k-first");
                            }, 200);
                            break;
                    }
                }
            });
        },

        /**
         * screen controls status toggle strategy
         * @param {any} context Context
         */
        toggleStrategy: function (context) {

            let toggle = function () {
                this.context = context;
                this.handler = "";
            }

            toggle.prototype = {
                set: function (handler) {
                    handler.context = this.context;
                    this.handler = handler;
                },

                run: function () {
                    return this.handler.run();
                }
            };

            return new toggle();
        },

        /** Execute screen toggle strategy*/
        executeToggle: function () {

            let toggle = new this.toggleStrategy(this);

            apputils.each(this.bindTogglehandlers(), (handler) => {
                toggle.set(new handler());
                toggle.run();
            });

            //call VCR buttons so they are set to correct state
            this.handleNavigationButtons(this.navId, false);
        },

        formatVCRInputValue: function (val) {
            return val;
        },

        /**
         * Go to event handler after completion of event
         * @param {any} name
         * @param {any} fnc Callback function
         */
        onCompleteGoToEventHandler: function (name, fnc) {
            Keeler.listenTo(MessageBus.msg, name, ()=> {
                fnc();

                Keeler.stopListening(MessageBus.msg, name);
            });
        },

        /** Load screen settings options */
        preRender: function (okList = {}) {
            okList.currencyTable = this.loadCurrencyTable();
            okList.IC = this.loadICOptions();
            okList.PO = this.loadPOOptions();
            okList.companyProfile = this.loadCompanyProfile();
            okList.currencyCodes = this.loadCurrencyCodes();
            okList.fiscalCalendars = this.loadFiscalCalendars();

            return this;
        },
        /** Screen render */
        render: function () {
            this.initMainObject();
            let self = this;
            self.makeDocReady();
        },

        //Add custom UI controls for bindings
        addCustomHtmlControls: function (customFields) {
            customFields.forEach(f => {
                let uiType = 'input';
                let type = f.Type.replace("Type=", '').replaceAll(`"`, ``).toLowerCase();
                switch (type) {
                    case "dropdown":
                        uiType = 'dropdownbox';
                        break;
                    case "checkbox":
                        uiType = type;
                }
                let id = f.ID.replace("ID=", '').replaceAll(`"`, ``);
                let viewid = f.ViewId.replace("ViewId=", '').replaceAll(`"`, ``);
                let field = f.Field.replace("Field=", '').replaceAll(`"`, ``);
                this.htmlControls.push({ field: field, id: id, type: uiType, viewid: viewid });
            })
        },

        addCustomFields: function () {
            const customFields = this.viewModel ? this.viewModel.CustomFields : [] ;
            if (customFields && customFields.length > 0) {
                this.addCustomHtmlControls(customFields);
            }
        },

        /** Screen load ready to execute actions */
        makeDocReady: function () {
            let self = this;

            $(document).ready(function(e) {
                self.windowBeforeUnload();
                self.addCustomFields();
                self.setMainUIVCRBindings();

                self.renderDropdown();
                self.renderDateTimePickers();
                self.renderHamburgers();
                self.bindAllFinderListeners();
                self.assignGrids();
                self.assignTrees();
                self.bindEvents();
                self.bindHtmlControls();
                self.postRender();

                self.initDataLoad();

                self.bindHtmlControlsToggle();
                self.setPasteHandlerBindings();
                self.VCRCallback();
                self.setCustomEventBindings();
                self.bindHotKeyEvents();
            });
        },

        /** After screen render, to execute function. Override in screen  */
        postRender: function () { },

        //Template gets loaded by default but can override this function to retrieve other records when screen loads for the first time

        /** Initiate data load */
        initDataLoad: function () {
            let self = this;

            //For inquiry only access user, can't create template, just go to first record. 
            if (this.viewModel && this.viewModel.HasInquiryOnlyAccess) {
                setTimeout(() => $('#btnDataFirst').trigger('click'));
                return;
            }

            //mark ready only to get template
            this.mainCollectionObj.rowIsReadonly = true;
            this.mainCollectionObj.getTemplate().then(() => {
                if (apputils.isFunction(self.mainCollectionObj.deletePrePopDataFromGrid)) {
                    self.mainCollectionObj.deletePrePopDataFromGrid();
                }
            });
        },

        isCallFromMainUI: function (popupId) {

            return apputils.isUndefined($("#" + popupId).data("kendoWindow")) ? true : $("#" + popupId).data("kendoWindow").element.is(":hidden");
        },

        /**
         * Set focus to the field
         * @param {any} field the field to set the focus to
         * @param {any} viewdid the viewid the field is associated with
         */
        setFocus: function (field, viewid) {
            //due to async nature need bit of delay allowing previous events to complete
            setTimeout(() => {

                let controls = [];
                if (apputils.isDefined(viewid)) {
                    controls = this.htmlControls().filter(i => i.field === field && i.viewid === viewid);
                } else {
                    controls = this.htmlControls().filter(i => i.field === field);
                }

                if (controls.length > 0) {
                    $("#" + controls[0].id).trigger("focus");
                }
            });
            
        },

        setFocusByColumn: function (viewId, column) {
            const msg = viewId + column.rowIndex + column.field + apputils.EventMsgTags.svrUpdate;
            this.setFocusByEvent(msg);
        },

        setFocusByEvent: function (msg) {
            baseStaticControlfixtures.setFocus(msg);
        },

        setValueByColumn: function (viewId, column) {
            const msg = viewId + column.rowIndex + column.field + apputils.EventMsgTags.svrUpdate;
            this.setValueByEvent(msg, column.value);
        },

        setValueByEvent: function (msg, value) {
            baseStaticControlfixtures.setValue(msg, value);
        },

        setValueAndFocus: function (field) {
            this.setFocusByColumn(field.viewid, field);
            this.setValueByColumn(field.viewid, field);
        },

        concatEventMessages: function (messageObj) {
            let result = [];

            for (let key in messageObj) {
                result.push(messageObj[key]);
            }

            return result.join(' ');
        },

        getSvrUpdateMessageName: function (id) {
            return this.getContainerBasedId(id) + apputils.EventMsgTags.svrUpdate;
        },

        setCurrentRowIndex: function () {
            const selectedRowIndex = this.detailsGrid().selectedRowIndex;
            return selectedRowIndex === -1 ? 0 : selectedRowIndex;
        },

        mAppControls: function (cntrlId) {
            let self = this;

            return {
                set UIEnabledFlag(v) {
                    $("#" + self.getContainerBasedId(cntrlId)).prop("disabled", !v);
                },

                set UIVisibleFlag(v) {
                    const dropDownList = $("#" + self.getContainerBasedId(cntrlId)).data("kendoDropDownList");

                    if (apputils.isDefined(dropDownList)) {
                        $('#' + self.getContainerBasedId(cntrlId)).closest(".k-widget")[v ? 'show' : 'hide']();

                    } else {
                        $('#' + self.getContainerBasedId(cntrlId))[v ? 'show' : 'hide']();
                    }
                }

            }
        },

        //.Caption
        //.value
        //.Text (can be label or dropdown)
        //.ListIndex ( dropdown) (not sure what's this for)
        //.ListCount ( dropdown) (not sure what's this for)
        //ViewFinderButton
        fieldEditControl: function (cntrlId, extraControlObj = {}) {
            let self = this;

            let dropDownList = $("#" + self.getContainerBasedId(cntrlId)).data("kendoDropDownList");

            let fncDDSelect = function (method, value, propt) {
                if (dropDownList) {
                    dropDownList.select((dataItem) => {
                        //using implicit comparsion since value and status may be string and number
                        return dataItem.value == value;
                    });
                }

                self[cntrlId][propt] = value;

                return value;
            };

            let fncDDGet = function (method, propt) {
                if (dropDownList) {
                    return dropDownList.text();
                }

                return self[cntrlId][propt];
            };

            let isDropDownBox = apputils.isDefined(dropDownList);
            let isInputBox = $("#" + self.getContainerBasedId(cntrlId)).is("input");

            let fncSet = function (method, value, propt) {

                $("#" + self.getContainerBasedId(cntrlId))[method](value);
                self[cntrlId][propt] = value;

                return value;

            };

            let fncGet = function (method, propt) {
                if ($("#" + self.getContainerBasedId(cntrlId)).length === 0) {
                    return self[cntrlId][propt];
                }

                return $("#" + self.getContainerBasedId(cntrlId))[method]();
            };

            let fncSetProp = function (method, name, value, ctr = cntrlId) {

                $("#" + self.getContainerBasedId(ctr))[method](name, value);
                //self[ctr][name] = value;

                return value;

            };

            let fncGetProp = function (method, name, ctr = cntrlId) {
                if ($("#" + self.getContainerBasedId(ctr)).length === 0) {
                    return; //self[ctr][name];
                }

                return $("#" + self.getContainerBasedId(ctr))[method](name);
            };

            let Refresh = function (v) {
                fncDDSelect('text', v, 'Refresh');
            };

            let ListIndex = function () {
                if (dropDownList) {
                    return dropDownList.value();
                }

                return "";
            };

            let List = function (v) {
                if (dropDownList) {
                    return dropDownList.text();
                }

                return "";
            };

            return {
                Refresh,
                ListIndex,
                List,

                get id() {
                    return self.getContainerBasedId(cntrlId);
                },

                set value(v) {
                    return fncSet('val', v, 'value');
                },

                get value() {
                    return fncGet('val', 'value');
                },

                set Caption(v) {
                    return fncSet('text', v, 'Caption');
                },

                get Caption() {
                    return fncGet('text', 'Caption');
                },

                set Text(v) {

                    if (isDropDownBox) {
                        return fncDDSelect('text', v, 'Text');

                    } else if (isInputBox) {
                        return fncSet('val', v, 'Text');
                    }
                    else {
                        return fncSet('text', v, 'Text');
                    }
                },

                get Text() {

                    if (isDropDownBox) {
                        return fncDDGet('text', 'Text');

                    } else if (isInputBox) {
                        return fncGet('val', 'Text');

                    }else {
                        return fncGet('text', 'Text');
                    }

                },

                set CurrencyCode(v) {
                    return fncSet('prop', v, 'CurrencyCode');
                },

                get CurrencyCode() {
                    return fncGet('prop', 'CurrencyCode');
                },

                set NumberDecimalsAfter(v) {
                    return fncSet('val', v, 'NumberDecimalsAfter');
                },

                get NumberDecimalsAfter() {
                    return fncGet('val', 'NumberDecimalsAfter');
                },

                set FieldName(v) {
                    return fncSetProp('prop', 'FieldName', v);
                },

                get FieldName() {
                    return fncGetProp('prop', 'FieldName');
                },

                set Checked(v) {
                    return fncSetProp('prop', 'checked', v);
                },

                get Checked() {
                    return fncGetProp('prop', 'checked');
                },

                set ViewFinderButton(v) {
                    return fncSetProp('prop', 'disabled', !v, extraControlObj.finderBtn);
                },

                get ViewFinderButton() {
                    return fncGetProp('prop', 'disabled', extraControlObj.finderBtn);
                },

                set AddItem(v) {
                    return "TODOForDropdown"; 
                }
            }
        },
    };

    this.viewLifeCycleObj = helpers.View.extend(domainViewLifeCycle);
    this.viewLifeCycle = helpers.View.extend({}, domainViewLifeCycle);

}).call(this);