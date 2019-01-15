/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */
// @ts-check
"use strict";

(function (sg, props, $) {

    sg.viewFinderProperties = {

		/*
			Developer Note:
			
			- Add new entries as necessary
		*/
	
        AP: {
            AccountSet: {
                viewID: "AP0006",
                viewOrder: 0,
                returnFieldNames: ["ACCTSET"],
                displayFieldNames: ["ACCTSET", "TEXTDESC", "SWACTV", "DATEINACTV", "DATELASTMN", "CURRCODE"],
            },
        },

        AR: {
            Customers: {
                viewID: "AR0024",
                viewOrder: 0,
                returnFieldNames: ["IDCUST"],
                displayFieldNames: ["IDCUST", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSNAM", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "EMAIL2", "NAMECTAC", "CTACPHONE",
                    "CTACFAX", "EMAIL1"],
            },

            CustomerGroups: {
                viewID: "AR0025",
                viewOrder: 0,
                returnFieldNames: ["IDGRP"],
                displayFieldNames: ["IDGRP", "TEXTDESC", "SWACTV"],
            },

            Items: {
                viewID: "AR0010",
                viewOrder: 0,
                returnFieldNames: ["IDITEM"],
                displayFieldNames: ["IDITEM", "TEXTDESC", "SWACTV"],
            },

            Salespersons: {
                viewID: "AR0018",
                viewOrder: 0,
                returnFieldNames: ["CODESLSP"],
                displayFieldNames: ["CODESLSP", "NAMEEMPL", "SWACTV"],
            },

            NationalAccounts: {
                viewID: "AR0028",
                viewOrder: 0,
                returnFieldNames: ["IDNATACCT"],
                returnFieldDisplayMask: "{0}",
                displayFieldNames: ["IDNATACCT", "NAMEACCT", "IDGRP", "SWACTV", "SWHOLD", "CODECURN", "SWBALFWD"],
            },
        }, 

        AS: {
            // Add new IC finders here
        },

        CS: {
            CurrencyCodes: {
                viewID: "CS0003",
                viewOrder: 0,
                returnFieldNames: ["CURID", "CURNAME"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL"],
            },

            FiscalCalendars: {
                viewID: "CS0002",
                viewOrder: 0,
                returnFieldNames: ["FSCYEAR"],
                displayFieldNames: ["FSCYEAR", "PERIODS", "QTR4PERD", "ACTIVE", "BGNDATE1", "BGNDATE2", "BGNDATE3",
                    "BGNDATE4", "BGNDATE5", "BGNDATE6", "BGNDATE7", "BGNDATE8", "BGNDATE9",
                    "BGNDATE10", "BGNDATE11", "BGNDATE12", "BGNDATE13",
                    "ENDDATE1", "ENDDATE2", "ENDDATE3", "ENDDATE4", "ENDDATE5", "ENDDATE6",
                    "ENDDATE7", "ENDDATE8", "ENDDATE9", "ENDDATE10", "ENDDATE11", "ENDDATE12", "ENDDATE13"],
            },
        },

        GL: {
            Accounts: {
                viewID: "GL0001",
                viewOrder: 0,
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC"],
                displayFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID",
                                    "ALLOCSW", "MCSW", "QTYSW", "UOM"],
            },

            SourceCodes: {
                viewID: "GL0002",
                viewOrder: 0,
                returnFieldNames: ["SRCELEDGER", "SRCETYPE"],
                displayFieldNames: ["SRCELEDGER", "SRCETYPE", "SRCEDESC"],
            },

            SourceJournalProfiles: {
                viewID: "GL0019",
                viewOrder: 0,
                returnFieldNames: ["SRCEJRNL"],
                displayFieldNames: ["SRCEJRNL"],
            },
        },

        IC: {
            // Add new IC finders here
        },

        PO: {
            // Add new PO finders here
        },

        TX: {
            TaxAuthorities: {
                viewID: "TX0002",
                viewOrder: 0,
                returnFieldNames: ["AUTHORITY"],
                displayFieldNames: ["AUTHORITY", "DESC", "SCURN", "MAXTAX", "MINTAX", "TXBASE",
                    "INCLUDABLE", "LIABILITY", "AUDITLEVEL", "RECOVERABL", "RATERECOV",
                    "ACCTRECOV", "EXPSEPARTE", "ACCTEXP", "LASTMAINT"],
            },
        },
    };

})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);
