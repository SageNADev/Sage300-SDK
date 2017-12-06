// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.

"use strict";
var adhocInquiryUIKoExtn = {
    modelExtensions: function(model) {
        model.isDeleteDisable = ko.computed(function () {
            return model.Data.InquiryQueryType() === adhocInquiryUI.adhocQueryType.Template;
        });

        model.LocalizedName = ko.observable(localizedName);

        model.LocalizedFeatureName = ko.observable(localizedFeatureName);

        model.removeFilter = function() {
            model.Data.InquiryFilters.remove(this);
        };

        model.refreshFilterControl = function (element, item) {
            adhocInquiryUI.refreshDropDownList(element, item)
        };
    }
};
