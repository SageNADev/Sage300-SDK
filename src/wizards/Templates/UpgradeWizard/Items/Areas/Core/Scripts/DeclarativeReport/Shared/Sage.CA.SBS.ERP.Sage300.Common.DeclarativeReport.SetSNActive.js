/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const moduleId = 'SN';
    const fieldId = '#hdnSWSNLTLIC';
    const validValues = ['1', '0'];
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === `${moduleId}`).length > 0) ? validValues[0] : validValues[1];
    $(`${fieldId}`).val(isActive);
});