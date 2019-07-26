/* Copyright (c) 2019 Sage Software, Inc.  All rights reserved. */
// @ts-check
"use strict";

(function (sg, props, $) {

    /**
     * 
     * Predefined setViewFinder() {@link SetViewFinderProperties} instances.
     * 
     * */
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

            OpenDocumentDetails: {
                viewID: "AR0200",
                viewOrder: 0,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "CONTRACT", "PROJECT", "CATEGORY", "COSTCLASS", "RESOURCE", "AMTINVCTC", "AMTDUETC"],
                filterTemplate: "IDCUST = \"{0}\" AND IDINVC = \"{1}\"  AND AMTDUETC != 0",
            },

            OpenDocumentDetailsOriginalDetail: {
                viewID: "AR0200",
                viewOrder: 0,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "CONTRACT", "PROJECT", "CATEGORY", "COSTCLASS", "RESOURCE", "BILLDATE", "IDITEM","UNITMEAS", "QTYINVC", "IDDIST", "IDGLACCT", "AMTINVCTC", "AMTDUETC", "SWDISCABL", "RTGAMTTC", "RTGOAMTTC", "RTGDATEDUE"],
                filterTemplate: "IDCUST = \"{0}\" AND IDINVC = \"{1}\""
            }
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
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT",
                                    "PICKINGSEQ", "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA",
                                    "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM"],
                //optionalFieldBindings: "IC0313,IC0377[0]"
            },

            Receipt: {
                viewID: "IC0590",
                viewOrder: 2,
                displayFieldNames: ["RECPNUMBER", "RECPDESC", "RECPDATE", "FISCYEAR", "FISCPERIOD", "PONUM", "REFERENCE", "RECPTYPE",
                    "RATEOP", "VENDNUMBER", "RECPCUR", "RECPRATE", "RATETYPE", "RATEDATE", "RATEOVRRD", "ADDCOST", "ADDCOSTHM", "ADDCOSTSRC",
                    "ADDCUR", "TOTCSTHM", "TOTCSTSRC", "NUMCSTDETL", "LABELS", "ADDCSTTYPE", "ORIGTOTSRC", "ORIGTOTHM", "ADDCSTHOME", "TOTALCOST",
                    "RECPDECIML", "VENDNAME", "VENDEXISTS", "STATUS"],
                returnFieldNames: ["RECPNUMBER", "SEQUENCENO"],
                parentValAsInitKey: true, //$("#txtReceiptNumber").val() === "*** NEW ***" ? false : true,
                filter: "DELETED = 0",
                //viewFinder.optionalFieldBindings = "IC0595, IC0377[2]";  // comment out for now as CSFND doesn't support filterCount yet
            }
        },

        PO: {

            CreditDebitNote: {
                viewID: "PO0311",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CRNNUMBER"],
                displayFieldNames: ["CRNNUMBER", "VDCODE", "VDNAME", "TRANSTYPE", "DATE", "DESCRIPTIO", "REFERENCE", "FROMDOC",
                    "RETNUMBER", "INVNUMBER", "HASJOB"],
                optionalFieldBindings: "PO0314,PO0580[14]"
            },

            Invoices: {
                viewID: "PO0420",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["INVNUMBER"],
                displayFieldNames: ["INVNUMBER", "VDCODE", "VDNAME", "DATE", "DESCRIPTIO", "REFERENCE", "HASJOB"],
            },

            ItemPOStandalone: {
                viewID: "PO0124",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "CNTLACCT", "SELLABLE"],
                //optionalFieldBindings: "PO0125,PO0119[0]"
            },

            PurchaseOrders: {
                viewID: "PO0620",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["PONUMBER"],
                displayFieldNames: ["PONUMBER", "PORTYPE", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                    "ISCOMPLETE", "HASJOB"],
            },

            Receipts: {
                viewID: "PO0700",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RCPNUMBER"],
                displayFieldNames: ["RCPNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                                    "PONUMBER", "INVNUMBER", "ISCOMPLETE", "HASJOB"],
            },

            Requisitions: {
                viewID: "PO0760",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RQNNUMBER"],
                displayFieldNames: ["RQNNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                                    "ISCOMPLETE", "REQUESTBY", "HASJOB"],
            },

            Returns: {
                viewID: "PO0731",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RETNUMBER"],
                displayFieldNames: ["RETNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                                    "RCPNUMBER", "PONUMBER", "ISCOMPLETE", "HASJOB"],
            },
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
                viewOrder: 1,
                returnFieldNames: ["FMTCONTNO"],
                displayFieldNames: ["FMTCONTNO", "DESC", "CUSTOMER", "MANAGER", "STATUS", "STARTDATE", "CURENDDATE", "CLOSEDDATE"],
                //optionalFieldBindings: "PM0850,PM0500[1]"
            },
            Project: {
                viewID: "PM0022",
                viewOrder: 2,
                displayFieldNames: ["PROJECT", "DESC", "CUSTOMER", "IDACCTSET", "CUSTCCY", "MULTICUST", "PONUMBER", "PROJSTAT",
                                    "PROJTYPE", "REVREC", "BILLTYPE", "CLOSEBILL", "CLOSECOST", "STARTDATE", "CURENDDATE", "ORJENDDATE",
                                    "CLOSEDDATE", "CODETAXGRP"],
                returnFieldNames: ["PROJECT"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" ",
                //optionalFieldBindings: "PM0851,PM0500[2]"
            },
            Category: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "TYPE", "BILLTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" ",
                //optionalFieldBindings: "PM0852,PM0500[3]"
            },
            Resource: {
                viewID: "PM0121",
                viewOrder: 3,
                displayFieldNames: ["RESOURCE", "RESDESC"],
                returnFieldNames: ["RESOURCE"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" AND CATEGORY = \"{2}\" "
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

        PR: { //Payroll
            CAEmployeeTimecard: {
                viewID: "CP0102",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "ENDDATE", "TCARDDESC", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "ENDDATE"],
                hidePageNavigation: true
            },
            CAEmployee: {
                viewID: "CP0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                hidePageNavigation: true
            },
            USEmployeeTimecard: {
                viewID: "UP0102",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "ENDDATE", "TCARDDESC", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "ENDDATE"],
                hidePageNavigation: true
            },
            USEmployee: {
                viewID: "UP0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                hidePageNavigation: true
            }
        }
    };

})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);
