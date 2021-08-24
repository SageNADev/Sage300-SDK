/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */
"use strict";

sg.multiSelectionHelper = {
    
    //id - the id of the multiselect
    //model - object describing the structure of the data returned from the server. NOTE: must include the id of your keyfield -- use getModel function
    //template - function or string describing how to display the data in the multiselect widget
    //getDataUrl - controller url for returning the data to the multiselect
    //field - field name in table used by the valuemapper function
    //table - table name used by the valuemapper function
    //currentValue - an empty array to prevent duplicate values in the widget

    init: function (id, model, template, getDataUrl, field, table, currentValue) {
        $("#" + id).kendoMultiSelect({
            autoBind: false,
            dataSource: new kendo.data.DataSource({
                filter: { "Field": model.id, "Value": "", "getdataurl": getDataUrl },
                pageSize: 28, //must be equal to (height/itemHeight) * 4
                schema: 
                {
                    model:  model,
                    data: 'data',
                    total: 'total',
                },
                serverPaging: true,
                serverFiltering: true,             
                transport: {
                    read: function (e) {
                        // The line below is to prevent value mapper to be triggered more than once.
                        currentValue = [];
                        sg.multiSelectionHelper.getData(e);
                    },                
                },           
            }),            
            dataTextField: model.id,
            dataValueField: model.id,
            minLength: 0,            
            select: function (e) {
                // "No Results Found" string from resource file.
                var noResultsFound = globalResource.NoResultsFound;

                if (noResultsFound == e.item.text()) {
                    e.preventDefault();
                }
            },
            itemTemplate: template,
            virtual: {
                itemHeight: 30,
                valueMapper: function (e) {
                    sg.multiSelectionHelper.getRowNumbers(this.dataTextField, field, table, id, currentValue, e); 
                }
            },
            height: 210,
        });
    },

    getData: function (e) {           
        var data = new Object();
        data.pageSize = e.data.pageSize;
        data.pageNumber = e.data.page;
        data.filterOptions = e.data.filter.filters[0]; // typing in Portal > Add Report Link > UI Profile MultiSelect box

        var dataurl;
        if (e.data.filter.filters[1]) {
            dataurl = e.data.filter.filters[1].getdataurl;
        } else {
            dataurl = e.data.filter.filters[0].getdataurl;
        }

        sg.utls.ajaxPost(dataurl,
                         data, 
                         function (successData) {
                             e.success({ data: successData.Items, total: successData.TotalResultsCount });
                         });
    },

    getRowNumbers: function (id, field, table, controlName, currentValue, e) {
        var data = new Object();
        data.ID = e.value;      
        data.indexColumn = field;
        data.table = table;

        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "MultiSelection", "GetRowNumbers"), data, function (successData) {
            var data = [];
            for (var i = 0; i < successData.Items.length; i++) {
                data.push(successData.Items[i].RowNumber-1);
            }

            // This block is to prevent value mapper to be triggered multiple times, resulting in duplicate values in the widget.
            var isTheSame = (data.length == currentValue.length) && currentValue.every(function (element, index) {
                return element === data[index];
            });

            if (isTheSame)
                return;
            else {
		//Do not need to clean up from data source, not sure the reason to clean up
                //$("#" + controlName).data("kendoMultiSelect").value([]);
                currentValue = data;
            }
            // End of block.

            e.success(data);
        });
    },

    getModel: function (keyfield, fields) {
        //keyfield - Field that is the key for your multiselect data. Used as the value and display text in the multiselect. 
        //fields - Fields that need to be returned in data from controller and used in template. Must include ID field. Use format {field : "name", type: "datatype"}
        var model = {
            id: keyfield,
            fields: {}
        };
        $.each(fields, function (index, value) {
            model[value.field] = { type: value.type };
        });
        return model;
    }
}
