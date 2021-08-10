/* Copyright (c) 1994-2020 Sage Software, Inc.  All rights reserved. */

// @ts-check

"use strict";

/**
 * Functionality for formatting, enabling, disabling and selecting controls.
 */
(function (sg, $) {
    //$(this).parent().addClass("cBox-disabled");

    sg.controls = {
        /**
         * Spans a copy of an HTML element (which should be a checkbox input) with the 'checkBox' CSS class.
         * 
         * @param {string} checkboxHtml The HTML of the element to span.
         */
        ApplyCheckboxStyle: function (checkboxHtml) {
            return "<label class='checkbox-container'><span>" + checkboxHtml + "<span class='checkmark'></span></span></label>";
        },

        /**
         * Spans a copy of an HTML element with the 'radioboxHtml' CSS class.
         * 
         * @param {string} radioboxHtml The HTML of the element to span.
         */
        ApplyRadioboxStyle: function (radioboxHtml) {
            return "<label class='radio-container'><span>" + radioboxHtml + "<span class='checkmark'></span></span></label>";
        },

        /**
         * Applies CSS styles for disabled or selected radiobutton elements.
         *
         * @param {object} The HTML radiobutton element to disable. 
         */
        ApplyCheckboxRadioButtonStyle: function (element) {
            sg.controls.ApplyCheckBoxRadioButtonDisable(element);
        },

        /**
         * Applies disablement or selection CSS styles to a specified checkbox or radiobutton element.
         *
         * @param {object} element The HTML element to which to apply the styles. 
         */
        ApplyCheckBoxRadioButtonDisable: function (element) {
            sg.controls.RemoveCheckBoxRadioButtonStyle(element);
        },

        /**
         * Removes the disabled and selected styles from a specified checkbox or radiobutton element.
         *
         * @param {object} element The HTML element from which to remove the styles. 
         */
        RemoveCheckBoxRadioButtonStyle: function (element) {
        },

        /**
         * Uncheck all radio buttons and check the current selected one
         * 
         * @param {string}  currentButtonId Id of the selected radio button element
         * @param {string[]} radioIdList List of Ids of selectable radio buttons
         */
        SelectRadioButton: function (currentButtonId, radioIdList) {
            $.each(radioIdList, function(key, val) {
                if (val !== currentButtonId) {
                    $('#' + val).parent().parent().removeClass('focus');
                    $('#' + val).prop('checked', false);
                }
            });
            $('#' + currentButtonId).prop('checked', true);
            $('#' + currentButtonId).parent().parent().addClass('focus');
        },

        /**
         * Initial the select behaviour of radio buttons
         * 
         * @param {string[]} radioIdList List of Ids of selectable radio buttons
         */
        InitSelectRadioButtonBehaviour: function (radioIdList) {
            $.each(radioIdList, function(key, val) {
                $('#' + val).click(function() {
                    return sg.controls.SelectRadioButton(val, radioIdList);
                });
            });
        },

        /**
         * Applies the input focus to a specified element.
         * 
         * @param {object} element The element to set the focus to.
         */ 
        Focus: function (element) {
            element.focus();
        },

        /**
         * Applies the input focus to the list of a specified Kendo dropdown element.
         * 
         * @param {object} element The element to set the focus to.
         */
        KendoDropDownFocus: function (element) {
            var dropdownlist = element.data("kendoDropDownList");
            dropdownlist.focus();
        },

        /**
         * Select the element with the focus.
         * 
         * @param {object} element The element to select.
         */
        Select: function (element) {
            if (element.is(':focus')) {
                element.select();
            }
        },

        /**
         * Coalesce a string. 
         * 
         * @returns {string} The string, if non-null, otherwise an empty string.
         */
        GetString: function (string) {
            if (string == null) {
                return "";
            }
            return string;
        },

        /**
         * Disable a specified element. 
         * 
         * Note: the following comments are not currently relevant
         *       because the specific block of code has been disabled.
         * 
         * If incoming element is a string, prefix it with a hashtag if necessary
         * If incoming element is an object, skip hashtag prefix check/step
         *
         * @param {object} The element to disable.
         */ 
        disable: function (element) {
            // var tp = $.type(element);
            // if (tp === "string") {
            //     element = this.prefixElementWithHashtagIfNecessary(element);
            // }
            sg.controls.enableDisable(element, true);
        },

        /**
         * Enable a specified element. 
         * 
         * Note: the following comments are not currently relevant
         *       because the specific block of code has been disabled.
         * 
         * If incoming element is a string, prefix it with a hashtag if necessary
         * If incoming element is an object, skip hashtag prefix check/step
         *
         * @param {object} The element to enable.
         */
        enable: function (element) {
            // var tp = $.type(element);
            // if (tp === "string") {
            //     element = this.prefixElementWithHashtagIfNecessary(element);
            // }
            sg.controls.enableDisable(element, false);
        },

        /**
         * Note: This method is currently disabled
         * 
         * Inspect the element name and if it does not start
         * with a hashtag #, prefix the element name with one
         * and return
         *
         * @param {object} element - The element to inspect.
         */
        // prefixElementWithHashtagIfNecessary: function(element) {
        //     var elm = String(element);
        //     var hashTag = '#';
        //     var index = elm.indexOf(hashTag);
        //     return (index == 0) ? elm : hashTag + elm;
        // },

        /**
         * Adds or removes the 'disabled' attribute from a specified element. 
         *
         * @param {object} element The element to update.
         * @param {boolean} flag True to apply the disabled attribute, otherwise false.
         */
        enableDisable: function (element, flag) {
            if (flag) {
                $(element).attr("disabled", "disabled");
            }
            else {
                $(element).removeAttr("disabled");
            }
        },

        /**
         * Enables or disables a specified radiobutton, checkbox, or Kendo element. 
         *
         * @param {object} element The element to enable.
         * @param {boolean} currentModelValue True to enable the element, false to disable it.
         */
        KendoEnableDisable: function (element, currentModelValue) {
            if (!element.type) {return;} // Labels don't contain an element.type attribute.
			
            var $elm = $(element);
            var type = element.type.toLowerCase();
            var className = element.className;

            switch (type) {
                case "select-one":
                    // If this is a single-select Kendo dropdown, enable it.
                    var dropDown = $elm.data('kendoDropDownList');
                    if (dropDown) {
                        dropDown.enable(currentModelValue);
                    }
                    break;
                     
                case "text":
                    if (className.indexOf("datepicker") > -1) {
                        var datePicker = $elm.data('kendoDatePicker');
                        if (datePicker) {
                            datePicker.enable(currentModelValue);
                        }

                    } else if (className.indexOf("kendonumeric") > -1) {
                        var numericTextBox = $elm.data('kendoNumericTextBox');
                        if (numericTextBox) {
                            numericTextBox.enable(currentModelValue);
                        }

                    } else if (className.indexOf("numeric") > -1) {
                        // Is this a kendoNumericTextBox?
                        var numericTextBox = $elm.data('kendoNumericTextBox');
                        if (numericTextBox) {
                            numericTextBox.enable(currentModelValue);
                        } else {
                            // Is this a regular text box?
                            sg.controls.enableDisable(element, !currentModelValue);
                        }
                    }
                    break;
                case "checkbox":
                    if (element.parentElement && element.parentElement.parentElement && element.parentElement.parentElement.nextElementSibling &&
                        element.parentElement.parentElement.nextElementSibling.nodeName &&
                        element.parentElement.parentElement.nextElementSibling.nodeName.toLowerCase() === "label") {
                        let label = element.parentElement.parentElement.nextElementSibling;
                        if (currentModelValue) {
                            $(label).removeClass("disabled");
                        } else {
                            $(label).addClass("disabled");
                        }
                    }
                    break;
            }
        },

        /**
         * Sets a specified element's text to a specified numeric value, formatted to the home currency decimals if applicable.
         *
         * @param {object} element The element for which to set the value.
         * @param {number} value The value with which to set the element's text.
         */
        applyAmountFormat: function(element, value) {
            var value = ko.utils.unwrapObservable(value());
            if (sg.utls.homeCurrency != null) {
                value = parseFloat(value).toFixed(sg.utls.homeCurrency.Decimals);
            }
            var text = kendo.toString(parseFloat(value), "n");
            if ($(element)[0].nodeName == "LABEL") 
                $(element).text(text);
            else
                $(element).val(text);
        }
    };
}(this.sg = this.sg || {}, jQuery));

$(function () {
    jQuery.fn.extend({
        applyCheckboxStyle: function () {
            return this.each(function () {
                if ($(this).is(':checked')) {
                    $(this).prop('checked', true);
                } else {
                    $(this).prop('checked', false);
                }
            });
        }
    });

    $(document).on('change', 'input[type="checkbox"]', function () {
        sg.controls.ApplyCheckboxRadioButtonStyle(this);
    });

    $(document).on('change', 'input[type="radio"]', function () {
        sg.controls.ApplyCheckboxRadioButtonStyle(this);
    });

    $(document).on('focus', 'input', function () {
        var $this = $(this)
            .one('mouseup.mouseupSelect', function () {
                $this.select();
                return false;
            })
            .one('mousedown', function () {
                $this.off('mouseup.mouseupSelect');
            })
            .select();
    });

    $(document).on("focus", 'input[type="checkbox"], input[type="radio"]', function () {
        $(this).parent().parent().addClass("focus");
    });

    $(document).on("blur", 'input[type="checkbox"], input[type="radio"]', function () {
        $(this).parent().parent().removeClass("focus");
    });
});

