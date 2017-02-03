/* Copyright (c) 2016 Sage Software, Inc.  All rights reserved. */

"use strict";

var ARDocumentTypes = { Invoice: 1, DebitNote: 2, CreditNote: 3, Interest: 4, UnappliedCash: 5, Prepayment: 10, Receipt: 11, Refund: 19, MiscReceipt: 20 };

/* Inquiry Grid Helper */
var InquiryGridHelper = InquiryGridHelper || {};
InquiryGridHelper = {
    appendDrillDownLink: function(grid, isOEActive) {
        var that = this;
        this.popupHeight = 1440;
        this.popupWidth = 1100;
        this.inquiryForm = $("#frmInquiry");

        this.appendDocumentNumberClickEvent = function() {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnDocumentNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnDocumentNumber', this.btnDocumentNumberClickHandler);
        };

        this.appendOrderNumberClickEvent = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnOrderNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnOrderNumber',
                isOEActive === 'True' ? this.btnOrderNumberClickHandler : this.btnOENotActiveHandler);
        };

        this.appendShipmentNumberClickEvent = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnShipmentNumber');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnShipmentNumber',
                isOEActive === 'True' ? this.btnShipmentNumberClickHandler : this.btnOENotActiveHandler);
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
                case ARDocumentTypes.MiscReceipt:
                    that.drillDownToARReceipt(selectedRowData);
                    break;

                case ARDocumentTypes.Refund:
                    that.drillDownToARRefund(selectedRowData);
                    break;
            }
        };

        this.btnOrderNumberClickHandler = function () {
            that.injectModalToInquiryForm();

            var selectedRowData = that.getSelectedRowData($(this));
            sg.utls.ajaxPost(sg.utls.url.buildUrl("OE", "OrderEntry", "GetById"), { 'id': selectedRowData.OrderNumber }, function (result) {
                var url = sg.utls.url.buildUrl("OE", "OrderEntry", "Index") + "?id=" + selectedRowData.OrderNumber + "&isEditable=false";
                that.openDrillDownPopup(result, url);
            });
        };

        this.btnOENotActiveHandler = function () {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, InquiryResources.NotAuthorizedMessage);
        }

        this.btnShipmentNumberClickHandler = function () {
            that.injectModalToInquiryForm();

            var selectedRowData = that.getSelectedRowData($(this));
            sg.utls.ajaxPost(sg.utls.url.buildUrl("OE", "ShipmentEntry", "GetById"), { 'id': selectedRowData.ShipmentNumber }, function (result) {
                var url = sg.utls.url.buildUrl("OE", "ShipmentEntry", "Index") + "?id=" + selectedRowData.ShipmentNumber + "&isEditable=false";
                that.openDrillDownPopup(result, url);
            });
        };

        this.drillDownToARInvoice = function (rowData) {
            sg.utls.ajaxPost(sg.utls.url.buildUrl("AR", "InvoiceEntry", "GetInvoice"), { 'batchNumber': rowData.BatchNumber, 'entryNumber': rowData.EntryNumber }, function (result) {
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

        this.appendDocumentNumberClickEvent(grid);
        this.appendOrderNumberClickEvent(grid);
        this.appendShipmentNumberClickEvent(grid);
    }
}
