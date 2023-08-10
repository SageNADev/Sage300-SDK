/* Copyright (c) 2022 The Sage Group plc or its licensors.  All rights reserved. */

"use strict";

$(function () {
    $("#hdnEftLicensed").val(DeclarativeReportViewModel.CheckEFTLicense ? 1 : 0);
});
