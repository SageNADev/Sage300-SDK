// Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved.

"use strict";

sg.fiscalCalendar = sg.fiscalCalendar || {};

sg.fiscalCalendar = {
	AccountsPayable: "AP",
	AccountsReceivable: "AR",
	AdministrativeServices: "AS",
	CommonServices: "CS",
	GeneralLedger: "GL",	
	InventoryControl: "IC",
	BankServices: "BK",
	PurchaseOrder: "PO",
    OrderEntry: "OE"
};

sg.utls = sg.utls || {};

sg.utls.InquiryPreferences = {
    FuncInquiryGridPreferenceKey: "2F0CBC17-8F9C-48F9-B121-4612BAC60730",
    CustInquiryGridPreferenceKey: "B472C638-1222-4D44-B5E4-4B77138CB047",
    FuncInquirySecondGridPreferenceKey: "A55E5C6D-FA03-415C-8849-C42048808EF5",
    CustInquirySecondGridPreferenceKey: "778C2CE4-A5A5-436B-B326-3FD42E4D9084"
};

sg.utls.localStorageKeys = {
    RECENT_WINDOWS_BASE: '_RecentlyUsedWindows'
};
