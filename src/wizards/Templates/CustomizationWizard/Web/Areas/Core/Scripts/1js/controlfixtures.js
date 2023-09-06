(function () {
    'use strict';

    var root = this;
    var baseStaticControlfixtures;
	if (typeof exports !== 'undefined') {
        baseStaticControlfixtures = exports;
	} else {
        baseStaticControlfixtures = root.baseStaticControlfixtures = {};
    }

    /** Screen control fixtures Object  */
    var controlfixtures = {
        control: {
            ctrlid : -1,
            ctrlType : '', 
            viewid : '',
            field : '',
            defaultValue : '',
            customBinding : ''
            },

        controls : [],
        controlEvent: {
            msg: '',
            controls : []    
            },
        /**
         * Screen control initialize
         * @param {any} ctrlType Control type
         * @param {any} ctrlid Control html elemnt id
         * @param {any} viewid Accpac view id
         * @param {any} field Control binding model field
         * @param {any} defaultValue Control default value
         * @param {any} customBinding Control custom binding function name
         */
        init: function(ctrlType, ctrlid, viewid, field, defaultValue, customBinding){

            //undefined field means not bound to any data model
            if (apputils.isUndefined(field) || field === ""){
                return;
            }

            var self = this;

            var newControl = apputils.cloneDeep(self.control); //clone(self.control);
            newControl.ctrlid = ctrlid;
            newControl.ctrlType = ctrlType;
            newControl.viewid = viewid;
            newControl.field = field;
            newControl.defaultValue = defaultValue;
            newControl.customBinding = customBinding;
    
            var msgFromServer = viewid + field + apputils.EventMsgTags.svrUpdate;

            this._controlEvents || (this._controlEvents = {});
            var events = this._controlEvents[msgFromServer] || (this._controlEvents[msgFromServer] = []);
            events.push({ctrlType: ctrlType, ctrlid: ctrlid, viewid: viewid, field: field, defaultValue: defaultValue, customBinding: customBinding});

            //set all listenTo to apputils.EventMsgTags.userctx to separate other listners on the same message
            Keeler.listenTo(MessageBus.msg, viewid + field + apputils.EventMsgTags.svrUpdate, self.UpdateFromServer, apputils.EventMsgTags.userctx);
            Keeler.listenTo(MessageBus.msg, viewid + field + apputils.EventMsgTags.svrValid, self.Valid, apputils.EventMsgTags.userctx);
            Keeler.listenTo(MessageBus.msg, viewid + field + apputils.EventMsgTags.svrInvalid, self.Invalid, apputils.EventMsgTags.userctx);

            $(document).ready(function () {
                //Note: onchange event fires whenever the value changes therefore this can be called more times. This is okay when
                //calling only client side objects but not good for making server calls. Changing default to onBlur.
                //let event = "blur";

                //Oct 26 - changed back to "change" since focus event wasn't working properly
                let event = "change";

                //some control must have their own event type
                if (ctrlType === "checkbox"){
                    event = "click";
                } else if (ctrlType === "dropdownbox"){
                    event = "change";
                }

                //fixed AT-77045 - now apputils.activeElementId can be set as needed. But all user input in text box will clear out this setting. 
                $('#' + ctrlid).mousedown(() => {
                    apputils.activeElementId = "";

                });

                $("#" + ctrlid).on(event, function (data) {

                    if ($("#" + ctrlid).data('offchangeEvent')) {
                        return;
                    }

                    //when focus on the control(textbox) and click finder button, not trigger change event. See AT-76579
                    if (window.activeElementId && window.activeElementId === ctrlid) {
                        window.activeElementId = '';
                        return;
                    }


                    window.activeElementId = '';
                    MessageBus.msg.trigger(viewid + field + apputils.EventMsgTags.usrUpdate, { rowIndex: 0, field: field, value: getData(data.currentTarget), customBinding: customBinding });
                });

                $("#" + ctrlid).on(apputils.EventTrigger.System, function (data) {
                    MessageBus.msg.trigger(viewid + field + apputils.EventMsgTags.usrUpdate, { rowIndex: 0, field: field, value: getData(data.currentTarget), eventTrigger: apputils.EventTrigger.System, customBinding: customBinding });

                });

                /**
                 * Get control value
                 * @param {any} input control
                 */
                function getData(input) {
                    if (input.type === "checkbox") {
                        return input.checked ? "True" : "False";

                    } else if (input.type === "dropdownbox"){
                        return input.selected;
                    } 
                    else {
                        const upperCase = input.className && input.className.includes('txt-upper');
                        return upperCase ? input.value.trim().toUpperCase() : input.value.trim();
                    }
                }
            });
        },

        /**
         * Update control value from server data 
         * @param {any} data data from server
         * @param {any} msg The message index
         */
        UpdateFromServer: function(data, msg){
            let controls = baseStaticControlfixtures._controlEvents[msg];

            //TODO: check if UpdateFromServer is not called as many times the same msg is bound to controls therefore
            //apputils.each will be executed multiple times as well which may be bit inefficient. 
            apputils.each(controls, (control)=> {
                let ctrl = $("#" + control.ctrlid);
                if (ctrl) {
                    if (control.ctrlType === "checkbox") {
                        if (ctrl[0]) {
                            ctrl[0].checked = control.defaultValue === data;
                            if (!control.defaultValue && !isNaN(data)) {
                                ctrl[0].checked = parseFloat(data) > 0;
                            }
                        } else {
                            return;
                        }
                    } else if (control.ctrlType === "dropdownbox") {
                        let dropDownList = $(ctrl).data("kendoDropDownList");
                        if (dropDownList) {
                            dropDownList.select((dataItem) => {
                                return dataItem.display === data;
                            });
                        }
                    } else if (control.ctrlType === "input"){
                        data = apputils.unescape(apputils.unescape(data)); //double unescape as server side escape extra time.
                        ctrl.val(data);
                    }
                    else {
                        data = apputils.unescape(apputils.unescape(data)); //double unescape as server side escape extra time.
                        ctrl.text(data);
                    }

                    //[RC] 5/26/2022 - left for UI done prior to this date
                    MessageBus.msg.trigger(control.ctrlid + apputils.EventMsgTags.svrUpdate, { msg: "UpdatedByServer", value: data });
                    MessageBus.msg.trigger(control.viewid + control.ctrlid + apputils.EventMsgTags.svrUpdate, { msg: "UpdatedByServer", value: data});
                    //Add this message to simplify screen object message listening handlers( like formatting all numeric textbox)
                    MessageBus.msg.trigger(control.viewid + apputils.EventMsgTags.svrUpdate, { msg: "UpdatedByServer", value: data, ctrlId: control.ctrlid });
                }
            });
        },

        /**
         * When valid, hide the error
         * @param {boolean} success
         * @param {int} msg The error message index
         */
        Valid: function (success, msg) {
            let control = baseStaticControlfixtures._controlEvents[msg];
            let group = $("#" + control[0].ctrlid + '-help-block-override').length > 0 ? $("#" + control[0].ctrlid + '-help-block-override') : $("#" + control[0].ctrlid + 'help-block');
            
            //TODO: may be we want to display 'success' msg but lets ignore for now
            group.removeClass('has-error');
            group.html('').addClass('hidden');
        },

        /**
         * When invalid, show the error
         * @param {object[]} errors The errors object array
         * @param {int} msg The error message index
         */
        Invalid: function (errors, msg) {
            let control = baseStaticControlfixtures._controlEvents[msg];
            let group = $("#" + control[0].ctrlid + '-help-block-override').length > 0 ? $("#" + control[0].ctrlid + '-help-block-override') : $("#" + control[0].ctrlid + 'help-block');
        
            group.addClass('has-error');
            group.html(errors.join('</br>')).removeClass('hidden');

            trace.warn(`${control[0].ctrlid} - ${errors[0]}`);
        },

        /**
         * Set control focus
         * @param {any} msg
         */
        setFocus: function (msg) {
            let control = baseStaticControlfixtures._controlEvents[msg];
            if (apputils.isUndefined(control)) {
                return;
            }
            $("#" + control[0].ctrlid).trigger("focus"); 
        },

        /**
         * set control value
         * @param {any} msg
         * @param {any} value
         */
        setValue: function (msg, value) {
            let control = baseStaticControlfixtures._controlEvents[msg];

            if (apputils.isUndefined(control)) {
                return;
            }

            $("#" + control[0].ctrlid).val(value);
        }
    };

    apputils.extend(baseStaticControlfixtures, controlfixtures);

}).call(this);