/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'PM').length > 0) ? 1 : 0;
    $("#hdnPMActive").val(isActive);
});
