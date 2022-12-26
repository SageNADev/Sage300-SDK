(function() {
    'use strict';

    let utils = {}; 

    const appOperators = utils.appOperators = apputils.Operators;
    const appDataType = utils.appDataType = apputils.DataType;

    /** Define query Helpers object */
    const queryHelpers = {
        operator1 : function(options){
			switch(options.operator){
                case (appOperators.StartsWith):
				case (appOperators.EndsWith):
				case (appOperators.Contains):
                case (appOperators.Like):
                    options.operator = " like ";
                    break;
                default:
                    options.operator = ` ${options.operator} `;
            }

            return options;
        },

		/**
		 * Convert filter start with operator to accpac view accept filter 
		 * @param {any} options operator options
		 */
		operator2StartsWith : function(options){
			switch(options.operator){
                case (appOperators.StartsWith):
				case (appOperators.Contains):
                case (appOperators.Like):
                    options.value = options.value + "%";
                    break;
                default:
					return options;
            }

            return options;
        },

        /**
         * Convert filter end with operator to accpac view accept filter
         * @param {any} options operator options
         */
		operator3EndsWith : function(options){
			switch(options.operator){
                case (appOperators.EndsWith):
				case (appOperators.Contains):
                case (appOperators.Like):
                    options.value = "%" + options.value;
                    break;
                default:
					return options;
            }

            return options;
		},
    };

    /** Field helper utility object */
    const fieldhelpers = {
        /**
         * Get mapped field
         * @param {any} mappedObject Mapped object
         * @param {any} mappedViewId Mapped view id
         * @param {any} fieldName field name
         */
        getMappedField : function(mappedObject, mappedViewId, fieldName){
			let mappedFields = apputils.keys(mappedObject);

			let field = apputils.find(mappedFields, (field)=>{
				return mappedObject[field][mappedViewId] === fieldName;
			});
			
			return field;
        },

		/**
		 * Get field object
		 * @param {any} modelDef Model
		 * @param {any} fieldName field name
		 */
		getField : function(modelDef, fieldName){
			
			let field = apputils.find(modelDef, (model)=>{
				return model.field === fieldName;
			});
			
			return field;
        },

        /**
         * Get field object by field name
         * @param {any} modelDef
         * @param {any} name
         */
        getFieldByName : function(modelDef, name){
			
			let field = apputils.find(modelDef, (model)=>{
				return model.name === name;
			});
			
			return field;
        },

		/**
		 * Format filter string, add double quote to string value
		 * @param {any} fieldName
		 * @param {any} operator
		 * @param {any} value
		 * @param {any} dataType
		 */
        formatQueryString: function(fieldName, operator, value, dataType){

            //Execution sequence : functions are run in reverse order 
            //Eg: operator2StartsWith, operator3EndsWith, operator1
            //let getFilterString = apputils.flowRight(queryHelpers.operator1, queryHelpers.operator3EndsWith, queryHelpers.operator2StartsWith);
            let getFilterString = apputils.compose(queryHelpers.operator1, queryHelpers.operator3EndsWith, queryHelpers.operator2StartsWith);

			let filterString = getFilterString({fieldName, operator, value});
			
			switch(dataType){
                case (appDataType.Decimal):
                case (appDataType.Int):
				case (appDataType.Long):
				case (appDataType.Bool):
				//case (appDataType.Date):
                    return filterString.fieldName + filterString.operator + filterString.value;
                default:
                    return filterString.fieldName + filterString.operator + "\"" + filterString.value + "\"";
            }
        },

        /**
         * Get search filter string
         * @param {any} field
         * @param {any} value
         */
        getSearchFilter: function (field, value) {
            return this.formatQueryString(field.name, "=", value, field.dataType);
        },

        /**
         * Build 'or' filter string
         * @param {any} field
         * @param {any} values
         */
        buildORFilter: function(/*dataModel,*/ field, values){
            let filterString = "";

            apputils.each(values, (value)=>{
                if (value.length > 0){
                    //filterString += (filterString.length === 0)? this.getSearchFilter(dataModel, field, value) : " or " + this.getSearchFilter(dataModel, field, value);
                    filterString += (filterString.length === 0) ? this.getSearchFilter(field, value) : " or " + this.getSearchFilter(field, value);
                }
            });

        
            return filterString;
        },
    };

    //all 'utils' are exposed via apputils
    apputils.fields = apputils.extend({}, fieldhelpers);

}).call(this);