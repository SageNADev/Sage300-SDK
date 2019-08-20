
// The MIT License (MIT) 
// Copyright (c) 1994-2019 The Sage Group plc or its licensors.  All rights reserved.
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

var modelData;
var segmentCodesUI = segmentCodesUI || {};

segmentCodesUI = {
    segmentCodesModel: {},
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,


    /**
     * Apply binding
	 *
	 * @method applyBinding
	 */
    applyBinding: function () {
        segmentCodesUI.segmentCodesModel = ko.mapping.fromJS(SegmentCodesViewModel);
        segmentCodesUI.segments = SegmentCodesViewModel.Segments;
        segmentCodesUI.segmentCodeLength = SegmentCodesViewModel.Segments.map(function (o) {
            return o.SegmentLength;
        });
        segmentCodesUI.hasKoBindingApplied = true;
        modelData = segmentCodesUI.segmentCodesModel.Data;
        segmentCodesObservableExtension(segmentCodesUI.segmentCodesModel, sg.utls.OperationMode.NEW);
        segmentCodesUI.segmentCodesModel.isModelDirty = new ko.dirtyFlag(modelData, segmentCodesUI.ignoreIsDirtyProperties);
        ko.applyBindings(segmentCodesUI.segmentCodesModel);
    },

    // Init Dropdowns here
    initDropDownList: function () {
        $("#SegmentNameList").kendoDropDownList({
            change: function () {
                if (sg.viewList.commit("segmentCodesGrid")) {
                    var select = $("#SegmentNameList").data("kendoDropDownList").select();
                    var value = segmentCodesUI.segments[select].Value;
                    sg.viewList.filter("segmentCodesGrid", 'SEGMENT=' + value);
                    sg.viewList.refresh("segmentCodesGrid");
                }
            }
        });
    },

	/**
     * Initialization
	 *
	 * @method init
	 */
    init: function () {
        // initialize grid(s)
        sg.viewList.init("segmentCodesGrid");

        // hide the edit column button
        $("#btnsegmentCodesGridEditCol").hide();

        segmentCodesUI.initButtons();

        segmentCodesUI.applyBinding();

        segmentCodesUI.initDropDownList();
    },

	/**
     * Save
	 *
	 * @method saveSegmentCodes
	 */
    saveSegmentCodes: function () {
        if ($("#frmSegmentCodes").valid()) {
            segmentCodesRepository.post(segmentCodesUISuccess.post);
        }
    },

	/**
     * Initialize the Buttons
	 *
	 * @method initButtons
	 */
    initButtons: function () {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusegmentcodes", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusegmentcodes", false, $.noop);

        // Save Button
        $("#btnSave").on('click', function () {
            if (sg.viewList.commit("segmentCodesGrid")) {
            	sg.utls.SyncExecute(segmentCodesUI.saveSegmentCodes);
            	sg.viewList.dirty("segmentCodesGrid", false);
            }
        });
    },

    /**
     * Grid customize function: After create new reocrd, update the key and segment value based on dropdown list select value
     * @param {any} record: select row data
     */
    gridAfterCreate: function (record) {
        var segment = record.SEGMENT;
        var value = $("#SegmentNameList").data("kendoDropDownList").select() + 1;
        var key = record.KendoGridAccpacViewPrimaryKey.replace(segment, value);
        record.set("SEGMENT", value);

        var grid = $("#segmentCodesGrid").data("kendoGrid");
        var ds = grid.dataSource;
        var row = ds.data().filter(function (r) { return r.SEGVAL === ""; })[0];
        row.KendoGridAccpacViewPrimaryKey = key;
        row.id = key;
    },

    /**
     * Grid customize function: After grid record value changed, update the key value based on dropdown list select value
     * @param {any} record: select row data
     */

    gridchanged: function (record, fieldName) {
        if (fieldName === "SEGVAL") {
            var grid = $("#segmentCodesGrid").data("kendoGrid");
            var segval = record.SEGVAL;
            var data = grid.dataSource.data();
            var row = data.filter(function (r) { return r.SEGVAL === segval; })[0];
            var oldKey = row.KendoGridAccpacViewPrimaryKey;
            var newKey = oldKey.substring(0, oldKey.length - segval.length - 1) + segval;
            row.KendoGridAccpacViewPrimaryKey = newKey;
            row.id = newKey;
        }
    },

    /**
     * Column customize function: After record value changed, update the key value based on dropdown list select value
     * @param {any} record: select row data
     */

    columnChanged: function (record, event, fieldName) {
        if (fieldName === "SEGVAL") {
            var grid = $("#segmentCodesGrid").data("kendoGrid");
            var segval = record.SEGVAL;
            var data = grid.dataSource.data();
            var row = data.filter(function (r) { return r.SEGVAL === segval; })[0];
            var oldKey = row.KendoGridAccpacViewPrimaryKey;
            var newKey = oldKey.substring(0, oldKey.length - segval.length - 1) + segval;
            row.KendoGridAccpacViewPrimaryKey = newKey;
            row.id = newKey;
        }
    },

    /**
     * Customize segment code value input length based on dropdown list
     * @param {any} record: select row data
     */

    columnStartEdit: function (record, event, field, editor) {
        var value = $("#SegmentNameList").data("kendoDropDownList").select();
        var length = 2;
        switch (value) {
            case 0:
                length = 2;
                break;
            case 1:
                length = 3;
                break;
            case 2:
                length = 1;
                break;
            case 3:
                length = 5;
                break;
            case 4:
                length = 4;
                break;
            case 5:
                length = 20;
                break;
        }
        editor.prop('maxLength', length);
    }
};

// Callbacks
var segmentCodesUISuccess = {
	/**
     * Update
	 *
	 * @method update
	 * @param jsonResult
	 */
    post: function (jsonResult) {
        if (jsonResult.UserMessage.IsSuccess) {
            segmentCodesUISuccess.displayResult(jsonResult, sg.utls.OperationMode.SAVE);
        }
        sg.utls.showMessage(jsonResult);
    },
		
	/**
     * Display Result
	 *
	 * @method displayResult
	 * @param jsonResult
	 * @param uiMode
	 */
	displayResult: function (jsonResult, uiMode) {
        sg.viewList.refresh("segmentCodesGrid");
    },
};

// Initial Entry
$(function () {
    segmentCodesUI.init();
});
