/* Copyright (c) 2021 Sage Software, Inc.  All rights reserved. */

"use strict";
var sg = sg || {};

/*
 * Component for a popup with a text area.
 * Add an empty div to your page with an id, and have knockout binding set up with your model.
 */
sg.textAreaPopup = (function () {
    const _defaultMaxLength = '250';
    const _defaultPopupWidth = 680;
    const _defaultInnerClass = 'p10 w640 h180 max-h535';
    const _defaultOuterClass = 'textarea-group';

    /**
     * @function
     * @name init
     * @description Initializes popup with a textarea bound to a knockout model
     * @param {string} id popup html id attribute
     * @param {string} title popup header title
     * @param {string} fieldName knockout model field name bound to the text area
     * @param {Function} closeCallback callback function when popup is closed
     * @param {string} textAreaId (optional) textarea html id attribute
     * @param {string} textAreaClass (optional) textarea css class
     * @param {string} containerClass (optional) textarea container css class
     * @param {string} maxLength (optional) textarea maxlength
     * @param {int} popupWidth (optional) popup window width
     */
    const init = (id, title, fieldName, closeCallback, textAreaId, textAreaClass, containerClass, maxLength, popupWidth) => {
        const innerClass = textAreaClass === undefined ? _defaultInnerClass : textAreaClass;
        const outerClass = containerClass === undefined ? _defaultOuterClass : containerClass;
        const max = maxLength === undefined ? _defaultMaxLength : maxLength;
        const innerId = textAreaId === undefined ? `${id}_txt${fieldName}` : textAreaId;
        const width = popupWidth === undefined ? _defaultPopupWidth : popupWidth;

        const template =
            `<div class=\"${outerClass}\">
                <textarea id=\"${innerId}\" 
                    maxlength=\"${max}\"
                    class=\"${innerClass}\"
                    data-bind="sagevalue:${fieldName},valueUpdate:'input'"
                ></textarea>
            </div>`;

        $(id).append(template);

        sg.utls.initializeKendoWindowPopupWithWidth(id, title, closeCallback, width);
    }

    /**
     * @function
     * @name showPopUp
     * @description Display the popup
     * @param {string} id popup html id attribute
     * @param {bool} isReadOnly textarea is disabled
     */
    const showPopUp = (id, isReadOnly) => {
        sg.utls.openKendoWindowPopup(id, null);

        if (isReadOnly !== undefined)
            sg.textAreaPopup.readOnly(id, isReadOnly);
    }

    /**
     * @function
     * @name readOnly
     * @description Set the text area disabled
     * @param {string} id popup html id attribute
     * @param {bool} isReadOnly textarea is disabled
     */
    const readOnly = (id, isReadOnly) => {
        $(id).find('textarea').prop('disabled', isReadOnly);
    }

    // Expose module(class) public methods
    return {
        init: init,
        showPopUp: showPopUp,
        readOnly: readOnly
    };
})();
