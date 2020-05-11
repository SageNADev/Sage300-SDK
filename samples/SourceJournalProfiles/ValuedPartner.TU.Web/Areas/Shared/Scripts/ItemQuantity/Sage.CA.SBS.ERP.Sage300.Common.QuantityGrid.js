/* Copyright (c) 1994-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var quantity = quantity || {};

/**
 * Holds the grid's dataset column names.
 */
var quantityColumnName = {
    Detail:"",
    ExpectedShipArrivalDate: "",
    Location: "",
    Quantity: "",
    DocumentNumber: "",
    Date: "",
    CustVendorNumber: "",
    CustVendorName: "",
};

var quantityGridScreenNameEnum = {
    InvoiceEntry: "InvoiceEntry",
    ShipmentEntry: "ShipmentEntry",
}

/**
 * Functionality for configuring the grid columns.
 */
var gridQuantityColConfig = {

    /**
     * @summary Custom Kendo grid cell editor function, which renders the cell both non-editable and non-selectable. 
     * 
     * @callback gridQuantityColConfig~noEditor 
     *
     * @param {Object} container The containing grid cell.
     * @param {Object} options   Not used.
     */ 
    noEditor: function (container, options) {
        sg.utls.kndoUI.nonEditable($('#' + quantityUIGrid.gridId).data("kendoGrid"), container);
    },

    /**
     * @summary Creates a Kendo column definition entry. 
     * 
     * @param {String}  fieldName         The name of the dataset field.
     * @param {Boolean} isHidden          True if the column is hidden, otherwise false.
     * @param {String}  caption           The text that is displayed in the column header cell. If not set the field is used.
     * @param {String}  columnClass       HTML attributes of the table cell.
     * @param {String}  templateExp       The Kendo template expression which renders the column content.
     * @param {String}  headerTemplateExp The template which renders the column header content. By default the value of the title column option is displayed in the column header cell.
     * @param {String}  editor            Provides a way to specify a custom editing UI for the column. Use the container parameter to create the editing UI. 
     */
    getColumn: function (fieldName, isHidden, caption, columnClass, templateExp, headerTemplateExp, editor) {
        var column = {
            field: fieldName,
            hidden: isHidden,
            title: caption,
            attributes: isHidden ? { sg_Customizable: false } : { "class": columnClass },
            headerAttributes: { "class": columnClass },
            template: templateExp,
            headerTemplate: headerTemplateExp,
            editor: editor,
        }
        return column;
    },
};

/**
 * The Item Quantity Grid client-side settings.
 */
var quantityUIGrid =
{
    /**
     * @summary Initialize the viewmodel.
     * 
     * @param params The grid configuration parameters.
     */
    init: function (params) {
        quantityUIGrid.gridId = params.gridId,
        quantityUIGrid.modelData = params.modelData,
        quantityUIGrid.currentRowItemNumber = params.currentRowItemNumber(),
        quantityUIGrid.currentLocation = params.allLocation ? null : params.currentLocation(),
        quantityUIGrid.currentDocumentType = params.currentDocumentType,
        quantityUIGrid.btnEditColumnsId = params.btnEditColumnsId,
        quantityUIGrid.btnPOReceipt = params.btnPOReceipt,
        quantityUIGrid.preferencesTypeId = params.preferencesTypeId,
        quantityUIGrid.pageSize = params.pageSize,
        quantityUIGrid.pageNumber = params.pageNumber,
        quantityUIGrid.formattedDecimal = params.formattedDecimal,
        quantityUIGrid.ScreenName = params.ScreenName,
        quantityUIGrid.initButton();
    },
    /**
     * Initialize this instance's associated buttons.
     */
    initButton: function () {

        var grid = $('#' + quantityUIGrid.gridId).data("kendoGrid");
        quantityUIGrid.defaultColumns = $.extend(true, {}, grid.columns);
        quantityUIGrid.defaultColumns.length = grid.columns.length;

        $('#' + quantityUIGrid.btnEditColumnsId).on('click', function () {
            GridPreferences.initialize('#' + quantityUIGrid.gridId, quantityUIGrid.preferencesTypeId, $(this), quantityUIGrid.defaultColumns);
        });

        $('#' + quantityUIGrid.btnPOReceipt).on('click', function () {
            var gridData = sg.utls.kndoUI.getSelectedRowData(grid);

            var guid = sg.utls.guid();
            var url = sg.utls.url.buildUrl("PO", "PendingReceiptsInquiry", "Index") + "?guid=" + guid + "&itemNumber=" + quantityUIGrid.currentRowItemNumber + "&location=" + gridData.DocumentLocation;
            sg.utls.iFrameHelper.openWindow(guid, "", url, 600);            
        });
    },
    /**
     * @summary Retrieves the grid's dataset key ('DetailLineUniquifier' if not found).
     * 
     * @returns {string} the grid's dataset key ('DetailLineUniquifier' if not found).
     */
    getDatasetKey: function () {
        var key = 'DetailLineUniquifier';

        // Get the dataset record key.
        var bind = $('#' + quantityUIGrid.gridId).data().bind;
        if (bind !== undefined) {
            var i;
            var bindValues = bind.split(',');
            for (i = 0; i < bindValues.length; i++) {
                var pair = bindValues[i].split(':');
                if (pair[0].trim() === 'key') {
                    key = pair[1].trim().replace(/'/g, '');
                    break;
                }
            }
        }

        return key;
    },
    /**
     * @summary Formats a specified numeric field value (null for a formatted zero).
     * 
     * @param fieldValue The field value to format.
     *
     * @returns The field value, formatted as a decimal. 
     */
    getFormattedValue: function (fieldValue) {
        if (fieldValue != null)
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(!isNaN(parseFloat(fieldValue)) ? parseFloat(fieldValue) : 0, quantityUIGrid.formattedDecimal);
        else {
            fieldValue = sg.utls.kndoUI.getFormattedDecimalNumber(0, quantityUIGrid.formattedDecimal);
        }
        return fieldValue;
    },    
    /**
     * @summary Formats a specified date field value.
     * 
     * @param fieldValue The field value to format.
     *
     * @returns If field value non-null, field value formatted as a date, otherwise null. 
     */
    getFormattedDate: function (fieldValue) {
        if (fieldValue != null) {
            return sg.utls.kndoUI.getFormattedDate(fieldValue);
        }
        return fieldValue;
    },
    /**
     * @summary Creates the HTML for a QuantityGrid row div element.
     * 
     * @param {Number} container The row index of the row to be created.
     *
     * @returns The HTML for a QuantityGrid row div element.
     */
    getSettings: function (container) {
        var div = $(quantityPanelField.drillDown);
        var button = div.find('input[type=button]')[0];
        button.id = button.id + container;
        $(button).attr('tag', container);
        $(button)[0].classList.add("btnDetail");
        return div.html();
    },
    /**
     * @summary Sets each row's drilldown button click event handler.
     *
     * @param e The event parameters.
     */
    registerSettingsEvent: function (e) {
        $('.btnDrillDown').unbind('click')
            .click(
                /**
                 * @summary Handler for QuantityGrid drilldown button click event.
                 * 
                 * @param e The event parameters.
       `          */
                function (e) {
                    var rowIndex = parseInt($(this).attr('tag'));
                    var itemGrid = $("#" + quantityUIGrid.gridId).data("kendoGrid");
                    var selectedData = sg.utls.kndoUI.getRowByKey(itemGrid.dataSource.data(), quantityUIGrid.getDatasetKey(), rowIndex);

                    var guid = sg.utls.guid();

                    var url = "#"
                    if (quantityUIGrid.currentDocumentType === 1 || quantityUIGrid.currentDocumentType === 2) {
                        url = sg.utls.url.buildUrl("OE", "OrderEntry", "Index") + "?guid=" + guid + "&id=" + encodeURIComponent(selectedData.DocumentNumber) + "&isEditable=false";
                    } else {
                        url = sg.utls.url.buildUrl("PO", "PurchaseOrderEntry", "Index") + "?guid=" + guid + "&id=" + encodeURIComponent(selectedData.DocumentNumber) + "&disableAll=true";
                    }

                    sg.utls.iFrameHelper.openWindow(guid, "", url, 600);
                });
    },
    gridId: "",
    btnEditColumnsId: "",
    modelData: null,
    currentRowItemNumber: null,
    currentLocation: null,
    currentDocumentType: null,
    preferencesTypeId: null,
    defaultColumns: null,
    allLocation: false,
    pageSize: null,
    pageNumber:null,

    /**
     * @summary Gets a customized set of configuration parameters for the grid.
     *
     * @param area       The module ID (used as an Ajax URL segment).
     * @param controller The MVC controller-name (used as an Ajax URL segment).
     * @param action     The action to perform on the controller (used as an Ajax URL segment).
     * @param gridId     The ID for this grid. 
     * @param modelName  The model name of the grid's dataset. 
     * @param columnList The dataset column-names.
     * @param dataKey    The dataset key-name.
     * 
     * @returns A customized set of configuration parameters for the grid.
     */
    quantityGridConfig: function (area, controller, action, gridId, modelName, columnList) {
        quantityUIGrid.gridId = gridId;
        
        // Copy the dataset's column names to their respective fields in quantityColumnName.
        for (var prop in columnList) {
            quantityColumnName[prop] = columnList[prop];
        }

        return {
            autoBind: false,
            pageSize: sg.utls.gridPageSize,
            scrollable: true,
            reorderable: sg.utls.reorderable,
            // Enable grid cell tabbing for Safari browser.
            navigatable: true, 
            resizable: true,
            selectable: true,
            // reorderable: sg.utls.reorderable,
            isServerPaging: true,
            // Param will contain null for Get and the data that needs to be passed to the server for Create.
            param: null,
            // URL to get the data from the server. 
            pageUrl: sg.utls.url.buildUrl(area, controller, action),
            pageable: {
                input: true,
                numeric: false
                },
            /**
             * @summary Gets a grid-page's construction parameters.
             * 
             * @returns The grid-page's construction parameters.
             */
            getParam: function () {
                var grid = $('#' + gridId).data("kendoGrid");

                var parameters = {
                    pageNumber: grid.dataSource.page() - 1,
                    pageSize: sg.utls.gridPageSize,
                    model: ko.mapping.toJS(quantityUIGrid.modelData),
                    itemNumber: quantityUIGrid.currentRowItemNumber,
                    location: quantityUIGrid.currentLocation,
                    documentType: quantityUIGrid.currentDocumentType,
                };
                return parameters;
            },
            /**
             * @summary Callback for dataset-retrieval AJAX server call: prepares the grid's UI and Knockout viewmodel.
             *
             * @param successData The server call's JSON payload.
             */
            buildGridData: function (successData) {
                var gridData = null;

                if (successData == null) {
                    return;
                }

                if ((successData.UserMessage && successData.UserMessage.IsSuccess) || successData.Items !== undefined) {
                    gridData = [];

                    var documentNumberData = (successData.Data !== undefined) ? successData.Data[modelName] : successData;
                    ko.mapping.fromJS(documentNumberData, {}, quantityUIGrid.modelData[modelName]);
                    gridData.data = documentNumberData.Items
                    gridData.totalResultsCount = documentNumberData.TotalResultsCount;

                } else {
                    sg.utls.showMessage(successData);
                    if (quantityUIGrid.ScreenName == "OrderEntry") {
                        $("#quantityOnSoGridWindow").data("kendoWindow").close();
                        $("#quantityOnPoGridWindow").data("kendoWindow").close();
                        $("#quantityCommittedGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantitySoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityPoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityCommittedGridWindow").data("kendoWindow").close();
                    }
                    else if (quantityUIGrid.ScreenName == "CreditDebitNoteEntry") {
                        $("#creditdebitquantityOnSoGridWindow").data("kendoWindow").close();
                        $("#creditdebitquantityOnPoGridWindow").data("kendoWindow").close();
                        $("#creditdebitquantityCommittedGridWindow").data("kendoWindow").close();
                        $("#creditdebitAllLocQuantitySoGridWindow").data("kendoWindow").close();
                        $("#creditdebitAllLocQuantityPoGridWindow").data("kendoWindow").close();
                        $("#creditdebitAllLocQuantityCommittedGridWindow").data("kendoWindow").close();
                    }
                    else if (quantityUIGrid.ScreenName === quantityGridScreenNameEnum.InvoiceEntry) {
                        //This method need to improve in futrue. Just keep consistent as OE Order Entry as temporary solution
                        $("#quantityOnSoGridWindow").data("kendoWindow").close();
                        $("#quantityOnPoGridWindow").data("kendoWindow").close();
                        $("#quantityCommittedGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantitySoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityPoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityCommittedGridWindow").data("kendoWindow").close();
                    }
                    else if (quantityUIGrid.ScreenName === quantityGridScreenNameEnum.ShipmentEntry) {
                        //This method need to improve in futrue. Just keep consistent as OE Order Entry as temporary solution
                        $("#quantityOnSoGridWindow").data("kendoWindow").close();
                        $("#quantityOnPoGridWindow").data("kendoWindow").close();
                        $("#quantityCommittedGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantitySoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityPoGridWindow").data("kendoWindow").close();
                        $("#AllLocQuantityCommittedGridWindow").data("kendoWindow").close();
                    }
                }
               return gridData;
            },
            /**
             * @summary Callback to set a newly-added line as editable after the data is bound to the grid.
             *
             * @param e The event parameters.
             */
            afterDataBind: function (e) {
                quantityUIGrid.registerSettingsEvent(e);
            },
            /**
             * @summary Persists the column order after a reorder.
             * @callback quantityUIGrid~columnReorder
             * @param e The event parameters.
             */ 
            columnReorder: function (e) {
                GridPreferencesHelper.saveColumnOrder(e, '#' + quantityUIGrid.gridId, quantityUIGrid.preferencesTypeId);
            },
            /**
             * @summary {Array} columns The grid columns configuration.
             */
            columns: [
                 gridQuantityColConfig.getColumn(quantityColumnName.Detail, false, quantityGridResources.DetailTitle, "w80", '#= quantityUIGrid.getSettings(' + quantityUIGrid.getDatasetKey() + ') #', null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.ExpectedShipArrivalDate, false, "", "w220", '#= quantityUIGrid.getFormattedDate(ExpectedShippingArrivalDate) #', null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.Location, false, quantityGridResources.Location, "w220", null, null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.Quantity, false, quantityGridResources.Quantity, "w220", '#= quantityUIGrid.getFormattedValue(QtyInStockingUnit) #', null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.DocumentNumber, false, "", "w220", null, null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.Date, false, quantityGridResources.Date, "w220", '#= quantityUIGrid.getFormattedDate(DocumentDate) #', null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.CustVendorNumber, false,"", "w220", null, null, gridQuantityColConfig.noEditor),
                 gridQuantityColConfig.getColumn(quantityColumnName.CustVendorName, false, "", "w220", null, null, gridQuantityColConfig.noEditor),
            ],
        }
    }
};
