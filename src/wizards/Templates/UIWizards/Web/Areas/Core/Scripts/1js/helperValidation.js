(function () {
    'use strict';

    this.compareType = {
        field: 0, 
        literal: 1,
        func: 2
    };
    Object.freeze(this.compareType);

    let objectValidation = {

        init: function () {
            this.addDefault();
            this.addMoreThan();
            this.addLessThan();
            this.addEqualTo();
        },

        /**
         * Get the value from the parameter
         * @param {any} pars The parameters
         */
        getValue: function (pars) {
            let newValue = pars.compareTo;
            let field;

            if (apputils.isUndefined(pars.compareType) || pars.compareType === compareType.field) {
                let columnInfo = baseStaticObjectValidation.context.getColumnFormatedValueByFieldName(pars.compareTo);
                newValue = columnInfo.value;
                field = columnInfo.field;
            } else if (pars.compareType === compareType.func) {
                
                newValue = pars.compareTo({ userValue: pars.userValue, validateObj: baseStaticObjectValidation.context.caller.objToValidate || baseStaticObjectValidation.context.caller});
                field = "empty"
            }
                    
            return {value: newValue, field: field};
        },

        /**
         * Default validation rule
         */
        addDefault: function () {
             let defaultRule = {
                title : 'Default Rule',
                validate : function (value, pars) {
                    return true;
                }
            };

            approve.addTest(defaultRule, 'defaultRule');
        },

        /**
         * Validation for value be more than
         */
        addMoreThan: function () {
             var moreThan = {
                title : 'moreThan',
                expects : ['compareType', 'compareTo'],
                messageTemplate : 'value must be more than {compareTo}.',
                validate : function (value, pars) {

                    let compareValue = objectValidation.getValue(pars);
                    
                    //this is the right place to replace messageTemplate msg since other rules have different message where replacement wouldn't make sense. 
                    this.message = this.messageTemplate.replace("{compareTo}", (apputils.isUndefined(compareValue.value.length) || compareValue.value.length>0) ? compareValue.value : compareValue.field);

                    return v8n().greaterThan(compareValue.value).test(value);
                }
            };

            approve.addTest(moreThan, 'moreThan');
        },

        /**
         * Validation for value be less than
         * */
         addLessThan: function () {
             let lessThan = {
                title : 'lessThan',
                expects : ['compareType', 'compareTo'],
                messageTemplate : 'value must be less than {compareTo}.',
                validate : function (value, pars) {

                    let compareValue = objectValidation.getValue(pars);
                    
                    //this is the right place for messageTemplate msg replace since other rules have different message where replacement wouldn't make sense. 
                    this.message = this.messageTemplate.replace("{compareTo}", (apputils.isUndefined(compareValue.value.length) || compareValue.value.length>0) ? compareValue.value : compareValue.field);

                    return v8n().lessThan(compareValue.value).test(value);

                }
            };

            approve.addTest(lessThan, 'lessThan');
        },

        /**
         * Validation for value be equal to
         */
        addEqualTo: function () {
            let equalTo = {
                title: 'equalTo',
                expects: ['compareType', 'compareTo'],
                messageTemplate: 'value must be equal to {compareTo}.',
                validate: function (value, pars) {

                    pars.userValue = value;

                    let compareValue = objectValidation.getValue(pars);

                    //this is the right place for messageTemplate msg replace since other rules have different message where replacement wouldn't make sense. 
                    this.message = this.messageTemplate.replace("{compareTo}", (apputils.isUndefined(compareValue.value.length) || compareValue.value.length > 0) ? compareValue.value : compareValue.field);

                    let test = approve.value(value, { equal: { value: compareValue.value, field: compareValue.field } }).approved;

                    return test === true;

                }
            };

            approve.addTest(equalTo, 'equalTo');
        },

        execute: function (rule, valueToValidate, context) {

            if (rule.required === false && valueToValidate.length === 0){
                return approve.value(valueToValidate, {defaultRule:{required:false}});
            }

            baseStaticObjectValidation.context = context;
            return approve.value(valueToValidate, rule);
        },
    }

    //this.baseValidation = helpers.View.extend(objectValidation); //works but not too sure if needed
    let helperValidation = this.baseStaticObjectValidation = helpers.View.extend({}, objectValidation);

    const approve = typeof require === 'function' ? require('approve') : window.approve;
    if (typeof define === 'function' && define.amd) {
        define('helperValidation', ['approve'], function () {
            
            helperValidation.init();
            return helperValidation;
        });
    } else {
        this.baseStaticObjectValidation.init();
    }
}).call(this);