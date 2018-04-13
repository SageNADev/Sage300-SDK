/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */

"use strict";

/**
 * Functionality for formatting, enabling, disabling and selecting controls.
 */
(function () {
    $(this).parent().addClass("cBox-disabled");

    sg.controls = {
        /**
         * Spans a copy of an HTML element (which should be a checkbox input) with the 'checkBox' CSS class.
         * 
         * @param {string} checkboxHtml The HTML of the element to span.
         */
        ApplyCheckboxStyle: function (checkboxHtml) {
            return "<span class='icon checkBox'>" + checkboxHtml + "</span>";
        },
        /**
         * Spans a copy of an HTML element with the 'radioboxHtml' CSS class.
         * 
         * @param {string} radioboxHtml The HTML of the element to span.
         */
        ApplyRadioboxStyle: function (radioboxHtml) {
            return "<span class='icon radioBox'>" + radioboxHtml + "</span>";
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
            if (element.disabled) {
                if (element.checked) {
                    $(element).parent().addClass("checked-disabled");
                } else {
                    if (element.type === "checkbox") {
                        $(element).parent().addClass("cBox-disabled");
                    }
                    else if (element.type === "radio") {
                        $(element).parent().addClass("rBox-disabled");
                    }
                }
            } else {
                if (element.checked) {
                    $(element).parent().addClass("selected");
                }
            }
        },
        /**
         * Removes the disabled and selected styles from a specified checkbox or radiobutton element.
         *
         * @param {object} element The HTML element from which to remove the styles. 
         */
        RemoveCheckBoxRadioButtonStyle: function (element) {
            if (element.type === "checkbox") {
                $(element).parent().removeClass("cBox-disabled selected checked-disabled");
            }
            else if (element.type === "radio") {
                $(element).parent().removeClass("rBox-disabled selected checked-disabled");
            }
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
                    $('#' + val).parent().removeClass('focus selected');
                    $('#' + val).prop('checked', false);
                }
            });
            $('#' + currentButtonId).prop('checked', true);
            $('#' + currentButtonId).parent().addClass('focus selected');
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
         * @param {object} The element to disable.
         */ 
        disable: function (element) {
            sg.controls.enableDisable(element, true);
        },
        /**
         * Enable a specified element. 
         *
         * @param {object} The element to enable.
         */
        enable: function (element) {
            sg.controls.enableDisable(element, false);
        },
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
            if (element.type === "radio" || element.type === "checkbox") {
                sg.controls.ApplyCheckBoxRadioButtonDisable(element);
            } else if (element.type === "text" && element.className.indexOf("datepicker") > -1) {
                var datePicker = $(element).data('kendoDatePicker');
                if (datePicker) {
                    datePicker.enable(currentModelValue);
                }
            } else if (element.type === "select-one") {
                // If this is a single-select Kendo dropdown, enable it.
                var dropDown = $(element).data('kendoDropDownList');
                if (dropDown) {
                    dropDown.enable(currentModelValue);
                }
            } else if (element.type === "text" && element.className.indexOf("kendonumeric") > -1) {
                var numericTextBox = $(element).data('kendoNumericTextBox');
                if (numericTextBox) {
                    numericTextBox.enable(currentModelValue);
                }
            }
            else if (element.type === "text" && element.className.indexOf("numeric") > -1) {
                sg.controls.enableDisable(element, !currentModelValue);
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
                    $(this).parent().addClass("selected");
                } else {
                    $(this).parent().removeClass("selected");
                }
            });
        }
    });
    $(document).on('change', 'input[type="checkbox"]', function () {
        sg.controls.ApplyCheckboxRadioButtonStyle(this);
    });
    $(document).on('mouseenter', 'input[type="checkbox"]', function () {
        $(this).parent().addClass("cBox-hover");
    });
    $(document).on('mouseleave', 'input[type="checkbox"]', function () {
        $(this).parent().removeClass("cBox-hover");
    });
    $(document).on('change', 'input[type="radio"]', function () {
        sg.controls.ApplyCheckboxRadioButtonStyle(this);
    });
    $(document).on('mouseenter', 'input[type="radio"]', function () {
        $(this).parent().addClass("radioBox-hover");
    });
    $(document).on('mouseleave', 'input[type="radio"]', function () {
        $(this).parent().removeClass("radioBox-hover");
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
        $(this).parent().addClass("focus");
    });

    $(document).on("blur", 'input[type="checkbox"], input[type="radio"]', function () {
        $(this).parent().removeClass("focus");
    });
});

