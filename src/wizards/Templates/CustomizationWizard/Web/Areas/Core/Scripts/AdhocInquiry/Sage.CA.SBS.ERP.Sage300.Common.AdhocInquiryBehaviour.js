// Copyright (c) 2017 Sage Software, Inc.  All rights reserved.

"use strict";
var adhocInquiryUI = adhocInquiryUI || {};
adhocInquiryUI = {
    adhocQueryType: {
        Template: 0,
        Public: 1,
        Private: 2
    },

    inquiryModel: {},

    inquiryFieldControlList: {},

    init: function (model, fieldsControlList) {
        adhocInquiryUI.inquiryModel = ko.mapping.fromJS(model);
        adhocInquiryUI.inquiryFieldControlList = fieldsControlList;
        adhocInquiryUIKoExtn.modelExtensions(adhocInquiryUI.inquiryModel);
        ko.applyBindings(adhocInquiryUI.inquiryModel);
        adhocInquiryUI.initButton();
        adhocInquiryUI.initMessageEventListener();
        adhocInquiryGridUI.initGrid();
    },

    initButton: function() {
        $("#btnSaveQuery").click(function () {
            adhocInquiryFilters.validateFilter(adhocInquiryUI.showSaveInquiryQueryPanel);
        });

        $("#btnDelete").click(function() {
            adhocInquiryUI.deleteQuery();
        });

        $("#btnApply").click(function () {
            adhocInquiryFilters.validateFilter(adhocInquiryGridUI.refreshGridData);
        });

        $("#btnAddFilter").click(function () {
            adhocInquiryUI.inquiryModel.Data.InquiryFilters.push(ko.mapping.fromJS(NewFilterObject));
        });
    },

    refreshDropDownList: function (element, item) {
        var filterDisplayIndex = 1;
        var filterDivContainer = element[1];
        var previousFilter = filterDivContainer.previousElementSibling;
        if (previousFilter) { //Get the index of the previous element and increment
            var previousIndex = previousFilter.id.split('_')[1];
            filterDisplayIndex = sg.utls.toInt(previousIndex, 10) + 1;
        }

        //The ids of the html elements have templates like "filterDdl_", "btnDeleteFilter_"
        //We will append the index at the end to make the ids unique
        filterDivContainer.id += filterDisplayIndex;
        filterDivContainer.getElementsByTagName("button")[0].id += filterDisplayIndex;
        filterDivContainer.getElementsByTagName("select")[0].id += filterDisplayIndex;

        var filterDdl = $(filterDivContainer.getElementsByTagName("select"));
        filterDdl.kendoDropDownList({
            optionLabel: AdhocInquiryResources.Select
        });
        filterDdl.data("kendoDropDownList").optionLabel.hide();

        function changeFilterControlGroup (e) {
            //Display the optionLable on initialization, otherwiserender the filter control div from html in inquiryFieldControlList.
            if (!item.BaseFieldName() && typeof e[0] !== "undefined") {
                this.select(0);
            } else {
                var field = $.grep(adhocInquiryUI.inquiryFieldControlList, function (obj) {
                    return obj.inquiryField.Name === e.sender.value();
                })[0];

                var html = jQuery.validator.format(field.fieldHtml, filterDisplayIndex);
                var filterValueControl = filterDivContainer.children[2];
                $(filterValueControl).html(html);
                ko.applyBindings(adhocInquiryUI.inquiryModel, filterValueControl);
                if (typeof e[0] === "undefined") { //This cleanup has to done if it is not initialize and after the html is rendered
                    item.Value(null);
                    item.SqlOperator(0);
                }
                adhocInquiryFilters.initRenderedAttributes(e.sender.selectedIndex - 1, filterDisplayIndex, e.sender.value(), item);
                item.BaseFieldName(item.Field.field());
            }
        }

        ////Attach on change event to the displayed fields dropdown list, which displays the corresponding html stored in that object list of fields.
        ////Also Trigger the "change", so it is called atleast once.
        filterDdl.data("kendoDropDownList").bind("change", changeFilterControlGroup).trigger('change', [{fromInit:true}]);
    },

    initMessageEventListener: function () {
        window.top.addEventListener("message", adhocInquiryUI.receiveWindowMessage, false);
    },
    
    getIFrameWindowId: function () {
        return window.frameElement ? window.frameElement.id : "";
    },

    receiveWindowMessage: function (e) {
        if (e.data.Id === "saveQuery" && e.data.Data.iFrameId === adhocInquiryUI.getIFrameWindowId()) {
            var data = { "query": e.data.Data };
            adhocInquiryRepository.CheckQueryExist(data, onSuccess.CheckQueryExist);
        }
    },

    showSaveInquiryQueryPanel: function () {
        var data = {
            'showSaveQueryPanel': true, 'iFrameId': adhocInquiryUI.getIFrameWindowId(),
            'title': adhocInquiryUI.inquiryModel.LocalizedFeatureName(),
            'inquiryFeatureTypeSecurity': adhocInquiryUI.inquiryModel.Data.InquiryFeatureType.Security()
        };
        window.top.postMessage(data, "*");
    },

    deleteQuery: function () {
        var message = jQuery.validator.format(AdhocInquiryResources.DeleteConfirmMessage, AdhocInquiryResources.Query);
        sg.utls.showKendoConfirmationDialog(function () {
            var model = ko.mapping.toJS(adhocInquiryUI.inquiryModel);
            var data = { "InquiryQuery": model.Data };
            adhocInquiryRepository.DeleteQuery(data, onSuccess.DeleteQuery);
        }, null, message, "");
    },

    apply: function (options) {
        //TODO: this need to change once we know how get database from all the controls either by
        // reading value or data binding
        var model = ko.mapping.toJS(adhocInquiryUI.inquiryModel);
        var data = { "InquiryQuery": model.Data };

        adhocInquiryRepository.ApplyQuery(data, onSuccess.ApplyQuery(options));
    },
};

var onSuccess = onSuccess || {};
onSuccess = {
    SaveQuery: function(result) {
        if (result) {
            sg.utls.showMessage(result);
            if (result.UserMessage && result.UserMessage.IsSuccess) {
                localizedName = null;
                adhocInquiryUI.inquiryModel.LocalizedName(result.Data.Name);
                
                var data = { 'hideSaveQueryPanel': true };
                window.top.postMessage(data, "*");
            }
        }
    },

    DeleteQuery: function(result) {
        if (result && result.UserMessage && result.UserMessage.IsSuccess) {
            var iframeId = "dv" + adhocInquiryUI.getIFrameWindowId();
            $("span", parent.top.document.getElementById(iframeId))[1].click();
        } else {
            sg.utls.showMessage(result);
        }
    },
    CheckQueryExist: function (result) {
        if (result.IsNewLine) {
            onSuccess.SaveAfterCheckQuery(result);
        } else {
            var message = jQuery.validator.format(AdhocInquiryResources.SaveReplaceMessage,
                                                  AdhocInquiryResources.QueryTypeString[result.InquiryQuery.InquiryQueryType],
                                                  result.InquiryQuery.Name);
            sg.utls.showKendoConfirmationDialog(function () {
                onSuccess.SaveAfterCheckQuery(result);
            }, null, message, "");
        }
    },

    SaveAfterCheckQuery: function (result) {
        adhocInquiryUI.inquiryModel.Data.Name(result.InquiryQuery.Name);
        adhocInquiryUI.inquiryModel.Data.Description(result.InquiryQuery.Description);
        adhocInquiryUI.inquiryModel.Data.DateModified(result.InquiryQuery.DateModified);
        adhocInquiryUI.inquiryModel.Data.InquiryQueryType(result.InquiryQuery.InquiryQueryType);
        
        // before save, remove localized of the original name
        adhocInquiryUI.inquiryModel.Data.ResourceKey("");

        adhocInquiryRepository.SaveQuery({ "InquiryQuery": ko.mapping.toJS(adhocInquiryUI.inquiryModel).Data }, onSuccess.SaveQuery);
    },

    ApplyQuery: function (options) {
        return function (result) {
            if (result.Errors && result.Errors.length > 0) {
                sg.utls.showMessageInEnumerableResponse(result);
            }
            options.success(result);
        }
    }
};

$(function () {
    if (AdhocInquiryViewModel) {
        adhocInquiryUI.init(AdhocInquiryViewModel, InquiryFieldControlList);
    }
})
