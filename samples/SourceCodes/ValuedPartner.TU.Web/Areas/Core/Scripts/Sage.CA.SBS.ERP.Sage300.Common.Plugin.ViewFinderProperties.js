/* Copyright (c) 2019-2023 The Sage Group plc or its licensors.  All rights reserved. */
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
            Add new entries as necessary
        */
    
        AP: {
            DistributionCodes: {
                viewID: "AP0005",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["DISTID", "TEXTDESC", "SWACTV", "IDGLACCT", "SWDISCABL"],
                displayFieldNames: ["DISTID", "TEXTDESC", "SWACTV", "IDGLACCT"]
            },

            AccountSet: {
                viewID: "AP0006",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["ACCTSET", "TEXTDESC", "SWACTV", "CURRCODE"],
                displayFieldNames: ["ACCTSET", "TEXTDESC", "SWACTV", "DATEINACTV", "DATELASTMN", "CURRCODE"],
                filterTemplate: "CURRCODE = \"{0}\""
            },

            _1099CPRS: {
                viewID: "AP0007",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CLASSID", "CLASSDESC", "SWACTV"],
                displayFieldNames: ["CLASSID", "CLASSDESC", "SWACTV"]
            },

            DistributionSet: {
                viewID: "AP0009",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["DISTSET", "TEXTDESC", "SWACTV"],
                displayFieldNames: ["DISTSET", "TEXTDESC", "SWACTV", "CODEMETH"]
            },

            PaymentCodes: {
                viewID: "AP0010",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PAYMCODE", "PAYMTYPE", "ACTVSW", "TEXTDESC"],
                displayFieldNames: ["PAYMCODE", "ACTVSW", "TEXTDESC", "PAYMTYPE"]
            },

            Terms: {
                viewID: "AP0012",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["TERMSCODE", "CODEDESC", "SWACTV"],
                displayFieldNames: ["TERMSCODE", "CODEDESC", "SWACTV", "SWMULTPAYM"]
            },

            APVendor: {
                url: ["AP", "VendorViewFinder", "Find"],
                viewID: "AP0015",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["VENDORID", "VENDNAME", "SWACTV", "CURNCODE"],
                displayFieldNames: ["VENDORID", "SHORTNAME", "VENDNAME", "SWACTV", "SWHOLD", "IDGRP", "CURNCODE", "TEXTSTRE1", "TEXTSTRE2",
                                    "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2",
                                    "EMAIL2", "NAMECTAC", "CTACPHONE", "CTACFAX", "EMAIL1"]
            },

            Vendor: {
                url: ["AP", "VendorViewFinder", "Find"],
                viewID: "AP0015",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VENDORID", "VENDNAME", "SHORTNAME", "RATETYPE", "CURNCODE", "NAMECTAC"],
                displayFieldNames: ["VENDORID", "VENDNAME", "SWACTV", "IDGRP", "CURNCODE", "SHORTNAME",
                                    "SWHOLD", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4",
                                    "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2"]
            },

            ShortName: {
                url: ["AP", "VendorViewFinder", "Find"],
                viewID: "AP0015",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["SHORTNAME"],
                displayFieldNames: ["SHORTNAME", "VENDNAME", "VENDORID", "SWACTV", "IDGRP", "CURNCODE", "NAMECTAC"]
            },

            VendorGroups: {
                viewID: "AP0016",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["GROUPID", "ACTIVESW"],
                displayFieldNames: ["GROUPID", "DESCRIPTN", "ACTIVESW", "CURNCODE", "ACCTSETID", "BANKID", "TERMCODE", "TAXGRP"]
            },

            RemitToLocations: {
                viewID: "AP0018",
                viewOrder: 0,
                returnFieldNames: ["IDVENDRMIT", "RMITNAME", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "IDVEND"],
                displayFieldNames: ["IDVENDRMIT", "RMITNAME", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                                    "CODEPSTL", "CODECTRY", "SWACTV", "TEXTPHON1", "TEXTPHON2", "EMAIL", "NAMECTAC", "CTACPHONE", "CTACFAX",
                                    "CTACEMAIL"],
                filterTemplate: "IDVEND = \"{0}\""
            },

            InvoiceBatches: {
                viewID: "AP0020",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BTCHDESC", "DATEBTCH", "BTCHTYPE", "BTCHSTTS", "SRCEAPPL", "CNTINVCENT", "AMTENTR",
                                    "SWPRINTED", "POSTSEQNBR"],
                filter: "(BTCHSTTS = 1) OR (BTCHSTTS = 7)"
            },

            Invoices: {
                viewID: "AP0021",
                viewOrder: 0,
                returnFieldNames: ["CNTITEM"],
                displayFieldNames: ["CNTITEM", "IDVEND", "IDINVC", "INVCDESC", "AMTGROSDST"],
                filterTemplate: "CNTBTCH = {0}"
            },

            Document: {
                viewID: "AP0025",
                viewOrder: 0,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDPONBR", "DATEINVCDU", "IDTRXTYPE", "TXTTRXTYPE", "DESCINVC",
                                    "DATEINVC", "CODETERM", "DATEDISC", "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAY"],
                filterTemplate: "(IDVEND = \"{0}\") AND (TXTTRXTYPE <= 4)"
            },

            ApplyToDocument: {
                viewID: "AP0025",
                viewOrder: 0,
                returnFieldNames: ["IDINVC", "SWJOB", "IDPONBR", "IDORDERNBR"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDPONBR", "DATEINVCDU", "TXTTRXTYPE", "AMTDUEHC",
                                    "AMTDUETC"],
                filterTemplate: "(IDVEND = \"{0}\") AND (TXTTRXTYPE = 1) AND (SWPAID = 0)"
            },

            OriginalDocument: {
                viewID: "AP0025",
                viewOrder: 0,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDPONBR", "DATEINVCDU", "IDTRXTYPE", "TXTTRXTYPE", "DESCINVC",
                                    "DATEINVC", "RTGAMTTC", "CODETERM", "DATEDISC", "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAY"],
                filterTemplate: "(IDVEND = \"{0}\") AND (TXTTRXTYPE = \"{1}\") AND (RTGAPPLYTO = \"\") AND (SWRTGOUT = 1)"
            },

            DocumentNumber: {
                viewID: "AP0026",
                viewOrder: 0,
                returnFieldNames: ["IDINVC", "CNTPAYM", "TXTTRXTYPE"],
                displayFieldNames: ["IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE", "SWPAID", "AMTDUETC", "AMTPYMRMTC"],
                filterTemplate: "(IDVEND = \"{0}\") AND (TXTTRXTYPE!=11)"
            },

            StartingDocumentNumber: {
                viewID: "AP0026",
                viewOrder: 0,
                returnFieldNames: ["IDINVC", "SWJOB", "CNTPAYM"],
                displayFieldNames: ["IDINVC", "CNTPAYM", "IDRMIT", "DATEDUE", "DATEDISC", "AMTDUETC", "AMTDSCRMTC", "AMTPYMRMTC", "TXTTRXTYPE", "DATEINVC"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingPONumber: {
                viewID: "AP0026",
                viewOrder: 3,
                returnFieldNames: ["IDPONBR", "SWJOB"],
                displayFieldNames: ["IDPONBR", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingDueDate: {
                viewID: "AP0026",
                viewOrder: 4,
                returnFieldNames: ["DATEDUE", "SWJOB"],
                displayFieldNames: ["DATEDUE", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingOrderNumber: {
                viewID: "AP0026",
                viewOrder: 2,
                returnFieldNames: ["IDORDRNBR", "SWJOB"],
                displayFieldNames: ["IDORDRNBR", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingDocumentDate: {
                viewID: "AP0026",
                viewOrder: 7,
                returnFieldNames: ["DATEINVC", "SWJOB"],
                displayFieldNames: ["DATEINVC", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingCurrentBalance: {
                viewID: "AP0026",
                viewOrder: 5,
                returnFieldNames: ["AMTPYMRMTC", "SWJOB"],
                displayFieldNames: ["AMTPYMRMTC", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingOriginalDocument: {
                viewID: "AP0026",
                viewOrder: 10,
                returnFieldNames: ["RTGAPPLYTO", "SWJOB"],
                displayFieldNames: ["RTGAPPLYTO", "IDINVC", "CNTPAYM", "IDRMIT", "TXTTRXTYPE"],
                filterTemplate: "(IDVEND = \"{0}\") AND ((TXTTRXTYPE = 1) OR (TXTTRXTYPE = 2) OR (TXTTRXTYPE = 3) OR (TXTTRXTYPE = 10) OR (TXTTRXTYPE = 4)) AND (SWPAID = 0)"
            },

            StartingCheckNumber: {
                url: ["AP", "PostedPaymentsViewFinder", "Find"],
                viewID: "AP0029",
                viewOrder: 2,
                returnFieldNames: ["IDRMIT"],
                displayFieldNames: ["IDBANK", "IDRMIT", "DATERMIT", "CNTFISCYR", "CNTFISCPER", "IDVEND", "TEXTPAYOR", "CODECURN",
                                    "AMTPAYM", "IDRATETYPE", "DATERATE", "RATEEXCHHC", "AMTRMITTC", "AMTRMITHC", "TRXTYPETXT",
                                    "IDINVC", "SWCHKCLRD", "DATECLRD", "DATERVRSD", "TEXTRETRN", "CNTBTCH", "CNTITEM", "DATEBATCH"],
                filterTemplate: "IDVEND = \"{0}\""
            },

            BankCodeCheckNumber: {
                url: ["AP", "PostedPaymentsViewFinder", "Find"],
                viewID: "AP0029",
                viewOrder: 3,
                returnFieldNames: ["IDRMIT"],
                displayFieldNames: ["IDBANK", "IDRMIT", "DATERMIT", "CNTFISCYR", "CNTFISCPER", "IDVEND", "TEXTPAYOR", "CODECURN",
                    "AMTPAYM", "IDRATETYPE", "DATERATE", "RATEEXCHHC", "AMTRMITTC", "AMTRMITHC", "TRXTYPETXT",
                    "IDINVC", "SWCHKCLRD", "DATECLRD", "DATERVRSD", "TEXTRETRN", "CNTBTCH", "CNTITEM", "DATEBATCH"],
                filterTemplate: "((IDBANK >=\"{0}\") AND (IDBANK <= \"{1}\") AND (IDVEND >=\"{2}\") AND (IDVEND <= \"{3}\") AND (DATERMIT >= {4}) AND (DATERMIT <= {5}) AND (((CNTFISCYR = \"{6}\") AND (CNTFISCPER >= \"{7}\")) OR (CNTFISCYR > \"{6}\")) AND (((CNTFISCYR = \"{8}\") AND (CNTFISCPER <= \"{9}\")) OR (CNTFISCYR < \"{8}\")))"
            },

            AdjustmentBatches: {
                viewID: "AP0030",
                viewOrder: 0,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BATCHDESC", "DATEBTCH", "BATCHTYPE", "BATCHSTAT", "SRCEAPPL", "CNTENTER", "AMTENTER",
                                    "SWPRINTED", "POSTSEQNBR"],
                filter: "PAYMTYPE = \"AD\" AND ((BATCHSTAT = 1) OR (BATCHSTAT = 7) OR (BATCHSTAT = 8))"
            },

            PaymentBatches: {
                url: ["AP", "PaymentBatchesViewFinder", "Find"],
                viewID: "AP0030",
                viewOrder: 0,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BATCHDESC", "DATEBTCH", "BATCHTYPE", "BATCHSTAT", "SRCEAPPL", "CNTENTER", "AMTENTER",
                                    "IDBANK", "CODECURN", "DATERATE", "RATEEXCHHC", "SWPRINTED", "POSTSEQNBR", "FUNCAMOUNT"],
                filter: "PAYMTYPE = \"PY\" AND (BATCHSTAT = 1 OR BATCHSTAT = 7 OR BATCHSTAT = 8)"
            },

            Adjustments: {
                viewID: "AP0031",
                viewOrder: 0,
                returnFieldNames: ["CNTENTR"],
                displayFieldNames: ["CNTENTR", "IDVEND", "TEXTRMIT", "AMTADJHCUR"],
                filterTemplate: "BTCHTYPE=\"AD\" AND CNTBTCH = {0}"
            },

            Payments: {
                viewID: "AP0031",
                viewOrder: 0,
                returnFieldNames: ["CNTENTR", "CNTBTCH"],
                displayFieldNames: ["CNTENTR", "IDRMIT", "IDVEND", "AMTRMIT"],
                filterTemplate: "(BTCHTYPE = \"PY\" AND CNTBTCH = {0} AND RMITTYPE != 5)"
            },

            SelectionCriteriaHeader: {
                viewID: "AP0035",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDSELECT"],
                displayFieldNames: ["IDSELECT", "SWACTV", "DATELASMNT", "SWDOCSPROC", "SWSELLBY"],
                filter: "IDSELECT != \"SYSTEM\""
            },

            RecurringPayable: {
                viewID: "AP0064",
                viewOrder: 1,
                returnFieldNames: ["IDRECURR", "IDVEND"],
                displayFieldNames: ["IDRECURR", "IDVEND", "DESC", "SCHEDKEY", "SWACTV"]
            },

            StartingDocuments: {
                viewID: "AP0110",
                viewOrder: 0,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDPONBR", "DATEINVCDU", "IDTRXTYPE", "TXTTRXTYPE", "AMTREMIT",
                                    "DESCINVC", "DATEINVC", "CODETERM", "DATEDISC", "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAY"],
                filterTemplate: "IDVEND = \"{0}\""
            },

            StartingDocumentsOrderNumber: {
                viewID: "AP0110",
                viewOrder: 1,
                returnFieldNames: ["IDORDERNBR"],
                displayFieldNames: ["IDORDERNBR", "IDINVC", "IDPONBR", "TXTTRXTYPE", "IDRMIT", "AMTINVCHC", "AMTDUEHC"],
                filterTemplate: "IDVEND = \"{0}\""
            },

            StartingDocumentsPONumber: {
                viewID: "AP0110",
                viewOrder: 2,
                returnFieldNames: ["IDPONBR"],
                displayFieldNames: ["IDPONBR", "IDINVC", "IDORDERNBR", "TXTTRXTYPE", "IDRMIT", "AMTINVCHC", "AMTDUEHC"],
                filterTemplate: "IDVEND = \"{0}\""
            },

            EmailMessages: {
                viewID: "AP0120",
                viewOrder: 0,
                returnFieldNames: ["MSGID", "TEXTDESC", "DTELSTMNTN", "ACTIVESW", "DATEINAC", "SUBJECT", "BODY"],
                displayFieldNames: ["MSGID", "TEXTDESC", "SUBJECT", "BODY"]
            },

            OpenDocumentDetails: {
                viewID: "AP0200",
                viewOrder: 0,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "IDDIST", "IDGLACCT", "AMTINVCTC", "AMTDUETC", "SWDISCABL"],
                filterTemplate: "(IDVEND = \"{0}\") AND (IDINVC = \"{1}\")"
            },

            RetainatageDocumentDetails: {
                viewID: "AP0200",
                viewOrder: 0,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "IDDIST", "IDGLACCT", "RTGAMTTC", "RTGOAMTTC", "RTGDATEDUE"],
                filterTemplate: "(IDVEND = \"{0}\") AND (IDINVC = \"{1}\") AND (RTGAMTTC != 0)"
            },

            OptionalFields: {
                viewID: "AP0500",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            },

            PostingJournals: {
                viewID: "AP0512",
                viewOrder: 0,
                returnFieldNames: ["POSTSEQNCE"],
                displayFieldNames: ["POSTSEQNCE", "DATEPOSTED", "DATEBUS", "SWPRINTED", "SWPOSTGL", "DATEPOSTGL"],
                filterTemplate: "TYPEBTCH = \"{0}\""
            }
        },

        AR: {
            DunningMessages: {
                viewID: "AR0008",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODESTMT", "ACTIVESW"],
                displayFieldNames: ["CODESTMT", "TEXTDESC", "ACTIVESW"]
            },

            ItemPricing: {
                viewID: "AR0009",
                viewOrder: 0,
                returnFieldNames: ["UNITMEAS"],
                displayFieldNames: ["IDITEM", "CODECURN", "UNITMEAS", "AMTPRICE", "AMTBASETAX"],
                filterTemplate: "IDITEM = \"{0}\" AND CODECURN = \"{1}\""
            },

            ItemPricingGrid: {
                viewID: "AR0009",
                viewOrder: 0,
                returnFieldNames: ["UNITMEAS"],
                displayFieldNames: ["CODECURN", "UNITMEAS", "AMTPRICE", "AMTBASETAX"],
                filter: "IDITEM=ARITEM AND CODECURN=BILLCCY",
                initKeyFieldNames: ["IDITEM", "CODECURN"]
            },

            ItemUOMEquipmentCode: {
                viewID: "AR0009",
                viewOrder: 1,
                returnFieldNames: ["UNITMEAS"],
                displayFieldNames: ["UNITMEAS", "CODECURN", "AMTCOST", "AMTPRICE"],
            },

            Items: {
                viewID: "AR0010",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDITEM", "TEXTDESC"],
                displayFieldNames: ["IDITEM", "TEXTDESC", "SWACTV"]
            },

            PaymentCodes: {
                viewID: "AR0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PAYMCODE", "PAYMTYPE", "TEXTDESC"],
                displayFieldNames: ["PAYMCODE", "ACTVSW", "TEXTDESC", "PAYMTYPE"]
            },

            AccountSet: {
                viewID: "AR0013",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDACCTSET", "TEXTDESC"],
                displayFieldNames: ["IDACCTSET", "TEXTDESC", "ACTVSW", "CURNCODE"],
                filterTemplate: "CURNCODE = \"{0}\""
            },

            BillingCycle: {
                viewID: "AR0014",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDCYCL", "TEXTDESC", "ACTVSW"],
                displayFieldNames: ["IDCYCL", "TEXTDESC", "ACTVSW"]
            },

            DistributionCode: {
                viewID: "AR0015",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDDIST", "SWACTV"],
                displayFieldNames: ["IDDIST", "TEXTDESC", "SWACTV", "IDACCTREV", "IDACCTINV", "IDACCTCOGS"],
            },

            TermsCodes: {
                viewID: "AR0016",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODETERM", "TEXTDESC"],
                displayFieldNames: ["CODETERM", "TEXTDESC", "ACTIVESW", "MULTIPAYM"],
            },

            SalesPersons: {
                viewID: "AR0018",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODESLSP", "NAMEEMPL"],
                displayFieldNames: ["CODESLSP", "NAMEEMPL", "SWACTV"]
            },

            InterestProfiles: {
                viewID: "AR0020",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODESVCCHR", "TEXTDESC", "ACTVSW"],
                displayFieldNames: ["CODESVCCHR", "TEXTDESC", "ACTVSW"]
            },

            ShipToLocation: {
                viewID: "AR0023",
                viewOrder: 0,
                returnFieldNames: ["IDCUSTSHPT"],
                displayFieldNames: ["IDCUSTSHPT", "IDCUST", "NAMELOCN", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "SWACTV", "TEXTPHON1", "TEXTPHON2", "EMAIL", "NAMECTAC", "CTACPHONE", "CTACFAX", "CTACEMAIL"],
                filterTemplate: "IDCUST = \"{0}\""
            },

            ShipToLocationForCustomerNumber: {
                viewID: "AR0023",
                viewOrder: 0,
                returnFieldNames: ["IDCUST"],
                displayFieldNames: ["IDCUST", "IDCUSTSHPT", "SWACTV", "NAMELOCN"]
            },

            Customers: {
                url: ["AR", "CustomerViewFinder", "Find"],
                viewID: "AR0024",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDCUST", "NAMECUST", "NAMECTAC", "PRICLIST", "CODECURN"],
                displayFieldNames: ["IDCUST", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSNAM", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "EMAIL2", "NAMECTAC", "CTACPHONE",
                    "CTACFAX", "EMAIL1"]
            },

            CustomersReport: {
                url: ["AR", "CustomerViewFinder", "Find"],
                viewID: "AR0024",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDCUST", "NAMECUST", "NAMECTAC", "PRICLIST", "CODECURN"],
                displayFieldNames: ["IDCUST", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSNAM", "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2"]
            },

            ShortenedCustomers: {
                url: ["AR", "CustomerViewFinder", "Find"],
                viewID: "AR0024",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDCUST", "NAMECUST", "CODECURN", "SWHOLD"],
                displayFieldNames: ["IDCUST", "TEXTSNAM", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "NAMECTAC", "PRICLIST"]
            },

            ShortNameCustomers: {
                url: ["AR", "CustomerViewFinder", "Find"],
                viewID: "AR0024",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["TEXTSNAM"],
                displayFieldNames: ["TEXTSNAM", "IDCUST", "NAMECUST", "IDGRP", "IDNATACCT", "CODECURN", "NAMECTAC"]
            },

            CustomerGroups: {
                viewID: "AR0025",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDGRP", "TEXTDESC"],
                displayFieldNames: ["IDGRP", "TEXTDESC", "SWACTV"]
            },

            NationalAccounts: {
                viewID: "AR0028",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDNATACCT"],
                displayFieldNames: ["IDNATACCT", "NAMEACCT", "IDGRP", "SWACTV", "SWHOLD", "CODECURN", "SWBALFWD"],
            },

            InvoiceBatches: {
                viewID: "AR0031",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BTCHDESC", "DATEBTCH", "BTCHTYPE", "BTCHSTTS", "SRCEAPPL", "INVCTYPE", "CNTINVCENT", "AMTENTR"],
                filterTemplate: "(BTCHSTTS = \"1\" AND CNTBTCH >= \"{0}\") OR (BTCHSTTS = \"7\" AND CNTBTCH >= \"{0}\")"
            },

            InvoiceBatchesForInvoiceEntry: {
                viewID: "AR0031",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BTCHDESC", "DATEBTCH", "BTCHTYPE", "BTCHSTTS", "SRCEAPPL", "INVCTYPE", "CNTINVCENT", "AMTENTR"],
                filter: "BTCHSTTS = \"1\" OR BTCHSTTS = \"7\""
            },

            Invoices: {
                viewID: "AR0032",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTITEM"],
                displayFieldNames: ["CNTITEM", "IDCUST", "IDINVC", "DATEINVC", "INVCDESC", "AMTDUE"],
                filterTemplate: "CNTBTCH = \"{0}\""
            },

            InvoicesForDocumentNumber: {
                viewID: "AR0032",
                viewOrder: 2,
                parentValAsInitKey: true,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDCUST", "TEXTTRX", "CNTBTCH", "CNTITEM", "DATEINVC", "SWPRTINVC"],
                filter: "TEXTTRX = \"1\""
            },

            Documents: {
                viewID: "AR0036",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: [
                    "IDINVC", "IDRMIT", "IDORDERNBR", "IDCUSTPO", "DATEDUE", "TRXTYPEID", "TRXTYPETXT", "DESCINVC",
                    "DATEINVC",
                    "CODETERM", "AMTDUEHC", "AMTDISCHC", "SWPAID", "IDPREPAID"
                ],
                filterTemplate:
                    "IDCUST = \"{0}\" AND ((TRXTYPETXT <= \"2\")  OR (TRXTYPETXT = \"3\" AND AMTDUETC != 0) OR (TRXTYPETXT = \"4\"))"
            },

            Document: {
                viewID: "AR0036",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["IDINVC", "TRXTYPETXT"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDCUSTPO", "DATEDUE",
                    "TRXTYPEID", "TRXTYPETXT", "DESCINVC", "DATEINVC", "CODETERM", "DATEDISC",
                    "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAID"],
                filterTemplate: "(IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND (TRXTYPETXT = \"{2}\" OR TRXTYPETXT = \"{3}\" OR TRXTYPETXT = \"{4}\")) OR (IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND TRXTYPETXT = \"{5}\" AND AMTDUETC < \"{6}\")"
            },

            Document_ApplyByDocument: {
                viewID: "AR0036",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDRMIT", "IDORDERNBR", "IDCUSTPO", "DATEDUE",
                    "TRXTYPEID", "TRXTYPETXT", "DATEINVC", "DATEDISC",
                    "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAID"],
                filterTemplate: "IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND TRXTYPETXT = \"{2}\""
            },

            Document_ApplyByOrder: {
                viewID: "AR0036",
                viewOrder: 2,
                parentValAsInitKey: false,
                returnFieldNames: ["IDORDERNBR"],
                displayFieldNames: ["IDORDERNBR", "IDINVC", "IDRMIT", "IDCUSTPO", "DATEDUE",
                    "TRXTYPEID", "TRXTYPETXT", "DATEINVC", "DATEDISC",
                    "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAID"],
                filterTemplate: "IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND TRXTYPETXT = \"{2}\""
            },

            Document_ApplyByPONumber: {
                viewID: "AR0036",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["IDCUSTPO"],
                displayFieldNames: ["IDCUSTPO", "IDINVC", "IDRMIT", "IDORDERNBR", "DATEDUE",
                    "TRXTYPEID", "TRXTYPETXT", "DATEINVC", "DATEDISC",
                    "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAID"],
                filterTemplate: "IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND TRXTYPETXT = \"{2}\""
            },

            Document_ApplyByShipment: {
                viewID: "AR0036",
                viewOrder: 9,
                parentValAsInitKey: false,
                returnFieldNames: ["IDSHIPNBR"],
                displayFieldNames: ["IDSHIPNBR", "IDINVC", "IDRMIT", "IDCUSTPO", "DATEDUE",
                    "TRXTYPEID", "TRXTYPETXT", "DATEINVC", "DATEDISC",
                    "AMTDUETC", "AMTDISCTC", "SWPAID", "IDPREPAID"],
                filterTemplate: "IDCUST = \"{0}\" AND SWPAID = \"{1}\" AND TRXTYPETXT = \"{2}\""
            },

            DocumentSchedulePayment: {
                viewID: "AR0037",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "CNTPAYM", "DATEDUE", "DATEDISC", "AMTDUETC",
                    "AMTDSCRMTC", "AMTPYMRMTC", "TXTTRXTYPE", "DATEINVC"]
            },

            StartingDocumentNumber: {
                viewID: "AR0037",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["IDINVC", "SWJOB"],
                displayFieldNames: ["IDINVC", "CNTPAYM", "IDRMIT"]
            },

            StartingOrderNumber: {
                viewID: "AR0037",
                viewOrder: 2,
                parentValAsInitKey: false,
                returnFieldNames: ["IDORDRNBR", "SWJOB"],
                displayFieldNames: ["IDORDRNBR", "IDINVC", "CNTPAYM"]
            },

            StartingPoNumber: {
                viewID: "AR0037",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["IDCUSTPO", "SWJOB"],
                displayFieldNames: ["IDCUSTPO", "IDINVC", "CNTPAYM"]
            },

            StartingDueDate: {
                viewID: "AR0037",
                viewOrder: 4,
                parentValAsInitKey: false,
                returnFieldNames: ["DATEDUE", "SWJOB"],
                displayFieldNames: ["DATEDUE", "IDINVC", "CNTPAYM"]
            },

            StartingCurrentBalance: {
                viewID: "AR0037",
                viewOrder: 6,
                parentValAsInitKey: false,
                returnFieldNames: ["AMTPYMRMTC", "SWJOB"],
                displayFieldNames: ["AMTPYMRMTC", "AMTDUETC", "DATEDUE", "IDINVC", "CNTPAYM"]
            },

            StartingDocumentDate: {
                viewID: "AR0037",
                viewOrder: 8,
                parentValAsInitKey: false,
                returnFieldNames: ["DATEINVC", "SWJOB"],
                displayFieldNames: ["DATEINVC", "IDINVC", "CNTPAYM"]
            },

            StartingShipmentNumber: {
                viewID: "AR0037",
                viewOrder: 12,
                parentValAsInitKey: false,
                returnFieldNames: ["IDSHIPNBR", "SWJOB"],
                displayFieldNames: ["IDSHIPNBR", "IDINVC", "CNTPAYM"]
            },

            StartingOriginalDocumentNumber: {
                viewID: "AR0037",
                viewOrder: 13,
                parentValAsInitKey: false,
                returnFieldNames: ["RTGAPPLYTO", "SWJOB"],
                displayFieldNames: ["RTGAPPLYTO", "IDINVC", "CNTPAYM"]
            },

            PostedReceipts: {
                viewID: "AR0040",
                viewOrder: 0,
                returnFieldNames: ["IDRMIT", "IDINVC"],
                displayFieldNames: ["IDRMIT", "IDBANK", "IDCUST", "DATERMIT", "DEPSTNBR", "DATEBTCH", "AMTRMITTC", "AMTPAYM",
                    "AMTDISC", "PAYMCODE", "CODECURN", "IDRATETYPE", "RATEEXCHHC", "SWOVRDRATE", "TEXTRETRN",
                    "DATELSTMTN", "DATELSTSTM", "AMTROUNDER", "DATERATE", "FISCYR", "FISCPER", "NAMERMIT",
                    "CNTBTCH", "CNTITEM", "SWCHKCLRD", "AMTRMITHC", "AMTADJ", "DEPSEQ"],
                filterTemplate: "(IDBANK >=\"{0}\") AND (IDBANK <= \"{1}\") AND (IDCUST >=\"{2}\") AND (IDCUST <= \"{3}\") AND (DATERMIT >= {4}) AND (DATERMIT <= {5}) AND (((FISCYR = \"{6}\") AND (FISCPER >= \"{7}\")) OR (FISCYR > \"{6}\")) AND (((FISCYR = \"{8}\") AND (FISCPER <= \"{9}\")) OR (FISCYR < \"{8}\"))",
            },

            ReceiptEntryBatch: {
                viewID: "AR0041",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BATCHDESC", "DATEBTCH", "BATCHTYPE", "BATCHSTAT",
                    "SRCEAPPL", "SWPRTDEP", "AMTENTER", "IDBANK", "CODECURN"],
                filterTemplate: "CODEPYMTYP = \"{0}\" AND (BATCHSTAT = \"{1}\" OR BATCHSTAT = \"{2}\")"
            },

            ReceiptAdjustmentHeader: {
                viewID: "AR0042",
                viewOrder: 0,
                returnFieldNames: ["CNTITEM"],
                displayFieldNames: ["CNTITEM", "DOCNBR", "IDRMIT", "IDCUST", "DATERMIT",
                    "TEXTRMIT", "TEXTPAYOR", "AMTRMIT"],
                filterTemplate: "CNTBTCH = \"{0}\" AND  CODEPYMTYP = \"{1}\" "
            },

            ReceiptAdjustmentsForCheckReceiptNumber: {
                viewID: "AR0042",
                viewOrder: 2,
                returnFieldNames: ["IDRMIT"],
                displayFieldNames: ["IDRMIT", "IDCUST", "CNTBTCH", "CNTITEM", "DOCNBR", "SWPRINTED"],
                filterTemplate: "CODEPYMTYP = \"{0}\" AND RMITTYPE != \"4\" AND (ERRBATCH = 0 and ERRENTRY = 0)"
            },

            ReceiptAdjustmentsForDocumentNumber: {
                viewID: "AR0042",
                viewOrder: 3,
                returnFieldNames: ["DOCNBR"],
                displayFieldNames: ["DOCNBR", "IDCUST", "CNTBTCH", "CNTITEM", "IDRMIT", "SWPRINTED"],
                filterTemplate: "CODEPYMTYP = \"{0}\" AND RMITTYPE != \"4\" AND (ERRBATCH = 0 and ERRENTRY = 0)"
            },

            RecurringCharge: {
                viewID: "AR0046",
                viewOrder: 0,
                returnFieldNames: ["IDSTDINVC", "IDCUST"],
                displayFieldNames: ["IDSTDINVC", "IDCUST", "TEXTDESC", "SCHEDKEY", "SWACTV"]
            },

            CommentTypes: {
                viewID: "AR0094",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CMNTTYPE"],
                displayFieldNames: ["CMNTTYPE", "TEXTDESC", "ACTVSW"]
            },

            ReprintStatementRunDate: {
                viewID: "AR0110",
                viewOrder: 1,
                returnFieldNames: ["STMTDATE"],
                displayFieldNames: ["STMTDATE", "DATECUTOFF", "SWTYPERUN", "SWFINISH", "DELMETHOD", "SWDEBIT", "SWCREDIT",
                                    "SWZEROBAL", "SWINCLPAID"]
            },

            ReprintStatementCustomersByStatements: {
                viewID: "AR0111",
                viewOrder: 1,
                returnFieldNames: ["IDCUST"],
                displayFieldNames: ["IDCUST", "STMTDATE", "SWPRINTED"]
            },

            ReprintStatementNATCustomersByRunDate: {
                viewID: "AR0114",
                viewOrder: 1,
                returnFieldNames: ["IDNATACCT"],
                displayFieldNames: ["IDNATACCT", "STMTDATE", "SWPRINTED"]
            },

            EmailMessages: {
                viewID: "AR0120",
                viewOrder: 0,
                returnFieldNames: ["MSGTYPE", "MSGID", "ACTIVESW"],
                displayFieldNames: ["MSGTYPE", "MSGID", "TEXTDESC", "SUBJECT", "BODY"]
            },

            ARDocuments: {
                viewID: "AR0130",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDCUST", "TRXTYPETXT", "CNTBTCH", "CNTITEM", "IDRMIT", "IDORDERNBR",
                                    "IDCUSTPO", "DATEDUE", "TRXTYPEID", "DESCINVC", "DATEINVC", "CODETERM", "DATEDISC",
                                    "SWPAID", "IDPREPAID"]
            },

            PaymentsDocumentNumber: {
                viewID: "AR0139",
                viewOrder: 0,
                returnFieldNames: ["IDINVC"],
                displayFieldNames: ["IDINVC", "IDBANK", "CHECKNUM", "DOCDATE", "AMTTC", "AMTPC", "CODECURN", "RATETYPE",
                    "RATEEXCH", "SWRATE", "RATEDATE", "FISCYR", "FISCPER", "NAMERMIT", "CNTBTCH", "CNTITEM",
                    "SWSTATUS", "AMTHC", "TEXTRETRN"],
                filterTemplate: "(IDBANK >=\"{0}\") AND (IDBANK <= \"{1}\") AND (IDCUST >=\"{2}\") AND (IDCUST <= \"{3}\") AND (DOCDATE >= {4}) AND (DOCDATE <= {5}) AND (((FISCYR = \"{6}\") AND (FISCPER >= \"{7}\")) OR (FISCYR > \"{6}\")) AND (((FISCYR = \"{8}\") AND (FISCPER <= \"{9}\")) OR (FISCYR < \"{8}\"))"
            },

            PaymentsCheckNumber: {
                viewID: "AR0139",
                viewOrder: 1,
                returnFieldNames: ["CHECKNUM"],
                displayFieldNames: ["PAYMTYPE", "IDBANK", "CHECKNUM", "IDACCT", "DOCDATE", "FISCYR", "FISCPER", "IDCUST",
                    "NAMERMIT", "CODECURN", "AMTPC", "RATETYPE", "RATEDATE", "RATEEXCH", "AMTTC", "AMTHC", "IDINVC",
                    "SWSTATUS", "DATECLRD", "DATERVRSD", "CNTBTCH", "CNTITEM", "TEXTRETRN"],
                filterTemplate: "(IDBANK >=\"{0}\") AND (IDBANK <= \"{1}\") AND (IDCUST >=\"{2}\") AND (IDCUST <= \"{3}\") AND (IDINVC >=\"{4}\") AND (IDINVC <= \"{5}\") AND (DOCDATE >= {6}) AND (DOCDATE <= {7}) AND (((FISCYR = \"{8}\") AND (FISCPER >= \"{9}\")) OR (FISCYR > \"{8}\")) AND (((FISCYR = \"{10}\") AND (FISCPER <= \"{11}\")) OR (FISCYR < \"{10}\"))"
            },

            RefundBatch: {
                viewID: "AR0140",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTBTCH"],
                displayFieldNames: ["CNTBTCH", "BTCHDESC", "BTCHDATE", "BTCHTYPE", "BTCHSTTS", "SRCEAPPL", "ENTRYCNT", "ENTRYTOT"],
                filterTemplate: "BTCHSTTS = \"{0}\" OR BTCHSTTS = \"{1}\" OR BTCHSTTS = \"{2}\""
            },

            RefundEntry: {
                viewID: "AR0141",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTITEM"],
                displayFieldNames: ["CNTITEM", "DOCDESC", "DOCDATE", "IDCUST", "IDINVC"],
                filterTemplate: "CNTBTCH = \"{0}\""
            },

            OpenDocumentDetails: {
                viewID: "AR0200",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "CONTRACT", "PROJECT", "CATEGORY", "COSTCLASS", "RESOURCE", "AMTINVCTC", "AMTDUETC"],
                filterTemplate: "IDCUST = \"{0}\" AND IDINVC = \"{1}\""
            },

            AdjustmentEntryDocumentDetails: {
                viewID: "AR0200",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "BILLDATE", "UNITMEAS", "QTYINVC", "IDDIST", "IDGLACCT", "AMTINVCTC", "AMTDUETC", "SWDISCABL", "RTGAMTTC", "RTGOAMTTC", "RTGDATEDUE"],
                filterTemplate: "IDCUST = \"{0}\" AND IDINVC = \"{1}\""
            },

            OpenDocumentDetailsOriginalDetail: {
                viewID: "AR0200",
                viewOrder: 0,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "CONTRACT", "PROJECT", "CATEGORY", "COSTCLASS", "RESOURCE", "BILLDATE", "IDITEM","UNITMEAS", "QTYINVC", "IDDIST", "IDGLACCT", "AMTINVCTC", "AMTDUETC", "SWDISCABL", "RTGAMTTC", "RTGOAMTTC", "RTGDATEDUE"],
                filterTemplate: "IDCUST = \"{0}\" AND IDINVC = \"{1}\""
            },

            OpenDocumentDetailsInvoiceEntry: {
                viewID: "AR0200",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CNTLINE"],
                displayFieldNames: ["CNTLINE", "IDITEM", "IDDIST", "IDGLACCT", "SWDISCABL"],
                filterTemplate: "RTGAMTHC != \"{0}\" AND IDINVC = \"{1}\""
            },

            PostingJournals: {
                viewID: "AR0409",
                viewOrder: 0,
                returnFieldNames: ["POSTSEQNCE"],
                displayFieldNames: ["POSTSEQNCE", "DATEPOSTED", "DATEBUS", "SWPRINTED", "SWPOSTGL", "DATEPOSTGL"],
                filterTemplate: "TYPEBTCH = \"{0}\""
            },

            OptionalFields: {
                viewID: "AR0500",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            }
        }, 

        AS: {
            SecurityGroups: {
                viewID: "AS0001",
                viewOrder: 0,
                returnFieldNames: ["PROFILEID", "PROFDESC"],
                displayFieldNames: ["PROFILEID", "PROFDESC"],
                filterTemplate: "(PGMID = \"{0}\" AND PGMVER = \"{1}\" AND RESOURCEID = \" \" )"
            },

            Users: {
                viewID: "AS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["USERID", "USERNAME"],
                displayFieldNames: ["USERID", "USERNAME", "EMAIL1", "LANGUAGE"]
            },

            GLAccountUsers: {
                viewID: "AS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["USERID"],
                displayFieldNames: ["USERID", "USERNAME", "ACCTSTATUS", "ACCTTYPE"],
                filter: "USERID != \"ADMIN\" AND ACCTTYPE = 0"
            },

            UIProfileHeaders: {
                viewID: "AS0005",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PROFILEID"],
                displayFieldNames: ["PROFILEID", "PROFDESC"],
            },

            Customizations: {
                viewID: "AS0051",
                viewOrder: 0,
                returnFieldNames: ["CUSTID", "DESCRIPT"],
                displayFieldNames: ["CUSTID", "DESCRIPT"],
                filterTemplate: "SCREENID = \"{0}\" ",
            },

            Organizations: {
                viewID: "AS0020",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ORGID", "DESC"],
                displayFieldNames: ["ORGID", "DESC", "ORGTYPE", "SYSORGID", "SYSORGID", "ORGTYPE", "SYSORGID", "STATUS"],
                filterTemplate: "ORGID = \"{0}\" "
            }
        },

        BK: {
            Banks: {
                url: ["CS", "BankViewFinder", "Find"],
                viewID: "BK0001",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["BANK", "MULTICUR", "NAME", "CURNSTMT", "INACTIVE"],
                displayFieldNames: ["BANK", "NAME", "CURNSTMT", "ADDR1", "ADDR2", "ADDR3", "ADDR4", "CITY", "STATE",
                    "COUNTRY", "POSTAL", "CONTACT", "PHONE", "FAX", "TRANSIT", "INACTIVE", "INACTDATE", "IDACCT",
                    "BKACCT", "IDACCTERR", "LSTMNTND"]
            },

            BanksWithMultiCurrency: {
                url: ["CS", "BankViewFinder", "Find"],
                viewID: "BK0001",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["BANK", "MULTICUR", "NAME", "CURNSTMT", "INACTIVE", "BKACCT"],
                displayFieldNames: ["BANK", "NAME", "CURNSTMT", "ADDR1", "ADDR2", "ADDR3", "ADDR4", "CITY", "STATE",
                    "COUNTRY", "POSTAL", "CONTACT", "PHONE", "FAX", "TRANSIT", "MULTICUR", "INACTIVE", "INACTDATE", "IDACCT",
                    "BKACCT", "IDACCTERR", "IDACCTCCC", "LSTMNTND"]
            },

            BankCurrency: {
                viewID: "BK0002",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CURN", "CURDESC"],
                displayFieldNames: ["CURN", "RTYPCHK", "RTYPDEP", "GAINACCT", "LOSSACCT"],
                filterTemplate: "BANK = \"{0}\""
            },

            BankDistributionCodes: {
                viewID: "BK0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["TYPE", "DESC", "ACCT", "INACTIVE"],
                displayFieldNames: ["TYPE", "DESC", "ACCT", "INACTIVE", "INACTDATE","LSTMNTND"]
            },

            CheckStocks: {
                viewID: "BK0008",
                viewOrder: 0,
                returnFieldNames: ["FORMID"],
                displayFieldNames: ["BANK", "FORMID", "DESC", "STKTYPE", "FORMSPEC1", "FORMSPEC2", "ADVICE", "LANGUAGE"],
                filterTemplate: "BANK = \"{0}\""
            },

            BankPostingJournal: {
                viewID: "BK0020",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PSTSEQ"],
                displayFieldNames: ["PSTSEQ", "FROMBANK", "TOBANK", "POSTDATE", "POSTUSER", "PRINTED"]
            },

            CreditCardTypes: {
                viewID: "BK0240",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CCTYPE"],
                displayFieldNames: ["CCTYPE", "DESC", "INACTIVE", "LASTMAINT", "INACTDATE"]
            },

            BankEntriesHeaderBySequenceNumber: {
                url: ["CS", "BankEntriesHeaderViewFinder", "Find"],
                viewID: "BK0450",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["SEQUENCENO"],
                displayFieldNames: ["SEQUENCENO", "ENTRYNBR", "BANK", "TOTSRCEGRO", "TOTFUNCGRO", "POSTDATE", "TRANSTYPE", "STATUS"]
            },

            BankEntriesHeader: {
                url: ["CS", "BankEntriesHeaderViewFinder", "Find"],
                viewID: "BK0450",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["ENTRYNBR"],
                displayFieldNames: ["ENTRYNBR", "REFERENCE", "BANK", "TOTSRCEGRO", "TOTFUNCGRO", "STATUS", "POSTDATE", "TRANSTYPE"]
            },

            BankDistributionSetHeader: {
                viewID: "BK0445",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["DSETCODE"],
                displayFieldNames: ["DSETCODE", "DESC", "DISTCUR", "INACTIVE", "INACTDATE", "LSTMNTND"]
            },

            BankTransactionDetailsByRemittanceID: {
                viewID: "BK0840",
                viewOrder: 6,
                returnFieldNames: ["IDREMIT", "SERIAL", "LINE"],
                displayFieldNames: ["IDREMIT", "PAYORID", "PAYORNAME", "DATEREMIT", "FUNCAMOUNT", "BTCHNBR", "ENTRYNBR",
                    "SRCECURN", "SRCEAMOUNT", "RECOUTSTND", "VENDORNAME", "PAYMCODE", "COMMENT", "SRCEAPP", "CHKFORM",
                    "SRCEDOCNUM"],
                filterTemplate: "BANK = \"{0}\" AND SRCEAPP = \"{1}\" AND TRANSTYPE = {2} AND ENTRYTYPE = {3} AND STATUS = {4}"
            },

            BankTransactionDetails: {
                viewID: "BK0840",
                viewOrder: 7,
                parentValAsInitKey: false,
                returnFieldNames: ["IDREMIT", "SERIAL", "LINE"],
                displayFieldNames: ["IDREMIT", "PAYORID", "PAYORNAME", "DATEREMIT", "FUNCAMOUNT",
                    "BTCHNBR", "ENTRYNBR", "RECOUTSTND", "VENDORNAME", "PAYMCODE", "COMMENT",
                    "SRCEAPP", "CHKFORM", "SRCEDOCNUM"],
                filterTemplate: "BANK = \"{0}\" AND SRCEAPP = \"{1}\" AND TRANSTYPE = {2} AND ENTRYTYPE = {3} AND STATUS = {4}"
            },

            BankTransactionHeader: {
                viewID: "BK0845",
                viewOrder: 4,
                parentValAsInitKey: false,
                returnFieldNames: ["TRANSNUM", "TRANSDATE"],
                displayFieldNames: ["BANK", "TRANSNUM", "REFERENCE", "DESC", "TRANSDATE",
                    "FSCYEAR", "FSCPERIOD", "PRINTED", "TOTAMOUNT", "TOTBALAMT", "TOTCLEARED",
                    "STATUS"],
                filterTemplate: "BANK = \"{0}\" AND SRCEAPP = \"{1}\" AND STATUS = \"{2}\" AND TRANSTYPE = \"{3}\" AND RECSTATUS != \"{4}\" "
            }
        },

        CS: {
            FiscalCalendars: {
                viewID: "CS0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["FSCYEAR", "PERIODS"],
                displayFieldNames: ["FSCYEAR", "PERIODS", "QTR4PERD", "ACTIVE", "BGNDATE1", "BGNDATE2", "BGNDATE3",
                    "BGNDATE4", "BGNDATE5", "BGNDATE6", "BGNDATE7", "BGNDATE8", "BGNDATE9",
                    "BGNDATE10", "BGNDATE11", "BGNDATE12", "BGNDATE13",
                    "ENDDATE1", "ENDDATE2", "ENDDATE3", "ENDDATE4", "ENDDATE5", "ENDDATE6",
                    "ENDDATE7", "ENDDATE8", "ENDDATE9", "ENDDATE10", "ENDDATE11", "ENDDATE12", "ENDDATE13"],
            },

            FiscalCalendarsWithPeriodStatus: {
                viewID: "CS0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["FSCYEAR", "PERIODS"],
                displayFieldNames: ["FSCYEAR", "PERIODS", "QTR4PERD", "ACTIVE", "BGNDATE1", "BGNDATE2", "BGNDATE3",
                    "BGNDATE4", "BGNDATE5", "BGNDATE6", "BGNDATE7", "BGNDATE8", "BGNDATE9",
                    "BGNDATE10", "BGNDATE11", "BGNDATE12", "BGNDATE13",
                    "ENDDATE1", "ENDDATE2", "ENDDATE3", "ENDDATE4", "ENDDATE5", "ENDDATE6",
                    "ENDDATE7", "ENDDATE8", "ENDDATE9", "ENDDATE10", "ENDDATE11", "ENDDATE12", "ENDDATE13", "STATUSADJ", "STATUSCLS"]
            },

            FiscalCalendarsWithMinimalDetails: {
                viewID: "CS0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["FSCYEAR"],
                displayFieldNames: ["FSCYEAR", "PERIODS", "ACTIVE"]
            },

            AccountSetCurrencyCodes: {
                viewID: "CS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CURID", "CURNAME", "DECIMALS"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL", "DECIMALS", "SYMBOLPOS", "THOUSSEP", "DECSEP", "NEGDISP"]
            },

            CurrencyCodes: {
                viewID: "CS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CURID", "CURNAME", "DECIMALS"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL", "DECIMALS"]
            },

            CurrencyCodesWithPosition: {
                viewID: "CS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CURID", "CURNAME", "DECIMALS"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL", "DECIMALS", "SYMBOLPOS"]
            },

            CurrencyCodesWithDetails: {
                viewID: "CS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CURID", "CURNAME", "DECIMALS"],
                displayFieldNames: ["CURID", "CURNAME", "SYMBOL", "DECIMALS", "SYMBOLPOS", "THOUSSEP", "DECSEP", "NEGDISP"]
            },

            CurrencyRateType: {
                viewID: "CS0004",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["RATETYPE", "RATEDESC"],
                displayFieldNames: ["RATETYPE", "RATEDESC"]
            },

            CurrencyTable: {
                viewID: "CS0005",
                viewOrder: 0,
                returnFieldNames: ["HOMECUR", "RATETYPE"],
                displayFieldNames: ["HOMECUR", "RATETYPE", "TABLEDESC", "DATEMATCH", "RATEOPER", "RATESRCE"]
            },

            CurrencyRate: {
                viewID: "CS0006",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RATE", "RATEDATE", "RATETYPE", "SPREAD"],
                displayFieldNames: ["RATEDATE", "RATE", "SPREAD"],
                filterTemplate: "RATETYPE = \"{0}\" AND HOMECUR = \"{1}\" AND SOURCECUR = \"{2}\" "
            },

            OptionalFields: {
                viewID: "CS0011",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "ALLOWNULL", "VALIDATE"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
            },

            OptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VDESC"],
                displayFieldNames: ["VALUE", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            TextOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFTEXT", "VDESC"],
                displayFieldNames: ["VALIFTEXT", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            AmountOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFMONEY", "VDESC"],
                displayFieldNames: ["VALIFMONEY", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            NumberOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFNUM", "VDESC"],
                displayFieldNames: ["VALIFNUM", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            IntegerOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFLONG", "VDESC"],
                displayFieldNames: ["VALIFLONG", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            YesNoOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFBOOL", "VDESC"],
                displayFieldNames: ["VALIFBOOL", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            DateOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFDATE", "VDESC"],
                displayFieldNames: ["VALIFDATE", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            TimeOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VALUE", "TYPE", "VALIFTIME", "VDESC"],
                displayFieldNames: ["VALIFTIME", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },

            Schedules: {
                viewID: "CS0030",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["SCHEDKEY"],
                displayFieldNames: ["SCHEDKEY", "SCHEDDESC"]
            },

            SchedulesWithDetails: {
                viewID: "CS0030",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["SCHEDKEY", "SCHEDDESC", "FILTERDATE"],
            },

            BankCurrency: {
                viewID: "BK0002",
                viewOrder: 0,
                returnFieldNames: ["CURN", "CURDESC", "RTYPCHK"],
                displayFieldNames: ["BANK", "CURN", "RTYPCHK", "RTYPDEP", "GAINACCT", "LOSSACCT"],
                filterTemplate: "BANK = \"{0}\""
            }
        },

        GL: {
            Accounts: {
                url: ["GL", "AccountViewFinder", "Find"],
                viewID: "GL0001",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "MCSW", "ACCTID", "ACTIVESW"],
                displayFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID", "ALLOCSW", "MCSW", "QTYSW", "UOM"]
            },

            GLAccounts: {
                url: ["GL", "AccountViewFinder", "Find"],
                viewID: "GL0001",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "MCSW", "ACCTID", "ACTIVESW", "ACCTGRPCOD", "CONSLDSW", "ROLLUPSW",
                                    "ACCTTYPE", "ACCTBAL", "ACCTSEGVAL"],
                displayFieldNames: ["ACCTID", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID", "CTRLACCTSW", "ALLOCSW",
                                    "MCSW", "QTYSW", "UOM"]
            },

            SegmentCodeSortedOnAccount: {
                url: ["GL", "AccountViewFinder", "Find"],
                viewID: "GL0001",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ACCTSEGVAL", "ACCTID", "ACCTFMTTD", "ACSEGVAL01"],
                displayFieldNames: ["ACSEGVAL01", "ACCTFMTTD", "ACCTDESC"]
            },

            AccountsForSegmentCode: {
                url: ["GL", "AccountViewFinder", "Find"],
                viewID: "GL0001",
                viewOrder: 2,
                returnFieldNames: ["ACCTSEGVAL", "ACCTID", "ACCTFMTTD", "ACSEGVAL01"],
                displayFieldNames: ["ACSEGVAL01", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "MCSW"]
            },

            MainAccountSegment: {
                url: ["GL", "AccountViewFinder", "Find"],
                viewID: "GL0001",
                parentValAsInitKey: true,
                displayFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACTIVESW", "MCSW"]
            },

            SourceInquiryAccounts: {
                viewID: "GL0001",
                viewOrder: 12,
                parentValAsInitKey: false,
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "ROLLUPSW"],
                displayFieldNames: ["ACCTID", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID", "CTRLACCTSW",
                    "ALLOCSW", "MCSW", "QTYSW", "UOM"],
                filter: "MCSW = \"1\""
            },

            SourceCodes: {
                viewID: "GL0002",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["SRCELEDGER", "SRCETYPE", "SRCEDESC"],
                displayFieldNames: ["SRCELEDGER", "SRCETYPE", "SRCEDESC"],
                filterTemplate: "SRCELEDGER = \"{0}\""
            },

            JournalHeaders: {
                viewID: "GL0006",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["BTCHENTRY"],
                displayFieldNames: ["BATCHID", "BTCHENTRY", "JRNLDESC", "SRCELEDGER", "SRCETYPE", "FSCSYR", "FSCSPERD", "DATEENTRY"],
                filterTemplate: "BATCHID = \"{0}\""
            },

            Batches: {
                viewID: "GL0008",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["BATCHID"],
                displayFieldNames: ["BATCHID", "BTCHDESC", "BATCHTYPE", "BATCHSTAT", "SRCELEDGR", "DEBITTOT", "CREDITTOT",
                    "QTYTOTAL", "ERRORCNT"],
                filter: "BATCHSTAT = \"1\" OR BATCHSTAT = \"9\""
            },

            AccountValidCurrencies: {
                viewID: "GL0012",
                viewOrder: 0,
                returnFieldNames: ["CURNID"],
                displayFieldNames: ["CURNID", "ACCTID"],
                filterTemplate: "ACCTID = \"{0}\""
            },

            SourceJournalProfiles: {
                viewID: "GL0019",
                viewOrder: 0,
                parentValAsInitKey: true, 
                returnFieldNames: ["SRCEJRNL"],
                displayFieldNames: ["SRCEJRNL"]
            },

            RevaluationCodes: {
                viewID: "GL0020",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["RVALID"],
                displayFieldNames: ["RVALID", "DESC"]
            },

            SegmentCodes: {
                viewID: "GL0021",
                viewOrder: 0,
                returnFieldNames: ["SEGVAL"],
                displayFieldNames: ["SEGVAL", "SEGVALDESC"],
                filterTemplate: "IDSEG = \"{0}\""
            },

            StructureCodes: {
                viewID: "GL0023",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ACCTBRKID", "ABRKDESC"],
                displayFieldNames: ["ACCTBRKID", "ABRKDESC"]
            },

            RecurringJournalHeaders: {
                viewID: "GL0041",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["RECID"],
                displayFieldNames: ["RECID", "RECDESC", "SCHEDKEY", "SWACTIVE"]
            },

            PreviewAccounts: {
                viewID: "GL0046",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACCTID"],
                displayFieldNames: ["ACCTID", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "CTRLACCTSW", "MCSW", "QTYSW", "UOM"]
            },

            PreviewAccountsWithAccountKey: {
                viewID: "GL0046",
                viewOrder: 2,
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "ACCTID", "ACCTSEGVAL", "ACSEGVAL01"],
                displayFieldNames: ["ACSEGVAL01", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "MCSW"]
            },

            Users: {
                viewID: "GL0054",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["USER"],
                displayFieldNames: ["USER", "NAME"]
            },

            AccountGroups: {
                viewID: "GL0055",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ACCTGRPCOD", "SORTCODE"],
                displayFieldNames: ["ACCTGRPCOD", "SORTCODE", "ACCTGRPDES", "GRPCOD"]
            },

            AccountGroupsSorted: {
                viewID: "GL0055",
                viewOrder: 1,
                returnFieldNames: ["SORTCODE","ACCTGRPCOD"],
                displayFieldNames: ["SORTCODE", "ACCTGRPCOD", "ACCTGRPDES", "GRPCOD"]
            },

            AccountTransactionOptionalFields: {
                viewID: "GL0401",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "ACCTID = \"{0}\""
            },

            OptionalFields: {
                viewID: "GL0500",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            }
        },

        IC: {
            AccountSet: {
                viewID: "IC0100",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTLACCT"],
                displayFieldNames: ["CNTLACCT", "DESC", "INACTIVE", "COSTMETHOD"]
            },

            AdjustmentHeader: {
                viewID: "IC0120",
                viewOrder: 3,
                returnFieldNames: ["DOCNUM"],
                displayFieldNames: ["DOCNUM", "HDRDESC", "TRANSDATE", "FISCYEAR", "FISCPERIOD", "REFERENCE", "STATUS", "TRANSNUM"],
                filter: "DELETED = 0" 
            },

            Assembly: {
                viewID: "IC0160",
                viewOrder: 5,
                parentValAsInitKey: true,
                returnFieldNames: ["DOCNUM", "ENTEREDBY"],
                displayFieldNames: ["DOCNUM", "HDRDESC", "TRANSDATE", "FISCYEAR", "FISCPERIOD", "REFERENCE", "TRANSTYPE", "ITEMNO",
                    "BOMNO", "LOCATION", "QUANTITY", "UNIT", "STATUS", "TRANSNUM", "FROMASSNUM", "FROMASSQTY", "MASTASSNUM", "SITEMCOUNT", "LITEMCOUNT"],
                filter: "DELETED = 0"
            },

            BOMNumber: {
                viewID: "IC0200",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["BOMNO"],
                displayFieldNames: ["FMTITEMNO", "BOMNO", "ITEMDESC", "DESC", "REMARK", "FIXEDCOST", "BUILDQTY", "UNIT",
                    "VARBLCOST", "STARTDATE", "ENDDATE", "INACTIVE"],
                filterTemplate: "ITEMNO = \"{0}\""
            },

            BOMNumber_Items: {
                viewID: "IC0200",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ITEMNO", "FMTITEMNO", "BOMNO", "BUILDQTY", "UNIT", "ITEMDESC"],
                displayFieldNames: ["FMTITEMNO", "BOMNO", "ITEMDESC", "DESC", "REMARK", "FIXEDCOST", "BUILDQTY", "UNIT",
                    "VARBLCOST", "STARTDATE", "ENDDATE", "INACTIVE"],
                filterTemplate: "BOMNO = \"{0}\" "
            },

            Category: {
                viewID: "IC0210",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CATEGORY", "DESC", "INACTIVE"],
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COMMSNPAID"]
            },

            ReceiptCost: {
                viewID: "IC0260",
                viewOrder: 0,
                returnFieldNames: ["RECEIPTNUM"],
                displayFieldNames: ["RECEIPTNUM", "TRANSDATE", "QTY", "COST", "SHIPQTY",
                    "SHIPCOST"],
            },

            ContractPricing: {
                viewID: "IC0274",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CUSTNO"],
                displayFieldNames: ["CUSTNO", "PRICEBY", "CATEGORY", "ITEMNO", "PRICELIST", "USELOWEST", "STARTDATE",
                    "EXPIRE", "PRICETYPE", "CUSTTYPE", "DISCPER", "DISCAMT", "COSTMETHOD", "PLUSAMT",
                    "PLUSPER", "FIXPRICE", "CALCPRICE", "CALCDECS", "FMTITEMNO", "CUSTDESC", "CATDESC",
                    "ITEMDESC", "PRLSTDESC", "CURRDESC", "PRUNTDESC", "PRICPRICBY"],
            },

            InternalUsage: {
                viewID: "IC0288",
                viewOrder: 3,
                returnFieldNames: ["DOCNUM", "SEQUENCENO"],
                displayFieldNames: ["DOCNUM", "HDRDESC", "TRANSDATE", "FISCYEAR", "FISCPERIOD",
                    "REFERENCE", "STATUS", "TRANSNUM"],
                filterTemplate: "DELETED = \"{0}\""
            },

            ManufacturerItemNumber: {
                viewID: "IC0305",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["MANITEMNO", "ITEMNO", "FMTITEMNO", "ITEMDESC", "UNIT"],
                displayFieldNames: ["MANITEMNO", "FMTITEMNO", "UNIT", "ITEMDESC"],
                filterTemplate: "MANITEMNO = \"{0}\""
            },

            MaskStructure: {
                viewID: "IC0805",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["MASKCODE"],
                displayFieldNames: ["MASKCODE", "MASKTYPE", "DESC", "MASKSTRUCT"]
            },

            Item: {
                viewID: "IC0310",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO", "ITEMNO", "STOCKUNIT", "DESC"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT",
                    "PICKINGSEQ", "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA",
                    "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM", "ITEMNO"]
                //optionalFieldBindings: "IC0313,IC0377[0]"
            },

            Items: {
                viewID: "IC0310",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["FMTITEMNO", "ITEMNO", "COSTMETHOD", "STOCKITEM", "INACTIVE", "KITTING", "DESC", "STOCKUNIT", "SELLABLE", "PICKINGSEQ"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT",
                    "PICKINGSEQ", "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA",
                    "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM"]
            },

            Item_Alternative: {
                viewID: "IC0310",
                viewOrder: 2,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO", "ITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT",
                    "PICKINGSEQ", "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA",
                    "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM"]
            },

            ItemStructure: {
                viewID: "IC0320",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ITEMBRKID", "DESC"],
                displayFieldNames: ["ITEMBRKID", "DESC"]
            },

            VendorItemNumbers: {
                viewID: "IC0340",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["ITEMNO", "VENDITEM"],
                displayFieldNames: ["ITEMNO", "VENDTYPE", "VENDNUM", "VENDNAME", "VENDITEM", "VENDCONT", "VENDCNCY", "VENDCOST",
                    "VENDEXISTS", "FACTOR", "COSTUNIT"],
                filterTemplate: "ITEMNO = \"{0}\""
            },

            KittingItem: {
                viewID: "IC0356",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["KITNO", "FMTITEMNO", "ITEMDESC", "ITEMNO", "DESC"],
                displayFieldNames: ["KITNO", "FMTITEMNO", "ITEMDESC", "DESC", "REMARK", "BUILDQTY", "UNIT"],
                filterTemplate: "ITEMNO = \"{0}\" "
            },

            Label: {
                viewID: "IC0360",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RECPNUM"],
                displayFieldNames: ["RECPNUM"]
            },

            Location: {
                viewID: "IC0370",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["LOCATION", "DESC", "LOCTYPE", "INACTIVE"],
                displayFieldNames: ["LOCATION", "DESC", "INACTIVE", "LOCTYPE"]
            },

            LocationQuantity: {
                viewID: "IC0372",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["LOCATION"],
                displayFieldNames: ["LOCATION", "DESC", "ACTIVE", "AQTYONHAND", "QTYONORDER",
                    "QTYSALORDR", "QTYAVAIL"]
            },

            OptionalFields: {
                viewID: "IC0377",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            },

            PriceListCodes: {
                viewID: "IC0390",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PRICELIST", "DESC"],
                displayFieldNames: ["PRICELIST", "DESC"]
            },

            DetailedPriceListCodes: {
                viewID: "IC0390",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PRICELIST", "DESC"],
                displayFieldNames: ["PRICELIST", "DESC", "PRICETYPE", "PRICEFMT", "PRCNTLVL1", "PRCNTLVL2", "PRCNTLVL3", "PRCNTLVL4", "PRCNTLVL5",
                    "PRICEBASE", "PRICEQTY1", "PRICEQTY2", "PRICEQTY3", "PRICEQTY4", "PRICEQTY5", "PRICEDECS", "ROUNDMETHD", "ROUNDAMT",
                    "AMOUNTLVL1", "AMOUNTLVL2", "AMOUNTLVL3", "AMOUNTLVL4", "AMOUNTLVL5"]
            },

            ItemPricing: {
                viewID: "IC0480",
                viewOrder: 2,
                returnFieldNames: ["PRICELIST", "DESC", "CURRENCY", "ITEMNO"],
                displayFieldNames: ["FMTITEMNO", "PRICELIST", "DESC", "PRICEDECS", "PRICEBY", "PRICESTART", "PRICEEND"],
                filterTemplate: "ITEMNO = \"{0}\" AND CURRENCY = \"{1}\""
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
                filter: "DELETED = 0"
                //viewFinder.optionalFieldBindings = "IC0595, IC0377[2]";  // comment out for now as CSFND doesn't support filterCount yet
            },

            ReorderQuantities: {
                viewID: "IC0599",
                viewOrder: 0,
                displayFieldNames: ["FMTITEMNO", "LOCATION", "ORDERFOR", "VALUES"],
                returnFieldNames: ["FMTITEMNO", "ITEMNO", "LOCATION"]
            },

            SegmentCode: {
                viewID: "IC0620",
                viewOrder: 0,
                returnFieldNames: ["SEGVAL", "DESC"],
                displayFieldNames: ["SEGVAL", "DESC"],
                filterTemplate: "SEGMENT = \"{0}\""
            },

            ShipmentNumber: {
                viewID: "IC0640",
                viewOrder: 3,
                returnFieldNames: ["DOCNUM"],
                displayFieldNames: ["DOCNUM", "HDRDESC", "TRANSDATE", "FISCYEAR", "FISCPERIOD",
                    "REFERENCE", "TRANSTYPE", "CUSTNO", "CUSTNAME", "CUSTEXISTS", "CONTACT",
                    "CURRENCY", "PRICELIST", "EXCHRATE", "RATETYPE", "RATEDATE", "RATEOP",
                    "RATEOVRRD", "STATUS", "TRANSNUM"],
                filter: "DELETED = 0"
            },

            TransferHeader: {
                viewID: "IC0740",
                viewOrder: 3,
                returnFieldNames: ["DOCNUM"],
                displayFieldNames: ["DOCNUM", "DOCTYPE", "HDRDESC", "TRANSDATE", "FISCYEAR",
                    "FISCPERIOD", "EXPARDATE", "REFERENCE", "ADDCOST", "PRORMETHOD", "MPRORATE",
                    "STATUS", "TRANSNUM"]
            },

            UnitOfMeasure: {
                viewID: "IC0746",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["UNIT", "DEFCONV"],
                displayFieldNames: ["UNIT", "DEFCONV"]
            },

            ItemUnitOfMeasure: {
                viewID: "IC0750",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["UNIT", "CONVERSION"],
                displayFieldNames: ["UNIT", "CONVERSION"],
                filterTemplate: "ITEMNO=\"{0}\"",
                filter: "ITEMNO=UNFMTITEM"
            },

            WeightUnitOfMeasure: {
                viewID: "IC0758",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WEIGHTUNIT", "WUOMDESC", "CONVERSION"],
                displayFieldNames: ["WEIGHTUNIT", "WUOMDESC", "CONVERSION", "DEFAULT"]
            },

            InventoryWorksheet: {
                viewID: "IC0770",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["LOCATION"],
                displayFieldNames: ["LOCATION", "POSTTYPE", "DESC", "COMMENT"]
            },

            ContractCode: {
                viewID: "IC0800",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CONTCODE"],
                displayFieldNames: ["CONTCODE", "CONTDESC"]
            },

            WarrantyCode: {
                viewID: "IC0850",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WARRCODE"],
                displayFieldNames: ["WARRCODE", "WARRDESC"]
            },

            BillsMaterial: {
                viewID: "IC0200",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ITEMNO", "BOMNO"],
                displayFieldNames: ["ITEMNO", "BOMNO", "ITEMDESC", "DESC", "REMARK", "FIXEDCOST", "BUILDQTY", "VARBLCOST", "STARTDATE", "ENDDATE", "INACTIVE"]
            },

            LotRecallRelease: {
                viewID: "IC0822",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["DOCNUM", "LOTNUM", "TRANSTYPE", "ITEMNO", "TRANSDATE"],
                displayFieldNames: ["DOCNUM", "LOTNUM", "ITEMNO", "TRANSDATE"]
            },

            LotNumber: {
                viewID: "IC0810",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["LOTNUM", "LOTNUMF", "ITEMNUM"],
                displayFieldNames: ["LOTNUMF", "ITEMNUM", "LOCATION", "QTYAVAIL", "QTYORDED", "STOCKDATE", "EXPIRYDATE", "QUARTRELDT", "RECALLED", "RECALLDATE"]
            },

            SerialNumber: {
                viewID: "IC0830",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["SERIALNUM", "ITEMNUM", "SERIALNUMF"],
                displayFieldNames: ["SERIALNUMF", "ITEMNUM", "LOCATION", "STATUS", "STOCKDATE", "EXPIRYDATE", "ASSETCOST"]
            }
        },

        OE: {
            CreditDebitNote: {
                viewID: "OE0240",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["CRDNUMBER", "DESC", "CUSTOMER", "ADJTYPE"],
                displayFieldNames: ["CRDNUMBER", "DESC", "CRDDATE", "RETDATE", "CRDFISCYR",
                    "CRDFISCPER", "CUSTOMER", "BILNAME", "INVNUMBER", "REFERENCE", "INVDATE"]
            },

            Invoices: {
                viewID: "OE0420",
                viewOrder: 6,
                parentValAsInitKey: true,
                returnFieldNames: ["INVNUMBER"],
                displayFieldNames: ["INVNUMBER", "DESC", "INVDATE", "INVFISCYR", "INVFISCPER", "CUSTOMER", "BILNAME", "ORDNUMBER",
                                    "ORDDATE", "BILADDR1", "BILADDR2", "BILADDR3", "BILADDR4", "BILCITY", "BILSTATE", "BILZIP",
                                    "BILCOUNTRY", "BILPHONE", "BILFAX", "BILCONTACT", "SHPNAME", "SHPADDR1", "SHPADDR2",
                                    "SHPADDR3", "SHPADDR4", "SHPCITY", "SHPSTATE", "SHPZIP", "SHPCOUNTRY", "SHPPHONE", "SHPFAX",
                                    "SHPCONTACT", "PONUMBER", "REFERENCE", "INVNETWTX", "SALESPER1", "SALESPER2", "SALESPER3",
                                    "SALESPER4", "SALESPER5"]
            },

            Invoices_Customers: {
                viewID: "OE0420",
                viewOrder: 6,
                parentValAsInitKey: false,
                returnFieldNames: ["INVNUMBER", "CUSTOMER"],
                displayFieldNames: ["INVNUMBER", "DESC", "INVDATE", "INVFISCYR", "INVFISCPER", "CUSTOMER", "BILNAME", "ORDNUMBER",
                    "ORDDATE", "BILADDR1", "BILADDR2", "BILADDR3", "BILADDR4", "BILCITY", "BILSTATE", "BILZIP",
                    "BILCOUNTRY", "BILPHONE", "BILFAX", "BILCONTACT", "SHPNAME", "SHPADDR1", "SHPADDR2",
                    "SHPADDR3", "SHPADDR4", "SHPCITY", "SHPSTATE", "SHPZIP", "SHPCOUNTRY", "SHPPHONE", "SHPFAX",
                    "SHPCONTACT", "PONUMBER", "REFERENCE", "INVNETWTX", "SALESPER1", "SALESPER2", "SALESPER3",
                    "SALESPER4", "SALESPER5"],
                filterTemplate: "CUSTOMER = \"{0}\""
            },

            MiscellaneousCharge: {
                viewID: "OE0440",
                viewOrder: 0,
                returnFieldNames: ["MISCCHARGE", "CURRENCY"],
                displayFieldNames: ["MISCCHARGE", "DESC", "HASJOB", "MISCACCT", "AMOUNT", "MISCACDESC"],
                filterTemplate: "CURRENCY = \"{0}\""
            },

            MiscellaneousChargeReport: {
                viewID: "OE0440",
                parentValAsInitKey: true,
                viewOrder: 0,
                returnFieldNames: ["MISCCHARGE"],
                displayFieldNames: ["MISCCHARGE", "DESC", "HASJOB", "MISCACCT", "AMOUNT", "MISCACDESC"],
            },

            EmailMessage: {
                viewID: "OE0465",
                viewOrder: 0,
                returnFieldNames: ["MSGID", "MSGTYPE"],
                displayFieldNames: ["MSGTYPE", "MSGID", "TEXTDESC", "SUBJECT", "BODY"]
            },

            OptionalFields: {
                viewID: "OE0470",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            },

            OrderEntry: {
                viewID: "OE0520",
                viewOrder: 1,
                returnFieldNames: ["ORDNUMBER", "PONUMBER"],
                displayFieldNames: ["ORDNUMBER", "DESC", "ORDDATE", "TYPE", "CUSTOMER", "BILNAME", "PONUMBER", "REFERENCE",
                    "EXPDATE", "QUONUMBER", "QTEXPDATE", "ONHOLD", "ORDERSOURC", "PRINTSTAT", "COMPLETE",
                    "BILADDR1", "BILADDR2", "BILADDR3", "BILADDR4", "BILCITY", "BILSTATE", "BILZIP",
                    "BILCOUNTRY", "BILPHONE", "BILFAX", "BILCONTACT", "SHPNAME", "SHPADDR1", "SHPADDR2",
                    "SHPADDR3", "SHPADDR4", "SHPCITY", "SHPSTATE", "SHPZIP", "SHPCOUNTRY", "SHPPHONE",
                    "SHPFAX", "SHPCONTACT", "SHIPVIA", "ORDPAYMENT", "INVNETWTX", "SALESPER1", "SALESPER2",
                    "SALESPER3", "SALESPER4", "SALESPER5", "HASPREAUTH"],
                filterTemplate: "CUSTOMER = \"{0}\" AND TYPE = \"1\" AND (COMPLETE = \"1\" OR COMPLETE = \"2\") AND (CUSTEXIST = \"1\" OR (COMPANYID = \"0\" AND OPPOID = \"0\"))"
            },

            Templates: {
                viewID: "OE0540",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["TEMPLATE"],
                displayFieldNames: ["TEMPLATE", "PLATEDESC"]
            },

            ItemPricing: {
                viewID: "OE0630",
                viewOrder: 0,
                //This view has a maximum records of 8 and cannot perform pagination
                //because the RECORDTYPE key is a presentation list which 6 out of 8 of the selections have the same text
                //and GoBottom call fails with calling Browse without filter underneath the Accpac.Net layer
                pageSize: 8,
                returnFieldNames: ["CALCPRICE"],
                displayFieldNames: ["CUSTTYPE", "DISCPERCNT", "CALCPRICE", "PRICEUNIT", "ORDERPRICE", "ORDERUNIT"]
            },

            Shipment: {
                viewID: "OE0692",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["SHINUMBER"],
                displayFieldNames: ["SHINUMBER", "DESC", "SHIDATE", "CUSTOMER", "BILNAME", "PONUMBER", "REFERENCE", "EXPDATE", "PRINTSTAT",
                    "COMPLETE", "BILADDR1", "BILADDR2", "BILADDR3", "BILADDR4", "BILCITY", "BILSTATE", "BILZIP", "BILCOUNTRY", "BILPHONE",
                    "BILFAX", "BILCONTACT", "SHPNAME", "SHPADDR1", "SHPADDR2", "SHPADDR3", "SHPADDR4", "SHPCITY", "SHPSTATE", "SHPZIP",
                    "SHPCOUNTRY", "SHPPHONE", "SHPFAX", "SHPCONTACT", "SHIPVIA", "SALESPER1", "SALESPER2", "SALESPER3", "SALESPER4", "SALESPER5"]
            },

            ShipViaCodes: {
                viewID: "OE0760",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODE", "NAME"],
                displayFieldNames: ["CODE", "NAME", "ADDRESS1", "ADDRESS2", "ADDRESS3", "ADDRESS4", "CITY", "STATE", "ZIP",
                    "COUNTRY", "PHONE", "FAX", "CONTACT", "COMMENT", "EMAIL", "PHONEC", "FAXC", "EMAILC"]
            }
        },

        PM: { // aka PJC
            CostTypes: {
                viewID: "PM0001",
                parentValAsInitKey: true,
                viewOrder: 0,
                returnFieldNames: ["COSTTYPE", "DESC"],
                displayFieldNames: ["COSTTYPE", "DESC", "INACTIVE", "TYPE"]
            },

            Labor: {
                viewID: "PM0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["STAFFCODE", "NAME", "INACTIVE", "EARNCODE", "GROUP"],
                returnFieldNames: ["STAFFCODE"]
            },

            ContractStructure: {
                viewID: "PM0011",
                viewOrder: 0,
                returnFieldNames: ["JOBBRKID", "DESC"],
                displayFieldNames: ["JOBBRKID", "DESC"]
            },

            AccountSet: {
                viewID: "PM0017",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDACCTSET", "TEXTDESC"],
                displayFieldNames: ["IDACCTSET", "TEXTDESC", "INACTIVE", 'CURNCODE']
            },

            Contract: {
                viewID: "PM0021",
                viewOrder: 1,
                returnFieldNames: ["FMTCONTNO", "CONTRACT", "DESC"],
                displayFieldNames: ["FMTCONTNO", "DESC", "CUSTOMER", "MANAGER", "STATUS", "STARTDATE", "CURENDDATE", "CLOSEDDATE"],
                //optionalFieldBindings: "PM0850,PM0500[1]"
            },

            ContractReport: {
                viewID: "PM0021",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["FMTCONTNO", "CONTRACT", "DESC"],
                displayFieldNames: ["FMTCONTNO", "DESC", "STATUS", "CONTBRKID", "CUSTOMER", "IDACCTSET", "MANAGER", "OPENED"],
            },

            ContractGrid: {
                viewID: "PM0021",
                viewOrder: 1,
                returnFieldNames: ["FMTCONTNO", "CONTRACT"],
                displayFieldNames: ["FMTCONTNO", "DESC", "STATUS", "CONTBRKID", "CUSTOMER", "IDACCTSET", "MANAGER", "OPENED", "ARACCTSET"],
                initKeyFieldNames: ["CONTRACT"],
            },

            ContractSettings: {
                viewID: "PM0021",
                viewOrder: 1,
                returnFieldNames: ["OVERHEAD", "LABOR", "USELABOR", "USEOVERH"],
                displayFieldNames: ["FMTCONTNO"],
            },

            ChargeCode: {
                viewID: "PM0023",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CHARGECODE"],
                displayFieldNames: ["CHARGECODE", "DESC", "INACTIVE"]
            },

            MaterialAllocationCategory: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "BILLTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY", "DESC"],
                initKeyFieldNames: ["CONTRACT", "PROJECT", "CATEGORY"],
                filter: "CONTRACT=CONTRACT AND PROJECT=PROJECT",
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" ",
            },

            MaterialAllocationContract: {
                viewID: "PM0021", // Note: May be altered at runtime if IC is active
                viewOrder: 1,
                returnFieldNames: ["FMTCONTNO", "CONTRACT", "DESC"],
                displayFieldNames: ["FMTCONTNO", "DESC", "STATUS", "CONTBRKID", "CUSTOMER", "IDACCTSET", "MANAGER", "OPENED", "ARACCTSET"],
                initKeyFieldNames: ["CONTRACT"],
                parentValAsInitKey: true,
            },                

            MaterialAllocationLocation: {
                viewID: "IC0372",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["LOCATION", "DESC"],
                displayFieldNames: ["LOCATION", "DESC", "ACTIVE", "AQTYONHAND", "QTYONORDER", "QTYSALORDR", "QTYAVAIL"]
            },

            MaterialAllocationProject: {
                viewID: "PM0022",
                viewOrder: 2,
                returnFieldNames: ["PROJECT", "DESC"],
                displayFieldNames: ["PROJECT", "DESC", "PROJSTAT", "CUSTOMER", "IDACCTSET", "CLOSECOST", "PROJTYPE", "REVREC", "BILLTYPE", "OPENED", "MULTICUST", "ARACCTSET"],
                initKeyFieldNames: ["CONTRACT", "PROJECT"],
                filter: "CONTRACT=CONTRACT",
                filterTemplate: "CONTRACT = \"{0}\" ",
            },

            MaterialAllocationUnitOfMeasure: {
                viewID: "IC0750", // Note: May be altered at runtime if IC is active
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["UNIT", "CONVERSION"],
                displayFieldNames: ["UNIT", "CONVERSION"],
                filterTemplate: "ITEMNO=\"{0}\"",
                filter: "ITEMNO=UNFMTITEM"
            },

            Project: {
                viewID: "PM0022",
                viewOrder: 2,
                displayFieldNames: ["PROJECT", "DESC", "CUSTOMER", "IDACCTSET", "CUSTCCY", "MULTICUST", "PONUMBER", "PROJSTAT",
                    "PROJTYPE", "REVREC", "BILLTYPE", "CLOSEBILL", "CLOSECOST", "STARTDATE", "CURENDDATE", "ORJENDDATE",
                    "CLOSEDDATE", "CODETAXGRP"],
                returnFieldNames: ["PROJECT", "DESC"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" ",
                //optionalFieldBindings: "PM0851,PM0500[2]"
            },

            ProjectReport: {
                viewID: "PM0006",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["PROJECT", "DESC", "INACTIVE", "PROJTYPE", "REVREC", "BILLTYPE"],
                returnFieldNames: ["PROJECT"],
            },

            ProjectGrid: {
                viewID: "PM0022",
                viewOrder: 2,
                returnFieldNames: ["PROJECT", "DESC"],
                displayFieldNames: ["PROJECT", "DESC", "PROJSTAT", "CUSTOMER", "IDACCTSET", "CLOSECOST", "PROJTYPE", "REVREC", "BILLTYPE", "OPENED", "MULTICUST", "ARACCTSET"],
                initKeyFieldNames: ["CONTRACT", "PROJECT"],
                filter: "CONTRACT=CONTRACT",
                filterTemplate: "CONTRACT = \"{0}\" ",
            },

            ProjectTransactionHistory: {
                viewID: "PM0006",
                viewOrder: 0,
                returnFieldNames: ["PROJECT"],
                displayFieldNames: ["PROJECT", "DESC", "INACTIVE", "REVREC", "BILLTYPE"],
                initKeyFieldNames: ["PROJECT"],
            },

            Equipment: {
                viewID: "PM0025",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["EQUIPMENT", "DESC", "INACTIVE"],
                returnFieldNames: ["EQUIPMENT", "DESC"]
            },

            Subcontractor: {
                viewID: "PM0026",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["SUBCONT", "NAME", "DESC", "INACTIVE", "VENDORID"],
                returnFieldNames: ["SUBCONT"]
            },

            StructureCode: {
                viewID: "PM0011",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["JOBBRKID", "DESC"],
                returnFieldNames: ["JOBBRKID"]
            },

            Miscellaneous: {
                viewID: "PM0028",
                viewOrder: 0,
                parentValAsInitKey: true,
                displayFieldNames: ["MISCCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["MISCCODE"]
            },

            Overhead: {
                viewID: "PM0029",
                parentValAsInitKey: true,
                viewOrder: 0,
                displayFieldNames: ["OHCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["OHCODE"]
            },

            CategoryAll: {
                viewID: "PM0018",
                viewOrder: 0,
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COSTTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY"],
                parentValAsInitKey: true
            },

            CategoryTypes: {
                viewID: "PM0018",
                viewOrder: 0,
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COSTTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY", "COSTTYPE", "OVERHD", "LABOR"],
            },

            Category: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "TYPE", "BILLTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY", "DESC"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" ",
                //optionalFieldBindings: "PM0852,PM0500[3]"
            },

            CategoryReport: {
                viewID: "PM0018",
                parentValAsInitKey: true,
                viewOrder: 0,
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COSTTYPE", "TYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY"],
            },

            CategoryGrid: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "BILLTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY", "DESC"],
                initKeyFieldNames: ["CONTRACT", "PROJECT", "CATEGORY"],
                filter: "CONTRACT=CONTRACT AND PROJECT=PROJECT",
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" ",
            },

            CategoryAdjGrid: {
                viewID: "PM0039",
                viewOrder: 2,
                displayFieldNames: ["CATEGORY", "DESC", "COSTTYPE", "BILLTYPE", "OHACCT", "LABOR"],
                returnFieldNames: ["CATEGORY"],
                initKeyFieldNames: ["CONTRACT", "PROJECT", "CATEGORY"],
                filter: "CONTRACT=CONTRACT AND PROJECT=PROJECT",
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" ",
            },

            CategoryTransactionHistory: {
                viewID: "PM0018",
                viewOrder: 0,
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COSTTYPE", "OVERHD", "LABOR"],
                returnFieldNames: ["CATEGORY"],
            },

            Resource: {
                viewID: "PM0121",
                viewOrder: 3,
                displayFieldNames: ["RESOURCE", "RESDESC"],
                returnFieldNames: ["RESOURCE", "RESDESC"],
                /*extra*/
                filterTemplate: "CONTRACT = \"{0}\" AND PROJECT = \"{1}\" AND CATEGORY = \"{2}\" ",
                filter: "CONTRACT=CONTRACT AND PROJECT=PROJECT AND CATEGORY=CATEGORY",
            },

            CostEntriesHeader: {
                viewID: "PM0420",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["DOCNUM"],
                displayFieldNames: ["DOCNUM", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            MaterialReturns: {
                viewID: "PM0046",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["MATERIALNO"],
                displayFieldNames: ["MATERIALNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            MaterialAllocationHeader: {
                viewID: "PM0460",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["MALLOCNO"],
                displayFieldNames: ["MALLOCNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            MaterialUsage: {
                viewID: "PM0050",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["MATERIALNO"],
                displayFieldNames: ["MATERIALNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            Employee: {
                viewID: "PM0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["STAFFCODE", "NAME"],
                displayFieldNames: ["STAFFCODE", "NAME", "INACTIVE", "EARNCODE","GROUP"],
            },

            EquipmentUsage: {
                viewID: "PM0030",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["EQUIPNO"],
                displayFieldNames: ["EQUIPNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
             },

            Timecard: {
                viewID: "PM0040",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["TIMECARDNO"],
                displayFieldNames: ["TIMECARDNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            Charges: {
                viewID: "PM0054",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["CHRGNO"],
                displayFieldNames: ["CHRGNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            Adjustments: {
                viewID: "PM0062",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["ADJUSTNO"],
                displayFieldNames: ["ADJUSTNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            ReviseEstimates: {
                viewID: "PM0058",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["CHNGORDNO"],
                displayFieldNames: ["CHNGORDNO", "DESC", "REFERENCE", "TRANSDATE", "COMPLETE"],
            },

            OpeningBalanceDocNumber: {
                viewID: "PM0401",
                viewOrder: 1,
                parentValAsInitKey: true,
                displayFieldNames: ["DOCNUM", "DESC", "TRANSDATE", "COMPLETE"],
                returnFieldNames: ["DOCNUM"]
            },

            RetainageNumber: {
                viewID: "PM0412",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: [
                    "DOCNUM",
                    "RETTYPE",
                    "TRANSDATE",
                    "FISCALYEAR",
                    "FISCALPER",
                    "COMPLETE",
                    "REFERENCE",
                    "DESC"
                ],
                displayFieldNames: [
                    "DOCNUM",
                    "TRANSDATE",
                    "COMPLETE",
                    "REFERENCE",
                    "DESC"
                ]
            },

            RetainageNumberDetail: {
                viewID: "PM0410",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["LINENO"],
                displayFieldNames: ["LINENO", "CUSTCCY", "FMTCONTNO", "PROJECT", "CATEGORY", "ODOCNUM"],
                filterTemplate: "DOCNUM = \"{0}\"",
            },

            SegmentCode: {
                viewID: "PM0014",
                viewOrder: 0,
                //parentValAsInitKey: true,
                returnFieldNames: ["SEGVAL"],
                displayFieldNames: ["SEGVAL", "DESC"],
                filterTemplate: "SEGMENT = \"{0}\"",
            },

            AdjustmentDocument: {
                viewID: "PM0111",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["DOCNUM", "TRANSNUM", "COSTREV", "DOCTYPE", "EXPTYPE", "EARNINGS"], //PMTRANS.IDX_COSTNUM, PMTRANS.IDX_REVNUM)
                displayFieldNames: ["DOCNUM", "DOCTYPE", "TRANSDATE", "FISCALYEAR", "FISCALPER", "REFERENCE", "DESC", "BILLTYPE", "ARITEM", "QUANTITY", "ARUOM", "UNITCOST", "EXTCOSTHM", "BILLRATE", "EXTBILLSR", "TRANACCT", "BILLCCY", "WIPACCT","CVACCT"],
                filterTemplate: 'CONTRACT = "{0}" AND PROJECT = "{1}" AND CATEGORY = "{2}" AND RESOURCE = "{3}"'
            },

            AdjustmentDocument1: {
                viewID: "PM0995",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["DOCNUM", "COSTNUM", "REVNUM", "COSTREV", "DOCTYPE", "TIMETYPE"],
                displayFieldNames: ["DOCNUM", "DOCTYPE", "TRANSDATE", "FISCALYEAR", "FISCALPER", "REFERENCE", "DESC", "BILLTYPE", "ARITEM", "QUANTITY", "ARUOM", "UNITCOST", "EXTCOSTHM", "BILLRATE", "EXTBILLSR", "TRANACCT", "BILLCCY", "WIPACCT", "CVACCT"],
                filterTemplate: 'CONTRACT = "{0}" AND PROJECT = "{1}" AND CATEGORY = "{2}" AND RESOURCE = "{3}" AND MODULE !="PO"'
            },

            ICCostDocument: {
                viewID: "IC0260",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["RECEIPTNUM", "TRANSDATE", "QTY", "COST", "SHIPQTY", "SHIPCOST"],
                displayFieldNames: ["RECEIPTNUM", "TRANSDATE", "QTY", "COST", "SHIPQTY", "SHIPCOST"],
                filterTemplate: 'FMTITEMNO = "{0}" AND LOCATION = "{1}"'
            },

            PMPayrollEarnings: {
                viewID: "PM0801",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CODE", "DESC"],
                displayFieldNames: ["CODE", "DESC"],
            },

            PMPayrollExpense: {
                viewID: "PM0802",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CODE", "DESC"],
                displayFieldNames: ["CODE", "DESC"],
            },

            TimecardAudit: {
                viewId: "PM0042",
                viewOrder: 4,
                parentValAsInitKey: false,
                returnFieldNames: ["TIMECARDNO", "PAYUPDATED"],
                displayFieldNames: ["TIMECARDNO"],
            }
        },
        PO: {

            AccountSet: {
                viewID: "PO0100",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CNTLACCT"],
                displayFieldNames: ["CNTLACCT", "DESC", "INACTIVE", 'COSTMETHOD']
            },

            ItemPOStandalone: {
                viewID: "PO0124",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "CNTLACCT", "SELLABLE"],
                //optionalFieldBindings: "PO0125,PO0119[0]"
            },

            Items: {
                viewID: "PO0124",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "CNTLACCT", "SELLABLE"]
            },

            VendorContractCosts: {
                viewID: "PO0181",
                viewOrder: 0,
                returnFieldNames: ["ITEMNO", "VDCODE"],
                displayFieldNames: ["FMTITEMNO", "ITEMDESC", "VDCODE", "VDNAME", "VDCURR", "VCPDESC"],
            },

            Costs: {
                viewID: "PO0297",
                viewOrder: 0,
                pageSize: 8,
                parentValAsInitKey: false,
                returnFieldNames: ["COSTTYPE", "UNITCOST"],
                displayFieldNames: ["COSTTYPE", "UNITCOST"],
            },

            AdditionalCosts: {
                viewID: "PO0300",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["ADDCOST"],
                displayFieldNames: ["ADDCOST", "DESC", "INACTIVE", "AMOUNT", "PRORMETHOD", "REPRORATE"],
                filterTemplate: "VDCODE = \"{0}\" OR (VDCODE = \"\" AND CURRENCY = \"{1}\")"
            },

            DuplicateCreditDebitNote: {
                viewID: "PO0308",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CRNHSEQ", "CRNNUMBER"],
                displayFieldNames: ["CRNNUMBER", "VDCODE", "DOCDATE", "DOCTOTAL"],
            },

            CreditDebitNote: {
                viewID: "PO0311",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["CRNNUMBER", "CRNHSEQ"],
                displayFieldNames: ["CRNNUMBER", "VDCODE", "VDNAME", "TRANSTYPE", "DATE", "DESCRIPTIO", "REFERENCE", "FROMDOC",
                    "RETNUMBER", "INVNUMBER", "HASJOB"],
                optionalFieldBindings: "PO0314,PO0580[14]"
            },

            Requisition: {
                viewID: "PO0345",
                viewOrder: 0,
                initKeyValues: [-999999999999999999, "", -999999999999999999, ""],
                returnFieldNames: ["RQNNUMBER"],
                displayFieldNames: ["RQNNUMBER", "DATE", "EXPARRIVA2", "EXPIRATION", "HASJOB"],
                filterTemplate: "VDCODE = \"{0}\" OR VDCODE = \"\"",
                hidePageNavigation: true
            },

            DuplicateInvoices: {
                viewID: "PO0414",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["INVHSEQ", "INVNUMBER"],
                displayFieldNames: ["INVNUMBER", "VDCODE", "DOCDATE", "DOCTOTAL"],
            },

            Invoices: {
                viewID: "PO0420",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["INVNUMBER", "INVHSEQ"],
                displayFieldNames: ["INVNUMBER", "VDCODE", "VDNAME", "DATE", "DESCRIPTIO", "REFERENCE", "HASJOB"],
            },

            EmailMessage: {
                viewID: "PO0540",
                viewOrder: 0,
                returnFieldNames: ["MSGID", "ACTIVESW", "MSGTYPE"],
                displayFieldNames: ["MSGTYPE", "MSGID", "TEXTDESC", "SUBJECT", "BODY"],
                filterTemplate: "MSGTYPE = \"{0}\" AND ACTIVESW = \"{1}\""
            },

            OptionalFields: {
                viewID: "PO0580",
                viewOrder: 0,
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS", "DEFVAL", "VDESC", "SWSET", "ALLOWNULL", "VALIDATE",
                    "DVIFTEXT", "DVIFMONEY", "DVIFNUM", "DVIFLONG", "DVIFBOOL", "DVIFDATE", "DVIFTIME", "VDESC"],
                displayFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LENGTH", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            },

            Templates: {
                viewID: "PO0605",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["TEMPLATE", "PLATEDESC"],
                displayFieldNames: ["TEMPLATE", "PLATEDESC"]
            },

            PurchaseOrders: {
                viewID: "PO0620",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["PONUMBER"],
                displayFieldNames: ["PONUMBER", "PORTYPE", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                    "ISCOMPLETE", "HASJOB"]
            },

            VendorDocument: {
                viewID: "PO0620",
                viewOrder: 3,
                parentValAsInitKey: false,
                returnFieldNames: ["PONUMBER", "HASJOB"],
                displayFieldNames: ["PONUMBER", "PORTYPE", "DATE", "DESCRIPTIO", "REFERENCE", "ISCOMPLETE", "ORDEREDON"],
                filterTemplate: "(VDCODE = \"{0}\" AND PORTYPE != 2)"
            },

            Receipts: {
                viewID: "PO0700",
                viewOrder: 1, //The fields only can be displayed when key equal 1.
                parentValAsInitKey: true,
                returnFieldNames: ["RCPNUMBER"],
                displayFieldNames: ["RCPNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                    "PONUMBER", "INVNUMBER", "ISCOMPLETE", "HASJOB"]
            },

            ReceiptsNoPM: {
                viewID: "PO0700",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["RCPNUMBER"],
                displayFieldNames: ["RCPNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                    "PONUMBER", "INVNUMBER", "ISCOMPLETE"]
            },

            ReceiptVendor: {
                viewID: "PO0718",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["VDCODE"],
                displayFieldNames: ["VDCODE", "VDNAME", "COSTS", "INVNUMBER", "DOCTOTAL", "CURRENCY"],
                filterTemplate: "RCPHSEQ = \"{0}\""
            },

            Returns: {
                viewID: "PO0731",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["RETNUMBER"],
                displayFieldNames: ["RETNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                    "RCPNUMBER", "PONUMBER", "ISCOMPLETE", "HASJOB"],
            },

            Requisitions: {
                viewID: "PO0760",
                viewOrder: 1,
                parentValAsInitKey: false,
                returnFieldNames: ["RQNNUMBER"],
                displayFieldNames: ["RQNNUMBER", "VDCODE", "VDNAME", "ISPRINTED", "DATE", "DESCRIPTIO", "REFERENCE",
                                    "ISCOMPLETE", "REQUESTBY", "HASJOB"],
            },

            ShipVia: {
                viewID: "PO0900",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CODE", "NAME"],
                displayFieldNames: ["CODE", "NAME"]
            }
        },

        PR: { // Payroll Finders are identical between CP and UP and "~~" will be replaced with CP or UP upon usage
            TimecardsReport: {
                viewID: "~~0031",
                viewOrder: 0,
                displayFieldNames: ["PEREND", "TIMECARD", "TCARDDESC"],
                returnFieldNames: ["TIMECARD"]
            },
            ClassCodes: {
                viewID: "~~0006",
                viewOrder: 0,
                displayFieldNames: ["CLASSCODE", "CLASSDESC"],
                returnFieldNames: ["CLASSCODE", "CLASSDESC"],
                filterTemplate: "CLASS = \"{0}\""
            },
            EarnDeductCodes: {
                viewID: "~~0007",
                viewOrder: 0,
                displayFieldNames: ["EARNDED", "LONGDESC", "CATEGORY"],
                returnFieldNames: ["EARNDED"]
            },
            DistributionCodes: {
                viewID: "~~0009",
                viewOrder: 0,
                displayFieldNames: ["DISTCODE", "DISTRNAME"],
                returnFieldNames: ["DISTCODE"],
                initKeyFieldNames: ["EARNDED", "DISTCODE"],
                filterTemplate: "EARNDED = \"{0}\" "
            },
            EmployeeTax: {
                viewID: "~~0010",
                viewOrder: 0,
                displayFieldNames: ["TAXID", "DESC"],
                returnFieldNames: ["TAXID"],
                initKeyFieldNames: ["EMPLOYEE", "EARNDED"],
                filterTemplate: "EMPLOYEE = \"{0}\" "
            },
            BillingRates: {
                viewID: "~~0041",
                viewOrder: 0,
                displayFieldNames: ["CURRCODE", "CURRDESC", "BILLRATE1"],
                returnFieldNames: ["CURRCODE"],
                initKeyFieldNames: ["EMPLOYEE", "EARNDED", "CURRCODE"],
                filterTemplate: "EARNDED = \"{0}\" "
            },
            EmployeeSelectionList: {
                viewID: "~~0045",
                viewOrder: 0,
                displayFieldNames: ["EMPLISTID", "EMPLISTDSC"],
                returnFieldNames: ["EMPLISTID", "EMPLISTDSC"]
            },
            EmployeeTimecard: {
                viewID: "~~0102",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "ENDDATE", "TCARDDESC", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "ENDDATE"]
            },
            Employee: {
                viewID: "~~0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                parentValAsInitKey: true
            },
            CheckListEmployee: {
                viewID: "~~0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS","CLASS1","CLASS2","CLASS3","CLASS4"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                parentValAsInitKey: true
            },
            Timecard: {
                viewID: "~~0031",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "PEREND", "TIMECARD", "TCARDDESC"],
                returnFieldNames: ["EMPLOYEE", "TIMECARD", "PEREND"]
            },
            TimecardForEmployee: {
                viewID: "~~0031",
                viewOrder: 0,
                displayFieldNames: ["TIMECARD", "PEREND", "TCARDDESC"],
                returnFieldNames: ["EMPLOYEE", "TIMECARD", "PEREND"],
                filterTemplate: "EMPLOYEE = \"{0}\" "
            },
            OptionalFields: {
                viewID: "~~0121",
                viewOrder: 0,
                displayFieldNames: ["OPTFIELD", "FDESC"],
                returnFieldNames: ["OPTFIELD", "FDESC", "TYPE", "LOCATION", "DECIMALS"],
                filterTemplate: "LOCATION = \"{0}\""
            },
            FromToOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFTEXT", "VDESC"],
                displayFieldNames: ["VALIFTEXT", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            TextOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFTEXT", "VDESC"],
                displayFieldNames: ["VALIFTEXT", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            AmountOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFMONEY", "VDESC"],
                displayFieldNames: ["VALIFMONEY", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            NumberOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFNUM", "VDESC"],
                displayFieldNames: ["VALIFNUM", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            IntegerOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFLONG", "VDESC"],
                displayFieldNames: ["VALIFLONG", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            YesNoOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFBOOL", "VDESC"],
                displayFieldNames: ["VALIFBOOL", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            DateOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFDATE", "VDESC"],
                displayFieldNames: ["VALIFDATE", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            TimeOptionalFieldValue: {
                viewID: "CS0012",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VALUE", "TYPE", "VALIFTIME", "VDESC"],
                displayFieldNames: ["VALIFTIME", "VDESC"],
                filterTemplate: "OPTFIELD = \"{0}\""
            },
            WorkClassification: {
                viewID: "~~0027",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WORKCODE", "WORKDESC"],
                displayFieldNames: ["WORKCODE", "WORKDESC"]
            },
            WorkersCompMaster: {
                viewID: "~~0036",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WCCGROUP", "POLICYDESC"],
                displayFieldNames: ["WCCGROUP", "POLICYDESC"]
            },
            WorkersCompCode: {
                viewID: "~~0037",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WCC"],
                displayFieldNames: ["WCC", "DESC"],
                filterTemplate: "WCCGROUP = \"{0}\""
            },
            OvertimeSched: {
                viewID: "~~0022",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["OTSCHED", "OTSDESC"],
                displayFieldNames: ["OTSCHED", "OTSDESC"]
            },
            ShiftDiffSched: {
                viewID: "~~0025",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["SHIFTSCHED", "SHFTSDESC"],
                displayFieldNames: ["SHIFTSCHED", "SHFTSDESC"]
            },
            FederalStateTaxes: {
                viewID: "~~0029",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["TAXID"],
                displayFieldNames: ["TAXID", "TAXTYPE", "LONGDESC"]
            },
            CheckInquiryEmployee: {
                viewID: "~~0048",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "PEREND", "ENTRYSEQ", "ENTRYTYPE", "BANK", "TRANSNUM", "CHECKSTAT", "PRPOSTSTAT", "TRANSAMT"],
                returnFieldNames: ["EMPLOYEE", "PEREND", "ENTRYSEQ"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                parentValAsInitKey: true
            },
            CheckInquiryEnterSequence: {
                viewID: "~~0048",
                viewOrder: 0,
                displayFieldNames: ["ENTRYSEQ", "ENTRYTYPE", "BANK", "CHECKSTAT", "PRPOSTSTAT", "TRANSAMT"],
                returnFieldNames: ["PEREND", "ENTRYSEQ"],
                filterTemplate: "EMPLOYEE = \"{0}\" AND PEREND = \"{1}\"",
                parentValAsInitKey: true
            },
            PeriodEndDate: {
                viewID: "~~0048",
                viewOrder: 0,
                displayFieldNames: ["PEREND", "ENTRYSEQ", "ENTRYTYPE", "BANK", "CHECKSTAT", "PRPOSTSTAT", "TRANSAMT"],
                returnFieldNames: ["PEREND", "ENTRYSEQ"],
                filterTemplate: "EMPLOYEE = \"{0}\"",
                parentValAsInitKey: true
            },
            EmployeeEarningDeduction: {
                viewID: "~~0008",
                viewOrder: 0,
                filterTemplate: "EMPLOYEE = \"{0}\"",
                displayFieldNames: ["EARNDED", "DESC", "CATEGORY"],
                returnFieldNames: ["EARNDED"]
            },
            WorkersCompensationCode: {
                viewID: "~~0037",
                viewOrder: 0,
                filterTemplate: "WCCGROUP = \"{0}\"",
                displayFieldNames: ["WCC", "DESC"],
                returnFieldNames: ["WCC"]
            }
        },

        TS: {
            TaxCode: {
                viewID: "TS0500",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["TAXRCODE"],
                displayFieldNames: ["TAXRCODE", "EFFDATE", "TTYPE", "DESC", "REMARK", "DEPRECATED","REPLACEDBY"],
                filterTemplate: "EFFDATE <=\"{0}\" AND TTYPE = \"{1}\" AND TAXRCODE != SRCA-C AND TAXRCODE != SRRC "
            }
        },

        TX: {
            PMTaxClasses: {
                viewID: "TX0001",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CLASS", "DESC"],
                displayFieldNames: ["CLASS", "DESC"],
                filterTemplate: "AUTHORITY = \"{0}\" AND CLASSTYPE = \"{1}\" AND CLASSAXIS = \"{2}\""
            },

            TaxClasses: {
                viewID: "TX0001",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CLASS", "DESC"],
                displayFieldNames: ["CLASS", "DESC", "EXEMPT"],
                filterTemplate: "AUTHORITY = \"{0}\" AND CLASSTYPE = \"{1}\" AND CLASSAXIS = \"{2}\""
            },

            TaxAuthorities: {
                viewID: "TX0002",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["AUTHORITY", "DESC"],
                displayFieldNames: ["AUTHORITY", "DESC", "SCURN", "MAXTAX", "MINTAX", "TXBASE",
                    "INCLUDABLE", "LIABILITY", "AUDITLEVEL", "RECOVERABL", "RATERECOV",
                    "ACCTRECOV", "EXPSEPARTE", "ACCTEXP", "LASTMAINT"]
            },

            TaxGroups: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["GROUPID", "TTYPE", "DESC", "SRCCURN"],
                displayFieldNames: ["GROUPID", "TTYPE", "DESC", "SRCCURN", "TRATETYPE", "CALCMETHOD", "LASTMAINT"],
                filterTemplate: "TTYPE = \"{0}\""
            },

            TaxGroup: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                returnFieldNames: ["GROUPID", "DESC"],
                displayFieldNames: ["GROUPID", "DESC", "SRCCURN", "AUTHORITY1", "AUTHORITY2",
                    "AUTHORITY3", "AUTHORITY4", "AUTHORITY5", "CALCMETHOD", "LASTMAINT"],
                filter: "TTYPE = \"1\""
            },

            PurchaseTaxGroup: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["GROUPID", "DESC", "SRCCURN"],
                displayFieldNames: ["GROUPID", "TTYPE", "LASTMAINT", "DESC", "SRCCURN", "CALCMETHOD"],
                filter: "TTYPE = 2"
            },

            TaxGroupForPurchaseAndSales: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                returnFieldNames: ["GROUPID", "TTYPE", "DESC", "TRATETYPE", "SRCCURN"],
                displayFieldNames: ["GROUPID", "DESC", "AUTHORITY1", "AUTHORITY2", "AUTHORITY3", "AUTHORITY4", "AUTHORITY5"],
                filterTemplate: "TTYPE = \"{0}\""
            }
        },

        YP: {
            PaymentInfo: {
                viewID: "YP0406",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["IDCUST", "IDCARD"],
                displayFieldNames: ["IDCUST", "IDCARD", "DESC", "PROCESSCOD", "CARDTYPE", "CURRENCY", "SWACTV", "SWDEFCARD",
                                    "NAMEONCARD", "CARDNUMBER", "EXPDATE", "CARDCMNT", "SWAUTOPAY"],
                filterTemplate: "PROCESSCOD = \"{0}\" AND IDCUST = \"{1}\""
            },

            PaymentProcessingCode: {
                viewID: "YP0500",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["PROCESSCOD"],
                displayFieldNames: ["PROCESSCOD", "TEXTDESC", "BANK", "BANKD", "CURRENCY",
                    "CURRENCYD", "MERCHID", "MERCHKEY"],
                filterTemplate: "BANK = \"{0}\" AND CURRENCY = \"{1}\""
            }
        }
    };

})(this.sg = this.sg || {}, this.sg.viewFinderProperties = this.sg.viewFinderProperties || {}, jQuery);
