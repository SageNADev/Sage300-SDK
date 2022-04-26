/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";
var ICOptionalFieldsUtils = ICOptionalFieldsUtils || {};

/*
 * The optional fields control expects a dropdown to select the optional fields,
 * a pair of finders, and another pair of dropdowns to select the values
 * On page load, call init().
 * On dropdown change, call rebuildControl() and setDefaultValue(), and set finders and labels
 */
ICOptionalFieldsUtils = {

    // Constants
    Constants: Object.freeze({
        ContainerDivClass: 'search-group',
        ContainerDivClassNumeric: 'numeric-group',
        ClassNumeric: 'numeric',
        ClassDefault: 'default',
        ClassTextUpper: 'txt-upper',
        ClassLarge: 'large',
        ClassMedium: 'medium',
        ClassSmaller: 'smaller',
        ClassDatePicker: 'datepicker',

        DefaultNumberMaxLength: 16,
        DefaultNumberLength: "18",

        DefaultIntegerLength: "10",
        MaxTimeLength: 8
    }),

    OptionalFieldTypeEnum: Object.freeze({
        Text: 1,
        Date: 3,
        Time: 4,
        Number: 6,
        Integer: 8,
        Boolean: 9,
        Currency: 100
    }),

    // list of optional fields
    optionalFields: [],

    // id of the dropdown
    dropdownId: '',

    // ids of helper controls
    fromTxtId: '', fromBtnId: '', fromDropdownId: '', toTxtId: '', toBtnId: '', toDropdownId: '', 

    // Text message for invalid dates
    txtInvalidDateString: '',

    /**
    * @function
    * @name init
    * @description Initializes the optional fields dropdown
    * @namespace ICOptionalFieldsUtils
    * @public
    * 
    * @param {string} dropdownId id of the specified dropdown
    * @param {string} fromTxtId id of the from helper textbox
    * @param {string} fromBtnId id of the from helper textbox finder button
    * @param {string} fromDropdownId id of the from helper dropdown
    * @param {string} toTxtId id of the to helper textbox
    * @param {string} toBtnId id of the to helper textbox finder button
    * @param {string} toDropdownId id of the to helper dropdown
    * @param {string} txtInvalidDateString Localized message for invalid date error
    */
    init: function (dropdownId, fromTxtId, fromBtnId, fromDropdownId, toTxtId, toBtnId, toDropdownId, txtInvalidDateString = 'Enter a valid date.') {
        // store control ids
        this.dropdownId = dropdownId;
        this.fromTxtId = fromTxtId
        this.fromBtnId = fromBtnId;
        this.fromDropdownId = fromDropdownId;
        this.toTxtId = toTxtId;
        this.toBtnId = toBtnId;
        this.toDropdownId = toDropdownId;
        this.txtInvalidDateString = txtInvalidDateString;

        this.initDropdown(dropdownId);
    },

    /**
    * @function
    * @name initDropdown
    * @description Appends IC optional fields to the specified dropdown
    * @namespace ICOptionalFieldsUtils
    * @public
    * 
    * @param {string} dropdownId id of the specified dropdown
    */
    initDropdown: function (id) {
        const dropdownlist = $(`#${id}`).data("kendoDropDownList");

        this.getOptionalFields().then((result) => {
            this.optionalFields = result;
            result.forEach((optionalField) => {
                dropdownlist.dataSource.add({
                    Value: optionalField.OptField,
                    LabelText: optionalField.Description,
                });
            });
            dropdownlist.dataSource.sync();
        });
    },

    /**
    * @function
    * @name getOptionalFields
    * @description Gets the IC Optional Fields
    * @namespace ICOptionalFieldsUtils
    * @public
    * 
    * @returns Promise with IC Optional Fields data
    */
    getOptionalFields: () => {
        const url = sg.utls.url.buildUrl("IC", "OptionalField", "GetOptionalFieldDescriptions");
        return sg.utls.ajaxPostWithPromise(url, {});
    },

    /**
    * @function
    * @name getFinderProperty
    * @description Gets the optional field finder property
    * @namespace ICOptionalFieldsUtils
    * @public
    * 
    * @param {string} type optional field type
    * @param {string} optionalField optional field field name
    * @param {string} id id of the finder textbox
    * @returns function of the finder property
    */
    getFinderProperty: function (type, optionalField, id) {
        const { Date, Time, Integer, Currency, Number, Text } = this.OptionalFieldTypeEnum;
        const props = sg.viewFinderProperties.CS;
        const deepCopy = sg.utls.deepCopy;

        return () => {
            let property = deepCopy(props.OptionalFieldValue);
            let value = $(`#${id}`).val();
            const obType = parseInt(type);
            switch (obType) {
                case Date:
                    property = deepCopy(props.DateOptionalFieldValue);
                    value = sg.utls.kndoUI.getDateYYYMMDDFormat(value);
                    break;
                case Time:
                    property = deepCopy(props.TimeOptionalFieldValue);
                    value = sg.utls.checkIfValidTimeFormat(value).replaceAll(':', '');
                    break;
                case Integer:
                    property = deepCopy(props.IntegerOptionalFieldValue);
                    break;
                case Currency:
                    property = deepCopy(props.AmountOptionalFieldValue);
                    break;
                case Number:
                    property = deepCopy(props.NumberOptionalFieldValue);
                    break;
                case Text:
                    property = deepCopy(props.TextOptionalFieldValue);
                    break;

                default:
                    break;
            }
            property.initKeyValues = [optionalField, value];
            property.filter = $.validator.format(property["filterTemplate"], optionalField);

            return property;
        }
    },

    /**
     * @function
     * @name rebuildControl
     * @description Rebuild a control
     * @namespace ICOptionalFieldsUtils
     * @public
     * 
     * @param {string} controlId The Control Id string
     * @param {object} fieldInfo The field info object
     */
    rebuildControl: function (controlId, fieldInfo) {
        // Get the field type
        const fieldType = parseInt(fieldInfo.TypeIndex);

        // Get the control reference
        var $control = $(controlId);

        let controlName = this.getNameFromId(controlId);

        // Remove all event handlers from the control
        $control.off();

        sg.utls.unmask(controlId);

        // Remove some unnecessary wrappers from these (if they exist)
        sg.utls.kndoUI.removeDatePicker(controlName);
        this.removeDropDown(controlName);
        this.removeNumeric(controlName);

        // Remove all attributes from control (except for a few)
        this.removeControlAttributes($control);

        // Create the actual control, if necessary
        this.createControl(controlId, $control, fieldInfo);

        // Reapply attributes to control based on field information
        this.applyControlAttributes($control, fieldInfo);

        // Ensure the control (finders and dropdowns) are contained in a class
        // appropriate for the control type 
        this.setContainerClass($control, fieldType);

        // This needs to be called AFTER the controls have been 're-created'
        this.applyFieldLengthRelatedProperties(controlId, $control, fieldInfo);

        // Now set the controls default field value
        this.setDefaultValue(controlId, fieldInfo);

        // Setup the 'change' event handlers
        this.initChangeEvents($control, fieldInfo);
    },

    /**
     * @function
     * @name createControl
     * @description If necessary, create the kendo control
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlId The Control Id string
     * @param {object} $control The jquery wrapped object reference
     * @param {object} fieldInfo The field info object
     *
     * @returns Either the attribute value or the default value
     */
    createControl: function (controlId, $control, fieldInfo) {
        const { Date, Time, Boolean } = this.OptionalFieldTypeEnum;

        // Get the field type
        const fieldType = parseInt(fieldInfo.TypeIndex);

        // Get the control name (from controlId)
        const controlName = this.getNameFromId(controlId);

        if (fieldType === Date) {

            sg.utls.kndoUI.datePicker(controlName);

        } else if (fieldType === Time) {

            sg.utls.maskTimeformat(controlId);

        } else if (this.isNumericField(fieldType)) {
            const isDecimal = this.isDecimalNumericField(fieldType);
            const decimalVal = isDecimal ? this.getAttributeValueIfDefined($control, 'decimal', fieldInfo.OptionsDecimal) : 0;
            let maxLength = isDecimal ? this.Constants.DefaultNumberLength : this.Constants.DefaultIntegerLength;
            maxLength = (maxLength > this.Constants.DefaultNumberMaxLength) ? this.Constants.DefaultNumberMaxLength : maxLength;

            let minValue = null;
            let maxValue = null;
            let formatVal = null;

            if (isDecimal) {
                //
                // Floating point numerics
                //
                if (decimalVal == 0) { maxLength = maxLength - 1; }
                minValue = sg.utls.getMinValue("9", decimalVal, maxLength, isDecimal);
                maxValue = sg.utls.getMaxValue("9", decimalVal, maxLength, isDecimal);
                formatVal = "n" + decimalVal;
            } else {
                //
                // Integer numerics
                //
                minValue = fieldInfo.MinLength;
                maxValue = fieldInfo.MaxLength;
                formatVal = "n0";
            }

            $control.kendoNumericTextBox({
                format: formatVal,
                step: 0,
                decimals: decimalVal,
                min: minValue,
                max: maxValue,
                spinners: false,
            });

        } else if (fieldType === Boolean) {

            $control.kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: this.dropdownDataSourceFromConfig(controlId),
                index: 0
            });
        }
    },

    /**
     * @function
     * @name getAttributeValueIfDefined
     * @description Get an attribute value if defined, otherwise use passed in default value
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} control The control reference
     * @param {string} attribute The attribute name
     * @param {string} defaultValue The default value
     * 
     * @returns Either the attribute value or the default value
     */
    getAttributeValueIfDefined: function (control, attribute, defaultValue) {
        var attributeValue = control.attr(attribute);
        if (attributeValue !== undefined) {
            return attributeValue;
        }
        return defaultValue;
    },

    /**
     * @function
     * @name applyFieldLengthRelatedProperties
     * @description Apply some field length related attributes to a control
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlId The Control Id string
     * @param {object} $control The jquery wrapped object reference
     * @param {object} fieldInfo The field info object
     */
    applyFieldLengthRelatedProperties: function (controlId, $control, fieldInfo) {
        const { Date, Time, Boolean } = this.OptionalFieldTypeEnum;

        const controlName = this.getNameFromId(controlId);
        const fieldType = parseInt(fieldInfo.TypeIndex);

        if (this.isNumericField(fieldType)) {

            const $kendoNumericTextBox = $control.data('kendoNumericTextBox');

            var isDecimal = this.isDecimalNumericField(fieldType);
            var decimalVal = isDecimal ? this.getAttributeValueIfDefined($control, 'decimal', fieldInfo.OptionsDecimal) : 0;
            var maxLength = isDecimal ? this.Constants.DefaultNumberLength : this.Constants.DefaultIntegerLength;
            maxLength = maxLength > this.Constants.DefaultNumberMaxLength ? this.Constants.DefaultNumberMaxLength : maxLength;

            if (isDecimal) {
                //
                // Floating point numerics
                //
                if (decimalVal == 0) {
                    maxLength = maxLength - 1;
                }

                sg.utls.kndoUI.restrictDecimals($kendoNumericTextBox, decimalVal, maxLength - decimalVal);

            } else {
                //
                // Integer numerics
                //
                const defaultLength = (controlName === this.fromTxtId) ? fieldInfo.MinLength.length : fieldInfo.MaxLength.length;
                sg.utls.kndoUI.restrictDecimals($kendoNumericTextBox, 0, defaultLength);
            }

        } else if (fieldType === Date) {

            $control.attr("disabled", false);
            $control.show();

        } else if (fieldType === Time) {

            this.setMaxLength(controlId, this.Constants.MaxTimeLength);

        } else if (fieldType === Boolean) {

            $control.removeAttr("type");

        } else {
            //
            // Text box
            //
            let defaultLength = fieldInfo.OptionsLength;
            this.setMaxLength(controlId, defaultLength);
        }
    },

    /**
     * @function
     * @name setDefaultValue
     * @description Set default values for a control
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlId The Control Id string
     * @param {object} fieldInfo The field info object
     */
    setDefaultValue: function (controlId, fieldInfo) {
        const { Date, Time, Boolean } = this.OptionalFieldTypeEnum;

        const controlName = this.getNameFromId(controlId);
        const fieldType = parseInt(fieldInfo.TypeIndex);

        let $control = $(controlId);

        if (this.isNumericField(fieldType)) {

            const $kendoControl = $control.data('kendoNumericTextBox');

            if (this.isDecimalNumericField(fieldType)) {
                //
                // Floating point numerics
                //
                var defaultLength = this.Constants.DefaultNumberLength;

                defaultLength = defaultLength > 16 ? 16 : defaultLength;
                if (fieldInfo.OptionsDecimal == 0) {
                    defaultLength = defaultLength - 1;
                }

                if (controlName == this.fromTxtId) {
                    var minValue = sg.utls.getMinValue("9", fieldInfo.OptionsDecimal, defaultLength, true);
                    $kendoControl.value(minValue);

                } else {
                    var maxValue = sg.utls.getMaxValue("9", fieldInfo.OptionsDecimal, defaultLength, true);
                    $kendoControl.value(maxValue);
                }
            } else {
                //
                // Integer numerics
                // integer start default value is hardcoded to match vb
                const value = (controlName === this.fromTxtId) ? parseInt(fieldInfo.MinLength) + 1 : fieldInfo.MaxLength;
                $kendoControl.value(value);
            }

            $kendoControl.trigger('change');

        } else if (fieldType === Date) {

            const $kendoDatePicker = $control.data('kendoDatePicker');
            const value = (controlName == this.fromTxtId) ? "" : fieldInfo.MaxLength;
            $kendoDatePicker.value(value);

        } else if (fieldType === Boolean) {

            const $kendoDropDownList = $control.data('kendoDropDownList');
            // boolean default values are hardcoded instead of using optional fields setting
            const value = (controlName === this.fromDropdownId) ? 0 : 1;
            $kendoDropDownList.value(value);

        } else if (fieldType === Time) {

            // time start default value is hardcoded instead of using optional fields setting
            const value = (controlName === this.fromTxtId) ? sg.utls.checkIfValidTimeFormat(0) : fieldInfo.MaxLength;
            $control.val(value);

        } else {
            //
            // Text field
            //
            let defaultLength = fieldInfo.OptionsLength;
            let defaultChar = fieldInfo.MaxLength;
            let defaultValue = null;
            if (defaultChar) {
                defaultValue = window.sg.utls.strPad(defaultChar, defaultLength, defaultChar);
            }
            const value = (controlName === this.fromTxtId) ? '' : defaultValue;
            $control.val(value);
        }
    },

    /**
     * @function
     * @name initChangeEvents
     * @description Initialize the control change event
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} $control The jquery wrapped object reference
     * @param {object} fieldInfo The field info object
     */
    initChangeEvents: function ($control, fieldInfo) {
        const { Date, Time } = this.OptionalFieldTypeEnum;
        const fieldType = parseInt(fieldInfo.TypeIndex);

        if (fieldType === Date) {

            $control.on('change', function (e) {
                ICOptionalFieldsUtils.checkDynamicDateValidation(fieldInfo, e.target.id);
            });

        } else if (fieldType === Time) {

            $control.on('change', function (e) {
                // If necessary, replace invalid time with a reasonable time
                const timeValue = $control.val();
                const fixedTime = sg.utls.checkIfValidTimeFormat(timeValue);
                $control.val(fixedTime);
            });

        }
    },

    /**
     * @function
     * @name setContainerClass
     * @description Set the container class based on field type
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} $control The jquery control object
     * @param {number} fieldType The field Type enum value
     */
    setContainerClass: function ($control, fieldType) {
        const { Text, Date, Time, Boolean, Number, Integer, Currency } = this.OptionalFieldTypeEnum;

        let containerClassName = this.Constants.ContainerDivClass;
        let $parent = {};

        switch (fieldType) {

            case Text:
            case Time:
                $parent = $control.parent();
                break;

            case Date:
            case Boolean:
                $parent = $control.parent().parent().parent();
                break;


            case Number:
            case Integer:
            case Currency:
                containerClassName = this.Constants.ContainerDivClassNumeric;
                $parent = $control.parent().parent().parent();
                break;
        }

        $parent.attr('class', containerClassName);
    },

    /**
     * @function
     * @name isNumericField
     * @description Determine if field is numeric based on field type
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {number} fieldType The field type value
     *
     * @returns {boolean} true = Field is a numeric type | false = Field is not a numeric type
     */
    isNumericField: function (fieldType) {
        const { Currency, Integer, Number } = this.OptionalFieldTypeEnum;
        return fieldType === Currency || fieldType === Integer || fieldType === Number;
    },

    /**
     * @function
     * @name isDecimalNumericField
     * @description Determine if field is decimal numeric based on field type
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {number} fieldType The field type value
     * 
     * @returns {boolean} true = Field is a decimal numeric type | false = Field is not a decimal numeric type
     */
    isDecimalNumericField: function (fieldType) {
        const { Currency, Number } = this.OptionalFieldTypeEnum;
        return fieldType === Currency || fieldType === Number;
    },

    /**
     * @function
     * @name dropdownDataSourceFromConfig
     * @description Create a datasource object from the json defined datasource
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlId The Control Id string
     * @returns The datasource object
     */
    dropdownDataSourceFromConfig: function (controlId) {
        let controlName = this.getNameFromId(controlId);
        let dropdownDataSourceFromConfig = DeclarativeReportViewModel.Data.Parameters.find(item => item.ID === controlName).DataSource;
        let dataSource = [];
        for (let index = 0; index < dropdownDataSourceFromConfig.length; index++) {
            let value = dropdownDataSourceFromConfig[index].Value;
            let text = dropdownDataSourceFromConfig[index].LabelText;
            dataSource.push({
                value: value,
                text: text
            });
        }
        return dataSource;
    },

    /**
     * @function
     * @name checkDynamicDateValidation
     * @description Date validation
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} fieldInfo The field info object
     * @returns {boolean} TODO - Add description
     */
    checkDynamicDateValidation: function (fieldInfo, controlName) {
        const { Date } = this.OptionalFieldTypeEnum;
        let $control = $(`#${controlName}`);
        const fieldType = parseInt(fieldInfo.TypeIndex);

        if (fieldType === Date) {
            var validDate = sg.utls.kndoUI.checkForValidDateNull($control.val(), false) != null;
            if (!validDate) {
                let errorMessage = this.txtInvalidDateString;
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
                return true;
            }
            else {
                $("#message").empty();
                return false;
            }
        }
    },

    /**
     * @function
     * @name applyControlAttributes
     * @description Apply attributes to the referenced control based on field (and fieldType)
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} $control The jquery control reference
     * @param {object} fieldInfo The field information object
     */
    applyControlAttributes: function ($control, fieldInfo) {
        const { Date, Time, Boolean } = this.OptionalFieldTypeEnum;

        const fieldType = parseInt(fieldInfo.TypeIndex);

        if (!$($control).is("[type]")) {
            $control.attr('type', 'text');
        }

        if (fieldInfo.AlphanumericField) {
            $control.attr("formattextbox", "alphaNumeric");
        }

        if (fieldInfo.UpperCaseField) {
            $control.addClass(this.Constants.ClassTextUpper);
        }

        if (fieldType === Date || fieldType === Boolean) {
            $control.addClass(this.Constants.ClassDefault);

            if (fieldType === Date) {
                $control.addClass(this.Constants.ClassDatePicker);
                $control.attr("disabled", false);
            }
        }
        else if (fieldType === Time) {
            $control.attr("disabled", false);
        }
        else if (this.isNumericField(fieldType)) {
            // Parent of parent container [span]
            $control.parent().parent().addClass(this.Constants.ClassNumeric);
            $control.parent().parent().addClass(this.Constants.ClassDefault);

            // Visible textbox
            $control.prev().addClass(this.Constants.ClassNumeric);
            $control.prev().addClass(this.Constants.ClassDefault);

            // Hidden textbox
            $control.addClass(this.Constants.ClassNumeric);
            $control.addClass(this.Constants.ClassDefault);
        }
        else {
            // textbox size depending on max input length
            this.setContainerClassLength($control, fieldInfo.OptionsLength);
        }
    },

    /**
     * @function
     * @name removeControlAttributes
     * @description Remove all attributes from a control 
     *              except ones specified
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {object} $control The jquery control reference
     */
    removeControlAttributes: function ($control) {
        const keepAttributes = ['id', 'name', 'type'];
        var attributes = $control[0].attributes;
        var count = attributes.length;
        var attributesString = "";
        for (var i = 0; i < count; i++) {
            let attribName = attributes[i].name;
            if (!keepAttributes.includes(attribName)) {
                attributesString = attributesString + " " + attribName;
            }
        }
        $control.removeAttr(attributesString);
    },

    /**
     * @function
     * @name setMaxLength
     * @description Sets the max length attribute of an input
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlId The Control ID string
     * @param {number} maxLength The maximum number of characters to allow
     */
    setMaxLength: function (controlId, maxLength) {
        $(controlId).attr('maxlength', maxLength);
    },

    /**
     * @function
     * @name setContainerClassLength
     * @description Sets the length class of an input
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} control The jquery control reference
     * @param {number} length The maximum number of characters to allow
     */
    setContainerClassLength: function (control, length) {
        if (length >= 60) {
            control.addClass(this.Constants.ClassLarge);
        }
        else if (length > 16 && length < 60) {
            control.addClass(this.Constants.ClassMedium);
        }
        else if (length < 4) {
            control.addClass(this.Constants.ClassSmaller);
        }
        else {
            control.addClass(this.Constants.ClassDefault);
        }
    },

    /**
     * @function
     * @name removeDropDown
     * @description Remove a kendoDropDownList from the DOM
     *              Note: this does not actually remove the html control
     *              It only removes the Kendo-related adornments.
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlName The control name
     */
    removeDropDown: function (controlName) {
        const kendoControl = $(`#${controlName}`).data("kendoDropDownList");
        if (kendoControl != null) {
            const input = kendoControl.element.show();
            input.removeClass("k-input");
            input.insertBefore(kendoControl.wrapper);
            kendoControl.wrapper.remove();
            input.removeData("kendoDropDownList");
        }
    },

    /**
     * @function
     * @name removeNumeric
     * @description Remove a kendoNumericTextBox from the DOM
     *              Note: this does not actually remove the html control
     *              It only removes the Kendo-related adornments.
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} controlName The control name
     */
    removeNumeric: function (controlName) {
        let controlId = $(`#${controlName}`);

        // Find the primary textbox by id
        let $control = $(controlId);

        let kendoControl = $control.data("kendoNumericTextBox");
        if (kendoControl) {
            const input = kendoControl.element.show();
            input.removeClass('k-input');
            input.insertBefore(kendoControl.wrapper);
            kendoControl.wrapper.remove();
            input.removeData('kendoNumericTextBox');
        }
    },

    /**
     * @function
     * @name getNameFromId
     * @description Trims the starting '#' from an id string
     * @namespace ICOptionalFieldsUtils
     * @public
     * 
     * @param {string} controlId A control id string
     * @returns {string} The control name (not the id)
     */
    getNameFromId: function (controlId) {
        let name = controlId.trim();
        let hashtagIndex = controlId.indexOf('#');
        if (hashtagIndex > -1 && hashtagIndex === 0) {
            name = controlId.substring('#'.length);
        }
        return name;
    },

    /**
     * @function
     * @name getIdFromName
     * @description Prepends '#' to an id string if not already exists
     * @namespace ICOptionalFieldsUtils
     * @public
     * 
     * @param {string} controlName A control name string
     * @returns {string} The control id (not the name)
     */
    getIdFromName: function (controlName) {
        let id = controlName.trim();
        let hashtagIndex = controlName.indexOf('#');
        if (hashtagIndex < 0) {
            // No hashtag found, prepend one
            id = `#${controlName}`;
        }
        return id;
    },

    /**
     * @function
     * @name getTimeHHMMSSFormat
     * @description Formats time string to HHMMSS
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} val Time string
     * @returns {string} Time string in HHMMSS
     */
    getTimeHHMMSSFormat: function (val) {
        const EMPTY_TIME = '000000';
        if (val !== null) {
            if (val !== "") {
                return $.trim(sg.utls.removeColon(val));
            } else {
                return EMPTY_TIME;
            }
        } else {
            return EMPTY_TIME;
        }
    },

    /**
     * @function
     * @name getFormattedDateOrDefault
     * @description Formats date string to YYYMMDD
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} text Date string
     * @returns {string} Date string in YYYMMDD
     */
    getFormattedDateOrDefault: function (text) {
        var defaultVal = "00000000";
        if (sg.utls.kndoUI.checkForValidDate(text)) {
            text = sg.utls.kndoUI.getDateYYYMMDDFormat(text);
            text = text !== "" ? text : defaultVal;
            return text;
        } else {
            return defaultVal;
        }
    },

    /**
     * @function
     * @name finderSuccess
     * @description Sets the value of the textbox from the optional fields finder
     * @namespace ICOptionalFieldsUtils
     * @public
     *
     * @param {string} id finder textbox id
     * @param {object} result result from finder success function
     */
    finderSuccess: function (id, result) {
        const { Text, Date, Time, Number, Integer, Currency, Boolean } = this.OptionalFieldTypeEnum;
        const fieldType = parseInt(result.TYPE);
        let isNumeric = false;
        let resultValue = '';
        switch (fieldType) {
            case Text:
            case Boolean:
                resultValue = result.VALIFTEXT;
                break;
            case Date:
                resultValue = result.VALIFDATE;
                break;
            case Time:
                resultValue = result.VALIFTIME.split('T')[1];
                break;
            case Number:
                isNumeric = true;
                resultValue = result.VALIFNUM;
                break;
            case Integer:
                isNumeric = true;
                resultValue = result.VALIFLONG;
                break;
            case Currency:
                isNumeric = true;
                resultValue = result.VALIFMONEY;
                break;
        }

        if (isNumeric) {
            $(`#${id}`).data('kendoNumericTextBox').value(resultValue);
        } else {
            $(`#${id}`).val(resultValue);
        }
    }
}
