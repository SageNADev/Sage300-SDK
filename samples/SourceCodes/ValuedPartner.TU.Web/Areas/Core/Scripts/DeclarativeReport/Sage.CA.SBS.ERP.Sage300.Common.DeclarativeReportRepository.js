/* Copyright (c) 2022 The Sage Group plc or its licensors.  All rights reserved. */

// Enable the following commented line to enable TypeScript static type checking
// @ts-check

"use strict";

var declarativeReportRepository = declarativeReportRepository || {};

declarativeReportRepository = {
    /**
     * @function
    * @name execute
     * @description Print the report
     * @namespace declarativeReportRepository
     * @public
     * 
     * @param {object} viewModel The model data to post to server
     */
    execute: (viewModel) => {
        const data = declarativeReportRepository.getUnobservableData(viewModel);
        const url = sg.utls.url.buildUrl("Core", "DeclarativeReport", "Execute");
        const callback = declarativeReportOnSuccess.execute;
        sg.utls.ajaxPost(url, data, callback);
    },
   
    /**
    * @function
    * @name getUnobservableData
    * @description Gets observable data
    * @namespace declarativeReportRepository
    * @public
    * 
    * @param {object} model The model data
    */
    getUnobservableData: function (model) {
        var data = {
            report: ko.mapping.toJS(model)
        };
        return data;
    }
};