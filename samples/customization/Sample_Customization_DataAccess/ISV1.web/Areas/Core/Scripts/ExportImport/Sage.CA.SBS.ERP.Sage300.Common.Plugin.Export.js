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
var kendoWindow = null;
(function (sg, $) {
    sg.exportHelper = {
        exportKeys: {},
        exportModel: {},
        abortPolling: false,
        gridOption: {
            scrollable: false,
            sortable: false,
            pageable: false,
            editable: false,
            selectable: true,
            resizable: true,
            columns: [
                { title: globalResource.Index, template: "#= ++exportResultRowNumber   #", width: 30 },
                { field: "PriorityString", title: globalResource.Priority },
                { field: "Message", title: globalResource.Description, width: 600, template: "#= sg.utls.formatMessageText(Message) #" }
            ],
            dataBinding: function () {
                exportResultRowNumber = 0;
            }
        },
        setExportEvent: function (id, exportName, hasOptions, exportKeys, callbackFunc) {
            $("#" + id).Export({
                name: exportName,
                exportImportOptions: hasOptions,
                keys: exportKeys,
                ok: callbackFunc
            });


        },
        showExportResult: function () {
            sg.exportHelper.abortPolling = true;
            var model = sg.exportHelper.exportModel;
            $("#exportResult").show();
            $("#exportMessageDiv").hide();
            $(".k-window-action").show();

            if (model.ExportResponse.Results().length == 1) {
                var messageType = model.ExportResponse.Results()[0].Priority();
                var message = model.ExportResponse.Results()[0].Message();
                window.sg.utls.showProcessMessageInfo(messageType, message, 'exportResultMessageDiv');
            } else {
                $("#resultgrid").show();
            }
            $("#btnClose").show();
            if (model.ExportResponse.Results()[0].Priority() === 1) {
                $("#lnkDownload").show();
            }
        },
        avoidCircularReference: false
    };

    sg.dataMigration = {
        Account: "account",
        AccountGroup: "accountgroup",
        AccountSet: "icaccountset",
        AccountStructure: "accountstructure",
        APAccountSet: "apaccountset",
        APDistributionCode: "apdistributioncode",
        APDistributionSet: "apdistributionset",
        APInvoiceEntry: "apinvoiceentry",
        APOptionalField: "apoptionalfield",
        APPaymentCode: "appaymentcode",
        APPaymentSelectionCode: "appaymentselectioncode",
        APTerms: "apterms",
        ARAccountSet: "araccountset",
        AROptionalField: "aroptionalfield",
        ARTerms: "arterms",
        Bank: "bank",
        BankDistributionCodes: "bankdistributioncode",
        BankEntry: "bankentry",
        BankOptions: "bankoptions",
        BillingCycle: "billingcycle",
        CommentType: "commenttype",
        CompanyProfile: "companyprofile",
        CPRSAmount: "cprsamount",
        CPRSCode: "apcprscode",
        CSOptionalField: "csoptionalfield",
        CurrencyCode: "currencycode",
        CurrencyRate: "currencyrate",
        CurrencyRateType: "currencyratetype",
        Customer: "customer",
        CustomerDetail: "iccustomerdetail",
        CustomerGroup: "customergroup",
        DistributionCode: "distributioncode",
        DistributionCodes: "distributioncode",
        DistributionSet: "distributionset",
        DunningMessage: "dunningmessage",
        EmailMessage: "emailmessage",
        EmailMessages: "emailmessages",
        FiscalCalendar: "fiscalcalendar",
        ICItemPricing: "icitempricing",
        ICOptionalField: "icoptionalfield",
        ICPriceListCode: "icpricelistcode",
        ICSegmentCode: "icsegmentcode",
        InterestProfiles: "interestprofiles",
        InvoiceEntry: "arinvoiceentry",
        Items: "item",
        ItemStructure: "itemstructure",
        JournalEntry: "journalentry",
        Location: "location",
        Category: "category",
        ManufacturersItem: "icmanufacturersitem",
        NationalAccount: "nationalaccount",
        OptionalFields: "optionalfields",
        PaymentCodes: "paymentcode",
        PaymentEntry: "appaymententry",
        POShipViaCode: "poshipviacodes",
        RecurringEntry: "recurringentry",
        RecurringPayable: "recurringpayable",
        RecurringCharge: "recurringcharge",
        Salesperson: "salesperson",
        SecurityGroup: "securitygroup",
        SecurityGroupSystem: "securitygroupsystem",
        SegmentCode: "segmentcode",
        ShipToLocation: "shiptolocation",
        SourceCode: "sourcecode",
        TaxAuthority: "taxauthority",
        TaxClasses: "taxclasses",
        TaxGroup: "taxgroup",
        TaxRates: "taxrates",
        TransactionStatistics: "ictransactionstatistics",
        UnitOfMeasure: "icunitsofmeasure",
        UserAuthorization: "userauthorization",
        UserAuthorizationSystem: "userauthorizationsystem",
        Vendor: "vendor",
        VendorDetail: "icvendordetail",
        VendorGroup: "vendorgroup",
        WeightUnitOfMeasure: "weightunitsofmeasure",
        ReconcileStatement: "reconcilestatement",
        ReceiptEntry: "arreceiptentry",
        OEOptionalField: "oeoptionalfield",
        Shipment: "icshipment",
        OETemplate: "oetemplate",
        Receipt: "icreceipt",
        ICKittingItem: "ickittingitem",
        SalesStatistic: "icsalesstatistic",
        POOptionalField: "pooptionalfield",
        POPurchaseStatistics: "popurchasestatistics",
        POTemplate: "potemplate",
        POAdditionalCost: "poadditionalcost",
        RequisitionEntry: "porequisitionentry",
        POEmailMessages: "poemailmessages",
        Oemiscellaneouscharge: "oemiscellaneouscharge",
        SalesHistory: "saleshistory",
        OEShipViaCodes: "oeshipviacodes",
        OEEmailMessage: "oeemailmessage",
        POVendorContractCost: "povendorcontractcost",
        POReceiptEntry: "poreceiptentry",
        PurchaseHistory: "purchasehistory",
        OESalesStatistic: "oesalesstatistic",
        ICAdjustment: "icadjustment",
        ReturnEntry: "returnentry",
        ICLocationDetail: "iclocationdetail",
        PhysicalInvQuantity: "physicalinvquantity",
        PhysicalInvQuantityHeader: "physicalinvquantityheader",
        POInvoiceEntry: "poinvoiceentry",
        Transfer: "transfer",
        ReorderQuantity: "reorderquantity",
        PurchaseOrderEntry: "purchaseorderentry",
        OEOrderEntry: "oeorderentry",
        GLSourceJournalProfile: "glsourcejournalprofile",
        InternalUsage: "icinternalusage",
        CreditDebitNoteEntry: "oecreditdebitnoteentry",
        POCreditDebitNoteEntry: "pocreditdebitnoteentry",
        GLRevaluationCode: "revaluationcode",
        ICContractCode: "contractcode",
        ICWarrantyCode:"warrantycode",
        ICAssembly: "icassembly",
        CSSchedule: "csschedule",
        RemitToLocation: "remittolocation",
        LotNumber: "lotnumber",
        ArAdjustmentEntry: "aradjustmententry",
        ArRefundEntry: "arrefundentry",
        BillsOfMaterial:"billsofmaterial",
        ArQuickReceiptEntry: "arquickreceiptentry",
        ARItemsExportImport: "aritems",
        CreateAccountPreview: "createaccountpreview",
        APAdjustmentEntry: "apadjustmententry",
        CSCreditCardType: "creditcardtype"
    };

}(sg = sg || {}, jQuery));

(function ($, window, document, undefined) {
    $.widget("sageuiwidgets.Export", {
        divExportDialogId: '',

        options: {
            id: "",
            name: "",
            title: globalResource.ExportWindowTitle,
            exportImportOptions: false,
            keys: $.noop,
            ok: $.noop,
            cancel: $.noop
        },

        _create: function () {
            var that = this;
            $(that.element).bind('click', function () {
                that._doAjax(that);
            });
        },

        _doAjax: function (that) {
            //If export options are not present
            if (that.options.exportImportOptions == false) {
                that._setUpExportWindow(that);
            } else {
                that._setUpExportImportOptionWindow(that);
            }
        },

        _setUpExportImportOptionWindow: function (that) {
            that.divExportDialogId = 'div_' + that.options.name + '_optionDialog';
            $('<div id="' + that.divExportDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divExportDialogId;
            var data = {
                viewModel: { Name: that.options.name, ExportRequest: { Name: that.options.name } }
            };

            kendoWindow = $(dialogId).kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: false,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: 820,
                minHeight: 100,
                maxHeight: 600,
                actions: ["Close"],
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate,
                close: function () {
                    that._destroyKendoWindow();
                },
                //Open Kendo Window in center of the Viewport
                open: function () {
                    sg.utls.setKendoWindowPosition(this);
                },
            }).data("kendoWindow");

            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ExportImport", "ExportImportOptions"), data, function (successData) {

                //Load HTML Content into Model window
                $(dialogId).html(successData);

                //KO winding within a div (exportScreen) not on DOM
                sg.exportHelper.exportModel = window.ko.mapping.fromJS(exportImportModelData);
                window.ko.applyBindings(sg.exportHelper.exportModel, $("#exportImportOptionScreen")[0]);

                window.sg.utls.kndoUI.dropDownList("exportImportOption");
                $(document).off('click', '#frmExportOptions');
                $(document).on('click', '#frmExportOptions', function () {
                    that._destroyKendoWindow();
                    that._setUpExportWindow(that, ko.mapping.toJS(sg.exportHelper.exportModel));
                });
                $("#exportImportOption").focus();
            });

            kendoWindow.open();
        },

        _setUpExportWindow: function (that, viewModel) {
            if (viewModel == undefined) {
                viewModel = { Name: that.options.name, ExportRequest: { Name: that.options.name } };
            }

            var data = {
                viewModel: viewModel
            };

            that.divExportDialogId = 'div_' + that.options.name + '_dialog';
            $('<div id="' + that.divExportDialogId + '"  style="display:none"/>').appendTo('body');
            var dialogId = "#" + that.divExportDialogId;

            kendoWindow = $(dialogId).kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: false,
                scrollable: false,
                visible: false,
                navigatable: true,
                width: 630,
                minHeight: 200,
                maxHeight: 600,
                actions: ["Close"],
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate,
                close: function () {
                    that._destroyKendoWindow();
                },
                //Open Kendo Window in center of the Viewport
                open: function () {
                    sg.utls.setKendoWindowPosition(this);
                },
            }).data("kendoWindow");

            window.sg.utls.ajaxPostHtml(window.sg.utls.url.buildUrl("Core", "ExportImport", "ExportIndex"), data, function (successData) {
                that._showExportScreen(that, successData, dialogId);
            });
        },

        _showExportScreen: function (that, data, dialogId) {
            $(dialogId).html(data);

            //clear all events;
            $(document).off('.plugin.export');


            //KO winding within a div (exportScreen) not on DOM
            sg.exportHelper.exportModel = window.ko.mapping.fromJS(exportModelData);
            window.ko.applyBindings(sg.exportHelper.exportModel, $("#exportScreen")[0]);

            kendoWindow.open();

            var exportWindowId = dialogId + "_wnd_title";
            $("#ExportOptions").insertAfter($(exportWindowId));
            window.sg.utls.kndoUI.dropDownList("FileTypes");
            $("#exportTreeView").kendoTreeView();

            $(document).on('click.plugin.export', '#btnExport', function () {
                var optionsDiv = $(".k-header").find("#ExportOptions");
                optionsDiv.remove();
                that._doExport(that);
            });

            $(document).on('click.plugin.export', '.exportImportHeader', function () {
                var headerIndex = $(this).attr("data-sg-index");
                $.each(sg.exportHelper.exportModel.ExportRequest.DataMigrationList(), function (index, header) {
                    if (headerIndex == index) {
                        $.each(header.Items(), function (ind, item) {
                            if (item.print() != header.Print()) {
                                item.print(header.Print());
                            }
                        });
                    }
                });
            });

            $(document).on('click.plugin.export', '.exportImportDetail', function () {
                var headerIndex = $(this).closest("ul").closest("li").find("input[type=checkbox]:first").attr("data-sg-index");
                $.each(sg.exportHelper.exportModel.ExportRequest.DataMigrationList(), function (index, header) {
                    if (headerIndex == index) {
                        header.Print(false);
                        $.each(header.Items(), function (ind, item) {
                            if (item.print()) {
                                header.Print(true);
                                return;
                            }
                        });
                    }
                });
            });

            $(document).on('click.plugin.export', '#btnCancel', function () {
                that._destroyKendoWindow();
            });

            $(document).on('click.plugin.export', '#btnOk', function () {
                that._destroyKendoWindow();
                if (that.options.ok !== $.noop) {
                    that.options.ok.call();
                }
            });

            $(document).on('click.plugin.export', '#btnSaveScript', function () {
                globalResource.AllowPageUnloadEvent = false;
                $("#targetId").submit();
                //This is required so that is dirty message defined on beforeunload event will not fire.
                setTimeout(function () {
                    globalResource.AllowPageUnloadEvent = true;
                }, 10);
            });

            $('#formLoadScript').ajaxForm(function (result) {

                if (result.UserMessage != undefined && result.UserMessage.Errors.length > 0) {
                    window.sg.utls.showMessageInfoInCustomDivWithoutClose(window.sg.utls.msgType.ERROR, result.UserMessage.Errors[0].Message, "loadScriptMessage");
                } else {
                    ko.mapping.fromJS(result.DataMigrationList, {}, sg.exportHelper.exportModel.ExportRequest.DataMigrationList);
                    $("#divLoadScript").data("kendoWindow").close();
                }
            });

            $(document).on('click.plugin.export', '#btnLoadScript', function () {
                $("#loadScriptMessage").empty();
                $('#formLoadScript')[0].reset();
                $("#divLoadScript").kendoWindow({
                    modal: true,
                    title: loadScript,
                    resizable: false,
                    draggable: false,
                    scrollable: false,
                    visible: false,
                    navigatable: true,
                    width: 630,
                    minHeight: 200,
                    maxHeight: 600,
                    // Custom function to suppot focus within kendo window
                    activate: sg.utls.kndoUI.onActivate,
                    //Open Kendo Window in center of the Viewport
                    open: function () {
                        sg.utls.setKendoWindowPosition(this);
                    },
                }).data("kendoWindow").open();
            });

            $(document).on('click.plugin.export', '#loadScriptSubmit', function () {
                $("#formLoadScript").submit();
            });

            // Option dropdown in popup screen
            var menuLink = $(".dropDown-Menu > li");
            menuLink.find("> a").append('<span class="arrow-grey"></span>');
            menuLink.hover(function () {
                $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
                $(this).children(".sub-menu").show();
            }, function () {
                $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
                $(this).children(".sub-menu").hide();
            });

            $(document).on('change.plugin.export', '#btnFile', function (e) {
                var files = e.target.files;
                var selectedFIle = files[0];
                $('#btnUploadFile').val(selectedFIle.name);
                $("#loadScriptMessage").empty();
            });

        },

        _destroyKendoWindow: function (e) {
            kendoWindow.destroy();
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _doExport: function (that) {
            sg.exportHelper.abortPolling = false;
            if (that.options.keys != $.noop) {
                sg.exportHelper.exportModel.ExportRequest.Keys(that.options.keys.call());
            }

            $("#exportTree").hide();
            $("#exportResult").show();
            $("#btnClose").hide();
            $(".k-window-action").hide();

            var data = { viewModel: ko.mapping.toJS(sg.exportHelper.exportModel) };
            sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "Export"), data, function (result) {
                ko.mapping.fromJS(result.Data.ExportResponse, {}, sg.exportHelper.exportModel.ExportResponse);
                var data = { viewModel: ko.mapping.toJS(sg.exportHelper.exportModel) };
                if (data.viewModel.ExportResponse.Status != 0) {
                    sg.exportHelper.showExportResult();
                } else {

                    window.sg.utls.recursiveAjaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "Progress"), data, that._progress, that._abort);
                };
            });
        },
        _progress: function (result) {
            ko.mapping.fromJS(result.ExportResponse, {}, sg.exportHelper.exportModel.ExportResponse);
            var model = sg.exportHelper.exportModel;

            if (model.ExportResponse.Status() === 2 || model.ExportResponse.Status() === 3) { //Error or Completed
                sg.exportHelper.showExportResult();

                var fileUrl = sg.utls.url.buildUrl("Core", "ExportImport", "GetExportBlobReference") + "?blobName=" + sg.exportHelper.exportModel.ExportResponse.FileName();
                $('#btnDownload').attr('href', fileUrl);
            }
        },
        _abort: function (that) {
            return sg.exportHelper.abortPolling;
        }
    });

})(jQuery, window, document);
