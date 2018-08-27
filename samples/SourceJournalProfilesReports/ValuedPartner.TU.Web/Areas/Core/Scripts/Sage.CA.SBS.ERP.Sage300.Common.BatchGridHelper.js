/* Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved. */

"use strict";

/* Batch Grid Helper */
var BatchGridHelper = BatchGridHelper || {};
BatchGridHelper = {
    appendJournalLink: function(grid, postUrl, errorUrl, postMsg, errorMsg, errorMsgPosted, postedStatus) {
        var that = this;
        this.popupHeight = 1440;
        this.popupWidth = 1100;

        this.appendPostJournalClickEvent = function() {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnPostingSequence');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnPostingSequence', this.btnPostingJournalClickHandler);
        };

        this.appendJournalErrorClickEvent = function () {
            $(document).off('click', 'tbody > tr > td > div > span > input.btnPostingErrors');
            $(document).on('click', 'tbody > tr > td > div > span > input.btnPostingErrors', this.btnPostingErrorClickHandler);
        };

        this.getSelectedRowData = function(obj) {
            var row = obj.closest("tr");
            grid.select(row);
            return grid.dataItem(row);
        };

        this.btnPostingJournalClickHandler = function() {
            var selectedRowData = that.getSelectedRowData($(this));
            var sequence = that.getSequenceNumber(selectedRowData);

            if (!sequence) {
                var errorMessage = postMsg;
                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
                return;
            }

            sg.utls.iFrameHelper.openWindow(sg.utls.guid(), "", postUrl + sequence, that.popupHeight, that.popupWidth);
        };

        this.btnPostingErrorClickHandler = function() {
            var selectedRowData = that.getSelectedRowData($(this));
            var errorCount = that.getErrorCount(selectedRowData);

            if (!errorCount) {
                var errorMessage, itemStatus;
                if (selectedRowData.Status !== undefined) { //Used in GL
                    itemStatus = selectedRowData.Status;
                } else if (selectedRowData.BatchStatus !== undefined) { //Used in AR-Refund, AR-Adjustment
                    itemStatus = selectedRowData.BatchStatus;
                }

                if (itemStatus === postedStatus) {
                    errorMessage = errorMsgPosted;
                } else {
                    errorMessage = errorMsg;
                }

                sg.utls.showMessageInfo(sg.utls.msgType.ERROR, errorMessage);
                return;
            }

            sg.utls.iFrameHelper.openWindow(sg.utls.guid(), "", errorUrl + that.getSequenceNumber(selectedRowData), that.popupHeight, that.popupWidth);
        };

        this.getSequenceNumber = function (rowData) {
            if (rowData.PostingSequence !== undefined) { //Used in GL
                return rowData.PostingSequence;
            } else if (rowData.PostingSequenceNo !== undefined) { //Used in AR-Refund, AR-Adjustment, AR-Receipt
                return rowData.PostingSequenceNo;
            }
        };

        this.getErrorCount = function (rowData) {
            if (rowData.NoofErrors !== undefined) { //Used in GL
                return rowData.NoofErrors;
            } else if (rowData.NumberOfErrors !== undefined) { //Used in AR-Refund
                return rowData.NumberOfErrors;
            } else if (rowData.NumberofErrors !== undefined) { //Used in AR-Adjustment, AR-Receipt
                return rowData.NumberofErrors;
            }
        };

        this.appendPostJournalClickEvent(grid, postMsg);
        this.appendJournalErrorClickEvent(grid, errorMsg, errorMsgPosted, postedStatus);
    },

    journalPostingColumnTemplate: function(value) {
        return '<div class="pencil-wrapper"><span class="pencil-txt">' + value +
            '</span><span class="pencil-icon"><input type="button" class="icon edit-field btnPostingSequence"/></span></div>';
    },

    journalErrorColumnTemplate: function(value) {
        return '<div class="pencil-wrapper"><span class="pencil-txt">' + value +
            '</span><span class="pencil-icon"><input type="button" class="icon edit-field btnPostingErrors"/></span></div>';
    }
}
