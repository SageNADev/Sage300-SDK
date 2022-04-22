/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'OE').length > 0) ? 1 : 0;
    $("#hdnOEActive").val(isActive);
});
