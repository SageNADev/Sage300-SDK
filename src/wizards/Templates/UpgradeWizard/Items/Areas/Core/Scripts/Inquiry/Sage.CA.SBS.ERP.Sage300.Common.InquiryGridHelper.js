/* Copyright (c) 2016-2017 Sage Software, Inc.  All rights reserved. */

"use strict";

var ARDocumentTypes = { Invoice: 1, DebitNote: 2, CreditNote: 3, Interest: 4, UnappliedCash: 5, Prepayment: 10, Receipt: 11, Adjustment: 14, Refund: 19 };
var ARTranTransactionTypes = { WriteOffPosted: 80, AdjustmentPosted: 81 };
var InquiryTypes = { Documents: "1", Receipts: "2", Refunds: "3", Adjustments: "4", DocumentTransaction: "100", DocumentTransactionDetails: "101" };
var InvoiceType = { NotApplicable: 0, Item: 1, Summary: 2 };
var JobRelated = { No: 0, Yes: 1 };
var HasRetainage = { No: 0, Yes: 1 };

/* Inquiry Grid Helper */
var InquiryGridHelper = InquiryGridHelper || {};
InquiryGridHelper = {
    appendDrillDownLink: function(grid, inquiryType, isOEActive) {
        var that = this;
        this.popupHeight = 1440;
        this.popupWidth = 1100;
        this.inquiryForm = $("#frmInquiry");

        this.appendArDocumentClickEvents = function() {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnDocumentNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnDocumentNumber', this.btnDocumentNumberClickHandler);

            $(document).off('click', 'tbody > tr > td > div > span > input.btnOrderNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnOrderNumber',
                isOEActive === 'True' ? this.btnOrderNumberClickHandler : this.btnOENotActiveHandler);

            $(document).off('click', 'tbody > tr > td > div > span > input.btnShipmentNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnShipmentNumber',
                isOEActive === 'True' ? this.btnShipmentNumberClickHandler : this.btnOENotActiveHandler);
        };

        this.appendArReceiptClickEvents = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnDocumentNo');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnDocumentNo', this.btnDocumentNumberClickHandler);
        };

        this.appendArRefundClickEvents = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnDocumentNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnDocumentNumber', this.btnDocumentNumberClickHandler);
        };

        this.appendArAdjustmentClickEvents = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnReferenceDocumentNo');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnReferenceDocumentNo', this.btnDocumentNumberClickHandler);
        };

        this.getSelectedRowData = function(obj) {
            var row = obj.closest("tr");
            grid.select(row);
            return grid.dataItem(row);
        };

        this.btnDocumentNumberClickHandler = function() {
            that.injectModalToInquiryForm();

            var selectedRowData = that.getSelectedRowData($(this));
            switch (selectedRowData.DocumentType) {
                case ARDocumentTypes.Invoice:
                case ARDocumentTypes.CreditNote:
                case ARDocumentTypes.DebitNote:
                case ARDocumentTypes.Interest:
                    that.drillDownToARInvoice(selectedRowData);
                    break;

                case ARDocumentTypes.Receipt:
                case ARDocumentTypes.UnappliedCash:
                case ARDocumentTypes.Prepayment:
                    that.drillDownToARReceipt(selectedRowData);
                    break;

                case ARDocumentTypes.Refund:
                    that.drillDownToARRefund(selectedRowData);
                    break;
                case ARDocumentTypes.Adjustment:
                    // When transaction type is adjustment and receipt no. is not empty, it means this adjustment
                    // is generated from Receipt - so open receipt entry instead
                    if (selectedRowData.TransactionType == ARTranTransactionTypes.AdjustmentPosted &&
                        selectedRowData.CheckReceiptNo) {
                        that.drillDownToARReceipt(selectedRowData);
                    } else {
                        that.drillDownToARAdjustment(selectedRowData);
                    }
                    break;
            }
        };

        this.btnOrderNumberClickHandler = function () {
            that.injectModalToInquiryForm();

            var selectedRowData = that.getSelectedRowData($(this));
            sg.utls.ajaxPost(sg.utls.url.buildUrl("OE", "OrderEntry", "GetById"), { 'id': selectedRowData.OrderNumber, 'isInquireMode': true }, function (result) {
                var url = sg.utls.url.buildUrl("OE", "OrderEntry", "Index") + "?id=" + selectedRowData.OrderNumber + "&isEditable=false";
                that.openDrillDownPopup(result, url);
            });
        };

        this.btnShipmentNumberClickHandler = function () {
            that.injectModalToInquiryForm();

            var selectedRowData = that.getSelectedRowData($(this));
            sg.utls.ajaxPost(sg.utls.url.buildUrl("OE", "ShipmentEntry", "GetById"), { 'id': selectedRowData.ShipmentNumber, 'isInquireMode': true }, function (result) {
                var url = sg.utls.url.buildUrl("OE", "ShipmentEntry", "Index") + "?id=" + selectedRowData.ShipmentNumber + "&isEditable=false";
                that.openDrillDownPopup(result, url);
            });
        };

        this.btnOENotActiveHandler = function () {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, InquiryResources.NotAuthorizedMessage);
        }

        this.drillDownToARInvoice = function (rowData) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("AR", "InvoiceEntry", "GetByIds"), { 'batchNumber': rowData.BatchNumber, 'entryNumber': rowData.EntryNumber }, function (result) {
                var url = sg.utls.url.buildUrl("AR", "InvoiceEntry", "Index") + "?batchNumber=" + rowData.BatchNumber + "&entryNumber=" + rowData.EntryNumber + "&actionType=Inquiry";
                that.openDrillDownPopup(result, url);
            });
        }

        this.drillDownToARReceipt = function (rowData) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("AR", "ReceiptEntry", "GetByIds"), { 'batchNumber': rowData.BatchNumber, 'entryNumber': rowData.EntryNumber }, function (result) {
                var url = sg.utls.url.buildUrl("AR", "ReceiptEntry", "Index") + "?batchNumber=" + rowData.BatchNumber + "&entryNumber=" + rowData.EntryNumber + "&actionType=Inquiry";
                that.openDrillDownPopup(result, url);
            });
        }

        this.drillDownToARRefund = function (rowData) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("AR", "RefundEntry", "GetByIds"), { 'batchNumber': rowData.BatchNumber, 'entryNumber': rowData.EntryNumber }, function (result) {
                var url = sg.utls.url.buildUrl("AR", "RefundEntry", "Index") + "?batchNumber=" + rowData.BatchNumber + "&entryNumber=" + rowData.EntryNumber + "&actionType=Inquiry";
                that.openDrillDownPopup(result, url);
            });
        }

        this.drillDownToARAdjustment = function (rowData) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("AR", "AdjustmentEntry", "Get"), { 'batchNumber': rowData.BatchNumber, 'entryNumber': rowData.EntryNumber }, function (result) {
                var url = sg.utls.url.buildUrl("AR", "AdjustmentEntry", "Index") + "?batchNumber=" + rowData.BatchNumber + "&entryNumber=" + rowData.EntryNumber + "&actionType=Inquiry";
                that.openDrillDownPopup(result, url);
            });
        }

        this.openDrillDownPopup = function (result, url) {
            if (result.UserMessage && result.UserMessage.IsSuccess === true) {
                sg.utls.iFrameHelper.openWindow(sg.utls.guid(), "", url, that.popupHeight, that.popupWidth);
            } else {
                //If not found, it means the batch has been cleared, so we display a more proper message
                //This is not the best way to display a proper message; however, the existing messages 
                //are expected at other places, so we cannot handle in backend. 
                var tempStr = InquiryResources.NotFoundMessage.replace('{0}', '');
                var notFoundMsgPrefix = $.trim(tempStr.substring(0, tempStr.length - 1));
                if (result.UserMessage.Errors[0].Message.indexOf(notFoundMsgPrefix) !== -1) {
                    result.UserMessage.Errors[0].Message = InquiryResources.BatchHasBeenClearedMessage;
                }
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, result.UserMessage.Errors[0].Message);
            }
            that.removeModalFromInquiryForm();
        }

        this.injectModalToInquiryForm = function() {
            that.inquiryForm.append("<div id='transparentOverlay' class='k-overlay k-overlay-transparent' ></div>");
        }

        this.removeModalFromInquiryForm = function() {
            $('#transparentOverlay').remove();
        }

        this.init = function() {
            switch (inquiryType) {
            case InquiryTypes.Documents:
            case InquiryTypes.DocumentTransaction:
                this.appendArDocumentClickEvents();
                break;
            case InquiryTypes.Receipts:
                this.appendArReceiptClickEvents();
                break;
            case InquiryTypes.Refunds:
                this.appendArRefundClickEvents();
                break;
            case InquiryTypes.Adjustments:
                this.appendArAdjustmentClickEvents();
                break;
            default:
                break;
            }
        }

        this.init();
    }
}
