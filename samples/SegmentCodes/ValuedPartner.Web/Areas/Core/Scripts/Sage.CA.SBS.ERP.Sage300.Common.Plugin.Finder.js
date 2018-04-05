/* Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved. */
"use strict";
(function (sg, $) {
    sg.finderHelper = {
        pageSize: 5,
        sortDir: false,
        cancelFuncCall: $.noop,
        setFinder: function (id, searchFinder, onSelectCallBack, onCancelCallBack, title, filters, uid, postbackNotRequired, height, top) {
            $("#" + id).Finder({
                searchFinder: searchFinder,
                sortDir: false,
                select: onSelectCallBack,
                cancel: onCancelCallBack,
                title: title,
                id: uid || id,
                filters: filters,
                postbackNotRequired: postbackNotRequired,
                height: height,
                top: top
            });
        },

        createFilter: function (field, operator, value, applyFilterIfNull) {
            if (applyFilterIfNull == null || applyFilterIfNull == undefined) {
                applyFilterIfNull = false;
            }
            return { Field: { field: field }, Value: value, Operator: operator, ApplyFilterIfNull: applyFilterIfNull };
        },

        createInquiryFilter: function (field, operator, value, applyFilterIfNull, isAndOperation, logisticGroup) {
            if (applyFilterIfNull == null || applyFilterIfNull == undefined) {
                applyFilterIfNull = false;
            }
            return { Field: { field: field }, Value: value, SqlOperator: operator, ApplyFilterIfNull: applyFilterIfNull };
        },
        createDefaultFunction: function (fieldControl, field, operator) {
            var func = function () {
                if (operator == undefined || operator.length == 0) {
                    operator = sg.finderOperator.StartsWith;
                }
                var filterData = [[]];
                var value = $("#" + fieldControl).val();
                filterData[0][0] = { Field: { field: field }, Operator: operator, Value: value };
                return filterData;
            };
            return func;
        },
    };

    sg.findEvent = null;
    sg.filterData = [];
    sg.mandatoryFilterData = [];

    /* Required for  Preferences */
    sg.isPreferencesPostback = false;
    sg.finderOptions = null;
    sg.filterType = null;
    /*--------------------------*/

    sg.keys = null;
    sg.FinderDisplayType = {
        All: 0,
        Filter: 1,
        Grid: 2
    };

    sg.finderDataType = {
        Amount: "amount",
        Integer: "int",
        Number: "number",
        Date: "date",
        Time: "time",
        Text: "text",
        SmallInteger: "smallint",
        Boolean: "bool",
        Decimal: "decimal"
    };
    sg.finderOperator = {
        GreaterThan: 0,
        GreaterThanOrEqual: 1,
        LessThan: 2,
        LessThanOrEqual: 3,
        NotEqual: 4,
        Equal: 5,
        Like: 6,
        StartsWith: 7,
        Contains: 8
    };
    //Not to be used
    sg.delayVariables = {
        IsInProgress: false,
        RowData: [],
        ColumnName: "",
        TextData: "",
        TextBoxElement: ""
    };
    sg.delayOnBlur = function (elementId, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }
            var isFound = false;
            if (elementId.constructor === Array) {
                isFound = $.inArray(elementInfocusAttrId, elementId) == -1;
            } else {
                isFound = elementInfocusAttrId != elementId;
            }
            if (elementInfocus == null || elementInfocusAttrId == null || isFound) {
                funcCall();
            } else {
                sg.finderHelper.cancelFuncCall = funcCall;
            }
        });
    };
    sg.delayOnChange = function (elementId, txtElementId, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }
            var isFound = false;
            if (elementId.constructor === Array) {
                isFound = $.inArray(elementInfocusAttrId, elementId) == -1;
            } else {
                isFound = elementInfocusAttrId != elementId;
            }
            if (elementInfocus == null || elementInfocusAttrId == null || isFound) {
                funcCall();
            }
            else {
                sg.delayVariables.IsInProgress = true;
                sg.delayVariables.TextBoxElement = txtElementId;
                sg.delayVariables.TextData = sg.delayVariables.TextBoxElement.text();
                sg.delayVariables.RowData = [];
                sg.delayVariables.ColumnName = "";
                sg.finderHelper.cancelFuncCall = funcCall;
            }
        });
    };
    sg.delayOnDataChange = function (elementId, rowData, columnName, funcCall) {
        var elementInfocus;
        setTimeout(function () {
            elementInfocus = $(document.activeElement);
            var elementInfocusAttrId = elementInfocus.attr("id");

            if (sg.utls.isSafari()) {
                elementInfocus = sg.findEvent;
                if (elementInfocus != null) {
                    elementInfocusAttrId = elementInfocus.id;
                } else {
                    elementInfocusAttrId = null;
                }
                sg.findEvent = null;
            }

            if (elementInfocus == null || elementInfocusAttrId == null || elementInfocusAttrId != elementId) {
                funcCall();
            } else {
                sg.delayVariables.IsInProgress = true;
                sg.delayVariables.RowData = rowData;
                sg.delayVariables.ColumnName = columnName;
                sg.delayVariables.TextBoxElement = "";
                sg.delayVariables.TextData = "";
                sg.finderHelper.cancelFuncCall = funcCall;
            }
        });

    };
    sg.finder = {
        SourceCode: "sourcecode",
        JournalBatch: "journalbatch",
        JournalEntry: "journalentry",
        JournalEntryNumber: "journalentrynumber",
        AccountGroup: "accountgroup",
        AccountType: "accounttype",
        AccountStructure: "accountstructure",
        CurrencyCode: "currencycode",
        RecurringEntry: "recurringentry",
        SourceJournalProfile: "sourcejournalprofile",
        ScheduleCode: "schedule",
        SegmentCode: "segmentcode",
        RateType: "ratetype",
        AccountReport: "accountreport",
        SegmentCodeReport: "segmentcodereport",
        AccountOptionalFinder: "accountoptionalfinder",
        CsOptionalFieldValue: "csoptionalfieldvalue",
        AccountValidCurrency: "accountspecificcurrency",
        CurrencyRate: "currencyrate",
        OptionalFieldValue: "csoptionalfieldvalue",
        RevaluationCode: "revaluationcode",
        TransOptionalFields: "transoptionalfields",
        Csoptionalfields: "csoptionalfields",
        AccountRollupOptionalFinder: "accountrollupoptionalfinder",
        TransOptionalFinder: "cstransoptionalfield",
        AccountGrpSortCode: "accountgroupsortcode",
        DistributionCode: "distributioncode",
        PaymentCode: "paymentcode",
        AccountDistribution: "accountdistribution",
        CommentTypes: "commenttypes",
        BillingCycle: "billingcycle",
        SecurityGroup: "securitygroup",
        User: "user",
        WeightUnitOfMeasure: "weightunitsofmeasure",
        Banks: "banks",
        ReceiptForlabel: "receiptforlabel",
        BKOptionsAccountFinder: "bkoptionsaccountfinder",
        BankDistributionCode: "bankdisitributioncode",
        TaxAuthorities: "taxauthority",
        TaxClasses: "taxclasses",
        BankDistributionSetCode: "bankdisitributioncode",
        CurrencyRateType: "currencyratetype",
        BankEntry: "bankentry",
        FiscalYear: "fiscalyear",
        AccountSet: "accountset",
        ItemStructure: "itemstructure",
        PriceListCode: "pricelistcode",
        EmailMessage: "emailmessage",
        APEmailMessage: "apemailmessage",
        APRecurringPayable: "aprecurringpayable",
        ARRecurringCharge: "arrecurringcharge",
        TaxGroup: "taxgroup",
        BankOptionsDistCodes: "bankoptionsdistCodes",
        InvoiceBatch: "invoicebatch",
        InvoiceBatchListFinder: "invoicebatchlistfinder",
        APDistributionCode: "apdistributioncode",
        PaymentCodes: "paymentCodes",
        PaymentInfo: "paymentinfo",
        CPRSCode: "cprscode",
        ARAccountSet: "araccountset",
        APAccountSet: "apaccountset",
        ApGLAccount: "apglaccount",
        APInvoiceBatch: "apinvoicebatch",
        APInvoiceEntry: "apinvoiceentry",
        BankDistributionSet: "bankdistributionset",

        PostingJournalFinder: "postingjournal",
        DunningMessage: "dunningmessage",
        ARInvoiceBatchFinder: "arinvoicebatch",
        ARReceiptBatchFinder: "arreceiptbatch",
        ARAdjustmentFinder: "aradjustmentbatch",
        PaymentBatchFinder: "paymentbatch",
        AdjustmentBatchFinder: "adjustmentbatch",
        APRetainageSchedule: "apretainageschedule",
        APDistributionSet: "apdistributionset",
        APVendorGroupCode: "apgroupcode",
        ARRefundBatchFinder: "arrefundbatch",
        Vendor: "vendor",
        FromBankCodeFinder: "frombankcodefinder",
        ToBankCodeFinder: "tobankcodefinder",
        BankEntryHeaderFinder: "bankentryheaderfinder",
        VendorNumber: "vendornumber",
        GroupCode: "apgroupcode",
        APTerms: "apterms",
        TermsCode: "termscode",
        InterestProfiles: "interestProfiles",
        BankPostingJournalFinder: "bankpostingjournalfinder",
        BankCurrencyCode: 'bankcurrencycode',
        Category: "category",
        IcSegmentCode: "icsegmentcode",
        Location: "location",
        SelectionCriteriaFinder: "selectioncriteria",
        AROptionalField: "aroptionalfield",
        PostedPayment: "postedpayment",
        Items: "items",
        CheckStock: "checkstock",
        CurrenctTable: "currenytable",
        ARItemNumberFinder: "aritem",
        ARItemNumberPricingFinder: "aritempricing",
        SalesPersonFy: "salespersonfy",
        SalesPersonFinder: "salesperson",
        ARPostingJournalFinder: "arpostingjournalfinder",
        Documents: "documents",
        DocumentSchedPayments: "documentschedpayments",
        ARCustomerFinder: "arcustomerfinder",
        CustomerGroup: "customergroup",
        PaymentAdjBatchFinder: "paymentadjustmentbatch",
        PaymentAdjustmentFinder: "paymentadjustment",
        RemitToLocationFinder: "remittolocation",
        ReceiptAdjustmentHeaderFinder: "receiptadjustmentheader",
        AgedPayableVendorNumber: "agedpayablevendornumber",
        AgedPayableVendorGroup: "agedpayablevendorgroup",
        AgedPayableAccountSets: "agedpayableaccountsets",
        AgedPayableTermsCode: "agedpayabletermscode",
        AgedReceivableCustomerNumber: "agedreceivablecustomernumber",
        AgedReceivableCustomerGroup: "agedreceivablecustomergroup",
        AgedReceivableAccountSets: "agedreceivableaccountsets",
        AgedReceivableSalespersons: "agedreceivablesalespersons",
        AgedReceivableTermsCodes: "agedreceivabletermscodes",
        AgedReceivableNationalAccounts: "agedreceivablenationalaccounts",
        AccountBalanceFinder: "accountbalancefinder",
        ARInvoiceFinder: "arinvoicefinder",
        ARShipToLocationFinder: "arshiptolocationfinder",
        APOptionalFields: "apoptionalfields",
        BankTransactionDetailFinder: 'banktransactiondetailfinder',
        NationalAccounts: "nationalaccounts",
        TaxCurrencyFinder: "taxcurrency",
        CsglAccountFinder: "csglaccount",
        ARInvoiceEntryFinder: "invoiceentryentrynumber",
        AROptionalFields: "aroptionalfields",
        AROptionalFieldsFinder: "aroptionalfieldsfinder",
        CSPaymentProcessingCode: "paymentprocessingcodefinder",
        VendorGroup: "vendorGroup",
        ContractPricing: "contractpricing",
        UnitOfMeasure: "unitsofmeasure",
        BankTransactionHeader: "banktransactionheader",
        ReprintStatement: "reprintstatementheader",
        ReprintCustomer: "reprintstatementcustomer",
        ReprintNATCustomer: "reprintstatementnatcustomer",
        OpenDocumentDetails: "opendocumentdetails",
        ARDocumentSchedulePayment: "ardocumentschedulepayment",
        ARStartingDocumentNumber: "arstartingdocumentnumber",
        ARStartingPoNumber: "arstartingponumber",
        ARStartingDueDate: "arstartingduedate",
        ARStartingOrderNumberFinder: "arstartingordernumber",
        ARStartingShipmentNumberFinder: "arstartingshipmentnumber",
        ARStartingDocumentDateFinder: "arstartingdocumentdate",
        ARStartingCurrentBalanceFinder: "arstartingcurrentbalance",
        ARStartingOriginalDocumentNumberFinder: "arstartingoriginaldocumentnumber",
        ARDocumentFinder: "ardocument",
        ICOptionalFields: "icoptionalfields",
        AROptFieldsFinder: "aroptfieldsfinder",
        OEFinder: "orderentryfinder",
        OEShipmentFinder: "oeshipment",
        AOCustomerFinder: "agedordercustomerfinder",
        VendorAcitivtyDocument: "vendoractivitydocument",
        BankTransactionInquiryFinder: "banktransactioninquiryfinder",
        OEFiscalYear: "oefiscalyearfinder",
        ICItemUnitOfMeasure: "icitemunitofmeasure",
        ICItemPricing: "icitempricing",
        ShipmentNumberFinder: "shipmentfinder",
        CreditDebitNoteFinder: "creditdebitnotefinder",
        OpenDocumentDetail: "opendocumentdetail",
        ARPrePaymentDocumentNumber: "arprepaymentdocumentnumber",
        ARPrePaymentPoNumber: "arprepaymentponumber",
        OEInvoiceEntry: "invoicentryfinder",
        ARPrePaymentOrderNumber: "arprepaymentordernumber",
        ARPrePaymentShipmentNumber: "arprepaymentshipmentnumber",
        POShipViaCode: "poshipviacode",
        APOpenDocumentDetails: "apopendocumentdetails",
        ReceiptNumberFinder: "receiptfinder",
        InternalUsageFinder: "internalusagefinder",
        AssembliesFinder: "assembliesfinder",
        ICInventoryWorksheetFinder: "icinventoryworksheetfinder",
        TemplateFinder: "template",
        ICKittingItemFinder: "ickittingitemfinder",
        OEOptionalField: "oeoptionalfield",
        OETemplate: "oetemplate",
        OEShipViaCode: "oeshipviacode",
        POReceiptFinder: "poreceiptfinder",
        MiscellaneousChargeFinder: "miscellaneouschargefinder",
        Requisition: "requisition",
        OECustomerShipViaCode: "oecustomershipviacode",
        OECustomerAccountSet: "oecustomeraccountset",
        OETaxGroup: "oetaxgroup",
        OptionalFieldsFinder: "optionalfieldsfinder",
        OEItemPricingFinder: "oeitempricingfinder",
        ICManufacturerItemNumber: "icmanufactureritemfinder",
        POEmailMessage: "poemailmessage",
        PurchaseOrder: "purchaseorder",
        VendorItemNumber: "vendoritemnumber",
        VendorContractCost: "vendorcontractcost",
        TransferDocumentNumber: "icdocumentnumberfinder",
        UnitCost: "unitcost",
        ReceiptVendor: "receiptvendor",
        ReturnEntry: "returnentry",
        AdditionalCost: "additionalcost",
        POOptionalField: "pooptionalfield",
        ICAdjustmentHeader: "icadjustmentheaderfinder",
        ItemNumber: "itemnumber",//used for PO - Setup- Vendor Contract Cost 
        OEEmailMessageFinder: "oeemailmessage",
        InventoryCountWorksheetFinder: "invcountworksheetfinder",
        POCreditDebitNoteFinder: "pocreditdebitnotefinder",
        PODuplicateInvoice: "poduplicateinvoice",
        //This finder is only for Invoice Entry.Please use the above value if you want to open duplicate Invoice
        POInvoiceEntryDuplicate: "poinvoiceentryduplicate",
        POInvoiceNumber: "poinvoicenumber",
        LocationQuantity: "locationquantity",
        ReorderQuantities: "reorderquantities",
        ICReceiptCost: "icreceiptcost",
        RequisitionDetail: "porequisition",
        PODuplicateCreditDebitNote: "duplicatecreditdebitnote",
        ICInventoryLotNumber: "inventorylotnumber",
        ICContractCode: "contractcode",
        ICWarrantyCode: "warrantycode",
        ICInventorySerialNumber: "inventoryserialnumber",
        BillsOfMaterial: "billsofmaterial",
        VendorTaxClasses: "vendortaxclasses", //used for PO where TaxClass to show ClassType = Vendors instead of Customers
        RecallReleseHeaders: "recallReleseHeaders",
        Payment: "payment",
        PrePaymentCheckNumber: "arprepaymentchecknumber",
        Label: "label",
        CustomerDetailFinder: "customerdetailfinder",
        SerialNumberHistory: "serialnumberhistory",
        ARPostedReceiptFinder: "arpostedreceiptfinder",
        ICReconciliationHeader: "reconciliationheader",
        ARRefundEntry: "arrefundentryfinder",
        PreviewAccountFinder: "previewaccountfinder",
        ICBillsOfMaterialComponent: "billsofmaterialcomponent",
        CreditCardType: "bankcreditcardtypefinder",
        GLAccountPermissionFinder: "glaccountpermissionfinder",
        UICustomizationFinder: "asuicustomization"
    };

    /* Add all the finders that doesn't require page navigation ie. First Page, Last Page buttons, shoud have the finder name in the below array 
     this string should match the sg.finder - finder name
    */
    $.extend(sg.finder, {
        hidePageNavigationFinderList: ["locationquantity", "porequisition", sg.finder.UnitCost, "oeitempricingfinder", sg.finder.PODuplicateCreditDebitNote]
    });

}(this.sg = this.sg || {}, jQuery));

(function ($, window, document, undefined) {
    var kendoWindow;
    $.widget("sageuiwidgets.Finder", {
        divFinderDialogId: '',
        options: {
            searchFinder: null,
            pageNumber: 0, // 0 based index
            pageSize: sg.finderHelper.pageSize,
            sortDir: sg.finderHelper.sortDir,
            select: $.noop,
            cancel: $.noop,
            title: "",
            id: "",
            filters: $.noop,
            postbackNotRequired: false
        },
        _create: function () {
            var that = this;
            $(that.element).bind('click', function () {

                if (!sg.utls.isFinderClicked) {
                    sg.utls.isFinderClicked = true;
                    sg.isPreferencesPostback = false;
                    that._doAjax(that);
                }
            });

            var isSafari = navigator.userAgent.indexOf("Safari") > -1;

            if (isSafari) {
                $(that.element).bind('mousedown', function () {

                    if (!sg.utls.isFinderClicked) {
                        sg.findEvent = event.currentTarget;
                    }
                });
            }
        },
        _doAjax: function (that) {
            var filterData = [[]];
            var mandatoryFilterAndData = [];
            var mandatoryFilterData = [];
            var andCounter = 0;
            var orCounter = 0;
            var item;

            if (that.options.filters != $.noop) {
                filterData = that.options.filters.call();
                if (filterData != null && filterData.length > 0) {
                    for (var orFilter in filterData) {
                        andCounter = 0;
                        mandatoryFilterAndData = [];
                        for (var andFilter in filterData[orFilter]) {
                            item = filterData[orFilter][andFilter];
                            if (item.IsMandatory != undefined && item.IsMandatory == true) {
                                mandatoryFilterAndData[andCounter] = item;
                                andCounter = andCounter + 1;
                            }
                        }
                        if (andCounter > 0) {
                            mandatoryFilterData[orCounter] = mandatoryFilterAndData;
                            orCounter = orCounter + 1;
                        }
                    }
                }
            }
            sg.filterData = filterData;
            sg.mandatoryFilterData = orCounter > 0 ? mandatoryFilterData : [];
            var finderOptions = {
                SearchFinder: that.options.searchFinder,
                PageNumber: that.options.pageNumber,
                PageSize: that.options.pageSize,
                SortAsc: that.options.sortDir,
                AdvancedFilter: filterData
            };

            //Required when post back happens when user click on Apply or Restore Table Default
            var data = { finderOptions: finderOptions };
            sg.finderOptions = finderOptions;

            that.divFinderDialogId = 'div_' + that.options.searchFinder + '_dialog';
            $('<div id="' + that.divFinderDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divFinderDialogId;

            var finderWidth = 820;
            var activeWidgetConfigIframe = $(window.top.$('iframe[id^="iframeWidgetConfiguration"]:visible'));
            var finderLeftPos = 0;
            if (!activeWidgetConfigIframe.is(':visible')) {
                finderLeftPos = (window.innerWidth - finderWidth) / 2;
            }
            else {
                finderLeftPos = (activeWidgetConfigIframe.parents('.k-widget.k-window').width() - finderWidth) / 2;
            }

            // determine whether page navigation should get hidden/disabled.
            var hidePageNavigation = FinderGridHelper.HideFinderPageNavigation(that.options.searchFinder);

            var finderHeight = 552;
            if (that.options.height !== undefined && that.options.height !== null && typeof that.options.height === 'number') {
                finderHeight = that.options.height;
            }

            var top = that.options.top;

            kendoWindow = $(dialogId).html("<span class='sage_loading'></span>").kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: true,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: finderWidth,
                height: finderHeight,
                activate: sg.utls.kndoUI.onActivate,
                //Open Kendo Window in center of the Viewport
                open: function () {
                    var windowHeight = $(window.top).scrollTop() - window.top.sg.utls.portalHeight;
                    var finderTopPos = (($(window.top).height() - kendoWindow.options.height) / 2) + windowHeight;
                    if (finderTopPos < 0) {
                        finderTopPos = 0;
                    }

                    if (top) {
                        finderTopPos = top;
                    }

                    this.wrapper.css({ top: finderTopPos });
                    this.wrapper.css({ left: finderLeftPos });

                    // hide page navigation when option is set...
                    if (hidePageNavigation) {
                        sg.utls.kndoUI.hidePageNavigation(dialogId);
                    }
                },
            }).data("kendoWindow");

            //Close Event -Do same as cancel
            kendoWindow.bind("close", function () {
                that._triggerChange(that);
                kendoWindow.destroy();
                var cancel = that.options.cancel;
                if (cancel) {
                    cancel();
                }
                sg.utls.isFinderClicked = false;
                sg.findEvent = null;
            });

            $(dialogId).parent().addClass("finder-window");
            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "Find", "Find"), data, function (successData) {
                that._showFinderScreen(that, successData, dialogId);
            }, function (jqXhr, textStatus, errorThrown) {
                sg.utls.isFinderClicked = false;
                sg.utls.ajaxErrorHandler(jqXhr, textStatus, errorThrown);
            });
        },
        _showFinderScreen: function (that, data, dialogId) {
            if (data) {
                $(dialogId).html(data);
                FinderGridHelper.init();
                FinderPreferences.Initialize();
                var $titleSpan = kendoWindow.wrapper.find('.k-window-title');
                $titleSpan.html(that.options.title);
                kendoWindow.open();

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefApply",
                        function() {
                            sg.isPreferencesPostback = true;
                            that._reload(that, false);
                        });

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefRestore",
                        function() {
                            sg.isPreferencesPostback = true;
                            that._reload(that, true);
                        });

                $(document)
                    .on('click.plugin.finderPref',
                        "#btnFinderPrefEditCols",
                        function() {
                            var prefHtml = $("#tblTBodyFinderPref").html();
                            if (prefHtml !== "") {
                                FinderPreferences.ShowFieldsWindow();
                            } else {
                                var data = { searchFinder: that.options.searchFinder };
                                window.sg.utls.ajaxPostHtmlSync(window.sg.utls.url
                                    .buildUrl("Core", "Find", "GetEditableColumns"),
                                    data,
                                    function(successData) {
                                        $("#tblTBodyFinderPref").html(successData);
                                        FinderPreferences.FinderPreferencesHTML = $("#tblTBodyFinderPref").html();
                                        FinderPreferences.ShowFieldsWindow();
                                    });
                            }
                        });

                $("#select")
                    .on('click',
                        function() {
                            sg.finderHelper.cancelFuncCall = $.noop;
                            sg.delayVariables.IsInProgress = false;
                            that._getSelectedRow(that);
                        });

                $("#cancel")
                    .on('click',
                        function() {
                            that._triggerChange(that);
                            var cancel = that.options.cancel;
                            if (cancel) {
                                $(this).on('click', cancel());
                            }
                            var finderWin = $("#" + that.divFinderDialogId).data("kendoWindow");
                            finderWin.destroy();
                            sg.utls.isFinderClicked = false;
                            sg.findEvent = null;

                        });
                $("#div_finder_grid .k-grid-content")
                    .delegate("tbody>tr",
                        "dblclick",
                        function() {
                            sg.finderHelper.cancelFuncCall = $.noop;
                            that._getSelectedRow(that);
                        });
            } else {
                kendoWindow.destroy()
                sg.utls.isFinderClicked = false;
            }
        },

        _reload: function (that, deleteUserPreference) {
            var options = sg.finderOptions;
            options.PageNumber = 0;
            options.CanSavePreferences = options.CanDeletePreferences = false;

            if (deleteUserPreference) {
                options.CanDeletePreferences = true;
            } else {
                options.CanSavePreferences = true;
                options.ColumnPreferences = FinderPreferences.GetSelectedColumns();
            }
            that._reloadFinder(that, options);
        },

        _reloadFinder: function (that, options) {
            var data = { finderOptions: options };
            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "Find", "Find"), data, function (successData) {
                that._showFinderScreen(that, successData, "#" + that.divFinderDialogId);
            });
        },

        _triggerChange: function (that) {
            that._resetFocus(that);
            if (sg.delayVariables.IsInProgress) {
                sg.delayVariables.IsInProgress = false;
                if (sg.delayVariables.RowData.Length > 0) {
                    var data = sg.delayVariables.RowData[sg.delayVariables.ColumnName];
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, "");
                    sg.delayVariables.RowData.set(sg.delayVariables.ColumnName, data);
                }
                if (sg.delayVariables.TextBoxElement) {
                    sg.delayVariables.TextBoxElement.change();
                }
            }
            if (sg.finderHelper.cancelFuncCall != $.noop) {
                sg.finderHelper.cancelFuncCall();
                sg.finderHelper.cancelFuncCall = $.noop;
            }
        },

        _selectGrid: function (that) {
            var grid, row, data;
            if ($('#div_finder_grid')) {
                grid = $('#div_finder_grid').data("kendoGrid");
                row = grid.select();
                data = grid.dataItem(row);
                if (row.length != 0 && !that.options.postbackNotRequired) {
                    //Get all the coulmns from the server.
                    var filterData = [[]];
                    for (var i = 0; i < sg.keys.length; i++) {
                        filterData[0].push({ "Field": sg.keys[i], "Operator": sg.finderOperator.Equal, "Value": data[sg.keys[i].field] });
                    }
                    var finderOptions = {
                        SearchFinder: that.options.searchFinder,
                        AdvancedFilter: filterData
                    };
                    //This call is synchronous which should be fine.
                    window.sg.utls.ajaxPostSync(window.sg.utls.url.buildUrl("Core", "Find", "Get"), finderOptions, function (successData) {
                        data = successData;
                    });
                }
            }
            return data;
        },

        _isNullOrUndefined: function (variable) {
            if (variable === null || typeof variable === "undefined") {
                return false;
            }

            return true;
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _getSelectedRow: function (that) {
            that._resetFocus(that);
            var dataSelected = that._selectGrid(that);
            if (that._isNullOrUndefined(dataSelected)) {
                $('.selectKendoGrid').attr("disabled", false).removeClass("btnStyle2Disabled");
                var select = that.options.select;
                if (select) {
                    //TODO:The arguments has to be sent as single json object
                    $(this).on('click', select(dataSelected, that.options.id));
                }
                var finderWin = $("#" + that.divFinderDialogId).data("kendoWindow");
                if (finderWin != undefined) {
                    finderWin.destroy();
                    sg.utls.isFinderClicked = false;
                    sg.findEvent = null;
                }
            }
        },
        _resetFocus: function (that) {
            var finderElement = $("#" + that.options.id);
            if (finderElement.length == 0) {
                finderElement.focus();
            } else {
                finderElement[0].focus();
            }
        }
    });

})(jQuery, window, document);

/* Finder Grid Columns Customization */
var FinderPreferences = FinderPreferences || {};
FinderPreferences = {
    FinderPreferencesHTML: null,

    Initialize: function () {
        $("#tblTBodyFinderPref").sortable({ cursor: "move" }).disableSelection();

        $(document).off('.plugin.finderPref');


        $(document).on('click.plugin.finderPref', "#btnFinderPrefCancel", function () {
            $("#tblTBodyFinderPref").html(FinderPreferences.FinderPreferencesHTML);
            FinderPreferences.Hide();
        });

        $(document).on('click.plugin.finderPref', "#chkSelectAll", function () {
            FinderPreferences.ChangeCheckState(this.checked);
        });
        $(document).on('click.plugin.finderPref', "[name='chkFinderPrefCol']", function () {
            FinderPreferences.SelectHeaderCheckBox();
        });
        $(document).on('click.plugin.finderPref', function (e) {
            var container = $('#divFinderPrefEditCols');
            var editColumnButton = $("#btnFinderPrefEditCols");
            // if the target of the click isn't the container... nor a descendant of the container
            if (!container.is(e.target) && !editColumnButton.is(e.target) && container.has(e.target).length === 0) {
                container.hide();
            }
        });
    },

    // Check/Uncheck the "Select All" checkbox based on the selection of list of columns
    SelectHeaderCheckBox: function () {
        $('#chkSelectAll').attr('checked', 'checked').applyCheckboxStyle();
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if (!$(this).is(':checked')) {
                $('#chkSelectAll').removeAttr('checked').applyCheckboxStyle();
                return;
            }
        });
    },
    /**
     * Hide the list of columns
     * @method hide      
     */
    Hide: function () {
        $('#divFinderPrefEditCols').hide();
    },
    // Show or Hide list of columns
    ShowFieldsWindow: function () {
        FinderPreferences.CheckFinderPrefCols('#div_finder_grid');
        FinderPreferences.SelectHeaderCheckBox();
        var container = $('#divFinderPrefEditCols');
        var btnEdit = $('#btnFinderPrefEditCols');

        //Static position of edit grid.
        var topAlignment = 130;
        var leftAlignment = 10;

        $('#divFinderPrefEditCols').css({ top: topAlignment, left: leftAlignment, position: 'absolute', "z-index": "1000" });
        $('#divFinderPrefEditCols').show();
    },

    GetSelectedColumns: function () {
        var selectedColumns = [];
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if ($(this).is(':checkbox')) {
                selectedColumns.push(FinderPreferences.GetGridColumn($(this).attr('data-finder-key'), $(this).is(':checked')));
            }
        });
        return selectedColumns;
    },

    GetGridColumn: function (id, isChecked) {
        var gridColumn = new Object();
        gridColumn.field = id;
        gridColumn.isHidden = !isChecked;
        return gridColumn;
    },

    // Check the columns by skipping saved hidden columns
    CheckFinderPrefCols: function (gridname) {
        var grid = $(gridname).data('kendoGrid');
        var isChecked = false;
        if (grid != null) {
            $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
                isChecked = false;
                for (var i = 0; i < grid.columns.length; i++) {
                    if ($(this).attr('data-finder-key') === grid.columns[i].field) {
                        if (!grid.columns[i].hidden) {
                            $(this).attr('checked', 'checked').applyCheckboxStyle();
                            isChecked = true;
                        }
                    }
                }
                if (!isChecked) {
                    $(this).removeAttr('checked').applyCheckboxStyle();
                }
            });
        }
    },

    // Select/Unselect all checkbox fields
    ChangeCheckState: function (flag) {
        $('#tblTBodyFinderPref input[name=chkFinderPrefCol][type=checkbox]').each(function () {
            if (flag) {
                $(this).attr('checked', 'checked').applyCheckboxStyle();
            } else {
                $(this).removeAttr('checked').applyCheckboxStyle();
            }

        });
    }
};
