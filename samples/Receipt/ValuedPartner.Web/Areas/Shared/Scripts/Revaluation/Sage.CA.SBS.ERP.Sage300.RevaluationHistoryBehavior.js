/* Copyright (c) 1994-2015 Sage Software, Inc.  All rights reserved. */

"use strict";

var revaluationHistoryUI = revaluationHistoryUI || {};

revaluationHistoryUI = {
    ModelData: "",
    init: function () {

        $('#revaluationHistoryPageTitle').html(screenSetup.screenTitle);

        revaluationHistoryUI.initFinder();
        revaluationHistoryUI.initTextBox();
        revaluationHistoryUI.loadHistory(revaluationHistoryViewModel);
    },

    bindData: function (result) {
        revaluationHistoryUI.ModelData = ko.mapping.fromJS(result);
        ko.applyBindings(revaluationHistoryUI.ModelData);
        sg.utls.focus("txtCurrencyCode");
    },

    revaluationHistoryFilter: function () {
        var revaluationHistoryFilters = [[]];
        var filter = sg.finderHelper.createFilter("CurrencyCode", sg.finderOperator.Equal, revaluationHistoryUI.ModelData.CurrencyCode());

        revaluationHistoryFilters[0][0] = filter;
        return revaluationHistoryFilters;
    },

    initFinder: function () {
        var title = jQuery.validator.format(revaluationHistoryResources.FinderTitle, revaluationHistoryResources.CurrencyCode);
        
        sg.finderHelper.setFinder(
            "btnFinder",
            sg.finder.CurrencyCode,
            revaluationHistoryUI.finderSuccess,
            $.noop(),
            title,
            sg.finderHelper.createDefaultFunction(
            "txtCurrencyCode", "CurrencyCodeId",
            sg.finderOperator.StartsWith), null, true, 550);
    },

    finderSuccess: function (result) {
        if (result !== null && result.CurrencyCodeId !== null) {
            revaluationHistoryUI.ModelData.CurrencyCode(result.CurrencyCodeId);
            var grid = $('#revaluationHistoryGrid').data("kendoGrid");
            grid.dataSource.read();
        } else {
            sg.utls.showMessageInfo(sg.utls.msgType.ERROR, jQuery.validator.format(revaluationHistoryResources.RecordDoesNotExist, revaluationHistoryResources.CurrencyCode, $("#txtCurrencyCode").val()));
        }
    },

    initTextBox: function () {
        $("#txtCurrencyCode").bind('change', function (e) {
            if (e.target.value !== "") {
                sg.delayOnChange("btnFinder", $("#txtCurrencyCode"), function () {
                    var value = e.target.value.trim().toUpperCase();
                    var data = { 'id': value };
                    sg.utls.ajaxPost(sg.utls.url.buildUrl("CS", "CurrencyCode", "Get"), data, function (result) {
                        if (result !== null && result.Data !== null && result.Data.CurrencyCodeId !== null) {
                            revaluationHistoryUI.ModelData.CurrencyCode(result.Data.CurrencyCodeId);
                            var grid = $('#revaluationHistoryGrid').data("kendoGrid");
                            grid.dataSource.page(1);
                        } else {
                            var currencyCode = $("#txtCurrencyCode").val().toUpperCase();
                            revaluationHistoryUI.ModelData.CurrencyCode(null);
                            revaluationHistoryUI.ModelData.CurrencyCodeDescription(null);
                            var grid = $('#revaluationHistoryGrid').data("kendoGrid");
                            grid.dataSource.data([]);
                            var errorMsg = $.validator.format(revaluationHistoryResources.RecordNotFoundMessage, revaluationHistoryResources.CurrencyCode, currencyCode);
                            window.sg.utls.showMessageInfo(window.sg.utls.msgType.ERROR, errorMsg);
                            sg.utls.focus("txtCurrencyCode");
                        }
                    });
                });
            } else {
                revaluationHistoryUI.ModelData.CurrencyCode(null);
                var grid = $('#revaluationHistoryGrid').data("kendoGrid");
                grid.dataSource.data([]);
                revaluationHistoryUI.ModelData.CurrencyCodeDescription(null);
                sg.utls.focus("txtCurrencyCode");
            }

        });
    },

    ConvertPostingSequenceNo: function (postingSequenceNo) {
        if (postingSequenceNo === 0) {
            return "";
        }
        else {
            return postingSequenceNo
        }
    },
    ConvertRevaluationMethod: function (revaluationMethod) {
        var match = $.grep(revaluationHistoryViewModel.RevaluationMethods, function (value, i) {
            return value.Value === revaluationMethod;
        });

        if (match !== null && match.length > 0) {
            return match[0].Text;
        }
        else {
            return revaluationMethod.toString();
        }
    },
    loadHistory: function (result) {
        if (result.UserMessage.IsSuccess) {
            revaluationHistoryUI.bindData(result);
        } else {
            sg.utls.showMessage(result);
        }
    }
}

var revaluationHistoryGrid = {
    revaluationHistoryConfig: {
        pageSize: sg.utls.gridPageSize,
        navigatable: true,
        pageable: {
            input: true,
            numeric: false,
            refresh: true
        },
        scrollable: true,
        reorderable: sg.utls.reorderable,
        schema: {
            model: {
                fields: {
                    RevaluationDate: { type: "date" },
                    RateDate: {type: "date" }
                }
            }
        },

        resizable: true,
        selectable: 'row',
        sortable: false,
        isServerPaging: true,
        param: null,

        getParam: function () {
            var grid = $('#revaluationHistoryGrid').data("kendoGrid");
            var pageNumber = grid.dataSource.page();
            var pageSize = grid.dataSource.pageSize();
            var model = ko.mapping.toJS(revaluationHistoryUI.ModelData);

            var parameters = {
                model: model,
                pageNumber: pageNumber - 1,
                pageSize: pageSize,
                filters: revaluationHistoryUI.revaluationHistoryFilter(),
                index: -1,
            };

            if (model.CurrencyCode === null) {
                parameters.stopPropagation = true;
                grid.dataSource.data([]);
                sg.utls.focus("txtCurrencyCode");
            }

            return parameters;
        },

        // URL to get the data from the server. 
        pageUrl: sg.utls.url.buildUrl(screenSetup.moduleName, "RevaluationHistory", "Get"),

        // Call back function when Get is successfull. In this, the data for the grid and the total results count are to be set along with updating knockout
        buildGridData: function (successData) {
            var gridData = null;
            var grid = $('#revaluationHistoryGrid').data("kendoGrid");

            if (successData !== null && successData.UserMessage.IsSuccess) {

                gridData = [];
                gridData.data = successData.DataList;

                ko.mapping.fromJS(successData, {}, revaluationHistoryUI.ModelData);

                gridData.totalResultsCount = successData.TotalResultsCount;

            } else {
                ko.mapping.fromJS(successData, {}, revaluationHistoryUI.ModelData);
                gridData = [];
                gridData.data = successData.DataList;
                sg.utls.showMessage(successData);
            }
            
            return gridData;
        },

        columns: [
            {
                field: "RevaluationDate",
                title: revaluationHistoryResources.RevaluationDate,
                width: 130,
                template: '#= kendo.toString(RevaluationDate, "M/dd/yyyy" ) #'
            },
            {
                field: "RateType",
                title: revaluationHistoryResources.RateType,
                width: 90
            },
            {
                field: "RateDate",
                title: revaluationHistoryResources.RateDate,
                width: 110,
                template: '#= kendo.toString(RateDate, "M/dd/yyyy" ) #'
            },
            {
                field: "ExchangeRate",
                title: revaluationHistoryResources.ExchangeRate,
                width: 120
            },
            {
                field: "PostingSequenceNo",
                title: revaluationHistoryResources.PostingSequenceNo,
                width: 150,
                template: '#= revaluationHistoryUI.ConvertPostingSequenceNo(PostingSequenceNo) #'
            },
            {
                field: "RevaluationMethod",
                title: revaluationHistoryResources.RevaluationMethod,
                width: 200,
                template: '#= revaluationHistoryUI.ConvertRevaluationMethod(RevaluationMethod) #'
            }
        ]
    }
}

function close_iFramePopup() {
    var data = { CurrencyCode: revaluationHistoryUI.ModelData.CurrencyCode() };
    var responseData = sg.utls.iFrameHelper.buildData("CurrencyCode", data);
    sg.utls.iFrameHelper.postDataToParent(responseData);
};

$(function () {
    revaluationHistoryUI.init();
})
