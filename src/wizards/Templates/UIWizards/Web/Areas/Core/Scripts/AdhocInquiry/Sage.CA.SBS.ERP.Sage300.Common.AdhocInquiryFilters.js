// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.

var adhocInquiryFilters = adhocInquiryFilters || {};

adhocInquiryFilters = {
    filterOperator: { Equal: 1, Between: -1, },
    fieldType: { Finder: 0, MultiSelect: 1, Date: 2, Enum:3, Text: 4 },
    initRenderedAttributes: function (fieldIndex, filterDisplayIndex, defaultFilterName, filterItem) {
        var control = InquiryFieldControlList[fieldIndex];

        for (var j = 0; j < control.renderedObjList.length; j++) {
            var renderedObj = control.renderedObjList[j];
            var id = jQuery.validator.format(renderedObj.Id, filterDisplayIndex);

            switch (renderedObj.objectType) {
                case "dropdown":
                    adhocInquiryFilters.initDropDownList(id, filterDisplayIndex, control, filterItem);
                    break;

                case "fromtextbox":
                    adhocInquiryFilters.initTextBox(id, filterItem, control);
                    break;

                //TODO: After the template is defined.
                //case "totextbox":

                case "fromdatepicker":
                    adhocInquiryFilters.initDatePicker(id, filterItem, control, filterDisplayIndex);
                    break;

                //case "todatepicker":

                //case "fromfinder":
                //    var textBox = $.grep(control.renderedObjList, function (obj) {
                //        return obj.objectType == "fromtextbox";
                //    })[0];
                //    adhocInquiryFilters.initFinder(renderedObj.Id,
                //        control.inquiryFilterControl.Title,
                //        control.inquiryFilterControl.FinderName,
                //        control.inquiryFilterControl.Field,
                //        textBox.Id);
                //    break;

                //case "tofinder":
                //    var textBox = $.grep(control.renderedObjList, function (obj) {
                //        return obj.objectType == "totextbox";
                //    })[0];
                //    adhocInquiryFilters.initFinder(renderedObj.Id,
                //        control.inquiryFilterControl.Title,
                //        control.inquiryFilterControl.FinderName,
                //        control.inquiryFilterControl.Field,
                //        textBox.Id);
                //    break;

                case "multiselect":
                    adhocInquiryFilters.initMultiSelect(id, filterDisplayIndex, filterItem);
                    break;
            }
        }
    },

    initTextBox: function (id, filterItem, control) {
        $("#" + id).attr("data-bind", "value: Value");
        var isFinderType = control.inquiryField.Type === adhocInquiryFilters.fieldType.Finder;
        if (isFinderType) {
            $("#" + id).bind("change", function () {
                filterItem.Value(this.value.toUpperCase());
            });
        }
        ko.applyBindings(filterItem, $("#" + id)[0]);
    },

    initDropDownList: function (id, filterDisplayIndex, control, filterItem) {
        sg.utls.kndoUI.dropDownList(id);
        var dropdown = $("#" + id).data("kendoDropDownList");

        var isFinderType = control.inquiryField.Type === adhocInquiryFilters.fieldType.Finder;
        var isDateType = control.inquiryField.Type === adhocInquiryFilters.fieldType.Date;
        var isEnumType = control.inquiryField.Type === adhocInquiryFilters.fieldType.Enum;

        if (isFinderType || isDateType) {
            dropdown.bind("change",
                function (e) {
                    var controlRenderedObjList = control.renderedObjList;
                    var toGroup = $.grep(controlRenderedObjList, function (renderedObj) {
                        return renderedObj.objectType === (isFinderType ? "finderGroup" : "datePickerGroup");
                    })[0];
                    var toGroupId = jQuery.validator.format(toGroup.Id, filterDisplayIndex);

                    var fromBox = $.grep(controlRenderedObjList, function (renderedObj) {
                        return renderedObj.objectType === (isFinderType ? "fromtextbox" : "fromdatepicker");
                    })[0];
                    var fromBoxId = jQuery.validator.format(fromBox.Id, filterDisplayIndex);

                    var toBox = $.grep(controlRenderedObjList, function (renderedObj) {
                        return renderedObj.objectType === (isFinderType ? "totextbox" : "todatepicker");
                    })[0];
                    var toBoxId = jQuery.validator.format(toBox.Id, filterDisplayIndex);

                    if (this.value() == adhocInquiryFilters.filterOperator.Between) {
                        $("#" + toGroupId).show();
                        isFinderType ? $("#" + fromBoxId).attr('placeholder', 'First') : $.noop();
                        isFinderType ? $("#" + toBoxId).attr('placeholder', 'Last') : $.noop();
                    } else {
                        $("#" + toGroupId).hide();
                        isFinderType ? $("#" + fromBoxId).removeAttr('placeholder') : $.noop();
                        isFinderType ? $("#" + toBoxId).removeAttr('placeholder') : $.noop();
                    }
            });
        }

        if (isEnumType) { //In case of enum type the dropdown becomes the Value and SqlOperator is always EQ
            $("#" + id).attr("data-bind", "value: Value");
            ko.applyBindings(filterItem, $("#" + id)[0]);
            dropdown.value(filterItem.Value()); //Reason to do this is because even if the value is binded to the Value, it doesn't update the UI (issue between ko and kendo)
            filterItem.SqlOperator(adhocInquiryFilters.filterOperator.Equal);
        }
        else {
            $("#" + id).attr("data-bind", "value: SqlOperator");
            ko.applyBindings(filterItem, $("#" + id)[0]);
            dropdown.value(filterItem.SqlOperator());
        }
    },

    initDatePicker: function (id, filterItem, control, filterDisplayIndex) {
        sg.utls.kndoUI.datePicker(id);
        var datePicker = $("#" + id).data("kendoDatePicker");

        if (filterItem && filterItem.Value() !== null) {
            var date = new Date(filterItem.Value());
            datePicker.value(date);
        }

        var changeFunc = function () {
            var value = $("#" + id).val();
            if (value) {
                var controlRenderedObjList = control.renderedObjList;
                var fromDatePickerSpan = $.grep(controlRenderedObjList, function (renderedObj) {
                    return renderedObj.objectType === "fromdatepicker_span";
                })[0];
                var fromDatePickerSpanId = jQuery.validator.format(fromDatePickerSpan.Id, filterDisplayIndex);

                var formattedDate = sg.utls.kndoUI.getDateYYYMMDDFormat(value);
                if (formattedDate && formattedDate !== "") {
                    filterItem.Value(formattedDate);
                    $("#" + fromDatePickerSpanId).hide();
                } else {
                    $("#" + fromDatePickerSpanId).show();
                }
            }
        };

        $("#" + id).bind("change", changeFunc);
    },

    initFinder: function (id, finderTitle, finderName, fieldName, textBoxId) {
        //TODO: This code is intially implemented the same way we did in inquiry screens, but recomvmendations are not to use eval()
        // hence need to rethink on how to evaluate it once we work on this part.

        //var title = $.validator.format(AdhocInquiryResources.FinderTitle, finderTitle);
        //var onSuccess = function (result) {
        //    if (result != null) {
        //        $("#" + textBoxId).val(eval("result." + fieldName));
        //    }
        //};
        //var finderFilter = function () {
        //    var filters = [[]];
        //    var filter = $("#" + textBoxId).val().toUpperCase();
        //    filters[0][0] = sg.filterHelper.createFilter(fieldName, sg.finderOperator.StartsWith, filter);
        //    return filters;
        //};

        //sg.finderHelper.setFinder(id, eval("sg.finder." + finderName), onSuccess, $.noop, title, finderFilter);
    },

    initMultiSelect: function (id, filterDisplayIndex, filterItem) {
        $("#" + id).kendoMultiSelect({
            autoClose: false,
            select: selectWithSelectAllLogics,
            change: onValueChange
        }).data("kendoMultiSelect");

        var ms = $("#" + id).data("kendoMultiSelect");
      
        if (filterItem && filterItem.Value() !== null) {
            var val = filterItem.Value().split(",");
            var ds = ms.dataSource.data();
            if (val.length === ds.length) {  //All are selected 
                val = addItemForAll();
            } 
            ms.value(val);
        } else {
            var value = addItemForAll();
            ms.value(value);
        }
        ms.trigger("change");

        function addItemForAll() {
            ms.dataSource.data().unshift({ text: AdhocInquiryResources.All, value: "-1" });
            return ms.dataSource.data()[0].value;
        }

        function contains(value, values) {
            for (var index = 0; index < values.length; index++) {
                if (values[index] === value) {
                    return true;
                }
            }
            return false;
        }

        function onValueChange(e) {
            var selectAllValue = "-1";
            var values = this.value();
            if (values.length === 1 && values[0] === selectAllValue) {
                values = $.map(ms.dataSource.data(), function(x) {
                    if (x.value !== selectAllValue)
                        return x.value;
                });
            }
            filterItem.Value(values.join(","));
        }

        function selectWithSelectAllLogics(e) {
            var selectAllValue = "-1";
            var dataItemValue = this.dataSource.view()[e.item.index()].value;
            var values = this.value();

            //If an selection already exists, simply return and the widget will unselect this selection
            if (dataItemValue !== selectAllValue && contains(dataItemValue, values)) {
                return;
            }

            //Clear selections when 'All' is selected and remove 'All' when any other selection is selected
            if (dataItemValue === selectAllValue) {
                values = [];
            } else if (values.indexOf(selectAllValue) !== -1) {
                values = $.grep(values, function (value) {
                    return value !== selectAllValue;
                });
            }

            values.push(dataItemValue);
            this.value(values);
            this.trigger("change");

            //Prevent Kendo default select event
            e.preventDefault();
        }
    },

    validateFilter: function (successHandler) {
        var filters = adhocInquiryUI.inquiryModel.Data.InquiryFilters();
        var emptyItem = sg.utls.ko.arrayFirstItemOf(filters, function (item) {
            return item.BaseFieldName() && (item.Value() === null || item.Value() === "");
        });
        if (emptyItem === -1) {
            successHandler();
        } else {
            var field = sg.utls.ko.arrayFirstItemOf(adhocInquiryUI.inquiryFieldControlList, function (obj) {
                return obj.inquiryField.Name === emptyItem.BaseFieldName();
            });
            sg.utls.showValidationMessage(jQuery.validator.format(AdhocInquiryResources.Required, field.inquiryField.DisplayName));
        }
    },
};
