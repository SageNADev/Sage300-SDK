/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'NA').length > 0) ? 1 : 0;
    $("#hdnNAActive").val(isActive);
});
