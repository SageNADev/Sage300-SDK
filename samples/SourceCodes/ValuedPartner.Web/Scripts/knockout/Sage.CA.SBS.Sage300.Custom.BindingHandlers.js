
ko.bindingHandlers.SagekendoGrid =
    {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var unwrap = ko.utils.unwrapObservable;
            var dataSource = valueAccessor();
            var binding = allBindingsAccessor();
            var preventRecursiveBinding;
            var preventKendoUpdate;
            var options = {};

            if (binding.gridOptions) {
                if (binding.columns) {
                    binding.gridOptions.columns = $.extend(true, binding.gridOptions.columns, binding.columns);
                }
                options = $.extend(options, binding.gridOptions);
            }

            var dataChange = function (e) {
                if (e.action && sg.utls.instantUpdateKO) {
                    var keyProp = binding.key;
                    var changedItem = e.items[0];
                    var changedColumn = e.field;
                    var actualItem = null;
                    if (unwrap(dataSource).length > 0) {
                        actualItem = ko.utils.arrayFirst(unwrap(dataSource), function (item) {
                            if ($.isFunction(item[keyProp])) {
                                return item[keyProp]() === changedItem[keyProp];
                            } else {
                                return item[keyProp] === changedItem[keyProp];
                            }
                        });
                    }
                    else {
                        actualItem = ko.utils.arrayFirst(this.data(), function (item) {
                            return item[keyProp] === changedItem[keyProp];
                        });
                    }

                    if (e.action == "add") {
                        var grid = $(element).data("kendoGrid");
                        var dataRows = grid.items();
                        var insertedIndex = 0;
                        insertedIndex = dataRows.index(grid.select());
                        dataSource.splice(insertedIndex + 1, 0, ko.mapping.fromJS(changedItem));
                    }
                    if (e.action == "itemchange") {
                        if (actualItem != null) {
                            if ($.isFunction(actualItem[changedColumn])) {
                                actualItem[changedColumn](changedItem[changedColumn]);
                            } else {
                                actualItem[changedColumn] = changedItem[changedColumn];
                            }
                            if (changedColumn !== "HasChanged") {
                                if ($.isFunction(actualItem["HasChanged"])) {
                                    actualItem["HasChanged"](true);
                                    //Defined in Knockoutextension.js
                                    dirtyFlags.isGridDirty(true);
                                } else {
                                    actualItem["HasChanged"] = true;
                                    //Defined in Knockoutextension.js
                                    dirtyFlags.isGridDirty(true);
                                }
                            }
                        }
                    }
                    //Callback

                    if (options.dataChange) {
                        var cellData;
                        if (actualItem != null) {
                            if ($.isFunction(actualItem[changedColumn])) {
                                cellData = actualItem[changedColumn]();
                            } else {
                                cellData = actualItem[changedColumn];
                            }
                            if (changedItem.HasChanged != undefined) {
                                if ($.isFunction(actualItem["HasChanged"])) {
                                    changedItem.HasChanged = actualItem["HasChanged"]();
                                } else {
                                    changedItem.HasChanged = actualItem["HasChanged"];
                                }
                            }
                            var changedData = {
                                columnName: changedColumn,
                                cellData: cellData,
                                rowData: changedItem
                            };
                            options.dataChange(changedData);
                        }
                    }
                    if (e.action == "remove") {
                        preventKendoUpdate = true;
                        if (!binding.hasIsCustomDelete) {
                            dataSource.remove(actualItem);
                        } else {
                            if (actualItem != null) {
                                if ($.isFunction(actualItem["IsDeleted"])) {
                                    actualItem["IsDeleted"](true);
                                    //Defined in Knockoutextension.js
                                    dirtyFlags.isGridDirty(true);
                                } else {
                                    actualItem["IsDeleted"] = true;
                                    //Defined in Knockoutextension.js
                                    dirtyFlags.isGridDirty(true);
                                }
                            }
                        }
                    }
                }
            };

            if (unwrap(dataSource) instanceof Array) {
                var mappedSource = ko.dependentObservable(function () {
                    var mapped = ko.utils.arrayMap(unwrap(dataSource), function (item) {
                        var result = {};
                        for (var prop in item) {
                            if (item.hasOwnProperty(prop)) {
                                result[prop] = unwrap(item[prop]);
                            }
                        }
                        return result;
                    });
                    return mapped;
                }, viewModel);

                //Subscribe to the knockout observable array to get new/remove items
                // The subscribe method is not used and is in place only for backward compatibility.
                mappedSource.subscribe(function (newValues) {
                    var grid = $(element).data('kendoGrid');
                    var i;
                    if (!options.isServerPaging && !preventKendoUpdate) {
                        var gridData = [];
                        if (binding.hasIsCustomDelete) {
                            for (i in newValues) {
                                if (!newValues[i].IsDeleted) {
                                    gridData.push(newValues[i]);
                                }
                            }
                        } else {
                            gridData = newValues;
                        }
                        if (!preventRecursiveBinding) {
                            grid.dataSource.data(gridData);
                            grid.refresh();
                        } else {
                            preventRecursiveBinding = false;
                        }
                    }
                    preventKendoUpdate = false;
                });
            } else
                throw 'The dataSource defined must be a javascript object array or knockout observable array!';
            var serverSideSchema = {
                data: "data",
                total: "totalResultsCount"
            };
            var schema;
            if (binding.gridOptions.schema) {
                schema = $.extend(serverSideSchema, binding.gridOptions.schema);
            } else {
                schema = serverSideSchema;
            }
            if (options.isServerPaging) {
                options.dataSource = new kendo.data.DataSource({
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: options.isServerSorting,
                    sort: options.defaultSort,
                    pageSize: options.pageSize,
                    transport: {
                        read: function (optionResult) {
                            preventRecursiveBinding = true;
                            var getParam;
                            if (typeof (binding.gridOptions.getParam) === "function") {
                                getParam = binding.gridOptions.getParam();
                            } else {
                                getParam = {
                                    pageNumber: optionResult.data.page == 0 ? optionResult.data.page : optionResult.data.page - 1,
                                    pageSize: optionResult.data.pageSize,
                                    filters: optionResult.data.filter ? optionResult.data.filter.filters ? optionResult.data.filter.filters : optionResult.data.filter : null
                                };
                            }

                            var params = binding.gridOptions.param || getParam;

                            if (params.stopPropagation != undefined && params.stopPropagation == true) {
                                optionResult.error();
                            } else {
                                var pageUrl;
                                if (binding.gridOptions.getPageUrl !== undefined) {
                                    pageUrl = binding.gridOptions.getPageUrl();
                                }
                                pageUrl = pageUrl || binding.gridOptions.pageUrl;

                                sg.utls.ajaxPost(pageUrl, params, function (successData) {
                                    binding.gridOptions.param = null;
                                    binding.gridOptions.pageUrl = options.pageUrl;
                                    var data = options.buildGridData(successData);
                                    if (data != null) {
                                        optionResult.success(data);
                                    } else {
                                        optionResult.error();
                                    }
                                });
                            }
                        }
                    },
                    schema: schema,
                    //data: mappedSource(),
                    change: dataChange
                });
            }
            else {
                options.dataSource = new kendo.data.DataSource({
                    data: mappedSource(),
                    pageSize: binding.pageSize || options.pageSize,
                    schema: binding.gridOptions.schema,
                    change: dataChange
                });
            }

            options.dataBound = function dataBound(e) {
                var grid = this;
                if (grid.dataSource.data().length === 0) {
                    //$(element).find('.k-grid-content tbody')
                    grid.table.next()
                        .css("visibility", "visible")
                        .html("<h3>" + "" + "</h3>");
                }
                else {
                    $(element).find('tbody tr:first').addClass('k-state-selected');
                }
                if (options.afterDataBind) {
                    options.afterDataBind(e);
                }
            };
            $(element).kendoGrid(options);
        }
    };

ko.bindingHandlers.Amount = {
    init: function (element, valueAccessor) {
        sg.controls.applyAmountFormat(element, valueAccessor);
    },
    update: function (element, valueAccessor) {
        sg.controls.applyAmountFormat(element, valueAccessor);
    }
};

ko.bindingHandlers.sagetext = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).text(value);
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).text(value);
    }
};

ko.bindingHandlers.sagevalue = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (element.type === "text" && $(element).hasClass('txt-upper')) {
            if (value) {
                valueAccessor()(value.toUpperCase());
            }
        }
        var result = ko.bindingHandlers.value.init.apply(this, arguments);
        if (element.type === "radio" || element.type === "checkbox") {
            sg.controls.ApplyCheckboxRadioButtonStyle(element);
        }
        return result;
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var result = ko.bindingHandlers.value.update.apply(this, arguments);

        if (element.type === "text") {
            if (value) {

                //formatTextbox is used to restrict disallowed character.
                //When we do copy-paste of disallowed values, we corect the value and change it using javascript, 
                //and in this scenario Knowkout will never get updated.
                //Below code will do the truncation of disallowed character and will update the KO correctly
                var attr = $(element).attr('formatTextbox');
                if (attr != undefined) {
                    var invalidChars;
                    if (attr === 'alphaNumeric') {
                        invalidChars = /[^0-9A-Za-z]/gi;
                    }
                    else if (attr === 'alpha') {
                        invalidChars = /[^A-Za-z]/gi;
                    }
                    else if (attr === 'numeric') {
                        invalidChars = /[^0-9]/gi;
                    }
                    var value = $(element).val();
                    if (invalidChars.test(value)) {
                        var newValue = value.replace(invalidChars, "");
                        $(element).val(newValue)
                        valueAccessor()(newValue);
                    }
                }

                if ($(element).hasClass('txt-upper')) {
                    valueAccessor()(valueAccessor()().toUpperCase());
                }
            }
        }
        if (element.type === "radio") {
            sg.controls.ApplyCheckboxRadioButtonStyle(element);
        }
        return result;
    }
};

ko.bindingHandlers.sagechecked = {
    init: function (element) {
        var result = ko.bindingHandlers.checked.init.apply(this, arguments);
        if (element.type === "radio" || element.type === "checkbox") {
            sg.controls.ApplyCheckboxRadioButtonStyle(element);
        }
        return result;
    },
    update: function (element) {
        var result = ko.bindingHandlers.checked.update.apply(this, arguments);
        if (element.type === "radio" || element.type === "checkbox") {
            sg.controls.ApplyCheckboxRadioButtonStyle(element);
        }
        // return result;
    }
};

ko.bindingHandlers.sagevisible = {
    update: function (element) {
        var result = ko.bindingHandlers.visible.update.apply(this, arguments);
        if (element.type === "radio" || element.type === "checkbox") {
            element.parentElement.style.display = element.style.display;
        }
        return result;
    }
};

ko.bindingHandlers.sagedisable = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var result = ko.bindingHandlers.disable.update.apply(this, arguments);
        var unwrap = ko.utils.unwrapObservable;
        var dataSource = valueAccessor();
        var currentModelValue = unwrap(dataSource);

        sg.controls.KendoEnableDisable(element, !currentModelValue);

        return result;
    }
};

ko.bindingHandlers.sageenable = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var result = ko.bindingHandlers.disable.update.apply(this, arguments);
        var unwrap = ko.utils.unwrapObservable;
        var dataSource = valueAccessor();
        var currentModelValue = unwrap(dataSource);

        sg.controls.KendoEnableDisable(element, currentModelValue);

        return result;
    }
};




//------------------Date Picker Knockout Custom Binding
ko.bindingHandlers.sageDatePicker = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var unwrap = ko.utils.unwrapObservable;
        var dataSource = valueAccessor();
        var binding = allBindingsAccessor();
        var options = sg.utls.kndoUI.datepickerOptions();

        $(element).attr("placeholder", sg.utls.kndoUI.getDisplayDateFormat());
        $(element).attr("formatTextbox", "date");
        $(element).attr("maxlength", "10");

        if (binding.datePickerOptions) {
            options = $.extend(options, binding.datePickerOptions);
        }

        if (dataSource) {
            var handleValueChange = function () {
                //change the knockout model object with the specified value
                var changeModel = function (value) {
                    if (ko.isWriteableObservable(dataSource)) {
                        //Since this is an observable, the update part will fire and select the 
                        //  appropriate display values in the controls
                        if ((value instanceof Date)) {
                            value = new Date(value.getFullYear(), value.getMonth(), value.getDate());
                        }
                        dataSource(value);
                    } else { //write to non-observable
                        if (binding['_ko_property_writers'] && binding['_ko_property_writers']['kendoDatePicker']) {
                            binding['_ko_property_writers']['kendoDatePicker'](value);
                        }
                    }
                };

                //Get the selected Value from the Kendo ComboBox
                var selectedValue = this.value();
                //If they dont select anything, then there intent is to null out the value
                if (!selectedValue) {
                    changeModel(null);
                } else {
                    changeModel(selectedValue);
                }
                return false;
            };
            options.change = handleValueChange;
        }

        //handle the choices being updated in a Dependant Observable (DO), so the update function doesn't 
        // have to do it each time the value is updated. Since we are passing the dataSource in DO, if it is
        // an observable, when you change the dataSource, the dependentObservable will be re-evaluated
        // and its subscribe event will fire allowing us to update the autocomplete datasource
        var mappedSource = ko.dependentObservable(function () {
            return unwrap(dataSource);
        }, viewModel);
        //Subscribe to the knockout observable array to get new/remove items
        mappedSource.subscribe(function (newValue) {
            var datePicker = $(element).data('kendoDatePicker');
            if (datePicker.value() != newValue) {
                datePicker.value(newValue);
            }
        });

        options.value = mappedSource();
        $(element).kendoDatePicker(options);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        //update value based on a model change
        var unwrap = ko.utils.unwrapObservable;
        var dataSource = valueAccessor();
        if (dataSource) {
            dataSource(sg.utls.kndoUI.convertStringToDate(dataSource()));
            var currentModelValue = unwrap(dataSource);
            if (currentModelValue != null) {
                $(element).data('kendoDatePicker').value(currentModelValue);
            }
        } else {
            $(element).data('kendoDatePicker').value('');
        }

        // Commenting the below line because, when this validate event is fired, 
        // element value has got changed and invalid attribute is getting set.
        // This shows inconsistancy in behaviour between the firefox and IE browser. 
        //$(element.form).validate().element($(element));
    }
};