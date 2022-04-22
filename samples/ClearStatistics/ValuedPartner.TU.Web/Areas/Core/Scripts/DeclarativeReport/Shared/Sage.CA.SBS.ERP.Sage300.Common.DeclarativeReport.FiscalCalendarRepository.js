/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

var fiscalCalendarRepository = {
    get: (filter, callback) => {
        const data = { filter: filter };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("CS", "FiscalCalendar", "Get"), data, callback);
    },

    isFiscalYearValid: (id, callback) => {
        const data = { id: id };
        sg.utls.ajaxPostSync(sg.utls.url.buildUrl("CS", "FiscalCalendar", "IsValid"), data, callback);
    },

    getCurrentPeriod: (result) => {
        const periods = [];
        for (let i = 1; i <= 13; i++) {
            periods.push({
                Period: i,
                StartDate: new Date(result.Data[`FiscalPeriod${i}StartDate`]),
                EndDate: new Date(result.Data[`FiscalPeriod${i}EndDate`])
            });
        }

        const today = new Date();
        const period = periods.find(x => x.StartDate <= today && x.EndDate >= today);
        return (period) ? period.Period : 1;
    }
};
