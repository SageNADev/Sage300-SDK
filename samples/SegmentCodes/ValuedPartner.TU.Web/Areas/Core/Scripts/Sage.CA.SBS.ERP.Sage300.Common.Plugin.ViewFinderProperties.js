/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */
// @ts-check
"use strict";

(function (sg, props, $) {

    sg.viewFinderProperties = {
        APAccountSet: {
            viewID: "AP0006",
            viewOrder: 0,
            returnFieldName: "ACCTSET",
            displayFieldNames: ["ACCTSET", "TEXTDESC", "SWACTV", "DATEINACTV", "DATELASTMN", "CURRCODE"],
        },

        ARItems: {
            viewID: "AR0010",
            viewOrder: 0,
            returnFieldName: "IDITEM",
            displayFieldNames: ["IDITEM", "TEXTDESC", "SWACTV"],
        },

        ARSalespersons: {
            viewID: "AR0018",
            viewOrder: 0,
            returnFieldName: "CODESLSP",
            displayFieldNames: ["CODESLSP", "NAMEEMPL", "SWACTV"],
        },

        ARCustomers: {
            viewID: "AR0024",
            viewOrder: 0,
            returnFieldName: "IDCUST",
            displayFieldNames: ["IDCUST", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                "TEXTSNAM", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "EMAIL2", "NAMECTAC", "CTACPHONE",
                "CTACFAX", "EMAIL1"],
        },

        ARCustomerGroups: {
            viewID: "AR0025",
            viewOrder: 0,
            returnFieldName: "IDGRP",
            displayFieldNames: ["IDGRP", "TEXTDESC", "SWACTV"],
        },

        ARNationalAccounts: {
            viewID: "AR0028",
            viewOrder: 0,
            returnFieldName: "IDNATACCT",
            displayFieldNames: ["IDNATACCT", "NAMEACCT", "IDGRP", "SWACTV", "SWHOLD", "CODECURN", "SWBALFWD"],
        },

        CSFiscalCalendars: {
            viewID: "CS0002",
            viewOrder: 0,
            returnFieldName: "FSCYEAR",
            displayFieldNames: ["FSCYEAR", "PERIODS", "QTR4PERD", "ACTIVE", "BGNDATE1", "BGNDATE2", "BGNDATE3",
                "BGNDATE4", "BGNDATE5", "BGNDATE6", "BGNDATE7", "BGNDATE8", "BGNDATE9",
                "BGNDATE10", "BGNDATE11", "BGNDATE12", "BGNDATE13",
                "ENDDATE1", "ENDDATE2", "ENDDATE3", "ENDDATE4", "ENDDATE5", "ENDDATE6",
                "ENDDATE7", "ENDDATE8", "ENDDATE9", "ENDDATE10", "ENDDATE11", "ENDDATE12", "ENDDATE13"],
        },
    };
})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);
