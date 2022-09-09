/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";
var declarativeReportUtls = declarativeReportUtls || {};


declarativeReportUtls = {
    /**
         * @function
         * @name resetDropdownListDataSource
         * @description rebind dropdown list data source and selected option
         * @namespace declarativeReportUI
         * @param {any} id Dropdown List Id
         * @param {any} optionsList custom options list
         * @param {any} disabled disabled
         * @public
         *
         * @param {object} result The JSON result payload
         */
    resetDropdownListDataSource: (id, optionsList, disabled, labelText) => {
        let dataSource = declarativeReportUtls.createDropdownListDataSource(optionsList, labelText);
        let ddl = $("#" + id).data("kendoDropDownList");
        ddl.setDataSource(dataSource.dataSource);
        ddl.select(dataSource.selected);
        if (typeof disabled == "boolean") {
            ddl.enable(!disabled);
        }
    },

    /**
     * @function
     * @name createDropdownListDataSource
     * @description build new dropdwon list data source
     * @namespace declarativeReportUI
     * @param {any} optionsList custom options list
     * @public
     *
     * @param {object} result The JSON result payload
     */
    createDropdownListDataSource: (optionsList, textName) => {
        var data = [];
        var selectedOption = 0;
        optionsList.forEach((item, index) => {
            data.push({
                LabelText: item["LabelText"] ?? item["Text"] ?? item[`${textName}`],
                Value: item.Value
            });
            if (item.Selected) {
                selectedOption = index;
            }
        })
        return {
            dataSource: new kendo.data.DataSource({
                data: data
            }),
            selected: selectedOption
        };
    }

}
