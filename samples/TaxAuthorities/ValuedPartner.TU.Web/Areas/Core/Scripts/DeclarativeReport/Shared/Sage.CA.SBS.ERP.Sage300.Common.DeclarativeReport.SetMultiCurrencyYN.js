/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";

$(function () {
    const isMultiCurrency = DeclarativeReportViewModel.IsMultiCurrency ? 'Y' : 'N';
    $("#hdnMulticurrency").val(isMultiCurrency);
});
