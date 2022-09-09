/* Copyright (c) 2022 Sage Software, Inc.  All rights reserved. */

"use strict";

var ICManufacturersItemUtils = ICManufacturersItemUtils || {};

/*
 * Sets up a pair of from/to textboxes to handle querying of item number using the manufacturer's item number.
 * On page load, call init().
 */
ICManufacturersItemUtils = {

    // ids of controls
    fromTxtId: '', fromManufacturerTxtId: '', fromManufacturerBtnId: '', toTxtId: '', toManufacturerTxtId: '', toManufacturerBtnId: '',

    // promise from get item number ajax call
    promise: null,

    /**
    * @function
    * @name init
    * @description Initializes the manufacturers item controls
    * @namespace ICManufacturersItemUtils
    * @public
    *
    * @param {string} fromTxtId id of the from textbox
    * @param {string} fromManufacturerTxtId id of the from helper textbox 
    * @param {string} fromManufacturerBtnId id of the from helper finder button
    * @param {string} toTxtId id of the to textbox
    * @param {string} toManufacturerTxtId id of the to helper textbox
    * @param {string} toManufacturerBtnId id of the to helper finder button
    */
    init: function (fromTxtId, fromManufacturerTxtId, fromManufacturerBtnId, toTxtId, toManufacturerTxtId, toManufacturerBtnId) {
        // store control ids
        this.fromTxtId = fromTxtId;
        this.fromManufacturerTxtId = fromManufacturerTxtId;
        this.fromManufacturerBtnId = fromManufacturerBtnId;
        this.toTxtId = toTxtId;
        this.toManufacturerTxtId = toManufacturerTxtId;
        this.toManufacturerBtnId = toManufacturerBtnId;

        this.initManufacturerItemFinders();
        this.addChangeEvent();
    },

    /**
    * @function
    * @name initManufacturerItemFinders
    * @description Initializes the manufacturers item finders
    * @namespace ICManufacturersItemUtils
    * @public
    */
    initManufacturerItemFinders: () => {
        const { fromTxtId, fromManufacturerTxtId, fromManufacturerBtnId, toTxtId, toManufacturerTxtId, toManufacturerBtnId } = ICManufacturersItemUtils;
        const manufacturerItemFromProp = () => {
            const property = sg.utls.deepCopy(sg.viewFinderProperties.IC.ManufacturerItemNumber);
            let item = $(`#${fromTxtId}`).val() ? $(`#${fromTxtId}`).val().toUpperCase() : '';
            property.initKeyValues = [item, item];
            property.filter = $.validator.format(property.filterTemplate, item);
            return property;
        };
        const manufacturerItemToProp = () => {
            const property = sg.utls.deepCopy(sg.viewFinderProperties.IC.ManufacturerItemNumber);
            let item = $(`#${toTxtId}`).val() ? $(`#${toTxtId}`).val().toUpperCase() : '';
            property.initKeyValues = [item, item];
            property.filter = $.validator.format(property.filterTemplate, item);
            return property;
        };

        sg.viewFinderHelper.setViewFinderEx(fromManufacturerBtnId, fromManufacturerTxtId, manufacturerItemFromProp,
            (successResult) => {
                $(`#${fromTxtId}`).val(successResult.FMTITEMNO);
                $(`#${fromTxtId}`).attr('unformattedItemNumber', successResult.ITEMNO);
                $(`#${fromTxtId}`).focus();
                ICManufacturersItemUtils.promise = Promise.resolve();
                $(`#${fromTxtId}`).trigger('change');
            },
            () => {
                $(`#${fromTxtId}`).focus();
                ICManufacturersItemUtils.promise = Promise.resolve();
            }
        );
        sg.viewFinderHelper.setViewFinderEx(toManufacturerBtnId, toManufacturerTxtId, manufacturerItemToProp,
            (successResult) => {
                $(`#${toTxtId}`).val(successResult.FMTITEMNO);
                $(`#${toTxtId}`).attr('unformattedItemNumber', successResult.ITEMNO);
                $(`#${toTxtId}`).focus();
                ICManufacturersItemUtils.promise = Promise.resolve();
                $(`#${toTxtId}`).trigger('change');
            },
            () => {
                $(`#${toTxtId}`).focus();
                ICManufacturersItemUtils.promise = Promise.resolve();
            }
        );
    },

    /**
    * @function
    * @name addChangeEvent
    * @description add change handlers to display formatted item number and save unformatted number in an attribute.
    * If manufacturer item number is entered, change to formatted item number.
    * If there are multiple item numbers associated with the manufacturer item number, open the manufacturer item finder popup.
    * If the manufacturer item number is the same as a different item number, show a warning popup.
    * @namespace ICManufacturersItemUtils
    * @public
    */
    addChangeEvent: () => {
        const { fromTxtId, fromManufacturerBtnId, toTxtId, toManufacturerBtnId } = ICManufacturersItemUtils;
        const url = sg.utls.url.buildUrl("IC", "Item", "GetItemType");
        $(`#${fromTxtId},#${toTxtId}`).on('change', function (e) {
            // clear unformatted number
            $(`#${e.target.id}`).removeAttr('unformattedItemNumber');

            const data = {
                itemId: e.target.value
            }
            ICManufacturersItemUtils.promise = sg.utls.ajaxPostWithPromise(url, data).then((result) => {
                if (result) {
                    if (result.NoOfManufacturerItemsMoreThan1) {
                        $(`#${e.target.id}`).attr('unformattedItemNumber', e.target.value);
                        if (e.target.id === fromTxtId) {
                            $(`#${fromManufacturerBtnId}`).click();
                        }
                        else {
                            $(`#${toManufacturerBtnId}`).click();
                        }

                        // open finder and do not proceed with callback
                        return Promise.reject();
                    }
                    else if (result.Item && result.Item.ItemNumber) {
                        $(`#${e.target.id}`).val(result.Item.ItemNumber);
                        $(`#${e.target.id}`).attr('unformattedItemNumber', result.Item.UnformattedItemNumber);
                    }
                    else {
                        $(`#${e.target.id}`).attr('unformattedItemNumber', e.target.value);
                    }
                }
            }).catch((result) => {
                if (result) {
                    if (result.Item && result.Item.ItemNumber) {
                        $(`#${e.target.id}`).val(result.Item.ItemNumber);
                        $(`#${e.target.id}`).attr('unformattedItemNumber', result.Item.UnformattedItemNumber);
                    }
                    if (result.UserMessage) {
                        sg.utls.showMessage(result, () => {
                            $(`#${e.target.id}`).focus();
                            ICManufacturersItemUtils.promise = Promise.resolve();
                        });
                    }
                }

                // do not proceed with callback
                return Promise.reject();
            });
        });
    }
}
