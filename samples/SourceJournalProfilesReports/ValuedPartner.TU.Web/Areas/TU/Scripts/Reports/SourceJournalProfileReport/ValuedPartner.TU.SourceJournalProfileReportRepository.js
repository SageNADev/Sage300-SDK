// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// @ts-check

"use strict";

var SourceJournalProfileReportRepository = SourceJournalProfileReportRepository || {};
SourceJournalProfileReportRepository = {

    /**
     * @name executeSourceJournalProfileReportRepositoryReport
     * @description Execute an ajax post request
     * @namespace SourceJournalProfileReportRepository
     * @public
     * 
     * @param {object} viewModel The model data to post to server
     */
    executeSourceJournalProfileReportRepositoryReport: (viewModel) => {
        let data = SourceJournalProfileReportRepository.getUnobservableData(viewModel);
        let url = sg.utls.url.buildUrl("TU", "SourceJournalProfileReport", "Execute");
        let callback = sourceJournalProfileReportOnSuccess.executeSourceJournalProfileReport;
        sg.utls.ajaxPost(url, data, callback);
    },

    /**
     * @name getUnobservableData
     * @description Setup model data 
     * @namespace SourceJournalProfileReportRepository
     * @public
     *
     * @param {object} model The model data to post to server
     */
    getUnobservableData: (model) => {
        let data = {
            report: ko.mapping.toJS(model)
        };
        return data;
    }
};

