/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const moduleId = 'IC';
    const fieldId = '#hdnICActive';
    const validValues = ['Y', 'N'];
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === `${moduleId}`).length > 0) ? validValues[0] : validValues[1];
    $(`${fieldId}`).val(isActive);
});