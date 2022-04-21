/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const CanadaPayrollId = 'CP';
    const USPayrollId = 'UP';
    const fieldId = '#hdnPayrollActive';
    const validValues = ['Y', 'N'];
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.IsInstalled === true).filter(e => e.AppId === `${CanadaPayrollId}` || e.AppId === `${USPayrollId}`).length > 0) ? validValues[0] : validValues[1];

    console.log(DeclarativeReportViewModel);
    console.log(isActive);
    $(`${fieldId}`).val(isActive);
});
