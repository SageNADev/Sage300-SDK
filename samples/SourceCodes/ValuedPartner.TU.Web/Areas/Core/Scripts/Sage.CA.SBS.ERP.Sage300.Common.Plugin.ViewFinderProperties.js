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
				parentValAsInitKey: true, 
                returnFieldNames: ["ACCTSET"],
                displayFieldNames: ["ACCTSET", "TEXTDESC", "SWACTV", "DATEINACTV", "DATELASTMN", "CURRCODE"],
            },
			
            Vendor: {
                viewID: "AP0015",
                viewOrder: 0,
                parentValAsInitKey: false, 
                returnFieldNames: ["VENDORID"],
                displayFieldNames: ["VENDORID", "VENDNAME", "SWACTV", "IDGRP", "CURNCODE", "SHORTNAME", 
                                    "SWHOLD", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", 
                                    "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2"],
            },
        },

        AR: {
            Customers: {
                viewID: "AR0024",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["IDCUST"],
                displayFieldNames: ["IDCUST", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSNAM", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "EMAIL2", "NAMECTAC", "CTACPHONE",
                    "CTACFAX", "EMAIL1"],
            },

            CustomerGroups: {
                viewID: "AR0025",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["IDGRP"],
                displayFieldNames: ["IDGRP", "TEXTDESC", "SWACTV"],
            },

            Items: {
                viewID: "AR0010",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["IDITEM"],
                displayFieldNames: ["IDITEM", "TEXTDESC", "SWACTV"],
            },

            Salespersons: {
                viewID: "AR0018",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["CODESLSP"],
                displayFieldNames: ["CODESLSP", "NAMEEMPL", "SWACTV"],
            },

            NationalAccounts: {
                viewID: "AR0028",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["IDNATACCT"],
                displayFieldNames: ["IDNATACCT", "NAMEACCT", "IDGRP", "SWACTV", "SWHOLD", "CODECURN", "SWBALFWD"],
            },
        }, 

        AS: {
            // Add new AS finders here
        },

        CS: {
            CurrencyCodes: {
                viewID: "CS0003",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["CURID", "CURNAME"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL"],
            },

            FiscalCalendars: {
                viewID: "CS0002",
                viewOrder: 0,
				parentValAsInitKey: true,
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
				parentValAsInitKey: false, // TODO - Need to ensure that this works correctly.
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC"],
                displayFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID",
                                    "ALLOCSW", "MCSW", "QTYSW", "UOM"],
            },

            SourceCodes: {
                viewID: "GL0002",
                viewOrder: 0,
				parentValAsInitKey: false, // TODO - Need to ensure that this works correctly.
                returnFieldNames: ["SRCELEDGER", "SRCETYPE"],
                displayFieldNames: ["SRCELEDGER", "SRCETYPE", "SRCEDESC"],
            },

            SourceJournalProfiles: {
                viewID: "GL0019",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["SRCEJRNL"],
                displayFieldNames: ["SRCEJRNL"],
            },
        },

        IC: {
            PriceListCodes: {
                viewID: "IC0390",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["PRICELIST"],
                displayFieldNames: ["PRICELIST", "DESC"],
            },

            Item: {
                viewID: "IC0310",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT", "PICKINGSEQ",
                    "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA", "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM"],
                //optionalFieldBindings: "IC0313,IC0377[0]"
            }
        },

        PO: {
            ItemPOStandalone: {
                viewID: "PO0124",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "CNTLACCT", "SELLABLE"],
                //optionalFieldBindings: "PO0125,PO0119[0]"
            }
        },

        TX: {
            TaxAuthorities: {
                viewID: "TX0002",
                viewOrder: 0,
				parentValAsInitKey: true,
                returnFieldNames: ["AUTHORITY"],
                displayFieldNames: ["AUTHORITY", "DESC", "SCURN", "MAXTAX", "MINTAX", "TXBASE",
                    "INCLUDABLE", "LIABILITY", "AUDITLEVEL", "RECOVERABL", "RATERECOV",
                    "ACCTRECOV", "EXPSEPARTE", "ACCTEXP", "LASTMAINT"],
            },
        },

        PM: { // aka PJC
            Contract: {
                viewID: "PM0021",
                viewOrder: 3,
                returnFieldNames: ["FMTCONTNO"],
                displayFieldNames: ["FMTCONTNO", "DESC", "CUSTOMER", "MANAGER", "STATUS", "STARTDATE", "CURENDDATE", "CLOSEDDATE"],
                //optionalFieldBindings: "PM0850,PM0500[1]"
            },
            Project: {
                viewID: "PM0022",
                viewOrder: 2,
                displayFieldNames: ["PROJECT", "DESC", "CUSTOMER", "IDACCTSET", "CUSTCCY", "MULTICUST", "PONUMBER", "PROJSTAT", "PROJTYPE", "REVREC", "BILLTYPE", "CLOSEBILL", "CLOSECOST", "STARTDATE", "CURENDDATE", "ORJENDDATE", "CLOSEDDATE", "CODETAXGRP"],
                returnFieldNames: ["PROJECT"],
                /*extra*/
                filterTemplate: "CONTRACT LIKE \"{0}%\" ",
                //optionalFieldBindings: "PM0851,PM0500[2]"
            },
            Category: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "TYPE", "BILLTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY"],
                /*extra*/
                filterTemplate: "CONTRACT LIKE \"{0}%\" AND PROJECT LIKE \"{1}%\" ",
                //optionalFieldBindings: "PM0852,PM0500[3]"
            },
            Resource: {
                viewID: "PM0121",
                viewOrder: 3,
                displayFieldNames: ["RESOURCE", "RESDESC"],
                returnFieldNames: ["RESOURCE"],
                /*extra*/
                filterTemplate: "CONTRACT LIKE \"{0}%\" AND PROJECT LIKE \"{1}%\" AND CATEGORY LIKE \"{2}%\" "
            },
            Labor: {
                viewID: "PM0002",
                viewOrder: 0,
                displayFieldNames: ["STAFFCODE", "NAME", "INACTIVE", "EARNCODE", "GROUP"],
                returnFieldNames: ["STAFFCODE"]
            },
            Equipment: {
                viewID: "PM0025",
                viewOrder: 0,
                displayFieldNames: ["EQUIPMENT", "DESC", "INACTIVE"],
                returnFieldNames: ["EQUIPMENT"]
            },
            Subcontractor: {
                viewID: "PM0026",
                viewOrder: 0,
                displayFieldNames: ["SUBCONT", "NAME", "DESC", "INACTIVE", "VENDORID"],
                returnFieldNames: ["SUBCONT"]
            },
            Overhead: {
                viewID: "PM0029",
                viewOrder: 0,
                displayFieldNames: ["OHCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["OHCODE"]
            },
            Miscellaneous: {
                viewID: "PM0028",
                viewOrder: 0,
                displayFieldNames: ["MISCCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["MISCCODE"]
            },
        },
    };

})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);
