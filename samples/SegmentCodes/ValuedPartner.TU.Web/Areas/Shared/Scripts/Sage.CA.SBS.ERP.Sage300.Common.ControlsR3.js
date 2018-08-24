/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

"use strict";
(function () {
    $(this).parent().addClass("cBox-disabled");
    sg.controls = {
        ApplyCheckboxStyle: function (checkboxHtml) {
            return "<span class='icon checkBox'>" + checkboxHtml + "</span>";
        },
        ApplyRadioboxStyle: function (radioboxHtml) {
            return "<span class='icon radioBox'>" + radioboxHtml + "</span>";
        },
        ApplyCheckboxRadioButtonStyle: function (element) {
            sg.controls.ApplyCheckBoxRadioButtonDisable(element);
        },
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
        RemoveCheckBoxRadioButtonStyle: function (element) {
            if (element.type === "checkbox") {
                $(element).parent().removeClass("cBox-disabled selected checked-disabled");
            }
            else if (element.type === "radio") {
                $(element).parent().removeClass("rBox-disabled selected checked-disabled");
            }
        },
        Focus: function (element) {
            element.focus();
        },
        KendoDropDownFocus: function (element) {
            var dropdownlist = element.data("kendoDropDownList");
            dropdownlist.focus();
        },
        Select: function (element) {
            if (element.is(':focus')) {
                element.select();
            }
        },
        GetString: function (string) {
            if (string == null) {
                return "";
            }
            return string;
        },
        disable: function (element) {
            sg.controls.enableDisable(element, true);
        },
        enable: function (element) {
            sg.controls.enableDisable(element, false);
        },
        enableDisable: function (element, flag) {
            if (flag) {
                $(element).attr("disabled", "disabled");
            }
            else {
                $(element).removeAttr("disabled");
            }
        },
        KendoEnableDisable: function (element, currentModelValue) {
            if (element.type === "radio" || element.type === "checkbox") {
                sg.controls.ApplyCheckBoxRadioButtonDisable(element);
            } else if (element.type === "text" && element.className.indexOf("datepicker") > -1) {
                var datePicker = $(element).data('kendoDatePicker');
                if (datePicker) {
                    datePicker.enable(currentModelValue);
                }
            } else if (element.type === "select-one") {
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
        applyAmountFormat: function(element, value) {
            var value = ko.utils.unwrapObservable(value());
            if (sg.utls.homeCurrency != null)
                value = parseFloat(value).toFixed(sg.utls.homeCurrency.Decimals);
            if ($(element)[0].nodeName == "LABEL")
                $(element).text(value);
            else
                $(element).val(value);
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

