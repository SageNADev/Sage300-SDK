/* Copyright (c) 1994-2014 Sage Software, Inc.  All rights reserved. */

var sg = sg || {};
sg.ic = sg.ic || {};
sg.ic.utls = sg.ic.utls || {};

$.extend(sg.ic.utls, {

    getItemType: function (itemNumber, callback) {
        var data = { 'itemId': itemNumber };
        sg.utls.ajaxPost(sg.utls.url.buildUrl("IC", "Item", "GetItemType"), data, callback);
    },

    setItemTypeResponse: function (itemTypeResult, manufacturerFinderControlId, callbackToPopulateItem) {
        if (itemTypeResult) {

            if (itemTypeResult.NoOfManufacturerItemsMoreThan1) {

                //open finder for manufacturer items
                $(manufacturerFinderControlId).trigger("click");
                return;
            }
            if (itemTypeResult.IsItemNumber && itemTypeResult.IsManufacturerNumber) {

                //display warning message 
                sg.utls.showMessage(itemTypeResult); 
            }
            if (itemTypeResult.Item != null) {

                //populate item
                callbackToPopulateItem(itemTypeResult.Item);
            } 
        }
    } 
});