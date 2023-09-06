(function() {
    'use strict';

    /** Kendo optional field grid editor object */
    let kendoOptionalFieldEditorObject = {
        /**
         * custom editor template
         * @param {any} container Kendo editor container
         * @param {any} options Kendo editor options
         */
        customTemplate: function (container, options) {
            const guid = kendo.guid();
            const gridId = options.gridId || container[0].id.split('_')[0];
            const grid = $('#' + gridId).data('kendoGrid');
            const rowIndex = grid.select().index();
            const field = options.field;
            const model = options.model;
            let dataType = model.TYPE;
            const maxLength = model.LENGTH;
            const isOptField = field === 'OPTFIELD';
            const txtClass = isOptField ? 'txt-upper' : '';
            const fmtTextbox = options.formattextbox || (isOptField ? 'alphaNumeric' : '');
            const html = `<input id="${guid}" name="${field}" type="text" value="${model[field]}" class="${txtClass}" maxlength="${maxLength}" formatTextbox="${fmtTextbox}" />`;
            const numArray = ['Decimal', 'Long', 'Int', 'Integer', 'Amount', 'Number'];
            const self = this;

            options.finderTextboxId = guid;

            if (model[field] && model[field].formatList) {
                dataType = 'DropdownList'
            }

            //Optional field value has different data types, append different editors based on data type
            if (numArray.includes(dataType)) {
                let numHtml = `<input id="${guid}" name="${field}" type="number" value="${model[field]}" />`;
                this.numericEditor(container, options, dataType, numHtml, guid);
            } else if (dataType === 'DateTime' || dataType === 'Date') {
                this.dateEditor(container, options, html, guid);
            } else if (dataType === 'DropdownList') {
                this.dropdownEditor(container, options, html);
            } else if (dataType === 'Yes/No' || dataType === 'YesNo' ) {
                this.yesNoEditor(container, options, html);
            } else {
                $(html).appendTo(container, options);
            }

            let preventFinderOpenChange = false;

            //Optional field value changes and validation
            $("#" + guid).on("change", function (e) {
                if (preventFinderOpenChange) {
                    preventFinderOpenChange = false;
                    return;
                }
                const model = options.model;
                const field = options.field;
                const preValue = model[field];
                let value = isOptField ? this.value.toUpperCase() : this.value;
                let msg = options.viewId + `${field}Finder`;
                model.set(field, value);
                model.set('SWSET', globalResource.Yes);
                if (model.TYPE === 'Date') {
                    value = sg.utls.kndoUI.convertStringToDate(value).toLocaleDateString();
                }
                MessageBus.msg.trigger(msg, { nav: "user", viewId: options.viewId, msgid: model.msgid, rowIndex: model.RowIndex, field: field, value: value});
                sendMessage('SWSET', 'Yes');
                optionalFieldValidation(preValue, self.optionalFieldsFinderProperty);
            });

            //Set  optional field finders
            let finderHtml = this.showIcon(guid, options, container);
            if (finderHtml.length > 0) {
                let id = guid + 'finderIcon';
                let self = this;
                
                $("#" + id).mousedown(() => {
                    //Prevent trigger textbox changes when open finder
                    preventFinderOpenChange = true;
                    let property = options.property || self.optionalFieldsFinderProperty(field === 'VALUE', options.model.OPTFIELD, options.finderViewId, options.location);
                    let initValue = $('#' + options.finderTextboxId).val().toUpperCase().trim();
                    property.initKeyValues.push(initValue);
                    
                    sg.viewFinderHelper.setViewFinderEx(id, options.finderTextboxId, property, function (row) {
                        let model = options.model;
                        let field = options.field;
                        let value = row[Object.keys(row)[0]];
                        let desc = isOptField ? 'FDESC' : 'VDESC';

                        if (isOptField) {
                            if (checkDuplicate(value, model, 0)) {
                                return;
                            }
                        }

                        model.set(field, value);
                        sendMessage(field, value);
                        model.set(desc, row[desc]);
                        sendMessage(desc, row[desc]);
                        if (isOptField && Object.keys(row).length > 4) {
                            const type = self.getOptionalFiedTextType(row.TYPE);
                            const value = row.DEFVAL.trim();
                            model.set('VALUE', value);
                            sendMessage('VALUE', value);
                            model.set('TYPE', type);
                            sendMessage('TYPE', type);
                            ['VDESC', 'VALIDATE', 'DECIMALS'].forEach(f => {
                                model.set(f, row[f]);
                                sendMessage(f, row[f]);
                            });
                        }
                        model.set('SWSET', globalResource.Yes)
                        sendMessage('SWSET', 'Yes');
                        setCellFocus('VALUE');
                    },
                    // Cancel
                    function (e) {
                        //options.model.set(options.field, '');
                        setCellFocus();
                    });
                });
            }

            /**
             * Optional field input validation, common validation function
             * @param {any} preValue
             * @param {any} getProperty
             */
            function optionalFieldValidation(preValue, getProperty) {
                const model = options.model;
                const field = options.field;
                const isValue = field === 'VALUE';
                const validate = (isValue && ['Yes', 'True'].includes(model.VALIDATE)) || isOptField;
                const fields = ['VALUE', 'TYPE', 'VALIDATE', 'FDESC', 'SWSET', 'VDESC', 'DECIMALS'];

                if (!isValue) {
                    if (checkDuplicate(model.OPTFIELD, model, 1)) {
                        return;
                    }
                }
                
                if (validate) {
                    let property = options.property || getProperty(isValue, model.OPTFIELD, options.finderViewId, options.location);
                    let finderDef = isValue ? sg.viewFinderProperties.CS.OptionalFieldValue : property;
                    let locFilter = isOptField ? finderDef.filter.split(' and')[0] : '';
                    
                    finderDef.filter = isValue ? `OPTFIELD="${model.OPTFIELD}" and VALUE="${model.VALUE}"` : `${locFilter} and OPTFIELD="${model.OPTFIELD}"`;
                    ViewFinderGridHelper.ExecuteFinder(finderDef, (data) => {
                        if (data && data.Data.length === 0) {
                            const errMessage = isValue ? kendo.format(globalResource.OptionalFieldValueValidation, model.VALUE, model.OPTFIELD) : kendo.format(globalResource.OptionalFieldValidation, model.OPTFIELD);
                            const message = { "UserMessage": { "IsSuccess": false, "Errors": [{ "Message": errMessage }] } };
                            sg.utls.showMessage(message, setCellFocus);
                            model.set(field, preValue);
                            sendMessage(field, preValue, isValue);
                        } else if (data && data.Data.length > 0) {
                            const rowData = data.Data[0];
                            if (field === 'OPTFIELD') {
                                rowData.TYPE = self.getOptionalFiedTextType(rowData.TYPE);
                                rowData.VALIDATE = self.getValidateValue(rowData.VALIDATE);
                                rowData.VALUE = rowData.DEFVAL;
                                fields.forEach(f => model.set(f, rowData[f]));
                                //fields.forEach(f => sendMessage(f, f === 'SWSET' ? 'Yes' : rowData[f]));
                                fields.forEach(f => sendMessage(f, rowData[f]));
                            }
                            model.set('VDESC', rowData.VDESC);
                            setCellFocus();
                        }
                    });
                }
            }

            /**
             * Trigger message when field value change
             * @param {any} field
             * @param {any} value
             */
            function sendMessage(field, value, valueFinder = true) {
                const viewId = options.viewId;
                const model = options.model;
                const msg = viewId + (valueFinder ? 'VALUEFinder' : 'OPTFIELDFinder');
                MessageBus.msg.trigger(msg, { nav: "user", viewId: viewId, msgid: model.msgid, rowIndex: model.RowIndex, field: field, value: value });
            }

            /**
             * Check optional field value duplicate
             * @param {any} value
             * @param {any} model
             * @param {any} count
             */
            function checkDuplicate(value, model, count) {
                const gridData = grid.dataSource.data();

                if (gridData.filter(f => f.OPTFIELD === value).length > count) {
                    model.set(field, '');
                    const message = { "UserMessage": { "IsSuccess": false, "Errors": [{ "Message": kendo.format(globalResource.OptionalFieldExist, value) }] } };
                    sg.utls.showMessage(message, setCellFocus);
                    return true;
                }
                return false;
            }

            /**
             * Set grid editable cell focus
             * @param {any} field
             */
            function setCellFocus(field) {
                const colIndex = GridPreferencesHelper.getGridColumnIndex(grid, field || options.field);
                const cell = grid.tbody.find("tr").eq(rowIndex).find("td").eq(colIndex);
                grid.select(`tr:eq(${rowIndex})`);
                grid.current(cell);
                grid.editCell(cell);
            };
        },

        /**
         * Get formatted value
         * @param {any} type field type
         * @param {any} value value
         */
        getFormattedValue: function(type, value) {
            if (type === 'DateTime' || type === 'Date') {
                return value === '00000000' ? '' : value;
            } else if (type === 'Yes/No' || type === 'YesNo') {
                return value === '1' ? globalResource.Yes : globalResource.No;
            } else {
                return value;
            }
        },

        /**
         * Get validate value
         * @param {any} Text
         */
        getValidateValue: function (Text) {
            let result = 'False';
            switch (Text) {
                case globalResource.True:
                case globalResource.Yes:
                    result = 'True';
                    break;
                case globalResource.False:
                case globalResource.No:
                    result = 'False';
            }
            return result;
        },

        /**
         * Get optional fied text type
         * @param {any} type optional field internal type
         */
        getOptionalFiedTextType: function (type) {
            let result = 'Text';
            switch (type) {
                case '1':
                case globalResource.TypeText:
                    result = 'Text';
                    break;
                case '3':
                case globalResource.TypeDate:
                    result = 'Date';
                    break;
                case '4':
                case globalResource.TypeTime:
                    result = 'Time';
                    break;
                case '9':
                case globalResource.TypeYesNo:
                    result = 'Yes/No';
                    break;
                case '6':
                case globalResource.TypeNumber:
                    result = 'Number';
                    break;
                case '8':
                case globalResource.TypeInteger:
                    result = 'Integer';
                    break;
                case '100':
                case globalResource.TypeAmount:
                    result = 'Amount';
                    break;
                default:
            }
            return result;
        },

        /**
         * Attach html segment to show finder icon
         * @param {any} guid Textbox unique Id 
         * @param {any} options Kendo grid custom editor options
         * @param {any} container Kendo grid custom editor container
         */
        showIcon: function (guid, options, container) {
            let id = guid + 'finderIcon';
            $('<input ' + '" id="' + id + '" class="icon btn-search" tabindex="-1" type="button" />').appendTo(container);
            let html = `<input id="${id}" class="icon btn-search" tabindex="-1" type="button" />`;
            return html;
        },

        /**
         * Date field editor
         * @param {any} container Kendo grid editor container
         * @param {any} options Kendo grid custom editor options
         * @param {any} html html string
         * @param {any} textId Textbox id
         */
        dateEditor: function (container, options, html, textId) {
            const value = options.model[options.field] || '';
            const tmpValue = value.toString().replaceAll("/", "");
            
            //options.model.VALUE = value.length === 8 ? kendo.parseDate(value.toString(), 'yyyyMMdd') : value;
            options.model.VALUE = tmpValue.length === 8 ? kendo.parseDate(tmpValue.toString(), 'yyyyMMdd') : value;
            html = '<div class="edit-container"><div class="edit-cell inpt-text">' + html + '</div>';

            $(html).appendTo(container);
            sg.utls.kndoUI.datePicker(textId);
        },

        //ToDo: Refractoring this when need
        /**
         * Drop down list editor
         * @param {any} container Kendo grid editor container
         * @param {any} options Kendo grid custom editor options
         * @param {any} html html string
         */
        dropdownEditor: function (container, options, html) {
            $(html).appendTo(container)
                .kendoDropDownList({
                    dataSource: options.model[field].formatList,
                    change: function (e) {
                        if (e.sender.dataItem() == null) {
                            options.model.set("result", null);
                        } else {
                            options.model.set("result", e.sender.dataItem().CategoryID);
                        }
                    }
                });
        },

        /**
         * Yes/No drop down list editor
         * @param {any} container Kendo grid editor container
         * @param {any} options Kendo grid custom editor options
         * @param {any} html html string
         */
        yesNoEditor: function (container, options, html) {
            let value = options.model[options.field].trim();
            options.model[options.field] = value;
            $(html).appendTo(container)
                .kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: [
                        { text: globalResource.No, value: '0' },
                        { text: globalResource.Yes, value: '1' }
                    ],
                    change: function (e) {
                    }
                });
        },

        //ToDo: Refractoring this when need

        /**
         * Text box editor
         * @param {any} container Kendo grid editor container
         * @param {any} options Kendo grid custom editor options
         * @param {any} html html string
         */
        textEditor: function (container, options) {
            $('<input type="text" name="' + options.field + '"/>')
                .addClass('k-textbox')
                .appendTo(container)
                .blur(function (e) {
                    if (e.originalEvent.target.value) {
                        options.model.set("result", 1);
                    } else {
                        options.model.set("result", null);
                    }
                })
        },

        /**
         * Numeric text box editor
         * @param {any} container Kendo grid editor container
         * @param {any} options Kendo grid custom editor options
         * @param {any} dataType Numeric date type
         * @param {any} html Html string
         * @param {any} txtBoxId Text box Id
         */
        numericEditor: function (container, options, dataType, html, txtBoxId) {
            const s = kendo.culture().numberFormat['.'];
            let maxLength = 16;
            let factor = 1;
            let format = 'n3';
            let max, min;
            let precision = apputils.isUndefined(options.precision) ? options.model.DECIMALS : options.precision;

            let value = options.model[options.field];

            if (apputils.isUndefined(value) || apputils.isNull(value)) {
                return;
            }

            //convert to string to find precision
            if (value && apputils.isNumber(value)) {
                value = value + "";
            }

            if (apputils.isUndefined(options.precision) && value && value.includes(s)) {
                value = options.model[options.field].split(s);
                if (value.length > 1) {
                    precision = value[1].length;
                }
            }

            switch (dataType) {
                case "Int":
                case "Integer":
                    min = -32768;
                    max = 32767;
                    maxLength = 5;
                    precision = 0;
                    break;
                case "Long":
                    min = -2147483648;
                    max = 2147483647;
                    maxLength = 10;
                    precision = 0;
                    break;
                case "Decimal":
                case "Number":
                    maxLength = 16;
                    break;
                case "Amount":
                    maxLength = 15;
                    break;
                default:
            }

            format = `n${precision}`;
            html = '<div class="edit-container"><div class="edit-cell inpt-text">' + html + '</div>';

            $(html).appendTo(container);
            const txtNumeric = $('#' + txtBoxId).kendoNumericTextBox({
                format: format,
                spinners: false,
                min: min,
                max: max,
                decimals: precision
            });

            sg.utls.kndoUI.restrictDecimals(txtNumeric, precision, maxLength - precision);
        },

        /**
         * Get display template
         * @param {any} data
         */
        getTemplate: function(data) {
            const type = data.TYPE;
            const value = data.VALUE;
            const decimals = data.DECIMALS;
            const numArray = ['Decimal', 'Long', 'Int', 'Integer', 'Amount', 'Number'];

            if (!value && !numArray.includes(type)) {
                return "";
            } else if (type === 'Yes/No' || type === 'YesNo') {
                return value.trim() === "1" ? globalResource.Yes : globalResource.No;
            } else if (type === 'DateTime' || type === 'Date') {
                return value === "00000000" ? "" : sg.utls.kndoUI.getFormattedDate(value);
            } else if (type === 'Time' && value.indexOf(':') < 0) {
                return value.substring(0, 2) + ":" + value.substring(2, 4) + ":" + value.substring(4, 6);
            } else if (numArray.includes(type)) {
                return '<span style="float:right">' + sg.utls.kndoUI.getFormattedDecimalNumber(value || 0, decimals) + '</span>';
            } else {
                return value;
            }
        },

        /**
         * Get optional field/value finder definition, can be passed by options.property for back compatibility
         * @param {any} isValue
         * @param {any} value
         * @param {any} viewId
         * @param {any} location
         */
        optionalFieldsFinderProperty: function (isValue, value, viewId, location) {
            return  {
                viewID: isValue ? "CS0012" :viewId,
                viewOrder: 0,
                filter: isValue ? "OPTFIELD=" + value : "LOCATION = " + location,
                returnFieldNames: isValue ? ["VALUE", "VDESC"] : ["OPTFIELD", "FDESC", "SWSET", "DEFVAL", "VDESC", "TYPE", "VALIDATE", "DECIMALS"],
                displayFieldNames: isValue ? ["VALUE", "VDESC"] : ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                initKeyValues: isValue ? [value] : [location]
            }
        }
    }

    this.kendoOptionalFieldEditor = helpers.View.extend({}, kendoOptionalFieldEditorObject);

}).call(this); //make the call to bind the extended object