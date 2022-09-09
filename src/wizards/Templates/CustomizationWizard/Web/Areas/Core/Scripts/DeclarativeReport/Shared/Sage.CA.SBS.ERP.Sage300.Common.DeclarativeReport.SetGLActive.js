/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'GL').length > 0) ? 1 : 0;
    $("#hdnGLActive").val(isActive);
});
