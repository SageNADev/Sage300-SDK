// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


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
    RECENT_WINDOWS_BASE: 'RecentWindowsStore-'
};
