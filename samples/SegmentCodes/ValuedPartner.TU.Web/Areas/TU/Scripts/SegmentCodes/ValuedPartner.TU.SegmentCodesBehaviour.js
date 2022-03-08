// The MIT License (MIT) 
// Copyright (c) 1994-2022 The Sage Group plc or its licensors.  All rights reserved.
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

//@ts-check

"use strict";

let modelData;
var segmentCodesUI = segmentCodesUI || {};

segmentCodesUI = {
    segmentCodesModel: {},
    computedProperties: ["UIMode"],
    hasKoBindingApplied: false,
    isKendoControlNotInitialised: false,

    /**
     * @name applyBinding
     * @description Apply Knockout bindings
     * @namespace segmentCodesUI
     * @public
     */
    applyBinding: () => {
        segmentCodesUI.segmentCodesModel = ko.mapping.fromJS(SegmentCodesViewModel);
        segmentCodesUI.segments = SegmentCodesViewModel.Segments;
        segmentCodesUI.segmentCodeLength = SegmentCodesViewModel.Segments.map((o) => {
            return o.SegmentLength;
        });
        segmentCodesUI.hasKoBindingApplied = true;
        modelData = segmentCodesUI.segmentCodesModel.Data;
        segmentCodesObservableExtension(segmentCodesUI.segmentCodesModel, sg.utls.OperationMode.NEW);
        segmentCodesUI.segmentCodesModel.isModelDirty = new ko.dirtyFlag(modelData, segmentCodesUI.ignoreIsDirtyProperties);
        ko.applyBindings(segmentCodesUI.segmentCodesModel);
    },

    /**
     * @name initdropDownList
     * @description Initialize the dropdownlist
     * @namespace segmentCodesUI
     * @public
     */
    initDropDownList: () => {
        $("#SegmentNameList").kendoDropDownList({
            change: () => {
                if (sg.viewList.commit("segmentCodesGrid")) {
                    let select = $("#SegmentNameList").data("kendoDropDownList").select();
                    let value = segmentCodesUI.segments[select].Value;
                    sg.viewList.filter("segmentCodesGrid", 'SEGMENT=' + value);
                    sg.viewList.refresh("segmentCodesGrid");
                }
            }
        });

        let dropdownlist = $("#SegmentNameList").data("kendoDropDownList");
        dropdownlist.trigger("change");
    },

    /**
     * @name init
     * @description Primary initialization routine
     * @namespace segmentCodesUI
     * @public
     */
    init: () => {
        // initialize grid(s)
        sg.viewList.init("segmentCodesGrid");

        // hide the edit column button
        $("#btnsegmentCodesGridEditCol").hide();

        segmentCodesUI.initButtons();
        segmentCodesUI.applyBinding();
        segmentCodesUI.initDropDownList();
    },

    /**
     * @name saveSegmentCodes
     * @description Save segment codes
     * @namespace segmentCodesUI
     * @public
     */
    saveSegmentCodes: () => {
        if ($("#frmSegmentCodes").valid()) {
            segmentCodesRepository.post(segmentCodesUISuccess.post);
        }
    },

    /**
     * @name initButtons
     * @description Initialize the page buttons
     * @namespace segmentCodesUI
     * @public
     */
    initButtons: () => {
        // Import/Export Buttons
        sg.exportHelper.setExportEvent("btnOptionExport", "tusegmentcodes", false, $.noop);
        sg.importHelper.setImportEvent("btnOptionImport", "tusegmentcodes", false, $.noop);

        // Save Button
        $("#btnSave").on('click', () => {
            if (sg.viewList.commit("segmentCodesGrid")) {
            	sg.utls.SyncExecute(segmentCodesUI.saveSegmentCodes);
            	sg.viewList.dirty("segmentCodesGrid", false);
            }
        });
    },

    /**
     * @name columnStartEdit
     * @description Customize segment code value input length based on dropdown list
     * @namespace segmentCodesUI
     * @public
     * 
     * @param {any} record: the select row data
     * @param {object} evt: call back event object
     * @param {string} field: field name
     * @param {object} editor: the editor
     */
    columnStartEdit: (record, evt, field, editor) => {
        let value = $("#SegmentNameList").data("kendoDropDownList").select();
        let length = 2;
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
     * @name post
     * @description post result event handler
     * @namespace segmentCodesUI
     * @public 
     * 
     * @param {object} result JSON result payload
     */
    post: (result) => {
        if (result.UserMessage.IsSuccess) {
            segmentCodesUISuccess.displayResult(result, sg.utls.OperationMode.SAVE);
        }
        sg.utls.showMessage(result);
    },
		
	/**
     * @name displayResult
     * @description
     * @namespace segmentCodesUI
     * @public
	 *
	 * @param {object} result JSON result payload
	 * @param {number} uiMode UI Mode specifier
	 */
	displayResult: (jsonResult, uiMode) => {
        sg.viewList.refresh("segmentCodesGrid");
    },
};

// Initial Entry
$(() => {
    segmentCodesUI.init();
});
