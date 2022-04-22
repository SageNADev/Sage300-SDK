/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

'use strict';

var ICSegmentCodeUtils = ICSegmentCodeUtils || {};

ICSegmentCodeUtils = {

    /**
    * @function
    * @name init
    * @description Initializes the segment code controls
    * @namespace ICSegmentCodeUtils
    * @public
    * @param {string} dropdownId id of the segment dropdown
    * @param {string} fromTxtId id of the from segment textbox
    * @param {string} fromBtnId id of the from segment textbox finder button
    * @param {string} toTxtId id of the to segment textbox
    * @param {string} toBtnId id of the from segment textbox finder button
    * @param {string} toBtnId id of the from segment textbox finder button
    * @param {object} segmentChangedEvent callback to function fired when change event happens
    */
    init: (dropdownId, fromTxtId, fromBtnId, toTxtId, toBtnId, segmentChangedEvent) => {
        // init finders
        const segmentFromProps = () => {
            let property = sg.utls.deepCopy(sg.viewFinderProperties.IC.SegmentCode);
            const segmentNumber = $(`#${dropdownId}`).data('kendoDropDownList').value();
            const segmentCode = $(`#${fromTxtId}`).val();
            property.initKeyValues = [segmentNumber, segmentCode];
            property.filter = $.validator.format(property['filterTemplate'], segmentNumber);
            return property;
        };
        const segmentToProps = () => {
            let property = sg.utls.deepCopy(sg.viewFinderProperties.IC.SegmentCode);
            const segmentNumber = $(`#${dropdownId}`).data('kendoDropDownList').value();
            const segmentCode = $(`#${toTxtId}`).val();
            property.initKeyValues = [segmentNumber, segmentCode];
            property.filter = $.validator.format(property['filterTemplate'], segmentNumber);
            return property;
        };
        sg.viewFinderHelper.setViewFinder(fromBtnId, fromTxtId, segmentFromProps);
        sg.viewFinderHelper.setViewFinder(toBtnId, toTxtId, segmentToProps);

        // init segment dropdown
        let specificSegment = [];
        let specificSegmentLen = [];

        // set input attributes
        const setInputAttr = (len) => {
            const defaultChar = 'Z';
            $(`#${fromTxtId}`).attr('maxlength', len);
            $(`#${toTxtId}`).attr('maxlength', len);
            $(`#${fromTxtId}`).val('');
            $(`#${toTxtId}`).val(`${defaultChar.repeat(len)}`);
        }

        // get segment data
        const url = sg.utls.url.buildUrl('IC', 'SegmentCode', 'GetSegmentNumbers');
        sg.utls.ajaxPostWithPromise(url, { 'property': 'SegmentName' }).then((ret) => {
            specificSegment = ret.Item1;
            specificSegmentLen = ret.Item2;

            declarativeReportUtls.resetDropdownListDataSource(dropdownId, specificSegment, false);
            setInputAttr(specificSegmentLen[0]);

            // Set the initial finder labels
            segmentChangedEvent();
        });

        // change selected segment
        const segmentDropdown = $(`#${dropdownId}`).data('kendoDropDownList');
        segmentDropdown.bind('change', function (e) {
            const len = specificSegmentLen[`${this.selectedIndex}`];
            setInputAttr(len);

            segmentChangedEvent();
        });
    }
}
