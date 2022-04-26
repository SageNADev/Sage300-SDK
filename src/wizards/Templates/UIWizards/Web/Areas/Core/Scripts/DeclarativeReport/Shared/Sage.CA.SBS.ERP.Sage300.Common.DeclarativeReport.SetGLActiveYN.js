/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isActive = (DeclarativeReportViewModel.ActiveApplications.filter(e => e.AppId === 'GL').length > 0) ? 'Y' : 'N';
    $("#hdnGLActive").val(isActive);
});