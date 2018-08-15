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
