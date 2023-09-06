/* Copyright (c) 2023 The Sage Group plc or its licensors.  All rights reserved. */

/* globals globalResource: false */
/* globals kendo: false */
/* globals exportModelData: false */
/* globals ko: false */
/* globals setCriteriaUI: false */
/* globals exportImportModelData: false */
/* globals loadScript: false */

"use strict";

var kendoWindow = null;
var exportResultRowNumber = 0;
(function (sg, $) {
    sg.exportHelper = {
        exportKeys: [],
        exportModel: {},
        abortPolling: false,
        initCriteriaTabPage: true,
        initPreviewTabPage: true,
        needRefresh: true,
        expanded: false,
        entityContext: null,

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
                { field: "Message", title: globalResource.Description, width: 600, template: "#: sg.utls.formatMessageText(Message) #" }
            ],
            dataBinding: function () {
                exportResultRowNumber = 0;
            }
        },

        setExportEvent: function (id, exportName, hasOptions, exportKeys, callbackFunc, entityContext = null) {
            $("#" + id).Export({
                name: exportName,
                exportImportOptions: hasOptions,
                keys: exportKeys,
                ok: callbackFunc
            });
            sg.exportHelper.exportKeys = exportKeys;

            // optional entity context for PR
            if (entityContext) {
                sg.exportHelper.entityContext = entityContext;
            }
        },

        showExportResult: function () {
            sg.exportHelper.abortPolling = true;
            var model = sg.exportHelper.exportModel;
            $("#exportResult").show();
            $("#exportMessageDiv").hide();
            $(".k-window-action").show();

            var results = model.ExportResponse.Results();
            if (results.length === 1) {
                var messageType = results[0].Priority();
                var message = results[0].Message();
                $("#resultgrid").hide();
                window.sg.utls.showProcessMessageInfo(messageType, message, 'exportResultMessageDiv');
                $("#exportResultMessageDiv").find("h3").css("margin-top", "19px").css("margin-left", "70px");
                
            } else {
                $("#resultgrid").show();
            }
            $("#btnClose").show();

            if (results.length > 0 && results[0].Priority() === 1) {
                $("#lnkDownload").show();
            }
        },
        avoidCircularReference: false,

        getEntityContext: () => {
            return sg.exportHelper.entityContext;
        },
    };

    sg.dataMigration = {
        Account: "account",
        AccountHistoryInquiry: "accounthistoryinquiry",
        AccountGroup: "accountgroup",
        AccountPermissions: "accountpermission",
        AccountSet: "icaccountset",
        AccountStructure: "accountstructure",
        APAccountSet: "apaccountset",
        APAdjustmentEntry: "apadjustmententry",
        APDistributionCode: "apdistributioncode",
        APDistributionSet: "apdistributionset",
        APInvoiceEntry: "apinvoiceentry",
        APOptionalField: "apoptionalfield",
        APPaymentCode: "appaymentcode",
        APPaymentSelectionCode: "appaymentselectioncode",
        APTerms: "apterms",
        ARAccountSet: "araccountset",
        ArAdjustmentEntry: "aradjustmententry",
        ARItemsExportImport: "aritems",
        AROptionalField: "aroptionalfield",
        ArQuickReceiptEntry: "arquickreceiptentry",
        ArRefundEntry: "arrefundentry",
        ARTerms: "arterms",
        Bank: "bank",
        BankDistributionCodes: "bankdistributioncode",
        BankEntry: "bankentry",
        BankOptions: "bankoptions",
        BillingCycle: "billingcycle",
        BillsOfMaterial: "billsofmaterial",
        Category: "category",
        CommentType: "commenttype",
        CompanyProfile: "companyprofile",
        CPRSAmount: "cprsamount",
        CPRSCode: "apcprscode",
        CreateAccountPreview: "createaccountpreview",
        CreditDebitNoteEntry: "oecreditdebitnoteentry",
        CSCreditCardType: "creditcardtype",
        CSOptionalField: "csoptionalfield",
        CSSchedule: "csschedule",
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
        GLRevaluationCode: "revaluationcode",
        GLSourceJournalProfile: "glsourcejournalprofile",
        ICAdjustment: "icadjustment",
        ICAssembly: "icassembly",
        ICContractCode: "contractcode",
        ICItemPricing: "icitempricing",
        ICKittingItem: "ickittingitem",
        ICLocationDetail: "iclocationdetail",
        ICOptionalField: "icoptionalfield",
        ICPriceListCode: "icpricelistcode",
        ICSegmentCode: "icsegmentcode",
        ICWarrantyCode: "warrantycode",
        InterestProfiles: "interestprofiles",
        InternalUsage: "icinternalusage",
        InvoiceEntry: "arinvoiceentry",
        Items: "item",
        ItemStructure: "itemstructure",
        JournalEntry: "journalentry",
        Location: "location",
        LotNumber: "lotnumber",
        ManufacturersItem: "icmanufacturersitem",
        MTOptionalField: "mtoptionalfield",
        NationalAccount: "nationalaccount",
        OEEmailMessage: "oeemailmessage",
        OEInvoiceEntry: "oeinvoiceentry",
        Oemiscellaneouscharge: "oemiscellaneouscharge",
        OEOptionalField: "oeoptionalfield",
        OEOrderEntry: "oeorderentry",
        OESalesStatistic: "oesalesstatistic",
        OEShipmentEntry: "oeshipmententry",
        OEShipViaCodes: "oeshipviacodes",
        OETemplate: "oetemplate",
        OptionalFields: "optionalfields",
        PaymentCodes: "paymentcode",
        PaymentEntry: "appaymententry",
        PhysicalInvQuantity: "physicalinvquantity",
        PhysicalInvQuantityHeader: "physicalinvquantityheader",
        PMAccountSet: "pmaccountset",
        POAdditionalCost: "poadditionalcost",
        POCreditDebitNoteEntry: "pocreditdebitnoteentry",
        POEmailMessages: "poemailmessages",
        POInvoiceEntry: "poinvoiceentry",
        POOptionalField: "pooptionalfield",
        POPurchaseStatistics: "popurchasestatistics",
        POReceiptEntry: "poreceiptentry",
        POShipViaCode: "poshipviacodes",
        POTemplate: "potemplate",
        POVendorContractCost: "povendorcontractcost",
        PurchaseHistory: "purchasehistory",
        PurchaseOrderEntry: "purchaseorderentry",
        Receipt: "icreceipt",
        ReceiptEntry: "arreceiptentry",
        ReconcileStatement: "reconcilestatement",
        RecurringCharge: "recurringcharge",
        RecurringEntry: "recurringentry",
        RecurringPayable: "recurringpayable",
        RemitToLocation: "remittolocation",
        ReorderQuantity: "reorderquantity",
        RequisitionEntry: "porequisitionentry",
        ReturnEntry: "returnentry",
        ReverseCharges: "reversecharges",
        SalesHistory: "saleshistory",
        Salesperson: "salesperson",
        SalesStatistic: "icsalesstatistic",
        SecurityGroup: "securitygroup",
        SecurityGroupSystem: "securitygroupsystem",
        SegmentCode: "segmentcode",
        Shipment: "icshipment",
        ShipToLocation: "shiptolocation",
        SourceCode: "sourcecode",
        TaxAuthority: "taxauthority",
        TaxClasses: "taxclasses",
        TaxGroup: "taxgroup",
        TaxRates: "taxrates",
        TmRCode: "tmrcode",
        TmTxMap: "tmtxmap",
        TransactionStatistics: "ictransactionstatistics",
        Transfer: "transfer",
        TsRCode: "tsrcode",
        TsTxMap: "tstxmap",
        UnitOfMeasure: "icunitsofmeasure",
        ASUser: "asuser",
        UserAuthorization: "userauthorization",
        UserAuthorizationSystem: "userauthorizationsystem",
        Vendor: "vendor",
        VendorDetail: "icvendordetail",
        VendorGroup: "vendorgroup",
        WeightUnitOfMeasure: "weightunitsofmeasure",
        WithholdingTaxRates: "withholdingtaxrates",
        EmployeeTimecard: "premployeetimecard",
        Timecard: "prtimecard"
    };
}(sg || {}, jQuery));

(function ($, window, document, undefined) {
    var processExportTimer;
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
            if (that.options.exportImportOptions === false) {
                that._setUpExportWindow(that);
            } else {
                that._setUpExportImportOptionWindow(that);
            }
        },

        _setUpExportImportOptionWindow: function (that) {
            that.divExportDialogId = 'div_' + that.options.name + '_optionDialog';
            $('<div id="' + that.divExportDialogId + '"  style="display:none"></div>').appendTo('body');
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
                //Open Kendo Window in center of the Viewport. Also set title bar color
                open: sg.utls.kndoUI.onOpen,
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate,
                close: function () {
                    that._destroyKendoWindow();
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
            if (viewModel === undefined) {
                viewModel = { Name: that.options.name, ExportRequest: { Name: that.options.name } };
            }

            var data = {
                viewModel: viewModel,
                entityPrefix: sg.exportHelper.getEntityContext()
            };

            that.divExportDialogId = 'div_' + that.options.name + '_dialog';
            $('<div id="' + that.divExportDialogId + '" class="export-window"  style="display:none"></div>').appendTo('body');
            var dialogId = "#" + that.divExportDialogId;

            kendoWindow = $(dialogId).kendoWindow({
                modal: true,
                title: that.options.title,
                resizable: false,
                draggable: false,
                scrollable: false,
                visible: false,
                navigatable: false,
                width: 860,
                //height:500,
                minHeight: 200,
                maxHeight: 800,
                actions: ["Close"],
                //Open Kendo Window in center of the Viewport. Also set title bar color
                open: sg.utls.kndoUI.onOpen,
                //custom function to suppot focus within kendo window
                activate: sg.utls.kndoUI.onActivate,
                close: function () {
                    that._destroyKendoWindow();
                    sg.exportHelper.initCriteriaTabPage = true;
                    sg.exportHelper.initPreviewTabPage = true;
                },
            }).data("kendoWindow");

            var buildUrl = window.sg.utls.url.buildUrl;
            window.sg.utls.ajaxPostHtml(buildUrl("Core", "ExportImport", "ExportIndex"), data, function (successData) {
                that._showExportScreen(that, successData, dialogId);
            });

        },

        _initCriteriaTab: function () {
            if (sg.exportHelper.initCriteriaTabPage) {
                var exportDataSource = exportModelData.ExportRequest.DataMigrationList;

                var columns = exportDataSource[0].Items.filter(function (item) {
                    return !item.IsVirtual;
                });

                // If every field is virtual, include them
                if (columns.length === 0) {
                    columns = exportDataSource[0].Items;
                }

                setCriteriaUI.init(columns, "title", "columnName");
                sg.exportHelper.initCriteriaTabPage = false;
            }
        },

        _initPreviewTab: function (that) {
            if (sg.exportHelper.initPreviewTabPage) {
                //Init preview tab grid controls
                that._configCriteriaPreviewGrid(that);
                sg.exportHelper.initPreviewTabPage = false;
            }
        },

        //map export model print property to tree node checked property, add required fields mark symbol *
        _createTreeViewDataSource: function (exportDataSource) {
            exportDataSource.forEach(function (obj) {
                obj.checked = obj.Print;
                obj.Items.forEach(function (item) {
                    item.checked = item.print;
                    if (item.IsKey && item.title.slice(-1) !== "*") {
                        item.title += " *";
                    }
                });
            });

            //create hierarchical data source for kendo tree view
            var dataSource = new kendo.data.HierarchicalDataSource({
                data: exportDataSource,
                schema: {
                    model: {
                        children: "Items"
                    }
                }
            });
            return dataSource;
        },

        //show export screen
        _showExportScreen: function (that, data, dialogId) {
            $(dialogId).html(data);
            //clear all events;
            $(document).off('.plugin.export');

            //KO winding within a div (exportScreen) not on DOM
            sg.exportHelper.exportModel = window.ko.mapping.fromJS(exportModelData);
            var exportDataSource = exportModelData.ExportRequest.DataMigrationList;

            $("#ExportTabstrip").kendoTabStrip({
                animation: {
                    open: {
                        effects: "fadeIn"
                    }
                },
                select: function (e) {
                    var index = $(e.item).index();
                    var $treeView = $("#exportTreeView");
                    var $preViewGrid = $("#CriteriaGrid");
                    var $criteriaGrid = $("SetCriteriaGrid");

                    $treeView.hide();
                    $preViewGrid.hide();
                    $criteriaGrid.hide();

                    if (index === 0) {
                        $treeView.show();
                    }

                    if (index === 1) {
                        that._initCriteriaTab(that);
                        $criteriaGrid.show();
                        $("#btnClearCriteria").show();
                        $("#btnShowCriteria").show();
                    } else {
                        $("#btnClearCriteria").hide();
                        $("#btnShowCriteria").hide();
                        $("#CriteriaStringPopup").removeClass('show');
                    }

                    if (index === 2) {
                        that._initPreviewTab(that);
                        if (sg.exportHelper.needRefresh) {
                            $preViewGrid.data("kendoGrid").dataSource.page(1);
                            sg.exportHelper.needRefresh = false;
                        }
                        $preViewGrid.show();
                   }
                },
            }).data("kendoTabStrip");

            $("#btnClearCriteria").hide();
            $("#btnShowCriteria").hide();
            
            window.ko.applyBindings(sg.exportHelper.exportModel, $("#exportResult")[0]);
            window.ko.applyBindings(sg.exportHelper.exportModel, $("#exportScreen")[0]);

            kendoWindow.open();

            var exportWindowId = dialogId + "_wnd_title";
            $("#ExportOptions").insertAfter($(exportWindowId));
            window.sg.utls.kndoUI.dropDownList("FileTypes");

            var $treeView = $("#exportTreeView");
            var dataSource = that._createTreeViewDataSource(exportDataSource);

            $treeView.kendoTreeView({
                checkboxes: {
                    checkChildren: true
                },
                dataSource: dataSource,
                loadOnDemand: true,
                dataTextField: ["Description", "title"],
                check: function (e) {
                    var dataItem = this.dataItem(e.node);
                    if (dataItem.Items && dataItem.Items.length === 0) {
                        this.expand(e.node);
                        dataItem.set("checked", false);
                    }
                },
                dataBound: function (e) {
                    var node = e.node;
                    if (node) {
                        //change the * color and keep checkbox tri state
                        var nodeHtml = $(node).html();
                        var cb = $(node).find(":checkbox:first");
                        var state = (cb.length > 0) ? cb[0].indeterminate : false;
                        nodeHtml = nodeHtml.replace(/\*/g, "<span class='export-required'>*</span>");
                        $(node).html(nodeHtml);
                        //set html will remove the tri state, reset it
                        cb = $(node).find(":checkbox:first");
                        if (cb.length > 0 && state) {
                            cb[0].indeterminate = state;
                        }
                    }
                }
            });
            $treeView.off("hover");

            $(document).on('click.plugin.export', '#btnExport', function () {
                var optionsDiv = $(".k-header").find("#ExportOptions");
                optionsDiv.remove();
                that._doExport(that);
            });

            $(document).on('click.plugin.export', '#btnCancel', function () {
                sg.exportHelper.initCriteriaTabPage = true;
                sg.exportHelper.initPreviewTabPage = true;
                that._destroyKendoWindow();
            });

            $(document).on('click.plugin.export', '#btnOk', function () {
                that._destroyKendoWindow();
                if (that.options.ok !== $.noop) {
                    that.options.ok.call();
                }
            });

            $(document).on('click.plugin.export', '#btnSaveScript', function () {
                pageUnloadEventManager.disable();

                var data = { viewModel: ko.mapping.toJS(sg.exportHelper.exportModel) };
                that._exportData(data, false);
                var treeData = data.viewModel.ExportRequest.DataMigrationList;
                var strData = JSON.stringify(treeData);
                $("#DataMigrationList").val(strData );

                var grid = $("#SetCriteriaGrid").data("kendoGrid");
                if (grid) {
                    var gridOptions = grid.getOptions();
                    gridOptions.dataSource.fields = null;
                    $("#ExportCriteria").val(kendo.stringify(gridOptions));
                } else {
                    $("#ExportCriteria").val("");
                }

                $("#targetId").submit();
              
                //This is required so that is dirty message defined on beforeunload event will not fire.
                setTimeout(function () {
                    pageUnloadEventManager.enable();
                }, 10);
            });

            $('#formLoadScript').ajaxForm(function (result) {

                if (result.UserMessage !== undefined && result.UserMessage.Errors.length > 0) {
                    window.sg.utls.showMessageInfoInCustomDivWithoutClose(window.sg.utls.msgType.ERROR, result.UserMessage.Errors[0].Message, "loadScriptMessage");
                    $("#divLoadScript").parent().css({ height: '390px' });
                } else {
                    $("#divLoadScript").parent().css({ height: '230px' });
                    var loadModelName = result.Name;
                    var screenName = sg.exportHelper.exportModel.ExportRequest.Name();
                    if (loadModelName !== screenName) {
                        return;
                    }

                    that._initCriteriaTab();
                    //Load grid criteria data and refresh results 
                    var grid = $("#SetCriteriaGrid").data("kendoGrid");
                    var options = result.ExportCriteria;
                    if (grid && options) {
                        grid.setOptions(JSON.parse(options));
                        var data = grid.dataSource.data();
                        setCriteriaUI.refreshResults(data);
                    }
                    //Load tree view data and set data source
                    var len = result.DataMigrationList.length;
                    if (len > 0) {
                        var treeview = $("#exportTreeView").data("kendoTreeView");
                        var modelData = result.DataMigrationList;
                        var dataSource = that._createTreeViewDataSource(modelData);
                        var triStateList = [];
                        treeview.setDataSource(dataSource);
                        //Get tri state list (sublevel nodes partially checked)
                        for (var i = 0, length = modelData.length ; i < length; i++) {
                            var items = modelData[i].Items;
                            var itemLength = items.length;
                            var checkedLength = items.filter(function (item) { return item.checked; }).length;
                            var indeterminate = (itemLength !== checkedLength && checkedLength > 0);
                            triStateList.push(indeterminate);
                        }
                        //Set parent node checkbox as indeterminate (tri state)
                        var cbList = $("#exportTreeView").find(":checkbox");
                        if (cbList.length === triStateList.length) {
                            for (var j = 0, length1 = triStateList.length ; j < length1; j++) {
                                if (triStateList[j]) {
                                    cbList[j].indeterminate = true;
                                }
                            }
                        }
                    }

                    ko.mapping.fromJS(result.DataMigrationList, {}, sg.exportHelper.exportModel.ExportRequest.DataMigrationList);
                    $("#divLoadScript").data("kendoWindow").close();
                }
            });

            $(document).on('click.plugin.export', '#btnSetCriteria', function () {
                $("#divSetCriteria").kendoWindow({
                    modal: true,
                    title: "Set Criteria",
                    resizable: false,
                    draggable: false,
                    scrollable: false,
                    visible: false,
                    navigatable: true,
                    width: 1000,
                    minHeight: 200,
                    maxHeight: 850,
                    //Open Kendo Window in center of the Viewport. Also set title bar color
                    open: sg.utls.kndoUI.onOpen,
                    // Custom function to suppot focus within kendo window
                    activate: sg.utls.kndoUI.onActivate,
                    close: function () {
                    },
                }).data("kendoWindow").open();
            });

            $(document).on('click.plugin.export', '#btnLoadScript', function () {
                $("#divLoadScript").parent().css({ height: '230px' });
                $("#loadScriptMessage").empty();
                $('#formLoadScript')[0].reset();
                $("#divLoadScript").kendoWindow({
                    modal: true,
                    title: loadScript,
                    resizable: true,
                    draggable: true,
                    scrollable: false,
                    visible: false,
                    navigatable: true,
                    width: 520,
                    //height: 150,
                    minHeight: 150,
                    maxHeight: 600,
                    //Open Kendo Window in center of the Viewport. Also set title bar color
                    open: sg.utls.kndoUI.onOpen,
                    // Custom function to suppot focus within kendo window
                    activate: sg.utls.kndoUI.onActivate,
                }).data("kendoWindow").open();
            });

            $(document).on('click.plugin.export', '#loadScriptSubmit', function () {
                $("#formLoadScript").submit();
            });

            // Option dropdown in popup screen
            var menuLink = $(".dropDown-Menu > li");
            menuLink.find("> a").append('<span class="arrow-grey"></span>');
            menuLink.on("mouseenter", function () {
                $(this).find(".arrow-grey").removeClass("arrow-grey").addClass("arrow-white");
                $(this).children(".sub-menu").show();
            }).on("mouseleave", function () {
                $(this).find(".arrow-white").removeClass("arrow-white").addClass("arrow-grey");
                $(this).children(".sub-menu").hide();
            });

            $(document).on('change.plugin.export', '#btnFile', function (e) {
                var files = e.target.files;
                var selectedFIle = files[0];
                $('#btnUploadFile').val(selectedFIle.name);
                $("#loadScriptMessage").empty();
                $("#divLoadScript").parent().css({ height: '230px' });
            });
        },

        //Config export results grid columns
        _configColFilter: function () {
            var items = exportModelData.ExportRequest.DataMigrationList[0].Items;
            var length = items.length;
            var cols = [];
            var numbers = ["Int", "Long", "Byte", "Real", "Decimal"];
            var exist = false;

            for (var i = 0; i < length; i++) {
                var col = {};
                var item = items[i];
                var type = item.dataType;
                var attr = "w150";
                if (numbers.indexOf(type) > -1) {
                    attr += " align-right";
                }
                if (type === "Decimal") {
                    var precision = item.Precision;
                    if (precision === 0) {
                        type = "Int";
                    }
                    col.template = '#: kendo.format("{0:n' + precision + '}", ' + item.columnName + ')#';
                }
                col.title = item.field;
                col.field = item.columnName;
                exist = cols.filter(function (c) { return c.title === item.field; }).length > 0;
                if (exist) {
                    col.hidden = true;
                }
                col.columnName = item.columnName;
                col.attributes = { "class": attr };
                col.headerAttributes = { "class": attr };
                col.dataType = type;

                cols.push(col);
            }
            return cols;
        },

        //Config Criteria Results Grid
        _configCriteriaPreviewGrid: function (that) {
            var cols = that._configColFilter();
            var dataSource = new kendo.data.DataSource({
                serverPaging: true,
                serverFiltering: true,
                pageSize: 10,
                transport: {
                    read: function (options) {
                        
                        //function setFilterString() {
                        //    var keyValues = sg.exportHelper.exportKeys;
                        //    if (keyValues) {
                        //        keyValues = (typeof keyValues === "function") ? keyValues() : keyValues;
                        //        if (keyValues && keyValues.length > 0 && keyValues[0].trim()) {
                        //            var table = exportModelData.ExportRequest.DataMigrationList[0];
                        //            var keys = table.Items.filter(function (f) { return f.IsKey; });
                        //            var filters = table.FilterString;
                        //            var length = Math.min(keyValues.length, keys.length);
                        //            var keyFilters = "";
                        //            var filter = "";
                        //            for (var i = 0; i < length; i++) {
                        //                filter = keys[i].columnName + " = " + keyValues[i];
                        //                keyFilters += (i === 0) ? filter : " AND " + filter;
                        //            }
                        //            if (keyFilters) {
                        //                filters = (filters) ? keyFilters + " AND (" + filters + ")" : keyFilters;
                        //                table.FilterString = filters;
                        //            }
                        //        }
                        //    }
                        //}

                        that._setFilterString();
                        var paramters = {
                            currentPageNumber: (grid) ? grid.dataSource.page() -1 : 0,
                            pageSize: 10,
                            dataMigration: exportModelData.ExportRequest.DataMigrationList[0]
                        };
                        sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "Search", "Get"), paramters, function (successData) {
                            // Map array to list of grid column object    
                            var gridData = [];
                            var length = successData.Items.length;
                            var items = successData.Items;
                            var fieldList = exportModelData.ExportRequest.DataMigrationList[0].Items;
                            for (var i = 0; i < length; i++) {
                                var row = items[i];
                                var len = row.length;
                                var r = {};
                                var c, v, t, d, f;

                                for (var j = 0; j < len; j++) {
                                    c = cols[j];
                                    v = row[j];
                                    t = c.dataType;
                                    f = c.field;
                                    if (v && (t === "Date" || t === "Time")) {
                                        d = kendo.parseDate(v, 'yyyy-MM-dd');
                                        v = (t === "Date") ? kendo.toString(d, "yyyy/MM/dd") : d.toLocaleTimeString();
                                    }
                                    if (c.dataType === "Int" && fieldList[j].PresentationList !== null) {
                                        var list = fieldList[j].PresentationList;
                                        var matchList = list.filter(function (i) { return i.Value === v; });
                                        if (matchList.length > 0) {
                                            v =  matchList[0].Text;
                                        }
                                    }
                                    r[f] = v;
                                }
                                gridData.push(r);
                            }
                            options.success({ data: gridData, totalRecCount: successData.TotalResultsCount });
                            setTimeout(function () {
                                if (gridData.length < 10) {
                                    $("#CriteriaGrid").find(".k-link.k-pager-nav:nth-child(3)").addClass("k-state-disabled");
                                }
                            }, 100);
                        });
                    }
                },
                schema: {
                    total: 'totalRecCount',
                    data: 'data'
                }
            });

            var $preViewGrid = $("#CriteriaGrid");
            var grid = $preViewGrid.kendoGrid({
                dataSource: dataSource,
                columns: cols,
                editable: false,
                navigatable: true,
                selectable: true,
                scrollable: true,
                resizable: true,
                pageable: {
                    input: true,
                    numeric: false,
                    refresh: false
                },
            }).data("kendoGrid");

            return grid;
        },

        _destroyKendoWindow: function () {
            kendoWindow.destroy();
        },

        destroy: function () {
            $.Widget.prototype.destroy.call(this);
        },

        _doExport: function (that) {
            sg.exportHelper.abortPolling = false;
            if (that.options.keys !== $.noop) {
                sg.exportHelper.exportModel.ExportRequest.Keys(that.options.keys.call());
                //resign the DataMigrationList if user passed the keys in
                that._setFilterString();
                sg.exportHelper.exportModel.ExportRequest.DataMigrationList(exportModelData.ExportRequest
                    .DataMigrationList);

            }
            $("#ExportTabstrip").hide();
            $("#btnClose").hide();
            $(".k-window-action").hide();
            $('#btnDownload').hide();
            $('#btnExportGroup').hide();
            $("#exportResult").show();

            var data = { viewModel: ko.mapping.toJS(sg.exportHelper.exportModel) };
            that._exportData(data, true);

            processExportTimer = sg.utls.showProgressBar("#progressBarForExport");

            sg.utls.ajaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "Export"), data, function (result) {
                ko.mapping.fromJS(result.Data.ExportResponse, {}, sg.exportHelper.exportModel.ExportResponse);
                var data = { viewModel: ko.mapping.toJS(sg.exportHelper.exportModel) };
                if (data.viewModel.ExportResponse.Status > 1) {
                    sg.utls.progressBarControl("#progressBarForExport", 100);
                    clearInterval(processExportTimer);
                    sg.exportHelper.showExportResult();
                    $('#btnDownload').show();
                } else {
                    window.sg.utls.recursiveAjaxPost(sg.utls.url.buildUrl("Core", "ExportImport", "Progress"), data, that._progress.bind(that), that._abort.bind(that));
                }
            });
        },

        _exportData: function (data, isExport) {
            var exportData = data.viewModel.ExportRequest.DataMigrationList;
            var tree = $("#exportTreeView").data("kendoTreeView");
            var treeData = tree.dataSource.data();
            var length = treeData.length;

            //synchronize export data from tree view data source
            for (var i = 0; i < length; i++) {
                exportData[i].Print = (treeData[i].checked === undefined) ? isExport : treeData[i].checked;

                var exportItems = exportData[i].Items;
                var treeItems = treeData[i].Items;
                var len = Math.min(exportItems.length, treeItems.length);

                var subNodeLoaded = treeItems.length > 0;
                var parentChecked = exportData[i].Print;

                for (var j = 0; j < len; j++) {
                    exportItems[j].print = subNodeLoaded ? treeItems[j].checked : parentChecked;
                }
            }
        },

        /**
         * This function is used to build DataMigrationList filter string based on keys
         */
        _setFilterString: function() {
            var keyValues = sg.exportHelper.exportKeys;
            if(keyValues) {
                keyValues = (typeof keyValues === "function") ? keyValues() : keyValues;
                if (keyValues && keyValues.length > 0 && keyValues[0].trim()) {
                    var table = exportModelData.ExportRequest.DataMigrationList[0];
                    var keys = table.Items.filter(function (f) { return f.IsKey; });
                    var filters = table.FilterString;
                    var length = Math.min(keyValues.length, keys.length);
                    var keyFilters = "";
                    var filter = "";
                    for (var i = 0; i < length; i++) {
                        filter = keys[i].columnName + " = " + keyValues[i];
                        keyFilters += (i === 0) ? filter : " AND " + filter;
                    }
                    if (keyFilters) {
                        filters = (filters) ? keyFilters + " AND (" + filters + ")" : keyFilters;
                        table.FilterString = filters;
                    }
                }
            }
        },

        _progress: function (result) {
            ko.mapping.fromJS(result.ExportResponse, {}, sg.exportHelper.exportModel.ExportResponse);
            var model = sg.exportHelper.exportModel;

            if (model.ExportResponse.Status() === 2 || model.ExportResponse.Status() === 3) { //Error or Completed
                sg.exportHelper.abortPolling = true;
                clearInterval(processExportTimer);
                sg.utls.progressBarControl("#progressBarForExport", 100);
                sg.exportHelper.showExportResult();
                var fileUrl = sg.utls.url.buildUrl("Core", "ExportImport", "GetExportBlobReference") + "?blobName=" + sg.exportHelper.exportModel.ExportResponse.FileName();
                $('#btnDownload').show();
                $('#btnDownload').attr('href', fileUrl);
            } else {
                if (result.ExportResponse.Results.length > 0) {
                    $("#resultgrid").show();
                } else {
                    $("#resultgrid").hide();
                }
            }
        },

        _abort: function () {
            return sg.exportHelper.abortPolling;
        }
    });
})(jQuery, window, document);
