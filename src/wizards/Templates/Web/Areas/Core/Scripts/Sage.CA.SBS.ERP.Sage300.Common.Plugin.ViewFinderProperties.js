/* Copyright (c) 2019-2021 Sage Software, Inc.  All rights reserved. */
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
                returnFieldNames: ["VENDORID", "VENDNAME", "SWACTV"],
                displayFieldNames: ["VENDORID", "SHORTNAME", "VENDNAME", "SWACTV", "SWHOLD", "IDGRP", "CURNCODE", "TEXTSTRE1", "TEXTSTRE2",
                                    "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE", "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2",
                                    "EMAIL2", "NAMECTAC", "CTACPHONE", "CTACFAX", "EMAIL1"]
            },

            Vendor: {
                url: ["AP", "VendorViewFinder", "Find"],
                viewID: "AP0015",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["VENDORID", "VENDNAME", "SHORTNAME", "RATETYPE"],
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
                filterTemplate: "(IDVEND = \"{0}\") AND (TXTTRXTYPE = 1) AND (RTGAPPLYTO = \"\") AND (SWRTGOUT = 1)"
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
            ItemPricing: {
                viewID: "AR0009",
                viewOrder: 0,
                returnFieldNames: ["UNITMEAS"],
                displayFieldNames: ["IDITEM", "CODECURN", "UNITMEAS", "AMTPRICE", "AMTBASETAX"],
                filterTemplate: "IDITEM = \"{0}\" AND CODECURN = \"{1}\""
            },

            Items: {
                viewID: "AR0010",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDITEM"],
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
                returnFieldNames: ["IDACCTSET"],
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
                returnFieldNames: ["IDDIST"],
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

            ShortenedCustomers: {
                url: ["AR", "CustomerViewFinder", "Find"],
                viewID: "AR0024",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["IDCUST"],
                displayFieldNames: ["IDCUST", "TEXTSNAM", "NAMECUST", "SWACTV", "SWHOLD", "IDGRP", "IDNATACCT", "SWBALFWD", "CODECURN",
                    "TEXTSTRE1", "TEXTSTRE2", "TEXTSTRE3", "TEXTSTRE4", "NAMECITY", "CODESTTE",
                    "CODEPSTL", "CODECTRY", "TEXTPHON1", "TEXTPHON2", "NAMECTAC", "PRICLIST"]
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
                parentValAsInitKey: false,
                returnFieldNames: ["CNTITEM"],
                displayFieldNames: ["CNTITEM", "DOCNBR", "IDRMIT", "IDCUST", "DATERMIT",
                    "TEXTRMIT", "TEXTPAYOR", "AMTRMIT"],
                filterTemplate: "CNTBTCH = \"{0}\" AND  CODEPYMTYP = \"{1}\" "
            },

            CommentTypes: {
                viewID: "AR0094",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CMNTTYPE"],
                displayFieldNames: ["CMNTTYPE", "TEXTDESC", "ACTVSW", "ACTVSW"]
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
            Users: {
                viewID: "AS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                calculatePageCount: false,
                returnFieldNames: ["USERID"],
                displayFieldNames: ["USERID", "USERNAME", "EMAIL1", "LANGUAGE"],
                filter: null,
                initKeyValues: []
            },

            GLAccountUsers: {
                viewID: "AS0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["USERID"],
                displayFieldNames: ["USERID", "USERNAME", "ACCTSTATUS", "ACCTTYPE"],
                filter: "USERID != \"ADMIN\" AND ACCTTYPE = 0"
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

            BankCurrency: {
                viewID: "BK0002",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["CURN"],
                displayFieldNames: ["CURN", "RTYPCHK", "RTYPDEP", "GAINACCT", "LOSSACCT"],
                filterTemplate: "BANK = \"{0}\""
            },

            CheckStocks: {
                viewID: "BK0008",
                viewOrder: 0,
                returnFieldNames: ["FORMID"],
                displayFieldNames: ["BANK", "FORMID", "DESC", "STKTYPE", "FORMSPEC1", "FORMSPEC2", "ADVICE", "LANGUAGE"],
                //StockType CheckThenAdvice is not supported in web
                filterTemplate: "BANK = \"{0}\" AND STKTYPE != 2"
            },

            BankEntry: {
                viewID: "BK0450",
                viewOrder: 1,
                parentValAsInitKey: true,
                returnFieldNames: ["ENTRYNBR"],
                displayFieldNames: ["ENTRYNBR", "REFERENCE", "BANK", "TOTSRCEGRO", "TOTFUNCGRO", "STATUS", "POSTDATE", "TRANSTYPE"]
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
                returnFieldNames: ["FSCYEAR"],
                displayFieldNames: ["FSCYEAR", "PERIODS", "QTR4PERD", "ACTIVE", "BGNDATE1", "BGNDATE2", "BGNDATE3",
                    "BGNDATE4", "BGNDATE5", "BGNDATE6", "BGNDATE7", "BGNDATE8", "BGNDATE9",
                    "BGNDATE10", "BGNDATE11", "BGNDATE12", "BGNDATE13",
                    "ENDDATE1", "ENDDATE2", "ENDDATE3", "ENDDATE4", "ENDDATE5", "ENDDATE6",
                    "ENDDATE7", "ENDDATE8", "ENDDATE9", "ENDDATE10", "ENDDATE11", "ENDDATE12", "ENDDATE13"],
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
                viewOrder: 1,
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
                returnFieldNames: ["ACCTFMTTD", "ACCTDESC", "MCSW", "ACCTID", "ACTIVESW"],
                displayFieldNames: ["ACCTID", "ACCTFMTTD", "ACCTDESC", "ACTIVESW", "ACCTTYPE", "ABRKID", "CTRLACCTSW", "ALLOCSW",
                                    "MCSW", "QTYSW", "UOM"]
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


            SegmentCodes: {
                viewID: "GL0021",
                viewOrder: 0,
                returnFieldNames: ["SEGVAL"],
                displayFieldNames: ["SEGVAL", "SEGVALDESC"],
                filterTemplate: "IDSEG = \"{0}\""
            },

            Users: {
                viewID: "GL0054",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["USER"],
                displayFieldNames: ["USER", "NAME"]
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
            AdjustmentHeader: {
                viewID: "IC0120",
                viewOrder: 3,
                returnFieldNames: ["DOCNUM"],
                displayFieldNames: ["DOCNUM", "HDRDESC", "TRANSDATE", "FISCYEAR", "FISCPERIOD", "REFERENCE", "STATUS", "TRANSNUM"],
                filter: "DELETED = 0" 
            },

            BOMNumber: {
                viewID: "IC0200",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["BOMNO"],
                displayFieldNames: ["FMTITEMNO", "BOMNO", "ITEMDESC", "DESC", "REMARK", "FIXEDCOST", "BUILDQTY", "UNIT",
                    "VARBLCOST", "STARTDATE", "ENDDATE", "INACTIVE"],
                filterTemplate: "FMTITEMNO = \"{0}\" "
            },

            Category: {
                viewID: "IC0210",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["CATEGORY"],
                displayFieldNames: ["CATEGORY", "DESC", "INACTIVE", "COMMSNPAID"]
            },

            ReceiptCost: {
                viewID: "IC0260",
                viewOrder: 0,
                returnFieldNames: ["RECEIPTNUM"],
                displayFieldNames: ["RECEIPTNUM", "TRANSDATE", "QTY", "COST", "SHIPQTY",
                    "SHIPCOST"],
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
                returnFieldNames: ["MANITEMNO", "ITEMNO", "FMTITEMNO", "ITEMDESC"],
                displayFieldNames: ["MANITEMNO", "FMTITEMNO", "UNIT", "ITEMDESC"],
                filterTemplate: "MANITEMNO = \"{0}\""
            },

            Item: {
                viewID: "IC0310",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["FMTITEMNO", "ITEMNO"],
                displayFieldNames: ["FMTITEMNO", "DESC", "INACTIVE", "ITEMBRKID", "CATEGORY", "CNTLACCT", "STOCKITEM", "STOCKUNIT",
                    "PICKINGSEQ", "DEFPRICLST", "SELLABLE", "SERIALNO", "LOTITEM", "QTONHANDA", "QTONORDERA",
                    "QTSALORDRA", "QTAVAILA", "QTYCOMMITA", "PREVENDOR", "VENDITEM", "ITEMNO"]
                //optionalFieldBindings: "IC0313,IC0377[0]"
            },

            Items: {
                viewID: "IC0310",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["FMTITEMNO", "ITEMNO", "COSTMETHOD", "STOCKITEM", "INACTIVE"],
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
                returnFieldNames: ["KITNO"],
                displayFieldNames: ["KITNO", "FMTITEMNO", "ITEMDESC", "DESC", "REMARK", "BUILDQTY", "UNIT"],
                filterTemplate: "ITEMNO = \"{0}\" "
            },

            Location: {
                viewID: "IC0370",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["LOCATION", "DESC"],
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
                returnFieldNames: ["PRICELIST", "DESC"],
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
                filter: "DELETED = 0",
                //viewFinder.optionalFieldBindings = "IC0595, IC0377[2]";  // comment out for now as CSFND doesn't support filterCount yet
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

            ItemUnitOfMeasure: {
                viewID: "IC0750",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["UNIT"],
                displayFieldNames: ["UNIT", "CONVERSION"],
                filterTemplate: "ITEMNO=\"{0}\""
            },

            WeightUnitOfMeasure: {
                viewID: "IC0758",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["WEIGHTUNIT", "WUOMDESC"],
                displayFieldNames: ["WEIGHTUNIT", "WUOMDESC", "CONVERSION", "DEFAULT"]
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
                returnFieldNames: ["MISCCHARGE"],
                displayFieldNames: ["MISCCHARGE", "DESC", "HASJOB", "MISCACCT", "AMOUNT", "MISCACDESC"],
                filterTemplate: "CURRENCY = \"{0}\""
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
                    "SHPFAX", "SHPCONTACT", "SHIPVIA", "ORDPAYMENT", "INVNETWTX", "SALES1NAME", "SALES2NAME",
                    "SALES3NAME", "SALES4NAME", "SALES5NAME", "HASPREAUTH"],
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
            Labor: {
                viewID: "PM0002",
                viewOrder: 0,
                displayFieldNames: ["STAFFCODE", "NAME", "INACTIVE", "EARNCODE", "GROUP"],
                returnFieldNames: ["STAFFCODE"]
            },

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

            Miscellaneous: {
                viewID: "PM0028",
                viewOrder: 0,
                displayFieldNames: ["MISCCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["MISCCODE"]
            },

            Overhead: {
                viewID: "PM0029",
                viewOrder: 0,
                displayFieldNames: ["OHCODE", "DESC", "INACTIVE"],
                returnFieldNames: ["OHCODE"]
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
                displayFieldNames: ["RQNNUMBER", "DATE", "EXPARRIVA2", "EXPIRATION"],
                filterTemplate: "VDCODE = \"{0}\" OR VDCODE = \"\""
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

        PR: { // Payroll
            CAEmployee: {
                viewID: "CP0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                parentValAsInitKey: true,
            },
            CAEmployeeTimecard: {
                viewID: "CP0102",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "ENDDATE", "TCARDDESC", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "ENDDATE"],
            },
            USEmployee: {
                viewID: "UP0014",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "LASTNAME", "FIRSTNAME", "MIDDLENAME", "PAYFREQ", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "FULLNAME"],
                filterTemplate: "TCUSERID = \"{0}\" ",
                parentValAsInitKey: true
            },
            USEmployeeTimecard: {
                viewID: "UP0102",
                viewOrder: 0,
                displayFieldNames: ["EMPLOYEE", "ENDDATE", "TCARDDESC", "STATUS"],
                returnFieldNames: ["EMPLOYEE", "ENDDATE"],
            }
        },

        TX: {
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
                returnFieldNames: ["AUTHORITY"],
                displayFieldNames: ["AUTHORITY", "DESC", "SCURN", "MAXTAX", "MINTAX", "TXBASE",
                    "INCLUDABLE", "LIABILITY", "AUDITLEVEL", "RECOVERABL", "RATERECOV",
                    "ACCTRECOV", "EXPSEPARTE", "ACCTEXP", "LASTMAINT"],
            },

            TaxGroups: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["GROUPID", "DESC", "SRCCURN"],
                displayFieldNames: ["GROUPID", "TTYPE", "DESC", "SRCCURN", "TRATETYPE", "CALCMETHOD", "LASTMAINT"],
                filterTemplate: "TTYPE = \"{0}\""
            },

            TaxGroup: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                parentValAsInitKey: false,
                returnFieldNames: ["GROUPID"],
                displayFieldNames: ["GROUPID", "DESC", "SRCCURN", "AUTHORITY1", "AUTHORITY2",
                    "AUTHORITY3", "AUTHORITY4", "AUTHORITY5", "CALCMETHOD", "LASTMAINT"],
                filter: "TTYPE = \"1\""
            },

            PurchaseTaxGroup: {
                url: ["CS", "TaxGroupViewFinder", "Find"],
                viewID: "TX0003",
                viewOrder: 0,
                parentValAsInitKey: true,
                returnFieldNames: ["GROUPID", "DESC"],
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
